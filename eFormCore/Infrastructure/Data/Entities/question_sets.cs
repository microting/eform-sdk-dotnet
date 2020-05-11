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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class question_sets : BaseEntity
    {
        public string Name { get; set; }
        
        public bool HasChild { get; set; }
        
        public bool PosiblyDeployed { get; set; }
        
        public int ParentId { get; set; }
        
        public bool Share { get; set; }
        
        public int? MicrotingUid { get; set; }
        
        public virtual ICollection<language_question_sets> LanguageQuestionSetses { get; set; }
        
        public virtual ICollection<questions> Questions { get; set; }
        
        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Version = 1;

            dbContext.question_sets.Add(this);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            dbContext.question_set_versions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            question_sets questionSet = await dbContext.question_sets.FirstOrDefaultAsync(x => x.Id == Id);

            if (questionSet == null)
            {
                throw new NullReferenceException($"Could not find question set with Id: {Id}");
            }

            questionSet.Name = Name;
            questionSet.Share = Share;
            questionSet.HasChild = HasChild;
            questionSet.PosiblyDeployed = PosiblyDeployed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                UpdatedAt = DateTime.UtcNow;
                Version += 1;

                dbContext.question_set_versions.Add(MapVersions(questionSet));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
              
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            question_sets questionSet = await dbContext.question_sets.FirstOrDefaultAsync(x => x.Id == Id);

            if (questionSet == null)
            {
                throw new NullReferenceException($"Could not find question set with Id: {Id}");
            }

            questionSet.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                UpdatedAt = DateTime.UtcNow;
                Version += 1;

                dbContext.question_set_versions.Add(MapVersions(questionSet));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
              
            }
        }
        
        private question_set_versions MapVersions(question_sets questionSet)
        {
            return new question_set_versions
            {
                QuestionSetId = questionSet.Id,
                Name = questionSet.Name,
                Share = questionSet.Share,
                HasChild = questionSet.HasChild,
                PossiblyDeployed = questionSet.PosiblyDeployed,
                Version = questionSet.Version,
                CreatedAt = questionSet.CreatedAt,
                UpdatedAt = questionSet.UpdatedAt,
                WorkflowState = questionSet.WorkflowState,
                MicrotingUid = questionSet.MicrotingUid
            };
        }
    }
}