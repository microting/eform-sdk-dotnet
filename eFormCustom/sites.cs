namespace eFormCustom
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sites
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string area { get; set; }

        [Required]
        [StringLength(50)]
        public string type { get; set; }

        public int site_id { get; set; }
    }
}
