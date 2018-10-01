using eFormCore;
using eFormCore.Helpers;
using eFormData;
using eFormShared;
using eFormSqlController;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    class CoreTestFolders : DbTestFixture
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
            sut.SetPicturePath(path + @"\output\dataFolder\picture\");
            sut.SetPdfPath(path + @"\output\dataFolder\pdf\");
            sut.SetJasperPath(path + @"\output\dataFolder\reports\");
            testHelpers = new TestHelpers();
            //sut.StartLog(new CoreBase());
        }

        [Test]
        public void Core_FolderCreate_CreatesFolder()
        {
            // Arrance
            //entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            folders fo = testHelpers.CreateFolder(Constants.WorkflowStates.Created, 1, DateTime.Now, DateTime.Now, "microting_UUID", "folder1", "A folder", 1, 2, 1, 0);
            // Act
            //sut.EntitySelectItemCreate(eG1.id, "Jon Doe", 0, "");
            sut.FolderCreate("folder1", 0);
            List<folders> folders = DbContext.folders.ToList();

            // Assert
            Assert.AreEqual(1, folders.Count());
        }

        [Test]
        public void Core_FolderDelete_DeletesFolder()
        {
            // Arrance
            entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            entity_items et = testHelpers.CreateEntityItem("", 0, eG1.id, "", "", "Jon Doe", 1, 0, Constants.WorkflowStates.Created);

            // Act
            sut.EntityItemDelete(et.id);
            List<entity_items> items = DbContext.entity_items.ToList();

            // Assert
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(Constants.WorkflowStates.Removed, items[0].workflow_state);
        }

        [Test]
        public void Core_FolderCreateExistingRemovedItem_ChangesWorkflowStateToCreated()
        {
            // Arrance
            entity_groups eG1 = testHelpers.CreateEntityGroup("microtingUIdC1", "EntityGroup1", Constants.FieldTypes.EntitySelect, Constants.WorkflowStates.Created);
            entity_items et = testHelpers.CreateEntityItem("", 0, eG1.id, "", "", "Jon Doe", 1, 0, Constants.WorkflowStates.Removed);

            // Act
            EntityItem result_item = sut.EntitySelectItemCreate(eG1.id, "Jon Doe", 0, "");
            List<entity_items> items = DbContext.entity_items.ToList();

            // Assert
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, items[0].workflow_state);
            Assert.AreEqual(Constants.WorkflowStates.Created, result_item.WorkflowState);
        }

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
