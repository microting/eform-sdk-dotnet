namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("microting.check_list_sites")]
    public partial class check_list_sites
    {
        public int id { get; set; }

        public int? check_list_id { get; set; }

        public int? site_id { get; set; }

        [StringLength(255)]
        public string microting_check_list_uuid { get; set; }

        [StringLength(255)]
        public string last_check_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime updated_at { get; set; }

        public int? version { get; set; }
    }
}
