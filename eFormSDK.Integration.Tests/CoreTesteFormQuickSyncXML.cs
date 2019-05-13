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
    public class CoreTesteFormQuickSyncXML : DbTestFixture
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
            sut.SetSdkSetting(Settings.fileLocationPicture, path + @"\output\dataFolder\picture\");
            sut.SetSdkSetting(Settings.fileLocationPdf, path + @"\output\dataFolder\pdf\");
            sut.SetSdkSetting(Settings.fileLocationJasper, path + @"\output\dataFolder\reports\");
            testHelpers = new TestHelpers();
            //sut.StartLog(new CoreBase());
        }

        [Test] // Core_Template_TemplateFromXml_ReturnsTemplate()
        public void Core_eForm_QuickSyncEnabledeFormFromXML_ReturnseMainElement()
        {
            // Arrange
            string xmlstring = @"
                <?xml version='1.0' encoding='utf-8'?>
                <Main xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                  <Id>35</Id>
                  <Label>Lorem ipsum</Label>
                  <DisplayOrder>0</DisplayOrder>
                  <CheckListFolderName>Modtagerkontrol</CheckListFolderName>
                  <Repeated>1</Repeated>
                  <StartDate>2018-08-23 10:40:52</StartDate>
                  <EndDate>2058-08-29 10:40:52</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <DownloadEntities>false</DownloadEntities>
                  <ManualSync>false</ManualSync>
                  <EnableQuickSync>true</EnableQuickSync>
                  <ElementList>
                    <Element xsi:type='DataElement'>
                      <Id>36</Id>
                      <Label>Lorem ipsum</Label>
                      <DisplayOrder>0</DisplayOrder>
                      <Description><![CDATA[Lorem ipsum description]]></Description>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <ReviewEnabled>false</ReviewEnabled>
                      <DoneButtonEnabled>true</DoneButtonEnabled>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <PinkBarText />
                      <DataItemGroupList />
                      <DataItemList>
                        <DataItem xsi:type='SingleSelect'>
                          <Id>22</Id>
                          <Mandatory>false</Mandatory>
                          <ReadOnly>false</ReadOnly>
                          <Label>Is everything OK:</Label>
                          <Description><![CDATA[]]></Description>
                          <DisplayOrder>1</DisplayOrder>
                          <KeyValuePairList>
                            <KeyValuePair>
                              <Key>1</Key>
                              <Value>OK</Value>
                              <Selected>false</Selected>
                              <DisplayOrder>1</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>2</Key>
                              <Value>Not OK</Value>
                              <Selected>false</Selected>
                              <DisplayOrder>2</DisplayOrder>
                            </KeyValuePair>
                          </KeyValuePairList>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                  <PushMessageTitle />
                  <PushMessageBody />
                </Main>";
            // Act
            MainElement match = sut.TemplateFromXml(xmlstring);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual("", match.CaseType);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual(35, match.Id);
            Assert.AreEqual("Lorem ipsum", match.Label);
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            Assert.AreEqual(0, match.DisplayOrder);
            Assert.AreEqual(1, match.ElementList.Count());
            Assert.AreEqual(true, match.EnableQuickSync);

            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(dE.DataItemList.Count(), 1);
            Assert.AreEqual("Lorem ipsum", dE.Label);

            CDataValue cd = new CDataValue();

            // Assert.AreEqual(dE.Description, cd); TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(dE.ManualSync) //TODO No Method for ManualSync 
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO DoneButtonDisabled no method found
            Assert.AreEqual(false, dE.ApprovalEnabled);

            SingleSelect commentField = (SingleSelect)dE.DataItemList[0];
            Assert.AreEqual("Is everything OK:", commentField.Label);
            // Assert.AreEqual(commentField.Description, cd);
            Assert.AreEqual(1, commentField.DisplayOrder);
            // Assert.AreEqual(commentField.Multi, 0) //TODO No method MULTI
            // Assert.AreEqual(commentField.geolocation, false) //TODO no method geolocation
            // Assert.AreEqual(commentField.Split, false) //TODO no method Split
            // Assert.AreEqual("", commentField.Value);
            Assert.AreEqual(false, commentField.ReadOnly);
            Assert.AreEqual(false, commentField.Mandatory);
            // Assert.AreEqual(Constants.FieldColors.Grey, commentField.Color);


        }


        [Test]
        public void Core_Template_TemplateRead_ReturnsTemplateWithQuickSync()
        {
            // Arrange
            #region Tempalte

            DateTime cl1_ca = DateTime.Now;
            DateTime cl1_ua = DateTime.Now;
            check_lists cl1 = testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);
            //cl1.quick_sync_enabled = 1;
            check_lists cl_ud = DbContext.check_lists.Single(x => x.Id == cl1.Id);
            //DbContext.check_lists.Add(cl1);
            cl_ud.QuickSyncEnabled = 1;
            DbContext.SaveChanges();

            #endregion

            // Act
            MainElement match = sut.TemplateRead(cl1.Id);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match.Id, cl1.Id);
            Assert.AreEqual(match.CaseType, cl1.CaseType);
            Assert.AreEqual(match.FastNavigation, false);
            Assert.AreEqual(match.Label, cl1.Label);
            Assert.AreEqual(match.ManualSync, false);
            Assert.AreEqual(match.MultiApproval, false);
            Assert.AreEqual(match.Repeated, cl1.Repeated);
            Assert.AreEqual(match.EnableQuickSync, true);


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
