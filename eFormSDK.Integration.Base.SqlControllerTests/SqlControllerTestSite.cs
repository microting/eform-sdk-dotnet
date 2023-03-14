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
    public class SqlControllerTestSite : DbTestFixture
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


        #region site

        [Test]
        public async Task SQL_Site_SiteGetAll_DoesReturnAllSites()
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
//
//            #region Worker
//
//            workers worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
//
//            #endregion

            #region sites

            #region Site1

            Site site1 = await testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2

            Site site2 = await testHelpers.CreateSite("SiteName2", 88);

            #endregion

            #region Site3

            Site site3 = await testHelpers.CreateSite("SiteName3", 88);

            #endregion

            #region Site4

            Site site4 = await testHelpers.CreateSite("SiteName4", 88);

            #endregion

            #region Site5

            Site site5 = await testHelpers.CreateSite("SiteName5", 88);

            #endregion

            #region Site6

            Site site6 = await testHelpers.CreateSite("SiteName6", 88);

            #endregion

            #region Site7

            Site site7 = await testHelpers.CreateSite("SiteName7", 88);

            #endregion

            #region Site8

            Site site8 = await testHelpers.CreateSite("SiteName8", 88);

            #endregion

            #region Site9

            Site site9 = await testHelpers.CreateSite("SiteName9", 88);

            #endregion

            #region Site10

            Site site10 = await testHelpers.CreateSite("SiteName10", 88);

            #endregion

            #endregion

            #endregion


            // Act

            var getAllSitesOnlyCreated = sut.SiteGetAll(false).Result.ToList();
            var getAllSitesInclRemoved = sut.SiteGetAll(true).Result.ToList();


            // Assert
            Assert.True(true);

            Assert.AreEqual(10, getAllSitesOnlyCreated.Count());
            Assert.AreEqual(10, getAllSitesInclRemoved.Count());

            Assert.AreEqual(site1.Name, getAllSitesOnlyCreated[0].SiteName);
            Assert.AreEqual(site2.Name, getAllSitesOnlyCreated[1].SiteName);
            Assert.AreEqual(site3.Name, getAllSitesOnlyCreated[2].SiteName);
            Assert.AreEqual(site4.Name, getAllSitesOnlyCreated[3].SiteName);
            Assert.AreEqual(site5.Name, getAllSitesOnlyCreated[4].SiteName);
            Assert.AreEqual(site6.Name, getAllSitesOnlyCreated[5].SiteName);
            Assert.AreEqual(site7.Name, getAllSitesOnlyCreated[6].SiteName);
            Assert.AreEqual(site8.Name, getAllSitesOnlyCreated[7].SiteName);
            Assert.AreEqual(site9.Name, getAllSitesOnlyCreated[8].SiteName);
            Assert.AreEqual(site10.Name, getAllSitesOnlyCreated[9].SiteName);


            Assert.AreEqual(site1.Name, getAllSitesInclRemoved[0].SiteName);
            Assert.AreEqual(site2.Name, getAllSitesInclRemoved[1].SiteName);
            Assert.AreEqual(site3.Name, getAllSitesInclRemoved[2].SiteName);
            Assert.AreEqual(site4.Name, getAllSitesInclRemoved[3].SiteName);
            Assert.AreEqual(site5.Name, getAllSitesInclRemoved[4].SiteName);
            Assert.AreEqual(site6.Name, getAllSitesInclRemoved[5].SiteName);
            Assert.AreEqual(site7.Name, getAllSitesInclRemoved[6].SiteName);
            Assert.AreEqual(site8.Name, getAllSitesInclRemoved[7].SiteName);
            Assert.AreEqual(site9.Name, getAllSitesInclRemoved[8].SiteName);
            Assert.AreEqual(site10.Name, getAllSitesInclRemoved[9].SiteName);
        }

        [Test]
        public async Task SQL_Site_SimpleSiteGetAll_DoesReturnSiteList()
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
//
//            #region Worker
//
//            workers worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
//
//            #endregion

            #region sites

            #region Site1

            Site site1 = await testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2

            Site site2 = await testHelpers.CreateSite("SiteName2", 88);

            #endregion

            #region Site3

            Site site3 = await testHelpers.CreateSite("SiteName3", 88);

            #endregion

            #region Site4

            Site site4 = await testHelpers.CreateSite("SiteName4", 88);

            #endregion

            #region Site5

            Site site5 = await testHelpers.CreateSite("SiteName5", 88);

            #endregion

            #region Site6

            Site site6 = await testHelpers.CreateSite("SiteName6", 88);

            #endregion

            #region Site7

            Site site7 = await testHelpers.CreateSite("SiteName7", 88);

            #endregion

            #region Site8

            Site site8 = await testHelpers.CreateSite("SiteName8", 88);

            #endregion

            #region Site9

            Site site9 = await testHelpers.CreateSite("SiteName9", 88);

            #endregion

            #region Site10

            Site site10 = await testHelpers.CreateSite("SiteName10", 88);

            #endregion

            #endregion

            #endregion

            // Act
            var match = await sut.SimpleSiteGetAll(Constants.WorkflowStates.Created, 0, 1);


            // Assert
            Assert.AreEqual(10, match.Count());


            Assert.AreEqual(site1.Name, match[0].SiteName);
            Assert.AreEqual(site2.Name, match[1].SiteName);
            Assert.AreEqual(site3.Name, match[2].SiteName);
            Assert.AreEqual(site4.Name, match[3].SiteName);
            Assert.AreEqual(site5.Name, match[4].SiteName);
            Assert.AreEqual(site6.Name, match[5].SiteName);
            Assert.AreEqual(site7.Name, match[6].SiteName);
            Assert.AreEqual(site8.Name, match[7].SiteName);
            Assert.AreEqual(site9.Name, match[8].SiteName);
            Assert.AreEqual(site10.Name, match[9].SiteName);
        }

        [Test]
        public async Task SQL_Site_SiteCreate_ReturnsSiteId()
        {
            // Arrance


            // Act

            var match = await sut.SiteCreate(88, "siteName1");

            // Assert
            var sites = DbContext.Sites.AsNoTracking().ToList();

            Assert.NotNull(match);

            Assert.AreEqual(1, sites.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, sites[0].WorkflowState);
        }

        [Test]
        public async Task SQL_Site_SiteRead_ReadsSite()
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
//
//            #region Worker
//
//            workers worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
//
//            #endregion

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

            #endregion

            // Act

            var match = await sut.SiteRead((int)site1.MicrotingUid);

            // Assert
            Assert.AreEqual(site1.MicrotingUid, match.SiteUId);
            Assert.AreEqual(site1.Name, match.SiteName);
        }

        [Test]
        public async Task SQL_Site_SiteReadSimple_ReadsSite()
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

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region sites

            #region Site1

            Site site1 = await testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2

            Site site2 = await testHelpers.CreateSite("SiteName2", 89);

            #endregion

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

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion

            #region site_workers

            await testHelpers.CreateSiteWorker(55, site1, worker);

            #endregion

            #endregion

            // Act

            var match = await sut.SiteReadSimple((int)site1.MicrotingUid);

            // Assert
            Assert.AreEqual(site1.MicrotingUid, match.SiteId);
            Assert.AreEqual(site1.Name, match.SiteName);
        }

        [Test]
        public async Task SQL_Site_SiteUpdate_UpdatesSite()
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

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

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
//            site_workers site_workers = await testHelpers.CreateSiteWorker(55, site1, worker);
//
//            #endregion

            #endregion

            // Act

            var match = await sut.SiteUpdate((int)site1.MicrotingUid, site1.Name);

            // Assert
            Assert.True(match);
        }

        [Test]
        public async Task SQL_Site_SiteDelete_DeletesSite()
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

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

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
//            site_workers site_workers = await testHelpers.CreateSiteWorker(55, site1, worker);
//
//            #endregion

            #endregion

            // Act

            var match = await sut.SiteDelete((int)site1.MicrotingUid);

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