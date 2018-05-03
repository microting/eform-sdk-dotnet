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

        }


        #endregion


        #region sites


        #endregion

        #region workers


        #endregion

        #region site_workers


        #endregion

        #region units


        #endregion

        #region fields


        #endregion

        #region Entitygouplist


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