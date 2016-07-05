using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eFormRequest
{
    public class Entity_Search : DataItem
    {
        //TODO

        internal Entity_Search()
        {

        }

        public Entity_Search(string id, bool mandatory, bool readOnly, string label, string description, DataItemColors color, int displayOrder,
            bool isNum, string queryType, int minSearchLenght, bool barcodeEnabled, string barcodeType, int entityTypeId)

        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            SetColor(color);
            DisplayOrder = displayOrder;

            IsNum = isNum;
            QueryType = queryType;
            MinSearchLenght = minSearchLenght;
            BarcodeEnabled = barcodeEnabled;
            BarcodeType = barcodeType;
            EntityTypeId = entityTypeId;
        }

        public bool IsNum { get; set; }
        public string QueryType { get; set; }
        public int MinSearchLenght { get; set; }
        public bool BarcodeEnabled { get; set; }
        public string BarcodeType { get; set; }
        public int EntityTypeId { get; set; }
    }
}
