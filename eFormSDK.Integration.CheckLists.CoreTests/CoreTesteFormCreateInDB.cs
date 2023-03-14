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
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;
using Field = Microting.eForm.Infrastructure.Data.Entities.Field;

namespace eFormSDK.Integration.CheckLists.CoreTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class CoreTesteFormCreateInDB : DbTestFixture
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
            path = Path.GetDirectoryName(path);
            await sut.SetSdkSetting(Settings.fileLocationPicture,
                Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SetSdkSetting(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SetSdkSetting(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
            testHelpers = new TestHelpers(ConnectionString);
            await testHelpers.GenerateDefaultLanguages();
            //sut.StartLog(new CoreBase());
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual("", cl[0].CaseType);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("CommentMain", checkLisTranslations[0].Text);
            // Assert.AreEqual("da", match.Language);
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            Assert.AreEqual(0, cl[0].DisplayIndex);
            // Assert.AreEqual(1, match.ElementList.Count());
            //DataElement dE = (DataElement)cl[1];
            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("CommentDataElement", checkLisTranslations[1].Text);

            //CDataValue cd = new CDataValue();

            Assert.AreEqual("CommentDataElementDescription", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex);
            Assert.AreEqual(0, cl[1].ReviewEnabled); //False
            // Assert.AreEqual(0, cl[1].manual_sync); //false TODO was null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //False
            // Assert.AreEqual(0, cl[1].done_button_Disabled); //TODO no method
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //False

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            //Comment commentField = (Comment)dE.DataItemList[0];
            Assert.AreEqual("CommentField", fieldTranslations[0].Text);
            Assert.AreEqual("CommentFieldDescription", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            // Assert.AreEqual(1, _fields[0].multi); was null TODO
            // Assert.AreEqual(0, _fields[0].geolocation_enabled); //False TODO was null
            Assert.AreEqual(0, _fields[0].Split); //TODO no method Split
            Assert.AreEqual("", _fields[0].DefaultValue);
            Assert.AreEqual(0, _fields[0].ReadOnly); //false
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Picture test", checkLisTranslations[0].Text);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("Picture test", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Take two pictures", fieldTranslations[0].Text);
            Assert.AreEqual("", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Date", checkLisTranslations[0].Text);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("Date", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Select date", fieldTranslations[0].Text);
            Assert.AreEqual("", fieldTranslations[0].Description);
            // Assert.AreEqual("2018-01-18", _fields.minvalue) //todo
            // Assert.AreEqual("2028-01-18", _fields.maxvalue) //todo
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            // Assert.AreEqual("", _fields[0].value); //TODO
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(0, _fields[0].ReadOnly); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("ny pdf", checkLisTranslations[0].Text);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("ny pdf", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("bla", fieldTranslations[0].Text);
            Assert.AreEqual("", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(null, _fields[0].DefaultValue); //false
            Assert.AreEqual("20d483dd7791cd6becf089432724c663", fieldTranslations[0].DefaultValue); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();


            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Tester grupper", checkLisTranslations[0].Text);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); //TODO
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex);


            Assert.AreEqual(2, cl.Count());
            Assert.AreEqual("Tester grupper", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex);
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false


            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);

            Assert.AreEqual("Gruppe efter tjekboks", fieldTranslations[0].Text);
            Assert.AreEqual("", fieldTranslations[0].Description);
            Assert.AreEqual(1, _fields[0].DisplayIndex);
            Assert.AreEqual("Closed", _fields[0].DefaultValue);
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);


            Assert.AreEqual("Tjekboks inde i gruppe", fieldTranslations[1].Text);
            Assert.AreEqual("", fieldTranslations[1].Description);
            Assert.AreEqual(0, _fields[1].DisplayIndex);
            Assert.AreEqual(0, _fields[1].Selected); //false
            Assert.AreEqual(0, _fields[1].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);


            Assert.AreEqual("Tjekboks før gruppe", fieldTranslations[2].Text);
            Assert.AreEqual("", fieldTranslations[2].Description);
            Assert.AreEqual(0, _fields[2].DisplayIndex);
            Assert.AreEqual(0, _fields[2].Selected); //false
            Assert.AreEqual(0, _fields[2].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[2].Color);


            Assert.AreEqual("Tjekboks efter gruppe", fieldTranslations[3].Text);
            Assert.AreEqual("", fieldTranslations[3].Description);
            Assert.AreEqual(2, _fields[3].DisplayIndex);
            Assert.AreEqual(0, _fields[3].Selected); //false
            Assert.AreEqual(0, _fields[3].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[3].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();


            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Billede og signatur", checkLisTranslations[0].Text);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].Language); //todo
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex);


            Assert.AreEqual(2, cl.Count());
            Assert.AreEqual("Billede og signatur", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex);
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Tag et billede", fieldTranslations[0].Text);
            Assert.AreEqual("", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);


            Assert.AreEqual("Skriv", fieldTranslations[1].Text);
            Assert.AreEqual(
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam dui neque, molestie at maximus a, malesuada at mi. Cras venenatis porttitor augue nec sagittis. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris urna massa, sagittis at fringilla ut, convallis sed dolor. Praesent scelerisque magna dolor, quis blandit metus pharetra eu. Cras euismod facilisis risus at ullamcorper. Pellentesque vitae maximus elit. Sed scelerisque nec velit dictum sodales. Duis sed dapibus odio. Sed non luctus sem. Donec eu mollis lectus, nec porta nisl. Aenean a consequat metus, ac auctor arcu. Cras sit amet blandit velit. Pellentesque faucibus eros sed ullamcorper rutrum.<br><br><br>Pellentesque ultrices ex erat. Pellentesque rhoncus eget lectus et scelerisque. Cras vitae diam ex. Ut felis ligula, venenatis ut lorem vel, venenatis convallis turpis. Sed rutrum ac odio ac auctor. Sed mauris ipsum, vulputate ut sodales a, mattis et purus. Nam convallis augue velit, nec blandit ipsum porta vitae. Quisque et iaculis lectus. Donec eu fringilla turpis, id rutrum mauris.<br><br><br>Proin eu sagittis sem. Aenean vel placerat sapien. Praesent et rutrum justo. Mauris consectetur venenatis est, eu vulputate enim elementum eget. In hac habitasse platea dictumst. Sed vehicula nec neque sed posuere. Aenean sodales lectus a purus posuere lacinia. Aenean ut enim vel odio varius placerat. Phasellus faucibus turpis sed arcu ultrices interdum. Sed porta, nisi nec vehicula lacinia, ante tortor tristique justo, vel sagittis felis ligula eu magna. Pellentesque a velit laoreet nunc aliquet ornare sit amet eget lorem. Duis aliquet viverra pretium. Etiam a mauris tellus. Sed viverra eros eget lectus lobortis, in vestibulum lorem rhoncus. Aliquam sem felis, suscipit a gravida ut, eleifend et ipsum. Nullam lacus lacus, rutrum quis sollicitudin et, porta et erat.",
                fieldTranslations[1].Description);
            Assert.AreEqual(1, _fields[1].DisplayIndex);
            Assert.AreEqual(0, _fields[1].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[1].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();


            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Muligheder med Microting eForm", checkLisTranslations[0].Text);
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); todo
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(76, cl[0].DisplayIndex);

            Assert.AreEqual(2, cl.Count());
            Assert.AreEqual("Muligheder med Microting eForm", checkLisTranslations[1].Text);
            Assert.AreEqual(
                "Tryk her og prøv hvordan du indsamler data med Microting eForm.<br><br><br>God fornøjelse :-)",
                checkLisTranslations[1].Description);
            Assert.AreEqual(76, cl[1].DisplayIndex);
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(dE.DoneButtonDisabled, true); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);


            Assert.AreEqual("GEM", fieldTranslations[0].Text);
            Assert.AreEqual("Tryk her for at gemme dine indtastede data<br><br>", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(null, _fields[0].DefaultValue);
            Assert.AreEqual("GEM", fieldTranslations[0].DefaultValue);


            Assert.AreEqual("START-STOP TID", fieldTranslations[1].Text);
            Assert.AreEqual("Start-stop tid.<br><br>", fieldTranslations[1].Description);
            Assert.AreEqual(1, _fields[1].DisplayIndex);
            // Assert.AreEqual(0, _fields[1].stop_on_save);//false TODO
            Assert.AreEqual(0, _fields[1].Mandatory); //false


            Assert.AreEqual("INFO", fieldTranslations[2].Text);
            Assert.AreEqual(
                "I dette tekstfelt vises ikke redigerbar tekst.<br><br>Er Microting eForm integreret med ERP-system, kan data fra ERP-systemet vises i dette felt fx. baggrundsinformation på kunder.<br>",
                fieldTranslations[2].Description);
            Assert.AreEqual(2, _fields[2].DisplayIndex);


            Assert.AreEqual("PDF", fieldTranslations[3].Text);
            Assert.AreEqual("Her vises PDF-filer.<br>", fieldTranslations[3].Description);
            Assert.AreEqual(3, _fields[3].DisplayIndex);
            Assert.AreEqual("a60ad2d8c22ed24780bfa9a348376232", fieldTranslations[3].DefaultValue);
            Assert.AreEqual(null, _fields[3].DefaultValue);


            Assert.AreEqual("TJEK", fieldTranslations[4].Text);
            Assert.AreEqual("I et tjekfelt sættes et flueben.<br>", fieldTranslations[4].Description);
            Assert.AreEqual(5, _fields[4].DisplayIndex);
            Assert.AreEqual(0, _fields[4].Selected); //false
            Assert.AreEqual(0, _fields[4].Mandatory); //false


            Assert.AreEqual("VÆLG", fieldTranslations[5].Text);
            Assert.AreEqual(
                "Vælg én eller flere i liste.<br><br>Er Microting eForm integreret med ERP-system, kan valgmulighederne komme derfra.<br>",
                fieldTranslations[5].Description);
            Assert.AreEqual(6, _fields[5].DisplayIndex);
            Assert.AreEqual(0, _fields[5].Mandatory); //false

            var fieldOptionsQuery = DbContext.FieldOptions.Where(x => x.FieldId == _fields[5].Id);
            List<int> fieldOptionIds = await fieldOptionsQuery.Select(x => x.Id).ToListAsync();
            List<FieldOptionTranslation> fieldOptionTranslations = await DbContext.FieldOptionTranslations
                .Where(x => fieldOptionIds.Contains(x.FieldOptionId)).ToListAsync();
            List<FieldOption> fieldOptions = await fieldOptionsQuery.ToListAsync();

            //List<KeyValuePair> kvp = sut.PairRead(_fields[5].KeyValuePairList);
            Assert.AreEqual("1", fieldOptions[0].DisplayOrder);
            Assert.AreEqual("Valgmulighed 1", fieldOptionTranslations[0].Text);
            Assert.AreEqual(false, fieldOptions[0].Selected); //false
            Assert.AreEqual("1", fieldOptions[0].Key);


            Assert.AreEqual("2", fieldOptions[1].DisplayOrder);
            Assert.AreEqual("Valgmulighed 2", fieldOptionTranslations[1].Text);
            Assert.AreEqual(false, fieldOptions[1].Selected); //false
            Assert.AreEqual("2", fieldOptions[1].Key);


            Assert.AreEqual("3", fieldOptions[2].DisplayOrder);
            Assert.AreEqual("Valgmulighed 3", fieldOptionTranslations[2].Text);
            Assert.AreEqual(false, fieldOptions[2].Selected); //false
            Assert.AreEqual("3", fieldOptions[2].Key);


            Assert.AreEqual("4", fieldOptions[3].DisplayOrder);
            Assert.AreEqual("Valgmulighed 4", fieldOptionTranslations[3].Text);
            Assert.AreEqual(false, fieldOptions[3].Selected); //false
            Assert.AreEqual("4", fieldOptions[3].Key);


            Assert.AreEqual("5", fieldOptions[4].DisplayOrder);
            Assert.AreEqual("Valgmulighed 5", fieldOptionTranslations[4].Text);
            Assert.AreEqual(false, fieldOptions[4].Selected); //false
            Assert.AreEqual("5", fieldOptions[4].Key);


            Assert.AreEqual("6", fieldOptions[5].DisplayOrder);
            Assert.AreEqual("Valgmulighed 6", fieldOptionTranslations[5].Text);
            Assert.AreEqual(false, fieldOptions[5].Selected); //false
            Assert.AreEqual("6", fieldOptions[5].Key);

            Assert.AreEqual("7", fieldOptions[6].DisplayOrder);
            Assert.AreEqual("Valgmulighed 7", fieldOptionTranslations[6].Text);
            Assert.AreEqual(false, fieldOptions[6].Selected); //false
            Assert.AreEqual("7", fieldOptions[6].Key);

            Assert.AreEqual("8", fieldOptions[7].DisplayOrder);
            Assert.AreEqual("Valgmulighed 8", fieldOptionTranslations[7].Text);
            Assert.AreEqual(false, fieldOptions[7].Selected); //false
            Assert.AreEqual("8", fieldOptions[7].Key);

            Assert.AreEqual("9", fieldOptions[8].DisplayOrder);
            Assert.AreEqual("Valgmulighed 9", fieldOptionTranslations[8].Text);
            Assert.AreEqual(false, fieldOptions[8].Selected); //false
            Assert.AreEqual("9", fieldOptions[8].Key);

            Assert.AreEqual("10", fieldOptions[9].DisplayOrder);
            Assert.AreEqual("Valgmulighed N", fieldOptionTranslations[9].Text);
            Assert.AreEqual(false, fieldOptions[9].Selected); //false
            Assert.AreEqual("10", fieldOptions[9].Key);


            Assert.AreEqual("VÆLG ÉN", fieldTranslations[6].Text);
            Assert.AreEqual(
                "Vælg én blandt flere valgmuligheder.<br><br>Er Microting eForm integreret med ERP-system, kan valgmulighederne komme derfra.",
                fieldTranslations[6].Description);
            Assert.AreEqual(7, _fields[6].DisplayIndex);
            Assert.AreEqual(0, _fields[6].Mandatory); //false

            fieldOptionsQuery = DbContext.FieldOptions.Where(x => x.FieldId == _fields[6].Id);
            fieldOptionIds = await fieldOptionsQuery.Select(x => x.Id).ToListAsync();
            fieldOptionTranslations = await DbContext.FieldOptionTranslations
                .Where(x => fieldOptionIds.Contains(x.FieldOptionId)).ToListAsync();
            fieldOptions = await fieldOptionsQuery.ToListAsync();


            Assert.AreEqual("1", fieldOptions[0].Key);
            Assert.AreEqual("Valgmulighed 1", fieldOptionTranslations[0].Text);
            Assert.AreEqual(false, fieldOptions[0].Selected); //false
            Assert.AreEqual("1", fieldOptions[0].DisplayOrder);

            Assert.AreEqual("2", fieldOptions[1].DisplayOrder);
            Assert.AreEqual("Valgmulighed 2", fieldOptionTranslations[1].Text);
            Assert.AreEqual(false, fieldOptions[1].Selected); //false
            Assert.AreEqual("2", fieldOptions[1].Key);


            Assert.AreEqual("3", fieldOptions[2].DisplayOrder);
            Assert.AreEqual("Valgmulighed 3", fieldOptionTranslations[2].Text);
            Assert.AreEqual(false, fieldOptions[2].Selected); //false
            Assert.AreEqual("3", fieldOptions[2].Key);


            Assert.AreEqual("4", fieldOptions[3].DisplayOrder);
            Assert.AreEqual("Valgmulighed 4", fieldOptionTranslations[3].Text);
            Assert.AreEqual(false, fieldOptions[3].Selected); //false
            Assert.AreEqual("4", fieldOptions[3].Key);


            Assert.AreEqual("5", fieldOptions[4].DisplayOrder);
            Assert.AreEqual("Valgmulighed 5", fieldOptionTranslations[4].Text);
            Assert.AreEqual(false, fieldOptions[4].Selected); //false
            Assert.AreEqual("5", fieldOptions[4].Key);


            Assert.AreEqual("6", fieldOptions[5].DisplayOrder);
            Assert.AreEqual("Valgmulighed 6", fieldOptionTranslations[5].Text);
            Assert.AreEqual(false, fieldOptions[5].Selected); //false
            Assert.AreEqual("6", fieldOptions[5].Key);

            Assert.AreEqual("7", fieldOptions[6].DisplayOrder);
            Assert.AreEqual("Valgmulighed 7", fieldOptionTranslations[6].Text);
            Assert.AreEqual(false, fieldOptions[6].Selected); //false
            Assert.AreEqual("7", fieldOptions[6].Key);

            Assert.AreEqual("8", fieldOptions[7].DisplayOrder);
            Assert.AreEqual("Valgmulighed 8", fieldOptionTranslations[7].Text);
            Assert.AreEqual(false, fieldOptions[7].Selected); //false
            Assert.AreEqual("8", fieldOptions[7].Key);

            Assert.AreEqual("9", fieldOptions[8].DisplayOrder);
            Assert.AreEqual("Valgmulighed 9", fieldOptionTranslations[8].Text);
            Assert.AreEqual(false, fieldOptions[8].Selected); //false
            Assert.AreEqual("9", fieldOptions[8].Key);

            Assert.AreEqual("10", fieldOptions[9].DisplayOrder);
            Assert.AreEqual("Valgmulighed N", fieldOptionTranslations[9].Text);
            Assert.AreEqual(false, fieldOptions[9].Selected); //false
            Assert.AreEqual("10", fieldOptions[9].Key);


            Assert.AreEqual("DATO", fieldTranslations[7].Text);
            Assert.AreEqual(
                "Vælg dato<br><br>Er Microting eForm integreret med ERP-system, kan valgt dato leveres direkte i ERP-system.<br>",
                fieldTranslations[7].Description);
            Assert.AreEqual(8, _fields[7].DisplayIndex);
            // Assert.AreEqual("2016-06-09", d1.MinValue); TODO
            // Assert.AreEqual("2026-06-09", d1.MaxValue); TODO
            Assert.AreEqual(0, _fields[7].Mandatory); //false
            Assert.AreEqual(0, _fields[7].ReadOnly); //false


            Assert.AreEqual("INDTAST TAL", fieldTranslations[8].Text);
            Assert.AreEqual(
                "Indtast tal og opsæt evt. regler for mindste/højeste tilladte værdi.<br><br>Er Microting eForm integreret med ERP-system, sendes de indtastede værdier direkte til ERP-systemet.<br>",
                fieldTranslations[8].Description);
            Assert.AreEqual(9, _fields[8].DisplayIndex);
            // Assert.AreEqual("", _fields[8].min_value); todo
            // Assert.AreEqual("", _fields[8].max_value); todo
            Assert.AreEqual(0, _fields[8].DecimalCount);
            Assert.AreEqual("", _fields[8].UnitName);
            Assert.AreEqual(0, _fields[8].Mandatory); //false


            Assert.AreEqual("SKRIV KORT KOMMENTAR", fieldTranslations[9].Text);
            Assert.AreEqual("Skriv kort kommentar uden linieskift.", fieldTranslations[9].Description);
            Assert.AreEqual(10, _fields[9].DisplayIndex);
            // Assert.AreEqual(0, _fields[9].multi); todo
            Assert.AreEqual(0, _fields[9].GeolocationEnabled); //false
            // Assert.AreEqual(0, _fields[9].split_screen); todo
            Assert.AreEqual("", fieldTranslations[9].DefaultValue);
            Assert.AreEqual(null, _fields[9].DefaultValue);
            Assert.AreEqual(0, _fields[9].ReadOnly); //false
            Assert.AreEqual(0, _fields[9].Mandatory); //false


            Assert.AreEqual("FOTO", fieldTranslations[10].Text);
            Assert.AreEqual(
                "Tag billeder<br><br>Er Microting eForm integreret med ERP-system, kan billederne vises direkte i virksomhedens ERP/andet databasesystem.",
                fieldTranslations[10].Description);
            Assert.AreEqual(11, _fields[10].DisplayIndex);
            Assert.AreEqual(0, _fields[10].Mandatory); //false


            Assert.AreEqual("SKRIV LANG KOMMENTAR", fieldTranslations[11].Text);
            Assert.AreEqual("Skriv længere kommentar med mulighed for linieskift.", fieldTranslations[11].Description);
            Assert.AreEqual(12, _fields[11].DisplayIndex);
            // Assert.AreEqual(1, cc1.multi);
            // Assert.AreEqual(false, cc1.geolocation);
            Assert.AreEqual("", _fields[11].DefaultValue);
            Assert.AreEqual(0, _fields[11].ReadOnly); //false
            Assert.AreEqual(0, _fields[11].Mandatory); //false


            Assert.AreEqual("UNDERSKRIFT", fieldTranslations[12].Text);
            Assert.AreEqual(
                "Underskrift<br><br>Er Microting eForm integreret med ERP-system, kan underskrifterne sendes direkte til ERP/andet databasesystem.",
                fieldTranslations[12].Description);
            Assert.AreEqual(13, _fields[12].DisplayIndex);
            Assert.AreEqual(0, _fields[12].Mandatory); //false


            Assert.AreEqual("GEM", fieldTranslations[13].Text);
            Assert.AreEqual("<br>Tryk for at gemme data.<br>Press to save data.<br>",
                fieldTranslations[13].Description);
            Assert.AreEqual(14, _fields[13].DisplayIndex);
            Assert.AreEqual("GEM/SAVE", fieldTranslations[13].DefaultValue);
            Assert.AreEqual(null, _fields[13].DefaultValue);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();


            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Multiselect", checkLisTranslations[0].Text);
            // Assert.AreEqual("2017-01-22", match.StartDate); TODO
            // Assert.AreEqual("2027-01-22", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].Language); todo
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex);

            Assert.AreEqual(2, cl.Count());


            Assert.AreEqual("Multiselect", checkLisTranslations[1].Text);
            // Assert.AreEqual(CDataValue, dE.Text); //TODO
            Assert.AreEqual(0, cl[1].DisplayIndex);
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(false, dE.DoneButtondisabled); //TODO
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false
            Assert.AreEqual("", checkLisTranslations[1].Description);

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();


            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Flere valg", fieldTranslations[0].Text);
            Assert.AreEqual("sfsfs", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(0, _fields[0].Mandatory); //false


            var fieldOptionsQuery = DbContext.FieldOptions.Where(x => x.FieldId == _fields[0].Id);
            List<int> fieldOptionIds = await fieldOptionsQuery.Select(x => x.Id).ToListAsync();
            List<FieldOptionTranslation> fieldOptionTranslations = await DbContext.FieldOptionTranslations
                .Where(x => fieldOptionIds.Contains(x.FieldOptionId)).ToListAsync();
            List<FieldOption> fieldOptions = await fieldOptionsQuery.ToListAsync();

            Assert.AreEqual("1", fieldOptions[0].Key);
            // Assert.AreEqual(CData, fieldOptionTranslations[0].Text); todo
            Assert.AreEqual(false, fieldOptions[0].Selected); //false
            Assert.AreEqual("1", fieldOptions[0].DisplayOrder);

            Assert.AreEqual("2", fieldOptions[1].Key);
            // Assert.AreEqual(CData, fieldOptionTranslations[0].Text); todo
            Assert.AreEqual(false, fieldOptions[1].Selected); //false
            Assert.AreEqual("2", fieldOptions[1].DisplayOrder);

            Assert.AreEqual("3", fieldOptions[2].Key);
            // Assert.AreEqual(CData, fieldOptionTranslations[0].Text); todo
            Assert.AreEqual(false, fieldOptions[2].Selected); //false
            Assert.AreEqual("3", fieldOptions[2].DisplayOrder);

            Assert.AreEqual("4", fieldOptions[3].Key);
            // Assert.AreEqual(CData, fieldOptionTranslations[0].Text); todo
            Assert.AreEqual(false, fieldOptions[3].Selected); //false
            Assert.AreEqual("4", fieldOptions[3].DisplayOrder);


            Assert.AreEqual("Choose one option", fieldTranslations[1].Text);
            Assert.AreEqual("This is a description", fieldTranslations[1].Description);
            Assert.AreEqual(1, _fields[1].DisplayIndex);
            Assert.AreEqual(0, _fields[1].Mandatory); //false


            fieldOptionsQuery = DbContext.FieldOptions.Where(x => x.FieldId == _fields[1].Id);
            fieldOptionIds = await fieldOptionsQuery.Select(x => x.Id).ToListAsync();
            fieldOptionTranslations = await DbContext.FieldOptionTranslations
                .Where(x => fieldOptionIds.Contains(x.FieldOptionId)).ToListAsync();
            fieldOptions = await fieldOptionsQuery.ToListAsync();

            Assert.AreEqual("1", fieldOptions[0].Key);
            // Assert.AreEqual(CData, fieldOptionTranslations[0].Text); todo
            Assert.AreEqual(false, fieldOptions[0].Selected); //false
            Assert.AreEqual("1", fieldOptions[0].DisplayOrder);

            Assert.AreEqual("2", fieldOptions[1].Key);
            // Assert.AreEqual(CData, fieldOptionTranslations[0].Text); todo
            Assert.AreEqual(false, fieldOptions[1].Selected); //false
            Assert.AreEqual("2", fieldOptions[1].DisplayOrder);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();


            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Single Select", checkLisTranslations[0].Text);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); todo
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(cl[0].summary, false); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex);

            Assert.AreEqual(2, cl.Count());

            Assert.AreEqual("Single Select", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            // Assert.AreEqual(CDataValue, dE.Text); //TODO
            Assert.AreEqual(0, cl[1].DisplayIndex);
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); //todo
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false;
            // Assert.AreEqual(false, cl[0].donebuttondisabled); //TODO
            Assert.AreEqual(0, cl[0].ApprovalEnabled); //false


            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Single Select 1", fieldTranslations[0].Text);
            Assert.AreEqual("Single Select 1 description", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(0, _fields[0].Mandatory); //false

            //List<KeyValuePair> kvp = sut.PairRead(_fields[0].KeyValuePairList);

            var fieldOptionsQuery = DbContext.FieldOptions.Where(x => x.FieldId == _fields[0].Id);
            List<int> fieldOptionIds = await fieldOptionsQuery.Select(x => x.Id).ToListAsync();
            List<FieldOptionTranslation> fieldOptionTranslations = await DbContext.FieldOptionTranslations
                .Where(x => fieldOptionIds.Contains(x.FieldOptionId)).ToListAsync();
            List<FieldOption> fieldOptions = await fieldOptionsQuery.ToListAsync();

            Assert.AreEqual("1", fieldOptions[0].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, fieldOptions[0].Selected);
            Assert.AreEqual("1", fieldOptions[0].DisplayOrder);


            Assert.AreEqual("2", fieldOptions[1].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, fieldOptions[1].Selected);
            Assert.AreEqual("2", fieldOptions[1].DisplayOrder);

            Assert.AreEqual("3", fieldOptions[2].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, fieldOptions[2].Selected);
            Assert.AreEqual("3", fieldOptions[2].DisplayOrder);

            Assert.AreEqual("4", fieldOptions[3].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, fieldOptions[3].Selected);
            Assert.AreEqual("4", fieldOptions[3].DisplayOrder);

            Assert.AreEqual("5", fieldOptions[4].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, fieldOptions[4].Selected);
            Assert.AreEqual("5", fieldOptions[4].DisplayOrder);

            Assert.AreEqual("6", fieldOptions[5].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, fieldOptions[5].Selected);
            Assert.AreEqual("6", fieldOptions[5].DisplayOrder);

            Assert.AreEqual("7", fieldOptions[6].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, fieldOptions[6].Selected);
            Assert.AreEqual("7", fieldOptions[6].DisplayOrder);

            Assert.AreEqual("8", fieldOptions[7].Key);
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.AreEqual(false, fieldOptions[7].Selected);
            Assert.AreEqual("8", fieldOptions[7].DisplayOrder);

            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("comment", checkLisTranslations[0].Text);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("comment", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Comment", fieldTranslations[0].Text);
            Assert.AreEqual("", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            // Assert.AreEqual(1, _fields[0].multi); todo meant to be false but was null
            // Assert.AreEqual(0, _fields[0].geolocation_enabled); //todo meant to be false but was null
            Assert.AreEqual(0, _fields[0].Split); //false
            Assert.AreEqual("", _fields[0].DefaultValue);
            Assert.AreEqual(0, _fields[0].ReadOnly); //false
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Single line", checkLisTranslations[0].Text);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("Single line", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Single line 1", fieldTranslations[0].Text);
            Assert.AreEqual("Single line 1 description", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            // Assert.AreEqual(1, _fields[0].multi); todo meant to be false but was null
            // Assert.AreEqual(0, _fields[0].geolocation_enabled); //todo meant to be false but was null
            // Assert.AreEqual(0, _fields[0].split_screen); //false todo meant to be false but was null
            Assert.AreEqual(null, _fields[0].DefaultValue);
            Assert.AreEqual("", fieldTranslations[0].DefaultValue);
            Assert.AreEqual(0, _fields[0].ReadOnly); //false
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Number 1", checkLisTranslations[0].Text);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("Number 1", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Number 1", fieldTranslations[0].Text);
            Assert.AreEqual("Number 1 description", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual("1", _fields[0].MinValue);
            Assert.AreEqual("1100", _fields[0].MaxValue);
            // Assert.AreEqual("24", _fields[0].field_values); TODO NO METHOD NAMED VALUE
            Assert.AreEqual(2, _fields[0].DecimalCount);
            Assert.AreEqual("", _fields[0].UnitName);
            Assert.AreEqual(0, _fields[0].ReadOnly); //false
            Assert.AreEqual(1, _fields[0].Mandatory); //true
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Info box", checkLisTranslations[0].Text);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("Info box", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Info box 1", fieldTranslations[0].Text);
            Assert.AreEqual("Info box 1 description", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("checkbox", checkLisTranslations[0].Text);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("checkbox", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Checkbox 1", fieldTranslations[0].Text);
            Assert.AreEqual("Checkbox 1 description", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(0, _fields[0].Selected); //false
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("TimerStartStop", checkLisTranslations[0].Text);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("TimerStartStop", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Timer Start Stop 1", fieldTranslations[0].Text);
            Assert.AreEqual("Timer Start Stop 1 Description", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            // Assert.AreEqual(0, _fields[0].stop_on_save); //todo meant to be false be was null
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("Save button", checkLisTranslations[0].Text);
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex); //false

            Assert.AreEqual(2, cl.Count);
            Assert.AreEqual("Save button", checkLisTranslations[1].Text);
            Assert.AreEqual("", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex); //false
            Assert.AreEqual(0, cl[1].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.AreEqual(0, cl[1].ExtraFieldsEnabled); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, true); //TODO no method donebuttondisabled
            Assert.AreEqual(0, cl[1].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.AreEqual(1, _fields.Count());
            Assert.AreEqual(cl[1].Id, _fields[0].CheckListId);
            Assert.AreEqual("Save button 1", fieldTranslations[0].Text);
            Assert.AreEqual("Save button 1 Description", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            // Assert.AreEqual(0, _fields[0].value); //todo meant to be false be was null
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);
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
            List<CheckList> cl = DbContext.CheckLists.AsNoTracking().ToList();
            List<CheckListTranslation> checkLisTranslations =
                await DbContext.CheckListTranslations.AsNoTracking().ToListAsync();
            List<FieldTranslation> fieldTranslations =
                await DbContext.FieldTranslations.AsNoTracking().ToListAsync();

            // Assert
            Assert.NotNull(cl);
            Assert.AreEqual(1, cl[0].Repeated);
            Assert.AreEqual("MultiLvlTest", checkLisTranslations[0].Text);
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); todo
            Assert.AreEqual(0, cl[0].MultiApproval); //false
            Assert.AreEqual(0, cl[0].FastNavigation); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, cl[0].DisplayIndex);


            Assert.AreEqual(6, cl.Count());

            Assert.AreEqual("1 lvl", checkLisTranslations[1].Text);
            Assert.AreEqual("1 lvl description", checkLisTranslations[1].Description);
            Assert.AreEqual(0, cl[1].DisplayIndex);


            Assert.AreEqual("1.1 lvl", checkLisTranslations[2].Text);
            Assert.AreEqual("1.1 lvl description", checkLisTranslations[2].Description);
            Assert.AreEqual(0, cl[2].DisplayIndex);
            Assert.AreEqual(0, cl[2].ReviewEnabled); //false
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.AreEqual(0, cl[2].ExtraFieldsEnabled); //false
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.AreEqual(0, cl[2].ApprovalEnabled); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.AreEqual(cl[2].Id, _fields[0].CheckListId);
            Assert.AreEqual("1.1 lvl checkbox", fieldTranslations[0].Text);
            Assert.AreEqual("1.1 lvl cehckbox description", fieldTranslations[0].Description);
            Assert.AreEqual(0, _fields[0].DisplayIndex);
            Assert.AreEqual(0, _fields[0].Selected); //false
            Assert.AreEqual(0, _fields[0].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[0].Color);


            Assert.AreEqual("1.2 lvl", checkLisTranslations[3].Text);
            Assert.AreEqual("1.2 lvl description", checkLisTranslations[3].Description);
            Assert.AreEqual(1, cl[3].DisplayIndex);
            Assert.AreEqual(0, cl[3].ReviewEnabled); //false
            // Assert.AreEqual(false, dE2.manualsync)//TODO
            Assert.AreEqual(0, cl[3].ExtraFieldsEnabled); //false
            // Assert.AreEqual(false, dE2.DoneButtonDisabled); //TODO
            Assert.AreEqual(0, cl[3].ApprovalEnabled); //false


            Assert.AreEqual("1.2 lvl checkbox", fieldTranslations[1].Text);
            Assert.AreEqual("1.2 lvl checkbox description", fieldTranslations[1].Description);
            Assert.AreEqual(0, _fields[1].DisplayIndex);
            Assert.AreEqual(0, _fields[1].Selected); //false
            Assert.AreEqual(0, _fields[1].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[1].Color);


            Assert.AreEqual("1.3 lvl", checkLisTranslations[4].Text);
            Assert.AreEqual("1.3 lvl description", checkLisTranslations[4].Description);
            Assert.AreEqual(2, cl[4].DisplayIndex);


            Assert.AreEqual("1.3.1 lvl", checkLisTranslations[5].Text);
            Assert.AreEqual("1.3.1 lvl description", checkLisTranslations[5].Description);
            Assert.AreEqual(0, cl[5].DisplayIndex);
            Assert.AreEqual(0, cl[5].ReviewEnabled); //false
            // Assert.AreEqual(0, cl[5].manual_sync);//todo false
            Assert.AreEqual(0, cl[5].ExtraFieldsEnabled); //false
            // Assert.AreEqual(0, cl[5].donebuttondisabled)//TODO
            Assert.AreEqual(0, cl[5].ApprovalEnabled); //false

            Assert.AreEqual("1.3.1 lvl checkbox", fieldTranslations[2].Text);
            Assert.AreEqual("1.3.1 lvl checkbox description", fieldTranslations[2].Description);
            Assert.AreEqual(0, _fields[2].DisplayIndex);
            Assert.AreEqual(0, _fields[2].Selected); //false
            Assert.AreEqual(0, _fields[2].Mandatory); //false
            Assert.AreEqual(Constants.FieldColors.Default, _fields[2].Color);
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