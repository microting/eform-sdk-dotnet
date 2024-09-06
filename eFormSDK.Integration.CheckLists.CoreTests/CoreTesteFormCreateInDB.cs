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
            Assert.That(cl[0].CaseType, Is.EqualTo(""));
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("CommentMain"));
            // Assert.AreEqual("da", match.Language);
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0));
            // Assert.AreEqual(1, match.ElementList.Count());
            //DataElement dE = (DataElement)cl[1];
            Assert.That(cl.Count, Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("CommentDataElement"));

            //CDataValue cd = new CDataValue();

            Assert.That(checkLisTranslations[1].Description, Is.EqualTo("CommentDataElementDescription"));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0));
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //False
            // Assert.AreEqual(0, cl[1].manual_sync); //false TODO was null
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //False
            // Assert.AreEqual(0, cl[1].done_button_Disabled); //TODO no method
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //False

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.That(_fields.Count(), Is.EqualTo(1));
            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            //Comment commentField = (Comment)dE.DataItemList[0];
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("CommentField"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo("CommentFieldDescription"));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            // Assert.AreEqual(1, _fields[0].multi); was null TODO
            // Assert.AreEqual(0, _fields[0].geolocation_enabled); //False TODO was null
            Assert.That(_fields[0].Split, Is.EqualTo(0)); //TODO no method Split
            Assert.That(_fields[0].DefaultValue, Is.EqualTo(""));
            Assert.That(_fields[0].ReadOnly, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("Picture test"));
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0)); //false

            Assert.That(cl.Count, Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("Picture test"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0)); //false
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.That(_fields.Count(), Is.EqualTo(1));
            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Take two pictures"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo(""));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[0].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("Date"));
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0)); //false

            Assert.That(cl.Count, Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("Date"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0)); //false
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.That(_fields.Count(), Is.EqualTo(1));
            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Select date"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo(""));
            // Assert.AreEqual("2018-01-18", _fields.minvalue) //todo
            // Assert.AreEqual("2028-01-18", _fields.maxvalue) //todo
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            // Assert.AreEqual("", _fields[0].value); //TODO
            Assert.That(_fields[0].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[0].ReadOnly, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("ny pdf"));
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0)); //false

            Assert.That(cl.Count, Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("ny pdf"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0)); //false
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.That(_fields.Count(), Is.EqualTo(1));
            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("bla"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo(""));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[0].DefaultValue, Is.EqualTo(null)); //false
            Assert.That(fieldTranslations[0].DefaultValue, Is.EqualTo("20d483dd7791cd6becf089432724c663")); //false
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("Tester grupper"));
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); //TODO
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0));


            Assert.That(cl.Count(), Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("Tester grupper"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0));
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false


            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));

            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Gruppe efter tjekboks"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo(""));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(1));
            Assert.That(_fields[0].DefaultValue, Is.EqualTo("Closed"));
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));


            Assert.That(fieldTranslations[1].Text, Is.EqualTo("Tjekboks inde i gruppe"));
            Assert.That(fieldTranslations[1].Description, Is.EqualTo(""));
            Assert.That(_fields[1].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[1].Selected, Is.EqualTo(0)); //false
            Assert.That(_fields[1].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));


            Assert.That(fieldTranslations[2].Text, Is.EqualTo("Tjekboks før gruppe"));
            Assert.That(fieldTranslations[2].Description, Is.EqualTo(""));
            Assert.That(_fields[2].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[2].Selected, Is.EqualTo(0)); //false
            Assert.That(_fields[2].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[2].Color, Is.EqualTo(Constants.FieldColors.Default));


            Assert.That(fieldTranslations[3].Text, Is.EqualTo("Tjekboks efter gruppe"));
            Assert.That(fieldTranslations[3].Description, Is.EqualTo(""));
            Assert.That(_fields[3].DisplayIndex, Is.EqualTo(2));
            Assert.That(_fields[3].Selected, Is.EqualTo(0)); //false
            Assert.That(_fields[3].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[3].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("Billede og signatur"));
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].Language); //todo
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0));


            Assert.That(cl.Count(), Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("Billede og signatur"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0));
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Tag et billede"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo(""));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[0].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));


            Assert.That(fieldTranslations[1].Text, Is.EqualTo("Skriv"));
            Assert.That(
                fieldTranslations[1].Description,
                Is.EqualTo("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam dui neque, molestie at maximus a, malesuada at mi. Cras venenatis porttitor augue nec sagittis. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris urna massa, sagittis at fringilla ut, convallis sed dolor. Praesent scelerisque magna dolor, quis blandit metus pharetra eu. Cras euismod facilisis risus at ullamcorper. Pellentesque vitae maximus elit. Sed scelerisque nec velit dictum sodales. Duis sed dapibus odio. Sed non luctus sem. Donec eu mollis lectus, nec porta nisl. Aenean a consequat metus, ac auctor arcu. Cras sit amet blandit velit. Pellentesque faucibus eros sed ullamcorper rutrum.<br><br><br>Pellentesque ultrices ex erat. Pellentesque rhoncus eget lectus et scelerisque. Cras vitae diam ex. Ut felis ligula, venenatis ut lorem vel, venenatis convallis turpis. Sed rutrum ac odio ac auctor. Sed mauris ipsum, vulputate ut sodales a, mattis et purus. Nam convallis augue velit, nec blandit ipsum porta vitae. Quisque et iaculis lectus. Donec eu fringilla turpis, id rutrum mauris.<br><br><br>Proin eu sagittis sem. Aenean vel placerat sapien. Praesent et rutrum justo. Mauris consectetur venenatis est, eu vulputate enim elementum eget. In hac habitasse platea dictumst. Sed vehicula nec neque sed posuere. Aenean sodales lectus a purus posuere lacinia. Aenean ut enim vel odio varius placerat. Phasellus faucibus turpis sed arcu ultrices interdum. Sed porta, nisi nec vehicula lacinia, ante tortor tristique justo, vel sagittis felis ligula eu magna. Pellentesque a velit laoreet nunc aliquet ornare sit amet eget lorem. Duis aliquet viverra pretium. Etiam a mauris tellus. Sed viverra eros eget lectus lobortis, in vestibulum lorem rhoncus. Aliquam sem felis, suscipit a gravida ut, eleifend et ipsum. Nullam lacus lacus, rutrum quis sollicitudin et, porta et erat."));
            Assert.That(_fields[1].DisplayIndex, Is.EqualTo(1));
            Assert.That(_fields[1].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[1].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("Muligheder med Microting eForm"));
            // Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            // Assert.AreEqual("2028-04-25", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); todo
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(76));

            Assert.That(cl.Count(), Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("Muligheder med Microting eForm"));
            Assert.That(
                checkLisTranslations[1].Description,
                Is.EqualTo("Tryk her og prøv hvordan du indsamler data med Microting eForm.<br><br><br>God fornøjelse :-)"));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(76));
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(dE.DoneButtonDisabled, true); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));


            Assert.That(fieldTranslations[0].Text, Is.EqualTo("GEM"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo("Tryk her for at gemme dine indtastede data<br><br>"));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[0].DefaultValue, Is.EqualTo(null));
            Assert.That(fieldTranslations[0].DefaultValue, Is.EqualTo("GEM"));


            Assert.That(fieldTranslations[1].Text, Is.EqualTo("START-STOP TID"));
            Assert.That(fieldTranslations[1].Description, Is.EqualTo("Start-stop tid.<br><br>"));
            Assert.That(_fields[1].DisplayIndex, Is.EqualTo(1));
            // Assert.AreEqual(0, _fields[1].stop_on_save);//false TODO
            Assert.That(_fields[1].Mandatory, Is.EqualTo(0)); //false


            Assert.That(fieldTranslations[2].Text, Is.EqualTo("INFO"));
            Assert.That(
                fieldTranslations[2].Description,
                Is.EqualTo("I dette tekstfelt vises ikke redigerbar tekst.<br><br>Er Microting eForm integreret med ERP-system, kan data fra ERP-systemet vises i dette felt fx. baggrundsinformation på kunder.<br>"));
            Assert.That(_fields[2].DisplayIndex, Is.EqualTo(2));


            Assert.That(fieldTranslations[3].Text, Is.EqualTo("PDF"));
            Assert.That(fieldTranslations[3].Description, Is.EqualTo("Her vises PDF-filer.<br>"));
            Assert.That(_fields[3].DisplayIndex, Is.EqualTo(3));
            Assert.That(fieldTranslations[3].DefaultValue, Is.EqualTo("a60ad2d8c22ed24780bfa9a348376232"));
            Assert.That(_fields[3].DefaultValue, Is.EqualTo(null));


            Assert.That(fieldTranslations[4].Text, Is.EqualTo("TJEK"));
            Assert.That(fieldTranslations[4].Description, Is.EqualTo("I et tjekfelt sættes et flueben.<br>"));
            Assert.That(_fields[4].DisplayIndex, Is.EqualTo(5));
            Assert.That(_fields[4].Selected, Is.EqualTo(0)); //false
            Assert.That(_fields[4].Mandatory, Is.EqualTo(0)); //false


            Assert.That(fieldTranslations[5].Text, Is.EqualTo("VÆLG"));
            Assert.That(
                fieldTranslations[5].Description,
                Is.EqualTo("Vælg én eller flere i liste.<br><br>Er Microting eForm integreret med ERP-system, kan valgmulighederne komme derfra.<br>"));
            Assert.That(_fields[5].DisplayIndex, Is.EqualTo(6));
            Assert.That(_fields[5].Mandatory, Is.EqualTo(0)); //false

            var fieldOptionsQuery = DbContext.FieldOptions.Where(x => x.FieldId == _fields[5].Id);
            List<int> fieldOptionIds = await fieldOptionsQuery.Select(x => x.Id).ToListAsync();
            List<FieldOptionTranslation> fieldOptionTranslations = await DbContext.FieldOptionTranslations
                .Where(x => fieldOptionIds.Contains(x.FieldOptionId)).ToListAsync();
            List<FieldOption> fieldOptions = await fieldOptionsQuery.ToListAsync();

            //List<KeyValuePair> kvp = sut.PairRead(_fields[5].KeyValuePairList);
            Assert.That(fieldOptions[0].DisplayOrder, Is.EqualTo("1"));
            Assert.That(fieldOptionTranslations[0].Text, Is.EqualTo("Valgmulighed 1"));
            Assert.That(fieldOptions[0].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[0].Key, Is.EqualTo("1"));


            Assert.That(fieldOptions[1].DisplayOrder, Is.EqualTo("2"));
            Assert.That(fieldOptionTranslations[1].Text, Is.EqualTo("Valgmulighed 2"));
            Assert.That(fieldOptions[1].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[1].Key, Is.EqualTo("2"));


            Assert.That(fieldOptions[2].DisplayOrder, Is.EqualTo("3"));
            Assert.That(fieldOptionTranslations[2].Text, Is.EqualTo("Valgmulighed 3"));
            Assert.That(fieldOptions[2].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[2].Key, Is.EqualTo("3"));


            Assert.That(fieldOptions[3].DisplayOrder, Is.EqualTo("4"));
            Assert.That(fieldOptionTranslations[3].Text, Is.EqualTo("Valgmulighed 4"));
            Assert.That(fieldOptions[3].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[3].Key, Is.EqualTo("4"));


            Assert.That(fieldOptions[4].DisplayOrder, Is.EqualTo("5"));
            Assert.That(fieldOptionTranslations[4].Text, Is.EqualTo("Valgmulighed 5"));
            Assert.That(fieldOptions[4].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[4].Key, Is.EqualTo("5"));


            Assert.That(fieldOptions[5].DisplayOrder, Is.EqualTo("6"));
            Assert.That(fieldOptionTranslations[5].Text, Is.EqualTo("Valgmulighed 6"));
            Assert.That(fieldOptions[5].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[5].Key, Is.EqualTo("6"));

            Assert.That(fieldOptions[6].DisplayOrder, Is.EqualTo("7"));
            Assert.That(fieldOptionTranslations[6].Text, Is.EqualTo("Valgmulighed 7"));
            Assert.That(fieldOptions[6].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[6].Key, Is.EqualTo("7"));

            Assert.That(fieldOptions[7].DisplayOrder, Is.EqualTo("8"));
            Assert.That(fieldOptionTranslations[7].Text, Is.EqualTo("Valgmulighed 8"));
            Assert.That(fieldOptions[7].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[7].Key, Is.EqualTo("8"));

            Assert.That(fieldOptions[8].DisplayOrder, Is.EqualTo("9"));
            Assert.That(fieldOptionTranslations[8].Text, Is.EqualTo("Valgmulighed 9"));
            Assert.That(fieldOptions[8].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[8].Key, Is.EqualTo("9"));

            Assert.That(fieldOptions[9].DisplayOrder, Is.EqualTo("10"));
            Assert.That(fieldOptionTranslations[9].Text, Is.EqualTo("Valgmulighed N"));
            Assert.That(fieldOptions[9].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[9].Key, Is.EqualTo("10"));


            Assert.That(fieldTranslations[6].Text, Is.EqualTo("VÆLG ÉN"));
            Assert.That(
                fieldTranslations[6].Description,
                Is.EqualTo("Vælg én blandt flere valgmuligheder.<br><br>Er Microting eForm integreret med ERP-system, kan valgmulighederne komme derfra."));
            Assert.That(_fields[6].DisplayIndex, Is.EqualTo(7));
            Assert.That(_fields[6].Mandatory, Is.EqualTo(0)); //false

            fieldOptionsQuery = DbContext.FieldOptions.Where(x => x.FieldId == _fields[6].Id);
            fieldOptionIds = await fieldOptionsQuery.Select(x => x.Id).ToListAsync();
            fieldOptionTranslations = await DbContext.FieldOptionTranslations
                .Where(x => fieldOptionIds.Contains(x.FieldOptionId)).ToListAsync();
            fieldOptions = await fieldOptionsQuery.ToListAsync();


            Assert.That(fieldOptions[0].Key, Is.EqualTo("1"));
            Assert.That(fieldOptionTranslations[0].Text, Is.EqualTo("Valgmulighed 1"));
            Assert.That(fieldOptions[0].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[0].DisplayOrder, Is.EqualTo("1"));

            Assert.That(fieldOptions[1].DisplayOrder, Is.EqualTo("2"));
            Assert.That(fieldOptionTranslations[1].Text, Is.EqualTo("Valgmulighed 2"));
            Assert.That(fieldOptions[1].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[1].Key, Is.EqualTo("2"));


            Assert.That(fieldOptions[2].DisplayOrder, Is.EqualTo("3"));
            Assert.That(fieldOptionTranslations[2].Text, Is.EqualTo("Valgmulighed 3"));
            Assert.That(fieldOptions[2].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[2].Key, Is.EqualTo("3"));


            Assert.That(fieldOptions[3].DisplayOrder, Is.EqualTo("4"));
            Assert.That(fieldOptionTranslations[3].Text, Is.EqualTo("Valgmulighed 4"));
            Assert.That(fieldOptions[3].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[3].Key, Is.EqualTo("4"));


            Assert.That(fieldOptions[4].DisplayOrder, Is.EqualTo("5"));
            Assert.That(fieldOptionTranslations[4].Text, Is.EqualTo("Valgmulighed 5"));
            Assert.That(fieldOptions[4].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[4].Key, Is.EqualTo("5"));


            Assert.That(fieldOptions[5].DisplayOrder, Is.EqualTo("6"));
            Assert.That(fieldOptionTranslations[5].Text, Is.EqualTo("Valgmulighed 6"));
            Assert.That(fieldOptions[5].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[5].Key, Is.EqualTo("6"));

            Assert.That(fieldOptions[6].DisplayOrder, Is.EqualTo("7"));
            Assert.That(fieldOptionTranslations[6].Text, Is.EqualTo("Valgmulighed 7"));
            Assert.That(fieldOptions[6].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[6].Key, Is.EqualTo("7"));

            Assert.That(fieldOptions[7].DisplayOrder, Is.EqualTo("8"));
            Assert.That(fieldOptionTranslations[7].Text, Is.EqualTo("Valgmulighed 8"));
            Assert.That(fieldOptions[7].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[7].Key, Is.EqualTo("8"));

            Assert.That(fieldOptions[8].DisplayOrder, Is.EqualTo("9"));
            Assert.That(fieldOptionTranslations[8].Text, Is.EqualTo("Valgmulighed 9"));
            Assert.That(fieldOptions[8].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[8].Key, Is.EqualTo("9"));

            Assert.That(fieldOptions[9].DisplayOrder, Is.EqualTo("10"));
            Assert.That(fieldOptionTranslations[9].Text, Is.EqualTo("Valgmulighed N"));
            Assert.That(fieldOptions[9].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[9].Key, Is.EqualTo("10"));


            Assert.That(fieldTranslations[7].Text, Is.EqualTo("DATO"));
            Assert.That(
                fieldTranslations[7].Description,
                Is.EqualTo("Vælg dato<br><br>Er Microting eForm integreret med ERP-system, kan valgt dato leveres direkte i ERP-system.<br>"));
            Assert.That(_fields[7].DisplayIndex, Is.EqualTo(8));
            // Assert.AreEqual("2016-06-09", d1.MinValue); TODO
            // Assert.AreEqual("2026-06-09", d1.MaxValue); TODO
            Assert.That(_fields[7].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[7].ReadOnly, Is.EqualTo(0)); //false


            Assert.That(fieldTranslations[8].Text, Is.EqualTo("INDTAST TAL"));
            Assert.That(
                fieldTranslations[8].Description,
                Is.EqualTo("Indtast tal og opsæt evt. regler for mindste/højeste tilladte værdi.<br><br>Er Microting eForm integreret med ERP-system, sendes de indtastede værdier direkte til ERP-systemet.<br>"));
            Assert.That(_fields[8].DisplayIndex, Is.EqualTo(9));
            // Assert.AreEqual("", _fields[8].min_value); todo
            // Assert.AreEqual("", _fields[8].max_value); todo
            Assert.That(_fields[8].DecimalCount, Is.EqualTo(0));
            Assert.That(_fields[8].UnitName, Is.EqualTo(""));
            Assert.That(_fields[8].Mandatory, Is.EqualTo(0)); //false


            Assert.That(fieldTranslations[9].Text, Is.EqualTo("SKRIV KORT KOMMENTAR"));
            Assert.That(fieldTranslations[9].Description, Is.EqualTo("Skriv kort kommentar uden linieskift."));
            Assert.That(_fields[9].DisplayIndex, Is.EqualTo(10));
            // Assert.AreEqual(0, _fields[9].multi); todo
            Assert.That(_fields[9].GeolocationEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, _fields[9].split_screen); todo
            Assert.That(fieldTranslations[9].DefaultValue, Is.EqualTo(""));
            Assert.That(_fields[9].DefaultValue, Is.EqualTo(null));
            Assert.That(_fields[9].ReadOnly, Is.EqualTo(0)); //false
            Assert.That(_fields[9].Mandatory, Is.EqualTo(0)); //false


            Assert.That(fieldTranslations[10].Text, Is.EqualTo("FOTO"));
            Assert.That(
                fieldTranslations[10].Description,
                Is.EqualTo("Tag billeder<br><br>Er Microting eForm integreret med ERP-system, kan billederne vises direkte i virksomhedens ERP/andet databasesystem."));
            Assert.That(_fields[10].DisplayIndex, Is.EqualTo(11));
            Assert.That(_fields[10].Mandatory, Is.EqualTo(0)); //false


            Assert.That(fieldTranslations[11].Text, Is.EqualTo("SKRIV LANG KOMMENTAR"));
            Assert.That(fieldTranslations[11].Description, Is.EqualTo("Skriv længere kommentar med mulighed for linieskift."));
            Assert.That(_fields[11].DisplayIndex, Is.EqualTo(12));
            // Assert.AreEqual(1, cc1.multi);
            // Assert.AreEqual(false, cc1.geolocation);
            Assert.That(_fields[11].DefaultValue, Is.EqualTo(""));
            Assert.That(_fields[11].ReadOnly, Is.EqualTo(0)); //false
            Assert.That(_fields[11].Mandatory, Is.EqualTo(0)); //false


            Assert.That(fieldTranslations[12].Text, Is.EqualTo("UNDERSKRIFT"));
            Assert.That(
                fieldTranslations[12].Description,
                Is.EqualTo("Underskrift<br><br>Er Microting eForm integreret med ERP-system, kan underskrifterne sendes direkte til ERP/andet databasesystem."));
            Assert.That(_fields[12].DisplayIndex, Is.EqualTo(13));
            Assert.That(_fields[12].Mandatory, Is.EqualTo(0)); //false


            Assert.That(fieldTranslations[13].Text, Is.EqualTo("GEM"));
            Assert.That(fieldTranslations[13].Description,
                Is.EqualTo("<br>Tryk for at gemme data.<br>Press to save data.<br>"));
            Assert.That(_fields[13].DisplayIndex, Is.EqualTo(14));
            Assert.That(fieldTranslations[13].DefaultValue, Is.EqualTo("GEM/SAVE"));
            Assert.That(_fields[13].DefaultValue, Is.EqualTo(null));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("Multiselect"));
            // Assert.AreEqual("2017-01-22", match.StartDate); TODO
            // Assert.AreEqual("2027-01-22", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].Language); todo
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0));

            Assert.That(cl.Count(), Is.EqualTo(2));


            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("Multiselect"));
            // Assert.AreEqual(CDataValue, dE.Text); //TODO
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0));
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(false, dE.DoneButtondisabled); //TODO
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();


            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Flere valg"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo("sfsfs"));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[0].Mandatory, Is.EqualTo(0)); //false


            var fieldOptionsQuery = DbContext.FieldOptions.Where(x => x.FieldId == _fields[0].Id);
            List<int> fieldOptionIds = await fieldOptionsQuery.Select(x => x.Id).ToListAsync();
            List<FieldOptionTranslation> fieldOptionTranslations = await DbContext.FieldOptionTranslations
                .Where(x => fieldOptionIds.Contains(x.FieldOptionId)).ToListAsync();
            List<FieldOption> fieldOptions = await fieldOptionsQuery.ToListAsync();

            Assert.That(fieldOptions[0].Key, Is.EqualTo("1"));
            // Assert.AreEqual(CData, fieldOptionTranslations[0].Text); todo
            Assert.That(fieldOptions[0].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[0].DisplayOrder, Is.EqualTo("1"));

            Assert.That(fieldOptions[1].Key, Is.EqualTo("2"));
            // Assert.AreEqual(CData, fieldOptionTranslations[0].Text); todo
            Assert.That(fieldOptions[1].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[1].DisplayOrder, Is.EqualTo("2"));

            Assert.That(fieldOptions[2].Key, Is.EqualTo("3"));
            // Assert.AreEqual(CData, fieldOptionTranslations[0].Text); todo
            Assert.That(fieldOptions[2].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[2].DisplayOrder, Is.EqualTo("3"));

            Assert.That(fieldOptions[3].Key, Is.EqualTo("4"));
            // Assert.AreEqual(CData, fieldOptionTranslations[0].Text); todo
            Assert.That(fieldOptions[3].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[3].DisplayOrder, Is.EqualTo("4"));


            Assert.That(fieldTranslations[1].Text, Is.EqualTo("Choose one option"));
            Assert.That(fieldTranslations[1].Description, Is.EqualTo("This is a description"));
            Assert.That(_fields[1].DisplayIndex, Is.EqualTo(1));
            Assert.That(_fields[1].Mandatory, Is.EqualTo(0)); //false


            fieldOptionsQuery = DbContext.FieldOptions.Where(x => x.FieldId == _fields[1].Id);
            fieldOptionIds = await fieldOptionsQuery.Select(x => x.Id).ToListAsync();
            fieldOptionTranslations = await DbContext.FieldOptionTranslations
                .Where(x => fieldOptionIds.Contains(x.FieldOptionId)).ToListAsync();
            fieldOptions = await fieldOptionsQuery.ToListAsync();

            Assert.That(fieldOptions[0].Key, Is.EqualTo("1"));
            // Assert.AreEqual(CData, fieldOptionTranslations[0].Text); todo
            Assert.That(fieldOptions[0].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[0].DisplayOrder, Is.EqualTo("1"));

            Assert.That(fieldOptions[1].Key, Is.EqualTo("2"));
            // Assert.AreEqual(CData, fieldOptionTranslations[0].Text); todo
            Assert.That(fieldOptions[1].Selected, Is.EqualTo(false)); //false
            Assert.That(fieldOptions[1].DisplayOrder, Is.EqualTo("2"));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("Single Select"));
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); todo
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(cl[0].summary, false); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0));

            Assert.That(cl.Count(), Is.EqualTo(2));

            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("Single Select"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            // Assert.AreEqual(CDataValue, dE.Text); //TODO
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0));
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[1].manual_sync); //todo
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false;
            // Assert.AreEqual(false, cl[0].donebuttondisabled); //TODO
            Assert.That(cl[0].ApprovalEnabled, Is.EqualTo(0)); //false


            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Single Select 1"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo("Single Select 1 description"));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[0].Mandatory, Is.EqualTo(0)); //false

            //List<KeyValuePair> kvp = sut.PairRead(_fields[0].KeyValuePairList);

            var fieldOptionsQuery = DbContext.FieldOptions.Where(x => x.FieldId == _fields[0].Id);
            List<int> fieldOptionIds = await fieldOptionsQuery.Select(x => x.Id).ToListAsync();
            List<FieldOptionTranslation> fieldOptionTranslations = await DbContext.FieldOptionTranslations
                .Where(x => fieldOptionIds.Contains(x.FieldOptionId)).ToListAsync();
            List<FieldOption> fieldOptions = await fieldOptionsQuery.ToListAsync();

            Assert.That(fieldOptions[0].Key, Is.EqualTo("1"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(fieldOptions[0].Selected, Is.EqualTo(false));
            Assert.That(fieldOptions[0].DisplayOrder, Is.EqualTo("1"));


            Assert.That(fieldOptions[1].Key, Is.EqualTo("2"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(fieldOptions[1].Selected, Is.EqualTo(false));
            Assert.That(fieldOptions[1].DisplayOrder, Is.EqualTo("2"));

            Assert.That(fieldOptions[2].Key, Is.EqualTo("3"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(fieldOptions[2].Selected, Is.EqualTo(false));
            Assert.That(fieldOptions[2].DisplayOrder, Is.EqualTo("3"));

            Assert.That(fieldOptions[3].Key, Is.EqualTo("4"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(fieldOptions[3].Selected, Is.EqualTo(false));
            Assert.That(fieldOptions[3].DisplayOrder, Is.EqualTo("4"));

            Assert.That(fieldOptions[4].Key, Is.EqualTo("5"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(fieldOptions[4].Selected, Is.EqualTo(false));
            Assert.That(fieldOptions[4].DisplayOrder, Is.EqualTo("5"));

            Assert.That(fieldOptions[5].Key, Is.EqualTo("6"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(fieldOptions[5].Selected, Is.EqualTo(false));
            Assert.That(fieldOptions[5].DisplayOrder, Is.EqualTo("6"));

            Assert.That(fieldOptions[6].Key, Is.EqualTo("7"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(fieldOptions[6].Selected, Is.EqualTo(false));
            Assert.That(fieldOptions[6].DisplayOrder, Is.EqualTo("7"));

            Assert.That(fieldOptions[7].Key, Is.EqualTo("8"));
            // Assert.AreEqual(CData, kP.Value); TODO
            Assert.That(fieldOptions[7].Selected, Is.EqualTo(false));
            Assert.That(fieldOptions[7].DisplayOrder, Is.EqualTo("8"));

            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("comment"));
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0)); //false

            Assert.That(cl.Count, Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("comment"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0)); //false
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.That(_fields.Count(), Is.EqualTo(1));
            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Comment"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo(""));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            // Assert.AreEqual(1, _fields[0].multi); todo meant to be false but was null
            // Assert.AreEqual(0, _fields[0].geolocation_enabled); //todo meant to be false but was null
            Assert.That(_fields[0].Split, Is.EqualTo(0)); //false
            Assert.That(_fields[0].DefaultValue, Is.EqualTo(""));
            Assert.That(_fields[0].ReadOnly, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("Single line"));
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0)); //false

            Assert.That(cl.Count, Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("Single line"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0)); //false
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.That(_fields.Count(), Is.EqualTo(1));
            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Single line 1"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo("Single line 1 description"));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            // Assert.AreEqual(1, _fields[0].multi); todo meant to be false but was null
            // Assert.AreEqual(0, _fields[0].geolocation_enabled); //todo meant to be false but was null
            // Assert.AreEqual(0, _fields[0].split_screen); //false todo meant to be false but was null
            Assert.That(_fields[0].DefaultValue, Is.EqualTo(null));
            Assert.That(fieldTranslations[0].DefaultValue, Is.EqualTo(""));
            Assert.That(_fields[0].ReadOnly, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("Number 1"));
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0)); //false

            Assert.That(cl.Count, Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("Number 1"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0)); //false
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.That(_fields.Count(), Is.EqualTo(1));
            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Number 1"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo("Number 1 description"));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[0].MinValue, Is.EqualTo("1"));
            Assert.That(_fields[0].MaxValue, Is.EqualTo("1100"));
            // Assert.AreEqual("24", _fields[0].field_values); TODO NO METHOD NAMED VALUE
            Assert.That(_fields[0].DecimalCount, Is.EqualTo(2));
            Assert.That(_fields[0].UnitName, Is.EqualTo(""));
            Assert.That(_fields[0].ReadOnly, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Mandatory, Is.EqualTo(1)); //true
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("Info box"));
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0)); //false

            Assert.That(cl.Count, Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("Info box"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0)); //false
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.That(_fields.Count(), Is.EqualTo(1));
            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Info box 1"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo("Info box 1 description"));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("checkbox"));
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0)); //false

            Assert.That(cl.Count, Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("checkbox"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0)); //false
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.That(_fields.Count(), Is.EqualTo(1));
            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Checkbox 1"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo("Checkbox 1 description"));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[0].Selected, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("TimerStartStop"));
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0)); //false

            Assert.That(cl.Count, Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("TimerStartStop"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0)); //false
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.That(_fields.Count(), Is.EqualTo(1));
            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Timer Start Stop 1"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo("Timer Start Stop 1 Description"));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            // Assert.AreEqual(0, _fields[0].stop_on_save); //todo meant to be false be was null
            Assert.That(_fields[0].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("Save button"));
            // Assert.AreEqual("2017-08-04", match.StartDate); TODO
            // Assert.AreEqual("2027-08-04", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); TODO no method
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[0].review); //TODO no method review
            // Assert.AreEqual(0, cl[0].summary); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0)); //false

            Assert.That(cl.Count, Is.EqualTo(2));
            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("Save button"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo(""));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0)); //false
            Assert.That(cl[1].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[1].manual_sync); // meant to be false but was Null
            Assert.That(cl[1].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(cl[1].doneButtonDisabled, true); //TODO no method donebuttondisabled
            Assert.That(cl[1].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.NotNull(_fields);
            Assert.That(_fields.Count(), Is.EqualTo(1));
            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[1].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("Save button 1"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo("Save button 1 Description"));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            // Assert.AreEqual(0, _fields[0].value); //todo meant to be false be was null
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));
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
            Assert.That(cl[0].Repeated, Is.EqualTo(1));
            Assert.That(checkLisTranslations[0].Text, Is.EqualTo("MultiLvlTest"));
            // Assert.AreEqual("2018-05-08", match.StartDate); TODO
            // Assert.AreEqual("2028-05-08", match.EndDate); TODO
            // Assert.AreEqual("da", cl[0].language); todo
            Assert.That(cl[0].MultiApproval, Is.EqualTo(0)); //false
            Assert.That(cl[0].FastNavigation, Is.EqualTo(0)); //false
            // Assert.AreEqual(match.review, false); //TODO no method review
            // Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.That(cl[0].DisplayIndex, Is.EqualTo(0));


            Assert.That(cl.Count(), Is.EqualTo(6));

            Assert.That(checkLisTranslations[1].Text, Is.EqualTo("1 lvl"));
            Assert.That(checkLisTranslations[1].Description, Is.EqualTo("1 lvl description"));
            Assert.That(cl[1].DisplayIndex, Is.EqualTo(0));


            Assert.That(checkLisTranslations[2].Text, Is.EqualTo("1.1 lvl"));
            Assert.That(checkLisTranslations[2].Description, Is.EqualTo("1.1 lvl description"));
            Assert.That(cl[2].DisplayIndex, Is.EqualTo(0));
            Assert.That(cl[2].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(false, dE.manualSync) //TODO
            Assert.That(cl[2].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(false, dE.DoneButtondisabled); //todo
            Assert.That(cl[2].ApprovalEnabled, Is.EqualTo(0)); //false

            List<Field> _fields = DbContext.Fields.AsNoTracking().ToList();

            Assert.That(_fields[0].CheckListId, Is.EqualTo(cl[2].Id));
            Assert.That(fieldTranslations[0].Text, Is.EqualTo("1.1 lvl checkbox"));
            Assert.That(fieldTranslations[0].Description, Is.EqualTo("1.1 lvl cehckbox description"));
            Assert.That(_fields[0].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[0].Selected, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[0].Color, Is.EqualTo(Constants.FieldColors.Default));


            Assert.That(checkLisTranslations[3].Text, Is.EqualTo("1.2 lvl"));
            Assert.That(checkLisTranslations[3].Description, Is.EqualTo("1.2 lvl description"));
            Assert.That(cl[3].DisplayIndex, Is.EqualTo(1));
            Assert.That(cl[3].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(false, dE2.manualsync)//TODO
            Assert.That(cl[3].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(false, dE2.DoneButtonDisabled); //TODO
            Assert.That(cl[3].ApprovalEnabled, Is.EqualTo(0)); //false


            Assert.That(fieldTranslations[1].Text, Is.EqualTo("1.2 lvl checkbox"));
            Assert.That(fieldTranslations[1].Description, Is.EqualTo("1.2 lvl checkbox description"));
            Assert.That(_fields[1].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[1].Selected, Is.EqualTo(0)); //false
            Assert.That(_fields[1].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[1].Color, Is.EqualTo(Constants.FieldColors.Default));


            Assert.That(checkLisTranslations[4].Text, Is.EqualTo("1.3 lvl"));
            Assert.That(checkLisTranslations[4].Description, Is.EqualTo("1.3 lvl description"));
            Assert.That(cl[4].DisplayIndex, Is.EqualTo(2));


            Assert.That(checkLisTranslations[5].Text, Is.EqualTo("1.3.1 lvl"));
            Assert.That(checkLisTranslations[5].Description, Is.EqualTo("1.3.1 lvl description"));
            Assert.That(cl[5].DisplayIndex, Is.EqualTo(0));
            Assert.That(cl[5].ReviewEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[5].manual_sync);//todo false
            Assert.That(cl[5].ExtraFieldsEnabled, Is.EqualTo(0)); //false
            // Assert.AreEqual(0, cl[5].donebuttondisabled)//TODO
            Assert.That(cl[5].ApprovalEnabled, Is.EqualTo(0)); //false

            Assert.That(fieldTranslations[2].Text, Is.EqualTo("1.3.1 lvl checkbox"));
            Assert.That(fieldTranslations[2].Description, Is.EqualTo("1.3.1 lvl checkbox description"));
            Assert.That(_fields[2].DisplayIndex, Is.EqualTo(0));
            Assert.That(_fields[2].Selected, Is.EqualTo(0)); //false
            Assert.That(_fields[2].Mandatory, Is.EqualTo(0)); //false
            Assert.That(_fields[2].Color, Is.EqualTo(Constants.FieldColors.Default));
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