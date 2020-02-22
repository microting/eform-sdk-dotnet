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
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestQuestion : DbTestFixture
    {
        #region create
        [Test]
        public async Task question_Create_DoesCreate_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = true, 
                HasChild = true, 
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = true,
                BackButtonEnabled = true,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_PrioritisedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = true, 
                HasChild = true, 
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_ValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = true, 
                HasChild = true, 
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = true,
                BackButtonEnabled = false,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_BackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = true, 
                HasChild = true, 
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = true,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_ImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = true, 
                HasChild = true, 
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_PrioritisedValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = true, 
                HasChild = true,
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = true,
                BackButtonEnabled = false,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_PrioritisedBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name,
                Share = true, 
                HasChild = true,
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = false,
                BackButtonEnabled = true,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_PrioritisedImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = true, 
                HasChild = true,
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_ValidDisplayBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = true,
                HasChild = true,
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = true,
                BackButtonEnabled = true,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_ValidDisplayImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = true, 
                HasChild = true,
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = true,
                BackButtonEnabled = false,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_BackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name,
                Share = true, 
                HasChild = true,
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = true,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_PrioritisedValidDisplayBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name,
                Share = true,
                HasChild = true,
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = true,
                BackButtonEnabled = true,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_PrioritisedValidDisplayImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name,
                Share = true, 
                HasChild = true,
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = true,
                BackButtonEnabled = false,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_PrioritisedBackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = true, 
                HasChild = true,
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = false,
                BackButtonEnabled = true,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetTrue_ValidDisplayBackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name,
                Share = true, 
                HasChild = true,
                PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = true,
                BackButtonEnabled = true,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_PrioritisedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name,
                Share = false,
                HasChild = false,
                PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_ValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = true,
                BackButtonEnabled = false,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_BackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = true,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_ImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_PrioritisedValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = true,
                BackButtonEnabled = false,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_PrioritisedBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = false,
                BackButtonEnabled = true,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_PrioritisedImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_ValidDisplayBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = true,
                BackButtonEnabled = true,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_ValidDisplayImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = true,
                BackButtonEnabled = false,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_BackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new
                question_sets {Name = name, Share = false, HasChild = false, PosiblyDeployed = false};
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = true,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_PrioritisedValidDisplayBackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = true,
                BackButtonEnabled = true,
                Image = false
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_PrioritisedValidDisplayImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = true,
                BackButtonEnabled = false,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Create_DoesCreate_QuestionSetFalse_ValidDisplayBackButtonEnabledImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = true,
                BackButtonEnabled = true,
                Image = true
            };

            // Act
            await question.Create(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Update_DoesUpdate_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = true, HasChild = true, PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = false
            };
            await question.Create(dbContext);

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
            
            await question.Update(dbContext);
            
            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Update_DoesUpdate_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = true,
                BackButtonEnabled = true,
                Image = true
            };
            await question.Create(dbContext);

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
            
            await question.Update(dbContext);
            
            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_PrioritisedTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_ValidDisplayTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_BackButtonEnabledTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_ImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_PrioritisedValidDisplayTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_PrioritisedBackButtonEnabledTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_PrioritisedImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_ValidDisplayBackButtonEnabledTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_ValidDisplayImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_BackButtonEnabledImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_PrioritisedValidDisplayBackButtonEnabledTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_PrioritisedBackButtonEnabledImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
                public async Task question_Update_DoesUpdate_ValidDisplayBackButtonEnabledImageTrue()
                {
                    // Arrange
                    string name = Guid.NewGuid().ToString();
                    question_sets questionSet = new question_sets
                    {
                        Name = name, Share = true, HasChild = true, PosiblyDeployed = true
                    };
                    await questionSet.Create(dbContext);
        
                    Random rnd = new Random();
                    string type = Guid.NewGuid().ToString();
                    string questionType = Guid.NewGuid().ToString();
                    string imagePosition = Guid.NewGuid().ToString();
                    string fontSize = Guid.NewGuid().ToString();
                    questions question = new questions
                    {
                        Type = type,
                        QuestionType = questionType,
                        ImagePosition = imagePosition,
                        FontSize = fontSize,
                        QuestionSetId = questionSet.Id,
                        Maximum = rnd.Next(1, 255),
                        Minimum = rnd.Next(1, 255),
                        RefId = rnd.Next(1, 255),
                        MaxDuration = rnd.Next(1, 255),
                        MinDuration = rnd.Next(1, 255),
                        QuestionIndex = rnd.Next(1, 255),
                        ContinuousQuestionId = rnd.Next(1, 255),
                        Prioritised = false,
                        ValidDisplay = false,
                        BackButtonEnabled = false,
                        Image = false
                    };
                    await question.Create(dbContext);
        
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
                    
                    await question.Update(dbContext);
                    
                    questions dbQuestion = dbContext.questions.AsNoTracking().First();
                    question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_PrioritisedTrue_QSFalse()
            {
                // Arrange
                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);

    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_ValidDisplayTrue_QSFalse()
            {
                // Arrange
                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);

    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_BackButtonEnabledTrue_QSFalse()
            {
                // Arrange
                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);

    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_ImageTrue_QSFalse()
            {
                // Arrange
                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);

    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_PrioritisedValidDisplayTrue_QSFalse()
            {
                // Arrange
                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);

    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_PrioritisedBackButtonEnabledTrue_QSFalse()
            {
                // Arrange
                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);

    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_PrioritisedImageTrue_QSFalse()
            {
                // Arrange
                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);

    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_ValidDisplayBackButtonEnabledTrue_QSFalse()
            {
                // Arrange
                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);

    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_ValidDisplayImageTrue_QSFalse()
            {
                // Arrange
                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);

    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_BackButtonEnabledImageTrue_QSFalse()
            {
                // Arrange
                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);

    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_PrioritisedValidDisplayBackButtonEnabledTrue_QSFalse()
            {
                // Arrange
                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);

    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_PrioritisedBackButtonEnabledImageTrue_QSFalse()
            {
                // Arrange

                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);

    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
            public async Task question_Update_DoesUpdate_ValidDisplayBackButtonEnabledImageTrue_QSFalse()
            {
                // Arrange
                string name = Guid.NewGuid().ToString();
                question_sets questionSet = new question_sets
                {
                    Name = name, Share = false, HasChild = false, PosiblyDeployed = false
                };
                await questionSet.Create(dbContext);
    
                Random rnd = new Random();
                string type = Guid.NewGuid().ToString();
                string questionType = Guid.NewGuid().ToString();
                string imagePosition = Guid.NewGuid().ToString();
                string fontSize = Guid.NewGuid().ToString();
                questions question = new questions
                {
                    Type = type,
                    QuestionType = questionType,
                    ImagePosition = imagePosition,
                    FontSize = fontSize,
                    QuestionSetId = questionSet.Id,
                    Maximum = rnd.Next(1, 255),
                    Minimum = rnd.Next(1, 255),
                    RefId = rnd.Next(1, 255),
                    MaxDuration = rnd.Next(1, 255),
                    MinDuration = rnd.Next(1, 255),
                    QuestionIndex = rnd.Next(1, 255),
                    ContinuousQuestionId = rnd.Next(1, 255),
                    Prioritised = false,
                    ValidDisplay = false,
                    BackButtonEnabled = false,
                    Image = false
                };
                await question.Create(dbContext);
    
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
                
                await question.Update(dbContext);
                
                questions dbQuestion = dbContext.questions.AsNoTracking().First();
                question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Delete_DoesDelete_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = false
            };
            await question.Create(dbContext);

            // Act
                
            await question.Delete(dbContext);
            
            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Delete_DoesDelete_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = true, HasChild = true, PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = true,
                BackButtonEnabled = true,
                Image = true
            };
            await question.Create(dbContext);

            // Act

            await question.Delete(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Delete_DoesDelete_QuestionSetTrue_PrioritisedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = true, HasChild = true, PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = false
            };
            await question.Create(dbContext);

            // Act

            await question.Delete(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Delete_DoesDelete_QuestionSetTrue_ValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = true, HasChild = true, PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = true,
                BackButtonEnabled = false,
                Image = false
            };
            await question.Create(dbContext);

            // Act

            await question.Delete(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Delete_DoesDelete_QuestionSetTrue_BackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = true, HasChild = true, PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = true,
                Image = false
            };
            await question.Create(dbContext);

            // Act
            
            await question.Delete(dbContext);


            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Delete_DoesDelete_QuestionSetTrue_ImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = true, HasChild = true, PosiblyDeployed = true
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = true
            };
            await question.Create(dbContext);

            // Act

            await question.Delete(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Delete_DoesDelete_QuestionSetFalse_PrioritisedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = true,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = false
            };
            await question.Create(dbContext);

            // Act

            await question.Delete(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Delete_DoesDelete_QuestionSetFalse_ValidDisplayTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = true,
                BackButtonEnabled = false,
                Image = false
            };
            await question.Create(dbContext);

            // Act
            await question.Delete(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Delete_DoesDelete_QuestionSetFalse_BackButtonEnabledTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = true,
                Image = false
            };
            await question.Create(dbContext);

            // Act
            await question.Delete(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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
        public async Task question_Delete_DoesDelete_QuestionSetFalse_ImageTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, Share = false, HasChild = false, PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            Random rnd = new Random();
            string type = Guid.NewGuid().ToString();
            string questionType = Guid.NewGuid().ToString();
            string imagePosition = Guid.NewGuid().ToString();
            string fontSize = Guid.NewGuid().ToString();
            questions question = new questions
            {
                Type = type,
                QuestionType = questionType,
                ImagePosition = imagePosition,
                FontSize = fontSize,
                QuestionSetId = questionSet.Id,
                Maximum = rnd.Next(1, 255),
                Minimum = rnd.Next(1, 255),
                RefId = rnd.Next(1, 255),
                MaxDuration = rnd.Next(1, 255),
                MinDuration = rnd.Next(1, 255),
                QuestionIndex = rnd.Next(1, 255),
                ContinuousQuestionId = rnd.Next(1, 255),
                Prioritised = false,
                ValidDisplay = false,
                BackButtonEnabled = false,
                Image = true
            };
            await question.Create(dbContext);

            // Act
            await question.Delete(dbContext);

            questions dbQuestion = dbContext.questions.AsNoTracking().First();
            question_versions dbQuestionVersion = dbContext.question_versions.AsNoTracking().First();
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