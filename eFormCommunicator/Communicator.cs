/*
The MIT License (MIT)

Copyright (c) 2014 microting

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

using eFormShared;
using eFormSqlController;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace eFormCommunicator
{
    public class Communicator
    {
        #region var
        public object _lockSending = new object();
        Http http;
        SqlController sqlController;
        Tools t = new Tools();
        #endregion

        #region con
        /// <summary>
        /// Microting XML eForm API C# DLL.
        /// </summary>
        /// <param name="token">Your company's XML eForm API access token.</param>
        /// <param name="comAddressApi">Microting's eForm API server address.</param>
        /// <param name="comOrganizationId">Your company's organization id.</param>
        public Communicator(SqlController sqlController)
        {
            this.sqlController = sqlController;

            string token = sqlController.SettingRead(Settings.token);
            string comAddressApi = sqlController.SettingRead(Settings.comAddressApi);
            string comAddressBasic = sqlController.SettingRead(Settings.comAddressBasic);
            string comOrganizationId = sqlController.SettingRead(Settings.comOrganizationId);
            string ComAddressPdfUpload = "";
            try {
                ComAddressPdfUpload = sqlController.SettingRead(Settings.comAddressPdfUpload);
            } catch
            {               
                // This is okay in a upgrade case.
            }
            

            #region CheckInput token & serverAddress
            string errorsFound = "";

            if (token.Length != 32)
                errorsFound += "Tokens are always 32 charactors long" + Environment.NewLine;

            if (!comAddressApi.Contains("http://") && !comAddressApi.Contains("https://"))
                errorsFound += "comAddressApi is missing 'http://' or 'https://'" + Environment.NewLine;

            if (!comAddressBasic.Contains("http://") && !comAddressBasic.Contains("https://"))
                errorsFound += "comAddressBasic is missing 'http://' or 'https://'" + Environment.NewLine;

            if (comOrganizationId == null)
                comOrganizationId = "";

            if (comOrganizationId == "")
                errorsFound += "comOrganizationId is missing" + Environment.NewLine;

            if (errorsFound != "")
                throw new InvalidOperationException(errorsFound.TrimEnd());
            #endregion

            http = new Http(token, comAddressBasic, comAddressApi, comOrganizationId, ComAddressPdfUpload);
        }
        #endregion

        #region public api
        /// <summary>
        /// Posts the XML eForm to Microting and returns the XML encoded restponse (Does not support the complex elements Entity_Search or Entity_Select).
        /// </summary>
        /// <param name="xmlString">XML encoded eForm string.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public string PostXml           (string xmlString, int siteId)
        {
            lock (_lockSending)
            {
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

                //TriggerEventLog("siteId:" + siteId.ToString() + ", xmlString:");
                //TriggerEventLog(xmlString);

                return http.Post(xmlString, siteId.ToString());
            }
        }

        /// <summary>
        /// Retrieve the XML encoded status from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve status of.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public string CheckStatus       (string eFormId, int siteId)
        {
            lock (_lockSending)
            {
                //TriggerEventLog("eFormId:" + eFormId + ", siteId:" + siteId.ToString());

                return http.Status(eFormId, siteId.ToString());
            }
        }

        /// <summary>
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve results from.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public string Retrieve          (string eFormId, int siteId)
        {
            lock (_lockSending)
            {
                //TriggerEventLog("eFormId:" + eFormId + ", siteId:" + siteId.ToString());

                return http.Retrieve(eFormId, "0", siteId); //Always gets the first
            }
        }

        /// <summary>
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve results from.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        /// <param name="eFormCheckId">Identifier of the check to begin from.</param>
        public string RetrieveFromId    (string eFormId, int siteId, string eFormCheckId)
        {
            lock (_lockSending)
            {
                //TriggerEventLog("eFormId:" + eFormId + ", siteId:" + siteId.ToString() + ", eFormCheckId:" + eFormCheckId);

                return http.Retrieve(eFormId, eFormCheckId, siteId);
            }
        }

        /// <summary>
        /// Marks an element for deletion and retrieve the XML encoded response from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to mark for deletion.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public string Delete            (string eFormId, int siteId)
        {
            lock (_lockSending)
            {
                //TriggerEventLog("eFormId:" + eFormId + ", siteId:" + siteId.ToString());

                return http.Delete(eFormId, siteId.ToString());
            }
        }
        #endregion

        #region public site
        #region public siteName
        public Tuple<Site_Dto, Unit_Dto>       SiteCreate(string name)
        {
            string response = http.SiteCreate(name);
            var parsedData = JRaw.Parse(response);

            int unitId = int.Parse(parsedData["unit_id"].ToString());
            int otpCode = int.Parse(parsedData["otp_code"].ToString());
            Site_Dto siteDto = new Site_Dto(int.Parse(parsedData["id"].ToString()), parsedData["name"].ToString(), "", "", 0, 0, unitId, 0); // WorkerUid is set to 0, because it's used in this context.
            Unit_Dto unitDto = new Unit_Dto(unitId, 0, otpCode, siteDto.SiteId, DateTime.Parse(parsedData["created_at"].ToString()), DateTime.Parse(parsedData["updated_at"].ToString()));
            Tuple<Site_Dto, Unit_Dto> result = new Tuple<Site_Dto, Unit_Dto>(siteDto, unitDto);

            return result;
        }

        public bool         SiteUpdate(int siteId, string name)
        {
            return http.SiteUpdate(siteId, name);
        }

        public bool         SiteDelete(int siteId)
        {
            string response = http.SiteDelete(siteId);
            var parsedData = JRaw.Parse(response);

            if(parsedData["workflow_state"].ToString() == "removed")
            {
                return true;
            } else
            {
                return false;
            }
        }

        public List<SiteName_Dto>  SiteLoadAllFromRemote()
        {
            var parsedData = JRaw.Parse(http.SiteLoadAllFromRemote());
            List<SiteName_Dto> lst = new List<SiteName_Dto>();

            foreach (JToken item in parsedData)
            {
                string name = item["name"].ToString();
                int microtingUid = int.Parse(item["id"].ToString());
                DateTime? createdAt = DateTime.Parse(item["created_at"].ToString());
                DateTime? updatedAt = DateTime.Parse(item["updated_at"].ToString());
                SiteName_Dto temp = new SiteName_Dto(microtingUid, name, createdAt, updatedAt);
                lst.Add(temp);
            }
            return lst;
        }
        #endregion

        #region public worker
        public Worker_Dto       WorkerCreate(string firstName, string lastName, string email)
        {
            string result = http.WorkerCreate(firstName, lastName, email);
            var parsedData = JRaw.Parse(result);
            int workerUid = int.Parse(parsedData["id"].ToString());
            DateTime? createdAt = DateTime.Parse(parsedData["created_at"].ToString());
            DateTime? updatedAt = DateTime.Parse(parsedData["updated_at"].ToString());
            return new Worker_Dto(workerUid, firstName, lastName, email, createdAt, updatedAt);

        }

        public bool             WorkerUpdate(int workerId, string firstName, string lastName, string email)
        {
            return http.WorkerUpdate(workerId, firstName, lastName, email);
        }

        public bool             WorkerDelete(int workerId)
        {
            string response = http.WorkerDelete(workerId);
            var parsedData = JRaw.Parse(response);

            if (parsedData["workflow_state"].ToString() == "removed")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Worker_Dto> WorkerLoadAllFromRemote()
        {
            var parsedData = JRaw.Parse(http.WorkerLoadAllFromRemote());
            List<Worker_Dto> lst = new List<Worker_Dto>();

            foreach (JToken item in parsedData)
            {
                string firstName = item["first_name"].ToString();
                string lastName = item["last_name"].ToString();
                string email = item["email"].ToString();
                int microtingUid = int.Parse(item["id"].ToString());
                DateTime? createdAt = DateTime.Parse(item["created_at"].ToString());
                DateTime? updatedAt = DateTime.Parse(item["updated_at"].ToString());
                Worker_Dto temp = new Worker_Dto(microtingUid, firstName, lastName, email, createdAt, updatedAt);
                lst.Add(temp);
            }
            return lst;
        }
        #endregion

        #region public site_worker
        public Site_Worker_Dto SiteWorkerCreate(int siteId, int workerId)
        {
            string result = http.SiteWorkerCreate(siteId, workerId);
            var parsedData = JRaw.Parse(result);
            int workerUid = int.Parse(parsedData["id"].ToString());
            return new Site_Worker_Dto(workerUid, siteId, workerId);
        }

        public bool         SiteWorkerDelete(int workerId)
        {

            string response = http.SiteWorkerDelete(workerId);
            var parsedData = JRaw.Parse(response);

            if (parsedData["workflow_state"].ToString() == "removed")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Site_Worker_Dto> SiteWorkerLoadAllFromRemote()
        {
            var parsedData = JRaw.Parse(http.SiteWorkerLoadAllFromRemote());
            List<Site_Worker_Dto> lst = new List<Site_Worker_Dto>();

            foreach (JToken item in parsedData)
            {
                int microtingUid = int.Parse(item["id"].ToString());
                int siteUId = int.Parse(item["site_id"].ToString());
                int workerUId = int.Parse(item["user_id"].ToString());
                Site_Worker_Dto temp = new Site_Worker_Dto(microtingUid, siteUId, workerUId);
                lst.Add(temp);
            }
            return lst;
        }
        #endregion

        #region public unit      
        public int          UnitRequestOtp(int unitId)
        {
            return http.UnitRequestOtp(unitId);
        }

        public List<Unit_Dto> UnitLoadAllFromRemote(int customerNo)
        {
            var parsedData = JRaw.Parse(http.UnitLoadAllFromRemote());
            List<Unit_Dto> lst = new List<Unit_Dto>();

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
                Unit_Dto temp = new Unit_Dto(microtingUid, customerNo, otpCode, siteUId, createdAt, updatedAt);
                lst.Add(temp);
            }
            return lst;
        }
        #endregion

        #region public organization      
        public Organization_Dto OrganizationLoadAllFromRemote(string token)
        {
            Http specialHttp = new Http(token, "https://basic.microting.com", "https://srv05.microting.com", "666", "");

            JToken orgResult = JRaw.Parse(specialHttp.OrganizationLoadAllFromRemote());

            Organization_Dto organizationDto = new Organization_Dto(int.Parse(orgResult.First.First["id"].ToString()),
                orgResult.First.First["name"].ToString(),
                int.Parse(orgResult.First.First["customer_no"].ToString()),
                int.Parse(orgResult.First.First["unit_license_number"].ToString()),
                orgResult.First.First["aws_id"].ToString(),
                orgResult.First.First["aws_key"].ToString(),
                orgResult.First.First["aws_endpoint"].ToString(),
                orgResult.First.First["com_address"].ToString(),
                orgResult.First.First["com_address_basic"].ToString(),
                orgResult.First.First["com_address_pdf_upload"].ToString());

            return organizationDto;
        }

        #endregion
        #endregion

        #region public entity
        public string   EntityGroupCreate(string entityType, string name, string id)
        {
            try
            {
                if (entityType == "EntitySearch")
                {
                    string microtingUId = http.EntitySearchGroupCreate(name, id);

                    if (microtingUId == null)
                        throw new Exception("EntityGroupCreate failed, due to microtingUId:'null'");
                    else
                        return microtingUId;
                }

                if (entityType == "EntitySelect")
                {
                    string microtingUId = http.EntitySelectGroupCreate(name, id);

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

        public void     EntityGroupDelete(string entityType, string entityGroupId)
        {
            try
            {
                if (entityType == "EntitySearch")
                {
                    if (http.EntitySearchGroupDelete(entityGroupId))
                        return;
                    else
                        throw new Exception("EntitySearchItemDelete failed");
                }

                if (entityType == "EntitySelect")
                {
                    if (http.EntitySelectGroupDelete(entityGroupId))
                        return;
                    else
                        throw new Exception("EntitySearchItemDelete failed");
                }

                throw new Exception("entityType:'" + entityType + "' not known");
            }
            catch (Exception ex)
            {
                throw new Exception("EntityGroupDelete failed", ex);
            }
        }

        //---

        public string   EntitySearchItemCreate(string entitySearchGroupId, string name, string description, string id)
        {
            try
            {
                return http.EntitySearchItemCreate(entitySearchGroupId, name, description, id);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemCreate failed", ex);
            }
        }

        public bool     EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name, string description, string id)
        {
            try
            {
                return http.EntitySearchItemUpdate(entitySearchGroupId, entitySearchItemId, name, description, id);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemUpdate failed", ex);
            }
        }

        public bool     EntitySearchItemDelete(string entitySearchItemId)
        {
            try
            {
                return http.EntitySearchItemDelete(entitySearchItemId);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemDelete failed", ex);
            }
        }

        //---

        public string   EntitySelectItemCreate(string entitySearchGroupId, string name, int displayOrder, string id)
        {
            try
            {
                return http.EntitySelectItemCreate(entitySearchGroupId, name, displayOrder, id);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemCreate failed", ex);
            }
        }

        public bool     EntitySelectItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name, int displayOrder, string id)
        {
            try
            {
                return http.EntitySelectItemUpdate(entitySearchGroupId, entitySearchItemId, name, displayOrder, id);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemUpdate failed", ex);
            }
        }

        public bool     EntitySelectItemDelete(string entitySearchItemId)
        {
            try
            {
                return http.EntitySelectItemDelete(entitySearchItemId);
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchItemDelete failed", ex);
            }
        }

        //---

        public bool     PdfUpload(string localPath, string hash)
        {
            try
            {
                return http.PdfUpload(localPath, hash);
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
            }
        }

        public string TemplateDisplayIndexChange(string microtingUId, int siteId, int newDisplayIndex)
        {
            try
            {
                return http.TemplateDisplayIndexChange(microtingUId, siteId, newDisplayIndex);
            }
            catch (Exception ex)
            {
                throw new Exception(t.GetMethodName() + " failed", ex);
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
