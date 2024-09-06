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
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Helpers;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;

namespace eFormSDK.Integration.CheckLists.CoreTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class CoreTesteFormFromXML : DbTestFixture
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
        }

        [Test] // Core_Template_TemplateFromXml_ReturnsTemplate()
        public async Task Core_eForm_SimpleCommenteFormFromXML_ReturnseMainElement()
        {
            // Arrange
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                    <Id>9060</Id>
                    <Repeated>0</Repeated>
                    <Label>CommentMain</Label>
                    <StartDate>2017-07-07</StartDate>
                    <EndDate>2027-07-07</EndDate>
                    <Language>da</Language>
                    <MultiApproval>false</MultiApproval>
                    <FastNavigation>false</FastNavigation>
                    <Review>false</Review>
                    <Summary>false</Summary>
                    <DisplayOrder>0</DisplayOrder>
                    <ElementList>
                        <Element type='DataElement'>
                            <Id>9060</Id>
                            <Label>CommentDataElement</Label>
                            <Description><![CDATA[CommentDataElementDescription]]></Description>
                            <DisplayOrder>0</DisplayOrder>
                            <ReviewEnabled>false</ReviewEnabled>
                            <ManualSync>false</ManualSync>
                            <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                            <DoneButtonDisabled>false</DoneButtonDisabled>
                            <ApprovalEnabled>false</ApprovalEnabled>
                            <DataItemList>
                                <DataItem type='Comment'>
                                    <Id>73660</Id>
                                    <Label>CommentField</Label>
                                    <Description><![CDATA[CommentFieldDescription]]></Description>
                                    <DisplayOrder>0</DisplayOrder>
                                    <Multi>1</Multi>
                                    <GeolocationEnabled>false</GeolocationEnabled>
                                    <Split>false</Split>
                                    <Value />
                                    <ReadOnly>false</ReadOnly>
                                    <Mandatory>false</Mandatory>
                                    <Color>e8eaf6</Color>
                                </DataItem>
                            </DataItemList>
                        </Element>
                    </ElementList>
                </Main>";
            // Act
            var match = await sut.TemplateFromXml(xmlstring);

            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.CaseType, Is.EqualTo(""));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.OriginalId, Is.EqualTo("9060"));
            Assert.That(match.Id, Is.EqualTo(0));
            Assert.That(match.Label, Is.EqualTo("CommentMain"));
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            Assert.That(match.DisplayOrder, Is.EqualTo(0));
            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(1, Is.EqualTo(dE.DataItemList.Count()));
            Assert.That(dE.Label, Is.EqualTo("CommentDataElement"));

            CDataValue cd = new CDataValue();

            // Assert.AreEqual(dE.Description, cd); TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.ManualSync) //TODO No Method for ManualSync
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO DoneButtonDisabled no method found
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            Comment commentField = (Comment)dE.DataItemList[0];
            Assert.That(commentField.Label, Is.EqualTo("CommentField"));
            // Assert.AreEqual(commentField.Description, cd);
            Assert.That(commentField.DisplayOrder, Is.EqualTo(0));
            // Assert.AreEqual(commentField.Multi, 0) //TODO No method MULTI
            // Assert.AreEqual(commentField.geolocation, false) //TODO no method geolocation
            // Assert.AreEqual(commentField.Split, false) //TODO no method Split
            Assert.That(commentField.Value, Is.EqualTo(""));
            Assert.That(commentField.ReadOnly, Is.EqualTo(false));
            Assert.That(commentField.Mandatory, Is.EqualTo(false));
            Assert.That(commentField.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test]
        public async Task Core_eForm_SimplePictureFormFromXML_ReturnseMainElement()
        {
            // Arrange
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>138708</Id>
                  <Repeated>0</Repeated>
                  <Label>Picture test</Label>
                  <StartDate>2018-04-25</StartDate>
                  <EndDate>2028-04-25</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>138708</Id>
                      <Label>Picture test</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='Picture'>
                          <Id>343753</Id>
                          <Label>Take two pictures</Label>
                          <Description><![CDATA[]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Mandatory>false</Mandatory>
                          <Color>e8eaf6</Color>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";

            // Act
            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("Picture test"));
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(1));
            Assert.That(dE.Label, Is.EqualTo("Picture test"));
            // Assert.AreEqual(dE.DisplayOrder, CDataValue); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            Picture pictureField = (Picture)dE.DataItemList[0];
            Assert.That(pictureField.Label, Is.EqualTo("Take two pictures"));
            // Assert.AreEqual(pictureField.Description, CDataValue) //TODO
            Assert.That(pictureField.DisplayOrder, Is.EqualTo(0));
            Assert.That(pictureField.Mandatory, Is.EqualTo(false));
            Assert.That(pictureField.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test]
        public async Task Core_eForm_SimpleDateFormFromXML_ReturnseMainElement()
        {
            // Arrange
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>76348</Id>
                  <Repeated>0</Repeated>
                  <Label>Date</Label>
                  <StartDate>2018-01-18</StartDate>
                  <EndDate>2028-01-18</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>76348</Id>
                      <Label>Date</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='Date'>
                          <Id>216438</Id>
                          <Label>Select date</Label>
                          <Description><![CDATA[]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <MinValue>2018-01-18</MinValue>
                          <MaxValue>2028-01-18</MaxValue>
                          <Value/>
                          <Mandatory>false</Mandatory>
                          <ReadOnly>false</ReadOnly>
                          <Color>e8eaf6</Color>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";


            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("Date"));
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(1));
            Assert.That(dE.Label, Is.EqualTo("Date"));
            // Assert.AreEqual(dE.DisplayOrder, CDataValue); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            Date dateField = (Date)dE.DataItemList[0];
            Assert.That(dateField.Label, Is.EqualTo("Select date"));
            // Assert.AreEqual(dateField.Description, CDataValue) //TODO
            Assert.That(dateField.DisplayOrder, Is.EqualTo(0));
            // Assert.AreEqual("2018-04-25 00:00:00", dateField.MinValue); //TODO
            // Assert.AreEqual("2028-04-25", dateField.MaxValue); //TODO
            Assert.That(dateField.Mandatory, Is.EqualTo(false));
            Assert.That(dateField.ReadOnly, Is.EqualTo(false));
            Assert.That(dateField.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test]
        public async Task Core_eForm_SimplePdfFormFromXML_ReturnseMainElement()
        {
            // Arrange
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>9085</Id>
                  <Repeated>0</Repeated>
                  <Label>ny pdf</Label>
                  <StartDate>2017-08-04</StartDate>
                  <EndDate>2027-08-04</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>9085</Id>
                      <Label>ny pdf</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='ShowPDF'>
                          <Id>73835</Id>
                          <Label>bla</Label>
                          <Description><![CDATA[]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Color>e8eaf6</Color>
                          <Value>https://eform.microting.com/app_files/uploads/20170804132716_13790_20d483dd7791cd6becf089432724c663.pdf</Value>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";


            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("ny pdf"));
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(1));
            Assert.That(dE.Label, Is.EqualTo("ny pdf"));
            // Assert.AreEqual(dE.DisplayOrder, CDataValue); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            ShowPdf showPDFField = (ShowPdf)dE.DataItemList[0];
            Assert.That(showPDFField.Label, Is.EqualTo("bla"));
            // Assert.AreEqual(dateField.Description, CDataValue) //TODO
            Assert.That(showPDFField.DisplayOrder, Is.EqualTo(0));
            Assert.That(showPDFField.Color, Is.EqualTo(Constants.FieldColors.Default));
            Assert.That(
                showPDFField.Value,
                Is.EqualTo("https://eform.microting.com/app_files/uploads/20170804132716_13790_20d483dd7791cd6becf089432724c663.pdf"));
        }

        [Test]
        public async Task Core_eForm_SimpleFieldGroupsFormFromXML_ReturnseMainElement()
        {
            // Arrange
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>76198</Id>
                  <Repeated>0</Repeated>
                  <Label>Tester grupper</Label>
                  <StartDate>2017-12-06</StartDate>
                  <EndDate>2027-12-06</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>76198</Id>
                      <Label>Tester grupper</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemGroupList>
                        <DataItemGroup type='FieldGroup'>
                          <Id>215438</Id>
                          <Label>Gruppe efter tjekboks</Label>
                          <Description><![CDATA[]]></Description>
                          <DisplayOrder>1</DisplayOrder>
                          <Value>Closed</Value>
                          <Color>e8eaf6</Color>
                          <DataItemList>
                            <DataItem type='CheckBox'>
                              <Id>215443</Id>
                              <Label>Tjekboks inde i gruppe</Label>
                              <Description><![CDATA[]]></Description>
                              <DisplayOrder>0</DisplayOrder>
                              <Selected>false</Selected>
                              <Mandatory>false</Mandatory>
                              <Color>e8eaf6</Color>
                            </DataItem>
                          </DataItemList>
                        </DataItemGroup>
                      </DataItemGroupList>
                      <DataItemList>
                        <DataItem type='CheckBox'>
                          <Id>215433</Id>
                          <Label>Tjekboks før gruppe</Label>
                          <Description><![CDATA[]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Selected>false</Selected>
                          <Mandatory>false</Mandatory>
                          <Color>e8eaf6</Color>
                        </DataItem>
                        <DataItem type='CheckBox'>
                          <Id>215448</Id>
                          <Label>Tjekboks efter gruppe</Label>
                          <Description><![CDATA[]]></Description>
                          <DisplayOrder>2</DisplayOrder>
                          <Selected>false</Selected>
                          <Mandatory>false</Mandatory>
                          <Color>e8eaf6</Color>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";


            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("Tester grupper"));
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));


            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(3));
            Assert.That(dE.Label, Is.EqualTo("Tester grupper"));
            // Assert.AreEqual(dE.DisplayOrder, CDataValue); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));


            FieldContainer fC = (FieldContainer)dE.DataItemList[0];
            Assert.That(fC.Label, Is.EqualTo("Gruppe efter tjekboks"));
            // Assert.AreEqual(CDataValue, fE.Description); TODO
            Assert.That(fC.DisplayOrder, Is.EqualTo(1));
            Assert.That(fC.Value, Is.EqualTo("Closed"));
            Assert.That(fC.Color, Is.EqualTo(Constants.FieldColors.Default));

            CheckBox fE = (CheckBox)fC.DataItemList[0];
            Assert.That(fE.Label, Is.EqualTo("Tjekboks inde i gruppe"));
            // Assert.AreEqual(dateField.Description, CDataValue) //TODO
            Assert.That(fE.DisplayOrder, Is.EqualTo(0));
            Assert.That(fE.Selected, Is.EqualTo(false));
            Assert.That(fE.Mandatory, Is.EqualTo(false));
            Assert.That(fE.Color, Is.EqualTo(Constants.FieldColors.Default));


            CheckBox checkboxField = (CheckBox)dE.DataItemList[1];
            Assert.That(checkboxField.Label, Is.EqualTo("Tjekboks før gruppe"));
            // Assert.AreEqual(dateField.Description, CDataValue) //TODO
            Assert.That(checkboxField.DisplayOrder, Is.EqualTo(0));
            Assert.That(checkboxField.Selected, Is.EqualTo(false));
            Assert.That(checkboxField.Mandatory, Is.EqualTo(false));
            Assert.That(checkboxField.Color, Is.EqualTo(Constants.FieldColors.Default));


            CheckBox checkboxField1 = (CheckBox)dE.DataItemList[2];
            Assert.That(checkboxField1.Label, Is.EqualTo("Tjekboks efter gruppe"));
            // Assert.AreEqual(dateField.Description, CDataValue) //TODO
            Assert.That(checkboxField1.DisplayOrder, Is.EqualTo(2));
            Assert.That(checkboxField1.Selected, Is.EqualTo(false));
            Assert.That(checkboxField1.Mandatory, Is.EqualTo(false));
            Assert.That(checkboxField1.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test]
        public async Task Core_eForm_SimplePictureAndSignatureFormFromXML_ReturnseMainElement()
        {
            // Arrange
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>9114</Id>
                  <Repeated>0</Repeated>
                  <Label>Billede og signatur</Label>
                  <StartDate>2017-08-07</StartDate>
                  <EndDate>2027-08-07</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>9114</Id>
                      <Label>Billede og signatur</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='Picture'>
                          <Id>73879</Id>
                          <Label>Tag et billede</Label>
                          <Description><![CDATA[]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Mandatory>false</Mandatory>
                          <Color>e8eaf6</Color>
                        </DataItem>
                        <DataItem type='Signature'>
                          <Id>74257</Id>
                          <Label>Skriv</Label>
                          <Description><![CDATA[Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam dui neque, molestie at maximus a, malesuada at mi. Cras venenatis porttitor augue nec sagittis. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris urna massa, sagittis at fringilla ut, convallis sed dolor. Praesent scelerisque magna dolor, quis blandit metus pharetra eu. Cras euismod facilisis risus at ullamcorper. Pellentesque vitae maximus elit. Sed scelerisque nec velit dictum sodales. Duis sed dapibus odio. Sed non luctus sem. Donec eu mollis lectus, nec porta nisl. Aenean a consequat metus, ac auctor arcu. Cras sit amet blandit velit. Pellentesque faucibus eros sed ullamcorper rutrum.<br><br><br>Pellentesque ultrices ex erat. Pellentesque rhoncus eget lectus et scelerisque. Cras vitae diam ex. Ut felis ligula, venenatis ut lorem vel, venenatis convallis turpis. Sed rutrum ac odio ac auctor. Sed mauris ipsum, vulputate ut sodales a, mattis et purus. Nam convallis augue velit, nec blandit ipsum porta vitae. Quisque et iaculis lectus. Donec eu fringilla turpis, id rutrum mauris.<br><br><br>Proin eu sagittis sem. Aenean vel placerat sapien. Praesent et rutrum justo. Mauris consectetur venenatis est, eu vulputate enim elementum eget. In hac habitasse platea dictumst. Sed vehicula nec neque sed posuere. Aenean sodales lectus a purus posuere lacinia. Aenean ut enim vel odio varius placerat. Phasellus faucibus turpis sed arcu ultrices interdum. Sed porta, nisi nec vehicula lacinia, ante tortor tristique justo, vel sagittis felis ligula eu magna. Pellentesque a velit laoreet nunc aliquet ornare sit amet eget lorem. Duis aliquet viverra pretium. Etiam a mauris tellus. Sed viverra eros eget lectus lobortis, in vestibulum lorem rhoncus. Aliquam sem felis, suscipit a gravida ut, eleifend et ipsum. Nullam lacus lacus, rutrum quis sollicitudin et, porta et erat.]]></Description>
                          <DisplayOrder>1</DisplayOrder>
                          <Mandatory>false</Mandatory>
                          <Color>e8eaf6</Color>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";


            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("Billede og signatur"));
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));


            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(2));
            Assert.That(dE.Label, Is.EqualTo("Billede og signatur"));
            // Assert.AreEqual(CD.Datavalue, de.description); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            Picture fE = (Picture)dE.DataItemList[0];
            Assert.That(fE.Label, Is.EqualTo("Tag et billede"));
            // Assert.AreEqual(cD.Datavalue, fe.description) //TODO
            Assert.That(fE.DisplayOrder, Is.EqualTo(0));
            Assert.That(fE.Mandatory, Is.EqualTo(false));
            Assert.That(fE.Color, Is.EqualTo(Constants.FieldColors.Default));

            Signature fE1 = (Signature)dE.DataItemList[1];
            Assert.That(fE1.Label, Is.EqualTo("Skriv"));
            //TODO Statement below -> CD.Datavalue
            // Assert.AreEqual("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam dui neque, molestie at maximus a, malesuada at mi. Cras venenatis porttitor augue nec sagittis. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris urna massa, sagittis at fringilla ut, convallis sed dolor. Praesent scelerisque magna dolor, quis blandit metus pharetra eu. Cras euismod facilisis risus at ullamcorper. Pellentesque vitae maximus elit. Sed scelerisque nec velit dictum sodales. Duis sed dapibus odio. Sed non luctus sem. Donec eu mollis lectus, nec porta nisl. Aenean a consequat metus, ac auctor arcu. Cras sit amet blandit velit. Pellentesque faucibus eros sed ullamcorper rutrum.<br><br><br>Pellentesque ultrices ex erat. Pellentesque rhoncus eget lectus et scelerisque. Cras vitae diam ex. Ut felis ligula, venenatis ut lorem vel, venenatis convallis turpis. Sed rutrum ac odio ac auctor. Sed mauris ipsum, vulputate ut sodales a, mattis et purus. Nam convallis augue velit, nec blandit ipsum porta vitae. Quisque et iaculis lectus. Donec eu fringilla turpis, id rutrum mauris.<br><br><br>Proin eu sagittis sem. Aenean vel placerat sapien. Praesent et rutrum justo. Mauris consectetur venenatis est, eu vulputate enim elementum eget. In hac habitasse platea dictumst. Sed vehicula nec neque sed posuere. Aenean sodales lectus a purus posuere lacinia. Aenean ut enim vel odio varius placerat. Phasellus faucibus turpis sed arcu ultrices interdum. Sed porta, nisi nec vehicula lacinia, ante tortor tristique justo, vel sagittis felis ligula eu magna. Pellentesque a velit laoreet nunc aliquet ornare sit amet eget lorem. Duis aliquet viverra pretium. Etiam a mauris tellus. Sed viverra eros eget lectus lobortis, in vestibulum lorem rhoncus. Aliquam sem felis, suscipit a gravida ut, eleifend et ipsum. Nullam lacus lacus, rutrum quis sollicitudin et, porta et erat", fE1.Description);
            Assert.That(fE1.DisplayOrder, Is.EqualTo(1));
            Assert.That(fE1.Mandatory, Is.EqualTo(false));
            Assert.That(fE1.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test]
        public async Task Core_eForm_OptionsWithMicrotingFormFromXML_ReturnseMainElement()
        {
            // Arrange
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>7430</Id>
                  <Repeated>0</Repeated>
                  <Label>Muligheder med Microting eForm</Label>
                  <StartDate>2017-06-15</StartDate>
                  <EndDate>2027-06-15</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>76</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>7430</Id>
                      <Label>Muligheder med Microting eForm</Label>
                      <Description><![CDATA[Tryk her og prøv hvordan du indsamler data med Microting eForm.<br><br><br>God fornøjelse :-)]]></Description>
                      <DisplayOrder>76</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>true</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='SaveButton'>
                          <Id>59290</Id>
                          <Label>GEM</Label>
                          <Description><![CDATA[Tryk her for at gemme dine indtastede data<br><br>]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Value>GEM</Value>
                        </DataItem>
                        <DataItem type='Timer'>
                          <Id>59325</Id>
                          <Label>START-STOP TID</Label>
                          <Description><![CDATA[Start-stop tid.<br><br>]]></Description>
                          <DisplayOrder>1</DisplayOrder>
                          <StopOnSave>false</StopOnSave>
                          <Mandatory>false</Mandatory>
                        </DataItem>
                        <DataItem type='None'>
                          <Id>59305</Id>
                          <Label>INFO</Label>
                          <Description><![CDATA[I dette tekstfelt vises ikke redigerbar tekst.<br><br>Er Microting eForm integreret med ERP-system, kan data fra ERP-systemet vises i dette felt fx. baggrundsinformation på kunder.<br>]]></Description>
                          <DisplayOrder>2</DisplayOrder>
                        </DataItem>
                        <DataItem type='ShowPDF'>
                          <Id>59355</Id>
                          <Label>PDF</Label>
                          <Description><![CDATA[Her vises PDF-filer.<br>]]></Description>
                          <DisplayOrder>3</DisplayOrder>
                          <Value>https://eform.microting.com/app_files/uploads/20160609143348_366_a60ad2d8c22ed24780bfa9a348376232.pdf</Value>
                        </DataItem>
                        <DataItem type='CheckBox'>
                          <Id>59340</Id>
                          <Label>TJEK</Label>
                          <Description><![CDATA[I et tjekfelt sættes et flueben.<br>]]></Description>
                          <DisplayOrder>5</DisplayOrder>
                          <Selected>false</Selected>
                          <Mandatory>false</Mandatory>
                        </DataItem>
                        <DataItem type='MultiSelect'>
                          <Id>59310</Id>
                          <Label>VÆLG</Label>
                          <Description><![CDATA[Vælg én eller flere i liste.<br><br>Er Microting eForm integreret med ERP-system, kan valgmulighederne komme derfra.<br>]]></Description>
                          <DisplayOrder>6</DisplayOrder>
                          <Mandatory>false</Mandatory>
                          <KeyValuePairList>
                            <KeyValuePair>
                              <Key>1</Key>
                              <Value><![CDATA[Valgmulighed 1]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>1</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>2</Key>
                              <Value><![CDATA[Valgmulighed 2]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>2</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>3</Key>
                              <Value><![CDATA[Valgmulighed 3]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>3</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>4</Key>
                              <Value><![CDATA[Valgmulighed 4]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>4</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>5</Key>
                              <Value><![CDATA[Valgmulighed 5]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>5</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>6</Key>
                              <Value><![CDATA[Valgmulighed 6]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>6</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>7</Key>
                              <Value><![CDATA[Valgmulighed 7]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>7</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>8</Key>
                              <Value><![CDATA[Valgmulighed 8]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>8</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>9</Key>
                              <Value><![CDATA[Valgmulighed 9]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>9</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>10</Key>
                              <Value><![CDATA[Valgmulighed N]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>10</DisplayOrder>
                            </KeyValuePair>
                          </KeyValuePairList>
                        </DataItem>
                        <DataItem type='SingleSelect'>
                          <Id>59320</Id>
                          <Label>VÆLG ÉN</Label>
                          <Description><![CDATA[Vælg én blandt flere valgmuligheder.<br><br>Er Microting eForm integreret med ERP-system, kan valgmulighederne komme derfra.]]></Description>
                          <DisplayOrder>7</DisplayOrder>
                          <Mandatory>false</Mandatory>
                          <KeyValuePairList>
                            <KeyValuePair>
                              <Key>1</Key>
                              <Value><![CDATA[Valgmulighed 1]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>1</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>2</Key>
                              <Value><![CDATA[Valgmulighed 2]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>2</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>3</Key>
                              <Value><![CDATA[Valgmulighed 3]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>3</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>4</Key>
                              <Value><![CDATA[Valgmulighed 4]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>4</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>5</Key>
                              <Value><![CDATA[Valgmulighed 5]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>5</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>6</Key>
                              <Value><![CDATA[Valgmulighed 6]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>6</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>7</Key>
                              <Value><![CDATA[Valgmulighed 7]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>7</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>8</Key>
                              <Value><![CDATA[Valgmulighed 8]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>8</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>9</Key>
                              <Value><![CDATA[Valgmulighed 9]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>9</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>10</Key>
                              <Value><![CDATA[Valgmulighed N]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>10</DisplayOrder>
                            </KeyValuePair>
                          </KeyValuePairList>
                        </DataItem>
                        <DataItem type='Date'>
                          <Id>59300</Id>
                          <Label>DATO</Label>
                          <Description><![CDATA[Vælg dato<br><br>Er Microting eForm integreret med ERP-system, kan valgt dato leveres direkte i ERP-system.<br>]]></Description>
                          <DisplayOrder>8</DisplayOrder>
                          <MinValue>2016-06-09</MinValue>
                          <MaxValue>2026-06-09</MaxValue>
                          <Value/>
                          <Mandatory>false</Mandatory>
                          <ReadOnly>false</ReadOnly>
                        </DataItem>
                        <DataItem type='Number'>
                          <Id>59315</Id>
                          <Label>INDTAST TAL</Label>
                          <Description><![CDATA[Indtast tal og opsæt evt. regler for mindste/højeste tilladte værdi.<br><br>Er Microting eForm integreret med ERP-system, sendes de indtastede værdier direkte til ERP-systemet.<br>]]></Description>
                          <DisplayOrder>9</DisplayOrder>
                          <Mandatory>false</Mandatory>
                          <MinValue/>
                          <MaxValue/>
                          <Value/>
                          <DecimalCount/>
                          <UnitName/>
                        </DataItem>
                        <DataItem type='Text'>
                          <Id>59335</Id>
                          <Label>SKRIV KORT KOMMENTAR</Label>
                          <Description><![CDATA[Skriv kort kommentar uden linieskift.]]></Description>
                          <DisplayOrder>10</DisplayOrder>
                          <Multi>0</Multi>
                          <GeolocationEnabled>false</GeolocationEnabled>
                          <Split>false</Split>
                          <Value/>
                          <ReadOnly>false</ReadOnly>
                          <Mandatory>false</Mandatory>
                        </DataItem>
                        <DataItem type='Picture'>
                          <Id>59295</Id>
                          <Label>FOTO</Label>
                          <Description><![CDATA[Tag billeder<br><br>Er Microting eForm integreret med ERP-system, kan billederne vises direkte i virksomhedens ERP/andet databasesystem.]]></Description>
                          <DisplayOrder>11</DisplayOrder>
                          <Mandatory>false</Mandatory>
                        </DataItem>
                        <DataItem type='Comment'>
                          <Id>59330</Id>
                          <Label>SKRIV LANG KOMMENTAR</Label>
                          <Description><![CDATA[Skriv længere kommentar med mulighed for linieskift.]]></Description>
                          <DisplayOrder>12</DisplayOrder>
                          <Multi>1</Multi>
                          <GeolocationEnabled>false</GeolocationEnabled>
                          <Split>false</Split>
                          <Value/>
                          <ReadOnly>false</ReadOnly>
                          <Mandatory>false</Mandatory>
                        </DataItem>
                        <DataItem type='Signature'>
                          <Id>59345</Id>
                          <Label>UNDERSKRIFT</Label>
                          <Description><![CDATA[Underskrift<br><br>Er Microting eForm integreret med ERP-system, kan underskrifterne sendes direkte til ERP/andet databasesystem.]]></Description>
                          <DisplayOrder>13</DisplayOrder>
                          <Mandatory>false</Mandatory>
                        </DataItem>
                        <DataItem type='SaveButton'>
                          <Id>59350</Id>
                          <Label>GEM</Label>
                          <Description><![CDATA[<br>Tryk for at gemme data.<br>Press to save data.<br>]]></Description>
                          <DisplayOrder>14</DisplayOrder>
                          <Value>GEM/SAVE</Value>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";

            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("Muligheder med Microting eForm"));
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(76));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(14));
            Assert.That(dE.Label, Is.EqualTo("Muligheder med Microting eForm"));
            // Assert.AreEqual(CDataValue, "Tryk her og prøv hvordan du indsamler data med Microting eForm.<br><br>God fornøjelse :-)"); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(76));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(dE.DoneButtonDisabled, true); //TODO no method donebuttondisabled
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            SaveButton sB = (SaveButton)dE.DataItemList[0];
            Assert.That(sB.Label, Is.EqualTo("GEM"));
            // Assert.AreEqual("Tryk her for at gemme dine indtastede data", CDataValue) //TODO
            Assert.That(sB.DisplayOrder, Is.EqualTo(0));
            Assert.That(sB.Value, Is.EqualTo("GEM"));

            Timer t1 = (Timer)dE.DataItemList[1];
            Assert.That(t1.Label, Is.EqualTo("START-STOP TID"));
            // Assert.AreEqual("Start-stop tid.", CDataValue) //TODO
            Assert.That(sB.DisplayOrder, Is.EqualTo(0));
            Assert.That(t1.StopOnSave, Is.EqualTo(false));
            Assert.That(t1.Mandatory, Is.EqualTo(false));

            None n1 = (None)dE.DataItemList[2];
            Assert.That(n1.Label, Is.EqualTo("INFO"));
            // Assert.AreEqual("I dette tekstfelt vises ikke redigerbar tekst.", n1.Description); TODO
            Assert.That(n1.DisplayOrder, Is.EqualTo(2));

            ShowPdf sp = (ShowPdf)dE.DataItemList[3];
            Assert.That(sp.Label, Is.EqualTo("PDF"));
            // Assert.AreEqual("Her vises PDF-filer.", sp.Description); TODO
            Assert.That(sp.DisplayOrder, Is.EqualTo(3));
            Assert.That(
                sp.Value,
                Is.EqualTo("https://eform.microting.com/app_files/uploads/20160609143348_366_a60ad2d8c22ed24780bfa9a348376232.pdf"));

            CheckBox cB = (CheckBox)dE.DataItemList[4];
            Assert.That(cB.Label, Is.EqualTo("TJEK"));
            // Assert.AreEqual("I et tjekfelt sættes et flueben.", cB.Description); TODO
            Assert.That(cB.DisplayOrder, Is.EqualTo(5));
            Assert.That(cB.Selected, Is.EqualTo(false));
            Assert.That(cB.Mandatory, Is.EqualTo(false));

            MultiSelect mS = (MultiSelect)dE.DataItemList[5];
            Assert.That(mS.Label, Is.EqualTo("VÆLG"));
            // Assert.AreEqual("Vælg en eller flere i liste. er Microting eform integereret med ERP-System, kan valgmulighederne komme derfra", cB.Description); TODO
            Assert.That(mS.DisplayOrder, Is.EqualTo(6));
            Assert.That(mS.Mandatory, Is.EqualTo(false));

            KeyValuePair kP = mS.KeyValuePairList[0];
            Assert.That(kP.Key, Is.EqualTo("1"));
            Assert.That(kP.Value, Is.EqualTo("Valgmulighed 1"));
            Assert.That(kP.Selected, Is.EqualTo(false));
            Assert.That(kP.DisplayOrder, Is.EqualTo("1"));

            KeyValuePair kP1 = mS.KeyValuePairList[1];
            Assert.That(kP1.Key, Is.EqualTo("2"));
            Assert.That(kP1.Value, Is.EqualTo("Valgmulighed 2"));
            Assert.That(kP1.Selected, Is.EqualTo(false));
            Assert.That(kP1.DisplayOrder, Is.EqualTo("2"));

            KeyValuePair kP2 = mS.KeyValuePairList[2];
            Assert.That(kP2.Key, Is.EqualTo("3"));
            Assert.That(kP2.Value, Is.EqualTo("Valgmulighed 3"));
            Assert.That(kP2.Selected, Is.EqualTo(false));
            Assert.That(kP2.DisplayOrder, Is.EqualTo("3"));

            KeyValuePair kP3 = mS.KeyValuePairList[3];
            Assert.That(kP3.Key, Is.EqualTo("4"));
            Assert.That(kP3.Value, Is.EqualTo("Valgmulighed 4"));
            Assert.That(kP3.Selected, Is.EqualTo(false));
            Assert.That(kP3.DisplayOrder, Is.EqualTo("4"));

            KeyValuePair kP4 = mS.KeyValuePairList[4];
            Assert.That(kP4.Key, Is.EqualTo("5"));
            Assert.That(kP4.Value, Is.EqualTo("Valgmulighed 5"));
            Assert.That(kP4.Selected, Is.EqualTo(false));
            Assert.That(kP4.DisplayOrder, Is.EqualTo("5"));

            KeyValuePair kP5 = mS.KeyValuePairList[5];
            Assert.That(kP5.Key, Is.EqualTo("6"));
            Assert.That(kP5.Value, Is.EqualTo("Valgmulighed 6"));
            Assert.That(kP5.Selected, Is.EqualTo(false));
            Assert.That(kP5.DisplayOrder, Is.EqualTo("6"));

            KeyValuePair kP6 = mS.KeyValuePairList[6];
            Assert.That(kP6.Key, Is.EqualTo("7"));
            Assert.That(kP6.Value, Is.EqualTo("Valgmulighed 7"));
            Assert.That(kP6.Selected, Is.EqualTo(false));
            Assert.That(kP6.DisplayOrder, Is.EqualTo("7"));

            KeyValuePair kP7 = mS.KeyValuePairList[7];
            Assert.That(kP7.Key, Is.EqualTo("8"));
            Assert.That(kP7.Value, Is.EqualTo("Valgmulighed 8"));
            Assert.That(kP7.Selected, Is.EqualTo(false));
            Assert.That(kP7.DisplayOrder, Is.EqualTo("8"));

            KeyValuePair kP8 = mS.KeyValuePairList[8];
            Assert.That(kP8.Key, Is.EqualTo("9"));
            Assert.That(kP8.Value, Is.EqualTo("Valgmulighed 9"));
            Assert.That(kP8.Selected, Is.EqualTo(false));
            Assert.That(kP8.DisplayOrder, Is.EqualTo("9"));

            KeyValuePair kP9 = mS.KeyValuePairList[9];
            Assert.That(kP9.Value, Is.EqualTo("Valgmulighed N"));
            Assert.That(kP9.Selected, Is.EqualTo(false));
            Assert.That(kP9.DisplayOrder, Is.EqualTo("10"));

            SingleSelect sS = (SingleSelect)dE.DataItemList[6];
            Assert.That(sS.Label, Is.EqualTo("VÆLG ÉN"));
            // Assert.AreEqual("Vælg én blandt flere valgmuligheder.<br><br>Er Microting eForm integreret med ERP-system, kan valgmulighederne komme derfra.]]></", cB.Description); TODO
            Assert.That(sS.DisplayOrder, Is.EqualTo(7));
            Assert.That(sS.Mandatory, Is.EqualTo(false));

            KeyValuePair skP = mS.KeyValuePairList[0];
            Assert.That(kP.Key, Is.EqualTo("1"));
            Assert.That(kP.Value, Is.EqualTo("Valgmulighed 1"));
            Assert.That(kP.Selected, Is.EqualTo(false));
            Assert.That(kP.DisplayOrder, Is.EqualTo("1"));

            KeyValuePair skP1 = mS.KeyValuePairList[1];
            Assert.That(kP1.Key, Is.EqualTo("2"));
            Assert.That(kP1.Value, Is.EqualTo("Valgmulighed 2"));
            Assert.That(kP1.Selected, Is.EqualTo(false));
            Assert.That(kP1.DisplayOrder, Is.EqualTo("2"));

            KeyValuePair skP2 = mS.KeyValuePairList[2];
            Assert.That(kP2.Key, Is.EqualTo("3"));
            Assert.That(kP2.Value, Is.EqualTo("Valgmulighed 3"));
            Assert.That(kP2.Selected, Is.EqualTo(false));
            Assert.That(kP2.DisplayOrder, Is.EqualTo("3"));

            KeyValuePair skP3 = mS.KeyValuePairList[3];
            Assert.That(kP3.Key, Is.EqualTo("4"));
            Assert.That(kP3.Value, Is.EqualTo("Valgmulighed 4"));
            Assert.That(kP3.Selected, Is.EqualTo(false));
            Assert.That(kP3.DisplayOrder, Is.EqualTo("4"));

            KeyValuePair skP4 = mS.KeyValuePairList[4];
            Assert.That(kP4.Key, Is.EqualTo("5"));
            Assert.That(kP4.Value, Is.EqualTo("Valgmulighed 5"));
            Assert.That(kP4.Selected, Is.EqualTo(false));
            Assert.That(kP4.DisplayOrder, Is.EqualTo("5"));

            KeyValuePair skP5 = mS.KeyValuePairList[5];
            Assert.That(kP5.Key, Is.EqualTo("6"));
            Assert.That(kP5.Value, Is.EqualTo("Valgmulighed 6"));
            Assert.That(kP5.Selected, Is.EqualTo(false));
            Assert.That(kP5.DisplayOrder, Is.EqualTo("6"));

            KeyValuePair skP6 = mS.KeyValuePairList[6];
            Assert.That(kP6.Key, Is.EqualTo("7"));
            Assert.That(kP6.Value, Is.EqualTo("Valgmulighed 7"));
            Assert.That(kP6.Selected, Is.EqualTo(false));
            Assert.That(kP6.DisplayOrder, Is.EqualTo("7"));

            KeyValuePair skP7 = mS.KeyValuePairList[7];
            Assert.That(kP7.Key, Is.EqualTo("8"));
            Assert.That(kP7.Value, Is.EqualTo("Valgmulighed 8"));
            Assert.That(kP7.Selected, Is.EqualTo(false));
            Assert.That(kP7.DisplayOrder, Is.EqualTo("8"));

            KeyValuePair skP8 = mS.KeyValuePairList[8];
            Assert.That(kP8.Key, Is.EqualTo("9"));
            Assert.That(kP8.Value, Is.EqualTo("Valgmulighed 9"));
            Assert.That(kP8.Selected, Is.EqualTo(false));
            Assert.That(kP8.DisplayOrder, Is.EqualTo("9"));

            KeyValuePair skP9 = mS.KeyValuePairList[9];
            Assert.That(kP9.Value, Is.EqualTo("Valgmulighed N"));
            Assert.That(kP9.Selected, Is.EqualTo(false));
            Assert.That(kP9.DisplayOrder, Is.EqualTo("10"));

            Date d1 = (Date)dE.DataItemList[7];
            Assert.That(d1.Label, Is.EqualTo("DATO"));
            // Assert.AreEqual("Vælg dato<br><br>Er Microting eForm integreret med ERP-system, kan valgt dato leveres direkte i ERP-system]]></", cB.Description); TODO
            Assert.That(d1.DisplayOrder, Is.EqualTo(8));
            // Assert.AreEqual("2016-06-09", d1.MinValue); TODO
            // Assert.AreEqual("2026-06-09", d1.MaxValue); TODO
            Assert.That(d1.Mandatory, Is.EqualTo(false));
            Assert.That(d1.ReadOnly, Is.EqualTo(false));

            Number n2 = (Number)dE.DataItemList[8];
            Assert.That(n2.Label, Is.EqualTo("INDTAST TAL"));
            // Assert.AreEqual("Indtast tal og opsæt evt. regler for mindste/højeste tilladte værdi.<br><br>Er Microting eForm integreret med ERP-system, sendes de indtastede værdier direkte til ERP-systemet]></", cB.Description); TODO
            Assert.That(n2.DisplayOrder, Is.EqualTo(9));
            //         Assert.AreEqual("", n2.MinValue);
            //         Assert.AreEqual("", n2.MaxValue);
            //         Assert.AreEqual("", n2.DecimalCount);
            //         Assert.AreEqual("", n2.UnitName);
            Assert.That(d1.Mandatory, Is.EqualTo(false));

            Text tt1 = (Text)dE.DataItemList[9];
            Assert.That(tt1.Label, Is.EqualTo("SKRIV KORT KOMMENTAR"));
            // Assert.AreEqual(" Skriv kort kommentar uden linieskift]></", cB.Description); TODO
            Assert.That(tt1.DisplayOrder, Is.EqualTo(10));
            // Assert.AreEqual(0, tt1.multi) TODO
            Assert.That(tt1.GeolocationEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, tt1.split) TODO
            // Assert.AreEqual("", tt1.Value); TODO
            Assert.That(tt1.ReadOnly, Is.EqualTo(false));
            Assert.That(tt1.Mandatory, Is.EqualTo(false));

            Picture pp1 = (Picture)dE.DataItemList[10];
            Assert.That(pp1.Label, Is.EqualTo("FOTO"));
            // Assert.AreEqual("Er Microting eForm integreret med ERP-system, kan billederne vises direkte i virksomhedens ERP/andet databasesystem.", pp1.Description);
            Assert.That(pp1.DisplayOrder, Is.EqualTo(11));
            Assert.That(pp1.Mandatory, Is.EqualTo(false));

            Comment cc1 = (Comment)dE.DataItemList[11];
            Assert.That(cc1.Label, Is.EqualTo("SKRIV LANG KOMMENTAR"));
            // Assert.AreEqual("Skriv længere kommentar med mulighed for linieskift.", cc1.Description);
            Assert.That(cc1.DisplayOrder, Is.EqualTo(12));
            // Assert.AreEqual(1, cc1.multi);
            // Assert.AreEqual(false, cc1.geolocation);
            Assert.That(cc1.Value, Is.EqualTo(""));
            Assert.That(cc1.ReadOnly, Is.EqualTo(false));
            Assert.That(cc1.Mandatory, Is.EqualTo(false));

            Signature ss1 = (Signature)dE.DataItemList[12];
            Assert.That(ss1.Label, Is.EqualTo("UNDERSKRIFT"));
            // Assert.AreEqual("Underskrift<br><br>Er Microting eForm integreret med ERP-system, kan underskrifterne sendes direkte til ERP/andet databasesystem.", ss1.Description);
            Assert.That(ss1.DisplayOrder, Is.EqualTo(13));
            Assert.That(ss1.Mandatory, Is.EqualTo(false));

            SaveButton ssB = (SaveButton)dE.DataItemList[13];
            Assert.That(ssB.Label, Is.EqualTo("GEM"));
            // Assert.AreEqual("Tryk for at gemme data.<br>Press to save data.", ssB.Description);
            Assert.That(ssB.DisplayOrder, Is.EqualTo(14));
            Assert.That(ssB.Value, Is.EqualTo("GEM/SAVE"));
        }

        [Test]
        public async Task Core_eForm_SimpleMultiSelectFormFromXML_ReturnseMainElement()
        {
            // Arrange
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>6210</Id>
                  <Repeated>0</Repeated>
                  <Label>Multiselect</Label>
                  <StartDate>2017-01-22</StartDate>
                  <EndDate>2027-01-22</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>6210</Id>
                      <Label>Multiselect</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='MultiSelect'>
                          <Id>42600</Id>
                          <Label>Flere valg</Label>
                          <Description><![CDATA[sfsfs]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Mandatory>false</Mandatory>
                          <KeyValuePairList>
                            <KeyValuePair>
                              <Key>1</Key>
                              <Value><![CDATA[a]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>1</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>2</Key>
                              <Value><![CDATA[b]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>2</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>3</Key>
                              <Value><![CDATA[c]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>3</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>4</Key>
                              <Value><![CDATA[d]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>4</DisplayOrder>
                            </KeyValuePair>
                          </KeyValuePairList>
                        </DataItem>
                        <DataItem type='SingleSelect'>
                          <Id>42605</Id>
                          <Label>Choose one option</Label>
                          <Description><![CDATA[This is a description]]></Description>
                          <DisplayOrder>1</DisplayOrder>
                          <Mandatory>false</Mandatory>
                          <KeyValuePairList>
                            <KeyValuePair>
                              <Key>1</Key>
                              <Value><![CDATA[Option 1]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>1</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>2</Key>
                              <Value><![CDATA[Option 2]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>2</DisplayOrder>
                            </KeyValuePair>
                          </KeyValuePairList>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";

            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("Multiselect"));
            // Assert.AreEqual("2017-01-22", match.StartDate); TODO
            // Assert.AreEqual("2027-01-22", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(2));

            Assert.That(dE.Label, Is.EqualTo("Multiselect"));
            // Assert.AreEqual(CDataValue, dE.Label); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.DoneButtondisabled); //TODO
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            MultiSelect mS = (MultiSelect)dE.DataItemList[0];


            Assert.That(mS.Label, Is.EqualTo("Flere valg"));
            // Assert.AreEqual(CDataValue, de.description)"); //TODO
            Assert.That(mS.DisplayOrder, Is.EqualTo(0));
            Assert.That(mS.Mandatory, Is.EqualTo(false));

            KeyValuePair kP = mS.KeyValuePairList[0];
            Assert.That(kP.Key, Is.EqualTo("1"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(kP.Selected, Is.EqualTo(false));
            Assert.That(kP.DisplayOrder, Is.EqualTo("1"));

            KeyValuePair kP2 = mS.KeyValuePairList[1];
            Assert.That(kP2.Key, Is.EqualTo("2"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(kP2.Selected, Is.EqualTo(false));
            Assert.That(kP2.DisplayOrder, Is.EqualTo("2"));

            KeyValuePair kP3 = mS.KeyValuePairList[2];
            Assert.That(kP3.Key, Is.EqualTo("3"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(kP3.Selected, Is.EqualTo(false));
            Assert.That(kP3.DisplayOrder, Is.EqualTo("3"));

            KeyValuePair kP4 = mS.KeyValuePairList[3];
            Assert.That(kP4.Key, Is.EqualTo("4"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(kP4.Selected, Is.EqualTo(false));
            Assert.That(kP4.DisplayOrder, Is.EqualTo("4"));

            SingleSelect sS = (SingleSelect)dE.DataItemList[1];
            Assert.That(sS.Label, Is.EqualTo("Choose one option"));
            // Assert.AreEqual("This is a description", sS.Description) //TODO
            Assert.That(sS.DisplayOrder, Is.EqualTo(1));
            Assert.That(sS.Mandatory, Is.EqualTo(false));

            KeyValuePair kkP = sS.KeyValuePairList[0];
            Assert.That(kkP.Key, Is.EqualTo("1"));
            // Assert.AreEqual(CData, kkP.Value); TODO
            Assert.That(kkP.Selected, Is.EqualTo(false));
            Assert.That(kkP.DisplayOrder, Is.EqualTo("1"));

            KeyValuePair kkP2 = sS.KeyValuePairList[1];
            Assert.That(kkP2.Key, Is.EqualTo("2"));
            // Assert.AreEqual(CData, kkP2.Value); TODO
            Assert.That(kkP2.Selected, Is.EqualTo(false));
            Assert.That(kkP2.DisplayOrder, Is.EqualTo("2"));
        }

        [Test]
        public async Task Core_eFormSimpleSingleSelectFormFromXML_ReturnseMainElement()
        {
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>138808</Id>
                  <Repeated>0</Repeated>
                  <Label>Single Select</Label>
                  <StartDate>2018-05-08</StartDate>
                  <EndDate>2028-05-08</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>138808</Id>
                      <Label>Single Select</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='SingleSelect'>
                          <Id>343973</Id>
                          <Label>Single Select 1</Label>
                          <Description><![CDATA[Single Select 1 description]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Mandatory>false</Mandatory>
                          <KeyValuePairList>
                            <KeyValuePair>
                              <Key>1</Key>
                              <Value><![CDATA[a]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>1</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>2</Key>
                              <Value><![CDATA[b]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>2</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>3</Key>
                              <Value><![CDATA[c]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>3</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>4</Key>
                              <Value><![CDATA[d]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>4</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>5</Key>
                              <Value><![CDATA[...]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>5</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>6</Key>
                              <Value><![CDATA[x]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>6</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>7</Key>
                              <Value><![CDATA[y]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>7</DisplayOrder>
                            </KeyValuePair>
                            <KeyValuePair>
                              <Key>8</Key>
                              <Value><![CDATA[z]]></Value>
                              <Selected>false</Selected>
                              <DisplayOrder>8</DisplayOrder>
                            </KeyValuePair>
                          </KeyValuePairList>
                          <Color>e8eaf6</Color>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";

            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("Single Select"));
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(1));

            Assert.That(dE.Label, Is.EqualTo("Single Select"));
            // Assert.AreEqual(CDataValue, dE.Label); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.DoneButtondisabled); //TODO
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            SingleSelect sS = (SingleSelect)dE.DataItemList[0];


            Assert.That(sS.Label, Is.EqualTo("Single Select 1"));
            // Assert.AreEqual(CDataValue, ss.description)"); //TODO
            Assert.That(sS.DisplayOrder, Is.EqualTo(0));
            Assert.That(sS.Mandatory, Is.EqualTo(false));

            KeyValuePair kP = sS.KeyValuePairList[0];
            Assert.That(kP.Key, Is.EqualTo("1"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(kP.Selected, Is.EqualTo(false));
            Assert.That(kP.DisplayOrder, Is.EqualTo("1"));

            KeyValuePair kP2 = sS.KeyValuePairList[1];
            Assert.That(kP2.Key, Is.EqualTo("2"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(kP2.Selected, Is.EqualTo(false));
            Assert.That(kP2.DisplayOrder, Is.EqualTo("2"));

            KeyValuePair kP3 = sS.KeyValuePairList[2];
            Assert.That(kP3.Key, Is.EqualTo("3"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(kP3.Selected, Is.EqualTo(false));
            Assert.That(kP3.DisplayOrder, Is.EqualTo("3"));

            KeyValuePair kP4 = sS.KeyValuePairList[3];
            Assert.That(kP4.Key, Is.EqualTo("4"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(kP4.Selected, Is.EqualTo(false));
            Assert.That(kP4.DisplayOrder, Is.EqualTo("4"));

            KeyValuePair kP5 = sS.KeyValuePairList[4];
            Assert.That(kP5.Key, Is.EqualTo("5"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(kP5.Selected, Is.EqualTo(false));
            Assert.That(kP5.DisplayOrder, Is.EqualTo("5"));

            KeyValuePair kP6 = sS.KeyValuePairList[5];
            Assert.That(kP6.Key, Is.EqualTo("6"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(kP6.Selected, Is.EqualTo(false));
            Assert.That(kP6.DisplayOrder, Is.EqualTo("6"));

            KeyValuePair kP7 = sS.KeyValuePairList[6];
            Assert.That(kP7.Key, Is.EqualTo("7"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(kP7.Selected, Is.EqualTo(false));
            Assert.That(kP7.DisplayOrder, Is.EqualTo("7"));

            KeyValuePair kP8 = sS.KeyValuePairList[7];
            Assert.That(kP8.Key, Is.EqualTo("8"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(kP8.Selected, Is.EqualTo(false));
            Assert.That(kP8.DisplayOrder, Is.EqualTo("8"));

            Assert.That(sS.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test] // Comment
        public async Task Core_eFormSimpleTextMultiLineFormFromXML_ReturnseMainElement()
        {
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>9060</Id>
                  <Repeated>0</Repeated>
                  <Label>comment</Label>
                  <StartDate>2017-07-07</StartDate>
                  <EndDate>2027-07-07</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>9060</Id>
                      <Label>comment</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='Comment'>
                          <Id>73660</Id>
                          <Label>Comment</Label>
                          <Description><![CDATA[]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Multi>1</Multi>
                          <GeolocationEnabled>false</GeolocationEnabled>
                          <Split>false</Split>
                          <Value/>
                          <ReadOnly>false</ReadOnly>
                          <Mandatory>false</Mandatory>
                          <Color>e8eaf6</Color>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";

            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("comment"));
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(1));

            Assert.That(dE.Label, Is.EqualTo("comment"));
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            Comment cc = (Comment)dE.DataItemList[0];

            Assert.That(cc.Label, Is.EqualTo("Comment"));
            // Assert.AreEqual(CDataValue, cc.Description); //todo
            Assert.That(cc.DisplayOrder, Is.EqualTo(0));
            // Assert.AreEqual(1, cc.multi) //TODO
            // Assert.AreEqual(false, cc.geolocation) //todo
            // Assert.AreEqual(false, cc.split) //TODO
            Assert.That(cc.Value, Is.EqualTo(""));
            Assert.That(cc.ReadOnly, Is.EqualTo(false));
            Assert.That(cc.Mandatory, Is.EqualTo(false));
            Assert.That(cc.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test] // Text
        public async Task Core_eFormSimpleTextSingleLineFormFromXML_ReturnseMainElement()
        {
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>138803</Id>
                  <Repeated>0</Repeated>
                  <Label>Single line</Label>
                  <StartDate>2018-05-08</StartDate>
                  <EndDate>2028-05-08</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>138803</Id>
                      <Label>Single line</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='Text'>
                          <Id>343968</Id>
                          <Label>Single line 1</Label>
                          <Description><![CDATA[Single line 1 description]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Multi>0</Multi>
                          <GeolocationEnabled>false</GeolocationEnabled>
                          <Split>false</Split>
                          <Value/>
                          <ReadOnly>false</ReadOnly>
                          <Mandatory>false</Mandatory>
                          <Color>e8eaf6</Color>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";

            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("Single line"));
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(1));

            Assert.That(dE.Label, Is.EqualTo("Single line"));
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            Text t = (Text)dE.DataItemList[0];

            Assert.That(t.Label, Is.EqualTo("Single line 1"));
            // Assert.AreEqual(CDataValue, t.Description); //TODO
            Assert.That(t.DisplayOrder, Is.EqualTo(0));
            // Assert.AreEqual(0, t.multi) //TODO
            Assert.That(t.GeolocationEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, t.split) //TODO
            Assert.That(t.Value, Is.EqualTo(""));
            Assert.That(t.ReadOnly, Is.EqualTo(false));
            Assert.That(t.Mandatory, Is.EqualTo(false));
            Assert.That(t.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test]
        public async Task Core_eFormSimpleNumberFormFromXML_ReturnseMainElement()
        {
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>138798</Id>
                  <Repeated>0</Repeated>
                  <Label>Number 1</Label>
                  <StartDate>2018-05-08</StartDate>
                  <EndDate>2028-05-08</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>138798</Id>
                      <Label>Number 1</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='Number'>
                          <Id>343963</Id>
                          <Label>Number 1</Label>
                          <Description><![CDATA[Number 1 description]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Mandatory>true</Mandatory>
                          <MinValue>1</MinValue>
                          <MaxValue>1100</MaxValue>
                          <Value>24</Value>
                          <DecimalCount>2</DecimalCount>
                          <UnitName/>
                          <Color>e8eaf6</Color>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";

            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("Number 1"));
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(1));

            Assert.That(dE.Label, Is.EqualTo("Number 1"));
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            Number n = (Number)dE.DataItemList[0];

            Assert.That(n.Label, Is.EqualTo("Number 1"));
            // Assert.AreEqual(CDataValue, t.Description); //TODO
            Assert.That(n.DisplayOrder, Is.EqualTo(0));
            Assert.That(n.MinValue, Is.EqualTo("1"));
            Assert.That(n.MaxValue, Is.EqualTo("1100"));
            // Assert.AreEqual(24, n.value) //TODO
            Assert.That(n.DecimalCount, Is.EqualTo(2));
            Assert.That(n.Mandatory, Is.EqualTo(true));
            Assert.That(n.UnitName, Is.EqualTo(""));
            Assert.That(n.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test]
        public async Task Core_eFormSimpleInfoboxFormFromXML_ReturnseMainElement()
        {
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>138793</Id>
                  <Repeated>0</Repeated>
                  <Label>Info box</Label>
                  <StartDate>2018-05-08</StartDate>
                  <EndDate>2028-05-08</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>138793</Id>
                      <Label>Info box</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='None'>
                          <Id>343958</Id>
                          <Label>Info box 1</Label>
                          <Description><![CDATA[Info box 1 description]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Color>e8eaf6</Color>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";

            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("Info box"));
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(1));

            Assert.That(dE.Label, Is.EqualTo("Info box"));
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            None n = (None)dE.DataItemList[0];

            Assert.That(n.Label, Is.EqualTo("Info box 1"));
            // Assert.AreEqual(CDataValue, t.Description); //TODO
            Assert.That(n.DisplayOrder, Is.EqualTo(0));
            Assert.That(n.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test]
        public async Task Core_eFormSimpleCheckBoxFormFromXML_ReturnseMainElement()
        {
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>138778</Id>
                  <Repeated>0</Repeated>
                  <Label>checkbox</Label>
                  <StartDate>2018-05-08</StartDate>
                  <EndDate>2028-05-08</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>138778</Id>
                      <Label>checkbox</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='CheckBox'>
                          <Id>343943</Id>
                          <Label>Checkbox 1</Label>
                          <Description><![CDATA[Checkbox 1 description]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Selected>false</Selected>
                          <Mandatory>false</Mandatory>
                          <Color>e8eaf6</Color>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";

            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("checkbox"));
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(1));

            Assert.That(dE.Label, Is.EqualTo("checkbox"));
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            CheckBox cB = (CheckBox)dE.DataItemList[0];

            Assert.That(cB.Label, Is.EqualTo("Checkbox 1"));
            // Assert.AreEqual(CDataValue, t.Description); //TODO
            Assert.That(cB.DisplayOrder, Is.EqualTo(0));
            Assert.That(cB.Selected, Is.EqualTo(false));
            Assert.That(cB.Mandatory, Is.EqualTo(false));
            Assert.That(cB.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test]
        public async Task Core_eFormSimpleTimerFormFromXML_ReturnseMainElement()
        {
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>138783</Id>
                  <Repeated>0</Repeated>
                  <Label>TimerStartStop</Label>
                  <StartDate>2018-05-08</StartDate>
                  <EndDate>2028-05-08</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>138783</Id>
                      <Label>TimerStartStop</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>false</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='Timer'>
                          <Id>343948</Id>
                          <Label>Timer Start Stop 1</Label>
                          <Description><![CDATA[Timer Start Stop 1 Description]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <StopOnSave>false</StopOnSave>
                          <Mandatory>false</Mandatory>
                          <Color>e8eaf6</Color>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";

            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("TimerStartStop"));
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(1));

            Assert.That(dE.Label, Is.EqualTo("TimerStartStop"));
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            Timer t = (Timer)dE.DataItemList[0];

            Assert.That(t.Label, Is.EqualTo("Timer Start Stop 1"));
            // Assert.AreEqual(CDataValue, t.Description); //TODO
            Assert.That(t.DisplayOrder, Is.EqualTo(0));
            Assert.That(t.StopOnSave, Is.EqualTo(false));
            Assert.That(t.Mandatory, Is.EqualTo(false));
            Assert.That(t.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test]
        public async Task Core_eFormSimpleSaveButtonFormFromXML_ReturnseMainElement()
        {
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>138788</Id>
                  <Repeated>0</Repeated>
                  <Label>Save button</Label>
                  <StartDate>2018-05-08</StartDate>
                  <EndDate>2028-05-08</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='DataElement'>
                      <Id>138788</Id>
                      <Label>Save button</Label>
                      <Description><![CDATA[]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ReviewEnabled>false</ReviewEnabled>
                      <ManualSync>false</ManualSync>
                      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                      <DoneButtonDisabled>true</DoneButtonDisabled>
                      <ApprovalEnabled>false</ApprovalEnabled>
                      <DataItemList>
                        <DataItem type='SaveButton'>
                          <Id>343953</Id>
                          <Label>Save button 1</Label>
                          <Description><![CDATA[Save button 1 Description]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <Value/>
                          <Color>e8eaf6</Color>
                        </DataItem>
                      </DataItemList>
                    </Element>
                  </ElementList>
                </Main>";

            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("Save button"));
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.That(dE.DataItemList.Count(), Is.EqualTo(1));

            Assert.That(dE.Label, Is.EqualTo("Save button"));
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            SaveButton sB = (SaveButton)dE.DataItemList[0];

            Assert.That(sB.Label, Is.EqualTo("Save button 1"));
            // Assert.AreEqual(CDataValue, t.Description); //TODO
            Assert.That(sB.DisplayOrder, Is.EqualTo(0));
            Assert.That(sB.Value, Is.EqualTo(""));
            Assert.That(sB.Color, Is.EqualTo(Constants.FieldColors.Default));
        }

        [Test]
        public async Task Core_eForm_MultiLvleFormFromXML_ReturnseMainElement()
        {
            // Arrange
            string xmlstring = @"
                <?xml version='1.0' encoding='UTF-8'?>
                <Main>
                  <Id>138738</Id>
                  <Repeated>0</Repeated>
                  <Label>MultiLvlTest</Label>
                  <StartDate>2018-05-04</StartDate>
                  <EndDate>2028-05-04</EndDate>
                  <Language>da</Language>
                  <MultiApproval>false</MultiApproval>
                  <FastNavigation>false</FastNavigation>
                  <Review>false</Review>
                  <Summary>false</Summary>
                  <DisplayOrder>0</DisplayOrder>
                  <ElementList>
                    <Element type='GroupElement'>
                      <Id>138743</Id>
                      <Label>1 lvl</Label>
                      <Description><![CDATA[1 lvl description]]></Description>
                      <DisplayOrder>0</DisplayOrder>
                      <ElementList>
                        <Element type='DataElement'>
                          <Id>138748</Id>
                          <Label>1.1 lvl</Label>
                          <Description><![CDATA[1.1 lvl description]]></Description>
                          <DisplayOrder>0</DisplayOrder>
                          <ReviewEnabled>false</ReviewEnabled>
                          <ManualSync>false</ManualSync>
                          <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                          <DoneButtonDisabled>false</DoneButtonDisabled>
                          <ApprovalEnabled>false</ApprovalEnabled>
                          <DataItemList>
                            <DataItem type='CheckBox'>
                              <Id>343828</Id>
                              <Label>1.1 lvl checkbox</Label>
                              <Description><![CDATA[1.1 lvl cehckbox description]]></Description>
                              <DisplayOrder>0</DisplayOrder>
                              <Selected>false</Selected>
                              <Mandatory>false</Mandatory>
                              <Color>e8eaf6</Color>
                            </DataItem>
                          </DataItemList>
                        </Element>
                        <Element type='DataElement'>
                          <Id>138753</Id>
                          <Label>1.2 lvl</Label>
                          <Description><![CDATA[1.2 lvl description]]></Description>
                          <DisplayOrder>1</DisplayOrder>
                          <ReviewEnabled>false</ReviewEnabled>
                          <ManualSync>false</ManualSync>
                          <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                          <DoneButtonDisabled>false</DoneButtonDisabled>
                          <ApprovalEnabled>false</ApprovalEnabled>
                          <DataItemList>
                            <DataItem type='CheckBox'>
                              <Id>343833</Id>
                              <Label>1.2 lvl checkbox</Label>
                              <Description><![CDATA[1.2 lvl checkbox description]]></Description>
                              <DisplayOrder>0</DisplayOrder>
                              <Selected>false</Selected>
                              <Mandatory>false</Mandatory>
                              <Color>e8eaf6</Color>
                            </DataItem>
                          </DataItemList>
                        </Element>
                        <Element type='GroupElement'>
                          <Id>138758</Id>
                          <Label>1.3 lvl</Label>
                          <Description><![CDATA[1.3 lvl descrition]]></Description>
                          <DisplayOrder>2</DisplayOrder>
                          <ElementList>
                            <Element type='DataElement'>
                              <Id>138763</Id>
                              <Label>1.3.1 lvl</Label>
                              <Description><![CDATA[1.3.1 lvl description]]></Description>
                              <DisplayOrder>0</DisplayOrder>
                              <ReviewEnabled>false</ReviewEnabled>
                              <ManualSync>false</ManualSync>
                              <ExtraFieldsEnabled>false</ExtraFieldsEnabled>
                              <DoneButtonDisabled>false</DoneButtonDisabled>
                              <ApprovalEnabled>false</ApprovalEnabled>
                              <DataItemList>
                                <DataItem type='CheckBox'>
                                  <Id>343838</Id>
                                  <Label>1.3.1 lvl checkbox</Label>
                                  <Description><![CDATA[1.3.1 lvl checkbox description]]></Description>
                                  <DisplayOrder>0</DisplayOrder>
                                  <Selected>false</Selected>
                                  <Mandatory>false</Mandatory>
                                  <Color>e8eaf6</Color>
                                </DataItem>
                              </DataItemList>
                            </Element>
                          </ElementList>
                        </Element>
                      </ElementList>
                    </Element>
                  </ElementList>
                </Main>";

            var match = await sut.TemplateFromXml(xmlstring);


            // Assert
            Assert.That(match, Is.Not.EqualTo(null));
            Assert.That(match.Repeated, Is.EqualTo(1));
            Assert.That(match.Label, Is.EqualTo("MultiLvlTest"));
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.That(match.Language, Is.EqualTo("da"));
            Assert.That(match.MultiApproval, Is.EqualTo(false));
            Assert.That(match.FastNavigation, Is.EqualTo(false));
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(match.DisplayOrder, Is.EqualTo(0));

            Assert.That(match.ElementList.Count(), Is.EqualTo(1));
            GroupElement gE = (GroupElement)match.ElementList[0];
            Assert.That(gE.ElementList.Count(), Is.EqualTo(3));

            Assert.That(gE.Label, Is.EqualTo("1 lvl"));
            // Assert.AreEqual("1 lvl description", gE.Description); //TODO
            Assert.That(gE.DisplayOrder, Is.EqualTo(0));

            DataElement dE = (DataElement)gE.ElementList[0];
            Assert.That(dE.Label, Is.EqualTo("1.1 lvl"));
            // Assert.AreEqual("1.1 lvl description", de.description); TODO
            Assert.That(dE.DisplayOrder, Is.EqualTo(0));
            Assert.That(dE.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.That(dE.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.That(dE.ApprovalEnabled, Is.EqualTo(false));

            CheckBox cB = (CheckBox)dE.DataItemList[0];
            Assert.That(cB.Label, Is.EqualTo("1.1 lvl checkbox"));
            // Assert.AreEqual("1.1 lvl cehckbox description", cB.Description) //TODO
            Assert.That(cB.DisplayOrder, Is.EqualTo(0));
            Assert.That(cB.Selected, Is.EqualTo(false));
            Assert.That(cB.Mandatory, Is.EqualTo(false));
            Assert.That(cB.Color, Is.EqualTo(Constants.FieldColors.Default));

            DataElement dE2 = (DataElement)gE.ElementList[1];
            Assert.That(dE2.Label, Is.EqualTo("1.2 lvl"));
            // Assert.AreEqual("1.2 lvl description", dE2.Description); //TODO
            Assert.That(dE2.DisplayOrder, Is.EqualTo(1));
            Assert.That(dE2.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE2.manualsync)//TODO
            Assert.That(dE2.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, dE2.DoneButtonDisabled); //TODO
            Assert.That(dE2.ApprovalEnabled, Is.EqualTo(false));

            CheckBox cb2 = (CheckBox)dE2.DataItemList[0];
            Assert.That(cb2.Label, Is.EqualTo("1.2 lvl checkbox"));
            // Assert.AreEqual("1.2 lvl checkbox description", cb2.Description) //TODO
            Assert.That(cb2.DisplayOrder, Is.EqualTo(0));
            Assert.That(cb2.Selected, Is.EqualTo(false));
            Assert.That(cb2.Mandatory, Is.EqualTo(false));
            Assert.That(cb2.Color, Is.EqualTo(Constants.FieldColors.Default));

            GroupElement gE2 = (GroupElement)gE.ElementList[2];
            Assert.That(gE2.Label, Is.EqualTo("1.3 lvl"));
            // Assert.AreEqual("1.3 lvl description", gE2.Description); //TODO
            Assert.That(gE2.DisplayOrder, Is.EqualTo(2));

            DataElement de3 = (DataElement)gE2.ElementList[0];
            Assert.That(de3.Label, Is.EqualTo("1.3.1 lvl"));
            // Assert.AreEqual("1.3.1.1 lvl description", de3.Description) //TODO
            Assert.That(cb2.DisplayOrder, Is.EqualTo(0));
            Assert.That(de3.ReviewEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, de3.manualsync);//TODO
            Assert.That(de3.ExtraFieldsEnabled, Is.EqualTo(false));
            // Assert.AreEqual(false, de3.donebuttondisabled)//TODO
            Assert.That(de3.ApprovalEnabled, Is.EqualTo(false));

            CheckBox cb3 = (CheckBox)de3.DataItemList[0];
            Assert.That(cb3.Label, Is.EqualTo("1.3.1 lvl checkbox"));
            // Assert.AreEqual("1.2 lvl checkbox description", cb2.Description) //TODO
            Assert.That(cb3.DisplayOrder, Is.EqualTo(0));
            Assert.That(cb3.Selected, Is.EqualTo(false));
            Assert.That(cb3.Mandatory, Is.EqualTo(false));
            Assert.That(cb3.Color, Is.EqualTo(Constants.FieldColors.Default));
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