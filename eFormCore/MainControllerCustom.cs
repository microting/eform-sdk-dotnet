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
using eFormShared;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace eFormCore
{
    public class MainControllerCustom
    {
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
                Case_Dto cDto = (Case_Dto)sender;
                Logic(cDto);
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
                    File.AppendAllText("log\\" + DateTime.Now.ToString("MM.dd") + "_log.txt", sender.ToString() + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    EventException(ex, EventArgs.Empty);
                }
            }
        }

        public void EventMessage(object sender, EventArgs args)
        {
            try
            {
                Console.WriteLine(sender.ToString());
            }
            catch (Exception ex)
            {
                EventException(ex, EventArgs.Empty);
            }
        }

        public void EventWarning(object sender, EventArgs args)
        {
            lock (_lockLogFil)
            {
                try
                {
                    File.AppendAllText("log\\" + DateTime.Now.ToString("MM.dd_HH.mm.ss") + "_warning.txt", sender.ToString() + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    EventException(ex, EventArgs.Empty);
                }
            }
        }

        public void EventException(object sender, EventArgs args)
        {
            Exception inEx = (Exception)sender;

            lock (_lockLogFil)
            {
                try
                {
                    File.AppendAllText("log\\" + DateTime.Now.ToString("MM.dd_HH.mm.ss") + "_EXCEPTION.txt", t.PrintException("Exception from Core", inEx) + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    EventException(ex, EventArgs.Empty);
                }
            }
        }
        #endregion

        #region var
        object _lockLogFil = new object();
        object _lockLogic = new object();
        bool keepSync;

        string serverConnectionString;
        ICore core;
        SqlController sqlCon;
        SqlControllerCustom sqlCustom;
        eFormShared.Tools t = new eFormShared.Tools();
        #endregion

        #region con
        public MainControllerCustom(string serverConnectionString)
        {
            this.serverConnectionString = serverConnectionString;

            core = new Core();
            #region connect event triggers
            core.HandleCaseCreated += EventCaseCreated;
            core.HandleCaseRetrived += EventCaseRetrived;
            core.HandleCaseCompleted += EventCaseCompleted;
            core.HandleCaseDeleted += EventCaseDeleted;
            core.HandleFileDownloaded += EventFileDownloaded;
            core.HandleSiteActivated += EventSiteActivated;
            core.HandleEventLog += EventLog;
            core.HandleEventMessage += EventMessage;
            core.HandleEventWarning += EventWarning;
            core.HandleEventException += EventException;
            #endregion

            sqlCon = new SqlController(serverConnectionString);
            sqlCustom = new SqlControllerCustom(serverConnectionString.Insert(serverConnectionString.IndexOf("Microting")+9, "Custom"));
        }
        #endregion

        #region public
        public void Run()
        {
            Setup();

            core.Start(serverConnectionString);

            Thread syncThread = new Thread(() => Sync());
            syncThread.Start();

            Console.WriteLine("Program running. Press any key to close");
            Console.ReadKey();

            Close();
        }

        public void Close()
        {
            keepSync = false;
            core.Close();
        }
        #endregion

        #region private
        #region key logic
        private void    Setup()
        {
            if (sqlCustom.VariableGet("setup_done") == "false")
            {
                Console.WriteLine("Type 'Y' only IF you are sure you want to remove ALL eForms");
                Console.WriteLine("from devices, and clean the database COMPLETLY, and resetup system.");
                Console.WriteLine("Press any other key to return");
                string input = Console.ReadLine();

                if (input.ToLower() == "y")
                {
                    Console.WriteLine("Program setting up...");

                    AdminTools at = new AdminTools(serverConnectionString);
                    at.SystemReset();

                    core.Start(serverConnectionString);

                    int copiesOnTable = 2;

                    #region import xml
                    string xml = File.ReadAllText("custom\\customStep1.txt");
                    string eGId;
                    int id;
                    EntityGroup eG;

                    eGId = core.EntityGroupCreate("EntitySearch", "containers");
                    xml = xml.Replace("[[container]]", eGId);
                    sqlCustom.VariableSet("container", eGId);
                    eG = core.EntityGroupRead(eGId);
                    Thread.Sleep(1000);
                    eG.EntityGroupItemLst.Add(new EntityItem("Placeholder", "temp", "1"));
                    Thread.Sleep(1000);
                    core.EntityGroupUpdate(eG);
                    Thread.Sleep(1000);

                    eGId = core.EntityGroupCreate("EntitySelect", "factions");
                    xml = xml.Replace("[[faction]]", eGId);
                    sqlCustom.VariableSet("faction", eGId);
                    eG = core.EntityGroupRead(eGId);
                    Thread.Sleep(1000);
                    eG.EntityGroupItemLst.Add(new EntityItem("Placeholder", "temp", "1"));
                    Thread.Sleep(1000);
                    core.EntityGroupUpdate(eG);
                    Thread.Sleep(1000);

                    eGId = core.EntityGroupCreate("EntitySelect", "locations");
                    xml = xml.Replace("[[location]]", eGId);
                    sqlCustom.VariableSet("location", eGId);
                    eG = core.EntityGroupRead(eGId);
                    Thread.Sleep(1000);
                    eG.EntityGroupItemLst.Add(new EntityItem("Placeholder", "temp", "1"));
                    Thread.Sleep(1000);
                    core.EntityGroupUpdate(eG);
                    Thread.Sleep(1000);


                    var temp = core.TemplatFromXml(xml);
                    temp.CaseType = "step1";
                    temp.Repeated = 0;
                    List<int> siteIds = new List<int>();
                    siteIds = sqlCustom.SitesRead("Workers");
                    id = CreateInfinityCase(temp, siteIds, copiesOnTable);
                    sqlCustom.VariableSet("step1", id.ToString());


                    xml = File.ReadAllText("custom\\customStep2.txt");
                    temp = core.TemplatFromXml(xml);
                    temp.CaseType = "step2";
                    temp.Repeated = 1;
                    id = core.TemplatCreate(temp);
                    sqlCustom.VariableSet("step2", id.ToString());


                    xml = File.ReadAllText("custom\\customStep3.txt");
                    temp = core.TemplatFromXml(xml);
                    temp.CaseType = "step3";
                    temp.Repeated = 1;
                    id = core.TemplatCreate(temp);
                    sqlCustom.VariableSet("step3", id.ToString());


                    xml = File.ReadAllText("custom\\customStep3b.txt");
                    temp = core.TemplatFromXml(xml);
                    temp.CaseType = "step3b";
                    temp.Repeated = 1;
                    id = core.TemplatCreate(temp);
                    sqlCustom.VariableSet("step3b", id.ToString());


                    xml = File.ReadAllText("custom\\customStep4.txt");
                    temp = core.TemplatFromXml(xml);
                    temp.CaseType = "step4";
                    temp.Repeated = 1;
                    id = core.TemplatCreate(temp);
                    sqlCustom.VariableSet("step4", id.ToString());
                    #endregion

                    Thread.Sleep(10000);
                    Console.WriteLine("Program setting up, done.");
                }
                sqlCustom.VariableSet("setup_done", "true");
                sqlCustom.VariableSet("synced", "false");
            }
        }

        private void    Logic(Case_Dto cDto)
        {
            int siteId = cDto.SiteUId;
            string caseType = cDto.CaseType;
            string caseUid = cDto.CaseUId;
            string mUId = cDto.MicrotingUId;
            string checkUId = cDto.CheckUId;

            ReplyElement reply = core.CaseRead(mUId, checkUId);

            CheckListValue replyDataE = (CheckListValue)reply.ElementList[0];
            FieldValue answer = (FieldValue)replyDataE.DataItemList[0];
            string location = answer.ValueReadable;

            if (caseType == "step1")
            #region create offering
            {
                string bookingId = "b:" + GenerateUId();
                string description = GenerateOrderInfo(reply);
                string label = "Sted: " + location + ", dato: " + DateTime.Now.ToShortDateString();

                #region send order
                MainElement mainElement = core.TemplatRead(int.Parse(sqlCustom.VariableGet("step3")));
                DataElement dataE = (DataElement)mainElement.ElementList[0];

                mainElement.DisplayOrder = GenerateDescDisplayIndex();
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
                replyDataE = (CheckListValue)replyOld.ElementList[0];

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
                        MainElement mainElement = core.TemplatRead(int.Parse(sqlCustom.VariableGet("step4")));
                        DataElement dataE = (DataElement)mainElement.ElementList[0];

                        mainElement.DisplayOrder = GenerateDescDisplayIndex();
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
                    MainElement mainElement = core.TemplatRead(int.Parse(sqlCustom.VariableGet("step3b")));
                    DataElement dataE = (DataElement)mainElement.ElementList[0];

                    mainElement.DisplayOrder = GenerateDescDisplayIndex();
                    mainElement.Label = label;
                    dataE.Label = label;
                    dataE.Description.InderValue = "<div><b>Afhentning allerede taget af anden</b></div>" + description;

                    core.CaseCreate(mainElement, "", siteId);
                    #endregion
                }
            }
            #endregion

            if (caseType == "step4")
            #region confirm/cancel step
            {
                string[] tokens = reply.Custom.Split(new[] { "//" }, StringSplitOptions.None);
                ReplyElement replyOld = core.CaseRead(tokens[0], tokens[1]);
                replyDataE = (CheckListValue)replyOld.ElementList[0];

                List<string> worker = sqlCustom.WorkerRead(reply.DoneById);
                string locationWorker = worker[0];

                string bookingId = tokens[2];
                string description = GenerateOrderInfo(replyOld);
                string label = "Sted: " + locationWorker + ", dato: " + DateTime.Now.ToShortDateString();

                CheckListValue replyResend = (CheckListValue)reply.ElementList[0];
                answer = (FieldValue)replyResend.DataItemList[1];
                if (answer.Value == "checked")
                {
                    #region resend order
                    MainElement mainElement = core.TemplatRead(int.Parse(sqlCustom.VariableGet("step3")));
                    DataElement dataE = (DataElement)mainElement.ElementList[0];

                    mainElement.DisplayOrder = GenerateDescDisplayIndex();
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

        private void    Sync()
        {
            keepSync = true;

            while (keepSync)
            {
                try
                {
                    if (sqlCustom.VariableGet("synced") == "false")
                    {
                        string mUId;
                        EntityGroup eG;

                        mUId = sqlCustom.VariableGet("container");
                        eG = core.EntityGroupRead(mUId);
                        eG.EntityGroupItemLst = sqlCustom.ContainerRead();
                        core.EntityGroupUpdate(eG);

                        mUId = sqlCustom.VariableGet("faction");
                        eG = core.EntityGroupRead(mUId);
                        eG.EntityGroupItemLst = sqlCustom.FactionRead();
                        core.EntityGroupUpdate(eG);

                        mUId = sqlCustom.VariableGet("location");
                        eG = core.EntityGroupRead(mUId);
                        eG.EntityGroupItemLst = sqlCustom.LocationRead();
                        core.EntityGroupUpdate(eG);

                        sqlCustom.VariableSet("synced", "true");
                    }
                }
                catch (Exception ex)
                {
                    EventWarning("Sync failed, system  unstable." + ex.Message, EventArgs.Empty);
                    Console.WriteLine("Sync failed, system  unstable." + ex.Message);
                }
                Thread.Sleep(5000);
            }
        }
        #endregion

        #region support logic
        private int     CreateInfinityCase(MainElement mainElement, List<int> siteIds, int instances)
        {
            if (mainElement.Repeated != 0)
                throw new Exception("InfinityCase are always Repeated = 0");

            int templatId = core.TemplatCreate(mainElement);
            mainElement = core.TemplatRead(templatId);

            foreach (int siteId in siteIds)
            {
                for (int i = 0; i < instances; i++)
                {
                    Thread.Sleep(250);

                    List<int> siteShortList = new List<int>();
                    siteShortList.Add(siteId);

                    core.CaseCreate(mainElement, "", siteShortList, GenerateUId(), true);
                }
            }

            return templatId;
        }

        private void    UpdateBooking(string bookingId, string label, string description)
        {
            //Delete old
            List<Case_Dto> lstCDto = sqlCon.CaseFindCustomMatchs(bookingId);
            foreach (Case_Dto item in lstCDto)
                core.CaseDelete(item.MicrotingUId);

            //Send new
            MainElement mainElement = core.TemplatRead(int.Parse(sqlCustom.VariableGet("step2")));
            DataElement dataE = (DataElement)mainElement.ElementList[0];

            mainElement.DisplayOrder = GenerateDescDisplayIndex();
            mainElement.Label = label;
            dataE.Label = label;
            dataE.Description.InderValue = description;

            List<int> siteIds = sqlCustom.SitesRead("Workers");
            core.CaseCreate(mainElement, "", siteIds, bookingId, false);
        }

        private string  GenerateUId()
        {
            Random rdn = new Random();
            return DateTime.Now.ToString("yyyyMMddHHmmss") + "r" + rdn.Next(10000, 99999).ToString();
        }

        private int     GenerateDescDisplayIndex()
        {
            return -int.Parse(DateTime.Now.ToString("MMddHHmmss"));
        }

        private string  GenerateOrderInfo(ReplyElement reply)
        {
            CheckListValue replyDataE = (CheckListValue)reply.ElementList[0];

            FieldValue answer = (FieldValue)replyDataE.DataItemList[1];
            string container = answer.ValueReadable;
            answer = (FieldValue)replyDataE.DataItemList[2];
            string faction = answer.ValueReadable;
            answer = (FieldValue)replyDataE.DataItemList[3];
            string location = answer.ValueReadable;

            List<string> worker = sqlCustom.WorkerRead(reply.DoneById);
            string name = worker[1];
            string phone = worker[2];

            string rtnStr =
                "<div>Container: " + container + "</div>" +
                "<div>Fraktion: " + faction + "</div>" +
                "<div>Placering: " + location + ", " + "</div>" +
                "<div>Bestilt dato og tid: " + DateTime.Now.ToShortDateString() + " kl. " + DateTime.Now.ToShortTimeString() + "</div>" +
                "<div>Bestilt af: " + name + " (tlf: " + phone + ")</div>";

            return rtnStr;
        }
        #endregion
        #endregion
    }
}