/*
The MIT License (MIT)

Copyright (c) 2007 - 2021 Microting A/S

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
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microting.eForm.Communication
{
    using System.Linq;
    using System.Net.Http.Headers;

    public class Http : IHttp
    {
        private const string ProtocolXml = "6";
        private const string ProtocolEntitySearch = "1";
        private const string ProtocolEntitySelect = "4";

        private readonly string token;
        private readonly string addressApi;
        private readonly string addressBasic;
        private readonly string newAddressBasic;
        private readonly string addressPdfUpload;
        private readonly string addressSpeechToText;
        private readonly string organizationId;
        private readonly string dllVersion;

        private readonly Tools t = new Tools();
        //private object _lock = new object(); // todo maybe need delete
        public Http(string token, string comAddressBasic, string comAddressApi, string comOrganizationId, string comAddressPdfUpload, string comSpeechToText)
        {
            this.token = token;
            addressBasic = comAddressBasic;
            addressApi = comAddressApi;
            addressPdfUpload = comAddressPdfUpload;
            organizationId = comOrganizationId;
            addressSpeechToText = comSpeechToText;
            newAddressBasic = "https://microcore.microting.com";

            dllVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        }

        // public API

        private async Task<string> HttpPost(string url, byte[] content, string contentType = null, bool addToken = false)
        {
            try
            {
                var start = DateTime.UtcNow;
                WriteDebugConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}",
                    $"called at {start}");

                using var httpClient = new HttpClient();
                if (contentType != null)
                {
                    httpClient.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue(contentType));
                }
                if (addToken)
                {
                    httpClient.DefaultRequestHeaders
                        .Add("Authorization", token);
                }

                var response = await httpClient.PostAsync(url, new ByteArrayContent(content));

                if (response.StatusCode == HttpStatusCode.Found)
                {
                    return response.Headers.Location.ToString();
                }
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                WriteDebugConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}", $"Finished at {DateTime.UtcNow} - took {start - DateTime.UtcNow}");
                return responseBody;
            }
            catch (Exception ex)
            {
                WriteErrorConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}", ex.Message);
                throw;
            }
        }

        private async Task<string> HttpPut(string url, byte[] content, string contentType = null, bool addToken = false)
        {
            try
            {
                var start = DateTime.UtcNow;
                WriteDebugConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}",
                    $"called at {start}");

                using var httpClient = new HttpClient();
                if (contentType != null)
                {
                    httpClient.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue(contentType));
                }
                if (addToken)
                {
                    httpClient.DefaultRequestHeaders
                        .Add(HttpRequestHeader.Authorization.ToString(), token);
                }

                var response = await httpClient.PutAsync(url, new ByteArrayContent(content));

                if (response.StatusCode == HttpStatusCode.Found)
                {
                    return response.Headers.Location.ToString();
                }
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                WriteDebugConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}", $"Finished at {DateTime.UtcNow} - took {start - DateTime.UtcNow}");
                return responseBody;
            }
            catch (Exception ex)
            {
                WriteErrorConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}", ex.Message);
                throw;
            }
        }

        private async Task<string> HttpGet(string url, string contentType = null, bool addToken = false)
        {
            try
            {
                var start = DateTime.UtcNow;
                WriteDebugConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}",
                    $"called at {start}");

                using var httpClient = new HttpClient();
                if (contentType != null)
                {
                    httpClient.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue(contentType));
                }
                if (addToken)
                {
                    httpClient.DefaultRequestHeaders
                        .Add(HttpRequestHeader.Authorization.ToString(), token);
                }

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                WriteDebugConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}", $"Finished at {DateTime.UtcNow} - took {start - DateTime.UtcNow}");
                return responseBody;
            }
            catch (Exception ex)
            {
                WriteErrorConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}", ex.Message);
                throw;
            }
        }

        private async Task<string> HttpDelete(string url, string contentType = null, bool addToken = false)
        {
            try
            {
                var start = DateTime.UtcNow;
                WriteDebugConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}",
                    $"called at {start}");

                using var httpClient = new HttpClient();
                if (contentType != null)
                {
                    httpClient.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue(contentType));
                }
                if (addToken)
                {
                    httpClient.DefaultRequestHeaders
                        .Add(HttpRequestHeader.Authorization.ToString(), token);
                }

                var response = await httpClient.DeleteAsync(url);
                if (response.StatusCode == HttpStatusCode.Found)
                {
                    return response.Headers.Location.ToString();
                }
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                WriteDebugConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}", $"Finished at {DateTime.UtcNow} - took {start - DateTime.UtcNow}");
                return responseBody;
            }
            catch (Exception ex)
            {
                WriteErrorConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}", ex.Message);
                throw;
            }
        }

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
                WriteDebugConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}", $"called at {DateTime.UtcNow}");
                var url = $"{addressApi}/gwt/inspection_app/integration/?token={token}&protocol={ProtocolXml}&site_id={siteId}&sdk_ver={dllVersion}";
                var content = Encoding.UTF8.GetBytes(data);

                return await HttpPost(url, content, contentType);
            }
            catch (Exception ex)
            {
                if (contentType == "application/x-www-form-urlencoded")
                {
                    return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" + ex.Message + "</Value>\n\t</Response>";

                }
                return $"{{\r\nValue: {{\r\nType: \"success\",\r\nValue: \"{ex.Message}\"}}}}";
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
                var url =
                    $"{addressApi}/gwt/inspection_app/integration/{elementId}?token={token}&protocol={ProtocolXml}&site_id={siteId}&download=false&delete=false&sdk_ver={dllVersion}";
                return await HttpGet(url);
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
                var url =
                    $"{addressApi}/gwt/inspection_app/integration/{microtingUuid}?token={token}&protocol={ProtocolXml}&site_id={siteId}&download=true&delete=false&last_check_id={microtingCheckUuid}&sdk_ver={dllVersion}";
                return await HttpGet(url);
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
                var url =
                    $"{addressApi}/gwt/inspection_app/integration/{elementId}?token={token}&protocol={ProtocolXml}&site_id={siteId}&download=false&delete=true&sdk_ver={dllVersion}";
                var result = await HttpGet(url);

                if (result.Contains("No database connection information was found"))
                {
                    Thread.Sleep(5000);
                    result = await HttpGet(url);
                }

                return result;

            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='error'>ConverterError: " + ex.Message + "</Value>\n\t</Response>";
            }
        }

        // public EntitySearch
        public async Task<string> EntitySearchGroupCreate(string name, string id)
        {
            try
            {
                var xmlData = "<EntityTypes><EntityType><Name><![CDATA[" + name + "]]></Name><Id>" + id + "</Id></EntityType></EntityTypes>";
                const string contentType = "application/x-www-form-urlencoded";
                var content = Encoding.UTF8.GetBytes(xmlData);
                var url = $"{addressApi}/gwt/entity_app/entity_types?token={token}&protocol={ProtocolEntitySearch}&organization_id={organizationId}&sdk_ver={dllVersion}";
                var responseBody = await HttpPost(url, content, contentType);
                //var responseXml = await PostToServer(request, content);

                if (responseBody.Contains("workflowState=\"created"))
                {
                    return t.Locate(responseBody, "<MicrotingUUId>", "</");
                }
                return null;
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" + ex.Message + "</Value>\n\t</Response>";
            }
        }

        public async Task<bool> EntitySearchGroupUpdate(int id, string name, string entityGroupMuId)
        {
            var xmlData = "<EntityTypes><EntityType><Name><![CDATA[" + name + "]]></Name><Id>" + id + "</Id></EntityType></EntityTypes>";
            var url =
                $"{addressApi}/gwt/entity_app/entity_types/{entityGroupMuId}?token={token}&protocol={ProtocolEntitySearch}&organization_id={organizationId}&sdk_ver={dllVersion}";
            const string contentType = "application/x-www-form-urlencoded";
            var content = Encoding.UTF8.GetBytes(xmlData);
            var responseXml = await HttpPut(url, content, contentType);
            return responseXml.Contains("workflowState=\"created");
        }

        public async Task<bool> EntitySearchGroupDelete(string entityGroupId)
        {
            try
            {
                const string contentType = "application/x-www-form-urlencoded";
                var url =
                    $"{addressApi}/gwt/entity_app/entity_types/{entityGroupId}?token={token}&protocol={ProtocolEntitySearch}&organization_id={organizationId}&sdk_ver={dllVersion}";
                var responseXml = await HttpDelete(url, contentType);

                return responseXml.Contains("Value type=\"success");
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchGroupDelete failed", ex);
            }
        }

        public async Task<string> EntitySearchItemCreate(string entitySearchGroupId, string name, string description, string id)
        {
            var content = Encoding.UTF8
                .GetBytes("<Entities><Entity>" +
                            $"<EntityTypeId>{entitySearchGroupId}</EntityTypeId>" +
                            $"<Identifier><![CDATA[{ name }]]></Identifier>" +
                            $"<Description><![CDATA[{ description }]]></Description>" +
                            "<Km></Km><Colour></Colour><Radiocode></Radiocode>" + // todo Legacy. To be removed server side
                            $"<Id>{ id }</Id>" +
                            "</Entity></Entities>");
            var url =
                $"{addressApi}/gwt/entity_app/entities?token={token}&protocol={ProtocolEntitySearch}&organization_id={organizationId}&sdk_ver={dllVersion}";

            var responseXml = await HttpPost(url, content, "application/x-www-form-urlencoded");

            if (responseXml.Contains("workflowState=\"created"))
            {
                return t.Locate(responseXml, "<MicrotingUUId>", "</");
            }
            return null;
        }

        public async Task<bool> EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name, string description, string id)
        {
            var content = Encoding.UTF8
                .GetBytes("<Entities><Entity>" +
                          $"<EntityTypeId>{entitySearchGroupId}</EntityTypeId>" +
                          $"<Identifier><![CDATA[{ name }]]></Identifier>" +
                          $"<Description><![CDATA[{ description }]]></Description>" +
                          "<Km></Km><Colour></Colour><Radiocode></Radiocode>" + // todo Legacy. To be removed server side
                          $"<Id>{ id }</Id>" +
                          "</Entity></Entities>");
            var url =
                $"{addressApi}/gwt/entity_app/entities/{entitySearchItemId}?token={token}&protocol={ProtocolEntitySearch}&organization_id={organizationId}&sdk_ver={dllVersion}";

            var newUrl = await HttpPut(url, content, "application/x-www-form-urlencoded");
            return !string.IsNullOrEmpty(newUrl);
        }

        public async Task<bool> EntitySearchItemDelete(string entitySearchItemId)
        {
            var url =
                $"{addressApi}/gwt/entity_app/entities/{entitySearchItemId}?token={token}&protocol={ProtocolEntitySearch}&organization_id={organizationId}&sdk_ver={dllVersion}";
            var responseXml = await HttpDelete(url, "application/x-www-form-urlencoded");
            return responseXml.Contains("Value type=\"success");
        }

        // public EntitySelect
        public async Task<string> EntitySelectGroupCreate(string name, string id)
        {
            try
            {
                var content = Encoding.UTF8.GetBytes(JObject.FromObject(new { model = new { name, api_uuid = id } }).ToString());
                var url =
                    $"{addressApi}/gwt/inspection_app/searchable_item_groups.json?token={token}&protocol={ProtocolEntitySelect}&organization_id={organizationId}&sdk_ver={dllVersion}";
                var responseXml = await HttpPost(url, content, "application/json; charset=utf-8"); // todo maybe not need content type
                if (responseXml.Contains("workflow_state\": \"created"))
                {
                    return t.Locate(responseXml, "\"id\": \"", "\"");
                }

                return null;
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" + ex.Message + "</Value>\n\t</Response>";
            }
        }

        public async Task<bool> EntitySelectGroupUpdate(int id, string name, string entityGroupMuId)
        {
            var content = Encoding.UTF8.GetBytes(JObject.FromObject(new { model = new { name, api_uuid = id } }).ToString());
            var url =
                $"{addressApi}/gwt/inspection_app/searchable_item_groups/{entityGroupMuId}?token={token}&protocol={ProtocolEntitySelect}&organization_id={organizationId}&sdk_ver={dllVersion}";
            var newUrl = await HttpPut(url, content, "application/json; charset=utf-8"); // todo maybe not need content type

            // var url2 = $"{newUrl}?token={token}";
            var response = await HttpGet(newUrl);

            return response.Contains("workflow_state\": \"created");
        }

        public async Task<bool> EntitySelectGroupDelete(string entityGroupId)
        {
            try
            {
                var url =
                    $"{addressApi}/gwt/inspection_app/searchable_item_groups/{entityGroupId}.json?token={token}&protocol={ProtocolEntitySelect}&organization_id={organizationId}&sdk_ver={dllVersion}";
                var newUrl = await HttpDelete(url, "application/json"); // todo maybe not need content type

                // var url2 = $"{newUrl}?token={token}";
                var response = await HttpGet(newUrl);
                return response.Contains("workflow_state\": \"removed");
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchGroupDelete failed", ex);
            }
        }

        public async Task<string> EntitySelectItemCreate(string entitySelectGroupId, string name, int displayIndex, string id)
        {
            var contentToServer = JObject.FromObject(new { model = new { data = name, api_uuid = id, display_order = displayIndex, searchable_group_id = entitySelectGroupId } });

            var url =
                $"{addressApi}/gwt/inspection_app/searchable_items.json?token={token}&protocol={ProtocolEntitySelect}&organization_id={organizationId}&sdk_ver={dllVersion}";
            var response = await HttpPost(url, Encoding.UTF8.GetBytes(contentToServer.ToString()), "application/json; charset=utf-8"); // todo maybe not need content type

            if (response.Contains("workflow_state\": \"created"))
            {
                return t.Locate(response, "\"id\": \"", "\"");
            }

            return null;
        }

        public async Task<bool> EntitySelectItemUpdate(string entitySelectGroupId, string entitySelectItemId, string name, int displayIndex, string ownUuid)
        {
            var contentToServer = JObject.FromObject(new { model = new { data = name, api_uuid = ownUuid, display_order = displayIndex, searchable_group_id = entitySelectGroupId } });
            var url =
                $"{addressApi}/gwt/inspection_app/searchable_items/{entitySelectItemId}?token={token}&protocol={ProtocolEntitySelect}&organization_id={organizationId}&sdk_ver={dllVersion}";
            var newUrl = await HttpPut(url, Encoding.UTF8.GetBytes(contentToServer.ToString()), "application/json; charset=utf-8"); // todo maybe not need content type
            // var url2 = $"{newUrl}?token={token}";
            var response = await HttpGet(newUrl);
            return response.Contains("workflow_state\": \"created");
        }

        public async Task<bool> EntitySelectItemDelete(string entitySelectItemId)
        {

            var url =
                $"{addressApi}/gwt/inspection_app/searchable_items/{entitySelectItemId}.json?token={token}&protocol={ProtocolEntitySelect}&organization_id={organizationId}&sdk_ver={dllVersion}";
            var newUrl = await HttpDelete(url, "application/json; charset=utf-8"); // todo maybe not need content type
            // var url2 = $"{newUrl}?token={token}";
            var response = await HttpGet(newUrl);
            return response.Contains("workflow_state\": \"removed");

        }

        public async Task<bool> PdfUpload(string name, string hash)
        {
            try
            {
                var url =
                    $"{addressPdfUpload}/data_uploads/upload?token={token}&hash={hash}&extension=pdf&sdk_ver={dllVersion}";
                var form = new MultipartFormDataContent();
                var stream = new FileStream(name, FileMode.Open);
                var content = new StreamContent(stream);
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = name,
                    FileName = name.Split('\\').Last(),
                };
                form.Add(content);
                using var client = new HttpClient();
                await client.PostAsync(url, form);
                return true;
            }
            catch (Exception ex)
            {
                WriteErrorConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}", ex.Message);
                return false;
            }
        }

        public async Task<string> TemplateDisplayIndexChange(string microtingUId, int siteId, int newDisplayIndex)
        {
            try
            {
                var url =
                    $"{addressApi}/gwt/inspection_app/integration/{microtingUId}?token={token}&protocol={ProtocolXml}&site_id={siteId}&download=false&delete=false&update_attributes=true&display_order={newDisplayIndex}&sdk_ver={dllVersion}";
                var response = await HttpGet(url);
                return response;
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='error'>ConverterError: " + ex.Message + "</Value>\n\t</Response>";
                //return false;
            }
        }

        public async Task<string> SiteCreate(string name, string languageCode)
        {
            var contentToServer = JObject.FromObject(new { name });
            var url =
                $"{newAddressBasic}/Site?token={token}&name={Uri.EscapeDataString(name)}&languageCode={languageCode}&sdkVersion={dllVersion}";
            var newUrl = await HttpPost(url, Encoding.UTF8.GetBytes(contentToServer.ToString()), "application/json; charset=utf-8", true); // todo maybe not need content type
            var url2 = $"{newAddressBasic}{newUrl}?token={token}";
            var response = await HttpGet(url2, null, true);

            return response;
        }

        public async Task<bool> SiteUpdate(int id, string name, string languageCode)
        {
            var contentToServer = JObject.FromObject(new { name });
            var url =
                $"{newAddressBasic}/Site/{id}?token={token}&name={Uri.EscapeDataString(name)}&languageCode={languageCode}&sdkVersion={dllVersion}";
            var newUrl = await HttpPut(url, Encoding.UTF8.GetBytes(contentToServer.ToString()), "application/json; charset=utf-8", true); // todo maybe not need content type
            return !string.IsNullOrEmpty(newUrl);
        }

        public async Task<string> SiteDelete(int id)
        {
            try
            {
                var url =
                    $"{newAddressBasic}/Site/{id}?token={token}&sdkVersion={dllVersion}";
                var newUrl = await HttpDelete(url, "application/x-www-form-urlencoded", true);

                return await HttpGet($"{newAddressBasic}{newUrl}?token={token}", null, true);
            }
            catch (Exception ex)
            {
                throw new Exception("SiteDelete failed", ex);
            }
        }

        public async Task<string> SiteLoadAllFromRemote()
        {
            var url =
                $"{newAddressBasic}/Site?token={token}&sdkVersion={dllVersion}";
            return await HttpGet(url, null, true);
        }

        public async Task<string> WorkerCreate(string firstName, string lastName, string email)
        {
            var contentToServer = JObject.FromObject(new { first_name = firstName, last_name = lastName, email });
            var url =
                $"{newAddressBasic}/User?token={token}&firstName={Uri.EscapeDataString(firstName)}&lastName={Uri.EscapeDataString(lastName)}&email={email}&sdkVersion={dllVersion}";
            var content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            var newUrl = await HttpPost(url, content, "application/json; charset=utf-8", true); // todo maybe not need content type
            var url2 = $"{newAddressBasic}{newUrl}?token={token}";
            var response = await HttpGet(url2, null, true);

            return response;
        }

        public async Task<bool> WorkerUpdate(int id, string firstName, string lastName, string email)
        {
            var contentToServer = JObject.FromObject(new { first_name = firstName, last_name = lastName, email });
            var url =
                $"{newAddressBasic}/User/{id}?token={token}&firstName={Uri.EscapeDataString(firstName)}&lastName={Uri.EscapeDataString(lastName)}&email={email}&sdkVersion={dllVersion}";
            var content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            var newUrl = await HttpPut(url, content, "application/json; charset=utf-8", true); // todo maybe not need content type
            return !string.IsNullOrEmpty(newUrl);
        }

        public async Task<string> WorkerDelete(int id)
        {
            try
            {
                var url =
                    $"{newAddressBasic}/User/{id}?token={token}&sdkVersion={dllVersion}";
                var newUrl = await HttpDelete(url, "application/x-www-form-urlencoded", true);
                var url2 = $"{newAddressBasic}{newUrl}?token={token}";
                return await HttpGet(url2, null, true);
            }
            catch (Exception ex)
            {
                throw new Exception("WorkerDelete failed", ex);
            }
        }

        public async Task<string> WorkerLoadAllFromRemote()
        {
            var url = $"{newAddressBasic}/User?token={token}&sdkVersion={dllVersion}";
            return await HttpGet(url, null, true);
        }

        public async Task<string> SiteWorkerCreate(int siteId, int workerId)
        {
            var contentToServer = JObject.FromObject(new { user_id = workerId, site_id = siteId });
            var content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            var url =
                $"{newAddressBasic}/Worker?token={token}&siteid={siteId}&userId={workerId}&sdkVersion={dllVersion}";
            var newUrl = await HttpPost(url, content, "application/json; charset=utf-8", true); // todo maybe not need content type
            var url2 = $"{newAddressBasic}{newUrl}?token={token}";
            var response = await HttpGet(url2, null, true);
            return response;
        }

        public async Task<string> SiteWorkerDelete(int id)
        {
            try
            {
                var url =
                    $"{newAddressBasic}/Worker/{id}?token={token}&sdkVersion={dllVersion}";
                var newUrl = await HttpDelete(url, "application/x-www-form-urlencoded", true);
                var url2 = $"{newAddressBasic}{newUrl}?token={token}";
                return await HttpGet(url2, null, true);
            }
            catch (Exception ex)
            {
                throw new Exception("SiteWorkerDelete failed", ex);
            }
        }

        public async Task<string> SiteWorkerLoadAllFromRemote()
        {
            var url = $"{newAddressBasic}/Worker?token={token}&sdkVersion={dllVersion}";
            return await HttpGet(url, null, true);
        }

        public async Task<string> FolderLoadAllFromRemote()
        {
            var url = $"{newAddressBasic}/Folder?token={token}&sdkVersion={dllVersion}";
            return await HttpGet(url, null, true);
        }

        public async Task<string> FolderCreate(int uuid, int? parentId)
        {
            var url =
                $"{newAddressBasic}/Folder?token={token}&uuid={uuid}&parentId={parentId}&sdkVersion={dllVersion}";
            var newUrl = await HttpPost(url, Array.Empty<byte>(), null, true);
            var url2 = $"{newAddressBasic}{newUrl}?token={token}";
            var response = await HttpGet(url2, null, true);
            return response;
        }

        public async Task<bool> FolderUpdate(int id, string name, string description, string languageCode, int? parentId)
        {
            var url =
                $"{newAddressBasic}/Folder/{id}?token={token}&languageCode={languageCode}&name={Uri.EscapeDataString(name)}&description={Uri.EscapeDataString(description)}&parentId={parentId}&sdkVersion={dllVersion}";
            //JObject contentToServer = JObject.FromObject(new { languageCode, name = Uri.EscapeDataString(name), description = Uri.EscapeDataString(description), parent_id = parentId });
            await HttpPut(url, Array.Empty<byte>(), null, true);
            return true;
        }

        public async Task<string> FolderDelete(int id)
        {
            try
            {
                var url =
                    $"{newAddressBasic}/Folder/{id}?token={token}&sdkVersion={dllVersion}";
                var newUrl = await HttpDelete(url, "application/x-www-form-urlencoded", true);
                var url2 =
                    $"{newAddressBasic}{newUrl}?token={token}";
                return await HttpGet(url2, null, true);
            }
            catch (Exception ex)
            {
                throw new Exception("FolderDelete failed", ex);
            }
        }

        public async Task<string> UnitUpdate(int id, bool newOtp, int siteId, bool pushEnabled, bool syncDelayEnabled, bool syncDialogEnabled)
        {
            var contentToServer = JObject.FromObject(new { model = new { unit_id = id } });
            var url =
                $"{newAddressBasic}/Unit/{id}?token={token}&newOtp=true&pushEnabled={pushEnabled}&siteId={siteId}&syncDelayEnabled={syncDelayEnabled}&syncDialogEnabled={syncDialogEnabled}&sdkVersion={dllVersion}";
            var content = Encoding.UTF8.GetBytes(contentToServer.ToString());
            var newUrl = await HttpPut(url, content, "application/json; charset=utf-8", true); // todo maybe not need content type
            var url2 = $"{newAddressBasic}{newUrl}?token={token}&sdkVersion={dllVersion}";
            return await HttpGet(url2, null, true);
        }

        public async Task<string> UnitLoadAllFromRemote()
        {
            var url = $"{newAddressBasic}/Unit?token={token}&sdkVersion={dllVersion}";
            return await HttpGet(url, null, true);
        }

        public async Task<string> UnitDelete(int id)
        {
            try
            {
                var url =
                    $"{newAddressBasic}/Unit/{id}?token={token}&sdkVersion={dllVersion}";
                var newUrl = await HttpDelete(url, "application/x-www-form-urlencoded", true);
                var url2 = $"{newAddressBasic}{newUrl}?token={token}&sdkVersion={dllVersion}";
                return await HttpGet(url2, null, true);

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
                var url =
                    $"{newAddressBasic}/Unit/{unitId}?token={token}&sdkVersion={dllVersion}&siteId={siteId}";
                var newUrl = await HttpPut(url, Array.Empty<byte>(), "application/x-www-form-urlencoded", true);
                var url2 = $"{newAddressBasic}{newUrl}?token={token}&sdkVersion={dllVersion}";
                return await HttpGet(url2, null, true);
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
                //var contentToMicroting = JObject.FromObject(new { site_id = siteMicrotingUid } );
                var url =
                    $"{newAddressBasic}/Unit/?token={token}&siteId={siteMicrotingUid}&sdkVersion={dllVersion}";
                var newUrl = await HttpPost(url, Array.Empty<byte>(), "application/x-www-form-urlencoded", true);
                var url2 = $"{newAddressBasic}{newUrl}?token={token}&sdkVersion={dllVersion}";
                return await HttpGet(url2, null, true);
            }
            catch (Exception ex)
            {
                throw new Exception("UnitCreate failed", ex);
            }
        }

        public async Task<string> OrganizationLoadAllFromRemote()
        {
            var url = $"{newAddressBasic}/Organization?token={token}&sdkVersion={dllVersion}";
            return await HttpGet(url, null, true);
        }

        public async Task<int> SpeechToText(Stream pathToAudioFile, string language, string extension)
        {
            try
            {
                var url = $"{addressSpeechToText}/audio/?token={token}&sdk_ver={dllVersion}&lang={language}";
                using var tempFiles = new TempFileCollection();
                var file = tempFiles.AddExtension(extension);
                await using var fs = File.OpenWrite(file);
                await pathToAudioFile.CopyToAsync(fs);
                await fs.FlushAsync();
                fs.Close();


                var form = new MultipartFormDataContent();
                var content = new StreamContent(fs);
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = file,
                    FileName = file,
                };
                form.Add(content);
                using var client = new HttpClient();
                var response = await client.PostAsync(url, form);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
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
            var url =
                $"{addressSpeechToText}/audio/{requestId}?token={token}&sdk_ver={dllVersion}";
            var result = await HttpGet(url);
            var parsedData = JToken.Parse(result);
            return parsedData;
        }

        public async Task<bool> SetSurveyConfiguration(int id, int siteId, bool addSite)
        {
            if (addSite)
            {
                var url =
                    $"{addressBasic}/v1/survey_configurations/{id}?token={token}&add_site=true&site_id={siteId}&sdk_ver={dllVersion}";
                await HttpGet(url);
            }
            else
            {
                var url =
                    $"{addressBasic}/v1/survey_configurations/{id}?token={token}&remove_site=true&site_id={siteId}&sdk_ver={dllVersion}";
                await HttpGet(url);
            }

            return true;
        }

        public async Task<string> GetAllSurveyConfigurations()
        {
            var url = $"{addressBasic}/v1/survey_configurations?token={token}&sdk_ver={dllVersion}";
            return await HttpGet(url);
        }

        public async Task<string> GetSurveyConfiguration(int id)
        {
            var url = $"{addressBasic}/v1/survey_configurations/{id}?token={token}&sdk_ver={dllVersion}";
            return await HttpGet(url);
        }

        public async Task<string> GetAllQuestionSets()
        {
            var url = $"{addressBasic}/v1/question_sets?token={token}&sdk_ver={dllVersion}";
            return await HttpGet(url);
        }

        public async Task<string> GetQuestionSet(int id)
        {
            var url = $"{addressBasic}/v1/question_sets/{id}?token={token}&sdk_ver={dllVersion}";
            return await HttpGet(url);
        }

        public async Task<string> GetLastAnswer(int questionSetId, int lastAnswerId)
        {
            var url = $"{addressBasic}/v1/answers/{questionSetId}?token={token}&sdk_ver={dllVersion}&last_answer_id={lastAnswerId}";
            return await HttpGet(url);
        }

        public Task SendPushMessage(int microtingSiteId, string header, string body, int microtingUuid)
        {
            var url = $"{newAddressBasic}/PushMessage?SiteId={microtingSiteId}&token={token}&Header={header}&Body={body}&sdkVersion={dllVersion}&scheduleId={microtingUuid}";
            return HttpPost(url, Array.Empty<byte>(), null, true);
        }

        private static void WriteDebugConsoleLogEntry(string classMethodName, string message)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[DBG] {classMethodName}: {message}");
            Console.ForegroundColor = oldColor;
        }

        private static void WriteErrorConsoleLogEntry(string classMethodName, string message)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERR] {classMethodName}: {message}");
            Console.ForegroundColor = oldColor;
        }
    }
}