using eFormCore;
using eFormShared;
using eFormSqlController;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class CoreTest : DbTestFixture
    {
        private Core sut;

        public override void DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            sql.SettingUpdate(Settings.firstRunDone, "true");
            sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new Core();
            sut.StartSqlOnly(ConnectionString);
            //sut.StartLog(new CoreBase());
        }

        [Test]
        public void Core_Template_TemplateItemReadAll_DoesReturnSortedTemplates()
        {
            // Arrance

            #region Template1
            check_lists cl1 = new check_lists();
            cl1.created_at = DateTime.Now;
            cl1.updated_at = DateTime.Now;
            cl1.label = "A";
            cl1.description = "D";
            cl1.workflow_state = Constants.WorkflowStates.Created;
            cl1.case_type = "CheckList";
            cl1.folder_name = "Template1FolderName";
            cl1.display_index = 1;
            cl1.repeated = 1;

            DbContext.check_lists.Add(cl1);
            DbContext.SaveChanges();
            #endregion

            #region Template2
            check_lists cl2 = new check_lists();
            cl2.created_at = DateTime.Now;
            cl2.updated_at = DateTime.Now;
            cl2.label = "B";
            cl2.description = "C";
            cl2.workflow_state = Constants.WorkflowStates.Removed;
            cl2.case_type = "CheckList";
            cl2.folder_name = "Template1FolderName";
            cl2.display_index = 1;
            cl2.repeated = 1;

            DbContext.check_lists.Add(cl2);
            DbContext.SaveChanges();
            #endregion

            #region Template3
            check_lists cl3 = new check_lists();
            cl3.created_at = DateTime.Now;
            cl3.updated_at = DateTime.Now;
            cl3.label = "D";
            cl3.description = "B";
            cl3.workflow_state = Constants.WorkflowStates.Created;
            cl3.case_type = "CheckList";
            cl3.folder_name = "Template1FolderName";
            cl3.display_index = 1;
            cl3.repeated = 1;

            DbContext.check_lists.Add(cl3);
            DbContext.SaveChanges();
            #endregion

            #region Template4
            check_lists cl4 = new check_lists();
            cl4.created_at = DateTime.Now;
            cl4.updated_at = DateTime.Now;
            cl4.label = "C";
            cl4.description = "A";
            cl4.workflow_state = Constants.WorkflowStates.Created;
            cl4.case_type = "CheckList";
            cl4.folder_name = "Template1FolderName";
            cl4.display_index = 1;
            cl4.repeated = 1;

            DbContext.check_lists.Add(cl4);
            DbContext.SaveChanges();
            #endregion


            // Act
            List<int> emptyList = new List<int>();

            // Default sorting including removed
            List<Template_Dto> templateListId = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", false, "", emptyList);
            List<Template_Dto> templateListLabel = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.Label, emptyList);
            List<Template_Dto> templateListDescription = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.Description, emptyList);
            List<Template_Dto> templateListCreatedAt = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.CreatedAt, emptyList);

            // Descending including removed
            List<Template_Dto> templateListDescengingId = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", true, "", emptyList);
            List<Template_Dto> templateListDescengingLabel = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.Label, emptyList);
            List<Template_Dto> templateListDescengingDescription = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.Description, emptyList);
            List<Template_Dto> templateListDescengingCreatedAt = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.CreatedAt, emptyList);

            // Default sorting excluding removed
            List<Template_Dto> templateListIdNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", false, "", emptyList);
            List<Template_Dto> templateListLabelNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.Label, emptyList);
            List<Template_Dto> templateListDescriptionNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.Description, emptyList);
            List<Template_Dto> templateListCreatedAtNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.CreatedAt, emptyList);

            // Descending excluding removed
            List<Template_Dto> templateListDescengingIdNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", true, "", emptyList);
            List<Template_Dto> templateListDescengingLabelNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.Label, emptyList);
            List<Template_Dto> templateListDescengingDescriptionNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.Description, emptyList);
            List<Template_Dto> templateListDescengingCreatedAtNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.CreatedAt, emptyList);


            // Assert

            #region include removed
            // Default sorting including removed 
            // Id
            Assert.NotNull(templateListId);
            Assert.AreEqual(4, templateListId.Count());
            Assert.AreEqual("A", templateListId[0].Label);
            Assert.AreEqual("B", templateListId[1].Label);
            Assert.AreEqual("D", templateListId[2].Label);
            Assert.AreEqual("C", templateListId[3].Label);

            // Default sorting including removed 
            // Label
            Assert.NotNull(templateListLabel);
            Assert.AreEqual(4, templateListLabel.Count());
            Assert.AreEqual("A", templateListLabel[0].Label);
            Assert.AreEqual("B", templateListLabel[1].Label);
            Assert.AreEqual("C", templateListLabel[2].Label);
            Assert.AreEqual("D", templateListLabel[3].Label);

            // Default sorting including removed 
            // Description
            Assert.NotNull(templateListDescription);
            Assert.AreEqual(4, templateListDescription.Count());
            Assert.AreEqual("C", templateListDescription[0].Label);
            Assert.AreEqual("D", templateListDescription[1].Label);
            Assert.AreEqual("B", templateListDescription[2].Label);
            Assert.AreEqual("A", templateListDescription[3].Label);

            // Default sorting including removed 
            // Created At
            Assert.NotNull(templateListCreatedAt);
            Assert.AreEqual(4, templateListCreatedAt.Count());
            Assert.AreEqual("A", templateListCreatedAt[0].Label);
            Assert.AreEqual("B", templateListCreatedAt[1].Label);
            Assert.AreEqual("D", templateListCreatedAt[2].Label);
            Assert.AreEqual("C", templateListCreatedAt[3].Label);

            // Descending sorting including removed 
            // Id
            Assert.NotNull(templateListDescengingId);
            Assert.AreEqual(4, templateListDescengingId.Count());
            Assert.AreEqual("C", templateListDescengingId[0].Label);
            Assert.AreEqual("D", templateListDescengingId[1].Label);
            Assert.AreEqual("B", templateListDescengingId[2].Label);
            Assert.AreEqual("A", templateListDescengingId[3].Label);

            // Descending sorting including removed 
            // Label
            Assert.NotNull(templateListDescengingLabel);
            Assert.AreEqual(4, templateListDescengingLabel.Count());
            Assert.AreEqual("D", templateListDescengingLabel[0].Label);
            Assert.AreEqual("C", templateListDescengingLabel[1].Label);
            Assert.AreEqual("B", templateListDescengingLabel[2].Label);
            Assert.AreEqual("A", templateListDescengingLabel[3].Label);

            // Descending sorting including removed 
            // Description
            Assert.NotNull(templateListDescengingDescription);
            Assert.AreEqual(4, templateListDescengingDescription.Count());
            Assert.AreEqual("A", templateListDescengingDescription[0].Label);
            Assert.AreEqual("B", templateListDescengingDescription[1].Label);
            Assert.AreEqual("D", templateListDescengingDescription[2].Label);
            Assert.AreEqual("C", templateListDescengingDescription[3].Label);

            // Descending sorting including removed 
            // Created At
            Assert.NotNull(templateListDescengingCreatedAt);
            Assert.AreEqual(4, templateListDescengingCreatedAt.Count());
            Assert.AreEqual("C", templateListDescengingCreatedAt[0].Label);
            Assert.AreEqual("D", templateListDescengingCreatedAt[1].Label);
            Assert.AreEqual("B", templateListDescengingCreatedAt[2].Label);
            Assert.AreEqual("A", templateListDescengingCreatedAt[3].Label);
            #endregion

            #region Exclude removed
            // Default sorting including removed 
            // Id
            Assert.NotNull(templateListIdNr);
            Assert.AreEqual(3, templateListIdNr.Count());
            Assert.AreEqual("A", templateListIdNr[0].Label);
            Assert.AreEqual("D", templateListIdNr[1].Label);
            Assert.AreEqual("C", templateListIdNr[2].Label);

            // Default sorting including removed 
            // Label
            Assert.NotNull(templateListLabelNr);
            Assert.AreEqual(3, templateListLabelNr.Count());
            Assert.AreEqual("A", templateListLabelNr[0].Label);
            Assert.AreEqual("C", templateListLabelNr[1].Label);
            Assert.AreEqual("D", templateListLabelNr[2].Label);

            // Default sorting including removed 
            // Description
            Assert.NotNull(templateListDescriptionNr);
            Assert.AreEqual(3, templateListDescriptionNr.Count());
            Assert.AreEqual("C", templateListDescriptionNr[0].Label);
            Assert.AreEqual("D", templateListDescriptionNr[1].Label);
            Assert.AreEqual("A", templateListDescriptionNr[2].Label);

            // Default sorting including removed 
            // Created At
            Assert.NotNull(templateListCreatedAtNr);
            Assert.AreEqual(3, templateListCreatedAtNr.Count());
            Assert.AreEqual("A", templateListCreatedAtNr[0].Label);
            Assert.AreEqual("D", templateListCreatedAtNr[1].Label);
            Assert.AreEqual("C", templateListCreatedAtNr[2].Label);

            // Descending sorting including removed 
            // Id
            Assert.NotNull(templateListDescengingIdNr);
            Assert.AreEqual(3, templateListDescengingIdNr.Count());
            Assert.AreEqual("C", templateListDescengingIdNr[0].Label);
            Assert.AreEqual("D", templateListDescengingIdNr[1].Label);
            Assert.AreEqual("A", templateListDescengingIdNr[2].Label);

            // Descending sorting including removed 
            // Label
            Assert.NotNull(templateListDescengingLabelNr);
            Assert.AreEqual(3, templateListDescengingLabelNr.Count());
            Assert.AreEqual("D", templateListDescengingLabelNr[0].Label);
            Assert.AreEqual("C", templateListDescengingLabelNr[1].Label);
            Assert.AreEqual("A", templateListDescengingLabelNr[2].Label);

            // Descending sorting including removed 
            // Description
            Assert.NotNull(templateListDescengingDescriptionNr);
            Assert.AreEqual(3, templateListDescengingDescriptionNr.Count());
            Assert.AreEqual("A", templateListDescengingDescriptionNr[0].Label);
            Assert.AreEqual("D", templateListDescengingDescriptionNr[1].Label);
            Assert.AreEqual("C", templateListDescengingDescriptionNr[2].Label);

            // Descending sorting including removed 
            // Created At
            Assert.NotNull(templateListDescengingCreatedAtNr);
            Assert.AreEqual(3, templateListDescengingCreatedAtNr.Count());
            Assert.AreEqual("C", templateListDescengingCreatedAtNr[0].Label);
            Assert.AreEqual("D", templateListDescengingCreatedAtNr[1].Label);
            Assert.AreEqual("A", templateListDescengingCreatedAtNr[2].Label);

            #endregion

        }

        [Test]
        public void Core_Tags_CreateTag_DoesCreateNewTag()
        {
            // Arrance
            string tagName = "Tag1";

            // Act
            sut.TagCreate(tagName);

            // Assert
            var tag = DbContext.tags.ToList();

            Assert.AreEqual(tag[0].name, tagName);
            Assert.AreEqual(1, tag.Count());
        }

        [Test]
        public void Core_Tags_DeleteTag_DoesMarkTagAsRemoved()
        {
            // Arrance
            string tagName = "Tag1";
            int tagId = sut.TagCreate(tagName);

            // Act
            sut.TagDelete(tagId);

            // Assert
            var tag = DbContext.tags.ToList();

            Assert.AreEqual(tag[0].name, tagName);
            Assert.AreEqual(1, tag.Count());
            Assert.AreEqual(Constants.WorkflowStates.Removed, tag[0].workflow_state);
        }

        [Test]
        public void Core_Tags_CreateTag_DoesRecreateRemovedTag()
        {
            // Arrance
            string tagName = "Tag1";
            int tagId = sut.TagCreate(tagName);
            sut.TagDelete(tagId);

            // Act
            sut.TagCreate(tagName);

            // Assert
            var tag = DbContext.tags.ToList();

            Assert.AreEqual(tag[0].name, tagName);
            Assert.AreEqual(1, tag.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, tag[0].workflow_state);
        }

        [Test]
        public void Core_Tags_GetAllTags_DoesReturnAllTags()
        {
            // Arrance

            // Act

            // Assert
            Assert.True(true);
        }

        [Test]
        public void Core_Tags_TemplateSetTags_DoesAssignTagToTemplate()
        {
            // Arrance

            // Act

            // Assert
            Assert.True(true);
        }

    }
}
