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

namespace eFormSDK.Integration.Base.CoreTests;

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
        await sql.SettingUpdate(Settings.comAddressNewApi, "none");

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

        Assert.That(folders, Is.Not.EqualTo(null));
        Assert.That(folderVersions, Is.Not.EqualTo(null));

        Assert.That(folders.Count, Is.EqualTo(1));
        Assert.That(folderVersions.Count, Is.EqualTo(2));
        Assert.That(folderTranslations.Count, Is.EqualTo(1));
        Assert.That(folderTranslationVersions.Count, Is.EqualTo(1));

        Assert.That(folders[0].Name, Is.EqualTo(null));
        Assert.That(folders[0].Description, Is.EqualTo(null));
        Assert.That(folders[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderVersions[0].Name, Is.EqualTo(null));
        Assert.That(folderVersions[0].Description, Is.EqualTo(null));
        Assert.That(folderVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslations[0].Name, Is.EqualTo(folderName));
        Assert.That(folderTranslations[0].Description, Is.EqualTo(folderDescription));
        Assert.That(folderTranslations[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[0].Name, Is.EqualTo(folderName));
        Assert.That(folderTranslationVersions[0].Description, Is.EqualTo(folderDescription));
        Assert.That(folderTranslationVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
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

        Assert.That(folders, Is.Not.EqualTo(null));
        Assert.That(folderVersions, Is.Not.EqualTo(null));

        Assert.That(folders.Count, Is.EqualTo(1));
        Assert.That(folderVersions.Count, Is.EqualTo(2));
        Assert.That(folderTranslations.Count, Is.EqualTo(3));
        Assert.That(folderTranslationVersions.Count, Is.EqualTo(3));

        Assert.That(folders[0].Name, Is.EqualTo(null));
        Assert.That(folders[0].Description, Is.EqualTo(null));
        Assert.That(folders[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderVersions[0].Name, Is.EqualTo(null));
        Assert.That(folderVersions[0].Description, Is.EqualTo(null));
        Assert.That(folderVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslations[0].Name, Is.EqualTo(folderName));
        Assert.That(folderTranslations[0].Description, Is.EqualTo(folderDescription));
        Assert.That(folderTranslations[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslations[1].Name, Is.EqualTo(enfolderName));
        Assert.That(folderTranslations[1].Description, Is.EqualTo(enfolderDescription));
        Assert.That(folderTranslations[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslations[2].Name, Is.EqualTo(defolderName));
        Assert.That(folderTranslations[2].Description, Is.EqualTo(defolderDescription));
        Assert.That(folderTranslations[2].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[0].Name, Is.EqualTo(folderName));
        Assert.That(folderTranslationVersions[0].Description, Is.EqualTo(folderDescription));
        Assert.That(folderTranslationVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[1].Name, Is.EqualTo(enfolderName));
        Assert.That(folderTranslationVersions[1].Description, Is.EqualTo(enfolderDescription));
        Assert.That(folderTranslationVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[2].Name, Is.EqualTo(defolderName));
        Assert.That(folderTranslationVersions[2].Description, Is.EqualTo(defolderDescription));
        Assert.That(folderTranslationVersions[2].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
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

        Assert.That(folders, Is.Not.EqualTo(null));
        Assert.That(folderVersions, Is.Not.EqualTo(null));

        Assert.That(folders.Count, Is.EqualTo(2));
        Assert.That(folderVersions.Count, Is.EqualTo(4));
        Assert.That(folderTranslations.Count, Is.EqualTo(2));
        Assert.That(folderTranslationVersions.Count, Is.EqualTo(2));

        Assert.That(folders[0].Name, Is.EqualTo(null));
        Assert.That(folders[0].Description, Is.EqualTo(null));

        Assert.That(folders[1].Name, Is.EqualTo(null));
        Assert.That(folders[1].Description, Is.EqualTo(null));
        Assert.That(firstFolderId, Is.EqualTo(folders[1].ParentId));

        Assert.That(folderVersions[0].Name, Is.EqualTo(null));
        Assert.That(folderVersions[0].Description, Is.EqualTo(null));
        Assert.That(folderVersions[0].ParentId, Is.EqualTo(null));

        Assert.That(folderVersions[1].Name, Is.EqualTo(null));
        Assert.That(folderVersions[1].Description, Is.EqualTo(null));
        Assert.That(folderVersions[1].ParentId, Is.EqualTo(null));

        Assert.That(folderVersions[2].Name, Is.EqualTo(null));
        Assert.That(folderVersions[2].Description, Is.EqualTo(null));
        Assert.That(folderVersions[2].ParentId, Is.EqualTo(firstFolderId));

        Assert.That(folderVersions[3].Name, Is.EqualTo(null));
        Assert.That(folderVersions[3].Description, Is.EqualTo(null));
        Assert.That(folderVersions[3].ParentId, Is.EqualTo(firstFolderId));

        Assert.That(folderTranslations[0].Name, Is.EqualTo(folderName));
        Assert.That(folderTranslations[0].Description, Is.EqualTo(folderDescription));

        Assert.That(folderTranslations[1].Name, Is.EqualTo(subFolderName));
        Assert.That(folderTranslations[1].Description, Is.EqualTo(subFolderDescription));

        Assert.That(folderTranslationVersions[0].Name, Is.EqualTo(folderName));
        Assert.That(folderTranslationVersions[0].Description, Is.EqualTo(folderDescription));

        Assert.That(folderTranslationVersions[1].Name, Is.EqualTo(subFolderName));
        Assert.That(folderTranslationVersions[1].Description, Is.EqualTo(subFolderDescription));
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

        Assert.That(folders, Is.Not.EqualTo(null));
        Assert.That(folderVersions, Is.Not.EqualTo(null));

        Assert.That(folders.Count, Is.EqualTo(2));
        Assert.That(folderVersions.Count, Is.EqualTo(4));
        Assert.That(folderTranslations.Count, Is.EqualTo(6));
        Assert.That(folderTranslationVersions.Count, Is.EqualTo(6));

        Assert.That(folders[0].Name, Is.EqualTo(null));
        Assert.That(folders[0].Description, Is.EqualTo(null));

        Assert.That(folders[1].Name, Is.EqualTo(null));
        Assert.That(folders[1].Description, Is.EqualTo(null));
        Assert.That(folders[1].ParentId, Is.EqualTo(firstFolderId));

        Assert.That(folderVersions[0].Name, Is.EqualTo(null));
        Assert.That(folderVersions[0].Description, Is.EqualTo(null));
        Assert.That(folderVersions[0].ParentId, Is.EqualTo(null));

        Assert.That(folderVersions[1].Name, Is.EqualTo(null));
        Assert.That(folderVersions[1].Description, Is.EqualTo(null));
        Assert.That(folderVersions[1].ParentId, Is.EqualTo(null));

        Assert.That(folderVersions[2].Name, Is.EqualTo(null));
        Assert.That(folderVersions[2].Description, Is.EqualTo(null));
        Assert.That(folderVersions[2].ParentId, Is.EqualTo(firstFolderId));

        Assert.That(folderVersions[3].Name, Is.EqualTo(null));
        Assert.That(folderVersions[3].Description, Is.EqualTo(null));
        Assert.That(folderVersions[3].ParentId, Is.EqualTo(firstFolderId));

        Assert.That(folderTranslations[0].Name, Is.EqualTo(folderName));
        Assert.That(folderTranslations[0].Description, Is.EqualTo(folderDescription));

        Assert.That(folderTranslations[1].Name, Is.EqualTo(enfolderName));
        Assert.That(folderTranslations[1].Description, Is.EqualTo(enfolderDescription));

        Assert.That(folderTranslations[2].Name, Is.EqualTo(defolderName));
        Assert.That(folderTranslations[2].Description, Is.EqualTo(defolderDescription));

        Assert.That(folderTranslations[3].Name, Is.EqualTo(subFolderName));
        Assert.That(folderTranslations[3].Description, Is.EqualTo(subFolderDescription));

        Assert.That(folderTranslations[4].Name, Is.EqualTo(ensubFolderName));
        Assert.That(folderTranslations[4].Description, Is.EqualTo(ensubFolderDescription));

        Assert.That(folderTranslations[5].Name, Is.EqualTo(desubFolderName));
        Assert.That(folderTranslations[5].Description, Is.EqualTo(desubFolderDescription));

        Assert.That(folderTranslationVersions[0].Name, Is.EqualTo(folderName));
        Assert.That(folderTranslationVersions[0].Description, Is.EqualTo(folderDescription));

        Assert.That(folderTranslationVersions[1].Name, Is.EqualTo(enfolderName));
        Assert.That(folderTranslationVersions[1].Description, Is.EqualTo(enfolderDescription));

        Assert.That(folderTranslationVersions[2].Name, Is.EqualTo(defolderName));
        Assert.That(folderTranslationVersions[2].Description, Is.EqualTo(defolderDescription));

        Assert.That(folderTranslationVersions[3].Name, Is.EqualTo(subFolderName));
        Assert.That(folderTranslationVersions[3].Description, Is.EqualTo(subFolderDescription));

        Assert.That(folderTranslationVersions[4].Name, Is.EqualTo(ensubFolderName));
        Assert.That(folderTranslationVersions[4].Description, Is.EqualTo(ensubFolderDescription));

        Assert.That(folderTranslationVersions[5].Name, Is.EqualTo(desubFolderName));
        Assert.That(folderTranslationVersions[5].Description, Is.EqualTo(desubFolderDescription));
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

        Assert.That(folders, Is.Not.EqualTo(null));
        Assert.That(folderVersions, Is.Not.EqualTo(null));

        Assert.That(folders.Count, Is.EqualTo(1));
        Assert.That(folderVersions.Count, Is.EqualTo(2));

        Assert.That(folderName, Is.EqualTo(folders[0].Name));
        Assert.That(folderDescription, Is.EqualTo(folders[0].Description));
        Assert.That(folders[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));


        Assert.That(folders[0].Name, Is.EqualTo(folderVersions[0].Name));
        Assert.That(folders[0].Description, Is.EqualTo(folderVersions[0].Description));
        Assert.That(folderVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));


        Assert.That(folders[0].Name, Is.EqualTo(folderVersions[1].Name));
        Assert.That(folders[0].Description, Is.EqualTo(folderVersions[1].Description));
        Assert.That(folderVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
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

        Assert.That(folders, Is.Not.EqualTo(null));
        Assert.That(folderVersions, Is.Not.EqualTo(null));

        Assert.That(folders.Count, Is.EqualTo(1));
        Assert.That(folderVersions.Count, Is.EqualTo(1));
        Assert.That(folderTranslations.Count, Is.EqualTo(1));
        Assert.That(folderTranslationVersions.Count, Is.EqualTo(2));

        Assert.That(folders[0].Name, Is.EqualTo(null));
        Assert.That(folders[0].Description, Is.EqualTo(null));
        Assert.That(folders[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderVersions[0].Name, Is.EqualTo(null));
        Assert.That(folderVersions[0].Description, Is.EqualTo(null));
        Assert.That(folderVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslations[0].Name, Is.EqualTo(newFolderName));
        Assert.That(folderTranslations[0].Description, Is.EqualTo(newDescription));
        Assert.That(folderTranslations[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[0].Name, Is.EqualTo(folderName));
        Assert.That(folderTranslationVersions[0].Description, Is.EqualTo(folderDescription));
        Assert.That(folderTranslationVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[1].Name, Is.EqualTo(newFolderName));
        Assert.That(folderTranslationVersions[1].Description, Is.EqualTo(newDescription));
        Assert.That(folderTranslationVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
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

        Assert.That(folders, Is.Not.EqualTo(null));
        Assert.That(folderVersions, Is.Not.EqualTo(null));

        Assert.That(folders.Count, Is.EqualTo(1));
        Assert.That(folderVersions.Count, Is.EqualTo(1));
        Assert.That(folderTranslations.Count, Is.EqualTo(3));
        Assert.That(folderTranslationVersions.Count, Is.EqualTo(6));

        Assert.That(folders[0].Name, Is.EqualTo(null));
        Assert.That(folders[0].Description, Is.EqualTo(null));

        Assert.That(folderVersions[0].Name, Is.EqualTo(null));
        Assert.That(folderVersions[0].Description, Is.EqualTo(null));

        Assert.That(folderTranslations[0].Name, Is.EqualTo(newFolderName));
        Assert.That(folderTranslations[0].Description, Is.EqualTo(newDescription));
        Assert.That(folderTranslations[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslations[1].Name, Is.EqualTo(ennewFolderName));
        Assert.That(folderTranslations[1].Description, Is.EqualTo(ennewDescription));
        Assert.That(folderTranslations[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslations[2].Name, Is.EqualTo(denewFolderName));
        Assert.That(folderTranslations[2].Description, Is.EqualTo(denewDescription));
        Assert.That(folderTranslations[2].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[0].Name, Is.EqualTo(folderName));
        Assert.That(folderTranslationVersions[0].Description, Is.EqualTo(folderDescription));
        Assert.That(folderTranslationVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[1].Name, Is.EqualTo(enfolderName));
        Assert.That(folderTranslationVersions[1].Description, Is.EqualTo(enfolderDescription));
        Assert.That(folderTranslationVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[2].Name, Is.EqualTo(defolderName));
        Assert.That(folderTranslationVersions[2].Description, Is.EqualTo(defolderDescription));
        Assert.That(folderTranslationVersions[2].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[3].Name, Is.EqualTo(newFolderName));
        Assert.That(folderTranslationVersions[3].Description, Is.EqualTo(newDescription));
        Assert.That(folderTranslationVersions[3].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[4].Name, Is.EqualTo(ennewFolderName));
        Assert.That(folderTranslationVersions[4].Description, Is.EqualTo(ennewDescription));
        Assert.That(folderTranslationVersions[4].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[5].Name, Is.EqualTo(denewFolderName));
        Assert.That(folderTranslationVersions[5].Description, Is.EqualTo(denewDescription));
        Assert.That(folderTranslationVersions[5].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
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

        Assert.That(folders, Is.Not.EqualTo(null));
        Assert.That(folderVersions, Is.Not.EqualTo(null));

        Assert.That(folders.Count, Is.EqualTo(1));
        Assert.That(folderVersions.Count, Is.EqualTo(1));
        Assert.That(folderTranslations.Count, Is.EqualTo(3));
        Assert.That(folderTranslationVersions.Count, Is.EqualTo(4));

        Assert.That(folders[0].Name, Is.EqualTo(null));
        Assert.That(folders[0].Description, Is.EqualTo(null));

        Assert.That(folderVersions[0].Name, Is.EqualTo(null));
        Assert.That(folderVersions[0].Description, Is.EqualTo(null));

        Assert.That(folderTranslations[0].Name, Is.EqualTo(folderName));
        Assert.That(folderTranslations[0].Description, Is.EqualTo(folderDescription));
        Assert.That(folderTranslations[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslations[1].Name, Is.EqualTo(enfolderName));
        Assert.That(folderTranslations[1].Description, Is.EqualTo(enfolderDescription));
        Assert.That(folderTranslations[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslations[2].Name, Is.EqualTo(denewFolderName));
        Assert.That(folderTranslations[2].Description, Is.EqualTo(denewDescription));
        Assert.That(folderTranslations[2].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[0].Name, Is.EqualTo(folderName));
        Assert.That(folderTranslationVersions[0].Description, Is.EqualTo(folderDescription));
        Assert.That(folderTranslationVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[1].Name, Is.EqualTo(enfolderName));
        Assert.That(folderTranslationVersions[1].Description, Is.EqualTo(enfolderDescription));
        Assert.That(folderTranslationVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[2].Name, Is.EqualTo(defolderName));
        Assert.That(folderTranslationVersions[2].Description, Is.EqualTo(defolderDescription));
        Assert.That(folderTranslationVersions[2].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        Assert.That(folderTranslationVersions[3].Name, Is.EqualTo(denewFolderName));
        Assert.That(folderTranslationVersions[3].Description, Is.EqualTo(denewDescription));
        Assert.That(folderTranslationVersions[3].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
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