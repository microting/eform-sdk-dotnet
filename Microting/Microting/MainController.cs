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
        Core core;
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

            core = new Core(comToken, comAddress, subscriberToken, subscriberAddress, subscriberName, serverConnectionString, userId, fileLocation);
            #region connect event triggers
            core.HandleCaseCreated += EventCaseCreated;
            core.HandleCaseRetrived += EventCaseRetrived;
            core.HandleCaseUpdated += EventCaseUpdated;
            core.HandleCaseDeleted += EventCaseDeleted;
            core.HandleFileDownloaded += EventFileDownloaded;
            core.HandleSiteActivated += EventSiteActivated;
            core.HandleEventLog += EventLog;
            core.HandleEventWarning += EventWarning;
            core.HandleEventException += EventException;
            #endregion
            core.Start();
        }
        #endregion

        #region public
        public int  TemplatCreate(string xmlString)
        {
            try
            {
                return core.TemplatCreate(xmlString);
            }
            catch (Exception ex)
            {
                EventLog(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public int  TemplatCreate(MainElement mainElement)
        {
            try
            {
                return core.TemplatCreate(mainElement);
            }
            catch (Exception ex)
            {
                EventLog(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void CaseCreate(int templatId, string pushMessageTitle, string pushMessageBody, string numberPlate, string roadData, string roadNumber)
        {
            Random rdn = new Random();
            try
            {
                MainElement mainElement = core.TemplatRead(templatId);

                #region numberPlate // vejData
                mainElement.Label = numberPlate;
                DataElement dE = (DataElement)mainElement.ElementList[0];
                dE.Label = numberPlate;

                dE = (DataElement)mainElement.ElementList[0];
                dE.DataItemList[0].Label = roadData + " // " + roadNumber;
                #endregion

                // pushMessageTitle // pushMessageBody
                mainElement.PushMessageTitle = pushMessageTitle;
                mainElement.PushMessageBody = pushMessageBody;

                mainElement.SetStartDate(DateTime.Now);
                mainElement.SetEndDate(DateTime.Now.AddDays(2));

                string caseType = "WasteControlCase";
                
                core.CaseCreateAllSitesExtended(mainElement, roadData, caseType, DateTime.MinValue, numberPlate, roadNumber);
            }
            catch (Exception ex)
            {
                EventLog(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void CaseRead(string muuId)
        {
            try
            {
                ReplyElement replyElement = core.CaseRead(muuId);
            }
            catch (Exception ex)
            {
                EventLog(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        public void CaseReadFromGroup(string caseUId)
        {
            try
            {
                ReplyElement replyElement = core.CaseReadAllSites(caseUId);
            }
            catch (Exception ex)
            {
                EventLog(ex.ToString(), EventArgs.Empty);

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
                EventLog(ex.ToString(), EventArgs.Empty);

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
                EventLog(ex.ToString(), EventArgs.Empty);

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
            //DOSOMETHING: changed to fit your wishes and needs 
            Case_Dto temp = (Case_Dto)sender;
            string caseUid = temp.CaseUId;
            string muuId = temp.MicrotingUId;
            int siteId = temp.SiteId;
        }

        public void EventCaseRetrived(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Case_Dto temp = (Case_Dto)sender;
            string caseUid = temp.CaseUId;
            string muuId = temp.MicrotingUId;
            int siteId = temp.SiteId;
        }

        public void EventCaseUpdated(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Case_Dto temp = (Case_Dto)sender;
            string caseUid = temp.CaseUId;
            string muuId = temp.MicrotingUId;
            int siteId = temp.SiteId;
        }

        public void EventCaseDeleted(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Case_Dto temp = (Case_Dto)sender;
            string caseUid = temp.CaseUId;
            string muuId = temp.MicrotingUId;
            int siteId = temp.SiteId;
        }

        public void EventFileDownloaded(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            File_Dto temp = (File_Dto)sender;
            string caseUid = temp.CaseUId;
            string muuId = temp.MicrotingUId;
            int siteId = temp.SiteId;
            string fileLocation = temp.FileLocation;
        }

        public void EventSiteActivated(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            int siteId = int.Parse(sender.ToString());
        }

        public void EventLog(object sender, EventArgs args)
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
            Console.WriteLine("## EXCEPTION ##");
            Console.WriteLine("Message:" + ex.Message);
            Console.WriteLine("Source:" + ex.Source);
            Console.WriteLine("StackTrace:" + ex.StackTrace);
            Console.WriteLine("InnerException:" + ex.InnerException);
            Console.WriteLine("## EXCEPTION ##");
        }
        #endregion
    }
}