namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class a_interaction_case_versions
    {

        public a_interaction_case_versions()
        {
    
        }

        [Key]
        public int id { get; set; }

        public int? a_interaction_case_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        [Column(TypeName = "date")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "date")]
        public DateTime? updated_at { get; set; }

        public int template_id { get; set; }

        [StringLength(255)]
        public string case_uid { get; set; }

        public string custom { get; set; }

        public short? reversed { get; set; }

        public short? connected { get; set; }

        public string replacements { get; set; }

        public short? synced { get; set; }

        public string expectionString { get; set; }
    }
}
