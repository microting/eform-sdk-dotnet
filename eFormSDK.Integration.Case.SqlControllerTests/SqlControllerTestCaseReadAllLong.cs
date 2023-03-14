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
using System.Linq;
using System.Threading.Tasks;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using NUnit.Framework;

namespace eFormSDK.Integration.Case.SqlControllerTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SqlControllerTestCaseReadAllLong : DbTestFixture
    {
        private SqlController sut;

        private TestHelpers testHelpers;

//        private string path;
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

            sut = new SqlController(dbContextHelper);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers(ConnectionString);
            await testHelpers.GenerateDefaultLanguages();
            await sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            await sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            await sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }


        [Test]
        public async Task SQL_PostCase_CaseReadAll()
        {
            // Arrance

            #region Arrance

            Random rnd = new Random();

            #region Template1

            DateTime cl1_Ca = DateTime.UtcNow;
            DateTime cl1_Ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

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

            Worker worker1 = await testHelpers.CreateWorker(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), await testHelpers.GetRandomInt());
            Worker worker2 = await testHelpers.CreateWorker(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), await testHelpers.GetRandomInt());
            Worker worker3 = await testHelpers.CreateWorker(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), await testHelpers.GetRandomInt());
            Worker worker4 = await testHelpers.CreateWorker(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), await testHelpers.GetRandomInt());

            #endregion

            #region site

            Site site1 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), await testHelpers.GetRandomInt());
            Site site2 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), await testHelpers.GetRandomInt());
            Site site3 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), await testHelpers.GetRandomInt());
            Site site4 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), await testHelpers.GetRandomInt());

            #endregion

            #region units

            Unit unit1 = await testHelpers.CreateUnit(await testHelpers.GetRandomInt(),
                await testHelpers.GetRandomInt(), site1, 348);
            Unit unit2 = await testHelpers.CreateUnit(await testHelpers.GetRandomInt(),
                await testHelpers.GetRandomInt(), site2, 348);
            Unit unit3 = await testHelpers.CreateUnit(await testHelpers.GetRandomInt(),
                await testHelpers.GetRandomInt(), site3, 348);
            Unit unit4 = await testHelpers.CreateUnit(await testHelpers.GetRandomInt(),
                await testHelpers.GetRandomInt(), site4, 348);

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
                Constants.WorkflowStates.Created, "1000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
                Constants.WorkflowStates.Created, "1000", false, Constants.CaseSortParameters.Status, timeZoneInfo);
            List<Microting.eForm.Dto.Case> caseListC1SortUnitId = await sut.CaseReadAll(cl1.Id, null, null,
                Constants.WorkflowStates.Created, "1000", false, Constants.CaseSortParameters.UnitId, timeZoneInfo);
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
                Constants.WorkflowStates.Created, "0004", true, Constants.CaseSortParameters.Status, timeZoneInfo);
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
                DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-8), Constants.WorkflowStates.Created, "1000",
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
                Constants.WorkflowStates.Retracted, "10000", false, Constants.CaseSortParameters.DoneAt, timeZoneInfo);
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
            Assert.AreEqual(4, caseListDoneAt.Count);
            Assert.AreEqual(aCase1.Type, caseListDoneAt[1].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListDoneAt[1].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListDoneAt[1].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListDoneAt[1].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListDoneAt[1].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListDoneAt[1].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListDoneAt[1].SiteName);
            Assert.AreEqual(aCase1.Status, caseListDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListDoneAt[1].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListDoneAt[1].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListDoneAt[1].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName, caseListDoneAt[1].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListDoneAt[1].WorkflowState);

            #endregion

            #region caseListDoneAt aCase2

            Assert.AreEqual(aCase2.Type, caseListDoneAt[3].CaseType);
            Assert.AreEqual(aCase2.CaseUid, caseListDoneAt[3].CaseUId);
            Assert.AreEqual(aCase2.MicrotingCheckUid, caseListDoneAt[3].CheckUIid);
            Assert.AreEqual(c2_ca.ToString(), caseListDoneAt[3].CreatedAt.ToString());
            Assert.AreEqual(aCase2.Custom, caseListDoneAt[3].Custom);
            Assert.AreEqual(c2_da.ToString(), caseListDoneAt[3].DoneAt.ToString());
            Assert.AreEqual(aCase2.Id, caseListDoneAt[3].Id);
            Assert.AreEqual(aCase2.MicrotingUid, caseListDoneAt[3].MicrotingUId);
            Assert.AreEqual(aCase2.Site.MicrotingUid, caseListDoneAt[3].SiteId);
            Assert.AreEqual(aCase2.Site.Name, caseListDoneAt[3].SiteName);
            Assert.AreEqual(aCase2.Status, caseListDoneAt[3].Status);
            Assert.AreEqual(cl1.Id, caseListDoneAt[3].TemplatId);
            Assert.AreEqual(aCase2.Unit.MicrotingUid, caseListDoneAt[3].UnitId);
//            Assert.AreEqual(c2_ua.ToString(), caseListDoneAt[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase2.Version, caseListDoneAt[3].Version);
            Assert.AreEqual(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName, caseListDoneAt[3].WorkerName);
            Assert.AreEqual(aCase2.WorkflowState, caseListDoneAt[3].WorkflowState);

            #endregion

            #region caseListDoneAt aCase3

            Assert.AreEqual(aCase3.Type, caseListDoneAt[0].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListDoneAt[0].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListDoneAt[0].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListDoneAt[0].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListDoneAt[0].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListDoneAt[0].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListDoneAt[0].SiteName);
            Assert.AreEqual(aCase3.Status, caseListDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListDoneAt[0].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListDoneAt[0].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListDoneAt[0].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName, caseListDoneAt[0].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListDoneAt[0].WorkflowState);

            #endregion

            #region caseListDoneAt aCase4

            Assert.AreEqual(aCase4.Type, caseListDoneAt[2].CaseType);
            Assert.AreEqual(aCase4.CaseUid, caseListDoneAt[2].CaseUId);
            Assert.AreEqual(aCase4.MicrotingCheckUid, caseListDoneAt[2].CheckUIid);
            Assert.AreEqual(c4_ca.ToString(), caseListDoneAt[2].CreatedAt.ToString());
            Assert.AreEqual(aCase4.Custom, caseListDoneAt[2].Custom);
            Assert.AreEqual(c4_da.ToString(), caseListDoneAt[2].DoneAt.ToString());
            Assert.AreEqual(aCase4.Id, caseListDoneAt[2].Id);
            Assert.AreEqual(aCase4.MicrotingUid, caseListDoneAt[2].MicrotingUId);
            Assert.AreEqual(aCase4.Site.MicrotingUid, caseListDoneAt[2].SiteId);
            Assert.AreEqual(aCase4.Site.Name, caseListDoneAt[2].SiteName);
            Assert.AreEqual(aCase4.Status, caseListDoneAt[2].Status);
            Assert.AreEqual(cl1.Id, caseListDoneAt[2].TemplatId);
            Assert.AreEqual(aCase4.Unit.MicrotingUid, caseListDoneAt[2].UnitId);
//            Assert.AreEqual(c4_ua.ToString(), caseListDoneAt[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase4.Version, caseListDoneAt[2].Version);
            Assert.AreEqual(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName, caseListDoneAt[2].WorkerName);
            Assert.AreEqual(aCase4.WorkflowState, caseListDoneAt[2].WorkflowState);

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

            #region Def Sort Des

            #region caseListDescendingDoneAt Def Sort Des

            #region caseListDescendingDoneAt aCase1

            Assert.NotNull(caseListDescendingDoneAt);
            Assert.AreEqual(4, caseListDescendingDoneAt.Count);
            Assert.AreEqual(aCase1.Type, caseListDescendingDoneAt[2].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListDescendingDoneAt[2].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListDescendingDoneAt[2].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDescendingDoneAt[2].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListDescendingDoneAt[2].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDescendingDoneAt[2].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListDescendingDoneAt[2].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListDescendingDoneAt[2].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListDescendingDoneAt[2].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListDescendingDoneAt[2].SiteName);
            Assert.AreEqual(aCase1.Status, caseListDescendingDoneAt[2].Status);
            Assert.AreEqual(cl1.Id, caseListDescendingDoneAt[2].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListDescendingDoneAt[2].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListDescendingDoneAt[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListDescendingDoneAt[2].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName,
                caseListDescendingDoneAt[2].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListDescendingDoneAt[2].WorkflowState);

            #endregion

            #region caseListDescendingDoneAt aCase2

            Assert.AreEqual(aCase2.Type, caseListDescendingDoneAt[0].CaseType);
            Assert.AreEqual(aCase2.CaseUid, caseListDescendingDoneAt[0].CaseUId);
            Assert.AreEqual(aCase2.MicrotingCheckUid, caseListDescendingDoneAt[0].CheckUIid);
            Assert.AreEqual(c2_ca.ToString(), caseListDescendingDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase2.Custom, caseListDescendingDoneAt[0].Custom);
            Assert.AreEqual(c2_da.ToString(), caseListDescendingDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase2.Id, caseListDescendingDoneAt[0].Id);
            Assert.AreEqual(aCase2.MicrotingUid, caseListDescendingDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase2.Site.MicrotingUid, caseListDescendingDoneAt[0].SiteId);
            Assert.AreEqual(aCase2.Site.Name, caseListDescendingDoneAt[0].SiteName);
            Assert.AreEqual(aCase2.Status, caseListDescendingDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListDescendingDoneAt[0].TemplatId);
            Assert.AreEqual(aCase2.Unit.MicrotingUid, caseListDescendingDoneAt[0].UnitId);
//            Assert.AreEqual(c2_ua.ToString(), caseListDescendingDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase2.Version, caseListDescendingDoneAt[0].Version);
            Assert.AreEqual(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName,
                caseListDescendingDoneAt[0].WorkerName);
            Assert.AreEqual(aCase2.WorkflowState, caseListDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListDescendingDoneAt aCase3

            Assert.AreEqual(aCase3.Type, caseListDescendingDoneAt[3].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListDescendingDoneAt[3].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListDescendingDoneAt[3].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDescendingDoneAt[3].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListDescendingDoneAt[3].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDescendingDoneAt[3].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListDescendingDoneAt[3].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListDescendingDoneAt[3].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListDescendingDoneAt[3].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListDescendingDoneAt[3].SiteName);
            Assert.AreEqual(aCase3.Status, caseListDescendingDoneAt[3].Status);
            Assert.AreEqual(cl1.Id, caseListDescendingDoneAt[3].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListDescendingDoneAt[3].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListDescendingDoneAt[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListDescendingDoneAt[3].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName,
                caseListDescendingDoneAt[3].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListDescendingDoneAt[3].WorkflowState);

            #endregion

            #region caseListDescendingDoneAt aCase4

            Assert.AreEqual(aCase4.Type, caseListDescendingDoneAt[1].CaseType);
            Assert.AreEqual(aCase4.CaseUid, caseListDescendingDoneAt[1].CaseUId);
            Assert.AreEqual(aCase4.MicrotingCheckUid, caseListDescendingDoneAt[1].CheckUIid);
            Assert.AreEqual(c4_ca.ToString(), caseListDescendingDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase4.Custom, caseListDescendingDoneAt[1].Custom);
            Assert.AreEqual(c4_da.ToString(), caseListDescendingDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase4.Id, caseListDescendingDoneAt[1].Id);
            Assert.AreEqual(aCase4.MicrotingUid, caseListDescendingDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase4.Site.MicrotingUid, caseListDescendingDoneAt[1].SiteId);
            Assert.AreEqual(aCase4.Site.Name, caseListDescendingDoneAt[1].SiteName);
            Assert.AreEqual(aCase4.Status, caseListDescendingDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListDescendingDoneAt[1].TemplatId);
            Assert.AreEqual(aCase4.Unit.MicrotingUid, caseListDescendingDoneAt[1].UnitId);
//            Assert.AreEqual(c4_ua.ToString(), caseListDescendingDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase4.Version, caseListDescendingDoneAt[1].Version);
            Assert.AreEqual(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName,
                caseListDescendingDoneAt[1].WorkerName);
            Assert.AreEqual(aCase4.WorkflowState, caseListDescendingDoneAt[1].WorkflowState);

            #endregion

            #endregion

            #region caseListDescendingStatus Def Sort Des

            #region caseListDescendingStatus aCase1

            Assert.NotNull(caseListDescendingStatus);
            Assert.AreEqual(4, caseListDescendingStatus.Count);
            Assert.AreEqual(aCase1.Type, caseListDescendingStatus[3].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListDescendingStatus[3].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListDescendingStatus[3].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDescendingStatus[3].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListDescendingStatus[3].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDescendingStatus[3].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListDescendingStatus[3].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListDescendingStatus[3].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListDescendingStatus[3].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListDescendingStatus[3].SiteName);
            Assert.AreEqual(aCase1.Status, caseListDescendingStatus[3].Status);
            Assert.AreEqual(aCase1.CheckListId, caseListDescendingStatus[3].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListDescendingStatus[3].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListDescendingStatus[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListDescendingStatus[3].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName,
                caseListDescendingStatus[3].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListDescendingStatus[3].WorkflowState);

            #endregion

            #region caseListDescendingStatus aCase2

            Assert.AreEqual(aCase2.Type, caseListDescendingStatus[2].CaseType);
            Assert.AreEqual(aCase2.CaseUid, caseListDescendingStatus[2].CaseUId);
            Assert.AreEqual(aCase2.MicrotingCheckUid, caseListDescendingStatus[2].CheckUIid);
            Assert.AreEqual(c2_ca.ToString(), caseListDescendingStatus[2].CreatedAt.ToString());
            Assert.AreEqual(aCase2.Custom, caseListDescendingStatus[2].Custom);
            Assert.AreEqual(c2_da.ToString(), caseListDescendingStatus[2].DoneAt.ToString());
            Assert.AreEqual(aCase2.Id, caseListDescendingStatus[2].Id);
            Assert.AreEqual(aCase2.MicrotingUid, caseListDescendingStatus[2].MicrotingUId);
            Assert.AreEqual(aCase2.Site.MicrotingUid, caseListDescendingStatus[2].SiteId);
            Assert.AreEqual(aCase2.Site.Name, caseListDescendingStatus[2].SiteName);
            Assert.AreEqual(aCase2.Status, caseListDescendingStatus[2].Status);
            Assert.AreEqual(aCase2.CheckListId, caseListDescendingStatus[2].TemplatId);
            Assert.AreEqual(aCase2.Unit.MicrotingUid, caseListDescendingStatus[2].UnitId);
//            Assert.AreEqual(c2_ua.ToString(), caseListDescendingStatus[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase2.Version, caseListDescendingStatus[2].Version);
            Assert.AreEqual(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName,
                caseListDescendingStatus[2].WorkerName);
            Assert.AreEqual(aCase2.WorkflowState, caseListDescendingStatus[2].WorkflowState);

            #endregion

            #region caseListDescendingStatus aCase3

            Assert.AreEqual(aCase3.Type, caseListDescendingStatus[1].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListDescendingStatus[1].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListDescendingStatus[1].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDescendingStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListDescendingStatus[1].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDescendingStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListDescendingStatus[1].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListDescendingStatus[1].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListDescendingStatus[1].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListDescendingStatus[1].SiteName);
            Assert.AreEqual(aCase3.Status, caseListDescendingStatus[1].Status);
            Assert.AreEqual(aCase3.CheckListId, caseListDescendingStatus[1].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListDescendingStatus[1].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListDescendingStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListDescendingStatus[1].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName,
                caseListDescendingStatus[1].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListDescendingStatus[1].WorkflowState);

            #endregion

            #region caseListDescendingStatus aCase4

            Assert.AreEqual(aCase4.Type, caseListDescendingStatus[0].CaseType);
            Assert.AreEqual(aCase4.CaseUid, caseListDescendingStatus[0].CaseUId);
            Assert.AreEqual(aCase4.MicrotingCheckUid, caseListDescendingStatus[0].CheckUIid);
            Assert.AreEqual(c4_ca.ToString(), caseListDescendingStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase4.Custom, caseListDescendingStatus[0].Custom);
            Assert.AreEqual(c4_da.ToString(), caseListDescendingStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase4.Id, caseListDescendingStatus[0].Id);
            Assert.AreEqual(aCase4.MicrotingUid, caseListDescendingStatus[0].MicrotingUId);
            Assert.AreEqual(aCase4.Site.MicrotingUid, caseListDescendingStatus[0].SiteId);
            Assert.AreEqual(aCase4.Site.Name, caseListDescendingStatus[0].SiteName);
            Assert.AreEqual(aCase4.Status, caseListDescendingStatus[0].Status);
            Assert.AreEqual(aCase4.CheckListId, caseListDescendingStatus[0].TemplatId);
            Assert.AreEqual(aCase4.Unit.MicrotingUid, caseListDescendingStatus[0].UnitId);
//            Assert.AreEqual(c4_ua.ToString(), caseListDescendingStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase4.Version, caseListDescendingStatus[0].Version);
            Assert.AreEqual(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName,
                caseListDescendingStatus[0].WorkerName);
            Assert.AreEqual(aCase4.WorkflowState, caseListDescendingStatus[0].WorkflowState);

            #endregion

            #endregion

            #region caseListDescendingUnitId Def Sort Des

            #region caseListDescendingUnitId aCase1

            Assert.NotNull(caseListDescendingUnitId);
            Assert.AreEqual(4, caseListDescendingUnitId.Count);
            Assert.AreEqual(aCase1.Type, caseListDescendingUnitId[3].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListDescendingUnitId[3].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListDescendingUnitId[3].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDescendingUnitId[3].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListDescendingUnitId[3].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDescendingUnitId[3].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListDescendingUnitId[3].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListDescendingUnitId[3].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListDescendingUnitId[3].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListDescendingUnitId[3].SiteName);
            Assert.AreEqual(aCase1.Status, caseListDescendingUnitId[3].Status);
            Assert.AreEqual(aCase1.CheckListId, caseListDescendingUnitId[3].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListDescendingUnitId[3].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListDescendingUnitId[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListDescendingUnitId[3].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName,
                caseListDescendingUnitId[3].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListDescendingUnitId[3].WorkflowState);

            #endregion

            #region caseListDescendingUnitId aCase2

            Assert.AreEqual(aCase2.Type, caseListDescendingUnitId[2].CaseType);
            Assert.AreEqual(aCase2.CaseUid, caseListDescendingUnitId[2].CaseUId);
            Assert.AreEqual(aCase2.MicrotingCheckUid, caseListDescendingUnitId[2].CheckUIid);
            Assert.AreEqual(c2_ca.ToString(), caseListDescendingUnitId[2].CreatedAt.ToString());
            Assert.AreEqual(aCase2.Custom, caseListDescendingUnitId[2].Custom);
            Assert.AreEqual(c2_da.ToString(), caseListDescendingUnitId[2].DoneAt.ToString());
            Assert.AreEqual(aCase2.Id, caseListDescendingUnitId[2].Id);
            Assert.AreEqual(aCase2.MicrotingUid, caseListDescendingUnitId[2].MicrotingUId);
            Assert.AreEqual(aCase2.Site.MicrotingUid, caseListDescendingUnitId[2].SiteId);
            Assert.AreEqual(aCase2.Site.Name, caseListDescendingUnitId[2].SiteName);
            Assert.AreEqual(aCase2.Status, caseListDescendingUnitId[2].Status);
            Assert.AreEqual(aCase2.CheckListId, caseListDescendingUnitId[2].TemplatId);
            Assert.AreEqual(aCase2.Unit.MicrotingUid, caseListDescendingUnitId[2].UnitId);
//            Assert.AreEqual(c2_ua.ToString(), caseListDescendingUnitId[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase2.Version, caseListDescendingUnitId[2].Version);
            Assert.AreEqual(aCase2.Worker.FirstName + " " + aCase2.Worker.LastName,
                caseListDescendingUnitId[2].WorkerName);
            Assert.AreEqual(aCase2.WorkflowState, caseListDescendingUnitId[2].WorkflowState);

            #endregion

            #region caseListDescendingUnitId aCase3

            Assert.AreEqual(aCase3.Type, caseListDescendingUnitId[1].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListDescendingUnitId[1].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListDescendingUnitId[1].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDescendingUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListDescendingUnitId[1].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDescendingUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListDescendingUnitId[1].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListDescendingUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListDescendingUnitId[1].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListDescendingUnitId[1].SiteName);
            Assert.AreEqual(aCase3.Status, caseListDescendingUnitId[1].Status);
            Assert.AreEqual(aCase3.CheckListId, caseListDescendingUnitId[1].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListDescendingUnitId[1].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListDescendingUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListDescendingUnitId[1].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName,
                caseListDescendingUnitId[1].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListDescendingUnitId[1].WorkflowState);

            #endregion

            #region caseListDescendingUnitId aCase4

            Assert.AreEqual(aCase4.Type, caseListDescendingUnitId[0].CaseType);
            Assert.AreEqual(aCase4.CaseUid, caseListDescendingUnitId[0].CaseUId);
            Assert.AreEqual(aCase4.MicrotingCheckUid, caseListDescendingUnitId[0].CheckUIid);
            Assert.AreEqual(c4_ca.ToString(), caseListDescendingUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase4.Custom, caseListDescendingUnitId[0].Custom);
            Assert.AreEqual(c4_da.ToString(), caseListDescendingUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase4.Id, caseListDescendingUnitId[0].Id);
            Assert.AreEqual(aCase4.MicrotingUid, caseListDescendingUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase4.Site.MicrotingUid, caseListDescendingUnitId[0].SiteId);
            Assert.AreEqual(aCase4.Site.Name, caseListDescendingUnitId[0].SiteName);
            Assert.AreEqual(aCase4.Status, caseListDescendingUnitId[0].Status);
            Assert.AreEqual(aCase4.CheckListId, caseListDescendingUnitId[0].TemplatId);
            Assert.AreEqual(aCase4.Unit.MicrotingUid, caseListDescendingUnitId[0].UnitId);
//            Assert.AreEqual(c4_ua.ToString(), caseListDescendingUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase4.Version, caseListDescendingUnitId[0].Version);
            Assert.AreEqual(aCase4.Worker.FirstName + " " + aCase4.Worker.LastName,
                caseListDescendingUnitId[0].WorkerName);
            Assert.AreEqual(aCase4.WorkflowState, caseListDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #endregion

            #region Def Sort Asc w. DateTime

            #region caseListDtDoneAt Def Sort Asc w. DateTime

            #region caseListDtDoneAt aCase1

            Assert.NotNull(caseListDtDoneAt);
            Assert.AreEqual(2, caseListDtDoneAt.Count);
            Assert.AreEqual(aCase1.Type, caseListDtDoneAt[1].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListDtDoneAt[1].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListDtDoneAt[1].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDtDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListDtDoneAt[1].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDtDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListDtDoneAt[1].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListDtDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListDtDoneAt[1].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListDtDoneAt[1].SiteName);
            Assert.AreEqual(aCase1.Status, caseListDtDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListDtDoneAt[1].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListDtDoneAt[1].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListDtDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListDtDoneAt[1].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName, caseListDtDoneAt[1].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListDtDoneAt[1].WorkflowState);

            #endregion

            #region caseListDtDoneAt aCase3

            Assert.AreEqual(aCase3.Type, caseListDtDoneAt[0].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListDtDoneAt[0].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListDtDoneAt[0].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDtDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListDtDoneAt[0].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDtDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListDtDoneAt[0].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListDtDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListDtDoneAt[0].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListDtDoneAt[0].SiteName);
            Assert.AreEqual(aCase3.Status, caseListDtDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListDtDoneAt[0].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListDtDoneAt[0].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListDtDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListDtDoneAt[0].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName, caseListDtDoneAt[0].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListDtDoneAt[0].WorkflowState);

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

            #region Def Sort Des w. DateTime

            #region caseListDtDescendingDoneAt Def Sort Des

            #region caseListDtDescendingDoneAt aCase1

            Assert.NotNull(caseListDtDescendingDoneAt);
            Assert.AreEqual(2, caseListDtDescendingDoneAt.Count);
            Assert.AreEqual(aCase1.Type, caseListDtDescendingDoneAt[0].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListDtDescendingDoneAt[0].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListDtDescendingDoneAt[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDtDescendingDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListDtDescendingDoneAt[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDtDescendingDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListDtDescendingDoneAt[0].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListDtDescendingDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListDtDescendingDoneAt[0].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListDtDescendingDoneAt[0].SiteName);
            Assert.AreEqual(aCase1.Status, caseListDtDescendingDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListDtDescendingDoneAt[0].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListDtDescendingDoneAt[0].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListDtDescendingDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListDtDescendingDoneAt[0].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName,
                caseListDtDescendingDoneAt[0].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListDtDescendingDoneAt aCase4

            Assert.AreEqual(aCase3.Type, caseListDtDescendingDoneAt[1].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListDtDescendingDoneAt[1].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListDtDescendingDoneAt[1].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDtDescendingDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListDtDescendingDoneAt[1].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDtDescendingDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListDtDescendingDoneAt[1].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListDtDescendingDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListDtDescendingDoneAt[1].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListDtDescendingDoneAt[1].SiteName);
            Assert.AreEqual(aCase3.Status, caseListDtDescendingDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListDtDescendingDoneAt[1].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListDtDescendingDoneAt[1].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListDtDescendingDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListDtDescendingDoneAt[1].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName,
                caseListDtDescendingDoneAt[1].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListDtDescendingDoneAt[1].WorkflowState);

            #endregion

            #endregion

            #region caseListDtDescendingStatus Def Sort Des

            #region caseListDtDescendingStatus aCase1

            Assert.NotNull(caseListDtDescendingStatus);
            Assert.AreEqual(2, caseListDtDescendingStatus.Count);
            Assert.AreEqual(aCase1.Type, caseListDtDescendingStatus[1].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListDtDescendingStatus[1].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListDtDescendingStatus[1].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDtDescendingStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListDtDescendingStatus[1].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDtDescendingStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListDtDescendingStatus[1].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListDtDescendingStatus[1].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListDtDescendingStatus[1].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListDtDescendingStatus[1].SiteName);
            Assert.AreEqual(aCase1.Status, caseListDtDescendingStatus[1].Status);
            Assert.AreEqual(aCase1.CheckListId, caseListDtDescendingStatus[1].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListDtDescendingStatus[1].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListDtDescendingStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListDtDescendingStatus[1].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName,
                caseListDtDescendingStatus[1].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListDtDescendingStatus[1].WorkflowState);

            #endregion

            #region caseListDtDescendingStatus aCase3

            Assert.AreEqual(aCase3.Type, caseListDtDescendingStatus[0].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListDtDescendingStatus[0].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListDtDescendingStatus[0].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDtDescendingStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListDtDescendingStatus[0].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDtDescendingStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListDtDescendingStatus[0].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListDtDescendingStatus[0].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListDtDescendingStatus[0].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListDtDescendingStatus[0].SiteName);
            Assert.AreEqual(aCase3.Status, caseListDtDescendingStatus[0].Status);
            Assert.AreEqual(aCase3.CheckListId, caseListDtDescendingStatus[0].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListDtDescendingStatus[0].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListDtDescendingStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListDtDescendingStatus[0].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName,
                caseListDtDescendingStatus[0].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListDtDescendingStatus[0].WorkflowState);

            #endregion

            #endregion

            #region caseListDtDescendingUnitId Def Sort Des

            #region caseListDtDescendingUnitId aCase3

            Assert.NotNull(caseListDtDescendingUnitId);
            Assert.AreEqual(2, caseListDtDescendingUnitId.Count);

            Assert.AreEqual(aCase1.Type, caseListDtDescendingUnitId[1].CaseType);
            Assert.AreEqual(aCase1.CaseUid, caseListDtDescendingUnitId[1].CaseUId);
            Assert.AreEqual(aCase1.MicrotingCheckUid, caseListDtDescendingUnitId[1].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDtDescendingUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase1.Custom, caseListDtDescendingUnitId[1].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDtDescendingUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase1.Id, caseListDtDescendingUnitId[1].Id);
            Assert.AreEqual(aCase1.MicrotingUid, caseListDtDescendingUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase1.Site.MicrotingUid, caseListDtDescendingUnitId[1].SiteId);
            Assert.AreEqual(aCase1.Site.Name, caseListDtDescendingUnitId[1].SiteName);
            Assert.AreEqual(aCase1.Status, caseListDtDescendingUnitId[1].Status);
            Assert.AreEqual(aCase1.CheckListId, caseListDtDescendingUnitId[1].TemplatId);
            Assert.AreEqual(aCase1.Unit.MicrotingUid, caseListDtDescendingUnitId[1].UnitId);
//            Assert.AreEqual(c1_ua.ToString(), caseListDtDescendingUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.Version, caseListDtDescendingUnitId[1].Version);
            Assert.AreEqual(aCase1.Worker.FirstName + " " + aCase1.Worker.LastName,
                caseListDtDescendingUnitId[1].WorkerName);
            Assert.AreEqual(aCase1.WorkflowState, caseListDtDescendingUnitId[1].WorkflowState);

            #endregion

            #region caseListDtDescendingUnitId aCase3

            Assert.AreEqual(aCase3.Type, caseListDtDescendingUnitId[0].CaseType);
            Assert.AreEqual(aCase3.CaseUid, caseListDtDescendingUnitId[0].CaseUId);
            Assert.AreEqual(aCase3.MicrotingCheckUid, caseListDtDescendingUnitId[0].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDtDescendingUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase3.Custom, caseListDtDescendingUnitId[0].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDtDescendingUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase3.Id, caseListDtDescendingUnitId[0].Id);
            Assert.AreEqual(aCase3.MicrotingUid, caseListDtDescendingUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase3.Site.MicrotingUid, caseListDtDescendingUnitId[0].SiteId);
            Assert.AreEqual(aCase3.Site.Name, caseListDtDescendingUnitId[0].SiteName);
            Assert.AreEqual(aCase3.Status, caseListDtDescendingUnitId[0].Status);
            Assert.AreEqual(aCase3.CheckListId, caseListDtDescendingUnitId[0].TemplatId);
            Assert.AreEqual(aCase3.Unit.MicrotingUid, caseListDtDescendingUnitId[0].UnitId);
//            Assert.AreEqual(c3_ua.ToString(), caseListDtDescendingUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.Version, caseListDtDescendingUnitId[0].Version);
            Assert.AreEqual(aCase3.Worker.FirstName + " " + aCase3.Worker.LastName,
                caseListDtDescendingUnitId[0].WorkerName);
            Assert.AreEqual(aCase3.WorkflowState, caseListDtDescendingUnitId[0].WorkflowState);

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

            #region aCase Sort Des

            #region aCase1 Sort Des

            #region caseListC1SortDescendingDoneAt aCase1

            Assert.NotNull(caseListC1SortDescendingDoneAt);
            Assert.AreEqual(0, caseListC1SortDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListC1SortDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListC1SortDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListC2SortDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListC2SortDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListC2SortDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListC3SortDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListC3SortDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListC3SortDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListC4SortDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListC4SortDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListC4SortDescendingUnitId.Count);
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

            #region aCase Sort Des w. DateTime

            #region aCase1 Sort Des w. DateTime

            #region caseListC1SortDtDescendingDoneAt aCase1

            Assert.NotNull(caseListC1SortDtDescendingDoneAt);
            Assert.AreEqual(0, caseListC1SortDtDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListC1SortDtDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListC1SortDtDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListC2SortDtDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListC2SortDtDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListC2SortDtDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListC3SortDtDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListC3SortDtDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListC3SortDtDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListC4SortDtDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListC4SortDtDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListC4SortDtDescendingUnitId.Count);
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
            Assert.AreEqual(4, caseListRemovedDoneAt.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedDoneAt[1].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedDoneAt[1].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedDoneAt[1].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedDoneAt[1].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedDoneAt[1].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedDoneAt[1].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedDoneAt[1].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDoneAt[1].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedDoneAt[1].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedDoneAt[1].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedDoneAt[1].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedDoneAt[1].WorkflowState);

            #endregion

            #region caseListRemovedDoneAt aCase2Removed

            Assert.AreEqual(aCase2Removed.Type, caseListRemovedDoneAt[3].CaseType);
            Assert.AreEqual(aCase2Removed.CaseUid, caseListRemovedDoneAt[3].CaseUId);
            Assert.AreEqual(aCase2Removed.MicrotingCheckUid, caseListRemovedDoneAt[3].CheckUIid);
            Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedDoneAt[3].CreatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Custom, caseListRemovedDoneAt[3].Custom);
            Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedDoneAt[3].DoneAt.ToString());
            Assert.AreEqual(aCase2Removed.Id, caseListRemovedDoneAt[3].Id);
            Assert.AreEqual(aCase2Removed.MicrotingUid, caseListRemovedDoneAt[3].MicrotingUId);
            Assert.AreEqual(aCase2Removed.Site.MicrotingUid, caseListRemovedDoneAt[3].SiteId);
            Assert.AreEqual(aCase2Removed.Site.Name, caseListRemovedDoneAt[3].SiteName);
            Assert.AreEqual(aCase2Removed.Status, caseListRemovedDoneAt[3].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDoneAt[3].TemplatId);
            Assert.AreEqual(aCase2Removed.Unit.MicrotingUid, caseListRemovedDoneAt[3].UnitId);
//            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedDoneAt[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Version, caseListRemovedDoneAt[3].Version);
            Assert.AreEqual(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName,
                caseListRemovedDoneAt[3].WorkerName);
            Assert.AreEqual(aCase2Removed.WorkflowState, caseListRemovedDoneAt[3].WorkflowState);

            #endregion

            #region caseListRemovedDoneAt aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedDoneAt[0].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedDoneAt[0].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedDoneAt[0].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedDoneAt[0].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedDoneAt[0].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedDoneAt[0].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedDoneAt[0].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDoneAt[0].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedDoneAt[0].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedDoneAt[0].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedDoneAt[0].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedDoneAt aCase4Removed

            Assert.AreEqual(aCase4Removed.Type, caseListRemovedDoneAt[2].CaseType);
            Assert.AreEqual(aCase4Removed.CaseUid, caseListRemovedDoneAt[2].CaseUId);
            Assert.AreEqual(aCase4Removed.MicrotingCheckUid, caseListRemovedDoneAt[2].CheckUIid);
            Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedDoneAt[2].CreatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Custom, caseListRemovedDoneAt[2].Custom);
            Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedDoneAt[2].DoneAt.ToString());
            Assert.AreEqual(aCase4Removed.Id, caseListRemovedDoneAt[2].Id);
            Assert.AreEqual(aCase4Removed.MicrotingUid, caseListRemovedDoneAt[2].MicrotingUId);
            Assert.AreEqual(aCase4Removed.Site.MicrotingUid, caseListRemovedDoneAt[2].SiteId);
            Assert.AreEqual(aCase4Removed.Site.Name, caseListRemovedDoneAt[2].SiteName);
            Assert.AreEqual(aCase4Removed.Status, caseListRemovedDoneAt[2].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDoneAt[2].TemplatId);
            Assert.AreEqual(aCase4Removed.Unit.MicrotingUid, caseListRemovedDoneAt[2].UnitId);
//            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedDoneAt[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Version, caseListRemovedDoneAt[2].Version);
            Assert.AreEqual(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName,
                caseListRemovedDoneAt[2].WorkerName);
            Assert.AreEqual(aCase4Removed.WorkflowState, caseListRemovedDoneAt[2].WorkflowState);

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

            #region Def Sort Des

            #region caseListRemovedDescendingDoneAt Def Sort Des

            #region caseListRemovedDescendingDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedDescendingDoneAt);
            Assert.AreEqual(4, caseListRemovedDescendingDoneAt.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedDescendingDoneAt[2].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedDescendingDoneAt[2].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedDescendingDoneAt[2].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedDescendingDoneAt[2].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedDescendingDoneAt[2].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedDescendingDoneAt[2].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedDescendingDoneAt[2].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedDescendingDoneAt[2].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedDescendingDoneAt[2].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedDescendingDoneAt[2].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedDescendingDoneAt[2].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDescendingDoneAt[2].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedDescendingDoneAt[2].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDescendingDoneAt[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedDescendingDoneAt[2].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedDescendingDoneAt[2].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedDescendingDoneAt[2].WorkflowState);

            #endregion

            #region caseListRemovedDescendingDoneAt aCase2Removed

            Assert.AreEqual(aCase2Removed.Type, caseListRemovedDescendingDoneAt[0].CaseType);
            Assert.AreEqual(aCase2Removed.CaseUid, caseListRemovedDescendingDoneAt[0].CaseUId);
            Assert.AreEqual(aCase2Removed.MicrotingCheckUid, caseListRemovedDescendingDoneAt[0].CheckUIid);
            Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedDescendingDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Custom, caseListRemovedDescendingDoneAt[0].Custom);
            Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedDescendingDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase2Removed.Id, caseListRemovedDescendingDoneAt[0].Id);
            Assert.AreEqual(aCase2Removed.MicrotingUid, caseListRemovedDescendingDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase2Removed.Site.MicrotingUid, caseListRemovedDescendingDoneAt[0].SiteId);
            Assert.AreEqual(aCase2Removed.Site.Name, caseListRemovedDescendingDoneAt[0].SiteName);
            Assert.AreEqual(aCase2Removed.Status, caseListRemovedDescendingDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDescendingDoneAt[0].TemplatId);
            Assert.AreEqual(aCase2Removed.Unit.MicrotingUid, caseListRemovedDescendingDoneAt[0].UnitId);
//            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedDescendingDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Version, caseListRemovedDescendingDoneAt[0].Version);
            Assert.AreEqual(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName,
                caseListRemovedDescendingDoneAt[0].WorkerName);
            Assert.AreEqual(aCase2Removed.WorkflowState, caseListRemovedDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedDescendingDoneAt aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedDescendingDoneAt[3].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedDescendingDoneAt[3].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedDescendingDoneAt[3].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedDescendingDoneAt[3].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedDescendingDoneAt[3].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedDescendingDoneAt[3].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedDescendingDoneAt[3].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedDescendingDoneAt[3].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedDescendingDoneAt[3].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedDescendingDoneAt[3].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedDescendingDoneAt[3].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDescendingDoneAt[3].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedDescendingDoneAt[3].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDescendingDoneAt[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedDescendingDoneAt[3].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedDescendingDoneAt[3].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedDescendingDoneAt[3].WorkflowState);

            #endregion

            #region caseListRemovedDescendingDoneAt aCase4Removed

            Assert.AreEqual(aCase4Removed.Type, caseListRemovedDescendingDoneAt[1].CaseType);
            Assert.AreEqual(aCase4Removed.CaseUid, caseListRemovedDescendingDoneAt[1].CaseUId);
            Assert.AreEqual(aCase4Removed.MicrotingCheckUid, caseListRemovedDescendingDoneAt[1].CheckUIid);
            Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedDescendingDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Custom, caseListRemovedDescendingDoneAt[1].Custom);
            Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedDescendingDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase4Removed.Id, caseListRemovedDescendingDoneAt[1].Id);
            Assert.AreEqual(aCase4Removed.MicrotingUid, caseListRemovedDescendingDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase4Removed.Site.MicrotingUid, caseListRemovedDescendingDoneAt[1].SiteId);
            Assert.AreEqual(aCase4Removed.Site.Name, caseListRemovedDescendingDoneAt[1].SiteName);
            Assert.AreEqual(aCase4Removed.Status, caseListRemovedDescendingDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDescendingDoneAt[1].TemplatId);
            Assert.AreEqual(aCase4Removed.Unit.MicrotingUid, caseListRemovedDescendingDoneAt[1].UnitId);
//            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedDescendingDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Version, caseListRemovedDescendingDoneAt[1].Version);
            Assert.AreEqual(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName,
                caseListRemovedDescendingDoneAt[1].WorkerName);
            Assert.AreEqual(aCase4Removed.WorkflowState, caseListRemovedDescendingDoneAt[1].WorkflowState);

            #endregion

            #endregion

            #region caseListRemovedDescendingStatus Def Sort Des

            #region caseListRemovedDescendingStatus aCase1Removed

            Assert.NotNull(caseListRemovedDescendingStatus);
            Assert.AreEqual(4, caseListRemovedDescendingStatus.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedDescendingStatus[3].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedDescendingStatus[3].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedDescendingStatus[3].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedDescendingStatus[3].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedDescendingStatus[3].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedDescendingStatus[3].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedDescendingStatus[3].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedDescendingStatus[3].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedDescendingStatus[3].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedDescendingStatus[3].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedDescendingStatus[3].Status);
            Assert.AreEqual(aCase1Removed.CheckListId, caseListRemovedDescendingStatus[3].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedDescendingStatus[3].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDescendingStatus[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedDescendingStatus[3].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedDescendingStatus[3].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedDescendingStatus[3].WorkflowState);

            #endregion

            #region caseListRemovedDescendingStatus aCase2Removed

            Assert.AreEqual(aCase2Removed.Type, caseListRemovedDescendingStatus[2].CaseType);
            Assert.AreEqual(aCase2Removed.CaseUid, caseListRemovedDescendingStatus[2].CaseUId);
            Assert.AreEqual(aCase2Removed.MicrotingCheckUid, caseListRemovedDescendingStatus[2].CheckUIid);
            Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedDescendingStatus[2].CreatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Custom, caseListRemovedDescendingStatus[2].Custom);
            Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedDescendingStatus[2].DoneAt.ToString());
            Assert.AreEqual(aCase2Removed.Id, caseListRemovedDescendingStatus[2].Id);
            Assert.AreEqual(aCase2Removed.MicrotingUid, caseListRemovedDescendingStatus[2].MicrotingUId);
            Assert.AreEqual(aCase2Removed.Site.MicrotingUid, caseListRemovedDescendingStatus[2].SiteId);
            Assert.AreEqual(aCase2Removed.Site.Name, caseListRemovedDescendingStatus[2].SiteName);
            Assert.AreEqual(aCase2Removed.Status, caseListRemovedDescendingStatus[2].Status);
            Assert.AreEqual(aCase2Removed.CheckListId, caseListRemovedDescendingStatus[2].TemplatId);
            Assert.AreEqual(aCase2Removed.Unit.MicrotingUid, caseListRemovedDescendingStatus[2].UnitId);
//            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedDescendingStatus[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Version, caseListRemovedDescendingStatus[2].Version);
            Assert.AreEqual(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName,
                caseListRemovedDescendingStatus[2].WorkerName);
            Assert.AreEqual(aCase2Removed.WorkflowState, caseListRemovedDescendingStatus[2].WorkflowState);

            #endregion

            #region caseListRemovedDescendingStatus aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedDescendingStatus[1].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedDescendingStatus[1].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedDescendingStatus[1].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedDescendingStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedDescendingStatus[1].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedDescendingStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedDescendingStatus[1].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedDescendingStatus[1].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedDescendingStatus[1].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedDescendingStatus[1].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedDescendingStatus[1].Status);
            Assert.AreEqual(aCase3Removed.CheckListId, caseListRemovedDescendingStatus[1].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedDescendingStatus[1].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDescendingStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedDescendingStatus[1].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedDescendingStatus[1].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedDescendingStatus[1].WorkflowState);

            #endregion

            #region caseListRemovedDescendingStatus aCase4Removed

            Assert.AreEqual(aCase4Removed.Type, caseListRemovedDescendingStatus[0].CaseType);
            Assert.AreEqual(aCase4Removed.CaseUid, caseListRemovedDescendingStatus[0].CaseUId);
            Assert.AreEqual(aCase4Removed.MicrotingCheckUid, caseListRemovedDescendingStatus[0].CheckUIid);
            Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedDescendingStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Custom, caseListRemovedDescendingStatus[0].Custom);
            Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedDescendingStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase4Removed.Id, caseListRemovedDescendingStatus[0].Id);
            Assert.AreEqual(aCase4Removed.MicrotingUid, caseListRemovedDescendingStatus[0].MicrotingUId);
            Assert.AreEqual(aCase4Removed.Site.MicrotingUid, caseListRemovedDescendingStatus[0].SiteId);
            Assert.AreEqual(aCase4Removed.Site.Name, caseListRemovedDescendingStatus[0].SiteName);
            Assert.AreEqual(aCase4Removed.Status, caseListRemovedDescendingStatus[0].Status);
            Assert.AreEqual(aCase4Removed.CheckListId, caseListRemovedDescendingStatus[0].TemplatId);
            Assert.AreEqual(aCase4Removed.Unit.MicrotingUid, caseListRemovedDescendingStatus[0].UnitId);
//            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedDescendingStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Version, caseListRemovedDescendingStatus[0].Version);
            Assert.AreEqual(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName,
                caseListRemovedDescendingStatus[0].WorkerName);
            Assert.AreEqual(aCase4Removed.WorkflowState, caseListRemovedDescendingStatus[0].WorkflowState);

            #endregion

            #endregion

            #region caseListRemovedDescendingUnitId Def Sort Des

            #region caseListRemovedDescendingUnitId aCase1Removed

            Assert.NotNull(caseListRemovedDescendingUnitId);
            Assert.AreEqual(4, caseListRemovedDescendingUnitId.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedDescendingUnitId[3].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedDescendingUnitId[3].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedDescendingUnitId[3].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedDescendingUnitId[3].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedDescendingUnitId[3].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedDescendingUnitId[3].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedDescendingUnitId[3].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedDescendingUnitId[3].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedDescendingUnitId[3].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedDescendingUnitId[3].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedDescendingUnitId[3].Status);
            Assert.AreEqual(aCase1Removed.CheckListId, caseListRemovedDescendingUnitId[3].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedDescendingUnitId[3].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDescendingUnitId[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedDescendingUnitId[3].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedDescendingUnitId[3].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedDescendingUnitId[3].WorkflowState);

            #endregion

            #region caseListRemovedDescendingUnitId aCase2Removed

            Assert.AreEqual(aCase2Removed.Type, caseListRemovedDescendingUnitId[2].CaseType);
            Assert.AreEqual(aCase2Removed.CaseUid, caseListRemovedDescendingUnitId[2].CaseUId);
            Assert.AreEqual(aCase2Removed.MicrotingCheckUid, caseListRemovedDescendingUnitId[2].CheckUIid);
            Assert.AreEqual(c2Removed_ca.ToString(), caseListRemovedDescendingUnitId[2].CreatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Custom, caseListRemovedDescendingUnitId[2].Custom);
            Assert.AreEqual(c2Removed_da.ToString(), caseListRemovedDescendingUnitId[2].DoneAt.ToString());
            Assert.AreEqual(aCase2Removed.Id, caseListRemovedDescendingUnitId[2].Id);
            Assert.AreEqual(aCase2Removed.MicrotingUid, caseListRemovedDescendingUnitId[2].MicrotingUId);
            Assert.AreEqual(aCase2Removed.Site.MicrotingUid, caseListRemovedDescendingUnitId[2].SiteId);
            Assert.AreEqual(aCase2Removed.Site.Name, caseListRemovedDescendingUnitId[2].SiteName);
            Assert.AreEqual(aCase2Removed.Status, caseListRemovedDescendingUnitId[2].Status);
            Assert.AreEqual(aCase2Removed.CheckListId, caseListRemovedDescendingUnitId[2].TemplatId);
            Assert.AreEqual(aCase2Removed.Unit.MicrotingUid, caseListRemovedDescendingUnitId[2].UnitId);
//            Assert.AreEqual(c2Removed_ua.ToString(), caseListRemovedDescendingUnitId[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Removed.Version, caseListRemovedDescendingUnitId[2].Version);
            Assert.AreEqual(aCase2Removed.Worker.FirstName + " " + aCase2Removed.Worker.LastName,
                caseListRemovedDescendingUnitId[2].WorkerName);
            Assert.AreEqual(aCase2Removed.WorkflowState, caseListRemovedDescendingUnitId[2].WorkflowState);

            #endregion

            #region caseListRemovedDescendingUnitId aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedDescendingUnitId[1].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedDescendingUnitId[1].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedDescendingUnitId[1].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedDescendingUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedDescendingUnitId[1].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedDescendingUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedDescendingUnitId[1].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedDescendingUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedDescendingUnitId[1].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedDescendingUnitId[1].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedDescendingUnitId[1].Status);
            Assert.AreEqual(aCase3Removed.CheckListId, caseListRemovedDescendingUnitId[1].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedDescendingUnitId[1].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDescendingUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedDescendingUnitId[1].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedDescendingUnitId[1].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedDescendingUnitId[1].WorkflowState);

            #endregion

            #region caseListRemovedDescendingUnitId aCase4Removed

            Assert.AreEqual(aCase4Removed.Type, caseListRemovedDescendingUnitId[0].CaseType);
            Assert.AreEqual(aCase4Removed.CaseUid, caseListRemovedDescendingUnitId[0].CaseUId);
            Assert.AreEqual(aCase4Removed.MicrotingCheckUid, caseListRemovedDescendingUnitId[0].CheckUIid);
            Assert.AreEqual(c4Removed_ca.ToString(), caseListRemovedDescendingUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Custom, caseListRemovedDescendingUnitId[0].Custom);
            Assert.AreEqual(c4Removed_da.ToString(), caseListRemovedDescendingUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase4Removed.Id, caseListRemovedDescendingUnitId[0].Id);
            Assert.AreEqual(aCase4Removed.MicrotingUid, caseListRemovedDescendingUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase4Removed.Site.MicrotingUid, caseListRemovedDescendingUnitId[0].SiteId);
            Assert.AreEqual(aCase4Removed.Site.Name, caseListRemovedDescendingUnitId[0].SiteName);
            Assert.AreEqual(aCase4Removed.Status, caseListRemovedDescendingUnitId[0].Status);
            Assert.AreEqual(aCase4Removed.CheckListId, caseListRemovedDescendingUnitId[0].TemplatId);
            Assert.AreEqual(aCase4Removed.Unit.MicrotingUid, caseListRemovedDescendingUnitId[0].UnitId);
//            Assert.AreEqual(c4Removed_ua.ToString(), caseListRemovedDescendingUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Removed.Version, caseListRemovedDescendingUnitId[0].Version);
            Assert.AreEqual(aCase4Removed.Worker.FirstName + " " + aCase4Removed.Worker.LastName,
                caseListRemovedDescendingUnitId[0].WorkerName);
            Assert.AreEqual(aCase4Removed.WorkflowState, caseListRemovedDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #endregion

            #region Def Sort Asc w. DateTime

            #region caseListRemovedDtDoneAt Def Sort Asc w. DateTime

            #region caseListRemovedDtDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedDtDoneAt);
            Assert.AreEqual(2, caseListRemovedDtDoneAt.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedDtDoneAt[1].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedDtDoneAt[1].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedDtDoneAt[1].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedDtDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedDtDoneAt[1].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedDtDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedDtDoneAt[1].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedDtDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedDtDoneAt[1].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedDtDoneAt[1].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedDtDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDtDoneAt[1].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedDtDoneAt[1].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedDtDoneAt[1].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedDtDoneAt[1].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedDtDoneAt[1].WorkflowState);

            #endregion

            #region caseListRemovedDtDoneAt aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedDtDoneAt[0].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedDtDoneAt[0].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedDtDoneAt[0].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedDtDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedDtDoneAt[0].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedDtDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedDtDoneAt[0].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedDtDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedDtDoneAt[0].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedDtDoneAt[0].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedDtDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDtDoneAt[0].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedDtDoneAt[0].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedDtDoneAt[0].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedDtDoneAt[0].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedDtDoneAt[0].WorkflowState);

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

            #region Def Sort Des w. DateTime

            #region caseListRemovedDtDescendingDoneAt Def Sort Des

            #region caseListRemovedDtDescendingDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedDtDescendingDoneAt);
            Assert.AreEqual(2, caseListRemovedDtDescendingDoneAt.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedDtDescendingDoneAt[0].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedDtDescendingDoneAt[0].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedDtDescendingDoneAt[0].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedDtDescendingDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedDtDescendingDoneAt[0].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedDtDescendingDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedDtDescendingDoneAt[0].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedDtDescendingDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedDtDescendingDoneAt[0].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedDtDescendingDoneAt[0].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedDtDescendingDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDtDescendingDoneAt[0].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedDtDescendingDoneAt[0].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtDescendingDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedDtDescendingDoneAt[0].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedDtDescendingDoneAt[0].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRemovedDtDescendingDoneAt aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedDtDescendingDoneAt[1].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedDtDescendingDoneAt[1].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedDtDescendingDoneAt[1].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedDtDescendingDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedDtDescendingDoneAt[1].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedDtDescendingDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedDtDescendingDoneAt[1].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedDtDescendingDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedDtDescendingDoneAt[1].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedDtDescendingDoneAt[1].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedDtDescendingDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListRemovedDtDescendingDoneAt[1].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedDtDescendingDoneAt[1].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtDescendingDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedDtDescendingDoneAt[1].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedDtDescendingDoneAt[1].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedDtDescendingDoneAt[1].WorkflowState);

            #endregion

            #endregion

            #region caseListRemovedDtDescendingStatus Def Sort Des

            #region caseListRemovedDtDescendingStatus aCase1Removed

            Assert.NotNull(caseListRemovedDtDescendingStatus);
            Assert.AreEqual(2, caseListRemovedDtDescendingStatus.Count);
            Assert.AreEqual(aCase1Removed.Type, caseListRemovedDtDescendingStatus[1].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedDtDescendingStatus[1].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedDtDescendingStatus[1].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedDtDescendingStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedDtDescendingStatus[1].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedDtDescendingStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedDtDescendingStatus[1].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedDtDescendingStatus[1].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedDtDescendingStatus[1].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedDtDescendingStatus[1].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedDtDescendingStatus[1].Status);
            Assert.AreEqual(aCase1Removed.CheckListId, caseListRemovedDtDescendingStatus[1].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedDtDescendingStatus[1].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtDescendingStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedDtDescendingStatus[1].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedDtDescendingStatus[1].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedDtDescendingStatus[1].WorkflowState);

            #endregion


            #region caseListRemovedDtDescendingStatus aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedDtDescendingStatus[0].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedDtDescendingStatus[0].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedDtDescendingStatus[0].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedDtDescendingStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedDtDescendingStatus[0].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedDtDescendingStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedDtDescendingStatus[0].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedDtDescendingStatus[0].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedDtDescendingStatus[0].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedDtDescendingStatus[0].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedDtDescendingStatus[0].Status);
            Assert.AreEqual(aCase3Removed.CheckListId, caseListRemovedDtDescendingStatus[0].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedDtDescendingStatus[0].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtDescendingStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedDtDescendingStatus[0].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedDtDescendingStatus[0].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedDtDescendingStatus[0].WorkflowState);

            #endregion

            #endregion

            #region caseListRemovedDtDescendingUnitId Def Sort Des

            #region caseListRemovedDtDescendingUnitId aCase1Removed

            Assert.NotNull(caseListRemovedDtDescendingUnitId);
            Assert.AreEqual(2, caseListRemovedDtDescendingUnitId.Count);

            Assert.AreEqual(aCase1Removed.Type, caseListRemovedDtDescendingUnitId[1].CaseType);
            Assert.AreEqual(aCase1Removed.CaseUid, caseListRemovedDtDescendingUnitId[1].CaseUId);
            Assert.AreEqual(aCase1Removed.MicrotingCheckUid, caseListRemovedDtDescendingUnitId[1].CheckUIid);
            Assert.AreEqual(c1Removed_ca.ToString(), caseListRemovedDtDescendingUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Custom, caseListRemovedDtDescendingUnitId[1].Custom);
            Assert.AreEqual(c1Removed_da.ToString(), caseListRemovedDtDescendingUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase1Removed.Id, caseListRemovedDtDescendingUnitId[1].Id);
            Assert.AreEqual(aCase1Removed.MicrotingUid, caseListRemovedDtDescendingUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase1Removed.Site.MicrotingUid, caseListRemovedDtDescendingUnitId[1].SiteId);
            Assert.AreEqual(aCase1Removed.Site.Name, caseListRemovedDtDescendingUnitId[1].SiteName);
            Assert.AreEqual(aCase1Removed.Status, caseListRemovedDtDescendingUnitId[1].Status);
            Assert.AreEqual(aCase1Removed.CheckListId, caseListRemovedDtDescendingUnitId[1].TemplatId);
            Assert.AreEqual(aCase1Removed.Unit.MicrotingUid, caseListRemovedDtDescendingUnitId[1].UnitId);
//            Assert.AreEqual(c1Removed_ua.ToString(), caseListRemovedDtDescendingUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Removed.Version, caseListRemovedDtDescendingUnitId[1].Version);
            Assert.AreEqual(aCase1Removed.Worker.FirstName + " " + aCase1Removed.Worker.LastName,
                caseListRemovedDtDescendingUnitId[1].WorkerName);
            Assert.AreEqual(aCase1Removed.WorkflowState, caseListRemovedDtDescendingUnitId[1].WorkflowState);

            #endregion


            #region caseListRemovedDtDescendingUnitId aCase3Removed

            Assert.AreEqual(aCase3Removed.Type, caseListRemovedDtDescendingUnitId[0].CaseType);
            Assert.AreEqual(aCase3Removed.CaseUid, caseListRemovedDtDescendingUnitId[0].CaseUId);
            Assert.AreEqual(aCase3Removed.MicrotingCheckUid, caseListRemovedDtDescendingUnitId[0].CheckUIid);
            Assert.AreEqual(c3Removed_ca.ToString(), caseListRemovedDtDescendingUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Custom, caseListRemovedDtDescendingUnitId[0].Custom);
            Assert.AreEqual(c3Removed_da.ToString(), caseListRemovedDtDescendingUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase3Removed.Id, caseListRemovedDtDescendingUnitId[0].Id);
            Assert.AreEqual(aCase3Removed.MicrotingUid, caseListRemovedDtDescendingUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase3Removed.Site.MicrotingUid, caseListRemovedDtDescendingUnitId[0].SiteId);
            Assert.AreEqual(aCase3Removed.Site.Name, caseListRemovedDtDescendingUnitId[0].SiteName);
            Assert.AreEqual(aCase3Removed.Status, caseListRemovedDtDescendingUnitId[0].Status);
            Assert.AreEqual(aCase3Removed.CheckListId, caseListRemovedDtDescendingUnitId[0].TemplatId);
            Assert.AreEqual(aCase3Removed.Unit.MicrotingUid, caseListRemovedDtDescendingUnitId[0].UnitId);
//            Assert.AreEqual(c3Removed_ua.ToString(), caseListRemovedDtDescendingUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Removed.Version, caseListRemovedDtDescendingUnitId[0].Version);
            Assert.AreEqual(aCase3Removed.Worker.FirstName + " " + aCase3Removed.Worker.LastName,
                caseListRemovedDtDescendingUnitId[0].WorkerName);
            Assert.AreEqual(aCase3Removed.WorkflowState, caseListRemovedDtDescendingUnitId[0].WorkflowState);

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

            #region aCase Sort Des

            #region aCase1Removed Sort Des

            #region caseListRemovedC1SortDescendingDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDescendingDoneAt);
            Assert.AreEqual(0, caseListRemovedC1SortDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRemovedC1SortDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRemovedC1SortDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListRemovedC2SortDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRemovedC2SortDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRemovedC2SortDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListRemovedC3SortDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRemovedC3SortDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRemovedC3SortDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListRemovedC4SortDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRemovedC4SortDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRemovedC4SortDescendingUnitId.Count);
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

            #region aCase Sort Des w. DateTime

            #region aCase1Removed Sort Des w. DateTime

            #region caseListRemovedC1SortDtDescendingDoneAt aCase1Removed

            Assert.NotNull(caseListRemovedC1SortDtDescendingDoneAt);
            Assert.AreEqual(0, caseListRemovedC1SortDtDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRemovedC1SortDtDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRemovedC1SortDtDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListRemovedC2SortDtDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRemovedC2SortDtDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRemovedC2SortDtDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListRemovedC3SortDtDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRemovedC3SortDtDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRemovedC3SortDtDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListRemovedC4SortDtDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRemovedC4SortDtDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRemovedC4SortDtDescendingUnitId.Count);
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
            Assert.AreEqual(4, caseListRetractedDoneAt.Count);
            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedDoneAt[1].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedDoneAt[1].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedDoneAt[1].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedDoneAt[1].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedDoneAt[1].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedDoneAt[1].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedDoneAt[1].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDoneAt[1].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedDoneAt[1].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedDoneAt[1].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedDoneAt[1].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedDoneAt[1].WorkflowState);

            #endregion

            #region caseListRetractedDoneAt aCase2Retracted

            Assert.AreEqual(aCase2Retracted.Type, caseListRetractedDoneAt[3].CaseType);
            Assert.AreEqual(aCase2Retracted.CaseUid, caseListRetractedDoneAt[3].CaseUId);
            Assert.AreEqual(aCase2Retracted.MicrotingCheckUid, caseListRetractedDoneAt[3].CheckUIid);
            Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedDoneAt[3].CreatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Custom, caseListRetractedDoneAt[3].Custom);
            Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedDoneAt[3].DoneAt.ToString());
            Assert.AreEqual(aCase2Retracted.Id, caseListRetractedDoneAt[3].Id);
            Assert.AreEqual(aCase2Retracted.MicrotingUid, caseListRetractedDoneAt[3].MicrotingUId);
            Assert.AreEqual(aCase2Retracted.Site.MicrotingUid, caseListRetractedDoneAt[3].SiteId);
            Assert.AreEqual(aCase2Retracted.Site.Name, caseListRetractedDoneAt[3].SiteName);
            Assert.AreEqual(aCase2Retracted.Status, caseListRetractedDoneAt[3].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDoneAt[3].TemplatId);
            Assert.AreEqual(aCase2Retracted.Unit.MicrotingUid, caseListRetractedDoneAt[3].UnitId);
//            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedDoneAt[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Version, caseListRetractedDoneAt[3].Version);
            Assert.AreEqual(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName,
                caseListRetractedDoneAt[3].WorkerName);
            Assert.AreEqual(aCase2Retracted.WorkflowState, caseListRetractedDoneAt[3].WorkflowState);

            #endregion

            #region caseListRetractedDoneAt aCase3Retracted

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedDoneAt[0].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedDoneAt[0].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedDoneAt[0].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedDoneAt[0].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedDoneAt[0].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedDoneAt[0].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedDoneAt[0].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDoneAt[0].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedDoneAt[0].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedDoneAt[0].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedDoneAt[0].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedDoneAt aCase4Retracted

            Assert.AreEqual(aCase4Retracted.Type, caseListRetractedDoneAt[2].CaseType);
            Assert.AreEqual(aCase4Retracted.CaseUid, caseListRetractedDoneAt[2].CaseUId);
            Assert.AreEqual(aCase4Retracted.MicrotingCheckUid, caseListRetractedDoneAt[2].CheckUIid);
            Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedDoneAt[2].CreatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Custom, caseListRetractedDoneAt[2].Custom);
            Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedDoneAt[2].DoneAt.ToString());
            Assert.AreEqual(aCase4Retracted.Id, caseListRetractedDoneAt[2].Id);
            Assert.AreEqual(aCase4Retracted.MicrotingUid, caseListRetractedDoneAt[2].MicrotingUId);
            Assert.AreEqual(aCase4Retracted.Site.MicrotingUid, caseListRetractedDoneAt[2].SiteId);
            Assert.AreEqual(aCase4Retracted.Site.Name, caseListRetractedDoneAt[2].SiteName);
            Assert.AreEqual(aCase4Retracted.Status, caseListRetractedDoneAt[2].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDoneAt[2].TemplatId);
            Assert.AreEqual(aCase4Retracted.Unit.MicrotingUid, caseListRetractedDoneAt[2].UnitId);
//            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedDoneAt[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Version, caseListRetractedDoneAt[2].Version);
            Assert.AreEqual(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName,
                caseListRetractedDoneAt[2].WorkerName);
            Assert.AreEqual(aCase4Retracted.WorkflowState, caseListRetractedDoneAt[2].WorkflowState);

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

            #region Def Sort Des

            #region caseListRetractedDescendingDoneAt Def Sort Des

            #region caseListRetractedDescendingDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedDescendingDoneAt);
            Assert.AreEqual(4, caseListRetractedDescendingDoneAt.Count);
            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedDescendingDoneAt[2].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedDescendingDoneAt[2].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedDescendingDoneAt[2].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedDescendingDoneAt[2].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedDescendingDoneAt[2].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedDescendingDoneAt[2].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedDescendingDoneAt[2].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedDescendingDoneAt[2].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedDescendingDoneAt[2].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedDescendingDoneAt[2].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedDescendingDoneAt[2].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDescendingDoneAt[2].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedDescendingDoneAt[2].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDescendingDoneAt[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedDescendingDoneAt[2].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedDescendingDoneAt[2].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedDescendingDoneAt[2].WorkflowState);

            #endregion

            #region caseListRetractedDescendingDoneAt aCase2Retracted

            Assert.AreEqual(aCase2Retracted.Type, caseListRetractedDescendingDoneAt[0].CaseType);
            Assert.AreEqual(aCase2Retracted.CaseUid, caseListRetractedDescendingDoneAt[0].CaseUId);
            Assert.AreEqual(aCase2Retracted.MicrotingCheckUid, caseListRetractedDescendingDoneAt[0].CheckUIid);
            Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedDescendingDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Custom, caseListRetractedDescendingDoneAt[0].Custom);
            Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedDescendingDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase2Retracted.Id, caseListRetractedDescendingDoneAt[0].Id);
            Assert.AreEqual(aCase2Retracted.MicrotingUid, caseListRetractedDescendingDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase2Retracted.Site.MicrotingUid, caseListRetractedDescendingDoneAt[0].SiteId);
            Assert.AreEqual(aCase2Retracted.Site.Name, caseListRetractedDescendingDoneAt[0].SiteName);
            Assert.AreEqual(aCase2Retracted.Status, caseListRetractedDescendingDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDescendingDoneAt[0].TemplatId);
            Assert.AreEqual(aCase2Retracted.Unit.MicrotingUid, caseListRetractedDescendingDoneAt[0].UnitId);
//            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedDescendingDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Version, caseListRetractedDescendingDoneAt[0].Version);
            Assert.AreEqual(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName,
                caseListRetractedDescendingDoneAt[0].WorkerName);
            Assert.AreEqual(aCase2Retracted.WorkflowState, caseListRetractedDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedDescendingDoneAt aCase3Retracted

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedDescendingDoneAt[3].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedDescendingDoneAt[3].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedDescendingDoneAt[3].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedDescendingDoneAt[3].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedDescendingDoneAt[3].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedDescendingDoneAt[3].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedDescendingDoneAt[3].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedDescendingDoneAt[3].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedDescendingDoneAt[3].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedDescendingDoneAt[3].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedDescendingDoneAt[3].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDescendingDoneAt[3].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedDescendingDoneAt[3].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDescendingDoneAt[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedDescendingDoneAt[3].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedDescendingDoneAt[3].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedDescendingDoneAt[3].WorkflowState);

            #endregion

            #region caseListRetractedDescendingDoneAt aCase4Retracted

            Assert.AreEqual(aCase4Retracted.Type, caseListRetractedDescendingDoneAt[1].CaseType);
            Assert.AreEqual(aCase4Retracted.CaseUid, caseListRetractedDescendingDoneAt[1].CaseUId);
            Assert.AreEqual(aCase4Retracted.MicrotingCheckUid, caseListRetractedDescendingDoneAt[1].CheckUIid);
            Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedDescendingDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Custom, caseListRetractedDescendingDoneAt[1].Custom);
            Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedDescendingDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase4Retracted.Id, caseListRetractedDescendingDoneAt[1].Id);
            Assert.AreEqual(aCase4Retracted.MicrotingUid, caseListRetractedDescendingDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase4Retracted.Site.MicrotingUid, caseListRetractedDescendingDoneAt[1].SiteId);
            Assert.AreEqual(aCase4Retracted.Site.Name, caseListRetractedDescendingDoneAt[1].SiteName);
            Assert.AreEqual(aCase4Retracted.Status, caseListRetractedDescendingDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDescendingDoneAt[1].TemplatId);
            Assert.AreEqual(aCase4Retracted.Unit.MicrotingUid, caseListRetractedDescendingDoneAt[1].UnitId);
//            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedDescendingDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Version, caseListRetractedDescendingDoneAt[1].Version);
            Assert.AreEqual(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName,
                caseListRetractedDescendingDoneAt[1].WorkerName);
            Assert.AreEqual(aCase4Retracted.WorkflowState, caseListRetractedDescendingDoneAt[1].WorkflowState);

            #endregion

            #endregion

            #region caseListRetractedDescendingStatus Def Sort Des

            #region caseListRetractedDescendingStatus aCase1Retracted

            Assert.NotNull(caseListRetractedDescendingStatus);
            Assert.AreEqual(4, caseListRetractedDescendingStatus.Count);
            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedDescendingStatus[3].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedDescendingStatus[3].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedDescendingStatus[3].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedDescendingStatus[3].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedDescendingStatus[3].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedDescendingStatus[3].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedDescendingStatus[3].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedDescendingStatus[3].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedDescendingStatus[3].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedDescendingStatus[3].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedDescendingStatus[3].Status);
            Assert.AreEqual(aCase1Retracted.CheckListId, caseListRetractedDescendingStatus[3].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedDescendingStatus[3].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDescendingStatus[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedDescendingStatus[3].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedDescendingStatus[3].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedDescendingStatus[3].WorkflowState);

            #endregion

            #region caseListRetractedDescendingStatus aCase2Retracted

            Assert.AreEqual(aCase2Retracted.Type, caseListRetractedDescendingStatus[2].CaseType);
            Assert.AreEqual(aCase2Retracted.CaseUid, caseListRetractedDescendingStatus[2].CaseUId);
            Assert.AreEqual(aCase2Retracted.MicrotingCheckUid, caseListRetractedDescendingStatus[2].CheckUIid);
            Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedDescendingStatus[2].CreatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Custom, caseListRetractedDescendingStatus[2].Custom);
            Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedDescendingStatus[2].DoneAt.ToString());
            Assert.AreEqual(aCase2Retracted.Id, caseListRetractedDescendingStatus[2].Id);
            Assert.AreEqual(aCase2Retracted.MicrotingUid, caseListRetractedDescendingStatus[2].MicrotingUId);
            Assert.AreEqual(aCase2Retracted.Site.MicrotingUid, caseListRetractedDescendingStatus[2].SiteId);
            Assert.AreEqual(aCase2Retracted.Site.Name, caseListRetractedDescendingStatus[2].SiteName);
            Assert.AreEqual(aCase2Retracted.Status, caseListRetractedDescendingStatus[2].Status);
            Assert.AreEqual(aCase2Retracted.CheckListId, caseListRetractedDescendingStatus[2].TemplatId);
            Assert.AreEqual(aCase2Retracted.Unit.MicrotingUid, caseListRetractedDescendingStatus[2].UnitId);
//            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedDescendingStatus[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Version, caseListRetractedDescendingStatus[2].Version);
            Assert.AreEqual(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName,
                caseListRetractedDescendingStatus[2].WorkerName);
            Assert.AreEqual(aCase2Retracted.WorkflowState, caseListRetractedDescendingStatus[2].WorkflowState);

            #endregion

            #region caseListRetractedDescendingStatus aCase3Retracted

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedDescendingStatus[1].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedDescendingStatus[1].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedDescendingStatus[1].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedDescendingStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedDescendingStatus[1].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedDescendingStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedDescendingStatus[1].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedDescendingStatus[1].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedDescendingStatus[1].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedDescendingStatus[1].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedDescendingStatus[1].Status);
            Assert.AreEqual(aCase3Retracted.CheckListId, caseListRetractedDescendingStatus[1].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedDescendingStatus[1].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDescendingStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedDescendingStatus[1].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedDescendingStatus[1].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedDescendingStatus[1].WorkflowState);

            #endregion

            #region caseListRetractedDescendingStatus aCase4Retracted

            Assert.AreEqual(aCase4Retracted.Type, caseListRetractedDescendingStatus[0].CaseType);
            Assert.AreEqual(aCase4Retracted.CaseUid, caseListRetractedDescendingStatus[0].CaseUId);
            Assert.AreEqual(aCase4Retracted.MicrotingCheckUid, caseListRetractedDescendingStatus[0].CheckUIid);
            Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedDescendingStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Custom, caseListRetractedDescendingStatus[0].Custom);
            Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedDescendingStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase4Retracted.Id, caseListRetractedDescendingStatus[0].Id);
            Assert.AreEqual(aCase4Retracted.MicrotingUid, caseListRetractedDescendingStatus[0].MicrotingUId);
            Assert.AreEqual(aCase4Retracted.Site.MicrotingUid, caseListRetractedDescendingStatus[0].SiteId);
            Assert.AreEqual(aCase4Retracted.Site.Name, caseListRetractedDescendingStatus[0].SiteName);
            Assert.AreEqual(aCase4Retracted.Status, caseListRetractedDescendingStatus[0].Status);
            Assert.AreEqual(aCase4Retracted.CheckListId, caseListRetractedDescendingStatus[0].TemplatId);
            Assert.AreEqual(aCase4Retracted.Unit.MicrotingUid, caseListRetractedDescendingStatus[0].UnitId);
//            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedDescendingStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Version, caseListRetractedDescendingStatus[0].Version);
            Assert.AreEqual(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName,
                caseListRetractedDescendingStatus[0].WorkerName);
            Assert.AreEqual(aCase4Retracted.WorkflowState, caseListRetractedDescendingStatus[0].WorkflowState);

            #endregion

            #endregion

            #region caseListRetractedDescendingUnitId Def Sort Des

            #region caseListRetractedDescendingUnitId aCase1Retracted

            Assert.NotNull(caseListRetractedDescendingUnitId);
            Assert.AreEqual(4, caseListRetractedDescendingUnitId.Count);
            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedDescendingUnitId[3].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedDescendingUnitId[3].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedDescendingUnitId[3].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedDescendingUnitId[3].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedDescendingUnitId[3].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedDescendingUnitId[3].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedDescendingUnitId[3].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedDescendingUnitId[3].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedDescendingUnitId[3].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedDescendingUnitId[3].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedDescendingUnitId[3].Status);
            Assert.AreEqual(aCase1Retracted.CheckListId, caseListRetractedDescendingUnitId[3].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedDescendingUnitId[3].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDescendingUnitId[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedDescendingUnitId[3].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedDescendingUnitId[3].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedDescendingUnitId[3].WorkflowState);

            #endregion

            #region caseListRetractedDescendingUnitId aCase2Retracted

            Assert.AreEqual(aCase2Retracted.Type, caseListRetractedDescendingUnitId[2].CaseType);
            Assert.AreEqual(aCase2Retracted.CaseUid, caseListRetractedDescendingUnitId[2].CaseUId);
            Assert.AreEqual(aCase2Retracted.MicrotingCheckUid, caseListRetractedDescendingUnitId[2].CheckUIid);
            Assert.AreEqual(c2Retracted_ca.ToString(), caseListRetractedDescendingUnitId[2].CreatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Custom, caseListRetractedDescendingUnitId[2].Custom);
            Assert.AreEqual(c2Retracted_da.ToString(), caseListRetractedDescendingUnitId[2].DoneAt.ToString());
            Assert.AreEqual(aCase2Retracted.Id, caseListRetractedDescendingUnitId[2].Id);
            Assert.AreEqual(aCase2Retracted.MicrotingUid, caseListRetractedDescendingUnitId[2].MicrotingUId);
            Assert.AreEqual(aCase2Retracted.Site.MicrotingUid, caseListRetractedDescendingUnitId[2].SiteId);
            Assert.AreEqual(aCase2Retracted.Site.Name, caseListRetractedDescendingUnitId[2].SiteName);
            Assert.AreEqual(aCase2Retracted.Status, caseListRetractedDescendingUnitId[2].Status);
            Assert.AreEqual(aCase2Retracted.CheckListId, caseListRetractedDescendingUnitId[2].TemplatId);
            Assert.AreEqual(aCase2Retracted.Unit.MicrotingUid, caseListRetractedDescendingUnitId[2].UnitId);
//            Assert.AreEqual(c2Retracted_ua.ToString(), caseListRetractedDescendingUnitId[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase2Retracted.Version, caseListRetractedDescendingUnitId[2].Version);
            Assert.AreEqual(aCase2Retracted.Worker.FirstName + " " + aCase2Retracted.Worker.LastName,
                caseListRetractedDescendingUnitId[2].WorkerName);
            Assert.AreEqual(aCase2Retracted.WorkflowState, caseListRetractedDescendingUnitId[2].WorkflowState);

            #endregion

            #region caseListRetractedDescendingUnitId aCase3Retracted

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedDescendingUnitId[1].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedDescendingUnitId[1].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedDescendingUnitId[1].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedDescendingUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedDescendingUnitId[1].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedDescendingUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedDescendingUnitId[1].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedDescendingUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedDescendingUnitId[1].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedDescendingUnitId[1].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedDescendingUnitId[1].Status);
            Assert.AreEqual(aCase3Retracted.CheckListId, caseListRetractedDescendingUnitId[1].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedDescendingUnitId[1].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDescendingUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedDescendingUnitId[1].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedDescendingUnitId[1].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedDescendingUnitId[1].WorkflowState);

            #endregion

            #region caseListRetractedDescendingUnitId aCase4Retracted

            Assert.AreEqual(aCase4Retracted.Type, caseListRetractedDescendingUnitId[0].CaseType);
            Assert.AreEqual(aCase4Retracted.CaseUid, caseListRetractedDescendingUnitId[0].CaseUId);
            Assert.AreEqual(aCase4Retracted.MicrotingCheckUid, caseListRetractedDescendingUnitId[0].CheckUIid);
            Assert.AreEqual(c4Retracted_ca.ToString(), caseListRetractedDescendingUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Custom, caseListRetractedDescendingUnitId[0].Custom);
            Assert.AreEqual(c4Retracted_da.ToString(), caseListRetractedDescendingUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase4Retracted.Id, caseListRetractedDescendingUnitId[0].Id);
            Assert.AreEqual(aCase4Retracted.MicrotingUid, caseListRetractedDescendingUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase4Retracted.Site.MicrotingUid, caseListRetractedDescendingUnitId[0].SiteId);
            Assert.AreEqual(aCase4Retracted.Site.Name, caseListRetractedDescendingUnitId[0].SiteName);
            Assert.AreEqual(aCase4Retracted.Status, caseListRetractedDescendingUnitId[0].Status);
            Assert.AreEqual(aCase4Retracted.CheckListId, caseListRetractedDescendingUnitId[0].TemplatId);
            Assert.AreEqual(aCase4Retracted.Unit.MicrotingUid, caseListRetractedDescendingUnitId[0].UnitId);
//            Assert.AreEqual(c4Retracted_ua.ToString(), caseListRetractedDescendingUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase4Retracted.Version, caseListRetractedDescendingUnitId[0].Version);
            Assert.AreEqual(aCase4Retracted.Worker.FirstName + " " + aCase4Retracted.Worker.LastName,
                caseListRetractedDescendingUnitId[0].WorkerName);
            Assert.AreEqual(aCase4Retracted.WorkflowState, caseListRetractedDescendingUnitId[0].WorkflowState);

            #endregion

            #endregion

            #endregion

            #region Def Sort Asc w. DateTime

            #region caseListRetractedDtDoneAt Def Sort Asc w. DateTime

            #region caseListRetractedDtDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedDtDoneAt);
            Assert.AreEqual(2, caseListRetractedDtDoneAt.Count);
            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedDtDoneAt[1].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedDtDoneAt[1].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedDtDoneAt[1].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedDtDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedDtDoneAt[1].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedDtDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedDtDoneAt[1].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedDtDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedDtDoneAt[1].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedDtDoneAt[1].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedDtDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDtDoneAt[1].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedDtDoneAt[1].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedDtDoneAt[1].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedDtDoneAt[1].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedDtDoneAt[1].WorkflowState);

            #endregion

            #region caseListRetractedDtDoneAt aCase3Retracted

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedDtDoneAt[0].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedDtDoneAt[0].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedDtDoneAt[0].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedDtDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedDtDoneAt[0].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedDtDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedDtDoneAt[0].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedDtDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedDtDoneAt[0].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedDtDoneAt[0].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedDtDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDtDoneAt[0].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedDtDoneAt[0].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedDtDoneAt[0].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedDtDoneAt[0].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedDtDoneAt[0].WorkflowState);

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

            #region Def Sort Des w. DateTime

            #region caseListRetractedDtDescendingDoneAt Def Sort Des

            #region caseListRetractedDtDescendingDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedDtDescendingDoneAt);
            Assert.AreEqual(2, caseListRetractedDtDescendingDoneAt.Count);
            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedDtDescendingDoneAt[0].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedDtDescendingDoneAt[0].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedDtDescendingDoneAt[0].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedDtDescendingDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedDtDescendingDoneAt[0].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedDtDescendingDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedDtDescendingDoneAt[0].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedDtDescendingDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedDtDescendingDoneAt[0].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedDtDescendingDoneAt[0].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedDtDescendingDoneAt[0].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDtDescendingDoneAt[0].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedDtDescendingDoneAt[0].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtDescendingDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedDtDescendingDoneAt[0].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedDtDescendingDoneAt[0].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedDtDescendingDoneAt[0].WorkflowState);

            #endregion

            #region caseListRetractedDtDescendingDoneAt aCase3Retracted

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedDtDescendingDoneAt[1].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedDtDescendingDoneAt[1].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedDtDescendingDoneAt[1].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedDtDescendingDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedDtDescendingDoneAt[1].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedDtDescendingDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedDtDescendingDoneAt[1].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedDtDescendingDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedDtDescendingDoneAt[1].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedDtDescendingDoneAt[1].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedDtDescendingDoneAt[1].Status);
            Assert.AreEqual(cl1.Id, caseListRetractedDtDescendingDoneAt[1].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedDtDescendingDoneAt[1].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtDescendingDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedDtDescendingDoneAt[1].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedDtDescendingDoneAt[1].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedDtDescendingDoneAt[1].WorkflowState);

            #endregion

            #endregion

            #region caseListRetractedDtDescendingStatus Def Sort Des

            #region caseListRetractedDtDescendingStatus aCase1Retracted

            Assert.NotNull(caseListRetractedDtDescendingStatus);
            Assert.AreEqual(2, caseListRetractedDtDescendingStatus.Count);
            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedDtDescendingStatus[0].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedDtDescendingStatus[0].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedDtDescendingStatus[0].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedDtDescendingStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedDtDescendingStatus[0].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedDtDescendingStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedDtDescendingStatus[0].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedDtDescendingStatus[0].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedDtDescendingStatus[0].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedDtDescendingStatus[0].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedDtDescendingStatus[0].Status);
            Assert.AreEqual(aCase3Retracted.CheckListId, caseListRetractedDtDescendingStatus[0].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedDtDescendingStatus[0].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtDescendingStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedDtDescendingStatus[0].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedDtDescendingStatus[0].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedDtDescendingStatus[0].WorkflowState);

            #endregion

            #region caseListRetractedDtDescendingStatus aCase3Retracted

            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedDtDescendingStatus[1].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedDtDescendingStatus[1].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedDtDescendingStatus[1].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedDtDescendingStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedDtDescendingStatus[1].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedDtDescendingStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedDtDescendingStatus[1].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedDtDescendingStatus[1].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedDtDescendingStatus[1].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedDtDescendingStatus[1].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedDtDescendingStatus[1].Status);
            Assert.AreEqual(aCase1Retracted.CheckListId, caseListRetractedDtDescendingStatus[1].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedDtDescendingStatus[1].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtDescendingStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedDtDescendingStatus[1].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedDtDescendingStatus[1].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedDtDescendingStatus[1].WorkflowState);

            #endregion

            #endregion

            #region caseListRetractedDtDescendingUnitId Def Sort Des

            #region caseListRetractedDtDescendingUnitId aCase1Retracted

            Assert.NotNull(caseListRetractedDtDescendingUnitId);
            Assert.AreEqual(2, caseListRetractedDtDescendingUnitId.Count);

            Assert.AreEqual(aCase3Retracted.Type, caseListRetractedDtDescendingUnitId[0].CaseType);
            Assert.AreEqual(aCase3Retracted.CaseUid, caseListRetractedDtDescendingUnitId[0].CaseUId);
            Assert.AreEqual(aCase3Retracted.MicrotingCheckUid, caseListRetractedDtDescendingUnitId[0].CheckUIid);
            Assert.AreEqual(c3Retracted_ca.ToString(), caseListRetractedDtDescendingUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Custom, caseListRetractedDtDescendingUnitId[0].Custom);
            Assert.AreEqual(c3Retracted_da.ToString(), caseListRetractedDtDescendingUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase3Retracted.Id, caseListRetractedDtDescendingUnitId[0].Id);
            Assert.AreEqual(aCase3Retracted.MicrotingUid, caseListRetractedDtDescendingUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase3Retracted.Site.MicrotingUid, caseListRetractedDtDescendingUnitId[0].SiteId);
            Assert.AreEqual(aCase3Retracted.Site.Name, caseListRetractedDtDescendingUnitId[0].SiteName);
            Assert.AreEqual(aCase3Retracted.Status, caseListRetractedDtDescendingUnitId[0].Status);
            Assert.AreEqual(aCase3Retracted.CheckListId, caseListRetractedDtDescendingUnitId[0].TemplatId);
            Assert.AreEqual(aCase3Retracted.Unit.MicrotingUid, caseListRetractedDtDescendingUnitId[0].UnitId);
//            Assert.AreEqual(c3Retracted_ua.ToString(), caseListRetractedDtDescendingUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase3Retracted.Version, caseListRetractedDtDescendingUnitId[0].Version);
            Assert.AreEqual(aCase3Retracted.Worker.FirstName + " " + aCase3Retracted.Worker.LastName,
                caseListRetractedDtDescendingUnitId[0].WorkerName);
            Assert.AreEqual(aCase3Retracted.WorkflowState, caseListRetractedDtDescendingUnitId[0].WorkflowState);

            #endregion

            #region caseListRetractedDtDescendingUnitId aCase3Retracted

            Assert.AreEqual(aCase1Retracted.Type, caseListRetractedDtDescendingUnitId[1].CaseType);
            Assert.AreEqual(aCase1Retracted.CaseUid, caseListRetractedDtDescendingUnitId[1].CaseUId);
            Assert.AreEqual(aCase1Retracted.MicrotingCheckUid, caseListRetractedDtDescendingUnitId[1].CheckUIid);
            Assert.AreEqual(c1Retracted_ca.ToString(), caseListRetractedDtDescendingUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Custom, caseListRetractedDtDescendingUnitId[1].Custom);
            Assert.AreEqual(c1Retracted_da.ToString(), caseListRetractedDtDescendingUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase1Retracted.Id, caseListRetractedDtDescendingUnitId[1].Id);
            Assert.AreEqual(aCase1Retracted.MicrotingUid, caseListRetractedDtDescendingUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase1Retracted.Site.MicrotingUid, caseListRetractedDtDescendingUnitId[1].SiteId);
            Assert.AreEqual(aCase1Retracted.Site.Name, caseListRetractedDtDescendingUnitId[1].SiteName);
            Assert.AreEqual(aCase1Retracted.Status, caseListRetractedDtDescendingUnitId[1].Status);
            Assert.AreEqual(aCase1Retracted.CheckListId, caseListRetractedDtDescendingUnitId[1].TemplatId);
            Assert.AreEqual(aCase1Retracted.Unit.MicrotingUid, caseListRetractedDtDescendingUnitId[1].UnitId);
//            Assert.AreEqual(c1Retracted_ua.ToString(), caseListRetractedDtDescendingUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase1Retracted.Version, caseListRetractedDtDescendingUnitId[1].Version);
            Assert.AreEqual(aCase1Retracted.Worker.FirstName + " " + aCase1Retracted.Worker.LastName,
                caseListRetractedDtDescendingUnitId[1].WorkerName);
            Assert.AreEqual(aCase1Retracted.WorkflowState, caseListRetractedDtDescendingUnitId[1].WorkflowState);

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

            #region aCase Sort Des

            #region aCase1Retracted Sort Des

            #region caseListRetractedC1SortDescendingDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDescendingDoneAt);
            Assert.AreEqual(0, caseListRetractedC1SortDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRetractedC1SortDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRetractedC1SortDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListRetractedC2SortDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRetractedC2SortDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRetractedC2SortDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListRetractedC3SortDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRetractedC3SortDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRetractedC3SortDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListRetractedC4SortDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRetractedC4SortDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRetractedC4SortDescendingUnitId.Count);
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

            #region aCase Sort Des w. DateTime

            #region aCase1Retracted Sort Des w. DateTime

            #region caseListRetractedC1SortDtDescendingDoneAt aCase1Retracted

            Assert.NotNull(caseListRetractedC1SortDtDescendingDoneAt);
            Assert.AreEqual(0, caseListRetractedC1SortDtDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRetractedC1SortDtDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRetractedC1SortDtDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListRetractedC2SortDtDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRetractedC2SortDtDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRetractedC2SortDtDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListRetractedC3SortDtDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRetractedC3SortDtDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRetractedC3SortDtDescendingUnitId.Count);
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
            Assert.AreEqual(0, caseListRetractedC4SortDtDescendingDoneAt.Count);
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
            Assert.AreEqual(0, caseListRetractedC4SortDtDescendingStatus.Count);
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
            Assert.AreEqual(0, caseListRetractedC4SortDtDescendingUnitId.Count);
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