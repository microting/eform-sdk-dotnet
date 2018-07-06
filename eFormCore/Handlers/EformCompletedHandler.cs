using eForm.Messages;
using eFormSqlController;
using Rebus.Handlers;
using System;
using eFormShared;
using System.Threading.Tasks;
using System.Collections.Generic;
using eFormData;
using eFormCommunicator;
using System.Xml;

namespace eFormCore.Handlers
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
                CheckStatusByMicrotingUid(message.MicrotringUUID);
                sqlController.NotificationUpdate(message.NotificationUId, message.MicrotringUUID, Constants.WorkflowStates.Processed, "", "");
            } catch (Exception ex)
            {
                sqlController.NotificationUpdate(message.NotificationUId, message.MicrotringUUID, Constants.WorkflowStates.NotFound, ex.Message, ex.StackTrace.ToString());
                Note_Dto note_Dto = new Note_Dto(message.NotificationUId, message.MicrotringUUID, Constants.WorkflowStates.NotFound);
                core.FireHandleNotificationNotFound(note_Dto);
            }
        }

        private bool CheckStatusByMicrotingUid(string microtingUid)
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
                    string checkIdLastKnown = sqlController.CaseReadLastCheckIdByMicrotingUId(microtingUid); //null if NOT a checkListSite
                    log.LogVariable(t.GetMethodName("EformCompletedHandler"), nameof(checkIdLastKnown), checkIdLastKnown);

                    string respXml;
                    if (checkIdLastKnown == null)
                        respXml = communicator.Retrieve(microtingUid, concreteCase.SiteUId);
                    else
                        respXml = communicator.RetrieveFromId(microtingUid, concreteCase.SiteUId, checkIdLastKnown);
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

                                List<int> uploadedDataIds = sqlController.ChecksCreate(resp, checks.ChildNodes[i].OuterXml.ToString(), i);

                                foreach (int uploadedDataid in uploadedDataIds)
                                {
                                    if (core.downloadUploadedData(uploadedDataid))
                                    {
                                        core.TranscribeUploadedData(uploadedDataid);
                                    } else
                                    {
                                        log.LogEverything(t.GetMethodName("Core"), "downloadUploadedData failed for uploadedDataid :" + uploadedDataid.ToString());
                                    }
                                }

                                sqlController.CaseUpdateCompleted(microtingUid, check.Id, DateTime.Parse(check.Date), workerUId, unitUId);
                                log.LogEverything(t.GetMethodName("EformCompletedHandler"), "sqlController.CaseUpdateCompleted(...)");

                                #region IF needed retract case, thereby completing the process
                                if (checkIdLastKnown == null)
                                {
                                    string responseRetractionXml = communicator.Delete(aCase.MicrotingUId, aCase.SiteUId);
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

                                sqlController.CaseRetract(microtingUid, check.Id);
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
                    core.CaseDelete(aCase.MicrotingUId);
                }
            }
            return true;
        }
    }
}
