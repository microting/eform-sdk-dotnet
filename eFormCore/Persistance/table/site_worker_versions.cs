namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class site_worker_versions
    {
        [Key]
        public int id { get; set; }

        public int? site_id { get; set; }

        public int? worker_id { get; set; }

        public int? microting_uid { get; set; }

        public int? version { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }


        public DateTime? created_at { get; set; }


        public DateTime? updated_at { get; set; }

        public int? site_worker_id { get; set; }
    }
}
