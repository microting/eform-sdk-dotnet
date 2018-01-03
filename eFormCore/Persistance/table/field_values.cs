namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class field_values
    {
        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }


        public DateTime? created_at { get; set; }


        public DateTime? updated_at { get; set; }


        public DateTime? done_at { get; set; }


        public DateTime? date { get; set; }

        [ForeignKey("worker")]
        public int? user_id { get; set; }

        public int? case_id { get; set; }

        [ForeignKey("field")]
        public int? field_id { get; set; }

        [ForeignKey("check_list")]
        public int? check_list_id { get; set; }

        public int? check_list_duplicate_id { get; set; }

        [ForeignKey("uploaded_data")]
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

        public virtual workers worker { get; set; }

        public virtual fields field { get; set; }

        public virtual check_lists check_list { get; set; }

        public virtual uploaded_data uploaded_data { get; set; }
    }
}
