namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class version_entity_items
    {
        public int id { get; set; }

        public int entity_items_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }

        public int? entity_group_id { get; set; }

        public string name { get; set; }

        public string microting_uid { get; set; }

        public string description { get; set; }

        public short? synced { get; set; }
    }
}
