/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 microting

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
using eFormCore.Helpers;

namespace eFormCommunicator
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
        public string Post(string xmlData, string siteId)
        {
            if (xmlData.Contains("throw new Exception"))
                throw new Exception("Post created 'new' Exception as per request");

            if (xmlData.Contains("throw other Exception"))
                throw new Exception("Post created 'other' Exception as per request");

            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + "M" + t.GetRandomInt(5) + "</Value></Response>";
        }

        public string Status(string elementId, string siteId)
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + "success" + "</Value><Unit fetched_at=\"\" id=\"\"/></Response>";
        }

        public string Retrieve(string microtingUuid, string microtingCheckUuid, int siteId)
        {
            if (microtingUuid == "MultiPictureTestInMultipleChecks")
            {
                return _testHelperReturnXml.CreateMultiPictureXMLResult(false);
            } else
            {
                return "failed";
            }
        }

        public string Delete(string elementId, string siteId)
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + "success" + "</Value><Unit fetched_at=\"\" id=\"\"/></Response>";
        }
        #endregion

        #region public EntitySearch
        public string EntitySearchGroupCreate(string name, string id)
        {
            return t.GetRandomInt(6).ToString();
        }

        public bool EntitySearchGroupUpdate(int id, string name, string entityGroupMUId)
        {
            return true;
        }

        public bool EntitySearchGroupDelete(string entityGroupId)
        {
            return true;
        }

        public string EntitySearchItemCreate(string entitySearchGroupId, string name, string description, string id)
        {
            return t.GetRandomInt(6).ToString();
        }

        public bool EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId, string name, string description, string id)
        {
            return true;
        }

        public bool EntitySearchItemDelete(string entitySearchItemId)
        {
            return true;
        }
        #endregion

        #region public EntitySelect
        public string EntitySelectGroupCreate(string name, string id)
        {
            return t.GetRandomInt(6).ToString();
        }

        public bool EntitySelectGroupUpdate(int id, string name, string entityGroupMUId)
        {
            return true;
        }

        public bool EntitySelectGroupDelete(string entityGroupId)
        {
            return true;
        }

        public string EntitySelectItemCreate(string entitySelectGroupId, string name, int displayOrder, string id)
        {
            return t.GetRandomInt(6).ToString();
        }

        public bool EntitySelectItemUpdate(string entitySelectGroupId, string entitySelectItemId, string name, int displayOrder, string id)
        {
            return true;
        }

        public bool EntitySelectItemDelete(string entitySelectItemId)
        {
            return true;
        }
        #endregion

        #region public PdfUpload
        public bool PdfUpload(string name, string hash)
        {
            return true;
        }
        #endregion

        #region public TemplateDisplayIndexChange
        public string TemplateDisplayIndexChange(string microtingUId, int siteId, int newDisplayIndex)
        {
            return "Not implemented!";
        }
        #endregion

        #region public site
        public string SiteCreate(string name)
        {
            if (name == "John Noname Doe")
            {
                int id = t.GetRandomInt(6);
                JObject content_to_microting = JObject.FromObject(new { name = name, id = id, unit_id = 2345, otp_code = 259784, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z" });
                return content_to_microting.ToString();
            } else
            {
                int id = t.GetRandomInt(6);
                int unit_id = t.GetRandomInt(6);
                int otp_code = t.GetRandomInt(6);
                JObject content_to_microting = JObject.FromObject(new { name = name, id = id, unit_id = unit_id, otp_code = otp_code, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z" });
                return content_to_microting.ToString();
            }
            
            //
        }

        public bool SiteUpdate(int id, string name)
        {
            return true;
        }

        public string SiteDelete(int id)
        {
            JObject content_to_microting = JObject.FromObject(new { name = "Some name", id = id, unit_id = 2345, otp_code = 259784, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z", workflow_state = Constants.WorkflowStates.Removed });
            return content_to_microting.ToString();
            
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

        public string SiteLoadAllFromRemote()
        {
            return "{}";
        }
        #endregion

        #region public Worker
        public string WorkerCreate(string firstName, string lastName, string email)
        {
            int id = t.GetRandomInt(6);
            JObject content_to_microting = JObject.FromObject(new { firstName = firstName, id = id, lastName = lastName, email = email, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z" });
            return content_to_microting.ToString();
//            if (firstName == "John Noname")
//            {
//            }
//            else
//            {
//                return "Not implemented!";
//            }
        }

        public bool WorkerUpdate(int id, string firstName, string lastName, string email)
        {
            return true;
        }

        public string WorkerDelete(int id)
        {
            string firstName = "John Noname";
            string lastName = "Doe";
            string email = "jhd@invalid.invalid";
            JObject content_to_microting = JObject.FromObject(new { firstName = firstName, id = id, lastName = lastName, email = email, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z", workflow_state = Constants.WorkflowStates.Removed });
            return content_to_microting.ToString();
//            if (id == 1)
//            {
//            }
//            else
//            {
//                return "Not implemented!";
//            }
        }

        public string WorkerLoadAllFromRemote()
        {
            return "{}";
        }
        #endregion

        #region public SiteWorker
        public string SiteWorkerCreate(int siteId, int workerId)
        {
            int id = t.GetRandomInt(6);
            JObject content_to_microting = JObject.FromObject(new { id = id, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z" });
            return content_to_microting.ToString();
        }

        public string SiteWorkerDelete(int id)
        {
            JObject content_to_microting = JObject.FromObject(new { id = id, created_at = "2018-01-12T01:01:00Z", updated_at = "2018-01-12T01:01:10Z", workflow_state = Constants.WorkflowStates.Removed });
            return content_to_microting.ToString();
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
        
        
        public string SiteWorkerLoadAllFromRemote()
        {
            return _testHelperReturnXml.CreateSiteUnitWorkersForFullLoaed(false);
        }

        public string FolderLoadAllFromRemote()
        {
            return "";
        }

        public string FolderCreate(string name, string description, int? parent_id)
        {
            return "";
        }

        public void FolderUpdate(int id, string name, string description, int? parent_id)
        {
            
        }

        public void FolderDelete(int id)
        {
            
        }
        #endregion

        #region public Unit
        public int UnitRequestOtp(int id)
        {
            return 558877;
        }

        public string UnitLoadAllFromRemote()
        {
            return "{}";
        }
        #endregion

        #region public Organization
        public string OrganizationLoadAllFromRemote()
        {
//            int id = t.GetRandomInt(6);
            JObject content_to_microting = JObject.FromObject(new { my_organization = new
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
            return content_to_microting.ToString();
        }
        #endregion

        #region SpeechToText
        public int SpeechToText(string pathToAudioFile, string language)
        {
            throw new NotImplementedException();
        }

        public JToken SpeechToText(int requestId)
        {
            throw new NotImplementedException();
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