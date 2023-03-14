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
using System.Globalization;
using System.Linq;
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

namespace eFormSDK.Integration.Case.SqlControllerTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SqlControllerTestCase : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
        private DbContextHelper dbContextHelper;
        private Language language;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            dbContextHelper = new DbContextHelper(ConnectionString);
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
            language = DbContext.Languages.Single(x => x.Name == "Danish");
        }

        [Test]
        public async Task SQL_Case_CaseCreate_DoesCaseCreate()
        {
            Random rnd = new Random();
            Site site1 = await testHelpers.CreateSite("MySite", 22);

            DateTime cl1_Ca = DateTime.UtcNow;
            DateTime cl1_Ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "template1", "template_desc", "", "", 1, 1);

            //Case aCase = CreateCase("caseUID", cl1, )
            DateTime c1_ca = DateTime.UtcNow.AddDays(-9);

            int microtingUId = rnd.Next(1, 255);
            int microtingCheckId = rnd.Next(1, 255);


            // Act
            int matches = await sut.CaseCreate(cl1.Id, (int)site1.MicrotingUid, microtingUId, microtingCheckId, "", "",
                c1_ca, null);
            List<CheckListSite> checkListSiteResult1 = DbContext.CheckListSites.AsNoTracking().ToList();
            var versionedMatches1 = DbContext.CheckListSiteVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(matches);
            // Assert.AreEqual(Constants.WorkflowStates.Created, versionedMatches1[1].workflow_state);
            // Assert.AreEqual(Constants.WorkflowStates.Created, versionedMatches1[0].workflow_state);
        }

        [Test]
        public async Task SQL_Case_CaseReadLastCheckIdByMicrotingUId_DoesCaseReadLastIDByMicrotiingUID()
        {
            Random rnd = new Random();
            Site site1 = await testHelpers.CreateSite("mySite2", 331);
            DateTime cl1_Ca = DateTime.UtcNow;
            DateTime cl1_Ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "template2", "template_desc", "", "", 1, 1);

            string guid = Guid.NewGuid().ToString();
            string guid2 = Guid.NewGuid().ToString();
            int lastCheckUid1 = rnd.Next(1, 255);


            //check_list_sites cls1 = await testHelpers.CreateCheckListSite(cl1.Id, site1.Id, guid, Constants.WorkflowStates.Created, lastCheckUid1);
            CheckListSite cls1 = await testHelpers.CreateCheckListSite(cl1, cl1_Ca, site1, cl1_Ua, 1,
                Constants.WorkflowStates.Created, lastCheckUid1);

            //cases case1 = CreateCase

            // Act
            int? matches = await sut.CaseReadLastCheckIdByMicrotingUId(cls1.MicrotingUid);
            List<CheckListSite> checkListSiteResult1 = DbContext.CheckListSites.AsNoTracking().ToList();
            var versionedMatches1 = DbContext.CheckListSiteVersions.AsNoTracking().ToList();


            // Assert
            Assert.AreEqual(cls1.LastCheckId, matches);
        }

        [Test]
        public async Task SQL_Case_CaseUpdateRetrieved_DoesCaseGetUpdated()
        {
            // Arrance
            Random rnd = new Random();
            Site site = new Site
            {
                Name = "SiteName"
            };
            DbContext.Sites.Add(site);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            CheckList cl = new CheckList
            {
                Label = "label"
            };

            DbContext.CheckLists.Add(cl);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase =
                new Microting.eForm.Infrastructure.Data.Entities.Case
                {
                    MicrotingUid = rnd.Next(1, 255),
                    MicrotingCheckUid = rnd.Next(1, 255),
                    WorkflowState = Constants.WorkflowStates.Created,
                    CheckListId = cl.Id,
                    SiteId = site.Id,
                    Status = 66
                };

            DbContext.Cases.Add(aCase);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            await sut.CaseUpdateRetrieved((int)aCase.MicrotingUid);
            //CaseDto caseResult = await sut.CaseFindCustomMatchs(aCase.microting_uid);
            List<Microting.eForm.Infrastructure.Data.Entities.Case> caseResults =
                DbContext.Cases.AsNoTracking().ToList();


            // Assert


            Assert.NotNull(caseResults);
            Assert.AreEqual(1, caseResults.Count);
            Assert.AreNotEqual(1, caseResults[0]);
        }

        [Test]
        public async Task SQL_Case_CaseUpdateCompleted_DoesCaseGetUpdated()
        {
            MicrotingDbContext ldbContext = dbContextHelper.GetDbContext();
            Random rnd = new Random();
            Site site1 = await testHelpers.CreateSite("MySite", 22);
            DateTime cl1_Ca = DateTime.UtcNow;
            DateTime cl1_Ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ca, "template1", "template_desc", "", "", 1, 1);

            string guid = Guid.NewGuid().ToString();
            string lastCheckUid1 = Guid.NewGuid().ToString();

            //Case aCase = CreateCase("caseUID", cl1, )
            DateTime c1_ca = DateTime.UtcNow.AddDays(-9);
            string date = "2020-06-11 10:08:16";
            // string Date = date.AsSpan(0, 19).ToString();
            CultureInfo culture = CultureInfo.CreateSpecificCulture("da-DK");
            DateTime c1_da = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", culture);
            DateTime c1_ua = DateTime.UtcNow.AddDays(-8);
            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
            SiteWorker siteWorker = await testHelpers.CreateSiteWorker(55, site1, worker);
            Unit unit = await testHelpers.CreateUnit(48, 49, site1, 348);

            string microtingUId = Guid.NewGuid().ToString();
            string microtingCheckId = Guid.NewGuid().ToString();

            Microting.eForm.Infrastructure.Data.Entities.Case aCase1 = await testHelpers.CreateCase("case1UId", cl1,
                c1_ca, "custom1",
                DateTime.UtcNow, worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site1, 66, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);

            // Act
            //sut.CaseUpdateCompleted(aCase1.microting_uid, aCase1.microting_check_uid, c1_ua, aCase1.Id, aCase1.Id);
            List<Microting.eForm.Infrastructure.Data.Entities.Case> caseResults = DbContext.Cases.AsNoTracking()
                .Where(x => x.MicrotingUid == aCase1.MicrotingUid).ToList();
            Assert.NotNull(caseResults);
            Assert.AreEqual(1, caseResults.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, caseResults[0].WorkflowState);
            Assert.AreEqual(66, caseResults[0].Status);

            await sut.CaseUpdateCompleted((int)aCase1.MicrotingUid, (int)aCase1.MicrotingCheckUid, c1_da,
                aCase1.Worker.MicrotingUid, (int)unit.MicrotingUid);
            caseResults = DbContext.Cases.AsNoTracking().Where(x => x.MicrotingUid == aCase1.MicrotingUid).ToList();
            var versionedMatches1 = DbContext.CaseVersions.AsNoTracking()
                .Where(x => x.MicrotingUid == aCase1.MicrotingUid).ToList();
            //DbContext.cases

            // Assert


            Assert.NotNull(caseResults);
            Assert.AreEqual(1, caseResults.Count());
            Assert.AreEqual(1, versionedMatches1.Count());
            Assert.AreEqual(Constants.WorkflowStates.Created, caseResults[0].WorkflowState);
            Assert.AreEqual(Constants.WorkflowStates.Created, versionedMatches1[0].WorkflowState);
            Assert.AreEqual(100, caseResults[0].Status);
            Assert.AreEqual(100, versionedMatches1[0].Status);
            Assert.AreEqual("06/11/2020 10:08:16",
                caseResults[0].DoneAt.Value.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
            Assert.AreEqual("06/11/2020 10:08:16",
                caseResults[0].DoneAtUserModifiable.Value
                    .ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
            Assert.AreEqual("06/11/2020 10:08:16",
                versionedMatches1[0].DoneAt.Value.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
            Assert.AreEqual("06/11/2020 10:08:16",
                versionedMatches1[0].DoneAtUserModifiable.Value
                    .ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
        }

        [Test]
        public async Task SQL_Case_CaseRetract_DoesCaseGetRetracted()
        {
            // Arrance
            Random rnd = new Random();
            Site site = new Site
            {
                Name = "SiteName"
            };
            DbContext.Sites.Add(site);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            CheckList cl = new CheckList
            {
                Label = "label"
            };

            DbContext.CheckLists.Add(cl);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase =
                new Microting.eForm.Infrastructure.Data.Entities.Case
                {
                    MicrotingUid = rnd.Next(1, 255),
                    MicrotingCheckUid = rnd.Next(1, 255),
                    WorkflowState = Constants.WorkflowStates.Created,
                    CheckListId = cl.Id,
                    SiteId = site.Id
                };

            DbContext.Cases.Add(aCase);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            await sut.CaseRetract((int)aCase.MicrotingUid, (int)aCase.MicrotingCheckUid);
            //cases theCase = await sut.CaseReadFull(aCase.microting_uid, aCase.microting_check_uid);
            var match = DbContext.Cases.AsNoTracking().ToList();
            var versionedMatches = DbContext.CaseVersions.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Count);
            Assert.AreEqual(1, versionedMatches.Count);
            Assert.AreEqual(Constants.WorkflowStates.Retracted, match[0].WorkflowState);
            Assert.AreEqual(Constants.WorkflowStates.Retracted, versionedMatches[0].WorkflowState);
        }

        [Test]
        public async Task SQL_Case_CaseDelete_DoesCaseRemoved()
        {
            // Arrance
            Random rnd = new Random();
            Site site = new Site { Name = "SiteName" };
            DbContext.Sites.Add(site);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            CheckList cl = new CheckList { Label = "label" };

            DbContext.CheckLists.Add(cl);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase =
                new Microting.eForm.Infrastructure.Data.Entities.Case
                {
                    MicrotingUid = rnd.Next(1, 255),
                    MicrotingCheckUid = rnd.Next(1, 255),
                    WorkflowState = Constants.WorkflowStates.Created,
                    CheckListId = cl.Id,
                    SiteId = site.Id
                };

            DbContext.Cases.Add(aCase);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            await sut.CaseDelete((int)aCase.MicrotingUid);
            //cases theCase = await sut.CaseReadFull(aCase.microting_uid, aCase.microting_check_uid);
            var match = DbContext.Cases.AsNoTracking().ToList();
            var versionedMatches = DbContext.CaseVersions.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Count);
            Assert.AreEqual(1, versionedMatches.Count);
            Assert.AreEqual(Constants.WorkflowStates.Removed, match[0].WorkflowState);
            Assert.AreEqual(Constants.WorkflowStates.Removed, versionedMatches[0].WorkflowState);
        }

        [Test]
        public async Task SQL_Case_CaseDeleteResult_DoesMarkCaseRemoved()
        {
            // Arrance
            Random rnd = new Random();
            Site site = new Site { Name = "SiteName" };
            DbContext.Sites.Add(site);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            CheckList cl = new CheckList { Label = "label" };

            DbContext.CheckLists.Add(cl);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            Microting.eForm.Infrastructure.Data.Entities.Case aCase =
                new Microting.eForm.Infrastructure.Data.Entities.Case
                {
                    MicrotingUid = rnd.Next(1, 255),
                    MicrotingCheckUid = rnd.Next(1, 255),
                    WorkflowState = Constants.WorkflowStates.Created,
                    CheckListId = cl.Id,
                    SiteId = site.Id
                };

            DbContext.Cases.Add(aCase);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            await sut.CaseDeleteResult(aCase.Id);
            //cases theCase = await sut.CaseReadFull(aCase.microting_uid, aCase.microting_check_uid);
            var match = DbContext.Cases.AsNoTracking().ToList();
            var versionedMatches = DbContext.CaseVersions.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Count);
            Assert.AreEqual(1, versionedMatches.Count);
            Assert.AreEqual(Constants.WorkflowStates.Removed, match[0].WorkflowState);
            Assert.AreEqual(Constants.WorkflowStates.Removed, versionedMatches[0].WorkflowState);
        }

        [Test]
        public async Task SQL_PostCase_CaseReadFirstId()
        {
            // Arrance
            Random rnd = new Random();

            #region Arrance

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
                DateTime.UtcNow, "custom", DateTime.UtcNow,
                worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 100, "caseType", unit, DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            // Act
            var match = await sut.CaseReadFirstId(aCase.CheckListId, Constants.WorkflowStates.NotRemoved);
            // Assert
            Assert.AreEqual(aCase.Id, match);
        }

        [Test]
        public async Task SQL_PostCase_CaseFindCustomMatchs()
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

            #endregion

            #endregion

            // Act
            List<CaseDto> aCase1Custom = await sut.CaseFindCustomMatchs(aCase1.Custom);
            List<CaseDto> aCase2Custom = await sut.CaseFindCustomMatchs(aCase2.Custom);
            List<CaseDto> aCase3Custom = await sut.CaseFindCustomMatchs(aCase3.Custom);
            List<CaseDto> aCase4Custom = await sut.CaseFindCustomMatchs(aCase4.Custom);
            // Assert
            Assert.AreEqual(aCase1.Custom, aCase1Custom[0].Custom);
            Assert.AreEqual(aCase2.Custom, aCase2Custom[0].Custom);
            Assert.AreEqual(aCase3.Custom, aCase3Custom[0].Custom);
            Assert.AreEqual(aCase4.Custom, aCase4Custom[0].Custom);
        }

        [Test]
        public async Task SQL_PostCase_CaseUpdateFieldValues()
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

            Microting.eForm.Infrastructure.Data.Entities.Case aCase1 = await testHelpers.CreateCase("case1UId", cl2,
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

            #endregion

            #region UploadedData

            #region ud1

            UploadedData ud1 = await testHelpers.CreateUploadedData("checksum1", "File1", "no", "hjgjghjhg", "File1", 1,
                worker1,
                "local", 55, false);

            #endregion

            #region ud2

            UploadedData ud2 = await testHelpers.CreateUploadedData("checksum2", "File1", "no", "hjgjghjhg", "File2", 1,
                worker1,
                "local", 55, false);

            #endregion

            #region ud3

            UploadedData ud3 = await testHelpers.CreateUploadedData("checksum3", "File1", "no", "hjgjghjhg", "File3", 1,
                worker1,
                "local", 55, false);

            #endregion

            #region ud4

            UploadedData ud4 = await testHelpers.CreateUploadedData("checksum4", "File1", "no", "hjgjghjhg", "File4", 1,
                worker1,
                "local", 55, false);

            #endregion

            #region ud5

            UploadedData ud5 = await testHelpers.CreateUploadedData("checksum5", "File1", "no", "hjgjghjhg", "File5", 1,
                worker1,
                "local", 55, false);

            #endregion

            #region ud6

            UploadedData ud6 = await testHelpers.CreateUploadedData("checksum6", "File1", "no", "hjgjghjhg", "File6", 1,
                worker1,
                "local", 55, false);

            #endregion

            #region ud7

            UploadedData ud7 = await testHelpers.CreateUploadedData("checksum7", "File1", "no", "hjgjghjhg", "File7", 1,
                worker1,
                "local", 55, false);

            #endregion

            #region ud8

            UploadedData ud8 = await testHelpers.CreateUploadedData("checksum8", "File1", "no", "hjgjghjhg", "File8", 1,
                worker1,
                "local", 55, false);

            #endregion

            #region ud9

            UploadedData ud9 = await testHelpers.CreateUploadedData("checksum9", "File1", "no", "hjgjghjhg", "File9", 1,
                worker1,
                "local", 55, false);

            #endregion

            #region ud10

            UploadedData ud10 = await testHelpers.CreateUploadedData("checksum10", "File1", "no", "hjgjghjhg", "File10",
                1, worker1,
                "local", 55, false);

            #endregion

            #endregion

            #region Field Values

            #region fv1

            FieldValue field_Value1 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.Id, null, "tomt1", 61234, worker1);

            #endregion

            #region fv2

            FieldValue field_Value2 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.Id, null, "tomt2", 61234, worker1);

            #endregion

            #region fv3

            FieldValue field_Value3 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.Id, null, "tomt3", 61234, worker1);

            #endregion

            #region fv4

            FieldValue field_Value4 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.Id, null, "tomt4", 61234, worker1);

            #endregion

            #region fv5

            FieldValue field_Value5 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.Id, null, "tomt5", 61234, worker1);

            #endregion

            #region fv6

            FieldValue field_Value6 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.Id, null, "tomt6", 61234, worker1);

            #endregion

            #region fv7

            FieldValue field_Value7 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.Id, null, "tomt7", 61234, worker1);

            #endregion

            #region fv8

            FieldValue field_Value8 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.Id, null, "tomt8", 61234, worker1);

            #endregion

            #region fv9

            FieldValue field_Value9 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.Id, null, "tomt9", 61234, worker1);

            #endregion

            #region fv10

            FieldValue field_Value10 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.Id, null, "tomt10", 61234, worker1);

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
        public async Task SQL_PostCase_CaseReadByMUId_Returns_ReturnCase()
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

            #region Case1

            Microting.eForm.Infrastructure.Data.Entities.Case aCase = await testHelpers.CreateCase("caseUId", cl1,
                DateTime.UtcNow, "custom", DateTime.UtcNow,
                worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 66, "caseType", unit, DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region UploadedData

            #region ud1

            UploadedData ud1 = await testHelpers.CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File1", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud2

            UploadedData ud2 = await testHelpers.CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File2", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud3

            UploadedData ud3 = await testHelpers.CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File3", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud4

            UploadedData ud4 = await testHelpers.CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File4", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud5

            UploadedData ud5 = await testHelpers.CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File5", 1,
                worker,
                "local", 55, false);

            #endregion

            #endregion

            #region Check List Values

            CheckListValue checkListValue = await testHelpers.CreateCheckListValue(aCase, cl2, "checked", null, 865);

            #endregion

            #region Field Values

            #region fv1

            FieldValue field_Value1 =
                await testHelpers.CreateFieldValue(aCase, cl2, f1, ud1.Id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2

            FieldValue field_Value2 =
                await testHelpers.CreateFieldValue(aCase, cl2, f2, ud2.Id, null, "tomt2", 61234, worker);

            #endregion

            #region fv3

            FieldValue field_Value3 =
                await testHelpers.CreateFieldValue(aCase, cl2, f3, ud3.Id, null, "tomt3", 61234, worker);

            #endregion

            #region fv4

            FieldValue field_Value4 =
                await testHelpers.CreateFieldValue(aCase, cl2, f4, ud4.Id, null, "tomt4", 61234, worker);

            #endregion

            #region fv5

            FieldValue field_Value5 =
                await testHelpers.CreateFieldValue(aCase, cl2, f5, ud5.Id, null, "tomt5", 61234, worker);

            #endregion

            #endregion

            #endregion


            // Act

            var match = await sut.CaseReadByMUId((int)aCase.MicrotingUid);

            // Assert

            Assert.AreEqual(aCase.MicrotingUid, match.MicrotingUId);
        }

        [Test]
        public async Task SQL_PostCase_CaseReadByCaseId_Returns_cDto()
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

            #region Case1

            Microting.eForm.Infrastructure.Data.Entities.Case aCase = await testHelpers.CreateCase("caseUId", cl1,
                DateTime.UtcNow, "custom", DateTime.UtcNow,
                worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 66, "caseType", unit, DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion


            // Act

            var match = await sut.CaseReadByCaseId(aCase.Id);

            // Assert

            Assert.AreEqual(aCase.Id, match.CaseId);
        }

        [Test]
        public async Task SQL_PostCase_CaseReadByCaseUId_Returns_lstDto()
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

            #region Case1

            Microting.eForm.Infrastructure.Data.Entities.Case aCase = await testHelpers.CreateCase("caseUId", cl1,
                DateTime.UtcNow, "custom", DateTime.UtcNow,
                worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 66, "caseType", unit, DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion


            // Act

            var match = await sut.CaseReadByCaseUId(aCase.CaseUid);


            // Assert

            Assert.AreEqual(aCase.CaseUid, match[0].CaseUId);
        }

        [Test]
        public async Task SQL_PostCase_CaseReadFull()
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

            #region Case1

            Microting.eForm.Infrastructure.Data.Entities.Case aCase = await testHelpers.CreateCase("caseUId", cl1,
                DateTime.UtcNow, "custom", DateTime.UtcNow,
                worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 66, "caseType", unit, DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            // Act
            var match = await sut.CaseReadFull((int)aCase.MicrotingUid, (int)aCase.MicrotingCheckUid);
            // Assert
            Assert.AreEqual(aCase.MicrotingUid, match.MicrotingUid);
            Assert.AreEqual(aCase.MicrotingCheckUid, match.MicrotingCheckUid);
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