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
    public class CoreTestCaseReadAllShort : DbTestFixture
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
        public async Task Core_Case_CaseReadAll_Short()
        {
            // Arrance

            #region Arrance

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

            #region cases

            #region cases created

            #region Case1

            Random rnd = new Random();
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
            List<Microting.eForm.Dto.Case> caseListDoneAt = await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
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
            List<Microting.eForm.Dto.Case> caseListStatus = await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListUnitId = await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
            //List<Case> caseListWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region Default sorting ascending, with DateTime

            // Default sorting ascending, with DateTime
            //List<Case> caseListDtCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), timeZoneInfo);
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
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), timeZoneInfo);
            //List<Case> caseListDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region case sorting

            #region aCase sorting ascending

            #region aCase1 sorting ascendng

            //List<Case> caseListC1SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC1SortDoneAt =
                await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
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
            List<Microting.eForm.Dto.Case> caseListC1SortStatus =
                await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC1SortUnitId =
                await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
            //List<Case> caseListC1SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng

            //List<Case> caseListC2SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC2SortDoneAt =
                await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
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
            List<Microting.eForm.Dto.Case> caseListC2SortStatus =
                await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC2SortUnitId =
                await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
            //List<Case> caseListC2SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng

            //List<Case> caseListC3SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC3SortDoneAt =
                await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
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
            List<Microting.eForm.Dto.Case> caseListC3SortStatus =
                await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC3SortUnitId =
                await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
            //List<Case> caseListC3SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng

            //List<Case> caseListC4SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC4SortDoneAt =
                await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
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
            List<Microting.eForm.Dto.Case> caseListC4SortStatus =
                await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC4SortUnitId =
                await sut.CaseReadAll(cl1.Id, null, null, timeZoneInfo);
            //List<Case> caseListC4SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region aCase sorting ascending w. Dt

            #region aCase1 sorting ascendng w. Dt

            //List<Case> caseListC1SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC1SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), timeZoneInfo);
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
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC1SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), timeZoneInfo);
            //List<Case> caseListC1SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng w. Dt

            //List<Case> caseListC2SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC2SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), timeZoneInfo);
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
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC2SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), timeZoneInfo);
            //List<Case> caseListC2SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng w. Dt

            //List<Case> caseListC3SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC3SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), timeZoneInfo);
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
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC3SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), timeZoneInfo);
            //List<Case> caseListC3SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng w. Dt

            //List<Case> caseListC4SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC4SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), timeZoneInfo);
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
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC4SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), timeZoneInfo);
            //List<Case> caseListC4SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.WorkerName);

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
            Assert.AreEqual(8, caseListDoneAt.Count);
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
            Assert.AreEqual(8, caseListStatus.Count);
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
            Assert.AreEqual(8, caseListUnitId.Count);
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
            Assert.AreEqual(4, caseListDtDoneAt.Count);
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
            Assert.AreEqual(4, caseListDtStatus.Count);
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
            Assert.AreEqual(4, caseListDtUnitId.Count);
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
            Assert.AreEqual(8, caseListC1SortDoneAt.Count);
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
            Assert.AreEqual(8, caseListC1SortStatus.Count);
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
            Assert.AreEqual(8, caseListC1SortUnitId.Count);
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
            Assert.AreEqual(8, caseListC2SortDoneAt.Count);
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
            Assert.AreEqual(8, caseListC2SortStatus.Count);
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
            Assert.AreEqual(8, caseListC2SortUnitId.Count);
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
            Assert.AreEqual(8, caseListC3SortDoneAt.Count);
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
            Assert.AreEqual(8, caseListC3SortStatus.Count);
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
            Assert.AreEqual(8, caseListC3SortUnitId.Count);
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
            Assert.AreEqual(8, caseListC4SortDoneAt.Count);
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
            Assert.AreEqual(8, caseListC1SortDoneAt.Count);
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
            Assert.AreEqual(8, caseListC4SortDoneAt.Count);
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
            Assert.AreEqual(4, caseListC1SortDtDoneAt.Count);
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
            Assert.AreEqual(4, caseListC1SortDtStatus.Count);
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
            Assert.AreEqual(4, caseListC1SortDtUnitId.Count);
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
            Assert.AreEqual(4, caseListC2SortDtDoneAt.Count);
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
            Assert.AreEqual(4, caseListC2SortDtStatus.Count);
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
            Assert.AreEqual(4, caseListC2SortDtUnitId.Count);
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
            Assert.AreEqual(4, caseListC3SortDtDoneAt.Count);
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
            Assert.AreEqual(4, caseListC3SortDtStatus.Count);
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
            Assert.AreEqual(4, caseListC3SortDtUnitId.Count);
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
            Assert.AreEqual(4, caseListC4SortDtDoneAt.Count);
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
            Assert.AreEqual(4, caseListC4SortDtStatus.Count);
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
            Assert.AreEqual(4, caseListC4SortDtUnitId.Count);
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