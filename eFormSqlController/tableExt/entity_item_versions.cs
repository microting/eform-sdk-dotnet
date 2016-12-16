namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class entity_item_versions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int entity_items_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        [Required]
        [StringLength(50)]
        public string workflow_state { get; set; }

        public int version { get; set; }

        public int group_id { get; set; }

        public string name { get; set; }

        public string microtingUId { get; set; }

        public string description { get; set; }

        public short? synced { get; set; }
    }
}
