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
    public class SqlControllerTestCheckListSite : DbTestFixture
    {
        private SqlController sut;
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

            sut = new SqlController(ConnectionString);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            sut.SettingUpdate(Settings.fileLocationPicture, path + @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, path + @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, path + @"\output\dataFolder\reports\");
        }

        [Test]
        public void SQL_Case_CheckListSitesCreate_DoesSiteCreate()
        {
            sites site = testHelpers.CreateSite("mySite", 987);
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "template", "template_desc", "", "", 0, 0);


            // Act
            sut.CheckListSitesCreate(cl1.Id, (int)site.MicrotingUid, "ServerMicrotingUid");
            List<check_list_sites> checkListSiteResult = DbContext.check_list_sites.AsNoTracking().ToList();
            var versionedMatches = DbContext.check_list_site_versions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(checkListSiteResult);
            Assert.AreEqual(1, checkListSiteResult.Count);
            Assert.AreEqual(Constants.WorkflowStates.Created, checkListSiteResult[0].WorkflowState);
            Assert.AreEqual(Constants.WorkflowStates.Created, versionedMatches[0].WorkflowState);

        }

        [Test]
        public void SQL_Case_CheckListSitesRead_DoesSiteRead()
        {

            sites site1 = testHelpers.CreateSite("mySite2", 331);
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "template2", "template_desc", "", "", 1, 1);

            string guid = Guid.NewGuid().ToString();
            string guid2 = Guid.NewGuid().ToString();
            string lastCheckUid1 = Guid.NewGuid().ToString();
            string lastCheckUid2 = Guid.NewGuid().ToString();

            check_list_sites cls1 = testHelpers.CreateCheckListSite(cl1, cl1_Ca, site1, cl1_Ua, 1, Constants.WorkflowStates.Created, lastCheckUid1);
            check_list_sites cls2 = testHelpers.CreateCheckListSite(cl1, cl1_Ca, site1, cl1_Ua, 2, Constants.WorkflowStates.Created, lastCheckUid2);

            // Act
            List<string> matches = sut.CheckListSitesRead(cl1.Id, (int)site1.MicrotingUid, Constants.WorkflowStates.NotRemoved);
            List<string> matches2 = sut.CheckListSitesRead(cl1.Id, (int)site1.MicrotingUid, null);
            List<check_list_sites> checkListSiteResult1 = DbContext.check_list_sites.AsNoTracking().ToList();
            var versionedMatches1 = DbContext.check_list_site_versions.AsNoTracking().ToList();


            // Assert
            Assert.NotNull(matches);
            Assert.AreEqual(2, matches.Count);
            Assert.AreEqual(2, matches2.Count);
            Assert.AreEqual(cls1.MicrotingUid, matches[0]);
            Assert.AreEqual(cls1.MicrotingUid, matches2[0]);
            Assert.AreEqual(cls2.MicrotingUid, matches2[1]);
        }


        [Test]
        public void SQL_Case_CaseDeleteReversed_DoesDeletionReversed()
        {
            // Arrance
            sites site = testHelpers.CreateSite("mySite", 987);
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "bla", "bla_desc", "", "", 0, 0);

            string guid = Guid.NewGuid().ToString();
            string lastCheckUid1 = Guid.NewGuid().ToString();


            check_list_sites cls1 = testHelpers.CreateCheckListSite(cl1, cl1_Ca, site, cl1_Ua, 1, Constants.WorkflowStates.Created, lastCheckUid1);

            // Act
            sut.CaseDeleteReversed(cls1.MicrotingUid);
            List<check_list_sites> checkListSiteResult = DbContext.check_list_sites.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(checkListSiteResult);
            Assert.AreEqual(1, checkListSiteResult.Count);
            Assert.AreEqual(Constants.WorkflowStates.Removed, checkListSiteResult[0].WorkflowState);

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