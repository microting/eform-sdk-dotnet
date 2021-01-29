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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using NUnit.Framework;

namespace eFormSDK.Integration.Base.CoreTests
{
    [TestFixture]
    public class CoreTestFolders : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef").ConfigureAwait(false);
            await sql.SettingUpdate(Settings.firstRunDone, "true").ConfigureAwait(false);
            await sql.SettingUpdate(Settings.knownSitesDone, "true").ConfigureAwait(false);
            #endregion

            sut = new Core();
            sut.HandleCaseCreated += EventCaseCreated;
            sut.HandleCaseRetrived += EventCaseRetrived;
            sut.HandleCaseCompleted += EventCaseCompleted;
            sut.HandleCaseDeleted += EventCaseDeleted;
            sut.HandleFileDownloaded += EventFileDownloaded;
            sut.HandleSiteActivated += EventSiteActivated;
            await sut.StartSqlOnly(ConnectionString).ConfigureAwait(false);
            path = Assembly.GetExecutingAssembly().CodeBase;
            path = Path.GetDirectoryName(path).Replace(@"file:", "");
            await sut.SetSdkSetting(Settings.fileLocationPicture, Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers();
            await testHelpers.GenerateDefaultLanguages();
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

            await sut.FolderCreate(folderName, folderDescription, null).ConfigureAwait(false);

            //Assert

            var folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();
            var folders = DbContext.Folders.AsNoTracking().ToList();

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

            await sut.FolderCreate(folderName, folderDescription, null).ConfigureAwait(false);

            int firstFolderId = DbContext.Folders.First().Id;

            string subFolderName = Guid.NewGuid().ToString();
            string subFolderDescription = Guid.NewGuid().ToString();


            // Act
            await sut.FolderCreate(subFolderName, subFolderDescription, firstFolderId).ConfigureAwait(false);

            var folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();
            var folders = DbContext.Folders.AsNoTracking().ToList();

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
            Folder folder = new Folder();
            folder.Name = folderName;
            folder.Description = folderDescription;
            folder.WorkflowState = Constants.WorkflowStates.Created;
            folder.MicrotingUid = 23123;

            await folder.Create(DbContext).ConfigureAwait(false);

            //Act

            await sut.FolderDelete(folder.Id);

            var folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();
            var folders = DbContext.Folders.AsNoTracking().ToList();

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
            Folder folder = new Folder();
            folder.Name = folderName;
            folder.Description = folderDescription;
            folder.WorkflowState = Constants.WorkflowStates.Created;
            folder.MicrotingUid = 23123;

            await folder.Create(DbContext).ConfigureAwait(false);

            //Act

            string newFolderName = Guid.NewGuid().ToString();
            string newDescription = Guid.NewGuid().ToString();

            await sut.FolderUpdate(folder.Id, newFolderName, newDescription, null).ConfigureAwait(false);

            var folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();
            var folders = DbContext.Folders.AsNoTracking().ToList();

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