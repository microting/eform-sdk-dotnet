namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class a_input_cases
    {
        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }

        public string template_xml { get; set; }

        public string site_uids { get; set; }

        [StringLength(255)]
        public string case_uid { get; set; }

        public string custom { get; set; }

        public short? reversed { get; set; }

        [StringLength(255)]
        public string microting_uids { get; set; }

        public short? connected { get; set; }
    }
}
