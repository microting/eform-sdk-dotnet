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

using eFormCore;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestAnswerValue : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
//        private string path;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(dbContextHelper);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            await sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            await sut.SettingUpdate(Settings.fileLocationPdf,  @"\output\dataFolder\pdf\");
            await sut.SettingUpdate(Settings.fileLocationJasper,  @"\output\dataFolder\reports\");
        }

        [Test]
        public async Task SQL_answerValues_Create_DoesCreate()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = await testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages
            {
                Name = Guid.NewGuid().ToString(), Description = Guid.NewGuid().ToString()
            };
            await language.Create(dbContext).ConfigureAwait(false);
            
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext).ConfigureAwait(false);
            #endregion
            
            #region surveyConfiguration

            survey_configurations surveyConfiguration = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(dbContext).ConfigureAwait(false);
            #endregion

            #region Answer

            answers answer = new answers
            {
                SiteId = site1.Id,
                QuestionSetId = questionSet.Id,
                SurveyConfigurationId = surveyConfiguration.Id,
                UnitId = unit1.Id,
                TimeZone = Guid.NewGuid().ToString(),
                FinishedAt = DateTime.Now,
                LanguageId = language.Id,
                AnswerDuration = rnd.Next(1, 255),
                UtcAdjusted = true
            };
            await answer.Create(dbContext).ConfigureAwait(false);
            

            #endregion
           
            #region question     
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = false
            };
            await question.Create(dbContext).ConfigureAwait(false);
            #endregion
            
            #region Option

            options option = new options
            {
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id,
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                NextQuestionId = rnd.Next(1, 255),
                ContinuousOptionId = rnd.Next(1, 255)
            };
            await option.Create(dbContext).ConfigureAwait(false);
            #endregion

            answer_values answerValue = new answer_values
            {
                QuestionId = question.Id,
                Value = rnd.Next(1, 255).ToString(),
                Answer = answer,
                Option = option,
                AnswerId = answer.Id,
                Question = question,
                OptionId = option.Id
            };

            // Act
            await answerValue.Create(dbContext).ConfigureAwait(false);

            answer_values dbAnswerValue = dbContext.answer_values.AsNoTracking().First();
            answer_value_versions dbVersion = dbContext.answer_value_versions.AsNoTracking().First();
            
            // Assert
            Assert.NotNull(dbAnswerValue);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswerValue.QuestionId, answerValue.QuestionId);
            Assert.AreEqual(dbAnswerValue.AnswerId, answerValue.AnswerId);
            Assert.AreEqual(dbAnswerValue.OptionId, answerValue.OptionId);
            Assert.AreEqual(dbAnswerValue.Value, answerValue.Value);
        }
       
        [Test]
        public async Task SQL_answerValues_Update_DoesUpdate()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = await testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages
            {
                Name = Guid.NewGuid().ToString(), Description = Guid.NewGuid().ToString()
            };
            await language.Create(dbContext).ConfigureAwait(false);
            
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext).ConfigureAwait(false);
            #endregion
            
            #region surveyConfiguration

            survey_configurations surveyConfiguration = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(dbContext).ConfigureAwait(false);
            #endregion

            #region Answer

            answers answer = new answers
            {
                SiteId = site1.Id,
                QuestionSetId = questionSet.Id,
                SurveyConfigurationId = surveyConfiguration.Id,
                UnitId = unit1.Id,
                TimeZone = Guid.NewGuid().ToString(),
                FinishedAt = DateTime.Now,
                LanguageId = language.Id,
                AnswerDuration = rnd.Next(1, 255),
                UtcAdjusted = true
            };
            await answer.Create(dbContext).ConfigureAwait(false);
            

            #endregion
           
            #region question     
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = false
            };
            await question.Create(dbContext).ConfigureAwait(false);
            #endregion
            
            #region Option

            options option = new options
            {
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id,
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                NextQuestionId = rnd.Next(1, 255),
                ContinuousOptionId = rnd.Next(1, 255)
            };
            await option.Create(dbContext).ConfigureAwait(false);
            #endregion
            
            #region Answer2

            answers answer2 = new answers
            {
                SiteId = site1.Id,
                QuestionSetId = questionSet.Id,
                SurveyConfigurationId = surveyConfiguration.Id,
                UnitId = unit1.Id,
                TimeZone = Guid.NewGuid().ToString(),
                FinishedAt = DateTime.Now,
                LanguageId = language.Id,
                AnswerDuration = rnd.Next(1, 255),
                UtcAdjusted = true
            };
            await answer2.Create(dbContext).ConfigureAwait(false);
            

            #endregion
           
            #region question2     
            string type2 = Guid.NewGuid().ToString();
            string questionType2 = Guid.NewGuid().ToString();
            string imagePosition2 = Guid.NewGuid().ToString();
            string fontSize2 = Guid.NewGuid().ToString();
            questions question2 = new questions
            {
                Type = type2,
                QuestionType = questionType2,
                ImagePosition = imagePosition2,
                FontSize = fontSize2,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = false
            };

            #endregion
            
            #region Option2

            options option2 = new options
            {
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id,
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                NextQuestionId = rnd.Next(1, 255),
                ContinuousOptionId = rnd.Next(1, 255)
            };
            await option2.Create(dbContext).ConfigureAwait(false);
            #endregion

            answer_values answerValue = new answer_values
            {
                QuestionId = question.Id,
                Value = rnd.Next(1, 255).ToString(),
                Answer = answer,
                Option = option,
                AnswerId = answer.Id,
                Question = question,
                OptionId = option.Id
            };

            await answerValue.Create(dbContext).ConfigureAwait(false);
            // Act
            answerValue.QuestionId = question2.Id;
            answerValue.Value = rnd.Next(1, 255).ToString();
            answerValue.Answer = answer2;
            answerValue.Option = option2;
            answerValue.AnswerId = answer2.Id;
            answerValue.Question = question2;
            answerValue.OptionId = option2.Id;
            
            await answerValue.Update(dbContext).ConfigureAwait(false);
            
            answer_values dbAnswerValue = dbContext.answer_values.AsNoTracking().First();
            answer_value_versions dbVersion = dbContext.answer_value_versions.AsNoTracking().First();
            
            // Assert
            Assert.NotNull(dbAnswerValue);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswerValue.QuestionId, answerValue.QuestionId);
            Assert.AreEqual(dbAnswerValue.AnswerId, answerValue.AnswerId);
            Assert.AreEqual(dbAnswerValue.OptionId, answerValue.OptionId);
            Assert.AreEqual(dbAnswerValue.Value, answerValue.Value);
        }

        [Test]
        public async Task SQL_answerValues_Delete_DoesDelete()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = await testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages
            {
                Name = Guid.NewGuid().ToString(), Description = Guid.NewGuid().ToString()
            };
            await language.Create(dbContext).ConfigureAwait(false);
            
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext).ConfigureAwait(false);
            #endregion
            
            #region surveyConfiguration

            survey_configurations surveyConfiguration = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(dbContext).ConfigureAwait(false);
            #endregion

            #region Answer

            answers answer = new answers
            {
                SiteId = site1.Id,
                QuestionSetId = questionSet.Id,
                SurveyConfigurationId = surveyConfiguration.Id,
                UnitId = unit1.Id,
                TimeZone = Guid.NewGuid().ToString(),
                FinishedAt = DateTime.Now,
                LanguageId = language.Id,
                AnswerDuration = rnd.Next(1, 255),
                UtcAdjusted = true
            };
            await answer.Create(dbContext).ConfigureAwait(false);
            

            #endregion
           
            #region question     
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = false
            };
            await question.Create(dbContext).ConfigureAwait(false);
            #endregion
            
            #region Option

            options option = new options
            {
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id,
                Weight = rnd.Next(1, 255),
                OptionIndex = rnd.Next(1, 255),
                NextQuestionId = rnd.Next(1, 255),
                ContinuousOptionId = rnd.Next(1, 255)
            };
            await option.Create(dbContext).ConfigureAwait(false);
            #endregion

            answer_values answerValue = new answer_values
            {
                QuestionId = question.Id,
                Value = rnd.Next(1, 255).ToString(),
                Answer = answer,
                Option = option,
                AnswerId = answer.Id,
                Question = question,
                OptionId = option.Id
            };
            await answerValue.Create(dbContext).ConfigureAwait(false);

            // Act

            await answerValue.Delete(dbContext);
            
            answer_values dbAnswerValue = dbContext.answer_values.AsNoTracking().First();
            answer_value_versions dbVersion = dbContext.answer_value_versions.AsNoTracking().First();
            
            // Assert
            Assert.NotNull(dbAnswerValue);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswerValue.QuestionId, answerValue.QuestionId);
            Assert.AreEqual(dbAnswerValue.AnswerId, answerValue.AnswerId);
            Assert.AreEqual(dbAnswerValue.OptionId, answerValue.OptionId);
            Assert.AreEqual(dbAnswerValue.Value, answerValue.Value);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbAnswerValue.WorkflowState);
        }
    }
}