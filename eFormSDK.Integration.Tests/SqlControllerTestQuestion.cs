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
    public class SqlControllerTestQuestion : DbTestFixture
    {
        #region create
        [Test]
        public void question_Create_DoesCreate_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
        
        [Test]
        public void question_Create_DoesCreate_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = true;
            question.backButtonEnabled = true;
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
                
        #region QuestionSet True

        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = false;
            question.backButtonEnabled = false;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_ValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.validDisplay = true;
            question.backButtonEnabled = false;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_BackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.backButtonEnabled = true;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_ImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = true;
            question.backButtonEnabled = false;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = false;
            question.backButtonEnabled = true;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = false;
            question.backButtonEnabled = false;
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_ValidDisplayBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.validDisplay = true;
            question.backButtonEnabled = true;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_ValidDisplayImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.validDisplay = true;
            question.backButtonEnabled = false;
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_BackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.backButtonEnabled = true;
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
                       
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedValidDisplayBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = true;
            question.backButtonEnabled = true;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedValidDisplayImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = true;
            question.backButtonEnabled = false;
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedBackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = false;
            question.backButtonEnabled = true;
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
                       
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_ValidDisplayBackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.validDisplay = true;
            question.backButtonEnabled = true;
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
            
        #endregion
        
        #region QuestionSet False
        [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_PrioritisedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = false;
            question.backButtonEnabled = false;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
             
        [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_ValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.validDisplay = true;
            question.backButtonEnabled = false;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
             
        [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_BackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.backButtonEnabled = true;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
             
        [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_ImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_PrioritisedValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = true;
            question.backButtonEnabled = false;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_PrioritisedBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = false;
            question.backButtonEnabled = true;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_PrioritisedImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = false;
            question.backButtonEnabled = false;
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_ValidDisplayBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.validDisplay = true;
            question.backButtonEnabled = true;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
                        [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_ValidDisplayImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.validDisplay = true;
            question.backButtonEnabled = false;
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
                                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_BackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.backButtonEnabled = true;
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
                        [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_PrioritisedValidDisplayBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = true;
            question.backButtonEnabled = true;
            question.image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
                                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_PrioritisedValidDisplayImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = true;
            question.backButtonEnabled = false;
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
                                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_ValidDisplayBackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.validDisplay = true;
            question.backButtonEnabled = true;
            question.image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
        #endregion
        #endregion
        
        #region Update
        
        [Test]
        public void question_Update_DoesUpdate_Alltrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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

            // Act
            string newType = Guid.NewGuid().ToString();
            string newQuestionType = Guid.NewGuid().ToString();
            string newImagePosition = Guid.NewGuid().ToString();
            string newFontSize = Guid.NewGuid().ToString();

            question.type = newType;
            question.questionType = newQuestionType;
            question.imagePostion = newImagePosition;
            question.fontSize = newFontSize;
            question.questionSetId = questionSet.id;
            question.maximum = rnd.Next(1, 255);
            question.minimum = rnd.Next(1, 255);
            question.refId = rnd.Next(1, 255);
            question.maxDuration = rnd.Next(1, 255);
            question.minDuration = rnd.Next(1, 255);
            question.questionIndex = rnd.Next(1, 255);
            question.continuousQuestionId = rnd.Next(1, 255);
            question.prioritised = true;
            question.validDisplay = true;
            question.backButtonEnabled = true;
            question.image = true;
            
            question.Update(DbContext);
            
            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }
        
        [Test]
        public void question_Update_DoesUpdate_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = true;
            question.backButtonEnabled = true;
            question.image = true;
            question.Create(DbContext);

            // Act

            string newType = Guid.NewGuid().ToString();
            string newQuestionType = Guid.NewGuid().ToString();
            string newImagePosition = Guid.NewGuid().ToString();
            string newFontSize = Guid.NewGuid().ToString();

            question.type = newType;
            question.questionType = newQuestionType;
            question.imagePostion = newImagePosition;
            question.fontSize = newFontSize;
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
            
            question.Update(DbContext);
            
            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
        }

        #region QuestionSet_true

                [Test]
                public void question_Update_DoesUpdate_PrioritisedTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = true;
                    question.validDisplay = false;
                    question.backButtonEnabled = false;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                
                [Test]
                public void question_Update_DoesUpdate_ValidDisplayTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = false;
                    question.validDisplay = true;
                    question.backButtonEnabled = false;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                
                [Test]
                public void question_Update_DoesUpdate_BackButtonEnabledTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
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
                    question.backButtonEnabled = true;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                
                [Test]
                public void question_Update_DoesUpdate_ImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
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
                    question.image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedValidDisplayTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = true;
                    question.validDisplay = true;
                    question.backButtonEnabled = false;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedBackButtonEnabledTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = true;
                    question.validDisplay = false;
                    question.backButtonEnabled = true;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = true;
                    question.validDisplay = false;
                    question.backButtonEnabled = false;
                    question.image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_ValidDisplayBackButtonEnabledTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = false;
                    question.validDisplay = true;
                    question.backButtonEnabled = true;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
               [Test]
                public void question_Update_DoesUpdate_ValidDisplayImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = false;
                    question.validDisplay = true;
                    question.backButtonEnabled = false;
                    question.image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_BackButtonEnabledImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
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
                    question.backButtonEnabled = true;
                    question.image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedValidDisplayBacbButtonEnabledTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = true;
                    question.validDisplay = true;
                    question.backButtonEnabled = true;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedBackButtonEnabledImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = true;
                    question.validDisplay = false;
                    question.backButtonEnabled = true;
                    question.image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_ValidDisplayBackButtonEnabledImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = true;
                    questionSet.hasChild = true;
                    questionSet.posiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = false;
                    question.validDisplay = true;
                    question.backButtonEnabled = true;
                    question.image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
        #endregion

        #region QuestionSet_False

                [Test]
                public void question_Update_DoesUpdate_PrioritisedTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = true;
                    question.validDisplay = false;
                    question.backButtonEnabled = false;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                
                [Test]
                public void question_Update_DoesUpdate_ValidDisplayTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = false;
                    question.validDisplay = true;
                    question.backButtonEnabled = false;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                
                [Test]
                public void question_Update_DoesUpdate_BackButtonEnabledTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
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
                    question.backButtonEnabled = true;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                
                [Test]
                public void question_Update_DoesUpdate_ImageTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
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
                    question.image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedValidDisplayTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = true;
                    question.validDisplay = true;
                    question.backButtonEnabled = false;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedBackButtonEnabledTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = true;
                    question.validDisplay = false;
                    question.backButtonEnabled = true;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedImageTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = true;
                    question.validDisplay = false;
                    question.backButtonEnabled = false;
                    question.image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_ValidDisplayBackButtonEnabledTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = false;
                    question.validDisplay = true;
                    question.backButtonEnabled = true;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
               [Test]
                public void question_Update_DoesUpdate_ValidDisplayImageTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = false;
                    question.validDisplay = true;
                    question.backButtonEnabled = false;
                    question.image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_BackButtonEnabledImageTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
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
                    question.backButtonEnabled = true;
                    question.image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedValidDisplayBacbButtonEnabledTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = true;
                    question.validDisplay = true;
                    question.backButtonEnabled = true;
                    question.image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedBackButtonEnabledImageTrue_QSFalse()
                {
                    // Arrange

                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = true;
                    question.validDisplay = false;
                    question.backButtonEnabled = true;
                    question.image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_ValidDisplayBackButtonEnabledImageTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.name = name;
                    questionSet.share = false;
                    questionSet.hasChild = false;
                    questionSet.posiblyDeployed = false;
                    questionSet.Create(DbContext);
        
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
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.type = newType;
                    question.questionType = newQuestionType;
                    question.imagePostion = newImagePosition;
                    question.fontSize = newFontSize;
                    question.questionSetId = questionSet.id;
                    question.maximum = rnd.Next(1, 255);
                    question.minimum = rnd.Next(1, 255);
                    question.refId = rnd.Next(1, 255);
                    question.maxDuration = rnd.Next(1, 255);
                    question.minDuration = rnd.Next(1, 255);
                    question.questionIndex = rnd.Next(1, 255);
                    question.continuousQuestionId = rnd.Next(1, 255);
                    question.prioritised = false;
                    question.validDisplay = true;
                    question.backButtonEnabled = true;
                    question.image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.type , question.type);
                    Assert.AreEqual(dbQuestion.image , question.image);
                    Assert.AreEqual(dbQuestion.maximum , question.maximum);
                    Assert.AreEqual(dbQuestion.minimum , question.minimum);
                    Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
                    Assert.AreEqual(dbQuestion.refId , question.refId);
                    Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
                    Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
                    Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
                    Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
                    Assert.AreEqual(dbQuestion.questionType , question.questionType);
                    Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
                    Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
                    Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
                    Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
                    Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
                }
        #endregion

        
        #endregion

        #region Delete
 [Test]
        public void question_Delete_DoesDelete_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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

            // Act
                
            question.Delete(DbContext);
            
            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.workflow_state);
        }
        
        [Test]
        public void question_Delete_DoesDelete_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = true;
            question.backButtonEnabled = true;
            question.image = true;
            question.Create(DbContext);

            // Act

            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.workflow_state);
        }
        
                #region QuestionSet True

        [Test]
        public void question_Delete_DoesDelete_QuestionSetTrue_PrioritisedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = false;
            question.backButtonEnabled = false;
            question.image = false;
            question.Create(DbContext);

            // Act

            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.workflow_state);
        }
               
        [Test]
        public void question_Delete_DoesDelete_QuestionSetTrue_ValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.validDisplay = true;
            question.backButtonEnabled = false;
            question.image = false;
            question.Create(DbContext);

            // Act

            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.workflow_state);
        }
               
        [Test]
        public void question_Delete_DoesDelete_QuestionSetTrue_BackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.backButtonEnabled = true;
            question.image = false;
            question.Create(DbContext);

            // Act
            
            question.Delete(DbContext);


            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.workflow_state);
        }
               
        [Test]
        public void question_Delete_DoesDelete_QuestionSetTrue_ImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.image = true;
            question.Create(DbContext);

            // Act

            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.workflow_state);
        }
            
        #endregion
        
        #region QuestionSet False
        [Test]
        public void question_Delete_DoesDelete_QuestionSetFalse_PrioritisedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.prioritised = true;
            question.validDisplay = false;
            question.backButtonEnabled = false;
            question.image = false;
            question.Create(DbContext);

            // Act

            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.workflow_state);
        }
             
        [Test]
        public void question_Delete_DoesDelete_QuestionSetFalse_ValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.validDisplay = true;
            question.backButtonEnabled = false;
            question.image = false;
            question.Create(DbContext);

            // Act
            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.workflow_state);
        }
             
        [Test]
        public void question_Delete_DoesDelete_QuestionSetFalse_BackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.backButtonEnabled = true;
            question.image = false;
            question.Create(DbContext);

            // Act
            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.workflow_state);
        }
             
        [Test]
        public void question_Delete_DoesDelete_QuestionSetFalse_ImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.image = true;
            question.Create(DbContext);

            // Act
            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.type , question.type);
            Assert.AreEqual(dbQuestion.image , question.image);
            Assert.AreEqual(dbQuestion.maximum , question.maximum);
            Assert.AreEqual(dbQuestion.minimum , question.minimum);
            Assert.AreEqual(dbQuestion.prioritised , question.prioritised);
            Assert.AreEqual(dbQuestion.refId , question.refId);
            Assert.AreEqual(dbQuestion.fontSize , question.fontSize);
            Assert.AreEqual(dbQuestion.maxDuration , question.maxDuration);
            Assert.AreEqual(dbQuestion.minDuration , question.minDuration);
            Assert.AreEqual(dbQuestion.imagePostion , question.imagePostion);
            Assert.AreEqual(dbQuestion.questionType , question.questionType);
            Assert.AreEqual(dbQuestion.validDisplay , question.validDisplay);
            Assert.AreEqual(dbQuestion.questionIndex , question.questionIndex);
            Assert.AreEqual(dbQuestion.questionSetId , question.questionSetId);
            Assert.AreEqual(dbQuestion.backButtonEnabled , question.backButtonEnabled);
            Assert.AreEqual(dbQuestion.continuousQuestionId , question.continuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.workflow_state);
        }
        #endregion

        #endregion
    }
}