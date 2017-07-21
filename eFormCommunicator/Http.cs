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

using System;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Net.Http;
using System.Reflection;

namespace eFormCommunicator
{
    internal class Http
    {

        #region var
        private string protocolXml = "6";
        private string protocolEntitySearch = "1";
        private string protocolEntitySelect = "4";

        private string token;
        private string addressApi;
        private string addressBasic;
        private string addressPdfUpload;
        private string organizationId;

        private string dllVersion;

        Tools t = new Tools();
        object _lock = new object();
        #endregion

        #region con
        internal Http(string token, string comAddressBasic, string comAddressApi, string comOrganizationId, string comAddressPdfUpload)
        {
            this.token = token;
            addressBasic = comAddressBasic;
            addressApi = comAddressApi;
            addressPdfUpload = comAddressPdfUpload;
            organizationId = comOrganizationId;

            dllVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion

        #region internal
        #region internal API
        /// <summary>
        /// Posts the element to Microting and returns the XML encoded restponse.
        /// </summary>
        /// <param name="xmlData">Element converted to a xml encoded string.</param>
        internal string Post(string xmlData, string siteId)
        {
            try
            {
                WebRequest request = WebRequest.Create(addressApi + "/gwt/inspection_app/integration/?token=" + token + "&protocol=" + protocolXml + "&site_id=" + siteId + "&sdk_ver=" + dllVersion);
                request.Method = "POST";
                byte[] content = Encoding.UTF8.GetBytes(xmlData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = content.Length;

                return PostToServer(request, content);
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" + ex.Message + "</Value>\n\t</Response>";
            }
        }

        /// <summary>
        /// Retrieve the XML encoded status from Microting.
        /// </summary>
        /// <param name="elementId">Identifier of the element to retrieve status of.</param>
        internal string Status(string elementId, string siteId)
        {
            try
            {
                WebRequest request = WebRequest.Create(addressApi + "/gwt/inspection_app/integration/" + elementId + "?token=" + token + "&protocol=" + protocolXml + "&site_id=" + siteId + "&download=false&delete=false" + "&sdk_ver=" + dllVersion);
                request.Method = "GET";

                return PostToServer(request);
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='error'>ConverterError: " + ex.Message + "</Value>\n\t</Response>";
            }
        }

        /// <summary>
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="microtingUuid">Identifier of the element to retrieve results from.</param>
        /// <param name="microtingCheckUuid">Identifier of the check to begin from.</param>
        internal string Retrieve(string microtingUuid, string microtingCheckUuid, int siteId)
        {
            try
            {
                WebRequest request = WebRequest.Create(addressApi + "/gwt/inspection_app/integration/" + microtingUuid + "?token=" + token + "&protocol=" + protocolXml + "&site_id=" + siteId + "&download=true&delete=false&last_check_id=" + microtingCheckUuid + "&sdk_ver=" + dllVersion);
                request.Method = "GET";

                return PostToServer(request);
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" + ex.Message + "</Value>\n\t</Response>";
            }
        }

        /// <summary>
        /// Deletes a element and retrieve the XML encoded response from Microting.
        /// </summary>
        /// <param name="elementId">Identifier of the element to delete.</param>
        internal string Delete(string elementId, string siteId)
        {
            try
            {
                WebRequest request = WebRequest.Create(addressApi + "/gwt/inspection_app/integration/" + elementId + "?token=" + token + "&protocol=" + protocolXml + "&site_id=" + siteId + "&download=false&delete=true" + "&sdk_ver=" + dllVersion);
                request.Method = "GET";

                return PostToServer(request);
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='error'>ConverterError: " + ex.Message + "</Value>\n\t</Response>";
            }
        }
        #endregion

        #region internal EntitySearch
        internal string     EntitySearchGroupCreate(string name, string id)
        {
            try
            {
                string xmlData = "<EntityTypes><EntityType><Name>" + name + "</Name><Id>" + id + "</Id></EntityType></EntityTypes>";

                WebRequest request = WebRequest.Create(addressApi + "/gwt/entity_app/entity_types?token=" + token + "&protocol=" + protocolEntitySearch +
                    "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
                request.Method = "POST";
                byte[] content = Encoding.UTF8.GetBytes(xmlData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = content.Length;

                string responseXml = PostToServer(request, content);

                if (responseXml.Contains("workflowState=\"created"))
                    return t.Locate(responseXml, "<MicrotingUUId>", "</");
                else
                    return null;
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" + ex.Message + "</Value>\n\t</Response>";
            }
        }

        internal bool       EntitySearchGroupUpdate(int id, string name, string entityGroupMUId)
        {
            string xmlData = "<EntityTypes><EntityType><Name>" + name + "</Name><Id>" + id + "</Id></EntityType></EntityTypes>";

            WebRequest request = WebRequest.Create(addressApi + "/gwt/entity_app/entity_types/"+ entityGroupMUId + "?token=" + token + "&protocol=" + protocolEntitySearch +
                "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(xmlData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = content.Length;

            string responseXml = PostToServer(request, content);

            if (responseXml.Contains("workflowState=\"created"))
                return true;
            else
                return false;
        }       

        internal bool       EntitySearchGroupDelete(string entityGroupId)
        {
            try
            {
                WebRequest request = WebRequest.Create(addressApi + "/gwt/entity_app/entity_types/" + entityGroupId + "?token=" + token + "&protocol=" + protocolEntitySearch + 
                    "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
                request.Method = "DELETE";
                request.ContentType = "application/x-www-form-urlencoded";  //-- ?

                string responseXml = PostToServer(request);

                if (responseXml.Contains("Value type=\"success"))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchGroupDelete failed", ex);
            }
        }

        internal string     EntitySearchItemCreate(string entitySearchGroupId, string name, string description, string id)
        {
            string xmlData = "<Entities><Entity>" + 
                "<EntityTypeId>" + entitySearchGroupId + "</EntityTypeId><Identifier>" + name + "</Identifier><Description>" + description + "</Description>" +
                "<Km></Km><Colour></Colour><Radiocode></Radiocode>" + //Legacy. To be removed server side
                "<Id>" + id + "</Id>" + 
                "</Entity></Entities>";

            WebRequest request = WebRequest.Create(addressApi + "/gwt/entity_app/entities?token=" + token + "&protocol=" + protocolEntitySearch + 
                "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
            request.Method = "POST";
            byte[] content = Encoding.UTF8.GetBytes(xmlData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = content.Length;

            string responseXml = PostToServer(request, content);

            if (responseXml.Contains("workflowState=\"created"))
                return t.Locate(responseXml, "<MicrotingUUId>", "</");
            else
                return null;
        }

        internal bool       EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name, string description, string id)
        {
            string xmlData = "<Entities><Entity>" +
                "<EntityTypeId>" + entitySearchGroupId + "</EntityTypeId><Identifier>" + name + "</Identifier><Description>" + description + "</Description>" +
                "<Km></Km><Colour></Colour><Radiocode></Radiocode>" + //Legacy. To be removed server side
                "<Id>" + id + "</Id>" +
                "</Entity></Entities>";

            WebRequest request = WebRequest.Create(addressApi + "/gwt/entity_app/entities/" + entitySearchItemId + "?token=" + token + "&protocol=" + protocolEntitySearch +
                "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(xmlData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = content.Length;

            string responseXml = PostToServer(request, content);

            if (responseXml.Contains("workflowState=\"created"))
                return true;
            else
                return false;
        }

        internal bool       EntitySearchItemDelete(string entitySearchItemId)
        {
            WebRequest request = WebRequest.Create(addressApi + "/gwt/entity_app/entities/" + entitySearchItemId + "?token=" + token + "&protocol=" + protocolEntitySearch + 
                "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
            request.Method = "DELETE";
            request.ContentType = "application/x-www-form-urlencoded";  //-- ?

            string responseXml = PostToServer(request);

            if (responseXml.Contains("Value type=\"success"))
                return true;
            else
                return false;
        }
        #endregion

        #region internal EntitySelect
        internal string     EntitySelectGroupCreate(string name, string id)
        {
            try
            {
                //string xmlData = "{ \"model\" : { \"name\" : \"" + name + "\", \"api_uuid\" : \"" + id + "\" } }";
                JObject content_to_microting = JObject.FromObject(new { model = new { name = name, api_uuid = id} });

                WebRequest request = WebRequest.Create(addressApi + "/gwt/inspection_app/searchable_item_groups.json?token=" + token + "&protocol=" + protocolEntitySelect + 
                    "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
                request.Method = "POST";
                byte[] content = Encoding.UTF8.GetBytes(content_to_microting.ToString());
                request.ContentType = "application/json; charset=utf-8";
                request.ContentLength = content.Length;

                string responseXml = PostToServer(request, content);

                if (responseXml.Contains("workflow_state\": \"created"))
                    return t.Locate(responseXml, "\"id\": \"", "\"");
                else
                    return null;
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" + ex.Message + "</Value>\n\t</Response>";
            }
        }

        internal bool       EntitySelectGroupUpdate(int id, string name, string entityGroupMUId)
        {
            JObject content_to_microting = JObject.FromObject(new { model = new { name = name, api_uuid = id } });

            WebRequest request = WebRequest.Create(addressApi + "/gwt/inspection_app/searchable_item_groups/"+ entityGroupMUId + "?token=" + token + "&protocol=" + protocolEntitySelect + 
                "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(content_to_microting.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;

            string responseXml = PostToServerNoRedirect(request, content);

            if (responseXml.Contains("html><body>You are being <a href=") && responseXml.Contains(">redirected</a>.</body></html>"))
            {
                WebRequest request2 = WebRequest.Create(addressApi + "/gwt/inspection_app/searchable_item_groups/" + entityGroupMUId + ".json?token=" + token + "&protocol=" + protocolEntitySelect + 
                    "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
                request2.Method = "GET";
                string responseXml2 = PostToServer(request2);

                if (responseXml2.Contains("workflow_state\": \"created"))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        internal bool       EntitySelectGroupDelete(string entityGroupId)
        {
            try
            {
                WebRequest request = WebRequest.Create(addressApi + "/gwt/inspection_app/searchable_item_groups/" + entityGroupId + ".json?token=" + token + "&protocol=" + protocolEntitySelect +
                    "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
                request.Method = "DELETE";
                request.ContentType = "application/json; charset=utf-8";

                string responseXml = PostToServerNoRedirect(request);

                if (responseXml.Contains("html><body>You are being <a href=") && responseXml.Contains(">redirected</a>.</body></html>"))
                {
                    WebRequest request2 = WebRequest.Create(addressApi + "/gwt/inspection_app/searchable_item_groups/" + entityGroupId + ".json?token=" + token + "&protocol=" + protocolEntitySelect +
                        "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
                    request2.Method = "GET";
                    string responseXml2 = PostToServer(request2);

                    if (responseXml2.Contains("workflow_state\": \"removed"))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchGroupDelete failed", ex);
            }
        }

        internal string     EntitySelectItemCreate(string entitySelectGroupId, string name, int displayOrder, string id)
        {
            JObject content_to_microting = JObject.FromObject(new { model = new { data = name, api_uuid = id, display_order = displayOrder, searchable_group_id = entitySelectGroupId } });

            WebRequest request = WebRequest.Create(addressApi + "/gwt/inspection_app/searchable_items.json?token=" + token + "&protocol=" + protocolEntitySelect + 
                "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
            request.Method = "POST";
            byte[] content = Encoding.UTF8.GetBytes(content_to_microting.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;

            string responseXml = PostToServer(request, content);

            if (responseXml.Contains("workflow_state\": \"created"))
                return t.Locate(responseXml, "\"id\": \"", "\"");
            else
                return null;
        }

        internal bool       EntitySelectItemUpdate(string entitySelectGroupId, string entitySelectItemId, string name, int displayOrder, string id)
        {
            JObject content_to_microting = JObject.FromObject(new { model = new { data = name, api_uuid = id, display_order = displayOrder, searchable_group_id = entitySelectGroupId } });

            WebRequest request = WebRequest.Create(addressApi + "/gwt/inspection_app/searchable_items/" + entitySelectItemId + "?token=" + token + "&protocol=" + protocolEntitySelect +
                "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(content_to_microting.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;

            string responseXml = PostToServerNoRedirect(request, content);

            if (responseXml.Contains("html><body>You are being <a href=") && responseXml.Contains(">redirected</a>.</body></html>"))
            {
                WebRequest request2 = WebRequest.Create(addressApi + "/gwt/inspection_app/searchable_items/" + entitySelectItemId + ".json?token=" + token + "&protocol=" + protocolEntitySelect 
                    + "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
                request2.Method = "GET";
                string responseXml2 = PostToServer(request2);

                if (responseXml2.Contains("workflow_state\": \"created"))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        internal bool       EntitySelectItemDelete(string entitySelectItemId)
        {
            WebRequest request = WebRequest.Create(addressApi + "/gwt/inspection_app/searchable_items/" + entitySelectItemId + ".json?token=" + token + "&protocol=" + protocolEntitySelect + 
                "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
            request.Method = "DELETE";
            request.ContentType = "application/json; charset=utf-8";

            string responseXml = PostToServerNoRedirect(request);

            if (responseXml.Contains("html><body>You are being <a href=") && responseXml.Contains(">redirected</a>.</body></html>"))
            {
                WebRequest request2 = WebRequest.Create(addressApi + "/gwt/inspection_app/searchable_items/" + entitySelectItemId + ".json?token=" + token + "&protocol=" + protocolEntitySelect +
                    "&organization_id=" + organizationId + "&sdk_ver=" + dllVersion);
                request2.Method = "GET";
                string responseXml2 = PostToServer(request2);

                if (responseXml2.Contains("workflow_state\": \"removed"))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        #endregion

        #region internal PdfUpload
        internal bool PdfUpload(string name, string hash)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string url = addressPdfUpload + "/data_uploads/upload?token="+ token + "&hash=" + hash + "&extension=pdf" + "&sdk_ver=" + dllVersion;
                    client.UploadFile(url, name);
                }

                return true;
            }
            catch
            {
                return false; 
            }
        }
        #endregion

        #region internal TemplateDisplayIndexChange
        internal string TemplateDisplayIndexChange(string microtingUId, int siteId, int newDisplayIndex)
        {
            try
            {
                WebRequest request = WebRequest.Create(addressApi + "/gwt/inspection_app/integration/" + microtingUId + "?token=" + token + "&protocol=" + protocolXml +
                    "&site_id=" + siteId + "&download=false&delete=false&update_attributes=true&display_order=" + newDisplayIndex.ToString() + "&sdk_ver=" + dllVersion);
                request.Method = "GET";

                return PostToServer(request);
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='error'>ConverterError: " + ex.Message + "</Value>\n\t</Response>";
                //return false;
            }
        }
        #endregion

        #region internal site
        internal string     SiteCreate(string name)
        {
            JObject content_to_microting = JObject.FromObject(new { name = name });
            WebRequest request = WebRequest.Create(addressBasic + "/v1/sites?token=" + token + "&model=" + content_to_microting.ToString() + "&sdk_ver=" + dllVersion);
            request.Method = "POST";
            byte[] content = Encoding.UTF8.GetBytes(content_to_microting.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;

            string newUrl = PostToServerGetRedirect(request, content);

            request = WebRequest.Create(newUrl+"?token="+token);
            request.Method = "GET";

            string response = PostToServer(request);

            return response;
        }

        internal bool       SiteUpdate(int id, string name)
        {
            JObject content_to_microting = JObject.FromObject(new { name = name });
            WebRequest request = WebRequest.Create(addressBasic + "/v1/sites/" + id + "?token=" + token + "&model=" + content_to_microting.ToString() + "&sdk_ver=" + dllVersion);
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(content_to_microting.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;

            string newUrl = PostToServerGetRedirect(request, content);

            return true;
        }

        internal string     SiteDelete(int id)
        {
            try
            {
                WebRequest request = WebRequest.Create(addressBasic + "/v1/sites/" + id + "?token=" + token + "&sdk_ver=" + dllVersion);
                request.Method = "DELETE";
                request.ContentType = "application/x-www-form-urlencoded";  //-- ?

                string newUrl = PostToServerGetRedirect(request);

                request = WebRequest.Create(newUrl + "?token=" + token);
                request.Method = "GET";

                return PostToServer(request);

            }
            catch (Exception ex)
            {
                throw new Exception("SiteDelete failed", ex);
            }
        }

        internal string     SiteLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create(addressBasic + "/v1/sites?token=" + token + "&sdk_ver=" + dllVersion);
            request.Method = "GET";

            return PostToServer(request);
        }
        #endregion

        #region internal Worker
        internal string     WorkerCreate(string firstName, string lastName, string email)
        {
            JObject content_to_microting = JObject.FromObject(new { first_name = firstName, last_name = lastName, email = email });
            WebRequest request = WebRequest.Create(addressBasic + "/v1/users?token=" + token + "&model=" + content_to_microting.ToString() + "&sdk_ver=" + dllVersion);
            request.Method = "POST";
            byte[] content = Encoding.UTF8.GetBytes(content_to_microting.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;

            string newUrl = PostToServerGetRedirect(request, content);

            request = WebRequest.Create(newUrl + "?token=" + token);
            request.Method = "GET";

            string response = PostToServer(request);

            return response;
        }

        internal bool       WorkerUpdate(int id, string firstName, string lastName, string email)
        {
            JObject content_to_microting = JObject.FromObject(new { first_name = firstName, last_name = lastName, email = email });
            WebRequest request = WebRequest.Create(addressBasic + "/v1/users/" + id + "?token=" + token + "&model=" + content_to_microting.ToString() + "&sdk_ver=" + dllVersion);
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(content_to_microting.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;

            string newUrl = PostToServerGetRedirect(request, content);

            return true;
        }

        internal string     WorkerDelete(int id)
        {
            try
            {
                WebRequest request = WebRequest.Create(addressBasic + "/v1/users/" + id + "?token=" + token + "&sdk_ver=" + dllVersion);
                request.Method = "DELETE";
                request.ContentType = "application/x-www-form-urlencoded";  //-- ?

                string newUrl = PostToServerGetRedirect(request);

                request = WebRequest.Create(newUrl + "?token=" + token);
                request.Method = "GET";

                return PostToServer(request);

            }
            catch (Exception ex)
            {
                throw new Exception("WorkerDelete failed", ex);
            }
        }

        internal string     WorkerLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create(addressBasic + "/v1/users?token=" + token + "&sdk_ver=" + dllVersion);
            request.Method = "GET";

            return PostToServer(request);
        }
        #endregion

        #region internal SiteWorker
        internal string     SiteWorkerCreate(int siteId, int workerId)
        {
            JObject content_to_microting = JObject.FromObject(new { user_id = workerId, site_id = siteId });
            WebRequest request = WebRequest.Create(addressBasic + "/v1/workers?token=" + token + "&model=" + content_to_microting.ToString() + "&sdk_ver=" + dllVersion);
            request.Method = "POST";
            byte[] content = Encoding.UTF8.GetBytes(content_to_microting.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;

            string newUrl = PostToServerGetRedirect(request, content);

            request = WebRequest.Create(newUrl + "?token=" + token);
            request.Method = "GET";

            string response = PostToServer(request);

            return response;
        }

        internal string       SiteWorkerDelete(int id)
        {
            try
            {
                WebRequest request = WebRequest.Create(addressBasic + "/v1/workers/" + id + "?token=" + token + "&sdk_ver=" + dllVersion);
                request.Method = "DELETE";
                request.ContentType = "application/x-www-form-urlencoded";  //-- ?

                string newUrl = PostToServerGetRedirect(request);

                request = WebRequest.Create(newUrl + "?token=" + token);
                request.Method = "GET";

                return PostToServer(request);

            }
            catch (Exception ex)
            {
                throw new Exception("SiteWorkerDelete failed", ex);
            }
        }

        internal string     SiteWorkerLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create(addressBasic + "/v1/workers?token=" + token + "&sdk_ver=" + dllVersion);
            request.Method = "GET";

            return PostToServer(request);
        }
        #endregion

        #region internal Unit
        internal int        UnitRequestOtp(int id)
        {
            JObject content_to_microting = JObject.FromObject(new { model = new { unit_id = id } });
            WebRequest request = WebRequest.Create(addressBasic + "/v1/units/"+id+"?token=" + token + "&new_otp=true" + "&model=" + content_to_microting.ToString() + "&sdk_ver=" + dllVersion);
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(content_to_microting.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;

            string newUrl = PostToServerGetRedirect(request, content);

            request = WebRequest.Create(newUrl + "?token=" + token);
            request.Method = "GET";

            int response = int.Parse(JRaw.Parse(PostToServer(request))["otp_code"].ToString());

            return response;
        }

        internal string     UnitLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create(addressBasic + "/v1/units?token=" + token + "&sdk_ver=" + dllVersion);
            request.Method = "GET";

            return PostToServer(request);
        }
        #endregion

        #region internal Organization
        internal string OrganizationLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create(addressBasic + "/v1/organizations?token=" + token + "&sdk_ver=" + dllVersion);
            request.Method = "GET";

            return PostToServer(request);
        }
        #endregion
        #endregion

        #region private
        private string PostToServer(WebRequest request, byte[] content)
        {
            lock (_lock)
            {
                // Hack for ignoring certificate validation.
                ServicePointManager.ServerCertificateValidationCallback = Validator;
                Stream dataRequestStream = request.GetRequestStream();
                dataRequestStream.Write(content, 0, content.Length);
                dataRequestStream.Close();

                WebResponse response = request.GetResponse();

                Stream dataResponseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataResponseStream);
                string responseFromServer = reader.ReadToEnd();

                // Clean up the streams.
                try
                {
                    reader.Close();
                    dataResponseStream.Close();
                    response.Close();
                }
                catch
                {

                }

                return responseFromServer;
            }
        }

        private string PostToServerGetRedirect(WebRequest request, byte[] content)
        {
            lock (_lock)
            {
                // Hack for ignoring certificate validation.
                ServicePointManager.ServerCertificateValidationCallback = Validator;

                Stream dataRequestStream = request.GetRequestStream();
                dataRequestStream.Write(content, 0, content.Length);
                dataRequestStream.Close();

                HttpWebRequest httpRequest = (HttpWebRequest)request;
                httpRequest.CookieContainer = new CookieContainer();
                httpRequest.AllowAutoRedirect = false;


                HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
                string newUrl = "";
                if (response.StatusCode == HttpStatusCode.Redirect ||
                    response.StatusCode == HttpStatusCode.MovedPermanently)
                {
                    newUrl = response.Headers["Location"];
                }

                // Clean up the streams.
                try
                {
                    response.Close();
                }
                catch
                {

                }
                return newUrl;
            }
        }

        private string PostToServerGetRedirect(WebRequest request)
        {
            lock (_lock)
            {
                // Hack for ignoring certificate validation.
                ServicePointManager.ServerCertificateValidationCallback = Validator;

                HttpWebRequest httpRequest = (HttpWebRequest)request;
                httpRequest.CookieContainer = new CookieContainer();
                httpRequest.AllowAutoRedirect = false;

                HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
                string newUrl = "";
                if (response.StatusCode == HttpStatusCode.Redirect ||
                    response.StatusCode == HttpStatusCode.MovedPermanently)
                {
                    newUrl = response.Headers["Location"];
                }

                // Clean up the streams.
                try
                {
                    response.Close();
                }
                catch
                {

                }

                return newUrl;
            }
        }

        private string PostToServerNoRedirect(WebRequest request, byte[] content)
        {
            lock (_lock)
            {
                // Hack for ignoring certificate validation.
                ServicePointManager.ServerCertificateValidationCallback = Validator;

                Stream dataRequestStream = request.GetRequestStream();
                dataRequestStream.Write(content, 0, content.Length);
                dataRequestStream.Close();

                HttpWebRequest httpRequest = (HttpWebRequest)request;
                httpRequest.CookieContainer = new CookieContainer();
                httpRequest.AllowAutoRedirect = false;

                HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
                Stream dataResponseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataResponseStream);
                string responseFromServer = reader.ReadToEnd();

                // Clean up the streams.
                try
                {
                    reader.Close();
                    dataResponseStream.Close();
                    response.Close();
                }
                catch
                {

                }

                return responseFromServer;
            }
        }

        private string PostToServer(WebRequest request)
        {
            lock (_lock)
            {
                // Hack for ignoring certificate validation.
                ServicePointManager.ServerCertificateValidationCallback = Validator;

                WebResponse response = request.GetResponse();
                Stream dataResponseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataResponseStream);
                string responseFromServer = reader.ReadToEnd();

                // Clean up the streams.
                try
                {
                    reader.Close();
                    dataResponseStream.Close();
                    response.Close();
                }
                catch
                {

                }

                return responseFromServer;
            }
        }

        private string PostToServerNoRedirect(WebRequest request)
        {
            lock (_lock)
            { 
                // Hack for ignoring certificate validation.
                ServicePointManager.ServerCertificateValidationCallback = Validator;

                HttpWebRequest httpRequest = (HttpWebRequest)request;
                httpRequest.CookieContainer = new CookieContainer();
                httpRequest.AllowAutoRedirect = false;

                HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
                Stream dataResponseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataResponseStream);
                string responseFromServer = reader.ReadToEnd();

                // Clean up the streams.
                try
                {
                    reader.Close();
                    dataResponseStream.Close();
                    response.Close();
                }
                catch
                {

                }

                return responseFromServer;
            }
        }

        /// <summary>
        /// This method is a hack and will allways return true
        /// </summary>
        /// <param name='sender'>
        /// The sender object
        /// </param>
        /// <param name='certificate'>
        /// The certificate object
        /// </param>
        /// <param name='chain'>
        /// The certificate chain
        /// </param>
        /// <param name='sslpolicyErrors'>
        /// SslPolicy Enum
        /// </param>
        private bool Validator(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyErrors)
        {
            return true;
        }
        #endregion
    }
}