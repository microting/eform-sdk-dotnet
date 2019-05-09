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
        public int unitId { get; set; }
        
        [ForeignKey("site")]
        public int siteId { get; set; }
        
        public int answerDuration { get; set; }
        
        [ForeignKey("language")]
        public int languageId { get; set; }
        
        [ForeignKey("survey_configuration")]
        public int surveyConfigurationId { get; set; }
        
        public int finishedAt { get; set; }
        
        [ForeignKey("question_set")]
        public int questionSetId { get; set; }
        
        public bool UTCAdjusted { get; set; }
        
        public string timeZone { get; set; }
        
        public virtual sites site { get; set; }

        public virtual units unit { get; set; }
        
        public virtual languages language { get; set; }
        
        public virtual survey_configurations survey_configuration { get; set; }
        
        public virtual question_sets question_set { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
            version = 1;
            workflow_state = eFormShared.Constants.WorkflowStates.Created;
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

            answer.siteId = siteId;
            answer.unitId = unitId;
            answer.timeZone = timeZone;
            answer.finishedAt = finishedAt;
            answer.languageId = languageId;
            answer.answerDuration = answerDuration;
            answer.questionSetId = questionSetId;
            answer.surveyConfigurationId = surveyConfigurationId;
            answer.UTCAdjusted = UTCAdjusted;

            if (dbContext.ChangeTracker.HasChanges())
            {
                answer.updated_at = DateTime.Now;
                answer.version += 1;

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

            answer.workflow_state = eFormShared.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                answer.updated_at = DateTime.Now;
                answer.version += 1;

                dbContext.answer_versions.Add(MapVersions(answer));
                dbContext.SaveChanges();
            }
        }

        private answer_versions MapVersions(answers answer)
        {
            answer_versions answerVersion = new answer_versions();

            answerVersion.siteId = answer.siteId;
            answerVersion.unitId = answer.unitId;
            answerVersion.answerId = answer.Id;
            answerVersion.timeZone = answer.timeZone;
            answerVersion.finishedAt = answer.finishedAt;
            answerVersion.languageId = answer.languageId;
            answerVersion.answerDuration = answer.answerDuration;
            answerVersion.questionSetId = answer.questionSetId;
            answerVersion.surveyConfigurationId = answer.surveyConfigurationId;
            answerVersion.UTCAdjusted = answer.UTCAdjusted;
            answerVersion.updated_at = answer.updated_at;
            answerVersion.created_at = answer.created_at;
            answerVersion.version = answer.version;
            answerVersion.workflow_state = answer.workflow_state;
            
            return answerVersion;
        }
    }
}