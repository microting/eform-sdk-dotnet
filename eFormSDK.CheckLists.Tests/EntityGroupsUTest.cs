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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.CheckLists.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class EntityGroupsUTest : DbTestFixture
    {
        [Test]
        public async Task EntityGroups_Create_DoesCreate()
        {
            EntityGroup entityGroup = new EntityGroup
            {
                Name = Guid.NewGuid().ToString(),
                Type = Guid.NewGuid().ToString(),
                MicrotingUid = Guid.NewGuid().ToString()
            };

            //Act

            await entityGroup.Create(DbContext).ConfigureAwait(false);

            List<EntityGroup> entityGroups = DbContext.EntityGroups.AsNoTracking().ToList();
            List<EntityGroupVersion> entityGroupVersion = DbContext.EntityGroupVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(entityGroups);
            Assert.NotNull(entityGroupVersion);

            Assert.AreEqual(1, entityGroups.Count());
            Assert.AreEqual(1, entityGroupVersion.Count());

            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroups[0].CreatedAt.ToString());
            Assert.AreEqual(entityGroup.Version, entityGroups[0].Version);
//            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroups[0].UpdatedAt.ToString());
            Assert.AreEqual(entityGroups[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityGroup.Id, entityGroups[0].Id);
            Assert.AreEqual(entityGroup.Name, entityGroups[0].Name);
            Assert.AreEqual(entityGroup.Type, entityGroups[0].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroups[0].MicrotingUid);

            //Versions
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroupVersion[0].CreatedAt.ToString());
            Assert.AreEqual(entityGroup.Version, entityGroupVersion[0].Version);
//            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroupVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(entityGroupVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityGroup.Id, entityGroupVersion[0].Id);
            Assert.AreEqual(entityGroup.Name, entityGroupVersion[0].Name);
            Assert.AreEqual(entityGroup.Type, entityGroupVersion[0].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroupVersion[0].MicrotingUid);
        }

        [Test]
        public async Task EntityGroups_Update_DoesUpdate()
        {
            EntityGroup entityGroup = new EntityGroup
            {
                Name = Guid.NewGuid().ToString(),
                Type = Guid.NewGuid().ToString(),
                MicrotingUid = Guid.NewGuid().ToString()
            };
            await entityGroup.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = entityGroup.UpdatedAt;
            string oldName = entityGroup.Name;
            string oldType = entityGroup.Type;
            string oldMicrotingUid = entityGroup.MicrotingUid;

            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();

            await entityGroup.Update(DbContext).ConfigureAwait(false);

            List<EntityGroup> entityGroups = DbContext.EntityGroups.AsNoTracking().ToList();
            List<EntityGroupVersion> entityGroupVersion = DbContext.EntityGroupVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(entityGroups);
            Assert.NotNull(entityGroupVersion);

            Assert.AreEqual(1, entityGroups.Count());
            Assert.AreEqual(2, entityGroupVersion.Count());

            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroups[0].CreatedAt.ToString());
            Assert.AreEqual(entityGroup.Version, entityGroups[0].Version);
//            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroups[0].UpdatedAt.ToString());
            Assert.AreEqual(entityGroups[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityGroup.Id, entityGroups[0].Id);
            Assert.AreEqual(entityGroup.Name, entityGroups[0].Name);
            Assert.AreEqual(entityGroup.Type, entityGroups[0].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroups[0].MicrotingUid);

            //Old Version
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroupVersion[0].CreatedAt.ToString());
            Assert.AreEqual(1, entityGroupVersion[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), entityGroupVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(entityGroupVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityGroup.Id, entityGroupVersion[0].EntityGroupId);
            Assert.AreEqual(oldName, entityGroupVersion[0].Name);
            Assert.AreEqual(oldType, entityGroupVersion[0].Type);
            Assert.AreEqual(oldMicrotingUid, entityGroupVersion[0].MicrotingUid);

            //New Version
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroupVersion[1].CreatedAt.ToString());
            Assert.AreEqual(2, entityGroupVersion[1].Version);
//            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroupVersion[1].UpdatedAt.ToString());
            Assert.AreEqual(entityGroupVersion[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityGroup.Id, entityGroupVersion[1].EntityGroupId);
            Assert.AreEqual(entityGroup.Name, entityGroupVersion[1].Name);
            Assert.AreEqual(entityGroup.Type, entityGroupVersion[1].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroupVersion[1].MicrotingUid);
        }

        [Test]
        public async Task EntityGroups_Delete_DoesSetWorkflowStateToRemoved()
        {
            EntityGroup entityGroup = new EntityGroup
            {
                Name = Guid.NewGuid().ToString(),
                Type = Guid.NewGuid().ToString(),
                MicrotingUid = Guid.NewGuid().ToString()
            };
            await entityGroup.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = entityGroup.UpdatedAt;

            await entityGroup.Delete(DbContext);

            List<EntityGroup> entityGroups = DbContext.EntityGroups.AsNoTracking().ToList();
            List<EntityGroupVersion> entityGroupVersion = DbContext.EntityGroupVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(entityGroups);
            Assert.NotNull(entityGroupVersion);

            Assert.AreEqual(1, entityGroups.Count());
            Assert.AreEqual(2, entityGroupVersion.Count());

            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroups[0].CreatedAt.ToString());
            Assert.AreEqual(entityGroup.Version, entityGroups[0].Version);
//            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroups[0].UpdatedAt.ToString());
            Assert.AreEqual(entityGroups[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(entityGroup.Id, entityGroups[0].Id);
            Assert.AreEqual(entityGroup.Name, entityGroups[0].Name);
            Assert.AreEqual(entityGroup.Type, entityGroups[0].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroups[0].MicrotingUid);

            //Old Version
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroupVersion[0].CreatedAt.ToString());
            Assert.AreEqual(1, entityGroupVersion[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), entityGroupVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(entityGroupVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityGroup.Id, entityGroupVersion[0].EntityGroupId);
            Assert.AreEqual(entityGroup.Name, entityGroupVersion[0].Name);
            Assert.AreEqual(entityGroup.Type, entityGroupVersion[0].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroupVersion[0].MicrotingUid);

            //New Version
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroupVersion[1].CreatedAt.ToString());
            Assert.AreEqual(2, entityGroupVersion[1].Version);
//            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroupVersion[1].UpdatedAt.ToString());
            Assert.AreEqual(entityGroupVersion[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(entityGroup.Id, entityGroupVersion[1].EntityGroupId);
            Assert.AreEqual(entityGroup.Name, entityGroupVersion[1].Name);
            Assert.AreEqual(entityGroup.Type, entityGroupVersion[1].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroupVersion[1].MicrotingUid);
        }
    }
}