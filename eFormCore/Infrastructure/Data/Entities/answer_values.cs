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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class answer_values : BaseEntity
    {
        [ForeignKey("answer")]
        public int AnswerId { get; set; }
        
        [ForeignKey("question")]
        public int QuestionId { get; set; }
        
        [ForeignKey("options")]
        public int OptionsId { get; set; }
        
        public int Value { get; set; }
        
        public int? MicrotingUid { get; set; }
        
        public virtual answers Answer { get; set; }
        
        public virtual questions Question { get; set; }
        
        public virtual options Option { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.answer_values.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.answer_value_versions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            answer_values answerValue = await dbContext.answer_values.FirstOrDefaultAsync(x => x.Id == Id);

            if (answerValue == null)
            {
                throw new NullReferenceException($"Could not find answer value with Id: {Id}");
            }

            answerValue.Value = Value;
            answerValue.AnswerId = AnswerId;
            answerValue.OptionsId = OptionsId;
            answerValue.QuestionId = QuestionId;

            if (dbContext.ChangeTracker.HasChanges())
            {
                answerValue.Version += 1;
                answerValue.UpdatedAt = DateTime.Now;

                dbContext.answer_value_versions.Add(MapVersions(answerValue));
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            answer_values answerValue = await dbContext.answer_values.FirstOrDefaultAsync(x => x.Id == Id);

            if (answerValue == null)
            {
                throw new NullReferenceException($"Could not find answer value with Id: {Id}");
            }

            answerValue.WorkflowState = Constants.Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                answerValue.Version += 1;
                answerValue.UpdatedAt = DateTime.Now;

                dbContext.answer_value_versions.Add(MapVersions(answerValue));
                await dbContext.SaveChangesAsync();
            }
        }
        private answer_value_versions MapVersions(answer_values answerValue)
        {
            return new answer_value_versions
            {
                QuestionId = answerValue.QuestionId,
                Value = answerValue.Value,
                OptionsId = answerValue.OptionsId,
                AnswerId = answerValue.AnswerId,
                AnswerValueId = answerValue.Id,
                CreatedAt = answerValue.CreatedAt,
                Version = answerValue.Version,
                UpdatedAt = answerValue.UpdatedAt,
                WorkflowState = answerValue.WorkflowState,
                MicrotingUid = answerValue.MicrotingUid
            };
        }
    }
}