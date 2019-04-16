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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class entity_groups : base_entity
    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int id { get; set; }
//
//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public int? version { get; set; }
//
//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        public string microting_uid { get; set; }

        public string name { get; set; }

        [StringLength(50)]
        public string type { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
           
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.entity_groups.Add(this);
            dbContext.SaveChanges();

            dbContext.entity_group_versions.Add(MapEntityGroupVersions(this));
            dbContext.SaveChanges();

        }
        
        public void Update(MicrotingDbAnySql dbContext)
        {
            entity_groups entityGroups = dbContext.entity_groups.FirstOrDefault(x => x.id == id);

            if (entityGroups == null)
            {
                throw new NullReferenceException($"Could not find Entity Group with ID: {id}");
            }

            entityGroups.microting_uid = microting_uid;
            entityGroups.name = name;
            entityGroups.type = type;

            if (dbContext.ChangeTracker.HasChanges())
            {
                entityGroups.updated_at = DateTime.Now;
                entityGroups.version += 1;

                dbContext.entity_group_versions.Add(MapEntityGroupVersions(entityGroups));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            
            entity_groups entityGroups = dbContext.entity_groups.FirstOrDefault(x => x.id == id);

            if (entityGroups == null)
            {
                throw new NullReferenceException($"Could not find Entity Group with ID: {id}");
            }

            entityGroups.workflow_state = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                entityGroups.updated_at = DateTime.Now;
                entityGroups.version += 1;

                dbContext.entity_group_versions.Add(MapEntityGroupVersions(entityGroups));
                dbContext.SaveChanges();
            }
            
        }
        
        
        
        private entity_group_versions MapEntityGroupVersions(entity_groups entityGroup)
        {
            entity_group_versions entityGroupVer = new entity_group_versions();
            entityGroupVer.created_at = entityGroup.created_at;
            entityGroupVer.entity_group_id = entityGroup.id;
            entityGroupVer.microting_uid = entityGroup.microting_uid;
            entityGroupVer.name = entityGroup.name;
            entityGroupVer.type = entityGroup.type;
            entityGroupVer.updated_at = entityGroup.updated_at;
            entityGroupVer.version = entityGroup.version;
            entityGroupVer.workflow_state = entityGroup.workflow_state;

            entityGroupVer.entity_group_id = entityGroup.id; //<<--

            return entityGroupVer;
        }
    }
}
