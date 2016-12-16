namespace eFormCustom
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class workers
    {
        public int id { get; set; }

        public int? site_id { get; set; }

        [StringLength(50)]
        public string location { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        [StringLength(50)]
        public string phone { get; set; }
    }
}
