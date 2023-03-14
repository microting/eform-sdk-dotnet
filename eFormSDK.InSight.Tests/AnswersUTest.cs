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
    public class AnswersUTest : DbTestFixture
    {
        [Test]
        public async Task Answers_Create_DoesCreate()
        {
            //Arrange
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Site siteForUnit = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await siteForUnit.Create(DbContext).ConfigureAwait(false);

            Unit unit = new Unit
            {
                SiteId = siteForUnit.Id,
                CustomerNo = rnd.Next(1, 255),
                MicrotingUid = rnd.Next(1, 255),
                OtpCode = rnd.Next(1, 255)
            };
            await unit.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet.Create(DbContext).ConfigureAwait(false);

            SurveyConfiguration surveyConfiguration = new SurveyConfiguration
            {
                Name = Guid.NewGuid().ToString(),
                Start = DateTime.UtcNow,
                Stop = DateTime.UtcNow,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(DbContext).ConfigureAwait(false);

            Answer answer = new Answer
            {
                AnswerDuration = rnd.Next(1, 255),
                FinishedAt = DateTime.UtcNow,
                LanguageId = language.Id,
                Language = language,
                SiteId = site.Id,
                UnitId = unit.Id,
                QuestionSetId = questionSet.Id,
                SurveyConfigurationId = surveyConfiguration.Id,
                TimeZone = Guid.NewGuid().ToString(),
                UtcAdjusted = randomBool
            };

            //Act

            await answer.Create(DbContext).ConfigureAwait(false);

            List<Answer> answers = DbContext.Answers.AsNoTracking().ToList();
            List<AnswerVersion> answerVersions = DbContext.AnswerVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(answers);
            Assert.NotNull(answerVersions);

            Assert.AreEqual(1, answers.Count());
            Assert.AreEqual(1, answerVersions.Count());

            Assert.AreEqual(answer.CreatedAt.ToString(), answers[0].CreatedAt.ToString());
            Assert.AreEqual(answer.Version, answers[0].Version);
//            Assert.AreEqual(answer.UpdatedAt.ToString(), answers[0].UpdatedAt.ToString());
            Assert.AreEqual(answers[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(answer.Id, answers[0].Id);
            Assert.AreEqual(answer.AnswerDuration, answers[0].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt.ToString(), answers[0].FinishedAt.ToString());
            Assert.AreEqual(answer.LanguageId, language.Id);
            Assert.AreEqual(answer.SiteId, site.Id);
            Assert.AreEqual(answer.TimeZone, answers[0].TimeZone);
            Assert.AreEqual(answer.UnitId, unit.Id);
            Assert.AreEqual(answer.UtcAdjusted, answers[0].UtcAdjusted);
            Assert.AreEqual(answer.QuestionSetId, questionSet.Id);
            Assert.AreEqual(answer.SurveyConfigurationId, surveyConfiguration.Id);


            //Version 1
            Assert.AreEqual(answer.CreatedAt.ToString(), answerVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, answerVersions[0].Version);
//            Assert.AreEqual(answer.UpdatedAt.ToString(), answerVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(answerVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(answer.Id, answerVersions[0].AnswerId);
            Assert.AreEqual(answer.AnswerDuration, answerVersions[0].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt.ToString(), answerVersions[0].FinishedAt.ToString());
            Assert.AreEqual(language.Id, answerVersions[0].LanguageId);
            Assert.AreEqual(site.Id, answerVersions[0].SiteId);
            Assert.AreEqual(answer.TimeZone, answerVersions[0].TimeZone);
            Assert.AreEqual(unit.Id, answerVersions[0].UnitId);
            Assert.AreEqual(answer.UtcAdjusted, answerVersions[0].UtcAdjusted);
            Assert.AreEqual(questionSet.Id, answerVersions[0].QuestionSetId);
            Assert.AreEqual(surveyConfiguration.Id, answerVersions[0].SurveyConfigurationId);
        }

        [Test]
        public async Task Answer_update_DoesUpdate()
        {
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Site siteForUnit = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await siteForUnit.Create(DbContext).ConfigureAwait(false);

            Unit unit = new Unit
            {
                SiteId = siteForUnit.Id,
                CustomerNo = rnd.Next(1, 255),
                MicrotingUid = rnd.Next(1, 255),
                OtpCode = rnd.Next(1, 255)
            };
            await unit.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };
            await questionSet.Create(DbContext).ConfigureAwait(false);

            SurveyConfiguration surveyConfiguration = new SurveyConfiguration
            {
                Name = Guid.NewGuid().ToString(),
                Start = DateTime.UtcNow,
                Stop = DateTime.UtcNow,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(DbContext).ConfigureAwait(false);

            Answer answer = new Answer
            {
                AnswerDuration = rnd.Next(1, 255),
                FinishedAt = DateTime.UtcNow,
                LanguageId = language.Id,
                SiteId = site.Id,
                SurveyConfiguration = surveyConfiguration,
                TimeZone = Guid.NewGuid().ToString(),
                UnitId = unit.Id,
                UtcAdjusted = randomBool,
                QuestionSetId = questionSet.Id,
                SurveyConfigurationId = surveyConfiguration.Id
            };
            await answer.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = answer.UpdatedAt;
            int oldAnswerDuration = answer.AnswerDuration;
            DateTime oldFinishedAt = answer.FinishedAt;
            string oldTimeZone = answer.TimeZone;
            bool oldUtcAdjusted = answer.UtcAdjusted;

            answer.AnswerDuration = rnd.Next(1, 255);
            answer.FinishedAt = DateTime.UtcNow;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.UtcAdjusted = randomBool;

            await answer.Update(DbContext).ConfigureAwait(false);

            List<Answer> answers = DbContext.Answers.AsNoTracking().ToList();
            List<AnswerVersion> answerVersions = DbContext.AnswerVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(answers);
            Assert.NotNull(answerVersions);

            Assert.AreEqual(1, answers.Count());
            Assert.AreEqual(2, answerVersions.Count());

            Assert.AreEqual(answer.CreatedAt.ToString(), answers[0].CreatedAt.ToString());
            Assert.AreEqual(answer.Version, answers[0].Version);
//            Assert.AreEqual(answer.UpdatedAt.ToString(), answers[0].UpdatedAt.ToString());
            Assert.AreEqual(answers[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(answer.Id, answers[0].Id);
            Assert.AreEqual(answer.AnswerDuration, answers[0].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt.ToString(), answers[0].FinishedAt.ToString());
            Assert.AreEqual(answer.LanguageId, language.Id);
            Assert.AreEqual(answer.SiteId, site.Id);
            Assert.AreEqual(answer.TimeZone, answers[0].TimeZone);
            Assert.AreEqual(answer.UnitId, unit.Id);
            Assert.AreEqual(answer.UtcAdjusted, answers[0].UtcAdjusted);
            Assert.AreEqual(answer.QuestionSetId, questionSet.Id);
            Assert.AreEqual(answer.SurveyConfigurationId, surveyConfiguration.Id);

            //Version 1 Old Version
            Assert.AreEqual(answer.CreatedAt.ToString(), answerVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, answerVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), answerVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(answerVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(oldAnswerDuration, answerVersions[0].AnswerDuration);
            Assert.AreEqual(oldFinishedAt.ToString(), answerVersions[0].FinishedAt.ToString());
            Assert.AreEqual(oldUtcAdjusted, answerVersions[0].UtcAdjusted);
            Assert.AreEqual(oldTimeZone, answerVersions[0].TimeZone);


            //Version 2 Updated Version
            Assert.AreEqual(answer.CreatedAt.ToString(), answerVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, answerVersions[1].Version);
//            Assert.AreEqual(answer.UpdatedAt.ToString(), answerVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(answerVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(answer.Id, answerVersions[1].AnswerId);
            Assert.AreEqual(answer.AnswerDuration, answerVersions[1].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt.ToString(), answerVersions[1].FinishedAt.ToString());
            Assert.AreEqual(language.Id, answerVersions[1].LanguageId);
            Assert.AreEqual(site.Id, answerVersions[1].SiteId);
            Assert.AreEqual(answer.TimeZone, answerVersions[1].TimeZone);
            Assert.AreEqual(unit.Id, answerVersions[1].UnitId);
            Assert.AreEqual(answer.UtcAdjusted, answerVersions[1].UtcAdjusted);
            Assert.AreEqual(questionSet.Id, answerVersions[1].QuestionSetId);
            Assert.AreEqual(surveyConfiguration.Id, answerVersions[1].SurveyConfigurationId);
        }

        [Test]
        public async Task Answer_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange

            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Site siteForUnit = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await siteForUnit.Create(DbContext).ConfigureAwait(false);

            Unit unit = new Unit
            {
                CustomerNo = rnd.Next(1, 255),
                MicrotingUid = rnd.Next(1, 255),
                OtpCode = rnd.Next(1, 255),
                SiteId = siteForUnit.Id
            };
            await unit.Create(DbContext).ConfigureAwait(false);

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSet = new QuestionSet
            {
                Name = Guid.NewGuid().ToString(),
                Share = randomBool,
                HasChild = randomBool,
                PossiblyDeployed = randomBool
            };

            await questionSet.Create(DbContext).ConfigureAwait(false);

            SurveyConfiguration surveyConfiguration = new SurveyConfiguration
            {
                Name = Guid.NewGuid().ToString(),
                Start = DateTime.UtcNow,
                Stop = DateTime.UtcNow,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(DbContext).ConfigureAwait(false);

            Answer answer = new Answer
            {
                AnswerDuration = rnd.Next(1, 255),
                FinishedAt = DateTime.UtcNow,
                LanguageId = language.Id,
                SiteId = site.Id,
                TimeZone = Guid.NewGuid().ToString(),
                UnitId = unit.Id,
                UtcAdjusted = randomBool,
                QuestionSetId = questionSet.Id,
                SurveyConfigurationId = surveyConfiguration.Id
            };
            await answer.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = answer.UpdatedAt;

            await answer.Delete(DbContext);

            List<Answer> answers = DbContext.Answers.AsNoTracking().ToList();
            List<AnswerVersion> answerVersions = DbContext.AnswerVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(answers);
            Assert.NotNull(answerVersions);

            Assert.AreEqual(1, answers.Count());
            Assert.AreEqual(2, answerVersions.Count());

            Assert.AreEqual(answer.CreatedAt.ToString(), answers[0].CreatedAt.ToString());
            Assert.AreEqual(answer.Version, answers[0].Version);
//            Assert.AreEqual(answer.UpdatedAt.ToString(), answers[0].UpdatedAt.ToString());
            Assert.AreEqual(answers[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(answer.Id, answers[0].Id);
            Assert.AreEqual(answer.AnswerDuration, answers[0].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt.ToString(), answers[0].FinishedAt.ToString());
            Assert.AreEqual(answer.LanguageId, language.Id);
            Assert.AreEqual(answer.SiteId, site.Id);
            Assert.AreEqual(answer.TimeZone, answers[0].TimeZone);
            Assert.AreEqual(answer.UnitId, unit.Id);
            Assert.AreEqual(answer.UtcAdjusted, answers[0].UtcAdjusted);
            Assert.AreEqual(answer.QuestionSetId, questionSet.Id);
            Assert.AreEqual(answer.SurveyConfigurationId, surveyConfiguration.Id);

            //Version 1 Old Version
            Assert.AreEqual(answer.CreatedAt.ToString(), answerVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, answerVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), answerVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(answerVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(answer.Id, answerVersions[0].AnswerId);
            Assert.AreEqual(answer.AnswerDuration, answerVersions[0].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt.ToString(), answerVersions[0].FinishedAt.ToString());
            Assert.AreEqual(language.Id, answerVersions[0].LanguageId);
            Assert.AreEqual(site.Id, answerVersions[0].SiteId);
            Assert.AreEqual(answer.TimeZone, answerVersions[0].TimeZone);
            Assert.AreEqual(unit.Id, answerVersions[0].UnitId);
            Assert.AreEqual(answer.UtcAdjusted, answerVersions[0].UtcAdjusted);
            Assert.AreEqual(questionSet.Id, answerVersions[0].QuestionSetId);
            Assert.AreEqual(surveyConfiguration.Id, answerVersions[0].SurveyConfigurationId);

            //Version 2 Deleted Version
            Assert.AreEqual(answer.CreatedAt.ToString(), answerVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, answerVersions[1].Version);
//            Assert.AreEqual(answer.UpdatedAt.ToString(), answerVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(answer.Id, answerVersions[1].AnswerId);
            Assert.AreEqual(answer.AnswerDuration, answerVersions[1].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt.ToString(), answerVersions[1].FinishedAt.ToString());
            Assert.AreEqual(language.Id, answerVersions[1].LanguageId);
            Assert.AreEqual(site.Id, answerVersions[1].SiteId);
            Assert.AreEqual(answer.TimeZone, answerVersions[1].TimeZone);
            Assert.AreEqual(unit.Id, answerVersions[1].UnitId);
            Assert.AreEqual(answer.UtcAdjusted, answerVersions[1].UtcAdjusted);
            Assert.AreEqual(questionSet.Id, answerVersions[1].QuestionSetId);
            Assert.AreEqual(surveyConfiguration.Id, answerVersions[1].SurveyConfigurationId);
            Assert.AreEqual(answerVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
        }
    }
}