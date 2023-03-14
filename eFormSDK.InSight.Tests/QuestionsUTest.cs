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
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.InSight.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class QuestionsUTest : DbTestFixture
    {
        [Test]
        public async Task Questions_Create_DoesCreate()
        {
            //Arrange

            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSetForQuestion = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion.Create(DbContext).ConfigureAwait(false);

            Question question = new Question
            {
                Image = randomBool,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                Prioritised = randomBool,
                Type = Guid.NewGuid().ToString(),
                FontSize = Guid.NewGuid().ToString(),
                ImagePosition = Guid.NewGuid().ToString(),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                QuestionType = Guid.NewGuid().ToString(),
                RefId = rnd.Next(1, 255),
                ValidDisplay = randomBool,
                BackButtonEnabled = randomBool,
                ContinuousQuestionId = rnd.Next(1, 255),
                QuestionSetId = questionSetForQuestion.Id
            };

            //Act

            await question.Create(DbContext).ConfigureAwait(false);

            List<Question> questions = DbContext.Questions.AsNoTracking().ToList();
            List<QuestionVersion> questionVersions = DbContext.QuestionVersions.AsNoTracking().ToList();

            Assert.NotNull(questions);
            Assert.NotNull(questionVersions);

            Assert.AreEqual(1, questions.Count());
            Assert.AreEqual(1, questionVersions.Count());

            Assert.AreEqual(question.CreatedAt.ToString(), questions[0].CreatedAt.ToString());
            Assert.AreEqual(question.Version, questions[0].Version);
//            Assert.AreEqual(question.UpdatedAt.ToString(), questions[0].UpdatedAt.ToString());
            Assert.AreEqual(questions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(question.Id, questionVersions[0].Id);
            Assert.AreEqual(question.Image, questions[0].Image);
            Assert.AreEqual(question.Maximum, questions[0].Maximum);
            Assert.AreEqual(question.Minimum, questions[0].Minimum);
            Assert.AreEqual(question.Prioritised, questions[0].Prioritised);
            Assert.AreEqual(question.Type, questions[0].Type);
            Assert.AreEqual(question.FontSize, questions[0].FontSize);
            Assert.AreEqual(question.ImagePosition, questions[0].ImagePosition);
            Assert.AreEqual(question.MaxDuration, questions[0].MaxDuration);
            Assert.AreEqual(question.MinDuration, questions[0].MinDuration);
            Assert.AreEqual(question.QuestionIndex, questions[0].QuestionIndex);
            Assert.AreEqual(question.QuestionType, questions[0].QuestionType);
            Assert.AreEqual(question.RefId, questions[0].RefId);
            Assert.AreEqual(question.ValidDisplay, questions[0].ValidDisplay);
            Assert.AreEqual(question.BackButtonEnabled, questions[0].BackButtonEnabled);
            Assert.AreEqual(question.QuestionSetId, questionSetForQuestion.Id);

            //Versions
            Assert.AreEqual(question.CreatedAt.ToString(), questionVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, questionVersions[0].Version);
//            Assert.AreEqual(question.UpdatedAt.ToString(), questionVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(questionVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(question.Id, questionVersions[0].QuestionId);
            Assert.AreEqual(question.Image, questionVersions[0].Image);
            Assert.AreEqual(question.Maximum, questionVersions[0].Maximum);
            Assert.AreEqual(question.Minimum, questionVersions[0].Minimum);
            Assert.AreEqual(question.Prioritised, questionVersions[0].Prioritised);
            Assert.AreEqual(question.Type, questionVersions[0].Type);
            Assert.AreEqual(question.FontSize, questionVersions[0].FontSize);
            Assert.AreEqual(question.ImagePosition, questionVersions[0].ImagePosition);
            Assert.AreEqual(question.MaxDuration, questionVersions[0].MaxDuration);
            Assert.AreEqual(question.MinDuration, questionVersions[0].MinDuration);
            Assert.AreEqual(question.QuestionIndex, questionVersions[0].QuestionIndex);
            Assert.AreEqual(question.QuestionType, questionVersions[0].QuestionType);
            Assert.AreEqual(question.RefId, questionVersions[0].RefId);
            Assert.AreEqual(question.ValidDisplay, questionVersions[0].ValidDisplay);
            Assert.AreEqual(question.BackButtonEnabled, questionVersions[0].BackButtonEnabled);
            Assert.AreEqual(questionSetForQuestion.Id, questionVersions[0].QuestionSetId);
        }

        [Test]
        public async Task Questions_Update_DoesUpdate()
        {
            //Arrange

            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSetForQuestion = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion.Create(DbContext).ConfigureAwait(false);

            Question question = new Question
            {
                Image = randomBool,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                Prioritised = randomBool,
                Type = Guid.NewGuid().ToString(),
                FontSize = Guid.NewGuid().ToString(),
                ImagePosition = Guid.NewGuid().ToString(),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                QuestionType = Guid.NewGuid().ToString(),
                RefId = rnd.Next(1, 255),
                ValidDisplay = randomBool,
                BackButtonEnabled = randomBool,
                ContinuousQuestionId = rnd.Next(1, 255),
                QuestionSetId = questionSetForQuestion.Id
            };
            await question.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = question.UpdatedAt;
            bool oldImage = question.Image;
            int? oldMaximum = question.Maximum;
            int? oldMinimum = question.Minimum;
            bool oldPrioritised = question.Prioritised;
            string oldType = question.Type;
            string oldFontSize = question.FontSize;
            string oldImagePosition = question.ImagePosition;
            int? oldMaxDuration = question.MaxDuration;
            int? oldMinDuration = question.MinDuration;
            int? oldQuestionIndex = question.QuestionIndex;
            string oldQuestionType = question.QuestionType;
            int? oldRefId = question.RefId;
            bool oldValidDisplau = question.ValidDisplay;
            bool oldBackButtonEnabled = question.BackButtonEnabled;

            question.Image = randomBool;
            question.Maximum = rnd.Next(1, 255);
            question.Minimum = rnd.Next(1, 255);
            question.Prioritised = randomBool;
            question.Type = Guid.NewGuid().ToString();
            question.FontSize = Guid.NewGuid().ToString();
            question.ImagePosition = Guid.NewGuid().ToString();
            question.MaxDuration = rnd.Next(1, 255);
            question.MinDuration = rnd.Next(1, 255);
            question.QuestionIndex = rnd.Next(1, 255);
            question.QuestionType = Guid.NewGuid().ToString();
            question.RefId = rnd.Next(1, 255);
            question.ValidDisplay = randomBool;
            question.BackButtonEnabled = randomBool;
            question.ContinuousQuestionId = rnd.Next(1, 255);

            await question.Update(DbContext).ConfigureAwait(false);

            List<Question> questions = DbContext.Questions.AsNoTracking().ToList();
            List<QuestionVersion> questionVersions = DbContext.QuestionVersions.AsNoTracking().ToList();

            Assert.NotNull(questions);
            Assert.NotNull(questionVersions);

            Assert.AreEqual(1, questions.Count());
            Assert.AreEqual(2, questionVersions.Count());

            Assert.AreEqual(question.CreatedAt.ToString(), questions[0].CreatedAt.ToString());
            Assert.AreEqual(question.Version, questions[0].Version);
//            Assert.AreEqual(question.UpdatedAt.ToString(), questions[0].UpdatedAt.ToString());
            Assert.AreEqual(questions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(question.Id, questionVersions[0].Id);
            Assert.AreEqual(question.Image, questions[0].Image);
            Assert.AreEqual(question.Maximum, questions[0].Maximum);
            Assert.AreEqual(question.Minimum, questions[0].Minimum);
            Assert.AreEqual(question.Prioritised, questions[0].Prioritised);
            Assert.AreEqual(question.Type, questions[0].Type);
            Assert.AreEqual(question.FontSize, questions[0].FontSize);
            Assert.AreEqual(question.ImagePosition, questions[0].ImagePosition);
            Assert.AreEqual(question.MaxDuration, questions[0].MaxDuration);
            Assert.AreEqual(question.MinDuration, questions[0].MinDuration);
            Assert.AreEqual(question.QuestionIndex, questions[0].QuestionIndex);
            Assert.AreEqual(question.QuestionType, questions[0].QuestionType);
            Assert.AreEqual(question.RefId, questions[0].RefId);
            Assert.AreEqual(question.ValidDisplay, questions[0].ValidDisplay);
            Assert.AreEqual(question.BackButtonEnabled, questions[0].BackButtonEnabled);
            Assert.AreEqual(question.QuestionSetId, questionSetForQuestion.Id);

            //Old Version
            Assert.AreEqual(question.CreatedAt.ToString(), questionVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, questionVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), questionVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(questionVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(question.Id, questionVersions[0].QuestionId);
            Assert.AreEqual(oldImage, questionVersions[0].Image);
            Assert.AreEqual(oldMaximum, questionVersions[0].Maximum);
            Assert.AreEqual(oldMinimum, questionVersions[0].Minimum);
            Assert.AreEqual(oldPrioritised, questionVersions[0].Prioritised);
            Assert.AreEqual(oldType, questionVersions[0].Type);
            Assert.AreEqual(oldFontSize, questionVersions[0].FontSize);
            Assert.AreEqual(oldImagePosition, questionVersions[0].ImagePosition);
            Assert.AreEqual(oldMaxDuration, questionVersions[0].MaxDuration);
            Assert.AreEqual(oldMinDuration, questionVersions[0].MinDuration);
            Assert.AreEqual(oldQuestionIndex, questionVersions[0].QuestionIndex);
            Assert.AreEqual(oldQuestionType, questionVersions[0].QuestionType);
            Assert.AreEqual(oldRefId, questionVersions[0].RefId);
            Assert.AreEqual(oldValidDisplau, questionVersions[0].ValidDisplay);
            Assert.AreEqual(oldBackButtonEnabled, questionVersions[0].BackButtonEnabled);
            Assert.AreEqual(questionSetForQuestion.Id, questionVersions[0].QuestionSetId);

            //New Version
            Assert.AreEqual(question.CreatedAt.ToString(), questionVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, questionVersions[1].Version);
//            Assert.AreEqual(question.UpdatedAt.ToString(), questionVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(questionVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(question.Id, questionVersions[0].QuestionId);
            Assert.AreEqual(question.Image, questionVersions[1].Image);
            Assert.AreEqual(question.Maximum, questionVersions[1].Maximum);
            Assert.AreEqual(question.Minimum, questionVersions[1].Minimum);
            Assert.AreEqual(question.Prioritised, questionVersions[1].Prioritised);
            Assert.AreEqual(question.Type, questionVersions[1].Type);
            Assert.AreEqual(question.FontSize, questionVersions[1].FontSize);
            Assert.AreEqual(question.ImagePosition, questionVersions[1].ImagePosition);
            Assert.AreEqual(question.MaxDuration, questionVersions[1].MaxDuration);
            Assert.AreEqual(question.MinDuration, questionVersions[1].MinDuration);
            Assert.AreEqual(question.QuestionIndex, questionVersions[1].QuestionIndex);
            Assert.AreEqual(question.QuestionType, questionVersions[1].QuestionType);
            Assert.AreEqual(question.RefId, questionVersions[1].RefId);
            Assert.AreEqual(question.ValidDisplay, questionVersions[1].ValidDisplay);
            Assert.AreEqual(question.BackButtonEnabled, questionVersions[1].BackButtonEnabled);
            Assert.AreEqual(questionSetForQuestion.Id, questionVersions[1].QuestionSetId);
        }

        [Test]
        public async Task Questions_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange

            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSetForQuestion = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion.Create(DbContext).ConfigureAwait(false);

            Question question = new Question
            {
                Image = randomBool,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                Prioritised = randomBool,
                Type = Guid.NewGuid().ToString(),
                FontSize = Guid.NewGuid().ToString(),
                ImagePosition = Guid.NewGuid().ToString(),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                QuestionType = Guid.NewGuid().ToString(),
                RefId = rnd.Next(1, 255),
                ValidDisplay = randomBool,
                BackButtonEnabled = randomBool,
                ContinuousQuestionId = rnd.Next(1, 255),
                QuestionSetId = questionSetForQuestion.Id
            };
            await question.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = question.UpdatedAt;

            await question.Delete(DbContext);

            List<Question> questions = DbContext.Questions.AsNoTracking().ToList();
            List<QuestionVersion> questionVersions = DbContext.QuestionVersions.AsNoTracking().ToList();

            Assert.NotNull(questions);
            Assert.NotNull(questionVersions);

            Assert.AreEqual(1, questions.Count());
            Assert.AreEqual(2, questionVersions.Count());

            Assert.AreEqual(question.CreatedAt.ToString(), questions[0].CreatedAt.ToString());
            Assert.AreEqual(question.Version, questions[0].Version);
//            Assert.AreEqual(question.UpdatedAt.ToString(), questions[0].UpdatedAt.ToString());
            Assert.AreEqual(questions[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(question.Id, questionVersions[0].Id);
            Assert.AreEqual(question.Image, questions[0].Image);
            Assert.AreEqual(question.Maximum, questions[0].Maximum);
            Assert.AreEqual(question.Minimum, questions[0].Minimum);
            Assert.AreEqual(question.Prioritised, questions[0].Prioritised);
            Assert.AreEqual(question.Type, questions[0].Type);
            Assert.AreEqual(question.FontSize, questions[0].FontSize);
            Assert.AreEqual(question.ImagePosition, questions[0].ImagePosition);
            Assert.AreEqual(question.MaxDuration, questions[0].MaxDuration);
            Assert.AreEqual(question.MinDuration, questions[0].MinDuration);
            Assert.AreEqual(question.QuestionIndex, questions[0].QuestionIndex);
            Assert.AreEqual(question.QuestionType, questions[0].QuestionType);
            Assert.AreEqual(question.RefId, questions[0].RefId);
            Assert.AreEqual(question.ValidDisplay, questions[0].ValidDisplay);
            Assert.AreEqual(question.BackButtonEnabled, questions[0].BackButtonEnabled);
            Assert.AreEqual(question.QuestionSetId, questionSetForQuestion.Id);

            //Old Version
            Assert.AreEqual(question.CreatedAt.ToString(), questionVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, questionVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), questionVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(questionVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(question.Id, questionVersions[0].QuestionId);
            Assert.AreEqual(question.Image, questionVersions[0].Image);
            Assert.AreEqual(question.Maximum, questionVersions[0].Maximum);
            Assert.AreEqual(question.Minimum, questionVersions[0].Minimum);
            Assert.AreEqual(question.Prioritised, questionVersions[0].Prioritised);
            Assert.AreEqual(question.Type, questionVersions[0].Type);
            Assert.AreEqual(question.FontSize, questionVersions[0].FontSize);
            Assert.AreEqual(question.ImagePosition, questionVersions[0].ImagePosition);
            Assert.AreEqual(question.MaxDuration, questionVersions[0].MaxDuration);
            Assert.AreEqual(question.MinDuration, questionVersions[0].MinDuration);
            Assert.AreEqual(question.QuestionIndex, questionVersions[0].QuestionIndex);
            Assert.AreEqual(question.QuestionType, questionVersions[0].QuestionType);
            Assert.AreEqual(question.RefId, questionVersions[0].RefId);
            Assert.AreEqual(question.ValidDisplay, questionVersions[0].ValidDisplay);
            Assert.AreEqual(question.BackButtonEnabled, questionVersions[0].BackButtonEnabled);
            Assert.AreEqual(questionSetForQuestion.Id, questionVersions[0].QuestionSetId);

            //New Version
            Assert.AreEqual(question.CreatedAt.ToString(), questionVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, questionVersions[1].Version);
//            Assert.AreEqual(question.UpdatedAt.ToString(), questionVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(questionVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(question.Id, questionVersions[0].QuestionId);
            Assert.AreEqual(question.Image, questionVersions[1].Image);
            Assert.AreEqual(question.Maximum, questionVersions[1].Maximum);
            Assert.AreEqual(question.Minimum, questionVersions[1].Minimum);
            Assert.AreEqual(question.Prioritised, questionVersions[1].Prioritised);
            Assert.AreEqual(question.Type, questionVersions[1].Type);
            Assert.AreEqual(question.FontSize, questionVersions[1].FontSize);
            Assert.AreEqual(question.ImagePosition, questionVersions[1].ImagePosition);
            Assert.AreEqual(question.MaxDuration, questionVersions[1].MaxDuration);
            Assert.AreEqual(question.MinDuration, questionVersions[1].MinDuration);
            Assert.AreEqual(question.QuestionIndex, questionVersions[1].QuestionIndex);
            Assert.AreEqual(question.QuestionType, questionVersions[1].QuestionType);
            Assert.AreEqual(question.RefId, questionVersions[1].RefId);
            Assert.AreEqual(question.ValidDisplay, questionVersions[1].ValidDisplay);
            Assert.AreEqual(question.BackButtonEnabled, questionVersions[1].BackButtonEnabled);
            Assert.AreEqual(questionSetForQuestion.Id, questionVersions[1].QuestionSetId);
        }
    }
}