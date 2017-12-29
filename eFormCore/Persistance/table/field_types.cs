namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class field_types
    {
        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string field_type { get; set; }

        [StringLength(255)]
        public string description { get; set; }
    }
}
