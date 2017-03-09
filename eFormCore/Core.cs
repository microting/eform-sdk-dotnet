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
using eFormData;
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
using System.Text;

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
        eFormShared.Tools t = new eFormShared.Tools();

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
                    if (sqlController.SettingRead("firstRunDone") == "false")
                        throw new ArgumentException("firstRunDone==false. Use AdminTools to setup settings");

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
                try
                {
                    comToken = sqlController.SettingRead("comToken");
                    comAddress = sqlController.SettingRead("comAddress");
                    organizationId = sqlController.SettingRead("organizationId");
                    return true;
                } catch (Exception ex2)
                {
                    throw new Exception("FATAL Exception. Could not read settings!", ex2);
                }
                
                throw new Exception("FATAL Exception. Core failed to start", ex);
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

                xmlString = xmlString.Replace("=\"ShowPDF\">", "=\"ShowPdf\">");
       
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

                xmlString = xmlString.Replace("<MinValue />", "<MinValue>" + long.MinValue + "</MinValue>");
                xmlString = xmlString.Replace("<MinValue/>", "<MinValue>" + long.MinValue + "</MinValue>");
                xmlString = xmlString.Replace("<MaxValue />", "<MaxValue>" + long.MaxValue + "</MaxValue>");
                xmlString = xmlString.Replace("<MaxValue/>", "<MaxValue>" + long.MaxValue + "</MaxValue>");
                xmlString = xmlString.Replace("<MaxValue></MaxValue>", "<MaxValue>" + long.MaxValue + "</MaxValue>");
                xmlString = xmlString.Replace("<MinValue></MinValue>", "<MinValue>" + long.MinValue + "</MinValue>");
                xmlString = xmlString.Replace("<DecimalCount></DecimalCount>", "<DecimalCount>0</DecimalCount>");
                xmlString = xmlString.Replace("<DecimalCount />", "<DecimalCount>" + "0" + "</DecimalCount>");
                xmlString = xmlString.Replace("<DecimalCount/>", "<DecimalCount>" + "0" + "</DecimalCount>");

                List<string> dILst = t.LocateList(xmlString, "type=\"Date\">", "</DataItem>");
                if (dILst != null)
                {
                    foreach (var item in dILst)
                    {
                        string before = item;
                        string after = item;
                        after = after.Replace("<Value />", "<DefaultValue/>");
                        after = after.Replace("<Value/>", "<DefaultValue/>");
                        after = after.Replace("<Value>", "<DefaultValue>");
                        after = after.Replace("</Value>", "</DefaultValue>");

                        xmlString = xmlString.Replace(before, after);
                    }
                }
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

        public List<string>     TemplatValidation(MainElement mainElement)
        {
            if (mainElement == null)
                throw new ArgumentNullException("mainElement not allowed to be null");

            try
            {
                if (coreRunning)
                {
                    List<string> errorLst = new List<string>();
                    var dataItems = mainElement.DataItemGetAll();

                    foreach ( var dataItem in dataItems)
                    {
                        #region entities
                        if (dataItem.GetType() == typeof(EntitySearch))
                        {
                            EntitySearch entitySearch = (EntitySearch)dataItem;
                            var temp = sqlController.EntityGroupRead(entitySearch.EntityTypeId.ToString());
                            if (temp == null)
                                errorLst.Add("Element entitySearch.EntityTypeId:'" + entitySearch.EntityTypeId + "' is an reference to a local unknown EntitySearch group. Please update reference");
                        }

                        if (dataItem.GetType() == typeof(EntitySelect))
                        {
                            EntitySelect entitySelect = (EntitySelect)dataItem;
                            var temp = sqlController.EntityGroupRead(entitySelect.Source.ToString());
                            if (temp == null)
                                errorLst.Add("Element entitySelect.Source:'" + entitySelect.Source + "' is an reference to a local unknown EntitySearch group. Please update reference");
                        }
                        #endregion

                        #region PDF
                        if (dataItem.GetType() == typeof(ShowPdf))
                        {
                            ShowPdf showPdf = (ShowPdf)dataItem;
                            if (showPdf.Value.ToLower().Contains("microting.com"))
                                errorLst.Add("Element showPdf.Id:'" + showPdf.Id + "' contains an URL that points to Microting's builder temporary hosting. Move the fil to a proper hosting URL");
                            if (!showPdf.Value.ToLower().Contains("http"))
                                if (!showPdf.Value.ToLower().Contains("https"))
                                    errorLst.Add("Element showPdf.Id:'" + showPdf.Id + "' lacks HTTP or HTTPS. Indicating that it's not a proper URL");
                        }
                        #endregion
                    }

                    return errorLst;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("TemplatValidation failed", ex, true);
                throw new Exception("TemplatValidation failed", ex);
            }
        }

        public int              TemplatCreate(MainElement mainElement)
        {
            if (mainElement == null)
                throw new ArgumentNullException("mainElement not allowed to be null");

            try
            {
                if (coreRunning)
                {
                    List<string> errors = TemplatValidation(mainElement);

                    if (errors == null) errors = new List<string>();
                    if (errors.Count > 0)
                        throw new Exception("mainElement failed TemplatValidation. Run TemplatValidation to see errors");

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

        public Template_Dto TemplateSimpleRead(int templatId)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("Templat id:" + templatId.ToString() + " trying to be read");
                    return sqlController.TemplateSimpleRead(templatId);
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

        public List<Template_Dto> TemplateSimpleReadAll()
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("TemplateSimpleReadAll() called");
                    return TemplateSimpleReadAll("not_removed");
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("TemplateSimpleReadAll failed", ex, true);
                throw new Exception("TemplateSimpleReadAll failed", ex);
            }
        }

        public List<Template_Dto> TemplateSimpleReadAll(string workflowState)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("TemplateSimpleReadAll() called");
                    return sqlController.TemplateSimpleReadAll(workflowState);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("TemplateSimpleReadAll failed", ex, true);
                throw new Exception("TemplateSimpleReadAll failed", ex);
            }
        }

        public List<MainElement> TemplatReadAll()
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

        public bool             TemplateDelete(int templatId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("templatId:" + templatId);

                    return sqlController.TemplatDelete(templatId);
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

        #region field
        public Field        FieldRead(int id)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("id:" + id);

                    return sqlController.FieldRead(id);
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
                        DateTime start = DateTime.Parse(mainElement.StartDate.ToShortDateString());
                        DateTime end   = DateTime.Parse(mainElement.EndDate  .ToShortDateString());

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
                                sqlController.CaseCreate(mainElement.Id, siteId, mUId, null, caseUId, custom, DateTime.Now);
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

        public string           CaseCheck(string microtingUId)
        {
            try
            {
                if (coreRunning)
                {
                    Case_Dto cDto = CaseLookupMUId(microtingUId);
                    return communicator.CheckStatus(cDto.MicrotingUId, cDto.SiteUId);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseCheck failed", ex, true);
                throw new Exception("CaseCheck failed", ex);
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

                    ReplyElement replyElement = sqlController.CheckRead(microtingUId, checkUId);
                    //ReplyElement replyElement = new ReplyElement(sqlController.TemplatRead((int)aCase.check_list_id));
                    //replyElement.Custom = aCase.custom;
                    //replyElement.DoneAt = (DateTime)aCase.done_at;
                    //replyElement.DoneById = (int)aCase.done_by_user_id;
                    //replyElement.UnitId = (int)aCase.unit_id;

                    //List<FieldValue> lstAnswers = new List<FieldValue>();
                    //List<field_values> lstReplies = sqlController.ChecksRead(microtingUId, checkUId);
                    //#region remove replicates from lstReplies. Ex. multiple pictures
                    //List<field_values> lstRepliesTemp = new List<field_values>();
                    //bool found;

                    //foreach (var reply in lstReplies)
                    //{
                    //    found = false;
                    //    foreach (var tempReply in lstRepliesTemp)
                    //    {
                    //        if (reply.field_id == tempReply.field_id)
                    //        {
                    //            found = true;
                    //            break;
                    //        }
                    //    }
                    //    if (found == false)
                    //        lstRepliesTemp.Add(reply);
                    //}

                    //lstReplies = lstRepliesTemp;
                    //#endregion

                    //foreach (field_values reply in lstReplies)
                    //{
                    //    FieldValue answer = sqlController.FieldValueRead(reply.id);
                    //    lstAnswers.Add(answer);
                    //}
                    //TriggerLog("Questons and answers found");

                    ////replace DataItem(s) with DataItem(s) Answer
                    //replyElement.ElementList = ReplaceDataElementsAndDataItems(aCase.id, replyElement.ElementList, lstAnswers);

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

        public Case_Dto CaseReadByCaseId(int id)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("Templat id:" + id.ToString() + " trying to read all cases");
                    return sqlController.CaseReadByCaseId(id);
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

        public List<Case>       CaseReadAll(int? templatId, DateTime? start, DateTime? end)
        {
            return CaseReadAll(templatId, start, end, "not_removed");
        }
        public List<Case>       CaseReadAll(int? templatId, DateTime? start, DateTime? end, string workflowState)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("Templat id:" + templatId.ToString() + " trying to read all cases");

                    return sqlController.CaseReadAll(templatId, start, end, workflowState);
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

        public bool CaseDelete(int templateId, int siteUId)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("templateId:" + templateId + "siteUId:" + siteUId + ", requested to be deleted");
                    int microtingUId = sqlController.CheckListSitesRead(templateId, siteUId);
                    return CaseDelete(microtingUId.ToString());
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

        public string           CasesToExcel(int? templatId, DateTime? start, DateTime? end, string pathAndName)
        {
            try
            {
                if (coreRunning)
                {
                    List<List<string>> dataSet = GenerateDataSetFromCases(templatId, start, end);

                    if (dataSet == null)
                        return "";

                    if (!pathAndName.Contains(".xlsx"))
                        pathAndName = pathAndName + ".xlsx";

                    return excelController.CreateExcel(dataSet, pathAndName);
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

        public string           CasesToCsv(int? templatId, DateTime? start, DateTime? end, string pathAndName)
        {
            try
            {
                if (coreRunning)
                {
                    List<List<string>> dataSet = GenerateDataSetFromCases(templatId, start, end);

                    if (dataSet == null)
                        return "";

                    List<string> temp;
                    string text = "";

                    for (int rowN = 0; rowN < dataSet[0].Count; rowN++)
                    {
                        temp = new List<string>();

                        foreach (List<string> lst in dataSet)
                            temp.Add(lst[rowN]); 

                        text += string.Join(";", temp.ToArray()) + Environment.NewLine;
                    }

                    if (!pathAndName.Contains(".csv"))
                        pathAndName = pathAndName + ".csv";

                    File.WriteAllText(pathAndName, text.TrimEnd(), Encoding.UTF8);
                    return Path.GetFullPath(pathAndName);
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CasesToCsv failed", ex, false);
                return "";
            }
        }
        #endregion

        #region sites
        public List<SiteName_Dto>   SiteGetAll()
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    return sqlController.SiteGetAll();
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

        public List<Site_Dto>       SimpleSiteGetAll()
        {
            return SimpleSiteGetAll("not_removed", null, null);
        }

        public List<Site_Dto>       SimpleSiteGetAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    return sqlController.SimpleSiteGetAll(workflowState, offSet, limit);
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

        public Site_Dto SiteCreateSimple(string name, string userFirstName, string userLastName, string userEmail)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("siteName:" + name + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName);

                    Tuple<Site_Dto, Unit_Dto> siteResult = communicator.SiteCreate(name);

                    Organization_Dto organizationDto = communicator.OrganizationLoadAllFromRemote();

                    int customerNo = organizationDto.CustomerNo;

                    string siteName = siteResult.Item1.SiteName;
                    int siteId = siteResult.Item1.SiteId;
                    int unitUId = siteResult.Item2.UnitUId;
                    int otpCode = siteResult.Item2.OtpCode;
                    SiteName_Dto siteDto = sqlController.SiteRead(siteResult.Item1.SiteId);
                    if (siteDto == null)
                    {
                        sqlController.SiteCreate((int)siteId, siteName);
                    }
                    siteDto = sqlController.SiteRead(siteId);
                    Unit_Dto unitDto = sqlController.UnitRead(unitUId);
                    if (unitDto == null)
                    {
                        sqlController.UnitCreate(unitUId, customerNo, otpCode, siteDto.SiteUId);
                    }

                    if (string.IsNullOrEmpty(userEmail))
                    {
                        Random rdn = new Random();
                        userEmail = siteId + "." + customerNo + "@invalid.invalid";
                    }

                    Worker_Dto workerDto = WorkerCreate(userFirstName, userLastName, userEmail);

                    SiteWorkerCreate(siteDto, workerDto);

                    return SiteReadSimple(siteId);
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

        public bool                 SiteUpdateSimple(int siteId, string name, string userFirstName, string userLastName, string userEmail)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    Site_Dto siteDto = SiteReadSimple(siteId);
                    SiteUpdate(siteId, name);
                    WorkerUpdate((int)siteDto.WorkerUid, userFirstName, userLastName, userEmail);
                    return true;
                } else
                    throw new Exception("Core is not running");

            } catch (Exception ex)
            {
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public bool                 SiteDeleteSimple(int siteId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    Site_Dto siteDto = SiteReadSimple(siteId);
                    SiteDelete(siteId);
                    Site_Worker_Dto siteWorkerDto = SiteWorkerRead(null, siteId, siteDto.WorkerUid);
                    SiteWorkerDelete(siteWorkerDto.MicrotingUId);
                    WorkerDelete((int)siteDto.WorkerUid);
                    return true;
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

                    Tuple<Site_Dto, Unit_Dto> siteResult = communicator.SiteCreate(name);

                    Organization_Dto organizationDto = communicator.OrganizationLoadAllFromRemote();

                    int customerNo = organizationDto.CustomerNo;

                    string siteName = siteResult.Item1.SiteName;
                    int siteId = siteResult.Item1.SiteId;
                    int unitUId = siteResult.Item2.UnitUId;
                    int otpCode = siteResult.Item2.OtpCode;
                    SiteName_Dto siteDto = sqlController.SiteRead(siteResult.Item1.SiteId);
                    if (siteDto == null)
                    {
                        sqlController.SiteCreate((int)siteId, siteName);
                    }
                    siteDto = sqlController.SiteRead(siteId);
                    Unit_Dto unitDto = sqlController.UnitRead(unitUId);
                    if (unitDto == null)
                    {
                        sqlController.UnitCreate(unitUId, customerNo, otpCode, siteDto.SiteUId);
                    }
                    return SiteRead(siteDto.SiteUId);
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
        public List<Worker_Dto> WorkerGetAll()
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    return sqlController.WorkerGetAll();
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

        public Worker_Dto       WorkerCreate(string firstName, string lastName, string email)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("firstName:" + firstName + " / lastName:" + lastName + " / email:" + email);


                    Worker_Dto workerDto = communicator.WorkerCreate(firstName, lastName, email);
                    int workerUId = workerDto.WorkerUId;

                    workerDto = sqlController.WorkerRead(workerDto.WorkerUId);
                    if (workerDto == null)
                    {
                        sqlController.WorkerCreate(workerUId, firstName, lastName, email);
                    }

                    return WorkerRead(workerUId);
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
                    TriggerLog(methodName + " called");
                    TriggerLog("siteId:" + siteDto.SiteUId + " / workerId:" + workerDto.WorkerUId);

                    Site_Worker_Dto result = communicator.SiteWorkerCreate(siteDto.SiteUId, workerDto.WorkerUId);
                    //var parsedData = JRaw.Parse(result);
                    //int workerUid = int.Parse(parsedData["id"].ToString());

                    Site_Worker_Dto siteWorkerDto = sqlController.SiteWorkerRead(result.WorkerUId, null, null);

                    if (siteWorkerDto == null)
                    {
                        sqlController.SiteWorkerCreate(result.WorkerUId, siteDto.SiteUId, workerDto.WorkerUId);
                    }

                    return SiteWorkerRead(result.WorkerUId, null, null);
                    //return null;
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

        public Site_Worker_Dto SiteWorkerRead(int? siteWorkerId, int? siteId, int? workerId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("siteWorkerId:" + siteWorkerId);

                    return sqlController.SiteWorkerRead(siteWorkerId, siteId, workerId);
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
        public List<Unit_Dto> UnitGetAll()
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    return sqlController.UnitGetAll();
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

                        List<DataItem> dataItemListTemp = new List<DataItem>();
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
                    List<DataItem> dataItemListTemp2 = new List<DataItem>();
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

        private List<List<string>> GenerateDataSetFromCases (int? templatId, DateTime? start, DateTime? end)
        {
            List<List<string>>  dataSet         = new List<List<string>>();
            List<string>        colume1CaseIds  = new List<string> { "Id" };

            List<Case>         caseList        = sqlController.CaseReadAll(templatId, start, end, "not_removed");

            if (caseList.Count == 0)
                return null;

            #region remove cases that are not completed
            //for (int i = caseList.Count; i < 0; i--)
            //{
            //    if (caseList[i].WorkflowState != "retracted")
            //        caseList.RemoveAt(i);
            //}
            #endregion

            #region firstColumes generate
            {
                List<string> colume2 = new List<string> { "Date" };
                List<string> colume3 = new List<string> { "Time" };
                List<string> colume4 = new List<string> { "Day" };
                List<string> colume5 = new List<string> { "Week" };
                List<string> colume6 = new List<string> { "Month" };
                List<string> colume7 = new List<string> { "Year" };
                List<string> colume8 = new List<string> { "Site" };
                List<string> colume9 = new List<string> { "Device User" };
                List<string> colume10 = new List<string> { "Device Id" };

                var cal = DateTimeFormatInfo.CurrentInfo.Calendar;
                foreach (var aCase in caseList)
                {
                    DateTime time = (DateTime)aCase.DoneAt;
                    colume1CaseIds.Add(aCase.Id.ToString());

                    colume2.Add(time.ToString("yyyy.MM.dd"));
                    colume3.Add(time.ToString("hh:mm:ss"));
                    colume4.Add(time.DayOfWeek.ToString());
                    colume5.Add(time.Year.ToString() + "." + cal.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday));
                    colume6.Add(time.Year.ToString() + "." + time.ToString("MMMM").Substring(0, 3));
                    colume7.Add(time.Year.ToString());
                    colume8.Add(aCase.SiteName);
                    colume9.Add(aCase.WorkerName);
                    colume10.Add(aCase.UnitId.ToString());
                }

                dataSet.Add(colume1CaseIds);
                dataSet.Add(colume2);
                dataSet.Add(colume3);
                dataSet.Add(colume4);
                dataSet.Add(colume5);
                dataSet.Add(colume6);
                dataSet.Add(colume7);
                dataSet.Add(colume8);
                dataSet.Add(colume9);
                dataSet.Add(colume10);
            }
            #endregion

            #region fieldValue generate
            {
                if (templatId != null)
                {
                    MainElement templateData = sqlController.TemplatRead((int)templatId);

                    List<string> lstReturn = new List<string>();
                    lstReturn = GenerateDataSetFromCasesSubSet(lstReturn, templateData.ElementList, "");

                    List<string> newRow;
                    foreach (string set in lstReturn)
                    {
                        int fieldId = int.Parse(t.SplitToList(set, 0, false));
                        string label = t.SplitToList(set, 1, false);

                        List<List<string>> result = sqlController.FieldValueReadAllValues(fieldId, start, end);

                        if (result.Count == 1)
                        {
                            newRow = result[0];
                            newRow.Insert(0, label);
                            dataSet.Add(newRow);
                        }
                        else
                        {
                            int option = 0;
                            foreach (var lst in result)
                            {
                                option++;
                                newRow = lst;
                                newRow.Insert(0, label + " | Option " + option);
                                dataSet.Add(newRow);
                            }
                        }
                    }
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

                    foreach (FieldGroup dataItemGroup in dataE.DataItemGroupList)
                    {
                        foreach (DataItem dataItem in dataItemGroup.DataItemList)
                        {
                            if (dataItem.GetType() == typeof(SaveButton))
                                continue;
                            if (dataItem.GetType() == typeof(None))
                                continue;

                            lstReturn.Add(dataItemGroup.Id + "|" + preLabel.Remove(0, sep.Length) + sep + dataItemGroup.Label + sep + dataItem.Label);
                        }
                    }

                    foreach (DataItem dataItem in dataE.DataItemList)
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
                        if (chechSum != fileName.Substring(fileName.LastIndexOf(".")-32, 32))
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
                                                string checkIdLastKnown = sqlController.CaseReadCheckIdByMUId(noteUId); //null if NOT a checkListSite
                                                string respXml;

                                                if (checkIdLastKnown == null)
                                                    respXml = communicator.Retrieve(noteUId, concreteCase.SiteUId);
                                                else
                                                    respXml = communicator.RetrieveFromId(noteUId, concreteCase.SiteUId, checkIdLastKnown);

                                                Response resp = new Response();
                                                resp = resp.XmlToClass(respXml);

                                                if (resp.Type == Response.ResponseTypes.Success)
                                                {
                                                    if (resp.Checks.Count > 0)
                                                    {
                                                        sqlController.ChecksCreate(resp, respXml);

                                                        int unitUId = sqlController.UnitRead(int.Parse(resp.Checks[0].UnitId)).UnitUId;
                                                        int workerUId = sqlController.WorkerRead(int.Parse(resp.Checks[0].WorkerId)).WorkerUId;
                                                        sqlController.CaseUpdateCompleted(noteUId, resp.Checks[0].Id, DateTime.Parse(resp.Checks[0].Date), workerUId, unitUId);

                                                        #region IF needed retract case, thereby completing the process
                                                        if (checkIdLastKnown == null)
                                                        {
                                                            string responseRetractionXml = communicator.Delete(aCase.MicrotingUId, aCase.SiteUId);
                                                            Response respRet = new Response();
                                                            respRet = respRet.XmlToClass(respXml);

                                                            if (respRet.Type == Response.ResponseTypes.Success)
                                                            {
                                                                TriggerLog(aCase.ToString() + " has been retracted");
                                                            }
                                                            else
                                                                TriggerWarning("Failed to retract eForm MicrotingUId:" + aCase.MicrotingUId + "/SideId:" + aCase.SiteUId + ". Not a critical issue, but needs to be fixed if repeated");
                                                        }
                                                        #endregion

                                                        sqlController.CaseRetract(noteUId, resp.Checks[0].Id);

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
                                        try
                                        {
                                            Unit_Dto unitDto = sqlController.UnitRead(int.Parse(noteUId));
                                            sqlController.UnitUpdate(unitDto.UnitUId, unitDto.CustomerNo, 0, unitDto.SiteUId);
                                            sqlController.NotificationProcessed(notificationStr, "processed");
                                        } catch
                                        {
                                            sqlController.NotificationProcessed(notificationStr, "processed");
                                        }
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
                                            string microtingUId = communicator.EntitySelectItemCreate(eI.entity_group_id.ToString(), eI.name, eI.display_index, eI.entity_item_uid);

                                            if (microtingUId != null)
                                            {
                                                sqlController.EntityItemSyncedProcessed(eI.entity_group_id, eI.entity_item_uid, microtingUId, "created");
                                                continue;
                                            }
                                        }

                                        if (eI.workflow_state == "updated")
                                        {
                                            // TODO! el.displayOrder missing and remove int.Parse(eI.description)
                                            if (communicator.EntitySelectItemUpdate(eI.entity_group_id.ToString(), eI.microting_uid, eI.name, eI.display_index, eI.entity_item_uid))
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
                            catch (Exception ex)
                            {
                                TriggerWarning("EntityItem entity_group_id:'" + eI.entity_group_id + "', entity_item_uid:'" + eI.entity_item_uid + "', microting:'" + eI.microting_uid + "', workflow_state:'" + eI.workflow_state + "',  failed to sync. Exception:'" + ex.Message + "'");
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