/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Extensions;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class EntityGroup : PnBase
    {
        public string MicrotingUid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Locked { get; set; }

        public bool Editable { get; set; }

        [StringLength(50)] public string Type { get; set; }

        public static async Task<Models.EntityGroup> ReadSorted(MicrotingDbContext dbContext, string entityGroupMUId,
            string sort,
            string nameFilter)
        {
            EntityGroup eG =
                await dbContext.EntityGroups.FirstOrDefaultAsync(x => x.MicrotingUid == entityGroupMUId);

            if (eG == null)
                return null;

            List<Models.EntityItem> lst = new List<Models.EntityItem>();
            Models.EntityGroup rtnEG = new Models.EntityGroup
            {
                Id = eG.Id,
                Name = eG.Name,
                Type = eG.Type,
                MicrotingUUID = eG.MicrotingUid,
                EntityGroupItemLst = lst,
                WorkflowState = eG.WorkflowState,
                Description = eG.Description,
                CreatedAt = eG.CreatedAt,
                UpdatedAt = eG.UpdatedAt,
                Locked = eG.Locked,
                Editable = eG.Editable
            };

            List<EntityItem> eILst = null;

            if (string.IsNullOrEmpty(nameFilter))
            {
                eILst = dbContext.EntityItems.Where(x => x.EntityGroupId == eG.Id
                                                         && x.WorkflowState !=
                                                         Constants.Constants.WorkflowStates.Removed
                                                         && x.WorkflowState != Constants.Constants.WorkflowStates
                                                             .FailedToSync).CustomOrderBy(sort).ToList();
            }
            else
            {
                eILst = dbContext.EntityItems.Where(x => x.EntityGroupId == eG.Id
                                                         && x.WorkflowState !=
                                                         Constants.Constants.WorkflowStates.Removed
                                                         && x.WorkflowState != Constants.Constants.WorkflowStates
                                                             .FailedToSync
                                                         && x.Name.Contains(nameFilter)).CustomOrderBy(sort).ToList();
            }

            if (eILst.Count > 0)
                lst.AddRange(eILst.Select(item => new Models.EntityItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    EntityItemUId = item.EntityItemUid,
                    MicrotingUUID = item.MicrotingUid,
                    WorkflowState = item.WorkflowState,
                    DisplayIndex = item.DisplayIndex
                }));

            return rtnEG;
        }
    }
}