namespace eFormSqlController
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class logs
    {
        [Key]
        public int id { get; set; }


        public DateTime created_at { get; set; }

        public int level { get; set; }

        public string type { get; set; }

        public string message { get; set; }
    }
}
