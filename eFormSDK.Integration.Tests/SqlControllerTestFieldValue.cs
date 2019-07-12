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
using Microting.eForm.Infrastructure.Models;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestFieldValue : DbTestFixture
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
        public void SQL_Check_FieldValueRead_ReturnsAnswer()
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
                5, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);          
            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.FieldType == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
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
            #endregion

            #region UploadedData
            uploaded_data ud = testHelpers.CreateUploadedData("checksum", "File1", "no", "mappe", "File1", 1, worker,
                "local", 55, false);

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase, cl2, f1, ud.Id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);
            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);
            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);
            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);
            #endregion


            #endregion
            #endregion
            // Act

            var match = sut.FieldValueRead(field_Value1, false);

            // Assert
            #region Assert
            Assert.True(match is FieldValue);
            Assert.AreEqual(field_Value1.Accuracy, match.Accuracy);
            Assert.AreEqual(field_Value1.Altitude, match.Altitude);
            // Assert.AreEqual(field_Value1.case_id, match.case_id);
            // Assert.AreEqual(field_Value1.check_list, match.check_list);
            // Assert.AreEqual(field_Value1.check_list_duplicate_id, match.check_list_duplicate_id);
            // Assert.AreEqual(field_Value1.check_list_id, match.check_list_id);
            // Assert.AreEqual(field_Value1.created_at, match.created_at);
            Assert.AreEqual(field_Value1.Date, match.Date);
            // Assert.AreEqual(field_Value1.done_at, match.done_at);
            Assert.AreEqual(field_Value1.Field, f1);
            Assert.AreEqual(field_Value1.FieldId, match.FieldId);
            Assert.AreEqual(field_Value1.Heading, match.Heading);
            Assert.AreEqual(field_Value1.Id, match.Id);
            Assert.AreEqual(field_Value1.Latitude, match.Latitude);
            Assert.AreEqual(field_Value1.Longitude, match.Longitude);
            // Assert.AreEqual(field_Value1.updated_at, match.updated_at);
            // Assert.AreEqual("mappeFile1", match.UploadedData);
            Assert.AreEqual(field_Value1.UploadedData.Checksum, match.UploadedDataObj.Checksum);
            Assert.AreEqual(field_Value1.UploadedData.CurrentFile, match.UploadedDataObj.CurrentFile);
            Assert.AreEqual(field_Value1.UploadedData.Extension, match.UploadedDataObj.Extension);
            Assert.AreEqual(field_Value1.UploadedData.FileLocation, match.UploadedDataObj.FileLocation);
            Assert.AreEqual(field_Value1.UploadedData.FileName, match.UploadedDataObj.FileName);
            Assert.AreEqual(field_Value1.UploadedData.Id, match.UploadedDataObj.Id);
            Assert.AreEqual(field_Value1.UploadedData.UploaderId, match.UploadedDataObj.UploaderId);
            Assert.AreEqual(field_Value1.UploadedData.UploaderType, match.UploadedDataObj.UploaderType);
            // Assert.AreEqual(field_Value1.user_id, match.user_id);
            Assert.AreEqual(field_Value1.Value, match.Value);
            // Assert.AreEqual(field_Value1.version, match.version);
            // Assert.AreEqual(field_Value1.worker, match.worker);
            // Assert.AreEqual(field_Value1.workflow_state, match.workflow_state);
            #endregion

        }
        [Test]
        public void SQL_Check_FieldValueReadWithUploadedData_ReturnsAnswer()
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
                5, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);
            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);

            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.FieldType == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);

            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
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
 

            #endregion

            #region UploadedData
            uploaded_data ud = testHelpers.CreateUploadedData("checksum", "File1", "no", "mappe", "File1", 1, worker,
                "local", 55, false);

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase, cl2, f1, ud.Id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase, cl2, f2, ud.Id, null, "tomt2", 61234, worker);
  
            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase, cl2, f3, ud.Id, null, "tomt3", 61234, worker);
   
            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase, cl2, f4, ud.Id, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase, cl2, f5, ud.Id, null, "tomt5", 61234, worker);
   
            #endregion


            #endregion


            #endregion
            // Act

            var match = sut.FieldValueRead(field_Value1, true);

            //// Assert
            #region Assert
            Assert.True(match is FieldValue);
            Assert.AreEqual(field_Value1.Accuracy, match.Accuracy);
            Assert.AreEqual(field_Value1.Altitude, match.Altitude);
            // Assert.AreEqual(field_Value1.case_id, match.case_id);
            // Assert.AreEqual(field_Value1.check_list, match.check_list);
            // Assert.AreEqual(field_Value1.check_list_duplicate_id, match.check_list_duplicate_id);
            // Assert.AreEqual(field_Value1.check_list_id, match.check_list_id);
            // Assert.AreEqual(field_Value1.created_at, match.created_at);
            Assert.AreEqual(field_Value1.Date, match.Date);
            // Assert.AreEqual(field_Value1.done_at, match.done_at);
            Assert.AreEqual(field_Value1.Field, f1);
            Assert.AreEqual(field_Value1.FieldId, match.FieldId);
            Assert.AreEqual(field_Value1.Heading, match.Heading);
            Assert.AreEqual(field_Value1.Id, match.Id);
            Assert.AreEqual(field_Value1.Latitude, match.Latitude);
            Assert.AreEqual(field_Value1.Longitude, match.Longitude);
            // Assert.AreEqual(field_Value1.updated_at, match.updated_at);
            Assert.AreEqual("mappeFile1", match.UploadedData);
            // Assert.AreEqual(field_Value1.uploaded_data_id, match.UploadedDataObj);
            // Assert.AreEqual(field_Value1.user_id, match.user_id);         
            Assert.AreEqual(field_Value1.Value, match.Value);
            // Assert.AreEqual(field_Value1.version, match.version);
            // Assert.AreEqual(field_Value1.worker, match.worker);
            // Assert.AreEqual(field_Value1.workflow_state, match.workflow_state);

            #endregion
        }
//        [Test]
//        public void SQL_Check_FieldValueRead_ReturnsTrue()
//        {
//            // Arrance
//
//            #region Arrance
//            #region Template1
//            DateTime cl1_Ca = DateTime.Now;
//            DateTime cl1_Ua = DateTime.Now;
//            check_lists cl1 = testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region Fields
//            #region field1
//
//
//            fields f1 = testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, DbContext.field_types.Where(x => x.FieldType == "number").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//            #endregion
//
//            #region Worker
//
//            workers worker = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
//
//            #endregion
//
//            #region site
//            sites site = testHelpers.CreateSite("SiteName", 88);
//
//            #endregion
//
//            #region units
//            units unit = testHelpers.CreateUnit(48, 49, site, 348);
//
//            #endregion
//
//            #region site_workers
//            site_workers site_workers = testHelpers.CreateSiteWorker(55, site, worker);
//
//            #endregion
//
//            #region Case1
//
//            cases aCase = testHelpers.CreateCase("caseUId", cl1, DateTime.Now, "custom", DateTime.Now,
//                worker, "microtingCheckUId", "microtingUId",
//               site, 66, "caseType", unit, DateTime.Now, 1, worker, Constants.WorkflowStates.Created);
//
//            #endregion
//
//            #region Check List Values
//            check_list_values check_List_Values = testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);
//
//
//            #endregion
//
//            #region Field Values
//            #region fv1
//            field_values field_Value1 = testHelpers.CreateFieldValue(aCase, cl2, f1, null, null, "tomt1", 61234, worker);
//
//            #endregion
//
//            #region fv2
//            field_values field_Value2 = testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);
//
//            #endregion
//
//            #region fv3
//            field_values field_Value3 = testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);
//
//            #endregion
//
//            #region fv4
//            field_values field_Value4 = testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);
//
//            #endregion
//
//            #region fv5
//            field_values field_Value5 = testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);
//
//            #endregion
//
//
//            #endregion
//            #endregion
//
//            // Act
//
//            var match = sut.FieldValueRead(field_Value1.Id);
//
//            // Assert
//
//            Assert.AreEqual(field_Value1.Id, match.Id);
//
//        }
        [Test]
        public void SQL_Check_FieldValueReadList_ReturnsList()
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
                5, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.FieldType == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
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


            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase, cl2, f1, null, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);

            #endregion


            #endregion
            #endregion
            // Act

            List<FieldValue> match = sut.FieldValueReadList(f1.Id, 5);

            // Assert

            Assert.AreEqual(field_Value1.Value, match[0].Value);

        }
        [Test]
        public void SQL_Check_FieldValueUpdate_UpdatesFieldValue()
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
                5, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.FieldType == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
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


            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase, cl2, f1, null, null, "tomt1", 61234, worker);

            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);

            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);

            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);

            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);

            #endregion


            #endregion
            #endregion

            // Act

            sut.FieldValueUpdate(aCase.Id, field_Value1.Id, "udfyldt");



            // Assert
            var newValue = DbContext.field_values.AsNoTracking().SingleOrDefault(x => x.Id == field_Value1.Id);

            Assert.AreEqual(newValue.Value, "udfyldt");


        }
        [Test]
        public void SQL_Check_FieldValueReadAllValues_ReturnsReturnList()
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
            check_list_values check_List_Values = testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);


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
            List<int> listOfCaseIds = new List<int>();
            listOfCaseIds.Add(aCase.Id);
            var matchF1 = sut.FieldValueReadAllValues(f1.Id, listOfCaseIds, "mappe/");
            var matchF2 = sut.FieldValueReadAllValues(f2.Id, listOfCaseIds, "mappe/");
            var matchF3 = sut.FieldValueReadAllValues(f3.Id, listOfCaseIds, "mappe/");
            var matchF4 = sut.FieldValueReadAllValues(f4.Id, listOfCaseIds, "mappe/");
            var matchF5 = sut.FieldValueReadAllValues(f5.Id, listOfCaseIds, "mappe/");

            // Assert
            #region Assert
            Assert.AreEqual("mappe/File1", matchF1[0].ElementAt(0).Value);
            Assert.AreEqual("mappe/File2", matchF2[0].ElementAt(0).Value);
            Assert.AreEqual("mappe/File3", matchF3[0].ElementAt(0).Value);
            Assert.AreEqual("mappe/File4", matchF4[0].ElementAt(0).Value);
            Assert.AreEqual("mappe/File5", matchF5[0].ElementAt(0).Value);
            #endregion

        }
        [Test]
        public void SQL_Check_ChecksRead_ReturnsListOfFieldValues()
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
                5, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.FieldType == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);

            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
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
       

            #endregion

            #region Field Values
            #region fv1
            field_values field_Value1 = testHelpers.CreateFieldValue(aCase, cl2, f1, null, null, "tomt1", 61234, worker);
         
            #endregion

            #region fv2
            field_values field_Value2 = testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);
     
            #endregion

            #region fv3
            field_values field_Value3 = testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);
        
            #endregion

            #region fv4
            field_values field_Value4 = testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);
        
            #endregion

            #region fv5
            field_values field_Value5 = testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);
       
            #endregion


            #endregion
            #endregion
            // Act

            List<field_values> match = sut.ChecksRead(aCase.MicrotingUid, aCase.MicrotingCheckUid);

            // Assert


            Assert.AreEqual(field_Value1.Value, match[0].Value);
            Assert.AreEqual(field_Value2.Value, match[1].Value);
            Assert.AreEqual(field_Value3.Value, match[2].Value);
            Assert.AreEqual(field_Value4.Value, match[3].Value);
            Assert.AreEqual(field_Value5.Value, match[4].Value);





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