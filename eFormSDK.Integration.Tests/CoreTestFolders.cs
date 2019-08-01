using System;
using System.Linq;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class CoreTestFolders : DbTestFixture
    {
        private Core sut;
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

            sut = new Core();
            sut.HandleCaseCreated += EventCaseCreated;
            sut.HandleCaseRetrived += EventCaseRetrived;
            sut.HandleCaseCompleted += EventCaseCompleted;
            sut.HandleCaseDeleted += EventCaseDeleted;
            sut.HandleFileDownloaded += EventFileDownloaded;
            sut.HandleSiteActivated += EventSiteActivated;
            sut.StartSqlOnly(ConnectionString);
            path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:\", "");
            sut.SetSdkSetting(Settings.fileLocationPicture, path + @"\output\dataFolder\picture\");
            sut.SetSdkSetting(Settings.fileLocationPdf, path + @"\output\dataFolder\pdf\");
            sut.SetSdkSetting(Settings.fileLocationJasper, path + @"\output\dataFolder\reports\");
            testHelpers = new TestHelpers();
            //sut.StartLog(new CoreBase());
        }

        #region folder

        [Test]
        public void Core_Folders_CreateFolder_DoesCreateNewFolder()
        {
            // Arrange
            
            string folderName = Guid.NewGuid().ToString();
            string folderDescription = Guid.NewGuid().ToString();

            //Act
            
            sut.FolderCreate(folderName, folderDescription, null);
            
            //Assert

            var folder = DbContext.folders.ToList();
            
            Assert.AreEqual(folder[0].Name, folderName);
            Assert.AreEqual(folder[0].Description, folderDescription);
            Assert.AreEqual(1, folder.Count());
        }

        [Test]
        public void Core_Folders_DeleteFolder_DoesMarkFolderAsRemoved()
        {
            // Arrange
            
            string folderName = Guid.NewGuid().ToString();
            string folderDescription = Guid.NewGuid().ToString();
            folders folder = new folders();
            folder.Name = folderName;
            folder.Description = folderDescription;
            folder.WorkflowState = Constants.WorkflowStates.Created;
            folder.MicrotingUid = 23123;

            folder.Create(DbContext);

            //Act
            
            sut.FolderDelete(folder.Id);
            
            //Assert

            var result = DbContext.folders.AsNoTracking().ToList();
            
            Assert.AreEqual(result[0].Name, folderName);
            Assert.AreEqual(result[0].Description, folderDescription);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Constants.WorkflowStates.Removed, result[0].WorkflowState);
        }

        [Test]
        public void Core_Folders_UpdateFolder_DoesUpdateFolder()
        {
            // Arrange
            
            string folderName = Guid.NewGuid().ToString();
            string folderDescription = Guid.NewGuid().ToString();
            folders folder = new folders();
            folder.Name = folderName;
            folder.Description = folderDescription;
            folder.WorkflowState = Constants.WorkflowStates.Created;
            folder.MicrotingUid = 23123;

            folder.Create(DbContext);

            //Act

            string newFolderName = Guid.NewGuid().ToString();
            string newDescription = Guid.NewGuid().ToString();
            
            sut.FolderUpdate(folder.Id, newFolderName, newDescription, null);
            
            //Assert

            var result = DbContext.folders.AsNoTracking().ToList();
            
            Assert.AreEqual(result[0].Name, newFolderName);
            Assert.AreEqual(result[0].Description, newDescription);
            Assert.AreEqual(1, result.Count());
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