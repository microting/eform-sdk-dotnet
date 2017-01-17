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
using eFormRequest;
using eFormResponse;
using eFormSubscriber;
using eFormSqlController;
using Trools;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;
using System.Security.Cryptography;
using System.Linq;

namespace Microting
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
        string organizationId;
        string subscriberToken;
        string subscriberAddress;
        string subscriberName;

        string serverConnectionString;
        string fileLocation;
        bool logEvents;
        #endregion

        #region con
        public Core(string comToken, string comAddress, string organizationId, string subscriberToken, string subscriberAddress, string subscriberName, string serverConnectionString, string fileLocation, bool logEvents)
        {
            if (string.IsNullOrEmpty(comToken))
                throw new ArgumentException("comToken is not allowed to be null or empty");

            if (string.IsNullOrEmpty(comAddress))
                throw new ArgumentException("comAddress is not allowed to be null or empty");

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentException("organizationId is not allowed to be null or empty");

            if (string.IsNullOrEmpty(subscriberToken))
                throw new ArgumentException("subscriberToken is not allowed to be null or empty");

            if (string.IsNullOrEmpty(subscriberAddress))
                throw new ArgumentException("subscriberAddress is not allowed to be null or empty");

            if (string.IsNullOrEmpty(subscriberName))
                throw new ArgumentException("subscriberName is not allowed to be null or empty");
            if (subscriberName.Contains(" ") || subscriberName.Contains("æ") || subscriberName.Contains("ø") || subscriberName.Contains("å"))
                throw new ArgumentException("subscriberName is not allowed to contain blank spaces ' ', æ, ø and å");

            if (string.IsNullOrEmpty(serverConnectionString))
                throw new ArgumentException("serverConnectionString is not allowed to be null or empty");

            if (string.IsNullOrEmpty(fileLocation))
                throw new ArgumentException("fileLocation is not allowed to be null or empty");

            this.comToken = comToken;
            this.comAddress = comAddress;
            this.organizationId = organizationId;
            this.subscriberToken = subscriberToken;
            this.subscriberAddress = subscriberAddress;
            this.subscriberName = subscriberName;
            this.serverConnectionString = serverConnectionString;
            this.fileLocation = fileLocation;
            this.logEvents = logEvents;
        }
        #endregion

        #region public state
        public void     Start()
        {
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
                    TriggerLog("comToken:" + comToken + " comAddress: " + comAddress + " subscriberToken:" + subscriberToken + " subscriberAddress:" + subscriberAddress +
                        " subscriberName:" + subscriberName + " serverConnectionString:" + serverConnectionString + " fileLocation:" + fileLocation + " logEvents:" + logEvents.ToString());
                    TriggerLog("Controller started");


                    //sqlController
                    sqlController = new SqlController(serverConnectionString);
                    TriggerLog("SqlEformController started");


                    //communicators
                    communicator = new Communicator(comAddress, comToken, organizationId);
                    communicator.EventLog += CoreHandleEventLog;
                    TriggerLog("Communicator started");


                    //subscriber
                    subscriber = new Subscriber(subscriberToken, subscriberAddress, subscriberName);
                    TriggerLog("Subscriber created");
                    subscriber.EventMsgClient += CoreHandleEventClient;
                    subscriber.EventMsgServer += CoreHandleEventServer;
                    TriggerLog("Subscriber now triggers events");
                    subscriber.Start();

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
        }

        public void     Close()
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
        }

        public bool     Running()
        {
            return coreRunning;
        }
        #endregion

        #region public actions
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

        public void             CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds)
        {
            try
            {
                if (coreRunning)
                {
                    string siteIdsStr = string.Join(",", siteIds);
                    TriggerLog("siteIds:" + siteIdsStr + ", requested to be created");

                    Thread subscriberThread = new Thread(() => CaseCreateMethodThreaded(mainElement, caseUId, siteIds, "", false));
                    subscriberThread.Start();
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

        public void             CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, string custom, bool reversed)
        {
            try
            {
                if (coreRunning)
                {
                    string siteIdsStr = string.Join(",", siteIds);
                    TriggerLog("caseUId:" + caseUId + " siteIds:" + siteIdsStr + " custom:" + custom + " reversed:" + reversed.ToString() + ", requested to be created");

                    Thread caseThread = new Thread(() => CaseCreateMethodThreaded(mainElement, caseUId, siteIds, custom, reversed));
                    caseThread.Start();
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

                    List<Answer> lstAnswers = new List<Answer>();
                    List<field_values> lstReplies = sqlController.ChecksRead(microtingUId);
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
                        Answer answer = sqlController.ChecksReadAnswer(reply.id);
                        lstAnswers.Add(answer);
                    }
                    TriggerLog("Questons and answers found");

                    //replace DataItem(s) with DataItem(s) Answer
                    ReplaceDataItemsInElementsWithAnswers(replyElement.ElementList, lstAnswers);

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

        public ReplyElement     CaseReadAllSites(string caseUId)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("caseUId:" + caseUId + ", requested to be read");

                    ReplyElement replyElement;

                    string mUID = sqlController.CaseFindActive(caseUId);
                    TriggerLog("mUID:" + mUID + ", found");

                    if (mUID == "")
                        return null;

                    replyElement = CaseRead(mUID, null);

                    return replyElement;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseReadAllSites failed", ex, true);
                throw new Exception("CaseReadAllSites failed", ex);
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
                    string xmlResponse = communicator.Delete(microtingUId, aCase.SiteId);

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

        public int              CaseDeleteAllSites(string caseUId)
        {
            try
            {
                if (coreRunning)
                {
                    TriggerLog("caseUId:" + caseUId + ", requested to be deleted");
                    int deleted = 0;

                    List<Case_Dto> lst = sqlController.CaseReadByCaseUId(caseUId);
                    TriggerLog("lst.Count:" + lst.Count + ", found");

                    foreach (var item in lst)
                    {
                        Response resp = new Response();
                        resp = resp.XmlToClass(communicator.Delete(item.MicrotingUId, item.SiteId));

                        if (resp.Value == "success")
                            deleted++;
                    }
                    TriggerLog("deleted:" + deleted.ToString());

                    return deleted;
                }
                else
                    throw new Exception("Core is not running");
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseDeleteAllSites failed", ex, true);
                throw new Exception("CaseDeleteAllSites failed", ex);
            }
        }

        public Case_Dto         CaseLookup(string microtingUId)
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
                TriggerHandleExpection("CaseLookup failed", ex, true);
                throw new Exception("CaseLookup failed", ex);
            }
        }

        public List<Case_Dto>   CaseLookupAllSites(string caseUId)
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
                TriggerHandleExpection("CaseLookupAllSites failed", ex, true);
                throw new Exception("CaseLookupAllSites failed", ex);
            }
        }

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

        public void             EntityGroupUpdate(EntityGroup entityGroup)
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
        }

        public void             EntityGroupDelete(string entityGroupMUId)
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
        }
        #endregion

        #region private
        private void    CaseCreateMethodThreaded(MainElement mainElement, string caseUId, List<int> siteIds, string custom, bool reversed)
        {
            try //Threaded method, hence the try/catch
            {
                lock (_lockMain)
                {
                    if (mainElement.Repeated != 1 && reversed == false)
                        throw new ArgumentException("mainElement.Repeat was not equal to 1 & reversed is false. Hence no case can be created");

                    DateTime start = DateTime.Parse(mainElement.StartDate);
                    start = DateTime.Parse(start.ToShortTimeString());

                    DateTime end = DateTime.Parse(mainElement.EndDate);
                    start = DateTime.Parse(start.ToShortTimeString());

                    if (end < DateTime.Now)
                        throw new ArgumentException("mainElement.EndDate needs to be a future date");

                    if (end <= start)
                        throw new ArgumentException("mainElement.StartDate needs to be at least the day, before the remove date (mainElement.EndDate)");


                    //sending and getting a reply
                    bool found = false;
                    foreach (int siteId in siteIds)
                    {
                        string mUId = SendXml(mainElement, siteId);

                        if (reversed == false)
                            sqlController.CaseCreate(mUId, mainElement.Id, siteId, caseUId, custom);
                        else
                            sqlController.CheckListSitesCreate(mainElement.Id, siteId, mUId);

                        Case_Dto cDto = sqlController.CaseReadByMUId(mUId);
                        HandleCaseCreated(cDto, EventArgs.Empty);
                        TriggerMessage(cDto.ToString() + " has been created");

                        found = true;
                    }

                    if (!found)
                        throw new Exception("CaseCreateFullMethod failed. No matching sites found. No eForms created");

                    TriggerMessage("eForm created");
                }
            }
            catch (Exception ex)
            {
                TriggerHandleExpection("CaseCreateMethodThreaded failed", ex, true);
            }
        }

        private void    ReplaceDataItemsInElementsWithAnswers(List<Element> elementList, List<Answer> lstAnswers)
        {
            foreach (Element element in elementList)
            {
                #region DataElement
                if (element.GetType() == typeof(DataElement))
                {
                    DataElement dataE = (DataElement)element;

                    //replace DataItemGroups
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

                    //replace DataItems
                    List<eFormRequest.DataItem> dataItemListTemp2 = new List<eFormRequest.DataItem>();
                    foreach (var dataItem in dataE.DataItemList)
                    {
                        foreach (var answer in lstAnswers)
                        {
                            if (dataItem.Id == answer.Id)
                            {
                                dataItemListTemp2.Add(answer);
                                break;
                            }
                        }
                    }
                    dataE.DataItemList = dataItemListTemp2;
                }
                #endregion

                #region GroupElement
                if (element.GetType() == typeof(GroupElement))
                {
                    GroupElement groupE = (GroupElement)element;

                    ReplaceDataItemsInElementsWithAnswers(groupE.ElementList, lstAnswers);
                }
                #endregion
            }
        }

        private string  SendXml(MainElement mainElement, int siteId)
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
                        File_Dto fDto = new File_Dto(dto.SiteId, dto.CaseType, dto.CaseUId, dto.MicrotingUId, dto.CheckUId, fileLocation + fileName);
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
                    string notificationStr, noteUId, noteType = "";
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
                                            if (aCase.SiteId == concreteCase.SiteId)
                                            {
                                                #region get response's data and update DB with data
                                                string lastId = sqlController.CaseReadCheckIdByMUId(noteUId);
                                                string respXml;

                                                if (lastId == null)
                                                    respXml = communicator.Retrieve(noteUId, concreteCase.SiteId);
                                                else
                                                    respXml = communicator.RetrieveFromId(noteUId, concreteCase.SiteId, lastId);

                                                Response resp = new Response();
                                                resp = resp.XmlToClass(respXml);

                                                if (resp.Type == Response.ResponseTypes.Success)
                                                {
                                                    if (resp.Checks.Count > 0)
                                                    {
                                                        sqlController.ChecksCreate(resp, respXml);

                                                        if (lastId == null)
                                                        {
                                                            sqlController.CaseUpdate(noteUId, DateTime.Parse(resp.Checks[0].Date), int.Parse(resp.Checks[0].WorkerId), null, int.Parse(resp.Checks[0].UnitId));

                                                            #region retract case, thereby completing the process

                                                            string responseRetractionXml = communicator.Delete(aCase.MicrotingUId, aCase.SiteId);
                                                            Response respRet = new Response();
                                                            respRet = respRet.XmlToClass(respXml);

                                                            if (respRet.Type == Response.ResponseTypes.Success)
                                                            {
                                                                sqlController.CaseRetract(aCase.MicrotingUId);
                                                                TriggerLog(aCase.ToString() + " has been retracted");
                                                            }
                                                            else
                                                                TriggerWarning("Failed to retract eForm MicrotingUId:" + aCase.MicrotingUId + "/SideId:" + aCase.SiteId + ". Not a critical issue, but needs to be fixed if repeated");
                                                            #endregion
                                                        }
                                                        else
                                                            sqlController.CaseUpdate(noteUId, DateTime.Parse(resp.Checks[0].Date), int.Parse(resp.Checks[0].WorkerId), resp.Checks[0].Id, int.Parse(resp.Checks[0].UnitId));

                                                        Case_Dto cDto = sqlController.CaseReadByMUId(noteUId);
                                                        HandleCaseCompleted(cDto, EventArgs.Empty);
                                                        TriggerMessage(cDto.ToString() + " has been completed");
                                                    }
                                                }
                                                else
                                                    throw new Exception("Failed to retrive eForm " + noteUId + " from site " + aCase.SiteId);
                                                #endregion
                                            }
                                            else
                                            {
                                                #region delete eForm on other tablets and update DB to "deleted"

                                                string respXml = communicator.Delete(aCase.MicrotingUId, aCase.SiteId);
                                                Response resp = new Response();
                                                resp = resp.XmlToClass(respXml);

                                                if (resp.Type == Response.ResponseTypes.Success)
                                                    sqlController.CaseDelete(aCase.MicrotingUId);
                                                else
                                                    TriggerWarning("Failed to delete eForm MicrotingUId:" + aCase.MicrotingUId + "/SideId:" + aCase.SiteId + ". Not a critical issue, but needs to be fixed if repeated");

                                                HandleCaseDeleted(aCase, EventArgs.Empty);
                                                TriggerMessage(aCase.ToString() + " has been removed");

                                                #endregion
                                            }
                                        }

                                        sqlController.NotificationProcessed(notificationStr, "processed");
                                        break;
                                    }
                                #endregion

                                #region unit fetch / checklist retrieve by device
                                case "unit_fetch":
                                    {
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
                        catch
                        {
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
                                            string microtingUId = communicator.EntitySelectItemCreate(eI.entity_group_id.ToString(), eI.name, eI.description, eI.entity_item_uid);

                                            if (microtingUId != null)
                                            {
                                                sqlController.EntityItemSyncedProcessed(eI.entity_group_id, eI.entity_item_uid, microtingUId, "created");
                                                continue;
                                            }
                                        }

                                        if (eI.workflow_state == "updated")
                                        {
                                            if (communicator.EntitySelectItemUpdate(eI.entity_group_id.ToString(), eI.microting_uid, eI.name, eI.description, eI.entity_item_uid))
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
            if (logEvents)
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

                if (exceptionDescription == null)
                    exceptionDescription = "";

                exceptionDescription =
                "" + Environment.NewLine +
                "######## " + exceptionDescription + Environment.NewLine +
                "######## EXCEPTION FOUND; BEGIN ########" + Environment.NewLine +
                PrintException(ex, 1) + Environment.NewLine +
                "######## EXCEPTION FOUND; ENDED ########" + Environment.NewLine +
                "";

                TriggerMessage (exceptionDescription);
                ExceptionClass exCls = new ExceptionClass(exceptionDescription, DateTime.Now);
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

        private string      PrintException(Exception ex, int level)
        {
            if (ex == null)
                return "";

            return 
            "######## -Expection at level " + level + "- ########" + Environment.NewLine +
            "Message    :" + ex.Message + Environment.NewLine +
            "Source     :" + ex.Source + Environment.NewLine +
            "StackTrace :" + ex.StackTrace + Environment.NewLine + 
            PrintException(ex.InnerException, level + 1).TrimEnd();
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
            if (count == 2)
                secondsDelay = 6;

            if (count == 3)
                secondsDelay = 60;

            if (count == 4)
                secondsDelay = 600;

            if (count > 4)
                throw new Exception("FATAL Exception. Same Exception repeated to many times within one hour");

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
                    Start();

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