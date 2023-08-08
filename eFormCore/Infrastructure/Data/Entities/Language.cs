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

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Danish") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "Danish");
                language.Name = "Dansk";
                await language.Update(dbContext);
            }
            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Dansk") == null)
            {
                Language language = new Language
                {
                    Name = "Dansk",
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

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "German") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "German");
                language.Name = "Deutsch";
                await language.Update(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Deutsch") == null)
            {
                Language language = new Language
                {
                    Name = "Deutsch",
                    LanguageCode = "de-DE",
                    IsActive = true
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Ukrainian") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "Ukrainian");
                language.Name = "українська";
                await language.Update(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "українська") == null)
            {
                Language language = new Language
                {
                    Name = "українська",
                    LanguageCode = "uk-UA",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Polish") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "Polish");
                language.Name = "Polski";
                await language.Update(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Polski") == null)
            {
                Language language = new Language
                {
                    Name = "Polski",
                    LanguageCode = "pl-PL",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Norwegian") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "Norwegian");
                language.Name = "Norsk";
                await language.Update(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Norsk") == null)
            {
                Language language = new Language
                {
                    Name = "Norsk",
                    LanguageCode = "no-NO",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Swedish") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "Swedish");
                language.Name = "Svenska";
                await language.Update(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Svenska") == null)
            {
                Language language = new Language
                {
                    Name = "Svenska",
                    LanguageCode = "sv-SE",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Spanish") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "Spanish");
                language.Name = "Español";
                await language.Update(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Español") == null)
            {
                Language language = new Language
                {
                    Name = "Española",
                    LanguageCode = "es-ES",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "French") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "French");
                language.Name = "Français";
                await language.Update(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Français") == null)
            {
                Language language = new Language
                {
                    Name = "Français",
                    LanguageCode = "fr-FR",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Italian") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "Italian");
                language.Name = "Italiana";
                await language.Update(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Italiana") == null)
            {
                Language language = new Language
                {
                    Name = "Italiana",
                    LanguageCode = "it-IT",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Dutch") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "Dutch");
                language.Name = "Neerlandais";
                await language.Update(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Neerlandais") == null)
            {
                Language language = new Language
                {
                    Name = "Neerlandais",
                    LanguageCode = "nl-NL",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Brazilian Portuguese") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "Brazilian Portuguese");
                language.Name = "Portugues do Brasil";
                await language.Update(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Portugues do Brasil") == null)
            {
                Language language = new Language
                {
                    Name = "Portugues do Brasil",
                    LanguageCode = "pt-BR",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Portuguese") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "Portuguese");
                language.Name = "Português";
                await language.Update(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Português") == null)
            {
                Language language = new Language
                {
                    Name = "Português",
                    LanguageCode = "pt-PT",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Finish") != null)
            {
                Language language = dbContext.Languages.First(x => x.Name == "Finish");
                language.Name = "Suomalainen";
                await language.Update(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Suomalainen") == null)
            {
                Language language = new Language
                {
                    Name = "Suomalainen",
                    LanguageCode = "fi-FI",
                    IsActive = false
                };
                await language.Create(dbContext);
            }

            if (dbContext.Languages.FirstOrDefault(x => x.Name == "Türkçe") == null)
            {
                Language language = new Language
                {
                    Name = "Türkçe",
                    LanguageCode = "tr-TR",
                    IsActive = false
                };
                await language.Create(dbContext);
            }
        }
    }
}