using eFormCore;
using eFormShared;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestCheckListValue : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

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
            sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }

        [Test]
        public void SQL_Check_CheckListValueStatusRead_ReturnsCheckListValuesStatus()
        {
            // Arrance
            #region Arrance
            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 1, 0, 1, 0,
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
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase, cl2, f1, ud1.Id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase, cl2, f2, ud2.Id, null, "tomt2", 61234, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase, cl2, f3, ud3.Id, null, "tomt3", 61234, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase, cl2, f4, ud4.Id, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase, cl2, f5, ud5.Id, null, "tomt5", 61234, worker);

            #endregion


            #endregion
            #endregion
            // Act
            var match = sut.CheckListValueStatusRead(aCase.Id, cl2.Id);
            // Assert

            Assert.AreEqual(check_List_Values.Status, "checked");

        }
        [Test]
        public void SQL_Check_CheckListValueStatusUpdate_UpdatesCheckListValues()
        {
            // Arrance
            #region Arrance
            #region Template1
            DateTime cl1_Ca = DateTime.Now;
            DateTime cl1_Ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


            #endregion

            #region Fields
            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 1, 0, 1, 0,
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
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase, cl2, f1, ud1.Id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase, cl2, f2, ud2.Id, null, "tomt2", 61234, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase, cl2, f3, ud3.Id, null, "tomt3", 61234, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase, cl2, f4, ud4.Id, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase, cl2, f5, ud5.Id, null, "tomt5", 61234, worker);

            #endregion


            #endregion
            #endregion
            // Act

            sut.CheckListValueStatusUpdate(aCase.Id, cl2.Id, "not_approved");

            // Assert
            var newValue = DbContext.check_list_values.AsNoTracking().SingleOrDefault(x => x.Id == check_List_Values.Id);

            Assert.AreEqual(newValue.Status, "not_approved");


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