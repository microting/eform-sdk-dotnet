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

            Assert.That(languages, Is.Not.EqualTo(null));
            Assert.That(languageVersions, Is.Not.EqualTo(null));

            Assert.That(languages.Count, Is.EqualTo(29));
            Assert.That(languageVersions.Count, Is.EqualTo(31));

            Assert.That(languages[28].CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
            Assert.That(languages[28].Version, Is.EqualTo(language.Version));
            //            Assert.AreEqual(language.UpdatedAt.ToString(), languages[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(languages[28].WorkflowState));
            Assert.That(languages[28].Id, Is.EqualTo(language.Id));
            Assert.That(languages[28].LanguageCode, Is.EqualTo(language.LanguageCode));
            Assert.That(languages[28].Name, Is.EqualTo(language.Name));

            Assert.That(languageVersions[30].CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
            Assert.That(languageVersions[30].Version, Is.EqualTo(language.Version));
            //            Assert.AreEqual(language.UpdatedAt.ToString(), languageVersions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(languageVersions[30].WorkflowState));
            Assert.That(languageVersions[30].LanguageId, Is.EqualTo(language.Id));
            Assert.That(languageVersions[30].LanguageCode, Is.EqualTo(language.LanguageCode));
            Assert.That(languageVersions[30].Name, Is.EqualTo(language.Name));
            Assert.That(languages[28].IsActive, Is.EqualTo(true));
            Assert.That(languageVersions[30].IsActive, Is.EqualTo(true));
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

            Assert.That(languages, Is.Not.EqualTo(null));
            Assert.That(languageVersions, Is.Not.EqualTo(null));

            Assert.That(languages.Count, Is.EqualTo(29));
            Assert.That(languageVersions.Count, Is.EqualTo(32));

            Assert.That(languages[28].CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
            Assert.That(languages[28].Version, Is.EqualTo(language.Version));
            //            Assert.AreEqual(language.UpdatedAt.ToString(), languages[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(languages[28].WorkflowState));
            Assert.That(languages[28].Id, Is.EqualTo(language.Id));
            Assert.That(languages[28].LanguageCode, Is.EqualTo(language.LanguageCode));
            Assert.That(languages[28].Name, Is.EqualTo(language.Name));

            //Old Version
            Assert.That(languageVersions[30].CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
            Assert.That(languageVersions[29].Version, Is.EqualTo(1));
            //            Assert.AreEqual(oldUpdatedAt.ToString(), languageVersions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(languageVersions[29].WorkflowState));
            Assert.That(languageVersions[31].LanguageId, Is.EqualTo(language.Id));
            Assert.That(languageVersions[30].LanguageCode, Is.EqualTo(oldDescription));
            Assert.That(languageVersions[30].Name, Is.EqualTo(oldName));
            Assert.That(languageVersions[30].IsActive, Is.EqualTo(false));

            //New Version
            Assert.That(languageVersions[30].CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
            Assert.That(languageVersions[31].Version, Is.EqualTo(language.Version));
            //            Assert.AreEqual(language.UpdatedAt.ToString(), languageVersions[1].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(languageVersions[31].WorkflowState));
            Assert.That(languageVersions[31].LanguageId, Is.EqualTo(language.Id));
            Assert.That(languageVersions[31].LanguageCode, Is.EqualTo(language.LanguageCode));
            Assert.That(languageVersions[31].Name, Is.EqualTo(language.Name));
            Assert.That(languageVersions[31].IsActive, Is.EqualTo(true));
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

            Assert.That(languages, Is.Not.EqualTo(null));
            Assert.That(languageVersions, Is.Not.EqualTo(null));

            Assert.That(languages.Count, Is.EqualTo(29));
            Assert.That(languageVersions.Count, Is.EqualTo(32));

            Assert.That(languages[28].CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
            Assert.That(languages[28].Version, Is.EqualTo(language.Version));
            //            Assert.AreEqual(language.UpdatedAt.ToString(), languages[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Removed, Is.EqualTo(languages[28].WorkflowState));
            Assert.That(languages[28].Id, Is.EqualTo(language.Id));
            Assert.That(languages[28].LanguageCode, Is.EqualTo(language.LanguageCode));
            Assert.That(languages[28].Name, Is.EqualTo(language.Name));

            //Old Version
            Assert.That(languageVersions[30].CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
            Assert.That(languageVersions[30].Version, Is.EqualTo(1));
            //            Assert.AreEqual(oldUpdatedAt.ToString(), languageVersions[0].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Created, Is.EqualTo(languageVersions[29].WorkflowState));
            Assert.That(languageVersions[30].LanguageId, Is.EqualTo(language.Id));
            Assert.That(languageVersions[30].LanguageCode, Is.EqualTo(language.LanguageCode));
            Assert.That(languageVersions[30].Name, Is.EqualTo(language.Name));

            //New Version
            Assert.That(languageVersions[30].CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
            Assert.That(languageVersions[31].Version, Is.EqualTo(language.Version));
            //            Assert.AreEqual(language.UpdatedAt.ToString(), languageVersions[1].UpdatedAt.ToString());
            Assert.That(Constants.WorkflowStates.Removed, Is.EqualTo(languageVersions[31].WorkflowState));
            Assert.That(languageVersions[31].LanguageId, Is.EqualTo(language.Id));
            Assert.That(languageVersions[31].LanguageCode, Is.EqualTo(language.LanguageCode));
            Assert.That(languageVersions[31].Name, Is.EqualTo(language.Name));
        }
    }
}