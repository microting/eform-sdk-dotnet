using eFormCore;
using eFormCore.Helpers;
using eFormData;
using eFormShared;
using eFormSqlController;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class CoreTestCaseReadAllShort : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;

        public override void DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            sql.SettingUpdate(Settings.firstRunDone, "true");
            sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new Core();
            sut.HandleCaseCreated += EventCaseCreated;
            sut.HandleCaseRetrived += EventCaseRetrived;
            sut.HandleCaseCompleted += EventCaseCompleted;
            sut.HandleCaseDeleted += EventCaseDeleted;
            sut.HandleFileDownloaded += EventFileDownloaded;
            sut.HandleSiteActivated += EventSiteActivated;
            sut.StartSqlOnly(ConnectionString);
            path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:\", "");
            sut.SetPicturePath(path + @"\output\dataFolder\picture\");
            sut.SetPdfPath(path + @"\output\dataFolder\pdf\");
            sut.SetJasperPath(path + @"\output\dataFolder\reports\");
            testHelpers = new TestHelpers();
            //sut.StartLog(new CoreBase());
        }

        [Test]
        public void Core_Case_CaseReadAll_Short()
        {


            // Arrance
            #region Arrance
            #region Template1
            DateTime c1_Ca = DateTime.Now;
            DateTime c1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(c1_Ca, c1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion
            #endregion

            #region Worker

            workers worker = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region site
            sites site = testHelpers.CreateSite("SiteName", 88);

            #endregion

            #region units
            units unit = testHelpers.CreateUnit(48, 49, site, 348);

            #endregion

            #region site_workers
            site_workers site_workers = testHelpers.CreateSiteWorker(55, site, worker);

            #endregion

            #region cases
            #region cases created
            #region Case1

            DateTime c1_ca = DateTime.Now.AddDays(-9);
            DateTime c1_da = DateTime.Now.AddDays(-8).AddHours(-12);
            DateTime c1_ua = DateTime.Now.AddDays(-8);

            cases aCase1 = testHelpers.CreateCase("case1UId", cl1, c1_ca, "custom1",
                c1_da, worker, "microtingCheckUId1", "microtingUId1",
               site, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Case2

            DateTime c2_ca = DateTime.Now.AddDays(-7);
            DateTime c2_da = DateTime.Now.AddDays(-6).AddHours(-12);
            DateTime c2_ua = DateTime.Now.AddDays(-6);
            cases aCase2 = testHelpers.CreateCase("case2UId", cl1, c2_ca, "custom2",
             c2_da, worker, "microtingCheck2UId", "microting2UId",
               site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case3
            DateTime c3_ca = DateTime.Now.AddDays(-10);
            DateTime c3_da = DateTime.Now.AddDays(-9).AddHours(-12);
            DateTime c3_ua = DateTime.Now.AddDays(-9);

            cases aCase3 = testHelpers.CreateCase("case3UId", cl1, c3_ca, "custom3",
              c3_da, worker, "microtingCheck3UId", "microtin3gUId",
               site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case4
            DateTime c4_ca = DateTime.Now.AddDays(-8);
            DateTime c4_da = DateTime.Now.AddDays(-7).AddHours(-12);
            DateTime c4_ua = DateTime.Now.AddDays(-7);

            cases aCase4 = testHelpers.CreateCase("case4UId", cl1, c4_ca, "custom4",
                c4_da, worker, "microtingCheck4UId", "microting4UId",
               site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion
            #endregion

            #region cases removed

            #region Case1Removed

            DateTime c1Removed_ca = DateTime.Now.AddDays(-9);
            DateTime c1Removed_da = DateTime.Now.AddDays(-8).AddHours(-12);
            DateTime c1Removed_ua = DateTime.Now.AddDays(-8);

            cases aCase1Removed = testHelpers.CreateCase("case1UId", cl1, c1Removed_ca, "custom1",
                c1Removed_da, worker, "microtingCheckUId1", "microtingUId1",
               site, 1, "caseType1", unit, c1Removed_ua, 1, worker, Constants.WorkflowStates.Removed);

            #endregion

            #region Case2Removed

            DateTime c2Removed_ca = DateTime.Now.AddDays(-7);
            DateTime c2Removed_da = DateTime.Now.AddDays(-6).AddHours(-12);
            DateTime c2Removed_ua = DateTime.Now.AddDays(-6);
            cases aCase2Removed = testHelpers.CreateCase("case2UId", cl1, c2Removed_ca, "custom2",
             c2Removed_da, worker, "microtingCheck2UId", "microting2UId",
               site, 10, "caseType2", unit, c2Removed_ua, 1, worker, Constants.WorkflowStates.Removed);
            #endregion

            #region Case3Removed
            DateTime c3Removed_ca = DateTime.Now.AddDays(-10);
            DateTime c3Removed_da = DateTime.Now.AddDays(-9).AddHours(-12);
            DateTime c3Removed_ua = DateTime.Now.AddDays(-9);

            cases aCase3Removed = testHelpers.CreateCase("case3UId", cl1, c3Removed_ca, "custom3",
              c3Removed_da, worker, "microtingCheck3UId", "microtin3gUId",
               site, 15, "caseType3", unit, c3Removed_ua, 1, worker, Constants.WorkflowStates.Removed);
            #endregion

            #region Case4Removed
            DateTime c4Removed_ca = DateTime.Now.AddDays(-8);
            DateTime c4Removed_da = DateTime.Now.AddDays(-7).AddHours(-12);
            DateTime c4Removed_ua = DateTime.Now.AddDays(-7);

            cases aCase4Removed = testHelpers.CreateCase("case4UId", cl1, c4Removed_ca, "custom4",
                c4Removed_da, worker, "microtingCheck4UId", "microting4UId",
               site, 100, "caseType4", unit, c4Removed_ua, 1, worker, Constants.WorkflowStates.Removed);
            #endregion

            #endregion

            #region cases Retracted

            #region Case1Retracted

            DateTime c1Retracted_ca = DateTime.Now.AddDays(-9);
            DateTime c1Retracted_da = DateTime.Now.AddDays(-8).AddHours(-12);
            DateTime c1Retracted_ua = DateTime.Now.AddDays(-8);

            cases aCase1Retracted = testHelpers.CreateCase("case1UId", cl1, c1Retracted_ca, "custom1",
                c1Retracted_da, worker, "microtingCheckUId1", "microtingUId1",
               site, 1, "caseType1", unit, c1Retracted_ua, 1, worker, Constants.WorkflowStates.Retracted);

            #endregion

            #region Case2Retracted

            DateTime c2Retracted_ca = DateTime.Now.AddDays(-7);
            DateTime c2Retracted_da = DateTime.Now.AddDays(-6).AddHours(-12);
            DateTime c2Retracted_ua = DateTime.Now.AddDays(-6);

            cases aCase2Retracted = testHelpers.CreateCase("case2UId", cl1, c2Retracted_ca, "custom2",
             c2Retracted_da, worker, "microtingCheck2UId", "microting2UId",
               site, 10, "caseType2", unit, c2Retracted_ua, 1, worker, Constants.WorkflowStates.Retracted);
            #endregion

            #region Case3Retracted
            DateTime c3Retracted_ca = DateTime.Now.AddDays(-10);
            DateTime c3Retracted_da = DateTime.Now.AddDays(-9).AddHours(-12);
            DateTime c3Retracted_ua = DateTime.Now.AddDays(-9);

            cases aCase3Retracted = testHelpers.CreateCase("case3UId", cl1, c3Retracted_ca, "custom3",
              c3Retracted_da, worker, "microtingCheck3UId", "microtin3gUId",
               site, 15, "caseType3", unit, c3Retracted_ua, 1, worker, Constants.WorkflowStates.Retracted);
            #endregion

            #region Case4Retracted
            DateTime c4Retracted_ca = DateTime.Now.AddDays(-8);
            DateTime c4Retracted_da = DateTime.Now.AddDays(-7).AddHours(-12);
            DateTime c4Retracted_ua = DateTime.Now.AddDays(-7);

            cases aCase4Retracted = testHelpers.CreateCase("case4UId", cl1, c4Retracted_ca, "custom4",
                c4Retracted_da, worker, "microtingCheck4UId", "microting4UId",
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
            //List<Case> caseListCreatedAt = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Case> caseListDoneAt = sut.CaseReadAll(cl1.id, null, null);
            //List<Case> caseListFieldValue1 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListFieldValue2 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListFieldValue3 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListFieldValue4 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListFieldValue5 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListFieldValue6 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListFieldValue7 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListFieldValue8 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListFieldValue9 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListFieldValue10 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListSiteName = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.SiteName);
            List<Case> caseListStatus = sut.CaseReadAll(cl1.id, null, null);
            List<Case> caseListUnitId = sut.CaseReadAll(cl1.id, null, null);
            //List<Case> caseListWorkerName = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.WorkerName);
            #endregion

            #region Default sorting ascending, with DateTime
            // Default sorting ascending, with DateTime
            //List<Case> caseListDtCreatedAt = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.CreatedAt);
            List<Case> caseListDtDoneAt = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8));
            //List<Case> caseDtListFieldValue1 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseDtListFieldValue2 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseDtListFieldValue3 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseDtListFieldValue4 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseDtListFieldValue5 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseDtListFieldValue6 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseDtListFieldValue7 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseDtListFieldValue8 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseDtListFieldValue9 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseDtListFieldValue10 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseDtListSiteName = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.SiteName);
            List<Case> caseListDtStatus = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8));
            List<Case> caseListDtUnitId = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8));
            //List<Case> caseListDtWorkerName = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "", false, Constants.CaseSortParameters.WorkerName);
            #endregion

            #endregion

            #region case sorting

            #region aCase sorting ascending
            #region aCase1 sorting ascendng
            //List<Case> caseListC1SortCreatedAt = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Case> caseListC1SortDoneAt = sut.CaseReadAll(cl1.id, null, null);
            //List<Case> caseListC1SortFieldValue1 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC1SortFieldValue2 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC1SortFieldValue3 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC1SortFieldValue4 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC1SortFieldValue5 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC1SortFieldValue6 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC1SortFieldValue7 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC1SortFieldValue8 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC1SortFieldValue9 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC1SortFieldValue10 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC1SortSiteName = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.SiteName);
            List<Case> caseListC1SortStatus = sut.CaseReadAll(cl1.id, null, null);
            List<Case> caseListC1SortUnitId = sut.CaseReadAll(cl1.id, null, null);
            //List<Case> caseListC1SortWorkerName = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.WorkerName);
            #endregion

            #region aCase2 sorting ascendng
            //List<Case> caseListC2SortCreatedAt = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Case> caseListC2SortDoneAt = sut.CaseReadAll(cl1.id, null, null);
            //List<Case> caseListC2SortFieldValue1 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC2SortFieldValue2 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC2SortFieldValue3 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC2SortFieldValue4 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC2SortFieldValue5 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC2SortFieldValue6 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC2SortFieldValue7 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC2SortFieldValue8 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC2SortFieldValue9 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC2SortFieldValue10 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC2SortSiteName = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.SiteName);
            List<Case> caseListC2SortStatus = sut.CaseReadAll(cl1.id, null, null);
            List<Case> caseListC2SortUnitId = sut.CaseReadAll(cl1.id, null, null);
            //List<Case> caseListC2SortWorkerName = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.WorkerName);
            #endregion

            #region aCase3 sorting ascendng
            //List<Case> caseListC3SortCreatedAt = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Case> caseListC3SortDoneAt = sut.CaseReadAll(cl1.id, null, null);
            //List<Case> caseListC3SortFieldValue1 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC3SortFieldValue2 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC3SortFieldValue3 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC3SortFieldValue4 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC3SortFieldValue5 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC3SortFieldValue6 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC3SortFieldValue7 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC3SortFieldValue8 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC3SortFieldValue9 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC3SortFieldValue10 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC3SortSiteName = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.SiteName);
            List<Case> caseListC3SortStatus = sut.CaseReadAll(cl1.id, null, null);
            List<Case> caseListC3SortUnitId = sut.CaseReadAll(cl1.id, null, null);
            //List<Case> caseListC3SortWorkerName = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.WorkerName);
            #endregion

            #region aCase4 sorting ascendng
            //List<Case> caseListC4SortCreatedAt = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Case> caseListC4SortDoneAt = sut.CaseReadAll(cl1.id, null, null);
            //List<Case> caseListC4SortFieldValue1 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC4SortFieldValue2 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC4SortFieldValue3 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC4SortFieldValue4 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC4SortFieldValue5 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC4SortFieldValue6 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC4SortFieldValue7 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC4SortFieldValue8 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC4SortFieldValue9 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC4SortFieldValue10 = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC4SortSiteName = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.SiteName);
            List<Case> caseListC4SortStatus = sut.CaseReadAll(cl1.id, null, null);
            List<Case> caseListC4SortUnitId = sut.CaseReadAll(cl1.id, null, null);
            //List<Case> caseListC4SortWorkerName = sut.CaseReadAll(cl1.id, null, null, Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.WorkerName);
            #endregion

            #endregion

            #region aCase sorting ascending w. Dt
            #region aCase1 sorting ascendng w. Dt
            //List<Case> caseListC1SortDtCreatedAt = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.CreatedAt);
            List<Case> caseListC1SortDtDoneAt = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8));
            //List<Case> caseListC1SortDtFieldValue1 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC1SortDtFieldValue2 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC1SortDtFieldValue3 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC1SortDtFieldValue4 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC1SortDtFieldValue5 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC1SortDtFieldValue6 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC1SortDtFieldValue7 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC1SortDtFieldValue8 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC1SortDtFieldValue9 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC1SortDtFieldValue10 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC1SortDtSiteName = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.SiteName);
            List<Case> caseListC1SortDtStatus = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8));
            List<Case> caseListC1SortDtUnitId = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8));
            //List<Case> caseListC1SortDtWorkerName = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "1", false, Constants.CaseSortParameters.WorkerName);
            #endregion

            #region aCase2 sorting ascendng w. Dt
            //List<Case> caseListC2SortDtCreatedAt = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.CreatedAt);
            List<Case> caseListC2SortDtDoneAt = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8));
            //List<Case> caseListC2SortDtFieldValue1 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC2SortDtFieldValue2 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC2SortDtFieldValue3 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC2SortDtFieldValue4 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC2SortDtFieldValue5 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC2SortDtFieldValue6 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC2SortDtFieldValue7 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC2SortDtFieldValue8 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC2SortDtFieldValue9 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC2SortDtFieldValue10 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC2SortDtSiteName = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.SiteName);
            List<Case> caseListC2SortDtStatus = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8));
            List<Case> caseListC2SortDtUnitId = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8));
            //List<Case> caseListC2SortDtWorkerName = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-8), Constants.WorkflowStates.Created, "2", false, Constants.CaseSortParameters.WorkerName);
            #endregion

            #region aCase3 sorting ascendng w. Dt
            //List<Case> caseListC3SortDtCreatedAt = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.CreatedAt);
            List<Case> caseListC3SortDtDoneAt = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6));
            //List<Case> caseListC3SortDtFieldValue1 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC3SortDtFieldValue2 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC3SortDtFieldValue3 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC3SortDtFieldValue4 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC3SortDtFieldValue5 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC3SortDtFieldValue6 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC3SortDtFieldValue7 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC3SortDtFieldValue8 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC3SortDtFieldValue9 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC3SortDtFieldValue10 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC3SortDtSiteName = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.SiteName);
            List<Case> caseListC3SortDtStatus = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6));
            List<Case> caseListC3SortDtUnitId = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6));
            //List<Case> caseListC3SortDtWorkerName = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "3", false, Constants.CaseSortParameters.WorkerName);
            #endregion

            #region aCase4 sorting ascendng w. Dt
            //List<Case> caseListC4SortDtCreatedAt = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.CreatedAt);
            List<Case> caseListC4SortDtDoneAt = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6));
            //List<Case> caseListC4SortDtFieldValue1 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue1);
            //List<Case> caseListC4SortDtFieldValue2 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue2);
            //List<Case> caseListC4SortDtFieldValue3 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue3);
            //List<Case> caseListC4SortDtFieldValue4 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue4);
            //List<Case> caseListC4SortDtFieldValue5 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue5);
            //List<Case> caseListC4SortDtFieldValue6 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue6);
            //List<Case> caseListC4SortDtFieldValue7 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue7);
            //List<Case> caseListC4SortDtFieldValue8 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue8);
            //List<Case> caseListC4SortDtFieldValue9 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue9);
            //List<Case> caseListC4SortDtFieldValue10 = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.FieldValue10);
            //List<Case> caseListC4SortDtSiteName = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.SiteName);
            List<Case> caseListC4SortDtStatus = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6));
            List<Case> caseListC4SortDtUnitId = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6));
            //List<Case> caseListC4SortDtWorkerName = sut.CaseReadAll(cl1.id, DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-6), Constants.WorkflowStates.Created, "4", false, Constants.CaseSortParameters.WorkerName);
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
            Assert.AreEqual(aCase1.type, caseListDoneAt[0].CaseType);
            Assert.AreEqual(aCase1.case_uid, caseListDoneAt[0].CaseUId);
            Assert.AreEqual(aCase1.microting_check_uid, caseListDoneAt[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.custom, caseListDoneAt[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.id, caseListDoneAt[0].Id);
            Assert.AreEqual(aCase1.microting_uid, caseListDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase1.site.microting_uid, caseListDoneAt[0].SiteId);
            Assert.AreEqual(aCase1.site.name, caseListDoneAt[0].SiteName);
            Assert.AreEqual(aCase1.status, caseListDoneAt[0].Status);
            Assert.AreEqual(cl1.id, caseListDoneAt[0].TemplatId);
            Assert.AreEqual(aCase1.unit.microting_uid, caseListDoneAt[0].UnitId);
            Assert.AreEqual(c1_ua.ToString(), caseListDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.version, caseListDoneAt[0].Version);
            Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListDoneAt[0].WorkerName);
            Assert.AreEqual(aCase1.workflow_state, caseListDoneAt[0].WorkflowState);
            #endregion

            #region caseListDoneAt aCase2
            Assert.AreEqual(aCase2.type, caseListDoneAt[1].CaseType);
            Assert.AreEqual(aCase2.case_uid, caseListDoneAt[1].CaseUId);
            Assert.AreEqual(aCase2.microting_check_uid, caseListDoneAt[1].CheckUIid);
            Assert.AreEqual(c2_ca.ToString(), caseListDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase2.custom, caseListDoneAt[1].Custom);
            Assert.AreEqual(c2_da.ToString(), caseListDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase2.id, caseListDoneAt[1].Id);
            Assert.AreEqual(aCase2.microting_uid, caseListDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase2.site.microting_uid, caseListDoneAt[1].SiteId);
            Assert.AreEqual(aCase2.site.name, caseListDoneAt[1].SiteName);
            Assert.AreEqual(aCase2.status, caseListDoneAt[1].Status);
            Assert.AreEqual(cl1.id, caseListDoneAt[1].TemplatId);
            Assert.AreEqual(aCase2.unit.microting_uid, caseListDoneAt[1].UnitId);
            Assert.AreEqual(c2_ua.ToString(), caseListDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase2.version, caseListDoneAt[1].Version);
            Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListDoneAt[1].WorkerName);
            Assert.AreEqual(aCase2.workflow_state, caseListDoneAt[1].WorkflowState);
            #endregion

            #region caseListDoneAt aCase3
            Assert.AreEqual(aCase3.type, caseListDoneAt[2].CaseType);
            Assert.AreEqual(aCase3.case_uid, caseListDoneAt[2].CaseUId);
            Assert.AreEqual(aCase3.microting_check_uid, caseListDoneAt[2].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDoneAt[2].CreatedAt.ToString());
            Assert.AreEqual(aCase3.custom, caseListDoneAt[2].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDoneAt[2].DoneAt.ToString());
            Assert.AreEqual(aCase3.id, caseListDoneAt[2].Id);
            Assert.AreEqual(aCase3.microting_uid, caseListDoneAt[2].MicrotingUId);
            Assert.AreEqual(aCase3.site.microting_uid, caseListDoneAt[2].SiteId);
            Assert.AreEqual(aCase3.site.name, caseListDoneAt[2].SiteName);
            Assert.AreEqual(aCase3.status, caseListDoneAt[2].Status);
            Assert.AreEqual(cl1.id, caseListDoneAt[2].TemplatId);
            Assert.AreEqual(aCase3.unit.microting_uid, caseListDoneAt[2].UnitId);
            Assert.AreEqual(c3_ua.ToString(), caseListDoneAt[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.version, caseListDoneAt[2].Version);
            Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListDoneAt[2].WorkerName);
            Assert.AreEqual(aCase3.workflow_state, caseListDoneAt[2].WorkflowState);
            #endregion

            #region caseListDoneAt aCase4
            Assert.AreEqual(aCase4.type, caseListDoneAt[3].CaseType);
            Assert.AreEqual(aCase4.case_uid, caseListDoneAt[3].CaseUId);
            Assert.AreEqual(aCase4.microting_check_uid, caseListDoneAt[3].CheckUIid);
            Assert.AreEqual(c4_ca.ToString(), caseListDoneAt[3].CreatedAt.ToString());
            Assert.AreEqual(aCase4.custom, caseListDoneAt[3].Custom);
            Assert.AreEqual(c4_da.ToString(), caseListDoneAt[3].DoneAt.ToString());
            Assert.AreEqual(aCase4.id, caseListDoneAt[3].Id);
            Assert.AreEqual(aCase4.microting_uid, caseListDoneAt[3].MicrotingUId);
            Assert.AreEqual(aCase4.site.microting_uid, caseListDoneAt[3].SiteId);
            Assert.AreEqual(aCase4.site.name, caseListDoneAt[3].SiteName);
            Assert.AreEqual(aCase4.status, caseListDoneAt[3].Status);
            Assert.AreEqual(cl1.id, caseListDoneAt[3].TemplatId);
            Assert.AreEqual(aCase4.unit.microting_uid, caseListDoneAt[3].UnitId);
            Assert.AreEqual(c4_ua.ToString(), caseListDoneAt[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase4.version, caseListDoneAt[3].Version);
            Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListDoneAt[3].WorkerName);
            Assert.AreEqual(aCase4.workflow_state, caseListDoneAt[3].WorkflowState);
            #endregion

            #endregion

            #region caseListStatus Def Sort Asc

            #region caseListStatus aCase1

            Assert.NotNull(caseListStatus);
            Assert.AreEqual(8, caseListStatus.Count);
            Assert.AreEqual(aCase1.type, caseListStatus[0].CaseType);
            Assert.AreEqual(aCase1.case_uid, caseListStatus[0].CaseUId);
            Assert.AreEqual(aCase1.microting_check_uid, caseListStatus[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.custom, caseListStatus[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.id, caseListStatus[0].Id);
            Assert.AreEqual(aCase1.microting_uid, caseListStatus[0].MicrotingUId);
            Assert.AreEqual(aCase1.site.microting_uid, caseListStatus[0].SiteId);
            Assert.AreEqual(aCase1.site.name, caseListStatus[0].SiteName);
            Assert.AreEqual(aCase1.status, caseListStatus[0].Status);
            Assert.AreEqual(aCase1.check_list_id, caseListStatus[0].TemplatId);
            Assert.AreEqual(aCase1.unit.microting_uid, caseListStatus[0].UnitId);
            Assert.AreEqual(c1_ua.ToString(), caseListStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.version, caseListStatus[0].Version);
            Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListStatus[0].WorkerName);
            Assert.AreEqual(aCase1.workflow_state, caseListStatus[0].WorkflowState);

            #endregion

            #region caseListStatus aCase2

            Assert.AreEqual(aCase2.type, caseListStatus[1].CaseType);
            Assert.AreEqual(aCase2.case_uid, caseListStatus[1].CaseUId);
            Assert.AreEqual(aCase2.microting_check_uid, caseListStatus[1].CheckUIid);
            Assert.AreEqual(c2_ca.ToString(), caseListStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase2.custom, caseListStatus[1].Custom);
            Assert.AreEqual(c2_da.ToString(), caseListStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase2.id, caseListStatus[1].Id);
            Assert.AreEqual(aCase2.microting_uid, caseListStatus[1].MicrotingUId);
            Assert.AreEqual(aCase2.site.microting_uid, caseListStatus[1].SiteId);
            Assert.AreEqual(aCase2.site.name, caseListStatus[1].SiteName);
            Assert.AreEqual(aCase2.status, caseListStatus[1].Status);
            Assert.AreEqual(aCase2.check_list_id, caseListStatus[1].TemplatId);
            Assert.AreEqual(aCase2.unit.microting_uid, caseListStatus[1].UnitId);
            Assert.AreEqual(c2_ua.ToString(), caseListStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase2.version, caseListStatus[1].Version);
            Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListStatus[1].WorkerName);
            Assert.AreEqual(aCase2.workflow_state, caseListStatus[1].WorkflowState);

            #endregion

            #region caseListStatus aCase3

            Assert.AreEqual(aCase3.type, caseListStatus[2].CaseType);
            Assert.AreEqual(aCase3.case_uid, caseListStatus[2].CaseUId);
            Assert.AreEqual(aCase3.microting_check_uid, caseListStatus[2].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListStatus[2].CreatedAt.ToString());
            Assert.AreEqual(aCase3.custom, caseListStatus[2].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListStatus[2].DoneAt.ToString());
            Assert.AreEqual(aCase3.id, caseListStatus[2].Id);
            Assert.AreEqual(aCase3.microting_uid, caseListStatus[2].MicrotingUId);
            Assert.AreEqual(aCase3.site.microting_uid, caseListStatus[2].SiteId);
            Assert.AreEqual(aCase3.site.name, caseListStatus[2].SiteName);
            Assert.AreEqual(aCase3.status, caseListStatus[2].Status);
            Assert.AreEqual(aCase3.check_list_id, caseListStatus[2].TemplatId);
            Assert.AreEqual(aCase3.unit.microting_uid, caseListStatus[2].UnitId);
            Assert.AreEqual(c3_ua.ToString(), caseListStatus[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.version, caseListStatus[2].Version);
            Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListStatus[2].WorkerName);
            Assert.AreEqual(aCase3.workflow_state, caseListStatus[2].WorkflowState);

            #endregion

            #region caseListStatus aCase4

            Assert.AreEqual(aCase4.type, caseListStatus[3].CaseType);
            Assert.AreEqual(aCase4.case_uid, caseListStatus[3].CaseUId);
            Assert.AreEqual(aCase4.microting_check_uid, caseListStatus[3].CheckUIid);
            Assert.AreEqual(c4_ca.ToString(), caseListStatus[3].CreatedAt.ToString());
            Assert.AreEqual(aCase4.custom, caseListStatus[3].Custom);
            Assert.AreEqual(c4_da.ToString(), caseListStatus[3].DoneAt.ToString());
            Assert.AreEqual(aCase4.id, caseListStatus[3].Id);
            Assert.AreEqual(aCase4.microting_uid, caseListStatus[3].MicrotingUId);
            Assert.AreEqual(aCase4.site.microting_uid, caseListStatus[3].SiteId);
            Assert.AreEqual(aCase4.site.name, caseListStatus[3].SiteName);
            Assert.AreEqual(aCase4.status, caseListStatus[3].Status);
            Assert.AreEqual(aCase4.check_list_id, caseListStatus[3].TemplatId);
            Assert.AreEqual(aCase4.unit.microting_uid, caseListStatus[3].UnitId);
            Assert.AreEqual(c4_ua.ToString(), caseListStatus[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase4.version, caseListStatus[3].Version);
            Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListStatus[3].WorkerName);
            Assert.AreEqual(aCase4.workflow_state, caseListStatus[3].WorkflowState);

            #endregion

            #endregion

            #region caseListUnitId Def Sort Asc

            #region caseListUnitId aCase1
            Assert.NotNull(caseListUnitId);
            Assert.AreEqual(8, caseListUnitId.Count);
            Assert.AreEqual(aCase1.type, caseListUnitId[0].CaseType);
            Assert.AreEqual(aCase1.case_uid, caseListUnitId[0].CaseUId);
            Assert.AreEqual(aCase1.microting_check_uid, caseListUnitId[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.custom, caseListUnitId[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.id, caseListUnitId[0].Id);
            Assert.AreEqual(aCase1.microting_uid, caseListUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase1.site.microting_uid, caseListUnitId[0].SiteId);
            Assert.AreEqual(aCase1.site.name, caseListUnitId[0].SiteName);
            Assert.AreEqual(aCase1.status, caseListUnitId[0].Status);
            Assert.AreEqual(aCase1.check_list_id, caseListUnitId[0].TemplatId);
            Assert.AreEqual(aCase1.unit.microting_uid, caseListUnitId[0].UnitId);
            Assert.AreEqual(c1_ua.ToString(), caseListUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.version, caseListUnitId[0].Version);
            Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListUnitId[0].WorkerName);
            Assert.AreEqual(aCase1.workflow_state, caseListUnitId[0].WorkflowState);

            #endregion

            #region caseListUnitId aCase2
            Assert.AreEqual(aCase2.type, caseListUnitId[1].CaseType);
            Assert.AreEqual(aCase2.case_uid, caseListUnitId[1].CaseUId);
            Assert.AreEqual(aCase2.microting_check_uid, caseListUnitId[1].CheckUIid);
            Assert.AreEqual(c2_ca.ToString(), caseListUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase2.custom, caseListUnitId[1].Custom);
            Assert.AreEqual(c2_da.ToString(), caseListUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase2.id, caseListUnitId[1].Id);
            Assert.AreEqual(aCase2.microting_uid, caseListUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase2.site.microting_uid, caseListUnitId[1].SiteId);
            Assert.AreEqual(aCase2.site.name, caseListUnitId[1].SiteName);
            Assert.AreEqual(aCase2.status, caseListUnitId[1].Status);
            Assert.AreEqual(aCase2.check_list_id, caseListUnitId[1].TemplatId);
            Assert.AreEqual(aCase2.unit.microting_uid, caseListUnitId[1].UnitId);
            Assert.AreEqual(c2_ua.ToString(), caseListUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase2.version, caseListUnitId[1].Version);
            Assert.AreEqual(aCase2.worker.first_name + " " + aCase2.worker.last_name, caseListUnitId[1].WorkerName);
            Assert.AreEqual(aCase2.workflow_state, caseListUnitId[1].WorkflowState);

            #endregion

            #region caseListUnitId aCase3
            Assert.AreEqual(aCase3.type, caseListUnitId[2].CaseType);
            Assert.AreEqual(aCase3.case_uid, caseListUnitId[2].CaseUId);
            Assert.AreEqual(aCase3.microting_check_uid, caseListUnitId[2].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListUnitId[2].CreatedAt.ToString());
            Assert.AreEqual(aCase3.custom, caseListUnitId[2].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListUnitId[2].DoneAt.ToString());
            Assert.AreEqual(aCase3.id, caseListUnitId[2].Id);
            Assert.AreEqual(aCase3.microting_uid, caseListUnitId[2].MicrotingUId);
            Assert.AreEqual(aCase3.site.microting_uid, caseListUnitId[2].SiteId);
            Assert.AreEqual(aCase3.site.name, caseListUnitId[2].SiteName);
            Assert.AreEqual(aCase3.status, caseListUnitId[2].Status);
            Assert.AreEqual(aCase3.check_list_id, caseListUnitId[2].TemplatId);
            Assert.AreEqual(aCase3.unit.microting_uid, caseListUnitId[2].UnitId);
            Assert.AreEqual(c3_ua.ToString(), caseListUnitId[2].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.version, caseListUnitId[2].Version);
            Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListUnitId[2].WorkerName);
            Assert.AreEqual(aCase3.workflow_state, caseListUnitId[2].WorkflowState);

            #endregion

            #region caseListUnitId aCase4
            Assert.AreEqual(aCase4.type, caseListUnitId[3].CaseType);
            Assert.AreEqual(aCase4.case_uid, caseListUnitId[3].CaseUId);
            Assert.AreEqual(aCase4.microting_check_uid, caseListUnitId[3].CheckUIid);
            Assert.AreEqual(c4_ca.ToString(), caseListUnitId[3].CreatedAt.ToString());
            Assert.AreEqual(aCase4.custom, caseListUnitId[3].Custom);
            Assert.AreEqual(c4_da.ToString(), caseListUnitId[3].DoneAt.ToString());
            Assert.AreEqual(aCase4.id, caseListUnitId[3].Id);
            Assert.AreEqual(aCase4.microting_uid, caseListUnitId[3].MicrotingUId);
            Assert.AreEqual(aCase4.site.microting_uid, caseListUnitId[3].SiteId);
            Assert.AreEqual(aCase4.site.name, caseListUnitId[3].SiteName);
            Assert.AreEqual(aCase4.status, caseListUnitId[3].Status);
            Assert.AreEqual(aCase4.check_list_id, caseListUnitId[3].TemplatId);
            Assert.AreEqual(aCase4.unit.microting_uid, caseListUnitId[3].UnitId);
            Assert.AreEqual(c4_ua.ToString(), caseListUnitId[3].UpdatedAt.ToString());
            Assert.AreEqual(aCase4.version, caseListUnitId[3].Version);
            Assert.AreEqual(aCase4.worker.first_name + " " + aCase4.worker.last_name, caseListUnitId[3].WorkerName);
            Assert.AreEqual(aCase4.workflow_state, caseListUnitId[3].WorkflowState);

            #endregion

            #endregion

            #endregion



            #region Def Sort Asc w. DateTime


            #region caseListDtDoneAt Def Sort Asc w. DateTime
            #region caseListDtDoneAt aCase1
            Assert.NotNull(caseListDtDoneAt);
            Assert.AreEqual(4, caseListDtDoneAt.Count);
            Assert.AreEqual(aCase1.type, caseListDtDoneAt[0].CaseType);
            Assert.AreEqual(aCase1.case_uid, caseListDtDoneAt[0].CaseUId);
            Assert.AreEqual(aCase1.microting_check_uid, caseListDtDoneAt[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDtDoneAt[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.custom, caseListDtDoneAt[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDtDoneAt[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.id, caseListDtDoneAt[0].Id);
            Assert.AreEqual(aCase1.microting_uid, caseListDtDoneAt[0].MicrotingUId);
            Assert.AreEqual(aCase1.site.microting_uid, caseListDtDoneAt[0].SiteId);
            Assert.AreEqual(aCase1.site.name, caseListDtDoneAt[0].SiteName);
            Assert.AreEqual(aCase1.status, caseListDtDoneAt[0].Status);
            Assert.AreEqual(cl1.id, caseListDtDoneAt[0].TemplatId);
            Assert.AreEqual(aCase1.unit.microting_uid, caseListDtDoneAt[0].UnitId);
            Assert.AreEqual(c1_ua.ToString(), caseListDtDoneAt[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.version, caseListDtDoneAt[0].Version);
            Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListDtDoneAt[0].WorkerName);
            Assert.AreEqual(aCase1.workflow_state, caseListDtDoneAt[0].WorkflowState);
            #endregion

            #region caseListDtDoneAt aCase3
            Assert.AreEqual(aCase3.type, caseListDtDoneAt[1].CaseType);
            Assert.AreEqual(aCase3.case_uid, caseListDtDoneAt[1].CaseUId);
            Assert.AreEqual(aCase3.microting_check_uid, caseListDtDoneAt[1].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDtDoneAt[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3.custom, caseListDtDoneAt[1].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDtDoneAt[1].DoneAt.ToString());
            Assert.AreEqual(aCase3.id, caseListDtDoneAt[1].Id);
            Assert.AreEqual(aCase3.microting_uid, caseListDtDoneAt[1].MicrotingUId);
            Assert.AreEqual(aCase3.site.microting_uid, caseListDtDoneAt[1].SiteId);
            Assert.AreEqual(aCase3.site.name, caseListDtDoneAt[1].SiteName);
            Assert.AreEqual(aCase3.status, caseListDtDoneAt[1].Status);
            Assert.AreEqual(cl1.id, caseListDtDoneAt[1].TemplatId);
            Assert.AreEqual(aCase3.unit.microting_uid, caseListDtDoneAt[1].UnitId);
            Assert.AreEqual(c3_ua.ToString(), caseListDtDoneAt[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.version, caseListDtDoneAt[1].Version);
            Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListDtDoneAt[1].WorkerName);
            Assert.AreEqual(aCase3.workflow_state, caseListDtDoneAt[1].WorkflowState);
            #endregion


            #endregion

            #region caseListDtStatus Def Sort Asc w. DateTime

            #region caseListDtStatus aCase1

            Assert.NotNull(caseListDtStatus);
            Assert.AreEqual(4, caseListDtStatus.Count);
            Assert.AreEqual(aCase1.type, caseListDtStatus[0].CaseType);
            Assert.AreEqual(aCase1.case_uid, caseListDtStatus[0].CaseUId);
            Assert.AreEqual(aCase1.microting_check_uid, caseListDtStatus[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDtStatus[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.custom, caseListDtStatus[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDtStatus[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.id, caseListDtStatus[0].Id);
            Assert.AreEqual(aCase1.microting_uid, caseListDtStatus[0].MicrotingUId);
            Assert.AreEqual(aCase1.site.microting_uid, caseListDtStatus[0].SiteId);
            Assert.AreEqual(aCase1.site.name, caseListDtStatus[0].SiteName);
            Assert.AreEqual(aCase1.status, caseListDtStatus[0].Status);
            Assert.AreEqual(aCase1.check_list_id, caseListDtStatus[0].TemplatId);
            Assert.AreEqual(aCase1.unit.microting_uid, caseListDtStatus[0].UnitId);
            Assert.AreEqual(c1_ua.ToString(), caseListDtStatus[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.version, caseListDtStatus[0].Version);
            Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListDtStatus[0].WorkerName);
            Assert.AreEqual(aCase1.workflow_state, caseListDtStatus[0].WorkflowState);

            #endregion

            #region caseListDtStatus aCase3

            Assert.AreEqual(aCase3.type, caseListDtStatus[1].CaseType);
            Assert.AreEqual(aCase3.case_uid, caseListDtStatus[1].CaseUId);
            Assert.AreEqual(aCase3.microting_check_uid, caseListDtStatus[1].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDtStatus[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3.custom, caseListDtStatus[1].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDtStatus[1].DoneAt.ToString());
            Assert.AreEqual(aCase3.id, caseListDtStatus[1].Id);
            Assert.AreEqual(aCase3.microting_uid, caseListDtStatus[1].MicrotingUId);
            Assert.AreEqual(aCase3.site.microting_uid, caseListDtStatus[1].SiteId);
            Assert.AreEqual(aCase3.site.name, caseListDtStatus[1].SiteName);
            Assert.AreEqual(aCase3.status, caseListDtStatus[1].Status);
            Assert.AreEqual(aCase3.check_list_id, caseListDtStatus[1].TemplatId);
            Assert.AreEqual(aCase3.unit.microting_uid, caseListDtStatus[1].UnitId);
            Assert.AreEqual(c3_ua.ToString(), caseListDtStatus[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.version, caseListDtStatus[1].Version);
            Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListDtStatus[1].WorkerName);
            Assert.AreEqual(aCase3.workflow_state, caseListDtStatus[1].WorkflowState);

            #endregion

            #endregion

            #region caseListDtUnitId Def Sort Asc w. DateTime

            #region caseListDtUnitId aCase1

            Assert.NotNull(caseListDtUnitId);
            Assert.AreEqual(4, caseListDtUnitId.Count);
            Assert.AreEqual(aCase1.type, caseListDtUnitId[0].CaseType);
            Assert.AreEqual(aCase1.case_uid, caseListDtUnitId[0].CaseUId);
            Assert.AreEqual(aCase1.microting_check_uid, caseListDtUnitId[0].CheckUIid);
            Assert.AreEqual(c1_ca.ToString(), caseListDtUnitId[0].CreatedAt.ToString());
            Assert.AreEqual(aCase1.custom, caseListDtUnitId[0].Custom);
            Assert.AreEqual(c1_da.ToString(), caseListDtUnitId[0].DoneAt.ToString());
            Assert.AreEqual(aCase1.id, caseListDtUnitId[0].Id);
            Assert.AreEqual(aCase1.microting_uid, caseListDtUnitId[0].MicrotingUId);
            Assert.AreEqual(aCase1.site.microting_uid, caseListDtUnitId[0].SiteId);
            Assert.AreEqual(aCase1.site.name, caseListDtUnitId[0].SiteName);
            Assert.AreEqual(aCase1.status, caseListDtUnitId[0].Status);
            Assert.AreEqual(aCase1.check_list_id, caseListDtUnitId[0].TemplatId);
            Assert.AreEqual(aCase1.unit.microting_uid, caseListDtUnitId[0].UnitId);
            Assert.AreEqual(c1_ua.ToString(), caseListDtUnitId[0].UpdatedAt.ToString());
            Assert.AreEqual(aCase1.version, caseListDtUnitId[0].Version);
            Assert.AreEqual(aCase1.worker.first_name + " " + aCase1.worker.last_name, caseListDtUnitId[0].WorkerName);
            Assert.AreEqual(aCase1.workflow_state, caseListDtUnitId[0].WorkflowState);

            #endregion

            #region caseListDtUnitId aCase3

            Assert.AreEqual(aCase3.type, caseListDtUnitId[1].CaseType);
            Assert.AreEqual(aCase3.case_uid, caseListDtUnitId[1].CaseUId);
            Assert.AreEqual(aCase3.microting_check_uid, caseListDtUnitId[1].CheckUIid);
            Assert.AreEqual(c3_ca.ToString(), caseListDtUnitId[1].CreatedAt.ToString());
            Assert.AreEqual(aCase3.custom, caseListDtUnitId[1].Custom);
            Assert.AreEqual(c3_da.ToString(), caseListDtUnitId[1].DoneAt.ToString());
            Assert.AreEqual(aCase3.id, caseListDtUnitId[1].Id);
            Assert.AreEqual(aCase3.microting_uid, caseListDtUnitId[1].MicrotingUId);
            Assert.AreEqual(aCase3.site.microting_uid, caseListDtUnitId[1].SiteId);
            Assert.AreEqual(aCase3.site.name, caseListDtUnitId[1].SiteName);
            Assert.AreEqual(aCase3.status, caseListDtUnitId[1].Status);
            Assert.AreEqual(aCase3.check_list_id, caseListDtUnitId[1].TemplatId);
            Assert.AreEqual(aCase3.unit.microting_uid, caseListDtUnitId[1].UnitId);
            Assert.AreEqual(c3_ua.ToString(), caseListDtUnitId[1].UpdatedAt.ToString());
            Assert.AreEqual(aCase3.version, caseListDtUnitId[1].Version);
            Assert.AreEqual(aCase3.worker.first_name + " " + aCase3.worker.last_name, caseListDtUnitId[1].WorkerName);
            Assert.AreEqual(aCase3.workflow_state, caseListDtUnitId[1].WorkflowState);

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
            // Assert.AreEqual(aCase1.id, caseListC1SortDoneAt[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.id, caseListC1SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.id, caseListC1SortDoneAt[0].UnitId);
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
            // Assert.AreEqual(aCase1.id, caseListC1SortStatus[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortStatus[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortStatus[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortStatus[0].Status);
            // Assert.AreEqual(cl1.id, caseListC1SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.id, caseListC1SortStatus[0].UnitId);
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
            // Assert.AreEqual(aCase1.id, caseListC1SortUnitId[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortUnitId[0].Status);
            // Assert.AreEqual(cl1.id, caseListC1SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.id, caseListC1SortUnitId[0].UnitId);
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
            // Assert.AreEqual(aCase2.id, caseListC2SortDoneAt[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.id, caseListC2SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.id, caseListC2SortDoneAt[0].UnitId);
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
            // Assert.AreEqual(aCase2.id, caseListC2SortStatus[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortStatus[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortStatus[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortStatus[0].Status);
            // Assert.AreEqual(cl1.id, caseListC2SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.id, caseListC2SortStatus[0].UnitId);
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
            // Assert.AreEqual(aCase2.id, caseListC2SortUnitId[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortUnitId[0].Status);
            // Assert.AreEqual(cl1.id, caseListC2SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.id, caseListC2SortUnitId[0].UnitId);
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
            // Assert.AreEqual(aCase3.id, caseListC3SortDoneAt[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.id, caseListC3SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.id, caseListC3SortDoneAt[0].UnitId);
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
            // Assert.AreEqual(aCase3.id, caseListC3SortStatus[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortStatus[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortStatus[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortStatus[0].Status);
            // Assert.AreEqual(cl1.id, caseListC3SortStatus[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.id, caseListC3SortStatus[0].UnitId);
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
            // Assert.AreEqual(aCase3.id, caseListC3SortUnitId[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortUnitId[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortUnitId[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortUnitId[0].Status);
            // Assert.AreEqual(cl1.id, caseListC3SortUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.id, caseListC3SortUnitId[0].UnitId);
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
            // Assert.AreEqual(aCase4.id, caseListC4SortDoneAt[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.id, caseListC4SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.id, caseListC4SortDoneAt[0].UnitId);
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
            // Assert.AreEqual(aCase4.id, caseListC1SortDoneAt[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC1SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC1SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC1SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC1SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.id, caseListC1SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.id, caseListC1SortDoneAt[0].UnitId);
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
            // Assert.AreEqual(aCase4.id, caseListC4SortDoneAt[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDoneAt[0].Status);
            // Assert.AreEqual(cl1.id, caseListC4SortDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.id, caseListC4SortDoneAt[0].UnitId);
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
            // Assert.AreEqual(aCase1.id, caseListC1SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.id, caseListC1SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.id, caseListC1SortDtDoneAt[0].UnitId);
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
            // Assert.AreEqual(aCase1.id, caseListC1SortDtStatus[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.id, caseListC1SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.id, caseListC1SortDtStatus[0].UnitId);
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
            // Assert.AreEqual(aCase1.id, caseListC1SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase1.microting_uid, caseListC1SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase1.site.microting_uid, caseListC1SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase1.site.name, caseListC1SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase1.status, caseListC1SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.id, caseListC1SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase1.unit.id, caseListC1SortDtUnitId[0].UnitId);
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
            // Assert.AreEqual(aCase2.id, caseListC2SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.id, caseListC2SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.id, caseListC2SortDtDoneAt[0].UnitId);
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
            // Assert.AreEqual(aCase2.id, caseListC2SortDtStatus[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.id, caseListC2SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.id, caseListC2SortDtStatus[0].UnitId);
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
            // Assert.AreEqual(aCase2.id, caseListC2SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase2.microting_uid, caseListC2SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase2.site.microting_uid, caseListC2SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase2.site.name, caseListC2SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase2.status, caseListC2SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.id, caseListC2SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase2.unit.id, caseListC2SortDtUnitId[0].UnitId);
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
            // Assert.AreEqual(aCase3.id, caseListC3SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.id, caseListC3SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.id, caseListC3SortDtDoneAt[0].UnitId);
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
            // Assert.AreEqual(aCase3.id, caseListC3SortDtStatus[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.id, caseListC3SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.id, caseListC3SortDtStatus[0].UnitId);
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
            // Assert.AreEqual(aCase3.id, caseListC3SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase3.microting_uid, caseListC3SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase3.site.microting_uid, caseListC3SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase3.site.name, caseListC3SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase3.status, caseListC3SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.id, caseListC3SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase3.unit.id, caseListC3SortDtUnitId[0].UnitId);
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
            // Assert.AreEqual(aCase4.id, caseListC4SortDtDoneAt[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDtDoneAt[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDtDoneAt[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDtDoneAt[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDtDoneAt[0].Status);
            // Assert.AreEqual(cl1.id, caseListC4SortDtDoneAt[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.id, caseListC4SortDtDoneAt[0].UnitId);
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
            // Assert.AreEqual(aCase4.id, caseListC4SortDtStatus[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDtStatus[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDtStatus[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDtStatus[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDtStatus[0].Status);
            // Assert.AreEqual(cl1.id, caseListC4SortDtStatus[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.id, caseListC4SortDtStatus[0].UnitId);
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
            // Assert.AreEqual(aCase4.id, caseListC4SortDtUnitId[0].Id);
            // Assert.AreEqual(aCase4.microting_uid, caseListC4SortDtUnitId[0].MicrotingUId);
            // Assert.AreEqual(aCase4.site.microting_uid, caseListC4SortDtUnitId[0].SiteId);
            // Assert.AreEqual(aCase4.site.name, caseListC4SortDtUnitId[0].SiteName);
            // Assert.AreEqual(aCase4.status, caseListC4SortDtUnitId[0].Status);
            // Assert.AreEqual(cl1.id, caseListC4SortDtUnitId[0].TemplatId);
            // Assert.AreEqual(aCase4.unit.id, caseListC4SortDtUnitId[0].UnitId);
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