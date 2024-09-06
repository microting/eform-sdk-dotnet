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

            Assert.That(questions.Count(), Is.EqualTo(1));
            Assert.That(questionVersions.Count(), Is.EqualTo(1));

            Assert.That(questions[0].CreatedAt.ToString(), Is.EqualTo(question.CreatedAt.ToString()));
            Assert.That(questions[0].Version, Is.EqualTo(question.Version));
            //            Assert.AreEqual(question.UpdatedAt.ToString(), questions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(questions[0].WorkflowState));
            Assert.That(questionVersions[0].Id, Is.EqualTo(question.Id));
            Assert.That(questions[0].Image, Is.EqualTo(question.Image));
            Assert.That(questions[0].Maximum, Is.EqualTo(question.Maximum));
            Assert.That(questions[0].Minimum, Is.EqualTo(question.Minimum));
            Assert.That(questions[0].Prioritised, Is.EqualTo(question.Prioritised));
            Assert.That(questions[0].Type, Is.EqualTo(question.Type));
            Assert.That(questions[0].FontSize, Is.EqualTo(question.FontSize));
            Assert.That(questions[0].ImagePosition, Is.EqualTo(question.ImagePosition));
            Assert.That(questions[0].MaxDuration, Is.EqualTo(question.MaxDuration));
            Assert.That(questions[0].MinDuration, Is.EqualTo(question.MinDuration));
            Assert.That(questions[0].QuestionIndex, Is.EqualTo(question.QuestionIndex));
            Assert.That(questions[0].QuestionType, Is.EqualTo(question.QuestionType));
            Assert.That(questions[0].RefId, Is.EqualTo(question.RefId));
            Assert.That(questions[0].ValidDisplay, Is.EqualTo(question.ValidDisplay));
            Assert.That(questions[0].BackButtonEnabled, Is.EqualTo(question.BackButtonEnabled));
            Assert.That(questionSetForQuestion.Id, Is.EqualTo(question.QuestionSetId));

            //Versions
            Assert.That(questionVersions[0].CreatedAt.ToString(), Is.EqualTo(question.CreatedAt.ToString()));
            Assert.That(questionVersions[0].Version, Is.EqualTo(1));
            //            Assert.AreEqual(question.UpdatedAt.ToString(), questionVersions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(questionVersions[0].WorkflowState));
            Assert.That(questionVersions[0].QuestionId, Is.EqualTo(question.Id));
            Assert.That(questionVersions[0].Image, Is.EqualTo(question.Image));
            Assert.That(questionVersions[0].Maximum, Is.EqualTo(question.Maximum));
            Assert.That(questionVersions[0].Minimum, Is.EqualTo(question.Minimum));
            Assert.That(questionVersions[0].Prioritised, Is.EqualTo(question.Prioritised));
            Assert.That(questionVersions[0].Type, Is.EqualTo(question.Type));
            Assert.That(questionVersions[0].FontSize, Is.EqualTo(question.FontSize));
            Assert.That(questionVersions[0].ImagePosition, Is.EqualTo(question.ImagePosition));
            Assert.That(questionVersions[0].MaxDuration, Is.EqualTo(question.MaxDuration));
            Assert.That(questionVersions[0].MinDuration, Is.EqualTo(question.MinDuration));
            Assert.That(questionVersions[0].QuestionIndex, Is.EqualTo(question.QuestionIndex));
            Assert.That(questionVersions[0].QuestionType, Is.EqualTo(question.QuestionType));
            Assert.That(questionVersions[0].RefId, Is.EqualTo(question.RefId));
            Assert.That(questionVersions[0].ValidDisplay, Is.EqualTo(question.ValidDisplay));
            Assert.That(questionVersions[0].BackButtonEnabled, Is.EqualTo(question.BackButtonEnabled));
            Assert.That(questionVersions[0].QuestionSetId, Is.EqualTo(questionSetForQuestion.Id));
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

            Assert.That(questions.Count(), Is.EqualTo(1));
            Assert.That(questionVersions.Count(), Is.EqualTo(2));

            Assert.That(questions[0].CreatedAt.ToString(), Is.EqualTo(question.CreatedAt.ToString()));
            Assert.That(questions[0].Version, Is.EqualTo(question.Version));
            //            Assert.AreEqual(question.UpdatedAt.ToString(), questions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(questions[0].WorkflowState));
            Assert.That(questionVersions[0].Id, Is.EqualTo(question.Id));
            Assert.That(questions[0].Image, Is.EqualTo(question.Image));
            Assert.That(questions[0].Maximum, Is.EqualTo(question.Maximum));
            Assert.That(questions[0].Minimum, Is.EqualTo(question.Minimum));
            Assert.That(questions[0].Prioritised, Is.EqualTo(question.Prioritised));
            Assert.That(questions[0].Type, Is.EqualTo(question.Type));
            Assert.That(questions[0].FontSize, Is.EqualTo(question.FontSize));
            Assert.That(questions[0].ImagePosition, Is.EqualTo(question.ImagePosition));
            Assert.That(questions[0].MaxDuration, Is.EqualTo(question.MaxDuration));
            Assert.That(questions[0].MinDuration, Is.EqualTo(question.MinDuration));
            Assert.That(questions[0].QuestionIndex, Is.EqualTo(question.QuestionIndex));
            Assert.That(questions[0].QuestionType, Is.EqualTo(question.QuestionType));
            Assert.That(questions[0].RefId, Is.EqualTo(question.RefId));
            Assert.That(questions[0].ValidDisplay, Is.EqualTo(question.ValidDisplay));
            Assert.That(questions[0].BackButtonEnabled, Is.EqualTo(question.BackButtonEnabled));
            Assert.That(questionSetForQuestion.Id, Is.EqualTo(question.QuestionSetId));

            //Old Version
            Assert.That(questionVersions[0].CreatedAt.ToString(), Is.EqualTo(question.CreatedAt.ToString()));
            Assert.That(questionVersions[0].Version, Is.EqualTo(1));
            //            Assert.AreEqual(oldUpdatedAt.ToString(), questionVersions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(questionVersions[0].WorkflowState));
            Assert.That(questionVersions[0].QuestionId, Is.EqualTo(question.Id));
            Assert.That(questionVersions[0].Image, Is.EqualTo(oldImage));
            Assert.That(questionVersions[0].Maximum, Is.EqualTo(oldMaximum));
            Assert.That(questionVersions[0].Minimum, Is.EqualTo(oldMinimum));
            Assert.That(questionVersions[0].Prioritised, Is.EqualTo(oldPrioritised));
            Assert.That(questionVersions[0].Type, Is.EqualTo(oldType));
            Assert.That(questionVersions[0].FontSize, Is.EqualTo(oldFontSize));
            Assert.That(questionVersions[0].ImagePosition, Is.EqualTo(oldImagePosition));
            Assert.That(questionVersions[0].MaxDuration, Is.EqualTo(oldMaxDuration));
            Assert.That(questionVersions[0].MinDuration, Is.EqualTo(oldMinDuration));
            Assert.That(questionVersions[0].QuestionIndex, Is.EqualTo(oldQuestionIndex));
            Assert.That(questionVersions[0].QuestionType, Is.EqualTo(oldQuestionType));
            Assert.That(questionVersions[0].RefId, Is.EqualTo(oldRefId));
            Assert.That(questionVersions[0].ValidDisplay, Is.EqualTo(oldValidDisplau));
            Assert.That(questionVersions[0].BackButtonEnabled, Is.EqualTo(oldBackButtonEnabled));
            Assert.That(questionVersions[0].QuestionSetId, Is.EqualTo(questionSetForQuestion.Id));

            //New Version
            Assert.That(questionVersions[1].CreatedAt.ToString(), Is.EqualTo(question.CreatedAt.ToString()));
            Assert.That(questionVersions[1].Version, Is.EqualTo(2));
            //            Assert.AreEqual(question.UpdatedAt.ToString(), questionVersions[1].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(questionVersions[1].WorkflowState));
            Assert.That(questionVersions[0].QuestionId, Is.EqualTo(question.Id));
            Assert.That(questionVersions[1].Image, Is.EqualTo(question.Image));
            Assert.That(questionVersions[1].Maximum, Is.EqualTo(question.Maximum));
            Assert.That(questionVersions[1].Minimum, Is.EqualTo(question.Minimum));
            Assert.That(questionVersions[1].Prioritised, Is.EqualTo(question.Prioritised));
            Assert.That(questionVersions[1].Type, Is.EqualTo(question.Type));
            Assert.That(questionVersions[1].FontSize, Is.EqualTo(question.FontSize));
            Assert.That(questionVersions[1].ImagePosition, Is.EqualTo(question.ImagePosition));
            Assert.That(questionVersions[1].MaxDuration, Is.EqualTo(question.MaxDuration));
            Assert.That(questionVersions[1].MinDuration, Is.EqualTo(question.MinDuration));
            Assert.That(questionVersions[1].QuestionIndex, Is.EqualTo(question.QuestionIndex));
            Assert.That(questionVersions[1].QuestionType, Is.EqualTo(question.QuestionType));
            Assert.That(questionVersions[1].RefId, Is.EqualTo(question.RefId));
            Assert.That(questionVersions[1].ValidDisplay, Is.EqualTo(question.ValidDisplay));
            Assert.That(questionVersions[1].BackButtonEnabled, Is.EqualTo(question.BackButtonEnabled));
            Assert.That(questionVersions[1].QuestionSetId, Is.EqualTo(questionSetForQuestion.Id));
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

            Assert.That(questions.Count(), Is.EqualTo(1));
            Assert.That(questionVersions.Count(), Is.EqualTo(2));

            Assert.That(questions[0].CreatedAt.ToString(), Is.EqualTo(question.CreatedAt.ToString()));
            Assert.That(questions[0].Version, Is.EqualTo(question.Version));
            //            Assert.AreEqual(question.UpdatedAt.ToString(), questions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Removed, Is.EqualTo(questions[0].WorkflowState));
            Assert.That(questionVersions[0].Id, Is.EqualTo(question.Id));
            Assert.That(questions[0].Image, Is.EqualTo(question.Image));
            Assert.That(questions[0].Maximum, Is.EqualTo(question.Maximum));
            Assert.That(questions[0].Minimum, Is.EqualTo(question.Minimum));
            Assert.That(questions[0].Prioritised, Is.EqualTo(question.Prioritised));
            Assert.That(questions[0].Type, Is.EqualTo(question.Type));
            Assert.That(questions[0].FontSize, Is.EqualTo(question.FontSize));
            Assert.That(questions[0].ImagePosition, Is.EqualTo(question.ImagePosition));
            Assert.That(questions[0].MaxDuration, Is.EqualTo(question.MaxDuration));
            Assert.That(questions[0].MinDuration, Is.EqualTo(question.MinDuration));
            Assert.That(questions[0].QuestionIndex, Is.EqualTo(question.QuestionIndex));
            Assert.That(questions[0].QuestionType, Is.EqualTo(question.QuestionType));
            Assert.That(questions[0].RefId, Is.EqualTo(question.RefId));
            Assert.That(questions[0].ValidDisplay, Is.EqualTo(question.ValidDisplay));
            Assert.That(questions[0].BackButtonEnabled, Is.EqualTo(question.BackButtonEnabled));
            Assert.That(questionSetForQuestion.Id, Is.EqualTo(question.QuestionSetId));

            //Old Version
            Assert.That(questionVersions[0].CreatedAt.ToString(), Is.EqualTo(question.CreatedAt.ToString()));
            Assert.That(questionVersions[0].Version, Is.EqualTo(1));
            //            Assert.AreEqual(oldUpdatedAt.ToString(), questionVersions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(questionVersions[0].WorkflowState));
            Assert.That(questionVersions[0].QuestionId, Is.EqualTo(question.Id));
            Assert.That(questionVersions[0].Image, Is.EqualTo(question.Image));
            Assert.That(questionVersions[0].Maximum, Is.EqualTo(question.Maximum));
            Assert.That(questionVersions[0].Minimum, Is.EqualTo(question.Minimum));
            Assert.That(questionVersions[0].Prioritised, Is.EqualTo(question.Prioritised));
            Assert.That(questionVersions[0].Type, Is.EqualTo(question.Type));
            Assert.That(questionVersions[0].FontSize, Is.EqualTo(question.FontSize));
            Assert.That(questionVersions[0].ImagePosition, Is.EqualTo(question.ImagePosition));
            Assert.That(questionVersions[0].MaxDuration, Is.EqualTo(question.MaxDuration));
            Assert.That(questionVersions[0].MinDuration, Is.EqualTo(question.MinDuration));
            Assert.That(questionVersions[0].QuestionIndex, Is.EqualTo(question.QuestionIndex));
            Assert.That(questionVersions[0].QuestionType, Is.EqualTo(question.QuestionType));
            Assert.That(questionVersions[0].RefId, Is.EqualTo(question.RefId));
            Assert.That(questionVersions[0].ValidDisplay, Is.EqualTo(question.ValidDisplay));
            Assert.That(questionVersions[0].BackButtonEnabled, Is.EqualTo(question.BackButtonEnabled));
            Assert.That(questionVersions[0].QuestionSetId, Is.EqualTo(questionSetForQuestion.Id));

            //New Version
            Assert.That(questionVersions[1].CreatedAt.ToString(), Is.EqualTo(question.CreatedAt.ToString()));
            Assert.That(questionVersions[1].Version, Is.EqualTo(2));
            //            Assert.AreEqual(question.UpdatedAt.ToString(), questionVersions[1].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Removed, Is.EqualTo(questionVersions[1].WorkflowState));
            Assert.That(questionVersions[0].QuestionId, Is.EqualTo(question.Id));
            Assert.That(questionVersions[1].Image, Is.EqualTo(question.Image));
            Assert.That(questionVersions[1].Maximum, Is.EqualTo(question.Maximum));
            Assert.That(questionVersions[1].Minimum, Is.EqualTo(question.Minimum));
            Assert.That(questionVersions[1].Prioritised, Is.EqualTo(question.Prioritised));
            Assert.That(questionVersions[1].Type, Is.EqualTo(question.Type));
            Assert.That(questionVersions[1].FontSize, Is.EqualTo(question.FontSize));
            Assert.That(questionVersions[1].ImagePosition, Is.EqualTo(question.ImagePosition));
            Assert.That(questionVersions[1].MaxDuration, Is.EqualTo(question.MaxDuration));
            Assert.That(questionVersions[1].MinDuration, Is.EqualTo(question.MinDuration));
            Assert.That(questionVersions[1].QuestionIndex, Is.EqualTo(question.QuestionIndex));
            Assert.That(questionVersions[1].QuestionType, Is.EqualTo(question.QuestionType));
            Assert.That(questionVersions[1].RefId, Is.EqualTo(question.RefId));
            Assert.That(questionVersions[1].ValidDisplay, Is.EqualTo(question.ValidDisplay));
            Assert.That(questionVersions[1].BackButtonEnabled, Is.EqualTo(question.BackButtonEnabled));
            Assert.That(questionVersions[1].QuestionSetId, Is.EqualTo(questionSetForQuestion.Id));
        }
    }
}