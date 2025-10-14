/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities;

public class Language : PnBase
{
    public string Name { get; set; }

    public string LanguageCode { get; set; }

    public bool IsActive { get; set; } = true;

    public static async Task AddDefaultLanguages(MicrotingDbContext dbContext)
    {
        var languages = new List<KeyValuePair<string, string>>
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

        foreach (var kvp in languages)
        {
            if (dbContext.Languages.AsNoTracking().FirstOrDefault(x => x.LanguageCode == kvp.Value) == null)
            {
                Language language = new Language
                {
                    Name = kvp.Key,
                    LanguageCode = kvp.Value,
                    IsActive = kvp.Value == "da" || kvp.Value == "en-US" || kvp.Value == "de-DE"
                };
                await language.Create(dbContext);
            }
        }
    }
}