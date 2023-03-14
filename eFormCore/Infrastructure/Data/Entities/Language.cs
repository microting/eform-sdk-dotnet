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

using System.Linq;
using System.Threading.Tasks;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class Language : PnBase
    {
        public string Name { get; set; }

        public string LanguageCode { get; set; }

        public bool IsActive { get; set; } = true;

        public static async Task AddDefaultLanguages(MicrotingDbContext dbContext)
        {
            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Danish") == null)
            {
                Language language = new Language
                {
                    Name = "Danish",
                    LanguageCode = "da",
                    IsActive = true
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "English") == null)
            {
                Language language = new Language
                {
                    Name = "English",
                    LanguageCode = "en-US",
                    IsActive = true
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "German") == null)
            {
                Language language = new Language
                {
                    Name = "German",
                    LanguageCode = "de-DE",
                    IsActive = true
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Ukrainian") == null)
            {
                Language language = new Language
                {
                    Name = "Ukrainian",
                    LanguageCode = "uk-UA",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Polish") == null)
            {
                Language language = new Language
                {
                    Name = "Polish",
                    LanguageCode = "pl-PL",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Norwegian") == null)
            {
                Language language = new Language
                {
                    Name = "Norwegian",
                    LanguageCode = "no-NO",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Swedish") == null)
            {
                Language language = new Language
                {
                    Name = "Swedish",
                    LanguageCode = "sv-SE",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Spanish") == null)
            {
                Language language = new Language
                {
                    Name = "Spanish",
                    LanguageCode = "es-ES",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "French") == null)
            {
                Language language = new Language
                {
                    Name = "French",
                    LanguageCode = "fr-FR",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Italian") == null)
            {
                Language language = new Language
                {
                    Name = "Italian",
                    LanguageCode = "it-IT",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Dutch") == null)
            {
                Language language = new Language
                {
                    Name = "Dutch",
                    LanguageCode = "nl-NL",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Brazilian Portuguese") == null)
            {
                Language language = new Language
                {
                    Name = "Brazilian Portuguese",
                    LanguageCode = "pt-BR",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Portuguese") == null)
            {
                Language language = new Language
                {
                    Name = "Portuguese",
                    LanguageCode = "pt-PT",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Finish") == null)
            {
                Language language = new Language
                {
                    Name = "Finish",
                    LanguageCode = "fi-FI",
                    IsActive = false
                };
                await language.Create(dbContext);
            }
        }
    }
}