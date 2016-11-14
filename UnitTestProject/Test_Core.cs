using eFormCommunicator;
using eFormRequest;
using eFormResponse;
using eFormSqlController;
using eFormSubscriber;
using Microting;
using Trools;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace UnitTestProject
{
    [TestClass]
    public class Test_Core
    {
        #region var
        object _filLock = new object();
        object _setupLock = new object();
        Core                    core;
        Communicator            communicator;
        SqlController           sqlCon;
        SqlControllerUnitTest   sqlConUT;
        Tools t = new Tools();

        string comToken;
        string comAddress;
        string subscriberToken;
        string subscriberAddress;
        string subscriberName;
        string serverConnectionString;
        int userId;
        string fileLocation; 
        bool makeLog;
        int siteId;
        #endregion

        #region tests - 00x - basic
        [TestMethod]
        public void T001_Basic_SetupAndCleanUp()
        {
            //Arrange
            Setup();
            int checkValueA;
            int checkValueB;


            //...
            //Act
            checkValueA = 1;

            checkValueB = 1;


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T002_Basic_XmlImporter()
        {
            //Arrange
            string checkValueA;
            string checkValueB;


            //...
            //Act
            checkValueA = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Main xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Id>1</Id>  <Label>A container check list</Label>  <DisplayOrder>1</DisplayOrder>  <CheckListFolderName>Main element</CheckListFolderName>  <Repeated>1</Repeated>  <StartDate>11-10-2016</StartDate>  <EndDate>11-10-2017</EndDate>  <Language>en</Language>  <MultiApproval>true</MultiApproval>  <FastNavigation>false</FastNavigation>  <DownloadEntities>false</DownloadEntities>  <ManualSync>true</ManualSync>  <ElementList>    <Element xsi:type=\"DataElement\">      <Id>1</Id>      <Label>Basic list</Label>      <DisplayOrder>1</DisplayOrder>      <Description>Data element</Description>      <ApprovalEnabled>true</ApprovalEnabled>      <ReviewEnabled>true</ReviewEnabled>      <DoneButtonEnabled>true</DoneButtonEnabled>      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>      <PinkBarText />      <DataItemList>        <DataItem xsi:type=\"Number\">          <Id>1</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Number field</Label>          <Description>this is a description</Description>          <Color>e2f4fb</Color>          <DisplayOrder>1</DisplayOrder>          <MinValue>0</MinValue>          <MaxValue>1000</MaxValue>          <DefaultValue>0</DefaultValue>          <DecimalCount>0</DecimalCount>          <UnitName />        </DataItem>        <DataItem xsi:type=\"Text\">          <Id>2</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Text field</Label>          <Description>this is a description bla</Description>          <Color>e2f4fb</Color>          <DisplayOrder>8</DisplayOrder>          <Value>true</Value>          <MaxLength>100</MaxLength>          <GeolocationEnabled>false</GeolocationEnabled>          <GeolocationForced>false</GeolocationForced>          <GeolocationHidden>true</GeolocationHidden>          <BarcodeEnabled>false</BarcodeEnabled>          <BarcodeType />        </DataItem>        <DataItem xsi:type=\"Comment\">          <Id>3</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Comment field</Label>          <Description>this is a description</Description>          <Color>e2f4fb</Color>          <DisplayOrder>3</DisplayOrder>          <Value>value</Value>          <Maxlength>10000</Maxlength>          <SplitScreen>false</SplitScreen>        </DataItem>        <DataItem xsi:type=\"Picture\">          <Id>4</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Picture field</Label>          <Description>this is a description</Description>          <Color>e2f4fb</Color>          <DisplayOrder>4</DisplayOrder>          <Multi>1</Multi>          <GeolocationEnabled>true</GeolocationEnabled>        </DataItem>        <DataItem xsi:type=\"Check_Box\">          <Id>5</Id>          <Mandatory>false</Mandatory>          <ReadOnly>true</ReadOnly>          <Label>Check box</Label>          <Description>this is a description</Description>          <Color>e2f4fb</Color>          <DisplayOrder>15</DisplayOrder>          <DefaultValue>true</DefaultValue>          <Selected>true</Selected>        </DataItem>        <DataItem xsi:type=\"Date\">          <Id>6</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Date field</Label>          <Description>this is a description</Description>          <Color>e2f4fb</Color>          <DisplayOrder>16</DisplayOrder>          <DefaultValue>11-10-2016 15:20:51</DefaultValue>          <MaxValue>2016-10-11T15:20:51.5733094+02:00</MaxValue>          <MinValue>2016-10-11T15:20:51.5733094+02:00</MinValue>        </DataItem>        <DataItem xsi:type=\"None\">          <Id>7</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>None field, only shows text</Label>          <Description>this is a description</Description>          <Color>e2f4fb</Color>          <DisplayOrder>7</DisplayOrder>        </DataItem>      </DataItemList>    </Element>  </ElementList>  <PushMessageTitle />  <PushMessageBody /></Main>";

            checkValueB = LoadFil("xml.txt");
            checkValueB = checkValueA.Replace("\n", String.Empty).Replace("\r", String.Empty).Replace("\t", String.Empty);


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T003_Basic_XmlConverter()
        {
            //Arrange
            Setup();
            string checkValueA;
            string checkValueB;


            //...
            //Act
            checkValueA = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Main xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Id>1</Id>  <Label>A container check list</Label>  <DisplayOrder>1</DisplayOrder>  <CheckListFolderName>Main element</CheckListFolderName>  <Repeated>1</Repeated>  <StartDate>11-10-2016</StartDate>  <EndDate>11-10-2017</EndDate>  <Language>en</Language>  <MultiApproval>true</MultiApproval>  <FastNavigation>false</FastNavigation>  <DownloadEntities>false</DownloadEntities>  <ManualSync>true</ManualSync>  <ElementList>    <Element xsi:type=\"DataElement\">      <Id>1</Id>      <Label>Basic list</Label>      <DisplayOrder>1</DisplayOrder>      <Description><![CDATA[Data element]]></Description>      <ApprovalEnabled>true</ApprovalEnabled>      <ReviewEnabled>true</ReviewEnabled>      <DoneButtonEnabled>true</DoneButtonEnabled>      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>      <PinkBarText />      <DataItemGroupList />      <DataItemList>        <DataItem xsi:type=\"Number\">          <Id>1</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Number field</Label>          <Description><![CDATA[this is a description]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>1</DisplayOrder>          <MinValue>0</MinValue>          <MaxValue>1000</MaxValue>          <DefaultValue>0</DefaultValue>          <DecimalCount>0</DecimalCount>          <UnitName />        </DataItem>        <DataItem xsi:type=\"Text\">          <Id>2</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Text field</Label>          <Description><![CDATA[this is a description bla]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>8</DisplayOrder>          <Value>true</Value>          <MaxLength>100</MaxLength>          <GeolocationEnabled>false</GeolocationEnabled>          <GeolocationForced>false</GeolocationForced>          <GeolocationHidden>true</GeolocationHidden>          <BarcodeEnabled>false</BarcodeEnabled>          <BarcodeType />        </DataItem>        <DataItem xsi:type=\"Comment\">          <Id>3</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Comment field</Label>          <Description><![CDATA[this is a description]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>3</DisplayOrder>          <Value>value</Value>          <Maxlength>10000</Maxlength>          <SplitScreen>false</SplitScreen>        </DataItem>        <DataItem xsi:type=\"Picture\">          <Id>4</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Picture field</Label>          <Description><![CDATA[this is a description]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>4</DisplayOrder>          <Multi>1</Multi>          <GeolocationEnabled>true</GeolocationEnabled>        </DataItem>        <DataItem xsi:type=\"Check_Box\">          <Id>5</Id>          <Mandatory>false</Mandatory>          <ReadOnly>true</ReadOnly>          <Label>Check box</Label>          <Description><![CDATA[this is a description]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>15</DisplayOrder>          <DefaultValue>true</DefaultValue>          <Selected>true</Selected>        </DataItem>        <DataItem xsi:type=\"Date\">          <Id>6</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Date field</Label>          <Description><![CDATA[this is a description]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>16</DisplayOrder>          <DefaultValue>11-10-2016 15:20:51</DefaultValue>          <MaxValue>2016-10-11T15:20:51.5733094+02:00</MaxValue>          <MinValue>2016-10-11T15:20:51.5733094+02:00</MinValue>        </DataItem>        <DataItem xsi:type=\"None\">          <Id>7</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>None field, only shows text</Label>          <Description><![CDATA[this is a description]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>7</DisplayOrder>        </DataItem>      </DataItemList>    </Element>  </ElementList>  <PushMessageTitle />  <PushMessageBody /></Main>";

            checkValueB = LoadFil("xml.txt");
            MainElement main = core.TemplatFromXml(checkValueA);
            checkValueB = main.ClassToXml();
            checkValueB = checkValueA.Replace("\n", String.Empty).Replace("\r", String.Empty).Replace("\t", String.Empty);


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T004_Basic_TemplatCreate()
        {
            //Arrange
            Setup();
            int checkValueA;
            int checkValueB;
            MainElement main;
            string xmlStr;


            //...
            //Act
            checkValueA = -1;

            checkValueB = -1;
            xmlStr = LoadFil("xml.txt");
            main = core.TemplatFromXml(xmlStr);
            checkValueB = core.TemplatCreate(main);


            //...
            //Assert
            Assert.AreNotEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T005_Basic_TemplatRead()
        {
            //Arrange
            Setup();
            string checkValueA;
            string checkValueB;
            MainElement main;
            int templatId;
            string xmlStr;


            //...
            //Act
            xmlStr = LoadFil("xml.txt");
            main = core.TemplatFromXml(xmlStr);
            checkValueA = main.ClassToXml();
            checkValueA = ClearXml(checkValueA);


            templatId = core.TemplatCreate(main);
            main = core.TemplatRead(templatId);
            checkValueB = main.ClassToXml();
            checkValueB = ClearXml(checkValueB);


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }
        #endregion

        #region tests - 01x - communicator
        [TestMethod]
        public void T011_Communicator_PostXml()
        {
            //Arrange
            Setup();
            bool checkValueA;
            bool checkValueB;
            string xmlStr;
            MainElement main = new MainElement();
            Communicator com = new Communicator(comToken, comAddress);


            //...
            //Act
            checkValueA = true;

            xmlStr = LoadFil("xml.txt");
            main = core.TemplatFromXml(xmlStr);
            string responseStr = com.PostXml(xmlStr, siteId);
            checkValueB = responseStr.Contains("<Response><Value type=\"success\">");

            if (checkValueB == true)
            {
                string mUId = t.Locate(responseStr, "<Value type=\"success\">", "</");

                WaitForAvailableMicroting(mUId);
            }


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T012_Communicator_CheckStatus()
        {
            //Arrange
            Setup();
            bool checkValueA;
            bool checkValueB;
            string xmlStr;
            MainElement main = new MainElement();
            Communicator com = new Communicator(comToken, comAddress);


            //...
            //Act
            checkValueA = true;
            checkValueB = true;

            xmlStr = LoadFil("xml.txt");
            main = core.TemplatFromXml(xmlStr);
            string responseStr = com.PostXml(xmlStr, siteId);

            if (responseStr.Contains("<Response><Value type=\"success\">"))
            {
                string mUId = t.Locate(responseStr, "<Value type=\"success\">", "</");
                responseStr = com.CheckStatus(mUId, siteId);

                if (responseStr.Contains("<Response><Value type=\"success\">"))
                {
                    checkValueB = true;

                    WaitForAvailableMicroting(mUId);
                }
            }


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T013_Communicator_Retrieve()
        {
            //Arrange
            Setup();
            bool checkValueA;
            bool checkValueB;
            string xmlStr;
            MainElement main = new MainElement();
            Communicator com = new Communicator(comToken, comAddress);


            //...
            //Act
            checkValueA = true;
            checkValueB = false;

            xmlStr = LoadFil("xml.txt");
            main = core.TemplatFromXml(xmlStr);
            string responseStr = com.PostXml(xmlStr, siteId);

            if (responseStr.Contains("<Response><Value type=\"success\">"))
            {
                string mUId = t.Locate(responseStr, "<Value type=\"success\">", "</");
                responseStr = com.Retrieve(mUId, siteId);

                if (responseStr.Contains("<Response><Value type="))
                {
                    checkValueB = true;

                    WaitForAvailableMicroting(mUId);
                }
            }


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T014_Communicator_RetrieveFromId()
        {
            //Arrange
            Setup();
            bool checkValueA;
            bool checkValueB;
            string xmlStr;
            MainElement main = new MainElement();
            Communicator com = new Communicator(comToken, comAddress);


            //...
            //Act
            checkValueA = true;
            checkValueB = false;

            xmlStr = LoadFil("xml.txt");
            main = core.TemplatFromXml(xmlStr);
            string responseStr = com.PostXml(xmlStr, siteId);
            string mUId = "";

            if (responseStr.Contains("<Response><Value type=\"success\">"))
            {
                mUId = t.Locate(responseStr, "<Value type=\"success\">", "</");
                responseStr = com.RetrieveFromId(mUId, siteId, "");

                if (responseStr.Contains("<Response><Value type="))
                {
                    checkValueB = true;

                    WaitForAvailableMicroting(mUId);
                }
            }


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T015_Communicator_Delete()
        {
            //Arrange
            Setup();
            bool checkValueA;
            bool checkValueB;
            string xmlStr;
            MainElement main = new MainElement();
            Communicator com = new Communicator(comToken, comAddress);


            //...
            //Act
            checkValueA = true;
            checkValueB = false;

            xmlStr = LoadFil("xml.txt");
            main = core.TemplatFromXml(xmlStr);
            string responseStr = com.PostXml(xmlStr, siteId);

            if (responseStr.Contains("<Response><Value type=\"success\">"))
            {
                string mUId = t.Locate(responseStr, "<Value type=\"success\">", "</");
                WaitForAvailableMicroting(mUId);

                if (responseStr.Contains("<Response><Value type=\"success\">"))
                {
                    checkValueB = true;
                }
            }


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }
        #endregion

        #region tests - 02x - request
        [TestMethod]
        public void T021_Request_ClassToXml()
        {
            //Arrange
            string checkValueA = ClearXml(LoadFil("requestXmlFromClass.txt"));
            string checkValueB = "";

            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddYears(1);
            MainElement main = new MainElement(1, "Sample 3", 1, "Main element", 1, startDate, endDate, "en", true, false, false, true, "", "", "", new List<Element>());
            #region main populated
            GroupElement g1 = new GroupElement(11, "Group of advanced check lists", 1, "Group element", false, false, false, false, "", new List<Element>());
            main.ElementList.Add(g1);


            DataElement e1 = new DataElement(21, "Advanced list", 1, "Data element", true, true, true, false, "", null, new List<eFormRequest.DataItem>());
            g1.ElementList.Add(e1);

            DataElement e2 = new DataElement(22, "Advanced list", 2, "Data element", true, true, true, false, "", null, new List<eFormRequest.DataItem>());
            g1.ElementList.Add(e2);

            DataElement e3 = new DataElement(23, "Advanced list", 3, "Data element", true, true, true, false, "", null, new List<eFormRequest.DataItem>());
            g1.ElementList.Add(e3);


            List<KeyValuePair> singleKeyValuePairList = new List<KeyValuePair>();
            singleKeyValuePairList.Add(new KeyValuePair("1", "option 1", true, "1"));
            singleKeyValuePairList.Add(new KeyValuePair("2", "option 2", false, "2"));
            singleKeyValuePairList.Add(new KeyValuePair("3", "option 3", false, "3"));

            List<KeyValuePair> multiKeyValuePairList = new List<KeyValuePair>();
            multiKeyValuePairList.Add(new KeyValuePair("1", "option 1", true, "1"));
            multiKeyValuePairList.Add(new KeyValuePair("2", "option 2", true, "2"));
            multiKeyValuePairList.Add(new KeyValuePair("3", "option 3", false, "3"));

            e1.DataItemList.Add(new Single_Select("1", false, false, "Single select field", "this is a description", "e2f4fb", 1, singleKeyValuePairList));
            e1.DataItemList.Add(new Multi_Select("2", false, false, "Multi select field", "this is a description", "e2f4fb", 2, multiKeyValuePairList));
            e1.DataItemList.Add(new Audio("3", false, false, "Audio field", "this is a description", "e2f4fb", 3, 1));
            e1.DataItemList.Add(new Comment("5", false, false, "Comment field", "this is a description", "e2f4fb", 5, "value", 10000, false));

            e2.DataItemList.Add(new Number("1", false, false, "Number field", "this is a description", "e2f4fb", 1, 0, 1000, 2, 0, ""));
            e2.DataItemList.Add(new Text("2", false, false, "Text field", "this is a description bla", "e2f4fb", 2, "true", 100, false, false, true, false, ""));
            e2.DataItemList.Add(new Comment("3", false, false, "Comment field", "this is a description", "e2f4fb", 3, "value", 10000, false));
            e2.DataItemList.Add(new Picture("4", false, false, "Picture field", "this is a description", "e2f4fb", 4, 1, true));
            e2.DataItemList.Add(new Check_Box("5", false, false, "Check box", "this is a description", "e2f4fb", 5, true, true));
            e2.DataItemList.Add(new Date("6", false, false, "Date field", "this is a description", "e2f4fb", 6, startDate, startDate, startDate.ToString()));
            e2.DataItemList.Add(new None("7", false, false, "None field, only shows text", "this is a description", "e2f4fb", 7));
            e2.DataItemList.Add(new eFormRequest.Timer("8", false, false, "Timer", "this is a description", "e2f4fb", 8, false));
            e2.DataItemList.Add(new Signature("9", false, false, "Signature", "this is a description", "e2f4fb", 9));

            e3.DataItemList.Add(new Check_Box("1", true, false, "You are sure?", "Verify please", "e2f4fb", 1, false, false));
            #endregion


            //...
            //Act
            checkValueB = ClearXml(main.ClassToXml());


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T022_Request_XmlToClass()
        {
            //Arrange
            string checkValueA = ClearXml(LoadFil("requestXmlFromXml.txt"));
            string checkValueB = LoadFil("requestXmlFromClass.txt");

            MainElement main = new MainElement();


            //...
            //Act
            main = main.XmlToClass(checkValueB);
            checkValueB = ClearXml(main.ClassToXml());


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }
        #endregion

        #region tests - 03x - response
        [TestMethod]
        public void T031_Response_XmlToClassSimple()
        {
            //Arrange
            string responseStr = LoadFil("responseXmlFromXml.txt");
            string checkValueA1 = "903390";
            string checkValueB1 = "";
            Response.ResponseTypes checkValueA2 = Response.ResponseTypes.Success;
            Response.ResponseTypes checkValueB2;

            Response resp = new Response();

            //...
            //Act
            resp = resp.XmlToClass(responseStr);
            checkValueB1 = resp.Value;
            checkValueB2 = resp.Type;


            //...
            //Assert
            Assert.AreEqual(checkValueA1, checkValueB1);
            Assert.AreEqual(checkValueA2, checkValueB2);
        }

        [TestMethod]
        public void T032_Response_XmlToClassExt()
        {
            //Arrange
            string responseStr = LoadFil("responseXmlFromXmlExt.txt");

            string checkValueA1 = "903392";
            string checkValueB1 = "";

            Response.ResponseTypes checkValueA2 = Response.ResponseTypes.Success;
            Response.ResponseTypes checkValueB2;

            string checkValueA3 = "1749";
            string checkValueB3 = "";

            string checkValueA4 = "approved";
            string checkValueB4 = "";

            string checkValueA5 = "42";
            string checkValueB5 = "";

            string checkValueA6 = "2017-10-07";
            string checkValueB6 = "";

            Response resp = new Response();

            //...
            //Act
            resp = resp.XmlToClass(responseStr);
            checkValueB1 = resp.Value;
            checkValueB2 = resp.Type;
            checkValueB3 = resp.Checks[0].UnitId;
            checkValueB4 = resp.Checks[0].ElementList[0].Status;
            checkValueB5 = resp.Checks[0].ElementList[0].DataItemList[0].Value.InderValue;
            checkValueB6 = resp.Checks[0].ElementList[0].DataItemList[5].Value.InderValue;

            //...
            //Assert
            Assert.AreEqual(checkValueA1, checkValueB1);
            Assert.AreEqual(checkValueA2, checkValueB2);
            Assert.AreEqual(checkValueA3, checkValueB3);
            Assert.AreEqual(checkValueA4, checkValueB4);
            Assert.AreEqual(checkValueA5, checkValueB5);
            Assert.AreEqual(checkValueA6, checkValueB6);
        }
        #endregion

        #region tests - 04x - sqlController Templat and Case
        [TestMethod]
        public void T041_SqlController_TemplatCreateAndRead()
        {
            //Arrange
            Setup();
            string checkValueA = ClearXml(LoadFil("requestXmlFromXml.txt"));
            string checkValueB = LoadFil("requestXmlFromClass.txt");
            int templatId = -1;

            MainElement main = new MainElement();


            //...
            //Act
            main = main.XmlToClass(checkValueB);
            templatId = sqlCon.TemplatCreate(main);
            main = sqlCon.TemplatRead(templatId);
            checkValueB = ClearXml(main.ClassToXml());


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T042_SqlController_CaseCreateAndRead()
        {
            //Arrange
            Setup();
            string checkValue1A = "created";
            string checkValue1B = "";
            int checkValue2A = 66;
            int checkValue2B = 0;
            int templatId = -1;

            MainElement main = new MainElement();


            //...
            //Act
            main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
            templatId = sqlCon.TemplatCreate(main);
            main = sqlCon.TemplatRead(templatId);

            List<int> siteIds = new List<int>();
            siteIds.Add(siteId);

            sqlCon.CaseCreate("696969", templatId, 1311, "testCase", DateTime.MinValue, "", "");

            cases aCase = sqlCon.CaseReadFull("696969", null);
            checkValue1B = aCase.workflow_state;
            checkValue2B = (int)aCase.status;


            //...
            //Assert
            Assert.AreEqual(checkValue1A, checkValue1B);
            Assert.AreEqual(checkValue2A, checkValue2B);
        }

        [TestMethod]
        public void T043_SqlController_CaseDelete()
        {
            //Arrange
            Setup();
            string checkValue1A = "removed";
            string checkValue1B = "";
            int checkValue2A = 66;
            int checkValue2B = 0;
            int templatId = -1;

            MainElement main = new MainElement();


            //...
            //Act
            main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
            templatId = sqlCon.TemplatCreate(main);
            main = sqlCon.TemplatRead(templatId);

            List<int> siteIds = new List<int>();
            siteIds.Add(siteId);

            sqlCon.CaseCreate("696969", templatId, 1311, "testCase", DateTime.MinValue, "", "");

            sqlCon.CaseDelete("696969");
            cases aCase = sqlCon.CaseReadFull("696969", null);
            checkValue1B = aCase.workflow_state;
            checkValue2B = (int)aCase.status;


            //...
            //Assert
            Assert.AreEqual(checkValue1A, checkValue1B);
            Assert.AreEqual(checkValue2A, checkValue2B);
        }

        [TestMethod]
        public void T044_SqlController_CaseUpdate()
        {
            //Arrange
            Setup();
            string checkValue1A = "created";
            string checkValue1B = "";
            int checkValue2A = 100;
            int checkValue2B = 0;
            int templatId = -1;

            MainElement main = new MainElement();


            //...
            //Act
            main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
            templatId = sqlCon.TemplatCreate(main);
            main = sqlCon.TemplatRead(templatId);

            List<int> siteIds = new List<int>();
            siteIds.Add(siteId);

            sqlCon.CaseCreate("696969", templatId, 1311, "testCase", DateTime.MinValue, "", "");

            sqlCon.CaseUpdate("696969", DateTime.Now, 7, null, 42);
            cases aCase = sqlCon.CaseReadFull("696969", null);
            checkValue1B = aCase.workflow_state;
            checkValue2B = (int)aCase.status;


            //...
            //Assert
            Assert.AreEqual(checkValue1A, checkValue1B);
            Assert.AreEqual(checkValue2A, checkValue2B);
        }

        [TestMethod]
        public void T045_SqlController_CaseRetract()
        {
            //Arrange
            Setup();
            string checkValue1A = "retracted";
            string checkValue1B = "";
            int checkValue2A = 66;
            int checkValue2B = 0;
            int templatId = -1;

            MainElement main = new MainElement();


            //...
            //Act
            main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
            templatId = sqlCon.TemplatCreate(main);
            main = sqlCon.TemplatRead(templatId);

            List<int> siteIds = new List<int>();
            siteIds.Add(siteId);

            sqlCon.CaseCreate("696969", templatId, 1311, "testCase", DateTime.MinValue, "", "");

            sqlCon.CaseRetract("696969");
            cases aCase = sqlCon.CaseReadFull("696969", null);
            checkValue1B = aCase.workflow_state;
            checkValue2B = (int)aCase.status;


            //...
            //Assert
            Assert.AreEqual(checkValue1A, checkValue1B);
            Assert.AreEqual(checkValue2A, checkValue2B);
        }
        #endregion

        #region tests - 05x - subscriber
        [TestMethod]
        public void T051_Subscriber_StartAndClose()
        {
            //Arrange
            Setup();
            bool checkValueA = true;
            bool checkValueB;
            Subscriber subS = new Subscriber(subscriberToken, subscriberAddress, subscriberName);


            //...
            //Act
            subS.EventMsgClient += EventCaseCreated;
            subS.EventMsgServer += EventCaseCreated;
            subS.Start();
            checkValueB = subS.IsActive();
            subS.Close(true);


            //...
            //Assert
             Assert.AreEqual(checkValueA, checkValueB);
        }
        #endregion

        #region tests - 06x - microting
        [TestMethod]
        public void T061_Core_CaseCreate()
        {
            //Arrange
            Setup();
            int checkValueA = 1;
            int checkValueB;
            int templatId = -1;

            MainElement main = new MainElement();
            SqlControllerUnitTest sqlConUT = new SqlControllerUnitTest(serverConnectionString, userId);


            //...
            //Act
            main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));

            templatId = core.TemplatCreate(main);
            main = core.TemplatRead(templatId);

            List<int> siteIds = new List<int>();
            siteIds.Add(siteId);

            core.CaseCreate(main, "testCase", siteIds, false);
            List<string> mUIds = WaitForAvailableDB();
            checkValueB = mUIds.Count;


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T062_Core_CaseCreateReversed()
        {
            //Arrange
            Setup();
            int checkValueA = 1;
            int checkValueB;
            int templatId = -1;

            MainElement main = new MainElement();
            SqlControllerUnitTest sqlConUT = new SqlControllerUnitTest(serverConnectionString, userId);


            //...
            //Act
            main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));

            templatId = core.TemplatCreate(main);
            main = core.TemplatRead(templatId);

            List<int> siteIds = new List<int>();
            siteIds.Add(siteId);

            core.CaseCreate(main, "testCase", siteIds, true);
            List<string> mUIds = WaitForAvailableDB();
            checkValueB = mUIds.Count;


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T063_Core_CaseDelete()
        {
            //Arrange
            Setup();
            bool checkValueA = true;
            bool checkValueB;
            int templatId = -1;

            MainElement main = new MainElement();
            SqlControllerUnitTest sqlConUT = new SqlControllerUnitTest(serverConnectionString, userId);


            //...
            //Act
            main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));

            templatId = core.TemplatCreate(main);
            main = core.TemplatRead(templatId);

            List<int> siteIds = new List<int>();
            siteIds.Add(siteId);

            core.CaseCreate(main, "testCase", siteIds, false);
            List<string> mUIds = WaitForAvailableDB();

            WaitForAvailableMicroting(mUIds[0]);
            checkValueB = core.CaseDelete(mUIds[0]);


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void T064_Core_CaseLookup()
        {
            //Arrange
            Setup();
            Case_Dto checkValueA = new Case_Dto();
            Case_Dto checkValueB;
            int templatId = -1;

            MainElement main = new MainElement();
            SqlControllerUnitTest sqlConUT = new SqlControllerUnitTest(serverConnectionString, userId);


            //...
            //Act
            main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));

            templatId = core.TemplatCreate(main);
            main = core.TemplatRead(templatId);

            List<int> siteIds = new List<int>();
            siteIds.Add(siteId);

            core.CaseCreate(main, "testCase", siteIds, false);
            List<string> mUIds = WaitForAvailableDB();

            WaitForAvailableMicroting(mUIds[0]);
            checkValueB = core.CaseLookup(mUIds[0]);


            //...
            //Assert
            Assert.AreEqual(checkValueA.GetType(), checkValueB.GetType());
        }

        [TestMethod]
        public void T065_Core_Close()
        {
            //Arrange
            Setup();
            bool checkValueA = true;
            bool checkValueB = true;
 

            //...
            //Act
            core.Close();


            //...
            //Assert
            Assert.AreEqual(checkValueA, checkValueB);
        }
        #endregion



        #region private
        private void Setup()
        {
            lock (_setupLock)
            {
                #region read settings
                string[] lines;
                lock (_filLock)
                {
                    lines = File.ReadAllLines("Input.txt");
                }

                comToken = lines[0];
                comAddress = lines[1];

                subscriberToken = lines[3];
                subscriberAddress = lines[4];
                subscriberName = lines[5];

                serverConnectionString = lines[7];
                userId = int.Parse(lines[8]);

                fileLocation = lines[10];
                #endregion
                makeLog = bool.Parse(lines[12]);
                siteId = int.Parse(lines[13]);
                core = new Core(comToken, comAddress, subscriberToken, subscriberAddress, subscriberName, serverConnectionString, userId, fileLocation, makeLog);

                #region connect event triggers
                core.HandleCaseCreated += EventCaseCreated;
                core.HandleCaseRetrived += EventCaseRetrived;
                core.HandleCaseUpdated += EventCaseUpdated;
                core.HandleCaseDeleted += EventCaseDeleted;
                core.HandleFileDownloaded += EventFileDownloaded;
                core.HandleSiteActivated += EventSiteActivated;
                core.HandleEventLog += EventLog;
                core.HandleEventMessage += EventMessage;
                core.HandleEventWarning += EventWarning;
                core.HandleEventException += EventException;
                #endregion
                core.Start();

                sqlConUT = new SqlControllerUnitTest(serverConnectionString, userId);
                #region clean database
                try
                {
                    List<string> lstMUIds = sqlConUT.CleanActiveCases();
                    foreach (string mUId in lstMUIds)
                        core.CaseDelete(mUId);

                    sqlConUT.CleanUpDB();
                }
                catch (Exception ex)
                {
                    throw new Exception("CleanUp failed", ex);
                }
                #endregion

                sqlCon = new SqlController(serverConnectionString, userId);
                communicator = new Communicator(comToken, comAddress);
            }
        }

        private List<string> WaitForAvailableDB()
        {
            try
            {
                for (int i = 0; i < 25; i++)
                {
                    List<string> lstMUId = sqlConUT.CheckCase();

                    if (lstMUId.Count == 1)
                    {
                        return lstMUId;
                    }
                    else
                    {
                        Thread.Sleep(200);
                    }
                }
                throw new Exception("WaitForAvailableDB failed. Due to failed 25 attempts");
            }
            catch (Exception ex)
            {
                throw new Exception("WaitForAvailableDB failed", ex);
            }
        }

        private void WaitForAvailableMicroting(string microtingUId)
        {
            try
            {
                for (int i = 0; i < 25; i++)
                {
                    string responseStr = communicator.CheckStatus(microtingUId, siteId);

                    if (responseStr.Contains("<Response><Value type=\"success\">"))
                    {
                        responseStr = communicator.Delete(microtingUId, siteId);

                        if (responseStr.Contains("<Response><Value type=\"success\">"))
                        {
                            return;
                        }
                        else
                        {
                            throw new Exception("WaitForAvailableMicroting failed. Due to Delete failed");
                        }
                    }
                    else
                    {
                        Thread.Sleep(400);
                    }
                }
                throw new Exception("WaitForAvailableMicroting failed. Due to failed 25 attempts");
            }
            catch (Exception ex)
            {
                throw new Exception("WaitForAvailableMicroting failed", ex);
            }
        }

        private string ClearXml(string inputXmlString)
        {
            inputXmlString = t.LocateReplaceAll(inputXmlString, "<StartDate>", "</StartDate>", "xxx");
            inputXmlString = t.LocateReplaceAll(inputXmlString, "<EndDate>", "</EndDate>", "xxx");
            inputXmlString = t.LocateReplaceAll(inputXmlString, "<Language>", "</Language>", "xxx");
            inputXmlString = t.LocateReplaceAll(inputXmlString, "<Id>", "</Id>", "xxx");
            inputXmlString = t.LocateReplaceAll(inputXmlString, "<DefaultValue>", "</DefaultValue>", "xxx");
            inputXmlString = t.LocateReplaceAll(inputXmlString, "<MaxValue>", "</MaxValue>", "xxx");
            inputXmlString = t.LocateReplaceAll(inputXmlString, "<MinValue>", "</MinValue>", "xxx");

            return inputXmlString;
        }

        private string LoadFil(string path)
        {
            try
            {
                lock (_filLock)
                {
                    string str = "";
                    using (StreamReader sr = new StreamReader(path, true))
                    {
                        str = sr.ReadToEnd();
                    }
                    return str;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load fil", ex);
            }
        }
        #endregion

        #region events
        public void EventCaseCreated(object sender, EventArgs args)
        {
            ////DOSOMETHING: changed to fit your wishes and needs 
            //Case_Dto temp = (Case_Dto)sender;
            //int siteId = temp.SiteId;
            //string caseType = temp.CaseType;
            //string caseUid = temp.CaseUId;
            //string mUId = temp.MicrotingUId;
            //string checkUId = temp.CheckUId;
        }

        public void EventCaseRetrived(object sender, EventArgs args)
        {
            ////DOSOMETHING: changed to fit your wishes and needs 
            //Case_Dto temp = (Case_Dto)sender;
            //int siteId = temp.SiteId;
            //string caseType = temp.CaseType;
            //string caseUid = temp.CaseUId;
            //string mUId = temp.MicrotingUId;
            //string checkUId = temp.CheckUId;
        }

        public void EventCaseUpdated(object sender, EventArgs args)
        {
            ////DOSOMETHING: changed to fit your wishes and needs
            //Case_Dto temp = (Case_Dto)sender;
            //int siteId = temp.SiteId;
            //string caseType = temp.CaseType;
            //string caseUid = temp.CaseUId;
            //string mUId = temp.MicrotingUId;
            //string checkUId = temp.CheckUId;
        }

        public void EventCaseDeleted(object sender, EventArgs args)
        {
            ////DOSOMETHING: changed to fit your wishes and needs
            //Case_Dto temp = (Case_Dto)sender;
            //int siteId = temp.SiteId;
            //string caseType = temp.CaseType;
            //string caseUid = temp.CaseUId;
            //string mUId = temp.MicrotingUId;
            //string checkUId = temp.CheckUId;
        }

        public void EventFileDownloaded(object sender, EventArgs args)
        {
            ////DOSOMETHING: changed to fit your wishes and needs 
            //File_Dto temp = (File_Dto)sender;
            //int siteId = temp.SiteId;
            //string caseType = temp.CaseType;
            //string caseUid = temp.CaseUId;
            //string mUId = temp.MicrotingUId;
            //string checkUId = temp.CheckUId;
            //string fileLocation = temp.FileLocation;
        }

        public void EventSiteActivated(object sender, EventArgs args)
        {
            ////DOSOMETHING: changed to fit your wishes and needs 
            //int siteId = int.Parse(sender.ToString());
        }

        public void EventLog(object sender, EventArgs args)
        {
            //lock (_logFilLock)
            //{
            //    try
            //    {
            //        //DOSOMETHING: changed to fit your wishes and needs 
            //        //File.AppendAllText(@"log.txt", sender.ToString() + Environment.NewLine);
            //    }
            //    catch (Exception ex)
            //    {
            //        EventException(ex, EventArgs.Empty);
            //    }
            //}
        }

        public void EventMessage(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            //Console.WriteLine(sender.ToString());
        }

        public void EventWarning(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            //Console.WriteLine("## WARNING ## " + sender.ToString() + " ## WARNING ##");
        }

        public void EventException(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            //Exception ex = (Exception)sender;
        }
        #endregion
    }
}