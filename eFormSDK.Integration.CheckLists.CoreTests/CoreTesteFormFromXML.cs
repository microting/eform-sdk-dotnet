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
            Assert.NotNull(match);
            Assert.AreEqual("", match.CaseType);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("9060", match.OriginalId);
            Assert.AreEqual(0, match.Id);
            Assert.AreEqual("CommentMain", match.Label);
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            Assert.AreEqual(0, match.DisplayOrder);
            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(dE.DataItemList.Count(), 1);
            Assert.AreEqual("CommentDataElement", dE.Label);

            CDataValue cd = new CDataValue();

            // Assert.AreEqual(dE.Description, cd); TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(dE.ManualSync) //TODO No Method for ManualSync
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO DoneButtonDisabled no method found
            Assert.AreEqual(false, dE.ApprovalEnabled);

            Comment commentField = (Comment)dE.DataItemList[0];
            Assert.AreEqual("CommentField", commentField.Label);
            // Assert.AreEqual(commentField.Description, cd);
            Assert.AreEqual(0, commentField.DisplayOrder);
            // Assert.AreEqual(commentField.Multi, 0) //TODO No method MULTI
            // Assert.AreEqual(commentField.geolocation, false) //TODO no method geolocation
            // Assert.AreEqual(commentField.Split, false) //TODO no method Split
            Assert.AreEqual("", commentField.Value);
            Assert.AreEqual(false, commentField.ReadOnly);
            Assert.AreEqual(false, commentField.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, commentField.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Picture test", match.Label);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());
            Assert.AreEqual("Picture test", dE.Label);
            // Assert.AreEqual(dE.DisplayOrder, CDataValue); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(false, dE.ApprovalEnabled);

            Picture pictureField = (Picture)dE.DataItemList[0];
            Assert.AreEqual("Take two pictures", pictureField.Label);
            // Assert.AreEqual(pictureField.Description, CDataValue) //TODO
            Assert.AreEqual(0, pictureField.DisplayOrder);
            Assert.AreEqual(false, pictureField.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, pictureField.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Date", match.Label);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());
            Assert.AreEqual("Date", dE.Label);
            // Assert.AreEqual(dE.DisplayOrder, CDataValue); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(false, dE.ApprovalEnabled);

            Date dateField = (Date)dE.DataItemList[0];
            Assert.AreEqual("Select date", dateField.Label);
            // Assert.AreEqual(dateField.Description, CDataValue) //TODO
            Assert.AreEqual(0, dateField.DisplayOrder);
            // Assert.AreEqual("2018-04-25 00:00:00", dateField.MinValue); //TODO
            // Assert.AreEqual("2028-04-25", dateField.MaxValue); //TODO
            Assert.AreEqual(false, dateField.Mandatory);
            Assert.AreEqual(false, dateField.ReadOnly);
            Assert.AreEqual(Constants.FieldColors.Default, dateField.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("ny pdf", match.Label);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());
            Assert.AreEqual("ny pdf", dE.Label);
            // Assert.AreEqual(dE.DisplayOrder, CDataValue); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(false, dE.ApprovalEnabled);

            ShowPdf showPDFField = (ShowPdf)dE.DataItemList[0];
            Assert.AreEqual("bla", showPDFField.Label);
            // Assert.AreEqual(dateField.Description, CDataValue) //TODO
            Assert.AreEqual(0, showPDFField.DisplayOrder);
            Assert.AreEqual(Constants.FieldColors.Default, showPDFField.Color);
            Assert.AreEqual(
                "https://eform.microting.com/app_files/uploads/20170804132716_13790_20d483dd7791cd6becf089432724c663.pdf",
                showPDFField.Value);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Tester grupper", match.Label);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);


            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(3, dE.DataItemList.Count());
            Assert.AreEqual("Tester grupper", dE.Label);
            // Assert.AreEqual(dE.DisplayOrder, CDataValue); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(false, dE.ApprovalEnabled);


            FieldContainer fC = (FieldContainer)dE.DataItemList[0];
            Assert.AreEqual("Gruppe efter tjekboks", fC.Label);
            // Assert.AreEqual(CDataValue, fE.Description); TODO
            Assert.AreEqual(1, fC.DisplayOrder);
            Assert.AreEqual("Closed", fC.Value);
            Assert.AreEqual(Constants.FieldColors.Default, fC.Color);

            CheckBox fE = (CheckBox)fC.DataItemList[0];
            Assert.AreEqual("Tjekboks inde i gruppe", fE.Label);
            // Assert.AreEqual(dateField.Description, CDataValue) //TODO
            Assert.AreEqual(0, fE.DisplayOrder);
            Assert.AreEqual(false, fE.Selected);
            Assert.AreEqual(false, fE.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, fE.Color);


            CheckBox checkboxField = (CheckBox)dE.DataItemList[1];
            Assert.AreEqual("Tjekboks før gruppe", checkboxField.Label);
            // Assert.AreEqual(dateField.Description, CDataValue) //TODO
            Assert.AreEqual(0, checkboxField.DisplayOrder);
            Assert.AreEqual(false, checkboxField.Selected);
            Assert.AreEqual(false, checkboxField.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, checkboxField.Color);


            CheckBox checkboxField1 = (CheckBox)dE.DataItemList[2];
            Assert.AreEqual("Tjekboks efter gruppe", checkboxField1.Label);
            // Assert.AreEqual(dateField.Description, CDataValue) //TODO
            Assert.AreEqual(2, checkboxField1.DisplayOrder);
            Assert.AreEqual(false, checkboxField1.Selected);
            Assert.AreEqual(false, checkboxField1.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, checkboxField1.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Billede og signatur", match.Label);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);


            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(2, dE.DataItemList.Count());
            Assert.AreEqual("Billede og signatur", dE.Label);
            // Assert.AreEqual(CD.Datavalue, de.description); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(false, dE.ApprovalEnabled);

            Picture fE = (Picture)dE.DataItemList[0];
            Assert.AreEqual("Tag et billede", fE.Label);
            // Assert.AreEqual(cD.Datavalue, fe.description) //TODO
            Assert.AreEqual(0, fE.DisplayOrder);
            Assert.AreEqual(false, fE.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, fE.Color);

            Signature fE1 = (Signature)dE.DataItemList[1];
            Assert.AreEqual("Skriv", fE1.Label);
            //TODO Statement below -> CD.Datavalue
            // Assert.AreEqual("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam dui neque, molestie at maximus a, malesuada at mi. Cras venenatis porttitor augue nec sagittis. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris urna massa, sagittis at fringilla ut, convallis sed dolor. Praesent scelerisque magna dolor, quis blandit metus pharetra eu. Cras euismod facilisis risus at ullamcorper. Pellentesque vitae maximus elit. Sed scelerisque nec velit dictum sodales. Duis sed dapibus odio. Sed non luctus sem. Donec eu mollis lectus, nec porta nisl. Aenean a consequat metus, ac auctor arcu. Cras sit amet blandit velit. Pellentesque faucibus eros sed ullamcorper rutrum.<br><br><br>Pellentesque ultrices ex erat. Pellentesque rhoncus eget lectus et scelerisque. Cras vitae diam ex. Ut felis ligula, venenatis ut lorem vel, venenatis convallis turpis. Sed rutrum ac odio ac auctor. Sed mauris ipsum, vulputate ut sodales a, mattis et purus. Nam convallis augue velit, nec blandit ipsum porta vitae. Quisque et iaculis lectus. Donec eu fringilla turpis, id rutrum mauris.<br><br><br>Proin eu sagittis sem. Aenean vel placerat sapien. Praesent et rutrum justo. Mauris consectetur venenatis est, eu vulputate enim elementum eget. In hac habitasse platea dictumst. Sed vehicula nec neque sed posuere. Aenean sodales lectus a purus posuere lacinia. Aenean ut enim vel odio varius placerat. Phasellus faucibus turpis sed arcu ultrices interdum. Sed porta, nisi nec vehicula lacinia, ante tortor tristique justo, vel sagittis felis ligula eu magna. Pellentesque a velit laoreet nunc aliquet ornare sit amet eget lorem. Duis aliquet viverra pretium. Etiam a mauris tellus. Sed viverra eros eget lectus lobortis, in vestibulum lorem rhoncus. Aliquam sem felis, suscipit a gravida ut, eleifend et ipsum. Nullam lacus lacus, rutrum quis sollicitudin et, porta et erat", fE1.Description);
            Assert.AreEqual(1, fE1.DisplayOrder);
            Assert.AreEqual(false, fE1.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, fE1.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Muligheder med Microting eForm", match.Label);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(76, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(14, dE.DataItemList.Count());
            Assert.AreEqual("Muligheder med Microting eForm", dE.Label);
            // Assert.AreEqual(CDataValue, "Tryk her og prøv hvordan du indsamler data med Microting eForm.<br><br>God fornøjelse :-)"); //TODO
            Assert.AreEqual(76, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(dE.DoneButtonDisabled, true); //TODO no method donebuttondisabled
            Assert.AreEqual(false, dE.ApprovalEnabled);

            SaveButton sB = (SaveButton)dE.DataItemList[0];
            Assert.AreEqual("GEM", sB.Label);
            // Assert.AreEqual("Tryk her for at gemme dine indtastede data", CDataValue) //TODO
            Assert.AreEqual(0, sB.DisplayOrder);
            Assert.AreEqual("GEM", sB.Value);

            Timer t1 = (Timer)dE.DataItemList[1];
            Assert.AreEqual("START-STOP TID", t1.Label);
            // Assert.AreEqual("Start-stop tid.", CDataValue) //TODO
            Assert.AreEqual(0, sB.DisplayOrder);
            Assert.AreEqual(false, t1.StopOnSave);
            Assert.AreEqual(false, t1.Mandatory);

            None n1 = (None)dE.DataItemList[2];
            Assert.AreEqual("INFO", n1.Label);
            // Assert.AreEqual("I dette tekstfelt vises ikke redigerbar tekst.", n1.Description); TODO
            Assert.AreEqual(2, n1.DisplayOrder);

            ShowPdf sp = (ShowPdf)dE.DataItemList[3];
            Assert.AreEqual("PDF", sp.Label);
            // Assert.AreEqual("Her vises PDF-filer.", sp.Description); TODO
            Assert.AreEqual(3, sp.DisplayOrder);
            Assert.AreEqual(
                "https://eform.microting.com/app_files/uploads/20160609143348_366_a60ad2d8c22ed24780bfa9a348376232.pdf",
                sp.Value);

            CheckBox cB = (CheckBox)dE.DataItemList[4];
            Assert.AreEqual("TJEK", cB.Label);
            // Assert.AreEqual("I et tjekfelt sættes et flueben.", cB.Description); TODO
            Assert.AreEqual(5, cB.DisplayOrder);
            Assert.AreEqual(false, cB.Selected);
            Assert.AreEqual(false, cB.Mandatory);

            MultiSelect mS = (MultiSelect)dE.DataItemList[5];
            Assert.AreEqual("VÆLG", mS.Label);
            // Assert.AreEqual("Vælg en eller flere i liste. er Microting eform integereret med ERP-System, kan valgmulighederne komme derfra", cB.Description); TODO
            Assert.AreEqual(6, mS.DisplayOrder);
            Assert.AreEqual(false, mS.Mandatory);

            KeyValuePair kP = mS.KeyValuePairList[0];
            Assert.AreEqual("1", kP.Key);
            Assert.AreEqual("Valgmulighed 1", kP.Value);
            Assert.AreEqual(false, kP.Selected);
            Assert.AreEqual("1", kP.DisplayOrder);

            KeyValuePair kP1 = mS.KeyValuePairList[1];
            Assert.AreEqual("2", kP1.Key);
            Assert.AreEqual("Valgmulighed 2", kP1.Value);
            Assert.AreEqual(false, kP1.Selected);
            Assert.AreEqual("2", kP1.DisplayOrder);

            KeyValuePair kP2 = mS.KeyValuePairList[2];
            Assert.AreEqual("3", kP2.Key);
            Assert.AreEqual("Valgmulighed 3", kP2.Value);
            Assert.AreEqual(false, kP2.Selected);
            Assert.AreEqual("3", kP2.DisplayOrder);

            KeyValuePair kP3 = mS.KeyValuePairList[3];
            Assert.AreEqual("4", kP3.Key);
            Assert.AreEqual("Valgmulighed 4", kP3.Value);
            Assert.AreEqual(false, kP3.Selected);
            Assert.AreEqual("4", kP3.DisplayOrder);

            KeyValuePair kP4 = mS.KeyValuePairList[4];
            Assert.AreEqual("5", kP4.Key);
            Assert.AreEqual("Valgmulighed 5", kP4.Value);
            Assert.AreEqual(false, kP4.Selected);
            Assert.AreEqual("5", kP4.DisplayOrder);

            KeyValuePair kP5 = mS.KeyValuePairList[5];
            Assert.AreEqual("6", kP5.Key);
            Assert.AreEqual("Valgmulighed 6", kP5.Value);
            Assert.AreEqual(false, kP5.Selected);
            Assert.AreEqual("6", kP5.DisplayOrder);

            KeyValuePair kP6 = mS.KeyValuePairList[6];
            Assert.AreEqual("7", kP6.Key);
            Assert.AreEqual("Valgmulighed 7", kP6.Value);
            Assert.AreEqual(false, kP6.Selected);
            Assert.AreEqual("7", kP6.DisplayOrder);

            KeyValuePair kP7 = mS.KeyValuePairList[7];
            Assert.AreEqual("8", kP7.Key);
            Assert.AreEqual("Valgmulighed 8", kP7.Value);
            Assert.AreEqual(false, kP7.Selected);
            Assert.AreEqual("8", kP7.DisplayOrder);

            KeyValuePair kP8 = mS.KeyValuePairList[8];
            Assert.AreEqual("9", kP8.Key);
            Assert.AreEqual("Valgmulighed 9", kP8.Value);
            Assert.AreEqual(false, kP8.Selected);
            Assert.AreEqual("9", kP8.DisplayOrder);

            KeyValuePair kP9 = mS.KeyValuePairList[9];
            Assert.AreEqual("Valgmulighed N", kP9.Value);
            Assert.AreEqual(false, kP9.Selected);
            Assert.AreEqual("10", kP9.DisplayOrder);

            SingleSelect sS = (SingleSelect)dE.DataItemList[6];
            Assert.AreEqual("VÆLG ÉN", sS.Label);
            // Assert.AreEqual("Vælg én blandt flere valgmuligheder.<br><br>Er Microting eForm integreret med ERP-system, kan valgmulighederne komme derfra.]]></", cB.Description); TODO
            Assert.AreEqual(7, sS.DisplayOrder);
            Assert.AreEqual(false, sS.Mandatory);

            KeyValuePair skP = mS.KeyValuePairList[0];
            Assert.AreEqual("1", kP.Key);
            Assert.AreEqual("Valgmulighed 1", kP.Value);
            Assert.AreEqual(false, kP.Selected);
            Assert.AreEqual("1", kP.DisplayOrder);

            KeyValuePair skP1 = mS.KeyValuePairList[1];
            Assert.AreEqual("2", kP1.Key);
            Assert.AreEqual("Valgmulighed 2", kP1.Value);
            Assert.AreEqual(false, kP1.Selected);
            Assert.AreEqual("2", kP1.DisplayOrder);

            KeyValuePair skP2 = mS.KeyValuePairList[2];
            Assert.AreEqual("3", kP2.Key);
            Assert.AreEqual("Valgmulighed 3", kP2.Value);
            Assert.AreEqual(false, kP2.Selected);
            Assert.AreEqual("3", kP2.DisplayOrder);

            KeyValuePair skP3 = mS.KeyValuePairList[3];
            Assert.AreEqual("4", kP3.Key);
            Assert.AreEqual("Valgmulighed 4", kP3.Value);
            Assert.AreEqual(false, kP3.Selected);
            Assert.AreEqual("4", kP3.DisplayOrder);

            KeyValuePair skP4 = mS.KeyValuePairList[4];
            Assert.AreEqual("5", kP4.Key);
            Assert.AreEqual("Valgmulighed 5", kP4.Value);
            Assert.AreEqual(false, kP4.Selected);
            Assert.AreEqual("5", kP4.DisplayOrder);

            KeyValuePair skP5 = mS.KeyValuePairList[5];
            Assert.AreEqual("6", kP5.Key);
            Assert.AreEqual("Valgmulighed 6", kP5.Value);
            Assert.AreEqual(false, kP5.Selected);
            Assert.AreEqual("6", kP5.DisplayOrder);

            KeyValuePair skP6 = mS.KeyValuePairList[6];
            Assert.AreEqual("7", kP6.Key);
            Assert.AreEqual("Valgmulighed 7", kP6.Value);
            Assert.AreEqual(false, kP6.Selected);
            Assert.AreEqual("7", kP6.DisplayOrder);

            KeyValuePair skP7 = mS.KeyValuePairList[7];
            Assert.AreEqual("8", kP7.Key);
            Assert.AreEqual("Valgmulighed 8", kP7.Value);
            Assert.AreEqual(false, kP7.Selected);
            Assert.AreEqual("8", kP7.DisplayOrder);

            KeyValuePair skP8 = mS.KeyValuePairList[8];
            Assert.AreEqual("9", kP8.Key);
            Assert.AreEqual("Valgmulighed 9", kP8.Value);
            Assert.AreEqual(false, kP8.Selected);
            Assert.AreEqual("9", kP8.DisplayOrder);

            KeyValuePair skP9 = mS.KeyValuePairList[9];
            Assert.AreEqual("Valgmulighed N", kP9.Value);
            Assert.AreEqual(false, kP9.Selected);
            Assert.AreEqual("10", kP9.DisplayOrder);

            Date d1 = (Date)dE.DataItemList[7];
            Assert.AreEqual("DATO", d1.Label);
            // Assert.AreEqual("Vælg dato<br><br>Er Microting eForm integreret med ERP-system, kan valgt dato leveres direkte i ERP-system]]></", cB.Description); TODO
            Assert.AreEqual(8, d1.DisplayOrder);
            // Assert.AreEqual("2016-06-09", d1.MinValue); TODO
            // Assert.AreEqual("2026-06-09", d1.MaxValue); TODO
            Assert.AreEqual(false, d1.Mandatory);
            Assert.AreEqual(false, d1.ReadOnly);

            Number n2 = (Number)dE.DataItemList[8];
            Assert.AreEqual("INDTAST TAL", n2.Label);
            // Assert.AreEqual("Indtast tal og opsæt evt. regler for mindste/højeste tilladte værdi.<br><br>Er Microting eForm integreret med ERP-system, sendes de indtastede værdier direkte til ERP-systemet]></", cB.Description); TODO
            Assert.AreEqual(9, n2.DisplayOrder);
            //         Assert.AreEqual("", n2.MinValue);
            //         Assert.AreEqual("", n2.MaxValue);
            //         Assert.AreEqual("", n2.DecimalCount);
            //         Assert.AreEqual("", n2.UnitName);
            Assert.AreEqual(false, d1.Mandatory);

            Text tt1 = (Text)dE.DataItemList[9];
            Assert.AreEqual("SKRIV KORT KOMMENTAR", tt1.Label);
            // Assert.AreEqual(" Skriv kort kommentar uden linieskift]></", cB.Description); TODO
            Assert.AreEqual(10, tt1.DisplayOrder);
            // Assert.AreEqual(0, tt1.multi) TODO
            Assert.AreEqual(false, tt1.GeolocationEnabled);
            // Assert.AreEqual(false, tt1.split) TODO
            // Assert.AreEqual("", tt1.Value); TODO
            Assert.AreEqual(false, tt1.ReadOnly);
            Assert.AreEqual(false, tt1.Mandatory);

            Picture pp1 = (Picture)dE.DataItemList[10];
            Assert.AreEqual("FOTO", pp1.Label);
            // Assert.AreEqual("Er Microting eForm integreret med ERP-system, kan billederne vises direkte i virksomhedens ERP/andet databasesystem.", pp1.Description);
            Assert.AreEqual(11, pp1.DisplayOrder);
            Assert.AreEqual(false, pp1.Mandatory);

            Comment cc1 = (Comment)dE.DataItemList[11];
            Assert.AreEqual("SKRIV LANG KOMMENTAR", cc1.Label);
            // Assert.AreEqual("Skriv længere kommentar med mulighed for linieskift.", cc1.Description);
            Assert.AreEqual(12, cc1.DisplayOrder);
            // Assert.AreEqual(1, cc1.multi);
            // Assert.AreEqual(false, cc1.geolocation);
            Assert.AreEqual("", cc1.Value);
            Assert.AreEqual(false, cc1.ReadOnly);
            Assert.AreEqual(false, cc1.Mandatory);

            Signature ss1 = (Signature)dE.DataItemList[12];
            Assert.AreEqual("UNDERSKRIFT", ss1.Label);
            // Assert.AreEqual("Underskrift<br><br>Er Microting eForm integreret med ERP-system, kan underskrifterne sendes direkte til ERP/andet databasesystem.", ss1.Description);
            Assert.AreEqual(13, ss1.DisplayOrder);
            Assert.AreEqual(false, ss1.Mandatory);

            SaveButton ssB = (SaveButton)dE.DataItemList[13];
            Assert.AreEqual("GEM", ssB.Label);
            // Assert.AreEqual("Tryk for at gemme data.<br>Press to save data.", ssB.Description);
            Assert.AreEqual(14, ssB.DisplayOrder);
            Assert.AreEqual("GEM/SAVE", ssB.Value);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Multiselect", match.Label);
            // Assert.AreEqual("2017-01-22", match.StartDate); TODO
            // Assert.AreEqual("2027-01-22", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(2, dE.DataItemList.Count());

            Assert.AreEqual("Multiselect", dE.Label);
            // Assert.AreEqual(CDataValue, dE.Label); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(false, dE.DoneButtondisabled); //TODO
            Assert.AreEqual(false, dE.ApprovalEnabled);

            MultiSelect mS = (MultiSelect)dE.DataItemList[0];


            Assert.AreEqual("Flere valg", mS.Label);
            // Assert.AreEqual(CDataValue, de.description)"); //TODO
            Assert.AreEqual(0, mS.DisplayOrder);
            Assert.AreEqual(false, mS.Mandatory);

            KeyValuePair kP = mS.KeyValuePairList[0];
            Assert.AreEqual("1", kP.Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kP.Selected);
            Assert.AreEqual("1", kP.DisplayOrder);

            KeyValuePair kP2 = mS.KeyValuePairList[1];
            Assert.AreEqual("2", kP2.Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kP2.Selected);
            Assert.AreEqual("2", kP2.DisplayOrder);

            KeyValuePair kP3 = mS.KeyValuePairList[2];
            Assert.AreEqual("3", kP3.Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kP3.Selected);
            Assert.AreEqual("3", kP3.DisplayOrder);

            KeyValuePair kP4 = mS.KeyValuePairList[3];
            Assert.AreEqual("4", kP4.Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kP4.Selected);
            Assert.AreEqual("4", kP4.DisplayOrder);

            SingleSelect sS = (SingleSelect)dE.DataItemList[1];
            Assert.AreEqual("Choose one option", sS.Label);
            // Assert.AreEqual("This is a description", sS.Description) //TODO
            Assert.AreEqual(1, sS.DisplayOrder);
            Assert.AreEqual(false, sS.Mandatory);

            KeyValuePair kkP = sS.KeyValuePairList[0];
            Assert.AreEqual("1", kkP.Key);
            // Assert.AreEqual(CData, kkP.Value); TODO
            Assert.AreEqual(false, kkP.Selected);
            Assert.AreEqual("1", kkP.DisplayOrder);

            KeyValuePair kkP2 = sS.KeyValuePairList[1];
            Assert.AreEqual("2", kkP2.Key);
            // Assert.AreEqual(CData, kkP2.Value); TODO
            Assert.AreEqual(false, kkP2.Selected);
            Assert.AreEqual("2", kkP2.DisplayOrder);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Single Select", match.Label);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());

            Assert.AreEqual("Single Select", dE.Label);
            // Assert.AreEqual(CDataValue, dE.Label); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(false, dE.DoneButtondisabled); //TODO
            Assert.AreEqual(false, dE.ApprovalEnabled);

            SingleSelect sS = (SingleSelect)dE.DataItemList[0];


            Assert.AreEqual("Single Select 1", sS.Label);
            // Assert.AreEqual(CDataValue, ss.description)"); //TODO
            Assert.AreEqual(0, sS.DisplayOrder);
            Assert.AreEqual(false, sS.Mandatory);

            KeyValuePair kP = sS.KeyValuePairList[0];
            Assert.AreEqual("1", kP.Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kP.Selected);
            Assert.AreEqual("1", kP.DisplayOrder);

            KeyValuePair kP2 = sS.KeyValuePairList[1];
            Assert.AreEqual("2", kP2.Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kP2.Selected);
            Assert.AreEqual("2", kP2.DisplayOrder);

            KeyValuePair kP3 = sS.KeyValuePairList[2];
            Assert.AreEqual("3", kP3.Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kP3.Selected);
            Assert.AreEqual("3", kP3.DisplayOrder);

            KeyValuePair kP4 = sS.KeyValuePairList[3];
            Assert.AreEqual("4", kP4.Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kP4.Selected);
            Assert.AreEqual("4", kP4.DisplayOrder);

            KeyValuePair kP5 = sS.KeyValuePairList[4];
            Assert.AreEqual("5", kP5.Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kP5.Selected);
            Assert.AreEqual("5", kP5.DisplayOrder);

            KeyValuePair kP6 = sS.KeyValuePairList[5];
            Assert.AreEqual("6", kP6.Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kP6.Selected);
            Assert.AreEqual("6", kP6.DisplayOrder);

            KeyValuePair kP7 = sS.KeyValuePairList[6];
            Assert.AreEqual("7", kP7.Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kP7.Selected);
            Assert.AreEqual("7", kP7.DisplayOrder);

            KeyValuePair kP8 = sS.KeyValuePairList[7];
            Assert.AreEqual("8", kP8.Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kP8.Selected);
            Assert.AreEqual("8", kP8.DisplayOrder);

            Assert.AreEqual(Constants.FieldColors.Default, sS.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("comment", match.Label);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());

            Assert.AreEqual("comment", dE.Label);
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.AreEqual(false, dE.ApprovalEnabled);

            Comment cc = (Comment)dE.DataItemList[0];

            Assert.AreEqual("Comment", cc.Label);
            // Assert.AreEqual(CDataValue, cc.Description); //todo
            Assert.AreEqual(0, cc.DisplayOrder);
            // Assert.AreEqual(1, cc.multi) //TODO
            // Assert.AreEqual(false, cc.geolocation) //todo
            // Assert.AreEqual(false, cc.split) //TODO
            Assert.AreEqual("", cc.Value);
            Assert.AreEqual(false, cc.ReadOnly);
            Assert.AreEqual(false, cc.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, cc.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Single line", match.Label);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());

            Assert.AreEqual("Single line", dE.Label);
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.AreEqual(false, dE.ApprovalEnabled);

            Text t = (Text)dE.DataItemList[0];

            Assert.AreEqual("Single line 1", t.Label);
            // Assert.AreEqual(CDataValue, t.Description); //TODO
            Assert.AreEqual(0, t.DisplayOrder);
            // Assert.AreEqual(0, t.multi) //TODO
            Assert.AreEqual(false, t.GeolocationEnabled);
            // Assert.AreEqual(false, t.split) //TODO
            Assert.AreEqual("", t.Value);
            Assert.AreEqual(false, t.ReadOnly);
            Assert.AreEqual(false, t.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, t.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Number 1", match.Label);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());

            Assert.AreEqual("Number 1", dE.Label);
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.AreEqual(false, dE.ApprovalEnabled);

            Number n = (Number)dE.DataItemList[0];

            Assert.AreEqual("Number 1", n.Label);
            // Assert.AreEqual(CDataValue, t.Description); //TODO
            Assert.AreEqual(0, n.DisplayOrder);
            Assert.AreEqual("1", n.MinValue);
            Assert.AreEqual("1100", n.MaxValue);
            // Assert.AreEqual(24, n.value) //TODO
            Assert.AreEqual(2, n.DecimalCount);
            Assert.AreEqual(true, n.Mandatory);
            Assert.AreEqual("", n.UnitName);
            Assert.AreEqual(Constants.FieldColors.Default, n.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Info box", match.Label);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());

            Assert.AreEqual("Info box", dE.Label);
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.AreEqual(false, dE.ApprovalEnabled);

            None n = (None)dE.DataItemList[0];

            Assert.AreEqual("Info box 1", n.Label);
            // Assert.AreEqual(CDataValue, t.Description); //TODO
            Assert.AreEqual(0, n.DisplayOrder);
            Assert.AreEqual(Constants.FieldColors.Default, n.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("checkbox", match.Label);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());

            Assert.AreEqual("checkbox", dE.Label);
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.AreEqual(false, dE.ApprovalEnabled);

            CheckBox cB = (CheckBox)dE.DataItemList[0];

            Assert.AreEqual("Checkbox 1", cB.Label);
            // Assert.AreEqual(CDataValue, t.Description); //TODO
            Assert.AreEqual(0, cB.DisplayOrder);
            Assert.AreEqual(false, cB.Selected);
            Assert.AreEqual(false, cB.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, cB.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("TimerStartStop", match.Label);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());

            Assert.AreEqual("TimerStartStop", dE.Label);
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.AreEqual(false, dE.ApprovalEnabled);

            Timer t = (Timer)dE.DataItemList[0];

            Assert.AreEqual("Timer Start Stop 1", t.Label);
            // Assert.AreEqual(CDataValue, t.Description); //TODO
            Assert.AreEqual(0, t.DisplayOrder);
            Assert.AreEqual(false, t.StopOnSave);
            Assert.AreEqual(false, t.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, t.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Save button", match.Label);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());

            Assert.AreEqual("Save button", dE.Label);
            // Assert.AreEqual(CDataValue, dE.Description); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.AreEqual(false, dE.ApprovalEnabled);

            SaveButton sB = (SaveButton)dE.DataItemList[0];

            Assert.AreEqual("Save button 1", sB.Label);
            // Assert.AreEqual(CDataValue, t.Description); //TODO
            Assert.AreEqual(0, sB.DisplayOrder);
            Assert.AreEqual("", sB.Value);
            Assert.AreEqual(Constants.FieldColors.Default, sB.Color);
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
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("MultiLvlTest", match.Label);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            GroupElement gE = (GroupElement)match.ElementList[0];
            Assert.AreEqual(3, gE.ElementList.Count());

            Assert.AreEqual("1 lvl", gE.Label);
            // Assert.AreEqual("1 lvl description", gE.Description); //TODO
            Assert.AreEqual(0, gE.DisplayOrder);

            DataElement dE = (DataElement)gE.ElementList[0];
            Assert.AreEqual("1.1 lvl", dE.Label);
            // Assert.AreEqual("1.1 lvl description", de.description); TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.AreEqual(false, dE.ApprovalEnabled);

            CheckBox cB = (CheckBox)dE.DataItemList[0];
            Assert.AreEqual("1.1 lvl checkbox", cB.Label);
            // Assert.AreEqual("1.1 lvl cehckbox description", cB.Description) //TODO
            Assert.AreEqual(0, cB.DisplayOrder);
            Assert.AreEqual(false, cB.Selected);
            Assert.AreEqual(false, cB.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, cB.Color);

            DataElement dE2 = (DataElement)gE.ElementList[1];
            Assert.AreEqual("1.2 lvl", dE2.Label);
            // Assert.AreEqual("1.2 lvl description", dE2.Description); //TODO
            Assert.AreEqual(1, dE2.DisplayOrder);
            Assert.AreEqual(false, dE2.ReviewEnabled);
            // Assert.AreEqual(false, dE2.manualsync)//TODO
            Assert.AreEqual(false, dE2.ExtraFieldsEnabled);
            // Assert.AreEqual(false, dE2.DoneButtonDisabled); //TODO
            Assert.AreEqual(false, dE2.ApprovalEnabled);

            CheckBox cb2 = (CheckBox)dE2.DataItemList[0];
            Assert.AreEqual("1.2 lvl checkbox", cb2.Label);
            // Assert.AreEqual("1.2 lvl checkbox description", cb2.Description) //TODO
            Assert.AreEqual(0, cb2.DisplayOrder);
            Assert.AreEqual(false, cb2.Selected);
            Assert.AreEqual(false, cb2.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, cb2.Color);

            GroupElement gE2 = (GroupElement)gE.ElementList[2];
            Assert.AreEqual("1.3 lvl", gE2.Label);
            // Assert.AreEqual("1.3 lvl description", gE2.Description); //TODO
            Assert.AreEqual(2, gE2.DisplayOrder);

            DataElement de3 = (DataElement)gE2.ElementList[0];
            Assert.AreEqual("1.3.1 lvl", de3.Label);
            // Assert.AreEqual("1.3.1.1 lvl description", de3.Description) //TODO
            Assert.AreEqual(0, cb2.DisplayOrder);
            Assert.AreEqual(false, de3.ReviewEnabled);
            // Assert.AreEqual(false, de3.manualsync);//TODO
            Assert.AreEqual(false, de3.ExtraFieldsEnabled);
            // Assert.AreEqual(false, de3.donebuttondisabled)//TODO
            Assert.AreEqual(false, de3.ApprovalEnabled);

            CheckBox cb3 = (CheckBox)de3.DataItemList[0];
            Assert.AreEqual("1.3.1 lvl checkbox", cb3.Label);
            // Assert.AreEqual("1.2 lvl checkbox description", cb2.Description) //TODO
            Assert.AreEqual(0, cb3.DisplayOrder);
            Assert.AreEqual(false, cb3.Selected);
            Assert.AreEqual(false, cb3.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Default, cb3.Color);
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