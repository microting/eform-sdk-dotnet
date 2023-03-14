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
    public class CoreTestSite : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;
        private DbContextHelper _dbContextHelper;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            _dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(_dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");

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

        #region site

        [Test]
        public async Task Core_Site_SiteCreate_ReturnsSiteId()
        {
            // Arrance


            // Act

            var match = await sut.SiteCreate("John Noname Doe", "John Noname", "Doe", "some_email@invalid.com", "da");

            // Assert
            var sites = DbContext.Sites.AsNoTracking().ToList();

            Assert.NotNull(sites);

            Assert.AreEqual(1, sites.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, sites[0].WorkflowState);
        }

        [Test] //Using communicator, needs httpMock
#pragma warning disable 1998
        public async Task Core_Site_SiteCreate_ReturnSiteId()
        {
            //// Arrange

            //// Act

            //var site = await sut.SiteCreate("site1", "René", "Madsen", "rm@rm.dk");

            //// Assert
            // Assert.NotNull(site);
            // Assert.AreEqual(site.SiteName, "site1");
            // Assert.AreEqual(site.FirstName, "René");
            // Assert.AreEqual(site.LastName, "Madsen");
        }
#pragma warning restore 1998

        [Test]
        public async Task Core_Site_SiteRead_ReturnsFullSite()
        {
            // Arrange
//            #region Template1
//            DateTime cl1_Ca = DateTime.UtcNow;
//            DateTime cl1_Ua = DateTime.UtcNow;
//            check_lists cl1 = await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);
//
//            #endregion
//
//            #region subtemplates
//            #region SubTemplate1
//            check_lists cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl3 = await testHelpers.CreateSubTemplate("A.2", "D.2", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl4 = await testHelpers.CreateSubTemplate("A.3", "D.3", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl5 = await testHelpers.CreateSubTemplate("A.4", "D.4", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl6 = await testHelpers.CreateSubTemplate("A.5", "D.5", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl7 = await testHelpers.CreateSubTemplate("A.6", "D.6", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl8 = await testHelpers.CreateSubTemplate("A.7", "D.7", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl9 = await testHelpers.CreateSubTemplate("A.8", "D.8", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl10 = await testHelpers.CreateSubTemplate("A.9", "D.9", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl11 = await testHelpers.CreateSubTemplate("A.10", "D.10", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//            #endregion
//
//            #region Fields
//            #region field1
//
//
//            fields f1 = await testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field6
//
//            fields f6 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                86, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field7
//
//            fields f7 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                87, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field8
//
//            fields f8 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                88, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field9
//
//            fields f9 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                89, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field10
//
//            fields f10 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                90, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #endregion

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region site_workers

            await testHelpers.CreateSiteWorker(55, site, worker);

            #endregion

            // Act

            var match = await sut.SiteRead((int)site.MicrotingUid);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match.SiteId, site.MicrotingUid);
            Assert.AreEqual(match.SiteName, site.Name);
        }

        [Test]
        public async Task Core_Site_SiteReadAll_ReturnsSites()
        {
            // Arrange
//            #region Template1
//            DateTime cl1_Ca = DateTime.UtcNow;
//            DateTime cl1_Ua = DateTime.UtcNow;
//            check_lists cl1 = await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);
//
//            #endregion
//
//            #region subtemplates
//            #region SubTemplate1
//            check_lists cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl3 = await testHelpers.CreateSubTemplate("A.2", "D.2", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl4 = await testHelpers.CreateSubTemplate("A.3", "D.3", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl5 = await testHelpers.CreateSubTemplate("A.4", "D.4", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl6 = await testHelpers.CreateSubTemplate("A.5", "D.5", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl7 = await testHelpers.CreateSubTemplate("A.6", "D.6", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl8 = await testHelpers.CreateSubTemplate("A.7", "D.7", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl9 = await testHelpers.CreateSubTemplate("A.8", "D.8", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl10 = await testHelpers.CreateSubTemplate("A.9", "D.9", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl11 = await testHelpers.CreateSubTemplate("A.10", "D.10", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//            #endregion
//
//            #region Fields
//            #region field1
//
//
//            fields f1 = await testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field6
//
//            fields f6 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                86, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field7
//
//            fields f7 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                87, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field8
//
//            fields f8 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                88, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field9
//
//            fields f9 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                89, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field10
//
//            fields f10 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                90, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #endregion

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region site_workers

            SiteWorker siteWorker = await testHelpers.CreateSiteWorker(55, site, worker);

            #endregion

            // Act
            var matchNotRemoved = await sut.SiteReadAll(false);
            var matchInclRemoved = await sut.SiteReadAll(true);
            // Assert
            Assert.NotNull(matchInclRemoved);
            Assert.NotNull(matchNotRemoved);

            Assert.AreEqual(matchInclRemoved.Count, 1);
            Assert.AreEqual(matchNotRemoved.Count, 1);
        }

        [Test] //Using Communicatorn needs httpMock
        public async Task Core_Site_SiteReset_ReturnsSite()
        {
            // Arrange

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            // Act

            // Assert
        }

        [Test] //Using Communicatorn needs httpMock
        public async Task Core_Site_SiteUpdate_returnsTrue()
        {
            // Arrange
            // Arrange

            #region site

            string siteName = Guid.NewGuid().ToString();
            int siteMicrotingUid = 1; // This needs to be 1 for our tests to pass through the FakeHttp
            // TODO: Improve the test for supporting random id.

            Site site = await testHelpers.CreateSite(siteName, siteMicrotingUid);
            SiteNameDto siteName_Dto = new SiteNameDto
            {
                SiteUId = (int)site.MicrotingUid,
                SiteName = site.Name,
                CreatedAt = site.CreatedAt,
                UpdatedAt = site.UpdatedAt
            };

            #endregion

            #region worker

            string email = Guid.NewGuid().ToString();
            string firstName = Guid.NewGuid().ToString();
            string lastName = Guid.NewGuid().ToString();
            //int workerMicrotingUid = await testHelpers.GetRandomInt();
            int workerMicrotingUid = 1; // This needs to be 1 for our tests to pass through the FakeHttp
            // TODO: Improve the test for supporting random id.

            Worker worker = await testHelpers.CreateWorker(email, firstName, lastName, workerMicrotingUid);

            #endregion

            #region site_worker

            SiteWorker siteWorker = await testHelpers.CreateSiteWorker(1, site, worker);

            #endregion

            #region unit

            Unit unit = await testHelpers.CreateUnit(1, 1, site, 1);

            #endregion

            var match = await sut.SiteUpdate((int)site.MicrotingUid, site.Name, firstName, lastName, email, "da");

            // Assert
            await using MicrotingDbContext dbContext = _dbContextHelper.GetDbContext();
            var items = await dbContext.EntityItems.CountAsync();
            var itemVersions = await dbContext.EntityItemVersions.CountAsync();
            var groups = await dbContext.EntityGroups.CountAsync();
            var groupVersions = await dbContext.EntityGroupVersions.CountAsync();

            Assert.AreEqual(2, items);
            Assert.AreEqual(2, itemVersions);
            Assert.AreEqual(2, groups);
            Assert.AreEqual(4, groupVersions);

            Assert.True(match);
        }

        [Test] //Using Communicatorn needs httpMock
        public async Task Core_Site_SiteDelete_ReturnsTrue()
        {
            // Arrange
            string siteName = Guid.NewGuid().ToString();
            int siteMicrotingUid = 1; // This needs to be 1 for our tests to pass through the FakeHttp
            // TODO: Improve the test for supporting random id.

            Site site = await testHelpers.CreateSite(siteName, siteMicrotingUid);
            SiteNameDto siteName_Dto = new SiteNameDto
            {
                SiteUId = (int)site.MicrotingUid,
                SiteName = site.Name,
                CreatedAt = site.CreatedAt,
                UpdatedAt = site.UpdatedAt
            };

            await using MicrotingDbContext db = _dbContextHelper.GetDbContext();
            EntityGroup entityGroup = new EntityGroup
            {
                Editable = true,
                Locked = false,
                Name = "Device users",
                Type = Constants.FieldTypes.EntitySearch
            };

            await entityGroup.Create(db);

            EntityItem entityItem = new EntityItem
            {
                Name = siteName,
                EntityGroupId = entityGroup.Id
            };
            await entityItem.Create(db);
            site.SearchableEntityItemId = entityItem.Id;
            await site.Update(db);

            entityGroup = new EntityGroup
            {
                Editable = true,
                Locked = false,
                Name = "Device users",
                Type = Constants.FieldTypes.EntitySelect
            };

            await entityGroup.Create(db);

            entityItem = new EntityItem
            {
                Name = siteName,
                EntityGroupId = entityGroup.Id
            };
            await entityItem.Create(db);
            site.SelectableEntityItemId = entityItem.Id;
            await site.Update(db);

            #region worker

            string email = Guid.NewGuid().ToString();
            string firstName = Guid.NewGuid().ToString();
            string lastName = Guid.NewGuid().ToString();
            //int workerMicrotingUid = await testHelpers.GetRandomInt();
            int workerMicrotingUid = 1; // This needs to be 1 for our tests to pass through the FakeHttp
            // TODO: Improve the test for supporting random id.

            Worker worker = await testHelpers.CreateWorker(email, firstName, lastName, workerMicrotingUid);

            #endregion

            #region site_worker

            SiteWorker siteWorker = await testHelpers.CreateSiteWorker(1, site, worker);

            #endregion

            #region unit

            Unit unit = await testHelpers.CreateUnit(1, 1, site, 1);

            #endregion

            // Act
            var match = await sut.SiteDelete((int)site.MicrotingUid);
            // Assert
            Assert.True(match);
            //#endregion
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