namespace eFormCustom
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class factions
    {
        public int id { get; set; }

        [Required]
        public string name { get; set; }
    }
}
