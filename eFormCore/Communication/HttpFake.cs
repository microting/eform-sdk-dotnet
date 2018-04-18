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
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">" + "success" + "</Value><Unit fetched_at=\"\" id=\"\"/></Response>";
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
            return "Not implemented!";
        }

        public bool SiteUpdate(int id, string name)
        {
            return true;
        }

        public string SiteDelete(int id)
        {
            return "Not implemented!";
        }

        public string SiteLoadAllFromRemote()
        {
            return "Not implemented!";
        }
        #endregion

        #region public Worker
        public string WorkerCreate(string firstName, string lastName, string email)
        {
            return "Not implemented!";
        }

        public bool WorkerUpdate(int id, string firstName, string lastName, string email)
        {
            return true;
        }

        public string WorkerDelete(int id)
        {
            return "Not implemented!";
        }

        public string WorkerLoadAllFromRemote()
        {
            return "Not implemented!";
        }
        #endregion

        #region public SiteWorker
        public string SiteWorkerCreate(int siteId, int workerId)
        {
            return "Not implemented!";
        }

        public string SiteWorkerDelete(int id)
        {
            return "Not implemented!";
        }

        public string SiteWorkerLoadAllFromRemote()
        {
            return "Not implemented!";
        }
        #endregion

        #region public Unit
        public int UnitRequestOtp(int id)
        {
            return 1;
        }

        public string UnitLoadAllFromRemote()
        {
            return "Not implemented!";
        }
        #endregion

        #region public Organization
        public string OrganizationLoadAllFromRemote()
        {
            return "Not implemented!";
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