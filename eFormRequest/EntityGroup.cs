using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eFormRequest
{
    public class EntityGroup
    {
        public EntityGroup()
        {

        }

        public EntityGroup(int id, string name, string type, string entityGroupMUId, List<EntityItem> entityGroupItemLst)
        {
            Id = id;
            Name = name;
            Type = type;
            EntityGroupMUId = entityGroupMUId;
            EntityGroupItemLst = entityGroupItemLst;
        }

        public int Id { get; set; }
        public string Name { get; }
        public string Type { get; }
        public string EntityGroupMUId { get; }
        public List<EntityItem> EntityGroupItemLst { get; set; }
    }
}
