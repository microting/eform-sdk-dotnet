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
using Microting.eForm.Infrastructure.Extensions;
using Microting.eForm.Infrastructure.Models;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class entity_groups : BaseEntity
    {
        public string MicrotingUid { get; set; }

        public string Name { get; set; }

        [StringLength(50)] public string Type { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {

            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.entity_groups.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.entity_group_versions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            entity_groups entityGroups = await dbContext.entity_groups.FirstOrDefaultAsync(x => x.Id == Id);

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

                dbContext.entity_group_versions.Add(MapVersions(entityGroups));
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {

            entity_groups entityGroups = await dbContext.entity_groups.FirstOrDefaultAsync(x => x.Id == Id);

            if (entityGroups == null)
            {
                throw new NullReferenceException($"Could not find Entity Group with Id: {Id}");
            }

            entityGroups.WorkflowState = Constants.Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                entityGroups.UpdatedAt = DateTime.Now;
                entityGroups.Version += 1;

                dbContext.entity_group_versions.Add(MapVersions(entityGroups));
                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task<EntityGroup> ReadSorted(MicrotingDbContext dbContext, string entityGroupMUId, string sort,
            string nameFilter)
        {
            entity_groups eG =
                await dbContext.entity_groups.SingleOrDefaultAsync(x => x.MicrotingUid == entityGroupMUId);

            if (eG == null)
                return null;

            List<EntityItem> lst = new List<EntityItem>();
            EntityGroup rtnEG = new EntityGroup
            {
                Id = eG.Id,
                Name = eG.Name,
                Type = eG.Type,
                MicrotingUUID = eG.MicrotingUid,
                EntityGroupItemLst = lst,
                WorkflowState = eG.WorkflowState,
                CreatedAt = eG.CreatedAt,
                UpdatedAt = eG.UpdatedAt
            };

            List<entity_items> eILst = null;

            if (string.IsNullOrEmpty(nameFilter))
            {
                eILst = dbContext.entity_items.Where(x => x.EntityGroupId == eG.Id
                                                          && x.WorkflowState !=
                                                          Constants.Constants.WorkflowStates.Removed
                                                          && x.WorkflowState != Constants.Constants.WorkflowStates
                                                              .FailedToSync).CustomOrderBy(sort).ToList();
            }
            else
            {
                eILst = dbContext.entity_items.Where(x => x.EntityGroupId == eG.Id
                                                          && x.WorkflowState !=
                                                          Constants.Constants.WorkflowStates.Removed
                                                          && x.WorkflowState != Constants.Constants.WorkflowStates
                                                              .FailedToSync
                                                          && x.Name.Contains(nameFilter)).CustomOrderBy(sort).ToList();
            }

            if (eILst.Count > 0)
                foreach (entity_items item in eILst)
                {
                    EntityItem eI = new EntityItem
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        EntityItemUId = item.EntityItemUid,
                        MicrotingUUID = item.MicrotingUid,
                        WorkflowState = item.WorkflowState,
                        DisplayIndex = item.DisplayIndex
                    };
                    lst.Add(eI);
                }

            return rtnEG;
        } 
        
        private entity_group_versions MapVersions(entity_groups entityGroup)
        {
            return new entity_group_versions
            {
                CreatedAt = entityGroup.CreatedAt,
                MicrotingUid = entityGroup.MicrotingUid,
                Name = entityGroup.Name,
                Type = entityGroup.Type,
                UpdatedAt = entityGroup.UpdatedAt,
                Version = entityGroup.Version,
                WorkflowState = entityGroup.WorkflowState,
                EntityGroupId = entityGroup.Id
            };
        }
    }
}
