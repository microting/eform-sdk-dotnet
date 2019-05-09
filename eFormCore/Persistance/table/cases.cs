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
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using eFormShared;
namespace eFormSqlController
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cases : BaseEntity
    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int id { get; set; }

//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public int? version { get; set; }

        public int? status { get; set; }

//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }


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
        
            
        public void Create(MicrotingDbAnySql dbContext) 
        {
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.cases.Add(this);
            dbContext.SaveChanges();

            dbContext.case_versions.Add(MapCaseVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            cases cases = dbContext.cases.FirstOrDefault(x => x.id == id);

            if (cases == null)
            {
                throw new NullReferenceException($"Could not find case with ID: {id}");
            }

            cases.custom = custom;
            cases.status = status;
            cases.done_at = done_at;
            cases.site_id = site_id;
            cases.unit_id = unit_id;
            cases.case_uid = case_uid;
            cases.check_list = check_list;
            cases.check_list_id = check_list_id;
            cases.field_value_1 = field_value_1;
            cases.field_value_2 = field_value_2;
            cases.field_value_3 = field_value_3;
            cases.field_value_4 = field_value_4;
            cases.field_value_5 = field_value_5;
            cases.field_value_6 = field_value_6;
            cases.field_value_7 = field_value_7;
            cases.field_value_8 = field_value_8;
            cases.field_value_9 = field_value_9;
            cases.field_value_10 = field_value_10;
            cases.microting_uid = microting_uid;
            cases.done_by_user_id = done_by_user_id;
            cases.microting_check_uid = microting_check_uid;

            if (dbContext.ChangeTracker.HasChanges())
            {
                cases.version += 1;
                cases.updated_at = DateTime.Now;

                dbContext.case_versions.Add(MapCaseVersions(cases));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            cases cases = dbContext.cases.FirstOrDefault(x => x.id == id);

            if (cases == null)
            {
                throw new NullReferenceException($"Could not find case with ID: {id}");
            }

            cases.workflow_state = Constants.WorkflowStates.Removed;
           
            if (dbContext.ChangeTracker.HasChanges())
            {
                cases.version += 1;
                cases.updated_at = DateTime.Now;

                dbContext.case_versions.Add(MapCaseVersions(cases));
                dbContext.SaveChanges();
            }
            
        }
        
        
        
        
        
        private case_versions MapCaseVersions(cases aCase)
        {
            case_versions caseVer = new case_versions();
            caseVer.status = aCase.status;
            caseVer.done_at = aCase.done_at;
            caseVer.updated_at = aCase.updated_at;
            caseVer.done_by_user_id = aCase.done_by_user_id;
            caseVer.workflow_state = aCase.workflow_state;
            caseVer.version = aCase.version;
            caseVer.microting_check_uid = aCase.microting_check_uid;
            caseVer.unit_id = aCase.unit_id;

            caseVer.type = aCase.type;
            caseVer.created_at = aCase.created_at;
            caseVer.check_list_id = aCase.check_list_id;
            caseVer.microting_uid = aCase.microting_uid;
            caseVer.site_id = aCase.site_id;
            caseVer.field_value_1 = aCase.field_value_1;
            caseVer.field_value_2 = aCase.field_value_2;
            caseVer.field_value_3 = aCase.field_value_3;
            caseVer.field_value_4 = aCase.field_value_4;
            caseVer.field_value_5 = aCase.field_value_5;
            caseVer.field_value_6 = aCase.field_value_6;
            caseVer.field_value_7 = aCase.field_value_7;
            caseVer.field_value_8 = aCase.field_value_8;
            caseVer.field_value_9 = aCase.field_value_9;
            caseVer.field_value_10 = aCase.field_value_10;

            caseVer.case_id = aCase.id; //<<--

            return caseVer;
        }
    }

}