/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 Microting A/S

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class AnswersUTest : DbTestFixture
    {
        [Test]
        public async Task Answers_Create_DoesCreate()
        {
            //Arrange
             Random rnd = new Random();

             bool randomBool = rnd.Next(0, 2) > 0;

             sites site = new sites();
             site.Name = Guid.NewGuid().ToString();
             site.MicrotingUid = rnd.Next(1, 255);
             await site.Create(dbContext);
             
             sites siteForUnit = new sites();
             siteForUnit.Name = Guid.NewGuid().ToString();
             siteForUnit.MicrotingUid = rnd.Next(1, 255);
             await siteForUnit.Create(dbContext);
             
             units unit = new units();
             unit.SiteId = siteForUnit.Id;
             unit.CustomerNo = rnd.Next(1, 255);
             unit.MicrotingUid = rnd.Next(1, 255);
             unit.OtpCode = rnd.Next(1, 255);
             await unit.Create(dbContext);

             languages language = new languages();
             language.Description = Guid.NewGuid().ToString();
             language.Name = Guid.NewGuid().ToString();
             await language.Create(dbContext);
             
             question_sets questionSet = new question_sets();
             questionSet.Name = Guid.NewGuid().ToString();
             questionSet.Share = randomBool;
             questionSet.HasChild = randomBool;
             questionSet.PosiblyDeployed = randomBool;
             await questionSet.Create(dbContext);
             
             survey_configurations surveyConfiguration = new survey_configurations();
             surveyConfiguration.Name = Guid.NewGuid().ToString();
             surveyConfiguration.Start = DateTime.Now;
             surveyConfiguration.Stop = DateTime.Now;
             surveyConfiguration.TimeOut = rnd.Next(1, 255);
             surveyConfiguration.TimeToLive = rnd.Next(1, 255);
             await surveyConfiguration.Create(dbContext);
             
             answers answer = new answers();
             answer.AnswerDuration = rnd.Next(1, 255);
             answer.FinishedAt = rnd.Next(1, 255);
             answer.LanguageId = language.Id;
             answer.Language = language;
             answer.SiteId = site.Id;
             answer.UnitId = unit.Id;
             answer.QuestionSetId = questionSet.Id;
             answer.SurveyConfigurationId = surveyConfiguration.Id;
             answer.TimeZone = Guid.NewGuid().ToString();
             answer.UtcAdjusted = randomBool;
             
             //Act
            
             await answer.Create(dbContext);

             List<answers> answers = dbContext.answers.AsNoTracking().ToList();
             List<answer_versions> answerVersions = dbContext.answer_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(answers);                                                             
            Assert.NotNull(answerVersions);                                                             

            Assert.AreEqual(1,answers.Count());  
            Assert.AreEqual(1,answerVersions.Count());  
            
            Assert.AreEqual(answer.CreatedAt.ToString(), answers[0].CreatedAt.ToString());                                  
            Assert.AreEqual(answer.Version, answers[0].Version);                                      
            Assert.AreEqual(answer.UpdatedAt.ToString(), answers[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(answers[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(answer.Id, answers[0].Id);
            Assert.AreEqual(answer.AnswerDuration, answers[0].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt, answers[0].FinishedAt); 
            Assert.AreEqual(answer.LanguageId, language.Id); 
            Assert.AreEqual(answer.SiteId, site.Id);
            Assert.AreEqual(answer.TimeZone, answers[0].TimeZone);
            Assert.AreEqual(answer.UnitId, unit.Id);
            Assert.AreEqual(answer.UtcAdjusted, answers[0].UtcAdjusted);
            Assert.AreEqual(answer.QuestionSetId, questionSet.Id);
            Assert.AreEqual(answer.SurveyConfigurationId, surveyConfiguration.Id);
            

            //Version 1
            Assert.AreEqual(answer.CreatedAt.ToString(), answerVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, answerVersions[0].Version);                                      
            Assert.AreEqual(answer.UpdatedAt.ToString(), answerVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(answerVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(answer.Id, answerVersions[0].AnswerId);
            Assert.AreEqual(answer.AnswerDuration, answerVersions[0].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt, answerVersions[0].FinishedAt); 
            Assert.AreEqual(language.Id, answerVersions[0].LanguageId); 
            Assert.AreEqual(site.Id, answerVersions[0].SiteId);
            Assert.AreEqual(answer.TimeZone, answerVersions[0].TimeZone);
            Assert.AreEqual(unit.Id, answerVersions[0].UnitId);
            Assert.AreEqual(answer.UtcAdjusted, answerVersions[0].UtcAdjusted);
            Assert.AreEqual(questionSet.Id, answerVersions[0].QuestionSetId);
            Assert.AreEqual(surveyConfiguration.Id, answerVersions[0].SurveyConfigurationId);
        }

        [Test]
        public async Task Answer_update_DoesUpdate()
        {
             Random rnd = new Random();

             bool randomBool = rnd.Next(0, 2) > 0;

             sites site = new sites();
             site.Name = Guid.NewGuid().ToString();
             site.MicrotingUid = rnd.Next(1, 255);
             await site.Create(dbContext);
             
             sites siteForUnit = new sites();
             siteForUnit.Name = Guid.NewGuid().ToString();
             siteForUnit.MicrotingUid = rnd.Next(1, 255);
             await siteForUnit.Create(dbContext);
             
             units unit = new units();
             unit.SiteId = siteForUnit.Id;
             unit.CustomerNo = rnd.Next(1, 255);
             unit.MicrotingUid = rnd.Next(1, 255);
             unit.OtpCode = rnd.Next(1, 255);
             await unit.Create(dbContext);
             
             languages language = new languages();
             language.Description = Guid.NewGuid().ToString();
             language.Name = Guid.NewGuid().ToString();
             await language.Create(dbContext);
             
             question_sets questionSet = new question_sets();
             questionSet.Name = Guid.NewGuid().ToString();
             questionSet.Share = randomBool;
             questionSet.HasChild = randomBool;
             questionSet.PosiblyDeployed = randomBool;
             await questionSet.Create(dbContext);
             
             survey_configurations surveyConfiguration = new survey_configurations();
             surveyConfiguration.Name = Guid.NewGuid().ToString();
             surveyConfiguration.Start = DateTime.Now;
             surveyConfiguration.Stop = DateTime.Now;
             surveyConfiguration.TimeOut = rnd.Next(1, 255);
             surveyConfiguration.TimeToLive = rnd.Next(1, 255);
             await surveyConfiguration.Create(dbContext);
             
             answers answer = new answers();
             answer.AnswerDuration = rnd.Next(1, 255);
             answer.FinishedAt = rnd.Next(1, 255);
             answer.LanguageId = language.Id;
             answer.SiteId = site.Id;
             answer.SurveyConfiguration = surveyConfiguration;
             answer.TimeZone = Guid.NewGuid().ToString();
             answer.UnitId = unit.Id;
             answer.UtcAdjusted = randomBool;
             answer.QuestionSetId = questionSet.Id;
             answer.SurveyConfigurationId = surveyConfiguration.Id;
             answer.Create(dbContext);
            
            //Act

            DateTime? oldUpdatedAt = answer.UpdatedAt;
            int oldAnswerDuration = answer.AnswerDuration;
            int oldFinishedAt = answer.FinishedAt;
            string oldTimeZone = answer.TimeZone;
            bool oldUtcAdjusted = answer.UtcAdjusted;
            
            answer.AnswerDuration = rnd.Next(1, 255);
            answer.FinishedAt = rnd.Next(1, 255);
            answer.TimeZone = Guid.NewGuid().ToString();
            answer.UtcAdjusted = randomBool;
            
            await answer.Update(dbContext);
            
            List<answers> answers = dbContext.answers.AsNoTracking().ToList();                            
            List<answer_versions> answerVersions = dbContext.answer_versions.AsNoTracking().ToList(); 
            
            //Assert
            
            Assert.NotNull(answers);                                                             
            Assert.NotNull(answerVersions);                                                          

            Assert.AreEqual(1,answers.Count());
            Assert.AreEqual(2,answerVersions.Count());  
            
            Assert.AreEqual(answer.CreatedAt.ToString(), answers[0].CreatedAt.ToString());                                  
            Assert.AreEqual(answer.Version, answers[0].Version);                                      
            Assert.AreEqual(answer.UpdatedAt.ToString(), answers[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(answers[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(answer.Id, answers[0].Id);
            Assert.AreEqual(answer.AnswerDuration, answers[0].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt, answers[0].FinishedAt); 
            Assert.AreEqual(answer.LanguageId, language.Id); 
            Assert.AreEqual(answer.SiteId, site.Id);
            Assert.AreEqual(answer.TimeZone, answers[0].TimeZone);
            Assert.AreEqual(answer.UnitId, unit.Id);
            Assert.AreEqual(answer.UtcAdjusted, answers[0].UtcAdjusted);
            Assert.AreEqual(answer.QuestionSetId, questionSet.Id);
            Assert.AreEqual(answer.SurveyConfigurationId, surveyConfiguration.Id);
            
            //Version 1 Old Version
            Assert.AreEqual(answer.CreatedAt.ToString(), answerVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, answerVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), answerVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(answerVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(oldAnswerDuration, answerVersions[0].AnswerDuration);
            Assert.AreEqual(oldFinishedAt, answerVersions[0].FinishedAt);
            Assert.AreEqual(oldUtcAdjusted, answerVersions[0].UtcAdjusted);
            Assert.AreEqual(oldTimeZone, answerVersions[0].TimeZone);


            //Version 2 Updated Version
            Assert.AreEqual(answer.CreatedAt.ToString(), answerVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, answerVersions[1].Version);                                      
            Assert.AreEqual(answer.UpdatedAt.ToString(), answerVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(answerVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(answer.Id, answerVersions[1].AnswerId);
            Assert.AreEqual(answer.AnswerDuration, answerVersions[1].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt, answerVersions[1].FinishedAt); 
            Assert.AreEqual(language.Id, answerVersions[1].LanguageId); 
            Assert.AreEqual(site.Id, answerVersions[1].SiteId);
            Assert.AreEqual(answer.TimeZone, answerVersions[1].TimeZone);
            Assert.AreEqual(unit.Id, answerVersions[1].UnitId);
            Assert.AreEqual(answer.UtcAdjusted, answerVersions[1].UtcAdjusted);
            Assert.AreEqual(questionSet.Id, answerVersions[1].QuestionSetId);
            Assert.AreEqual(surveyConfiguration.Id, answerVersions[1].SurveyConfigurationId);
        }

        [Test]
        public async Task Answer_Delete_DoesSetWorkflowStateToRemoved()
        {
             //Arrange
            
             Random rnd = new Random();

             bool randomBool = rnd.Next(0, 2) > 0;

             sites site = new sites();
             site.Name = Guid.NewGuid().ToString();
             site.MicrotingUid = rnd.Next(1, 255);
             await site.Create(dbContext);
             
             sites siteForUnit = new sites();
             siteForUnit.Name = Guid.NewGuid().ToString();
             siteForUnit.MicrotingUid = rnd.Next(1, 255);
             await siteForUnit.Create(dbContext);
             
             units unit = new units();
             unit.CustomerNo = rnd.Next(1, 255);
             unit.MicrotingUid = rnd.Next(1, 255);
             unit.OtpCode = rnd.Next(1, 255);
             unit.SiteId = siteForUnit.Id;
             await unit.Create(dbContext);
             
             languages language = new languages();
             language.Description = Guid.NewGuid().ToString();
             language.Name = Guid.NewGuid().ToString();
             await language.Create(dbContext);
             
             question_sets questionSet = new question_sets();
             questionSet.Name = Guid.NewGuid().ToString();
             questionSet.Share = randomBool;
             questionSet.HasChild = randomBool; 
             questionSet.PosiblyDeployed = randomBool;
             await questionSet.Create(dbContext);
             
             survey_configurations surveyConfiguration = new survey_configurations();
             surveyConfiguration.Name = Guid.NewGuid().ToString();
             surveyConfiguration.Start = DateTime.Now;
             surveyConfiguration.Stop = DateTime.Now;
             surveyConfiguration.TimeOut = rnd.Next(1, 255);
             surveyConfiguration.TimeToLive = rnd.Next(1, 255);
             await surveyConfiguration.Create(dbContext);
             
             answers answer = new answers();
             answer.AnswerDuration = rnd.Next(1, 255);
             answer.FinishedAt = rnd.Next(1, 255);
             answer.LanguageId = language.Id;
             answer.SiteId = site.Id;
             answer.TimeZone = Guid.NewGuid().ToString();
             answer.UnitId = unit.Id;
             answer.UtcAdjusted = randomBool;
             answer.QuestionSetId = questionSet.Id;
             answer.SurveyConfigurationId = surveyConfiguration.Id;
             await answer.Create(dbContext);
            
            //Act
            
            DateTime? oldUpdatedAt = answer.UpdatedAt;
            
            await answer.Delete(dbContext);
            
            List<answers> answers = dbContext.answers.AsNoTracking().ToList();                            
            List<answer_versions> answerVersions = dbContext.answer_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(answers);
            Assert.NotNull(answerVersions);
            
            Assert.AreEqual(1, answers.Count());
            Assert.AreEqual(2, answerVersions.Count());
            
            Assert.AreEqual(answer.CreatedAt.ToString(), answers[0].CreatedAt.ToString());                                  
            Assert.AreEqual(answer.Version, answers[0].Version);                                      
            Assert.AreEqual(answer.UpdatedAt.ToString(), answers[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(answers[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(answer.Id, answers[0].Id);
            Assert.AreEqual(answer.AnswerDuration, answers[0].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt, answers[0].FinishedAt); 
            Assert.AreEqual(answer.LanguageId, language.Id); 
            Assert.AreEqual(answer.SiteId, site.Id);
            Assert.AreEqual(answer.TimeZone, answers[0].TimeZone);
            Assert.AreEqual(answer.UnitId, unit.Id);
            Assert.AreEqual(answer.UtcAdjusted, answers[0].UtcAdjusted);
            Assert.AreEqual(answer.QuestionSetId, questionSet.Id);
            Assert.AreEqual(answer.SurveyConfigurationId, surveyConfiguration.Id);

            //Version 1 Old Version
            Assert.AreEqual(answer.CreatedAt.ToString(), answerVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, answerVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), answerVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(answerVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(answer.Id, answerVersions[0].AnswerId);
            Assert.AreEqual(answer.AnswerDuration, answerVersions[0].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt, answerVersions[0].FinishedAt); 
            Assert.AreEqual(language.Id, answerVersions[0].LanguageId); 
            Assert.AreEqual(site.Id, answerVersions[0].SiteId);
            Assert.AreEqual(answer.TimeZone, answerVersions[0].TimeZone);
            Assert.AreEqual(unit.Id, answerVersions[0].UnitId);
            Assert.AreEqual(answer.UtcAdjusted, answerVersions[0].UtcAdjusted);
            Assert.AreEqual(questionSet.Id, answerVersions[0].QuestionSetId);
            Assert.AreEqual(surveyConfiguration.Id, answerVersions[0].SurveyConfigurationId);

            //Version 2 Deleted Version
            Assert.AreEqual(answer.CreatedAt.ToString(), answerVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, answerVersions[1].Version);                                      
            Assert.AreEqual(answer.UpdatedAt.ToString(), answerVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(answer.Id, answerVersions[1].AnswerId);
            Assert.AreEqual(answer.AnswerDuration, answerVersions[1].AnswerDuration);
            Assert.AreEqual(answer.FinishedAt, answerVersions[1].FinishedAt); 
            Assert.AreEqual(language.Id, answerVersions[1].LanguageId); 
            Assert.AreEqual(site.Id, answerVersions[1].SiteId);
            Assert.AreEqual(answer.TimeZone, answerVersions[1].TimeZone);
            Assert.AreEqual(unit.Id, answerVersions[1].UnitId);
            Assert.AreEqual(answer.UtcAdjusted, answerVersions[1].UtcAdjusted);
            Assert.AreEqual(questionSet.Id, answerVersions[1].QuestionSetId);
            Assert.AreEqual(surveyConfiguration.Id, answerVersions[1].SurveyConfigurationId);
            Assert.AreEqual(answerVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
        }
        
    }
}