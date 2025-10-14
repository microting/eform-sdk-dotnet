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

namespace eFormSDK.InSight.Tests;

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

        Assert.That(answers, Is.Not.EqualTo(null));
        Assert.That(answerVersions, Is.Not.EqualTo(null));

        Assert.That(answers.Count(), Is.EqualTo(1));
        Assert.That(answerVersions.Count(), Is.EqualTo(1));

        Assert.That(answers[0].CreatedAt.ToString(), Is.EqualTo(answer.CreatedAt.ToString()));
        Assert.That(answers[0].Version, Is.EqualTo(answer.Version));
        //            Assert.AreEqual(answer.UpdatedAt.ToString(), answers[0].UpdatedAt.ToString());
        Assert.That(answers[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(answers[0].Id, Is.EqualTo(answer.Id));
        Assert.That(answers[0].AnswerDuration, Is.EqualTo(answer.AnswerDuration));
        Assert.That(answers[0].FinishedAt.ToString(), Is.EqualTo(answer.FinishedAt.ToString()));
        Assert.That(language.Id, Is.EqualTo(answer.LanguageId));
        Assert.That(site.Id, Is.EqualTo(answer.SiteId));
        Assert.That(answers[0].TimeZone, Is.EqualTo(answer.TimeZone));
        Assert.That(unit.Id, Is.EqualTo(answer.UnitId));
        Assert.That(answers[0].UtcAdjusted, Is.EqualTo(answer.UtcAdjusted));
        Assert.That(questionSet.Id, Is.EqualTo(answer.QuestionSetId));
        Assert.That(surveyConfiguration.Id, Is.EqualTo(answer.SurveyConfigurationId));


        //Version 1
        Assert.That(answerVersions[0].CreatedAt.ToString(), Is.EqualTo(answer.CreatedAt.ToString()));
        Assert.That(answerVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(answer.UpdatedAt.ToString(), answerVersions[0].UpdatedAt.ToString());
        Assert.That(answerVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(answerVersions[0].AnswerId, Is.EqualTo(answer.Id));
        Assert.That(answerVersions[0].AnswerDuration, Is.EqualTo(answer.AnswerDuration));
        Assert.That(answerVersions[0].FinishedAt.ToString(), Is.EqualTo(answer.FinishedAt.ToString()));
        Assert.That(answerVersions[0].LanguageId, Is.EqualTo(language.Id));
        Assert.That(answerVersions[0].SiteId, Is.EqualTo(site.Id));
        Assert.That(answerVersions[0].TimeZone, Is.EqualTo(answer.TimeZone));
        Assert.That(answerVersions[0].UnitId, Is.EqualTo(unit.Id));
        Assert.That(answerVersions[0].UtcAdjusted, Is.EqualTo(answer.UtcAdjusted));
        Assert.That(answerVersions[0].QuestionSetId, Is.EqualTo(questionSet.Id));
        Assert.That(answerVersions[0].SurveyConfigurationId, Is.EqualTo(surveyConfiguration.Id));
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

        Assert.That(answers, Is.Not.EqualTo(null));
        Assert.That(answerVersions, Is.Not.EqualTo(null));

        Assert.That(answers.Count(), Is.EqualTo(1));
        Assert.That(answerVersions.Count(), Is.EqualTo(2));

        Assert.That(answers[0].CreatedAt.ToString(), Is.EqualTo(answer.CreatedAt.ToString()));
        Assert.That(answers[0].Version, Is.EqualTo(answer.Version));
        //            Assert.AreEqual(answer.UpdatedAt.ToString(), answers[0].UpdatedAt.ToString());
        Assert.That(answers[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(answers[0].Id, Is.EqualTo(answer.Id));
        Assert.That(answers[0].AnswerDuration, Is.EqualTo(answer.AnswerDuration));
        Assert.That(answers[0].FinishedAt.ToString(), Is.EqualTo(answer.FinishedAt.ToString()));
        Assert.That(language.Id, Is.EqualTo(answer.LanguageId));
        Assert.That(site.Id, Is.EqualTo(answer.SiteId));
        Assert.That(answers[0].TimeZone, Is.EqualTo(answer.TimeZone));
        Assert.That(unit.Id, Is.EqualTo(answer.UnitId));
        Assert.That(answers[0].UtcAdjusted, Is.EqualTo(answer.UtcAdjusted));
        Assert.That(questionSet.Id, Is.EqualTo(answer.QuestionSetId));
        Assert.That(surveyConfiguration.Id, Is.EqualTo(answer.SurveyConfigurationId));

        //Version 1 Old Version
        Assert.That(answerVersions[0].CreatedAt.ToString(), Is.EqualTo(answer.CreatedAt.ToString()));
        Assert.That(answerVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), answerVersions[0].UpdatedAt.ToString());
        Assert.That(answerVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(answerVersions[0].AnswerDuration, Is.EqualTo(oldAnswerDuration));
        Assert.That(answerVersions[0].FinishedAt.ToString(), Is.EqualTo(oldFinishedAt.ToString()));
        Assert.That(answerVersions[0].UtcAdjusted, Is.EqualTo(oldUtcAdjusted));
        Assert.That(answerVersions[0].TimeZone, Is.EqualTo(oldTimeZone));


        //Version 2 Updated Version
        Assert.That(answerVersions[1].CreatedAt.ToString(), Is.EqualTo(answer.CreatedAt.ToString()));
        Assert.That(answerVersions[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(answer.UpdatedAt.ToString(), answerVersions[1].UpdatedAt.ToString());
        Assert.That(answerVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(answerVersions[1].AnswerId, Is.EqualTo(answer.Id));
        Assert.That(answerVersions[1].AnswerDuration, Is.EqualTo(answer.AnswerDuration));
        Assert.That(answerVersions[1].FinishedAt.ToString(), Is.EqualTo(answer.FinishedAt.ToString()));
        Assert.That(answerVersions[1].LanguageId, Is.EqualTo(language.Id));
        Assert.That(answerVersions[1].SiteId, Is.EqualTo(site.Id));
        Assert.That(answerVersions[1].TimeZone, Is.EqualTo(answer.TimeZone));
        Assert.That(answerVersions[1].UnitId, Is.EqualTo(unit.Id));
        Assert.That(answerVersions[1].UtcAdjusted, Is.EqualTo(answer.UtcAdjusted));
        Assert.That(answerVersions[1].QuestionSetId, Is.EqualTo(questionSet.Id));
        Assert.That(answerVersions[1].SurveyConfigurationId, Is.EqualTo(surveyConfiguration.Id));
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

        Assert.That(answers, Is.Not.EqualTo(null));
        Assert.That(answerVersions, Is.Not.EqualTo(null));

        Assert.That(answers.Count(), Is.EqualTo(1));
        Assert.That(answerVersions.Count(), Is.EqualTo(2));

        Assert.That(answers[0].CreatedAt.ToString(), Is.EqualTo(answer.CreatedAt.ToString()));
        Assert.That(answers[0].Version, Is.EqualTo(answer.Version));
        //            Assert.AreEqual(answer.UpdatedAt.ToString(), answers[0].UpdatedAt.ToString());
        Assert.That(answers[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(answers[0].Id, Is.EqualTo(answer.Id));
        Assert.That(answers[0].AnswerDuration, Is.EqualTo(answer.AnswerDuration));
        Assert.That(answers[0].FinishedAt.ToString(), Is.EqualTo(answer.FinishedAt.ToString()));
        Assert.That(language.Id, Is.EqualTo(answer.LanguageId));
        Assert.That(site.Id, Is.EqualTo(answer.SiteId));
        Assert.That(answers[0].TimeZone, Is.EqualTo(answer.TimeZone));
        Assert.That(unit.Id, Is.EqualTo(answer.UnitId));
        Assert.That(answers[0].UtcAdjusted, Is.EqualTo(answer.UtcAdjusted));
        Assert.That(questionSet.Id, Is.EqualTo(answer.QuestionSetId));
        Assert.That(surveyConfiguration.Id, Is.EqualTo(answer.SurveyConfigurationId));

        //Version 1 Old Version
        Assert.That(answerVersions[0].CreatedAt.ToString(), Is.EqualTo(answer.CreatedAt.ToString()));
        Assert.That(answerVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), answerVersions[0].UpdatedAt.ToString());
        Assert.That(answerVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(answerVersions[0].AnswerId, Is.EqualTo(answer.Id));
        Assert.That(answerVersions[0].AnswerDuration, Is.EqualTo(answer.AnswerDuration));
        Assert.That(answerVersions[0].FinishedAt.ToString(), Is.EqualTo(answer.FinishedAt.ToString()));
        Assert.That(answerVersions[0].LanguageId, Is.EqualTo(language.Id));
        Assert.That(answerVersions[0].SiteId, Is.EqualTo(site.Id));
        Assert.That(answerVersions[0].TimeZone, Is.EqualTo(answer.TimeZone));
        Assert.That(answerVersions[0].UnitId, Is.EqualTo(unit.Id));
        Assert.That(answerVersions[0].UtcAdjusted, Is.EqualTo(answer.UtcAdjusted));
        Assert.That(answerVersions[0].QuestionSetId, Is.EqualTo(questionSet.Id));
        Assert.That(answerVersions[0].SurveyConfigurationId, Is.EqualTo(surveyConfiguration.Id));

        //Version 2 Deleted Version
        Assert.That(answerVersions[1].CreatedAt.ToString(), Is.EqualTo(answer.CreatedAt.ToString()));
        Assert.That(answerVersions[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(answer.UpdatedAt.ToString(), answerVersions[1].UpdatedAt.ToString());
        Assert.That(answerVersions[1].AnswerId, Is.EqualTo(answer.Id));
        Assert.That(answerVersions[1].AnswerDuration, Is.EqualTo(answer.AnswerDuration));
        Assert.That(answerVersions[1].FinishedAt.ToString(), Is.EqualTo(answer.FinishedAt.ToString()));
        Assert.That(answerVersions[1].LanguageId, Is.EqualTo(language.Id));
        Assert.That(answerVersions[1].SiteId, Is.EqualTo(site.Id));
        Assert.That(answerVersions[1].TimeZone, Is.EqualTo(answer.TimeZone));
        Assert.That(answerVersions[1].UnitId, Is.EqualTo(unit.Id));
        Assert.That(answerVersions[1].UtcAdjusted, Is.EqualTo(answer.UtcAdjusted));
        Assert.That(answerVersions[1].QuestionSetId, Is.EqualTo(questionSet.Id));
        Assert.That(answerVersions[1].SurveyConfigurationId, Is.EqualTo(surveyConfiguration.Id));
        Assert.That(answerVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
    }
}