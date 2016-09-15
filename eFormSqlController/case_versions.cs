namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("microting.case_versions")]
    public partial class case_versions
    {
        public int id { get; set; }

        public int? case_id { get; set; }

        public int? version { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? date_of_doing { get; set; }

        public int? well_id { get; set; }

        public int? machine_id { get; set; }

        public int? status { get; set; }

        public int? created_by_user_id { get; set; }

        public int? done_by_user_id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }

        public int? check_list_id { get; set; }

        [StringLength(255)]
        public string microting_api_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        [StringLength(255)]
        public string versioned_type { get; set; }

        [StringLength(255)]
        public string microting_check_id { get; set; }

        public int? site_id { get; set; }

        public int? dumped { get; set; }

        public int? unit_id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? navision_time { get; set; }

        [StringLength(255)]
        public string reg_number { get; set; }

        public string serialized_values { get; set; }

        public short? reports_error { get; set; }

        public short? contains_note { get; set; }

        [StringLength(255)]
        public string vejenummer { get; set; }

        [StringLength(255)]
        public string reference_table_model { get; set; }

        public int? reference_table_id { get; set; }

        public int? removed_by_user_id { get; set; }

        public short? has_extra_report { get; set; }
    }
}
