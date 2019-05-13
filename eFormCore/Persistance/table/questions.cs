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
        public int QuestionSetId { get; set; }
        
        public string QuestionType { get; set; }
        
        public int Minimum { get; set; }
        
        public int Maximum { get; set; }
        
        public string Type { get; set; }
        
        public int RefId { get; set; }
        
        public int QuestionIndex { get; set; }
        
        public bool Image { get; set; }
        
        public int ContinuousQuestionId { get; set; }
        
        public string ImagePosition { get; set; }
        
        public bool Prioritised { get; set; }
        
        public bool BackButtonEnabled { get; set; }
        
        public string FontSize { get; set; }
        
        public int MinDuration { get; set; }
        
        public int MaxDuration { get; set; }
        
        public bool ValidDisplay { get; set; }

        public virtual question_sets QuestionSet { get; set; }
        public void Create(MicrotingDbAnySql dbContext)
        {
            WorkflowState = Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.questions.Add(this);
            dbContext.SaveChanges();

            dbContext.question_versions.Add(MapVersions(this));
            dbContext.SaveChanges();

            Id = Id;
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            questions question = dbContext.questions.FirstOrDefault(x => x.Id == Id);

            if (question == null)
            {
                throw new NullReferenceException($"Could no find question with Id: {Id}");
            }

            question.Type = Type;
            question.Image = Image;
            question.BackButtonEnabled = BackButtonEnabled;
            question.Maximum = Maximum;
            question.Minimum = Minimum;
            question.Prioritised = Prioritised;
            question.RefId = RefId;
            question.FontSize = FontSize;
            question.MaxDuration = MaxDuration;
            question.MinDuration = MinDuration;
            question.ImagePosition = ImagePosition;
            question.QuestionType = QuestionType;
            question.ValidDisplay = ValidDisplay;
            question.QuestionIndex = QuestionIndex;
            question.QuestionSetId = QuestionSetId;
            question.ContinuousQuestionId = ContinuousQuestionId;

            if (dbContext.ChangeTracker.HasChanges())
            {
                Version += 1;
                UpdatedAt = DateTime.Now;

                dbContext.question_versions.Add(MapVersions(question));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            questions question = dbContext.questions.FirstOrDefault(x => x.Id == Id);

            if (question == null)
            {
                throw new NullReferenceException($"Could no find question with Id: {Id}");
            }

            question.WorkflowState = Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                Version += 1;
                UpdatedAt = DateTime.Now;

                dbContext.question_versions.Add(MapVersions(question));
                dbContext.SaveChanges();
            }
        }

        public question_versions MapVersions(questions question)
        {
            question_versions questionVersion = new question_versions();

            questionVersion.QuestionSetId = question.QuestionSetId;
            questionVersion.Type = question.Type;
            questionVersion.Image = question.Image;
            questionVersion.Maximum = question.Maximum;
            questionVersion.Minimum = question.Minimum;
            questionVersion.Prioritised = question.Prioritised;
            questionVersion.RefId = question.RefId;
            questionVersion.FontSize = question.FontSize;
            questionVersion.QuestionId = question.Id;
            questionVersion.MaxDuration = question.MaxDuration;
            questionVersion.MinDuration = question.MinDuration;
            questionVersion.ImagePosition = question.ImagePosition;
            questionVersion.QuestionType = question.QuestionType;
            questionVersion.ValidDisplay = question.ValidDisplay;
            questionVersion.QuestionIndex = question.QuestionIndex;
            questionVersion.BackButtonEnabled = question.BackButtonEnabled;
            questionVersion.ContinuousQuestionId = question.ContinuousQuestionId;

            return questionVersion;
        }
    }
}