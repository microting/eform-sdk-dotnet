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

    public partial class check_list_values : BaseEntity
    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }
//
//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public int? version { get; set; }

        [StringLength(255)]
        public string Status { get; set; }

//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        public int? UserId { get; set; }

        public int? CaseId { get; set; }

        public int? CheckListId { get; set; }

        public int? CheckListDuplicateId { get; set; }


        public void Create(MicrotingDbAnySql dbContext)
        {
            
            WorkflowState = Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.check_list_values.Add(this);
            dbContext.SaveChanges();

            dbContext.check_list_value_versions.Add(MapCheckListValueVersions(this));
            dbContext.SaveChanges();

        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            check_list_values clv = dbContext.check_list_values.FirstOrDefault(x => x.Id == Id);

            if (clv == null)
            {
                throw new NullReferenceException($"Could not find Check List Value with Id: {Id}");
            }

            clv.Status = Status;
            clv.UserId = UserId;
            clv.CaseId = CaseId;
            clv.CheckListId = CheckListId;
            clv.CheckListDuplicateId = CheckListDuplicateId;


            if (dbContext.ChangeTracker.HasChanges())
            {
                clv.UpdatedAt = DateTime.Now;
                clv.Version += 1;

                dbContext.check_list_value_versions.Add(MapCheckListValueVersions(clv));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            check_list_values clv = dbContext.check_list_values.FirstOrDefault(x => x.Id == Id);

            if (clv == null)
            {
                throw new NullReferenceException($"Could not find Check List Value with Id: {Id}");
            }

            clv.WorkflowState = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                clv.UpdatedAt = DateTime.Now;
                clv.Version += 1;

                dbContext.check_list_value_versions.Add(MapCheckListValueVersions(clv));
                dbContext.SaveChanges();
            }
        }
        
        
        private check_list_value_versions MapCheckListValueVersions(check_list_values checkListValue)
        {
            check_list_value_versions clvv = new check_list_value_versions();
            clvv.Version = checkListValue.Version;
            clvv.CreatedAt = checkListValue.CreatedAt;
            clvv.UpdatedAt = checkListValue.UpdatedAt;
            clvv.CheckListId = checkListValue.CheckListId;
            clvv.CaseId = checkListValue.CaseId;
            clvv.Status = checkListValue.Status;
            clvv.UserId = checkListValue.UserId;
            clvv.WorkflowState = checkListValue.WorkflowState;
            clvv.CheckListDuplicateId = checkListValue.CheckListDuplicateId;

            clvv.CheckListValueId = checkListValue.Id; //<<--

            return clvv;
        }
    }
}
