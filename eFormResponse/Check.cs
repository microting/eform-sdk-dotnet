using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace eFormResponse
{
    [Serializable()]
    public class Check
    {
        internal Check()
        {
            ElementList = new List<ElementList>();
        }

        #region var
        public string UnitId { get; set; }
        public string Date { get; set; }
        public string Worker { get; set; }
        public string Id { get; set; }
        public string WorkerId { get; set; }

        [XmlArray("ElementList"), XmlArrayItem(typeof(ElementList), ElementName = "Element")]
        public List<ElementList> ElementList { get; set; }
        #endregion
    }
}
