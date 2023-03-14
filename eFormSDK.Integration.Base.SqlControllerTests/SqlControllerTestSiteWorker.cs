using System;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace eFormSDK.Integration.Base.SqlControllerTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SqlControllerTestSiteWorker : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
        string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace(@"file:", "");

        public override async Task DoSetup()
        {
            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            testHelpers = new TestHelpers(ConnectionString);
            await testHelpers.GenerateDefaultLanguages();

            sut = new SqlController(dbContextHelper);
            sut.StartLog(new CoreBase());

            await sut.SettingUpdate(Settings.fileLocationPicture,
                Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SettingUpdate(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SettingUpdate(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
        }


        #region site_worker

        [Test]
        public async Task SQL_SiteWorker_SiteWorkerCreate_ReturnsID()
        {
            // Arrance

            #region Arrance

            #region Checklist

            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            CheckList Cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);

            #endregion

            #region SubCheckList

            CheckList Cl2 = await testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);

            #endregion

//            #region Fields
//
//            #region field1
//
//
//            fields f1 = await testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field6
//
//            fields f6 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                86, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field7
//
//            fields f7 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                87, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field8
//
//            fields f8 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                88, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field9
//
//            fields f9 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                89, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field10
//
//            fields f10 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                90, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #endregion

            #region Workers

            #region worker1

            Worker worker1 = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

//            #region worker2
//            workers worker2 = await testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);
//
//            #endregion
//
//            #region worker3
//            workers worker3 = await testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);
//
//            #endregion
//
//            #region worker4
//            workers worker4 = await testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);
//
//            #endregion
//
//            #region worker5
//            workers worker5 = await testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);
//
//            #endregion
//
//            #region worker6
//            workers worker6 = await testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);
//
//            #endregion
//
//            #region worker7
//            workers worker7 = await testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);
//
//            #endregion
//
//            #region worker8
//            workers worker8 = await testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);
//
//            #endregion
//
//            #region worker9
//            workers worker9 = await testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);
//
//            #endregion
//
//            #region worker10
//            workers worker10 = await testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);
//
//            #endregion

            #endregion

            #region sites

            #region Site1

            Site site1 = await testHelpers.CreateSite("SiteName1", 88);

            #endregion

//            #region Site2
//            sites site2 = await testHelpers.CreateSite("SiteName2", 89);
//
//            #endregion
//
//            #region Site3
//            sites site3 = await testHelpers.CreateSite("SiteName3", 90);
//
//            #endregion
//
//            #region Site4
//            sites site4 = await testHelpers.CreateSite("SiteName4", 91);
//
//            #endregion
//
//            #region Site5
//            sites site5 = await testHelpers.CreateSite("SiteName5", 92);
//
//            #endregion
//
//            #region Site6
//            sites site6 = await testHelpers.CreateSite("SiteName6", 93);
//
//            #endregion
//
//            #region Site7
//            sites site7 = await testHelpers.CreateSite("SiteName7", 94);
//
//            #endregion
//
//            #region Site8
//            sites site8 = await testHelpers.CreateSite("SiteName8", 95);
//
//            #endregion
//
//            #region Site9
//            sites site9 = await testHelpers.CreateSite("SiteName9", 96);
//
//            #endregion
//
//            #region Site10
//            sites site10 = await testHelpers.CreateSite("SiteName10", 97);
//
//            #endregion

            #endregion

//            #region units
//            units unit = await testHelpers.CreateUnit(48, 49, site1, 348);
//
//            #endregion

            #endregion

            // Act

            var match = await sut.SiteWorkerCreate(5, (int)site1.MicrotingUid, worker1.MicrotingUid);

            // Assert
            var siteWorkers = DbContext.SiteWorkers.AsNoTracking().ToList();

            Assert.NotNull(match);
            Assert.AreEqual(1, siteWorkers.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, siteWorkers[0].WorkflowState);
        }

        [Test]
        public async Task SQL_SiteWorker_SiteWorkerRead_ReadsSiteWorker()
        {
            // Arrance

            #region Arrance

            #region Checklist

            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            CheckList Cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);

            #endregion

            #region SubCheckList

            CheckList Cl2 = await testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);

            #endregion

//            #region Fields
//
//            #region field1
//
//
//            fields f1 = await testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field6
//
//            fields f6 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                86, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field7
//
//            fields f7 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                87, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field8
//
//            fields f8 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                88, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field9
//
//            fields f9 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                89, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field10
//
//            fields f10 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                90, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #endregion

            #region Workers

            #region worker1

            Worker worker1 = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

//            #region worker2
//            workers worker2 = await testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);
//
//            #endregion
//
//            #region worker3
//            workers worker3 = await testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);
//
//            #endregion
//
//            #region worker4
//            workers worker4 = await testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);
//
//            #endregion
//
//            #region worker5
//            workers worker5 = await testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);
//
//            #endregion
//
//            #region worker6
//            workers worker6 = await testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);
//
//            #endregion
//
//            #region worker7
//            workers worker7 = await testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);
//
//            #endregion
//
//            #region worker8
//            workers worker8 = await testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);
//
//            #endregion
//
//            #region worker9
//            workers worker9 = await testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);
//
//            #endregion
//
//            #region worker10
//            workers worker10 = await testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);
//
//            #endregion

            #endregion

            #region sites

            #region Site1

            Site site1 = await testHelpers.CreateSite("SiteName1", 88);

            #endregion

//            #region Site2
//            sites site2 = await testHelpers.CreateSite("SiteName2", 89);
//
//            #endregion
//
//            #region Site3
//            sites site3 = await testHelpers.CreateSite("SiteName3", 90);
//
//            #endregion
//
//            #region Site4
//            sites site4 = await testHelpers.CreateSite("SiteName4", 91);
//
//            #endregion
//
//            #region Site5
//            sites site5 = await testHelpers.CreateSite("SiteName5", 92);
//
//            #endregion
//
//            #region Site6
//            sites site6 = await testHelpers.CreateSite("SiteName6", 93);
//
//            #endregion
//
//            #region Site7
//            sites site7 = await testHelpers.CreateSite("SiteName7", 94);
//
//            #endregion
//
//            #region Site8
//            sites site8 = await testHelpers.CreateSite("SiteName8", 95);
//
//            #endregion
//
//            #region Site9
//            sites site9 = await testHelpers.CreateSite("SiteName9", 96);
//
//            #endregion
//
//            #region Site10
//            sites site10 = await testHelpers.CreateSite("SiteName10", 97);
//
//            #endregion

            #endregion

//            #region units
//            units unit = await testHelpers.CreateUnit(48, 49, site1, 348);
//
//            #endregion

            #region site_workers

            SiteWorker siteWorker = await testHelpers.CreateSiteWorker(55, site1, worker1);

            #endregion

            #endregion

            // Act

            SiteWorkerDto match = await sut.SiteWorkerRead(siteWorker.MicrotingUid, site1.Id, worker1.Id);

            // Assert

            Assert.AreEqual(siteWorker.MicrotingUid, match.MicrotingUId);
            Assert.AreEqual(siteWorker.Worker.MicrotingUid, match.WorkerUId);
            Assert.AreEqual(siteWorker.Site.MicrotingUid, match.SiteUId);
        }

        [Test]
        public async Task SQL_SiteWorker_SiteWorkerUpdate_UpdatesSiteWorker()
        {
            // Arrance

            #region Arrance

            #region Checklist

            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            CheckList Cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);

            #endregion

            #region SubCheckList

            CheckList Cl2 = await testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);

            #endregion

//            #region Fields
//
//            #region field1
//
//
//            fields f1 = await testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field6
//
//            fields f6 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                86, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field7
//
//            fields f7 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                87, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field8
//
//            fields f8 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                88, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field9
//
//            fields f9 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                89, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field10
//
//            fields f10 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                90, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #endregion

            #region Workers

            #region worker1

            Worker worker1 = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

//            #region worker2
//            workers worker2 = await testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);
//
//            #endregion
//
//            #region worker3
//            workers worker3 = await testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);
//
//            #endregion
//
//            #region worker4
//            workers worker4 = await testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);
//
//            #endregion
//
//            #region worker5
//            workers worker5 = await testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);
//
//            #endregion
//
//            #region worker6
//            workers worker6 = await testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);
//
//            #endregion
//
//            #region worker7
//            workers worker7 = await testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);
//
//            #endregion
//
//            #region worker8
//            workers worker8 = await testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);
//
//            #endregion
//
//            #region worker9
//            workers worker9 = await testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);
//
//            #endregion
//
//            #region worker10
//            workers worker10 = await testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);
//
//            #endregion

            #endregion

            #region sites

            #region Site1

            Site site1 = await testHelpers.CreateSite("SiteName1", 88);

            #endregion

//            #region Site2
//            sites site2 = await testHelpers.CreateSite("SiteName2", 89);
//
//            #endregion
//
//            #region Site3
//            sites site3 = await testHelpers.CreateSite("SiteName3", 90);
//
//            #endregion
//
//            #region Site4
//            sites site4 = await testHelpers.CreateSite("SiteName4", 91);
//
//            #endregion
//
//            #region Site5
//            sites site5 = await testHelpers.CreateSite("SiteName5", 92);
//
//            #endregion
//
//            #region Site6
//            sites site6 = await testHelpers.CreateSite("SiteName6", 93);
//
//            #endregion
//
//            #region Site7
//            sites site7 = await testHelpers.CreateSite("SiteName7", 94);
//
//            #endregion
//
//            #region Site8
//            sites site8 = await testHelpers.CreateSite("SiteName8", 95);
//
//            #endregion
//
//            #region Site9
//            sites site9 = await testHelpers.CreateSite("SiteName9", 96);
//
//            #endregion
//
//            #region Site10
//            sites site10 = await testHelpers.CreateSite("SiteName10", 97);
//
//            #endregion

            #endregion

//            #region units
//            units unit = await testHelpers.CreateUnit(48, 49, site1, 348);
//
//            #endregion

            #region site_workers

            SiteWorker siteWorker = await testHelpers.CreateSiteWorker(55, site1, worker1);

            #endregion

            #endregion

            // Act

            var match = await sut.SiteWorkerUpdate((int)siteWorker.MicrotingUid, site1.Id, worker1.Id);

            // Assert

            Assert.True(match);
        }

        [Test]
        public async Task SQL_SiteWorker_SiteWorkerDelete_DeletesSiteWorker()
        {
            // Arrance

            #region Arrance

            #region Checklist

            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            CheckList Cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);

            #endregion

            #region SubCheckList

            CheckList Cl2 = await testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);

            #endregion

//            #region Fields
//
//            #region field1
//
//
//            fields f1 = await testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, dbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field6
//
//            fields f6 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                86, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field7
//
//            fields f7 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                87, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field8
//
//            fields f8 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                88, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field9
//
//            fields f9 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                89, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field10
//
//            fields f10 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                90, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #endregion

            #region Workers

            #region worker1

            Worker worker1 = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

//            #region worker2
//            workers worker2 = await testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);
//
//            #endregion
//
//            #region worker3
//            workers worker3 = await testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);
//
//            #endregion
//
//            #region worker4
//            workers worker4 = await testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);
//
//            #endregion
//
//            #region worker5
//            workers worker5 = await testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);
//
//            #endregion
//
//            #region worker6
//            workers worker6 = await testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);
//
//            #endregion
//
//            #region worker7
//            workers worker7 = await testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);
//
//            #endregion
//
//            #region worker8
//            workers worker8 = await testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);
//
//            #endregion
//
//            #region worker9
//            workers worker9 = await testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);
//
//            #endregion
//
//            #region worker10
//            workers worker10 = await testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);
//
//            #endregion

            #endregion

            #region sites

            #region Site1

            Site site1 = await testHelpers.CreateSite("SiteName1", 88);

            #endregion

//            #region Site2
//            sites site2 = await testHelpers.CreateSite("SiteName2", 89);
//
//            #endregion
//
//            #region Site3
//            sites site3 = await testHelpers.CreateSite("SiteName3", 90);
//
//            #endregion
//
//            #region Site4
//            sites site4 = await testHelpers.CreateSite("SiteName4", 91);
//
//            #endregion
//
//            #region Site5
//            sites site5 = await testHelpers.CreateSite("SiteName5", 92);
//
//            #endregion
//
//            #region Site6
//            sites site6 = await testHelpers.CreateSite("SiteName6", 93);
//
//            #endregion
//
//            #region Site7
//            sites site7 = await testHelpers.CreateSite("SiteName7", 94);
//
//            #endregion
//
//            #region Site8
//            sites site8 = await testHelpers.CreateSite("SiteName8", 95);
//
//            #endregion
//
//            #region Site9
//            sites site9 = await testHelpers.CreateSite("SiteName9", 96);
//
//            #endregion
//
//            #region Site10
//            sites site10 = await testHelpers.CreateSite("SiteName10", 97);
//
//            #endregion

            #endregion

//            #region units
//            units unit = await testHelpers.CreateUnit(48, 49, site1, 348);
//
//            #endregion

            #region site_workers

            SiteWorker siteWorker = await testHelpers.CreateSiteWorker(55, site1, worker1);

            #endregion

            #endregion

            // Act

            var match = await sut.SiteWorkerDelete((int)siteWorker.MicrotingUid);

            // Assert
            Assert.True(match);
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