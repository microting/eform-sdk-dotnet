using eFormCore;
using eFormCore.Helpers;
using eFormData;
using eFormShared;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestAnswer : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
//        private readonly string _path;


        public override void DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            sql.SettingUpdate(Settings.firstRunDone, "true");
            sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(ConnectionString);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }

        [Test]
        public void SQL_answers_Create_DoesCreate_UTCAdjustedTrue()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

            
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
            answer.Create(DbContext);

            answers dbAnswer = DbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = DbContext.answer_versions.AsNoTracking().First();
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
        public void SQL_answers_Create_DoesCreate_UTCAdjustedFalse()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

            
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
            answer.Create(DbContext);

            answers dbAnswer = DbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = DbContext.answer_versions.AsNoTracking().First();
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
        public void SQL_answers_Update_DoesUpdate_UTCAdjustedTrue()
        {
            // Arrange
            Random rnd = new Random();
            
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            sites site2 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            units unit2 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
           
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            languages language2 = new languages();
            language2.Name = Guid.NewGuid().ToString();
            language2.Description = Guid.NewGuid().ToString();
            language2.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            survey_configurations surveyConfiguration2 = new survey_configurations();
            surveyConfiguration2.Name = Guid.NewGuid().ToString();
            surveyConfiguration2.Stop = DateTime.Now;
            surveyConfiguration2.Start = DateTime.Now;
            surveyConfiguration2.TimeOut = rnd.Next(1, 255);
            surveyConfiguration2.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration2.Create(DbContext);

            
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);
            
            string name2 = Guid.NewGuid().ToString();
            question_sets questionSet2 = new question_sets();
            questionSet2.Name = name2;
            questionSet2.Share = false;
            questionSet2.HasChild = false;
            questionSet2.PosiblyDeployed = false;
            questionSet2.Create(DbContext);

            
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
            answer.Create(DbContext);
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
            
            answer.Update(DbContext);
            
            answers dbAnswer = DbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = DbContext.answer_versions.AsNoTracking().First();
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
        public void SQL_answers_Update_DoesUpdate_UTCAdjustedFalse()
        {
            // Arrange
            Random rnd = new Random();
                       
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            sites site2 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            units unit2 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
           
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            languages language2 = new languages();
            language2.Name = Guid.NewGuid().ToString();
            language2.Description = Guid.NewGuid().ToString();
            language2.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            survey_configurations surveyConfiguration2 = new survey_configurations();
            surveyConfiguration2.Name = Guid.NewGuid().ToString();
            surveyConfiguration2.Stop = DateTime.Now;
            surveyConfiguration2.Start = DateTime.Now;
            surveyConfiguration2.TimeOut = rnd.Next(1, 255);
            surveyConfiguration2.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration2.Create(DbContext);

            
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);
            
            string name2 = Guid.NewGuid().ToString();
            question_sets questionSet2 = new question_sets();
            questionSet2.Name = name2;
            questionSet2.Share = false;
            questionSet2.HasChild = false;
            questionSet2.PosiblyDeployed = false;
            questionSet2.Create(DbContext);

            
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
            answer.Create(DbContext);
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
            
            answer.Update(DbContext);
            answers dbAnswer = DbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = DbContext.answer_versions.AsNoTracking().First();
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
        public void SQL_answers_Delete_DoesDelete_UTCAdjustedTrue()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

            
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
            answer.Create(DbContext);
            // Act

            answer.Delete(DbContext);
            
            answers dbAnswer = DbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = DbContext.answer_versions.AsNoTracking().First();
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
        public void SQL_answers_Delete_DoesDelete_UTCAdjustedFalse()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

            
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
            answer.Create(DbContext);
            // Act

            answer.Delete(DbContext);
            
            answers dbAnswer = DbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = DbContext.answer_versions.AsNoTracking().First();
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