/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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
            sut.StartLog(new CoreBase());
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

            languages language = new languages
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };


            // Act
            await language.Create(dbContext).ConfigureAwait(false);

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

            languages language = new languages
            {
                Name = name,
                Description = description
            };


            await language.Create(dbContext).ConfigureAwait(false);
            // Act

            string newName = Guid.NewGuid().ToString();
            string newDescription = Guid.NewGuid().ToString();

            language.Name = newName;
            language.Description = newDescription;
            await language.Update(dbContext).ConfigureAwait(false);

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

            languages language = new languages
            {
                Name = name,
                Description = description
            };


            await language.Create(dbContext).ConfigureAwait(false);

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