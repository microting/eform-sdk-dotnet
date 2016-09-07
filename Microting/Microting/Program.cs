using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microting
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller cont = new Controller();
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Press 1,2,3,4,5,6 or q");
                string input = Console.ReadLine();

                Random ran = new Random();
                string roadData = ran.Next(10000, 99999).ToString();
                string roadNumber = ran.Next(10000, 99999).ToString();

                if (input == "1")
                    cont.Create(Controller.Templat.AsbestholdigtBygningsaffald, "LLNNNNN", roadData, roadNumber);

                if (input == "2")
                    cont.Create(Controller.Templat.Deponiaffald, "LLNNNNN", roadData, roadNumber);

                if (input == "3")
                    cont.Create(Controller.Templat.DeponiaffaldTilForbehandling, "LLNNNNN", roadData, roadNumber);

                if (input == "4")
                    cont.Create(Controller.Templat.PcbHoldigtBygningsaffald, "LLNNNNN", roadData, roadNumber);

                if (input == "5")
                    cont.Create(Controller.Templat.SmåtBrændbart, "LLNNNNN", roadData, roadNumber);

                if (input == "6")
                    cont.Create(Controller.Templat.StortBrændbart, "LLNNNNN", roadData, roadNumber);

                if (input.ToLower() == "q")
                    keepRunning = false;
            }
            Console.WriteLine("Trying to shutting down");
            cont.Close();
    //        Console.WriteLine("Press any key to close");
    //        Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
