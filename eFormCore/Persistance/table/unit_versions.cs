namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class unit_versions
    {
        [Key]
        public int id { get; set; }

        public int? microting_uid { get; set; }

        public int? otp_code { get; set; }

        public int? customer_no { get; set; }

        public int? version { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? unit_id { get; set; }


        public DateTime? created_at { get; set; }


        public DateTime? updated_at { get; set; }

        public int? site_id { get; set; }
    }
}
