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

    #region Nested type: NodeType

    public class CDataValue
    {
        [XmlIgnore]
        public string InderValue { get; set; }

        [XmlText]
        public XmlNode[] CDataWrapper
        {
            get
            {
                var dummy = new XmlDocument();
                return new XmlNode[] { dummy.CreateCDataSection(InderValue) };
            }
            set
            {
                if (value == null)
                {
                    InderValue = null;
                    return;
                }

                if (value.Length != 1)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            "Invalid array length {0}", value.Length));
                }

                InderValue = value[0].Value;
            }
        }
    }

    #endregion
}