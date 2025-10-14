/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

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
public class WorkersUTest : DbTestFixture
{
    [Test]
    public async Task Workers_Create_DoesCreate()
    {
        //Arrange
        Random rnd = new Random();


        Worker worker = new Worker
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString(),
            MicrotingUid = rnd.Next(1, 255)
        };

        //Act

        await worker.Create(DbContext).ConfigureAwait(false);

        List<Worker> workers = DbContext.Workers.AsNoTracking().ToList();
        List<WorkerVersion> workersVersion = DbContext.WorkerVersions.AsNoTracking().ToList();

        //Assert

        Assert.That(workers, Is.Not.EqualTo(null));
        Assert.That(workersVersion, Is.Not.EqualTo(null));

        Assert.That(workersVersion.Count(), Is.EqualTo(1));
        Assert.That(workers.Count(), Is.EqualTo(1));

        Assert.That(workers[0].CreatedAt.ToString(), Is.EqualTo(worker.CreatedAt.ToString()));
        Assert.That(workers[0].Version, Is.EqualTo(worker.Version));
        //            Assert.AreEqual(worker.UpdatedAt.ToString(), workers[0].UpdatedAt.ToString());
        Assert.That(workers[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(workers[0].Email, Is.EqualTo(worker.Email));
        Assert.That(workers[0].FirstName, Is.EqualTo(worker.FirstName));
        Assert.That(workers[0].LastName, Is.EqualTo(worker.LastName));
        Assert.That(workers[0].MicrotingUid, Is.EqualTo(worker.MicrotingUid));
        Assert.That(workers[0].full_name(), Is.EqualTo(worker.full_name()));

        //Versions
        Assert.That(workersVersion[0].CreatedAt.ToString(), Is.EqualTo(worker.CreatedAt.ToString()));
        Assert.That(workersVersion[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(worker.UpdatedAt.ToString(), workersVersion[0].UpdatedAt.ToString());
        Assert.That(workersVersion[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(workersVersion[0].Email, Is.EqualTo(worker.Email));
        Assert.That(workersVersion[0].FirstName, Is.EqualTo(worker.FirstName));
        Assert.That(workersVersion[0].LastName, Is.EqualTo(worker.LastName));
        Assert.That(workersVersion[0].MicrotingUid, Is.EqualTo(worker.MicrotingUid));
    }

    [Test]
    public async Task Workers_Update_DoesUpdate()
    {
        //Arrange

        Random rnd = new Random();


        Worker worker = new Worker
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString(),
            MicrotingUid = rnd.Next(1, 255)
        };

        await worker.Create(DbContext).ConfigureAwait(false);

        //Act

        DateTime? oldUpdatedAt = worker.UpdatedAt;
        string oldFirstName = worker.FirstName;
        string oldLastName = worker.LastName;
        string oldEmail = worker.Email;
        int? oldMicrotingUid = worker.MicrotingUid;

        worker.FirstName = Guid.NewGuid().ToString();
        worker.LastName = Guid.NewGuid().ToString();
        worker.Email = Guid.NewGuid().ToString();
        worker.MicrotingUid = rnd.Next(1, 255);

        await worker.Update(DbContext).ConfigureAwait(false);

        List<Worker> workers = DbContext.Workers.AsNoTracking().ToList();
        List<WorkerVersion> workersVersion = DbContext.WorkerVersions.AsNoTracking().ToList();

        //Assert

        Assert.That(workers, Is.Not.EqualTo(null));
        Assert.That(workersVersion, Is.Not.EqualTo(null));

        Assert.That(workers.Count(), Is.EqualTo(1));
        Assert.That(workersVersion.Count(), Is.EqualTo(2));

        Assert.That(workers[0].CreatedAt.ToString(), Is.EqualTo(worker.CreatedAt.ToString()));
        Assert.That(workers[0].Version, Is.EqualTo(worker.Version));
        //            Assert.AreEqual(worker.UpdatedAt.ToString(), workers[0].UpdatedAt.ToString());
        Assert.That(workers[0].Email, Is.EqualTo(worker.Email));
        Assert.That(workers[0].FirstName, Is.EqualTo(worker.FirstName));
        Assert.That(workers[0].LastName, Is.EqualTo(worker.LastName));
        Assert.That(workers[0].MicrotingUid, Is.EqualTo(worker.MicrotingUid));
        Assert.That(workers[0].full_name(), Is.EqualTo(worker.full_name()));

        //Version 1 Old Version
        Assert.That(workersVersion[0].CreatedAt.ToString(), Is.EqualTo(worker.CreatedAt.ToString()));
        Assert.That(workersVersion[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), workersVersion[0].UpdatedAt.ToString());
        Assert.That(workersVersion[0].Email, Is.EqualTo(oldEmail));
        Assert.That(workersVersion[0].FirstName, Is.EqualTo(oldFirstName));
        Assert.That(workersVersion[0].LastName, Is.EqualTo(oldLastName));
        Assert.That(workersVersion[0].MicrotingUid, Is.EqualTo(oldMicrotingUid));


        //Version 2 Updated Version
        Assert.That(workersVersion[1].CreatedAt.ToString(), Is.EqualTo(worker.CreatedAt.ToString()));
        Assert.That(workersVersion[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(worker.UpdatedAt.ToString(), workersVersion[1].UpdatedAt.ToString());
        Assert.That(workersVersion[1].Email, Is.EqualTo(worker.Email));
        Assert.That(workersVersion[1].FirstName, Is.EqualTo(worker.FirstName));
        Assert.That(workersVersion[1].LastName, Is.EqualTo(worker.LastName));
        Assert.That(workersVersion[1].MicrotingUid, Is.EqualTo(worker.MicrotingUid));
    }

    [Test]
    public async Task Workers_Delete_DoesSetWorkflowstateToRemoved()
    {
        //Arrange

        Random rnd = new Random();


        Worker worker = new Worker
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString(),
            MicrotingUid = rnd.Next(1, 255)
        };

        await worker.Create(DbContext).ConfigureAwait(false);

        //Act

        DateTime? oldUpdatedAt = worker.UpdatedAt;

        await worker.Delete(DbContext);

        List<Worker> workers = DbContext.Workers.AsNoTracking().ToList();
        List<WorkerVersion> workersVersion = DbContext.WorkerVersions.AsNoTracking().ToList();

        //Assert

        Assert.That(workers, Is.Not.EqualTo(null));
        Assert.That(workersVersion, Is.Not.EqualTo(null));

        Assert.That(workers.Count(), Is.EqualTo(1));
        Assert.That(workersVersion.Count(), Is.EqualTo(2));

        Assert.That(workers[0].CreatedAt.ToString(), Is.EqualTo(worker.CreatedAt.ToString()));
        Assert.That(workers[0].Version, Is.EqualTo(worker.Version));
        //            Assert.AreEqual(worker.UpdatedAt.ToString(), workers[0].UpdatedAt.ToString());
        Assert.That(workers[0].Email, Is.EqualTo(worker.Email));
        Assert.That(workers[0].FirstName, Is.EqualTo(worker.FirstName));
        Assert.That(workers[0].LastName, Is.EqualTo(worker.LastName));
        Assert.That(workers[0].MicrotingUid, Is.EqualTo(worker.MicrotingUid));
        Assert.That(workers[0].full_name(), Is.EqualTo(worker.full_name()));

        Assert.That(workers[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));

        //Version 1
        Assert.That(workersVersion[0].CreatedAt.ToString(), Is.EqualTo(worker.CreatedAt.ToString()));
        Assert.That(workersVersion[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), workersVersion[0].UpdatedAt.ToString());
        Assert.That(workersVersion[0].Email, Is.EqualTo(worker.Email));
        Assert.That(workersVersion[0].FirstName, Is.EqualTo(worker.FirstName));
        Assert.That(workersVersion[0].LastName, Is.EqualTo(worker.LastName));
        Assert.That(workersVersion[0].MicrotingUid, Is.EqualTo(worker.MicrotingUid));

        Assert.That(workersVersion[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        //Version 2 Deleted Version
        Assert.That(workersVersion[1].CreatedAt.ToString(), Is.EqualTo(worker.CreatedAt.ToString()));
        Assert.That(workersVersion[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(worker.UpdatedAt.ToString(), workersVersion[1].UpdatedAt.ToString());
        Assert.That(workersVersion[1].Email, Is.EqualTo(worker.Email));
        Assert.That(workersVersion[1].FirstName, Is.EqualTo(worker.FirstName));
        Assert.That(workersVersion[1].LastName, Is.EqualTo(worker.LastName));
        Assert.That(workersVersion[1].MicrotingUid, Is.EqualTo(worker.MicrotingUid));

        Assert.That(workersVersion[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
    }
}