using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Integration.SqlControllerTests
{
    [TestFixture]
    public class SqlControllerTestLanguage : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
        string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:", "");

        public override async Task DoSetup()
        {
            if (sut == null)
            {
                sut = new SqlController(ConnectionString);
                await sut.StartLog(new CoreBase());
            }
            testHelpers = new TestHelpers();
            await sut.SettingUpdate(Settings.fileLocationPicture, Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SettingUpdate(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SettingUpdate(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
        }

        [Test]
        public async Task languages_Create_DoesCreate()
        {
            // Arrange
//            string name = Guid.NewGuid().ToString();
//            string description = Guid.NewGuid().ToString();

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