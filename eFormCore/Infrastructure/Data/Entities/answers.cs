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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class answers : BaseEntity
    {
        
        [ForeignKey("unit")]
        public int? UnitId { get; set; }
        
        [ForeignKey("site")]
        public int SiteId { get; set; }
        
        public int AnswerDuration { get; set; }
        
        [ForeignKey("language")]
        public int LanguageId { get; set; }
        
        [ForeignKey("survey_configuration")]
        public int SurveyConfigurationId { get; set; }
        
        public DateTime FinishedAt { get; set; }
        
        [ForeignKey("question_set")]
        public int QuestionSetId { get; set; }
        
        public bool UtcAdjusted { get; set; }
        
        public string TimeZone { get; set; }
        
        public virtual sites Site { get; set; }

        public virtual units Unit { get; set; }
        
        public virtual languages Language { get; set; }
        
        public virtual survey_configurations SurveyConfiguration { get; set; }
        
        public virtual question_sets QuestionSet { get; set; }
        
        public int? MicrotingUid { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Version = 1;
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            dbContext.answers.Add(this);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            dbContext.answer_versions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            answers answer = await dbContext.answers.FirstOrDefaultAsync(x => x.Id == Id);

            if (answer == null)
            {
                throw new NullReferenceException($"Could not find answer with Id: {Id}");
            }

            answer.SiteId = SiteId;
            answer.UnitId = UnitId;
            answer.TimeZone = TimeZone;
            answer.FinishedAt = FinishedAt;
            answer.LanguageId = LanguageId;
            answer.AnswerDuration = AnswerDuration;
            answer.QuestionSetId = QuestionSetId;
            answer.SurveyConfigurationId = SurveyConfigurationId;
            answer.UtcAdjusted = UtcAdjusted;

            if (dbContext.ChangeTracker.HasChanges())
            {
                answer.UpdatedAt = DateTime.UtcNow;
                answer.Version += 1;

                dbContext.answer_versions.Add(MapVersions(answer));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            answers answer = await dbContext.answers.FirstOrDefaultAsync(x => x.Id == Id);

            if (answer == null)
            {
                throw new NullReferenceException($"Could not find answer with Id: {Id}");
            }

            answer.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                answer.UpdatedAt = DateTime.UtcNow;
                answer.Version += 1;

                dbContext.answer_versions.Add(MapVersions(answer));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private answer_versions MapVersions(answers answer)
        {
            return new answer_versions
            {
                SiteId = answer.SiteId,
                UnitId = answer.UnitId,
                AnswerId = answer.Id,
                TimeZone = answer.TimeZone,
                FinishedAt = answer.FinishedAt,
                LanguageId = answer.LanguageId,
                AnswerDuration = answer.AnswerDuration,
                QuestionSetId = answer.QuestionSetId,
                SurveyConfigurationId = answer.SurveyConfigurationId,
                UtcAdjusted = answer.UtcAdjusted,
                MicrotingUid = answer.MicrotingUid,
                UpdatedAt = answer.UpdatedAt,
                CreatedAt = answer.CreatedAt,
                Version = answer.Version,
                WorkflowState = answer.WorkflowState
            };
        }
    }
}