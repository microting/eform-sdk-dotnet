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
public class OptionsUTest : DbTestFixture
{
    [Test]
    public async Task Options_Create_DoesCreate()
    {
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

        //Act

        await option.Create(DbContext).ConfigureAwait(false);

        List<Option> options = DbContext.Options.AsNoTracking().ToList();
        List<OptionVersion> optionVersions = DbContext.OptionVersions.AsNoTracking().ToList();

        Assert.That(options, Is.Not.EqualTo(null));
        Assert.That(optionVersions, Is.Not.EqualTo(null));

        Assert.That(options.Count(), Is.EqualTo(1));
        Assert.That(optionVersions.Count(), Is.EqualTo(1));

        Assert.That(options[0].CreatedAt.ToString(), Is.EqualTo(option.CreatedAt.ToString()));
        Assert.That(options[0].Version, Is.EqualTo(option.Version));
        //            Assert.AreEqual(option.UpdatedAt.ToString(), options[0].UpdatedAt.ToString());
        Assert.That(options[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(options[0].Id, Is.EqualTo(option.Id));
        Assert.That(options[0].Weight, Is.EqualTo(option.Weight));
        Assert.That(options[0].OptionIndex, Is.EqualTo(option.OptionIndex));
        Assert.That(options[0].WeightValue, Is.EqualTo(option.WeightValue));
        Assert.That(question.Id, Is.EqualTo(option.QuestionId));

        Assert.That(optionVersions[0].CreatedAt.ToString(), Is.EqualTo(option.CreatedAt.ToString()));
        Assert.That(optionVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(option.UpdatedAt.ToString(), optionVersions[0].UpdatedAt.ToString());
        Assert.That(optionVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(optionVersions[0].OptionId, Is.EqualTo(option.Id));
        Assert.That(optionVersions[0].Weight, Is.EqualTo(option.Weight));
        Assert.That(optionVersions[0].OptionIndex, Is.EqualTo(option.OptionIndex));
        Assert.That(optionVersions[0].WeightValue, Is.EqualTo(option.WeightValue));
        Assert.That(optionVersions[0].QuestionId, Is.EqualTo(question.Id));
    }

    [Test]
    public async Task Options_Update_DoesUpdate()
    {
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

        //Act

        DateTime? oldUpdatedAt = option.UpdatedAt;
        int oldWeight = option.Weight;
        int oldOptionsIndex = option.OptionIndex;
        int oldWeightValue = option.WeightValue;

        option.Weight = rnd.Next(1, 255);
        option.OptionIndex = rnd.Next(1, 255);
        option.WeightValue = rnd.Next(1, 255);
        await option.Update(DbContext).ConfigureAwait(false);

        List<Option> options = DbContext.Options.AsNoTracking().ToList();
        List<OptionVersion> optionVersions = DbContext.OptionVersions.AsNoTracking().ToList();

        Assert.That(options, Is.Not.EqualTo(null));
        Assert.That(optionVersions, Is.Not.EqualTo(null));

        Assert.That(options.Count(), Is.EqualTo(1));
        Assert.That(optionVersions.Count(), Is.EqualTo(2));

        Assert.That(options[0].CreatedAt.ToString(), Is.EqualTo(option.CreatedAt.ToString()));
        Assert.That(options[0].Version, Is.EqualTo(option.Version));
        //            Assert.AreEqual(option.UpdatedAt.ToString(), options[0].UpdatedAt.ToString());
        Assert.That(options[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(options[0].Id, Is.EqualTo(option.Id));
        Assert.That(options[0].Weight, Is.EqualTo(option.Weight));
        Assert.That(options[0].OptionIndex, Is.EqualTo(option.OptionIndex));
        Assert.That(options[0].WeightValue, Is.EqualTo(option.WeightValue));
        Assert.That(question.Id, Is.EqualTo(option.QuestionId));

        //Old Version
        Assert.That(optionVersions[0].CreatedAt.ToString(), Is.EqualTo(option.CreatedAt.ToString()));
        Assert.That(optionVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), optionVersions[0].UpdatedAt.ToString());
        Assert.That(optionVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(optionVersions[0].OptionId, Is.EqualTo(option.Id));
        Assert.That(optionVersions[0].Weight, Is.EqualTo(oldWeight));
        Assert.That(optionVersions[0].OptionIndex, Is.EqualTo(oldOptionsIndex));
        Assert.That(optionVersions[0].WeightValue, Is.EqualTo(oldWeightValue));
        Assert.That(optionVersions[0].QuestionId, Is.EqualTo(question.Id));

        //New Version
        Assert.That(optionVersions[1].CreatedAt.ToString(), Is.EqualTo(option.CreatedAt.ToString()));
        Assert.That(optionVersions[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(option.UpdatedAt.ToString(), optionVersions[1].UpdatedAt.ToString());
        Assert.That(optionVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(optionVersions[1].OptionId, Is.EqualTo(option.Id));
        Assert.That(optionVersions[1].Weight, Is.EqualTo(option.Weight));
        Assert.That(optionVersions[1].OptionIndex, Is.EqualTo(option.OptionIndex));
        Assert.That(optionVersions[1].WeightValue, Is.EqualTo(option.WeightValue));
        Assert.That(optionVersions[1].QuestionId, Is.EqualTo(question.Id));
    }

    [Test]
    public async Task Options_Delete_DoesSetWorkflowStateToRemoved()
    {
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

        //Act

        DateTime? oldUpdatedAt = option.UpdatedAt;

        await option.Delete(DbContext);

        List<Option> options = DbContext.Options.AsNoTracking().ToList();
        List<OptionVersion> optionVersions = DbContext.OptionVersions.AsNoTracking().ToList();

        Assert.That(options, Is.Not.EqualTo(null));
        Assert.That(optionVersions, Is.Not.EqualTo(null));

        Assert.That(options.Count(), Is.EqualTo(1));
        Assert.That(optionVersions.Count(), Is.EqualTo(2));

        Assert.That(options[0].CreatedAt.ToString(), Is.EqualTo(option.CreatedAt.ToString()));
        Assert.That(options[0].Version, Is.EqualTo(option.Version));
        //            Assert.AreEqual(option.UpdatedAt.ToString(), options[0].UpdatedAt.ToString());
        Assert.That(options[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(options[0].Id, Is.EqualTo(option.Id));
        Assert.That(options[0].Weight, Is.EqualTo(option.Weight));
        Assert.That(options[0].OptionIndex, Is.EqualTo(option.OptionIndex));
        Assert.That(options[0].WeightValue, Is.EqualTo(option.WeightValue));
        Assert.That(question.Id, Is.EqualTo(option.QuestionId));

        //Old Version
        Assert.That(optionVersions[0].CreatedAt.ToString(), Is.EqualTo(option.CreatedAt.ToString()));
        Assert.That(optionVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), optionVersions[0].UpdatedAt.ToString());
        Assert.That(optionVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(optionVersions[0].OptionId, Is.EqualTo(option.Id));
        Assert.That(optionVersions[0].Weight, Is.EqualTo(option.Weight));
        Assert.That(optionVersions[0].OptionIndex, Is.EqualTo(option.OptionIndex));
        Assert.That(optionVersions[0].WeightValue, Is.EqualTo(option.WeightValue));
        Assert.That(optionVersions[0].QuestionId, Is.EqualTo(question.Id));

        //New Version
        Assert.That(optionVersions[1].CreatedAt.ToString(), Is.EqualTo(option.CreatedAt.ToString()));
        Assert.That(optionVersions[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(option.UpdatedAt.ToString(), optionVersions[1].UpdatedAt.ToString());
        Assert.That(optionVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(optionVersions[1].OptionId, Is.EqualTo(option.Id));
        Assert.That(optionVersions[1].Weight, Is.EqualTo(option.Weight));
        Assert.That(optionVersions[1].OptionIndex, Is.EqualTo(option.OptionIndex));
        Assert.That(optionVersions[1].WeightValue, Is.EqualTo(option.WeightValue));
        Assert.That(optionVersions[1].QuestionId, Is.EqualTo(question.Id));
    }
}