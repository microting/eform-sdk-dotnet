namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class a_interaction_cases
    {
        public a_interaction_cases()
        {
            this.a_interaction_case_lists = new HashSet<a_interaction_case_lists>();
        }

        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public int template_id { get; set; }

        [StringLength(255)]
        public string case_uid { get; set; }

        public string custom { get; set; }

        public short? connected { get; set; }

        public string replacements { get; set; }

        public short? synced { get; set; }

        public string expectionString { get; set; }

        public virtual ICollection<a_interaction_case_lists> a_interaction_case_lists { get; set; }
    }
}