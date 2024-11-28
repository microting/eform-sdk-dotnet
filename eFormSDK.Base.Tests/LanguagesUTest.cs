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

namespace eFormSDK.Base.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class LanguagesUTest : DbTestFixture
{
    [Test]
    public async Task Languages_Create_DoesCreate()
    {
        // Arrange
        await Language.AddDefaultLanguages(DbContext);

        // Act
        List<Language> languages = DbContext.Languages.AsNoTracking().ToList();
        List<LanguageVersion> languageVersions = DbContext.LanguageVersions.AsNoTracking().ToList();

        // Assert
        var expectedLanguages = new List<KeyValuePair<string, string>>
        {
            new("Dansk", "da"),
            new("English", "en-US"),
            new("Deutsch", "de-DE"),
            new("українська", "uk-UA"),
            new("Polski", "pl-PL"),
            new("Norsk", "no-NO"),
            new("Svenska", "sv-SE"),
            new("Española", "es-ES"),
            new("Français", "fr-FR"),
            new("Italiana", "it-IT"),
            new("Neerlandais", "nl-NL"),
            new("Portugues do Brasil", "pt-BR"),
            new("Português", "pt-PT"),
            new("Suomalainen", "fi-FI"),
            new("Türkçe", "tr-TR"),
            new("Eesti", "et-ET"),
            new("Latviski", "lv-LV"),
            new("Lietuvių", "lt-LT"),
            new("Română", "ro-RO"),
            new("български", "bg-BG"),
            new("Slovenský", "sk-SK"),
            new("Slovenščina", "sl-SL"),
            new("Íslenska", "is-IS"),
            new("Čeština", "cs-CZ"),
            new("Hrvatski", "hr-HR"),
            new("Ελληνικά", "el-GR"),
            new("Magyar", "hu-HU")
        };

        Assert.That(languages.Count, Is.EqualTo(expectedLanguages.Count));
        Assert.That(languageVersions.Count,
            Is.EqualTo(expectedLanguages.Count + 2)); // Each language has 2 versions plus 2 initial versions

        foreach (var kvp in expectedLanguages)
        {
            var language = languages.FirstOrDefault(x => x.LanguageCode == kvp.Value);
            var languageVersion =
                languageVersions.FirstOrDefault(x => x.LanguageCode == kvp.Value && x.Version == 1);

            Assert.That(language, Is.Not.Null);
            Assert.That(language.Name, Is.EqualTo(kvp.Key));
            Assert.That(language.IsActive,
                Is.EqualTo(kvp.Value == "da" || kvp.Value == "en-US" || kvp.Value == "de-DE"));

            Assert.That(languageVersion, Is.Not.Null);
            Assert.That(languageVersion.Name, Is.EqualTo(kvp.Key));
            Assert.That(languageVersion.IsActive,
                Is.EqualTo(kvp.Value == "da" || kvp.Value == "en-US" || kvp.Value == "de-DE"));
        }
    }

    [Test]
    public async Task Languages_Update_DoesUpdate()
    {
        // Arrange
        Language language = new Language
        {
            LanguageCode = Guid.NewGuid().ToString(),
            Name = Guid.NewGuid().ToString(),
            IsActive = false
        };
        await language.Create(DbContext).ConfigureAwait(false);

        // Act
        DateTime? oldUpdatedAt = language.UpdatedAt;
        string oldLanguageCode = language.LanguageCode;
        string oldName = language.Name;

        language.LanguageCode = Guid.NewGuid().ToString();
        language.Name = Guid.NewGuid().ToString();
        language.IsActive = true;
        await language.Update(DbContext).ConfigureAwait(false);

        List<Language> languages = DbContext.Languages.AsNoTracking().ToList();
        List<LanguageVersion> languageVersions = DbContext.LanguageVersions.AsNoTracking().ToList();

        // Assert
        Assert.That(languages, Is.Not.Null);
        Assert.That(languageVersions, Is.Not.Null);

        var updatedLanguage = languages.FirstOrDefault(x => x.Id == language.Id);
        var oldVersion = languageVersions.FirstOrDefault(x => x.LanguageCode == oldLanguageCode && x.Version == 1);
        var newVersion =
            languageVersions.FirstOrDefault(x => x.LanguageCode == language.LanguageCode && x.Version == 2);

        Assert.That(updatedLanguage, Is.Not.Null);
        Assert.That(updatedLanguage.CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
        Assert.That(updatedLanguage.Version, Is.EqualTo(language.Version));
        Assert.That(updatedLanguage.WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(updatedLanguage.LanguageCode, Is.EqualTo(language.LanguageCode));
        Assert.That(updatedLanguage.Name, Is.EqualTo(language.Name));
        Assert.That(updatedLanguage.IsActive, Is.EqualTo(language.IsActive));

        Assert.That(oldVersion, Is.Not.Null);
        Assert.That(oldVersion.CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
        Assert.That(oldVersion.Version, Is.EqualTo(1));
        Assert.That(oldVersion.WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(oldVersion.LanguageId, Is.EqualTo(language.Id));
        Assert.That(oldVersion.LanguageCode, Is.EqualTo(oldLanguageCode));
        Assert.That(oldVersion.Name, Is.EqualTo(oldName));
        Assert.That(oldVersion.IsActive, Is.EqualTo(false));

        Assert.That(newVersion, Is.Not.Null);
        Assert.That(newVersion.CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
        Assert.That(newVersion.Version, Is.EqualTo(language.Version));
        Assert.That(newVersion.WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(newVersion.LanguageId, Is.EqualTo(language.Id));
        Assert.That(newVersion.LanguageCode, Is.EqualTo(language.LanguageCode));
        Assert.That(newVersion.Name, Is.EqualTo(language.Name));
        Assert.That(newVersion.IsActive, Is.EqualTo(true));
    }

    [Test]
    public async Task Languages_Delete_DoesSetWorkflowStateToRemoved()
    {
        // Arrange
        Language language = new Language
        {
            LanguageCode = Guid.NewGuid().ToString(),
            Name = Guid.NewGuid().ToString()
        };
        await language.Create(DbContext).ConfigureAwait(false);

        // Act
        DateTime? oldUpdatedAt = language.UpdatedAt;
        await language.Delete(DbContext);

        List<Language> languages = DbContext.Languages.AsNoTracking().ToList();
        List<LanguageVersion> languageVersions = DbContext.LanguageVersions.AsNoTracking().ToList();

        // Assert
        Assert.That(languages, Is.Not.Null);
        Assert.That(languageVersions, Is.Not.Null);

        var deletedLanguage = languages.FirstOrDefault(x => x.Id == language.Id);
        var oldVersion =
            languageVersions.FirstOrDefault(x => x.LanguageCode == language.LanguageCode && x.Version == 1);
        var newVersion =
            languageVersions.FirstOrDefault(x => x.LanguageCode == language.LanguageCode && x.Version == 2);

        Assert.That(deletedLanguage, Is.Not.Null);
        Assert.That(deletedLanguage.CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
        Assert.That(deletedLanguage.Version, Is.EqualTo(language.Version));
        Assert.That(deletedLanguage.WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(deletedLanguage.LanguageCode, Is.EqualTo(language.LanguageCode));
        Assert.That(deletedLanguage.Name, Is.EqualTo(language.Name));

        Assert.That(oldVersion, Is.Not.Null);
        Assert.That(oldVersion.CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
        Assert.That(oldVersion.Version, Is.EqualTo(1));
        Assert.That(oldVersion.WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(oldVersion.LanguageId, Is.EqualTo(language.Id));
        Assert.That(oldVersion.LanguageCode, Is.EqualTo(language.LanguageCode));
        Assert.That(oldVersion.Name, Is.EqualTo(language.Name));

        Assert.That(newVersion, Is.Not.Null);
        Assert.That(newVersion.CreatedAt.ToString(), Is.EqualTo(language.CreatedAt.ToString()));
        Assert.That(newVersion.Version, Is.EqualTo(language.Version));
        Assert.That(newVersion.WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(newVersion.LanguageId, Is.EqualTo(language.Id));
        Assert.That(newVersion.LanguageCode, Is.EqualTo(language.LanguageCode));
        Assert.That(newVersion.Name, Is.EqualTo(language.Name));
    }
}