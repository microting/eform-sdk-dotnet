namespace eFormSqlController
{
    using System.ComponentModel.DataAnnotations;

    public partial class field_types
    {
        [Key]
        public int id { get; set; }

        [StringLength(255)]
        public string field_type { get; set; }

        [StringLength(255)]
        public string description { get; set; }
    }
}
