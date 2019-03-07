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

    public partial class cases
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [StringLength(255)]
        public string workflow_state { get; set; }

        public int? version { get; set; }

        public int? status { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }


        public DateTime? done_at { get; set; }

        [ForeignKey("site")]
        public int? site_id { get; set; }

        [ForeignKey("unit")]
        public int? unit_id { get; set; }

        [ForeignKey("worker")]
        public int? done_by_user_id { get; set; }

        [ForeignKey("check_list")]
        public int? check_list_id { get; set; }

        [StringLength(255)]
        public string type { get; set; }

        [StringLength(255)]
        public string microting_uid { get; set; }

        [StringLength(255)]
        public string microting_check_uid { get; set; }

        [StringLength(255)]
        public string case_uid { get; set; }

        public string custom { get; set; }

        public string field_value_1 { get; set; }

        public string field_value_2 { get; set; }

        public string field_value_3 { get; set; }

        public string field_value_4 { get; set; }

        public string field_value_5 { get; set; }

        public string field_value_6 { get; set; }

        public string field_value_7 { get; set; }

        public string field_value_8 { get; set; }

        public string field_value_9 { get; set; }

        public string field_value_10 { get; set; }

        public virtual check_lists check_list { get; set; }

        public virtual sites site { get; set; }

        public virtual units unit { get; set; }

        public virtual workers worker { get; set; }
    }
}