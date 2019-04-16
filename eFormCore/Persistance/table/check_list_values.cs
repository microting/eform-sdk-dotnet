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

    public partial class check_list_values : base_entity
    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int id { get; set; }
//
//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public int? version { get; set; }

        [StringLength(255)]
        public string status { get; set; }

//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        public int? user_id { get; set; }

        public int? case_id { get; set; }

        public int? check_list_id { get; set; }

        public int? check_list_duplicate_id { get; set; }


        public void Create(MicrotingDbAnySql dbContext)
        {
            
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.check_list_values.Add(this);
            dbContext.SaveChanges();

            dbContext.check_list_value_versions.Add(MapCheckListValueVersions(this));
            dbContext.SaveChanges();

        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            check_list_values clv = dbContext.check_list_values.FirstOrDefault(x => x.id == id);

            if (clv == null)
            {
                throw new NullReferenceException($"Could not find Check List Value with ID: {id}");
            }

            clv.status = status;
            clv.user_id = user_id;
            clv.case_id = case_id;
            clv.check_list_id = check_list_id;
            clv.check_list_duplicate_id = check_list_duplicate_id;


            if (dbContext.ChangeTracker.HasChanges())
            {
                clv.updated_at = DateTime.Now;
                clv.version += 1;

                dbContext.check_list_value_versions.Add(MapCheckListValueVersions(clv));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            check_list_values clv = dbContext.check_list_values.FirstOrDefault(x => x.id == id);

            if (clv == null)
            {
                throw new NullReferenceException($"Could not find Check List Value with ID: {id}");
            }

            clv.workflow_state = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                clv.updated_at = DateTime.Now;
                clv.version += 1;

                dbContext.check_list_value_versions.Add(MapCheckListValueVersions(clv));
                dbContext.SaveChanges();
            }
        }
        
        
        private check_list_value_versions MapCheckListValueVersions(check_list_values checkListValue)
        {
            check_list_value_versions clvv = new check_list_value_versions();
            clvv.version = checkListValue.version;
            clvv.created_at = checkListValue.created_at;
            clvv.updated_at = checkListValue.updated_at;
            clvv.check_list_id = checkListValue.check_list_id;
            clvv.case_id = checkListValue.case_id;
            clvv.status = checkListValue.status;
            clvv.user_id = checkListValue.user_id;
            clvv.workflow_state = checkListValue.workflow_state;
            clvv.check_list_duplicate_id = checkListValue.check_list_duplicate_id;

            clvv.check_list_value_id = checkListValue.id; //<<--

            return clvv;
        }
    }
}
