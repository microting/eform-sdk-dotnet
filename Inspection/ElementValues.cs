using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspection
{
    public class ElementValues
    {
        public ElementValues(int element_id, string value, string status, int case_id)
        {
            this.ElementId = element_id;
            this.Value = value;
            this.Status = status;
            this.CaseId = case_id;
        }

        public int ElementId { get; set; }
        public string Value { get; set; }
        public string Status { get; set; }
        public int CaseId { get; set; }
    }
}
