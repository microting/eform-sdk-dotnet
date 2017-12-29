namespace eFormSqlController
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class tagging_versions
    {
        [Key]
        public int id { get; set; }

        public int? tag_id { get; set; }

        public int? check_list_id { get; set; }

        public int? tagger_id { get; set; } // this will refer to some user id.

        public int? version { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public int? tagging_id { get; set; }
    }
}
