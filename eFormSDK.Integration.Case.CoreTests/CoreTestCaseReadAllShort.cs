/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

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

        Assert.That(caseListDoneAt, Is.Not.EqualTo(null));
        Assert.That(caseListDoneAt.Count, Is.EqualTo(8));
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
        Assert.That(caseListStatus.Count, Is.EqualTo(8));
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
        Assert.That(caseListUnitId.Count, Is.EqualTo(8));
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
        Assert.That(caseListDtDoneAt.Count, Is.EqualTo(4));
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
        Assert.That(caseListDtStatus.Count, Is.EqualTo(4));
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
        Assert.That(caseListDtUnitId.Count, Is.EqualTo(4));
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
        Assert.That(caseListC1SortDoneAt.Count, Is.EqualTo(8));
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
        Assert.That(caseListC1SortStatus.Count, Is.EqualTo(8));
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
        Assert.That(caseListC1SortUnitId.Count, Is.EqualTo(8));
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
        Assert.That(caseListC2SortDoneAt.Count, Is.EqualTo(8));
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
        Assert.That(caseListC2SortStatus.Count, Is.EqualTo(8));
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
        Assert.That(caseListC2SortUnitId.Count, Is.EqualTo(8));
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
        Assert.That(caseListC3SortDoneAt.Count, Is.EqualTo(8));
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
        Assert.That(caseListC3SortStatus.Count, Is.EqualTo(8));
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
        Assert.That(caseListC3SortUnitId.Count, Is.EqualTo(8));
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
        Assert.That(caseListC4SortDoneAt.Count, Is.EqualTo(8));
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
        Assert.That(caseListC1SortDoneAt.Count, Is.EqualTo(8));
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
        Assert.That(caseListC4SortDoneAt.Count, Is.EqualTo(8));
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
        Assert.That(caseListC1SortDtDoneAt.Count, Is.EqualTo(4));
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
        Assert.That(caseListC1SortDtStatus.Count, Is.EqualTo(4));
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
        Assert.That(caseListC1SortDtUnitId.Count, Is.EqualTo(4));
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
        Assert.That(caseListC2SortDtDoneAt.Count, Is.EqualTo(4));
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
        Assert.That(caseListC2SortDtStatus.Count, Is.EqualTo(4));
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
        Assert.That(caseListC2SortDtUnitId.Count, Is.EqualTo(4));
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
        Assert.That(caseListC3SortDtDoneAt.Count, Is.EqualTo(4));
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
        Assert.That(caseListC3SortDtStatus.Count, Is.EqualTo(4));
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
        Assert.That(caseListC3SortDtUnitId.Count, Is.EqualTo(4));
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
        Assert.That(caseListC4SortDtDoneAt.Count, Is.EqualTo(4));
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
        Assert.That(caseListC4SortDtStatus.Count, Is.EqualTo(4));
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
        Assert.That(caseListC4SortDtUnitId.Count, Is.EqualTo(4));
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