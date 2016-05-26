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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace eFormDll
{
    public class Element
    {
        public Element()
        {

        }
        
        public Element(string id, string label, int repeated, DateTime start_date, DateTime end_date, string language, bool multi_approval, bool fast_navigation, string parent_id, bool review_enabled, bool approved_enabled, bool done_button_enabled, bool extra_data_elements_enabled, bool manual_sync, string folder_name, bool download_entities, string pink_bar_text, string description)
        {
            this.Id = id;
            this.Label = label;
            this.Repeated = repeated;
            this.StartDate = start_date;
            this.EndDate = end_date;
            this.Language = language;
            this.MultiApproval = multi_approval;
            this.FastNavigation = fast_navigation;
            this.ParentId = parent_id;
            this.ReviewEnabled = review_enabled;
            this.ApprovedEnabled = approved_enabled;
            this.DoneButtonEnabled = done_button_enabled;
            this.ExtraDataElementsEnabled = extra_data_elements_enabled;
            this.ManualSync = manual_sync;
            this.FolderName = folder_name;
            this.DownloadEntities = download_entities;
            this.PinkBarText = pink_bar_text;
            this.Description = description;
        }

        public String Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public string FolderName { get; set; }
        public int Repeated { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Language { get; set; }
        public bool MultiApproval { get; set; }
        public bool FastNavigation { get; set; }
        public bool ReviewEnabled { get; set; }
        public bool ApprovedEnabled { get; set; }
        public bool DoneButtonEnabled { get; set; }
        public bool ExtraDataElementsEnabled { get; set; }
        public bool ManualSync { get; set; }
        public bool DownloadEntities { get; set; }
        public string PinkBarText { get; set; }
        public bool Mandatory { get; set; }
        public bool ReadOnly { get; set; }

        public void mainToXml(XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Main");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
            writer.WriteElementString("Id", this.Id.ToString());
            writer.WriteElementString("Label", this.Label);
            writer.WriteElementString("Description", this.Description);
            writer.WriteElementString("Repeated", this.Repeated.ToString().ToLower());
            writer.WriteElementString("StartDate", this.StartDate.ToString("yyyy-MM-dd"));
            writer.WriteElementString("EndDate", this.EndDate.ToString("yyyy-MM-dd"));
            writer.WriteElementString("Language", this.Language);
            writer.WriteElementString("MultiApproval", this.MultiApproval.ToString().ToLower());
            writer.WriteElementString("FastNavigation", this.FastNavigation.ToString().ToLower());
            writer.WriteElementString("ManualSync", this.ManualSync.ToString().ToLower());
            writer.WriteElementString("CheckListFolderName", this.FolderName);
            writer.WriteElementString("DownloadEnities", this.DownloadEntities.ToString().ToLower());
            
            writer.WriteStartElement("ElementList");
            List<Element> elements = DbFacade.Instance.findChildElementsById(this.Id);
            foreach (Element e in elements)
            {
                e.toXml(writer);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        public void toXml(XmlWriter writer)
        {
            List<Element> elements = DbFacade.Instance.findChildElementsById(this.Id);
            if (elements.Count == 0)
            {
                writer.WriteStartElement("Element");
                writer.WriteAttributeString("type", "DataElement");
            }
            else
            {
                writer.WriteStartElement("Element");
                writer.WriteAttributeString("type", "GroupElement");
            }
            writer.WriteElementString("Id", this.Id);
            writer.WriteElementString("Label", this.Label);
            writer.WriteElementString("Description", this.Description);

            // Find children
            if (elements.Count == 0)
            {

                writer.WriteElementString("ReviewEnabled", this.ReviewEnabled.ToString().ToLower());
                writer.WriteElementString("ApprovedEnabled", this.ApprovedEnabled.ToString().ToLower());
                writer.WriteElementString("DoneButtonEnabled", this.DoneButtonEnabled.ToString().ToLower());
                writer.WriteElementString("ExtraFieldsEnabled", this.ExtraDataElementsEnabled.ToString().ToLower());
                List<DataElement> dataElements = DbFacade.Instance.findAllDataElementsByElementId(this.Id);
                if (dataElements.Count != 0) 
                {
                    writer.WriteStartElement("DataItemList");
                    foreach (DataElement d in dataElements)
                    {
                        d.toXml(writer);                        
                    }
                    writer.WriteEndElement();
                }
            }
            else
            {
                writer.WriteStartElement("ElementList");
                foreach (Element e in elements)
                {
                    e.toXml(writer);
                }
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

        }
    }

}
