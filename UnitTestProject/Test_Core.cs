using Microting;
using eFormCommunicator;
using eFormRequest;
using eFormSqlController;
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
        Core core;
        SqlControllerUnitTest sqlConUT;
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

        #region tests - basic
        [TestMethod]
        public void Test_001_SetupAndCleanUp()
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
            CleanUpDb();
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void Test_002_XmlImporter()
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
        public void Test_003_XmlConverter()
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
            core.Close();
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void Test_004_TemplatCreate()
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
            CleanUpDb();
            Assert.AreNotEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void Test_005_TemplatRead()
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
            CleanUpDb();
            Assert.AreEqual(checkValueA, checkValueB);
        }
        #endregion

        #region tests - communicator
        [TestMethod]
        public void Test_011_PostXml()
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

                CleanUpMicroting(com, mUId, siteId);
            }


            //...
            //Assert
            CleanUpDb();
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void Test_012_CheckStatus()
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

                    CleanUpMicroting(com, mUId, siteId);
                }
            }


            //...
            //Assert
            CleanUpDb();
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void Test_013_Retrieve()
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

                    CleanUpMicroting(com, mUId, siteId);
                }
            }


            //...
            //Assert
            CleanUpDb();
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void Test_014_RetrieveFromId()
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

                    CleanUpMicroting(com, mUId, siteId);
                }
            }


            //...
            //Assert
            CleanUpDb();
            Assert.AreEqual(checkValueA, checkValueB);
        }

        [TestMethod]
        public void Test_015_Delete()
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
                CleanUpMicroting(com, mUId, siteId);

                if (responseStr.Contains("<Response><Value type=\"success\">"))
                {
                    checkValueB = true;
                }
            }


            //...
            //Assert
            CleanUpDb();
            Assert.AreEqual(checkValueA, checkValueB);
        }
        #endregion

        

        #region private
        private void Setup()
        {
            #region read settings
            string[] lines = File.ReadAllLines("Input.txt");

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
        }

        private void CleanUpDb()
        {
            try
            {
                List<string> lstMUIds = sqlConUT.CleanActiveCases();
                foreach (string mUId in lstMUIds)
                {
                    core.CaseDelete(mUId);
                }

                sqlConUT.CleanUpDB();

                core.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("CleanUp failed", ex);
            }
        }

        private void CleanUpMicroting(Communicator communicator, string microtingUId, int siteId)
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
                            throw new Exception("CleanUpMicroting failed. Due to Delete failed");
                        }
                    }
                    else
                    {
                        Thread.Sleep(400);
                    }
                }
                throw new Exception("CleanUpMicroting failed. Due to failed 25 attempts");
            }
            catch (Exception ex)
            {
                throw new Exception("CleanUpMicroting failed", ex);
            }
        }


        private string ClearXml(string inputXmlString)
        {
            inputXmlString = t.LocateReplaceAll(inputXmlString, "<StartDate>", "</StartDate>", "xxx");
            inputXmlString = t.LocateReplaceAll(inputXmlString, "<EndDate>", "</EndDate>", "xxx");
            inputXmlString = t.LocateReplaceAll(inputXmlString, "<Language>", "</Language>", "xxx");
            inputXmlString = t.LocateReplaceAll(inputXmlString, "<Id>", "</Id>", "xxx");
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