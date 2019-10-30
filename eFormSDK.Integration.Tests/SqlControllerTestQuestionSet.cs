using eFormCore;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestQuestionSet : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(ConnectionString);
            await sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            await sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            await sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            await sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }
        
        [Test]
        public async Task questionSet_Create_DoesCreate_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(false, dbQuestionSet.Share);
            Assert.AreEqual(false, dbQuestionSet.HasChild);
            Assert.AreEqual(false, dbQuestionSet.PosiblyDeployed);
        }  
        [Test]
        public async Task questionSet_Create_DoesCreate_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(true, dbQuestionSet.Share);
            Assert.AreEqual(true, dbQuestionSet.HasChild);
            Assert.AreEqual(true, dbQuestionSet.PosiblyDeployed);
        }
        [Test]
        public async Task questionSet_Create_DoesCreate_ShareTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(true, dbQuestionSet.Share);
            Assert.AreEqual(false, dbQuestionSet.HasChild);
            Assert.AreEqual(false, dbQuestionSet.PosiblyDeployed);
        }
        [Test]
        public async Task questionSet_Create_DoesCreate_HasChildTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = false;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(false, dbQuestionSet.Share);
            Assert.AreEqual(true, dbQuestionSet.HasChild);
            Assert.AreEqual(false, dbQuestionSet.PosiblyDeployed);
        }
        [Test]
        public async Task questionSet_Create_DoesCreate_PosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = true;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(false, dbQuestionSet.Share);
            Assert.AreEqual(false, dbQuestionSet.HasChild);
            Assert.AreEqual(true, dbQuestionSet.PosiblyDeployed);
        }
        [Test]
        public async Task questionSet_Create_DoesCreate_ShareAndHasChildTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = false;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(true, dbQuestionSet.Share);
            Assert.AreEqual(true, dbQuestionSet.HasChild);
            Assert.AreEqual(false, dbQuestionSet.PosiblyDeployed);
        }
        [Test]
        public async Task questionSet_Create_DoesCreate_ShareAndPosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = true;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(true, dbQuestionSet.Share);
            Assert.AreEqual(false, dbQuestionSet.HasChild);
            Assert.AreEqual(true, dbQuestionSet.PosiblyDeployed);
        }
        [Test]
        public async Task questionSet_Create_DoesCreate_HasChildAndPosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(false, dbQuestionSet.Share);
            Assert.AreEqual(true, dbQuestionSet.HasChild);
            Assert.AreEqual(true, dbQuestionSet.PosiblyDeployed);
        }
        [Test]
        public async Task questionSet_Update_DoesUpdate_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;

            questionSet.Create(DbContext);
            // Act

            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;

            questionSet.Update(DbContext);
            
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(false, dbQuestionSet.Share);
            Assert.AreEqual(false, dbQuestionSet.HasChild);
            Assert.AreEqual(false, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
        }  
        [Test]
        public async Task questionSet_Update_DoesUpdate_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            
            questionSet.Create(DbContext);

            // Act
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;

            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(true, dbQuestionSet.Share);
            Assert.AreEqual(true, dbQuestionSet.HasChild);
            Assert.AreEqual(true, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
        }
        [Test]
        public async Task questionSet_Update_DoesUpdate_ShareTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            
            questionSet.Create(DbContext);
            // Act
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            
            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(true, dbQuestionSet.Share);
            Assert.AreEqual(false, dbQuestionSet.HasChild);
            Assert.AreEqual(false, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
        }
        [Test]
        public async Task questionSet_Update_DoesUpdate_HasChildTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = false;
            
            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(false, dbQuestionSet.Share);
            Assert.AreEqual(true, dbQuestionSet.HasChild);
            Assert.AreEqual(false, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
        }
        [Test]
        public async Task questionSet_Update_DoesUpdate_PosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = true;
            
            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(false, dbQuestionSet.Share);
            Assert.AreEqual(false, dbQuestionSet.HasChild);
            Assert.AreEqual(true, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
        }
        [Test]
        public async Task questionSet_Update_DoesUpdate_ShareAndHasChildTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = false;
            
            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(true, dbQuestionSet.Share);
            Assert.AreEqual(true, dbQuestionSet.HasChild);
            Assert.AreEqual(false, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
        }
        [Test]
        public async Task questionSet_Update_DoesUpdate_ShareAndPosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = true;
            
            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(true, dbQuestionSet.Share);
            Assert.AreEqual(false, dbQuestionSet.HasChild);
            Assert.AreEqual(true, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
        }
        [Test]
        public async Task questionSet_Update_DoesUpdate_HasChildAndPosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            
            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(false, dbQuestionSet.Share);
            Assert.AreEqual(true, dbQuestionSet.HasChild);
            Assert.AreEqual(true, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
        }
        [Test]
        public async Task questionSet_Delete_DoesDelete_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;

            questionSet.Create(DbContext);
            // Act

            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;

            questionSet.Delete(DbContext);
            
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(false, dbQuestionSet.Share);
            Assert.AreEqual(false, dbQuestionSet.HasChild);
            Assert.AreEqual(false, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
            Assert.AreEqual(dbQuestionSet.WorkflowState, Constants.WorkflowStates.Removed);
        }  
        [Test]
        public async Task questionSet_Delete_DoesDelete_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            
            questionSet.Create(DbContext);

            // Act
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;

            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(true, dbQuestionSet.Share);
            Assert.AreEqual(true, dbQuestionSet.HasChild);
            Assert.AreEqual(true, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
            Assert.AreEqual(dbQuestionSet.WorkflowState, Constants.WorkflowStates.Removed);
        }
        [Test]
        public async Task questionSet_Delete_DoesDelete_ShareTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            
            questionSet.Create(DbContext);
            // Act
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;
            
            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(true, dbQuestionSet.Share);
            Assert.AreEqual(false, dbQuestionSet.HasChild);
            Assert.AreEqual(false, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
            Assert.AreEqual(dbQuestionSet.WorkflowState, Constants.WorkflowStates.Removed);
        }
        [Test]
        public async Task questionSet_Delete_DoesDelete_HasChildTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = false;
            
            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(false, dbQuestionSet.Share);
            Assert.AreEqual(true, dbQuestionSet.HasChild);
            Assert.AreEqual(false, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
            Assert.AreEqual(dbQuestionSet.WorkflowState, Constants.WorkflowStates.Removed);
        }
        [Test]
        public async Task questionSet_Delete_DoesDelete_PosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = true;
            
            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(false, dbQuestionSet.Share);
            Assert.AreEqual(false, dbQuestionSet.HasChild);
            Assert.AreEqual(true, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
            Assert.AreEqual(dbQuestionSet.WorkflowState, Constants.WorkflowStates.Removed);
        }
        [Test]
        public async Task questionSet_Delete_DoesDelete_ShareAndHasChildTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = false;
            
            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(true, dbQuestionSet.Share);
            Assert.AreEqual(true, dbQuestionSet.HasChild);
            Assert.AreEqual(false, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
            Assert.AreEqual(dbQuestionSet.WorkflowState, Constants.WorkflowStates.Removed);
        }
        [Test]
        public async Task questionSet_Delete_DoesDelete_ShareAndPosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.Name = name;
            questionSet.Share = true;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = true;
            
            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(true, dbQuestionSet.Share);
            Assert.AreEqual(false, dbQuestionSet.HasChild);
            Assert.AreEqual(true, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
            Assert.AreEqual(dbQuestionSet.WorkflowState, Constants.WorkflowStates.Removed);
        }
        [Test]
        public async Task questionSet_Delete_DoesDelete_HasChildAndPosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = false;
            questionSet.PosiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.Name = name;
            questionSet.Share = false;
            questionSet.HasChild = true;
            questionSet.PosiblyDeployed = true;
            
            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.Name, dbQuestionSet.Name);
            Assert.AreEqual(false, dbQuestionSet.Share);
            Assert.AreEqual(true, dbQuestionSet.HasChild);
            Assert.AreEqual(true, dbQuestionSet.PosiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.Version);
            Assert.AreEqual(dbQuestionSet.WorkflowState, Constants.WorkflowStates.Removed);
        }
    }
}