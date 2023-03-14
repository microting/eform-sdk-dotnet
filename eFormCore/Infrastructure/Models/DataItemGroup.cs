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
using System.Xml.Serialization;

namespace Microting.eForm.Infrastructure.Models
{
    [Serializable]
    [XmlInclude(typeof(FieldGroup))]
    public class DataItemGroup
    {
        #region con

        internal DataItemGroup()
        {
        }

        //public DataItemGroup(string id, string label, string description, string color, int displayOrder, string value, List<DataItem> dataItemList)
        //{
        //    id = id;
        //    label = label;
        //    description = description;
        //    color = color;
        //    DisplayOrder = displayOrder;

        //    value = value;
        //    DataItemList = dataItemList;
        //}

        #endregion

        #region var

        public string Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int DisplayOrder { get; set; }
        public string Value { get; set; }

        [XmlArray("DataItemList"), XmlArrayItem(typeof(DataItem), ElementName = "DataItem")]
        public List<DataItem> DataItemList { get; set; }

        #endregion
    }

    #region FieldGroup : DataItemGroup

    public class FieldGroup : DataItemGroup
    {
        internal FieldGroup()
        {
        }

        public FieldGroup(string id, string label, string description, string color, int displayOrder, string value,
            List<DataItem> dataItemList)
        {
            Id = id;
            Label = label;
            Description = description;
            Color = color;
            DisplayOrder = displayOrder;

            Value = value;
            DataItemList = dataItemList;
        }

        //public string Value { get; set; }

        //[XmlArray("DataItemList"), XmlArrayItem(typeof(DataItem), ElementName = "DataItem")]
        //public List<DataItem> DataItemList { get; set; }
    }

    #endregion
}