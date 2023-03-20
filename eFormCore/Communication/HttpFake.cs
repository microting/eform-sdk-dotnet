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
        // var
//        private string protocolXml = "6";

        private readonly string _token = "";
//        private string addressApi = "";
//        private string addressBasic = "";
//        private string addressPdfUpload = "";

//        private string dllVersion = "";
//        private TestHelpers _testHelpers = new TestHelpers(ConnectionString);
        private readonly TestHelperReturnXML _testHelperReturnXml;


        public HttpFake(string connectionString)
        {
            _testHelperReturnXml = new TestHelperReturnXML(connectionString);
        }

        Tools t = new Tools();

        object _lock = new object();

        //

        // public
        // public API
        public async Task<string> Post(string xmlData, string siteId,
            string contentType = "application/x-www-form-urlencoded")
        {
            await Task.Run(() => { });
            if (xmlData.Contains("throw new Exception"))
                throw new Exception("Post created 'new' Exception as per request");

            if (xmlData.Contains("throw other Exception"))
                throw new Exception("Post created 'other' Exception as per request");

            if (contentType == "application/xml")
                return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" +
                       t.GetRandomInt(5) + "</Value></Response>";

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
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + "success" +
                   "</Value><Unit fetched_at=\"\" id=\"\"/></Response>";
        }

        public async Task<string> Retrieve(string microtingUuid, string microtingCheckUuid, int siteId)
        {
            if (microtingUuid == "555")
            {
                return await _testHelperReturnXml.CreateMultiPictureXMLResult(false);
            }

            return "failed";
        }

        public async Task<string> Delete(string elementId, string siteId)
        {
            await Task.Run(() => { });
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + "success" +
                   "</Value><Unit fetched_at=\"\" id=\"\"/></Response>";
        }
        //

        // public EntitySearch
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

        public async Task<string> EntitySearchItemCreate(string entitySearchGroupId, string name, string description,
            string id)
        {
            await Task.Run(() => { });
            return t.GetRandomInt(6).ToString();
        }

        public async Task<bool> EntitySearchItemUpdate(string entitySearchGroupId, string entitySearchItemId,
            string name, string description, string id)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<bool> EntitySearchItemDelete(string entitySearchItemId)
        {
            await Task.Run(() => { });
            return true;
        }
        //

        // public EntitySelect
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

        public async Task<string> EntitySelectItemCreate(string entitySelectGroupId, string name, int displayOrder,
            string id)
        {
            await Task.Run(() => { });
            return t.GetRandomInt(6).ToString();
        }

        public async Task<bool> EntitySelectItemUpdate(string entitySelectGroupId, string entitySelectItemId,
            string name, int displayOrder, string id)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<bool> EntitySelectItemDelete(string entitySelectItemId)
        {
            await Task.Run(() => { });
            return true;
        }
        //

        // public PdfUpload
        public async Task<bool> PdfUpload(string name, string hash)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<bool> PdfUpload(Stream stream, string hash, string fileName)
        {
            await Task.Run(() => { });
            return true;
        }
        //

        // public TemplateDisplayIndexChange
        public async Task<string> TemplateDisplayIndexChange(string microtingUId, int siteId, int newDisplayIndex)
        {
            await Task.Run(() => { });
            return "Not implemented!";
        }
        //

        // public site
        public async Task<string> SiteCreate(string name, string languageCode)
        {
            await Task.Run(() => { });
            if (name == "John Noname Doe")
            {
                int MicrotingUid = t.GetRandomInt(6);
                JObject contentToServer = JObject.FromObject(new
                {
                    Name = name, MicrotingUid, CreatedAt = "2018-01-12T01:01:00Z", UpdatedAt = "2018-01-12T01:01:10Z"
                });
                return contentToServer.ToString();
            }
            else
            {
                int MicrotingUid = t.GetRandomInt(6);
                JObject contentToServer = JObject.FromObject(new
                {
                    Name = name, MicrotingUid, CreatedAt = "2018-01-12T01:01:00Z", UpdatedAt = "2018-01-12T01:01:10Z"
                });
                return contentToServer.ToString();
            }

            //
        }

        public async Task<bool> SiteUpdate(int id, string name, string languageCode)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<string> SiteDelete(int id)
        {
            await Task.Run(() => { });
            JObject contentToServer = JObject.FromObject(new
            {
                Name = "Some name", MicrotingUid = id, CreatedAt = "2018-01-12T01:01:00Z",
                UpdatedAt = "2018-01-12T01:01:10Z", WorkflowState = Constants.WorkflowStates.Removed
            });
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
        //

        // public Worker
        public async Task<string> WorkerCreate(string firstName, string lastName, string email)
        {
            await Task.Run(() => { });
            int MicrotingUid = t.GetRandomInt(6);
            JObject contentToServer = JObject.FromObject(new
            {
                firstName, MicrotingUid, lastName, email, CreatedAt = "2018-01-12T01:01:00Z",
                UpdatedAt = "2018-01-12T01:01:10Z"
            });
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
            JObject contentToServer = JObject.FromObject(new
            {
                firstName, MicrotingUid = id, lastName, email, CreatedAt = "2018-01-12T01:01:00Z",
                UpdatedAt = "2018-01-12T01:01:10Z", WorkflowState = Constants.WorkflowStates.Removed
            });
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
        //

        // public SiteWorker
        public async Task<string> SiteWorkerCreate(int siteId, int workerId)
        {
            await Task.Run(() => { });
            int MicrotingUid = t.GetRandomInt(6);
            JObject contentToServer = JObject.FromObject(new
                { MicrotingUid, CreatedAt = "2018-01-12T01:01:00Z", UpdatedAt = "2018-01-12T01:01:10Z" });
            return contentToServer.ToString();
        }

        public async Task<string> SiteWorkerDelete(int id)
        {
            await Task.Run(() => { });
            JObject contentToServer = JObject.FromObject(new
            {
                MicrotingUid = id, CreatedAt = "2018-01-12T01:01:00Z", UpdatedAt = "2018-01-12T01:01:10Z",
                WorkflowState = Constants.WorkflowStates.Removed
            });
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
        //

        // folder


        public async Task<string> SiteWorkerLoadAllFromRemote()
        {
            return await _testHelperReturnXml.CreateSiteUnitWorkersForFullLoaed(false);
        }

        public async Task<string> FolderLoadAllFromRemote()
        {
            await Task.Run(() => { });
            return "{}";
        }

        public async Task<string> FolderCreate(int uuid, int? parentId)
        {
            await Task.Run(() => { });
            int id = t.GetRandomInt(6);
            JObject contentToServer = JObject.FromObject(new
                { MicrotingUid = id, ParentId = parentId });
            return contentToServer.ToString();
        }

        public async Task<bool> FolderUpdate(int id, string name, string description, string languageCode,
            int? parentId)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<string> FolderDelete(int id)
        {
            await Task.Run(() => { });

            JObject contentToServer = JObject.FromObject(new
            {
                name = "Some Name", description = "Some Description", id, created_at = "2018-01-12T01:01:00Z",
                updated_at = "2018-01-12T01:01:10Z", WorkflowState = Constants.WorkflowStates.Removed
            });
            return contentToServer.ToString();
////            if (id == 1)
////            {
////            }
////            else
////            {
////                return "Not implemented!";
//            }
        }
        //

        // public Unit
        public async Task<string> UnitUpdate(int id, bool newOtp, int siteId, bool pushEnabled, bool syncDelayEnabled,
            bool syncDialogEnabled)
        {
            await Task.Run(() => { });
            JObject contentToServer = JObject.FromObject(new
            {
                MicrotingUid = id, PushEnabled = pushEnabled, SyncDelayEnabled = syncDelayEnabled,
                SyncDialog = syncDialogEnabled, WorkflowState = Constants.WorkflowStates.Created, OtpCode = 558877
            });
            return contentToServer.ToString();
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

            int MicrotingUid = t.GetRandomInt(6);
            int otpCode = t.GetRandomInt(6);
            JObject contentToServer = JObject.FromObject(new
                { MicrotingUid, WorkflowState = Constants.WorkflowStates.Created, OtpCode = otpCode });
            return contentToServer.ToString();
        }
        //

        // public Organization
        public async Task<string> OrganizationLoadAllFromRemote()
        {
            await Task.Run(() => { });
//            int id = t.GetRandomInt(6);
            JObject contentToServer = JObject.FromObject(new
            {
                my_organization = new
                {
                    AwsEndpoint = "https://sqs.eu-central-1.amazonaws.com/564456879978/",
                    AwsId = "3T98EGIO4Y9H8W2",
                    AwsKey = "098u34098uergijt3098w",
                    CreatedAt = "2018-01-12T01:01:00Z",
                    CustomerNo = "342345",
                    Id = 64856189,
                    Name = "John Doe corporation Ltd.",
                    PaymentOverdue = false,
                    PaymentStatus = "OK",
                    UnitLicenseNumber = 55,
                    UpdatedAt = "2018-01-12T01:01:10Z",
                    WorkflowState = "new",
                    Token = _token,
                    TokenExpires = "2034-01-12T01:01:10Z",
                    ComAddress = "http://srv05.microting.com",
                    ComAddressBasic = "https://basic.microting.com",
                    ComAddressPdfUpload = "https://srv16.microting.com",
                    ComSpeechToText = "https://srv16.microting.com",
                    S3EndPoint = "",
                    S3Id = "sfsefregwef43r2fsfr",
                    S3Key = "john_doen_corporation_ltd"
                }
            });
            return contentToServer.ToString();
        }
        //

        // SpeechToText
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
        //

        // InSight

        public async Task<bool> SetSurveyConfiguration(int id, int siteId, bool addSite)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<string> GetAllSurveyConfigurations()
        {
            await Task.Run(() => { });
            return "{}";
        }

        public async Task<string> GetSurveyConfiguration(int id)
        {
            await Task.Run(() => { });
            return "{}";
        }

        public async Task<string> GetAllQuestionSets()
        {
            await Task.Run(() => { });
            return "{}";
        }

        public async Task<string> GetQuestionSet(int id)
        {
            await Task.Run(() => { });
            return "{}";
        }

        public async Task<string> GetLastAnswer(int questionSetId, int lastAnswerId)
        {
            await Task.Run(() => { });
            return "{}";
        }

        public Task SendPushMessage(int microtingSiteId, string header, string body, int microtingUuid)
        {
            throw new NotImplementedException();
        }

        //

        //

        // private
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
        private bool Validator(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslpolicyErrors)
        {
            return true;
        }
        //
    }
}