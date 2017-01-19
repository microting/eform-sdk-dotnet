using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace eFormRequest
{
    #region EntityGroup
    public class EntityGroup
    {
        public EntityGroup()
        {

        }

        public EntityGroup(string name, string type, string entityGroupMUId, List<EntityItem> entityGroupItemLst)
        {
            Name = name;
            Type = type;
            EntityGroupMUId = entityGroupMUId;
            EntityGroupItemLst = entityGroupItemLst;
        }

        public string Name { get; }
        public string Type { get; }
        public string EntityGroupMUId { get; }
        public List<EntityItem> EntityGroupItemLst { get; set; }
    }
    #endregion

    #region EntityItem
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
    #endregion
}