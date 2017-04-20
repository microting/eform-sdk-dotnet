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

using eFormData;
using eFormShared;

using System;
using System.Collections.Generic;
using System.IO;

namespace eFormCore
{
    public class Samples
    {
        #region var
        object _logFilLock = new object();
        object _lockLogic = new object();

        List<int> siteIdsDW = new List<int>();
        List<int> siteIdsSA = new List<int>();

        int step2tId = -1;
        int step3WtId = -1;
        int step3LtId = -1;
        int step4tId = -1;

        string serverConnectionString;
        ICore core;
        #endregion

        #region con
        public Samples(string serverConnectionString)
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

            core.Start(serverConnectionString);
        }
        #endregion

        #region public
        public void Run()
        {
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("Select which MainController element to start. Type:");
                Console.WriteLine("'E' for exiting program,");
                Console.WriteLine("'1' for sample 1");
                Console.WriteLine("'2' for sample 2");
                Console.WriteLine("'3' for sample 3");

                string input = Console.ReadLine();
                if (input.ToLower() == "e") break;
                if (input.ToLower() == "1") Sample1();
                if (input.ToLower() == "2") Sample2();
                if (input.ToLower() == "3") Sample3();
            }
        }

        public void Sample1()
        {
            List<int> siteIds = new List<int>();
            int templatId = -1;

            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Type 'S' for setting up the eForm templat from the sample1xml.txt, and define the siteId");
                Console.WriteLine("Type 'C' to create an eForm case based on the templat, and 'Q' (to quit)");
                Console.WriteLine("As long as the Core left running, the system is able to process eForms");
                string input = Console.ReadLine();

                if (input.ToLower() == "q")
                    keepRunning = false;

                if (input.ToLower() == "s")
                {
                    siteIds = new List<int>();
                    Console.WriteLine("Type 'siteId' of the tablet wanted to be used");
                    int inputT = int.Parse(Console.ReadLine());
                    siteIds.Add(inputT);

                    string xmlStr = File.ReadAllText("sample\\sample1xml.txt");
                    var main = TemplatFromXml(xmlStr);
                    main = core.TemplateUploadData(main);

                    // Best practice is to validate the parsed xml before trying to save and handle the error(s) gracefully.
                    List<string> validationErrors = core.TemplateValidation(main);
                    if (validationErrors.Count < 1)
                    {
                        main.Repeated = 1;
                        main.CaseType = "Test";
                        main.StartDate = DateTime.Now;
                        main.EndDate = DateTime.Now.AddDays(2);

                        templatId = TemplatCreate(main);
                    } else
                    {
                        foreach (string error in validationErrors)
                        {
                            Console.WriteLine("The following error is stopping us from creating the template: " + error);
                        }
                        Console.WriteLine(@"Correct the errors in sample\sample1xml.txt and try again");
                    }                    
                }

                if (input.ToLower() == "c")
                {
                    if (templatId == -1)
                    {
                        Console.WriteLine("System has not been setup. Run setup before trying to create a case");
                    }
                    else
                    {
                        CaseCreate(templatId, siteIds);
                        Console.WriteLine("eForm case sent to Microting, should be able to be retrieved on your tablet soon");
                    }
                }
            }
            Console.WriteLine("Trying to shutting down");
            Close();
        }

        public void Sample2()
        {
            List<int> siteIds = new List<int>();
            int templatId = -1;


            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Type 'S' for setting up the eForm templat from the sample2xml.txt, and define the siteId");
                Console.WriteLine("Type 'C' to create an eForm case based on the templat, and 'Q' (to quit)");
                Console.WriteLine("As long as the Core left running, the system is able to process eForms");
                string input = Console.ReadLine();


                if (input.ToLower() == "q")
                    keepRunning = false;


                if (input.ToLower() == "s")
                {
                    siteIds = new List<int>();
                    Console.WriteLine("Type 'siteId 1' of the device wanted to be used");
                    int inputT = int.Parse(Console.ReadLine());
                    siteIds.Add(inputT);

                    Console.WriteLine("Type 'siteId 2' of the device wanted to be used");
                    inputT = int.Parse(Console.ReadLine());
                    siteIds.Add(inputT);


                    string xmlStr = File.ReadAllText("sample\\sample2xml.txt");
                    var main = TemplatFromXml(xmlStr);

                    // Best practice is to validate the parsed xml before trying to save and handle the error(s) gracefully.
                    List<string> validationErrors = core.TemplateValidation(main);
                    if (validationErrors.Count < 1)
                    {
                        main.Repeated = 1;
                        main.CaseType = "Test";
                        main.StartDate = DateTime.Now;
                        main.EndDate = DateTime.Now.AddDays(2);

                        templatId = TemplatCreate(main);
                    }
                    else
                    {
                        foreach (string error in validationErrors)
                        {
                            Console.WriteLine("The following error is stopping us from creating the template: " + error);
                        }
                        Console.WriteLine(@"Correct the errors in sample\sample1xml.txt and try again");
                    }
                }

                if (input.ToLower() == "c")
                {
                    if (templatId == -1)
                    {
                        Console.WriteLine("System has not been setup. Run setup before trying to create a case");
                    }
                    else
                    {
                        CaseCreate(templatId, siteIds);
                        Console.WriteLine("eForm case sent to Microting, should be able to be retrieved on your devices soon");
                    }
                }
            }
            Console.WriteLine("Trying to shutting down");
            Close();
        }

        public void Sample3()
        {
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Type 'S' to create needed eForms in the database and other elements and 'Q' (to quit)");
                Console.WriteLine("As long as the Core left running, the system is able to process eForms");
                string input = Console.ReadLine();

                if (input.ToLower() == "q")
                {
                    keepRunning = false;
                }

                if (input.ToLower() == "s")
                {
                    MainElement mainElement;
                    string filStr = "";

                    #region input
                    List<int> siteIdsDW = new List<int>();
                    Console.WriteLine("Type 'siteId' of the 'Dockyard Worker's' tablet");
                    int inputDW = int.Parse(Console.ReadLine());
                    siteIdsDW.Add(inputDW);
                    filStr += inputDW + Environment.NewLine + Environment.NewLine;

                    Console.WriteLine("Type 'siteId' of the 'Shipping Agent 1's' tablet");
                    int inputSA = int.Parse(Console.ReadLine());
                    filStr += inputSA + Environment.NewLine;

                    Console.WriteLine("Type 'siteId' of the 'Shipping Agent 2's' tablet");
                    inputSA = int.Parse(Console.ReadLine());
                    filStr += inputSA + Environment.NewLine;

                    Console.WriteLine("Type 'siteId' of the 'Shipping Agent 3's' tablet");
                    inputSA = int.Parse(Console.ReadLine());
                    filStr += inputSA + Environment.NewLine + Environment.NewLine;
                    #endregion

                    #region step 2
                    string xmlStr2 = File.ReadAllText("sample\\sample3step2xml.txt");
                    mainElement = TemplatFromXml(xmlStr2);

                    // Best practice is to validate the parsed xml before trying to save and handle the error(s) gracefully.
                    List<string> validationErrors = core.TemplateValidation(mainElement);
                    if (validationErrors.Count < 1)
                    {
                        mainElement.CaseType = "Step two";
                        mainElement.CheckListFolderName = "Orders";
                        int tId2 = TemplatCreate(mainElement);
                        filStr += tId2 + Environment.NewLine;
                    }
                    else
                    {
                        foreach (string error in validationErrors)
                        {
                            Console.WriteLine("The following error is stopping us from creating the template: " + error);
                        }
                        Console.WriteLine(@"Correct the errors in sample\sample3step2xml.txt and try again");
                    }
                    #endregion

                    #region step 3
                    string xmlStr3W = File.ReadAllText("sample\\sample3step3Wxml.txt");
                    mainElement = TemplatFromXml(xmlStr3W);

                    // Best practice is to validate the parsed xml before trying to save and handle the error(s) gracefully.
                    validationErrors = core.TemplateValidation(mainElement);
                    if (validationErrors.Count < 1)
                    {
                        mainElement.CaseType = "Step three";
                        mainElement.CheckListFolderName = "Orders";
                        int tId3W = TemplatCreate(mainElement);
                        filStr += tId3W + Environment.NewLine;
                    }
                    else
                    {
                        foreach (string error in validationErrors)
                        {
                            Console.WriteLine("The following error is stopping us from creating the template: " + error);
                        }
                        Console.WriteLine(@"Correct the errors in sample\sample3step3Wxml.txt and try again");
                    }

                    string xmlStr3L = File.ReadAllText("sample\\sample3step3Lxml.txt");
                    mainElement = TemplatFromXml(xmlStr3L);

                    // Best practice is to validate the parsed xml before trying to save and handle the error(s) gracefully.
                    validationErrors = core.TemplateValidation(mainElement);
                    if (validationErrors.Count < 1)
                    {
                        mainElement.CaseType = "Step three";
                        mainElement.CheckListFolderName = "Orders";
                        int tId3L = TemplatCreate(mainElement);
                        filStr += tId3L + Environment.NewLine;
                    }
                    else
                    {
                        foreach (string error in validationErrors)
                        {
                            Console.WriteLine("The following error is stopping us from creating the template: " + error);
                        }
                        Console.WriteLine(@"Correct the errors in sample\sample3step3Lxml.txt and try again");
                    }
                    
                    #endregion

                    #region step 4
                    string xmlStr4 = File.ReadAllText("sample\\sample3step4xml.txt");
                    mainElement = TemplatFromXml(xmlStr4);

                    // Best practice is to validate the parsed xml before trying to save and handle the error(s) gracefully.
                    validationErrors = core.TemplateValidation(mainElement);
                    if (validationErrors.Count < 1)
                    {
                        mainElement.CaseType = "Step four";
                        mainElement.CheckListFolderName = "Containers";
                        int tId4 = TemplatCreate(mainElement);
                        filStr += tId4;
                    }
                    else
                    {
                        foreach (string error in validationErrors)
                        {
                            Console.WriteLine("The following error is stopping us from creating the template: " + error);
                        }
                        Console.WriteLine(@"Correct the errors in sample\sample3step4xml.txt and try again");
                    }
                    
                    #endregion

                    File.WriteAllText("sample\\sample3settings.txt", filStr);
                    SetSetting();

                    #region step 1
                    string xmlStr1 = File.ReadAllText("sample\\sample3step1xml.txt");
                    mainElement = TemplatFromXml(xmlStr1);

                    // Best practice is to validate the parsed xml before trying to save and handle the error(s) gracefully.
                    validationErrors = core.TemplateValidation(mainElement);
                    if (validationErrors.Count < 1)
                    {
                        mainElement.Repeated = 1;
                        mainElement.CaseType = "Step one";
                        mainElement.StartDate = DateTime.Now;
                        mainElement.EndDate = DateTime.Now.AddDays(3);

                        TemplatCreateInfinityCase(mainElement, siteIdsDW, 4);
                    }
                    else
                    {
                        foreach (string error in validationErrors)
                        {
                            Console.WriteLine("The following error is stopping us from creating the template: " + error);
                        }
                        Console.WriteLine(@"Correct the errors in sample\sample3step1xml.txt and try again");
                    }
                    #endregion
                }
            }
            Console.WriteLine("Trying to shutting down");
            Close();
        }
        #endregion

        #region private
        private void         SetSetting()
        {
            try
            {
                string[] lines = File.ReadAllLines("sample3settings.txt");

                siteIdsDW = new List<int>();
                siteIdsDW.Add(int.Parse(lines[0]));

                siteIdsSA = new List<int>();
                siteIdsSA.Add(int.Parse(lines[2]));
                siteIdsSA.Add(int.Parse(lines[3]));
                siteIdsSA.Add(int.Parse(lines[4]));

                step2tId = int.Parse(lines[6]);
                step3WtId = int.Parse(lines[7]);
                step3LtId = int.Parse(lines[8]);
                step4tId = int.Parse(lines[9]);
            }
            catch
            {

            }
        }

        private MainElement  TemplatFromXml(string xmlString)
        {
            MainElement temp = core.TemplateFromXml(xmlString);
            if (temp == null)
                throw new Exception("TemplatFromXml failed. Failed to convert xml");
            return temp;
        }

        private int          TemplatCreate(MainElement mainElement)
        {
            try
            {
                return core.TemplateCreate(mainElement);
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        private void         TemplatCreateInfinityCase(MainElement mainElement, List<int> siteIds, int instances)
        {
            if (mainElement.Repeated != 0)
                throw new Exception("InfinityCase are always Repeated = 0");

            try
            {
                int templatId = TemplatCreate(mainElement);
                mainElement = core.TemplateRead(templatId);

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

        private void         CaseCreate(int templatId, List<int> siteIds)
        {
            try
            {
                MainElement mainElement = core.TemplateRead(templatId);
                mainElement.PushMessageTitle = "";
                mainElement.PushMessageBody = "";
                mainElement.StartDate = DateTime.Now;
                mainElement.EndDate = DateTime.Now.AddDays(2);

                foreach (int siteId in siteIds)
                    core.CaseCreate(mainElement, "", siteId);
            }
            catch (Exception ex)
            {
                EventMessage(ex.ToString(), EventArgs.Empty);

                //DOSOMETHING: Handle the expection
                throw new NotImplementedException();
            }
        }

        private void         Close()
        {
            core.Close();
        }
        #endregion

        #region events
        public void     EventCaseCreated(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Case_Dto temp = (Case_Dto)sender;
            int siteId = temp.SiteUId;
            string caseType = temp.CaseType;
            string caseUid = temp.CaseUId;
            string mUId = temp.MicrotingUId;
            string checkUId = temp.CheckUId;
        }

        public void     EventCaseRetrived(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            Case_Dto temp = (Case_Dto)sender;
            int siteId = temp.SiteUId;
            string caseType = temp.CaseType;
            string caseUid = temp.CaseUId;
            string mUId = temp.MicrotingUId;
            string checkUId = temp.CheckUId;
        }

        public void     EventCaseCompleted(object sender, EventArgs args)
        {
            lock (_lockLogic)
            {
                try
                {
                    Case_Dto trigger = (Case_Dto)sender;
                    int siteId = trigger.SiteUId;
                    string caseType = trigger.CaseType;
                    string caseUid = trigger.CaseUId;
                    string mUId = trigger.MicrotingUId;
                    string checkUId = trigger.CheckUId;


                    //--------------------
                    Random rdn = new Random();

                    #region create offering
                    if (caseType == "Step one")
                    {
                        CoreElement reply = core.CaseRead(mUId, checkUId);

                        DataElement replyDataE = (DataElement)reply.ElementList[0];
                        FieldValue answer = (FieldValue)replyDataE.DataItemList[0];

                        MainElement mainElement = core.TemplateRead(step2tId);
                        DataElement dataE = (DataElement)mainElement.ElementList[0];
                        None none = (None)dataE.DataItemList[0];


                        none.Label = "Container with stat:" + answer.Value + " is ready for collection";
                        none.Description = new CDataValue();
                        none.Description.InderValue = DateTime.Now.ToShortDateString() + "/" + DateTime.Now.ToLongTimeString();


                        foreach (int siteIdTemp in siteIdsSA)
                            core.CaseCreate(mainElement, "", siteIdTemp);
                    }
                    #endregion

                    #region create replies (winner/others)
                    if (caseType == "Step two")
                    {
                        #region is the first?
                        bool isFirst = false;
                        int found = 0;
                        try
                        {
                            List<Case_Dto> lst = core.CaseLookupCaseUId(caseUid);

                            foreach (var item in lst)
                                if (item.Stat == "Completed")
                                    found++;

                            if (found == 1)
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
                            CoreElement reply = core.CaseRead(mUId, checkUId);

                            DataElement replyDataE = (DataElement)reply.ElementList[0];
                            FieldValue answer = (FieldValue)replyDataE.DataItemList[0];

                            MainElement mainElement = core.TemplateRead(step3WtId);
                            DataElement dataE = (DataElement)mainElement.ElementList[0];
                            Date date = (Date)dataE.DataItemList[0];

                            dataE.Description = new CDataValue();
                            dataE.Description.InderValue = DateTime.Now.ToShortDateString() + "/" + DateTime.Now.ToLongTimeString();

                            date.MinValue = DateTime.Now;
                            date.MaxValue = DateTime.Now.AddDays(1);
                            date.DefaultValue = DateTime.Now.AddMinutes(1).ToString("u");


                            core.CaseCreate(mainElement, "", siteId);
                            #endregion
                        }
                        else
                        {
                            #region send loss eForm
                            CoreElement reply = core.CaseRead(mUId, checkUId);

                            DataElement replyDataE = (DataElement)reply.ElementList[0];
                            FieldValue answer = (FieldValue)replyDataE.DataItemList[0];

                            MainElement mainElement = core.TemplateRead(step3LtId);
                            DataElement dataE = (DataElement)mainElement.ElementList[0];
                            None none = (None)dataE.DataItemList[0];

                            none.Description = new CDataValue();
                            none.Description.InderValue = "Collection missed at:" + DateTime.Now.ToShortDateString() + "/" + DateTime.Now.ToLongTimeString();


                            core.CaseCreate(mainElement, "", siteId);
                            #endregion
                        }
                    }
                    #endregion

                    #region final step
                    if (caseType == "Step three")
                    {
                        CoreElement reply = core.CaseRead(mUId, checkUId);

                        DataElement replyDataE = (DataElement)reply.ElementList[0];
                        FieldValue answer = (FieldValue)replyDataE.DataItemList[0];

                        #region is the winner?
                        bool isWinner = false;
                        try
                        {
                            if (replyDataE.Label == "Won - Container collection")
                                isWinner = true;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("isWinner failed", ex);
                        }
                        #endregion

                        if (isWinner)
                        {
                            MainElement mainElement = core.TemplateRead(step4tId);
                            DataElement dataE = (DataElement)mainElement.ElementList[0];
                            None none = (None)dataE.DataItemList[0];

                            none.Label = "Container collect at:" + answer.Value;

                            foreach (int siteIdTemp in siteIdsDW)
                                core.CaseCreate(mainElement, "", siteIdTemp);
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
            Case_Dto temp = (Case_Dto)sender;
            int siteId = temp.SiteUId;
            string caseType = temp.CaseType;
            string caseUid = temp.CaseUId;
            string mUId = temp.MicrotingUId;
            string checkUId = temp.CheckUId;
        }

        public void     EventFileDownloaded(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            File_Dto temp = (File_Dto)sender;
            int siteId = temp.SiteUId;
            string caseType = temp.CaseType;
            string caseUid = temp.CaseUId;
            string mUId = temp.MicrotingUId;
            string checkUId = temp.CheckUId;
            string fileLocation = temp.FileLocation;
        }

        public void     EventSiteActivated(object sender, EventArgs args)
        {
            //DOSOMETHING: changed to fit your wishes and needs 
            int siteId = int.Parse(sender.ToString());
        }

        public void     EventLog(object sender, EventArgs args)
        {
            lock (_logFilLock)
            {
                try
                {
                    //DOSOMETHING: changed to fit your wishes and needs 
                    File.AppendAllText(@"log\\log.txt", sender.ToString() + Environment.NewLine);
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