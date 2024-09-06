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
    public class CoreTestCaseReadAllLong : DbTestFixture
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
        public async Task Core_Case_CaseReadAll_Long()
        {
            // Arrance

            #region Arrance

            #region Template1

            DateTime cl1_ca = DateTime.UtcNow;
            DateTime cl1_ua = DateTime.UtcNow;
            Random rnd = new Random();
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

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

            Worker worker1 =
                await testHelpers.CreateWorker("aa@tak1.dk", "Arne1", "Jensen", await testHelpers.GetRandomInt());
            Worker worker2 =
                await testHelpers.CreateWorker("aa@tak2.dk", "Arne2", "Jensen", await testHelpers.GetRandomInt());
            Worker worker3 =
                await testHelpers.CreateWorker("aa@tak3.dk", "Arne3", "Jensen", await testHelpers.GetRandomInt());
            Worker worker4 =
                await testHelpers.CreateWorker("aa@tak4.dk", "Arne4", "Jensen", await testHelpers.GetRandomInt());

            #endregion

            #region site

            Site site1 = await testHelpers.CreateSite("SiteName1", await testHelpers.GetRandomInt());
            Site site2 = await testHelpers.CreateSite("SiteName2", await testHelpers.GetRandomInt());
            Site site3 = await testHelpers.CreateSite("SiteName3", await testHelpers.GetRandomInt());
            Site site4 = await testHelpers.CreateSite("SiteName4", await testHelpers.GetRandomInt());

            #endregion

            #region units

            Unit unit1 = await testHelpers.CreateUnit(await testHelpers.GetRandomInt(), 49, site1, 348);
            Unit unit2 = await testHelpers.CreateUnit(await testHelpers.GetRandomInt(), 49, site2, 348);
            Unit unit3 = await testHelpers.CreateUnit(await testHelpers.GetRandomInt(), 49, site3, 348);
            Unit unit4 = await testHelpers.CreateUnit(await testHelpers.GetRandomInt(), 49, site4, 348);

            #endregion

            #region site_workers

            SiteWorker site_workers1 =
                await testHelpers.CreateSiteWorker(await testHelpers.GetRandomInt(), site1, worker1);
            SiteWorker site_workers2 =
                await testHelpers.CreateSiteWorker(await testHelpers.GetRandomInt(), site2, worker2);
            SiteWorker site_workers3 =
                await testHelpers.CreateSiteWorker(await testHelpers.GetRandomInt(), site3, worker3);
            SiteWorker site_workers4 =
                await testHelpers.CreateSiteWorker(await testHelpers.GetRandomInt(), site4, worker4);

            #endregion

            #region cases

            #region cases created

            #region Case1

            DateTime c1_ca = DateTime.UtcNow.AddDays(-9);
            DateTime c1_da = DateTime.UtcNow.AddDays(-8).AddHours(-12);
            DateTime c1_ua = DateTime.UtcNow.AddDays(-8);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase1 = await testHelpers.CreateCase("case1UId", cl1,
                c1_ca, "custom1",
                c1_da, worker1, rnd.Next(1, 255), rnd.Next(1, 255),
                site1, 1, "caseType1", unit1, c1_ua, 1, worker1, Constants.WorkflowStates.Created);

            #endregion

            #region Case2

            DateTime c2_ca = DateTime.UtcNow.AddDays(-7);
            DateTime c2_da = DateTime.UtcNow.AddDays(-6).AddHours(-12);
            DateTime c2_ua = DateTime.UtcNow.AddDays(-6);
            Microting.eForm.Infrastructure.Data.Entities.Case aCase2 = await testHelpers.CreateCase("case2UId", cl1,
                c2_ca, "custom2",
                c2_da, worker2, rnd.Next(1, 255), rnd.Next(1, 255),
                site2, 10, "caseType2", unit2, c2_ua, 1, worker2, Constants.WorkflowStates.Created);

            #endregion

            #region Case3

            DateTime c3_ca = DateTime.UtcNow.AddDays(-10);
            DateTime c3_da = DateTime.UtcNow.AddDays(-9).AddHours(-12);
            DateTime c3_ua = DateTime.UtcNow.AddDays(-9);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase3 = await testHelpers.CreateCase("case3UId", cl1,
                c3_ca, "custom3",
                c3_da, worker3, rnd.Next(1, 255), rnd.Next(1, 255),
                site3, 15, "caseType3", unit3, c3_ua, 1, worker3, Constants.WorkflowStates.Created);

            #endregion

            #region Case4

            DateTime c4_ca = DateTime.UtcNow.AddDays(-8);
            DateTime c4_da = DateTime.UtcNow.AddDays(-7).AddHours(-12);
            DateTime c4_ua = DateTime.UtcNow.AddDays(-7);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase4 = await testHelpers.CreateCase("case4UId", cl1,
                c4_ca, "custom4",
                c4_da, worker4, rnd.Next(1, 255), rnd.Next(1, 255),
                site4, 100, "caseType4", unit4, c4_ua, 1, worker4, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            #region cases removed

            #region Case1Removed

            DateTime c1Removed_ca = DateTime.UtcNow.AddDays(-9);
            DateTime c1Removed_da = DateTime.UtcNow.AddDays(-8).AddHours(-12);
            DateTime c1Removed_ua = DateTime.UtcNow.AddDays(-8);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase1Removed = await testHelpers.CreateCase("case1UId",
                cl1, c1Removed_ca, "custom1",
                c1Removed_da, worker1, rnd.Next(1, 255), rnd.Next(1, 255),
                site1, 1, "caseType1", unit1, c1Removed_ua, 1, worker1, Constants.WorkflowStates.Removed);

            #endregion

            #region Case2Removed

            DateTime c2Removed_ca = DateTime.UtcNow.AddDays(-7);
            DateTime c2Removed_da = DateTime.UtcNow.AddDays(-6).AddHours(-12);
            DateTime c2Removed_ua = DateTime.UtcNow.AddDays(-6);
            Microting.eForm.Infrastructure.Data.Entities.Case aCase2Removed = await testHelpers.CreateCase("case2UId",
                cl1, c2Removed_ca, "custom2",
                c2Removed_da, worker2, rnd.Next(1, 255), rnd.Next(1, 255),
                site2, 10, "caseType2", unit2, c2Removed_ua, 1, worker2, Constants.WorkflowStates.Removed);

            #endregion

            #region Case3Removed

            DateTime c3Removed_ca = DateTime.UtcNow.AddDays(-10);
            DateTime c3Removed_da = DateTime.UtcNow.AddDays(-9).AddHours(-12);
            DateTime c3Removed_ua = DateTime.UtcNow.AddDays(-9);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase3Removed = await testHelpers.CreateCase("case3UId",
                cl1, c3Removed_ca, "custom3",
                c3Removed_da, worker3, rnd.Next(1, 255), rnd.Next(1, 255),
                site3, 15, "caseType3", unit3, c3Removed_ua, 1, worker3, Constants.WorkflowStates.Removed);

            #endregion

            #region Case4Removed

            DateTime c4Removed_ca = DateTime.UtcNow.AddDays(-8);
            DateTime c4Removed_da = DateTime.UtcNow.AddDays(-7).AddHours(-12);
            DateTime c4Removed_ua = DateTime.UtcNow.AddDays(-7);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase4Removed = await testHelpers.CreateCase("case4UId",
                cl1, c4Removed_ca, "custom4",
                c4Removed_da, worker4, rnd.Next(1, 255), rnd.Next(1, 255),
                site4, 100, "caseType4", unit4, c4Removed_ua, 1, worker4, Constants.WorkflowStates.Removed);

            #endregion

            #endregion

            #region cases Retracted

            #region Case1Retracted

            DateTime c1Retracted_ca = DateTime.UtcNow.AddDays(-9);
            DateTime c1Retracted_da = DateTime.UtcNow.AddDays(-8).AddHours(-12);
            DateTime c1Retracted_ua = DateTime.UtcNow.AddDays(-8);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase1Retracted = await testHelpers.CreateCase("case1UId",
                cl1, c1Retracted_ca, "custom1",
                c1Retracted_da, worker1, rnd.Next(1, 255), rnd.Next(1, 255),
                site1, 1, "caseType1", unit1, c1Retracted_ua, 1, worker1, Constants.WorkflowStates.Retracted);

            #endregion

            #region Case2Retracted

            DateTime c2Retracted_ca = DateTime.UtcNow.AddDays(-7);
            DateTime c2Retracted_da = DateTime.UtcNow.AddDays(-6).AddHours(-12);
            DateTime c2Retracted_ua = DateTime.UtcNow.AddDays(-6);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase2Retracted = await testHelpers.CreateCase("case2UId",
                cl1, c2Retracted_ca, "custom2",
                c2Retracted_da, worker2, rnd.Next(1, 255), rnd.Next(1, 255),
                site2, 10, "caseType2", unit2, c2Retracted_ua, 1, worker2, Constants.WorkflowStates.Retracted);

            #endregion

            #region Case3Retracted

            DateTime c3Retracted_ca = DateTime.UtcNow.AddDays(-10);
            DateTime c3Retracted_da = DateTime.UtcNow.AddDays(-9).AddHours(-12);
            DateTime c3Retracted_ua = DateTime.UtcNow.AddDays(-9);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase3Retracted = await testHelpers.CreateCase("case3UId",
                cl1, c3Retracted_ca, "custom3",
                c3Retracted_da, worker3, rnd.Next(1, 255), rnd.Next(1, 255),
                site3, 15, "caseType3", unit3, c3Retracted_ua, 1, worker3, Constants.WorkflowStates.Retracted);

            #endregion

            #region Case4Retracted

            DateTime c4Retracted_ca = DateTime.UtcNow.AddDays(-8);
            DateTime c4Retracted_da = DateTime.UtcNow.AddDays(-7).AddHours(-12);
            DateTime c4Retracted_ua = DateTime.UtcNow.AddDays(-7);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase4Retracted = await testHelpers.CreateCase("case4UId",
                cl1, c4Retracted_ca, "custom4",
                c4Retracted_da, worker4, rnd.Next(1, 255), rnd.Next(1, 255),
                site4, 100, "caseType4", unit4, c4Retracted_ua, 1, worker4, Constants.WorkflowStates.Retracted);

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
                Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region Default sorting descending

            //Default sorting descending
            //List<Case> caseListDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListDescendingStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListDescendingUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region Default sorting ascending, with DateTime

            // Default sorting ascending, with DateTime
            //List<Case> caseListDtCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false,
                Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false,
                Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false,
                Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region Default sorting descending, with DateTime

            //Default sorting descending, with DateTime
            //List<Case> caseListDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true,
                Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true,
                Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true,
                Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region case sorting

            #region aCase sorting ascending

            #region aCase1 sorting ascendng

            //List<Case> caseListC1SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC1SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "10000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Created, "10000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC1SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "10000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC1SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng

            //List<Case> caseListC2SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC2SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "2000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Created, "2000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC2SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "2000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC2SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng

            //List<Case> caseListC3SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC3SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "3000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Created, "3000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC3SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "3000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC3SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng

            //List<Case> caseListC4SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC4SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "4000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Created, "4000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC4SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "4000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC4SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region aCase sorting Descending

            #region aCase1 sorting Descending

            //List<Case> caseListC1SortDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC1SortDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "1000", true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListC1SortDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC1SortDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC1SortDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC1SortDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC1SortDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC1SortDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC1SortDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC1SortDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC1SortDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC1SortDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC1SortDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC1SortDescendingStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "1000", true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC1SortDescendingUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "1000", true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC1SortDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting Descending

            //List<Case> caseListC2SortDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC2SortDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "2000", true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListC2SortDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC2SortDescendingDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC2SortDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC2SortDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC2SortDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC2SortDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC2SortDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC2SortDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC2SortDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC2SortDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC2SortDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC2SortDescendingStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "2000", true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC2SortDescendingUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "2000", true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC2SortDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting Descending

            //List<Case> caseListC3SortDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC3SortDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "3000", true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListC3SortDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC3SortDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC3SortDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC3SortDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC3SortDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC3SortDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC3SortDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC3SortDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC3SortDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC3SortDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC3SortDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC3SortDescendingStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "3000", true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC3SortDescendingUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "3000", true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC3SortDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting Descending

            //List<Case> caseListC4SortDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC4SortDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "4000", true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListC4SortDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC4SortDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC4SortDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC4SortDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC4SortDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC4SortDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC4SortDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC4SortDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC4SortDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC4SortDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC4SortDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC4SortDescendingStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "4000", true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC4SortDescendingUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "4000", true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC4SortDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region aCase sorting ascending w. Dt

            #region aCase1 sorting ascendng w. Dt

            //List<Case> caseListC1SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC1SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1000",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC1SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "0001",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC1SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng w. Dt

            //List<Case> caseListC2SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC2SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2000",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC2SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2000",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC2SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng w. Dt

            //List<Case> caseListC3SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC3SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3000",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC3SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3000",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC3SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng w. Dt

            //List<Case> caseListC4SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC4SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4000",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC4SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4000",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC4SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region aCase sorting Descending w. Dt

            #region aCase1 sorting Descending w. Dt

            //List<Case> caseListC1SortDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC1SortDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1000",
                true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListC1SortDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC1SortDtDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC1SortDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC1SortDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC1SortDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC1SortDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC1SortDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC1SortDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC1SortDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC1SortDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC1SortvDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC1SortDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1000",
                true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC1SortDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1000",
                true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC1SortDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting Descending w. Dt

            //List<Case> caseListC2SortDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC2SortDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2000",
                true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListC2SortDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC2SortDtDescendingDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC2SortDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC2SortDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC2SortDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC2SortDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC2SortDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC2SortDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC2SortDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC2SortDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC2SortDtDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC2SortDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2000",
                true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC2SortDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2000",
                true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC2SortDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "2", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting Descending w. Dt

            //List<Case> caseListC3SortDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC3SortDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3000",
                true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListC3SortDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC3SortDtDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC3SortDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC3SortDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC3SortDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC3SortDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC3SortDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC3SortDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC3SortDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC3SortDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC3SortDtDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC3SortDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3000",
                true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC3SortDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3000",
                true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC3SortDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "3", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting Descending w. Dt

            //List<Case> caseListC4SortDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListC4SortDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4000",
                true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListC4SortDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC4SortDtDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC4SortDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC4SortDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC4SortDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC4SortDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC4SortDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC4SortDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC4SortDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC4SortDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC4SortDtDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListC4SortDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4000",
                true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC4SortDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4000",
                true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListC4SortDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Created, "4", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #endregion

            #endregion

            #region sorting by WorkflowState removed

            #region Default sorting

            #region Default sorting ascending

            // Default sorting ascending
            //List<Case> caseListRemovedCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
            List<Microting.eForm.Dto.Case> caseListRemovedStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region Default sorting descending

            //Default sorting descending
            //List<Case> caseListRemovedDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListRemovedDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedDescendingStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedDescendingUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region Default sorting ascending, with DateTime

            // Default sorting ascending, with DateTime
            //List<Case> caseListRemovedDtCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false,
                Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false,
                Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false,
                Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region Default sorting descending, with DateTime

            //Default sorting descending, with DateTime
            //List<Case> caseListRemovedDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true,
                Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListRemovedDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedDtDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedDtDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true,
                Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true,
                Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region case sorting

            #region aCase sorting ascending

            #region aCase1 sorting ascendng

            //List<Case> caseListRemovedC1SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "1000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Removed, "1000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "1000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedC1SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng

            //List<Case> caseListRemovedC2SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "2000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Removed, "2000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "2000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedC2SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng

            //List<Case> caseListRemovedC3SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "3000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Removed, "3000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "3000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedC3SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng

            //List<Case> caseListRemovedC4SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "4000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Removed, "4000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Removed, "4000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedC4SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region aCase sorting Descending

            #region aCase1 sorting Descending

            //List<Case> caseListRemovedC1SortDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Removed, "1000", true, Constants.CaseSortParameters.DoneAt,
                timeZoneInfo);
            //List<Case> caseListRemovedC1SortDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC1SortDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC1SortDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC1SortDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC1SortDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC1SortDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC1SortDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC1SortDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC1SortDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC1SortDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedC1SortDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDescendingStatus = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Removed, "1000", true, Constants.CaseSortParameters.Status,
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDescendingUnitId = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Removed, "1000", true, Constants.CaseSortParameters.UnitId,
                timeZoneInfo);
            //List<Case> caseListRemovedC1SortDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting Descending

            //List<Case> caseListRemovedC2SortDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Removed, "2000", true, Constants.CaseSortParameters.DoneAt,
                timeZoneInfo);
            //List<Case> caseListRemovedC2SortDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC2SortDescendingDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC2SortDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC2SortDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC2SortDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC2SortDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC2SortDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC2SortDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC2SortDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC2SortDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedC2SortDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDescendingStatus = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Removed, "2000", true, Constants.CaseSortParameters.Status,
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDescendingUnitId = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Removed, "2000", true, Constants.CaseSortParameters.UnitId,
                timeZoneInfo);
            //List<Case> caseListRemovedC2SortDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting Descending

            //List<Case> caseListRemovedC3SortDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Removed, "3000", true, Constants.CaseSortParameters.DoneAt,
                timeZoneInfo);
            //List<Case> caseListRemovedC3SortDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC3SortDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC3SortDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC3SortDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC3SortDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC3SortDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC3SortDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC3SortDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC3SortDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC3SortDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC3SortDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDescendingStatus = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Removed, "3000", true, Constants.CaseSortParameters.Status,
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDescendingUnitId = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Removed, "3000", true, Constants.CaseSortParameters.UnitId,
                timeZoneInfo);
            //List<Case> caseListRemovedC3SortDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting Descending

            //List<Case> caseListRemovedC4SortDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Removed, "4000", true, Constants.CaseSortParameters.DoneAt,
                timeZoneInfo);
            //List<Case> caseListRemovedC4SortDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC4SortDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC4SortDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC4SortDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC4SortDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC4SortDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC4SortDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC4SortDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC4SortDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC4SortDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC4SortDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDescendingStatus = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Removed, "4000", true, Constants.CaseSortParameters.Status,
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDescendingUnitId = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Removed, "4000", true, Constants.CaseSortParameters.UnitId,
                timeZoneInfo);
            //List<Case> caseListRemovedC4SortDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region aCase sorting ascending w. Dt

            #region aCase1 sorting ascendng w. Dt

            //List<Case> caseListRemovedC1SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1000",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1000",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedC1SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng w. Dt

            //List<Case> caseListRemovedC2SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2000",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2000",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedC2SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng w. Dt

            //List<Case> caseListRemovedC3SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3000",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3000",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedC3SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng w. Dt

            //List<Case> caseListRemovedC4SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4000",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4000",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedC4SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region aCase sorting Descending w. Dt

            #region aCase1 sorting Descending w. Dt

            //List<Case> caseListRemovedC1SortDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1000",
                true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListRemovedC1SortDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC1SortDtDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC1SortDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC1SortDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC1SortDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC1SortDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC1SortDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC1SortDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC1SortDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC1SortDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedC1SortvDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1000",
                true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC1SortDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1000",
                true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedC1SortDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "1", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting Descending w. Dt

            //List<Case> caseListRemovedC2SortDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2000",
                true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListRemovedC2SortDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC2SortDtDescendingDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC2SortDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC2SortDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC2SortDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC2SortDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC2SortDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC2SortDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC2SortDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC2SortDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedC2SortDtDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2000",
                true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC2SortDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2000",
                true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedC2SortDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Removed, "2", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting Descending w. Dt

            //List<Case> caseListRemovedC3SortDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3000",
                true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListRemovedC3SortDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC3SortDtDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC3SortDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC3SortDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC3SortDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC3SortDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC3SortDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC3SortDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC3SortDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC3SortDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC3SortDtDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3000",
                true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC3SortDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3000",
                true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedC3SortDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "3", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting Descending w. Dt

            //List<Case> caseListRemovedC4SortDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4000",
                true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListRemovedC4SortDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRemovedC4SortDtDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRemovedC4SortDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRemovedC4SortDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRemovedC4SortDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRemovedC4SortDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRemovedC4SortDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRemovedC4SortDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRemovedC4SortDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRemovedC4SortDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRemovedC4SortDtDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4000",
                true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRemovedC4SortDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4000",
                true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRemovedC4SortDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Removed, "4", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #endregion

            #endregion

            #region sorting by WorkflowState Retracted

            #region Default sorting

            #region Default sorting ascending

            // Default sorting ascending
            //List<Case> caseListRetractedCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
            List<Microting.eForm.Dto.Case> caseListRetractedStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRetractedWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region Default sorting descending

            //Default sorting descending
            //List<Case> caseListRetractedDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseListRetractedDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRetractedDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRetractedDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRetractedDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRetractedDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRetractedDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRetractedDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRetractedDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRetractedDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRetractedDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRetractedDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedDescendingStatus = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedDescendingUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRetractedDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.WorkerName);

            #endregion


            #region Default sorting ascending, with DateTime

            // Default sorting ascending, with DateTime
            //List<Case> caseDtListRetractedDtCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseDtListRetractedDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region Default sorting descending, with DateTime

            //Default sorting descending, with DateTime
            //List<Case> caseDtListRetractedDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true,
                Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseDtListRetractedDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseDtListRetractedDtDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListRetractedDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListRetractedDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListRetractedDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListRetractedDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListRetractedDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListRetractedDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListRetractedDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListRetractedDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseDtListRetractedDtDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true,
                Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true,
                Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseDtListRetractedDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region case sorting

            #region aCase sorting ascending

            #region aCase1 sorting ascendng

            //List<Case> caseListRetractedC1SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "1000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Retracted, "1000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "1000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRetractedC1SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng

            //List<Case> caseListRetractedC2SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "2000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Retracted, "2000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "2000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRetractedC2SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng

            //List<Case> caseListRetractedC3SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "3000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Retracted, "3000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "3000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRetractedC3SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng

            //List<Case> caseListRetractedC4SortCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDoneAt = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "4000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Retracted, "4000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Retracted, "4000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseListRetractedC4SortWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region aCase sorting Descending

            #region aCase1 sorting Descending

            //List<Case> caseListRetractedC1SortDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Retracted, "1000", true, Constants.CaseSortParameters.DoneAt,
                timeZoneInfo);
            //List<Case> caseListRetractedC1SortDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRetractedC1SortDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRetractedC1SortDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRetractedC1SortDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRetractedC1SortDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRetractedC1SortDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRetractedC1SortDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRetractedC1SortDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRetractedC1SortDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRetractedC1SortDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRetractedC1SortDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDescendingStatus = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Retracted, "1000", true, Constants.CaseSortParameters.Status,
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDescendingUnitId = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Retracted, "1000", true, Constants.CaseSortParameters.UnitId,
                timeZoneInfo);
            //List<Case> caseListRetractedC1SortDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting Descending

            //List<Case> caseListRetractedC2SortDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Retracted, "2000", true, Constants.CaseSortParameters.DoneAt,
                timeZoneInfo);
            //List<Case> caseListRetractedC2SortDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC2SortDescendingDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRetractedC2SortDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRetractedC2SortDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRetractedC2SortDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRetractedC2SortDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRetractedC2SortDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRetractedC2SortDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRetractedC2SortDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRetractedC2SortDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListRetractedC2SortDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDescendingStatus = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Retracted, "2000", true, Constants.CaseSortParameters.Status,
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDescendingUnitId = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Retracted, "2000", true, Constants.CaseSortParameters.UnitId,
                timeZoneInfo);
            //List<Case> caseListRetractedC2SortDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting Descending

            //List<Case> caseListRetractedC3SortDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Retracted, "3000", true, Constants.CaseSortParameters.DoneAt,
                timeZoneInfo);
            //List<Case> caseListRetractedC3SortDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRetractedC3SortDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRetractedC3SortDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRetractedC3SortDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRetractedC3SortDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRetractedC3SortDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRetractedC3SortDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRetractedC3SortDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRetractedC3SortDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRetractedC3SortDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC3SortDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDescendingStatus = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Retracted, "3000", true, Constants.CaseSortParameters.Status,
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDescendingUnitId = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Retracted, "3000", true, Constants.CaseSortParameters.UnitId,
                timeZoneInfo);
            //List<Case> caseListRetractedC3SortDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting Descending

            //List<Case> caseListRetractedC4SortDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDescendingDoneAt = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Retracted, "4000", true, Constants.CaseSortParameters.DoneAt,
                timeZoneInfo);
            //List<Case> caseListRetractedC4SortDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListRetractedC4SortDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListRetractedC4SortDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListRetractedC4SortDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListRetractedC4SortDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListRetractedC4SortDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListRetractedC4SortDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListRetractedC4SortDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListRetractedC4SortDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListRetractedC4SortDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC4SortDescendingSiteName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDescendingStatus = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Retracted, "4000", true, Constants.CaseSortParameters.Status,
                timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDescendingUnitId = await sut.CaseReadAll(cl1.Id, null,
                null, Constants.WorkflowStates.Retracted, "4000", true, Constants.CaseSortParameters.UnitId,
                timeZoneInfo);
            //List<Case> caseListRetractedC4SortDescendingWorkerName = await sut.CaseReadAll(cl1.Id, null, null, Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region aCase sorting ascending w. Dt

            #region aCase1 sorting ascendng w. Dt

            //List<Case> caseDtListRetractedC1SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1000",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1000",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseDtListRetractedC1SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting ascendng w. Dt

            //List<Case> caseDtListRetractedC2SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2000",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2000",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseDtListRetractedC2SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting ascendng w. Dt

            //List<Case> caseDtListRetractedC3SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3000",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3000",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseDtListRetractedC3SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting ascendng w. Dt

            //List<Case> caseDtListRetractedC4SortDtCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDtDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4000",
                false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDtUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4000",
                false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseDtListRetractedC4SortDtWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", false, Constants.CaseSortParameters.WorkerName);

            #endregion

            #endregion

            #region aCase sorting Descending w. Dt

            #region aCase1 sorting Descending w. Dt

            //List<Case> caseDtListRetractedC1SortDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1000",
                true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseDtListRetractedC1SortDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseDtListRetractedC1SortDtDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListRetractedC1SortDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListRetractedC1SortDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListRetractedC1SortDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListRetractedC1SortDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListRetractedC1SortDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListRetractedC1SortDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListRetractedC1SortDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListRetractedC1SortDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseDtListRetractedC1SortvDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1000",
                true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC1SortDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1000",
                true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseDtListRetractedC1SortDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "1", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase2 sorting Descending w. Dt

            //List<Case> caseDtListRetractedC2SortDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2000",
                true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseDtListRetractedC2SortDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC2SortDtDescendingDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListRetractedC2SortDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListRetractedC2SortDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListRetractedC2SortDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListRetractedC2SortDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListRetractedC2SortDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListRetractedC2SortDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListRetractedC2SortDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListRetractedC2SortDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseDtListRetractedC2SortDtDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2000",
                true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC2SortDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2000",
                true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseDtListRetractedC2SortDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Retracted, "2", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase3 sorting Descending w. Dt

            //List<Case> caseDtListRetractedC3SortDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3000",
                true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseDtListRetractedC3SortDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseDtListRetractedC3SortDtDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListRetractedC3SortDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListRetractedC3SortDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListRetractedC3SortDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListRetractedC3SortDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListRetractedC3SortDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListRetractedC3SortDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListRetractedC3SortDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListRetractedC3SortDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC3SortDtDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3000",
                true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC3SortDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3000",
                true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseDtListRetractedC3SortDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "3", true, Constants.CaseSortParameters.WorkerName);

            #endregion

            #region aCase4 sorting Descending w. Dt

            //List<Case> caseDtListRetractedC4SortDtDescendingCreatedAt = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.CreatedAt);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDtDescendingDoneAt = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4000",
                true, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
            //List<Case> caseDtListRetractedC4SortDtDescendingFieldValue1 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseDtListRetractedC4SortDtDescendingFieldValue2 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListRetractedC4SortDtDescendingFieldValue3 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListRetractedC4SortDtDescendingFieldValue4 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListRetractedC4SortDtDescendingFieldValue5 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListRetractedC4SortDtDescendingFieldValue6 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListRetractedC4SortDtDescendingFieldValue7 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListRetractedC4SortDtDescendingFieldValue8 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListRetractedC4SortDtDescendingFieldValue9 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListRetractedC4SortDtDescendingFieldValue10 = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseDtListRetractedC4SortDtDescendingSiteName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.SiteName);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDtDescendingStatus = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4000",
                true, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListRetractedC4SortDtDescendingUnitId = await sut.CaseReadAll(cl1.Id,
                DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4000",
                true, Constants.CaseSortParameters.UnitId, timeZoneInfo);
            //List<Case> caseDtListRetractedC4SortDtDescendingWorkerName = await sut.CaseReadAll(cl1.Id, DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-6), Constants.WorkflowStates.Retracted, "4", true, Constants.CaseSortParameters.WorkerName);

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
            Assert.That(caseListDoneAt.Count, Is.EqualTo(4));
            Assert.That(caseListDoneAt[1].CaseType, Is.EqualTo(aCase1.Type));
            Assert.That(caseListDoneAt[1].CaseUId, Is.EqualTo(aCase1.CaseUid));
            Assert.That(caseListDoneAt[1].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
            Assert.That(caseListDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
            Assert.That(caseListDoneAt[1].Custom, Is.EqualTo(aCase1.Custom));
            Assert.That(caseListDoneAt[1].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
            Assert.That(caseListDoneAt[1].Id, Is.EqualTo(aCase1.Id));
            Assert.That(caseListDoneAt[1].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
            Assert.That(caseListDoneAt[1].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
            Assert.That(caseListDoneAt[1].SiteName, Is.EqualTo(aCase1.Site.Name));
            Assert.That(caseListDoneAt[1].Status, Is.EqualTo(aCase1.Status));
            Assert.That(caseListDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListDoneAt[1].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
            //            Assert.AreEqual(c1_ua.ToString(), caseListDoneAt[1].UpdatedAt.ToString());
            Assert.That(caseListDoneAt[1].Version, Is.EqualTo(aCase1.Version));
            Assert.That(caseListDoneAt[1].WorkerName, Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
            Assert.That(caseListDoneAt[1].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

            #endregion

            #region caseListDoneAt aCase2

            Assert.That(caseListDoneAt[3].CaseType, Is.EqualTo(aCase2.Type));
            Assert.That(caseListDoneAt[3].CaseUId, Is.EqualTo(aCase2.CaseUid));
            Assert.That(caseListDoneAt[3].CheckUIid, Is.EqualTo(aCase2.MicrotingCheckUid));
            Assert.That(caseListDoneAt[3].CreatedAt.ToString(), Is.EqualTo(c2_ca.ToString()));
            Assert.That(caseListDoneAt[3].Custom, Is.EqualTo(aCase2.Custom));
            Assert.That(caseListDoneAt[3].DoneAt.ToString(), Is.EqualTo(c2_da.ToString()));
            Assert.That(caseListDoneAt[3].Id, Is.EqualTo(aCase2.Id));
            Assert.That(caseListDoneAt[3].MicrotingUId, Is.EqualTo(aCase2.MicrotingUid));
            Assert.That(caseListDoneAt[3].SiteId, Is.EqualTo(aCase2.Site.MicrotingUid));
            Assert.That(caseListDoneAt[3].SiteName, Is.EqualTo(aCase2.Site.Name));
            Assert.That(caseListDoneAt[3].Status, Is.EqualTo(aCase2.Status));
            Assert.That(caseListDoneAt[3].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListDoneAt[3].UnitId, Is.EqualTo(aCase2.Unit.MicrotingUid));
            //            Assert.AreEqual(c2_ua.ToString(), caseListDoneAt[3].UpdatedAt.ToString());
            Assert.That(caseListDoneAt[3].Version, Is.EqualTo(aCase2.Version));
            Assert.That(caseListDoneAt[3].WorkerName, Is.EqualTo(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName));
            Assert.That(caseListDoneAt[3].WorkflowState, Is.EqualTo(aCase2.WorkflowState));

            #endregion

            #region caseListDoneAt aCase3

            Assert.That(caseListDoneAt[0].CaseType, Is.EqualTo(aCase3.Type));
            Assert.That(caseListDoneAt[0].CaseUId, Is.EqualTo(aCase3.CaseUid));
            Assert.That(caseListDoneAt[0].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
            Assert.That(caseListDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
            Assert.That(caseListDoneAt[0].Custom, Is.EqualTo(aCase3.Custom));
            Assert.That(caseListDoneAt[0].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
            Assert.That(caseListDoneAt[0].Id, Is.EqualTo(aCase3.Id));
            Assert.That(caseListDoneAt[0].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
            Assert.That(caseListDoneAt[0].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
            Assert.That(caseListDoneAt[0].SiteName, Is.EqualTo(aCase3.Site.Name));
            Assert.That(caseListDoneAt[0].Status, Is.EqualTo(aCase3.Status));
            Assert.That(caseListDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListDoneAt[0].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
            //            Assert.AreEqual(c3_ua.ToString(), caseListDoneAt[0].UpdatedAt.ToString());
            Assert.That(caseListDoneAt[0].Version, Is.EqualTo(aCase3.Version));
            Assert.That(caseListDoneAt[0].WorkerName, Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
            Assert.That(caseListDoneAt[0].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

            #endregion

            #region caseListDoneAt aCase4

            Assert.That(caseListDoneAt[2].CaseType, Is.EqualTo(aCase4.Type));
            Assert.That(caseListDoneAt[2].CaseUId, Is.EqualTo(aCase4.CaseUid));
            Assert.That(caseListDoneAt[2].CheckUIid, Is.EqualTo(aCase4.MicrotingCheckUid));
            Assert.That(caseListDoneAt[2].CreatedAt.ToString(), Is.EqualTo(c4_ca.ToString()));
            Assert.That(caseListDoneAt[2].Custom, Is.EqualTo(aCase4.Custom));
            Assert.That(caseListDoneAt[2].DoneAt.ToString(), Is.EqualTo(c4_da.ToString()));
            Assert.That(caseListDoneAt[2].Id, Is.EqualTo(aCase4.Id));
            Assert.That(caseListDoneAt[2].MicrotingUId, Is.EqualTo(aCase4.MicrotingUid));
            Assert.That(caseListDoneAt[2].SiteId, Is.EqualTo(aCase4.Site.MicrotingUid));
            Assert.That(caseListDoneAt[2].SiteName, Is.EqualTo(aCase4.Site.Name));
            Assert.That(caseListDoneAt[2].Status, Is.EqualTo(aCase4.Status));
            Assert.That(caseListDoneAt[2].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListDoneAt[2].UnitId, Is.EqualTo(aCase4.Unit.MicrotingUid));
            //            Assert.AreEqual(c4_ua.ToString(), caseListDoneAt[2].UpdatedAt.ToString());
            Assert.That(caseListDoneAt[2].Version, Is.EqualTo(aCase4.Version));
            Assert.That(caseListDoneAt[2].WorkerName, Is.EqualTo(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName));
            Assert.That(caseListDoneAt[2].WorkflowState, Is.EqualTo(aCase4.WorkflowState));

            #endregion

            #endregion

            #region caseListStatus Def Sort Asc

            #region caseListStatus aCase1

            Assert.NotNull(caseListStatus);
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

            Assert.NotNull(caseListUnitId);
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

            #region Def Sort Des

            #region caseListDescendingDoneAt Def Sort Des

            #region caseListDescendingDoneAt aCase1

            Assert.NotNull(caseListDescendingDoneAt);
            Assert.That(caseListDescendingDoneAt.Count, Is.EqualTo(4));
            Assert.That(caseListDescendingDoneAt[2].CaseType, Is.EqualTo(aCase1.Type));
            Assert.That(caseListDescendingDoneAt[2].CaseUId, Is.EqualTo(aCase1.CaseUid));
            Assert.That(caseListDescendingDoneAt[2].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
            Assert.That(caseListDescendingDoneAt[2].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
            Assert.That(caseListDescendingDoneAt[2].Custom, Is.EqualTo(aCase1.Custom));
            Assert.That(caseListDescendingDoneAt[2].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
            Assert.That(caseListDescendingDoneAt[2].Id, Is.EqualTo(aCase1.Id));
            Assert.That(caseListDescendingDoneAt[2].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
            Assert.That(caseListDescendingDoneAt[2].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
            Assert.That(caseListDescendingDoneAt[2].SiteName, Is.EqualTo(aCase1.Site.Name));
            Assert.That(caseListDescendingDoneAt[2].Status, Is.EqualTo(aCase1.Status));
            Assert.That(caseListDescendingDoneAt[2].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListDescendingDoneAt[2].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
            //            Assert.AreEqual(c1_ua.ToString(), caseListDescendingDoneAt[2].UpdatedAt.ToString());
            Assert.That(caseListDescendingDoneAt[2].Version, Is.EqualTo(aCase1.Version));
            Assert.That(caseListDescendingDoneAt[2].WorkerName,
                Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
            Assert.That(caseListDescendingDoneAt[2].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

            #endregion

            #region caseListDescendingDoneAt aCase2

            Assert.That(caseListDescendingDoneAt[0].CaseType, Is.EqualTo(aCase2.Type));
            Assert.That(caseListDescendingDoneAt[0].CaseUId, Is.EqualTo(aCase2.CaseUid));
            Assert.That(caseListDescendingDoneAt[0].CheckUIid, Is.EqualTo(aCase2.MicrotingCheckUid));
            Assert.That(caseListDescendingDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c2_ca.ToString()));
            Assert.That(caseListDescendingDoneAt[0].Custom, Is.EqualTo(aCase2.Custom));
            Assert.That(caseListDescendingDoneAt[0].DoneAt.ToString(), Is.EqualTo(c2_da.ToString()));
            Assert.That(caseListDescendingDoneAt[0].Id, Is.EqualTo(aCase2.Id));
            Assert.That(caseListDescendingDoneAt[0].MicrotingUId, Is.EqualTo(aCase2.MicrotingUid));
            Assert.That(caseListDescendingDoneAt[0].SiteId, Is.EqualTo(aCase2.Site.MicrotingUid));
            Assert.That(caseListDescendingDoneAt[0].SiteName, Is.EqualTo(aCase2.Site.Name));
            Assert.That(caseListDescendingDoneAt[0].Status, Is.EqualTo(aCase2.Status));
            Assert.That(caseListDescendingDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListDescendingDoneAt[0].UnitId, Is.EqualTo(aCase2.Unit.MicrotingUid));
            //            Assert.AreEqual(c2_ua.ToString(), caseListDescendingDoneAt[0].UpdatedAt.ToString());
            Assert.That(caseListDescendingDoneAt[0].Version, Is.EqualTo(aCase2.Version));
            Assert.That(caseListDescendingDoneAt[0].WorkerName,
                Is.EqualTo(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName));
            Assert.That(caseListDescendingDoneAt[0].WorkflowState, Is.EqualTo(aCase2.WorkflowState));

            #endregion

            #region caseListDescendingDoneAt aCase3

            Assert.That(caseListDescendingDoneAt[3].CaseType, Is.EqualTo(aCase3.Type));
            Assert.That(caseListDescendingDoneAt[3].CaseUId, Is.EqualTo(aCase3.CaseUid));
            Assert.That(caseListDescendingDoneAt[3].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
            Assert.That(caseListDescendingDoneAt[3].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
            Assert.That(caseListDescendingDoneAt[3].Custom, Is.EqualTo(aCase3.Custom));
            Assert.That(caseListDescendingDoneAt[3].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
            Assert.That(caseListDescendingDoneAt[3].Id, Is.EqualTo(aCase3.Id));
            Assert.That(caseListDescendingDoneAt[3].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
            Assert.That(caseListDescendingDoneAt[3].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
            Assert.That(caseListDescendingDoneAt[3].SiteName, Is.EqualTo(aCase3.Site.Name));
            Assert.That(caseListDescendingDoneAt[3].Status, Is.EqualTo(aCase3.Status));
            Assert.That(caseListDescendingDoneAt[3].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListDescendingDoneAt[3].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
            //            Assert.AreEqual(c3_ua.ToString(), caseListDescendingDoneAt[3].UpdatedAt.ToString());
            Assert.That(caseListDescendingDoneAt[3].Version, Is.EqualTo(aCase3.Version));
            Assert.That(caseListDescendingDoneAt[3].WorkerName,
                Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
            Assert.That(caseListDescendingDoneAt[3].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

            #endregion

            #region caseListDescendingDoneAt aCase4

            Assert.That(caseListDescendingDoneAt[1].CaseType, Is.EqualTo(aCase4.Type));
            Assert.That(caseListDescendingDoneAt[1].CaseUId, Is.EqualTo(aCase4.CaseUid));
            Assert.That(caseListDescendingDoneAt[1].CheckUIid, Is.EqualTo(aCase4.MicrotingCheckUid));
            Assert.That(caseListDescendingDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c4_ca.ToString()));
            Assert.That(caseListDescendingDoneAt[1].Custom, Is.EqualTo(aCase4.Custom));
            Assert.That(caseListDescendingDoneAt[1].DoneAt.ToString(), Is.EqualTo(c4_da.ToString()));
            Assert.That(caseListDescendingDoneAt[1].Id, Is.EqualTo(aCase4.Id));
            Assert.That(caseListDescendingDoneAt[1].MicrotingUId, Is.EqualTo(aCase4.MicrotingUid));
            Assert.That(caseListDescendingDoneAt[1].SiteId, Is.EqualTo(aCase4.Site.MicrotingUid));
            Assert.That(caseListDescendingDoneAt[1].SiteName, Is.EqualTo(aCase4.Site.Name));
            Assert.That(caseListDescendingDoneAt[1].Status, Is.EqualTo(aCase4.Status));
            Assert.That(caseListDescendingDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListDescendingDoneAt[1].UnitId, Is.EqualTo(aCase4.Unit.MicrotingUid));
            //            Assert.AreEqual(c4_ua.ToString(), caseListDescendingDoneAt[1].UpdatedAt.ToString());
            Assert.That(caseListDescendingDoneAt[1].Version, Is.EqualTo(aCase4.Version));
            Assert.That(caseListDescendingDoneAt[1].WorkerName,
                Is.EqualTo(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName));
            Assert.That(caseListDescendingDoneAt[1].WorkflowState, Is.EqualTo(aCase4.WorkflowState));

            #endregion

            #endregion

            #region caseListDescendingStatus Def Sort Des

            #region caseListDescendingStatus aCase1

            Assert.NotNull(caseListDescendingStatus);
            Assert.That(caseListDescendingStatus.Count, Is.EqualTo(4));
            Assert.That(caseListDescendingStatus[3].CaseType, Is.EqualTo(aCase1.Type));
            Assert.That(caseListDescendingStatus[3].CaseUId, Is.EqualTo(aCase1.CaseUid));
            Assert.That(caseListDescendingStatus[3].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
            Assert.That(caseListDescendingStatus[3].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
            Assert.That(caseListDescendingStatus[3].Custom, Is.EqualTo(aCase1.Custom));
            Assert.That(caseListDescendingStatus[3].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
            Assert.That(caseListDescendingStatus[3].Id, Is.EqualTo(aCase1.Id));
            Assert.That(caseListDescendingStatus[3].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
            Assert.That(caseListDescendingStatus[3].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
            Assert.That(caseListDescendingStatus[3].SiteName, Is.EqualTo(aCase1.Site.Name));
            Assert.That(caseListDescendingStatus[3].Status, Is.EqualTo(aCase1.Status));
            Assert.That(caseListDescendingStatus[3].TemplatId, Is.EqualTo(aCase1.CheckListId));
            Assert.That(caseListDescendingStatus[3].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
            //            Assert.AreEqual(c1_ua.ToString(), caseListDescendingStatus[3].UpdatedAt.ToString());
            Assert.That(caseListDescendingStatus[3].Version, Is.EqualTo(aCase1.Version));
            Assert.That(caseListDescendingStatus[3].WorkerName,
                Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
            Assert.That(caseListDescendingStatus[3].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

            #endregion

            #region caseListDescendingStatus aCase2

            Assert.That(caseListDescendingStatus[2].CaseType, Is.EqualTo(aCase2.Type));
            Assert.That(caseListDescendingStatus[2].CaseUId, Is.EqualTo(aCase2.CaseUid));
            Assert.That(caseListDescendingStatus[2].CheckUIid, Is.EqualTo(aCase2.MicrotingCheckUid));
            Assert.That(caseListDescendingStatus[2].CreatedAt.ToString(), Is.EqualTo(c2_ca.ToString()));
            Assert.That(caseListDescendingStatus[2].Custom, Is.EqualTo(aCase2.Custom));
            Assert.That(caseListDescendingStatus[2].DoneAt.ToString(), Is.EqualTo(c2_da.ToString()));
            Assert.That(caseListDescendingStatus[2].Id, Is.EqualTo(aCase2.Id));
            Assert.That(caseListDescendingStatus[2].MicrotingUId, Is.EqualTo(aCase2.MicrotingUid));
            Assert.That(caseListDescendingStatus[2].SiteId, Is.EqualTo(aCase2.Site.MicrotingUid));
            Assert.That(caseListDescendingStatus[2].SiteName, Is.EqualTo(aCase2.Site.Name));
            Assert.That(caseListDescendingStatus[2].Status, Is.EqualTo(aCase2.Status));
            Assert.That(caseListDescendingStatus[2].TemplatId, Is.EqualTo(aCase2.CheckListId));
            Assert.That(caseListDescendingStatus[2].UnitId, Is.EqualTo(aCase2.Unit.MicrotingUid));
            //            Assert.AreEqual(c2_ua.ToString(), caseListDescendingStatus[2].UpdatedAt.ToString());
            Assert.That(caseListDescendingStatus[2].Version, Is.EqualTo(aCase2.Version));
            Assert.That(caseListDescendingStatus[2].WorkerName,
                Is.EqualTo(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName));
            Assert.That(caseListDescendingStatus[2].WorkflowState, Is.EqualTo(aCase2.WorkflowState));

            #endregion

            #region caseListDescendingStatus aCase3

            Assert.That(caseListDescendingStatus[1].CaseType, Is.EqualTo(aCase3.Type));
            Assert.That(caseListDescendingStatus[1].CaseUId, Is.EqualTo(aCase3.CaseUid));
            Assert.That(caseListDescendingStatus[1].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
            Assert.That(caseListDescendingStatus[1].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
            Assert.That(caseListDescendingStatus[1].Custom, Is.EqualTo(aCase3.Custom));
            Assert.That(caseListDescendingStatus[1].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
            Assert.That(caseListDescendingStatus[1].Id, Is.EqualTo(aCase3.Id));
            Assert.That(caseListDescendingStatus[1].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
            Assert.That(caseListDescendingStatus[1].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
            Assert.That(caseListDescendingStatus[1].SiteName, Is.EqualTo(aCase3.Site.Name));
            Assert.That(caseListDescendingStatus[1].Status, Is.EqualTo(aCase3.Status));
            Assert.That(caseListDescendingStatus[1].TemplatId, Is.EqualTo(aCase3.CheckListId));
            Assert.That(caseListDescendingStatus[1].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
            //            Assert.AreEqual(c3_ua.ToString(), caseListDescendingStatus[1].UpdatedAt.ToString());
            Assert.That(caseListDescendingStatus[1].Version, Is.EqualTo(aCase3.Version));
            Assert.That(caseListDescendingStatus[1].WorkerName,
                Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
            Assert.That(caseListDescendingStatus[1].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

            #endregion

            #region caseListDescendingStatus aCase4

            Assert.That(caseListDescendingStatus[0].CaseType, Is.EqualTo(aCase4.Type));
            Assert.That(caseListDescendingStatus[0].CaseUId, Is.EqualTo(aCase4.CaseUid));
            Assert.That(caseListDescendingStatus[0].CheckUIid, Is.EqualTo(aCase4.MicrotingCheckUid));
            Assert.That(caseListDescendingStatus[0].CreatedAt.ToString(), Is.EqualTo(c4_ca.ToString()));
            Assert.That(caseListDescendingStatus[0].Custom, Is.EqualTo(aCase4.Custom));
            Assert.That(caseListDescendingStatus[0].DoneAt.ToString(), Is.EqualTo(c4_da.ToString()));
            Assert.That(caseListDescendingStatus[0].Id, Is.EqualTo(aCase4.Id));
            Assert.That(caseListDescendingStatus[0].MicrotingUId, Is.EqualTo(aCase4.MicrotingUid));
            Assert.That(caseListDescendingStatus[0].SiteId, Is.EqualTo(aCase4.Site.MicrotingUid));
            Assert.That(caseListDescendingStatus[0].SiteName, Is.EqualTo(aCase4.Site.Name));
            Assert.That(caseListDescendingStatus[0].Status, Is.EqualTo(aCase4.Status));
            Assert.That(caseListDescendingStatus[0].TemplatId, Is.EqualTo(aCase4.CheckListId));
            Assert.That(caseListDescendingStatus[0].UnitId, Is.EqualTo(aCase4.Unit.MicrotingUid));
            //            Assert.AreEqual(c4_ua.ToString(), caseListDescendingStatus[0].UpdatedAt.ToString());
            Assert.That(caseListDescendingStatus[0].Version, Is.EqualTo(aCase4.Version));
            Assert.That(caseListDescendingStatus[0].WorkerName,
                Is.EqualTo(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName));
            Assert.That(caseListDescendingStatus[0].WorkflowState, Is.EqualTo(aCase4.WorkflowState));

            #endregion

            #endregion

            #region caseListDescendingUnitId Def Sort Des

            #region caseListDescendingUnitId aCase1

            Assert.NotNull(caseListDescendingUnitId);
            Assert.That(caseListDescendingUnitId.Count, Is.EqualTo(4));
            Assert.That(caseListDescendingUnitId[3].CaseType, Is.EqualTo(aCase1.Type));
            Assert.That(caseListDescendingUnitId[3].CaseUId, Is.EqualTo(aCase1.CaseUid));
            Assert.That(caseListDescendingUnitId[3].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
            Assert.That(caseListDescendingUnitId[3].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
            Assert.That(caseListDescendingUnitId[3].Custom, Is.EqualTo(aCase1.Custom));
            Assert.That(caseListDescendingUnitId[3].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
            Assert.That(caseListDescendingUnitId[3].Id, Is.EqualTo(aCase1.Id));
            Assert.That(caseListDescendingUnitId[3].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
            Assert.That(caseListDescendingUnitId[3].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
            Assert.That(caseListDescendingUnitId[3].SiteName, Is.EqualTo(aCase1.Site.Name));
            Assert.That(caseListDescendingUnitId[3].Status, Is.EqualTo(aCase1.Status));
            Assert.That(caseListDescendingUnitId[3].TemplatId, Is.EqualTo(aCase1.CheckListId));
            Assert.That(caseListDescendingUnitId[3].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
            //            Assert.AreEqual(c1_ua.ToString(), caseListDescendingUnitId[3].UpdatedAt.ToString());
            Assert.That(caseListDescendingUnitId[3].Version, Is.EqualTo(aCase1.Version));
            Assert.That(caseListDescendingUnitId[3].WorkerName,
                Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
            Assert.That(caseListDescendingUnitId[3].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

            #endregion

            #region caseListDescendingUnitId aCase2

            Assert.That(caseListDescendingUnitId[2].CaseType, Is.EqualTo(aCase2.Type));
            Assert.That(caseListDescendingUnitId[2].CaseUId, Is.EqualTo(aCase2.CaseUid));
            Assert.That(caseListDescendingUnitId[2].CheckUIid, Is.EqualTo(aCase2.MicrotingCheckUid));
            Assert.That(caseListDescendingUnitId[2].CreatedAt.ToString(), Is.EqualTo(c2_ca.ToString()));
            Assert.That(caseListDescendingUnitId[2].Custom, Is.EqualTo(aCase2.Custom));
            Assert.That(caseListDescendingUnitId[2].DoneAt.ToString(), Is.EqualTo(c2_da.ToString()));
            Assert.That(caseListDescendingUnitId[2].Id, Is.EqualTo(aCase2.Id));
            Assert.That(caseListDescendingUnitId[2].MicrotingUId, Is.EqualTo(aCase2.MicrotingUid));
            Assert.That(caseListDescendingUnitId[2].SiteId, Is.EqualTo(aCase2.Site.MicrotingUid));
            Assert.That(caseListDescendingUnitId[2].SiteName, Is.EqualTo(aCase2.Site.Name));
            Assert.That(caseListDescendingUnitId[2].Status, Is.EqualTo(aCase2.Status));
            Assert.That(caseListDescendingUnitId[2].TemplatId, Is.EqualTo(aCase2.CheckListId));
            Assert.That(caseListDescendingUnitId[2].UnitId, Is.EqualTo(aCase2.Unit.MicrotingUid));
            //            Assert.AreEqual(c2_ua.ToString(), caseListDescendingUnitId[2].UpdatedAt.ToString());
            Assert.That(caseListDescendingUnitId[2].Version, Is.EqualTo(aCase2.Version));
            Assert.That(caseListDescendingUnitId[2].WorkerName,
                Is.EqualTo(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName));
            Assert.That(caseListDescendingUnitId[2].WorkflowState, Is.EqualTo(aCase2.WorkflowState));

            #endregion

            #region caseListDescendingUnitId aCase3

            Assert.That(caseListDescendingUnitId[1].CaseType, Is.EqualTo(aCase3.Type));
            Assert.That(caseListDescendingUnitId[1].CaseUId, Is.EqualTo(aCase3.CaseUid));
            Assert.That(caseListDescendingUnitId[1].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
            Assert.That(caseListDescendingUnitId[1].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
            Assert.That(caseListDescendingUnitId[1].Custom, Is.EqualTo(aCase3.Custom));
            Assert.That(caseListDescendingUnitId[1].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
            Assert.That(caseListDescendingUnitId[1].Id, Is.EqualTo(aCase3.Id));
            Assert.That(caseListDescendingUnitId[1].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
            Assert.That(caseListDescendingUnitId[1].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
            Assert.That(caseListDescendingUnitId[1].SiteName, Is.EqualTo(aCase3.Site.Name));
            Assert.That(caseListDescendingUnitId[1].Status, Is.EqualTo(aCase3.Status));
            Assert.That(caseListDescendingUnitId[1].TemplatId, Is.EqualTo(aCase3.CheckListId));
            Assert.That(caseListDescendingUnitId[1].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
            //            Assert.AreEqual(c3_ua.ToString(), caseListDescendingUnitId[1].UpdatedAt.ToString());
            Assert.That(caseListDescendingUnitId[1].Version, Is.EqualTo(aCase3.Version));
            Assert.That(caseListDescendingUnitId[1].WorkerName,
                Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
            Assert.That(caseListDescendingUnitId[1].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

            #endregion

            #region caseListDescendingUnitId aCase4

            Assert.That(caseListDescendingUnitId[0].CaseType, Is.EqualTo(aCase4.Type));
            Assert.That(caseListDescendingUnitId[0].CaseUId, Is.EqualTo(aCase4.CaseUid));
            Assert.That(caseListDescendingUnitId[0].CheckUIid, Is.EqualTo(aCase4.MicrotingCheckUid));
            Assert.That(caseListDescendingUnitId[0].CreatedAt.ToString(), Is.EqualTo(c4_ca.ToString()));
            Assert.That(caseListDescendingUnitId[0].Custom, Is.EqualTo(aCase4.Custom));
            Assert.That(caseListDescendingUnitId[0].DoneAt.ToString(), Is.EqualTo(c4_da.ToString()));
            Assert.That(caseListDescendingUnitId[0].Id, Is.EqualTo(aCase4.Id));
            Assert.That(caseListDescendingUnitId[0].MicrotingUId, Is.EqualTo(aCase4.MicrotingUid));
            Assert.That(caseListDescendingUnitId[0].SiteId, Is.EqualTo(aCase4.Site.MicrotingUid));
            Assert.That(caseListDescendingUnitId[0].SiteName, Is.EqualTo(aCase4.Site.Name));
            Assert.That(caseListDescendingUnitId[0].Status, Is.EqualTo(aCase4.Status));
            Assert.That(caseListDescendingUnitId[0].TemplatId, Is.EqualTo(aCase4.CheckListId));
            Assert.That(caseListDescendingUnitId[0].UnitId, Is.EqualTo(aCase4.Unit.MicrotingUid));
            //            Assert.AreEqual(c4_ua.ToString(), caseListDescendingUnitId[0].UpdatedAt.ToString());
            Assert.That(caseListDescendingUnitId[0].Version, Is.EqualTo(aCase4.Version));
            Assert.That(caseListDescendingUnitId[0].WorkerName,
                Is.EqualTo(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName));
            Assert.That(caseListDescendingUnitId[0].WorkflowState, Is.EqualTo(aCase4.WorkflowState));

            #endregion

            #endregion

            #endregion

            #region Def Sort Asc w. DateTime

            #region caseListDtDoneAt Def Sort Asc w. DateTime

            #region caseListDtDoneAt aCase1

            Assert.NotNull(caseListDtDoneAt);
            Assert.That(caseListDtDoneAt.Count, Is.EqualTo(2));
            Assert.That(caseListDtDoneAt[1].CaseType, Is.EqualTo(aCase1.Type));
            Assert.That(caseListDtDoneAt[1].CaseUId, Is.EqualTo(aCase1.CaseUid));
            Assert.That(caseListDtDoneAt[1].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
            Assert.That(caseListDtDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
            Assert.That(caseListDtDoneAt[1].Custom, Is.EqualTo(aCase1.Custom));
            Assert.That(caseListDtDoneAt[1].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
            Assert.That(caseListDtDoneAt[1].Id, Is.EqualTo(aCase1.Id));
            Assert.That(caseListDtDoneAt[1].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
            Assert.That(caseListDtDoneAt[1].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
            Assert.That(caseListDtDoneAt[1].SiteName, Is.EqualTo(aCase1.Site.Name));
            Assert.That(caseListDtDoneAt[1].Status, Is.EqualTo(aCase1.Status));
            Assert.That(caseListDtDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListDtDoneAt[1].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
            //            Assert.AreEqual(c1_ua.ToString(), caseListDtDoneAt[1].UpdatedAt.ToString());
            Assert.That(caseListDtDoneAt[1].Version, Is.EqualTo(aCase1.Version));
            Assert.That(caseListDtDoneAt[1].WorkerName, Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
            Assert.That(caseListDtDoneAt[1].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

            #endregion

            #region caseListDtDoneAt aCase3

            Assert.That(caseListDtDoneAt[0].CaseType, Is.EqualTo(aCase3.Type));
            Assert.That(caseListDtDoneAt[0].CaseUId, Is.EqualTo(aCase3.CaseUid));
            Assert.That(caseListDtDoneAt[0].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
            Assert.That(caseListDtDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
            Assert.That(caseListDtDoneAt[0].Custom, Is.EqualTo(aCase3.Custom));
            Assert.That(caseListDtDoneAt[0].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
            Assert.That(caseListDtDoneAt[0].Id, Is.EqualTo(aCase3.Id));
            Assert.That(caseListDtDoneAt[0].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
            Assert.That(caseListDtDoneAt[0].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
            Assert.That(caseListDtDoneAt[0].SiteName, Is.EqualTo(aCase3.Site.Name));
            Assert.That(caseListDtDoneAt[0].Status, Is.EqualTo(aCase3.Status));
            Assert.That(caseListDtDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListDtDoneAt[0].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
            //            Assert.AreEqual(c3_ua.ToString(), caseListDtDoneAt[0].UpdatedAt.ToString());
            Assert.That(caseListDtDoneAt[0].Version, Is.EqualTo(aCase3.Version));
            Assert.That(caseListDtDoneAt[0].WorkerName, Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
            Assert.That(caseListDtDoneAt[0].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

            #endregion

            #endregion

            #region caseListDtStatus Def Sort Asc w. DateTime

            #region caseListDtStatus aCase1

            Assert.NotNull(caseListDtStatus);
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

            Assert.NotNull(caseListDtUnitId);
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

            #region Def Sort Des w. DateTime

            #region caseListDtDescendingDoneAt Def Sort Des

            #region caseListDtDescendingDoneAt aCase1

            Assert.NotNull(caseListDtDescendingDoneAt);
            Assert.That(caseListDtDescendingDoneAt.Count, Is.EqualTo(2));
            Assert.That(caseListDtDescendingDoneAt[0].CaseType, Is.EqualTo(aCase1.Type));
            Assert.That(caseListDtDescendingDoneAt[0].CaseUId, Is.EqualTo(aCase1.CaseUid));
            Assert.That(caseListDtDescendingDoneAt[0].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
            Assert.That(caseListDtDescendingDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
            Assert.That(caseListDtDescendingDoneAt[0].Custom, Is.EqualTo(aCase1.Custom));
            Assert.That(caseListDtDescendingDoneAt[0].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
            Assert.That(caseListDtDescendingDoneAt[0].Id, Is.EqualTo(aCase1.Id));
            Assert.That(caseListDtDescendingDoneAt[0].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
            Assert.That(caseListDtDescendingDoneAt[0].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
            Assert.That(caseListDtDescendingDoneAt[0].SiteName, Is.EqualTo(aCase1.Site.Name));
            Assert.That(caseListDtDescendingDoneAt[0].Status, Is.EqualTo(aCase1.Status));
            Assert.That(caseListDtDescendingDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListDtDescendingDoneAt[0].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
            //            Assert.AreEqual(c1_ua.ToString(), caseListDtDescendingDoneAt[0].UpdatedAt.ToString());
            Assert.That(caseListDtDescendingDoneAt[0].Version, Is.EqualTo(aCase1.Version));
            Assert.That(caseListDtDescendingDoneAt[0].WorkerName,
                Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
            Assert.That(caseListDtDescendingDoneAt[0].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

            #endregion

            #region caseListDtDescendingDoneAt aCase4

            Assert.That(caseListDtDescendingDoneAt[1].CaseType, Is.EqualTo(aCase3.Type));
            Assert.That(caseListDtDescendingDoneAt[1].CaseUId, Is.EqualTo(aCase3.CaseUid));
            Assert.That(caseListDtDescendingDoneAt[1].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
            Assert.That(caseListDtDescendingDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
            Assert.That(caseListDtDescendingDoneAt[1].Custom, Is.EqualTo(aCase3.Custom));
            Assert.That(caseListDtDescendingDoneAt[1].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
            Assert.That(caseListDtDescendingDoneAt[1].Id, Is.EqualTo(aCase3.Id));
            Assert.That(caseListDtDescendingDoneAt[1].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
            Assert.That(caseListDtDescendingDoneAt[1].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
            Assert.That(caseListDtDescendingDoneAt[1].SiteName, Is.EqualTo(aCase3.Site.Name));
            Assert.That(caseListDtDescendingDoneAt[1].Status, Is.EqualTo(aCase3.Status));
            Assert.That(caseListDtDescendingDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListDtDescendingDoneAt[1].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
            //            Assert.AreEqual(c3_ua.ToString(), caseListDtDescendingDoneAt[1].UpdatedAt.ToString());
            Assert.That(caseListDtDescendingDoneAt[1].Version, Is.EqualTo(aCase3.Version));
            Assert.That(caseListDtDescendingDoneAt[1].WorkerName,
                Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
            Assert.That(caseListDtDescendingDoneAt[1].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

            #endregion

            #endregion

            #region caseListDtDescendingStatus Def Sort Des

            #region caseListDtDescendingStatus aCase1

            Assert.NotNull(caseListDtDescendingStatus);
            Assert.That(caseListDtDescendingStatus.Count, Is.EqualTo(2));
            Assert.That(caseListDtDescendingStatus[1].CaseType, Is.EqualTo(aCase1.Type));
            Assert.That(caseListDtDescendingStatus[1].CaseUId, Is.EqualTo(aCase1.CaseUid));
            Assert.That(caseListDtDescendingStatus[1].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
            Assert.That(caseListDtDescendingStatus[1].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
            Assert.That(caseListDtDescendingStatus[1].Custom, Is.EqualTo(aCase1.Custom));
            Assert.That(caseListDtDescendingStatus[1].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
            Assert.That(caseListDtDescendingStatus[1].Id, Is.EqualTo(aCase1.Id));
            Assert.That(caseListDtDescendingStatus[1].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
            Assert.That(caseListDtDescendingStatus[1].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
            Assert.That(caseListDtDescendingStatus[1].SiteName, Is.EqualTo(aCase1.Site.Name));
            Assert.That(caseListDtDescendingStatus[1].Status, Is.EqualTo(aCase1.Status));
            Assert.That(caseListDtDescendingStatus[1].TemplatId, Is.EqualTo(aCase1.CheckListId));
            Assert.That(caseListDtDescendingStatus[1].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
            //            Assert.AreEqual(c1_ua.ToString(), caseListDtDescendingStatus[1].UpdatedAt.ToString());
            Assert.That(caseListDtDescendingStatus[1].Version, Is.EqualTo(aCase1.Version));
            Assert.That(caseListDtDescendingStatus[1].WorkerName,
                Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
            Assert.That(caseListDtDescendingStatus[1].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

            #endregion

            #region caseListDtDescendingStatus aCase3

            Assert.That(caseListDtDescendingStatus[0].CaseType, Is.EqualTo(aCase3.Type));
            Assert.That(caseListDtDescendingStatus[0].CaseUId, Is.EqualTo(aCase3.CaseUid));
            Assert.That(caseListDtDescendingStatus[0].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
            Assert.That(caseListDtDescendingStatus[0].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
            Assert.That(caseListDtDescendingStatus[0].Custom, Is.EqualTo(aCase3.Custom));
            Assert.That(caseListDtDescendingStatus[0].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
            Assert.That(caseListDtDescendingStatus[0].Id, Is.EqualTo(aCase3.Id));
            Assert.That(caseListDtDescendingStatus[0].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
            Assert.That(caseListDtDescendingStatus[0].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
            Assert.That(caseListDtDescendingStatus[0].SiteName, Is.EqualTo(aCase3.Site.Name));
            Assert.That(caseListDtDescendingStatus[0].Status, Is.EqualTo(aCase3.Status));
            Assert.That(caseListDtDescendingStatus[0].TemplatId, Is.EqualTo(aCase3.CheckListId));
            Assert.That(caseListDtDescendingStatus[0].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
            //            Assert.AreEqual(c3_ua.ToString(), caseListDtDescendingStatus[0].UpdatedAt.ToString());
            Assert.That(caseListDtDescendingStatus[0].Version, Is.EqualTo(aCase3.Version));
            Assert.That(caseListDtDescendingStatus[0].WorkerName,
                Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
            Assert.That(caseListDtDescendingStatus[0].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

            #endregion

            #endregion

            #region caseListDtDescendingUnitId Def Sort Des

            #region caseListDtDescendingUnitId aCase3

            Assert.NotNull(caseListDtDescendingUnitId);
            Assert.That(caseListDtDescendingUnitId.Count, Is.EqualTo(2));

            Assert.That(caseListDtDescendingUnitId[1].CaseType, Is.EqualTo(aCase1.Type));
            Assert.That(caseListDtDescendingUnitId[1].CaseUId, Is.EqualTo(aCase1.CaseUid));
            Assert.That(caseListDtDescendingUnitId[1].CheckUIid, Is.EqualTo(aCase1.MicrotingCheckUid));
            Assert.That(caseListDtDescendingUnitId[1].CreatedAt.ToString(), Is.EqualTo(c1_ca.ToString()));
            Assert.That(caseListDtDescendingUnitId[1].Custom, Is.EqualTo(aCase1.Custom));
            Assert.That(caseListDtDescendingUnitId[1].DoneAt.ToString(), Is.EqualTo(c1_da.ToString()));
            Assert.That(caseListDtDescendingUnitId[1].Id, Is.EqualTo(aCase1.Id));
            Assert.That(caseListDtDescendingUnitId[1].MicrotingUId, Is.EqualTo(aCase1.MicrotingUid));
            Assert.That(caseListDtDescendingUnitId[1].SiteId, Is.EqualTo(aCase1.Site.MicrotingUid));
            Assert.That(caseListDtDescendingUnitId[1].SiteName, Is.EqualTo(aCase1.Site.Name));
            Assert.That(caseListDtDescendingUnitId[1].Status, Is.EqualTo(aCase1.Status));
            Assert.That(caseListDtDescendingUnitId[1].TemplatId, Is.EqualTo(aCase1.CheckListId));
            Assert.That(caseListDtDescendingUnitId[1].UnitId, Is.EqualTo(aCase1.Unit.MicrotingUid));
            //            Assert.AreEqual(c1_ua.ToString(), caseListDtDescendingUnitId[1].UpdatedAt.ToString());
            Assert.That(caseListDtDescendingUnitId[1].Version, Is.EqualTo(aCase1.Version));
            Assert.That(caseListDtDescendingUnitId[1].WorkerName,
                Is.EqualTo(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName));
            Assert.That(caseListDtDescendingUnitId[1].WorkflowState, Is.EqualTo(aCase1.WorkflowState));

            #endregion

            #region caseListDtDescendingUnitId aCase3

            Assert.That(caseListDtDescendingUnitId[0].CaseType, Is.EqualTo(aCase3.Type));
            Assert.That(caseListDtDescendingUnitId[0].CaseUId, Is.EqualTo(aCase3.CaseUid));
            Assert.That(caseListDtDescendingUnitId[0].CheckUIid, Is.EqualTo(aCase3.MicrotingCheckUid));
            Assert.That(caseListDtDescendingUnitId[0].CreatedAt.ToString(), Is.EqualTo(c3_ca.ToString()));
            Assert.That(caseListDtDescendingUnitId[0].Custom, Is.EqualTo(aCase3.Custom));
            Assert.That(caseListDtDescendingUnitId[0].DoneAt.ToString(), Is.EqualTo(c3_da.ToString()));
            Assert.That(caseListDtDescendingUnitId[0].Id, Is.EqualTo(aCase3.Id));
            Assert.That(caseListDtDescendingUnitId[0].MicrotingUId, Is.EqualTo(aCase3.MicrotingUid));
            Assert.That(caseListDtDescendingUnitId[0].SiteId, Is.EqualTo(aCase3.Site.MicrotingUid));
            Assert.That(caseListDtDescendingUnitId[0].SiteName, Is.EqualTo(aCase3.Site.Name));
            Assert.That(caseListDtDescendingUnitId[0].Status, Is.EqualTo(aCase3.Status));
            Assert.That(caseListDtDescendingUnitId[0].TemplatId, Is.EqualTo(aCase3.CheckListId));
            Assert.That(caseListDtDescendingUnitId[0].UnitId, Is.EqualTo(aCase3.Unit.MicrotingUid));
            //            Assert.AreEqual(c3_ua.ToString(), caseListDtDescendingUnitId[0].UpdatedAt.ToString());
            Assert.That(caseListDtDescendingUnitId[0].Version, Is.EqualTo(aCase3.Version));
            Assert.That(caseListDtDescendingUnitId[0].WorkerName,
                Is.EqualTo(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName));
            Assert.That(caseListDtDescendingUnitId[0].WorkflowState, Is.EqualTo(aCase3.WorkflowState));

            #endregion

            #endregion

            #endregion

            #endregion

            #region Case Sort

            #region aCase Sort Asc

            #region aCase1 sort asc

            #region caseListC1DoneAt aCase1

            Assert.NotNull(caseListC1SortDoneAt);
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

            Assert.NotNull(caseListC1SortStatus);
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

            Assert.NotNull(caseListC1SortUnitId);
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

            Assert.NotNull(caseListC2SortDoneAt);
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

            Assert.NotNull(caseListC2SortStatus);
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

            Assert.NotNull(caseListC2SortUnitId);
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

            Assert.NotNull(caseListC3SortDoneAt);
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

            Assert.NotNull(caseListC3SortStatus);
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

            Assert.NotNull(caseListC3SortUnitId);
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

            Assert.NotNull(caseListC4SortDoneAt);
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

            Assert.NotNull(caseListC1SortDoneAt);
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

            Assert.NotNull(caseListC4SortDoneAt);
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

            #region aCase Sort Des

            #region aCase1 Sort Des

            #region caseListC1SortDescendingDoneAt aCase1

            Assert.NotNull(caseListC1SortDescendingDoneAt);
            Assert.That(caseListC1SortDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1.type, caseListC1SortDescendingDoneAt[1].CaseType);
            // Assert.AreEqual(aCase1.case_uid, caseListC1SortDescendingDoneAt[1].CaseUId);
            // Assert.AreEqual(aCase1.microting_check_uid, caseListC1SortDescendingDoneAt[1].CheckUIid);
            // Assert.AreEqual(c1_ca.ToString(), caseListC1SortDescendingDoneAt[1].CreatedAt.ToString());
            // Assert.AreEqual(aCase1.custom, caseListC1SortDescendingDoneAt[1].Custom);
            // Assert.AreEqual(c1_da.ToString(), caseListC1SortDescendingDoneAt[1].DoneAt.ToString());
            // Assert.AreEqual(aCase1.Id, caseListC1SortDescendingDoneAt[1].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDescendingDoneAt[1].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDescendingDoneAt[1].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDescendingDoneAt[1].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDescendingDoneAt[1].Status);
            // Assert.AreEqual(aCase1.check_list_id, caseListC1SortDescendingDoneAt[1].TemplatId);
            // Assert.AreEqual(aCase1.unit.microting_uid, caseListC1SortDescendingDoneAt[1].UnitId);
            // Assert.AreEqual(c1_ua.ToString(), caseListC1SortDescendingDoneAt[1].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1.version, caseListC1SortDescendingDoneAt[1].Version);
            // Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListC1SortDescendingDoneAt[1].WorkerName);
            // Assert.AreEqual(aCase1.workflow_state, caseListC1SortDescendingDoneAt[1].WorkflowState);

            #endregion

            #region caseListC1SortDescendingStatus aCase1

            Assert.NotNull(caseListC1SortDescendingStatus);
            Assert.That(caseListC1SortDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1.type, caseListC1SortDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase1.case_uid, caseListC1SortDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase1.microting_check_uid, caseListC1SortDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c1_ca.ToString(), caseListC1SortDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1.custom, caseListC1SortDescendingStatus[0].Custom);
            // Assert.AreEqual(c1_da.ToString(), caseListC1SortDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1.Id, caseListC1SortDescendingStatus[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDescendingStatus[0].Status);
            // Assert.AreEqual(aCase1.check_list_id, caseListC1SortDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.microting_uid, caseListC1SortDescendingStatus[0].UnitId);
            // Assert.AreEqual(c1_ua.ToString(), caseListC1SortDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1.version, caseListC1SortDescendingStatus[0].Version);
            // Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListC1SortDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase1.workflow_state, caseListC1SortDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListC1SortDescendingUnitId aCase1

            Assert.NotNull(caseListC1SortDescendingUnitId);
            Assert.That(caseListC1SortDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1.type, caseListC1SortDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase1.case_uid, caseListC1SortDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase1.microting_check_uid, caseListC1SortDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c1_ca.ToString(), caseListC1SortDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1.custom, caseListC1SortDescendingUnitId[0].Custom);
            // Assert.AreEqual(c1_da.ToString(), caseListC1SortDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1.Id, caseListC1SortDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase1.check_list_id, caseListC1SortDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.microting_uid, caseListC1SortDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c1_ua.ToString(), caseListC1SortDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1.version, caseListC1SortDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListC1SortDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase1.workflow_state, caseListC1SortDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase2 Sort Des

            #region caseListC2SortDescendingDoneAt aCase2

            Assert.NotNull(caseListC2SortDescendingDoneAt);
            Assert.That(caseListC2SortDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2.type, caseListC2SortDescendingDoneAt[3].CaseType);
            // Assert.AreEqual(aCase2.case_uid, caseListC2SortDescendingDoneAt[3].CaseUId);
            // Assert.AreEqual(aCase2.microting_check_uid, caseListC2SortDescendingDoneAt[3].CheckUIid);
            // Assert.AreEqual(c2_ca.ToString(), caseListC2SortDescendingDoneAt[3].CreatedAt.ToString());
            // Assert.AreEqual(aCase2.custom, caseListC2SortDescendingDoneAt[3].Custom);
            // Assert.AreEqual(c2_da.ToString(), caseListC2SortDescendingDoneAt[3].DoneAt.ToString());
            // Assert.AreEqual(aCase2.Id, caseListC2SortDescendingDoneAt[3].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDescendingDoneAt[3].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDescendingDoneAt[3].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDescendingDoneAt[3].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDescendingDoneAt[3].Status);
            // Assert.AreEqual(aCase2.check_list_id, caseListC2SortDescendingDoneAt[3].TemplatId);
            // Assert.AreEqual(aCase2.unit.microting_uid, caseListC2SortDescendingDoneAt[3].UnitId);
            // Assert.AreEqual(c2_ua.ToString(), caseListC2SortDescendingDoneAt[3].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2.version, caseListC2SortDescendingDoneAt[3].Version);
            // Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListC2SortDescendingDoneAt[3].WorkerName);
            // Assert.AreEqual(aCase2.workflow_state, caseListC2SortDescendingDoneAt[3].WorkflowState);

            #endregion

            #region caseListC2SortDescendingStatus aCase2

            Assert.NotNull(caseListC2SortDescendingStatus);
            Assert.That(caseListC2SortDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2.type, caseListC2SortDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase2.case_uid, caseListC2SortDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase2.microting_check_uid, caseListC2SortDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c2_ca.ToString(), caseListC2SortDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2.custom, caseListC2SortDescendingStatus[0].Custom);
            // Assert.AreEqual(c2_da.ToString(), caseListC2SortDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2.Id, caseListC2SortDescendingStatus[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDescendingStatus[0].Status);
            // Assert.AreEqual(aCase2.check_list_id, caseListC2SortDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.microting_uid, caseListC2SortDescendingStatus[0].UnitId);
            // Assert.AreEqual(c2_ua.ToString(), caseListC2SortDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2.version, caseListC2SortDescendingStatus[0].Version);
            // Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListC2SortDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase2.workflow_state, caseListC2SortDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListC2SortDescendingUnitId aCase2

            Assert.NotNull(caseListC2SortDescendingUnitId);
            Assert.That(caseListC2SortDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2.type, caseListC2SortDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase2.case_uid, caseListC2SortDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase2.microting_check_uid, caseListC2SortDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c2_ca.ToString(), caseListC2SortDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2.custom, caseListC2SortDescendingUnitId[0].Custom);
            // Assert.AreEqual(c2_da.ToString(), caseListC2SortDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2.Id, caseListC2SortDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase2.check_list_id, caseListC2SortDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.microting_uid, caseListC2SortDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c2_ua.ToString(), caseListC2SortDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2.version, caseListC2SortDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListC2SortDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase2.workflow_state, caseListC2SortDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase3 Sort Des

            #region caseListC3SortDescendingDoneAt aCase3

            Assert.NotNull(caseListC3SortDescendingDoneAt);
            Assert.That(caseListC3SortDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3.type, caseListC3SortDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase3.case_uid, caseListC3SortDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase3.microting_check_uid, caseListC3SortDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c3_ca.ToString(), caseListC3SortDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3.custom, caseListC3SortDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c3_da.ToString(), caseListC3SortDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3.Id, caseListC3SortDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase3.check_list_id, caseListC3SortDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.microting_uid, caseListC3SortDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c3_ua.ToString(), caseListC3SortDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3.version, caseListC3SortDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListC3SortDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase3.workflow_state, caseListC3SortDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListC3SortDescendingStatus aCase3

            Assert.NotNull(caseListC3SortDescendingStatus);
            Assert.That(caseListC3SortDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3.type, caseListC3SortDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase3.case_uid, caseListC3SortDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase3.microting_check_uid, caseListC3SortDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c3_ca.ToString(), caseListC3SortDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3.custom, caseListC3SortDescendingStatus[0].Custom);
            // Assert.AreEqual(c3_da.ToString(), caseListC3SortDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3.Id, caseListC3SortDescendingStatus[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDescendingStatus[0].Status);
            // Assert.AreEqual(aCase3.check_list_id, caseListC3SortDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.microting_uid, caseListC3SortDescendingStatus[0].UnitId);
            // Assert.AreEqual(c3_ua.ToString(), caseListC3SortDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3.version, caseListC3SortDescendingStatus[0].Version);
            // Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListC3SortDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase3.workflow_state, caseListC3SortDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListC3SortDescendingUnitId aCase3

            Assert.NotNull(caseListC3SortDescendingUnitId);
            Assert.That(caseListC3SortDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3.type, caseListC3SortDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase3.case_uid, caseListC3SortDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase3.microting_check_uid, caseListC3SortDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c3_ca.ToString(), caseListC3SortDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3.custom, caseListC3SortDescendingUnitId[0].Custom);
            // Assert.AreEqual(c3_da.ToString(), caseListC3SortDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3.Id, caseListC3SortDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase3.check_list_id, caseListC3SortDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.microting_uid, caseListC3SortDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c3_ua.ToString(), caseListC3SortDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3.version, caseListC3SortDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListC3SortDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase3.workflow_state, caseListC3SortDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase4 Sort Des

            #region caseListC4SortDescendingDoneAt aCase4

            Assert.NotNull(caseListC4SortDescendingDoneAt);
            Assert.That(caseListC4SortDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4.type, caseListC4SortDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4.case_uid, caseListC4SortDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4.microting_check_uid, caseListC4SortDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4_ca.ToString(), caseListC4SortDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4.custom, caseListC4SortDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c4_da.ToString(), caseListC4SortDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4.Id, caseListC4SortDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase4.check_list_id, caseListC4SortDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.microting_uid, caseListC4SortDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c4_ua.ToString(), caseListC4SortDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4.version, caseListC4SortDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListC4SortDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4.workflow_state, caseListC4SortDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListC4SortDescendingStatus aCase4

            Assert.NotNull(caseListC4SortDescendingStatus);
            Assert.That(caseListC4SortDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4.type, caseListC4SortDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase4.case_uid, caseListC4SortDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase4.microting_check_uid, caseListC4SortDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c4_ca.ToString(), caseListC4SortDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4.custom, caseListC4SortDescendingStatus[0].Custom);
            // Assert.AreEqual(c4_da.ToString(), caseListC4SortDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4.Id, caseListC4SortDescendingStatus[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDescendingStatus[0].Status);
            // Assert.AreEqual(aCase4.check_list_id, caseListC4SortDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.microting_uid, caseListC4SortDescendingStatus[0].UnitId);
            // Assert.AreEqual(c4_ua.ToString(), caseListC4SortDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4.version, caseListC4SortDescendingStatus[0].Version);
            // Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListC4SortDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase4.workflow_state, caseListC4SortDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListC4SortDescendingUnitId aCase4

            Assert.NotNull(caseListC4SortDescendingUnitId);
            Assert.That(caseListC4SortDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4.type, caseListC4SortDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase4.case_uid, caseListC4SortDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase4.microting_check_uid, caseListC4SortDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c4_ca.ToString(), caseListC4SortDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4.custom, caseListC4SortDescendingUnitId[0].Custom);
            // Assert.AreEqual(c4_da.ToString(), caseListC4SortDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4.Id, caseListC4SortDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase4.check_list_id, caseListC4SortDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.microting_uid, caseListC4SortDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c4_ua.ToString(), caseListC4SortDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4.version, caseListC4SortDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListC4SortDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase4.workflow_state, caseListC4SortDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #endregion

            #region aCase Sort Asc w. DateTime

            #region aCase1 sort asc w. DateTime

            #region caseListC1SortDtDoneAt aCase1

            Assert.NotNull(caseListC1SortDtDoneAt);
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

            Assert.NotNull(caseListC1SortDtStatus);
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

            Assert.NotNull(caseListC1SortDtUnitId);
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

            Assert.NotNull(caseListC2SortDtDoneAt);
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

            Assert.NotNull(caseListC2SortDtStatus);
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

            Assert.NotNull(caseListC2SortDtUnitId);
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

            Assert.NotNull(caseListC3SortDtDoneAt);
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

            Assert.NotNull(caseListC3SortDtStatus);
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

            Assert.NotNull(caseListC3SortDtUnitId);
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

            Assert.NotNull(caseListC4SortDtDoneAt);
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

            Assert.NotNull(caseListC4SortDtStatus);
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

            Assert.NotNull(caseListC4SortDtUnitId);
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

            #region aCase Sort Des w. DateTime

            #region aCase1 Sort Des w. DateTime

            #region caseListC1SortDtDescendingDoneAt aCase1

            Assert.NotNull(caseListC1SortDtDescendingDoneAt);
            Assert.That(caseListC1SortDtDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1.type, caseListC1SortDtDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase1.case_uid, caseListC1SortDtDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase1.microting_check_uid, caseListC1SortDtDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c1_ca.ToString(), caseListC1SortDtDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1.custom, caseListC1SortDtDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c1_da.ToString(), caseListC1SortDtDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1.Id, caseListC1SortDtDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDtDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDtDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDtDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDtDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase1.check_list_id, caseListC1SortDtDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.microting_uid, caseListC1SortDtDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c1_ua.ToString(), caseListC1SortDtDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1.version, caseListC1SortDtDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListC1SortDtDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase1.workflow_state, caseListC1SortDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListC1SortDtDescendingStatus aCase1

            Assert.NotNull(caseListC1SortDtDescendingStatus);
            Assert.That(caseListC1SortDtDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1.type, caseListC1SortDtDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase1.case_uid, caseListC1SortDtDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase1.microting_check_uid, caseListC1SortDtDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c1_ca.ToString(), caseListC1SortDtDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1.custom, caseListC1SortDtDescendingStatus[0].Custom);
            // Assert.AreEqual(c1_da.ToString(), caseListC1SortDtDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1.Id, caseListC1SortDtDescendingStatus[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDtDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDtDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDtDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDtDescendingStatus[0].Status);
            // Assert.AreEqual(aCase1.check_list_id, caseListC1SortDtDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.microting_uid, caseListC1SortDtDescendingStatus[0].UnitId);
            // Assert.AreEqual(c1_ua.ToString(), caseListC1SortDtDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1.version, caseListC1SortDtDescendingStatus[0].Version);
            // Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListC1SortDtDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase1.workflow_state, caseListC1SortDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListC1SortDtDescendingUnitId aCase1

            Assert.NotNull(caseListC1SortDtDescendingUnitId);
            Assert.That(caseListC1SortDtDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1.type, caseListC1SortDtDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase1.case_uid, caseListC1SortDtDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase1.microting_check_uid, caseListC1SortDtDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c1_ca.ToString(), caseListC1SortDtDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1.custom, caseListC1SortDtDescendingUnitId[0].Custom);
            // Assert.AreEqual(c1_da.ToString(), caseListC1SortDtDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1.Id, caseListC1SortDtDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDtDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDtDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDtDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDtDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase1.check_list_id, caseListC1SortDtDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.microting_uid, caseListC1SortDtDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c1_ua.ToString(), caseListC1SortDtDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1.version, caseListC1SortDtDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListC1SortDtDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase1.workflow_state, caseListC1SortDtDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase2 Sort Des w. DateTime

            #region caseListC2SortDtDescendingDoneAt aCase2

            Assert.NotNull(caseListC2SortDtDescendingDoneAt);
            Assert.That(caseListC2SortDtDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2.type, caseListC2SortDtDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase2.case_uid, caseListC2SortDtDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase2.microting_check_uid, caseListC2SortDtDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c2_ca.ToString(), caseListC2SortDtDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2.custom, caseListC2SortDtDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c2_da.ToString(), caseListC2SortDtDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2.Id, caseListC2SortDtDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDtDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDtDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDtDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDtDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase2.check_list_id, caseListC2SortDtDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.microting_uid, caseListC2SortDtDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c2_ua.ToString(), caseListC2SortDtDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2.version, caseListC2SortDtDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListC2SortDtDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase2.workflow_state, caseListC2SortDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListC2SortDtDescendingStatus aCase2

            Assert.NotNull(caseListC2SortDtDescendingStatus);
            Assert.That(caseListC2SortDtDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2.type, caseListC2SortDtDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase2.case_uid, caseListC2SortDtDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase2.microting_check_uid, caseListC2SortDtDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c2_ca.ToString(), caseListC2SortDtDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2.custom, caseListC2SortDtDescendingStatus[0].Custom);
            // Assert.AreEqual(c2_da.ToString(), caseListC2SortDtDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2.Id, caseListC2SortDtDescendingStatus[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDtDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDtDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDtDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDtDescendingStatus[0].Status);
            // Assert.AreEqual(aCase2.check_list_id, caseListC2SortDtDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.microting_uid, caseListC2SortDtDescendingStatus[0].UnitId);
            // Assert.AreEqual(c2_ua.ToString(), caseListC2SortDtDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2.version, caseListC2SortDtDescendingStatus[0].Version);
            // Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListC2SortDtDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase2.workflow_state, caseListC2SortDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListC2SortDtDescendingUnitId aCase2

            Assert.NotNull(caseListC2SortDtDescendingUnitId);
            Assert.That(caseListC2SortDtDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2.type, caseListC2SortDtDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase2.case_uid, caseListC2SortDtDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase2.microting_check_uid, caseListC2SortDtDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c2_ca.ToString(), caseListC2SortDtDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2.custom, caseListC2SortDtDescendingUnitId[0].Custom);
            // Assert.AreEqual(c2_da.ToString(), caseListC2SortDtDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2.Id, caseListC2SortDtDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDtDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDtDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDtDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDtDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase2.check_list_id, caseListC2SortDtDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.microting_uid, caseListC2SortDtDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c2_ua.ToString(), caseListC2SortDtDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2.version, caseListC2SortDtDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListC2SortDtDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase2.workflow_state, caseListC2SortDtDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase3 Sort Des w. DateTime

            #region caseListC3SortDtDescendingDoneAt aCase3

            Assert.NotNull(caseListC3SortDtDescendingDoneAt);
            Assert.That(caseListC3SortDtDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3.type, caseListC3SortDtDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase3.case_uid, caseListC3SortDtDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase3.microting_check_uid, caseListC3SortDtDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c3_ca.ToString(), caseListC3SortDtDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3.custom, caseListC3SortDtDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c3_da.ToString(), caseListC3SortDtDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3.Id, caseListC3SortDtDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDtDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDtDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDtDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDtDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase3.check_list_id, caseListC3SortDtDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.microting_uid, caseListC3SortDtDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c3_ua.ToString(), caseListC3SortDtDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3.version, caseListC3SortDtDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListC3SortDtDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase3.workflow_state, caseListC3SortDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListC3SortDtDescendingStatus aCase3

            Assert.NotNull(caseListC3SortDtDescendingStatus);
            Assert.That(caseListC3SortDtDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3.type, caseListC3SortDtDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase3.case_uid, caseListC3SortDtDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase3.microting_check_uid, caseListC3SortDtDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c3_ca.ToString(), caseListC3SortDtDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3.custom, caseListC3SortDtDescendingStatus[0].Custom);
            // Assert.AreEqual(c3_da.ToString(), caseListC3SortDtDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3.Id, caseListC3SortDtDescendingStatus[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDtDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDtDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDtDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDtDescendingStatus[0].Status);
            // Assert.AreEqual(aCase3.check_list_id, caseListC3SortDtDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.microting_uid, caseListC3SortDtDescendingStatus[0].UnitId);
            // Assert.AreEqual(c3_ua.ToString(), caseListC3SortDtDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3.version, caseListC3SortDtDescendingStatus[0].Version);
            // Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListC3SortDtDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase3.workflow_state, caseListC3SortDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListC3SortDtDescendingUnitId aCase3

            Assert.NotNull(caseListC3SortDtDescendingUnitId);
            Assert.That(caseListC3SortDtDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3.type, caseListC3SortDtDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase3.case_uid, caseListC3SortDtDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase3.microting_check_uid, caseListC3SortDtDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c3_ca.ToString(), caseListC3SortDtDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3.custom, caseListC3SortDtDescendingUnitId[0].Custom);
            // Assert.AreEqual(c3_da.ToString(), caseListC3SortDtDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3.Id, caseListC3SortDtDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDtDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDtDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDtDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDtDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase3.check_list_id, caseListC3SortDtDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.microting_uid, caseListC3SortDtDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c3_ua.ToString(), caseListC3SortDtDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3.version, caseListC3SortDtDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListC3SortDtDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase3.workflow_state, caseListC3SortDtDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase4 Sort Des w. DateTime

            #region caseListC4SortDtDescendingDoneAt aCase4

            Assert.NotNull(caseListC4SortDtDescendingDoneAt);
            Assert.That(caseListC4SortDtDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4.type, caseListC4SortDtDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4.case_uid, caseListC4SortDtDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4.microting_check_uid, caseListC4SortDtDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4_ca.ToString(), caseListC4SortDtDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4.custom, caseListC4SortDtDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c4_da.ToString(), caseListC4SortDtDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4.Id, caseListC4SortDtDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDtDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDtDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDtDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDtDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase4.check_list_id, caseListC4SortDtDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.microting_uid, caseListC4SortDtDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c4_ua.ToString(), caseListC4SortDtDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4.version, caseListC4SortDtDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListC4SortDtDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4.workflow_state, caseListC4SortDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListC4SortDtDescendingStatus aCase4

            Assert.NotNull(caseListC4SortDtDescendingStatus);
            Assert.That(caseListC4SortDtDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4.type, caseListC4SortDtDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase4.case_uid, caseListC4SortDtDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase4.microting_check_uid, caseListC4SortDtDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c4_ca.ToString(), caseListC4SortDtDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4.custom, caseListC4SortDtDescendingStatus[0].Custom);
            // Assert.AreEqual(c4_da.ToString(), caseListC4SortDtDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4.Id, caseListC4SortDtDescendingStatus[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDtDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDtDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDtDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDtDescendingStatus[0].Status);
            // Assert.AreEqual(aCase4.check_list_id, caseListC4SortDtDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.microting_uid, caseListC4SortDtDescendingStatus[0].UnitId);
            // Assert.AreEqual(c4_ua.ToString(), caseListC4SortDtDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4.version, caseListC4SortDtDescendingStatus[0].Version);
            // Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListC4SortDtDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase4.workflow_state, caseListC4SortDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListC4SortDtDescendingUnitId aCase4

            Assert.NotNull(caseListC4SortDtDescendingUnitId);
            Assert.That(caseListC4SortDtDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4.type, caseListC4SortDtDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase4.case_uid, caseListC4SortDtDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase4.microting_check_uid, caseListC4SortDtDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c4_ca.ToString(), caseListC4SortDtDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4.custom, caseListC4SortDtDescendingUnitId[0].Custom);
            // Assert.AreEqual(c4_da.ToString(), caseListC4SortDtDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4.Id, caseListC4SortDtDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDtDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDtDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDtDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDtDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase4.check_list_id, caseListC4SortDtDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.microting_uid, caseListC4SortDtDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c4_ua.ToString(), caseListC4SortDtDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4.version, caseListC4SortDtDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListC4SortDtDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase4.workflow_state, caseListC4SortDtDescendingUnitId[0].WorkflowState);

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
            Assert.That(caseListRemovedDoneAt.Count, Is.EqualTo(4));
            Assert.That(caseListRemovedDoneAt[1].CaseType, Is.EqualTo(aCase1Removed.Type));
            Assert.That(caseListRemovedDoneAt[1].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
            Assert.That(caseListRemovedDoneAt[1].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
            Assert.That(caseListRemovedDoneAt[1].Custom, Is.EqualTo(aCase1Removed.Custom));
            Assert.That(caseListRemovedDoneAt[1].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
            Assert.That(caseListRemovedDoneAt[1].Id, Is.EqualTo(aCase1Removed.Id));
            Assert.That(caseListRemovedDoneAt[1].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
            Assert.That(caseListRemovedDoneAt[1].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDoneAt[1].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
            Assert.That(caseListRemovedDoneAt[1].Status, Is.EqualTo(aCase1Removed.Status));
            Assert.That(caseListRemovedDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRemovedDoneAt[1].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDoneAt[1].UpdatedAt.ToString());
            Assert.That(caseListRemovedDoneAt[1].Version, Is.EqualTo(aCase1Removed.Version));
            Assert.That(caseListRemovedDoneAt[1].WorkerName,
                Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
            Assert.That(caseListRemovedDoneAt[1].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

            #endregion

            #region caseListRemovedDoneAt aCase2Removed

            Assert.That(caseListRemovedDoneAt[3].CaseType, Is.EqualTo(aCase2Removed.Type));
            Assert.That(caseListRemovedDoneAt[3].CaseUId, Is.EqualTo(aCase2Removed.CaseUid));
            Assert.That(caseListRemovedDoneAt[3].CheckUIid, Is.EqualTo(aCase2Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDoneAt[3].CreatedAt.ToString(), Is.EqualTo(c2Removed_ca.ToString()));
            Assert.That(caseListRemovedDoneAt[3].Custom, Is.EqualTo(aCase2Removed.Custom));
            Assert.That(caseListRemovedDoneAt[3].DoneAt.ToString(), Is.EqualTo(c2Removed_da.ToString()));
            Assert.That(caseListRemovedDoneAt[3].Id, Is.EqualTo(aCase2Removed.Id));
            Assert.That(caseListRemovedDoneAt[3].MicrotingUId, Is.EqualTo(aCase2Removed.MicrotingUid));
            Assert.That(caseListRemovedDoneAt[3].SiteId, Is.EqualTo(aCase2Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDoneAt[3].SiteName, Is.EqualTo(aCase2Removed.Site.Name));
            Assert.That(caseListRemovedDoneAt[3].Status, Is.EqualTo(aCase2Removed.Status));
            Assert.That(caseListRemovedDoneAt[3].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRemovedDoneAt[3].UnitId, Is.EqualTo(aCase2Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedDoneAt[3].UpdatedAt.ToString());
            Assert.That(caseListRemovedDoneAt[3].Version, Is.EqualTo(aCase2Removed.Version));
            Assert.That(caseListRemovedDoneAt[3].WorkerName,
                Is.EqualTo(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName));
            Assert.That(caseListRemovedDoneAt[3].WorkflowState, Is.EqualTo(aCase2Removed.WorkflowState));

            #endregion

            #region caseListRemovedDoneAt aCase3Removed

            Assert.That(caseListRemovedDoneAt[0].CaseType, Is.EqualTo(aCase3Removed.Type));
            Assert.That(caseListRemovedDoneAt[0].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
            Assert.That(caseListRemovedDoneAt[0].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
            Assert.That(caseListRemovedDoneAt[0].Custom, Is.EqualTo(aCase3Removed.Custom));
            Assert.That(caseListRemovedDoneAt[0].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
            Assert.That(caseListRemovedDoneAt[0].Id, Is.EqualTo(aCase3Removed.Id));
            Assert.That(caseListRemovedDoneAt[0].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
            Assert.That(caseListRemovedDoneAt[0].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDoneAt[0].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
            Assert.That(caseListRemovedDoneAt[0].Status, Is.EqualTo(aCase3Removed.Status));
            Assert.That(caseListRemovedDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRemovedDoneAt[0].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDoneAt[0].UpdatedAt.ToString());
            Assert.That(caseListRemovedDoneAt[0].Version, Is.EqualTo(aCase3Removed.Version));
            Assert.That(caseListRemovedDoneAt[0].WorkerName,
                Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
            Assert.That(caseListRemovedDoneAt[0].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

            #endregion

            #region caseListRemovedDoneAt aCase4Removed

            Assert.That(caseListRemovedDoneAt[2].CaseType, Is.EqualTo(aCase4Removed.Type));
            Assert.That(caseListRemovedDoneAt[2].CaseUId, Is.EqualTo(aCase4Removed.CaseUid));
            Assert.That(caseListRemovedDoneAt[2].CheckUIid, Is.EqualTo(aCase4Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDoneAt[2].CreatedAt.ToString(), Is.EqualTo(c4Removed_ca.ToString()));
            Assert.That(caseListRemovedDoneAt[2].Custom, Is.EqualTo(aCase4Removed.Custom));
            Assert.That(caseListRemovedDoneAt[2].DoneAt.ToString(), Is.EqualTo(c4Removed_da.ToString()));
            Assert.That(caseListRemovedDoneAt[2].Id, Is.EqualTo(aCase4Removed.Id));
            Assert.That(caseListRemovedDoneAt[2].MicrotingUId, Is.EqualTo(aCase4Removed.MicrotingUid));
            Assert.That(caseListRemovedDoneAt[2].SiteId, Is.EqualTo(aCase4Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDoneAt[2].SiteName, Is.EqualTo(aCase4Removed.Site.Name));
            Assert.That(caseListRemovedDoneAt[2].Status, Is.EqualTo(aCase4Removed.Status));
            Assert.That(caseListRemovedDoneAt[2].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRemovedDoneAt[2].UnitId, Is.EqualTo(aCase4Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedDoneAt[2].UpdatedAt.ToString());
            Assert.That(caseListRemovedDoneAt[2].Version, Is.EqualTo(aCase4Removed.Version));
            Assert.That(caseListRemovedDoneAt[2].WorkerName,
                Is.EqualTo(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName));
            Assert.That(caseListRemovedDoneAt[2].WorkflowState, Is.EqualTo(aCase4Removed.WorkflowState));

            #endregion

            #endregion

            #region caseListRemovedStatus Def Sort Asc

            #region caseListRemovedStatus aCase1Removed

            Assert.NotNull(caseListRemovedStatus);
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

            Assert.NotNull(caseListRemovedUnitId);
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

            #region Def Sort Des

            #region caseListRemovedDescendingDoneAt Def Sort Des

            #region caseListRemovedDescendingDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedDescendingDoneAt);
            Assert.That(caseListRemovedDescendingDoneAt.Count, Is.EqualTo(4));
            Assert.That(caseListRemovedDescendingDoneAt[2].CaseType, Is.EqualTo(aCase1Removed.Type));
            Assert.That(caseListRemovedDescendingDoneAt[2].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
            Assert.That(caseListRemovedDescendingDoneAt[2].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDescendingDoneAt[2].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
            Assert.That(caseListRemovedDescendingDoneAt[2].Custom, Is.EqualTo(aCase1Removed.Custom));
            Assert.That(caseListRemovedDescendingDoneAt[2].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
            Assert.That(caseListRemovedDescendingDoneAt[2].Id, Is.EqualTo(aCase1Removed.Id));
            Assert.That(caseListRemovedDescendingDoneAt[2].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
            Assert.That(caseListRemovedDescendingDoneAt[2].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDescendingDoneAt[2].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
            Assert.That(caseListRemovedDescendingDoneAt[2].Status, Is.EqualTo(aCase1Removed.Status));
            Assert.That(caseListRemovedDescendingDoneAt[2].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRemovedDescendingDoneAt[2].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDescendingDoneAt[2].UpdatedAt.ToString());
            Assert.That(caseListRemovedDescendingDoneAt[2].Version, Is.EqualTo(aCase1Removed.Version));
            Assert.That(caseListRemovedDescendingDoneAt[2].WorkerName,
                Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
            Assert.That(caseListRemovedDescendingDoneAt[2].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

            #endregion

            #region caseListRemovedDescendingDoneAt aCase2Removed

            Assert.That(caseListRemovedDescendingDoneAt[0].CaseType, Is.EqualTo(aCase2Removed.Type));
            Assert.That(caseListRemovedDescendingDoneAt[0].CaseUId, Is.EqualTo(aCase2Removed.CaseUid));
            Assert.That(caseListRemovedDescendingDoneAt[0].CheckUIid, Is.EqualTo(aCase2Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDescendingDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c2Removed_ca.ToString()));
            Assert.That(caseListRemovedDescendingDoneAt[0].Custom, Is.EqualTo(aCase2Removed.Custom));
            Assert.That(caseListRemovedDescendingDoneAt[0].DoneAt.ToString(), Is.EqualTo(c2Removed_da.ToString()));
            Assert.That(caseListRemovedDescendingDoneAt[0].Id, Is.EqualTo(aCase2Removed.Id));
            Assert.That(caseListRemovedDescendingDoneAt[0].MicrotingUId, Is.EqualTo(aCase2Removed.MicrotingUid));
            Assert.That(caseListRemovedDescendingDoneAt[0].SiteId, Is.EqualTo(aCase2Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDescendingDoneAt[0].SiteName, Is.EqualTo(aCase2Removed.Site.Name));
            Assert.That(caseListRemovedDescendingDoneAt[0].Status, Is.EqualTo(aCase2Removed.Status));
            Assert.That(caseListRemovedDescendingDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRemovedDescendingDoneAt[0].UnitId, Is.EqualTo(aCase2Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedDescendingDoneAt[0].UpdatedAt.ToString());
            Assert.That(caseListRemovedDescendingDoneAt[0].Version, Is.EqualTo(aCase2Removed.Version));
            Assert.That(caseListRemovedDescendingDoneAt[0].WorkerName,
                Is.EqualTo(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName));
            Assert.That(caseListRemovedDescendingDoneAt[0].WorkflowState, Is.EqualTo(aCase2Removed.WorkflowState));

            #endregion

            #region caseListRemovedDescendingDoneAt aCase3Removed

            Assert.That(caseListRemovedDescendingDoneAt[3].CaseType, Is.EqualTo(aCase3Removed.Type));
            Assert.That(caseListRemovedDescendingDoneAt[3].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
            Assert.That(caseListRemovedDescendingDoneAt[3].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDescendingDoneAt[3].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
            Assert.That(caseListRemovedDescendingDoneAt[3].Custom, Is.EqualTo(aCase3Removed.Custom));
            Assert.That(caseListRemovedDescendingDoneAt[3].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
            Assert.That(caseListRemovedDescendingDoneAt[3].Id, Is.EqualTo(aCase3Removed.Id));
            Assert.That(caseListRemovedDescendingDoneAt[3].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
            Assert.That(caseListRemovedDescendingDoneAt[3].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDescendingDoneAt[3].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
            Assert.That(caseListRemovedDescendingDoneAt[3].Status, Is.EqualTo(aCase3Removed.Status));
            Assert.That(caseListRemovedDescendingDoneAt[3].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRemovedDescendingDoneAt[3].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDescendingDoneAt[3].UpdatedAt.ToString());
            Assert.That(caseListRemovedDescendingDoneAt[3].Version, Is.EqualTo(aCase3Removed.Version));
            Assert.That(caseListRemovedDescendingDoneAt[3].WorkerName,
                Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
            Assert.That(caseListRemovedDescendingDoneAt[3].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

            #endregion

            #region caseListRemovedDescendingDoneAt aCase4Removed

            Assert.That(caseListRemovedDescendingDoneAt[1].CaseType, Is.EqualTo(aCase4Removed.Type));
            Assert.That(caseListRemovedDescendingDoneAt[1].CaseUId, Is.EqualTo(aCase4Removed.CaseUid));
            Assert.That(caseListRemovedDescendingDoneAt[1].CheckUIid, Is.EqualTo(aCase4Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDescendingDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c4Removed_ca.ToString()));
            Assert.That(caseListRemovedDescendingDoneAt[1].Custom, Is.EqualTo(aCase4Removed.Custom));
            Assert.That(caseListRemovedDescendingDoneAt[1].DoneAt.ToString(), Is.EqualTo(c4Removed_da.ToString()));
            Assert.That(caseListRemovedDescendingDoneAt[1].Id, Is.EqualTo(aCase4Removed.Id));
            Assert.That(caseListRemovedDescendingDoneAt[1].MicrotingUId, Is.EqualTo(aCase4Removed.MicrotingUid));
            Assert.That(caseListRemovedDescendingDoneAt[1].SiteId, Is.EqualTo(aCase4Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDescendingDoneAt[1].SiteName, Is.EqualTo(aCase4Removed.Site.Name));
            Assert.That(caseListRemovedDescendingDoneAt[1].Status, Is.EqualTo(aCase4Removed.Status));
            Assert.That(caseListRemovedDescendingDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRemovedDescendingDoneAt[1].UnitId, Is.EqualTo(aCase4Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedDescendingDoneAt[1].UpdatedAt.ToString());
            Assert.That(caseListRemovedDescendingDoneAt[1].Version, Is.EqualTo(aCase4Removed.Version));
            Assert.That(caseListRemovedDescendingDoneAt[1].WorkerName,
                Is.EqualTo(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName));
            Assert.That(caseListRemovedDescendingDoneAt[1].WorkflowState, Is.EqualTo(aCase4Removed.WorkflowState));

            #endregion

            #endregion

            #region caseListRemovedDescendingStatus Def Sort Des

            #region caseListRemovedDescendingStatus aCase1Removed

            Assert.NotNull(caseListRemovedDescendingStatus);
            Assert.That(caseListRemovedDescendingStatus.Count, Is.EqualTo(4));
            Assert.That(caseListRemovedDescendingStatus[3].CaseType, Is.EqualTo(aCase1Removed.Type));
            Assert.That(caseListRemovedDescendingStatus[3].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
            Assert.That(caseListRemovedDescendingStatus[3].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDescendingStatus[3].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
            Assert.That(caseListRemovedDescendingStatus[3].Custom, Is.EqualTo(aCase1Removed.Custom));
            Assert.That(caseListRemovedDescendingStatus[3].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
            Assert.That(caseListRemovedDescendingStatus[3].Id, Is.EqualTo(aCase1Removed.Id));
            Assert.That(caseListRemovedDescendingStatus[3].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
            Assert.That(caseListRemovedDescendingStatus[3].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDescendingStatus[3].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
            Assert.That(caseListRemovedDescendingStatus[3].Status, Is.EqualTo(aCase1Removed.Status));
            Assert.That(caseListRemovedDescendingStatus[3].TemplatId, Is.EqualTo(aCase1Removed.CheckListId));
            Assert.That(caseListRemovedDescendingStatus[3].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDescendingStatus[3].UpdatedAt.ToString());
            Assert.That(caseListRemovedDescendingStatus[3].Version, Is.EqualTo(aCase1Removed.Version));
            Assert.That(caseListRemovedDescendingStatus[3].WorkerName,
                Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
            Assert.That(caseListRemovedDescendingStatus[3].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

            #endregion

            #region caseListRemovedDescendingStatus aCase2Removed

            Assert.That(caseListRemovedDescendingStatus[2].CaseType, Is.EqualTo(aCase2Removed.Type));
            Assert.That(caseListRemovedDescendingStatus[2].CaseUId, Is.EqualTo(aCase2Removed.CaseUid));
            Assert.That(caseListRemovedDescendingStatus[2].CheckUIid, Is.EqualTo(aCase2Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDescendingStatus[2].CreatedAt.ToString(), Is.EqualTo(c2Removed_ca.ToString()));
            Assert.That(caseListRemovedDescendingStatus[2].Custom, Is.EqualTo(aCase2Removed.Custom));
            Assert.That(caseListRemovedDescendingStatus[2].DoneAt.ToString(), Is.EqualTo(c2Removed_da.ToString()));
            Assert.That(caseListRemovedDescendingStatus[2].Id, Is.EqualTo(aCase2Removed.Id));
            Assert.That(caseListRemovedDescendingStatus[2].MicrotingUId, Is.EqualTo(aCase2Removed.MicrotingUid));
            Assert.That(caseListRemovedDescendingStatus[2].SiteId, Is.EqualTo(aCase2Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDescendingStatus[2].SiteName, Is.EqualTo(aCase2Removed.Site.Name));
            Assert.That(caseListRemovedDescendingStatus[2].Status, Is.EqualTo(aCase2Removed.Status));
            Assert.That(caseListRemovedDescendingStatus[2].TemplatId, Is.EqualTo(aCase2Removed.CheckListId));
            Assert.That(caseListRemovedDescendingStatus[2].UnitId, Is.EqualTo(aCase2Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedDescendingStatus[2].UpdatedAt.ToString());
            Assert.That(caseListRemovedDescendingStatus[2].Version, Is.EqualTo(aCase2Removed.Version));
            Assert.That(caseListRemovedDescendingStatus[2].WorkerName,
                Is.EqualTo(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName));
            Assert.That(caseListRemovedDescendingStatus[2].WorkflowState, Is.EqualTo(aCase2Removed.WorkflowState));

            #endregion

            #region caseListRemovedDescendingStatus aCase3Removed

            Assert.That(caseListRemovedDescendingStatus[1].CaseType, Is.EqualTo(aCase3Removed.Type));
            Assert.That(caseListRemovedDescendingStatus[1].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
            Assert.That(caseListRemovedDescendingStatus[1].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDescendingStatus[1].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
            Assert.That(caseListRemovedDescendingStatus[1].Custom, Is.EqualTo(aCase3Removed.Custom));
            Assert.That(caseListRemovedDescendingStatus[1].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
            Assert.That(caseListRemovedDescendingStatus[1].Id, Is.EqualTo(aCase3Removed.Id));
            Assert.That(caseListRemovedDescendingStatus[1].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
            Assert.That(caseListRemovedDescendingStatus[1].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDescendingStatus[1].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
            Assert.That(caseListRemovedDescendingStatus[1].Status, Is.EqualTo(aCase3Removed.Status));
            Assert.That(caseListRemovedDescendingStatus[1].TemplatId, Is.EqualTo(aCase3Removed.CheckListId));
            Assert.That(caseListRemovedDescendingStatus[1].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDescendingStatus[1].UpdatedAt.ToString());
            Assert.That(caseListRemovedDescendingStatus[1].Version, Is.EqualTo(aCase3Removed.Version));
            Assert.That(caseListRemovedDescendingStatus[1].WorkerName,
                Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
            Assert.That(caseListRemovedDescendingStatus[1].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

            #endregion

            #region caseListRemovedDescendingStatus aCase4Removed

            Assert.That(caseListRemovedDescendingStatus[0].CaseType, Is.EqualTo(aCase4Removed.Type));
            Assert.That(caseListRemovedDescendingStatus[0].CaseUId, Is.EqualTo(aCase4Removed.CaseUid));
            Assert.That(caseListRemovedDescendingStatus[0].CheckUIid, Is.EqualTo(aCase4Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDescendingStatus[0].CreatedAt.ToString(), Is.EqualTo(c4Removed_ca.ToString()));
            Assert.That(caseListRemovedDescendingStatus[0].Custom, Is.EqualTo(aCase4Removed.Custom));
            Assert.That(caseListRemovedDescendingStatus[0].DoneAt.ToString(), Is.EqualTo(c4Removed_da.ToString()));
            Assert.That(caseListRemovedDescendingStatus[0].Id, Is.EqualTo(aCase4Removed.Id));
            Assert.That(caseListRemovedDescendingStatus[0].MicrotingUId, Is.EqualTo(aCase4Removed.MicrotingUid));
            Assert.That(caseListRemovedDescendingStatus[0].SiteId, Is.EqualTo(aCase4Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDescendingStatus[0].SiteName, Is.EqualTo(aCase4Removed.Site.Name));
            Assert.That(caseListRemovedDescendingStatus[0].Status, Is.EqualTo(aCase4Removed.Status));
            Assert.That(caseListRemovedDescendingStatus[0].TemplatId, Is.EqualTo(aCase4Removed.CheckListId));
            Assert.That(caseListRemovedDescendingStatus[0].UnitId, Is.EqualTo(aCase4Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedDescendingStatus[0].UpdatedAt.ToString());
            Assert.That(caseListRemovedDescendingStatus[0].Version, Is.EqualTo(aCase4Removed.Version));
            Assert.That(caseListRemovedDescendingStatus[0].WorkerName,
                Is.EqualTo(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName));
            Assert.That(caseListRemovedDescendingStatus[0].WorkflowState, Is.EqualTo(aCase4Removed.WorkflowState));

            #endregion

            #endregion

            #region caseListRemovedDescendingUnitId Def Sort Des

            #region caseListRemovedDescendingUnitId aCase1Removed

            Assert.NotNull(caseListRemovedDescendingUnitId);
            Assert.That(caseListRemovedDescendingUnitId.Count, Is.EqualTo(4));
            Assert.That(caseListRemovedDescendingUnitId[3].CaseType, Is.EqualTo(aCase1Removed.Type));
            Assert.That(caseListRemovedDescendingUnitId[3].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
            Assert.That(caseListRemovedDescendingUnitId[3].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDescendingUnitId[3].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
            Assert.That(caseListRemovedDescendingUnitId[3].Custom, Is.EqualTo(aCase1Removed.Custom));
            Assert.That(caseListRemovedDescendingUnitId[3].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
            Assert.That(caseListRemovedDescendingUnitId[3].Id, Is.EqualTo(aCase1Removed.Id));
            Assert.That(caseListRemovedDescendingUnitId[3].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
            Assert.That(caseListRemovedDescendingUnitId[3].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDescendingUnitId[3].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
            Assert.That(caseListRemovedDescendingUnitId[3].Status, Is.EqualTo(aCase1Removed.Status));
            Assert.That(caseListRemovedDescendingUnitId[3].TemplatId, Is.EqualTo(aCase1Removed.CheckListId));
            Assert.That(caseListRemovedDescendingUnitId[3].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDescendingUnitId[3].UpdatedAt.ToString());
            Assert.That(caseListRemovedDescendingUnitId[3].Version, Is.EqualTo(aCase1Removed.Version));
            Assert.That(caseListRemovedDescendingUnitId[3].WorkerName,
                Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
            Assert.That(caseListRemovedDescendingUnitId[3].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

            #endregion

            #region caseListRemovedDescendingUnitId aCase2Removed

            Assert.That(caseListRemovedDescendingUnitId[2].CaseType, Is.EqualTo(aCase2Removed.Type));
            Assert.That(caseListRemovedDescendingUnitId[2].CaseUId, Is.EqualTo(aCase2Removed.CaseUid));
            Assert.That(caseListRemovedDescendingUnitId[2].CheckUIid, Is.EqualTo(aCase2Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDescendingUnitId[2].CreatedAt.ToString(), Is.EqualTo(c2Removed_ca.ToString()));
            Assert.That(caseListRemovedDescendingUnitId[2].Custom, Is.EqualTo(aCase2Removed.Custom));
            Assert.That(caseListRemovedDescendingUnitId[2].DoneAt.ToString(), Is.EqualTo(c2Removed_da.ToString()));
            Assert.That(caseListRemovedDescendingUnitId[2].Id, Is.EqualTo(aCase2Removed.Id));
            Assert.That(caseListRemovedDescendingUnitId[2].MicrotingUId, Is.EqualTo(aCase2Removed.MicrotingUid));
            Assert.That(caseListRemovedDescendingUnitId[2].SiteId, Is.EqualTo(aCase2Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDescendingUnitId[2].SiteName, Is.EqualTo(aCase2Removed.Site.Name));
            Assert.That(caseListRemovedDescendingUnitId[2].Status, Is.EqualTo(aCase2Removed.Status));
            Assert.That(caseListRemovedDescendingUnitId[2].TemplatId, Is.EqualTo(aCase2Removed.CheckListId));
            Assert.That(caseListRemovedDescendingUnitId[2].UnitId, Is.EqualTo(aCase2Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedDescendingUnitId[2].UpdatedAt.ToString());
            Assert.That(caseListRemovedDescendingUnitId[2].Version, Is.EqualTo(aCase2Removed.Version));
            Assert.That(caseListRemovedDescendingUnitId[2].WorkerName,
                Is.EqualTo(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName));
            Assert.That(caseListRemovedDescendingUnitId[2].WorkflowState, Is.EqualTo(aCase2Removed.WorkflowState));

            #endregion

            #region caseListRemovedDescendingUnitId aCase3Removed

            Assert.That(caseListRemovedDescendingUnitId[1].CaseType, Is.EqualTo(aCase3Removed.Type));
            Assert.That(caseListRemovedDescendingUnitId[1].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
            Assert.That(caseListRemovedDescendingUnitId[1].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDescendingUnitId[1].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
            Assert.That(caseListRemovedDescendingUnitId[1].Custom, Is.EqualTo(aCase3Removed.Custom));
            Assert.That(caseListRemovedDescendingUnitId[1].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
            Assert.That(caseListRemovedDescendingUnitId[1].Id, Is.EqualTo(aCase3Removed.Id));
            Assert.That(caseListRemovedDescendingUnitId[1].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
            Assert.That(caseListRemovedDescendingUnitId[1].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDescendingUnitId[1].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
            Assert.That(caseListRemovedDescendingUnitId[1].Status, Is.EqualTo(aCase3Removed.Status));
            Assert.That(caseListRemovedDescendingUnitId[1].TemplatId, Is.EqualTo(aCase3Removed.CheckListId));
            Assert.That(caseListRemovedDescendingUnitId[1].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDescendingUnitId[1].UpdatedAt.ToString());
            Assert.That(caseListRemovedDescendingUnitId[1].Version, Is.EqualTo(aCase3Removed.Version));
            Assert.That(caseListRemovedDescendingUnitId[1].WorkerName,
                Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
            Assert.That(caseListRemovedDescendingUnitId[1].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

            #endregion

            #region caseListRemovedDescendingUnitId aCase4Removed

            Assert.That(caseListRemovedDescendingUnitId[0].CaseType, Is.EqualTo(aCase4Removed.Type));
            Assert.That(caseListRemovedDescendingUnitId[0].CaseUId, Is.EqualTo(aCase4Removed.CaseUid));
            Assert.That(caseListRemovedDescendingUnitId[0].CheckUIid, Is.EqualTo(aCase4Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDescendingUnitId[0].CreatedAt.ToString(), Is.EqualTo(c4Removed_ca.ToString()));
            Assert.That(caseListRemovedDescendingUnitId[0].Custom, Is.EqualTo(aCase4Removed.Custom));
            Assert.That(caseListRemovedDescendingUnitId[0].DoneAt.ToString(), Is.EqualTo(c4Removed_da.ToString()));
            Assert.That(caseListRemovedDescendingUnitId[0].Id, Is.EqualTo(aCase4Removed.Id));
            Assert.That(caseListRemovedDescendingUnitId[0].MicrotingUId, Is.EqualTo(aCase4Removed.MicrotingUid));
            Assert.That(caseListRemovedDescendingUnitId[0].SiteId, Is.EqualTo(aCase4Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDescendingUnitId[0].SiteName, Is.EqualTo(aCase4Removed.Site.Name));
            Assert.That(caseListRemovedDescendingUnitId[0].Status, Is.EqualTo(aCase4Removed.Status));
            Assert.That(caseListRemovedDescendingUnitId[0].TemplatId, Is.EqualTo(aCase4Removed.CheckListId));
            Assert.That(caseListRemovedDescendingUnitId[0].UnitId, Is.EqualTo(aCase4Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedDescendingUnitId[0].UpdatedAt.ToString());
            Assert.That(caseListRemovedDescendingUnitId[0].Version, Is.EqualTo(aCase4Removed.Version));
            Assert.That(caseListRemovedDescendingUnitId[0].WorkerName,
                Is.EqualTo(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName));
            Assert.That(caseListRemovedDescendingUnitId[0].WorkflowState, Is.EqualTo(aCase4Removed.WorkflowState));

            #endregion

            #endregion

            #endregion

            #region Def Sort Asc w. DateTime

            #region caseListRemovedDtDoneAt Def Sort Asc w. DateTime

            #region caseListRemovedDtDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedDtDoneAt);
            Assert.That(caseListRemovedDtDoneAt.Count, Is.EqualTo(2));
            Assert.That(caseListRemovedDtDoneAt[1].CaseType, Is.EqualTo(aCase1Removed.Type));
            Assert.That(caseListRemovedDtDoneAt[1].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
            Assert.That(caseListRemovedDtDoneAt[1].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDtDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
            Assert.That(caseListRemovedDtDoneAt[1].Custom, Is.EqualTo(aCase1Removed.Custom));
            Assert.That(caseListRemovedDtDoneAt[1].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
            Assert.That(caseListRemovedDtDoneAt[1].Id, Is.EqualTo(aCase1Removed.Id));
            Assert.That(caseListRemovedDtDoneAt[1].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
            Assert.That(caseListRemovedDtDoneAt[1].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDtDoneAt[1].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
            Assert.That(caseListRemovedDtDoneAt[1].Status, Is.EqualTo(aCase1Removed.Status));
            Assert.That(caseListRemovedDtDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRemovedDtDoneAt[1].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtDoneAt[1].UpdatedAt.ToString());
            Assert.That(caseListRemovedDtDoneAt[1].Version, Is.EqualTo(aCase1Removed.Version));
            Assert.That(caseListRemovedDtDoneAt[1].WorkerName,
                Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
            Assert.That(caseListRemovedDtDoneAt[1].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

            #endregion

            #region caseListRemovedDtDoneAt aCase3Removed

            Assert.That(caseListRemovedDtDoneAt[0].CaseType, Is.EqualTo(aCase3Removed.Type));
            Assert.That(caseListRemovedDtDoneAt[0].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
            Assert.That(caseListRemovedDtDoneAt[0].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDtDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
            Assert.That(caseListRemovedDtDoneAt[0].Custom, Is.EqualTo(aCase3Removed.Custom));
            Assert.That(caseListRemovedDtDoneAt[0].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
            Assert.That(caseListRemovedDtDoneAt[0].Id, Is.EqualTo(aCase3Removed.Id));
            Assert.That(caseListRemovedDtDoneAt[0].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
            Assert.That(caseListRemovedDtDoneAt[0].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDtDoneAt[0].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
            Assert.That(caseListRemovedDtDoneAt[0].Status, Is.EqualTo(aCase3Removed.Status));
            Assert.That(caseListRemovedDtDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRemovedDtDoneAt[0].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtDoneAt[0].UpdatedAt.ToString());
            Assert.That(caseListRemovedDtDoneAt[0].Version, Is.EqualTo(aCase3Removed.Version));
            Assert.That(caseListRemovedDtDoneAt[0].WorkerName,
                Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
            Assert.That(caseListRemovedDtDoneAt[0].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

            #endregion

            #endregion

            #region caseListRemovedDtStatus Def Sort Asc w. DateTime

            #region caseListRemovedDtStatus aCase1Removed

            Assert.NotNull(caseListRemovedDtStatus);
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

            Assert.NotNull(caseListRemovedDtUnitId);
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

            #region Def Sort Des w. DateTime

            #region caseListRemovedDtDescendingDoneAt Def Sort Des

            #region caseListRemovedDtDescendingDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedDtDescendingDoneAt);
            Assert.That(caseListRemovedDtDescendingDoneAt.Count, Is.EqualTo(2));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].CaseType, Is.EqualTo(aCase1Removed.Type));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].Custom, Is.EqualTo(aCase1Removed.Custom));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].Id, Is.EqualTo(aCase1Removed.Id));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].Status, Is.EqualTo(aCase1Removed.Status));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtDescendingDoneAt[0].UpdatedAt.ToString());
            Assert.That(caseListRemovedDtDescendingDoneAt[0].Version, Is.EqualTo(aCase1Removed.Version));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].WorkerName,
                Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
            Assert.That(caseListRemovedDtDescendingDoneAt[0].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

            #endregion

            #region caseListRemovedDtDescendingDoneAt aCase3Removed

            Assert.That(caseListRemovedDtDescendingDoneAt[1].CaseType, Is.EqualTo(aCase3Removed.Type));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].Custom, Is.EqualTo(aCase3Removed.Custom));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].Id, Is.EqualTo(aCase3Removed.Id));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].Status, Is.EqualTo(aCase3Removed.Status));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtDescendingDoneAt[1].UpdatedAt.ToString());
            Assert.That(caseListRemovedDtDescendingDoneAt[1].Version, Is.EqualTo(aCase3Removed.Version));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].WorkerName,
                Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
            Assert.That(caseListRemovedDtDescendingDoneAt[1].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

            #endregion

            #endregion

            #region caseListRemovedDtDescendingStatus Def Sort Des

            #region caseListRemovedDtDescendingStatus aCase1Removed

            Assert.NotNull(caseListRemovedDtDescendingStatus);
            Assert.That(caseListRemovedDtDescendingStatus.Count, Is.EqualTo(2));
            Assert.That(caseListRemovedDtDescendingStatus[1].CaseType, Is.EqualTo(aCase1Removed.Type));
            Assert.That(caseListRemovedDtDescendingStatus[1].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
            Assert.That(caseListRemovedDtDescendingStatus[1].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDtDescendingStatus[1].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
            Assert.That(caseListRemovedDtDescendingStatus[1].Custom, Is.EqualTo(aCase1Removed.Custom));
            Assert.That(caseListRemovedDtDescendingStatus[1].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
            Assert.That(caseListRemovedDtDescendingStatus[1].Id, Is.EqualTo(aCase1Removed.Id));
            Assert.That(caseListRemovedDtDescendingStatus[1].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
            Assert.That(caseListRemovedDtDescendingStatus[1].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDtDescendingStatus[1].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
            Assert.That(caseListRemovedDtDescendingStatus[1].Status, Is.EqualTo(aCase1Removed.Status));
            Assert.That(caseListRemovedDtDescendingStatus[1].TemplatId, Is.EqualTo(aCase1Removed.CheckListId));
            Assert.That(caseListRemovedDtDescendingStatus[1].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtDescendingStatus[1].UpdatedAt.ToString());
            Assert.That(caseListRemovedDtDescendingStatus[1].Version, Is.EqualTo(aCase1Removed.Version));
            Assert.That(caseListRemovedDtDescendingStatus[1].WorkerName,
                Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
            Assert.That(caseListRemovedDtDescendingStatus[1].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

            #endregion


            #region caseListRemovedDtDescendingStatus aCase3Removed

            Assert.That(caseListRemovedDtDescendingStatus[0].CaseType, Is.EqualTo(aCase3Removed.Type));
            Assert.That(caseListRemovedDtDescendingStatus[0].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
            Assert.That(caseListRemovedDtDescendingStatus[0].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDtDescendingStatus[0].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
            Assert.That(caseListRemovedDtDescendingStatus[0].Custom, Is.EqualTo(aCase3Removed.Custom));
            Assert.That(caseListRemovedDtDescendingStatus[0].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
            Assert.That(caseListRemovedDtDescendingStatus[0].Id, Is.EqualTo(aCase3Removed.Id));
            Assert.That(caseListRemovedDtDescendingStatus[0].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
            Assert.That(caseListRemovedDtDescendingStatus[0].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDtDescendingStatus[0].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
            Assert.That(caseListRemovedDtDescendingStatus[0].Status, Is.EqualTo(aCase3Removed.Status));
            Assert.That(caseListRemovedDtDescendingStatus[0].TemplatId, Is.EqualTo(aCase3Removed.CheckListId));
            Assert.That(caseListRemovedDtDescendingStatus[0].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtDescendingStatus[0].UpdatedAt.ToString());
            Assert.That(caseListRemovedDtDescendingStatus[0].Version, Is.EqualTo(aCase3Removed.Version));
            Assert.That(caseListRemovedDtDescendingStatus[0].WorkerName,
                Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
            Assert.That(caseListRemovedDtDescendingStatus[0].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

            #endregion

            #endregion

            #region caseListRemovedDtDescendingUnitId Def Sort Des

            #region caseListRemovedDtDescendingUnitId aCase1Removed

            Assert.NotNull(caseListRemovedDtDescendingUnitId);
            Assert.That(caseListRemovedDtDescendingUnitId.Count, Is.EqualTo(2));

            Assert.That(caseListRemovedDtDescendingUnitId[1].CaseType, Is.EqualTo(aCase1Removed.Type));
            Assert.That(caseListRemovedDtDescendingUnitId[1].CaseUId, Is.EqualTo(aCase1Removed.CaseUid));
            Assert.That(caseListRemovedDtDescendingUnitId[1].CheckUIid, Is.EqualTo(aCase1Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDtDescendingUnitId[1].CreatedAt.ToString(), Is.EqualTo(c1Removed_ca.ToString()));
            Assert.That(caseListRemovedDtDescendingUnitId[1].Custom, Is.EqualTo(aCase1Removed.Custom));
            Assert.That(caseListRemovedDtDescendingUnitId[1].DoneAt.ToString(), Is.EqualTo(c1Removed_da.ToString()));
            Assert.That(caseListRemovedDtDescendingUnitId[1].Id, Is.EqualTo(aCase1Removed.Id));
            Assert.That(caseListRemovedDtDescendingUnitId[1].MicrotingUId, Is.EqualTo(aCase1Removed.MicrotingUid));
            Assert.That(caseListRemovedDtDescendingUnitId[1].SiteId, Is.EqualTo(aCase1Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDtDescendingUnitId[1].SiteName, Is.EqualTo(aCase1Removed.Site.Name));
            Assert.That(caseListRemovedDtDescendingUnitId[1].Status, Is.EqualTo(aCase1Removed.Status));
            Assert.That(caseListRemovedDtDescendingUnitId[1].TemplatId, Is.EqualTo(aCase1Removed.CheckListId));
            Assert.That(caseListRemovedDtDescendingUnitId[1].UnitId, Is.EqualTo(aCase1Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtDescendingUnitId[1].UpdatedAt.ToString());
            Assert.That(caseListRemovedDtDescendingUnitId[1].Version, Is.EqualTo(aCase1Removed.Version));
            Assert.That(caseListRemovedDtDescendingUnitId[1].WorkerName,
                Is.EqualTo(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName));
            Assert.That(caseListRemovedDtDescendingUnitId[1].WorkflowState, Is.EqualTo(aCase1Removed.WorkflowState));

            #endregion


            #region caseListRemovedDtDescendingUnitId aCase3Removed

            Assert.That(caseListRemovedDtDescendingUnitId[0].CaseType, Is.EqualTo(aCase3Removed.Type));
            Assert.That(caseListRemovedDtDescendingUnitId[0].CaseUId, Is.EqualTo(aCase3Removed.CaseUid));
            Assert.That(caseListRemovedDtDescendingUnitId[0].CheckUIid, Is.EqualTo(aCase3Removed.MicrotingCheckUid));
            Assert.That(caseListRemovedDtDescendingUnitId[0].CreatedAt.ToString(), Is.EqualTo(c3Removed_ca.ToString()));
            Assert.That(caseListRemovedDtDescendingUnitId[0].Custom, Is.EqualTo(aCase3Removed.Custom));
            Assert.That(caseListRemovedDtDescendingUnitId[0].DoneAt.ToString(), Is.EqualTo(c3Removed_da.ToString()));
            Assert.That(caseListRemovedDtDescendingUnitId[0].Id, Is.EqualTo(aCase3Removed.Id));
            Assert.That(caseListRemovedDtDescendingUnitId[0].MicrotingUId, Is.EqualTo(aCase3Removed.MicrotingUid));
            Assert.That(caseListRemovedDtDescendingUnitId[0].SiteId, Is.EqualTo(aCase3Removed.Site.MicrotingUid));
            Assert.That(caseListRemovedDtDescendingUnitId[0].SiteName, Is.EqualTo(aCase3Removed.Site.Name));
            Assert.That(caseListRemovedDtDescendingUnitId[0].Status, Is.EqualTo(aCase3Removed.Status));
            Assert.That(caseListRemovedDtDescendingUnitId[0].TemplatId, Is.EqualTo(aCase3Removed.CheckListId));
            Assert.That(caseListRemovedDtDescendingUnitId[0].UnitId, Is.EqualTo(aCase3Removed.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtDescendingUnitId[0].UpdatedAt.ToString());
            Assert.That(caseListRemovedDtDescendingUnitId[0].Version, Is.EqualTo(aCase3Removed.Version));
            Assert.That(caseListRemovedDtDescendingUnitId[0].WorkerName,
                Is.EqualTo(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName));
            Assert.That(caseListRemovedDtDescendingUnitId[0].WorkflowState, Is.EqualTo(aCase3Removed.WorkflowState));

            #endregion

            #endregion

            #endregion

            #endregion

            #region Case Sort

            #region aCase Sort Asc

            #region aCase1Removed sort asc

            #region caseListC1DoneAt aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDoneAt);
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

            Assert.NotNull(caseListRemovedC1SortStatus);
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

            Assert.NotNull(caseListRemovedC1SortUnitId);
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

            Assert.NotNull(caseListRemovedC2SortDoneAt);
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

            Assert.NotNull(caseListRemovedC2SortStatus);
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

            Assert.NotNull(caseListRemovedC2SortUnitId);
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

            Assert.NotNull(caseListRemovedC3SortDoneAt);
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

            Assert.NotNull(caseListRemovedC3SortStatus);
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

            Assert.NotNull(caseListRemovedC3SortUnitId);
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

            Assert.NotNull(caseListRemovedC4SortDoneAt);
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

            Assert.NotNull(caseListRemovedC4SortStatus);
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

            Assert.NotNull(caseListRemovedC4SortUnitId);
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

            #region aCase Sort Des

            #region aCase1Removed Sort Des

            #region caseListRemovedC1SortDescendingDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDescendingDoneAt);
            Assert.That(caseListRemovedC1SortDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1Removed.type, caseListRemovedC1SortDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase1Removed.case_uid, caseListRemovedC1SortDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase1Removed.microting_check_uid, caseListRemovedC1SortDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedC1SortDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.custom, caseListRemovedC1SortDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedC1SortDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Removed.Id, caseListRemovedC1SortDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase1Removed.microting_uid, caseListRemovedC1SortDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1Removed.site.microting_uid, caseListRemovedC1SortDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1Removed.site.name, caseListRemovedC1SortDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1Removed.status, caseListRemovedC1SortDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase1Removed.check_list_id, caseListRemovedC1SortDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1Removed.unit.microting_uid, caseListRemovedC1SortDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedC1SortDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.version, caseListRemovedC1SortDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase1Removed.worker.first_name + " " + aCase1Removed.worker.last_name, caseListRemovedC1SortDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase1Removed.workflow_state, caseListRemovedC1SortDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC1SortDescendingStatus aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDescendingStatus);
            Assert.That(caseListRemovedC1SortDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1Removed.type, caseListRemovedC1SortDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase1Removed.case_uid, caseListRemovedC1SortDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase1Removed.microting_check_uid, caseListRemovedC1SortDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedC1SortDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.custom, caseListRemovedC1SortDescendingStatus[0].Custom);
            // Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedC1SortDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Removed.Id, caseListRemovedC1SortDescendingStatus[0].Id);
            // Assert.AreEqual(aCase1Removed.microting_uid, caseListRemovedC1SortDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1Removed.site.microting_uid, caseListRemovedC1SortDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase1Removed.site.name, caseListRemovedC1SortDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase1Removed.status, caseListRemovedC1SortDescendingStatus[0].Status);
            // Assert.AreEqual(aCase1Removed.check_list_id, caseListRemovedC1SortDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase1Removed.unit.microting_uid, caseListRemovedC1SortDescendingStatus[0].UnitId);
            // Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedC1SortDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.version, caseListRemovedC1SortDescendingStatus[0].Version);
            // Assert.AreEqual(aCase1Removed.worker.first_name + " " + aCase1Removed.worker.last_name, caseListRemovedC1SortDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase1Removed.workflow_state, caseListRemovedC1SortDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC1SortDescendingUnitId aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDescendingUnitId);
            Assert.That(caseListRemovedC1SortDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1Removed.type, caseListRemovedC1SortDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase1Removed.case_uid, caseListRemovedC1SortDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase1Removed.microting_check_uid, caseListRemovedC1SortDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedC1SortDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.custom, caseListRemovedC1SortDescendingUnitId[0].Custom);
            // Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedC1SortDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Removed.Id, caseListRemovedC1SortDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase1Removed.microting_uid, caseListRemovedC1SortDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1Removed.site.microting_uid, caseListRemovedC1SortDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase1Removed.site.name, caseListRemovedC1SortDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase1Removed.status, caseListRemovedC1SortDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase1Removed.check_list_id, caseListRemovedC1SortDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1Removed.unit.microting_uid, caseListRemovedC1SortDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedC1SortDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.version, caseListRemovedC1SortDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase1Removed.worker.first_name + " " + aCase1Removed.worker.last_name, caseListRemovedC1SortDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase1Removed.workflow_state, caseListRemovedC1SortDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase2Removed Sort Des

            #region caseListRemovedC2SortDescendingDoneAt aCase2Removed

            Assert.NotNull(caseListRemovedC2SortDescendingDoneAt);
            Assert.That(caseListRemovedC2SortDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2Removed.type, caseListRemovedC2SortDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase2Removed.case_uid, caseListRemovedC2SortDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase2Removed.microting_check_uid, caseListRemovedC2SortDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedC2SortDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.custom, caseListRemovedC2SortDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedC2SortDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Removed.Id, caseListRemovedC2SortDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase2Removed.microting_uid, caseListRemovedC2SortDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2Removed.site.microting_uid, caseListRemovedC2SortDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2Removed.site.name, caseListRemovedC2SortDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2Removed.status, caseListRemovedC2SortDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase2Removed.check_list_id, caseListRemovedC2SortDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2Removed.unit.microting_uid, caseListRemovedC2SortDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedC2SortDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.version, caseListRemovedC2SortDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase2Removed.worker.first_name + " " + aCase2Removed.worker.last_name, caseListRemovedC2SortDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase2Removed.workflow_state, caseListRemovedC2SortDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC2SortDescendingStatus aCase2Removed

            Assert.NotNull(caseListRemovedC2SortDescendingStatus);
            Assert.That(caseListRemovedC2SortDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2Removed.type, caseListRemovedC2SortDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase2Removed.case_uid, caseListRemovedC2SortDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase2Removed.microting_check_uid, caseListRemovedC2SortDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedC2SortDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.custom, caseListRemovedC2SortDescendingStatus[0].Custom);
            // Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedC2SortDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Removed.Id, caseListRemovedC2SortDescendingStatus[0].Id);
            // Assert.AreEqual(aCase2Removed.microting_uid, caseListRemovedC2SortDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2Removed.site.microting_uid, caseListRemovedC2SortDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase2Removed.site.name, caseListRemovedC2SortDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase2Removed.status, caseListRemovedC2SortDescendingStatus[0].Status);
            // Assert.AreEqual(aCase2Removed.check_list_id, caseListRemovedC2SortDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase2Removed.unit.microting_uid, caseListRemovedC2SortDescendingStatus[0].UnitId);
            // Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedC2SortDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.version, caseListRemovedC2SortDescendingStatus[0].Version);
            // Assert.AreEqual(aCase2Removed.worker.first_name + " " + aCase2Removed.worker.last_name, caseListRemovedC2SortDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase2Removed.workflow_state, caseListRemovedC2SortDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC2SortDescendingUnitId aCase2Removed

            Assert.NotNull(caseListRemovedC2SortDescendingUnitId);
            Assert.That(caseListRemovedC2SortDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2Removed.type, caseListRemovedC2SortDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase2Removed.case_uid, caseListRemovedC2SortDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase2Removed.microting_check_uid, caseListRemovedC2SortDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedC2SortDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.custom, caseListRemovedC2SortDescendingUnitId[0].Custom);
            // Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedC2SortDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Removed.Id, caseListRemovedC2SortDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase2Removed.microting_uid, caseListRemovedC2SortDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2Removed.site.microting_uid, caseListRemovedC2SortDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase2Removed.site.name, caseListRemovedC2SortDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase2Removed.status, caseListRemovedC2SortDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase2Removed.check_list_id, caseListRemovedC2SortDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2Removed.unit.microting_uid, caseListRemovedC2SortDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedC2SortDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.version, caseListRemovedC2SortDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase2Removed.worker.first_name + " " + aCase2Removed.worker.last_name, caseListRemovedC2SortDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase2Removed.workflow_state, caseListRemovedC2SortDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase3Removed Sort Des

            #region caseListRemovedC3SortDescendingDoneAt aCase3Removed

            Assert.NotNull(caseListRemovedC3SortDescendingDoneAt);
            Assert.That(caseListRemovedC3SortDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3Removed.type, caseListRemovedC3SortDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase3Removed.case_uid, caseListRemovedC3SortDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase3Removed.microting_check_uid, caseListRemovedC3SortDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedC3SortDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.custom, caseListRemovedC3SortDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedC3SortDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Removed.Id, caseListRemovedC3SortDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase3Removed.microting_uid, caseListRemovedC3SortDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3Removed.site.microting_uid, caseListRemovedC3SortDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3Removed.site.name, caseListRemovedC3SortDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3Removed.status, caseListRemovedC3SortDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase3Removed.check_list_id, caseListRemovedC3SortDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3Removed.unit.microting_uid, caseListRemovedC3SortDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedC3SortDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.version, caseListRemovedC3SortDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase3Removed.worker.first_name + " " + aCase3Removed.worker.last_name, caseListRemovedC3SortDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase3Removed.workflow_state, caseListRemovedC3SortDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC3SortDescendingStatus aCase3Removed

            Assert.NotNull(caseListRemovedC3SortDescendingStatus);
            Assert.That(caseListRemovedC3SortDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3Removed.type, caseListRemovedC3SortDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase3Removed.case_uid, caseListRemovedC3SortDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase3Removed.microting_check_uid, caseListRemovedC3SortDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedC3SortDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.custom, caseListRemovedC3SortDescendingStatus[0].Custom);
            // Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedC3SortDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Removed.Id, caseListRemovedC3SortDescendingStatus[0].Id);
            // Assert.AreEqual(aCase3Removed.microting_uid, caseListRemovedC3SortDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3Removed.site.microting_uid, caseListRemovedC3SortDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase3Removed.site.name, caseListRemovedC3SortDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase3Removed.status, caseListRemovedC3SortDescendingStatus[0].Status);
            // Assert.AreEqual(aCase3Removed.check_list_id, caseListRemovedC3SortDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase3Removed.unit.microting_uid, caseListRemovedC3SortDescendingStatus[0].UnitId);
            // Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedC3SortDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.version, caseListRemovedC3SortDescendingStatus[0].Version);
            // Assert.AreEqual(aCase3Removed.worker.first_name + " " + aCase3Removed.worker.last_name, caseListRemovedC3SortDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase3Removed.workflow_state, caseListRemovedC3SortDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC3SortDescendingUnitId aCase3Removed

            Assert.NotNull(caseListRemovedC3SortDescendingUnitId);
            Assert.That(caseListRemovedC3SortDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3Removed.type, caseListRemovedC3SortDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase3Removed.case_uid, caseListRemovedC3SortDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase3Removed.microting_check_uid, caseListRemovedC3SortDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedC3SortDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.custom, caseListRemovedC3SortDescendingUnitId[0].Custom);
            // Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedC3SortDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Removed.Id, caseListRemovedC3SortDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase3Removed.microting_uid, caseListRemovedC3SortDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3Removed.site.microting_uid, caseListRemovedC3SortDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase3Removed.site.name, caseListRemovedC3SortDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase3Removed.status, caseListRemovedC3SortDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase3Removed.check_list_id, caseListRemovedC3SortDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3Removed.unit.microting_uid, caseListRemovedC3SortDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedC3SortDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.version, caseListRemovedC3SortDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase3Removed.worker.first_name + " " + aCase3Removed.worker.last_name, caseListRemovedC3SortDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase3Removed.workflow_state, caseListRemovedC3SortDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase4Removed Sort Des

            #region caseListRemovedC4SortDescendingDoneAt aCase4Removed

            Assert.NotNull(caseListRemovedC4SortDescendingDoneAt);
            Assert.That(caseListRemovedC4SortDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4Removed.type, caseListRemovedC4SortDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4Removed.case_uid, caseListRemovedC4SortDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4Removed.microting_check_uid, caseListRemovedC4SortDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedC4SortDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.custom, caseListRemovedC4SortDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedC4SortDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Removed.Id, caseListRemovedC4SortDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase4Removed.microting_uid, caseListRemovedC4SortDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4Removed.site.microting_uid, caseListRemovedC4SortDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4Removed.site.name, caseListRemovedC4SortDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4Removed.status, caseListRemovedC4SortDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase4Removed.check_list_id, caseListRemovedC4SortDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4Removed.unit.microting_uid, caseListRemovedC4SortDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedC4SortDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.version, caseListRemovedC4SortDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase4Removed.worker.first_name + " " + aCase4Removed.worker.last_name, caseListRemovedC4SortDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4Removed.workflow_state, caseListRemovedC4SortDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC4SortDescendingStatus aCase4Removed

            Assert.NotNull(caseListRemovedC4SortDescendingStatus);
            Assert.That(caseListRemovedC4SortDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4Removed.type, caseListRemovedC4SortDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase4Removed.case_uid, caseListRemovedC4SortDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase4Removed.microting_check_uid, caseListRemovedC4SortDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedC4SortDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.custom, caseListRemovedC4SortDescendingStatus[0].Custom);
            // Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedC4SortDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Removed.Id, caseListRemovedC4SortDescendingStatus[0].Id);
            // Assert.AreEqual(aCase4Removed.microting_uid, caseListRemovedC4SortDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase4Removed.site.microting_uid, caseListRemovedC4SortDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase4Removed.site.name, caseListRemovedC4SortDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase4Removed.status, caseListRemovedC4SortDescendingStatus[0].Status);
            // Assert.AreEqual(aCase4Removed.check_list_id, caseListRemovedC4SortDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase4Removed.unit.microting_uid, caseListRemovedC4SortDescendingStatus[0].UnitId);
            // Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedC4SortDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.version, caseListRemovedC4SortDescendingStatus[0].Version);
            // Assert.AreEqual(aCase4Removed.worker.first_name + " " + aCase4Removed.worker.last_name, caseListRemovedC4SortDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase4Removed.workflow_state, caseListRemovedC4SortDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC4SortDescendingUnitId aCase4Removed

            Assert.NotNull(caseListRemovedC4SortDescendingUnitId);
            Assert.That(caseListRemovedC4SortDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4Removed.type, caseListRemovedC4SortDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase4Removed.case_uid, caseListRemovedC4SortDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase4Removed.microting_check_uid, caseListRemovedC4SortDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedC4SortDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.custom, caseListRemovedC4SortDescendingUnitId[0].Custom);
            // Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedC4SortDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Removed.Id, caseListRemovedC4SortDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase4Removed.microting_uid, caseListRemovedC4SortDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase4Removed.site.microting_uid, caseListRemovedC4SortDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase4Removed.site.name, caseListRemovedC4SortDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase4Removed.status, caseListRemovedC4SortDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase4Removed.check_list_id, caseListRemovedC4SortDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase4Removed.unit.microting_uid, caseListRemovedC4SortDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedC4SortDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.version, caseListRemovedC4SortDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase4Removed.worker.first_name + " " + aCase4Removed.worker.last_name, caseListRemovedC4SortDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase4Removed.workflow_state, caseListRemovedC4SortDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #endregion

            #region aCase Sort Asc w. DateTime

            #region aCase1Removed sort asc w. DateTime

            #region caseListRemovedC1SortDtDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDtDoneAt);
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

            Assert.NotNull(caseListRemovedC1SortDtStatus);
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

            Assert.NotNull(caseListRemovedC1SortDtUnitId);
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

            Assert.NotNull(caseListRemovedC2SortDtDoneAt);
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

            Assert.NotNull(caseListRemovedC2SortDtStatus);
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

            Assert.NotNull(caseListRemovedC2SortDtUnitId);
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

            Assert.NotNull(caseListRemovedC3SortDtDoneAt);
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

            Assert.NotNull(caseListRemovedC3SortDtStatus);
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

            Assert.NotNull(caseListRemovedC3SortDtUnitId);
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

            Assert.NotNull(caseListRemovedC4SortDtDoneAt);
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

            Assert.NotNull(caseListRemovedC4SortDtStatus);
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

            Assert.NotNull(caseListRemovedC4SortDtUnitId);
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

            #region aCase Sort Des w. DateTime

            #region aCase1Removed Sort Des w. DateTime

            #region caseListRemovedC1SortDtDescendingDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDtDescendingDoneAt);
            Assert.That(caseListRemovedC1SortDtDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1Removed.type, caseListRemovedC1SortDtDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase1Removed.case_uid, caseListRemovedC1SortDtDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase1Removed.microting_check_uid, caseListRemovedC1SortDtDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedC1SortDtDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.custom, caseListRemovedC1SortDtDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedC1SortDtDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Removed.Id, caseListRemovedC1SortDtDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase1Removed.microting_uid, caseListRemovedC1SortDtDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1Removed.site.microting_uid, caseListRemovedC1SortDtDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1Removed.site.name, caseListRemovedC1SortDtDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1Removed.status, caseListRemovedC1SortDtDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase1Removed.check_list_id, caseListRemovedC1SortDtDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1Removed.unit.microting_uid, caseListRemovedC1SortDtDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedC1SortDtDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.version, caseListRemovedC1SortDtDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase1Removed.worker.first_name + " " + aCase1Removed.worker.last_name, caseListRemovedC1SortDtDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase1Removed.workflow_state, caseListRemovedC1SortDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC1SortDtDescendingStatus aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDtDescendingStatus);
            Assert.That(caseListRemovedC1SortDtDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1Removed.type, caseListRemovedC1SortDtDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase1Removed.case_uid, caseListRemovedC1SortDtDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase1Removed.microting_check_uid, caseListRemovedC1SortDtDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedC1SortDtDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.custom, caseListRemovedC1SortDtDescendingStatus[0].Custom);
            // Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedC1SortDtDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Removed.Id, caseListRemovedC1SortDtDescendingStatus[0].Id);
            // Assert.AreEqual(aCase1Removed.microting_uid, caseListRemovedC1SortDtDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1Removed.site.microting_uid, caseListRemovedC1SortDtDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase1Removed.site.name, caseListRemovedC1SortDtDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase1Removed.status, caseListRemovedC1SortDtDescendingStatus[0].Status);
            // Assert.AreEqual(aCase1Removed.check_list_id, caseListRemovedC1SortDtDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase1Removed.unit.microting_uid, caseListRemovedC1SortDtDescendingStatus[0].UnitId);
            // Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedC1SortDtDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.version, caseListRemovedC1SortDtDescendingStatus[0].Version);
            // Assert.AreEqual(aCase1Removed.worker.first_name + " " + aCase1Removed.worker.last_name, caseListRemovedC1SortDtDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase1Removed.workflow_state, caseListRemovedC1SortDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC1SortDtDescendingUnitId aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDtDescendingUnitId);
            Assert.That(caseListRemovedC1SortDtDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1Removed.type, caseListRemovedC1SortDtDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase1Removed.case_uid, caseListRemovedC1SortDtDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase1Removed.microting_check_uid, caseListRemovedC1SortDtDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedC1SortDtDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.custom, caseListRemovedC1SortDtDescendingUnitId[0].Custom);
            // Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedC1SortDtDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Removed.Id, caseListRemovedC1SortDtDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase1Removed.microting_uid, caseListRemovedC1SortDtDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1Removed.site.microting_uid, caseListRemovedC1SortDtDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase1Removed.site.name, caseListRemovedC1SortDtDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase1Removed.status, caseListRemovedC1SortDtDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase1Removed.check_list_id, caseListRemovedC1SortDtDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1Removed.unit.microting_uid, caseListRemovedC1SortDtDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedC1SortDtDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Removed.version, caseListRemovedC1SortDtDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase1Removed.worker.first_name + " " + aCase1Removed.worker.last_name, caseListRemovedC1SortDtDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase1Removed.workflow_state, caseListRemovedC1SortDtDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase2Removed Sort Des w. DateTime

            #region caseListRemovedC2SortDtDescendingDoneAt aCase2Removed

            Assert.NotNull(caseListRemovedC2SortDtDescendingDoneAt);
            Assert.That(caseListRemovedC2SortDtDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2Removed.type, caseListRemovedC2SortDtDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase2Removed.case_uid, caseListRemovedC2SortDtDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase2Removed.microting_check_uid, caseListRemovedC2SortDtDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedC2SortDtDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.custom, caseListRemovedC2SortDtDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedC2SortDtDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Removed.Id, caseListRemovedC2SortDtDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase2Removed.microting_uid, caseListRemovedC2SortDtDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2Removed.site.microting_uid, caseListRemovedC2SortDtDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2Removed.site.name, caseListRemovedC2SortDtDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2Removed.status, caseListRemovedC2SortDtDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase2Removed.check_list_id, caseListRemovedC2SortDtDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2Removed.unit.microting_uid, caseListRemovedC2SortDtDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedC2SortDtDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.version, caseListRemovedC2SortDtDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase2Removed.worker.first_name + " " + aCase2Removed.worker.last_name, caseListRemovedC2SortDtDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase2Removed.workflow_state, caseListRemovedC2SortDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC2SortDtDescendingStatus aCase2Removed

            Assert.NotNull(caseListRemovedC2SortDtDescendingStatus);
            Assert.That(caseListRemovedC2SortDtDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2Removed.type, caseListRemovedC2SortDtDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase2Removed.case_uid, caseListRemovedC2SortDtDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase2Removed.microting_check_uid, caseListRemovedC2SortDtDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedC2SortDtDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.custom, caseListRemovedC2SortDtDescendingStatus[0].Custom);
            // Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedC2SortDtDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Removed.Id, caseListRemovedC2SortDtDescendingStatus[0].Id);
            // Assert.AreEqual(aCase2Removed.microting_uid, caseListRemovedC2SortDtDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2Removed.site.microting_uid, caseListRemovedC2SortDtDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase2Removed.site.name, caseListRemovedC2SortDtDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase2Removed.status, caseListRemovedC2SortDtDescendingStatus[0].Status);
            // Assert.AreEqual(aCase2Removed.check_list_id, caseListRemovedC2SortDtDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase2Removed.unit.microting_uid, caseListRemovedC2SortDtDescendingStatus[0].UnitId);
            // Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedC2SortDtDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.version, caseListRemovedC2SortDtDescendingStatus[0].Version);
            // Assert.AreEqual(aCase2Removed.worker.first_name + " " + aCase2Removed.worker.last_name, caseListRemovedC2SortDtDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase2Removed.workflow_state, caseListRemovedC2SortDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC2SortDtDescendingUnitId aCase2Removed

            Assert.NotNull(caseListRemovedC2SortDtDescendingUnitId);
            Assert.That(caseListRemovedC2SortDtDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2Removed.type, caseListRemovedC2SortDtDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase2Removed.case_uid, caseListRemovedC2SortDtDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase2Removed.microting_check_uid, caseListRemovedC2SortDtDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedC2SortDtDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.custom, caseListRemovedC2SortDtDescendingUnitId[0].Custom);
            // Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedC2SortDtDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Removed.Id, caseListRemovedC2SortDtDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase2Removed.microting_uid, caseListRemovedC2SortDtDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2Removed.site.microting_uid, caseListRemovedC2SortDtDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase2Removed.site.name, caseListRemovedC2SortDtDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase2Removed.status, caseListRemovedC2SortDtDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase2Removed.check_list_id, caseListRemovedC2SortDtDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2Removed.unit.microting_uid, caseListRemovedC2SortDtDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedC2SortDtDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Removed.version, caseListRemovedC2SortDtDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase2Removed.worker.first_name + " " + aCase2Removed.worker.last_name, caseListRemovedC2SortDtDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase2Removed.workflow_state, caseListRemovedC2SortDtDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase3Removed Sort Des w. DateTime

            #region caseListRemovedC3SortDtDescendingDoneAt aCase3Removed

            Assert.NotNull(caseListRemovedC3SortDtDescendingDoneAt);
            Assert.That(caseListRemovedC3SortDtDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3Removed.type, caseListRemovedC3SortDtDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase3Removed.case_uid, caseListRemovedC3SortDtDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase3Removed.microting_check_uid, caseListRemovedC3SortDtDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedC3SortDtDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.custom, caseListRemovedC3SortDtDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedC3SortDtDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Removed.Id, caseListRemovedC3SortDtDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase3Removed.microting_uid, caseListRemovedC3SortDtDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3Removed.site.microting_uid, caseListRemovedC3SortDtDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3Removed.site.name, caseListRemovedC3SortDtDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3Removed.status, caseListRemovedC3SortDtDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase3Removed.check_list_id, caseListRemovedC3SortDtDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3Removed.unit.microting_uid, caseListRemovedC3SortDtDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedC3SortDtDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.version, caseListRemovedC3SortDtDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase3Removed.worker.first_name + " " + aCase3Removed.worker.last_name, caseListRemovedC3SortDtDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase3Removed.workflow_state, caseListRemovedC3SortDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC3SortDtDescendingStatus aCase3Removed

            Assert.NotNull(caseListRemovedC3SortDtDescendingStatus);
            Assert.That(caseListRemovedC3SortDtDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3Removed.type, caseListRemovedC3SortDtDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase3Removed.case_uid, caseListRemovedC3SortDtDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase3Removed.microting_check_uid, caseListRemovedC3SortDtDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedC3SortDtDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.custom, caseListRemovedC3SortDtDescendingStatus[0].Custom);
            // Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedC3SortDtDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Removed.Id, caseListRemovedC3SortDtDescendingStatus[0].Id);
            // Assert.AreEqual(aCase3Removed.microting_uid, caseListRemovedC3SortDtDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3Removed.site.microting_uid, caseListRemovedC3SortDtDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase3Removed.site.name, caseListRemovedC3SortDtDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase3Removed.status, caseListRemovedC3SortDtDescendingStatus[0].Status);
            // Assert.AreEqual(aCase3Removed.check_list_id, caseListRemovedC3SortDtDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase3Removed.unit.microting_uid, caseListRemovedC3SortDtDescendingStatus[0].UnitId);
            // Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedC3SortDtDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.version, caseListRemovedC3SortDtDescendingStatus[0].Version);
            // Assert.AreEqual(aCase3Removed.worker.first_name + " " + aCase3Removed.worker.last_name, caseListRemovedC3SortDtDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase3Removed.workflow_state, caseListRemovedC3SortDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC3SortDtDescendingUnitId aCase3Removed

            Assert.NotNull(caseListRemovedC3SortDtDescendingUnitId);
            Assert.That(caseListRemovedC3SortDtDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3Removed.type, caseListRemovedC3SortDtDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase3Removed.case_uid, caseListRemovedC3SortDtDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase3Removed.microting_check_uid, caseListRemovedC3SortDtDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedC3SortDtDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.custom, caseListRemovedC3SortDtDescendingUnitId[0].Custom);
            // Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedC3SortDtDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Removed.Id, caseListRemovedC3SortDtDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase3Removed.microting_uid, caseListRemovedC3SortDtDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3Removed.site.microting_uid, caseListRemovedC3SortDtDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase3Removed.site.name, caseListRemovedC3SortDtDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase3Removed.status, caseListRemovedC3SortDtDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase3Removed.check_list_id, caseListRemovedC3SortDtDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3Removed.unit.microting_uid, caseListRemovedC3SortDtDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedC3SortDtDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Removed.version, caseListRemovedC3SortDtDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase3Removed.worker.first_name + " " + aCase3Removed.worker.last_name, caseListRemovedC3SortDtDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase3Removed.workflow_state, caseListRemovedC3SortDtDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase4Removed Sort Des w. DateTime

            #region caseListRemovedC4SortDtDescendingDoneAt aCase4Removed

            Assert.NotNull(caseListRemovedC4SortDtDescendingDoneAt);
            Assert.That(caseListRemovedC4SortDtDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4Removed.type, caseListRemovedC4SortDtDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4Removed.case_uid, caseListRemovedC4SortDtDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4Removed.microting_check_uid, caseListRemovedC4SortDtDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedC4SortDtDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.custom, caseListRemovedC4SortDtDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedC4SortDtDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Removed.Id, caseListRemovedC4SortDtDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase4Removed.microting_uid, caseListRemovedC4SortDtDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4Removed.site.microting_uid, caseListRemovedC4SortDtDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4Removed.site.name, caseListRemovedC4SortDtDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4Removed.status, caseListRemovedC4SortDtDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase4Removed.check_list_id, caseListRemovedC4SortDtDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4Removed.unit.microting_uid, caseListRemovedC4SortDtDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedC4SortDtDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.version, caseListRemovedC4SortDtDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase4Removed.worker.first_name + " " + aCase4Removed.worker.last_name, caseListRemovedC4SortDtDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4Removed.workflow_state, caseListRemovedC4SortDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedC4SortDtDescendingStatus aCase4Removed

            Assert.NotNull(caseListRemovedC4SortDtDescendingStatus);
            Assert.That(caseListRemovedC4SortDtDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4Removed.type, caseListRemovedC4SortDtDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase4Removed.case_uid, caseListRemovedC4SortDtDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase4Removed.microting_check_uid, caseListRemovedC4SortDtDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedC4SortDtDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.custom, caseListRemovedC4SortDtDescendingStatus[0].Custom);
            // Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedC4SortDtDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Removed.Id, caseListRemovedC4SortDtDescendingStatus[0].Id);
            // Assert.AreEqual(aCase4Removed.microting_uid, caseListRemovedC4SortDtDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase4Removed.site.microting_uid, caseListRemovedC4SortDtDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase4Removed.site.name, caseListRemovedC4SortDtDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase4Removed.status, caseListRemovedC4SortDtDescendingStatus[0].Status);
            // Assert.AreEqual(aCase4Removed.check_list_id, caseListRemovedC4SortDtDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase4Removed.unit.microting_uid, caseListRemovedC4SortDtDescendingStatus[0].UnitId);
            // Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedC4SortDtDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.version, caseListRemovedC4SortDtDescendingStatus[0].Version);
            // Assert.AreEqual(aCase4Removed.worker.first_name + " " + aCase4Removed.worker.last_name, caseListRemovedC4SortDtDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase4Removed.workflow_state, caseListRemovedC4SortDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRemovedC4SortDtDescendingUnitId aCase4Removed

            Assert.NotNull(caseListRemovedC4SortDtDescendingUnitId);
            Assert.That(caseListRemovedC4SortDtDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4Removed.type, caseListRemovedC4SortDtDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase4Removed.case_uid, caseListRemovedC4SortDtDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase4Removed.microting_check_uid, caseListRemovedC4SortDtDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedC4SortDtDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.custom, caseListRemovedC4SortDtDescendingUnitId[0].Custom);
            // Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedC4SortDtDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Removed.Id, caseListRemovedC4SortDtDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase4Removed.microting_uid, caseListRemovedC4SortDtDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase4Removed.site.microting_uid, caseListRemovedC4SortDtDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase4Removed.site.name, caseListRemovedC4SortDtDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase4Removed.status, caseListRemovedC4SortDtDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase4Removed.check_list_id, caseListRemovedC4SortDtDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase4Removed.unit.microting_uid, caseListRemovedC4SortDtDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedC4SortDtDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Removed.version, caseListRemovedC4SortDtDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase4Removed.worker.first_name + " " + aCase4Removed.worker.last_name, caseListRemovedC4SortDtDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase4Removed.workflow_state, caseListRemovedC4SortDtDescendingUnitId[0].WorkflowState);

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
            Assert.That(caseListRetractedDoneAt.Count, Is.EqualTo(4));
            Assert.That(caseListRetractedDoneAt[1].CaseType, Is.EqualTo(aCase1Retracted.Type));
            Assert.That(caseListRetractedDoneAt[1].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
            Assert.That(caseListRetractedDoneAt[1].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
            Assert.That(caseListRetractedDoneAt[1].Custom, Is.EqualTo(aCase1Retracted.Custom));
            Assert.That(caseListRetractedDoneAt[1].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
            Assert.That(caseListRetractedDoneAt[1].Id, Is.EqualTo(aCase1Retracted.Id));
            Assert.That(caseListRetractedDoneAt[1].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
            Assert.That(caseListRetractedDoneAt[1].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDoneAt[1].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
            Assert.That(caseListRetractedDoneAt[1].Status, Is.EqualTo(aCase1Retracted.Status));
            Assert.That(caseListRetractedDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRetractedDoneAt[1].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDoneAt[1].UpdatedAt.ToString());
            Assert.That(caseListRetractedDoneAt[1].Version, Is.EqualTo(aCase1Retracted.Version));
            Assert.That(caseListRetractedDoneAt[1].WorkerName,
                Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
            Assert.That(caseListRetractedDoneAt[1].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDoneAt aCase2Retracted

            Assert.That(caseListRetractedDoneAt[3].CaseType, Is.EqualTo(aCase2Retracted.Type));
            Assert.That(caseListRetractedDoneAt[3].CaseUId, Is.EqualTo(aCase2Retracted.CaseUid));
            Assert.That(caseListRetractedDoneAt[3].CheckUIid, Is.EqualTo(aCase2Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDoneAt[3].CreatedAt.ToString(), Is.EqualTo(c2Retracted_ca.ToString()));
            Assert.That(caseListRetractedDoneAt[3].Custom, Is.EqualTo(aCase2Retracted.Custom));
            Assert.That(caseListRetractedDoneAt[3].DoneAt.ToString(), Is.EqualTo(c2Retracted_da.ToString()));
            Assert.That(caseListRetractedDoneAt[3].Id, Is.EqualTo(aCase2Retracted.Id));
            Assert.That(caseListRetractedDoneAt[3].MicrotingUId, Is.EqualTo(aCase2Retracted.MicrotingUid));
            Assert.That(caseListRetractedDoneAt[3].SiteId, Is.EqualTo(aCase2Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDoneAt[3].SiteName, Is.EqualTo(aCase2Retracted.Site.Name));
            Assert.That(caseListRetractedDoneAt[3].Status, Is.EqualTo(aCase2Retracted.Status));
            Assert.That(caseListRetractedDoneAt[3].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRetractedDoneAt[3].UnitId, Is.EqualTo(aCase2Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedDoneAt[3].UpdatedAt.ToString());
            Assert.That(caseListRetractedDoneAt[3].Version, Is.EqualTo(aCase2Retracted.Version));
            Assert.That(caseListRetractedDoneAt[3].WorkerName,
                Is.EqualTo(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName));
            Assert.That(caseListRetractedDoneAt[3].WorkflowState, Is.EqualTo(aCase2Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDoneAt aCase3Retracted

            Assert.That(caseListRetractedDoneAt[0].CaseType, Is.EqualTo(aCase3Retracted.Type));
            Assert.That(caseListRetractedDoneAt[0].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
            Assert.That(caseListRetractedDoneAt[0].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
            Assert.That(caseListRetractedDoneAt[0].Custom, Is.EqualTo(aCase3Retracted.Custom));
            Assert.That(caseListRetractedDoneAt[0].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
            Assert.That(caseListRetractedDoneAt[0].Id, Is.EqualTo(aCase3Retracted.Id));
            Assert.That(caseListRetractedDoneAt[0].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
            Assert.That(caseListRetractedDoneAt[0].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDoneAt[0].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
            Assert.That(caseListRetractedDoneAt[0].Status, Is.EqualTo(aCase3Retracted.Status));
            Assert.That(caseListRetractedDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRetractedDoneAt[0].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDoneAt[0].UpdatedAt.ToString());
            Assert.That(caseListRetractedDoneAt[0].Version, Is.EqualTo(aCase3Retracted.Version));
            Assert.That(caseListRetractedDoneAt[0].WorkerName,
                Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
            Assert.That(caseListRetractedDoneAt[0].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDoneAt aCase4Retracted

            Assert.That(caseListRetractedDoneAt[2].CaseType, Is.EqualTo(aCase4Retracted.Type));
            Assert.That(caseListRetractedDoneAt[2].CaseUId, Is.EqualTo(aCase4Retracted.CaseUid));
            Assert.That(caseListRetractedDoneAt[2].CheckUIid, Is.EqualTo(aCase4Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDoneAt[2].CreatedAt.ToString(), Is.EqualTo(c4Retracted_ca.ToString()));
            Assert.That(caseListRetractedDoneAt[2].Custom, Is.EqualTo(aCase4Retracted.Custom));
            Assert.That(caseListRetractedDoneAt[2].DoneAt.ToString(), Is.EqualTo(c4Retracted_da.ToString()));
            Assert.That(caseListRetractedDoneAt[2].Id, Is.EqualTo(aCase4Retracted.Id));
            Assert.That(caseListRetractedDoneAt[2].MicrotingUId, Is.EqualTo(aCase4Retracted.MicrotingUid));
            Assert.That(caseListRetractedDoneAt[2].SiteId, Is.EqualTo(aCase4Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDoneAt[2].SiteName, Is.EqualTo(aCase4Retracted.Site.Name));
            Assert.That(caseListRetractedDoneAt[2].Status, Is.EqualTo(aCase4Retracted.Status));
            Assert.That(caseListRetractedDoneAt[2].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRetractedDoneAt[2].UnitId, Is.EqualTo(aCase4Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedDoneAt[2].UpdatedAt.ToString());
            Assert.That(caseListRetractedDoneAt[2].Version, Is.EqualTo(aCase4Retracted.Version));
            Assert.That(caseListRetractedDoneAt[2].WorkerName,
                Is.EqualTo(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName));
            Assert.That(caseListRetractedDoneAt[2].WorkflowState, Is.EqualTo(aCase4Retracted.WorkflowState));

            #endregion

            #endregion

            #region caseListRetractedStatus Def Sort Asc

            #region caseListRetractedStatus aCase1Retracted

            Assert.NotNull(caseListRetractedStatus);
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

            Assert.NotNull(caseListRetractedUnitId);
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

            #region Def Sort Des

            #region caseListRetractedDescendingDoneAt Def Sort Des

            #region caseListRetractedDescendingDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedDescendingDoneAt);
            Assert.That(caseListRetractedDescendingDoneAt.Count, Is.EqualTo(4));
            Assert.That(caseListRetractedDescendingDoneAt[2].CaseType, Is.EqualTo(aCase1Retracted.Type));
            Assert.That(caseListRetractedDescendingDoneAt[2].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
            Assert.That(caseListRetractedDescendingDoneAt[2].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDescendingDoneAt[2].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
            Assert.That(caseListRetractedDescendingDoneAt[2].Custom, Is.EqualTo(aCase1Retracted.Custom));
            Assert.That(caseListRetractedDescendingDoneAt[2].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
            Assert.That(caseListRetractedDescendingDoneAt[2].Id, Is.EqualTo(aCase1Retracted.Id));
            Assert.That(caseListRetractedDescendingDoneAt[2].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
            Assert.That(caseListRetractedDescendingDoneAt[2].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDescendingDoneAt[2].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
            Assert.That(caseListRetractedDescendingDoneAt[2].Status, Is.EqualTo(aCase1Retracted.Status));
            Assert.That(caseListRetractedDescendingDoneAt[2].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRetractedDescendingDoneAt[2].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDescendingDoneAt[2].UpdatedAt.ToString());
            Assert.That(caseListRetractedDescendingDoneAt[2].Version, Is.EqualTo(aCase1Retracted.Version));
            Assert.That(caseListRetractedDescendingDoneAt[2].WorkerName,
                Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
            Assert.That(caseListRetractedDescendingDoneAt[2].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDescendingDoneAt aCase2Retracted

            Assert.That(caseListRetractedDescendingDoneAt[0].CaseType, Is.EqualTo(aCase2Retracted.Type));
            Assert.That(caseListRetractedDescendingDoneAt[0].CaseUId, Is.EqualTo(aCase2Retracted.CaseUid));
            Assert.That(caseListRetractedDescendingDoneAt[0].CheckUIid, Is.EqualTo(aCase2Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDescendingDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c2Retracted_ca.ToString()));
            Assert.That(caseListRetractedDescendingDoneAt[0].Custom, Is.EqualTo(aCase2Retracted.Custom));
            Assert.That(caseListRetractedDescendingDoneAt[0].DoneAt.ToString(), Is.EqualTo(c2Retracted_da.ToString()));
            Assert.That(caseListRetractedDescendingDoneAt[0].Id, Is.EqualTo(aCase2Retracted.Id));
            Assert.That(caseListRetractedDescendingDoneAt[0].MicrotingUId, Is.EqualTo(aCase2Retracted.MicrotingUid));
            Assert.That(caseListRetractedDescendingDoneAt[0].SiteId, Is.EqualTo(aCase2Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDescendingDoneAt[0].SiteName, Is.EqualTo(aCase2Retracted.Site.Name));
            Assert.That(caseListRetractedDescendingDoneAt[0].Status, Is.EqualTo(aCase2Retracted.Status));
            Assert.That(caseListRetractedDescendingDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRetractedDescendingDoneAt[0].UnitId, Is.EqualTo(aCase2Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedDescendingDoneAt[0].UpdatedAt.ToString());
            Assert.That(caseListRetractedDescendingDoneAt[0].Version, Is.EqualTo(aCase2Retracted.Version));
            Assert.That(caseListRetractedDescendingDoneAt[0].WorkerName,
                Is.EqualTo(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName));
            Assert.That(caseListRetractedDescendingDoneAt[0].WorkflowState, Is.EqualTo(aCase2Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDescendingDoneAt aCase3Retracted

            Assert.That(caseListRetractedDescendingDoneAt[3].CaseType, Is.EqualTo(aCase3Retracted.Type));
            Assert.That(caseListRetractedDescendingDoneAt[3].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
            Assert.That(caseListRetractedDescendingDoneAt[3].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDescendingDoneAt[3].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
            Assert.That(caseListRetractedDescendingDoneAt[3].Custom, Is.EqualTo(aCase3Retracted.Custom));
            Assert.That(caseListRetractedDescendingDoneAt[3].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
            Assert.That(caseListRetractedDescendingDoneAt[3].Id, Is.EqualTo(aCase3Retracted.Id));
            Assert.That(caseListRetractedDescendingDoneAt[3].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
            Assert.That(caseListRetractedDescendingDoneAt[3].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDescendingDoneAt[3].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
            Assert.That(caseListRetractedDescendingDoneAt[3].Status, Is.EqualTo(aCase3Retracted.Status));
            Assert.That(caseListRetractedDescendingDoneAt[3].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRetractedDescendingDoneAt[3].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDescendingDoneAt[3].UpdatedAt.ToString());
            Assert.That(caseListRetractedDescendingDoneAt[3].Version, Is.EqualTo(aCase3Retracted.Version));
            Assert.That(caseListRetractedDescendingDoneAt[3].WorkerName,
                Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
            Assert.That(caseListRetractedDescendingDoneAt[3].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDescendingDoneAt aCase4Retracted

            Assert.That(caseListRetractedDescendingDoneAt[1].CaseType, Is.EqualTo(aCase4Retracted.Type));
            Assert.That(caseListRetractedDescendingDoneAt[1].CaseUId, Is.EqualTo(aCase4Retracted.CaseUid));
            Assert.That(caseListRetractedDescendingDoneAt[1].CheckUIid, Is.EqualTo(aCase4Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDescendingDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c4Retracted_ca.ToString()));
            Assert.That(caseListRetractedDescendingDoneAt[1].Custom, Is.EqualTo(aCase4Retracted.Custom));
            Assert.That(caseListRetractedDescendingDoneAt[1].DoneAt.ToString(), Is.EqualTo(c4Retracted_da.ToString()));
            Assert.That(caseListRetractedDescendingDoneAt[1].Id, Is.EqualTo(aCase4Retracted.Id));
            Assert.That(caseListRetractedDescendingDoneAt[1].MicrotingUId, Is.EqualTo(aCase4Retracted.MicrotingUid));
            Assert.That(caseListRetractedDescendingDoneAt[1].SiteId, Is.EqualTo(aCase4Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDescendingDoneAt[1].SiteName, Is.EqualTo(aCase4Retracted.Site.Name));
            Assert.That(caseListRetractedDescendingDoneAt[1].Status, Is.EqualTo(aCase4Retracted.Status));
            Assert.That(caseListRetractedDescendingDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRetractedDescendingDoneAt[1].UnitId, Is.EqualTo(aCase4Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedDescendingDoneAt[1].UpdatedAt.ToString());
            Assert.That(caseListRetractedDescendingDoneAt[1].Version, Is.EqualTo(aCase4Retracted.Version));
            Assert.That(caseListRetractedDescendingDoneAt[1].WorkerName,
                Is.EqualTo(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName));
            Assert.That(caseListRetractedDescendingDoneAt[1].WorkflowState, Is.EqualTo(aCase4Retracted.WorkflowState));

            #endregion

            #endregion

            #region caseListRetractedDescendingStatus Def Sort Des

            #region caseListRetractedDescendingStatus aCase1Retracted

            Assert.NotNull(caseListRetractedDescendingStatus);
            Assert.That(caseListRetractedDescendingStatus.Count, Is.EqualTo(4));
            Assert.That(caseListRetractedDescendingStatus[3].CaseType, Is.EqualTo(aCase1Retracted.Type));
            Assert.That(caseListRetractedDescendingStatus[3].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
            Assert.That(caseListRetractedDescendingStatus[3].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDescendingStatus[3].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
            Assert.That(caseListRetractedDescendingStatus[3].Custom, Is.EqualTo(aCase1Retracted.Custom));
            Assert.That(caseListRetractedDescendingStatus[3].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
            Assert.That(caseListRetractedDescendingStatus[3].Id, Is.EqualTo(aCase1Retracted.Id));
            Assert.That(caseListRetractedDescendingStatus[3].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
            Assert.That(caseListRetractedDescendingStatus[3].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDescendingStatus[3].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
            Assert.That(caseListRetractedDescendingStatus[3].Status, Is.EqualTo(aCase1Retracted.Status));
            Assert.That(caseListRetractedDescendingStatus[3].TemplatId, Is.EqualTo(aCase1Retracted.CheckListId));
            Assert.That(caseListRetractedDescendingStatus[3].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDescendingStatus[3].UpdatedAt.ToString());
            Assert.That(caseListRetractedDescendingStatus[3].Version, Is.EqualTo(aCase1Retracted.Version));
            Assert.That(caseListRetractedDescendingStatus[3].WorkerName,
                Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
            Assert.That(caseListRetractedDescendingStatus[3].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDescendingStatus aCase2Retracted

            Assert.That(caseListRetractedDescendingStatus[2].CaseType, Is.EqualTo(aCase2Retracted.Type));
            Assert.That(caseListRetractedDescendingStatus[2].CaseUId, Is.EqualTo(aCase2Retracted.CaseUid));
            Assert.That(caseListRetractedDescendingStatus[2].CheckUIid, Is.EqualTo(aCase2Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDescendingStatus[2].CreatedAt.ToString(), Is.EqualTo(c2Retracted_ca.ToString()));
            Assert.That(caseListRetractedDescendingStatus[2].Custom, Is.EqualTo(aCase2Retracted.Custom));
            Assert.That(caseListRetractedDescendingStatus[2].DoneAt.ToString(), Is.EqualTo(c2Retracted_da.ToString()));
            Assert.That(caseListRetractedDescendingStatus[2].Id, Is.EqualTo(aCase2Retracted.Id));
            Assert.That(caseListRetractedDescendingStatus[2].MicrotingUId, Is.EqualTo(aCase2Retracted.MicrotingUid));
            Assert.That(caseListRetractedDescendingStatus[2].SiteId, Is.EqualTo(aCase2Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDescendingStatus[2].SiteName, Is.EqualTo(aCase2Retracted.Site.Name));
            Assert.That(caseListRetractedDescendingStatus[2].Status, Is.EqualTo(aCase2Retracted.Status));
            Assert.That(caseListRetractedDescendingStatus[2].TemplatId, Is.EqualTo(aCase2Retracted.CheckListId));
            Assert.That(caseListRetractedDescendingStatus[2].UnitId, Is.EqualTo(aCase2Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedDescendingStatus[2].UpdatedAt.ToString());
            Assert.That(caseListRetractedDescendingStatus[2].Version, Is.EqualTo(aCase2Retracted.Version));
            Assert.That(caseListRetractedDescendingStatus[2].WorkerName,
                Is.EqualTo(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName));
            Assert.That(caseListRetractedDescendingStatus[2].WorkflowState, Is.EqualTo(aCase2Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDescendingStatus aCase3Retracted

            Assert.That(caseListRetractedDescendingStatus[1].CaseType, Is.EqualTo(aCase3Retracted.Type));
            Assert.That(caseListRetractedDescendingStatus[1].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
            Assert.That(caseListRetractedDescendingStatus[1].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDescendingStatus[1].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
            Assert.That(caseListRetractedDescendingStatus[1].Custom, Is.EqualTo(aCase3Retracted.Custom));
            Assert.That(caseListRetractedDescendingStatus[1].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
            Assert.That(caseListRetractedDescendingStatus[1].Id, Is.EqualTo(aCase3Retracted.Id));
            Assert.That(caseListRetractedDescendingStatus[1].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
            Assert.That(caseListRetractedDescendingStatus[1].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDescendingStatus[1].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
            Assert.That(caseListRetractedDescendingStatus[1].Status, Is.EqualTo(aCase3Retracted.Status));
            Assert.That(caseListRetractedDescendingStatus[1].TemplatId, Is.EqualTo(aCase3Retracted.CheckListId));
            Assert.That(caseListRetractedDescendingStatus[1].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDescendingStatus[1].UpdatedAt.ToString());
            Assert.That(caseListRetractedDescendingStatus[1].Version, Is.EqualTo(aCase3Retracted.Version));
            Assert.That(caseListRetractedDescendingStatus[1].WorkerName,
                Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
            Assert.That(caseListRetractedDescendingStatus[1].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDescendingStatus aCase4Retracted

            Assert.That(caseListRetractedDescendingStatus[0].CaseType, Is.EqualTo(aCase4Retracted.Type));
            Assert.That(caseListRetractedDescendingStatus[0].CaseUId, Is.EqualTo(aCase4Retracted.CaseUid));
            Assert.That(caseListRetractedDescendingStatus[0].CheckUIid, Is.EqualTo(aCase4Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDescendingStatus[0].CreatedAt.ToString(), Is.EqualTo(c4Retracted_ca.ToString()));
            Assert.That(caseListRetractedDescendingStatus[0].Custom, Is.EqualTo(aCase4Retracted.Custom));
            Assert.That(caseListRetractedDescendingStatus[0].DoneAt.ToString(), Is.EqualTo(c4Retracted_da.ToString()));
            Assert.That(caseListRetractedDescendingStatus[0].Id, Is.EqualTo(aCase4Retracted.Id));
            Assert.That(caseListRetractedDescendingStatus[0].MicrotingUId, Is.EqualTo(aCase4Retracted.MicrotingUid));
            Assert.That(caseListRetractedDescendingStatus[0].SiteId, Is.EqualTo(aCase4Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDescendingStatus[0].SiteName, Is.EqualTo(aCase4Retracted.Site.Name));
            Assert.That(caseListRetractedDescendingStatus[0].Status, Is.EqualTo(aCase4Retracted.Status));
            Assert.That(caseListRetractedDescendingStatus[0].TemplatId, Is.EqualTo(aCase4Retracted.CheckListId));
            Assert.That(caseListRetractedDescendingStatus[0].UnitId, Is.EqualTo(aCase4Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedDescendingStatus[0].UpdatedAt.ToString());
            Assert.That(caseListRetractedDescendingStatus[0].Version, Is.EqualTo(aCase4Retracted.Version));
            Assert.That(caseListRetractedDescendingStatus[0].WorkerName,
                Is.EqualTo(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName));
            Assert.That(caseListRetractedDescendingStatus[0].WorkflowState, Is.EqualTo(aCase4Retracted.WorkflowState));

            #endregion

            #endregion

            #region caseListRetractedDescendingUnitId Def Sort Des

            #region caseListRetractedDescendingUnitId aCase1Retracted

            Assert.NotNull(caseListRetractedDescendingUnitId);
            Assert.That(caseListRetractedDescendingUnitId.Count, Is.EqualTo(4));
            Assert.That(caseListRetractedDescendingUnitId[3].CaseType, Is.EqualTo(aCase1Retracted.Type));
            Assert.That(caseListRetractedDescendingUnitId[3].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
            Assert.That(caseListRetractedDescendingUnitId[3].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDescendingUnitId[3].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
            Assert.That(caseListRetractedDescendingUnitId[3].Custom, Is.EqualTo(aCase1Retracted.Custom));
            Assert.That(caseListRetractedDescendingUnitId[3].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
            Assert.That(caseListRetractedDescendingUnitId[3].Id, Is.EqualTo(aCase1Retracted.Id));
            Assert.That(caseListRetractedDescendingUnitId[3].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
            Assert.That(caseListRetractedDescendingUnitId[3].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDescendingUnitId[3].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
            Assert.That(caseListRetractedDescendingUnitId[3].Status, Is.EqualTo(aCase1Retracted.Status));
            Assert.That(caseListRetractedDescendingUnitId[3].TemplatId, Is.EqualTo(aCase1Retracted.CheckListId));
            Assert.That(caseListRetractedDescendingUnitId[3].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDescendingUnitId[3].UpdatedAt.ToString());
            Assert.That(caseListRetractedDescendingUnitId[3].Version, Is.EqualTo(aCase1Retracted.Version));
            Assert.That(caseListRetractedDescendingUnitId[3].WorkerName,
                Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
            Assert.That(caseListRetractedDescendingUnitId[3].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDescendingUnitId aCase2Retracted

            Assert.That(caseListRetractedDescendingUnitId[2].CaseType, Is.EqualTo(aCase2Retracted.Type));
            Assert.That(caseListRetractedDescendingUnitId[2].CaseUId, Is.EqualTo(aCase2Retracted.CaseUid));
            Assert.That(caseListRetractedDescendingUnitId[2].CheckUIid, Is.EqualTo(aCase2Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDescendingUnitId[2].CreatedAt.ToString(), Is.EqualTo(c2Retracted_ca.ToString()));
            Assert.That(caseListRetractedDescendingUnitId[2].Custom, Is.EqualTo(aCase2Retracted.Custom));
            Assert.That(caseListRetractedDescendingUnitId[2].DoneAt.ToString(), Is.EqualTo(c2Retracted_da.ToString()));
            Assert.That(caseListRetractedDescendingUnitId[2].Id, Is.EqualTo(aCase2Retracted.Id));
            Assert.That(caseListRetractedDescendingUnitId[2].MicrotingUId, Is.EqualTo(aCase2Retracted.MicrotingUid));
            Assert.That(caseListRetractedDescendingUnitId[2].SiteId, Is.EqualTo(aCase2Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDescendingUnitId[2].SiteName, Is.EqualTo(aCase2Retracted.Site.Name));
            Assert.That(caseListRetractedDescendingUnitId[2].Status, Is.EqualTo(aCase2Retracted.Status));
            Assert.That(caseListRetractedDescendingUnitId[2].TemplatId, Is.EqualTo(aCase2Retracted.CheckListId));
            Assert.That(caseListRetractedDescendingUnitId[2].UnitId, Is.EqualTo(aCase2Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedDescendingUnitId[2].UpdatedAt.ToString());
            Assert.That(caseListRetractedDescendingUnitId[2].Version, Is.EqualTo(aCase2Retracted.Version));
            Assert.That(caseListRetractedDescendingUnitId[2].WorkerName,
                Is.EqualTo(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName));
            Assert.That(caseListRetractedDescendingUnitId[2].WorkflowState, Is.EqualTo(aCase2Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDescendingUnitId aCase3Retracted

            Assert.That(caseListRetractedDescendingUnitId[1].CaseType, Is.EqualTo(aCase3Retracted.Type));
            Assert.That(caseListRetractedDescendingUnitId[1].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
            Assert.That(caseListRetractedDescendingUnitId[1].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDescendingUnitId[1].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
            Assert.That(caseListRetractedDescendingUnitId[1].Custom, Is.EqualTo(aCase3Retracted.Custom));
            Assert.That(caseListRetractedDescendingUnitId[1].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
            Assert.That(caseListRetractedDescendingUnitId[1].Id, Is.EqualTo(aCase3Retracted.Id));
            Assert.That(caseListRetractedDescendingUnitId[1].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
            Assert.That(caseListRetractedDescendingUnitId[1].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDescendingUnitId[1].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
            Assert.That(caseListRetractedDescendingUnitId[1].Status, Is.EqualTo(aCase3Retracted.Status));
            Assert.That(caseListRetractedDescendingUnitId[1].TemplatId, Is.EqualTo(aCase3Retracted.CheckListId));
            Assert.That(caseListRetractedDescendingUnitId[1].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDescendingUnitId[1].UpdatedAt.ToString());
            Assert.That(caseListRetractedDescendingUnitId[1].Version, Is.EqualTo(aCase3Retracted.Version));
            Assert.That(caseListRetractedDescendingUnitId[1].WorkerName,
                Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
            Assert.That(caseListRetractedDescendingUnitId[1].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDescendingUnitId aCase4Retracted

            Assert.That(caseListRetractedDescendingUnitId[0].CaseType, Is.EqualTo(aCase4Retracted.Type));
            Assert.That(caseListRetractedDescendingUnitId[0].CaseUId, Is.EqualTo(aCase4Retracted.CaseUid));
            Assert.That(caseListRetractedDescendingUnitId[0].CheckUIid, Is.EqualTo(aCase4Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDescendingUnitId[0].CreatedAt.ToString(), Is.EqualTo(c4Retracted_ca.ToString()));
            Assert.That(caseListRetractedDescendingUnitId[0].Custom, Is.EqualTo(aCase4Retracted.Custom));
            Assert.That(caseListRetractedDescendingUnitId[0].DoneAt.ToString(), Is.EqualTo(c4Retracted_da.ToString()));
            Assert.That(caseListRetractedDescendingUnitId[0].Id, Is.EqualTo(aCase4Retracted.Id));
            Assert.That(caseListRetractedDescendingUnitId[0].MicrotingUId, Is.EqualTo(aCase4Retracted.MicrotingUid));
            Assert.That(caseListRetractedDescendingUnitId[0].SiteId, Is.EqualTo(aCase4Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDescendingUnitId[0].SiteName, Is.EqualTo(aCase4Retracted.Site.Name));
            Assert.That(caseListRetractedDescendingUnitId[0].Status, Is.EqualTo(aCase4Retracted.Status));
            Assert.That(caseListRetractedDescendingUnitId[0].TemplatId, Is.EqualTo(aCase4Retracted.CheckListId));
            Assert.That(caseListRetractedDescendingUnitId[0].UnitId, Is.EqualTo(aCase4Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedDescendingUnitId[0].UpdatedAt.ToString());
            Assert.That(caseListRetractedDescendingUnitId[0].Version, Is.EqualTo(aCase4Retracted.Version));
            Assert.That(caseListRetractedDescendingUnitId[0].WorkerName,
                Is.EqualTo(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName));
            Assert.That(caseListRetractedDescendingUnitId[0].WorkflowState, Is.EqualTo(aCase4Retracted.WorkflowState));

            #endregion

            #endregion

            #endregion

            #region Def Sort Asc w. DateTime

            #region caseListRetractedDtDoneAt Def Sort Asc w. DateTime

            #region caseListRetractedDtDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedDtDoneAt);
            Assert.That(caseListRetractedDtDoneAt.Count, Is.EqualTo(2));
            Assert.That(caseListRetractedDtDoneAt[1].CaseType, Is.EqualTo(aCase1Retracted.Type));
            Assert.That(caseListRetractedDtDoneAt[1].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
            Assert.That(caseListRetractedDtDoneAt[1].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDtDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
            Assert.That(caseListRetractedDtDoneAt[1].Custom, Is.EqualTo(aCase1Retracted.Custom));
            Assert.That(caseListRetractedDtDoneAt[1].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
            Assert.That(caseListRetractedDtDoneAt[1].Id, Is.EqualTo(aCase1Retracted.Id));
            Assert.That(caseListRetractedDtDoneAt[1].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
            Assert.That(caseListRetractedDtDoneAt[1].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDtDoneAt[1].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
            Assert.That(caseListRetractedDtDoneAt[1].Status, Is.EqualTo(aCase1Retracted.Status));
            Assert.That(caseListRetractedDtDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRetractedDtDoneAt[1].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtDoneAt[1].UpdatedAt.ToString());
            Assert.That(caseListRetractedDtDoneAt[1].Version, Is.EqualTo(aCase1Retracted.Version));
            Assert.That(caseListRetractedDtDoneAt[1].WorkerName,
                Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
            Assert.That(caseListRetractedDtDoneAt[1].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDtDoneAt aCase3Retracted

            Assert.That(caseListRetractedDtDoneAt[0].CaseType, Is.EqualTo(aCase3Retracted.Type));
            Assert.That(caseListRetractedDtDoneAt[0].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
            Assert.That(caseListRetractedDtDoneAt[0].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDtDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
            Assert.That(caseListRetractedDtDoneAt[0].Custom, Is.EqualTo(aCase3Retracted.Custom));
            Assert.That(caseListRetractedDtDoneAt[0].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
            Assert.That(caseListRetractedDtDoneAt[0].Id, Is.EqualTo(aCase3Retracted.Id));
            Assert.That(caseListRetractedDtDoneAt[0].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
            Assert.That(caseListRetractedDtDoneAt[0].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDtDoneAt[0].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
            Assert.That(caseListRetractedDtDoneAt[0].Status, Is.EqualTo(aCase3Retracted.Status));
            Assert.That(caseListRetractedDtDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRetractedDtDoneAt[0].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtDoneAt[0].UpdatedAt.ToString());
            Assert.That(caseListRetractedDtDoneAt[0].Version, Is.EqualTo(aCase3Retracted.Version));
            Assert.That(caseListRetractedDtDoneAt[0].WorkerName,
                Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
            Assert.That(caseListRetractedDtDoneAt[0].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

            #endregion

            #endregion

            #region caseListRetractedDtStatus Def Sort Asc w. DateTime

            #region caseListRetractedDtStatus aCase1Retracted

            Assert.NotNull(caseListRetractedDtStatus);
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

            Assert.NotNull(caseListRetractedDtUnitId);
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

            #region Def Sort Des w. DateTime

            #region caseListRetractedDtDescendingDoneAt Def Sort Des

            #region caseListRetractedDtDescendingDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedDtDescendingDoneAt);
            Assert.That(caseListRetractedDtDescendingDoneAt.Count, Is.EqualTo(2));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].CaseType, Is.EqualTo(aCase1Retracted.Type));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].Custom, Is.EqualTo(aCase1Retracted.Custom));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].Id, Is.EqualTo(aCase1Retracted.Id));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].Status, Is.EqualTo(aCase1Retracted.Status));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtDescendingDoneAt[0].UpdatedAt.ToString());
            Assert.That(caseListRetractedDtDescendingDoneAt[0].Version, Is.EqualTo(aCase1Retracted.Version));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].WorkerName,
                Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
            Assert.That(caseListRetractedDtDescendingDoneAt[0].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDtDescendingDoneAt aCase3Retracted

            Assert.That(caseListRetractedDtDescendingDoneAt[1].CaseType, Is.EqualTo(aCase3Retracted.Type));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].Custom, Is.EqualTo(aCase3Retracted.Custom));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].Id, Is.EqualTo(aCase3Retracted.Id));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].Status, Is.EqualTo(aCase3Retracted.Status));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].TemplatId, Is.EqualTo(cl1.Id));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtDescendingDoneAt[1].UpdatedAt.ToString());
            Assert.That(caseListRetractedDtDescendingDoneAt[1].Version, Is.EqualTo(aCase3Retracted.Version));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].WorkerName,
                Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
            Assert.That(caseListRetractedDtDescendingDoneAt[1].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

            #endregion

            #endregion

            #region caseListRetractedDtDescendingStatus Def Sort Des

            #region caseListRetractedDtDescendingStatus aCase1Retracted

            Assert.NotNull(caseListRetractedDtDescendingStatus);
            Assert.That(caseListRetractedDtDescendingStatus.Count, Is.EqualTo(2));
            Assert.That(caseListRetractedDtDescendingStatus[0].CaseType, Is.EqualTo(aCase3Retracted.Type));
            Assert.That(caseListRetractedDtDescendingStatus[0].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
            Assert.That(caseListRetractedDtDescendingStatus[0].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDtDescendingStatus[0].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
            Assert.That(caseListRetractedDtDescendingStatus[0].Custom, Is.EqualTo(aCase3Retracted.Custom));
            Assert.That(caseListRetractedDtDescendingStatus[0].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
            Assert.That(caseListRetractedDtDescendingStatus[0].Id, Is.EqualTo(aCase3Retracted.Id));
            Assert.That(caseListRetractedDtDescendingStatus[0].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
            Assert.That(caseListRetractedDtDescendingStatus[0].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDtDescendingStatus[0].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
            Assert.That(caseListRetractedDtDescendingStatus[0].Status, Is.EqualTo(aCase3Retracted.Status));
            Assert.That(caseListRetractedDtDescendingStatus[0].TemplatId, Is.EqualTo(aCase3Retracted.CheckListId));
            Assert.That(caseListRetractedDtDescendingStatus[0].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtDescendingStatus[0].UpdatedAt.ToString());
            Assert.That(caseListRetractedDtDescendingStatus[0].Version, Is.EqualTo(aCase3Retracted.Version));
            Assert.That(caseListRetractedDtDescendingStatus[0].WorkerName,
                Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
            Assert.That(caseListRetractedDtDescendingStatus[0].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDtDescendingStatus aCase3Retracted

            Assert.That(caseListRetractedDtDescendingStatus[1].CaseType, Is.EqualTo(aCase1Retracted.Type));
            Assert.That(caseListRetractedDtDescendingStatus[1].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
            Assert.That(caseListRetractedDtDescendingStatus[1].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDtDescendingStatus[1].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
            Assert.That(caseListRetractedDtDescendingStatus[1].Custom, Is.EqualTo(aCase1Retracted.Custom));
            Assert.That(caseListRetractedDtDescendingStatus[1].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
            Assert.That(caseListRetractedDtDescendingStatus[1].Id, Is.EqualTo(aCase1Retracted.Id));
            Assert.That(caseListRetractedDtDescendingStatus[1].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
            Assert.That(caseListRetractedDtDescendingStatus[1].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDtDescendingStatus[1].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
            Assert.That(caseListRetractedDtDescendingStatus[1].Status, Is.EqualTo(aCase1Retracted.Status));
            Assert.That(caseListRetractedDtDescendingStatus[1].TemplatId, Is.EqualTo(aCase1Retracted.CheckListId));
            Assert.That(caseListRetractedDtDescendingStatus[1].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtDescendingStatus[1].UpdatedAt.ToString());
            Assert.That(caseListRetractedDtDescendingStatus[1].Version, Is.EqualTo(aCase1Retracted.Version));
            Assert.That(caseListRetractedDtDescendingStatus[1].WorkerName,
                Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
            Assert.That(caseListRetractedDtDescendingStatus[1].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

            #endregion

            #endregion

            #region caseListRetractedDtDescendingUnitId Def Sort Des

            #region caseListRetractedDtDescendingUnitId aCase1Retracted

            Assert.NotNull(caseListRetractedDtDescendingUnitId);
            Assert.That(caseListRetractedDtDescendingUnitId.Count, Is.EqualTo(2));

            Assert.That(caseListRetractedDtDescendingUnitId[0].CaseType, Is.EqualTo(aCase3Retracted.Type));
            Assert.That(caseListRetractedDtDescendingUnitId[0].CaseUId, Is.EqualTo(aCase3Retracted.CaseUid));
            Assert.That(caseListRetractedDtDescendingUnitId[0].CheckUIid, Is.EqualTo(aCase3Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDtDescendingUnitId[0].CreatedAt.ToString(), Is.EqualTo(c3Retracted_ca.ToString()));
            Assert.That(caseListRetractedDtDescendingUnitId[0].Custom, Is.EqualTo(aCase3Retracted.Custom));
            Assert.That(caseListRetractedDtDescendingUnitId[0].DoneAt.ToString(), Is.EqualTo(c3Retracted_da.ToString()));
            Assert.That(caseListRetractedDtDescendingUnitId[0].Id, Is.EqualTo(aCase3Retracted.Id));
            Assert.That(caseListRetractedDtDescendingUnitId[0].MicrotingUId, Is.EqualTo(aCase3Retracted.MicrotingUid));
            Assert.That(caseListRetractedDtDescendingUnitId[0].SiteId, Is.EqualTo(aCase3Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDtDescendingUnitId[0].SiteName, Is.EqualTo(aCase3Retracted.Site.Name));
            Assert.That(caseListRetractedDtDescendingUnitId[0].Status, Is.EqualTo(aCase3Retracted.Status));
            Assert.That(caseListRetractedDtDescendingUnitId[0].TemplatId, Is.EqualTo(aCase3Retracted.CheckListId));
            Assert.That(caseListRetractedDtDescendingUnitId[0].UnitId, Is.EqualTo(aCase3Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtDescendingUnitId[0].UpdatedAt.ToString());
            Assert.That(caseListRetractedDtDescendingUnitId[0].Version, Is.EqualTo(aCase3Retracted.Version));
            Assert.That(caseListRetractedDtDescendingUnitId[0].WorkerName,
                Is.EqualTo(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName));
            Assert.That(caseListRetractedDtDescendingUnitId[0].WorkflowState, Is.EqualTo(aCase3Retracted.WorkflowState));

            #endregion

            #region caseListRetractedDtDescendingUnitId aCase3Retracted

            Assert.That(caseListRetractedDtDescendingUnitId[1].CaseType, Is.EqualTo(aCase1Retracted.Type));
            Assert.That(caseListRetractedDtDescendingUnitId[1].CaseUId, Is.EqualTo(aCase1Retracted.CaseUid));
            Assert.That(caseListRetractedDtDescendingUnitId[1].CheckUIid, Is.EqualTo(aCase1Retracted.MicrotingCheckUid));
            Assert.That(caseListRetractedDtDescendingUnitId[1].CreatedAt.ToString(), Is.EqualTo(c1Retracted_ca.ToString()));
            Assert.That(caseListRetractedDtDescendingUnitId[1].Custom, Is.EqualTo(aCase1Retracted.Custom));
            Assert.That(caseListRetractedDtDescendingUnitId[1].DoneAt.ToString(), Is.EqualTo(c1Retracted_da.ToString()));
            Assert.That(caseListRetractedDtDescendingUnitId[1].Id, Is.EqualTo(aCase1Retracted.Id));
            Assert.That(caseListRetractedDtDescendingUnitId[1].MicrotingUId, Is.EqualTo(aCase1Retracted.MicrotingUid));
            Assert.That(caseListRetractedDtDescendingUnitId[1].SiteId, Is.EqualTo(aCase1Retracted.Site.MicrotingUid));
            Assert.That(caseListRetractedDtDescendingUnitId[1].SiteName, Is.EqualTo(aCase1Retracted.Site.Name));
            Assert.That(caseListRetractedDtDescendingUnitId[1].Status, Is.EqualTo(aCase1Retracted.Status));
            Assert.That(caseListRetractedDtDescendingUnitId[1].TemplatId, Is.EqualTo(aCase1Retracted.CheckListId));
            Assert.That(caseListRetractedDtDescendingUnitId[1].UnitId, Is.EqualTo(aCase1Retracted.Unit.MicrotingUid));
            //            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtDescendingUnitId[1].UpdatedAt.ToString());
            Assert.That(caseListRetractedDtDescendingUnitId[1].Version, Is.EqualTo(aCase1Retracted.Version));
            Assert.That(caseListRetractedDtDescendingUnitId[1].WorkerName,
                Is.EqualTo(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName));
            Assert.That(caseListRetractedDtDescendingUnitId[1].WorkflowState, Is.EqualTo(aCase1Retracted.WorkflowState));

            #endregion

            #endregion

            #endregion

            #endregion

            #region Case Sort

            #region aCase Sort Asc

            #region aCase1Retracted sort asc

            #region caseListC1DoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDoneAt);
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

            Assert.NotNull(caseListRetractedC1SortStatus);
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

            Assert.NotNull(caseListRetractedC1SortUnitId);
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

            Assert.NotNull(caseListRetractedC2SortDoneAt);
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

            Assert.NotNull(caseListRetractedC2SortStatus);
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

            Assert.NotNull(caseListRetractedC2SortUnitId);
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

            Assert.NotNull(caseListRetractedC3SortDoneAt);
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

            Assert.NotNull(caseListRetractedC3SortStatus);
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

            Assert.NotNull(caseListRetractedC3SortUnitId);
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

            Assert.NotNull(caseListRetractedC4SortDoneAt);
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

            Assert.NotNull(caseListRetractedC4SortStatus);
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

            Assert.NotNull(caseListRetractedC4SortUnitId);
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

            #region aCase Sort Des

            #region aCase1Retracted Sort Des

            #region caseListRetractedC1SortDescendingDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDescendingDoneAt);
            Assert.That(caseListRetractedC1SortDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1Retracted.type, caseListRetractedC1SortDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase1Retracted.case_uid, caseListRetractedC1SortDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase1Retracted.microting_check_uid, caseListRetractedC1SortDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedC1SortDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.custom, caseListRetractedC1SortDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedC1SortDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Retracted.Id, caseListRetractedC1SortDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase1Retracted.microting_uid, caseListRetractedC1SortDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1Retracted.site.microting_uid, caseListRetractedC1SortDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1Retracted.site.name, caseListRetractedC1SortDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1Retracted.status, caseListRetractedC1SortDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase1Retracted.check_list_id, caseListRetractedC1SortDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1Retracted.unit.microting_uid, caseListRetractedC1SortDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedC1SortDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.version, caseListRetractedC1SortDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase1Retracted.worker.first_name + " " + aCase1Retracted.worker.last_name, caseListRetractedC1SortDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase1Retracted.workflow_state, caseListRetractedC1SortDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC1SortDescendingStatus aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDescendingStatus);
            Assert.That(caseListRetractedC1SortDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1Retracted.type, caseListRetractedC1SortDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase1Retracted.case_uid, caseListRetractedC1SortDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase1Retracted.microting_check_uid, caseListRetractedC1SortDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedC1SortDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.custom, caseListRetractedC1SortDescendingStatus[0].Custom);
            // Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedC1SortDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Retracted.Id, caseListRetractedC1SortDescendingStatus[0].Id);
            // Assert.AreEqual(aCase1Retracted.microting_uid, caseListRetractedC1SortDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1Retracted.site.microting_uid, caseListRetractedC1SortDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase1Retracted.site.name, caseListRetractedC1SortDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase1Retracted.status, caseListRetractedC1SortDescendingStatus[0].Status);
            // Assert.AreEqual(aCase1Retracted.check_list_id, caseListRetractedC1SortDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase1Retracted.unit.microting_uid, caseListRetractedC1SortDescendingStatus[0].UnitId);
            // Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedC1SortDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.version, caseListRetractedC1SortDescendingStatus[0].Version);
            // Assert.AreEqual(aCase1Retracted.worker.first_name + " " + aCase1Retracted.worker.last_name, caseListRetractedC1SortDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase1Retracted.workflow_state, caseListRetractedC1SortDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC1SortDescendingUnitId aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDescendingUnitId);
            Assert.That(caseListRetractedC1SortDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1Retracted.type, caseListRetractedC1SortDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase1Retracted.case_uid, caseListRetractedC1SortDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase1Retracted.microting_check_uid, caseListRetractedC1SortDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedC1SortDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.custom, caseListRetractedC1SortDescendingUnitId[0].Custom);
            // Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedC1SortDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Retracted.Id, caseListRetractedC1SortDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase1Retracted.microting_uid, caseListRetractedC1SortDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1Retracted.site.microting_uid, caseListRetractedC1SortDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase1Retracted.site.name, caseListRetractedC1SortDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase1Retracted.status, caseListRetractedC1SortDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase1Retracted.check_list_id, caseListRetractedC1SortDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1Retracted.unit.microting_uid, caseListRetractedC1SortDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedC1SortDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.version, caseListRetractedC1SortDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase1Retracted.worker.first_name + " " + aCase1Retracted.worker.last_name, caseListRetractedC1SortDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase1Retracted.workflow_state, caseListRetractedC1SortDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase2Retracted Sort Des

            #region caseListRetractedC2SortDescendingDoneAt aCase2Retracted

            Assert.NotNull(caseListRetractedC2SortDescendingDoneAt);
            Assert.That(caseListRetractedC2SortDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2Retracted.type, caseListRetractedC2SortDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase2Retracted.case_uid, caseListRetractedC2SortDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase2Retracted.microting_check_uid, caseListRetractedC2SortDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedC2SortDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.custom, caseListRetractedC2SortDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedC2SortDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Retracted.Id, caseListRetractedC2SortDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase2Retracted.microting_uid, caseListRetractedC2SortDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2Retracted.site.microting_uid, caseListRetractedC2SortDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2Retracted.site.name, caseListRetractedC2SortDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2Retracted.status, caseListRetractedC2SortDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase2Retracted.check_list_id, caseListRetractedC2SortDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2Retracted.unit.microting_uid, caseListRetractedC2SortDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedC2SortDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.version, caseListRetractedC2SortDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase2Retracted.worker.first_name + " " + aCase2Retracted.worker.last_name, caseListRetractedC2SortDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase2Retracted.workflow_state, caseListRetractedC2SortDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC2SortDescendingStatus aCase2Retracted

            Assert.NotNull(caseListRetractedC2SortDescendingStatus);
            Assert.That(caseListRetractedC2SortDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2Retracted.type, caseListRetractedC2SortDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase2Retracted.case_uid, caseListRetractedC2SortDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase2Retracted.microting_check_uid, caseListRetractedC2SortDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedC2SortDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.custom, caseListRetractedC2SortDescendingStatus[0].Custom);
            // Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedC2SortDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Retracted.Id, caseListRetractedC2SortDescendingStatus[0].Id);
            // Assert.AreEqual(aCase2Retracted.microting_uid, caseListRetractedC2SortDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2Retracted.site.microting_uid, caseListRetractedC2SortDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase2Retracted.site.name, caseListRetractedC2SortDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase2Retracted.status, caseListRetractedC2SortDescendingStatus[0].Status);
            // Assert.AreEqual(aCase2Retracted.check_list_id, caseListRetractedC2SortDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase2Retracted.unit.microting_uid, caseListRetractedC2SortDescendingStatus[0].UnitId);
            // Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedC2SortDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.version, caseListRetractedC2SortDescendingStatus[0].Version);
            // Assert.AreEqual(aCase2Retracted.worker.first_name + " " + aCase2Retracted.worker.last_name, caseListRetractedC2SortDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase2Retracted.workflow_state, caseListRetractedC2SortDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC2SortDescendingUnitId aCase2Retracted

            Assert.NotNull(caseListRetractedC2SortDescendingUnitId);
            Assert.That(caseListRetractedC2SortDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2Retracted.type, caseListRetractedC2SortDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase2Retracted.case_uid, caseListRetractedC2SortDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase2Retracted.microting_check_uid, caseListRetractedC2SortDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedC2SortDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.custom, caseListRetractedC2SortDescendingUnitId[0].Custom);
            // Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedC2SortDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Retracted.Id, caseListRetractedC2SortDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase2Retracted.microting_uid, caseListRetractedC2SortDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2Retracted.site.microting_uid, caseListRetractedC2SortDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase2Retracted.site.name, caseListRetractedC2SortDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase2Retracted.status, caseListRetractedC2SortDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase2Retracted.check_list_id, caseListRetractedC2SortDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2Retracted.unit.microting_uid, caseListRetractedC2SortDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedC2SortDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.version, caseListRetractedC2SortDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase2Retracted.worker.first_name + " " + aCase2Retracted.worker.last_name, caseListRetractedC2SortDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase2Retracted.workflow_state, caseListRetractedC2SortDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase3Retracted Sort Des

            #region caseListRetractedC3SortDescendingDoneAt aCase3Retracted

            Assert.NotNull(caseListRetractedC3SortDescendingDoneAt);
            Assert.That(caseListRetractedC3SortDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3Retracted.type, caseListRetractedC3SortDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase3Retracted.case_uid, caseListRetractedC3SortDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase3Retracted.microting_check_uid, caseListRetractedC3SortDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedC3SortDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.custom, caseListRetractedC3SortDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedC3SortDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Retracted.Id, caseListRetractedC3SortDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase3Retracted.microting_uid, caseListRetractedC3SortDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3Retracted.site.microting_uid, caseListRetractedC3SortDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3Retracted.site.name, caseListRetractedC3SortDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3Retracted.status, caseListRetractedC3SortDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase3Retracted.check_list_id, caseListRetractedC3SortDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3Retracted.unit.microting_uid, caseListRetractedC3SortDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedC3SortDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.version, caseListRetractedC3SortDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase3Retracted.worker.first_name + " " + aCase3Retracted.worker.last_name, caseListRetractedC3SortDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase3Retracted.workflow_state, caseListRetractedC3SortDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC3SortDescendingStatus aCase3Retracted

            Assert.NotNull(caseListRetractedC3SortDescendingStatus);
            Assert.That(caseListRetractedC3SortDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3Retracted.type, caseListRetractedC3SortDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase3Retracted.case_uid, caseListRetractedC3SortDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase3Retracted.microting_check_uid, caseListRetractedC3SortDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedC3SortDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.custom, caseListRetractedC3SortDescendingStatus[0].Custom);
            // Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedC3SortDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Retracted.Id, caseListRetractedC3SortDescendingStatus[0].Id);
            // Assert.AreEqual(aCase3Retracted.microting_uid, caseListRetractedC3SortDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3Retracted.site.microting_uid, caseListRetractedC3SortDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase3Retracted.site.name, caseListRetractedC3SortDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase3Retracted.status, caseListRetractedC3SortDescendingStatus[0].Status);
            // Assert.AreEqual(aCase3Retracted.check_list_id, caseListRetractedC3SortDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase3Retracted.unit.microting_uid, caseListRetractedC3SortDescendingStatus[0].UnitId);
            // Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedC3SortDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.version, caseListRetractedC3SortDescendingStatus[0].Version);
            // Assert.AreEqual(aCase3Retracted.worker.first_name + " " + aCase3Retracted.worker.last_name, caseListRetractedC3SortDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase3Retracted.workflow_state, caseListRetractedC3SortDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC3SortDescendingUnitId aCase3Retracted

            Assert.NotNull(caseListRetractedC3SortDescendingUnitId);
            Assert.That(caseListRetractedC3SortDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3Retracted.type, caseListRetractedC3SortDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase3Retracted.case_uid, caseListRetractedC3SortDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase3Retracted.microting_check_uid, caseListRetractedC3SortDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedC3SortDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.custom, caseListRetractedC3SortDescendingUnitId[0].Custom);
            // Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedC3SortDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Retracted.Id, caseListRetractedC3SortDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase3Retracted.microting_uid, caseListRetractedC3SortDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3Retracted.site.microting_uid, caseListRetractedC3SortDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase3Retracted.site.name, caseListRetractedC3SortDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase3Retracted.status, caseListRetractedC3SortDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase3Retracted.check_list_id, caseListRetractedC3SortDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3Retracted.unit.microting_uid, caseListRetractedC3SortDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedC3SortDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.version, caseListRetractedC3SortDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase3Retracted.worker.first_name + " " + aCase3Retracted.worker.last_name, caseListRetractedC3SortDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase3Retracted.workflow_state, caseListRetractedC3SortDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase4Retracted Sort Des

            #region caseListRetractedC4SortDescendingDoneAt aCase4Retracted

            Assert.NotNull(caseListRetractedC4SortDescendingDoneAt);
            Assert.That(caseListRetractedC4SortDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4Retracted.type, caseListRetractedC4SortDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4Retracted.case_uid, caseListRetractedC4SortDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4Retracted.microting_check_uid, caseListRetractedC4SortDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedC4SortDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.custom, caseListRetractedC4SortDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedC4SortDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Retracted.Id, caseListRetractedC4SortDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase4Retracted.microting_uid, caseListRetractedC4SortDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4Retracted.site.microting_uid, caseListRetractedC4SortDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4Retracted.site.name, caseListRetractedC4SortDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4Retracted.status, caseListRetractedC4SortDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase4Retracted.check_list_id, caseListRetractedC4SortDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4Retracted.unit.microting_uid, caseListRetractedC4SortDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedC4SortDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.version, caseListRetractedC4SortDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase4Retracted.worker.first_name + " " + aCase4Retracted.worker.last_name, caseListRetractedC4SortDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4Retracted.workflow_state, caseListRetractedC4SortDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC4SortDescendingStatus aCase4Retracted

            Assert.NotNull(caseListRetractedC4SortDescendingStatus);
            Assert.That(caseListRetractedC4SortDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4Retracted.type, caseListRetractedC4SortDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase4Retracted.case_uid, caseListRetractedC4SortDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase4Retracted.microting_check_uid, caseListRetractedC4SortDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedC4SortDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.custom, caseListRetractedC4SortDescendingStatus[0].Custom);
            // Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedC4SortDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Retracted.Id, caseListRetractedC4SortDescendingStatus[0].Id);
            // Assert.AreEqual(aCase4Retracted.microting_uid, caseListRetractedC4SortDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase4Retracted.site.microting_uid, caseListRetractedC4SortDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase4Retracted.site.name, caseListRetractedC4SortDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase4Retracted.status, caseListRetractedC4SortDescendingStatus[0].Status);
            // Assert.AreEqual(aCase4Retracted.check_list_id, caseListRetractedC4SortDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase4Retracted.unit.microting_uid, caseListRetractedC4SortDescendingStatus[0].UnitId);
            // Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedC4SortDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.version, caseListRetractedC4SortDescendingStatus[0].Version);
            // Assert.AreEqual(aCase4Retracted.worker.first_name + " " + aCase4Retracted.worker.last_name, caseListRetractedC4SortDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase4Retracted.workflow_state, caseListRetractedC4SortDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC4SortDescendingUnitId aCase4Retracted

            Assert.NotNull(caseListRetractedC4SortDescendingUnitId);
            Assert.That(caseListRetractedC4SortDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4Retracted.type, caseListRetractedC4SortDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase4Retracted.case_uid, caseListRetractedC4SortDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase4Retracted.microting_check_uid, caseListRetractedC4SortDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedC4SortDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.custom, caseListRetractedC4SortDescendingUnitId[0].Custom);
            // Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedC4SortDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Retracted.Id, caseListRetractedC4SortDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase4Retracted.microting_uid, caseListRetractedC4SortDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase4Retracted.site.microting_uid, caseListRetractedC4SortDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase4Retracted.site.name, caseListRetractedC4SortDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase4Retracted.status, caseListRetractedC4SortDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase4Retracted.check_list_id, caseListRetractedC4SortDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase4Retracted.unit.microting_uid, caseListRetractedC4SortDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedC4SortDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.version, caseListRetractedC4SortDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase4Retracted.worker.first_name + " " + aCase4Retracted.worker.last_name, caseListRetractedC4SortDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase4Retracted.workflow_state, caseListRetractedC4SortDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #endregion

            #region aCase Sort Asc w. DateTime

            #region aCase1Retracted sort asc w. DateTime

            #region caseListRetractedC1SortDtDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDtDoneAt);
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

            Assert.NotNull(caseListRetractedC1SortDtStatus);
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

            Assert.NotNull(caseListRetractedC1SortDtUnitId);
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

            Assert.NotNull(caseListRetractedC2SortDtDoneAt);
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

            Assert.NotNull(caseListRetractedC2SortDtStatus);
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

            Assert.NotNull(caseListRetractedC2SortDtUnitId);
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

            Assert.NotNull(caseListRetractedC3SortDtDoneAt);
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

            Assert.NotNull(caseListRetractedC3SortDtStatus);
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

            Assert.NotNull(caseListRetractedC3SortDtUnitId);
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

            Assert.NotNull(caseListRetractedC4SortDtDoneAt);
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

            Assert.NotNull(caseListRetractedC4SortDtStatus);
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

            Assert.NotNull(caseListRetractedC4SortDtUnitId);
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

            #region aCase Sort Des w. DateTime

            #region aCase1Retracted Sort Des w. DateTime

            #region caseListRetractedC1SortDtDescendingDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDtDescendingDoneAt);
            Assert.That(caseListRetractedC1SortDtDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1Retracted.type, caseListRetractedC1SortDtDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase1Retracted.case_uid, caseListRetractedC1SortDtDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase1Retracted.microting_check_uid, caseListRetractedC1SortDtDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedC1SortDtDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.custom, caseListRetractedC1SortDtDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedC1SortDtDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Retracted.Id, caseListRetractedC1SortDtDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase1Retracted.microting_uid, caseListRetractedC1SortDtDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1Retracted.site.microting_uid, caseListRetractedC1SortDtDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1Retracted.site.name, caseListRetractedC1SortDtDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1Retracted.status, caseListRetractedC1SortDtDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase1Retracted.check_list_id, caseListRetractedC1SortDtDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1Retracted.unit.microting_uid, caseListRetractedC1SortDtDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedC1SortDtDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.version, caseListRetractedC1SortDtDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase1Retracted.worker.first_name + " " + aCase1Retracted.worker.last_name, caseListRetractedC1SortDtDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase1Retracted.workflow_state, caseListRetractedC1SortDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC1SortDtDescendingStatus aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDtDescendingStatus);
            Assert.That(caseListRetractedC1SortDtDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1Retracted.type, caseListRetractedC1SortDtDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase1Retracted.case_uid, caseListRetractedC1SortDtDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase1Retracted.microting_check_uid, caseListRetractedC1SortDtDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedC1SortDtDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.custom, caseListRetractedC1SortDtDescendingStatus[0].Custom);
            // Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedC1SortDtDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Retracted.Id, caseListRetractedC1SortDtDescendingStatus[0].Id);
            // Assert.AreEqual(aCase1Retracted.microting_uid, caseListRetractedC1SortDtDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1Retracted.site.microting_uid, caseListRetractedC1SortDtDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase1Retracted.site.name, caseListRetractedC1SortDtDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase1Retracted.status, caseListRetractedC1SortDtDescendingStatus[0].Status);
            // Assert.AreEqual(aCase1Retracted.check_list_id, caseListRetractedC1SortDtDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase1Retracted.unit.microting_uid, caseListRetractedC1SortDtDescendingStatus[0].UnitId);
            // Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedC1SortDtDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.version, caseListRetractedC1SortDtDescendingStatus[0].Version);
            // Assert.AreEqual(aCase1Retracted.worker.first_name + " " + aCase1Retracted.worker.last_name, caseListRetractedC1SortDtDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase1Retracted.workflow_state, caseListRetractedC1SortDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC1SortDtDescendingUnitId aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDtDescendingUnitId);
            Assert.That(caseListRetractedC1SortDtDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase1Retracted.type, caseListRetractedC1SortDtDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase1Retracted.case_uid, caseListRetractedC1SortDtDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase1Retracted.microting_check_uid, caseListRetractedC1SortDtDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedC1SortDtDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.custom, caseListRetractedC1SortDtDescendingUnitId[0].Custom);
            // Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedC1SortDtDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase1Retracted.Id, caseListRetractedC1SortDtDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase1Retracted.microting_uid, caseListRetractedC1SortDtDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1Retracted.site.microting_uid, caseListRetractedC1SortDtDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase1Retracted.site.name, caseListRetractedC1SortDtDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase1Retracted.status, caseListRetractedC1SortDtDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase1Retracted.check_list_id, caseListRetractedC1SortDtDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1Retracted.unit.microting_uid, caseListRetractedC1SortDtDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedC1SortDtDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase1Retracted.version, caseListRetractedC1SortDtDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase1Retracted.worker.first_name + " " + aCase1Retracted.worker.last_name, caseListRetractedC1SortDtDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase1Retracted.workflow_state, caseListRetractedC1SortDtDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase2Retracted Sort Des w. DateTime

            #region caseListRetractedC2SortDtDescendingDoneAt aCase2Retracted

            Assert.NotNull(caseListRetractedC2SortDtDescendingDoneAt);
            Assert.That(caseListRetractedC2SortDtDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2Retracted.type, caseListRetractedC2SortDtDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase2Retracted.case_uid, caseListRetractedC2SortDtDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase2Retracted.microting_check_uid, caseListRetractedC2SortDtDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedC2SortDtDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.custom, caseListRetractedC2SortDtDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedC2SortDtDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Retracted.Id, caseListRetractedC2SortDtDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase2Retracted.microting_uid, caseListRetractedC2SortDtDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2Retracted.site.microting_uid, caseListRetractedC2SortDtDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2Retracted.site.name, caseListRetractedC2SortDtDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2Retracted.status, caseListRetractedC2SortDtDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase2Retracted.check_list_id, caseListRetractedC2SortDtDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2Retracted.unit.microting_uid, caseListRetractedC2SortDtDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedC2SortDtDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.version, caseListRetractedC2SortDtDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase2Retracted.worker.first_name + " " + aCase2Retracted.worker.last_name, caseListRetractedC2SortDtDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase2Retracted.workflow_state, caseListRetractedC2SortDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC2SortDtDescendingStatus aCase2Retracted

            Assert.NotNull(caseListRetractedC2SortDtDescendingStatus);
            Assert.That(caseListRetractedC2SortDtDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2Retracted.type, caseListRetractedC2SortDtDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase2Retracted.case_uid, caseListRetractedC2SortDtDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase2Retracted.microting_check_uid, caseListRetractedC2SortDtDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedC2SortDtDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.custom, caseListRetractedC2SortDtDescendingStatus[0].Custom);
            // Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedC2SortDtDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Retracted.Id, caseListRetractedC2SortDtDescendingStatus[0].Id);
            // Assert.AreEqual(aCase2Retracted.microting_uid, caseListRetractedC2SortDtDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2Retracted.site.microting_uid, caseListRetractedC2SortDtDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase2Retracted.site.name, caseListRetractedC2SortDtDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase2Retracted.status, caseListRetractedC2SortDtDescendingStatus[0].Status);
            // Assert.AreEqual(aCase2Retracted.check_list_id, caseListRetractedC2SortDtDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase2Retracted.unit.microting_uid, caseListRetractedC2SortDtDescendingStatus[0].UnitId);
            // Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedC2SortDtDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.version, caseListRetractedC2SortDtDescendingStatus[0].Version);
            // Assert.AreEqual(aCase2Retracted.worker.first_name + " " + aCase2Retracted.worker.last_name, caseListRetractedC2SortDtDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase2Retracted.workflow_state, caseListRetractedC2SortDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC2SortDtDescendingUnitId aCase2Retracted

            Assert.NotNull(caseListRetractedC2SortDtDescendingUnitId);
            Assert.That(caseListRetractedC2SortDtDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase2Retracted.type, caseListRetractedC2SortDtDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase2Retracted.case_uid, caseListRetractedC2SortDtDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase2Retracted.microting_check_uid, caseListRetractedC2SortDtDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedC2SortDtDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.custom, caseListRetractedC2SortDtDescendingUnitId[0].Custom);
            // Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedC2SortDtDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase2Retracted.Id, caseListRetractedC2SortDtDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase2Retracted.microting_uid, caseListRetractedC2SortDtDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2Retracted.site.microting_uid, caseListRetractedC2SortDtDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase2Retracted.site.name, caseListRetractedC2SortDtDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase2Retracted.status, caseListRetractedC2SortDtDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase2Retracted.check_list_id, caseListRetractedC2SortDtDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2Retracted.unit.microting_uid, caseListRetractedC2SortDtDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedC2SortDtDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase2Retracted.version, caseListRetractedC2SortDtDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase2Retracted.worker.first_name + " " + aCase2Retracted.worker.last_name, caseListRetractedC2SortDtDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase2Retracted.workflow_state, caseListRetractedC2SortDtDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase3Retracted Sort Des w. DateTime

            #region caseListRetractedC3SortDtDescendingDoneAt aCase3Retracted

            Assert.NotNull(caseListRetractedC3SortDtDescendingDoneAt);
            Assert.That(caseListRetractedC3SortDtDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3Retracted.type, caseListRetractedC3SortDtDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase3Retracted.case_uid, caseListRetractedC3SortDtDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase3Retracted.microting_check_uid, caseListRetractedC3SortDtDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedC3SortDtDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.custom, caseListRetractedC3SortDtDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedC3SortDtDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Retracted.Id, caseListRetractedC3SortDtDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase3Retracted.microting_uid, caseListRetractedC3SortDtDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3Retracted.site.microting_uid, caseListRetractedC3SortDtDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3Retracted.site.name, caseListRetractedC3SortDtDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3Retracted.status, caseListRetractedC3SortDtDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase3Retracted.check_list_id, caseListRetractedC3SortDtDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3Retracted.unit.microting_uid, caseListRetractedC3SortDtDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedC3SortDtDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.version, caseListRetractedC3SortDtDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase3Retracted.worker.first_name + " " + aCase3Retracted.worker.last_name, caseListRetractedC3SortDtDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase3Retracted.workflow_state, caseListRetractedC3SortDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC3SortDtDescendingStatus aCase3Retracted

            Assert.NotNull(caseListRetractedC3SortDtDescendingStatus);
            Assert.That(caseListRetractedC3SortDtDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3Retracted.type, caseListRetractedC3SortDtDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase3Retracted.case_uid, caseListRetractedC3SortDtDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase3Retracted.microting_check_uid, caseListRetractedC3SortDtDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedC3SortDtDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.custom, caseListRetractedC3SortDtDescendingStatus[0].Custom);
            // Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedC3SortDtDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Retracted.Id, caseListRetractedC3SortDtDescendingStatus[0].Id);
            // Assert.AreEqual(aCase3Retracted.microting_uid, caseListRetractedC3SortDtDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3Retracted.site.microting_uid, caseListRetractedC3SortDtDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase3Retracted.site.name, caseListRetractedC3SortDtDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase3Retracted.status, caseListRetractedC3SortDtDescendingStatus[0].Status);
            // Assert.AreEqual(aCase3Retracted.check_list_id, caseListRetractedC3SortDtDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase3Retracted.unit.microting_uid, caseListRetractedC3SortDtDescendingStatus[0].UnitId);
            // Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedC3SortDtDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.version, caseListRetractedC3SortDtDescendingStatus[0].Version);
            // Assert.AreEqual(aCase3Retracted.worker.first_name + " " + aCase3Retracted.worker.last_name, caseListRetractedC3SortDtDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase3Retracted.workflow_state, caseListRetractedC3SortDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC3SortDtDescendingUnitId aCase3Retracted

            Assert.NotNull(caseListRetractedC3SortDtDescendingUnitId);
            Assert.That(caseListRetractedC3SortDtDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase3Retracted.type, caseListRetractedC3SortDtDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase3Retracted.case_uid, caseListRetractedC3SortDtDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase3Retracted.microting_check_uid, caseListRetractedC3SortDtDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedC3SortDtDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.custom, caseListRetractedC3SortDtDescendingUnitId[0].Custom);
            // Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedC3SortDtDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase3Retracted.Id, caseListRetractedC3SortDtDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase3Retracted.microting_uid, caseListRetractedC3SortDtDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3Retracted.site.microting_uid, caseListRetractedC3SortDtDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase3Retracted.site.name, caseListRetractedC3SortDtDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase3Retracted.status, caseListRetractedC3SortDtDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase3Retracted.check_list_id, caseListRetractedC3SortDtDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3Retracted.unit.microting_uid, caseListRetractedC3SortDtDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedC3SortDtDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase3Retracted.version, caseListRetractedC3SortDtDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase3Retracted.worker.first_name + " " + aCase3Retracted.worker.last_name, caseListRetractedC3SortDtDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase3Retracted.workflow_state, caseListRetractedC3SortDtDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #region aCase4Retracted Sort Des w. DateTime

            #region caseListRetractedC4SortDtDescendingDoneAt aCase4Retracted

            Assert.NotNull(caseListRetractedC4SortDtDescendingDoneAt);
            Assert.That(caseListRetractedC4SortDtDescendingDoneAt.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4Retracted.type, caseListRetractedC4SortDtDescendingDoneAt[0].CaseType);
            // Assert.AreEqual(aCase4Retracted.case_uid, caseListRetractedC4SortDtDescendingDoneAt[0].CaseUId);
            // Assert.AreEqual(aCase4Retracted.microting_check_uid, caseListRetractedC4SortDtDescendingDoneAt[0].CheckUIid);
            // Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedC4SortDtDescendingDoneAt[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.custom, caseListRetractedC4SortDtDescendingDoneAt[0].Custom);
            // Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedC4SortDtDescendingDoneAt[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Retracted.Id, caseListRetractedC4SortDtDescendingDoneAt[0].Id);
            // Assert.AreEqual(aCase4Retracted.microting_uid, caseListRetractedC4SortDtDescendingDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4Retracted.site.microting_uid, caseListRetractedC4SortDtDescendingDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4Retracted.site.name, caseListRetractedC4SortDtDescendingDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4Retracted.status, caseListRetractedC4SortDtDescendingDoneAt[0].Status);
            // Assert.AreEqual(aCase4Retracted.check_list_id, caseListRetractedC4SortDtDescendingDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4Retracted.unit.microting_uid, caseListRetractedC4SortDtDescendingDoneAt[0].UnitId);
            // Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedC4SortDtDescendingDoneAt[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.version, caseListRetractedC4SortDtDescendingDoneAt[0].Version);
            // Assert.AreEqual(aCase4Retracted.worker.first_name + " " + aCase4Retracted.worker.last_name, caseListRetractedC4SortDtDescendingDoneAt[0].WorkerName);
            // Assert.AreEqual(aCase4Retracted.workflow_state, caseListRetractedC4SortDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedC4SortDtDescendingStatus aCase4Retracted

            Assert.NotNull(caseListRetractedC4SortDtDescendingStatus);
            Assert.That(caseListRetractedC4SortDtDescendingStatus.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4Retracted.type, caseListRetractedC4SortDtDescendingStatus[0].CaseType);
            // Assert.AreEqual(aCase4Retracted.case_uid, caseListRetractedC4SortDtDescendingStatus[0].CaseUId);
            // Assert.AreEqual(aCase4Retracted.microting_check_uid, caseListRetractedC4SortDtDescendingStatus[0].CheckUIid);
            // Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedC4SortDtDescendingStatus[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.custom, caseListRetractedC4SortDtDescendingStatus[0].Custom);
            // Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedC4SortDtDescendingStatus[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Retracted.Id, caseListRetractedC4SortDtDescendingStatus[0].Id);
            // Assert.AreEqual(aCase4Retracted.microting_uid, caseListRetractedC4SortDtDescendingStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase4Retracted.site.microting_uid, caseListRetractedC4SortDtDescendingStatus[0].SiteId);
            // Assert.AreEqual(aCase4Retracted.site.name, caseListRetractedC4SortDtDescendingStatus[0].SiteName);
            // Assert.AreEqual(aCase4Retracted.status, caseListRetractedC4SortDtDescendingStatus[0].Status);
            // Assert.AreEqual(aCase4Retracted.check_list_id, caseListRetractedC4SortDtDescendingStatus[0].TemplatId);
            // Assert.AreEqual(aCase4Retracted.unit.microting_uid, caseListRetractedC4SortDtDescendingStatus[0].UnitId);
            // Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedC4SortDtDescendingStatus[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.version, caseListRetractedC4SortDtDescendingStatus[0].Version);
            // Assert.AreEqual(aCase4Retracted.worker.first_name + " " + aCase4Retracted.worker.last_name, caseListRetractedC4SortDtDescendingStatus[0].WorkerName);
            // Assert.AreEqual(aCase4Retracted.workflow_state, caseListRetractedC4SortDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedC4SortDtDescendingUnitId aCase4Retracted

            Assert.NotNull(caseListRetractedC4SortDtDescendingUnitId);
            Assert.That(caseListRetractedC4SortDtDescendingUnitId.Count, Is.EqualTo(0));
            // Assert.AreEqual(aCase4Retracted.type, caseListRetractedC4SortDtDescendingUnitId[0].CaseType);
            // Assert.AreEqual(aCase4Retracted.case_uid, caseListRetractedC4SortDtDescendingUnitId[0].CaseUId);
            // Assert.AreEqual(aCase4Retracted.microting_check_uid, caseListRetractedC4SortDtDescendingUnitId[0].CheckUIid);
            // Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedC4SortDtDescendingUnitId[0].CreatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.custom, caseListRetractedC4SortDtDescendingUnitId[0].Custom);
            // Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedC4SortDtDescendingUnitId[0].DoneAt.ToString());
            // Assert.AreEqual(aCase4Retracted.Id, caseListRetractedC4SortDtDescendingUnitId[0].Id);
            // Assert.AreEqual(aCase4Retracted.microting_uid, caseListRetractedC4SortDtDescendingUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase4Retracted.site.microting_uid, caseListRetractedC4SortDtDescendingUnitId[0].SiteId);
            // Assert.AreEqual(aCase4Retracted.site.name, caseListRetractedC4SortDtDescendingUnitId[0].SiteName);
            // Assert.AreEqual(aCase4Retracted.status, caseListRetractedC4SortDtDescendingUnitId[0].Status);
            // Assert.AreEqual(aCase4Retracted.check_list_id, caseListRetractedC4SortDtDescendingUnitId[0].TemplatId);
            // Assert.AreEqual(aCase4Retracted.unit.microting_uid, caseListRetractedC4SortDtDescendingUnitId[0].UnitId);
            // Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedC4SortDtDescendingUnitId[0].UpdatedAt.ToString());
            // Assert.AreEqual(aCase4Retracted.version, caseListRetractedC4SortDtDescendingUnitId[0].Version);
            // Assert.AreEqual(aCase4Retracted.worker.first_name + " " + aCase4Retracted.worker.last_name, caseListRetractedC4SortDtDescendingUnitId[0].WorkerName);
            // Assert.AreEqual(aCase4Retracted.workflow_state, caseListRetractedC4SortDtDescendingUnitId[0].WorkflowState);

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