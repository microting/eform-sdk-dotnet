namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class field_types
    {
        public int id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime updated_at { get; set; }

        [StringLength(255)]
        public string field_type { get; set; }

        [StringLength(255)]
        public string description { get; set; }

        public string serialized_properties { get; set; }
    }
}
