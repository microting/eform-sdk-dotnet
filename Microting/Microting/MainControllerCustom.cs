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
        bool keepSync;
        string serverConnectionString;

        ICore core;
        SqlController sqlContro;
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

                   serverConnectionString = lines[8];
            string customConnectionString = lines[9];
            string fileLocation = lines[10];
            bool logEnabled = bool.Parse(lines[11]);
            #endregion
            core = new Core(comToken, comAddress, organizationId, subscriberToken, subscriberAddress, subscriberName, serverConnectionString, fileLocation, logEnabled);
            sqlContro = new SqlController(serverConnectionString);
            sqlCustom = new SqlControllerCustom(customConnectionString);

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

            Thread syncThread = new Thread(() => Sync());
            syncThread.Start();
        }
        #endregion

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

        public int TemplatCreateInfinityCase(MainElement mainElement, List<int> siteIds, int instances)
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
                        Thread.Sleep(250);

                        List<int> siteShortList = new List<int>();
                        siteShortList.Add(siteId);

                        core.CaseCreate(mainElement, "ReversedCase", siteShortList, GenerateUId(), true);
                    }
                }

                return templatId;
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
            keepSync = false;
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

        #region custom
        public void     Setup()
        {
            if (!sqlCustom.VariableGetBool("setup_done"))
            {
                Console.WriteLine("Type 'Y' only IF you are sure you want to remove ALL eForms");
                Console.WriteLine("from devices, and clean the database COMPLETLY, and resetup system.");
                Console.WriteLine("Press any other key to return");
                string input = Console.ReadLine();

                if (input.ToLower() == "y")
                {
                    Console.WriteLine("Program setting up...");
                    #region clean database
                    try
                    {
                        SqlControllerUnitTest sqlConUT = new SqlControllerUnitTest(serverConnectionString);

                        List<string> lstCaseMUIds = sqlConUT.FindAllActiveCases();
                        foreach (string mUId in lstCaseMUIds)
                            core.CaseDelete(mUId);

                        List<string> lstEntityMUIds = sqlConUT.FindAllActiveEntities();
                        foreach (string mUId in lstEntityMUIds)
                            core.EntityGroupDelete(mUId);

                        sqlConUT.CleanDB();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("CleanUp failed", ex);
                    }
                    #endregion

                    int copiesOnTable = 6;

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


                    var temp = TemplatFromXml(xml);
                    temp.CaseType = "step1";
                    temp.Repeated = 0;
                    List<int> siteIds = new List<int>();
                    siteIds = sqlCustom.SitesRead("Workers");
                    id = TemplatCreateInfinityCase(temp, siteIds, copiesOnTable);
                    sqlCustom.VariableSet("step1", id.ToString());


                    xml = File.ReadAllText("custom\\customStep2.txt");
                    temp = TemplatFromXml(xml);
                    temp.CaseType = "step2";
                    temp.Repeated = 1;
                    id = TemplatCreate(temp);
                    sqlCustom.VariableSet("step2", id.ToString());


                    xml = File.ReadAllText("custom\\customStep3.txt");
                    temp = TemplatFromXml(xml);
                    temp.CaseType = "step3";
                    temp.Repeated = 1;
                    id = TemplatCreate(temp);
                    sqlCustom.VariableSet("step3", id.ToString());


                    xml = File.ReadAllText("custom\\customStep3b.txt");
                    temp = TemplatFromXml(xml);
                    temp.CaseType = "step3b";
                    temp.Repeated = 1;
                    id = TemplatCreate(temp);
                    sqlCustom.VariableSet("step3b", id.ToString());


                    xml = File.ReadAllText("custom\\customStep4.txt");
                    temp = TemplatFromXml(xml);
                    temp.CaseType = "step4";
                    temp.Repeated = 1;
                    id = TemplatCreate(temp);
                    sqlCustom.VariableSet("step4", id.ToString());
                    #endregion

                    Thread.Sleep(10000);
                    Console.WriteLine("Program setting up, done.");
                }
                sqlCustom.VariableSet("setup_done", "true");
            }
        }

        private void    Sync()
        {
            keepSync = true;

            while (keepSync)
            {
                try
                {
                    if (!sqlCustom.VariableGetBool("synced"))
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
                    Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    throw new Exception("Sync failed, system  unstable.", ex);
                }
            }
        }

        private void    Logic(Case_Dto cDto)
        {
            int siteId = cDto.SiteId;
            string caseType = cDto.CaseType;
            string caseUid = cDto.CaseUId;
            string mUId = cDto.MicrotingUId;
            string checkUId = cDto.CheckUId;

            ReplyElement reply = core.CaseRead(mUId, checkUId);

            DataElement replyDataE = (DataElement)reply.ElementList[0];
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
                replyDataE = (DataElement)replyOld.ElementList[0];

                List<string> worker = sqlCustom.WorkerRead(reply.DoneById);
                string locationWorker = worker[0];

                string bookingId = tokens[2];
                string description = GenerateOrderInfo(replyOld);
                string label = "Sted: " + locationWorker + ", dato: " + DateTime.Now.ToShortDateString();

                #region is the first?
                bool isFirst = false;
                try
                {
                    int count = sqlContro.CaseCountResponses(caseUid);
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
                replyDataE = (DataElement)replyOld.ElementList[0];

                List<string> worker = sqlCustom.WorkerRead(reply.DoneById);
                string locationWorker = worker[0];

                string bookingId = tokens[2];
                string description = GenerateOrderInfo(replyOld);
                string label = "Sted: " + locationWorker + ", dato: " + DateTime.Now.ToShortDateString();

                DataElement replyResend = (DataElement)reply.ElementList[0];
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

        private void    UpdateBooking(string bookingId, string label, string description)
        {
            //Delete old
            List<Case_Dto> lstCDto = sqlContro.CaseFindCustomMatchs(bookingId);
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
            DataElement replyDataE = (DataElement)reply.ElementList[0];

            FieldValue answer = (FieldValue)replyDataE.DataItemList[1];
            string container = answer.ValueReadable;
            answer = (FieldValue)replyDataE.DataItemList[2];
            string faction = answer.ValueReadable;
            answer = (FieldValue)replyDataE.DataItemList[3];
            string location = answer.ValueReadable;

            List<string> worker = sqlCustom.WorkerRead(reply.DoneById);
            string name  = worker[1];
            string phone = worker[2];

            string rtnStr =
                "<div>Container: " + container + "</div>" +
                "<div>Fraktion: " + faction + "</div>" +
                "<div>Placering: " + location + ", " +"</div>" +
                "<div>Bestilt dato og tid: " + DateTime.Now.ToShortDateString() + " kl. " + DateTime.Now.ToShortTimeString() + "</div>" +
                "<div>Bestilt af: " + name + " (tlf: " + phone + ")</div>";

            return rtnStr;
        }
        #endregion
    }
}