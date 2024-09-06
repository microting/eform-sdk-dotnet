/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.CheckLists.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class UploadedDataUTest : DbTestFixture
    {
        [Test]
        public async Task UploadedData_Create_DoesCreate()
        {
            //Arrange
            Random rnd = new Random();

            short minValue = Int16.MinValue;
            short maxValue = Int16.MaxValue;

            UploadedData uploadedData = new UploadedData
            {
                Checksum = Guid.NewGuid().ToString(),
                Extension = Guid.NewGuid().ToString(),
                Local = (short)rnd.Next(minValue, maxValue),
                CurrentFile = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.UtcNow,
                FileLocation = Guid.NewGuid().ToString(),
                FileName = Guid.NewGuid().ToString(),
                TranscriptionId = rnd.Next(1, 255),
                UploaderId = rnd.Next(1, 255),
                UploaderType = Guid.NewGuid().ToString()
            };

            //Act

            await uploadedData.Create(DbContext).ConfigureAwait(false);

            List<UploadedData> uploadedDatas = DbContext.UploadedDatas.AsNoTracking().ToList();
            List<UploadedDataVersion> uploadedDataVersions = DbContext.UploadedDataVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(uploadedDatas);
            Assert.NotNull(uploadedDataVersions);

            Assert.That(uploadedDatas.Count(), Is.EqualTo(1));
            Assert.That(uploadedDataVersions.Count(), Is.EqualTo(1));


            Assert.That(uploadedDatas[0].CreatedAt.ToString(), Is.EqualTo(uploadedData.CreatedAt.ToString()));
            Assert.That(uploadedDatas[0].Version, Is.EqualTo(uploadedData.Version));
            //            Assert.AreEqual(uploadedData.UpdatedAt.ToString(), uploadedDatas[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(uploadedDatas[0].WorkflowState));
            Assert.That(uploadedDatas[0].Checksum, Is.EqualTo(uploadedData.Checksum));
            Assert.That(uploadedDatas[0].Extension, Is.EqualTo(uploadedData.Extension));
            Assert.That(uploadedDatas[0].Local, Is.EqualTo(uploadedData.Local));
            Assert.That(uploadedDatas[0].CurrentFile, Is.EqualTo(uploadedData.CurrentFile));
            Assert.That(uploadedDatas[0].ExpirationDate.ToString(), Is.EqualTo(uploadedData.ExpirationDate.ToString()));
            Assert.That(uploadedDatas[0].FileLocation, Is.EqualTo(uploadedData.FileLocation));
            Assert.That(uploadedDatas[0].FileName, Is.EqualTo(uploadedData.FileName));
            Assert.That(uploadedDatas[0].TranscriptionId, Is.EqualTo(uploadedData.TranscriptionId));
            Assert.That(uploadedDatas[0].UploaderId, Is.EqualTo(uploadedData.UploaderId));
            Assert.That(uploadedDatas[0].UploaderType, Is.EqualTo(uploadedData.UploaderType));

            //Versions
            Assert.That(uploadedDataVersions[0].CreatedAt.ToString(), Is.EqualTo(uploadedData.CreatedAt.ToString()));
            Assert.That(uploadedDataVersions[0].Version, Is.EqualTo(uploadedData.Version));
            //            Assert.AreEqual(uploadedData.UpdatedAt.ToString(), uploadedDataVersions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(uploadedDataVersions[0].WorkflowState));
            Assert.That(uploadedDataVersions[0].Checksum, Is.EqualTo(uploadedData.Checksum));
            Assert.That(uploadedDataVersions[0].Extension, Is.EqualTo(uploadedData.Extension));
            Assert.That(uploadedDataVersions[0].Local, Is.EqualTo(uploadedData.Local));
            Assert.That(uploadedDataVersions[0].CurrentFile, Is.EqualTo(uploadedData.CurrentFile));
            Assert.That(uploadedDataVersions[0].ExpirationDate.ToString(), Is.EqualTo(uploadedData.ExpirationDate.ToString()));
            Assert.That(uploadedDataVersions[0].FileLocation, Is.EqualTo(uploadedData.FileLocation));
            Assert.That(uploadedDataVersions[0].FileName, Is.EqualTo(uploadedData.FileName));
            Assert.That(uploadedDataVersions[0].UploaderId, Is.EqualTo(uploadedData.UploaderId));
            Assert.That(uploadedDataVersions[0].UploaderType, Is.EqualTo(uploadedData.UploaderType));
            Assert.That(uploadedDataVersions[0].TranscriptionId, Is.EqualTo(uploadedData.TranscriptionId));
        }

        [Test]
        public async Task UploadedData_Update_DoesUpdate()
        {
            //Arrange

            Random rnd = new Random();


            short minValue = Int16.MinValue;
            short maxValue = Int16.MaxValue;

            UploadedData uploadedData = new UploadedData
            {
                Checksum = Guid.NewGuid().ToString(),
                Extension = Guid.NewGuid().ToString(),
                Local = (short)rnd.Next(minValue, maxValue),
                CurrentFile = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.UtcNow,
                FileLocation = Guid.NewGuid().ToString(),
                FileName = Guid.NewGuid().ToString(),
                TranscriptionId = rnd.Next(1, 255),
                UploaderId = rnd.Next(1, 255),
                UploaderType = Guid.NewGuid().ToString()
            };

            await uploadedData.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = uploadedData.UpdatedAt;
            string oldCheckSum = uploadedData.Checksum;
            string oldExtension = uploadedData.Extension;
            short oldLocal = (short)uploadedData.Local;
            string oldCurrentFile = uploadedData.CurrentFile;
            DateTime? oldExpirationDate = uploadedData.ExpirationDate;
            string oldFileLocation = uploadedData.FileLocation;
            string oldFileName = uploadedData.FileName;
            int? oldTranscriptionId = uploadedData.TranscriptionId;
            int? oldUploaderId = uploadedData.UploaderId;
            string oldUploaderType = uploadedData.UploaderType;


            uploadedData.Checksum = Guid.NewGuid().ToString();
            uploadedData.Extension = Guid.NewGuid().ToString();
            uploadedData.Local = (short)rnd.Next(minValue, maxValue);
            uploadedData.CurrentFile = Guid.NewGuid().ToString();
            uploadedData.ExpirationDate = DateTime.UtcNow;
            uploadedData.FileLocation = Guid.NewGuid().ToString();
            uploadedData.FileName = Guid.NewGuid().ToString();
            uploadedData.TranscriptionId = rnd.Next(1, 255);
            uploadedData.UploaderId = rnd.Next(1, 255);
            uploadedData.UploaderType = Guid.NewGuid().ToString();

            await uploadedData.Update(DbContext).ConfigureAwait(false);

            List<UploadedData> uploadedDatas = DbContext.UploadedDatas.AsNoTracking().ToList();
            List<UploadedDataVersion> uploadedDataVersions = DbContext.UploadedDataVersions.AsNoTracking().ToList();

            //Assert


            Assert.NotNull(uploadedDatas);
            Assert.NotNull(uploadedDataVersions);

            Assert.That(uploadedDatas.Count(), Is.EqualTo(1));
            Assert.That(uploadedDataVersions.Count(), Is.EqualTo(2));

            Assert.That(uploadedDatas[0].CreatedAt.ToString(), Is.EqualTo(uploadedData.CreatedAt.ToString()));
            Assert.That(uploadedDatas[0].Version, Is.EqualTo(uploadedData.Version));
            //            Assert.AreEqual(uploadedData.UpdatedAt.ToString(), uploadedDatas[0].UpdatedAt.ToString());
            Assert.That(uploadedDatas[0].Checksum, Is.EqualTo(uploadedData.Checksum));
            Assert.That(uploadedDatas[0].Extension, Is.EqualTo(uploadedData.Extension));
            Assert.That(uploadedDatas[0].Local, Is.EqualTo(uploadedData.Local));
            Assert.That(uploadedDatas[0].CurrentFile, Is.EqualTo(uploadedData.CurrentFile));
            Assert.That(uploadedDatas[0].ExpirationDate.ToString(), Is.EqualTo(uploadedData.ExpirationDate.ToString()));
            Assert.That(uploadedDatas[0].FileLocation, Is.EqualTo(uploadedData.FileLocation));
            Assert.That(uploadedDatas[0].FileName, Is.EqualTo(uploadedData.FileName));
            Assert.That(uploadedDatas[0].TranscriptionId, Is.EqualTo(uploadedData.TranscriptionId));
            Assert.That(uploadedDatas[0].UploaderId, Is.EqualTo(uploadedData.UploaderId));
            Assert.That(uploadedDatas[0].UploaderType, Is.EqualTo(uploadedData.UploaderType));

            //Version 1 Old Version
            Assert.That(uploadedDataVersions[0].CreatedAt.ToString(), Is.EqualTo(uploadedData.CreatedAt.ToString()));
            Assert.That(uploadedDataVersions[0].Version, Is.EqualTo(1));
            //            Assert.AreEqual(oldUpdatedAt.ToString(), uploadedDataVersions[0].UpdatedAt.ToString());
            Assert.That(uploadedDataVersions[0].Checksum, Is.EqualTo(oldCheckSum));
            Assert.That(uploadedDataVersions[0].Extension, Is.EqualTo(oldExtension));
            Assert.That(uploadedDataVersions[0].Local, Is.EqualTo(oldLocal));
            Assert.That(uploadedDataVersions[0].CurrentFile, Is.EqualTo(oldCurrentFile));
            Assert.That(uploadedDataVersions[0].ExpirationDate.ToString(), Is.EqualTo(oldExpirationDate.ToString()));
            Assert.That(uploadedDataVersions[0].FileLocation, Is.EqualTo(oldFileLocation));
            Assert.That(uploadedDataVersions[0].FileName, Is.EqualTo(oldFileName));
            Assert.That(uploadedDataVersions[0].TranscriptionId, Is.EqualTo(oldTranscriptionId));
            Assert.That(uploadedDataVersions[0].UploaderId, Is.EqualTo(oldUploaderId));
            Assert.That(uploadedDataVersions[0].UploaderType, Is.EqualTo(oldUploaderType));

            //Version 2 Updated Version
            Assert.That(uploadedDataVersions[1].CreatedAt.ToString(), Is.EqualTo(uploadedData.CreatedAt.ToString()));
            Assert.That(uploadedDataVersions[1].Version, Is.EqualTo(2));
            //            Assert.AreEqual(uploadedData.UpdatedAt.ToString(), uploadedDataVersions[1].UpdatedAt.ToString());
            Assert.That(uploadedDataVersions[1].Checksum, Is.EqualTo(uploadedData.Checksum));
            Assert.That(uploadedDataVersions[1].Extension, Is.EqualTo(uploadedData.Extension));
            Assert.That(uploadedDataVersions[1].Local, Is.EqualTo(uploadedData.Local));
            Assert.That(uploadedDataVersions[1].CurrentFile, Is.EqualTo(uploadedData.CurrentFile));
            Assert.That(uploadedDataVersions[1].ExpirationDate.ToString(), Is.EqualTo(uploadedData.ExpirationDate.ToString()));
            Assert.That(uploadedDataVersions[1].FileLocation, Is.EqualTo(uploadedData.FileLocation));
            Assert.That(uploadedDataVersions[1].FileName, Is.EqualTo(uploadedData.FileName));
            Assert.That(uploadedDataVersions[1].TranscriptionId, Is.EqualTo(uploadedData.TranscriptionId));
            Assert.That(uploadedDataVersions[1].UploaderId, Is.EqualTo(uploadedData.UploaderId));
            Assert.That(uploadedDataVersions[1].UploaderType, Is.EqualTo(uploadedData.UploaderType));
        }

        [Test]
        public async Task UploadedData_Delete_DoesSetWorkflowstateToRemoved()
        {
            //Arrange

            Random rnd = new Random();

            short minValue = Int16.MinValue;
            short maxValue = Int16.MaxValue;

            UploadedData uploadedData = new UploadedData
            {
                Checksum = Guid.NewGuid().ToString(),
                Extension = Guid.NewGuid().ToString(),
                Local = (short)rnd.Next(minValue, maxValue),
                CurrentFile = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.UtcNow,
                FileLocation = Guid.NewGuid().ToString(),
                FileName = Guid.NewGuid().ToString(),
                TranscriptionId = rnd.Next(1, 255),
                UploaderId = rnd.Next(1, 255),
                UploaderType = Guid.NewGuid().ToString()
            };

            await uploadedData.Create(DbContext).ConfigureAwait(false);
            //Act

            DateTime? oldUpdatedAt = uploadedData.UpdatedAt;

            await uploadedData.Delete(DbContext);

            List<UploadedData> uploadedDatas = DbContext.UploadedDatas.AsNoTracking().ToList();
            List<UploadedDataVersion> uploadedDataVersions = DbContext.UploadedDataVersions.AsNoTracking().ToList();

            //Assert


            Assert.NotNull(uploadedDatas);
            Assert.NotNull(uploadedDataVersions);

            Assert.That(uploadedDatas.Count(), Is.EqualTo(1));
            Assert.That(uploadedDataVersions.Count(), Is.EqualTo(2));

            Assert.That(uploadedDatas[0].CreatedAt.ToString(), Is.EqualTo(uploadedData.CreatedAt.ToString()));
            Assert.That(uploadedDatas[0].Version, Is.EqualTo(uploadedData.Version));
            Assert.That(uploadedDatas[0].Checksum, Is.EqualTo(uploadedData.Checksum));
            Assert.That(uploadedDatas[0].Extension, Is.EqualTo(uploadedData.Extension));
            Assert.That(uploadedDatas[0].Local, Is.EqualTo(uploadedData.Local));
            Assert.That(uploadedDatas[0].CurrentFile, Is.EqualTo(uploadedData.CurrentFile));
            Assert.That(uploadedDatas[0].ExpirationDate.ToString(), Is.EqualTo(uploadedData.ExpirationDate.ToString()));
            Assert.That(uploadedDatas[0].FileLocation, Is.EqualTo(uploadedData.FileLocation));
            Assert.That(uploadedDatas[0].FileName, Is.EqualTo(uploadedData.FileName));
            Assert.That(uploadedDatas[0].TranscriptionId, Is.EqualTo(uploadedData.TranscriptionId));
            Assert.That(uploadedDatas[0].UploaderId, Is.EqualTo(uploadedData.UploaderId));
            Assert.That(uploadedDatas[0].UploaderType, Is.EqualTo(uploadedData.UploaderType));

            //Version 1
            Assert.That(uploadedDataVersions[0].CreatedAt.ToString(), Is.EqualTo(uploadedData.CreatedAt.ToString()));
            Assert.That(uploadedDataVersions[0].Version, Is.EqualTo(1));
            Assert.That(uploadedDataVersions[0].Checksum, Is.EqualTo(uploadedData.Checksum));
            Assert.That(uploadedDataVersions[0].Extension, Is.EqualTo(uploadedData.Extension));
            Assert.That(uploadedDataVersions[0].Local, Is.EqualTo(uploadedData.Local));
            Assert.That(uploadedDataVersions[0].CurrentFile, Is.EqualTo(uploadedData.CurrentFile));
            Assert.That(uploadedDataVersions[0].ExpirationDate.ToString(), Is.EqualTo(uploadedData.ExpirationDate.ToString()));
            Assert.That(uploadedDataVersions[0].FileLocation, Is.EqualTo(uploadedData.FileLocation));
            Assert.That(uploadedDataVersions[0].FileName, Is.EqualTo(uploadedData.FileName));
            Assert.That(uploadedDataVersions[0].TranscriptionId, Is.EqualTo(uploadedData.TranscriptionId));
            Assert.That(uploadedDataVersions[0].UploaderId, Is.EqualTo(uploadedData.UploaderId));
            Assert.That(uploadedDataVersions[0].UploaderType, Is.EqualTo(uploadedData.UploaderType));

            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(uploadedDataVersions[0].WorkflowState));

            //Version 2 Deleted Version
            Assert.That(uploadedDataVersions[1].CreatedAt.ToString(), Is.EqualTo(uploadedData.CreatedAt.ToString()));
            Assert.That(uploadedDataVersions[1].Version, Is.EqualTo(2));
            Assert.That(uploadedDataVersions[1].Checksum, Is.EqualTo(uploadedData.Checksum));
            Assert.That(uploadedDataVersions[1].Extension, Is.EqualTo(uploadedData.Extension));
            Assert.That(uploadedDataVersions[1].Local, Is.EqualTo(uploadedData.Local));
            Assert.That(uploadedDataVersions[1].CurrentFile, Is.EqualTo(uploadedData.CurrentFile));
            Assert.That(uploadedDataVersions[1].ExpirationDate.ToString(), Is.EqualTo(uploadedData.ExpirationDate.ToString()));
            Assert.That(uploadedDataVersions[1].FileLocation, Is.EqualTo(uploadedData.FileLocation));
            Assert.That(uploadedDataVersions[1].FileName, Is.EqualTo(uploadedData.FileName));
            Assert.That(uploadedDataVersions[1].TranscriptionId, Is.EqualTo(uploadedData.TranscriptionId));
            Assert.That(uploadedDataVersions[1].UploaderId, Is.EqualTo(uploadedData.UploaderId));
            Assert.That(uploadedDataVersions[1].UploaderType, Is.EqualTo(uploadedData.UploaderType));

            Assert.That(Constants.WorkflowStates.Removed, Is.EqualTo(uploadedDatas[0].WorkflowState));
            Assert.That(Constants.WorkflowStates.Removed, Is.EqualTo(uploadedDataVersions[1].WorkflowState));
        }
    }
}