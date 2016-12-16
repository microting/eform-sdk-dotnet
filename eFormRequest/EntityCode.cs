using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eFormRequest
{
    public class EntityCode
    {
        public EntityCode()
        {

        }

        #region old method
        public void Run(string xmlString, string organizationId)
        {
            int startIndex = 0;
            while (xmlString.Contains("<EntityTypeData>"))
            {
                string inderXmlStr, responseXml, mUUId;

                #region create EntityType server side.
                try
                {
                    inderXmlStr = ReadFirst(xmlString, "<EntityTypeData>", "</EntityTypeData>", false);

                    string entityTypeXmlStr = "<EntityTypes><EntityType>" + ReadFirst(inderXmlStr, "<Name>", "</Name>", true) + ReadFirst(inderXmlStr, "<Id>", "</Id>", true) + "</EntityType></EntityTypes>";

                    responseXml = ""; //http.CreatEntityType(entityTypeXmlStr, organizationId);
                }
                catch (Exception ex)
                {
                    throw new SystemException("Failed to create EntityType", ex);
                }
                #endregion

                #region create Entity server side.
                try
                {
                    if (responseXml.Contains("workflowState=\"created\""))
                    {
                        mUUId = ReadFirst(responseXml, "<MicrotingUUId>", "</MicrotingUUId>", false);

                        string oldTag = ReadFirst(inderXmlStr, "<EntityTypeId>", "</EntityTypeId>", true);

                        string entityXmlStr = inderXmlStr.Replace(oldTag, "<EntityTypeId>" + mUUId + "</EntityTypeId>");

                        entityXmlStr = ReadFirst(entityXmlStr, "<Entities>", "</Entities>", true);

                        responseXml = ""; //http.CreatEntity(entityXmlStr, organizationId);
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
                    if (responseXml.Contains("workflowState=\"created\""))
                    {
                        string textToBeReplaced = ReadFirst(xmlString, "<EntityTypeData>", "</EntityTypeData>", true);
                        xmlString = xmlString.Remove(xmlString.IndexOf(textToBeReplaced, startIndex), textToBeReplaced.Length);



                        textToBeReplaced = ReadFirst(xmlString.Substring(startIndex), "<EntityTypeId>", "</EntityTypeId>", true);
                        int index = xmlString.IndexOf(textToBeReplaced, startIndex);

                        xmlString = xmlString.Remove(index, textToBeReplaced.Length);
                        xmlString = xmlString.Insert(index, "<EntityTypeId>" + mUUId + "</EntityTypeId>");

                        if (xmlString.Contains("<EntityTypeData>"))
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
    }
}
