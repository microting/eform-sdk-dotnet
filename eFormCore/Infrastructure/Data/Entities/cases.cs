/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 Microting A/S

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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class cases : BaseEntity
    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }

//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public int? version { get; set; }

        public int? Status { get; set; }

//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }


        public DateTime? DoneAt { get; set; }

        [ForeignKey("site")]
        public int? SiteId { get; set; }

        [ForeignKey("unit")]
        public int? UnitId { get; set; }

        [ForeignKey("worker")]
        public int? WorkerId { get; set; }

        [ForeignKey("check_list")]
        public int? CheckListId { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        public int? MicrotingUid { get; set; }

        public int? MicrotingCheckUid { get; set; }

        [StringLength(255)]
        public string CaseUid { get; set; }

        public string Custom { get; set; }

        public string FieldValue1 { get; set; }

        public string FieldValue2 { get; set; }

        public string FieldValue3 { get; set; }

        public string FieldValue4 { get; set; }

        public string FieldValue5 { get; set; }

        public string FieldValue6 { get; set; }

        public string FieldValue7 { get; set; }

        public string FieldValue8 { get; set; }

        public string FieldValue9 { get; set; }

        public string FieldValue10 { get; set; }

        public virtual check_lists CheckList { get; set; }

        public virtual sites Site { get; set; }

        public virtual units Unit { get; set; }

        public virtual workers Worker { get; set; }
        
            
        public async Task Create(MicrotingDbContext dbContext) 
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.cases.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.case_versions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            cases cases = await dbContext.cases.FirstOrDefaultAsync(x => x.Id == Id);

            if (cases == null)
            {
                throw new NullReferenceException($"Could not find case with Id: {Id}");
            }

            cases.Custom = Custom;
            cases.Status = Status;
            cases.DoneAt = DoneAt;
            cases.SiteId = SiteId;
            cases.UnitId = UnitId;
            cases.CaseUid = CaseUid;
            cases.CheckList = CheckList;
            cases.CheckListId = CheckListId;
            cases.FieldValue1 = FieldValue1;
            cases.FieldValue2 = FieldValue2;
            cases.FieldValue3 = FieldValue3;
            cases.FieldValue4 = FieldValue4;
            cases.FieldValue5 = FieldValue5;
            cases.FieldValue6 = FieldValue6;
            cases.FieldValue7 = FieldValue7;
            cases.FieldValue8 = FieldValue8;
            cases.FieldValue9 = FieldValue9;
            cases.FieldValue10 = FieldValue10;
            cases.MicrotingUid = MicrotingUid;
            cases.WorkerId = WorkerId;
            cases.MicrotingCheckUid = MicrotingCheckUid;
            cases.WorkflowState = WorkflowState; // TODO extend tests to include WorkflowState

            if (dbContext.ChangeTracker.HasChanges())
            {
                cases.Version += 1;
                cases.UpdatedAt = DateTime.Now;

                dbContext.case_versions.Add(MapVersions(cases));
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            cases cases = await dbContext.cases.FirstOrDefaultAsync(x => x.Id == Id);

            if (cases == null)
            {
                throw new NullReferenceException($"Could not find case with Id: {Id}");
            }

            cases.WorkflowState = Constants.Constants.WorkflowStates.Removed;
           
            if (dbContext.ChangeTracker.HasChanges())
            {
                cases.Version += 1;
                cases.UpdatedAt = DateTime.Now;

                dbContext.case_versions.Add(MapVersions(cases));
                await dbContext.SaveChangesAsync();
            }
            
        }
        
        private case_versions MapVersions(cases aCase)
        {
            case_versions caseVer = new case_versions();
            caseVer.Status = aCase.Status;
            caseVer.DoneAt = aCase.DoneAt;
            caseVer.UpdatedAt = aCase.UpdatedAt;
            caseVer.WorkerId = aCase.WorkerId;
            caseVer.WorkflowState = aCase.WorkflowState;
            caseVer.Version = aCase.Version;
            caseVer.MicrotingCheckUid = aCase.MicrotingCheckUid;
            caseVer.UnitId = aCase.UnitId;

            caseVer.Type = aCase.Type;
            caseVer.CreatedAt = aCase.CreatedAt;
            caseVer.CheckListId = aCase.CheckListId;
            caseVer.MicrotingUid = aCase.MicrotingUid;
            caseVer.SiteId = aCase.SiteId;
            caseVer.FieldValue1 = aCase.FieldValue1;
            caseVer.FieldValue2 = aCase.FieldValue2;
            caseVer.FieldValue3 = aCase.FieldValue3;
            caseVer.FieldValue4 = aCase.FieldValue4;
            caseVer.FieldValue5 = aCase.FieldValue5;
            caseVer.FieldValue6 = aCase.FieldValue6;
            caseVer.FieldValue7 = aCase.FieldValue7;
            caseVer.FieldValue8 = aCase.FieldValue8;
            caseVer.FieldValue9 = aCase.FieldValue9;
            caseVer.FieldValue10 = aCase.FieldValue10;
            caseVer.Custom = aCase.Custom;
            caseVer.CaseUid = aCase.CaseUid;

            caseVer.CaseId = aCase.Id; //<<--

            return caseVer;
        }
    }

}