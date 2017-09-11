namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class check_list_site_versions
    {
        [Key]
        public int id { get; set; }

        public int? check_list_site_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        [Column(TypeName = "date")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "date")]
        public DateTime? updated_at { get; set; }

        public int? site_id { get; set; }

        public int? check_list_id { get; set; }

        [StringLength(255)]
        public string microting_uid { get; set; }

        [StringLength(255)]
        public string last_check_id { get; set; }
    }
}
