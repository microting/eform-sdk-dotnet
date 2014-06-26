using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspection
{
    public class TimerElement : DataElement
    {
        public TimerElement(string id, string label, string description, bool mandatory, int color, string element_id)
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
