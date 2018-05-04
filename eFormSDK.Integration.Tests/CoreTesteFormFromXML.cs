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
    public class CoreTesteFormFromXML : DbTestFixture
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

        [Test] // Core_Template_TemplateFromXml_ReturnsTemplate()
        public void Core_eForm_SimpleCommenteFromFromXML_ReturnsTemplate()
        {
            //Arrance
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
            //Act
            var match = sut.TemplateFromXml(xmlstring);

            //Assert
            Assert.NotNull(match);
            Assert.AreEqual("", match.CaseType);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual(1, match.Id);
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

            //Assert.AreEqual(dE.Description, cd); TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            //Assert.AreEqual(dE.ManualSync) //TODO No Method for ManualSync 
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            //Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO DoneButtonDisabled no method found
            Assert.AreEqual(false, dE.ApprovalEnabled);

            Comment commentField = (Comment)dE.DataItemList[0];
            Assert.AreEqual("CommentField", commentField.Label);
            //Assert.AreEqual(commentField.Description, cd);
            Assert.AreEqual(0, commentField.DisplayOrder);
            //Assert.AreEqual(commentField.Multi, 0) //TODO No method MULTI
            //Assert.AreEqual(commentField.geolocation, false) //TODO no method geolocation
            //Assert.AreEqual(commentField.Split, false) //TODO no method Split
            Assert.AreEqual("", commentField.Value);
            Assert.AreEqual(false, commentField.ReadOnly);
            Assert.AreEqual(false, commentField.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Grey, commentField.Color);


        }

        [Test]
        public void Core_eForm_SimplePictureFromFromXML_ReturnsTemplate()
        {
            //Arrance
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

            //Act
            var match = sut.TemplateFromXml(xmlstring);


            //Assert
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Picture test", match.Label);
            //Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            //Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            //Assert.AreEqual(match.review, false); //TODO no method review
            //Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());
            Assert.AreEqual("Picture test", dE.Label);
            //Assert.AreEqual(dE.DisplayOrder, CDataValue); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            //Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            //Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(false, dE.ApprovalEnabled);

            Picture pictureField = (Picture)dE.DataItemList[0];
            Assert.AreEqual("Take two pictures", pictureField.Label);
            //Assert.AreEqual(pictureField.Description, CDataValue) //TODO
            Assert.AreEqual(0, pictureField.DisplayOrder);
            Assert.AreEqual(false, pictureField.Mandatory);
            Assert.AreEqual(Constants.FieldColors.Grey, pictureField.Color);

        }

        [Test]
        public void Core_eForm_SimpleDateFromFromXML_ReturnsTemplate()
        {
            //Arrance
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


            var match = sut.TemplateFromXml(xmlstring);


            //Assert
            Assert.NotNull(match);
            Assert.AreEqual(1, match.Repeated);
            Assert.AreEqual("Date", match.Label);
            //Assert.AreEqual("2018-04-25 00:00:00", match.StartDate); TODO
            //Assert.AreEqual("2028-04-25", match.EndDate); TODO
            Assert.AreEqual("da", match.Language);
            Assert.AreEqual(false, match.MultiApproval);
            Assert.AreEqual(false, match.FastNavigation);
            //Assert.AreEqual(match.review, false); //TODO no method review
            //Assert.AreEqual(match.summary, false); //TODO no method summary
            Assert.AreEqual(0, match.DisplayOrder);

            Assert.AreEqual(1, match.ElementList.Count());
            DataElement dE = (DataElement)match.ElementList[0];
            Assert.AreEqual(1, dE.DataItemList.Count());
            Assert.AreEqual("Date", dE.Label);
            //Assert.AreEqual(dE.DisplayOrder, CDataValue); //TODO
            Assert.AreEqual(0, dE.DisplayOrder);
            Assert.AreEqual(false, dE.ReviewEnabled);
            //Assert.AreEqual(dE.manualsync, false); //TODO NO METHOD MANUALSYNC
            Assert.AreEqual(false, dE.ExtraFieldsEnabled);
            //Assert.AreEqual(dE.DoneButtonDisabled, false); //TODO no method donebuttondisabled
            Assert.AreEqual(false, dE.ApprovalEnabled);

            Date dateField = (Date)dE.DataItemList[0];
            Assert.AreEqual("Select date", dateField.Label);
            //Assert.AreEqual(dateField.Description, CDataValue) //TODO
            Assert.AreEqual(0, dateField.DisplayOrder);
            //Assert.AreEqual("2018-04-25 00:00:00", dateField.MinValue); //TODO 
            //Assert.AreEqual("2028-04-25", dateField.MaxValue); //TODO 
            Assert.AreEqual(false, dateField.Mandatory);
            Assert.AreEqual(false, dateField.ReadOnly);
            Assert.AreEqual(Constants.FieldColors.Grey, dateField.Color);

        }

        [Test]
        public void Core_eForm_SimplePdfFromFromXML_ReturnsTemplate()
        {
            //Arrance
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
        }

        [Test]
        public void Core_eForm_SimpleFieldGroupsFromFromXML_ReturnsTemplate()
        {
            //Arrance
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
        }

        [Test]
        public void Core_eForm_SimplePictureAndSignatureFromFromXML_ReturnsTemplate()
        {
            //Arrance
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
        }

        [Test]
        public void Core_eForm_OptionsWithMicrotingFromFromXML_ReturnsTemplate()
        {
            //Arrance
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
        }

        [Test]
        public void Core_eForm_SimpleMultiSelectFromXML_ReturnsTemplate()
        {
            //Arrance
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
        }

        [Test]
        public void Core_eForm_MultiLvleFormFromXML_ReturnsTemplate()
        {
            //Arrance
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