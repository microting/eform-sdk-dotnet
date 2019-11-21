using eFormCore;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestCheckListSite : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
        string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:", "");

        public override async Task DoSetup()
        {
            if (sut == null)
            {
                sut = new SqlController(ConnectionString);
                await sut.StartLog(new CoreBase());
            }
            testHelpers = new TestHelpers();
            await sut.SettingUpdate(Settings.fileLocationPicture, Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SettingUpdate(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SettingUpdate(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
        }

        [Test]
        public async Task SQL_Case_CheckListSitesCreate_DoesSiteCreate()
        {
            Random rnd = new Random();
            sites site = await testHelpers.CreateSite("mySite", 987);
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "template", "template_desc", "", "", 0, 0);


            // Act
            await sut.CheckListSitesCreate(cl1.Id, (int)site.MicrotingUid, rnd.Next(1, 255));
            List<check_list_sites> checkListSiteResult = dbContext.check_list_sites.AsNoTracking().ToList();
            var versionedMatches = dbContext.check_list_site_versions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(checkListSiteResult);
            Assert.AreEqual(1, checkListSiteResult.Count);
            Assert.AreEqual(Constants.WorkflowStates.Created, checkListSiteResult[0].WorkflowState);
            Assert.AreEqual(Constants.WorkflowStates.Created, versionedMatches[0].WorkflowState);

        }

        [Test]
        public async Task SQL_Case_CheckListSitesRead_DoesSiteRead()
        {
            Random rnd = new Random();
            sites site1 = await testHelpers.CreateSite("mySite2", 331);
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "template2", "template_desc", "", "", 1, 1);

            string guid = Guid.NewGuid().ToString();
            string guid2 = Guid.NewGuid().ToString();
            int lastCheckUid1 = rnd.Next(1, 255);
            int lastCheckUid2 = rnd.Next(1, 255);

            check_list_sites cls1 = await testHelpers.CreateCheckListSite(cl1, cl1_Ca, site1, cl1_Ua, 1, Constants.WorkflowStates.Created, lastCheckUid1);
            check_list_sites cls2 = await testHelpers.CreateCheckListSite(cl1, cl1_Ca, site1, cl1_Ua, 2, Constants.WorkflowStates.Created, lastCheckUid2);

            // Act
            List<int> matches = await sut.CheckListSitesRead(cl1.Id, (int)site1.MicrotingUid, Constants.WorkflowStates.NotRemoved);
            List<int> matches2 = await sut.CheckListSitesRead(cl1.Id, (int)site1.MicrotingUid, null);
            List<check_list_sites> checkListSiteResult1 = dbContext.check_list_sites.AsNoTracking().ToList();
            var versionedMatches1 = dbContext.check_list_site_versions.AsNoTracking().ToList();


            // Assert
            Assert.NotNull(matches);
            Assert.AreEqual(2, matches.Count);
            Assert.AreEqual(2, matches2.Count);
            Assert.AreEqual(cls1.MicrotingUid, matches[0]);
            Assert.AreEqual(cls1.MicrotingUid, matches2[0]);
            Assert.AreEqual(cls2.MicrotingUid, matches2[1]);
        }


        [Test]
        public async Task SQL_Case_CaseDeleteReversed_DoesDeletionReversed()
        {
            // Arrance
            Random rnd = new Random();
            sites site = await testHelpers.CreateSite("mySite", 987);
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "bla", "bla_desc", "", "", 0, 0);

            string guid = Guid.NewGuid().ToString();
            int lastCheckUid1 = rnd.Next(1, 255);


            check_list_sites cls1 = await testHelpers.CreateCheckListSite(cl1, cl1_Ca, site, cl1_Ua, 1, Constants.WorkflowStates.Created, lastCheckUid1);

            // Act
            await sut.CaseDeleteReversed(cls1.MicrotingUid);
            List<check_list_sites> checkListSiteResult = dbContext.check_list_sites.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(checkListSiteResult);
            Assert.AreEqual(1, checkListSiteResult.Count);
            Assert.AreEqual(Constants.WorkflowStates.Removed, checkListSiteResult[0].WorkflowState);

        }

        
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