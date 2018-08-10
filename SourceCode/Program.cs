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

using eForm.Messages;
using eFormCore;
using eFormShared;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;
using System.IO;
using System.Threading;

namespace SourceCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //MicrotingDbMs DbContext;

            string ConnectionString = @"data source=(LocalDb)\SharedInstance;Initial catalog=eformsdk-tests;Integrated Security=True";
            //DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder();
            //dbContextOptionsBuilder.UseSqlServer(ConnectionString);
            //DbContext = new MicrotingDbMs(dbContextOptionsBuilder.Options);

            try
            {
                #region pick database

                string serverConnectionString = "";

                Console.WriteLine("Enter database to use:");
                Console.WriteLine("> If left blank, it will use 'Microting'");
                Console.WriteLine("  Enter name of database to be used");
                string databaseName = Console.ReadLine();

                if (databaseName.ToUpper() != "")
                    serverConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" + databaseName + ";Integrated Security=True";
                if (databaseName.ToUpper() == "T")
                    serverConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" + "MicrotingTest" + ";Integrated Security=True";
                if (databaseName.ToUpper() == "O")
                    serverConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" + "MicrotingOdense" + ";Integrated Security=True";
                if (serverConnectionString == "")
                    serverConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" + "MicrotingSourceCode" + ";Integrated Security=True";

                Console.WriteLine(serverConnectionString);
                #endregion

                #region Console.WriteLine(...text...)
                Console.WriteLine("");
                Console.WriteLine("Enter one of the following keys to start:");
                Console.WriteLine("  'A', for Admin tools");
                Console.WriteLine("> 'S', for sample programs");
                Console.WriteLine("  'I', for purely run core");
                Console.WriteLine("");
                Console.WriteLine("Any other will close Console");
                string input = Console.ReadLine().ToUpper();
                #endregion


                if (input == "A")
                {
                    var program = new AdminTools(serverConnectionString);
                    program.RunConsole();
                }
                if (input == "S")
                {
                    var program = new Samples(serverConnectionString);
                    program.Run();
                }
                if (input == "I")
                {
                    var core = new Core();
                    core.Start(serverConnectionString);
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


                Console.WriteLine("");
                Console.WriteLine("Console will close in 1s");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
            #region ...catch all...
            catch (Exception ex)
            {
                Tools t = new Tools();

                try
                {
                    File.AppendAllText("FatalException_" + DateTime.Now.ToString("MM.dd_HH.mm.ss") + ".txt", t.PrintException("Fatal Exception", ex));
                }
                catch { }

                Console.WriteLine("");
                Console.WriteLine(t.PrintException("Fatal Exception", ex));
                Console.WriteLine("");
                Console.WriteLine("Fatal Exception found and logged. Fil can be found at log/");
                Console.WriteLine("Console will close in 6s");
                Thread.Sleep(6000);
                Environment.Exit(0);
            }
            #endregion
        }
    }
}