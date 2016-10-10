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
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.IO;
using System.Text;

namespace eFormCommunicator
{
    internal class Http
    {

        #region var
        private string protocolXml = "6";
        private string protocolEntityType = "1";
        
        internal string Token { get; set; }
        internal string SrvAdd { get; set; }
        #endregion

        #region con
        internal Http(string token, string serverAddress)
        {
            Token = token;
            SrvAdd = serverAddress;
        }
        #endregion

        #region internal
        /// <summary>
        /// Deletes a element and retrieve the XML encoded response from Microting.
        /// </summary>
        /// <param name="elementId">Identifier of the element to delete.</param>
        internal string Delete(string elementId, string siteId)
        {
            try
            {
                WebRequest request = WebRequest.Create(SrvAdd + "/gwt/inspection_app/integration/" + elementId + "?token=" + Token + "&protocol=" + protocolXml + "&site_id=" + siteId + "&download=false&delete=true");
                request.Method = "GET";

                return PostToServer(request);
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='error'>ConverterError: " + ex.Message + "</Value>\n\t</Response>";
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
                WebRequest request = WebRequest.Create(SrvAdd + "/gwt/inspection_app/integration/" + elementId + "?token=" + Token + "&protocol=" + protocolXml + "&site_id=" + siteId + "&download=false&delete=false");
                request.Method = "GET";

                return PostToServer(request);
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='error'>ConverterError: " + ex.Message + "</Value>\n\t</Response>";
            }
        }

        /// <summary>
        /// Posts the element to Microting and returns the XML encoded restponse.
        /// </summary>
        /// <param name="xmlData">Element converted to a xml encoded string.</param>
        internal string Post(string xmlData, string siteId)
        {
            try
            {
                WebRequest request = WebRequest.Create(SrvAdd + "/gwt/inspection_app/integration/?token=" + Token + "&protocol=" + protocolXml + "&site_id=" + siteId);
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
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="microtingUuid">Identifier of the element to retrieve results from.</param>
        /// <param name="microtingCheckUuid">Identifier of the check to begin from.</param>
        internal string Retrieve(string microtingUuid, int microtingCheckUuid, string siteId)
        {
            try
            {
                WebRequest request = WebRequest.Create(SrvAdd + "/gwt/inspection_app/integration/" + microtingUuid + "?token=" + Token + "&protocol=" + protocolXml + "&site_id=" + siteId + "&download=true&delete=false&last_check_id=" + microtingCheckUuid);
                request.Method = "GET";

                return PostToServer(request);
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" + ex.Message + "</Value>\n\t</Response>";
            }
        }

        internal string CreatEntity(string xmlData, string organizationId)
        {
            try
            {
                WebRequest request = WebRequest.Create(SrvAdd + "/gwt/entity_app/entities?token=" + Token + "&protocol=" + protocolEntityType + "&organization_id=" + organizationId);
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

        internal string CreatEntityType(string xmlData, string organizationId)
        {
            try
            {
                WebRequest request = WebRequest.Create(SrvAdd + "/gwt/entity_app/entity_types?token=" + Token + "&protocol=" + protocolEntityType + "&organization_id=" + organizationId);
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
        #endregion

        #region private
        private string PostToServer(WebRequest request)
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

        private string PostToServer(WebRequest request, byte[] content)
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

