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
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class folders : BaseEntity
    {
        public folders()
        {            
            this.Children = new HashSet<folders>();
        }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public int? MicrotingUid { get; set; }
        
        public int? ParentId { get; set; }

        public virtual folders Parent { get; set; }

        public virtual ICollection<folders> Children { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Version = 1;
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            
            dbContext.folders.Add(this);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            dbContext.folder_versions.Add(MapFolderVersions(dbContext, this));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            folders folder = dbContext.folders.FirstOrDefaultAsync(x => x.Id == Id).Result;

            if (folder == null)
            {
                throw new NullReferenceException($"Could not find area with Id: {Id}");
            }

            folder.Name = Name;
            folder.Description = Description;
            folder.ParentId = ParentId;

            if (dbContext.ChangeTracker.HasChanges())
            {
                folder.UpdatedAt = DateTime.Now;
                folder.Version += 1;

                dbContext.folder_versions.Add(MapFolderVersions(dbContext, folder));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            folders folder = dbContext.folders.SingleOrDefaultAsync(x => x.Id == Id).Result;

            if (folder == null)
            {
                throw new NullReferenceException($"Could not find area with Id: {Id}");
            }
            
            folder.WorkflowState = Constants.Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                folder.UpdatedAt = DateTime.Now;
                folder.Version += 1;

                dbContext.folder_versions.Add(MapFolderVersions(dbContext, folder));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private folder_versions MapFolderVersions(MicrotingDbContext _dbContext, folders folder)
        {
            return new folder_versions
            {
                Name = folder.Name,
                Description = folder.Description,
                ParentId = folder.ParentId,
                FolderId = folder.Id,
                CreatedAt = folder.CreatedAt,
                UpdatedAt = folder.UpdatedAt,
                WorkflowState = folder.WorkflowState,
                MicrotingUid = folder.MicrotingUid,
                Version = folder.Version
            };
        }
    }
}