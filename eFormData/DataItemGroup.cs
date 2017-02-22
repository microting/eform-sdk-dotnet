using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace eFormData
{
    [Serializable()]
    [XmlInclude(typeof(FieldGroup))]
    public class DataItemGroup
    {
        #region con
        internal DataItemGroup()
        {

        }
        #endregion

        #region var
        public string Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int DisplayOrder { get; set; }
        #endregion
    }

    #region FieldGroup : DataItemGroup
    public class FieldGroup : DataItemGroup
    {
        internal FieldGroup()
        {

        }

        public FieldGroup(string id, string label, string description, string color, int displayOrder, string value, List<DataItem> dataItemList)
        {
            Id = id;
            Label = label;
            Description = description;
            Color = color;
            DisplayOrder = displayOrder;

            Value = value;
            DataItemList = dataItemList;
        }

        public string Value { get; set; }

        [XmlArray("DataItemList"), XmlArrayItem(typeof(DataItem), ElementName = "DataItem")]
        public List<DataItem> DataItemList { get; set; }
    }
    #endregion
}