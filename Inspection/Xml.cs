using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace Inspection
{
    public class Xml
    {
        #region Public methods

        /// <summary>
        /// Converts the XML to an Result object.
        /// </summary>
        /// <param name="xmlData">XML encoded string from Microting.</param>
       
        /* LASSE
        public static Result XmlToResult(string xmlData)
        {
            Dictionary<string, string> typeDic = new Dictionary<string, string>();
            return XmlToResult(xmlData, typeDic);
        }
        */
       
        /// <summary>
        /// Converts the XML to an Result object.
        /// </summary>
        /// <param name="xmlData">XML encoded string from Microting.</param>
        /// <param name="typeDic">Dictionary containing id (Key) and type (Value) for the items in the result</param>
        ///

        /* LASSE
        public static Result XmlToResult(string xmlData, Dictionary<string, string> typeDic)
        {
            Result result = new Result();

            // Response and value
            XDocument xmlDoc = XDocument.Parse(xmlData);
            var xResponses = from xResponse in xmlDoc.Descendants("Response")
                             select new
                             {
                                 ExecutionStatus = xResponse.Element("Value").Attribute("type").Value,
                                 Message = xResponse.Element("Value"),
                             };
            result.ExecutionStatus = (xResponses.First().ExecutionStatus.ToString() == "success") ? true : false;
            result.Message = xResponses.First().Message.Value.ToString();

            // Checks
            var xChecks = from xCheck in xmlDoc.Descendants("Check")
                          select new
                          {
                              UnitId = xCheck.Attribute("unit_id"),
                              Date = xCheck.Attribute("date"),
                              DoneByUserId = xCheck.Attribute("worker_id"),
                              Id = xCheck.Attribute("id"),
                              ElementLists = xCheck.Elements("ElementList")     
                          };

            if (xChecks.Count() != 0)
            {
                foreach (var xCheck in xChecks)
                {
                    Cases cases = new Cases();
                    cases.UnitId = Int32.Parse(xCheck.UnitId.Value.ToString());
                    cases.DoneByUserId = Int32.Parse(xCheck.DoneByUserId.Value.ToString());
                    cases.Id = Int32.Parse(xCheck.Id.Value.ToString());
                    cases.Date = DateConverter(xCheck.Date.Value.ToString());

                    foreach (var xElementList in xCheck.ElementLists)
                    {
                        ElementList elementList = new ElementList();
                        elementList.Id = xElementList.Element("Id").Value;
                        elementList.Status = xElementList.Element("Status").Value;

                        // DataItemLists
                        foreach (var xDataItemList in xElementList.Descendants("DataItemList"))
                        {
                            DataItemList dataItemList = new DataItemList();
                            foreach (var xDataItem in xDataItemList.Descendants("DataItem"))
                            {
                                string id = (xDataItem.Element("Id") != null) ? xDataItem.Element("Id").Value : "";
                                string valueOfItem = xDataItem.Element("Value").Value;
                                string type = (typeDic.FirstOrDefault(t => t.Key == id).Value != null) ? typeDic.FirstOrDefault(t => t.Key == id).Value : "";
                                Geolocation geo = new Geolocation("00.0", "00.0", "00.0", "00.0", DateTime.MinValue);
                                foreach (var xGeo in xDataItem.Descendants("Geolocation"))
                                {
                                    geo.Latitude = (xGeo.Element("Latitude") != null) ? xGeo.Element("Latitude").Value : "00.0";
                                    geo.Longitude = (xGeo.Element("Longitude") != null) ? xGeo.Element("Longitude").Value : "00.0";
                                    geo.Alttitude = (xGeo.Element("Altitude") != null) ? xGeo.Element("Altitude").Value : "00.0";
                                    geo.Heading = (xGeo.Element("Heading") != null) ? xGeo.Element("Heading").Value : "00.0";
                                    geo.Date = (xGeo.Element("Date") != null) ? DateConverter(xGeo.Element("Date").Value) : DateTime.MinValue;
                                }
                                DataElement dataElement = new DataElement(id, valueOfItem, type, geo);
                                dataItemList.AddDataItem(dataElement);
                            }
                            elementList.AddDataItemList(dataItemList);
                        }

                        // ExtraItemLists
                        foreach (var xExtraItemList in xElementList.Descendants("ExtraDataItemList"))
                        {
                            ExtraDataItemList extraItemList = new ExtraDataItemList();
                            foreach (var xExtraItem in xExtraItemList.Descendants("DataItem"))
                            {
                                string id = (xExtraItem.Element("Id") != null) ? xExtraItem.Element("Id").Value : "";
                                string valueOfItem = xExtraItem.Element("Value").Value;
                                string type = (typeDic.FirstOrDefault(t => t.Key == id).Value != null) ? typeDic.FirstOrDefault(t => t.Key == id).Value : "";
                                Geolocation geo = new Geolocation("00.0", "00.0", "00.0", "00.0", DateTime.MinValue);
                                foreach (var xGeo in xExtraItem.Descendants("Geolocation"))
                                {
                                    geo.Latitude = (xGeo.Element("Latitude") != null) ? xGeo.Element("Latitude").Value : "00.0";
                                    geo.Longitude = (xGeo.Element("Longitude") != null) ? xGeo.Element("Longitude").Value : "00.0";
                                    geo.Alttitude = (xGeo.Element("Altitude") != null) ? xGeo.Element("Altitude").Value : "00.0";
                                    geo.Heading = (xGeo.Element("Heading") != null) ? xGeo.Element("Heading").Value : "00.0";
                                    geo.Date = (xGeo.Element("Date") != null) ? DateConverter(xGeo.Element("Date").Value) : DateTime.MinValue;
                                }
                                ExtraDataItem extraItem = new ExtraDataItem(id, valueOfItem, type, geo);
                                extraItemList.AddExtraDataItem(extraItem);
                            }
                            elementList.AddExtraDataItemList(extraItemList);
                        }
                        cases.ElementLists.Add(elementList);
                    }
                    result.AddCheck(cases);
                }
            }
            return result;
        }
        */
        
        /// <summary>
        /// Converts the checkList to an XML encoded string compatible with Microting API 1.2
        /// </summary>
        /// <param name='element'>The checkList</param>
        public static string ElementToXml(Element element)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = new UTF8Encoding(false);
            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
            xmlWriterSettings.Indent = true;

            XmlWriter writer = XmlWriter.Create(memoryStream, xmlWriterSettings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Main");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
            writer.WriteElementString("Id", element.Id.ToString());
            writer.WriteElementString("Label", element.Label);
            writer.WriteElementString("Repeated", element.Repeated.ToString().ToLower());
            writer.WriteElementString("StartDate", element.StartDate.ToString("yyyy-MM-dd"));
            writer.WriteElementString("EndDate", element.EndDate.ToString("yyyy-MM-dd"));
            writer.WriteElementString("Language", element.Language);
            writer.WriteElementString("MultiApproval", element.MultiApproval.ToString().ToLower());
            writer.WriteElementString("FastNavigation", element.FastNavigation.ToString().ToLower());
            writer.WriteElementString("ManualSync", element.ManualSync.ToString().ToLower());
            writer.WriteElementString("CheckListFolderName", element.FolderName);
            writer.WriteElementString("DownloadEnities", element.DownloadEntities.ToString().ToLower());

            /* LASSE
            if (element.CheckListList != null)
                RecChkListListIteration(writer, element.CheckListList);
            else if (element.ElementList != null)
                RecElementListIteration(writer, element);
            */
            writer.WriteEndElement(); // Main
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            string xmlString = Encoding.UTF8.GetString(memoryStream.ToArray());

            return xmlString;
        }

        /// <summary>
        /// Converts the checkList to an XML reference text, which will be used to match results and checks.
        /// </summary>
        /// <param name="element">The checkList</param>
        public static string ElementToXmlRef(Element element)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = new UTF8Encoding(false);
            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
            xmlWriterSettings.Indent = true;

            XmlWriter writer = XmlWriter.Create(memoryStream, xmlWriterSettings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Element");
            writer.WriteElementString("Id", element.Id.ToString());

            /* LASSE
            if (element.CheckListList != null)
                RecChkListListIterationRef(writer, element.CheckListList);
            else if (element.ElementList != null)
                RecElementListIterationRef(writer, element);
            */
            
            writer.WriteEndElement(); // Main
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            string xmlString = Encoding.UTF8.GetString(memoryStream.ToArray());

            return xmlString;
        }

        public static Dictionary<string, string> GetCheckTypes(string xmlData)
        {
            Dictionary<string, string> typeDic = new Dictionary<string, string>();
            XDocument xmlDoc = XDocument.Parse(xmlData);

            var xChecks = from xCheck in xmlDoc.Descendants("Check")
                          select new
                          {
                              Id = xCheck.Attribute("id"),
                              Type = xCheck.Attribute("type")
                          };
            foreach (var xCheck in xChecks)
            {
                typeDic.Add(xCheck.Id.Value.ToString(), xCheck.Type.Value.ToString());
            }

            return typeDic;
        }

        #endregion

        #region Private methods

        private static DateTime DateConverter(string xmlDateTime)
        {
            int Year = Int32.Parse(xmlDateTime.Substring(0, 4));
            int Month = Int32.Parse(xmlDateTime.Substring(5, 2));
            int Day = Int32.Parse(xmlDateTime.Substring(8, 2));
            int Hour = Int32.Parse(xmlDateTime.Substring(11, 2));
            int Minute = Int32.Parse(xmlDateTime.Substring(14, 2));
            int Second = Int32.Parse(xmlDateTime.Substring(17, 2));

            DateTime returnDate = new DateTime(Year, Month, Day, Hour, Minute, Second);
            return returnDate;
        }

        /// <summary>
        /// Writes CheckElements to the XML.
        /// </summary>
        /// <param name='writer'>The XML-Writer to write XML in.</param>
        /// <param name='element'>The checkList that contains a list of elements.</param>
        private static void RecElementListIteration(XmlWriter writer, Element element)
        {
            writer.WriteStartElement("ElementList");
            writer.WriteStartElement("Element");
            writer.WriteAttributeString("type", "DataElement");
            writer.WriteElementString("Id", element.Id.ToString());
            writer.WriteElementString("Label", element.Label);
            writer.WriteElementString("ReviewEnabled", element.ReviewEnabled.ToString().ToLower());
            writer.WriteElementString("ApprovedEnabled", element.ApprovedEnabled.ToString().ToLower());
            writer.WriteElementString("DoneButtonDisabled", element.DoneButtonDisabled.ToString().ToLower());
            writer.WriteElementString("ExtraFieldsEnabled", element.ExtraDataElementsEnabled.ToString().ToLower());

            writer.WriteStartElement("DataItemList");
            foreach (Element e in element.ElementList)
            {
                switch (e.GetType().ToString())
                {
                    case "Microting.CheckLists.AudioElement":
                        AudioElement eAudioElement = (AudioElement)e;
                        writer.WriteStartElement("DataItem");
                        writer.WriteAttributeString("type", "audio");
                        writer.WriteElementString("Id", e.Id);
                        writer.WriteElementString("Label", e.Label);
                        writer.WriteElementString("ReadOnly", e.ReadOnly.ToString().ToLower());
                        writer.WriteElementString("Mandatory", e.Mandatory.ToString().ToLower());
                        writer.WriteElementString("Multi", eAudioElement.Multi.ToString());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.CheckBoxElement":
                        CheckBoxElement eCheckBoxElement = (CheckBoxElement)e;
                        writer.WriteStartElement("DataItem");
                        writer.WriteAttributeString("type", "check_box");
                        writer.WriteElementString("Id", e.Id);
                        writer.WriteElementString("ReadOnly", e.ReadOnly.ToString().ToLower());
                        writer.WriteElementString("Mandatory", e.Mandatory.ToString().ToLower());
                        writer.WriteElementString("Label", e.Label);
                        writer.WriteElementString("Value", eCheckBoxElement.ValueOfElement.ToString().ToLower());
                        writer.WriteElementString("Selected", eCheckBoxElement.Selected.ToString().ToLower());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.CommentElement":
                        CommentElement eCommentElement = (CommentElement)e;
                        writer.WriteStartElement("DataItem");
                        writer.WriteAttributeString("type", "comment");
                        writer.WriteElementString("Id", e.Id);
                        writer.WriteElementString("ReadOnly", e.ReadOnly.ToString().ToLower());
                        writer.WriteElementString("Mandatory", e.Mandatory.ToString().ToLower());
                        writer.WriteElementString("Label", e.Label);
                        writer.WriteElementString("Value", eCommentElement.ValueOfElement);
                        writer.WriteElementString("MaxLength", eCommentElement.Maxlength.ToString());
                        writer.WriteElementString("Split", eCommentElement.SplitScreen.ToString().ToLower());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.DateElement":
                        DateElement eDateElement = (DateElement)e;
                        writer.WriteStartElement("DataItem");
                        writer.WriteAttributeString("type", "date");
                        writer.WriteElementString("Id", e.Id);
                        writer.WriteElementString("ReadOnly", e.ReadOnly.ToString().ToLower());
                        writer.WriteElementString("Mandatory", e.Mandatory.ToString().ToLower());
                        writer.WriteElementString("Label", e.Label);
                        writer.WriteElementString("MinValue", eDateElement.MinValue.ToString("yyyy-MM-dd"));
                        writer.WriteElementString("MaxValue", eDateElement.MaxValue.ToString("yyyy-MM-dd"));
                        writer.WriteElementString("Value", eDateElement.ValueOfElement.ToString("yyyy-MM-dd"));
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.MovieElement":
                        //MovieElement eMovieElement = (MovieElement)e;
                        writer.WriteStartElement("DataItem");
                        writer.WriteAttributeString("type", "movie");
                        writer.WriteElementString("Id", e.Id);
                        writer.WriteElementString("ReadOnly", e.ReadOnly.ToString().ToLower());
                        writer.WriteElementString("Mandatory", e.Mandatory.ToString().ToLower());
                        writer.WriteElementString("Label", e.Label);
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.MultiSelectElement":
                        MultiSelectElement eMultiSelectElement = (MultiSelectElement)e;
                        writer.WriteStartElement("DataItem");
                        writer.WriteAttributeString("type", "multi_select");
                        writer.WriteElementString("Id", e.Id);
                        writer.WriteElementString("ReadOnly", e.ReadOnly.ToString().ToLower());
                        writer.WriteElementString("Mandatory", e.Mandatory.ToString().ToLower());
                        writer.WriteElementString("Label", e.Label);
                        writer.WriteStartElement("KeyValueCheckedList");
                        foreach (KeyValueChecked kvp in eMultiSelectElement.KeyValueCheckedList)
                        {
                            writer.WriteStartElement("KeyValueChecked");
                            writer.WriteElementString("Key", kvp.Key);
                            writer.WriteElementString("Value", kvp.ValueOfKeyValueChecked);
                            writer.WriteElementString("Selected", kvp.Selected.ToString().ToLower());
                            writer.WriteEndElement(); // KeyValueChecked
                        }
                        writer.WriteEndElement(); // KeyValueCheckedList
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.NoneElement":
                        //NoneElement eNoneElement = (NoneElement)e;
                        writer.WriteStartElement("DataItem");
                        writer.WriteAttributeString("type", "none");
                        writer.WriteElementString("Id", e.Id);
                        writer.WriteElementString("ReadOnly", e.ReadOnly.ToString().ToLower());
                        writer.WriteElementString("Mandatory", e.Mandatory.ToString().ToLower());
                        writer.WriteElementString("Label", e.Label);
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.NumberElement":
                        NumberElement eNumberElement = (NumberElement)e;
                        writer.WriteStartElement("DataItem");
                        writer.WriteAttributeString("type", "number");
                        writer.WriteElementString("Id", e.Id);
                        writer.WriteElementString("ReadOnly", e.ReadOnly.ToString().ToLower());
                        writer.WriteElementString("Mandatory", e.Mandatory.ToString().ToLower());
                        writer.WriteElementString("Label", e.Label);
                        writer.WriteElementString("MinValue", eNumberElement.MinValue.ToString());
                        writer.WriteElementString("MaxValue", eNumberElement.MaxValue.ToString());
                        writer.WriteElementString("Value", eNumberElement.ValueOfElement.ToString());
                        writer.WriteElementString("DecimalCount", eNumberElement.DecimalCount.ToString());
                        writer.WriteElementString("UnitName", eNumberElement.UnitName);
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.PictureElement":
                        PictureElement ePictureElement = (PictureElement)e;
                        writer.WriteStartElement("DataItem");
                        writer.WriteAttributeString("type", "picture");
                        writer.WriteElementString("Id", e.Id);
                        writer.WriteElementString("ReadOnly", e.ReadOnly.ToString().ToLower());
                        writer.WriteElementString("Mandatory", e.Mandatory.ToString().ToLower());
                        writer.WriteElementString("Label", e.Label);
                        writer.WriteElementString("Multi", ePictureElement.Multi.ToString());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.SingleSelectElement":
                        SingleSelectElement eSingleSelectElement = (SingleSelectElement)e;
                        writer.WriteStartElement("DataItem");
                        writer.WriteAttributeString("type", "single_select");
                        writer.WriteElementString("Id", e.Id);
                        writer.WriteElementString("ReadOnly", e.ReadOnly.ToString().ToLower());
                        writer.WriteElementString("Mandatory", e.Mandatory.ToString().ToLower());
                        writer.WriteElementString("Label", e.Label);
                        writer.WriteStartElement("KeyValueCheckedList");
                        foreach (KeyValueChecked kvp in eSingleSelectElement.KeyValueCheckedList)
                        {
                            writer.WriteStartElement("KeyValueChecked");
                            writer.WriteElementString("Key", kvp.Key);
                            writer.WriteElementString("Value", kvp.ValueOfKeyValueChecked);
                            writer.WriteElementString("Selected", kvp.Selected.ToString().ToLower());
                            writer.WriteEndElement(); // KeyValueChecked
                        }
                        writer.WriteEndElement(); // KeyValueCheckedList
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.TextElement":
                        TextElement eTextElement = (TextElement)e;
                        writer.WriteStartElement("DataItem");
                        writer.WriteAttributeString("type", "text");
                        writer.WriteElementString("Id", e.Id);
                        writer.WriteElementString("ReadOnly", e.ReadOnly.ToString().ToLower());
                        writer.WriteElementString("Mandatory", e.Mandatory.ToString().ToLower());
                        writer.WriteElementString("Label", e.Label);
                        writer.WriteElementString("Value", eTextElement.ValueOfElement);
                        writer.WriteElementString("MaxLength", eTextElement.MaxLength.ToString());
                        writer.WriteElementString("GeolocationEnabled", eTextElement.GeolocationEnabled.ToString().ToLower());
                        writer.WriteElementString("GeolocationHidden", eTextElement.GeolocationHidden.ToString().ToLower());
                        writer.WriteElementString("GeolocationForced", eTextElement.GeolocationForced.ToString().ToLower());
                        writer.WriteEndElement();
                        break;
                }
            }
            writer.WriteEndElement(); // DataItemList

            writer.WriteEndElement(); // Element
            writer.WriteEndElement(); // ElementList
        }

        /// <summary>
        /// Iterates through checkLists and writes either GroupElement or calls method to write CheckElement to the XML. 
        /// </summary>
        /// <param name='writer'>The XML-Writer to write XML in.</param>
        /// <param name='ElementList'>The list of checkLists, which either contain further checkLists or elementlists</param>
        private static void RecChkListListIteration(XmlWriter writer, List<Element> ElementList)
        {
            foreach (Element e in ElementList)
            {
                // If CheckList contains checkLists, write as GroupElement
                if (e.ElementList != null)
                {
                    writer.WriteStartElement("ElementList");
                    writer.WriteStartElement("Element");
                    writer.WriteAttributeString("type", "GroupElement");
                    writer.WriteElementString("Id", e.Id.ToString());
                    writer.WriteElementString("Label", e.Label);
                    writer.WriteElementString("PinkBarText", e.PinkBarText);
                    writer.WriteElementString("Description", e.Description);

                    RecChkListListIteration(writer, e.ElementList);

                    writer.WriteEndElement(); // Element
                    writer.WriteEndElement(); // ElementList
                }

                // If checkList contains elements, write as DataElement
                if (e.ElementList != null)
                    RecElementListIteration(writer, e);
            }
        }

        /// <summary>
        /// Writes CheckElements to the reference XML.
        /// </summary>
        /// <param name='writer'>The XML-Writer to write XML in.</param>
        /// <param name='element'>The checkList that contains a list of elements.</param>
        private static void RecElementListIterationRef(XmlWriter writer, Element element)
        {
            writer.WriteStartElement("CheckList");
            writer.WriteStartElement("CheckGroup");
            writer.WriteAttributeString("type", "DataElement");
            writer.WriteElementString("Id", element.Id.ToString());

            writer.WriteStartElement("CheckItemList");
            foreach (Element e in element.ElementList)
            {
                switch (e.GetType().ToString())
                {
                    case "Microting.CheckLists.AudioElement":
                        writer.WriteStartElement("Check");
                        writer.WriteAttributeString("type", "audio");
                        writer.WriteAttributeString("id", e.Id.ToString());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.CheckBoxElement":
                        writer.WriteStartElement("Check");
                        writer.WriteAttributeString("type", "check_box");
                        writer.WriteAttributeString("id", e.Id.ToString());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.CommentElement":
                        writer.WriteStartElement("Check");
                        writer.WriteAttributeString("type", "comment");
                        writer.WriteAttributeString("id", e.Id.ToString());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.DateElement":
                        writer.WriteStartElement("Check");
                        writer.WriteAttributeString("type", "date");
                        writer.WriteAttributeString("id", e.Id.ToString());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.MovieElement":
                        writer.WriteStartElement("Check");
                        writer.WriteAttributeString("type", "movie");
                        writer.WriteAttributeString("id", e.Id.ToString());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.MultiSelectElement":
                        writer.WriteStartElement("Check");
                        writer.WriteAttributeString("type", "multi_select");
                        writer.WriteAttributeString("id", e.Id.ToString());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.NoneElement":
                        writer.WriteStartElement("Check");
                        writer.WriteAttributeString("type", "none");
                        writer.WriteAttributeString("id", e.Id.ToString());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.NumberElement":
                        writer.WriteStartElement("Check");
                        writer.WriteAttributeString("type", "number");
                        writer.WriteAttributeString("id", e.Id.ToString());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.PictureElement":
                        writer.WriteStartElement("Check");
                        writer.WriteAttributeString("type", "picture");
                        writer.WriteAttributeString("id", e.Id.ToString());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.SingleSelectElement":
                        writer.WriteStartElement("Check");
                        writer.WriteAttributeString("type", "single_select");
                        writer.WriteAttributeString("id", e.Id.ToString());
                        writer.WriteEndElement();
                        break;
                    case "Microting.CheckLists.TextElement":
                        writer.WriteStartElement("Check");
                        writer.WriteAttributeString("type", "text");
                        writer.WriteAttributeString("id", e.Id.ToString());
                        writer.WriteEndElement();
                        break;
                }
            }
            writer.WriteEndElement(); // CheckItemList

            writer.WriteEndElement(); // Check
            writer.WriteEndElement(); // CheckList
        }

        /// <summary>
        /// Iterates through checkLists and writes either GroupElement or calls method to write CheckElement to the reference XML. 
        /// </summary>
        /// <param name='writer'>The XML-Writer to write XML in.</param>
        /// <param name='elementList'>The list of checkLists, which either contain further checkLists or elementlists</param>
        private static void RecChkListListIterationRef(XmlWriter writer, List<Element> elementList)
        {
            foreach (Element e in elementList)
            {
                // If CheckList does contain CheckLists, write as GroupElement
                if (e.ElementList != null)
                {
                    writer.WriteStartElement("CheckList");
                    writer.WriteStartElement("CheckGroup");
                    writer.WriteAttributeString("type", "GroupElement");
                    writer.WriteElementString("Id", e.Id.ToString());


                    RecChkListListIterationRef(writer, e.ElementList);

                    writer.WriteEndElement(); // Check
                    writer.WriteEndElement(); // CheckList
                }
                // Else if checklist does contain elements, write as DataElement
                else if (e.ElementList != null)
                    RecElementListIterationRef(writer, e);
            }
        }

        #endregion
    }
}
