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
    public class CoreTestSite : DbTestFixture
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

        #region site
        [Test]
        public void Core_Site_SiteCreate_ReturnsSiteId()
        {
            // Arrance


            // Act

            var match = sut.SiteCreate("John Noname Doe", "John Noname", "Doe", "some_email@invalid.com");

            // Assert
            var sites = DbContext.sites.AsNoTracking().ToList();

            Assert.NotNull(sites);

            Assert.AreEqual(1, sites.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, sites[0].workflow_state);

        }
        [Test]//Using communicator, needs httpMock
        public void Core_Site_SiteCreate_ReturnSiteId()
        {
            ////Arrance

            ////Act

            //var site = sut.SiteCreate("site1", "René", "Madsen", "rm@rm.dk");

            ////Assert
            //Assert.NotNull(site);
            //Assert.AreEqual(site.SiteName, "site1");
            //Assert.AreEqual(site.FirstName, "René");
            //Assert.AreEqual(site.LastName, "Madsen");



        }
        [Test]
        public void Core_Site_SiteRead_ReturnsFullSite()
        {
            //Arrance
            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates
            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl3 = testHelpers.CreateSubTemplate("A.2", "D.2", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl4 = testHelpers.CreateSubTemplate("A.3", "D.3", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl5 = testHelpers.CreateSubTemplate("A.4", "D.4", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl6 = testHelpers.CreateSubTemplate("A.5", "D.5", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl7 = testHelpers.CreateSubTemplate("A.6", "D.6", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl8 = testHelpers.CreateSubTemplate("A.7", "D.7", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl9 = testHelpers.CreateSubTemplate("A.8", "D.8", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl10 = testHelpers.CreateSubTemplate("A.9", "D.9", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl11 = testHelpers.CreateSubTemplate("A.10", "D.10", "CheckList", 1, 1, cl1);


            #endregion
            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #endregion

            #region Worker

            workers worker = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units
            units unit = testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = testHelpers.CreateSiteWorker(55, site, worker);

            #endregion

            //Act

            var match = sut.SiteRead((int)site.microting_uid);

            //Assert
            Assert.NotNull(match);
            Assert.AreEqual(match.SiteId, site.microting_uid);
            Assert.AreEqual(match.SiteName, site.name);


        }
        [Test]
        public void Core_Site_SiteReadAll_ReturnsSites()
        {

            //Arrance
            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates
            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl3 = testHelpers.CreateSubTemplate("A.2", "D.2", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl4 = testHelpers.CreateSubTemplate("A.3", "D.3", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl5 = testHelpers.CreateSubTemplate("A.4", "D.4", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl6 = testHelpers.CreateSubTemplate("A.5", "D.5", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl7 = testHelpers.CreateSubTemplate("A.6", "D.6", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl8 = testHelpers.CreateSubTemplate("A.7", "D.7", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl9 = testHelpers.CreateSubTemplate("A.8", "D.8", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl10 = testHelpers.CreateSubTemplate("A.9", "D.9", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl11 = testHelpers.CreateSubTemplate("A.10", "D.10", "CheckList", 1, 1, cl1);


            #endregion
            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #endregion

            #region Worker

            workers worker = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units
            units unit = testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = testHelpers.CreateSiteWorker(55, site, worker);

            #endregion
            //Act
            var matchNotRemoved = sut.SiteReadAll(false);
            var matchInclRemoved = sut.SiteReadAll(true);
            //Assert
            Assert.NotNull(matchInclRemoved);
            Assert.NotNull(matchNotRemoved);

            Assert.AreEqual(matchInclRemoved.Count, 1);
            Assert.AreEqual(matchNotRemoved.Count, 1);



        }
        [Test]//Using Communicatorn needs httpMock
        public void Core_Site_SiteReset_ReturnsSite()
        {

            //Arrance

            //Act

            //Assert

        }
        [Test]//Using Communicatorn needs httpMock
        public void Core_Site_SiteUpdate_returnsTrue()
        {
            //Arrance

            //Act

            //Assert
        }
        [Test]//Using Communicatorn needs httpMock
        public void Core_Site_SiteDelete_ReturnsTrue()
        {
            //Arrance

            //Act

            //Assert
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