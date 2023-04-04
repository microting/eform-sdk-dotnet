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

        private readonly string _token;
        private readonly string _addressApi;
        private readonly string _addressBasic;
        private readonly string _newAddressBasic;
        private readonly string _addressPdfUpload;
        private readonly string _addressSpeechToText;
        private readonly string _organizationId;
        private readonly string _dllVersion;

        private readonly Tools t = new Tools();

        //private object _lock = new object(); // todo maybe need delete
        public Http(string token, string comAddressBasic, string comAddressApi, string comOrganizationId,
            string comAddressPdfUpload, string comSpeechToText)
        {
            this._token = token;
            _addressBasic = comAddressBasic;
            _addressApi = comAddressApi;
            _addressPdfUpload = comAddressPdfUpload;
            _organizationId = comOrganizationId;
            _addressSpeechToText = comSpeechToText;
            _newAddressBasic = "https://microcore.microting.com";

            _dllVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        }

        // public API

        private async Task<string> HttpPost(string url, StringContent content, string contentType = null,
            bool addToken = false, bool followRedirect = false)
        {
            try
            {
                var start = DateTime.UtcNow;
                WriteDebugConsoleLogEntry("HttpPost",
                    $"called at {start} for url {url}");

                using var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = followRedirect });
                if (contentType != null)
                {
                    httpClient.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue(contentType));
                }

                if (addToken)
                {
                    httpClient.DefaultRequestHeaders
                        .Add("Authorization", _token);
                }

                var response = await httpClient.PostAsync(url, content).ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.Found)
                {
                    return response.Headers.Location.ToString();
                }

                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                WriteDebugConsoleLogEntry("HttpPost",
                    $"Finished at {DateTime.UtcNow} - took {start - DateTime.UtcNow}");
                return responseBody;
            }
            catch (Exception ex)
            {
                WriteErrorConsoleLogEntry("HttpPost", ex.Message);
                throw;
            }
        }

        private async Task<string> HttpPut(string url, StringContent content, string contentType = null,
            bool addToken = false, bool followRedirect = false)
        {
            try
            {
                var start = DateTime.UtcNow;
                WriteDebugConsoleLogEntry("HttpPut",
                    $"called at {start} for url {url}");

                using var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = followRedirect });
                if (contentType != null)
                {
                    httpClient.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue(contentType));
                }

                if (addToken)
                {
                    httpClient.DefaultRequestHeaders
                        .Add("Authorization", _token);
                }

                var response = await httpClient.PutAsync(url, content).ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.Found)
                {
                    return response.Headers.Location.ToString();
                }

                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                WriteDebugConsoleLogEntry("HttpPut", $"Finished at {DateTime.UtcNow} - took {start - DateTime.UtcNow}");
                return responseBody;
            }
            catch (Exception ex)
            {
                WriteErrorConsoleLogEntry("HttpPut", ex.Message);
                throw;
            }
        }

        private async Task<string> HttpGet(string url, string contentType = null, bool addToken = false)
        {
            try
            {
                var start = DateTime.UtcNow;
                WriteDebugConsoleLogEntry("HttpGet",
                    $"called at {start} for url {url}");

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
                        .Add("Authorization", _token);
                }

                var response = await httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                WriteDebugConsoleLogEntry("HttpGet", $"Finished at {DateTime.UtcNow} - took {start - DateTime.UtcNow}");
                return responseBody;
            }
            catch (Exception ex)
            {
                WriteErrorConsoleLogEntry("HttpGet", ex.Message);
                throw;
            }
        }

        private async Task<string> HttpDelete(string url, string contentType = null, bool addToken = false,
            bool followRedirect = false)
        {
            try
            {
                var start = DateTime.UtcNow;
                WriteDebugConsoleLogEntry("HttpDelete",
                    $"called at {start} for url {url}");

                using var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = followRedirect });
                if (contentType != null)
                {
                    httpClient.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue(contentType));
                }

                if (addToken)
                {
                    httpClient.DefaultRequestHeaders
                        .Add("Authorization", _token);
                }

                var response = await httpClient.DeleteAsync(url).ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.Found)
                {
                    return response.Headers.Location.ToString();
                }

                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                WriteDebugConsoleLogEntry("HttpDelete",
                    $"Finished at {DateTime.UtcNow} - took {start - DateTime.UtcNow}");
                return responseBody;
            }
            catch (Exception ex)
            {
                WriteErrorConsoleLogEntry("HttpDelete", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Posts the element to Microting and returns the XML encoded restponse.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="siteId"></param>
        /// <param name="contentType"></param>
        public async Task<string> Post(string data, string siteId, string contentType = "application/xml")
        {
            try
            {
                WriteDebugConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}",
                    $"called at {DateTime.UtcNow}");
                var url =
                    $"{_addressApi}/gwt/inspection_app/integration/?token={_token}&protocol={ProtocolXml}&site_id={siteId}&sdk_ver={_dllVersion}";
                var content = new StringContent(data, Encoding.UTF8, contentType);

                return await HttpPost(url, content, contentType).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (contentType == "application/xml")
                {
                    return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" +
                           ex.Message + "</Value>\n\t</Response>";
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
                    $"{_addressApi}/gwt/inspection_app/integration/{elementId}?token={_token}&protocol={ProtocolXml}&site_id={siteId}&download=false&delete=false&sdk_ver={_dllVersion}";
                return await HttpGet(url).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return
                    "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='error'>ConverterError: " +
                    ex.Message + "</Value>\n\t</Response>";
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
                    $"{_addressApi}/gwt/inspection_app/integration/{microtingUuid}?token={_token}&protocol={ProtocolXml}&site_id={siteId}&download=true&delete=false&last_check_id={microtingCheckUuid}&sdk_ver={_dllVersion}";
                return await HttpGet(url).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" +
                       ex.Message + "</Value>\n\t</Response>";
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
                    $"{_addressApi}/gwt/inspection_app/integration/{elementId}?token={_token}&protocol={ProtocolXml}&site_id={siteId}&download=false&delete=true&sdk_ver={_dllVersion}";
                var result = await HttpGet(url).ConfigureAwait(false);

                if (result.Contains("No database connection information was found"))
                {
                    Thread.Sleep(5000);
                    result = await HttpGet(url).ConfigureAwait(false);
                }

                return result;
            }
            catch (Exception ex)
            {
                return
                    "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='error'>ConverterError: " +
                    ex.Message + "</Value>\n\t</Response>";
            }
        }

        // public EntitySearch
        public async Task<string> EntitySearchGroupCreate(string name, string id)
        {
            try
            {
                var xmlData = "<EntityTypes><EntityType><Name><![CDATA[" + name + "]]></Name><Id>" + id +
                              "</Id></EntityType></EntityTypes>";
                const string contentType = "application/xml";
                var content = new StringContent(xmlData, Encoding.UTF8, "application/xml");
                var url =
                    $"{_addressApi}/gwt/entity_app/entity_types?token={_token}&protocol={ProtocolEntitySearch}&organization_id={_organizationId}&sdk_ver={_dllVersion}";
                var responseBody = await HttpPost(url, content, contentType).ConfigureAwait(false);

                if (responseBody.Contains("workflowState=\"created"))
                {
                    return t.Locate(responseBody, "<MicrotingUUId>", "</");
                }

                return null;
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" +
                       ex.Message + "</Value>\n\t</Response>";
            }
        }

        public async Task<bool> EntitySearchGroupUpdate(int id, string name, string entityGroupMuId)
        {
            var xmlData = "<EntityTypes><EntityType><Name><![CDATA[" + name + "]]></Name><Id>" + id +
                          "</Id></EntityType></EntityTypes>";
            var url =
                $"{_addressApi}/gwt/entity_app/entity_types/{entityGroupMuId}?token={_token}&protocol={ProtocolEntitySearch}&organization_id={_organizationId}&sdk_ver={_dllVersion}";
            const string contentType = "application/xml";
            var content = new StringContent(xmlData, Encoding.UTF8, "application/xml");
            var responseXml = await HttpPut(url, content, contentType).ConfigureAwait(false);
            return responseXml.Contains("workflowState=\"created");
        }

        public async Task<bool> EntitySearchGroupDelete(string entityGroupId)
        {
            try
            {
                const string contentType = "application/xml";
                var url =
                    $"{_addressApi}/gwt/entity_app/entity_types/{entityGroupId}?token={_token}&protocol={ProtocolEntitySearch}&organization_id={_organizationId}&sdk_ver={_dllVersion}";
                var responseXml = await HttpDelete(url, contentType).ConfigureAwait(false);

                return responseXml.Contains("Value type=\"success");
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchGroupDelete failed", ex);
            }
        }

        public async Task<string> EntitySearchItemCreate(string entitySearchGroupId, string name, string description,
            string id)
        {
            var content = new StringContent("<Entities><Entity>" +
                                            $"<EntityTypeId>{entitySearchGroupId}</EntityTypeId>" +
                                            $"<Identifier><![CDATA[{name}]]></Identifier>" +
                                            $"<Description><![CDATA[{description}]]></Description>" +
                                            "<Km></Km><Colour></Colour><Radiocode></Radiocode>" + // todo Legacy. To be removed server side
                                            $"<Id>{id}</Id>" +
                                            "</Entity></Entities>", Encoding.UTF8, "application/xml");
            var url =
                $"{_addressApi}/gwt/entity_app/entities?token={_token}&protocol={ProtocolEntitySearch}&organization_id={_organizationId}&sdk_ver={_dllVersion}";

            var responseXml = await HttpPost(url, content, "application/xml", false, true).ConfigureAwait(false);

            if (responseXml.Contains("workflowState=\"created"))
            {
                return t.Locate(responseXml, "<MicrotingUUId>", "</");
            }

            return null;
        }

        public async Task<bool> EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId,
            string name, string description, string id)
        {
            var content = new StringContent("<Entities><Entity>" +
                                            $"<EntityTypeId>{entitySearchGroupId}</EntityTypeId>" +
                                            $"<Identifier><![CDATA[{name}]]></Identifier>" +
                                            $"<Description><![CDATA[{description}]]></Description>" +
                                            "<Km></Km><Colour></Colour><Radiocode></Radiocode>" + // todo Legacy. To be removed server side
                                            $"<Id>{id}</Id>" +
                                            "</Entity></Entities>", Encoding.UTF8, "application/xml");
            var url =
                $"{_addressApi}/gwt/entity_app/entities/{entitySearchItemId}?token={_token}&protocol={ProtocolEntitySearch}&organization_id={_organizationId}&sdk_ver={_dllVersion}";

            var newUrl = await HttpPut(url, content, "application/xml").ConfigureAwait(false);
            return !string.IsNullOrEmpty(newUrl);
        }

        public async Task<bool> EntitySearchItemDelete(string entitySearchItemId)
        {
            var url =
                $"{_addressApi}/gwt/entity_app/entities/{entitySearchItemId}?token={_token}&protocol={ProtocolEntitySearch}&organization_id={_organizationId}&sdk_ver={_dllVersion}";
            var responseXml = await HttpDelete(url, "application/xml").ConfigureAwait(false);
            return responseXml.Contains("Value type=\"success");
        }

        // public EntitySelect
        public async Task<string> EntitySelectGroupCreate(string name, string id)
        {
            try
            {
                var content =
                    new StringContent(JObject.FromObject(new { model = new { name, api_uuid = id } }).ToString(),
                        Encoding.UTF8, "application/json");
                var url =
                    $"{_addressApi}/gwt/inspection_app/searchable_item_groups.json?token={_token}&protocol={ProtocolEntitySelect}&organization_id={_organizationId}&sdk_ver={_dllVersion}";
                var responseXml =
                    await HttpPost(url, content, "application/json", false, true)
                        .ConfigureAwait(false); // todo maybe not need content type
                if (responseXml.Contains("workflow_state\": \"created"))
                {
                    return t.Locate(responseXml, "\"id\": \"", "\"");
                }

                return null;
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" +
                       ex.Message + "</Value>\n\t</Response>";
            }
        }

        public async Task<bool> EntitySelectGroupUpdate(int id, string name, string entityGroupMuId)
        {
            var content = new StringContent(JObject.FromObject(new { model = new { name, api_uuid = id } }).ToString(),
                Encoding.UTF8, "application/json");
            var url =
                $"{_addressApi}/gwt/inspection_app/searchable_item_groups/{entityGroupMuId}?token={_token}&protocol={ProtocolEntitySelect}&organization_id={_organizationId}&sdk_ver={_dllVersion}";
            var newUrl =
                await HttpPut(url, content, "application/json", false, true)
                    .ConfigureAwait(false); // todo maybe not need content type
            var response = await HttpGet(newUrl).ConfigureAwait(false);

            return response.Contains("workflow_state\": \"created");
        }

        public async Task<bool> EntitySelectGroupDelete(string entityGroupId)
        {
            try
            {
                var url =
                    $"{_addressApi}/gwt/inspection_app/searchable_item_groups/{entityGroupId}.json?token={_token}&protocol={ProtocolEntitySelect}&organization_id={_organizationId}&sdk_ver={_dllVersion}";
                var newUrl =
                    await HttpDelete(url, "application/json").ConfigureAwait(false); // todo maybe not need content type
                var response = await HttpGet(newUrl).ConfigureAwait(false);
                return response.Contains("workflow_state\": \"removed");
            }
            catch (Exception ex)
            {
                throw new Exception("EntitySearchGroupDelete failed", ex);
            }
        }

        public async Task<string> EntitySelectItemCreate(string entitySelectGroupId, string name, int displayIndex,
            string id)
        {
            var contentToServer = JObject.FromObject(new
            {
                model = new
                {
                    data = name, api_uuid = id, display_order = displayIndex, searchable_group_id = entitySelectGroupId
                }
            });

            var content = new StringContent(contentToServer.ToString(), Encoding.UTF8, "application/json");
            var url =
                $"{_addressApi}/gwt/inspection_app/searchable_items.json?token={_token}&protocol={ProtocolEntitySelect}&organization_id={_organizationId}&sdk_ver={_dllVersion}";
            var response = await HttpPost(url, content, null, false, true).ConfigureAwait(false);

            if (response.Contains("workflow_state\": \"created"))
            {
                return t.Locate(response, "\"id\": \"", "\"");
            }

            return null;
        }

        public async Task<bool> EntitySelectItemUpdate(string entitySelectGroupId, string entitySelectItemId,
            string name, int displayIndex, string ownUuid)
        {
            var contentToServer = JObject.FromObject(new
            {
                model = new
                {
                    data = name, api_uuid = ownUuid, display_order = displayIndex,
                    searchable_group_id = entitySelectGroupId
                }
            });
            var url =
                $"{_addressApi}/gwt/inspection_app/searchable_items/{entitySelectItemId}?token={_token}&protocol={ProtocolEntitySelect}&organization_id={_organizationId}&sdk_ver={_dllVersion}";
            var newUrl = await HttpPut(url,
                    new StringContent(contentToServer.ToString(), Encoding.UTF8, "application/json"),
                    "application/json")
                .ConfigureAwait(false); // todo maybe not need content type
            // var url2 = $"{newUrl}?token={token}";
            var response = await HttpGet(newUrl).ConfigureAwait(false);
            return response.Contains("workflow_state\": \"created");
        }

        public async Task<bool> EntitySelectItemDelete(string entitySelectItemId)
        {
            var url =
                $"{_addressApi}/gwt/inspection_app/searchable_items/{entitySelectItemId}.json?token={_token}&protocol={ProtocolEntitySelect}&organization_id={_organizationId}&sdk_ver={_dllVersion}";
            var newUrl =
                await HttpDelete(url, "application/json").ConfigureAwait(false); // todo maybe not need content type
            var response = await HttpGet(newUrl).ConfigureAwait(false);
            return response.Contains("workflow_state\": \"removed");
        }

        public async Task<bool> PdfUpload(string name, string hash)
        {
            try
            {
                var url =
                    $"{_addressPdfUpload}/data_uploads/upload?token={_token}&hash={hash}&extension=pdf&sdk_ver={_dllVersion}";
                var form = new MultipartFormDataContent();
                var fileStream = new FileStream(name, FileMode.Open);
                form.Add(new StreamContent(fileStream), "file", name.Split('/').Last());
                form.Add(new StringContent(hash), "hash");
                form.Add(new StringContent(name.Split('/').Last()), "orig_file");
                form.Add(new StringContent(_token), "token");
                form.Add(new StringContent("pdf"), "extension");
                form.Add(new StringContent("sdk"), "application_name");
                using var client = new HttpClient();
                client.DefaultRequestHeaders
                    .Add("Authorization", _token);
                await client.PostAsync(url, form).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                WriteErrorConsoleLogEntry($"{GetType()}.{MethodBase.GetCurrentMethod()?.Name}", ex.Message);
                return false;
            }
        }

        public async Task<bool> PdfUpload(Stream stream, string hash, string fileName)
        {
            try
            {
                var url =
                    $"{_addressPdfUpload}/data_uploads/upload?token={_token}&hash={hash}&extension=pdf&sdk_ver={_dllVersion}";
                var form = new MultipartFormDataContent();
                // var fileStream = new FileStream(name, FileMode.Open);
                form.Add(new StreamContent(stream), "file", fileName);
                form.Add(new StringContent(hash), "hash");
                form.Add(new StringContent(fileName), "orig_file");
                form.Add(new StringContent(_token), "token");
                form.Add(new StringContent("pdf"), "extension");
                form.Add(new StringContent("sdk"), "application_name");
                using var client = new HttpClient();
                client.DefaultRequestHeaders
                    .Add("Authorization", _token);
                await client.PostAsync(url, form).ConfigureAwait(false);
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
                    $"{_addressApi}/gwt/inspection_app/integration/{microtingUId}?token={_token}&protocol={ProtocolXml}&site_id={siteId}&download=false&delete=false&update_attributes=true&display_order={newDisplayIndex}&sdk_ver={_dllVersion}";
                var response = await HttpGet(url).ConfigureAwait(false);
                return response;
            }
            catch (Exception ex)
            {
                return
                    "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='error'>ConverterError: " +
                    ex.Message + "</Value>\n\t</Response>";
            }
        }

        public async Task<string> SiteCreate(string name, string languageCode)
        {
            var contentToServer = JObject.FromObject(new { name });
            var url =
                $"{_newAddressBasic}/Site?token={_token}&name={Uri.EscapeDataString(name)}&languageCode={languageCode}&sdkVersion={_dllVersion}";
            var newUrl = await HttpPost(url,
                new StringContent(contentToServer.ToString(), Encoding.UTF8, "application/json"), "application/json",
                true).ConfigureAwait(false); // todo maybe not need content type
            var url2 = $"{_newAddressBasic}{newUrl}?token={_token}";
            var response = await HttpGet(url2, null, true).ConfigureAwait(false);

            return response;
        }

        public async Task<bool> SiteUpdate(int id, string name, string languageCode)
        {
            var contentToServer = JObject.FromObject(new { name });
            var url =
                $"{_newAddressBasic}/Site/{id}?token={_token}&name={Uri.EscapeDataString(name)}&languageCode={languageCode}&sdkVersion={_dllVersion}";
            var newUrl = await HttpPut(url,
                new StringContent(contentToServer.ToString(), Encoding.UTF8, "application/json"), "application/json",
                true).ConfigureAwait(false); // todo maybe not need content type
            return !string.IsNullOrEmpty(newUrl);
        }

        public async Task<string> SiteDelete(int id)
        {
            try
            {
                var url =
                    $"{_newAddressBasic}/Site/{id}?token={_token}&sdkVersion={_dllVersion}";
                var newUrl = await HttpDelete(url, "application/json", true).ConfigureAwait(false);

                return await HttpGet($"{_newAddressBasic}{newUrl}?token={_token}", null, true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("SiteDelete failed", ex);
            }
        }

        public async Task<string> SiteLoadAllFromRemote()
        {
            var url =
                $"{_newAddressBasic}/Site?token={_token}&sdkVersion={_dllVersion}";
            return await HttpGet(url, null, true).ConfigureAwait(false);
        }

        public async Task<string> WorkerCreate(string firstName, string lastName, string email)
        {
            var contentToServer = JObject.FromObject(new { first_name = firstName, last_name = lastName, email });
            var url =
                $"{_newAddressBasic}/User?token={_token}&firstName={Uri.EscapeDataString(firstName)}&lastName={Uri.EscapeDataString(lastName)}&email={email}&sdkVersion={_dllVersion}";
            var content = new StringContent(contentToServer.ToString(), Encoding.UTF8, "application/json");
            var newUrl =
                await HttpPost(url, content, "application/json", true)
                    .ConfigureAwait(false); // todo maybe not need content type
            var url2 = $"{_newAddressBasic}{newUrl}?token={_token}";
            var response = await HttpGet(url2, null, true).ConfigureAwait(false);

            return response;
        }

        public async Task<bool> WorkerUpdate(int id, string firstName, string lastName, string email)
        {
            var contentToServer = JObject.FromObject(new { first_name = firstName, last_name = lastName, email });
            var url =
                $"{_newAddressBasic}/User/{id}?token={_token}&firstName={Uri.EscapeDataString(firstName)}&lastName={Uri.EscapeDataString(lastName)}&email={email}&sdkVersion={_dllVersion}";
            var content = new StringContent(contentToServer.ToString(), Encoding.UTF8, "application/json");
            var newUrl =
                await HttpPut(url, content, "application/json", true)
                    .ConfigureAwait(false); // todo maybe not need content type
            return !string.IsNullOrEmpty(newUrl);
        }

        public async Task<string> WorkerDelete(int id)
        {
            try
            {
                var url =
                    $"{_newAddressBasic}/User/{id}?token={_token}&sdkVersion={_dllVersion}";
                var newUrl = await HttpDelete(url, "application/json", true).ConfigureAwait(false);
                var url2 = $"{_newAddressBasic}{newUrl}?token={_token}";
                return await HttpGet(url2, null, true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("WorkerDelete failed", ex);
            }
        }

        public async Task<string> WorkerLoadAllFromRemote()
        {
            var url = $"{_newAddressBasic}/User?token={_token}&sdkVersion={_dllVersion}";
            return await HttpGet(url, null, true).ConfigureAwait(false);
        }

        public async Task<string> SiteWorkerCreate(int siteId, int workerId)
        {
            var contentToServer = JObject.FromObject(new { user_id = workerId, site_id = siteId });
            var content = new StringContent(contentToServer.ToString(), Encoding.UTF8, "application/json");
            var url =
                $"{_newAddressBasic}/Worker?token={_token}&siteid={siteId}&userId={workerId}&sdkVersion={_dllVersion}";
            var newUrl =
                await HttpPost(url, content, "application/json", true)
                    .ConfigureAwait(false); // todo maybe not need content type
            var url2 = $"{_newAddressBasic}{newUrl}?token={_token}";
            var response = await HttpGet(url2, null, true).ConfigureAwait(false);
            return response;
        }

        public async Task<string> SiteWorkerDelete(int id)
        {
            try
            {
                var url =
                    $"{_newAddressBasic}/Worker/{id}?token={_token}&sdkVersion={_dllVersion}";
                var newUrl = await HttpDelete(url, "application/json", true).ConfigureAwait(false);
                var url2 = $"{_newAddressBasic}{newUrl}?token={_token}";
                return await HttpGet(url2, null, true);
            }
            catch (Exception ex)
            {
                throw new Exception("SiteWorkerDelete failed", ex);
            }
        }

        public async Task<string> SiteWorkerLoadAllFromRemote()
        {
            var url = $"{_newAddressBasic}/Worker?token={_token}&sdkVersion={_dllVersion}";
            return await HttpGet(url, null, true).ConfigureAwait(false);
        }

        public async Task<string> FolderLoadAllFromRemote()
        {
            var url = $"{_newAddressBasic}/Folder?token={_token}&sdkVersion={_dllVersion}";
            return await HttpGet(url, null, true).ConfigureAwait(false);
        }

        public async Task<string> FolderCreate(int uuid, int? parentId)
        {
            var url =
                $"{_newAddressBasic}/Folder?token={_token}&uuid={uuid}&parentId={parentId}&sdkVersion={_dllVersion}";
            var newUrl = await HttpPost(url, new StringContent(""), null, true).ConfigureAwait(false);
            var url2 = $"{_newAddressBasic}{newUrl}?token={_token}";
            var response = await HttpGet(url2, null, true).ConfigureAwait(false);
            return response;
        }

        public async Task<bool> FolderUpdate(int id, string name, string description, string languageCode,
            int? parentId)
        {
            var url =
                $"{_newAddressBasic}/Folder/{id}?token={_token}&languageCode={languageCode}&name={Uri.EscapeDataString(name)}&description={Uri.EscapeDataString(description)}&parentId={parentId}&sdkVersion={_dllVersion}";
            await HttpPut(url, new StringContent(""), null, true).ConfigureAwait(false);
            return true;
        }

        public async Task<string> FolderDelete(int id)
        {
            try
            {
                var url =
                    $"{_newAddressBasic}/Folder/{id}?token={_token}&sdkVersion={_dllVersion}";
                var newUrl = await HttpDelete(url, "application/json", true).ConfigureAwait(false);
                var url2 =
                    $"{_newAddressBasic}{newUrl}?token={_token}";
                return await HttpGet(url2, null, true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("FolderDelete failed", ex);
            }
        }

        public async Task<string> UnitUpdate(int id, bool newOtp, int siteId, bool pushEnabled, bool syncDelayEnabled,
            bool syncDialogEnabled)
        {
            var contentToServer = JObject.FromObject(new { model = new { unit_id = id } });
            var url =
                $"{_newAddressBasic}/Unit/{id}?token={_token}&newOtp=true&pushEnabled={pushEnabled}&siteId={siteId}&syncDelayEnabled={syncDelayEnabled}&syncDialogEnabled={syncDialogEnabled}&sdkVersion={_dllVersion}";
            var content = new StringContent(contentToServer.ToString(), Encoding.UTF8, "application/json");
            var newUrl =
                await HttpPut(url, content, "application/json", true)
                    .ConfigureAwait(false); // todo maybe not need content type
            var url2 = $"{_newAddressBasic}{newUrl}?token={_token}&sdkVersion={_dllVersion}";
            return await HttpGet(url2, null, true).ConfigureAwait(false);
        }

        public async Task<string> UnitLoadAllFromRemote()
        {
            var url = $"{_newAddressBasic}/Unit?token={_token}&sdkVersion={_dllVersion}";
            return await HttpGet(url, null, true).ConfigureAwait(false);
        }

        public async Task<string> UnitDelete(int id)
        {
            try
            {
                var url =
                    $"{_newAddressBasic}/Unit/{id}?token={_token}&sdkVersion={_dllVersion}";
                var newUrl = await HttpDelete(url, "application/json", true).ConfigureAwait(false);
                var url2 = $"{_newAddressBasic}{newUrl}?token={_token}&sdkVersion={_dllVersion}";
                return await HttpGet(url2, null, true).ConfigureAwait(false);
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
                    $"{_newAddressBasic}/Unit/{unitId}?token={_token}&sdkVersion={_dllVersion}&siteId={siteId}";
                var newUrl = await HttpPut(url, new StringContent(""), "application/json", true).ConfigureAwait(false);
                var url2 = $"{_newAddressBasic}{newUrl}?token={_token}&sdkVersion={_dllVersion}";
                return await HttpGet(url2, null, true).ConfigureAwait(false);
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
                var url =
                    $"{_newAddressBasic}/Unit/?token={_token}&siteId={siteMicrotingUid}&sdkVersion={_dllVersion}";
                var newUrl = await HttpPost(url, new StringContent(""), "application/json", true).ConfigureAwait(false);
                var url2 = $"{_newAddressBasic}{newUrl}?token={_token}&sdkVersion={_dllVersion}";
                return await HttpGet(url2, null, true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("UnitCreate failed", ex);
            }
        }

        public async Task<string> OrganizationLoadAllFromRemote()
        {
            var url = $"{_newAddressBasic}/Organization?token={_token}&sdkVersion={_dllVersion}";
            return await HttpGet(url, null, true).ConfigureAwait(false);
        }

        public async Task<int> SpeechToText(Stream pathToAudioFile, string language, string extension)
        {
            try
            {
                var url = $"{_addressSpeechToText}/audio/?token={_token}&sdk_ver={_dllVersion}&lang={language}";
                using var tempFiles = new TempFileCollection();
                var file = tempFiles.AddExtension(extension);
                await using var fs = File.OpenWrite(file);
                await pathToAudioFile.CopyToAsync(fs).ConfigureAwait(false);
                await fs.FlushAsync().ConfigureAwait(false);
                fs.Close();


                var form = new MultipartFormDataContent();
                var content = new StreamContent(fs);
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = file,
                    FileName = file
                };
                form.Add(content);
                using var client = new HttpClient();
                var response = await client.PostAsync(url, form).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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
                $"{_addressSpeechToText}/audio/{requestId}?token={_token}&sdk_ver={_dllVersion}";
            var result = await HttpGet(url).ConfigureAwait(false);
            var parsedData = JToken.Parse(result);
            return parsedData;
        }

        public async Task<bool> SetSurveyConfiguration(int id, int siteId, bool addSite)
        {
            if (addSite)
            {
                var url =
                    $"{_addressBasic}/v1/survey_configurations/{id}?token={_token}&add_site=true&site_id={siteId}&sdk_ver={_dllVersion}";
                await HttpGet(url).ConfigureAwait(false);
            }
            else
            {
                var url =
                    $"{_addressBasic}/v1/survey_configurations/{id}?token={_token}&remove_site=true&site_id={siteId}&sdk_ver={_dllVersion}";
                await HttpGet(url).ConfigureAwait(false);
            }

            return true;
        }

        public async Task<string> GetAllSurveyConfigurations()
        {
            var url = $"{_addressBasic}/v1/survey_configurations?token={_token}&sdk_ver={_dllVersion}";
            return await HttpGet(url).ConfigureAwait(false);
        }

        public async Task<string> GetSurveyConfiguration(int id)
        {
            var url = $"{_addressBasic}/v1/survey_configurations/{id}?token={_token}&sdk_ver={_dllVersion}";
            return await HttpGet(url).ConfigureAwait(false);
        }

        public async Task<string> GetAllQuestionSets()
        {
            var url = $"{_addressBasic}/v1/question_sets?token={_token}&sdk_ver={_dllVersion}";
            return await HttpGet(url).ConfigureAwait(false);
        }

        public async Task<string> GetQuestionSet(int id)
        {
            var url = $"{_addressBasic}/v1/question_sets/{id}?token={_token}&sdk_ver={_dllVersion}";
            return await HttpGet(url).ConfigureAwait(false);
        }

        public async Task<string> GetLastAnswer(int questionSetId, int lastAnswerId)
        {
            var url =
                $"{_addressBasic}/v1/answers/{questionSetId}?token={_token}&sdk_ver={_dllVersion}&last_answer_id={lastAnswerId}";
            return await HttpGet(url).ConfigureAwait(false);
        }

        public Task SendPushMessage(int microtingSiteId, string header, string body, int microtingUuid)
        {
            var url =
                $"{_newAddressBasic}/PushMessage?SiteId={microtingSiteId}&token={_token}&Header={header}&Body={body}&sdkVersion={_dllVersion}&scheduleId={microtingUuid}";
            return HttpPost(url, new StringContent(""), null, true);
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