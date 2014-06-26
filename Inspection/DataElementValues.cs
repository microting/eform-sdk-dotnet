using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspection
{
    public class DataElementValues
    {
        public DataElementValues(int element_id, int data_element_id, string value, int uploaded_data_id, int case_id)
        {
            this.ElementId = element_id;
            this.DataElementId = data_element_id;
            this.Value = value;
            this.UploadedDataId = uploaded_data_id;
            this.CaseId = case_id;
        }

        public int DataElementId { get; set; }
        public string Value { get; set; }
        public int UploadedDataId { get; set; }
        public int CaseId { get; set; }
        public int ElementId { get; set; }
    }
}
