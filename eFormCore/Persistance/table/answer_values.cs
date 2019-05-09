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
using Rebus.Topic;

namespace eFormSqlController
{
    public partial class answer_values : BaseEntity
    {
        [ForeignKey("answer")]
        public int answerId { get; set; }
        
        [ForeignKey("question")]
        public int questionId { get; set; }
        
        [ForeignKey("options")]
        public int optionsId { get; set; }
        
        public int value { get; set; }
        
        public virtual answers Answer { get; set; }
        public virtual questions Question { get; set; }
        public virtual options Option { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.answer_values.Add(this);
            dbContext.SaveChanges();

            dbContext.answer_value_versions.Add(MapVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            answer_values answerValue = dbContext.answer_values.FirstOrDefault(x => x.Id == Id);

            if (answerValue == null)
            {
                throw new NullReferenceException($"Could not find answer value with Id: {Id}");
            }

            answerValue.value = value;
            answerValue.answerId = answerId;
            answerValue.optionsId = optionsId;
            answerValue.questionId = questionId;

            if (dbContext.ChangeTracker.HasChanges())
            {
                answerValue.version += 1;
                answerValue.updated_at = DateTime.Now;

                dbContext.answer_value_versions.Add(MapVersions(answerValue));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            answer_values answerValue = dbContext.answer_values.FirstOrDefault(x => x.Id == Id);

            if (answerValue == null)
            {
                throw new NullReferenceException($"Could not find answer value with Id: {Id}");
            }

            answerValue.workflow_state = Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                answerValue.version += 1;
                answerValue.updated_at = DateTime.Now;

                dbContext.answer_value_versions.Add(MapVersions(answerValue));
                dbContext.SaveChanges();
            }
        }
        private answer_value_versions MapVersions(answer_values answerValue)
        {
            answer_value_versions answerValueVersion = new answer_value_versions();

            answerValueVersion.questionId = answerValue.questionId;
            answerValueVersion.value = answerValue.value;
            answerValueVersion.optionsId = answerValue.optionsId;
            answerValueVersion.answerId = answerValue.answerId;
            answerValueVersion.answerValueId = answerValue.Id;
            

            return answerValueVersion;
        }
    }
}