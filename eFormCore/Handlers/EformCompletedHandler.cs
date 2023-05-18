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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Communication;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Models;
using Microting.eForm.Infrastructure.Models.reply;
using Microting.eForm.Messages;
using Rebus.Handlers;

namespace Microting.eForm.Handlers
{
    public class EformCompletedHandler : IHandleMessages<EformCompleted>
    {
        private readonly SqlController _sqlController;
        private readonly Communicator _communicator;
        private readonly Log _log;
        private readonly Core _core;
        private readonly Tools _t = new Tools();

        public EformCompletedHandler(SqlController sqlController, Communicator communicator, Log log, Core core)
        {
            _sqlController = sqlController;
            _communicator = communicator;
            _log = log;
            _core = core;
        }

#pragma warning disable 1998
        public async Task Handle(EformCompleted message)
        {
            try
            {
                await CheckStatusByMicrotingUid(message.MicrotringUUID);
                await _sqlController.NotificationUpdate(message.NotificationUId, message.MicrotringUUID,
                    Constants.WorkflowStates.Processed, "", "");
            }
            catch (Exception ex)
            {
                await _sqlController.NotificationUpdate(message.NotificationUId, message.MicrotringUUID,
                    Constants.WorkflowStates.NotFound, ex.Message, ex.StackTrace);
                NoteDto noteDto = new NoteDto(message.NotificationUId, message.MicrotringUUID,
                    Constants.WorkflowStates.NotFound);
                await _core.FireHandleNotificationNotFound(noteDto);
            }
        }

        private async Task<bool> CheckStatusByMicrotingUid(int microtingUid)
        {
            await using MicrotingDbContext dbContext = _core.DbContextHelper.GetDbContext();
            List<CaseDto> lstCase = new List<CaseDto>();

            CaseDto concreteCase = await _sqlController.CaseReadByMUId(microtingUid);
            _log.LogEverything("EformCompletedHandler.CheckStatusByMicrotingUid", concreteCase + " has been matched");

            if (concreteCase.CaseUId == "" || concreteCase.CaseUId == "ReversedCase")
                lstCase.Add(concreteCase);
            else
                lstCase = await _sqlController.CaseReadByCaseUId(concreteCase.CaseUId);

            bool noResults = true;
            foreach (CaseDto aCase in lstCase)
            {
                if (aCase.SiteUId == concreteCase.SiteUId)
                {
                    int? checkIdLastKnown =
                        await _sqlController
                            .CaseReadLastCheckIdByMicrotingUId(microtingUid); //null if NOT a checkListSite
                    _log.LogVariable(_t.GetMethodName("EformCompletedHandler"), nameof(checkIdLastKnown),
                        checkIdLastKnown);

                    string respXml;
                    if (checkIdLastKnown == null)
                    {
                        respXml = await _communicator.Retrieve(microtingUid.ToString(), concreteCase.SiteUId);
                        _log.LogVariable(_t.GetMethodName("EformCompletedHandler"), nameof(respXml), respXml);
                        Response resp = new Response();
                        resp = resp.XmlToClassUsingXmlDocument(respXml);

                        if (resp.Type == Response.ResponseTypes.Success)
                        {
                            _log.LogEverything(_t.GetMethodName("EformCompletedHandler"),
                                "resp.Type == Response.ResponseTypes.Success (true)");
                            if (resp.Checks.Count > 0)
                            {
                                await SaveResult(resp, respXml, dbContext, microtingUid, null, aCase)
                                    .ConfigureAwait(false);
                                noResults = false;
                            }
                        }
                        else
                        {
                            _log.LogEverything(_t.GetMethodName("EformCompletedHandler"),
                                "resp.Type == Response.ResponseTypes.Success (false)");
                            throw new Exception("Failed to retrive eForm " + microtingUid + " from site " +
                                                aCase.SiteUId);
                        }
                    }
                    else
                    {
                        respXml = "";
                        Response resp = null;
                        while (FetchData(microtingUid.ToString(), concreteCase, checkIdLastKnown.ToString(),
                                   ref respXml, ref resp))
                        {
                            checkIdLastKnown =
                                await SaveResult(resp, respXml, dbContext, microtingUid, checkIdLastKnown, aCase)
                                    .ConfigureAwait(false);
                            noResults = false;
                        }
                    }
                }
            }

            if (noResults)
            {
                CaseDto cDto = await _sqlController.CaseReadByMUId(microtingUid);
                Console.WriteLine($"{DateTime.Now} FireHandleCaseCompleted");
                await _core.FireHandleCaseCompleted(cDto);
            }

            return true;
        }

        private bool FetchData(string microtingUid, CaseDto concreteCase, string checkIdLastKnown, ref string respXml,
            ref Response resp)
        {
            respXml = _communicator.RetrieveFromId(microtingUid, concreteCase.SiteUId, checkIdLastKnown).GetAwaiter()
                .GetResult();
            resp = new Response();

            respXml = respXml.Replace("<Response>",
                "<Response xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            respXml = respXml.Replace("<DataItem type=\"comment\"", "<DataItem xsi:type=\"Comment\"");
            respXml = respXml.Replace("<DataItem type=\"picture\"", "<DataItem xsi:type=\"Picture\"");
            respXml = respXml.Replace("<DataItem type=\"audio\"", "<DataItem xsi:type=\"Audio\"");
            resp = resp.XmlToClassUsingXmlDocument(respXml);
            if (resp.Type == Response.ResponseTypes.Success)
            {
                _log.LogEverything(_t.GetMethodName("EformCompletedHandler"),
                    "resp.Type == Response.ResponseTypes.Success (true)");
                if (resp.Checks.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private async Task<int> SaveResult(Response resp, string respXml, MicrotingDbContext dbContext,
            int microtingUid, int? checkIdLastKnown, CaseDto aCase)
        {
            XmlDocument xDoc = new XmlDocument();

            xDoc.LoadXml(respXml);
            int returnId = 0;
            if (xDoc.DocumentElement != null)
            {
                XmlNode checks = xDoc.DocumentElement.LastChild;
                int i = 0;
                foreach (Check check in resp.Checks)
                {
                    var unit = await dbContext.Units.FirstAsync(x => x.MicrotingUid == int.Parse(check.UnitId));
                    unit.Manufacturer = check.Manufacturer;
                    unit.Model = check.Model;
                    unit.OsVersion = check.OsVersion;
                    unit.eFormVersion = check.SoftwareVersion;
                    await unit.Update(dbContext);

                    int? unitUId = unit.MicrotingUid; //sqlController.UnitRead(int.Parse(check.UnitId)).Result.UnitUId;
                    _log.LogVariable(_t.GetMethodName("EformCompletedHandler"), nameof(unitUId), unitUId);
                    int? workerUId = dbContext.Workers
                        .Single(x => x.MicrotingUid == int.Parse(check.WorkerId))
                        .MicrotingUid; //sqlController.WorkerRead(int.Parse(check.WorkerId)).Result.WorkerUId;
                    _log.LogVariable(_t.GetMethodName("EformCompletedHandler"), nameof(workerUId), workerUId);
                    Console.WriteLine($"{DateTime.Now} ChecksCreate start");

                    List<int> uploadedDataIds =
                        await _sqlController.ChecksCreate(resp, checks.ChildNodes[i].OuterXml, i);
                    Console.WriteLine($"{DateTime.Now} ChecksCreate end");

                    Console.WriteLine($"{DateTime.Now} uploadedDataIds");
                    foreach (int uploadedDataid in uploadedDataIds)
                    {
                        if (await _core.DownloadUploadedData(uploadedDataid))
                        {
                            await _core.TranscribeUploadedData(uploadedDataid);
                        }
                        else
                        {
                            _log.LogEverything(_t.GetMethodName("Core"),
                                "downloadUploadedData failed for uploadedDataid :" + uploadedDataid);
                        }
                    }

                    CultureInfo culture = CultureInfo.CreateSpecificCulture("da-DK");
                    DateTime dateTime = DateTime.ParseExact(check.Date, "yyyy-MM-dd HH:mm:ss", culture);
                    _log.LogEverything(_t.GetMethodName("EformCompletedHandler"), $"XML date is {check.Date}");
                    _log.LogEverything(_t.GetMethodName("EformCompletedHandler"),
                        $"Parsed date is {dateTime.ToString(CultureInfo.InvariantCulture)}");

                    await _sqlController.CaseUpdateCompleted(microtingUid, (int)check.Id, dateTime, (int)workerUId,
                        (int)unitUId);
                    _log.LogEverything(_t.GetMethodName("EformCompletedHandler"),
                        "sqlController.CaseUpdateCompleted(...)");

                    // IF needed retract case, thereby completing the process

                    Console.WriteLine($"{DateTime.Now} checkIdLastKnown");
                    if (checkIdLastKnown == null)
                    {
                        await _communicator.Delete(aCase.MicrotingUId.ToString(), aCase.SiteUId);

                        if (respXml.Contains("<Value type=\"success\">"))
                        {
                            _log.LogEverything(_t.GetMethodName("EformCompletedHandler"),
                                aCase + " has been retracted");
                        }
                        else
                            _log.LogWarning(_t.GetMethodName("EformCompletedHandler"),
                                "Failed to retract eForm MicrotingUId:" + aCase.MicrotingUId + "/SideId:" +
                                aCase.SiteUId + ". Not a critical issue, but needs to be fixed if repeated");
                    }

                    await _sqlController.CaseRetract(microtingUid, (int)check.Id);
                    _log.LogEverything(_t.GetMethodName("EformCompletedHandler"), "sqlController.CaseRetract(...)");
                    // TODO add case.id
                    CaseDto cDto = await _sqlController.CaseReadByMUId(microtingUid);
                    Console.WriteLine($"{DateTime.Now} FireHandleCaseCompleted");
                    await _core.FireHandleCaseCompleted(cDto);
                    _log.LogStandard(_t.GetMethodName("EformCompletedHandler"), cDto + " has been completed");
                    i++;
                    returnId = (int)check.Id;
                }
            }

            return returnId;
        }
    }
}