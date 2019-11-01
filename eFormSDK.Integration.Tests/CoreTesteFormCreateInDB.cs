using eFormCore;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Models;
using KeyValuePair = Microting.eForm.Dto.KeyValuePair;
using System.IO;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class CoreTesteFormCreateInDB : DbTestFixture
    {
        private Core sut;
        private TestHelpers testHelpers;
        private string path;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
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
            path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:", "");
            await sut.SetSdkSetting(Settings.fileLocationPicture, Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers();
            //await sut.StartLog(new CoreBase());
        }

        [Test] // Core_Template_TemplateFromXml_ReturnsTemplate()
        public async Task Core_eForm_SimpleCommenteFormCreateInDB_ReturnseFormId()
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
            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement);
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual("", cl[0].CaseType);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("CommentMain", cl[0].Label);
            // Assert.AreEqual("da", match.Language);
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            Assert.AreEqual(0, cl[0].DisplayIndex);
            // Assert.AreEqual(1, match.ElementList.Count());
            //DataElement dE = (DataElement)cl[1];
            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("CommentDataElement", cl[1].Label);

            //CDataValue cd = new CDataValue();

            Assert.AreEqual("CommentDataElementDescription", cl[1].Description); 
            Assert.AreEqual(0, cl[1].DisplayIndex);
            Assert.AreEqual(0, cl[1].ReviewEnabled); //False
            // Assert.AreEqual(0, cl[1].manual_sync); //false TODO was null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //False
            // Assert.AreEqual(0, cl[1].done_button_Disabled); //TODO no method
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //False

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            //Comment commentField = (Comment)dE.DataItemList[0];
            Assert.AreEqual("CommentField", _fields[0].Label);
            Assert.AreEqual("CommentFieldDescription", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            // Assert.AreEqual(1, _fields[0].multi); was null TODO
            // Assert.AreEqual(0, _fields[0].geolocation_enabled); //False TODO was null
            Assert.AreEqual(0, _fields[0].SplitScreen); //TODO no method Split
            Assert.AreEqual("", _fields[0].DefaultValue);
            Assert.AreEqual(0, _fields[0].ReadOnly); //false
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);


        }

        [Test]
        public async Task Core_eForm_SimplePictureFormCreateInDB_ReturnseFormId()
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
            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement);
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Picture test", cl[0].Label);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval);//false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("Picture test", cl[1].Label);
            Assert.AreEqual("", cl[1].Description); 
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Take two pictures", _fields[0].Label);
            Assert.AreEqual("", _fields[0].Description); 
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);

        }

        [Test]
        public async Task Core_eForm_SimpleDateFormCreateInDB_ReturnseFormId()
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


            // Act
            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement);
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Date", cl[0].Label);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval);//false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("Date", cl[1].Label);
            Assert.AreEqual("", cl[1].Description); 
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Select date", _fields[0].Label);
            Assert.AreEqual("", _fields[0].Description);
            // Assert.AreEqual("2018-01-18", _fields.minvalue) //todo
            // Assert.AreEqual("2028-01-18", _fields.maxvalue) //todo
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            // Assert.AreEqual("", _fields[0].value); //TODO
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(0, _fields[0].ReadOnly); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);

        }

        [Test]
        public async Task Core_eForm_SimplePdfFormCreateInDB_ReturnseFormId()
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


            // Act
            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            mainElement = await sut.TemplateUploadData(mainElement);
            int eFormId = await sut.TemplateCreate(mainElement); 
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("ny pdf", cl[0].Label);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval);//false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("ny pdf", cl[1].Label);
            Assert.AreEqual("", cl[1].Description); 
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("bla", _fields[0].Label);
            Assert.AreEqual("", _fields[0].Description); 
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual("20d483dd7791cd6becf089432724c663", _fields[0].DefaultValue); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);


        }

        [Test]
        public async Task Core_eForm_SimpleFieldGroupsFormCreateInDB_ReturnseFormId()
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


            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement); 
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();


            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Tester grupper", cl[0].Label);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); //TODO
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex);


            Assert.AreEqual(2, cl.Count());            
            Assert.AreEqual("Tester grupper", cl[1].Label);
            Assert.AreEqual("", cl[1].Description); 
            Assert.AreEqual(0, cl[1].DisplayIndex);
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false


            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);

            Assert.AreEqual("Gruppe efter tjekboks", _fields[0].Label);
            Assert.AreEqual("", _fields[0].Description); 
            Assert.AreEqual(1, _fields[0].DisplayIndex);
            Assert.AreEqual("Closed", _fields[0].DefaultValue); 
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);


            Assert.AreEqual("Tjekboks inde i gruppe", _fields[1].Label);
            Assert.AreEqual("", _fields[1].Description);
            Assert.AreEqual(0, _fields[1].DisplayIndex);
            Assert.AreEqual(0, _fields[1].Selected); //false
            Assert.AreEqual(0, _fields[1].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);


            
            Assert.AreEqual("Tjekboks før gruppe", _fields[2].Label);
            Assert.AreEqual("", _fields[2].Description);
            Assert.AreEqual(0, _fields[2].DisplayIndex);
            Assert.AreEqual(0, _fields[2].Selected); //false
            Assert.AreEqual(0, _fields[2].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[2].Color);


            
            Assert.AreEqual("Tjekboks efter gruppe", _fields[3].Label);
            Assert.AreEqual("", _fields[3].Description);
            Assert.AreEqual(2, _fields[3].DisplayIndex);
            Assert.AreEqual(0, _fields[3].Selected); //false
            Assert.AreEqual(0, _fields[3].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[3].Color);


        }

        [Test]
        public async Task Core_eForm_SimplePictureAndSignatureFormCreateInDB_ReturnseFormId()
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


            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement); 
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();


            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Billede og signatur", cl[0].Label);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].Language); //todo
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex);


            Assert.AreEqual(2, cl.Count());
            Assert.AreEqual("Billede og signatur", cl[1].Label);
            Assert.AreEqual("", cl[1].Description); 
            Assert.AreEqual(0, cl[1].DisplayIndex);
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Tag et billede", _fields[0].Label);
            Assert.AreEqual("", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);

         
            Assert.AreEqual("Skriv", _fields[1].Label);
            Assert.AreEqual("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam dui neque, molestie at maximus a, malesuada at mi. Cras venenatis porttitor augue nec sagittis. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris urna massa, sagittis at fringilla ut, convallis sed dolor. Praesent scelerisque magna dolor, quis blandit metus pharetra eu. Cras euismod facilisis risus at ullamcorper. Pellentesque vitae maximus elit. Sed scelerisque nec velit dictum sodales. Duis sed dapibus odio. Sed non luctus sem. Donec eu mollis lectus, nec porta nisl. Aenean a consequat metus, ac auctor arcu. Cras sit amet blandit velit. Pellentesque faucibus eros sed ullamcorper rutrum.<br><br><br>Pellentesque ultrices ex erat. Pellentesque rhoncus eget lectus et scelerisque. Cras vitae diam ex. Ut felis ligula, venenatis ut lorem vel, venenatis convallis turpis. Sed rutrum ac odio ac auctor. Sed mauris ipsum, vulputate ut sodales a, mattis et purus. Nam convallis augue velit, nec blandit ipsum porta vitae. Quisque et iaculis lectus. Donec eu fringilla turpis, id rutrum mauris.<br><br><br>Proin eu sagittis sem. Aenean vel placerat sapien. Praesent et rutrum justo. Mauris consectetur venenatis est, eu vulputate enim elementum eget. In hac habitasse platea dictumst. Sed vehicula nec neque sed posuere. Aenean sodales lectus a purus posuere lacinia. Aenean ut enim vel odio varius placerat. Phasellus faucibus turpis sed arcu ultrices interdum. Sed porta, nisi nec vehicula lacinia, ante tortor tristique justo, vel sagittis felis ligula eu magna. Pellentesque a velit laoreet nunc aliquet ornare sit amet eget lorem. Duis aliquet viverra pretium. Etiam a mauris tellus. Sed viverra eros eget lectus lobortis, in vestibulum lorem rhoncus. Aliquam sem felis, suscipit a gravida ut, eleifend et ipsum. Nullam lacus lacus, rutrum quis sollicitudin et, porta et erat.", _fields[1].Description);
            Assert.AreEqual(1, _fields[1].DisplayIndex);
            Assert.AreEqual(0, _fields[1].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[1].Color);


        }

        [Test]
        public async Task Core_eForm_OptionsWithMicrotingFormCreateInDB_ReturnseFormId()
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

            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            mainElement = await sut.TemplateUploadData(mainElement);
            int eFormId = await sut.TemplateCreate(mainElement); //TODO
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();


            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Muligheder med Microting eForm", cl[0].Label);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); todo
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(76, cl[0].DisplayIndex);

            Assert.AreEqual(2, cl.Count());
            Assert.AreEqual("Muligheder med Microting eForm", cl[1].Label);
            Assert.AreEqual("Tryk her og prøv hvordan du indsamler data med Microting eForm.<br><br><br>God fornøjelse :-)", cl[1].Description);
            Assert.AreEqual(76, cl[1].DisplayIndex);
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(dE.DoneButtonDisabled, true); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);


            Assert.AreEqual("GEM", _fields[0].Label);
            Assert.AreEqual("Tryk her for at gemme dine indtastede data<br><br>", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual("GEM", _fields[0].DefaultValue);

           
            Assert.AreEqual("START-STOP TID", _fields[1].Label);
            Assert.AreEqual("Start-stop tid.<br><br>", _fields[1].Description);
            Assert.AreEqual(1, _fields[1].DisplayIndex);
            // Assert.AreEqual(0, _fields[1].stop_on_save);//false TODO
            Assert.AreEqual(0, _fields[1].Mandatory); //false

            
            Assert.AreEqual("INFO", _fields[2].Label);
            Assert.AreEqual("I dette tekstfelt vises ikke redigerbar tekst.<br><br>Er Microting eForm integreret med ERP-system, kan data fra ERP-systemet vises i dette felt fx. baggrundsinformation på kunder.<br>", _fields[2].Description); 
            Assert.AreEqual(2, _fields[2].DisplayIndex);

            
            Assert.AreEqual("PDF", _fields[3].Label);
            Assert.AreEqual("Her vises PDF-filer.<br>", _fields[3].Description);  
            Assert.AreEqual(3, _fields[3].DisplayIndex);
            Assert.AreEqual("a60ad2d8c22ed24780bfa9a348376232", _fields[3].DefaultValue);

        
            Assert.AreEqual("TJEK", _fields[4].Label);
            Assert.AreEqual("I et tjekfelt sættes et flueben.<br>", _fields[4].Description); 
            Assert.AreEqual(5, _fields[4].DisplayIndex);
            Assert.AreEqual(0, _fields[4].Selected); //false
            Assert.AreEqual(0, _fields[4].Mandatory); //false

            
            Assert.AreEqual("VÆLG", _fields[5].Label);
            Assert.AreEqual("Vælg én eller flere i liste.<br><br>Er Microting eForm integreret med ERP-system, kan valgmulighederne komme derfra.<br>", _fields[5].Description); 
            Assert.AreEqual(6, _fields[5].DisplayIndex);
            Assert.AreEqual(0, _fields[5].Mandatory); //false

            List<KeyValuePair> kvp = sut.PairRead(_fields[5].KeyValuePairList);
            Assert.AreEqual("1", kvp[0].DisplayOrder);
            Assert.AreEqual("Valgmulighed 1", kvp[0].Value);
            Assert.AreEqual(false, kvp[0].Selected); //false
            Assert.AreEqual("1", kvp[0].Key);

            
            Assert.AreEqual("2", kvp[1].DisplayOrder);
            Assert.AreEqual("Valgmulighed 2", kvp[1].Value);
            Assert.AreEqual(false, kvp[1].Selected); //false
            Assert.AreEqual("2", kvp[1].Key);

            
            Assert.AreEqual("3", kvp[2].DisplayOrder);
            Assert.AreEqual("Valgmulighed 3", kvp[2].Value);
            Assert.AreEqual(false, kvp[2].Selected); //false
            Assert.AreEqual("3", kvp[2].Key);

            
            Assert.AreEqual("4", kvp[3].DisplayOrder);
            Assert.AreEqual("Valgmulighed 4", kvp[3].Value);
            Assert.AreEqual(false, kvp[3].Selected); //false
            Assert.AreEqual("4", kvp[3].Key);

            
            Assert.AreEqual("5", kvp[4].DisplayOrder);
            Assert.AreEqual("Valgmulighed 5", kvp[4].Value);
            Assert.AreEqual(false, kvp[4].Selected); //false
            Assert.AreEqual("5", kvp[4].Key);

            
            Assert.AreEqual("6", kvp[5].DisplayOrder);
            Assert.AreEqual("Valgmulighed 6", kvp[5].Value);
            Assert.AreEqual(false, kvp[5].Selected); //false
            Assert.AreEqual("6", kvp[5].Key);

            Assert.AreEqual("7", kvp[6].DisplayOrder);
            Assert.AreEqual("Valgmulighed 7", kvp[6].Value);
            Assert.AreEqual(false, kvp[6].Selected); //false
            Assert.AreEqual("7", kvp[6].Key);

            Assert.AreEqual("8", kvp[7].DisplayOrder);
            Assert.AreEqual("Valgmulighed 8", kvp[7].Value);
            Assert.AreEqual(false, kvp[7].Selected); //false
            Assert.AreEqual("8", kvp[7].Key);

            Assert.AreEqual("9", kvp[8].DisplayOrder);
            Assert.AreEqual("Valgmulighed 9", kvp[8].Value);
            Assert.AreEqual(false, kvp[8].Selected); //false
            Assert.AreEqual("9", kvp[8].Key);

            Assert.AreEqual("10", kvp[9].DisplayOrder);
            Assert.AreEqual("Valgmulighed N", kvp[9].Value);
            Assert.AreEqual(false, kvp[9].Selected); //false
            Assert.AreEqual("10", kvp[9].Key);


            Assert.AreEqual("VÆLG ÉN", _fields[6].Label);
            Assert.AreEqual("Vælg én blandt flere valgmuligheder.<br><br>Er Microting eForm integreret med ERP-system, kan valgmulighederne komme derfra.", _fields[6].Description); 
            Assert.AreEqual(7, _fields[6].DisplayIndex);
            Assert.AreEqual(0, _fields[6].Mandatory); //false


            List<KeyValuePair> kvpp1 = sut.PairRead(_fields[6].KeyValuePairList);
            Assert.AreEqual("1", kvpp1[0].Key);
            Assert.AreEqual("Valgmulighed 1", kvpp1[0].Value);
            Assert.AreEqual(false, kvpp1[0].Selected); //false
            Assert.AreEqual("1", kvpp1[0].DisplayOrder);

            Assert.AreEqual("2", kvpp1[1].DisplayOrder);
            Assert.AreEqual("Valgmulighed 2", kvpp1[1].Value);
            Assert.AreEqual(false, kvpp1[1].Selected); //false
            Assert.AreEqual("2", kvpp1[1].Key);


            Assert.AreEqual("3", kvpp1[2].DisplayOrder);
            Assert.AreEqual("Valgmulighed 3", kvpp1[2].Value);
            Assert.AreEqual(false, kvpp1[2].Selected); //false
            Assert.AreEqual("3", kvpp1[2].Key);


            Assert.AreEqual("4", kvpp1[3].DisplayOrder);
            Assert.AreEqual("Valgmulighed 4", kvpp1[3].Value);
            Assert.AreEqual(false, kvpp1[3].Selected); //false
            Assert.AreEqual("4", kvpp1[3].Key);


            Assert.AreEqual("5", kvpp1[4].DisplayOrder);
            Assert.AreEqual("Valgmulighed 5", kvpp1[4].Value);
            Assert.AreEqual(false, kvpp1[4].Selected); //false
            Assert.AreEqual("5", kvpp1[4].Key);


            Assert.AreEqual("6", kvpp1[5].DisplayOrder);
            Assert.AreEqual("Valgmulighed 6", kvpp1[5].Value);
            Assert.AreEqual(false, kvpp1[5].Selected); //false
            Assert.AreEqual("6", kvpp1[5].Key);

            Assert.AreEqual("7", kvpp1[6].DisplayOrder);
            Assert.AreEqual("Valgmulighed 7", kvpp1[6].Value);
            Assert.AreEqual(false, kvpp1[6].Selected); //false
            Assert.AreEqual("7", kvpp1[6].Key);

            Assert.AreEqual("8", kvpp1[7].DisplayOrder);
            Assert.AreEqual("Valgmulighed 8", kvpp1[7].Value);
            Assert.AreEqual(false, kvpp1[7].Selected); //false
            Assert.AreEqual("8", kvpp1[7].Key);

            Assert.AreEqual("9", kvpp1[8].DisplayOrder);
            Assert.AreEqual("Valgmulighed 9", kvpp1[8].Value);
            Assert.AreEqual(false, kvpp1[8].Selected); //false
            Assert.AreEqual("9", kvpp1[8].Key);

            Assert.AreEqual("10", kvpp1[9].DisplayOrder);
            Assert.AreEqual("Valgmulighed N", kvpp1[9].Value);
            Assert.AreEqual(false, kvpp1[9].Selected); //false
            Assert.AreEqual("10", kvpp1[9].Key);



            Assert.AreEqual("DATO", _fields[7].Label);
            Assert.AreEqual("Vælg dato<br><br>Er Microting eForm integreret med ERP-system, kan valgt dato leveres direkte i ERP-system.<br>", _fields[7].Description); 
            Assert.AreEqual(8, _fields[7].DisplayIndex);
            // Assert.AreEqual("2016-06-09", d1.MinValue); TODO
            // Assert.AreEqual("2026-06-09", d1.MaxValue); TODO
            Assert.AreEqual(0, _fields[7].Mandatory); //false
            Assert.AreEqual(0, _fields[7].ReadOnly); //false

            
            Assert.AreEqual("INDTAST TAL", _fields[8].Label);
            Assert.AreEqual("Indtast tal og opsæt evt. regler for mindste/højeste tilladte værdi.<br><br>Er Microting eForm integreret med ERP-system, sendes de indtastede værdier direkte til ERP-systemet.<br>", _fields[8].Description); 
            Assert.AreEqual(9, _fields[8].DisplayIndex);
            // Assert.AreEqual("", _fields[8].min_value); todo
            // Assert.AreEqual("", _fields[8].max_value); todo
            Assert.AreEqual(0, _fields[8].DecimalCount);
            Assert.AreEqual("", _fields[8].UnitName);      
            Assert.AreEqual(0, _fields[8].Mandatory); //false

            
            Assert.AreEqual("SKRIV KORT KOMMENTAR", _fields[9].Label);
            Assert.AreEqual("Skriv kort kommentar uden linieskift.", _fields[9].Description); 
            Assert.AreEqual(10, _fields[9].DisplayIndex);
            // Assert.AreEqual(0, _fields[9].multi); todo
            Assert.AreEqual(0, _fields[9].GeolocationEnabled); //false
            // Assert.AreEqual(0, _fields[9].split_screen); todo
            Assert.AreEqual("", _fields[9].DefaultValue); 
            Assert.AreEqual(0, _fields[9].ReadOnly); //false
            Assert.AreEqual(0, _fields[9].Mandatory); //false

            
            Assert.AreEqual("FOTO", _fields[10].Label);
            Assert.AreEqual("Tag billeder<br><br>Er Microting eForm integreret med ERP-system, kan billederne vises direkte i virksomhedens ERP/andet databasesystem.", _fields[10].Description);
            Assert.AreEqual(11, _fields[10].DisplayIndex);
            Assert.AreEqual(0, _fields[10].Mandatory); //false

            
            Assert.AreEqual("SKRIV LANG KOMMENTAR", _fields[11].Label);
            Assert.AreEqual("Skriv længere kommentar med mulighed for linieskift.", _fields[11].Description);
            Assert.AreEqual(12, _fields[11].DisplayIndex);
            // Assert.AreEqual(1, cc1.multi);
            // Assert.AreEqual(false, cc1.geolocation);
            Assert.AreEqual("", _fields[11].DefaultValue);
            Assert.AreEqual(0, _fields[11].ReadOnly); //false
            Assert.AreEqual(0, _fields[11].Mandatory); //false

            
            Assert.AreEqual("UNDERSKRIFT", _fields[12].Label);
            Assert.AreEqual("Underskrift<br><br>Er Microting eForm integreret med ERP-system, kan underskrifterne sendes direkte til ERP/andet databasesystem.", _fields[12].Description);
            Assert.AreEqual(13, _fields[12].DisplayIndex);
            Assert.AreEqual(0, _fields[12].Mandatory); //false

           
            Assert.AreEqual("GEM", _fields[13].Label);
            Assert.AreEqual("<br>Tryk for at gemme data.<br>Press to save data.<br>", _fields[13].Description);
            Assert.AreEqual(14, _fields[13].DisplayIndex);
            Assert.AreEqual("GEM/SAVE", _fields[13].DefaultValue);
        }

        [Test]
        public async Task Core_eForm_SimpleMultiSelectFormCreateInDB_ReturnseFormId()
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

            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement); 
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();


            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Multiselect", cl[0].Label);
            // Assert.AreEqual("2017-01-22", match.StartDate); TODO
            // Assert.AreEqual("2027-01-22", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].Language); todo
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex);

            Assert.AreEqual(2, cl.Count());
            

            Assert.AreEqual("Multiselect", cl[1].Label);
            // Assert.AreEqual(CDataValue, dE.Label); //TODO
            Assert.AreEqual(0, cl[1].DisplayIndex);
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(false, dE.DoneButtondisabled); //TODO
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false
            Assert.AreEqual("", cl[1].Description);

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Flere valg", _fields[0].Label);
            Assert.AreEqual("sfsfs", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(0, _fields[0].Mandatory); //false

            List<KeyValuePair> kvp = sut.PairRead(_fields[0].KeyValuePairList);
            Assert.AreEqual("1", kvp[0].Key); 
            // Assert.AreEqual(CData, kvp[0].Value); todo
            Assert.AreEqual(false, kvp[0].Selected); //false
            Assert.AreEqual("1", kvp[0].DisplayOrder);

            Assert.AreEqual("2", kvp[1].Key);
            // Assert.AreEqual(CData, kvp[0].Value); todo
            Assert.AreEqual(false, kvp[1].Selected); //false
            Assert.AreEqual("2", kvp[1].DisplayOrder);

            Assert.AreEqual("3", kvp[2].Key);
            // Assert.AreEqual(CData, kvp[0].Value); todo
            Assert.AreEqual(false, kvp[2].Selected); //false
            Assert.AreEqual("3", kvp[2].DisplayOrder);

            Assert.AreEqual("4", kvp[3].Key);
            // Assert.AreEqual(CData, kvp[0].Value); todo
            Assert.AreEqual(false, kvp[3].Selected); //false
            Assert.AreEqual("4", kvp[3].DisplayOrder);
   

            Assert.AreEqual("Choose one option", _fields[1].Label);
            Assert.AreEqual("This is a description", _fields[1].Description);
            Assert.AreEqual(1, _fields[1].DisplayIndex);
            Assert.AreEqual(0, _fields[1].Mandatory); //false

            List<KeyValuePair> kvpp1 = sut.PairRead(_fields[1].KeyValuePairList);
            Assert.AreEqual("1", kvpp1[0].Key);
            // Assert.AreEqual(CData, kvp[0].Value); todo
            Assert.AreEqual(false, kvpp1[0].Selected); //false
            Assert.AreEqual("1", kvpp1[0].DisplayOrder);

            Assert.AreEqual("2", kvpp1[1].Key);
            // Assert.AreEqual(CData, kvp[0].Value); todo
            Assert.AreEqual(false, kvpp1[1].Selected); //false
            Assert.AreEqual("2", kvpp1[1].DisplayOrder);

        }

        [Test]
        public async Task Core_eFormSimpleSingleSelectFormCreateInDB_ReturnseFormId()
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

            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement);
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();


            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Single Select", cl[0].Label);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); todo
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(cl[0].summary, false); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex);

            Assert.AreEqual(2, cl.Count());

            Assert.AreEqual("Single Select", cl[1].Label);
            Assert.AreEqual("", cl[1].Description);
            // Assert.AreEqual(CDataValue, dE.Label); //TODO
            Assert.AreEqual(0, cl[1].DisplayIndex);
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); //todo
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false;
            // Assert.AreEqual(false, cl[0].donebuttondisabled); //TODO
            Assert.AreEqual(0, cl[0].ApprovalEnabled); //false


            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Single Select 1", _fields[0].Label);
            Assert.AreEqual("Single Select 1 description", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(0, _fields[0].Mandatory); //false

            List<KeyValuePair> kvp = sut.PairRead(_fields[0].KeyValuePairList);

            
            Assert.AreEqual("1", kvp[0].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kvp[0].Selected);
            Assert.AreEqual("1", kvp[0].DisplayOrder);

            
            Assert.AreEqual("2", kvp[1].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kvp[1].Selected);
            Assert.AreEqual("2", kvp[1].DisplayOrder);

            Assert.AreEqual("3", kvp[2].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kvp[2].Selected);
            Assert.AreEqual("3", kvp[2].DisplayOrder);

            Assert.AreEqual("4", kvp[3].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kvp[3].Selected);
            Assert.AreEqual("4", kvp[3].DisplayOrder);

            Assert.AreEqual("5", kvp[4].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kvp[4].Selected);
            Assert.AreEqual("5", kvp[4].DisplayOrder);

            Assert.AreEqual("6", kvp[5].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kvp[5].Selected);
            Assert.AreEqual("6", kvp[5].DisplayOrder);

            Assert.AreEqual("7", kvp[6].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kvp[6].Selected);
            Assert.AreEqual("7", kvp[6].DisplayOrder);

            Assert.AreEqual("8", kvp[7].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, kvp[7].Selected);
            Assert.AreEqual("8", kvp[7].DisplayOrder);

            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);


        }

        [Test] // Comment
        public async Task Core_eFormSimpleTextMultiLineFormCreateInDB_ReturnseFormId()
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

            // Act
            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement); 
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("comment", cl[0].Label);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval);//false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("comment", cl[1].Label);
            Assert.AreEqual("", cl[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Comment", _fields[0].Label);
            Assert.AreEqual("", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            // Assert.AreEqual(1, _fields[0].multi); todo meant to be false but was null
            // Assert.AreEqual(0, _fields[0].geolocation_enabled); //todo meant to be false but was null
            Assert.AreEqual(0, _fields[0].SplitScreen); //false
            Assert.AreEqual("", _fields[0].DefaultValue);
            Assert.AreEqual(0, _fields[0].ReadOnly); //false
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);

        }

        [Test] // Text
        public async Task Core_eFormSimpleTextSingleLineFormCreateInDB_ReturnseFormId()
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

            // Act
            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement);
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Single line", cl[0].Label);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval);//false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("Single line", cl[1].Label);
            Assert.AreEqual("", cl[1].Description); 
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Single line 1", _fields[0].Label);
            Assert.AreEqual("Single line 1 description", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            // Assert.AreEqual(1, _fields[0].multi); todo meant to be false but was null
            // Assert.AreEqual(0, _fields[0].geolocation_enabled); //todo meant to be false but was null
            // Assert.AreEqual(0, _fields[0].split_screen); //false todo meant to be false but was null
            Assert.AreEqual("", _fields[0].DefaultValue);
            Assert.AreEqual(0, _fields[0].ReadOnly); //false
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);


        }

        [Test]
        public async Task Core_eFormSimpleNumberFormCreateInDB_ReturnseFormId()
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

            // Act
            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement);
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Number 1", cl[0].Label);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval);//false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("Number 1", cl[1].Label);
            Assert.AreEqual("", cl[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Number 1", _fields[0].Label);
            Assert.AreEqual("Number 1 description", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual("1", _fields[0].MinValue);
            Assert.AreEqual("1100", _fields[0].MaxValue);
            // Assert.AreEqual("24", _fields[0].field_values); TODO NO METHOD NAMED VALUE
            Assert.AreEqual(2, _fields[0].DecimalCount);
            Assert.AreEqual("", _fields[0].UnitName);
            Assert.AreEqual(0, _fields[0].ReadOnly); //false
            Assert.AreEqual(1, _fields[0].Mandatory); //true
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);

        }

        [Test]
        public async Task Core_eFormSimpleInfoboxFormCreateInDB_ReturnseFormId()
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

            // Act
            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement);
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Info box", cl[0].Label);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval);//false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("Info box", cl[1].Label);
            Assert.AreEqual("", cl[1].Description); 
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Info box 1", _fields[0].Label);
            Assert.AreEqual("Info box 1 description", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);

        }

        [Test]
        public async Task Core_eFormSimpleCheckBoxFormCreateInDB_ReturnseFormId()
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

            // Act
            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement);
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("checkbox", cl[0].Label);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval);//false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("checkbox", cl[1].Label);
            Assert.AreEqual("", cl[1].Description); 
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Checkbox 1", _fields[0].Label);
            Assert.AreEqual("Checkbox 1 description", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(0, _fields[0].Selected); //false
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);

        }

        [Test]
        public async Task Core_eFormSimpleTimerFormCreateInDB_ReturnseFormId()
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

            // Act
            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement);
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("TimerStartStop", cl[0].Label);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval);//false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("TimerStartStop", cl[1].Label);
            Assert.AreEqual("", cl[1].Description); 
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false
           
            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Timer Start Stop 1", _fields[0].Label);
            Assert.AreEqual("Timer Start Stop 1 Description", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            // Assert.AreEqual(0, _fields[0].stop_on_save); //todo meant to be false be was null
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);

        }

        [Test]
        public async Task Core_eFormSimpleSaveButtonFormCreateInDB_ReturnseFormId()
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

            // Act
            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement);
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Save button", cl[0].Label);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval);//false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("Save button", cl[1].Label);
            Assert.AreEqual("", cl[1].Description); 
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, true); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Save button 1", _fields[0].Label);
            Assert.AreEqual("Save button 1 Description", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            // Assert.AreEqual(0, _fields[0].value); //todo meant to be false be was null
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);

        }

        [Test]
        public async Task Core_eForm_MultiLvleFormCreateInDB_ReturnseFormId()
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
                          <Description><![CDATA[1.3 lvl description]]></Description>
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

            MainElement mainElement = await sut.TemplateFromXml(xmlstring);
            int eFormId = await sut.TemplateCreate(mainElement);
            List<check_lists> cl = dbContext.check_lists.AsNoTracking().ToList();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("MultiLvlTest", cl[0].Label);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); todo
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex);

           
            Assert.AreEqual(6, cl.Count());

            Assert.AreEqual("1 lvl", cl[1].Label);
            Assert.AreEqual("1 lvl description", cl[1].Description); 
            Assert.AreEqual(0, cl[1].DisplayIndex);

            

            Assert.AreEqual("1.1 lvl", cl[2].Label);
            Assert.AreEqual("1.1 lvl description", cl[2].Description); 
            Assert.AreEqual(0, cl[2].DisplayIndex);
            Assert.AreEqual(0, cl[2].ReviewEnabled); //false
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(0, cl[2].ExtraFieldsEnabled); //false
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.AreEqual(0, cl[2].ApprovalEnabled); //false

            List<fields> _fields = dbContext.fields.AsNoTracking().ToList();

            Assert.AreEqual(cl[2].Id, _fields[0].CheckListId);
            Assert.AreEqual("1.1 lvl checkbox", _fields[0].Label);
            Assert.AreEqual("1.1 lvl cehckbox description", _fields[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(0, _fields[0].Selected); //false
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[0].Color);

            
            Assert.AreEqual("1.2 lvl", cl[3].Label);
            Assert.AreEqual("1.2 lvl description", cl[3].Description); 
            Assert.AreEqual(1, cl[3].DisplayIndex);
            Assert.AreEqual(0, cl[3].ReviewEnabled); //false
            // Assert.AreEqual(false, dE2.manualsync)//TODO
            Assert.AreEqual(0, cl[3].ExtraFieldsEnabled); //false
            // Assert.AreEqual(false, dE2.DoneButtonDisabled); //TODO
            Assert.AreEqual(0, cl[3].ApprovalEnabled); //false

            
            Assert.AreEqual("1.2 lvl checkbox", _fields[1].Label);
            Assert.AreEqual("1.2 lvl checkbox description", _fields[1].Description);
            Assert.AreEqual(0, _fields[1].DisplayIndex);
            Assert.AreEqual(0, _fields[1].Selected); //false
            Assert.AreEqual(0, _fields[1].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[1].Color);

            
            Assert.AreEqual("1.3 lvl", cl[4].Label);
            Assert.AreEqual("1.3 lvl description", cl[4].Description);
            Assert.AreEqual(2, cl[4].DisplayIndex);

           
            Assert.AreEqual("1.3.1 lvl", cl[5].Label);
            Assert.AreEqual("1.3.1 lvl description", cl[5].Description);
            Assert.AreEqual(0, cl[5].DisplayIndex);
            Assert.AreEqual(0, cl[5].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[5].manual_sync);//todo false
            Assert.AreEqual(0, cl[5].ExtraFieldsEnabled); //false
            // Assert.AreEqual(0, cl[5].donebuttondisabled)//TODO
            Assert.AreEqual(0, cl[5].ApprovalEnabled); //false

            Assert.AreEqual("1.3.1 lvl checkbox", _fields[2].Label);
            Assert.AreEqual("1.3.1 lvl checkbox description", _fields[2].Description); 
            Assert.AreEqual(0, _fields[2].DisplayIndex);
            Assert.AreEqual(0, _fields[2].Selected); //false
            Assert.AreEqual(0, _fields[2].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Grey, _fields[2].Color);

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