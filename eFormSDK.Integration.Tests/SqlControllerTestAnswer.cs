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
    public class SqlControllerTestAnswer : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
//        private readonly string _path;


        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

//            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sut = new SqlController(dbContextHelper);
            await sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            await sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            await sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            await sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }

        [Test]
        public async Task SQL_answers_Create_DoesCreate_UTCAdjustedTrue()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = await testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages
            {
                Name = Guid.NewGuid().ToString(), Description = Guid.NewGuid().ToString()
            };
            await language.Create(dbContext);
            
            

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            survey_configurations surveyConfiguration = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(dbContext);

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
            // Act
            await answer.Create(dbContext);

            answers dbAnswer = dbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = dbContext.answer_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbAnswer);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswer.SiteId, answer.SiteId);
            Assert.AreEqual(dbAnswer.QuestionSetId, answer.QuestionSetId);
            Assert.AreEqual(dbAnswer.SurveyConfigurationId, answer.SurveyConfigurationId);
            Assert.AreEqual(dbAnswer.UnitId, answer.UnitId);
            Assert.AreEqual(dbAnswer.TimeZone, answer.TimeZone);
            Assert.AreEqual(dbAnswer.FinishedAt.ToString(), answer.FinishedAt.ToString());
            Assert.AreEqual(dbAnswer.LanguageId, answer.LanguageId);
            Assert.AreEqual(dbAnswer.AnswerDuration, answer.AnswerDuration);
            Assert.AreEqual(dbAnswer.UtcAdjusted, answer.UtcAdjusted);
        }
        
        [Test]
        public async Task SQL_answers_Create_DoesCreate_UTCAdjustedFalse()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = await testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            await language.Create(dbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            survey_configurations surveyConfiguration = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(dbContext);

            answers answer = new answers();
            answer.SiteId = site1.Id;
            answer.QuestionSetId = questionSet.Id;
            answer.SurveyConfigurationId = surveyConfiguration.Id;
            answer.UnitId = unit1.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = DateTime.Now;
            answer.LanguageId = language.Id;
            answer.AnswerDuration = rnd.Next(1, 255);
            answer.UtcAdjusted = false;
            // Act
            await answer.Create(dbContext);

            answers dbAnswer = dbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = dbContext.answer_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbAnswer);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswer.SiteId, answer.SiteId);
            Assert.AreEqual(dbAnswer.QuestionSetId, answer.QuestionSetId);
            Assert.AreEqual(dbAnswer.SurveyConfigurationId, answer.SurveyConfigurationId);
            Assert.AreEqual(dbAnswer.UnitId, answer.UnitId);
            Assert.AreEqual(dbAnswer.TimeZone, answer.TimeZone);
            Assert.AreEqual(dbAnswer.FinishedAt.ToString(), answer.FinishedAt.ToString());
            Assert.AreEqual(dbAnswer.LanguageId, answer.LanguageId);
            Assert.AreEqual(dbAnswer.AnswerDuration, answer.AnswerDuration);
            Assert.AreEqual(dbAnswer.UtcAdjusted, answer.UtcAdjusted);
        }
        
        [Test]
        public async Task SQL_answers_Update_DoesUpdate_UTCAdjustedTrue()
        {
            // Arrange
            Random rnd = new Random();
            
            sites site1 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            sites site2 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            
            units unit1 = await testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            units unit2 = await testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
           
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            languages language2 = new languages();
            language2.Name = Guid.NewGuid().ToString();
            language2.Description = Guid.NewGuid().ToString();
            await language2.Create(dbContext);
            
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);
            
            string name2 = Guid.NewGuid().ToString();
            question_sets questionSet2 = new question_sets
            {
                Name = name2, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet2.Create(dbContext);

            survey_configurations surveyConfiguration = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(dbContext);

            survey_configurations surveyConfiguration2 = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet2.Id
            };
            await surveyConfiguration2.Create(dbContext);
            
            answers answer = new answers();
            answer.SiteId = site1.Id;
            answer.QuestionSetId = questionSet.Id;
            answer.SurveyConfigurationId = surveyConfiguration.Id;
            answer.UnitId = unit1.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = DateTime.Now;
            answer.LanguageId = language.Id;
            answer.AnswerDuration = rnd.Next(1, 255);
            answer.UtcAdjusted = false;
            await answer.Create(dbContext);
            // Act

            answer.SiteId = site2.Id;
            answer.QuestionSetId = questionSet2.Id;
            answer.SurveyConfigurationId = surveyConfiguration2.Id;
            answer.UnitId = unit2.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = DateTime.Now;
            answer.LanguageId = language.Id;
            answer.AnswerDuration = rnd.Next(1, 255);
            answer.UtcAdjusted = true;
            
            await answer.Update(dbContext);
            
            answers dbAnswer = dbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = dbContext.answer_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbAnswer);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswer.SiteId, answer.SiteId);
            Assert.AreEqual(dbAnswer.QuestionSetId, answer.QuestionSetId);
            Assert.AreEqual(dbAnswer.SurveyConfigurationId, answer.SurveyConfigurationId);
            Assert.AreEqual(dbAnswer.UnitId, answer.UnitId);
            Assert.AreEqual(dbAnswer.TimeZone, answer.TimeZone);
            Assert.AreEqual(dbAnswer.FinishedAt.ToString(), answer.FinishedAt.ToString());
            Assert.AreEqual(dbAnswer.LanguageId, answer.LanguageId);
            Assert.AreEqual(dbAnswer.AnswerDuration, answer.AnswerDuration);
            Assert.AreEqual(dbAnswer.UtcAdjusted, answer.UtcAdjusted);
        }
        
        [Test]
        public async Task SQL_answers_Update_DoesUpdate_UTCAdjustedFalse()
        {
            // Arrange
            Random rnd = new Random();
                       
            sites site1 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            sites site2 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            
            units unit1 = await testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            units unit2 = await testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
           
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            languages language2 = new languages();
            language2.Name = Guid.NewGuid().ToString();
            language2.Description = Guid.NewGuid().ToString();
            await language2.Create(dbContext);
            
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);
            
            string name2 = Guid.NewGuid().ToString();
            question_sets questionSet2 = new question_sets
            {
                Name = name2, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet2.Create(dbContext);

            survey_configurations surveyConfiguration = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(dbContext);

            survey_configurations surveyConfiguration2 = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet2.Id
            };
            await surveyConfiguration2.Create(dbContext);
            
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
            await answer.Create(dbContext);
            // Act

            answer.SiteId = site2.Id;
            answer.QuestionSetId = questionSet2.Id;
            answer.SurveyConfigurationId = surveyConfiguration2.Id;
            answer.UnitId = unit2.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = DateTime.Now;
            answer.LanguageId = language.Id;
            answer.AnswerDuration = rnd.Next(1, 255);
            answer.UtcAdjusted = false;
            
            await answer.Update(dbContext);
            answers dbAnswer = dbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = dbContext.answer_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbAnswer);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswer.SiteId, answer.SiteId);
            Assert.AreEqual(dbAnswer.QuestionSetId, answer.QuestionSetId);
            Assert.AreEqual(dbAnswer.SurveyConfigurationId, answer.SurveyConfigurationId);
            Assert.AreEqual(dbAnswer.UnitId, answer.UnitId);
            Assert.AreEqual(dbAnswer.TimeZone, answer.TimeZone);
            Assert.AreEqual(dbAnswer.FinishedAt.ToString(), answer.FinishedAt.ToString());
            Assert.AreEqual(dbAnswer.LanguageId, answer.LanguageId);
            Assert.AreEqual(dbAnswer.AnswerDuration, answer.AnswerDuration);
            Assert.AreEqual(dbAnswer.UtcAdjusted, answer.UtcAdjusted);
        }
        
        [Test]
        public async Task SQL_answers_Delete_DoesDelete_UTCAdjustedTrue()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = await testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            survey_configurations surveyConfiguration = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(dbContext);

            answers answer = new answers();
            answer.SiteId = site1.Id;
            answer.QuestionSetId = questionSet.Id;
            answer.SurveyConfigurationId = surveyConfiguration.Id;
            answer.UnitId = unit1.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = DateTime.Now;
            answer.LanguageId = language.Id;
            answer.AnswerDuration = rnd.Next(1, 255);
            answer.UtcAdjusted = true;
            await answer.Create(dbContext);
            // Act

            await answer.Delete(dbContext);
            
            answers dbAnswer = dbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = dbContext.answer_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbAnswer);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswer.SiteId, answer.SiteId);
            Assert.AreEqual(dbAnswer.QuestionSetId, answer.QuestionSetId);
            Assert.AreEqual(dbAnswer.SurveyConfigurationId, answer.SurveyConfigurationId);
            Assert.AreEqual(dbAnswer.UnitId, answer.UnitId);
            Assert.AreEqual(dbAnswer.TimeZone, answer.TimeZone);
            Assert.AreEqual(dbAnswer.FinishedAt.ToString(), answer.FinishedAt.ToString());
            Assert.AreEqual(dbAnswer.LanguageId, answer.LanguageId);
            Assert.AreEqual(dbAnswer.AnswerDuration, answer.AnswerDuration);
            Assert.AreEqual(dbAnswer.UtcAdjusted, answer.UtcAdjusted);
            Assert.AreEqual(Constants.WorkflowStates.Removed, answer.WorkflowState);
        }
        
        [Test]
        public async Task SQL_answers_Delete_DoesDelete_UTCAdjustedFalse()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = await testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = await testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            survey_configurations surveyConfiguration = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(dbContext);

            answers answer = new answers();
            answer.SiteId = site1.Id;
            answer.QuestionSetId = questionSet.Id;
            answer.SurveyConfigurationId = surveyConfiguration.Id;
            answer.UnitId = unit1.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = DateTime.Now;
            answer.LanguageId = language.Id;
            answer.AnswerDuration = rnd.Next(1, 255);
            answer.UtcAdjusted = false;
            await answer.Create(dbContext);
            // Act

            await answer.Delete(dbContext);
            
            answers dbAnswer = dbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = dbContext.answer_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbAnswer);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswer.SiteId, answer.SiteId);
            Assert.AreEqual(dbAnswer.QuestionSetId, answer.QuestionSetId);
            Assert.AreEqual(dbAnswer.SurveyConfigurationId, answer.SurveyConfigurationId);
            Assert.AreEqual(dbAnswer.UnitId, answer.UnitId);
            Assert.AreEqual(dbAnswer.TimeZone, answer.TimeZone);
            Assert.AreEqual(dbAnswer.FinishedAt.ToString(), answer.FinishedAt.ToString());
            Assert.AreEqual(dbAnswer.LanguageId, answer.LanguageId);
            Assert.AreEqual(dbAnswer.AnswerDuration, answer.AnswerDuration);
            Assert.AreEqual(dbAnswer.UtcAdjusted, answer.UtcAdjusted);
            Assert.AreEqual(Constants.WorkflowStates.Removed, answer.WorkflowState);
        }
        
        
    }
}