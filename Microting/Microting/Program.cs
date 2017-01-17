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
using System.Threading;

namespace Microting
{
    class Program
    {
        static void     Main(string[] args)
        {
            RunCustom();
            //RunDefault();
        }

        static void     RunCustom()
        {
            MainControllerCustom mainController = new MainControllerCustom();
            mainController.Setup();
            Console.WriteLine("Program running. Press any key to close");
            Console.ReadKey();
            mainController.Close();

            Console.WriteLine("Program has been closed. Will close Console in 2s");
            Thread.Sleep(2000);
            Environment.Exit(0);
        }

        static void     RunDefault()
        {
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("Select which MainController element to start. Type:");
                Console.WriteLine("'E' for exiting program,");
                Console.WriteLine("'D' for default,");
                Console.WriteLine("'1' for sample 1,");
                Console.WriteLine("'2' for sample 2,");
                Console.WriteLine("'3' for sample 3, and");
                Console.WriteLine("'C' for cleaning database and devices");

                string input = Console.ReadLine();

                if (input.ToLower() == "e")
                    break;

                if (input.ToLower() == "d")
                    Default();

                if (input.ToLower() == "1")
                    Sample1();

                if (input.ToLower() == "2")
                    Sample2();

                if (input.ToLower() == "3")
                    Sample3();

                if (input.ToLower() == "c")
                    CleanUp();
            }

            Console.WriteLine("Program has been closed. Will close Console in 2s");
            Thread.Sleep(2000);
            Environment.Exit(0);
        }

        static void     Default()
        {
            MainControllerSamples mainController = new MainControllerSamples();

            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Type 'Q' (to quit)");
                Console.WriteLine("As long as the Core left running, the system is able to process eForms");
                string input = Console.ReadLine();


                if (input.ToLower() == "Q")
                    keepRunning = false;
            }
            Console.WriteLine("Trying to shutting down");
            mainController.Close();
        }

        static void     Sample1()
        {
            MainControllerSamples mainController = new MainControllerSamples();
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
                    var main = mainController.TemplatFromXml(xmlStr);

                    main.Repeated = 1;
                    main.CaseType = "Test";
                    main.SetStartDate(DateTime.Now);
                    main.SetEndDate(DateTime.Now.AddDays(2));
                    
                    templatId = mainController.TemplatCreate(main);
                }

                if (input.ToLower() == "c")
                {
                    if (templatId == -1)
                    {
                        Console.WriteLine("System has not been setup. Run setup before trying to create a case");
                    }
                    else
                    {
                        mainController.CaseCreate(templatId, siteIds);
                        Console.WriteLine("eForm case sent to Microting, should be able to be retrieved on your tablet soon");
                    }
                }
            }
            Console.WriteLine("Trying to shutting down");
            mainController.Close();
        }

        static void     Sample2()
        {
            MainControllerSamples mainController = new MainControllerSamples();
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
                    var main = mainController.TemplatFromXml(xmlStr);

                    main.Repeated = 1;
                    main.CaseType = "Test";
                    main.SetStartDate(DateTime.Now);
                    main.SetEndDate(DateTime.Now.AddDays(2));

                    templatId = mainController.TemplatCreate(main);
                }

                if (input.ToLower() == "c")
                {
                    if (templatId == -1)
                    {
                        Console.WriteLine("System has not been setup. Run setup before trying to create a case");
                    }
                    else
                    {
                        mainController.CaseCreate(templatId, siteIds);
                        Console.WriteLine("eForm case sent to Microting, should be able to be retrieved on your devices soon");
                    }
                }
            }
            Console.WriteLine("Trying to shutting down");
            mainController.Close();
        }

        static void     Sample3()
        {
            MainControllerSamples mainController = new MainControllerSamples();
            mainController.SetSetting();

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
                    mainElement = mainController.TemplatFromXml(xmlStr2);

                    mainElement.CaseType = "Step two";
                    mainElement.CheckListFolderName = "Orders";

                    int tId2 = mainController.TemplatCreate(mainElement);
                    filStr += tId2 + Environment.NewLine;
                    #endregion

                    #region step 3
                    string xmlStr3W = File.ReadAllText("sample\\sample3step3Wxml.txt");
                    mainElement = mainController.TemplatFromXml(xmlStr3W);

                    mainElement.CaseType = "Step three";
                    mainElement.CheckListFolderName = "Orders";

                    int tId3W = mainController.TemplatCreate(mainElement);
                    filStr += tId3W + Environment.NewLine;



                    string xmlStr3L = File.ReadAllText("sample\\sample3step3Lxml.txt");
                    mainElement = mainController.TemplatFromXml(xmlStr3L);

                    mainElement.CaseType = "Step three";
                    mainElement.CheckListFolderName = "Orders";

                    int tId3L = mainController.TemplatCreate(mainElement);
                    filStr += tId3L + Environment.NewLine;
                    #endregion

                    #region step 4
                    string xmlStr4 = File.ReadAllText("sample\\sample3step4xml.txt");
                    mainElement = mainController.TemplatFromXml(xmlStr4);

                    mainElement.CaseType = "Step four";
                    mainElement.CheckListFolderName = "Containers";

                    int tId4 = mainController.TemplatCreate(mainElement);
                    filStr += tId4;
                    #endregion

                    FilSave(filStr, "sample\\sample3settings");
                    mainController.SetSetting();

                    #region step 1
                    string xmlStr1 = File.ReadAllText("sample\\sample3step1xml.txt");
                    mainElement = mainController.TemplatFromXml(xmlStr1);

                    mainElement.Repeated = 0;
                    mainElement.CaseType = "Step one";
                    mainElement.SetStartDate(DateTime.Now);
                    mainElement.SetEndDate(DateTime.Now.AddDays(3));

                    mainController.TemplatCreateInfinityCase(mainElement, siteIdsDW, 4);
                    #endregion
                }
            }
            Console.WriteLine("Trying to shutting down");
            mainController.Close();
        }

        static void     CleanUp()
        {
            Console.WriteLine("Type 'Y' only IF you are sure you want to remove ALL eForms");
            Console.WriteLine("from devices, and clean the database. Any other key to return");
            string input = Console.ReadLine();

            if (input.ToLower() != "y")
                return;

            try
            {
                #region read settings
                string[] lines = File.ReadAllLines("Input.txt");
                string serverConnectionString = lines[8];
                #endregion
                
                SqlControllerUnitTest sqlConUT = new SqlControllerUnitTest(serverConnectionString);
                MainController temp = new MainController();
                ICore core = temp.core;

                #region clean database
                try
                {
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
                core.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("CleanUp failed", ex);
            }
        }

        static void     FilSave(string str, string name)
        {
            File.WriteAllText(name + ".txt", str);
        }
    }
}