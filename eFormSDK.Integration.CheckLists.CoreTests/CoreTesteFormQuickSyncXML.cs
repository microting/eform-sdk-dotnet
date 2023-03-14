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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;

namespace eFormSDK.Integration.CheckLists.CoreTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class CoreTesteFormQuickSyncXML : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");

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
        }

        [Test] // Core_Template_TemplateFromXml_ReturnsTemplate()
        public async Task Core_eForm_QuickSyncEnabledeFormFromXML_ReturnseMainElement()
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
            MainElement match = await sut.TemplateFromXml(xmlstring);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual("", match.CaseType);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("35", match.OriginalId);
            Assert.AreEqual(0, match.Id);
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
        public async Task Core_Template_TemplateRead_ReturnsTemplateWithQuickSync()
        {
            // Arrange

            #region Tempalte

            DateTime cl1_ca = DateTime.UtcNow;
            DateTime cl1_ua = DateTime.UtcNow;
            CheckList cl1 =
                await testHelpers.CreateTemplate(cl1_ca, cl1_ua, "A", "D", "CheckList", "Template1FolderName", 1, 1);
            //cl1.quick_sync_enabled = 1;
            CheckList cl_ud = await DbContext.CheckLists.FirstAsync(x => x.Id == cl1.Id);
            //DbContext.check_lists.Add(cl1);
            cl_ud.QuickSyncEnabled = 1;
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            #endregion

            // Act
            Language language = DbContext.Languages.Single(x => x.Id == 1);
            MainElement match = await sut.ReadeForm(cl1.Id, language);

            // Assert
            Assert.NotNull(match);
            Assert.AreEqual(match.Id, cl1.Id);
            Assert.AreEqual(match.CaseType, cl1.CaseType);
            Assert.AreEqual(match.FastNavigation, false);
            Assert.AreEqual(match.Label, "A");
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