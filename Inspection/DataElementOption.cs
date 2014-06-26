using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Inspection
{
    class DataElementOption
    {
        public DataElementOption(string key, string value, bool selected)
        {
            this.Key = key;
            this.Value = value;
            this.Selected = selected;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }

        public void toXml(XmlWriter writer)
        {

        }
    }
}
