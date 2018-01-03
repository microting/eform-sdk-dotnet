namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class a_interaction_case_list_versions
    {
        [Key]
        public int id { get; set; }

        public int? a_interaction_case_list_version_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }


        public DateTime? created_at { get; set; }


        public DateTime? updated_at { get; set; }

        public int? siteId { get; set; }

        public string stat { get; set; }

        public string microting_uid { get; set; }

        public string check_uid { get; set; }

        public int? case_id { get; set; }

    }
}
