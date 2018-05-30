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
    public class SqlControllerTestSiteWorker : DbTestFixture
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


        #region site_worker
        [Test]
        public void SQL_SiteWorker_SiteWorkerCreate_ReturnsID()
        {

            // Arrance
            #region Arrance

            #region Checklist
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists Cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);

            #endregion

            #region SubCheckList

            check_lists Cl2 = testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);

            #endregion

            #region Fields

            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #endregion

            #region Workers

            #region worker1
            workers worker1 = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region worker2
            workers worker2 = testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);

            #endregion

            #region worker3
            workers worker3 = testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);

            #endregion

            #region worker4
            workers worker4 = testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);

            #endregion

            #region worker5
            workers worker5 = testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);

            #endregion

            #region worker6
            workers worker6 = testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);

            #endregion

            #region worker7
            workers worker7 = testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);

            #endregion

            #region worker8
            workers worker8 = testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);

            #endregion

            #region worker9
            workers worker9 = testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);

            #endregion

            #region worker10
            workers worker10 = testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);

            #endregion

            #endregion

            #region sites

            #region Site1
            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2
            sites site2 = testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3
            sites site3 = testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4
            sites site4 = testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5
            sites site5 = testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6
            sites site6 = testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7
            sites site7 = testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8
            sites site8 = testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9
            sites site9 = testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10
            sites site10 = testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units
            units unit = testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion



            #endregion
            // Act

            var match = sut.SiteWorkerCreate(5, (int)site1.microting_uid, (int)worker1.microting_uid);

            // Assert
            var siteWorkers = DbContext.site_workers.AsNoTracking().ToList();

            Assert.NotNull(match);
            Assert.AreEqual(1, siteWorkers.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, siteWorkers[0].workflow_state);

        }

        [Test]
        public void SQL_SiteWorker_SiteWorkerRead_ReadsSiteWorker()
        {

            // Arrance

            #region Arrance

            #region Checklist
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists Cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);

            #endregion

            #region SubCheckList

            check_lists Cl2 = testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);

            #endregion

            #region Fields

            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #endregion

            #region Workers

            #region worker1
            workers worker1 = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region worker2
            workers worker2 = testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);

            #endregion

            #region worker3
            workers worker3 = testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);

            #endregion

            #region worker4
            workers worker4 = testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);

            #endregion

            #region worker5
            workers worker5 = testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);

            #endregion

            #region worker6
            workers worker6 = testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);

            #endregion

            #region worker7
            workers worker7 = testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);

            #endregion

            #region worker8
            workers worker8 = testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);

            #endregion

            #region worker9
            workers worker9 = testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);

            #endregion

            #region worker10
            workers worker10 = testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);

            #endregion

            #endregion

            #region sites

            #region Site1
            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2
            sites site2 = testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3
            sites site3 = testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4
            sites site4 = testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5
            sites site5 = testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6
            sites site6 = testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7
            sites site7 = testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8
            sites site8 = testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9
            sites site9 = testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10
            sites site10 = testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units
            units unit = testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion

            #region site_workers
            site_workers site_workers = testHelpers.CreateSiteWorker(55, site1, worker1);

            #endregion

            #endregion

            // Act

            var match = sut.SiteWorkerRead(site_workers.microting_uid, site1.id, worker1.id);

            // Assert

            Assert.AreEqual(site_workers.microting_uid, match.MicrotingUId);
            Assert.AreEqual(site_workers.worker_id, match.WorkerUId);
            Assert.AreEqual(site_workers.site_id, match.SiteUId);



        }

        [Test]
        public void SQL_SiteWorker_SiteWorkerUpdate_UpdatesSiteWorker()
        {

            // Arrance

            #region Arrance

            #region Checklist
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists Cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);

            #endregion

            #region SubCheckList

            check_lists Cl2 = testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);

            #endregion

            #region Fields

            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #endregion

            #region Workers

            #region worker1
            workers worker1 = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region worker2
            workers worker2 = testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);

            #endregion

            #region worker3
            workers worker3 = testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);

            #endregion

            #region worker4
            workers worker4 = testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);

            #endregion

            #region worker5
            workers worker5 = testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);

            #endregion

            #region worker6
            workers worker6 = testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);

            #endregion

            #region worker7
            workers worker7 = testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);

            #endregion

            #region worker8
            workers worker8 = testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);

            #endregion

            #region worker9
            workers worker9 = testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);

            #endregion

            #region worker10
            workers worker10 = testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);

            #endregion

            #endregion

            #region sites

            #region Site1
            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2
            sites site2 = testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3
            sites site3 = testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4
            sites site4 = testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5
            sites site5 = testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6
            sites site6 = testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7
            sites site7 = testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8
            sites site8 = testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9
            sites site9 = testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10
            sites site10 = testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units
            units unit = testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion

            #region site_workers
            site_workers site_workers = testHelpers.CreateSiteWorker(55, site1, worker1);

            #endregion

            #endregion

            // Act

            var match = sut.SiteWorkerUpdate((int)site_workers.microting_uid, site1.id, worker1.id);

            // Assert

            Assert.True(match);


        }

        [Test]
        public void SQL_SiteWorker_SiteWorkerDelete_DeletesSiteWorker()
        {

            // Arrance

            #region Arrance

            #region Checklist
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists Cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);

            #endregion

            #region SubCheckList

            check_lists Cl2 = testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);

            #endregion

            #region Fields

            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #endregion

            #region Workers

            #region worker1
            workers worker1 = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region worker2
            workers worker2 = testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);

            #endregion

            #region worker3
            workers worker3 = testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);

            #endregion

            #region worker4
            workers worker4 = testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);

            #endregion

            #region worker5
            workers worker5 = testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);

            #endregion

            #region worker6
            workers worker6 = testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);

            #endregion

            #region worker7
            workers worker7 = testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);

            #endregion

            #region worker8
            workers worker8 = testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);

            #endregion

            #region worker9
            workers worker9 = testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);

            #endregion

            #region worker10
            workers worker10 = testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);

            #endregion

            #endregion

            #region sites

            #region Site1
            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2
            sites site2 = testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3
            sites site3 = testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4
            sites site4 = testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5
            sites site5 = testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6
            sites site6 = testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7
            sites site7 = testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8
            sites site8 = testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9
            sites site9 = testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10
            sites site10 = testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units
            units unit = testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion

            #region site_workers
            site_workers site_workers = testHelpers.CreateSiteWorker(55, site1, worker1);

            #endregion

            #endregion

            // Act

            var match = sut.SiteWorkerDelete((int)site_workers.microting_uid);

            // Assert
            Assert.True(match);

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