/*
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
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure.Constants;
using Newtonsoft.Json.Linq;

namespace Microting.eForm.Communication
{
    public class HttpFake : IHttp
    {
        #region var
//        private string protocolXml = "6";

        private readonly string _token = "";
//        private string addressApi = "";
//        private string addressBasic = "";
//        private string addressPdfUpload = "";

//        private string dllVersion = "";
//        private TestHelpers _testHelpers = new TestHelpers();
        private readonly TestHelperReturnXML _testHelperReturnXml = new TestHelperReturnXML();


        Tools t = new Tools();

        object _lock = new object();

        #endregion

        #region public
        #region public API
        public async Task<string> Post(string xmlData, string siteId, string contentType = "application/x-www-form-urlencoded")
        {
            await Task.Run(() => { });
            if (xmlData.Contains("throw new Exception"))
                throw new Exception("Post created 'new' Exception as per request");

            if (xmlData.Contains("throw other Exception"))
                throw new Exception("Post created 'other' Exception as per request");

            if (contentType == "application/x-www-form-urlencoded")
                return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + t.GetRandomInt(5) + "</Value></Response>";

            return @"{
                        Value: {
                            Type: ""success"",
                            Value: """ + t.GetRandomInt(5) + @"""
                        }

                    }";
        }

        public async Task<string> Status(string elementId, string siteId)
        {
            await Task.Run(() => { });
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + "success" + "</Value><Unit fetched_at=\"\" id=\"\"/></Response>";
        }

        public async Task<string> Retrieve(string microtingUuid, string microtingCheckUuid, int siteId)
        {
            if (microtingUuid == "555")
            {
                return await _testHelperReturnXml.CreateMultiPictureXMLResult(false);
            } else
            {
                return "failed";
            }
        }

        public async Task<string> Delete(string elementId, string siteId)
        {
            await Task.Run(() => { });
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + "success" + "</Value><Unit fetched_at=\"\" id=\"\"/></Response>";
        }
        #endregion

        #region public EntitySearch
        public async Task<string> EntitySearchGroupCreate(string name, string id)
        {
            await Task.Run(() => { });
            return t.GetRandomInt(6).ToString();
        }

        public async Task<bool> EntitySearchGroupUpdate(int id, string name, string entityGroupMUId)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<bool> EntitySearchGroupDelete(string entityGroupId)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<string> EntitySearchItemCreate(string entitySearchGroupId, string name, string description, string id)
        {
            await Task.Run(() => { });
            return t.GetRandomInt(6).ToString();
        }

        public async Task<bool> EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name, string description, string id)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<bool> EntitySearchItemDelete(string entitySearchItemId)
        {
            await Task.Run(() => { });
            return true;
        }
        #endregion

        #region public EntitySelect
        public async Task<string> EntitySelectGroupCreate(string name, string id)
        {
            await Task.Run(() => { });
            return t.GetRandomInt(6).ToString();
        }

        public async Task<bool> EntitySelectGroupUpdate(int id, string name, string entityGroupMUId)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<bool> EntitySelectGroupDelete(string entityGroupId)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<string> EntitySelectItemCreate(string entitySelectGroupId, string name, int displayOrder, string id)
        {
            await Task.Run(() => { });
            return t.GetRandomInt(6).ToString();
        }

        public async Task<bool> EntitySelectItemUpdate(string entitySelectGroupId, string entitySelectItemId, string name, int displayOrder, string id)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<bool> EntitySelectItemDelete(string entitySelectItemId)
        {
            await Task.Run(() => { });
            return true;
        }
        #endregion

        #region public PdfUpload
        public async Task<bool> PdfUpload(string name, string hash)
        {
            await Task.Run(() => { });
            return true;
        }
        #endregion

        #region public TemplateDisplayIndexChange
        public async Task<string> TemplateDisplayIndexChange(string microtingUId, int siteId, int newDisplayIndex)
        {
            await Task.Run(() => { });
            return "Not implemented!";
        }
        #endregion

        #region public site
        public async Task<string> SiteCreate(string name)
        {
            await Task.Run(() => { });
            if (name == "John Noname Doe")
            {
                int id = t.GetRandomInt(6);
                JObject contentToServer = JObject.FromObject(new { name = name, id = id, unit_id = 2345, otp_code = 259784, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z" });
                return contentToServer.ToString();
            } else
            {
                int id = t.GetRandomInt(6);
                int unit_id = t.GetRandomInt(6);
                int otp_code = t.GetRandomInt(6);
                JObject contentToServer = JObject.FromObject(new { name = name, id = id, unit_id = unit_id, otp_code = otp_code, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z" });
                return contentToServer.ToString();
            }

            //
        }

        public async Task<bool> SiteUpdate(int id, string name)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<string> SiteDelete(int id)
        {
            await Task.Run(() => { });
            JObject contentToServer = JObject.FromObject(new { name = "Some name", id = id, unit_id = 2345, otp_code = 259784, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z", workflow_state = Constants.WorkflowStates.Removed });
            return contentToServer.ToString();

//            if (id == 1)
//            {
//                string name = "John Noname Doe";
//
//            }
//            else
//            {
//                return "Not implemented!";
//            }
        }

        public async Task<string> SiteLoadAllFromRemote()
        {
            await Task.Run(() => { });
            return "{}";
        }
        #endregion

        #region public Worker
        public async Task<string> WorkerCreate(string firstName, string lastName, string email)
        {
            await Task.Run(() => { });
            int id = t.GetRandomInt(6);
            JObject contentToServer = JObject.FromObject(new { firstName = firstName, id = id, lastName = lastName, email = email, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z" });
            return contentToServer.ToString();
//            if (firstName == "John Noname")
//            {
//            }
//            else
//            {
//                return "Not implemented!";
//            }
        }

        public async Task<bool> WorkerUpdate(int id, string firstName, string lastName, string email)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<string> WorkerDelete(int id)
        {
            await Task.Run(() => { });
            string firstName = "John Noname";
            string lastName = "Doe";
            string email = "jhd@invalid.invalid";
            JObject contentToServer = JObject.FromObject(new { firstName = firstName, id = id, lastName = lastName, email = email, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z", workflow_state = Constants.WorkflowStates.Removed });
            return contentToServer.ToString();
//            if (id == 1)
//            {
//            }
//            else
//            {
//                return "Not implemented!";
//            }
        }

        public async Task<string> WorkerLoadAllFromRemote()
        {
            await Task.Run(() => { });
            return "{}";
        }
        #endregion

        #region public SiteWorker
        public async Task<string> SiteWorkerCreate(int siteId, int workerId)
        {
            await Task.Run(() => { });
            int id = t.GetRandomInt(6);
            JObject contentToServer = JObject.FromObject(new { id = id, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z" });
            return contentToServer.ToString();
        }

        public async Task<string> SiteWorkerDelete(int id)
        {
            await Task.Run(() => { });
            JObject contentToServer = JObject.FromObject(new { id = id, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z", workflow_state = Constants.WorkflowStates.Removed });
            return contentToServer.ToString();
//            if (id == 1)
//            {
//
//            }
//            else
//            {
//                return "Not implemented!";
//            }
        }
        #endregion

        #region folder


        public async Task<string> SiteWorkerLoadAllFromRemote()
        {
            return await _testHelperReturnXml.CreateSiteUnitWorkersForFullLoaed(false);
        }

        public async Task<string> FolderLoadAllFromRemote()
        {
            await Task.Run(() => { });
            return "{}";
        }

        public async Task<string> FolderCreate(string name, string description, int? parentId)
        {
            await Task.Run(() => { });
            int id = t.GetRandomInt(6);
            JObject contentToServer = JObject.FromObject(new
                {id, name, description, parent_id = parentId});
            return contentToServer.ToString();
        }

        public async Task<bool> FolderUpdate(int id, string name, string description, int? parentId)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<string> FolderDelete(int id)
        {
            await Task.Run(() => { });

            JObject contentToServer = JObject.FromObject(new {name = "Some Name", description = "Some Description", id = id, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z", workflow_state = Constants.WorkflowStates.Removed });
            return contentToServer.ToString();
////            if (id == 1)
////            {
////            }
////            else
////            {
////                return "Not implemented!";
//            }
        }
        #endregion

        #region public Unit
        public async Task<int> UnitRequestOtp(int id)
        {
            await Task.Run(() => { });
            return 558877;
        }

        public async Task<string> UnitLoadAllFromRemote()
        {
            await Task.Run(() => { });
            return "{}";
        }

        public async Task<string> UnitDelete(int id)
        {
            await Task.Run(() => { });
            JObject contentToServer = JObject.FromObject(new { workflow_state = Constants.WorkflowStates.Removed });
            return contentToServer.ToString();
        }

        public async Task<string> UnitMove(int unitId, int siteId)
        {
            await Task.Run(() => { });
            JObject contentToServer = JObject.FromObject(new { workflow_state = Constants.WorkflowStates.Created });
            return contentToServer.ToString();
        }

        public async Task<string> UnitCreate(int siteId)
        {
            await Task.Run(() => { });
            JObject contentToServer = JObject.FromObject(new { workflow_state = Constants.WorkflowStates.Created });
            return contentToServer.ToString();
        }
        #endregion

        #region public Organization
        public async Task<string> OrganizationLoadAllFromRemote()
        {
            await Task.Run(() => { });
//            int id = t.GetRandomInt(6);
            JObject contentToServer = JObject.FromObject(new { my_organization = new
            { aws_endpoint = "https://sqs.eu-central-1.amazonaws.com/564456879978/",
                aws_id = "3T98EGIO4Y9H8W2",
                aws_key = "098u34098uergijt3098w",
                created_at = "2018-01-12T01:01:00Z",
                customer_no = "342345",
                cvr_no = 234234,
                ean_no = 235234,
                id = 64856189,
                name = "John Doe corporation Ltd.",
                payment_overdue = false,
                payment_status = "OK",
                unit_license_number = 55,
                updated_at = "2018-01-12T01:01:10Z",
                workflow_state = "new",
                token = _token,
                token_expires = "2034-01-12T01:01:10Z",
                com_address = "http://srv05.microting.com",
                com_address_basic = "https://basic.microting.com",
                com_address_pdf_upload = "https://srv16.microting.com",
                com_speech_to_text = "https://srv16.microting.com",
                subscriber_address = "notification.microting.com",
                subscriber_token = _token,
                subscriber_name = "john_doen_corporation_ltd" } });
            return contentToServer.ToString();
        }
        #endregion

        #region SpeechToText
        public async Task<int> SpeechToText(Stream pathToAudioFile, string language, string extension)
        {
            await Task.Run(() => { });
            throw new NotImplementedException();
        }

        public async Task<JToken> SpeechToText(int requestId)
        {
            await Task.Run(() => { });
            throw new NotImplementedException();
        }
        #endregion

        #region InSight

        public async Task<bool> SetSurveyConfiguration(int id, int siteId, bool addSite)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<string> GetAllSurveyConfigurations()
        {
            await Task.Run(() => {
            });
            return "{}";        }

        public async Task<string> GetSurveyConfiguration(int id)
        {
            await Task.Run(() => {
            });
            return "{}";        }

        public async Task<string> GetAllQuestionSets()
        {
            await Task.Run(() => {
            });
            return "{}";
        }

        public async Task<string> GetQuestionSet(int id)
        {
            await Task.Run(() => {
            });
            return "{}";
        }

        public async Task<string> GetLastAnswer(int questionSetId, int lastAnswerId)
        {
            await Task.Run(() => {
            });
            return "{}";
        }

        #endregion

        #endregion

        #region private
        private string PostToServer(WebRequest request, byte[] content)
        {
            return "Not implemented!";
        }

        private string PostToServerGetRedirect(WebRequest request, byte[] content)
        {
            return "Not implemented!";
        }

        private string PostToServerGetRedirect(WebRequest request)
        {
            return "Not implemented!";
        }

        private string PostToServerNoRedirect(WebRequest request, byte[] content)
        {
            return "Not implemented!";
        }

        private string PostToServer(WebRequest request)
        {
            return "Not implemented!";
        }

        private string PostToServerNoRedirect(WebRequest request)
        {
            return "Not implemented!";
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