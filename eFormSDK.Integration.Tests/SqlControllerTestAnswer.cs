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
        private string path;

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
            sut.SettingUpdate(Settings.fileLocationPicture, path + @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, path + @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, path + @"\output\dataFolder\reports\");
        }

        [Test]
        public void SQL_answers_Create_DoesCreate_UTCAdjustedTrue()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.name = Guid.NewGuid().ToString();
            language.description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.name = Guid.NewGuid().ToString();
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

            
            answers answer = new answers();
            answer.siteId = site1.id;
            answer.questionSetId = questionSet.id;
            answer.surveyConfigurationId = surveyConfiguration.id;
            answer.unitId = unit1.id;
            answer.timeZone = Guid.NewGuid().ToString();
            answer.finishedAt = rnd.Next(1, 255);
            answer.languageId = language.id;
            answer.answerDuration = rnd.Next(1, 255);
            answer.UTCAdjusted = true;
            // Act
            answer.Create(DbContext);

            answers dbAnswer = DbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = DbContext.answer_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbAnswer);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswer.siteId, answer.siteId);
            Assert.AreEqual(dbAnswer.questionSetId, answer.questionSetId);
            Assert.AreEqual(dbAnswer.surveyConfigurationId, answer.surveyConfigurationId);
            Assert.AreEqual(dbAnswer.unitId, answer.unitId);
            Assert.AreEqual(dbAnswer.timeZone, answer.timeZone);
            Assert.AreEqual(dbAnswer.finishedAt, answer.finishedAt);
            Assert.AreEqual(dbAnswer.languageId, answer.languageId);
            Assert.AreEqual(dbAnswer.answerDuration, answer.answerDuration);
            Assert.AreEqual(dbAnswer.UTCAdjusted, answer.UTCAdjusted);
        }
        
        [Test]
        public void SQL_answers_Create_DoesCreate_UTCAdjustedFalse()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.name = Guid.NewGuid().ToString();
            language.description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.name = Guid.NewGuid().ToString();
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

            
            answers answer = new answers();
            answer.siteId = site1.id;
            answer.questionSetId = questionSet.id;
            answer.surveyConfigurationId = surveyConfiguration.id;
            answer.unitId = unit1.id;
            answer.timeZone = Guid.NewGuid().ToString();
            answer.finishedAt = rnd.Next(1, 255);
            answer.languageId = language.id;
            answer.answerDuration = rnd.Next(1, 255);
            answer.UTCAdjusted = false;
            // Act
            answer.Create(DbContext);

            answers dbAnswer = DbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = DbContext.answer_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbAnswer);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswer.siteId, answer.siteId);
            Assert.AreEqual(dbAnswer.questionSetId, answer.questionSetId);
            Assert.AreEqual(dbAnswer.surveyConfigurationId, answer.surveyConfigurationId);
            Assert.AreEqual(dbAnswer.unitId, answer.unitId);
            Assert.AreEqual(dbAnswer.timeZone, answer.timeZone);
            Assert.AreEqual(dbAnswer.finishedAt, answer.finishedAt);
            Assert.AreEqual(dbAnswer.languageId, answer.languageId);
            Assert.AreEqual(dbAnswer.answerDuration, answer.answerDuration);
            Assert.AreEqual(dbAnswer.UTCAdjusted, answer.UTCAdjusted);
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
            language.name = Guid.NewGuid().ToString();
            language.description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            languages language2 = new languages();
            language2.name = Guid.NewGuid().ToString();
            language2.description = Guid.NewGuid().ToString();
            language2.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.name = Guid.NewGuid().ToString();
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            survey_configurations surveyConfiguration2 = new survey_configurations();
            surveyConfiguration2.name = Guid.NewGuid().ToString();
            surveyConfiguration2.stop = DateTime.Now;
            surveyConfiguration2.start = DateTime.Now;
            surveyConfiguration2.timeOut = rnd.Next(1, 255);
            surveyConfiguration2.timeToLive = rnd.Next(1, 255);
            surveyConfiguration2.Create(DbContext);

            
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);
            
            string name2 = Guid.NewGuid().ToString();
            question_sets questionSet2 = new question_sets();
            questionSet2.name = name2;
            questionSet2.share = false;
            questionSet2.hasChild = false;
            questionSet2.posiblyDeployed = false;
            questionSet2.Create(DbContext);

            
            answers answer = new answers();
            answer.siteId = site1.id;
            answer.questionSetId = questionSet.id;
            answer.surveyConfigurationId = surveyConfiguration.id;
            answer.unitId = unit1.id;
            answer.timeZone = Guid.NewGuid().ToString();
            answer.finishedAt = rnd.Next(1, 255);
            answer.languageId = language.id;
            answer.answerDuration = rnd.Next(1, 255);
            answer.UTCAdjusted = false;
            answer.Create(DbContext);
            // Act

            answer.siteId = site2.id;
            answer.questionSetId = questionSet2.id;
            answer.surveyConfigurationId = surveyConfiguration2.id;
            answer.unitId = unit2.id;
            answer.timeZone = Guid.NewGuid().ToString();
            answer.finishedAt = rnd.Next(1, 255);
            answer.languageId = language.id;
            answer.answerDuration = rnd.Next(1, 255);
            answer.UTCAdjusted = true;
            
            answer.Update(DbContext);
            
            answers dbAnswer = DbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = DbContext.answer_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbAnswer);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswer.siteId, answer.siteId);
            Assert.AreEqual(dbAnswer.questionSetId, answer.questionSetId);
            Assert.AreEqual(dbAnswer.surveyConfigurationId, answer.surveyConfigurationId);
            Assert.AreEqual(dbAnswer.unitId, answer.unitId);
            Assert.AreEqual(dbAnswer.timeZone, answer.timeZone);
            Assert.AreEqual(dbAnswer.finishedAt, answer.finishedAt);
            Assert.AreEqual(dbAnswer.languageId, answer.languageId);
            Assert.AreEqual(dbAnswer.answerDuration, answer.answerDuration);
            Assert.AreEqual(dbAnswer.UTCAdjusted, answer.UTCAdjusted);
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
            language.name = Guid.NewGuid().ToString();
            language.description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            languages language2 = new languages();
            language2.name = Guid.NewGuid().ToString();
            language2.description = Guid.NewGuid().ToString();
            language2.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.name = Guid.NewGuid().ToString();
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            survey_configurations surveyConfiguration2 = new survey_configurations();
            surveyConfiguration2.name = Guid.NewGuid().ToString();
            surveyConfiguration2.stop = DateTime.Now;
            surveyConfiguration2.start = DateTime.Now;
            surveyConfiguration2.timeOut = rnd.Next(1, 255);
            surveyConfiguration2.timeToLive = rnd.Next(1, 255);
            surveyConfiguration2.Create(DbContext);

            
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);
            
            string name2 = Guid.NewGuid().ToString();
            question_sets questionSet2 = new question_sets();
            questionSet2.name = name2;
            questionSet2.share = false;
            questionSet2.hasChild = false;
            questionSet2.posiblyDeployed = false;
            questionSet2.Create(DbContext);

            
            answers answer = new answers();
            answer.siteId = site1.id;
            answer.questionSetId = questionSet.id;
            answer.surveyConfigurationId = surveyConfiguration.id;
            answer.unitId = unit1.id;
            answer.timeZone = Guid.NewGuid().ToString();
            answer.finishedAt = rnd.Next(1, 255);
            answer.languageId = language.id;
            answer.answerDuration = rnd.Next(1, 255);
            answer.UTCAdjusted = true;
            answer.Create(DbContext);
            // Act

            answer.siteId = site2.id;
            answer.questionSetId = questionSet2.id;
            answer.surveyConfigurationId = surveyConfiguration2.id;
            answer.unitId = unit2.id;
            answer.timeZone = Guid.NewGuid().ToString();
            answer.finishedAt = rnd.Next(1, 255);
            answer.languageId = language.id;
            answer.answerDuration = rnd.Next(1, 255);
            answer.UTCAdjusted = false;
            
            answer.Update(DbContext);
            answers dbAnswer = DbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = DbContext.answer_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbAnswer);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswer.siteId, answer.siteId);
            Assert.AreEqual(dbAnswer.questionSetId, answer.questionSetId);
            Assert.AreEqual(dbAnswer.surveyConfigurationId, answer.surveyConfigurationId);
            Assert.AreEqual(dbAnswer.unitId, answer.unitId);
            Assert.AreEqual(dbAnswer.timeZone, answer.timeZone);
            Assert.AreEqual(dbAnswer.finishedAt, answer.finishedAt);
            Assert.AreEqual(dbAnswer.languageId, answer.languageId);
            Assert.AreEqual(dbAnswer.answerDuration, answer.answerDuration);
            Assert.AreEqual(dbAnswer.UTCAdjusted, answer.UTCAdjusted);
        }
        
        [Test]
        public void SQL_answers_Delete_DoesDelete_UTCAdjustedTrue()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.name = Guid.NewGuid().ToString();
            language.description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.name = Guid.NewGuid().ToString();
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

            
            answers answer = new answers();
            answer.siteId = site1.id;
            answer.questionSetId = questionSet.id;
            answer.surveyConfigurationId = surveyConfiguration.id;
            answer.unitId = unit1.id;
            answer.timeZone = Guid.NewGuid().ToString();
            answer.finishedAt = rnd.Next(1, 255);
            answer.languageId = language.id;
            answer.answerDuration = rnd.Next(1, 255);
            answer.UTCAdjusted = true;
            answer.Create(DbContext);
            // Act

            answer.Delete(DbContext);
            
            answers dbAnswer = DbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = DbContext.answer_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbAnswer);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswer.siteId, answer.siteId);
            Assert.AreEqual(dbAnswer.questionSetId, answer.questionSetId);
            Assert.AreEqual(dbAnswer.surveyConfigurationId, answer.surveyConfigurationId);
            Assert.AreEqual(dbAnswer.unitId, answer.unitId);
            Assert.AreEqual(dbAnswer.timeZone, answer.timeZone);
            Assert.AreEqual(dbAnswer.finishedAt, answer.finishedAt);
            Assert.AreEqual(dbAnswer.languageId, answer.languageId);
            Assert.AreEqual(dbAnswer.answerDuration, answer.answerDuration);
            Assert.AreEqual(dbAnswer.UTCAdjusted, answer.UTCAdjusted);
            Assert.AreEqual(Constants.WorkflowStates.Removed, answer.workflow_state);
        }
        
        [Test]
        public void SQL_answers_Delete_DoesDelete_UTCAdjustedFalse()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.name = Guid.NewGuid().ToString();
            language.description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.name = Guid.NewGuid().ToString();
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

            
            answers answer = new answers();
            answer.siteId = site1.id;
            answer.questionSetId = questionSet.id;
            answer.surveyConfigurationId = surveyConfiguration.id;
            answer.unitId = unit1.id;
            answer.timeZone = Guid.NewGuid().ToString();
            answer.finishedAt = rnd.Next(1, 255);
            answer.languageId = language.id;
            answer.answerDuration = rnd.Next(1, 255);
            answer.UTCAdjusted = false;
            answer.Create(DbContext);
            // Act

            answer.Delete(DbContext);
            
            answers dbAnswer = DbContext.answers.AsNoTracking().First();
            answer_versions dbVersion = DbContext.answer_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbAnswer);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswer.siteId, answer.siteId);
            Assert.AreEqual(dbAnswer.questionSetId, answer.questionSetId);
            Assert.AreEqual(dbAnswer.surveyConfigurationId, answer.surveyConfigurationId);
            Assert.AreEqual(dbAnswer.unitId, answer.unitId);
            Assert.AreEqual(dbAnswer.timeZone, answer.timeZone);
            Assert.AreEqual(dbAnswer.finishedAt, answer.finishedAt);
            Assert.AreEqual(dbAnswer.languageId, answer.languageId);
            Assert.AreEqual(dbAnswer.answerDuration, answer.answerDuration);
            Assert.AreEqual(dbAnswer.UTCAdjusted, answer.UTCAdjusted);
            Assert.AreEqual(Constants.WorkflowStates.Removed, answer.workflow_state);
        }
        
        
    }
}