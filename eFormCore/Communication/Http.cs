﻿/*
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
using System.CodeDom.Compiler;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microting.eForm.Communication
{
    public class Http : IHttp
    {
        // start var section
        private string protocolXml = "6";
        private string protocolEntitySearch = "1";
        private string protocolEntitySelect = "4";

        private string token;
        private string addressApi;
        private string addressBasic;
        private string newAddressBasic;
        private string addressPdfUpload;
        private string addressSpeechToText;
        private string organizationId;

        private string dllVersion;

        Tools t = new Tools();
        object _lock = new object();
        // end var section

        // start con section
        public Http(string token, string comAddressBasic, string comAddressApi, string comOrganizationId, string comAddressPdfUpload, string comSpeechToText)
        {
            this.token = token;
            addressBasic = comAddressBasic;
            addressApi = comAddressApi;
            addressPdfUpload = comAddressPdfUpload;
            organizationId = comOrganizationId;
            addressSpeechToText = comSpeechToText;
            newAddressBasic = "https://microcore.microting.com";

            dllVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        // end con section

        // public
        // public API

        /// <summary>
        /// Posts the element to Microting and returns the XML encoded restponse.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="siteId"></param>
        /// <param name="contentType"></param>
        public async Task<string> Post(string data, string siteId, string contentType = "application/x-www-form-urlencoded")
        {
            try
            {
                WriteDebugConsoleLogEntry("Http.Post", $"called at {DateTime.UtcNow}");
                WebRequest request = WebRequest.Create(
                    $"{addressApi}/gwt/inspection_app/integration/?token={token}&protocol={protocolXml}&site_id={siteId}&sdk_ver={dllVersion}");
                request.Method = "POST";
                byte[] content = Encoding.UTF8.GetBytes(data);
                request.ContentType = contentType;
                request.ContentLength = content.Length;

                return await PostToServer(request, content);
            }
            catch (Exception ex)
            {
                if (contentType == "application/x-www-form-urlencoded")
                {
                    return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" + ex.Message + "</Value>\n\t</Response>";

                }
                return  @"{
                        Value: {
                            Type: ""success"",
                            Value: """ + ex.Message + @"""
                        }

                    }";
            }
        }

        /// <summary>
        /// Retrieve the XML encoded status from Microting.
        /// </summary>
        /// <param name="elementId">Identifier of the element to retrieve status of.</param>
        /// <param name="siteId"></param>
        public async Task<string> Status(string elementId, string siteId)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                    $"{addressApi}/gwt/inspection_app/integration/{elementId}?token={token}&protocol={protocolXml}&site_id={siteId}&download=false&delete=false&sdk_ver={dllVersion}");
                request.Method = "GET";

                return await PostToServer(request).ConfigureAwait(false);
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
        /// <param name="siteId"></param>
        public async Task<string> Retrieve(string microtingUuid, string microtingCheckUuid, int siteId)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                    $"{addressApi}/gwt/inspection_app/integration/{microtingUuid}?token={token}&protocol={protocolXml}&site_id={siteId}&download=true&delete=false&last_check_id={microtingCheckUuid}&sdk_ver={dllVersion}");
                request.Method = "GET";

                return await PostToServer(request).ConfigureAwait(false);
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
        /// <param name="siteId"></param>
        public async Task<string> Delete(string elementId, string siteId)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                    $"{addressApi}/gwt/inspection_app/integration/{elementId}?token={token}&protocol={protocolXml}&site_id={siteId}&download=false&delete=true&sdk_ver={dllVersion}");
                request.Method = "GET";

                string result = await PostToServer(request).ConfigureAwait(false);

                if (result.Contains("No database connection information was found"))
                {
                    Thread.Sleep(5000);
                    result = await PostToServer(request).ConfigureAwait(false);
                }

                return result;

            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='error'>ConverterError: " + ex.Message + "</Value>\n\t</Response>";
            }
        }
        //

        // public EntitySearch
        public async Task<string> EntitySearchGroupCreate(string name, string id)
        {
            try
            {
                string xmlData = "<EntityTypes><EntityType><Name><![CDATA[" + name + "]]></Name><Id>" + id + "</Id></EntityType></EntityTypes>";

                WebRequest request = WebRequest.Create(
                    $"{addressApi}/gwt/entity_app/entity_types?token={token}&protocol={protocolEntitySearch}&organization_id={organizationId}&sdk_ver={dllVersion}");
                request.Method = "POST";
                byte[] content = Encoding.UTF8.GetBytes(xmlData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = content.Length;

                string responseXml = await PostToServer(request, content);

                if (responseXml.Contains("workflowState=\"created"))
                    return t.Locate(responseXml, "<MicrotingUUId>", "</");
                return null;
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" + ex.Message + "</Value>\n\t</Response>";
            }
        }

        public async Task<bool> EntitySearchGroupUpdate(int id, string name, string entityGroupMuId)
        {
            string xmlData = "<EntityTypes><EntityType><Name><![CDATA[" + name + "]]></Name><Id>" + id + "</Id></EntityType></EntityTypes>";

            WebRequest request = WebRequest.Create(
                $"{addressApi}/gwt/entity_app/entity_types/{entityGroupMuId}?token={token}&protocol={protocolEntitySearch}&organization_id={organizationId}&sdk_ver={dllVersion}");
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(xmlData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = content.Length;

            string responseXml = await PostToServer(request, content);

            if (responseXml.Contains("workflowState=\"created"))
                return true;
            return false;
        }

        public async Task<bool> EntitySearchGroupDelete(string entityGroupId)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                    $"{addressApi}/gwt/entity_app/entity_types/{entityGroupId}?token={token}&protocol={protocolEntitySearch}&organization_id={organizationId}&sdk_ver={dllVersion}");
                request.Method = "DELETE";
                request.ContentType = "application/x-www-form-urlencoded";  //-- ?

                string responseXml = await PostToServer(request).ConfigureAwait(false);

                if (responseXml.Contains("Value type=\"success"))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchGroupDelete failed", ex);
            }
        }

        public async Task<string> EntitySearchItemCreate(string entitySearchGroupId, string name, string description, string id)
        {
            string xmlData = "<Entities><Entity>" +
                "<EntityTypeId>" + entitySearchGroupId + "</EntityTypeId><Identifier><![CDATA[" + name + "]]></Identifier><Description><![CDATA[" + description + "]]></Description>" +
                "<Km></Km><Colour></Colour><Radiocode></Radiocode>" + //Legacy. To be removed server side
                "<Id>" + id + "</Id>" +
                "</Entity></Entities>";

            WebRequest request = WebRequest.Create(
                $"{addressApi}/gwt/entity_app/entities?token={token}&protocol={protocolEntitySearch}&organization_id={organizationId}&sdk_ver={dllVersion}");
            request.Method = "POST";
            byte[] content = Encoding.UTF8.GetBytes(xmlData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = content.Length;

            string responseXml = await PostToServer(request, content);

            if (responseXml.Contains("workflowState=\"created"))
                return t.Locate(responseXml, "<MicrotingUUId>", "</");
            return null;
        }

        public async Task<bool> EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name, string description, string id)
        {
            string xmlData = "<Entities><Entity>" +
                "<EntityTypeId>" + entitySearchGroupId + "</EntityTypeId><Identifier><![CDATA[" + name + "]]></Identifier><Description><![CDATA[" + description + "]]></Description>" +
                "<Km></Km><Colour></Colour><Radiocode></Radiocode>" + //Legacy. To be removed server side
                "<Id>" + id + "</Id>" +
                "</Entity></Entities>";

            WebRequest request = WebRequest.Create(
                $"{addressApi}/gwt/entity_app/entities/{entitySearchItemId}?token={token}&protocol={protocolEntitySearch}&organization_id={organizationId}&sdk_ver={dllVersion}");
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(xmlData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = content.Length;

            string newUrl = await PostToServer(request, content);
            if (string.IsNullOrEmpty(newUrl))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> EntitySearchItemDelete(string entitySearchItemId)
        {
            WebRequest request = WebRequest.Create(
                $"{addressApi}/gwt/entity_app/entities/{entitySearchItemId}?token={token}&protocol={protocolEntitySearch}&organization_id={organizationId}&sdk_ver={dllVersion}");
            request.Method = "DELETE";
            request.ContentType = "application/x-www-form-urlencoded";  //-- ?

            string responseXml = await PostToServer(request).ConfigureAwait(false);

            if (responseXml.Contains("Value type=\"success"))
                return true;
            return false;
        }
        //

        // public EntitySelect
        public async Task<string> EntitySelectGroupCreate(string name, string id)
        {
            try
            {
                //string xmlData = "{ \"model\" : { \"name\" : \"" + name + "\", \"api_uuid\" : \"" + id + "\" } }";
                JObject contentToServer = JObject.FromObject(new { model = new { name, api_uuid = id } });

                WebRequest request = WebRequest.Create(
                    $"{addressApi}/gwt/inspection_app/searchable_item_groups.json?token={token}&protocol={protocolEntitySelect}&organization_id={organizationId}&sdk_ver={dllVersion}");
                request.Method = "POST";
                byte[] content = Encoding.UTF8.GetBytes(contentToServer.ToString());
                request.ContentType = "application/json; charset=utf-8";
                request.ContentLength = content.Length;

                string responseXml = await PostToServer(request, content);

                if (responseXml.Contains("workflow_state\": \"created"))
                    return t.Locate(responseXml, "\"id\": \"", "\"");
                return null;
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" + ex.Message + "</Value>\n\t</Response>";
            }
        }

        public async Task<bool> EntitySelectGroupUpdate(int id, string name, string entityGroupMuId)
        {
            JObject contentToServer = JObject.FromObject(new { model = new { name, api_uuid = id } });

            WebRequest request = WebRequest.Create(
                $"{addressApi}/gwt/inspection_app/searchable_item_groups/{entityGroupMuId}?token={token}&protocol={protocolEntitySelect}&organization_id={organizationId}&sdk_ver={dllVersion}");
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;

            string newUrl = await PostToServerGetRedirect(request, content);

            request = WebRequest.Create($"{newUrl}?token={token}");
            request.Method = "GET";

            string response = await PostToServer(request).ConfigureAwait(false);

            return response.Contains("workflow_state\": \"created");
        }

        public async Task<bool> EntitySelectGroupDelete(string entityGroupId)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                    $"{addressApi}/gwt/inspection_app/searchable_item_groups/{entityGroupId}.json?token={token}&protocol={protocolEntitySelect}&organization_id={organizationId}&sdk_ver={dllVersion}");
                request.Method = "DELETE";
                request.ContentType = "application/json; charset=utf-8";

                string newUrl = await PostToServerGetRedirect(request).ConfigureAwait(false);

                request = WebRequest.Create($"{newUrl}?token={token}");
                request.Method = "GET";

                string responseXml = await PostToServer(request).ConfigureAwait(false);

                return responseXml.Contains("workflow_state\": \"removed");
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchGroupDelete failed", ex);
            }
        }

        public async Task<string> EntitySelectItemCreate(string entitySelectGroupId, string name, int displayIndex, string id)
        {
            JObject contentToServer = JObject.FromObject(new { model = new { data = name, api_uuid = id, display_order = displayIndex, searchable_group_id = entitySelectGroupId } });

            WebRequest request = WebRequest.Create(
                $"{addressApi}/gwt/inspection_app/searchable_items.json?token={token}&protocol={protocolEntitySelect}&organization_id={organizationId}&sdk_ver={dllVersion}");
            request.Method = "POST";
            byte[] content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;

            string responseXml = await PostToServer(request, content);

            if (responseXml.Contains("workflow_state\": \"created"))
                return t.Locate(responseXml, "\"id\": \"", "\"");
            return null;
        }

        public async Task<bool> EntitySelectItemUpdate(string entitySelectGroupId, string entitySelectItemId, string name, int displayIndex, string ownUuid)
        {
            JObject contentToServer = JObject.FromObject(new { model = new { data = name, api_uuid = ownUuid, display_order = displayIndex, searchable_group_id = entitySelectGroupId } });

            WebRequest request = WebRequest.Create(
                $"{addressApi}/gwt/inspection_app/searchable_items/{entitySelectItemId}?token={token}&protocol={protocolEntitySelect}&organization_id={organizationId}&sdk_ver={dllVersion}");
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;

			string newUrl = await PostToServerGetRedirect(request, content);

			request = WebRequest.Create($"{newUrl}?token={token}");
			request.Method = "GET";

			string responseXml = await PostToServer(request).ConfigureAwait(false);
			return responseXml.Contains("workflow_state\": \"created");
		}

        public async Task<bool> EntitySelectItemDelete(string entitySelectItemId)
        {
            WebRequest request = WebRequest.Create(
                $"{addressApi}/gwt/inspection_app/searchable_items/{entitySelectItemId}.json?token={token}&protocol={protocolEntitySelect}&organization_id={organizationId}&sdk_ver={dllVersion}");
            request.Method = "DELETE";
            request.ContentType = "application/json; charset=utf-8";

			string newUrl = await PostToServerGetRedirect(request).ConfigureAwait(false);

			request = WebRequest.Create($"{newUrl}?token={token}");
			request.Method = "GET";

			string responseXml = await PostToServer(request).ConfigureAwait(false);
			return responseXml.Contains("workflow_state\": \"removed");

		}
		//

		// public PdfUpload
		public async Task<bool> PdfUpload(string name, string hash)
        {
            try
            {
                using WebClient client = new WebClient();
                string url =
                    $"{addressPdfUpload}/data_uploads/upload?token={token}&hash={hash}&extension=pdf&sdk_ver={dllVersion}";
                await client.UploadFileTaskAsync(url, name);

                return true;
            }
            catch
            {
                return false;
            }
        }
        //

        // public TemplateDisplayIndexChange
        public async Task<string> TemplateDisplayIndexChange(string microtingUId, int siteId, int newDisplayIndex)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                    $"{addressApi}/gwt/inspection_app/integration/{microtingUId}?token={token}&protocol={protocolXml}&site_id={siteId}&download=false&delete=false&update_attributes=true&display_order={newDisplayIndex}&sdk_ver={dllVersion}");
                request.Method = "GET";

                return await PostToServer(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='error'>ConverterError: " + ex.Message + "</Value>\n\t</Response>";
                //return false;
            }
        }
        //

        // public site
        public async Task<string> SiteCreate(string name, string languageCode)
        {
            JObject contentToServer = JObject.FromObject(new { name });
            WebRequest request = WebRequest.Create(
                $"{newAddressBasic}/Site?token={token}&name={Uri.EscapeDataString(name)}&languageCode={languageCode}&sdkVersion={dllVersion}");
            request.Method = "POST";
            byte[] content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            string newUrl = await PostToServerGetRedirect(request, content);

            request = WebRequest.Create($"{newAddressBasic}{newUrl}?token={token}");
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            string response = await PostToServer(request).ConfigureAwait(false);

            return response;
        }

        public async Task<bool> SiteUpdate(int id, string name, string languageCode)
        {
            JObject contentToServer = JObject.FromObject(new { name });
            WebRequest request = WebRequest.Create(
                $"{newAddressBasic}/Site/{id}?token={token}&name={Uri.EscapeDataString(name)}&languageCode={languageCode}&sdkVersion={dllVersion}");
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            string newUrl = await PostToServerGetRedirect(request, content);

            if (string.IsNullOrEmpty(newUrl))
            {
                return false;
            }

            return true;
        }

        public async Task<string> SiteDelete(int id)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                    $"{newAddressBasic}/Site/{id}?token={token}&sdkVersion={dllVersion}");
                request.Method = "DELETE";
                request.ContentType = "application/x-www-form-urlencoded";  //-- ?
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                string newUrl = await PostToServerGetRedirect(request).ConfigureAwait(false);

                request = WebRequest.Create($"{newAddressBasic}{newUrl}?token={token}");
                request.Method = "GET";
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                return await PostToServer(request).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                throw new Exception("SiteDelete failed", ex);
            }
        }

        public async Task<string> SiteLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create($"{newAddressBasic}/Site?token={token}&sdkVersion={dllVersion}");
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            return await PostToServer(request).ConfigureAwait(false);
        }
        //

        // public Worker
        public async Task<string> WorkerCreate(string firstName, string lastName, string email)
        {
            JObject contentToServer = JObject.FromObject(new { first_name = firstName, last_name = lastName, email });
            WebRequest request = WebRequest.Create(
                $"{newAddressBasic}/User?token={token}&firstName={Uri.EscapeDataString(firstName)}&lastName={Uri.EscapeDataString(lastName)}&email={email}&sdkVersion={dllVersion}");
            request.Method = "POST";
            byte[] content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            string newUrl = await PostToServerGetRedirect(request, content);

            request = WebRequest.Create($"{newAddressBasic}{newUrl}?token={token}");
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            string response = await PostToServer(request).ConfigureAwait(false);

            return response;
        }

        public async Task<bool> WorkerUpdate(int id, string firstName, string lastName, string email)
        {
            JObject contentToServer = JObject.FromObject(new { first_name = firstName, last_name = lastName, email });
            WebRequest request = WebRequest.Create(
                $"{newAddressBasic}/User/{id}?token={token}&firstName={Uri.EscapeDataString(firstName)}&lastName={Uri.EscapeDataString(lastName)}&email={email}&sdkVersion={dllVersion}");
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            string newUrl = await PostToServerGetRedirect(request, content);

            if (string.IsNullOrEmpty(newUrl))
            {
                return false;
            }
            return true;
        }

        public async Task<string> WorkerDelete(int id)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                    $"{newAddressBasic}/User/{id}?token={token}&sdkVersion={dllVersion}");
                request.Method = "DELETE";
                request.ContentType = "application/x-www-form-urlencoded";  //-- ?
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                string newUrl = await PostToServerGetRedirect(request).ConfigureAwait(false);

                request = WebRequest.Create($"{newAddressBasic}{newUrl}?token={token}");
                request.Method = "GET";
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                return await PostToServer(request).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                throw new Exception("WorkerDelete failed", ex);
            }
        }

        public async Task<string> WorkerLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create($"{newAddressBasic}/User?token={token}&sdkVersion={dllVersion}");
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            return await PostToServer(request).ConfigureAwait(false);
        }
        //

        // public SiteWorker
        public async Task<string> SiteWorkerCreate(int siteId, int workerId)
        {
            JObject contentToServer = JObject.FromObject(new { user_id = workerId, site_id = siteId });
            WebRequest request = WebRequest.Create(
                $"{newAddressBasic}/Worker?token={token}&siteid={siteId}&userId={workerId}&sdkVersion={dllVersion}");
            request.Method = "POST";
            byte[] content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            string newUrl = await PostToServerGetRedirect(request, content);

            request = WebRequest.Create($"{newAddressBasic}{newUrl}?token={token}");
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            string response = await PostToServer(request).ConfigureAwait(false);

            return response;
        }

        public async Task<string> SiteWorkerDelete(int id)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                    $"{newAddressBasic}/Worker/{id}?token={token}&sdkVersion={dllVersion}");
                request.Method = "DELETE";
                request.ContentType = "application/x-www-form-urlencoded";  //-- ?
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                string newUrl = await PostToServerGetRedirect(request).ConfigureAwait(false);

                request = WebRequest.Create($"{newAddressBasic}{newUrl}?token={token}");
                request.Method = "GET";
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                return await PostToServer(request).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                throw new Exception("SiteWorkerDelete failed", ex);
            }
        }

        public async Task<string> SiteWorkerLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create($"{newAddressBasic}/Worker?token={token}&sdkVersion={dllVersion}");
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            return await PostToServer(request).ConfigureAwait(false);
        }

        //

        // folder

        public async Task<string> FolderLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create($"{newAddressBasic}/Folder?token={token}&sdkVersion={dllVersion}");
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            return await PostToServer(request).ConfigureAwait(false);
        }

        public async Task<string> FolderCreate(int uuid, int? parentId)
        {
            WebRequest request = WebRequest.Create(
                $"{newAddressBasic}/Folder?token={token}&uuid={uuid}&parentId={parentId}&sdkVersion={dllVersion}");
            request.Method = "POST";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            string newUrl = await PostToServerGetRedirect(request);

            request = WebRequest.Create($"{newAddressBasic}{newUrl}?token={token}");
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            string response = await PostToServer(request).ConfigureAwait(false);

            return response;
        }

        public async Task<bool> FolderUpdate(int id, string name, string description, string languageCode, int? parentId)
        {
            //JObject contentToServer = JObject.FromObject(new { languageCode, name = Uri.EscapeDataString(name), description = Uri.EscapeDataString(description), parent_id = parentId });
            WebRequest request = WebRequest.Create(
                $"{newAddressBasic}/Folder/{id}?token={token}&languageCode={languageCode}&name={Uri.EscapeDataString(name)}&description={Uri.EscapeDataString(description)}&parentId={parentId}&sdkVersion={dllVersion}");
            request.Method = "PUT";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            await PostToServerGetRedirect(request);
            return true;
        }

        public async Task<string> FolderDelete(int id)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                    $"{newAddressBasic}/Folder/{id}?token={token}&sdkVersion={dllVersion}");
                request.Method = "DELETE";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                string newUrl = await PostToServerGetRedirect(request).ConfigureAwait(false);

                request = WebRequest.Create($"{newAddressBasic}{newUrl}?token={token}");
                request.Method = "GET";
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                return await PostToServer(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("FolderDelete failed", ex);
            }
        }
        //

        // public Unit
        public async Task<string> UnitUpdate(int id, bool newOtp, int siteId, bool pushEnabled, bool syncDelayEnabled, bool syncDialogEnabled)
        {
            JObject contentToServer = JObject.FromObject(new { model = new { unit_id = id } });
            WebRequest request = WebRequest.Create(
                $"{newAddressBasic}/Unit/{id}?token={token}&newOtp=true&pushEnabled={pushEnabled}&siteId={siteId}&syncDelayEnabled={syncDelayEnabled}&syncDialogEnabled={syncDialogEnabled}&sdkVersion={dllVersion}");
            request.Method = "PUT";
            byte[] content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = content.Length;
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            string newUrl = await PostToServerGetRedirect(request, content);

            request = WebRequest.Create($"{newAddressBasic}{newUrl}?token={token}&sdkVersion={dllVersion}");
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            return await PostToServer(request).ConfigureAwait(false);
        }

        public async Task<string> UnitLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create($"{newAddressBasic}/Unit?token={token}&sdkVersion={dllVersion}");
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            return await PostToServer(request).ConfigureAwait(false);
        }

        public async Task<string> UnitDelete(int id)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                    $"{newAddressBasic}/Unit/{id}?token={token}&sdkVersion={dllVersion}");
                request.Method = "DELETE";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                string newUrl = await PostToServerGetRedirect(request).ConfigureAwait(false);

                request = WebRequest.Create($"{newAddressBasic}{newUrl}?token={token}&sdkVersion={dllVersion}");
                request.Method = "GET";
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                return await PostToServer(request).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                throw new Exception("UnitDelete failed", ex);
            }
        }

        public async Task<string> UnitMove(int unitId, int siteId)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                    $"{newAddressBasic}/Unit/{unitId}?token={token}&sdkVersion={dllVersion}&siteId={siteId}");
                request.Method = "PUT";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                string newUrl = await PostToServerGetRedirect(request).ConfigureAwait(false);

                request = WebRequest.Create($"{newAddressBasic}{newUrl}?token={token}&sdkVersion={dllVersion}");
                request.Method = "GET";
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                return await PostToServer(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("UnitDelete failed", ex);
            }
        }

        public async Task<string> UnitCreate(int siteMicrotingUid)
        {
            try
            {
                JObject contentToMicroting = JObject.FromObject(new { site_id = siteMicrotingUid } );
                WebRequest request = WebRequest.Create(
                    $"{newAddressBasic}/Unit/?token={token}&siteId={siteMicrotingUid}&sdkVersion={dllVersion}");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                string newUrl = await PostToServerGetRedirect(request).ConfigureAwait(false);

                request = WebRequest.Create($"{newAddressBasic}{newUrl}?token={token}&sdkVersion={dllVersion}");
                request.Method = "GET";
                request.Headers.Add(HttpRequestHeader.Authorization, token);

                return await PostToServer(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("UnitCreate failed", ex);
            }
        }

        //

        // public Organization
        public async Task<string> OrganizationLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create(newAddressBasic + "/Organization?token=" + token + "&sdkVersion=" + dllVersion);
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            return await PostToServer(request).ConfigureAwait(false);
        }
        //

        // SpeechToText
        public async Task<int> SpeechToText(Stream pathToAudioFile, string language, string extension)
        {
            try
            {
                using WebClient client = new WebClient();
                string url = $"{addressSpeechToText}/audio/?token={token}&sdk_ver={dllVersion}&lang={language}";
                using var tempFiles = new TempFileCollection();
                string file = tempFiles.AddExtension(extension);
                await using (FileStream fs = File.OpenWrite(file))
                {
                    await pathToAudioFile.CopyToAsync(fs);
                    await fs.FlushAsync();
                    fs.Close();
                }
                byte[] responseArray = await client.UploadFileTaskAsync(url, file);

                string result = Encoding.UTF8.GetString(responseArray);
                var parsedData = JToken.Parse(result);
                return int.Parse(parsedData["id"]?.ToString() ?? throw new InvalidOperationException());
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to upload the file", ex);
            }
        }

        public async Task<JToken> SpeechToText(int requestId)
        {
            WebRequest request = WebRequest.Create(
                $"{addressSpeechToText}/audio/{requestId}?token={token}&sdk_ver={dllVersion}");
            request.Method = "GET";

            string result = await PostToServer(request).ConfigureAwait(false);
            JToken parsedData = JToken.Parse(result);
            return parsedData;
        }
        //

        // InSight

        // SurveyConfiguration

        public async Task<bool> SetSurveyConfiguration(int id, int siteId, bool addSite)
        {
            if (addSite)
            {
                WebRequest request = WebRequest.Create(
                    $"{addressBasic}/v1/survey_configurations/{id}?token={token}&add_site=true&site_id={siteId}&sdk_ver={dllVersion}");
                request.Method = "GET";

                await PostToServer(request).ConfigureAwait(false);
            }
            else
            {
                WebRequest request = WebRequest.Create(
                    $"{addressBasic}/v1/survey_configurations/{id}?token={token}&remove_site=true&site_id={siteId}&sdk_ver={dllVersion}");
                request.Method = "GET";

                await PostToServer(request).ConfigureAwait(false);
            }

            return true;
        }

        public Task<string> GetAllSurveyConfigurations()
        {
            WebRequest request = WebRequest.Create(
                $"{addressBasic}/v1/survey_configurations?token={token}&sdk_ver={dllVersion}");
            request.Method = "GET";

            return PostToServer(request);
        }

        public Task<string> GetSurveyConfiguration(int id)
        {
            WebRequest request = WebRequest.Create(
                $"{addressBasic}/v1/survey_configurations/{id}?token={token}&sdk_ver={dllVersion}");
            request.Method = "GET";

            return PostToServer(request);
        }


        //

        // QuestionSet

        public Task<string> GetAllQuestionSets()
        {

            WebRequest request = WebRequest.Create(
                $"{addressBasic}/v1/question_sets?token={token}&sdk_ver={dllVersion}");
            request.Method = "GET";

            return PostToServer(request);
        }

        public Task<string> GetQuestionSet(int id)
        {
            WebRequest request = WebRequest.Create(
                $"{addressBasic}/v1/question_sets/{id}?token={token}&sdk_ver={dllVersion}");
            request.Method = "GET";

            return PostToServer(request);
        }

        //

        // Answer

        public Task<string> GetLastAnswer(int questionSetId, int lastAnswerId)
        {
            WebRequest request = WebRequest.Create(
                $"{addressBasic}/v1/answers/{questionSetId}?token={token}&sdk_ver={dllVersion}&last_answer_id={lastAnswerId}");
            request.Method = "GET";

            return PostToServer(request);
        }

        public Task SendPushMessage(int microtingSiteId, string header, string body, int microtingUuid)
        {
            WebRequest request = WebRequest.Create(
                $"{newAddressBasic}/PushMessage?SiteId={microtingSiteId}&token={token}&Header={header}&Body={body}&sdkVersion={dllVersion}&uuid={microtingUuid}");
            request.Method = "POST";
            request.Headers.Add(HttpRequestHeader.Authorization, token);

            return PostToServer(request);
        }

        //

        //

        //

        // private
        private async Task<string> PostToServer(WebRequest request, byte[] content)
        {
            Console.WriteLine($"[DBG] Http.PostToServer: Calling {request.RequestUri}");

            // Hack for ignoring certificate validation.
            DateTime start = DateTime.UtcNow;
            WriteDebugConsoleLogEntry("Http.PostToServer", $"Called at {start}");
            ServicePointManager.ServerCertificateValidationCallback = Validator;
            Stream dataRequestStream = await request.GetRequestStreamAsync().ConfigureAwait(false);
            await dataRequestStream.WriteAsync(content, 0, content.Length);
            dataRequestStream.Close();

            WebResponse response = await request.GetResponseAsync().ConfigureAwait(false);

            Stream dataResponseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataResponseStream);
            string responseFromServer = await reader.ReadToEndAsync();

            // Clean up the streams.

            reader.Close();
            dataResponseStream.Close();
            response.Close();

            WriteDebugConsoleLogEntry("Http.PostToServer", $"Finished at {DateTime.UtcNow} - took {(start - DateTime.UtcNow).ToString()}");
            return responseFromServer;
        }

        private async Task<string> PostToServerGetRedirect(WebRequest request, byte[] content)
        {
            Console.WriteLine($"[DBG] Http.PostToServerGetRedirect: Calling {request.RequestUri}");

            // Hack for ignoring certificate validation.
            ServicePointManager.ServerCertificateValidationCallback = Validator;

            Stream dataRequestStream = await request.GetRequestStreamAsync().ConfigureAwait(false);
            await dataRequestStream.WriteAsync(content, 0, content.Length).ConfigureAwait(false);
            dataRequestStream.Close();

            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.CookieContainer = new CookieContainer();
            httpRequest.AllowAutoRedirect = false;

            string newUrl = "";
            HttpWebResponse response = (HttpWebResponse) await httpRequest.GetResponseAsync();
            if (response.StatusCode == HttpStatusCode.Found)
            {
                newUrl = response.Headers["Location"];
            }

            return newUrl;
        }

        private async Task<string> PostToServerGetRedirect(WebRequest request)
        {
            Console.WriteLine($"[DBG] Http.PostToServerGetRedirect: Calling {request.RequestUri}");

            // Hack for ignoring certificate validation.
            ServicePointManager.ServerCertificateValidationCallback = Validator;

            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.CookieContainer = new CookieContainer();
            httpRequest.AllowAutoRedirect = false;

            HttpWebResponse response;

            string newUrl = "";
            response = (HttpWebResponse) await httpRequest.GetResponseAsync().ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.Found)
            {
                newUrl = response.Headers["Location"];
            }

            return newUrl;
        }

        private async Task<string> PostToServerNoRedirect(WebRequest request, byte[] content)
        {
            Console.WriteLine($"[DBG] Http.PostToServerNoRedirect: Calling {request.RequestUri}");

            // Hack for ignoring certificate validation.
            ServicePointManager.ServerCertificateValidationCallback = Validator;

            Stream dataRequestStream = await request.GetRequestStreamAsync().ConfigureAwait(false);
            await dataRequestStream.WriteAsync(content, 0, content.Length).ConfigureAwait(false);
            dataRequestStream.Close();

            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.CookieContainer = new CookieContainer();
            httpRequest.AllowAutoRedirect = false;

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            Stream dataResponseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataResponseStream ?? throw new InvalidOperationException());
            string responseFromServer = await reader.ReadToEndAsync().ConfigureAwait(false);

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

        private async Task<string> PostToServer(WebRequest request)
        {
            Console.WriteLine($"[DBG] Http.PostToServer: Calling {request.RequestUri}");
            // Hack for ignoring certificate validation.

            ServicePointManager.ServerCertificateValidationCallback = Validator;

            WebResponse response = await request.GetResponseAsync().ConfigureAwait(false);
            Stream dataResponseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataResponseStream ?? throw new InvalidOperationException());
            string responseFromServer = await reader.ReadToEndAsync().ConfigureAwait(false);

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

        private async Task<string> PostToServerNoRedirect(WebRequest request)
        {
            Console.WriteLine($"[DBG] Http.PostToServerNoRedirect: Calling {request.RequestUri}");

            // Hack for ignoring certificate validation.
            ServicePointManager.ServerCertificateValidationCallback = Validator;

            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.CookieContainer = new CookieContainer();
            httpRequest.AllowAutoRedirect = false;

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            Stream dataResponseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataResponseStream ?? throw new InvalidOperationException());
            string responseFromServer = await reader.ReadToEndAsync().ConfigureAwait(false);

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

        private void WriteDebugConsoleLogEntry(string classMethodName, string message)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[DBG] {classMethodName}: {message}");
            Console.ForegroundColor = oldColor;
        }

        private void WriteErrorConsoleLogEntry(string classMethodName, string message)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERR] {classMethodName}: {message}");
            Console.ForegroundColor = oldColor;
        }

        //
    }
}