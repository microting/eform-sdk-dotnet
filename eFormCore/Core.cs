/*
The MIT License (MIT)

Copyright (c) 2014 microting

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

using eFormCommunicator;
using eFormOffice;
using eFormRequest;
using eFormResponse;
using eFormShared;
using eFormSubscriber;
using eFormSqlController;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;
using System.Security.Cryptography;
using System.Linq;
using System.Globalization;

namespace eFormCore
{
    public class Core : ICore
    {
        #region events
        public event EventHandler HandleCaseCreated;
        public event EventHandler HandleCaseRetrived;
        public event EventHandler HandleCaseCompleted;
        public event EventHandler HandleCaseDeleted;
        public event EventHandler HandleFileDownloaded;
        public event EventHandler HandleSiteActivated;

        public event EventHandler HandleEventLog;
        public event EventHandler HandleEventMessage;
        public event EventHandler HandleEventWarning;
        public event EventHandler HandleEventException;
        #endregion

        #region var
        Communicator communicator;
        SqlController sqlController;
        Subscriber subscriber;
        ExcelController excelController;
        Tools t = new Tools();

        object _lockMain = new object();
        object _lockEventClient = new object();
        object _lockEventServer = new object();
        object _lockEventMessage = new object();
        bool updateIsRunningFiles = false;
        bool updateIsRunningNotifications = false;
        bool updateIsRunningEntities = false;
        bool coreRunning = false;
        bool coreRestarting = false;
        bool coreStatChanging = false;
        List<ExceptionClass> exceptionLst = new List<ExceptionClass>();

        string comToken;
        string comAddress;
        string comAddressBasic;
        string organizationId;
        string subscriberToken;
        string subscriberAddress;
        string subscriberName;

        string serverConnectionString;
        string fileLocation;
        bool logLevel = false;
        #endregion

        #region con
        public Core()
        {

        }
        #endregion

        #region public state
        public bool     Start(string serverConnectionString)
        {
            try
            {
                if (!coreRunning && !coreStatChanging)
                {
                    coreStatChanging = true;

                    if (string.IsNullOrEmpty(serverConnectionString))
                        throw new ArgumentException("serverConnectionString is not allowed to be null or empty");

                    //sqlController
                    sqlController = new SqlController(serverConnectionString);
                    TriggerLog("SqlEformController started");


                    #region settings read and checked
                    comToken = sqlController.SettingRead("comToken");
                    comAddress = sqlController.SettingRead("comAddress");
                    comAddressBasic = sqlController.SettingRead("comAddressBasic");
                    organizationId = sqlController.SettingRead("organizationId");
                    subscriberToken = sqlController.SettingRead("subscriberToken");
                    subscriberAddress = sqlController.SettingRead("subscriberAddress");
                    subscriberName = sqlController.SettingRead("subscriberName");
                    fileLocation = sqlController.SettingRead("fileLocation");
                    logLevel = bool.Parse(sqlController.SettingRead("logLevel"));
                    this.serverConnectionString = serverConnectionString;
                    TriggerLog("Settings read");


                    if (string.IsNullOrEmpty(comToken)) throw new ArgumentException("comToken is not allowed to be null or empty");
                    if (string.IsNullOrEmpty(comAddress)) throw new ArgumentException("comAddress is not allowed to be null or empty");
                    if (string.IsNullOrEmpty(organizationId)) throw new ArgumentException("organizationId is not allowed to be null or empty");
                    if (string.IsNullOrEmpty(subscriberToken)) throw new ArgumentException("subscriberToken is not allowed to be null or empty");
                    if (string.IsNullOrEmpty(subscriberAddress)) throw new ArgumentException("subscriberAddress is not allowed to be null or empty");
                    if (string.IsNullOrEmpty(subscriberName)) throw new ArgumentException("subscriberName is not allowed to be null or empty");
                    if (subscriberName.Contains(" ") || subscriberName.Contains("æ") || subscriberName.Contains("ø") || subscriberName.Contains("å")) throw new ArgumentException("subscriberName is not allowed to contain blank spaces ' ', æ, ø and å");
                    if (string.IsNullOrEmpty(fileLocation)) throw new ArgumentException("fileLocation is not allowed to be null or empty");
                    TriggerLog("Settings checked");
                    #endregion


                    #region core.Start()
                    TriggerLog("");
                    TriggerLog("");
                    TriggerLog("###################################################################################################################");
                    TriggerMessage("Core.Start() at:" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                    TriggerLog("###################################################################################################################");
                    TriggerLog("comToken:" + comToken + " comAddress: " + comAddress + " subscriberToken:" + subscriberToken + " subscriberAddress:" + subscriberAddress +
                        " subscriberName:" + subscriberName + " serverConnectionString:" + serverConnectionString + " fileLocation:" + fileLocation + " logEvents:" + logLevel.ToString());
                    TriggerLog("Core started");
                    #endregion


                    //communicators
                    communicator = new Communicator(comAddress, comToken, organizationId, comAddressBasic);
                    communicator.EventLog += CoreHandleEventLog;
                    TriggerLog("Communicator started");


                    //subscriber
                    subscriber = new Subscriber(subscriberToken, subscriberAddress, subscriberName);
                    TriggerLog("Subscriber created");
                    subscriber.EventMsgClient += CoreHandleEventClient;
                    subscriber.EventMsgServer += CoreHandleEventServer;
                    TriggerLog("Subscriber now triggers events");
                    subscriber.Start();


                    //communicators
                    excelController = new ExcelController();
                    TriggerLog("Excel (Office) started");


                    #region known sites
                    if (!bool.Parse(sqlController.SettingRead("knownSitesDone")))
                    {
                        sqlController.UnitTest_CleanAllSitesTabels();

                        foreach (var item in communicator.SiteLoadAllFromRemote())
                        {
                            SiteName_Dto siteDto = sqlController.SiteRead(item.SiteUId);
                            if (siteDto == null)
                            {
                                sqlController.SiteCreate(item.SiteUId, item.SiteName);
                            }
                        }

                        foreach (var item in communicator.WorkerLoadAllFromRemote())
                        {
                            Worker_Dto workerDto = sqlController.WorkerRead(item.WorkerUId);
                            if (workerDto == null)
                            {
                                sqlController.WorkerCreate(item.WorkerUId, item.FirstName, item.LastName, item.Email);
                            }
                        }

                        foreach (var item in communicator.SiteWorkerLoadAllFromRemote())
                        {
                            Site_Worker_Dto siteWorkerDto = sqlController.SiteWorkerRead(item.MicrotingUId);
                            if (siteWorkerDto == null)
                            {
                                sqlController.SiteWorkerCreate(item.MicrotingUId, item.SiteUId, item.WorkerUId);
                            }
                        }

                        int customerNo = communicator.OrganizationLoadAllFromRemote();
                        foreach (var item in communicator.UnitLoadAllFromRemote(customerNo))
                        {
                            Unit_Dto unitDto = sqlController.UnitRead(item.UnitUId);
                            if (unitDto == null)
                            {
                                sqlController.UnitCreate(item.UnitUId, item.CustomerNo, item.OtpCode, item.SiteUId);
                            }
                        }

                        TriggerLog("Known sites added to Database");
                        sqlController.SettingUpdate("knownSitesDone", "true");
                    }
                    #endregion


                    coreRunning = true;
                    coreStatChanging = false;
                }
            }
            catch (Exception ex)
            {
                coreRunning = false;
                coreStatChanging = false;
                throw new Exception("FATAL Exception. Core failed to start", ex);
            }
            return true;
        }

        public bool     StartSqlOnly(string serverConnectionString)
        {
            if (string.IsNullOrEmpty(serverConnectionString))
                throw new ArgumentException("serverConnectionString is not allowed to be null or empty");

            this.serverConnectionString = serverConnectionString;
       
            try
            {
                if (!coreRunning && !coreStatChanging)
                {
                    coreStatChanging = true;

                    TriggerLog("");
                    TriggerLog("");
                    TriggerLog("###################################################################################################################");
                    TriggerMessage("Core.Start() at:" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                    TriggerLog("###################################################################################################################");
                    TriggerLog("serverConnectionString:" + serverConnectionString + " logEvents:" + logLevel.ToString());
                    TriggerLog("Controller started");

                    //sqlController
                    sqlController = new SqlController(serverConnectionString);
                    TriggerLog("SqlEformController started");


                    comToken = sqlController.SettingRead("comToken");
                    comAddress = sqlController.SettingRead("comAddress");
                    comAddressBasic = sqlController.SettingRead("comAddressBasic");
                    organizationId = sqlController.SettingRead("organizationId");

                    //communicators
                    communicator = new Communicator(comAddress, comToken, organizationId, comAddressBasic);
                    communicator.EventLog += CoreHandleEventLog;
                    TriggerLog("Communicator started");

                    coreRunning = true;
                    coreStatChanging = false;
                }
            }
            catch (Exception ex)
            {
                coreRunning = false;
                coreStatChanging = false;
                comToken = sqlController.SettingRead("comToken");
                comAddress = sqlController.SettingRead("comAddress");
                organizationId = sqlController.SettingRead("organizationId"); throw new Exception("FATAL Exception. Core failed to start", ex);
            }
            return true;
        }

        public bool     Close()
        {
            try
            {
                if (coreRunning && !coreStatChanging)
                {
                    lock (_lockMain) //Will let sending Cases sending finish, before closing
                    {
                        coreStatChanging = true;

                        coreRunning = false;
                        TriggerMessage("Core.Close() at:" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());

                        try
                        {
                            TriggerMessage("Subscriber requested to close connection");
                            subscriber.Close();
                        }
                        catch { }

                        #region remove triggers
                        try
                        {
                            communicator.EventLog += CoreHandleEventLog;
                        }
                        catch { }

                        try
                        {
                            subscriber.EventMsgClient -= CoreHandleEventClient;
                            subscriber.EventMsgServer -= CoreHandleEventServer;
                        }
                        catch { }
                        #endregion

                        subscriber = null;
                        communicator = null;
                        sqlController = null;

                        TriggerLog("Subscriber no longer triggers events");
                        TriggerLog("Controller closed");
                        TriggerLog("");

                        coreStatChanging = false;
                    }
                }
            }
            catch (Exception ex)
            {
                coreRunning = false;
                coreStatChanging = false;
                throw new Exception("FATAL Exception. Core failed to close", ex);
            }
            return true;
        }

        public bool     Running()
        {
            return coreRunning;
        }
        #endregion

        #region public actions
        #region templat
        public MainElement      TemplatFromXml(string xmlString)
        {
            try
            {
                TriggerLog("XML to transform:");
                TriggerLog(xmlString);

                //XML HACK TODO
                #region xmlString = corrected xml if needed
                xmlString = xmlString.Replace("<Main>", "<Main xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                xmlString = xmlString.Replace("<Element type=", "<Element xsi:type=");
                xmlString = xmlString.Replace("<DataItem type=", "<DataItem xsi:type=");
                xmlString = xmlString.Replace("<DataItemGroup type=", "<DataItemGroup xsi:type=");

                xmlString = xmlString.Replace("<FolderName>", "<CheckListFolderName>");
                xmlString = xmlString.Replace("</FolderName>", "</CheckListFolderName>");

                string temp = t.Locate(xmlString, "<DoneButtonDisabled>", "</DoneButtonDisabled>");
                if (temp == "false")
                {
                    xmlString = xmlString.Replace("DoneButtonDisabled", "DoneButtonEnabled");
                    xmlString = xmlString.Replace("<DoneButtonEnabled>false", "<DoneButtonEnabled>true");
                }
                if (temp == "true")
                {
                    xmlString = xmlString.Replace("DoneButtonDisabled", "DoneButtonEnabled");
                    xmlString = xmlString.Replace("<DoneButtonEnabled>true", "<DoneButtonEnabled>false");
                }

                xmlString = xmlString.Replace("<MinValue/>", "<MinValue>" + long.MinValue + "</MinValue>");
                xmlString = xmlString.Replace("<MaxValue/>", "<MaxValue>" + long.MaxValue + "</MaxValue>");
                xmlString = xmlString.Replace("<DecimalCount/>", "<DecimalCount>" + "0" + "</DecimalCount>");
                #endregion

                MainElement mainElement = new MainElement();
                mainElement = mainElement.XmlToClass(xmlString);

                TriggerLog("XML after possible corrections:");
                TriggerLog(xmlString);

                //XML HACK
                mainElement.CaseType = "";
                mainElement.PushMessageTitle = "";
                mainElement.PushMessageBody = "";
                if (mainElement.Repeated < 1)
                {
                    TriggerMessage("mainElement.Repeated = 1 // enforced");
                    mainElement.Repeated = 1;
                }

                return mainElement;
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("TemplatFromXml failed", ex, false);
                return null;
            }
        }

        public int              TemplatCreate(MainElement mainElement)
        {
            try
            {
                if (mainElement == null)
                    throw new ArgumentNullException("mainElement not allowed to be null");

            }
            catch (Exception ex)
            {
                TriggerHandleExpection("TemplatCreate input failed", ex, false);
            }

            try
            {
                if (coreRunning)
                {
                    int templatId = sqlController.TemplatCreate(mainElement);
                    TriggerLog("Templat id:" + templatId.ToString() + " created in DB");
                    return templatId;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("TemplatCreate failed", ex, true);
                throw new Exception("TemplatCreate failed", ex);
            }
        }

        public MainElement      TemplatRead(int templatId)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("Templat id:" + templatId.ToString() + " trying to be read");
                    MainElement mainElement = new MainElement(sqlController.TemplatRead(templatId));
                    return mainElement;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("TemplatRead failed", ex, true);
                throw new Exception("TemplatRead failed", ex);
            }
        }

        public List<MainElement>        TemplatReadAll()
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("TemplatReadAll() called");
                    return sqlController.TemplatReadAll();
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("TemplatReadAll failed", ex, true);
                throw new Exception("TemplatReadAll failed", ex);
            }
        }

        public bool                     TemplateDelete(int templatId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("templatId:" + templatId);

                    return sqlController.TemplateDelete(templatId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region case
        public string           CaseCreate(MainElement mainElement, string caseUId, int siteId)
        {
            List<string> lst = CaseCreate(mainElement, caseUId, new List<int> { siteId }, "", false);
            return lst[0];
        }

        public List<string>     CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, string custom, bool reversed)
        {
            try
            {
                if (coreRunning)
                {
                    lock (_lockMain) //Will let sending Cases sending finish, before closing
                    {
                        string siteIdsStr = string.Join(",", siteIds);
                        TriggerLog("caseUId:" + caseUId + " siteIds:" + siteIdsStr + " custom:" + custom + " reversed:" + reversed.ToString() + ", requested to be created");

                        #region check input
                        DateTime start = DateTime.Parse(mainElement.StartDate);
                        start = DateTime.Parse(start.ToShortTimeString());

                        DateTime end = DateTime.Parse(mainElement.EndDate);
                        start = DateTime.Parse(start.ToShortTimeString());

                        if (end < DateTime.Now)
                            throw new ArgumentException("mainElement.EndDate needs to be a future date");

                        if (end <= start)
                            throw new ArgumentException("mainElement.StartDate needs to be at least the day, before the remove date (mainElement.EndDate)");

                        if (reversed == false && mainElement.Repeated != 1)
                            throw new ArgumentException("if reversed == false, mainElement.Repeat has to be 1");

                        if (reversed == true && caseUId != "")
                            throw new ArgumentException("if reversed == true, caseUId can't be used and has to be left blank");

                        if (reversed == true && caseUId != "")
                            throw new ArgumentException("if reversed == true, caseUId can't be used and has to be left blank");
                        #endregion

                        //sending and getting a reply
                        List<string> lstMUId = new List<string>();

                        foreach (int siteId in siteIds)
                        {
                            string mUId = SendXml(mainElement, siteId);

                            if (reversed == false)
                                sqlController.CaseCreate(mainElement.Id, siteId, mUId, caseUId, custom);
                            else
                                sqlController.CheckListSitesCreate(mainElement.Id, siteId, mUId);


                            Case_Dto cDto = sqlController.CaseReadByMUId(mUId);
                            HandleCaseCreated(cDto, EventArgs.Empty);
                            TriggerMessage(cDto.ToString() + " has been created");

                            lstMUId.Add(mUId);
                        }

                        return lstMUId;
                    }
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseCreate failed", ex, true);
                throw new Exception("CaseCreate failed", ex);
            }
        }

        public ReplyElement     CaseRead(string microtingUId, string checkUId)
        {
            try
            {
                if (coreRunning)
                {
                    if (checkUId == null)
                        checkUId = "";

                    TriggerLog("microtingUId:" + microtingUId + " checkUId:" + checkUId + ", requested to be read");

                    if (checkUId == "" || checkUId == "0")
                        checkUId = null;

                    cases aCase = sqlController.CaseReadFull(microtingUId, checkUId);
                    #region handling if no match case found
                    if (aCase == null)
                    {
                        HandleEventWarning("No case found with MuuId:'" + microtingUId + "'", EventArgs.Empty);
                        return null;
                    }
                    #endregion
                    int id = aCase.id;
                    TriggerLog("aCase.id:" + aCase.id.ToString() + ", found");

                    ReplyElement replyElement = new ReplyElement(sqlController.TemplatRead((int)aCase.check_list_id));
                    replyElement.Custom = aCase.custom;
                    replyElement.DoneAt = (DateTime)aCase.done_at;
                    replyElement.DoneById = (int)aCase.done_by_user_id;
                    replyElement.UnitId = (int)aCase.unit_id;

                    List<FieldValue> lstAnswers = new List<FieldValue>();
                    List<field_values> lstReplies = sqlController.ChecksRead(microtingUId, checkUId);
                    #region remove replicates from lstReplies. Ex. multiple pictures
                    List<field_values> lstRepliesTemp = new List<field_values>();
                    bool found;

                    foreach (var reply in lstReplies)
                    {
                        found = false;
                        foreach (var tempReply in lstRepliesTemp)
                        {
                            if (reply.field_id == tempReply.field_id)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                            lstRepliesTemp.Add(reply);
                    }

                    lstReplies = lstRepliesTemp;
                    #endregion

                    foreach (field_values reply in lstReplies)
                    {
                        FieldValue answer = sqlController.FieldValueRead(reply.id);
                        lstAnswers.Add(answer);
                    }
                    TriggerLog("Questons and answers found");

                    //replace DataItem(s) with DataItem(s) Answer
                    replyElement.ElementList = ReplaceDataElementsAndDataItems(aCase.id, replyElement.ElementList, lstAnswers);

                    return replyElement;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseRead failed", ex, true);
                throw new Exception("CaseRead failed", ex);
            }
        }

        public List<cases>      CaseReadAll(int templatId, DateTime? start, DateTime? end)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("Templat id:" + templatId.ToString() + " trying to read all cases");

                    return sqlController.CaseReadAllIds(templatId, start, end);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseReadAll failed", ex, true);
                throw new Exception("CaseReadAll failed", ex);
            }
        }

        public bool             CaseUpdate(int caseId, List<string> newFieldValuePairLst, List<string> newCheckListValuePairLst)
        {
            try
            {
                if (coreRunning)
                {
                    if (newFieldValuePairLst == null)
                        newFieldValuePairLst = new List<string>();

                    if (newCheckListValuePairLst == null)
                        newCheckListValuePairLst = new List<string>();

                    TriggerLog("CaseUpdate caseId:'" + caseId + "', newFieldValuePairLst.Count:'" + newFieldValuePairLst.Count + "', newCheckListValuePairLst.Count:'" + newCheckListValuePairLst.Count + "'");

                    int id = 0;
                    string value = "";

                    foreach (string str in newFieldValuePairLst)
                    {
                        id = int.Parse(t.SplitToList(str, 0, false));
                        value = t.SplitToList(str, 1, false);
                        sqlController.FieldValueUpdate(caseId, id, value);
                    }

                    foreach (string str in newCheckListValuePairLst)
                    {
                        id = int.Parse(t.SplitToList(str, 0, false));
                        value = t.SplitToList(str, 1, false);
                        sqlController.CheckListValueStatusUpdate(caseId, id, value);
                    }

                    return true;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseUpdate failed", ex, true);
                throw new Exception("CaseUpdate failed", ex);
            }
        }

        public bool             CaseDelete(string microtingUId)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("microtingUId:" + microtingUId + ", requested to be deleted");

                    var aCase = sqlController.CaseReadByMUId(microtingUId);
                    string xmlResponse = communicator.Delete(microtingUId, aCase.SiteUId);

                    TriggerLog("XML response:");
                    TriggerLog(xmlResponse);

                    Response resp = new Response();
                    resp = resp.XmlToClass(xmlResponse);
                    if (resp.Type.ToString() == "Success")
                    {
                        try
                        {
                            sqlController.CaseDelete(microtingUId);
                            return true;
                        }
                        catch { }

                        try
                        {
                            sqlController.CaseDeleteReversed(microtingUId);
                            return true;
                        }
                        catch { }

                        HandleCaseDeleted(aCase, EventArgs.Empty);
                        TriggerMessage(aCase.ToString() + " has been removed");
                    }
                    return false;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseDelete failed", ex, true);
                throw new Exception("CaseDelete failed", ex);
            }
        }

        public Case_Dto         CaseLookupMUId(string microtingUId)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("microtingUId:" + microtingUId + ", requested to be looked up");
                    return sqlController.CaseReadByMUId(microtingUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseLookupMUId failed", ex, true);
                throw new Exception("CaseLookupMUId failed", ex);
            }
        }

        public Case_Dto         CaseLookupCaseId(int caseId)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("caseId:" + caseId + ", requested to be looked up");
                    return sqlController.CaseReadByCaseId(caseId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseLookupCaseId failed", ex, true);
                throw new Exception("CaseLookupCaseId failed", ex);
            }
        }

        public List<Case_Dto>   CaseLookupCaseUId(string caseUId)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("caseUId:" + caseUId + ", requested to be looked up");
                    return sqlController.CaseReadByCaseUId(caseUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseLookupCaseUId failed", ex, true);
                throw new Exception("CaseLookupCaseUId failed", ex);
            }
        }

        public int              CaseIdLookup(string microtingUId, string checkUId)
        {
            try
            {
                if (coreRunning)
                {
                    if (checkUId == null)
                        checkUId = "";

                    TriggerLog("microtingUId:" + microtingUId + " checkUId:" + checkUId + ", requested to be read");

                    if (checkUId == "" || checkUId == "0")
                        checkUId = null;

                    cases aCase = sqlController.CaseReadFull(microtingUId, checkUId);
                    #region handling if no match case found
                    if (aCase == null)
                    {
                        HandleEventWarning("No case found with MuuId:'" + microtingUId + "'", EventArgs.Empty);
                        return -1;
                    }
                    #endregion
                    int id = aCase.id;
                    TriggerLog("aCase.id:" + aCase.id.ToString() + ", found");

                    return id;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseIdLookup failed", ex, true);
                throw new Exception("CaseIdLookup failed", ex);
            }
        }

        public string           CasesToExcel(int templatId, DateTime? start, DateTime? end, string pathAndFilName)
        {
            try
            {
                if (coreRunning)
                {
                    List<List<string>> dataSet = GenerateDataSetFromCases(templatId, start, end);

                    if (dataSet == null)
                        return "";

                    return excelController.CreateExcel(dataSet, pathAndFilName);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CasesToExcel failed", ex, true);
                throw new Exception("CasesToExcel failed", ex);
            }
        }

        public string           CasesToCsv(int templatId, DateTime? start, DateTime? end, string pathAndFilName)
        {
            try
            {
                if (coreRunning)
                {
                    List<List<string>> dataSet = GenerateDataSetFromCases(templatId, start, end);

                    if (dataSet == null)
                        return "";

                    using (TextWriter writer = File.CreateText(pathAndFilName))
                    {
                        List<string> temp;

                        for (int rowN = 0; rowN < dataSet[0].Count; rowN++)
                        {
                            temp = new List<string>();

                            foreach (List<string> lst in dataSet)
                            {
                                temp.Add(lst[rowN]); 
                            }

                            writer.WriteLine(string.Join(";", temp.ToArray()));
                        }
                    }

                    return pathAndFilName;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CasesToCsv failed", ex, true);
                throw new Exception("CasesToCsv failed", ex);
            }
        }
        #endregion

        #region sites
        public List<SiteName_Dto>   SiteGetAll()
        {
            throw new NotImplementedException();
        }

        public List<Site_Dto> SimpleSiteGetAll()
        {
            throw new NotImplementedException();
        }

        public Site_Dto  SiteCreateSimple(string name, string userFirstName, string userLastName, string userEmail)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    //TriggerLog(methodName + " called");
                    //TriggerLog("siteName:" + name + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    //string result = communicator.SiteCreate(name);

                    //var parsedData = JRaw.Parse(result);

                    //var orgResult = JRaw.Parse(communicator.OrganizationLoadAllFromRemote());

                    //int customerNo = int.Parse(orgResult.First().First()["customer_no"].ToString());

                    //string siteName = parsedData["name"].ToString();
                    //int siteId = int.Parse(parsedData["id"].ToString());
                    //int unitUId = int.Parse(parsedData["unit_id"].ToString());
                    //int otpCode = int.Parse(parsedData["otp_code"].ToString());
                    //Site_Dto siteDto = sqlController.SiteRead(siteId);
                    //if (siteDto == null)
                    //{
                    //    sqlController.SiteCreate((int)siteId, siteName);
                    //}
                    //siteDto = sqlController.SiteRead(siteId);
                    //Unit_Dto unitDto = sqlController.UnitRead(unitUId);
                    //if (unitDto == null)
                    //{
                    //    sqlController.UnitCreate(unitUId, customerNo, otpCode, siteDto.Id);
                    //}

                    //if (string.IsNullOrEmpty(userEmail))
                    //{
                    //    Random rdn = new Random();
                    //    userEmail = siteId + "." + customerNo + "@invalid.invalid";
                    //}

                    //Worker_Dto workerDto = WorkerCreate(userFirstName, userLastName, userEmail);

                    //SiteWorkerCreate(siteDto, workerDto);

                    //return SiteReadSimple(siteId);
                    return null;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public Site_Dto  SiteReadSimple(int siteId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("siteId:" + siteId);

                    return sqlController.SiteReadSimple(siteId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public SiteName_Dto         SiteCreate(string name)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("siteName:" + name);

                    string siteData = communicator.SiteCreate(name);

                    int siteUid = siteData[0];
                    int customerNumber = siteData[1];
                    int otpCode = siteData[2];
                    int unitUId = siteData[3];

                    //sqlController.SiteCreate(microting_uid, name, customerNumber, otpCode, unitUId, userId, userFirstName, userLastName, workerId);
                    sqlController.SiteCreate(siteUid, name);
                    sqlController.UnitCreate(unitUId, customerNumber, otpCode, siteUid);

                    return SiteRead(siteUid);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public SiteName_Dto         SiteRead(int siteId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("siteId:" + siteId);

                    return sqlController.SiteRead(siteId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public bool             SiteUpdate(int siteId, string name)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("siteId:" + siteId);

                    if (sqlController.SiteRead(siteId) == null)
                        return false;

                    bool success = communicator.SiteUpdate(siteId, name);
                    if (!success)
                        return false;

                    return sqlController.SiteUpdate(siteId, name);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public bool             SiteDelete(int siteId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("siteId:" + siteId);

                    bool success = communicator.SiteDelete(siteId);
                    if (!success)
                        return false;

                    return sqlController.SiteDelete(siteId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }
        
        #endregion

        #region workers
        public Worker_Dto       WorkerCreate(string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    //TriggerLog(methodName + " called");
                    //TriggerLog("firstName:" + firstName + " / lastName:" + lastName + " / email:" + email);


                    //string result = communicator.WorkerCreate(firstName, lastName, email);
                    //var parsedData = JRaw.Parse(result);
                    //int workerUid = int.Parse(parsedData["id"].ToString());

                    //Worker_Dto workerDto = sqlController.WorkerRead(workerUid);
                    //if (workerDto == null)
                    //{
                    //    sqlController.WorkerCreate(workerUid, firstName, lastName, email);
                    //}

                    //return WorkerRead(workerUid);
                    return null;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public Worker_Dto       WorkerRead(int workerId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("workerId:" + workerId);

                    return sqlController.WorkerRead(workerId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public bool             WorkerUpdate(int workerId, string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("workerId:" + workerId + " / firstName:" + firstName + " / lastName:" + lastName + " / email:" + email);

                    if (sqlController.WorkerRead(workerId) == null)
                        return false;

                    bool success = communicator.WorkerUpdate(workerId, firstName, lastName, email);
                    if (!success)
                        return false;

                    return sqlController.WorkerUpdate(workerId, firstName, lastName, email);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public bool             WorkerDelete(int workerId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("workerId:" + workerId);

                    bool success = communicator.WorkerDelete(workerId);
                    if (!success)
                        return false;

                    return sqlController.WorkerDelete(workerId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region site_workers
        public Site_Worker_Dto SiteWorkerCreate(SiteName_Dto siteDto, Worker_Dto workerDto)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    //TriggerLog(methodName + " called");
                    //TriggerLog("siteId:" + siteDto.Id + " / workerId:" + workerDto.Id);

                    //string result = communicator.SiteWorkerCreate(siteDto.MicrotingUid, workerDto.MicrotingUid);
                    //var parsedData = JRaw.Parse(result);
                    //int workerUid = int.Parse(parsedData["id"].ToString());

                    //Site_Worker_Dto siteWorkerDto = sqlController.SiteWorkerRead(workerUid);

                    //if (siteWorkerDto == null)
                    //{
                    //    sqlController.SiteWorkerCreate(workerUid, siteDto.Id, workerDto.Id);
                    //}

                    //return SiteWorkerRead(workerUid);
                    return null;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public Site_Worker_Dto SiteWorkerRead(int siteWorkerId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("siteWorkerId:" + siteWorkerId);

                    return sqlController.SiteWorkerRead(siteWorkerId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public bool            SiteWorkerUpdate(int siteWorkerId, int workerId, int siteId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("siteWorkerId:" + siteWorkerId + " / workerId:" + workerId + " / siteId:" + siteId);

                    if (sqlController.SiteWorkerRead(siteWorkerId) == null)
                        return false;

                    bool success = communicator.SiteWorkerUpdate(siteWorkerId, workerId, siteId);
                    if (!success)
                        return false;

                    return sqlController.SiteWorkerUpdate(siteWorkerId, workerId, siteId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public bool            SiteWorkerDelete(int workerId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("workerId:" + workerId);

                    bool success = communicator.SiteWorkerDelete(workerId);
                    if (!success)
                        return false;

                    return sqlController.SiteWorkerDelete(workerId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region units
        public List<Unit_Dto>  UnitGetAll()
        {
            return null;
        }

        public Unit_Dto        UnitRead(int unitId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("unitId:" + unitId);

                    return sqlController.UnitRead(unitId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }


        public Unit_Dto        UnitRequestOtp(int unitId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("unitId:" + unitId);

                    int otp_code = communicator.UnitRequestOtp(unitId);

                    Unit_Dto my_dto = UnitRead(unitId);

                    sqlController.UnitUpdate(unitId, my_dto.CustomerNo, otp_code, my_dto.SiteUId);

                    return UnitRead(unitId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }
        #endregion

        #region entity group
        public string           EntityGroupCreate(string entityType, string name)
        {
            try
            {
                if (coreRunning)
                {
                    int entityGroupId = sqlController.EntityGroupCreate(name, entityType);

                    string entityGroupMUId = communicator.EntityGroupCreate(entityType, name, entityGroupId.ToString());

                    bool isCreated = sqlController.EntityGroupUpdate(entityGroupId, entityGroupMUId);

                    if (isCreated)
                        return entityGroupMUId;
                    else
                    {
                        sqlController.EntityGroupDelete(entityGroupMUId);
                        throw new Exception("EntityListCreate failed, due to list not created correct");
                    }
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("EntityListCreate failed", ex, true);
                throw new Exception("EntityListCreate failed", ex);
            }
        }
   
        public EntityGroup      EntityGroupRead(string entityGroupMUId)
        {
            try
            {
                if (coreRunning)
                {
                    while (updateIsRunningEntities)
                        Thread.Sleep(200);

                    return sqlController.EntityGroupRead(entityGroupMUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("EntityGroupRead failed", ex, true);
                throw new Exception("EntityGroupRead failed", ex);
            }
        }

        public bool             EntityGroupUpdate(EntityGroup entityGroup)
        {
            try
            {
                if (coreRunning)
                {
                    List<string> ids = new List<string>();
                    foreach (var item in entityGroup.EntityGroupItemLst)
                        ids.Add(item.EntityItemUId);

                    if (ids.Count != ids.Distinct().Count())
                        throw new Exception("List entityGroup.entityItemUIds are not all unique"); // Duplicates exist

                    while (updateIsRunningEntities)
                        Thread.Sleep(200);

                    sqlController.EntityGroupUpdateItems(entityGroup);

                    Thread aThread = new Thread(() => CoreHandleUpdateEntityItems());
                    aThread.Start();
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("EntityGroupRead failed", ex, true);
                throw new Exception("EntityGroupRead failed", ex);
            }
            return true;
        }

        public bool             EntityGroupDelete(string entityGroupMUId)
        {
            try
            {
                if (coreRunning)
                {
                    while (updateIsRunningEntities)
                        Thread.Sleep(200);

                    string type = sqlController.EntityGroupDelete(entityGroupMUId);

                    if (type != null)
                        communicator.EntityGroupDelete(type, entityGroupMUId);

                    Thread aThread = new Thread(() => CoreHandleUpdateEntityItems());
                    aThread.Start();
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("EntityGroupDelete failed", ex, true);
                throw new Exception("EntityGroupDelete failed", ex);
            }
            return true;
        }
        #endregion
        #endregion

        #region private
        private List<Element>   ReplaceDataElementsAndDataItems(int caseId, List<Element> elementList, List<FieldValue> lstAnswers)
        {
            List<Element> elementListReplaced = new List<Element>();

            foreach (Element element in elementList)
            {
                #region if DataElement
                if (element.GetType() == typeof(DataElement))
                {
                    DataElement dataE = (DataElement)element;

                    #region replace DataItemGroups
                    foreach (var dataItemGroup in dataE.DataItemGroupList)
                    {
                        FieldGroup fG = (FieldGroup)dataItemGroup;

                        List<eFormRequest.DataItem> dataItemListTemp = new List<eFormRequest.DataItem>();
                        foreach (var dataItem in fG.DataItemList)
                        {
                            foreach (var answer in lstAnswers)
                            {
                                if (dataItem.Id == answer.Id)
                                {
                                    dataItemListTemp.Add(answer);
                                    break;
                                }
                            }
                        }
                        fG.DataItemList = dataItemListTemp;
                    }
                    #endregion

                    #region replace DataItems
                    List<eFormRequest.DataItem> dataItemListTemp2 = new List<eFormRequest.DataItem>();
                    foreach (var dataItem in dataE.DataItemList)
                    {
                        foreach (var answer in lstAnswers)
                        {
                            if (dataItem.Id == answer.FieldId)
                            {
                                dataItemListTemp2.Add(answer);
                                break;
                            }
                        }
                    }
                    dataE.DataItemList = dataItemListTemp2;
                    #endregion

                    elementListReplaced.Add(new CheckListValue(dataE, sqlController.CheckListValueStatusRead(caseId, element.Id)));
                }
                #endregion

                #region if GroupElement
                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;

                    ReplaceDataElementsAndDataItems(caseId, groupE.ElementList, lstAnswers);

                    elementListReplaced.Add(groupE);
                }
                #endregion
            }

            return elementListReplaced;
        }

        private string          SendXml(MainElement mainElement, int siteId)
        {
            TriggerLog("siteId:" + siteId + ", requested sent eForm");

            string xmlStrRequest = mainElement.ClassToXml();
            string xmlStrResponse = communicator.PostXml(xmlStrRequest, siteId);

            Response response = new Response();
            response = response.XmlToClass(xmlStrResponse);

            //if reply is "success", it's created
            if (response.Type.ToString().ToLower() == "success")
            {
                return response.Value;
            }

            throw new Exception("siteId:'" + siteId + "' // failed to create eForm at Microting // Response :" + xmlStrResponse);
        }

        private List<List<string>> GenerateDataSetFromCases (int templatId, DateTime? start, DateTime? end)
        {
            List<List<string>>  dataSet         = new List<List<string>>();
            List<string>        colume1CaseIds  = new List<string> { "Id" };

            List<cases>         caseList        = sqlController.CaseReadAllIds(templatId, start, end);

            if (caseList.Count == 0)
                return null;

            #region remove case that has been "removed"
            for (int i = caseList.Count; i < 0; i--)
            {
                if (caseList[i].workflow_state == "removed")
                    caseList.RemoveAt(i);
            }
            #endregion

            #region firstColumes generate
            {
                List<string> colume2 = new List<string> { "Dato" };
                List<string> colume3 = new List<string> { "Dag" };
                List<string> colume4 = new List<string> { "Uge" };
                List<string> colume5 = new List<string> { "Måned" };
                List<string> colume6 = new List<string> { "År" };
                List<string> colume7 = new List<string> { "Lokation" };
                List<string> colume8 = new List<string> { "Worker" };

                var cal = DateTimeFormatInfo.CurrentInfo.Calendar;
                foreach (var item in caseList)
                {
                    DateTime time = item.done_at.Value;

                    colume1CaseIds.Add(item.id.ToString());
                    colume2.Add(time.ToString("dd.MM.yyyy"));
                    colume3.Add(time.DayOfWeek.ToString());
                    colume4.Add(time.Year.ToString() + "-" + cal.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday));
                    colume5.Add(time.Year.ToString().Substring(2, 2) + "-" + time.ToString("MMMM").Substring(0, 3));
                    colume6.Add(time.Year.ToString());
                    colume7.Add(item.site_id.ToString()); //TODO
                    colume8.Add(item.unit_id.ToString()); //ALSO
                }

                dataSet.Add(colume1CaseIds);
                dataSet.Add(colume2);
                dataSet.Add(colume3);
                dataSet.Add(colume4);
                dataSet.Add(colume5);
                dataSet.Add(colume6);
                dataSet.Add(colume7);
                dataSet.Add(colume8);
            }
            #endregion

            #region fieldValue generate
            {
                MainElement templateData = sqlController.TemplatRead(templatId);

                List<string> lstReturn = new List<string>();
                lstReturn = GenerateDataSetFromCasesSubSet(lstReturn, templateData.ElementList, "");

                List<string> newRow;
                foreach (string set in lstReturn)
                {
                    int fieldId = int.Parse(t.SplitToList(set, 0, false));
                    string label = t.SplitToList(set, 1, false);

                    newRow = sqlController.FieldValueReadAllValues(fieldId, start, end);
                    newRow.Insert(0, label);
                    dataSet.Add(newRow);
                }
            }
            #endregion

            return dataSet;
        }

        private List<string>    GenerateDataSetFromCasesSubSet(List<string> lstReturn, List<Element> elementList, string preLabel)
        {
            foreach (Element element in elementList)
            {
                string sep = " / ";

                #region if DataElement
                if (element.GetType() == typeof(DataElement))
                {
                    DataElement dataE = (DataElement)element;

                    preLabel = preLabel + sep + dataE.Label;

                    foreach (eFormRequest.DataItem dataItem in dataE.DataItemList)
                    {
                        if (dataItem.GetType() == typeof(SaveButton))
                            continue;
                        if (dataItem.GetType() == typeof(None))
                            continue;

                        lstReturn.Add(dataItem.Id + "|" + preLabel.Remove(0, sep.Length) + sep + dataItem.Label);
                    }
                }
                #endregion

                #region if GroupElement
                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;

                    GenerateDataSetFromCasesSubSet(lstReturn, groupE.ElementList, preLabel + sep + groupE.Label);
                }
                #endregion
            }

            return lstReturn;
        }
        #endregion

        #region inward Event handlers
        private void    CoreHandleUpdateDatabases()
        {
            try
            {
                TriggerLog("CoreHandleUpdateDatabases() initiated");

                Thread updateFilesThread            = new Thread(() => CoreHandleUpdateFiles());
                updateFilesThread.Start();

                Thread updateNotificationsThread    = new Thread(() => CoreHandleUpdateNotifications());
                updateNotificationsThread.Start();
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CoreHandleUpdateDatabases failed", ex, true);
            }
        }

        private void    CoreHandleUpdateFiles()
        {
            try
            {
                if (!updateIsRunningFiles)
                {
                    updateIsRunningFiles = true;

                    #region update files
                    string urlStr = "";
                    bool oneFound = true;
                    while (oneFound)
                    {
                        urlStr = sqlController.FileRead();
                        if (urlStr == "")
                        {
                            oneFound = false;
                            break;
                        }

                        #region finding file name and creating folder if needed
                        FileInfo file = new FileInfo(fileLocation);
                        file.Directory.Create(); // If the directory already exists, this method does nothing.

                        int index = urlStr.LastIndexOf("/") + 1;
                        string fileName = urlStr.Remove(0, index);
                        #endregion

                        #region download file
                        using (var client = new WebClient())
                        {
                            try
                            {
                                client.DownloadFile(urlStr, fileLocation + fileName);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Downloading and creating fil locally failed.", ex);
                            }
                        }
                        #endregion

                        #region finding checkSum
                        string chechSum = "";
                        using (var md5 = MD5.Create())
                        {
                            using (var stream = File.OpenRead(fileLocation + fileName))
                            {
                                byte[] grr = md5.ComputeHash(stream);
                                chechSum = BitConverter.ToString(grr).Replace("-", "").ToLower();
                            }
                        }
                        #endregion

                        #region checks checkSum
                        if (chechSum != fileName.Substring(0, 32))
                            HandleEventWarning("Download of '" + urlStr + "' failed. Check sum did not match", EventArgs.Empty);
                        #endregion

                        Case_Dto dto = sqlController.FileCaseFindMUId(urlStr);
                        File_Dto fDto = new File_Dto(dto.SiteUId, dto.CaseType, dto.CaseUId, dto.MicrotingUId, dto.CheckUId, fileLocation + fileName);
                        HandleFileDownloaded(fDto, EventArgs.Empty);
                        TriggerMessage("Downloaded file '" + urlStr + "'.");

                        sqlController.FileProcessed(urlStr, chechSum, fileLocation, fileName);
                    }
                    #endregion

                    updateIsRunningFiles = false;
                }
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CoreHandleUpdateFiles failed", ex, true);
            }
        }

        private void    CoreHandleUpdateNotifications()
        {
            try
            {
                if (!updateIsRunningNotifications)
                {
                    updateIsRunningNotifications = true;

                    #region update notifications
                    string notificationStr = "";
                    string noteUId = "";
                    string noteType = "";
                    bool oneFound = true;
                    while (oneFound)
                    {
                        notificationStr = sqlController.NotificationRead();
                        #region if no new notification found - stops method
                        if (notificationStr == "")
                        {
                            oneFound = false;
                            break;
                        }
                        #endregion

                        try
                        {
                            noteUId = t.Locate(notificationStr, "microting_uuid\\\":\\\"", "\\");
                            noteType = t.Locate(notificationStr, "text\\\":\\\"", "\\\"");

                            switch (noteType)
                            {
                                #region check_status / checklist completed on the device
                                case "check_status":
                                    {
                                        List<Case_Dto> lstCase = new List<Case_Dto>();
                                        MainElement mainElement = new MainElement();

                                        Case_Dto concreteCase = sqlController.CaseReadByMUId(noteUId);
                                        if (concreteCase.CaseUId == "" || concreteCase.CaseUId == "ReversedCase")
                                            lstCase.Add(concreteCase);
                                        else    
                                            lstCase = sqlController.CaseReadByCaseUId(concreteCase.CaseUId);

                                        foreach (Case_Dto aCase in lstCase)
                                        {
                                            if (aCase.SiteUId == concreteCase.SiteUId)
                                            {
                                                #region get response's data and update DB with data
                                                string lastId = sqlController.CaseReadCheckIdByMUId(noteUId);
                                                string respXml;

                                                if (lastId == null)
                                                    respXml = communicator.Retrieve(noteUId, concreteCase.SiteUId);
                                                else
                                                    respXml = communicator.RetrieveFromId(noteUId, concreteCase.SiteUId, lastId);

                                                Response resp = new Response();
                                                resp = resp.XmlToClass(respXml);

                                                if (resp.Type == Response.ResponseTypes.Success)
                                                {
                                                    if (resp.Checks.Count > 0)
                                                    {
                                                        sqlController.ChecksCreate(resp, respXml);

                                                        if (lastId == null)
                                                        {
                                                            int unitId = sqlController.UnitRead(int.Parse(resp.Checks[0].UnitId)).UnitUId;
                                                            int workerId = sqlController.WorkerRead(int.Parse(resp.Checks[0].WorkerId)).WorkerUId;
                                                            sqlController.CaseUpdateCompleted(noteUId, resp.Checks[0].Id, DateTime.Parse(resp.Checks[0].Date), workerId, unitId);

                                                            #region retract case, thereby completing the process

                                                            string responseRetractionXml = communicator.Delete(aCase.MicrotingUId, aCase.SiteUId);
                                                            Response respRet = new Response();
                                                            respRet = respRet.XmlToClass(respXml);

                                                            if (respRet.Type == Response.ResponseTypes.Success)
                                                            {
                                                                sqlController.CaseRetract(aCase.MicrotingUId);
                                                                TriggerLog(aCase.ToString() + " has been retracted");
                                                            }
                                                            else
                                                                TriggerWarning("Failed to retract eForm MicrotingUId:" + aCase.MicrotingUId + "/SideId:" + aCase.SiteUId + ". Not a critical issue, but needs to be fixed if repeated");
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            throw new Exception("Damn!!! - Still needed"); //TODO - remove? and above?
                                                            //int unitId = sqlController.UnitRead(int.Parse(resp.Checks[0].UnitId)).MicrotingUId;
                                                            //int workerId = sqlController.WorkerRead(int.Parse(resp.Checks[0].WorkerId)).MicrotingUid;
                                                            //sqlController.CaseUpdateCompleted(noteUId, resp.Checks[0].Id, DateTime.Parse(resp.Checks[0].Date), workerId, unitId);
                                                        }
  
                                                        Case_Dto cDto = sqlController.CaseReadByMUId(noteUId);
                                                        HandleCaseCompleted(cDto, EventArgs.Empty);
                                                        TriggerMessage(cDto.ToString() + " has been completed");
                                                    }
                                                }
                                                else
                                                    throw new Exception("Failed to retrive eForm " + noteUId + " from site " + aCase.SiteUId);
                                                #endregion
                                            }
                                            else
                                            {
                                                //delete eForm on other tablets and update DB to "deleted"
                                                CaseDelete(aCase.MicrotingUId);
                                            }
                                        }

                                        sqlController.NotificationProcessed(notificationStr, "processed");
                                        break;
                                    }
                                #endregion

                                #region unit fetch / checklist retrieve by device
                                case "unit_fetch":
                                    {
                                        sqlController.CaseUpdateRetrived(noteUId);
                                        Case_Dto cDto = sqlController.CaseReadByMUId(noteUId);
                                        HandleCaseRetrived(cDto, EventArgs.Empty);
                                        TriggerMessage(cDto.ToString() + " has been retrived");

                                        sqlController.NotificationProcessed(notificationStr, "processed");
                                        break;
                                    }
                                #endregion

                                #region unit_activate / tablet added
                                case "unit_activate":
                                    {
                                        HandleSiteActivated(noteUId, EventArgs.Empty);
                                        TriggerMessage(noteUId + " has been added");

                                        sqlController.NotificationProcessed(notificationStr, "processed");
                                        break;
                                    }
                                #endregion

                                default:
                                    throw new IndexOutOfRangeException("Notification type '" + noteType + "' is not known or mapped");
                            }
                        }
                        catch (Exception ex)
                        {
                            HandleEventWarning("CoreHandleUpdateNotifications failed. Case:'" + noteUId + "' marked as 'not_found'. " + ex.Message, EventArgs.Empty);
                            sqlController.NotificationProcessed(notificationStr, "not_found");
                        }
                    }
                    #endregion

                    updateIsRunningNotifications = false;
                }
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CoreHandleUpdateNotifications failed", ex, true);
            }
        }

        private void    CoreHandleUpdateEntityItems()
        {
            try
            {
                if (!updateIsRunningEntities)
                {
                    updateIsRunningEntities = true;

                    #region update EntityItems
                    bool more = true;
                    entity_items eI;
                    while (more)
                    {
                        eI = sqlController.EntityItemSyncedRead();

                        if (eI != null)
                        {
                            try
                            {
                                var type = sqlController.EntityGroupRead(eI.entity_group_id);

                                if (type != null)
                                {
                                    #region EntitySearch
                                    if (type.Type == "EntitySearch")
                                    {
                                        if (eI.workflow_state == "created")
                                        {
                                            string microtingUId = communicator.EntitySearchItemCreate(eI.entity_group_id.ToString(), eI.name, eI.description, eI.entity_item_uid);

                                            if (microtingUId != null)
                                            {
                                                sqlController.EntityItemSyncedProcessed(eI.entity_group_id, eI.entity_item_uid, microtingUId, "created");
                                                continue;
                                            }
                                        }

                                        if (eI.workflow_state == "updated")
                                        {
                                            if (communicator.EntitySearchItemUpdate(eI.entity_group_id.ToString(), eI.microting_uid, eI.name, eI.description, eI.entity_item_uid))
                                            {
                                                sqlController.EntityItemSyncedProcessed(eI.entity_group_id, eI.entity_item_uid, eI.microting_uid, "updated");
                                                continue;
                                            }
                                        }

                                        if (eI.workflow_state == "removed")
                                        {
                                            communicator.EntitySearchItemDelete(eI.microting_uid);

                                            sqlController.EntityItemSyncedProcessed(eI.entity_group_id, eI.entity_item_uid, eI.microting_uid, "removed");
                                            continue;
                                        }
                                    }
                                    #endregion

                                    #region EntitySelect
                                    if (type.Type == "EntitySelect")
                                    {
                                        if (eI.workflow_state == "created")
                                        {
                                            // TODO! el.displayOrder missing and remove int.Parse(eI.description)
                                            string microtingUId = communicator.EntitySelectItemCreate(eI.entity_group_id.ToString(), eI.name, int.Parse(eI.description), eI.entity_item_uid);

                                            if (microtingUId != null)
                                            {
                                                sqlController.EntityItemSyncedProcessed(eI.entity_group_id, eI.entity_item_uid, microtingUId, "created");
                                                continue;
                                            }
                                        }

                                        if (eI.workflow_state == "updated")
                                        {
                                            // TODO! el.displayOrder missing and remove int.Parse(eI.description)
                                            if (communicator.EntitySelectItemUpdate(eI.entity_group_id.ToString(), eI.microting_uid, eI.name, int.Parse(eI.description), eI.entity_item_uid))
                                            {
                                                sqlController.EntityItemSyncedProcessed(eI.entity_group_id, eI.entity_item_uid, eI.microting_uid, "updated");
                                                continue;
                                            }
                                        }

                                        if (eI.workflow_state == "removed")
                                        {
                                            communicator.EntitySelectItemDelete(eI.microting_uid);

                                            sqlController.EntityItemSyncedProcessed(eI.entity_group_id, eI.entity_item_uid, eI.microting_uid, "removed");
                                            continue;
                                        }
                                    }
                                    #endregion
                                }

                                sqlController.EntityItemSyncedProcessed(eI.entity_group_id, eI.entity_item_uid, eI.microting_uid, "failed_to_sync");
                                TriggerWarning("EntityItem entity_group_id:'" + eI.entity_group_id + "', entity_item_uid:'" + eI.entity_item_uid + "', microting:'" + eI.microting_uid + "', workflow_state:'" + eI.workflow_state + "',  failed to sync");
                            }
                            catch
                            {
                                TriggerWarning("EntityItem entity_group_id:'" + eI.entity_group_id + "', entity_item_uid:'" + eI.entity_item_uid + "', microting:'" + eI.microting_uid + "', workflow_state:'" + eI.workflow_state + "',  failed to sync");
                                sqlController.EntityItemSyncedProcessed(eI.entity_group_id, eI.entity_item_uid, eI.microting_uid, "failed to sync");
                            }
                        }
                        else
                            more = false;
                    }
                    #endregion

                    updateIsRunningEntities = false;
                }
            }
            catch (Exception ex)
            {
                TriggerWarning     (ex.Message);
                TriggerHandleExpection("CoreHandleUpdateEntityItems failed", ex, true);
            }
        }

        private void    CoreHandleEventClient(object sender, EventArgs args)
        {
            try
            {
                lock (_lockEventClient)
                {
                    TriggerLog("Client # " + sender.ToString());
                }
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CoreHandleEventClient failed", ex, false);
            }
        }

        private void    CoreHandleEventServer(object sender, EventArgs args)
        {
            try
            {
                lock (_lockEventServer)
                {
                    string reply = sender.ToString();
                    TriggerLog("Server # " + reply);

                    if (reply.Contains("-update\",\"data"))
                    {
                        if (reply.Contains("\"id\\\":"))
                        {
                            string mUId = t.Locate(reply, "microting_uuid\\\":\\\"", "\\");
                            string nfId = t.Locate(reply, "\"id\\\":", ",");

                            sqlController.NotificationCreate(mUId, reply);
                            subscriber.ConfirmId(nfId);
                        }
                    }

                    //checks if there already are unprocessed files and notifications in the system
                    CoreHandleUpdateDatabases();
                }
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CoreHandleEventServer failed", ex, true);
            }
        }

        private void    CoreHandleEventLog(object sender, EventArgs args)
        {
            try
            {
                lock (_lockEventMessage)
                {
                    TriggerLog("Log    # " + sender.ToString());
                }
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CoreHandleEventMessage failed", ex, true);
            }
        }
        #endregion

        #region outward Event triggers
        private void    TriggerLog(string str)
        {
            if (logLevel)
            {
                HandleEventLog(DateTime.Now.ToLongTimeString() + ":" + str, EventArgs.Empty);
            }
        }

        private void    TriggerMessage(string str)
        {
            TriggerLog(str);
            HandleEventMessage(str, EventArgs.Empty);
        }

        private void    TriggerWarning(string str)
        {
            TriggerLog(str);
            HandleEventWarning(str, EventArgs.Empty);
        }

        private void    TriggerHandleExpection(string exceptionDescription, Exception ex, bool restartCore)
        {
            try
            {
                HandleEventException(ex, EventArgs.Empty);

                string fullExceptionDescription = t.PrintException(exceptionDescription, ex);
                TriggerMessage (fullExceptionDescription);

                ExceptionClass exCls = new ExceptionClass(fullExceptionDescription, DateTime.Now);
                exceptionLst.Add(exCls);

                int secondsDelay = CheckExceptionLst(exCls);

                if (restartCore)
                {
                    Thread coreRestartThread = new Thread(() => Restart(secondsDelay));
                    coreRestartThread.Start();
                }
            }
            catch
            {
                coreRunning = false;
                throw new Exception("FATAL Exception. Core failed to handle an Expection", ex);
            }
        }

        private int         CheckExceptionLst(ExceptionClass exceptionClass)
        {
            int secondsDelay = 1;

            int count = 0;
            #region find count
            try
            {
                //remove Exceptions older than an hour
                for (int i = exceptionLst.Count; i < 0; i--)
                {
                    if (exceptionLst[i].Time < DateTime.Now.AddHours(-1))
                        exceptionLst.RemoveAt(i);
                }

                //keep only the last 10 Exceptions
                if (exceptionLst.Count > 10)
                {
                    exceptionLst.RemoveAt(0);
                }

                //find highest court of the same Exception
                if (exceptionLst.Count > 1)
                {
                    foreach (ExceptionClass exCls in exceptionLst)
                    {
                        if (exceptionClass.Description == exCls.Description)
                        {
                            count++;
                        }
                    }
                }
            }
            catch { }
            #endregion

            TriggerMessage(count + ". time the same Exception, within the last hour");
            if (count == 2) secondsDelay = 6;
            if (count == 3) secondsDelay = 60;
            if (count == 4) secondsDelay = 600;
            if (count >  4) throw new Exception("FATAL Exception. Same Exception repeated to many times within one hour");
            return secondsDelay;
        }

        private void        Restart(int secondsDelay)
        {
            try
            {
                if (coreRestarting == false)
                {
                    coreRestarting = true;

                    TriggerMessage("Core.Restart() at:" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                    Close();
                    TriggerMessage("");
                    TriggerMessage("Trying to restart the Core in " + secondsDelay + " seconds");
                    Thread.Sleep(secondsDelay * 1000);
                    Start(serverConnectionString);

                    coreRestarting = false;
                }
            }
            catch (Exception ex)
            {
                coreRunning = false;
                throw new Exception("FATAL Exception. Core failed to restart", ex);
            }
        }
        #endregion
    }

    internal class ExceptionClass
    {
        private     ExceptionClass()
        {

        }

        internal    ExceptionClass(string description, DateTime time)
        {
            Description = description;
            Time = time;
        }

        public string Description { get; set; }
        public DateTime Time { get; set; }
    }
}