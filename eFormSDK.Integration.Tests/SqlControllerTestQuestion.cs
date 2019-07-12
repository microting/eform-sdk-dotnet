using eFormCore;
using eFormShared;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;

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
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
        
        [Test]
        public void question_Create_DoesCreate_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = true;
            question.BackButtonEnabled = true;
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
                
        #region QuestionSet True

        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = false;
            question.BackButtonEnabled = false;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_ValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.ValidDisplay = true;
            question.BackButtonEnabled = false;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_BackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.BackButtonEnabled = true;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_ImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = true;
            question.BackButtonEnabled = false;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = false;
            question.BackButtonEnabled = true;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = false;
            question.BackButtonEnabled = false;
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_ValidDisplayBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.ValidDisplay = true;
            question.BackButtonEnabled = true;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_ValidDisplayImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.ValidDisplay = true;
            question.BackButtonEnabled = false;
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_BackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.BackButtonEnabled = true;
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
                       
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedValidDisplayBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = true;
            question.BackButtonEnabled = true;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedValidDisplayImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = true;
            question.BackButtonEnabled = false;
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
               
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_PrioritisedBackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = false;
            question.BackButtonEnabled = true;
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
                       
        [Test]
        public void question_Create_DoesCreate_QuestionSetTrue_ValidDisplayBackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.ValidDisplay = true;
            question.BackButtonEnabled = true;
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
            
        #endregion
        
        #region QuestionSet False
        [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_PrioritisedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = false;
            question.BackButtonEnabled = false;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
             
        [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_ValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.ValidDisplay = true;
            question.BackButtonEnabled = false;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
             
        [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_BackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.BackButtonEnabled = true;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
             
        [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_ImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_PrioritisedValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = true;
            question.BackButtonEnabled = false;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_PrioritisedBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = false;
            question.BackButtonEnabled = true;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_PrioritisedImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = false;
            question.BackButtonEnabled = false;
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_ValidDisplayBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.ValidDisplay = true;
            question.BackButtonEnabled = true;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
                        [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_ValidDisplayImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.ValidDisplay = true;
            question.BackButtonEnabled = false;
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
                                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_BackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.BackButtonEnabled = true;
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
                        [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_PrioritisedValidDisplayBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = true;
            question.BackButtonEnabled = true;
            question.Image = false;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
                                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_PrioritisedValidDisplayImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = true;
            question.BackButtonEnabled = false;
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
                                [Test]
        public void question_Create_DoesCreate_QuestionSetFalse_ValidDisplayBackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.ValidDisplay = true;
            question.BackButtonEnabled = true;
            question.Image = true;

            // Act
            question.Create(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
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
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Create(DbContext);

            // Act
            string newType = Guid.NewGuid().ToString();
            string newQuestionType = Guid.NewGuid().ToString();
            string newImagePosition = Guid.NewGuid().ToString();
            string newFontSize = Guid.NewGuid().ToString();

            question.Type = newType;
            question.QuestionType = newQuestionType;
            question.ImagePosition = newImagePosition;
            question.FontSize = newFontSize;
            question.QuestionSetId = questionSet.Id;
            question.Maximum = rnd.Next(1, 255);
            question.Minimum = rnd.Next(1, 255);
            question.RefId = rnd.Next(1, 255);
            question.MaxDuration = rnd.Next(1, 255);
            question.MinDuration = rnd.Next(1, 255);
            question.QuestionIndex = rnd.Next(1, 255);
            question.ContinuousQuestionId = rnd.Next(1, 255);
            question.Prioritised = true;
            question.ValidDisplay = true;
            question.BackButtonEnabled = true;
            question.Image = true;
            
            question.Update(DbContext);
            
            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }
        
        [Test]
        public void question_Update_DoesUpdate_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = true;
            question.BackButtonEnabled = true;
            question.Image = true;
            question.Create(DbContext);

            // Act

            string newType = Guid.NewGuid().ToString();
            string newQuestionType = Guid.NewGuid().ToString();
            string newImagePosition = Guid.NewGuid().ToString();
            string newFontSize = Guid.NewGuid().ToString();

            question.Type = newType;
            question.QuestionType = newQuestionType;
            question.ImagePosition = newImagePosition;
            question.FontSize = newFontSize;
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
            
            question.Update(DbContext);
            
            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
        }

        #region QuestionSet_true

                [Test]
                public void question_Update_DoesUpdate_PrioritisedTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = true;
                    question.ValidDisplay = false;
                    question.BackButtonEnabled = false;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                
                [Test]
                public void question_Update_DoesUpdate_ValidDisplayTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = false;
                    question.ValidDisplay = true;
                    question.BackButtonEnabled = false;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                
                [Test]
                public void question_Update_DoesUpdate_BackButtonEnabledTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
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
                    question.BackButtonEnabled = true;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                
                [Test]
                public void question_Update_DoesUpdate_ImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
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
                    question.Image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedValidDisplayTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = true;
                    question.ValidDisplay = true;
                    question.BackButtonEnabled = false;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedBackButtonEnabledTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = true;
                    question.ValidDisplay = false;
                    question.BackButtonEnabled = true;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = true;
                    question.ValidDisplay = false;
                    question.BackButtonEnabled = false;
                    question.Image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_ValidDisplayBackButtonEnabledTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = false;
                    question.ValidDisplay = true;
                    question.BackButtonEnabled = true;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
               [Test]
                public void question_Update_DoesUpdate_ValidDisplayImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = false;
                    question.ValidDisplay = true;
                    question.BackButtonEnabled = false;
                    question.Image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_BackButtonEnabledImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
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
                    question.BackButtonEnabled = true;
                    question.Image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedValidDisplayBacbButtonEnabledTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = true;
                    question.ValidDisplay = true;
                    question.BackButtonEnabled = true;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedBackButtonEnabledImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = true;
                    question.ValidDisplay = false;
                    question.BackButtonEnabled = true;
                    question.Image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_ValidDisplayBackButtonEnabledImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = true;
                    questionSet.HasChild = true;
                    questionSet.PosiblyDeployed = true;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = false;
                    question.ValidDisplay = true;
                    question.BackButtonEnabled = true;
                    question.Image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
        #endregion

        #region QuestionSet_False

                [Test]
                public void question_Update_DoesUpdate_PrioritisedTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = true;
                    question.ValidDisplay = false;
                    question.BackButtonEnabled = false;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                
                [Test]
                public void question_Update_DoesUpdate_ValidDisplayTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = false;
                    question.ValidDisplay = true;
                    question.BackButtonEnabled = false;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                
                [Test]
                public void question_Update_DoesUpdate_BackButtonEnabledTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
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
                    question.BackButtonEnabled = true;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                
                [Test]
                public void question_Update_DoesUpdate_ImageTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
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
                    question.Image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedValidDisplayTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = true;
                    question.ValidDisplay = true;
                    question.BackButtonEnabled = false;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedBackButtonEnabledTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = true;
                    question.ValidDisplay = false;
                    question.BackButtonEnabled = true;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedImageTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = true;
                    question.ValidDisplay = false;
                    question.BackButtonEnabled = false;
                    question.Image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_ValidDisplayBackButtonEnabledTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = false;
                    question.ValidDisplay = true;
                    question.BackButtonEnabled = true;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
               [Test]
                public void question_Update_DoesUpdate_ValidDisplayImageTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = false;
                    question.ValidDisplay = true;
                    question.BackButtonEnabled = false;
                    question.Image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_BackButtonEnabledImageTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
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
                    question.BackButtonEnabled = true;
                    question.Image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedValidDisplayBacbButtonEnabledTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = true;
                    question.ValidDisplay = true;
                    question.BackButtonEnabled = true;
                    question.Image = false;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_PrioritisedBackButtonEnabledImageTrue_QSFalse()
                {
                    // Arrange

                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);

        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = true;
                    question.ValidDisplay = false;
                    question.BackButtonEnabled = true;
                    question.Image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
                }
                                [Test]
                public void question_Update_DoesUpdate_ValidDisplayBackButtonEnabledImageTrue_QSFalse()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets();
                    questionSet.Name = name;
                    questionSet.Share = false;
                    questionSet.HasChild = false;
                    questionSet.PosiblyDeployed = false;
                    questionSet.Create(DbContext);
        
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
                    question.Create(DbContext);
        
                    // Act
                    string newType = Guid.NewGuid().ToString();
                    string newQuestionType = Guid.NewGuid().ToString();
                    string newImagePosition = Guid.NewGuid().ToString();
                    string newFontSize = Guid.NewGuid().ToString();
        
                    question.Type = newType;
                    question.QuestionType = newQuestionType;
                    question.ImagePosition = newImagePosition;
                    question.FontSize = newFontSize;
                    question.QuestionSetId = questionSet.Id;
                    question.Maximum = rnd.Next(1, 255);
                    question.Minimum = rnd.Next(1, 255);
                    question.RefId = rnd.Next(1, 255);
                    question.MaxDuration = rnd.Next(1, 255);
                    question.MinDuration = rnd.Next(1, 255);
                    question.QuestionIndex = rnd.Next(1, 255);
                    question.ContinuousQuestionId = rnd.Next(1, 255);
                    question.Prioritised = false;
                    question.ValidDisplay = true;
                    question.BackButtonEnabled = true;
                    question.Image = true;
                    
                    question.Update(DbContext);
                    
                    questions dbQuestion = DbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
                    // Assert
                    Assert.NotNull(dbQuestion);
                    Assert.NotNull(dbQuestionVersion);
                    
                    Assert.AreEqual(dbQuestion.Type , question.Type);
                    Assert.AreEqual(dbQuestion.Image , question.Image);
                    Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
                    Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
                    Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
                    Assert.AreEqual(dbQuestion.RefId , question.RefId);
                    Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
                    Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
                    Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
                    Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
                    Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
                    Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
                    Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
                    Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
                    Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
                    Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
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
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.Create(DbContext);

            // Act
                
            question.Delete(DbContext);
            
            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.WorkflowState);
        }
        
        [Test]
        public void question_Delete_DoesDelete_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = true;
            question.BackButtonEnabled = true;
            question.Image = true;
            question.Create(DbContext);

            // Act

            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.WorkflowState);
        }
        
                #region QuestionSet True

        [Test]
        public void question_Delete_DoesDelete_QuestionSetTrue_PrioritisedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = false;
            question.BackButtonEnabled = false;
            question.Image = false;
            question.Create(DbContext);

            // Act

            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.WorkflowState);
        }
               
        [Test]
        public void question_Delete_DoesDelete_QuestionSetTrue_ValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.ValidDisplay = true;
            question.BackButtonEnabled = false;
            question.Image = false;
            question.Create(DbContext);

            // Act

            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.WorkflowState);
        }
               
        [Test]
        public void question_Delete_DoesDelete_QuestionSetTrue_BackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.BackButtonEnabled = true;
            question.Image = false;
            question.Create(DbContext);

            // Act
            
            question.Delete(DbContext);


            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.WorkflowState);
        }
               
        [Test]
        public void question_Delete_DoesDelete_QuestionSetTrue_ImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            questionSet.Create(DbContext);

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
            question.Image = true;
            question.Create(DbContext);

            // Act

            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.WorkflowState);
        }
            
        #endregion
        
        #region QuestionSet False
        [Test]
        public void question_Delete_DoesDelete_QuestionSetFalse_PrioritisedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.Prioritised = true;
            question.ValidDisplay = false;
            question.BackButtonEnabled = false;
            question.Image = false;
            question.Create(DbContext);

            // Act

            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.WorkflowState);
        }
             
        [Test]
        public void question_Delete_DoesDelete_QuestionSetFalse_ValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.ValidDisplay = true;
            question.BackButtonEnabled = false;
            question.Image = false;
            question.Create(DbContext);

            // Act
            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.WorkflowState);
        }
             
        [Test]
        public void question_Delete_DoesDelete_QuestionSetFalse_BackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.BackButtonEnabled = true;
            question.Image = false;
            question.Create(DbContext);

            // Act
            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.WorkflowState);
        }
             
        [Test]
        public void question_Delete_DoesDelete_QuestionSetFalse_ImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            questionSet.Create(DbContext);

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
            question.Image = true;
            question.Create(DbContext);

            // Act
            question.Delete(DbContext);

            questions dbQuestion = DbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = DbContext.question_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestion);
            Assert.NotNull(dbQuestionVersion);
            
            Assert.AreEqual(dbQuestion.Type , question.Type);
            Assert.AreEqual(dbQuestion.Image , question.Image);
            Assert.AreEqual(dbQuestion.Maximum , question.Maximum);
            Assert.AreEqual(dbQuestion.Minimum , question.Minimum);
            Assert.AreEqual(dbQuestion.Prioritised , question.Prioritised);
            Assert.AreEqual(dbQuestion.RefId , question.RefId);
            Assert.AreEqual(dbQuestion.FontSize , question.FontSize);
            Assert.AreEqual(dbQuestion.MaxDuration , question.MaxDuration);
            Assert.AreEqual(dbQuestion.MinDuration , question.MinDuration);
            Assert.AreEqual(dbQuestion.ImagePosition , question.ImagePosition);
            Assert.AreEqual(dbQuestion.QuestionType , question.QuestionType);
            Assert.AreEqual(dbQuestion.ValidDisplay , question.ValidDisplay);
            Assert.AreEqual(dbQuestion.QuestionIndex , question.QuestionIndex);
            Assert.AreEqual(dbQuestion.QuestionSetId , question.QuestionSetId);
            Assert.AreEqual(dbQuestion.BackButtonEnabled , question.BackButtonEnabled);
            Assert.AreEqual(dbQuestion.ContinuousQuestionId , question.ContinuousQuestionId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, dbQuestion.WorkflowState);
        }
        #endregion

        #endregion
    }
}