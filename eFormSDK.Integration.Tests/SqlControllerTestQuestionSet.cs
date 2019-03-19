using eFormCore;
using eFormCore.Helpers;
using eFormData;
using eFormShared;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestQuestionSet : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
        private string path;

        public override void DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            sql.SettingUpdate(Settings.firstRunDone, "true");
            sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(ConnectionString);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            sut.SettingUpdate(Settings.fileLocationPicture, path + @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, path + @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, path + @"\output\dataFolder\reports\");
        }
        
        [Test]
        public void questionSet_Create_DoesCreate_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(false, dbQuestionSet.share);
            Assert.AreEqual(false, dbQuestionSet.hasChild);
            Assert.AreEqual(false, dbQuestionSet.posiblyDeployed);
        }  
        [Test]
        public void questionSet_Create_DoesCreate_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(true, dbQuestionSet.share);
            Assert.AreEqual(true, dbQuestionSet.hasChild);
            Assert.AreEqual(true, dbQuestionSet.posiblyDeployed);
        }
        [Test]
        public void questionSet_Create_DoesCreate_ShareTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(true, dbQuestionSet.share);
            Assert.AreEqual(false, dbQuestionSet.hasChild);
            Assert.AreEqual(false, dbQuestionSet.posiblyDeployed);
        }
        [Test]
        public void questionSet_Create_DoesCreate_HasChildTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = false;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(false, dbQuestionSet.share);
            Assert.AreEqual(true, dbQuestionSet.hasChild);
            Assert.AreEqual(false, dbQuestionSet.posiblyDeployed);
        }
        [Test]
        public void questionSet_Create_DoesCreate_PosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = true;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(false, dbQuestionSet.share);
            Assert.AreEqual(false, dbQuestionSet.hasChild);
            Assert.AreEqual(true, dbQuestionSet.posiblyDeployed);
        }
        [Test]
        public void questionSet_Create_DoesCreate_ShareAndHasChildTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = false;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(true, dbQuestionSet.share);
            Assert.AreEqual(true, dbQuestionSet.hasChild);
            Assert.AreEqual(false, dbQuestionSet.posiblyDeployed);
        }
        [Test]
        public void questionSet_Create_DoesCreate_ShareAndPosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = true;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(true, dbQuestionSet.share);
            Assert.AreEqual(false, dbQuestionSet.hasChild);
            Assert.AreEqual(true, dbQuestionSet.posiblyDeployed);
        }
        [Test]
        public void questionSet_Create_DoesCreate_HasChildAndPosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            
            // Act
            questionSet.Create(DbContext);

            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(false, dbQuestionSet.share);
            Assert.AreEqual(true, dbQuestionSet.hasChild);
            Assert.AreEqual(true, dbQuestionSet.posiblyDeployed);
        }
        [Test]
        public void questionSet_Update_DoesUpdate_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;

            questionSet.Create(DbContext);
            // Act

            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;

            questionSet.Update(DbContext);
            
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(false, dbQuestionSet.share);
            Assert.AreEqual(false, dbQuestionSet.hasChild);
            Assert.AreEqual(false, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
        }  
        [Test]
        public void questionSet_Update_DoesUpdate_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            
            questionSet.Create(DbContext);

            // Act
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;

            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(true, dbQuestionSet.share);
            Assert.AreEqual(true, dbQuestionSet.hasChild);
            Assert.AreEqual(true, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
        }
        [Test]
        public void questionSet_Update_DoesUpdate_ShareTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            
            questionSet.Create(DbContext);
            // Act
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            
            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(true, dbQuestionSet.share);
            Assert.AreEqual(false, dbQuestionSet.hasChild);
            Assert.AreEqual(false, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
        }
        [Test]
        public void questionSet_Update_DoesUpdate_HasChildTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = false;
            
            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(false, dbQuestionSet.share);
            Assert.AreEqual(true, dbQuestionSet.hasChild);
            Assert.AreEqual(false, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
        }
        [Test]
        public void questionSet_Update_DoesUpdate_PosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = true;
            
            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(false, dbQuestionSet.share);
            Assert.AreEqual(false, dbQuestionSet.hasChild);
            Assert.AreEqual(true, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
        }
        [Test]
        public void questionSet_Update_DoesUpdate_ShareAndHasChildTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = false;
            
            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(true, dbQuestionSet.share);
            Assert.AreEqual(true, dbQuestionSet.hasChild);
            Assert.AreEqual(false, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
        }
        [Test]
        public void questionSet_Update_DoesUpdate_ShareAndPosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = true;
            
            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(true, dbQuestionSet.share);
            Assert.AreEqual(false, dbQuestionSet.hasChild);
            Assert.AreEqual(true, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
        }
        [Test]
        public void questionSet_Update_DoesUpdate_HasChildAndPosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            
            questionSet.Update(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(false, dbQuestionSet.share);
            Assert.AreEqual(true, dbQuestionSet.hasChild);
            Assert.AreEqual(true, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
        }
        [Test]
        public void questionSet_Delete_DoesDelete_AllFalse()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;

            questionSet.Create(DbContext);
            // Act

            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;

            questionSet.Delete(DbContext);
            
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(false, dbQuestionSet.share);
            Assert.AreEqual(false, dbQuestionSet.hasChild);
            Assert.AreEqual(false, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
            Assert.AreEqual(dbQuestionSet.workflow_state, Constants.WorkflowStates.Removed);
        }  
        [Test]
        public void questionSet_Delete_DoesDelete_AllTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            
            questionSet.Create(DbContext);

            // Act
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;

            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(true, dbQuestionSet.share);
            Assert.AreEqual(true, dbQuestionSet.hasChild);
            Assert.AreEqual(true, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
            Assert.AreEqual(dbQuestionSet.workflow_state, Constants.WorkflowStates.Removed);
        }
        [Test]
        public void questionSet_Delete_DoesDelete_ShareTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            
            questionSet.Create(DbContext);
            // Act
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;
            
            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(true, dbQuestionSet.share);
            Assert.AreEqual(false, dbQuestionSet.hasChild);
            Assert.AreEqual(false, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
            Assert.AreEqual(dbQuestionSet.workflow_state, Constants.WorkflowStates.Removed);
        }
        [Test]
        public void questionSet_Delete_DoesDelete_HasChildTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = false;
            
            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(false, dbQuestionSet.share);
            Assert.AreEqual(true, dbQuestionSet.hasChild);
            Assert.AreEqual(false, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
            Assert.AreEqual(dbQuestionSet.workflow_state, Constants.WorkflowStates.Removed);
        }
        [Test]
        public void questionSet_Delete_DoesDelete_PosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = true;
            
            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(false, dbQuestionSet.share);
            Assert.AreEqual(false, dbQuestionSet.hasChild);
            Assert.AreEqual(true, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
            Assert.AreEqual(dbQuestionSet.workflow_state, Constants.WorkflowStates.Removed);
        }
        [Test]
        public void questionSet_Delete_DoesDelete_ShareAndHasChildTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = false;
            
            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(true, dbQuestionSet.share);
            Assert.AreEqual(true, dbQuestionSet.hasChild);
            Assert.AreEqual(false, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
            Assert.AreEqual(dbQuestionSet.workflow_state, Constants.WorkflowStates.Removed);
        }
        [Test]
        public void questionSet_Delete_DoesDelete_ShareAndPosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.name = name;
            questionSet.share = true;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = true;
            
            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(true, dbQuestionSet.share);
            Assert.AreEqual(false, dbQuestionSet.hasChild);
            Assert.AreEqual(true, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
            Assert.AreEqual(dbQuestionSet.workflow_state, Constants.WorkflowStates.Removed);
        }
        [Test]
        public void questionSet_Delete_DoesDelete_HasChildAndPosiblyDeployedTrue()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            
            question_sets questionSet = new question_sets();
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = false;
            questionSet.posiblyDeployed = false;

            questionSet.Create(DbContext);
            // Act
            questionSet.name = name;
            questionSet.share = false;
            questionSet.hasChild = true;
            questionSet.posiblyDeployed = true;
            
            questionSet.Delete(DbContext);
            question_sets dbQuestionSet = DbContext.question_sets.AsNoTracking().First();
            question_set_versions dbQuestionSetVersion = DbContext.question_set_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbQuestionSet);
            Assert.NotNull(dbQuestionSetVersion);
            
            Assert.AreEqual(questionSet.name, dbQuestionSet.name);
            Assert.AreEqual(false, dbQuestionSet.share);
            Assert.AreEqual(true, dbQuestionSet.hasChild);
            Assert.AreEqual(true, dbQuestionSet.posiblyDeployed);
            Assert.AreEqual(2, dbQuestionSet.version);
            Assert.AreEqual(dbQuestionSet.workflow_state, Constants.WorkflowStates.Removed);
        }
    }
}