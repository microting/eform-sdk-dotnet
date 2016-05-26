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
using System.Xml;

namespace eFormDll
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

