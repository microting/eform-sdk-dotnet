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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace Microting.eForm.Infrastructure.Models.reply
{
    public class Value
    {
        public string Type;
    }

    public class Response
    {
        // con
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
        //

        // var
        public ResponseTypes Type { get; set; }
        public string Value { get; set; }
        public string UnitFetchedAt { get; set; }
        public string UnitId { get; set; }
        public List<Check> Checks { get; set; }

        Tools t = new Tools();
        //

        // public
        public Response XmlToClassUsingXmlDocument(string xmlStr)
        {
            try
            {
                ResponseTypes rType = ResponseTypes.Invalid;
                string value = "";

                // value type
                if (xmlStr.Contains("<Value type="))
                {
                    string subXmlStr = t.Locate(xmlStr, "<Response>", "</Response>").Trim();
                    string valueTypeLower =
                        t.Locate(xmlStr, "Value type=\"", "\"").Trim().ToLower(); //digs out value's type
                    value = t.Locate(xmlStr, "\">", "</").Trim();

                    switch (valueTypeLower)
                    {
                        case "success":
                            rType = ResponseTypes.Success;
                            break;

                        case "error":
                            rType = ResponseTypes.Error;
                            break;

                        case "parsing":
                            rType = ResponseTypes.Parsing;
                            break;

                        case "received":
                            rType = ResponseTypes.Received;
                            break;

                        case "invalid":
                            rType = ResponseTypes.Invalid;
                            break;

                        default:
                            throw new IndexOutOfRangeException("ResponseType:'" + valueTypeLower + "' is not known. " +
                                                               xmlStr);
                    }
                }
                //

                Response resp = new Response(rType, value);

                XmlDocument xDoc = new XmlDocument();

                xDoc.LoadXml(xmlStr);

                XmlNode checks = xDoc.DocumentElement.LastChild;
                foreach (XmlNode xmlCheck in checks.ChildNodes)
                {
                    string rawXml = xmlCheck.OuterXml;
                    {
                        Check check = new Check
                        {
                            UnitId = t.Locate(rawXml, " unit_id=\"", "\""),
                            Date = t.Locate(rawXml, " date=\"", "\""),
                            Worker = t.Locate(rawXml, " worker=\"", "\""),
                            Id = int.Parse(t.Locate(rawXml, " id=\"", "\"")),
                            WorkerId = t.Locate(rawXml, " worker_id=\"", "\""),
                            Manufacturer = t.Locate(rawXml, "manufacturer=\"", "\""),
                            Model = t.Locate(rawXml, "model=\"", "\""),
                            OsVersion = t.Locate(rawXml, "os_version=\"", "\""),
                            SoftwareVersion = t.Locate(rawXml, "software_version=\"", "\"")
                        };

                        while (rawXml.Contains("<ElementList>"))
                        {
                            string inderXmlStr = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?><ElementList>" +
                                                 t.Locate(rawXml, "<ElementList>", "</ElementList>") + "</ElementList>";
                            ElementList eResp = XmlToClassCheck(inderXmlStr);
                            check.ElementList.Add(eResp);

                            int index = rawXml.IndexOf("</ElementList>");
                            rawXml = rawXml.Substring(index + 14); //removes extracted xml
                        }

                        resp.Checks.Add(check);
                    }
                }

                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception("Response failed to convert XML", ex);
            }
        }

        public Response XmlToClass(string xmlStr)
        {
            try
            {
                ResponseTypes rType = ResponseTypes.Invalid;
                string value = "";

                // value type
                if (xmlStr.Contains("<Value type="))
                {
                    string subXmlStr = t.Locate(xmlStr, "<Response>", "</Response>").Trim();
                    string valueTypeLower =
                        t.Locate(xmlStr, "Value type=\"", "\"").Trim().ToLower(); //digs out value's type
                    value = t.Locate(xmlStr, "\">", "</").Trim();

                    switch (valueTypeLower)
                    {
                        case "success":
                            rType = ResponseTypes.Success;
                            break;

                        case "error":
                            rType = ResponseTypes.Error;
                            break;

                        case "parsing":
                            rType = ResponseTypes.Parsing;
                            break;

                        case "received":
                            rType = ResponseTypes.Received;
                            break;

                        case "invalid":
                            rType = ResponseTypes.Invalid;
                            break;

                        default:
                            throw new IndexOutOfRangeException("ResponseType:'" + valueTypeLower + "' is not known. " +
                                                               xmlStr);
                    }
                }
                //

                Response resp = new Response(rType, value);

                // Unit fetched
                if (xmlStr.Contains("<Unit fetched_at="))
                {
                    string subXmlStr =
                        xmlStr.Substring(xmlStr.IndexOf("<Unit fetched_at=\"") +
                                         18); // 18 magic int = "<Unit fetched_at=\"".Length;
                    string dateTimeStr = subXmlStr.Substring(0, subXmlStr.IndexOf("\"")); //digs out unit's dateTime
                    string idStr = subXmlStr.Substring(dateTimeStr.Length + 6,
                        subXmlStr.IndexOf("\"/>") - dateTimeStr.Length - 6); //digs out value's text

                    resp.UnitFetchedAt = dateTimeStr;
                    resp.UnitId = idStr;
                }
                //

                // checks
                string checkXmlStr = xmlStr;
                while (checkXmlStr.Contains("<Check "))
                {
                    Check check = new Check
                    {
                        UnitId = t.Locate(checkXmlStr, " unit_id=\"", "\""),
                        Date = t.Locate(checkXmlStr, " date=\"", "\""),
                        Worker = t.Locate(checkXmlStr, " worker=\"", "\""),
                        Id = int.Parse(t.Locate(checkXmlStr, " id=\"", "\"")),
                        WorkerId = t.Locate(checkXmlStr, " worker_id=\"", "\""),
                        Manufacturer = t.Locate(checkXmlStr, "manufacturer=\"", "\""),
                        Model = t.Locate(checkXmlStr, "model=\"", "\""),
                        OsVersion = t.Locate(checkXmlStr, "os_version=\"", "\""),
                        SoftwareVersion = t.Locate(checkXmlStr, "software_version=\"", "\"")
                    };

                    while (checkXmlStr.Contains("<ElementList>"))
                    {
                        string inderXmlStr = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?><ElementList>" +
                                             t.Locate(checkXmlStr, "<ElementList>", "</ElementList>") +
                                             "</ElementList>";
                        ElementList eResp = XmlToClassCheck(inderXmlStr);
                        check.ElementList.Add(eResp);

                        int index = checkXmlStr.IndexOf("</ElementList>");
                        checkXmlStr = checkXmlStr.Substring(index + 14); //removes extracted xml
                    }

                    resp.Checks.Add(check);
                }
                //

                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception("Response failed to convert XML", ex);
            }
        }

        public Response JsonToClass(string json)
        {
            try
            {
                string value = "";
                ResponseTypes rType = ResponseTypes.Invalid;

                // value type

                var jObject = JObject.Parse(json);
                if (jObject["Value"]?["Type"] != null)
                {
                    value = jObject["Value"]["Value"]?.ToString();
                    var parsed = Enum.TryParse(jObject["Value"]["Type"].ToString(), true, out rType);
                    if (!parsed)
                        throw new IndexOutOfRangeException("ResponseType:'" + jObject["Value"]["Type"] +
                                                           "' is not known. " + json);
                }
                //

                Response resp = new Response(rType, value);

                // Unit fetched
                if (jObject["Unit"] != null)
                {
                    resp.UnitFetchedAt = jObject["Unit"]["FatchedAt"]?.ToString();
                    resp.UnitId = jObject["Unit"]["Id"]?.ToString();
                }
                //

                // checks
                if (jObject["Checks"] != null)
                {
                    foreach (var item in jObject["Checks"])
                    {
                        Check check = new Check
                        {
                            UnitId = item["UnitId"]?.ToString(),
                            Date = item["Date"]?.ToString(),
                            Worker = item["Worker"]?.ToString(),
                            Id = item["Id"]?.ToObject<int>(),
                            WorkerId = item["WorkerId"]?.ToString(),
                            Manufacturer = item["manufacturer"]?.ToString(),
                            Model = item["model"]?.ToString(),
                            OsVersion = item["os_version"]?.ToString(),
                            SoftwareVersion = item["software_version"]?.ToString()
                        };

                        foreach (var el in item["ElementList"])
                            check.ElementList.Add(el.ToObject<ElementList>());

                        resp.Checks.Add(check);
                    }
                }
                //

                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception("Response failed to convert Json", ex);
            }
        }


        public string ClassToXml()
        {
            try
            {
                string xmlStr = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine +
                                "<Response>" + Environment.NewLine +
                                "<Value type=\"" + Type + "\">" + Value + "</Value>" + Environment.NewLine;
                if (UnitId != null)
                    xmlStr += "<Unit fetched_at=\"" + UnitFetchedAt + "\" id=\"" + UnitId + "\"/>" +
                              Environment.NewLine;
                if (Checks.Count > 0)
                {
                    xmlStr += "<Checks>" + Environment.NewLine;
                    foreach (Check chk in Checks)
                    {
                        xmlStr += "<Check unit_id=\"" + chk.UnitId + "\" date=\"" + chk.Date + "\" worker=\"" +
                                  chk.Worker + "\" id=\"" + chk.Id + "\" worker_id=\"" + chk.WorkerId + "\">";
                        foreach (ElementList elemLst in chk.ElementList)
                        {
                            xmlStr += PureXml(ClassToXmlCheck(elemLst)) + Environment.NewLine;
                        }

                        xmlStr += "</Check>" + Environment.NewLine;
                    }

                    xmlStr += "</Checks>" + Environment.NewLine;
                }

                xmlStr += "</Response>";

                return xmlStr;
            }
            catch (Exception ex)
            {
                throw new Exception("Response failed to convert Class", ex);
            }
        }
        //

        // private
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
            xmlStr = xmlStr.Replace(
                "<ElementList xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">",
                "<ElementList>");

            return xmlStr.Trim();
        }
        //

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