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
    public class HttpFake : IHttp
    {
        #region var
        private string protocolXml = "6";

        private string token = "";
        private string addressApi = "";
        private string addressBasic = "";
        private string addressPdfUpload = "";

        private string dllVersion = "";

        Tools t = new Tools();
        object _lock = new object();
        #endregion

        #region public
        #region public API
        public string Post(string xmlData, string siteId)
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + "M" + t.GetRandomInt(5) + "</Value></Response>";
        }

        public string Status(string elementId, string siteId)
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + "success" + "</Value><Unit fetched_at=\"\" id=\"\"/></Response>";
        }

        public string Retrieve(string microtingUuid, string microtingCheckUuid, int siteId)
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + "success" + "</Value><Unit fetched_at=\"\" id=\"\"/></Response>";
        }

        public string Delete(string elementId, string siteId)
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + "success" + "</Value><Unit fetched_at=\"\" id=\"\"/></Response>";
        }
        #endregion

        #region public EntitySearch
        public string     EntitySearchGroupCreate(string name, string id)
        {
            return t.GetRandomInt(6).ToString();
        }

        public bool       EntitySearchGroupUpdate(int id, string name, string entityGroupMUId)
        {
            return true;
        }

        public bool       EntitySearchGroupDelete(string entityGroupId)
        {
            return true;
        }

        public string     EntitySearchItemCreate(string entitySearchGroupId, string name, string description, string id)
        {
            return t.GetRandomInt(6).ToString();
        }

        public bool       EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name, string description, string id)
        {
            return true;
        }

        public bool       EntitySearchItemDelete(string entitySearchItemId)
        {
            return true;
        }
        #endregion

        #region public EntitySelect
        public string     EntitySelectGroupCreate(string name, string id)
        {
            return t.GetRandomInt(6).ToString();
        }

        public bool       EntitySelectGroupUpdate(int id, string name, string entityGroupMUId)
        {
            return true;
        }

        public bool       EntitySelectGroupDelete(string entityGroupId)
        {
            return true;
        }

        public string     EntitySelectItemCreate(string entitySelectGroupId, string name, int displayOrder, string id)
        {
            return t.GetRandomInt(6).ToString();
        }

        public bool       EntitySelectItemUpdate(string entitySelectGroupId, string entitySelectItemId, string name, int displayOrder, string id)
        {
            return true;
        }

        public bool       EntitySelectItemDelete(string entitySelectItemId)
        {
            return true;
        }
        #endregion

        #region public PdfUpload
        public bool PdfUpload(string name, string hash)
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

        #region public TemplateDisplayIndexChange
        public string TemplateDisplayIndexChange(string microtingUId, int siteId, int newDisplayIndex)
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

        #region public site
        public string     SiteCreate(string name)
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

        public bool       SiteUpdate(int id, string name)
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

        public string     SiteDelete(int id)
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

        public string     SiteLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create(addressBasic + "/v1/sites?token=" + token + "&sdk_ver=" + dllVersion);
            request.Method = "GET";

            return PostToServer(request);
        }
        #endregion

        #region public Worker
        public string     WorkerCreate(string firstName, string lastName, string email)
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

        public bool       WorkerUpdate(int id, string firstName, string lastName, string email)
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

        public string     WorkerDelete(int id)
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

        public string     WorkerLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create(addressBasic + "/v1/users?token=" + token + "&sdk_ver=" + dllVersion);
            request.Method = "GET";

            return PostToServer(request);
        }
        #endregion

        #region public SiteWorker
        public string     SiteWorkerCreate(int siteId, int workerId)
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

        public string       SiteWorkerDelete(int id)
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

        public string     SiteWorkerLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create(addressBasic + "/v1/workers?token=" + token + "&sdk_ver=" + dllVersion);
            request.Method = "GET";

            return PostToServer(request);
        }
        #endregion

        #region public Unit
        public int        UnitRequestOtp(int id)
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

        public string     UnitLoadAllFromRemote()
        {
            WebRequest request = WebRequest.Create(addressBasic + "/v1/units?token=" + token + "&sdk_ver=" + dllVersion);
            request.Method = "GET";

            return PostToServer(request);
        }
        #endregion

        #region public Organization
        public string OrganizationLoadAllFromRemote()
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