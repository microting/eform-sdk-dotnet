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

            Assert.That(entityGroups, Is.Not.EqualTo(null));
            Assert.That(entityGroupVersion, Is.Not.EqualTo(null));

            Assert.That(entityGroups.Count(), Is.EqualTo(1));
            Assert.That(entityGroupVersion.Count(), Is.EqualTo(1));

            Assert.That(entityGroups[0].CreatedAt.ToString(), Is.EqualTo(entityGroup.CreatedAt.ToString()));
            Assert.That(entityGroups[0].Version, Is.EqualTo(entityGroup.Version));
            //            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroups[0].UpdatedAt.ToString());
            Assert.That(entityGroups[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(entityGroups[0].Id, Is.EqualTo(entityGroup.Id));
            Assert.That(entityGroups[0].Name, Is.EqualTo(entityGroup.Name));
            Assert.That(entityGroups[0].Type, Is.EqualTo(entityGroup.Type));
            Assert.That(entityGroups[0].MicrotingUid, Is.EqualTo(entityGroup.MicrotingUid));

            //Versions
            Assert.That(entityGroupVersion[0].CreatedAt.ToString(), Is.EqualTo(entityGroup.CreatedAt.ToString()));
            Assert.That(entityGroupVersion[0].Version, Is.EqualTo(entityGroup.Version));
            //            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroupVersion[0].UpdatedAt.ToString());
            Assert.That(entityGroupVersion[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(entityGroupVersion[0].Id, Is.EqualTo(entityGroup.Id));
            Assert.That(entityGroupVersion[0].Name, Is.EqualTo(entityGroup.Name));
            Assert.That(entityGroupVersion[0].Type, Is.EqualTo(entityGroup.Type));
            Assert.That(entityGroupVersion[0].MicrotingUid, Is.EqualTo(entityGroup.MicrotingUid));
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

            Assert.That(entityGroups, Is.Not.EqualTo(null));
            Assert.That(entityGroupVersion, Is.Not.EqualTo(null));

            Assert.That(entityGroups.Count(), Is.EqualTo(1));
            Assert.That(entityGroupVersion.Count(), Is.EqualTo(2));

            Assert.That(entityGroups[0].CreatedAt.ToString(), Is.EqualTo(entityGroup.CreatedAt.ToString()));
            Assert.That(entityGroups[0].Version, Is.EqualTo(entityGroup.Version));
            //            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroups[0].UpdatedAt.ToString());
            Assert.That(entityGroups[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(entityGroups[0].Id, Is.EqualTo(entityGroup.Id));
            Assert.That(entityGroups[0].Name, Is.EqualTo(entityGroup.Name));
            Assert.That(entityGroups[0].Type, Is.EqualTo(entityGroup.Type));
            Assert.That(entityGroups[0].MicrotingUid, Is.EqualTo(entityGroup.MicrotingUid));

            //Old Version
            Assert.That(entityGroupVersion[0].CreatedAt.ToString(), Is.EqualTo(entityGroup.CreatedAt.ToString()));
            Assert.That(entityGroupVersion[0].Version, Is.EqualTo(1));
            //            Assert.AreEqual(oldUpdatedAt.ToString(), entityGroupVersion[0].UpdatedAt.ToString());
            Assert.That(entityGroupVersion[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(entityGroupVersion[0].EntityGroupId, Is.EqualTo(entityGroup.Id));
            Assert.That(entityGroupVersion[0].Name, Is.EqualTo(oldName));
            Assert.That(entityGroupVersion[0].Type, Is.EqualTo(oldType));
            Assert.That(entityGroupVersion[0].MicrotingUid, Is.EqualTo(oldMicrotingUid));

            //New Version
            Assert.That(entityGroupVersion[1].CreatedAt.ToString(), Is.EqualTo(entityGroup.CreatedAt.ToString()));
            Assert.That(entityGroupVersion[1].Version, Is.EqualTo(2));
            //            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroupVersion[1].UpdatedAt.ToString());
            Assert.That(entityGroupVersion[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(entityGroupVersion[1].EntityGroupId, Is.EqualTo(entityGroup.Id));
            Assert.That(entityGroupVersion[1].Name, Is.EqualTo(entityGroup.Name));
            Assert.That(entityGroupVersion[1].Type, Is.EqualTo(entityGroup.Type));
            Assert.That(entityGroupVersion[1].MicrotingUid, Is.EqualTo(entityGroup.MicrotingUid));
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

            Assert.That(entityGroups, Is.Not.EqualTo(null));
            Assert.That(entityGroupVersion, Is.Not.EqualTo(null));

            Assert.That(entityGroups.Count(), Is.EqualTo(1));
            Assert.That(entityGroupVersion.Count(), Is.EqualTo(2));

            Assert.That(entityGroups[0].CreatedAt.ToString(), Is.EqualTo(entityGroup.CreatedAt.ToString()));
            Assert.That(entityGroups[0].Version, Is.EqualTo(entityGroup.Version));
            //            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroups[0].UpdatedAt.ToString());
            Assert.That(entityGroups[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
            Assert.That(entityGroups[0].Id, Is.EqualTo(entityGroup.Id));
            Assert.That(entityGroups[0].Name, Is.EqualTo(entityGroup.Name));
            Assert.That(entityGroups[0].Type, Is.EqualTo(entityGroup.Type));
            Assert.That(entityGroups[0].MicrotingUid, Is.EqualTo(entityGroup.MicrotingUid));

            //Old Version
            Assert.That(entityGroupVersion[0].CreatedAt.ToString(), Is.EqualTo(entityGroup.CreatedAt.ToString()));
            Assert.That(entityGroupVersion[0].Version, Is.EqualTo(1));
            //            Assert.AreEqual(oldUpdatedAt.ToString(), entityGroupVersion[0].UpdatedAt.ToString());
            Assert.That(entityGroupVersion[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
            Assert.That(entityGroupVersion[0].EntityGroupId, Is.EqualTo(entityGroup.Id));
            Assert.That(entityGroupVersion[0].Name, Is.EqualTo(entityGroup.Name));
            Assert.That(entityGroupVersion[0].Type, Is.EqualTo(entityGroup.Type));
            Assert.That(entityGroupVersion[0].MicrotingUid, Is.EqualTo(entityGroup.MicrotingUid));

            //New Version
            Assert.That(entityGroupVersion[1].CreatedAt.ToString(), Is.EqualTo(entityGroup.CreatedAt.ToString()));
            Assert.That(entityGroupVersion[1].Version, Is.EqualTo(2));
            //            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroupVersion[1].UpdatedAt.ToString());
            Assert.That(entityGroupVersion[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
            Assert.That(entityGroupVersion[1].EntityGroupId, Is.EqualTo(entityGroup.Id));
            Assert.That(entityGroupVersion[1].Name, Is.EqualTo(entityGroup.Name));
            Assert.That(entityGroupVersion[1].Type, Is.EqualTo(entityGroup.Type));
            Assert.That(entityGroupVersion[1].MicrotingUid, Is.EqualTo(entityGroup.MicrotingUid));
        }
    }
}