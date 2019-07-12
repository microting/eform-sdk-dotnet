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
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Microting.eForm.Infrastructure.Data.Entities
{
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
        public int EntityGroupId { get; set; }

        [StringLength(50)]
        public string EntityItemUid { get; set; }

        public string MicrotingUid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public short? Synced { get; set; }

        public int DisplayIndex { get; set; }

        //public bool migrated_entity_group_id { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
            
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

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

            entityItems.EntityGroupId = EntityGroupId;
            entityItems.MicrotingUid = MicrotingUid;
            entityItems.Name = Name;
            entityItems.Description = Description;
            entityItems.Synced = Synced;
            entityItems.DisplayIndex = DisplayIndex;

            if (dbContext.ChangeTracker.HasChanges())
            {
                entityItems.UpdatedAt = DateTime.Now;
                entityItems.Version += 1;

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

            entityItems.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                entityItems.UpdatedAt = DateTime.Now;
                entityItems.Version += 1;

                dbContext.entity_item_versions.Add(MapEntityItemVersions(entityItems));
                dbContext.SaveChanges();
            }
        }
        
        private entity_item_versions MapEntityItemVersions(entity_items entityItem)
        {
            entity_item_versions entityItemVer = new entity_item_versions();
            entityItemVer.WorkflowState = entityItem.WorkflowState;
            entityItemVer.Version = entityItem.Version;
            entityItemVer.CreatedAt = entityItem.CreatedAt;
            entityItemVer.UpdatedAt = entityItem.UpdatedAt;
            entityItemVer.EntityItemUid = entityItem.EntityItemUid;
            entityItemVer.MicrotingUid = entityItem.MicrotingUid;
            entityItemVer.EntityGroupId = entityItem.EntityGroupId;
            entityItemVer.Name = entityItem.Name;
            entityItemVer.Description = entityItem.Description;
            entityItemVer.Synced = entityItem.Synced;
            entityItemVer.DisplayIndex = entityItem.DisplayIndex;

            entityItemVer.EntityItemsId = entityItem.Id; //<<--

            return entityItemVer;
        }
    }
}