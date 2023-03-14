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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using eFormCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using NUnit.Framework;

namespace eFormSDK.Integration.Case.CoreTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class CoreTestCaseReadAllMedium : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
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

        [Test]
        public async Task Core_Case_CaseReadAll_Medium()
        {
            // Arrance

            #region Arrance

            Random rnd = new Random();

            #region Template1

            DateTime c1_Ca = DateTime.UtcNow;
            DateTime c1_Ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(c1_Ca, c1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1

            CheckList cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

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

            #region site_workers

            SiteWorker siteWorker = await testHelpers.CreateSiteWorker(55, site, worker);

            #endregion

            #region cases

            #region cases created

            #region Case1

            DateTime c1_ca = DateTime.UtcNow.AddDays(-9);
            DateTime c1_da = DateTime.UtcNow.AddDays(-8).AddHours(-12);
            DateTime c1_ua = DateTime.UtcNow.AddDays(-8);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase1 = await testHelpers.CreateCase("case1UId", cl1,
                c1_ca, "custom1",
                c1_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Case2

            DateTime c2_ca = DateTime.UtcNow.AddDays(-7);
            DateTime c2_da = DateTime.UtcNow.AddDays(-6).AddHours(-12);
            DateTime c2_ua = DateTime.UtcNow.AddDays(-6);
            Microting.eForm.Infrastructure.Data.Entities.Case aCase2 = await testHelpers.CreateCase("case2UId", cl1,
                c2_ca, "custom2",
                c2_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Case3

            DateTime c3_ca = DateTime.UtcNow.AddDays(-10);
            DateTime c3_da = DateTime.UtcNow.AddDays(-9).AddHours(-12);
            DateTime c3_ua = DateTime.UtcNow.AddDays(-9);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase3 = await testHelpers.CreateCase("case3UId", cl1,
                c3_ca, "custom3",
                c3_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Case4

            DateTime c4_ca = DateTime.UtcNow.AddDays(-8);
            DateTime c4_da = DateTime.UtcNow.AddDays(-7).AddHours(-12);
            DateTime c4_ua = DateTime.UtcNow.AddDays(-7);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase4 = await testHelpers.CreateCase("case4UId", cl1,
                c4_ca, "custom4",
                c4_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            #region cases removed

            #region Case1Removed

            DateTime c1Removed_ca = DateTime.UtcNow.AddDays(-9);
            DateTime c1Removed_da = DateTime.UtcNow.AddDays(-8).AddHours(-12);
            DateTime c1Removed_ua = DateTime.UtcNow.AddDays(-8);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase1Removed = await testHelpers.CreateCase("case1UId",
                cl1, c1Removed_ca, "custom1",
                c1Removed_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 1, "caseType1", unit, c1Removed_ua, 1, worker, Constants.WorkflowStates.Removed);

            #endregion

            #region Case2Removed

            DateTime c2Removed_ca = DateTime.UtcNow.AddDays(-7);
            DateTime c2Removed_da = DateTime.UtcNow.AddDays(-6).AddHours(-12);
            DateTime c2Removed_ua = DateTime.UtcNow.AddDays(-6);
            Microting.eForm.Infrastructure.Data.Entities.Case aCase2Removed = await testHelpers.CreateCase("case2UId",
                cl1, c2Removed_ca, "custom2",
                c2Removed_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 10, "caseType2", unit, c2Removed_ua, 1, worker, Constants.WorkflowStates.Removed);

            #endregion

            #region Case3Removed

            DateTime c3Removed_ca = DateTime.UtcNow.AddDays(-10);
            DateTime c3Removed_da = DateTime.UtcNow.AddDays(-9).AddHours(-12);
            DateTime c3Removed_ua = DateTime.UtcNow.AddDays(-9);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase3Removed = await testHelpers.CreateCase("case3UId",
                cl1, c3Removed_ca, "custom3",
                c3Removed_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 15, "caseType3", unit, c3Removed_ua, 1, worker, Constants.WorkflowStates.Removed);

            #endregion

            #region Case4Removed

            DateTime c4Removed_ca = DateTime.UtcNow.AddDays(-8);
            DateTime c4Removed_da = DateTime.UtcNow.AddDays(-7).AddHours(-12);
            DateTime c4Removed_ua = DateTime.UtcNow.AddDays(-7);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase4Removed = await testHelpers.CreateCase("case4UId",
                cl1, c4Removed_ca, "custom4",
                c4Removed_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 100, "caseType4", unit, c4Removed_ua, 1, worker, Constants.WorkflowStates.Removed);

            #endregion

            #endregion

            #region cases Retracted

            #region Case1Retracted

            DateTime c1Retracted_ca = DateTime.UtcNow.AddDays(-9);
            DateTime c1Retracted_da = DateTime.UtcNow.AddDays(-8).AddHours(-12);
            DateTime c1Retracted_ua = DateTime.UtcNow.AddDays(-8);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase1Retracted = await testHelpers.CreateCase("case1UId",
                cl1, c1Retracted_ca, "custom1",
                c1Retracted_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 1, "caseType1", unit, c1Retracted_ua, 1, worker, Constants.WorkflowStates.Retracted);

            #endregion

            #region Case2Retracted

            DateTime c2Retracted_ca = DateTime.UtcNow.AddDays(-7);
            DateTime c2Retracted_da = DateTime.UtcNow.AddDays(-6).AddHours(-12);
            DateTime c2Retracted_ua = DateTime.UtcNow.AddDays(-6);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase2Retracted = await testHelpers.CreateCase("case2UId",
                cl1, c2Retracted_ca, "custom2",
                c2Retracted_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 10, "caseType2", unit, c2Retracted_ua, 1, worker, Constants.WorkflowStates.Retracted);

            #endregion

            #region Case3Retracted

            DateTime c3Retracted_ca = DateTime.UtcNow.AddDays(-10);
            DateTime c3Retracted_da = DateTime.UtcNow.AddDays(-9).AddHours(-12);
            DateTime c3Retracted_ua = DateTime.UtcNow.AddDays(-9);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase3Retracted = await testHelpers.CreateCase("case3UId",
                cl1, c3Retracted_ca, "custom3",
                c3Retracted_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 15, "caseType3", unit, c3Retracted_ua, 1, worker, Constants.WorkflowStates.Retracted);

            #endregion

            #region Case4Retracted

            DateTime c4Retracted_ca = DateTime.UtcNow.AddDays(-8);
            DateTime c4Retracted_da = DateTime.UtcNow.AddDays(-7).AddHours(-12);
            DateTime c4Retracted_ua = DateTime.UtcNow.AddDays(-7);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase4Retracted = await testHelpers.CreateCase("case4UId",
                cl1, c4Retracted_ca, "custom4",
                c4Retracted_da, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 100, "caseType4", unit, c4Retracted_ua, 1, worker, Constants.WorkflowStates.Retracted);

            #endregion

            #endregion

            #endregion

            #endregion

            // Act

            #region sorting by WorkflowState created

            #region Default sorting

            #region Default sorting ascending

            // Default sorting ascending
            //List<Case> caseListCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "", timeZoneInfo);
            //List<Case> caseListFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "", timeZoneInfo);
            //List<Case> caseListWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region Default sorting ascending, with DateTime

            // Default sorting ascending, with DateTime
            //List<Case> caseListDtCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "",
                timeZoneInfo);
            //List<Case> caseDtListFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseDtListFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseDtListSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "",
                timeZoneInfo);
            //List<Case> caseListDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region case sorting

            #region aCase sorting ascending

            #region aCase1 sorting ascendng

            //List<Case> caseListC1SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC1SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "1000", timeZoneInfo);
            //List<Case> caseListC1SortFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC1SortFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC1SortFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC1SortFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC1SortFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC1SortFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC1SortFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC1SortFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC1SortFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC1SortFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC1SortSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC1SortStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "1000", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC1SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "1000", timeZoneInfo);
            //List<Case> caseListC1SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng

            //List<Case> caseListC2SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC2SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "2000", timeZoneInfo);
            //List<Case> caseListC2SortFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC2SortFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC2SortFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC2SortFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC2SortFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC2SortFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC2SortFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC2SortFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC2SortFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC2SortFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC2SortSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC2SortStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "2000", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC2SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "2000", timeZoneInfo);
            //List<Case> caseListC2SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng

            //List<Case> caseListC3SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC3SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "3000", timeZoneInfo);
            //List<Case> caseListC3SortFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC3SortFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC3SortFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC3SortFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC3SortFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC3SortFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC3SortFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC3SortFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC3SortFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC3SortFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC3SortSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC3SortStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "3000", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC3SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "3000", timeZoneInfo);
            //List<Case> caseListC3SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng

            //List<Case> caseListC4SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC4SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "4000", timeZoneInfo);
            //List<Case> caseListC4SortFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC4SortFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC4SortFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC4SortFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC4SortFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC4SortFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC4SortFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC4SortFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC4SortFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC4SortFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC4SortSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC4SortStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "4000", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC4SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "4000", timeZoneInfo);
            //List<Case> caseListC4SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion


            #region aCase sorting ascending w. Dt

            #region aCase1 sorting ascendng w. Dt

            //List<Case> caseListC1SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC1SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1000",
                timeZoneInfo);
            //List<Case> caseListC1SortDtFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC1SortDtFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC1SortDtFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC1SortDtFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC1SortDtFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC1SortDtFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC1SortDtFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC1SortDtFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC1SortDtFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC1SortDtFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC1SortDtSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC1SortDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1000",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC1SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1000",
                timeZoneInfo);
            //List<Case> caseListC1SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng w. Dt

            //List<Case> caseListC2SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC2SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2000",
                timeZoneInfo);
            //List<Case> caseListC2SortDtFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC2SortDtFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC2SortDtFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC2SortDtFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC2SortDtFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC2SortDtFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC2SortDtFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC2SortDtFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC2SortDtFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC2SortDtFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC2SortDtSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC2SortDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2000",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC2SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2000",
                timeZoneInfo);
            //List<Case> caseListC2SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng w. Dt

            //List<Case> caseListC3SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC3SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3000",
                timeZoneInfo);
            //List<Case> caseListC3SortDtFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC3SortDtFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC3SortDtFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC3SortDtFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC3SortDtFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC3SortDtFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC3SortDtFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC3SortDtFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC3SortDtFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC3SortDtFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC3SortDtSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC3SortDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3000",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC3SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3000",
                timeZoneInfo);
            //List<Case> caseListC3SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng w. Dt

            //List<Case> caseListC4SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC4SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4000",
                timeZoneInfo);
            //List<Case> caseListC4SortDtFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC4SortDtFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC4SortDtFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC4SortDtFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC4SortDtFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC4SortDtFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC4SortDtFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC4SortDtFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC4SortDtFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC4SortDtFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC4SortDtSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC4SortDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4000",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC4SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4000",
                timeZoneInfo);
            //List<Case> caseListC4SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #endregion

            #endregion

            #region sorting by WorkflowState removed

            #region Default sorting

            #region Default sorting ascending

            // Default sorting ascending
            //List<Case> caseListRemovedCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedDoneAt =
                await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", timeZoneInfo);
            //List<Case> caseLisrtRemovedFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseLisrtRemovedFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseLisrtRemovedFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseLisrtRemovedFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseLisrtRemovedFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseLisrtRemovedFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseLisrtRemovedFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseLisrtRemovedFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseLisrtRemovedFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseLisrtRemovedFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedStatus =
                await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedUnitId =
                await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", timeZoneInfo);
            //List<Case> caseListRemovedWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region Default sorting ascending, with DateTime

            // Default sorting ascending, with DateTime
            //List<Case> caseListRemovedDtCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "",
                timeZoneInfo);
            //List<Case> caseDtListRemovedFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseDtListRemovedFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListRemovedFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListRemovedFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListRemovedFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListRemovedFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListRemovedFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListRemovedFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListRemovedFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListRemovedFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseDtListRemovedSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "",
                timeZoneInfo);
            //List<Case> caseListRemovedDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region case sorting

            #region aCase sorting ascending

            #region aCase1 sorting ascendng

            //List<Case> caseListRemovedC1SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "1000", timeZoneInfo);
            //List<Case> caseListRemovedC1SortFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC1SortFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC1SortFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC1SortFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC1SortFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC1SortFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC1SortFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC1SortFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC1SortFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC1SortFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedC1SortSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "1000", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "1000", timeZoneInfo);
            //List<Case> caseListRemovedC1SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng

            //List<Case> caseListRemovedC2SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "2000", timeZoneInfo);
            //List<Case> caseListRemovedC2SortFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC2SortFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC2SortFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC2SortFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC2SortFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC2SortFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC2SortFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC2SortFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC2SortFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC2SortFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedC2SortSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "2000", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "2000", timeZoneInfo);
            //List<Case> caseListRemovedC2SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng

            //List<Case> caseListRemovedC3SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "3000", timeZoneInfo);
            //List<Case> caseListRemovedC3SortFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC3SortFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC3SortFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC3SortFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC3SortFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC3SortFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC3SortFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC3SortFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC3SortFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC3SortFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedC3SortSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "3000", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "3000", timeZoneInfo);
            //List<Case> caseListRemovedC3SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng

            //List<Case> caseListRemovedC4SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "4000", timeZoneInfo);
            //List<Case> caseListRemovedC4SortFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC4SortFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC4SortFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC4SortFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC4SortFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC4SortFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC4SortFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC4SortFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC4SortFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC4SortFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC4SortSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "4000", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "4000", timeZoneInfo);
            //List<Case> caseListRemovedC4SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion


            #region aCase sorting ascending w. Dt

            #region aCase1 sorting ascendng w. Dt

            //List<Case> caseListRemovedC1SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1000",
                timeZoneInfo);
            //List<Case> caseListRemovedC1SortDtFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC1SortDtFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC1SortDtFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC1SortDtFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC1SortDtFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC1SortDtFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC1SortDtFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC1SortDtFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC1SortDtFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC1SortDtFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedC1SortDtSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1000",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1000",
                timeZoneInfo);
            //List<Case> caseListRemovedC1SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng w. Dt

            //List<Case> caseListRemovedC2SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2000",
                timeZoneInfo);
            //List<Case> caseListRemovedC2SortDtFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC2SortDtFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC2SortDtFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC2SortDtFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC2SortDtFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC2SortDtFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC2SortDtFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC2SortDtFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC2SortDtFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC2SortDtFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedC2SortDtSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2000",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2000",
                timeZoneInfo);
            //List<Case> caseListRemovedC2SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng w. Dt

            //List<Case> caseListRemovedC3SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3000",
                timeZoneInfo);
            //List<Case> caseListRemovedC3SortDtFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC3SortDtFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC3SortDtFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC3SortDtFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC3SortDtFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC3SortDtFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC3SortDtFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC3SortDtFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC3SortDtFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC3SortDtFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedC3SortDtSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3000",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3000",
                timeZoneInfo);
            //List<Case> caseListRemovedC3SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng w. Dt

            //List<Case> caseListRemovedC4SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4000",
                timeZoneInfo);
            //List<Case> caseListRemovedC4SortDtFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC4SortDtFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC4SortDtFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC4SortDtFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC4SortDtFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC4SortDtFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC4SortDtFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC4SortDtFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC4SortDtFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC4SortDtFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedC4SortDtSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4000",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4000",
                timeZoneInfo);
            //List<Case> caseListRemovedC4SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #endregion

            #endregion

            #region sorting by WorkflowState Retracted

            #region Default sorting

            #region Default sorting ascending

            // Default sorting ascending
            //List<Case> caseListRetractedCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedDoneAt =
                await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", timeZoneInfo);
            //List<Case> caseLisrtRetractedFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseLisrtRetractedFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseLisrtRetractedFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseLisrtRetractedFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseLisrtRetractedFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseLisrtRetractedFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseLisrtRetractedFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseLisrtRetractedFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseLisrtRetractedFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseLisrtRetractedFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRetractedSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedStatus =
                await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedUnitId =
                await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", timeZoneInfo);
            //List<Case> caseListRetractedWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region Default sorting ascending, with DateTime

            // Default sorting ascending, with DateTime
            //List<Case> caseDtListRetractedDtCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "",
                timeZoneInfo);
            //List<Case> caseDtListRetractedFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseDtListRetractedFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListRetractedFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListRetractedFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListRetractedFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListRetractedFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListRetractedFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListRetractedFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListRetractedFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListRetractedFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseDtListRemovedSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "",
                timeZoneInfo);
            //List<Case> caseDtListRetractedDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region case sorting

            #region aCase sorting ascending

            #region aCase1 sorting ascendng

            //List<Case> caseListRetractedC1SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "1000", timeZoneInfo);
            //List<Case> caseListRetractedC1SortFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRetractedC1SortFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRetractedC1SortFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRetractedC1SortFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRetractedC1SortFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRetractedC1SortFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRetractedC1SortFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRetractedC1SortFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRetractedC1SortFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRetractedC1SortFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRetractedC1SortSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "1000", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "1000", timeZoneInfo);
            //List<Case> caseListRetractedC1SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng

            //List<Case> caseListRetractedC2SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "2000", timeZoneInfo);
            //List<Case> caseListRetractedC2SortFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRetractedC2SortFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRetractedC2SortFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRetractedC2SortFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRetractedC2SortFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRetractedC2SortFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRetractedC2SortFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRetractedC2SortFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRetractedC2SortFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRetractedC2SortFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRetractedC2SortSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "2000", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "2000", timeZoneInfo);
            //List<Case> caseListRetractedC2SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng

            //List<Case> caseListRetractedC3SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "3000", timeZoneInfo);
            //List<Case> caseListRetractedC3SortFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRetractedC3SortFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRetractedC3SortFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRetractedC3SortFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRetractedC3SortFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRetractedC3SortFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRetractedC3SortFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRetractedC3SortFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRetractedC3SortFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRetractedC3SortFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRetractedC3SortSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "3000", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "3000", timeZoneInfo);
            //List<Case> caseListRetractedC3SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng

            //List<Case> caseListRetractedC4SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "4000", timeZoneInfo);
            //List<Case> caseListRetractedC4SortFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRetractedC4SortFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRetractedC4SortFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRetractedC4SortFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRetractedC4SortFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRetractedC4SortFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRetractedC4SortFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRetractedC4SortFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRetractedC4SortFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRetractedC4SortFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC4SortSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "4000", timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "4000", timeZoneInfo);
            //List<Case> caseListRetractedC4SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region aCase sorting ascending w. Dt

            #region aCase1 sorting ascendng w. Dt

            //List<Case> caseDtListRetractedC1SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1000",
                timeZoneInfo);
            //List<Case> caseDtListRetractedC1SortDtFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseDtListRetractedC1SortDtFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListRetractedC1SortDtFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListRetractedC1SortDtFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListRetractedC1SortDtFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListRetractedC1SortDtFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListRetractedC1SortDtFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListRetractedC1SortDtFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListRetractedC1SortDtFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListRetractedC1SortDtFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseDtListRetractedC1SortDtSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1000",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1000",
                timeZoneInfo);
            //List<Case> caseDtListRetractedC1SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng w. Dt

            //List<Case> caseDtListRetractedC2SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2000",
                timeZoneInfo);
            //List<Case> caseDtListRetractedC2SortDtFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseDtListRetractedC2SortDtFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListRetractedC2SortDtFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListRetractedC2SortDtFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListRetractedC2SortDtFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListRetractedC2SortDtFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListRetractedC2SortDtFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListRetractedC2SortDtFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListRetractedC2SortDtFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListRetractedC2SortDtFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseDtListRetractedC2SortDtSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2000",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2000",
                timeZoneInfo);
            //List<Case> caseDtListRetractedC2SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng w. Dt

            //List<Case> caseDtListRetractedC3SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3000",
                timeZoneInfo);
            //List<Case> caseDtListRetractedC3SortDtFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseDtListRetractedC3SortDtFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListRetractedC3SortDtFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListRetractedC3SortDtFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListRetractedC3SortDtFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListRetractedC3SortDtFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListRetractedC3SortDtFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListRetractedC3SortDtFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListRetractedC3SortDtFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListRetractedC3SortDtFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseDtListRetractedC3SortDtSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3000",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3000",
                timeZoneInfo);
            //List<Case> caseDtListRetractedC3SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng w. Dt

            //List<Case> caseDtListRetractedC4SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4000",
                timeZoneInfo);
            //List<Case> caseDtListRetractedC4SortDtFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseDtListRetractedC4SortDtFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListRetractedC4SortDtFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListRetractedC4SortDtFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListRetractedC4SortDtFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListRetractedC4SortDtFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListRetractedC4SortDtFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListRetractedC4SortDtFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListRetractedC4SortDtFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListRetractedC4SortDtFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseDtListRetractedC4SortDtSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDtStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4000",
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4000",
                timeZoneInfo);
            //List<Case> caseDtListRetractedC4SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #endregion

            #endregion

            // Assert

            #region sort by WorkflowState Created

            #region Def Sort

            #region Def Sort Asc

            #region caseListDoneAt Def Sort Asc

            #region caseListDoneAt aCase1

            Assert.NotNull(caseListDoneAt);
            Assert.AreEqual(4, caseListDoneAt.Count);
            Assert.AreEqual(aCase1.Type, caseListDoneAt[0].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListDoneAt[0].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListDoneAt[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListDoneAt[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListDoneAt[0].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListDoneAt[0].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListDoneAt[0].SiteName);
            Assert.AreEqual(aCase1.Status, caseListDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListDoneAt[0].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListDoneAt[0].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListDoneAt[0].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName, caseListDoneAt[0].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListDoneAt[0].WorkflowState);

            #endregion

            #region caseListDoneAt aCase2

            Assert.AreEqual(aCase2.Type, caseListDoneAt[1].CaseType);
            Assert.AreEqual(aCase2.CaseUid, caseListDoneAt[1].CaseUId);
            Assert.AreEqual(aCase2.MicrotingCheckUid, caseListDoneAt[1].CheckUIid);
            Assert.AreEqual(c2_ca.ToString(), caseListDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase2.Custom, caseListDoneAt[1].Custom);
            Assert.AreEqual(c2_da.ToString(), caseListDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase2.Id, caseListDoneAt[1].Id);
            Assert.AreEqual(aCase2.MicrotingUid, caseListDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase2.Site.MicrotingUid, caseListDoneAt[1].SiteId);
            Assert.AreEqual(aCase2.Site.Name, caseListDoneAt[1].SiteName);
            Assert.AreEqual(aCase2.Status, caseListDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListDoneAt[1].TemplatId);
            Assert.AreEqual(aCase2.Unit.MicrotingUid, caseListDoneAt[1].UnitId);
//            Assert.AreEqual(c2_ua.ToString(), caseListDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase2.Version, caseListDoneAt[1].Version);
            Assert.AreEqual(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName, caseListDoneAt[1].WorkerName);
            Assert.AreEqual(aCase2.WorkflowState, caseListDoneAt[1].WorkflowState);

            #endregion

            #region caseListDoneAt aCase3

            Assert.AreEqual(aCase3.Type, caseListDoneAt[2].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListDoneAt[2].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListDoneAt[2].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDoneAt[2].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListDoneAt[2].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDoneAt[2].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListDoneAt[2].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListDoneAt[2].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListDoneAt[2].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListDoneAt[2].SiteName);
            Assert.AreEqual(aCase3.Status, caseListDoneAt[2].Status);
            Assert.AreEqual(cl1.Id, caseListDoneAt[2].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListDoneAt[2].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListDoneAt[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListDoneAt[2].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName, caseListDoneAt[2].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListDoneAt[2].WorkflowState);

            #endregion

            #region caseListDoneAt aCase4

            Assert.AreEqual(aCase4.Type, caseListDoneAt[3].CaseType);
            Assert.AreEqual(aCase4.CaseUid, caseListDoneAt[3].CaseUId);
            Assert.AreEqual(aCase4.MicrotingCheckUid, caseListDoneAt[3].CheckUIid);
            Assert.AreEqual(c4_ca.ToString(), caseListDoneAt[3].CreatedAt.ToString());
            Assert.AreEqual(aCase4.Custom, caseListDoneAt[3].Custom);
            Assert.AreEqual(c4_da.ToString(), caseListDoneAt[3].DoneAt.ToString());
            Assert.AreEqual(aCase4.Id, caseListDoneAt[3].Id);
            Assert.AreEqual(aCase4.MicrotingUid, caseListDoneAt[3].MicrotingUId);
            Assert.AreEqual(aCase4.Site.MicrotingUid, caseListDoneAt[3].SiteId);
            Assert.AreEqual(aCase4.Site.Name, caseListDoneAt[3].SiteName);
            Assert.AreEqual(aCase4.Status, caseListDoneAt[3].Status);
            Assert.AreEqual(cl1.Id, caseListDoneAt[3].TemplatId);
            Assert.AreEqual(aCase4.Unit.MicrotingUid, caseListDoneAt[3].UnitId);
//            Assert.AreEqual(c4_ua.ToString(), caseListDoneAt[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase4.Version, caseListDoneAt[3].Version);
            Assert.AreEqual(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName, caseListDoneAt[3].WorkerName);
            Assert.AreEqual(aCase4.WorkflowState, caseListDoneAt[3].WorkflowState);

            #endregion

            #endregion

            #region caseListStatus Def Sort Asc

            #region caseListStatus aCase1

            Assert.NotNull(caseListStatus);
            Assert.AreEqual(4, caseListStatus.Count);
            Assert.AreEqual(aCase1.Type, caseListStatus[0].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListStatus[0].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListStatus[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListStatus[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListStatus[0].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListStatus[0].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListStatus[0].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListStatus[0].SiteName);
            Assert.AreEqual(aCase1.Status, caseListStatus[0].Status);
            Assert.AreEqual(aCase1.CheckListId, caseListStatus[0].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListStatus[0].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListStatus[0].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName, caseListStatus[0].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListStatus[0].WorkflowState);

            #endregion

            #region caseListStatus aCase2

            Assert.AreEqual(aCase2.Type, caseListStatus[1].CaseType);
            Assert.AreEqual(aCase2.CaseUid, caseListStatus[1].CaseUId);
            Assert.AreEqual(aCase2.MicrotingCheckUid, caseListStatus[1].CheckUIid);
            Assert.AreEqual(c2_ca.ToString(), caseListStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase2.Custom, caseListStatus[1].Custom);
            Assert.AreEqual(c2_da.ToString(), caseListStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase2.Id, caseListStatus[1].Id);
            Assert.AreEqual(aCase2.MicrotingUid, caseListStatus[1].MicrotingUId);
            Assert.AreEqual(aCase2.Site.MicrotingUid, caseListStatus[1].SiteId);
            Assert.AreEqual(aCase2.Site.Name, caseListStatus[1].SiteName);
            Assert.AreEqual(aCase2.Status, caseListStatus[1].Status);
            Assert.AreEqual(aCase2.CheckListId, caseListStatus[1].TemplatId);
            Assert.AreEqual(aCase2.Unit.MicrotingUid, caseListStatus[1].UnitId);
//            Assert.AreEqual(c2_ua.ToString(), caseListStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase2.Version, caseListStatus[1].Version);
            Assert.AreEqual(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName, caseListStatus[1].WorkerName);
            Assert.AreEqual(aCase2.WorkflowState, caseListStatus[1].WorkflowState);

            #endregion

            #region caseListStatus aCase3

            Assert.AreEqual(aCase3.Type, caseListStatus[2].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListStatus[2].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListStatus[2].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListStatus[2].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListStatus[2].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListStatus[2].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListStatus[2].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListStatus[2].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListStatus[2].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListStatus[2].SiteName);
            Assert.AreEqual(aCase3.Status, caseListStatus[2].Status);
            Assert.AreEqual(aCase3.CheckListId, caseListStatus[2].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListStatus[2].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListStatus[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListStatus[2].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName, caseListStatus[2].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListStatus[2].WorkflowState);

            #endregion

            #region caseListStatus aCase4

            Assert.AreEqual(aCase4.Type, caseListStatus[3].CaseType);
            Assert.AreEqual(aCase4.CaseUid, caseListStatus[3].CaseUId);
            Assert.AreEqual(aCase4.MicrotingCheckUid, caseListStatus[3].CheckUIid);
            Assert.AreEqual(c4_ca.ToString(), caseListStatus[3].CreatedAt.ToString());
            Assert.AreEqual(aCase4.Custom, caseListStatus[3].Custom);
            Assert.AreEqual(c4_da.ToString(), caseListStatus[3].DoneAt.ToString());
            Assert.AreEqual(aCase4.Id, caseListStatus[3].Id);
            Assert.AreEqual(aCase4.MicrotingUid, caseListStatus[3].MicrotingUId);
            Assert.AreEqual(aCase4.Site.MicrotingUid, caseListStatus[3].SiteId);
            Assert.AreEqual(aCase4.Site.Name, caseListStatus[3].SiteName);
            Assert.AreEqual(aCase4.Status, caseListStatus[3].Status);
            Assert.AreEqual(aCase4.CheckListId, caseListStatus[3].TemplatId);
            Assert.AreEqual(aCase4.Unit.MicrotingUid, caseListStatus[3].UnitId);
//            Assert.AreEqual(c4_ua.ToString(), caseListStatus[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase4.Version, caseListStatus[3].Version);
            Assert.AreEqual(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName, caseListStatus[3].WorkerName);
            Assert.AreEqual(aCase4.WorkflowState, caseListStatus[3].WorkflowState);

            #endregion

            #endregion

            #region caseListUnitId Def Sort Asc

            #region caseListUnitId aCase1

            Assert.NotNull(caseListUnitId);
            Assert.AreEqual(4, caseListUnitId.Count);
            Assert.AreEqual(aCase1.Type, caseListUnitId[0].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListUnitId[0].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListUnitId[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListUnitId[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListUnitId[0].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListUnitId[0].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListUnitId[0].SiteName);
            Assert.AreEqual(aCase1.Status, caseListUnitId[0].Status);
            Assert.AreEqual(aCase1.CheckListId, caseListUnitId[0].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListUnitId[0].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListUnitId[0].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName, caseListUnitId[0].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListUnitId[0].WorkflowState);

            #endregion

            #region caseListUnitId aCase2

            Assert.AreEqual(aCase2.Type, caseListUnitId[1].CaseType);
            Assert.AreEqual(aCase2.CaseUid, caseListUnitId[1].CaseUId);
            Assert.AreEqual(aCase2.MicrotingCheckUid, caseListUnitId[1].CheckUIid);
            Assert.AreEqual(c2_ca.ToString(), caseListUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase2.Custom, caseListUnitId[1].Custom);
            Assert.AreEqual(c2_da.ToString(), caseListUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase2.Id, caseListUnitId[1].Id);
            Assert.AreEqual(aCase2.MicrotingUid, caseListUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase2.Site.MicrotingUid, caseListUnitId[1].SiteId);
            Assert.AreEqual(aCase2.Site.Name, caseListUnitId[1].SiteName);
            Assert.AreEqual(aCase2.Status, caseListUnitId[1].Status);
            Assert.AreEqual(aCase2.CheckListId, caseListUnitId[1].TemplatId);
            Assert.AreEqual(aCase2.Unit.MicrotingUid, caseListUnitId[1].UnitId);
//            Assert.AreEqual(c2_ua.ToString(), caseListUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase2.Version, caseListUnitId[1].Version);
            Assert.AreEqual(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName, caseListUnitId[1].WorkerName);
            Assert.AreEqual(aCase2.WorkflowState, caseListUnitId[1].WorkflowState);

            #endregion

            #region caseListUnitId aCase3

            Assert.AreEqual(aCase3.Type, caseListUnitId[2].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListUnitId[2].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListUnitId[2].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListUnitId[2].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListUnitId[2].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListUnitId[2].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListUnitId[2].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListUnitId[2].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListUnitId[2].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListUnitId[2].SiteName);
            Assert.AreEqual(aCase3.Status, caseListUnitId[2].Status);
            Assert.AreEqual(aCase3.CheckListId, caseListUnitId[2].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListUnitId[2].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListUnitId[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListUnitId[2].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName, caseListUnitId[2].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListUnitId[2].WorkflowState);

            #endregion

            #region caseListUnitId aCase4

            Assert.AreEqual(aCase4.Type, caseListUnitId[3].CaseType);
            Assert.AreEqual(aCase4.CaseUid, caseListUnitId[3].CaseUId);
            Assert.AreEqual(aCase4.MicrotingCheckUid, caseListUnitId[3].CheckUIid);
            Assert.AreEqual(c4_ca.ToString(), caseListUnitId[3].CreatedAt.ToString());
            Assert.AreEqual(aCase4.Custom, caseListUnitId[3].Custom);
            Assert.AreEqual(c4_da.ToString(), caseListUnitId[3].DoneAt.ToString());
            Assert.AreEqual(aCase4.Id, caseListUnitId[3].Id);
            Assert.AreEqual(aCase4.MicrotingUid, caseListUnitId[3].MicrotingUId);
            Assert.AreEqual(aCase4.Site.MicrotingUid, caseListUnitId[3].SiteId);
            Assert.AreEqual(aCase4.Site.Name, caseListUnitId[3].SiteName);
            Assert.AreEqual(aCase4.Status, caseListUnitId[3].Status);
            Assert.AreEqual(aCase4.CheckListId, caseListUnitId[3].TemplatId);
            Assert.AreEqual(aCase4.Unit.MicrotingUid, caseListUnitId[3].UnitId);
//            Assert.AreEqual(c4_ua.ToString(), caseListUnitId[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase4.Version, caseListUnitId[3].Version);
            Assert.AreEqual(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName, caseListUnitId[3].WorkerName);
            Assert.AreEqual(aCase4.WorkflowState, caseListUnitId[3].WorkflowState);

            #endregion

            #endregion

            #endregion


            #region Def Sort Asc w. DateTime

            #region caseListDtDoneAt Def Sort Asc w. DateTime

            #region caseListDtDoneAt aCase1

            Assert.NotNull(caseListDtDoneAt);
            Assert.AreEqual(2, caseListDtDoneAt.Count);
            Assert.AreEqual(aCase1.Type, caseListDtDoneAt[0].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListDtDoneAt[0].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListDtDoneAt[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDtDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListDtDoneAt[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDtDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListDtDoneAt[0].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListDtDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListDtDoneAt[0].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListDtDoneAt[0].SiteName);
            Assert.AreEqual(aCase1.Status, caseListDtDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListDtDoneAt[0].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListDtDoneAt[0].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListDtDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListDtDoneAt[0].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName, caseListDtDoneAt[0].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListDtDoneAt aCase3

            Assert.AreEqual(aCase3.Type, caseListDtDoneAt[1].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListDtDoneAt[1].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListDtDoneAt[1].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDtDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListDtDoneAt[1].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDtDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListDtDoneAt[1].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListDtDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListDtDoneAt[1].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListDtDoneAt[1].SiteName);
            Assert.AreEqual(aCase3.Status, caseListDtDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListDtDoneAt[1].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListDtDoneAt[1].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListDtDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListDtDoneAt[1].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName, caseListDtDoneAt[1].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListDtDoneAt[1].WorkflowState);

            #endregion

            #endregion

            #region caseListDtStatus Def Sort Asc w. DateTime

            #region caseListDtStatus aCase1

            Assert.NotNull(caseListDtStatus);
            Assert.AreEqual(2, caseListDtStatus.Count);
            Assert.AreEqual(aCase1.Type, caseListDtStatus[0].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListDtStatus[0].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListDtStatus[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDtStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListDtStatus[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDtStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListDtStatus[0].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListDtStatus[0].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListDtStatus[0].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListDtStatus[0].SiteName);
            Assert.AreEqual(aCase1.Status, caseListDtStatus[0].Status);
            Assert.AreEqual(aCase1.CheckListId, caseListDtStatus[0].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListDtStatus[0].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListDtStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListDtStatus[0].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName, caseListDtStatus[0].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListDtStatus[0].WorkflowState);

            #endregion

            #region caseListDtStatus aCase3

            Assert.AreEqual(aCase3.Type, caseListDtStatus[1].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListDtStatus[1].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListDtStatus[1].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDtStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListDtStatus[1].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDtStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListDtStatus[1].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListDtStatus[1].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListDtStatus[1].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListDtStatus[1].SiteName);
            Assert.AreEqual(aCase3.Status, caseListDtStatus[1].Status);
            Assert.AreEqual(aCase3.CheckListId, caseListDtStatus[1].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListDtStatus[1].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListDtStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListDtStatus[1].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName, caseListDtStatus[1].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListDtStatus[1].WorkflowState);

            #endregion

            #endregion

            #region caseListDtUnitId Def Sort Asc w. DateTime

            #region caseListDtUnitId aCase1

            Assert.NotNull(caseListDtUnitId);
            Assert.AreEqual(2, caseListDtUnitId.Count);
            Assert.AreEqual(aCase1.Type, caseListDtUnitId[0].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListDtUnitId[0].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListDtUnitId[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDtUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListDtUnitId[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDtUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListDtUnitId[0].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListDtUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListDtUnitId[0].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListDtUnitId[0].SiteName);
            Assert.AreEqual(aCase1.Status, caseListDtUnitId[0].Status);
            Assert.AreEqual(aCase1.CheckListId, caseListDtUnitId[0].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListDtUnitId[0].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListDtUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListDtUnitId[0].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName, caseListDtUnitId[0].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListDtUnitId[0].WorkflowState);

            #endregion

            #region caseListDtUnitId aCase3

            Assert.AreEqual(aCase3.Type, caseListDtUnitId[1].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListDtUnitId[1].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListDtUnitId[1].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDtUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListDtUnitId[1].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDtUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListDtUnitId[1].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListDtUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListDtUnitId[1].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListDtUnitId[1].SiteName);
            Assert.AreEqual(aCase3.Status, caseListDtUnitId[1].Status);
            Assert.AreEqual(aCase3.CheckListId, caseListDtUnitId[1].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListDtUnitId[1].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListDtUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListDtUnitId[1].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName, caseListDtUnitId[1].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListDtUnitId[1].WorkflowState);

            #endregion

            #endregion

            #endregion

            #endregion

            #region Case Sort

            #region aCase Sort Asc

            #region aCase1 sort asc

            #region caseListC1DoneAt aCase1

            Assert.NotNull(caseListC1SortDoneAt);
            Assert.AreEqual(0, caseListC1SortDoneAt.Count);
            // Assert.AreEqual(aCase1.type, caseListC1SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase1.case_uid, caseListC1SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase1.microting_check_uid, caseListC1SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c1_ca.ToString(), caseListC1SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1.custom, caseListC1SortDoneAt[0].Custom);
            // Assert.AreEqual(c1_da.ToString(), caseListC1SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1.Id, caseListC1SortDoneAt[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC1SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.Id, caseListC1SortDoneAt[0].UnitId);
            // Assert.AreEqual(c1_ua.ToString(), caseListC1SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1.version, caseListC1SortDoneAt[0].Version);
            // Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListC1SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase1.workflow_state, caseListC1SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListC1SortStatus aCase1

            Assert.NotNull(caseListC1SortStatus);
            Assert.AreEqual(0, caseListC1SortStatus.Count);
            // Assert.AreEqual(aCase1.type, caseListC1SortStatus[0].CaseType);
            // Assert.AreEqual(aCase1.case_uid, caseListC1SortStatus[0].CaseUId);
            // Assert.AreEqual(aCase1.microting_check_uid, caseListC1SortStatus[0].CheckUIid);
            // Assert.AreEqual(c1_ca.ToString(), caseListC1SortStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1.custom, caseListC1SortStatus[0].Custom);
            // Assert.AreEqual(c1_da.ToString(), caseListC1SortStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1.Id, caseListC1SortStatus[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortStatus[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortStatus[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC1SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.Id, caseListC1SortStatus[0].UnitId);
            // Assert.AreEqual(c1_ua.ToString(), caseListC1SortStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1.version, caseListC1SortStatus[0].Version);
            // Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListC1SortStatus[0].WorkerName);
            // Assert.AreEqual(aCase1.workflow_state, caseListC1SortStatus[0].WorkflowState);

            #endregion

            #region caseListC1SortUnitId

            Assert.NotNull(caseListC1SortUnitId);
            Assert.AreEqual(0, caseListC1SortUnitId.Count);
            // Assert.AreEqual(aCase1.type, caseListC1SortUnitId[0].CaseType);
            // Assert.AreEqual(aCase1.case_uid, caseListC1SortUnitId[0].CaseUId);
            // Assert.AreEqual(aCase1.microting_check_uid, caseListC1SortUnitId[0].CheckUIid);
            // Assert.AreEqual(c1_ca.ToString(), caseListC1SortUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1.custom, caseListC1SortUnitId[0].Custom);
            // Assert.AreEqual(c1_da.ToString(), caseListC1SortUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1.Id, caseListC1SortUnitId[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC1SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.Id, caseListC1SortUnitId[0].UnitId);
            // Assert.AreEqual(c1_ua.ToString(), caseListC1SortUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1.version, caseListC1SortUnitId[0].Version);
            // Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListC1SortUnitId[0].WorkerName);
            // Assert.AreEqual(aCase1.workflow_state, caseListC1SortUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase2 sort asc

            #region caseListC2DoneAt aCase2

            Assert.NotNull(caseListC2SortDoneAt);
            Assert.AreEqual(0, caseListC2SortDoneAt.Count);
            // Assert.AreEqual(aCase2.type, caseListC2SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase2.case_uid, caseListC2SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase2.microting_check_uid, caseListC2SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c2_ca.ToString(), caseListC2SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2.custom, caseListC2SortDoneAt[0].Custom);
            // Assert.AreEqual(c2_da.ToString(), caseListC2SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2.Id, caseListC2SortDoneAt[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC2SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.Id, caseListC2SortDoneAt[0].UnitId);
            // Assert.AreEqual(c2_ua.ToString(), caseListC2SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2.version, caseListC2SortDoneAt[0].Version);
            // Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListC2SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase2.workflow_state, caseListC2SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListC2SortStatus aCase2

            Assert.NotNull(caseListC2SortStatus);
            Assert.AreEqual(0, caseListC2SortStatus.Count);
            // Assert.AreEqual(aCase2.type, caseListC2SortStatus[0].CaseType);
            // Assert.AreEqual(aCase2.case_uid, caseListC2SortStatus[0].CaseUId);
            // Assert.AreEqual(aCase2.microting_check_uid, caseListC2SortStatus[0].CheckUIid);
            // Assert.AreEqual(c2_ca.ToString(), caseListC2SortStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2.custom, caseListC2SortStatus[0].Custom);
            // Assert.AreEqual(c2_da.ToString(), caseListC2SortStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2.Id, caseListC2SortStatus[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortStatus[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortStatus[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC2SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.Id, caseListC2SortStatus[0].UnitId);
            // Assert.AreEqual(c2_ua.ToString(), caseListC2SortStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2.version, caseListC2SortStatus[0].Version);
            // Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListC2SortStatus[0].WorkerName);
            // Assert.AreEqual(aCase2.workflow_state, caseListC2SortStatus[0].WorkflowState);

            #endregion

            #region caseListC2SortUnitId aCase2

            Assert.NotNull(caseListC2SortUnitId);
            Assert.AreEqual(0, caseListC2SortUnitId.Count);
            // Assert.AreEqual(aCase2.type, caseListC2SortUnitId[0].CaseType);
            // Assert.AreEqual(aCase2.case_uid, caseListC2SortUnitId[0].CaseUId);
            // Assert.AreEqual(aCase2.microting_check_uid, caseListC2SortUnitId[0].CheckUIid);
            // Assert.AreEqual(c2_ca.ToString(), caseListC2SortUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2.custom, caseListC2SortUnitId[0].Custom);
            // Assert.AreEqual(c2_da.ToString(), caseListC2SortUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2.Id, caseListC2SortUnitId[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC2SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.Id, caseListC2SortUnitId[0].UnitId);
            // Assert.AreEqual(c2_ua.ToString(), caseListC2SortUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2.version, caseListC2SortUnitId[0].Version);
            // Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListC2SortUnitId[0].WorkerName);
            // Assert.AreEqual(aCase2.workflow_state, caseListC2SortUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase3 sort asc

            #region caseListC3DoneAt aCase3

            Assert.NotNull(caseListC3SortDoneAt);
            Assert.AreEqual(0, caseListC3SortDoneAt.Count);
            // Assert.AreEqual(aCase3.type, caseListC3SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase3.case_uid, caseListC3SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase3.microting_check_uid, caseListC3SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c3_ca.ToString(), caseListC3SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3.custom, caseListC3SortDoneAt[0].Custom);
            // Assert.AreEqual(c3_da.ToString(), caseListC3SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3.Id, caseListC3SortDoneAt[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC3SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.Id, caseListC3SortDoneAt[0].UnitId);
            // Assert.AreEqual(c3_ua.ToString(), caseListC3SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3.version, caseListC3SortDoneAt[0].Version);
            // Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListC3SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase3.workflow_state, caseListC3SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListC3status aCase3

            Assert.NotNull(caseListC3SortStatus);
            Assert.AreEqual(0, caseListC3SortStatus.Count);
            // Assert.AreEqual(aCase3.type, caseListC3SortStatus[0].CaseType);
            // Assert.AreEqual(aCase3.case_uid, caseListC3SortStatus[0].CaseUId);
            // Assert.AreEqual(aCase3.microting_check_uid, caseListC3SortStatus[0].CheckUIid);
            // Assert.AreEqual(c3_ca.ToString(), caseListC3SortStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3.custom, caseListC3SortStatus[0].Custom);
            // Assert.AreEqual(c3_da.ToString(), caseListC3SortStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3.Id, caseListC3SortStatus[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortStatus[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortStatus[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC3SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.Id, caseListC3SortStatus[0].UnitId);
            // Assert.AreEqual(c3_ua.ToString(), caseListC3SortStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3.version, caseListC3SortStatus[0].Version);
            // Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListC3SortStatus[0].WorkerName);
            // Assert.AreEqual(aCase3.workflow_state, caseListC3SortStatus[0].WorkflowState);

            #endregion

            #region caseListC3UnitId aCase3

            Assert.NotNull(caseListC3SortUnitId);
            Assert.AreEqual(0, caseListC3SortUnitId.Count);
            // Assert.AreEqual(aCase3.type, caseListC3SortUnitId[0].CaseType);
            // Assert.AreEqual(aCase3.case_uid, caseListC3SortUnitId[0].CaseUId);
            // Assert.AreEqual(aCase3.microting_check_uid, caseListC3SortUnitId[0].CheckUIid);
            // Assert.AreEqual(c3_ca.ToString(), caseListC3SortUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3.custom, caseListC3SortUnitId[0].Custom);
            // Assert.AreEqual(c3_da.ToString(), caseListC3SortUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3.Id, caseListC3SortUnitId[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC3SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.Id, caseListC3SortUnitId[0].UnitId);
            // Assert.AreEqual(c3_ua.ToString(), caseListC3SortUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3.version, caseListC3SortUnitId[0].Version);
            // Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListC3SortUnitId[0].WorkerName);
            // Assert.AreEqual(aCase3.workflow_state, caseListC3SortUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase4 sort asc

            #region caseListC4SortDoneAt aCase4

            Assert.NotNull(caseListC4SortDoneAt);
            Assert.AreEqual(0, caseListC4SortDoneAt.Count);
            // Assert.AreEqual(aCase4.type, caseListC4SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4.case_uid, caseListC4SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4.microting_check_uid, caseListC4SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4_ca.ToString(), caseListC4SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4.custom, caseListC4SortDoneAt[0].Custom);
            // Assert.AreEqual(c4_da.ToString(), caseListC4SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4.Id, caseListC4SortDoneAt[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC4SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.Id, caseListC4SortDoneAt[0].UnitId);
            // Assert.AreEqual(c4_ua.ToString(), caseListC4SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4.version, caseListC4SortDoneAt[0].Version);
            // Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListC4SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4.workflow_state, caseListC4SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListC4SortStatus aCase4

            Assert.NotNull(caseListC1SortDoneAt);
            Assert.AreEqual(0, caseListC1SortDoneAt.Count);
            // Assert.AreEqual(aCase4.type, caseListC1SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4.case_uid, caseListC1SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4.microting_check_uid, caseListC1SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4_ca.ToString(), caseListC1SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4.custom, caseListC1SortDoneAt[0].Custom);
            // Assert.AreEqual(c4_da.ToString(), caseListC1SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4.Id, caseListC1SortDoneAt[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC1SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC1SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC1SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC1SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC1SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.Id, caseListC1SortDoneAt[0].UnitId);
            // Assert.AreEqual(c4_ua.ToString(), caseListC1SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4.version, caseListC1SortDoneAt[0].Version);
            // Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListC1SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4.workflow_state, caseListC1SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListC4SortUnitId aCase4

            Assert.NotNull(caseListC4SortDoneAt);
            Assert.AreEqual(0, caseListC4SortDoneAt.Count);
            // Assert.AreEqual(aCase4.type, caseListC4SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4.case_uid, caseListC4SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4.microting_check_uid, caseListC4SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4_ca.ToString(), caseListC4SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4.custom, caseListC4SortDoneAt[0].Custom);
            // Assert.AreEqual(c4_da.ToString(), caseListC4SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4.Id, caseListC4SortDoneAt[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC4SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.Id, caseListC4SortDoneAt[0].UnitId);
            // Assert.AreEqual(c4_ua.ToString(), caseListC4SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4.version, caseListC4SortDoneAt[0].Version);
            // Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListC4SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4.workflow_state, caseListC4SortDoneAt[0].WorkflowState);

            #endregion

            #endregion

            #endregion


            #region aCase Sort Asc w. DateTime

            #region aCase1 sort asc w. DateTime

            #region caseListC1SortDtDoneAt aCase1

            Assert.NotNull(caseListC1SortDtDoneAt);
            Assert.AreEqual(0, caseListC1SortDtDoneAt.Count);
            // Assert.AreEqual(aCase1.type, caseListC1SortDtDoneAt[0].CaseType);
            // Assert.AreEqual(aCase1.case_uid, caseListC1SortDtDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase1.microting_check_uid, caseListC1SortDtDoneAt[0].CheckUIid);
            // Assert.AreEqual(c1_ca.ToString(), caseListC1SortDtDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1.custom, caseListC1SortDtDoneAt[0].Custom);
            // Assert.AreEqual(c1_da.ToString(), caseListC1SortDtDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1.Id, caseListC1SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC1SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.Id, caseListC1SortDtDoneAt[0].UnitId);
            // Assert.AreEqual(c1_ua.ToString(), caseListC1SortDtDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1.version, caseListC1SortDtDoneAt[0].Version);
            // Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListC1SortDtDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase1.workflow_state, caseListC1SortDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListC1SortDtStatus aCase1

            Assert.NotNull(caseListC1SortDtStatus);
            Assert.AreEqual(0, caseListC1SortDtStatus.Count);
            // Assert.AreEqual(aCase1.type, caseListC1SortDtStatus[0].CaseType);
            // Assert.AreEqual(aCase1.case_uid, caseListC1SortDtStatus[0].CaseUId);
            // Assert.AreEqual(aCase1.microting_check_uid, caseListC1SortDtStatus[0].CheckUIid);
            // Assert.AreEqual(c1_ca.ToString(), caseListC1SortDtStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1.custom, caseListC1SortDtStatus[0].Custom);
            // Assert.AreEqual(c1_da.ToString(), caseListC1SortDtStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1.Id, caseListC1SortDtStatus[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC1SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.Id, caseListC1SortDtStatus[0].UnitId);
            // Assert.AreEqual(c1_ua.ToString(), caseListC1SortDtStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1.version, caseListC1SortDtStatus[0].Version);
            // Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListC1SortDtStatus[0].WorkerName);
            // Assert.AreEqual(aCase1.workflow_state, caseListC1SortDtStatus[0].WorkflowState);

            #endregion

            #region caseListC1SortDtUnitId aCase1

            Assert.NotNull(caseListC1SortDtUnitId);
            Assert.AreEqual(0, caseListC1SortDtUnitId.Count);
            // Assert.AreEqual(aCase1.type, caseListC1SortDtUnitId[0].CaseType);
            // Assert.AreEqual(aCase1.case_uid, caseListC1SortDtUnitId[0].CaseUId);
            // Assert.AreEqual(aCase1.microting_check_uid, caseListC1SortDtUnitId[0].CheckUIid);
            // Assert.AreEqual(c1_ca.ToString(), caseListC1SortDtUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1.custom, caseListC1SortDtUnitId[0].Custom);
            // Assert.AreEqual(c1_da.ToString(), caseListC1SortDtUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1.Id, caseListC1SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC1SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.Id, caseListC1SortDtUnitId[0].UnitId);
            // Assert.AreEqual(c1_ua.ToString(), caseListC1SortDtUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1.version, caseListC1SortDtUnitId[0].Version);
            // Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListC1SortDtUnitId[0].WorkerName);
            // Assert.AreEqual(aCase1.workflow_state, caseListC1SortDtUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase2 sort asc w. DateTime

            #region caseListC2SortDtDoneAt aCase2

            Assert.NotNull(caseListC2SortDtDoneAt);
            Assert.AreEqual(0, caseListC2SortDtDoneAt.Count);
            // Assert.AreEqual(aCase2.type, caseListC2SortDtDoneAt[0].CaseType);
            // Assert.AreEqual(aCase2.case_uid, caseListC2SortDtDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase2.microting_check_uid, caseListC2SortDtDoneAt[0].CheckUIid);
            // Assert.AreEqual(c2_ca.ToString(), caseListC2SortDtDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2.custom, caseListC2SortDtDoneAt[0].Custom);
            // Assert.AreEqual(c2_da.ToString(), caseListC2SortDtDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2.Id, caseListC2SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC2SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.Id, caseListC2SortDtDoneAt[0].UnitId);
            // Assert.AreEqual(c2_ua.ToString(), caseListC2SortDtDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2.version, caseListC2SortDtDoneAt[0].Version);
            // Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListC2SortDtDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase2.workflow_state, caseListC2SortDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListC2SortDtStatus aCase2

            Assert.NotNull(caseListC2SortDtStatus);
            Assert.AreEqual(0, caseListC2SortDtStatus.Count);
            // Assert.AreEqual(aCase2.type, caseListC2SortDtStatus[0].CaseType);
            // Assert.AreEqual(aCase2.case_uid, caseListC2SortDtStatus[0].CaseUId);
            // Assert.AreEqual(aCase2.microting_check_uid, caseListC2SortDtStatus[0].CheckUIid);
            // Assert.AreEqual(c2_ca.ToString(), caseListC2SortDtStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2.custom, caseListC2SortDtStatus[0].Custom);
            // Assert.AreEqual(c2_da.ToString(), caseListC2SortDtStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2.Id, caseListC2SortDtStatus[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC2SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.Id, caseListC2SortDtStatus[0].UnitId);
            // Assert.AreEqual(c2_ua.ToString(), caseListC2SortDtStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2.version, caseListC2SortDtStatus[0].Version);
            // Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListC2SortDtStatus[0].WorkerName);
            // Assert.AreEqual(aCase2.workflow_state, caseListC2SortDtStatus[0].WorkflowState);

            #endregion

            #region caseListC2SortDtUnitId aCase2

            Assert.NotNull(caseListC2SortDtUnitId);
            Assert.AreEqual(0, caseListC2SortDtUnitId.Count);
            // Assert.AreEqual(aCase2.type, caseListC2SortDtUnitId[0].CaseType);
            // Assert.AreEqual(aCase2.case_uid, caseListC2SortDtUnitId[0].CaseUId);
            // Assert.AreEqual(aCase2.microting_check_uid, caseListC2SortDtUnitId[0].CheckUIid);
            // Assert.AreEqual(c2_ca.ToString(), caseListC2SortDtUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2.custom, caseListC2SortDtUnitId[0].Custom);
            // Assert.AreEqual(c2_da.ToString(), caseListC2SortDtUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2.Id, caseListC2SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC2SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.Id, caseListC2SortDtUnitId[0].UnitId);
            // Assert.AreEqual(c2_ua.ToString(), caseListC2SortDtUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2.version, caseListC2SortDtUnitId[0].Version);
            // Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListC2SortDtUnitId[0].WorkerName);
            // Assert.AreEqual(aCase2.workflow_state, caseListC2SortDtUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase3 sort asc w. DateTime

            #region caseListC3SortDtDoneAt aCase3

            Assert.NotNull(caseListC3SortDtDoneAt);
            Assert.AreEqual(0, caseListC3SortDtDoneAt.Count);
            // Assert.AreEqual(aCase3.type, caseListC3SortDtDoneAt[0].CaseType);
            // Assert.AreEqual(aCase3.case_uid, caseListC3SortDtDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase3.microting_check_uid, caseListC3SortDtDoneAt[0].CheckUIid);
            // Assert.AreEqual(c3_ca.ToString(), caseListC3SortDtDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3.custom, caseListC3SortDtDoneAt[0].Custom);
            // Assert.AreEqual(c3_da.ToString(), caseListC3SortDtDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3.Id, caseListC3SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC3SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.Id, caseListC3SortDtDoneAt[0].UnitId);
            // Assert.AreEqual(c3_ua.ToString(), caseListC3SortDtDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3.version, caseListC3SortDtDoneAt[0].Version);
            // Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListC3SortDtDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase3.workflow_state, caseListC3SortDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListC3SortDtStatus aCase3

            Assert.NotNull(caseListC3SortDtStatus);
            Assert.AreEqual(0, caseListC3SortDtStatus.Count);
            // Assert.AreEqual(aCase3.type, caseListC3SortDtStatus[0].CaseType);
            // Assert.AreEqual(aCase3.case_uid, caseListC3SortDtStatus[0].CaseUId);
            // Assert.AreEqual(aCase3.microting_check_uid, caseListC3SortDtStatus[0].CheckUIid);
            // Assert.AreEqual(c3_ca.ToString(), caseListC3SortDtStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3.custom, caseListC3SortDtStatus[0].Custom);
            // Assert.AreEqual(c3_da.ToString(), caseListC3SortDtStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3.Id, caseListC3SortDtStatus[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC3SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.Id, caseListC3SortDtStatus[0].UnitId);
            // Assert.AreEqual(c3_ua.ToString(), caseListC3SortDtStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3.version, caseListC3SortDtStatus[0].Version);
            // Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListC3SortDtStatus[0].WorkerName);
            // Assert.AreEqual(aCase3.workflow_state, caseListC3SortDtStatus[0].WorkflowState);

            #endregion

            #region caseListC3SortDtUnitId aCase3

            Assert.NotNull(caseListC3SortDtUnitId);
            Assert.AreEqual(0, caseListC3SortDtUnitId.Count);
            // Assert.AreEqual(aCase3.type, caseListC3SortDtUnitId[0].CaseType);
            // Assert.AreEqual(aCase3.case_uid, caseListC3SortDtUnitId[0].CaseUId);
            // Assert.AreEqual(aCase3.microting_check_uid, caseListC3SortDtUnitId[0].CheckUIid);
            // Assert.AreEqual(c3_ca.ToString(), caseListC3SortDtUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3.custom, caseListC3SortDtUnitId[0].Custom);
            // Assert.AreEqual(c3_da.ToString(), caseListC3SortDtUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3.Id, caseListC3SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC3SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.Id, caseListC3SortDtUnitId[0].UnitId);
            // Assert.AreEqual(c3_ua.ToString(), caseListC3SortDtUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3.version, caseListC3SortDtUnitId[0].Version);
            // Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListC3SortDtUnitId[0].WorkerName);
            // Assert.AreEqual(aCase3.workflow_state, caseListC3SortDtUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase4 sort asc w. DateTime

            #region caseListC4SortDtDoneAt aCase4

            Assert.NotNull(caseListC4SortDtDoneAt);
            Assert.AreEqual(0, caseListC4SortDtDoneAt.Count);
            // Assert.AreEqual(aCase4.type, caseListC4SortDtDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4.case_uid, caseListC4SortDtDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4.microting_check_uid, caseListC4SortDtDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4_ca.ToString(), caseListC4SortDtDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4.custom, caseListC4SortDtDoneAt[0].Custom);
            // Assert.AreEqual(c4_da.ToString(), caseListC4SortDtDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4.Id, caseListC4SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC4SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.Id, caseListC4SortDtDoneAt[0].UnitId);
            // Assert.AreEqual(c4_ua.ToString(), caseListC4SortDtDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4.version, caseListC4SortDtDoneAt[0].Version);
            // Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListC4SortDtDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4.workflow_state, caseListC4SortDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListC4SortDtStatus aCase4

            Assert.NotNull(caseListC4SortDtStatus);
            Assert.AreEqual(0, caseListC4SortDtStatus.Count);
            // Assert.AreEqual(aCase4.type, caseListC4SortDtStatus[0].CaseType);
            // Assert.AreEqual(aCase4.case_uid, caseListC4SortDtStatus[0].CaseUId);
            // Assert.AreEqual(aCase4.microting_check_uid, caseListC4SortDtStatus[0].CheckUIid);
            // Assert.AreEqual(c4_ca.ToString(), caseListC4SortDtStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4.custom, caseListC4SortDtStatus[0].Custom);
            // Assert.AreEqual(c4_da.ToString(), caseListC4SortDtStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4.Id, caseListC4SortDtStatus[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC4SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.Id, caseListC4SortDtStatus[0].UnitId);
            // Assert.AreEqual(c4_ua.ToString(), caseListC4SortDtStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4.version, caseListC4SortDtStatus[0].Version);
            // Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListC4SortDtStatus[0].WorkerName);
            // Assert.AreEqual(aCase4.workflow_state, caseListC4SortDtStatus[0].WorkflowState);

            #endregion

            #region caseListC4SortDtUnitId aCase4

            Assert.NotNull(caseListC4SortDtUnitId);
            Assert.AreEqual(0, caseListC4SortDtUnitId.Count);
            // Assert.AreEqual(aCase4.type, caseListC4SortDtUnitId[0].CaseType);
            // Assert.AreEqual(aCase4.case_uid, caseListC4SortDtUnitId[0].CaseUId);
            // Assert.AreEqual(aCase4.microting_check_uid, caseListC4SortDtUnitId[0].CheckUIid);
            // Assert.AreEqual(c4_ca.ToString(), caseListC4SortDtUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4.custom, caseListC4SortDtUnitId[0].Custom);
            // Assert.AreEqual(c4_da.ToString(), caseListC4SortDtUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4.Id, caseListC4SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListC4SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.Id, caseListC4SortDtUnitId[0].UnitId);
            // Assert.AreEqual(c4_ua.ToString(), caseListC4SortDtUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4.version, caseListC4SortDtUnitId[0].Version);
            // Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListC4SortDtUnitId[0].WorkerName);
            // Assert.AreEqual(aCase4.workflow_state, caseListC4SortDtUnitId[0].WorkflowState);

            #endregion

            #endregion

            #endregion

            #endregion

            #endregion

            #region sort by WorkflowState removed

            #region Def Sort

            #region Def Sort Asc

            #region caseListRemovedDoneAt Def Sort Asc

            #region caseListRemovedDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedDoneAt);
            Assert.AreEqual(4, caseListRemovedDoneAt.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedDoneAt[0].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedDoneAt[0].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedDoneAt[0].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedDoneAt[0].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedDoneAt[0].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedDoneAt[0].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedDoneAt[0].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDoneAt[0].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedDoneAt[0].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedDoneAt[0].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedDoneAt[0].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedDoneAt aCase2Removed

            Assert.AreEqual(aCase2Removed.Type, caseListRemovedDoneAt[1].CaseType);
            Assert.AreEqual(aCase2Removed.CaseUid, caseListRemovedDoneAt[1].CaseUId);
            Assert.AreEqual(aCase2Removed.MicrotingCheckUid, caseListRemovedDoneAt[1].CheckUIid);
            Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Custom, caseListRemovedDoneAt[1].Custom);
            Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase2Removed.Id, caseListRemovedDoneAt[1].Id);
            Assert.AreEqual(aCase2Removed.MicrotingUid, caseListRemovedDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase2Removed.Site.MicrotingUid, caseListRemovedDoneAt[1].SiteId);
            Assert.AreEqual(aCase2Removed.Site.Name, caseListRemovedDoneAt[1].SiteName);
            Assert.AreEqual(aCase2Removed.Status, caseListRemovedDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDoneAt[1].TemplatId);
            Assert.AreEqual(aCase2Removed.Unit.MicrotingUid, caseListRemovedDoneAt[1].UnitId);
//            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Version, caseListRemovedDoneAt[1].Version);
            Assert.AreEqual(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName,
                caseListRemovedDoneAt[1].WorkerName);
            Assert.AreEqual(aCase2Removed.WorkflowState, caseListRemovedDoneAt[1].WorkflowState);

            #endregion

            #region caseListRemovedDoneAt aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedDoneAt[2].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedDoneAt[2].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedDoneAt[2].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedDoneAt[2].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedDoneAt[2].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedDoneAt[2].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedDoneAt[2].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedDoneAt[2].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedDoneAt[2].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedDoneAt[2].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedDoneAt[2].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDoneAt[2].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedDoneAt[2].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDoneAt[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedDoneAt[2].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedDoneAt[2].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedDoneAt[2].WorkflowState);

            #endregion

            #region caseListRemovedDoneAt aCase4Removed

            Assert.AreEqual(aCase4Removed.Type, caseListRemovedDoneAt[3].CaseType);
            Assert.AreEqual(aCase4Removed.CaseUid, caseListRemovedDoneAt[3].CaseUId);
            Assert.AreEqual(aCase4Removed.MicrotingCheckUid, caseListRemovedDoneAt[3].CheckUIid);
            Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedDoneAt[3].CreatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Custom, caseListRemovedDoneAt[3].Custom);
            Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedDoneAt[3].DoneAt.ToString());
            Assert.AreEqual(aCase4Removed.Id, caseListRemovedDoneAt[3].Id);
            Assert.AreEqual(aCase4Removed.MicrotingUid, caseListRemovedDoneAt[3].MicrotingUId);
            Assert.AreEqual(aCase4Removed.Site.MicrotingUid, caseListRemovedDoneAt[3].SiteId);
            Assert.AreEqual(aCase4Removed.Site.Name, caseListRemovedDoneAt[3].SiteName);
            Assert.AreEqual(aCase4Removed.Status, caseListRemovedDoneAt[3].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDoneAt[3].TemplatId);
            Assert.AreEqual(aCase4Removed.Unit.MicrotingUid, caseListRemovedDoneAt[3].UnitId);
//            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedDoneAt[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Version, caseListRemovedDoneAt[3].Version);
            Assert.AreEqual(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName,
                caseListRemovedDoneAt[3].WorkerName);
            Assert.AreEqual(aCase4Removed.WorkflowState, caseListRemovedDoneAt[3].WorkflowState);

            #endregion

            #endregion

            #region caseListRemovedStatus Def Sort Asc

            #region caseListRemovedStatus aCase1Removed

            Assert.NotNull(caseListRemovedStatus);
            Assert.AreEqual(4, caseListRemovedStatus.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedStatus[0].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedStatus[0].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedStatus[0].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedStatus[0].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedStatus[0].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedStatus[0].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedStatus[0].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedStatus[0].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedStatus[0].Status);
            Assert.AreEqual(aCase1Removed.CheckListId, caseListRemovedStatus[0].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedStatus[0].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedStatus[0].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedStatus[0].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedStatus aCase2Removed

            Assert.AreEqual(aCase2Removed.Type, caseListRemovedStatus[1].CaseType);
            Assert.AreEqual(aCase2Removed.CaseUid, caseListRemovedStatus[1].CaseUId);
            Assert.AreEqual(aCase2Removed.MicrotingCheckUid, caseListRemovedStatus[1].CheckUIid);
            Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Custom, caseListRemovedStatus[1].Custom);
            Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase2Removed.Id, caseListRemovedStatus[1].Id);
            Assert.AreEqual(aCase2Removed.MicrotingUid, caseListRemovedStatus[1].MicrotingUId);
            Assert.AreEqual(aCase2Removed.Site.MicrotingUid, caseListRemovedStatus[1].SiteId);
            Assert.AreEqual(aCase2Removed.Site.Name, caseListRemovedStatus[1].SiteName);
            Assert.AreEqual(aCase2Removed.Status, caseListRemovedStatus[1].Status);
            Assert.AreEqual(aCase2Removed.CheckListId, caseListRemovedStatus[1].TemplatId);
            Assert.AreEqual(aCase2Removed.Unit.MicrotingUid, caseListRemovedStatus[1].UnitId);
//            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Version, caseListRemovedStatus[1].Version);
            Assert.AreEqual(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName,
                caseListRemovedStatus[1].WorkerName);
            Assert.AreEqual(aCase2Removed.WorkflowState, caseListRemovedStatus[1].WorkflowState);

            #endregion

            #region caseListRemovedStatus aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedStatus[2].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedStatus[2].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedStatus[2].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedStatus[2].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedStatus[2].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedStatus[2].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedStatus[2].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedStatus[2].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedStatus[2].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedStatus[2].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedStatus[2].Status);
            Assert.AreEqual(aCase3Removed.CheckListId, caseListRemovedStatus[2].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedStatus[2].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedStatus[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedStatus[2].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedStatus[2].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedStatus[2].WorkflowState);

            #endregion

            #region caseListRemovedStatus aCase4Removed

            Assert.AreEqual(aCase4Removed.Type, caseListRemovedStatus[3].CaseType);
            Assert.AreEqual(aCase4Removed.CaseUid, caseListRemovedStatus[3].CaseUId);
            Assert.AreEqual(aCase4Removed.MicrotingCheckUid, caseListRemovedStatus[3].CheckUIid);
            Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedStatus[3].CreatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Custom, caseListRemovedStatus[3].Custom);
            Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedStatus[3].DoneAt.ToString());
            Assert.AreEqual(aCase4Removed.Id, caseListRemovedStatus[3].Id);
            Assert.AreEqual(aCase4Removed.MicrotingUid, caseListRemovedStatus[3].MicrotingUId);
            Assert.AreEqual(aCase4Removed.Site.MicrotingUid, caseListRemovedStatus[3].SiteId);
            Assert.AreEqual(aCase4Removed.Site.Name, caseListRemovedStatus[3].SiteName);
            Assert.AreEqual(aCase4Removed.Status, caseListRemovedStatus[3].Status);
            Assert.AreEqual(aCase4Removed.CheckListId, caseListRemovedStatus[3].TemplatId);
            Assert.AreEqual(aCase4Removed.Unit.MicrotingUid, caseListRemovedStatus[3].UnitId);
//            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedStatus[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Version, caseListRemovedStatus[3].Version);
            Assert.AreEqual(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName,
                caseListRemovedStatus[3].WorkerName);
            Assert.AreEqual(aCase4Removed.WorkflowState, caseListRemovedStatus[3].WorkflowState);

            #endregion

            #endregion

            #region caseListRemovedUnitId Def Sort Asc

            #region caseListRemovedUnitId aCase1Removed

            Assert.NotNull(caseListRemovedUnitId);
            Assert.AreEqual(4, caseListRemovedUnitId.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedUnitId[0].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedUnitId[0].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedUnitId[0].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedUnitId[0].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedUnitId[0].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedUnitId[0].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedUnitId[0].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedUnitId[0].Status);
            Assert.AreEqual(aCase1Removed.CheckListId, caseListRemovedUnitId[0].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedUnitId[0].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedUnitId[0].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedUnitId[0].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedUnitId[0].WorkflowState);

            #endregion

            #region caseListRemovedUnitId aCase2Removed

            Assert.AreEqual(aCase2Removed.Type, caseListRemovedUnitId[1].CaseType);
            Assert.AreEqual(aCase2Removed.CaseUid, caseListRemovedUnitId[1].CaseUId);
            Assert.AreEqual(aCase2Removed.MicrotingCheckUid, caseListRemovedUnitId[1].CheckUIid);
            Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Custom, caseListRemovedUnitId[1].Custom);
            Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase2Removed.Id, caseListRemovedUnitId[1].Id);
            Assert.AreEqual(aCase2Removed.MicrotingUid, caseListRemovedUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase2Removed.Site.MicrotingUid, caseListRemovedUnitId[1].SiteId);
            Assert.AreEqual(aCase2Removed.Site.Name, caseListRemovedUnitId[1].SiteName);
            Assert.AreEqual(aCase2Removed.Status, caseListRemovedUnitId[1].Status);
            Assert.AreEqual(aCase2Removed.CheckListId, caseListRemovedUnitId[1].TemplatId);
            Assert.AreEqual(aCase2Removed.Unit.MicrotingUid, caseListRemovedUnitId[1].UnitId);
//            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Version, caseListRemovedUnitId[1].Version);
            Assert.AreEqual(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName,
                caseListRemovedUnitId[1].WorkerName);
            Assert.AreEqual(aCase2Removed.WorkflowState, caseListRemovedUnitId[1].WorkflowState);

            #endregion

            #region caseListRemovedUnitId aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedUnitId[2].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedUnitId[2].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedUnitId[2].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedUnitId[2].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedUnitId[2].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedUnitId[2].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedUnitId[2].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedUnitId[2].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedUnitId[2].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedUnitId[2].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedUnitId[2].Status);
            Assert.AreEqual(aCase3Removed.CheckListId, caseListRemovedUnitId[2].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedUnitId[2].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedUnitId[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedUnitId[2].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedUnitId[2].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedUnitId[2].WorkflowState);

            #endregion

            #region caseListRemovedUnitId aCase4Removed

            Assert.AreEqual(aCase4Removed.Type, caseListRemovedUnitId[3].CaseType);
            Assert.AreEqual(aCase4Removed.CaseUid, caseListRemovedUnitId[3].CaseUId);
            Assert.AreEqual(aCase4Removed.MicrotingCheckUid, caseListRemovedUnitId[3].CheckUIid);
            Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedUnitId[3].CreatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Custom, caseListRemovedUnitId[3].Custom);
            Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedUnitId[3].DoneAt.ToString());
            Assert.AreEqual(aCase4Removed.Id, caseListRemovedUnitId[3].Id);
            Assert.AreEqual(aCase4Removed.MicrotingUid, caseListRemovedUnitId[3].MicrotingUId);
            Assert.AreEqual(aCase4Removed.Site.MicrotingUid, caseListRemovedUnitId[3].SiteId);
            Assert.AreEqual(aCase4Removed.Site.Name, caseListRemovedUnitId[3].SiteName);
            Assert.AreEqual(aCase4Removed.Status, caseListRemovedUnitId[3].Status);
            Assert.AreEqual(aCase4Removed.CheckListId, caseListRemovedUnitId[3].TemplatId);
            Assert.AreEqual(aCase4Removed.Unit.MicrotingUid, caseListRemovedUnitId[3].UnitId);
//            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedUnitId[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Version, caseListRemovedUnitId[3].Version);
            Assert.AreEqual(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName,
                caseListRemovedUnitId[3].WorkerName);
            Assert.AreEqual(aCase4Removed.WorkflowState, caseListRemovedUnitId[3].WorkflowState);

            #endregion

            #endregion

            #endregion


            #region Def Sort Asc w. DateTime

            #region caseListRemovedDtDoneAt Def Sort Asc w. DateTime

            #region caseListRemovedDtDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedDtDoneAt);
            Assert.AreEqual(2, caseListRemovedDtDoneAt.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedDtDoneAt[0].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedDtDoneAt[0].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedDtDoneAt[0].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedDtDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedDtDoneAt[0].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedDtDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedDtDoneAt[0].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedDtDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedDtDoneAt[0].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedDtDoneAt[0].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedDtDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDtDoneAt[0].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedDtDoneAt[0].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedDtDoneAt[0].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedDtDoneAt[0].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedDtDoneAt aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedDtDoneAt[1].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedDtDoneAt[1].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedDtDoneAt[1].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedDtDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedDtDoneAt[1].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedDtDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedDtDoneAt[1].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedDtDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedDtDoneAt[1].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedDtDoneAt[1].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedDtDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDtDoneAt[1].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedDtDoneAt[1].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedDtDoneAt[1].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedDtDoneAt[1].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedDtDoneAt[1].WorkflowState);

            #endregion

            #endregion

            #region caseListRemovedDtStatus Def Sort Asc w. DateTime

            #region caseListRemovedDtStatus aCase1Removed

            Assert.NotNull(caseListRemovedDtStatus);
            Assert.AreEqual(2, caseListRemovedDtStatus.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedDtStatus[0].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedDtStatus[0].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedDtStatus[0].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedDtStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedDtStatus[0].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedDtStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedDtStatus[0].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedDtStatus[0].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedDtStatus[0].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedDtStatus[0].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedDtStatus[0].Status);
            Assert.AreEqual(aCase1Removed.CheckListId, caseListRemovedDtStatus[0].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedDtStatus[0].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedDtStatus[0].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedDtStatus[0].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedDtStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedDtStatus aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedDtStatus[1].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedDtStatus[1].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedDtStatus[1].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedDtStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedDtStatus[1].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedDtStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedDtStatus[1].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedDtStatus[1].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedDtStatus[1].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedDtStatus[1].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedDtStatus[1].Status);
            Assert.AreEqual(aCase3Removed.CheckListId, caseListRemovedDtStatus[1].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedDtStatus[1].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedDtStatus[1].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedDtStatus[1].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedDtStatus[1].WorkflowState);

            #endregion

            #endregion

            #region caseListRemovedDtUnitId Def Sort Asc w. DateTime

            #region caseListRemovedDtUnitId aCase1Removed

            Assert.NotNull(caseListRemovedDtUnitId);
            Assert.AreEqual(2, caseListRemovedDtUnitId.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedDtUnitId[0].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedDtUnitId[0].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedDtUnitId[0].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedDtUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedDtUnitId[0].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedDtUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedDtUnitId[0].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedDtUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedDtUnitId[0].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedDtUnitId[0].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedDtUnitId[0].Status);
            Assert.AreEqual(aCase1Removed.CheckListId, caseListRemovedDtUnitId[0].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedDtUnitId[0].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedDtUnitId[0].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedDtUnitId[0].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedDtUnitId[0].WorkflowState);

            #endregion


            #region caseListRemovedDtUnitId aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedDtUnitId[1].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedDtUnitId[1].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedDtUnitId[1].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedDtUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedDtUnitId[1].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedDtUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedDtUnitId[1].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedDtUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedDtUnitId[1].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedDtUnitId[1].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedDtUnitId[1].Status);
            Assert.AreEqual(aCase3Removed.CheckListId, caseListRemovedDtUnitId[1].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedDtUnitId[1].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedDtUnitId[1].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedDtUnitId[1].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedDtUnitId[1].WorkflowState);

            #endregion

            #endregion

            #endregion

            #endregion

            #region Case Sort

            #region aCase Sort Asc

            #region aCase1Removed sort asc

            #region caseListC1DoneAt aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDoneAt);
            Assert.AreEqual(0, caseListRemovedC1SortDoneAt.Count);
            // Assert.AreEqual(aCase1Removed.type, caseListRemovedC1SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase1Removed.case_uid, caseListRemovedC1SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase1Removed.microting_check_uid, caseListRemovedC1SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedC1SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.custom, caseListRemovedC1SortDoneAt[0].Custom);
            // Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedC1SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Removed.Id, caseListRemovedC1SortDoneAt[0].Id);
            // Assert.AreEqual(aCase1Removed.microting_uid, caseListRemovedC1SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1Removed.site.microting_uid, caseListRemovedC1SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1Removed.site.name, caseListRemovedC1SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1Removed.status, caseListRemovedC1SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC1SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1Removed.unit.Id, caseListRemovedC1SortDoneAt[0].UnitId);
            // Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedC1SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.version, caseListRemovedC1SortDoneAt[0].Version);
            // Assert.AreEqual(aCase1Removed.worker.first_name + " " + aCase1Removed.worker.last_name, caseListRemovedC1SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase1Removed.workflow_state, caseListRemovedC1SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC1SortStatus aCase1Removed

            Assert.NotNull(caseListRemovedC1SortStatus);
            Assert.AreEqual(0, caseListRemovedC1SortStatus.Count);
            // Assert.AreEqual(aCase1Removed.type, caseListRemovedC1SortStatus[0].CaseType);
            // Assert.AreEqual(aCase1Removed.case_uid, caseListRemovedC1SortStatus[0].CaseUId);
            // Assert.AreEqual(aCase1Removed.microting_check_uid, caseListRemovedC1SortStatus[0].CheckUIid);
            // Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedC1SortStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.custom, caseListRemovedC1SortStatus[0].Custom);
            // Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedC1SortStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Removed.Id, caseListRemovedC1SortStatus[0].Id);
            // Assert.AreEqual(aCase1Removed.microting_uid, caseListRemovedC1SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1Removed.site.microting_uid, caseListRemovedC1SortStatus[0].SiteId);
            // Assert.AreEqual(aCase1Removed.site.name, caseListRemovedC1SortStatus[0].SiteName);
            // Assert.AreEqual(aCase1Removed.status, caseListRemovedC1SortStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC1SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase1Removed.unit.Id, caseListRemovedC1SortStatus[0].UnitId);
            // Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedC1SortStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.version, caseListRemovedC1SortStatus[0].Version);
            // Assert.AreEqual(aCase1Removed.worker.first_name + " " + aCase1Removed.worker.last_name, caseListRemovedC1SortStatus[0].WorkerName);
            // Assert.AreEqual(aCase1Removed.workflow_state, caseListRemovedC1SortStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC1SortUnitId

            Assert.NotNull(caseListRemovedC1SortUnitId);
            Assert.AreEqual(0, caseListRemovedC1SortUnitId.Count);
            // Assert.AreEqual(aCase1Removed.type, caseListRemovedC1SortUnitId[0].CaseType);
            // Assert.AreEqual(aCase1Removed.case_uid, caseListRemovedC1SortUnitId[0].CaseUId);
            // Assert.AreEqual(aCase1Removed.microting_check_uid, caseListRemovedC1SortUnitId[0].CheckUIid);
            // Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedC1SortUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.custom, caseListRemovedC1SortUnitId[0].Custom);
            // Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedC1SortUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Removed.Id, caseListRemovedC1SortUnitId[0].Id);
            // Assert.AreEqual(aCase1Removed.microting_uid, caseListRemovedC1SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1Removed.site.microting_uid, caseListRemovedC1SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase1Removed.site.name, caseListRemovedC1SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase1Removed.status, caseListRemovedC1SortUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC1SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1Removed.unit.Id, caseListRemovedC1SortUnitId[0].UnitId);
            // Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedC1SortUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.version, caseListRemovedC1SortUnitId[0].Version);
            // Assert.AreEqual(aCase1Removed.worker.first_name + " " + aCase1Removed.worker.last_name, caseListRemovedC1SortUnitId[0].WorkerName);
            // Assert.AreEqual(aCase1Removed.workflow_state, caseListRemovedC1SortUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase2Removed sort asc

            #region caseListC2DoneAt aCase2Removed

            Assert.NotNull(caseListRemovedC2SortDoneAt);
            Assert.AreEqual(0, caseListRemovedC2SortDoneAt.Count);
            // Assert.AreEqual(aCase2Removed.type, caseListRemovedC2SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase2Removed.case_uid, caseListRemovedC2SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase2Removed.microting_check_uid, caseListRemovedC2SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedC2SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.custom, caseListRemovedC2SortDoneAt[0].Custom);
            // Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedC2SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Removed.Id, caseListRemovedC2SortDoneAt[0].Id);
            // Assert.AreEqual(aCase2Removed.microting_uid, caseListRemovedC2SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2Removed.site.microting_uid, caseListRemovedC2SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2Removed.site.name, caseListRemovedC2SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2Removed.status, caseListRemovedC2SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC2SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2Removed.unit.Id, caseListRemovedC2SortDoneAt[0].UnitId);
            // Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedC2SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.version, caseListRemovedC2SortDoneAt[0].Version);
            // Assert.AreEqual(aCase2Removed.worker.first_name + " " + aCase2Removed.worker.last_name, caseListRemovedC2SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase2Removed.workflow_state, caseListRemovedC2SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC2SortStatus aCase2Removed

            Assert.NotNull(caseListRemovedC2SortStatus);
            Assert.AreEqual(0, caseListRemovedC2SortStatus.Count);
            // Assert.AreEqual(aCase2Removed.type, caseListRemovedC2SortStatus[0].CaseType);
            // Assert.AreEqual(aCase2Removed.case_uid, caseListRemovedC2SortStatus[0].CaseUId);
            // Assert.AreEqual(aCase2Removed.microting_check_uid, caseListRemovedC2SortStatus[0].CheckUIid);
            // Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedC2SortStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.custom, caseListRemovedC2SortStatus[0].Custom);
            // Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedC2SortStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Removed.Id, caseListRemovedC2SortStatus[0].Id);
            // Assert.AreEqual(aCase2Removed.microting_uid, caseListRemovedC2SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2Removed.site.microting_uid, caseListRemovedC2SortStatus[0].SiteId);
            // Assert.AreEqual(aCase2Removed.site.name, caseListRemovedC2SortStatus[0].SiteName);
            // Assert.AreEqual(aCase2Removed.status, caseListRemovedC2SortStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC2SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase2Removed.unit.Id, caseListRemovedC2SortStatus[0].UnitId);
            // Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedC2SortStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.version, caseListRemovedC2SortStatus[0].Version);
            // Assert.AreEqual(aCase2Removed.worker.first_name + " " + aCase2Removed.worker.last_name, caseListRemovedC2SortStatus[0].WorkerName);
            // Assert.AreEqual(aCase2Removed.workflow_state, caseListRemovedC2SortStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC2SortUnitId aCase2Removed

            Assert.NotNull(caseListRemovedC2SortUnitId);
            Assert.AreEqual(0, caseListRemovedC2SortUnitId.Count);
            // Assert.AreEqual(aCase2Removed.type, caseListRemovedC2SortUnitId[0].CaseType);
            // Assert.AreEqual(aCase2Removed.case_uid, caseListRemovedC2SortUnitId[0].CaseUId);
            // Assert.AreEqual(aCase2Removed.microting_check_uid, caseListRemovedC2SortUnitId[0].CheckUIid);
            // Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedC2SortUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.custom, caseListRemovedC2SortUnitId[0].Custom);
            // Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedC2SortUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Removed.Id, caseListRemovedC2SortUnitId[0].Id);
            // Assert.AreEqual(aCase2Removed.microting_uid, caseListRemovedC2SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2Removed.site.microting_uid, caseListRemovedC2SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase2Removed.site.name, caseListRemovedC2SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase2Removed.status, caseListRemovedC2SortUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC2SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2Removed.unit.Id, caseListRemovedC2SortUnitId[0].UnitId);
            // Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedC2SortUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.version, caseListRemovedC2SortUnitId[0].Version);
            // Assert.AreEqual(aCase2Removed.worker.first_name + " " + aCase2Removed.worker.last_name, caseListRemovedC2SortUnitId[0].WorkerName);
            // Assert.AreEqual(aCase2Removed.workflow_state, caseListRemovedC2SortUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase3Removed sort asc

            #region caseListC3DoneAt aCase3Removed

            Assert.NotNull(caseListRemovedC3SortDoneAt);
            Assert.AreEqual(0, caseListRemovedC3SortDoneAt.Count);
            // Assert.AreEqual(aCase3Removed.type, caseListRemovedC3SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase3Removed.case_uid, caseListRemovedC3SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase3Removed.microting_check_uid, caseListRemovedC3SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedC3SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.custom, caseListRemovedC3SortDoneAt[0].Custom);
            // Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedC3SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Removed.Id, caseListRemovedC3SortDoneAt[0].Id);
            // Assert.AreEqual(aCase3Removed.microting_uid, caseListRemovedC3SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3Removed.site.microting_uid, caseListRemovedC3SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3Removed.site.name, caseListRemovedC3SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3Removed.status, caseListRemovedC3SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC3SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3Removed.unit.Id, caseListRemovedC3SortDoneAt[0].UnitId);
            // Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedC3SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.version, caseListRemovedC3SortDoneAt[0].Version);
            // Assert.AreEqual(aCase3Removed.worker.first_name + " " + aCase3Removed.worker.last_name, caseListRemovedC3SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase3Removed.workflow_state, caseListRemovedC3SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListC3status aCase3Removed

            Assert.NotNull(caseListRemovedC3SortStatus);
            Assert.AreEqual(0, caseListRemovedC3SortStatus.Count);
            // Assert.AreEqual(aCase3Removed.type, caseListRemovedC3SortStatus[0].CaseType);
            // Assert.AreEqual(aCase3Removed.case_uid, caseListRemovedC3SortStatus[0].CaseUId);
            // Assert.AreEqual(aCase3Removed.microting_check_uid, caseListRemovedC3SortStatus[0].CheckUIid);
            // Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedC3SortStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.custom, caseListRemovedC3SortStatus[0].Custom);
            // Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedC3SortStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Removed.Id, caseListRemovedC3SortStatus[0].Id);
            // Assert.AreEqual(aCase3Removed.microting_uid, caseListRemovedC3SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3Removed.site.microting_uid, caseListRemovedC3SortStatus[0].SiteId);
            // Assert.AreEqual(aCase3Removed.site.name, caseListRemovedC3SortStatus[0].SiteName);
            // Assert.AreEqual(aCase3Removed.status, caseListRemovedC3SortStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC3SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase3Removed.unit.Id, caseListRemovedC3SortStatus[0].UnitId);
            // Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedC3SortStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.version, caseListRemovedC3SortStatus[0].Version);
            // Assert.AreEqual(aCase3Removed.worker.first_name + " " + aCase3Removed.worker.last_name, caseListRemovedC3SortStatus[0].WorkerName);
            // Assert.AreEqual(aCase3Removed.workflow_state, caseListRemovedC3SortStatus[0].WorkflowState);

            #endregion

            #region caseListC3UnitId aCase3Removed

            Assert.NotNull(caseListRemovedC3SortUnitId);
            Assert.AreEqual(0, caseListRemovedC3SortUnitId.Count);
            // Assert.AreEqual(aCase3Removed.type, caseListRemovedC3SortUnitId[0].CaseType);
            // Assert.AreEqual(aCase3Removed.case_uid, caseListRemovedC3SortUnitId[0].CaseUId);
            // Assert.AreEqual(aCase3Removed.microting_check_uid, caseListRemovedC3SortUnitId[0].CheckUIid);
            // Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedC3SortUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.custom, caseListRemovedC3SortUnitId[0].Custom);
            // Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedC3SortUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Removed.Id, caseListRemovedC3SortUnitId[0].Id);
            // Assert.AreEqual(aCase3Removed.microting_uid, caseListRemovedC3SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3Removed.site.microting_uid, caseListRemovedC3SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase3Removed.site.name, caseListRemovedC3SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase3Removed.status, caseListRemovedC3SortUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC3SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3Removed.unit.Id, caseListRemovedC3SortUnitId[0].UnitId);
            // Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedC3SortUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.version, caseListRemovedC3SortUnitId[0].Version);
            // Assert.AreEqual(aCase3Removed.worker.first_name + " " + aCase3Removed.worker.last_name, caseListRemovedC3SortUnitId[0].WorkerName);
            // Assert.AreEqual(aCase3Removed.workflow_state, caseListRemovedC3SortUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase4Removed sort asc

            #region caseListRemovedC4SortDoneAt aCase4Removed

            Assert.NotNull(caseListRemovedC4SortDoneAt);
            Assert.AreEqual(0, caseListRemovedC4SortDoneAt.Count);
            // Assert.AreEqual(aCase4Removed.type, caseListRemovedC4SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4Removed.case_uid, caseListRemovedC4SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4Removed.microting_check_uid, caseListRemovedC4SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedC4SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.custom, caseListRemovedC4SortDoneAt[0].Custom);
            // Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedC4SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Removed.Id, caseListRemovedC4SortDoneAt[0].Id);
            // Assert.AreEqual(aCase4Removed.microting_uid, caseListRemovedC4SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4Removed.site.microting_uid, caseListRemovedC4SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4Removed.site.name, caseListRemovedC4SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4Removed.status, caseListRemovedC4SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC4SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4Removed.unit.Id, caseListRemovedC4SortDoneAt[0].UnitId);
            // Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedC4SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.version, caseListRemovedC4SortDoneAt[0].Version);
            // Assert.AreEqual(aCase4Removed.worker.first_name + " " + aCase4Removed.worker.last_name, caseListRemovedC4SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4Removed.workflow_state, caseListRemovedC4SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC4SortStatus aCase4Removed

            Assert.NotNull(caseListRemovedC4SortStatus);
            Assert.AreEqual(0, caseListRemovedC4SortStatus.Count);
            // Assert.AreEqual(aCase4Removed.type, caseListRemovedC4SortStatus[0].CaseType);
            // Assert.AreEqual(aCase4Removed.case_uid, caseListRemovedC4SortStatus[0].CaseUId);
            // Assert.AreEqual(aCase4Removed.microting_check_uid, caseListRemovedC4SortStatus[0].CheckUIid);
            // Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedC4SortStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.custom, caseListRemovedC4SortStatus[0].Custom);
            // Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedC4SortStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Removed.Id, caseListRemovedC4SortStatus[0].Id);
            // Assert.AreEqual(aCase4Removed.microting_uid, caseListRemovedC4SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase4Removed.site.microting_uid, caseListRemovedC4SortStatus[0].SiteId);
            // Assert.AreEqual(aCase4Removed.site.name, caseListRemovedC4SortStatus[0].SiteName);
            // Assert.AreEqual(aCase4Removed.status, caseListRemovedC4SortStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC4SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase4Removed.unit.Id, caseListRemovedC4SortStatus[0].UnitId);
            // Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedC4SortStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.version, caseListRemovedC4SortStatus[0].Version);
            // Assert.AreEqual(aCase4Removed.worker.first_name + " " + aCase4Removed.worker.last_name, caseListRemovedC4SortStatus[0].WorkerName);
            // Assert.AreEqual(aCase4Removed.workflow_state, caseListRemovedC4SortStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC4SortUnitId aCase4Removed

            Assert.NotNull(caseListRemovedC4SortUnitId);
            Assert.AreEqual(0, caseListRemovedC4SortUnitId.Count);
            // Assert.AreEqual(aCase4Removed.type, caseListRemovedC4SortUnitId[0].CaseType);
            // Assert.AreEqual(aCase4Removed.case_uid, caseListRemovedC4SortUnitId[0].CaseUId);
            // Assert.AreEqual(aCase4Removed.microting_check_uid, caseListRemovedC4SortUnitId[0].CheckUIid);
            // Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedC4SortUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.custom, caseListRemovedC4SortUnitId[0].Custom);
            // Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedC4SortUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Removed.Id, caseListRemovedC4SortUnitId[0].Id);
            // Assert.AreEqual(aCase4Removed.microting_uid, caseListRemovedC4SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase4Removed.site.microting_uid, caseListRemovedC4SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase4Removed.site.name, caseListRemovedC4SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase4Removed.status, caseListRemovedC4SortUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC4SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase4Removed.unit.Id, caseListRemovedC4SortUnitId[0].UnitId);
            // Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedC4SortUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.version, caseListRemovedC4SortUnitId[0].Version);
            // Assert.AreEqual(aCase4Removed.worker.first_name + " " + aCase4Removed.worker.last_name, caseListRemovedC4SortUnitId[0].WorkerName);
            // Assert.AreEqual(aCase4Removed.workflow_state, caseListRemovedC4SortUnitId[0].WorkflowState);

            #endregion

            #endregion

            #endregion


            #region aCase Sort Asc w. DateTime

            #region aCase1Removed sort asc w. DateTime

            #region caseListRemovedC1SortDtDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDtDoneAt);
            Assert.AreEqual(0, caseListRemovedC1SortDtDoneAt.Count);
            // Assert.AreEqual(aCase1Removed.type, caseListRemovedC1SortDtDoneAt[0].CaseType);
            // Assert.AreEqual(aCase1Removed.case_uid, caseListRemovedC1SortDtDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase1Removed.microting_check_uid, caseListRemovedC1SortDtDoneAt[0].CheckUIid);
            // Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedC1SortDtDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.custom, caseListRemovedC1SortDtDoneAt[0].Custom);
            // Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedC1SortDtDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Removed.Id, caseListRemovedC1SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase1Removed.microting_uid, caseListRemovedC1SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1Removed.site.microting_uid, caseListRemovedC1SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1Removed.site.name, caseListRemovedC1SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1Removed.status, caseListRemovedC1SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC1SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1Removed.unit.Id, caseListRemovedC1SortDtDoneAt[0].UnitId);
            // Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedC1SortDtDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.version, caseListRemovedC1SortDtDoneAt[0].Version);
            // Assert.AreEqual(aCase1Removed.worker.first_name + " " + aCase1Removed.worker.last_name, caseListRemovedC1SortDtDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase1Removed.workflow_state, caseListRemovedC1SortDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC1SortDtStatus aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDtStatus);
            Assert.AreEqual(0, caseListRemovedC1SortDtStatus.Count);
            // Assert.AreEqual(aCase1Removed.type, caseListRemovedC1SortDtStatus[0].CaseType);
            // Assert.AreEqual(aCase1Removed.case_uid, caseListRemovedC1SortDtStatus[0].CaseUId);
            // Assert.AreEqual(aCase1Removed.microting_check_uid, caseListRemovedC1SortDtStatus[0].CheckUIid);
            // Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedC1SortDtStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.custom, caseListRemovedC1SortDtStatus[0].Custom);
            // Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedC1SortDtStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Removed.Id, caseListRemovedC1SortDtStatus[0].Id);
            // Assert.AreEqual(aCase1Removed.microting_uid, caseListRemovedC1SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1Removed.site.microting_uid, caseListRemovedC1SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase1Removed.site.name, caseListRemovedC1SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase1Removed.status, caseListRemovedC1SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC1SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase1Removed.unit.Id, caseListRemovedC1SortDtStatus[0].UnitId);
            // Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedC1SortDtStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.version, caseListRemovedC1SortDtStatus[0].Version);
            // Assert.AreEqual(aCase1Removed.worker.first_name + " " + aCase1Removed.worker.last_name, caseListRemovedC1SortDtStatus[0].WorkerName);
            // Assert.AreEqual(aCase1Removed.workflow_state, caseListRemovedC1SortDtStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC1SortDtUnitId aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDtUnitId);
            Assert.AreEqual(0, caseListRemovedC1SortDtUnitId.Count);
            // Assert.AreEqual(aCase1Removed.type, caseListRemovedC1SortDtUnitId[0].CaseType);
            // Assert.AreEqual(aCase1Removed.case_uid, caseListRemovedC1SortDtUnitId[0].CaseUId);
            // Assert.AreEqual(aCase1Removed.microting_check_uid, caseListRemovedC1SortDtUnitId[0].CheckUIid);
            // Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedC1SortDtUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.custom, caseListRemovedC1SortDtUnitId[0].Custom);
            // Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedC1SortDtUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Removed.Id, caseListRemovedC1SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase1Removed.microting_uid, caseListRemovedC1SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1Removed.site.microting_uid, caseListRemovedC1SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase1Removed.site.name, caseListRemovedC1SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase1Removed.status, caseListRemovedC1SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC1SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1Removed.unit.Id, caseListRemovedC1SortDtUnitId[0].UnitId);
            // Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedC1SortDtUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.version, caseListRemovedC1SortDtUnitId[0].Version);
            // Assert.AreEqual(aCase1Removed.worker.first_name + " " + aCase1Removed.worker.last_name, caseListRemovedC1SortDtUnitId[0].WorkerName);
            // Assert.AreEqual(aCase1Removed.workflow_state, caseListRemovedC1SortDtUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase2Removed sort asc w. DateTime

            #region caseListRemovedC2SortDtDoneAt aCase2Removed

            Assert.NotNull(caseListRemovedC2SortDtDoneAt);
            Assert.AreEqual(0, caseListRemovedC2SortDtDoneAt.Count);
            // Assert.AreEqual(aCase2Removed.type, caseListRemovedC2SortDtDoneAt[0].CaseType);
            // Assert.AreEqual(aCase2Removed.case_uid, caseListRemovedC2SortDtDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase2Removed.microting_check_uid, caseListRemovedC2SortDtDoneAt[0].CheckUIid);
            // Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedC2SortDtDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.custom, caseListRemovedC2SortDtDoneAt[0].Custom);
            // Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedC2SortDtDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Removed.Id, caseListRemovedC2SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase2Removed.microting_uid, caseListRemovedC2SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2Removed.site.microting_uid, caseListRemovedC2SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2Removed.site.name, caseListRemovedC2SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2Removed.status, caseListRemovedC2SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC2SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2Removed.unit.Id, caseListRemovedC2SortDtDoneAt[0].UnitId);
            // Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedC2SortDtDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.version, caseListRemovedC2SortDtDoneAt[0].Version);
            // Assert.AreEqual(aCase2Removed.worker.first_name + " " + aCase2Removed.worker.last_name, caseListRemovedC2SortDtDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase2Removed.workflow_state, caseListRemovedC2SortDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC2SortDtStatus aCase2Removed

            Assert.NotNull(caseListRemovedC2SortDtStatus);
            Assert.AreEqual(0, caseListRemovedC2SortDtStatus.Count);
            // Assert.AreEqual(aCase2Removed.type, caseListRemovedC2SortDtStatus[0].CaseType);
            // Assert.AreEqual(aCase2Removed.case_uid, caseListRemovedC2SortDtStatus[0].CaseUId);
            // Assert.AreEqual(aCase2Removed.microting_check_uid, caseListRemovedC2SortDtStatus[0].CheckUIid);
            // Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedC2SortDtStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.custom, caseListRemovedC2SortDtStatus[0].Custom);
            // Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedC2SortDtStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Removed.Id, caseListRemovedC2SortDtStatus[0].Id);
            // Assert.AreEqual(aCase2Removed.microting_uid, caseListRemovedC2SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2Removed.site.microting_uid, caseListRemovedC2SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase2Removed.site.name, caseListRemovedC2SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase2Removed.status, caseListRemovedC2SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC2SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase2Removed.unit.Id, caseListRemovedC2SortDtStatus[0].UnitId);
            // Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedC2SortDtStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.version, caseListRemovedC2SortDtStatus[0].Version);
            // Assert.AreEqual(aCase2Removed.worker.first_name + " " + aCase2Removed.worker.last_name, caseListRemovedC2SortDtStatus[0].WorkerName);
            // Assert.AreEqual(aCase2Removed.workflow_state, caseListRemovedC2SortDtStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC2SortDtUnitId aCase2Removed

            Assert.NotNull(caseListRemovedC2SortDtUnitId);
            Assert.AreEqual(0, caseListRemovedC2SortDtUnitId.Count);
            // Assert.AreEqual(aCase2Removed.type, caseListRemovedC2SortDtUnitId[0].CaseType);
            // Assert.AreEqual(aCase2Removed.case_uid, caseListRemovedC2SortDtUnitId[0].CaseUId);
            // Assert.AreEqual(aCase2Removed.microting_check_uid, caseListRemovedC2SortDtUnitId[0].CheckUIid);
            // Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedC2SortDtUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.custom, caseListRemovedC2SortDtUnitId[0].Custom);
            // Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedC2SortDtUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Removed.Id, caseListRemovedC2SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase2Removed.microting_uid, caseListRemovedC2SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2Removed.site.microting_uid, caseListRemovedC2SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase2Removed.site.name, caseListRemovedC2SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase2Removed.status, caseListRemovedC2SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC2SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2Removed.unit.Id, caseListRemovedC2SortDtUnitId[0].UnitId);
            // Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedC2SortDtUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.version, caseListRemovedC2SortDtUnitId[0].Version);
            // Assert.AreEqual(aCase2Removed.worker.first_name + " " + aCase2Removed.worker.last_name, caseListRemovedC2SortDtUnitId[0].WorkerName);
            // Assert.AreEqual(aCase2Removed.workflow_state, caseListRemovedC2SortDtUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase3Removed sort asc w. DateTime

            #region caseListRemovedC3SortDtDoneAt aCase3Removed

            Assert.NotNull(caseListRemovedC3SortDtDoneAt);
            Assert.AreEqual(0, caseListRemovedC3SortDtDoneAt.Count);
            // Assert.AreEqual(aCase3Removed.type, caseListRemovedC3SortDtDoneAt[0].CaseType);
            // Assert.AreEqual(aCase3Removed.case_uid, caseListRemovedC3SortDtDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase3Removed.microting_check_uid, caseListRemovedC3SortDtDoneAt[0].CheckUIid);
            // Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedC3SortDtDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.custom, caseListRemovedC3SortDtDoneAt[0].Custom);
            // Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedC3SortDtDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Removed.Id, caseListRemovedC3SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase3Removed.microting_uid, caseListRemovedC3SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3Removed.site.microting_uid, caseListRemovedC3SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3Removed.site.name, caseListRemovedC3SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3Removed.status, caseListRemovedC3SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC3SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3Removed.unit.Id, caseListRemovedC3SortDtDoneAt[0].UnitId);
            // Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedC3SortDtDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.version, caseListRemovedC3SortDtDoneAt[0].Version);
            // Assert.AreEqual(aCase3Removed.worker.first_name + " " + aCase3Removed.worker.last_name, caseListRemovedC3SortDtDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase3Removed.workflow_state, caseListRemovedC3SortDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC3SortDtStatus aCase3Removed

            Assert.NotNull(caseListRemovedC3SortDtStatus);
            Assert.AreEqual(0, caseListRemovedC3SortDtStatus.Count);
            // Assert.AreEqual(aCase3Removed.type, caseListRemovedC3SortDtStatus[0].CaseType);
            // Assert.AreEqual(aCase3Removed.case_uid, caseListRemovedC3SortDtStatus[0].CaseUId);
            // Assert.AreEqual(aCase3Removed.microting_check_uid, caseListRemovedC3SortDtStatus[0].CheckUIid);
            // Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedC3SortDtStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.custom, caseListRemovedC3SortDtStatus[0].Custom);
            // Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedC3SortDtStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Removed.Id, caseListRemovedC3SortDtStatus[0].Id);
            // Assert.AreEqual(aCase3Removed.microting_uid, caseListRemovedC3SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3Removed.site.microting_uid, caseListRemovedC3SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase3Removed.site.name, caseListRemovedC3SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase3Removed.status, caseListRemovedC3SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC3SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase3Removed.unit.Id, caseListRemovedC3SortDtStatus[0].UnitId);
            // Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedC3SortDtStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.version, caseListRemovedC3SortDtStatus[0].Version);
            // Assert.AreEqual(aCase3Removed.worker.first_name + " " + aCase3Removed.worker.last_name, caseListRemovedC3SortDtStatus[0].WorkerName);
            // Assert.AreEqual(aCase3Removed.workflow_state, caseListRemovedC3SortDtStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC3SortDtUnitId aCase3Removed

            Assert.NotNull(caseListRemovedC3SortDtUnitId);
            Assert.AreEqual(0, caseListRemovedC3SortDtUnitId.Count);
            // Assert.AreEqual(aCase3Removed.type, caseListRemovedC3SortDtUnitId[0].CaseType);
            // Assert.AreEqual(aCase3Removed.case_uid, caseListRemovedC3SortDtUnitId[0].CaseUId);
            // Assert.AreEqual(aCase3Removed.microting_check_uid, caseListRemovedC3SortDtUnitId[0].CheckUIid);
            // Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedC3SortDtUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.custom, caseListRemovedC3SortDtUnitId[0].Custom);
            // Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedC3SortDtUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Removed.Id, caseListRemovedC3SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase3Removed.microting_uid, caseListRemovedC3SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3Removed.site.microting_uid, caseListRemovedC3SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase3Removed.site.name, caseListRemovedC3SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase3Removed.status, caseListRemovedC3SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC3SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3Removed.unit.Id, caseListRemovedC3SortDtUnitId[0].UnitId);
            // Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedC3SortDtUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.version, caseListRemovedC3SortDtUnitId[0].Version);
            // Assert.AreEqual(aCase3Removed.worker.first_name + " " + aCase3Removed.worker.last_name, caseListRemovedC3SortDtUnitId[0].WorkerName);
            // Assert.AreEqual(aCase3Removed.workflow_state, caseListRemovedC3SortDtUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase4Removed sort asc w. DateTime

            #region caseListRemovedC4SortDtDoneAt aCase4Removed

            Assert.NotNull(caseListRemovedC4SortDtDoneAt);
            Assert.AreEqual(0, caseListRemovedC4SortDtDoneAt.Count);
            // Assert.AreEqual(aCase4Removed.type, caseListRemovedC4SortDtDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4Removed.case_uid, caseListRemovedC4SortDtDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4Removed.microting_check_uid, caseListRemovedC4SortDtDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedC4SortDtDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.custom, caseListRemovedC4SortDtDoneAt[0].Custom);
            // Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedC4SortDtDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Removed.Id, caseListRemovedC4SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase4Removed.microting_uid, caseListRemovedC4SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4Removed.site.microting_uid, caseListRemovedC4SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4Removed.site.name, caseListRemovedC4SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4Removed.status, caseListRemovedC4SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC4SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4Removed.unit.Id, caseListRemovedC4SortDtDoneAt[0].UnitId);
            // Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedC4SortDtDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.version, caseListRemovedC4SortDtDoneAt[0].Version);
            // Assert.AreEqual(aCase4Removed.worker.first_name + " " + aCase4Removed.worker.last_name, caseListRemovedC4SortDtDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4Removed.workflow_state, caseListRemovedC4SortDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC4SortDtStatus aCase4Removed

            Assert.NotNull(caseListRemovedC4SortDtStatus);
            Assert.AreEqual(0, caseListRemovedC4SortDtStatus.Count);
            // Assert.AreEqual(aCase4Removed.type, caseListRemovedC4SortDtStatus[0].CaseType);
            // Assert.AreEqual(aCase4Removed.case_uid, caseListRemovedC4SortDtStatus[0].CaseUId);
            // Assert.AreEqual(aCase4Removed.microting_check_uid, caseListRemovedC4SortDtStatus[0].CheckUIid);
            // Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedC4SortDtStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.custom, caseListRemovedC4SortDtStatus[0].Custom);
            // Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedC4SortDtStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Removed.Id, caseListRemovedC4SortDtStatus[0].Id);
            // Assert.AreEqual(aCase4Removed.microting_uid, caseListRemovedC4SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase4Removed.site.microting_uid, caseListRemovedC4SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase4Removed.site.name, caseListRemovedC4SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase4Removed.status, caseListRemovedC4SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC4SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase4Removed.unit.Id, caseListRemovedC4SortDtStatus[0].UnitId);
            // Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedC4SortDtStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.version, caseListRemovedC4SortDtStatus[0].Version);
            // Assert.AreEqual(aCase4Removed.worker.first_name + " " + aCase4Removed.worker.last_name, caseListRemovedC4SortDtStatus[0].WorkerName);
            // Assert.AreEqual(aCase4Removed.workflow_state, caseListRemovedC4SortDtStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC4SortDtUnitId aCase4Removed

            Assert.NotNull(caseListRemovedC4SortDtUnitId);
            Assert.AreEqual(0, caseListRemovedC4SortDtUnitId.Count);
            // Assert.AreEqual(aCase4Removed.type, caseListRemovedC4SortDtUnitId[0].CaseType);
            // Assert.AreEqual(aCase4Removed.case_uid, caseListRemovedC4SortDtUnitId[0].CaseUId);
            // Assert.AreEqual(aCase4Removed.microting_check_uid, caseListRemovedC4SortDtUnitId[0].CheckUIid);
            // Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedC4SortDtUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.custom, caseListRemovedC4SortDtUnitId[0].Custom);
            // Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedC4SortDtUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Removed.Id, caseListRemovedC4SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase4Removed.microting_uid, caseListRemovedC4SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase4Removed.site.microting_uid, caseListRemovedC4SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase4Removed.site.name, caseListRemovedC4SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase4Removed.status, caseListRemovedC4SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRemovedC4SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase4Removed.unit.Id, caseListRemovedC4SortDtUnitId[0].UnitId);
            // Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedC4SortDtUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.version, caseListRemovedC4SortDtUnitId[0].Version);
            // Assert.AreEqual(aCase4Removed.worker.first_name + " " + aCase4Removed.worker.last_name, caseListRemovedC4SortDtUnitId[0].WorkerName);
            // Assert.AreEqual(aCase4Removed.workflow_state, caseListRemovedC4SortDtUnitId[0].WorkflowState);

            #endregion

            #endregion

            #endregion

            #endregion

            #endregion

            #region sort by WorkflowState Retracted

            #region Def Sort

            #region Def Sort Asc

            #region caseListRetractedDoneAt Def Sort Asc

            #region caseListRetractedDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedDoneAt);
            Assert.AreEqual(4, caseListRetractedDoneAt.Count);
            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedDoneAt[0].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedDoneAt[0].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedDoneAt[0].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedDoneAt[0].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedDoneAt[0].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedDoneAt[0].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedDoneAt[0].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDoneAt[0].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedDoneAt[0].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedDoneAt[0].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedDoneAt[0].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedDoneAt aCase2Retracted

            Assert.AreEqual(aCase2Retracted.Type, caseListRetractedDoneAt[1].CaseType);
            Assert.AreEqual(aCase2Retracted.CaseUid, caseListRetractedDoneAt[1].CaseUId);
            Assert.AreEqual(aCase2Retracted.MicrotingCheckUid, caseListRetractedDoneAt[1].CheckUIid);
            Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Custom, caseListRetractedDoneAt[1].Custom);
            Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase2Retracted.Id, caseListRetractedDoneAt[1].Id);
            Assert.AreEqual(aCase2Retracted.MicrotingUid, caseListRetractedDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase2Retracted.Site.MicrotingUid, caseListRetractedDoneAt[1].SiteId);
            Assert.AreEqual(aCase2Retracted.Site.Name, caseListRetractedDoneAt[1].SiteName);
            Assert.AreEqual(aCase2Retracted.Status, caseListRetractedDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDoneAt[1].TemplatId);
            Assert.AreEqual(aCase2Retracted.Unit.MicrotingUid, caseListRetractedDoneAt[1].UnitId);
//            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Version, caseListRetractedDoneAt[1].Version);
            Assert.AreEqual(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName,
                caseListRetractedDoneAt[1].WorkerName);
            Assert.AreEqual(aCase2Retracted.WorkflowState, caseListRetractedDoneAt[1].WorkflowState);

            #endregion

            #region caseListRetractedDoneAt aCase3Retracted

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedDoneAt[2].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedDoneAt[2].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedDoneAt[2].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedDoneAt[2].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedDoneAt[2].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedDoneAt[2].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedDoneAt[2].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedDoneAt[2].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedDoneAt[2].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedDoneAt[2].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedDoneAt[2].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDoneAt[2].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedDoneAt[2].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDoneAt[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedDoneAt[2].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedDoneAt[2].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedDoneAt[2].WorkflowState);

            #endregion

            #region caseListRetractedDoneAt aCase4Retracted

            Assert.AreEqual(aCase4Retracted.Type, caseListRetractedDoneAt[3].CaseType);
            Assert.AreEqual(aCase4Retracted.CaseUid, caseListRetractedDoneAt[3].CaseUId);
            Assert.AreEqual(aCase4Retracted.MicrotingCheckUid, caseListRetractedDoneAt[3].CheckUIid);
            Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedDoneAt[3].CreatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Custom, caseListRetractedDoneAt[3].Custom);
            Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedDoneAt[3].DoneAt.ToString());
            Assert.AreEqual(aCase4Retracted.Id, caseListRetractedDoneAt[3].Id);
            Assert.AreEqual(aCase4Retracted.MicrotingUid, caseListRetractedDoneAt[3].MicrotingUId);
            Assert.AreEqual(aCase4Retracted.Site.MicrotingUid, caseListRetractedDoneAt[3].SiteId);
            Assert.AreEqual(aCase4Retracted.Site.Name, caseListRetractedDoneAt[3].SiteName);
            Assert.AreEqual(aCase4Retracted.Status, caseListRetractedDoneAt[3].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDoneAt[3].TemplatId);
            Assert.AreEqual(aCase4Retracted.Unit.MicrotingUid, caseListRetractedDoneAt[3].UnitId);
//            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedDoneAt[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Version, caseListRetractedDoneAt[3].Version);
            Assert.AreEqual(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName,
                caseListRetractedDoneAt[3].WorkerName);
            Assert.AreEqual(aCase4Retracted.WorkflowState, caseListRetractedDoneAt[3].WorkflowState);

            #endregion

            #endregion

            #region caseListRetractedStatus Def Sort Asc

            #region caseListRetractedStatus aCase1Retracted

            Assert.NotNull(caseListRetractedStatus);
            Assert.AreEqual(4, caseListRetractedStatus.Count);
            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedStatus[0].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedStatus[0].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedStatus[0].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedStatus[0].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedStatus[0].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedStatus[0].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedStatus[0].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedStatus[0].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedStatus[0].Status);
            Assert.AreEqual(aCase1Retracted.CheckListId, caseListRetractedStatus[0].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedStatus[0].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedStatus[0].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedStatus[0].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedStatus aCase2Retracted

            Assert.AreEqual(aCase2Retracted.Type, caseListRetractedStatus[1].CaseType);
            Assert.AreEqual(aCase2Retracted.CaseUid, caseListRetractedStatus[1].CaseUId);
            Assert.AreEqual(aCase2Retracted.MicrotingCheckUid, caseListRetractedStatus[1].CheckUIid);
            Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Custom, caseListRetractedStatus[1].Custom);
            Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase2Retracted.Id, caseListRetractedStatus[1].Id);
            Assert.AreEqual(aCase2Retracted.MicrotingUid, caseListRetractedStatus[1].MicrotingUId);
            Assert.AreEqual(aCase2Retracted.Site.MicrotingUid, caseListRetractedStatus[1].SiteId);
            Assert.AreEqual(aCase2Retracted.Site.Name, caseListRetractedStatus[1].SiteName);
            Assert.AreEqual(aCase2Retracted.Status, caseListRetractedStatus[1].Status);
            Assert.AreEqual(aCase2Retracted.CheckListId, caseListRetractedStatus[1].TemplatId);
            Assert.AreEqual(aCase2Retracted.Unit.MicrotingUid, caseListRetractedStatus[1].UnitId);
//            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Version, caseListRetractedStatus[1].Version);
            Assert.AreEqual(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName,
                caseListRetractedStatus[1].WorkerName);
            Assert.AreEqual(aCase2Retracted.WorkflowState, caseListRetractedStatus[1].WorkflowState);

            #endregion

            #region caseListRetractedStatus aCase3Retracted

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedStatus[2].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedStatus[2].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedStatus[2].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedStatus[2].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedStatus[2].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedStatus[2].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedStatus[2].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedStatus[2].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedStatus[2].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedStatus[2].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedStatus[2].Status);
            Assert.AreEqual(aCase3Retracted.CheckListId, caseListRetractedStatus[2].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedStatus[2].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedStatus[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedStatus[2].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedStatus[2].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedStatus[2].WorkflowState);

            #endregion

            #region caseListRetractedStatus aCase4Retracted

            Assert.AreEqual(aCase4Retracted.Type, caseListRetractedStatus[3].CaseType);
            Assert.AreEqual(aCase4Retracted.CaseUid, caseListRetractedStatus[3].CaseUId);
            Assert.AreEqual(aCase4Retracted.MicrotingCheckUid, caseListRetractedStatus[3].CheckUIid);
            Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedStatus[3].CreatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Custom, caseListRetractedStatus[3].Custom);
            Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedStatus[3].DoneAt.ToString());
            Assert.AreEqual(aCase4Retracted.Id, caseListRetractedStatus[3].Id);
            Assert.AreEqual(aCase4Retracted.MicrotingUid, caseListRetractedStatus[3].MicrotingUId);
            Assert.AreEqual(aCase4Retracted.Site.MicrotingUid, caseListRetractedStatus[3].SiteId);
            Assert.AreEqual(aCase4Retracted.Site.Name, caseListRetractedStatus[3].SiteName);
            Assert.AreEqual(aCase4Retracted.Status, caseListRetractedStatus[3].Status);
            Assert.AreEqual(aCase4Retracted.CheckListId, caseListRetractedStatus[3].TemplatId);
            Assert.AreEqual(aCase4Retracted.Unit.MicrotingUid, caseListRetractedStatus[3].UnitId);
//            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedStatus[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Version, caseListRetractedStatus[3].Version);
            Assert.AreEqual(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName,
                caseListRetractedStatus[3].WorkerName);
            Assert.AreEqual(aCase4Retracted.WorkflowState, caseListRetractedStatus[3].WorkflowState);

            #endregion

            #endregion

            #region caseListRetractedUnitId Def Sort Asc

            #region caseListRetractedUnitId aCase1Retracted

            Assert.NotNull(caseListRetractedUnitId);
            Assert.AreEqual(4, caseListRetractedUnitId.Count);
            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedUnitId[0].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedUnitId[0].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedUnitId[0].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedUnitId[0].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedUnitId[0].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedUnitId[0].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedUnitId[0].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedUnitId[0].Status);
            Assert.AreEqual(aCase1Retracted.CheckListId, caseListRetractedUnitId[0].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedUnitId[0].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedUnitId[0].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedUnitId[0].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedUnitId[0].WorkflowState);

            #endregion

            #region caseListRetractedUnitId aCase2Retracted

            Assert.AreEqual(aCase2Retracted.Type, caseListRetractedUnitId[1].CaseType);
            Assert.AreEqual(aCase2Retracted.CaseUid, caseListRetractedUnitId[1].CaseUId);
            Assert.AreEqual(aCase2Retracted.MicrotingCheckUid, caseListRetractedUnitId[1].CheckUIid);
            Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Custom, caseListRetractedUnitId[1].Custom);
            Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase2Retracted.Id, caseListRetractedUnitId[1].Id);
            Assert.AreEqual(aCase2Retracted.MicrotingUid, caseListRetractedUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase2Retracted.Site.MicrotingUid, caseListRetractedUnitId[1].SiteId);
            Assert.AreEqual(aCase2Retracted.Site.Name, caseListRetractedUnitId[1].SiteName);
            Assert.AreEqual(aCase2Retracted.Status, caseListRetractedUnitId[1].Status);
            Assert.AreEqual(aCase2Retracted.CheckListId, caseListRetractedUnitId[1].TemplatId);
            Assert.AreEqual(aCase2Retracted.Unit.MicrotingUid, caseListRetractedUnitId[1].UnitId);
//            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Version, caseListRetractedUnitId[1].Version);
            Assert.AreEqual(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName,
                caseListRetractedUnitId[1].WorkerName);
            Assert.AreEqual(aCase2Retracted.WorkflowState, caseListRetractedUnitId[1].WorkflowState);

            #endregion

            #region caseListRetractedUnitId aCase3Retracted

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedUnitId[2].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedUnitId[2].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedUnitId[2].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedUnitId[2].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedUnitId[2].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedUnitId[2].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedUnitId[2].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedUnitId[2].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedUnitId[2].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedUnitId[2].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedUnitId[2].Status);
            Assert.AreEqual(aCase3Retracted.CheckListId, caseListRetractedUnitId[2].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedUnitId[2].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedUnitId[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedUnitId[2].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedUnitId[2].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedUnitId[2].WorkflowState);

            #endregion

            #region caseListRetractedUnitId aCase4Retracted

            Assert.AreEqual(aCase4Retracted.Type, caseListRetractedUnitId[3].CaseType);
            Assert.AreEqual(aCase4Retracted.CaseUid, caseListRetractedUnitId[3].CaseUId);
            Assert.AreEqual(aCase4Retracted.MicrotingCheckUid, caseListRetractedUnitId[3].CheckUIid);
            Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedUnitId[3].CreatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Custom, caseListRetractedUnitId[3].Custom);
            Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedUnitId[3].DoneAt.ToString());
            Assert.AreEqual(aCase4Retracted.Id, caseListRetractedUnitId[3].Id);
            Assert.AreEqual(aCase4Retracted.MicrotingUid, caseListRetractedUnitId[3].MicrotingUId);
            Assert.AreEqual(aCase4Retracted.Site.MicrotingUid, caseListRetractedUnitId[3].SiteId);
            Assert.AreEqual(aCase4Retracted.Site.Name, caseListRetractedUnitId[3].SiteName);
            Assert.AreEqual(aCase4Retracted.Status, caseListRetractedUnitId[3].Status);
            Assert.AreEqual(aCase4Retracted.CheckListId, caseListRetractedUnitId[3].TemplatId);
            Assert.AreEqual(aCase4Retracted.Unit.MicrotingUid, caseListRetractedUnitId[3].UnitId);
//            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedUnitId[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Version, caseListRetractedUnitId[3].Version);
            Assert.AreEqual(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName,
                caseListRetractedUnitId[3].WorkerName);
            Assert.AreEqual(aCase4Retracted.WorkflowState, caseListRetractedUnitId[3].WorkflowState);

            #endregion

            #endregion

            #endregion


            #region Def Sort Asc w. DateTime

            #region caseListRetractedDtDoneAt Def Sort Asc w. DateTime

            #region caseListRetractedDtDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedDtDoneAt);
            Assert.AreEqual(2, caseListRetractedDtDoneAt.Count);
            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedDtDoneAt[0].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedDtDoneAt[0].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedDtDoneAt[0].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedDtDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedDtDoneAt[0].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedDtDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedDtDoneAt[0].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedDtDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedDtDoneAt[0].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedDtDoneAt[0].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedDtDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDtDoneAt[0].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedDtDoneAt[0].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedDtDoneAt[0].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedDtDoneAt[0].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedDtDoneAt aCase3Retracted

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedDtDoneAt[1].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedDtDoneAt[1].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedDtDoneAt[1].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedDtDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedDtDoneAt[1].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedDtDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedDtDoneAt[1].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedDtDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedDtDoneAt[1].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedDtDoneAt[1].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedDtDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDtDoneAt[1].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedDtDoneAt[1].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedDtDoneAt[1].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedDtDoneAt[1].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedDtDoneAt[1].WorkflowState);

            #endregion

            #endregion

            #region caseListRetractedDtStatus Def Sort Asc w. DateTime

            #region caseListRetractedDtStatus aCase1Retracted

            Assert.NotNull(caseListRetractedDtStatus);
            Assert.AreEqual(2, caseListRetractedDtStatus.Count);
            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedDtStatus[0].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedDtStatus[0].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedDtStatus[0].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedDtStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedDtStatus[0].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedDtStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedDtStatus[0].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedDtStatus[0].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedDtStatus[0].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedDtStatus[0].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedDtStatus[0].Status);
            Assert.AreEqual(aCase1Retracted.CheckListId, caseListRetractedDtStatus[0].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedDtStatus[0].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedDtStatus[0].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedDtStatus[0].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedDtStatus[0].WorkflowState);

            #endregion


            #region caseListRetractedDtStatus aCase3Retracted

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedDtStatus[1].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedDtStatus[1].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedDtStatus[1].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedDtStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedDtStatus[1].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedDtStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedDtStatus[1].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedDtStatus[1].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedDtStatus[1].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedDtStatus[1].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedDtStatus[1].Status);
            Assert.AreEqual(aCase3Retracted.CheckListId, caseListRetractedDtStatus[1].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedDtStatus[1].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedDtStatus[1].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedDtStatus[1].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedDtStatus[1].WorkflowState);

            #endregion

            #endregion

            #region caseListRetractedDtUnitId Def Sort Asc w. DateTime

            #region caseListRetractedDtUnitId aCase1Retracted

            Assert.NotNull(caseListRetractedDtUnitId);
            Assert.AreEqual(2, caseListRetractedDtUnitId.Count);
            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedDtUnitId[0].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedDtUnitId[0].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedDtUnitId[0].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedDtUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedDtUnitId[0].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedDtUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedDtUnitId[0].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedDtUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedDtUnitId[0].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedDtUnitId[0].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedDtUnitId[0].Status);
            Assert.AreEqual(aCase1Retracted.CheckListId, caseListRetractedDtUnitId[0].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedDtUnitId[0].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedDtUnitId[0].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedDtUnitId[0].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedDtUnitId[0].WorkflowState);

            #endregion


            #region caseListRetractedDtUnitId aCase3Retracted

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedDtUnitId[1].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedDtUnitId[1].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedDtUnitId[1].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedDtUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedDtUnitId[1].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedDtUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedDtUnitId[1].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedDtUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedDtUnitId[1].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedDtUnitId[1].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedDtUnitId[1].Status);
            Assert.AreEqual(aCase3Retracted.CheckListId, caseListRetractedDtUnitId[1].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedDtUnitId[1].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedDtUnitId[1].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedDtUnitId[1].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedDtUnitId[1].WorkflowState);

            #endregion

            #endregion

            #endregion

            #endregion

            #region Case Sort

            #region aCase Sort Asc

            #region aCase1Retracted sort asc

            #region caseListC1DoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDoneAt);
            Assert.AreEqual(0, caseListRetractedC1SortDoneAt.Count);
            // Assert.AreEqual(aCase1Retracted.type, caseListRetractedC1SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase1Retracted.case_uid, caseListRetractedC1SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase1Retracted.microting_check_uid, caseListRetractedC1SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedC1SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.custom, caseListRetractedC1SortDoneAt[0].Custom);
            // Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedC1SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Retracted.Id, caseListRetractedC1SortDoneAt[0].Id);
            // Assert.AreEqual(aCase1Retracted.microting_uid, caseListRetractedC1SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1Retracted.site.microting_uid, caseListRetractedC1SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1Retracted.site.name, caseListRetractedC1SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1Retracted.status, caseListRetractedC1SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC1SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1Retracted.unit.Id, caseListRetractedC1SortDoneAt[0].UnitId);
            // Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedC1SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.version, caseListRetractedC1SortDoneAt[0].Version);
            // Assert.AreEqual(aCase1Retracted.worker.first_name + " " + aCase1Retracted.worker.last_name, caseListRetractedC1SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase1Retracted.workflow_state, caseListRetractedC1SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC1SortStatus aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortStatus);
            Assert.AreEqual(0, caseListRetractedC1SortStatus.Count);
            // Assert.AreEqual(aCase1Retracted.type, caseListRetractedC1SortStatus[0].CaseType);
            // Assert.AreEqual(aCase1Retracted.case_uid, caseListRetractedC1SortStatus[0].CaseUId);
            // Assert.AreEqual(aCase1Retracted.microting_check_uid, caseListRetractedC1SortStatus[0].CheckUIid);
            // Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedC1SortStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.custom, caseListRetractedC1SortStatus[0].Custom);
            // Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedC1SortStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Retracted.Id, caseListRetractedC1SortStatus[0].Id);
            // Assert.AreEqual(aCase1Retracted.microting_uid, caseListRetractedC1SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1Retracted.site.microting_uid, caseListRetractedC1SortStatus[0].SiteId);
            // Assert.AreEqual(aCase1Retracted.site.name, caseListRetractedC1SortStatus[0].SiteName);
            // Assert.AreEqual(aCase1Retracted.status, caseListRetractedC1SortStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC1SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase1Retracted.unit.Id, caseListRetractedC1SortStatus[0].UnitId);
            // Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedC1SortStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.version, caseListRetractedC1SortStatus[0].Version);
            // Assert.AreEqual(aCase1Retracted.worker.first_name + " " + aCase1Retracted.worker.last_name, caseListRetractedC1SortStatus[0].WorkerName);
            // Assert.AreEqual(aCase1Retracted.workflow_state, caseListRetractedC1SortStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC1SortUnitId

            Assert.NotNull(caseListRetractedC1SortUnitId);
            Assert.AreEqual(0, caseListRetractedC1SortUnitId.Count);
            // Assert.AreEqual(aCase1Retracted.type, caseListRetractedC1SortUnitId[0].CaseType);
            // Assert.AreEqual(aCase1Retracted.case_uid, caseListRetractedC1SortUnitId[0].CaseUId);
            // Assert.AreEqual(aCase1Retracted.microting_check_uid, caseListRetractedC1SortUnitId[0].CheckUIid);
            // Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedC1SortUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.custom, caseListRetractedC1SortUnitId[0].Custom);
            // Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedC1SortUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Retracted.Id, caseListRetractedC1SortUnitId[0].Id);
            // Assert.AreEqual(aCase1Retracted.microting_uid, caseListRetractedC1SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1Retracted.site.microting_uid, caseListRetractedC1SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase1Retracted.site.name, caseListRetractedC1SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase1Retracted.status, caseListRetractedC1SortUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC1SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1Retracted.unit.Id, caseListRetractedC1SortUnitId[0].UnitId);
            // Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedC1SortUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.version, caseListRetractedC1SortUnitId[0].Version);
            // Assert.AreEqual(aCase1Retracted.worker.first_name + " " + aCase1Retracted.worker.last_name, caseListRetractedC1SortUnitId[0].WorkerName);
            // Assert.AreEqual(aCase1Retracted.workflow_state, caseListRetractedC1SortUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase2Retracted sort asc

            #region caseListC2DoneAt aCase2Retracted

            Assert.NotNull(caseListRetractedC2SortDoneAt);
            Assert.AreEqual(0, caseListRetractedC2SortDoneAt.Count);
            // Assert.AreEqual(aCase2Retracted.type, caseListRetractedC2SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase2Retracted.case_uid, caseListRetractedC2SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase2Retracted.microting_check_uid, caseListRetractedC2SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedC2SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.custom, caseListRetractedC2SortDoneAt[0].Custom);
            // Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedC2SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Retracted.Id, caseListRetractedC2SortDoneAt[0].Id);
            // Assert.AreEqual(aCase2Retracted.microting_uid, caseListRetractedC2SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2Retracted.site.microting_uid, caseListRetractedC2SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2Retracted.site.name, caseListRetractedC2SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2Retracted.status, caseListRetractedC2SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC2SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2Retracted.unit.Id, caseListRetractedC2SortDoneAt[0].UnitId);
            // Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedC2SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.version, caseListRetractedC2SortDoneAt[0].Version);
            // Assert.AreEqual(aCase2Retracted.worker.first_name + " " + aCase2Retracted.worker.last_name, caseListRetractedC2SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase2Retracted.workflow_state, caseListRetractedC2SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC2SortStatus aCase2Retracted

            Assert.NotNull(caseListRetractedC2SortStatus);
            Assert.AreEqual(0, caseListRetractedC2SortStatus.Count);
            // Assert.AreEqual(aCase2Retracted.type, caseListRetractedC2SortStatus[0].CaseType);
            // Assert.AreEqual(aCase2Retracted.case_uid, caseListRetractedC2SortStatus[0].CaseUId);
            // Assert.AreEqual(aCase2Retracted.microting_check_uid, caseListRetractedC2SortStatus[0].CheckUIid);
            // Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedC2SortStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.custom, caseListRetractedC2SortStatus[0].Custom);
            // Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedC2SortStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Retracted.Id, caseListRetractedC2SortStatus[0].Id);
            // Assert.AreEqual(aCase2Retracted.microting_uid, caseListRetractedC2SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2Retracted.site.microting_uid, caseListRetractedC2SortStatus[0].SiteId);
            // Assert.AreEqual(aCase2Retracted.site.name, caseListRetractedC2SortStatus[0].SiteName);
            // Assert.AreEqual(aCase2Retracted.status, caseListRetractedC2SortStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC2SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase2Retracted.unit.Id, caseListRetractedC2SortStatus[0].UnitId);
            // Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedC2SortStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.version, caseListRetractedC2SortStatus[0].Version);
            // Assert.AreEqual(aCase2Retracted.worker.first_name + " " + aCase2Retracted.worker.last_name, caseListRetractedC2SortStatus[0].WorkerName);
            // Assert.AreEqual(aCase2Retracted.workflow_state, caseListRetractedC2SortStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC2SortUnitId aCase2Retracted

            Assert.NotNull(caseListRetractedC2SortUnitId);
            Assert.AreEqual(0, caseListRetractedC2SortUnitId.Count);
            // Assert.AreEqual(aCase2Retracted.type, caseListRetractedC2SortUnitId[0].CaseType);
            // Assert.AreEqual(aCase2Retracted.case_uid, caseListRetractedC2SortUnitId[0].CaseUId);
            // Assert.AreEqual(aCase2Retracted.microting_check_uid, caseListRetractedC2SortUnitId[0].CheckUIid);
            // Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedC2SortUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.custom, caseListRetractedC2SortUnitId[0].Custom);
            // Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedC2SortUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Retracted.Id, caseListRetractedC2SortUnitId[0].Id);
            // Assert.AreEqual(aCase2Retracted.microting_uid, caseListRetractedC2SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2Retracted.site.microting_uid, caseListRetractedC2SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase2Retracted.site.name, caseListRetractedC2SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase2Retracted.status, caseListRetractedC2SortUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC2SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2Retracted.unit.Id, caseListRetractedC2SortUnitId[0].UnitId);
            // Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedC2SortUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.version, caseListRetractedC2SortUnitId[0].Version);
            // Assert.AreEqual(aCase2Retracted.worker.first_name + " " + aCase2Retracted.worker.last_name, caseListRetractedC2SortUnitId[0].WorkerName);
            // Assert.AreEqual(aCase2Retracted.workflow_state, caseListRetractedC2SortUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase3Retracted sort asc

            #region caseListC3DoneAt aCase3Retracted

            Assert.NotNull(caseListRetractedC3SortDoneAt);
            Assert.AreEqual(0, caseListRetractedC3SortDoneAt.Count);
            // Assert.AreEqual(aCase3Retracted.type, caseListRetractedC3SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase3Retracted.case_uid, caseListRetractedC3SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase3Retracted.microting_check_uid, caseListRetractedC3SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedC3SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.custom, caseListRetractedC3SortDoneAt[0].Custom);
            // Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedC3SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Retracted.Id, caseListRetractedC3SortDoneAt[0].Id);
            // Assert.AreEqual(aCase3Retracted.microting_uid, caseListRetractedC3SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3Retracted.site.microting_uid, caseListRetractedC3SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3Retracted.site.name, caseListRetractedC3SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3Retracted.status, caseListRetractedC3SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC3SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3Retracted.unit.Id, caseListRetractedC3SortDoneAt[0].UnitId);
            // Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedC3SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.version, caseListRetractedC3SortDoneAt[0].Version);
            // Assert.AreEqual(aCase3Retracted.worker.first_name + " " + aCase3Retracted.worker.last_name, caseListRetractedC3SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase3Retracted.workflow_state, caseListRetractedC3SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListC3status aCase3Retracted

            Assert.NotNull(caseListRetractedC3SortStatus);
            Assert.AreEqual(0, caseListRetractedC3SortStatus.Count);
            // Assert.AreEqual(aCase3Retracted.type, caseListRetractedC3SortStatus[0].CaseType);
            // Assert.AreEqual(aCase3Retracted.case_uid, caseListRetractedC3SortStatus[0].CaseUId);
            // Assert.AreEqual(aCase3Retracted.microting_check_uid, caseListRetractedC3SortStatus[0].CheckUIid);
            // Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedC3SortStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.custom, caseListRetractedC3SortStatus[0].Custom);
            // Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedC3SortStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Retracted.Id, caseListRetractedC3SortStatus[0].Id);
            // Assert.AreEqual(aCase3Retracted.microting_uid, caseListRetractedC3SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3Retracted.site.microting_uid, caseListRetractedC3SortStatus[0].SiteId);
            // Assert.AreEqual(aCase3Retracted.site.name, caseListRetractedC3SortStatus[0].SiteName);
            // Assert.AreEqual(aCase3Retracted.status, caseListRetractedC3SortStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC3SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase3Retracted.unit.Id, caseListRetractedC3SortStatus[0].UnitId);
            // Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedC3SortStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.version, caseListRetractedC3SortStatus[0].Version);
            // Assert.AreEqual(aCase3Retracted.worker.first_name + " " + aCase3Retracted.worker.last_name, caseListRetractedC3SortStatus[0].WorkerName);
            // Assert.AreEqual(aCase3Retracted.workflow_state, caseListRetractedC3SortStatus[0].WorkflowState);

            #endregion

            #region caseListC3UnitId aCase3Retracted

            Assert.NotNull(caseListRetractedC3SortUnitId);
            Assert.AreEqual(0, caseListRetractedC3SortUnitId.Count);
            // Assert.AreEqual(aCase3Retracted.type, caseListRetractedC3SortUnitId[0].CaseType);
            // Assert.AreEqual(aCase3Retracted.case_uid, caseListRetractedC3SortUnitId[0].CaseUId);
            // Assert.AreEqual(aCase3Retracted.microting_check_uid, caseListRetractedC3SortUnitId[0].CheckUIid);
            // Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedC3SortUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.custom, caseListRetractedC3SortUnitId[0].Custom);
            // Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedC3SortUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Retracted.Id, caseListRetractedC3SortUnitId[0].Id);
            // Assert.AreEqual(aCase3Retracted.microting_uid, caseListRetractedC3SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3Retracted.site.microting_uid, caseListRetractedC3SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase3Retracted.site.name, caseListRetractedC3SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase3Retracted.status, caseListRetractedC3SortUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC3SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3Retracted.unit.Id, caseListRetractedC3SortUnitId[0].UnitId);
            // Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedC3SortUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.version, caseListRetractedC3SortUnitId[0].Version);
            // Assert.AreEqual(aCase3Retracted.worker.first_name + " " + aCase3Retracted.worker.last_name, caseListRetractedC3SortUnitId[0].WorkerName);
            // Assert.AreEqual(aCase3Retracted.workflow_state, caseListRetractedC3SortUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase4Retracted sort asc

            #region caseListRetractedC4SortDoneAt aCase4Retracted

            Assert.NotNull(caseListRetractedC4SortDoneAt);
            Assert.AreEqual(0, caseListRetractedC4SortDoneAt.Count);
            // Assert.AreEqual(aCase4Retracted.type, caseListRetractedC4SortDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4Retracted.case_uid, caseListRetractedC4SortDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4Retracted.microting_check_uid, caseListRetractedC4SortDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedC4SortDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.custom, caseListRetractedC4SortDoneAt[0].Custom);
            // Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedC4SortDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Retracted.Id, caseListRetractedC4SortDoneAt[0].Id);
            // Assert.AreEqual(aCase4Retracted.microting_uid, caseListRetractedC4SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4Retracted.site.microting_uid, caseListRetractedC4SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4Retracted.site.name, caseListRetractedC4SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4Retracted.status, caseListRetractedC4SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC4SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4Retracted.unit.Id, caseListRetractedC4SortDoneAt[0].UnitId);
            // Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedC4SortDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.version, caseListRetractedC4SortDoneAt[0].Version);
            // Assert.AreEqual(aCase4Retracted.worker.first_name + " " + aCase4Retracted.worker.last_name, caseListRetractedC4SortDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4Retracted.workflow_state, caseListRetractedC4SortDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC4SortStatus aCase4Retracted

            Assert.NotNull(caseListRetractedC4SortStatus);
            Assert.AreEqual(0, caseListRetractedC4SortStatus.Count);
            // Assert.AreEqual(aCase4Retracted.type, caseListRetractedC4SortStatus[0].CaseType);
            // Assert.AreEqual(aCase4Retracted.case_uid, caseListRetractedC4SortStatus[0].CaseUId);
            // Assert.AreEqual(aCase4Retracted.microting_check_uid, caseListRetractedC4SortStatus[0].CheckUIid);
            // Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedC4SortStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.custom, caseListRetractedC4SortStatus[0].Custom);
            // Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedC4SortStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Retracted.Id, caseListRetractedC4SortStatus[0].Id);
            // Assert.AreEqual(aCase4Retracted.microting_uid, caseListRetractedC4SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase4Retracted.site.microting_uid, caseListRetractedC4SortStatus[0].SiteId);
            // Assert.AreEqual(aCase4Retracted.site.name, caseListRetractedC4SortStatus[0].SiteName);
            // Assert.AreEqual(aCase4Retracted.status, caseListRetractedC4SortStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC4SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase4Retracted.unit.Id, caseListRetractedC4SortStatus[0].UnitId);
            // Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedC4SortStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.version, caseListRetractedC4SortStatus[0].Version);
            // Assert.AreEqual(aCase4Retracted.worker.first_name + " " + aCase4Retracted.worker.last_name, caseListRetractedC4SortStatus[0].WorkerName);
            // Assert.AreEqual(aCase4Retracted.workflow_state, caseListRetractedC4SortStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC4SortUnitId aCase4Retracted

            Assert.NotNull(caseListRetractedC4SortUnitId);
            Assert.AreEqual(0, caseListRetractedC4SortUnitId.Count);
            // Assert.AreEqual(aCase4Retracted.type, caseListRetractedC4SortUnitId[0].CaseType);
            // Assert.AreEqual(aCase4Retracted.case_uid, caseListRetractedC4SortUnitId[0].CaseUId);
            // Assert.AreEqual(aCase4Retracted.microting_check_uid, caseListRetractedC4SortUnitId[0].CheckUIid);
            // Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedC4SortUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.custom, caseListRetractedC4SortUnitId[0].Custom);
            // Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedC4SortUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Retracted.Id, caseListRetractedC4SortUnitId[0].Id);
            // Assert.AreEqual(aCase4Retracted.microting_uid, caseListRetractedC4SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase4Retracted.site.microting_uid, caseListRetractedC4SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase4Retracted.site.name, caseListRetractedC4SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase4Retracted.status, caseListRetractedC4SortUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC4SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase4Retracted.unit.Id, caseListRetractedC4SortUnitId[0].UnitId);
            // Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedC4SortUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.version, caseListRetractedC4SortUnitId[0].Version);
            // Assert.AreEqual(aCase4Retracted.worker.first_name + " " + aCase4Retracted.worker.last_name, caseListRetractedC4SortUnitId[0].WorkerName);
            // Assert.AreEqual(aCase4Retracted.workflow_state, caseListRetractedC4SortUnitId[0].WorkflowState);

            #endregion

            #endregion

            #endregion


            #region aCase Sort Asc w. DateTime

            #region aCase1Retracted sort asc w. DateTime

            #region caseListRetractedC1SortDtDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDtDoneAt);
            Assert.AreEqual(0, caseListRetractedC1SortDtDoneAt.Count);
            // Assert.AreEqual(aCase1Retracted.type, caseListRetractedC1SortDtDoneAt[0].CaseType);
            // Assert.AreEqual(aCase1Retracted.case_uid, caseListRetractedC1SortDtDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase1Retracted.microting_check_uid, caseListRetractedC1SortDtDoneAt[0].CheckUIid);
            // Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedC1SortDtDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.custom, caseListRetractedC1SortDtDoneAt[0].Custom);
            // Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedC1SortDtDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Retracted.Id, caseListRetractedC1SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase1Retracted.microting_uid, caseListRetractedC1SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1Retracted.site.microting_uid, caseListRetractedC1SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1Retracted.site.name, caseListRetractedC1SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1Retracted.status, caseListRetractedC1SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC1SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1Retracted.unit.Id, caseListRetractedC1SortDtDoneAt[0].UnitId);
            // Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedC1SortDtDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.version, caseListRetractedC1SortDtDoneAt[0].Version);
            // Assert.AreEqual(aCase1Retracted.worker.first_name + " " + aCase1Retracted.worker.last_name, caseListRetractedC1SortDtDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase1Retracted.workflow_state, caseListRetractedC1SortDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC1SortDtStatus aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDtStatus);
            Assert.AreEqual(0, caseListRetractedC1SortDtStatus.Count);
            // Assert.AreEqual(aCase1Retracted.type, caseListRetractedC1SortDtStatus[0].CaseType);
            // Assert.AreEqual(aCase1Retracted.case_uid, caseListRetractedC1SortDtStatus[0].CaseUId);
            // Assert.AreEqual(aCase1Retracted.microting_check_uid, caseListRetractedC1SortDtStatus[0].CheckUIid);
            // Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedC1SortDtStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.custom, caseListRetractedC1SortDtStatus[0].Custom);
            // Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedC1SortDtStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Retracted.Id, caseListRetractedC1SortDtStatus[0].Id);
            // Assert.AreEqual(aCase1Retracted.microting_uid, caseListRetractedC1SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1Retracted.site.microting_uid, caseListRetractedC1SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase1Retracted.site.name, caseListRetractedC1SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase1Retracted.status, caseListRetractedC1SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC1SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase1Retracted.unit.Id, caseListRetractedC1SortDtStatus[0].UnitId);
            // Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedC1SortDtStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.version, caseListRetractedC1SortDtStatus[0].Version);
            // Assert.AreEqual(aCase1Retracted.worker.first_name + " " + aCase1Retracted.worker.last_name, caseListRetractedC1SortDtStatus[0].WorkerName);
            // Assert.AreEqual(aCase1Retracted.workflow_state, caseListRetractedC1SortDtStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC1SortDtUnitId aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDtUnitId);
            Assert.AreEqual(0, caseListRetractedC1SortDtUnitId.Count);
            // Assert.AreEqual(aCase1Retracted.type, caseListRetractedC1SortDtUnitId[0].CaseType);
            // Assert.AreEqual(aCase1Retracted.case_uid, caseListRetractedC1SortDtUnitId[0].CaseUId);
            // Assert.AreEqual(aCase1Retracted.microting_check_uid, caseListRetractedC1SortDtUnitId[0].CheckUIid);
            // Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedC1SortDtUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.custom, caseListRetractedC1SortDtUnitId[0].Custom);
            // Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedC1SortDtUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Retracted.Id, caseListRetractedC1SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase1Retracted.microting_uid, caseListRetractedC1SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1Retracted.site.microting_uid, caseListRetractedC1SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase1Retracted.site.name, caseListRetractedC1SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase1Retracted.status, caseListRetractedC1SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC1SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1Retracted.unit.Id, caseListRetractedC1SortDtUnitId[0].UnitId);
            // Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedC1SortDtUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.version, caseListRetractedC1SortDtUnitId[0].Version);
            // Assert.AreEqual(aCase1Retracted.worker.first_name + " " + aCase1Retracted.worker.last_name, caseListRetractedC1SortDtUnitId[0].WorkerName);
            // Assert.AreEqual(aCase1Retracted.workflow_state, caseListRetractedC1SortDtUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase2Retracted sort asc w. DateTime

            #region caseListRetractedC2SortDtDoneAt aCase2Retracted

            Assert.NotNull(caseListRetractedC2SortDtDoneAt);
            Assert.AreEqual(0, caseListRetractedC2SortDtDoneAt.Count);
            // Assert.AreEqual(aCase2Retracted.type, caseListRetractedC2SortDtDoneAt[0].CaseType);
            // Assert.AreEqual(aCase2Retracted.case_uid, caseListRetractedC2SortDtDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase2Retracted.microting_check_uid, caseListRetractedC2SortDtDoneAt[0].CheckUIid);
            // Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedC2SortDtDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.custom, caseListRetractedC2SortDtDoneAt[0].Custom);
            // Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedC2SortDtDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Retracted.Id, caseListRetractedC2SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase2Retracted.microting_uid, caseListRetractedC2SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2Retracted.site.microting_uid, caseListRetractedC2SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2Retracted.site.name, caseListRetractedC2SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2Retracted.status, caseListRetractedC2SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC2SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2Retracted.unit.Id, caseListRetractedC2SortDtDoneAt[0].UnitId);
            // Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedC2SortDtDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.version, caseListRetractedC2SortDtDoneAt[0].Version);
            // Assert.AreEqual(aCase2Retracted.worker.first_name + " " + aCase2Retracted.worker.last_name, caseListRetractedC2SortDtDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase2Retracted.workflow_state, caseListRetractedC2SortDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC2SortDtStatus aCase2Retracted

            Assert.NotNull(caseListRetractedC2SortDtStatus);
            Assert.AreEqual(0, caseListRetractedC2SortDtStatus.Count);
            // Assert.AreEqual(aCase2Retracted.type, caseListRetractedC2SortDtStatus[0].CaseType);
            // Assert.AreEqual(aCase2Retracted.case_uid, caseListRetractedC2SortDtStatus[0].CaseUId);
            // Assert.AreEqual(aCase2Retracted.microting_check_uid, caseListRetractedC2SortDtStatus[0].CheckUIid);
            // Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedC2SortDtStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.custom, caseListRetractedC2SortDtStatus[0].Custom);
            // Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedC2SortDtStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Retracted.Id, caseListRetractedC2SortDtStatus[0].Id);
            // Assert.AreEqual(aCase2Retracted.microting_uid, caseListRetractedC2SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2Retracted.site.microting_uid, caseListRetractedC2SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase2Retracted.site.name, caseListRetractedC2SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase2Retracted.status, caseListRetractedC2SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC2SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase2Retracted.unit.Id, caseListRetractedC2SortDtStatus[0].UnitId);
            // Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedC2SortDtStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.version, caseListRetractedC2SortDtStatus[0].Version);
            // Assert.AreEqual(aCase2Retracted.worker.first_name + " " + aCase2Retracted.worker.last_name, caseListRetractedC2SortDtStatus[0].WorkerName);
            // Assert.AreEqual(aCase2Retracted.workflow_state, caseListRetractedC2SortDtStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC2SortDtUnitId aCase2Retracted

            Assert.NotNull(caseListRetractedC2SortDtUnitId);
            Assert.AreEqual(0, caseListRetractedC2SortDtUnitId.Count);
            // Assert.AreEqual(aCase2Retracted.type, caseListRetractedC2SortDtUnitId[0].CaseType);
            // Assert.AreEqual(aCase2Retracted.case_uid, caseListRetractedC2SortDtUnitId[0].CaseUId);
            // Assert.AreEqual(aCase2Retracted.microting_check_uid, caseListRetractedC2SortDtUnitId[0].CheckUIid);
            // Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedC2SortDtUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.custom, caseListRetractedC2SortDtUnitId[0].Custom);
            // Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedC2SortDtUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Retracted.Id, caseListRetractedC2SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase2Retracted.microting_uid, caseListRetractedC2SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2Retracted.site.microting_uid, caseListRetractedC2SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase2Retracted.site.name, caseListRetractedC2SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase2Retracted.status, caseListRetractedC2SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC2SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2Retracted.unit.Id, caseListRetractedC2SortDtUnitId[0].UnitId);
            // Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedC2SortDtUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.version, caseListRetractedC2SortDtUnitId[0].Version);
            // Assert.AreEqual(aCase2Retracted.worker.first_name + " " + aCase2Retracted.worker.last_name, caseListRetractedC2SortDtUnitId[0].WorkerName);
            // Assert.AreEqual(aCase2Retracted.workflow_state, caseListRetractedC2SortDtUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase3Retracted sort asc w. DateTime

            #region caseListRetractedC3SortDtDoneAt aCase3Retracted

            Assert.NotNull(caseListRetractedC3SortDtDoneAt);
            Assert.AreEqual(0, caseListRetractedC3SortDtDoneAt.Count);
            // Assert.AreEqual(aCase3Retracted.type, caseListRetractedC3SortDtDoneAt[0].CaseType);
            // Assert.AreEqual(aCase3Retracted.case_uid, caseListRetractedC3SortDtDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase3Retracted.microting_check_uid, caseListRetractedC3SortDtDoneAt[0].CheckUIid);
            // Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedC3SortDtDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.custom, caseListRetractedC3SortDtDoneAt[0].Custom);
            // Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedC3SortDtDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Retracted.Id, caseListRetractedC3SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase3Retracted.microting_uid, caseListRetractedC3SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3Retracted.site.microting_uid, caseListRetractedC3SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3Retracted.site.name, caseListRetractedC3SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3Retracted.status, caseListRetractedC3SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC3SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3Retracted.unit.Id, caseListRetractedC3SortDtDoneAt[0].UnitId);
            // Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedC3SortDtDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.version, caseListRetractedC3SortDtDoneAt[0].Version);
            // Assert.AreEqual(aCase3Retracted.worker.first_name + " " + aCase3Retracted.worker.last_name, caseListRetractedC3SortDtDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase3Retracted.workflow_state, caseListRetractedC3SortDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC3SortDtStatus aCase3Retracted

            Assert.NotNull(caseListRetractedC3SortDtStatus);
            Assert.AreEqual(0, caseListRetractedC3SortDtStatus.Count);
            // Assert.AreEqual(aCase3Retracted.type, caseListRetractedC3SortDtStatus[0].CaseType);
            // Assert.AreEqual(aCase3Retracted.case_uid, caseListRetractedC3SortDtStatus[0].CaseUId);
            // Assert.AreEqual(aCase3Retracted.microting_check_uid, caseListRetractedC3SortDtStatus[0].CheckUIid);
            // Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedC3SortDtStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.custom, caseListRetractedC3SortDtStatus[0].Custom);
            // Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedC3SortDtStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Retracted.Id, caseListRetractedC3SortDtStatus[0].Id);
            // Assert.AreEqual(aCase3Retracted.microting_uid, caseListRetractedC3SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3Retracted.site.microting_uid, caseListRetractedC3SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase3Retracted.site.name, caseListRetractedC3SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase3Retracted.status, caseListRetractedC3SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC3SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase3Retracted.unit.Id, caseListRetractedC3SortDtStatus[0].UnitId);
            // Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedC3SortDtStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.version, caseListRetractedC3SortDtStatus[0].Version);
            // Assert.AreEqual(aCase3Retracted.worker.first_name + " " + aCase3Retracted.worker.last_name, caseListRetractedC3SortDtStatus[0].WorkerName);
            // Assert.AreEqual(aCase3Retracted.workflow_state, caseListRetractedC3SortDtStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC3SortDtUnitId aCase3Retracted

            Assert.NotNull(caseListRetractedC3SortDtUnitId);
            Assert.AreEqual(0, caseListRetractedC3SortDtUnitId.Count);
            // Assert.AreEqual(aCase3Retracted.type, caseListRetractedC3SortDtUnitId[0].CaseType);
            // Assert.AreEqual(aCase3Retracted.case_uid, caseListRetractedC3SortDtUnitId[0].CaseUId);
            // Assert.AreEqual(aCase3Retracted.microting_check_uid, caseListRetractedC3SortDtUnitId[0].CheckUIid);
            // Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedC3SortDtUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.custom, caseListRetractedC3SortDtUnitId[0].Custom);
            // Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedC3SortDtUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Retracted.Id, caseListRetractedC3SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase3Retracted.microting_uid, caseListRetractedC3SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3Retracted.site.microting_uid, caseListRetractedC3SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase3Retracted.site.name, caseListRetractedC3SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase3Retracted.status, caseListRetractedC3SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC3SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3Retracted.unit.Id, caseListRetractedC3SortDtUnitId[0].UnitId);
            // Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedC3SortDtUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.version, caseListRetractedC3SortDtUnitId[0].Version);
            // Assert.AreEqual(aCase3Retracted.worker.first_name + " " + aCase3Retracted.worker.last_name, caseListRetractedC3SortDtUnitId[0].WorkerName);
            // Assert.AreEqual(aCase3Retracted.workflow_state, caseListRetractedC3SortDtUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase4Retracted sort asc w. DateTime

            #region caseListRetractedC4SortDtDoneAt aCase4Retracted

            Assert.NotNull(caseListRetractedC4SortDtDoneAt);
            Assert.AreEqual(0, caseListRetractedC4SortDtDoneAt.Count);
            // Assert.AreEqual(aCase4Retracted.type, caseListRetractedC4SortDtDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4Retracted.case_uid, caseListRetractedC4SortDtDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4Retracted.microting_check_uid, caseListRetractedC4SortDtDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedC4SortDtDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.custom, caseListRetractedC4SortDtDoneAt[0].Custom);
            // Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedC4SortDtDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Retracted.Id, caseListRetractedC4SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase4Retracted.microting_uid, caseListRetractedC4SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4Retracted.site.microting_uid, caseListRetractedC4SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4Retracted.site.name, caseListRetractedC4SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4Retracted.status, caseListRetractedC4SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC4SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4Retracted.unit.Id, caseListRetractedC4SortDtDoneAt[0].UnitId);
            // Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedC4SortDtDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.version, caseListRetractedC4SortDtDoneAt[0].Version);
            // Assert.AreEqual(aCase4Retracted.worker.first_name + " " + aCase4Retracted.worker.last_name, caseListRetractedC4SortDtDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4Retracted.workflow_state, caseListRetractedC4SortDtDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC4SortDtStatus aCase4Retracted

            Assert.NotNull(caseListRetractedC4SortDtStatus);
            Assert.AreEqual(0, caseListRetractedC4SortDtStatus.Count);
            // Assert.AreEqual(aCase4Retracted.type, caseListRetractedC4SortDtStatus[0].CaseType);
            // Assert.AreEqual(aCase4Retracted.case_uid, caseListRetractedC4SortDtStatus[0].CaseUId);
            // Assert.AreEqual(aCase4Retracted.microting_check_uid, caseListRetractedC4SortDtStatus[0].CheckUIid);
            // Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedC4SortDtStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.custom, caseListRetractedC4SortDtStatus[0].Custom);
            // Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedC4SortDtStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Retracted.Id, caseListRetractedC4SortDtStatus[0].Id);
            // Assert.AreEqual(aCase4Retracted.microting_uid, caseListRetractedC4SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase4Retracted.site.microting_uid, caseListRetractedC4SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase4Retracted.site.name, caseListRetractedC4SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase4Retracted.status, caseListRetractedC4SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC4SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase4Retracted.unit.Id, caseListRetractedC4SortDtStatus[0].UnitId);
            // Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedC4SortDtStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.version, caseListRetractedC4SortDtStatus[0].Version);
            // Assert.AreEqual(aCase4Retracted.worker.first_name + " " + aCase4Retracted.worker.last_name, caseListRetractedC4SortDtStatus[0].WorkerName);
            // Assert.AreEqual(aCase4Retracted.workflow_state, caseListRetractedC4SortDtStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC4SortDtUnitId aCase4Retracted

            Assert.NotNull(caseListRetractedC4SortDtUnitId);
            Assert.AreEqual(0, caseListRetractedC4SortDtUnitId.Count);
            // Assert.AreEqual(aCase4Retracted.type, caseListRetractedC4SortDtUnitId[0].CaseType);
            // Assert.AreEqual(aCase4Retracted.case_uid, caseListRetractedC4SortDtUnitId[0].CaseUId);
            // Assert.AreEqual(aCase4Retracted.microting_check_uid, caseListRetractedC4SortDtUnitId[0].CheckUIid);
            // Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedC4SortDtUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.custom, caseListRetractedC4SortDtUnitId[0].Custom);
            // Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedC4SortDtUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Retracted.Id, caseListRetractedC4SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase4Retracted.microting_uid, caseListRetractedC4SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase4Retracted.site.microting_uid, caseListRetractedC4SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase4Retracted.site.name, caseListRetractedC4SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase4Retracted.status, caseListRetractedC4SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.Id, caseListRetractedC4SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase4Retracted.unit.Id, caseListRetractedC4SortDtUnitId[0].UnitId);
            // Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedC4SortDtUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.version, caseListRetractedC4SortDtUnitId[0].Version);
            // Assert.AreEqual(aCase4Retracted.worker.first_name + " " + aCase4Retracted.worker.last_name, caseListRetractedC4SortDtUnitId[0].WorkerName);
            // Assert.AreEqual(aCase4Retracted.workflow_state, caseListRetractedC4SortDtUnitId[0].WorkflowState);

            #endregion

            #endregion

            #endregion

            #endregion

            #endregion
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