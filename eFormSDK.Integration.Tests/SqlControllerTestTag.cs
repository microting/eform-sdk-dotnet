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
    public class SqlControllerTestTag : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

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
            sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }


        #region tag
        [Test]
        public void SQL_Tags_CreateTag_DoesCreateNewTag()
        {
            // Arrance
            string tagName = "Tag1";

            // Act
            sut.TagCreate(tagName);

            // Assert
            var tag = DbContext.tags.ToList();

            Assert.AreEqual(tag[0].Name, tagName);
            Assert.AreEqual(1, tag.Count());
        }

        [Test]
        public void SQL_Tags_DeleteTag_DoesMarkTagAsRemoved()
        {
            // Arrance
            string tagName = "Tag1";
            tags tag = new tags();
            tag.Name = tagName;
            tag.WorkflowState = Constants.WorkflowStates.Created;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();

            // Act
            sut.TagDelete(tag.Id);

            // Assert
            var result = DbContext.tags.AsNoTracking().ToList();

            Assert.AreEqual(result[0].Name, tagName);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Constants.WorkflowStates.Removed, result[0].WorkflowState);
        }

        [Test]
        public void SQL_Tags_CreateTag_DoesRecreateRemovedTag()
        {
            // Arrance
            string tagName = "Tag1";
            tags tag = new tags();
            tag.Name = tagName;
            tag.WorkflowState = Constants.WorkflowStates.Removed;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();

            // Act
            sut.TagCreate(tagName);

            // Assert
            var result = DbContext.tags.AsNoTracking().ToList();

            Assert.AreEqual(result[0].Name, tagName);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, result[0].WorkflowState);
        }

        [Test]
        public void SQL_Tags_GetAllTags_DoesReturnAllTags()
        {
            // Arrance
            string tagName1 = "Tag1";
            tags tag = new tags();
            tag.Name = tagName1;
            tag.WorkflowState = Constants.WorkflowStates.Removed;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();

            string tagName2 = "Tag2";
            tag = new tags();

            tag.Name = tagName2;
            tag.WorkflowState = Constants.WorkflowStates.Removed;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();
            string tagName3 = "Tag3";
            tag = new tags();

            tag.Name = tagName3;
            tag.WorkflowState = Constants.WorkflowStates.Removed;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();
            //int tagId3 = sut.TagCreate(tagName3);

            // Act
            var tags = sut.GetAllTags(true);

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
        public void SQL_Tags_TemplateSetTags_DoesAssignTagToTemplate()
        {
            // Arrance
            check_lists cl1 = new check_lists();
            cl1.CreatedAt = DateTime.Now;
            cl1.UpdatedAt = DateTime.Now;
            cl1.Label = "A";
            cl1.Description = "D";
            cl1.WorkflowState = Constants.WorkflowStates.Created;
            cl1.CaseType = "CheckList";
            cl1.FolderName = "Template1FolderName";
            cl1.DisplayIndex = 1;
            cl1.Repeated = 1;

            DbContext.check_lists.Add(cl1);
            DbContext.SaveChanges();

            string tagName1 = "Tag1";
            tags tag = new tags();
            tag.Name = tagName1;
            tag.WorkflowState = Constants.WorkflowStates.Created;

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();

            // Act
            List<int> tags = new List<int>();
            tags.Add(tag.Id);
            sut.TemplateSetTags(cl1.Id, tags);


            // Assert
            List<taggings> result = DbContext.taggings.AsNoTracking().ToList();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(tag.Id, result[0].TagId);
            Assert.AreEqual(cl1.Id, result[0].CheckListId);
            Assert.True(true);
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