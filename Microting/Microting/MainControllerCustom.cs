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
using System.Threading;

namespace Microting
{
    class MainControllerCustom
    {
        #region var
        object _lockLogFil = new object();
        object _lockLogic = new object();

        ICore core;
        SqlController sqlCon;
        SqlControllerCustom sqlCustom;
        #endregion

        #region con
        public MainControllerCustom()
        {
            #region read settings
            string[] lines = File.ReadAllLines("Input.txt");

            string comToken = lines[0];
            string comAddress = lines[1];
            string organizationId = lines[2];

            string subscriberToken = lines[4];
            string subscriberAddress = lines[5];
            string subscriberName = lines[6];

            string serverConnectionString = lines[8];
            string fileLocation = lines[9];
            bool logEnabled = bool.Parse(lines[10]);
            #endregion
            core = new Core(comToken, comAddress, organizationId, subscriberToken, subscriberAddress, subscriberName, serverConnectionString, fileLocation, logEnabled);
            sqlCon = new SqlController(serverConnectionString);
            sqlCustom = new SqlControllerCustom("Data Source=DESKTOP-7V1APE5\\SQLEXPRESS;Initial Catalog=MicrotingCustom;Integrated Security=True"); //<<----- TODO

            #region connect event triggers
            core.HandleCaseCreated      += EventCaseCreated;
            core.HandleCaseRetrived     += EventCaseRetrived;
            core.HandleCaseCompleted    += EventCaseCompleted;
            core.HandleCaseDeleted      += EventCaseDeleted;
            core.HandleFileDownloaded   += EventFileDownloaded;
            core.HandleSiteActivated    += EventSiteActivated;
            core.HandleEventLog         += EventLog;
            core.HandleEventMessage     += EventMessage;
            core.HandleEventWarning     += EventWarning;
            core.HandleEventException   += EventException;
            #endregion
            core.Start();
        }
        #endregion

        public void  Setup()
        {
            string xml = File.ReadAllText("custom\\customStep1.txt");
            var temp = TemplatFromXml(xml);
            temp.CaseType = "step1";
            temp.Repeated = 0;
            List<int> siteIds = new List<int>();
            siteIds = sqlCustom.SitesRead("Workers");
            TemplatCreateInfinityCase(temp, siteIds, 4);

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

            xml = File.ReadAllText("custom\\customStep3b.txt");
            temp = TemplatFromXml(xml);
            temp.CaseType = "step3b";
            temp.Repeated = 1;
            id = TemplatCreate(temp);
            if (id != 7)
                Console.WriteLine("id != 7");

            xml = File.ReadAllText("custom\\customStep4.txt");
            temp = TemplatFromXml(xml);
            temp.CaseType = "step4";
            temp.Repeated = 1;
            id = TemplatCreate(temp);
            if (id != 9)
                Console.WriteLine("id != 9");

            Thread.Sleep(5000);
        }

        public void  Test()
        {
            
            core.EntityGroupDelete("12924");

            string temp = core.EntityGroupCreate("EntitySearch", "MyTest");
            Console.WriteLine(temp);

            List<EntityItem> lst = new List<EntityItem>();
            EntityGroup eG = new EntityGroup("Group","EntitySearch", temp, lst);
            lst.Add(new EntityItem("Item1", "description"));
            lst.Add(new EntityItem("Item2", "description"));
            lst.Add(new EntityItem("Item3", "description"));
            lst.Add(new EntityItem("Item4", "description"));

            core.EntityGroupUpdate(eG);

            var tempi = core.EntityGroupRead(temp);

            tempi.EntityGroupItemLst[0].Name = "New";
            tempi.EntityGroupItemLst.Add(new EntityItem("Item5", "added"));

            core.EntityGroupUpdate(tempi);

            var tempii = core.EntityGroupRead(temp);

            core.EntityGroupDelete(temp);
        }

        private void Logic(Case_Dto cDto)
        {
            int siteId = cDto.SiteId;
            string caseType = cDto.CaseType;
            string caseUid = cDto.CaseUId;
            string mUId = cDto.MicrotingUId;
            string checkUId = cDto.CheckUId;

            ReplyElement reply = core.CaseRead(mUId, checkUId);

            if (caseType == "step1")
            #region create offering
            {
                List<string> worker = sqlCustom.WorkerRead(reply.DoneById);
                string locationWorker = worker[0];

                string bookingId = "b:" + GenerateUId();
                string description = GenerateOrderInfo(reply);
                string label = "Sted: " + locationWorker + ", dato: " + DateTime.Now.ToShortDateString();

                #region send order
                MainElement mainElement = core.TemplatRead(sqlCustom.TemplatIdRead("step3"));
                DataElement dataE = (DataElement)mainElement.ElementList[0];

                mainElement.Label = label;
                dataE.Label = label;
                dataE.Description.InderValue = description;

                List<int> siteIds = sqlCustom.SitesRead("ShippingAgents");
                core.CaseCreate(mainElement, GenerateUId(), siteIds, mUId + "//" + checkUId + "//" + bookingId, false);
                #endregion

                UpdateBooking(bookingId, label, "Bekræftet: <b>nej</b>" + description);
            }
            #endregion

            if (caseType == "step3")
            #region create replies (winner/others)
            {
                string[] tokens = reply.Custom.Split(new[] { "//" }, StringSplitOptions.None);
                ReplyElement replyOld = core.CaseRead(tokens[0], tokens[1]);
                DataElement replyDataE = (DataElement)replyOld.ElementList[0];

                List<string> worker = sqlCustom.WorkerRead(reply.DoneById);
                string locationWorker = worker[0];

                string bookingId = tokens[2];
                string description = GenerateOrderInfo(replyOld);
                string label = "Sted: " + locationWorker + ", dato: " + DateTime.Now.ToShortDateString();

                #region is the first?
                bool isFirst = false;
                try
                {
                    int count = sqlCon.CaseCountResponses(caseUid);
                    if (count == 1)
                        isFirst = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("isFirst failed", ex);
                }
                #endregion

                if (isFirst)
                {
                    #region send win eForm
                    {
                        MainElement mainElement = core.TemplatRead(sqlCustom.TemplatIdRead("step4"));
                        DataElement dataE = (DataElement)mainElement.ElementList[0];

                        mainElement.Label = label;
                        dataE.Label = label;
                        dataE.Description.InderValue = description + "<div><b>Tryk og indtast nettovægt fra vejeseddel</b></div>" +
                            "<div>Hvis du ikke ønsker at afhente containeren, skal du trykke og sætte kryds for at frigive container til ny modtager.</div>";

                        List<int> siteIds = new List<int>();
                        siteIds.Add(siteId);
                        core.CaseCreate(mainElement, "", siteIds, reply.Custom, false);
                    }
                    #endregion

                    UpdateBooking(bookingId, label, "Bekræftet: <b>ja</b>" + description);
                }
                else
                {
                    #region send loss eForm
                    MainElement mainElement = core.TemplatRead(sqlCustom.TemplatIdRead("step3b"));
                    DataElement dataE = (DataElement)mainElement.ElementList[0];

                    mainElement.Label = label;
                    dataE.Label = label;
                    dataE.Description.InderValue = "<div><b>Afhentning allerede taget af anden</b></div>" + description;

                    List<int> siteIds = new List<int>();
                    siteIds.Add(siteId);
                    core.CaseCreate(mainElement, "", siteIds);
                    #endregion
                }
            }
            #endregion

            if (caseType == "step4")
            #region confirm/cancel step
            {
                string[] tokens = reply.Custom.Split(new[] { "//" }, StringSplitOptions.None);
                ReplyElement replyOld = core.CaseRead(tokens[0], tokens[1]);
                DataElement replyDataE = (DataElement)replyOld.ElementList[0];

                List<string> worker = sqlCustom.WorkerRead(reply.DoneById);
                string locationWorker = worker[0];

                string bookingId = tokens[2];
                string description = GenerateOrderInfo(replyOld);
                string label = "Sted: " + locationWorker + ", dato: " + DateTime.Now.ToShortDateString();

                DataElement replyResend = (DataElement)reply.ElementList[0];
                Answer answer = (Answer)replyResend.DataItemList[1];
                if (answer.Value == "checked")
                {
                    #region resend order
                    MainElement mainElement = core.TemplatRead(sqlCustom.TemplatIdRead("step3"));
                    DataElement dataE = (DataElement)mainElement.ElementList[0];

                    mainElement.Label = label;
                    dataE.Label = label;
                    dataE.Description.InderValue = description;

                    List<int> siteIds = sqlCustom.SitesRead("ShippingAgents");
                    core.CaseCreate(mainElement, GenerateUId(), siteIds, reply.Custom, false);
                    #endregion

                    UpdateBooking(bookingId, label, "Bekræftet: <b>nej</b> (frigivet igen)" + description);
                }
                else
                {
                    UpdateBooking(bookingId, label, "Bekræftet: <b>afhentet</b>" + description);
                }
            }
            #endregion
        }

        private void UpdateBooking(string bookingId, string label, string description)
        {
            //Delete old
            List<Case_Dto> lstCDto = sqlCon.CaseFindCustomMatchs(bookingId);
            foreach (Case_Dto item in lstCDto)
                core.CaseDelete(item.MicrotingUId);

            //Send new
            MainElement mainElement = core.TemplatRead(sqlCustom.TemplatIdRead("step2"));
            DataElement dataE = (DataElement)mainElement.ElementList[0];

            mainElement.Label = label;
            dataE.Label = label;
            dataE.Description.InderValue = description;

            List<int> siteIds = sqlCustom.SitesRead("Workers");
            core.CaseCreate(mainElement, "", siteIds, bookingId, false);
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
                        Thread.Sleep(300);

                        List<int> siteShortList = new List<int>();
                        siteShortList.Add(siteId);

                        core.CaseCreate(mainElement, "ReversedCase", siteShortList, GenerateUId(), true);
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

        public void CaseCreate(int templatId, List<int> siteIds)
        {
            try
            {
                MainElement mainElement = core.TemplatRead(templatId);
                mainElement.PushMessageTitle = "";
                mainElement.PushMessageBody = "";
                mainElement.SetStartDate(DateTime.Now);
                mainElement.SetEndDate(DateTime.Now.AddDays(2));

                core.CaseCreate(mainElement, "", siteIds);
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

        }

        public void EventCaseRetrived(object sender, EventArgs args)
        {

        }

        public void EventCaseCompleted(object sender, EventArgs args)
        {
            lock (_lockLogic)
            {
                try
                {
                    Case_Dto cDto = (Case_Dto)sender;
                    Logic(cDto);
                }
                catch (Exception ex)
                {
                    EventMessage(ex.ToString(), EventArgs.Empty);
                }
            }
        }

        public void EventCaseDeleted(object sender, EventArgs args)
        {

        }

        public void EventFileDownloaded(object sender, EventArgs args)
        {

        }

        public void EventSiteActivated(object sender, EventArgs args)
        {
 
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

        private string GenerateUId()
        {
            Random rdn = new Random();
            return DateTime.Now.ToString("HHmmss") + "r" + rdn.Next(10000000, 99999999).ToString();
        }

        private string GenerateOrderInfo(ReplyElement reply)
        {
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

            string rtnStr =
                "<div>Container: " + container + "</div>" +
                "<div>Placering: " + location + "</div>" +
                "<div>Fraktion: " + faction + "</div>" +
                "<div>Bestilt dato og tid: " + DateTime.Now.ToShortDateString() + " kl. " + DateTime.Now.ToShortTimeString() + "</div>" +
                "<div>Bestilt af: " + name + " (tlf: " + phone + ")</div>";

            return rtnStr;
        }
    }
}
