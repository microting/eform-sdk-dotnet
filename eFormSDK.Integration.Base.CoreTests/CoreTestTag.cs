﻿/*
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
using Tag = Microting.eForm.Infrastructure.Data.Entities.Tag;

namespace eFormSDK.Integration.Base.CoreTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class CoreTestTag : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            await sql.SettingUpdate(Settings.comAddressNewApi, "none");

            #endregion

            sut = new Core();
            sut.HandleCaseCreated += EventCaseCreated;
            sut.HandleCaseRetrived += EventCaseRetrived;
            sut.HandleCaseCompleted += EventCaseCompleted;
            sut.HandleCaseDeleted += EventCaseDeleted;
            sut.HandleFileDownloaded += EventFileDownloaded;
            sut.HandleSiteActivated += EventSiteActivated;
            await sut.StartSqlOnly(ConnectionString);
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

        #region tag

        [Test]
        public async Task Core_Tags_CreateTag_DoesCreateNewTag()
        {
            // Arrance
            string tagName = "Tag1";

            // Act
            await sut.TagCreate(tagName);

            // Assert
            var tag = DbContext.Tags.ToList();

            Assert.AreEqual(tag[0].Name, tagName);
            Assert.AreEqual(1, tag.Count());
        }

        [Test]
        public async Task Core_Tags_DeleteTag_DoesMarkTagAsRemoved()
        {
            // Arrance
            string tagName = "Tag1";
            Tag tag = new Tag { Name = tagName, WorkflowState = Constants.WorkflowStates.Created };

            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            await sut.TagDelete(tag.Id);

            // Assert
            var result = DbContext.Tags.AsNoTracking().ToList();

            Assert.AreEqual(result[0].Name, tagName);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Constants.WorkflowStates.Removed, result[0].WorkflowState);
        }

        [Test]
        public async Task Core_Tags_CreateTag_DoesRecreateRemovedTag()
        {
            // Arrance
            string tagName = "Tag1";
            Tag tag = new Tag { Name = tagName, WorkflowState = Constants.WorkflowStates.Removed };

            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            await sut.TagCreate(tagName);

            // Assert
            var result = DbContext.Tags.AsNoTracking().ToList();

            Assert.AreEqual(result[0].Name, tagName);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, result[0].WorkflowState);
        }

        [Test]
        public async Task Core_Tags_GetAllTags_DoesReturnAllTags()
        {
            // Arrance
            string tagName1 = "Tag1";
            Tag tag = new Tag { Name = tagName1, WorkflowState = Constants.WorkflowStates.Removed };

            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            string tagName2 = "Tag2";
            tag = new Tag { Name = tagName2, WorkflowState = Constants.WorkflowStates.Removed };


            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            string tagName3 = "Tag3";
            tag = new Tag { Name = tagName3, WorkflowState = Constants.WorkflowStates.Removed };


            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            //int tagId3 = await sut.TagCreate(tagName3);

            // Act
            var tags = await sut.GetAllTags(true);

            // Assert
            Assert.True(true);
            Assert.AreEqual(3, tags.Count());
            Assert.AreEqual(tagName1, tags[0].Name);
            Assert.AreEqual(0, tags[0].TaggingCount);
            Assert.AreEqual(tagName2, tags[1].Name);
            Assert.AreEqual(0, tags[1].TaggingCount);
            Assert.AreEqual(tagName3, tags[2].Name);
            Assert.AreEqual(0, tags[2].TaggingCount);
        }

        [Test]
        public async Task Core_Tags_TemplateSetTags_DoesAssignTagToTemplate()
        {
            // Arrance
            CheckList cl1 = new CheckList
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Label = "A",
                Description = "D",
                WorkflowState = Constants.WorkflowStates.Created,
                CaseType = "CheckList",
                FolderName = "Template1FolderName",
                DisplayIndex = 1,
                Repeated = 1
            };

            DbContext.CheckLists.Add(cl1);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            string tagName1 = "Tag1";
            Tag tag = new Tag { Name = tagName1, WorkflowState = Constants.WorkflowStates.Created };

            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            List<int> tags = new List<int> { tag.Id };
            await sut.TemplateSetTags(cl1.Id, tags);


            // Assert
            List<Tagging> result = DbContext.Taggings.AsNoTracking().ToList();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(tag.Id, result[0].TagId);
            Assert.AreEqual(cl1.Id, result[0].CheckListId);
            Assert.True(true);
        }


        [Test]
        public async Task Core_Tags_TemplateSetTags_DoesAssignTagToTemplateWithoutDuplicates()
        {
            // Arrance
            CheckList cl1 = new CheckList
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Label = "A",
                Description = "D",
                WorkflowState = Constants.WorkflowStates.Created,
                CaseType = "CheckList",
                FolderName = "Template1FolderName",
                DisplayIndex = 1,
                Repeated = 1
            };

            DbContext.CheckLists.Add(cl1);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            #region Tags

            string tagName1 = "TagFor1CL";
            Tag tag1 = new Tag();
            tag1.Name = tagName1;
            tag1.WorkflowState = Constants.WorkflowStates.Created;

            DbContext.Tags.Add(tag1);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            string tagName2 = "TagFor2CLs";
            Tag tag2 = new Tag();
            tag2.Name = tagName2;
            tag2.WorkflowState = Constants.WorkflowStates.Created;

            DbContext.Tags.Add(tag2);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            string tagName3 = "Tag3";
            Tag tag3 = new Tag();
            tag3.Name = tagName3;
            tag3.WorkflowState = Constants.WorkflowStates.Created;

            DbContext.Tags.Add(tag3);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            string tagName4 = "Tag4";
            Tag tag4 = new Tag();
            tag4.Name = tagName4;
            tag4.WorkflowState = Constants.WorkflowStates.Created;

            DbContext.Tags.Add(tag4);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            #endregion

            // Act
            List<int> tags = new List<int> { tag1.Id, tag3.Id };
            await sut.TemplateSetTags(cl1.Id, tags);
            await sut.TemplateSetTags(cl1.Id, tags);

            //// Assert
            List<Tagging> result = DbContext.Taggings.AsNoTracking().ToList();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(tag1.Id, result[0].TagId);
            Assert.AreEqual(tag3.Id, result[1].TagId);
            Assert.AreEqual(cl1.Id, result[0].CheckListId);
            // Assert.True(true);
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