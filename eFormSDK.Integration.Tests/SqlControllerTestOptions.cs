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
    public class SqlControllerTestOptions : DbTestFixture
    {
        [Test]
        public async Task options_Create_DoesCreate()
        {        
            // Arrange
            #region QuestionSet
            string name = Guid.NewGuid().ToString();
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = false, 
                HasChild = false, 
                PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            #endregion
            
            #region Question
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
            #endregion
            
            #region Option

            options option = new options
            {
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id,
                Weight = rnd.Next(1, 255),
                OptionsIndex = rnd.Next(1, 255),
                NextQuestionId = rnd.Next(1, 255),
                ContinuousOptionId = rnd.Next(1, 255)
            };

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
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = false, 
                HasChild = false, 
                PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            #endregion
            
            #region Question
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
            #endregion
            #region Question2
            string type2 = Guid.NewGuid().ToString();
            string questionType2 = Guid.NewGuid().ToString();
            string imagePosition2 = Guid.NewGuid().ToString();
            string fontSize2 = Guid.NewGuid().ToString();
            questions question2 = new questions
            {
                Type = type2,
                QuestionType = questionType2,
                ImagePosition = imagePosition2,
                FontSize = fontSize2,
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

            await question2.Create(dbContext);
            #endregion
            #region Option

            options option = new options
            {
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id,
                Weight = rnd.Next(1, 255),
                OptionsIndex = rnd.Next(1, 255),
                NextQuestionId = rnd.Next(1, 255),
                ContinuousOptionId = rnd.Next(1, 255)
            };

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
            question_sets questionSet = new question_sets
            {
                Name = name, 
                Share = false, 
                HasChild = false, 
                PosiblyDeployed = false
            };
            await questionSet.Create(dbContext);

            #endregion
            
            #region Question
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
            #endregion
            
            #region Option

            options option = new options
            {
                WeightValue = rnd.Next(1, 255),
                QuestionId = question.Id,
                Weight = rnd.Next(1, 255),
                OptionsIndex = rnd.Next(1, 255),
                NextQuestionId = rnd.Next(1, 255),
                ContinuousOptionId = rnd.Next(1, 255)
            };
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