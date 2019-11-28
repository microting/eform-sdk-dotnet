using System;
using System.IO;
using System.Threading.Tasks;
using eFormCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;

namespace eFormSDK.Integration.CoreTests
{
    [TestFixture]
    public class CoreTestUploadedData : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;

        public override async Task DoSetup()
        {

            if (sut == null)
            {
                sut = new Core();
                sut.HandleCaseCreated += EventCaseCreated;
                sut.HandleCaseRetrived += EventCaseRetrived;
                sut.HandleCaseCompleted += EventCaseCompleted;
                sut.HandleCaseDeleted += EventCaseDeleted;
                sut.HandleFileDownloaded += EventFileDownloaded;
                sut.HandleSiteActivated += EventSiteActivated;
                await sut.StartSqlOnly(ConnectionString);
            }
            path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:", "");
            await sut.SetSdkSetting(Settings.fileLocationPicture, Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers();
            //await sut.StartLog(new CoreBase());
        }

        #region uploaded_datas
        [Test]
        public async Task Core_UploadedData_UploadedDataRead_DoesReturnOneUploadedDataClass()
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

            dU.CreatedAt = DateTime.Now;
            dU.UpdatedAt = DateTime.Now;
            dU.Extension = extension;
            dU.UploaderId = uploaderId;
            dU.UploaderType = Constants.UploaderTypes.System;
            dU.WorkflowState = Constants.WorkflowStates.PreCreated;
            dU.Version = 1;
            dU.Local = 0;
            dU.FileLocation = fileLocation;
            dU.FileName = fileName;
            dU.CurrentFile = currentFile;
            dU.Checksum = checksum;

            dbContext.uploaded_data.Add(dU);
            await dbContext.SaveChangesAsync();

            UploadedData ud = await sut.Advanced_UploadedDataRead(dU.Id);

            // Assert
            Assert.NotNull(ud);
            Assert.AreEqual(ud.Id, dU.Id);
            Assert.AreEqual(ud.Extension, dU.Extension);
            Assert.AreEqual(ud.UploaderId, dU.UploaderId);
            Assert.AreEqual(ud.UploaderType, dU.UploaderType);
            Assert.AreEqual(ud.FileLocation, dU.FileLocation);
            Assert.AreEqual(ud.FileName, dU.FileName);
            Assert.AreEqual(ud.CurrentFile, dU.CurrentFile);
            Assert.AreEqual(ud.Checksum, dU.Checksum);

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