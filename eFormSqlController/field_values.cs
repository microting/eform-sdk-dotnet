namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("microting.field_values")]
    public partial class field_values
    {
        public int id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime updated_at { get; set; }

        [StringLength(255)]
        public string value { get; set; }

        [StringLength(255)]
        public string latitude { get; set; }

        [StringLength(255)]
        public string longitude { get; set; }

        [StringLength(255)]
        public string altitude { get; set; }

        [StringLength(255)]
        public string heading { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? date { get; set; }

        [StringLength(255)]
        public string accuracy { get; set; }

        public int? uploaded_data_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        public int? case_id { get; set; }

        public int? field_id { get; set; }

        public int? user_id { get; set; }

        public int? check_list_id { get; set; }

        public int? check_list_duplicate_id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? date_of_doing { get; set; }
    }
}
