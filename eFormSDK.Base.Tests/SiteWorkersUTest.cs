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

namespace eFormSDK.Base.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SiteWorkersUTest : DbTestFixture
    {
        [Test]
        public async Task SiteWorkers_Create_DoesCreate()
        {
            // Arrange

            Random rnd = new Random();

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Worker worker = new Worker
            {
                Email = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await worker.Create(DbContext).ConfigureAwait(false);

            SiteWorker siteWorker = new SiteWorker
            {
                MicrotingUid = rnd.Next(1, 255),
                SiteId = site.Id,
                WorkerId = worker.Id
            };

            //Act

            await siteWorker.Create(DbContext).ConfigureAwait(false);

            List<SiteWorker> siteWorkers = DbContext.SiteWorkers.AsNoTracking().ToList();
            List<SiteWorkerVersion> siteWorkerVersions = DbContext.SiteWorkerVersions.AsNoTracking().ToList();

            Assert.NotNull(siteWorkers);
            Assert.NotNull(siteWorkerVersions);

            Assert.AreEqual(1, siteWorkers.Count());
            Assert.AreEqual(1, siteWorkerVersions.Count());

            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkers[0].CreatedAt.ToString());
            Assert.AreEqual(siteWorker.Version, siteWorkers[0].Version);
//            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkers[0].UpdatedAt.ToString());
            Assert.AreEqual(siteWorkers[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteWorker.Id, siteWorkers[0].Id);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkers[0].MicrotingUid);
            Assert.AreEqual(siteWorker.SiteId, site.Id);
            Assert.AreEqual(siteWorker.WorkerId, worker.Id);

            //Versions

            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkerVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, siteWorkerVersions[0].Version);
//            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkerVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(siteWorkerVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteWorker.Id, siteWorkerVersions[0].SiteWorkerId);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkerVersions[0].MicrotingUid);
            Assert.AreEqual(site.Id, siteWorkerVersions[0].SiteId);
            Assert.AreEqual(worker.Id, siteWorkerVersions[0].WorkerId);
        }

        [Test]
        public async Task SiteWorkers_Update_DoesUpdate()
        {
            // Arrange

            Random rnd = new Random();

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Worker worker = new Worker
            {
                Email = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await worker.Create(DbContext).ConfigureAwait(false);

            SiteWorker siteWorker = new SiteWorker
            {
                MicrotingUid = rnd.Next(1, 255),
                SiteId = site.Id,
                WorkerId = worker.Id
            };
            await siteWorker.Create(DbContext).ConfigureAwait(false);


            //Act

            DateTime? oldUpdatedAt = siteWorker.UpdatedAt;
            int? oldMicroTingUid = siteWorker.MicrotingUid;

            siteWorker.MicrotingUid = rnd.Next(1, 255);

            await siteWorker.Update(DbContext).ConfigureAwait(false);


            List<SiteWorker> siteWorkers = DbContext.SiteWorkers.AsNoTracking().ToList();
            List<SiteWorkerVersion> siteWorkerVersions = DbContext.SiteWorkerVersions.AsNoTracking().ToList();

            Assert.NotNull(siteWorkers);
            Assert.NotNull(siteWorkerVersions);

            Assert.AreEqual(1, siteWorkers.Count());
            Assert.AreEqual(2, siteWorkerVersions.Count());

            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkers[0].CreatedAt.ToString());
            Assert.AreEqual(siteWorker.Version, siteWorkers[0].Version);
//            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkers[0].UpdatedAt.ToString());
            Assert.AreEqual(siteWorkers[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteWorker.Id, siteWorkers[0].Id);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkers[0].MicrotingUid);
            Assert.AreEqual(siteWorker.SiteId, site.Id);
            Assert.AreEqual(siteWorker.WorkerId, worker.Id);

            //Old Version

            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkerVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, siteWorkerVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), siteWorkerVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(siteWorkerVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteWorker.Id, siteWorkerVersions[0].SiteWorkerId);
            Assert.AreEqual(oldMicroTingUid, siteWorkerVersions[0].MicrotingUid);
            Assert.AreEqual(site.Id, siteWorkerVersions[0].SiteId);
            Assert.AreEqual(worker.Id, siteWorkerVersions[0].WorkerId);

            //New Version

            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkerVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, siteWorkerVersions[1].Version);
//            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkerVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(siteWorkerVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteWorker.Id, siteWorkerVersions[1].SiteWorkerId);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkerVersions[1].MicrotingUid);
            Assert.AreEqual(site.Id, siteWorkerVersions[1].SiteId);
            Assert.AreEqual(worker.Id, siteWorkerVersions[1].WorkerId);
        }

        [Test]
        public async Task SiteWorkers_Delete_DoesSetWorkflowStateToRemoved()
        {
            // Arrange

            Random rnd = new Random();

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Worker worker = new Worker
            {
                Email = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await worker.Create(DbContext).ConfigureAwait(false);

            SiteWorker siteWorker = new SiteWorker
            {
                MicrotingUid = rnd.Next(1, 255),
                SiteId = site.Id,
                WorkerId = worker.Id
            };
            await siteWorker.Create(DbContext).ConfigureAwait(false);


            //Act

            DateTime? oldUpdatedAt = siteWorker.UpdatedAt;

            await siteWorker.Delete(DbContext);


            List<SiteWorker> siteWorkers = DbContext.SiteWorkers.AsNoTracking().ToList();
            List<SiteWorkerVersion> siteWorkerVersions = DbContext.SiteWorkerVersions.AsNoTracking().ToList();

            Assert.NotNull(siteWorkers);
            Assert.NotNull(siteWorkerVersions);

            Assert.AreEqual(1, siteWorkers.Count());
            Assert.AreEqual(2, siteWorkerVersions.Count());

            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkers[0].CreatedAt.ToString());
            Assert.AreEqual(siteWorker.Version, siteWorkers[0].Version);
//            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkers[0].UpdatedAt.ToString());
            Assert.AreEqual(siteWorkers[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(siteWorker.Id, siteWorkers[0].Id);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkers[0].MicrotingUid);
            Assert.AreEqual(siteWorker.SiteId, site.Id);
            Assert.AreEqual(siteWorker.WorkerId, worker.Id);

            //Old Version

            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkerVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, siteWorkerVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), siteWorkerVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(siteWorkerVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteWorker.Id, siteWorkerVersions[0].SiteWorkerId);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkerVersions[0].MicrotingUid);
            Assert.AreEqual(site.Id, siteWorkerVersions[0].SiteId);
            Assert.AreEqual(worker.Id, siteWorkerVersions[0].WorkerId);

            //New Version

            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkerVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, siteWorkerVersions[1].Version);
//            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkerVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(siteWorkerVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(siteWorker.Id, siteWorkerVersions[1].SiteWorkerId);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkerVersions[1].MicrotingUid);
            Assert.AreEqual(site.Id, siteWorkerVersions[1].SiteId);
            Assert.AreEqual(worker.Id, siteWorkerVersions[1].WorkerId);
        }
    }
}