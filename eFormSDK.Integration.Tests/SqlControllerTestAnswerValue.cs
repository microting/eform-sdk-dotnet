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
    public class SqlControllerTestAnswerValue : DbTestFixture
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
        public void SQL_answerValues_Create_DoesCreate()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.name = Guid.NewGuid().ToString();
            language.description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            
            #region surveyConfiguration
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.name = Guid.NewGuid().ToString();
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);
            #endregion
            
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);
            #endregion
            
            #region Answer
            answers answer = new answers();
            answer.siteId = site1.Id;
            answer.questionSetId = questionSet.Id;
            answer.surveyConfigurationId = surveyConfiguration.Id;
            answer.unitId = unit1.Id;
            answer.timeZone = Guid.NewGuid().ToString();
            answer.finishedAt = rnd.Next(1, 255);
            answer.languageId = language.Id;
            answer.answerDuration = rnd.Next(1, 255);
            answer.UTCAdjusted = true;
            answer.Create(DbContext);
            

            #endregion
           
            #region question     
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions();
            question.type = type;
            question.questionType = questionType;
            question.imagePostion = imagePosition;
            question.fontSize = fontSize;
            question.questionSetId = questionSet.Id;
            question.maximum = rnd.Next(1, 255);
            question.minimum = rnd.Next(1, 255);
            question.refId = rnd.Next(1, 255);
            question.maxDuration = rnd.Next(1, 255);
            question.minDuration = rnd.Next(1, 255);
            question.questionIndex = rnd.Next(1, 255);
            question.continuousQuestionId = rnd.Next(1, 255);
            question.prioritised = false;
            question.validDisplay = false;
            question.backButtonEnabled = false;
            question.image = false;
            question.Create(DbContext);
            #endregion
            
            #region Option
            
            options option = new options();
            option.weightValue = rnd.Next(1, 255);
            option.questionId = question.Id;
            option.weight = rnd.Next(1, 255);
            option.optionsIndex = rnd.Next(1, 255);
            option.nextQuestionId = rnd.Next(1, 255);
            option.continuousOptionId = rnd.Next(1, 255);
            option.Create(DbContext);
            #endregion
            
            answer_values answerValue = new answer_values();
            answerValue.questionId = question.Id;
            answerValue.value = rnd.Next(1, 255);
            answerValue.Answer = answer;
            answerValue.Option = option;
            answerValue.answerId = answer.Id;
            answerValue.Question = question;
            answerValue.optionsId = option.Id;
            
            // Act
            answerValue.Create(DbContext);

            answer_values dbAnswerValue = DbContext.answer_values.AsNoTracking().First();
            answer_value_versions dbVersion = DbContext.answer_value_versions.AsNoTracking().First();
            
            // Assert
            Assert.NotNull(dbAnswerValue);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswerValue.questionId, answerValue.questionId);
            Assert.AreEqual(dbAnswerValue.answerId, answerValue.answerId);
            Assert.AreEqual(dbAnswerValue.optionsId, answerValue.optionsId);
            Assert.AreEqual(dbAnswerValue.value, answerValue.value);
        }
       
        [Test]
        public void SQL_answerValues_Update_DoesUpdate()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.name = Guid.NewGuid().ToString();
            language.description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            
            #region surveyConfiguration
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.name = Guid.NewGuid().ToString();
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);
            #endregion
            
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);
            #endregion
            
            #region Answer
            answers answer = new answers();
            answer.siteId = site1.Id;
            answer.questionSetId = questionSet.Id;
            answer.surveyConfigurationId = surveyConfiguration.Id;
            answer.unitId = unit1.Id;
            answer.timeZone = Guid.NewGuid().ToString();
            answer.finishedAt = rnd.Next(1, 255);
            answer.languageId = language.Id;
            answer.answerDuration = rnd.Next(1, 255);
            answer.UTCAdjusted = true;
            answer.Create(DbContext);
            

            #endregion
           
            #region question     
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions();
            question.type = type;
            question.questionType = questionType;
            question.imagePostion = imagePosition;
            question.fontSize = fontSize;
            question.questionSetId = questionSet.Id;
            question.maximum = rnd.Next(1, 255);
            question.minimum = rnd.Next(1, 255);
            question.refId = rnd.Next(1, 255);
            question.maxDuration = rnd.Next(1, 255);
            question.minDuration = rnd.Next(1, 255);
            question.questionIndex = rnd.Next(1, 255);
            question.continuousQuestionId = rnd.Next(1, 255);
            question.prioritised = false;
            question.validDisplay = false;
            question.backButtonEnabled = false;
            question.image = false;
            question.Create(DbContext);
            #endregion
            
            #region Option
            
            options option = new options();
            option.weightValue = rnd.Next(1, 255);
            option.questionId = question.Id;
            option.weight = rnd.Next(1, 255);
            option.optionsIndex = rnd.Next(1, 255);
            option.nextQuestionId = rnd.Next(1, 255);
            option.continuousOptionId = rnd.Next(1, 255);
            option.Create(DbContext);
            #endregion
            
            #region Answer2
            answers answer2 = new answers();
            answer2.siteId = site1.Id;
            answer2.questionSetId = questionSet.Id;
            answer2.surveyConfigurationId = surveyConfiguration.Id;
            answer2.unitId = unit1.Id;
            answer2.timeZone = Guid.NewGuid().ToString();
            answer2.finishedAt = rnd.Next(1, 255);
            answer2.languageId = language.Id;
            answer2.answerDuration = rnd.Next(1, 255);
            answer2.UTCAdjusted = true;
            answer2.Create(DbContext);
            

            #endregion
           
            #region question2     
            string type2 = Guid.NewGuid().ToString();
            string questionType2 = Guid.NewGuid().ToString();
            string imagePosition2 = Guid.NewGuid().ToString();
            string fontSize2 = Guid.NewGuid().ToString();
            questions question2 = new questions();
            question2.type = type2;
            question2.questionType = questionType2;
            question2.imagePostion = imagePosition2;
            question2.fontSize = fontSize2;
            question2.questionSetId = questionSet.Id;
            question2.maximum = rnd.Next(1, 255);
            question2.minimum = rnd.Next(1, 255);
            question2.refId = rnd.Next(1, 255);
            question2.maxDuration = rnd.Next(1, 255);
            question2.minDuration = rnd.Next(1, 255);
            question2.questionIndex = rnd.Next(1, 255);
            question2.continuousQuestionId = rnd.Next(1, 255);
            question2.prioritised = false;
            question2.validDisplay = false;
            question2.backButtonEnabled = false;
            question2.image = false;
            #endregion
            
            #region Option2
            
            options option2 = new options();
            option2.weightValue = rnd.Next(1, 255);
            option2.questionId = question.Id;
            option2.weight = rnd.Next(1, 255);
            option2.optionsIndex = rnd.Next(1, 255);
            option2.nextQuestionId = rnd.Next(1, 255);
            option2.continuousOptionId = rnd.Next(1, 255);
            option2.Create(DbContext);
            #endregion
            answer_values answerValue = new answer_values();
            answerValue.questionId = question.Id;
            answerValue.value = rnd.Next(1, 255);
            answerValue.Answer = answer;
            answerValue.Option = option;
            answerValue.answerId = answer.Id;
            answerValue.Question = question;
            answerValue.optionsId = option.Id;
            
            answerValue.Create(DbContext);
            // Act
            answerValue.questionId = question2.Id;
            answerValue.value = rnd.Next(1, 255);
            answerValue.Answer = answer2;
            answerValue.Option = option2;
            answerValue.answerId = answer2.Id;
            answerValue.Question = question2;
            answerValue.optionsId = option2.Id;
            
            answerValue.Update(DbContext);
            
            answer_values dbAnswerValue = DbContext.answer_values.AsNoTracking().First();
            answer_value_versions dbVersion = DbContext.answer_value_versions.AsNoTracking().First();
            
            // Assert
            Assert.NotNull(dbAnswerValue);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswerValue.questionId, answerValue.questionId);
            Assert.AreEqual(dbAnswerValue.answerId, answerValue.answerId);
            Assert.AreEqual(dbAnswerValue.optionsId, answerValue.optionsId);
            Assert.AreEqual(dbAnswerValue.value, answerValue.value);
        }

        [Test]
        public void SQL_answerValues_Delete_DoesDelete()
        {
            // Arrange
            Random rnd = new Random();
            sites site1 = testHelpers.CreateSite(Guid.NewGuid().ToString(), rnd.Next(1, 255));
            units unit1 = testHelpers.CreateUnit(rnd.Next(1, 255), rnd.Next(1, 255), site1, rnd.Next(1, 255));
            languages language = new languages();
            language.name = Guid.NewGuid().ToString();
            language.description = Guid.NewGuid().ToString();
            language.Create(DbContext);
            
            #region surveyConfiguration
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.name = Guid.NewGuid().ToString();
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);
            #endregion
            
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);
            #endregion
            
            #region Answer
            answers answer = new answers();
            answer.siteId = site1.Id;
            answer.questionSetId = questionSet.Id;
            answer.surveyConfigurationId = surveyConfiguration.Id;
            answer.unitId = unit1.Id;
            answer.timeZone = Guid.NewGuid().ToString();
            answer.finishedAt = rnd.Next(1, 255);
            answer.languageId = language.Id;
            answer.answerDuration = rnd.Next(1, 255);
            answer.UTCAdjusted = true;
            answer.Create(DbContext);
            

            #endregion
           
            #region question     
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions();
            question.type = type;
            question.questionType = questionType;
            question.imagePostion = imagePosition;
            question.fontSize = fontSize;
            question.questionSetId = questionSet.Id;
            question.maximum = rnd.Next(1, 255);
            question.minimum = rnd.Next(1, 255);
            question.refId = rnd.Next(1, 255);
            question.maxDuration = rnd.Next(1, 255);
            question.minDuration = rnd.Next(1, 255);
            question.questionIndex = rnd.Next(1, 255);
            question.continuousQuestionId = rnd.Next(1, 255);
            question.prioritised = false;
            question.validDisplay = false;
            question.backButtonEnabled = false;
            question.image = false;
            question.Create(DbContext);
            #endregion
            
            #region Option
            
            options option = new options();
            option.weightValue = rnd.Next(1, 255);
            option.questionId = question.Id;
            option.weight = rnd.Next(1, 255);
            option.optionsIndex = rnd.Next(1, 255);
            option.nextQuestionId = rnd.Next(1, 255);
            option.continuousOptionId = rnd.Next(1, 255);
            option.Create(DbContext);
            #endregion
            
            answer_values answerValue = new answer_values();
            answerValue.questionId = question.Id;
            answerValue.value = rnd.Next(1, 255);
            answerValue.Answer = answer;
            answerValue.Option = option;
            answerValue.answerId = answer.Id;
            answerValue.Question = question;
            answerValue.optionsId = option.Id;
            answerValue.Create(DbContext);

            // Act

            answerValue.Delete(DbContext);
            
            answer_values dbAnswerValue = DbContext.answer_values.AsNoTracking().First();
            answer_value_versions dbVersion = DbContext.answer_value_versions.AsNoTracking().First();
            
            // Assert
            Assert.NotNull(dbAnswerValue);
            Assert.NotNull(dbVersion);
            
            Assert.AreEqual(dbAnswerValue.questionId, answerValue.questionId);
            Assert.AreEqual(dbAnswerValue.answerId, answerValue.answerId);
            Assert.AreEqual(dbAnswerValue.optionsId, answerValue.optionsId);
            Assert.AreEqual(dbAnswerValue.value, answerValue.value);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbAnswerValue.workflow_state);
        }
    }
}