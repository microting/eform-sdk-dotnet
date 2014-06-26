using System;

namespace Inspection
{
    public class NoneElement : DataElement
    {

        public NoneElement(string id, string label, string description, bool mandatory, bool readOnly, int color, string element_id)
        {
            Id = id;
            Label = label;
            Description = description;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            setColor(color);
            ElementId = element_id;
        }

        public bool ReadOnly { get; set; }
    }
}

