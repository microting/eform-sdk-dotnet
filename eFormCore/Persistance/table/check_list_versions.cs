/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 microting

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace eFormSqlController
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class check_list_versions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int? check_list_id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public string label { get; set; }

        public string description { get; set; }

        public string custom { get; set; }

        public int? parent_id { get; set; }

        public int? repeated { get; set; }

        public int? display_index { get; set; }

        [StringLength(255)]
        public string case_type { get; set; }

        [StringLength(255)]
        public string folder_name { get; set; }

        public short? review_enabled { get; set; }

        public short? manual_sync { get; set; }

        public short? extra_fields_enabled { get; set; }

        public short? done_button_enabled { get; set; }

        public short? approval_enabled { get; set; }

        public short? multi_approval { get; set; }

        public short? fast_navigation { get; set; }

        public short? download_entities { get; set; }

        public int? field_1 { get; set; }

        public int? field_2 { get; set; }

        public int? field_3 { get; set; }

        public int? field_4 { get; set; }

        public int? field_5 { get; set; }

        public int? field_6 { get; set; }

        public int? field_7 { get; set; }

        public int? field_8 { get; set; }

        public int? field_9 { get; set; }

        public int? field_10 { get; set; }

        public short? quick_sync_enabled { get; set; }

        public string original_id { get; set; }
    }
}