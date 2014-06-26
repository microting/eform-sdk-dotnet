using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspection
{
    public class DataElementTypes
    {
        public DataElementTypes(string type, int id, string description)
        {
            this.Type = type;
            this.Id = id;
            this.Description = description;
        }

        public string Type { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
