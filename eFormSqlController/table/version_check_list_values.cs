namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class version_check_list_values
    {
        [Key]
        public int id { get; set; }

        public int? check_list_value_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        [StringLength(255)]
        public string status { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }

        public int? user_id { get; set; }

        public int? case_id { get; set; }

        public int? check_list_id { get; set; }

        public int? check_list_duplicate_id { get; set; }
    }
}
