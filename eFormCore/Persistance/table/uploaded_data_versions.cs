namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class uploaded_data_versions
    {
        [Key]
        public int id { get; set; }

        public int? data_uploaded_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }


        public DateTime? created_at { get; set; }


        public DateTime? updated_at { get; set; }

        public int? uploader_id { get; set; }

        [StringLength(255)]
        public string checksum { get; set; }

        [StringLength(255)]
        public string extension { get; set; }

        [StringLength(255)]
        public string current_file { get; set; }

        [StringLength(255)]
        public string uploader_type { get; set; }

        [StringLength(255)]
        public string file_location { get; set; }

        [StringLength(255)]
        public string file_name { get; set; }


        public DateTime? expiration_date { get; set; }

        public short? local { get; set; }

        public int? transcription_id { get; set; }
    }
}
