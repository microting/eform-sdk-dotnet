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
using System;
using System.ComponentModel;

namespace eFormCommunicator
{
    public class Communicator
    {
        #region var
        public event EventHandler EventLog;

        private Http http;
        #endregion

        #region con
        /// <summary>
        /// Microting XML eForm API C# DLL.
        /// </summary>
        /// <param name="token">Your company's XML eForm API access token.</param>
        /// <param name="address">Microting's eForm API server address.</param>
        public Communicator(string token, string address)
        {
            #region CheckInput token & serverAddress
            string errorsFound = "";

            if (token.Length != 32)
            {
                errorsFound += "Tokens are always 32 charactors long" + Environment.NewLine;
            }

            if (!address.Contains("http://") && !address.Contains("https://"))
            {
                errorsFound += "Server Address is missing 'http://' or 'https://'" + Environment.NewLine;
            }

            if (errorsFound != "")
                throw new InvalidOperationException(errorsFound.TrimEnd());
            #endregion

            http = new Http(token, address);
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
            //TODO - ALL xml hacks

            //XML HACK
            //REMEMBER OTHER POST METHOD

            xmlString = xmlString.Replace("<color></color>", "");
            //Missing serverside. Will not accept blank/empty field
            xmlString = xmlString.Replace("<Color />", "");
            //Missing serverside. Will not accept blank/empty field
            xmlString = xmlString.Replace("DefaultValue", "Value");
            //Missing serverside.

            //REMEMBER OTHER POST METHOD
            //XML HACK


            if (xmlString.Contains("type=\"Entity_Select\">") || xmlString.Contains("type=\"Entity_Select\">"))
                throw new SystemException("XML contains Entity_Search and/or Entity_Select, use PostExtendedXml method instead, as they need a Organization Id");

            TriggerEventLog("siteId:" + siteId.ToString() + ", xmlString:");
            TriggerEventLog(xmlString);

            return http.Post(xmlString, siteId.ToString());
        }

        /// <summary>
        /// Posts the XML eForm to Microting and returns the XML encoded restponse (Supports Entity_Search or Entity_Select).
        /// </summary>
        /// <param name="xmlString">XML encoded eForm string.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        /// <param name="organizationId">Identifier of organization, data is to be connected to.</param>
        public string PostXmlExtended   (string xmlString, int siteId, string organizationId)
        {
            //XML HACK
            //REMEMBER OTHER POST METHOD

            xmlString = xmlString.Replace("<color></color>", "");
            //Missing serverside. Will not accept blank/empty field
            xmlString = xmlString.Replace("<Color />", "");         
            //Missing serverside. Will not accept blank/empty field
            xmlString = xmlString.Replace("DefaultValue", "Value");
            //Missing serverside.

            //REMEMBER OTHER POST METHOD
            //XML HACK


            TriggerEventLog("siteId:" + siteId.ToString() + ", xmlString:");
            TriggerEventLog(xmlString);

            return http.Post(xmlString, siteId.ToString());
        }

        /// <summary>
        /// Retrieve the XML encoded status from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve status of.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public string CheckStatus       (string eFormId, int siteId)
        {
            TriggerEventLog("eFormId:" + eFormId + ", siteId:" + siteId.ToString());

            return http.Status(eFormId, siteId.ToString());
        }

        /// <summary>
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve results from.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public string Retrieve          (string eFormId, int siteId)
        {
            TriggerEventLog("eFormId:" + eFormId + ", siteId:" + siteId.ToString());

            return http.Retrieve(eFormId, "0", siteId); //Always gets the first
        }

        /// <summary>
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve results from.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        /// <param name="eFormCheckId">Identifier of the check to begin from.</param>
        public string RetrieveFromId    (string eFormId, int siteId, string eFormCheckId)
        {
            TriggerEventLog("eFormId:" + eFormId + ", siteId:" + siteId.ToString() + ", eFormCheckId:" + eFormCheckId);

            return http.Retrieve(eFormId, eFormCheckId, siteId);
        }

        /// <summary>
        /// Marks an element for deletion and retrieve the XML encoded response from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to mark for deletion.</param>
        /// <param name="siteId">Your device's Microting ID.</param>
        public string Delete            (string eFormId, int siteId)
        {
            TriggerEventLog("eFormId:" + eFormId + ", siteId:" + siteId.ToString());

            return http.Delete(eFormId, siteId.ToString());
        }
        #endregion

        #region public entity
        public string EntityGroupCreate(string entityType)
        {
            return "12123";
        }

        public string EntityItemUpdate(string entityType, string action, string entityItemStr)
        {
            return "3213";
        }
        #endregion

        #region internal
        internal void TriggerEventLog(string message)
        {
            System.EventHandler handler = EventLog;
            if (handler != null)
            {
                handler(message, EventArgs.Empty);
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
