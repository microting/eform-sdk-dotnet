using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspection
{
    public class Sites
    {
        public Sites(int id, string name, string microting_uuid)
        {
            this.Id = id;
            this.Name = name;
            this.MicrotingUuid = microting_uuid;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string MicrotingUuid { get; set; }
    }
}
