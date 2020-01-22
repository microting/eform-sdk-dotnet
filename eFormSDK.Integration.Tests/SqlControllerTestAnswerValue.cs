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
            await sut.StartLog(new CoreBase());
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
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);
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
            await surveyConfiguration.Create(dbContext);
            #endregion

            #region Answer
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
            

            #endregion
           
            #region question     
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions();
            question.Type = type;
            question.QuestionType = questionType;
            question.ImagePosition = imagePosition;
            question.FontSize = fontSize;
            question.QuestionSetId = questionSet.Id;
            question.Maximum = rnd.Next(1, 255);
            question.Minimum = rnd.Next(1, 255);
            question.RefId = rnd.Next(1, 255);
            question.MaxDuration = rnd.Next(1, 255);
            question.MinDuration = rnd.Next(1, 255);
            question.QuestionIndex = rnd.Next(1, 255);
            question.ContinuousQuestionId = rnd.Next(1, 255);
            question.Prioritised = false;
            question.ValidDisplay = false;
            question.BackButtonEnabled = false;
            question.Image = false;
            await question.Create(dbContext);
            #endregion
            
            #region Option
            
            options option = new options();
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.NextQuestionId = rnd.Next(1, 255);
            option.ContinuousOptionId = rnd.Next(1, 255);
            await option.Create(dbContext);
            #endregion
            
            answer_values answerValue = new answer_values();
            answerValue.QuestionId = question.Id;
            answerValue.Value = rnd.Next(1, 255).ToString();
            answerValue.Answer = answer;
            answerValue.Option = option;
            answerValue.AnswerId = answer.Id;
            answerValue.Question = question;
            answerValue.OptionId = option.Id;
            
            // Act
            await answerValue.Create(dbContext);

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
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);
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
            await surveyConfiguration.Create(dbContext);
            #endregion

            #region Answer
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
            

            #endregion
           
            #region question     
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions();
            question.Type = type;
            question.QuestionType = questionType;
            question.ImagePosition = imagePosition;
            question.FontSize = fontSize;
            question.QuestionSetId = questionSet.Id;
            question.Maximum = rnd.Next(1, 255);
            question.Minimum = rnd.Next(1, 255);
            question.RefId = rnd.Next(1, 255);
            question.MaxDuration = rnd.Next(1, 255);
            question.MinDuration = rnd.Next(1, 255);
            question.QuestionIndex = rnd.Next(1, 255);
            question.ContinuousQuestionId = rnd.Next(1, 255);
            question.Prioritised = false;
            question.ValidDisplay = false;
            question.BackButtonEnabled = false;
            question.Image = false;
            await question.Create(dbContext);
            #endregion
            
            #region Option
            
            options option = new options();
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.NextQuestionId = rnd.Next(1, 255);
            option.ContinuousOptionId = rnd.Next(1, 255);
            await option.Create(dbContext);
            #endregion
            
            #region Answer2
            answers answer2 = new answers();
            answer2.SiteId = site1.Id;
            answer2.QuestionSetId = questionSet.Id;
            answer2.SurveyConfigurationId = surveyConfiguration.Id;
            answer2.UnitId = unit1.Id;
            answer2.TimeZone = Guid.NewGuid().ToString();
            answer2.FinishedAt = DateTime.Now;
            answer2.LanguageId = language.Id;
            answer2.AnswerDuration = rnd.Next(1, 255);
            answer2.UtcAdjusted = true;
            await answer2.Create(dbContext);
            

            #endregion
           
            #region question2     
            string type2 = Guid.NewGuid().ToString();
            string questionType2 = Guid.NewGuid().ToString();
            string imagePosition2 = Guid.NewGuid().ToString();
            string fontSize2 = Guid.NewGuid().ToString();
            questions question2 = new questions();
            question2.Type = type2;
            question2.QuestionType = questionType2;
            question2.ImagePosition = imagePosition2;
            question2.FontSize = fontSize2;
            question2.QuestionSetId = questionSet.Id;
            question2.Maximum = rnd.Next(1, 255);
            question2.Minimum = rnd.Next(1, 255);
            question2.RefId = rnd.Next(1, 255);
            question2.MaxDuration = rnd.Next(1, 255);
            question2.MinDuration = rnd.Next(1, 255);
            question2.QuestionIndex = rnd.Next(1, 255);
            question2.ContinuousQuestionId = rnd.Next(1, 255);
            question2.Prioritised = false;
            question2.ValidDisplay = false;
            question2.BackButtonEnabled = false;
            question2.Image = false;
            #endregion
            
            #region Option2
            
            options option2 = new options();
            option2.WeightValue = rnd.Next(1, 255);
            option2.QuestionId = question.Id;
            option2.Weight = rnd.Next(1, 255);
            option2.OptionsIndex = rnd.Next(1, 255);
            option2.NextQuestionId = rnd.Next(1, 255);
            option2.ContinuousOptionId = rnd.Next(1, 255);
            await option2.Create(dbContext);
            #endregion
            answer_values answerValue = new answer_values();
            answerValue.QuestionId = question.Id;
            answerValue.Value = rnd.Next(1, 255).ToString();
            answerValue.Answer = answer;
            answerValue.Option = option;
            answerValue.AnswerId = answer.Id;
            answerValue.Question = question;
            answerValue.OptionId = option.Id;
            
            await answerValue.Create(dbContext);
            // Act
            answerValue.QuestionId = question2.Id;
            answerValue.Value = rnd.Next(1, 255).ToString();
            answerValue.Answer = answer2;
            answerValue.Option = option2;
            answerValue.AnswerId = answer2.Id;
            answerValue.Question = question2;
            answerValue.OptionId = option2.Id;
            
            await answerValue.Update(dbContext);
            
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
            languages language = new languages();
            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);
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
            await surveyConfiguration.Create(dbContext);
            #endregion

            #region Answer
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
            

            #endregion
           
            #region question     
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions();
            question.Type = type;
            question.QuestionType = questionType;
            question.ImagePosition = imagePosition;
            question.FontSize = fontSize;
            question.QuestionSetId = questionSet.Id;
            question.Maximum = rnd.Next(1, 255);
            question.Minimum = rnd.Next(1, 255);
            question.RefId = rnd.Next(1, 255);
            question.MaxDuration = rnd.Next(1, 255);
            question.MinDuration = rnd.Next(1, 255);
            question.QuestionIndex = rnd.Next(1, 255);
            question.ContinuousQuestionId = rnd.Next(1, 255);
            question.Prioritised = false;
            question.ValidDisplay = false;
            question.BackButtonEnabled = false;
            question.Image = false;
            await question.Create(dbContext);
            #endregion
            
            #region Option
            
            options option = new options();
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.NextQuestionId = rnd.Next(1, 255);
            option.ContinuousOptionId = rnd.Next(1, 255);
            await option.Create(dbContext);
            #endregion
            
            answer_values answerValue = new answer_values();
            answerValue.QuestionId = question.Id;
            answerValue.Value = rnd.Next(1, 255).ToString();
            answerValue.Answer = answer;
            answerValue.Option = option;
            answerValue.AnswerId = answer.Id;
            answerValue.Question = question;
            answerValue.OptionId = option.Id;
            await answerValue.Create(dbContext);

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