namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("outlook")]
    public partial class outlook
    {
        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }

        public string global_id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? start_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? expire_at { get; set; }

        public int? duration { get; set; }

        public int? templat_id { get; set; }

        [StringLength(255)]
        public string subject { get; set; }

        [StringLength(255)]
        public string location { get; set; }

        public string body { get; set; }

        public string site_ids { get; set; }

        [StringLength(255)]
        public string title { get; set; }

        public string info { get; set; }

        public string custom_fields { get; set; }

        [StringLength(255)]
        public string microting_uid { get; set; }

        public short? connected { get; set; }

        public short? completed { get; set; }
    }
}
