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
        private string apiId;
        private Http http;
        #endregion

        #region con
        public Communicator(string apiId, string token, string serverAddress)
        {
            this.apiId = apiId;
            #region CheckInput token & serverAddress
            string errorsFound = "";

            if (token.Length != 32)
            {
                errorsFound += "Tokens are always 32 charactors long" + Environment.NewLine;
            }

            if (!serverAddress.Contains("http://") && !serverAddress.Contains("https://"))
            {
                errorsFound += "Server Address is missing 'http://' or 'https://'" + Environment.NewLine;
            }

            if (errorsFound != "")
                throw new InvalidOperationException(errorsFound.TrimEnd());
            #endregion

            http = new Http(token, serverAddress);
        }
        #endregion

        #region public
        /// <summary>
        /// Posts the XML eForm to Microting and returns the XML encoded restponse (Does not support the complex elements Entity_Search or Entity_Select).
        /// </summary>
        /// <param name="xmlStr">XML encoded eForm string.</param>
        public string PostXml        (string xmlStr)
        {
            if (xmlStr.Contains("type=\"Entity_Select\">"))
                throw new SystemException("Needs to use PostExtendedXml method instead, as Entity_Select is needs a Organization Id");

            return http.Post(xmlStr, apiId);
        }

        /// <summary>
        /// Posts the XML eForm to Microting and returns the XML encoded restponse (Supports Entity_Search or Entity_Select).
        /// </summary>
        /// <param name="xmlStr">XML encoded eForm string.</param>
        /// <param name="organizationId">Identifier of organization, data is to be connected to.</param>
        public string PostExtendedXml(string xmlStr, string organizationId)
        {
            if (xmlStr.Contains("type=\"Entity_Select\">"))
            {
                int startIndex = 0;
                while (xmlStr.Contains("<EntityTypeData>"))
                {
                    string inderXmlStr, responseXml, mUUId;

                    #region create EntityType server side.
                    try
                    {
                        inderXmlStr = ReadFirst(xmlStr, "<EntityTypeData>", "</EntityTypeData>", false);

                        string entityTypeXmlStr = "<EntityTypes><EntityType>" + ReadFirst(inderXmlStr, "<Name>", "</Name>", true) + ReadFirst(inderXmlStr, "<Id>", "</Id>", true) + "</EntityType></EntityTypes>";

                        responseXml = http.CreatEntityType(entityTypeXmlStr, organizationId);
                    }
                    catch (Exception ex)
                    {
                        throw new SystemException("Failed to create EntityType", ex);
                    }
                    #endregion

                    #region create Entity server side.
                    try
                    {
                        if (responseXml.Contains("workflowState=\"created\">"))
                        {
                            mUUId = ReadFirst(responseXml, "<MicrotingUUId>", "</MicrotingUUId>", false);

                            string oldTag = ReadFirst(inderXmlStr, "<EntityTypeId>", "</EntityTypeId>", true);

                            string entityXmlStr = inderXmlStr.Replace(oldTag, "<EntityTypeId>" + mUUId + "</EntityTypeId>");

                            entityXmlStr = ReadFirst(entityXmlStr, "<Entities>", "</Entities>", true);

                            responseXml = http.CreatEntity(entityXmlStr, organizationId);
                        }
                        else
                        {
                            throw new Exception("Failed to get 'workflowState =\"created\"'. Hence was unable to create the EntityType.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new SystemException("Failed to send or recieve EntityType xml", ex);
                    }
                    #endregion

                    #region update xml
                    try
                    {
                        if (responseXml.Contains("workflowState=\"created\">"))
                        {
                            string textToBeReplaced = ReadFirst(xmlStr, "<EntityTypeData>", "</EntityTypeData>", true);
                            xmlStr = xmlStr.Remove(xmlStr.IndexOf(textToBeReplaced, startIndex), textToBeReplaced.Length);



                            textToBeReplaced = ReadFirst(xmlStr.Substring(startIndex), "<Source>", "</Source>", true);
                            int index = xmlStr.IndexOf(textToBeReplaced, startIndex);

                            xmlStr = xmlStr.Remove(index, textToBeReplaced.Length);
                            xmlStr = xmlStr.Insert(index, "<Source>" + mUUId + "</Source>");

                            if (xmlStr.Contains("<EntityTypeData>"))
                                startIndex = index + textToBeReplaced.Length;
                        }
                        else
                        {
                            throw new Exception("Failed to get 'workflowState =\"created\"'. Hence was unable to create the Entities.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new SystemException("Failed to update xml with source id", ex);
                    }
                    #endregion
                }
            }

            return http.Post(xmlStr, apiId);
        }

        /// <summary>
        /// Retrieve the XML encoded status from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve status of.</param>
        public string CheckStatus    (string eFormId)
        {
            return http.Status(eFormId, apiId);
        }

        /// <summary>
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve results from.</param>
        public string Retrieve       (string eFormId)
        {
            return http.Retrieve(eFormId, 0, apiId); //Always gets the first
        }

        /// <summary>
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve results from.</param>
        /// <param name="eFormResponseId">Identifier of the check to begin from.</param>
        public string RetrieveFormId (string eFormId, int eFormResponseId)
        {
            return http.Retrieve(eFormId, eFormResponseId, apiId);
        }

        /// <summary>
        /// Marks an element for deletion and retrieve the XML encoded response from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to mark for deletion.</param>
        public string Delete         (string eFormId)
        {
            return http.Delete(eFormId, apiId);
        }
        #endregion

        #region private
        private string ReadFirst(string textStr, string startStr, string endStr, bool keepStartAndEnd)
        {
            try
            {
                int startIndex = textStr.IndexOf(startStr) + startStr.Length;
                int lenght = textStr.IndexOf(endStr, startIndex) - startIndex;
                if (keepStartAndEnd)
                    return startStr + textStr.Substring(startIndex, lenght) + endStr;
                else
                    return textStr.Substring(startIndex, lenght).Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to find:'" + startStr + "' or '" + endStr + "'.", ex);
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
