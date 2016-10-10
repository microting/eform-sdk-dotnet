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
        public string Id { get; set; }
        public string Label { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
        public bool ApprovalEnabled { get; set; }
        public bool ReviewEnabled { get; set; }
        public bool DoneButtonEnabled { get; set; }
        public bool ExtraFieldsEnabled { get; set; }
        public string PinkBarText { get; set; }
        #endregion
    }

    #region DataElement
    public class DataElement : Element
    {
        internal DataElement()
        {
            DataItemGroupList = new List<DataItemGroup>();
            DataItemList = new List<DataItem>();
        }

        public DataElement(string id, string label, int displayOrder, string description, bool approvalEnabled, bool reviewEnabled, bool doneButtonEnabled,
            bool extraDataElementsEnabled, string pinkBarText, List<DataItemGroup> dateItemGroupList, List<DataItem> dataItemList)
        {
            DataItemGroupList = new List<DataItemGroup>();
            DataItemList = new List<DataItem>();

            Id = id;
            Label = label;
            DisplayOrder = displayOrder;
            Description = description;
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

    #region GroupElement
    public class GroupElement : Element
    {
        internal GroupElement()
        {
            ElementList = new List<Element>();
        }

        public GroupElement(string id, string label, int displayOrder, string description, bool approvedEnabled, bool reviewEnabled, bool doneButtonEnabled,
            bool extraDataElementsEnabled, string pinkBarText, List<Element> elementList)
        {
            ElementList = new List<Element>();

            Id = id;
            Label = label;
            DisplayOrder = displayOrder;
            Description = description;
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
}

