using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormData
{
    public class Folders
    {
        public Folders()
        {
            Name = "";
            Description = "";
            MicrotingUUID = "";
        }

        public Folders(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public Folders(string name, string description, string entityItemUId, string workflowState)
        {
            Name = name;
            Description = description;
            WorkflowState = workflowState;
        }

        public Folders(string name, string description, string workflowState, string microtingUId, int displayOrder)
        {
            Name = name;
            Description = description;
            WorkflowState = workflowState;
            MicrotingUUID = microtingUId;
            DisplayOrder = displayOrder;

        }

        public Folders(int id, string name, string description, string microtingUId)
        {
            Id = id;
            Name = name;
            Description = description;
            MicrotingUUID = microtingUId;
        }

        public Folders(int id, string name, string description, string microtingUId, string workflowState)
        {
            Id = id;
            Name = name;
            Description = description;
            WorkflowState = workflowState;
            MicrotingUUID = microtingUId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WorkflowState { get; set; }
        public string MicrotingUUID { get; set; }
        public int DisplayOrder { get; set; }
    }
}

