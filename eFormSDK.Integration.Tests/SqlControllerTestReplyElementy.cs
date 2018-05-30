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
    public class SqlControllerTestReplyElementy : DbTestFixture
    {
        private SqlController sut;
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

            sut = new SqlController(ConnectionString);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            sut.SettingUpdate(Settings.fileLocationPicture, path + @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, path + @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, path + @"\output\dataFolder\reports\");
        }

        [Test]
        public void SQL_Check_CheckRead_ReturnsReplyElement()
        {
            // Arrance
            #region Arrance

            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);
            //    new check_lists();
            //cl1.created_at = DateTime.Now;
            //cl1.updated_at = DateTime.Now;
            //cl1.label = "A";
            //cl1.description = "D";
            //cl1.workflow_state = Constants.WorkflowStates.Created;
            //cl1.case_type = "CheckList";
            //cl1.folder_name = "Template1FolderName";
            //cl1.display_index = 1;
            //cl1.repeated = 1;

            //DbContext.check_lists.Add(cl1);
            //DbContext.SaveChanges();
            #endregion

            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
            //    new check_lists();
            //cl2.created_at = DateTime.Now;
            //cl2.updated_at = DateTime.Now;
            //cl2.label = "A.1";
            //cl2.description = "D.1";
            //cl2.workflow_state = Constants.WorkflowStates.Created;
            //cl2.case_type = "CheckList";
            //cl2.display_index = 1;
            //cl2.repeated = 1;
            //cl2.parent_id = cl1.id;

            //DbContext.check_lists.Add(cl2);
            //DbContext.SaveChanges();

            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);
            //    new fields();
            //field_types ft1 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f1.field_type = ft1;

            //f1.barcode_enabled = 1;
            //f1.barcode_type = "barcode";
            //f1.check_list_id = cl2.id;
            //f1.color = "e2f4fb";
            //f1.created_at = DateTime.Now;
            //f1.custom = "custom";
            //f1.decimal_count = null;
            //f1.default_value = "";
            //f1.description = "Comment field Description";
            //f1.display_index = 5;
            //f1.dummy = 1;
            //f1.geolocation_enabled = 0;
            //f1.geolocation_forced = 0;
            //f1.geolocation_hidden = 1;
            //f1.is_num = 0;
            //f1.label = "Comment field";
            //f1.mandatory = 1;
            //f1.max_length = 55;
            //f1.max_value = "55";
            //f1.min_value = "0";
            //f1.multi = 0;
            //f1.optional = 0;
            //f1.query_type = null;
            //f1.read_only = 1;
            //f1.selected = 0;
            //f1.split_screen = 0;
            //f1.stop_on_save = 0;
            //f1.unit_name = "";
            //f1.updated_at = DateTime.Now;
            //f1.version = 49;
            //f1.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f1);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);
            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
            //    new fields();
            //field_types ft2 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f2.field_type = ft2;

            //f2.barcode_enabled = 1;
            //f2.barcode_type = "barcode";
            //f2.check_list_id = cl2.id;
            //f2.color = "f5eafa";
            //f2.default_value = "";
            //f2.description = "showPDf Description";
            //f2.display_index = 45;
            //f2.dummy = 1;
            //f2.geolocation_enabled = 0;
            //f2.geolocation_forced = 1;
            //f2.geolocation_hidden = 0;
            //f2.is_num = 0;
            //f2.label = "ShowPdf";
            //f2.mandatory = 0;
            //f2.max_length = 5;
            //f2.max_value = "5";
            //f2.min_value = "0";
            //f2.multi = 0;
            //f2.optional = 0;
            //f2.query_type = null;
            //f2.read_only = 0;
            //f2.selected = 0;
            //f2.split_screen = 0;
            //f2.stop_on_save = 0;
            //f2.unit_name = "";
            //f2.updated_at = DateTime.Now;
            //f2.version = 9;
            //f2.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f2);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
            //    new fields();
            //field_types ft3 = DbContext.field_types.Where(x => x.field_type == "number").First();

            //f3.field_type = ft3;

            //f3.barcode_enabled = 0;
            //f3.barcode_type = "barcode";
            //f3.check_list_id = cl2.id;
            //f3.color = "f0f8db";
            //f3.created_at = DateTime.Now;
            //f3.custom = "custom";
            //f3.decimal_count = 3;
            //f3.default_value = "";
            //f3.description = "Number Field Description";
            //f3.display_index = 83;
            //f3.dummy = 0;
            //f3.geolocation_enabled = 0;
            //f3.geolocation_forced = 0;
            //f3.geolocation_hidden = 1;
            //f3.is_num = 0;
            //f3.label = "Numberfield";
            //f3.mandatory = 1;
            //f3.max_length = 8;
            //f3.max_value = "4865";
            //f3.min_value = "0";
            //f3.multi = 0;
            //f3.optional = 1;
            //f3.query_type = null;
            //f3.read_only = 1;
            //f3.selected = 0;
            //f3.split_screen = 0;
            //f3.stop_on_save = 0;
            //f3.unit_name = "";
            //f3.updated_at = DateTime.Now;
            //f3.version = 1;
            //f3.workflow_state = Constants.WorkflowStates.Created;



            //DbContext.fields.Add(f3);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
            //    new fields();
            //field_types ft4 = DbContext.field_types.Where(x => x.field_type == "comment").First();
            //f4.field_type = ft4;

            //f4.barcode_enabled = 1;
            //f4.barcode_type = "barcode";
            //f4.check_list_id = cl2.id;
            //f4.color = "fff6df";
            //f4.created_at = DateTime.Now;
            //f4.custom = "custom";
            //f4.decimal_count = null;
            //f4.default_value = "";
            //f4.description = "date Description";
            //f4.display_index = 84;
            //f4.dummy = 0;
            //f4.geolocation_enabled = 0;
            //f4.geolocation_forced = 0;
            //f4.geolocation_hidden = 1;
            //f4.is_num = 0;
            //f4.label = "Date";
            //f4.mandatory = 1;
            //f4.max_length = 666;
            //f4.max_value = "41153";
            //f4.min_value = "0";
            //f4.multi = 0;
            //f4.optional = 1;
            //f4.query_type = null;
            //f4.read_only = 0;
            //f4.selected = 1;
            //f4.split_screen = 0;
            //f4.stop_on_save = 0;
            //f4.unit_name = "";
            //f4.updated_at = DateTime.Now;
            //f4.version = 1;
            //f4.workflow_state = Constants.WorkflowStates.Created;


            //DbContext.fields.Add(f4);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
            //    new fields();
            //field_types ft5 = DbContext.field_types.Where(x => x.field_type == "comment").First();

            //f5.field_type = ft5;
            //f5.barcode_enabled = 0;
            //f5.barcode_type = "barcode";
            //f5.check_list_id = cl2.id;
            //f5.color = "ffe4e4";
            //f5.created_at = DateTime.Now;
            //f5.custom = "custom";
            //f5.decimal_count = null;
            //f5.default_value = "";
            //f5.description = "picture Description";
            //f5.display_index = 85;
            //f5.dummy = 0;
            //f5.geolocation_enabled = 1;
            //f5.geolocation_forced = 0;
            //f5.geolocation_hidden = 1;
            //f5.is_num = 0;
            //f5.label = "Picture";
            //f5.mandatory = 1;
            //f5.max_length = 69;
            //f5.max_value = "69";
            //f5.min_value = "1";
            //f5.multi = 0;
            //f5.optional = 1;
            //f5.query_type = null;
            //f5.read_only = 0;
            //f5.selected = 1;
            //f5.split_screen = 0;
            //f5.stop_on_save = 0;
            //f5.unit_name = "";
            //f5.updated_at = DateTime.Now;
            //f5.version = 1;
            //f5.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.fields.Add(f5);
            //DbContext.SaveChanges();
            //Thread.Sleep(2000);

            #endregion
            #endregion

            #region Worker

            workers worker = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
            //= new workers();
            //worker.first_name = "Arne";
            //worker.last_name = "Jensen";
            //worker.email = "aa@tak.dk";
            //worker.created_at = DateTime.Now;
            //worker.updated_at = DateTime.Now;
            //worker.microting_uid = 21;
            //worker.workflow_state = Constants.WorkflowStates.Created;
            //worker.version = 69;
            //DbContext.workers.Add(worker);
            //DbContext.SaveChanges();
            #endregion

            #region site
            sites site = testHelpers.CreateSite("SiteName", 88);
            //    new sites();
            //site.name = "SiteName";
            //site.microting_uid = 88;
            //site.updated_at = DateTime.Now;
            //site.created_at = DateTime.Now;
            //site.version = 64;
            //site.workflow_state = Constants.WorkflowStates.Created;
            //DbContext.sites.Add(site);
            //DbContext.SaveChanges();
            #endregion

            #region units
            units unit = testHelpers.CreateUnit(48, 49, site, 348);
            //    new units();
            //unit.microting_uid = 48;
            //unit.otp_code = 49;
            //unit.site = site;
            //unit.site_id = site.id;
            //unit.created_at = DateTime.Now;
            //unit.customer_no = 348;
            //unit.updated_at = DateTime.Now;
            //unit.version = 9;
            //unit.workflow_state = Constants.WorkflowStates.Created;

            //DbContext.units.Add(unit);
            //DbContext.SaveChanges();
            #endregion

            #region site_workers
            site_workers site_workers = testHelpers.CreateSiteWorker(55, site, worker);
            //    new site_workers();
            //site_workers.created_at = DateTime.Now;
            //site_workers.microting_uid = 55;
            //site_workers.updated_at = DateTime.Now;
            //site_workers.version = 63;
            //site_workers.site = site;
            //site_workers.site_id = site.id;
            //site_workers.worker = worker;
            //site_workers.worker_id = worker.id;
            //site_workers.workflow_state = Constants.WorkflowStates.Created;
            //DbContext.site_workers.Add(site_workers);
            //DbContext.SaveChanges();
            #endregion

            #region Case1

            cases aCase = testHelpers.CreateCase("caseUId", cl1, DateTime.Now, "custom", DateTime.Now,
                worker, "microtingCheckUId", "microtingUId",
               site, 66, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);
            //    new cases();
            //aCase.case_uid = caseUId;
            //aCase.check_list = cl1;
            //aCase.check_list_id = cl1.id;
            //aCase.created_at = DateTime.Now;
            //aCase.custom = custom;
            //aCase.done_at = DateTime.Now;
            //aCase.done_by_user_id = worker.id;
            //aCase.microting_check_uid = microtingCheckId;
            //aCase.microting_uid = microtingUId;
            //aCase.site = site;
            //aCase.site_id = site.id;
            //aCase.status = 66;
            //aCase.type = caseType;
            //aCase.unit = unit;
            //aCase.unit_id = unit.id;
            //aCase.updated_at = DateTime.Now;
            //aCase.version = 1;
            //aCase.worker = worker;
            //aCase.workflow_state = Constants.WorkflowStates.Created;       
            //DbContext.cases.Add(aCase);
            //DbContext.SaveChanges();
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
            field_values field_Values1 = testHelpers.CreateFieldValue(aCase, cl2, f1, null, null, "tomt1", 61234, worker);
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
            field_values field_Values2 = testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);
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
            field_values field_Values3 = testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);
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
            field_values field_Values4 = testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);
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
            field_values field_Values5 = testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);
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

            ReplyElement match = sut.CheckRead(aCase.microting_uid, aCase.microting_check_uid);

            // Assert
            #region Assert

            Assert.AreEqual(1, match.ElementList.Count());
            CheckListValue clv = (CheckListValue)match.ElementList[0];
            Assert.AreEqual(5, clv.DataItemList.Count);
            #region casts
            Field _f1 = (Field)clv.DataItemList[0];
            Field _f2 = (Field)clv.DataItemList[1];
            Field _f3 = (Field)clv.DataItemList[2];
            Field _f4 = (Field)clv.DataItemList[3];
            Field _f5 = (Field)clv.DataItemList[4];


            #endregion

            #region Barcode
            Assert.AreEqual(f1.barcode_enabled, 1);
            Assert.AreEqual(f2.barcode_enabled, 1);
            Assert.AreEqual(f3.barcode_enabled, 0);
            Assert.AreEqual(f4.barcode_enabled, 1);
            Assert.AreEqual(f5.barcode_enabled, 0);

            Assert.AreEqual(f1.barcode_type, "barcode");
            Assert.AreEqual(f2.barcode_type, "barcode");
            Assert.AreEqual(f3.barcode_type, "barcode");
            Assert.AreEqual(f4.barcode_type, "barcode");
            Assert.AreEqual(f5.barcode_type, "barcode");
            #endregion

            #region chckl_id

            Assert.AreEqual(f1.check_list_id, cl2.id);
            Assert.AreEqual(f2.check_list_id, cl2.id);
            Assert.AreEqual(f3.check_list_id, cl2.id);
            Assert.AreEqual(f4.check_list_id, cl2.id);
            Assert.AreEqual(f5.check_list_id, cl2.id);


            #endregion

            #region Color
            Assert.AreEqual(f1.color, _f1.FieldValues[0].Color);
            Assert.AreEqual(f2.color, _f2.FieldValues[0].Color);
            Assert.AreEqual(f3.color, _f3.FieldValues[0].Color);
            Assert.AreEqual(f4.color, _f4.FieldValues[0].Color);
            Assert.AreEqual(f5.color, _f5.FieldValues[0].Color);
            #endregion

            #region custom
            //  Assert.AreEqual(f1.custom, _f1.FieldValues[0].Id);
            #endregion

            #region Decimal_Count
            Assert.AreEqual(f1.decimal_count, null);
            Assert.AreEqual(f2.decimal_count, null);
            Assert.AreEqual(f3.decimal_count, 3);
            Assert.AreEqual(f4.decimal_count, null);
            Assert.AreEqual(f5.decimal_count, null);

            #endregion

            #region Default_value
            Assert.AreEqual(f1.default_value, "");
            Assert.AreEqual(f2.default_value, "");
            Assert.AreEqual(f3.default_value, "");
            Assert.AreEqual(f4.default_value, "");
            Assert.AreEqual(f5.default_value, "");
            #endregion

            #region Description
            CDataValue f1desc = (CDataValue)_f1.Description;
            CDataValue f2desc = (CDataValue)_f2.Description;
            CDataValue f3desc = (CDataValue)_f3.Description;
            CDataValue f4desc = (CDataValue)_f4.Description;
            CDataValue f5desc = (CDataValue)_f5.Description;

            Assert.AreEqual(f1.description, f1desc.InderValue);
            Assert.AreEqual(f2.description, f2desc.InderValue);
            Assert.AreEqual(f3.description, f3desc.InderValue);
            Assert.AreEqual(f4.description, f4desc.InderValue);
            Assert.AreEqual(f5.description, f5desc.InderValue);
            #endregion

            #region Displayindex
            Assert.AreEqual(f1.display_index, _f1.FieldValues[0].DisplayOrder);
            Assert.AreEqual(f2.display_index, _f2.FieldValues[0].DisplayOrder);
            Assert.AreEqual(f3.display_index, _f3.FieldValues[0].DisplayOrder);
            Assert.AreEqual(f4.display_index, _f4.FieldValues[0].DisplayOrder);
            Assert.AreEqual(f5.display_index, _f5.FieldValues[0].DisplayOrder);
            #endregion

            #region Dummy
            Assert.AreEqual(f1.dummy, 1);
            Assert.AreEqual(f2.dummy, 1);
            Assert.AreEqual(f3.dummy, 0);
            Assert.AreEqual(f4.dummy, 0);
            Assert.AreEqual(f5.dummy, 0);
            #endregion

            #region geolocation
            #region enabled
            Assert.AreEqual(f1.geolocation_enabled, 0);
            Assert.AreEqual(f2.geolocation_enabled, 0);
            Assert.AreEqual(f3.geolocation_enabled, 0);
            Assert.AreEqual(f4.geolocation_enabled, 0);
            Assert.AreEqual(f5.geolocation_enabled, 1);
            #endregion
            #region forced
            Assert.AreEqual(f1.geolocation_forced, 0);
            Assert.AreEqual(f2.geolocation_forced, 1);
            Assert.AreEqual(f3.geolocation_forced, 0);
            Assert.AreEqual(f4.geolocation_forced, 0);
            Assert.AreEqual(f5.geolocation_forced, 0);
            #endregion
            #region hidden
            Assert.AreEqual(f1.geolocation_hidden, 1);
            Assert.AreEqual(f2.geolocation_hidden, 0);
            Assert.AreEqual(f3.geolocation_hidden, 1);
            Assert.AreEqual(f4.geolocation_hidden, 1);
            Assert.AreEqual(f5.geolocation_hidden, 1);
            #endregion

            #endregion

            #region isNum
            Assert.AreEqual(f1.is_num, 0);
            Assert.AreEqual(f2.is_num, 0);
            Assert.AreEqual(f3.is_num, 0);
            Assert.AreEqual(f4.is_num, 0);
            Assert.AreEqual(f5.is_num, 0);


            #endregion

            #region Label
            Assert.AreEqual(f1.label, _f1.Label);
            Assert.AreEqual(f2.label, _f2.Label);
            Assert.AreEqual(f3.label, _f3.Label);
            Assert.AreEqual(f4.label, _f4.Label);
            Assert.AreEqual(f5.label, _f5.Label);
            #endregion

            #region Mandatory
            Assert.AreEqual(f1.mandatory, 1);
            Assert.AreEqual(f2.mandatory, 0);
            Assert.AreEqual(f3.mandatory, 1);
            Assert.AreEqual(f4.mandatory, 1);
            Assert.AreEqual(f5.mandatory, 1);
            #endregion

            #region maxLength
            Assert.AreEqual(f1.max_length, 55);
            Assert.AreEqual(f2.max_length, 5);
            Assert.AreEqual(f3.max_length, 8);
            Assert.AreEqual(f4.max_length, 666);
            Assert.AreEqual(f5.max_length, 69);

            #endregion

            #region min/max_Value
            #region max
            Assert.AreEqual(f1.max_value, "55");
            Assert.AreEqual(f2.max_value, "5");
            Assert.AreEqual(f3.max_value, "4865");
            Assert.AreEqual(f4.max_value, "41153");
            Assert.AreEqual(f5.max_value, "69");
            #endregion
            #region min
            Assert.AreEqual(f1.min_value, "0");
            Assert.AreEqual(f2.min_value, "0");
            Assert.AreEqual(f3.min_value, "0");
            Assert.AreEqual(f4.min_value, "0");
            Assert.AreEqual(f5.min_value, "1");
            #endregion
            #endregion

            #region Multi
            Assert.AreEqual(f1.multi, 0);
            Assert.AreEqual(f2.multi, 0);
            Assert.AreEqual(f3.multi, 0);
            Assert.AreEqual(f4.multi, 0);
            Assert.AreEqual(f5.multi, 0);
            #endregion

            #region Optional
            Assert.AreEqual(f1.optional, 0);
            Assert.AreEqual(f2.optional, 0);
            Assert.AreEqual(f3.optional, 1);
            Assert.AreEqual(f4.optional, 1);
            Assert.AreEqual(f5.optional, 1);

            #endregion

            #region Query_Type
            Assert.AreEqual(f1.query_type, null);
            Assert.AreEqual(f2.query_type, null);
            Assert.AreEqual(f3.query_type, null);
            Assert.AreEqual(f4.query_type, null);
            Assert.AreEqual(f5.query_type, null);

            #endregion

            #region Read_Only
            Assert.AreEqual(f1.read_only, 1);
            Assert.AreEqual(f2.read_only, 0);
            Assert.AreEqual(f3.read_only, 1);
            Assert.AreEqual(f4.read_only, 0);
            Assert.AreEqual(f5.read_only, 0);
            #endregion

            #region Selected
            Assert.AreEqual(f1.selected, 0);
            Assert.AreEqual(f2.selected, 0);
            Assert.AreEqual(f3.selected, 0);
            Assert.AreEqual(f4.selected, 1);
            Assert.AreEqual(f5.selected, 1);
            #endregion

            #region Split_Screen
            Assert.AreEqual(f1.split_screen, 0);
            Assert.AreEqual(f2.split_screen, 0);
            Assert.AreEqual(f3.split_screen, 0);
            Assert.AreEqual(f4.split_screen, 0);
            Assert.AreEqual(f5.split_screen, 0);

            #endregion

            #region Stop_On_Save
            Assert.AreEqual(f1.stop_on_save, 0);
            Assert.AreEqual(f2.stop_on_save, 0);
            Assert.AreEqual(f3.stop_on_save, 0);
            Assert.AreEqual(f4.stop_on_save, 0);
            Assert.AreEqual(f5.stop_on_save, 0);
            #endregion

            #region Unit_Name
            Assert.AreEqual(f1.unit_name, "");
            Assert.AreEqual(f2.unit_name, "");
            Assert.AreEqual(f3.unit_name, "");
            Assert.AreEqual(f4.unit_name, "");
            Assert.AreEqual(f5.unit_name, "");
            #endregion

            #region Values

            Assert.AreEqual(1, _f1.FieldValues.Count());
            Assert.AreEqual(1, _f2.FieldValues.Count());
            Assert.AreEqual(1, _f3.FieldValues.Count());
            Assert.AreEqual(1, _f4.FieldValues.Count());
            Assert.AreEqual(1, _f5.FieldValues.Count());

            Assert.AreEqual(field_Values1.value, _f1.FieldValues[0].Value);
            Assert.AreEqual(field_Values2.value, _f2.FieldValues[0].Value);
            Assert.AreEqual(field_Values3.value, _f3.FieldValues[0].Value);
            Assert.AreEqual(field_Values4.value, _f4.FieldValues[0].Value);
            Assert.AreEqual(field_Values5.value, _f5.FieldValues[0].Value);
            #endregion

            #region Version
            Assert.AreEqual(f1.version, 49);
            Assert.AreEqual(f2.version, 9);
            Assert.AreEqual(f3.version, 1);
            Assert.AreEqual(f4.version, 1);
            Assert.AreEqual(f5.version, 1);
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