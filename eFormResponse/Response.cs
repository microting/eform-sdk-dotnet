using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace eFormResponse
{
    public class Response
    {
        #region con
        public Response()
        {
            Checks = new List<Check>();
        }

        public Response(ResponseTypes type, string value)
        {
            Type = type;
            Value = value;
            Checks = new List<Check>();
        }
        #endregion

        #region var
        public ResponseTypes Type { get; set; }
        public string Value { get; set; }
        public string UnitFetchedAt { get; set; }
        public string UnitId { get; set; }
        public List<Check> Checks{ get; set; }
        #endregion

        #region public
        public Response XmlToClass(string xmlStr)
        {
            try
            {
                ResponseTypes rType = ResponseTypes.Invalid;
                string value = "";

                #region value type
                if (xmlStr.Contains("<Value type="))
                {
                    string subXmlStr = xmlStr.Substring(xmlStr.IndexOf("<Value type=\"") + 13); // 13 magic int = "<Value type=\"".Length;
                    string valueTypeLower = subXmlStr.Substring(0, subXmlStr.IndexOf("\"")).ToLower(); //digs out value's type
                    value = subXmlStr.Substring(valueTypeLower.Length + 2, subXmlStr.IndexOf("</Value>") - valueTypeLower.Length - 2); //digs out value's text

                    if (valueTypeLower == ("success"))
                        rType = ResponseTypes.Success;

                    if (valueTypeLower == ("error"))
                        rType = ResponseTypes.Error;

                    if (valueTypeLower == ("parsing"))
                        rType = ResponseTypes.Parsing;

                    if (valueTypeLower == ("received"))
                        rType = ResponseTypes.Received;
                }
                #endregion

                Response resp = new Response(rType, value);

                #region Unit fetched
                if (xmlStr.Contains("<Unit fetched_at="))
                {
                    string subXmlStr = xmlStr.Substring(xmlStr.IndexOf("<Unit fetched_at=\"") + 18); // 18 magic int = "<Unit fetched_at=\"".Length;
                    string dateTimeStr = subXmlStr.Substring(0, subXmlStr.IndexOf("\"")); //digs out unit's dateTime
                    string idStr = subXmlStr.Substring(dateTimeStr.Length + 6, subXmlStr.IndexOf("\"/>") - dateTimeStr.Length - 6); //digs out value's text

                    resp.UnitFetchedAt = dateTimeStr;
                    resp.UnitId = idStr;
                }
                #endregion

                #region checks
                string checkXmlStr = xmlStr;
                while (checkXmlStr.Contains("<Check "))
                {
                    Check check = new Check();

                    check.UnitId =   Locate(checkXmlStr, " unit_id=\"",   "\"");
                    check.Date =     Locate(checkXmlStr, " date=\"",      "\"");
                    check.Worker =   Locate(checkXmlStr, " worker=\"",    "\"");
                    check.Id =       Locate(checkXmlStr, " id=\"",        "\"");
                    check.WorkerId = Locate(checkXmlStr, " worker_id=\"", "\"");

                    while (checkXmlStr.Contains("<ElementList>"))
                    {
                        string inderXmlStr = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?><ElementList>" + Locate(checkXmlStr, "<ElementList>", "</ElementList>") + "</ElementList>";
                        ElementList eResp = XmlToClassCheck(inderXmlStr);
                        check.ElementList.Add(eResp);

                        int index = checkXmlStr.IndexOf("</ElementList>");
                        checkXmlStr = checkXmlStr.Substring(index + 14); //removes extracted xml
                    }

                    resp.Checks.Add(check);
                }
                #endregion

                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception("Response failed to convert XML", ex);
            }
        }

        public string ClassToXml()
        {
            try
            {
                string      xmlStr  = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine +
                                      "<Response>" + Environment.NewLine +
                                      "<Value type=\"" + Type + "\">" + Value + "</Value>" + Environment.NewLine; 
                if (UnitId != null)
                            xmlStr +=     "<Unit fetched_at=\"" + UnitFetchedAt + "\" id=\"" + UnitId + "\"/>" + Environment.NewLine;
                if (Checks.Count > 0)
                {
                            xmlStr +=     "<Checks>" + Environment.NewLine;
                    foreach (Check chk in Checks) {
                            xmlStr +=         "<Check unit_id=\""+chk.UnitId+"\" date=\""+chk.Date+"\" worker=\""+chk.Worker+"\" id=\""+chk.Id+"\" worker_id=\""+chk.WorkerId+"\">";
                        foreach (ElementList elemLst in chk.ElementList) { 
                            xmlStr +=             PureXml(ClassToXmlCheck(elemLst)) + Environment.NewLine;
                        }
                            xmlStr +=         "</Check>" + Environment.NewLine;
                    }
                            xmlStr +=     "</Checks>" + Environment.NewLine;
                }
                            xmlStr += "</Response>";

                return xmlStr;
            }
            catch (Exception ex)
            {
                throw new Exception("Response failed to convert Class", ex);
            }
        }
        #endregion

        #region private
        private ElementList XmlToClassCheck(string xmlStr)
        {
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(xmlStr);
                XmlSerializer serializer = new XmlSerializer(typeof(ElementList));
                StreamReader reader = new StreamReader(new MemoryStream(byteArray));

                ElementList elementResp = null;
                elementResp = (ElementList)serializer.Deserialize(reader);
                reader.Close();

                return elementResp;
            }
            catch (Exception ex)
            {
                throw new Exception("Response failed to convert XML", ex);
            }
        }

        private string ClassToXmlCheck(ElementList elementList)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(ElementList));
                string xmlStr;
                using (StringWriter writer = new Utf8StringWriter())
                {
                    serializer.Serialize(writer, elementList);
                    xmlStr = writer.ToString();
                }
                return xmlStr;
            }
            catch (Exception ex)
            {
                throw new Exception("Response failed to convert Class", ex);
            }
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

        private string PureXml(string xmlStr)
        {
            xmlStr = xmlStr.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
            xmlStr = xmlStr.Replace("<ElementList xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<ElementList>");

            return xmlStr.Trim();
        }

        private string Locate(string textStr, string startStr, string endStr)
        {
            int startIndex = textStr.IndexOf(startStr) + startStr.Length;
            int lenght = textStr.IndexOf(endStr, startIndex) - startIndex;
            return textStr.Substring(startIndex, lenght);
        }
        #endregion

        public enum ResponseTypes
        {
            Error,
            Received,
            Parsing,
            Success,
            Invalid
        }
    }
}