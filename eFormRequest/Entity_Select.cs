using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace eFormRequest
{
    [Serializable()]
    public class Entity_Select : DataItem
    {
        #region con
        internal Entity_Select()
        {

        }

        public Entity_Select(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, 
            int sourceOfExistingList)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            Color = color;
            DisplayOrder = displayOrder;
            
            Source = sourceOfExistingList;
        }

        public Entity_Select(string id, bool mandatory, bool readOnly, string label, string description, string color, int displayOrder, 
            EntityType CreateUpdateEntityType)
        {
            Id = id;
            Mandatory = mandatory;
            ReadOnly = readOnly;
            Label = label;
            Description = description;
            Color = color;
            DisplayOrder = displayOrder;
        
            EntityTypeData = CreateUpdateEntityType;
        }
        #endregion

        #region var
        public int Source { get; set; }
        public EntityType EntityTypeData { get; set; }
        #endregion
    }
}
