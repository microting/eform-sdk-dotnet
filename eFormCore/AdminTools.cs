/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 Microting A/S

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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microting.eForm;
using Microting.eForm.Communication;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure;

namespace eFormCore
{
    public class AdminTools
    {
        #region var
        string connectionString;
        SqlController sqlController;
        Log log;
        Tools t = new Tools();
        #endregion

        #region con
        public AdminTools(string serverConnectionString)
        {
            connectionString = serverConnectionString;
            sqlController = new SqlController(serverConnectionString);
            log = new Log(new CoreBase(), sqlController, 4);
        }
        #endregion

        #region public
        public async Task RunConsole()
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
                Console.WriteLine("> 'P' to prime, configure and add sites database");
                Console.WriteLine("  'S' to clear database for prime, configuration and sites");
                Console.WriteLine("  'I' to check database is primed");
                Console.WriteLine("");
                Console.WriteLine("> 'C' to retract all known eForm on devices");
                Console.WriteLine("  'E' to retract all known Entities");
                Console.WriteLine("  'D' to clear database for data");
                Console.WriteLine("  'T' to clear database for templats");
                Console.WriteLine("  'A' to complet database reset (all of the above)");
                Console.WriteLine("");
                Console.WriteLine("  'M' to force migration of database");
                Console.WriteLine("  'R' to reload settings from Microting (Token needs to be in db)");
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
                    //case "C":
                    //    Console.WriteLine("Retract all known eForm on devices");
                    //    reply = RetractEforms();
                    //    break;
                    //case "E":
                    //    Console.WriteLine("Retract all known Entities");
                    //    reply = RetractEntities();
                    //    break;
                    //case "D":
                    //    Console.WriteLine("Clear database for data");
                    //    reply = DbClearData();
                    //    break;
                    //case "T":
                    //    Console.WriteLine("Clear database for templats");
                    //    reply = DbClearTemplat();
                    //    break;
                    //case "A":
                    //    Console.WriteLine("Database and devices clear");
                    //    reply = DbClear();
                    //    break;
                    //case "S":
                    //    Console.WriteLine("Clear database for prime, configuration and sites");
                    //    reply = DbSetupClear();
                    //    break;
                    case "P":
                        Console.WriteLine("Prime, configure and add sites to  database");
                        Console.WriteLine("Enter your token:");
                        string token = Console.ReadLine();
                        reply = await DbSetup(token);
                        break;
                    case "I":
                        Console.WriteLine("Check is database primed");
                        List<string> checkResult = await DbSetupCompleted();
                        if (checkResult.Count == 1)
                        {
                            if (checkResult[0] == "NO SETTINGS PRESENT, NEEDS PRIMING!")
                                reply = checkResult[0];
                            else
                                reply = "Settings table is incomplete, please fix the following settings: " + String.Join(",", checkResult);
                        }
                        break;
                    case "M":
                        Console.WriteLine("MigrateDb");
//                        reply = MigrateDb().ToString();
                        break;
                    case "R":
                        Console.WriteLine("DbSettingsReloadRemote");
                        reply = DbSettingsReloadRemote().ToString();
                        break;
                        #endregion
                }

                if (reply == "")
                    Console.WriteLine("Done");
                else
                    Console.WriteLine(reply);
            }
        }

        public async Task<string> DbSetup(string token)
        {
//            try
//            {
                sqlController = new SqlController(connectionString);

                if (token == null)
                    token = await sqlController.SettingRead(Settings.token);
                
                await sqlController.SettingUpdate(Settings.token, token);

                // configure db
                await DbSettingsReloadRemote();


                string comAddressApi = await sqlController.SettingRead(Settings.comAddressApi);
                string comAddressBasic = await sqlController.SettingRead(Settings.comAddressBasic);
                string comOrganizationId = await sqlController.SettingRead(Settings.comOrganizationId);
                string ComAddressPdfUpload = await sqlController.SettingRead(Settings.comAddressPdfUpload);
                string ComSpeechToText = await sqlController.SettingRead(Settings.comSpeechToText);
                Communicator communicator = new Communicator(token, comAddressApi, comAddressBasic, comOrganizationId, ComAddressPdfUpload, log, ComSpeechToText);

                #region add site's data to db
                if (!bool.Parse(await sqlController.SettingRead(Settings.knownSitesDone)))
                {
                    foreach (var item in await communicator.SiteLoadAllFromRemote())
                    {
                        SiteName_Dto siteDto = await sqlController.SiteRead(item.SiteUId);
                        if (siteDto == null)
                        {
                            await sqlController.SiteCreate(item.SiteUId, item.SiteName);
                        }
                    }

                    foreach (var item in await communicator.WorkerLoadAllFromRemote())
                    {
                        Worker_Dto workerDto = await sqlController.WorkerRead(item.WorkerUId);
                        if (workerDto == null)
                        {
                            await sqlController.WorkerCreate(item.WorkerUId, item.FirstName, item.LastName, item.Email);
                        }
                    }

                    foreach (var item in await communicator.SiteWorkerLoadAllFromRemote())
                    {
                        Site_Worker_Dto siteWorkerDto = await sqlController.SiteWorkerRead(item.MicrotingUId, null, null);
                        if (siteWorkerDto == null)
                        {
                            try
                            {
                                await sqlController.SiteWorkerCreate(item.MicrotingUId, item.SiteUId, item.WorkerUId);
                            }
                            catch
                            {
                                // We do catch this because right now we a descripency at the API side.
                            }

                        }
                    }

                    int customerNo = communicator.OrganizationLoadAllFromRemote(token).Result.CustomerNo;

                    foreach (var item in await communicator.UnitLoadAllFromRemote(customerNo))
                    {
                        Unit_Dto unitDto = await sqlController.UnitRead(item.UnitUId);
                        if (unitDto == null)
                        {
                            try
                            {
                                await sqlController.UnitCreate(item.UnitUId, item.CustomerNo, item.OtpCode, item.SiteUId);
                            }
                            catch
                            {
                                // We do catch this because right now we a descripency at the API side.
                            }

                        }
                    }

                    foreach (Folder_Dto folderDto in await communicator.FolderLoadAllFromRemote())
                    {
                        if (folderDto.MicrotingUId != null)
                        {
                            Folder_Dto folder = await sqlController.FolderReadByMicrotingUUID((int)folderDto.MicrotingUId);

                            if (folder == null)
                            {
                                if (folderDto.ParentId == 0)
                                {
                                    await sqlController.FolderCreate(folderDto.Name, folderDto.Description, null,
                                        (int)folderDto.MicrotingUId);    

                                }
                                else
                                {
                                    if (folderDto.ParentId != null)
                                    {
                                        Folder_Dto parenFolder =
                                            await sqlController.FolderReadByMicrotingUUID((int) folderDto.ParentId);
                                    
                                        await sqlController.FolderCreate(folderDto.Name, folderDto.Description, parenFolder.Id,
                                            (int)folderDto.MicrotingUId);
                                    }
                                }
                            }
                        }
                    }
                    
                    await sqlController.SettingUpdate(Settings.knownSitesDone, "true");
                }
                #endregion

                await sqlController.SettingUpdate(Settings.firstRunDone, "true");

                return "";
//            }
//            catch (Exception ex)
//            {
//                return t.PrintException(t.GetMethodName("AdminTools") + " failed", ex);
//            }
        }

        public async Task<string> DbSettingsReloadRemote()
        {
            try
            {
                sqlController = new SqlController(connectionString);

                string token = await sqlController.SettingRead(Settings.token);
                Communicator communicator = new Communicator(token, @"https://srv05.microting.com", @"https://basic.microting.com", "", "", log, "https://speechtotext.microting.com");

                Organization_Dto organizationDto = await communicator.OrganizationLoadAllFromRemote(token);
                await sqlController.SettingUpdate(Settings.token, token);
                await sqlController.SettingUpdate(Settings.comAddressBasic, organizationDto.ComAddressBasic);
                await sqlController.SettingUpdate(Settings.comAddressPdfUpload, organizationDto.ComAddressPdfUpload);
                await sqlController.SettingUpdate(Settings.comAddressApi, organizationDto.ComAddressApi);
                await sqlController.SettingUpdate(Settings.comOrganizationId, organizationDto.Id.ToString());
                await sqlController.SettingUpdate(Settings.awsAccessKeyId, organizationDto.AwsAccessKeyId);
                await sqlController.SettingUpdate(Settings.awsSecretAccessKey, organizationDto.AwsSecretAccessKey);
                await sqlController.SettingUpdate(Settings.awsEndPoint, organizationDto.AwsEndPoint);
                await sqlController.SettingUpdate(Settings.unitLicenseNumber, organizationDto.UnitLicenseNumber.ToString());
                await sqlController.SettingUpdate(Settings.comSpeechToText, organizationDto.ComSpeechToText);
                if (await sqlController.SettingRead(Settings.logLevel) == "true")
                {
                    await sqlController.SettingUpdate(Settings.logLevel, "2");
                }

                return "";
            }
            catch (Exception ex)
            {
                return t.PrintException(t.GetMethodName("AdminTools") + " failed", ex);
            }
        }

        public async Task<List<string>> DbSetupCompleted()
        {
            return await sqlController.SettingCheckAll();

        }

//        public bool MigrateDb()
//        {
//            return sqlController.MigrateDb();
//        }
        #endregion

        #region private
        //private string DbSetupClear()
        //{
        //    try
        //    {
        //        sqlController.UnitTest_TruncateTable(typeof(sites).Name);
        //        sqlController.UnitTest_TruncateTable(typeof(site_versions).Name);
        //        //---
        //        sqlController.UnitTest_TruncateTable(typeof(site_workers).Name);
        //        sqlController.UnitTest_TruncateTable(typeof(site_worker_versions).Name);
        //        //---
        //        sqlController.UnitTest_TruncateTable(typeof(units).Name);
        //        sqlController.UnitTest_TruncateTable(typeof(unit_versions).Name);
        //        //---
        //        sqlController.UnitTest_TruncateTable(typeof(workers).Name);
        //        sqlController.UnitTest_TruncateTable(typeof(worker_versions).Name);
        //        //---


        //        //---
        //        sqlController.UnitTest_TruncateTable(typeof(settings).Name);
        //        //---
        //        sqlController.UnitTest_TruncateTable(typeof(field_types).Name);

        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        return t.PrintException(t.GetMethodName() + " failed", ex);
        //    }
        //}

        //private string DbSetupUnitTest()
        //{
        //    try
        //    {
        //        int siteUId_1 = sqlController.SiteCreate(2001, "first");
        //        int siteUId_2 = sqlController.SiteCreate(2002, "second");
        //        int siteUId_3 = sqlController.SiteCreate(2003, "third");

        //        int workerUId_1 = sqlController.WorkerCreate(444, "TesterA", "Alfa", "a@a.com");
        //        int workerUId_2 = sqlController.WorkerCreate(555, "TesterB", "Beta", "b@b.com");
        //        int workerUId_3 = sqlController.WorkerCreate(666, "TesterC", "Cent", "c@c.com");

        //        sqlController.SiteWorkerCreate(77, 2001, 444);
        //        sqlController.SiteWorkerCreate(88, 2002, 555);
        //        sqlController.SiteWorkerCreate(99, 2003, 666);

        //        sqlController.UnitCreate(123456, 11, 111111, 2001);
        //        sqlController.UnitCreate(234567, 12, 222222, 2002);
        //        sqlController.UnitCreate(345678, 13, 333333, 2003);

        //        sqlController.SettingUpdate(Settings.firstRunDone, "true");
        //        sqlController.SettingUpdate(Settings.knownSitesDone, "true");
        //        sqlController.SettingUpdate(Settings.token, "UNIT_TEST___________________L:32");
        //        sqlController.SettingUpdate(Settings.comAddressApi, "https://unittest.com");
        //        sqlController.SettingUpdate(Settings.comAddressPdfUpload, "https://unittest.com");
        //        sqlController.SettingUpdate(Settings.comOrganizationId, "-1");
        //        sqlController.SettingUpdate(Settings.unitLicenseNumber, "999");
        //        sqlController.SettingUpdate(Settings.httpServerAddress, "http://localhost:3000");

        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        return t.PrintException(t.GetMethodName() + " failed", ex);
        //    }
        //}
        #endregion
    }
}