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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class taggings : BaseEntity
    {
        [ForeignKey("tag")]
        public int? TagId { get; set; }

        [ForeignKey("check_list")]
        public int? CheckListId { get; set; }

        public int? TaggerId { get; set; } // this will refer to some user Id.

        public virtual tags Tag { get; set; }

        public virtual check_lists CheckList { get; set; }

        public async Task Create(MicrotingDbAnySql dbContext)
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Version = 1;
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            
            dbContext.taggings.Add(this);
            dbContext.SaveChanges();

            dbContext.tagging_versions.Add(MapTaggingVersions(this));
            dbContext.SaveChanges();
        }

        public async Task Update(MicrotingDbAnySql dbContext)
        {
            taggings tagging = dbContext.taggings.FirstOrDefault(x => x.Id == Id);

            if (tagging == null)
            {
                throw new NullReferenceException($"Could not find tagging with Id: {Id}");
            }

            tagging.WorkflowState = WorkflowState;
            tagging.UpdatedAt = DateTime.Now;
            tagging.TaggerId = TaggerId;
            tagging.Version += 1;

            dbContext.SaveChanges();

            dbContext.tagging_versions.Add(MapTaggingVersions(tagging));
            dbContext.SaveChanges();
        }

        public async Task Delete(MicrotingDbAnySql dbContext)
        {
            taggings tagging = dbContext.taggings.FirstOrDefault(x => x.Id == Id);

            if (tagging == null)
            {
                throw new NullReferenceException($"Could not find tagging with Id: {Id}");
            }

            tagging.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            tagging.UpdatedAt = DateTime.Now;
            tagging.Version += 1;

            dbContext.SaveChanges();

            dbContext.tagging_versions.Add(MapTaggingVersions(tagging));
            dbContext.SaveChanges();
        }
        
        private tagging_versions MapTaggingVersions(taggings tagging)
        {
            tagging_versions taggingVer = new tagging_versions();
            taggingVer.WorkflowState = tagging.WorkflowState;
            taggingVer.Version = tagging.Version;
            taggingVer.CreatedAt = tagging.CreatedAt;
            taggingVer.UpdatedAt = tagging.UpdatedAt;
            taggingVer.CheckListId = tagging.CheckListId;
            taggingVer.TagId = tagging.TagId;
            taggingVer.TaggerId = tagging.TaggerId;
            taggingVer.TaggingId = tagging.Id;

            return taggingVer;
        }
    }
}
