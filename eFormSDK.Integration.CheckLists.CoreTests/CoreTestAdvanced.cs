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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using NUnit.Framework;
using Case = Microting.eForm.Infrastructure.Data.Entities.Case;

namespace eFormSDK.Integration.CheckLists.CoreTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class CoreTestAdvanced : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;
        Random rnd = new Random();
        short shortMinValue = Int16.MinValue;
        short shortmaxValue = Int16.MaxValue;
        private Language language;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef").ConfigureAwait(false);
            await sql.SettingUpdate(Settings.firstRunDone, "true").ConfigureAwait(false);
            await sql.SettingUpdate(Settings.knownSitesDone, "true").ConfigureAwait(false);
            await sql.SettingUpdate(Settings.comAddressNewApi, "none");

            #endregion

            sut = new Core();
            sut.HandleCaseCreated += EventCaseCreated;
            sut.HandleCaseRetrived += EventCaseRetrived;
            sut.HandleCaseCompleted += EventCaseCompleted;
            sut.HandleCaseDeleted += EventCaseDeleted;
            sut.HandleFileDownloaded += EventFileDownloaded;
            sut.HandleSiteActivated += EventSiteActivated;
            await sut.StartSqlOnly(ConnectionString).ConfigureAwait(false);
            path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path);
            await sut.SetSdkSetting(Settings.fileLocationPicture,
                Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers(ConnectionString);
            await testHelpers.GenerateDefaultLanguages();
            language = DbContext.Languages.Single(x => x.Name == "Dansk");
            //sut.StartLog(new CoreBase());
        }

        #region public advanced actions

        #region Template

        [Test]
        public async Task Core_AdvancedTemplate_Advanced_TemplateDisplayIndexChangeDb_ChangesDisplayIndex()
        {
            // Arrange

            #region Tempalte1

            DateTime cl1_ca = DateTime.UtcNow;
            DateTime cl1_ua = DateTime.UtcNow;
            CheckList cl1 = await testHelpers
                .CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1)
                .ConfigureAwait(false);

            #endregion

            // Act
            bool match = await sut.Advanced_TemplateDisplayIndexChangeDb(cl1.Id, 5).ConfigureAwait(false);
            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match, Is.True);
        }

        [Test] // TODO Needs to use mocks
#pragma warning disable 1998
        public async Task Core_AdvancedTemplate_Advanced_TemplateDisplayIndexChangeServer_ChangesDisplayIndex()
        {
            //// Arrange
            //#region Tempalte1

            //DateTime cl1_ca = DateTime.UtcNow;
            //DateTime cl1_ua = DateTime.UtcNow;
            //check_lists cl1 = await testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            //#endregion

            //#region site
            //sites site = await testHelpers.CreateSite("SiteName", 88);

            //#endregion
            //// Act
            //bool match = await sut.Advanced_TemplateDisplayIndexChangeServer(cl1.Id,(int)site.microting_uid, 5);
            //// Assert
            // Assert.That(match, Is.Not.EqualTo(null));
            // Assert.True(match);
        }
#pragma warning restore 1998

        [Test]
        public async Task Core_AdvancedTemplate_Advanced_TemplateUpdateFieldIdsForColumns_ChangesIdsForColumns()
        {
            // Arrange

            #region Template1

            DateTime cl1_Ca = DateTime.UtcNow;
            DateTime cl1_Ua = DateTime.UtcNow;
            CheckList cl1 = await testHelpers
                .CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1)
                .ConfigureAwait(false);

            #endregion

            #region subtemplates

            #region SubTemplate1

            CheckList cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1)
                .ConfigureAwait(false);

            #endregion

            #region SubTemplate1

            CheckList cl3 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1)
                .ConfigureAwait(false);

            #endregion

            #region SubTemplate1

            CheckList cl4 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1)
                .ConfigureAwait(false);

            #endregion

            #region SubTemplate1

            CheckList cl5 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1)
                .ConfigureAwait(false);

            #endregion

            #region SubTemplate1

            CheckList cl6 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1)
                .ConfigureAwait(false);

            #endregion

            #region SubTemplate1

            CheckList cl7 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1)
                .ConfigureAwait(false);

            #endregion

            #region SubTemplate1

            CheckList cl8 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1)
                .ConfigureAwait(false);

            #endregion

            #region SubTemplate1

            CheckList cl9 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1)
                .ConfigureAwait(false);

            #endregion

            #region SubTemplate1

            CheckList cl10 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1)
                .ConfigureAwait(false);

            #endregion

            #region SubTemplate1

            CheckList cl11 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1)
                .ConfigureAwait(false);

            #endregion

            #endregion

            #region Fields

            #region field1

            Field f1 = await testHelpers.CreateField(1, "barcode", cl2, "e2f4fb", "custom", null, "",
                "Comment field description",
                5, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0, "Comment field", 1, 55,
                "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49).ConfigureAwait(false);

            #endregion

            #region field2

            Field f2 = await testHelpers.CreateField(1, "barcode", cl2, "f5eafa", "custom", null, "",
                "showPDf Description",
                45, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9).ConfigureAwait(false);

            #endregion

            #region field3

            Field f3 = await testHelpers.CreateField(0, "barcode", cl2, "f0f8db", "custom", 3, "",
                "Number Field Description",
                83, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1).ConfigureAwait(false);

            #endregion

            #region field4

            Field f4 = await testHelpers.CreateField(1, "barcode", cl2, "fff6df", "custom", null, "",
                "date Description",
                84, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1).ConfigureAwait(false);

            #endregion

            #region field5

            Field f5 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                85, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1).ConfigureAwait(false);

            #endregion

            #region field6

            Field f6 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                86, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1).ConfigureAwait(false);

            #endregion

            #region field7

            Field f7 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                87, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1).ConfigureAwait(false);

            #endregion

            #region field8

            Field f8 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                88, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1).ConfigureAwait(false);

            #endregion

            #region field9

            Field f9 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                89, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1).ConfigureAwait(false);

            #endregion

            #region field10

            Field f10 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "",
                "picture Description",
                90, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1).ConfigureAwait(false);

            #endregion

            #endregion

            // Act
            bool match = await sut
                .Advanced_TemplateUpdateFieldIdsForColumns(cl1.Id, f1.Id, f2.Id, f3.Id, f4.Id, f5.Id, f6.Id, f7.Id,
                    f8.Id, f9.Id, f10.Id).ConfigureAwait(false);

            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match, Is.True);
        }

        [Test]
        public async Task Core_AdvancedTemplate_Advanced_TemplateFieldReadAll_returnslistofids()
        {
            #region Templates

            #region template1

            DateTime cl1_ca = DateTime.UtcNow;
            DateTime cl1_ua = DateTime.UtcNow;
            CheckList Template1 = await testHelpers.CreateTemplate(cl1_ca, cl1_ua, "Label1", "Description1",
                "CaseType1", "FolderWithTemplate", 1, 0);

            #endregion

            #region template2

            DateTime cl2_ca = DateTime.UtcNow;
            DateTime cl2_ua = DateTime.UtcNow;
            CheckList Template2 = await testHelpers.CreateTemplate(cl2_ca, cl2_ua, "Label2", "Description2",
                "CaseType2", "FolderWithTemplate", 0, 1);

            #endregion

            #region template3

            DateTime cl3_ca = DateTime.UtcNow;
            DateTime cl3_ua = DateTime.UtcNow;
            CheckList Template3 = await testHelpers.CreateTemplate(cl3_ca, cl3_ua, "Label3", "Description3",
                "CaseType3", "FolderWithTemplate", 1, 1);

            #endregion

            #region template4

            DateTime cl4_ca = DateTime.UtcNow;
            DateTime cl4_ua = DateTime.UtcNow;
            CheckList Template4 = await testHelpers.CreateTemplate(cl4_ca, cl4_ua, "Label4", "Description4",
                "CaseType4", "FolderWithTemplate", 0, 0);

            #endregion

            #endregion

            #region SubTemplates

            #region subTemplate1

            CheckList subTemplate1 = await testHelpers.CreateSubTemplate("SubLabel1", "SubDescription1",
                "CaseType1", 1, 0, Template1);

            #endregion

            #region subTemplate2

            CheckList subTemplate2 = await testHelpers.CreateSubTemplate("SubLabel2", "SubDescription2",
                "CaseType2", 0, 1, Template2);

            #endregion

            #region subTemplate3

            CheckList subTemplate3 = await testHelpers.CreateSubTemplate("SubLabel3", "SubDescription3",
                "CaseType3", 1, 1, Template3);

            #endregion

            #region subTemplate4

            CheckList subTemplate4 = await testHelpers.CreateSubTemplate("SubLabel4", "SubDescription4",
                "CaseType4", 0, 0, Template4);

            #endregion

            #endregion

            #region Fields

            #region Field1

            Field Field1 = await testHelpers.CreateField(1, "barcode", subTemplate1, "e2f4fb", "custom", null, "",
                "Comment field description",
                5, 0, DbContext.FieldTypes.Where(x => x.Type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55,
                "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region Field2

            Field Field2 = await testHelpers.CreateField(1, "barcode", subTemplate1, "f5eafa", "custom", null, "",
                "showPDf Description",
                45, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 1, 0, 0,
                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);

            #endregion

            #region Field3

            Field Field3 = await testHelpers.CreateField(0, "barcode", subTemplate2, "f0f8db", "custom", 3, "",
                "Number Field Description",
                83, 0, DbContext.FieldTypes.Where(x => x.Type == "picture").First(), 0, 0, 1, 0,
                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);

            #endregion

            #region Field4

            Field Field4 = await testHelpers.CreateField(1, "barcode", subTemplate2, "fff6df", "custom", null, "",
                "date Description",
                84, 0, DbContext.FieldTypes.Where(x => x.Type == "picture").First(), 0, 0, 1, 0,
                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region Field5

            Field Field5 = await testHelpers.CreateField(0, "barcode", subTemplate2, "ffe4e4", "custom", null, "",
                "picture Description",
                85, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region Field6

            Field Field6 = await testHelpers.CreateField(0, "barcode", subTemplate3, "ffe4e4", "custom", null, "",
                "picture Description",
                86, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region Field7

            Field Field7 = await testHelpers.CreateField(0, "barcode", subTemplate3, "ffe4e4", "custom", null, "",
                "picture Description",
                87, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region Field8

            Field Field8 = await testHelpers.CreateField(0, "barcode", subTemplate4, "ffe4e4", "custom", null, "",
                "picture Description",
                88, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region Field9

            Field Field9 = await testHelpers.CreateField(0, "barcode", subTemplate4, "ffe4e4", "custom", null, "",
                "picture Description",
                89, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #region Field10

            Field Field10 = await testHelpers.CreateField(0, "barcode", subTemplate4, "ffe4e4", "custom", null, "",
                "picture Description",
                90, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 1, 0, 1, 0,
                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);

            #endregion

            #endregion

            // Act

            var match1 = await sut.Advanced_TemplateFieldReadAll(Template1.Id, language);
            var match2 = await sut.Advanced_TemplateFieldReadAll(Template2.Id, language);
            var match3 = await sut.Advanced_TemplateFieldReadAll(Template3.Id, language);
            var match4 = await sut.Advanced_TemplateFieldReadAll(Template4.Id, language);

            // Assert

            #region template1

            Assert.That(match1, Is.Not.EqualTo(null));
            Assert.That(Field1.Description, Is.EqualTo(match1[0].Description));
            Assert.That(match1[0].FieldType, Is.EqualTo("Picture"));
            Assert.That(Field1.Label, Is.EqualTo(match1[0].Label));
            Assert.That(Field1.Id, Is.EqualTo(match1[0].Id));

            Assert.That(Field2.Description, Is.EqualTo(match1[1].Description));
            Assert.That(match1[1].FieldType, Is.EqualTo("Comment"));
            Assert.That(Field2.Label, Is.EqualTo(match1[1].Label));
            Assert.That(Field2.Id, Is.EqualTo(match1[1].Id));

            #endregion

            #region template2

            Assert.That(match2, Is.Not.EqualTo(null));
            Assert.That(Field3.Description, Is.EqualTo(match2[0].Description));
            Assert.That(match2[0].FieldType, Is.EqualTo("Picture"));
            Assert.That(Field3.Label, Is.EqualTo(match2[0].Label));
            Assert.That(Field3.Id, Is.EqualTo(match2[0].Id));

            Assert.That(Field4.Description, Is.EqualTo(match2[1].Description));
            Assert.That(match2[1].FieldType, Is.EqualTo("Picture"));
            Assert.That(Field4.Label, Is.EqualTo(match2[1].Label));
            Assert.That(Field4.Id, Is.EqualTo(match2[1].Id));

            Assert.That(Field5.Description, Is.EqualTo(match2[2].Description));
            Assert.That(match2[2].FieldType, Is.EqualTo("Comment"));
            Assert.That(Field5.Label, Is.EqualTo(match2[2].Label));
            Assert.That(Field5.Id, Is.EqualTo(match2[2].Id));

            #endregion

            #region template3

            Assert.That(match3, Is.Not.EqualTo(null));
            Assert.That(Field6.Description, Is.EqualTo(match3[0].Description));
            Assert.That(match3[0].FieldType, Is.EqualTo("Comment"));
            Assert.That(Field6.Label, Is.EqualTo(match3[0].Label));
            Assert.That(Field6.Id, Is.EqualTo(match3[0].Id));

            Assert.That(Field7.Description, Is.EqualTo(match3[1].Description));
            Assert.That(match3[1].FieldType, Is.EqualTo("Comment"));
            Assert.That(Field7.Label, Is.EqualTo(match3[1].Label));
            Assert.That(Field7.Id, Is.EqualTo(match3[1].Id));

            #endregion

            #region template4

            Assert.That(match4, Is.Not.EqualTo(null));
            Assert.That(Field8.Description, Is.EqualTo(match4[0].Description));
            Assert.That(match4[0].FieldType, Is.EqualTo("Comment"));
            Assert.That(Field8.Label, Is.EqualTo(match4[0].Label));
            Assert.That(Field8.Id, Is.EqualTo(match4[0].Id));

            Assert.That(Field9.Description, Is.EqualTo(match4[1].Description));
            Assert.That(match4[1].FieldType, Is.EqualTo("Comment"));
            Assert.That(Field9.Label, Is.EqualTo(match4[1].Label));
            Assert.That(Field9.Id, Is.EqualTo(match4[1].Id));

            Assert.That(Field10.Description, Is.EqualTo(match4[2].Description));
            Assert.That(match4[2].FieldType, Is.EqualTo("Comment"));
            Assert.That(Field10.Label, Is.EqualTo(match4[2].Label));
            Assert.That(Field10.Id, Is.EqualTo(match4[2].Id));

            #endregion
        }
//        [Test]
//        public async Task Core_AdvancedTemplate_Advanced_ConsistencyCheckTemplates()
//        {
//            // Arrange
//
//            #region Arrance
//            #region Template1
//            DateTime cl1_Ca = DateTime.UtcNow;
//            DateTime cl1_Ua = DateTime.UtcNow;
//            check_lists cl1 = await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);
//
//            #endregion
//
//            #region subtemplates
//            #region SubTemplate1
//            check_lists cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl3 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl4 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl5 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl6 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl7 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl8 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl9 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl10 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
//
//            #region SubTemplate1
//            check_lists cl11 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);
//
//
//            #endregion
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
//                83, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 0, 1, 0,
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
//
//            #region field6
//
//            fields f6 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                86, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field7
//
//            fields f7 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                87, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field8
//
//            fields f8 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                88, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field9
//
//            fields f9 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                89, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field10
//
//            fields f10 = await testHelpers.CreateField(0, "barcode", cl2, "ffe4e4", "custom", null, "", "picture Description",
//                90, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
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
//            #region cases
//            #region cases created
//            #region Case1
//
//            DateTime c1_ca = DateTime.UtcNow.AddDays(-9);
//            DateTime c1_da = DateTime.UtcNow.AddDays(-8).AddHours(-12);
//            DateTime c1_ua = DateTime.UtcNow.AddDays(-8);
//
//            cases aCase1 = await testHelpers.CreateCase("case1UId", cl2, c1_ca, "custom1",
//                c1_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
//               site, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);
//
//            #endregion
//
//            #region Case2
//
//            DateTime c2_ca = DateTime.UtcNow.AddDays(-7);
//            DateTime c2_da = DateTime.UtcNow.AddDays(-6).AddHours(-12);
//            DateTime c2_ua = DateTime.UtcNow.AddDays(-6);
//            cases aCase2 = await testHelpers.CreateCase("case2UId", cl1, c2_ca, "custom2",
//             c2_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
//               site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);
//            #endregion
//
//            #region Case3
//            DateTime c3_ca = DateTime.UtcNow.AddDays(-10);
//            DateTime c3_da = DateTime.UtcNow.AddDays(-9).AddHours(-12);
//            DateTime c3_ua = DateTime.UtcNow.AddDays(-9);
//
//            cases aCase3 = await testHelpers.CreateCase("case3UId", cl1, c3_ca, "custom3",
//              c3_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
//               site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);
//            #endregion
//
//            #region Case4
//            DateTime c4_ca = DateTime.UtcNow.AddDays(-8);
//            DateTime c4_da = DateTime.UtcNow.AddDays(-7).AddHours(-12);
//            DateTime c4_ua = DateTime.UtcNow.AddDays(-7);
//
//            cases aCase4 = await testHelpers.CreateCase("case4UId", cl1, c4_ca, "custom4",
//                c4_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
//               site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);
//            #endregion
//            #endregion
//
//            #endregion
//
//            #region UploadedData
//            #region ud1
//            uploaded_data ud1 = await testHelpers.CreateUploadedData("checksum1", "File1", "no", "hjgjghjhg", "File1", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud2
//            uploaded_data ud2 = await testHelpers.CreateUploadedData("checksum2", "File1", "no", "hjgjghjhg", "File2", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud3
//            uploaded_data ud3 = await testHelpers.CreateUploadedData("checksum3", "File1", "no", "hjgjghjhg", "File3", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud4
//            uploaded_data ud4 = await testHelpers.CreateUploadedData("checksum4", "File1", "no", "hjgjghjhg", "File4", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud5
//            uploaded_data ud5 = await testHelpers.CreateUploadedData("checksum5", "File1", "no", "hjgjghjhg", "File5", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud6
//            uploaded_data ud6 = await testHelpers.CreateUploadedData("checksum6", "File1", "no", "hjgjghjhg", "File6", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud7
//            uploaded_data ud7 = await testHelpers.CreateUploadedData("checksum7", "File1", "no", "hjgjghjhg", "File7", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud8
//            uploaded_data ud8 = await testHelpers.CreateUploadedData("checksum8", "File1", "no", "hjgjghjhg", "File8", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud9
//            uploaded_data ud9 = await testHelpers.CreateUploadedData("checksum9", "File1", "no", "hjgjghjhg", "File9", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #region ud10
//            uploaded_data ud10 = await testHelpers.CreateUploadedData("checksum10", "File1", "no", "hjgjghjhg", "File10", 1, worker,
//                "local", 55, false);
//            #endregion
//
//            #endregion
//
//            #region Check List Values
//            #region clv1
//            check_list_values clv1 = await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);
//            #endregion
//
//            #region clv2
//            check_list_values clv2 = await testHelpers.CreateCheckListValue(aCase1, cl3, Constants.CheckListValues.Checked, null, 861);
//            #endregion
//
//            #region clv3
//            check_list_values clv3 = await testHelpers.CreateCheckListValue(aCase1, cl4, Constants.CheckListValues.Checked, null, 862);
//            #endregion
//
//            #region clv4
//            check_list_values clv4 = await testHelpers.CreateCheckListValue(aCase1, cl5, Constants.CheckListValues.NotChecked, null, 863);
//            #endregion
//
//            #region clv5
//            check_list_values clv5 = await testHelpers.CreateCheckListValue(aCase1, cl6, Constants.CheckListValues.NotChecked, null, 864);
//            #endregion
//
//            #region clv6
//            check_list_values clv6 = await testHelpers.CreateCheckListValue(aCase1, cl7, Constants.CheckListValues.NotChecked, null, 865);
//            #endregion
//
//            #region clv7
//            check_list_values clv7 = await testHelpers.CreateCheckListValue(aCase1, cl8, Constants.CheckListValues.NotApproved, null, 866);
//            #endregion
//
//            #region clv8
//            check_list_values clv8 = await testHelpers.CreateCheckListValue(aCase1, cl9, Constants.CheckListValues.NotApproved, null, 867);
//            #endregion
//
//            #region clv9
//            check_list_values clv9 = await testHelpers.CreateCheckListValue(aCase1, cl10, Constants.CheckListValues.NotApproved, null, 868);
//            #endregion
//
//            #region clv10
//            check_list_values clv10 = await testHelpers.CreateCheckListValue(aCase1, cl11, Constants.CheckListValues.NotApproved, null, 869);
//            #endregion
//
//            #endregion
//
//            #region Field Values
//            #region fv1
//            field_values field_Value1 = await testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.Id, null, "tomt1", 61230, worker);
//
//            #endregion
//
//            #region fv2
//            field_values field_Value2 = await testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.Id, null, "tomt2", 61231, worker);
//
//            #endregion
//
//            #region fv3
//            field_values field_Value3 = await testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.Id, null, "tomt3", 61232, worker);
//
//            #endregion
//
//            #region fv4
//            field_values field_Value4 = await testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.Id, null, "tomt4", 61233, worker);
//
//            #endregion
//
//            #region fv5
//            field_values field_Value5 = await testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.Id, null, "tomt5", 61234, worker);
//
//            #endregion
//
//            #region fv6
//            field_values field_Value6 = await testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.Id, null, "tomt6", 61235, worker);
//
//            #endregion
//
//            #region fv7
//            field_values field_Value7 = await testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.Id, null, "tomt7", 61236, worker);
//
//            #endregion
//
//            #region fv8
//            field_values field_Value8 = await testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.Id, null, "tomt8", 61237, worker);
//
//            #endregion
//
//            #region fv9
//            field_values field_Value9 = await testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.Id, null, "tomt9", 61238, worker);
//
//            #endregion
//
//            #region fv10
//            field_values field_Value10 = await testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.Id, null, "tomt10", 61239, worker);
//
//            #endregion
//
//
//            #endregion
//            #endregion
//
//            // Act
//            var removedcl2 = cl2.WorkflowState = Constants.WorkflowStates.Removed;
//            var removedcl3 = cl3.WorkflowState = Constants.WorkflowStates.Removed;
//            var removedcl4 = cl4.WorkflowState = Constants.WorkflowStates.Removed;
//            var removedcl5 = cl5.WorkflowState = Constants.WorkflowStates.Removed;
//            var removedcl6 = cl6.WorkflowState = Constants.WorkflowStates.Removed;
//            var removedcl7 = cl7.WorkflowState = Constants.WorkflowStates.Removed;
//            var removedcl8 = cl8.WorkflowState = Constants.WorkflowStates.Removed;
//            var removedcl9 = cl9.WorkflowState = Constants.WorkflowStates.Removed;
//            var removedcl10 = cl10.WorkflowState = Constants.WorkflowStates.Removed;
//            var removedcl11 = cl11.WorkflowState = Constants.WorkflowStates.Removed;
//            // Assert
//            sut.Advanced_ConsistencyCheckTemplates();
//
//
//        }

        #endregion

        #region site_workers

        [Test] //mangler mock
        public async Task Core_SiteWorkers_Advanced_SiteWorkerCreate_CreatesWorker()
        {
            // Arrange

            #region site

            string siteName = Guid.NewGuid().ToString();
            int siteMicrotingUid = await testHelpers.GetRandomInt();

            Site site = await testHelpers.CreateSite(siteName, siteMicrotingUid);
            SiteNameDto siteName_Dto = new SiteNameDto
            {
                SiteUId = (int)site.MicrotingUid,
                SiteName = site.Name,
                CreatedAt = site.CreatedAt,
                UpdatedAt = site.UpdatedAt
            };

            #endregion

            #region worker

            string email = Guid.NewGuid().ToString();
            string firstName = Guid.NewGuid().ToString();
            string lastName = Guid.NewGuid().ToString();
            int workerMicrotingUid = await testHelpers.GetRandomInt();

            Worker worker = await testHelpers.CreateWorker(email, firstName, lastName, workerMicrotingUid);
            WorkerDto worker_Dto = new WorkerDto
            {
                WorkerUId = worker.MicrotingUid,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                Email = worker.Email,
                CreatedAt = worker.CreatedAt,
                UpdatedAt = worker.UpdatedAt
            };

            #endregion

            // Act

            var match = await sut.Advanced_SiteWorkerCreate(siteName_Dto, worker_Dto);


            // Assert

            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(siteName_Dto.SiteUId, Is.EqualTo(match.SiteUId));
            Assert.That(worker_Dto.WorkerUId, Is.EqualTo(match.WorkerUId));
            Assert.That(match.MicrotingUId, Is.Not.EqualTo(null));
        }

        [Test]
        public async Task Core_SiteWorkers_Advanced_SiteWorkerRead_ReadsSiteWorker()
        {
            // Arrange

            #region Arrance

//
//            #region Checklist
//            DateTime cl_ca = DateTime.UtcNow;
//            DateTime cl_ua = DateTime.UtcNow;
//            check_lists Cl1 = await testHelpers.CreateTemplate(cl_ca,cl_ua,"A1", "D1", "caseType1", "WhereItIs", 1, 0);
//
//            #endregion
//
//            #region SubCheckList
//
//            check_lists Cl2 = await testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);
//
//            #endregion
//
//            #region Fields
//
//            #region field1
//
//
//            fields f1 = await testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field6
//
//            fields f6 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                86, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field7
//
//            fields f7 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                87, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field8
//
//            fields f8 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                88, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field9
//
//            fields f9 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                89, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field10
//
//            fields f10 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                90, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #endregion

            #region Workers

            #region worker1

            Worker worker1 = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region worker2

            Worker worker2 = await testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);

            #endregion

            #region worker3

            Worker worker3 = await testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);

            #endregion

            #region worker4

            Worker worker4 = await testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);

            #endregion

            #region worker5

            Worker worker5 = await testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);

            #endregion

            #region worker6

            Worker worker6 = await testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);

            #endregion

            #region worker7

            Worker worker7 = await testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);

            #endregion

            #region worker8

            Worker worker8 = await testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);

            #endregion

            #region worker9

            Worker worker9 = await testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);

            #endregion

            #region worker10

            Worker worker10 = await testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);

            #endregion

            #endregion

            #region sites

            #region Site1

            Site site1 = await testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2

            Site site2 = await testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3

            Site site3 = await testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4

            Site site4 = await testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5

            Site site5 = await testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6

            Site site6 = await testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7

            Site site7 = await testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8

            Site site8 = await testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9

            Site site9 = await testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10

            Site site10 = await testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion

            #region site_workers

            SiteWorker siteWorker = await testHelpers.CreateSiteWorker(55, site1, worker1);

            #endregion

            #endregion

            // Act
            var match = await sut.Advanced_SiteWorkerRead(siteWorker.MicrotingUid, site1.Id, worker1.Id);

            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(siteWorker.MicrotingUid, Is.EqualTo(match.MicrotingUId));
            Assert.That(siteWorker.Site.MicrotingUid, Is.EqualTo(match.SiteUId));
            Assert.That(siteWorker.Worker.MicrotingUid, Is.EqualTo(match.WorkerUId));
        }

        [Test]
        public async Task Core_SiteWorkers_Advanced_SiteWorkerDelete_MarksAsRemoved()
        {
            // Arrange

            #region site

            string siteName = Guid.NewGuid().ToString();
            int siteMicrotingUid = await testHelpers.GetRandomInt();

            Site site = await testHelpers.CreateSite(siteName, siteMicrotingUid);
            SiteNameDto siteName_Dto = new SiteNameDto
            {
                SiteUId = (int)site.MicrotingUid,
                SiteName = site.Name,
                CreatedAt = site.CreatedAt,
                UpdatedAt = site.UpdatedAt
            };

            #endregion

            #region worker

            string email = Guid.NewGuid().ToString();
            string firstName = Guid.NewGuid().ToString();
            string lastName = Guid.NewGuid().ToString();
            int workerMicrotingUid = await testHelpers.GetRandomInt();

            Worker worker = await testHelpers.CreateWorker(email, firstName, lastName, workerMicrotingUid);
            WorkerDto worker_Dto = new WorkerDto
            {
                WorkerUId = worker.MicrotingUid,
                CreatedAt = worker.CreatedAt,
                Email = worker.Email,
                FirstName = worker.FirstName,
                IsLocked = worker.IsLocked,
                LastName = worker.LastName,
                UpdatedAt = worker.UpdatedAt
            };

            SiteWorker siteWorker = await testHelpers.CreateSiteWorker(1, site, worker);

            #endregion

            // Act

            //var match = await sut.Advanced_SiteWorkerCreate(siteName_Dto, worker_Dto);
            var match2 = await sut.Advanced_SiteWorkerDelete(1);
            var result = DbContext.SiteWorkers.AsNoTracking().ToList();


            // Assert

            Assert.That(match2, Is.True);
            Assert.That(result[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        }

        [Test]
        public async Task Core_SiteWorkers_Advanced_WorkerDelete_MarksAsRemoved()
        {
            // Arrange
            Worker worker = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 1);


            // Act
            var match = await sut.Advanced_WorkerDelete(worker.MicrotingUid);
            var result = DbContext.Workers.AsNoTracking().ToList();
            var resultVersioned = DbContext.WorkerVersions.AsNoTracking().ToList();
            // Assert

            Assert.That(match, Is.True);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(resultVersioned.Count(), Is.EqualTo(1));
            Assert.That(result[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
            Assert.That(resultVersioned[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        }

        #endregion

        #region units

        [Test]
        public async Task Core_Unit_Advanced_UnitRead_ReadsUnit()
        {
            // Arrange
//            #region Checklist
//            DateTime cl_ca = DateTime.UtcNow;
//            DateTime cl_ua = DateTime.UtcNow;
//            check_lists Cl1 = await testHelpers.CreateTemplate(cl_ca, cl_ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);
//
//            #endregion
//
//            #region SubCheckList
//
//            check_lists Cl2 = await testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);
//
//            #endregion
//
//            #region Fields
//
//            #region field1
//
//
//            fields f1 = await testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field6
//
//            fields f6 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                86, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field7
//
//            fields f7 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                87, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field8
//
//            fields f8 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                88, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field9
//
//            fields f9 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                89, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field10
//
//            fields f10 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                90, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #endregion

            #region Workers

            #region worker1

            Worker worker1 = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region worker2

            Worker worker2 = await testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);

            #endregion

            #region worker3

            Worker worker3 = await testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);

            #endregion

            #region worker4

            Worker worker4 = await testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);

            #endregion

            #region worker5

            Worker worker5 = await testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);

            #endregion

            #region worker6

            Worker worker6 = await testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);

            #endregion

            #region worker7

            Worker worker7 = await testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);

            #endregion

            #region worker8

            Worker worker8 = await testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);

            #endregion

            #region worker9

            Worker worker9 = await testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);

            #endregion

            #region worker10

            Worker worker10 = await testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);

            #endregion

            #endregion

            #region sites

            #region Site1

            Site site1 = await testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2

            Site site2 = await testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3

            Site site3 = await testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4

            Site site4 = await testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5

            Site site5 = await testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6

            Site site6 = await testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7

            Site site7 = await testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8

            Site site8 = await testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9

            Site site9 = await testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10

            Site site10 = await testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion

            // Act

            UnitDto match = await sut.Advanced_UnitRead((int)unit.MicrotingUid);

            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.UnitUId, Is.EqualTo(unit.MicrotingUid));
            Assert.That(match.CustomerNo, Is.EqualTo(unit.CustomerNo));
        }

        [Test]
        public async Task Core_Unit_Advanced_UnitReadAll_ReturnsListOfUnits()
        {
            #region Arrance

//            #region Checklist
//            DateTime cl_ca = DateTime.UtcNow;
//            DateTime cl_ua = DateTime.UtcNow;
//            check_lists Cl1 = await testHelpers.CreateTemplate(cl_ca, cl_ua, "A1", "D1", "caseType1", "WhereItIs", 1, 0);
//
//            #endregion
//
//            #region SubCheckList
//
//            check_lists Cl2 = await testHelpers.CreateSubTemplate("A2", "D2", "caseType2", 2, 0, Cl1);
//
//            #endregion
//
//            #region Fields
//
//            #region field1
//
//
//            fields f1 = await testHelpers.CreateField(1, "barcode", Cl2, "e2f4fb", "custom", null, "", "Comment field description",
//                5, 1, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55, "55", "0", 0, 0, null, 1, 0,
//                0, 0, "", 49);
//
//            #endregion
//
//            #region field2
//
//
//            fields f2 = await testHelpers.CreateField(1, "barcode", Cl2, "f5eafa", "custom", null, "", "showPDf Description",
//                45, 1, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 0, 1, 0, 0,
//                "ShowPdf", 0, 5, "5", "0", 0, 0, null, 0, 0, 0, 0, "", 9);
//
//
//            #endregion
//
//            #region field3
//
//            fields f3 = await testHelpers.CreateField(0, "barcode", Cl2, "f0f8db", "custom", 3, "", "Number Field Description",
//                83, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Numberfield", 1, 8, "4865", "0", 0, 1, null, 1, 0, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field4
//
//
//            fields f4 = await testHelpers.CreateField(1, "barcode", Cl2, "fff6df", "custom", null, "", "date Description",
//                84, 0, DbContext.field_types.Where(x => x.FieldType == "picture").First(), 0, 0, 1, 0,
//                "Date", 1, 666, "41153", "0", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field5
//
//            fields f5 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                85, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field6
//
//            fields f6 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                86, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field7
//
//            fields f7 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                87, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field8
//
//            fields f8 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                88, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field9
//
//            fields f9 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                89, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #region field10
//
//            fields f10 = await testHelpers.CreateField(0, "barcode", Cl2, "ffe4e4", "custom", null, "", "picture Description",
//                90, 0, DbContext.field_types.Where(x => x.FieldType == "comment").First(), 1, 0, 1, 0,
//                "Picture", 1, 69, "69", "1", 0, 1, null, 0, 1, 0, 0, "", 1);
//
//
//            #endregion
//
//            #endregion

            #region Workers

            #region worker1

            Worker worker1 = await testHelpers.CreateWorker("aa@tak.dk", "Arne", "Jensen", 21);

            #endregion

            #region worker2

            Worker worker2 = await testHelpers.CreateWorker("ab@tak.dk", "Lasse", "Johansen", 44);

            #endregion

            #region worker3

            Worker worker3 = await testHelpers.CreateWorker("ac@tak.dk", "Svend", "Jensen", 22);

            #endregion

            #region worker4

            Worker worker4 = await testHelpers.CreateWorker("ad@tak.dk", "Bjarne", "Nielsen", 23);

            #endregion

            #region worker5

            Worker worker5 = await testHelpers.CreateWorker("ae@tak.dk", "Ib", "Hansen", 24);

            #endregion

            #region worker6

            Worker worker6 = await testHelpers.CreateWorker("af@tak.dk", "Hozan", "Aziz", 25);

            #endregion

            #region worker7

            Worker worker7 = await testHelpers.CreateWorker("ag@tak.dk", "Nicolai", "Peders", 26);

            #endregion

            #region worker8

            Worker worker8 = await testHelpers.CreateWorker("ah@tak.dk", "Amin", "Safari", 27);

            #endregion

            #region worker9

            Worker worker9 = await testHelpers.CreateWorker("ai@tak.dk", "Leo", "Rebaz", 28);

            #endregion

            #region worker10

            Worker worker10 = await testHelpers.CreateWorker("aj@tak.dk", "Stig", "Berthelsen", 29);

            #endregion

            #endregion

            #region sites

            #region Site1

            Site site1 = await testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2

            Site site2 = await testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3

            Site site3 = await testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4

            Site site4 = await testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5

            Site site5 = await testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6

            Site site6 = await testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7

            Site site7 = await testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8

            Site site8 = await testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9

            Site site9 = await testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10

            Site site10 = await testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units

            #region Unit1

            Unit unit1 = await testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion

            #region Unit2

            Unit unit2 = await testHelpers.CreateUnit(2, 55, site2, 349);

            #endregion

            #region Unit3

            Unit unit3 = await testHelpers.CreateUnit(3, 51, site3, 350);

            #endregion

            #region Unit4

            Unit unit4 = await testHelpers.CreateUnit(4, 52, site4, 351);

            #endregion

            #region Unit5

            Unit unit5 = await testHelpers.CreateUnit(5, 6, site5, 352);

            #endregion

            #region Unit6

            Unit unit6 = await testHelpers.CreateUnit(6, 85, site6, 353);

            #endregion

            #region Unit7

            Unit unit7 = await testHelpers.CreateUnit(7, 62, site7, 354);

            #endregion

            #region Unit8

            Unit unit8 = await testHelpers.CreateUnit(8, 96, site8, 355);

            #endregion

            #region Unit9

            Unit unit9 = await testHelpers.CreateUnit(9, 69, site9, 356);

            #endregion

            #region Unit10

            Unit unit10 = await testHelpers.CreateUnit(10, 100, site10, 357);

            #endregion

            #endregion

            #endregion

            // Act

            var getAllUnits = await sut.Advanced_UnitReadAll();

            // Assert

            Assert.That(getAllUnits.Count(), Is.EqualTo(10));

            Assert.That(getAllUnits[0].UnitUId, Is.EqualTo(unit1.MicrotingUid));
            Assert.That(getAllUnits[1].UnitUId, Is.EqualTo(unit2.MicrotingUid));
            Assert.That(getAllUnits[2].UnitUId, Is.EqualTo(unit3.MicrotingUid));
            Assert.That(getAllUnits[3].UnitUId, Is.EqualTo(unit4.MicrotingUid));
            Assert.That(getAllUnits[4].UnitUId, Is.EqualTo(unit5.MicrotingUid));
            Assert.That(getAllUnits[5].UnitUId, Is.EqualTo(unit6.MicrotingUid));
            Assert.That(getAllUnits[6].UnitUId, Is.EqualTo(unit7.MicrotingUid));
            Assert.That(getAllUnits[7].UnitUId, Is.EqualTo(unit8.MicrotingUid));
            Assert.That(getAllUnits[8].UnitUId, Is.EqualTo(unit9.MicrotingUid));
            Assert.That(getAllUnits[9].UnitUId, Is.EqualTo(unit10.MicrotingUid));
        }

        #endregion

        #region fields

        [Test]
        public async Task Core_Advanced_FieldRead_ReadsFieldId()
        {
            // Arrance

            #region Template1

            DateTime cl_ca = DateTime.UtcNow;
            DateTime cl_ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl_ca, cl_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

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

            // Act

            Microting.eForm.Infrastructure.Models.Field match = await sut.Advanced_FieldRead(f1.Id, language);

            // Assert

            Assert.That(match.Id, Is.EqualTo(f1.Id));
        }

        [Test]
        public async Task Core_Advanced_FieldValueReadList_ReturnsList()
        {
            // Arrance

            #region Arrance

            #region Template1

            DateTime cl_ca = DateTime.UtcNow;
            DateTime cl_ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl_ca, cl_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

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
                worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
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
                await sut.Advanced_FieldValueReadList(f1.Id, 5, language);

            // Assert

            Assert.That(match[0].Value, Is.EqualTo(field_Value1.Value));
        }

        #endregion

        #region Entitygouplist

        [Test]
        public async Task Core_Advanced_DeleteUploadedData_DeletesData()
        {
            // Arrange

            #region Template1

            DateTime cl_ca = DateTime.UtcNow;
            DateTime cl_ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl_ca, cl_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

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

            #region sites

            #region Site1

            Site site1 = await testHelpers.CreateSite("SiteName1", 88);

            #endregion

            #region Site2

            Site site2 = await testHelpers.CreateSite("SiteName2", 89);

            #endregion

            #region Site3

            Site site3 = await testHelpers.CreateSite("SiteName3", 90);

            #endregion

            #region Site4

            Site site4 = await testHelpers.CreateSite("SiteName4", 91);

            #endregion

            #region Site5

            Site site5 = await testHelpers.CreateSite("SiteName5", 92);

            #endregion

            #region Site6

            Site site6 = await testHelpers.CreateSite("SiteName6", 93);

            #endregion

            #region Site7

            Site site7 = await testHelpers.CreateSite("SiteName7", 94);

            #endregion

            #region Site8

            Site site8 = await testHelpers.CreateSite("SiteName8", 95);

            #endregion

            #region Site9

            Site site9 = await testHelpers.CreateSite("SiteName9", 96);

            #endregion

            #region Site10

            Site site10 = await testHelpers.CreateSite("SiteName10", 97);

            #endregion

            #endregion

            #region units

            Unit unit = await testHelpers.CreateUnit(48, 49, site1, 348);

            #endregion

            #region site_workers

            SiteWorker siteWorker = await testHelpers.CreateSiteWorker(55, site1, worker);

            #endregion


            string folderPath;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                folderPath = @"\output\dataFolder\picture\";
            }
            else
            {
                folderPath = @"/output/dataFolder/picture/";
            }

            #region UploadedData

            #region ud1

            UploadedData ud1 = await testHelpers.CreateUploadedData("", "File.jpg", "jpg", path + folderPath,
                "File.jpg", 1, worker,
                Constants.UploaderTypes.System, 55, true);

            #endregion

            #region ud2

            UploadedData ud2 = await testHelpers.CreateUploadedData("checksum2", "File1", "no", "hjgjghjhg", "File2", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud3

            UploadedData ud3 = await testHelpers.CreateUploadedData("checksum3", "File1", "no", "hjgjghjhg", "File3", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud4

            UploadedData ud4 = await testHelpers.CreateUploadedData("checksum4", "File1", "no", "hjgjghjhg", "File4", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud5

            UploadedData ud5 = await testHelpers.CreateUploadedData("checksum5", "File1", "no", "hjgjghjhg", "File5", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud6

            UploadedData ud6 = await testHelpers.CreateUploadedData("checksum6", "File1", "no", "hjgjghjhg", "File6", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud7

            UploadedData ud7 = await testHelpers.CreateUploadedData("checksum7", "File1", "no", "hjgjghjhg", "File7", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud8

            UploadedData ud8 = await testHelpers.CreateUploadedData("checksum8", "File1", "no", "hjgjghjhg", "File8", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud9

            UploadedData ud9 = await testHelpers.CreateUploadedData("checksum9", "File1", "no", "hjgjghjhg", "File9", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud10

            UploadedData ud10 = await testHelpers.CreateUploadedData("checksum10", "File1", "no", "hjgjghjhg", "File10",
                1, worker,
                "local", 55, false);

            #endregion

            #endregion

            // Act
            bool match = await sut.Advanced_DeleteUploadedData(f2.Id, ud1.Id);
            // Assert
            Assert.That(match, Is.True);
        }

        [Test]
        public async Task Core_Advanced_UpdateCaseFieldValue_UpdatesFieldValue()
        {
            // Arrange

            #region Arrance

            #region Template1

            DateTime cl1_Ca = DateTime.UtcNow;
            DateTime cl1_Ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_Ca, cl1_Ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region subtemplates

            #region SubTemplate1

            CheckList cl2 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl3 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl4 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl5 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl6 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl7 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl8 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl9 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl10 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

            #region SubTemplate1

            CheckList cl11 = await testHelpers.CreateSubTemplate("A.1", "D.1", "CheckList", 1, 1, cl1);

            #endregion

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
                83, 0, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 0, 1, 0,
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

            Case aCase1 = await testHelpers.CreateCase("case1UId", cl2, c1_ca, "custom1",
                c1_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 1, "caseType1", unit, c1_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Case2

            DateTime c2_ca = DateTime.UtcNow.AddDays(-7);
            DateTime c2_da = DateTime.UtcNow.AddDays(-6).AddHours(-12);
            DateTime c2_ua = DateTime.UtcNow.AddDays(-6);
            Case aCase2 = await testHelpers.CreateCase("case2UId", cl1, c2_ca, "custom2",
                c2_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 10, "caseType2", unit, c2_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Case3

            DateTime c3_ca = DateTime.UtcNow.AddDays(-10);
            DateTime c3_da = DateTime.UtcNow.AddDays(-9).AddHours(-12);
            DateTime c3_ua = DateTime.UtcNow.AddDays(-9);

            Case aCase3 = await testHelpers.CreateCase("case3UId", cl1, c3_ca, "custom3",
                c3_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 15, "caseType3", unit, c3_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #region Case4

            DateTime c4_ca = DateTime.UtcNow.AddDays(-8);
            DateTime c4_da = DateTime.UtcNow.AddDays(-7).AddHours(-12);
            DateTime c4_ua = DateTime.UtcNow.AddDays(-7);

            Case aCase4 = await testHelpers.CreateCase("case4UId", cl1, c4_ca, "custom4",
                c4_da, worker, rnd.Next(shortMinValue, shortmaxValue), rnd.Next(shortMinValue, shortmaxValue),
                site, 100, "caseType4", unit, c4_ua, 1, worker, Constants.WorkflowStates.Created);

            #endregion

            #endregion

            #endregion

            #region UploadedData

            #region ud1

            UploadedData ud1 = await testHelpers.CreateUploadedData("checksum1", "File1", "no", "hjgjghjhg", "File1", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud2

            UploadedData ud2 = await testHelpers.CreateUploadedData("checksum2", "File1", "no", "hjgjghjhg", "File2", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud3

            UploadedData ud3 = await testHelpers.CreateUploadedData("checksum3", "File1", "no", "hjgjghjhg", "File3", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud4

            UploadedData ud4 = await testHelpers.CreateUploadedData("checksum4", "File1", "no", "hjgjghjhg", "File4", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud5

            UploadedData ud5 = await testHelpers.CreateUploadedData("checksum5", "File1", "no", "hjgjghjhg", "File5", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud6

            UploadedData ud6 = await testHelpers.CreateUploadedData("checksum6", "File1", "no", "hjgjghjhg", "File6", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud7

            UploadedData ud7 = await testHelpers.CreateUploadedData("checksum7", "File1", "no", "hjgjghjhg", "File7", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud8

            UploadedData ud8 = await testHelpers.CreateUploadedData("checksum8", "File1", "no", "hjgjghjhg", "File8", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud9

            UploadedData ud9 = await testHelpers.CreateUploadedData("checksum9", "File1", "no", "hjgjghjhg", "File9", 1,
                worker,
                "local", 55, false);

            #endregion

            #region ud10

            UploadedData ud10 = await testHelpers.CreateUploadedData("checksum10", "File1", "no", "hjgjghjhg", "File10",
                1, worker,
                "local", 55, false);

            #endregion

            #endregion

            #region Check List Values

            #region clv1

            CheckListValue clv1 =
                await testHelpers.CreateCheckListValue(aCase1, cl2, Constants.CheckListValues.Checked, null, 860);

            #endregion

            #region clv2

            CheckListValue clv2 =
                await testHelpers.CreateCheckListValue(aCase1, cl3, Constants.CheckListValues.Checked, null, 861);

            #endregion

            #region clv3

            CheckListValue clv3 =
                await testHelpers.CreateCheckListValue(aCase1, cl4, Constants.CheckListValues.Checked, null, 862);

            #endregion

            #region clv4

            CheckListValue clv4 =
                await testHelpers.CreateCheckListValue(aCase1, cl5, Constants.CheckListValues.NotChecked, null, 863);

            #endregion

            #region clv5

            CheckListValue clv5 =
                await testHelpers.CreateCheckListValue(aCase1, cl6, Constants.CheckListValues.NotChecked, null, 864);

            #endregion

            #region clv6

            CheckListValue clv6 =
                await testHelpers.CreateCheckListValue(aCase1, cl7, Constants.CheckListValues.NotChecked, null, 865);

            #endregion

            #region clv7

            CheckListValue clv7 =
                await testHelpers.CreateCheckListValue(aCase1, cl8, Constants.CheckListValues.NotApproved, null, 866);

            #endregion

            #region clv8

            CheckListValue clv8 =
                await testHelpers.CreateCheckListValue(aCase1, cl9, Constants.CheckListValues.NotApproved, null, 867);

            #endregion

            #region clv9

            CheckListValue clv9 =
                await testHelpers.CreateCheckListValue(aCase1, cl10, Constants.CheckListValues.NotApproved, null, 868);

            #endregion

            #region clv10

            CheckListValue clv10 =
                await testHelpers.CreateCheckListValue(aCase1, cl11, Constants.CheckListValues.NotApproved, null, 869);

            #endregion

            #endregion

            #region Field Values

            #region fv1

            FieldValue field_Value1 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f1, ud1.Id, null, "tomt1", 61230, worker);

            #endregion

            #region fv2

            FieldValue field_Value2 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f2, ud2.Id, null, "tomt2", 61231, worker);

            #endregion

            #region fv3

            FieldValue field_Value3 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f3, ud3.Id, null, "tomt3", 61232, worker);

            #endregion

            #region fv4

            FieldValue field_Value4 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f4, ud4.Id, null, "tomt4", 61233, worker);

            #endregion

            #region fv5

            FieldValue field_Value5 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f5, ud5.Id, null, "tomt5", 61234, worker);

            #endregion

            #region fv6

            FieldValue field_Value6 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f6, ud6.Id, null, "tomt6", 61235, worker);

            #endregion

            #region fv7

            FieldValue field_Value7 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f7, ud7.Id, null, "tomt7", 61236, worker);

            #endregion

            #region fv8

            FieldValue field_Value8 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f8, ud8.Id, null, "tomt8", 61237, worker);

            #endregion

            #region fv9

            FieldValue field_Value9 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f9, ud9.Id, null, "tomt9", 61238, worker);

            #endregion

            #region fv10

            FieldValue field_Value10 =
                await testHelpers.CreateFieldValue(aCase1, cl2, f10, ud10.Id, null, "tomt10", 61239, worker);

            #endregion

            #endregion

            #endregion

            // Act
            Case theCase = DbContext.Cases.First();
            Assert.That(theCase, Is.Not.EqualTo(null));
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

            Assert.That(theCase.FieldValue1, Is.EqualTo(null));
            Assert.That(theCase.FieldValue2, Is.EqualTo(null));
            Assert.That(theCase.FieldValue3, Is.EqualTo(null));
            Assert.That(theCase.FieldValue4, Is.EqualTo(null));
            Assert.That(theCase.FieldValue5, Is.EqualTo(null));
            Assert.That(theCase.FieldValue6, Is.EqualTo(null));
            Assert.That(theCase.FieldValue7, Is.EqualTo(null));
            Assert.That(theCase.FieldValue8, Is.EqualTo(null));
            Assert.That(theCase.FieldValue9, Is.EqualTo(null));
            Assert.That(theCase.FieldValue10, Is.EqualTo(null));

            var testThis = await sut.Advanced_UpdateCaseFieldValue(aCase1.Id, language);

            // Assert
            Case theCaseAfter = DbContext.Cases.AsNoTracking().First();

            Assert.That(theCaseAfter, Is.Not.EqualTo(null));

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


            Assert.That(testThis, Is.True);

            Assert.That(theCaseAfter.FieldValue1, Is.EqualTo("tomt1"));
            Assert.That(theCaseAfter.FieldValue2, Is.EqualTo("tomt2"));
            Assert.That(theCaseAfter.FieldValue3, Is.EqualTo("tomt3"));
            Assert.That(theCaseAfter.FieldValue4, Is.EqualTo("tomt4"));
            Assert.That(theCaseAfter.FieldValue5, Is.EqualTo("tomt5"));
            Assert.That(theCaseAfter.FieldValue6, Is.EqualTo("tomt6"));
            Assert.That(theCaseAfter.FieldValue7, Is.EqualTo("tomt7"));
            Assert.That(theCaseAfter.FieldValue8, Is.EqualTo("tomt8"));
            Assert.That(theCaseAfter.FieldValue9, Is.EqualTo("tomt9"));
            Assert.That(theCaseAfter.FieldValue10, Is.EqualTo("tomt10"));
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