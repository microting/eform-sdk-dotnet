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
using System.Threading.Tasks;
using eFormShared;

namespace eFormSqlController
{
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class taggings : BaseEntity
    {
        [ForeignKey("tag")]
        public int? tag_id { get; set; }

        [ForeignKey("check_list")]
        public int? check_list_id { get; set; }

        public int? tagger_id { get; set; } // this will refer to some user id.

        public virtual tags tag { get; set; }

        public virtual check_lists check_list { get; set; }

        public void Save(MicrotingDbAnySql dbContext)
        {
            taggings tagging = new taggings
            {
                check_list_id = check_list_id,
                tag_id = tag_id,
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
                tagger_id = tagger_id,
                version = 1,
                workflow_state = Constants.WorkflowStates.Created
            };
            
            dbContext.taggings.Add(tagging);
            dbContext.SaveChanges();

            dbContext.tagging_versions.Add(MapTaggingVersions(tagging));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            taggings tagging = dbContext.taggings.FirstOrDefault(x => x.id == id);

            if (tagging == null)
            {
                throw new NullReferenceException($"Could not find tagging with id: {id}");
            }

            tagging.workflow_state = workflow_state;
            tagging.updated_at = DateTime.Now;
            tagging.tagger_id = tagger_id;
            tagging.version += 1;

            dbContext.SaveChanges();

            dbContext.tagging_versions.Add(MapTaggingVersions(tagging));
            dbContext.SaveChanges();
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            taggings tagging = dbContext.taggings.FirstOrDefault(x => x.id == id);

            if (tagging == null)
            {
                throw new NullReferenceException($"Could not find tagging with id: {id}");
            }

            tagging.workflow_state = Constants.WorkflowStates.Removed;
            tagging.updated_at = DateTime.Now;
            tagging.version += 1;

            dbContext.SaveChanges();

            dbContext.tagging_versions.Add(MapTaggingVersions(tagging));
            dbContext.SaveChanges();
        }
        
        private tagging_versions MapTaggingVersions(taggings tagging)
        {
            tagging_versions taggingVer = new tagging_versions();
            taggingVer.workflow_state = tagging.workflow_state;
            taggingVer.version = tagging.version;
            taggingVer.created_at = tagging.created_at;
            taggingVer.updated_at = tagging.updated_at;
            taggingVer.check_list_id = tagging.check_list_id;
            taggingVer.tag_id = tagging.tag_id;
            taggingVer.tagger_id = tagging.tagger_id;
            taggingVer.tagging_id = tagging.id;

            return taggingVer;
        }
    }
}
