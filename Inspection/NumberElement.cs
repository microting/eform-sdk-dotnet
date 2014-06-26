using System;

namespace Inspection
{
    public class NumberElement : DataElement
    {
        public NumberElement(string id, string label, string description, bool mandatory, int minValue, int maxValue, int value, int decimalCount, string unitName, int color, string element_id)
        {
            Id = id;
            Label = label;
            Description = description;
            Mandatory = mandatory;
            MinValue = minValue;
            MaxValue = maxValue;
            Value = value;
            DecimalCount = decimalCount;
            UnitName = unitName;
            setColor(color);
            ElementId = element_id;
        }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int Value { get; set; }
        public int DecimalCount { get; set; }
        public string UnitName { get; set; }
    }
}

