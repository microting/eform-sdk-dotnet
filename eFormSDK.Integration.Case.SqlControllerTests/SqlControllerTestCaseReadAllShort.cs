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
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;
using Field = Microting.eForm.Infrastructure.Data.Entities.Field;
using Tag = Microting.eForm.Infrastructure.Data.Entities.Tag;

namespace eFormSDK.Integration.Case.SqlControllerTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SqlControllerTestCaseReadAllShort : DbTestFixture
    {
        private SqlController sut;

        private TestHelpers testHelpers;

//        private string path;
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
        private Language language;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

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

        #region template

        [Test]
        public async Task SQL_Template_TemplateItemReadAll_DoesSortAccordingly()
        {
            // Arrance

            #region Template1

            CheckList cl1 = await testHelpers.CreateTemplate(DateTime.UtcNow, DateTime.UtcNow, "A", "D",
                "CheckList", "Template1FolderName", 1, 0);

            #endregion

            #region Template2

            CheckList cl2 = await testHelpers.CreateTemplate(DateTime.UtcNow, DateTime.UtcNow, "B", "C",
                "CheckList", "Template1FolderName", 1, 0);
            await cl2.Delete(DbContext);

            #endregion

            #region Template3

            CheckList cl3 = await testHelpers.CreateTemplate(DateTime.UtcNow, DateTime.UtcNow, "D", "B",
                "CheckList", "Template1FolderName", 1, 0);

            #endregion

            #region Template4

            CheckList cl4 = await testHelpers.CreateTemplate(DateTime.UtcNow, DateTime.UtcNow, "C", "A",
                "CheckList", "Template1FolderName", 1, 0);

            #endregion


            // Act
            List<int> emptyList = new List<int>();

            // Default sorting including removed
            List<Template_Dto> templateListId = await sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created,
                "", false, "", emptyList, timeZoneInfo, language);
            List<Template_Dto> templateListLabel = await sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created,
                "", false, Constants.eFormSortParameters.Label, emptyList, timeZoneInfo, language);
            List<Template_Dto> templateListDescription = await sut.TemplateItemReadAll(true,
                Constants.WorkflowStates.Created, "", false, Constants.eFormSortParameters.Description, emptyList,
                timeZoneInfo, language);
            List<Template_Dto> templateListCreatedAt = await sut.TemplateItemReadAll(true,
                Constants.WorkflowStates.Created, "", false, Constants.eFormSortParameters.CreatedAt, emptyList,
                timeZoneInfo, language);

            // Descending including removed
            List<Template_Dto> templateListDescengingId = await sut.TemplateItemReadAll(true,
                Constants.WorkflowStates.Created, "", true, "", emptyList, timeZoneInfo, language);
            List<Template_Dto> templateListDescengingLabel = await sut.TemplateItemReadAll(true,
                Constants.WorkflowStates.Created, "", true, Constants.eFormSortParameters.Label, emptyList,
                timeZoneInfo, language);
            List<Template_Dto> templateListDescengingDescription = await sut.TemplateItemReadAll(true,
                Constants.WorkflowStates.Created, "", true, Constants.eFormSortParameters.Description, emptyList,
                timeZoneInfo, language);
            List<Template_Dto> templateListDescengingCreatedAt = await sut.TemplateItemReadAll(true,
                Constants.WorkflowStates.Created, "", true, Constants.eFormSortParameters.CreatedAt, emptyList,
                timeZoneInfo, language);

            // Default sorting excluding removed
            List<Template_Dto> templateListIdNr = await sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created,
                "", false, "", emptyList, timeZoneInfo, language);
            List<Template_Dto> templateListLabelNr = await sut.TemplateItemReadAll(false,
                Constants.WorkflowStates.Created, "", false, Constants.eFormSortParameters.Label, emptyList,
                timeZoneInfo, language);
            List<Template_Dto> templateListDescriptionNr = await sut.TemplateItemReadAll(false,
                Constants.WorkflowStates.Created, "", false, Constants.eFormSortParameters.Description, emptyList,
                timeZoneInfo, language);
            List<Template_Dto> templateListCreatedAtNr = await sut.TemplateItemReadAll(false,
                Constants.WorkflowStates.Created, "", false, Constants.eFormSortParameters.CreatedAt, emptyList,
                timeZoneInfo, language);

            // Descending excluding removed
            List<Template_Dto> templateListDescengingIdNr = await sut.TemplateItemReadAll(false,
                Constants.WorkflowStates.Created, "", true, "", emptyList, timeZoneInfo, language);
            List<Template_Dto> templateListDescengingLabelNr = await sut.TemplateItemReadAll(false,
                Constants.WorkflowStates.Created, "", true, Constants.eFormSortParameters.Label, emptyList,
                timeZoneInfo, language);
            List<Template_Dto> templateListDescengingDescriptionNr = await sut.TemplateItemReadAll(false,
                Constants.WorkflowStates.Created, "", true, Constants.eFormSortParameters.Description, emptyList,
                timeZoneInfo, language);
            List<Template_Dto> templateListDescengingCreatedAtNr = await sut.TemplateItemReadAll(false,
                Constants.WorkflowStates.Created, "", true, Constants.eFormSortParameters.CreatedAt, emptyList,
                timeZoneInfo, language);


            // Assert

            #region include removed

            // Default sorting including removed
            // Id
            Assert.NotNull(templateListId);
            Assert.AreEqual(4, templateListId.Count());
            Assert.AreEqual("A", templateListId[0].Label);
            Assert.AreEqual("B", templateListId[1].Label);
            Assert.AreEqual("D", templateListId[2].Label);
            Assert.AreEqual("C", templateListId[3].Label);
            Assert.AreEqual(0, templateListId[0].Tags.Count());
            Assert.AreEqual(0, templateListId[1].Tags.Count());
            Assert.AreEqual(0, templateListId[2].Tags.Count());
            Assert.AreEqual(0, templateListId[3].Tags.Count());

            // Default sorting including removed
            // Label
            Assert.NotNull(templateListLabel);
            Assert.AreEqual(4, templateListLabel.Count());
            Assert.AreEqual("A", templateListLabel[0].Label);
            Assert.AreEqual("B", templateListLabel[1].Label);
            Assert.AreEqual("C", templateListLabel[2].Label);
            Assert.AreEqual("D", templateListLabel[3].Label);
            Assert.AreEqual(0, templateListLabel[0].Tags.Count());
            Assert.AreEqual(0, templateListLabel[1].Tags.Count());
            Assert.AreEqual(0, templateListLabel[2].Tags.Count());
            Assert.AreEqual(0, templateListLabel[3].Tags.Count());

            // Default sorting including removed
            // Description
            Assert.NotNull(templateListDescription);
            Assert.AreEqual(4, templateListDescription.Count());
            Assert.AreEqual("C", templateListDescription[0].Label);
            Assert.AreEqual("D", templateListDescription[1].Label);
            Assert.AreEqual("B", templateListDescription[2].Label);
            Assert.AreEqual("A", templateListDescription[3].Label);
            Assert.AreEqual(0, templateListDescription[0].Tags.Count());
            Assert.AreEqual(0, templateListDescription[1].Tags.Count());
            Assert.AreEqual(0, templateListDescription[2].Tags.Count());
            Assert.AreEqual(0, templateListDescription[3].Tags.Count());

            // Default sorting including removed
            // Created At
            Assert.NotNull(templateListCreatedAt);
            Assert.AreEqual(4, templateListCreatedAt.Count());
            Assert.AreEqual("A", templateListCreatedAt[0].Label);
            Assert.AreEqual("B", templateListCreatedAt[1].Label);
            Assert.AreEqual("D", templateListCreatedAt[2].Label);
            Assert.AreEqual("C", templateListCreatedAt[3].Label);
            Assert.AreEqual(0, templateListCreatedAt[0].Tags.Count());
            Assert.AreEqual(0, templateListCreatedAt[1].Tags.Count());
            Assert.AreEqual(0, templateListCreatedAt[2].Tags.Count());
            Assert.AreEqual(0, templateListCreatedAt[3].Tags.Count());

            // Descending sorting including removed
            // Id
            Assert.NotNull(templateListDescengingId);
            Assert.AreEqual(4, templateListDescengingId.Count());
            Assert.AreEqual("C", templateListDescengingId[0].Label);
            Assert.AreEqual("D", templateListDescengingId[1].Label);
            Assert.AreEqual("B", templateListDescengingId[2].Label);
            Assert.AreEqual("A", templateListDescengingId[3].Label);
            Assert.AreEqual(0, templateListDescengingId[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingId[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingId[2].Tags.Count());
            Assert.AreEqual(0, templateListDescengingId[3].Tags.Count());

            // Descending sorting including removed
            // Label
            Assert.NotNull(templateListDescengingLabel);
            Assert.AreEqual(4, templateListDescengingLabel.Count());
            Assert.AreEqual("D", templateListDescengingLabel[0].Label);
            Assert.AreEqual("C", templateListDescengingLabel[1].Label);
            Assert.AreEqual("B", templateListDescengingLabel[2].Label);
            Assert.AreEqual("A", templateListDescengingLabel[3].Label);
            Assert.AreEqual(0, templateListDescengingLabel[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingLabel[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingLabel[2].Tags.Count());
            Assert.AreEqual(0, templateListDescengingLabel[3].Tags.Count());

            // Descending sorting including removed
            // Description
            Assert.NotNull(templateListDescengingDescription);
            Assert.AreEqual(4, templateListDescengingDescription.Count());
            Assert.AreEqual("A", templateListDescengingDescription[0].Label);
            Assert.AreEqual("B", templateListDescengingDescription[1].Label);
            Assert.AreEqual("D", templateListDescengingDescription[2].Label);
            Assert.AreEqual("C", templateListDescengingDescription[3].Label);
            Assert.AreEqual(0, templateListDescengingDescription[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingDescription[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingDescription[2].Tags.Count());
            Assert.AreEqual(0, templateListDescengingDescription[3].Tags.Count());

            // Descending sorting including removed
            // Created At
            Assert.NotNull(templateListDescengingCreatedAt);
            Assert.AreEqual(4, templateListDescengingCreatedAt.Count());
            Assert.AreEqual("C", templateListDescengingCreatedAt[0].Label);
            Assert.AreEqual("D", templateListDescengingCreatedAt[1].Label);
            Assert.AreEqual("B", templateListDescengingCreatedAt[2].Label);
            Assert.AreEqual("A", templateListDescengingCreatedAt[3].Label);
            Assert.AreEqual(0, templateListDescengingCreatedAt[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingCreatedAt[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingCreatedAt[2].Tags.Count());
            Assert.AreEqual(0, templateListDescengingCreatedAt[3].Tags.Count());

            #endregion

            #region Exclude removed

            // Default sorting including removed
            // Id
            Assert.NotNull(templateListIdNr);
            Assert.AreEqual(3, templateListIdNr.Count());
            Assert.AreEqual("A", templateListIdNr[0].Label);
            Assert.AreEqual("D", templateListIdNr[1].Label);
            Assert.AreEqual("C", templateListIdNr[2].Label);
            Assert.AreEqual(0, templateListIdNr[0].Tags.Count());
            Assert.AreEqual(0, templateListIdNr[1].Tags.Count());
            Assert.AreEqual(0, templateListIdNr[2].Tags.Count());

            // Default sorting including removed
            // Label
            Assert.NotNull(templateListLabelNr);
            Assert.AreEqual(3, templateListLabelNr.Count());
            Assert.AreEqual("A", templateListLabelNr[0].Label);
            Assert.AreEqual("C", templateListLabelNr[1].Label);
            Assert.AreEqual("D", templateListLabelNr[2].Label);
            Assert.AreEqual(0, templateListLabelNr[0].Tags.Count());
            Assert.AreEqual(0, templateListLabelNr[1].Tags.Count());
            Assert.AreEqual(0, templateListLabelNr[2].Tags.Count());

            // Default sorting including removed
            // Description
            Assert.NotNull(templateListDescriptionNr);
            Assert.AreEqual(3, templateListDescriptionNr.Count());
            Assert.AreEqual("C", templateListDescriptionNr[0].Label);
            Assert.AreEqual("D", templateListDescriptionNr[1].Label);
            Assert.AreEqual("A", templateListDescriptionNr[2].Label);
            Assert.AreEqual(0, templateListDescriptionNr[0].Tags.Count());
            Assert.AreEqual(0, templateListDescriptionNr[1].Tags.Count());
            Assert.AreEqual(0, templateListDescriptionNr[2].Tags.Count());

            // Default sorting including removed
            // Created At
            Assert.NotNull(templateListCreatedAtNr);
            Assert.AreEqual(3, templateListCreatedAtNr.Count());
            Assert.AreEqual("A", templateListCreatedAtNr[0].Label);
            Assert.AreEqual("D", templateListCreatedAtNr[1].Label);
            Assert.AreEqual("C", templateListCreatedAtNr[2].Label);
            Assert.AreEqual(0, templateListCreatedAtNr[0].Tags.Count());
            Assert.AreEqual(0, templateListCreatedAtNr[1].Tags.Count());
            Assert.AreEqual(0, templateListCreatedAtNr[2].Tags.Count());

            // Descending sorting including removed
            // Id
            Assert.NotNull(templateListDescengingIdNr);
            Assert.AreEqual(3, templateListDescengingIdNr.Count());
            Assert.AreEqual("C", templateListDescengingIdNr[0].Label);
            Assert.AreEqual("D", templateListDescengingIdNr[1].Label);
            Assert.AreEqual("A", templateListDescengingIdNr[2].Label);
            Assert.AreEqual(0, templateListDescengingIdNr[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingIdNr[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingIdNr[2].Tags.Count());

            // Descending sorting including removed
            // Label
            Assert.NotNull(templateListDescengingLabelNr);
            Assert.AreEqual(3, templateListDescengingLabelNr.Count());
            Assert.AreEqual("D", templateListDescengingLabelNr[0].Label);
            Assert.AreEqual("C", templateListDescengingLabelNr[1].Label);
            Assert.AreEqual("A", templateListDescengingLabelNr[2].Label);
            Assert.AreEqual(0, templateListDescengingLabelNr[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingLabelNr[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingLabelNr[2].Tags.Count());

            // Descending sorting including removed
            // Description
            Assert.NotNull(templateListDescengingDescriptionNr);
            Assert.AreEqual(3, templateListDescengingDescriptionNr.Count());
            Assert.AreEqual("A", templateListDescengingDescriptionNr[0].Label);
            Assert.AreEqual("D", templateListDescengingDescriptionNr[1].Label);
            Assert.AreEqual("C", templateListDescengingDescriptionNr[2].Label);
            Assert.AreEqual(0, templateListDescengingDescriptionNr[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingDescriptionNr[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingDescriptionNr[2].Tags.Count());

            // Descending sorting including removed
            // Created At
            Assert.NotNull(templateListDescengingCreatedAtNr);
            Assert.AreEqual(3, templateListDescengingCreatedAtNr.Count());
            Assert.AreEqual("C", templateListDescengingCreatedAtNr[0].Label);
            Assert.AreEqual("D", templateListDescengingCreatedAtNr[1].Label);
            Assert.AreEqual("A", templateListDescengingCreatedAtNr[2].Label);
            Assert.AreEqual(0, templateListDescengingCreatedAtNr[0].Tags.Count());
            Assert.AreEqual(0, templateListDescengingCreatedAtNr[1].Tags.Count());
            Assert.AreEqual(0, templateListDescengingCreatedAtNr[2].Tags.Count());

            #endregion
        }

        [Test]
        public async Task SQL_Template_TemplateDelete_DoesMarkTemplateAsRemoved()
        {
            // Arrance

            #region Template1

            CheckList cl1 = await testHelpers.CreateTemplate(DateTime.UtcNow, DateTime.UtcNow, "A", "D",
                "CheckList", "Template1FolderName", 1, 0);

            #endregion

            // Act

            await sut.TemplateDelete(cl1.Id);

            Template_Dto clResult = await sut.TemplateItemRead(cl1.Id, language);

            // Assert

            var checkLists = DbContext.CheckLists.AsNoTracking().ToList();

            Assert.NotNull(clResult);
            Assert.AreEqual(1, checkLists.Count());
            Assert.AreEqual(Constants.WorkflowStates.Removed, checkLists[0].WorkflowState);
        }

//        [Test]
//        public async Task SQL_Template_UpdateCaseFieldValue_DoesUpdateFieldValues()
//        {
//            // Arrance
//
//            // Act
//
//            // Assert
//            Assert.True(true);
//        }

        [Test] //might need aditional testing
        public async Task SQL_Template_TemplateCreate_CreatesTemplate()
        {
            // Arrance
            CoreElement CElement = new CoreElement();

            DateTime startDt = DateTime.UtcNow;
            DateTime endDt = DateTime.UtcNow;
            MainElement main = new MainElement(1, "label1", 4, "folderWithList", 1, startDt,
                endDt, "Swahili", false, true, false, true, "type1", "MessageTitle",
                "MessageBody", false, CElement.ElementList, "");

            // Act
            int templateId = await sut.TemplateCreate(main);
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();

            CheckList cl1 = DbContext.CheckLists.AsNoTracking().First();
            // Assert
            Assert.NotNull(templateId);
            Assert.IsNull(cl1.ParentId);
            Assert.AreEqual(cl1.Id, templateId);
            Assert.AreEqual(cl1.Label, null);
            Assert.AreEqual("label1", checkLisTranslations[0].Text);
            Assert.AreEqual(cl1.FolderName, "folderWithList");
            Assert.AreEqual(cl1.CaseType, "type1");
            Assert.AreEqual(cl1.DisplayIndex, 4);
            Assert.AreEqual(cl1.WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(cl1.Version, 1);
            Assert.AreEqual(cl1.ManualSync, 1);
            Assert.AreEqual(cl1.MultiApproval, 0);
            Assert.AreEqual(cl1.FastNavigation, 1);
            Assert.AreEqual(cl1.DownloadEntities, 0);
            Assert.AreEqual(cl1.ExtraFieldsEnabled, 0);
            Assert.AreEqual(cl1.DoneButtonEnabled, 0);
            Assert.AreEqual(cl1.ApprovalEnabled, 0);
            Assert.AreEqual(cl1.ReviewEnabled, 0);
            Assert.AreEqual(cl1.Repeated, 1);
        }

        [Test]
        public async Task SQL_Template_TemplateItemRead_ReadsItems()
        {
            // Arrance

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
                5, 1, DbContext.FieldTypes.Where(x => x.Type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55,
                "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region Field2

            Field Field2 = await testHelpers.CreateField(1, "barcode", subTemplate1, "f5eafa", "custom", null, "",
                "showPDf Description",
                45, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 1, 0, 0,
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

            #region Tag

            Tag tag = await testHelpers.CreateTag("Tag1", Constants.WorkflowStates.Created, 1);

            #endregion

            // Act

            var match1 = await sut.TemplateItemRead(Template1.Id, language);
            var match2 = await sut.TemplateItemRead(Template2.Id, language);
            var match3 = await sut.TemplateItemRead(Template3.Id, language);
            var match4 = await sut.TemplateItemRead(Template4.Id, language);


            // Assert

            #region template1

            Assert.NotNull(match1);
            Assert.AreEqual(match1.Description, "Description1");
            Assert.AreEqual(match1.Label, "Label1");
            Assert.AreEqual(match1.CreatedAt.ToString(), Template1.CreatedAt.ToString());
            Assert.AreEqual(match1.FolderName, "FolderWithTemplate");
            Assert.AreEqual(match1.Id, Template1.Id);
//            Assert.AreEqual(match1.UpdatedAt.ToString(), Template1.UpdatedAt.ToString());

            #endregion

            #region template2

            Assert.NotNull(match1);
            Assert.AreEqual(match2.Description, "Description2");
            Assert.AreEqual(match2.Label, "Label2");
            Assert.AreEqual(match2.CreatedAt.ToString(), Template2.CreatedAt.ToString());
            Assert.AreEqual(match2.FolderName, "FolderWithTemplate");
            Assert.AreEqual(match2.Id, Template2.Id);
//            Assert.AreEqual(match2.UpdatedAt.ToString(), Template2.UpdatedAt.ToString());

            #endregion

            #region template3

            Assert.NotNull(match1);
            Assert.AreEqual(match3.Description, "Description3");
            Assert.AreEqual(match3.Label, "Label3");
            Assert.AreEqual(match3.CreatedAt.ToString(), Template3.CreatedAt.ToString());
            Assert.AreEqual(match3.FolderName, "FolderWithTemplate");
            Assert.AreEqual(match3.Id, Template3.Id);
//            Assert.AreEqual(match3.UpdatedAt.ToString(), Template3.UpdatedAt.ToString());

            #endregion

            #region template4

            Assert.NotNull(match1);
            Assert.AreEqual(match4.Description, "Description4");
            Assert.AreEqual(match4.Label, "Label4");
            Assert.AreEqual(match4.CreatedAt.ToString(), Template4.CreatedAt.ToString());
            Assert.AreEqual(match4.FolderName, "FolderWithTemplate");
            Assert.AreEqual(match4.Id, Template4.Id);
//            Assert.AreEqual(match4.UpdatedAt.ToString(), Template4.UpdatedAt.ToString());

            #endregion
        }

        [Test]
        public async Task SQL_Template_TemplateFieldReadAll_ReturnsFieldList()
        {
            // Arrance

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

            var match1 = await sut.TemplateFieldReadAll(Template1.Id, language);
            var match2 = await sut.TemplateFieldReadAll(Template2.Id, language);
            var match3 = await sut.TemplateFieldReadAll(Template3.Id, language);
            var match4 = await sut.TemplateFieldReadAll(Template4.Id, language);

            // Assert

            #region template1

            Assert.NotNull(match1);
            Assert.AreEqual(match1[0].Description, Field1.Description);
            Assert.AreEqual(match1[0].FieldType, "Picture");
            Assert.AreEqual(match1[0].Label, Field1.Label);
            Assert.AreEqual(match1[0].Id, Field1.Id);

            Assert.AreEqual(match1[1].Description, Field2.Description);
            Assert.AreEqual(match1[1].FieldType, "Comment");
            Assert.AreEqual(match1[1].Label, Field2.Label);
            Assert.AreEqual(match1[1].Id, Field2.Id);

            #endregion

            #region template2

            Assert.NotNull(match2);
            Assert.AreEqual(match2[0].Description, Field3.Description);
            Assert.AreEqual(match2[0].FieldType, "Picture");
            Assert.AreEqual(match2[0].Label, Field3.Label);
            Assert.AreEqual(match2[0].Id, Field3.Id);

            Assert.AreEqual(match2[1].Description, Field4.Description);
            Assert.AreEqual(match2[1].FieldType, "Picture");
            Assert.AreEqual(match2[1].Label, Field4.Label);
            Assert.AreEqual(match2[1].Id, Field4.Id);

            Assert.AreEqual(match2[2].Description, Field5.Description);
            Assert.AreEqual(match2[2].FieldType, "Comment");
            Assert.AreEqual(match2[2].Label, Field5.Label);
            Assert.AreEqual(match2[2].Id, Field5.Id);

            #endregion

            #region template3

            Assert.NotNull(match3);
            Assert.AreEqual(match3[0].Description, Field6.Description);
            Assert.AreEqual(match3[0].FieldType, "Comment");
            Assert.AreEqual(match3[0].Label, Field6.Label);
            Assert.AreEqual(match3[0].Id, Field6.Id);

            Assert.AreEqual(match3[1].Description, Field7.Description);
            Assert.AreEqual(match3[1].FieldType, "Comment");
            Assert.AreEqual(match3[1].Label, Field7.Label);
            Assert.AreEqual(match3[1].Id, Field7.Id);

            #endregion

            #region template4

            Assert.NotNull(match4);
            Assert.AreEqual(match4[0].Description, Field8.Description);
            Assert.AreEqual(match4[0].FieldType, "Comment");
            Assert.AreEqual(match4[0].Label, Field8.Label);
            Assert.AreEqual(match4[0].Id, Field8.Id);

            Assert.AreEqual(match4[1].Description, Field9.Description);
            Assert.AreEqual(match4[1].FieldType, "Comment");
            Assert.AreEqual(match4[1].Label, Field9.Label);
            Assert.AreEqual(match4[1].Id, Field9.Id);

            Assert.AreEqual(match4[2].Description, Field10.Description);
            Assert.AreEqual(match4[2].FieldType, "Comment");
            Assert.AreEqual(match4[2].Label, Field10.Label);
            Assert.AreEqual(match4[2].Id, Field10.Id);

            #endregion
        }

        [Test]
        public async Task SQL_Template_TemplateDisplayIndexChange_ChangesDisplayIndex()
        {
            // Arrance

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

            // Act
            var match1 = await sut.TemplateDisplayIndexChange(Template1.Id, 5);
            var match2 = await sut.TemplateDisplayIndexChange(Template2.Id, 1);
            var match3 = await sut.TemplateDisplayIndexChange(Template3.Id, 2);
            var match4 = await sut.TemplateDisplayIndexChange(Template4.Id, 3);
            // Assert
            Assert.NotNull(match1);
            Assert.NotNull(match2);
            Assert.NotNull(match3);
            Assert.NotNull(match4);
            Assert.True(match1);
            Assert.True(match2);
            Assert.True(match3);
            Assert.True(match4);
        }

        [Test]
        public async Task SQL_Template_TemplateSetTags_SetsTag()
        {
            // Arrance

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
                5, 1, DbContext.FieldTypes.Where(x => x.Type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55,
                "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region Field2

            Field Field2 = await testHelpers.CreateField(1, "barcode", subTemplate1, "f5eafa", "custom", null, "",
                "showPDf Description",
                45, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 1, 0, 0,
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

            #region Tags

            #region Tag1

            Tag tag1 = await testHelpers.CreateTag("Tag1", Constants.WorkflowStates.Created, 1);

            #endregion

            #region Tag2

            Tag tag2 = await testHelpers.CreateTag("Tag2", Constants.WorkflowStates.Created, 1);

            #endregion

            #region Tag3

            Tag tag3 = await testHelpers.CreateTag("Tag3", Constants.WorkflowStates.Created, 1);

            #endregion

            #region Tag4

            Tag tag4 = await testHelpers.CreateTag("Tag4", Constants.WorkflowStates.Created, 1);

            #endregion

            #region Tag5

            Tag tag5 = await testHelpers.CreateTag("Tag5", Constants.WorkflowStates.Created, 1);

            #endregion

            #region Tag6

            Tag tag6 = await testHelpers.CreateTag("Tag6", Constants.WorkflowStates.Created, 1);

            #endregion

            #endregion

            // Act
            List<int> taglist = new List<int>
            {
                tag1.Id,
                tag2.Id,
                tag3.Id,
                tag4.Id,
                tag5.Id,
                tag6.Id
            };

            var match1 = await sut.TemplateSetTags(Template1.Id, taglist);
            var match2 = await sut.TemplateSetTags(Template2.Id, taglist);
            var match3 = await sut.TemplateSetTags(Template3.Id, taglist);
            var match4 = await sut.TemplateSetTags(Template4.Id, taglist);

            // Assert

            Assert.NotNull(match1);
            Assert.NotNull(match2);
            Assert.NotNull(match3);
            Assert.NotNull(match4);
            Assert.True(match1);
            Assert.True(match2);
            Assert.True(match3);
            Assert.True(match4);
        }

        [Test]
        public async Task SQL_Template_TemplateUpdateFieldIdsForColumns_UpdatesIds()
        {
            // Arrance

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
                5, 1, DbContext.FieldTypes.Where(x => x.Type == "picture").First(), 0, 0, 1, 0, "Comment field", 1, 55,
                "55", "0", 0, 0, null, 1, 0,
                0, 0, "", 49);

            #endregion

            #region Field2

            Field Field2 = await testHelpers.CreateField(1, "barcode", subTemplate1, "f5eafa", "custom", null, "",
                "showPDf Description",
                45, 1, DbContext.FieldTypes.Where(x => x.Type == "comment").First(), 0, 1, 0, 0,
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

            var match1 = await sut.TemplateUpdateFieldIdsForColumns(Template1.Id, Field1.Id, Field2.Id, Field3.Id,
                Field4.Id, Field5.Id, Field6.Id, Field7.Id, Field8.Id, Field9.Id, Field10.Id);
            var match2 = await sut.TemplateUpdateFieldIdsForColumns(Template2.Id, Field1.Id, Field2.Id, Field3.Id,
                Field4.Id, Field5.Id, Field6.Id, Field7.Id, Field8.Id, Field9.Id, Field10.Id);
            var match3 = await sut.TemplateUpdateFieldIdsForColumns(Template3.Id, Field1.Id, Field2.Id, Field3.Id,
                Field4.Id, Field5.Id, Field6.Id, Field7.Id, Field8.Id, Field9.Id, Field10.Id);
            var match4 = await sut.TemplateUpdateFieldIdsForColumns(Template4.Id, Field1.Id, Field2.Id, Field3.Id,
                Field4.Id, Field5.Id, Field6.Id, Field7.Id, Field8.Id, Field9.Id, Field10.Id);

            // Assert
            Assert.NotNull(match1);
            Assert.NotNull(match2);
            Assert.NotNull(match3);
            Assert.NotNull(match4);
            Assert.True(match1);
            Assert.True(match2);
            Assert.True(match3);
            Assert.True(match4);
        }

        #endregion


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

#endregion