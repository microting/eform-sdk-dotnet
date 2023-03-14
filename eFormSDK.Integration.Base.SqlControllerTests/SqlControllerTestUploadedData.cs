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
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using NUnit.Framework;
using Case = Microting.eForm.Infrastructure.Data.Entities.Case;

namespace eFormSDK.Integration.Base.SqlControllerTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SqlControllerTestUploadedData : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");

            #endregion

            sut = new SqlController(dbContextHelper);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers(ConnectionString);
            await testHelpers.GenerateDefaultLanguages();
            await sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            await sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            await sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }

        #region uploaded_data

        [Test]
        public async Task SQL_UploadedData_FileRead_DoesReturnOneUploadedData()
        {
            // Arrance
            string checksum = "";
            string extension = "jpg";
            string currentFile = "Hello.jpg";
            int uploaderId = 1;
            string fileLocation = @"c:\here";
            string fileName = "Hello.jpg";

            // Act
            UploadedData dU = new UploadedData
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Extension = extension,
                UploaderId = uploaderId,
                UploaderType = Constants.UploaderTypes.System,
                WorkflowState = Constants.WorkflowStates.PreCreated,
                Version = 1,
                Local = 0,
                FileLocation = fileLocation,
                FileName = fileName,
                CurrentFile = currentFile,
                Checksum = checksum
            };


            DbContext.UploadedDatas.Add(dU);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            Microting.eForm.Infrastructure.Models.UploadedData ud = await sut.FileRead();

            // Assert
            Assert.NotNull(ud);
            Assert.AreEqual(dU.Id, ud.Id);
            Assert.AreEqual(dU.Checksum, ud.Checksum);
            Assert.AreEqual(dU.Extension, ud.Extension);
            Assert.AreEqual(dU.CurrentFile, ud.CurrentFile);
            Assert.AreEqual(dU.UploaderId, ud.UploaderId);
            Assert.AreEqual(dU.UploaderType, ud.UploaderType);
            Assert.AreEqual(dU.FileLocation, ud.FileLocation);
            Assert.AreEqual(dU.FileName, ud.FileName);
            // Assert.AreEqual(dU.local, ud.);
        }

        [Test]
        public async Task SQL_UploadedData_UploadedDataRead_DoesReturnOneUploadedDataClass()
        {
            // Arrance
            string checksum = "";
            string extension = "jpg";
            string currentFile = "Hello.jpg";
            int uploaderId = 1;
            string fileLocation = @"c:\here";
            string fileName = "Hello.jpg";

            // Act
            UploadedData dU = new UploadedData
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Extension = extension,
                UploaderId = uploaderId,
                UploaderType = Constants.UploaderTypes.System,
                WorkflowState = Constants.WorkflowStates.PreCreated,
                Version = 1,
                Local = 0,
                FileLocation = fileLocation,
                FileName = fileName,
                CurrentFile = currentFile,
                Checksum = checksum
            };


            DbContext.UploadedDatas.Add(dU);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            UploadedData ud = await sut.GetUploadedData(dU.Id);

            // Assert
            Assert.NotNull(ud);
            Assert.AreEqual(ud.Id, dU.Id);
            Assert.AreEqual(ud.Extension, dU.Extension);
            Assert.AreEqual(ud.UploaderId, dU.UploaderId);
            Assert.AreEqual(ud.UploaderType, dU.UploaderType);
            Assert.AreEqual(ud.WorkflowState, dU.WorkflowState);
            Assert.AreEqual(ud.Version, 1);
            Assert.AreEqual(ud.Local, 0);
            Assert.AreEqual(ud.FileLocation, dU.FileLocation);
            Assert.AreEqual(ud.FileName, dU.FileName);
            Assert.AreEqual(ud.CurrentFile, dU.CurrentFile);
            Assert.AreEqual(ud.Checksum, dU.Checksum);
        }


        [Test]
        public async Task SQL_File_FileRead_doesFileRead()
        {
            UploadedData ud = new UploadedData
            {
                Checksum = "checksum1",
                Extension = "extension",
                CurrentFile = "currentFile1",
                UploaderId = 223,
                UploaderType = "uploader_type",
                FileLocation = "file_location",
                FileName = "fileName",
                WorkflowState = Constants.WorkflowStates.PreCreated
            };

            DbContext.UploadedDatas.Add(ud);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            Microting.eForm.Infrastructure.Models.UploadedData Ud = await sut.FileRead();

            // Assert

            Assert.NotNull(ud);
            Assert.NotNull(Ud);
            Assert.AreEqual(Ud.Checksum, ud.Checksum);
            Assert.AreEqual(Ud.Extension, ud.Extension);
            Assert.AreEqual(Ud.CurrentFile, ud.CurrentFile);
            Assert.AreEqual(Ud.UploaderId, ud.UploaderId);
            Assert.AreEqual(Ud.UploaderType, ud.UploaderType);
            Assert.AreEqual(Ud.FileLocation, ud.FileLocation);
            Assert.AreEqual(Ud.FileName, ud.FileName);
            Assert.AreEqual(Ud.Id, ud.Id);
            Assert.AreEqual(Constants.WorkflowStates.PreCreated, ud.WorkflowState);
        }

        [Test]
        public async Task SQL_File_FileCaseFindMUId_doesFindMUId()
        {
            Random rnd = new Random();
            Site site1 = await testHelpers.CreateSite("MySite", 22);
            DateTime cl1_Ca = DateTime.UtcNow;
            DateTime cl1_Ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "template1", "template_desc", "", "", 1, 1);

            string guid = Guid.NewGuid().ToString();


            DateTime c1_ca = DateTime.UtcNow.AddDays(-9);
            DateTime c1_da = DateTime.UtcNow.AddDays(-8).AddHours(-12);
            DateTime c1_ua = DateTime.UtcNow.AddDays(-8);
            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
            Unit unit = await testHelpers.CreateUnit(48, 49, site1, 348);

            Case aCase1 = await testHelpers.CreateCase("case1UId", cl1, c1_ca, "custom1",
                c1_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site1, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);

            UploadedData ud = new UploadedData
            {
                Checksum = "checksum1",
                Extension = "extension",
                CurrentFile = "currentFile1",
                UploaderId = 223,
                UploaderType = "uploader_type",
                FileLocation = "url",
                FileName = "fileName"
            };

            DbContext.UploadedDatas.Add(ud);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            FieldValue fVs = new FieldValue
            {
                UploadedDataId = ud.Id,
                CaseId = aCase1.Id
            };

            DbContext.FieldValues.Add(fVs);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);


            // Act
            await sut.FileCaseFindMUId("url");


            Assert.NotNull(fVs);
            Assert.AreEqual(fVs.CaseId, aCase1.Id);
        }

        [Test]
        public async Task SQL_File_FileProcessed_isProcessed()
        {
            UploadedData ud = new UploadedData
            {
                Local = 0, WorkflowState = Constants.WorkflowStates.PreCreated, Version = 1
            };

            DbContext.UploadedDatas.Add(ud);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            await sut.FileProcessed("url", "myChecksum", "myFileLocation", "myFileName", ud.Id);
            List<UploadedData> uploadedDataResult = DbContext.UploadedDatas.AsNoTracking().ToList();
            //var versionedMatches = DbContext.uploaded_data_versions.AsNoTracking().ToList(); TODO 05/01/2018

            // Assert

            Assert.NotNull(uploadedDataResult);
            Assert.NotNull(ud);
            Assert.AreEqual(Constants.WorkflowStates.Created, uploadedDataResult[0].WorkflowState);
            Assert.AreEqual(0, uploadedDataResult[0].Local);
            Assert.AreEqual(2, uploadedDataResult[0].Version);
            Assert.AreEqual("myChecksum", uploadedDataResult[0].Checksum);
            Assert.AreEqual("myFileLocation", uploadedDataResult[0].FileLocation);
            Assert.AreEqual("myFileName", uploadedDataResult[0].FileName);
            Assert.AreEqual(ud.Id, uploadedDataResult[0].Id);
        }

        [Test]
        public async Task SQL_File_GetUploadedData_doesGetUploadedData()
        {
            UploadedData ud = new UploadedData();

            DbContext.UploadedDatas.Add(ud);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);


            await sut.GetUploadedData(ud.Id);
            List<UploadedData> uploadedDataResult = DbContext.UploadedDatas.AsNoTracking().ToList();


            Assert.NotNull(ud);
            Assert.NotNull(uploadedDataResult);
            Assert.AreEqual(ud.Id, uploadedDataResult[0].Id);
        }

        [Test]
        public async Task SQL_File_DeleteFile_doesFileGetDeleted()
        {
            UploadedData ud = new UploadedData { WorkflowState = Constants.WorkflowStates.Created, Version = 1 };

            DbContext.UploadedDatas.Add(ud);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            await sut.DeleteFile(ud.Id);
            List<UploadedData> uploadedDataResult = DbContext.UploadedDatas.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(ud);
            Assert.NotNull(uploadedDataResult);
            Assert.AreEqual(Constants.WorkflowStates.Removed, uploadedDataResult[0].WorkflowState);
            Assert.AreEqual(2, uploadedDataResult[0].Version);
            Assert.AreEqual(ud.Id, uploadedDataResult[0].Id);
        }

        #endregion

        #region eventhandlers

#pragma warning disable 1998
        public async Task EventCaseCreated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventCaseRetrived(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventCaseCompleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventCaseDeleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventFileDownloaded(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventSiteActivated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }
#pragma warning restore 1998

        #endregion
    }
}