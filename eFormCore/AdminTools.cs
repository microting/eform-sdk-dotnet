using eFormShared;
using eFormSqlController;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormCore
{
    public class AdminTools
    {
        #region var
        string serverConnectionString;
        ICore core;
        SqlController sqlCon;
        Tools t = new Tools();
        #endregion

        #region con
        public AdminTools(string serverConnectionString)
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

            sqlCon = new SqlController(serverConnectionString);
        }
        #endregion

        public void Run()
        {
            #region warning
            Console.WriteLine("");
            Console.WriteLine("Admin tools program running.");
            Console.WriteLine("");
            Console.WriteLine("!!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!!");
            Console.WriteLine("!!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!!");
            Console.WriteLine("");
            Console.WriteLine("                These tools include; tools that will remove ALL data from database,");
            Console.WriteLine("                and other that could be VERY harmfull if used incorrect");
            Console.WriteLine("");
            Console.WriteLine("                Use with great care");
            Console.WriteLine("");
            Console.WriteLine("!!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!!");
            Console.WriteLine("!!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!!");
            #endregion

            while (true)
            {
                #region text
                Console.WriteLine("");
                Console.WriteLine("Press the following keys to run:");
                Console.WriteLine("'C' to retract all known eForm on devices");
                Console.WriteLine("'E' to retract all known Entities");
                Console.WriteLine("'D' to clear data base for data");
                Console.WriteLine("'T' to clear data base for templats");
                Console.WriteLine("'S' to clear settings");
                Console.WriteLine("'I' to import settings");
                Console.WriteLine("");
                Console.WriteLine("'A' to do all above. Complet reset");
                Console.WriteLine("");
                Console.WriteLine("'Q' to close admin tools");
                string input = Console.ReadLine();
                #endregion

                if (input.ToUpper() == "C")
                {
                    Console.WriteLine("Retract all known eForm on devices");
                }

                if (input.ToUpper() == "E")
                {
                    Console.WriteLine("Retract all known Entities");
                }

                if (input.ToUpper() == "D")
                {
                    Console.WriteLine("Clear data base for data");
                }

                if (input.ToUpper() == "T")
                {
                    Console.WriteLine("Clear data base for templats");
                }

                if (input.ToUpper() == "S")
                {
                    Console.WriteLine("Clear settings");
                }

                if (input.ToUpper() == "I")
                {
                    Console.WriteLine("Import settings");
                }

                if (input.ToUpper() == "A")
                {
                    Console.WriteLine("Complet reset");
                    Console.WriteLine("Type 'Y' only IF you are sure you want to do this");
                    string confirm = Console.ReadLine();

                    if (confirm.ToUpper() == "Y")
                        ClearAndResetSystems();
                }

                if (input.ToUpper() == "Q")
                {
                    Console.WriteLine("Close admin tools");
                    break;
                }
            }

            core.Close();
        }

        public void ClearAndResetSystems()
        {
            List<string> lstCaseMUIds = sqlCon.UnitTest_FindAllActiveCases();
            foreach (string mUId in lstCaseMUIds)
                core.CaseDelete(mUId);

            List<string> lstEntityMUIds = sqlCon.UnitTest_FindAllActiveEntities();
            foreach (string mUId in lstEntityMUIds)
                core.EntityGroupDelete(mUId);

            sqlCon.UnitTest_CleanAndResetDB();
        }

        #region events
        public void EventCaseCreated(object sender, EventArgs args)
        {
        }

        public void EventCaseRetrived(object sender, EventArgs args)
        {
        }

        public void EventCaseCompleted(object sender, EventArgs args)
        {
        }

        public void EventCaseDeleted(object sender, EventArgs args)
        {
        }

        public void EventFileDownloaded(object sender, EventArgs args)
        {
        }

        public void EventSiteActivated(object sender, EventArgs args)
        {
        }

        public void EventLog(object sender, EventArgs args)
        {
        }

        public void EventMessage(object sender, EventArgs args)
        {
        }

        public void EventWarning(object sender, EventArgs args)
        {
        }

        public void EventException(object sender, EventArgs args)
        {
        }
        #endregion
    }
}
