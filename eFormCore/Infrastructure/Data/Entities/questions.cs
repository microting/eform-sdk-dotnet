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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
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
        
        public int? MicrotingUid { get; set; }

        public virtual question_sets QuestionSet { get; set; }

        public virtual ICollection<question_translations> QuestionTranslationses { get; set; }
        
        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.questions.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.question_versions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync();

            Id = Id;
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            questions question = await dbContext.questions.FirstOrDefaultAsync(x => x.Id == Id);

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
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            questions question = await dbContext.questions.FirstOrDefaultAsync(x => x.Id == Id);

            if (question == null)
            {
                throw new NullReferenceException($"Could no find question with Id: {Id}");
            }

            question.WorkflowState = Constants.Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                Version += 1;
                UpdatedAt = DateTime.Now;

                dbContext.question_versions.Add(MapVersions(question));
                await dbContext.SaveChangesAsync();
            }
        }

        public question_versions MapVersions(questions question)
        {
            question_versions questionVersion = new question_versions
            {
                QuestionSetId = question.QuestionSetId,
                Type = question.Type,
                Image = question.Image,
                Maximum = question.Maximum,
                Minimum = question.Minimum,
                Prioritised = question.Prioritised,
                RefId = question.RefId,
                FontSize = question.FontSize,
                QuestionId = question.Id,
                MaxDuration = question.MaxDuration,
                MinDuration = question.MinDuration,
                ImagePosition = question.ImagePosition,
                QuestionType = question.QuestionType,
                ValidDisplay = question.ValidDisplay,
                QuestionIndex = question.QuestionIndex,
                BackButtonEnabled = question.BackButtonEnabled,
                ContinuousQuestionId = question.ContinuousQuestionId,
                CreatedAt = question.CreatedAt,
                Version = question.Version,
                UpdatedAt = question.UpdatedAt,
                WorkflowState = question.WorkflowState,
                MicrotingUid = question.MicrotingUid
            };

            return questionVersion;
        }
    }
}