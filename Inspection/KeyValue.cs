using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inspection
{
    public class KeyValue
    {
        public KeyValue(string id, string key, string value)
        {
            this.Id = id;
            this.Key = key;
            this.Value = value;
        }

        public string Value { get; set; }
        public string Key { get; set; }
        public string Id { get; set; }
    }
}
