using eFormCore;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;

namespace eFormSDK.Integration.Tests
{   
    [TestFixture]
    public class SqlControllerTestOptions : DbTestFixture
    {
        [Test]
        public async Task options_Create_DoesCreate()
        {        
            // Arrange
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            await questionSet.Create(dbContext);

            #endregion
            
            #region Question
            Random rnd = new Random();
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

            #endregion

            // Act
            await option.Create(dbContext);

            options dbOption = dbContext.options.AsNoTracking().First();
            option_versions dbVersions = dbContext.option_versions.AsNoTracking().First();
            //Assert
            Assert.NotNull(dbOption);
            Assert.NotNull(dbVersions);
            
            Assert.AreEqual(option.WeightValue , dbOption.WeightValue);
            Assert.AreEqual(option.Weight , dbOption.Weight);
            Assert.AreEqual(option.QuestionId , dbOption.QuestionId);
            Assert.AreEqual(option.OptionsIndex , dbOption.OptionsIndex);
            Assert.AreEqual(option.NextQuestionId , dbOption.NextQuestionId);
            Assert.AreEqual(option.ContinuousOptionId , dbOption.ContinuousOptionId);

        }
                [Test]
        public async Task options_Update_DoesUpdate()
        {        
            // Arrange
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            await questionSet.Create(dbContext);

            #endregion
            
            #region Question
            Random rnd = new Random();
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
            #region Question2
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
            
            await question2.Create(dbContext);
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

            // Act
            
            option.WeightValue = rnd.Next(1, 550);
            option.QuestionId = question2.Id;
            option.Weight = rnd.Next(1, 550);
            option.OptionsIndex = rnd.Next(1, 550);
            option.NextQuestionId = rnd.Next(1, 550);
            option.ContinuousOptionId = rnd.Next(1, 550);

            await option.Update(dbContext);
            
            options dbOption = dbContext.options.AsNoTracking().First();
            option_versions dbVersions = dbContext.option_versions.AsNoTracking().First();
            //Assert
            Assert.NotNull(dbOption);
            Assert.NotNull(dbVersions);
            
            Assert.AreEqual(option.WeightValue , dbOption.WeightValue);
            Assert.AreEqual(option.Weight , dbOption.Weight);
            Assert.AreEqual(option.QuestionId , dbOption.QuestionId);
            Assert.AreEqual(option.OptionsIndex , dbOption.OptionsIndex);
            Assert.AreEqual(option.NextQuestionId , dbOption.NextQuestionId);
            Assert.AreEqual(option.ContinuousOptionId , dbOption.ContinuousOptionId);

        }
                [Test]
        public async Task options_Delete_DoesDelete()
        {        
            // Arrange
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            await questionSet.Create(dbContext);

            #endregion
            
            #region Question
            Random rnd = new Random();
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

            // Act

            await option.Delete(dbContext);
            
            options dbOption = dbContext.options.AsNoTracking().First();
            option_versions dbVersions = dbContext.option_versions.AsNoTracking().First();
            //Assert
            Assert.NotNull(dbOption);
            Assert.NotNull(dbVersions);
            
            Assert.AreEqual(option.WeightValue , dbOption.WeightValue);
            Assert.AreEqual(option.Weight , dbOption.Weight);
            Assert.AreEqual(option.QuestionId , dbOption.QuestionId);
            Assert.AreEqual(option.OptionsIndex , dbOption.OptionsIndex);
            Assert.AreEqual(option.NextQuestionId , dbOption.NextQuestionId);
            Assert.AreEqual(option.ContinuousOptionId , dbOption.ContinuousOptionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbOption.WorkflowState);

        }
    }
}