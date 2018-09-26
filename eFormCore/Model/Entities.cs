using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace eFormData
{
    #region EntityGroup
    public class EntityGroup
    {
        public EntityGroup()
        {

        }

        public EntityGroup(int id, string name, string type, string microtingUUId, List<EntityItem> entityGroupItemLst)
        {
            Id = id;
            Name = name;
            Type = type;
            MicrotingUUID = microtingUUId;
            EntityGroupItemLst = entityGroupItemLst;
        }

        public EntityGroup(int id, string name, string type, string microtingUUId, List<EntityItem> entityGroupItemLst, string workflowState, DateTime? createdAt, DateTime? updatedAt)
        {
            Id = id;
            Name = name;
            Type = type;
            MicrotingUUID = microtingUUId;
            EntityGroupItemLst = entityGroupItemLst;
            WorkflowState = workflowState;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; }
        public string Name { get; set; }
        public string Type { get; }
        public string MicrotingUUID { get; }
        public List<EntityItem> EntityGroupItemLst { get; set; }
        public string WorkflowState { get; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? CreatedAt { get; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? UpdatedAt { get; }
    }
    #endregion

    #region EntityGroupList
    public class EntityGroupList
    {
        public EntityGroupList()
        {

        }

        public EntityGroupList(int numOfElements, int pageNum, List<EntityGroup> entityGroupList)
        {
            this.NumOfElements = numOfElements;
            this.PageNum = pageNum;
            this.EntityGroups = entityGroupList;
        }

        public int NumOfElements { get; }
        public int PageNum { get; }
        public List<EntityGroup> EntityGroups { get; }
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
            MicrotingUUID = "";
        }

        public EntityItem(string name, string description, string entityItemUId)
        {
            Name = name;
            Description = description;
            EntityItemUId = entityItemUId;
        }
        public EntityItem(string name, string description, string entityItemUId, string workflowState)
        {
            Name = name;
            Description = description;
            EntityItemUId = entityItemUId;
            WorkflowState = workflowState;
        }

        public EntityItem(string name, string description, string entityItemUId, string workflowState, string microtingUId, int displayIndex)
        {
            Name = name;
            Description = description;
            EntityItemUId = entityItemUId;
            WorkflowState = workflowState;
            MicrotingUUID = microtingUId;
            DisplayIndex = displayIndex;

        }

        public EntityItem(int id, string name, string description, string entityItemUId, string microtingUId)
        {
            Id = id;
            Name = name;
            Description = description;
            EntityItemUId = entityItemUId;
            MicrotingUUID = microtingUId;
        }

        public EntityItem(int id, string name, string description, string entityItemUId, string microtingUId, string workflowState)
        {
            Id = id;
            Name = name;
            Description = description;
            EntityItemUId = entityItemUId;
            WorkflowState = workflowState;
            MicrotingUUID = microtingUId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EntityItemUId { get; set; }
        public string WorkflowState { get; set; }
        public string MicrotingUUID { get; set; }
        public int DisplayIndex { get; set; }
        public int EntityItemGroupId { get; set; }
    }
    #endregion
}