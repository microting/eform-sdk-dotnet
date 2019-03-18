using NUnit.Framework;
using eFormCore;
using eFormCore.Helpers;
using eFormData;
using eFormShared;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestLanguage : DbTestFixture
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
        public void languages_Create_DoesCreate()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            string description = Guid.NewGuid().ToString();

            languages language = new languages();

            language.name = Guid.NewGuid().ToString();
            language.description = Guid.NewGuid().ToString();

            // Act
            language.Create(DbContext);

            languages dbLanguage = DbContext.languages.AsNoTracking().First();
            language_versions dbLanguageVersion = DbContext.language_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbLanguage);
            Assert.NotNull(dbLanguageVersion);
            
            Assert.AreEqual(language.name, dbLanguage.name);
            Assert.AreEqual(language.description, dbLanguage.description);
        }
        
        [Test]
        public void languages_Update_DoesUpdate()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            string description = Guid.NewGuid().ToString();

            languages language = new languages();

            language.name = name;
            language.description = description;
                
            language.Create(DbContext);
            // Act

            string newName = Guid.NewGuid().ToString();
            string newDescription = Guid.NewGuid().ToString();

            language.name = newName;
            language.description = newDescription;
            language.Update(DbContext);

            languages dbLanguage = DbContext.languages.AsNoTracking().First();
            language_versions dbLanguageVersion = DbContext.language_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbLanguage);
            Assert.NotNull(dbLanguageVersion);
            
            Assert.AreNotEqual(name, dbLanguage.name);
            Assert.AreNotEqual(description, dbLanguage.description);
            Assert.AreEqual(newName, dbLanguage.name);
            Assert.AreEqual(newDescription, dbLanguage.description);
            
        }

        [Test]
        public void languages_Delete_DoesDelete()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            string description = Guid.NewGuid().ToString();

            languages language = new languages();

            language.name = name;
            language.description = description;

            language.Create(DbContext);

            // Act

            language.Delete(DbContext);
            
            languages dbLanguage = DbContext.languages.AsNoTracking().First();
            language_versions dbLanguageVersion = DbContext.language_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbLanguage);
            Assert.NotNull(dbLanguageVersion);
            
            Assert.AreEqual(language.workflow_state, Constants.WorkflowStates.Removed);
            
        }


        
    }
}