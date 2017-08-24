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

using eFormCore;
using eFormShared;

using System;
using System.IO;
using System.Threading;

namespace SourceCode
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region Console.WriteLine(...text...)
                Console.WriteLine("Enter one of the following keys to start:");
                Console.WriteLine("> 'S', for sample programs");
                Console.WriteLine("  'I', for purely run core");
                Console.WriteLine("");
                Console.WriteLine("- Admin tools program on:");
                Console.WriteLine("> 'A', Microting");
                Console.WriteLine("  'T', Microting Test");
                Console.WriteLine("  'C', [custom]");
                Console.WriteLine("");
                Console.WriteLine("Any other will close Console");
                #endregion
                string input = Console.ReadLine();

                string serverConnectionString = File.ReadAllText("input\\sql_connection.txt").Trim();
                if (input.ToUpper() == "S")
                {
                    var program = new Samples   (serverConnectionString.Replace("Microting", "Microting"));
                    program.Run();
                }
                if (input.ToUpper() == "I")
                {
                    var core = new Core();
                    core.Start                  (serverConnectionString.Replace("Microting", "Microting"));
                    #region keep core running
                    while (true)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Press any key to exit program,");
                        Console.ReadLine();
                        break;
                    }
                    #endregion
                }
                if (input.ToUpper() == "O")
                {
                    var core = new Core();
                    core.Start                  (serverConnectionString.Replace("Microting", "MicrotingOdense"));
                    #region keep core running
                    while (true)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Press any key to exit program,");
                        Console.ReadLine();
                        break;
                    }
                    #endregion
                }
                if (input.ToUpper() == "A")
                {
                    var program = new AdminTools(serverConnectionString.Replace("Microting", "Microting"));
                    program.RunConsole();
                }
                if (input.ToUpper() == "T")
                {
                    var program = new AdminTools(serverConnectionString.Replace("Microting", "MicrotingTest"));
                    program.RunConsole();
                }
                #region ...custom...
                if (input.ToUpper() == "C")
                {
                    Console.WriteLine("");
                    Console.WriteLine("Enter the name of database to be used:");
                    string input2 = Console.ReadLine().Replace(" ", "");

                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        var program = new AdminTools(serverConnectionString.Replace("Microting", input2));
                        program.RunConsole();
                    }
                }
                #endregion

                Console.WriteLine("");
                Console.WriteLine("Console will close in 1s");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
            #region ...catch all...
            catch (Exception ex)
            {
                try
                {
                    Tools t = new Tools();
                    File.AppendAllText("log\\FatalException_" + DateTime.Now.ToString("MM.dd_HH.mm.ss") + ".txt", t.PrintException("Fatal Exception", ex));
                }
                catch { }

                Console.WriteLine("");
                Console.WriteLine("Fatal Exception found and logged. Fil can be found at log/");
                Console.WriteLine("Console will close in 5s");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
            #endregion
        }
    }
}