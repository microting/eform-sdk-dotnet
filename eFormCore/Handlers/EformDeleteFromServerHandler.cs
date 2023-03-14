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
        private readonly SqlController sqlController;
        private readonly Communicator communicator;
        private readonly Log log;
        private readonly Core core;
        Tools t = new Tools();

        public EformDeleteFromServerHandler(SqlController sqlController, Communicator communicator, Log log, Core core)
        {
            this.sqlController = sqlController;
            this.communicator = communicator;
            this.log = log;
            this.core = core;
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
                    log.LogException(t.GetMethodName("EformDeleteFromServerHandler"),
                        methodName + " (EformDeleteFromServer message) failed, with message.MicrotringUUID " +
                        message.MicrotringUUID, ex);
                }
                catch
                {
                    log.LogException(t.GetMethodName("EformDeleteFromServerHandler"),
                        methodName + " (EformDeleteFromServer message) failed", ex);
                }
            }
        }

        private async Task<bool> DeleteCase(EformDeleteFromServer message)
        {
            int microtingUId = message.MicrotringUUID;
            string methodName = "EformDeleteFromServerHandler";

            log.LogStandard(t.GetMethodName("EformDeleteFromServerHandler"), methodName + " called");
            log.LogVariable(t.GetMethodName("EformDeleteFromServerHandler"), nameof(microtingUId), microtingUId);

            var cDto = await sqlController.CaseReadByMUId(microtingUId);
            string xmlResponse = await communicator.Delete(microtingUId.ToString(), cDto.SiteUId);
            Response resp = new Response();

            if (xmlResponse.Contains("Error occured: Contact Microting"))
            {
                log.LogEverything(t.GetMethodName("EformDeleteFromServerHandler"), "XML response:");
                log.LogEverything(t.GetMethodName("EformDeleteFromServerHandler"), xmlResponse);
                log.LogEverything("DELETE ERROR", methodName + " failed for microtingUId: " + microtingUId);
                return false;
            }

            if (xmlResponse.Contains("Error"))
            {
                try
                {
                    resp = resp.XmlToClass(xmlResponse);
                    log.LogException(t.GetMethodName("EformDeleteFromServerHandler"), methodName + " failed",
                        new Exception("Error from Microting server: " + resp.Value));
                    return false;
                }
                catch (Exception ex)
                {
                    try
                    {
                        log.LogException(t.GetMethodName("EformDeleteFromServerHandler"),
                            methodName + " (string " + microtingUId + ") failed", ex);
                    }
                    catch
                    {
                        log.LogException(t.GetMethodName("EformDeleteFromServerHandler"),
                            methodName + " (string microtingUId) failed", ex);
                    }

                    return false;
                }
            }

            if (xmlResponse.Contains("Parsing in progress: Can not delete check list!</Value>"))
                for (int i = 1; i < 7; i++)
                {
                    Thread.Sleep(i * 200);
                    xmlResponse = await communicator.Delete(microtingUId.ToString(), cDto.SiteUId);
                    if (!xmlResponse.Contains("Parsing in progress: Can not delete check list!</Value>"))
                        break;
                }

            log.LogEverything(t.GetMethodName("EformDeleteFromServerHandler"), "XML response:");
            log.LogEverything(t.GetMethodName("EformDeleteFromServerHandler"), xmlResponse);

            resp = resp.XmlToClass(xmlResponse);
            if (resp.Type.ToString() == "Success")
            {
                try
                {
                    await sqlController.CaseDelete(microtingUId);

                    cDto = await sqlController.CaseReadByMUId(microtingUId);
                    await core.FireHandleCaseDeleted(cDto);

                    log.LogStandard(t.GetMethodName("EformDeleteFromServerHandler"), cDto + " has been removed");

                    return true;
                }
                catch
                {
                }

                try
                {
                    await sqlController.CaseDeleteReversed(microtingUId);

                    cDto = await sqlController.CaseReadByMUId(microtingUId);
                    await core.FireHandleCaseDeleted(cDto);
                    log.LogStandard(t.GetMethodName("EformDeleteFromServerHandler"), cDto + " has been removed");

                    return true;
                }
                catch
                {
                }
            }

            return false;
        }
    }
}