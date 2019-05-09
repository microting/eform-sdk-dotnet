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

    public partial class entity_items : BaseEntity
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
        
        // TODO! Change this to be int and create migration to handle the move.
        public int entity_group_id { get; set; }

        [StringLength(50)]
        public string entity_item_uid { get; set; }

        public string microting_uid { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public short? synced { get; set; }

        public int display_index { get; set; }

        //public bool migrated_entity_group_id { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
            
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.entity_items.Add(this);
            dbContext.SaveChanges();

            dbContext.entity_item_versions.Add(MapEntityItemVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            entity_items entityItems = dbContext.entity_items.FirstOrDefault(x => x.Id == Id);

            if (entityItems == null)
            {
                throw new NullReferenceException($"Could not find Entity Item with Id: {Id}");
            }

            entityItems.entity_group_id = entity_group_id;
            entityItems.microting_uid = microting_uid;
            entityItems.name = name;
            entityItems.description = description;
            entityItems.synced = synced;
            entityItems.display_index = display_index;

            if (dbContext.ChangeTracker.HasChanges())
            {
                entityItems.updated_at = DateTime.Now;
                entityItems.version += 1;

                dbContext.entity_item_versions.Add(MapEntityItemVersions(entityItems));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            entity_items entityItems = dbContext.entity_items.FirstOrDefault(x => x.Id == Id);

            if (entityItems == null)
            {
                throw new NullReferenceException($"Could not find Entity Item with Id: {Id}");
            }

            entityItems.workflow_state = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                entityItems.updated_at = DateTime.Now;
                entityItems.version += 1;

                dbContext.entity_item_versions.Add(MapEntityItemVersions(entityItems));
                dbContext.SaveChanges();
            }
        }
        
        private entity_item_versions MapEntityItemVersions(entity_items entityItem)
        {
            entity_item_versions entityItemVer = new entity_item_versions();
            entityItemVer.workflow_state = entityItem.workflow_state;
            entityItemVer.version = entityItem.version;
            entityItemVer.created_at = entityItem.created_at;
            entityItemVer.updated_at = entityItem.updated_at;
            entityItemVer.entity_item_uid = entityItem.entity_item_uid;
            entityItemVer.microting_uid = entityItem.microting_uid;
            entityItemVer.entity_group_id = entityItem.entity_group_id;
            entityItemVer.name = entityItem.name;
            entityItemVer.description = entityItem.description;
            entityItemVer.synced = entityItem.synced;
            entityItemVer.display_index = entityItem.display_index;

            entityItemVer.entity_items_id = entityItem.Id; //<<--

            return entityItemVer;
        }
    }
}