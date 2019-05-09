/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 microting

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
using eFormShared;

namespace eFormSqlController
{
    public partial class languages : BaseEntity
    {
        public string name { get; set; }
        
        public string description { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
            version = 1;
            workflow_state = Constants.WorkflowStates.Created;

            dbContext.languages.Add(this);
            dbContext.SaveChanges();

            dbContext.language_versions.Add(MapVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            languages languages = dbContext.languages.FirstOrDefault(x => x.Id == Id);

            if (languages == null)
            {
                throw new NullReferenceException($"Could not find language wit Id: {Id}");
            }

            languages.name = name;
            languages.description = description;

            if (dbContext.ChangeTracker.HasChanges())
            {
                languages.version += 1;
                languages.updated_at = DateTime.Now;

                dbContext.language_versions.Add(MapVersions(languages));
                dbContext.SaveChanges();
            }

        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            languages language = dbContext.languages.FirstOrDefault(x => x.Id == Id);

            if (language == null)
            {
                throw new NullReferenceException($"Could not find language with Id: {Id}");
            }

            language.workflow_state = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                language.version += 1;
                language.updated_at = DateTime.Now;

                dbContext.language_versions.Add(MapVersions(language));
                dbContext.SaveChanges();
            }
        }
        
        private language_versions MapVersions(languages language)
        {
            language_versions languageVersions = new language_versions();

            languageVersions.languageId = language.Id;
            languageVersions.name = language.name;
            languageVersions.description = language.description;
            languageVersions.version = language.version;
            languageVersions.created_at = language.created_at;
            languageVersions.updated_at = language.updated_at;
            languageVersions.workflow_state = language.workflow_state;

            
            return languageVersions;
        }
    }
}