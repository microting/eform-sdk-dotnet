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
            language = DbContext.Languages.Single(x => x.Name == "Dansk");
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
            Assert.That(templateListId, Is.Not.EqualTo(null));
            Assert.That(templateListId.Count(), Is.EqualTo(4));
            Assert.That(templateListId[0].Label, Is.EqualTo("A"));
            Assert.That(templateListId[1].Label, Is.EqualTo("B"));
            Assert.That(templateListId[2].Label, Is.EqualTo("D"));
            Assert.That(templateListId[3].Label, Is.EqualTo("C"));
            Assert.That(templateListId[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListId[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListId[2].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListId[3].Tags.Count(), Is.EqualTo(0));

            // Default sorting including removed
            // Label
            Assert.That(templateListLabel, Is.Not.EqualTo(null));
            Assert.That(templateListLabel.Count(), Is.EqualTo(4));
            Assert.That(templateListLabel[0].Label, Is.EqualTo("A"));
            Assert.That(templateListLabel[1].Label, Is.EqualTo("B"));
            Assert.That(templateListLabel[2].Label, Is.EqualTo("C"));
            Assert.That(templateListLabel[3].Label, Is.EqualTo("D"));
            Assert.That(templateListLabel[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListLabel[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListLabel[2].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListLabel[3].Tags.Count(), Is.EqualTo(0));

            // Default sorting including removed
            // Description
            Assert.That(templateListDescription, Is.Not.EqualTo(null));
            Assert.That(templateListDescription.Count(), Is.EqualTo(4));
            Assert.That(templateListDescription[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescription[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescription[2].Label, Is.EqualTo("B"));
            Assert.That(templateListDescription[3].Label, Is.EqualTo("A"));
            Assert.That(templateListDescription[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescription[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescription[2].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescription[3].Tags.Count(), Is.EqualTo(0));

            // Default sorting including removed
            // Created At
            Assert.That(templateListCreatedAt, Is.Not.EqualTo(null));
            Assert.That(templateListCreatedAt.Count(), Is.EqualTo(4));
            Assert.That(templateListCreatedAt[0].Label, Is.EqualTo("A"));
            Assert.That(templateListCreatedAt[1].Label, Is.EqualTo("B"));
            Assert.That(templateListCreatedAt[2].Label, Is.EqualTo("D"));
            Assert.That(templateListCreatedAt[3].Label, Is.EqualTo("C"));
            Assert.That(templateListCreatedAt[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListCreatedAt[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListCreatedAt[2].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListCreatedAt[3].Tags.Count(), Is.EqualTo(0));

            // Descending sorting including removed
            // Id
            Assert.That(templateListDescengingId, Is.Not.EqualTo(null));
            Assert.That(templateListDescengingId.Count(), Is.EqualTo(4));
            Assert.That(templateListDescengingId[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingId[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingId[2].Label, Is.EqualTo("B"));
            Assert.That(templateListDescengingId[3].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingId[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingId[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingId[2].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingId[3].Tags.Count(), Is.EqualTo(0));

            // Descending sorting including removed
            // Label
            Assert.That(templateListDescengingLabel, Is.Not.EqualTo(null));
            Assert.That(templateListDescengingLabel.Count(), Is.EqualTo(4));
            Assert.That(templateListDescengingLabel[0].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingLabel[1].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingLabel[2].Label, Is.EqualTo("B"));
            Assert.That(templateListDescengingLabel[3].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingLabel[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingLabel[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingLabel[2].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingLabel[3].Tags.Count(), Is.EqualTo(0));

            // Descending sorting including removed
            // Description
            Assert.That(templateListDescengingDescription, Is.Not.EqualTo(null));
            Assert.That(templateListDescengingDescription.Count(), Is.EqualTo(4));
            Assert.That(templateListDescengingDescription[0].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingDescription[1].Label, Is.EqualTo("B"));
            Assert.That(templateListDescengingDescription[2].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingDescription[3].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingDescription[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingDescription[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingDescription[2].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingDescription[3].Tags.Count(), Is.EqualTo(0));

            // Descending sorting including removed
            // Created At
            Assert.That(templateListDescengingCreatedAt, Is.Not.EqualTo(null));
            Assert.That(templateListDescengingCreatedAt.Count(), Is.EqualTo(4));
            Assert.That(templateListDescengingCreatedAt[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingCreatedAt[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingCreatedAt[2].Label, Is.EqualTo("B"));
            Assert.That(templateListDescengingCreatedAt[3].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingCreatedAt[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingCreatedAt[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingCreatedAt[2].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingCreatedAt[3].Tags.Count(), Is.EqualTo(0));

            #endregion

            #region Exclude removed

            // Default sorting including removed
            // Id
            Assert.That(templateListIdNr, Is.Not.EqualTo(null));
            Assert.That(templateListIdNr.Count(), Is.EqualTo(3));
            Assert.That(templateListIdNr[0].Label, Is.EqualTo("A"));
            Assert.That(templateListIdNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListIdNr[2].Label, Is.EqualTo("C"));
            Assert.That(templateListIdNr[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListIdNr[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListIdNr[2].Tags.Count(), Is.EqualTo(0));

            // Default sorting including removed
            // Label
            Assert.That(templateListLabelNr, Is.Not.EqualTo(null));
            Assert.That(templateListLabelNr.Count(), Is.EqualTo(3));
            Assert.That(templateListLabelNr[0].Label, Is.EqualTo("A"));
            Assert.That(templateListLabelNr[1].Label, Is.EqualTo("C"));
            Assert.That(templateListLabelNr[2].Label, Is.EqualTo("D"));
            Assert.That(templateListLabelNr[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListLabelNr[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListLabelNr[2].Tags.Count(), Is.EqualTo(0));

            // Default sorting including removed
            // Description
            Assert.That(templateListDescriptionNr, Is.Not.EqualTo(null));
            Assert.That(templateListDescriptionNr.Count(), Is.EqualTo(3));
            Assert.That(templateListDescriptionNr[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescriptionNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescriptionNr[2].Label, Is.EqualTo("A"));
            Assert.That(templateListDescriptionNr[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescriptionNr[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescriptionNr[2].Tags.Count(), Is.EqualTo(0));

            // Default sorting including removed
            // Created At
            Assert.That(templateListCreatedAtNr, Is.Not.EqualTo(null));
            Assert.That(templateListCreatedAtNr.Count(), Is.EqualTo(3));
            Assert.That(templateListCreatedAtNr[0].Label, Is.EqualTo("A"));
            Assert.That(templateListCreatedAtNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListCreatedAtNr[2].Label, Is.EqualTo("C"));
            Assert.That(templateListCreatedAtNr[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListCreatedAtNr[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListCreatedAtNr[2].Tags.Count(), Is.EqualTo(0));

            // Descending sorting including removed
            // Id
            Assert.That(templateListDescengingIdNr, Is.Not.EqualTo(null));
            Assert.That(templateListDescengingIdNr.Count(), Is.EqualTo(3));
            Assert.That(templateListDescengingIdNr[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingIdNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingIdNr[2].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingIdNr[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingIdNr[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingIdNr[2].Tags.Count(), Is.EqualTo(0));

            // Descending sorting including removed
            // Label
            Assert.That(templateListDescengingLabelNr, Is.Not.EqualTo(null));
            Assert.That(templateListDescengingLabelNr.Count(), Is.EqualTo(3));
            Assert.That(templateListDescengingLabelNr[0].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingLabelNr[1].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingLabelNr[2].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingLabelNr[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingLabelNr[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingLabelNr[2].Tags.Count(), Is.EqualTo(0));

            // Descending sorting including removed
            // Description
            Assert.That(templateListDescengingDescriptionNr, Is.Not.EqualTo(null));
            Assert.That(templateListDescengingDescriptionNr.Count(), Is.EqualTo(3));
            Assert.That(templateListDescengingDescriptionNr[0].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingDescriptionNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingDescriptionNr[2].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingDescriptionNr[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingDescriptionNr[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingDescriptionNr[2].Tags.Count(), Is.EqualTo(0));

            // Descending sorting including removed
            // Created At
            Assert.That(templateListDescengingCreatedAtNr, Is.Not.EqualTo(null));
            Assert.That(templateListDescengingCreatedAtNr.Count(), Is.EqualTo(3));
            Assert.That(templateListDescengingCreatedAtNr[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingCreatedAtNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingCreatedAtNr[2].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingCreatedAtNr[0].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingCreatedAtNr[1].Tags.Count(), Is.EqualTo(0));
            Assert.That(templateListDescengingCreatedAtNr[2].Tags.Count(), Is.EqualTo(0));

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

            Assert.That(clResult, Is.Not.EqualTo(null));
            Assert.That(checkLists.Count(), Is.EqualTo(1));
            Assert.That(checkLists[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
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
            Assert.That(templateId, Is.Not.EqualTo(null));
            Assert.That(cl1.ParentId, Is.EqualTo(null));
            Assert.That(templateId, Is.EqualTo(cl1.Id));
            Assert.That(cl1.Label, Is.Null);
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("label1"));
            Assert.That(cl1.FolderName, Is.EqualTo("folderWithList"));
            Assert.That(cl1.CaseType, Is.EqualTo("type1"));
            Assert.That(cl1.DisplayIndex, Is.EqualTo(4));
            Assert.That(cl1.WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(cl1.Version, Is.EqualTo(1));
            Assert.That(cl1.ManualSync, Is.EqualTo(1));
            Assert.That(cl1.MultiApproval, Is.EqualTo(0));
            Assert.That(cl1.FastNavigation, Is.EqualTo(1));
            Assert.That(cl1.DownloadEntities, Is.EqualTo(0));
            Assert.That(cl1.ExtraFieldsEnabled, Is.EqualTo(0));
            Assert.That(cl1.DoneButtonEnabled, Is.EqualTo(0));
            Assert.That(cl1.ApprovalEnabled, Is.EqualTo(0));
            Assert.That(cl1.ReviewEnabled, Is.EqualTo(0));
            Assert.That(cl1.Repeated, Is.EqualTo(1));
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

            Assert.That(match1, Is.Not.EqualTo(null));
            Assert.That(match1.Description, Is.EqualTo("Description1"));
            Assert.That(match1.Label, Is.EqualTo("Label1"));
            Assert.That(Template1.CreatedAt.ToString(), Is.EqualTo(match1.CreatedAt.ToString()));
            Assert.That(match1.FolderName, Is.EqualTo("FolderWithTemplate"));
            Assert.That(Template1.Id, Is.EqualTo(match1.Id));
//            Assert.AreEqual(match1.UpdatedAt.ToString(), Template1.UpdatedAt.ToString());

            #endregion

            #region template2

            Assert.That(match1, Is.Not.EqualTo(null));
            Assert.That(match2.Description, Is.EqualTo("Description2"));
            Assert.That(match2.Label, Is.EqualTo("Label2"));
            Assert.That(Template2.CreatedAt.ToString(), Is.EqualTo(match2.CreatedAt.ToString()));
            Assert.That(match2.FolderName, Is.EqualTo("FolderWithTemplate"));
            Assert.That(Template2.Id, Is.EqualTo(match2.Id));
//            Assert.AreEqual(match2.UpdatedAt.ToString(), Template2.UpdatedAt.ToString());

            #endregion

            #region template3

            Assert.That(match1, Is.Not.EqualTo(null));
            Assert.That(match3.Description, Is.EqualTo("Description3"));
            Assert.That(match3.Label, Is.EqualTo("Label3"));
            Assert.That(Template3.CreatedAt.ToString(), Is.EqualTo(match3.CreatedAt.ToString()));
            Assert.That(match3.FolderName, Is.EqualTo("FolderWithTemplate"));
            Assert.That(Template3.Id, Is.EqualTo(match3.Id));
//            Assert.AreEqual(match3.UpdatedAt.ToString(), Template3.UpdatedAt.ToString());

            #endregion

            #region template4

            Assert.That(match1, Is.Not.EqualTo(null));
            Assert.That(match4.Description, Is.EqualTo("Description4"));
            Assert.That(match4.Label, Is.EqualTo("Label4"));
            Assert.That(Template4.CreatedAt.ToString(), Is.EqualTo(match4.CreatedAt.ToString()));
            Assert.That(match4.FolderName, Is.EqualTo("FolderWithTemplate"));
            Assert.That(Template4.Id, Is.EqualTo(match4.Id));
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
            Assert.That(match1, Is.Not.EqualTo(null));
            Assert.That(match2, Is.Not.EqualTo(null));
            Assert.That(match3, Is.Not.EqualTo(null));
            Assert.That(match4, Is.Not.EqualTo(null));
            Assert.That(match1, Is.True);
            Assert.That(match2, Is.True);
            Assert.That(match3, Is.True);
            Assert.That(match4, Is.True);
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

            Assert.That(match1, Is.Not.EqualTo(null));
            Assert.That(match2, Is.Not.EqualTo(null));
            Assert.That(match3, Is.Not.EqualTo(null));
            Assert.That(match4, Is.Not.EqualTo(null));
            Assert.That(match1, Is.True);
            Assert.That(match2, Is.True);
            Assert.That(match3, Is.True);
            Assert.That(match4, Is.True);
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
            Assert.That(match1, Is.Not.EqualTo(null));
            Assert.That(match2, Is.Not.EqualTo(null));
            Assert.That(match3, Is.Not.EqualTo(null));
            Assert.That(match4, Is.Not.EqualTo(null));
            Assert.That(match1, Is.True);
            Assert.That(match2, Is.True);
            Assert.That(match3, Is.True);
            Assert.That(match4, Is.True);
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