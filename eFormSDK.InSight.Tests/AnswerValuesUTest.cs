/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

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
public class AnswerValuesUTest : DbTestFixture
{
    [Test]
    public async Task AnswerValues_Create_DoesCreate()
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
            QuestionSetId = questionSet.Id,
            SiteId = site.Id,
            TimeZone = Guid.NewGuid().ToString(),
            UnitId = unit.Id,
            UtcAdjusted = randomBool
        };
        answer.QuestionSetId = questionSet.Id;
        answer.SurveyConfigurationId = surveyConfiguration.Id;
        await answer.Create(DbContext).ConfigureAwait(false);

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
            QuestionSetId = questionSet.Id
        };
        await question.Create(DbContext).ConfigureAwait(false);

        Option option = new Option
        {
            Question = question,
            Weight = rnd.Next(1, 255),
            OptionIndex = rnd.Next(1, 255),
            QuestionId = question.Id,
            WeightValue = rnd.Next(1, 255),
            ContinuousOptionId = rnd.Next(1, 255)
        };
        await option.Create(DbContext).ConfigureAwait(false);

        Question questionForAnswerValue = new Question
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
            QuestionSetId = questionSet.Id
        };
        await questionForAnswerValue.Create(DbContext).ConfigureAwait(false);

        AnswerValue answerValue = new AnswerValue
        {
            Value = rnd.Next(1, 255).ToString(),
            AnswerId = answer.Id,
            OptionId = option.Id,
            QuestionId = question.Id
        };

        //Act

        await answerValue.Create(DbContext).ConfigureAwait(false);

        List<AnswerValue> answerValues = DbContext.AnswerValues.AsNoTracking().ToList();
        List<AnswerValueVersion> answerValueVersions = DbContext.AnswerValueVersions.AsNoTracking().ToList();

        //Assert

        Assert.That(answerValues, Is.Not.EqualTo(null));
        Assert.That(answerValueVersions, Is.Not.EqualTo(null));

        Assert.That(answerValues.Count(), Is.EqualTo(1));
        Assert.That(answerValueVersions.Count(), Is.EqualTo(1));

        Assert.That(answerValues[0].CreatedAt.ToString(), Is.EqualTo(answerValue.CreatedAt.ToString()));
        Assert.That(answerValues[0].Version, Is.EqualTo(answerValue.Version));
        //             Assert.AreEqual(answerValue.UpdatedAt.ToString(), answerValues[0].UpdatedAt.ToString());
        Assert.That(answerValues[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(answerValues[0].Value, Is.EqualTo(answerValue.Value));
        Assert.That(answerValues[0].Id, Is.EqualTo(answerValue.Id));
        Assert.That(answerValue.AnswerId, Is.EqualTo(answer.Id));
        Assert.That(answerValue.OptionId, Is.EqualTo(option.Id));
        Assert.That(answerValue.QuestionId, Is.EqualTo(question.Id));

        //Versions
        Assert.That(answerValueVersions[0].CreatedAt.ToString(), Is.EqualTo(answerValue.CreatedAt.ToString()));
        Assert.That(answerValueVersions[0].Version, Is.EqualTo(1));
        //             Assert.AreEqual(answerValue.UpdatedAt.ToString(), answerValueVersions[0].UpdatedAt.ToString());
        Assert.That(answerValueVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(answerValueVersions[0].AnswerValueId, Is.EqualTo(answerValue.Id));
        Assert.That(answerValueVersions[0].Value, Is.EqualTo(answerValue.Value));
        Assert.That(answerValueVersions[0].AnswerId, Is.EqualTo(answer.Id));
        Assert.That(answerValueVersions[0].OptionId, Is.EqualTo(option.Id));
        Assert.That(answerValueVersions[0].QuestionId, Is.EqualTo(question.Id));
    }

    [Test]
    public async Task AnswerValues_Update_DoesUpdate()
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
            TimeZone = Guid.NewGuid().ToString(),
            UnitId = unit.Id,
            UtcAdjusted = randomBool,
            QuestionSetId = questionSet.Id,
            SurveyConfigurationId = surveyConfiguration.Id
        };
        await answer.Create(DbContext).ConfigureAwait(false);

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
            QuestionSetId = questionSet.Id
        };
        await question.Create(DbContext).ConfigureAwait(false);

        Option option = new Option
        {
            Question = question,
            Weight = rnd.Next(1, 255),
            OptionIndex = rnd.Next(1, 255),
            QuestionId = question.Id,
            WeightValue = rnd.Next(1, 255),
            ContinuousOptionId = rnd.Next(1, 255)
        };
        await option.Create(DbContext).ConfigureAwait(false);

        Question questionForAnswerValue = new Question
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
            QuestionSetId = questionSet.Id
        };
        await questionForAnswerValue.Create(DbContext).ConfigureAwait(false);

        AnswerValue answerValue = new AnswerValue
        {
            Value = rnd.Next(1, 255).ToString(),
            AnswerId = answer.Id,
            OptionId = option.Id,
            QuestionId = question.Id
        };
        await answerValue.Create(DbContext).ConfigureAwait(false);

        //Act

        DateTime? oldUpdatedAt = answerValue.UpdatedAt;
        string oldValue = answerValue.Value;

        answerValue.Value = rnd.Next(1, 255).ToString();

        await answerValue.Update(DbContext).ConfigureAwait(false);

        List<AnswerValue> answerValues =
            await DbContext.AnswerValues.AsNoTracking().ToListAsync().ConfigureAwait(false);
        List<AnswerValueVersion> answerValueVersions =
            await DbContext.AnswerValueVersions.AsNoTracking().ToListAsync().ConfigureAwait(false);

        //Assert

        Assert.That(answerValues, Is.Not.EqualTo(null));
        Assert.That(answerValueVersions, Is.Not.EqualTo(null));

        Assert.That(answerValues.Count(), Is.EqualTo(1));
        Assert.That(answerValueVersions.Count(), Is.EqualTo(2));

        Assert.That(answerValues[0].CreatedAt.ToString(), Is.EqualTo(answerValue.CreatedAt.ToString()));
        Assert.That(answerValues[0].Version, Is.EqualTo(answerValue.Version));
        //             Assert.AreEqual(answerValue.UpdatedAt.ToString(), answerValues[0].UpdatedAt.ToString());
        Assert.That(answerValues[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(answerValues[0].Value, Is.EqualTo(answerValue.Value));
        Assert.That(answerValues[0].Id, Is.EqualTo(answerValue.Id));
        Assert.That(answer.Id, Is.EqualTo(answerValue.AnswerId));
        Assert.That(option.Id, Is.EqualTo(answerValue.OptionId));
        Assert.That(question.Id, Is.EqualTo(answerValue.QuestionId));

        //Old Version
        Assert.That(answerValueVersions[0].CreatedAt.ToString(), Is.EqualTo(answerValue.CreatedAt.ToString()));
        Assert.That(answerValueVersions[0].Version, Is.EqualTo(1));
        //             Assert.AreEqual(oldUpdatedAt.ToString(), answerValueVersions[0].UpdatedAt.ToString());
        Assert.That(answerValueVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(answerValueVersions[0].AnswerValueId, Is.EqualTo(answerValue.Id));
        Assert.That(answerValueVersions[0].Value, Is.EqualTo(oldValue));
        Assert.That(answerValueVersions[0].AnswerId, Is.EqualTo(answer.Id));
        Assert.That(answerValueVersions[0].OptionId, Is.EqualTo(option.Id));
        Assert.That(answerValueVersions[0].QuestionId, Is.EqualTo(question.Id));

        //New Version
        Assert.That(answerValueVersions[1].CreatedAt.ToString(), Is.EqualTo(answerValue.CreatedAt.ToString()));
        Assert.That(answerValueVersions[1].Version, Is.EqualTo(2));
        //             Assert.AreEqual(answerValue.UpdatedAt.ToString(), answerValueVersions[1].UpdatedAt.ToString());
        Assert.That(answerValueVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(answerValueVersions[1].AnswerValueId, Is.EqualTo(answerValue.Id));
        Assert.That(answerValueVersions[1].Value, Is.EqualTo(answerValue.Value));
        Assert.That(answerValueVersions[1].AnswerId, Is.EqualTo(answer.Id));
        Assert.That(answerValueVersions[1].OptionId, Is.EqualTo(option.Id));
        Assert.That(answerValueVersions[1].QuestionId, Is.EqualTo(question.Id));
    }

    [Test]
    public async Task AnswerValues_Delete_DoesSetWorkflowStateToRemoved()
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
            QuestionSetId = questionSet.Id
        };
        await question.Create(DbContext).ConfigureAwait(false);

        Option option = new Option
        {
            Weight = rnd.Next(1, 255),
            OptionIndex = rnd.Next(1, 255),
            QuestionId = question.Id,
            WeightValue = rnd.Next(1, 255),
            ContinuousOptionId = rnd.Next(1, 255)
        };
        await option.Create(DbContext).ConfigureAwait(false);

        Question questionForAnswerValue = new Question
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
            QuestionSetId = questionSet.Id
        };
        await questionForAnswerValue.Create(DbContext).ConfigureAwait(false);

        AnswerValue answerValue = new AnswerValue
        {
            Value = rnd.Next(1, 255).ToString(),
            AnswerId = answer.Id,
            OptionId = option.Id,
            QuestionId = question.Id
        };
        await answerValue.Create(DbContext).ConfigureAwait(false);

        //Act

        DateTime? oldUpdatedAt = answerValue.UpdatedAt;

        await answerValue.Delete(DbContext);


        List<AnswerValue> answerValues = DbContext.AnswerValues.AsNoTracking().ToList();
        List<AnswerValueVersion> answerValueVersions = DbContext.AnswerValueVersions.AsNoTracking().ToList();

        //Assert

        Assert.That(answerValues, Is.Not.EqualTo(null));
        Assert.That(answerValueVersions, Is.Not.EqualTo(null));

        Assert.That(answerValues.Count(), Is.EqualTo(1));
        Assert.That(answerValueVersions.Count(), Is.EqualTo(2));

        Assert.That(answerValues[0].CreatedAt.ToString(), Is.EqualTo(answerValue.CreatedAt.ToString()));
        Assert.That(answerValues[0].Version, Is.EqualTo(answerValue.Version));
        //             Assert.AreEqual(answerValue.UpdatedAt.ToString(), answerValues[0].UpdatedAt.ToString());
        Assert.That(answerValues[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(answerValues[0].Value, Is.EqualTo(answerValue.Value));
        Assert.That(answerValues[0].Id, Is.EqualTo(answerValue.Id));
        Assert.That(answer.Id, Is.EqualTo(answerValue.AnswerId));
        Assert.That(option.Id, Is.EqualTo(answerValue.OptionId));
        Assert.That(question.Id, Is.EqualTo(answerValue.QuestionId));

        //Old Version
        Assert.That(answerValueVersions[0].CreatedAt.ToString(), Is.EqualTo(answerValue.CreatedAt.ToString()));
        Assert.That(answerValueVersions[0].Version, Is.EqualTo(1));
        //             Assert.AreEqual(oldUpdatedAt.ToString(), answerValueVersions[0].UpdatedAt.ToString());
        Assert.That(answerValueVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(answerValueVersions[0].AnswerValueId, Is.EqualTo(answerValue.Id));
        Assert.That(answerValueVersions[0].Value, Is.EqualTo(answerValue.Value));
        Assert.That(answerValueVersions[0].AnswerId, Is.EqualTo(answer.Id));
        Assert.That(answerValueVersions[0].OptionId, Is.EqualTo(option.Id));
        Assert.That(answerValueVersions[0].QuestionId, Is.EqualTo(question.Id));

        //New Version
        Assert.That(answerValueVersions[1].CreatedAt.ToString(), Is.EqualTo(answerValue.CreatedAt.ToString()));
        Assert.That(answerValueVersions[1].Version, Is.EqualTo(2));
        //             Assert.AreEqual(answerValue.UpdatedAt.ToString(), answerValueVersions[1].UpdatedAt.ToString());
        Assert.That(answerValueVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(answerValueVersions[1].AnswerValueId, Is.EqualTo(answerValue.Id));
        Assert.That(answerValueVersions[1].Value, Is.EqualTo(answerValue.Value));
        Assert.That(answerValueVersions[1].AnswerId, Is.EqualTo(answer.Id));
        Assert.That(answerValueVersions[1].OptionId, Is.EqualTo(option.Id));
        Assert.That(answerValueVersions[1].QuestionId, Is.EqualTo(question.Id));
    }
}