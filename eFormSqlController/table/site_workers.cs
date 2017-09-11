namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class site_workers
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("site")]
        public int? site_id { get; set; }

        [ForeignKey("worker")]
        public int? worker_id { get; set; }

        public int? microting_uid { get; set; }

        public int? version { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        [Column(TypeName = "date")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "date")]
        public DateTime? updated_at { get; set; }

        public virtual sites site { get; set; }

        public virtual workers worker { get; set; }
    }
}
