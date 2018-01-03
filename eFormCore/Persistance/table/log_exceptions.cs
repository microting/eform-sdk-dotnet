namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class log_exceptions
    {
        [Key]
        public int id { get; set; }

        public DateTime created_at { get; set; }

        public int level { get; set; }

        public string type { get; set; }

        public string message { get; set; }
    }
}
