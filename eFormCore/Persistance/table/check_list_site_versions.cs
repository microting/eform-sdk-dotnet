namespace eFormSqlController
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class check_list_site_versions
    {
        [Key]
        public int id { get; set; }

        public int? check_list_site_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }


        public DateTime? created_at { get; set; }


        public DateTime? updated_at { get; set; }

        public int? site_id { get; set; }

        public int? check_list_id { get; set; }

        [StringLength(255)]
        public string microting_uid { get; set; }

        [StringLength(255)]
        public string last_check_id { get; set; }
    }
}
