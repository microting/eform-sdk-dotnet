using System;

namespace Inspection
{
    public class KeyValuePair
    {
        public KeyValuePair(string key, string valueOfKeyValuePair, bool selected, string displayOrder)
        {
            DisplayOrder = displayOrder;
            Key = key;
            ValueOfKeyValuePair = valueOfKeyValuePair;
            Selected = selected;
        }

        public string Key { get; set; }
        public string ValueOfKeyValuePair { get; set; }
        public bool Selected { get; set; }
        public string DisplayOrder { get; set; }
    }
}

