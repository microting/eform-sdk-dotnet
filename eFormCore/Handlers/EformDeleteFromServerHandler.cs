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
using System.Threading;
using System.Threading.Tasks;
using eFormCore;
using Microting.eForm.Communication;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Models.reply;
using Microting.eForm.Messages;
using Rebus.Handlers;

namespace Microting.eForm.Handlers
{
    class EformDeleteFromServerHandler : IHandleMessages<EformDeleteFromServer>
    {
        private readonly SqlController _sqlController;
        private readonly Communicator _communicator;
        private readonly Log _log;
        private readonly Core _core;
        private readonly Tools _t = new Tools();

        public EformDeleteFromServerHandler(SqlController sqlController, Communicator communicator, Log log, Core core)
        {
            _sqlController = sqlController;
            _communicator = communicator;
            _log = log;
            _core = core;
        }

#pragma warning disable 1998
        public async Task Handle(EformDeleteFromServer message)
        {
            string methodName = "EformDeleteFromServer";

            try
            {
                await DeleteCase(message);
            }
            catch (Exception ex)
            {
                try
                {
                    _log.LogException(_t.GetMethodName("EformDeleteFromServerHandler"),
                        methodName + " (EformDeleteFromServer message) failed, with message.MicrotringUUID " +
                        message.MicrotringUUID, ex);
                }
                catch
                {
                    _log.LogException(_t.GetMethodName("EformDeleteFromServerHandler"),
                        methodName + " (EformDeleteFromServer message) failed", ex);
                }
            }
        }

        private async Task DeleteCase(EformDeleteFromServer message)
        {
            int microtingUId = message.MicrotringUUID;
            string methodName = "EformDeleteFromServerHandler";

            _log.LogStandard(_t.GetMethodName("EformDeleteFromServerHandler"), methodName + " called");
            _log.LogVariable(_t.GetMethodName("EformDeleteFromServerHandler"), nameof(microtingUId), microtingUId);

            var cDto = await _sqlController.CaseReadByMUId(microtingUId);
            string xmlResponse = await _communicator.Delete(microtingUId.ToString(), cDto.SiteUId);
            Response resp = new Response();

            if (xmlResponse.Contains("Error occured: Contact Microting"))
            {
                _log.LogEverything(_t.GetMethodName("EformDeleteFromServerHandler"), "XML response:");
                _log.LogEverything(_t.GetMethodName("EformDeleteFromServerHandler"), xmlResponse);
                _log.LogEverything("DELETE ERROR", methodName + " failed for microtingUId: " + microtingUId);
                return;
            }

            if (xmlResponse.Contains("Error"))
            {
                try
                {
                    resp = resp.XmlToClass(xmlResponse);
                    _log.LogException(_t.GetMethodName("EformDeleteFromServerHandler"), methodName + " failed",
                        new Exception("Error from Microting server: " + resp.Value));
                    return;
                }
                catch (Exception ex)
                {
                    try
                    {
                        _log.LogException(_t.GetMethodName("EformDeleteFromServerHandler"),
                            methodName + " (string " + microtingUId + ") failed", ex);
                    }
                    catch
                    {
                        _log.LogException(_t.GetMethodName("EformDeleteFromServerHandler"),
                            methodName + " (string microtingUId) failed", ex);
                    }

                    return;
                }
            }

            if (xmlResponse.Contains("Parsing in progress: Can not delete check list!</Value>"))
                for (int i = 1; i < 7; i++)
                {
                    Thread.Sleep(i * 200);
                    xmlResponse = await _communicator.Delete(microtingUId.ToString(), cDto.SiteUId);
                    if (!xmlResponse.Contains("Parsing in progress: Can not delete check list!</Value>"))
                        break;
                }

            _log.LogEverything(_t.GetMethodName("EformDeleteFromServerHandler"), "XML response:");
            _log.LogEverything(_t.GetMethodName("EformDeleteFromServerHandler"), xmlResponse);

            resp = resp.XmlToClass(xmlResponse);
            if (resp.Type.ToString() == "Success")
            {
                try
                {
                    await _sqlController.CaseDelete(microtingUId);

                    cDto = await _sqlController.CaseReadByMUId(microtingUId);
                    await _core.FireHandleCaseDeleted(cDto);

                    _log.LogStandard(_t.GetMethodName("EformDeleteFromServerHandler"), cDto + " has been removed");

                    return;
                }
                catch
                {
                    // ignored
                }

                try
                {
                    await _sqlController.CaseDeleteReversed(microtingUId);

                    cDto = await _sqlController.CaseReadByMUId(microtingUId);
                    await _core.FireHandleCaseDeleted(cDto);
                    _log.LogStandard(_t.GetMethodName("EformDeleteFromServerHandler"), cDto + " has been removed");
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}