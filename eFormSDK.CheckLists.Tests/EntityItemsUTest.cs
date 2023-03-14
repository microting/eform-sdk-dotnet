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
    public class EntityItemsUTest : DbTestFixture
    {
        [Test]
        public async Task EntityItems_Create_DoesCreate()
        {
            //Arrange

            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;

            Random rnd = new Random();

            EntityGroup entityGroup = new EntityGroup
            {
                Name = Guid.NewGuid().ToString(),
                Type = Guid.NewGuid().ToString(),
                MicrotingUid = Guid.NewGuid().ToString()
            };
            await entityGroup.Create(DbContext).ConfigureAwait(false);

            EntityItem entityItem = new EntityItem
            {
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Synced = (short)rnd.Next(shortMinValue, shortmaxValue),
                DisplayIndex = rnd.Next(1, 255),
                MicrotingUid = Guid.NewGuid().ToString(),
                EntityItemUid = Guid.NewGuid().ToString(),
                EntityGroupId = entityGroup.Id
            };

            //Act

            await entityItem.Create(DbContext).ConfigureAwait(false);

            List<EntityItem> entityItems = DbContext.EntityItems.AsNoTracking().ToList();
            List<EntityItemVersion> entityItemVersion = DbContext.EntityItemVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(entityItems);
            Assert.NotNull(entityItemVersion);

            Assert.AreEqual(1, entityItems.Count());
            Assert.AreEqual(1, entityItemVersion.Count());

            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItems[0].CreatedAt.ToString());
            Assert.AreEqual(entityItem.Version, entityItems[0].Version);
//            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItems[0].UpdatedAt.ToString());
            Assert.AreEqual(entityItems[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityItem.Id, entityItems[0].Id);
            Assert.AreEqual(entityItem.Name, entityItems[0].Name);
            Assert.AreEqual(entityItem.Description, entityItems[0].Description);
            Assert.AreEqual(entityItem.Synced, entityItems[0].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItems[0].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItems[0].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItems[0].MicrotingUid);
            Assert.AreEqual(entityItem.EntityGroupId, entityGroup.Id);

            //Versions
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItemVersion[0].CreatedAt.ToString());
            Assert.AreEqual(entityItem.Version, entityItemVersion[0].Version);
//            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItemVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(entityItemVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityItem.Id, entityItemVersion[0].EntityItemId);
            Assert.AreEqual(entityItem.Name, entityItemVersion[0].Name);
            Assert.AreEqual(entityItem.Description, entityItemVersion[0].Description);
            Assert.AreEqual(entityItem.Synced, entityItemVersion[0].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItemVersion[0].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItemVersion[0].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItemVersion[0].MicrotingUid);
            Assert.AreEqual(entityGroup.Id, entityItemVersion[0].EntityGroupId);
        }

        [Test]
        public async Task EntityItems_Update_DoesUpdate()
        {
            //Arrange

            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;

            Random rnd = new Random();

            EntityGroup entityGroup = new EntityGroup
            {
                Name = Guid.NewGuid().ToString(),
                Type = Guid.NewGuid().ToString(),
                MicrotingUid = Guid.NewGuid().ToString()
            };

            EntityItem entityItem = new EntityItem
            {
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Synced = (short)rnd.Next(shortMinValue, shortmaxValue),
                DisplayIndex = rnd.Next(1, 255),
                MicrotingUid = Guid.NewGuid().ToString(),
                EntityItemUid = Guid.NewGuid().ToString(),
                EntityGroupId = entityGroup.Id
            };
            await entityItem.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = entityItem.UpdatedAt;
            string oldDescription = entityItem.Description;
            string oldName = entityItem.Name;
            short? oldSynced = entityItem.Synced;
            int oldDisplayIndex = entityItem.DisplayIndex;
            string oldMicrotingUid = entityItem.MicrotingUid;
            string oldEntityItemUid = entityItem.EntityItemUid;

            entityItem.Description = Guid.NewGuid().ToString();
            entityItem.Name = Guid.NewGuid().ToString();
            entityItem.Synced = (short)rnd.Next(shortMinValue, shortmaxValue);
            entityItem.DisplayIndex = rnd.Next(1, 255);
            entityItem.MicrotingUid = Guid.NewGuid().ToString();
            entityItem.EntityItemUid = Guid.NewGuid().ToString();

            await entityItem.Update(DbContext).ConfigureAwait(false);

            List<EntityItem> entityItems = DbContext.EntityItems.AsNoTracking().ToList();
            List<EntityItemVersion> entityItemVersion = DbContext.EntityItemVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(entityItems);
            Assert.NotNull(entityItemVersion);

            Assert.AreEqual(1, entityItems.Count());
            Assert.AreEqual(2, entityItemVersion.Count());

            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItems[0].CreatedAt.ToString());
            Assert.AreEqual(entityItem.Version, entityItems[0].Version);
//            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItems[0].UpdatedAt.ToString());
            Assert.AreEqual(entityItems[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityItem.Id, entityItems[0].Id);
            Assert.AreEqual(entityItem.Name, entityItems[0].Name);
            Assert.AreEqual(entityItem.Description, entityItems[0].Description);
            Assert.AreEqual(entityItem.Synced, entityItems[0].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItems[0].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItems[0].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItems[0].MicrotingUid);
            Assert.AreEqual(entityItem.EntityGroupId, entityGroup.Id);

            //Old Version
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItemVersion[0].CreatedAt.ToString());
            Assert.AreEqual(1, entityItemVersion[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), entityItemVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(entityItemVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityItem.Id, entityItemVersion[0].EntityItemId);
            Assert.AreEqual(oldName, entityItemVersion[0].Name);
            Assert.AreEqual(oldDescription, entityItemVersion[0].Description);
            Assert.AreEqual(oldSynced, entityItemVersion[0].Synced);
            Assert.AreEqual(oldDisplayIndex, entityItemVersion[0].DisplayIndex);
            Assert.AreEqual(oldEntityItemUid, entityItemVersion[0].EntityItemUid);
            Assert.AreEqual(oldMicrotingUid, entityItemVersion[0].MicrotingUid);
            Assert.AreEqual(entityGroup.Id, entityItemVersion[0].EntityGroupId);

            //New Version
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItemVersion[1].CreatedAt.ToString());
            Assert.AreEqual(2, entityItemVersion[1].Version);
//            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItemVersion[1].UpdatedAt.ToString());
            Assert.AreEqual(entityItemVersion[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityItem.Id, entityItemVersion[1].EntityItemId);
            Assert.AreEqual(entityItem.Name, entityItemVersion[1].Name);
            Assert.AreEqual(entityItem.Description, entityItemVersion[1].Description);
            Assert.AreEqual(entityItem.Synced, entityItemVersion[1].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItemVersion[1].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItemVersion[1].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItemVersion[1].MicrotingUid);
            Assert.AreEqual(entityGroup.Id, entityItemVersion[1].EntityGroupId);
        }

        [Test]
        public async Task EntityItems_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange

            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;

            Random rnd = new Random();

            EntityGroup entityGroup = new EntityGroup
            {
                Name = Guid.NewGuid().ToString(),
                Type = Guid.NewGuid().ToString(),
                MicrotingUid = Guid.NewGuid().ToString()
            };

            EntityItem entityItem = new EntityItem
            {
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Synced = (short)rnd.Next(shortMinValue, shortmaxValue),
                DisplayIndex = rnd.Next(1, 255),
                MicrotingUid = Guid.NewGuid().ToString(),
                EntityItemUid = Guid.NewGuid().ToString(),
                EntityGroupId = entityGroup.Id
            };
            await entityItem.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = entityItem.UpdatedAt;

            await entityItem.Delete(DbContext);

            List<EntityItem> entityItems = DbContext.EntityItems.AsNoTracking().ToList();
            List<EntityItemVersion> entityItemVersion = DbContext.EntityItemVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(entityItems);
            Assert.NotNull(entityItemVersion);

            Assert.AreEqual(1, entityItems.Count());
            Assert.AreEqual(2, entityItemVersion.Count());

            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItems[0].CreatedAt.ToString());
            Assert.AreEqual(entityItem.Version, entityItems[0].Version);
//            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItems[0].UpdatedAt.ToString());
            Assert.AreEqual(entityItems[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(entityItem.Id, entityItems[0].Id);
            Assert.AreEqual(entityItem.Name, entityItems[0].Name);
            Assert.AreEqual(entityItem.Description, entityItems[0].Description);
            Assert.AreEqual(entityItem.Synced, entityItems[0].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItems[0].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItems[0].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItems[0].MicrotingUid);
            Assert.AreEqual(entityItem.EntityGroupId, entityGroup.Id);

            //Old Version
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItemVersion[0].CreatedAt.ToString());
            Assert.AreEqual(1, entityItemVersion[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), entityItemVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(entityItemVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityItem.Id, entityItemVersion[0].EntityItemId);
            Assert.AreEqual(entityItem.Name, entityItemVersion[0].Name);
            Assert.AreEqual(entityItem.Description, entityItemVersion[0].Description);
            Assert.AreEqual(entityItem.Synced, entityItemVersion[0].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItemVersion[0].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItemVersion[0].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItemVersion[0].MicrotingUid);
            Assert.AreEqual(entityGroup.Id, entityItemVersion[0].EntityGroupId);

            //New Version
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItemVersion[1].CreatedAt.ToString());
            Assert.AreEqual(2, entityItemVersion[1].Version);
//            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItemVersion[1].UpdatedAt.ToString());
            Assert.AreEqual(entityItemVersion[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(entityItem.Id, entityItemVersion[1].EntityItemId);
            Assert.AreEqual(entityItem.Name, entityItemVersion[1].Name);
            Assert.AreEqual(entityItem.Description, entityItemVersion[1].Description);
            Assert.AreEqual(entityItem.Synced, entityItemVersion[1].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItemVersion[1].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItemVersion[1].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItemVersion[1].MicrotingUid);
            Assert.AreEqual(entityGroup.Id, entityItemVersion[1].EntityGroupId);
        }
    }
}