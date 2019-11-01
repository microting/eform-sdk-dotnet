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
    public class QuestionSetsUTest : DbTestFixture
    {
        [Test]
        public async Task QuestionSets_Create_DoesCreate()
        {
            //Arrange
            
            Random rnd = new Random();
            bool randomBool = rnd.Next(0, 2) > 0;
            
            question_sets questionSet = new question_sets();
            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PosiblyDeployed = randomBool;
            
            //Act
            
            await questionSet.Create(dbContext);
            
            List<question_sets> questionSets = dbContext.question_sets.AsNoTracking().ToList();
            List<question_set_versions> questionSetVersions = dbContext.question_set_versions.AsNoTracking().ToList();
            
            Assert.NotNull(questionSets);                                                             
            Assert.NotNull(questionSetVersions);                                                             

            Assert.AreEqual(1,questionSets.Count());  
            Assert.AreEqual(1,questionSetVersions.Count());  
            
            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSets[0].CreatedAt.ToString());                                  
            Assert.AreEqual(questionSet.Version, questionSets[0].Version);                                      
//            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSets[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(questionSets[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(questionSet.Id, questionSets[0].Id);
            Assert.AreEqual(questionSet.Name, questionSets[0].Name);
            Assert.AreEqual(questionSet.Share, questionSets[0].Share);
            Assert.AreEqual(questionSet.HasChild, questionSets[0].HasChild);
            Assert.AreEqual(questionSet.PosiblyDeployed, questionSets[0].PosiblyDeployed);
            
            //Versions
            
            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSetVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(questionSet.Version, questionSetVersions[0].Version);                                      
//            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSetVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(questionSetVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(questionSet.Id, questionSetVersions[0].Id);
            Assert.AreEqual(questionSet.Name, questionSetVersions[0].Name);
            Assert.AreEqual(questionSet.Share, questionSetVersions[0].Share);
            Assert.AreEqual(questionSet.HasChild, questionSetVersions[0].HasChild);
            Assert.AreEqual(questionSet.PosiblyDeployed, questionSetVersions[0].PossiblyDeployed);
        }

        [Test]
        public async Task QuestionSets_Update_DoesUpdate()
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
            
            //Act

            DateTime? oldUpdatedAt = questionSet.UpdatedAt;
            string oldName = questionSet.Name;
            bool oldShare = questionSet.Share;
            bool oldHasChild = questionSet.HasChild;
            bool oldPossiblyDeployed = questionSet.PosiblyDeployed;
            
            questionSet.Name = Guid.NewGuid().ToString();
            questionSet.Share = randomBool;
            questionSet.HasChild = randomBool;
            questionSet.PosiblyDeployed = randomBool;
            
            await questionSet.Update(dbContext);

            List<question_sets> questionSets = dbContext.question_sets.AsNoTracking().ToList();
            List<question_set_versions> questionSetVersions = dbContext.question_set_versions.AsNoTracking().ToList();
            
            Assert.NotNull(questionSets);                                                             
            Assert.NotNull(questionSetVersions);                                                             

            Assert.AreEqual(1,questionSets.Count());  
            Assert.AreEqual(2,questionSetVersions.Count());  
            
            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSets[0].CreatedAt.ToString());                                  
            Assert.AreEqual(questionSet.Version, questionSets[0].Version);                                      
//            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSets[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(questionSets[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(questionSet.Id, questionSets[0].Id);
            Assert.AreEqual(questionSet.Name, questionSets[0].Name);
            Assert.AreEqual(questionSet.Share, questionSets[0].Share);
            Assert.AreEqual(questionSet.HasChild, questionSets[0].HasChild);
            Assert.AreEqual(questionSet.PosiblyDeployed, questionSets[0].PosiblyDeployed);
            
            //Old Version
            
            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSetVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, questionSetVersions[0].Version);                                      
//            Assert.AreEqual(oldUpdatedAt.ToString(), questionSetVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(questionSetVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(questionSet.Id, questionSetVersions[0].QuestionSetId);
            Assert.AreEqual(oldName, questionSetVersions[0].Name);
            Assert.AreEqual(oldShare, questionSetVersions[0].Share);
            Assert.AreEqual(oldHasChild, questionSetVersions[0].HasChild);
            Assert.AreEqual(oldPossiblyDeployed, questionSetVersions[0].PossiblyDeployed);
            
            //New Version
            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSetVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, questionSetVersions[1].Version);                                      
//            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSetVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(questionSetVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(questionSet.Id, questionSetVersions[1].QuestionSetId);
            Assert.AreEqual(questionSet.Name, questionSetVersions[1].Name);
            Assert.AreEqual(questionSet.Share, questionSetVersions[1].Share);
            Assert.AreEqual(questionSet.HasChild, questionSetVersions[1].HasChild);
            Assert.AreEqual(questionSet.PosiblyDeployed, questionSetVersions[1].PossiblyDeployed);
        }

        [Test]
        public async Task QuestionSets_Delete_DoesSetWorkflowStateToRemoved()
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
            
            //Act

            DateTime? oldUpdatedAt = questionSet.UpdatedAt;
            
            await questionSet.Delete(dbContext);

            List<question_sets> questionSets = dbContext.question_sets.AsNoTracking().ToList();
            List<question_set_versions> questionSetVersions = dbContext.question_set_versions.AsNoTracking().ToList();
            
            Assert.NotNull(questionSets);                                                             
            Assert.NotNull(questionSetVersions);                                                             

            Assert.AreEqual(1,questionSets.Count());  
            Assert.AreEqual(2,questionSetVersions.Count());  
            
            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSets[0].CreatedAt.ToString());                                  
            Assert.AreEqual(questionSet.Version, questionSets[0].Version);                                      
//            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSets[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(questionSets[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(questionSet.Id, questionSets[0].Id);
            Assert.AreEqual(questionSet.Name, questionSets[0].Name);
            Assert.AreEqual(questionSet.Share, questionSets[0].Share);
            Assert.AreEqual(questionSet.HasChild, questionSets[0].HasChild);
            Assert.AreEqual(questionSet.PosiblyDeployed, questionSets[0].PosiblyDeployed);
            
            //Old Version
            
            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSetVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, questionSetVersions[0].Version);                                      
//            Assert.AreEqual(oldUpdatedAt.ToString(), questionSetVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(questionSetVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(questionSet.Id, questionSetVersions[0].QuestionSetId);
            Assert.AreEqual(questionSet.Name, questionSetVersions[0].Name);
            Assert.AreEqual(questionSet.Share, questionSetVersions[0].Share);
            Assert.AreEqual(questionSet.HasChild, questionSetVersions[0].HasChild);
            Assert.AreEqual(questionSet.PosiblyDeployed, questionSetVersions[0].PossiblyDeployed);
            
            //New Version
            Assert.AreEqual(questionSet.CreatedAt.ToString(), questionSetVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, questionSetVersions[1].Version);                                      
//            Assert.AreEqual(questionSet.UpdatedAt.ToString(), questionSetVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(questionSetVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(questionSet.Id, questionSetVersions[1].QuestionSetId);
            Assert.AreEqual(questionSet.Name, questionSetVersions[1].Name);
            Assert.AreEqual(questionSet.Share, questionSetVersions[1].Share);
            Assert.AreEqual(questionSet.HasChild, questionSetVersions[1].HasChild);
            Assert.AreEqual(questionSet.PosiblyDeployed, questionSetVersions[1].PossiblyDeployed);
        }
    }
}