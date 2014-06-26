using System;

namespace Inspection
{
    public class CommentElement : DataElement
    {
        public CommentElement(string id, string label, string description, bool mandatory, string value, int maxLength, bool splitScreen, int color, string element_id)
        {
            Id = id;
            Label = label;
            Description = description;
            Mandatory = mandatory;
            Value = value;
            Maxlength = maxLength;
            SplitScreen = splitScreen;
            setColor(color);
            ElementId = element_id;
        }

        public string Value { get; set; }
        public bool SplitScreen { get; set; }
        public int Maxlength { get; set; }
    }
}

