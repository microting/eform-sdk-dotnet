namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class notifications
    {
        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }

        [StringLength(255)]
        public string microting_uid { get; set; }

        public string transmission { get; set; }
    }
}
