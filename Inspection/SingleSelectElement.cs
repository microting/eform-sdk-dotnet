using System;
using System.Collections.Generic;
using System.Xml;

namespace Inspection
{
    public class SingleSelectElement : DataElement
    {
        public SingleSelectElement(string id, string label, string description, bool mandatory, List<KeyValuePair> keyValuePairList, int color, string element_id)
        {
            Id = id;
            Label = label;
            Description = description;
            Mandatory = mandatory;
            KeyValuePairList = keyValuePairList;
            setColor(color);
            ElementId = element_id;
        }

        public List<KeyValuePair> KeyValuePairList { get; set; }

        public void toXml(XmlWriter writer, List<KeyValuePair> KeyValuePairList)
        {
            writer.WriteStartElement("KeyValuePairList");
            foreach (KeyValuePair kvp in KeyValuePairList)
            {
                writer.WriteStartElement("KeyValuePair");
                writer.WriteElementString("Key", kvp.Key);
                writer.WriteElementString("Value", kvp.ValueOfKeyValuePair);
                writer.WriteElementString("Selected", kvp.Selected.ToString().ToLower());
                writer.WriteElementString("DisplayOrder", kvp.DisplayOrder);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}

