/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class check_list_values : BaseEntity
    {
        [StringLength(255)]
        public string Status { get; set; }

        public int? UserId { get; set; }

        public int? CaseId { get; set; }

        public int? CheckListId { get; set; }

        public int? CheckListDuplicateId { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.check_list_values.Add(this);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            dbContext.check_list_value_versions.Add(MapCheckListValueVersions(this));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            check_list_values clv = await dbContext.check_list_values.FirstOrDefaultAsync(x => x.Id == Id);

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
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            check_list_values clv = await dbContext.check_list_values.FirstOrDefaultAsync(x => x.Id == Id);

            if (clv == null)
            {
                throw new NullReferenceException($"Could not find Check List Value with Id: {Id}");
            }

            clv.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                clv.UpdatedAt = DateTime.Now;
                clv.Version += 1;

                dbContext.check_list_value_versions.Add(MapCheckListValueVersions(clv));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }
        
        
        private check_list_value_versions MapCheckListValueVersions(check_list_values checkListValue)
        {
            return new check_list_value_versions
            {
                Version = checkListValue.Version,
                CreatedAt = checkListValue.CreatedAt,
                UpdatedAt = checkListValue.UpdatedAt,
                CheckListId = checkListValue.CheckListId,
                CaseId = checkListValue.CaseId,
                Status = checkListValue.Status,
                UserId = checkListValue.UserId,
                WorkflowState = checkListValue.WorkflowState,
                CheckListDuplicateId = checkListValue.CheckListDuplicateId,
                CheckListValueId = checkListValue.Id
            };
        }
    }
}
