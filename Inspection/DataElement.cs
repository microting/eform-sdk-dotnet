using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Inspection
{
    public class DataElement
    {
        private enum ColorCode
        {
            e2f4fb, // 0 Blue
            f5eafa, // 1 Purple
            f0f8db, // 2 Green
            fff6df, // 3 Yellow
            ffe4e4, // 4 Red
            None    // 5 
        };

        private string Color;
        List<DataElementOption> DataElementOptions;

        public DataElement(string id, string label, string description, bool mandatory, string element_id, int color)//, int data_element_type_id)
        {
            this.Id = id;
            this.Label = label;
            this.Description = description;
            this.Mandatory = mandatory;
            this.ElementId = element_id;
            Color = Enum.GetName(typeof(ColorCode), color);
            this.DataElementOptions = new List<DataElementOption>();
        }

        public DataElement() { }

        public string Id { get; set; }
        public string Description { get; set; }
        public bool Mandatory { get; set; }
        public string ElementId { get; set; }
        public string Label { get; set; }

        public void setColor(int color)
        {
            Color = Enum.GetName(typeof(ColorCode), color);
        }

        public void toXml(XmlWriter writer)
        {
            writer.WriteStartElement("DataItem");
            switch (this.GetType().Name.ToString())
            {
                case "TextElement":
                    TextElement textElement = (TextElement)this;
                    writer.WriteAttributeString("type", "text");
                    writer.WriteElementString("Value", textElement.Value.ToString().ToLower());
                    writer.WriteElementString("MaxLength", textElement.MaxLength.ToString().ToLower());
                    writer.WriteElementString("GeolocationEnabled", textElement.GeolocationEnabled.ToString().ToLower());
                    writer.WriteElementString("GeolocationForced", textElement.GeolocationForced.ToString().ToLower());
                    writer.WriteElementString("GeolocationHidden", textElement.GeolocationHidden.ToString().ToLower());
                    break;

                case "CheckBoxElement":
                    CheckBoxElement checkBoxElement = (CheckBoxElement)this;
                    writer.WriteAttributeString("type", "check_box");
                    writer.WriteElementString("Value", checkBoxElement.Value.ToString().ToLower());
                    writer.WriteElementString("Selected", checkBoxElement.Selected.ToString().ToLower());
                    break;

                case "AudioElement":
                    AudioElement audioElement = (AudioElement)this;
                    writer.WriteAttributeString("type", "audio");
                    writer.WriteElementString("Multi", audioElement.Multi.ToString().ToLower());
                    break;

                case "CommentElement":
                    CommentElement commentElement = (CommentElement)this;
                    writer.WriteAttributeString("type", "comment");
                    writer.WriteElementString("Value", commentElement.Value.ToString().ToLower());
                    writer.WriteElementString("MaxLength", commentElement.Maxlength.ToString().ToLower());
                    writer.WriteElementString("SplitScreen", commentElement.SplitScreen.ToString().ToLower());
                    break;

                case "DateElement":
                    DateElement dateElement = (DateElement)this;
                    writer.WriteAttributeString("type", "date");
                    writer.WriteElementString("Value", dateElement.Value.ToString().ToLower());
                    writer.WriteElementString("MinValue", dateElement.MinValue.ToString().ToLower());
                    writer.WriteElementString("MaxValue", dateElement.MaxValue.ToString().ToLower());
                    break;

                case "MultiSelectElement":
                    MultiSelectElement multiSelectElement = (MultiSelectElement)this;
                    writer.WriteAttributeString("type", "multi_select");
                    multiSelectElement.toXml(writer, multiSelectElement.KeyValuePairList);
                    break;

                case "NoneElement":
                    NoneElement noneElement = (NoneElement)this;
                    writer.WriteAttributeString("type", "none");
                    writer.WriteElementString("ReadOnly", noneElement.ReadOnly.ToString().ToLower());
                    
                    break;

                case "NumberElement":
                    NumberElement numberElement = (NumberElement)this;
                    writer.WriteAttributeString("type", "number");
                    writer.WriteElementString("Value", numberElement.Value.ToString().ToLower());
                    writer.WriteElementString("MinValue", numberElement.MinValue.ToString().ToLower());
                    writer.WriteElementString("MaxValue", numberElement.MaxValue.ToString().ToLower());
                    writer.WriteElementString("DecimalCount", numberElement.DecimalCount.ToString().ToLower());
                    writer.WriteElementString("UnitName", numberElement.UnitName.ToString().ToLower());
                    break;

                case "PictureElement":
                    PictureElement pictureElement = (PictureElement)this;
                    writer.WriteAttributeString("type", "picture");
                    writer.WriteElementString("Multi", pictureElement.Multi.ToString().ToLower());
                    break;

                case "SingleSelectElement":
                    SingleSelectElement singleSelectElement = (SingleSelectElement)this;
                    writer.WriteAttributeString("type", "single_select");
                    singleSelectElement.toXml(writer, singleSelectElement.KeyValuePairList);
                    break;

                case "PdfElement":
                    PdfElement pdfElement = (PdfElement)this;
                    writer.WriteAttributeString("type", "show_pdf");
                    writer.WriteElementString("PathValue", pdfElement.PathValue.ToString().ToLower());
                    break;

                case "TimerElement":
                    TimerElement timerElement = (TimerElement)this;
                    writer.WriteAttributeString("type", "timer");
                    break;

                case "SignatureElement":
                    SignatureElement signatureElement = (SignatureElement)this;
                    writer.WriteAttributeString("type", "signature");
                    break;
            }
            writer.WriteElementString("Id", this.Id);
            writer.WriteElementString("Label", this.Label);
            writer.WriteElementString("Description", this.Description);
            writer.WriteElementString("Mandatory", this.Mandatory.ToString().ToLower());
            if (this.Color.ToString() != ColorCode.None.ToString())
            {
                writer.WriteElementString("Color", this.Color);
            }
            writer.WriteEndElement();
        }
    }
}