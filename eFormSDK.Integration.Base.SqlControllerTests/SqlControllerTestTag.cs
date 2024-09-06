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
using Tag = Microting.eForm.Infrastructure.Data.Entities.Tag;

namespace eFormSDK.Integration.Base.SqlControllerTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SqlControllerTestTag : DbTestFixture
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


        #region tag

        [Test]
        public async Task SQL_Tags_CreateTag_DoesCreateNewTag()
        {
            // Arrance
            string tagName = "Tag1";

            // Act
            await sut.TagCreate(tagName);

            // Assert
            var tag = DbContext.Tags.ToList();

            Assert.That(tagName, Is.EqualTo(tag[0].Name));
            Assert.That(tag.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task SQL_Tags_DeleteTag_DoesMarkTagAsRemoved()
        {
            // Arrance
            string tagName = "Tag1";
            Tag tag = new Tag
            {
                Name = tagName,
                WorkflowState = Constants.WorkflowStates.Created
            };

            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            await sut.TagDelete(tag.Id);

            // Assert
            var result = DbContext.Tags.AsNoTracking().ToList();

            Assert.That(tagName, Is.EqualTo(result[0].Name));
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        }

        [Test]
        public async Task SQL_Tags_CreateTag_DoesRecreateRemovedTag()
        {
            // Arrance
            string tagName = "Tag1";
            Tag tag = new Tag
            {
                Name = tagName,
                WorkflowState = Constants.WorkflowStates.Removed
            };

            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            await sut.TagCreate(tagName);

            // Assert
            var result = DbContext.Tags.AsNoTracking().ToList();

            Assert.That(tagName, Is.EqualTo(result[0].Name));
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        }

        [Test]
        public async Task SQL_Tags_GetAllTags_DoesReturnAllTags()
        {
            // Arrance
            string tagName1 = "Tag1";
            Tag tag = new Tag
            {
                Name = tagName1,
                WorkflowState = Constants.WorkflowStates.Removed
            };

            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            string tagName2 = "Tag2";
            tag = new Tag
            {
                Name = tagName2,
                WorkflowState = Constants.WorkflowStates.Removed
            };


            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            string tagName3 = "Tag3";
            tag = new Tag
            {
                Name = tagName3,
                WorkflowState = Constants.WorkflowStates.Removed
            };


            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            //int tagId3 = await sut.TagCreate(tagName3);

            // Act
            var tags = await sut.GetAllTags(true);

            // Assert
            Assert.That(true, Is.True);
            Assert.That(tags.Count(), Is.EqualTo(3));
            Assert.That(tags[0].Name, Is.EqualTo(tagName1));
            Assert.That(tags[0].TaggingCount, Is.EqualTo(0));
            Assert.That(tags[1].Name, Is.EqualTo(tagName2));
            Assert.That(tags[1].TaggingCount, Is.EqualTo(0));
            Assert.That(tags[2].Name, Is.EqualTo(tagName3));
            Assert.That(tags[2].TaggingCount, Is.EqualTo(0));
        }

        [Test]
        public async Task SQL_Tags_TemplateSetTags_DoesAssignTagToTemplate()
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
            Tag tag = new Tag
            {
                Name = tagName1,
                WorkflowState = Constants.WorkflowStates.Created
            };

            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            List<int> tags = new List<int> { tag.Id };
            await sut.TemplateSetTags(cl1.Id, tags);


            // Assert
            List<Tagging> result = DbContext.Taggings.AsNoTracking().ToList();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result[0].TagId, Is.EqualTo(tag.Id));
            Assert.That(result[0].CheckListId, Is.EqualTo(cl1.Id));
            Assert.That(true, Is.True);
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