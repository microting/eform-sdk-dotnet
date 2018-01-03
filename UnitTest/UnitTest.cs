using eFormCommunicator;
using eFormCore;
using eFormData;
using eFormShared;
using eFormSqlController;
using eFormSubscriber;
using Rebus.Testing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Xunit;

namespace UnitTest
{
    public class TestContext : IDisposable
    {
        bool useLiveData = false;

        string connectionStringLocal_UnitTest = "Data Source=.\\SQLEXPRESS;Initial Catalog=" + "UnitTest_SDK_" + "Microting" + ";Integrated Security=True"; //Uses unit test data
        string connectionStringLocal_LiveData = "Data Source=.\\SQLEXPRESS;Initial Catalog=" + "UnitTest_SDK_" + "MicrotingLive" + ";Integrated Security=True"; //Uses LIVE data

        #region content
        #region var
        SqlController sqlController;
        string serverConnectionString = "";
        #endregion

        //once for all tests - build order
        public TestContext()
        {
            try
            {
                if (Environment.MachineName.ToLower().Contains("testing") || Environment.MachineName.ToLower().Contains("travis"))
                {
                    serverConnectionString = "Persist Security Info=True;server=localhost;database=" + "UnitTest_SDK_" + "Microting" + ";uid=root;password="; //Uses travis database
                    useLiveData = false; //travis can't use live data
                }
                else
                {
                    if (useLiveData)
                        serverConnectionString = connectionStringLocal_LiveData;
                    else
                        serverConnectionString = connectionStringLocal_UnitTest;
                }
            }
            catch { }

            sqlController = new SqlController(serverConnectionString);
            AdminTools at = new AdminTools(serverConnectionString);

            if (sqlController.SettingRead(Settings.token) == "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")
                at.DbSetup("unittest");
        }

        //once for all tests - teardown
        public void Dispose()
        {
            //sqlController.UnitTest_DeleteDb();
        }

        public string GetConnectionString()
        {
            return serverConnectionString;
        }

        public bool GetUseLiveData()
        {
            return useLiveData;
        }
        #endregion
    }

    [Collection("Database collection")]
    public class UnitTest
    {
        #region var
        Core core;
        CoreUnitTest core_UT;
        SqlController sqlController;
        Communicator communicator;
        AdminTools adminTool;
        Tools t = new Tools();

        object _lockTest = new object();
        object _lockFil = new object();

        int siteId1 = 2001;
        int siteId2 = 2002;
        int workerMUId = 666;
        int unitMUId = 345678;

        bool useLiveData = false;

        string token;
        string comAddressApi;
        string comAddressBasic;
        string comOrganizationId;
        string serverConnectionString = "";
        #endregion

        #region con
        public UnitTest(TestContext testContext)
        {
            serverConnectionString = testContext.GetConnectionString();
            useLiveData = testContext.GetUseLiveData();

            if (useLiveData)
            {
                siteId1 = 3818;
                siteId2 = 3823;
                workerMUId = 1778;
                unitMUId = 4938;
            }
        }
        #endregion

        #region prepare and teardown     
        private void TestPrepare(string testName, bool startSDK)
        {
            adminTool = new AdminTools(serverConnectionString);
            string temp = adminTool.DbClear();
            if (temp != "")
                throw new Exception("CleanUp failed");

            sqlController = new SqlController(serverConnectionString);
            sqlController.UnitTest_TruncateTable(nameof(logs));
            sqlController.UnitTest_TruncateTable(nameof(log_exceptions));

            token = sqlController.SettingRead(Settings.token);
            comAddressApi = sqlController.SettingRead(Settings.comAddressApi);
            comAddressBasic = sqlController.SettingRead(Settings.comAddressBasic);
            comOrganizationId = sqlController.SettingRead(Settings.comOrganizationId);

            core = new Core();
            core_UT = new CoreUnitTest(core);

            core.HandleNotificationNotFound += EventNotificationNotFound;
            core.HandleEventException += EventException;

            if (startSDK)
            {
                core.Start(serverConnectionString);

                communicator = new Communicator(sqlController, core.log);
            }
        }

        private void TestTeardown()
        {
            if (core != null)
                if (core.Running())
                    core_UT.Close();
        }
        #endregion

        #region - test 000x - virtal basics
        [Fact]
        public void Test000_Basics_1a_MustAlwaysPass()
        {
            lock (_lockTest)
            {
                //Arrange
                bool checkValueA = true;
                bool checkValueB = false;

                //Act
                checkValueB = true;

                //Assert
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test000_Basics_2a_PrepareAndTeardownTestdata()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), false);
                bool checkValueA = true;
                bool checkValueB = false;

                //Act
                checkValueB = true;

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }
        #endregion

        #region - test 001x - core
        [Fact]
        public void Test001_Core_1a_Start_WithNullExpection()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), false);
                string checkValueA = "serverConnectionString is not allowed to be null or empty";
                string checkValueB = "";

                //Act
                try
                {
                    checkValueB = core.Start(null) + "";
                }
                catch (Exception ex)
                {
                    checkValueB = ex.InnerException.InnerException.Message;
                }

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test001_Core_1b_Start_WithBlankExpection()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), false);
                string checkValueA = "serverConnectionString is not allowed to be null or empty";
                string checkValueB = "";

                //Act
                try
                {
                    checkValueB = core.Start("").ToString();
                }
                catch (Exception ex)
                {
                    checkValueB = ex.InnerException.InnerException.Message;
                }

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test001_Core_2a_StartSqlOnly()
        {
            //Arrange
            TestPrepare(t.GetMethodName(), false);
            string checkValueA = "True";
            string checkValueB = "";

            //Act
            try
            {
                checkValueB = core.StartSqlOnly(serverConnectionString).ToString();
            }
            catch (Exception ex)
            {
                checkValueB = t.PrintException(t.GetMethodName() + " failed", ex);
            }

            //Assert
            TestTeardown();
            Assert.Equal(checkValueA, checkValueB);
        }

        [Fact]
        public void Test001_Core_3a_Start()
        {
            //Arrange
            TestPrepare(t.GetMethodName(), false);
            string checkValueA = "True";
            string checkValueB = "";

            //Act
            try
            {
                checkValueB = core.Start(serverConnectionString).ToString();
            }
            catch (Exception ex)
            {
                checkValueB = t.PrintException(t.GetMethodName() + " failed", ex);
            }

            //Assert
            TestTeardown();
            Assert.Equal(checkValueA, checkValueB);
        }

        [Fact]
        public void Test001_Core_4a_IsRunning()
        {
            //Arrange
            TestPrepare(t.GetMethodName(), false);
            string checkValueA = "FalseTrue";
            string checkValueB = "";

            //Act
            try
            {
                checkValueB = core.Running().ToString();
                core.Start(serverConnectionString);
                checkValueB += core.Running().ToString();
            }
            catch (Exception ex)
            {
                checkValueB = t.PrintException(t.GetMethodName() + " failed", ex);
            }

            //Assert
            TestTeardown();
            Assert.Equal(checkValueA, checkValueB);
        }

        [Fact]
        public void Test001_Core_5a_Close()
        {
            //Arrange
            TestPrepare(t.GetMethodName(), true);
            string checkValueA = "True";
            string checkValueB = "";

            //Act
            try
            {
                checkValueB = core.Close().ToString();
            }
            catch (Exception ex)
            {
                checkValueB = t.PrintException(t.GetMethodName() + " failed", ex);
            }

            //Assert
            TestTeardown();
            Assert.Equal(checkValueA, checkValueB);
        }

        [Fact]
        public void Test001_Core_6a_RunningForWhileThenClose()
        {
            //Arrange
            TestPrepare(t.GetMethodName(), false);
            string checkValueA = "FalseTrueTrue";
            string checkValueB = "";

            //Act
            try
            {
                checkValueB = core.Running().ToString();
                core.Start(serverConnectionString);
                Thread.Sleep(30000);
                checkValueB += core.Running().ToString();
                Thread.Sleep(05000);
                checkValueB += core.Close().ToString();
            }
            catch (Exception ex)
            {
                checkValueB = t.PrintException(t.GetMethodName() + " failed", ex);
            }

            //Assert
            TestTeardown();
            Assert.Equal(checkValueA, checkValueB);
        }
        #endregion

        #region - test 002x - core (Exception handling)
        [Fact]
        public void Test002_Core_1a_ExceptionHandling()
        {
            #region //Arrange
            TestPrepare(t.GetMethodName(), true);
            string checkValueA1 = "1:100000/100000/10000/0";
            string checkValueA2 = "1:010000/010000/01000/0";
            string checkValueA3 = "1:001000/001000/00100/0";
            string checkValueA4 = "1:000100/000100/00010/0";
            string checkValueB1 = "";
            string checkValueB2 = "";
            string checkValueB3 = "";
            string checkValueB4 = "";
            string tempValue = "";
            MainElement main;
            string xmlStr = LoadFil("xml.txt");

            main = core.TemplateFromXml(xmlStr);
            main.Label = "throw new Exception";
            main.EndDate = DateTime.Now.AddDays(2);
            core.TemplateCreate(main);
            #endregion

            //Act
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    core.CaseCreate(main, "throw new Exception", siteId1);

                    if (core.Running())
                    {
                        tempValue += PrintLogLine();

                        sqlController.UnitTest_TruncateTable(nameof(logs));
                        sqlController.UnitTest_TruncateTable(nameof(log_exceptions));
                    }
                }
            }
            catch (Exception ex)
            {
                tempValue = t.PrintException(t.GetMethodName() + " failed", ex);
            }

            #region //Assert
            TestTeardown();

            tempValue = tempValue.Replace("\r", "").Replace("\n", "");
            checkValueB1 = tempValue.Substring(0, 23);
            checkValueB2 = tempValue.Substring(23, 23);
            checkValueB3 = tempValue.Substring(46, 23);
            checkValueB4 = tempValue.Substring(69, 23);

            if (useLiveData)
                Assert.Equal("Faked due to live data", "Faked due to live data");
            else
            {
                Assert.Equal(checkValueA1, checkValueB1);
                Assert.Equal(checkValueA2, checkValueB2);
                Assert.Equal(checkValueA3, checkValueB3);
                Assert.Equal(checkValueA4, checkValueB4);
            }
            #endregion
        }

        [Fact]
        public void Test002_Core_2a_DoubleExceptionHandling()
        {
            #region //Arrange
            TestPrepare(t.GetMethodName(), true);
            string checkValueA1 = "1:100000/100000/10000/0";
            string checkValueA2 = "1:010000/010000/01000/0";
            string checkValueA3 = "1:010000/010000/01000/0";
            string checkValueA4 = "1:001000/001000/00100/0";
            string checkValueA5 = "1:001000/001000/00100/0";
            string checkValueA6 = "1:000100/000100/00010/0";
            string checkValueA7 = "1:000100/000100/00010/0";
            string checkValueB1 = "";
            string checkValueB2 = "";
            string checkValueB3 = "";
            string checkValueB4 = "";
            string checkValueB5 = "";
            string checkValueB6 = "";
            string checkValueB7 = "";
            string tempValue = "";
            MainElement main1;
            MainElement main2;
            string xmlStr = LoadFil("xml.txt");

            main1 = core.TemplateFromXml(xmlStr);
            main1.Label = "throw new Exception";
            main1.EndDate = DateTime.Now.AddDays(2);
            core.TemplateCreate(main1);

            main2 = core.TemplateFromXml(xmlStr);
            main2.Label = "throw other Exception";
            main2.EndDate = DateTime.Now.AddDays(2);
            core.TemplateCreate(main2);
            #endregion

            //Act
            try
            {
                core.CaseCreate(main1, null, siteId1);
                tempValue += PrintLogLine();
                sqlController.UnitTest_TruncateTable(nameof(logs));
                sqlController.UnitTest_TruncateTable(nameof(log_exceptions));

                for (int i = 0; i < 3; i++)
                {
                    core.CaseCreate(main1, null, siteId1);
                    tempValue += PrintLogLine();
                    sqlController.UnitTest_TruncateTable(nameof(logs));
                    sqlController.UnitTest_TruncateTable(nameof(log_exceptions));

                    core.CaseCreate(main2, null, siteId1);
                    tempValue += PrintLogLine();
                    sqlController.UnitTest_TruncateTable(nameof(logs));
                    sqlController.UnitTest_TruncateTable(nameof(log_exceptions));
                }
            }
            catch (Exception ex)
            {
                tempValue = t.PrintException(t.GetMethodName() + " failed", ex);
            }

            #region //Assert
            TestTeardown();

            tempValue = tempValue.Replace("\r", "").Replace("\n", "");
            checkValueB1 = tempValue.Substring(0, 23);
            checkValueB2 = tempValue.Substring(23, 23);
            checkValueB3 = tempValue.Substring(46, 23);
            checkValueB4 = tempValue.Substring(69, 23);
            checkValueB5 = tempValue.Substring(92, 23);
            checkValueB6 = tempValue.Substring(115, 23);
            checkValueB7 = tempValue.Substring(138, 23);

            if (useLiveData)
                Assert.Equal("Faked due to live data", "Faked due to live data");
            else
            {
                Assert.Equal(checkValueA1, checkValueB1);
                Assert.Equal(checkValueA2, checkValueB2);
                Assert.Equal(checkValueA3, checkValueB3);
                Assert.Equal(checkValueA4, checkValueB4);
                Assert.Equal(checkValueA5, checkValueB5);
                Assert.Equal(checkValueA6, checkValueB6);
                Assert.Equal(checkValueA7, checkValueB7);
            }
            #endregion
        }

        [Fact]
        public void Test002_Core_3a_FatalExceptionHandling()
        {
            #region //Arrange
            TestPrepare(t.GetMethodName(), true);
            string checkValueA1 = "1:100000/100000/10000/0";
            string checkValueA2 = "1:010000/010000/01000/0";
            string checkValueA3 = "1:010000/010000/01000/0";
            string checkValueA4 = "1:001000/001000/00100/0";
            string checkValueA5 = "1:001000/001000/00100/0";
            string checkValueA6 = "1:000100/000100/00010/0";
            string checkValueA7 = "1:000100/000100/00010/0";
            string checkValueA8 = "2:000000/000020/00001/1";
            string checkValueB1 = "";
            string checkValueB2 = "";
            string checkValueB3 = "";
            string checkValueB4 = "";
            string checkValueB5 = "";
            string checkValueB6 = "";
            string checkValueB7 = "";
            string checkValueB8 = "";
            string tempValue = "";
            MainElement main1;
            MainElement main2;
            string xmlStr = LoadFil("xml.txt");

            main1 = core.TemplateFromXml(xmlStr);
            main1.Label = "throw new Exception";
            main1.EndDate = DateTime.Now.AddDays(2);
            core.TemplateCreate(main1);

            main2 = core.TemplateFromXml(xmlStr);
            main2.Label = "throw other Exception";
            main2.EndDate = DateTime.Now.AddDays(2);
            core.TemplateCreate(main2);
            #endregion

            //Act
            try
            {
                #region core.CaseCreate(main1, null, siteId1);
                for (int i = 0; i < 2; i++)
                {
                    core.CaseCreate(main1, null, siteId1);

                    tempValue += PrintLogLine();
                    sqlController.UnitTest_TruncateTable(nameof(logs));
                    sqlController.UnitTest_TruncateTable(nameof(log_exceptions));
                }
                #endregion

                #region core.CaseCreate(main2, null, siteId1);
                #endregion
                #region core.CaseCreate(main1, null, siteId1);
                for (int i = 0; i < 3; i++)
                {
                    core.CaseCreate(main2, null, siteId1);

                    tempValue += PrintLogLine();
                    sqlController.UnitTest_TruncateTable(nameof(logs));
                    sqlController.UnitTest_TruncateTable(nameof(log_exceptions));

                    core.CaseCreate(main1, null, siteId1);

                    tempValue += PrintLogLine();
                    sqlController.UnitTest_TruncateTable(nameof(logs));
                    sqlController.UnitTest_TruncateTable(nameof(log_exceptions));
                }
                #endregion
            }
            catch (Exception ex)
            {
                tempValue = t.PrintException(t.GetMethodName() + " failed", ex);
            }

            #region //Assert
            TestTeardown();

            tempValue = tempValue.Replace("\r", "").Replace("\n", "");
            checkValueB1 = tempValue.Substring(0, 23);
            checkValueB2 = tempValue.Substring(23, 23);
            checkValueB3 = tempValue.Substring(46, 23);
            checkValueB4 = tempValue.Substring(69, 23);
            checkValueB5 = tempValue.Substring(92, 23);
            checkValueB6 = tempValue.Substring(115, 23);
            checkValueB7 = tempValue.Substring(138, 23);
            checkValueB8 = tempValue.Substring(161, 23);

            if (useLiveData)
                Assert.Equal("Faked due to live data", "Faked due to live data");
            else
            {
                Assert.Equal(checkValueA1, checkValueB1);
                Assert.Equal(checkValueA2, checkValueB2);
                Assert.Equal(checkValueA3, checkValueB3);
                Assert.Equal(checkValueA4, checkValueB4);
                Assert.Equal(checkValueA5, checkValueB5);
                Assert.Equal(checkValueA6, checkValueB6);
                Assert.Equal(checkValueA7, checkValueB7);
                Assert.Equal(checkValueA8, checkValueB8);
            }
            #endregion
        }
        #endregion

        #region - test 003x - xml
        [Fact]
        public void Test003_Xml_1a_XmlImporter()
        {
            lock (_lockTest)
            {
                //Arrange
                string checkValueA;
                string checkValueB;

                //Act
                checkValueA = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Main xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Id>1</Id>  <Label>A container check list</Label>  <DisplayOrder>1</DisplayOrder>  <CheckListFolderName>Main element</CheckListFolderName>  <Repeated>1</Repeated>  <StartDate>11-10-2016</StartDate>  <EndDate>11-10-2017</EndDate>  <Language>en</Language>  <MultiApproval>true</MultiApproval>  <FastNavigation>false</FastNavigation>  <DownloadEntities>false</DownloadEntities>  <ManualSync>true</ManualSync>  <ElementList>    <Element xsi:type=\"DataElement\">      <Id>1</Id>      <Label>Basic list</Label>      <DisplayOrder>1</DisplayOrder>      <Description>Data element</Description>      <ApprovalEnabled>true</ApprovalEnabled>      <ReviewEnabled>true</ReviewEnabled>      <DoneButtonEnabled>true</DoneButtonEnabled>      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>      <PinkBarText />      <DataItemList>        <DataItem xsi:type=\"Number\">          <Id>1</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Number field</Label>          <Description>this is a description</Description>          <Color>e2f4fb</Color>          <DisplayOrder>1</DisplayOrder>          <MinValue>0</MinValue>          <MaxValue>1000</MaxValue>          <DefaultValue>0</DefaultValue>          <DecimalCount>0</DecimalCount>          <UnitName />        </DataItem>        <DataItem xsi:type=\"Text\">          <Id>2</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Text field</Label>          <Description>this is a description bla</Description>          <Color>e2f4fb</Color>          <DisplayOrder>8</DisplayOrder>          <Value>true</Value>          <MaxLength>100</MaxLength>          <GeolocationEnabled>false</GeolocationEnabled>          <GeolocationForced>false</GeolocationForced>          <GeolocationHidden>true</GeolocationHidden>          <BarcodeEnabled>false</BarcodeEnabled>          <BarcodeType />        </DataItem>        <DataItem xsi:type=\"Comment\">          <Id>3</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Comment field</Label>          <Description>this is a description</Description>          <Color>e2f4fb</Color>          <DisplayOrder>3</DisplayOrder>          <Value>value</Value>          <Maxlength>10000</Maxlength>          <SplitScreen>false</SplitScreen>        </DataItem>        <DataItem xsi:type=\"Picture\">          <Id>4</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Picture field</Label>          <Description>this is a description</Description>          <Color>e2f4fb</Color>          <DisplayOrder>4</DisplayOrder>          <Multi>1</Multi>          <GeolocationEnabled>true</GeolocationEnabled>        </DataItem>        <DataItem xsi:type=\"Check_Box\">          <Id>5</Id>          <Mandatory>false</Mandatory>          <ReadOnly>true</ReadOnly>          <Label>Check box</Label>          <Description>this is a description</Description>          <Color>e2f4fb</Color>          <DisplayOrder>15</DisplayOrder>          <DefaultValue>true</DefaultValue>          <Selected>true</Selected>        </DataItem>        <DataItem xsi:type=\"Date\">          <Id>6</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Date field</Label>          <Description>this is a description</Description>          <Color>e2f4fb</Color>          <DisplayOrder>16</DisplayOrder>          <DefaultValue>11-10-2016 15:20:51</DefaultValue>          <MaxValue>2016-10-11T15:20:51.5733094+02:00</MaxValue>          <MinValue>2016-10-11T15:20:51.5733094+02:00</MinValue>        </DataItem>        <DataItem xsi:type=\"None\">          <Id>7</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>None field, only shows text</Label>          <Description>this is a description</Description>          <Color>e2f4fb</Color>          <DisplayOrder>7</DisplayOrder>        </DataItem>      </DataItemList>    </Element>  </ElementList>  <PushMessageTitle />  <PushMessageBody /></Main>";

                checkValueB = LoadFil("xml.txt");
                checkValueB = checkValueA.Replace("\n", String.Empty).Replace("\r", String.Empty).Replace("\t", String.Empty);

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test003_Xml_2a_XmlConverter()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA;
                string checkValueB;

                //Act
                checkValueA = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Main xmlns:>  <Id>1</Id>  <Label>A container check list</Label>  <DisplayOrder>1</DisplayOrder>  <CheckListFolderName>Main element</CheckListFolderName>  <Repeated>1</Repeated>  <StartDate>2016-10-11 00:00:00</StartDate>  <EndDate>2017-10-11 00:00:00</EndDate>  <Language>en</Language>  <MultiApproval>true</MultiApproval>  <FastNavigation>false</FastNavigation>  <DownloadEntities>false</DownloadEntities>  <ManualSync>true</ManualSync>  <ElementList>    <Element xsi:type=\"DataElement\">      <Id>1</Id>      <Label>Basic list</Label>      <DisplayOrder>1</DisplayOrder>      <Description><![CDATA[Data element]]></Description>      <ApprovalEnabled>true</ApprovalEnabled>      <ReviewEnabled>true</ReviewEnabled>      <DoneButtonEnabled>true</DoneButtonEnabled>      <ExtraFieldsEnabled>false</ExtraFieldsEnabled>      <PinkBarText />      <DataItemGroupList />      <DataItemList>        <DataItem xsi:type=\"Number\">          <Id>1</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Number field</Label>          <Description><![CDATA[this is a description]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>1</DisplayOrder>          <MinValue>0</MinValue>          <MaxValue>1000</MaxValue>          <DefaultValue>0</DefaultValue>          <DecimalCount>0</DecimalCount>          <UnitName />        </DataItem>        <DataItem xsi:type=\"Text\">          <Id>1</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Text field</Label>          <Description><![CDATA[this is a description bla]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>8</DisplayOrder>          <Value>true</Value>          <MaxLength>100</MaxLength>          <GeolocationEnabled>false</GeolocationEnabled>          <GeolocationForced>false</GeolocationForced>          <GeolocationHidden>true</GeolocationHidden>          <BarcodeEnabled>false</BarcodeEnabled>          <BarcodeType />        </DataItem>        <DataItem xsi:type=\"Comment\">          <Id>1</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Comment field</Label>          <Description><![CDATA[this is a description]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>3</DisplayOrder>          <Value>value</Value>          <Maxlength>10000</Maxlength>          <SplitScreen>false</SplitScreen>        </DataItem>        <DataItem xsi:type=\"Picture\">          <Id>1</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Picture field</Label>          <Description><![CDATA[this is a description]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>4</DisplayOrder>          <Multi>1</Multi>          <GeolocationEnabled>true</GeolocationEnabled>        </DataItem>        <DataItem xsi:type=\"CheckBox\">          <Id>1</Id>          <Mandatory>false</Mandatory>          <ReadOnly>true</ReadOnly>          <Label>Check box</Label>          <Description><![CDATA[this is a description]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>15</DisplayOrder>          <DefaultValue>true</DefaultValue>          <Selected>true</Selected>        </DataItem>        <DataItem xsi:type=\"Date\">          <Id>1</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>Date field</Label>          <Description><![CDATA[this is a description]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>16</DisplayOrder>          <DefaultValue>11-10-2016 15:20:51</DefaultValue>          <MaxValue>2016-10-11</MaxValue>          <MinValue>2016-10-11</MinValue>        </DataItem>        <DataItem xsi:type=\"None\">          <Id>1</Id>          <Mandatory>false</Mandatory>          <ReadOnly>false</ReadOnly>          <Label>None field, only shows text</Label>          <Description><![CDATA[this is a description]]></Description>          <Color>e2f4fb</Color>          <DisplayOrder>7</DisplayOrder>        </DataItem>      </DataItemList>    </Element>  </ElementList>  <PushMessageTitle />  <PushMessageBody /></Main>";

                checkValueB = LoadFil("xml.txt");
                MainElement main = core.TemplateFromXml(checkValueB);
                checkValueB = main.ClassToXml();
                checkValueB = t.ReplaceAtLocation(checkValueB, "<Main xmlns:", ">", "", true);
                checkValueB = checkValueB.Replace("\n", String.Empty).Replace("\r", String.Empty).Replace("\t", String.Empty);

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test003_Xml_3a_TemplateCreate()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                int checkValueA;
                int checkValueB;
                MainElement main;
                string xmlStr;

                //Act
                checkValueA = -1;

                checkValueB = -1;
                xmlStr = LoadFil("xml.txt");
                main = core.TemplateFromXml(xmlStr);
                checkValueB = core.TemplateCreate(main);

                //Assert
                TestTeardown();
                Assert.NotEqual(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test003_Xml_4a_TemplateRead()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA;
                string checkValueB;
                MainElement main;
                int templatId;
                string xmlStr;

                //Act
                xmlStr = LoadFil("xml.txt");
                main = core.TemplateFromXml(xmlStr);
                checkValueA = main.ClassToXml();
                checkValueA = ClearXml(checkValueA);

                templatId = core.TemplateCreate(main);
                main = core.TemplateRead(templatId);
                checkValueB = main.ClassToXml();
                checkValueB = ClearXml(checkValueB);

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }
        #endregion

        #region - test 004x - communicator
        [Fact]
        public void Test004_Communicator_1a_PostXml()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                bool checkValueA = true;
                bool checkValueB;
                string xmlStr;
                MainElement main = new MainElement();

                //Act
                xmlStr = LoadFil("xml.txt");
                main = core.TemplateFromXml(xmlStr);
                string responseStr = communicator.PostXml(xmlStr, siteId1);
                checkValueB = responseStr.Contains("<Response><Value type=\"success\">");

                if (checkValueB)
                {
                    string mUId = t.Locate(responseStr, "<Value type=\"success\">", "</");
                    WaitForAvailableMicroting(mUId);
                }

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test004_Communicator_2a_CheckStatus()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                bool checkValueA = true;
                bool checkValueB = false;
                string xmlStr;
                MainElement main = new MainElement();

                //Act
                xmlStr = LoadFil("xml.txt");
                main = core.TemplateFromXml(xmlStr);
                string responseStr = communicator.PostXml(xmlStr, siteId1);

                if (responseStr.Contains("<Response><Value type=\"success\">"))
                {
                    string mUId = t.Locate(responseStr, "<Value type=\"success\">", "</");
                    responseStr = communicator.CheckStatus(mUId, siteId1);

                    if (responseStr.Contains("<Response><Value type=\"success\">") ||
                        responseStr.Contains("<Response><Value type=\"received\">"))
                        checkValueB = WaitForAvailableMicroting(mUId);
                }

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test004_Communicator_3a_Retrieve()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                bool checkValueA = true;
                bool checkValueB = false;
                string xmlStr;
                MainElement main = new MainElement();

                //Act
                xmlStr = LoadFil("xml.txt");
                main = core.TemplateFromXml(xmlStr);
                string responseStr = communicator.PostXml(xmlStr, siteId1);

                if (responseStr.Contains("<Response><Value type=\"success\">"))
                {
                    string mUId = t.Locate(responseStr, "<Value type=\"success\">", "</");
                    checkValueB = WaitForAvailableMicroting(mUId);
                }

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test004_Communicator_3b_RetrieveFromId()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                bool checkValueA = true;
                bool checkValueB = false;
                string xmlStr;
                MainElement main = new MainElement();

                //Act
                xmlStr = LoadFil("xml.txt");
                main = core.TemplateFromXml(xmlStr);
                string responseStr = communicator.PostXml(xmlStr, siteId1);
                string mUId = "";

                if (responseStr.Contains("<Response><Value type=\"success\">"))
                {
                    mUId = t.Locate(responseStr, "<Value type=\"success\">", "</");
                    responseStr = communicator.RetrieveFromId(mUId, siteId1, "");

                    if (responseStr.Contains("<Response><Value type="))
                    {
                        checkValueB = true;
                        WaitForAvailableMicroting(mUId);
                    }
                }

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test004_Communicator_4a_Delete()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                bool checkValueA = true;
                bool checkValueB = false;
                string xmlStr;
                MainElement main = new MainElement();

                //Act
                xmlStr = LoadFil("xml.txt");
                main = core.TemplateFromXml(xmlStr);
                string responseStr = communicator.PostXml(xmlStr, siteId1);

                if (responseStr.Contains("<Response><Value type=\"success\">"))
                {
                    string mUId = t.Locate(responseStr, "<Value type=\"success\">", "</");
                    WaitForAvailableMicroting(mUId);

                    if (responseStr.Contains("<Response><Value type=\"success\">"))
                    {
                        checkValueB = true;
                    }
                }

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }
        #endregion

        #region - test 005x - request (XML)
        [Fact]
        public void Test005_Request_1a_ClassToXml()
        {
            lock (_lockTest)
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


                DataElement e1 = new DataElement(21, "Advanced list", 1, "Data element", true, true, true, false, "", null, new List<DataItem>());
                g1.ElementList.Add(e1);

                DataElement e2 = new DataElement(22, "Advanced list", 2, "Data element", true, true, true, false, "", null, new List<DataItem>());
                g1.ElementList.Add(e2);

                DataElement e3 = new DataElement(23, "Advanced list", 3, "Data element", true, true, true, false, "", null, new List<DataItem>());
                g1.ElementList.Add(e3);


                List<KeyValuePair> singleKeyValuePairList = new List<KeyValuePair>();
                singleKeyValuePairList.Add(new KeyValuePair("1", "option 1", true, "1"));
                singleKeyValuePairList.Add(new KeyValuePair("2", "option 2", false, "2"));
                singleKeyValuePairList.Add(new KeyValuePair("3", "option 3", false, "3"));

                List<KeyValuePair> multiKeyValuePairList = new List<KeyValuePair>();
                multiKeyValuePairList.Add(new KeyValuePair("1", "option 1", true, "1"));
                multiKeyValuePairList.Add(new KeyValuePair("2", "option 2", true, "2"));
                multiKeyValuePairList.Add(new KeyValuePair("3", "option 3", false, "3"));

                e1.DataItemList.Add(new SingleSelect(1, false, false, "Single select field", "this is a description", "e2f4fb", 1, false, singleKeyValuePairList));
                e1.DataItemList.Add(new MultiSelect(2, false, false, "Multi select field", "this is a description", "e2f4fb", 2, false, multiKeyValuePairList));
                e1.DataItemList.Add(new Audio(3, false, false, "Audio field", "this is a description", "e2f4fb", 3, false, 1));
                e1.DataItemList.Add(new Comment(5, false, false, "Comment field", "this is a description", "e2f4fb", 5, false, "value", 10000, false));

                e2.DataItemList.Add(new Number(1, false, false, "Number field", "this is a description", "e2f4fb", 1, false, 0, 1000, 2, 0, ""));
                e2.DataItemList.Add(new Text(2, false, false, "Text field", "this is a description bla", "e2f4fb", 2, false, "true", 100, false, false, true, false, ""));
                e2.DataItemList.Add(new Comment(3, false, false, "Comment field", "this is a description", "e2f4fb", 3, false, "value", 10000, false));
                e2.DataItemList.Add(new Picture(4, false, false, "Picture field", "this is a description", "e2f4fb", 4, false, 1, true));
                e2.DataItemList.Add(new CheckBox(5, false, false, "Check box", "this is a description", "e2f4fb", 5, false, true, true));
                e2.DataItemList.Add(new Date(6, false, false, "Date field", "this is a description", "e2f4fb", 6, false, startDate, startDate, startDate.ToString()));
                e2.DataItemList.Add(new None(7, false, false, "None field, only shows text", "this is a description", "e2f4fb", 7, false));
                e2.DataItemList.Add(new eFormData.Timer(8, false, false, "Timer", "this is a description", "e2f4fb", 8, false, false));
                e2.DataItemList.Add(new Signature(9, false, false, "Signature", "this is a description", "e2f4fb", 9, false));

                e3.DataItemList.Add(new CheckBox(1, true, false, "You are sure?", "Verify please", "e2f4fb", 1, false, false, false));
                #endregion

                //Act
                string xml = main.ClassToXml();
                checkValueB = ClearXml(xml);
                checkValueA = t.ReplaceAtLocation(checkValueA, "<Main xmlns:", ">", "", true);
                checkValueB = t.ReplaceAtLocation(checkValueB, "<Main xmlns:", ">", "", true);

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test005_Request_2a_XmlToClass()
        {
            lock (_lockTest)
            {
                //Arrange
                string checkValueA = ClearXml(LoadFil("requestXmlFromXml.txt"));
                string checkValueB = LoadFil("requestXmlFromClass.txt");
                MainElement main = new MainElement();

                //Act
                main = main.XmlToClass(checkValueB);
                checkValueB = ClearXml(main.ClassToXml());
                checkValueA = t.ReplaceAtLocation(checkValueA, "<Main xmlns:", ">", "", true);
                checkValueB = t.ReplaceAtLocation(checkValueB, "<Main xmlns:", ">", "", true);

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }
        #endregion

        #region - test 006x - response (XML)
        [Fact]
        public void Test006_Response_1a_XmlToClassSimple()
        {
            lock (_lockTest)
            {
                //Arrange
                string responseStr = LoadFil("responseXmlFromXml.txt");
                string checkValueA1 = "903390";
                string checkValueB1 = "";
                Response.ResponseTypes checkValueA2 = Response.ResponseTypes.Success;
                Response.ResponseTypes checkValueB2;
                Response resp = new Response();

                //Act
                resp = resp.XmlToClass(responseStr);
                checkValueB1 = resp.Value;
                checkValueB2 = resp.Type;


                //Assert
                TestTeardown();
                Assert.Equal(checkValueA1, checkValueB1);
                Assert.Equal(checkValueA2, checkValueB2);
            }
        }

        [Fact]
        public void Test006_Response_2a_XmlToClassExt()
        {
            lock (_lockTest)
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

                //Act
                resp = resp.XmlToClass(responseStr);
                checkValueB1 = resp.Value;
                checkValueB2 = resp.Type;
                checkValueB3 = resp.Checks[0].UnitId;
                checkValueB4 = resp.Checks[0].ElementList[0].Status;
                checkValueB5 = resp.Checks[0].ElementList[0].DataItemList[0].Value.InderValue;
                checkValueB6 = resp.Checks[0].ElementList[0].DataItemList[5].Value.InderValue;

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA1, checkValueB1);
                Assert.Equal(checkValueA2, checkValueB2);
                Assert.Equal(checkValueA3, checkValueB3);
                Assert.Equal(checkValueA4, checkValueB4);
                Assert.Equal(checkValueA5, checkValueB5);
                Assert.Equal(checkValueA6, checkValueB6);
            }
        }
        #endregion

        #region - test 007x - sqlController (Templat and Case)
        [Fact]
        public void Test007_SqlController_1a_TemplateCreateAndRead()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = ClearXml(LoadFil("requestXmlFromXml.txt"));
                string checkValueB = LoadFil("requestXmlFromClass.txt");
                int templatId = -1;
                MainElement main = new MainElement();

                //Act
                main = main.XmlToClass(checkValueB);
                templatId = sqlController.TemplateCreate(main);
                main = sqlController.TemplateRead(templatId);
                checkValueB = ClearXml(main.ClassToXml());
                checkValueA = t.ReplaceAtLocation(checkValueA, "<Main xmlns:", ">", "", true);
                checkValueB = t.ReplaceAtLocation(checkValueB, "<Main xmlns:", ">", "", true);

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test007_SqlController_2a_CaseCreateAndRead()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValue1A = "created";
                string checkValue1B = "";
                int checkValue2A = 66;
                int checkValue2B = 0;
                int templatId = -1;
                MainElement main = new MainElement();

                //Act
                main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
                templatId = sqlController.TemplateCreate(main);
                main = sqlController.TemplateRead(templatId);

                List<int> siteIds = new List<int>();
                siteIds.Add(siteId1);

                sqlController.CaseCreate(templatId, siteId1, "696969", "0", "testCase", "", DateTime.Now);

                cases aCase = sqlController.CaseReadFull("696969", "0");
                checkValue1B = aCase.workflow_state;
                checkValue2B = (int)aCase.status;

                //Assert
                TestTeardown();
                Assert.Equal(checkValue1A, checkValue1B);
                Assert.Equal(checkValue2A, checkValue2B);
            }
        }

        [Fact]
        public void Test007_SqlController_3a_CaseDelete()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValue1A = "removed";
                string checkValue1B = "";
                int checkValue2A = 66;
                int checkValue2B = 0;
                int templatId = -1;
                MainElement main = new MainElement();

                //Act
                main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
                templatId = sqlController.TemplateCreate(main);
                main = sqlController.TemplateRead(templatId);

                List<int> siteIds = new List<int>();
                siteIds.Add(siteId1);

                sqlController.CaseCreate(templatId, siteId1, "696969", "1", "testCase", "", DateTime.Now);

                sqlController.CaseDelete("696969");
                cases aCase = sqlController.CaseReadFull("696969", "1");
                checkValue1B = aCase.workflow_state;
                checkValue2B = (int)aCase.status;

                //Assert
                TestTeardown();
                Assert.Equal(checkValue1A, checkValue1B);
                Assert.Equal(checkValue2A, checkValue2B);
            }
        }

        [Fact]
        public void Test007_SqlController_4a_CaseUpdate()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValue1A = "created";
                string checkValue1B = "";
                int checkValue2A = 100;
                int checkValue2B = 0;
                int templatId = -1;
                MainElement main = new MainElement();

                //Act
                main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
                templatId = sqlController.TemplateCreate(main);
                main = sqlController.TemplateRead(templatId);

                List<int> siteIds = new List<int>();
                siteIds.Add(siteId1);

                sqlController.CaseCreate(templatId, siteId1, "696969", null, "testCase", "", DateTime.Now);
                sqlController.CaseUpdateRetrived("696969");
                sqlController.CaseUpdateCompleted("696969", "2", DateTime.Now, workerMUId, unitMUId);

                cases aCase = sqlController.CaseReadFull("696969", "2");
                checkValue1B = aCase.workflow_state;
                checkValue2B = (int)aCase.status;

                //Assert
                TestTeardown();
                Assert.Equal(checkValue1A, checkValue1B);
                Assert.Equal(checkValue2A, checkValue2B);
            }
        }

        [Fact]
        public void Test007_SqlController_5a_CaseRetract()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValue1A = "retracted";
                string checkValue1B = "";
                int checkValue2A = 66;
                int checkValue2B = 0;
                int templatId = -1;
                MainElement main = new MainElement();

                //Act
                main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
                templatId = sqlController.TemplateCreate(main);
                main = sqlController.TemplateRead(templatId);

                List<int> siteIds = new List<int>();
                siteIds.Add(siteId1);

                sqlController.CaseCreate(templatId, siteId1, "696969", "3", "testCase", "", DateTime.Now);
                sqlController.CaseRetract("696969", "3");
                cases aCase = sqlController.CaseReadFull("696969", "3");
                checkValue1B = aCase.workflow_state;
                checkValue2B = (int)aCase.status;

                //Assert
                TestTeardown();
                Assert.Equal(checkValue1A, checkValue1B);
                Assert.Equal(checkValue2A, checkValue2B);
            }
        }
        #endregion

        #region - test 008x - subscriber
        [Fact]
        public void Test008_Subscriber_1a_StartAndClose()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "TrueFalse";
                string checkValueB = "";
                Subscriber subS = new Subscriber(sqlController, core.log, new FakeBus());
                core.Close();

                //Act
                subS.Start();
                checkValueB = subS.IsActive().ToString();
                subS.Close();
                checkValueB += subS.IsActive().ToString();

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test008_Subscriber_2a_LacksName()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "not_found = True";
                string checkValueB = "";

                //Act
                sqlController.NotificationCreate(DateTime.Now.ToLongTimeString(), "not in db", "check_status");
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(100);
                    var lst = sqlController.UnitTest_FindAllNotifications();
                    if (lst[0].workflow_state == "not_found")
                    {
                        checkValueB = "not_found = True";
                        break;
                    }
                }

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }
        #endregion

        #region - test 009x - core (Case)
        [Fact]
        public void Test009_Core_1a_CaseCreate()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                int checkValueA = 1;
                int checkValueB;
                int templatId = -1;
                MainElement main = new MainElement();

                //Act
                main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
                templatId = core.TemplateCreate(main);
                main = core.TemplateRead(templatId);

                core.CaseCreate(main, "", siteId1);
                List<string> mUIds = WaitForAvailableDB();
                checkValueB = mUIds.Count;

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test009_Core_2a_CaseDelete()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                bool checkValueA = true;
                bool checkValueB;
                int templatId = -1;
                MainElement main = new MainElement();

                //Act
                main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
                templatId = core.TemplateCreate(main);
                main = core.TemplateRead(templatId);

                string temp = core.CaseCreate(main, "", siteId1);
                List<string> mUIds = WaitForAvailableDB();

                WaitForAvailableMicroting(mUIds[0]);
                checkValueB = core.CaseDelete(mUIds[0]);

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test009_Core_3a_CaseCreateReversed()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "1True";
                string checkValueB;
                int templatId = -1;
                MainElement main = new MainElement();

                //Act
                main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
                templatId = core.TemplateCreate(main);
                main = core.TemplateRead(templatId);

                List<int> lstSiteIds = new List<int>();
                lstSiteIds.Add(siteId1);

                core.CaseCreate(main, "", lstSiteIds, "custom");
                List<string> mUIds = WaitForAvailableDB();

                checkValueB = mUIds.Count.ToString();
                checkValueB += WaitForAvailableMicroting(mUIds[0]).ToString();

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test009_Core_4a_CaseLookup()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                Case_Dto checkValueA = new Case_Dto();
                Case_Dto checkValueB;
                int templatId = -1;
                MainElement main = new MainElement();

                //Act
                main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
                templatId = core.TemplateCreate(main);
                main = core.TemplateRead(templatId);

                core.CaseCreate(main, "", siteId1);
                List<string> mUIds = WaitForAvailableDB();

                WaitForAvailableMicroting(mUIds[0]);
                checkValueB = core.CaseLookupMUId(mUIds[0]);

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA.GetType(), checkValueB.GetType());
            }
        }

        [Fact]
        public void Test009_Core_5a_Close()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                bool checkValueA = true;
                bool checkValueB = false;

                //Act
                checkValueB = core.Close();

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }
        #endregion

        #region - test 010x - entity
        [Fact]
        public void Test010_Entity_1a_EntityGroupCreate_EntitySearch()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "";
                EntityGroup checkValueB;

                //Act
                checkValueB = core.EntityGroupCreate("EntitySearch", "MyTest");

                //Assert
                TestTeardown();
                Assert.NotEqual(checkValueA, checkValueB.ToString());
            }
        }

        [Fact]
        public void Test010_Entity_1b_EntityGroupCreate_EntitySelect()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "";
                EntityGroup checkValueB;

                //Act
                checkValueB = core.EntityGroupCreate("EntitySelect", "MyTest");

                //Assert
                TestTeardown();
                Assert.NotEqual(checkValueA, checkValueB.ToString());
            }
        }

        [Fact]
        public void Test010_Entity_2a_EntityGroupUpdate_NotUnique()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "Failed";
                string checkValueB;


                //Act
                EntityGroup peG = core.EntityGroupCreate("EntitySearch", "MyTest");

                List<EntityItem> lst = new List<EntityItem>();
                EntityGroup eG = new EntityGroup(peG.Id, "Group", "EntitySearch", peG.EntityGroupMUId, lst, "created", DateTime.Now, DateTime.Now);
                lst.Add(new EntityItem("Item1", "description", "1", "created"));
                lst.Add(new EntityItem("Item2", "description", "4", "created"));
                lst.Add(new EntityItem("Item3", "description", "3", "created"));
                lst.Add(new EntityItem("Item4", "description", "4", "created"));

                try
                {
                    core.EntityGroupUpdate(eG);
                    checkValueB = "Passed, which it should not";
                }
                catch
                {
                    checkValueB = "Failed";
                }

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test010_Entity_3a_EntityGroupUpdateAndRead_EntitySearch()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "Item2New5Item6";
                string checkValueB;
                EntityGroup oEG;

                //Act
                oEG = core.EntityGroupCreate("EntitySearch", "MyTest");
                List<EntityItem> lst = new List<EntityItem>();
                EntityGroup eG = new EntityGroup(oEG.Id, "Group", "EntitySearch", oEG.EntityGroupMUId, lst, "created", DateTime.Now, DateTime.Now);
                lst.Add(new EntityItem("Item1", "description", "1", "created"));
                lst.Add(new EntityItem("Item2", "description", "2", "created"));
                lst.Add(new EntityItem("Item3", "description", "3", "created"));
                lst.Add(new EntityItem("Item4", "description", "4", "created"));
                core.EntityGroupUpdate(eG);

                var tempi = core.EntityGroupRead(oEG.EntityGroupMUId);
                checkValueB = tempi.EntityGroupItemLst[1].Name;

                tempi.EntityGroupItemLst[2].Name = "New";
                tempi.EntityGroupItemLst.Add(new EntityItem("Item5", "added", "5", "created"));
                tempi.EntityGroupItemLst.RemoveAt(3);
                tempi.EntityGroupItemLst.Add(new EntityItem("Item6", "added", "6", "created"));
                core.EntityGroupUpdate(tempi);

                var tempii = core.EntityGroupRead(oEG.EntityGroupMUId);
                checkValueB += tempi.EntityGroupItemLst[2].Name;
                checkValueB += tempi.EntityGroupItemLst.Count;
                checkValueB += tempi.EntityGroupItemLst[4].Name;

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test010_Entity_3b_EntityGroupUpdateAndRead_EntitySelect()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "Item2New5Item6True";
                string checkValueB;
                EntityGroup oEG;

                //Act
                oEG = core.EntityGroupCreate("EntitySelect", "MyTest");
                List<EntityItem> lst = new List<EntityItem>();
                EntityGroup eG = new EntityGroup(oEG.Id, "Group", "EntitySelect", oEG.EntityGroupMUId, lst, "created", DateTime.Now, DateTime.Now);
                lst.Add(new EntityItem("Item1", "description & more", "1", "created"));
                lst.Add(new EntityItem("Item2", "description", "2", "created"));
                lst.Add(new EntityItem("Item3 & more", "description", "3", "created"));
                lst.Add(new EntityItem("Item4", "description", "4", "created"));
                core.EntityGroupUpdate(eG);

                var tempi = core.EntityGroupRead(oEG.EntityGroupMUId);
                checkValueB = tempi.EntityGroupItemLst[1].Name;

                tempi.EntityGroupItemLst[2].Name = "New";
                tempi.EntityGroupItemLst.Add(new EntityItem("Item5", "added", "5", "created"));
                tempi.EntityGroupItemLst.RemoveAt(3);
                tempi.EntityGroupItemLst.Add(new EntityItem("Item6", "added", "6", "created"));
                core.EntityGroupUpdate(tempi);
                Thread.Sleep(250);

                var tempii = core.EntityGroupRead(oEG.EntityGroupMUId);
                checkValueB += tempi.EntityGroupItemLst[2].Name;
                checkValueB += tempi.EntityGroupItemLst.Count;
                checkValueB += tempi.EntityGroupItemLst[4].Name;
                checkValueB += sqlController.UnitTest_EntitiesAllSynced(oEG.EntityGroupMUId);

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test010_Entity_4a_EntityGroupItemRemoveAndAddAgain_EntitySearch()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "1st insert2nd insertTrue";
                string checkValueB;
                EntityGroup oEG;

                //Act
                oEG = core.EntityGroupCreate("EntitySearch", "MyTest");
                List<EntityItem> lst = new List<EntityItem>();
                EntityGroup eG = new EntityGroup(oEG.Id, "Group", "EntitySearch", oEG.EntityGroupMUId, lst, "created", DateTime.Now, DateTime.Now);
                lst.Add(new EntityItem("Item1", "1st insert", "3", "created"));
                lst.Add(new EntityItem("Item2", "other", "4", "created"));

                core.EntityGroupUpdate(eG);

                var tempi = core.EntityGroupRead(oEG.EntityGroupMUId);
                checkValueB = tempi.EntityGroupItemLst[0].Description;
                tempi.EntityGroupItemLst.RemoveAt(0);

                core.EntityGroupUpdate(tempi);

                var tempii = core.EntityGroupRead(oEG.EntityGroupMUId);
                tempii.EntityGroupItemLst.Add(new EntityItem("Item1", "2nd insert", "3", "created"));
                checkValueB += tempii.EntityGroupItemLst[1].Description;

                core.EntityGroupUpdate(tempii);

                var tempiii = core.EntityGroupRead(oEG.EntityGroupMUId);
                checkValueB += sqlController.UnitTest_EntitiesAllSynced(oEG.EntityGroupMUId);

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test010_Entity_5a_EntityGroupDelete_EntitySearch()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "4isZero";
                string checkValueB;
                EntityGroup oEG;

                //Act
                oEG = core.EntityGroupCreate("EntitySearch", "MyTest");
                List<EntityItem> lst = new List<EntityItem>();
                EntityGroup eG = new EntityGroup(oEG.Id, "Group", "EntitySearch", oEG.EntityGroupMUId, lst, "created", DateTime.Now, DateTime.Now);
                lst.Add(new EntityItem("Item1", "description", "1", "created"));
                lst.Add(new EntityItem("Item2", "description", "2", "created"));
                lst.Add(new EntityItem("Item3", "description", "3", "created"));
                lst.Add(new EntityItem("Item4", "description", "4", "created"));
                core.EntityGroupUpdate(eG);

                var tempi = core.EntityGroupRead(oEG.EntityGroupMUId);
                checkValueB = tempi.EntityGroupItemLst.Count.ToString();

                core.EntityGroupDelete(oEG.EntityGroupMUId);
                var tempii = core.EntityGroupRead(oEG.EntityGroupMUId);

                if (tempii.EntityGroupItemLst.Count == 0)
                    checkValueB += "isZero";

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test010_Entity_5b_EntityGroupDelete_EntitySelect()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "4isZero";
                string checkValueB;
                EntityGroup oEG;

                //Act
                oEG = core.EntityGroupCreate("EntitySelect", "MyTest");
                List<EntityItem> lst = new List<EntityItem>();
                EntityGroup eG = new EntityGroup(oEG.Id, "Group", "EntitySelect", oEG.EntityGroupMUId, lst, "created", DateTime.Now, DateTime.Now);
                lst.Add(new EntityItem("Item1", "description", "1", "created"));
                lst.Add(new EntityItem("Item2", "description", "2", "created"));
                lst.Add(new EntityItem("Item3", "description", "3", "created"));
                lst.Add(new EntityItem("Item4", "description", "4", "created"));
                core.EntityGroupUpdate(eG);

                var tempi = core.EntityGroupRead(oEG.EntityGroupMUId);
                checkValueB = tempi.EntityGroupItemLst.Count.ToString();

                core.EntityGroupDelete(oEG.EntityGroupMUId);
                var tempii = core.EntityGroupRead(oEG.EntityGroupMUId);

                if (tempii.EntityGroupItemLst.Count == 0)
                    checkValueB += "isZero";

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }
        #endregion

        #region - test 011x - case
        [Fact]
        public void Test011_Case_1a_Retrived()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "Retrived";
                string checkValueB;
                int templatId = -1;
                string mUId;
                MainElement main = new MainElement();

                //Act
                main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
                templatId = core.TemplateCreate(main);
                main = core.TemplateRead(templatId);
                core.CaseCreate(main, "", siteId1);
                List<string> mUIds = WaitForAvailableDB();

                mUId = mUIds[0];
                sqlController.NotificationCreate("42", mUId, "unit_fetch");

                while (sqlController.UnitTest_FindAllActiveNotifications().Count > 0)
                    Thread.Sleep(100);

                Case_Dto caseDto = core.CaseLookupMUId(mUId);
                checkValueB = caseDto.Stat;

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test011_Case_2a_Completed()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "Completed";
                string checkValueB;
                string mUId;

                //Act
                mUId = CaseCreate();
                CaseComplet(mUId, "1", false);

                Case_Dto caseDto = core.CaseLookupMUId(mUId);
                checkValueB = caseDto.Stat;

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test011_Case_2b_Reversed_Completed()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "Completed";
                string checkValueB;
                string mUId;

                //Act
                mUId = CaseCreateReversed();
                CaseComplet(mUId, "1", true);

                Case_Dto caseDto = core.CaseLookupMUId(mUId);
                checkValueB = caseDto.Stat;

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test011_Case_3a_Many_Completed()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "Completed4";
                string checkValueB = "Completed";
                Case_Dto caseDto;
                string mUId1; string mUId2; string mUId3; string mUId4;
                int count = 0;

                //Act
                mUId1 = CaseCreate();
                mUId2 = CaseCreate();
                mUId3 = CaseCreate();
                CaseComplet(mUId3, "1003", false);
                mUId4 = CaseCreate();
                CaseComplet(mUId4, "1004", false);
                CaseComplet(mUId2, "1002", false);
                CaseComplet(mUId1, "1001", false);

                caseDto = core.CaseLookupMUId(mUId1);
                if (caseDto.Stat == "Completed")
                    count++;

                caseDto = core.CaseLookupMUId(mUId2);
                if (caseDto.Stat == "Completed")
                    count++;

                caseDto = core.CaseLookupMUId(mUId3);
                if (caseDto.Stat == "Completed")
                    count++;

                caseDto = core.CaseLookupMUId(mUId4);
                if (caseDto.Stat == "Completed")
                    count++;

                checkValueB += count;

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }

        [Fact]
        public void Test011_Case_4a_ReversedMany_Completed()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), true);
                string checkValueA = "Completed7";
                string checkValueB = "Completed";
                string mUId1;
                string mUId2;
                int count = 0;

                //Act
                mUId1 = CaseCreateReversed();
                mUId2 = CaseCreateReversed();
                CaseComplet(mUId2, "201", true);
                CaseComplet(mUId1, "101", true);
                CaseComplet(mUId1, "102", true);
                CaseComplet(mUId1, "103", true);
                CaseComplet(mUId2, "202", true);
                CaseComplet(mUId2, "203", true);
                CaseComplet(mUId1, "104", true);

                if (core.CaseIdLookup(mUId1, "101") != null) count++;
                if (core.CaseIdLookup(mUId1, "102") != null) count++;
                if (core.CaseIdLookup(mUId1, "103") != null) count++;
                if (core.CaseIdLookup(mUId1, "104") != null) count++;
                if (core.CaseIdLookup(mUId2, "201") != null) count++;
                if (core.CaseIdLookup(mUId2, "202") != null) count++;
                if (core.CaseIdLookup(mUId2, "203") != null) count++;

                checkValueB += count;

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }
        #endregion

        //#region - test 012x - interaction cases
        //[Fact]
        //public void Test012_Interaction_Case_1a_CreatedInTable()
        //{
        //    lock (_lockTest)
        //    {
        //        //Arrange
        //        TestPrepare(t.GetMethodName(), true);
        //        string checkValueA = "Passed";
        //        string checkValueB;

        //        string xmlStr = LoadFil("xml.txt");
        //        MainElement main = core.TemplateFromXml(xmlStr);
        //        int templatId = core.TemplateCreate(main);
        //        List<int> siteUIds = new List<int>();
        //        siteUIds.Add(siteId1);
        //        siteUIds.Add(siteId2);

        //        //Act
        //        checkValueB = "" + core.Advanced_InteractionCaseCreate(templatId, "", siteUIds, "", false, null);
        //        if (checkValueB == "1" || checkValueB == "2" || checkValueB == "3")
        //            checkValueB = "Passed";

        //        //Assert
        //        TestTeardown();
        //        Assert.Equal(checkValueA, checkValueB);
        //    }
        //}

        //[Fact]
        //public void Test012_Interaction_Case_2a_Completed_Connected()
        //{
        //    lock (_lockTest)
        //    {
        //        //Arrange
        //        TestPrepare(t.GetMethodName(), true);
        //        string checkValueA = "CompletedCompleted";
        //        string checkValueB = "";

        //        string xmlStr = LoadFil("xml.txt");
        //        MainElement main = core.TemplateFromXml(xmlStr);
        //        int templatId = core.TemplateCreate(main);
        //        List<int> siteUIds = new List<int>();
        //        siteUIds.Add(siteId1);
        //        siteUIds.Add(siteId2);

        //        //Act
        //        int iCaseId = (int)core.Advanced_InteractionCaseCreate(templatId, "", siteUIds, "", true, null);

        //        WaitForAvailableMicroting(iCaseId);

        //        InteractionCaseComplet(iCaseId);

        //        var lst = sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId);

        //        foreach (var item in lst)
        //            checkValueB += item.stat;

        //        //Assert
        //        TestTeardown();
        //        Assert.Equal(checkValueA, checkValueB);
        //    }
        //}

        //[Fact]
        //public void Test012_Interaction_Case_2b_Completed_NotConnected()
        //{
        //    lock (_lockTest)
        //    {
        //        //Arrange
        //        TestPrepare(t.GetMethodName(), true);
        //        string checkValueA = "CompletedCompleted";
        //        string checkValueB = "";

        //        string xmlStr = LoadFil("xml.txt");
        //        MainElement main = core.TemplateFromXml(xmlStr);
        //        int templatId = core.TemplateCreate(main);
        //        List<int> siteUIds = new List<int>();
        //        siteUIds.Add(siteId1);
        //        siteUIds.Add(siteId2);

        //        //Act
        //        int iCaseId = (int)core.Advanced_InteractionCaseCreate(templatId, t.GetMethodName(), siteUIds, "", false, null);

        //        WaitForAvailableMicroting(iCaseId);
        //        InteractionCaseComplet(iCaseId);

        //        var lst = sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId);
        //        foreach (var item in lst)
        //            checkValueB += item.stat;

        //        //Assert
        //        TestTeardown();
        //        Assert.Equal(checkValueA, checkValueB);
        //    }
        //}

        //[Fact]
        //public void Test012_Interaction_Case_2c_CompletedWithReplacements()
        //{
        //    lock (_lockTest)
        //    {
        //        //Arrange
        //        TestPrepare(t.GetMethodName(), true);
        //        string checkValueA = "CompletedCompleted";
        //        string checkValueB = "";

        //        string xmlStr = LoadFil("xml.txt");
        //        MainElement main = core.TemplateFromXml(xmlStr);
        //        int templatId = core.TemplateCreate(main);
        //        List<int> siteUIds = new List<int>();
        //        siteUIds.Add(siteId1);
        //        siteUIds.Add(siteId2);

        //        //Act
        //        List<string> temp = new List<string>();
        //        temp.Add("pre_text1==post_text1");
        //        temp.Add("pre_text2==post_text2");
        //        temp.Add("Title::Test");
        //        temp.Add("Info::info TEXT added to eForm");
        //        temp.Add("Expire::" + DateTime.Now.AddDays(10).ToString());
        //        int iCaseId = (int)core.Advanced_InteractionCaseCreate(templatId, "", siteUIds, "", false, temp);

        //        WaitForAvailableMicroting(iCaseId);
        //        InteractionCaseComplet(iCaseId);

        //        var lst = sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId);
        //        foreach (var item in lst)
        //            checkValueB += item.stat;

        //        //Assert
        //        TestTeardown();
        //        Assert.Equal(checkValueA, checkValueB);
        //    }
        //}

        //[Fact]
        //public void Test012_Interaction_Case_2d_ReplacementsFailedDateTime()
        //{
        //    lock (_lockTest)
        //    {
        //        //Arrange
        //        TestPrepare(t.GetMethodName(), true);
        //        string checkValueA = "failed to sync";
        //        string checkValueB = "";

        //        string xmlStr = LoadFil("xml.txt");
        //        MainElement main = core.TemplateFromXml(xmlStr);
        //        int templatId = core.TemplateCreate(main);
        //        List<int> siteUIds = new List<int>();
        //        siteUIds.Add(siteId1);
        //        siteUIds.Add(siteId2);

        //        //Act
        //        List<string> temp = new List<string>();
        //        temp.Add("Expire::" + "TEXT THAT IS NOT A DATETIME");
        //        int iCaseId = (int)core.Advanced_InteractionCaseCreate(templatId, t.GetMethodName(), siteUIds, "", false, temp);

        //        WaitForAvailableMicroting(iCaseId);
        //        var lst = sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId);

        //        var cas = sqlController.UnitTest_FindInteractionCase(iCaseId);
        //        checkValueB = cas.workflow_state;

        //        //Assert
        //        TestTeardown();
        //        Assert.Equal(checkValueA, checkValueB);
        //    }
        //}

        //[Fact]
        //public void Test012_Interaction_Case_3a_DeletedSDK()
        //{
        //    lock (_lockTest)
        //    {
        //        //Arrange
        //        TestPrepare(t.GetMethodName(), true);
        //        string checkValueA = "DeletedDeleted";
        //        string checkValueB = "";

        //        string xmlStr = LoadFil("xml.txt");
        //        MainElement main = core.TemplateFromXml(xmlStr);
        //        int templatId = core.TemplateCreate(main);
        //        List<int> siteUIds = new List<int>();
        //        siteUIds.Add(siteId1);
        //        siteUIds.Add(siteId2);


        //        //Act
        //        int iCaseId = (int)core.Advanced_InteractionCaseCreate(templatId, t.GetMethodName(), siteUIds, "", false, null);

        //        WaitForAvailableMicroting(iCaseId);
        //        var lst = sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId);

        //        core_UT.CaseDelete(lst[1].microting_uid);
        //        sqlController.NotificationCreate(DateTime.Now.ToLongTimeString(), lst[0].microting_uid, "unit_fetch");

        //        while (sqlController.UnitTest_FindAllActiveNotifications().Count > 0)
        //            Thread.Sleep(100);

        //        core_UT.CaseDelete(lst[0].microting_uid);
        //        var lst2 = sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId);

        //        foreach (var item in lst2)
        //            checkValueB += item.stat;

        //        //Assert
        //        TestTeardown();
        //        Assert.Equal(checkValueA, checkValueB);
        //    }
        //}

        //[Fact]
        //public void Test012_Interaction_Case_3b_DeletedInteraction()
        //{
        //    lock (_lockTest)
        //    {
        //        //Arrange
        //        TestPrepare(t.GetMethodName(), true);
        //        string checkValueA = "DeletedDeleted";
        //        string checkValueB = "";

        //        string xmlStr = LoadFil("xml.txt");
        //        MainElement main = core.TemplateFromXml(xmlStr);
        //        int templatId = core.TemplateCreate(main);
        //        List<int> siteUIds = new List<int>();
        //        siteUIds.Add(siteId1);
        //        siteUIds.Add(siteId2);

        //        //Act
        //        int iCaseId = (int)core.Advanced_InteractionCaseCreate(templatId, t.GetMethodName(), siteUIds, "", false, null);

        //        WaitForAvailableMicroting(iCaseId);
        //        var lst = sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId);

        //        sqlController.NotificationCreate(DateTime.Now.ToLongTimeString(), lst[0].microting_uid, "unit_fetch");
        //        while (sqlController.UnitTest_FindAllActiveNotifications().Count > 0)
        //            Thread.Sleep(100);

        //        core.Advanced_InteractionCaseDelete(iCaseId);
        //        while (sqlController.UnitTest_FindAllActiveInteraction().Count > 0)
        //            Thread.Sleep(100);

        //        var lst2 = sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId);
        //        foreach (var item in lst2)
        //            checkValueB += item.stat;

        //        //Assert
        //        TestTeardown();
        //        Assert.Equal(checkValueA, checkValueB);
        //    }
        //}

        //[Fact]
        //public void Test012_Interaction_Case_4a__Multi_Completed()
        //{
        //    lock (_lockTest)
        //    {
        //        //Arrange
        //        TestPrepare(t.GetMethodName(), true);
        //        string checkValueA = "CompletedCompletedCompletedCompletedCompletedCompleted";
        //        string checkValueB = "";

        //        string xmlStr = LoadFil("xml.txt");
        //        MainElement main = core.TemplateFromXml(xmlStr);
        //        int templatId = core.TemplateCreate(main);
        //        List<int> siteUIds = new List<int>();
        //        siteUIds.Add(siteId1);
        //        siteUIds.Add(siteId2);

        //        //Act
        //        int iCaseId1 = (int)core.Advanced_InteractionCaseCreate(templatId, t.GetMethodName() + " case1", siteUIds, "", false, null);
        //        int iCaseId2 = (int)core.Advanced_InteractionCaseCreate(templatId, t.GetMethodName() + " case2", siteUIds, "", false, null);
        //        int iCaseId3 = (int)core.Advanced_InteractionCaseCreate(templatId, t.GetMethodName() + " case3", siteUIds, "", false, null);

        //        WaitForAvailableMicroting(iCaseId1);
        //        WaitForAvailableMicroting(iCaseId2);
        //        WaitForAvailableMicroting(iCaseId3);

        //        InteractionCaseComplet(iCaseId2);
        //        InteractionCaseComplet(iCaseId3);
        //        InteractionCaseComplet(iCaseId1);

        //        var lst = sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId1);
        //        lst.AddRange(sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId2));
        //        lst.AddRange(sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId3));

        //        foreach (var item in lst)
        //            checkValueB += item.stat;

        //        //Assert
        //        TestTeardown();
        //        Assert.Equal(checkValueA, checkValueB);
        //    }
        //}

        //[Fact]
        //public void Test012_Interaction_Case_5a_Multi_Completed_StressTest()
        //{
        //    lock (_lockTest)
        //    {
        //        //Arrange
        //        TestPrepare(t.GetMethodName(), true);
        //        string checkValueA = "Passed, if no expection";
        //        string checkValueB = "Passed, if no expection";

        //        string xmlStr = LoadFil("xml.txt");
        //        MainElement main = core.TemplateFromXml(xmlStr);
        //        int templatId = core.TemplateCreate(main);
        //        List<int> siteUIds = new List<int>();
        //        siteUIds.Add(siteId1);
        //        siteUIds.Add(siteId2);

        //        List<string> temp = new List<string>();
        //        temp.Add("pre_text1==post_text1");
        //        temp.Add("pre_text2==post_text2");
        //        temp.Add("Title::Test");
        //        temp.Add("Info::info TEXT added to eForm");
        //        temp.Add("Expire::" + DateTime.Now.AddDays(10).ToString());

        //        //Act
        //        int iCaseId1 = (int)core.Advanced_InteractionCaseCreate(templatId, "", siteUIds, "", false, temp);
        //        int iCaseId2 = (int)core.Advanced_InteractionCaseCreate(templatId, "", siteUIds, "", false, temp);
        //        int iCaseId3 = (int)core.Advanced_InteractionCaseCreate(templatId, "", siteUIds, "", true, temp);
        //        int iCaseId4 = (int)core.Advanced_InteractionCaseCreate(templatId, "", siteUIds, "", false, temp);
        //        int iCaseId5 = (int)core.Advanced_InteractionCaseCreate(templatId, "", siteUIds, "", false, temp);

        //        WaitForAvailableMicroting(iCaseId1);
        //        WaitForAvailableMicroting(iCaseId2);
        //        WaitForAvailableMicroting(iCaseId3);
        //        WaitForAvailableMicroting(iCaseId4);
        //        WaitForAvailableMicroting(iCaseId5);

        //        //InteractionCaseComplet(iCaseId2); on purpose missing
        //        InteractionCaseComplet(iCaseId4);
        //        InteractionCaseComplet(iCaseId3);
        //        InteractionCaseComplet(iCaseId5);
        //        InteractionCaseComplet(iCaseId1);

        //        var lstCompleted = sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId1);
        //        lstCompleted.AddRange(sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId3));
        //        lstCompleted.AddRange(sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId4));
        //        lstCompleted.AddRange(sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId5));

        //        var lstNot = sqlController.UnitTest_FindAllActiveInteractionCaseLists(iCaseId2);

        //        foreach (var item in lstCompleted)
        //        {
        //            if (item.stat != "Completed")
        //                throw new Exception("InteractionCase not 'Completed'");
        //        }

        //        foreach (var item in lstNot)
        //        {
        //            if (item.stat != "Sent")
        //                throw new Exception("InteractionCase not 'Completed'");
        //        }

        //        //Assert
        //        Assert.Equal(checkValueA, checkValueB);
        //    }
        //}
        //#endregion

        #region - test 013x - sqlController (Settings)
        //Not active, as would fuck up the stat of settings. Don't run unless settings stat is not improtant
        //[Fact]
        //public void Test013_SqlController_1a_SettingCreateDefaults()
        //{
        //    lock (_lockTest)
        //    {
        //        //Arrange
        //        TestPrepare(t.GetMethodName(), false);
        //        bool checkValueA = true;
        //        bool checkValueB = false;

        //        //Act
        //        checkValueB =  sqlController.SettingCreateDefaults();

        //        //Assert
        //        TestTeardown();
        //        Assert.Equal(checkValueA, checkValueB);
        //    }
        //}

        //Not active, as would fuck up the stat of settings
        //[Fact]
        //public void Test013_SqlController_2a_SettingCreate()
        //{
        //    lock (_lockTest)
        //    {
        //        //Arrange
        //        TestPrepare(t.GetMethodName(), false);
        //        bool checkValueA1 = true;
        //        bool checkValueA2 = true;
        //        bool checkValueB1 = false;
        //        bool checkValueB2 = false;

        //        //Act
        //        checkValueB1 = sqlController.SettingCreate(Settings.firstRunDone);
        //        checkValueB2 = sqlController.SettingCreate(Settings.logLevel);

        //        //Assert
        //        TestTeardown();
        //        Assert.Equal(checkValueA1, checkValueB1);
        //        Assert.Equal(checkValueA2, checkValueB2);
        //    }
        //}

        [Fact]
        public void Test013_SqlController_3a_SettingRead()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), false);
                string checkValueA1 = "true";
                string checkValueA2 = "4";
                string checkValueB1 = "";
                string checkValueB2 = "";

                //Act
                sqlController.SettingCreate(Settings.firstRunDone);
                sqlController.SettingCreate(Settings.logLevel);

                checkValueB1 = sqlController.SettingRead(Settings.firstRunDone);
                checkValueB2 = sqlController.SettingRead(Settings.logLevel);

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA1, checkValueB1);
                Assert.Equal(checkValueA2, checkValueB2);
            }
        }

        //Not active, as would fuck up the stat of settings. Don't run unless settings stat is not improtant
        //[Fact]
        //public void Test013_SqlController_4a_SettingUpdate()
        //{
        //    lock (_lockTest)
        //    {
        //        //Arrange
        //        TestPrepare(t.GetMethodName(), false);
        //        string checkValueA = "tempValuefinalValue";
        //        string checkValueB1 = "";
        //        string checkValueB2 = "";

        //        //Act
        //        sqlController.SettingCreate(Settings.firstRunDone);
        //        sqlController.SettingCreate(Settings.logLevel);

        //        sqlController.SettingUpdate(Settings.firstRunDone, "tempValue");
        //        sqlController.SettingUpdate(Settings.logLevel, "tempValue");

        //        checkValueB1 = sqlController.SettingRead(Settings.firstRunDone);
        //        checkValueB2 = sqlController.SettingRead(Settings.logLevel);

        //        sqlController.SettingUpdate(Settings.firstRunDone, "finalValue");
        //        sqlController.SettingUpdate(Settings.logLevel, "finalValue");

        //        checkValueB1 += sqlController.SettingRead(Settings.firstRunDone);
        //        checkValueB2 += sqlController.SettingRead(Settings.logLevel);

        //        //Assert
        //        TestTeardown();
        //        Assert.Equal(checkValueA, checkValueB1);
        //        Assert.Equal(checkValueA, checkValueB2);
        //    }
        //}

        [Fact]
        public void Test013_SqlController_5a_SettingCheckAll()
        {
            lock (_lockTest)
            {
                //Arrange
                TestPrepare(t.GetMethodName(), false);
                int checkValueA = 0;
                int checkValueB = -1;

                //Act
                sqlController.SettingCreateDefaults();
                var temp = sqlController.SettingCheckAll();
                checkValueB = temp.Count();

                //Assert
                TestTeardown();
                Assert.Equal(checkValueA, checkValueB);
            }
        }
        #endregion

        #region private
        private List<string> WaitForAvailableDB()
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    List<string> lstMUId = sqlController.UnitTest_FindAllActiveCases();

                    if (lstMUId.Count == 1)
                    {
                        return lstMUId;
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                throw new Exception("WaitForAvailableDB failed. Due to failed 100 attempts");
            }
            catch (Exception ex)
            {
                throw new Exception("WaitForAvailableDB failed", ex);
            }
        }

        private bool WaitForAvailableMicroting(string microtingUId)
        {
            if (useLiveData)
            {
                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        string responseStr = communicator.CheckStatus(microtingUId, siteId1);

                        if (responseStr.Contains("<Response><Value type=\"success\">"))
                        {
                            responseStr = communicator.Delete(microtingUId, siteId1);
                            if (responseStr.Contains("<Response><Value type=\"success\">"))
                                return true;
                            else
                                return false;
                        }
                        else
                        {
                            Thread.Sleep(300);
                        }
                    }
                    throw new Exception("WaitForAvailableMicroting failed. Due to failed 25 attempts");
                }
                catch (Exception ex)
                {
                    throw new Exception("WaitForAvailableMicroting failed", ex);
                }
            }
            return true;
        }

        private bool WaitForAvailableMicroting(int interactionCaseId)
        {
            try
            {
                string lastReply = "";

                for (int i = 0; i < 125; i++)
                {
                    var lst = sqlController.UnitTest_FindAllActiveInteractionCaseLists(interactionCaseId);
                    var cas = sqlController.UnitTest_FindInteractionCase(interactionCaseId);

                    if (cas.workflow_state == "failed to sync")
                        return true;

                    int missingCount = 0;

                    foreach (var item in lst)
                    {
                        if (string.IsNullOrEmpty(item.microting_uid))
                            missingCount++;
                    }

                    if (missingCount == 0)
                    {
                        lastReply = "";

                        foreach (var item in lst)
                        {
                            string reply = core.CaseCheck(item.microting_uid);

                            if (!reply.Contains("success"))
                                missingCount++;

                            lastReply += reply + " // ";
                        }

                        if (missingCount == 0)
                            return true;
                    }

                    Thread.Sleep(250 + 12 * i);
                }
                core.log.LogCritical("Not Specified", "TraceMsg:'" + lastReply.Trim() + "'");
                throw new Exception("WaitForAvailableMicroting failed. Due to failed 125 attempts (1+ min)");
            }
            catch (Exception ex)
            {
                throw new Exception("WaitForAvailableMicroting failed", ex);
            }
        }

        private string ClearXml(string inputXmlString)
        {
            inputXmlString = t.ReplaceAtLocationAll(inputXmlString, "<StartDate>", "</StartDate>", "xxx", true);
            inputXmlString = t.ReplaceAtLocationAll(inputXmlString, "<EndDate>", "</EndDate>", "xxx", true);
            inputXmlString = t.ReplaceAtLocationAll(inputXmlString, "<Language>", "</Language>", "xxx", true);
            inputXmlString = t.ReplaceAtLocationAll(inputXmlString, "<Id>", "</Id>", "xxx", true);
            inputXmlString = t.ReplaceAtLocationAll(inputXmlString, "<DefaultValue>", "</DefaultValue>", "xxx", true);
            inputXmlString = t.ReplaceAtLocationAll(inputXmlString, "<MaxValue>", "</MaxValue>", "xxx", true);
            inputXmlString = t.ReplaceAtLocationAll(inputXmlString, "<MinValue>", "</MinValue>", "xxx", true);

            return inputXmlString;
        }

        private string CaseCreate()
        {
            MainElement main = new MainElement();
            main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
            int templatId = core.TemplateCreate(main);
            main = core.TemplateRead(templatId);

            return core.CaseCreate(main, "", siteId1);
        }

        private string CaseCreateReversed()
        {
            List<int> siteLst = new List<int> { siteId1 };
            MainElement main = new MainElement();
            main = main.XmlToClass(LoadFil("requestXmlFromClass.txt"));
            main.Repeated = 0;
            int templatId = core.TemplateCreate(main);
            main = core.TemplateRead(templatId);

            return core.CaseCreate(main, "", siteLst, "")[0];
        }

        private void CaseComplet(string microtingUId, string checkUId, bool reversed)
        {
            WaitForAvailableMicroting(microtingUId);
            var dto = core.CaseLookupMUId(microtingUId);

            if (dto.Stat == "created")
            {
                sqlController.NotificationCreate(DateTime.Now.ToLongTimeString(), microtingUId, "unit_fetch");
                while (sqlController.UnitTest_FindAllActiveNotifications().Count > 0)
                    Thread.Sleep(100);
            }

            if (reversed)
                sqlController.CaseCreate(2, dto.SiteUId, microtingUId, checkUId, "ReversedCase", "", DateTime.Now);

            sqlController.NotificationCreate(DateTime.Now.ToLongTimeString(), microtingUId, "check_status");
            while (sqlController.UnitTest_FindAllActiveNotifications().Count > 0)
                Thread.Sleep(100);

            //As there is no data to these notifications, the case completion, retraction and events are triggered fake
            core_UT.CaseComplet(microtingUId, checkUId, workerMUId, unitMUId);

            //Clean up, in the instance of a case that has been created for real, but has been faked to be completed/retracted
            try
            {
                communicator.Delete(dto.MicrotingUId, dto.SiteUId);
            }
            catch
            {

            }
        }

        private void InteractionCaseComplet(int interactionCaseId)
        {
            var lst = sqlController.UnitTest_FindAllActiveInteractionCaseLists(interactionCaseId);

            foreach (var item in lst)
            {
                CaseComplet(item.microting_uid, "1", false);
            }
        }

        private string LoadFil(string path)
        {
            try
            {
                lock (_lockFil)
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

        private string PrintLogLine()
        {
            string str = "";
            str += sqlController.UnitTest_FindLog(1000, "Exception as per request");
            str += ":";
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:sameExceptionCountTried / Content:1");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:sameExceptionCountTried / Content:2");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:sameExceptionCountTried / Content:3");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:sameExceptionCountTried / Content:4");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:sameExceptionCountTried / Content:5");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:sameExceptionCountTried / Content:6");
            str += "/";
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:sameExceptionCountMax / Content:1");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:sameExceptionCountMax / Content:2");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:sameExceptionCountMax / Content:3");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:sameExceptionCountMax / Content:4");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:sameExceptionCountMax / Content:5");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:sameExceptionCountMax / Content:6");
            str += "/";
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:secondsDelay / Content:1");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:secondsDelay / Content:8");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:secondsDelay / Content:64");
            str += sqlController.UnitTest_FindLog(1000, "Variable Name:secondsDelay / Content:512");
            str += sqlController.UnitTest_FindLog(1000, "FatalExpection called for reason:'Restartfailed. Core failed to restart'");
            str += "/";
            str += sqlController.UnitTest_FindLog(1000, "Core triggered Exception event");
            str += Environment.NewLine;
            return str;
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

        public void EventCaseCompleted(object sender, EventArgs args)
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

        public void EventNotificationNotFound(object sender, EventArgs args)
        {

        }

        public void EventException(object sender, EventArgs args)
        {
            lock (_lockFil)
            {
                sqlController.WriteLogEntry(new LogEntry(-4, "FATAL ERROR", "Core triggered Exception event"));
            }

            throw (Exception)sender; //Core need to be able that the external code crashed
        }
        #endregion
    }

    #region dummy class
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<TestContext>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
    #endregion
}