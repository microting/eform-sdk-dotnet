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
    public class CoreTesteForm : DbTestFixture
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

        #region template
        [Test]
        public void Core_Template_TemplateFromXml_REturnsTemplate()
        {
            //Arrance
            string xmlstring = @"<?xml version='1.0' encoding='UTF-8' ?>
   <Main>
     <Id> 9060 </Id>
        <Repeated> 0 </Repeated>
        <Label> comment </Label>
        <StartDate> 2017 - 07 - 07 </StartDate>
        <EndDate> 2027 - 07 - 07 </EndDate>
        <Language> da </Language>
        <MultiApproval> false </MultiApproval>
        <FastNavigation> false </FastNavigation>
        <Review> false </Review>
        <Summary> false </Summary>
        <DisplayOrder> 0 </DisplayOrder>
        <ElementList>
          <Element type = 'DataElement'>
            <Id> 9060 </Id>
            <Label> comment </Label>
            <Description><![CDATA[]]></Description>
            <DisplayOrder> 0 </DisplayOrder>
            <ReviewEnabled> false </ReviewEnabled>
            <ManualSync> false </ManualSync>
            <ExtraFieldsEnabled> false </ExtraFieldsEnabled>
            <DoneButtonDisabled> false </DoneButtonDisabled>
            <ApprovalEnabled> false </ApprovalEnabled>
            <DataItemList>
              <DataItem type = 'Comment'>
                <Id> 73660 </Id>
                <Label> Comment </Label>
                 <Description><![CDATA[]]></Description>
                 <DisplayOrder> 0 </DisplayOrder>
                 <Multi> 1 </Multi>
                 <GeolocationEnabled> false </GeolocationEnabled>
                 <Split> false </Split>
                 <Value/>
                 <ReadOnly> false </ReadOnly>
                  <Mandatory> false </Mandatory>
                  <Color> e8eaf6 </Color>
                </DataItem>
              </DataItemList>
            </Element>
          </ElementList>
    </Main>";
            //Act
            var match = sut.TemplateFromXml(xmlstring);

            //Assert
            Assert.NotNull(match);
            Assert.AreEqual(match.CaseType, "");
            Assert.AreEqual(match.Repeated, 1);
            Assert.AreEqual(match.Id, 1);
            Assert.AreEqual(match.Label, " comment ");
            Assert.AreEqual(match.Language, " da ");
            Assert.AreEqual(match.MultiApproval, false);
            Assert.AreEqual(match.FastNavigation, false);
            Assert.AreEqual(match.DisplayOrder, 0);

        }
        [Test]
        public void Core_Template_TemplateItemReadAll_DoesReturnSortedTemplates()
        {
            // Arrance

            #region Template1
            check_lists cl1 = new check_lists();
            cl1.created_at = DateTime.Now;
            cl1.updated_at = DateTime.Now;
            cl1.label = "A";
            cl1.description = "D";
            cl1.workflow_state = Constants.WorkflowStates.Created;
            cl1.case_type = "CheckList";
            cl1.folder_name = "Template1FolderName";
            cl1.display_index = 1;
            cl1.repeated = 1;

            DbContext.check_lists.Add(cl1);
            DbContext.SaveChanges();
            Thread.Sleep(1000);
            #endregion

            #region Template2
            check_lists cl2 = new check_lists();
            cl2.created_at = DateTime.Now;
            cl2.updated_at = DateTime.Now;
            cl2.label = "B";
            cl2.description = "C";
            cl2.workflow_state = Constants.WorkflowStates.Removed;
            cl2.case_type = "CheckList";
            cl2.folder_name = "Template1FolderName";
            cl2.display_index = 1;
            cl2.repeated = 1;

            DbContext.check_lists.Add(cl2);
            DbContext.SaveChanges();
            Thread.Sleep(1000);
            #endregion

            #region Template3
            check_lists cl3 = new check_lists();
            cl3.created_at = DateTime.Now;
            cl3.updated_at = DateTime.Now;
            cl3.label = "D";
            cl3.description = "B";
            cl3.workflow_state = Constants.WorkflowStates.Created;
            cl3.case_type = "CheckList";
            cl3.folder_name = "Template1FolderName";
            cl3.display_index = 1;
            cl3.repeated = 1;

            DbContext.check_lists.Add(cl3);
            DbContext.SaveChanges();
            Thread.Sleep(1000);
            #endregion

            #region Template4
            check_lists cl4 = new check_lists();
            cl4.created_at = DateTime.Now;
            cl4.updated_at = DateTime.Now;
            cl4.label = "C";
            cl4.description = "A";
            cl4.workflow_state = Constants.WorkflowStates.Created;
            cl4.case_type = "CheckList";
            cl4.folder_name = "Template1FolderName";
            cl4.display_index = 1;
            cl4.repeated = 1;

            DbContext.check_lists.Add(cl4);
            DbContext.SaveChanges();
            #endregion


            // Act
            List<int> emptyList = new List<int>();

            // Default sorting including removed
            List<Template_Dto> templateListId = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", false, "", emptyList);
            List<Template_Dto> templateListLabel = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.Label, emptyList);
            List<Template_Dto> templateListDescription = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.Description, emptyList);
            List<Template_Dto> templateListCreatedAt = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.CreatedAt, emptyList);

            // Descending including removed
            List<Template_Dto> templateListDescengingId = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", true, "", emptyList);
            List<Template_Dto> templateListDescengingLabel = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.Label, emptyList);
            List<Template_Dto> templateListDescengingDescription = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.Description, emptyList);
            List<Template_Dto> templateListDescengingCreatedAt = sut.TemplateItemReadAll(true, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.CreatedAt, emptyList);

            // Default sorting excluding removed
            List<Template_Dto> templateListIdNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", false, "", emptyList);
            List<Template_Dto> templateListLabelNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.Label, emptyList);
            List<Template_Dto> templateListDescriptionNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.Description, emptyList);
            List<Template_Dto> templateListCreatedAtNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", false, Constants.TamplateSortParameters.CreatedAt, emptyList);

            // Descending excluding removed
            List<Template_Dto> templateListDescengingIdNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", true, "", emptyList);
            List<Template_Dto> templateListDescengingLabelNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.Label, emptyList);
            List<Template_Dto> templateListDescengingDescriptionNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.Description, emptyList);
            List<Template_Dto> templateListDescengingCreatedAtNr = sut.TemplateItemReadAll(false, Constants.WorkflowStates.Created, "", true, Constants.TamplateSortParameters.CreatedAt, emptyList);


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
        public void Core_Template_TemplateValidation_ReturnsErrorLst()
        {
            //Arrance
            CoreElement CElement = new CoreElement();
            //CElement.ElementList = new List<Element>();

            DateTime startDt = DateTime.Now;
            DateTime endDt = DateTime.Now;
            MainElement main = new MainElement(1, "label1", 4, "folderWithList", 1, startDt,
                endDt, "Swahili", false, true, false, true, "type1", "MessageTitle",
                "MessageBody", CElement.ElementList);
            //Act
            var match = sut.TemplateValidation(main);
            //Assert
            Assert.NotNull(match);
            Assert.AreEqual(match.Count(), 0);
        }
        [Test]
        public void Core_Template_TemplateUploadData_ReturnsmainElement()
        {
            //Arrance
            CoreElement CElement = new CoreElement();
            //CElement.ElementList = new List<Element>();

            DateTime startDt = DateTime.Now;
            DateTime endDt = DateTime.Now;
            MainElement main = new MainElement(1, "label1", 0, "folderWithList", 1, startDt,
                endDt, "Swahili", false, true, true, true, "type1", "MessageTitle",
                "MessageBody", CElement.ElementList);
            //Act
            var match = sut.TemplateUploadData(main);
            //Assert
            #region Assert
            Assert.NotNull(match);
            Assert.AreEqual(match.CaseType, main.CaseType);
            Assert.AreEqual(match.CheckListFolderName, main.CheckListFolderName);
            Assert.AreEqual(match.DisplayOrder, main.DisplayOrder);
            Assert.AreEqual(match.DownloadEntities, main.DownloadEntities);
            Assert.AreEqual(match.ElementList, main.ElementList);
            Assert.AreEqual(match.EndDate, main.EndDate);
            Assert.AreEqual(match.EndDateString, main.EndDateString);
            Assert.AreEqual(match.FastNavigation, main.FastNavigation);
            Assert.AreEqual(match.Id, main.Id);
            Assert.AreEqual(match.Label, main.Label);
            Assert.AreEqual(match.Language, main.Language);
            Assert.AreEqual(match.ManualSync, main.ManualSync);
            Assert.AreEqual(match.MicrotingUId, main.MicrotingUId);
            Assert.AreEqual(match.MultiApproval, main.MultiApproval);
            Assert.AreEqual(match.PushMessageBody, main.PushMessageBody);
            Assert.AreEqual(match.PushMessageTitle, main.PushMessageTitle);
            Assert.AreEqual(match.Repeated, main.Repeated);
            Assert.AreEqual(match.StartDate, main.StartDate);
            Assert.AreEqual(match.StartDateString, main.StartDateString);
            Assert.AreEqual(match, main);
            #endregion
        }
        [Test]
        public void Core_Template_TemplateCreate_CreatesTemplate()
        {

            //Arrance
            CoreElement CElement = new CoreElement();
            //CElement.ElementList = new List<Element>();

            DateTime startDt = DateTime.Now;
            DateTime endDt = DateTime.Now;
            MainElement main = new MainElement(1, "label1", 0, "folderWithList", 1, startDt,
                endDt, "Swahili", false, true, true, true, "type1", "MessageTitle",
                "MessageBody", CElement.ElementList);
            //Act
            var match = sut.TemplateCreate(main);
            //Assert

            Assert.NotNull(match);
            Assert.AreEqual(match, main.Id);

        }
        [Test]
        public void Core_Template_TemplateRead_ReturnsTemplate()
        {
            //Arrance
            #region Tempalte

            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion

            //Act
            var match = sut.TemplateRead(cl1.id);

            //Assert
            Assert.NotNull(match);
            Assert.AreEqual(match.Id, cl1.id);
            Assert.AreEqual(match.CaseType, cl1.case_type);
            Assert.AreEqual(match.FastNavigation, false);
            Assert.AreEqual(match.Label, cl1.label);
            Assert.AreEqual(match.ManualSync, false);
            Assert.AreEqual(match.MultiApproval, false);
            Assert.AreEqual(match.Repeated, cl1.repeated);


        }
        [Test]
        public void Core_Template_TemplateDelete_SetsWorkflowStateToRemoved()
        {
            //Arrance
            #region Tempalte1

            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion
            #region Tempalte2

            DateTime cl2_ca = DateTime.Now;
            DateTime cl2_ua = DateTime.Now;
            check_lists cl2 = testHelpers.CreateTemplate(cl2_ca, cl2_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion
            #region Tempalte3

            DateTime cl3_ca = DateTime.Now;
            DateTime cl3_ua = DateTime.Now;
            check_lists cl3 = testHelpers.CreateTemplate(cl3_ca, cl3_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion
            #region Tempalte4

            DateTime cl4_ca = DateTime.Now;
            DateTime cl4_ua = DateTime.Now;
            check_lists cl4 = testHelpers.CreateTemplate(cl4_ca, cl4_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);

            #endregion
            //Act
            var deleteTemplate1 = sut.TemplateDelete(cl1.id);
            var deleteTemplate2 = sut.TemplateDelete(cl2.id);
            var deleteTemplate3 = sut.TemplateDelete(cl3.id);
            var deleteTemplate4 = sut.TemplateDelete(cl4.id);
            //Assert
            Assert.NotNull(deleteTemplate1);
            Assert.NotNull(deleteTemplate2);
            Assert.NotNull(deleteTemplate3);
            Assert.NotNull(deleteTemplate4);
            Assert.True(deleteTemplate1);
            Assert.True(deleteTemplate2);
            Assert.True(deleteTemplate3);
            Assert.True(deleteTemplate4);

        }
        [Test]
        public void Core_Template_TemplateItemRead_ReadsTemplateItems()
        {
            // Arrance
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

            #region Tag

            tags tag = testHelpers.CreateTag("Tag1", Constants.WorkflowStates.Created, 1);

            #endregion
            // Act

            var match1 = sut.TemplateItemRead(Template1.id);
            var match2 = sut.TemplateItemRead(Template2.id);
            var match3 = sut.TemplateItemRead(Template3.id);
            var match4 = sut.TemplateItemRead(Template4.id);


            // Assert
            #region template1
            Assert.NotNull(match1);
            Assert.AreEqual(match1.Description, "Description1");
            Assert.AreEqual(match1.Label, "Label1");
            Assert.AreEqual(match1.CreatedAt.ToString(), Template1.created_at.ToString());
            Assert.AreEqual(match1.FolderName, "FolderWithTemplate");
            Assert.AreEqual(match1.Id, Template1.id);
            Assert.AreEqual(match1.UpdatedAt.ToString(), Template1.updated_at.ToString());
            #endregion

            #region template2
            Assert.NotNull(match1);
            Assert.AreEqual(match2.Description, "Description2");
            Assert.AreEqual(match2.Label, "Label2");
            Assert.AreEqual(match2.CreatedAt.ToString(), Template2.created_at.ToString());
            Assert.AreEqual(match2.FolderName, "FolderWithTemplate");
            Assert.AreEqual(match2.Id, Template2.id);
            Assert.AreEqual(match2.UpdatedAt.ToString(), Template2.updated_at.ToString());
            #endregion

            #region template3
            Assert.NotNull(match1);
            Assert.AreEqual(match3.Description, "Description3");
            Assert.AreEqual(match3.Label, "Label3");
            Assert.AreEqual(match3.CreatedAt.ToString(), Template3.created_at.ToString());
            Assert.AreEqual(match3.FolderName, "FolderWithTemplate");
            Assert.AreEqual(match3.Id, Template3.id);
            Assert.AreEqual(match3.UpdatedAt.ToString(), Template3.updated_at.ToString());
            #endregion

            #region template4
            Assert.NotNull(match1);
            Assert.AreEqual(match4.Description, "Description4");
            Assert.AreEqual(match4.Label, "Label4");
            Assert.AreEqual(match4.CreatedAt.ToString(), Template4.created_at.ToString());
            Assert.AreEqual(match4.FolderName, "FolderWithTemplate");
            Assert.AreEqual(match4.Id, Template4.id);
            Assert.AreEqual(match4.UpdatedAt.ToString(), Template4.updated_at.ToString());
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