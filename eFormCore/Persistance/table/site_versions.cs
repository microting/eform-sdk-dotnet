namespace eFormSqlController
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class site_versions
    {
        [Key]
        public int id { get; set; }


        public DateTime? created_at { get; set; }


        public DateTime? updated_at { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        public int? microting_uid { get; set; }

        public int? version { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? site_id { get; set; }
    }
}
