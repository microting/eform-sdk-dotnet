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
using Microsoft.EntityFrameworkCore;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using NUnit.Framework;
using Case = Microting.eForm.Infrastructure.Data.Entities.Case;

namespace eFormSDK.Integration.Base.SqlControllerTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SqlControllerTestFieldValue : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
        private Language language;

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
            language = DbContext.Languages.Single(x => x.Name == "Danish");
        }

        [Test]
        public async Task SQL_Check_FieldValueRead_ReturnsAnswer()
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
                5, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55,
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
                83, 0, DbContext.FieldTypes.Where(x => x.Type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);

            #endregion

            #region field4

            Field f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "",
                "date Description",
                84, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0,
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

            Case aCase = await testHelpers.CreateCase("caseUId", cl1, DateTime.UtcNow, "custom", DateTime.UtcNow,
                worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 66, "caseType", unit, DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Check List Values

            CheckListValue checkListValue = await testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);

            #endregion

            #region UploadedData

            UploadedData ud = await testHelpers.CreateUploadedData("checksum", "File1", "no", "mappe", "File1", 1,
                worker,
                "local", 55, false);

            #endregion

            #region Field Values

            #region fv1

            FieldValue field_Value1 =
                await testHelpers.CreateFieldValue(aCase, cl2, f1, ud.Id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2

            FieldValue field_Value2 =
                await testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);

            #endregion

            #region fv3

            FieldValue field_Value3 =
                await testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);

            #endregion

            #region fv4

            FieldValue field_Value4 =
                await testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);

            #endregion

            #region fv5

            FieldValue field_Value5 =
                await testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);

            #endregion

            #endregion

            #endregion

            // Act

            var match = await sut.FieldValueRead(field_Value1, false, language);

            // Assert

            #region Assert

            Assert.True(match is Microting.eForm.Infrastructure.Models.FieldValue);
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
        public async Task SQL_Check_FieldValueReadWithUploadedData_ReturnsAnswer()
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
                5, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55,
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
                83, 0, DbContext.FieldTypes.Where(x => x.Type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);

            #endregion

            #region field4

            Field f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "",
                "date Description",
                84, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0,
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

            Case aCase = await testHelpers.CreateCase("caseUId", cl1, DateTime.UtcNow, "custom", DateTime.UtcNow,
                worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 66, "caseType", unit, DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Check List Values

            CheckListValue checkListValue = await testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);

            #endregion

            #region UploadedData

            UploadedData ud = await testHelpers.CreateUploadedData("checksum", "File1", "no", "mappe", "File1", 1,
                worker,
                "local", 55, false);

            #endregion

            #region Field Values

            #region fv1

            FieldValue field_Value1 =
                await testHelpers.CreateFieldValue(aCase, cl2, f1, ud.Id, null, "tomt1", 61234, worker);

            #endregion

            #region fv2

            FieldValue field_Value2 =
                await testHelpers.CreateFieldValue(aCase, cl2, f2, ud.Id, null, "tomt2", 61234, worker);

            #endregion

            #region fv3

            FieldValue field_Value3 =
                await testHelpers.CreateFieldValue(aCase, cl2, f3, ud.Id, null, "tomt3", 61234, worker);

            #endregion

            #region fv4

            FieldValue field_Value4 =
                await testHelpers.CreateFieldValue(aCase, cl2, f4, ud.Id, null, "tomt4", 61234, worker);

            #endregion

            #region fv5

            FieldValue field_Value5 =
                await testHelpers.CreateFieldValue(aCase, cl2, f5, ud.Id, null, "tomt5", 61234, worker);

            #endregion

            #endregion

            #endregion

            // Act

            var match = await sut.FieldValueRead(field_Value1, true, language);

            //// Assert

            #region Assert

            Assert.True(match is Microting.eForm.Infrastructure.Models.FieldValue);
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
//        public async Task SQL_Check_FieldValueRead_ReturnsTrue()
//        {
//            // Arrance
//
//            #region Arrance
//            #region Template1
//            DateTime cl1_Ca = DateTime.UtcNow;
//            DateTime cl1_Ua = DateTime.UtcNow;
//            check_lists cl1 = await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region Fields
//            #region field1
//
//
//            fields f1 = await testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, DbContext.field_types.Where(x => x.FieldType == "number").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//            #endregion
//
//            #region Worker
//
//            workers worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);
//
//            #endregion
//
//            #region site
//            sites site = await testHelpers.CreateSite("SiteName", 88);
//
//            #endregion
//
//            #region units
//            units unit = await testHelpers.CreateUnit(48, 49, site, 348);
//
//            #endregion
//
//            #region site_workers
//            site_workers site_workers = await testHelpers.CreateSiteWorker(55, site, worker);
//
//            #endregion
//
//            #region Case1
//
//            cases aCase = await testHelpers.CreateCase("caseUId", cl1, DateTime.UtcNow, "custom", DateTime.UtcNow,
//                worker, "microtingCheckUId", "microtingUId",
//               site, 66, "caseType", unit, DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);
//
//            #endregion
//
//            #region Check List Values
//            check_list_values check_List_Values = await testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);
//
//
//            #endregion
//
//            #region Field Values
//            #region fv1
//            field_values field_Value1 = await testHelpers.CreateFieldValue(aCase, cl2, f1, null, null, "tomt1", 61234, worker);
//
//            #endregion
//
//            #region fv2
//            field_values field_Value2 = await testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);
//
//            #endregion
//
//            #region fv3
//            field_values field_Value3 = await testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);
//
//            #endregion
//
//            #region fv4
//            field_values field_Value4 = await testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);
//
//            #endregion
//
//            #region fv5
//            field_values field_Value5 = await testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);
//
//            #endregion
//
//
//            #endregion
//            #endregion
//
//            // Act
//
//            var match = await sut.FieldValueRead(field_Value1.Id);
//
//            // Assert
//
//            Assert.AreEqual(field_Value1.Id, match.Id);
//
//        }
        [Test]
        public async Task SQL_Check_FieldValueReadList_ReturnsList()
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
                5, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55,
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
                83, 0, DbContext.FieldTypes.Where(x => x.Type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);

            #endregion

            #region field4

            Field f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "",
                "date Description",
                84, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0,
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

            Case aCase = await testHelpers.CreateCase("caseUId", cl1, DateTime.UtcNow, "custom", DateTime.UtcNow,
                worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 66, "caseType", unit, DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Check List Values

            CheckListValue checkListValue = await testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);

            #endregion

            #region Field Values

            #region fv1

            FieldValue field_Value1 =
                await testHelpers.CreateFieldValue(aCase, cl2, f1, null, null, "tomt1", 61234, worker);

            #endregion

            #region fv2

            FieldValue field_Value2 =
                await testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);

            #endregion

            #region fv3

            FieldValue field_Value3 =
                await testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);

            #endregion

            #region fv4

            FieldValue field_Value4 =
                await testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);

            #endregion

            #region fv5

            FieldValue field_Value5 =
                await testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);

            #endregion

            #endregion

            #endregion

            // Act

            List<Microting.eForm.Infrastructure.Models.FieldValue> match =
                await sut.FieldValueReadList(f1.Id, 5, language);

            // Assert

            Assert.AreEqual(field_Value1.Value, match[0].Value);
        }

        [Test]
        public async Task SQL_Check_FieldValueUpdate_UpdatesFieldValue()
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
                5, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55,
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
                83, 0, DbContext.FieldTypes.Where(x => x.Type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);

            #endregion

            #region field4

            Field f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "",
                "date Description",
                84, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0,
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

            Case aCase = await testHelpers.CreateCase("caseUId", cl1, DateTime.UtcNow, "custom", DateTime.UtcNow,
                worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 66, "caseType", unit, DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Check List Values

            CheckListValue checkListValue = await testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);

            #endregion

            #region Field Values

            #region fv1

            FieldValue field_Value1 =
                await testHelpers.CreateFieldValue(aCase, cl2, f1, null, null, "tomt1", 61234, worker);

            #endregion

            #region fv2

            FieldValue field_Value2 =
                await testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);

            #endregion

            #region fv3

            FieldValue field_Value3 =
                await testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);

            #endregion

            #region fv4

            FieldValue field_Value4 =
                await testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);

            #endregion

            #region fv5

            FieldValue field_Value5 =
                await testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);

            #endregion

            #endregion

            #endregion

            // Act

            await sut.FieldValueUpdate(aCase.Id, field_Value1.Id, "udfyldt");


            // Assert
            var newValue = await DbContext.FieldValues.AsNoTracking().FirstOrDefaultAsync(x => x.Id == field_Value1.Id);

            Assert.AreEqual(newValue.Value, "udfyldt");
        }

        [Test]
        public async Task SQL_Check_FieldValueReadAllValues_ReturnsReturnList()
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
                45, 1, DbContext.FieldTypes.Where(x => x.Type == "picture").First(), 0, 1, 0, 0,
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
                85, 0, DbContext.FieldTypes.Where(x => x.Type == "picture").First(), 1, 0, 1, 0,
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

            Case aCase = await testHelpers.CreateCase("caseUId", cl1, DateTime.UtcNow, "custom", DateTime.UtcNow,
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

            CheckListValue checkListValue = await testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);

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
            List<int> listOfCaseIds = new List<int>();
            listOfCaseIds.Add(aCase.Id);
            var matchF1 = await sut.FieldValueReadAllValues(f1.Id, listOfCaseIds, "mappe/", language, false);
            var matchF2 = await sut.FieldValueReadAllValues(f2.Id, listOfCaseIds, "mappe/", language, false);
            var matchF3 = await sut.FieldValueReadAllValues(f3.Id, listOfCaseIds, "mappe/", language, false);
            var matchF4 = await sut.FieldValueReadAllValues(f4.Id, listOfCaseIds, "mappe/", language, false);
            var matchF5 = await sut.FieldValueReadAllValues(f5.Id, listOfCaseIds, "mappe/", language, false);

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
        public async Task SQL_Check_ChecksRead_ReturnsListOfFieldValues()
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
                5, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55,
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
                83, 0, DbContext.FieldTypes.Where(x => x.Type == "number").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);

            #endregion

            #region field4

            Field f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "",
                "date Description",
                84, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0,
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

            Case aCase = await testHelpers.CreateCase("caseUId", cl1, DateTime.UtcNow, "custom", DateTime.UtcNow,
                worker, rnd.Next(1, 255), rnd.Next(1, 255),
                site, 66, "caseType", unit, DateTime.UtcNow, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Check List Values

            CheckListValue checkListValue = await testHelpers.CreateCheckListValue(aCase, cl2, "completed", null, 865);

            #endregion

            #region Field Values

            #region fv1

            FieldValue field_Value1 =
                await testHelpers.CreateFieldValue(aCase, cl2, f1, null, null, "tomt1", 61234, worker);

            #endregion

            #region fv2

            FieldValue field_Value2 =
                await testHelpers.CreateFieldValue(aCase, cl2, f2, null, null, "tomt2", 61234, worker);

            #endregion

            #region fv3

            FieldValue field_Value3 =
                await testHelpers.CreateFieldValue(aCase, cl2, f3, null, null, "tomt3", 61234, worker);

            #endregion

            #region fv4

            FieldValue field_Value4 =
                await testHelpers.CreateFieldValue(aCase, cl2, f4, null, null, "tomt4", 61234, worker);

            #endregion

            #region fv5

            FieldValue field_Value5 =
                await testHelpers.CreateFieldValue(aCase, cl2, f5, null, null, "tomt5", 61234, worker);

            #endregion

            #endregion

            #endregion

            // Act

            List<FieldValue> match = await sut.ChecksRead((int)aCase.MicrotingUid, (int)aCase.MicrotingCheckUid);

            // Assert


            Assert.AreEqual(field_Value1.Value, match[0].Value);
            Assert.AreEqual(field_Value2.Value, match[1].Value);
            Assert.AreEqual(field_Value3.Value, match[2].Value);
            Assert.AreEqual(field_Value4.Value, match[3].Value);
            Assert.AreEqual(field_Value5.Value, match[4].Value);
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