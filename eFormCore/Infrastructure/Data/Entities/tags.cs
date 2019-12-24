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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Dto;

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
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.tags.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.tag_versions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync();

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
                tag.UpdatedAt = DateTime.Now;

                dbContext.tag_versions.Add(MapVersions(tag));
                await dbContext.SaveChangesAsync();
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
                tag.UpdatedAt = DateTime.Now;

                dbContext.tag_versions.Add(MapVersions(tag));
                await dbContext.SaveChangesAsync();
            }
        }
        
        public static async Task<List<Tag>> GetAll(MicrotingDbContext dbContext, bool includeRemoved)
        {
            List<Tag> tags = new List<Tag>();
            List<tags> matches = null;
            if (!includeRemoved)
                matches = await dbContext.tags.Where(x => x.WorkflowState == Constants.Constants.WorkflowStates.Created).ToListAsync();
            else
                matches = await dbContext.tags.ToListAsync();

            foreach (tags tag in matches)
            {
                Tag t = new Tag(tag.Id, tag.Name, tag.TaggingsCount);
                tags.Add(t);
            }

            return tags;
        }

        private tag_versions MapVersions(tags tags)
        {
            tag_versions tagVer = new tag_versions();
            tagVer.WorkflowState = tags.WorkflowState;
            tagVer.Version = tags.Version;
            tagVer.CreatedAt = tags.CreatedAt;
            tagVer.UpdatedAt = tags.UpdatedAt;
            tagVer.Name = tags.Name;
            tagVer.TagId = tags.Id;

            return tagVer;
        }

    }
}
