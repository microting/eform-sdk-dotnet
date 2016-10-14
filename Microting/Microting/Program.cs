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
using System.Threading.Tasks;

namespace Microting
{
    class Program
    {
        static void Main(string[] args)
        {
            MainController mainController = new MainController();
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Type 't' for test and 'q' (to quit)");
                string input = Console.ReadLine();


                if (input.ToLower() == "q")
                {
                    keepRunning = false;
                }


                if (input.ToLower() == "t")
                {
                    string xmlStr = File.ReadAllText("xml.txt");
                    int id = mainController.TemplatCreate(xmlStr, "Step one");
                    Console.WriteLine("id = '" + id.ToString() + "' created");

                    List<int> lst = new List<int>();
                    lst.Add(1311);

                    var main = mainController.core.TemplatRead(id);
                    main.Repeated = 0;

                    mainController.TemplatCreateInfinityCase(main, "Step one", lst, 4);
                }
            }
            Console.WriteLine("Trying to shutting down");
            mainController.Close();
            Environment.Exit(0);
        }
    }
}
