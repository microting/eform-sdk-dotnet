namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class entity_groups
    {
        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        
        public DateTime? created_at { get; set; }

        
        public DateTime? updated_at { get; set; }

        public string microting_uid { get; set; }

        public string name { get; set; }

        [StringLength(50)]
        public string type { get; set; }
    }
}
