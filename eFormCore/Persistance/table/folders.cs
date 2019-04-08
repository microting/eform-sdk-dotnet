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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using eFormShared;
using Microsoft.EntityFrameworkCore;

namespace eFormSqlController
{
    public class folders : base_entity
    {
        public folders()
        {            
            this.children = new HashSet<folders>();
            
        }
        
        public string name { get; set; }

        public string description { get; set; }

        public int? microting_uid { get; set; }
        
        public int? parent_id { get; set; }

        public virtual folders parent { get; set; }

        public virtual ICollection<folders> children { get; set; }

        public async Task Save(MicrotingDbAnySql _dbContext)
        {
            folders folder = new folders
            {
                name = name,
                description = description,
                parent_id = parent_id,
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
                version = 1,
                workflow_state = Constants.WorkflowStates.Created
            };

            _dbContext.folders.Add(folder);
            _dbContext.SaveChanges();

            _dbContext.folder_versions.Add(MapFolderVersions(_dbContext, folder));
            _dbContext.SaveChanges();
        }

        public async Task Update(MicrotingDbAnySql _dbContext)
        {
            folders folder = await _dbContext.folders.FirstOrDefaultAsync(x => x.id == id);

            if (folder == null)
            {
                throw new NullReferenceException($"Could not find area with id: {id}");
            }

            folder.name = name;
            folder.description = description;
            folder.parent_id = parent_id;

            if (_dbContext.ChangeTracker.HasChanges())
            {
                folder.updated_at = DateTime.Now;
                folder.version += 1;

                _dbContext.folder_versions.Add(MapFolderVersions(_dbContext, folder));
                _dbContext.SaveChanges();
            }
        }

        public async Task Delete(MicrotingDbAnySql _dbContext)
        {
            folders folder = await _dbContext.folders.SingleOrDefaultAsync(x => x.id == id);

            if (folder == null)
            {
                throw new NullReferenceException($"Could not find area with id: {id}");
            }
            
            folder.workflow_state = Constants.WorkflowStates.Removed;

            if (_dbContext.ChangeTracker.HasChanges())
            {
                folder.updated_at = DateTime.Now;
                folder.version += 1;

                _dbContext.folder_versions.Add(MapFolderVersions(_dbContext, folder));
                _dbContext.SaveChanges();
            }
        }

        private folder_versions MapFolderVersions(MicrotingDbAnySql _dbContext, folders folder)
        {
            folder_versions folderVersions = new folder_versions
            {
                name = folder.name,
                description = folder.description,
                parent_id = folder.parent_id,
                folder_id = folder.id,
                created_at = folder.created_at,
                updated_at = folder.updated_at,
                workflow_state = folder.workflow_state,
                version = folder.version
            };

            return folderVersions;
        }
    }
}