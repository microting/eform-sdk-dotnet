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

using System.Linq;
using eFormShared;

namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class tags : base_entity
    {
        public tags()
        {
            this.taggings = new HashSet<taggings>();
            //this.check_lists = new HashSet<check_lists>();
        }

//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int id { get; set; }
//
//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        public int? taggings_count { get; set; }

//        public int? version { get; set; }
//
//        [StringLength(255)]
//        public string workflow_state { get; set; }

        public virtual ICollection<taggings> taggings { get; set; }

        //public virtual ICollection<check_lists> check_lists { get; set; }
        public void Create(MicrotingDbAnySql dbContext)
        {
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.tags.Add(this);
            dbContext.SaveChanges();

            dbContext.tag_versions.Add(MapTagVersions(this));
            dbContext.SaveChanges();

        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            tags tag = dbContext.tags.FirstOrDefault(x => x.id == id);

            if (tag == null)
            {
                throw new NullReferenceException($"Could not find Tag with id: {id}");
            }

            tag.name = name;
            tag.taggings_count = taggings_count;

            if (dbContext.ChangeTracker.HasChanges())
            {
                tag.version += 1;
                tag.updated_at = DateTime.Now;

                dbContext.tag_versions.Add(MapTagVersions(tag));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            
            tags tag = dbContext.tags.FirstOrDefault(x => x.id == id);

            if (tag == null)
            {
                throw new NullReferenceException($"Could not find Tag with id: {id}");
            }

            tag.workflow_state = Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                tag.version += 1;
                tag.updated_at = DateTime.Now;

                dbContext.tag_versions.Add(MapTagVersions(tag));
                dbContext.SaveChanges();
            }
        }

        
        private tag_versions MapTagVersions(tags tags)
        {
            tag_versions tagVer = new tag_versions();
            tagVer.workflow_state = tags.workflow_state;
            tagVer.version = tags.version;
            tagVer.created_at = tags.created_at;
            tagVer.updated_at = tags.updated_at;
            tagVer.name = tags.name;

            return tagVer;
        }

    }
}
