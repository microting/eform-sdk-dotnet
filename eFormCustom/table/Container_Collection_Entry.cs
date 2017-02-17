namespace eFormCustom
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Container_Collection_Entry
    {
        [Key]
        public int Collection_ID { get; set; }

        [StringLength(50)]
        public string Booking_ID { get; set; }

        public int? Worker_ID { get; set; }

        public DateTime? Order_DateTime { get; set; }

        public int? Location_ID { get; set; }

        public int? Container_ID { get; set; }

        public int? Fraction_ID { get; set; }

        [StringLength(50)]
        public string Placement_ID { get; set; }

        public int? Lorry_ID { get; set; }

        public DateTime? Booking_DateTime { get; set; }

        [StringLength(50)]
        public string Weight { get; set; }

        public DateTime? Finished_DateTime { get; set; }

        public byte? Finished_NAV { get; set; }
    }
}
