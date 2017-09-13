namespace eFormSqlController
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class field_versions
    {
        [Key]
        public int id { get; set; }

        public int? field_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        
        public DateTime? created_at { get; set; }

        
        public DateTime? updated_at { get; set; }

        public int? parent_field_id { get; set; }

        public int? check_list_id { get; set; }

        public int? field_type_id { get; set; }

        public short? mandatory { get; set; }

        public short? read_only { get; set; }

        public string label { get; set; }

        public string description { get; set; }

        [StringLength(255)]
        public string color { get; set; }

        public int? display_index { get; set; }

        public short? dummy { get; set; }

        [StringLength(255)]
        public string default_value { get; set; }

        [StringLength(255)]
        public string unit_name { get; set; }

        [StringLength(255)]
        public string min_value { get; set; }

        [StringLength(255)]
        public string max_value { get; set; }

        public int? max_length { get; set; }

        public int? decimal_count { get; set; }

        public int? multi { get; set; }

        public short? optional { get; set; }

        public short? selected { get; set; }

        public short? split_screen { get; set; }

        public short? geolocation_enabled { get; set; }

        public short? geolocation_forced { get; set; }

        public short? geolocation_hidden { get; set; }

        public short? stop_on_save { get; set; }

        public short? is_num { get; set; }

        public short? barcode_enabled { get; set; }

        [StringLength(255)]
        public string barcode_type { get; set; }

        [StringLength(255)]
        public string query_type { get; set; }

        public string key_value_pair_list { get; set; }

        public string custom { get; set; }

        public int? entity_group_id { get; set; }
    }
}
