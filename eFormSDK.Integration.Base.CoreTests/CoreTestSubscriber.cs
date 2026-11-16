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
using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microting.eForm;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Helpers;
using Microting.eForm.Services;
using NSubstitute;
using NUnit.Framework;

namespace eFormSDK.Integration.Base.CoreTests;

/// <summary>
/// Covers how the Subscriber thread copes with SQS polls that fail.
///
/// The subscriber runs on a raw Thread, so an exception that escapes its loop is unhandled and
/// ends the whole hosting process. A DNS outage of a few seconds made every poll fail with an
/// HttpRequestException, and the second failure within five minutes was rethrown.
///
/// The SQS client is substituted, as there is no reachable SQS endpoint in CI, and the backoff
/// is shortened so the test does not have to wait for production delays.
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class CoreTestSubscriber : DbTestFixture
{
    private SqlController sqlController;
    private IAmazonSQS sqsClient;
    private CapturingLogWriter logWriter;

    public override Task DoSetup()
    {
        sqlController = new SqlController(new DbContextHelper(ConnectionString));
        sqsClient = Substitute.For<IAmazonSQS>();
        logWriter = new CapturingLogWriter();
        return Task.CompletedTask;
    }

    [TearDown]
    public void DisposeSqsClient()
    {
        sqsClient?.Dispose();
        sqsClient = null;
    }

    [Test]
    public async Task Subscriber_RepeatedHttpRequestExceptionFromSqs_KeepsPollingAndStopsCleanlyOnClose()
    {
        // Arrange
        const int failingPolls = 3;
        int polls = 0;
        sqsClient.ReceiveMessageAsync(Arg.Any<ReceiveMessageRequest>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                int poll = Interlocked.Increment(ref polls);
                if (poll <= failingPolls)
                {
                    return Task.FromException<ReceiveMessageResponse>(new HttpRequestException(
                        "Resource temporarily unavailable (sqs.eu-central-1.amazonaws.com:443)",
                        new SocketException((int)SocketError.TryAgain)));
                }

                return EmptyLongPoll(callInfo.Arg<CancellationToken>());
            });

        Subscriber subscriber = new Subscriber(sqlController, new Log(logWriter), null, sqsClient,
            TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(50));

        // Act
        subscriber.Start();
        bool keptPolling = await WaitUntil(() => Volatile.Read(ref polls) > failingPolls + 1);
        bool activeAfterFailures = subscriber.IsActive();

        await subscriber.Close();
        bool stopped = await WaitUntil(() => !subscriber.IsActive());

        // Assert
        Assert.That(keptPolling, Is.True,
            $"the subscriber stopped polling after {Volatile.Read(ref polls)} polls");
        Assert.That(activeAfterFailures, Is.True);
        Assert.That(stopped, Is.True, "the subscriber did not stop after Close");
        Assert.That(logWriter.AllText(), Does.Contain("Resource temporarily unavailable"));
    }

    [Test]
    public async Task Subscriber_CloseDuringBackoff_StopsWithoutWaitingForTheBackoff()
    {
        // Arrange
        int polls = 0;
        sqsClient.ReceiveMessageAsync(Arg.Any<ReceiveMessageRequest>(), Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                Interlocked.Increment(ref polls);
                return Task.FromException<ReceiveMessageResponse>(new HttpRequestException(
                    "Resource temporarily unavailable (sqs.eu-central-1.amazonaws.com:443)",
                    new SocketException((int)SocketError.TryAgain)));
            });

        // A backoff far longer than WaitUntil's timeout, so only a cancellable wait lets it stop.
        Subscriber subscriber = new Subscriber(sqlController, new Log(logWriter), null, sqsClient,
            TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10));

        // Act
        subscriber.Start();
        bool failedOnce = await WaitUntil(() => Volatile.Read(ref polls) >= 1);
        // Gives the thread time to log the failure and enter the backoff wait.
        await Task.Delay(500);

        await subscriber.Close();
        bool stopped = await WaitUntil(() => !subscriber.IsActive());

        // Assert
        Assert.That(failedOnce, Is.True, "the subscriber never polled");
        Assert.That(stopped, Is.True, "the subscriber did not stop while backing off");
        Assert.That(Volatile.Read(ref polls), Is.EqualTo(1));
    }

    private static async Task<ReceiveMessageResponse> EmptyLongPoll(CancellationToken cancellationToken)
    {
        // Stands in for SQS long polling, so the loop does not spin while the test waits.
        await Task.Delay(20, cancellationToken);
        return new ReceiveMessageResponse { Messages = new List<Message>() };
    }

    private static async Task<bool> WaitUntil(Func<bool> condition)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        while (stopwatch.Elapsed < TimeSpan.FromSeconds(15))
        {
            if (condition())
            {
                return true;
            }

            await Task.Delay(50);
        }

        return condition();
    }

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
}
