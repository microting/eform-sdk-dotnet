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
using System.Runtime.CompilerServices;

namespace eFormSqlController
{
    public partial class answers : BaseEntity
    {
        
        [ForeignKey("unit")]
        public int UnitId { get; set; }
        
        [ForeignKey("site")]
        public int SiteId { get; set; }
        
        public int AnswerDuration { get; set; }
        
        [ForeignKey("language")]
        public int LanguageId { get; set; }
        
        [ForeignKey("survey_configuration")]
        public int SurveyConfigurationId { get; set; }
        
        public int FinishedAt { get; set; }
        
        [ForeignKey("question_set")]
        public int QuestionSetId { get; set; }
        
        public bool UtcAdjusted { get; set; }
        
        public string TimeZone { get; set; }
        
        public virtual sites Site { get; set; }

        public virtual units Unit { get; set; }
        
        public virtual languages Language { get; set; }
        
        public virtual survey_configurations SurveyConfiguration { get; set; }
        
        public virtual question_sets QuestionSet { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Version = 1;
            WorkflowState = eFormShared.Constants.WorkflowStates.Created;
            dbContext.answers.Add(this);
            dbContext.SaveChanges();

            dbContext.answer_versions.Add(MapVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            answers answer = dbContext.answers.FirstOrDefault(x => x.Id == Id);

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
                answer.UpdatedAt = DateTime.Now;
                answer.Version += 1;

                dbContext.answer_versions.Add(MapVersions(answer));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            answers answer = dbContext.answers.FirstOrDefault(x => x.Id == Id);

            if (answer == null)
            {
                throw new NullReferenceException($"Could not find answer with Id: {Id}");
            }

            answer.WorkflowState = eFormShared.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                answer.UpdatedAt = DateTime.Now;
                answer.Version += 1;

                dbContext.answer_versions.Add(MapVersions(answer));
                dbContext.SaveChanges();
            }
        }

        private answer_versions MapVersions(answers answer)
        {
            answer_versions answerVersion = new answer_versions();

            answerVersion.SiteId = answer.SiteId;
            answerVersion.UnitId = answer.UnitId;
            answerVersion.AnswerId = answer.Id;
            answerVersion.TimeZone = answer.TimeZone;
            answerVersion.FinishedAt = answer.FinishedAt;
            answerVersion.languageId = answer.LanguageId;
            answerVersion.AnswerDuration = answer.AnswerDuration;
            answerVersion.QuestionSetId = answer.QuestionSetId;
            answerVersion.SurveyConfigurationId = answer.SurveyConfigurationId;
            answerVersion.UtcAdjusted = answer.UtcAdjusted;
            answerVersion.UpdatedAt = answer.UpdatedAt;
            answerVersion.CreatedAt = answer.CreatedAt;
            answerVersion.Version = answer.Version;
            answerVersion.WorkflowState = answer.WorkflowState;
            
            return answerVersion;
        }
    }
}