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

using eFormRequest;
using eFormSqlController;

using System;
using System.Collections.Generic;
using System.IO;

namespace Microting
{
    class MainController
    {
        #region var
        object _logFilLock = new object();
        object _lockLogic = new object();

        public ICore core;
        SqlControllerExtended sqlConExt;
        #endregion

        #region con
        public MainController()
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
            sqlConExt = new SqlControllerExtended(serverConnectionString, userId);

            #region connect event triggers
            core.HandleCaseCreated      += EventCaseCreated;
            core.HandleCaseRetrived     += EventCaseRetrived;
            core.HandleCaseUpdated      += EventCaseUpdated;
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

        #region public
        public MainElement  TemplatFromXml(string xmlString)
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

        public int          TemplatCreate(MainElement mainElement)
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

        public void         TemplatCreateInfinityCase(MainElement mainElement, List<int> siteIds, int instances)
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

        public void         CaseCreate(int templatId, string caseUId)
        {
            try
            {
                MainElement mainElement = core.TemplatRead(templatId);
                mainElement.PushMessageTitle = "";
                mainElement.PushMessageBody = "";
                mainElement.SetStartDate(DateTime.Now);
                mainElement.SetEndDate(DateTime.Now.AddDays(2));

                List<int> siteIds = new List<int>();
                siteIds.Add(1311);

                core.CaseCreate(mainElement, caseUId, siteIds, false);
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void         CaseRead(string mUId)
        {
            try
            {
                ReplyElement replyElement = core.CaseRead(mUId, null);
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void         CaseReadFromGroup(string caseUId)
        {
            try
            {
                ReplyElement replyElement = core.CaseReadAllSites(caseUId);
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void         CaseDelete(string muuId)
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

        public void         CaseDeleteAll(string caseUId)
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

        public void         Close()
        {
            core.Close();
        }
        #endregion

        #region events
        public void     EventCaseCreated(object sender, EventArgs args)
        {
            ////DOSOMETHING: changed to fit your wishes and needs 
            //Case_Dto temp = (Case_Dto)sender;
            //int siteId = temp.SiteId;
            //string caseType = temp.CaseType;
            //string caseUid = temp.CaseUId;
            //string mUId = temp.MicrotingUId;
            //string checkUId = temp.CheckUId;
        }

        public void     EventCaseRetrived(object sender, EventArgs args)
        {
            ////DOSOMETHING: changed to fit your wishes and needs 
            //Case_Dto temp = (Case_Dto)sender;
            //int siteId = temp.SiteId;
            //string caseType = temp.CaseType;
            //string caseUid = temp.CaseUId;
            //string mUId = temp.MicrotingUId;
            //string checkUId = temp.CheckUId;
        }

        public void     EventCaseUpdated(object sender, EventArgs args)
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
                    #region create siteIds lists
                    Random rdn = new Random();
                    List<int> tempSiteIds;

                    //Shipping agents
                    //Adjust - start
                    List<int> siteIdsSA = new List<int>();
                    siteIdsSA.Add(1310);
                    siteIdsSA.Add(1312);
                    siteIdsSA.Add(1313);
            
                    //Dockyard worker
                    List<int> siteIdsDW = new List<int>();
                    siteIdsDW.Add(1311);
                    //Adjust - end
                    #endregion

                    #region create offering
                    if (caseType == "Step one")
                    {
                        ReplyElement reply = core.CaseRead(mUId, checkUId);

                        //Adjust - start
                        DataElement replyDataE = (DataElement)reply.ElementList[0];
                        Answer answer = (Answer)replyDataE.DataItemList[0];

                        MainElement mainElement = core.TemplatRead(213);
                        DataElement dataE = (DataElement)mainElement.ElementList[0];
                        Number number = (Number)dataE.DataItemList[0];

                        number.Description = new CDataValue();
                        number.Description.InderValue = "start " + answer.Value + " something more";

                        dataE.Label = "Step two2";
                        //Adjust - end

                        core.CaseCreate(mainElement, DateTime.Now.ToLongTimeString() + "/" + rdn.Next(10000000, 99999999).ToString(), siteIdsSA, false);
                    }
                    #endregion

                    #region create replies (winner/others)
                    if (caseType == "Step two")
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
                            foreach (int item in siteIdsSA)
                            {
                                if (item == siteId)
                                {
                                    #region send win eForm
                                    ReplyElement reply = core.CaseRead(mUId, checkUId);


                                    //Adjust - start
                                    DataElement replyDataE = (DataElement)reply.ElementList[0];
                                    Answer answer = (Answer)replyDataE.DataItemList[0];

                                    MainElement mainElement = core.TemplatRead(215); // <---
                                    DataElement dataE = (DataElement)mainElement.ElementList[0];
                                    Number number = (Number)dataE.DataItemList[0];

                                    number.Description = new CDataValue();
                                    number.Description.InderValue = "Won " + answer.Value + " won";

                                    dataE.Label = "Step three - winner";
                                    //Adjust - end


                                    tempSiteIds = new List<int>();
                                    tempSiteIds.Add(siteId);
                                    core.CaseCreate(mainElement, DateTime.Now.ToLongTimeString() + "/" + rdn.Next(10000000, 99999999).ToString(), tempSiteIds, false);
                                    #endregion
                                }
                                else
                                {
                                    #region send loss eForm
                                    ReplyElement reply = core.CaseRead(mUId, checkUId);


                                    //Adjust - start
                                    DataElement replyDataE = (DataElement)reply.ElementList[0];
                                    Answer answer = (Answer)replyDataE.DataItemList[0];

                                    MainElement mainElement = core.TemplatRead(217); // <---
                                    DataElement dataE = (DataElement)mainElement.ElementList[0];
                                    Number number = (Number)dataE.DataItemList[0];

                                    number.Description = new CDataValue();
                                    number.Description.InderValue = "Lost " + answer.Value + " lost";


                                    dataE.Label = "Step three - lost";
                                    //Adjust - end


                                    tempSiteIds = new List<int>();
                                    tempSiteIds.Add(item);
                                    core.CaseCreate(mainElement, DateTime.Now.ToLongTimeString() + "/" + rdn.Next(10000000, 99999999).ToString(), tempSiteIds, false);
                                    #endregion
                                }
                            }
                        }
                    }
                    #endregion

                    #region final step
                    if (caseType == "Step three")
                    {
                        //Adjust - start
                        if (true) //if response from winner
                        {
                            ReplyElement reply = core.CaseRead(mUId, checkUId);

                            DataElement replyDataE = (DataElement)reply.ElementList[0];
                            Answer answer = (Answer)replyDataE.DataItemList[0];

                            MainElement mainElement = core.TemplatRead(215); // <---
                            DataElement dataE = (DataElement)mainElement.ElementList[0];
                            Number number = (Number)dataE.DataItemList[0];

                            number.Description = new CDataValue();
                            number.Description.InderValue = "Won " + answer.Value + " won";

                            dataE.Label = "Step four";

                            foreach (int item in siteIdsDW)
                            {
                                tempSiteIds = new List<int>();
                                tempSiteIds.Add(item); // <--- 
                                core.CaseCreate(mainElement, DateTime.Now.ToLongTimeString() + "/" + rdn.Next(10000000, 99999999).ToString(), tempSiteIds, false);
                            }
                            //Adjust - end
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    EventMessage(ex.ToString(), EventArgs.Empty);
                }
            }
        }

        public void     EventCaseDeleted(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            //Case_Dto temp = (Case_Dto)sender;
            //int siteId = temp.SiteId;
            //string caseType = temp.CaseType;
            //string caseUid = temp.CaseUId;
            //string mUId = temp.MicrotingUId;
            //string checkUId = temp.CheckUId;
        }

        public void     EventFileDownloaded(object sender, EventArgs args)
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

        public void     EventSiteActivated(object sender, EventArgs args)
        {
            ////DOSOMETHING: changed to fit your wishes and needs 
            //int siteId = int.Parse(sender.ToString());
        }

        public void     EventLog(object sender, EventArgs args)
        {
            lock (_logFilLock)
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

        public void     EventMessage(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Console.WriteLine(sender.ToString());
        }

        public void     EventWarning(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Console.WriteLine("## WARNING ## " + sender.ToString() + " ## WARNING ##");
        }

        public void     EventException(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Exception ex = (Exception)sender;
        }
        #endregion
    }
}