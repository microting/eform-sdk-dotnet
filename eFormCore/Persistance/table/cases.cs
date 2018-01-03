namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cases
    {
        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        public int? status { get; set; }


        public DateTime? created_at { get; set; }


        public DateTime? updated_at { get; set; }


        public DateTime? done_at { get; set; }

        [ForeignKey("site")]
        public int? site_id { get; set; }

        [ForeignKey("unit")]
        public int? unit_id { get; set; }

        [ForeignKey("worker")]
        public int? done_by_user_id { get; set; }

        [ForeignKey("check_list")]
        public int? check_list_id { get; set; }

        [StringLength(255)]
        public string type { get; set; }

        [StringLength(255)]
        public string microting_uid { get; set; }

        [StringLength(255)]
        public string microting_check_uid { get; set; }

        [StringLength(255)]
        public string case_uid { get; set; }

        public string custom { get; set; }

        public string field_value_1 { get; set; }

        public string field_value_2 { get; set; }

        public string field_value_3 { get; set; }

        public string field_value_4 { get; set; }

        public string field_value_5 { get; set; }

        public string field_value_6 { get; set; }

        public string field_value_7 { get; set; }

        public string field_value_8 { get; set; }

        public string field_value_9 { get; set; }

        public string field_value_10 { get; set; }

        public virtual check_lists check_list { get; set; }

        public virtual sites site { get; set; }

        public virtual units unit { get; set; }

        public virtual workers worker { get; set; }
    }
}