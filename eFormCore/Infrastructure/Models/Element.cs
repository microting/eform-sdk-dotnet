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
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure.Data.Entities;

namespace Microting.eForm.Infrastructure.Models
{
    [Serializable]
    [XmlInclude(typeof(DataElement))]
    [XmlInclude(typeof(GroupElement))]
    public class Element
    {
        // con
        internal Element()
        {
        }
        //

        // var
        public int Id { get; set; }
        public string Label { get; set; }
        public int DisplayOrder { get; set; }

        [XmlElement("Description")] public CDataValue Description { get; set; }
        public bool ApprovalEnabled { get; set; }
        public bool ReviewEnabled { get; set; }
        public bool DoneButtonEnabled { get; set; }
        public bool ExtraFieldsEnabled { get; set; }
        public string PinkBarText { get; set; }
        public bool QuickSyncEnabled { get; set; }

        public string OriginalId { get; set; }
        //
    }

    // GroupElement : Element
    public class GroupElement : Element
    {
        internal GroupElement()
        {
            ElementList = new List<Element>();
        }

        public GroupElement(int id, string label, int displayOrder, string description, bool approvedEnabled,
            bool reviewEnabled, bool doneButtonEnabled,
            bool extraDataElementsEnabled, string pinkBarText, bool quickSyncEnabled, List<Element> elementList)
        {
            ElementList = new List<Element>();

            Id = id;
            Label = label;
            DisplayOrder = displayOrder;
            Description = new CDataValue { InderValue = description };
            ApprovalEnabled = approvedEnabled;
            ReviewEnabled = reviewEnabled;
            DoneButtonEnabled = doneButtonEnabled;
            ExtraFieldsEnabled = extraDataElementsEnabled;
            PinkBarText = pinkBarText;
            QuickSyncEnabled = quickSyncEnabled;

            ElementList = elementList;
        }

        [XmlArray("ElementList"), XmlArrayItem(typeof(Element), ElementName = "Element")]
        public List<Element> ElementList { get; set; }
    }
    //

    // DataElement : Element
    public class DataElement : Element
    {
        internal DataElement()
        {
            DataItemGroupList = new List<DataItemGroup>();
            DataItemList = new List<DataItem>();
        }

        public DataElement(int id, string label, int displayOrder, string description, bool approvalEnabled,
            bool reviewEnabled, bool doneButtonEnabled,
            bool extraDataElementsEnabled, string pinkBarText, bool quickSyncEnabled,
            List<DataItemGroup> dataItemGroupList, List<DataItem> dataItemList)
        {
            //DataItemGroupList = new List<DataItemGroup>();
            //DataItemList = new List<DataItem>();

            Id = id;
            Label = label;
            DisplayOrder = displayOrder;
            Description = new CDataValue();
            Description.InderValue = description;
            ApprovalEnabled = approvalEnabled;
            ReviewEnabled = reviewEnabled;
            DoneButtonEnabled = doneButtonEnabled;
            ExtraFieldsEnabled = extraDataElementsEnabled;
            PinkBarText = pinkBarText;
            QuickSyncEnabled = quickSyncEnabled;

            DataItemGroupList = dataItemGroupList;
            DataItemList = dataItemList;
        }

        [XmlArray("DataItemGroupList"), XmlArrayItem(typeof(DataItemGroup), ElementName = "DataItemGroup")]
        public List<DataItemGroup> DataItemGroupList { get; set; }

        [XmlArray("DataItemList"), XmlArrayItem(typeof(DataItem), ElementName = "DataItem")]
        public List<DataItem> DataItemList { get; set; }

        [XmlIgnore] public List<Models.FieldValue> ExtraPictures { get; set; }
        [XmlIgnore] public List<Models.FieldValue> ExtraRecordings { get; set; }
        [XmlIgnore] public List<Models.FieldValue> ExtraComments { get; set; }
    }
    //

    // CheckListValue : Element
    public class CheckListValue : Element
    {
        internal CheckListValue()
        {
            DataItemGroupList = new List<DataItemGroup>();
            DataItemList = new List<DataItem>();
            ExtraPictures = new List<Models.FieldValue>();
            ExtraRecordings = new List<Models.FieldValue>();
            ExtraComments = new List<Models.FieldValue>();
        }

        public CheckListValue(DataElement dataElement, string status)
        {
            Id = dataElement.Id;
            OriginalId = dataElement.OriginalId;
            Label = dataElement.Label;
            DisplayOrder = dataElement.DisplayOrder;
            Description = new CDataValue();
            Description.InderValue = dataElement.Description.InderValue;
            ApprovalEnabled = dataElement.ApprovalEnabled;
            ReviewEnabled = dataElement.ReviewEnabled;
            DoneButtonEnabled = dataElement.DoneButtonEnabled;
            ExtraFieldsEnabled = dataElement.ExtraFieldsEnabled;
            PinkBarText = dataElement.PinkBarText;

            DataItemGroupList = dataElement.DataItemGroupList;
            DataItemList = dataElement.DataItemList;
            ExtraComments = dataElement.ExtraComments;
            ExtraPictures = dataElement.ExtraPictures;
            ExtraRecordings = dataElement.ExtraRecordings;
            Status = status;
        }

        [XmlArray("DataItemGroupList"), XmlArrayItem(typeof(DataItemGroup), ElementName = "DataItemGroup")]
        public List<DataItemGroup> DataItemGroupList { get; set; }

        [XmlArray("DataItemList"), XmlArrayItem(typeof(DataItem), ElementName = "DataItem")]
        public List<DataItem> DataItemList { get; set; }

        public List<FieldValue> ExtraPictures { get; set; }
        public List<FieldValue> ExtraRecordings { get; set; }
        public List<FieldValue> ExtraComments { get; set; }
        public string Status { get; set; }
    }
    //
}