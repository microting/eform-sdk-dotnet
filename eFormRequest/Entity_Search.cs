using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eFormRequest
{
    public class Entity_Search : DataItem
    {
        internal Entity_Search()
        {

        }

        public Entity_Search(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder,
            int sourceOfExistingList)

        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = new CDataValue();
            Description.InderValue = description;
            Color = color;
            DisplayOrder = displayOrder;

            MinSearchLenght = sourceOfExistingList;
        }

        public bool IsNum { get; set; }
        public string QueryType { get; set; }
        public int MinSearchLenght { get; set; }
        public bool BarcodeEnabled { get; set; }
        public string BarcodeType { get; set; }
        public int EntityTypeId { get; set; }
    }
}
