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
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.IO;
using System.Text;

namespace Inspection
{
    public class Http
    {
        #region Private fields
        private static string Protocol = "5";
        #endregion

        public Http(string token, string srv_name)
        {
            this.Token = token;
            this.SrvName = srv_name;
        }

        #region Public methods
        /// <summary>
        /// Deletes a element and retrieve the XML encoded response from Microting.
        /// </summary>
        /// <param name="elementId">Identifier of the element to delete.</param>
        public string Delete(string elementId, string site_id)
        {
            try
            {
                WebRequest request = WebRequest.Create(SrvName + "/gwt/inspection_app/integration/" + elementId + "?token=" + Token + "&protocol=" + Protocol + "&site_id=" + site_id + "&download=false&delete=true");
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
        public string Status(string elementId, string site_id)
        {
            try
            {
                WebRequest request = WebRequest.Create(SrvName + "/gwt/inspection_app/integration/" + elementId + "?token=" + Token + "&protocol=" + Protocol + "&site_id=" + site_id + "&download=false&delete=false");
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
        public string Post(string xmlData, string site_id)
        {
            try
            {
                //return xmlData;
                WebRequest request = WebRequest.Create(SrvName +"/gwt/inspection_app/integration/?token=" + Token + "&protocol=" + Protocol + "&site_id=" + site_id);
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
        public string Retrieve(string microtingUuid, int microtingCheckUuid, string site_id)
        {
            try
            {
                WebRequest request = WebRequest.Create(SrvName + "/gwt/inspection_app/integration/" + microtingUuid + "?token=" + Token + "&protocol=" + Protocol + "&site_id=" + site_id + "&download=true&delete=false&last_check_id=" + microtingCheckUuid);
                request.Method = "GET";

                return PostToServer(request);
            }
            catch (Exception ex)
            {
                return "<?xml version='1.0' encoding='UTF-8'?>\n\t<Response>\n\t\t<Value type='converterError'>" + ex.Message + "</Value>\n\t</Response>";
            }
        }

        #endregion

        #region Private methods
        private string PostToServer(WebRequest request)
        {
            // Hack for ignoring certificate validation.
            ServicePointManager.ServerCertificateValidationCallback = Validator;

            WebResponse response = request.GetResponse();
            Stream dataResponseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataResponseStream);
            string responseFromServer = reader.ReadToEnd();

            // Clean up the streams.
            reader.Close();
            dataResponseStream.Close();
            response.Close();

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
            reader.Close();
            dataResponseStream.Close();
            response.Close();

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

        public string Token { get; set; }
        public string SrvName { get; set; }
    }
}

