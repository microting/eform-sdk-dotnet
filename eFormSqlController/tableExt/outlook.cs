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

        [Required]
        public string global_id { get; set; }

        public DateTime start_at { get; set; }

        public int duration { get; set; }

        [Required]
        [StringLength(255)]
        public string subject { get; set; }

        [Required]
        [StringLength(255)]
        public string location { get; set; }

        [Required]
        public string body { get; set; }

        public int templat_id { get; set; }

        [Required]
        public string site_ids { get; set; }

        public short connected { get; set; }

        [StringLength(255)]
        public string title { get; set; }

        public string info { get; set; }

        public string custom_fields { get; set; }

        public DateTime? expire_at { get; set; }

        [StringLength(50)]
        public string microting_api_id { get; set; }

        [Required]
        [StringLength(50)]
        public string workflow_state { get; set; }

        public short completed { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public int version { get; set; }
    }
}
