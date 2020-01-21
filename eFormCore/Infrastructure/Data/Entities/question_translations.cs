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
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class question_translations : BaseEntity
    {
        [ForeignKey("question")]
        public int QuestionId { get; set; }
        
        [ForeignKey("language")]
        public int LanguageId { get; set; }
        
        public string Name { get; set; }
        
        public virtual questions Question { get; set; }
        
        public virtual languages Language { get; set; }
        
        public int? MicrotingUid { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            dbContext.QuestionTranslations.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.QuestionTranslationVersions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync();
        }
        public async Task Update(MicrotingDbContext dbContext)
        {
            question_translations questionTranslation = 
                await dbContext.QuestionTranslations.SingleOrDefaultAsync(x => x.Id == Id);

            if (questionTranslation == null)
            {
                throw new NullReferenceException($"Could not find question_translation with id {Id}");
            }

            questionTranslation.LanguageId = LanguageId;
            questionTranslation.QuestionId = QuestionId;
            questionTranslation.Name = Name;
            questionTranslation.WorkflowState = WorkflowState;
            questionTranslation.MicrotingUid = MicrotingUid;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                questionTranslation.UpdatedAt = DateTime.UtcNow;
                questionTranslation.Version += 1;

                dbContext.QuestionTranslationVersions.Add(MapVersions(questionTranslation));
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task Delete(MicrotingDbContext dbContext)
        {
            question_translations questionTranslation = 
                await dbContext.QuestionTranslations.SingleOrDefaultAsync(x => x.Id == Id);

            if (questionTranslation == null)
            {
                throw new NullReferenceException($"Could not find question_translation with id {Id}");
            }
            
            questionTranslation.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                questionTranslation.UpdatedAt = DateTime.UtcNow;
                questionTranslation.Version += 1;

                dbContext.QuestionTranslationVersions.Add(MapVersions(questionTranslation));
                await dbContext.SaveChangesAsync();
            }
        }

        private question_translation_versions MapVersions(question_translations questionTranslations)
        {
            return new question_translation_versions()
            {
                CreatedAt = questionTranslations.CreatedAt,
                UpdatedAt = questionTranslations.UpdatedAt,
                Version = questionTranslations.Version,
                WorkflowState = questionTranslations.WorkflowState,
                MicrotingUid = questionTranslations.MicrotingUid,
                LanguageId = questionTranslations.LanguageId,
                QuestionId = questionTranslations.QuestionId,
                Name = questionTranslations.Name,
                QuestionTranslationId = questionTranslations.Id
            };
        }
    }
}