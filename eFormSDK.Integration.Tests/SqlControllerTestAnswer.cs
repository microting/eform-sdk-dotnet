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

            SqlController sql = new SqlController(ConnectionString);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(ConnectionString);
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
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration.Create(dbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            await questionSet.Create(dbContext);

            
            answers answer = new answers();
            answer.SiteId = site1.Id;
            answer.QuestionSetId = questionSet.Id;
            answer.SurveyConfigurationId = surveyConfiguration.Id;
            answer.UnitId = unit1.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = rnd.Next(1, 255);
            answer.LanguageId = language.Id;
            answer.AnswerDuration = rnd.Next(1, 255);
            answer.UtcAdjusted = true;
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
            Assert.AreEqual(dbAnswer.FinishedAt, answer.FinishedAt);
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
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration.Create(dbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            await questionSet.Create(dbContext);

            
            answers answer = new answers();
            answer.SiteId = site1.Id;
            answer.QuestionSetId = questionSet.Id;
            answer.SurveyConfigurationId = surveyConfiguration.Id;
            answer.UnitId = unit1.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = rnd.Next(1, 255);
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
            Assert.AreEqual(dbAnswer.FinishedAt, answer.FinishedAt);
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
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration.Create(dbContext);

            survey_configurations surveyConfiguration2 = new survey_configurations();
            surveyConfiguration2.Name = Guid.NewGuid().ToString();
            surveyConfiguration2.Stop = DateTime.Now;
            surveyConfiguration2.Start = DateTime.Now;
            surveyConfiguration2.TimeOut = rnd.Next(1, 255);
            surveyConfiguration2.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration2.Create(dbContext);

            
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            await questionSet.Create(dbContext);
            
            string name2 = Guid.NewGuid().ToString();
            question_sets questionSet2 = new question_sets();
            questionSet2.Name = name2;
            questionSet2.Share = false;
            questionSet2.HasChild = false;
            questionSet2.PosiblyDeployed = false;
            await questionSet2.Create(dbContext);

            
            answers answer = new answers();
            answer.SiteId = site1.Id;
            answer.QuestionSetId = questionSet.Id;
            answer.SurveyConfigurationId = surveyConfiguration.Id;
            answer.UnitId = unit1.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = rnd.Next(1, 255);
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
            answer.FinishedAt = rnd.Next(1, 255);
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
            Assert.AreEqual(dbAnswer.FinishedAt, answer.FinishedAt);
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
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration.Create(dbContext);

            survey_configurations surveyConfiguration2 = new survey_configurations();
            surveyConfiguration2.Name = Guid.NewGuid().ToString();
            surveyConfiguration2.Stop = DateTime.Now;
            surveyConfiguration2.Start = DateTime.Now;
            surveyConfiguration2.TimeOut = rnd.Next(1, 255);
            surveyConfiguration2.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration2.Create(dbContext);

            
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            await questionSet.Create(dbContext);
            
            string name2 = Guid.NewGuid().ToString();
            question_sets questionSet2 = new question_sets();
            questionSet2.Name = name2;
            questionSet2.Share = false;
            questionSet2.HasChild = false;
            questionSet2.PosiblyDeployed = false;
            await questionSet2.Create(dbContext);

            
            answers answer = new answers();
            answer.SiteId = site1.Id;
            answer.QuestionSetId = questionSet.Id;
            answer.SurveyConfigurationId = surveyConfiguration.Id;
            answer.UnitId = unit1.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = rnd.Next(1, 255);
            answer.LanguageId = language.Id;
            answer.AnswerDuration = rnd.Next(1, 255);
            answer.UtcAdjusted = true;
            await answer.Create(dbContext);
            // Act

            answer.SiteId = site2.Id;
            answer.QuestionSetId = questionSet2.Id;
            answer.SurveyConfigurationId = surveyConfiguration2.Id;
            answer.UnitId = unit2.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = rnd.Next(1, 255);
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
            Assert.AreEqual(dbAnswer.FinishedAt, answer.FinishedAt);
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
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration.Create(dbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            await questionSet.Create(dbContext);

            
            answers answer = new answers();
            answer.SiteId = site1.Id;
            answer.QuestionSetId = questionSet.Id;
            answer.SurveyConfigurationId = surveyConfiguration.Id;
            answer.UnitId = unit1.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = rnd.Next(1, 255);
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
            Assert.AreEqual(dbAnswer.FinishedAt, answer.FinishedAt);
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
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration.Create(dbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            await questionSet.Create(dbContext);

            
            answers answer = new answers();
            answer.SiteId = site1.Id;
            answer.QuestionSetId = questionSet.Id;
            answer.SurveyConfigurationId = surveyConfiguration.Id;
            answer.UnitId = unit1.Id;
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.FinishedAt = rnd.Next(1, 255);
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
            Assert.AreEqual(dbAnswer.FinishedAt, answer.FinishedAt);
            Assert.AreEqual(dbAnswer.LanguageId, answer.LanguageId);
            Assert.AreEqual(dbAnswer.AnswerDuration, answer.AnswerDuration);
            Assert.AreEqual(dbAnswer.UtcAdjusted, answer.UtcAdjusted);
            Assert.AreEqual(Constants.WorkflowStates.Removed, answer.WorkflowState);
        }
        
        
    }
}