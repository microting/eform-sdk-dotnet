using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormData
{
    public class Folder
    {
        public Folder()
        {
            Name = "";
            Description = "";
            MicrotingUUID = "";
        }

        public Folder(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public Folder(string name, string description, string workflowState)
        {
            Name = name;
            Description = description;
            WorkflowState = workflowState;
        }

        public Folder(string name, string description, string workflowState, string microtingUUId, int displayOrder)
        {
            Name = name;
            Description = description;
            WorkflowState = workflowState;
            MicrotingUUID = microtingUUId;
            DisplayOrder = displayOrder;

        }

        public Folder(int id, string name, string description, string microtingUUId)
        {
            Id = id;
            Name = name;
            Description = description;
            MicrotingUUID = microtingUUId;
        }

        public Folder(int id, string name, string description, string microtingUUId, string workflowState, int parentId, int displayOrder)
        {
            Id = id;
            Name = name;
            Description = description;
            WorkflowState = workflowState;
            MicrotingUUID = microtingUUId;
            ParentId = parentId;
            DisplayOrder = displayOrder;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WorkflowState { get; set; }
        public string MicrotingUUID { get; set; }
        public int DisplayOrder { get; set; }
        public int ParentId { get; set; }
    }
}

