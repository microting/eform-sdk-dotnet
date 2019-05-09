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
using System.Security.AccessControl;
using eFormShared;

namespace eFormSqlController
{
    public partial class questions : BaseEntity
    {
        [ForeignKey("question_set")]
        public int questionSetId { get; set; }
        
        public string questionType { get; set; }
        
        public int minimum { get; set; }
        
        public int maximum { get; set; }
        
        public string type { get; set; }
        
        public int refId { get; set; }
        
        public int questionIndex { get; set; }
        
        public bool image { get; set; }
        
        public int continuousQuestionId { get; set; }
        
        public string imagePostion { get; set; }
        
        public bool prioritised { get; set; }
        
        public bool backButtonEnabled { get; set; }
        
        public string fontSize { get; set; }
        
        public int minDuration { get; set; }
        
        public int maxDuration { get; set; }
        
        public bool validDisplay { get; set; }

        public virtual question_sets QuestionSet { get; set; }
        public void Create(MicrotingDbAnySql dbContext)
        {
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.questions.Add(this);
            dbContext.SaveChanges();

            dbContext.question_versions.Add(MapVersions(this));
            dbContext.SaveChanges();

            id = id;
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            questions question = dbContext.questions.FirstOrDefault(x => x.id == id);

            if (question == null)
            {
                throw new NullReferenceException($"Could no find question with ID: {id}");
            }

            question.type = type;
            question.image = image;
            question.backButtonEnabled = backButtonEnabled;
            question.maximum = maximum;
            question.minimum = minimum;
            question.prioritised = prioritised;
            question.refId = refId;
            question.fontSize = fontSize;
            question.maxDuration = maxDuration;
            question.minDuration = minDuration;
            question.imagePostion = imagePostion;
            question.questionType = questionType;
            question.validDisplay = validDisplay;
            question.questionIndex = questionIndex;
            question.questionSetId = questionSetId;
            question.continuousQuestionId = continuousQuestionId;

            if (dbContext.ChangeTracker.HasChanges())
            {
                version += 1;
                updated_at = DateTime.Now;

                dbContext.question_versions.Add(MapVersions(question));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            questions question = dbContext.questions.FirstOrDefault(x => x.id == id);

            if (question == null)
            {
                throw new NullReferenceException($"Could no find question with ID: {id}");
            }

            question.workflow_state = Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                version += 1;
                updated_at = DateTime.Now;

                dbContext.question_versions.Add(MapVersions(question));
                dbContext.SaveChanges();
            }
        }

        public question_versions MapVersions(questions question)
        {
            question_versions questionVersion = new question_versions();

            questionVersion.questionSetId = question.questionSetId;
            questionVersion.type = question.type;
            questionVersion.image = question.image;
            questionVersion.maximum = question.maximum;
            questionVersion.minimum = question.minimum;
            questionVersion.prioritised = question.prioritised;
            questionVersion.refId = question.refId;
            questionVersion.fontSize = question.fontSize;
            questionVersion.questionId = question.id;
            questionVersion.maxDuration = question.maxDuration;
            questionVersion.minDuration = question.minDuration;
            questionVersion.imagePostion = question.imagePostion;
            questionVersion.questionType = question.questionType;
            questionVersion.validDisplay = question.validDisplay;
            questionVersion.questionIndex = question.questionIndex;
            questionVersion.backButtonEnabled = question.backButtonEnabled;
            questionVersion.continuousQuestionId = question.continuousQuestionId;

            return questionVersion;
        }
    }
}