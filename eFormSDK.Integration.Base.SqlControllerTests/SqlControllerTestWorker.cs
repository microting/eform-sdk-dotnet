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
    public class SqlControllerTestWorker : DbTestFixture
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
            testHelpers = new TestHelpers(ConnectionString);
            await testHelpers.GenerateDefaultLanguages();
            await sut.SettingUpdate(Settings.fileLocationPicture,
                Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SettingUpdate(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SettingUpdate(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
        }


        #region Worker

        [Test]
        public async Task SQL_Worker_WorkerGetAll_ReturnsAllWorkers()
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

            #region worker2

            Worker worker2 = await testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);

            #endregion

            #region worker3

            Worker worker3 = await testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);

            #endregion

            #region worker4

            Worker worker4 = await testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);

            #endregion

            #region worker5

            Worker worker5 = await testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);

            #endregion

            #region worker6

            Worker worker6 = await testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);

            #endregion

            #region worker7

            Worker worker7 = await testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);

            #endregion

            #region worker8

            Worker worker8 = await testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);

            #endregion

            #region worker9

            Worker worker9 = await testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);

            #endregion

            #region worker10

            Worker worker10 = await testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);

            #endregion

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
//
//            #region site_workers
//            site_workers site_workers = await testHelpers.CreateSiteWorker(55, site1, worker1);
//
//            #endregion

            #endregion

            // Act

            var getAllCreatedWorkers = await sut.WorkerGetAll(Constants.WorkflowStates.Created, 0, 1);
            var getAllRemovedWorkers = await sut.WorkerGetAll(Constants.WorkflowStates.Removed, 0, 1);


            // Assert

            Assert.AreEqual(10, getAllCreatedWorkers.Count());
            Assert.AreEqual(0, getAllRemovedWorkers.Count());

            Assert.AreEqual(worker1.FirstName, getAllCreatedWorkers[0].FirstName);
            Assert.AreEqual(worker2.FirstName, getAllCreatedWorkers[1].FirstName);
            Assert.AreEqual(worker3.FirstName, getAllCreatedWorkers[2].FirstName);
            Assert.AreEqual(worker4.FirstName, getAllCreatedWorkers[3].FirstName);
            Assert.AreEqual(worker5.FirstName, getAllCreatedWorkers[4].FirstName);
            Assert.AreEqual(worker6.FirstName, getAllCreatedWorkers[5].FirstName);
            Assert.AreEqual(worker7.FirstName, getAllCreatedWorkers[6].FirstName);
            Assert.AreEqual(worker8.FirstName, getAllCreatedWorkers[7].FirstName);
            Assert.AreEqual(worker9.FirstName, getAllCreatedWorkers[8].FirstName);
            Assert.AreEqual(worker10.FirstName, getAllCreatedWorkers[9].FirstName);


            Assert.AreEqual(worker1.LastName, getAllCreatedWorkers[0].LastName);
            Assert.AreEqual(worker2.LastName, getAllCreatedWorkers[1].LastName);
            Assert.AreEqual(worker3.LastName, getAllCreatedWorkers[2].LastName);
            Assert.AreEqual(worker4.LastName, getAllCreatedWorkers[3].LastName);
            Assert.AreEqual(worker5.LastName, getAllCreatedWorkers[4].LastName);
            Assert.AreEqual(worker6.LastName, getAllCreatedWorkers[5].LastName);
            Assert.AreEqual(worker7.LastName, getAllCreatedWorkers[6].LastName);
            Assert.AreEqual(worker8.LastName, getAllCreatedWorkers[7].LastName);
            Assert.AreEqual(worker9.LastName, getAllCreatedWorkers[8].LastName);
            Assert.AreEqual(worker10.LastName, getAllCreatedWorkers[9].LastName);
        }

        [Test]
        public async Task SQL_Worker_WorkerCreate_ReturnsWorkerId()
        {
            // Arrance

            // Act
            var match = await sut.WorkerCreate(55, "Arne", "Jensen", "aa@tak.dk");
            // Assert

            var workers = DbContext.Workers.AsNoTracking().ToList();

            Assert.NotNull(match);
            Assert.AreEqual(1, workers.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, workers[0].WorkflowState);
        }

        [Test]
        public async Task SQL_Worker_WorkerNameRead_ReadsName()
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
//
//            #region site_workers
//            site_workers site_workers = await testHelpers.CreateSiteWorker(55, site1, worker1);
//
//            #endregion

            #endregion

            // Act

            var match = await sut.WorkerNameRead(worker1.Id);

            // Assert

            Assert.AreEqual(worker1.FirstName + " " + worker1.LastName, match);
        }

        [Test]
        public async Task SQL_Worker_WorkerRead_ReadsWorker()
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
//
//            #region site_workers
//            site_workers site_workers = await testHelpers.CreateSiteWorker(55, site1, worker1);
//
//            #endregion

            #endregion

            // Act

            var match = await sut.WorkerRead(worker1.MicrotingUid);

            // Assert

            Assert.AreEqual(worker1.MicrotingUid, match.WorkerUId);
            Assert.AreEqual(worker1.FirstName, match.FirstName);
        }

        [Test]
        public async Task SQL_Worker_WorkerUpdate_UpdatesWorker()
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
//
//            #region site_workers
//            site_workers site_workers = await testHelpers.CreateSiteWorker(55, site1, worker1);
//
//            #endregion

            #endregion

            // Act

            var match = await sut.WorkerUpdate(worker1.MicrotingUid, worker1.FirstName, worker1.LastName,
                worker1.Email);


            // Assert

            Assert.True(match);
        }

        [Test]
        public async Task SQL_Worker_WorkerDelete_DeletesWorker()
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
//
//            #region site_workers
//            site_workers site_workers = await testHelpers.CreateSiteWorker(55, site1, worker1);
//
//            #endregion

            #endregion

            // Act

            var match = await sut.WorkerDelete(worker1.MicrotingUid);

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