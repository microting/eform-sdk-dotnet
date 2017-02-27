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
                Console.WriteLine("Press following keys to start:");
                Console.WriteLine("'S' for sample programs");
                Console.WriteLine("'A' for admin tools program");
                Console.WriteLine("any other will close Console");
                string input = Console.ReadLine();

                string serverConnectionString = File.ReadAllText("input\\sql_connection.txt").Trim();
                if (input.ToUpper() == "S")
                {
                    var program = new Samples(serverConnectionString);
                    program.Run();
                }
                if (input.ToUpper() == "A")
                {
                    var program = new AdminTools(serverConnectionString);
                    program.RunConsole();
                }

                Console.WriteLine("Console will close in 1s");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
            catch (Exception ex) //Catch !!ALL!!
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
        }
    }
}