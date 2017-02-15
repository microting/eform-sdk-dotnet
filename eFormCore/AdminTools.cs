using eFormShared;
using eFormCommunicator;
using eFormSqlController;

using System;
using System.Collections.Generic;
using System.IO;

namespace eFormCore
{
    public class AdminTools
    {
        #region var
        Communicator communicator;
        SqlController sqlCon;
        Tools t = new Tools();
        #endregion

        #region con
        public AdminTools(string serverConnectionString)
        {
            sqlCon = new SqlController(serverConnectionString);
            communicator = new Communicator(sqlCon.SettingRead("comAddress"), sqlCon.SettingRead("comToken"), 
                sqlCon.SettingRead("organizationId"), sqlCon.SettingRead("comAddressBasic"));
        }
        #endregion

        public void Run()
        {
            #region warning
            Console.WriteLine("");
            Console.WriteLine("!!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!!");
            Console.WriteLine("!!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!!");
            Console.WriteLine("");
            Console.WriteLine("                These admin tools include; tools that will remove ALL data from database,");
            Console.WriteLine("                and other that could be VERY harmfull if used incorrect");
            Console.WriteLine("");
            Console.WriteLine("                - Use with great care");
            Console.WriteLine("");
            Console.WriteLine("!!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!!");
            Console.WriteLine("!!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!!");
            Console.WriteLine("");
            Console.WriteLine("Admin tools program running.");
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
                    RetractEforms();
                    Console.WriteLine("Done");
                }

                if (input.ToUpper() == "E")
                {
                    Console.WriteLine("Retract all known Entities");
                    RetractEntities();
                    Console.WriteLine("Done");
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
                    {
                        SystemReset();
                        Console.WriteLine("Done");
                    }
                }

                if (input.ToUpper() == "Q")
                {
                    Console.WriteLine("Close admin tools");
                    break;
                }
            }
        }

        public void RetractEforms()
        {
            List<string> lstCaseMUIds = sqlCon.UnitTest_FindAllActiveCases();
            foreach (string mUId in lstCaseMUIds)
            {
                try
                {
                    var aCase = sqlCon.CaseReadByMUId(mUId);
                    if (aCase != null)
                        communicator.Delete(mUId, aCase.SiteUId);
                }
                catch (Exception ex)
                {
                    File.AppendAllText("log\\" + DateTime.Now.ToString("MM.dd") + "_warning_dbClean.txt", "CaseDelete  :'" + mUId + "' failed, due to:" + ex.Message + Environment.NewLine);
                }
            }
        }

        public void RetractEntities()
        {
            List<string> lstEntityMUIds = sqlCon.UnitTest_FindAllActiveEntities();
            foreach (string mUId in lstEntityMUIds)
            {
                try
                {
                    string type = sqlCon.EntityGroupDelete(mUId);
                    if (type != null)
                        communicator.EntityGroupDelete(type, mUId);
                }
                catch (Exception ex)
                {
                    File.AppendAllText("log\\" + DateTime.Now.ToString("MM.dd") + "_warning_dbClean.txt", "EntityDelete:'" + mUId + "' failed, due to:" + ex.Message + Environment.NewLine);
                }
            }
        }

        public void DbClearAll()
        {
            try
            {
                sqlCon.UnitTest_TruncateTableAll();
            }
            catch (Exception ex)
            {
                File.AppendAllText("log\\" + DateTime.Now.ToString("MM.dd") + "_warning_dbClean.txt", t.PrintException("ClearAndResetSystems failed", ex));
            }
        }

        public void SystemReset()
        {
            RetractEforms();
            RetractEntities();
            DbClearAll();
        }
    }
}
