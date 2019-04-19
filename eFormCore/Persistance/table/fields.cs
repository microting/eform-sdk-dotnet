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

using System.Linq;
using eFormShared;

namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class fields : base_entity
    {
        public fields()
        {
            this.children = new HashSet<fields>();
            this.field_values = new HashSet<field_values>();
        }
//
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int id { get; set; }
//
//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public int? version { get; set; }
//
//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        public int? parent_field_id { get; set; }

        [ForeignKey("check_list")]
        public int? check_list_id { get; set; }

        [ForeignKey("field_type")]
        public int? field_type_id { get; set; }

        public short? mandatory { get; set; }

        public short? read_only { get; set; }

        public string label { get; set; }

        public string description { get; set; }

        [StringLength(255)]
        public string color { get; set; }

        public int? display_index { get; set; }

        public short? dummy { get; set; }

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

        public string original_id { get; set; }

        public virtual field_types field_type { get; set; }

        public virtual check_lists check_list { get; set; }

        public virtual fields parent { get; set; }

        public virtual ICollection<fields> children { get; set; }

        public virtual ICollection<field_values> field_values { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.fields.Add(this);
            dbContext.SaveChanges();

            dbContext.field_versions.Add(MapFieldVersions(this));
            dbContext.SaveChanges();

        }


        public void Update(MicrotingDbAnySql dbContext)
        {
            fields field = dbContext.fields.FirstOrDefault(x => x.id == id);

            if (field == null)
            {
                throw new NullReferenceException($"Could not find Field with ID: {id}");
            }

            field.parent_field_id = parent_field_id;
            field.check_list_id = check_list_id;
            field.field_type_id = field_type_id;
            field.mandatory = mandatory;
            field.read_only = read_only;
            field.label = label;
            field.description = description;
            field.color = color;
            field.display_index = display_index;
            field.dummy = dummy;
            field.default_value = default_value;
            field.unit_name = unit_name;
            field.min_value = min_value;
            field.max_value = max_value;
            field.max_length = max_length;
            field.decimal_count = decimal_count;
            field.multi = multi;
            field.optional = optional;
            field.selected = selected;
            field.split_screen = split_screen;
            field.geolocation_forced = geolocation_forced;
            field.geolocation_hidden = geolocation_hidden;
            field.geolocation_enabled = geolocation_enabled;
            field.stop_on_save = stop_on_save;
            field.is_num = is_num;
            field.barcode_enabled = barcode_enabled;
            field.barcode_type = barcode_type;
            field.query_type = query_type;
            field.key_value_pair_list = key_value_pair_list;
            field.custom = custom;
            field.entity_group_id = entity_group_id;
            field.original_id = original_id;


            if (dbContext.ChangeTracker.HasChanges())
            {
                field.version += 1;
                field.updated_at = DateTime.Now;

                dbContext.field_versions.Add(MapFieldVersions(field));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            fields field = dbContext.fields.FirstOrDefault(x => x.id == id);

            if (field == null)
            {
                throw new NullReferenceException($"Could not find Field with ID: {id}");
            }

            field.workflow_state = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                field.version += 1;
                field.updated_at = DateTime.Now;

                dbContext.field_versions.Add(MapFieldVersions(field));
                dbContext.SaveChanges();
            }
        }
        
        
        
        private field_versions MapFieldVersions(fields field)
        {
            field_versions fv = new field_versions();

            fv.version = field.version;
            fv.created_at = field.created_at;
            fv.updated_at = field.updated_at;
            fv.custom = field.custom;
            fv.workflow_state = field.workflow_state;
            fv.check_list_id = field.check_list_id;
            fv.label = field.label;
            fv.description = field.description;
            fv.field_type_id = field.field_type_id;
            fv.display_index = field.display_index;
            fv.dummy = field.dummy;
            fv.parent_field_id = field.parent_field_id;
            fv.optional = field.optional;
            fv.multi = field.multi;
            fv.default_value = field.default_value;
            fv.selected = field.selected;
            fv.min_value = field.min_value;
            fv.max_value = field.max_value;
            fv.max_length = field.max_length;
            fv.split_screen = field.split_screen;
            fv.decimal_count = field.decimal_count;
            fv.unit_name = field.unit_name;
            fv.key_value_pair_list = field.key_value_pair_list;
            fv.geolocation_enabled = field.geolocation_enabled;
            fv.geolocation_forced = field.geolocation_forced;
            fv.geolocation_hidden = field.geolocation_hidden;
            fv.stop_on_save = field.stop_on_save;
            fv.mandatory = field.mandatory;
            fv.read_only = field.read_only;
            fv.color = field.color;
            fv.barcode_enabled = field.barcode_enabled;
            fv.barcode_type = field.barcode_type;

            fv.field_id = field.id; //<<--

            return fv;
        }

    }
}
