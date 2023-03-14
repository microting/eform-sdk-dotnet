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
    public class QuestionTranslationUTest : DbTestFixture
    {
        [Test]
        public async Task QuestionTranslation_Create_DoesCreate_W_MicrotingUID()
        {
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

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            QuestionTranslation questionTranslation = new QuestionTranslation
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
                MicrotingUid = rnd.Next(1, 255)
            };

            // Act
            await questionTranslation.Create(DbContext).ConfigureAwait(false);

            List<QuestionTranslation> questionTranslations = DbContext.QuestionTranslations.AsNoTracking().ToList();
            List<QuestionTranslationVersion> questionTranslationVersions =
                DbContext.QuestionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);

            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(1, questionTranslationVersions.Count);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslations[0].MicrotingUid);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslationVersions[0].MicrotingUid);
        }

        [Test]
        public async Task QuestionTranslation_Create_DoesCreate_WO_MicrotingUID()
        {
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

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            QuestionTranslation questionTranslation = new QuestionTranslation
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id
            };

            // Act
            await questionTranslation.Create(DbContext).ConfigureAwait(false);

            List<QuestionTranslation> questionTranslations = DbContext.QuestionTranslations.AsNoTracking().ToList();
            List<QuestionTranslationVersion> questionTranslationVersions =
                DbContext.QuestionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);

            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(1, questionTranslationVersions.Count);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(null, questionTranslations[0].MicrotingUid);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(null, questionTranslationVersions[0].MicrotingUid);
        }

        [Test]
        public async Task QuestionTranslation_Update_DoesUpdate_W_MicrotingUID()
        {
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

            QuestionSet questionSetForQuestion2 = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion2.Create(DbContext).ConfigureAwait(false);

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

            Question question2 = new Question
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
                QuestionSetId = questionSetForQuestion2.Id
            };
            await question2.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            Language language2 = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
            };
            await language2.Create(DbContext).ConfigureAwait(false);

            QuestionTranslation questionTranslation = new QuestionTranslation
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            await questionTranslation.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = questionTranslation.LanguageId;
            var oldName = questionTranslation.Name;
            var oldQuestionId = questionTranslation.QuestionId;
            var oldMicrotingUid = questionTranslation.MicrotingUid;

            questionTranslation.LanguageId = language2.Id;
            questionTranslation.Name = Guid.NewGuid().ToString();
            questionTranslation.QuestionId = question2.Id;
            questionTranslation.MicrotingUid = rnd.Next(1, 255);
            // Act
            await questionTranslation.Update(DbContext).ConfigureAwait(false);

            List<QuestionTranslation> questionTranslations = DbContext.QuestionTranslations.AsNoTracking().ToList();
            List<QuestionTranslationVersion> questionTranslationVersions =
                DbContext.QuestionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);

            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(2, questionTranslationVersions.Count);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslations[0].MicrotingUid);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[1].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[1].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[1].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslationVersions[1].MicrotingUid);

            Assert.AreEqual(oldLanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldName, questionTranslationVersions[0].Name);
            Assert.AreEqual(oldQuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(oldMicrotingUid, questionTranslationVersions[0].MicrotingUid);
        }

        [Test]
        public async Task QuestionTranslation_Update_DoesUpdate_WO_MicrotingUID()
        {
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

            QuestionSet questionSetForQuestion2 = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion2.Create(DbContext).ConfigureAwait(false);

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

            Question question2 = new Question
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
                QuestionSetId = questionSetForQuestion2.Id
            };
            await question2.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            Language language2 = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
            };
            await language2.Create(DbContext).ConfigureAwait(false);

            QuestionTranslation questionTranslation = new QuestionTranslation
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id
            };
            await questionTranslation.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = questionTranslation.LanguageId;
            var oldName = questionTranslation.Name;
            var oldQuestionId = questionTranslation.QuestionId;

            questionTranslation.LanguageId = language2.Id;
            questionTranslation.Name = Guid.NewGuid().ToString();
            questionTranslation.QuestionId = question2.Id;
            // Act
            await questionTranslation.Update(DbContext).ConfigureAwait(false);

            List<QuestionTranslation> questionTranslations = DbContext.QuestionTranslations.AsNoTracking().ToList();
            List<QuestionTranslationVersion> questionTranslationVersions =
                DbContext.QuestionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);

            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(2, questionTranslationVersions.Count);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslations[0].MicrotingUid);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[1].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[1].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[1].QuestionId);
            Assert.AreEqual(null, questionTranslationVersions[1].MicrotingUid);

            Assert.AreEqual(oldLanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldName, questionTranslationVersions[0].Name);
            Assert.AreEqual(oldQuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(null, questionTranslationVersions[0].MicrotingUid);
        }

        [Test]
        public async Task QuestionTranslation_Update_DoesUpdate_W_MicrotingUID_RemovesUid()
        {
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

            QuestionSet questionSetForQuestion2 = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion2.Create(DbContext).ConfigureAwait(false);

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

            Question question2 = new Question
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
                QuestionSetId = questionSetForQuestion2.Id
            };
            await question2.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            Language language2 = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language2.Create(DbContext).ConfigureAwait(false);

            QuestionTranslation questionTranslation = new QuestionTranslation
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            await questionTranslation.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = questionTranslation.LanguageId;
            var oldName = questionTranslation.Name;
            var oldQuestionId = questionTranslation.QuestionId;
            var oldMicrotingUid = questionTranslation.MicrotingUid;

            questionTranslation.LanguageId = language2.Id;
            questionTranslation.Name = Guid.NewGuid().ToString();
            questionTranslation.QuestionId = question2.Id;
            questionTranslation.MicrotingUid = null;
            // Act
            await questionTranslation.Update(DbContext).ConfigureAwait(false);

            List<QuestionTranslation> questionTranslations = DbContext.QuestionTranslations.AsNoTracking().ToList();
            List<QuestionTranslationVersion> questionTranslationVersions =
                DbContext.QuestionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);

            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(2, questionTranslationVersions.Count);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslations[0].MicrotingUid);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[1].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[1].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[1].QuestionId);
            Assert.AreEqual(null, questionTranslationVersions[1].MicrotingUid);

            Assert.AreEqual(oldLanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldName, questionTranslationVersions[0].Name);
            Assert.AreEqual(oldQuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(oldMicrotingUid, questionTranslationVersions[0].MicrotingUid);
        }

        [Test]
        public async Task QuestionTranslation_Update_DoesUpdate_WO_MicrotingUID_AddsUID()
        {
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

            QuestionSet questionSetForQuestion2 = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                ParentId = rnd.Next(1, 255),
                PossiblyDeployed = randomBool
            };
            await questionSetForQuestion2.Create(DbContext).ConfigureAwait(false);

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

            Question question2 = new Question
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
                QuestionSetId = questionSetForQuestion2.Id
            };
            await question2.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            Language language2 = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language2.Create(DbContext).ConfigureAwait(false);

            QuestionTranslation questionTranslation = new QuestionTranslation
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
            };
            await questionTranslation.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = questionTranslation.LanguageId;
            var oldName = questionTranslation.Name;
            var oldQuestionId = questionTranslation.QuestionId;

            questionTranslation.LanguageId = language2.Id;
            questionTranslation.Name = Guid.NewGuid().ToString();
            questionTranslation.QuestionId = question2.Id;
            questionTranslation.MicrotingUid = rnd.Next(1, 255);
            // Act
            await questionTranslation.Update(DbContext).ConfigureAwait(false);

            List<QuestionTranslation> questionTranslations = DbContext.QuestionTranslations.AsNoTracking().ToList();
            List<QuestionTranslationVersion> questionTranslationVersions =
                DbContext.QuestionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);

            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(2, questionTranslationVersions.Count);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslations[0].MicrotingUid);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[1].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[1].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[1].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslationVersions[1].MicrotingUid);

            Assert.AreEqual(oldLanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldName, questionTranslationVersions[0].Name);
            Assert.AreEqual(oldQuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(null, questionTranslationVersions[0].MicrotingUid);
        }

        [Test]
        public async Task QuestionTranslation_Delete_DoesDelete()
        {
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

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            QuestionTranslation questionTranslation = new QuestionTranslation
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            await questionTranslation.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = questionTranslation.LanguageId;
            var oldName = questionTranslation.Name;
            var oldQuestionId = questionTranslation.QuestionId;
            var oldMicrotingUid = questionTranslation.MicrotingUid;

            // Act
            await questionTranslation.Delete(DbContext).ConfigureAwait(false);

            List<QuestionTranslation> questionTranslations = DbContext.QuestionTranslations.AsNoTracking().ToList();
            List<QuestionTranslationVersion> questionTranslationVersions =
                DbContext.QuestionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);

            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(2, questionTranslationVersions.Count);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslations[0].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Removed, questionTranslations[0].WorkflowState);

            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[1].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[1].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[1].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslationVersions[1].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Removed, questionTranslationVersions[1].WorkflowState);

            Assert.AreEqual(oldLanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldName, questionTranslationVersions[0].Name);
            Assert.AreEqual(oldQuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(oldMicrotingUid, questionTranslationVersions[0].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Created, questionTranslationVersions[0].WorkflowState);
        }
    }
}