/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using eFormCore;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace eFormSDK.Integration.Base.CoreTests;

/// <summary>
/// Covers Core.GetFileFromS3Storage and both Core.PutFileToS3Storage overloads.
///
/// These methods had no coverage at all, and the gap was expensive: a tenant configured with
/// another tenant's credentials produced 760 consecutive HTTP 500s reading attachments, because
/// the read path threw "Sequence contains no elements" from inside its own AmazonS3Exception
/// handler and destroyed the 403 that explained everything. The write path swallowed the matching
/// 403 outright, so 2398 database rows were committed against objects that were never stored.
///
/// The S3 client is substituted rather than pointed at MinIO. A substitute is the only way to
/// produce a specific status code, error code and request id on demand, which is exactly what
/// these tests assert survives.
///
/// Core holds the S3 client in a static field, so this fixture mutates process wide state. It is
/// safe only because it is the only fixture in this assembly that touches S3. A second one would
/// race with it, and both would then need to be marked NonParallelizable.
///
/// Knowingly not covered: the arm of GetFileFromS3Storage where recovery succeeds and the read is
/// retried. Reaching it needs DownloadUploadedData to return true, which wants an HTTP 200, an
/// ImageMagick pass and matching field value, case and unit rows. The isRetry terminal that bounds
/// that recursion is covered directly instead.
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class CoreTestS3Storage : DbTestFixture
{
    private Core sut;
    private IAmazonS3 s3Client;
    private CapturingLogWriter logWriter;

    private const string CustomerNo = "123";
    private const string BucketName = "test-bucket";

    public override async Task DoSetup()
    {
        #region Setup SettingsTableContent

        DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
        SqlController sql = new SqlController(dbContextHelper);
        await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
        await sql.SettingUpdate(Settings.firstRunDone, "true");
        await sql.SettingUpdate(Settings.knownSitesDone, "true");

        // customerNo and s3Enabled are read into fields during StartSqlOnly, so they have to be
        // in place before the core starts. s3Enabled also gates the file path upload overload.
        await sql.SettingUpdate(Settings.customerNo, CustomerNo);
        await sql.SettingUpdate(Settings.s3Enabled, "true");
        await sql.SettingUpdate(Settings.s3BucketName, BucketName);

        #endregion

        sut = new Core();
        // Core.Log is assigned with ??=, so a writer set before startup survives and lets the
        // tests assert on what an operator would actually see.
        logWriter = new CapturingLogWriter();
        sut.Log = new Log(logWriter);
        await sut.StartSqlOnly(ConnectionString);

        // Replaces whatever client StartSqlOnly built from the placeholder credentials. Nothing
        // in these tests reaches the network.
        s3Client = Substitute.For<IAmazonS3>();
        Core.SetS3ClientForTesting(s3Client);
    }

    [TearDown]
    public void ResetS3Client()
    {
        // The field behind this is static, so leaving a substitute in place would leak into every
        // other fixture in the run.
        Core.SetS3ClientForTesting(null);
        // Disposing a substitute only records the call; clearing the static field above is what
        // actually matters. NUnit1032 requires the Dispose regardless.
        s3Client?.Dispose();
        s3Client = null;
    }

    #region get

    [Test]
    public async Task Core_S3Storage_GetFileFromS3Storage_DoesReturnTheStoredObject()
    {
        // Arrange
        byte[] content = Encoding.UTF8.GetBytes("stored bytes");
        GetObjectResponse expected = new GetObjectResponse
        {
            ResponseStream = new MemoryStream(content),
            HttpStatusCode = HttpStatusCode.OK
        };
        s3Client.GetObjectAsync(Arg.Any<GetObjectRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(expected));

        // Act
        GetObjectResponse actual = await sut.GetFileFromS3Storage("stored.jpg");

        // Assert
        Assert.That(actual, Is.SameAs(expected));
    }

    [Test]
    public async Task Core_S3Storage_GetFileFromS3Storage_DoesRequestKeyPrefixedWithCustomerNumber()
    {
        // Arrange
        GetObjectRequest captured = null;
        s3Client.GetObjectAsync(Arg.Do<GetObjectRequest>(request => captured = request),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new GetObjectResponse { ResponseStream = new MemoryStream() }));

        // Act
        await sut.GetFileFromS3Storage("prefixed.jpg");

        // Assert
        Assert.That(captured, Is.Not.Null);
        Assert.That(captured.BucketName, Is.EqualTo(BucketName));
        Assert.That(captured.Key, Is.EqualTo($"{CustomerNo}/prefixed.jpg"));
    }

    [Test]
    public void Core_S3Storage_GetFileFromS3Storage_DoesRethrowOriginalExceptionWhenNoUploadedDataRowExists()
    {
        // Arrange
        // A file with no uploaded_data row is unrecoverable, and is what every plugin owned file
        // looks like. This is the shape that used to surface as InvalidOperationException.
        // Note this locks the contract of the branch, not any single line of it: recovery is
        // wrapped in a catch-all, so a lookup that throws is converted to a reason rather than
        // escaping. What must never regress is that the caller still receives the S3 error.
        AmazonS3Exception expected = AccessDenied();
        s3Client.GetObjectAsync(Arg.Any<GetObjectRequest>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(expected);

        // Act
        AsyncTestDelegate act = async () => await sut.GetFileFromS3Storage("no-such-row.jpg");
        AmazonS3Exception actual = Assert.ThrowsAsync<AmazonS3Exception>(act);

        // Assert
        Assert.That(actual, Is.SameAs(expected));
        Assert.That(actual.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        Assert.That(actual.ErrorCode, Is.EqualTo("AccessDenied"));
        Assert.That(actual.RequestId, Is.EqualTo("REQUEST-ID-1"));
    }

    [Test]
    public async Task Core_S3Storage_GetFileFromS3Storage_DoesRethrowOriginalExceptionWhenDuplicateUploadedDataRowsExist()
    {
        // Arrange
        // uploaded_data.file_name carries no uniqueness constraint, so duplicates are possible.
        // Whatever the lookup does with them, the caller must still receive the S3 error rather
        // than "Sequence contains more than one element".
        const string fileName = "duplicated.jpg";
        await AddUploadedData(fileName);
        await AddUploadedData(fileName);

        AmazonS3Exception expected = AccessDenied();
        s3Client.GetObjectAsync(Arg.Any<GetObjectRequest>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(expected);

        // Act
        AsyncTestDelegate act = async () => await sut.GetFileFromS3Storage(fileName);
        AmazonS3Exception actual = Assert.ThrowsAsync<AmazonS3Exception>(act);

        // Assert
        Assert.That(actual, Is.SameAs(expected));
        Assert.That(actual.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));

        // A lookup that cannot tolerate duplicates would be caught by the recovery catch-all and
        // reported as "recovering it raised ...". Reaching the re-download instead is what tells
        // us the duplicates were handled rather than swallowed.
        Assert.That(logWriter.AllText(), Does.Contain("re-downloading uploaded data"));
    }

    [Test]
    public async Task Core_S3Storage_GetFileFromS3Storage_DoesRethrowWithoutRecoveringWhenAlreadyRetrying()
    {
        // Arrange
        // The retry pass must not attempt recovery again. Without this bound the read path could
        // recurse, and the incident was 760 consecutive failures.
        AmazonS3Exception expected = AccessDenied();
        s3Client.GetObjectAsync(Arg.Any<GetObjectRequest>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(expected);

        // Act
        AsyncTestDelegate act = async () => await sut.GetFileFromS3Storage("already-retried.jpg", true);
        AmazonS3Exception actual = Assert.ThrowsAsync<AmazonS3Exception>(act);

        // Assert
        Assert.That(actual, Is.SameAs(expected));
        await s3Client.Received(1)
            .GetObjectAsync(Arg.Any<GetObjectRequest>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public void Core_S3Storage_GetFileFromS3Storage_DoesPreserveANotFoundDistinctFromAccessDenied()
    {
        // Arrange
        // Telling 404 from 403 is the whole point of preserving the original exception: one means
        // the object is genuinely gone, the other means the credentials cannot see it.
        AmazonS3Exception expected = new AmazonS3Exception("The specified key does not exist.",
            ErrorType.Sender, "NoSuchKey", "REQUEST-ID-2", HttpStatusCode.NotFound);
        s3Client.GetObjectAsync(Arg.Any<GetObjectRequest>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(expected);

        // Act
        AsyncTestDelegate act = async () => await sut.GetFileFromS3Storage("missing.jpg");
        AmazonS3Exception actual = Assert.ThrowsAsync<AmazonS3Exception>(act);

        // Assert
        Assert.That(actual, Is.SameAs(expected));
        Assert.That(actual.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(actual.ErrorCode, Is.EqualTo("NoSuchKey"));
    }

    [Test]
    public void Core_S3Storage_GetFileFromS3Storage_DoesLogTheStatusCodeErrorCodeAndRequestId()
    {
        // Arrange
        // AmazonS3Exception carries these as properties rather than in Message, so unless they are
        // rendered explicitly they never reach the log. Their absence is what turned a plain 403
        // into an untraceable outage. Asserted as values, not as a sentence, so rewording is free.
        s3Client.GetObjectAsync(Arg.Any<GetObjectRequest>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(AccessDenied());

        // Act
        AsyncTestDelegate act = async () => await sut.GetFileFromS3Storage("logged.jpg");
        Assert.ThrowsAsync<AmazonS3Exception>(act);

        // Assert
        string logged = logWriter.AllText();
        Assert.That(logged, Does.Contain("403"));
        Assert.That(logged, Does.Contain("AccessDenied"));
        Assert.That(logged, Does.Contain("REQUEST-ID-1"));
        Assert.That(logged, Does.Contain($"{CustomerNo}/logged.jpg"));
        Assert.That(logged, Does.Contain(BucketName));
    }

    [Test]
    public void Core_S3Storage_GetFileFromS3Storage_DoesThrowDescriptiveExceptionWhenNoClientIsConfigured()
    {
        // Arrange
        Core.SetS3ClientForTesting(null);

        // Act & Assert
        AsyncTestDelegate act = async () => await sut.GetFileFromS3Storage("anything.jpg");
        Assert.That(act, Throws.TypeOf<InvalidOperationException>().With.Message.Contains("s3Enabled"));
    }

    #endregion

    #region put

    [Test]
    public void Core_S3Storage_PutFileToS3Storage_DoesThrowWhenTheUploadFails()
    {
        // Arrange
        // Callers commit a database row pointing at this key, so a swallowed failure here leaves a
        // permanently unreadable reference behind.
        AmazonS3Exception expected = AccessDenied();
        s3Client.PutObjectAsync(Arg.Any<PutObjectRequest>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(expected);

        // Act
        AsyncTestDelegate act = async () =>
            await sut.PutFileToS3Storage(new MemoryStream([1, 2, 3]), "upload.jpg");
        AmazonS3Exception actual = Assert.ThrowsAsync<AmazonS3Exception>(act);

        // Assert
        Assert.That(actual, Is.SameAs(expected));
        Assert.That(actual.ErrorCode, Is.EqualTo("AccessDenied"));
    }

    [Test]
    public async Task Core_S3Storage_PutFileToS3Storage_DoesNotRetryAStreamUpload()
    {
        // Arrange
        // Pins the single attempt. PutObjectRequest.AutoCloseStream defaults to true, so a second
        // attempt can be handed a closed stream and would then fail on the stream rather than on
        // S3, hiding the real cause. A substitute cannot demonstrate that hazard, so this asserts
        // the call count that prevents it.
        s3Client.PutObjectAsync(Arg.Any<PutObjectRequest>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(AccessDenied());

        // Act
        AsyncTestDelegate act = async () =>
            await sut.PutFileToS3Storage(new MemoryStream([1, 2, 3]), "once.jpg");
        Assert.ThrowsAsync<AmazonS3Exception>(act);

        // Assert
        await s3Client.Received(1)
            .PutObjectAsync(Arg.Any<PutObjectRequest>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Core_S3Storage_PutFileToS3Storage_DoesSendTheStreamToTheCustomerPrefixedKey()
    {
        // Arrange
        PutObjectRequest captured = null;
        s3Client.PutObjectAsync(Arg.Do<PutObjectRequest>(request => captured = request),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new PutObjectResponse()));

        // Act
        MemoryStream payload = new MemoryStream([1, 2, 3]);
        await sut.PutFileToS3Storage(payload, "sent.jpg");

        // Assert
        Assert.That(captured, Is.Not.Null);
        Assert.That(captured.InputStream, Is.SameAs(payload));
        Assert.That(captured.BucketName, Is.EqualTo(BucketName));
        Assert.That(captured.Key, Is.EqualTo($"{CustomerNo}/sent.jpg"));
    }

    [Test]
    public async Task Core_S3Storage_PutFileToStorageSystem_DoesRetryOnceThenThrow()
    {
        // Arrange
        // The file path overload does retry, unlike the stream overload. Two attempts, then the
        // original exception reaches the caller.
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        await File.WriteAllTextAsync(filePath, "retry me");
        s3Client.PutObjectAsync(Arg.Any<PutObjectRequest>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(AccessDenied());

        try
        {
            // Act
            AsyncTestDelegate act = async () => await sut.PutFileToStorageSystem(filePath, "retried.txt");
            Assert.ThrowsAsync<AmazonS3Exception>(act);

            // Assert
            await s3Client.Received(2).PutObjectAsync(
                Arg.Is<PutObjectRequest>(request =>
                    request.BucketName == BucketName && request.Key == $"{CustomerNo}/retried.txt"),
                Arg.Any<CancellationToken>());
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [Test]
    public async Task Core_S3Storage_PutFileToStorageSystem_DoesNothingWhenStorageIsDisabled()
    {
        // Arrange
        // Pins current behaviour rather than endorsing it: with s3Enabled false the upload is
        // skipped and the caller is told nothing, so it commits a row against a key that was never
        // written. That is the same end state as the incident, reached through configuration.
        DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
        SqlController sql = new SqlController(dbContextHelper);
        await sql.SettingUpdate(Settings.s3Enabled, "false");

        Core disabled = new Core();
        disabled.Log = new Log(new CapturingLogWriter());
        await disabled.StartSqlOnly(ConnectionString);

        // Act
        await disabled.PutFileToStorageSystem("/tmp/never-read.txt", "never-stored.txt");

        // Assert
        await s3Client.DidNotReceive()
            .PutObjectAsync(Arg.Any<PutObjectRequest>(), Arg.Any<CancellationToken>());
    }

    #endregion

    #region helpers

    /// <summary>
    /// Collects log entries so tests can assert on the diagnostics an operator would see.
    /// </summary>
    private sealed class CapturingLogWriter : LogWriter
    {
        private readonly List<LogEntry> entries = [];

        public override void WriteLogEntry(LogEntry logEntry)
        {
            lock (entries)
            {
                entries.Add(logEntry);
            }
        }

        public override void WriteIfFailed(string str)
        {
        }

        public string AllText()
        {
            lock (entries)
            {
                return string.Join(Environment.NewLine, entries.ConvertAll(entry => entry.Message));
            }
        }
    }

    private static AmazonS3Exception AccessDenied()
    {
        return new AmazonS3Exception("Access Denied", ErrorType.Sender, "AccessDenied", "REQUEST-ID-1",
            HttpStatusCode.Forbidden);
    }

    private async Task AddUploadedData(string fileName)
    {
        UploadedData uploadedData = new UploadedData
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Extension = "jpg",
            UploaderId = 1,
            UploaderType = Constants.UploaderTypes.System,
            WorkflowState = Constants.WorkflowStates.Created,
            Version = 1,
            Local = 0,
            // Deliberately scheme-less. DownloadUploadedData hands this to HttpClient, which
            // rejects a relative URI before opening a socket, so recovery fails fast and offline.
            // Giving this a scheme would make every run do real DNS under a 100 second timeout.
            FileLocation = "/tmp/not-a-real-location.jpg",
            FileName = fileName,
            CurrentFile = fileName,
            Checksum = ""
        };

        DbContext.UploadedDatas.Add(uploadedData);
        await DbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    #endregion
}
