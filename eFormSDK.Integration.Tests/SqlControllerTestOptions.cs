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
    public class SqlControllerTestOptions : DbTestFixture
    {
        [Test]
        public void options_Create_DoesCreate()
        {        
            // Arrange
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

            #endregion
            
            #region Question
            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions();
            question.type = type;
            question.questionType = questionType;
            question.imagePostion = imagePosition;
            question.fontSize = fontSize;
            question.questionSetId = questionSet.id;
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
            option.questionId = question.id;
            option.weight = rnd.Next(1, 255);
            option.optionsIndex = rnd.Next(1, 255);
            option.nextQuestionId = rnd.Next(1, 255);
            option.continuousOptionId = rnd.Next(1, 255);

            #endregion

            // Act
            option.Create(DbContext);

            options dbOption = DbContext.options.AsNoTracking().First();
            option_versions dbVersions = DbContext.option_versions.AsNoTracking().First();
            //Assert
            Assert.NotNull(dbOption);
            Assert.NotNull(dbVersions);
            
            Assert.AreEqual(option.weightValue , dbOption.weightValue);
            Assert.AreEqual(option.weight , dbOption.weight);
            Assert.AreEqual(option.questionId , dbOption.questionId);
            Assert.AreEqual(option.optionsIndex , dbOption.optionsIndex);
            Assert.AreEqual(option.nextQuestionId , dbOption.nextQuestionId);
            Assert.AreEqual(option.continuousOptionId , dbOption.continuousOptionId);

        }
                [Test]
        public void options_Update_DoesUpdate()
        {        
            // Arrange
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

            #endregion
            
            #region Question
            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions();
            question.type = type;
            question.questionType = questionType;
            question.imagePostion = imagePosition;
            question.fontSize = fontSize;
            question.questionSetId = questionSet.id;
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
            #region Question2
            string type2 = Guid.NewGuid().ToString();
            string questionType2 = Guid.NewGuid().ToString();
            string imagePosition2 = Guid.NewGuid().ToString();
            string fontSize2 = Guid.NewGuid().ToString();
            questions question2 = new questions();
            question2.type = type2;
            question2.questionType = questionType2;
            question2.imagePostion = imagePosition2;
            question2.fontSize = fontSize2;
            question2.questionSetId = questionSet.id;
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
            
            question2.Create(DbContext);
            #endregion
            #region Option
            
            options option = new options();
            option.weightValue = rnd.Next(1, 255);
            option.questionId = question.id;
            option.weight = rnd.Next(1, 255);
            option.optionsIndex = rnd.Next(1, 255);
            option.nextQuestionId = rnd.Next(1, 255);
            option.continuousOptionId = rnd.Next(1, 255);

            option.Create(DbContext);

            #endregion

            // Act
            
            option.weightValue = rnd.Next(1, 550);
            option.questionId = question2.id;
            option.weight = rnd.Next(1, 550);
            option.optionsIndex = rnd.Next(1, 550);
            option.nextQuestionId = rnd.Next(1, 550);
            option.continuousOptionId = rnd.Next(1, 550);

            option.Update(DbContext);
            
            options dbOption = DbContext.options.AsNoTracking().First();
            option_versions dbVersions = DbContext.option_versions.AsNoTracking().First();
            //Assert
            Assert.NotNull(dbOption);
            Assert.NotNull(dbVersions);
            
            Assert.AreEqual(option.weightValue , dbOption.weightValue);
            Assert.AreEqual(option.weight , dbOption.weight);
            Assert.AreEqual(option.questionId , dbOption.questionId);
            Assert.AreEqual(option.optionsIndex , dbOption.optionsIndex);
            Assert.AreEqual(option.nextQuestionId , dbOption.nextQuestionId);
            Assert.AreEqual(option.continuousOptionId , dbOption.continuousOptionId);

        }
                [Test]
        public void options_Delete_DoesDelete()
        {        
            // Arrange
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

            #endregion
            
            #region Question
            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions();
            question.type = type;
            question.questionType = questionType;
            question.imagePostion = imagePosition;
            question.fontSize = fontSize;
            question.questionSetId = questionSet.id;
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
            option.questionId = question.id;
            option.weight = rnd.Next(1, 255);
            option.optionsIndex = rnd.Next(1, 255);
            option.nextQuestionId = rnd.Next(1, 255);
            option.continuousOptionId = rnd.Next(1, 255);
            option.Create(DbContext);

            #endregion

            // Act

            option.Delete(DbContext);
            
            options dbOption = DbContext.options.AsNoTracking().First();
            option_versions dbVersions = DbContext.option_versions.AsNoTracking().First();
            //Assert
            Assert.NotNull(dbOption);
            Assert.NotNull(dbVersions);
            
            Assert.AreEqual(option.weightValue , dbOption.weightValue);
            Assert.AreEqual(option.weight , dbOption.weight);
            Assert.AreEqual(option.questionId , dbOption.questionId);
            Assert.AreEqual(option.optionsIndex , dbOption.optionsIndex);
            Assert.AreEqual(option.nextQuestionId , dbOption.nextQuestionId);
            Assert.AreEqual(option.continuousOptionId , dbOption.continuousOptionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbOption.workflow_state);

        }
    }
}