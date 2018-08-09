namespace eFormSqlController
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class worker_versions
    {
        [Key]
        public int id { get; set; }


        public DateTime? created_at { get; set; }


        public DateTime? updated_at { get; set; }

        public int microting_uid { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        [StringLength(255)]
        public string first_name { get; set; }

        [StringLength(255)]
        public string last_name { get; set; }

        [StringLength(255)]
        public string email { get; set; }

        public int? worker_id { get; set; }
    }
}
