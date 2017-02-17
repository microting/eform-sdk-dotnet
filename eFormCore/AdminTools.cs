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
            communicator = new Communicator(sqlCon.SettingRead("comAddress"), sqlCon.SettingRead("comToken"), //TODO
                sqlCon.SettingRead("organizationId"), sqlCon.SettingRead("comAddressBasic"));
        }
        #endregion

        #region public
        public void   Run()
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
                Console.WriteLine("");
                Console.WriteLine("'P' to prime database");
                Console.WriteLine("'S' to clear database for settings, entity types and sites");
                Console.WriteLine("'I' to check database is primed");
                Console.WriteLine("");
                Console.WriteLine("'C' to retract all known eForm on devices");
                Console.WriteLine("'E' to retract all known Entities");
                Console.WriteLine("'D' to clear database for data");
                Console.WriteLine("'T' to clear database for templats");
                Console.WriteLine("'A' to complet database reset (all of the above)");
                Console.WriteLine("");
                Console.WriteLine("'Q' to close admin tools");
                string input = Console.ReadLine();
                #endregion

                if (input.ToUpper() == "Q")
                {
                    Console.WriteLine("Close admin tools");
                    break;
                }

                string reply = "";

                if (input.ToUpper() == "C")
                {
                    Console.WriteLine("Retract all known eForm on devices");
                    reply = RetractEforms();
                }

                if (input.ToUpper() == "E")
                {
                    Console.WriteLine("Retract all known Entities");
                    reply = RetractEntities();
                }

                if (input.ToUpper() == "D")
                {
                    Console.WriteLine("Clear database for data");
                    reply = DbClearData();
                }

                if (input.ToUpper() == "T")
                {
                    Console.WriteLine("Clear database for templats");
                    reply = DbClearTemplat();
                }

                if (input.ToUpper() == "S")
                {
                    Console.WriteLine("Clear database for settings and sites");
                    reply = DbClearSettings();
                }

                if (input.ToUpper() == "P")
                {
                    Console.WriteLine("Prime database");
                    Console.WriteLine("Enter token:");
                    string token = Console.ReadLine();
                    reply = DbPrime(token);
                }

                if (input.ToUpper() == "I")
                {
                    Console.WriteLine("Check database is primed");
                    reply = DbPrimeIs().ToString();
                }

                if (input.ToUpper() == "A")
                {
                    Console.WriteLine("Complet database reset");
                    reply = DbReset();
                }

                if (reply == "")
                    Console.WriteLine("Done");
                else
                    Console.WriteLine(reply);
            }
        }

        public string RetractEforms()
        {
            string reply = "";
            List<string> lstCaseMUIds = sqlCon.UnitTest_FindAllActiveCases();
            foreach (string mUId in lstCaseMUIds)
            {
                try
                {
                    var aCase = sqlCon.CaseReadByMUId(mUId);
                    if (aCase != null)
                    {
                        communicator.Delete(mUId, aCase.SiteUId);

                        try
                        {
                            sqlCon.CaseDelete(mUId);
                        }
                        catch
                        {
                            sqlCon.CaseDeleteReversed(mUId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    reply += "EformDelete  :'" + mUId + "' failed, due to:" + ex.Message + Environment.NewLine;
                }
            }

            return reply.Trim();
        }

        public string RetractEntities()
        {
            string reply = "";
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
                    reply += "EntityDelete :'" + mUId + "' failed, due to:" + ex.Message + Environment.NewLine;
                }
            }

            return reply.Trim();
        }

        public string DbClearData()
        {
            try
            {
                RetractEforms();

                sqlCon.UnitTest_TruncateTable(typeof(cases).Name);
                sqlCon.UnitTest_TruncateTable(typeof(case_versions).Name);
                //---
                sqlCon.UnitTest_TruncateTable(typeof(check_list_sites).Name);
                sqlCon.UnitTest_TruncateTable(typeof(check_list_site_versions).Name);
                //---
                sqlCon.UnitTest_TruncateTable(typeof(check_list_values).Name);
                sqlCon.UnitTest_TruncateTable(typeof(check_list_value_versions).Name);
                //---
                sqlCon.UnitTest_TruncateTable(typeof(field_values).Name);
                sqlCon.UnitTest_TruncateTable(typeof(field_value_versions).Name);
                //---
                sqlCon.UnitTest_TruncateTable(typeof(uploaded_data).Name);
                sqlCon.UnitTest_TruncateTable(typeof(uploaded_data_versions).Name);
                //---
                sqlCon.UnitTest_TruncateTable(typeof(outlook).Name);
                //---

                //---
                sqlCon.UnitTest_TruncateTable(typeof(notifications).Name);

                return "";
            }
            catch (Exception ex)
            {
                return t.PrintException(t.GetMethodName() + " failed", ex);
            }
        }

        public string DbClearTemplat()
        {
            try
            {
                RetractEntities();

                sqlCon.UnitTest_TruncateTable(typeof(entity_groups).Name);
                sqlCon.UnitTest_TruncateTable(typeof(entity_group_versions).Name);
                //---
                sqlCon.UnitTest_TruncateTable(typeof(entity_items).Name);
                sqlCon.UnitTest_TruncateTable(typeof(entity_item_versions).Name);
                //---
                sqlCon.UnitTest_TruncateTable(typeof(check_lists).Name);
                sqlCon.UnitTest_TruncateTable(typeof(check_list_versions).Name);
                //---
                sqlCon.UnitTest_TruncateTable(typeof(fields).Name);
                sqlCon.UnitTest_TruncateTable(typeof(field_versions).Name);

                return "";
            }
            catch (Exception ex)
            {
                return t.PrintException(t.GetMethodName() + " failed", ex);
            }
        }

        public string DbReset()
        {
            string reply = "";
            reply += RetractEforms() + Environment.NewLine;
            reply += RetractEntities() + Environment.NewLine;
            reply += DbClearData() + Environment.NewLine;
            reply += DbClearTemplat() + Environment.NewLine;

            return reply.TrimEnd();
        }

        public bool   DbPrimeIs()
        {
            return false; //TODO
        }
        #endregion

        #region private
        private string DbClearSettings()
        {
            try
            {
                sqlCon.UnitTest_TruncateTable(typeof(sites).Name);
                sqlCon.UnitTest_TruncateTable(typeof(site_versions).Name);
                //---
                sqlCon.UnitTest_TruncateTable(typeof(site_workers).Name);
                sqlCon.UnitTest_TruncateTable(typeof(site_worker_versions).Name);
                //---
                sqlCon.UnitTest_TruncateTable(typeof(units).Name);
                sqlCon.UnitTest_TruncateTable(typeof(unit_versions).Name);
                //---
                sqlCon.UnitTest_TruncateTable(typeof(workers).Name);
                sqlCon.UnitTest_TruncateTable(typeof(worker_versions).Name);
                //---


                //---
                sqlCon.UnitTest_TruncateTable(typeof(settings).Name);
                //---
                sqlCon.UnitTest_TruncateTable(typeof(field_types).Name);

                return "";
            }
            catch (Exception ex)
            {
                return t.PrintException(t.GetMethodName() + " failed", ex);
            }
        }

        private string DbPrime(string token)
        {
            try
            {
                return "faked method"; //TODO
            }
            catch (Exception ex)
            {
                return t.PrintException(t.GetMethodName() + " failed", ex);
            }
        }
        #endregion
    }
}