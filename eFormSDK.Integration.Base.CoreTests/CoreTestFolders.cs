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
    [Parallelizable(ParallelScope.Fixtures)]
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
            path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path).Replace(@"file:", "");
            await sut.SetSdkSetting(Settings.fileLocationPicture,
                Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers(ConnectionString);
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

            List<KeyValuePair<string, string>> names = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> descriptions = new List<KeyValuePair<string, string>>();

            names.Add(new KeyValuePair<string, string>("da", folderName));
            descriptions.Add(new KeyValuePair<string, string>("da", folderDescription.Replace("&nbsp;", " ")));
            await sut.FolderCreate(names, descriptions, null).ConfigureAwait(false);

            //Assert

            var folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();
            var folders = DbContext.Folders.AsNoTracking().ToList();
            var folderTranslations = DbContext.FolderTranslations.AsNoTracking().ToList();
            var folderTranslationVersions = DbContext.FolderTranslationVersions.AsNoTracking().ToList();

            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(1, folders.Count);
            Assert.AreEqual(2, folderVersions.Count);
            Assert.AreEqual(1, folderTranslations.Count);
            Assert.AreEqual(1, folderTranslationVersions.Count);

            Assert.AreEqual(null, folders[0].Name);
            Assert.AreEqual(null, folders[0].Description);
            Assert.AreEqual(folders[0].WorkflowState, Constants.WorkflowStates.Created);

            Assert.AreEqual(null, folderVersions[0].Name);
            Assert.AreEqual(null, folderVersions[0].Description);
            Assert.AreEqual(folderVersions[0].WorkflowState, Constants.WorkflowStates.Created);

            Assert.AreEqual(folderName, folderTranslations[0].Name);
            Assert.AreEqual(folderDescription, folderTranslations[0].Description);
            Assert.AreEqual(folderTranslations[0].WorkflowState, Constants.WorkflowStates.Created);

            Assert.AreEqual(folderName, folderTranslationVersions[0].Name);
            Assert.AreEqual(folderDescription, folderTranslationVersions[0].Description);
            Assert.AreEqual(folderTranslationVersions[0].WorkflowState, Constants.WorkflowStates.Created);
        }


        [Test]
        public async Task Core_Folders_CreateFolder_DoesCreateNewFolderWithTranslations()
        {
            // Arrange

            string folderName = Guid.NewGuid().ToString();
            string enfolderName = Guid.NewGuid().ToString();
            string defolderName = Guid.NewGuid().ToString();
            string folderDescription = Guid.NewGuid().ToString();
            string enfolderDescription = Guid.NewGuid().ToString();
            string defolderDescription = Guid.NewGuid().ToString();

            //Act

            List<KeyValuePair<string, string>> names = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> descriptions = new List<KeyValuePair<string, string>>();

            names.Add(new KeyValuePair<string, string>("da", folderName));
            names.Add(new KeyValuePair<string, string>("en-US", enfolderName));
            names.Add(new KeyValuePair<string, string>("de-DE", defolderName));
            descriptions.Add(new KeyValuePair<string, string>("da", folderDescription.Replace("&nbsp;", " ")));
            descriptions.Add(new KeyValuePair<string, string>("en-US", enfolderDescription.Replace("&nbsp;", " ")));
            descriptions.Add(new KeyValuePair<string, string>("de-DE", defolderDescription.Replace("&nbsp;", " ")));
            await sut.FolderCreate(names, descriptions, null).ConfigureAwait(false);

            //Assert

            var folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();
            var folders = DbContext.Folders.AsNoTracking().ToList();
            var folderTranslations = DbContext.FolderTranslations.AsNoTracking().ToList();
            var folderTranslationVersions = DbContext.FolderTranslationVersions.AsNoTracking().ToList();

            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(1, folders.Count);
            Assert.AreEqual(2, folderVersions.Count);
            Assert.AreEqual(3, folderTranslations.Count);
            Assert.AreEqual(3, folderTranslationVersions.Count);

            Assert.AreEqual(null, folders[0].Name);
            Assert.AreEqual(null, folders[0].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folders[0].WorkflowState);

            Assert.AreEqual(null, folderVersions[0].Name);
            Assert.AreEqual(null, folderVersions[0].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderVersions[0].WorkflowState);

            Assert.AreEqual(folderName, folderTranslations[0].Name);
            Assert.AreEqual(folderDescription, folderTranslations[0].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslations[0].WorkflowState);

            Assert.AreEqual(enfolderName, folderTranslations[1].Name);
            Assert.AreEqual(enfolderDescription, folderTranslations[1].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslations[1].WorkflowState);

            Assert.AreEqual(defolderName, folderTranslations[2].Name);
            Assert.AreEqual(defolderDescription, folderTranslations[2].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslations[2].WorkflowState);

            Assert.AreEqual(folderName, folderTranslationVersions[0].Name);
            Assert.AreEqual(folderDescription, folderTranslationVersions[0].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[0].WorkflowState);

            Assert.AreEqual(enfolderName, folderTranslationVersions[1].Name);
            Assert.AreEqual(enfolderDescription, folderTranslationVersions[1].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[1].WorkflowState);

            Assert.AreEqual(defolderName, folderTranslationVersions[2].Name);
            Assert.AreEqual(defolderDescription, folderTranslationVersions[2].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[2].WorkflowState);
        }

        [Test]
        public async Task Core_Folders_CreateSubFolder_DoesCreateSubFolder()
        {
            // Arrange

            string folderName = Guid.NewGuid().ToString();
            string folderDescription = Guid.NewGuid().ToString();

            List<KeyValuePair<string, string>> names = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> descriptions = new List<KeyValuePair<string, string>>();

            names.Add(new KeyValuePair<string, string>("da", folderName));
            descriptions.Add(new KeyValuePair<string, string>("da", folderDescription.Replace("&nbsp;", " ")));
            await sut.FolderCreate(names, descriptions, null).ConfigureAwait(false);

            int firstFolderId = DbContext.Folders.First().Id;

            string subFolderName = Guid.NewGuid().ToString();
            string subFolderDescription = Guid.NewGuid().ToString();


            // Act

            List<KeyValuePair<string, string>> names1 = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> descriptions1 = new List<KeyValuePair<string, string>>();

            names1.Add(new KeyValuePair<string, string>("da", subFolderName));
            descriptions1.Add(new KeyValuePair<string, string>("da", subFolderDescription.Replace("&nbsp;", " ")));
            await sut.FolderCreate(names1, descriptions1, firstFolderId).ConfigureAwait(false);

            var folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();
            var folders = DbContext.Folders.AsNoTracking().ToList();
            var folderTranslations = DbContext.FolderTranslations.AsNoTracking().ToList();
            var folderTranslationVersions = DbContext.FolderTranslationVersions.AsNoTracking().ToList();

            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(2, folders.Count);
            Assert.AreEqual(4, folderVersions.Count);
            Assert.AreEqual(2, folderTranslations.Count);
            Assert.AreEqual(2, folderTranslationVersions.Count);

            Assert.AreEqual(null, folders[0].Name);
            Assert.AreEqual(null, folders[0].Description);

            Assert.AreEqual(null, folders[1].Name);
            Assert.AreEqual(null, folders[1].Description);
            Assert.AreEqual(folders[1].ParentId, firstFolderId);

            Assert.AreEqual(null, folderVersions[0].Name);
            Assert.AreEqual(null, folderVersions[0].Description);
            Assert.AreEqual(null, folderVersions[0].ParentId);

            Assert.AreEqual(null, folderVersions[1].Name);
            Assert.AreEqual(null, folderVersions[1].Description);
            Assert.AreEqual(null, folderVersions[1].ParentId);

            Assert.AreEqual(null, folderVersions[2].Name);
            Assert.AreEqual(null, folderVersions[2].Description);
            Assert.AreEqual(firstFolderId, folderVersions[2].ParentId);

            Assert.AreEqual(null, folderVersions[3].Name);
            Assert.AreEqual(null, folderVersions[3].Description);
            Assert.AreEqual(firstFolderId, folderVersions[3].ParentId);

            Assert.AreEqual(folderName, folderTranslations[0].Name);
            Assert.AreEqual(folderDescription, folderTranslations[0].Description);

            Assert.AreEqual(subFolderName, folderTranslations[1].Name);
            Assert.AreEqual(subFolderDescription, folderTranslations[1].Description);

            Assert.AreEqual(folderName, folderTranslationVersions[0].Name);
            Assert.AreEqual(folderDescription, folderTranslationVersions[0].Description);

            Assert.AreEqual(subFolderName, folderTranslationVersions[1].Name);
            Assert.AreEqual(subFolderDescription, folderTranslationVersions[1].Description);
        }

        [Test]
        public async Task Core_Folders_CreateSubFolder_DoesCreateSubFolderWithTranslations()
        {
            // Arrange

            string folderName = Guid.NewGuid().ToString();
            string enfolderName = Guid.NewGuid().ToString();
            string defolderName = Guid.NewGuid().ToString();
            string folderDescription = Guid.NewGuid().ToString();
            string enfolderDescription = Guid.NewGuid().ToString();
            string defolderDescription = Guid.NewGuid().ToString();

            List<KeyValuePair<string, string>> names = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> descriptions = new List<KeyValuePair<string, string>>();

            names.Add(new KeyValuePair<string, string>("da", folderName));
            names.Add(new KeyValuePair<string, string>("en-US", enfolderName));
            names.Add(new KeyValuePair<string, string>("de-DE", defolderName));
            descriptions.Add(new KeyValuePair<string, string>("da", folderDescription.Replace("&nbsp;", " ")));
            descriptions.Add(new KeyValuePair<string, string>("en-US", enfolderDescription.Replace("&nbsp;", " ")));
            descriptions.Add(new KeyValuePair<string, string>("de-DE", defolderDescription.Replace("&nbsp;", " ")));
            await sut.FolderCreate(names, descriptions, null).ConfigureAwait(false);

            int firstFolderId = DbContext.Folders.First().Id;

            string subFolderName = Guid.NewGuid().ToString();
            string ensubFolderName = Guid.NewGuid().ToString();
            string desubFolderName = Guid.NewGuid().ToString();
            string subFolderDescription = Guid.NewGuid().ToString();
            string ensubFolderDescription = Guid.NewGuid().ToString();
            string desubFolderDescription = Guid.NewGuid().ToString();


            // Act

            List<KeyValuePair<string, string>> names1 = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> descriptions1 = new List<KeyValuePair<string, string>>();

            names1.Add(new KeyValuePair<string, string>("da", subFolderName));
            names1.Add(new KeyValuePair<string, string>("en-US", ensubFolderName));
            names1.Add(new KeyValuePair<string, string>("de-DE", desubFolderName));
            descriptions1.Add(new KeyValuePair<string, string>("da", subFolderDescription.Replace("&nbsp;", " ")));
            descriptions1.Add(new KeyValuePair<string, string>("en-US", ensubFolderDescription.Replace("&nbsp;", " ")));
            descriptions1.Add(new KeyValuePair<string, string>("de-DE", desubFolderDescription.Replace("&nbsp;", " ")));
            await sut.FolderCreate(names1, descriptions1, firstFolderId).ConfigureAwait(false);

            var folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();
            var folders = DbContext.Folders.AsNoTracking().ToList();
            var folderTranslations = DbContext.FolderTranslations.AsNoTracking().ToList();
            var folderTranslationVersions = DbContext.FolderTranslationVersions.AsNoTracking().ToList();

            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(2, folders.Count);
            Assert.AreEqual(4, folderVersions.Count);
            Assert.AreEqual(6, folderTranslations.Count);
            Assert.AreEqual(6, folderTranslationVersions.Count);

            Assert.AreEqual(null, folders[0].Name);
            Assert.AreEqual(null, folders[0].Description);

            Assert.AreEqual(null, folders[1].Name);
            Assert.AreEqual(null, folders[1].Description);
            Assert.AreEqual(firstFolderId, folders[1].ParentId);

            Assert.AreEqual(null, folderVersions[0].Name);
            Assert.AreEqual(null, folderVersions[0].Description);
            Assert.AreEqual(null, folderVersions[0].ParentId);

            Assert.AreEqual(null, folderVersions[1].Name);
            Assert.AreEqual(null, folderVersions[1].Description);
            Assert.AreEqual(null, folderVersions[1].ParentId);

            Assert.AreEqual(null, folderVersions[2].Name);
            Assert.AreEqual(null, folderVersions[2].Description);
            Assert.AreEqual(firstFolderId, folderVersions[2].ParentId);

            Assert.AreEqual(null, folderVersions[3].Name);
            Assert.AreEqual(null, folderVersions[3].Description);
            Assert.AreEqual(firstFolderId, folderVersions[3].ParentId);

            Assert.AreEqual(folderName, folderTranslations[0].Name);
            Assert.AreEqual(folderDescription, folderTranslations[0].Description);

            Assert.AreEqual(enfolderName, folderTranslations[1].Name);
            Assert.AreEqual(enfolderDescription, folderTranslations[1].Description);

            Assert.AreEqual(defolderName, folderTranslations[2].Name);
            Assert.AreEqual(defolderDescription, folderTranslations[2].Description);

            Assert.AreEqual(subFolderName, folderTranslations[3].Name);
            Assert.AreEqual(subFolderDescription, folderTranslations[3].Description);

            Assert.AreEqual(ensubFolderName, folderTranslations[4].Name);
            Assert.AreEqual(ensubFolderDescription, folderTranslations[4].Description);

            Assert.AreEqual(desubFolderName, folderTranslations[5].Name);
            Assert.AreEqual(desubFolderDescription, folderTranslations[5].Description);

            Assert.AreEqual(folderName, folderTranslationVersions[0].Name);
            Assert.AreEqual(folderDescription, folderTranslationVersions[0].Description);

            Assert.AreEqual(enfolderName, folderTranslationVersions[1].Name);
            Assert.AreEqual(enfolderDescription, folderTranslationVersions[1].Description);

            Assert.AreEqual(defolderName, folderTranslationVersions[2].Name);
            Assert.AreEqual(defolderDescription, folderTranslationVersions[2].Description);

            Assert.AreEqual(subFolderName, folderTranslationVersions[3].Name);
            Assert.AreEqual(subFolderDescription, folderTranslationVersions[3].Description);

            Assert.AreEqual(ensubFolderName, folderTranslationVersions[4].Name);
            Assert.AreEqual(ensubFolderDescription, folderTranslationVersions[4].Description);

            Assert.AreEqual(desubFolderName, folderTranslationVersions[5].Name);
            Assert.AreEqual(desubFolderDescription, folderTranslationVersions[5].Description);
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
            Folder folder = new Folder { WorkflowState = Constants.WorkflowStates.Created, MicrotingUid = 23123 };

            await folder.Create(DbContext).ConfigureAwait(false);

            FolderTranslation folderTranslation = new FolderTranslation
            {
                FolderId = folder.Id,
                Name = folderName,
                Description = folderDescription,
                LanguageId = DbContext.Languages.First(x => x.LanguageCode == "da").Id
            };

            await folderTranslation.Create(DbContext);
            //Act

            string newFolderName = Guid.NewGuid().ToString();
            string newDescription = Guid.NewGuid().ToString();

            List<KeyValuePair<string, string>> names1 = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> descriptions1 = new List<KeyValuePair<string, string>>();

            names1.Add(new KeyValuePair<string, string>("da", newFolderName));
            descriptions1.Add(new KeyValuePair<string, string>("da", newDescription.Replace("&nbsp;", " ")));
            await sut.FolderUpdate(folder.Id, names1, descriptions1, null).ConfigureAwait(false);
            //await sut.FolderUpdate(folder.Id, newFolderName, newDescription, null).ConfigureAwait(false);

            var folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();
            var folders = DbContext.Folders.AsNoTracking().ToList();
            var folderTranslations = DbContext.FolderTranslations.AsNoTracking().ToList();
            var folderTranslationVersions = DbContext.FolderTranslationVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(1, folders.Count);
            Assert.AreEqual(1, folderVersions.Count);
            Assert.AreEqual(1, folderTranslations.Count);
            Assert.AreEqual(2, folderTranslationVersions.Count);

            Assert.AreEqual(null, folders[0].Name);
            Assert.AreEqual(null, folders[0].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folders[0].WorkflowState);

            Assert.AreEqual(null, folderVersions[0].Name);
            Assert.AreEqual(null, folderVersions[0].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderVersions[0].WorkflowState);

            Assert.AreEqual(newFolderName, folderTranslations[0].Name);
            Assert.AreEqual(newDescription, folderTranslations[0].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslations[0].WorkflowState);

            Assert.AreEqual(folderName, folderTranslationVersions[0].Name);
            Assert.AreEqual(folderDescription, folderTranslationVersions[0].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[0].WorkflowState);

            Assert.AreEqual(newFolderName, folderTranslationVersions[1].Name);
            Assert.AreEqual(newDescription, folderTranslationVersions[1].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[1].WorkflowState);
        }

        [Test]
        public async Task Core_Folders_UpdateFolder_DoesUpdateFolderWithTranslations()
        {
            // Arrange

            string folderName = Guid.NewGuid().ToString();
            string enfolderName = Guid.NewGuid().ToString();
            string defolderName = Guid.NewGuid().ToString();
            string folderDescription = Guid.NewGuid().ToString();
            string enfolderDescription = Guid.NewGuid().ToString();
            string defolderDescription = Guid.NewGuid().ToString();
            Folder folder = new Folder { WorkflowState = Constants.WorkflowStates.Created, MicrotingUid = 23123 };

            await folder.Create(DbContext).ConfigureAwait(false);

            FolderTranslation folderTranslation = new FolderTranslation
            {
                FolderId = folder.Id,
                Name = folderName,
                Description = folderDescription,
                LanguageId = DbContext.Languages.First(x => x.LanguageCode == "da").Id
            };

            await folderTranslation.Create(DbContext);

            folderTranslation = new FolderTranslation
            {
                FolderId = folder.Id,
                Name = enfolderName,
                Description = enfolderDescription,
                LanguageId = DbContext.Languages.First(x => x.LanguageCode == "en-US").Id
            };

            await folderTranslation.Create(DbContext);

            folderTranslation = new FolderTranslation
            {
                FolderId = folder.Id,
                Name = defolderName,
                Description = defolderDescription,
                LanguageId = DbContext.Languages.First(x => x.LanguageCode == "de-DE").Id
            };

            await folderTranslation.Create(DbContext);
            //Act

            string newFolderName = Guid.NewGuid().ToString();
            string ennewFolderName = Guid.NewGuid().ToString();
            string denewFolderName = Guid.NewGuid().ToString();
            string newDescription = Guid.NewGuid().ToString();
            string ennewDescription = Guid.NewGuid().ToString();
            string denewDescription = Guid.NewGuid().ToString();

            List<KeyValuePair<string, string>> names1 = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> descriptions1 = new List<KeyValuePair<string, string>>();

            names1.Add(new KeyValuePair<string, string>("da", newFolderName));
            names1.Add(new KeyValuePair<string, string>("en-US", ennewFolderName));
            names1.Add(new KeyValuePair<string, string>("de-DE", denewFolderName));
            descriptions1.Add(new KeyValuePair<string, string>("da", newDescription.Replace("&nbsp;", " ")));
            descriptions1.Add(new KeyValuePair<string, string>("en-US", ennewDescription.Replace("&nbsp;", " ")));
            descriptions1.Add(new KeyValuePair<string, string>("de-DE", denewDescription.Replace("&nbsp;", " ")));
            await sut.FolderUpdate(folder.Id, names1, descriptions1, null).ConfigureAwait(false);
            //await sut.FolderUpdate(folder.Id, newFolderName, newDescription, null).ConfigureAwait(false);

            var folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();
            var folders = DbContext.Folders.AsNoTracking().ToList();
            var folderTranslations = DbContext.FolderTranslations.AsNoTracking().ToList();
            var folderTranslationVersions = DbContext.FolderTranslationVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(1, folders.Count);
            Assert.AreEqual(1, folderVersions.Count);
            Assert.AreEqual(3, folderTranslations.Count);
            Assert.AreEqual(6, folderTranslationVersions.Count);

            Assert.AreEqual(null, folders[0].Name);
            Assert.AreEqual(null, folders[0].Description);

            Assert.AreEqual(null, folderVersions[0].Name);
            Assert.AreEqual(null, folderVersions[0].Description);

            Assert.AreEqual(newFolderName, folderTranslations[0].Name);
            Assert.AreEqual(newDescription, folderTranslations[0].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslations[0].WorkflowState);

            Assert.AreEqual(ennewFolderName, folderTranslations[1].Name);
            Assert.AreEqual(ennewDescription, folderTranslations[1].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslations[1].WorkflowState);

            Assert.AreEqual(denewFolderName, folderTranslations[2].Name);
            Assert.AreEqual(denewDescription, folderTranslations[2].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslations[2].WorkflowState);

            Assert.AreEqual(folderName, folderTranslationVersions[0].Name);
            Assert.AreEqual(folderDescription, folderTranslationVersions[0].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[0].WorkflowState);

            Assert.AreEqual(enfolderName, folderTranslationVersions[1].Name);
            Assert.AreEqual(enfolderDescription, folderTranslationVersions[1].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[1].WorkflowState);

            Assert.AreEqual(defolderName, folderTranslationVersions[2].Name);
            Assert.AreEqual(defolderDescription, folderTranslationVersions[2].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[2].WorkflowState);

            Assert.AreEqual(newFolderName, folderTranslationVersions[3].Name);
            Assert.AreEqual(newDescription, folderTranslationVersions[3].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[3].WorkflowState);

            Assert.AreEqual(ennewFolderName, folderTranslationVersions[4].Name);
            Assert.AreEqual(ennewDescription, folderTranslationVersions[4].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[4].WorkflowState);

            Assert.AreEqual(denewFolderName, folderTranslationVersions[5].Name);
            Assert.AreEqual(denewDescription, folderTranslationVersions[5].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[5].WorkflowState);
        }

        [Test]
        public async Task Core_Folders_UpdateFolder_DoesUpdateSingleFolderTranslation()
        {
            // Arrange

            string folderName = Guid.NewGuid().ToString();
            string enfolderName = Guid.NewGuid().ToString();
            string defolderName = Guid.NewGuid().ToString();
            string folderDescription = Guid.NewGuid().ToString();
            string enfolderDescription = Guid.NewGuid().ToString();
            string defolderDescription = Guid.NewGuid().ToString();
            Folder folder = new Folder { WorkflowState = Constants.WorkflowStates.Created, MicrotingUid = 23123 };

            await folder.Create(DbContext).ConfigureAwait(false);

            FolderTranslation folderTranslation = new FolderTranslation
            {
                FolderId = folder.Id,
                Name = folderName,
                Description = folderDescription,
                LanguageId = DbContext.Languages.First(x => x.LanguageCode == "da").Id
            };

            await folderTranslation.Create(DbContext);

            folderTranslation = new FolderTranslation
            {
                FolderId = folder.Id,
                Name = enfolderName,
                Description = enfolderDescription,
                LanguageId = DbContext.Languages.First(x => x.LanguageCode == "en-US").Id
            };

            await folderTranslation.Create(DbContext);

            folderTranslation = new FolderTranslation
            {
                FolderId = folder.Id,
                Name = defolderName,
                Description = defolderDescription,
                LanguageId = DbContext.Languages.First(x => x.LanguageCode == "de-DE").Id
            };

            await folderTranslation.Create(DbContext);
            //Act

            string newFolderName = Guid.NewGuid().ToString();
            string ennewFolderName = Guid.NewGuid().ToString();
            string denewFolderName = Guid.NewGuid().ToString();
            string newDescription = Guid.NewGuid().ToString();
            string ennewDescription = Guid.NewGuid().ToString();
            string denewDescription = Guid.NewGuid().ToString();

            List<KeyValuePair<string, string>> names1 = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> descriptions1 = new List<KeyValuePair<string, string>>();

            names1.Add(new KeyValuePair<string, string>("de-DE", denewFolderName));
            descriptions1.Add(new KeyValuePair<string, string>("de-DE", denewDescription.Replace("&nbsp;", " ")));
            await sut.FolderUpdate(folder.Id, names1, descriptions1, null).ConfigureAwait(false);
            //await sut.FolderUpdate(folder.Id, newFolderName, newDescription, null).ConfigureAwait(false);

            var folderVersions = DbContext.FolderVersions.AsNoTracking().ToList();
            var folders = DbContext.Folders.AsNoTracking().ToList();
            var folderTranslations = DbContext.FolderTranslations.AsNoTracking().ToList();
            var folderTranslationVersions = DbContext.FolderTranslationVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(folders);
            Assert.NotNull(folderVersions);

            Assert.AreEqual(1, folders.Count);
            Assert.AreEqual(1, folderVersions.Count);
            Assert.AreEqual(3, folderTranslations.Count);
            Assert.AreEqual(4, folderTranslationVersions.Count);

            Assert.AreEqual(null, folders[0].Name);
            Assert.AreEqual(null, folders[0].Description);

            Assert.AreEqual(null, folderVersions[0].Name);
            Assert.AreEqual(null, folderVersions[0].Description);

            Assert.AreEqual(folderName, folderTranslations[0].Name);
            Assert.AreEqual(folderDescription, folderTranslations[0].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslations[0].WorkflowState);

            Assert.AreEqual(enfolderName, folderTranslations[1].Name);
            Assert.AreEqual(enfolderDescription, folderTranslations[1].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslations[1].WorkflowState);

            Assert.AreEqual(denewFolderName, folderTranslations[2].Name);
            Assert.AreEqual(denewDescription, folderTranslations[2].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslations[2].WorkflowState);

            Assert.AreEqual(folderName, folderTranslationVersions[0].Name);
            Assert.AreEqual(folderDescription, folderTranslationVersions[0].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[0].WorkflowState);

            Assert.AreEqual(enfolderName, folderTranslationVersions[1].Name);
            Assert.AreEqual(enfolderDescription, folderTranslationVersions[1].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[1].WorkflowState);

            Assert.AreEqual(defolderName, folderTranslationVersions[2].Name);
            Assert.AreEqual(defolderDescription, folderTranslationVersions[2].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[2].WorkflowState);

            Assert.AreEqual(denewFolderName, folderTranslationVersions[3].Name);
            Assert.AreEqual(denewDescription, folderTranslationVersions[3].Description);
            Assert.AreEqual(Constants.WorkflowStates.Created, folderTranslationVersions[3].WorkflowState);
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