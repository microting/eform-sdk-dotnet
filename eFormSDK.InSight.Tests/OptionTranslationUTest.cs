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
    public class OptionTranslationUTest : DbTestFixture
    {
        [Test]
        public async Task OptionTranslation_Create_DoesCreate_W_MicrotingUid()
        {
            //Arrange

            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet.Create(DbContext).ConfigureAwait(false);

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
                QuestionSetId = questionSet.Id
            };
            await question.Create(DbContext).ConfigureAwait(false);

            Option option = new Option
            {
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id
            };
            await option.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            OptionTranslation optionTranslation = new OptionTranslation
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            // Act

            await optionTranslation.Create(DbContext).ConfigureAwait(false);

            List<OptionTranslation> optionTranslations = DbContext.OptionTranslations.AsNoTracking().ToList();
            List<OptionTranslationVersion> optionTranslationVersions =
                DbContext.OptionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);

            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(1, optionTranslationVersions.Count);

            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslations[0].MicrotingUid);

            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslationVersions[0].MicrotingUid);
        }

        [Test]
        public async Task OptionTranslation_Create_DoesCreate_WO_MicrotingUid()
        {
            //Arrange

            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet.Create(DbContext).ConfigureAwait(false);

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
                QuestionSetId = questionSet.Id
            };
            await question.Create(DbContext).ConfigureAwait(false);

            Option option = new Option
            {
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id
            };
            await option.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            OptionTranslation optionTranslation = new OptionTranslation
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString(),
            };
            // Act

            await optionTranslation.Create(DbContext).ConfigureAwait(false);

            List<OptionTranslation> optionTranslations = DbContext.OptionTranslations.AsNoTracking().ToList();
            List<OptionTranslationVersion> optionTranslationVersions =
                DbContext.OptionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);

            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(1, optionTranslationVersions.Count);

            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(null, optionTranslations[0].MicrotingUid);

            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(null, optionTranslationVersions[0].MicrotingUid);
        }

        [Test]
        public async Task OptionTranslation_Update_DoesUpdate_W_MicrotingUid()
        {
            //Arrange

            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSet2 = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet2.Create(DbContext).ConfigureAwait(false);

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
                QuestionSetId = questionSet.Id
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
                QuestionSetId = questionSet.Id
            };
            await question2.Create(DbContext).ConfigureAwait(false);

            Option option = new Option
            {
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id
            };
            await option.Create(DbContext).ConfigureAwait(false);

            Option option2 = new Option
            {
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id
            };
            await option2.Create(DbContext).ConfigureAwait(false);

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

            OptionTranslation optionTranslation = new OptionTranslation
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await optionTranslation.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = optionTranslation.LanguageId;
            var oldOptionId = optionTranslation.OptionId;
            var oldName = optionTranslation.Name;
            var oldMicrotingUid = optionTranslation.MicrotingUid;

            optionTranslation.Name = Guid.NewGuid().ToString();
            optionTranslation.LanguageId = language2.Id;
            optionTranslation.MicrotingUid = rnd.Next(1, 255);
            optionTranslation.OptionId = option2.Id;

            // Act
            await optionTranslation.Update(DbContext).ConfigureAwait(false);

            List<OptionTranslation> optionTranslations = DbContext.OptionTranslations.AsNoTracking().ToList();
            List<OptionTranslationVersion> optionTranslationVersions =
                DbContext.OptionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);

            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(2, optionTranslationVersions.Count);

            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslations[0].MicrotingUid);

            Assert.AreEqual(oldName, optionTranslationVersions[0].Name);
            Assert.AreEqual(oldOptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(oldLanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldMicrotingUid, optionTranslationVersions[0].MicrotingUid);

            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[1].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[1].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[1].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslationVersions[1].MicrotingUid);
        }

        [Test]
        public async Task OptionTranslation_Update_DoesUpdate_WO_MicrotingUid()
        {
            //Arrange

            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSet2 = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet2.Create(DbContext).ConfigureAwait(false);

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
                QuestionSetId = questionSet.Id
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
                QuestionSetId = questionSet.Id
            };
            await question2.Create(DbContext).ConfigureAwait(false);

            Option option = new Option
            {
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id
            };
            await option.Create(DbContext).ConfigureAwait(false);

            Option option2 = new Option
            {
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id
            };
            await option2.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            Language language2 = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
            };
            await language2.Create(DbContext).ConfigureAwait(false);

            OptionTranslation optionTranslation = new OptionTranslation
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString()
            };
            await optionTranslation.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = optionTranslation.LanguageId;
            var oldOptionId = optionTranslation.OptionId;
            var oldName = optionTranslation.Name;

            optionTranslation.Name = Guid.NewGuid().ToString();
            optionTranslation.LanguageId = language2.Id;
            optionTranslation.OptionId = option2.Id;

            // Act
            await optionTranslation.Update(DbContext).ConfigureAwait(false);

            List<OptionTranslation> optionTranslations = DbContext.OptionTranslations.AsNoTracking().ToList();
            List<OptionTranslationVersion> optionTranslationVersions =
                DbContext.OptionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);

            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(2, optionTranslationVersions.Count);

            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslations[0].MicrotingUid);

            Assert.AreEqual(oldName, optionTranslationVersions[0].Name);
            Assert.AreEqual(oldOptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(oldLanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(null, optionTranslationVersions[0].MicrotingUid);

            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[1].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[1].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[1].LanguageId);
            Assert.AreEqual(null, optionTranslationVersions[1].MicrotingUid);
        }

        [Test]
        public async Task OptionTranslation_Update_DoesUpdate_W_MicrotingUid_RemovesUid()
        {
            //Arrange

            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSet2 = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet2.Create(DbContext).ConfigureAwait(false);

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
                QuestionSetId = questionSet.Id
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
                QuestionSetId = questionSet.Id
            };
            await question2.Create(DbContext).ConfigureAwait(false);

            Option option = new Option
            {
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id
            };
            await option.Create(DbContext).ConfigureAwait(false);

            Option option2 = new Option
            {
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id
            };
            await option2.Create(DbContext).ConfigureAwait(false);

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

            OptionTranslation optionTranslation = new OptionTranslation
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await optionTranslation.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = optionTranslation.LanguageId;
            var oldOptionId = optionTranslation.OptionId;
            var oldName = optionTranslation.Name;
            var oldMicrotingUid = optionTranslation.MicrotingUid;

            optionTranslation.Name = Guid.NewGuid().ToString();
            optionTranslation.LanguageId = language2.Id;
            optionTranslation.MicrotingUid = null;
            optionTranslation.OptionId = option2.Id;

            // Act
            await optionTranslation.Update(DbContext).ConfigureAwait(false);

            List<OptionTranslation> optionTranslations = DbContext.OptionTranslations.AsNoTracking().ToList();
            List<OptionTranslationVersion> optionTranslationVersions =
                DbContext.OptionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);

            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(2, optionTranslationVersions.Count);

            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(null, optionTranslations[0].MicrotingUid);

            Assert.AreEqual(oldName, optionTranslationVersions[0].Name);
            Assert.AreEqual(oldOptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(oldLanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldMicrotingUid, optionTranslationVersions[0].MicrotingUid);

            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[1].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[1].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[1].LanguageId);
            Assert.AreEqual(null, optionTranslationVersions[1].MicrotingUid);
        }

        [Test]
        public async Task OptionTranslation_Update_DoesUpdate_WO_MicrotingUid_AddsUid()
        {
            //Arrange

            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSet2 = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet2.Create(DbContext).ConfigureAwait(false);

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
                QuestionSetId = questionSet.Id
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
                QuestionSetId = questionSet.Id
            };
            await question2.Create(DbContext).ConfigureAwait(false);

            Option option = new Option
            {
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id
            };
            await option.Create(DbContext).ConfigureAwait(false);

            Option option2 = new Option
            {
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id
            };
            await option2.Create(DbContext).ConfigureAwait(false);

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

            OptionTranslation optionTranslation = new OptionTranslation
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString()
            };
            await optionTranslation.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = optionTranslation.LanguageId;
            var oldOptionId = optionTranslation.OptionId;
            var oldName = optionTranslation.Name;

            optionTranslation.Name = Guid.NewGuid().ToString();
            optionTranslation.LanguageId = language2.Id;
            optionTranslation.OptionId = option2.Id;
            optionTranslation.MicrotingUid = rnd.Next(1, 255);

            // Act
            await optionTranslation.Update(DbContext).ConfigureAwait(false);

            List<OptionTranslation> optionTranslations = DbContext.OptionTranslations.AsNoTracking().ToList();
            List<OptionTranslationVersion> optionTranslationVersions =
                DbContext.OptionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);

            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(2, optionTranslationVersions.Count);

            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslations[0].MicrotingUid);

            Assert.AreEqual(oldName, optionTranslationVersions[0].Name);
            Assert.AreEqual(oldOptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(oldLanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(null, optionTranslationVersions[0].MicrotingUid);

            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[1].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[1].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[1].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslationVersions[1].MicrotingUid);
        }

        [Test]
        public async Task OptionTranslation_Delete_DoesDelete()
        {
            //Arrange

            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet.Create(DbContext).ConfigureAwait(false);

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
                QuestionSetId = questionSet.Id
            };
            await question.Create(DbContext).ConfigureAwait(false);


            Option option = new Option
            {
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id
            };
            await option.Create(DbContext).ConfigureAwait(false);


            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);


            OptionTranslation optionTranslation = new OptionTranslation
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await optionTranslation.Create(DbContext).ConfigureAwait(false);

            var oldLanguageId = optionTranslation.LanguageId;
            var oldOptionId = optionTranslation.OptionId;
            var oldName = optionTranslation.Name;
            var oldMicrotingUid = optionTranslation.MicrotingUid;

            // Act
            await optionTranslation.Delete(DbContext);

            List<OptionTranslation> optionTranslations = DbContext.OptionTranslations.AsNoTracking().ToList();
            List<OptionTranslationVersion> optionTranslationVersions =
                DbContext.OptionTranslationVersions.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);

            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(2, optionTranslationVersions.Count);

            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslations[0].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Removed, optionTranslations[0].WorkflowState);


            Assert.AreEqual(oldName, optionTranslationVersions[0].Name);
            Assert.AreEqual(oldOptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(oldLanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldMicrotingUid, optionTranslationVersions[0].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Created, optionTranslationVersions[0].WorkflowState);

            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[1].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[1].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[1].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslationVersions[1].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Removed, optionTranslationVersions[1].WorkflowState);
        }
    }
}