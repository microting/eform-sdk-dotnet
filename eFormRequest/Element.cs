using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace eFormRequest
{
    [Serializable()]
    [XmlInclude(typeof(DataElement))]
    [XmlInclude(typeof(GroupElement))]
    public class Element
    {
        #region con
        internal Element()
        {

        }
        #endregion

        #region var
        public int Id { get; set; }
        public string Label { get; set; }
        public int DisplayOrder { get; set; }

        [XmlElement("Description")]
        public CDataValue Description { get; set; }
        public bool ApprovalEnabled { get; set; }
        public bool ReviewEnabled { get; set; }
        public bool DoneButtonEnabled { get; set; }
        public bool ExtraFieldsEnabled { get; set; }
        public string PinkBarText { get; set; }
        #endregion
    }

    #region GroupElement : Element
    public class GroupElement : Element
    {
        internal GroupElement()
        {
            ElementList = new List<Element>();
        }

        public GroupElement(int id, string label, int displayOrder, string description, bool approvedEnabled, bool reviewEnabled, bool doneButtonEnabled,
            bool extraDataElementsEnabled, string pinkBarText, List<Element> elementList)
        {
            ElementList = new List<Element>();

            Id = id;
            Label = label;
            DisplayOrder = displayOrder;
            Description = new CDataValue();
            Description.InderValue = description;
            ApprovalEnabled = approvedEnabled;
            ReviewEnabled = reviewEnabled;
            DoneButtonEnabled = doneButtonEnabled;
            ExtraFieldsEnabled = extraDataElementsEnabled;
            PinkBarText = pinkBarText;

            ElementList = elementList;
        }

        [XmlArray("ElementList"), XmlArrayItem(typeof(Element), ElementName = "Element")]
        public List<Element> ElementList { get; set; }
    }
    #endregion

    #region DataElement : Element
    public class DataElement : Element
    {
        internal DataElement()
        {
            DataItemGroupList = new List<DataItemGroup>();
            DataItemList = new List<DataItem>();
        }

        public DataElement(int id, string label, int displayOrder, string description, bool approvalEnabled, bool reviewEnabled, bool doneButtonEnabled,
            bool extraDataElementsEnabled, string pinkBarText, List<DataItemGroup> dateItemGroupList, List<DataItem> dataItemList)
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

            DataItemGroupList = dateItemGroupList;
            DataItemList = dataItemList;
        }

        [XmlArray("DataItemGroupList"), XmlArrayItem(typeof(DataItemGroup), ElementName = "DataItemGroup")]
        public List<DataItemGroup> DataItemGroupList { get; set; }

        [XmlArray("DataItemList"), XmlArrayItem(typeof(DataItem), ElementName = "DataItem")]
        public List<DataItem> DataItemList { get; set; }
    }
    #endregion

    #region CheckListValue : Element
    public class CheckListValue : Element
    {
        internal CheckListValue()
        {
            DataItemGroupList = new List<DataItemGroup>();
            DataItemList = new List<DataItem>();
        }

        public CheckListValue(DataElement dataElement, string status)
        {
            Id = dataElement.Id;
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
            Status = status;
        }

        [XmlArray("DataItemGroupList"), XmlArrayItem(typeof(DataItemGroup), ElementName = "DataItemGroup")]
        public List<DataItemGroup> DataItemGroupList { get; set; }

        [XmlArray("DataItemList"), XmlArrayItem(typeof(DataItem), ElementName = "DataItem")]
        public List<DataItem> DataItemList { get; set; }
        public string Status { get; set; }
    }
    #endregion
}