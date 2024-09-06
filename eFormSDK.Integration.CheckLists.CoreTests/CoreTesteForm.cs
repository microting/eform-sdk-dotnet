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
using System.Threading.Tasks;
using eFormCore;
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

namespace eFormSDK.Integration.CheckLists.CoreTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class CoreTesteForm : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
        private Language language;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            await sql.SettingUpdate(Settings.comAddressNewApi, "none");

            #endregion

            sut = new Core();
            sut.HandleCaseCreated += EventCaseCreated;
            sut.HandleCaseRetrived += EventCaseRetrived;
            sut.HandleCaseCompleted += EventCaseCompleted;
            sut.HandleCaseDeleted += EventCaseDeleted;
            sut.HandleFileDownloaded += EventFileDownloaded;
            sut.HandleSiteActivated += EventSiteActivated;
            await sut.StartSqlOnly(ConnectionString);
            path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path).Replace(@"file:", "");
            await sut.SetSdkSetting(Settings.fileLocationPicture,
                Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers(ConnectionString);
            await testHelpers.GenerateDefaultLanguages();
            //sut.StartLog(new CoreBase());
            language = DbContext.Languages.Single(x => x.Name == "Dansk");
        }

        #region template

        [Test]
        public async Task Core_Template_TemplateItemReadAll_DoesReturnSortedTemplates()
        {
            // Arrance

            #region Tags

            string tagName1 = "TagFor1CL";
            Tag tag1 = new Tag
            {
                Name = tagName1, WorkflowState = Constants.WorkflowStates.Created
            };

            await DbContext.Tags.AddAsync(tag1);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            string tagName2 = "TagFor2CLs";
            Tag tag2 = new Tag
            {
                Name = tagName2, WorkflowState = Constants.WorkflowStates.Created
            };

            await DbContext.Tags.AddAsync(tag2);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            string tagName3 = "Tag3";
            Tag tag3 = new Tag
            {
                Name = tagName3, WorkflowState = Constants.WorkflowStates.Created
            };

            await DbContext.Tags.AddAsync(tag3);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            string tagName4 = "Tag4";
            Tag tag4 = new Tag
            {
                Name = tagName4, WorkflowState = Constants.WorkflowStates.Created
            };

            await DbContext.Tags.AddAsync(tag4);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            #endregion

            #region Template1

            CheckList cl1 = await testHelpers.CreateTemplate(DateTime.UtcNow, DateTime.UtcNow, "A", "D", "CheckList",
                "TemplateFolderName", 1, 1);

            #endregion

            #region Template2

            CheckList cl2 = await testHelpers.CreateTemplate(DateTime.UtcNow, DateTime.UtcNow, "B", "C", "CheckList",
                "TemplateFolderName", 1, 1);

            await cl2.Delete(DbContext);

            #endregion

            #region Template3

            CheckList cl3 = await testHelpers.CreateTemplate(DateTime.UtcNow, DateTime.UtcNow, "D", "B", "CheckList",
                "TemplateFolderName", 1, 1);

            #endregion

            #region Template4

            CheckList cl4 = await testHelpers.CreateTemplate(DateTime.UtcNow, DateTime.UtcNow, "C", "A", "CheckList",
                "TemplateFolderName", 1, 1);

            #endregion

            #region assigning Tags

            List<int> tagIds1 = new List<int>();
            List<int> tagIds2 = new List<int>();
            List<int> tagIds3 = new List<int>();
            List<int> tagIds4 = new List<int>();


            tagIds1.Add(tag1.Id);
            tagIds2.Add(tag2.Id);
            tagIds3.Add(tag3.Id);
            tagIds4.Add(tag3.Id);
            tagIds4.Add(tag4.Id);

            await sut.TemplateSetTags(cl1.Id, tagIds1);
            await sut.TemplateSetTags(cl2.Id, tagIds2);
            await sut.TemplateSetTags(cl3.Id, tagIds2);
            await sut.TemplateSetTags(cl4.Id, tagIds4);

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
            List<Template_Dto> templateListTag = await sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created,
                "", false, Constants.eFormSortParameters.Tags, emptyList, timeZoneInfo, language);
            List<Template_Dto> templateListSpecificTag = await sut.TemplateItemReadAll(true,
                Constants.WorkflowStates.Created, "", false, Constants.eFormSortParameters.Tags, tagIds2, timeZoneInfo,
                language);


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
            List<Template_Dto> templateListDescendingTag = await sut.TemplateItemReadAll(true,
                Constants.WorkflowStates.Created, "", true, Constants.eFormSortParameters.Tags, emptyList, timeZoneInfo,
                language);
            List<Template_Dto> templateListDescendingSpecificTag = await sut.TemplateItemReadAll(true,
                Constants.WorkflowStates.Created, "", true, Constants.eFormSortParameters.Tags, tagIds2, timeZoneInfo,
                language);

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
            List<Template_Dto> templateListTagNr = await sut.TemplateItemReadAll(false,
                Constants.WorkflowStates.Created, "", false, Constants.eFormSortParameters.Tags, emptyList,
                timeZoneInfo, language);
            List<Template_Dto> templateListSpecificTagNr = await sut.TemplateItemReadAll(false,
                Constants.WorkflowStates.Created, "", false, Constants.eFormSortParameters.Tags, tagIds2, timeZoneInfo,
                language);

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
            List<Template_Dto> templateListDescendingTagNr = await sut.TemplateItemReadAll(false,
                Constants.WorkflowStates.Created, "", true, Constants.eFormSortParameters.Tags, emptyList, timeZoneInfo,
                language);
            List<Template_Dto> templateListDescendingSpecificTagNr = await sut.TemplateItemReadAll(false,
                Constants.WorkflowStates.Created, "", true, Constants.eFormSortParameters.Tags, tagIds2, timeZoneInfo,
                language);

            // Assert

            #region include removed

            // Default sorting including removed
            // Id
            Assert.NotNull(templateListId);
            Assert.That(templateListId.Count(), Is.EqualTo(4));
            Assert.That(templateListId[0].Label, Is.EqualTo("A"));
            Assert.That(templateListId[1].Label, Is.EqualTo("B"));
            Assert.That(templateListId[2].Label, Is.EqualTo("D"));
            Assert.That(templateListId[3].Label, Is.EqualTo("C"));
            Assert.That(templateListId[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListId[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListId[2].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListId[3].Tags.Count(), Is.EqualTo(2));

            // Default sorting including removed
            // Label
            Assert.NotNull(templateListLabel);
            Assert.That(templateListLabel.Count(), Is.EqualTo(4));
            Assert.That(templateListLabel[0].Label, Is.EqualTo("A"));
            Assert.That(templateListLabel[1].Label, Is.EqualTo("B"));
            Assert.That(templateListLabel[2].Label, Is.EqualTo("C"));
            Assert.That(templateListLabel[3].Label, Is.EqualTo("D"));
            Assert.That(templateListLabel[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListLabel[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListLabel[2].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListLabel[3].Tags.Count(), Is.EqualTo(1));

            // Default sorting including removed
            // Description
            Assert.NotNull(templateListDescription);
            Assert.That(templateListDescription.Count(), Is.EqualTo(4));
            Assert.That(templateListDescription[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescription[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescription[2].Label, Is.EqualTo("B"));
            Assert.That(templateListDescription[3].Label, Is.EqualTo("A"));
            Assert.That(templateListDescription[0].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListDescription[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescription[2].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescription[3].Tags.Count(), Is.EqualTo(1));

            // Default sorting including removed
            // Created At
            Assert.NotNull(templateListCreatedAt);
            Assert.That(templateListCreatedAt.Count(), Is.EqualTo(4));
            Assert.That(templateListCreatedAt[0].Label, Is.EqualTo("A"));
            Assert.That(templateListCreatedAt[1].Label, Is.EqualTo("B"));
            Assert.That(templateListCreatedAt[2].Label, Is.EqualTo("D"));
            Assert.That(templateListCreatedAt[3].Label, Is.EqualTo("C"));
            Assert.That(templateListCreatedAt[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListCreatedAt[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListCreatedAt[2].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListCreatedAt[3].Tags.Count(), Is.EqualTo(2));

            // Default sorting including removed
            // Tag
            Assert.NotNull(templateListTag);
            Assert.That(templateListTag.Count(), Is.EqualTo(4));
            Assert.That(templateListTag[0].Label, Is.EqualTo("A"));
            Assert.That(templateListTag[1].Label, Is.EqualTo("B"));
            Assert.That(templateListTag[2].Label, Is.EqualTo("D"));
            Assert.That(templateListTag[3].Label, Is.EqualTo("C"));
            Assert.That(templateListTag[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListTag[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListTag[2].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListTag[3].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListTag[0].Tags[0].Value, Is.EqualTo("TagFor1CL"));
            Assert.That(templateListTag[1].Tags[0].Value, Is.EqualTo("TagFor2CLs"));
            Assert.That(templateListTag[2].Tags[0].Value, Is.EqualTo("TagFor2CLs"));
            Assert.That(templateListTag[3].Tags[0].Value, Is.EqualTo("Tag3"));
            Assert.That(templateListTag[3].Tags[1].Value, Is.EqualTo("Tag4"));

            // Default sorting including removed
            // Tagid
            Assert.NotNull(templateListSpecificTag);
            Assert.That(templateListSpecificTag.Count(), Is.EqualTo(2));
            Assert.That(templateListSpecificTag[0].Label, Is.EqualTo("B"));
            Assert.That(templateListSpecificTag[1].Label, Is.EqualTo("D"));
            Assert.That(templateListSpecificTag[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListSpecificTag[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListSpecificTag[0].Tags[0].Value, Is.EqualTo("TagFor2CLs"));
            Assert.That(templateListSpecificTag[1].Tags[0].Value, Is.EqualTo("TagFor2CLs"));

            // Descending sorting including removed
            // Id
            Assert.NotNull(templateListDescengingId);
            Assert.That(templateListDescengingId.Count(), Is.EqualTo(4));
            Assert.That(templateListDescengingId[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingId[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingId[2].Label, Is.EqualTo("B"));
            Assert.That(templateListDescengingId[3].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingId[0].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListDescengingId[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingId[2].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingId[3].Tags.Count(), Is.EqualTo(1));

            // Descending sorting including removed
            // Label
            Assert.NotNull(templateListDescengingLabel);
            Assert.That(templateListDescengingLabel.Count(), Is.EqualTo(4));
            Assert.That(templateListDescengingLabel[0].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingLabel[1].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingLabel[2].Label, Is.EqualTo("B"));
            Assert.That(templateListDescengingLabel[3].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingLabel[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingLabel[1].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListDescengingLabel[2].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingLabel[3].Tags.Count(), Is.EqualTo(1));

            // Descending sorting including removed
            // Description
            Assert.NotNull(templateListDescengingDescription);
            Assert.That(templateListDescengingDescription.Count(), Is.EqualTo(4));
            Assert.That(templateListDescengingDescription[0].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingDescription[1].Label, Is.EqualTo("B"));
            Assert.That(templateListDescengingDescription[2].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingDescription[3].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingDescription[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingDescription[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingDescription[2].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingDescription[3].Tags.Count(), Is.EqualTo(2));

            // Descending sorting including removed
            // Created At
            Assert.NotNull(templateListDescengingCreatedAt);
            Assert.That(templateListDescengingCreatedAt.Count(), Is.EqualTo(4));
            Assert.That(templateListDescengingCreatedAt[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingCreatedAt[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingCreatedAt[2].Label, Is.EqualTo("B"));
            Assert.That(templateListDescengingCreatedAt[3].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingCreatedAt[0].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListDescengingCreatedAt[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingCreatedAt[2].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingCreatedAt[3].Tags.Count(), Is.EqualTo(1));

            // Descending sorting including removed
            // Tag
            Assert.NotNull(templateListTag);
            Assert.That(templateListTag.Count(), Is.EqualTo(4));
            Assert.That(templateListDescendingTag[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescendingTag[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescendingTag[2].Label, Is.EqualTo("B"));
            Assert.That(templateListDescendingTag[3].Label, Is.EqualTo("A"));
            Assert.That(templateListDescendingTag[0].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListDescendingTag[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescendingTag[2].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescendingTag[3].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescendingTag[0].Tags[0].Value, Is.EqualTo("Tag3"));
            Assert.That(templateListDescendingTag[0].Tags[1].Value, Is.EqualTo("Tag4"));
            Assert.That(templateListDescendingTag[1].Tags[0].Value, Is.EqualTo("TagFor2CLs"));
            Assert.That(templateListDescendingTag[2].Tags[0].Value, Is.EqualTo("TagFor2CLs"));
            Assert.That(templateListDescendingTag[3].Tags[0].Value, Is.EqualTo("TagFor1CL"));

            // Descending sorting including removed
            // Tagid
            Assert.NotNull(templateListDescendingSpecificTag);
            Assert.That(templateListDescendingSpecificTag.Count(), Is.EqualTo(2));
            Assert.That(templateListDescendingSpecificTag[0].Label, Is.EqualTo("D"));
            Assert.That(templateListDescendingSpecificTag[1].Label, Is.EqualTo("B"));
            Assert.That(templateListDescendingSpecificTag[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescendingSpecificTag[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescendingSpecificTag[0].Tags[0].Value, Is.EqualTo("TagFor2CLs"));
            Assert.That(templateListDescendingSpecificTag[1].Tags[0].Value, Is.EqualTo("TagFor2CLs"));

            #endregion

            #region Exclude removed

            // Default sorting excluding removed
            // Id
            Assert.NotNull(templateListIdNr);
            Assert.That(templateListIdNr.Count(), Is.EqualTo(3));
            Assert.That(templateListIdNr[0].Label, Is.EqualTo("A"));
            Assert.That(templateListIdNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListIdNr[2].Label, Is.EqualTo("C"));
            Assert.That(templateListIdNr[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListIdNr[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListIdNr[2].Tags.Count(), Is.EqualTo(2));

            // Default sorting excluding removed
            // Label
            Assert.NotNull(templateListLabelNr);
            Assert.That(templateListLabelNr.Count(), Is.EqualTo(3));
            Assert.That(templateListLabelNr[0].Label, Is.EqualTo("A"));
            Assert.That(templateListLabelNr[1].Label, Is.EqualTo("C"));
            Assert.That(templateListLabelNr[2].Label, Is.EqualTo("D"));
            Assert.That(templateListLabelNr[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListLabelNr[1].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListLabelNr[2].Tags.Count(), Is.EqualTo(1));

            // Default sorting excluding removed
            // Description
            Assert.NotNull(templateListDescriptionNr);
            Assert.That(templateListDescriptionNr.Count(), Is.EqualTo(3));
            Assert.That(templateListDescriptionNr[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescriptionNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescriptionNr[2].Label, Is.EqualTo("A"));
            Assert.That(templateListDescriptionNr[0].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListDescriptionNr[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescriptionNr[2].Tags.Count(), Is.EqualTo(1));

            // Default sorting excluding removed
            // Created At
            Assert.NotNull(templateListCreatedAtNr);
            Assert.That(templateListCreatedAtNr.Count(), Is.EqualTo(3));
            Assert.That(templateListCreatedAtNr[0].Label, Is.EqualTo("A"));
            Assert.That(templateListCreatedAtNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListCreatedAtNr[2].Label, Is.EqualTo("C"));
            Assert.That(templateListCreatedAtNr[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListCreatedAtNr[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListCreatedAtNr[2].Tags.Count(), Is.EqualTo(2));

            // Default sorting excluding removed
            // Tag
            Assert.NotNull(templateListTag);
            Assert.That(templateListTag.Count(), Is.EqualTo(4));
            Assert.That(templateListTagNr[0].Label, Is.EqualTo("A"));
            Assert.That(templateListTagNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListTagNr[2].Label, Is.EqualTo("C"));
            Assert.That(templateListTagNr[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListTagNr[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListTagNr[2].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListTagNr[0].Tags[0].Value, Is.EqualTo("TagFor1CL"));
            Assert.That(templateListTagNr[1].Tags[0].Value, Is.EqualTo("TagFor2CLs"));
            Assert.That(templateListTagNr[2].Tags[0].Value, Is.EqualTo("Tag3"));
            Assert.That(templateListTagNr[2].Tags[1].Value, Is.EqualTo("Tag4"));

            // Default sorting excluding removed
            // Tagid
            Assert.NotNull(templateListSpecificTagNr);
            Assert.That(templateListSpecificTagNr.Count(), Is.EqualTo(1));
            Assert.That(templateListSpecificTagNr[0].Label, Is.EqualTo("D"));
            Assert.That(templateListSpecificTagNr[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListSpecificTagNr[0].Tags[0].Value, Is.EqualTo("TagFor2CLs"));

            // Descending sorting excluding removed
            // Id
            Assert.NotNull(templateListDescengingIdNr);
            Assert.That(templateListDescengingIdNr.Count(), Is.EqualTo(3));
            Assert.That(templateListDescengingIdNr[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingIdNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingIdNr[2].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingIdNr[0].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListDescengingIdNr[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingIdNr[2].Tags.Count(), Is.EqualTo(1));

            // Descending sorting excluding removed
            // Label
            Assert.NotNull(templateListDescengingLabelNr);
            Assert.That(templateListDescengingLabelNr.Count(), Is.EqualTo(3));
            Assert.That(templateListDescengingLabelNr[0].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingLabelNr[1].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingLabelNr[2].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingLabelNr[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingLabelNr[1].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListDescengingLabelNr[2].Tags.Count(), Is.EqualTo(1));

            // Descending sorting excluding removed
            // Description
            Assert.NotNull(templateListDescengingDescriptionNr);
            Assert.That(templateListDescengingDescriptionNr.Count(), Is.EqualTo(3));
            Assert.That(templateListDescengingDescriptionNr[0].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingDescriptionNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingDescriptionNr[2].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingDescriptionNr[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingDescriptionNr[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingDescriptionNr[2].Tags.Count(), Is.EqualTo(2));

            // Descending sorting excluding removed
            // Created At
            Assert.NotNull(templateListDescengingCreatedAtNr);
            Assert.That(templateListDescengingCreatedAtNr.Count(), Is.EqualTo(3));
            Assert.That(templateListDescengingCreatedAtNr[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescengingCreatedAtNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescengingCreatedAtNr[2].Label, Is.EqualTo("A"));
            Assert.That(templateListDescengingCreatedAtNr[0].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListDescengingCreatedAtNr[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescengingCreatedAtNr[2].Tags.Count(), Is.EqualTo(1));

            // Descending sorting excluding removed
            // Tag
            Assert.NotNull(templateListTag);
            Assert.That(templateListTag.Count(), Is.EqualTo(4));
            Assert.That(templateListDescendingTagNr[0].Label, Is.EqualTo("C"));
            Assert.That(templateListDescendingTagNr[1].Label, Is.EqualTo("D"));
            Assert.That(templateListDescendingTagNr[2].Label, Is.EqualTo("A"));
            Assert.That(templateListDescendingTagNr[0].Tags.Count(), Is.EqualTo(2));
            Assert.That(templateListDescendingTagNr[1].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescendingTagNr[2].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescendingTagNr[0].Tags[0].Value, Is.EqualTo("Tag3"));
            Assert.That(templateListDescendingTagNr[0].Tags[1].Value, Is.EqualTo("Tag4"));
            Assert.That(templateListDescendingTagNr[1].Tags[0].Value, Is.EqualTo("TagFor2CLs"));
            Assert.That(templateListDescendingTagNr[2].Tags[0].Value, Is.EqualTo("TagFor1CL"));

            // Descending sorting excluding removed
            // Tagid
            Assert.NotNull(templateListDescendingSpecificTagNr);
            Assert.That(templateListDescendingSpecificTagNr.Count(), Is.EqualTo(1));
            Assert.That(templateListDescendingSpecificTagNr[0].Label, Is.EqualTo("D"));
            Assert.That(templateListDescendingSpecificTagNr[0].Tags.Count(), Is.EqualTo(1));
            Assert.That(templateListDescendingSpecificTagNr[0].Tags[0].Value, Is.EqualTo("TagFor2CLs"));

            #endregion
        }

        [Test]
        public async Task Core_Template_TemplateValidation_ReturnsErrorLst()
        {
            // Arrange
            CoreElement CElement = new CoreElement();
            //CElement.ElementList = new List<Element>();

            DateTime startDt = DateTime.UtcNow;
            DateTime endDt = DateTime.UtcNow;
            MainElement main = new MainElement(1, "label1", 4, "folderWithList", 1, startDt,
                endDt, "Swahili", false, true, false, true, "type1", "MessageTitle",
                "MessageBody", false, CElement.ElementList, "");
            // Act
            var match = await sut.TemplateValidation(main);
            // Assert
            Assert.NotNull(match);
            Assert.That(0, Is.EqualTo(match.Count()));
        }

        [Test]
        public async Task Core_Template_TemplateUploAddAsyncata_ReturnsmainElement()
        {
            // Arrange
            CoreElement CElement = new CoreElement();
            //CElement.ElementList = new List<Element>();

            DateTime startDt = DateTime.UtcNow;
            DateTime endDt = DateTime.UtcNow;
            MainElement main = new MainElement(1, "label1", 0, "folderWithList", 1, startDt,
                endDt, "Swahili", false, true, true, true, "type1", "MessageTitle",
                "MessageBody", false, CElement.ElementList, "");
            // Act
            var match = await sut.TemplateUploadData(main);
            // Assert

            #region Assert

            Assert.NotNull(match);
            Assert.That(main.CaseType, Is.EqualTo(match.CaseType));
            Assert.That(main.CheckListFolderName, Is.EqualTo(match.CheckListFolderName));
            Assert.That(main.DisplayOrder, Is.EqualTo(match.DisplayOrder));
            Assert.That(main.DownloadEntities, Is.EqualTo(match.DownloadEntities));
            Assert.That(main.ElementList, Is.EqualTo(match.ElementList));
            Assert.That(main.EndDate, Is.EqualTo(match.EndDate));
            Assert.That(main.EndDateString, Is.EqualTo(match.EndDateString));
            Assert.That(main.FastNavigation, Is.EqualTo(match.FastNavigation));
            Assert.That(main.Id, Is.EqualTo(match.Id));
            Assert.That(main.Label, Is.EqualTo(match.Label));
            Assert.That(main.Language, Is.EqualTo(match.Language));
            Assert.That(main.ManualSync, Is.EqualTo(match.ManualSync));
            Assert.That(main.MicrotingUId, Is.EqualTo(match.MicrotingUId));
            Assert.That(main.MultiApproval, Is.EqualTo(match.MultiApproval));
            Assert.That(main.PushMessageBody, Is.EqualTo(match.PushMessageBody));
            Assert.That(main.PushMessageTitle, Is.EqualTo(match.PushMessageTitle));
            Assert.That(main.Repeated, Is.EqualTo(match.Repeated));
            Assert.That(main.StartDate, Is.EqualTo(match.StartDate));
            Assert.That(main.StartDateString, Is.EqualTo(match.StartDateString));
            Assert.That(main, Is.EqualTo(match));

            #endregion
        }

        [Test]
        public async Task Core_Template_TemplateCreate_CreatesTemplate()
        {
            // Arrange
            CoreElement CElement = new CoreElement();
            //CElement.ElementList = new List<Element>();

            DateTime startDt = DateTime.UtcNow;
            DateTime endDt = DateTime.UtcNow;
            MainElement main = new MainElement(1, "label1", 0, "folderWithList", 1, startDt,
                endDt, "Swahili", false, true, true, true, "type1", "MessageTitle",
                "MessageBody", false, CElement.ElementList, "");
            // Act
            var match = await sut.TemplateCreate(main);
            // Assert

            Assert.NotNull(match);
            Assert.That(main.Id, Is.EqualTo(match));
        }

        [Test]
        public async Task Core_Template_TemplateRead_ReturnsTemplate()
        {
            // Arrange

            #region Tempalte

            DateTime cl1_ca = DateTime.UtcNow;
            DateTime cl1_ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            // Act
            Language language = DbContext.Languages.Single(x => x.Id == 1);
            var match = await sut.ReadeForm(cl1.Id, language);

            // Assert
            Assert.NotNull(match);
            Assert.That(cl1.Id, Is.EqualTo(match.Id));
            Assert.That(cl1.CaseType, Is.EqualTo(match.CaseType));
            Assert.That(false, Is.EqualTo(match.FastNavigation));
            Assert.That("A", Is.EqualTo(match.Label));
            Assert.That(false, Is.EqualTo(match.ManualSync));
            Assert.That(false, Is.EqualTo(match.MultiApproval));
            Assert.That(cl1.Repeated, Is.EqualTo(match.Repeated));
        }

        [Test]
        public async Task Core_Template_TemplateDelete_SetsWorkflowStateToRemoved()
        {
            // Arrange

            #region Tempalte1

            DateTime cl1_ca = DateTime.UtcNow;
            DateTime cl1_ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region Tempalte2

            DateTime cl2_ca = DateTime.UtcNow;
            DateTime cl2_ua = DateTime.UtcNow;
            CheckList cl2 =
                await testHelpers.CreateTemplate(cl2_ca, cl2_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region Tempalte3

            DateTime cl3_ca = DateTime.UtcNow;
            DateTime cl3_ua = DateTime.UtcNow;
            CheckList cl3 =
                await testHelpers.CreateTemplate(cl3_ca, cl3_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            #region Tempalte4

            DateTime cl4_ca = DateTime.UtcNow;
            DateTime cl4_ua = DateTime.UtcNow;
            CheckList cl4 =
                await testHelpers.CreateTemplate(cl4_ca, cl4_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            // Act
            var deleteTemplate1 = await sut.TemplateDelete(cl1.Id);
            var deleteTemplate2 = await sut.TemplateDelete(cl2.Id);
            var deleteTemplate3 = await sut.TemplateDelete(cl3.Id);
            var deleteTemplate4 = await sut.TemplateDelete(cl4.Id);
            // Assert
            Assert.NotNull(deleteTemplate1);
            Assert.NotNull(deleteTemplate2);
            Assert.NotNull(deleteTemplate3);
            Assert.NotNull(deleteTemplate4);
            Assert.That(deleteTemplate1, Is.True);
            Assert.That(deleteTemplate2, Is.True);
            Assert.That(deleteTemplate3, Is.True);
            Assert.That(deleteTemplate4, Is.True);
        }

        [Test]
        public async Task Core_Template_TemplateItemRead_ReadsTemplateItems()
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

            //tags tag = await testHelpers.CreateTag("Tag1", Constants.WorkflowStates.Created, 1);

            #endregion

            // Act

            var match1 = await sut.TemplateItemRead(Template1.Id, language);
            var match2 = await sut.TemplateItemRead(Template2.Id, language);
            var match3 = await sut.TemplateItemRead(Template3.Id, language);
            var match4 = await sut.TemplateItemRead(Template4.Id, language);


            // Assert

            #region template1

            Assert.NotNull(match1);
            Assert.That("Description1", Is.EqualTo(match1.Description));
            Assert.That("Label1", Is.EqualTo(match1.Label));
            Assert.That(Template1.CreatedAt.ToString(), Is.EqualTo(match1.CreatedAt.ToString()));
            Assert.That("FolderWithTemplate", Is.EqualTo(match1.FolderName));
            Assert.That(Template1.Id, Is.EqualTo(match1.Id));
//            Assert.AreEqual(match1.UpdatedAt.ToString(), Template1.UpdatedAt.ToString());

            #endregion

            #region template2

            Assert.NotNull(match1);
            Assert.That("Description2", Is.EqualTo(match2.Description));
            Assert.That("Label2", Is.EqualTo(match2.Label));
            Assert.That(Template2.CreatedAt.ToString(), Is.EqualTo(match2.CreatedAt.ToString()));
            Assert.That("FolderWithTemplate", Is.EqualTo(match2.FolderName));
            Assert.That(Template2.Id, Is.EqualTo(match2.Id));
//            Assert.AreEqual(match2.UpdatedAt.ToString(), Template2.UpdatedAt.ToString());

            #endregion

            #region template3

            Assert.NotNull(match1);
            Assert.That("Description3", Is.EqualTo(match3.Description));
            Assert.That("Label3", Is.EqualTo(match3.Label));
            Assert.That(Template3.CreatedAt.ToString(), Is.EqualTo(match3.CreatedAt.ToString()));
            Assert.That("FolderWithTemplate", Is.EqualTo(match3.FolderName));
            Assert.That(Template3.Id, Is.EqualTo(match3.Id));
//            Assert.AreEqual(match3.UpdatedAt.ToString(), Template3.UpdatedAt.ToString());

            #endregion

            #region template4

            Assert.NotNull(match1);
            Assert.That("Description4", Is.EqualTo(match4.Description));
            Assert.That("Label4", Is.EqualTo(match4.Label));
            Assert.That(Template4.CreatedAt.ToString(), Is.EqualTo(match4.CreatedAt.ToString()));
            Assert.That("FolderWithTemplate", Is.EqualTo(match4.FolderName));
            Assert.That(Template4.Id, Is.EqualTo(match4.Id));
//            Assert.AreEqual(match4.UpdatedAt.ToString(), Template4.UpdatedAt.ToString());

            #endregion
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