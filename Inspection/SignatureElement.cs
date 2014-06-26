using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspection
{
    public class SignatureElement : DataElement
    {
        public SignatureElement(string id, string label, string description, bool mandatory, int color, string element_id)
        {
            Id = id;
            Label = label;
            Description = description;
            Mandatory = mandatory;
            setColor(color);
            ElementId = element_id;
        }
    }
}
