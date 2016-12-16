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
            EntityItemMUId = "";
            Description = "";
        }

        public EntityItem(string name, string entityItemMUId, string description)
        {
            Name = name;
            EntityItemMUId = entityItemMUId;
            Description = description;
        }

        public string Name { get; set; }
        public string EntityItemMUId { get; set; }
        public string Description { get; set; }
    }
}
