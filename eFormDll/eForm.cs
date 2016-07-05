using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace eFormDll
{
    public class eForm
    {
        #region con
        public eForm()
        {

        }
        #endregion

        #region public
        /// <summary>
        /// Posts the XML eForm to Microting and returns the XML encoded restponse (Does not support Entity_Search or Entity_Select).
        /// </summary>
        /// <param name="xmlStr">XML encoded eForm string.</param>
        public string PostXml        (string apiId, string token, string serverAddress, string xmlStr)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            if (xmlStr.Contains("type=\"Entity_Select\">"))
                throw new SystemException("Needs to use PostExtendedXml method instead, as Entity_Select is needs a Organization Id");

            Http http = new Http(token, serverAddress);

            return http.Post(xmlStr, apiId);
        }

        /// <summary>
        /// Posts the XML eForm to Microting and returns the XML encoded restponse (Supports Entity_Search or Entity_Select).
        /// </summary>
        /// <param name="xmlStr">XML encoded eForm string.</param>
        /// <param name="organizationId">Identifier of organization, data is to be connected to.</param>
        public string PostExtendedXml(string apiId, string token, string serverAddress, string xmlStr, string organizationId)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            Http http = new Http(token, serverAddress);

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
        public string CheckStatus    (string apiId, string token, string serverAddress, string eFormId)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            Http http = new Http(token, serverAddress);

            return http.Status(eFormId, apiId);
        }

        /// <summary>
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve results from.</param>
        public string Retrieve  (string apiId, string token, string serverAddress, string eFormId)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            Http http = new Http(token, serverAddress);

            return http.Retrieve(eFormId, 0, apiId); //Always gets the first
        }

        /// <summary>
        /// Retrieve the XML encoded results from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to retrieve results from.</param>
        /// <param name="eFormResponseId">Identifier of the check to begin from.</param>
        public string RetrieveFormId     (string apiId, string token, string serverAddress, string eFormId, int eFormResponseId)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            Http http = new Http(token, serverAddress);

            return http.Retrieve(eFormId, eFormResponseId, apiId);
        }

        /// <summary>
        /// Marks an element for deletion and retrieve the XML encoded response from Microting.
        /// </summary>
        /// <param name="eFormId">Identifier of the eForm to mark for deletion.</param>
        public string Delete         (string apiId, string token, string serverAddress, string eFormId)
        {
            if (CheckInput(token, serverAddress) != "")
                return CheckInput(token, serverAddress);

            Http http = new Http(token, serverAddress);

            return http.Delete(eFormId, apiId);
        }
        #endregion

        #region private
        private string CheckInput(string token, string serverAddress)
        {
            string returnMsg = "";

            if (token.Length != 32)
            {
                return "Tokens are always 32 charactors long" + Environment.NewLine ;
            }

            if (!serverAddress.Contains("http://") && !serverAddress.Contains("https://"))
            {
                return "Server Address is missing 'http://' or 'https://'" + Environment.NewLine;
            }

            return returnMsg.TrimEnd();
        }

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
