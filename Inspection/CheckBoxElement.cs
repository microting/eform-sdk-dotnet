using System;

namespace Inspection
{
    public class CheckBoxElement : DataElement
    {
        public CheckBoxElement(string id, string label, string description, bool mandatory, bool value, bool selected, int color, string element_id)
        {
            Id = id;
            Label = label;
            Description = description;
            Mandatory = mandatory;
            Value = value;
            Selected = selected;
            this.setColor(color);
            this.ElementId = element_id;
        }
        public bool Value { get; set; }
        public bool Selected { get; set; }
    }
}

