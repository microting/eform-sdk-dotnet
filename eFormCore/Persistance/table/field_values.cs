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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class field_values : BaseEntity
    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int id { get; set; }

//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public int? version { get; set; }
//
//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        public DateTime? done_at { get; set; }

        public DateTime? date { get; set; }

        [ForeignKey("worker")]
        public int? user_id { get; set; }

        public int? case_id { get; set; }

        [ForeignKey("field")]
        public int? field_id { get; set; }

        [ForeignKey("check_list")]
        public int? check_list_id { get; set; }

        public int? check_list_duplicate_id { get; set; }

        [ForeignKey("uploaded_data")]
        public int? uploaded_data_id { get; set; }

        public string value { get; set; }

        [StringLength(255)]
        public string latitude { get; set; }

        [StringLength(255)]
        public string longitude { get; set; }

        [StringLength(255)]
        public string altitude { get; set; }

        [StringLength(255)]
        public string heading { get; set; }

        [StringLength(255)]
        public string accuracy { get; set; }

        public virtual workers worker { get; set; }

        public virtual fields field { get; set; }

        public virtual check_lists check_list { get; set; }

        public virtual uploaded_data uploaded_data { get; set; }


        public void Create(MicrotingDbAnySql dbContext)
        {
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.field_values.Add(this);
            dbContext.SaveChanges();

            dbContext.field_value_versions.Add(MapFieldValueVersions(this));
            dbContext.SaveChanges();

        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            field_values fieldValues = dbContext.field_values.FirstOrDefault(x => x.id == id);

            if (fieldValues == null)
            {
                throw new NullReferenceException($"Could not find Field Value with ID: {id}");
            }

            fieldValues.done_at = done_at;
            fieldValues.date = date;
            fieldValues.user_id = user_id;
            fieldValues.case_id = case_id;
            fieldValues.field_id = field_id;
            fieldValues.check_list_id = check_list_id;
            fieldValues.check_list_duplicate_id = check_list_duplicate_id;
            fieldValues.uploaded_data_id = uploaded_data_id;
            fieldValues.value = value;
            fieldValues.latitude = latitude;
            fieldValues.longitude = longitude;
            fieldValues.altitude = altitude;
            fieldValues.heading = heading;
            fieldValues.accuracy = accuracy;

            if (dbContext.ChangeTracker.HasChanges())
            {
                fieldValues.updated_at = DateTime.Now;
                fieldValues.version += 1;

                dbContext.field_value_versions.Add(MapFieldValueVersions(fieldValues));
                dbContext.SaveChanges();
            }

        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            field_values fieldValues = dbContext.field_values.FirstOrDefault(x => x.id == id);

            if (fieldValues == null)
            {
                throw new NullReferenceException($"Could not find Field Value with ID: {id}");
            }

            fieldValues.workflow_state = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                fieldValues.updated_at = DateTime.Now;
                fieldValues.version += 1;

                dbContext.field_value_versions.Add(MapFieldValueVersions(fieldValues));
                dbContext.SaveChanges();
            }
        }
        
        
        private field_value_versions MapFieldValueVersions(field_values fieldValue)
        {
            field_value_versions fvv = new field_value_versions();

            fvv.created_at = fieldValue.created_at;
            fvv.updated_at = fieldValue.updated_at;
            fvv.value = fieldValue.value;
            fvv.latitude = fieldValue.latitude;
            fvv.longitude = fieldValue.longitude;
            fvv.altitude = fieldValue.altitude;
            fvv.heading = fieldValue.heading;
            fvv.date = fieldValue.date;
            fvv.accuracy = fieldValue.accuracy;
            fvv.uploaded_data_id = fieldValue.uploaded_data_id;
            fvv.version = fieldValue.version;
            fvv.case_id = fieldValue.case_id;
            fvv.field_id = fieldValue.field_id;
            fvv.user_id = fieldValue.user_id;
            fvv.workflow_state = fieldValue.workflow_state;
            fvv.check_list_id = fieldValue.check_list_id;
            fvv.check_list_duplicate_id = fieldValue.check_list_duplicate_id;
            fvv.done_at = fieldValue.done_at;

            fvv.field_value_id = fieldValue.id; //<<--

            return fvv;
        }
    }
}
