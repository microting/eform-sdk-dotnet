namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("microting.notifications")]
    public partial class notifications
    {
        public int id { get; set; }

        [StringLength(255)]
        public string microting_uuid { get; set; }

        [StringLength(255)]
        public string content { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }
    }
}
