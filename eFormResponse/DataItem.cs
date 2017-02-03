using eFormShared;

using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace eFormResponse
{
    [Serializable()]
    public class DataItem
    {
        internal DataItem()
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