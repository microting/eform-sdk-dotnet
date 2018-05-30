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
    public class SqlControllerTestCheckListSite : DbTestFixture
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
        public void SQL_Case_CheckListSitesCreate_DoesSiteCreate()
        {
            sites site = CreateSite("mySite", 987);

            check_lists cl1 = CreateTemplate("template", "template_desc", "", "", 0, 0);

            //check_list_sites cls1 = CreateCheckListSite(cl1.id, site.id);

            // Act
            sut.CheckListSitesCreate(cl1.id, (int)site.microting_uid, "ServerMicrotingUid");
            List<check_list_sites> checkListSiteResult = DbContext.check_list_sites.AsNoTracking().ToList();
            var versionedMatches = DbContext.check_list_site_versions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(checkListSiteResult);
            Assert.AreEqual(1, checkListSiteResult.Count);
            Assert.AreEqual(Constants.WorkflowStates.Created, checkListSiteResult[0].workflow_state);
            Assert.AreEqual(Constants.WorkflowStates.Created, versionedMatches[0].workflow_state);

        }

        [Test]
        public void SQL_Case_CheckListSitesRead_DoesSiteRead()
        {

            sites site1 = CreateSite("mySite2", 331);
            check_lists cl1 = CreateTemplate("template2", "template_desc", "", "", 1, 1);

            string guid = Guid.NewGuid().ToString();
            string guid2 = Guid.NewGuid().ToString();
            string lastCheckUid1 = Guid.NewGuid().ToString();
            string lastCheckUid2 = Guid.NewGuid().ToString();

            check_list_sites cls1 = CreateCheckListSite(cl1.id, site1.id, guid, Constants.WorkflowStates.Created, lastCheckUid1);
            check_list_sites cls2 = CreateCheckListSite(cl1.id, site1.id, guid2, Constants.WorkflowStates.Removed, lastCheckUid2);


            // Act
            List<string> matches = sut.CheckListSitesRead(cl1.id, (int)site1.microting_uid, Constants.WorkflowStates.NotRemoved);
            List<string> matches2 = sut.CheckListSitesRead(cl1.id, (int)site1.microting_uid, null);
            List<check_list_sites> checkListSiteResult1 = DbContext.check_list_sites.AsNoTracking().ToList();
            var versionedMatches1 = DbContext.check_list_site_versions.AsNoTracking().ToList();


            // Assert
            Assert.NotNull(matches);
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(2, matches2.Count);
            Assert.AreEqual(cls1.microting_uid, matches[0]);
            Assert.AreEqual(cls1.microting_uid, matches2[0]);
            Assert.AreEqual(cls2.microting_uid, matches2[1]);
        }


        [Test]
        public void SQL_Case_CaseDeleteReversed_DoesDeletionReversed()
        {
            // Arrance
            sites site = CreateSite("mySite", 987);

            check_lists cl1 = CreateTemplate("bla", "bla_desc", "", "", 0, 0);

            string guid = Guid.NewGuid().ToString();
            string lastCheckUid1 = Guid.NewGuid().ToString();


            check_list_sites cls1 = CreateCheckListSite(cl1.id, site.id, guid, Constants.WorkflowStates.Created, lastCheckUid1);

            // Act
            sut.CaseDeleteReversed(cls1.microting_uid);
            //Case_Dto caseResult = sut.CaseFindCustomMatchs(aCase.microting_uid);
            List<check_list_sites> checkListSiteResult = DbContext.check_list_sites.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(checkListSiteResult);
            Assert.AreEqual(1, checkListSiteResult.Count);
            Assert.AreEqual(Constants.WorkflowStates.Removed, checkListSiteResult[0].workflow_state);

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