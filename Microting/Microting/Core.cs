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

namespace Microting
{
    class Core : ICore
    {
        #region events
        public event EventHandler HandleCaseCreated;
        public event EventHandler HandleCaseRetrived;
        public event EventHandler HandleCaseUpdated;
        public event EventHandler HandleCaseDeleted;
        public event EventHandler HandleFileDownloaded;
        public event EventHandler HandleSiteActivated;

        public event EventHandler HandleEventLog;
        public event EventHandler HandleEventWarning;
        public event EventHandler HandleEventException;
        #endregion

        #region var
        Communicator communicator;
        SqlController sqlController;
        Subscriber subscriber;
        Tools t = new Tools();
        object _lockMain = new object();
        object _lockEvent = new object();
        object _lockEventReply = new object();
        bool updateIsRunningFiles = true;
        bool updateIsRunningNotifications = true;

        string comToken;
        string comAddress;
        string subscriberToken;
        string subscriberAddress;
        string subscriberName;
        string serverConnectionString;
        int userId;
        string fileLocation;
        #endregion

        #region con
        public Core(string comToken, string comAddress, string subscriberToken, string subscriberAddress, string subscriberName, string serverConnectionString, int userId, string fileLocation)
        {
            this.comToken = comToken;
            this.comAddress = comAddress;
            this.subscriberToken = subscriberToken;
            this.subscriberAddress = subscriberAddress;
            this.subscriberName = subscriberName;
            this.serverConnectionString = serverConnectionString;
            this.userId = userId;
            this.fileLocation = fileLocation;
        }
        #endregion

        #region public
        public void             Start()
        {
            try
            {
                HandleEvent("Controller started", null);
                

                //sqlController
                sqlController = new SqlController(serverConnectionString, userId);
                HandleEvent("SqlEformController started", null);


                //communicators
                communicator = new Communicator(comToken, comAddress);
                HandleEvent("Transmitter started", null);


                //subscriber
                subscriber = new Subscriber(subscriberToken, subscriberAddress, subscriberName);
                HandleEvent("Subscriber created", null);
                subscriber.EventMsgClient += HandleEvent;
                subscriber.EventMsgServer += HandleEventReply;
                HandleEvent("Subscriber now triggers events", null);
                subscriber.Start();
            }
            catch (Exception ex)
            {
                HandleEventWarning("Core failed", EventArgs.Empty);
                throw new Exception("Core.Start failed.", ex);
            }
        }

        public void             Close()
        {
            lock (_lockMain)
            {
                try
                {
                    subscriber.Close(true);
                }
                catch { }

                try
                {
                    subscriber.EventMsgClient -= HandleEvent;
                    subscriber.EventMsgServer -= HandleEventReply;
                }
                catch { }

                subscriber = null;

                HandleEvent("Subscriber no longer triggers events", null);
                HandleEvent("Controller closed", null);
                HandleEvent("", null);

                communicator = null;
                sqlController = null;
            }
        }

        public int              TemplatCreate(string xmlString)
        {
            //XML HACK
            #region correct xml if needed
            xmlString = xmlString.Replace("<Main>", "<Main xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            xmlString = xmlString.Replace("<Element type=", "<Element xsi:type=");
            xmlString = xmlString.Replace("<DataItem type=", "<DataItem xsi:type=");
            xmlString = xmlString.Replace("<DataItemGroup type=", "<DataItemGroup xsi:type=");

            xmlString = xmlString.Replace("\"MultiSelect\">", "\"Multi_Select\">");
            xmlString = xmlString.Replace("\"SingleSelect\">", "\"Multi_Select\">");
            xmlString = xmlString.Replace("FolderName", "CheckListFolderName");

            xmlString = xmlString.Replace("FolderName", "CheckListFolderName");
            xmlString = xmlString.Replace("FolderName", "CheckListFolderName");

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

            //XML HACK
            mainElement.PushMessageTitle = "";
            mainElement.PushMessageBody = "";
            if (mainElement.Repeated == 0 || mainElement.Repeated == -1)
                mainElement.Repeated = 1;
     
            return sqlController.TemplatCreate(mainElement);
        }

        public int              TemplatCreate(MainElement mainElement)
        {
            return sqlController.TemplatCreate(mainElement);
        }

        public MainElement      TemplatRead(int templatId)
        {
            return sqlController.TemplatRead(templatId);
        }

        public void             CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, bool reversed)
        {
            Thread subscriberThread = new Thread(() => CaseCreateThread(mainElement, siteIds, caseUId, DateTime.MinValue, "", "", reversed));
            subscriberThread.Start();
        }

        public void             CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, bool reversed, DateTime navisionTime, string numberPlate, string roadNumber)
        {
            Thread subscriberThread = new Thread(() => CaseCreateThread(mainElement, siteIds, caseUId, navisionTime, numberPlate, roadNumber, reversed));
            subscriberThread.Start();
        }

        public ReplyElement     CaseRead(string microtingUId)
        {
            cases aCase = sqlController.CaseReadFull(microtingUId);
            #region handling if no match case found
            if (aCase == null)
            {
                HandleEventWarning("No case found with MuuId:'" + microtingUId + "'", EventArgs.Empty);
                return null;
            }
            #endregion
            int id = aCase.id;

            ReplyElement replyElement = sqlController.TemplatRead((int)aCase.check_list_id);

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

            //replace DataItem(s) with DataItem(s) Answer
            ReplaceDataItemsInElements(replyElement.ElementList, lstAnswers);

            return replyElement;
        }

        public ReplyElement     CaseReadAllSites(string caseUId)
        {
            ReplyElement replyElement;

            string muuId = sqlController.CaseFindActive(caseUId);

            if (muuId == "")
                return null;

            replyElement = CaseRead(muuId);

            return replyElement;
            ;
        }

        public bool             CaseDelete(string microtingUId)
        {
            int siteId = -1;

            var lst = sqlController.CaseFindMatchs(microtingUId);
            foreach (var item in lst)
            {
                if (item.MicrotingUId == microtingUId)
                    siteId = item.SiteId;
            }

            Response resp = new Response();
            resp = resp.XmlToClass(communicator.Delete(microtingUId, siteId));

            if (resp.Value == "success")
            {
                sqlController.CaseDelete(microtingUId);
                return true;
            }
            return false;
        }

        public int              CaseDeleteAllSites(string caseUId)
        {
            int deleted = 0;

            List<Case_Dto> lst = sqlController.CaseReadByCaseUId(caseUId);
            foreach (var item in lst)
            {
                Response resp = new Response();
                resp = resp.XmlToClass(communicator.Delete(item.MicrotingUId, item.SiteId));

                if (resp.Value == "success")
                    deleted++;
            }

            return deleted;
        }

        public Case_Dto         CaseLookup(string microtingUId)
        {
            return sqlController.CaseReadByMUId(microtingUId);
        }

        public List<Case_Dto>   CaseLookupAllSites(string caseUId)
        {
            return sqlController.CaseReadByCaseUId(caseUId);
        }
        #endregion

        #region private
        private void    ReplaceDataItemsInElements(List<Element> elementList, List<Answer> lstAnswers)
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

                    ReplaceDataItemsInElements(groupE.ElementList, lstAnswers);
                }
                #endregion
            }
        }

        private void    CaseCreateThread(MainElement mainElement, List<int> siteIds, string caseUId, DateTime navisionTime, string numberPlate, string roadNumber, bool reversed)
        {
            lock (_lockMain)
            {
                try
                {
                    if (mainElement.Repeated != 1 && reversed == false)
                        throw new ArgumentException("mainElement.Repeat was not equal to 1 & reversed is false. Hence no case created");

                    //sending and getting a reply
                    bool found = false;
                    foreach (int siteId in siteIds)
                    {
                        string mUId = SendXml(mainElement, siteId);

                        if (reversed == false)
                            sqlController.CaseCreate(mUId, int.Parse(mainElement.Id), siteId, caseUId, navisionTime, numberPlate, roadNumber);
                        else
                            sqlController.CheckListSitesCreate(int.Parse(mainElement.Id), siteId, mUId);

                        Case_Dto cDto = sqlController.CaseReadByMUId(mUId);
                        HandleCaseCreated(cDto, EventArgs.Empty);
                        HandleEvent(cDto.ToString() + " has been created", null);

                        found = true;
                    }

                    if (!found)
                        throw new Exception("CaseCreateThread failed. No matching sites found. No eForms created");

                    HandleEvent("eForm created", null);
                }
                catch (Exception ex)
                {
                    HandleExpection(ex);
                }
            }
        }

        private string  SendXml(MainElement mainElement, int siteId)
        {
            string reply = communicator.PostXml(mainElement.ClassToXml(), siteId);

            Response response = new Response();
            response = response.XmlToClass(reply);

            //trace msg HandleEvent(reply);
            //if reply is "success", it's created
            if (response.Type.ToString().ToLower() == "success")
            {
                return response.Value;
            }

            throw new NotImplementedException("siteId:'" + siteId + "' // failed to create eForm at Microting // Response :" + reply);
        }

        private void    HandleUpdateFiles()
        {
            if (updateIsRunningFiles)
            {
                updateIsRunningFiles = false;
                try
                {
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
                            client.DownloadFile(urlStr, fileLocation + fileName);
                        }
                        #endregion

                        #region finding checkSum
                        string chechSum = "";
                        using (var md5 = MD5.Create())
                        {
                            using (var stream = File.OpenRead(fileLocation + fileName))
                            {
                                byte[] grr = md5.ComputeHash(stream);
                                chechSum = BitConverter.ToString(grr).Replace("-","").ToLower();
                            }
                        }
                        #endregion

                        #region checks checkSum
                        if (chechSum != fileName.Substring(0, 32))
                            HandleEventWarning("Download of '" + urlStr + "' failed. Check sum did not match", EventArgs.Empty);
                        #endregion
                        
                        File_Dto fDto = new File_Dto(sqlController.FileCaseFindMUId(urlStr), fileLocation + fileName);
                        HandleFileDownloaded(fDto, EventArgs.Empty);
                        HandleEvent("Downloaded file '" + urlStr + "'.", null);

                        sqlController.FileProcessed(urlStr, chechSum, fileLocation, fileName);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    HandleExpection(ex);
                }
                updateIsRunningFiles = true;
            }
        }

        private void    HandleUpdateNotifications()
        {
            if (updateIsRunningNotifications)
            {
                updateIsRunningNotifications = false;
                try
                {
                    #region update notifications
                    string notificationStr, noteMuuId, noteType = "";
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

                        noteMuuId = t.Locate(notificationStr, "microting_uuid\\\":\\\"", "\\");
                        noteType = t.Locate(notificationStr, "text\\\":\\\"", "\\\"");

                        switch (noteType)
                        {
                            #region check_status / checklist updated by tablet
                            case "check_status":
                                {
                                    List<Case_Dto> caseLst = sqlController.CaseFindMatchs(noteMuuId);

                                    MainElement mainElement = new MainElement();
                                    foreach (Case_Dto aCase in caseLst)
                                    {
                                        if (aCase.SiteId == sqlController.CaseReadByMUId(noteMuuId).SiteId)
                                        {
                                            #region get response's data and update DB with data
                                            string lastId = sqlController.CaseReadCheckIdByMUId(noteMuuId);
                                            string respXml;

                                            if (lastId == null)
                                                respXml = communicator.Retrieve      (noteMuuId, aCase.SiteId);
                                            else
                                                respXml = communicator.RetrieveFromId(noteMuuId, aCase.SiteId, lastId);

                                            Response resp = new Response();
                                            resp = resp.XmlToClass(respXml);

                                            if (resp.Type == Response.ResponseTypes.Success)
                                            {
                                                sqlController.ChecksCreate(resp, respXml);

                                                if (lastId == null)
                                                    sqlController.CaseUpdate(noteMuuId, DateTime.Parse(resp.Checks[0].Date), int.Parse(resp.Checks[0].WorkerId), null             , int.Parse(resp.Checks[0].UnitId));
                                                else
                                                    sqlController.CaseUpdate(noteMuuId, DateTime.Parse(resp.Checks[0].Date), int.Parse(resp.Checks[0].WorkerId), resp.Checks[0].Id, int.Parse(resp.Checks[0].UnitId));
                                            }
                                            else
                                                throw new Exception("Failed to retrive eForm " + noteMuuId + " from site " + aCase.SiteId);

                                            Case_Dto cDto = sqlController.CaseReadByMUId(noteMuuId);
                                            HandleCaseUpdated(cDto, EventArgs.Empty);
                                            HandleEvent(cDto.ToString() + " has been completed", null);

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
                                                throw new Exception("Failed to delete eForm " + aCase.MicrotingUId + " from site " + aCase.SiteId);

                                            Case_Dto cDto = sqlController.CaseReadByMUId(aCase.MicrotingUId);
                                            HandleCaseDeleted(cDto, EventArgs.Empty);
                                            HandleEvent(cDto.ToString() + " has been removed", null);

                                            #endregion
                                        }
                                    }

                                    sqlController.NotificationProcessed(notificationStr);
                                    break;
                                }
                            #endregion

                            #region unit fetch / checklist retrieve by tablet
                            case "unit_fetch":
                                {
                                    Case_Dto cDto = sqlController.CaseReadByMUId(noteMuuId);
                                    HandleCaseRetrived(cDto, EventArgs.Empty);
                                    HandleEvent(cDto.ToString() + " has been retrived", null);

                                    sqlController.NotificationProcessed(notificationStr);
                                    break;
                                }
                            #endregion

                            #region unit_activate / tablet added
                            case "unit_activate":
                                {
                                    Case_Dto cDto = new Case_Dto("", "", int.Parse(noteMuuId));
                                    HandleSiteActivated(noteMuuId, EventArgs.Empty);
                                    HandleEvent(cDto.ToString() + " has been added", null);

                                    sqlController.NotificationProcessed(notificationStr);
                                    break;
                                }
                            #endregion

                            default:
                                throw new IndexOutOfRangeException("Notification type '" + noteType + "' is not known or mapped");
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    HandleExpection(ex);
                }
                updateIsRunningNotifications = true;
            }
        }

        private void    HandleEvent(object sender, EventArgs args)
        {
            lock (_lockEvent)
            {
                HandleEventLog("Client # " + sender.ToString(), EventArgs.Empty);
            }
        }

        private void    HandleEventReply(object sender, EventArgs args)
        {
            lock (_lockEventReply)
            {
                try
                {
                    HandleEventLog("Server # " + sender.ToString(), EventArgs.Empty);

                    string reply = sender.ToString();

                    if (reply.Contains("-update\",\"data"))
                    {
                        if (reply.Contains("\"id\\\":"))
                        {
                            string muuId = t.Locate(reply, "microting_uuid\\\":\\\"", "\\");
                            string nfId = t.Locate(reply, "\"id\\\":", ",");

                            sqlController.NotificationCreate(muuId, reply);
                            subscriber.ConfirmId(nfId);
                        }
                    }

                    //notifications //checks if there already are unprocessed files and notifications in the system
                    TriggerDbUpdates();
                }
                catch (Exception ex)
                {
                    HandleExpection(ex);
                }
            }
        }

        private void    HandleExpection(Exception ex)
        {
            HandleEventLog("", EventArgs.Empty);
            HandleEventLog("###### # EXCEPTION FOUND", EventArgs.Empty);
            HandleEventLog("Message:" + ex.Message, EventArgs.Empty);
            HandleEventLog("Source:" + ex.Source, EventArgs.Empty);
            HandleEventLog("StackTrace:" + ex.StackTrace, EventArgs.Empty);
            HandleEventLog("InnerException:" + ex.InnerException, EventArgs.Empty);
            HandleEventLog("###### # EXCEPTION FOUND", EventArgs.Empty);
            HandleEventLog("", EventArgs.Empty);
            HandleEventException(ex, EventArgs.Empty);

            Close();
            Thread.Sleep(1000);
            Start();
        }

        private void    TriggerDbUpdates()
        {
            Thread updateFilesThread         = new Thread(() => HandleUpdateFiles());
            updateFilesThread.Start();

            Thread updateNotificationsThread = new Thread(() => HandleUpdateNotifications());
            updateNotificationsThread.Start();
        }
        #endregion
    }
}