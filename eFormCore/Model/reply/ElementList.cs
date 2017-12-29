using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace eFormData
{
    [Serializable()]
    public class ElementList
    {
        internal ElementList()
        {
            DataItemList = new List<DataItemReply>();
            ExtraDataItemList = new List<DataItemReply>();
        }

        #region var
        public string Id { get; set; }
        public string Status { get; set; }

        [XmlArray("DataItemList"), XmlArrayItem(typeof(DataItemReply), ElementName = "DataItem")]
        public List<DataItemReply> DataItemList { get; set; }

        [XmlArray("ExtraDataItemList"), XmlArrayItem(typeof(DataItemReply), ElementName = "DataItem")]
        public List<DataItemReply> ExtraDataItemList { get; set; }
        #endregion
    }
}