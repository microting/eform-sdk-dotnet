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
using System.Xml;
using eFormCore;
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
        private readonly SqlController sqlController;
        private readonly Communicator communicator;
        private readonly Log log;
        private readonly Core core;
        Tools t = new Tools();

        public EformCompletedHandler(SqlController sqlController, Communicator communicator, Log log, Core core)
        {
            this.sqlController = sqlController;
            this.communicator = communicator;
            this.log = log;
            this.core = core;
        }

        #pragma warning disable 1998
        public async Task Handle(EformCompleted message)
        {
            try
            {
                await CheckStatusByMicrotingUid(message.MicrotringUUID);
                sqlController.NotificationUpdate(message.NotificationUId, message.MicrotringUUID, Constants.WorkflowStates.Processed, "", "");
            } catch (Exception ex)
            {
                sqlController.NotificationUpdate(message.NotificationUId, message.MicrotringUUID, Constants.WorkflowStates.NotFound, ex.Message, ex.StackTrace.ToString());
                Note_Dto note_Dto = new Note_Dto(message.NotificationUId, message.MicrotringUUID, Constants.WorkflowStates.NotFound);
                core.FireHandleNotificationNotFound(note_Dto);
            }
        }

        private async Task<bool> CheckStatusByMicrotingUid(int microtingUid)
        {
            List<Case_Dto> lstCase = new List<Case_Dto>();
            MainElement mainElement = new MainElement();

            Case_Dto concreteCase = sqlController.CaseReadByMUId(microtingUid);
            log.LogEverything(t.GetMethodName("EformCompletedHandler"), concreteCase.ToString() + " has been matched");

            if (concreteCase.CaseUId == "" || concreteCase.CaseUId == "ReversedCase")
                lstCase.Add(concreteCase);
            else
                lstCase = sqlController.CaseReadByCaseUId(concreteCase.CaseUId);

            foreach (Case_Dto aCase in lstCase)
            {
                if (aCase.SiteUId == concreteCase.SiteUId)
                {
                    #region get response's data and update DB with data
                    int? checkIdLastKnown = sqlController.CaseReadLastCheckIdByMicrotingUId(microtingUid); //null if NOT a checkListSite
                    log.LogVariable(t.GetMethodName("EformCompletedHandler"), nameof(checkIdLastKnown), checkIdLastKnown);

                    string respXml;
                    if (checkIdLastKnown == null)
                        respXml = communicator.Retrieve(microtingUid.ToString(), concreteCase.SiteUId);
                    else
                        respXml = communicator.RetrieveFromId(microtingUid.ToString(), concreteCase.SiteUId, checkIdLastKnown.ToString());
                    log.LogVariable(t.GetMethodName("EformCompletedHandler"), nameof(respXml), respXml);

                    Response resp = new Response();
                    resp = resp.XmlToClassUsingXmlDocument(respXml);

                    if (resp.Type == Response.ResponseTypes.Success)
                    {
                        log.LogEverything(t.GetMethodName("EformCompletedHandler"), "resp.Type == Response.ResponseTypes.Success (true)");
                        if (resp.Checks.Count > 0)
                        {
                            XmlDocument xDoc = new XmlDocument();

                            xDoc.LoadXml(respXml);
                            XmlNode checks = xDoc.DocumentElement.LastChild;
                            int i = 0;
                            foreach (Check check in resp.Checks)
                            {

                                int unitUId = sqlController.UnitRead(int.Parse(check.UnitId)).UnitUId;
                                log.LogVariable(t.GetMethodName("EformCompletedHandler"), nameof(unitUId), unitUId);
                                int workerUId = sqlController.WorkerRead(int.Parse(check.WorkerId)).WorkerUId;
                                log.LogVariable(t.GetMethodName("EformCompletedHandler"), nameof(workerUId), workerUId);

                                List<int> uploadedDataIds = await sqlController.ChecksCreate(resp, checks.ChildNodes[i].OuterXml.ToString(), i);

                                foreach (int uploadedDataid in uploadedDataIds)
                                {
                                    if (core.DownloadUploadedData(uploadedDataid))
                                    {
                                        core.TranscribeUploadedData(uploadedDataid);
                                    } else
                                    {
                                        log.LogEverything(t.GetMethodName("Core"), "downloadUploadedData failed for uploadedDataid :" + uploadedDataid.ToString());
                                    }
                                }

                                await sqlController.CaseUpdateCompleted(microtingUid, (int)check.Id, DateTime.Parse(check.Date), workerUId, unitUId);
                                log.LogEverything(t.GetMethodName("EformCompletedHandler"), "sqlController.CaseUpdateCompleted(...)");

                                #region IF needed retract case, thereby completing the process
                                if (checkIdLastKnown == null)
                                {
                                    string responseRetractionXml = communicator.Delete(aCase.MicrotingUId.ToString(), aCase.SiteUId);
                                    Response respRet = new Response();
                                    respRet = respRet.XmlToClass(respXml);

                                    if (respRet.Type == Response.ResponseTypes.Success)
                                    {
                                        log.LogEverything(t.GetMethodName("EformCompletedHandler"), aCase.ToString() + " has been retracted");
                                    }
                                    else
                                        log.LogWarning(t.GetMethodName("EformCompletedHandler"), "Failed to retract eForm MicrotingUId:" + aCase.MicrotingUId + "/SideId:" + aCase.SiteUId + ". Not a critical issue, but needs to be fixed if repeated");
                                }
                                #endregion

                                await sqlController.CaseRetract(microtingUid, (int)check.Id);
                                log.LogEverything(t.GetMethodName("EformCompletedHandler"), "sqlController.CaseRetract(...)");
                                // TODO add case.id
                                Case_Dto cDto = sqlController.CaseReadByMUId(microtingUid);
                                core.FireHandleCaseCompleted(cDto);
                                log.LogStandard(t.GetMethodName("EformCompletedHandler"), cDto.ToString() + " has been completed");
                                i++;
                            }
                        }
                    }
                    else
                    {
                        log.LogEverything(t.GetMethodName("EformCompletedHandler"), "resp.Type == Response.ResponseTypes.Success (false)");
                        throw new Exception("Failed to retrive eForm " + microtingUid + " from site " + aCase.SiteUId);
                    }
                    #endregion
                }
                else
                {
                    await core.CaseDelete((int)aCase.MicrotingUId);
                }
            }
            return true;
        }
    }
}
