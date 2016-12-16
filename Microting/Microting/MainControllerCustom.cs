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

using eFormCustom;
using eFormRequest;
using eFormSqlController;

using System;
using System.Collections.Generic;
using System.IO;

namespace Microting
{
    class MainControllerCustom
    {
        #region var
        object _lockLogFil = new object();
        object _lockLogic = new object();

        ICore core;
        SqlControllerCustom sqlCustom;
        SqlControllerExtended sqlConExt;
        #endregion

        #region con
        public MainControllerCustom()
        {
            //DOSOMETHING: Change to your needs
            #region read settings
            string[] lines = File.ReadAllLines("Input.txt");

            string comToken = lines[0];
            string comAddress = lines[1];

            string subscriberToken = lines[3];
            string subscriberAddress = lines[4];
            string subscriberName = lines[5];

            string serverConnectionString = lines[7];
            int userId = int.Parse(lines[8]);

            string fileLocation = lines[10];
            #endregion

            core = new Core(comToken, comAddress, subscriberToken, subscriberAddress, subscriberName, serverConnectionString, userId, fileLocation, true);
            sqlCustom = new SqlControllerCustom("Data Source=DESKTOP-7V1APE5\\SQLEXPRESS;Initial Catalog=MicrotingCustom;Integrated Security=True"); //<<----- TODO
            sqlConExt = new SqlControllerExtended(serverConnectionString, userId);

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
        }
        #endregion

        public void Setup()
        {
            string xml = File.ReadAllText("custom\\customStep1.txt");
            var temp = TemplatFromXml(xml);
            temp.CaseType = "step1";
            temp.Repeated = 0;
            List<int> siteIds = new List<int>();
            siteIds = sqlCustom.SitesRead("Worker");
            TemplatCreateInfinityCase(temp, siteIds, 5);

            xml = File.ReadAllText("custom\\customStep2.txt");
            temp = TemplatFromXml(xml);
            temp.CaseType = "step2";
            temp.Repeated = 1;
            int id = TemplatCreate(temp);
            if (id != 3)
                Console.WriteLine("id != 3");

            xml = File.ReadAllText("custom\\customStep3.txt");
            temp = TemplatFromXml(xml);
            temp.CaseType = "step3";
            temp.Repeated = 1;
            id = TemplatCreate(temp);
            if (id != 5)
                Console.WriteLine("id != 5");

            xml = File.ReadAllText("custom\\customStep4.txt");
            temp = TemplatFromXml(xml);
            temp.CaseType = "step4";
            temp.Repeated = 1;
            id = TemplatCreate(temp);
            if (id != 7)
                Console.WriteLine("id != 7");
        }

        #region public
        public MainElement TemplatFromXml(string xmlString)
        {
            try
            {
                return core.TemplatFromXml(xmlString);
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public int TemplatCreate(MainElement mainElement)
        {
            try
            {
                return core.TemplatCreate(mainElement);
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void TemplatCreateInfinityCase(MainElement mainElement, List<int> siteIds, int instances)
        {
            if (mainElement.Repeated != 0)
                throw new Exception("InfinityCase are always Repeated = 0");

            try
            {
                int templatId = TemplatCreate(mainElement);
                mainElement = core.TemplatRead(templatId);

                foreach (int siteId in siteIds)
                {
                    for (int i = 0; i < instances; i++)
                    {
                        List<int> siteShortList = new List<int>();
                        siteShortList.Add(siteId);

                        core.CaseCreate(mainElement, "", siteShortList, true);
                    }
                }
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void CaseCreate(int templatId, string caseUId, List<int> siteIds)
        {
            try
            {
                MainElement mainElement = core.TemplatRead(templatId);
                mainElement.PushMessageTitle = "";
                mainElement.PushMessageBody = "";
                mainElement.SetStartDate(DateTime.Now);
                mainElement.SetEndDate(DateTime.Now.AddDays(2));

                core.CaseCreate(mainElement, caseUId, siteIds, false);
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void CaseRead(string mUId)
        {
            try
            {
                CoreElement replyElement = core.CaseRead(mUId, null);
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void CaseReadFromGroup(string caseUId)
        {
            try
            {
                CoreElement replyElement = core.CaseReadAllSites(caseUId);
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void CaseDelete(string muuId)
        {
            try
            {
                core.CaseDelete(muuId);
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void CaseDeleteAll(string caseUId)
        {
            try
            {
                int deletedCases = core.CaseDeleteAllSites(caseUId);
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void Close()
        {
            core.Close();
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
            lock (_lockLogic)
            {
                try
                {
                    Case_Dto trigger = (Case_Dto)sender;
                    int siteId = trigger.SiteId;
                    string caseType = trigger.CaseType;
                    string caseUid = trigger.CaseUId;
                    string mUId = trigger.MicrotingUId;
                    string checkUId = trigger.CheckUId;


                    //--------------------

                    #region create offering
                    if (caseType == "step1")
                    {
                        #region get info
                        ReplyElement reply = core.CaseRead(mUId, checkUId);
                        DataElement replyDataE = (DataElement)reply.ElementList[0];

                        Answer answer = (Answer)replyDataE.DataItemList[0];
                        string location = answer.Value;
                        answer = (Answer)replyDataE.DataItemList[1];
                        string container = answer.Value;
                        answer = (Answer)replyDataE.DataItemList[2];
                        string faction = answer.Value;
                        List<string> worker = sqlCustom.WorkerRead(reply.DoneById);
                        string locationWorker = worker[0];
                        string name = worker[1];
                        string phone = worker[2];

                        string label = "Sted: " + locationWorker + ", dato: " + DateTime.Now.ToShortDateString();
                        string description =
                            "<div>Container: " + container + "</div>" +
                            "<div>Placering: " + location + "</div>" +
                            "<div>Fraktion: " + faction + "</div>" +
                            "<div>Bestilt dato og tid: " + DateTime.Now.ToShortDateString() + " kl. " + DateTime.Now.ToShortTimeString() + "</div>" +
                            "<div>Bestilt af: " + name + " (tlf: " + phone + ")</div>";
                        #endregion

                        #region confirm booking
                        MainElement mainElement = core.TemplatRead(sqlCustom.TemplatIdRead("step2"));
                        DataElement dataE = (DataElement)mainElement.ElementList[0];
                        SaveButton saveButton = (SaveButton)dataE.DataItemList[0];

                        dataE.Label = label + ", bekræftet: <b>nej</b>";
                        dataE.Description = new CDataValue();
                        dataE.Description.InderValue = description;


                        List<int> siteIds = sqlCustom.SitesRead("Worker");
                        core.CaseCreate(mainElement, GenerateCaseUId(), siteIds, false);
                        #endregion

                        #region send order
                        mainElement = core.TemplatRead(sqlCustom.TemplatIdRead("step3"));
                        dataE = (DataElement)mainElement.ElementList[0];
                        saveButton = (SaveButton)dataE.DataItemList[0];

                        dataE.Label = label;
                        dataE.Description = new CDataValue();
                        dataE.Description.InderValue = description;


                        saveButton = (SaveButton)dataE.DataItemList[0];
                        saveButton.Description.InderValue = "Du skal tjekke din mappe 'Mine container-tømningsopgaver' for at se om du har fået opgaven.";

                        siteIds = sqlCustom.SitesRead("ShippingAgent");
                        core.CaseCreate(mainElement, GenerateCaseUId(), siteIds, false);
                        #endregion
                    }
                    #endregion

                    #region create replies (winner/others)
                    if (caseType == "step3")
                    {
                        #region is the first?
                        bool isFirst = false;
                        try
                        {
                            if (sqlConExt.CaseCountResponses(caseUid) == 1)
                                isFirst = true;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("isFirst failed", ex);
                        }
                        #endregion

                        if (isFirst)
                        {
                            ReplyElement reply = core.CaseRead(mUId, checkUId); //læse fra DB og ikke fra sendt data :(
                            DataElement replyDataE = (DataElement)reply.ElementList[0];

                            #region send win eForm
                            MainElement mainElement = core.TemplatRead(sqlCustom.TemplatIdRead("step4"));
                            DataElement dataE = (DataElement)mainElement.ElementList[0];

                            dataE.Label = replyDataE.Label;
                            dataE.Description.InderValue = replyDataE.Description.InderValue + "<div><b>Tryk og indtast nettovægt fra vejeseddel</b></div>" +
                                "<div>Hvis du ikke ønsker at afhente containeren, skal du trykke og sætte kryds for at frigive container til ny modtager.</div>";

                            List<int> siteIds = new List<int>();
                            siteIds.Add(siteId);
                            core.CaseCreate(mainElement, GenerateCaseUId(), siteIds, false);
                            #endregion

                            #region confirm order
                            MainElement mainElement2 = core.TemplatRead(sqlCustom.TemplatIdRead("step2"));
                            DataElement dataE2 = (DataElement)mainElement.ElementList[0];

                            dataE2.Label = replyDataE.Label + ", bekræftet: <b>ja</b>";
                            dataE2.Description.InderValue = replyDataE.Description.InderValue;

                            siteIds = sqlCustom.SitesRead("Worker");
                            core.CaseCreate(mainElement2, GenerateCaseUId(), siteIds, false);
                            #endregion
                        }
                        else
                        {
                            #region send loss eForm
                            //CoreElement reply = core.CaseRead(mUId, checkUId);

                            //DataElement replyDataE = (DataElement)reply.ElementList[0];
                            //Answer answer = (Answer)replyDataE.DataItemList[0];

                            //MainElement mainElement = core.TemplatRead(sqlCustom.TemplatIdRead("step3LtId"));
                            //DataElement dataE = (DataElement)mainElement.ElementList[0];
                            //None none = (None)dataE.DataItemList[0];

                            //none.Description = new CDataValue();
                            //none.Description.InderValue = "Collection missed at:" + DateTime.Now.ToShortDateString() + "/" + DateTime.Now.ToLongTimeString();


                            //List<int> siteIds = sqlCustom.SitesRead("ShippingAgent");
                            //core.CaseCreate(mainElement, DateTime.Now.ToLongTimeString() + "/" + rdn.Next(10000000, 99999999).ToString(), siteIds, false);
                            #endregion
                        }
                    }
                    #endregion

                    #region confirm step
                    if (caseType == "step4")
                    {
                        //CoreElement reply = core.CaseRead(mUId, checkUId);

                        //DataElement replyDataE = (DataElement)reply.ElementList[0];
                        //Answer answer = (Answer)replyDataE.DataItemList[0];

                        //#region is the winner?
                        //bool isWinner = false;
                        //try
                        //{
                        //    if (replyDataE.Label == "Won - Container collection")
                        //        isWinner = true;
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception("isWinner failed", ex);
                        //}
                        //#endregion

                        //if (isWinner)
                        //{
                        //    MainElement mainElement = core.TemplatRead(sqlCustom.TemplatIdRead("step4tId"));
                        //    DataElement dataE = (DataElement)mainElement.ElementList[0];
                        //    None none = (None)dataE.DataItemList[0];

                        //    none.Label = "Container collect at:" + answer.Value;

                        //    List<int> siteIds = sqlCustom.SitesRead("ShippingAgent");
                        //    core.CaseCreate(mainElement, DateTime.Now.ToLongTimeString() + "/" + rdn.Next(10000000, 99999999).ToString(), siteIds, false);
                        //}
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    EventMessage(ex.ToString(), EventArgs.Empty);
                }
            }
        }

        public void EventCaseDeleted(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
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
            lock (_lockLogFil)
            {
                try
                {
                    //DOSOMETHING: changed to fit your wishes and needs 
                    File.AppendAllText(@"log.txt", sender.ToString() + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    EventException(ex, EventArgs.Empty);
                }
            }
        }

        public void EventMessage(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Console.WriteLine(sender.ToString());
        }

        public void EventWarning(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Console.WriteLine("## WARNING ## " + sender.ToString() + " ## WARNING ##");
        }

        public void EventException(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Exception ex = (Exception)sender;
        }
        #endregion

        private string GenerateCaseUId()
        {
            Random rdn = new Random();
            return DateTime.Now.ToLongTimeString() + "/" + rdn.Next(10000000, 99999999).ToString();
        }
    }
}
