namespace eFormSqlController
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class notifications
    {
        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }


        public DateTime? created_at { get; set; }


        public DateTime? updated_at { get; set; }

        [StringLength(255)]
        public string microting_uid { get; set; }

        public string transmission { get; set; }

        [StringLength(255)]
        public string notification_uid { get; set; }

        public string activity { get; set; }

        public string exception { get; set; }

        public string stacktrace { get; set; }
    }
}
