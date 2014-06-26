using System;

namespace Inspection
{
    public class DateElement : DataElement
    {


        public DateElement(string id, string label, string description, bool mandatory, DateTime minValue, DateTime maxValue, string value, int color, string element_id)
        {
            Id = id;
            Label = label;
            Description = description;
            Mandatory = mandatory;
            MinValue = minValue;
            MaxValue = maxValue;
            Value = value;
            setColor(color); 
            ElementId = element_id;
        }
        public string Value { get; set; }
        public DateTime MaxValue { get; set; }
        public DateTime MinValue { get; set; }
    }
}

