using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspection
{
    public class PdfElement : DataElement
    {
        public PdfElement(string id, string label, string description, string path_value, bool mandatory, int color, string element_id)
        {
            Id = id;
            Label = label;
            Description = description;
            PathValue = path_value;
            Mandatory = mandatory;
            setColor(color);
            ElementId = element_id;
        }

        public string PathValue { get; set; }
    }
}
