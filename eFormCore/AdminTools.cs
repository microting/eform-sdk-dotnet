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
        SqlController sqlController;
        Tools t = new Tools();
        #endregion

        #region con
        public AdminTools(string serverConnectionString)
        {
            connectionString = serverConnectionString;
            sqlController = new SqlController(serverConnectionString);
        }
        #endregion

        #region public
        public void   RunConsole()
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

            Communicator communicator = new Communicator(sqlController);
            if (communicator == null)
                return "Failed to create a communicator. Action canceled. Database maybe not configured correct";

            List<string> lstCaseMUIds = sqlController.UnitTest_FindAllActiveCases();
            foreach (string mUId in lstCaseMUIds)
            {
                try
                {
                    var aCase = sqlController.CaseReadByMUId(mUId);
                    if (aCase != null)
                    {
                        communicator.Delete(mUId, aCase.SiteUId);

                        try
                        {
                            sqlController.CaseDelete(mUId);
                        }
                        catch
                        {
                            sqlController.CaseDeleteReversed(mUId);
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

            Communicator communicator = new Communicator(sqlController);
            if (communicator == null)
                return "Failed to create a communicator. Action canceled. Database maybe not configured correct";

            List<string> lstEntityMUIds = sqlController.UnitTest_FindAllActiveEntities();
            foreach (string mUId in lstEntityMUIds)
            {
                try
                {
                    string type = sqlController.EntityGroupDelete(mUId);
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

                sqlController.UnitTest_TruncateTable(typeof(cases).Name);
                sqlController.UnitTest_TruncateTable(typeof(case_versions).Name);
                //---
                sqlController.UnitTest_TruncateTable(typeof(check_list_sites).Name);
                sqlController.UnitTest_TruncateTable(typeof(check_list_site_versions).Name);
                //---
                sqlController.UnitTest_TruncateTable(typeof(check_list_values).Name);
                sqlController.UnitTest_TruncateTable(typeof(check_list_value_versions).Name);
                //---
                sqlController.UnitTest_TruncateTable(typeof(field_values).Name);
                sqlController.UnitTest_TruncateTable(typeof(field_value_versions).Name);
                //---
                sqlController.UnitTest_TruncateTable(typeof(uploaded_data).Name);
                sqlController.UnitTest_TruncateTable(typeof(uploaded_data_versions).Name);
                //---
                sqlController.UnitTest_TruncateTable(typeof(outlook).Name);
                //---

                //---
                sqlController.UnitTest_TruncateTable(typeof(notifications).Name);

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

                sqlController.UnitTest_TruncateTable(typeof(entity_groups).Name);
                sqlController.UnitTest_TruncateTable(typeof(entity_group_versions).Name);
                //---
                sqlController.UnitTest_TruncateTable(typeof(entity_items).Name);
                sqlController.UnitTest_TruncateTable(typeof(entity_item_versions).Name);
                //---
                sqlController.UnitTest_TruncateTable(typeof(fields).Name);
                sqlController.UnitTest_TruncateTable(typeof(field_versions).Name);
                //---
                sqlController.UnitTest_TruncateTable(typeof(check_lists).Name);
                sqlController.UnitTest_TruncateTable(typeof(check_list_versions).Name);
   
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

            Communicator communicator = new Communicator(sqlController);
            if (communicator == null)
                return "Failed to create a communicator. Action canceled. Database maybe not configured correct";

            reply += RetractEforms() + Environment.NewLine;
            reply += RetractEntities() + Environment.NewLine;
            reply += DbClearData() + Environment.NewLine;
            reply += DbClearTemplat() + Environment.NewLine;

            return reply.TrimEnd();
        }

        public string DbSetup(string token)
        {
            try
            {
                sqlController = new SqlController(connectionString);
                Communicator communicator = new Communicator(sqlController);

                #region configure db
                if (!bool.Parse(sqlController.SettingRead(Settings.firstRunDone)))
                {
                    Organization_Dto organizationDto = communicator.OrganizationLoadAllFromRemote(token);
                    sqlController.SettingUpdate(Settings.token, token);
                    sqlController.SettingUpdate(Settings.comAddressApi, organizationDto.ComAddressApi);
                    sqlController.SettingUpdate(Settings.comAddressBasic, organizationDto.ComAddressBasic);
                    sqlController.SettingUpdate(Settings.comOrganizationId, organizationDto.Id.ToString());
                    sqlController.SettingUpdate(Settings.awsAccessKeyId, organizationDto.AwsAccessKeyId);
                    sqlController.SettingUpdate(Settings.awsSecretAccessKey, organizationDto.AwsSecretAccessKey);
                    sqlController.SettingUpdate(Settings.awsEndPoint, organizationDto.AwsEndPoint);
                    sqlController.SettingUpdate(Settings.unitLicenseNumber, organizationDto.UnitLicenseNumber.ToString());

                    sqlController.SettingUpdate(Settings.firstRunDone, "true");
                }
                #endregion

                sqlController = new SqlController(connectionString);
                communicator = new Communicator(sqlController);

                #region add site's data to db
                if (!bool.Parse(sqlController.SettingRead(Settings.knownSitesDone)))
                {
                    sqlController.UnitTest_TruncateTable(typeof(sites).Name);
                    foreach (var item in communicator.SiteLoadAllFromRemote())
                    {
                        SiteName_Dto siteDto = sqlController.SiteRead(item.SiteUId);
                        if (siteDto == null)
                        {
                            sqlController.SiteCreate(item.SiteUId, item.SiteName);
                        }
                    }

                    sqlController.UnitTest_TruncateTable(typeof(workers).Name);
                    foreach (var item in communicator.WorkerLoadAllFromRemote())
                    {
                        Worker_Dto workerDto = sqlController.WorkerRead(item.WorkerUId);
                        if (workerDto == null)
                        {
                            sqlController.WorkerCreate(item.WorkerUId, item.FirstName, item.LastName, item.Email);
                        }
                    }

                    sqlController.UnitTest_TruncateTable(typeof(site_workers).Name);
                    foreach (var item in communicator.SiteWorkerLoadAllFromRemote())
                    {
                        Site_Worker_Dto siteWorkerDto = sqlController.SiteWorkerRead(item.MicrotingUId, null, null);
                        if (siteWorkerDto == null)
                        {
                            try
                            {
                                sqlController.SiteWorkerCreate(item.MicrotingUId, item.SiteUId, item.WorkerUId);
                            }
                            catch
                            {
                                // We do catch this because right now we a descripency at the API side.
                            }
                            
                        }
                    }

                    int customerNo = communicator.OrganizationLoadAllFromRemote(token).CustomerNo;

                    sqlController.UnitTest_TruncateTable(typeof(units).Name);
                    foreach (var item in communicator.UnitLoadAllFromRemote(customerNo))
                    {
                        Unit_Dto unitDto = sqlController.UnitRead(item.UnitUId);
                        if (unitDto == null)
                        {
                            try
                            {
                                sqlController.UnitCreate(item.UnitUId, item.CustomerNo, item.OtpCode, item.SiteUId);
                            }
                            catch
                            {
                                // We do catch this because right now we a descripency at the API side.
                            }

                        }
                    }
                    sqlController.SettingUpdate(Settings.knownSitesDone, "true");
                }
                #endregion

                return "";
            }
            catch (Exception ex)
            {
                return t.PrintException(t.GetMethodName() + " failed", ex);
            }
        }

        public bool   DbSetupCompleted()
        {
            return sqlController.SettingCheck();
        }
        #endregion

        #region private
        private string DbSetupClear()
        {
            try
            {
                sqlController.UnitTest_TruncateTable(typeof(sites).Name);
                sqlController.UnitTest_TruncateTable(typeof(site_versions).Name);
                //---
                sqlController.UnitTest_TruncateTable(typeof(site_workers).Name);
                sqlController.UnitTest_TruncateTable(typeof(site_worker_versions).Name);
                //---
                sqlController.UnitTest_TruncateTable(typeof(units).Name);
                sqlController.UnitTest_TruncateTable(typeof(unit_versions).Name);
                //---
                sqlController.UnitTest_TruncateTable(typeof(workers).Name);
                sqlController.UnitTest_TruncateTable(typeof(worker_versions).Name);
                //---


                //---
                sqlController.UnitTest_TruncateTable(typeof(settings).Name);
                //---
                sqlController.UnitTest_TruncateTable(typeof(field_types).Name);

                return "";
            }
            catch (Exception ex)
            {
                return t.PrintException(t.GetMethodName() + " failed", ex);
            }
        }
        #endregion
    }
}