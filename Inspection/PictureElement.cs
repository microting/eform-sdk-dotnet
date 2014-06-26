using System;

namespace Inspection
{
    public class PictureElement : DataElement
    {

        public PictureElement(string id, string label, string description, bool mandatory, int multi, int color, string element_id)
        {
            Id = id;
            Label = label;
            Description = description;
            Mandatory = mandatory;
            Multi = multi;
            setColor(color);
            ElementId = element_id;
        }

        public int Multi { get; set; }
    }
}

