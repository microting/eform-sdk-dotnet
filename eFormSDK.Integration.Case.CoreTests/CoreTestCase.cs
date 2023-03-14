using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;
using CheckListValue = Microting.eForm.Infrastructure.Data.Entities.CheckListValue;
using Field = Microting.eForm.Infrastructure.Data.Entities.Field;
using FieldValue = Microting.eForm.Infrastructure.Data.Entities.FieldValue;
using UploadedData = Microting.eForm.Infrastructure.Data.Entities.UploadedData;

namespace eFormSDK.Integration.Case.CoreTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class CoreTestCase : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;
        Random rnd = new Random();
        short shortMinValue = Int16.MinValue;
        short shortmaxValue = Int16.MaxValue;
        private Language language;

        public override async Task DoSetup()
        {
            if (sut == null)
            {
                sut = new Core();
                sut.HandleCaseCreated += EventCaseCreated;
                sut.HandleCaseRetrived += EventCaseRetrived;
                sut.HandleCaseCompleted += EventCaseCompleted;
                sut.HandleCaseDeleted += EventCaseDeleted;
                sut.HandleFileDownloaded += EventFileDownloaded;
                sut.HandleSiteActivated += EventSiteActivated;
                await sut.StartSqlOnly(ConnectionString);
            }

            path = Assembly.GetExecutingAssembly().Location;
            // UriBuilder uri = new UriBuilder(path);
            // path = Uri.UnescapeDataString(uri.Path);
            path = Path.GetDirectoryName(path);
            await sut.SetSdkSetting(Settings.fileLocationPicture,
                Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers(ConnectionString);
            await testHelpers.GenerateDefaultLanguages();
            language = DbContext.Languages.Single(x => x.Name == "Danish");
            //await sut.StartLog(new CoreBase());
        }

        #region case

        [Test]
        public async Task Core_Case_CaseDeleteResult_DoesMarkCaseRemoved()
        {
            // Arrance
            Site site = new Site();
            site.Name = "SiteName";
            site.MicrotingUid = 1234;
            DbContext.Sites.Add(site);
            await DbContext.SaveChangesAsync();

            CheckList cl = new CheckList();
            cl.Label = "label";

            DbContext.CheckLists.Add(cl);
            await DbContext.SaveChangesAsync();

            Microting.eForm.Infrastructure.Data.Entities.Case aCase =
                new Microting.eForm.Infrastructure.Data.Entities.Case
                {
                    MicrotingUid = rnd.Next(shortMinValue, shortmaxValue),
                    MicrotingCheckUid = rnd.Next(shortMinValue, shortmaxValue),
                    WorkflowState = Constants.WorkflowStates.Created,
                    CheckListId = cl.Id,
                    SiteId = site.Id
                };

            DbContext.Cases.Add(aCase);
            await DbContext.SaveChangesAsync();

            // Act
            await sut.CaseDeleteResult(aCase.Id);
            CaseDto theCase = await sut.CaseLookupCaseId(aCase.Id);

            // Assert
            Assert.NotNull(theCase);
            Assert.AreEqual(Constants.WorkflowStates.Removed, theCase.WorkflowState);
        }

//         [Test]// TODO needs http mock done
// #pragma warning disable 1998
//         public async Task Core_Case_CaseCheck_ChecksCase()
//         {
// #pragma warning restore 1998
//
//         }
        [Test]
        public async Task Core_Case_CaseRead_ReadsCase()
        {
            // Arrange

            #region Arrance

            #region Template1

            DateTime c1_Ca = DateTime.Now;
            DateTime c1_Ua = DateTime.Now;
            CheckList cl1 =
                await testHelpers.CreateTemplate(c1_Ca, c1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

//            #region SubTemplate1
//            check_lists cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
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

//            #region site_workers
//            site_workers site_workers = await testHelpers.CreateSiteWorker(55, site, worker);
//
//            #endregion

            #region Case1

            Microting.eForm.Infrastructure.Data.Entities.Case aCase = await testHelpers.CreateCase("caseUId", cl1,
                DateTime.Now, "custom", DateTime.Now,
                worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 66, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            // Act

            var match = await sut.CaseRead((int)aCase.MicrotingUid, (int)aCase.MicrotingCheckUid, language);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match.CaseType, aCase.Type);
        }

        [Test]
        public async Task Core_Case_CaseReadByCaseId_Returns_cDto()
        {
            // Arrance

            #region Arrance

            #region Template1

            DateTime c1_Ca = DateTime.Now;
            DateTime c1_Ua = DateTime.Now;
            CheckList cl1 =
                await testHelpers.CreateTemplate(c1_Ca, c1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region Case1

            Microting.eForm.Infrastructure.Data.Entities.Case aCase = await testHelpers.CreateCase("caseUId", cl1,
                DateTime.Now, "custom", DateTime.Now,
                worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 66, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion


            // Act

            var match = await sut.CaseReadByCaseId(aCase.Id);

            // Assert

            Assert.AreEqual(aCase.Id, match.CaseId);
        }

        [Test]
        public async Task Core_Case_CaseReadFirstId()
        {
            // Arrance

            #region Arrance

            #region Template1

            DateTime c1_Ca = DateTime.Now;
            DateTime c1_Ua = DateTime.Now;
            CheckList cl1 =
                await testHelpers.CreateTemplate(c1_Ca, c1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region Case1

            Microting.eForm.Infrastructure.Data.Entities.Case aCase = await testHelpers.CreateCase("caseUId", cl1,
                DateTime.Now, "custom", DateTime.Now,
                worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 100, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            // Act
            var match = await sut.CaseReadFirstId(aCase.CheckList.Id, aCase.WorkflowState);
            // Assert
            Assert.AreEqual(aCase.Id, match);
        }

        [Test]
        public async Task Core_Case_CaseUpdate_ReturnsTrue()
        {
            // Arrange

            #region Arrance

            #region Template1

            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates

            #region SubTemplate1

            CheckList cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl3 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl4 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl5 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl6 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl7 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl8 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl9 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl10 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl11 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #endregion

            #region Fields

            #region field1

            Field f1 = await testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "",
                "Comment field description",
                5, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55,
                "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2

            Field f2 = await testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "",
                "showPDf Description",
                45, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);

            #endregion

            #region field3

            Field f3 = await testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "",
                "Number Field Description",
                83, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);

            #endregion

            #region field4

            Field f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "",
                "date Description",
                84, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field5

            Field f5 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                85, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field6

            Field f6 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                86, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field7

            Field f7 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                87, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field8

            Field f8 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                88, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field9

            Field f9 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                89, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field10

            Field f10 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                90, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #endregion

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region cases

            #region cases created

            #region Case1

            DateTime c1_ca = DateTime.Now.AddDays(-9);
            DateTime c1_da = DateTime.Now.AddDays(-8).AddHours(-12);
            DateTime c1_ua = DateTime.Now.AddDays(-8);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase1 = await testHelpers.CreateCase("case1UId", cl1,
                c1_ca, "custom1",
                c1_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            #endregion

            #region UploadedData

            #region ud1

            UploadedData ud1 = await testHelpers.CreateUploadedData("checksum1", "File1", "no", "hjgjghjhg", "File1", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud2

            UploadedData ud2 = await testHelpers.CreateUploadedData("checksum2", "File1", "no", "hjgjghjhg", "File2", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud3

            UploadedData ud3 = await testHelpers.CreateUploadedData("checksum3", "File1", "no", "hjgjghjhg", "File3", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud4

            UploadedData ud4 = await testHelpers.CreateUploadedData("checksum4", "File1", "no", "hjgjghjhg", "File4", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud5

            UploadedData ud5 = await testHelpers.CreateUploadedData("checksum5", "File1", "no", "hjgjghjhg", "File5", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud6

            UploadedData ud6 = await testHelpers.CreateUploadedData("checksum6", "File1", "no", "hjgjghjhg", "File6", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud7

            UploadedData ud7 = await testHelpers.CreateUploadedData("checksum7", "File1", "no", "hjgjghjhg", "File7", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud8

            UploadedData ud8 = await testHelpers.CreateUploadedData("checksum8", "File1", "no", "hjgjghjhg", "File8", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud9

            UploadedData ud9 = await testHelpers.CreateUploadedData("checksum9", "File1", "no", "hjgjghjhg", "File9", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud10

            UploadedData ud10 = await testHelpers.CreateUploadedData("checksum10", "File1", "no", "hjgjghjhg", "File10",
                1, worker,
                "local", 55, false);

            #endregion

            #endregion

            #region Check List Values

            #region clv1

            CheckListValue clv1 =
                await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);

            #endregion

            #region clv2

            CheckListValue clv2 =
                await testHelpers.CreateCheckListValue(aCase1, cl3, Constants.CheckListValues.Checked, null, 861);

            #endregion

            #region clv3

            CheckListValue clv3 =
                await testHelpers.CreateCheckListValue(aCase1, cl4, Constants.CheckListValues.Checked, null, 862);

            #endregion

            #region clv4

            CheckListValue clv4 =
                await testHelpers.CreateCheckListValue(aCase1, cl5, Constants.CheckListValues.NotChecked, null, 863);

            #endregion

            #region clv5

            CheckListValue clv5 =
                await testHelpers.CreateCheckListValue(aCase1, cl6, Constants.CheckListValues.NotChecked, null, 864);

            #endregion

            #region clv6

            CheckListValue clv6 =
                await testHelpers.CreateCheckListValue(aCase1, cl7, Constants.CheckListValues.NotChecked, null, 865);

            #endregion

            #region clv7

            CheckListValue clv7 =
                await testHelpers.CreateCheckListValue(aCase1, cl8, Constants.CheckListValues.NotApproved, null, 866);

            #endregion

            #region clv8

            CheckListValue clv8 =
                await testHelpers.CreateCheckListValue(aCase1, cl9, Constants.CheckListValues.NotApproved, null, 867);

            #endregion

            #region clv9

            CheckListValue clv9 =
                await testHelpers.CreateCheckListValue(aCase1, cl10, Constants.CheckListValues.NotApproved, null, 868);

            #endregion

            #region clv10

            CheckListValue clv10 =
                await testHelpers.CreateCheckListValue(aCase1, cl11, Constants.CheckListValues.NotApproved, null, 869);

            #endregion

            #endregion

            #region Field Values

            #region fv1

            FieldValue field_Value1 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.Id, null, "tomt1", 61230, worker);

            #endregion

            #region fv2

            FieldValue field_Value2 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.Id, null, "tomt2", 61231, worker);

            #endregion

            #region fv3

            FieldValue field_Value3 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.Id, null, "tomt3", 61232, worker);

            #endregion

            #region fv4

            FieldValue field_Value4 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.Id, null, "tomt4", 61233, worker);

            #endregion

            #region fv5

            FieldValue field_Value5 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.Id, null, "tomt5", 61234, worker);

            #endregion

            #region fv6

            FieldValue field_Value6 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.Id, null, "tomt6", 61235, worker);

            #endregion

            #region fv7

            FieldValue field_Value7 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.Id, null, "tomt7", 61236, worker);

            #endregion

            #region fv8

            FieldValue field_Value8 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.Id, null, "tomt8", 61237, worker);

            #endregion

            #region fv9

            FieldValue field_Value9 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.Id, null, "tomt9", 61238, worker);

            #endregion

            #region fv10

            FieldValue field_Value10 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.Id, null, "tomt10", 61239, worker);

            #endregion

            #endregion

            #endregion

            // Act
            List<string> FVPlist = new List<string>();
            FVPlist.Add(field_Value1.Id + " |" + field_Value1.Value);
            FVPlist.Add(field_Value2.Id + " |" + field_Value2.Value);
            FVPlist.Add(field_Value3.Id + " |" + field_Value3.Value);
            FVPlist.Add(field_Value4.Id + " |" + field_Value4.Value);
            FVPlist.Add(field_Value5.Id + " |" + field_Value5.Value);
            FVPlist.Add(field_Value6.Id + " |" + field_Value6.Value);
            FVPlist.Add(field_Value7.Id + " |" + field_Value7.Value);
            FVPlist.Add(field_Value8.Id + " |" + field_Value8.Value);
            FVPlist.Add(field_Value9.Id + " |" + field_Value9.Value);
            FVPlist.Add(field_Value10.Id + " |" + field_Value10.Value);
            //FVPlist.ToList();

            List<string> CLVlist = new List<string>();
            CLVlist.Add(clv1.CheckListId + " |" + clv1.Status);
            CLVlist.Add(clv2.CheckListId + " |" + clv2.Status);
            CLVlist.Add(clv3.CheckListId + " |" + clv3.Status);
            CLVlist.Add(clv4.CheckListId + " |" + clv4.Status);
            CLVlist.Add(clv5.CheckListId + " |" + clv5.Status);
            CLVlist.Add(clv6.CheckListId + " |" + clv6.Status);
            CLVlist.Add(clv7.CheckListId + " |" + clv7.Status);
            CLVlist.Add(clv8.CheckListId + " |" + clv8.Status);
            CLVlist.Add(clv9.CheckListId + " |" + clv9.Status);
            CLVlist.Add(clv10.CheckListId + " |" + clv10.Status);
            //CLVlist.ToList();

            var match = await sut.CaseUpdate(aCase1.Id, FVPlist, CLVlist);

            Assert.NotNull(match);
            Assert.True(match);
        }
//         [Test]// TODO needs mocks
// #pragma warning disable 1998
//         public async Task Core_Case_CaseDelete_ReturnsTrue()
//         {
//
//         }
// #pragma warning restore 1998
//
// #pragma warning disable 1998
//         [Test]// TODO needs mocks
//         public async Task Core_Case_CaseDelete2_ReturnsTrue()
//         {
        //// Arrange
        //#region Arrance
        //#region Template1
        //DateTime cl1_Ca = DateTime.Now;
        //DateTime cl1_Ua = DateTime.Now;
        //check_lists cl1 = await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

        //#endregion

        //#region subtemplates
        //#region SubTemplate1
        //check_lists cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


        //#endregion

        //#region SubTemplate1
        //check_lists cl3 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


        //#endregion

        //#region SubTemplate1
        //check_lists cl4 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


        //#endregion

        //#region SubTemplate1
        //check_lists cl5 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


        //#endregion

        //#region SubTemplate1
        //check_lists cl6 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


        //#endregion

        //#region SubTemplate1
        //check_lists cl7 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


        //#endregion

        //#region SubTemplate1
        //check_lists cl8 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


        //#endregion

        //#region SubTemplate1
        //check_lists cl9 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


        //#endregion

        //#region SubTemplate1
        //check_lists cl10 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


        //#endregion

        //#region SubTemplate1
        //check_lists cl11 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


        //#endregion
        //#endregion

        //#region Fields
        //#region field1


        //fields f1 = await testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
        //    5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
        //    0, 0, "", 49);

        //#endregion

        //#region field2


        //fields f2 = await testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
        //    45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
        //    "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


        //#endregion

        //#region field3

        //fields f3 = await testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
        //    83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
        //    "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


        //#endregion

        //#region field4


        //fields f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
        //    84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
        //    "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


        //#endregion

        //#region field5

        //fields f5 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
        //    85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
        //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


        //#endregion

        //#region field6

        //fields f6 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
        //    86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
        //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


        //#endregion

        //#region field7

        //fields f7 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
        //    87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
        //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


        //#endregion

        //#region field8

        //fields f8 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
        //    88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
        //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


        //#endregion

        //#region field9

        //fields f9 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
        //    89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
        //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


        //#endregion

        //#region field10

        //fields f10 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
        //    90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
        //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


        //#endregion

        //#endregion

        //#region Worker

        //workers worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

        //#endregion

        //#region site
        //sites site = await testHelpers.CreateSite("SiteName", 88);

        //#endregion

        //#region units
        //units unit = await testHelpers.CreateUnit(48, 49, site, 348);

        //#endregion

        //#region site_workers
        //site_workers site_workers = await testHelpers.CreateSiteWorker(55, site, worker);

        //#endregion

        //#region cases
        //#region cases created
        //#region Case1

        //DateTime c1_ca = DateTime.Now.AddDays(-9);
        //DateTime c1_da = DateTime.Now.AddDays(-8).AddHours(-12);
        //DateTime c1_ua = DateTime.Now.AddDays(-8);

        //cases aCase1 = await testHelpers.CreateCase("case1UId", cl2, c1_ca, "custom1",
        //    c1_da, worker, "microtingCheckUId1", "microtingUId1",
        //   site, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);

        //#endregion


        //#endregion

        //#endregion

        //#region UploadedData
        //#region ud1
        //uploaded_data ud1 = await testHelpers.CreateUploadedData("checksum1", "File1", "no", "hjgjghjhg", "File1", 1, worker,
        //    "local", 55);
        //#endregion

        //#region ud2
        //uploaded_data ud2 = await testHelpers.CreateUploadedData("checksum2", "File1", "no", "hjgjghjhg", "File2", 1, worker,
        //    "local", 55);
        //#endregion

        //#region ud3
        //uploaded_data ud3 = await testHelpers.CreateUploadedData("checksum3", "File1", "no", "hjgjghjhg", "File3", 1, worker,
        //    "local", 55);
        //#endregion

        //#region ud4
        //uploaded_data ud4 = await testHelpers.CreateUploadedData("checksum4", "File1", "no", "hjgjghjhg", "File4", 1, worker,
        //    "local", 55);
        //#endregion

        //#region ud5
        //uploaded_data ud5 = await testHelpers.CreateUploadedData("checksum5", "File1", "no", "hjgjghjhg", "File5", 1, worker,
        //    "local", 55);
        //#endregion

        //#region ud6
        //uploaded_data ud6 = await testHelpers.CreateUploadedData("checksum6", "File1", "no", "hjgjghjhg", "File6", 1, worker,
        //    "local", 55);
        //#endregion

        //#region ud7
        //uploaded_data ud7 = await testHelpers.CreateUploadedData("checksum7", "File1", "no", "hjgjghjhg", "File7", 1, worker,
        //    "local", 55);
        //#endregion

        //#region ud8
        //uploaded_data ud8 = await testHelpers.CreateUploadedData("checksum8", "File1", "no", "hjgjghjhg", "File8", 1, worker,
        //    "local", 55);
        //#endregion

        //#region ud9
        //uploaded_data ud9 = await testHelpers.CreateUploadedData("checksum9", "File1", "no", "hjgjghjhg", "File9", 1, worker,
        //    "local", 55);
        //#endregion

        //#region ud10
        //uploaded_data ud10 = await testHelpers.CreateUploadedData("checksum10", "File1", "no", "hjgjghjhg", "File10", 1, worker,
        //    "local", 55);
        //#endregion

        //#endregion

        //#region Check List Values
        //#region clv1
        //check_list_values clv1 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
        //#endregion

        //#region clv2
        //check_list_values clv2 = await testHelpers.CreateCheckListValue(aCase1, cl3, Constants.CheckListValues.Checked, null, 861);
        //#endregion

        //#region clv3
        //check_list_values clv3 = await testHelpers.CreateCheckListValue(aCase1, cl4, Constants.CheckListValues.Checked, null, 862);
        //#endregion

        //#region clv4
        //check_list_values clv4 = await testHelpers.CreateCheckListValue(aCase1, cl5, Constants.CheckListValues.NotChecked, null, 863);
        //#endregion

        //#region clv5
        //check_list_values clv5 = await testHelpers.CreateCheckListValue(aCase1, cl6, Constants.CheckListValues.NotChecked, null, 864);
        //#endregion

        //#region clv6
        //check_list_values clv6 = await testHelpers.CreateCheckListValue(aCase1, cl7, Constants.CheckListValues.NotChecked, null, 865);
        //#endregion

        //#region clv7
        //check_list_values clv7 = await testHelpers.CreateCheckListValue(aCase1, cl8, Constants.CheckListValues.NotApproved, null, 866);
        //#endregion

        //#region clv8
        //check_list_values clv8 = await testHelpers.CreateCheckListValue(aCase1, cl9, Constants.CheckListValues.NotApproved, null, 867);
        //#endregion

        //#region clv9
        //check_list_values clv9 = await testHelpers.CreateCheckListValue(aCase1, cl10, Constants.CheckListValues.NotApproved, null, 868);
        //#endregion

        //#region clv10
        //check_list_values clv10 = await testHelpers.CreateCheckListValue(aCase1, cl11, Constants.CheckListValues.NotApproved, null, 869);
        //#endregion

        //#endregion

        //#region Field Values
        //#region fv1
        //field_values field_Value1 = await testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.Id, null, "tomt1", 61230, worker);

        //#endregion

        //#region fv2
        //field_values field_Value2 = await testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.Id, null, "tomt2", 61231, worker);

        //#endregion

        //#region fv3
        //field_values field_Value3 = await testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.Id, null, "tomt3", 61232, worker);

        //#endregion

        //#region fv4
        //field_values field_Value4 = await testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.Id, null, "tomt4", 61233, worker);

        //#endregion

        //#region fv5
        //field_values field_Value5 = await testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.Id, null, "tomt5", 61234, worker);

        //#endregion

        //#region fv6
        //field_values field_Value6 = await testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.Id, null, "tomt6", 61235, worker);

        //#endregion

        //#region fv7
        //field_values field_Value7 = await testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.Id, null, "tomt7", 61236, worker);

        //#endregion

        //#region fv8
        //field_values field_Value8 = await testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.Id, null, "tomt8", 61237, worker);

        //#endregion

        //#region fv9
        //field_values field_Value9 = await testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.Id, null, "tomt9", 61238, worker);

        //#endregion

        //#region fv10
        //field_values field_Value10 = await testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.Id, null, "tomt10", 61239, worker);

        //#endregion


        //#endregion

        //#region checkListSites
        //DateTime cls_ca = DateTime.Now;
        //DateTime cls_ua = DateTime.Now;
        //check_list_sites cls1 = await testHelpers.CreateCheckListSite(cl2, cls_ca, site,
        //   cls_ua, 5, Constants.WorkflowStates.Created);

        //#endregion
        //#endregion
        //// Act
        //var match = await sut.CaseDelete(cl2.Id, (int)cls1.site.microting_uid, Constants.WorkflowStates.Created);
        //// Assert
        // Assert.NotNull(match);
        // Assert.True(match);

        // }
// #pragma warning restore 1998

        [Test]
        public async Task Core_Case_CaseUpdateFieldValues()
        {
            // Arrance

            #region Arrance

            #region Template1

            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1

            CheckList cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//            check_lists cl3 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//            check_lists cl4 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//            check_lists cl5 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region Fields

            #region field1

            Field f1 = await testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "",
                "Comment field description",
                5, 1, DbContext.FieldTypes.Where(x => x.Type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55,
                "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2

            Field f2 = await testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "",
                "showPDf Description",
                45, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);

            #endregion

            #region field3

            Field f3 = await testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "",
                "Number Field Description",
                83, 0, DbContext.FieldTypes.Where(x => x.Type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);

            #endregion

            #region field4

            Field f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "",
                "date Description",
                84, 0, DbContext.FieldTypes.Where(x => x.Type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field5

            Field f5 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                85, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field6

            Field f6 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                86, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field7

            Field f7 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                87, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field8

            Field f8 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                88, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field9

            Field f9 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                89, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field10

            Field f10 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                90, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #endregion

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

//            #region site_workers
//            site_workers site_workers = await testHelpers.CreateSiteWorker(55, site, worker);
//
//            #endregion

            #region cases

            #region cases created

            #region Case1

            DateTime c1_ca = DateTime.Now.AddDays(-9);
            DateTime c1_da = DateTime.Now.AddDays(-8).AddHours(-12);
            DateTime c1_ua = DateTime.Now.AddDays(-8);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase1 = await testHelpers.CreateCase("case1UId", cl1,
                c1_ca, "custom1",
                c1_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

//            #region Case2
//
//            DateTime c2_ca = DateTime.Now.AddDays(-7);
//            DateTime c2_da = DateTime.Now.AddDays(-6).AddHours(-12);
//            DateTime c2_ua = DateTime.Now.AddDays(-6);
//            cases aCase2 = await testHelpers.CreateCase("case2UId", cl3, c2_ca, "custom2",
//             c2_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
//               site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);
//            #endregion
//
//            #region Case3
//            DateTime c3_ca = DateTime.Now.AddDays(-10);
//            DateTime c3_da = DateTime.Now.AddDays(-9).AddHours(-12);
//            DateTime c3_ua = DateTime.Now.AddDays(-9);
//
//            cases aCase3 = await testHelpers.CreateCase("case3UId", cl4, c3_ca, "custom3",
//              c3_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
//               site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);
//            #endregion
//
//            #region Case4
//            DateTime c4_ca = DateTime.Now.AddDays(-8);
//            DateTime c4_da = DateTime.Now.AddDays(-7).AddHours(-12);
//            DateTime c4_ua = DateTime.Now.AddDays(-7);
//
//            cases aCase4 = await testHelpers.CreateCase("case4UId", cl5, c4_ca, "custom4",
//                c4_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
//               site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);
//            #endregion

            #endregion

            #endregion

            #region UploadedData

            #region ud1

            UploadedData ud1 = await testHelpers.CreateUploadedData("checksum1", "File1", "no", "hjgjghjhg", "File1", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud2

            UploadedData ud2 = await testHelpers.CreateUploadedData("checksum2", "File1", "no", "hjgjghjhg", "File2", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud3

            UploadedData ud3 = await testHelpers.CreateUploadedData("checksum3", "File1", "no", "hjgjghjhg", "File3", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud4

            UploadedData ud4 = await testHelpers.CreateUploadedData("checksum4", "File1", "no", "hjgjghjhg", "File4", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud5

            UploadedData ud5 = await testHelpers.CreateUploadedData("checksum5", "File1", "no", "hjgjghjhg", "File5", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud6

            UploadedData ud6 = await testHelpers.CreateUploadedData("checksum6", "File1", "no", "hjgjghjhg", "File6", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud7

            UploadedData ud7 = await testHelpers.CreateUploadedData("checksum7", "File1", "no", "hjgjghjhg", "File7", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud8

            UploadedData ud8 = await testHelpers.CreateUploadedData("checksum8", "File1", "no", "hjgjghjhg", "File8", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud9

            UploadedData ud9 = await testHelpers.CreateUploadedData("checksum9", "File1", "no", "hjgjghjhg", "File9", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud10

            UploadedData ud10 = await testHelpers.CreateUploadedData("checksum10", "File1", "no", "hjgjghjhg", "File10",
                1, worker,
                "local", 55, false);

            #endregion

            #endregion

            #region Field Values

            #region fv1

            FieldValue field_Value1 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.Id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2

            FieldValue field_Value2 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.Id, null, "tomt2", 61234, worker);

            #endregion

            #region fv3

            FieldValue field_Value3 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.Id, null, "tomt3", 61234, worker);

            #endregion

            #region fv4

            FieldValue field_Value4 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.Id, null, "tomt4", 61234, worker);

            #endregion

            #region fv5

            FieldValue field_Value5 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.Id, null, "tomt5", 61234, worker);

            #endregion

            #region fv6

            FieldValue field_Value6 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.Id, null, "tomt6", 61234, worker);

            #endregion

            #region fv7

            FieldValue field_Value7 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.Id, null, "tomt7", 61234, worker);

            #endregion

            #region fv8

            FieldValue field_Value8 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.Id, null, "tomt8", 61234, worker);

            #endregion

            #region fv9

            FieldValue field_Value9 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.Id, null, "tomt9", 61234, worker);

            #endregion

            #region fv10

            FieldValue field_Value10 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.Id, null, "tomt10", 61234, worker);

            #endregion

            #endregion

            #endregion

            // Act
            Microting.eForm.Infrastructure.Data.Entities.Case theCase = DbContext.Cases.First();
            Assert.NotNull(theCase);
            CheckList theCheckList = DbContext.CheckLists.First();

            theCheckList.Field1 = f1.Id;
            theCheckList.Field2 = f2.Id;
            theCheckList.Field3 = f3.Id;
            theCheckList.Field4 = f4.Id;
            theCheckList.Field5 = f5.Id;
            theCheckList.Field6 = f6.Id;
            theCheckList.Field7 = f7.Id;
            theCheckList.Field8 = f8.Id;
            theCheckList.Field9 = f9.Id;
            theCheckList.Field10 = f10.Id;

            Assert.AreEqual(null, theCase.FieldValue1);
            Assert.AreEqual(null, theCase.FieldValue2);
            Assert.AreEqual(null, theCase.FieldValue3);
            Assert.AreEqual(null, theCase.FieldValue4);
            Assert.AreEqual(null, theCase.FieldValue5);
            Assert.AreEqual(null, theCase.FieldValue6);
            Assert.AreEqual(null, theCase.FieldValue7);
            Assert.AreEqual(null, theCase.FieldValue8);
            Assert.AreEqual(null, theCase.FieldValue9);
            Assert.AreEqual(null, theCase.FieldValue10);

            var testThis = await sut.CaseUpdateFieldValues(aCase1.Id, language);

            // Assert
            Microting.eForm.Infrastructure.Data.Entities.Case theCaseAfter = DbContext.Cases.AsNoTracking().First();

            Assert.NotNull(theCaseAfter);

            theCaseAfter.FieldValue1 = field_Value1.Value;
            theCaseAfter.FieldValue2 = field_Value2.Value;
            theCaseAfter.FieldValue3 = field_Value3.Value;
            theCaseAfter.FieldValue4 = field_Value4.Value;
            theCaseAfter.FieldValue5 = field_Value5.Value;
            theCaseAfter.FieldValue6 = field_Value6.Value;
            theCaseAfter.FieldValue7 = field_Value7.Value;
            theCaseAfter.FieldValue8 = field_Value8.Value;
            theCaseAfter.FieldValue9 = field_Value9.Value;
            theCaseAfter.FieldValue10 = field_Value10.Value;


            Assert.True(testThis);

            Assert.AreEqual("tomt1", theCaseAfter.FieldValue1);
            Assert.AreEqual("tomt2", theCaseAfter.FieldValue2);
            Assert.AreEqual("tomt3", theCaseAfter.FieldValue3);
            Assert.AreEqual("tomt4", theCaseAfter.FieldValue4);
            Assert.AreEqual("tomt5", theCaseAfter.FieldValue5);
            Assert.AreEqual("tomt6", theCaseAfter.FieldValue6);
            Assert.AreEqual("tomt7", theCaseAfter.FieldValue7);
            Assert.AreEqual("tomt8", theCaseAfter.FieldValue8);
            Assert.AreEqual("tomt9", theCaseAfter.FieldValue9);
            Assert.AreEqual("tomt10", theCaseAfter.FieldValue10);
        }

        [Test]
        public async Task Core_Case_CaseLookupMUId_Returns_ReturnCase()
        {
            // Arrance

            #region Arrance

            #region Template1

            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region Case1

            Microting.eForm.Infrastructure.Data.Entities.Case aCase = await testHelpers.CreateCase("caseUId", cl1,
                DateTime.Now, "custom", DateTime.Now,
                worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 66, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion


            // Act

            var match = await sut.CaseLookupMUId((int)aCase.MicrotingUid);

            // Assert

            Assert.AreEqual(aCase.MicrotingUid, match.MicrotingUId);
        }

        [Test]
        public async Task Core_Case_CaseLookupCaseId_Returns_cDto()
        {
            // Arrance

            #region Arrance

            #region Template1

            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region Case1

            Microting.eForm.Infrastructure.Data.Entities.Case aCase = await testHelpers.CreateCase("caseUId", cl1,
                DateTime.Now, "custom", DateTime.Now,
                worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 66, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion


            // Act

            var match = await sut.CaseLookupCaseId(aCase.Id);

            // Assert

            Assert.AreEqual(aCase.Id, match.CaseId);
        }

        [Test]
        public async Task Core_Case_CaseLookupCaseUId_Returns_lstDto()
        {
            // Arrance

            #region Arrance

            #region Template1

            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region Case1

            Microting.eForm.Infrastructure.Data.Entities.Case aCase = await testHelpers.CreateCase("caseUId", cl1,
                DateTime.Now, "custom", DateTime.Now,
                worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 66, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion


            // Act

            var match = await sut.CaseLookupCaseUId(aCase.CaseUid);


            // Assert

            Assert.AreEqual(aCase.CaseUid, match[0].CaseUId);
        }

        [Test]
        public async Task Core_Case_CaseIdLookUp_returnsId()
        {
            // Arrange

            #region Arrance

            #region Template1

            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region cases

            #region cases created

            #region Case1

            DateTime c1_ca = DateTime.Now.AddDays(-9);
            DateTime c1_da = DateTime.Now.AddDays(-8).AddHours(-12);
            DateTime c1_ua = DateTime.Now.AddDays(-8);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase1 = await testHelpers.CreateCase("case1UId", cl1,
                c1_ca, "custom1",
                c1_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            #endregion

            #endregion

            // Act
            var match = await sut.CaseIdLookup((int)aCase1.MicrotingUid, (int)aCase1.MicrotingCheckUid);
            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(aCase1.Id, match);
        }

        #region Core_Case_CasesToExcel_returnsPathAndName

//        [Test]
//        public async Task Core_Case_CasesToExcel_returnsPathAndName()
//        {
//            // Arrange
//            #region Arrance
//            #region Template1
//            DateTime cl1_Ca = DateTime.Now;
//            DateTime cl1_Ua = DateTime.Now;
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
//                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field6
//
//            fields f6 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field7
//
//            fields f7 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field8
//
//            fields f8 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field9
//
//            fields f9 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field10
//
//            fields f10 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
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
//
//            #region site
//            sites site = await testHelpers.CreateSite("SiteName", 88);
//
//            #endregion
//
//            #region units
//            units unit = await testHelpers.CreateUnit(48, 49, site, 348);
//
//            #endregion
//
//            #region site_workers
//            site_workers site_workers = await testHelpers.CreateSiteWorker(55, site, worker);
//
//            #endregion
//
//            #region cases
//            #region cases created
//            #region Case1
//
//            DateTime c1_ca = DateTime.Now.AddDays(-9);
//            DateTime c1_da = DateTime.Now.AddDays(-8).AddHours(-12);
//            DateTime c1_ua = DateTime.Now.AddDays(-8);
//
//            cases aCase1 = await testHelpers.CreateCase("case1UId", cl1, c1_ca, "custom1",
//                c1_da, worker, "microtingCheckUId1", "microtingUId1",
//               site, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);
//
//            #endregion
//
//            #region Case2
//
//            DateTime c2_ca = DateTime.Now.AddDays(-7);
//            DateTime c2_da = DateTime.Now.AddDays(-6).AddHours(-12);
//            DateTime c2_ua = DateTime.Now.AddDays(-6);
//            cases aCase2 = await testHelpers.CreateCase("case2UId", cl3, c2_ca, "custom2",
//             c2_da, worker, "microtingCheck2UId", "microting2UId",
//               site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);
//            #endregion
//
//            #region Case3
//            DateTime c3_ca = DateTime.Now.AddDays(-10);
//            DateTime c3_da = DateTime.Now.AddDays(-9).AddHours(-12);
//            DateTime c3_ua = DateTime.Now.AddDays(-9);
//
//            cases aCase3 = await testHelpers.CreateCase("case3UId", cl4, c3_ca, "custom3",
//              c3_da, worker, "microtingCheck3UId", "microtin3gUId",
//               site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);
//            #endregion
//
//            #region Case4
//            DateTime c4_ca = DateTime.Now.AddDays(-8);
//            DateTime c4_da = DateTime.Now.AddDays(-7).AddHours(-12);
//            DateTime c4_ua = DateTime.Now.AddDays(-7);
//
//            cases aCase4 = await testHelpers.CreateCase("case4UId", cl5, c4_ca, "custom4",
//                c4_da, worker, "microtingCheck4UId", "microting4UId",
//               site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);
//            #endregion
//            #endregion
//
//            #endregion
//
//            #region UploadedData
//            #region ud1
//            uploaded_data ud1 = await testHelpers.CreateUploadedData("checksum1", "File1", "no", @"C:\Users\soipi\Desktop", "File1", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud2
//            uploaded_data ud2 = await testHelpers.CreateUploadedData("checksum2", "File1", "no", @"C:\Users\soipi\Desktop", "File2", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud3
//            uploaded_data ud3 = await testHelpers.CreateUploadedData("checksum3", "File1", "no", @"C:\Users\soipi\Desktop", "File3", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud4
//            uploaded_data ud4 = await testHelpers.CreateUploadedData("checksum4", "File1", "no", @"C: \Users\soipi\Desktop", "File4", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud5
//            uploaded_data ud5 = await testHelpers.CreateUploadedData("checksum5", "File1", "no", @"C:\Users\soipi\Desktop", "File5", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud6
//            uploaded_data ud6 = await testHelpers.CreateUploadedData("checksum6", "File1", "no", @"C:\Users\soipi\Desktop", "File6", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud7
//            uploaded_data ud7 = await testHelpers.CreateUploadedData("checksum7", "File1", "no", @"C:\Users\soipi\Desktop", "File7", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud8
//            uploaded_data ud8 = await testHelpers.CreateUploadedData("checksum8", "File1", "no", @"C:\Users\soipi\Desktop", "File8", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud9
//            uploaded_data ud9 = await testHelpers.CreateUploadedData("checksum9", "File1", "no", @"C:\Users\soipi\Desktop", "File9", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud10
//            uploaded_data ud10 = await testHelpers.CreateUploadedData("checksum10", "File1", "no", @"C:\Users\soipi\Desktop", "File10", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #endregion
//
//            #region Check List Values
//            #region clv1
//            check_list_values clv1 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
//            #endregion
//
//            #region clv2
//            check_list_values clv2 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 861);
//            #endregion
//
//            #region clv3
//            check_list_values clv3 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 862);
//            #endregion
//
//            #region clv4
//            check_list_values clv4 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 863);
//            #endregion
//
//            #region clv5
//            check_list_values clv5 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 864);
//            #endregion
//
//            #region clv6
//            check_list_values clv6 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 865);
//            #endregion
//
//            #region clv7
//            check_list_values clv7 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 866);
//            #endregion
//
//            #region clv8
//            check_list_values clv8 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 867);
//            #endregion
//
//            #region clv9
//            check_list_values clv9 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 868);
//            #endregion
//
//            #region clv10
//            check_list_values clv10 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 869);
//            #endregion
//
//            #endregion
//
//            #region Field Values
//            #region fv1
//            field_values field_Value1 = await testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.Id, null, "tomt1", 61230, worker);
//
//            #endregion
//
//            #region fv2
//            field_values field_Value2 = await testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.Id, null, "tomt2", 61231, worker);
//
//            #endregion
//
//            #region fv3
//            field_values field_Value3 = await testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.Id, null, "tomt3", 61232, worker);
//
//            #endregion
//
//            #region fv4
//            field_values field_Value4 = await testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.Id, null, "tomt4", 61233, worker);
//
//            #endregion
//
//            #region fv5
//            field_values field_Value5 = await testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.Id, null, "tomt5", 61234, worker);
//
//            #endregion
//
//            #region fv6
//            field_values field_Value6 = await testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.Id, null, "tomt6", 61235, worker);
//
//            #endregion
//
//            #region fv7
//            field_values field_Value7 = await testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.Id, null, "tomt7", 61236, worker);
//
//            #endregion
//
//            #region fv8
//            field_values field_Value8 = await testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.Id, null, "tomt8", 61237, worker);
//
//            #endregion
//
//            #region fv9
//            field_values field_Value9 = await testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.Id, null, "tomt9", 61238, worker);
//
//            #endregion
//
//            #region fv10
//            field_values field_Value10 = await testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.Id, null, "tomt10", 61239, worker);
//
//            #endregion
//
//
//            #endregion
//
//            #region checkListSites
//            DateTime cls_ca = DateTime.Now;
//            DateTime cls_ua = DateTime.Now;
//            string microtingUid = Guid.NewGuid().ToString();
//            check_list_sites cls1 = await testHelpers.CreateCheckListSite(cl2, cls_ca, site,
//               cls_ua, 5, Constants.WorkflowStates.Created, microtingUid);
//
//            #endregion
//            #endregion
//            // Act
//
//            //var match = await sut.CasesToExcel(aCase1.check_list_id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(1), ud1.file_location + ud1.file_name, "mappe/");
//
//            //// Assert
//            // Assert.NotNull(match);
//            // Assert.AreEqual(match, "C:\\Users\\soipi\\DesktopFile1.xlsx");
//
//
//        }

        #endregion

        // [Test]
        // public async Task Core_Case_CasesToCsv_returnsPathAndName()
        // {
        //     // Arrange
        //     #region Arrance
//            #region Template1
//            DateTime cl1_Ca = DateTime.Now;
//            DateTime cl1_Ua = DateTime.Now;
//            check_lists cl1 = await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);
//
//            #endregion

        //#region subtemplates
//            #region SubTemplate1
//            check_lists cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion

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
        //#endregion

//            #region Fields
//            #region field1
//
//
//            fields f1 = await testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field6
//
//            fields f6 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                86, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field7
//
//            fields f7 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                87, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field8
//
//            fields f8 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                88, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field9
//
//            fields f9 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                89, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field10
//
//            fields f10 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                90, 0, dbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #endregion

//            #region Worker
//
//            workers worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
//
//            #endregion
//
//            #region site
//            sites site = await testHelpers.CreateSite("SiteName", 88);
//
//            #endregion
//
//            #region units
//            units unit = await testHelpers.CreateUnit(48, 49, site, 348);
//
//            #endregion

//            #region site_workers
//            site_workers site_workers = await testHelpers.CreateSiteWorker(55, site, worker);
//
//            #endregion

//             #region cases
//             #region cases created
// //            #region Case1
// //
// //            DateTime c1_ca = DateTime.Now.AddDays(-9);
// //            DateTime c1_da = DateTime.Now.AddDays(-8).AddHours(-12);
// //            DateTime c1_ua = DateTime.Now.AddDays(-8);
// //
// //            cases aCase1 = await testHelpers.CreateCase("case1UId", cl1, c1_ca, "custom1",
// //                c1_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
// //               site, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);
// //
// //            #endregion
//
// //            #region Case2
// //
// //            DateTime c2_ca = DateTime.Now.AddDays(-7);
// //            DateTime c2_da = DateTime.Now.AddDays(-6).AddHours(-12);
// //            DateTime c2_ua = DateTime.Now.AddDays(-6);
// //            cases aCase2 = await testHelpers.CreateCase("case2UId", cl3, c2_ca, "custom2",
// //             c2_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
// //               site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);
// //            #endregion
// //
// //            #region Case3
// //            DateTime c3_ca = DateTime.Now.AddDays(-10);
// //            DateTime c3_da = DateTime.Now.AddDays(-9).AddHours(-12);
// //            DateTime c3_ua = DateTime.Now.AddDays(-9);
// //
// //            cases aCase3 = await testHelpers.CreateCase("case3UId", cl4, c3_ca, "custom3",
// //              c3_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
// //               site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);
// //            #endregion
// //
// //            #region Case4
// //            DateTime c4_ca = DateTime.Now.AddDays(-8);
// //            DateTime c4_da = DateTime.Now.AddDays(-7).AddHours(-12);
// //            DateTime c4_ua = DateTime.Now.AddDays(-7);
// //
// //            cases aCase4 = await testHelpers.CreateCase("case4UId", cl5, c4_ca, "custom4",
// //                c4_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
// //               site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);
// //            #endregion
//             #endregion
//
//             #endregion

//            #region UploadedData
//            #region ud1
//            uploaded_data ud1 = await testHelpers.CreateUploadedData("checksum1", "File1", "no", @"C:\Users\soipi\Desktop", "File1", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud2
//            uploaded_data ud2 = await testHelpers.CreateUploadedData("checksum2", "File1", "no", @"C:\Users\soipi\Desktop", "File2", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud3
//            uploaded_data ud3 = await testHelpers.CreateUploadedData("checksum3", "File1", "no", @"C:\Users\soipi\Desktop", "File3", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud4
//            uploaded_data ud4 = await testHelpers.CreateUploadedData("checksum4", "File1", "no", @"C: \Users\soipi\Desktop", "File4", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud5
//            uploaded_data ud5 = await testHelpers.CreateUploadedData("checksum5", "File1", "no", @"C:\Users\soipi\Desktop", "File5", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud6
//            uploaded_data ud6 = await testHelpers.CreateUploadedData("checksum6", "File1", "no", @"C:\Users\soipi\Desktop", "File6", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud7
//            uploaded_data ud7 = await testHelpers.CreateUploadedData("checksum7", "File1", "no", @"C:\Users\soipi\Desktop", "File7", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud8
//            uploaded_data ud8 = await testHelpers.CreateUploadedData("checksum8", "File1", "no", @"C:\Users\soipi\Desktop", "File8", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud9
//            uploaded_data ud9 = await testHelpers.CreateUploadedData("checksum9", "File1", "no", @"C:\Users\soipi\Desktop", "File9", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud10
//            uploaded_data ud10 = await testHelpers.CreateUploadedData("checksum10", "File1", "no", @"C:\Users\soipi\Desktop", "File10", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #endregion

//            #region Check List Values
//            #region clv1
//            check_list_values clv1 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
//            #endregion
//
//            #region clv2
//            check_list_values clv2 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 861);
//            #endregion
//
//            #region clv3
//            check_list_values clv3 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 862);
//            #endregion
//
//            #region clv4
//            check_list_values clv4 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 863);
//            #endregion
//
//            #region clv5
//            check_list_values clv5 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 864);
//            #endregion
//
//            #region clv6
//            check_list_values clv6 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 865);
//            #endregion
//
//            #region clv7
//            check_list_values clv7 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 866);
//            #endregion
//
//            #region clv8
//            check_list_values clv8 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 867);
//            #endregion
//
//            #region clv9
//            check_list_values clv9 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 868);
//            #endregion
//
//            #region clv10
//            check_list_values clv10 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 869);
//            #endregion
//
//            #endregion

//            #region Field Values
//            #region fv1
//            field_values field_Value1 = await testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.Id, null, "tomt1", 61230, worker);
//
//            #endregion
//
//            #region fv2
//            field_values field_Value2 = await testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.Id, null, "tomt2", 61231, worker);
//
//            #endregion
//
//            #region fv3
//            field_values field_Value3 = await testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.Id, null, "tomt3", 61232, worker);
//
//            #endregion
//
//            #region fv4
//            field_values field_Value4 = await testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.Id, null, "tomt4", 61233, worker);
//
//            #endregion
//
//            #region fv5
//            field_values field_Value5 = await testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.Id, null, "tomt5", 61234, worker);
//
//            #endregion
//
//            #region fv6
//            field_values field_Value6 = await testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.Id, null, "tomt6", 61235, worker);
//
//            #endregion
//
//            #region fv7
//            field_values field_Value7 = await testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.Id, null, "tomt7", 61236, worker);
//
//            #endregion
//
//            #region fv8
//            field_values field_Value8 = await testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.Id, null, "tomt8", 61237, worker);
//
//            #endregion
//
//            #region fv9
//            field_values field_Value9 = await testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.Id, null, "tomt9", 61238, worker);
//
//            #endregion
//
//            #region fv10
//            field_values field_Value10 = await testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.Id, null, "tomt10", 61239, worker);
//
//            #endregion
//
//
//            #endregion

//            #region checkListSites
//            DateTime cls_ca = DateTime.Now;
//            DateTime cls_ua = DateTime.Now;
//            int microtingUid = rnd.Next(1,255);
//            check_list_sites cls1 = await testHelpers.CreateCheckListSite(cl2, cls_ca, site,
//               cls_ua, 5, Constants.WorkflowStates.Created, microtingUid);
//
//            #endregion
        //#endregion
        // Act

        //var match = await sut.CasesToCsv(aCase1.check_list_id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(1), ud1.file_location + ud1.file_name, "mappe/");

        // Assert
        // Assert.NotNull(match);
        // Assert.AreEqual(match, "C:\\Users\\soipi\\DesktopFile1.csv");


        //}
        [Test]
        public async Task Core_Case_CaseToJasperXml_ReturnsPath()
        {
            // Arrange

            #region Arrance

            #region Template1

            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates

            #region SubTemplate1

            CheckList cl3 = await testHelpers.CreateSubTemplate("A.2", "D.2", "CheckList", 1, 1, cl1);

            #endregion

            #endregion


            #region Worker

            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site

            Site site = await testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site, 348);

            #endregion


            #region cases

            #region cases created

            #region Case2

            DateTime c2_ca = DateTime.Now.AddDays(-7);
            DateTime c2_da = DateTime.Now.AddDays(-6).AddHours(-12);
            DateTime c2_ua = DateTime.Now.AddDays(-6);
            Microting.eForm.Infrastructure.Data.Entities.Case aCase2 = await testHelpers.CreateCase("case2UId", cl3,
                c2_ca, "custom2",
                c2_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            #endregion

            #endregion

            // Act

            string timeStamp = DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("hhmmss");
            string pdfPath = Path.Combine(Path.GetTempPath(), "results",
                $"{timeStamp}_{aCase2.Id}.xml");
            CaseDto cDto = await sut.CaseLookupCaseId(aCase2.Id);
            ReplyElement reply = await sut.CaseRead((int)cDto.MicrotingUId, (int)cDto.CheckUId, language);
            var match = await sut.CaseToJasperXml(cDto, reply, aCase2.Id, timeStamp, pdfPath, "", language);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match, pdfPath);
        }

        [Test]
        public async Task Core_Case_GetJasperPath_returnsPath()
        {
            // Arrange

            #region Arrance

            #endregion

            // Act

            var match = await sut.GetSdkSetting(Settings.fileLocationJasper);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match, Path.Combine(path, "output", "dataFolder", "reports"));
        }

        [Test]
        public async Task Core_Case_SetJasperPath_returnsTrue()
        {
            // Arrange

            // Act
            var match = await sut.SetSdkSetting(Settings.fileLocationJasper, @"C:\local\gitgud");
            // Assert
            Assert.NotNull(match);
            Assert.True(match);
        }

        [Test]
        public async Task Core_Case_GetPicturePath_returnsPath()
        {
            // Arrange

            #region Arrance

            #endregion

            // Act

            var match = await sut.GetSdkSetting(Settings.fileLocationPicture);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match, Path.Combine(path, "output", "dataFolder", "picture"));
        }

        [Test]
        public async Task Core_Case_SetPicturePath_returnsTrue()
        {
            // Arrange

            // Act
            var match = await sut.SetSdkSetting(Settings.fileLocationPicture, @"C:\local");
            // Assert
            Assert.NotNull(match);
            Assert.True(match);
        }

        [Test]
        public async Task Core_Case_GetPdfPath_returnsPath()
        {
            // Arrange

            #region Arrance

            #endregion

            // Act

            var match = await sut.GetSdkSetting(Settings.fileLocationPdf);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match, Path.Combine(path, "output", "dataFolder", "pdf"));
        }
        // [Test]
        // public async Task Core_Case_SetHttpServerAddress_ReturnsTrue()
        // {
        //     // Arrange
        //
        //     // Act
        //     var match = await sut.SetSdkSetting(Settings.httpServerAddress, "http://localhost:3000");
        //     // Assert
        //     Assert.NotNull(match);
        //     Assert.True(match);
        // }
        // [Test]
        // public async Task Core_Case_GetHttpServerAddress_returnsPath()
        // {
        //     // Arrange
        //     #region Arrance
        //
        //     var setting = DbContext.Settings.Single(x => x.Name == Settings.httpServerAddress.ToString());
        //     setting.Value = "http://localhost:3000";
        //     await DbContext.SaveChangesAsync();
        //     #endregion
        //     // Act
        //
        //     var match = await sut.GetSdkSetting(Settings.httpServerAddress);
        //
        //     // Assert
        //     Assert.NotNull(match);
        //     Assert.AreEqual("http://localhost:3000", match);
        //
        //
        // }

        [Test] // TODO add jaxml files
#pragma warning disable 1998
        public async Task Core_Case_CaseToPdf_returns_Path()
        {
            // Arrange

            // Act

            // Assert
        }
#pragma warning restore 1998

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