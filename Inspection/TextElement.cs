using System;
using System.Xml;

namespace Inspection
{
    public class TextElement : DataElement
    {

        public TextElement(string id, string label, string description, string element_id, bool mandatory, string value, int maxLength, bool geolocationEnabled, bool geolocationForced, bool geolocationhidden, int color)
        {
            Id = id;
            Label = label;
            Description = description;
            ElementId = element_id;
            setColor(color); 
            Mandatory = mandatory;
            Value = value;
            MaxLength = maxLength;
            GeolocationEnabled = geolocationEnabled;
            GeolocationForced = geolocationForced;
            GeolocationHidden = geolocationhidden;
        }

        public string Value { get; set; }
        public int MaxLength { get; set; }
        public bool GeolocationEnabled { get; set; }
        public bool GeolocationForced { get; set; }
        public bool GeolocationHidden { get; set; }
    }
}

