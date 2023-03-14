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

            Assert.AreEqual(1, uploadedDatas.Count());
            Assert.AreEqual(1, uploadedDataVersions.Count());


            Assert.AreEqual(uploadedData.CreatedAt.ToString(), uploadedDatas[0].CreatedAt.ToString());
            Assert.AreEqual(uploadedData.Version, uploadedDatas[0].Version);
//            Assert.AreEqual(uploadedData.UpdatedAt.ToString(), uploadedDatas[0].UpdatedAt.ToString());
            Assert.AreEqual(uploadedDatas[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(uploadedData.Checksum, uploadedDatas[0].Checksum);
            Assert.AreEqual(uploadedData.Extension, uploadedDatas[0].Extension);
            Assert.AreEqual(uploadedData.Local, uploadedDatas[0].Local);
            Assert.AreEqual(uploadedData.CurrentFile, uploadedDatas[0].CurrentFile);
            Assert.AreEqual(uploadedData.ExpirationDate.ToString(), uploadedDatas[0].ExpirationDate.ToString());
            Assert.AreEqual(uploadedData.FileLocation, uploadedDatas[0].FileLocation);
            Assert.AreEqual(uploadedData.FileName, uploadedDatas[0].FileName);
            Assert.AreEqual(uploadedData.TranscriptionId, uploadedDatas[0].TranscriptionId);
            Assert.AreEqual(uploadedData.UploaderId, uploadedDatas[0].UploaderId);
            Assert.AreEqual(uploadedData.UploaderType, uploadedDatas[0].UploaderType);

            //Versions
            Assert.AreEqual(uploadedData.CreatedAt.ToString(), uploadedDataVersions[0].CreatedAt.ToString());
            Assert.AreEqual(uploadedData.Version, uploadedDataVersions[0].Version);
//            Assert.AreEqual(uploadedData.UpdatedAt.ToString(), uploadedDataVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(uploadedDataVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(uploadedData.Checksum, uploadedDataVersions[0].Checksum);
            Assert.AreEqual(uploadedData.Extension, uploadedDataVersions[0].Extension);
            Assert.AreEqual(uploadedData.Local, uploadedDataVersions[0].Local);
            Assert.AreEqual(uploadedData.CurrentFile, uploadedDataVersions[0].CurrentFile);
            Assert.AreEqual(uploadedData.ExpirationDate.ToString(), uploadedDataVersions[0].ExpirationDate.ToString());
            Assert.AreEqual(uploadedData.FileLocation, uploadedDataVersions[0].FileLocation);
            Assert.AreEqual(uploadedData.FileName, uploadedDataVersions[0].FileName);
            Assert.AreEqual(uploadedData.UploaderId, uploadedDataVersions[0].UploaderId);
            Assert.AreEqual(uploadedData.UploaderType, uploadedDataVersions[0].UploaderType);
            Assert.AreEqual(uploadedData.TranscriptionId, uploadedDataVersions[0].TranscriptionId);
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

            Assert.AreEqual(1, uploadedDatas.Count());
            Assert.AreEqual(2, uploadedDataVersions.Count());

            Assert.AreEqual(uploadedData.CreatedAt.ToString(), uploadedDatas[0].CreatedAt.ToString());
            Assert.AreEqual(uploadedData.Version, uploadedDatas[0].Version);
//            Assert.AreEqual(uploadedData.UpdatedAt.ToString(), uploadedDatas[0].UpdatedAt.ToString());
            Assert.AreEqual(uploadedData.Checksum, uploadedDatas[0].Checksum);
            Assert.AreEqual(uploadedData.Extension, uploadedDatas[0].Extension);
            Assert.AreEqual(uploadedData.Local, uploadedDatas[0].Local);
            Assert.AreEqual(uploadedData.CurrentFile, uploadedDatas[0].CurrentFile);
            Assert.AreEqual(uploadedData.ExpirationDate.ToString(), uploadedDatas[0].ExpirationDate.ToString());
            Assert.AreEqual(uploadedData.FileLocation, uploadedDatas[0].FileLocation);
            Assert.AreEqual(uploadedData.FileName, uploadedDatas[0].FileName);
            Assert.AreEqual(uploadedData.TranscriptionId, uploadedDatas[0].TranscriptionId);
            Assert.AreEqual(uploadedData.UploaderId, uploadedDatas[0].UploaderId);
            Assert.AreEqual(uploadedData.UploaderType, uploadedDatas[0].UploaderType);

            //Version 1 Old Version
            Assert.AreEqual(uploadedData.CreatedAt.ToString(), uploadedDataVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, uploadedDataVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), uploadedDataVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(oldCheckSum, uploadedDataVersions[0].Checksum);
            Assert.AreEqual(oldExtension, uploadedDataVersions[0].Extension);
            Assert.AreEqual(oldLocal, uploadedDataVersions[0].Local);
            Assert.AreEqual(oldCurrentFile, uploadedDataVersions[0].CurrentFile);
            Assert.AreEqual(oldExpirationDate.ToString(), uploadedDataVersions[0].ExpirationDate.ToString());
            Assert.AreEqual(oldFileLocation, uploadedDataVersions[0].FileLocation);
            Assert.AreEqual(oldFileName, uploadedDataVersions[0].FileName);
            Assert.AreEqual(oldTranscriptionId, uploadedDataVersions[0].TranscriptionId);
            Assert.AreEqual(oldUploaderId, uploadedDataVersions[0].UploaderId);
            Assert.AreEqual(oldUploaderType, uploadedDataVersions[0].UploaderType);

            //Version 2 Updated Version
            Assert.AreEqual(uploadedData.CreatedAt.ToString(), uploadedDataVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, uploadedDataVersions[1].Version);
//            Assert.AreEqual(uploadedData.UpdatedAt.ToString(), uploadedDataVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(uploadedData.Checksum, uploadedDataVersions[1].Checksum);
            Assert.AreEqual(uploadedData.Extension, uploadedDataVersions[1].Extension);
            Assert.AreEqual(uploadedData.Local, uploadedDataVersions[1].Local);
            Assert.AreEqual(uploadedData.CurrentFile, uploadedDataVersions[1].CurrentFile);
            Assert.AreEqual(uploadedData.ExpirationDate.ToString(), uploadedDataVersions[1].ExpirationDate.ToString());
            Assert.AreEqual(uploadedData.FileLocation, uploadedDataVersions[1].FileLocation);
            Assert.AreEqual(uploadedData.FileName, uploadedDataVersions[1].FileName);
            Assert.AreEqual(uploadedData.TranscriptionId, uploadedDataVersions[1].TranscriptionId);
            Assert.AreEqual(uploadedData.UploaderId, uploadedDataVersions[1].UploaderId);
            Assert.AreEqual(uploadedData.UploaderType, uploadedDataVersions[1].UploaderType);
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

            Assert.AreEqual(1, uploadedDatas.Count());
            Assert.AreEqual(2, uploadedDataVersions.Count());

            Assert.AreEqual(uploadedData.CreatedAt.ToString(), uploadedDatas[0].CreatedAt.ToString());
            Assert.AreEqual(uploadedData.Version, uploadedDatas[0].Version);
            Assert.AreEqual(uploadedData.Checksum, uploadedDatas[0].Checksum);
            Assert.AreEqual(uploadedData.Extension, uploadedDatas[0].Extension);
            Assert.AreEqual(uploadedData.Local, uploadedDatas[0].Local);
            Assert.AreEqual(uploadedData.CurrentFile, uploadedDatas[0].CurrentFile);
            Assert.AreEqual(uploadedData.ExpirationDate.ToString(), uploadedDatas[0].ExpirationDate.ToString());
            Assert.AreEqual(uploadedData.FileLocation, uploadedDatas[0].FileLocation);
            Assert.AreEqual(uploadedData.FileName, uploadedDatas[0].FileName);
            Assert.AreEqual(uploadedData.TranscriptionId, uploadedDatas[0].TranscriptionId);
            Assert.AreEqual(uploadedData.UploaderId, uploadedDatas[0].UploaderId);
            Assert.AreEqual(uploadedData.UploaderType, uploadedDatas[0].UploaderType);

            //Version 1
            Assert.AreEqual(uploadedData.CreatedAt.ToString(), uploadedDataVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, uploadedDataVersions[0].Version);
            Assert.AreEqual(uploadedData.Checksum, uploadedDataVersions[0].Checksum);
            Assert.AreEqual(uploadedData.Extension, uploadedDataVersions[0].Extension);
            Assert.AreEqual(uploadedData.Local, uploadedDataVersions[0].Local);
            Assert.AreEqual(uploadedData.CurrentFile, uploadedDataVersions[0].CurrentFile);
            Assert.AreEqual(uploadedData.ExpirationDate.ToString(), uploadedDataVersions[0].ExpirationDate.ToString());
            Assert.AreEqual(uploadedData.FileLocation, uploadedDataVersions[0].FileLocation);
            Assert.AreEqual(uploadedData.FileName, uploadedDataVersions[0].FileName);
            Assert.AreEqual(uploadedData.TranscriptionId, uploadedDataVersions[0].TranscriptionId);
            Assert.AreEqual(uploadedData.UploaderId, uploadedDataVersions[0].UploaderId);
            Assert.AreEqual(uploadedData.UploaderType, uploadedDataVersions[0].UploaderType);

            Assert.AreEqual(uploadedDataVersions[0].WorkflowState, Constants.WorkflowStates.Created);

            //Version 2 Deleted Version
            Assert.AreEqual(uploadedData.CreatedAt.ToString(), uploadedDataVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, uploadedDataVersions[1].Version);
            Assert.AreEqual(uploadedData.Checksum, uploadedDataVersions[1].Checksum);
            Assert.AreEqual(uploadedData.Extension, uploadedDataVersions[1].Extension);
            Assert.AreEqual(uploadedData.Local, uploadedDataVersions[1].Local);
            Assert.AreEqual(uploadedData.CurrentFile, uploadedDataVersions[1].CurrentFile);
            Assert.AreEqual(uploadedData.ExpirationDate.ToString(), uploadedDataVersions[1].ExpirationDate.ToString());
            Assert.AreEqual(uploadedData.FileLocation, uploadedDataVersions[1].FileLocation);
            Assert.AreEqual(uploadedData.FileName, uploadedDataVersions[1].FileName);
            Assert.AreEqual(uploadedData.TranscriptionId, uploadedDataVersions[1].TranscriptionId);
            Assert.AreEqual(uploadedData.UploaderId, uploadedDataVersions[1].UploaderId);
            Assert.AreEqual(uploadedData.UploaderType, uploadedDataVersions[1].UploaderType);

            Assert.AreEqual(uploadedDatas[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(uploadedDataVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
        }
    }
}