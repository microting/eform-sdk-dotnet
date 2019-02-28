using eFormCore;
using eFormCore.Helpers;
using eFormData;
using eFormShared;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class CoreTestCase : DbTestFixture
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
            sut.SetSdkSetting(Settings.fileLocationPicture, path + @"\output\dataFolder\picture\");
            sut.SetSdkSetting(Settings.fileLocationPdf, path + @"\output\dataFolder\pdf\");
            sut.SetSdkSetting(Settings.fileLocationJasper, path + @"\output\dataFolder\reports\");
            testHelpers = new TestHelpers();
            //sut.StartLog(new CoreBase());
        }

        #region case
        [Test]
        public void Core_Case_CaseDeleteResult_DoesMarkCaseRemoved()
        {

            // Arrance
            sites site = new sites();
            site.name = "SiteName";
            site.microting_uid = 1234;
            DbContext.sites.Add(site);
            DbContext.SaveChanges();

            check_lists cl = new check_lists();
            cl.label = "label";

            DbContext.check_lists.Add(cl);
            DbContext.SaveChanges();

            cases aCase = new cases();
            aCase.microting_uid = "microting_uid";
            aCase.microting_check_uid = "microting_check_uid";
            aCase.workflow_state = Constants.WorkflowStates.Created;
            aCase.check_list_id = cl.id;
            aCase.site_id = site.id;

            DbContext.cases.Add(aCase);
            DbContext.SaveChanges();

            // Act
            sut.CaseDeleteResult(aCase.id);
            Case_Dto theCase = sut.CaseLookupCaseId(aCase.id);

            // Assert
            Assert.NotNull(theCase);
            Assert.AreEqual(Constants.WorkflowStates.Removed, theCase.WorkflowState);
        }

        [Test]//needs http mock done
        public void Core_Case_CaseCheck_ChecksCase()
        {

        }
        [Test]
        public void Core_Case_CaseRead_ReadsCase()
        {
            // Arrange
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

            #region Case1

            cases aCase = testHelpers.CreateCase("caseUId", cl1, DateTime.Now, "custom", DateTime.Now,
                worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Check List Values
            check_list_values check_List_Values = testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);
            //    new check_list_values();

            //check_List_Values.case_id = aCase.id;
            //check_List_Values.check_list_id = cl2.id;
            //check_List_Values.created_at = DateTime.Now;
            //check_List_Values.status = "completed";
            //check_List_Values.updated_at = DateTime.Now;
            //check_List_Values.user_id = null;
            //check_List_Values.version = 865;
            //check_List_Values.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.check_list_values.Add(check_List_Values);
            //DbContext.SaveChanges();

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase, cl2, f1, null, null, "tomt1", 61234, worker);
            //    new field_values();
            //field_Values1.case_id = aCase.id;
            //field_Values1.check_list = cl2;
            //field_Values1.check_list_id = cl2.id;
            //field_Values1.created_at = DateTime.Now;
            //field_Values1.date = DateTime.Now;
            //field_Values1.done_at = DateTime.Now;
            //field_Values1.field = f1;
            //field_Values1.field_id = f1.id;
            //field_Values1.updated_at = DateTime.Now;
            //field_Values1.user_id = null;
            //field_Values1.value = "tomt1";
            //field_Values1.version = 61234;
            //field_Values1.worker = worker;
            //field_Values1.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values1);
            //DbContext.SaveChanges();
            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);
            //    new field_values();
            //field_Values2.case_id = aCase.id;
            //field_Values2.check_list = cl2;
            //field_Values2.check_list_id = cl2.id;
            //field_Values2.created_at = DateTime.Now;
            //field_Values2.date = DateTime.Now;
            //field_Values2.done_at = DateTime.Now;
            //field_Values2.field = f2;
            //field_Values2.field_id = f2.id;
            //field_Values2.updated_at = DateTime.Now;
            //field_Values2.user_id = null;
            //field_Values2.value = "tomt2";
            //field_Values2.version = 61234;
            //field_Values2.worker = worker;
            //field_Values2.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values2);
            //DbContext.SaveChanges();
            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);
            //    new field_values();
            //field_Values3.case_id = aCase.id;
            //field_Values3.check_list = cl2;
            //field_Values3.check_list_id = cl2.id;
            //field_Values3.created_at = DateTime.Now;
            //field_Values3.date = DateTime.Now;
            //field_Values3.done_at = DateTime.Now;
            //field_Values3.field = f3;
            //field_Values3.field_id = f3.id;
            //field_Values3.updated_at = DateTime.Now;
            //field_Values3.user_id = null;
            //field_Values3.value = "tomt3";
            //field_Values3.version = 61234;
            //field_Values3.worker = worker;
            //field_Values3.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values3);
            //DbContext.SaveChanges();
            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);
            //    new field_values();
            //field_Values4.case_id = aCase.id;
            //field_Values4.check_list = cl2;
            //field_Values4.check_list_id = cl2.id;
            //field_Values4.created_at = DateTime.Now;
            //field_Values4.date = DateTime.Now;
            //field_Values4.done_at = DateTime.Now;
            //field_Values4.field = f4;
            //field_Values4.field_id = f4.id;
            //field_Values4.updated_at = DateTime.Now;
            //field_Values4.user_id = null;
            //field_Values4.value = "tomt4";
            //field_Values4.version = 61234;
            //field_Values4.worker = worker;
            //field_Values4.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values4);
            //DbContext.SaveChanges();
            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);
            //    new field_values();
            //field_Values5.case_id = aCase.id;
            //field_Values5.check_list = cl2;
            //field_Values5.check_list_id = cl2.id;
            //field_Values5.created_at = DateTime.Now;
            //field_Values5.date = DateTime.Now;
            //field_Values5.done_at = DateTime.Now;
            //field_Values5.field = f5;
            //field_Values5.field_id = f5.id;
            //field_Values5.updated_at = DateTime.Now;
            //field_Values5.user_id = null;
            //field_Values5.value = "tomt5";
            //field_Values5.version = 61234;
            //field_Values5.worker = worker;
            //field_Values5.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.field_values.Add(field_Values5);
            //DbContext.SaveChanges();
            #endregion


            #endregion
            #endregion

            // Act

            var match = sut.CaseRead(aCase.microting_uid, aCase.microting_check_uid);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match.CaseType, aCase.type);


           
        }
        [Test]
        public void Core_Case_CaseReadByCaseId_Returns_cDto()
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

            #region Case1

            cases aCase = testHelpers.CreateCase("caseUId", cl1, DateTime.Now, "custom", DateTime.Now,
                worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);

            #endregion


            #endregion


            // Act

            var match = sut.CaseReadByCaseId(aCase.id);

            // Assert

            Assert.AreEqual(aCase.id, match.CaseId);
        }
        [Test]
        public void Core_Case_CaseReadFirstId()
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

            #region Case1

            cases aCase = testHelpers.CreateCase("caseUId", cl1, DateTime.Now, "custom", DateTime.Now,
                worker, "microtingCheckUId", "microtingUId",
               site, 100, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);

            #endregion


            #endregion
            // Act
            var match = sut.CaseReadFirstId(aCase.check_list.id, aCase.workflow_state);
            // Assert
            Assert.AreEqual(aCase.id, match);
        }
        [Test]
        public void Core_Case_CaseUpdate_ReturnsTrue()
        {
            // Arrange
            #region Arrance
            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates
            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl3 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl4 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl5 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl6 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl7 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl8 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl9 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl10 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl11 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion
            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
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

            cases aCase1 = testHelpers.CreateCase("case1UId", cl2, c1_ca, "custom1",
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

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = testHelpers.CreateUploadedData("checksum1", "File1", "no", "hjgjghjhg", "File1", 1, worker,
                "local", 55, false);
            #endregion

            #region ud2
            uploaded_data ud2 = testHelpers.CreateUploadedData("checksum2", "File1", "no", "hjgjghjhg", "File2", 1, worker,
                "local", 55, false);
            #endregion

            #region ud3
            uploaded_data ud3 = testHelpers.CreateUploadedData("checksum3", "File1", "no", "hjgjghjhg", "File3", 1, worker,
                "local", 55, false);
            #endregion

            #region ud4
            uploaded_data ud4 = testHelpers.CreateUploadedData("checksum4", "File1", "no", "hjgjghjhg", "File4", 1, worker,
                "local", 55, false);
            #endregion

            #region ud5
            uploaded_data ud5 = testHelpers.CreateUploadedData("checksum5", "File1", "no", "hjgjghjhg", "File5", 1, worker,
                "local", 55, false);
            #endregion

            #region ud6
            uploaded_data ud6 = testHelpers.CreateUploadedData("checksum6", "File1", "no", "hjgjghjhg", "File6", 1, worker,
                "local", 55, false);
            #endregion

            #region ud7
            uploaded_data ud7 = testHelpers.CreateUploadedData("checksum7", "File1", "no", "hjgjghjhg", "File7", 1, worker,
                "local", 55, false);
            #endregion

            #region ud8
            uploaded_data ud8 = testHelpers.CreateUploadedData("checksum8", "File1", "no", "hjgjghjhg", "File8", 1, worker,
                "local", 55, false);
            #endregion

            #region ud9
            uploaded_data ud9 = testHelpers.CreateUploadedData("checksum9", "File1", "no", "hjgjghjhg", "File9", 1, worker,
                "local", 55, false);
            #endregion

            #region ud10
            uploaded_data ud10 = testHelpers.CreateUploadedData("checksum10", "File1", "no", "hjgjghjhg", "File10", 1, worker,
                "local", 55, false);
            #endregion

            #endregion

            #region Check List Values
            #region clv1
            check_list_values clv1 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
            #endregion

            #region clv2
            check_list_values clv2 = testHelpers.CreateCheckListValue(aCase1, cl3, Constants.CheckListValues.Checked, null, 861);
            #endregion

            #region clv3
            check_list_values clv3 = testHelpers.CreateCheckListValue(aCase1, cl4, Constants.CheckListValues.Checked, null, 862);
            #endregion

            #region clv4
            check_list_values clv4 = testHelpers.CreateCheckListValue(aCase1, cl5, Constants.CheckListValues.NotChecked, null, 863);
            #endregion

            #region clv5
            check_list_values clv5 = testHelpers.CreateCheckListValue(aCase1, cl6, Constants.CheckListValues.NotChecked, null, 864);
            #endregion

            #region clv6
            check_list_values clv6 = testHelpers.CreateCheckListValue(aCase1, cl7, Constants.CheckListValues.NotChecked, null, 865);
            #endregion

            #region clv7
            check_list_values clv7 = testHelpers.CreateCheckListValue(aCase1, cl8, Constants.CheckListValues.NotApproved, null, 866);
            #endregion

            #region clv8
            check_list_values clv8 = testHelpers.CreateCheckListValue(aCase1, cl9, Constants.CheckListValues.NotApproved, null, 867);
            #endregion

            #region clv9
            check_list_values clv9 = testHelpers.CreateCheckListValue(aCase1, cl10, Constants.CheckListValues.NotApproved, null, 868);
            #endregion

            #region clv10
            check_list_values clv10 = testHelpers.CreateCheckListValue(aCase1, cl11, Constants.CheckListValues.NotApproved, null, 869);
            #endregion

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.id, null, "tomt1", 61230, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.id, null, "tomt2", 61231, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.id, null, "tomt3", 61232, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.id, null, "tomt4", 61233, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion

            #region fv6
            field_values field_Value6 = testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.id, null, "tomt6", 61235, worker);

            #endregion

            #region fv7
            field_values field_Value7 = testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.id, null, "tomt7", 61236, worker);

            #endregion

            #region fv8
            field_values field_Value8 = testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.id, null, "tomt8", 61237, worker);

            #endregion

            #region fv9
            field_values field_Value9 = testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.id, null, "tomt9", 61238, worker);

            #endregion

            #region fv10
            field_values field_Value10 = testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.id, null, "tomt10", 61239, worker);

            #endregion


            #endregion
            #endregion
            // Act
            List<string> FVPlist = new List<string>();
            FVPlist.Add(field_Value1.id + " |" + field_Value1.value);
            FVPlist.Add(field_Value2.id + " |" + field_Value2.value);
            FVPlist.Add(field_Value3.id + " |" + field_Value3.value);
            FVPlist.Add(field_Value4.id + " |" + field_Value4.value);
            FVPlist.Add(field_Value5.id + " |" + field_Value5.value);
            FVPlist.Add(field_Value6.id + " |" + field_Value6.value);
            FVPlist.Add(field_Value7.id + " |" + field_Value7.value);
            FVPlist.Add(field_Value8.id + " |" + field_Value8.value);
            FVPlist.Add(field_Value9.id + " |" + field_Value9.value);
            FVPlist.Add(field_Value10.id + " |" + field_Value10.value);
            //FVPlist.ToList();

            List<string> CLVlist = new List<string>();
            CLVlist.Add(clv1.check_list_id + " |" + clv1.status);
            CLVlist.Add(clv2.check_list_id + " |" + clv2.status);
            CLVlist.Add(clv3.check_list_id + " |" + clv3.status);
            CLVlist.Add(clv4.check_list_id + " |" + clv4.status);
            CLVlist.Add(clv5.check_list_id + " |" + clv5.status);
            CLVlist.Add(clv6.check_list_id + " |" + clv6.status);
            CLVlist.Add(clv7.check_list_id + " |" + clv7.status);
            CLVlist.Add(clv8.check_list_id + " |" + clv8.status);
            CLVlist.Add(clv9.check_list_id + " |" + clv9.status);
            CLVlist.Add(clv10.check_list_id + " |" + clv10.status);
            //CLVlist.ToList();

            var match = sut.CaseUpdate(aCase1.id, FVPlist, CLVlist);

            Assert.NotNull(match);
            Assert.True(match);
        }
        [Test]//skal bruge communicator, mangler mock.
        public void Core_Case_CaseDelete_ReturnsTrue()
        {
            //// Arrange
            //#region Arrance
            //#region Template1
            //DateTime cl1_Ca = DateTime.Now;
            //DateTime cl1_Ua = DateTime.Now;
            //check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            //#endregion

            //#region subtemplates
            //#region SubTemplate1
            //check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl3 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl4 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl5 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl6 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl7 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl8 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl9 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl10 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl11 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion
            //#endregion

            //#region Fields
            //#region field1


            //fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
            //    5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
            //    0, 0, "", 49);

            //#endregion

            //#region field2


            //fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
            //    45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
            //    "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            //#endregion

            //#region field3

            //fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
            //    83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
            //    "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            //#endregion

            //#region field4


            //fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
            //    84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
            //    "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#region field5

            //fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
            //    85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
            //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#region field6

            //fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
            //    86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
            //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#region field7

            //fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
            //    87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
            //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#region field8

            //fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
            //    88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
            //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#region field9

            //fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
            //    89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
            //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#region field10

            //fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
            //    90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
            //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#endregion

            //#region Worker

            //workers worker = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            //#endregion

            //#region site
            //sites site = testHelpers.CreateSite("SiteName", 88);

            //#endregion

            //#region units
            //units unit = testHelpers.CreateUnit(48, 49, site, 348);

            //#endregion

            //#region site_workers
            //site_workers site_workers = testHelpers.CreateSiteWorker(55, site, worker);

            //#endregion

            //#region cases
            //#region cases created
            //#region Case1

            //DateTime c1_ca = DateTime.Now.AddDays(-9);
            //DateTime c1_da = DateTime.Now.AddDays(-8).AddHours(-12);
            //DateTime c1_ua = DateTime.Now.AddDays(-8);

            //cases aCase1 = testHelpers.CreateCase("case1UId", cl2, c1_ca, "custom1",
            //    c1_da, worker, "microtingCheckUId1", "microtingUId1",
            //   site, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);

            //#endregion


            //#endregion

            //#endregion

            //#region UploadedData
            //#region ud1
            //uploaded_data ud1 = testHelpers.CreateUploadedData("checksum1", "File1", "no", "hjgjghjhg", "File1", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud2
            //uploaded_data ud2 = testHelpers.CreateUploadedData("checksum2", "File1", "no", "hjgjghjhg", "File2", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud3
            //uploaded_data ud3 = testHelpers.CreateUploadedData("checksum3", "File1", "no", "hjgjghjhg", "File3", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud4
            //uploaded_data ud4 = testHelpers.CreateUploadedData("checksum4", "File1", "no", "hjgjghjhg", "File4", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud5
            //uploaded_data ud5 = testHelpers.CreateUploadedData("checksum5", "File1", "no", "hjgjghjhg", "File5", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud6
            //uploaded_data ud6 = testHelpers.CreateUploadedData("checksum6", "File1", "no", "hjgjghjhg", "File6", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud7
            //uploaded_data ud7 = testHelpers.CreateUploadedData("checksum7", "File1", "no", "hjgjghjhg", "File7", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud8
            //uploaded_data ud8 = testHelpers.CreateUploadedData("checksum8", "File1", "no", "hjgjghjhg", "File8", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud9
            //uploaded_data ud9 = testHelpers.CreateUploadedData("checksum9", "File1", "no", "hjgjghjhg", "File9", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud10
            //uploaded_data ud10 = testHelpers.CreateUploadedData("checksum10", "File1", "no", "hjgjghjhg", "File10", 1, worker,
            //    "local", 55);
            //#endregion

            //#endregion

            //#region Check List Values
            //#region clv1
            //check_list_values clv1 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
            //#endregion

            //#region clv2
            //check_list_values clv2 = testHelpers.CreateCheckListValue(aCase1, cl3, Constants.CheckListValues.Checked, null, 861);
            //#endregion

            //#region clv3
            //check_list_values clv3 = testHelpers.CreateCheckListValue(aCase1, cl4, Constants.CheckListValues.Checked, null, 862);
            //#endregion

            //#region clv4
            //check_list_values clv4 = testHelpers.CreateCheckListValue(aCase1, cl5, Constants.CheckListValues.NotChecked, null, 863);
            //#endregion

            //#region clv5
            //check_list_values clv5 = testHelpers.CreateCheckListValue(aCase1, cl6, Constants.CheckListValues.NotChecked, null, 864);
            //#endregion

            //#region clv6
            //check_list_values clv6 = testHelpers.CreateCheckListValue(aCase1, cl7, Constants.CheckListValues.NotChecked, null, 865);
            //#endregion

            //#region clv7
            //check_list_values clv7 = testHelpers.CreateCheckListValue(aCase1, cl8, Constants.CheckListValues.NotApproved, null, 866);
            //#endregion

            //#region clv8
            //check_list_values clv8 = testHelpers.CreateCheckListValue(aCase1, cl9, Constants.CheckListValues.NotApproved, null, 867);
            //#endregion

            //#region clv9
            //check_list_values clv9 = testHelpers.CreateCheckListValue(aCase1, cl10, Constants.CheckListValues.NotApproved, null, 868);
            //#endregion

            //#region clv10
            //check_list_values clv10 = testHelpers.CreateCheckListValue(aCase1, cl11, Constants.CheckListValues.NotApproved, null, 869);
            //#endregion

            //#endregion

            //#region Field Values
            //#region fv1
            //field_values field_Value1 = testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.id, null, "tomt1", 61230, worker);

            //#endregion

            //#region fv2
            //field_values field_Value2 = testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.id, null, "tomt2", 61231, worker);

            //#endregion

            //#region fv3
            //field_values field_Value3 = testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.id, null, "tomt3", 61232, worker);

            //#endregion

            //#region fv4
            //field_values field_Value4 = testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.id, null, "tomt4", 61233, worker);

            //#endregion

            //#region fv5
            //field_values field_Value5 = testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            //#endregion

            //#region fv6
            //field_values field_Value6 = testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.id, null, "tomt6", 61235, worker);

            //#endregion

            //#region fv7
            //field_values field_Value7 = testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.id, null, "tomt7", 61236, worker);

            //#endregion

            //#region fv8
            //field_values field_Value8 = testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.id, null, "tomt8", 61237, worker);

            //#endregion

            //#region fv9
            //field_values field_Value9 = testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.id, null, "tomt9", 61238, worker);

            //#endregion

            //#region fv10
            //field_values field_Value10 = testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.id, null, "tomt10", 61239, worker);

            //#endregion


            //#endregion

            //#region checkListSites
            // DateTime cls_ca = DateTime.Now;
            // DateTime cls_ua = DateTime.Now;
            // check_list_sites cls1 = testHelpers.CreateCheckListSite(cl2, cls_ca, site,
            //    cls_ua, 5, Constants.WorkflowStates.Removed);

            //#endregion
            //#endregion
            //// Act
            //var match = sut.CaseDelete(cl2.id,(int) cls1.site.microting_uid);
            //// Assert
            // Assert.NotNull(match);
            // Assert.True(match);

        }
        [Test]//skal bruge communicator, mangler mock.
        public void Core_Case_CaseDelete2_ReturnsTrue()
        {
            //// Arrange
            //#region Arrance
            //#region Template1
            //DateTime cl1_Ca = DateTime.Now;
            //DateTime cl1_Ua = DateTime.Now;
            //check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            //#endregion

            //#region subtemplates
            //#region SubTemplate1
            //check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl3 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl4 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl5 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl6 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl7 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl8 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl9 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl10 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion

            //#region SubTemplate1
            //check_lists cl11 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            //#endregion
            //#endregion

            //#region Fields
            //#region field1


            //fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
            //    5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
            //    0, 0, "", 49);

            //#endregion

            //#region field2


            //fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
            //    45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
            //    "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            //#endregion

            //#region field3

            //fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
            //    83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
            //    "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            //#endregion

            //#region field4


            //fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
            //    84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
            //    "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#region field5

            //fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
            //    85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
            //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#region field6

            //fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
            //    86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
            //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#region field7

            //fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
            //    87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
            //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#region field8

            //fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
            //    88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
            //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#region field9

            //fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
            //    89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
            //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#region field10

            //fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
            //    90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
            //    "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            //#endregion

            //#endregion

            //#region Worker

            //workers worker = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            //#endregion

            //#region site
            //sites site = testHelpers.CreateSite("SiteName", 88);

            //#endregion

            //#region units
            //units unit = testHelpers.CreateUnit(48, 49, site, 348);

            //#endregion

            //#region site_workers
            //site_workers site_workers = testHelpers.CreateSiteWorker(55, site, worker);

            //#endregion

            //#region cases
            //#region cases created
            //#region Case1

            //DateTime c1_ca = DateTime.Now.AddDays(-9);
            //DateTime c1_da = DateTime.Now.AddDays(-8).AddHours(-12);
            //DateTime c1_ua = DateTime.Now.AddDays(-8);

            //cases aCase1 = testHelpers.CreateCase("case1UId", cl2, c1_ca, "custom1",
            //    c1_da, worker, "microtingCheckUId1", "microtingUId1",
            //   site, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);

            //#endregion


            //#endregion

            //#endregion

            //#region UploadedData
            //#region ud1
            //uploaded_data ud1 = testHelpers.CreateUploadedData("checksum1", "File1", "no", "hjgjghjhg", "File1", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud2
            //uploaded_data ud2 = testHelpers.CreateUploadedData("checksum2", "File1", "no", "hjgjghjhg", "File2", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud3
            //uploaded_data ud3 = testHelpers.CreateUploadedData("checksum3", "File1", "no", "hjgjghjhg", "File3", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud4
            //uploaded_data ud4 = testHelpers.CreateUploadedData("checksum4", "File1", "no", "hjgjghjhg", "File4", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud5
            //uploaded_data ud5 = testHelpers.CreateUploadedData("checksum5", "File1", "no", "hjgjghjhg", "File5", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud6
            //uploaded_data ud6 = testHelpers.CreateUploadedData("checksum6", "File1", "no", "hjgjghjhg", "File6", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud7
            //uploaded_data ud7 = testHelpers.CreateUploadedData("checksum7", "File1", "no", "hjgjghjhg", "File7", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud8
            //uploaded_data ud8 = testHelpers.CreateUploadedData("checksum8", "File1", "no", "hjgjghjhg", "File8", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud9
            //uploaded_data ud9 = testHelpers.CreateUploadedData("checksum9", "File1", "no", "hjgjghjhg", "File9", 1, worker,
            //    "local", 55);
            //#endregion

            //#region ud10
            //uploaded_data ud10 = testHelpers.CreateUploadedData("checksum10", "File1", "no", "hjgjghjhg", "File10", 1, worker,
            //    "local", 55);
            //#endregion

            //#endregion

            //#region Check List Values
            //#region clv1
            //check_list_values clv1 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
            //#endregion

            //#region clv2
            //check_list_values clv2 = testHelpers.CreateCheckListValue(aCase1, cl3, Constants.CheckListValues.Checked, null, 861);
            //#endregion

            //#region clv3
            //check_list_values clv3 = testHelpers.CreateCheckListValue(aCase1, cl4, Constants.CheckListValues.Checked, null, 862);
            //#endregion

            //#region clv4
            //check_list_values clv4 = testHelpers.CreateCheckListValue(aCase1, cl5, Constants.CheckListValues.NotChecked, null, 863);
            //#endregion

            //#region clv5
            //check_list_values clv5 = testHelpers.CreateCheckListValue(aCase1, cl6, Constants.CheckListValues.NotChecked, null, 864);
            //#endregion

            //#region clv6
            //check_list_values clv6 = testHelpers.CreateCheckListValue(aCase1, cl7, Constants.CheckListValues.NotChecked, null, 865);
            //#endregion

            //#region clv7
            //check_list_values clv7 = testHelpers.CreateCheckListValue(aCase1, cl8, Constants.CheckListValues.NotApproved, null, 866);
            //#endregion

            //#region clv8
            //check_list_values clv8 = testHelpers.CreateCheckListValue(aCase1, cl9, Constants.CheckListValues.NotApproved, null, 867);
            //#endregion

            //#region clv9
            //check_list_values clv9 = testHelpers.CreateCheckListValue(aCase1, cl10, Constants.CheckListValues.NotApproved, null, 868);
            //#endregion

            //#region clv10
            //check_list_values clv10 = testHelpers.CreateCheckListValue(aCase1, cl11, Constants.CheckListValues.NotApproved, null, 869);
            //#endregion

            //#endregion

            //#region Field Values
            //#region fv1
            //field_values field_Value1 = testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.id, null, "tomt1", 61230, worker);

            //#endregion

            //#region fv2
            //field_values field_Value2 = testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.id, null, "tomt2", 61231, worker);

            //#endregion

            //#region fv3
            //field_values field_Value3 = testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.id, null, "tomt3", 61232, worker);

            //#endregion

            //#region fv4
            //field_values field_Value4 = testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.id, null, "tomt4", 61233, worker);

            //#endregion

            //#region fv5
            //field_values field_Value5 = testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            //#endregion

            //#region fv6
            //field_values field_Value6 = testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.id, null, "tomt6", 61235, worker);

            //#endregion

            //#region fv7
            //field_values field_Value7 = testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.id, null, "tomt7", 61236, worker);

            //#endregion

            //#region fv8
            //field_values field_Value8 = testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.id, null, "tomt8", 61237, worker);

            //#endregion

            //#region fv9
            //field_values field_Value9 = testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.id, null, "tomt9", 61238, worker);

            //#endregion

            //#region fv10
            //field_values field_Value10 = testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.id, null, "tomt10", 61239, worker);

            //#endregion


            //#endregion

            //#region checkListSites
            //DateTime cls_ca = DateTime.Now;
            //DateTime cls_ua = DateTime.Now;
            //check_list_sites cls1 = testHelpers.CreateCheckListSite(cl2, cls_ca, site,
            //   cls_ua, 5, Constants.WorkflowStates.Created);

            //#endregion
            //#endregion
            //// Act
            //var match = sut.CaseDelete(cl2.id, (int)cls1.site.microting_uid, Constants.WorkflowStates.Created);
            //// Assert
            // Assert.NotNull(match);
            // Assert.True(match);

        }
        [Test]
        public void Core_Case_CaseUpdateFieldValues()
        {


            // Arrance
            #region Arrance
            #region Template1
            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

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

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
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

            cases aCase1 = testHelpers.CreateCase("case1UId", cl2, c1_ca, "custom1",
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



            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = testHelpers.CreateUploadedData("checksum1", "File1", "no", "hjgjghjhg", "File1", 1, worker,
                "local", 55, false);
            #endregion

            #region ud2
            uploaded_data ud2 = testHelpers.CreateUploadedData("checksum2", "File1", "no", "hjgjghjhg", "File2", 1, worker,
                "local", 55, false);
            #endregion

            #region ud3
            uploaded_data ud3 = testHelpers.CreateUploadedData("checksum3", "File1", "no", "hjgjghjhg", "File3", 1, worker,
                "local", 55, false);
            #endregion

            #region ud4
            uploaded_data ud4 = testHelpers.CreateUploadedData("checksum4", "File1", "no", "hjgjghjhg", "File4", 1, worker,
                "local", 55, false);
            #endregion

            #region ud5
            uploaded_data ud5 = testHelpers.CreateUploadedData("checksum5", "File1", "no", "hjgjghjhg", "File5", 1, worker,
                "local", 55, false);
            #endregion

            #region ud6
            uploaded_data ud6 = testHelpers.CreateUploadedData("checksum6", "File1", "no", "hjgjghjhg", "File6", 1, worker,
                "local", 55, false);
            #endregion

            #region ud7
            uploaded_data ud7 = testHelpers.CreateUploadedData("checksum7", "File1", "no", "hjgjghjhg", "File7", 1, worker,
                "local", 55, false);
            #endregion

            #region ud8
            uploaded_data ud8 = testHelpers.CreateUploadedData("checksum8", "File1", "no", "hjgjghjhg", "File8", 1, worker,
                "local", 55, false);
            #endregion

            #region ud9
            uploaded_data ud9 = testHelpers.CreateUploadedData("checksum9", "File1", "no", "hjgjghjhg", "File9", 1, worker,
                "local", 55, false);
            #endregion

            #region ud10
            uploaded_data ud10 = testHelpers.CreateUploadedData("checksum10", "File1", "no", "hjgjghjhg", "File10", 1, worker,
                "local", 55, false);
            #endregion

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.id, null, "tomt2", 61234, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.id, null, "tomt3", 61234, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.id, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion

            #region fv6
            field_values field_Value6 = testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.id, null, "tomt6", 61234, worker);

            #endregion

            #region fv7
            field_values field_Value7 = testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.id, null, "tomt7", 61234, worker);

            #endregion

            #region fv8
            field_values field_Value8 = testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.id, null, "tomt8", 61234, worker);

            #endregion

            #region fv9
            field_values field_Value9 = testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.id, null, "tomt9", 61234, worker);

            #endregion

            #region fv10
            field_values field_Value10 = testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.id, null, "tomt10", 61234, worker);

            #endregion


            #endregion
            #endregion
            // Act
            cases theCase = DbContext.cases.First();
            Assert.NotNull(theCase);
            check_lists theCheckList = DbContext.check_lists.First();

            theCheckList.field_1 = f1.id;
            theCheckList.field_2 = f2.id;
            theCheckList.field_3 = f3.id;
            theCheckList.field_4 = f4.id;
            theCheckList.field_5 = f5.id;
            theCheckList.field_6 = f6.id;
            theCheckList.field_7 = f7.id;
            theCheckList.field_8 = f8.id;
            theCheckList.field_9 = f9.id;
            theCheckList.field_10 = f10.id;

            Assert.AreEqual(null, theCase.field_value_1);
            Assert.AreEqual(null, theCase.field_value_2);
            Assert.AreEqual(null, theCase.field_value_3);
            Assert.AreEqual(null, theCase.field_value_4);
            Assert.AreEqual(null, theCase.field_value_5);
            Assert.AreEqual(null, theCase.field_value_6);
            Assert.AreEqual(null, theCase.field_value_7);
            Assert.AreEqual(null, theCase.field_value_8);
            Assert.AreEqual(null, theCase.field_value_9);
            Assert.AreEqual(null, theCase.field_value_10);

            var testThis = sut.CaseUpdateFieldValues(aCase1.id);

            // Assert
            cases theCaseAfter = DbContext.cases.AsNoTracking().First();

            Assert.NotNull(theCaseAfter);

            theCaseAfter.field_value_1 = field_Value1.value;
            theCaseAfter.field_value_2 = field_Value2.value;
            theCaseAfter.field_value_3 = field_Value3.value;
            theCaseAfter.field_value_4 = field_Value4.value;
            theCaseAfter.field_value_5 = field_Value5.value;
            theCaseAfter.field_value_6 = field_Value6.value;
            theCaseAfter.field_value_7 = field_Value7.value;
            theCaseAfter.field_value_8 = field_Value8.value;
            theCaseAfter.field_value_9 = field_Value9.value;
            theCaseAfter.field_value_10 = field_Value10.value;


            Assert.True(testThis);

            Assert.AreEqual("tomt1", theCaseAfter.field_value_1);
            Assert.AreEqual("tomt2", theCaseAfter.field_value_2);
            Assert.AreEqual("tomt3", theCaseAfter.field_value_3);
            Assert.AreEqual("tomt4", theCaseAfter.field_value_4);
            Assert.AreEqual("tomt5", theCaseAfter.field_value_5);
            Assert.AreEqual("tomt6", theCaseAfter.field_value_6);
            Assert.AreEqual("tomt7", theCaseAfter.field_value_7);
            Assert.AreEqual("tomt8", theCaseAfter.field_value_8);
            Assert.AreEqual("tomt9", theCaseAfter.field_value_9);
            Assert.AreEqual("tomt10", theCaseAfter.field_value_10);

        }
        [Test]
        public void Core_Case_CaseLookupMUId_Returns_ReturnCase()
        {
            // Arrance
            #region Arrance
            #region Template1
            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

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

            #region Case1

            cases aCase = testHelpers.CreateCase("caseUId", cl1, DateTime.Now, "custom", DateTime.Now,
                worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = testHelpers.CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File1", 1, worker,
                "local", 55, false);
            #endregion

            #region ud2
            uploaded_data ud2 = testHelpers.CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File2", 1, worker,
                "local", 55, false);
            #endregion

            #region ud3
            uploaded_data ud3 = testHelpers.CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File3", 1, worker,
                "local", 55, false);
            #endregion

            #region ud4
            uploaded_data ud4 = testHelpers.CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File4", 1, worker,
                "local", 55, false);
            #endregion

            #region ud5
            uploaded_data ud5 = testHelpers.CreateUploadedData("checksum", "File1", "no", "hjgjghjhg", "File5", 1, worker,
                "local", 55, false);
            #endregion

            #endregion

            #region Check List Values
            check_list_values check_List_Values = testHelpers.CreateCheckListValue(aCase, cl2, "checked", null, 865);


            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase, cl2, f1, ud1.id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase, cl2, f2, ud2.id, null, "tomt2", 61234, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase, cl2, f3, ud3.id, null, "tomt3", 61234, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase, cl2, f4, ud4.id, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion


            #endregion
            #endregion


            // Act

            var match = sut.CaseLookupMUId(aCase.microting_uid);

            // Assert

             Assert.AreEqual(aCase.microting_uid, match.MicrotingUId);
           
         

        } 
        [Test]
        public void Core_Case_CaseLookupCaseId_Returns_cDto()
        {
            // Arrance
            #region Arrance
            #region Template1
            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

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

            #region Case1

            cases aCase = testHelpers.CreateCase("caseUId", cl1, DateTime.Now, "custom", DateTime.Now,
                worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);

            #endregion


            #endregion


            // Act

            var match = sut.CaseLookupCaseId(aCase.id);

            // Assert

            Assert.AreEqual(aCase.id, match.CaseId);
        }
        [Test]
        public void Core_Case_CaseLookupCaseUId_Returns_lstDto()
        {
            // Arrance
            #region Arrance
            #region Template1
            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

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

            #region Case1

            cases aCase = testHelpers.CreateCase("caseUId", cl1, DateTime.Now, "custom", DateTime.Now,
                worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);

            #endregion


            #endregion


            // Act

            var match = sut.CaseLookupCaseUId(aCase.case_uid);


            // Assert

            Assert.AreEqual(aCase.case_uid, match[0].CaseUId);
        }
        [Test]
        public void Core_Case_CaseIdLookUp_returnsId()
        {

            // Arrange
            #region Arrance
            #region Template1
            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

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

            #endregion

            #endregion
            // Act
            var match = sut.CaseIdLookup(aCase1.microting_uid, aCase1.microting_check_uid);
            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(aCase1.id, match);
        }
        [Test]
        public void Core_Case_CasesToExcel_returnsPathAndName()
        {
            // Arrange
            #region Arrance
            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates
            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl3 = testHelpers.CreateSubTemplate("A.2", "D.2", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl4 = testHelpers.CreateSubTemplate("A.3", "D.3", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl5 = testHelpers.CreateSubTemplate("A.4", "D.4", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl6 = testHelpers.CreateSubTemplate("A.5", "D.5", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl7 = testHelpers.CreateSubTemplate("A.6", "D.6", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl8 = testHelpers.CreateSubTemplate("A.7", "D.7", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl9 = testHelpers.CreateSubTemplate("A.8", "D.8", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl10 = testHelpers.CreateSubTemplate("A.9", "D.9", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl11 = testHelpers.CreateSubTemplate("A.10", "D.10", "CheckList", 1, 1, cl1);


            #endregion
            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
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
            cases aCase2 = testHelpers.CreateCase("case2UId", cl3, c2_ca, "custom2",
             c2_da, worker, "microtingCheck2UId", "microting2UId",
               site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case3
            DateTime c3_ca = DateTime.Now.AddDays(-10);
            DateTime c3_da = DateTime.Now.AddDays(-9).AddHours(-12);
            DateTime c3_ua = DateTime.Now.AddDays(-9);

            cases aCase3 = testHelpers.CreateCase("case3UId", cl4, c3_ca, "custom3",
              c3_da, worker, "microtingCheck3UId", "microtin3gUId",
               site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case4
            DateTime c4_ca = DateTime.Now.AddDays(-8);
            DateTime c4_da = DateTime.Now.AddDays(-7).AddHours(-12);
            DateTime c4_ua = DateTime.Now.AddDays(-7);

            cases aCase4 = testHelpers.CreateCase("case4UId", cl5, c4_ca, "custom4",
                c4_da, worker, "microtingCheck4UId", "microting4UId",
               site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion
            #endregion

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = testHelpers.CreateUploadedData("checksum1", "File1", "no", @"C:\Users\soipi\Desktop", "File1", 1, worker,
                "local", 55, false);
            #endregion

            #region ud2
            uploaded_data ud2 = testHelpers.CreateUploadedData("checksum2", "File1", "no", @"C:\Users\soipi\Desktop", "File2", 1, worker,
                "local", 55, false);
            #endregion

            #region ud3
            uploaded_data ud3 = testHelpers.CreateUploadedData("checksum3", "File1", "no", @"C:\Users\soipi\Desktop", "File3", 1, worker,
                "local", 55, false);
            #endregion

            #region ud4
            uploaded_data ud4 = testHelpers.CreateUploadedData("checksum4", "File1", "no", @"C: \Users\soipi\Desktop", "File4", 1, worker,
                "local", 55, false);
            #endregion

            #region ud5
            uploaded_data ud5 = testHelpers.CreateUploadedData("checksum5", "File1", "no", @"C:\Users\soipi\Desktop", "File5", 1, worker,
                "local", 55, false);
            #endregion

            #region ud6
            uploaded_data ud6 = testHelpers.CreateUploadedData("checksum6", "File1", "no", @"C:\Users\soipi\Desktop", "File6", 1, worker,
                "local", 55, false);
            #endregion

            #region ud7
            uploaded_data ud7 = testHelpers.CreateUploadedData("checksum7", "File1", "no", @"C:\Users\soipi\Desktop", "File7", 1, worker,
                "local", 55, false);
            #endregion

            #region ud8
            uploaded_data ud8 = testHelpers.CreateUploadedData("checksum8", "File1", "no", @"C:\Users\soipi\Desktop", "File8", 1, worker,
                "local", 55, false);
            #endregion

            #region ud9
            uploaded_data ud9 = testHelpers.CreateUploadedData("checksum9", "File1", "no", @"C:\Users\soipi\Desktop", "File9", 1, worker,
                "local", 55, false);
            #endregion

            #region ud10
            uploaded_data ud10 = testHelpers.CreateUploadedData("checksum10", "File1", "no", @"C:\Users\soipi\Desktop", "File10", 1, worker,
                "local", 55, false);
            #endregion

            #endregion

            #region Check List Values
            #region clv1
            check_list_values clv1 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
            #endregion

            #region clv2
            check_list_values clv2 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 861);
            #endregion

            #region clv3
            check_list_values clv3 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 862);
            #endregion

            #region clv4
            check_list_values clv4 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 863);
            #endregion

            #region clv5
            check_list_values clv5 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 864);
            #endregion

            #region clv6
            check_list_values clv6 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 865);
            #endregion

            #region clv7
            check_list_values clv7 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 866);
            #endregion

            #region clv8
            check_list_values clv8 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 867);
            #endregion

            #region clv9
            check_list_values clv9 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 868);
            #endregion

            #region clv10
            check_list_values clv10 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 869);
            #endregion

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.id, null, "tomt1", 61230, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.id, null, "tomt2", 61231, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.id, null, "tomt3", 61232, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.id, null, "tomt4", 61233, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion

            #region fv6
            field_values field_Value6 = testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.id, null, "tomt6", 61235, worker);

            #endregion

            #region fv7
            field_values field_Value7 = testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.id, null, "tomt7", 61236, worker);

            #endregion

            #region fv8
            field_values field_Value8 = testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.id, null, "tomt8", 61237, worker);

            #endregion

            #region fv9
            field_values field_Value9 = testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.id, null, "tomt9", 61238, worker);

            #endregion

            #region fv10
            field_values field_Value10 = testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.id, null, "tomt10", 61239, worker);

            #endregion


            #endregion

            #region checkListSites
            DateTime cls_ca = DateTime.Now;
            DateTime cls_ua = DateTime.Now;
            string microtingUid = Guid.NewGuid().ToString();
            check_list_sites cls1 = testHelpers.CreateCheckListSite(cl2, cls_ca, site,
               cls_ua, 5, Constants.WorkflowStates.Created, microtingUid);

            #endregion
            #endregion
            // Act

            //var match = sut.CasesToExcel(aCase1.check_list_id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(1), ud1.file_location + ud1.file_name, "mappe/");

            //// Assert
            // Assert.NotNull(match);
            // Assert.AreEqual(match, "C:\\Users\\soipi\\DesktopFile1.xlsx");


        }
        [Test]
        public void Core_Case_CasesToCsv_returnsPathAndName()
        {
            // Arrange
            #region Arrance
            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates
            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl3 = testHelpers.CreateSubTemplate("A.2", "D.2", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl4 = testHelpers.CreateSubTemplate("A.3", "D.3", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl5 = testHelpers.CreateSubTemplate("A.4", "D.4", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl6 = testHelpers.CreateSubTemplate("A.5", "D.5", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl7 = testHelpers.CreateSubTemplate("A.6", "D.6", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl8 = testHelpers.CreateSubTemplate("A.7", "D.7", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl9 = testHelpers.CreateSubTemplate("A.8", "D.8", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl10 = testHelpers.CreateSubTemplate("A.9", "D.9", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl11 = testHelpers.CreateSubTemplate("A.10", "D.10", "CheckList", 1, 1, cl1);


            #endregion
            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
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
            cases aCase2 = testHelpers.CreateCase("case2UId", cl3, c2_ca, "custom2",
             c2_da, worker, "microtingCheck2UId", "microting2UId",
               site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case3
            DateTime c3_ca = DateTime.Now.AddDays(-10);
            DateTime c3_da = DateTime.Now.AddDays(-9).AddHours(-12);
            DateTime c3_ua = DateTime.Now.AddDays(-9);

            cases aCase3 = testHelpers.CreateCase("case3UId", cl4, c3_ca, "custom3",
              c3_da, worker, "microtingCheck3UId", "microtin3gUId",
               site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case4
            DateTime c4_ca = DateTime.Now.AddDays(-8);
            DateTime c4_da = DateTime.Now.AddDays(-7).AddHours(-12);
            DateTime c4_ua = DateTime.Now.AddDays(-7);

            cases aCase4 = testHelpers.CreateCase("case4UId", cl5, c4_ca, "custom4",
                c4_da, worker, "microtingCheck4UId", "microting4UId",
               site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion
            #endregion

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = testHelpers.CreateUploadedData("checksum1", "File1", "no", @"C:\Users\soipi\Desktop", "File1", 1, worker,
                "local", 55, false);
            #endregion

            #region ud2
            uploaded_data ud2 = testHelpers.CreateUploadedData("checksum2", "File1", "no", @"C:\Users\soipi\Desktop", "File2", 1, worker,
                "local", 55, false);
            #endregion

            #region ud3
            uploaded_data ud3 = testHelpers.CreateUploadedData("checksum3", "File1", "no", @"C:\Users\soipi\Desktop", "File3", 1, worker,
                "local", 55, false);
            #endregion

            #region ud4
            uploaded_data ud4 = testHelpers.CreateUploadedData("checksum4", "File1", "no", @"C: \Users\soipi\Desktop", "File4", 1, worker,
                "local", 55, false);
            #endregion

            #region ud5
            uploaded_data ud5 = testHelpers.CreateUploadedData("checksum5", "File1", "no", @"C:\Users\soipi\Desktop", "File5", 1, worker,
                "local", 55, false);
            #endregion

            #region ud6
            uploaded_data ud6 = testHelpers.CreateUploadedData("checksum6", "File1", "no", @"C:\Users\soipi\Desktop", "File6", 1, worker,
                "local", 55, false);
            #endregion

            #region ud7
            uploaded_data ud7 = testHelpers.CreateUploadedData("checksum7", "File1", "no", @"C:\Users\soipi\Desktop", "File7", 1, worker,
                "local", 55, false);
            #endregion

            #region ud8
            uploaded_data ud8 = testHelpers.CreateUploadedData("checksum8", "File1", "no", @"C:\Users\soipi\Desktop", "File8", 1, worker,
                "local", 55, false);
            #endregion

            #region ud9
            uploaded_data ud9 = testHelpers.CreateUploadedData("checksum9", "File1", "no", @"C:\Users\soipi\Desktop", "File9", 1, worker,
                "local", 55, false);
            #endregion

            #region ud10
            uploaded_data ud10 = testHelpers.CreateUploadedData("checksum10", "File1", "no", @"C:\Users\soipi\Desktop", "File10", 1, worker,
                "local", 55, false);
            #endregion

            #endregion

            #region Check List Values
            #region clv1
            check_list_values clv1 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
            #endregion

            #region clv2
            check_list_values clv2 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 861);
            #endregion

            #region clv3
            check_list_values clv3 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 862);
            #endregion

            #region clv4
            check_list_values clv4 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 863);
            #endregion

            #region clv5
            check_list_values clv5 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 864);
            #endregion

            #region clv6
            check_list_values clv6 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 865);
            #endregion

            #region clv7
            check_list_values clv7 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 866);
            #endregion

            #region clv8
            check_list_values clv8 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 867);
            #endregion

            #region clv9
            check_list_values clv9 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 868);
            #endregion

            #region clv10
            check_list_values clv10 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 869);
            #endregion

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.id, null, "tomt1", 61230, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.id, null, "tomt2", 61231, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.id, null, "tomt3", 61232, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.id, null, "tomt4", 61233, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion

            #region fv6
            field_values field_Value6 = testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.id, null, "tomt6", 61235, worker);

            #endregion

            #region fv7
            field_values field_Value7 = testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.id, null, "tomt7", 61236, worker);

            #endregion

            #region fv8
            field_values field_Value8 = testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.id, null, "tomt8", 61237, worker);

            #endregion

            #region fv9
            field_values field_Value9 = testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.id, null, "tomt9", 61238, worker);

            #endregion

            #region fv10
            field_values field_Value10 = testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.id, null, "tomt10", 61239, worker);

            #endregion


            #endregion

            #region checkListSites
            DateTime cls_ca = DateTime.Now;
            DateTime cls_ua = DateTime.Now;
            string microtingUid = Guid.NewGuid().ToString();
            check_list_sites cls1 = testHelpers.CreateCheckListSite(cl2, cls_ca, site,
               cls_ua, 5, Constants.WorkflowStates.Created, microtingUid);

            #endregion
            #endregion
            // Act

            //var match = sut.CasesToCsv(aCase1.check_list_id, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(1), ud1.file_location + ud1.file_name, "mappe/");

            // Assert
            // Assert.NotNull(match);
            // Assert.AreEqual(match, "C:\\Users\\soipi\\DesktopFile1.csv");


        }
        [Test]
        public void Core_Case_CaseToJasperXml_ReturnsPath()
        {
            // Arrange
            #region Arrance
            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates
            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl3 = testHelpers.CreateSubTemplate("A.2", "D.2", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl4 = testHelpers.CreateSubTemplate("A.3", "D.3", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl5 = testHelpers.CreateSubTemplate("A.4", "D.4", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl6 = testHelpers.CreateSubTemplate("A.5", "D.5", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl7 = testHelpers.CreateSubTemplate("A.6", "D.6", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl8 = testHelpers.CreateSubTemplate("A.7", "D.7", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl9 = testHelpers.CreateSubTemplate("A.8", "D.8", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl10 = testHelpers.CreateSubTemplate("A.9", "D.9", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl11 = testHelpers.CreateSubTemplate("A.10", "D.10", "CheckList", 1, 1, cl1);


            #endregion
            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
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
            cases aCase2 = testHelpers.CreateCase("case2UId", cl3, c2_ca, "custom2",
             c2_da, worker, "microtingCheck2UId", "microting2UId",
               site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case3
            DateTime c3_ca = DateTime.Now.AddDays(-10);
            DateTime c3_da = DateTime.Now.AddDays(-9).AddHours(-12);
            DateTime c3_ua = DateTime.Now.AddDays(-9);

            cases aCase3 = testHelpers.CreateCase("case3UId", cl4, c3_ca, "custom3",
              c3_da, worker, "microtingCheck3UId", "microtin3gUId",
               site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case4
            DateTime c4_ca = DateTime.Now.AddDays(-8);
            DateTime c4_da = DateTime.Now.AddDays(-7).AddHours(-12);
            DateTime c4_ua = DateTime.Now.AddDays(-7);

            cases aCase4 = testHelpers.CreateCase("case4UId", cl5, c4_ca, "custom4",
                c4_da, worker, "microtingCheck4UId", "microting4UId",
               site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion
            #endregion

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = testHelpers.CreateUploadedData("checksum1", "File1", "no", @"C:\Users\soipi\Desktop", "File1", 1, worker,
                "local", 55, false);
            #endregion

            #region ud2
            uploaded_data ud2 = testHelpers.CreateUploadedData("checksum2", "File1", "no", @"C:\Users\soipi\Desktop", "File2", 1, worker,
                "local", 55, false);
            #endregion

            #region ud3
            uploaded_data ud3 = testHelpers.CreateUploadedData("checksum3", "File1", "no", @"C:\Users\soipi\Desktop", "File3", 1, worker,
                "local", 55, false);
            #endregion

            #region ud4
            uploaded_data ud4 = testHelpers.CreateUploadedData("checksum4", "File1", "no", @"C: \Users\soipi\Desktop", "File4", 1, worker,
                "local", 55, false);
            #endregion

            #region ud5
            uploaded_data ud5 = testHelpers.CreateUploadedData("checksum5", "File1", "no", @"C:\Users\soipi\Desktop", "File5", 1, worker,
                "local", 55, false);
            #endregion

            #region ud6
            uploaded_data ud6 = testHelpers.CreateUploadedData("checksum6", "File1", "no", @"C:\Users\soipi\Desktop", "File6", 1, worker,
                "local", 55, false);
            #endregion

            #region ud7
            uploaded_data ud7 = testHelpers.CreateUploadedData("checksum7", "File1", "no", @"C:\Users\soipi\Desktop", "File7", 1, worker,
                "local", 55, false);
            #endregion

            #region ud8
            uploaded_data ud8 = testHelpers.CreateUploadedData("checksum8", "File1", "no", @"C:\Users\soipi\Desktop", "File8", 1, worker,
                "local", 55, false);
            #endregion

            #region ud9
            uploaded_data ud9 = testHelpers.CreateUploadedData("checksum9", "File1", "no", @"C:\Users\soipi\Desktop", "File9", 1, worker,
                "local", 55, false);
            #endregion

            #region ud10
            uploaded_data ud10 = testHelpers.CreateUploadedData("checksum10", "File1", "no", @"C:\Users\soipi\Desktop", "File10", 1, worker,
                "local", 55, false);
            #endregion

            #endregion

            #region Check List Values
            #region clv1
            check_list_values clv1 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
            #endregion

            #region clv2
            check_list_values clv2 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 861);
            #endregion

            #region clv3
            check_list_values clv3 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 862);
            #endregion

            #region clv4
            check_list_values clv4 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 863);
            #endregion

            #region clv5
            check_list_values clv5 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 864);
            #endregion

            #region clv6
            check_list_values clv6 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 865);
            #endregion

            #region clv7
            check_list_values clv7 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 866);
            #endregion

            #region clv8
            check_list_values clv8 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 867);
            #endregion

            #region clv9
            check_list_values clv9 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 868);
            #endregion

            #region clv10
            check_list_values clv10 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 869);
            #endregion

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.id, null, "tomt1", 61230, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.id, null, "tomt2", 61231, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.id, null, "tomt3", 61232, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.id, null, "tomt4", 61233, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion

            #region fv6
            field_values field_Value6 = testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.id, null, "tomt6", 61235, worker);

            #endregion

            #region fv7
            field_values field_Value7 = testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.id, null, "tomt7", 61236, worker);

            #endregion

            #region fv8
            field_values field_Value8 = testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.id, null, "tomt8", 61237, worker);

            #endregion

            #region fv9
            field_values field_Value9 = testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.id, null, "tomt9", 61238, worker);

            #endregion

            #region fv10
            field_values field_Value10 = testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.id, null, "tomt10", 61239, worker);

            #endregion


            #endregion

            #region checkListSites
            DateTime cls_ca = DateTime.Now;
            DateTime cls_ua = DateTime.Now;
            check_list_sites cls1 = testHelpers.CreateCheckListSite(cl2, cls_ca, site,
               cls_ua, 5, Constants.WorkflowStates.Created, "");

            #endregion
            #endregion
            // Act

            string timeStamp = DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("hhmmss");
            string pdfPath = path + @"\output\dataFolder\reports\results\" + timeStamp + "_" + aCase2.id + ".xml";
            var match = sut.CaseToJasperXml(aCase2.id, timeStamp, pdfPath);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match, pdfPath);

        }
        [Test]
        public void Core_Case_GetJasperPath_returnsPath()
        {
            // Arrange
            #region Arrance
            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates
            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl3 = testHelpers.CreateSubTemplate("A.2", "D.2", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl4 = testHelpers.CreateSubTemplate("A.3", "D.3", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl5 = testHelpers.CreateSubTemplate("A.4", "D.4", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl6 = testHelpers.CreateSubTemplate("A.5", "D.5", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl7 = testHelpers.CreateSubTemplate("A.6", "D.6", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl8 = testHelpers.CreateSubTemplate("A.7", "D.7", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl9 = testHelpers.CreateSubTemplate("A.8", "D.8", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl10 = testHelpers.CreateSubTemplate("A.9", "D.9", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl11 = testHelpers.CreateSubTemplate("A.10", "D.10", "CheckList", 1, 1, cl1);


            #endregion
            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
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
            cases aCase2 = testHelpers.CreateCase("case2UId", cl3, c2_ca, "custom2",
             c2_da, worker, "microtingCheck2UId", "microting2UId",
               site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case3
            DateTime c3_ca = DateTime.Now.AddDays(-10);
            DateTime c3_da = DateTime.Now.AddDays(-9).AddHours(-12);
            DateTime c3_ua = DateTime.Now.AddDays(-9);

            cases aCase3 = testHelpers.CreateCase("case3UId", cl4, c3_ca, "custom3",
              c3_da, worker, "microtingCheck3UId", "microtin3gUId",
               site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case4
            DateTime c4_ca = DateTime.Now.AddDays(-8);
            DateTime c4_da = DateTime.Now.AddDays(-7).AddHours(-12);
            DateTime c4_ua = DateTime.Now.AddDays(-7);

            cases aCase4 = testHelpers.CreateCase("case4UId", cl5, c4_ca, "custom4",
                c4_da, worker, "microtingCheck4UId", "microting4UId",
               site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion
            #endregion

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = testHelpers.CreateUploadedData("checksum1", "File1", "no", @"C:\Users\soipi\Desktop", "File1", 1, worker,
                "local", 55, false);
            #endregion

            #region ud2
            uploaded_data ud2 = testHelpers.CreateUploadedData("checksum2", "File1", "no", @"C:\Users\soipi\Desktop", "File2", 1, worker,
                "local", 55, false);
            #endregion

            #region ud3
            uploaded_data ud3 = testHelpers.CreateUploadedData("checksum3", "File1", "no", @"C:\Users\soipi\Desktop", "File3", 1, worker,
                "local", 55, false);
            #endregion

            #region ud4
            uploaded_data ud4 = testHelpers.CreateUploadedData("checksum4", "File1", "no", @"C: \Users\soipi\Desktop", "File4", 1, worker,
                "local", 55, false);
            #endregion

            #region ud5
            uploaded_data ud5 = testHelpers.CreateUploadedData("checksum5", "File1", "no", @"C:\Users\soipi\Desktop", "File5", 1, worker,
                "local", 55, false);
            #endregion

            #region ud6
            uploaded_data ud6 = testHelpers.CreateUploadedData("checksum6", "File1", "no", @"C:\Users\soipi\Desktop", "File6", 1, worker,
                "local", 55, false);
            #endregion

            #region ud7
            uploaded_data ud7 = testHelpers.CreateUploadedData("checksum7", "File1", "no", @"C:\Users\soipi\Desktop", "File7", 1, worker,
                "local", 55, false);
            #endregion

            #region ud8
            uploaded_data ud8 = testHelpers.CreateUploadedData("checksum8", "File1", "no", @"C:\Users\soipi\Desktop", "File8", 1, worker,
                "local", 55, false);
            #endregion

            #region ud9
            uploaded_data ud9 = testHelpers.CreateUploadedData("checksum9", "File1", "no", @"C:\Users\soipi\Desktop", "File9", 1, worker,
                "local", 55, false);
            #endregion

            #region ud10
            uploaded_data ud10 = testHelpers.CreateUploadedData("checksum10", "File1", "no", @"C:\Users\soipi\Desktop", "File10", 1, worker,
                "local", 55, false);
            #endregion

            #endregion

            #region Check List Values
            #region clv1
            check_list_values clv1 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
            #endregion

            #region clv2
            check_list_values clv2 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 861);
            #endregion

            #region clv3
            check_list_values clv3 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 862);
            #endregion

            #region clv4
            check_list_values clv4 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 863);
            #endregion

            #region clv5
            check_list_values clv5 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 864);
            #endregion

            #region clv6
            check_list_values clv6 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 865);
            #endregion

            #region clv7
            check_list_values clv7 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 866);
            #endregion

            #region clv8
            check_list_values clv8 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 867);
            #endregion

            #region clv9
            check_list_values clv9 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 868);
            #endregion

            #region clv10
            check_list_values clv10 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 869);
            #endregion

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.id, null, "tomt1", 61230, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.id, null, "tomt2", 61231, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.id, null, "tomt3", 61232, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.id, null, "tomt4", 61233, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion

            #region fv6
            field_values field_Value6 = testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.id, null, "tomt6", 61235, worker);

            #endregion

            #region fv7
            field_values field_Value7 = testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.id, null, "tomt7", 61236, worker);

            #endregion

            #region fv8
            field_values field_Value8 = testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.id, null, "tomt8", 61237, worker);

            #endregion

            #region fv9
            field_values field_Value9 = testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.id, null, "tomt9", 61238, worker);

            #endregion

            #region fv10
            field_values field_Value10 = testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.id, null, "tomt10", 61239, worker);

            #endregion


            #endregion

            #region checkListSites
            DateTime cls_ca = DateTime.Now;
            DateTime cls_ua = DateTime.Now;
            check_list_sites cls1 = testHelpers.CreateCheckListSite(cl2, cls_ca, site,
               cls_ua, 5, Constants.WorkflowStates.Created, "");

            #endregion
            #endregion
            // Act

            var match = sut.GetSdkSetting(Settings.fileLocationJasper);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match, path + @"\output\dataFolder\reports\");


        }
        [Test]
        public void Core_Case_SetJasperPath_returnsTrue()
        {

            // Arrange

            // Act
            var match = sut.SetSdkSetting(Settings.fileLocationJasper, @"C:\local\gitgud");
            // Assert
            Assert.NotNull(match);
            Assert.True(match);

        }
        [Test]
        public void Core_Case_GetPicturePath_returnsPath()
        {
            // Arrange
            #region Arrance
            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates
            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl3 = testHelpers.CreateSubTemplate("A.2", "D.2", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl4 = testHelpers.CreateSubTemplate("A.3", "D.3", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl5 = testHelpers.CreateSubTemplate("A.4", "D.4", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl6 = testHelpers.CreateSubTemplate("A.5", "D.5", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl7 = testHelpers.CreateSubTemplate("A.6", "D.6", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl8 = testHelpers.CreateSubTemplate("A.7", "D.7", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl9 = testHelpers.CreateSubTemplate("A.8", "D.8", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl10 = testHelpers.CreateSubTemplate("A.9", "D.9", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl11 = testHelpers.CreateSubTemplate("A.10", "D.10", "CheckList", 1, 1, cl1);


            #endregion
            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
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
            cases aCase2 = testHelpers.CreateCase("case2UId", cl3, c2_ca, "custom2",
             c2_da, worker, "microtingCheck2UId", "microting2UId",
               site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case3
            DateTime c3_ca = DateTime.Now.AddDays(-10);
            DateTime c3_da = DateTime.Now.AddDays(-9).AddHours(-12);
            DateTime c3_ua = DateTime.Now.AddDays(-9);

            cases aCase3 = testHelpers.CreateCase("case3UId", cl4, c3_ca, "custom3",
              c3_da, worker, "microtingCheck3UId", "microtin3gUId",
               site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case4
            DateTime c4_ca = DateTime.Now.AddDays(-8);
            DateTime c4_da = DateTime.Now.AddDays(-7).AddHours(-12);
            DateTime c4_ua = DateTime.Now.AddDays(-7);

            cases aCase4 = testHelpers.CreateCase("case4UId", cl5, c4_ca, "custom4",
                c4_da, worker, "microtingCheck4UId", "microting4UId",
               site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion
            #endregion

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = testHelpers.CreateUploadedData("checksum1", "File1", "no", @"C:\Users\soipi\Desktop", "File1", 1, worker,
                "local", 55, false);
            #endregion

            #region ud2
            uploaded_data ud2 = testHelpers.CreateUploadedData("checksum2", "File1", "no", @"C:\Users\soipi\Desktop", "File2", 1, worker,
                "local", 55, false);
            #endregion

            #region ud3
            uploaded_data ud3 = testHelpers.CreateUploadedData("checksum3", "File1", "no", @"C:\Users\soipi\Desktop", "File3", 1, worker,
                "local", 55, false);
            #endregion

            #region ud4
            uploaded_data ud4 = testHelpers.CreateUploadedData("checksum4", "File1", "no", @"C: \Users\soipi\Desktop", "File4", 1, worker,
                "local", 55, false);
            #endregion

            #region ud5
            uploaded_data ud5 = testHelpers.CreateUploadedData("checksum5", "File1", "no", @"C:\Users\soipi\Desktop", "File5", 1, worker,
                "local", 55, false);
            #endregion

            #region ud6
            uploaded_data ud6 = testHelpers.CreateUploadedData("checksum6", "File1", "no", @"C:\Users\soipi\Desktop", "File6", 1, worker,
                "local", 55, false);
            #endregion

            #region ud7
            uploaded_data ud7 = testHelpers.CreateUploadedData("checksum7", "File1", "no", @"C:\Users\soipi\Desktop", "File7", 1, worker,
                "local", 55, false);
            #endregion

            #region ud8
            uploaded_data ud8 = testHelpers.CreateUploadedData("checksum8", "File1", "no", @"C:\Users\soipi\Desktop", "File8", 1, worker,
                "local", 55, false);
            #endregion

            #region ud9
            uploaded_data ud9 = testHelpers.CreateUploadedData("checksum9", "File1", "no", @"C:\Users\soipi\Desktop", "File9", 1, worker,
                "local", 55, false);
            #endregion

            #region ud10
            uploaded_data ud10 = testHelpers.CreateUploadedData("checksum10", "File1", "no", @"C:\Users\soipi\Desktop", "File10", 1, worker,
                "local", 55, false);
            #endregion

            #endregion

            #region Check List Values
            #region clv1
            check_list_values clv1 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
            #endregion

            #region clv2
            check_list_values clv2 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 861);
            #endregion

            #region clv3
            check_list_values clv3 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 862);
            #endregion

            #region clv4
            check_list_values clv4 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 863);
            #endregion

            #region clv5
            check_list_values clv5 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 864);
            #endregion

            #region clv6
            check_list_values clv6 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 865);
            #endregion

            #region clv7
            check_list_values clv7 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 866);
            #endregion

            #region clv8
            check_list_values clv8 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 867);
            #endregion

            #region clv9
            check_list_values clv9 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 868);
            #endregion

            #region clv10
            check_list_values clv10 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 869);
            #endregion

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.id, null, "tomt1", 61230, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.id, null, "tomt2", 61231, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.id, null, "tomt3", 61232, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.id, null, "tomt4", 61233, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion

            #region fv6
            field_values field_Value6 = testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.id, null, "tomt6", 61235, worker);

            #endregion

            #region fv7
            field_values field_Value7 = testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.id, null, "tomt7", 61236, worker);

            #endregion

            #region fv8
            field_values field_Value8 = testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.id, null, "tomt8", 61237, worker);

            #endregion

            #region fv9
            field_values field_Value9 = testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.id, null, "tomt9", 61238, worker);

            #endregion

            #region fv10
            field_values field_Value10 = testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.id, null, "tomt10", 61239, worker);

            #endregion


            #endregion

            #region checkListSites
            DateTime cls_ca = DateTime.Now;
            DateTime cls_ua = DateTime.Now;
            check_list_sites cls1 = testHelpers.CreateCheckListSite(cl2, cls_ca, site,
               cls_ua, 5, Constants.WorkflowStates.Created, "");

            #endregion
            #endregion
            // Act

            var match = sut.GetSdkSetting(Settings.fileLocationPicture);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match, path + @"\output\dataFolder\picture\");

        }
        [Test]
        public void Core_Case_SetPicturePath_returnsTrue()
        {

            // Arrange

            // Act
            var match = sut.SetSdkSetting(Settings.fileLocationPicture, @"C:\local");
            // Assert
            Assert.NotNull(match);
            Assert.True(match);

        }
        [Test]
        public void Core_Case_GetPdfPath_returnsPath()
        {
            // Arrange
            #region Arrance
            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates
            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl3 = testHelpers.CreateSubTemplate("A.2", "D.2", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl4 = testHelpers.CreateSubTemplate("A.3", "D.3", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl5 = testHelpers.CreateSubTemplate("A.4", "D.4", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl6 = testHelpers.CreateSubTemplate("A.5", "D.5", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl7 = testHelpers.CreateSubTemplate("A.6", "D.6", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl8 = testHelpers.CreateSubTemplate("A.7", "D.7", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl9 = testHelpers.CreateSubTemplate("A.8", "D.8", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl10 = testHelpers.CreateSubTemplate("A.9", "D.9", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl11 = testHelpers.CreateSubTemplate("A.10", "D.10", "CheckList", 1, 1, cl1);


            #endregion
            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
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
            cases aCase2 = testHelpers.CreateCase("case2UId", cl3, c2_ca, "custom2",
             c2_da, worker, "microtingCheck2UId", "microting2UId",
               site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case3
            DateTime c3_ca = DateTime.Now.AddDays(-10);
            DateTime c3_da = DateTime.Now.AddDays(-9).AddHours(-12);
            DateTime c3_ua = DateTime.Now.AddDays(-9);

            cases aCase3 = testHelpers.CreateCase("case3UId", cl4, c3_ca, "custom3",
              c3_da, worker, "microtingCheck3UId", "microtin3gUId",
               site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case4
            DateTime c4_ca = DateTime.Now.AddDays(-8);
            DateTime c4_da = DateTime.Now.AddDays(-7).AddHours(-12);
            DateTime c4_ua = DateTime.Now.AddDays(-7);

            cases aCase4 = testHelpers.CreateCase("case4UId", cl5, c4_ca, "custom4",
                c4_da, worker, "microtingCheck4UId", "microting4UId",
               site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion
            #endregion

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = testHelpers.CreateUploadedData("checksum1", "File1", "no", @"C:\Users\soipi\Desktop", "File1", 1, worker,
                "local", 55, false);
            #endregion

            #region ud2
            uploaded_data ud2 = testHelpers.CreateUploadedData("checksum2", "File1", "no", @"C:\Users\soipi\Desktop", "File2", 1, worker,
                "local", 55, false);
            #endregion

            #region ud3
            uploaded_data ud3 = testHelpers.CreateUploadedData("checksum3", "File1", "no", @"C:\Users\soipi\Desktop", "File3", 1, worker,
                "local", 55, false);
            #endregion

            #region ud4
            uploaded_data ud4 = testHelpers.CreateUploadedData("checksum4", "File1", "no", @"C: \Users\soipi\Desktop", "File4", 1, worker,
                "local", 55, false);
            #endregion

            #region ud5
            uploaded_data ud5 = testHelpers.CreateUploadedData("checksum5", "File1", "no", @"C:\Users\soipi\Desktop", "File5", 1, worker,
                "local", 55, false);
            #endregion

            #region ud6
            uploaded_data ud6 = testHelpers.CreateUploadedData("checksum6", "File1", "no", @"C:\Users\soipi\Desktop", "File6", 1, worker,
                "local", 55, false);
            #endregion

            #region ud7
            uploaded_data ud7 = testHelpers.CreateUploadedData("checksum7", "File1", "no", @"C:\Users\soipi\Desktop", "File7", 1, worker,
                "local", 55, false);
            #endregion

            #region ud8
            uploaded_data ud8 = testHelpers.CreateUploadedData("checksum8", "File1", "no", @"C:\Users\soipi\Desktop", "File8", 1, worker,
                "local", 55, false);
            #endregion

            #region ud9
            uploaded_data ud9 = testHelpers.CreateUploadedData("checksum9", "File1", "no", @"C:\Users\soipi\Desktop", "File9", 1, worker,
                "local", 55, false);
            #endregion

            #region ud10
            uploaded_data ud10 = testHelpers.CreateUploadedData("checksum10", "File1", "no", @"C:\Users\soipi\Desktop", "File10", 1, worker,
                "local", 55, false);
            #endregion

            #endregion

            #region Check List Values
            #region clv1
            check_list_values clv1 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
            #endregion

            #region clv2
            check_list_values clv2 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 861);
            #endregion

            #region clv3
            check_list_values clv3 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 862);
            #endregion

            #region clv4
            check_list_values clv4 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 863);
            #endregion

            #region clv5
            check_list_values clv5 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 864);
            #endregion

            #region clv6
            check_list_values clv6 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 865);
            #endregion

            #region clv7
            check_list_values clv7 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 866);
            #endregion

            #region clv8
            check_list_values clv8 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 867);
            #endregion

            #region clv9
            check_list_values clv9 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 868);
            #endregion

            #region clv10
            check_list_values clv10 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 869);
            #endregion

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.id, null, "tomt1", 61230, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.id, null, "tomt2", 61231, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.id, null, "tomt3", 61232, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.id, null, "tomt4", 61233, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion

            #region fv6
            field_values field_Value6 = testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.id, null, "tomt6", 61235, worker);

            #endregion

            #region fv7
            field_values field_Value7 = testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.id, null, "tomt7", 61236, worker);

            #endregion

            #region fv8
            field_values field_Value8 = testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.id, null, "tomt8", 61237, worker);

            #endregion

            #region fv9
            field_values field_Value9 = testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.id, null, "tomt9", 61238, worker);

            #endregion

            #region fv10
            field_values field_Value10 = testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.id, null, "tomt10", 61239, worker);

            #endregion


            #endregion

            #region checkListSites
            DateTime cls_ca = DateTime.Now;
            DateTime cls_ua = DateTime.Now;
            check_list_sites cls1 = testHelpers.CreateCheckListSite(cl2, cls_ca, site,
               cls_ua, 5, Constants.WorkflowStates.Created, "");

            #endregion
            #endregion
            // Act

            var match = sut.GetSdkSetting(Settings.fileLocationJasper);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match, path + @"\output\dataFolder\pdf\");

        }
        [Test]
        public void Core_Case_GetHttpServerAddress_returnsPath()
        {
            // Arrange
            #region Arrance
            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates
            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl3 = testHelpers.CreateSubTemplate("A.2", "D.2", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl4 = testHelpers.CreateSubTemplate("A.3", "D.3", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl5 = testHelpers.CreateSubTemplate("A.4", "D.4", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl6 = testHelpers.CreateSubTemplate("A.5", "D.5", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl7 = testHelpers.CreateSubTemplate("A.6", "D.6", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl8 = testHelpers.CreateSubTemplate("A.7", "D.7", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl9 = testHelpers.CreateSubTemplate("A.8", "D.8", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl10 = testHelpers.CreateSubTemplate("A.9", "D.9", "CheckList", 1, 1, cl1);


            #endregion

            #region SubTemplate1
            check_lists cl11 = testHelpers.CreateSubTemplate("A.10", "D.10", "CheckList", 1, 1, cl1);


            #endregion
            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
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
            cases aCase2 = testHelpers.CreateCase("case2UId", cl3, c2_ca, "custom2",
             c2_da, worker, "microtingCheck2UId", "microting2UId",
               site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case3
            DateTime c3_ca = DateTime.Now.AddDays(-10);
            DateTime c3_da = DateTime.Now.AddDays(-9).AddHours(-12);
            DateTime c3_ua = DateTime.Now.AddDays(-9);

            cases aCase3 = testHelpers.CreateCase("case3UId", cl4, c3_ca, "custom3",
              c3_da, worker, "microtingCheck3UId", "microtin3gUId",
               site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion

            #region Case4
            DateTime c4_ca = DateTime.Now.AddDays(-8);
            DateTime c4_da = DateTime.Now.AddDays(-7).AddHours(-12);
            DateTime c4_ua = DateTime.Now.AddDays(-7);

            cases aCase4 = testHelpers.CreateCase("case4UId", cl5, c4_ca, "custom4",
                c4_da, worker, "microtingCheck4UId", "microting4UId",
               site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);
            #endregion
            #endregion

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = testHelpers.CreateUploadedData("checksum1", "File1", "no", @"C:\Users\soipi\Desktop", "File1", 1, worker,
                "local", 55, false);
            #endregion

            #region ud2
            uploaded_data ud2 = testHelpers.CreateUploadedData("checksum2", "File1", "no", @"C:\Users\soipi\Desktop", "File2", 1, worker,
                "local", 55, false);
            #endregion

            #region ud3
            uploaded_data ud3 = testHelpers.CreateUploadedData("checksum3", "File1", "no", @"C:\Users\soipi\Desktop", "File3", 1, worker,
                "local", 55, false);
            #endregion

            #region ud4
            uploaded_data ud4 = testHelpers.CreateUploadedData("checksum4", "File1", "no", @"C: \Users\soipi\Desktop", "File4", 1, worker,
                "local", 55, false);
            #endregion

            #region ud5
            uploaded_data ud5 = testHelpers.CreateUploadedData("checksum5", "File1", "no", @"C:\Users\soipi\Desktop", "File5", 1, worker,
                "local", 55, false);
            #endregion

            #region ud6
            uploaded_data ud6 = testHelpers.CreateUploadedData("checksum6", "File1", "no", @"C:\Users\soipi\Desktop", "File6", 1, worker,
                "local", 55, false);
            #endregion

            #region ud7
            uploaded_data ud7 = testHelpers.CreateUploadedData("checksum7", "File1", "no", @"C:\Users\soipi\Desktop", "File7", 1, worker,
                "local", 55, false);
            #endregion

            #region ud8
            uploaded_data ud8 = testHelpers.CreateUploadedData("checksum8", "File1", "no", @"C:\Users\soipi\Desktop", "File8", 1, worker,
                "local", 55, false);
            #endregion

            #region ud9
            uploaded_data ud9 = testHelpers.CreateUploadedData("checksum9", "File1", "no", @"C:\Users\soipi\Desktop", "File9", 1, worker,
                "local", 55, false);
            #endregion

            #region ud10
            uploaded_data ud10 = testHelpers.CreateUploadedData("checksum10", "File1", "no", @"C:\Users\soipi\Desktop", "File10", 1, worker,
                "local", 55, false);
            #endregion

            #endregion

            #region Check List Values
            #region clv1
            check_list_values clv1 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
            #endregion

            #region clv2
            check_list_values clv2 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 861);
            #endregion

            #region clv3
            check_list_values clv3 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 862);
            #endregion

            #region clv4
            check_list_values clv4 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 863);
            #endregion

            #region clv5
            check_list_values clv5 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 864);
            #endregion

            #region clv6
            check_list_values clv6 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotChecked, null, 865);
            #endregion

            #region clv7
            check_list_values clv7 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 866);
            #endregion

            #region clv8
            check_list_values clv8 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 867);
            #endregion

            #region clv9
            check_list_values clv9 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 868);
            #endregion

            #region clv10
            check_list_values clv10 = testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.NotApproved, null, 869);
            #endregion

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.id, null, "tomt1", 61230, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.id, null, "tomt2", 61231, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.id, null, "tomt3", 61232, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.id, null, "tomt4", 61233, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.id, null, "tomt5", 61234, worker);

            #endregion

            #region fv6
            field_values field_Value6 = testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.id, null, "tomt6", 61235, worker);

            #endregion

            #region fv7
            field_values field_Value7 = testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.id, null, "tomt7", 61236, worker);

            #endregion

            #region fv8
            field_values field_Value8 = testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.id, null, "tomt8", 61237, worker);

            #endregion

            #region fv9
            field_values field_Value9 = testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.id, null, "tomt9", 61238, worker);

            #endregion

            #region fv10
            field_values field_Value10 = testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.id, null, "tomt10", 61239, worker);

            #endregion


            #endregion

            #region checkListSites
            DateTime cls_ca = DateTime.Now;
            DateTime cls_ua = DateTime.Now;
            check_list_sites cls1 = testHelpers.CreateCheckListSite(cl2, cls_ca, site,
               cls_ua, 5, Constants.WorkflowStates.Created, "");

            #endregion
            #endregion
            // Act

            var match = sut.GetSdkSetting(Settings.httpServerAddress);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match, "http://localhost:3000");


        }
        [Test]
        public void Core_Case_SetHttpServerAddress_ReturnsTrue()
        {
            // Arrange

            // Act
            var match = sut.SetSdkSetting(Settings.httpServerAddress, "facebook.com");
            // Assert
            Assert.NotNull(match);
            Assert.True(match);
        }
        [Test]//can't be done, because of Jaxml file.
        public void Core_Case_CaseToPdf_returns_Path()
        {
            // Arrange

            // Act

            // Assert
        }

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