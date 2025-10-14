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

namespace eFormSDK.Base.Tests;

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

        Assert.That(siteWorkers, Is.Not.EqualTo(null));
        Assert.That(siteWorkerVersions, Is.Not.EqualTo(null));

        Assert.That(siteWorkers.Count(), Is.EqualTo(1));
        Assert.That(siteWorkerVersions.Count(), Is.EqualTo(1));

        Assert.That(siteWorkers[0].CreatedAt.ToString(), Is.EqualTo(siteWorker.CreatedAt.ToString()));
        Assert.That(siteWorkers[0].Version, Is.EqualTo(siteWorker.Version));
        //            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkers[0].UpdatedAt.ToString());
        Assert.That(siteWorkers[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(siteWorkers[0].Id, Is.EqualTo(siteWorker.Id));
        Assert.That(siteWorkers[0].MicrotingUid, Is.EqualTo(siteWorker.MicrotingUid));
        Assert.That(site.Id, Is.EqualTo(siteWorker.SiteId));
        Assert.That(worker.Id, Is.EqualTo(siteWorker.WorkerId));

        //Versions

        Assert.That(siteWorkerVersions[0].CreatedAt.ToString(), Is.EqualTo(siteWorker.CreatedAt.ToString()));
        Assert.That(siteWorkerVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkerVersions[0].UpdatedAt.ToString());
        Assert.That(siteWorkerVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(siteWorkerVersions[0].SiteWorkerId, Is.EqualTo(siteWorker.Id));
        Assert.That(siteWorkerVersions[0].MicrotingUid, Is.EqualTo(siteWorker.MicrotingUid));
        Assert.That(siteWorkerVersions[0].SiteId, Is.EqualTo(site.Id));
        Assert.That(siteWorkerVersions[0].WorkerId, Is.EqualTo(worker.Id));
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

        Assert.That(siteWorkers, Is.Not.EqualTo(null));
        Assert.That(siteWorkerVersions, Is.Not.EqualTo(null));

        Assert.That(siteWorkers.Count(), Is.EqualTo(1));
        Assert.That(siteWorkerVersions.Count(), Is.EqualTo(2));

        Assert.That(siteWorkers[0].CreatedAt.ToString(), Is.EqualTo(siteWorker.CreatedAt.ToString()));
        Assert.That(siteWorkers[0].Version, Is.EqualTo(siteWorker.Version));
        //            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkers[0].UpdatedAt.ToString());
        Assert.That(siteWorkers[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(siteWorkers[0].Id, Is.EqualTo(siteWorker.Id));
        Assert.That(siteWorkers[0].MicrotingUid, Is.EqualTo(siteWorker.MicrotingUid));
        Assert.That(site.Id, Is.EqualTo(siteWorker.SiteId));
        Assert.That(worker.Id, Is.EqualTo(siteWorker.WorkerId));

        //Old Version

        Assert.That(siteWorkerVersions[0].CreatedAt.ToString(), Is.EqualTo(siteWorker.CreatedAt.ToString()));
        Assert.That(siteWorkerVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), siteWorkerVersions[0].UpdatedAt.ToString());
        Assert.That(siteWorkerVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(siteWorkerVersions[0].SiteWorkerId, Is.EqualTo(siteWorker.Id));
        Assert.That(siteWorkerVersions[0].MicrotingUid, Is.EqualTo(oldMicroTingUid));
        Assert.That(siteWorkerVersions[0].SiteId, Is.EqualTo(site.Id));
        Assert.That(siteWorkerVersions[0].WorkerId, Is.EqualTo(worker.Id));

        //New Version

        Assert.That(siteWorkerVersions[1].CreatedAt.ToString(), Is.EqualTo(siteWorker.CreatedAt.ToString()));
        Assert.That(siteWorkerVersions[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkerVersions[1].UpdatedAt.ToString());
        Assert.That(siteWorkerVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(siteWorkerVersions[1].SiteWorkerId, Is.EqualTo(siteWorker.Id));
        Assert.That(siteWorkerVersions[1].MicrotingUid, Is.EqualTo(siteWorker.MicrotingUid));
        Assert.That(siteWorkerVersions[1].SiteId, Is.EqualTo(site.Id));
        Assert.That(siteWorkerVersions[1].WorkerId, Is.EqualTo(worker.Id));
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

        Assert.That(siteWorkers, Is.Not.EqualTo(null));
        Assert.That(siteWorkerVersions, Is.Not.EqualTo(null));

        Assert.That(siteWorkers.Count(), Is.EqualTo(1));
        Assert.That(siteWorkerVersions.Count(), Is.EqualTo(2));

        Assert.That(siteWorkers[0].CreatedAt.ToString(), Is.EqualTo(siteWorker.CreatedAt.ToString()));
        Assert.That(siteWorkers[0].Version, Is.EqualTo(siteWorker.Version));
        //            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkers[0].UpdatedAt.ToString());
        Assert.That(siteWorkers[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(siteWorkers[0].Id, Is.EqualTo(siteWorker.Id));
        Assert.That(siteWorkers[0].MicrotingUid, Is.EqualTo(siteWorker.MicrotingUid));
        Assert.That(site.Id, Is.EqualTo(siteWorker.SiteId));
        Assert.That(worker.Id, Is.EqualTo(siteWorker.WorkerId));

        //Old Version

        Assert.That(siteWorkerVersions[0].CreatedAt.ToString(), Is.EqualTo(siteWorker.CreatedAt.ToString()));
        Assert.That(siteWorkerVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), siteWorkerVersions[0].UpdatedAt.ToString());
        Assert.That(siteWorkerVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(siteWorkerVersions[0].SiteWorkerId, Is.EqualTo(siteWorker.Id));
        Assert.That(siteWorkerVersions[0].MicrotingUid, Is.EqualTo(siteWorker.MicrotingUid));
        Assert.That(siteWorkerVersions[0].SiteId, Is.EqualTo(site.Id));
        Assert.That(siteWorkerVersions[0].WorkerId, Is.EqualTo(worker.Id));

        //New Version

        Assert.That(siteWorkerVersions[1].CreatedAt.ToString(), Is.EqualTo(siteWorker.CreatedAt.ToString()));
        Assert.That(siteWorkerVersions[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkerVersions[1].UpdatedAt.ToString());
        Assert.That(siteWorkerVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(siteWorkerVersions[1].SiteWorkerId, Is.EqualTo(siteWorker.Id));
        Assert.That(siteWorkerVersions[1].MicrotingUid, Is.EqualTo(siteWorker.MicrotingUid));
        Assert.That(siteWorkerVersions[1].SiteId, Is.EqualTo(site.Id));
        Assert.That(siteWorkerVersions[1].WorkerId, Is.EqualTo(worker.Id));
    }
}