using NUnit.Framework;
using eFormCore;
using Microsoft.EntityFrameworkCore;
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
using Microting.eForm.Infrastructure.Helpers;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestLanguage : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(dbContextHelper);
            await sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            await sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            await sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            await sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }

        [Test]
        public async Task languages_Create_DoesCreate()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            string description = Guid.NewGuid().ToString();

            languages language = new languages();

            language.Name = Guid.NewGuid().ToString();
            language.Description = Guid.NewGuid().ToString();

            // Act
            await language.Create(dbContext);

            languages dbLanguage = dbContext.languages.AsNoTracking().First();
            language_versions dbLanguageVersion = dbContext.language_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbLanguage);
            Assert.NotNull(dbLanguageVersion);
            
            Assert.AreEqual(language.Name, dbLanguage.Name);
            Assert.AreEqual(language.Description, dbLanguage.Description);
        }
        
        [Test]
        public async Task languages_Update_DoesUpdate()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            string description = Guid.NewGuid().ToString();

            languages language = new languages();

            language.Name = name;
            language.Description = description;
                
            await language.Create(dbContext);
            // Act

            string newName = Guid.NewGuid().ToString();
            string newDescription = Guid.NewGuid().ToString();

            language.Name = newName;
            language.Description = newDescription;
            await language.Update(dbContext);

            languages dbLanguage = dbContext.languages.AsNoTracking().First();
            language_versions dbLanguageVersion = dbContext.language_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbLanguage);
            Assert.NotNull(dbLanguageVersion);
            
            Assert.AreNotEqual(name, dbLanguage.Name);
            Assert.AreNotEqual(description, dbLanguage.Description);
            Assert.AreEqual(newName, dbLanguage.Name);
            Assert.AreEqual(newDescription, dbLanguage.Description);
            
        }

        [Test]
        public async Task languages_Delete_DoesDelete()
        {
            // Arrange
            string name = Guid.NewGuid().ToString();
            string description = Guid.NewGuid().ToString();

            languages language = new languages();

            language.Name = name;
            language.Description = description;

            await language.Create(dbContext);

            // Act

            await language.Delete(dbContext);
            
            languages dbLanguage = dbContext.languages.AsNoTracking().First();
            language_versions dbLanguageVersion = dbContext.language_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbLanguage);
            Assert.NotNull(dbLanguageVersion);
            
            Assert.AreEqual(language.WorkflowState, Constants.WorkflowStates.Removed);
            
        }


        
    }
}