namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sites
    {
        public sites()
        {
            this.cases = new HashSet<cases>();
            this.units = new HashSet<units>();
            this.site_workers = new HashSet<site_workers>();
            this.check_list_sites = new HashSet<check_list_sites>();
        }

        [Key]
        public int id { get; set; }


        public DateTime? created_at { get; set; }


        public DateTime? updated_at { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        public int? microting_uid { get; set; }

        public int? version { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public virtual ICollection<cases> cases { get; set; }

        public virtual ICollection<units> units { get; set; }

        public virtual ICollection<site_workers> site_workers { get; set; }

        public virtual ICollection<check_list_sites> check_list_sites { get; set; }
    }
}
