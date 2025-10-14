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

namespace eFormSDK.CheckLists.Tests;

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

        Assert.That(entityItems, Is.Not.EqualTo(null));
        Assert.That(entityItemVersion, Is.Not.EqualTo(null));

        Assert.That(entityItems.Count(), Is.EqualTo(1));
        Assert.That(entityItemVersion.Count(), Is.EqualTo(1));

        Assert.That(entityItems[0].CreatedAt.ToString(), Is.EqualTo(entityItem.CreatedAt.ToString()));
        Assert.That(entityItems[0].Version, Is.EqualTo(entityItem.Version));
        //            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItems[0].UpdatedAt.ToString());
        Assert.That(entityItems[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(entityItems[0].Id, Is.EqualTo(entityItem.Id));
        Assert.That(entityItems[0].Name, Is.EqualTo(entityItem.Name));
        Assert.That(entityItems[0].Description, Is.EqualTo(entityItem.Description));
        Assert.That(entityItems[0].Synced, Is.EqualTo(entityItem.Synced));
        Assert.That(entityItems[0].DisplayIndex, Is.EqualTo(entityItem.DisplayIndex));
        Assert.That(entityItems[0].EntityItemUid, Is.EqualTo(entityItem.EntityItemUid));
        Assert.That(entityItems[0].MicrotingUid, Is.EqualTo(entityItem.MicrotingUid));
        Assert.That(entityGroup.Id, Is.EqualTo(entityItem.EntityGroupId));

        //Versions
        Assert.That(entityItemVersion[0].CreatedAt.ToString(), Is.EqualTo(entityItem.CreatedAt.ToString()));
        Assert.That(entityItemVersion[0].Version, Is.EqualTo(entityItem.Version));
        //            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItemVersion[0].UpdatedAt.ToString());
        Assert.That(entityItemVersion[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(entityItemVersion[0].EntityItemId, Is.EqualTo(entityItem.Id));
        Assert.That(entityItemVersion[0].Name, Is.EqualTo(entityItem.Name));
        Assert.That(entityItemVersion[0].Description, Is.EqualTo(entityItem.Description));
        Assert.That(entityItemVersion[0].Synced, Is.EqualTo(entityItem.Synced));
        Assert.That(entityItemVersion[0].DisplayIndex, Is.EqualTo(entityItem.DisplayIndex));
        Assert.That(entityItemVersion[0].EntityItemUid, Is.EqualTo(entityItem.EntityItemUid));
        Assert.That(entityItemVersion[0].MicrotingUid, Is.EqualTo(entityItem.MicrotingUid));
        Assert.That(entityItemVersion[0].EntityGroupId, Is.EqualTo(entityGroup.Id));
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

        Assert.That(entityItems, Is.Not.EqualTo(null));
        Assert.That(entityItemVersion, Is.Not.EqualTo(null));

        Assert.That(entityItems.Count(), Is.EqualTo(1));
        Assert.That(entityItemVersion.Count(), Is.EqualTo(2));

        Assert.That(entityItems[0].CreatedAt.ToString(), Is.EqualTo(entityItem.CreatedAt.ToString()));
        Assert.That(entityItems[0].Version, Is.EqualTo(entityItem.Version));
        //            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItems[0].UpdatedAt.ToString());
        Assert.That(entityItems[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(entityItems[0].Id, Is.EqualTo(entityItem.Id));
        Assert.That(entityItems[0].Name, Is.EqualTo(entityItem.Name));
        Assert.That(entityItems[0].Description, Is.EqualTo(entityItem.Description));
        Assert.That(entityItems[0].Synced, Is.EqualTo(entityItem.Synced));
        Assert.That(entityItems[0].DisplayIndex, Is.EqualTo(entityItem.DisplayIndex));
        Assert.That(entityItems[0].EntityItemUid, Is.EqualTo(entityItem.EntityItemUid));
        Assert.That(entityItems[0].MicrotingUid, Is.EqualTo(entityItem.MicrotingUid));
        Assert.That(entityGroup.Id, Is.EqualTo(entityItem.EntityGroupId));

        //Old Version
        Assert.That(entityItemVersion[0].CreatedAt.ToString(), Is.EqualTo(entityItem.CreatedAt.ToString()));
        Assert.That(entityItemVersion[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), entityItemVersion[0].UpdatedAt.ToString());
        Assert.That(entityItemVersion[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(entityItemVersion[0].EntityItemId, Is.EqualTo(entityItem.Id));
        Assert.That(entityItemVersion[0].Name, Is.EqualTo(oldName));
        Assert.That(entityItemVersion[0].Description, Is.EqualTo(oldDescription));
        Assert.That(entityItemVersion[0].Synced, Is.EqualTo(oldSynced));
        Assert.That(entityItemVersion[0].DisplayIndex, Is.EqualTo(oldDisplayIndex));
        Assert.That(entityItemVersion[0].EntityItemUid, Is.EqualTo(oldEntityItemUid));
        Assert.That(entityItemVersion[0].MicrotingUid, Is.EqualTo(oldMicrotingUid));
        Assert.That(entityItemVersion[0].EntityGroupId, Is.EqualTo(entityGroup.Id));

        //New Version
        Assert.That(entityItemVersion[1].CreatedAt.ToString(), Is.EqualTo(entityItem.CreatedAt.ToString()));
        Assert.That(entityItemVersion[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItemVersion[1].UpdatedAt.ToString());
        Assert.That(entityItemVersion[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(entityItemVersion[1].EntityItemId, Is.EqualTo(entityItem.Id));
        Assert.That(entityItemVersion[1].Name, Is.EqualTo(entityItem.Name));
        Assert.That(entityItemVersion[1].Description, Is.EqualTo(entityItem.Description));
        Assert.That(entityItemVersion[1].Synced, Is.EqualTo(entityItem.Synced));
        Assert.That(entityItemVersion[1].DisplayIndex, Is.EqualTo(entityItem.DisplayIndex));
        Assert.That(entityItemVersion[1].EntityItemUid, Is.EqualTo(entityItem.EntityItemUid));
        Assert.That(entityItemVersion[1].MicrotingUid, Is.EqualTo(entityItem.MicrotingUid));
        Assert.That(entityItemVersion[1].EntityGroupId, Is.EqualTo(entityGroup.Id));
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

        Assert.That(entityItems, Is.Not.EqualTo(null));
        Assert.That(entityItemVersion, Is.Not.EqualTo(null));

        Assert.That(entityItems.Count(), Is.EqualTo(1));
        Assert.That(entityItemVersion.Count(), Is.EqualTo(2));

        Assert.That(entityItems[0].CreatedAt.ToString(), Is.EqualTo(entityItem.CreatedAt.ToString()));
        Assert.That(entityItems[0].Version, Is.EqualTo(entityItem.Version));
        //            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItems[0].UpdatedAt.ToString());
        Assert.That(entityItems[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(entityItems[0].Id, Is.EqualTo(entityItem.Id));
        Assert.That(entityItems[0].Name, Is.EqualTo(entityItem.Name));
        Assert.That(entityItems[0].Description, Is.EqualTo(entityItem.Description));
        Assert.That(entityItems[0].Synced, Is.EqualTo(entityItem.Synced));
        Assert.That(entityItems[0].DisplayIndex, Is.EqualTo(entityItem.DisplayIndex));
        Assert.That(entityItems[0].EntityItemUid, Is.EqualTo(entityItem.EntityItemUid));
        Assert.That(entityItems[0].MicrotingUid, Is.EqualTo(entityItem.MicrotingUid));
        Assert.That(entityGroup.Id, Is.EqualTo(entityItem.EntityGroupId));

        //Old Version
        Assert.That(entityItemVersion[0].CreatedAt.ToString(), Is.EqualTo(entityItem.CreatedAt.ToString()));
        Assert.That(entityItemVersion[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), entityItemVersion[0].UpdatedAt.ToString());
        Assert.That(entityItemVersion[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(entityItemVersion[0].EntityItemId, Is.EqualTo(entityItem.Id));
        Assert.That(entityItemVersion[0].Name, Is.EqualTo(entityItem.Name));
        Assert.That(entityItemVersion[0].Description, Is.EqualTo(entityItem.Description));
        Assert.That(entityItemVersion[0].Synced, Is.EqualTo(entityItem.Synced));
        Assert.That(entityItemVersion[0].DisplayIndex, Is.EqualTo(entityItem.DisplayIndex));
        Assert.That(entityItemVersion[0].EntityItemUid, Is.EqualTo(entityItem.EntityItemUid));
        Assert.That(entityItemVersion[0].MicrotingUid, Is.EqualTo(entityItem.MicrotingUid));
        Assert.That(entityItemVersion[0].EntityGroupId, Is.EqualTo(entityGroup.Id));

        //New Version
        Assert.That(entityItemVersion[1].CreatedAt.ToString(), Is.EqualTo(entityItem.CreatedAt.ToString()));
        Assert.That(entityItemVersion[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItemVersion[1].UpdatedAt.ToString());
        Assert.That(entityItemVersion[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(entityItemVersion[1].EntityItemId, Is.EqualTo(entityItem.Id));
        Assert.That(entityItemVersion[1].Name, Is.EqualTo(entityItem.Name));
        Assert.That(entityItemVersion[1].Description, Is.EqualTo(entityItem.Description));
        Assert.That(entityItemVersion[1].Synced, Is.EqualTo(entityItem.Synced));
        Assert.That(entityItemVersion[1].DisplayIndex, Is.EqualTo(entityItem.DisplayIndex));
        Assert.That(entityItemVersion[1].EntityItemUid, Is.EqualTo(entityItem.EntityItemUid));
        Assert.That(entityItemVersion[1].MicrotingUid, Is.EqualTo(entityItem.MicrotingUid));
        Assert.That(entityItemVersion[1].EntityGroupId, Is.EqualTo(entityGroup.Id));
    }
}