/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm;
using Microting.eForm.Communication;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            DbContextHelper dbContextHelper = new DbContextHelper(serverConnectionString);
            sqlController = new SqlController(dbContextHelper);
            log = new Log(sqlController);
        }

        #endregion

        #region public

        public async Task RunConsole()
        {
            #region warning

            Console.WriteLine("");
            Console.WriteLine(
                "!!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!!");
            Console.WriteLine(
                "!!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!!");
            Console.WriteLine("");
            Console.WriteLine(
                "                These admin tools include; tools that will remove ALL data from database,");
            Console.WriteLine("                and other that could be VERY harmfull if used incorrect");
            Console.WriteLine("");
            Console.WriteLine("                - Use with great care");
            Console.WriteLine("");
            Console.WriteLine(
                "!!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!!");
            Console.WriteLine(
                "!!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!! - !!!WARNING!!!");
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
                                reply = "Settings table is incomplete, please fix the following settings: " +
                                        String.Join(",", checkResult);
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
            // try
            // {
            DbContextHelper dbContextHelper = new DbContextHelper(connectionString);
            sqlController = new SqlController(dbContextHelper);

            if (string.IsNullOrEmpty(token))
            {
                token = await sqlController.SettingRead(Settings.token);
            }
            else
            {
                await sqlController.SettingUpdate(Settings.token, token);
            }

            // configure db
            await DbSettingsReloadRemote();

            string comAddressApi = await sqlController.SettingRead(Settings.comAddressApi);
            string comAddressBasic = await sqlController.SettingRead(Settings.comAddressBasic);
            string comOrganizationId = await sqlController.SettingRead(Settings.comOrganizationId);
            string ComAddressPdfUpload = await sqlController.SettingRead(Settings.comAddressPdfUpload);
            string ComSpeechToText = await sqlController.SettingRead(Settings.comSpeechToText);
            Communicator communicator = new Communicator(token, comAddressApi, comAddressBasic, comOrganizationId,
                ComAddressPdfUpload, log, ComSpeechToText, connectionString);

            #region add site's data to db

            var settings = new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } };
            var parsedData = JRaw.Parse(await communicator.SiteLoadAllFromRemote());
            using (var dbContext = dbContextHelper.GetDbContext())
            {
                foreach (JToken item in parsedData)
                {
                    Site site = JsonConvert.DeserializeObject<Site>(item.ToString(), settings);
                    if (!dbContext.Sites.Any(x => x.MicrotingUid == site.MicrotingUid))
                    {
                        bool removed = false;
                        site.WorkflowState = site.WorkflowState == "active" ? "created" : site.WorkflowState;
                        site.WorkflowState = site.WorkflowState == "inactive" ? "removed" : site.WorkflowState;
                        if (site.WorkflowState == "removed")
                        {
                            removed = true;
                        }

                        await site.Create(dbContext);
                        if (removed)
                        {
                            await site.Delete(dbContext);
                        }
                    }
                    else
                    {
                        site = await dbContext.Sites.FirstAsync(x => x.MicrotingUid == site.MicrotingUid);
                        site.WorkflowState = item["WorkflowState"].ToString();
                        site.WorkflowState = site.WorkflowState == "active" ? "created" : site.WorkflowState;
                        site.WorkflowState = site.WorkflowState == "inactive" ? "removed" : site.WorkflowState;
                        site.Name = item["Name"].ToString();
                        await site.Update(dbContext);
                    }
                }

                parsedData = JRaw.Parse(await communicator.WorkerLoadAllFromRemote());
                foreach (JToken item in parsedData)
                {
                    Worker worker = JsonConvert.DeserializeObject<Worker>(item.ToString(), settings);

                    if (!dbContext.Workers.Any(x => x.MicrotingUid == worker.MicrotingUid))
                    {
                        bool removed = false;
                        worker.WorkflowState = worker.WorkflowState == "active" ? "created" : worker.WorkflowState;
                        worker.WorkflowState = worker.WorkflowState == "inactive" ? "removed" : worker.WorkflowState;
                        if (worker.WorkflowState == "removed")
                        {
                            removed = true;
                        }

                        await worker.Create(dbContext);
                        if (removed)
                        {
                            await worker.Delete(dbContext);
                        }
                    }
                    else
                    {
                        worker = await dbContext.Workers.FirstAsync(x => x.MicrotingUid == worker.MicrotingUid);
                        worker.WorkflowState = item["WorkflowState"].ToString();
                        worker.WorkflowState = worker.WorkflowState == "active" ? "created" : worker.WorkflowState;
                        worker.WorkflowState = worker.WorkflowState == "inactive" ? "removed" : worker.WorkflowState;
                        worker.FirstName = item["FirstName"].ToString();
                        worker.LastName = item["LastName"].ToString();
                        worker.Email = item["Email"].ToString();
                        await worker.Update(dbContext);
                    }
                }

                parsedData = JRaw.Parse(await communicator.SiteWorkerLoadAllFromRemote());
                foreach (JToken item in parsedData)
                {
                    try
                    {
                        int workerUId = int.Parse(item["UserId"].ToString());

                        SiteWorker siteWorker = JsonConvert.DeserializeObject<SiteWorker>(item.ToString(), settings);

                        int localSiteId = dbContext.Sites.FirstAsync(x => x.MicrotingUid == siteWorker.SiteId)
                            .GetAwaiter().GetResult().Id;
                        var workerAsync = await dbContext.Workers.FirstOrDefaultAsync(x => x.MicrotingUid == workerUId);
                        if (workerAsync != null)
                        {
                            int localWorkerId = workerAsync.Id;
                            if (!dbContext.SiteWorkers.Any(x => x.MicrotingUid == siteWorker.MicrotingUid))
                            {
                                bool removed = false;
                                siteWorker.SiteId = localSiteId;
                                siteWorker.WorkerId = localWorkerId;
                                siteWorker.WorkflowState = siteWorker.WorkflowState == "active"
                                    ? "created"
                                    : siteWorker.WorkflowState;
                                siteWorker.WorkflowState = siteWorker.WorkflowState == "inactive"
                                    ? "removed"
                                    : siteWorker.WorkflowState;
                                if (siteWorker.WorkflowState == "removed")
                                {
                                    removed = true;
                                }

                                await siteWorker.Create(dbContext);
                                if (removed)
                                {
                                    await siteWorker.Delete(dbContext);
                                }
                            }
                            else
                            {
                                siteWorker =
                                    await dbContext.SiteWorkers.FirstAsync(x =>
                                        x.MicrotingUid == siteWorker.MicrotingUid);
                                siteWorker.WorkflowState = item["WorkflowState"].ToString();
                                siteWorker.WorkflowState = siteWorker.WorkflowState == "active"
                                    ? "created"
                                    : siteWorker.WorkflowState;
                                siteWorker.WorkflowState = siteWorker.WorkflowState == "inactive"
                                    ? "removed"
                                    : siteWorker.WorkflowState;
                                siteWorker.SiteId = localSiteId;
                                siteWorker.WorkerId = localWorkerId;
                                await siteWorker.Update(dbContext);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                int customerNo = communicator.OrganizationLoadAllFromRemote(token).Result.CustomerNo;

                parsedData = JRaw.Parse(await communicator.UnitLoadAllFromRemote(customerNo));
                foreach (JToken item in parsedData)
                {
                    Unit unit = JsonConvert.DeserializeObject<Unit>(item.ToString(), settings);

                    int localSiteId = dbContext.Sites.FirstAsync(x => x.MicrotingUid == unit.SiteId).GetAwaiter()
                        .GetResult().Id;

                    if (!dbContext.Units.Any(x => x.MicrotingUid == unit.MicrotingUid))
                    {
                        bool removed = false;
                        unit.SiteId = localSiteId;
                        unit.WorkflowState = unit.WorkflowState == "active" ? "created" : unit.WorkflowState;
                        unit.WorkflowState = unit.WorkflowState == "new" ? "created" : unit.WorkflowState;
                        unit.WorkflowState = unit.WorkflowState == "inactive" ? "removed" : unit.WorkflowState;
                        if (unit.WorkflowState == "removed")
                        {
                            removed = true;
                        }

                        await unit.Create(dbContext);
                        if (removed)
                        {
                            await unit.Delete(dbContext);
                        }
                    }
                    else
                    {
                        unit = await dbContext.Units.FirstAsync(x => x.MicrotingUid == unit.MicrotingUid);
                        unit.WorkflowState = item["WorkflowState"].ToString();
                        unit.WorkflowState = unit.WorkflowState == "active" ? "created" : unit.WorkflowState;
                        unit.WorkflowState = unit.WorkflowState == "new" ? "created" : unit.WorkflowState;
                        unit.WorkflowState = unit.WorkflowState == "inactive" ? "removed" : unit.WorkflowState;
                        unit.SiteId = localSiteId;
                        unit.OsVersion = item["OsVersion"].ToString();
                        unit.Manufacturer = item["Manufacturer"].ToString();
                        unit.Model = item["Model"].ToString();
                        unit.CustomerNo = customerNo;
                        await unit.Update(dbContext);
                    }
                }
            }

            await sqlController.SettingUpdate(Settings.knownSitesDone, "true");
            await sqlController.SettingUpdate(Settings.firstRunDone, "true");

            #endregion

            return "";
        }

        public async Task<string> DbSettingsReloadRemote()
        {
            try
            {
                DbContextHelper dbContextHelper = new DbContextHelper(connectionString);
                sqlController = new SqlController(dbContextHelper);
//                sqlController = new SqlController(connectionString);

                string token = await sqlController.SettingRead(Settings.token);
                Communicator communicator = new Communicator(token, @"https://srv05.microting.com",
                    @"https://basic.microting.com", "", "", log, "https://speechtotext.microting.com",
                    connectionString);

                OrganizationDto organizationDto = await communicator.OrganizationLoadAllFromRemote(token);
                await sqlController.SettingUpdate(Settings.token, token);
                await sqlController.SettingUpdate(Settings.comAddressBasic, organizationDto.ComAddressBasic);
                await sqlController.SettingUpdate(Settings.comAddressPdfUpload, organizationDto.ComAddressPdfUpload);
                await sqlController.SettingUpdate(Settings.comAddressApi, organizationDto.ComAddressApi);
                await sqlController.SettingUpdate(Settings.comOrganizationId, organizationDto.Id.ToString());
                await sqlController.SettingUpdate(Settings.awsAccessKeyId, organizationDto.AwsAccessKeyId);
                await sqlController.SettingUpdate(Settings.awsSecretAccessKey, organizationDto.AwsSecretAccessKey);
                await sqlController.SettingUpdate(Settings.awsEndPoint, organizationDto.AwsEndPoint);
                await sqlController.SettingUpdate(Settings.unitLicenseNumber,
                    organizationDto.UnitLicenseNumber.ToString());
                await sqlController.SettingUpdate(Settings.comSpeechToText, organizationDto.ComSpeechToText);
                if (!string.IsNullOrEmpty(organizationDto.S3Endpoint))
                {
                    await sqlController.SettingUpdate(Settings.s3Enabled, "true");
                    await sqlController.SettingUpdate(Settings.s3Endpoint, organizationDto.S3Endpoint);
                    await sqlController.SettingUpdate(Settings.s3AccessKeyId, organizationDto.S3Id);
                    await sqlController.SettingUpdate(Settings.s3SecrectAccessKey, organizationDto.S3Key);
                }

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