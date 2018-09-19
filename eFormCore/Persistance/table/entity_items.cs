namespace eFormSqlController
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class entity_items
    {
        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }
        
        // TODO! Change this to be int and create migration to handle the move.
        public int entity_group_id { get; set; }

        [StringLength(50)]
        public string entity_item_uid { get; set; }

        public string microting_uid { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public short? synced { get; set; }

        public int display_index { get; set; }

        public bool migrated_entity_group_id { get; set; }
    }
}