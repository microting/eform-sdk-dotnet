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
using System.IO;
using System.Threading.Tasks;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure.Constants;
using Newtonsoft.Json.Linq;

//using eFormSqlController;

namespace Microting.eForm.Communication
{
    public class Communicator
    {
        #region var

        //SqlController sqlController;
        private Log _log;
        private IHttp _http;
        private Tools _t = new Tools();
        private readonly string _connectionString;

        #endregion

        #region con

        /// <summary>
        /// Microting XML eForm API C# DLL.
        /// </summary>
        /// <param name="token">Your company's XML eForm API access token.</param>
        /// <param name="comAddressApi">Microting's eForm API server address.</param>
        /// <param name="comAddressBasic"></param>
        /// <param name="comOrganizationId">Your company's organization id.</param>
        /// <param name="comAddressPdfUpload"></param>
        /// <param name="log"></param>
        /// <param name="comSpeechToText"></param>
        public Communicator(string token, string comAddressApi, string comAddressBasic, string comOrganizationId,
            string comAddressPdfUpload, Log log, string comSpeechToText, string connectionString)
        {
            //this.sqlController = sqlController;
            _log = log;
            _connectionString = connectionString;

            //string token = sqlController.SettingRead(Settings.token);
            //string comAddressApi = sqlController.SettingRead(Settings.comAddressApi);
            //string comAddressBasic = sqlController.SettingRead(Settings.comAddressBasic);
            //string comOrganizationId = sqlController.SettingRead(Settings.comOrganizationId);
            //string ComAddressPdfUpload = sqlController.SettingRead(Settings.comAddressPdfUpload);

            #region is unit test

            if (token == "abc1234567890abc1234567890abcdef")
            {
                _http = new HttpFake(connectionString);
                return;
            }

            #endregion

            #region CheckInput token & serverAddress

            string errorsFound = "";

            if (token.Length != 32)
                errorsFound += "Tokens are always 32 characters long" + Environment.NewLine;

            if (!comAddressApi.Contains("http://") && !comAddressApi.Contains("https://"))
                errorsFound += "comAddressApi is missing 'http://' or 'https://'" + Environment.NewLine;

            if (!comAddressBasic.Contains("http://") && !comAddressBasic.Contains("https://"))
                errorsFound += "comAddressBasic is missing 'http://' or 'https://'" + Environment.NewLine;

            if (comOrganizationId == null)
                comOrganizationId = "";

            //if (comOrganizationId == "")
            //    errorsFound += "comOrganizationId is missing" + Environment.NewLine;

            if (errorsFound != "")
                throw new InvalidOperationException(errorsFound.TrimEnd());

            #endregion

            _http = new Http(token, comAddressBasic, comAddressApi, comOrganizationId, comAddressPdfUpload,
                comSpeechToText);
        }

        #endregion

        #region public api

        /// <summary>
        /// Posts the XML eForm to Microting and returns the XML encoded restponse (Does not support the complex elements Entity_Search or Entity_Select).
        /// </summary>
        /// <param name="xmlString">XML encoded eForm string.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public Task<string> PostXml(string xmlString, int siteId)
        {
            _log.LogEverything("Communicator.PostXml", "called");

            //TODO - ALL xml hacks
            //XML HACK
            xmlString = xmlString.Replace("<color></color>", "");
            //Missing serverside. Will not accept blank/empty field
            xmlString = xmlString.Replace("<Color />", "");
            //Missing serverside. Will not accept blank/empty field
            xmlString = xmlString.Replace("<DefaultValue>0</DefaultValue>", "<DefaultValue></DefaultValue>");
            xmlString = xmlString.Replace("DefaultValue", "Value");
            //Missing serverside.
            //XML HACK

            _log.LogVariable("Communicator.PostXml", nameof(xmlString), xmlString);
            _log.LogVariable("Communicator.PostXml", nameof(siteId), siteId);

            return _http.Post(xmlString, siteId.ToString());
        }

        public Task<string> PostJson(string json, int siteId)
        {
            _log.LogEverything("Communicator.PostJson", "called");
            _log.LogVariable("Communicator.PostJson", nameof(json), json);
            _log.LogVariable("Communicator.PostJson", nameof(siteId), siteId);

            return _http.Post(json, siteId.ToString(), "application/json");
        }

        /// <summary>
        /// Retrieve the XML encoded status from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve status of.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public Task<string> CheckStatus(string eFormId, int siteId)
        {
            _log.LogEverything("Communicator.CheckStatus", "called");
            _log.LogVariable("Communicator.CheckStatus", nameof(eFormId), eFormId);
            _log.LogVariable("Communicator.CheckStatus", nameof(siteId), siteId);

            return _http.Status(eFormId, siteId.ToString());
        }

        //public bool CheckStatusUpdateIfNeeded(string microtingUId)
        //{
        //    lock (_lockSending)
        //    {
        //        await log.LogEverything("Communicator.", "called");
        //        await log.LogVariable("Communicator.", nameof(microtingUId), microtingUId);

        //        string correctStat = null;
        //        CaseDto caseDto = null;

        //        try
        //        {
        //            caseDto = sqlController.CaseReadByMUId(microtingUId);
        //            correctStat = "Created"; //Sent

        //            string status = CheckStatus(caseDto.MicrotingUId, caseDto.SiteUId);
        //            if (!status.Contains("id=\"\"/>"))
        //            {
        //                correctStat = "Retrived";

        //                string reply = Retrieve(caseDto.MicrotingUId, caseDto.SiteUId);

        //                Response resp = new Response();
        //                resp = resp.XmlToClassUsingXmlDocument(reply);
        //                if (resp.Type == Response.ResponseTypes.Success)
        //                {
        //                    if (resp.Checks.Count > 0)
        //                        correctStat = "Completed";
        //                }

        //                if (caseDto.Stat == "Deleted" || caseDto.Stat == correctStat)
        //                    return true;
        //            }
        //        }
        //        catch
        //        {

        //        }

        //        if (correctStat == "Retrived")
        //            sqlController.NotificationCreate("F:" + t.GetRandomInt(10), caseDto.MicrotingUId, "unit_fetch");

        //        if (correctStat == "Completed")
        //            sqlController.NotificationCreate("F:" + t.GetRandomInt(10), caseDto.MicrotingUId, "check_status");

        //        return false;
        //    }
        //}

        /// <summary>
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve results from.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public Task<string> Retrieve(string eFormId, int siteId)
        {
            _log.LogEverything("Communicator.Retrieve", "called");
            _log.LogVariable("Communicator.Retrieve", nameof(eFormId), eFormId);
            _log.LogVariable("Communicator.Retrieve", nameof(siteId), siteId);

            return _http.Retrieve(eFormId, "0", siteId); //Always gets the first
        }

        /// <summary>
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve results from.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        /// <param name="eFormCheckId">Identifier of the check to begin from.</param>
        public Task<string> RetrieveFromId(string eFormId, int siteId, string eFormCheckId)
        {
            _log.LogEverything("Communicator.RetrieveFromId", "called");
            _log.LogVariable("Communicator.RetrieveFromId", nameof(eFormId), eFormId);
            _log.LogVariable("Communicator.RetrieveFromId", nameof(siteId), siteId);
            _log.LogVariable("Communicator.RetrieveFromId", nameof(eFormCheckId), eFormCheckId);

            return _http.Retrieve(eFormId, eFormCheckId, siteId);
        }

        /// <summary>
        /// Marks an element for deletion and retrieve the XML encoded response from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to mark for deletion.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public Task<string> Delete(string eFormId, int siteId)
        {
            _log.LogEverything("Communicator.Delete", "called");
            _log.LogVariable("Communicator.Delete", nameof(eFormId), eFormId);
            _log.LogVariable("Communicator.Delete", nameof(siteId), siteId);

            return _http.Delete(eFormId, siteId.ToString());
        }

        #endregion

        #region public site

        #region public siteName

        public async Task<Tuple<SiteDto, UnitDto>> SiteCreate(string name, string languageCode)
        {
            _log.LogEverything("Communicator.SiteCreate", "called");
            _log.LogVariable("Communicator.SiteCreate", nameof(name), name);

            string response = await _http.SiteCreate(name, languageCode);
            var parsedSiteData = JToken.Parse(response);

            response = await _http.UnitCreate(int.Parse(parsedSiteData["MicrotingUid"].ToString()));
            var parsedUnitData = JToken.Parse(response);
            int unitId = int.Parse(parsedUnitData["MicrotingUid"].ToString());
            int otpCode = int.Parse(parsedUnitData["OtpCode"].ToString());

            SiteDto siteDto = new SiteDto()
            {
                SiteId = int.Parse(parsedSiteData["MicrotingUid"].ToString()),
                SiteName = parsedSiteData["Name"].ToString(),
                FirstName = "",
                LastName = "",
                CustomerNo = 0,
                OtpCode = 0,
                UnitId = unitId,
                Email = "",
                WorkerUid = 0
            };

            UnitDto unitDto = new UnitDto
            {
                UnitUId = unitId,
                CustomerNo = 0,
                OtpCode = otpCode,
                SiteUId = siteDto.SiteId,
                WorkflowState = Constants.WorkflowStates.Created
            };
            Tuple<SiteDto, UnitDto> result = new Tuple<SiteDto, UnitDto>(siteDto, unitDto);

            return result;
        }

        public Task<bool> SiteUpdate(int siteId, string name, string languageCode)
        {
            _log.LogEverything("Communicator.SiteUpdate", "called");
            _log.LogVariable("Communicator.SiteUpdate", nameof(siteId), siteId);
            _log.LogVariable("Communicator.SiteUpdate", nameof(name), name);

            return _http.SiteUpdate(siteId, name, languageCode);
        }

        public async Task<bool> SiteDelete(int siteId)
        {
            _log.LogEverything("Communicator.SiteDelete", "called");
            _log.LogVariable("Communicator.SiteDelete", nameof(siteId), siteId);

            string response = await _http.SiteDelete(siteId);
            var parsedData = JToken.Parse(response);

            if (parsedData["WorkflowState"].ToString() == Constants.WorkflowStates.Removed)
            {
                return true;
            }

            return false;
        }

        public Task<string> SiteLoadAllFromRemote()
        {
            _log.LogEverything("Communicator.SiteLoadAllFromRemote", "called");

            return _http.SiteLoadAllFromRemote();
        }

        #endregion

        #region public worker

        public async Task<WorkerDto> WorkerCreate(string firstName, string lastName, string email)
        {
            _log.LogEverything("Communicator.WorkerCreate", "called");
            _log.LogVariable("Communicator.WorkerCreate", nameof(firstName), firstName);
            _log.LogVariable("Communicator.WorkerCreate", nameof(lastName), lastName);
            _log.LogVariable("Communicator.WorkerCreate", nameof(email), email);

            string result = await _http.WorkerCreate(firstName, lastName, email);
            var parsedData = JToken.Parse(result);
            int workerUid = int.Parse(parsedData["MicrotingUid"].ToString());
            DateTime? createdAt = DateTime.Parse(parsedData["CreatedAt"].ToString());
            DateTime? updatedAt = DateTime.Parse(parsedData["UpdatedAt"].ToString());
            return new WorkerDto
            {
                WorkerUId = workerUid,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt
            };
        }

        public Task<bool> WorkerUpdate(int workerId, string firstName, string lastName, string email)
        {
            _log.LogEverything("Communicator.WorkerUpdate", "called");
            _log.LogVariable("Communicator.WorkerUpdate", nameof(workerId), workerId);
            _log.LogVariable("Communicator.WorkerUpdate", nameof(firstName), firstName);
            _log.LogVariable("Communicator.WorkerUpdate", nameof(lastName), lastName);
            _log.LogVariable("Communicator.WorkerUpdate", nameof(email), email);

            return _http.WorkerUpdate(workerId, firstName, lastName, email);
        }

        public async Task<bool> WorkerDelete(int workerId)
        {
            _log.LogEverything("Communicator.WorkerDelete", "called");
            _log.LogVariable("Communicator.WorkerDelete", nameof(workerId), workerId);

            string response = await _http.WorkerDelete(workerId);
            var parsedData = JToken.Parse(response);

            if (parsedData["WorkflowState"].ToString() == Constants.WorkflowStates.Removed)
                return true;
            return false;
        }

        public Task<string> WorkerLoadAllFromRemote()
        {
            _log.LogEverything("Communicator.WorkerLoadAllFromRemote", "called");

            return _http.WorkerLoadAllFromRemote();
        }

        #endregion

        #region public site_worker

        public async Task<SiteWorkerDto> SiteWorkerCreate(int siteId, int workerId)
        {
            _log.LogEverything("Communicator.SiteWorkerCreate", "called");
            _log.LogVariable("Communicator.SiteWorkerCreate", nameof(siteId), siteId);
            _log.LogVariable("Communicator.SiteWorkerCreate", nameof(workerId), workerId);

            string result = await _http.SiteWorkerCreate(siteId, workerId);
            var parsedData = JToken.Parse(result);
            int workerUid = int.Parse(parsedData["MicrotingUid"].ToString());
            return new SiteWorkerDto(workerUid, siteId, workerId);
        }

        public async Task<bool> SiteWorkerDelete(int workerId)
        {
            _log.LogEverything("Communicator.SiteWorkerDelete", "called");
            _log.LogVariable("Communicator.SiteWorkerDelete", nameof(workerId), workerId);

            string response = await _http.SiteWorkerDelete(workerId);
            var parsedData = JToken.Parse(response);

            if (parsedData["WorkflowState"].ToString() == Constants.WorkflowStates.Removed)
                return true;
            return false;
        }

        public Task<string> SiteWorkerLoadAllFromRemote()
        {
            _log.LogEverything("Communicator.SiteWorkerLoadAllFromRemote", "called");

            return _http.SiteWorkerLoadAllFromRemote();
        }

        #endregion

        #region public unit

        public Task<string> UnitRequestOtp(int microtingUid, int siteId, bool newOtp, bool pushEnabled,
            bool syncDelayEnabled, bool syncDialogEnabled)
        {
            _log.LogEverything("Communicator.UnitRequestOtp", "called");
            _log.LogVariable("Communicator.UnitRequestOtp", nameof(microtingUid), microtingUid);

            return _http.UnitUpdate(microtingUid, newOtp, siteId, pushEnabled, syncDelayEnabled, syncDialogEnabled);
        }

        public Task<string> UnitLoadAllFromRemote(int customerNo)
        {
            _log.LogEverything("Communicator.UnitLoadAllFromRemote", "called");
            _log.LogVariable("Communicator.UnitLoadAllFromRemote", nameof(customerNo), customerNo);

            return _http.UnitLoadAllFromRemote();
        }

        public async Task<bool> UnitDelete(int unitId)
        {
            _log.LogEverything("Communicator.UnitDelete", "called");
            _log.LogVariable("Communicator.UnitDelete", nameof(unitId), unitId);

            string response = await _http.UnitDelete(unitId);
            var parsedData = JToken.Parse(response);

            if (parsedData["workflow_state"].ToString() == Constants.WorkflowStates.Removed)
            {
                return true;
            }

            return false;
        }

        public async Task<string> UnitCreate(int siteMicrotingUid)
        {
            string methodName = "Communicator.UnitCreate";

            _log.LogEverything(methodName, "called");
            _log.LogVariable(methodName, nameof(siteMicrotingUid), siteMicrotingUid);

            string response = await _http.UnitCreate(siteMicrotingUid);

            if (response.Contains(Constants.WorkflowStates.Active))
            {
                return response;
            }

            throw new Exception("Unable to create the unit");
        }

        public Task<string> UnitMove(int unitMicrotingUid, int siteMicrotingUid)
        {
            string methodName = "Communicator.UnitMove";

            _log.LogEverything(methodName, "called");
            _log.LogVariable(methodName, nameof(unitMicrotingUid), unitMicrotingUid);
            _log.LogVariable(methodName, nameof(siteMicrotingUid), siteMicrotingUid);

            return _http.UnitMove(unitMicrotingUid, siteMicrotingUid);
        }

        #endregion

        #region public organization

        public async Task<OrganizationDto> OrganizationLoadAllFromRemote(string token)
        {
            _log.LogEverything("Communicator.OrganizationLoadAllFromRemote", "called");
            _log.LogVariable("Communicator.OrganizationLoadAllFromRemote", nameof(token), token);
            IHttp specialHttp;
            if (token == "abc1234567890abc1234567890abcdef")
            {
                specialHttp = new HttpFake(_connectionString);
            }
            else
            {
                specialHttp = new Http(token, "https://basic.microting.com", "https://srv05.microting.com", "000", "",
                    "https://speechtotext.microting.com");
            }

            JToken orgResult = JToken.Parse(await specialHttp.OrganizationLoadAllFromRemote());

            OrganizationDto organizationDto = new OrganizationDto(int.Parse(orgResult.First.First["Id"].ToString()),
                orgResult.First.First["Name"].ToString(),
                int.Parse(orgResult.First.First["CustomerNo"].ToString()),
                int.Parse(orgResult.First.First["UnitLicenseNumber"].ToString()),
                orgResult.First.First["AwsId"].ToString(),
                orgResult.First.First["AwsKey"].ToString(),
                orgResult.First.First["AwsEndpoint"].ToString(),
                orgResult.First.First["ComAddress"].ToString(),
                orgResult.First.First["ComAddressBasic"].ToString(),
                orgResult.First.First["ComSpeechToText"].ToString(),
                orgResult.First.First["ComAddressPdfUpload"].ToString());
            organizationDto.S3Endpoint = orgResult.First.First["S3EndPoint"].ToString();
            organizationDto.S3Id = orgResult.First.First["S3Id"].ToString();
            organizationDto.S3Key = orgResult.First.First["S3Key"].ToString();

            return organizationDto;
        }

        #endregion

        #region folder

        public async Task<string> FolderLoadAllFromRemote()
        {
            _log.LogEverything("Communicator.FolderLoadAllFromRemote", "called");

            return await _http.FolderLoadAllFromRemote().ConfigureAwait(false);
        }

        public async Task<int> FolderCreate(int uuid, int? parentId)
        {
            var parsedData = JToken.Parse(await _http.FolderCreate(uuid, parentId).ConfigureAwait(false));

            int microtingUuid = int.Parse(parsedData["MicrotingUid"].ToString());

            return microtingUuid;
        }

        public Task FolderUpdate(int id, string name, string description, string languageCode, int? parentId)
        {
            return _http.FolderUpdate(id, name, description, languageCode, parentId);
        }

        public async Task<bool> FolderDelete(int id)
        {
            string response = await _http.FolderDelete(id).ConfigureAwait(false);
            var parsedData = JToken.Parse(response);

            if (parsedData["WorkflowState"].ToString() == Constants.WorkflowStates.Removed)
            {
                return true;
            }

            return false;
        }

        #endregion

        #endregion

        #region speech2text

        public int uploadAudioFile(string path)
        {
            return 1;
        }

        public string getTranscription(int id)
        {
            return "";
        }

        #endregion

        #region public entity

        public async Task<string> EntityGroupCreate(string entityType, string name, string id)
        {
            _log.LogEverything("Communicator.EntityGroupCreate", "called");
            _log.LogVariable("Communicator.EntityGroupCreate", nameof(entityType), entityType);
            _log.LogVariable("Communicator.EntityGroupCreate", nameof(name), name);
            _log.LogVariable("Communicator.EntityGroupCreate", nameof(id), id);

            try
            {
                if (entityType == Constants.FieldTypes.EntitySearch)
                {
                    string microtingUId = await _http.EntitySearchGroupCreate(name, id);

                    if (microtingUId == null)
                        throw new Exception("EntityGroupCreate failed, due to microtingUId:'null'");
                    return microtingUId;
                }

                if (entityType == Constants.FieldTypes.EntitySelect)
                {
                    string microtingUId = await _http.EntitySelectGroupCreate(name, id);

                    if (microtingUId == null)
                        throw new Exception("EntityGroupCreate failed, due to microtingUId:'null'");
                    return microtingUId;
                }

                throw new Exception("entityType:'" + entityType + "' not known");
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupCreate failed", ex);
            }
        }

        public async Task<bool> EntityGroupUpdate(string entityType, string name, int id, string entityGroupMuId)
        {
            _log.LogEverything("Communicator.EntityGroupUpdate", "called");
            _log.LogVariable("Communicator.EntityGroupUpdate", nameof(entityType), entityType);
            _log.LogVariable("Communicator.EntityGroupUpdate", nameof(name), name);
            _log.LogVariable("Communicator.EntityGroupUpdate", nameof(id), id);
            _log.LogVariable("Communicator.EntityGroupUpdate", nameof(entityGroupMuId), entityGroupMuId);

            try
            {
                if (entityType == Constants.FieldTypes.EntitySearch)
                {
                    if (await _http.EntitySearchGroupUpdate(id, name, entityGroupMuId))
                        return true;
                    throw new Exception("EntityGroupUpdate failed");
                }

                if (entityType == Constants.FieldTypes.EntitySelect)
                {
                    if (await _http.EntitySelectGroupUpdate(id, name, entityGroupMuId))
                        return true;
                    throw new Exception("EntityGroupUpdate failed");
                }

                throw new Exception("entityType:'" + entityType + "' not known");
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupUpdate failed", ex);
            }
        }

        public async Task EntityGroupDelete(string entityType, string entityGroupId)
        {
            _log.LogEverything("Communicator.EntityGroupDelete", "called");
            _log.LogVariable("Communicator.EntityGroupDelete", nameof(entityType), entityType);
            _log.LogVariable("Communicator.EntityGroupDelete", nameof(entityGroupId), entityGroupId);

            try
            {
                if (entityType == Constants.FieldTypes.EntitySearch)
                {
                    if (await _http.EntitySearchGroupDelete(entityGroupId))
                        return;
                    throw new Exception("EntityGroupDelete failed");
                }

                if (entityType == Constants.FieldTypes.EntitySelect)
                {
                    if (await _http.EntitySelectGroupDelete(entityGroupId))
                        return;
                    throw new Exception("EntityGroupDelete failed");
                }

                throw new Exception("entityType:'" + entityType + "' not known");
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupDelete failed", ex);
            }
        }

        public async Task<string> EntitySearchItemCreate(string entitySearchGroupId, string name, string description,
            string id)
        {
            _log.LogEverything("Communicator.EntitySearchItemCreate", "called");
            _log.LogVariable("Communicator.EntitySearchItemCreate", nameof(entitySearchGroupId), entitySearchGroupId);
            _log.LogVariable("Communicator.EntitySearchItemCreate", nameof(name), name);
            _log.LogVariable("Communicator.EntitySearchItemCreate", nameof(id), id);
            _log.LogVariable("Communicator.EntitySearchItemCreate", nameof(description), description);

            try
            {
                return await _http.EntitySearchItemCreate(entitySearchGroupId, name, description, id);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemCreate failed", ex);
            }
        }

        public Task<bool> EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name,
            string description, string id)
        {
            _log.LogEverything("Communicator.EntitySearchItemUpdate", "called");
            _log.LogVariable("Communicator.EntitySearchItemUpdate", nameof(entitySearchGroupId), entitySearchGroupId);
            _log.LogVariable("Communicator.EntitySearchItemUpdate", nameof(entitySearchItemId), entitySearchItemId);
            _log.LogVariable("Communicator.EntitySearchItemUpdate", nameof(name), name);
            _log.LogVariable("Communicator.EntitySearchItemUpdate", nameof(id), id);
            _log.LogVariable("Communicator.EntitySearchItemUpdate", nameof(description), description);

            return _http.EntitySearchItemUpdate(entitySearchGroupId, entitySearchItemId, name, description, id);
        }

        public async Task<bool> EntitySearchItemDelete(string entitySearchItemId)
        {
            _log.LogEverything("Communicator.EntitySearchItemDelete", "called");
            _log.LogVariable("Communicator.EntitySearchItemDelete", nameof(entitySearchItemId), entitySearchItemId);

            try
            {
                return await _http.EntitySearchItemDelete(entitySearchItemId);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemDelete failed", ex);
            }
        }

        public async Task<string> EntitySelectItemCreate(string entitySearchGroupId, string name, int displayOrder,
            string ownUUID)
        {
            _log.LogEverything("Communicator.EntitySelectItemCreate", "called");
            _log.LogVariable("Communicator.EntitySelectItemCreate", nameof(entitySearchGroupId), entitySearchGroupId);
            _log.LogVariable("Communicator.EntitySelectItemCreate", nameof(name), name);
            _log.LogVariable("Communicator.EntitySelectItemCreate", nameof(displayOrder), displayOrder);
            _log.LogVariable("Communicator.EntitySelectItemCreate", nameof(ownUUID), ownUUID);

            try
            {
                return await _http.EntitySelectItemCreate(entitySearchGroupId, name, displayOrder, ownUUID);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemCreate failed", ex);
            }
        }

        public Task<bool> EntitySelectItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name,
            int displayOrder, string ownUUID)
        {
            _log.LogEverything("Communicator.EntitySelectItemUpdate", "called");
            _log.LogVariable("Communicator.EntitySelectItemUpdate", nameof(entitySearchGroupId), entitySearchGroupId);
            _log.LogVariable("Communicator.EntitySelectItemUpdate", nameof(entitySearchItemId), entitySearchItemId);
            _log.LogVariable("Communicator.EntitySelectItemUpdate", nameof(name), name);
            _log.LogVariable("Communicator.EntitySelectItemUpdate", nameof(displayOrder), displayOrder);
            _log.LogVariable("Communicator.EntitySelectItemUpdate", nameof(ownUUID), ownUUID);

            return _http.EntitySelectItemUpdate(entitySearchGroupId, entitySearchItemId, name, displayOrder, ownUUID);
        }

        public async Task<bool> EntitySelectItemDelete(string entitySearchItemId)
        {
            _log.LogEverything("Communicator.EntitySelectItemDelete", "called");
            _log.LogVariable("Communicator.EntitySelectItemDelete", nameof(entitySearchItemId), entitySearchItemId);

            try
            {
                return await _http.EntitySelectItemDelete(entitySearchItemId);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemDelete failed", ex);
            }
        }

        public async Task<bool> PdfUpload(string localPath, string hash)
        {
            _log.LogEverything("Communicator.PdfUpload", "called");
            _log.LogVariable("Communicator.PdfUpload", nameof(localPath), localPath);
            _log.LogVariable("Communicator.PdfUpload", nameof(hash), hash);

            try
            {
                return await _http.PdfUpload(localPath, hash);
            }
            catch (Exception ex)
            {
                throw new Exception("Communicator.PdfUpload" + " failed", ex);
            }
        }

        public async Task<bool> PdfUpload(Stream stream, string hash, string fileName)
        {
            _log.LogEverything("Communicator.PdfUpload", "called");
            _log.LogVariable("Communicator.PdfUpload", nameof(fileName), fileName);
            _log.LogVariable("Communicator.PdfUpload", nameof(hash), hash);

            try
            {
                return await _http.PdfUpload(stream, hash, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception("Communicator.PdfUpload" + " failed", ex);
            }
        }

        public async Task<string> TemplateDisplayIndexChange(string microtingUId, int siteId, int newDisplayIndex)
        {
            _log.LogEverything("Communicator.TemplateDisplayIndexChange", "called");
            _log.LogVariable("Communicator.TemplateDisplayIndexChange", nameof(microtingUId), microtingUId);
            _log.LogVariable("Communicator.TemplateDisplayIndexChange", nameof(siteId), siteId);
            _log.LogVariable("Communicator.TemplateDisplayIndexChange", nameof(newDisplayIndex), newDisplayIndex);

            try
            {
                return await _http.TemplateDisplayIndexChange(microtingUId, siteId, newDisplayIndex);
            }
            catch (Exception ex)
            {
                throw new Exception("Communicator.TemplateDisplayIndexChange" + " failed", ex);
            }
        }

        #endregion

        #region public speechToText

        public async Task<int> SpeechToText(Stream pathToAudioFile, string extension)
        {
            _log.LogEverything("Communicator.SpeechToText", "called");

            try
            {
                return await _http.SpeechToText(pathToAudioFile, "da-DK", extension);
            }
            catch (Exception ex)
            {
                throw new Exception("Communicator.SpeechToText" + " failed", ex);
            }
        }

        public async Task<JToken> SpeechToText(int requestId)
        {
            _log.LogEverything("Communicator.SpeechToText", "called");
            _log.LogVariable("Communicator.SpeechToText", nameof(requestId), requestId);

            try
            {
                return await _http.SpeechToText(requestId);
            }
            catch (Exception ex)
            {
                throw new Exception("Communicator.SpeechToText" + " failed", ex);
            }
        }

        #endregion

        #region InSight

        #region SurveyConfiguration

        public async Task<bool> SetSurveyConfiguration(int id, int siteId, bool addSite)
        {
            _log.LogEverything("Communicator.SetSurveyConfiguration", "called");

            try
            {
                return await _http.SetSurveyConfiguration(id, siteId, addSite);
            }
            catch (Exception ex)
            {
                throw new Exception("Communicator.SetSurveyConfiguration" + " failed", ex);
            }
        }

        public async Task<string> GetAllSurveyConfigurations()
        {
            _log.LogEverything("Communicator.GetAllSurveyConfigurations", "called");

            try
            {
                return await _http.GetAllSurveyConfigurations();
            }
            catch (Exception ex)
            {
                throw new Exception("Communicator.GetAllSurveyConfigurations" + " failed", ex);
            }
        }

        public async Task<string> GetSurveyConfiguration(int id)
        {
            _log.LogEverything("Communicator.GetSurveyConfiguration", "called");

            try
            {
                return await _http.GetSurveyConfiguration(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Communicator.GetSurveyConfiguration" + " failed", ex);
            }
        }

        #endregion

        #region QuestionSet

        public async Task<string> GetAllQuestionSets()
        {
            _log.LogEverything("Communicator.GetAllQuestionSets", "called");

            try
            {
                return await _http.GetAllQuestionSets();
            }
            catch (Exception ex)
            {
                throw new Exception("Communicator.GetAllQuestionSets" + " failed", ex);
            }
        }

        public async Task<string> GetQuestionSet(int id)
        {
            _log.LogEverything("Communicator.GetQuestionSet", "called");

            try
            {
                return await _http.GetQuestionSet(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Communicator.GetQuestionSet" + " failed", ex);
            }
        }

        #endregion

        #region Answer

        public async Task<string> GetLastAnswer(int questionSetId, int lastAnswerId)
        {
            _log.LogEverything("Communicator.GetLastAnswer", "called");

            try
            {
                return await _http.GetLastAnswer(questionSetId, lastAnswerId);
            }
            catch (Exception ex)
            {
                throw new Exception("Communicator.GetLastAnswer" + " failed", ex);
            }
        }

        #endregion

        #endregion

        public async Task SendPushMessage(int microtingSiteId, string header, string body, int microtingUuid)
        {
            await _http.SendPushMessage(microtingSiteId, header, body, microtingUuid);
        }
    }
}