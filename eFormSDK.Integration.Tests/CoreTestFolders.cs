using System;
using System.IO;
using System.Linq;
using eFormCore;
using System.Threading.Tasks;
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
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:", "");
            sut.SetSdkSetting(Settings.fileLocationPicture, Path.Combine(path, "output", "dataFolder", "picture"));
            sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers();
            //sut.StartLog(new CoreBase());
        }

        #region folder

        [Test]
        public async Task Core_Folders_CreateFolder_DoesCreateNewFolder()
        {
            // Arrange
            
            string folderName = Guid.NewGuid().ToString();
            string folderDescription = Guid.NewGuid().ToString();

            //Act
            
            sut.FolderCreate(folderName, folderDescription, null);
            
            //Assert

            var folderVersions = DbContext.folder_versions.AsNoTracking().ToList();
            var folders = DbContext.folders.AsNoTracking().ToList();
            
            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(1, folders.Count);
            Assert.AreEqual(1, folderVersions.Count);
            
            Assert.AreEqual(folders[0].Name, folderName);
            Assert.AreEqual(folders[0].Description, folderDescription);
            Assert.AreEqual(folders[0].WorkflowState, Constants.WorkflowStates.Created);

            
            Assert.AreEqual(folderVersions[0].Name, folders[0].Name);
            Assert.AreEqual(folderVersions[0].Description, folders[0].Description);
            Assert.AreEqual(folderVersions[0].WorkflowState, Constants.WorkflowStates.Created);

        }

        [Test]
        public async Task Core_Folders_CreateSubFolder_DoesCreateSubFolder()
        {
            // Arrange
            
            string folderName = Guid.NewGuid().ToString();
            string folderDescription = Guid.NewGuid().ToString();
            
            sut.FolderCreate(folderName, folderDescription, null);

            int firstFolderId = DbContext.folders.First().Id;
            
            string subFolderName = Guid.NewGuid().ToString();
            string subFolderDescription = Guid.NewGuid().ToString();
            
            
            // Act
            sut.FolderCreate(subFolderName, subFolderDescription, firstFolderId);

            var folderVersions = DbContext.folder_versions.AsNoTracking().ToList();
            var folders = DbContext.folders.AsNoTracking().ToList();
            
            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(2, folders.Count);
            Assert.AreEqual(2, folderVersions.Count);
            
            Assert.AreEqual(folders[0].Name, folderName);
            Assert.AreEqual(folders[0].Description, folderDescription);

            Assert.AreEqual(folders[1].Name, subFolderName);
            Assert.AreEqual(folders[1].Description, subFolderDescription);
            Assert.AreEqual(folders[1].ParentId, firstFolderId);
            
            Assert.AreEqual(folderVersions[0].Name, folders[0].Name);
            Assert.AreEqual(folderVersions[0].Description, folders[0].Description);
            
            Assert.AreEqual(folderVersions[1].Name, folders[1].Name);
            Assert.AreEqual(folderVersions[1].Description, folders[1].Description);
            Assert.AreEqual(folderVersions[1].ParentId, firstFolderId);
        }

        [Test]
        public async Task Core_Folders_DeleteFolder_DoesMarkFolderAsRemoved()
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
            
            var folderVersions = DbContext.folder_versions.AsNoTracking().ToList();
            var folders = DbContext.folders.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(1, folders.Count);
            Assert.AreEqual(2, folderVersions.Count);
            
            Assert.AreEqual(folders[0].Name, folderName);
            Assert.AreEqual(folders[0].Description, folderDescription);
            Assert.AreEqual(folders[0].WorkflowState, Constants.WorkflowStates.Removed);


            Assert.AreEqual(folderVersions[0].Name, folders[0].Name);
            Assert.AreEqual(folderVersions[0].Description, folders[0].Description);
            Assert.AreEqual(folderVersions[0].WorkflowState, Constants.WorkflowStates.Created);

            
            Assert.AreEqual(folderVersions[1].Name, folders[0].Name);
            Assert.AreEqual(folderVersions[1].Description, folders[0].Description);
            Assert.AreEqual(folderVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
        }

        [Test]
        public async Task Core_Folders_UpdateFolder_DoesUpdateFolder()
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
            
            var folderVersions = DbContext.folder_versions.AsNoTracking().ToList();
            var folders = DbContext.folders.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(1, folders.Count);
            Assert.AreEqual(2, folderVersions.Count);
            
            Assert.AreEqual(folders[0].Name, newFolderName);
            Assert.AreEqual(folders[0].Description, newDescription);


            Assert.AreEqual(folderVersions[0].Name, folder.Name);
            Assert.AreEqual(folderVersions[0].Description, folder.Description);
            
            Assert.AreEqual(folderVersions[1].Name, folders[0].Name);
            Assert.AreEqual(folderVersions[1].Description, folders[0].Description);
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