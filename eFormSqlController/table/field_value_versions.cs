namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class field_value_versions
    {
        [Key]
        public int id { get; set; }

        public int? field_value_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        
        public DateTime? created_at { get; set; }

        
        public DateTime? updated_at { get; set; }

        
        public DateTime? done_at { get; set; }

        
        public DateTime? date { get; set; }

        public int? user_id { get; set; }

        public int? case_id { get; set; }

        public int? field_id { get; set; }

        public int? check_list_id { get; set; }

        public int? check_list_duplicate_id { get; set; }

        public int? uploaded_data_id { get; set; }

        public string value { get; set; }

        [StringLength(255)]
        public string latitude { get; set; }

        [StringLength(255)]
        public string longitude { get; set; }

        [StringLength(255)]
        public string altitude { get; set; }

        [StringLength(255)]
        public string heading { get; set; }

        [StringLength(255)]
        public string accuracy { get; set; }
    }
}
