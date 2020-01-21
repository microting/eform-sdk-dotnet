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
    public class QuestionTranslationUTest : DbTestFixture
    {
        [Test]
        public async Task QuestionTranslation_Create_DoesCreate_W_MicrotingUID()
        {
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            questions question = new questions();
            question.Image = randomBool;
            question.Maximum = rnd.Next(1, 255);
            question.Minimum = rnd.Next(1, 255);
            question.Prioritised = randomBool;
            question.Type = Guid.NewGuid().ToString();
            question.FontSize = Guid.NewGuid().ToString();
            question.ImagePosition = Guid.NewGuid().ToString();
            question.MaxDuration = rnd.Next(1, 255);
            question.MinDuration = rnd.Next(1, 255);
            question.QuestionIndex = rnd.Next(1, 255);
            question.QuestionType = Guid.NewGuid().ToString();
            question.RefId = rnd.Next(1, 255);
            question.ValidDisplay = randomBool;
            question.BackButtonEnabled = randomBool;
            question.ContinuousQuestionId = rnd.Next(1, 255);
            question.QuestionSetId = questionSetForQuestion.Id;
            await question.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);

            question_translations questionTranslation = new question_translations
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            
            // Act
            await questionTranslation.Create(dbContext);

            List<question_translations> questionTranslations = dbContext.QuestionTranslations.AsNoTracking().ToList();
            List<question_translation_versions> questionTranslationVersions =
                dbContext.QuestionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);
            
            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(1, questionTranslationVersions.Count);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslations[0].MicrotingUid);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslationVersions[0].MicrotingUid);
        }
        
        [Test]
        public async Task QuestionTranslation_Create_DoesCreate_WO_MicrotingUID()
        {
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            questions question = new questions();
            question.Image = randomBool;
            question.Maximum = rnd.Next(1, 255);
            question.Minimum = rnd.Next(1, 255);
            question.Prioritised = randomBool;
            question.Type = Guid.NewGuid().ToString();
            question.FontSize = Guid.NewGuid().ToString();
            question.ImagePosition = Guid.NewGuid().ToString();
            question.MaxDuration = rnd.Next(1, 255);
            question.MinDuration = rnd.Next(1, 255);
            question.QuestionIndex = rnd.Next(1, 255);
            question.QuestionType = Guid.NewGuid().ToString();
            question.RefId = rnd.Next(1, 255);
            question.ValidDisplay = randomBool;
            question.BackButtonEnabled = randomBool;
            question.ContinuousQuestionId = rnd.Next(1, 255);
            question.QuestionSetId = questionSetForQuestion.Id;
            await question.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);

            question_translations questionTranslation = new question_translations
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id
            };
            
            // Act
            await questionTranslation.Create(dbContext);

            List<question_translations> questionTranslations = dbContext.QuestionTranslations.AsNoTracking().ToList();
            List<question_translation_versions> questionTranslationVersions =
                dbContext.QuestionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);
            
            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(1, questionTranslationVersions.Count);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(null, questionTranslations[0].MicrotingUid);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(null, questionTranslationVersions[0].MicrotingUid);
        }

        [Test]
        public async Task QuestionTranslation_Update_DoesUpdate_W_MicrotingUID()
        {
                        Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            question_sets questionSetForQuestion2 = new question_sets();
            questionSetForQuestion2.Name = Guid.NewGuid().ToString();
            questionSetForQuestion2.Share = randomBool;
            questionSetForQuestion2.HasChild = randomBool;
            questionSetForQuestion2.ParentId = rnd.Next(1, 255);
            questionSetForQuestion2.PosiblyDeployed = randomBool;
            await questionSetForQuestion2.Create(dbContext);
            
            questions question = new questions();
            question.Image = randomBool;
            question.Maximum = rnd.Next(1, 255);
            question.Minimum = rnd.Next(1, 255);
            question.Prioritised = randomBool;
            question.Type = Guid.NewGuid().ToString();
            question.FontSize = Guid.NewGuid().ToString();
            question.ImagePosition = Guid.NewGuid().ToString();
            question.MaxDuration = rnd.Next(1, 255);
            question.MinDuration = rnd.Next(1, 255);
            question.QuestionIndex = rnd.Next(1, 255);
            question.QuestionType = Guid.NewGuid().ToString();
            question.RefId = rnd.Next(1, 255);
            question.ValidDisplay = randomBool;
            question.BackButtonEnabled = randomBool;
            question.ContinuousQuestionId = rnd.Next(1, 255);
            question.QuestionSetId = questionSetForQuestion.Id;
            await question.Create(dbContext);
            
            questions question2 = new questions();
            question2.Image = randomBool;
            question2.Maximum = rnd.Next(1, 255);
            question2.Minimum = rnd.Next(1, 255);
            question2.Prioritised = randomBool;
            question2.Type = Guid.NewGuid().ToString();
            question2.FontSize = Guid.NewGuid().ToString();
            question2.ImagePosition = Guid.NewGuid().ToString();
            question2.MaxDuration = rnd.Next(1, 255);
            question2.MinDuration = rnd.Next(1, 255);
            question2.QuestionIndex = rnd.Next(1, 255);
            question2.QuestionType = Guid.NewGuid().ToString();
            question2.RefId = rnd.Next(1, 255);
            question2.ValidDisplay = randomBool;
            question2.BackButtonEnabled = randomBool;
            question2.ContinuousQuestionId = rnd.Next(1, 255);
            question2.QuestionSetId = questionSetForQuestion2.Id;
            await question2.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            languages language2 = new languages();
            language2.Description = Guid.NewGuid().ToString();
            language2.Name = Guid.NewGuid().ToString();
            await language2.Create(dbContext);

            question_translations questionTranslation = new question_translations
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            await questionTranslation.Create(dbContext);

            var oldLanguageId = questionTranslation.LanguageId;
            var oldName = questionTranslation.Name;
            var oldQuestionId = questionTranslation.QuestionId;
            var oldMicrotingUid = questionTranslation.MicrotingUid;

            questionTranslation.LanguageId = language2.Id;
            questionTranslation.Name = Guid.NewGuid().ToString();
            questionTranslation.QuestionId = question2.Id;
            questionTranslation.MicrotingUid = rnd.Next(1, 255);
            // Act
            await questionTranslation.Update(dbContext);
            
            List<question_translations> questionTranslations = dbContext.QuestionTranslations.AsNoTracking().ToList();
            List<question_translation_versions> questionTranslationVersions =
                dbContext.QuestionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);
            
            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(2, questionTranslationVersions.Count);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslations[0].MicrotingUid);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[1].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[1].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[1].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslationVersions[1].MicrotingUid);
            
            Assert.AreEqual(oldLanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldName, questionTranslationVersions[0].Name);
            Assert.AreEqual(oldQuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(oldMicrotingUid, questionTranslationVersions[0].MicrotingUid);
        }
        
        [Test]
        public async Task QuestionTranslation_Update_DoesUpdate_WO_MicrotingUID()
        {
                        Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            question_sets questionSetForQuestion2 = new question_sets();
            questionSetForQuestion2.Name = Guid.NewGuid().ToString();
            questionSetForQuestion2.Share = randomBool;
            questionSetForQuestion2.HasChild = randomBool;
            questionSetForQuestion2.ParentId = rnd.Next(1, 255);
            questionSetForQuestion2.PosiblyDeployed = randomBool;
            await questionSetForQuestion2.Create(dbContext);
            
            questions question = new questions();
            question.Image = randomBool;
            question.Maximum = rnd.Next(1, 255);
            question.Minimum = rnd.Next(1, 255);
            question.Prioritised = randomBool;
            question.Type = Guid.NewGuid().ToString();
            question.FontSize = Guid.NewGuid().ToString();
            question.ImagePosition = Guid.NewGuid().ToString();
            question.MaxDuration = rnd.Next(1, 255);
            question.MinDuration = rnd.Next(1, 255);
            question.QuestionIndex = rnd.Next(1, 255);
            question.QuestionType = Guid.NewGuid().ToString();
            question.RefId = rnd.Next(1, 255);
            question.ValidDisplay = randomBool;
            question.BackButtonEnabled = randomBool;
            question.ContinuousQuestionId = rnd.Next(1, 255);
            question.QuestionSetId = questionSetForQuestion.Id;
            await question.Create(dbContext);
            
            questions question2 = new questions();
            question2.Image = randomBool;
            question2.Maximum = rnd.Next(1, 255);
            question2.Minimum = rnd.Next(1, 255);
            question2.Prioritised = randomBool;
            question2.Type = Guid.NewGuid().ToString();
            question2.FontSize = Guid.NewGuid().ToString();
            question2.ImagePosition = Guid.NewGuid().ToString();
            question2.MaxDuration = rnd.Next(1, 255);
            question2.MinDuration = rnd.Next(1, 255);
            question2.QuestionIndex = rnd.Next(1, 255);
            question2.QuestionType = Guid.NewGuid().ToString();
            question2.RefId = rnd.Next(1, 255);
            question2.ValidDisplay = randomBool;
            question2.BackButtonEnabled = randomBool;
            question2.ContinuousQuestionId = rnd.Next(1, 255);
            question2.QuestionSetId = questionSetForQuestion2.Id;
            await question2.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            languages language2 = new languages();
            language2.Description = Guid.NewGuid().ToString();
            language2.Name = Guid.NewGuid().ToString();
            await language2.Create(dbContext);

            question_translations questionTranslation = new question_translations
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id
            };
            await questionTranslation.Create(dbContext);

            var oldLanguageId = questionTranslation.LanguageId;
            var oldName = questionTranslation.Name;
            var oldQuestionId = questionTranslation.QuestionId;

            questionTranslation.LanguageId = language2.Id;
            questionTranslation.Name = Guid.NewGuid().ToString();
            questionTranslation.QuestionId = question2.Id;
            // Act
            await questionTranslation.Update(dbContext);
            
            List<question_translations> questionTranslations = dbContext.QuestionTranslations.AsNoTracking().ToList();
            List<question_translation_versions> questionTranslationVersions =
                dbContext.QuestionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);
            
            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(2, questionTranslationVersions.Count);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslations[0].MicrotingUid);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[1].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[1].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[1].QuestionId);
            Assert.AreEqual(null, questionTranslationVersions[1].MicrotingUid);
            
            Assert.AreEqual(oldLanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldName, questionTranslationVersions[0].Name);
            Assert.AreEqual(oldQuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(null, questionTranslationVersions[0].MicrotingUid);
        }
        
        [Test]
        public async Task QuestionTranslation_Update_DoesUpdate_W_MicrotingUID_RemovesUid()
        {
                        Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            question_sets questionSetForQuestion2 = new question_sets();
            questionSetForQuestion2.Name = Guid.NewGuid().ToString();
            questionSetForQuestion2.Share = randomBool;
            questionSetForQuestion2.HasChild = randomBool;
            questionSetForQuestion2.ParentId = rnd.Next(1, 255);
            questionSetForQuestion2.PosiblyDeployed = randomBool;
            await questionSetForQuestion2.Create(dbContext);
            
            questions question = new questions();
            question.Image = randomBool;
            question.Maximum = rnd.Next(1, 255);
            question.Minimum = rnd.Next(1, 255);
            question.Prioritised = randomBool;
            question.Type = Guid.NewGuid().ToString();
            question.FontSize = Guid.NewGuid().ToString();
            question.ImagePosition = Guid.NewGuid().ToString();
            question.MaxDuration = rnd.Next(1, 255);
            question.MinDuration = rnd.Next(1, 255);
            question.QuestionIndex = rnd.Next(1, 255);
            question.QuestionType = Guid.NewGuid().ToString();
            question.RefId = rnd.Next(1, 255);
            question.ValidDisplay = randomBool;
            question.BackButtonEnabled = randomBool;
            question.ContinuousQuestionId = rnd.Next(1, 255);
            question.QuestionSetId = questionSetForQuestion.Id;
            await question.Create(dbContext);
            
            questions question2 = new questions();
            question2.Image = randomBool;
            question2.Maximum = rnd.Next(1, 255);
            question2.Minimum = rnd.Next(1, 255);
            question2.Prioritised = randomBool;
            question2.Type = Guid.NewGuid().ToString();
            question2.FontSize = Guid.NewGuid().ToString();
            question2.ImagePosition = Guid.NewGuid().ToString();
            question2.MaxDuration = rnd.Next(1, 255);
            question2.MinDuration = rnd.Next(1, 255);
            question2.QuestionIndex = rnd.Next(1, 255);
            question2.QuestionType = Guid.NewGuid().ToString();
            question2.RefId = rnd.Next(1, 255);
            question2.ValidDisplay = randomBool;
            question2.BackButtonEnabled = randomBool;
            question2.ContinuousQuestionId = rnd.Next(1, 255);
            question2.QuestionSetId = questionSetForQuestion2.Id;
            await question2.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            languages language2 = new languages();
            language2.Description = Guid.NewGuid().ToString();
            language2.Name = Guid.NewGuid().ToString();
            await language2.Create(dbContext);

            question_translations questionTranslation = new question_translations
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            await questionTranslation.Create(dbContext);

            var oldLanguageId = questionTranslation.LanguageId;
            var oldName = questionTranslation.Name;
            var oldQuestionId = questionTranslation.QuestionId;
            var oldMicrotingUid = questionTranslation.MicrotingUid;

            questionTranslation.LanguageId = language2.Id;
            questionTranslation.Name = Guid.NewGuid().ToString();
            questionTranslation.QuestionId = question2.Id;
            questionTranslation.MicrotingUid = null;
            // Act
            await questionTranslation.Update(dbContext);
            
            List<question_translations> questionTranslations = dbContext.QuestionTranslations.AsNoTracking().ToList();
            List<question_translation_versions> questionTranslationVersions =
                dbContext.QuestionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);
            
            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(2, questionTranslationVersions.Count);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslations[0].MicrotingUid);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[1].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[1].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[1].QuestionId);
            Assert.AreEqual(null, questionTranslationVersions[1].MicrotingUid);
            
            Assert.AreEqual(oldLanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldName, questionTranslationVersions[0].Name);
            Assert.AreEqual(oldQuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(oldMicrotingUid, questionTranslationVersions[0].MicrotingUid);
        }
        
        [Test]
        public async Task QuestionTranslation_Update_DoesUpdate_WO_MicrotingUID_AddsUID()
        {
                        Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);
            
            question_sets questionSetForQuestion2 = new question_sets();
            questionSetForQuestion2.Name = Guid.NewGuid().ToString();
            questionSetForQuestion2.Share = randomBool;
            questionSetForQuestion2.HasChild = randomBool;
            questionSetForQuestion2.ParentId = rnd.Next(1, 255);
            questionSetForQuestion2.PosiblyDeployed = randomBool;
            await questionSetForQuestion2.Create(dbContext);
            
            questions question = new questions();
            question.Image = randomBool;
            question.Maximum = rnd.Next(1, 255);
            question.Minimum = rnd.Next(1, 255);
            question.Prioritised = randomBool;
            question.Type = Guid.NewGuid().ToString();
            question.FontSize = Guid.NewGuid().ToString();
            question.ImagePosition = Guid.NewGuid().ToString();
            question.MaxDuration = rnd.Next(1, 255);
            question.MinDuration = rnd.Next(1, 255);
            question.QuestionIndex = rnd.Next(1, 255);
            question.QuestionType = Guid.NewGuid().ToString();
            question.RefId = rnd.Next(1, 255);
            question.ValidDisplay = randomBool;
            question.BackButtonEnabled = randomBool;
            question.ContinuousQuestionId = rnd.Next(1, 255);
            question.QuestionSetId = questionSetForQuestion.Id;
            await question.Create(dbContext);
            
            questions question2 = new questions();
            question2.Image = randomBool;
            question2.Maximum = rnd.Next(1, 255);
            question2.Minimum = rnd.Next(1, 255);
            question2.Prioritised = randomBool;
            question2.Type = Guid.NewGuid().ToString();
            question2.FontSize = Guid.NewGuid().ToString();
            question2.ImagePosition = Guid.NewGuid().ToString();
            question2.MaxDuration = rnd.Next(1, 255);
            question2.MinDuration = rnd.Next(1, 255);
            question2.QuestionIndex = rnd.Next(1, 255);
            question2.QuestionType = Guid.NewGuid().ToString();
            question2.RefId = rnd.Next(1, 255);
            question2.ValidDisplay = randomBool;
            question2.BackButtonEnabled = randomBool;
            question2.ContinuousQuestionId = rnd.Next(1, 255);
            question2.QuestionSetId = questionSetForQuestion2.Id;
            await question2.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            languages language2 = new languages();
            language2.Description = Guid.NewGuid().ToString();
            language2.Name = Guid.NewGuid().ToString();
            await language2.Create(dbContext);

            question_translations questionTranslation = new question_translations
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
            };
            await questionTranslation.Create(dbContext);

            var oldLanguageId = questionTranslation.LanguageId;
            var oldName = questionTranslation.Name;
            var oldQuestionId = questionTranslation.QuestionId;

            questionTranslation.LanguageId = language2.Id;
            questionTranslation.Name = Guid.NewGuid().ToString();
            questionTranslation.QuestionId = question2.Id;
            questionTranslation.MicrotingUid = rnd.Next(1, 255);
            // Act
            await questionTranslation.Update(dbContext);
            
            List<question_translations> questionTranslations = dbContext.QuestionTranslations.AsNoTracking().ToList();
            List<question_translation_versions> questionTranslationVersions =
                dbContext.QuestionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);
            
            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(2, questionTranslationVersions.Count);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslations[0].MicrotingUid);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[1].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[1].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[1].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslationVersions[1].MicrotingUid);
            
            Assert.AreEqual(oldLanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldName, questionTranslationVersions[0].Name);
            Assert.AreEqual(oldQuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(null, questionTranslationVersions[0].MicrotingUid);
        }

        [Test]
        public async Task QuestionTranslation_Delete_DoesDelete()
        {
                                    Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSetForQuestion = new question_sets();
            questionSetForQuestion.Name = Guid.NewGuid().ToString();
            questionSetForQuestion.Share = randomBool;
            questionSetForQuestion.HasChild = randomBool;
            questionSetForQuestion.ParentId = rnd.Next(1, 255);
            questionSetForQuestion.PosiblyDeployed = randomBool;
            await questionSetForQuestion.Create(dbContext);

            questions question = new questions();
            question.Image = randomBool;
            question.Maximum = rnd.Next(1, 255);
            question.Minimum = rnd.Next(1, 255);
            question.Prioritised = randomBool;
            question.Type = Guid.NewGuid().ToString();
            question.FontSize = Guid.NewGuid().ToString();
            question.ImagePosition = Guid.NewGuid().ToString();
            question.MaxDuration = rnd.Next(1, 255);
            question.MinDuration = rnd.Next(1, 255);
            question.QuestionIndex = rnd.Next(1, 255);
            question.QuestionType = Guid.NewGuid().ToString();
            question.RefId = rnd.Next(1, 255);
            question.ValidDisplay = randomBool;
            question.BackButtonEnabled = randomBool;
            question.ContinuousQuestionId = rnd.Next(1, 255);
            question.QuestionSetId = questionSetForQuestion.Id;
            await question.Create(dbContext);
            
            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            question_translations questionTranslation = new question_translations
            {
                LanguageId = language.Id,
                Name = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
                MicrotingUid = rnd.Next(1, 255)
            };
            await questionTranslation.Create(dbContext);

            var oldLanguageId = questionTranslation.LanguageId;
            var oldName = questionTranslation.Name;
            var oldQuestionId = questionTranslation.QuestionId;
            var oldMicrotingUid = questionTranslation.MicrotingUid;

            // Act
            await questionTranslation.Delete(dbContext);
            
            List<question_translations> questionTranslations = dbContext.QuestionTranslations.AsNoTracking().ToList();
            List<question_translation_versions> questionTranslationVersions =
                dbContext.QuestionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(questionTranslations);
            Assert.NotNull(questionTranslationVersions);
            
            Assert.AreEqual(1, questionTranslations.Count);
            Assert.AreEqual(2, questionTranslationVersions.Count);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslations[0].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslations[0].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslations[0].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslations[0].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Removed, questionTranslations[0].WorkflowState);
            
            Assert.AreEqual(questionTranslation.LanguageId, questionTranslationVersions[1].LanguageId);
            Assert.AreEqual(questionTranslation.Name, questionTranslationVersions[1].Name);
            Assert.AreEqual(questionTranslation.QuestionId, questionTranslationVersions[1].QuestionId);
            Assert.AreEqual(questionTranslation.MicrotingUid, questionTranslationVersions[1].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Removed, questionTranslationVersions[1].WorkflowState);

            Assert.AreEqual(oldLanguageId, questionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldName, questionTranslationVersions[0].Name);
            Assert.AreEqual(oldQuestionId, questionTranslationVersions[0].QuestionId);
            Assert.AreEqual(oldMicrotingUid, questionTranslationVersions[0].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Created, questionTranslationVersions[0].WorkflowState);
        }
        
    }
}