using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class OptionTranslationUTest : DbTestFixture
    {
        [Test]
        public async Task OptionTranslation_Create_DoesCreate_W_MicrotingUid()
        {
            //Arrange
            
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSet = new question_sets();
            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PosiblyDeployed = randomBool;
            await questionSet.Create(dbContext);
            
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
            question.QuestionSetId = questionSet.Id;
            await question.Create(dbContext);
            
            options option = new options();
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            await option.Create(dbContext);

            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);

            option_translations optionTranslation = new option_translations
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            // Act

            await optionTranslation.Create(dbContext);

            List<option_translations> optionTranslations = dbContext.OptionTranslations.AsNoTracking().ToList();
            List<option_translation_versions> optionTranslationVersions =
                dbContext.OptionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);
            
            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(1, optionTranslationVersions.Count);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslations[0].MicrotingUid);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslationVersions[0].MicrotingUid);
        }
            
        [Test]
        public async Task OptionTranslation_Create_DoesCreate_WO_MicrotingUid()
        {
            //Arrange
            
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSet = new question_sets();
            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PosiblyDeployed = randomBool;
            await questionSet.Create(dbContext);
            
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
            question.QuestionSetId = questionSet.Id;
            await question.Create(dbContext);
            
            options option = new options();
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            await option.Create(dbContext);

            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);

            option_translations optionTranslation = new option_translations
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString(),
            };
            // Act

            await optionTranslation.Create(dbContext);

            List<option_translations> optionTranslations = dbContext.OptionTranslations.AsNoTracking().ToList();
            List<option_translation_versions> optionTranslationVersions =
                dbContext.OptionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);
            
            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(1, optionTranslationVersions.Count);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(null, optionTranslations[0].MicrotingUid);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(null, optionTranslationVersions[0].MicrotingUid);
        }
        
        [Test]
        public async Task OptionTranslation_Update_DoesUpdate_W_MicrotingUid()
        {
            //Arrange
            
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSet = new question_sets();
            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PosiblyDeployed = randomBool;
            await questionSet.Create(dbContext);
            
            question_sets questionSet2 = new question_sets();
            questionSet2.Name = Guid.NewGuid().ToString();
            questionSet2.Share = randomBool;
            questionSet2.HasChild = randomBool;
            questionSet2.PosiblyDeployed = randomBool;
            await questionSet2.Create(dbContext);
            
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
            question.QuestionSetId = questionSet.Id;
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
            question2.QuestionSetId = questionSet.Id;
            await question2.Create(dbContext);
            
            options option = new options();
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            await option.Create(dbContext);
            
            options option2 = new options();
            option2.Weight = rnd.Next(1, 255);
            option2.OptionsIndex = rnd.Next(1, 255);
            option2.WeightValue = rnd.Next(1, 255);
            option2.QuestionId = question.Id;
            await option2.Create(dbContext);

            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            languages language2 = new languages();
            language2.Description = Guid.NewGuid().ToString();
            language2.Name = Guid.NewGuid().ToString();
            await language2.Create(dbContext);

            option_translations optionTranslation = new option_translations
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await optionTranslation.Create(dbContext);

            var oldLanguageId = optionTranslation.LanguageId;
            var oldOptionId = optionTranslation.OptionId;
            var oldName = optionTranslation.Name;
            var oldMicrotingUid = optionTranslation.MicrotingUid;

            optionTranslation.Name = Guid.NewGuid().ToString();
            optionTranslation.LanguageId = language2.Id;
            optionTranslation.MicrotingUid = rnd.Next(1, 255);
            optionTranslation.OptionId = option2.Id;
            
            // Act
            await optionTranslation.Update(dbContext);

            List<option_translations> optionTranslations = dbContext.OptionTranslations.AsNoTracking().ToList();
            List<option_translation_versions> optionTranslationVersions =
                dbContext.OptionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);
            
            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(2, optionTranslationVersions.Count);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslations[0].MicrotingUid);
            
            Assert.AreEqual(oldName, optionTranslationVersions[0].Name);
            Assert.AreEqual(oldOptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(oldLanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldMicrotingUid, optionTranslationVersions[0].MicrotingUid);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[1].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[1].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[1].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslationVersions[1].MicrotingUid);
        }
            
        [Test]
        public async Task OptionTranslation_Update_DoesUpdate_WO_MicrotingUid()
        {
            //Arrange
            
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSet = new question_sets();
            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PosiblyDeployed = randomBool;
            await questionSet.Create(dbContext);
            
            question_sets questionSet2 = new question_sets();
            questionSet2.Name = Guid.NewGuid().ToString();
            questionSet2.Share = randomBool;
            questionSet2.HasChild = randomBool;
            questionSet2.PosiblyDeployed = randomBool;
            await questionSet2.Create(dbContext);
            
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
            question.QuestionSetId = questionSet.Id;
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
            question2.QuestionSetId = questionSet.Id;
            await question2.Create(dbContext);
            
            options option = new options();
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            await option.Create(dbContext);
            
            options option2 = new options();
            option2.Weight = rnd.Next(1, 255);
            option2.OptionsIndex = rnd.Next(1, 255);
            option2.WeightValue = rnd.Next(1, 255);
            option2.QuestionId = question.Id;
            await option2.Create(dbContext);

            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            languages language2 = new languages();
            language2.Description = Guid.NewGuid().ToString();
            language2.Name = Guid.NewGuid().ToString();
            await language2.Create(dbContext);

            option_translations optionTranslation = new option_translations
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString()
            };
            await optionTranslation.Create(dbContext);

            var oldLanguageId = optionTranslation.LanguageId;
            var oldOptionId = optionTranslation.OptionId;
            var oldName = optionTranslation.Name;

            optionTranslation.Name = Guid.NewGuid().ToString();
            optionTranslation.LanguageId = language2.Id;
            optionTranslation.OptionId = option2.Id;
            
            // Act
            await optionTranslation.Update(dbContext);

            List<option_translations> optionTranslations = dbContext.OptionTranslations.AsNoTracking().ToList();
            List<option_translation_versions> optionTranslationVersions =
                dbContext.OptionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);
            
            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(2, optionTranslationVersions.Count);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslations[0].MicrotingUid);
            
            Assert.AreEqual(oldName, optionTranslationVersions[0].Name);
            Assert.AreEqual(oldOptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(oldLanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(null, optionTranslationVersions[0].MicrotingUid);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[1].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[1].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[1].LanguageId);
            Assert.AreEqual(null, optionTranslationVersions[1].MicrotingUid);
        }
        
        [Test]
        public async Task OptionTranslation_Update_DoesUpdate_W_MicrotingUid_RemovesUid()
        {
            //Arrange
            
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSet = new question_sets();
            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PosiblyDeployed = randomBool;
            await questionSet.Create(dbContext);
            
            question_sets questionSet2 = new question_sets();
            questionSet2.Name = Guid.NewGuid().ToString();
            questionSet2.Share = randomBool;
            questionSet2.HasChild = randomBool;
            questionSet2.PosiblyDeployed = randomBool;
            await questionSet2.Create(dbContext);
            
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
            question.QuestionSetId = questionSet.Id;
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
            question2.QuestionSetId = questionSet.Id;
            await question2.Create(dbContext);
            
            options option = new options();
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            await option.Create(dbContext);
            
            options option2 = new options();
            option2.Weight = rnd.Next(1, 255);
            option2.OptionsIndex = rnd.Next(1, 255);
            option2.WeightValue = rnd.Next(1, 255);
            option2.QuestionId = question.Id;
            await option2.Create(dbContext);

            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            languages language2 = new languages();
            language2.Description = Guid.NewGuid().ToString();
            language2.Name = Guid.NewGuid().ToString();
            await language2.Create(dbContext);

            option_translations optionTranslation = new option_translations
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await optionTranslation.Create(dbContext);

            var oldLanguageId = optionTranslation.LanguageId;
            var oldOptionId = optionTranslation.OptionId;
            var oldName = optionTranslation.Name;
            var oldMicrotingUid = optionTranslation.MicrotingUid;

            optionTranslation.Name = Guid.NewGuid().ToString();
            optionTranslation.LanguageId = language2.Id;
            optionTranslation.MicrotingUid = null;
            optionTranslation.OptionId = option2.Id;
            
            // Act
            await optionTranslation.Update(dbContext);

            List<option_translations> optionTranslations = dbContext.OptionTranslations.AsNoTracking().ToList();
            List<option_translation_versions> optionTranslationVersions =
                dbContext.OptionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);
            
            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(2, optionTranslationVersions.Count);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(null, optionTranslations[0].MicrotingUid);
            
            Assert.AreEqual(oldName, optionTranslationVersions[0].Name);
            Assert.AreEqual(oldOptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(oldLanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldMicrotingUid, optionTranslationVersions[0].MicrotingUid);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[1].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[1].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[1].LanguageId);
            Assert.AreEqual(null, optionTranslationVersions[1].MicrotingUid);
        }
            
        [Test]
        public async Task OptionTranslation_Update_DoesUpdate_WO_MicrotingUid_AddsUid()
        {
            //Arrange
            
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSet = new question_sets();
            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PosiblyDeployed = randomBool;
            await questionSet.Create(dbContext);
            
            question_sets questionSet2 = new question_sets();
            questionSet2.Name = Guid.NewGuid().ToString();
            questionSet2.Share = randomBool;
            questionSet2.HasChild = randomBool;
            questionSet2.PosiblyDeployed = randomBool;
            await questionSet2.Create(dbContext);
            
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
            question.QuestionSetId = questionSet.Id;
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
            question2.QuestionSetId = questionSet.Id;
            await question2.Create(dbContext);
            
            options option = new options();
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            await option.Create(dbContext);
            
            options option2 = new options();
            option2.Weight = rnd.Next(1, 255);
            option2.OptionsIndex = rnd.Next(1, 255);
            option2.WeightValue = rnd.Next(1, 255);
            option2.QuestionId = question.Id;
            await option2.Create(dbContext);

            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
            
            languages language2 = new languages();
            language2.Description = Guid.NewGuid().ToString();
            language2.Name = Guid.NewGuid().ToString();
            await language2.Create(dbContext);

            option_translations optionTranslation = new option_translations
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString()
            };
            await optionTranslation.Create(dbContext);

            var oldLanguageId = optionTranslation.LanguageId;
            var oldOptionId = optionTranslation.OptionId;
            var oldName = optionTranslation.Name;

            optionTranslation.Name = Guid.NewGuid().ToString();
            optionTranslation.LanguageId = language2.Id;
            optionTranslation.OptionId = option2.Id;
            optionTranslation.MicrotingUid = rnd.Next(1, 255);
            
            // Act
            await optionTranslation.Update(dbContext);

            List<option_translations> optionTranslations = dbContext.OptionTranslations.AsNoTracking().ToList();
            List<option_translation_versions> optionTranslationVersions =
                dbContext.OptionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);
            
            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(2, optionTranslationVersions.Count);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslations[0].MicrotingUid);
            
            Assert.AreEqual(oldName, optionTranslationVersions[0].Name);
            Assert.AreEqual(oldOptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(oldLanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(null, optionTranslationVersions[0].MicrotingUid);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[1].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[1].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[1].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslationVersions[1].MicrotingUid);
        }
                
        [Test]
        public async Task OptionTranslation_Delete_DoesDelete()
        {
            //Arrange
            
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSet = new question_sets();
            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PosiblyDeployed = randomBool;
            await questionSet.Create(dbContext);
    
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
            question.QuestionSetId = questionSet.Id;
            await question.Create(dbContext);
   
            
            options option = new options();
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            await option.Create(dbContext);
        

            languages language = new languages();
            language.Description = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            await language.Create(dbContext);
       

            option_translations optionTranslation = new option_translations
            {
                LanguageId = language.Id,
                OptionId = option.Id,
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await optionTranslation.Create(dbContext);

            var oldLanguageId = optionTranslation.LanguageId;
            var oldOptionId = optionTranslation.OptionId;
            var oldName = optionTranslation.Name;
            var oldMicrotingUid = optionTranslation.MicrotingUid;

            // Act
            await optionTranslation.Delete(dbContext);

            List<option_translations> optionTranslations = dbContext.OptionTranslations.AsNoTracking().ToList();
            List<option_translation_versions> optionTranslationVersions =
                dbContext.OptionTranslationVersions.AsNoTracking().ToList();
            
            // Assert
            
            Assert.NotNull(optionTranslations);
            Assert.NotNull(optionTranslationVersions);
            
            Assert.AreEqual(1, optionTranslations.Count);
            Assert.AreEqual(2, optionTranslationVersions.Count);
            
            Assert.AreEqual(optionTranslation.Name, optionTranslations[0].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslations[0].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslations[0].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslations[0].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Removed, optionTranslations[0].WorkflowState);
            
            
            Assert.AreEqual(oldName, optionTranslationVersions[0].Name);
            Assert.AreEqual(oldOptionId, optionTranslationVersions[0].OptionId);
            Assert.AreEqual(oldLanguageId, optionTranslationVersions[0].LanguageId);
            Assert.AreEqual(oldMicrotingUid, optionTranslationVersions[0].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Created, optionTranslationVersions[0].WorkflowState);

            Assert.AreEqual(optionTranslation.Name, optionTranslationVersions[1].Name);
            Assert.AreEqual(optionTranslation.OptionId, optionTranslationVersions[1].OptionId);
            Assert.AreEqual(optionTranslation.LanguageId, optionTranslationVersions[1].LanguageId);
            Assert.AreEqual(optionTranslation.MicrotingUid, optionTranslationVersions[1].MicrotingUid);
            Assert.AreEqual(Constants.WorkflowStates.Removed, optionTranslationVersions[1].WorkflowState);

        }
        
    }
}