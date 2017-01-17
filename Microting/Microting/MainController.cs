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
    public class MainController
    {
        #region var
        object _lockLogFil = new object();
        object _lockLogic = new object();

        List<int> siteIdsWo = new List<int>();
        List<int> siteIdsSA = new List<int>();

        List<string> containers = new List<string>();
        List<string> factions   = new List<string>();
        List<string> locations  = new List<string>();

        public ICore core;
        #endregion

        #region con
        public MainController()
        {
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

            core.Start(comToken, comAddress, organizationId, subscriberToken, subscriberAddress, subscriberName, serverConnectionString, fileLocation, logEnabled);
        }
        #endregion

        #region public
        public MainElement  TemplatFromXml(string xmlString)
        {
            MainElement temp = core.TemplatFromXml(xmlString);
            if (temp == null)
                throw new Exception("TemplatFromXml failed. Failed to convert xml");
            return temp;
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

                        core.CaseCreate(mainElement, "", siteShortList, "", true);
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

        public void         CaseCreate(int templatId, string caseUId, int siteId)
        {
            try
            {
                MainElement mainElement = core.TemplatRead(templatId);
                mainElement.PushMessageTitle = "";
                mainElement.PushMessageBody = "";
                mainElement.SetStartDate(DateTime.Now);
                mainElement.SetEndDate(DateTime.Now.AddDays(2));

                core.CaseCreate(mainElement, "", siteId);
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
                CoreElement replyElement = core.CaseRead(mUId, null);
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
                CoreElement replyElement = core.CaseReadAllSites(caseUId);
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

        public void     EventCaseCompleted(object sender, EventArgs args)
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
                }
                catch
                {

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