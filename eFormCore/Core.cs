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
using System.Xml;

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
        object _lockEventMessage = new object();

        bool updateIsRunningFiles = false;
        bool updateIsRunningNotifications = false;
        bool updateIsRunningTables = false;
        bool updateIsRunningEntities = false;

        bool coreRunning = false;
        bool coreRestarting = false;
        bool coreStatChanging = false;
        bool coreThreadAlive = false;

        List<ExceptionClass> exceptionLst = new List<ExceptionClass>();

        string connectionString;
        string fileLocationPicture;
        string fileLocationPdf;
        bool logLevel = false;
        #endregion

        #region con
        public Core()
        {

        }
        #endregion

        #region public state
        public bool             Start(string connectionString)
        {
            try
            {
                if (!coreRunning && !coreStatChanging)
                {
                    if (string.IsNullOrEmpty(connectionString))
                        throw new ArgumentException("serverConnectionString is not allowed to be null or empty");

                    coreStatChanging = true;

                    //sqlController
                    sqlController = new SqlController(connectionString);
                    logLevel = bool.Parse(sqlController.SettingRead(Settings.logLevel));
                    TriggerLog("SqlEformController started");

                    #region settings read
                    if (!sqlController.SettingCheckAll())
                        throw new ArgumentException("Use AdminTool to setup database correct");

                    fileLocationPicture = sqlController.SettingRead(Settings.fileLocationPicture);
                    fileLocationPdf = sqlController.SettingRead(Settings.fileLocationPdf);
                    this.connectionString = connectionString;
                    TriggerLog("Settings read");
                    #endregion

                    #region core.Start()
                    TriggerLog("");
                    TriggerLog("");
                    TriggerLog("###################################################################################################################");
                    TriggerMessage("Core.Start() at:" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                    TriggerLog("###################################################################################################################");
                    TriggerLog("connectionString:'" + connectionString + "'");
                    TriggerLog("Core started");
                    #endregion

                    //communicators
                    communicator = new Communicator(sqlController);
                    TriggerLog("Communicator started");

                    //subscriber
                    subscriber = new Subscriber(sqlController);
                    subscriber.Start();
                    TriggerLog("Subscriber started");

                    //communicators
                    excelController = new ExcelController();
                    TriggerLog("Excel (Office) started");

                    //coreThread
                    Thread coreThread = new Thread(() => CoreThread());
                    coreThread.Start();
                    TriggerLog("CoreThread started");

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

        public bool             StartSqlOnly(string connectionString)
        {
            try
            {
                if (!coreRunning && !coreStatChanging)
                {
                    if (string.IsNullOrEmpty(connectionString))
                        throw new ArgumentException("serverConnectionString is not allowed to be null or empty");

                    coreStatChanging = true;

                    //sqlController
                    sqlController = new SqlController(connectionString);
                    logLevel = bool.Parse(sqlController.SettingRead(Settings.logLevel));
                    TriggerLog("SqlEformController started");

                    #region settings read
                    if (!sqlController.SettingCheckAll())
                        throw new ArgumentException("Use AdminTool to setup database correct");

                    fileLocationPicture = sqlController.SettingRead(Settings.fileLocationPicture);
                    fileLocationPdf = sqlController.SettingRead(Settings.fileLocationPdf);
                    this.connectionString = connectionString;
                    TriggerLog("Settings read");
                    #endregion

                    #region core.Start()
                    TriggerLog("");
                    TriggerLog("");
                    TriggerLog("###################################################################################################################");
                    TriggerMessage("Core.Start() at:" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                    TriggerLog("###################################################################################################################");
                    TriggerLog("connectionString:'" + connectionString + "'");
                    TriggerLog("Core started");
                    #endregion

                    //communicators
                    communicator = new Communicator(sqlController);
                    TriggerLog("Communicator started");
                    coreRunning = true;

                    coreStatChanging = false;
                }
            }
            catch (Exception ex)
            {
                coreRunning = false;
                coreStatChanging = false;
                if (ex.InnerException.Message.Contains("PrimeDb"))
                {
                    throw ex.InnerException;
                }
                try
                {
                    return true;
                } catch (Exception ex2)
                {
                    throw new Exception("FATAL Exception. Could not read settings!", ex2);
                }
                
                throw new Exception("FATAL Exception. Core failed to start", ex);
            }
            return true;
        }

        public bool             Close()
        {
            try
            {
                if (coreRunning && !coreStatChanging)
                {
                    lock (_lockMain) //Will let sending Cases sending finish, before closing
                    {
                        coreStatChanging = true;

                        coreThreadAlive = false;
                        TriggerMessage("Core.Close() at:" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());

                        try
                        {
                            TriggerMessage("Subscriber requested to close connection");
                            subscriber.Close();
                            TriggerLog("Subscriber closed");
                        }
                        catch { }

                        while (coreRunning)
                            Thread.Sleep(200);

                        subscriber = null;
                        communicator = null;
                        sqlController = null;

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

        public bool             Running()
        {
            return coreRunning;
        }
        #endregion

        #region public actions
        #region template
        public MainElement      TemplateFromXml(string xmlString)
        {
            string methodName = t.GetMethodName();
            try
            {
                TriggerLog(methodName + " called");
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
                xmlString = xmlString.Replace("=\"choose_entity\">", "=\"EntitySearch\">");
       
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
                TriggerHandleExpection(methodName + " failed", ex, false);
                return null;
            }
        }

        public List<string>     TemplateValidation(MainElement mainElement)
        {
            if (mainElement == null)
                throw new ArgumentNullException("mainElement not allowed to be null");

            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");

                    List<string> errorLst = new List<string>();
                    var dataItems = mainElement.DataItemGetAll();

                    foreach (var dataItem in dataItems)
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
                            errorLst.AddRange(PdfValidate(showPdf.Value, showPdf.Id));
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
                TriggerHandleExpection(methodName + " failed", ex, true);
                throw new Exception(methodName + " failed", ex);
            }
        }

        public MainElement      TemplateUploadData(MainElement mainElement)
        {
            if (mainElement == null)
                throw new ArgumentNullException("mainElement not allowed to be null");

            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");

                    List<string> errorLst = new List<string>();
                    var dataItems = mainElement.DataItemGetAll();

                    foreach (var dataItem in dataItems)
                    {
                        #region PDF
                        if (dataItem.GetType() == typeof(ShowPdf))
                        {
                            ShowPdf showPdf = (ShowPdf)dataItem;

                            if (PdfValidate(showPdf.Value, showPdf.Id).Count != 0)
                            {
                                try
                                {
                                    //download file
                                    string downloadPath = sqlController.SettingRead(Settings.fileLocationPdf);
                                    try
                                    {
                                        (new FileInfo(downloadPath)).Directory.Create();

                                        using (WebClient client = new WebClient())
                                        {
                                            client.DownloadFile(showPdf.Value, downloadPath + "temp.pdf");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Download failed. Path:'" + showPdf.Value + "'", ex);
                                    }

                                    //upload file
                                    string hash = PdfUpload(downloadPath + "temp.pdf");
                                    if (hash != null)
                                    {
                                        //rename local file
                                        FileInfo FileInfo = new FileInfo(downloadPath + "temp.pdf");
                                        FileInfo.CopyTo(downloadPath + hash + ".pdf", true);
                                        FileInfo.Delete();

                                        showPdf.Value = hash;
                                    }
                                    else
                                    {
                                        throw new Exception(methodName + " hash is null for field id:'" + showPdf.Id + "'");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(methodName + " failed, for one PDF field id:'" + showPdf.Id + "'", ex);
                                }
                            }
                        }
                        #endregion
                    }

                    return mainElement;
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

        public int              TemplateCreate(MainElement mainElement)
        {
            if (mainElement == null)
                throw new ArgumentNullException("mainElement not allowed to be null");

            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");

                    List<string> errors = TemplateValidation(mainElement);

                    if (errors == null) errors = new List<string>();
                    if (errors.Count > 0)
                        throw new Exception("mainElement failed TemplateValidation. Run TemplateValidation to see errors");

                    int templateId = sqlController.TemplateCreate(mainElement);
                    TriggerLog("Template id:" + templateId.ToString() + " created in DB");
                    return templateId;
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

        public MainElement      TemplateRead(int templateId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("templateId:" + templateId);

                    return sqlController.TemplateRead(templateId);
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

        public bool             TemplateDelete(int templateId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("templateId:" + templateId);

                    return sqlController.TemplateDelete(templateId);
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

        public Template_Dto     TemplateItemRead(int templateId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("templateId:" + templateId);

                    return sqlController.TemplateItemRead(templateId);
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

        public List<Template_Dto> TemplateItemReadAll(bool includeRemoved)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("includeRemoved:" + includeRemoved);

                    return sqlController.TemplateItemReadAll(includeRemoved);
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
            List<string> lst = CaseCreate(mainElement, caseUId, new List<int> { siteId }, "");
            return lst[0];
        }

        public List<string>     CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, string custom)
        {
            try
            {
                if (coreRunning)
                {
                    lock (_lockMain) //Will let sending Cases sending finish, before closing
                    {
                        string siteIdsStr = string.Join(",", siteIds);
                        TriggerLog("caseUId:" + caseUId + " siteIds:" + siteIdsStr + " custom:" + custom + ", requested to be created");

                        #region check input
                        DateTime start = DateTime.Parse(mainElement.StartDate.ToShortDateString());
                        DateTime end   = DateTime.Parse(mainElement.EndDate  .ToShortDateString());

                        if (end < DateTime.Now)
                            throw new ArgumentException("mainElement.EndDate needs to be a future date");

                        if (end <= start)
                            throw new ArgumentException("mainElement.StartDate needs to be at least the day, before the remove date (mainElement.EndDate)");

                        if (caseUId != "" && mainElement.Repeated != 1)
                            throw new ArgumentException("if caseUId can only be used for mainElement.Repeated == 1");
                        #endregion

                        //sending and getting a reply
                        List<string> lstMUId = new List<string>();

                        foreach (int siteId in siteIds)
                        {
                            string mUId = SendXml(mainElement, siteId);

                            if (mainElement.Repeated == 1)
                                sqlController.CaseCreate          (mainElement.Id, siteId, mUId, null, caseUId, custom, DateTime.Now);
                            else
                                sqlController.CheckListSitesCreate(mainElement.Id, siteId, mUId);

                            Case_Dto cDto = sqlController.CaseReadByMUId(mUId);
                            InteractionCaseUpdate(cDto);
                            HandleCaseCreated?.Invoke(cDto, EventArgs.Empty);
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
                        TriggerWarning("No case found with MuuId:'" + microtingUId + "'");
                        return null;
                    }
                    #endregion

                    int id = aCase.id;
                    TriggerLog("aCase.id:" + aCase.id.ToString() + ", found");

                    ReplyElement replyElement = sqlController.CheckRead(microtingUId, checkUId);
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

        public Case_Dto         CaseReadByCaseId(int id)
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

        public bool             CaseDelete(int templateId, int siteUId)
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

                    var cDto = sqlController.CaseReadByMUId(microtingUId);
                    string xmlResponse = communicator.Delete(microtingUId, cDto.SiteUId);

                    if (xmlResponse.Contains("Parsing in progress: Can not delete check list!</Value>"))
                        for (int i = 1; i < 7; i++)
                        {
                            Thread.Sleep(i * 200);
                            xmlResponse = communicator.Delete(microtingUId, cDto.SiteUId);
                            if (!xmlResponse.Contains("Parsing in progress: Can not delete check list!</Value>"))
                                break;
                        }

                    TriggerLog("XML response:");
                    TriggerLog(xmlResponse);

                    Response resp = new Response();
                    resp = resp.XmlToClass(xmlResponse);
                    if (resp.Type.ToString() == "Success")
                    {
                        try
                        {
                            sqlController.CaseDelete(microtingUId);

                            cDto = sqlController.CaseReadByMUId(microtingUId);
                            InteractionCaseUpdate(cDto);
                            HandleCaseDeleted?.Invoke(cDto, EventArgs.Empty);
                            TriggerMessage(cDto.ToString() + " has been removed");

                            return true;
                        }
                        catch { }

                        try
                        {
                            sqlController.CaseDeleteReversed(microtingUId);

                            cDto = sqlController.CaseReadByMUId(microtingUId);
                            InteractionCaseUpdate(cDto);
                            HandleCaseDeleted?.Invoke(cDto, EventArgs.Empty);
                            TriggerMessage(cDto.ToString() + " has been removed");

                            return true;
                        }
                        catch { }
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
                        TriggerWarning("No case found with MuuId:'" + microtingUId + "'");
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

        public string           CasesToExcel(int? templatId, DateTime? start, DateTime? end, string pathAndName, string customPathForUploadedData)
        {
            try
            {
                if (coreRunning)
                {
                    List<List<string>> dataSet = GenerateDataSetFromCases(templatId, start, end, customPathForUploadedData);

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

        public string           CasesToCsv(int? templatId, DateTime? start, DateTime? end, string pathAndName, string customPathForUploadedData)
        {
            try
            {
                if (coreRunning)
                {
                    List<List<string>> dataSet = GenerateDataSetFromCases(templatId, start, end, customPathForUploadedData);

                    if (dataSet == null)
                        return "";

                    List<string> temp;
                    string text = "";

                    for (int rowN = 0; rowN < dataSet[0].Count; rowN++)
                    {
                        temp = new List<string>();

                        foreach (List<string> lst in dataSet)
                        {
                            try
                            {
                                int.Parse(lst[rowN]);
                                temp.Add(lst[rowN]);
                            }
                            catch
                            {
                                try
                                {
                                    DateTime.Parse(lst[rowN]);
                                    temp.Add(lst[rowN]);
                                }
                                catch
                                {
                                    temp.Add("\"" + lst[rowN] + "\"");
                                }
                            }
                        }

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

        #region site
        public Site_Dto         SiteCreate(string name, string userFirstName, string userLastName, string userEmail)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("siteName:" + name + " / userFirstName:" + userFirstName + " / userLastName:" + userLastName + " / userEmail:" + userEmail);

                    Tuple<Site_Dto, Unit_Dto> siteResult = communicator.SiteCreate(name);

                    string token = sqlController.SettingRead(Settings.token);
                    int customerNo = communicator.OrganizationLoadAllFromRemote(token).CustomerNo;

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

                    Worker_Dto workerDto = Advanced_WorkerCreate(userFirstName, userLastName, userEmail);
                    Advanced_SiteWorkerCreate(siteDto, workerDto);

                    return SiteRead(siteId);
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

        public Site_Dto         SiteRead(int siteId)
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

        public List<Site_Dto>   SiteReadAll(bool includeRemoved)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    if (includeRemoved)
                        return Advanced_SiteReadAll(null, null, null);
                    else
                        return Advanced_SiteReadAll("not_removed", null, null);
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

        public Site_Dto         SiteReset(int siteId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("siteId:" + siteId);

                    Site_Dto site = SiteRead(siteId);
                    Advanced_UnitRequestOtp((int)site.UnitId);

                    return SiteRead(siteId);
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

        public bool             SiteUpdate(int siteId, string name, string userFirstName, string userLastName, string userEmail)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    Site_Dto siteDto = SiteRead(siteId);
                    Advanced_SiteItemUpdate(siteId, name);
                    Advanced_WorkerUpdate((int)siteDto.WorkerUid, userFirstName, userLastName, userEmail);
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

        public bool             SiteDelete(int siteId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    Site_Dto siteDto = SiteRead(siteId);
                    Advanced_SiteItemDelete(siteId);
                    Site_Worker_Dto siteWorkerDto = Advanced_SiteWorkerRead(null, siteId, siteDto.WorkerUid);
                    Advanced_SiteWorkerDelete(siteWorkerDto.MicrotingUId);
                    Advanced_WorkerDelete((int)siteDto.WorkerUid);
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
        #endregion

        #region entity
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

                    CoreHandleUpdateEntityItems();
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

                    CoreHandleUpdateEntityItems();
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

        public string           PdfUpload(string localPath)
        {
            try
            {
                if (coreRunning)
                {
                    string chechSum = "";
                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(localPath))
                        {
                            byte[] grr = md5.ComputeHash(stream);
                            chechSum = BitConverter.ToString(grr).Replace("-", "").ToLower();
                        }
                    }

                    if (communicator.PdfUpload(localPath, chechSum))
                        return chechSum;
                    else
                    {
                        TriggerWarning("Uploading of PDF failed");
                        return null;
                    }
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(t.GetMethodName() + " failed", ex, true);
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }
        #endregion
        #endregion

        #region public advanced actions
        #region sites
        public List<Site_Dto> Advanced_SiteReadAll(string workflowState, int? offSet, int? limit)
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

        public SiteName_Dto Advanced_SiteItemRead(int siteId)
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

        public List<SiteName_Dto> Advanced_SiteItemReadAll()
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

        public bool Advanced_SiteItemUpdate(int siteId, string name)
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

        public bool Advanced_SiteItemDelete(int siteId)
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
        public Worker_Dto Advanced_WorkerCreate(string firstName, string lastName, string email)
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

                    return Advanced_WorkerRead(workerUId);
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

        public Worker_Dto Advanced_WorkerRead(int workerId)
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

        public List<Worker_Dto> Advanced_WorkerReadAll(string workflowState, int? offSet, int? limit)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    return sqlController.WorkerGetAll(workflowState, offSet, limit);
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

        public bool Advanced_WorkerUpdate(int workerId, string firstName, string lastName, string email)
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

        public bool Advanced_WorkerDelete(int workerId)
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
        public Site_Worker_Dto Advanced_SiteWorkerCreate(SiteName_Dto siteDto, Worker_Dto workerDto)
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
                        sqlController.SiteWorkerCreate(result.MicrotingUId, siteDto.SiteUId, workerDto.WorkerUId);
                    }

                    return Advanced_SiteWorkerRead(result.WorkerUId, null, null);
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

        public Site_Worker_Dto Advanced_SiteWorkerRead(int? siteWorkerId, int? siteId, int? workerId)
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

        public bool Advanced_SiteWorkerDelete(int workerId)
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
        public Unit_Dto Advanced_UnitRead(int unitId)
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

        public List<Unit_Dto> Advanced_UnitReadAll()
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

        public Unit_Dto Advanced_UnitRequestOtp(int unitId)
        {
            string methodName = t.GetMethodName();
            try
            {
                if (coreRunning)
                {
                    TriggerLog(methodName + " called");
                    TriggerLog("unitId:" + unitId);

                    int otp_code = communicator.UnitRequestOtp(unitId);

                    Unit_Dto my_dto = Advanced_UnitRead(unitId);

                    sqlController.UnitUpdate(unitId, my_dto.CustomerNo, otp_code, my_dto.SiteUId);

                    return Advanced_UnitRead(unitId);
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

        #region interaction case
        public int              Advanced_InteractionCaseCreate(int templateId, string caseUId, List<int> siteUIds, string custom, bool connected, string replacements)
        {
            return sqlController.InteractionCaseCreate(templateId, caseUId, siteUIds, custom, connected, replacements);
        }

        public bool             Advanced_InteractionCaseDelete(int interactionCaseId)
        {
            return sqlController.InteractionCaseDelete(interactionCaseId);
        }
        #endregion

        public Field            FieldRead(int id)
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

        #region internal UnitTest
        internal void UnitTest_CaseComplet(string microtingUId, string checkUId)
        {
            sqlController.CaseRetract(microtingUId, checkUId);
            Case_Dto cDto = sqlController.CaseReadByMUId(microtingUId);
            InteractionCaseUpdate(cDto);
            HandleCaseCompleted?.Invoke(cDto, EventArgs.Empty);
            TriggerMessage(cDto.ToString() + " has been retrived");
        }

        internal void UnitTest_CaseDelete(string microtingUId)
        {
            Case_Dto cDto = sqlController.CaseReadByMUId(microtingUId);
            Case_Dto cDtoDel = new Case_Dto(cDto.CaseId, "Deleted", cDto.SiteUId, cDto.CaseType, cDto.CaseUId, cDto.MicrotingUId, cDto.CheckUId, cDto.Custom, cDto.CheckListId);

            InteractionCaseUpdate(cDtoDel);
            HandleCaseDeleted?.Invoke(cDtoDel, EventArgs.Empty);
            TriggerMessage(cDto.ToString() + " has been deleted");
        }

        internal void UnitTest_TriggerLog(string text)
        {
            TriggerLog(text);
        }
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

        private List<List<string>> GenerateDataSetFromCases (int? templateId, DateTime? start, DateTime? end, string customPathForUploadedData)
        {
            List<List<string>>  dataSet         = new List<List<string>>();
            List<string>        colume1CaseIds  = new List<string> { "Id" };
            List<int>           caseIds         = new List<int>();

            List<Case>         caseList        = sqlController.CaseReadAll(templateId, start, end, "not_removed");
            var                template        = sqlController.TemplateItemRead((int)templateId);

            if (caseList.Count == 0)
                return null;

            #region firstColumes generate
            {
                List<string> colume2 = new List<string> { "Date" };
                List<string> colume3 = new List<string> { "Time" };
                List<string> colume4 = new List<string> { "Day" };
                List<string> colume5 = new List<string> { "Week" };
                List<string> colume6 = new List<string> { "Month" };
                List<string> colume7 = new List<string> { "Year" };
                List<string> colume8 = new List<string> { "Created At" };
                List<string> colume9 = new List<string> { "Site" };
                List<string> colume10 = new List<string> { "Device User" };
                List<string> colume11 = new List<string> { "Device Id" };
                List<string> colume12 = new List<string> { "eForm Name" };

                var cal = DateTimeFormatInfo.CurrentInfo.Calendar;
                foreach (var aCase in caseList)
                {
                    DateTime time = (DateTime)aCase.DoneAt;
                    colume1CaseIds.Add(aCase.Id.ToString());
                    caseIds.Add(aCase.Id);

                    colume2.Add(time.ToString("yyyy.MM.dd"));
                    colume3.Add(time.ToString("HH:mm:ss"));
                    colume4.Add(time.DayOfWeek.ToString());
                    colume5.Add(time.Year.ToString() + "." + cal.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday));
                    colume6.Add(time.Year.ToString() + "." + time.ToString("MMMM").Substring(0, 3));
                    colume7.Add(time.Year.ToString());
                    colume8.Add(aCase.CreatedAt.Value.ToString("yyyy.MM.dd HH:mm:ss"));
                    colume9.Add(aCase.SiteName);
                    colume10.Add(aCase.WorkerName);
                    colume11.Add(aCase.UnitId.ToString());
                    colume12.Add(template.Label);
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
                dataSet.Add(colume11);
                dataSet.Add(colume12);
            }
            #endregion

            #region fieldValue generate
            {
                if (templateId != null)
                {
                    MainElement templateData = sqlController.TemplateRead((int)templateId);

                    List<string> lstReturn = new List<string>();
                    lstReturn = GenerateDataSetFromCasesSubSet(lstReturn, templateData.ElementList, "");

                    List<string> newRow;
                    foreach (string set in lstReturn)
                    {
                        int fieldId = int.Parse(t.SplitToList(set, 0, false));
                        string label = t.SplitToList(set, 1, false);

                        List<List<string>> result = sqlController.FieldValueReadAllValues(fieldId, caseIds, customPathForUploadedData);

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

                    if (preLabel != "")
                        preLabel = preLabel + sep + dataE.Label;

                    foreach (FieldGroup dataItemGroup in dataE.DataItemGroupList)
                    {
                        foreach (DataItem dataItem in dataItemGroup.DataItemList)
                        {
                            if (dataItem.GetType() == typeof(SaveButton))
                                continue;
                            if (dataItem.GetType() == typeof(None))
                                continue;

                            if (preLabel != "")
                                lstReturn.Add(dataItem.Id + "|" + preLabel.Remove(0, sep.Length) + sep + dataItemGroup.Label + sep + dataItem.Label);
                            else
                                lstReturn.Add(dataItem.Id + "|" + dataItemGroup.Label + sep + dataItem.Label);
                        }
                    }

                    foreach (DataItem dataItem in dataE.DataItemList)
                    {
                        if (dataItem.GetType() == typeof(SaveButton))
                            continue;
                        if (dataItem.GetType() == typeof(None))
                            continue;

                        if (preLabel != "")
                            lstReturn.Add(dataItem.Id + "|" + preLabel.Remove(0, sep.Length) + sep + dataItem.Label);
                        else
                            lstReturn.Add(dataItem.Id + "|" + dataItem.Label);
                    }
                }
                #endregion

                #region if GroupElement
                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;

                    if (preLabel != "")
                        GenerateDataSetFromCasesSubSet(lstReturn, groupE.ElementList, preLabel + sep + groupE.Label);
                    else
                        GenerateDataSetFromCasesSubSet(lstReturn, groupE.ElementList, groupE.Label);
                }
                #endregion
            }

            return lstReturn;
        }

        private List<string>    PdfValidate(string pdfString, int pdfId)
        {
            List<string> errorLst = new List<string>();

            if (pdfString.ToLower().Contains("microting.com"))
                errorLst.Add("Element showPdf.Id:'" + pdfId + "' contains an URL that points to Microting's builder temporary hosting. Indicating that it's not a proper hash");
            if (pdfString.ToLower().Contains("http") || pdfString.ToLower().Contains("https"))
                errorLst.Add("Element showPdf.Id:'" + pdfId + "' contains an HTTP or HTTPS. Indicating that it's not a proper hash");
            if (pdfString.Length != 32)
                errorLst.Add("Element showPdf.Id:'" + pdfId + "' lenght is not the correct lenght (32). Indicating that it's not a proper hash");

            if (errorLst.Count > 0)
                errorLst.Add("Element showPdf.Id:'" + pdfId + "' please check 'value' input, and consider running PdfUpload");

            return errorLst;
        }

        private void            InteractionCaseUpdate(Case_Dto caseDto)
        {
            try
            {
                if (!sqlController.InteractionCaseUpdate(caseDto))
                    TriggerWarning(t.GetMethodName() + " failed, for:'" + caseDto.ToString() + "', reason due to no matching case");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection(t.GetMethodName() + " failed, for:'" + caseDto.ToString() + "'", ex, true);
            }
        }
        #endregion

        #region inward Event handlers
        private void    CoreThread()
        {
            coreRunning = true;
            coreThreadAlive = true;

            while (coreThreadAlive)
            {
                try
                {
                    if (coreRunning)
                    {
                        TriggerLog(t.GetMethodName() + " initiated");

                        Thread updateFilesThread            
                            = new Thread(() => CoreHandleUpdateFiles());
                            updateFilesThread.Start();

                        Thread updateNotificationsThread    
                            = new Thread(() => CoreHandleUpdateNotifications());
                            updateNotificationsThread.Start();

                        Thread updateTablesThread
                            = new Thread(() => CoreHandleUpdateTables());
                            updateTablesThread.Start();

                        Thread.Sleep(2000);
                    }

                    Thread.Sleep(500);
                }
                catch (ThreadAbortException) {
                    TriggerWarning(t.GetMethodName() + " catch of ThreadAbortException");
                }
                catch (Exception ex) {
                    throw new Exception("FATAL Exception. " + t.GetMethodName() + " failed", ex);
                }
            }

            coreRunning = false;
            coreStatChanging = false;
        }

        private void    CoreHandleUpdateFiles()
        {
            try
            {
                if (!updateIsRunningFiles)
                {
                    updateIsRunningFiles = true;

                    #region update files
                    UploadedData ud = null;
                    string urlStr = "";
                    bool oneFound = true;
                    while (oneFound)
                    {
                        ud = sqlController.FileRead();
                        if (ud != null)
                            urlStr = ud.FileLocation;
                        else
                            break;
                        
                        #region finding file name and creating folder if needed
                        FileInfo file = new FileInfo(fileLocationPicture);
                        file.Directory.Create(); // If the directory already exists, this method does nothing.

                        int index = urlStr.LastIndexOf("/") + 1;
                        string fileName = ud.Id.ToString() + "_" + urlStr.Remove(0, index);
                        #endregion

                        #region download file
                        using (var client = new WebClient())
                        {
                            try
                            {
                                client.DownloadFile(urlStr, fileLocationPicture + fileName);
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
                            using (var stream = File.OpenRead(fileLocationPicture + fileName))
                            {
                                byte[] grr = md5.ComputeHash(stream);
                                chechSum = BitConverter.ToString(grr).Replace("-", "").ToLower();
                            }
                        }
                        #endregion

                        #region checks checkSum
                        if (chechSum != fileName.Substring(fileName.LastIndexOf(".")-32, 32))
                            TriggerWarning("Download of '" + urlStr + "' failed. Check sum did not match");
                        #endregion

                        Case_Dto dto = sqlController.FileCaseFindMUId(urlStr);
                        File_Dto fDto = new File_Dto(dto.SiteUId, dto.CaseType, dto.CaseUId, dto.MicrotingUId, dto.CheckUId, fileLocationPicture + fileName);
                        HandleFileDownloaded?.Invoke(fDto, EventArgs.Empty);
                        TriggerMessage("Downloaded file '" + urlStr + "'.");

                        sqlController.FileProcessed(urlStr, chechSum, fileLocationPicture, fileName, ud.Id);
                    }
                    #endregion

                    updateIsRunningFiles = false;
                }
            }
            catch (Exception ex)
            {
                updateIsRunningFiles = false;
                TriggerHandleExpection(t.GetMethodName() + " failed", ex, true);
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
                    Note_Dto notification;
                    string noteUId;
                    bool oneFound = true;
                    while (oneFound)
                    {
                        notification = sqlController.NotificationReadFirst();
                        #region if no new notification found - stops method
                        if (notification == null)
                        {
                            oneFound = false;
                            break;
                        }
                        #endregion

                        noteUId = notification.MicrotingUId;

                        try
                        {
                            switch (notification.Activity)
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
                                                resp = resp.XmlToClassUsingXmlDocument(respXml);
                                                //resp = resp.XmlToClass(respXml);

                                                if (resp.Type == Response.ResponseTypes.Success)
                                                {
                                                    if (resp.Checks.Count > 0)
                                                    {
                                                        XmlDocument xDoc = new XmlDocument();

                                                        xDoc.LoadXml(respXml);
                                                        XmlNode checks = xDoc.DocumentElement.LastChild;
                                                        int i = 0;
                                                        foreach (Check check in resp.Checks)
                                                        {
                                                            sqlController.ChecksCreate(resp, checks.ChildNodes[i].OuterXml.ToString(), i);

                                                            int unitUId = sqlController.UnitRead(int.Parse(check.UnitId)).UnitUId;
                                                            int workerUId = sqlController.WorkerRead(int.Parse(check.WorkerId)).WorkerUId;
                                                            sqlController.CaseUpdateCompleted(noteUId, check.Id, DateTime.Parse(check.Date), workerUId, unitUId);

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

                                                            sqlController.CaseRetract(noteUId, check.Id);

                                                            Case_Dto cDto = sqlController.CaseReadByMUId(noteUId);
                                                            InteractionCaseUpdate(cDto);
                                                            HandleCaseCompleted?.Invoke(cDto, EventArgs.Empty);
                                                            TriggerMessage(cDto.ToString() + " has been completed");
                                                            i++;
                                                        }
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

                                        sqlController.NotificationProcessed(notification.Id, "processed");
                                        break;
                                    }
                                #endregion

                                #region unit fetch / checklist retrieve by device
                                case "unit_fetch":
                                    {
                                        sqlController.CaseUpdateRetrived(noteUId);
                                        Case_Dto cDto = sqlController.CaseReadByMUId(noteUId);
                                        InteractionCaseUpdate(cDto);
                                        HandleCaseRetrived?.Invoke(cDto, EventArgs.Empty);
                                        TriggerMessage(cDto.ToString() + " has been retrived");

                                        sqlController.NotificationProcessed(notification.Id, "processed");
                                        break;
                                    }
                                #endregion

                                #region unit_activate / tablet added
                                case "unit_activate":
                                    {
                                        HandleSiteActivated?.Invoke(noteUId, EventArgs.Empty);
                                        TriggerMessage(noteUId + " has been added");
                                        try
                                        {
                                            Unit_Dto unitDto = sqlController.UnitRead(int.Parse(noteUId));
                                            sqlController.UnitUpdate(unitDto.UnitUId, unitDto.CustomerNo, 0, unitDto.SiteUId);
                                            sqlController.NotificationProcessed(notification.Id, "processed");
                                        } catch
                                        {
                                            sqlController.NotificationProcessed(notification.Id, "processed");
                                        }
                                        break;
                                    }
                                #endregion

                                default:
                                    throw new IndexOutOfRangeException("Notification activity '" + notification.Activity + "' is not known or mapped");
                            }
                        }
                        catch (Exception ex)
                        {
                            TriggerWarning("CoreHandleUpdateNotifications failed. Case:'" + noteUId + "' marked as 'not_found'. " + ex.Message);
                            sqlController.NotificationProcessed(notification.Id, "not_found");
                        }
                    }
                    #endregion

                    updateIsRunningNotifications = false;
                }
            }
            catch (Exception ex)
            {
                updateIsRunningNotifications = false;
                TriggerHandleExpection(t.GetMethodName() + " failed", ex, true);
            }
        }

        private void    CoreHandleUpdateTables()
        {
            try
            {
                if (!updateIsRunningTables)
                {
                    updateIsRunningTables = true;

                    #region update tables
                    bool oneFound = true;

                    while (oneFound)
                    {
                        oneFound = false;
                        #region check if out of sync

                            //a_interaction_template
                        #region TemplateCreate
                        //if (!oneFound)
                        //{
                        //    if (null != null)
                        //    {
                        //        oneFound = true;
                        //    }
                        //}
                        #endregion

                            //a_interaction_case
                        #region CaseCreate
                        if (!oneFound)
                        {
                            a_interaction_cases iC = sqlController.InteractionCaseReadFirstCreate();
     
                            try
                            {
                                if (iC != null)
                                {
                                    oneFound = true;
                                    List<a_interaction_case_lists> caseList = sqlController.InteractionCaseListRead(iC.id);
                                    List<int> siteIds = new List<int>();

                                    foreach (var item in caseList)
                                        siteIds.Add((int)item.siteId);

                                    MainElement mainElement = sqlController.TemplateRead(iC.template_id);
                                    //do magic - replacement TODO

                                    List<string> lstMUIds = new List<string>();

                                    if (t.Bool(iC.connected))
                                    {
                                        lstMUIds = CaseCreate(mainElement, iC.case_uid, siteIds, iC.custom);
                                    }
                                    else
                                    {
                                        foreach (var site in siteIds)
                                            lstMUIds.AddRange(CaseCreate(mainElement, iC.case_uid, new List<int> { site }, iC.custom));
                                    }

                                    sqlController.InteractionCaseProcessedCreate(iC.id, siteIds, lstMUIds);
                                }
                            }
                            catch (Exception ex)
                            {
                                sqlController.InteractionCaseFailed(iC.id, t.PrintException(t.GetMethodName() + " failed. CaseCreate logic failed", ex));
                            }
                        }
                        #endregion

                        #region CaseDelete
                        if (!oneFound)
                        {
                            a_interaction_cases iC = sqlController.InteractionCaseReadFirstDelete();

                            try
                            {
                                if (iC != null)
                                {
                                    oneFound = true;
                                    int found = 0;

                                    List<a_interaction_case_lists> caseList = sqlController.InteractionCaseListRead(iC.id);
                                    foreach (var item in caseList)
                                    {
                                        found += 1 - t.Bool(CaseDelete(item.microting_uid));
                                    }

                                    if (found == 0)
                                        sqlController.InteractionCaseProcessedDelete(iC.id);
                                    else
                                        sqlController.InteractionCaseFailed(iC.id, t.GetMethodName() + " failed. CaseDelete failed. " + found + " eForms not deleted");
                                }
                            }
                            catch (Exception ex)
                            {
                                sqlController.InteractionCaseFailed(iC.id, t.PrintException(t.GetMethodName() + " failed. CaseDelete logic failed", ex));
                            }
                        }
                        #endregion

                        #endregion
                    }
                    #endregion

                    updateIsRunningTables = false;
                }
            }
            catch (Exception ex)
            {
                updateIsRunningTables = false;
                TriggerHandleExpection(t.GetMethodName()+  " failed", ex, true);
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
                updateIsRunningEntities = false;
                TriggerHandleExpection("CoreHandleUpdateEntityItems failed", ex, true);
            }
        }
        #endregion

        #region outward Event triggers
        private void    TriggerLog(string str)
        {
            if (logLevel)
            {
                HandleEventLog?.Invoke(DateTime.Now.ToLongTimeString() + ":" + str, EventArgs.Empty);
            }
        }

        private void    TriggerMessage(string str)
        {
            TriggerLog(str);
            HandleEventMessage?.Invoke(str, EventArgs.Empty);
        }

        private void    TriggerWarning(string str)
        {
            TriggerLog(str);
            HandleEventWarning?.Invoke(str, EventArgs.Empty);
        }

        private void    TriggerHandleExpection(string exceptionDescription, Exception ex, bool restartCore)
        {
            try
            {
                HandleEventException?.Invoke(ex, EventArgs.Empty);

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
                    Start(connectionString);

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
}