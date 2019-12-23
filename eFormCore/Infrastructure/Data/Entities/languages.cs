/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 Microting A/S

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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class languages : BaseEntity
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Version = 1;
            WorkflowState = Constants.Constants.WorkflowStates.Created;

            dbContext.languages.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.language_versions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            languages languages = await dbContext.languages.FirstOrDefaultAsync(x => x.Id == Id);

            if (languages == null)
            {
                throw new NullReferenceException($"Could not find language wit Id: {Id}");
            }

            languages.Name = Name;
            languages.Description = Description;

            if (dbContext.ChangeTracker.HasChanges())
            {
                languages.Version += 1;
                languages.UpdatedAt = DateTime.Now;

                dbContext.language_versions.Add(MapVersions(languages));
                await dbContext.SaveChangesAsync();
            }

        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            languages language = await dbContext.languages.FirstOrDefaultAsync(x => x.Id == Id);

            if (language == null)
            {
                throw new NullReferenceException($"Could not find language with Id: {Id}");
            }

            language.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                language.Version += 1;
                language.UpdatedAt = DateTime.Now;

                dbContext.language_versions.Add(MapVersions(language));
                await dbContext.SaveChangesAsync();
            }
        }
        
        private language_versions MapVersions(languages language)
        {
            language_versions languageVersions = new language_versions();

            languageVersions.LanguageId = language.Id;
            languageVersions.Name = language.Name;
            languageVersions.Description = language.Description;
            languageVersions.Version = language.Version;
            languageVersions.CreatedAt = language.CreatedAt;
            languageVersions.UpdatedAt = language.UpdatedAt;
            languageVersions.WorkflowState = language.WorkflowState;

            
            return languageVersions;
        }
    }
}