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

    public partial class entity_groups : BaseEntity
    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }
//
//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public int? version { get; set; }
//
//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        public string MicrotingUid { get; set; }

        public string Name { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
           
            WorkflowState = Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.entity_groups.Add(this);
            dbContext.SaveChanges();

            dbContext.entity_group_versions.Add(MapEntityGroupVersions(this));
            dbContext.SaveChanges();

        }
        
        public void Update(MicrotingDbAnySql dbContext)
        {
            entity_groups entityGroups = dbContext.entity_groups.FirstOrDefault(x => x.Id == Id);

            if (entityGroups == null)
            {
                throw new NullReferenceException($"Could not find Entity Group with Id: {Id}");
            }

            entityGroups.MicrotingUid = MicrotingUid;
            entityGroups.Name = Name;
            entityGroups.Type = Type;

            if (dbContext.ChangeTracker.HasChanges())
            {
                entityGroups.UpdatedAt = DateTime.Now;
                entityGroups.Version += 1;

                dbContext.entity_group_versions.Add(MapEntityGroupVersions(entityGroups));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            
            entity_groups entityGroups = dbContext.entity_groups.FirstOrDefault(x => x.Id == Id);

            if (entityGroups == null)
            {
                throw new NullReferenceException($"Could not find Entity Group with Id: {Id}");
            }

            entityGroups.WorkflowState = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                entityGroups.UpdatedAt = DateTime.Now;
                entityGroups.Version += 1;

                dbContext.entity_group_versions.Add(MapEntityGroupVersions(entityGroups));
                dbContext.SaveChanges();
            }
            
        }
        
        
        
        private entity_group_versions MapEntityGroupVersions(entity_groups entityGroup)
        {
            entity_group_versions entityGroupVer = new entity_group_versions();
            entityGroupVer.CreatedAt = entityGroup.CreatedAt;
            entityGroupVer.EntityGroupId = entityGroup.Id;
            entityGroupVer.MicrotingUid = entityGroup.MicrotingUid;
            entityGroupVer.Name = entityGroup.Name;
            entityGroupVer.Type = entityGroup.Type;
            entityGroupVer.UpdatedAt = entityGroup.UpdatedAt;
            entityGroupVer.Version = entityGroup.Version;
            entityGroupVer.WorkflowState = entityGroup.WorkflowState;

            entityGroupVer.EntityGroupId = entityGroup.Id; //<<--

            return entityGroupVer;
        }
    }
}
