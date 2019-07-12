using NUnit.Framework;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestLanguage : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

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
            sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }

        [Test]
        public void languages_Create_DoesCreate()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            string description = Guid.NewGuid().ToString();

            languages language = new languages();

            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();

            // Act
            language.Create(DbContext);

            languages dbLanguage = DbContext.languages.AsNoTracking().First();
            language_versions dbLanguageVersion = DbContext.language_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbLanguage);
            Assert.NotNull(dbLanguageVersion);
            
            Assert.AreEqual(language.Name, dbLanguage.Name);
            Assert.AreEqual(language.Description, dbLanguage.Description);
        }
        
        [Test]
        public void languages_Update_DoesUpdate()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            string description = Guid.NewGuid().ToString();

            languages language = new languages();

            language.Name = name;
            language.Description = description;
                
            language.Create(DbContext);
            // Act

            string newName = Guid.NewGuid().ToString();
            string newDescription = Guid.NewGuid().ToString();

            language.Name = newName;
            language.Description = newDescription;
            language.Update(DbContext);

            languages dbLanguage = DbContext.languages.AsNoTracking().First();
            language_versions dbLanguageVersion = DbContext.language_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbLanguage);
            Assert.NotNull(dbLanguageVersion);
            
            Assert.AreNotEqual(name, dbLanguage.Name);
            Assert.AreNotEqual(description, dbLanguage.Description);
            Assert.AreEqual(newName, dbLanguage.Name);
            Assert.AreEqual(newDescription, dbLanguage.Description);
            
        }

        [Test]
        public void languages_Delete_DoesDelete()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            string description = Guid.NewGuid().ToString();

            languages language = new languages();

            language.Name = name;
            language.Description = description;

            language.Create(DbContext);

            // Act

            language.Delete(DbContext);
            
            languages dbLanguage = DbContext.languages.AsNoTracking().First();
            language_versions dbLanguageVersion = DbContext.language_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbLanguage);
            Assert.NotNull(dbLanguageVersion);
            
            Assert.AreEqual(language.WorkflowState, Constants.WorkflowStates.Removed);
            
        }


        
    }
}