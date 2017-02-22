using eFormShared;

using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace eFormData
{
    [Serializable()]
    [XmlRoot("DataItem")]
    public class DataItemReply
    {
        internal DataItemReply()
        {

        }

        #region var
        public string Id { get; set; }

        public GeolocationData Geolocation { get; set; }

        [XmlElement("Value")]
        public CDataValue Value { get; set; }

        public string Extension { get; set; }
        public string URL { get; set; }
        #endregion
    }
}