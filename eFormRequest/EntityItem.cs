using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eFormRequest
{
    public class EntityItem
    {
        public EntityItem()
        {
            Name = "";
            Description = "";
            EntityItemUId = "";
        }

        public EntityItem(string name, string description, string entityItemUId)
        {
            Name = name;
            Description = description;
            EntityItemUId = entityItemUId;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string EntityItemUId { get; set; }
    }
}
