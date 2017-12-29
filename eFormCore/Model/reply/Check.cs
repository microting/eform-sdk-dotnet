using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace eFormData
{
    [Serializable()]
    public class Check
    {
        #region con
        internal Check()
        {
            ElementList = new List<ElementList>();
        }
        #endregion

        #region var
        public string UnitId { get; set; }
        #region public string Date { get; set; }
        private string date = "";
        public string Date { get { return date.Substring(0, 19); } set { date = value; } }
        #endregion
        public string Worker { get; set; }
        public string Id { get; set; }
        public string WorkerId { get; set; }

        [XmlArray("ElementList"), XmlArrayItem(typeof(ElementList), ElementName = "Element")]
        public List<ElementList> ElementList { get; set; }
        #endregion
    }
}
