namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("microting.users")]
    public partial class users
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string email { get; set; }

        [Required]
        [StringLength(255)]
        public string encrypted_password { get; set; }

        [StringLength(255)]
        public string reset_password_token { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? reset_password_sent_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? remember_created_at { get; set; }

        public int? sign_in_count { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? current_sign_in_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? last_sign_in_at { get; set; }

        [StringLength(255)]
        public string current_sign_in_ip { get; set; }

        [StringLength(255)]
        public string last_sign_in_ip { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime updated_at { get; set; }

        [StringLength(255)]
        public string first_name { get; set; }

        [StringLength(255)]
        public string middle_name { get; set; }

        [StringLength(255)]
        public string last_name { get; set; }

        public int? version { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        [StringLength(255)]
        public string microting_uuid { get; set; }

        public short? admin { get; set; }
    }
}
