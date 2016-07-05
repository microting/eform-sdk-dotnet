using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace eFormRequest
{
    [Serializable()]
    public class EntityType
    {
        #region con
        internal EntityType()
        {
            Entities = new List<Entity>();
        }

        public EntityType(string name, string id, List<Entity> entities)
        {
            Name = name;
            Id = id;
            Entities = entities;
        }
        #endregion

        #region var
        public string Name { get; set; }
        public string Id { get; set; }
        public string MicrotingUUId { get; set; }

        [XmlArray("Entities"), XmlArrayItem(typeof(Entity), ElementName = "Entity")]
        public List<Entity> Entities { get; set; }
        #endregion
    }
}
