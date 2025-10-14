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

namespace eFormSDK.Integration.Case.CoreTests;

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

        Assert.That(caseListDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListDoneAt.Count, Is.EqualTo(4));
        Assert.That(caseListDoneAt[0].CaseType, Is.EqualTo(aCase1.Type));
        Assert.That(caseListDoneAt[0].CaseUId, Is.EqualTo(aCase1.CaseUid));
        Assert.That(caseListDoneAt[0].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
        Assert.That(caseListDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
        Assert.That(caseListDoneAt[0].Custom, Is.EqualTo(aCase1.Custom));
        Assert.That(caseListDoneAt[0].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
        Assert.That(caseListDoneAt[0].Id, Is.EqualTo(aCase1.Id));
        Assert.That(caseListDoneAt[0].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
        Assert.That(caseListDoneAt[0].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
        Assert.That(caseListDoneAt[0].SiteName, Is.EqualTo(aCase1.Site.Name));
        Assert.That(caseListDoneAt[0].Status, Is.EqualTo(aCase1.Status));
        Assert.That(caseListDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListDoneAt[0].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
        //            Assert.AreEqual(c1_ua.ToString(), caseListDoneAt[0].UpdatedAt.ToString());
        Assert.That(caseListDoneAt[0].Version, Is.EqualTo(aCase1.Version));
        Assert.That(caseListDoneAt[0].WorkerName, Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
        Assert.That(caseListDoneAt[0].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

        #endregion

        #region caseListDoneAt aCase2

        Assert.That(caseListDoneAt[1].CaseType, Is.EqualTo(aCase2.Type));
        Assert.That(caseListDoneAt[1].CaseUId, Is.EqualTo(aCase2.CaseUid));
        Assert.That(caseListDoneAt[1].CheckUIid, Is.EqualTo(aCase2.MicrotingCheckUid));
        Assert.That(caseListDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c2_ca.ToString()));
        Assert.That(caseListDoneAt[1].Custom, Is.EqualTo(aCase2.Custom));
        Assert.That(caseListDoneAt[1].DoneAt.ToString(), Is.EqualTo(c2_da.ToString()));
        Assert.That(caseListDoneAt[1].Id, Is.EqualTo(aCase2.Id));
        Assert.That(caseListDoneAt[1].MicrotingUId, Is.EqualTo(aCase2.MicrotingUid));
        Assert.That(caseListDoneAt[1].SiteId, Is.EqualTo(aCase2.Site.MicrotingUid));
        Assert.That(caseListDoneAt[1].SiteName, Is.EqualTo(aCase2.Site.Name));
        Assert.That(caseListDoneAt[1].Status, Is.EqualTo(aCase2.Status));
        Assert.That(caseListDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListDoneAt[1].UnitId, Is.EqualTo(aCase2.Unit.MicrotingUid));
        //            Assert.AreEqual(c2_ua.ToString(), caseListDoneAt[1].UpdatedAt.ToString());
        Assert.That(caseListDoneAt[1].Version, Is.EqualTo(aCase2.Version));
        Assert.That(caseListDoneAt[1].WorkerName, Is.EqualTo(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName));
        Assert.That(caseListDoneAt[1].WorkflowState, Is.EqualTo(aCase2.WorkflowState));

        #endregion

        #region caseListDoneAt aCase3

        Assert.That(caseListDoneAt[2].CaseType, Is.EqualTo(aCase3.Type));
        Assert.That(caseListDoneAt[2].CaseUId, Is.EqualTo(aCase3.CaseUid));
        Assert.That(caseListDoneAt[2].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
        Assert.That(caseListDoneAt[2].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
        Assert.That(caseListDoneAt[2].Custom, Is.EqualTo(aCase3.Custom));
        Assert.That(caseListDoneAt[2].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
        Assert.That(caseListDoneAt[2].Id, Is.EqualTo(aCase3.Id));
        Assert.That(caseListDoneAt[2].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
        Assert.That(caseListDoneAt[2].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
        Assert.That(caseListDoneAt[2].SiteName, Is.EqualTo(aCase3.Site.Name));
        Assert.That(caseListDoneAt[2].Status, Is.EqualTo(aCase3.Status));
        Assert.That(caseListDoneAt[2].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListDoneAt[2].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
        //            Assert.AreEqual(c3_ua.ToString(), caseListDoneAt[2].UpdatedAt.ToString());
        Assert.That(caseListDoneAt[2].Version, Is.EqualTo(aCase3.Version));
        Assert.That(caseListDoneAt[2].WorkerName, Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
        Assert.That(caseListDoneAt[2].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

        #endregion

        #region caseListDoneAt aCase4

        Assert.That(caseListDoneAt[3].CaseType, Is.EqualTo(aCase4.Type));
        Assert.That(caseListDoneAt[3].CaseUId, Is.EqualTo(aCase4.CaseUid));
        Assert.That(caseListDoneAt[3].CheckUIid, Is.EqualTo(aCase4.MicrotingCheckUid));
        Assert.That(caseListDoneAt[3].CreatedAt.ToString(), Is.EqualTo(c4_ca.ToString()));
        Assert.That(caseListDoneAt[3].Custom, Is.EqualTo(aCase4.Custom));
        Assert.That(caseListDoneAt[3].DoneAt.ToString(), Is.EqualTo(c4_da.ToString()));
        Assert.That(caseListDoneAt[3].Id, Is.EqualTo(aCase4.Id));
        Assert.That(caseListDoneAt[3].MicrotingUId, Is.EqualTo(aCase4.MicrotingUid));
        Assert.That(caseListDoneAt[3].SiteId, Is.EqualTo(aCase4.Site.MicrotingUid));
        Assert.That(caseListDoneAt[3].SiteName, Is.EqualTo(aCase4.Site.Name));
        Assert.That(caseListDoneAt[3].Status, Is.EqualTo(aCase4.Status));
        Assert.That(caseListDoneAt[3].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListDoneAt[3].UnitId, Is.EqualTo(aCase4.Unit.MicrotingUid));
        //            Assert.AreEqual(c4_ua.ToString(), caseListDoneAt[3].UpdatedAt.ToString());
        Assert.That(caseListDoneAt[3].Version, Is.EqualTo(aCase4.Version));
        Assert.That(caseListDoneAt[3].WorkerName, Is.EqualTo(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName));
        Assert.That(caseListDoneAt[3].WorkflowState, Is.EqualTo(aCase4.WorkflowState));

        #endregion

        #endregion

        #region caseListStatus Def Sort Asc

        #region caseListStatus aCase1

        Assert.That(caseListStatus, Is.Not.EqualTo(null));
        Assert.That(caseListStatus.Count, Is.EqualTo(4));
        Assert.That(caseListStatus[0].CaseType, Is.EqualTo(aCase1.Type));
        Assert.That(caseListStatus[0].CaseUId, Is.EqualTo(aCase1.CaseUid));
        Assert.That(caseListStatus[0].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
        Assert.That(caseListStatus[0].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
        Assert.That(caseListStatus[0].Custom, Is.EqualTo(aCase1.Custom));
        Assert.That(caseListStatus[0].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
        Assert.That(caseListStatus[0].Id, Is.EqualTo(aCase1.Id));
        Assert.That(caseListStatus[0].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
        Assert.That(caseListStatus[0].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
        Assert.That(caseListStatus[0].SiteName, Is.EqualTo(aCase1.Site.Name));
        Assert.That(caseListStatus[0].Status, Is.EqualTo(aCase1.Status));
        Assert.That(caseListStatus[0].TemplatId, Is.EqualTo(aCase1.CheckListId));
        Assert.That(caseListStatus[0].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
        //            Assert.AreEqual(c1_ua.ToString(), caseListStatus[0].UpdatedAt.ToString());
        Assert.That(caseListStatus[0].Version, Is.EqualTo(aCase1.Version));
        Assert.That(caseListStatus[0].WorkerName, Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
        Assert.That(caseListStatus[0].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

        #endregion

        #region caseListStatus aCase2

        Assert.That(caseListStatus[1].CaseType, Is.EqualTo(aCase2.Type));
        Assert.That(caseListStatus[1].CaseUId, Is.EqualTo(aCase2.CaseUid));
        Assert.That(caseListStatus[1].CheckUIid, Is.EqualTo(aCase2.MicrotingCheckUid));
        Assert.That(caseListStatus[1].CreatedAt.ToString(), Is.EqualTo(c2_ca.ToString()));
        Assert.That(caseListStatus[1].Custom, Is.EqualTo(aCase2.Custom));
        Assert.That(caseListStatus[1].DoneAt.ToString(), Is.EqualTo(c2_da.ToString()));
        Assert.That(caseListStatus[1].Id, Is.EqualTo(aCase2.Id));
        Assert.That(caseListStatus[1].MicrotingUId, Is.EqualTo(aCase2.MicrotingUid));
        Assert.That(caseListStatus[1].SiteId, Is.EqualTo(aCase2.Site.MicrotingUid));
        Assert.That(caseListStatus[1].SiteName, Is.EqualTo(aCase2.Site.Name));
        Assert.That(caseListStatus[1].Status, Is.EqualTo(aCase2.Status));
        Assert.That(caseListStatus[1].TemplatId, Is.EqualTo(aCase2.CheckListId));
        Assert.That(caseListStatus[1].UnitId, Is.EqualTo(aCase2.Unit.MicrotingUid));
        //            Assert.AreEqual(c2_ua.ToString(), caseListStatus[1].UpdatedAt.ToString());
        Assert.That(caseListStatus[1].Version, Is.EqualTo(aCase2.Version));
        Assert.That(caseListStatus[1].WorkerName, Is.EqualTo(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName));
        Assert.That(caseListStatus[1].WorkflowState, Is.EqualTo(aCase2.WorkflowState));

        #endregion

        #region caseListStatus aCase3

        Assert.That(caseListStatus[2].CaseType, Is.EqualTo(aCase3.Type));
        Assert.That(caseListStatus[2].CaseUId, Is.EqualTo(aCase3.CaseUid));
        Assert.That(caseListStatus[2].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
        Assert.That(caseListStatus[2].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
        Assert.That(caseListStatus[2].Custom, Is.EqualTo(aCase3.Custom));
        Assert.That(caseListStatus[2].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
        Assert.That(caseListStatus[2].Id, Is.EqualTo(aCase3.Id));
        Assert.That(caseListStatus[2].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
        Assert.That(caseListStatus[2].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
        Assert.That(caseListStatus[2].SiteName, Is.EqualTo(aCase3.Site.Name));
        Assert.That(caseListStatus[2].Status, Is.EqualTo(aCase3.Status));
        Assert.That(caseListStatus[2].TemplatId, Is.EqualTo(aCase3.CheckListId));
        Assert.That(caseListStatus[2].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
        //            Assert.AreEqual(c3_ua.ToString(), caseListStatus[2].UpdatedAt.ToString());
        Assert.That(caseListStatus[2].Version, Is.EqualTo(aCase3.Version));
        Assert.That(caseListStatus[2].WorkerName, Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
        Assert.That(caseListStatus[2].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

        #endregion

        #region caseListStatus aCase4

        Assert.That(caseListStatus[3].CaseType, Is.EqualTo(aCase4.Type));
        Assert.That(caseListStatus[3].CaseUId, Is.EqualTo(aCase4.CaseUid));
        Assert.That(caseListStatus[3].CheckUIid, Is.EqualTo(aCase4.MicrotingCheckUid));
        Assert.That(caseListStatus[3].CreatedAt.ToString(), Is.EqualTo(c4_ca.ToString()));
        Assert.That(caseListStatus[3].Custom, Is.EqualTo(aCase4.Custom));
        Assert.That(caseListStatus[3].DoneAt.ToString(), Is.EqualTo(c4_da.ToString()));
        Assert.That(caseListStatus[3].Id, Is.EqualTo(aCase4.Id));
        Assert.That(caseListStatus[3].MicrotingUId, Is.EqualTo(aCase4.MicrotingUid));
        Assert.That(caseListStatus[3].SiteId, Is.EqualTo(aCase4.Site.MicrotingUid));
        Assert.That(caseListStatus[3].SiteName, Is.EqualTo(aCase4.Site.Name));
        Assert.That(caseListStatus[3].Status, Is.EqualTo(aCase4.Status));
        Assert.That(caseListStatus[3].TemplatId, Is.EqualTo(aCase4.CheckListId));
        Assert.That(caseListStatus[3].UnitId, Is.EqualTo(aCase4.Unit.MicrotingUid));
        //            Assert.AreEqual(c4_ua.ToString(), caseListStatus[3].UpdatedAt.ToString());
        Assert.That(caseListStatus[3].Version, Is.EqualTo(aCase4.Version));
        Assert.That(caseListStatus[3].WorkerName, Is.EqualTo(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName));
        Assert.That(caseListStatus[3].WorkflowState, Is.EqualTo(aCase4.WorkflowState));

        #endregion

        #endregion

        #region caseListUnitId Def Sort Asc

        #region caseListUnitId aCase1

        Assert.That(caseListUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListUnitId.Count, Is.EqualTo(4));
        Assert.That(caseListUnitId[0].CaseType, Is.EqualTo(aCase1.Type));
        Assert.That(caseListUnitId[0].CaseUId, Is.EqualTo(aCase1.CaseUid));
        Assert.That(caseListUnitId[0].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
        Assert.That(caseListUnitId[0].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
        Assert.That(caseListUnitId[0].Custom, Is.EqualTo(aCase1.Custom));
        Assert.That(caseListUnitId[0].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
        Assert.That(caseListUnitId[0].Id, Is.EqualTo(aCase1.Id));
        Assert.That(caseListUnitId[0].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
        Assert.That(caseListUnitId[0].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
        Assert.That(caseListUnitId[0].SiteName, Is.EqualTo(aCase1.Site.Name));
        Assert.That(caseListUnitId[0].Status, Is.EqualTo(aCase1.Status));
        Assert.That(caseListUnitId[0].TemplatId, Is.EqualTo(aCase1.CheckListId));
        Assert.That(caseListUnitId[0].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
        //            Assert.AreEqual(c1_ua.ToString(), caseListUnitId[0].UpdatedAt.ToString());
        Assert.That(caseListUnitId[0].Version, Is.EqualTo(aCase1.Version));
        Assert.That(caseListUnitId[0].WorkerName, Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
        Assert.That(caseListUnitId[0].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

        #endregion

        #region caseListUnitId aCase2

        Assert.That(caseListUnitId[1].CaseType, Is.EqualTo(aCase2.Type));
        Assert.That(caseListUnitId[1].CaseUId, Is.EqualTo(aCase2.CaseUid));
        Assert.That(caseListUnitId[1].CheckUIid, Is.EqualTo(aCase2.MicrotingCheckUid));
        Assert.That(caseListUnitId[1].CreatedAt.ToString(), Is.EqualTo(c2_ca.ToString()));
        Assert.That(caseListUnitId[1].Custom, Is.EqualTo(aCase2.Custom));
        Assert.That(caseListUnitId[1].DoneAt.ToString(), Is.EqualTo(c2_da.ToString()));
        Assert.That(caseListUnitId[1].Id, Is.EqualTo(aCase2.Id));
        Assert.That(caseListUnitId[1].MicrotingUId, Is.EqualTo(aCase2.MicrotingUid));
        Assert.That(caseListUnitId[1].SiteId, Is.EqualTo(aCase2.Site.MicrotingUid));
        Assert.That(caseListUnitId[1].SiteName, Is.EqualTo(aCase2.Site.Name));
        Assert.That(caseListUnitId[1].Status, Is.EqualTo(aCase2.Status));
        Assert.That(caseListUnitId[1].TemplatId, Is.EqualTo(aCase2.CheckListId));
        Assert.That(caseListUnitId[1].UnitId, Is.EqualTo(aCase2.Unit.MicrotingUid));
        //            Assert.AreEqual(c2_ua.ToString(), caseListUnitId[1].UpdatedAt.ToString());
        Assert.That(caseListUnitId[1].Version, Is.EqualTo(aCase2.Version));
        Assert.That(caseListUnitId[1].WorkerName, Is.EqualTo(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName));
        Assert.That(caseListUnitId[1].WorkflowState, Is.EqualTo(aCase2.WorkflowState));

        #endregion

        #region caseListUnitId aCase3

        Assert.That(caseListUnitId[2].CaseType, Is.EqualTo(aCase3.Type));
        Assert.That(caseListUnitId[2].CaseUId, Is.EqualTo(aCase3.CaseUid));
        Assert.That(caseListUnitId[2].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
        Assert.That(caseListUnitId[2].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
        Assert.That(caseListUnitId[2].Custom, Is.EqualTo(aCase3.Custom));
        Assert.That(caseListUnitId[2].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
        Assert.That(caseListUnitId[2].Id, Is.EqualTo(aCase3.Id));
        Assert.That(caseListUnitId[2].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
        Assert.That(caseListUnitId[2].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
        Assert.That(caseListUnitId[2].SiteName, Is.EqualTo(aCase3.Site.Name));
        Assert.That(caseListUnitId[2].Status, Is.EqualTo(aCase3.Status));
        Assert.That(caseListUnitId[2].TemplatId, Is.EqualTo(aCase3.CheckListId));
        Assert.That(caseListUnitId[2].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
        //            Assert.AreEqual(c3_ua.ToString(), caseListUnitId[2].UpdatedAt.ToString());
        Assert.That(caseListUnitId[2].Version, Is.EqualTo(aCase3.Version));
        Assert.That(caseListUnitId[2].WorkerName, Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
        Assert.That(caseListUnitId[2].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

        #endregion

        #region caseListUnitId aCase4

        Assert.That(caseListUnitId[3].CaseType, Is.EqualTo(aCase4.Type));
        Assert.That(caseListUnitId[3].CaseUId, Is.EqualTo(aCase4.CaseUid));
        Assert.That(caseListUnitId[3].CheckUIid, Is.EqualTo(aCase4.MicrotingCheckUid));
        Assert.That(caseListUnitId[3].CreatedAt.ToString(), Is.EqualTo(c4_ca.ToString()));
        Assert.That(caseListUnitId[3].Custom, Is.EqualTo(aCase4.Custom));
        Assert.That(caseListUnitId[3].DoneAt.ToString(), Is.EqualTo(c4_da.ToString()));
        Assert.That(caseListUnitId[3].Id, Is.EqualTo(aCase4.Id));
        Assert.That(caseListUnitId[3].MicrotingUId, Is.EqualTo(aCase4.MicrotingUid));
        Assert.That(caseListUnitId[3].SiteId, Is.EqualTo(aCase4.Site.MicrotingUid));
        Assert.That(caseListUnitId[3].SiteName, Is.EqualTo(aCase4.Site.Name));
        Assert.That(caseListUnitId[3].Status, Is.EqualTo(aCase4.Status));
        Assert.That(caseListUnitId[3].TemplatId, Is.EqualTo(aCase4.CheckListId));
        Assert.That(caseListUnitId[3].UnitId, Is.EqualTo(aCase4.Unit.MicrotingUid));
        //            Assert.AreEqual(c4_ua.ToString(), caseListUnitId[3].UpdatedAt.ToString());
        Assert.That(caseListUnitId[3].Version, Is.EqualTo(aCase4.Version));
        Assert.That(caseListUnitId[3].WorkerName, Is.EqualTo(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName));
        Assert.That(caseListUnitId[3].WorkflowState, Is.EqualTo(aCase4.WorkflowState));

        #endregion

        #endregion

        #endregion


        #region Def Sort Asc w. DateTime

        #region caseListDtDoneAt Def Sort Asc w. DateTime

        #region caseListDtDoneAt aCase1

        Assert.That(caseListDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListDtDoneAt.Count, Is.EqualTo(2));
        Assert.That(caseListDtDoneAt[0].CaseType, Is.EqualTo(aCase1.Type));
        Assert.That(caseListDtDoneAt[0].CaseUId, Is.EqualTo(aCase1.CaseUid));
        Assert.That(caseListDtDoneAt[0].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
        Assert.That(caseListDtDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
        Assert.That(caseListDtDoneAt[0].Custom, Is.EqualTo(aCase1.Custom));
        Assert.That(caseListDtDoneAt[0].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
        Assert.That(caseListDtDoneAt[0].Id, Is.EqualTo(aCase1.Id));
        Assert.That(caseListDtDoneAt[0].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
        Assert.That(caseListDtDoneAt[0].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
        Assert.That(caseListDtDoneAt[0].SiteName, Is.EqualTo(aCase1.Site.Name));
        Assert.That(caseListDtDoneAt[0].Status, Is.EqualTo(aCase1.Status));
        Assert.That(caseListDtDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListDtDoneAt[0].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
        //            Assert.AreEqual(c1_ua.ToString(), caseListDtDoneAt[0].UpdatedAt.ToString());
        Assert.That(caseListDtDoneAt[0].Version, Is.EqualTo(aCase1.Version));
        Assert.That(caseListDtDoneAt[0].WorkerName, Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
        Assert.That(caseListDtDoneAt[0].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

        #endregion

        #region caseListDtDoneAt aCase3

        Assert.That(caseListDtDoneAt[1].CaseType, Is.EqualTo(aCase3.Type));
        Assert.That(caseListDtDoneAt[1].CaseUId, Is.EqualTo(aCase3.CaseUid));
        Assert.That(caseListDtDoneAt[1].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
        Assert.That(caseListDtDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
        Assert.That(caseListDtDoneAt[1].Custom, Is.EqualTo(aCase3.Custom));
        Assert.That(caseListDtDoneAt[1].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
        Assert.That(caseListDtDoneAt[1].Id, Is.EqualTo(aCase3.Id));
        Assert.That(caseListDtDoneAt[1].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
        Assert.That(caseListDtDoneAt[1].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
        Assert.That(caseListDtDoneAt[1].SiteName, Is.EqualTo(aCase3.Site.Name));
        Assert.That(caseListDtDoneAt[1].Status, Is.EqualTo(aCase3.Status));
        Assert.That(caseListDtDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListDtDoneAt[1].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
        //            Assert.AreEqual(c3_ua.ToString(), caseListDtDoneAt[1].UpdatedAt.ToString());
        Assert.That(caseListDtDoneAt[1].Version, Is.EqualTo(aCase3.Version));
        Assert.That(caseListDtDoneAt[1].WorkerName, Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
        Assert.That(caseListDtDoneAt[1].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

        #endregion

        #endregion

        #region caseListDtStatus Def Sort Asc w. DateTime

        #region caseListDtStatus aCase1

        Assert.That(caseListDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListDtStatus.Count, Is.EqualTo(2));
        Assert.That(caseListDtStatus[0].CaseType, Is.EqualTo(aCase1.Type));
        Assert.That(caseListDtStatus[0].CaseUId, Is.EqualTo(aCase1.CaseUid));
        Assert.That(caseListDtStatus[0].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
        Assert.That(caseListDtStatus[0].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
        Assert.That(caseListDtStatus[0].Custom, Is.EqualTo(aCase1.Custom));
        Assert.That(caseListDtStatus[0].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
        Assert.That(caseListDtStatus[0].Id, Is.EqualTo(aCase1.Id));
        Assert.That(caseListDtStatus[0].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
        Assert.That(caseListDtStatus[0].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
        Assert.That(caseListDtStatus[0].SiteName, Is.EqualTo(aCase1.Site.Name));
        Assert.That(caseListDtStatus[0].Status, Is.EqualTo(aCase1.Status));
        Assert.That(caseListDtStatus[0].TemplatId, Is.EqualTo(aCase1.CheckListId));
        Assert.That(caseListDtStatus[0].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
        //            Assert.AreEqual(c1_ua.ToString(), caseListDtStatus[0].UpdatedAt.ToString());
        Assert.That(caseListDtStatus[0].Version, Is.EqualTo(aCase1.Version));
        Assert.That(caseListDtStatus[0].WorkerName, Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
        Assert.That(caseListDtStatus[0].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

        #endregion

        #region caseListDtStatus aCase3

        Assert.That(caseListDtStatus[1].CaseType, Is.EqualTo(aCase3.Type));
        Assert.That(caseListDtStatus[1].CaseUId, Is.EqualTo(aCase3.CaseUid));
        Assert.That(caseListDtStatus[1].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
        Assert.That(caseListDtStatus[1].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
        Assert.That(caseListDtStatus[1].Custom, Is.EqualTo(aCase3.Custom));
        Assert.That(caseListDtStatus[1].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
        Assert.That(caseListDtStatus[1].Id, Is.EqualTo(aCase3.Id));
        Assert.That(caseListDtStatus[1].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
        Assert.That(caseListDtStatus[1].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
        Assert.That(caseListDtStatus[1].SiteName, Is.EqualTo(aCase3.Site.Name));
        Assert.That(caseListDtStatus[1].Status, Is.EqualTo(aCase3.Status));
        Assert.That(caseListDtStatus[1].TemplatId, Is.EqualTo(aCase3.CheckListId));
        Assert.That(caseListDtStatus[1].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
        //            Assert.AreEqual(c3_ua.ToString(), caseListDtStatus[1].UpdatedAt.ToString());
        Assert.That(caseListDtStatus[1].Version, Is.EqualTo(aCase3.Version));
        Assert.That(caseListDtStatus[1].WorkerName, Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
        Assert.That(caseListDtStatus[1].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

        #endregion

        #endregion

        #region caseListDtUnitId Def Sort Asc w. DateTime

        #region caseListDtUnitId aCase1

        Assert.That(caseListDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListDtUnitId.Count, Is.EqualTo(2));
        Assert.That(caseListDtUnitId[0].CaseType, Is.EqualTo(aCase1.Type));
        Assert.That(caseListDtUnitId[0].CaseUId, Is.EqualTo(aCase1.CaseUid));
        Assert.That(caseListDtUnitId[0].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
        Assert.That(caseListDtUnitId[0].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
        Assert.That(caseListDtUnitId[0].Custom, Is.EqualTo(aCase1.Custom));
        Assert.That(caseListDtUnitId[0].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
        Assert.That(caseListDtUnitId[0].Id, Is.EqualTo(aCase1.Id));
        Assert.That(caseListDtUnitId[0].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
        Assert.That(caseListDtUnitId[0].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
        Assert.That(caseListDtUnitId[0].SiteName, Is.EqualTo(aCase1.Site.Name));
        Assert.That(caseListDtUnitId[0].Status, Is.EqualTo(aCase1.Status));
        Assert.That(caseListDtUnitId[0].TemplatId, Is.EqualTo(aCase1.CheckListId));
        Assert.That(caseListDtUnitId[0].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
        //            Assert.AreEqual(c1_ua.ToString(), caseListDtUnitId[0].UpdatedAt.ToString());
        Assert.That(caseListDtUnitId[0].Version, Is.EqualTo(aCase1.Version));
        Assert.That(caseListDtUnitId[0].WorkerName, Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
        Assert.That(caseListDtUnitId[0].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

        #endregion

        #region caseListDtUnitId aCase3

        Assert.That(caseListDtUnitId[1].CaseType, Is.EqualTo(aCase3.Type));
        Assert.That(caseListDtUnitId[1].CaseUId, Is.EqualTo(aCase3.CaseUid));
        Assert.That(caseListDtUnitId[1].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
        Assert.That(caseListDtUnitId[1].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
        Assert.That(caseListDtUnitId[1].Custom, Is.EqualTo(aCase3.Custom));
        Assert.That(caseListDtUnitId[1].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
        Assert.That(caseListDtUnitId[1].Id, Is.EqualTo(aCase3.Id));
        Assert.That(caseListDtUnitId[1].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
        Assert.That(caseListDtUnitId[1].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
        Assert.That(caseListDtUnitId[1].SiteName, Is.EqualTo(aCase3.Site.Name));
        Assert.That(caseListDtUnitId[1].Status, Is.EqualTo(aCase3.Status));
        Assert.That(caseListDtUnitId[1].TemplatId, Is.EqualTo(aCase3.CheckListId));
        Assert.That(caseListDtUnitId[1].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
        //            Assert.AreEqual(c3_ua.ToString(), caseListDtUnitId[1].UpdatedAt.ToString());
        Assert.That(caseListDtUnitId[1].Version, Is.EqualTo(aCase3.Version));
        Assert.That(caseListDtUnitId[1].WorkerName, Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
        Assert.That(caseListDtUnitId[1].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

        #endregion

        #endregion

        #endregion

        #endregion

        #region Case Sort

        #region aCase Sort Asc

        #region aCase1 sort asc

        #region caseListC1DoneAt aCase1

        Assert.That(caseListC1SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListC1SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListC1SortStatus, Is.Not.EqualTo(null));
        Assert.That(caseListC1SortStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListC1SortUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListC1SortUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListC2SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListC2SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListC2SortStatus, Is.Not.EqualTo(null));
        Assert.That(caseListC2SortStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListC2SortUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListC2SortUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListC3SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListC3SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListC3SortStatus, Is.Not.EqualTo(null));
        Assert.That(caseListC3SortStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListC3SortUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListC3SortUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListC4SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListC4SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListC1SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListC1SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListC4SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListC4SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListC1SortDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListC1SortDtDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListC1SortDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListC1SortDtStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListC1SortDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListC1SortDtUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListC2SortDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListC2SortDtDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListC2SortDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListC2SortDtStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListC2SortDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListC2SortDtUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListC3SortDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListC3SortDtDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListC3SortDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListC3SortDtStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListC3SortDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListC3SortDtUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListC4SortDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListC4SortDtDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListC4SortDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListC4SortDtStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListC4SortDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListC4SortDtUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedDoneAt.Count, Is.EqualTo(4));
        Assert.That(caseListRemovedDoneAt[0].CaseType, Is.EqualTo(aCase1Removed.Type));
        Assert.That(caseListRemovedDoneAt[0].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
        Assert.That(caseListRemovedDoneAt[0].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
        Assert.That(caseListRemovedDoneAt[0].Custom, Is.EqualTo(aCase1Removed.Custom));
        Assert.That(caseListRemovedDoneAt[0].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
        Assert.That(caseListRemovedDoneAt[0].Id, Is.EqualTo(aCase1Removed.Id));
        Assert.That(caseListRemovedDoneAt[0].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
        Assert.That(caseListRemovedDoneAt[0].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedDoneAt[0].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
        Assert.That(caseListRemovedDoneAt[0].Status, Is.EqualTo(aCase1Removed.Status));
        Assert.That(caseListRemovedDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListRemovedDoneAt[0].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDoneAt[0].UpdatedAt.ToString());
        Assert.That(caseListRemovedDoneAt[0].Version, Is.EqualTo(aCase1Removed.Version));
        Assert.That(caseListRemovedDoneAt[0].WorkerName,
            Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
        Assert.That(caseListRemovedDoneAt[0].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

        #endregion

        #region caseListRemovedDoneAt aCase2Removed

        Assert.That(caseListRemovedDoneAt[1].CaseType, Is.EqualTo(aCase2Removed.Type));
        Assert.That(caseListRemovedDoneAt[1].CaseUId, Is.EqualTo(aCase2Removed.CaseUid));
        Assert.That(caseListRemovedDoneAt[1].CheckUIid, Is.EqualTo(aCase2Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c2Removed_ca.ToString()));
        Assert.That(caseListRemovedDoneAt[1].Custom, Is.EqualTo(aCase2Removed.Custom));
        Assert.That(caseListRemovedDoneAt[1].DoneAt.ToString(), Is.EqualTo(c2Removed_da.ToString()));
        Assert.That(caseListRemovedDoneAt[1].Id, Is.EqualTo(aCase2Removed.Id));
        Assert.That(caseListRemovedDoneAt[1].MicrotingUId, Is.EqualTo(aCase2Removed.MicrotingUid));
        Assert.That(caseListRemovedDoneAt[1].SiteId, Is.EqualTo(aCase2Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedDoneAt[1].SiteName, Is.EqualTo(aCase2Removed.Site.Name));
        Assert.That(caseListRemovedDoneAt[1].Status, Is.EqualTo(aCase2Removed.Status));
        Assert.That(caseListRemovedDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListRemovedDoneAt[1].UnitId, Is.EqualTo(aCase2Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedDoneAt[1].UpdatedAt.ToString());
        Assert.That(caseListRemovedDoneAt[1].Version, Is.EqualTo(aCase2Removed.Version));
        Assert.That(caseListRemovedDoneAt[1].WorkerName,
            Is.EqualTo(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName));
        Assert.That(caseListRemovedDoneAt[1].WorkflowState, Is.EqualTo(aCase2Removed.WorkflowState));

        #endregion

        #region caseListRemovedDoneAt aCase3Removed

        Assert.That(caseListRemovedDoneAt[2].CaseType, Is.EqualTo(aCase3Removed.Type));
        Assert.That(caseListRemovedDoneAt[2].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
        Assert.That(caseListRemovedDoneAt[2].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedDoneAt[2].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
        Assert.That(caseListRemovedDoneAt[2].Custom, Is.EqualTo(aCase3Removed.Custom));
        Assert.That(caseListRemovedDoneAt[2].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
        Assert.That(caseListRemovedDoneAt[2].Id, Is.EqualTo(aCase3Removed.Id));
        Assert.That(caseListRemovedDoneAt[2].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
        Assert.That(caseListRemovedDoneAt[2].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedDoneAt[2].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
        Assert.That(caseListRemovedDoneAt[2].Status, Is.EqualTo(aCase3Removed.Status));
        Assert.That(caseListRemovedDoneAt[2].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListRemovedDoneAt[2].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDoneAt[2].UpdatedAt.ToString());
        Assert.That(caseListRemovedDoneAt[2].Version, Is.EqualTo(aCase3Removed.Version));
        Assert.That(caseListRemovedDoneAt[2].WorkerName,
            Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
        Assert.That(caseListRemovedDoneAt[2].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

        #endregion

        #region caseListRemovedDoneAt aCase4Removed

        Assert.That(caseListRemovedDoneAt[3].CaseType, Is.EqualTo(aCase4Removed.Type));
        Assert.That(caseListRemovedDoneAt[3].CaseUId, Is.EqualTo(aCase4Removed.CaseUid));
        Assert.That(caseListRemovedDoneAt[3].CheckUIid, Is.EqualTo(aCase4Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedDoneAt[3].CreatedAt.ToString(), Is.EqualTo(c4Removed_ca.ToString()));
        Assert.That(caseListRemovedDoneAt[3].Custom, Is.EqualTo(aCase4Removed.Custom));
        Assert.That(caseListRemovedDoneAt[3].DoneAt.ToString(), Is.EqualTo(c4Removed_da.ToString()));
        Assert.That(caseListRemovedDoneAt[3].Id, Is.EqualTo(aCase4Removed.Id));
        Assert.That(caseListRemovedDoneAt[3].MicrotingUId, Is.EqualTo(aCase4Removed.MicrotingUid));
        Assert.That(caseListRemovedDoneAt[3].SiteId, Is.EqualTo(aCase4Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedDoneAt[3].SiteName, Is.EqualTo(aCase4Removed.Site.Name));
        Assert.That(caseListRemovedDoneAt[3].Status, Is.EqualTo(aCase4Removed.Status));
        Assert.That(caseListRemovedDoneAt[3].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListRemovedDoneAt[3].UnitId, Is.EqualTo(aCase4Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedDoneAt[3].UpdatedAt.ToString());
        Assert.That(caseListRemovedDoneAt[3].Version, Is.EqualTo(aCase4Removed.Version));
        Assert.That(caseListRemovedDoneAt[3].WorkerName,
            Is.EqualTo(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName));
        Assert.That(caseListRemovedDoneAt[3].WorkflowState, Is.EqualTo(aCase4Removed.WorkflowState));

        #endregion

        #endregion

        #region caseListRemovedStatus Def Sort Asc

        #region caseListRemovedStatus aCase1Removed

        Assert.That(caseListRemovedStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedStatus.Count, Is.EqualTo(4));
        Assert.That(caseListRemovedStatus[0].CaseType, Is.EqualTo(aCase1Removed.Type));
        Assert.That(caseListRemovedStatus[0].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
        Assert.That(caseListRemovedStatus[0].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedStatus[0].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
        Assert.That(caseListRemovedStatus[0].Custom, Is.EqualTo(aCase1Removed.Custom));
        Assert.That(caseListRemovedStatus[0].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
        Assert.That(caseListRemovedStatus[0].Id, Is.EqualTo(aCase1Removed.Id));
        Assert.That(caseListRemovedStatus[0].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
        Assert.That(caseListRemovedStatus[0].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedStatus[0].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
        Assert.That(caseListRemovedStatus[0].Status, Is.EqualTo(aCase1Removed.Status));
        Assert.That(caseListRemovedStatus[0].TemplatId, Is.EqualTo(aCase1Removed.CheckListId));
        Assert.That(caseListRemovedStatus[0].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedStatus[0].UpdatedAt.ToString());
        Assert.That(caseListRemovedStatus[0].Version, Is.EqualTo(aCase1Removed.Version));
        Assert.That(caseListRemovedStatus[0].WorkerName,
            Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
        Assert.That(caseListRemovedStatus[0].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

        #endregion

        #region caseListRemovedStatus aCase2Removed

        Assert.That(caseListRemovedStatus[1].CaseType, Is.EqualTo(aCase2Removed.Type));
        Assert.That(caseListRemovedStatus[1].CaseUId, Is.EqualTo(aCase2Removed.CaseUid));
        Assert.That(caseListRemovedStatus[1].CheckUIid, Is.EqualTo(aCase2Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedStatus[1].CreatedAt.ToString(), Is.EqualTo(c2Removed_ca.ToString()));
        Assert.That(caseListRemovedStatus[1].Custom, Is.EqualTo(aCase2Removed.Custom));
        Assert.That(caseListRemovedStatus[1].DoneAt.ToString(), Is.EqualTo(c2Removed_da.ToString()));
        Assert.That(caseListRemovedStatus[1].Id, Is.EqualTo(aCase2Removed.Id));
        Assert.That(caseListRemovedStatus[1].MicrotingUId, Is.EqualTo(aCase2Removed.MicrotingUid));
        Assert.That(caseListRemovedStatus[1].SiteId, Is.EqualTo(aCase2Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedStatus[1].SiteName, Is.EqualTo(aCase2Removed.Site.Name));
        Assert.That(caseListRemovedStatus[1].Status, Is.EqualTo(aCase2Removed.Status));
        Assert.That(caseListRemovedStatus[1].TemplatId, Is.EqualTo(aCase2Removed.CheckListId));
        Assert.That(caseListRemovedStatus[1].UnitId, Is.EqualTo(aCase2Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedStatus[1].UpdatedAt.ToString());
        Assert.That(caseListRemovedStatus[1].Version, Is.EqualTo(aCase2Removed.Version));
        Assert.That(caseListRemovedStatus[1].WorkerName,
            Is.EqualTo(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName));
        Assert.That(caseListRemovedStatus[1].WorkflowState, Is.EqualTo(aCase2Removed.WorkflowState));

        #endregion

        #region caseListRemovedStatus aCase3Removed

        Assert.That(caseListRemovedStatus[2].CaseType, Is.EqualTo(aCase3Removed.Type));
        Assert.That(caseListRemovedStatus[2].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
        Assert.That(caseListRemovedStatus[2].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedStatus[2].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
        Assert.That(caseListRemovedStatus[2].Custom, Is.EqualTo(aCase3Removed.Custom));
        Assert.That(caseListRemovedStatus[2].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
        Assert.That(caseListRemovedStatus[2].Id, Is.EqualTo(aCase3Removed.Id));
        Assert.That(caseListRemovedStatus[2].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
        Assert.That(caseListRemovedStatus[2].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedStatus[2].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
        Assert.That(caseListRemovedStatus[2].Status, Is.EqualTo(aCase3Removed.Status));
        Assert.That(caseListRemovedStatus[2].TemplatId, Is.EqualTo(aCase3Removed.CheckListId));
        Assert.That(caseListRemovedStatus[2].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedStatus[2].UpdatedAt.ToString());
        Assert.That(caseListRemovedStatus[2].Version, Is.EqualTo(aCase3Removed.Version));
        Assert.That(caseListRemovedStatus[2].WorkerName,
            Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
        Assert.That(caseListRemovedStatus[2].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

        #endregion

        #region caseListRemovedStatus aCase4Removed

        Assert.That(caseListRemovedStatus[3].CaseType, Is.EqualTo(aCase4Removed.Type));
        Assert.That(caseListRemovedStatus[3].CaseUId, Is.EqualTo(aCase4Removed.CaseUid));
        Assert.That(caseListRemovedStatus[3].CheckUIid, Is.EqualTo(aCase4Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedStatus[3].CreatedAt.ToString(), Is.EqualTo(c4Removed_ca.ToString()));
        Assert.That(caseListRemovedStatus[3].Custom, Is.EqualTo(aCase4Removed.Custom));
        Assert.That(caseListRemovedStatus[3].DoneAt.ToString(), Is.EqualTo(c4Removed_da.ToString()));
        Assert.That(caseListRemovedStatus[3].Id, Is.EqualTo(aCase4Removed.Id));
        Assert.That(caseListRemovedStatus[3].MicrotingUId, Is.EqualTo(aCase4Removed.MicrotingUid));
        Assert.That(caseListRemovedStatus[3].SiteId, Is.EqualTo(aCase4Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedStatus[3].SiteName, Is.EqualTo(aCase4Removed.Site.Name));
        Assert.That(caseListRemovedStatus[3].Status, Is.EqualTo(aCase4Removed.Status));
        Assert.That(caseListRemovedStatus[3].TemplatId, Is.EqualTo(aCase4Removed.CheckListId));
        Assert.That(caseListRemovedStatus[3].UnitId, Is.EqualTo(aCase4Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedStatus[3].UpdatedAt.ToString());
        Assert.That(caseListRemovedStatus[3].Version, Is.EqualTo(aCase4Removed.Version));
        Assert.That(caseListRemovedStatus[3].WorkerName,
            Is.EqualTo(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName));
        Assert.That(caseListRemovedStatus[3].WorkflowState, Is.EqualTo(aCase4Removed.WorkflowState));

        #endregion

        #endregion

        #region caseListRemovedUnitId Def Sort Asc

        #region caseListRemovedUnitId aCase1Removed

        Assert.That(caseListRemovedUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedUnitId.Count, Is.EqualTo(4));
        Assert.That(caseListRemovedUnitId[0].CaseType, Is.EqualTo(aCase1Removed.Type));
        Assert.That(caseListRemovedUnitId[0].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
        Assert.That(caseListRemovedUnitId[0].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedUnitId[0].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
        Assert.That(caseListRemovedUnitId[0].Custom, Is.EqualTo(aCase1Removed.Custom));
        Assert.That(caseListRemovedUnitId[0].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
        Assert.That(caseListRemovedUnitId[0].Id, Is.EqualTo(aCase1Removed.Id));
        Assert.That(caseListRemovedUnitId[0].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
        Assert.That(caseListRemovedUnitId[0].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedUnitId[0].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
        Assert.That(caseListRemovedUnitId[0].Status, Is.EqualTo(aCase1Removed.Status));
        Assert.That(caseListRemovedUnitId[0].TemplatId, Is.EqualTo(aCase1Removed.CheckListId));
        Assert.That(caseListRemovedUnitId[0].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedUnitId[0].UpdatedAt.ToString());
        Assert.That(caseListRemovedUnitId[0].Version, Is.EqualTo(aCase1Removed.Version));
        Assert.That(caseListRemovedUnitId[0].WorkerName,
            Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
        Assert.That(caseListRemovedUnitId[0].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

        #endregion

        #region caseListRemovedUnitId aCase2Removed

        Assert.That(caseListRemovedUnitId[1].CaseType, Is.EqualTo(aCase2Removed.Type));
        Assert.That(caseListRemovedUnitId[1].CaseUId, Is.EqualTo(aCase2Removed.CaseUid));
        Assert.That(caseListRemovedUnitId[1].CheckUIid, Is.EqualTo(aCase2Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedUnitId[1].CreatedAt.ToString(), Is.EqualTo(c2Removed_ca.ToString()));
        Assert.That(caseListRemovedUnitId[1].Custom, Is.EqualTo(aCase2Removed.Custom));
        Assert.That(caseListRemovedUnitId[1].DoneAt.ToString(), Is.EqualTo(c2Removed_da.ToString()));
        Assert.That(caseListRemovedUnitId[1].Id, Is.EqualTo(aCase2Removed.Id));
        Assert.That(caseListRemovedUnitId[1].MicrotingUId, Is.EqualTo(aCase2Removed.MicrotingUid));
        Assert.That(caseListRemovedUnitId[1].SiteId, Is.EqualTo(aCase2Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedUnitId[1].SiteName, Is.EqualTo(aCase2Removed.Site.Name));
        Assert.That(caseListRemovedUnitId[1].Status, Is.EqualTo(aCase2Removed.Status));
        Assert.That(caseListRemovedUnitId[1].TemplatId, Is.EqualTo(aCase2Removed.CheckListId));
        Assert.That(caseListRemovedUnitId[1].UnitId, Is.EqualTo(aCase2Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedUnitId[1].UpdatedAt.ToString());
        Assert.That(caseListRemovedUnitId[1].Version, Is.EqualTo(aCase2Removed.Version));
        Assert.That(caseListRemovedUnitId[1].WorkerName,
            Is.EqualTo(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName));
        Assert.That(caseListRemovedUnitId[1].WorkflowState, Is.EqualTo(aCase2Removed.WorkflowState));

        #endregion

        #region caseListRemovedUnitId aCase3Removed

        Assert.That(caseListRemovedUnitId[2].CaseType, Is.EqualTo(aCase3Removed.Type));
        Assert.That(caseListRemovedUnitId[2].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
        Assert.That(caseListRemovedUnitId[2].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedUnitId[2].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
        Assert.That(caseListRemovedUnitId[2].Custom, Is.EqualTo(aCase3Removed.Custom));
        Assert.That(caseListRemovedUnitId[2].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
        Assert.That(caseListRemovedUnitId[2].Id, Is.EqualTo(aCase3Removed.Id));
        Assert.That(caseListRemovedUnitId[2].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
        Assert.That(caseListRemovedUnitId[2].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedUnitId[2].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
        Assert.That(caseListRemovedUnitId[2].Status, Is.EqualTo(aCase3Removed.Status));
        Assert.That(caseListRemovedUnitId[2].TemplatId, Is.EqualTo(aCase3Removed.CheckListId));
        Assert.That(caseListRemovedUnitId[2].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedUnitId[2].UpdatedAt.ToString());
        Assert.That(caseListRemovedUnitId[2].Version, Is.EqualTo(aCase3Removed.Version));
        Assert.That(caseListRemovedUnitId[2].WorkerName,
            Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
        Assert.That(caseListRemovedUnitId[2].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

        #endregion

        #region caseListRemovedUnitId aCase4Removed

        Assert.That(caseListRemovedUnitId[3].CaseType, Is.EqualTo(aCase4Removed.Type));
        Assert.That(caseListRemovedUnitId[3].CaseUId, Is.EqualTo(aCase4Removed.CaseUid));
        Assert.That(caseListRemovedUnitId[3].CheckUIid, Is.EqualTo(aCase4Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedUnitId[3].CreatedAt.ToString(), Is.EqualTo(c4Removed_ca.ToString()));
        Assert.That(caseListRemovedUnitId[3].Custom, Is.EqualTo(aCase4Removed.Custom));
        Assert.That(caseListRemovedUnitId[3].DoneAt.ToString(), Is.EqualTo(c4Removed_da.ToString()));
        Assert.That(caseListRemovedUnitId[3].Id, Is.EqualTo(aCase4Removed.Id));
        Assert.That(caseListRemovedUnitId[3].MicrotingUId, Is.EqualTo(aCase4Removed.MicrotingUid));
        Assert.That(caseListRemovedUnitId[3].SiteId, Is.EqualTo(aCase4Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedUnitId[3].SiteName, Is.EqualTo(aCase4Removed.Site.Name));
        Assert.That(caseListRemovedUnitId[3].Status, Is.EqualTo(aCase4Removed.Status));
        Assert.That(caseListRemovedUnitId[3].TemplatId, Is.EqualTo(aCase4Removed.CheckListId));
        Assert.That(caseListRemovedUnitId[3].UnitId, Is.EqualTo(aCase4Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedUnitId[3].UpdatedAt.ToString());
        Assert.That(caseListRemovedUnitId[3].Version, Is.EqualTo(aCase4Removed.Version));
        Assert.That(caseListRemovedUnitId[3].WorkerName,
            Is.EqualTo(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName));
        Assert.That(caseListRemovedUnitId[3].WorkflowState, Is.EqualTo(aCase4Removed.WorkflowState));

        #endregion

        #endregion

        #endregion


        #region Def Sort Asc w. DateTime

        #region caseListRemovedDtDoneAt Def Sort Asc w. DateTime

        #region caseListRemovedDtDoneAt aCase1Removed

        Assert.That(caseListRemovedDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedDtDoneAt.Count, Is.EqualTo(2));
        Assert.That(caseListRemovedDtDoneAt[0].CaseType, Is.EqualTo(aCase1Removed.Type));
        Assert.That(caseListRemovedDtDoneAt[0].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
        Assert.That(caseListRemovedDtDoneAt[0].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedDtDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
        Assert.That(caseListRemovedDtDoneAt[0].Custom, Is.EqualTo(aCase1Removed.Custom));
        Assert.That(caseListRemovedDtDoneAt[0].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
        Assert.That(caseListRemovedDtDoneAt[0].Id, Is.EqualTo(aCase1Removed.Id));
        Assert.That(caseListRemovedDtDoneAt[0].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
        Assert.That(caseListRemovedDtDoneAt[0].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedDtDoneAt[0].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
        Assert.That(caseListRemovedDtDoneAt[0].Status, Is.EqualTo(aCase1Removed.Status));
        Assert.That(caseListRemovedDtDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListRemovedDtDoneAt[0].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtDoneAt[0].UpdatedAt.ToString());
        Assert.That(caseListRemovedDtDoneAt[0].Version, Is.EqualTo(aCase1Removed.Version));
        Assert.That(caseListRemovedDtDoneAt[0].WorkerName,
            Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
        Assert.That(caseListRemovedDtDoneAt[0].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

        #endregion

        #region caseListRemovedDtDoneAt aCase3Removed

        Assert.That(caseListRemovedDtDoneAt[1].CaseType, Is.EqualTo(aCase3Removed.Type));
        Assert.That(caseListRemovedDtDoneAt[1].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
        Assert.That(caseListRemovedDtDoneAt[1].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedDtDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
        Assert.That(caseListRemovedDtDoneAt[1].Custom, Is.EqualTo(aCase3Removed.Custom));
        Assert.That(caseListRemovedDtDoneAt[1].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
        Assert.That(caseListRemovedDtDoneAt[1].Id, Is.EqualTo(aCase3Removed.Id));
        Assert.That(caseListRemovedDtDoneAt[1].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
        Assert.That(caseListRemovedDtDoneAt[1].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedDtDoneAt[1].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
        Assert.That(caseListRemovedDtDoneAt[1].Status, Is.EqualTo(aCase3Removed.Status));
        Assert.That(caseListRemovedDtDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListRemovedDtDoneAt[1].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtDoneAt[1].UpdatedAt.ToString());
        Assert.That(caseListRemovedDtDoneAt[1].Version, Is.EqualTo(aCase3Removed.Version));
        Assert.That(caseListRemovedDtDoneAt[1].WorkerName,
            Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
        Assert.That(caseListRemovedDtDoneAt[1].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

        #endregion

        #endregion

        #region caseListRemovedDtStatus Def Sort Asc w. DateTime

        #region caseListRemovedDtStatus aCase1Removed

        Assert.That(caseListRemovedDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedDtStatus.Count, Is.EqualTo(2));
        Assert.That(caseListRemovedDtStatus[0].CaseType, Is.EqualTo(aCase1Removed.Type));
        Assert.That(caseListRemovedDtStatus[0].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
        Assert.That(caseListRemovedDtStatus[0].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedDtStatus[0].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
        Assert.That(caseListRemovedDtStatus[0].Custom, Is.EqualTo(aCase1Removed.Custom));
        Assert.That(caseListRemovedDtStatus[0].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
        Assert.That(caseListRemovedDtStatus[0].Id, Is.EqualTo(aCase1Removed.Id));
        Assert.That(caseListRemovedDtStatus[0].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
        Assert.That(caseListRemovedDtStatus[0].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedDtStatus[0].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
        Assert.That(caseListRemovedDtStatus[0].Status, Is.EqualTo(aCase1Removed.Status));
        Assert.That(caseListRemovedDtStatus[0].TemplatId, Is.EqualTo(aCase1Removed.CheckListId));
        Assert.That(caseListRemovedDtStatus[0].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtStatus[0].UpdatedAt.ToString());
        Assert.That(caseListRemovedDtStatus[0].Version, Is.EqualTo(aCase1Removed.Version));
        Assert.That(caseListRemovedDtStatus[0].WorkerName,
            Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
        Assert.That(caseListRemovedDtStatus[0].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

        #endregion

        #region caseListRemovedDtStatus aCase3Removed

        Assert.That(caseListRemovedDtStatus[1].CaseType, Is.EqualTo(aCase3Removed.Type));
        Assert.That(caseListRemovedDtStatus[1].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
        Assert.That(caseListRemovedDtStatus[1].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedDtStatus[1].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
        Assert.That(caseListRemovedDtStatus[1].Custom, Is.EqualTo(aCase3Removed.Custom));
        Assert.That(caseListRemovedDtStatus[1].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
        Assert.That(caseListRemovedDtStatus[1].Id, Is.EqualTo(aCase3Removed.Id));
        Assert.That(caseListRemovedDtStatus[1].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
        Assert.That(caseListRemovedDtStatus[1].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedDtStatus[1].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
        Assert.That(caseListRemovedDtStatus[1].Status, Is.EqualTo(aCase3Removed.Status));
        Assert.That(caseListRemovedDtStatus[1].TemplatId, Is.EqualTo(aCase3Removed.CheckListId));
        Assert.That(caseListRemovedDtStatus[1].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtStatus[1].UpdatedAt.ToString());
        Assert.That(caseListRemovedDtStatus[1].Version, Is.EqualTo(aCase3Removed.Version));
        Assert.That(caseListRemovedDtStatus[1].WorkerName,
            Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
        Assert.That(caseListRemovedDtStatus[1].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

        #endregion

        #endregion

        #region caseListRemovedDtUnitId Def Sort Asc w. DateTime

        #region caseListRemovedDtUnitId aCase1Removed

        Assert.That(caseListRemovedDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedDtUnitId.Count, Is.EqualTo(2));
        Assert.That(caseListRemovedDtUnitId[0].CaseType, Is.EqualTo(aCase1Removed.Type));
        Assert.That(caseListRemovedDtUnitId[0].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
        Assert.That(caseListRemovedDtUnitId[0].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedDtUnitId[0].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
        Assert.That(caseListRemovedDtUnitId[0].Custom, Is.EqualTo(aCase1Removed.Custom));
        Assert.That(caseListRemovedDtUnitId[0].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
        Assert.That(caseListRemovedDtUnitId[0].Id, Is.EqualTo(aCase1Removed.Id));
        Assert.That(caseListRemovedDtUnitId[0].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
        Assert.That(caseListRemovedDtUnitId[0].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedDtUnitId[0].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
        Assert.That(caseListRemovedDtUnitId[0].Status, Is.EqualTo(aCase1Removed.Status));
        Assert.That(caseListRemovedDtUnitId[0].TemplatId, Is.EqualTo(aCase1Removed.CheckListId));
        Assert.That(caseListRemovedDtUnitId[0].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtUnitId[0].UpdatedAt.ToString());
        Assert.That(caseListRemovedDtUnitId[0].Version, Is.EqualTo(aCase1Removed.Version));
        Assert.That(caseListRemovedDtUnitId[0].WorkerName,
            Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
        Assert.That(caseListRemovedDtUnitId[0].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

        #endregion


        #region caseListRemovedDtUnitId aCase3Removed

        Assert.That(caseListRemovedDtUnitId[1].CaseType, Is.EqualTo(aCase3Removed.Type));
        Assert.That(caseListRemovedDtUnitId[1].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
        Assert.That(caseListRemovedDtUnitId[1].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
        Assert.That(caseListRemovedDtUnitId[1].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
        Assert.That(caseListRemovedDtUnitId[1].Custom, Is.EqualTo(aCase3Removed.Custom));
        Assert.That(caseListRemovedDtUnitId[1].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
        Assert.That(caseListRemovedDtUnitId[1].Id, Is.EqualTo(aCase3Removed.Id));
        Assert.That(caseListRemovedDtUnitId[1].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
        Assert.That(caseListRemovedDtUnitId[1].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
        Assert.That(caseListRemovedDtUnitId[1].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
        Assert.That(caseListRemovedDtUnitId[1].Status, Is.EqualTo(aCase3Removed.Status));
        Assert.That(caseListRemovedDtUnitId[1].TemplatId, Is.EqualTo(aCase3Removed.CheckListId));
        Assert.That(caseListRemovedDtUnitId[1].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
        //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtUnitId[1].UpdatedAt.ToString());
        Assert.That(caseListRemovedDtUnitId[1].Version, Is.EqualTo(aCase3Removed.Version));
        Assert.That(caseListRemovedDtUnitId[1].WorkerName,
            Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
        Assert.That(caseListRemovedDtUnitId[1].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

        #endregion

        #endregion

        #endregion

        #endregion

        #region Case Sort

        #region aCase Sort Asc

        #region aCase1Removed sort asc

        #region caseListC1DoneAt aCase1Removed

        Assert.That(caseListRemovedC1SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC1SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC1SortStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC1SortStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC1SortUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC1SortUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC2SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC2SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC2SortStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC2SortStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC2SortUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC2SortUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC3SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC3SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC3SortStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC3SortStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC3SortUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC3SortUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC4SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC4SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC4SortStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC4SortStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC4SortUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC4SortUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC1SortDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC1SortDtDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC1SortDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC1SortDtStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC1SortDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC1SortDtUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC2SortDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC2SortDtDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC2SortDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC2SortDtStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC2SortDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC2SortDtUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC3SortDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC3SortDtDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC3SortDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC3SortDtStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC3SortDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC3SortDtUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC4SortDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC4SortDtDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC4SortDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC4SortDtStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRemovedC4SortDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRemovedC4SortDtUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedDoneAt.Count, Is.EqualTo(4));
        Assert.That(caseListRetractedDoneAt[0].CaseType, Is.EqualTo(aCase1Retracted.Type));
        Assert.That(caseListRetractedDoneAt[0].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
        Assert.That(caseListRetractedDoneAt[0].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
        Assert.That(caseListRetractedDoneAt[0].Custom, Is.EqualTo(aCase1Retracted.Custom));
        Assert.That(caseListRetractedDoneAt[0].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
        Assert.That(caseListRetractedDoneAt[0].Id, Is.EqualTo(aCase1Retracted.Id));
        Assert.That(caseListRetractedDoneAt[0].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
        Assert.That(caseListRetractedDoneAt[0].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedDoneAt[0].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
        Assert.That(caseListRetractedDoneAt[0].Status, Is.EqualTo(aCase1Retracted.Status));
        Assert.That(caseListRetractedDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListRetractedDoneAt[0].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDoneAt[0].UpdatedAt.ToString());
        Assert.That(caseListRetractedDoneAt[0].Version, Is.EqualTo(aCase1Retracted.Version));
        Assert.That(caseListRetractedDoneAt[0].WorkerName,
            Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
        Assert.That(caseListRetractedDoneAt[0].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

        #endregion

        #region caseListRetractedDoneAt aCase2Retracted

        Assert.That(caseListRetractedDoneAt[1].CaseType, Is.EqualTo(aCase2Retracted.Type));
        Assert.That(caseListRetractedDoneAt[1].CaseUId, Is.EqualTo(aCase2Retracted.CaseUid));
        Assert.That(caseListRetractedDoneAt[1].CheckUIid, Is.EqualTo(aCase2Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c2Retracted_ca.ToString()));
        Assert.That(caseListRetractedDoneAt[1].Custom, Is.EqualTo(aCase2Retracted.Custom));
        Assert.That(caseListRetractedDoneAt[1].DoneAt.ToString(), Is.EqualTo(c2Retracted_da.ToString()));
        Assert.That(caseListRetractedDoneAt[1].Id, Is.EqualTo(aCase2Retracted.Id));
        Assert.That(caseListRetractedDoneAt[1].MicrotingUId, Is.EqualTo(aCase2Retracted.MicrotingUid));
        Assert.That(caseListRetractedDoneAt[1].SiteId, Is.EqualTo(aCase2Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedDoneAt[1].SiteName, Is.EqualTo(aCase2Retracted.Site.Name));
        Assert.That(caseListRetractedDoneAt[1].Status, Is.EqualTo(aCase2Retracted.Status));
        Assert.That(caseListRetractedDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListRetractedDoneAt[1].UnitId, Is.EqualTo(aCase2Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedDoneAt[1].UpdatedAt.ToString());
        Assert.That(caseListRetractedDoneAt[1].Version, Is.EqualTo(aCase2Retracted.Version));
        Assert.That(caseListRetractedDoneAt[1].WorkerName,
            Is.EqualTo(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName));
        Assert.That(caseListRetractedDoneAt[1].WorkflowState, Is.EqualTo(aCase2Retracted.WorkflowState));

        #endregion

        #region caseListRetractedDoneAt aCase3Retracted

        Assert.That(caseListRetractedDoneAt[2].CaseType, Is.EqualTo(aCase3Retracted.Type));
        Assert.That(caseListRetractedDoneAt[2].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
        Assert.That(caseListRetractedDoneAt[2].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedDoneAt[2].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
        Assert.That(caseListRetractedDoneAt[2].Custom, Is.EqualTo(aCase3Retracted.Custom));
        Assert.That(caseListRetractedDoneAt[2].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
        Assert.That(caseListRetractedDoneAt[2].Id, Is.EqualTo(aCase3Retracted.Id));
        Assert.That(caseListRetractedDoneAt[2].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
        Assert.That(caseListRetractedDoneAt[2].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedDoneAt[2].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
        Assert.That(caseListRetractedDoneAt[2].Status, Is.EqualTo(aCase3Retracted.Status));
        Assert.That(caseListRetractedDoneAt[2].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListRetractedDoneAt[2].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDoneAt[2].UpdatedAt.ToString());
        Assert.That(caseListRetractedDoneAt[2].Version, Is.EqualTo(aCase3Retracted.Version));
        Assert.That(caseListRetractedDoneAt[2].WorkerName,
            Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
        Assert.That(caseListRetractedDoneAt[2].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

        #endregion

        #region caseListRetractedDoneAt aCase4Retracted

        Assert.That(caseListRetractedDoneAt[3].CaseType, Is.EqualTo(aCase4Retracted.Type));
        Assert.That(caseListRetractedDoneAt[3].CaseUId, Is.EqualTo(aCase4Retracted.CaseUid));
        Assert.That(caseListRetractedDoneAt[3].CheckUIid, Is.EqualTo(aCase4Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedDoneAt[3].CreatedAt.ToString(), Is.EqualTo(c4Retracted_ca.ToString()));
        Assert.That(caseListRetractedDoneAt[3].Custom, Is.EqualTo(aCase4Retracted.Custom));
        Assert.That(caseListRetractedDoneAt[3].DoneAt.ToString(), Is.EqualTo(c4Retracted_da.ToString()));
        Assert.That(caseListRetractedDoneAt[3].Id, Is.EqualTo(aCase4Retracted.Id));
        Assert.That(caseListRetractedDoneAt[3].MicrotingUId, Is.EqualTo(aCase4Retracted.MicrotingUid));
        Assert.That(caseListRetractedDoneAt[3].SiteId, Is.EqualTo(aCase4Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedDoneAt[3].SiteName, Is.EqualTo(aCase4Retracted.Site.Name));
        Assert.That(caseListRetractedDoneAt[3].Status, Is.EqualTo(aCase4Retracted.Status));
        Assert.That(caseListRetractedDoneAt[3].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListRetractedDoneAt[3].UnitId, Is.EqualTo(aCase4Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedDoneAt[3].UpdatedAt.ToString());
        Assert.That(caseListRetractedDoneAt[3].Version, Is.EqualTo(aCase4Retracted.Version));
        Assert.That(caseListRetractedDoneAt[3].WorkerName,
            Is.EqualTo(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName));
        Assert.That(caseListRetractedDoneAt[3].WorkflowState, Is.EqualTo(aCase4Retracted.WorkflowState));

        #endregion

        #endregion

        #region caseListRetractedStatus Def Sort Asc

        #region caseListRetractedStatus aCase1Retracted

        Assert.That(caseListRetractedStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedStatus.Count, Is.EqualTo(4));
        Assert.That(caseListRetractedStatus[0].CaseType, Is.EqualTo(aCase1Retracted.Type));
        Assert.That(caseListRetractedStatus[0].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
        Assert.That(caseListRetractedStatus[0].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedStatus[0].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
        Assert.That(caseListRetractedStatus[0].Custom, Is.EqualTo(aCase1Retracted.Custom));
        Assert.That(caseListRetractedStatus[0].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
        Assert.That(caseListRetractedStatus[0].Id, Is.EqualTo(aCase1Retracted.Id));
        Assert.That(caseListRetractedStatus[0].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
        Assert.That(caseListRetractedStatus[0].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedStatus[0].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
        Assert.That(caseListRetractedStatus[0].Status, Is.EqualTo(aCase1Retracted.Status));
        Assert.That(caseListRetractedStatus[0].TemplatId, Is.EqualTo(aCase1Retracted.CheckListId));
        Assert.That(caseListRetractedStatus[0].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedStatus[0].UpdatedAt.ToString());
        Assert.That(caseListRetractedStatus[0].Version, Is.EqualTo(aCase1Retracted.Version));
        Assert.That(caseListRetractedStatus[0].WorkerName,
            Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
        Assert.That(caseListRetractedStatus[0].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

        #endregion

        #region caseListRetractedStatus aCase2Retracted

        Assert.That(caseListRetractedStatus[1].CaseType, Is.EqualTo(aCase2Retracted.Type));
        Assert.That(caseListRetractedStatus[1].CaseUId, Is.EqualTo(aCase2Retracted.CaseUid));
        Assert.That(caseListRetractedStatus[1].CheckUIid, Is.EqualTo(aCase2Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedStatus[1].CreatedAt.ToString(), Is.EqualTo(c2Retracted_ca.ToString()));
        Assert.That(caseListRetractedStatus[1].Custom, Is.EqualTo(aCase2Retracted.Custom));
        Assert.That(caseListRetractedStatus[1].DoneAt.ToString(), Is.EqualTo(c2Retracted_da.ToString()));
        Assert.That(caseListRetractedStatus[1].Id, Is.EqualTo(aCase2Retracted.Id));
        Assert.That(caseListRetractedStatus[1].MicrotingUId, Is.EqualTo(aCase2Retracted.MicrotingUid));
        Assert.That(caseListRetractedStatus[1].SiteId, Is.EqualTo(aCase2Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedStatus[1].SiteName, Is.EqualTo(aCase2Retracted.Site.Name));
        Assert.That(caseListRetractedStatus[1].Status, Is.EqualTo(aCase2Retracted.Status));
        Assert.That(caseListRetractedStatus[1].TemplatId, Is.EqualTo(aCase2Retracted.CheckListId));
        Assert.That(caseListRetractedStatus[1].UnitId, Is.EqualTo(aCase2Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedStatus[1].UpdatedAt.ToString());
        Assert.That(caseListRetractedStatus[1].Version, Is.EqualTo(aCase2Retracted.Version));
        Assert.That(caseListRetractedStatus[1].WorkerName,
            Is.EqualTo(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName));
        Assert.That(caseListRetractedStatus[1].WorkflowState, Is.EqualTo(aCase2Retracted.WorkflowState));

        #endregion

        #region caseListRetractedStatus aCase3Retracted

        Assert.That(caseListRetractedStatus[2].CaseType, Is.EqualTo(aCase3Retracted.Type));
        Assert.That(caseListRetractedStatus[2].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
        Assert.That(caseListRetractedStatus[2].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedStatus[2].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
        Assert.That(caseListRetractedStatus[2].Custom, Is.EqualTo(aCase3Retracted.Custom));
        Assert.That(caseListRetractedStatus[2].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
        Assert.That(caseListRetractedStatus[2].Id, Is.EqualTo(aCase3Retracted.Id));
        Assert.That(caseListRetractedStatus[2].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
        Assert.That(caseListRetractedStatus[2].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedStatus[2].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
        Assert.That(caseListRetractedStatus[2].Status, Is.EqualTo(aCase3Retracted.Status));
        Assert.That(caseListRetractedStatus[2].TemplatId, Is.EqualTo(aCase3Retracted.CheckListId));
        Assert.That(caseListRetractedStatus[2].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedStatus[2].UpdatedAt.ToString());
        Assert.That(caseListRetractedStatus[2].Version, Is.EqualTo(aCase3Retracted.Version));
        Assert.That(caseListRetractedStatus[2].WorkerName,
            Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
        Assert.That(caseListRetractedStatus[2].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

        #endregion

        #region caseListRetractedStatus aCase4Retracted

        Assert.That(caseListRetractedStatus[3].CaseType, Is.EqualTo(aCase4Retracted.Type));
        Assert.That(caseListRetractedStatus[3].CaseUId, Is.EqualTo(aCase4Retracted.CaseUid));
        Assert.That(caseListRetractedStatus[3].CheckUIid, Is.EqualTo(aCase4Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedStatus[3].CreatedAt.ToString(), Is.EqualTo(c4Retracted_ca.ToString()));
        Assert.That(caseListRetractedStatus[3].Custom, Is.EqualTo(aCase4Retracted.Custom));
        Assert.That(caseListRetractedStatus[3].DoneAt.ToString(), Is.EqualTo(c4Retracted_da.ToString()));
        Assert.That(caseListRetractedStatus[3].Id, Is.EqualTo(aCase4Retracted.Id));
        Assert.That(caseListRetractedStatus[3].MicrotingUId, Is.EqualTo(aCase4Retracted.MicrotingUid));
        Assert.That(caseListRetractedStatus[3].SiteId, Is.EqualTo(aCase4Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedStatus[3].SiteName, Is.EqualTo(aCase4Retracted.Site.Name));
        Assert.That(caseListRetractedStatus[3].Status, Is.EqualTo(aCase4Retracted.Status));
        Assert.That(caseListRetractedStatus[3].TemplatId, Is.EqualTo(aCase4Retracted.CheckListId));
        Assert.That(caseListRetractedStatus[3].UnitId, Is.EqualTo(aCase4Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedStatus[3].UpdatedAt.ToString());
        Assert.That(caseListRetractedStatus[3].Version, Is.EqualTo(aCase4Retracted.Version));
        Assert.That(caseListRetractedStatus[3].WorkerName,
            Is.EqualTo(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName));
        Assert.That(caseListRetractedStatus[3].WorkflowState, Is.EqualTo(aCase4Retracted.WorkflowState));

        #endregion

        #endregion

        #region caseListRetractedUnitId Def Sort Asc

        #region caseListRetractedUnitId aCase1Retracted

        Assert.That(caseListRetractedUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedUnitId.Count, Is.EqualTo(4));
        Assert.That(caseListRetractedUnitId[0].CaseType, Is.EqualTo(aCase1Retracted.Type));
        Assert.That(caseListRetractedUnitId[0].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
        Assert.That(caseListRetractedUnitId[0].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedUnitId[0].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
        Assert.That(caseListRetractedUnitId[0].Custom, Is.EqualTo(aCase1Retracted.Custom));
        Assert.That(caseListRetractedUnitId[0].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
        Assert.That(caseListRetractedUnitId[0].Id, Is.EqualTo(aCase1Retracted.Id));
        Assert.That(caseListRetractedUnitId[0].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
        Assert.That(caseListRetractedUnitId[0].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedUnitId[0].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
        Assert.That(caseListRetractedUnitId[0].Status, Is.EqualTo(aCase1Retracted.Status));
        Assert.That(caseListRetractedUnitId[0].TemplatId, Is.EqualTo(aCase1Retracted.CheckListId));
        Assert.That(caseListRetractedUnitId[0].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedUnitId[0].UpdatedAt.ToString());
        Assert.That(caseListRetractedUnitId[0].Version, Is.EqualTo(aCase1Retracted.Version));
        Assert.That(caseListRetractedUnitId[0].WorkerName,
            Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
        Assert.That(caseListRetractedUnitId[0].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

        #endregion

        #region caseListRetractedUnitId aCase2Retracted

        Assert.That(caseListRetractedUnitId[1].CaseType, Is.EqualTo(aCase2Retracted.Type));
        Assert.That(caseListRetractedUnitId[1].CaseUId, Is.EqualTo(aCase2Retracted.CaseUid));
        Assert.That(caseListRetractedUnitId[1].CheckUIid, Is.EqualTo(aCase2Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedUnitId[1].CreatedAt.ToString(), Is.EqualTo(c2Retracted_ca.ToString()));
        Assert.That(caseListRetractedUnitId[1].Custom, Is.EqualTo(aCase2Retracted.Custom));
        Assert.That(caseListRetractedUnitId[1].DoneAt.ToString(), Is.EqualTo(c2Retracted_da.ToString()));
        Assert.That(caseListRetractedUnitId[1].Id, Is.EqualTo(aCase2Retracted.Id));
        Assert.That(caseListRetractedUnitId[1].MicrotingUId, Is.EqualTo(aCase2Retracted.MicrotingUid));
        Assert.That(caseListRetractedUnitId[1].SiteId, Is.EqualTo(aCase2Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedUnitId[1].SiteName, Is.EqualTo(aCase2Retracted.Site.Name));
        Assert.That(caseListRetractedUnitId[1].Status, Is.EqualTo(aCase2Retracted.Status));
        Assert.That(caseListRetractedUnitId[1].TemplatId, Is.EqualTo(aCase2Retracted.CheckListId));
        Assert.That(caseListRetractedUnitId[1].UnitId, Is.EqualTo(aCase2Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedUnitId[1].UpdatedAt.ToString());
        Assert.That(caseListRetractedUnitId[1].Version, Is.EqualTo(aCase2Retracted.Version));
        Assert.That(caseListRetractedUnitId[1].WorkerName,
            Is.EqualTo(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName));
        Assert.That(caseListRetractedUnitId[1].WorkflowState, Is.EqualTo(aCase2Retracted.WorkflowState));

        #endregion

        #region caseListRetractedUnitId aCase3Retracted

        Assert.That(caseListRetractedUnitId[2].CaseType, Is.EqualTo(aCase3Retracted.Type));
        Assert.That(caseListRetractedUnitId[2].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
        Assert.That(caseListRetractedUnitId[2].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedUnitId[2].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
        Assert.That(caseListRetractedUnitId[2].Custom, Is.EqualTo(aCase3Retracted.Custom));
        Assert.That(caseListRetractedUnitId[2].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
        Assert.That(caseListRetractedUnitId[2].Id, Is.EqualTo(aCase3Retracted.Id));
        Assert.That(caseListRetractedUnitId[2].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
        Assert.That(caseListRetractedUnitId[2].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedUnitId[2].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
        Assert.That(caseListRetractedUnitId[2].Status, Is.EqualTo(aCase3Retracted.Status));
        Assert.That(caseListRetractedUnitId[2].TemplatId, Is.EqualTo(aCase3Retracted.CheckListId));
        Assert.That(caseListRetractedUnitId[2].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedUnitId[2].UpdatedAt.ToString());
        Assert.That(caseListRetractedUnitId[2].Version, Is.EqualTo(aCase3Retracted.Version));
        Assert.That(caseListRetractedUnitId[2].WorkerName,
            Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
        Assert.That(caseListRetractedUnitId[2].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

        #endregion

        #region caseListRetractedUnitId aCase4Retracted

        Assert.That(caseListRetractedUnitId[3].CaseType, Is.EqualTo(aCase4Retracted.Type));
        Assert.That(caseListRetractedUnitId[3].CaseUId, Is.EqualTo(aCase4Retracted.CaseUid));
        Assert.That(caseListRetractedUnitId[3].CheckUIid, Is.EqualTo(aCase4Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedUnitId[3].CreatedAt.ToString(), Is.EqualTo(c4Retracted_ca.ToString()));
        Assert.That(caseListRetractedUnitId[3].Custom, Is.EqualTo(aCase4Retracted.Custom));
        Assert.That(caseListRetractedUnitId[3].DoneAt.ToString(), Is.EqualTo(c4Retracted_da.ToString()));
        Assert.That(caseListRetractedUnitId[3].Id, Is.EqualTo(aCase4Retracted.Id));
        Assert.That(caseListRetractedUnitId[3].MicrotingUId, Is.EqualTo(aCase4Retracted.MicrotingUid));
        Assert.That(caseListRetractedUnitId[3].SiteId, Is.EqualTo(aCase4Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedUnitId[3].SiteName, Is.EqualTo(aCase4Retracted.Site.Name));
        Assert.That(caseListRetractedUnitId[3].Status, Is.EqualTo(aCase4Retracted.Status));
        Assert.That(caseListRetractedUnitId[3].TemplatId, Is.EqualTo(aCase4Retracted.CheckListId));
        Assert.That(caseListRetractedUnitId[3].UnitId, Is.EqualTo(aCase4Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedUnitId[3].UpdatedAt.ToString());
        Assert.That(caseListRetractedUnitId[3].Version, Is.EqualTo(aCase4Retracted.Version));
        Assert.That(caseListRetractedUnitId[3].WorkerName,
            Is.EqualTo(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName));
        Assert.That(caseListRetractedUnitId[3].WorkflowState, Is.EqualTo(aCase4Retracted.WorkflowState));

        #endregion

        #endregion

        #endregion


        #region Def Sort Asc w. DateTime

        #region caseListRetractedDtDoneAt Def Sort Asc w. DateTime

        #region caseListRetractedDtDoneAt aCase1Retracted

        Assert.That(caseListRetractedDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedDtDoneAt.Count, Is.EqualTo(2));
        Assert.That(caseListRetractedDtDoneAt[0].CaseType, Is.EqualTo(aCase1Retracted.Type));
        Assert.That(caseListRetractedDtDoneAt[0].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
        Assert.That(caseListRetractedDtDoneAt[0].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedDtDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
        Assert.That(caseListRetractedDtDoneAt[0].Custom, Is.EqualTo(aCase1Retracted.Custom));
        Assert.That(caseListRetractedDtDoneAt[0].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
        Assert.That(caseListRetractedDtDoneAt[0].Id, Is.EqualTo(aCase1Retracted.Id));
        Assert.That(caseListRetractedDtDoneAt[0].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
        Assert.That(caseListRetractedDtDoneAt[0].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedDtDoneAt[0].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
        Assert.That(caseListRetractedDtDoneAt[0].Status, Is.EqualTo(aCase1Retracted.Status));
        Assert.That(caseListRetractedDtDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListRetractedDtDoneAt[0].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtDoneAt[0].UpdatedAt.ToString());
        Assert.That(caseListRetractedDtDoneAt[0].Version, Is.EqualTo(aCase1Retracted.Version));
        Assert.That(caseListRetractedDtDoneAt[0].WorkerName,
            Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
        Assert.That(caseListRetractedDtDoneAt[0].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

        #endregion

        #region caseListRetractedDtDoneAt aCase3Retracted

        Assert.That(caseListRetractedDtDoneAt[1].CaseType, Is.EqualTo(aCase3Retracted.Type));
        Assert.That(caseListRetractedDtDoneAt[1].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
        Assert.That(caseListRetractedDtDoneAt[1].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedDtDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
        Assert.That(caseListRetractedDtDoneAt[1].Custom, Is.EqualTo(aCase3Retracted.Custom));
        Assert.That(caseListRetractedDtDoneAt[1].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
        Assert.That(caseListRetractedDtDoneAt[1].Id, Is.EqualTo(aCase3Retracted.Id));
        Assert.That(caseListRetractedDtDoneAt[1].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
        Assert.That(caseListRetractedDtDoneAt[1].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedDtDoneAt[1].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
        Assert.That(caseListRetractedDtDoneAt[1].Status, Is.EqualTo(aCase3Retracted.Status));
        Assert.That(caseListRetractedDtDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
        Assert.That(caseListRetractedDtDoneAt[1].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtDoneAt[1].UpdatedAt.ToString());
        Assert.That(caseListRetractedDtDoneAt[1].Version, Is.EqualTo(aCase3Retracted.Version));
        Assert.That(caseListRetractedDtDoneAt[1].WorkerName,
            Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
        Assert.That(caseListRetractedDtDoneAt[1].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

        #endregion

        #endregion

        #region caseListRetractedDtStatus Def Sort Asc w. DateTime

        #region caseListRetractedDtStatus aCase1Retracted

        Assert.That(caseListRetractedDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedDtStatus.Count, Is.EqualTo(2));
        Assert.That(caseListRetractedDtStatus[0].CaseType, Is.EqualTo(aCase1Retracted.Type));
        Assert.That(caseListRetractedDtStatus[0].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
        Assert.That(caseListRetractedDtStatus[0].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedDtStatus[0].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
        Assert.That(caseListRetractedDtStatus[0].Custom, Is.EqualTo(aCase1Retracted.Custom));
        Assert.That(caseListRetractedDtStatus[0].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
        Assert.That(caseListRetractedDtStatus[0].Id, Is.EqualTo(aCase1Retracted.Id));
        Assert.That(caseListRetractedDtStatus[0].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
        Assert.That(caseListRetractedDtStatus[0].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedDtStatus[0].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
        Assert.That(caseListRetractedDtStatus[0].Status, Is.EqualTo(aCase1Retracted.Status));
        Assert.That(caseListRetractedDtStatus[0].TemplatId, Is.EqualTo(aCase1Retracted.CheckListId));
        Assert.That(caseListRetractedDtStatus[0].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtStatus[0].UpdatedAt.ToString());
        Assert.That(caseListRetractedDtStatus[0].Version, Is.EqualTo(aCase1Retracted.Version));
        Assert.That(caseListRetractedDtStatus[0].WorkerName,
            Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
        Assert.That(caseListRetractedDtStatus[0].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

        #endregion


        #region caseListRetractedDtStatus aCase3Retracted

        Assert.That(caseListRetractedDtStatus[1].CaseType, Is.EqualTo(aCase3Retracted.Type));
        Assert.That(caseListRetractedDtStatus[1].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
        Assert.That(caseListRetractedDtStatus[1].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedDtStatus[1].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
        Assert.That(caseListRetractedDtStatus[1].Custom, Is.EqualTo(aCase3Retracted.Custom));
        Assert.That(caseListRetractedDtStatus[1].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
        Assert.That(caseListRetractedDtStatus[1].Id, Is.EqualTo(aCase3Retracted.Id));
        Assert.That(caseListRetractedDtStatus[1].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
        Assert.That(caseListRetractedDtStatus[1].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedDtStatus[1].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
        Assert.That(caseListRetractedDtStatus[1].Status, Is.EqualTo(aCase3Retracted.Status));
        Assert.That(caseListRetractedDtStatus[1].TemplatId, Is.EqualTo(aCase3Retracted.CheckListId));
        Assert.That(caseListRetractedDtStatus[1].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtStatus[1].UpdatedAt.ToString());
        Assert.That(caseListRetractedDtStatus[1].Version, Is.EqualTo(aCase3Retracted.Version));
        Assert.That(caseListRetractedDtStatus[1].WorkerName,
            Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
        Assert.That(caseListRetractedDtStatus[1].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

        #endregion

        #endregion

        #region caseListRetractedDtUnitId Def Sort Asc w. DateTime

        #region caseListRetractedDtUnitId aCase1Retracted

        Assert.That(caseListRetractedDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedDtUnitId.Count, Is.EqualTo(2));
        Assert.That(caseListRetractedDtUnitId[0].CaseType, Is.EqualTo(aCase1Retracted.Type));
        Assert.That(caseListRetractedDtUnitId[0].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
        Assert.That(caseListRetractedDtUnitId[0].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedDtUnitId[0].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
        Assert.That(caseListRetractedDtUnitId[0].Custom, Is.EqualTo(aCase1Retracted.Custom));
        Assert.That(caseListRetractedDtUnitId[0].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
        Assert.That(caseListRetractedDtUnitId[0].Id, Is.EqualTo(aCase1Retracted.Id));
        Assert.That(caseListRetractedDtUnitId[0].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
        Assert.That(caseListRetractedDtUnitId[0].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedDtUnitId[0].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
        Assert.That(caseListRetractedDtUnitId[0].Status, Is.EqualTo(aCase1Retracted.Status));
        Assert.That(caseListRetractedDtUnitId[0].TemplatId, Is.EqualTo(aCase1Retracted.CheckListId));
        Assert.That(caseListRetractedDtUnitId[0].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtUnitId[0].UpdatedAt.ToString());
        Assert.That(caseListRetractedDtUnitId[0].Version, Is.EqualTo(aCase1Retracted.Version));
        Assert.That(caseListRetractedDtUnitId[0].WorkerName,
            Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
        Assert.That(caseListRetractedDtUnitId[0].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

        #endregion


        #region caseListRetractedDtUnitId aCase3Retracted

        Assert.That(caseListRetractedDtUnitId[1].CaseType, Is.EqualTo(aCase3Retracted.Type));
        Assert.That(caseListRetractedDtUnitId[1].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
        Assert.That(caseListRetractedDtUnitId[1].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
        Assert.That(caseListRetractedDtUnitId[1].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
        Assert.That(caseListRetractedDtUnitId[1].Custom, Is.EqualTo(aCase3Retracted.Custom));
        Assert.That(caseListRetractedDtUnitId[1].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
        Assert.That(caseListRetractedDtUnitId[1].Id, Is.EqualTo(aCase3Retracted.Id));
        Assert.That(caseListRetractedDtUnitId[1].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
        Assert.That(caseListRetractedDtUnitId[1].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
        Assert.That(caseListRetractedDtUnitId[1].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
        Assert.That(caseListRetractedDtUnitId[1].Status, Is.EqualTo(aCase3Retracted.Status));
        Assert.That(caseListRetractedDtUnitId[1].TemplatId, Is.EqualTo(aCase3Retracted.CheckListId));
        Assert.That(caseListRetractedDtUnitId[1].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
        //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtUnitId[1].UpdatedAt.ToString());
        Assert.That(caseListRetractedDtUnitId[1].Version, Is.EqualTo(aCase3Retracted.Version));
        Assert.That(caseListRetractedDtUnitId[1].WorkerName,
            Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
        Assert.That(caseListRetractedDtUnitId[1].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

        #endregion

        #endregion

        #endregion

        #endregion

        #region Case Sort

        #region aCase Sort Asc

        #region aCase1Retracted sort asc

        #region caseListC1DoneAt aCase1Retracted

        Assert.That(caseListRetractedC1SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC1SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC1SortStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC1SortStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC1SortUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC1SortUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC2SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC2SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC2SortStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC2SortStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC2SortUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC2SortUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC3SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC3SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC3SortStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC3SortStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC3SortUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC3SortUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC4SortDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC4SortDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC4SortStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC4SortStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC4SortUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC4SortUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC1SortDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC1SortDtDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC1SortDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC1SortDtStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC1SortDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC1SortDtUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC2SortDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC2SortDtDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC2SortDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC2SortDtStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC2SortDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC2SortDtUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC3SortDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC3SortDtDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC3SortDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC3SortDtStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC3SortDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC3SortDtUnitId.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC4SortDtDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC4SortDtDoneAt.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC4SortDtStatus, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC4SortDtStatus.Count, Is.EqualTo(0));
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

        Assert.That(caseListRetractedC4SortDtUnitId, Is.Not.EqualTo(null));
        Assert.That(caseListRetractedC4SortDtUnitId.Count, Is.EqualTo(0));
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