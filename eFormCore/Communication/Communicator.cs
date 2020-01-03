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
using System.ComponentModel;
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
        Log log;
        IHttp http;
        Tools t = new Tools();
        #endregion

        #region con
        /// <summary>
        /// Microting XML eForm API C# DLL.
        /// </summary>
        /// <param name="token">Your company's XML eForm API access token.</param>
        /// <param name="comAddressApi">Microting's eForm API server address.</param>
        /// <param name="comOrganizationId">Your company's organization id.</param>
        public Communicator(string token, string comAddressApi, string comAddressBasic, string comOrganizationId, string ComAddressPdfUpload, Log log, string ComSpeechToText)
        {
            //this.sqlController = sqlController;
            this.log = log;

            //string token = sqlController.SettingRead(Settings.token);
            //string comAddressApi = sqlController.SettingRead(Settings.comAddressApi);
            //string comAddressBasic = sqlController.SettingRead(Settings.comAddressBasic);
            //string comOrganizationId = sqlController.SettingRead(Settings.comOrganizationId);
            //string ComAddressPdfUpload = sqlController.SettingRead(Settings.comAddressPdfUpload);

            #region is unit test
            if (token == "abc1234567890abc1234567890abcdef")
            {
                http = new HttpFake();
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

            http = new Http(token, comAddressBasic, comAddressApi, comOrganizationId, ComAddressPdfUpload, ComSpeechToText);
        }
        #endregion

        #region public api
        /// <summary>
        /// Posts the XML eForm to Microting and returns the XML encoded restponse (Does not support the complex elements Entity_Search or Entity_Select).
        /// </summary>
        /// <param name="xmlString">XML encoded eForm string.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public async Task<string> PostXml(string xmlString, int siteId)
        {
//            lock (_lockSending)
//            {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");

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

            await log.LogVariable(t.GetMethodName("Communicator"), nameof(xmlString), xmlString);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(siteId), siteId);

            return await http.Post(xmlString, siteId.ToString());
//            }
        }

        /// <summary>
        /// Retrieve the XML encoded status from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve status of.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public async Task<string> CheckStatus(string eFormId, int siteId)
        {
//            lock (_lockSending)
//            {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(eFormId), eFormId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(siteId), siteId);

            return await http.Status(eFormId, siteId.ToString());
//            }
        }

        //public bool CheckStatusUpdateIfNeeded(string microtingUId)
        //{
        //    lock (_lockSending)
        //    {
        //        await log.LogEverything(t.GetMethodName("Communicator"), "called");
        //        await log.LogVariable(t.GetMethodName("Communicator"), nameof(microtingUId), microtingUId);

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
        public async Task<string> Retrieve(string eFormId, int siteId)
        {
//            lock (_lockSending)
//            {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(eFormId), eFormId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(siteId), siteId);

            return await http.Retrieve(eFormId, "0", siteId); //Always gets the first
//            }
        }

        /// <summary>
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve results from.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        /// <param name="eFormCheckId">Identifier of the check to begin from.</param>
        public async Task<string> RetrieveFromId(string eFormId, int siteId, string eFormCheckId)
        {
//            lock (_lockSending)
//            {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(eFormId), eFormId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(siteId), siteId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(eFormCheckId), eFormCheckId);

            return await http.Retrieve(eFormId, eFormCheckId, siteId);
//            }
        }

        /// <summary>
        /// Marks an element for deletion and retrieve the XML encoded response from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to mark for deletion.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public async Task<string> Delete(string eFormId, int siteId)
        {
//            lock (_lockSending)
//            {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(eFormId), eFormId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(siteId), siteId);

            return await http.Delete(eFormId, siteId.ToString());
//            }
        }
        #endregion

        #region public site
        #region public siteName
        public async Task<Tuple<SiteDto, UnitDto>> SiteCreate(string name)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(name), name);

            string response = await http.SiteCreate(name);
            var parsedData = JRaw.Parse(response);

            int unitId = int.Parse(parsedData["unit_id"].ToString());
            int otpCode = int.Parse(parsedData["otp_code"].ToString());
            SiteDto siteDto = new SiteDto(int.Parse(parsedData["id"].ToString()), parsedData["name"].ToString(), "", "", 0, 0, unitId, 0); // WorkerUid is set to 0, because it's used in this context.
            UnitDto unitDto = new UnitDto()
            {
                UnitUId = unitId,
                CustomerNo = 0,
                OtpCode = otpCode,
                SiteUId = siteDto.SiteId,
                CreatedAt = DateTime.Parse(parsedData["created_at"].ToString()),
                UpdatedAt = DateTime.Parse(parsedData["updated_at"].ToString()),
                WorkflowState = Constants.WorkflowStates.Created
            };
            Tuple<SiteDto, UnitDto> result = new Tuple<SiteDto, UnitDto>(siteDto, unitDto);

            return result;
        }

        public async Task<bool> SiteUpdate(int siteId, string name)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(siteId), siteId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(name), name);

            return await http.SiteUpdate(siteId, name);
        }

        public async Task<bool> SiteDelete(int siteId)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(siteId), siteId);

            string response = await http.SiteDelete(siteId);
            var parsedData = JRaw.Parse(response);

            if (parsedData["workflow_state"].ToString() == Constants.WorkflowStates.Removed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<SiteNameDto>> SiteLoadAllFromRemote()
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");

            var parsedData = JRaw.Parse(await http.SiteLoadAllFromRemote());
            List<SiteNameDto> lst = new List<SiteNameDto>();

            foreach (JToken item in parsedData)
            {
                string name = item["name"].ToString();
                int microtingUid = int.Parse(item["id"].ToString());
                DateTime? createdAt = DateTime.Parse(item["created_at"].ToString());
                DateTime? updatedAt = DateTime.Parse(item["updated_at"].ToString());
                SiteNameDto temp = new SiteNameDto(microtingUid, name, createdAt, updatedAt);
                lst.Add(temp);
            }
            return lst;
        }
        #endregion

        #region public worker
        public async Task<WorkerDto> WorkerCreate(string firstName, string lastName, string email)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(firstName), firstName);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(lastName), lastName);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(email), email);

            string result = await http.WorkerCreate(firstName, lastName, email);
            var parsedData = JRaw.Parse(result);
            int workerUid = int.Parse(parsedData["id"].ToString());
            DateTime? createdAt = DateTime.Parse(parsedData["created_at"].ToString());
            DateTime? updatedAt = DateTime.Parse(parsedData["updated_at"].ToString());
            return new WorkerDto(workerUid, firstName, lastName, email, createdAt, updatedAt);
        }

        public async Task<bool> WorkerUpdate(int workerId, string firstName, string lastName, string email)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(workerId), workerId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(firstName), firstName);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(lastName), lastName);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(email), email);

            return await http.WorkerUpdate(workerId, firstName, lastName, email);
        }

        public async Task<bool> WorkerDelete(int workerId)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(workerId), workerId);

            string response = await http.WorkerDelete(workerId);
            var parsedData = JRaw.Parse(response);

            if (parsedData["workflow_state"].ToString() == Constants.WorkflowStates.Removed)
                return true;
            else
                return false;
        }

        public async Task<List<WorkerDto>> WorkerLoadAllFromRemote()
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");

            var parsedData = JRaw.Parse(await http.WorkerLoadAllFromRemote());
            List<WorkerDto> lst = new List<WorkerDto>();

            foreach (JToken item in parsedData)
            {
                string firstName = item["first_name"].ToString();
                string lastName = item["last_name"].ToString();
                string email = item["email"].ToString();
                int microtingUid = int.Parse(item["id"].ToString());
                DateTime? createdAt = DateTime.Parse(item["created_at"].ToString());
                DateTime? updatedAt = DateTime.Parse(item["updated_at"].ToString());
                WorkerDto temp = new WorkerDto(microtingUid, firstName, lastName, email, createdAt, updatedAt);
                lst.Add(temp);
            }
            return lst;
        }
        #endregion

        #region public site_worker
        public async Task<SiteWorkerDto> SiteWorkerCreate(int siteId, int workerId)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(siteId), siteId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(workerId), workerId);

            string result = await http.SiteWorkerCreate(siteId, workerId);
            var parsedData = JRaw.Parse(result);
            int workerUid = int.Parse(parsedData["id"].ToString());
            return new SiteWorkerDto(workerUid, siteId, workerId);
        }

        public async Task<bool> SiteWorkerDelete(int workerId)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(workerId), workerId);

            string response = await http.SiteWorkerDelete(workerId);
            var parsedData = JRaw.Parse(response);

            if (parsedData["workflow_state"].ToString() == Constants.WorkflowStates.Removed)
                return true;
            else
                return false;
        }

        public async Task<List<SiteWorkerDto>> SiteWorkerLoadAllFromRemote()
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");

            var parsedData = JRaw.Parse(await http.SiteWorkerLoadAllFromRemote());
            List<SiteWorkerDto> lst = new List<SiteWorkerDto>();

            foreach (JToken item in parsedData)
            {
                int microtingUid = int.Parse(item["id"].ToString());
                int siteUId = int.Parse(item["site_id"].ToString());
                int workerUId = int.Parse(item["user_id"].ToString());
                SiteWorkerDto temp = new SiteWorkerDto(microtingUid, siteUId, workerUId);
                lst.Add(temp);
            }
            return lst;
        }
        #endregion

        #region public unit      
        public async Task<int> UnitRequestOtp(int microtingUid)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(microtingUid), microtingUid);

            return await http.UnitRequestOtp(microtingUid);
        }

        public async Task<List<UnitDto>> UnitLoadAllFromRemote(int customerNo)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(customerNo), customerNo);

            var parsedData = JRaw.Parse(await http.UnitLoadAllFromRemote());
            List<UnitDto> lst = new List<UnitDto>();

            foreach (JToken item in parsedData)
            {
                int microtingUid = int.Parse(item["id"].ToString());
                int siteUId = int.Parse(item["site_id"].ToString());

                bool otpEnabled = bool.Parse(item["otp_enabled"].ToString());
                int otpCode = 0;
                if (otpEnabled)
                {
                    otpCode = int.Parse(item["otp_code"].ToString());
                }

                DateTime? createdAt = DateTime.Parse(item["created_at"].ToString());
                DateTime? updatedAt = DateTime.Parse(item["updated_at"].ToString());
                UnitDto unitDto = new UnitDto()
                {
                    UnitUId = microtingUid,
                    CustomerNo = customerNo,
                    OtpCode = otpCode,
                    SiteUId = siteUId,
                    CreatedAt = createdAt,
                    UpdatedAt = updatedAt,
                    WorkflowState = Constants.WorkflowStates.Created
                };
                lst.Add(unitDto);
            }
            return lst;
        }

        public async Task<bool> UnitDelete(int unitId)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(unitId), unitId);

            string response = await http.UnitDelete(unitId);
            var parsedData = JRaw.Parse(response);

            if (parsedData["workflow_state"].ToString() == Constants.WorkflowStates.Removed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region public organization      
        public async Task<OrganizationDto> OrganizationLoadAllFromRemote(string token)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(token), token);
            IHttp specialHttp;
            if (token == "abc1234567890abc1234567890abcdef")
            {
                specialHttp = new HttpFake();
            } else
            {
                specialHttp = new Http(token, "https://basic.microting.com", "https://srv05.microting.com", "000", "", "https://speechtotext.microting.com");
            }
            

            JToken orgResult = JRaw.Parse(await specialHttp.OrganizationLoadAllFromRemote());

            OrganizationDto organizationDto = new OrganizationDto(int.Parse(orgResult.First.First["id"].ToString()),
                orgResult.First.First["name"].ToString(),
                int.Parse(orgResult.First.First["customer_no"].ToString()),
                int.Parse(orgResult.First.First["unit_license_number"].ToString()),
                orgResult.First.First["aws_id"].ToString(),
                orgResult.First.First["aws_key"].ToString(),
                orgResult.First.First["aws_endpoint"].ToString(),
                orgResult.First.First["com_address"].ToString(),
                orgResult.First.First["com_address_basic"].ToString(),
                orgResult.First.First["com_speech_to_text"].ToString(),
                orgResult.First.First["com_address_pdf_upload"].ToString());

            return organizationDto;
        }

        #endregion
        
        #region folder

        public async Task<List<FolderDto>> FolderLoadAllFromRemote()
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");

            string rawData = await http.FolderLoadAllFromRemote();
            
            List<FolderDto> list = new List<FolderDto>();
            if (!string.IsNullOrEmpty(rawData))
            {
                var parsedData = JRaw.Parse(rawData);

                foreach (JToken item in parsedData)
                {
                    int microtingUUID = int.Parse(item["id"].ToString());
                    string name = item["name"].ToString();
                    string description = item["description"].ToString();
                    int? parentId = null;
                    try
                    {
                        parentId = int.Parse(item["parent_id"].ToString());
                    } catch {}
                
                
                    FolderDto folderDto = new FolderDto(null, name, description, parentId, null, null, microtingUUID);
                
                    list.Add(folderDto);
                }
            }            

            return list;
        }
        
        public async Task<int> FolderCreate(string name, string description, int? parentId)
        {
            var parsedData = JRaw.Parse(await http.FolderCreate(name, description, parentId));

            int microtingUUID  = int.Parse(parsedData["id"].ToString());

            return microtingUUID;
        }

        public async Task FolderUpdate(int id, string name, string description, int? parentId)
        {
            await http.FolderUpdate(id, name, description, parentId);
        }

        public async Task<bool> FolderDelete(int id)
        {
            string response = await http.FolderDelete(id);
            var parsedData = JRaw.Parse(response);

            if (parsedData["workflow_state"].ToString() == Constants.WorkflowStates.Removed)
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
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entityType), entityType);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(name), name);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(id), id);

            try
            {
                if (entityType == Constants.FieldTypes.EntitySearch)
                {
                    string microtingUId = await http.EntitySearchGroupCreate(name, id);

                    if (microtingUId == null)
                        throw new Exception("EntityGroupCreate failed, due to microtingUId:'null'");
                    else
                        return microtingUId;
                }

                if (entityType == Constants.FieldTypes.EntitySelect)
                {
                    string microtingUId = await http.EntitySelectGroupCreate(name, id);

                    if (microtingUId == null)
                        throw new Exception("EntityGroupCreate failed, due to microtingUId:'null'");
                    else
                        return microtingUId;
                }

                throw new Exception("entityType:'" + entityType + "' not known");
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupCreate failed", ex);
            }
        }

        public async Task<bool> EntityGroupUpdate(string entityType, string name, int id, string entityGroupMUId)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entityType), entityType);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(name), name);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(id), id);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entityGroupMUId), entityGroupMUId);

            try
            {
                if (entityType == Constants.FieldTypes.EntitySearch)
                {
                    if (await http.EntitySearchGroupUpdate(id, name, entityGroupMUId))
                        return true;
                    else
                        throw new Exception("EntityGroupUpdate failed");
                }

                if (entityType == Constants.FieldTypes.EntitySelect)
                {
                    if (await http.EntitySelectGroupUpdate(id, name, entityGroupMUId))
                        return true;
                    else
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
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entityType), entityType);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entityGroupId), entityGroupId);

            try
            {
                if (entityType == Constants.FieldTypes.EntitySearch)
                {
                    if (await http.EntitySearchGroupDelete(entityGroupId))
                        return;
                    else
                        throw new Exception("EntityGroupDelete failed");
                }

                if (entityType == Constants.FieldTypes.EntitySelect)
                {
                    if (await http.EntitySelectGroupDelete(entityGroupId))
                        return;
                    else
                        throw new Exception("EntityGroupDelete failed");
                }

                throw new Exception("entityType:'" + entityType + "' not known");
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupDelete failed", ex);
            }
        }

        //---

        public async Task<string> EntitySearchItemCreate(string entitySearchGroupId, string name, string description, string id)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entitySearchGroupId), entitySearchGroupId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(name), name);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(id), id);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(description), description);

            try
            {
                return await http.EntitySearchItemCreate(entitySearchGroupId, name, description, id);
            }
            catch (Exception ex)
            {
                throw new Exception("Communicator.EntitySearchItemCreate failed", ex);
            }
        }

        public async Task<bool> EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name, string description, string id)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entitySearchGroupId), entitySearchGroupId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entitySearchItemId), entitySearchItemId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(name), name);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(id), id);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(description), description);

            //try
            //{
            return await http.EntitySearchItemUpdate(entitySearchGroupId, entitySearchItemId, name, description, id);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("EntitySearchItemUpdate failed", ex);
            //}
        }

        public async Task<bool> EntitySearchItemDelete(string entitySearchItemId)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entitySearchItemId), entitySearchItemId);

            try
            {
                return await http.EntitySearchItemDelete(entitySearchItemId);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemDelete failed", ex);
            }
        }

        //---

        public async Task<string> EntitySelectItemCreate(string entitySearchGroupId, string name, int displayOrder, string ownUUID)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entitySearchGroupId), entitySearchGroupId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(name), name);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(displayOrder), displayOrder);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(ownUUID), ownUUID);

            try
            {
                return await http.EntitySelectItemCreate(entitySearchGroupId, name, displayOrder, ownUUID);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemCreate failed", ex);
            }
        }

        public async Task<bool> EntitySelectItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name, int displayOrder, string ownUUID)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entitySearchGroupId), entitySearchGroupId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entitySearchItemId), entitySearchItemId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(name), name);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(displayOrder), displayOrder);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(ownUUID), ownUUID);

            //try
            //{
            return await http.EntitySelectItemUpdate(entitySearchGroupId, entitySearchItemId, name, displayOrder, ownUUID);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("EntitySearchItemUpdate failed", ex);
            //}
        }

        public async Task<bool> EntitySelectItemDelete(string entitySearchItemId)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(entitySearchItemId), entitySearchItemId);

            try
            {
                return await http.EntitySelectItemDelete(entitySearchItemId);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemDelete failed", ex);
            }
        }

        //---

        public async Task<bool> PdfUpload(string localPath, string hash)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(localPath), localPath);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(hash), hash);

            try
            {
                return await http.PdfUpload(localPath, hash);
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("Communicator") + " failed", ex);
            }
        }

        public async Task<string> TemplateDisplayIndexChange(string microtingUId, int siteId, int newDisplayIndex)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(microtingUId), microtingUId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(siteId), siteId);
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(newDisplayIndex), newDisplayIndex);

            try
            {
                return await http.TemplateDisplayIndexChange(microtingUId, siteId, newDisplayIndex);
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("Communicator") + " failed", ex);
            }
        }
        #endregion

        #region public speechToText
        public async Task<int> SpeechToText(string pathToAudioFile)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(pathToAudioFile), pathToAudioFile);

            try
            {
                return await http.SpeechToText(pathToAudioFile, "da-DK");
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("Communicator") + " failed", ex);
            }
        }

        public async Task<JToken> SpeechToText(int requestId)
        {
            await log.LogEverything(t.GetMethodName("Communicator"), "called");
            await log.LogVariable(t.GetMethodName("Communicator"), nameof(requestId), requestId);

            try
            {
                return await http.SpeechToText(requestId);
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName("Communicator") + " failed", ex);
            }
        }
        #endregion

        #region remove unwanted/uneeded methods from finished DLL
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
    }
}