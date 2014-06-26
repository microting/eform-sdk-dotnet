using System;

namespace Inspection
{
    public class AudioElement : DataElement
    {
        public AudioElement(string id, string label, string description, bool mandatory, int multi, int color, string element_id)
        {
            Id = id;
            Label = label;
            Description = description;
            Mandatory = mandatory;
            Multi = multi;
            this.setColor(color);
            this.ElementId = element_id;
        }

        public int Multi { get; set; }
    }
}