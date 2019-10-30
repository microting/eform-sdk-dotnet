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
    public class OptionsUTest : DbTestFixture
    {
        [Test]
        public async Task Options_Create_DoesCreate()
        {
            Random rnd = new Random();

            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSet = new question_sets();
            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PosiblyDeployed = randomBool;
            questionSet.Create(DbContext);
            
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
            question.Create(DbContext);
            
            options option = new options();
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            
            //Act
            
            option.Create(DbContext);
            
            List<options> options = DbContext.options.AsNoTracking().ToList();
            List<option_versions> optionVersions = DbContext.option_versions.AsNoTracking().ToList();
            
            Assert.NotNull(options);                                                             
            Assert.NotNull(optionVersions);                                                             

            Assert.AreEqual(1,options.Count());  
            Assert.AreEqual(1,optionVersions.Count());  
            
            Assert.AreEqual(option.CreatedAt.ToString(), options[0].CreatedAt.ToString());                                  
            Assert.AreEqual(option.Version, options[0].Version);                                      
            Assert.AreEqual(option.UpdatedAt.ToString(), options[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(options[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(option.Id, options[0].Id);
            Assert.AreEqual(option.Weight, options[0].Weight);
            Assert.AreEqual(option.OptionsIndex, options[0].OptionsIndex);
            Assert.AreEqual(option.WeightValue, options[0].WeightValue);
            Assert.AreEqual(option.QuestionId, question.Id);
            
            Assert.AreEqual(option.CreatedAt.ToString(), optionVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, optionVersions[0].Version);                                      
            Assert.AreEqual(option.UpdatedAt.ToString(), optionVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(optionVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(option.Id, optionVersions[0].OptionId);
            Assert.AreEqual(option.Weight, optionVersions[0].Weight);
            Assert.AreEqual(option.OptionsIndex, optionVersions[0].OptionsIndex);
            Assert.AreEqual(option.WeightValue, optionVersions[0].WeightValue);
            Assert.AreEqual(question.Id, optionVersions[0].QuestionId);
        }

        [Test]
        public async Task Options_Update_DoesUpdate()
        {
            Random rnd = new Random();
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSet = new question_sets();
            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PosiblyDeployed = randomBool;
            questionSet.Create(DbContext);
            
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
            question.Create(DbContext);
            
            options option = new options();
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            option.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = option.UpdatedAt;
            int oldWeight = option.Weight;
            int oldOptionsIndex = option.OptionsIndex;
            int oldWeightValue = option.WeightValue;
            
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.WeightValue = rnd.Next(1, 255);
            option.Update(DbContext);

            List<options> options = DbContext.options.AsNoTracking().ToList();
            List<option_versions> optionVersions = DbContext.option_versions.AsNoTracking().ToList();
            
            Assert.NotNull(options);                                                             
            Assert.NotNull(optionVersions);                                                             

            Assert.AreEqual(1,options.Count());  
            Assert.AreEqual(2,optionVersions.Count());  
            
            Assert.AreEqual(option.CreatedAt.ToString(), options[0].CreatedAt.ToString());                                  
            Assert.AreEqual(option.Version, options[0].Version);                                      
            Assert.AreEqual(option.UpdatedAt.ToString(), options[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(options[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(option.Id, options[0].Id);
            Assert.AreEqual(option.Weight, options[0].Weight);
            Assert.AreEqual(option.OptionsIndex, options[0].OptionsIndex);
            Assert.AreEqual(option.WeightValue, options[0].WeightValue);
            Assert.AreEqual(option.QuestionId, question.Id);
            
            //Old Version
            Assert.AreEqual(option.CreatedAt.ToString(), optionVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, optionVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), optionVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(optionVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(option.Id, optionVersions[0].OptionId);
            Assert.AreEqual(oldWeight, optionVersions[0].Weight);
            Assert.AreEqual(oldOptionsIndex, optionVersions[0].OptionsIndex);
            Assert.AreEqual(oldWeightValue, optionVersions[0].WeightValue);
            Assert.AreEqual(question.Id, optionVersions[0].QuestionId); 
            
            //New Version
            Assert.AreEqual(option.CreatedAt.ToString(), optionVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, optionVersions[1].Version);                                      
            Assert.AreEqual(option.UpdatedAt.ToString(), optionVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(optionVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(option.Id, optionVersions[1].OptionId);
            Assert.AreEqual(option.Weight, optionVersions[1].Weight);
            Assert.AreEqual(option.OptionsIndex, optionVersions[1].OptionsIndex);
            Assert.AreEqual(option.WeightValue, optionVersions[1].WeightValue);
            Assert.AreEqual(question.Id, optionVersions[1].QuestionId);
        }

        [Test]
        public async Task Options_Delete_DoesSetWorkflowStateToRemoved()
        {
            Random rnd = new Random();
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSet = new question_sets();
            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PosiblyDeployed = randomBool;
            questionSet.Create(DbContext);
            
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
            question.Create(DbContext);
            
            options option = new options();
            option.Weight = rnd.Next(1, 255);
            option.OptionsIndex = rnd.Next(1, 255);
            option.WeightValue = rnd.Next(1, 255);
            option.QuestionId = question.Id;
            option.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = option.UpdatedAt;
            
            option.Delete(DbContext);

            List<options> options = DbContext.options.AsNoTracking().ToList();
            List<option_versions> optionVersions = DbContext.option_versions.AsNoTracking().ToList();
            
            Assert.NotNull(options);                                                             
            Assert.NotNull(optionVersions);                                                             

            Assert.AreEqual(1,options.Count());  
            Assert.AreEqual(2,optionVersions.Count());  
            
            Assert.AreEqual(option.CreatedAt.ToString(), options[0].CreatedAt.ToString());                                  
            Assert.AreEqual(option.Version, options[0].Version);                                      
            Assert.AreEqual(option.UpdatedAt.ToString(), options[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(options[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(option.Id, options[0].Id);
            Assert.AreEqual(option.Weight, options[0].Weight);
            Assert.AreEqual(option.OptionsIndex, options[0].OptionsIndex);
            Assert.AreEqual(option.WeightValue, options[0].WeightValue);
            Assert.AreEqual(option.QuestionId, question.Id);
            
            //Old Version
            Assert.AreEqual(option.CreatedAt.ToString(), optionVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, optionVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), optionVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(optionVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(option.Id, optionVersions[0].OptionId);
            Assert.AreEqual(option.Weight, optionVersions[0].Weight);
            Assert.AreEqual(option.OptionsIndex, optionVersions[0].OptionsIndex);
            Assert.AreEqual(option.WeightValue, optionVersions[0].WeightValue);
            Assert.AreEqual(question.Id, optionVersions[0].QuestionId); 
            
            //New Version
            Assert.AreEqual(option.CreatedAt.ToString(), optionVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, optionVersions[1].Version);                                      
            Assert.AreEqual(option.UpdatedAt.ToString(), optionVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(optionVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(option.Id, optionVersions[1].OptionId);
            Assert.AreEqual(option.Weight, optionVersions[1].Weight);
            Assert.AreEqual(option.OptionsIndex, optionVersions[1].OptionsIndex);
            Assert.AreEqual(option.WeightValue, optionVersions[1].WeightValue);
            Assert.AreEqual(question.Id, optionVersions[1].QuestionId);
        }
    }
}