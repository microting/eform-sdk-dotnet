using eFormShared;
using eFormCommunicator;
using eFormSqlController;

using System;
using System.Collections.Generic;

namespace eFormCore
{
    public class AdminTools
    {
        #region var
        string connectionString;
        SqlController sqlCon;
        Tools t = new Tools();
        #endregion

        #region con
        public AdminTools(string serverConnectionString)
        {
            connectionString = serverConnectionString;
            sqlCon = new SqlController(serverConnectionString);
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
                Console.WriteLine("> 'C' to retract all known eForm on devices");
                Console.WriteLine("  'E' to retract all known Entities");
                Console.WriteLine("  'D' to clear database for data");
                Console.WriteLine("  'T' to clear database for templats");
                Console.WriteLine("  'A' to complet database reset (all of the above)");
                Console.WriteLine("");
                Console.WriteLine("> 'P' to prime, configure and add sites database");
                Console.WriteLine("  'S' to clear database for prime, configuration and sites");
                Console.WriteLine("  'I' to check database is primed");
                Console.WriteLine("");
                Console.WriteLine("> 'Q' to close admin tools");
                string input = Console.ReadLine();
                #endregion

                string reply = "";

                if (input.ToUpper() == "Q")
                {
                    Console.WriteLine("Close admin tools");
                    break;
                }

                switch (input.ToUpper())
                {
                    #region options
                    case "C":
                        Console.WriteLine("Retract all known eForm on devices");
                        reply = RetractEforms();
                        break;
                    case "E":
                        Console.WriteLine("Retract all known Entities");
                        reply = RetractEntities();
                        break;
                    case "D":
                        Console.WriteLine("Clear database for data");
                        reply = DbClearData();
                        break;
                    case "T":
                        Console.WriteLine("Clear database for templats");
                        reply = DbClearTemplat();
                        break;
                    case "A":
                        Console.WriteLine("Database and devices clear");
                        reply = DbClear();
                        break;
                    case "S":
                        Console.WriteLine("Clear database for prime, configuration and sites");
                        reply = DbSetupClear();
                        break;
                    case "P":
                        Console.WriteLine("Prime, configure and add sites to  database");
                        Console.WriteLine("Enter your token:");
                        string token = Console.ReadLine();
                        reply = DbSetup(token);
                        break;
                    case "I":
                        Console.WriteLine("Check is database primed and configured");
                        reply = DbSetupCompleted().ToString();
                        break;
                        #endregion
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

            Communicator communicator = GetCommunicator();
            if (communicator == null)
                return "Failed to create a communicator. Action canceled. Database maybe not configured correct";

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

            Communicator communicator = GetCommunicator();
            if (communicator == null)
                return "Failed to create a communicator. Action canceled. Database maybe not configured correct";

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

        public string DbClear()
        {
            string reply = "";

            Communicator communicator = GetCommunicator();
            if (communicator == null)
                return "Failed to create a communicator. Action canceled. Database maybe not configured correct";

            reply += RetractEforms() + Environment.NewLine;
            reply += RetractEntities() + Environment.NewLine;
            reply += DbClearData() + Environment.NewLine;
            reply += DbClearTemplat() + Environment.NewLine;

            return reply.TrimEnd();
        }

        public bool   DbSetupCompleted()
        {
            return sqlCon.SettingCheck();
        }
        #endregion

        #region private
        private string DbSetupClear()
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

        private string DbSetup(string token)
        {
            try
            {
                #region prime db
                sqlCon = null;
                sqlCon = new SqlController(connectionString);
                #endregion

                #region configure db
                string comAddressBasic = sqlCon.SettingRead("comAddressBasic");
                CommunicatorBasic basicCom = new CommunicatorBasic(comAddressBasic);
                List<string> settingLst = basicCom.GetSettings(token);

                foreach (var setting in settingLst)
                {
                    string[] line = setting.Split('|');
                    sqlCon.SettingUpdate(line[0], line[1]);
                }

                sqlCon.SettingUpdate("firstRunDone", "true");
                #endregion

                #region add sites to db
                Communicator communicator = GetCommunicator();
                if (communicator == null)
                    return "Failed to create a communicator. Action canceled. Database has not loaded sites";

                if (!bool.Parse(sqlCon.SettingRead("knownSitesDone")))
                {
                    sqlCon.UnitTest_TruncateTable(typeof(sites).Name);
                    foreach (var item in communicator.SiteLoadAllFromRemote())
                    {
                        SiteName_Dto siteDto = sqlCon.SiteRead(item.SiteUId);
                        if (siteDto == null)
                        {
                            sqlCon.SiteCreate(item.SiteUId, item.SiteName);
                        }
                    }

                    sqlCon.UnitTest_TruncateTable(typeof(workers).Name);
                    foreach (var item in communicator.WorkerLoadAllFromRemote())
                    {
                        Worker_Dto workerDto = sqlCon.WorkerRead(item.WorkerUId);
                        if (workerDto == null)
                        {
                            sqlCon.WorkerCreate(item.WorkerUId, item.FirstName, item.LastName, item.Email);
                        }
                    }

                    sqlCon.UnitTest_TruncateTable(typeof(site_workers).Name);
                    foreach (var item in communicator.SiteWorkerLoadAllFromRemote())
                    {
                        Site_Worker_Dto siteWorkerDto = sqlCon.SiteWorkerRead(item.MicrotingUId);
                        if (siteWorkerDto == null)
                        {
                            sqlCon.SiteWorkerCreate(item.MicrotingUId, item.SiteUId, item.WorkerUId);
                        }
                    }

                    Organization_Dto organizationDto = communicator.OrganizationLoadAllFromRemote();
                    int customerNo = organizationDto.CustomerNo;

                    sqlCon.UnitTest_TruncateTable(typeof(units).Name);
                    foreach (var item in communicator.UnitLoadAllFromRemote(customerNo))
                    {
                        Unit_Dto unitDto = sqlCon.UnitRead(item.UnitUId);
                        if (unitDto == null)
                        {
                            sqlCon.UnitCreate(item.UnitUId, item.CustomerNo, item.OtpCode, item.SiteUId);
                        }
                    }
                }
  
                sqlCon.SettingUpdate("knownSitesDone", "true");
                #endregion

                return "";
            }
            catch (Exception ex)
            {
                return t.PrintException(t.GetMethodName() + " failed", ex);
            }
        }

        private Communicator GetCommunicator()
        {
            try
            {
                string address =        sqlCon.SettingRead("comAddress");
                string token =          sqlCon.SettingRead("comToken");
                string organizationId = sqlCon.SettingRead("organizationId");
                string addressBasic =   sqlCon.SettingRead("comAddressBasic");

                Communicator communicator = new Communicator(address, token, organizationId, addressBasic);
                return communicator;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}