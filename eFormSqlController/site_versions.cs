namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("microting.site_versions")]
    public partial class site_versions
    {
        public int id { get; set; }

        public int? site_id { get; set; }

        public int? version { get; set; }

        [StringLength(255)]
        public string microting_uuid { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }
    }
}
