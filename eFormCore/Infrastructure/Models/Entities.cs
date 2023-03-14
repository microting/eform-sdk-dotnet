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

using System;
using System.Collections.Generic;

namespace Microting.eForm.Infrastructure.Models
{
    #region EntityGroup

    public class EntityGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string MicrotingUUID { get; set; }
        public List<EntityItem> EntityGroupItemLst { get; set; }
        public string WorkflowState { get; set; }
        public string Description { get; set; }
        public bool Locked { get; set; }
        public bool Editable { get; set; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public static implicit operator Microting.eForm.Infrastructure.Data.Entities.EntityGroup(
            EntityGroup entityGroup)
        {
            return new Data.Entities.EntityGroup
            {
                Id = entityGroup.Id,
                Name = entityGroup.Name,
                Description = entityGroup.Description,
                MicrotingUid = entityGroup.MicrotingUUID,
                WorkflowState = entityGroup.WorkflowState,
                Locked = entityGroup.Locked,
                Editable = entityGroup.Editable
            };
        }
    }

    #endregion

    #region EntityGroupList

    public class EntityGroupList
    {
        public EntityGroupList()
        {
        }

        public EntityGroupList(int numOfElements, int pageNum, List<EntityGroup> entityGroupList)
        {
            NumOfElements = numOfElements;
            PageNum = pageNum;
            EntityGroups = entityGroupList;
        }

        public int NumOfElements { get; }
        public int PageNum { get; }
        public List<EntityGroup> EntityGroups { get; }
    }

    #endregion

    #region EntityItem

    public class EntityItem
    {
        public EntityItem()
        {
            Name = "";
            Description = "";
            EntityItemUId = "";
            MicrotingUUID = "";
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EntityItemUId { get; set; }
        public string WorkflowState { get; set; }
        public string MicrotingUUID { get; set; }
        public int DisplayIndex { get; set; }
        public int EntityItemGroupId { get; set; }
    }

    #endregion
}