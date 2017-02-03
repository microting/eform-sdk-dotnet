namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class version_sites
    {
        public int id { get; set; }

        public int? site_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }

        public int? site_uid { get; set; }

        [StringLength(255)]
        public string site_name { get; set; }

        public int? customer_number { get; set; }

        public int? otp_code { get; set; }

        public int? unit_id { get; set; }

        public int? user_id { get; set; }

        [StringLength(255)]
        public string user_first_name { get; set; }

        [StringLength(255)]
        public string user_last_name { get; set; }

        public int? worker_id { get; set; }
    }
}
