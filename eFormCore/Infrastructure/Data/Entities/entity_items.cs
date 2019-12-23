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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class entity_items : BaseEntity
    {
        
        public int EntityGroupId { get; set; }

        [StringLength(50)]
        public string EntityItemUid { get; set; }

        public string MicrotingUid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public short? Synced { get; set; }

        public int DisplayIndex { get; set; }
        
        public async Task Create(MicrotingDbContext dbContext)
        {
            
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.entity_items.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.entity_item_versions.Add(MapEntityItemVersions(this));
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            entity_items entityItem = await dbContext.entity_items.FirstOrDefaultAsync(x => x.Id == Id);

            if (entityItem == null)
            {
                throw new NullReferenceException($"Could not find Entity Item with Id: {Id}");
            }

            entityItem.EntityGroupId = EntityGroupId;
            entityItem.MicrotingUid = MicrotingUid;
            entityItem.Name = Name;
            entityItem.Description = Description;
            entityItem.Synced = Synced;
            entityItem.DisplayIndex = DisplayIndex;

            if (dbContext.ChangeTracker.HasChanges())
            {
                entityItem.UpdatedAt = DateTime.Now;
                entityItem.Version += 1;

                dbContext.entity_item_versions.Add(MapEntityItemVersions(entityItem));
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            entity_items entityItem = await dbContext.entity_items.FirstOrDefaultAsync(x => x.Id == Id);

            if (entityItem == null)
            {
                throw new NullReferenceException($"Could not find Entity Item with Id: {Id}");
            }

            entityItem.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                entityItem.UpdatedAt = DateTime.Now;
                entityItem.Version += 1;

                dbContext.entity_item_versions.Add(MapEntityItemVersions(entityItem));
                await dbContext.SaveChangesAsync();
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