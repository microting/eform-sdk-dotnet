using eFormCore;
using eFormCore.Helpers;
using eFormData;
using eFormShared;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestUploadedData : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
        private string path;

        public override void DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            sql.SettingUpdate(Settings.firstRunDone, "true");
            sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(ConnectionString);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            sut.SettingUpdate(Settings.fileLocationPicture, path + @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, path + @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, path + @"\output\dataFolder\reports\");
        }

        #region uploaded_data
        [Test]
        public void SQL_UploadedData_FileRead_DoesReturnOneUploadedData()
        {
            // Arrance
            string checksum = "";
            string extension = "jpg";
            string currentFile = "Hello.jpg";
            int uploaderId = 1;
            string fileLocation = @"c:\here";
            string fileName = "Hello.jpg";

            // Act
            uploaded_data dU = new uploaded_data();

            dU.created_at = DateTime.Now;
            dU.updated_at = DateTime.Now;
            dU.extension = extension;
            dU.uploader_id = uploaderId;
            dU.uploader_type = Constants.UploaderTypes.System;
            dU.workflow_state = Constants.WorkflowStates.PreCreated;
            dU.version = 1;
            dU.local = 0;
            dU.file_location = fileLocation;
            dU.file_name = fileName;
            dU.current_file = currentFile;
            dU.checksum = checksum;

            DbContext.uploaded_data.Add(dU);
            DbContext.SaveChanges();

            UploadedData ud = sut.FileRead();

            // Assert
            Assert.NotNull(ud);
            Assert.AreEqual(dU.id, ud.Id);
            Assert.AreEqual(dU.checksum, ud.Checksum);
            Assert.AreEqual(dU.extension, ud.Extension);
            Assert.AreEqual(dU.current_file, ud.CurrentFile);
            Assert.AreEqual(dU.uploader_id, ud.UploaderId);
            Assert.AreEqual(dU.uploader_type, ud.UploaderType);
            Assert.AreEqual(dU.file_location, ud.FileLocation);
            Assert.AreEqual(dU.file_name, ud.FileName);
            // Assert.AreEqual(dU.local, ud.);

        }

        [Test]
        public void SQL_UploadedData_UploadedDataRead_DoesReturnOneUploadedDataClass()
        {
            // Arrance
            string checksum = "";
            string extension = "jpg";
            string currentFile = "Hello.jpg";
            int uploaderId = 1;
            string fileLocation = @"c:\here";
            string fileName = "Hello.jpg";

            // Act
            uploaded_data dU = new uploaded_data();

            dU.created_at = DateTime.Now;
            dU.updated_at = DateTime.Now;
            dU.extension = extension;
            dU.uploader_id = uploaderId;
            dU.uploader_type = Constants.UploaderTypes.System;
            dU.workflow_state = Constants.WorkflowStates.PreCreated;
            dU.version = 1;
            dU.local = 0;
            dU.file_location = fileLocation;
            dU.file_name = fileName;
            dU.current_file = currentFile;
            dU.checksum = checksum;

            DbContext.uploaded_data.Add(dU);
            DbContext.SaveChanges();

            uploaded_data ud = sut.GetUploadedData(dU.id);

            // Assert
            Assert.NotNull(ud);
            Assert.AreEqual(ud.id, dU.id);
            Assert.AreEqual(ud.extension, dU.extension);
            Assert.AreEqual(ud.uploader_id, dU.uploader_id);
            Assert.AreEqual(ud.uploader_type, dU.uploader_type);
            Assert.AreEqual(ud.workflow_state, dU.workflow_state);
            Assert.AreEqual(ud.version, 1);
            Assert.AreEqual(ud.local, 0);
            Assert.AreEqual(ud.file_location, dU.file_location);
            Assert.AreEqual(ud.file_name, dU.file_name);
            Assert.AreEqual(ud.current_file, dU.current_file);
            Assert.AreEqual(ud.checksum, dU.checksum);

        }


        [Test]
        public void SQL_File_FileRead_doesFileRead()
        {
            uploaded_data ud = new uploaded_data();

            ud.checksum = "checksum1";
            ud.extension = "extension";
            ud.current_file = "currentFile1";
            ud.uploader_id = 223;
            ud.uploader_type = "uploader_type";
            ud.file_location = "file_location";
            ud.file_name = "fileName";
            //ud.id = 111;

            ud.workflow_state = Constants.WorkflowStates.PreCreated;



            DbContext.uploaded_data.Add(ud);
            DbContext.SaveChanges();


            // Act
            UploadedData Ud = sut.FileRead();


            // Assert

            Assert.NotNull(ud);
            Assert.NotNull(Ud);
            Assert.AreEqual(Ud.Checksum, ud.checksum);
            Assert.AreEqual(Ud.Extension, ud.extension);
            Assert.AreEqual(Ud.CurrentFile, ud.current_file);
            Assert.AreEqual(Ud.UploaderId, ud.uploader_id);
            Assert.AreEqual(Ud.UploaderType, ud.uploader_type);
            Assert.AreEqual(Ud.FileLocation, ud.file_location);
            Assert.AreEqual(Ud.FileName, ud.file_name);
            Assert.AreEqual(Ud.Id, ud.id);
            Assert.AreEqual(Constants.WorkflowStates.PreCreated, ud.workflow_state);




        }

        [Test]
        public void SQL_File_FileCaseFindMUId_doesFindMUId()
        {
            sites site1 = testHelpers.CreateSite("MySite", 22);
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "template1", "template_desc", "", "", 1, 1);

            string guid = Guid.NewGuid().ToString();


            DateTime c1_ca = DateTime.Now.AddDays(-9);
            DateTime c1_da = DateTime.Now.AddDays(-8).AddHours(-12);
            DateTime c1_ua = DateTime.Now.AddDays(-8);
            workers worker = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
            site_workers site_workers = testHelpers.CreateSiteWorker(55, site1, worker);
            units unit = testHelpers.CreateUnit(48, 49, site1, 348);

            string microtingUId = Guid.NewGuid().ToString();
            string microtingCheckId = Guid.NewGuid().ToString();
            cases aCase1 = testHelpers.CreateCase("case1UId", cl1, c1_ca, "custom1",
                c1_da, worker, "microtingCheckUId1", "microtingUId1",
               site1, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);

            uploaded_data ud = new uploaded_data();

            ud.checksum = "checksum1";
            ud.extension = "extension";
            ud.current_file = "currentFile1";
            ud.uploader_id = 223;
            ud.uploader_type = "uploader_type";
            ud.file_location = "url";
            ud.file_name = "fileName";
            //ud.id = 111;

            DbContext.uploaded_data.Add(ud);
            DbContext.SaveChanges();

            field_values fVs = new field_values();
            fVs.uploaded_data_id = ud.id;
            fVs.case_id = aCase1.id;

            DbContext.field_values.Add(fVs);
            DbContext.SaveChanges();


            // Act
            sut.FileCaseFindMUId("url");


            Assert.NotNull(fVs);
            Assert.AreEqual(fVs.case_id, aCase1.id);

        }

        [Test]
        public void SQL_File_FileProcessed_isProcessed()
        {
            uploaded_data ud = new uploaded_data();


            ud.local = 0;
            ud.workflow_state = Constants.WorkflowStates.PreCreated;
            ud.version = 1;

            DbContext.uploaded_data.Add(ud);
            DbContext.SaveChanges();


            // Act
            sut.FileProcessed("url", "myChecksum", "myFileLocation", "myFileName", ud.id);
            List<uploaded_data> uploadedDataResult = DbContext.uploaded_data.AsNoTracking().ToList();
            //var versionedMatches = DbContext.uploaded_data_versions.AsNoTracking().ToList(); TODO 05/01/2018

            // Assert

            Assert.NotNull(uploadedDataResult);
            Assert.NotNull(ud);
            Assert.AreEqual(Constants.WorkflowStates.Created, uploadedDataResult[0].workflow_state);
            Assert.AreEqual(1, uploadedDataResult[0].local);
            Assert.AreEqual(2, uploadedDataResult[0].version);
            Assert.AreEqual("myChecksum", uploadedDataResult[0].checksum);
            Assert.AreEqual("myFileLocation", uploadedDataResult[0].file_location);
            Assert.AreEqual("myFileName", uploadedDataResult[0].file_name);
            Assert.AreEqual(ud.id, uploadedDataResult[0].id);

        }

        [Test]
        public void SQL_File_GetUploadedData_doesGetUploadedData()
        {
            uploaded_data ud = new uploaded_data();

            DbContext.uploaded_data.Add(ud);
            DbContext.SaveChanges();


            sut.GetUploadedData(ud.id);
            List<uploaded_data> uploadedDataResult = DbContext.uploaded_data.AsNoTracking().ToList();


            Assert.NotNull(ud);
            Assert.NotNull(uploadedDataResult);
            Assert.AreEqual(ud.id, uploadedDataResult[0].id);

        }

        [Test]
        public void SQL_File_DeleteFile_doesFileGetDeleted()
        {
            uploaded_data ud = new uploaded_data();

            ud.workflow_state = Constants.WorkflowStates.Created;
            ud.version = 1;
            DbContext.uploaded_data.Add(ud);
            DbContext.SaveChanges();

            // Act
            sut.DeleteFile(ud.id);
            List<uploaded_data> uploadedDataResult = DbContext.uploaded_data.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(ud);
            Assert.NotNull(uploadedDataResult);
            Assert.AreEqual(Constants.WorkflowStates.Removed, uploadedDataResult[0].workflow_state);
            Assert.AreEqual(2, uploadedDataResult[0].version);
            Assert.AreEqual(ud.id, uploadedDataResult[0].id);
        }

        #endregion

        #region eventhandlers
        public void EventCaseCreated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseRetrived(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseCompleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseDeleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventFileDownloaded(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventSiteActivated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }
        #endregion
    }

}