using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class LanguageQuestionSetUTest : DbTestFixture
    {
        [Test]
        public async Task LanguageQuestionSet_Create_DoesCreate_W_MicrotingUid()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);

            language_question_sets languageQuestionSet = new language_question_sets
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            
            //Act
            await languageQuestionSet.Create(dbContext);

            List<language_question_sets> languageQuestionSets = dbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<language_question_set_versions> languageQuestionSetVersions =
                dbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);
            
            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(1, languageQuestionSetVersions.Count);
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSets[0].MicrotingUid);
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSetVersions[0].MicrotingUid);
        }

        [Test]
        public async Task LanguageQuestionSet_Create_DoesCreate_WO_MicrotingUid()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);

            language_question_sets languageQuestionSet = new language_question_sets
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
            };
            
            //Act
            await languageQuestionSet.Create(dbContext);

            List<language_question_sets> languageQuestionSets = dbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<language_question_set_versions> languageQuestionSetVersions =
                dbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);
            
            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(1, languageQuestionSetVersions.Count);
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSets[0].MicrotingUid);
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSetVersions[0].MicrotingUid);
        }
        
        [Test]
        public async Task LanguageQuestionSet_Update_DoesUpdate_W_MicrotingUid()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            question_sets questionSetForQuestion2 = new question_sets();
            questionSetForQuestion2.Name = Guid.NewGuid().ToString();
            questionSetForQuestion2.Share = randomBool;
            questionSetForQuestion2.HasChild = randomBool;
            questionSetForQuestion2.ParentId = rnd.Next(1, 255);
            questionSetForQuestion2.PosiblyDeployed = randomBool;
            await questionSetForQuestion2.Create(dbContext);
            
            languages language2 = new languages();
            language2.Description = Guid.NewGuid().ToString();
            language2.Name = Guid.NewGuid().ToString();
            await language2.Create(dbContext);

            language_question_sets languageQuestionSet = new language_question_sets
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            await languageQuestionSet.Create(dbContext);

            var oldLanguageId = languageQuestionSet.LanguageId;
            var oldQuestionSetId = languageQuestionSet.QuestionSetId;
            var oldMicrotingUid = languageQuestionSet.MicrotingUid;

            languageQuestionSet.LanguageId = language2.Id;
            languageQuestionSet.QuestionSetId = questionSetForQuestion2.Id;
            languageQuestionSet.MicrotingUid = rnd.Next(1, 255);
            
            
            //Act
            await languageQuestionSet.Update(dbContext);    
            
            List<language_question_sets> languageQuestionSets = dbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<language_question_set_versions> languageQuestionSetVersions =
                dbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);
            
            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(2, languageQuestionSetVersions.Count);
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSets[0].MicrotingUid);
            
            Assert.AreEqual(oldLanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(oldQuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(oldMicrotingUid, languageQuestionSetVersions[0].MicrotingUid);
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[1].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[1].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSetVersions[1].MicrotingUid);

        }

        [Test]
        public async Task LanguageQuestionSet_Update_DoesUpdate_WO_MicrotingUid()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
           
            question_sets questionSetForQuestion2 = new question_sets();
            questionSetForQuestion2.Name = Guid.NewGuid().ToString();
            questionSetForQuestion2.Share = randomBool;
            questionSetForQuestion2.HasChild = randomBool;
            questionSetForQuestion2.ParentId = rnd.Next(1, 255);
            questionSetForQuestion2.PosiblyDeployed = randomBool;
            await questionSetForQuestion2.Create(dbContext);
            
            languages language2 = new languages();
            language2.Description = Guid.NewGuid().ToString();
            language2.Name = Guid.NewGuid().ToString();
            await language2.Create(dbContext);
            
            language_question_sets languageQuestionSet = new language_question_sets
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
            };
            await languageQuestionSet.Create(dbContext);
            
            var oldLanguageId = languageQuestionSet.LanguageId;
            var oldQuestionSetId = languageQuestionSet.QuestionSetId;
            
            languageQuestionSet.LanguageId = language2.Id;
            languageQuestionSet.QuestionSetId = questionSetForQuestion2.Id;
            
            //Act
            await languageQuestionSet.Update(dbContext);
            
            List<language_question_sets> languageQuestionSets = dbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<language_question_set_versions> languageQuestionSetVersions =
                dbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);
            
            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(2, languageQuestionSetVersions.Count);
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSets[0].MicrotingUid);
            
            Assert.AreEqual(oldLanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(oldQuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSetVersions[0].MicrotingUid);        
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[1].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[1].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSetVersions[1].MicrotingUid);
        }
        
        [Test]
        public async Task LanguageQuestionSet_Update_DoesUpdate_W_MicrotingUid_RemovesUid()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            question_sets questionSetForQuestion2 = new question_sets();
            questionSetForQuestion2.Name = Guid.NewGuid().ToString();
            questionSetForQuestion2.Share = randomBool;
            questionSetForQuestion2.HasChild = randomBool;
            questionSetForQuestion2.ParentId = rnd.Next(1, 255);
            questionSetForQuestion2.PosiblyDeployed = randomBool;
            await questionSetForQuestion2.Create(dbContext);
            
            languages language2 = new languages();
            language2.Description = Guid.NewGuid().ToString();
            language2.Name = Guid.NewGuid().ToString();
            await language2.Create(dbContext);

            language_question_sets languageQuestionSet = new language_question_sets
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            await languageQuestionSet.Create(dbContext);

            var oldLanguageId = languageQuestionSet.LanguageId;
            var oldQuestionSetId = languageQuestionSet.QuestionSetId;
            var oldMicrotingUid = languageQuestionSet.MicrotingUid;

            languageQuestionSet.LanguageId = language2.Id;
            languageQuestionSet.QuestionSetId = questionSetForQuestion2.Id;
            languageQuestionSet.MicrotingUid = null;
            
            
            //Act
            await languageQuestionSet.Update(dbContext);    
            
            List<language_question_sets> languageQuestionSets = dbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<language_question_set_versions> languageQuestionSetVersions =
                dbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);
            
            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(2, languageQuestionSetVersions.Count);
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSets[0].MicrotingUid);
            
            Assert.AreEqual(oldLanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(oldQuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(oldMicrotingUid, languageQuestionSetVersions[0].MicrotingUid);
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[1].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[1].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSetVersions[1].MicrotingUid);

        }

        [Test]
        public async Task LanguageQuestionSet_Update_DoesUpdate_WO_MicrotingUid_AddsUid()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
           
            question_sets questionSetForQuestion2 = new question_sets();
            questionSetForQuestion2.Name = Guid.NewGuid().ToString();
            questionSetForQuestion2.Share = randomBool;
            questionSetForQuestion2.HasChild = randomBool;
            questionSetForQuestion2.ParentId = rnd.Next(1, 255);
            questionSetForQuestion2.PosiblyDeployed = randomBool;
            await questionSetForQuestion2.Create(dbContext);
            
            languages language2 = new languages();
            language2.Description = Guid.NewGuid().ToString();
            language2.Name = Guid.NewGuid().ToString();
            await language2.Create(dbContext);
            
            language_question_sets languageQuestionSet = new language_question_sets
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
            };
            await languageQuestionSet.Create(dbContext);
            
            var oldLanguageId = languageQuestionSet.LanguageId;
            var oldQuestionSetId = languageQuestionSet.QuestionSetId;
            
            languageQuestionSet.LanguageId = language2.Id;
            languageQuestionSet.QuestionSetId = questionSetForQuestion2.Id;
            languageQuestionSet.MicrotingUid = rnd.Next(1, 255);
            
            //Act
            await languageQuestionSet.Update(dbContext);
            
            List<language_question_sets> languageQuestionSets = dbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<language_question_set_versions> languageQuestionSetVersions =
                dbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);
            
            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(2, languageQuestionSetVersions.Count);
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSets[0].MicrotingUid);
            
            Assert.AreEqual(oldLanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(oldQuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(null, languageQuestionSetVersions[0].MicrotingUid);        
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[1].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[1].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSetVersions[1].MicrotingUid);
        }
        
        [Test]
        public async Task LanguageQuestionSet_Delete_DoesDelete()
        {
            //Assert
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);

            language_question_sets languageQuestionSet = new language_question_sets
            {
                LanguageId = language.Id,
                QuestionSetId = questionSetForQuestion.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            await languageQuestionSet.Create(dbContext);

            var oldLanguageId = languageQuestionSet.LanguageId;
            var oldQuestionSetId = languageQuestionSet.QuestionSetId;
            var oldMicrotingUid = languageQuestionSet.MicrotingUid;

            //Act
            await languageQuestionSet.Delete(dbContext);
            
            List<language_question_sets> languageQuestionSets = dbContext.LanguageQuestionSets.AsNoTracking().ToList();
            List<language_question_set_versions> languageQuestionSetVersions =
                dbContext.LanguageQuestionSetVersions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(languageQuestionSets);
            Assert.NotNull(languageQuestionSetVersions);
            
            Assert.AreEqual(1, languageQuestionSets.Count);
            Assert.AreEqual(2, languageQuestionSetVersions.Count);
            
            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSets[0].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSets[0].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSets[0].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Removed, languageQuestionSets[0].WorkflowState);
            
            Assert.AreEqual(oldLanguageId, languageQuestionSetVersions[0].LanguageId);
            Assert.AreEqual(oldQuestionSetId, languageQuestionSetVersions[0].QuestionSetId);
            Assert.AreEqual(oldMicrotingUid, languageQuestionSetVersions[0].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Created, languageQuestionSetVersions[0].WorkflowState);

            Assert.AreEqual(languageQuestionSet.LanguageId, languageQuestionSetVersions[1].LanguageId);
            Assert.AreEqual(languageQuestionSet.QuestionSetId, languageQuestionSetVersions[1].QuestionSetId);
            Assert.AreEqual(languageQuestionSet.MicrotingUid, languageQuestionSetVersions[1].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Removed, languageQuestionSetVersions[1].WorkflowState);
        }
    }
}