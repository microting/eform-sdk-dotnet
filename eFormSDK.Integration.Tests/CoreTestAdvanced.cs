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
    public class CoreTestAdvanced : DbTestFixture
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

        #region public advanced actions

        #region Template

        [Test]
        public void Core_AdvancedTemplate_Advanced_TemplateDisplayIndexChangeDb_ChangesDisplayIndex()
        {

            //Arrance
            #region Tempalte1

            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion
            //Act
            bool match = sut.Advanced_TemplateDisplayIndexChangeDb(cl1.id, 5);
            //Assert
            Assert.NotNull(match);
            Assert.True(match);

        }
        [Test]//skal bruge mock
        public void Core_AdvancedTemplate_Advanced_TemplateDisplayIndexChangeServer_ChangesDisplayIndex()
        {

            ////Arrance
            //#region Tempalte1

            //DateTime cl1_ca = DateTime.Now;
            //DateTime cl1_ua = DateTime.Now;
            //check_lists cl1 = testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            //#endregion

            //#region site
            //sites site = testHelpers.CreateSite("SiteName", 88);

            //#endregion
            ////Act
            //bool match = sut.Advanced_TemplateDisplayIndexChangeServer(cl1.id,(int)site.microting_uid, 5);
            ////Assert
            //Assert.NotNull(match);
            //Assert.True(match);

        }
        [Test]
        public void Core_AdvancedTemplate_Advanced_TemplateUpdateFieldIdsForColumns_ChangesIdsForColumns()
        {
            //Arrance
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

            //Act
            bool match = sut.Advanced_TemplateUpdateFieldIdsForColumns(cl1.id, f1.id, f2.id, f3.id, f4.id, f5.id, f6.id, f7.id, f8.id, f9.id, f10.id);

            //Assert
            Assert.NotNull(match);
            Assert.True(match);

        }
        [Test]
        public void Core_AdvancedTemplate_Advanced_TemplateFieldReadAll_returnslistofids()
        {
            #region Templates

            #region template1
            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            check_lists Template1 = testHelpers.CreateTemplate(cl1_ca, cl1_ua, "Label1", "Description1",
                "CaseType1", "FolderWithTemplate", 1, 0);

            #endregion

            #region template2
            DateTime cl2_ca = DateTime.Now;
            DateTime cl2_ua = DateTime.Now;
            check_lists Template2 = testHelpers.CreateTemplate(cl2_ca, cl2_ua, "Label2", "Description2",
                "CaseType2", "FolderWithTemplate", 0, 1);

            #endregion

            #region template3
            DateTime cl3_ca = DateTime.Now;
            DateTime cl3_ua = DateTime.Now;
            check_lists Template3 = testHelpers.CreateTemplate(cl3_ca, cl3_ua, "Label3", "Description3",
                "CaseType3", "FolderWithTemplate", 1, 1);

            #endregion

            #region template4
            DateTime cl4_ca = DateTime.Now;
            DateTime cl4_ua = DateTime.Now;
            check_lists Template4 = testHelpers.CreateTemplate(cl4_ca, cl4_ua, "Label4", "Description4",
                "CaseType4", "FolderWithTemplate", 0, 0);

            #endregion

            #endregion

            #region SubTemplates

            #region subTemplate1

            check_lists subTemplate1 = testHelpers.CreateSubTemplate("SubLabel1", "SubDescription1",
                "CaseType1", 1, 0, Template1);

            #endregion

            #region subTemplate2

            check_lists subTemplate2 = testHelpers.CreateSubTemplate("SubLabel2", "SubDescription2",
                "CaseType2", 0, 1, Template2);

            #endregion

            #region subTemplate3

            check_lists subTemplate3 = testHelpers.CreateSubTemplate("SubLabel3", "SubDescription3",
                "CaseType3", 1, 1, Template3);

            #endregion

            #region subTemplate4

            check_lists subTemplate4 = testHelpers.CreateSubTemplate("SubLabel4", "SubDescription4",
                "CaseType4", 0, 0, Template4);

            #endregion

            #endregion

            #region Fields

            #region Field1

            fields Field1 = testHelpers.CreateField(1, "barcode", subTemplate1, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region Field2

            fields Field2 = testHelpers.CreateField(1, "barcode", subTemplate1, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);

            #endregion

            #region Field3

            fields Field3 = testHelpers.CreateField(0, "barcode", subTemplate2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region Field4

            fields Field4 = testHelpers.CreateField(1, "barcode", subTemplate2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region Field5

            fields Field5 = testHelpers.CreateField(0, "barcode", subTemplate2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region Field6

            fields Field6 = testHelpers.CreateField(0, "barcode", subTemplate3, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region Field7

            fields Field7 = testHelpers.CreateField(0, "barcode", subTemplate3, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region Field8

            fields Field8 = testHelpers.CreateField(0, "barcode", subTemplate4, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region Field9

            fields Field9 = testHelpers.CreateField(0, "barcode", subTemplate4, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region Field10

            fields Field10 = testHelpers.CreateField(0, "barcode", subTemplate4, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion


            #endregion
            // Act

            var match1 = sut.Advanced_TemplateFieldReadAll(Template1.id);
            var match2 = sut.Advanced_TemplateFieldReadAll(Template2.id);
            var match3 = sut.Advanced_TemplateFieldReadAll(Template3.id);
            var match4 = sut.Advanced_TemplateFieldReadAll(Template4.id);

            // Assert
            #region template1
            Assert.NotNull(match1);
            Assert.AreEqual(match1[0].Description, Field1.description);
            Assert.AreEqual(match1[0].FieldType, Field1.field_type.field_type);
            Assert.AreEqual(match1[0].Label, Field1.label);
            Assert.AreEqual(match1[0].Id, Field1.id);

            Assert.AreEqual(match1[1].Description, Field2.description);
            Assert.AreEqual(match1[1].FieldType, Field2.field_type.field_type);
            Assert.AreEqual(match1[1].Label, Field2.label);
            Assert.AreEqual(match1[1].Id, Field2.id);
            #endregion

            #region template2
            Assert.NotNull(match2);
            Assert.AreEqual(match2[0].Description, Field3.description);
            Assert.AreEqual(match2[0].FieldType, Field3.field_type.field_type);
            Assert.AreEqual(match2[0].Label, Field3.label);
            Assert.AreEqual(match2[0].Id, Field3.id);

            Assert.AreEqual(match2[1].Description, Field4.description);
            Assert.AreEqual(match2[1].FieldType, Field4.field_type.field_type);
            Assert.AreEqual(match2[1].Label, Field4.label);
            Assert.AreEqual(match2[1].Id, Field4.id);

            Assert.AreEqual(match2[2].Description, Field5.description);
            Assert.AreEqual(match2[2].FieldType, Field5.field_type.field_type);
            Assert.AreEqual(match2[2].Label, Field5.label);
            Assert.AreEqual(match2[2].Id, Field5.id);
            #endregion

            #region template3
            Assert.NotNull(match3);
            Assert.AreEqual(match3[0].Description, Field6.description);
            Assert.AreEqual(match3[0].FieldType, Field6.field_type.field_type);
            Assert.AreEqual(match3[0].Label, Field6.label);
            Assert.AreEqual(match3[0].Id, Field6.id);

            Assert.AreEqual(match3[1].Description, Field7.description);
            Assert.AreEqual(match3[1].FieldType, Field7.field_type.field_type);
            Assert.AreEqual(match3[1].Label, Field7.label);
            Assert.AreEqual(match3[1].Id, Field7.id);
            #endregion

            #region template4
            Assert.NotNull(match4);
            Assert.AreEqual(match4[0].Description, Field8.description);
            Assert.AreEqual(match4[0].FieldType, Field8.field_type.field_type);
            Assert.AreEqual(match4[0].Label, Field8.label);
            Assert.AreEqual(match4[0].Id, Field8.id);

            Assert.AreEqual(match4[1].Description, Field9.description);
            Assert.AreEqual(match4[1].FieldType, Field9.field_type.field_type);
            Assert.AreEqual(match4[1].Label, Field9.label);
            Assert.AreEqual(match4[1].Id, Field9.id);

            Assert.AreEqual(match4[2].Description, Field10.description);
            Assert.AreEqual(match4[2].FieldType, Field10.field_type.field_type);
            Assert.AreEqual(match4[2].Label, Field10.label);
            Assert.AreEqual(match4[2].Id, Field10.id);
            #endregion



        }
        [Test]
        public void Core_AdvancedTemplate_Advanced_ConsistencyCheckTemplates()
        {
            //Arrance

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

            //Act
            var removedcl2 = cl2.workflow_state = Constants.WorkflowStates.Removed;
            var removedcl3 = cl3.workflow_state = Constants.WorkflowStates.Removed;
            var removedcl4 = cl4.workflow_state = Constants.WorkflowStates.Removed;
            var removedcl5 = cl5.workflow_state = Constants.WorkflowStates.Removed;
            var removedcl6 = cl6.workflow_state = Constants.WorkflowStates.Removed;
            var removedcl7 = cl7.workflow_state = Constants.WorkflowStates.Removed;
            var removedcl8 = cl8.workflow_state = Constants.WorkflowStates.Removed;
            var removedcl9 = cl9.workflow_state = Constants.WorkflowStates.Removed;
            var removedcl10 = cl10.workflow_state = Constants.WorkflowStates.Removed;
            var removedcl11 = cl11.workflow_state = Constants.WorkflowStates.Removed;
            //Assert
            sut.Advanced_ConsistencyCheckTemplates();


        }


        #endregion

        #region site_workers

        [Test]//mangler mock
        public void Core_SiteWorkers_Advanced_SiteWorkerCreate_CreatesWorker()
        {
            //Arrance
            #region checklist
            DateTime cl_ca = DateTime.Now;
            DateTime cl_ua = DateTime.Now;
            check_lists Cl1 = testHelpers.CreateTemplate(cl_ca, cl_ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);
            #endregion

            #region SubCheckList
            check_lists Cl2 = testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);
            #endregion

            #region Fields

            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #endregion

            #region Workers

            #region worker1
            workers worker1 = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region worker2
            workers worker2 = testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);

            #endregion

            #region worker3
            workers worker3 = testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);

            #endregion

            #region worker4
            workers worker4 = testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);

            #endregion

            #region worker5
            workers worker5 = testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);

            #endregion

            #region worker6
            workers worker6 = testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);

            #endregion

            #region worker7
            workers worker7 = testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);

            #endregion

            #region worker8
            workers worker8 = testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);

            #endregion

            #region worker9
            workers worker9 = testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);

            #endregion

            #region worker10
            workers worker10 = testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);

            #endregion

            #endregion

            #region sites

            #region Site1
            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2
            sites site2 = testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3
            sites site3 = testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4
            sites site4 = testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5
            sites site5 = testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6
            sites site6 = testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7
            sites site7 = testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8
            sites site8 = testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9
            sites site9 = testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10
            sites site10 = testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units
            units unit = testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion

            #region site_workers
            site_workers site_workers = testHelpers.CreateSiteWorker(55, site1, worker1);
            #endregion



            SiteName_Dto siteWorkerDto = new SiteName_Dto();
            
            


            var workerDto = new Worker_Dto();




            //Act

            //TODO
         
            //var match2 = sut.Advanced_SiteWorkerCreate(siteWorkerDto, workerDto);
        

            //Assert

            //Assert.NotNull(match2);


        }
        [Test]
        public void Core_SiteWorkers_Advanced_SiteWorkerRead_ReadsSiteWorker()
        {
            //Arrance
            #region Arrance

            #region Checklist
            DateTime cl_ca = DateTime.Now;
            DateTime cl_ua = DateTime.Now;
            check_lists Cl1 = testHelpers.CreateTemplate(cl_ca,cl_ua,"A1", "D1", "caseType1", "WhereItIs", 1, 0);

            #endregion

            #region SubCheckList

            check_lists Cl2 = testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);

            #endregion

            #region Fields

            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #endregion

            #region Workers

            #region worker1
            workers worker1 = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region worker2
            workers worker2 = testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);

            #endregion

            #region worker3
            workers worker3 = testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);

            #endregion

            #region worker4
            workers worker4 = testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);

            #endregion

            #region worker5
            workers worker5 = testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);

            #endregion

            #region worker6
            workers worker6 = testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);

            #endregion

            #region worker7
            workers worker7 = testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);

            #endregion

            #region worker8
            workers worker8 = testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);

            #endregion

            #region worker9
            workers worker9 = testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);

            #endregion

            #region worker10
            workers worker10 = testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);

            #endregion

            #endregion

            #region sites

            #region Site1
            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2
            sites site2 = testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3
            sites site3 = testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4
            sites site4 = testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5
            sites site5 = testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6
            sites site6 = testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7
            sites site7 = testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8
            sites site8 = testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9
            sites site9 = testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10
            sites site10 = testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units
            units unit = testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion

            #region site_workers
            site_workers site_workers = testHelpers.CreateSiteWorker(55, site1, worker1);

            #endregion

            #endregion
            //Act
            var match = sut.Advanced_SiteWorkerRead(site_workers.microting_uid, site1.id, worker1.id);
            //Assert
            Assert.NotNull(match);
            Assert.AreEqual(match.MicrotingUId, site_workers.microting_uid);
            Assert.AreEqual(match.SiteUId, site_workers.site_id);
            Assert.AreEqual(match.WorkerUId, site_workers.worker_id);



        }
        [Test]//mangler mock
        public void Core_SiteWorkers_Advanced_SiteWorkerDelete_MarksAsRemoved()
        {



        }
        
        

        #endregion

        #region units
        [Test]
        public void Core_Unit_Advanced_UnitRead_ReadsUnit()
        {
            //Arrance
            #region Checklist
            DateTime cl_ca = DateTime.Now;
            DateTime cl_ua = DateTime.Now;
            check_lists Cl1 = testHelpers.CreateTemplate(cl_ca, cl_ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);

            #endregion

            #region SubCheckList

            check_lists Cl2 = testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);

            #endregion

            #region Fields

            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #endregion

            #region Workers

            #region worker1
            workers worker1 = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region worker2
            workers worker2 = testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);

            #endregion

            #region worker3
            workers worker3 = testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);

            #endregion

            #region worker4
            workers worker4 = testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);

            #endregion

            #region worker5
            workers worker5 = testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);

            #endregion

            #region worker6
            workers worker6 = testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);

            #endregion

            #region worker7
            workers worker7 = testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);

            #endregion

            #region worker8
            workers worker8 = testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);

            #endregion

            #region worker9
            workers worker9 = testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);

            #endregion

            #region worker10
            workers worker10 = testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);

            #endregion

            #endregion

            #region sites

            #region Site1
            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2
            sites site2 = testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3
            sites site3 = testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4
            sites site4 = testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5
            sites site5 = testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6
            sites site6 = testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7
            sites site7 = testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8
            sites site8 = testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9
            sites site9 = testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10
            sites site10 = testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units
            units unit = testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion
            //Act

            Unit_Dto match = sut.Advanced_UnitRead((int) unit.microting_uid);

            //Assert
            Assert.NotNull(match);
            Assert.AreEqual(unit.microting_uid, match.UnitUId);
            Assert.AreEqual(unit.customer_no, match.CustomerNo);

        }
        [Test]
        public void Core_Unit_Advanced_UnitReadAll_ReturnsListOfUnits()
        {
            #region Arrance

            #region Checklist
            DateTime cl_ca = DateTime.Now;
            DateTime cl_ua = DateTime.Now;
            check_lists Cl1 = testHelpers.CreateTemplate(cl_ca, cl_ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);

            #endregion

            #region SubCheckList

            check_lists Cl2 = testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);

            #endregion

            #region Fields

            #region field1


            fields f1 = testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
                5, 1, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region field2


            fields f2 = testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
                45, 1, DbContext.field_types.Where(x => x.field_type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);


            #endregion

            #region field3

            fields f3 = testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
                83, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);


            #endregion

            #region field4


            fields f4 = testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
                84, 0, DbContext.field_types.Where(x => x.field_type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field5

            fields f5 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                85, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field6

            fields f6 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                86, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field7

            fields f7 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                87, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field8

            fields f8 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                88, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field9

            fields f9 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                89, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #region field10

            fields f10 = testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
                90, 0, DbContext.field_types.Where(x => x.field_type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);


            #endregion

            #endregion

            #region Workers

            #region worker1
            workers worker1 = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region worker2
            workers worker2 = testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);

            #endregion

            #region worker3
            workers worker3 = testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);

            #endregion

            #region worker4
            workers worker4 = testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);

            #endregion

            #region worker5
            workers worker5 = testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);

            #endregion

            #region worker6
            workers worker6 = testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);

            #endregion

            #region worker7
            workers worker7 = testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);

            #endregion

            #region worker8
            workers worker8 = testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);

            #endregion

            #region worker9
            workers worker9 = testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);

            #endregion

            #region worker10
            workers worker10 = testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);

            #endregion

            #endregion

            #region sites

            #region Site1
            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2
            sites site2 = testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3
            sites site3 = testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4
            sites site4 = testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5
            sites site5 = testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6
            sites site6 = testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7
            sites site7 = testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8
            sites site8 = testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9
            sites site9 = testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10
            sites site10 = testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units

            #region Unit1
            units unit1 = testHelpers.CreateUnit(48, 49, site1, 348);
            #endregion

            #region Unit2
            units unit2 = testHelpers.CreateUnit(2, 55, site2, 349);
            #endregion

            #region Unit3
            units unit3 = testHelpers.CreateUnit(3, 51, site3, 350);
            #endregion

            #region Unit4
            units unit4 = testHelpers.CreateUnit(4, 52, site4, 351);
            #endregion

            #region Unit5
            units unit5 = testHelpers.CreateUnit(5, 6, site5, 352);
            #endregion

            #region Unit6
            units unit6 = testHelpers.CreateUnit(6, 85, site6, 353);
            #endregion

            #region Unit7
            units unit7 = testHelpers.CreateUnit(7, 62, site7, 354);
            #endregion

            #region Unit8
            units unit8 = testHelpers.CreateUnit(8, 96, site8, 355);
            #endregion

            #region Unit9
            units unit9 = testHelpers.CreateUnit(9, 69, site9, 356);
            #endregion

            #region Unit10
            units unit10 = testHelpers.CreateUnit(10, 100, site10, 357);
            #endregion


            #endregion

            #region site_workers
            site_workers site_workers = testHelpers.CreateSiteWorker(55, site1, worker1);

            #endregion

            #endregion
            // Act

            var getAllUnits = sut.Advanced_UnitReadAll();

            // Assert

            Assert.AreEqual(10, getAllUnits.Count());

            Assert.AreEqual(unit1.microting_uid, getAllUnits[0].UnitUId);
            Assert.AreEqual(unit2.microting_uid, getAllUnits[1].UnitUId);
            Assert.AreEqual(unit3.microting_uid, getAllUnits[2].UnitUId);
            Assert.AreEqual(unit4.microting_uid, getAllUnits[3].UnitUId);
            Assert.AreEqual(unit5.microting_uid, getAllUnits[4].UnitUId);
            Assert.AreEqual(unit6.microting_uid, getAllUnits[5].UnitUId);
            Assert.AreEqual(unit7.microting_uid, getAllUnits[6].UnitUId);
            Assert.AreEqual(unit8.microting_uid, getAllUnits[7].UnitUId);
            Assert.AreEqual(unit9.microting_uid, getAllUnits[8].UnitUId);
            Assert.AreEqual(unit10.microting_uid, getAllUnits[9].UnitUId);
        }

        #endregion

        #region fields
        [Test]  
        public void Core_Advanced_FieldRead_ReadsFieldId()
        {
            // Arrance

            #region Template1
            DateTime cl_ca = DateTime.Now;
            DateTime cl_ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl_ca, cl_ua,"A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


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
                83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
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
            #endregion

            // Act

            Field match = sut.Advanced_FieldRead(f1.id);

            // Assert

            Assert.AreEqual(f1.id, match.Id);

        }
        [Test]
        public void Core_Advanced_FieldValueReadList_ReturnsList()
        {
            // Arrance
            #region Arrance
            #region Template1
            DateTime cl_ca = DateTime.Now;
            DateTime cl_ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl_ca, cl_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region SubTemplate1
            check_lists cl2 = testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);


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
                83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
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

            List<FieldValue> match = sut.Advanced_FieldValueReadList(f1.id, 5);

            // Assert

            Assert.AreEqual(field_Value1.value, match[0].Value);



        }


        #endregion

        #region Entitygouplist
        [Test]
        public void Core_Advanced_DeleteUploadedData_DeletesData()
        {
            //Arrance
            #region Template1
            DateTime cl_ca = DateTime.Now;
            DateTime cl_ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl_ca, cl_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

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
                83, 0, DbContext.field_types.Where(x => x.field_type == "number").First(), 0, 0, 1, 0,
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
            #endregion

            #region Worker

            workers worker = testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region sites

            #region Site1
            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2
            sites site2 = testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3
            sites site3 = testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4
            sites site4 = testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5
            sites site5 = testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6
            sites site6 = testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7
            sites site7 = testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8
            sites site8 = testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9
            sites site9 = testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10
            sites site10 = testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units
            units unit = testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion

            #region site_workers
            site_workers site_workers = testHelpers.CreateSiteWorker(55, site1, worker);

            #endregion

            #region UploadedData
            #region ud1
            uploaded_data ud1 = testHelpers.CreateUploadedData("", "File.jpg", "jpg", path + @"\output\dataFolder\picture\", "File.jpg", 1, worker,
                Constants.UploaderTypes.System, 55, true);
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
            //Act
            bool match = sut.Advanced_DeleteUploadedData(f2.id, ud1.id);
            //Assert
            Assert.True(match);

        }
        [Test]
        public void Core_Advanced_UpdateCaseFieldValue_UpdatesFieldValue()
        {
            //Arrance
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

            //Act
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

            var testThis = sut.Advanced_UpdateCaseFieldValue(aCase1.id);

            //Assert
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


        #endregion


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