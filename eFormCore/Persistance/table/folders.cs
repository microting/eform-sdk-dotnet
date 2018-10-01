
namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class folders
    {
        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int version { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public string microting_uuid { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public int parent_id { get; set; }

        public int display_order { get; set; }

        public short? update_status { get; set; }

        public short? no_click { get; set; }



    }
}
