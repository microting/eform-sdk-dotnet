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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Base.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class LanguagesUTest : DbTestFixture
    {
        [Test]
        public async Task Languages_Create_DoesCreate()
        {
            //Arrange

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                IsActive = true
            };

            //Act

            await language.Create(DbContext).ConfigureAwait(false);

            List<Language> languages = DbContext.Languages.AsNoTracking().ToList();
            List<LanguageVersion> languageVersions = DbContext.LanguageVersions.AsNoTracking().ToList();

            Assert.NotNull(languages);
            Assert.NotNull(languageVersions);

            Assert.AreEqual(29, languages.Count);
            Assert.AreEqual(31, languageVersions.Count);

            Assert.AreEqual(language.CreatedAt.ToString(), languages[28].CreatedAt.ToString());
            Assert.AreEqual(language.Version, languages[28].Version);
//            Assert.AreEqual(language.UpdatedAt.ToString(), languages[0].UpdatedAt.ToString());
            Assert.AreEqual(languages[28].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(language.Id, languages[28].Id);
            Assert.AreEqual(language.LanguageCode, languages[28].LanguageCode);
            Assert.AreEqual(language.Name, languages[28].Name);

            Assert.AreEqual(language.CreatedAt.ToString(), languageVersions[30].CreatedAt.ToString());
            Assert.AreEqual(language.Version, languageVersions[30].Version);
//            Assert.AreEqual(language.UpdatedAt.ToString(), languageVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(languageVersions[30].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(language.Id, languageVersions[30].LanguageId);
            Assert.AreEqual(language.LanguageCode, languageVersions[30].LanguageCode);
            Assert.AreEqual(language.Name, languageVersions[30].Name);
            Assert.AreEqual(true, languages[28].IsActive);
            Assert.AreEqual(true, languageVersions[30].IsActive);
        }

        [Test]
        public async Task Languages_Update_DoesUpdate()
        {
            //Arrange

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                IsActive = false
            };
            await language.Create(DbContext).ConfigureAwait(false);

            //Act
            DateTime? oldUpdatedAt = language.UpdatedAt;
            string oldDescription = language.LanguageCode;
            string oldName = language.Name;

            language.LanguageCode = Guid.NewGuid().ToString();
            language.Name = Guid.NewGuid().ToString();
            language.IsActive = true;
            await language.Update(DbContext).ConfigureAwait(false);


            List<Language> languages = DbContext.Languages.AsNoTracking().ToList();
            List<LanguageVersion> languageVersions = DbContext.LanguageVersions.AsNoTracking().ToList();

            Assert.NotNull(languages);
            Assert.NotNull(languageVersions);

            Assert.AreEqual(29, languages.Count);
            Assert.AreEqual(32, languageVersions.Count);

            Assert.AreEqual(language.CreatedAt.ToString(), languages[28].CreatedAt.ToString());
            Assert.AreEqual(language.Version, languages[28].Version);
//            Assert.AreEqual(language.UpdatedAt.ToString(), languages[0].UpdatedAt.ToString());
            Assert.AreEqual(languages[28].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(language.Id, languages[28].Id);
            Assert.AreEqual(language.LanguageCode, languages[28].LanguageCode);
            Assert.AreEqual(language.Name, languages[28].Name);

            //Old Version
            Assert.AreEqual(language.CreatedAt.ToString(), languageVersions[30].CreatedAt.ToString());
            Assert.AreEqual(1, languageVersions[29].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), languageVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(languageVersions[29].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(language.Id, languageVersions[31].LanguageId);
            Assert.AreEqual(oldDescription, languageVersions[30].LanguageCode);
            Assert.AreEqual(oldName, languageVersions[30].Name);
            Assert.AreEqual(false, languageVersions[30].IsActive);

            //New Version
            Assert.AreEqual(language.CreatedAt.ToString(), languageVersions[30].CreatedAt.ToString());
            Assert.AreEqual(language.Version, languageVersions[31].Version);
//            Assert.AreEqual(language.UpdatedAt.ToString(), languageVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(languageVersions[31].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(language.Id, languageVersions[31].LanguageId);
            Assert.AreEqual(language.LanguageCode, languageVersions[31].LanguageCode);
            Assert.AreEqual(language.Name, languageVersions[31].Name);
            Assert.AreEqual(true, languageVersions[31].IsActive);
        }

        [Test]
        public async Task Languages_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange

            Language language = new Language
            {
                LanguageCode = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };
            await language.Create(DbContext).ConfigureAwait(false);

            //Act
            DateTime? oldUpdatedAt = language.UpdatedAt;

            await language.Delete(DbContext);


            List<Language> languages = DbContext.Languages.AsNoTracking().ToList();
            List<LanguageVersion> languageVersions = DbContext.LanguageVersions.AsNoTracking().ToList();

            Assert.NotNull(languages);
            Assert.NotNull(languageVersions);

            Assert.AreEqual(29, languages.Count);
            Assert.AreEqual(32, languageVersions.Count);

            Assert.AreEqual(language.CreatedAt.ToString(), languages[28].CreatedAt.ToString());
            Assert.AreEqual(language.Version, languages[28].Version);
//            Assert.AreEqual(language.UpdatedAt.ToString(), languages[0].UpdatedAt.ToString());
            Assert.AreEqual(languages[28].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(language.Id, languages[28].Id);
            Assert.AreEqual(language.LanguageCode, languages[28].LanguageCode);
            Assert.AreEqual(language.Name, languages[28].Name);

            //Old Version
            Assert.AreEqual(language.CreatedAt.ToString(), languageVersions[30].CreatedAt.ToString());
            Assert.AreEqual(1, languageVersions[30].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), languageVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(languageVersions[29].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(language.Id, languageVersions[30].LanguageId);
            Assert.AreEqual(language.LanguageCode, languageVersions[30].LanguageCode);
            Assert.AreEqual(language.Name, languageVersions[30].Name);

            //New Version
            Assert.AreEqual(language.CreatedAt.ToString(), languageVersions[30].CreatedAt.ToString());
            Assert.AreEqual(language.Version, languageVersions[31].Version);
//            Assert.AreEqual(language.UpdatedAt.ToString(), languageVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(languageVersions[31].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(language.Id, languageVersions[31].LanguageId);
            Assert.AreEqual(language.LanguageCode, languageVersions[31].LanguageCode);
            Assert.AreEqual(language.Name, languageVersions[31].Name);
        }
    }
}