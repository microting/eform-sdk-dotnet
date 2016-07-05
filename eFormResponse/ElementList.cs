using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace eFormResponse
{
    [Serializable()]
    public class ElementList
    {
        internal ElementList()
        {
            DataItemList = new List<DataItem>();
            ExtraDataItemList = new List<DataItem>();
        }

        #region var
        public string Id { get; set; }
        public string Status { get; set; }

        [XmlArray("DataItemList"), XmlArrayItem(typeof(DataItem), ElementName = "DataItem")]
        public List<DataItem> DataItemList { get; set; }

        [XmlArray("ExtraDataItemList"), XmlArrayItem(typeof(DataItem), ElementName = "DataItem")]
        public List<DataItem> ExtraDataItemList { get; set; }
        #endregion
    }
}