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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class tags : BaseEntity
    {
        public tags()
        {
            this.Taggings = new HashSet<taggings>();
        }

        [StringLength(255)]
        public string Name { get; set; }

        public int? TaggingsCount { get; set; }

        public virtual ICollection<taggings> Taggings { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            dbContext.tags.Add(this);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            dbContext.tag_versions.Add(MapTagVersions(this));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            tags tag = await dbContext.tags.FirstOrDefaultAsync(x => x.Id == Id);

            if (tag == null)
            {
                throw new NullReferenceException($"Could not find Tag with Id: {Id}");
            }

            tag.Name = Name;
            tag.TaggingsCount = TaggingsCount;
            tag.WorkflowState = WorkflowState; // TODO extend tests to include WorkflowState

            if (dbContext.ChangeTracker.HasChanges())
            {
                tag.Version += 1;
                tag.UpdatedAt = DateTime.UtcNow;

                dbContext.tag_versions.Add(MapTagVersions(tag));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            
            tags tag = await dbContext.tags.FirstOrDefaultAsync(x => x.Id == Id);

            if (tag == null)
            {
                throw new NullReferenceException($"Could not find Tag with Id: {Id}");
            }

            tag.WorkflowState = Constants.Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                tag.Version += 1;
                tag.UpdatedAt = DateTime.UtcNow;

                dbContext.tag_versions.Add(MapTagVersions(tag));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        
        private tag_versions MapTagVersions(tags tags)
        {
            return new tag_versions
            {
                WorkflowState = tags.WorkflowState,
                Version = tags.Version,
                CreatedAt = tags.CreatedAt,
                UpdatedAt = tags.UpdatedAt,
                Name = tags.Name,
                TagId = tags.Id
            };
        }

    }
}
