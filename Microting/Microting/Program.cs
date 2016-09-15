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
                Console.WriteLine("Press 1,2,3,4,5,6, t (for test) or q (to quit)");
                string input = Console.ReadLine();

                Random ran = new Random();
                string title = ""; // ran.Next(10000, 99999).ToString();
                string body = ""; // ran.Next(10000, 99999).ToString();

                if (input == "1")
                    cont.CreateEform(33, title, body, "1311");

                if (input == "2")
                    cont.CreateEform(35, title, body, "1311");

                if (input == "3")
                    cont.CreateEform(37, title, body, "1311");

                if (input == "4")
                    cont.CreateEform(39, title, body, "1311");

                if (input == "5")
                    cont.CreateEform(41, title, body, "1311");

                if (input == "6")
                    cont.CreateEform(43, title, body, "1311");

                if (input == "t")
                    cont.Test();

                if (input.ToLower() == "q")
                    keepRunning = false;
            }
            Console.WriteLine("Trying to shutting down");
            cont.Close();
            Environment.Exit(0);
        }
    }
}
