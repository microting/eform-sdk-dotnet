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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Select which MainController to start. Type 'd' for default, '1' for sample 1,  '2' for sample 2, and  '3' for sample 3");
            while (true)
            {
                string input = Console.ReadLine();

                if (input.ToLower() == "d")
                {
                    Default();
                    break;
                }

                if (input.ToLower() == "1")
                {
                    Sample1();
                    break;
                }

                if (input.ToLower() == "2")
                {
                    Sample2();
                    break;
                }

                if (input.ToLower() == "3")
                {
                    Sample3();
                    break;
                }
            }

            Console.WriteLine("Core has been closed. Will close Console in 5s");
            Thread.Sleep(5000);
            Environment.Exit(0);
        }

        static void     Default()
        {
            MainControllerSample1 mainController = new MainControllerSample1();
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Type 'q' (to quit)");
                Console.WriteLine("As long as the Core left running, the system is able to process eForms");
                string input = Console.ReadLine();


                if (input.ToLower() == "q")
                {
                    keepRunning = false;
                }
            }
            Console.WriteLine("Trying to shutting down");
            mainController.Close();
        }

        static void     Sample1()
        {
            MainControllerSample1 mainController = new MainControllerSample1();
            List<int> siteIds = new List<int>();
            int templatId = -1;

            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Type 's' for setting up the eForm templat from the sample1xml.txt, and define the siteId");
                Console.WriteLine("Type 'c' to create an eForm case based on the templat, and 'q' (to quit)");
                Console.WriteLine("As long as the Core left running, the system is able to process eForms");
                string input = Console.ReadLine();


                if (input.ToLower() == "q")
                {
                    keepRunning = false;
                }


                if (input.ToLower() == "s")
                {
                    siteIds = new List<int>();
                    Console.WriteLine("Type 'siteId' of the tablet wanted to be used");
                    int inputT = int.Parse(Console.ReadLine());
                    siteIds.Add(inputT);

                    string xmlStr = File.ReadAllText("sample1xml.txt");
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
                        mainController.CaseCreate(templatId, GenerateSampleCaseId(), siteIds);
                        Console.WriteLine("eForm case sent to Microting, should be able to be retrieved on your tablet soon");
                    }
                }
            }
            Console.WriteLine("Trying to shutting down");
            mainController.Close();
        }

        static void     Sample2()
        {
            MainControllerSample1 mainController = new MainControllerSample1();
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Type 'q' (to quit)");
                Console.WriteLine("As long as the Core left running, the system is able to process eForms");
                string input = Console.ReadLine();


                if (input.ToLower() == "q")
                {
                    keepRunning = false;
                }
            }
            Console.WriteLine("Trying to shutting down");
            mainController.Close();
        }

        static void     Sample3()
        {
            MainControllerSample3 mainController = new MainControllerSample3();
            mainController.SetSetting();

            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Type 's' to create needed eForms in the database and other elements (run only once) and 'q' (to quit)");
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
                    string xmlStr2 = File.ReadAllText("sample3step2xml.txt");
                    mainElement = mainController.TemplatFromXml(xmlStr2);

                    mainElement.CaseType = "Step two";
                    mainElement.CheckListFolderName = "Orders";

                    int tId2 = mainController.TemplatCreate(mainElement);
                    filStr += tId2 + Environment.NewLine;
                    #endregion

                    #region step 3
                    string xmlStr3W = File.ReadAllText("sample3step3Wxml.txt");
                    mainElement = mainController.TemplatFromXml(xmlStr3W);

                    mainElement.CaseType = "Step three";
                    mainElement.CheckListFolderName = "Orders";

                    int tId3W = mainController.TemplatCreate(mainElement);
                    filStr += tId3W + Environment.NewLine;



                    string xmlStr3L = File.ReadAllText("sample3step3Lxml.txt");
                    mainElement = mainController.TemplatFromXml(xmlStr3L);

                    mainElement.CaseType = "Step three";
                    mainElement.CheckListFolderName = "Orders";

                    int tId3L = mainController.TemplatCreate(mainElement);
                    filStr += tId3L + Environment.NewLine;
                    #endregion

                    #region step 4
                    string xmlStr4 = File.ReadAllText("sample3step4xml.txt");
                    mainElement = mainController.TemplatFromXml(xmlStr4);

                    mainElement.CaseType = "Step four";
                    mainElement.CheckListFolderName = "Containers";

                    int tId4 = mainController.TemplatCreate(mainElement);
                    filStr += tId4;
                    #endregion

                    FilSave(filStr, "sample3settings");
                    mainController.SetSetting();

                    #region step 1
                    string xmlStr1 = File.ReadAllText("sample3step1xml.txt");
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



        static string   GenerateSampleCaseId()
        {
            Random rdn = new Random();
            string str = DateTime.Now.ToLongTimeString() + "/" + rdn.Next(100000, 999999).ToString();
            return str;
        }

        static void     FilSave(string str, string name)
        {
            File.WriteAllText(name + ".txt", str);
        }
    }
}
