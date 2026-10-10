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
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
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

    // The column is plain `datetime` (no fractional seconds), so test dates are whole seconds.
    private static readonly DateTime ResignedOn = new DateTime(2026, 3, 15);

    private static Worker NewWorker(bool resigned, DateTime? resignedAtDate) => new Worker
    {
        FirstName = Guid.NewGuid().ToString(),
        LastName = Guid.NewGuid().ToString(),
        Email = Guid.NewGuid().ToString(),
        MicrotingUid = new Random().Next(1, 255),
        Resigned = resigned,
        ResignedAtDate = resignedAtDate
    };

    [Test]
    public async Task Workers_Create_NotResigned_KeepsResignedAtDateNull()
    {
        Worker worker = NewWorker(false, null);

        await worker.Create(DbContext).ConfigureAwait(false);

        Worker stored = await DbContext.Workers.AsNoTracking().SingleAsync();
        WorkerVersion version = await DbContext.WorkerVersions.AsNoTracking().SingleAsync();

        Assert.That(stored.Resigned, Is.False);
        Assert.That(stored.ResignedAtDate, Is.Null);
        Assert.That(version.Resigned, Is.False);
        Assert.That(version.ResignedAtDate, Is.Null);
    }

    [Test]
    public async Task Workers_Create_Resigned_StoresResignedAtDateOnWorkerAndVersion()
    {
        Worker worker = NewWorker(true, ResignedOn);

        await worker.Create(DbContext).ConfigureAwait(false);

        Worker stored = await DbContext.Workers.AsNoTracking().SingleAsync();
        WorkerVersion version = await DbContext.WorkerVersions.AsNoTracking().SingleAsync();

        Assert.That(stored.Resigned, Is.True);
        Assert.That(stored.ResignedAtDate, Is.EqualTo(ResignedOn));
        Assert.That(version.Resigned, Is.True);
        Assert.That(version.ResignedAtDate, Is.EqualTo(ResignedOn));
    }

    [Test]
    public async Task Workers_Update_ResignThenReinstate_VersionsMirrorResignedAtDate()
    {
        Worker worker = NewWorker(false, null);
        await worker.Create(DbContext).ConfigureAwait(false);

        worker.Resigned = true;
        worker.ResignedAtDate = ResignedOn;
        await worker.Update(DbContext).ConfigureAwait(false);

        worker.Resigned = false;
        worker.ResignedAtDate = null;
        await worker.Update(DbContext).ConfigureAwait(false);

        Worker stored = await DbContext.Workers.AsNoTracking().SingleAsync();
        List<WorkerVersion> versions = await DbContext.WorkerVersions.AsNoTracking()
            .OrderBy(x => x.Version).ToListAsync();

        Assert.That(stored.Resigned, Is.False);
        Assert.That(stored.ResignedAtDate, Is.Null);

        Assert.That(versions.Select(x => x.Resigned), Is.EqualTo(new[] { false, true, false }));
        Assert.That(versions.Select(x => x.ResignedAtDate),
            Is.EqualTo(new DateTime?[] { null, ResignedOn, null }));
    }

    [Test]
    public async Task Migration_MakeWorkerResignedAtDateNullable_ClearsDateOnlyForNotResignedWorkers()
    {
        const string previousMigration = "20251120072535_AddingResignedToWorker";
        DateTime placeholder = new DateTime(2026, 1, 2);

        Worker notResigned = NewWorker(false, null);
        await notResigned.Create(DbContext).ConfigureAwait(false);
        Worker resigned = NewWorker(true, ResignedOn);
        await resigned.Create(DbContext).ConfigureAwait(false);

        IMigrator migrator = DbContext.GetService<IMigrator>();

        // Down(): NULLs get default(DateTime) before the column is NOT NULL again.
        await migrator.MigrateAsync(previousMigration).ConfigureAwait(false);

        Worker downNotResigned = await DbContext.Workers.AsNoTracking()
            .SingleAsync(x => x.Id == notResigned.Id);
        Assert.That(downNotResigned.ResignedAtDate, Is.EqualTo(DateTime.MinValue));

        // The pre-fix web dialog wrote "today" on every save; simulate that placeholder.
        await DbContext.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE `Workers` SET `ResignedAtDate` = {placeholder} WHERE `Id` = {notResigned.Id}");

        // Up(): the placeholder is cleared on the live row only.
        await migrator.MigrateAsync().ConfigureAwait(false);

        Worker upNotResigned = await DbContext.Workers.AsNoTracking()
            .SingleAsync(x => x.Id == notResigned.Id);
        Worker upResigned = await DbContext.Workers.AsNoTracking()
            .SingleAsync(x => x.Id == resigned.Id);
        WorkerVersion notResignedVersion = await DbContext.WorkerVersions.AsNoTracking()
            .SingleAsync(x => x.WorkerId == notResigned.Id);

        Assert.That(upNotResigned.ResignedAtDate, Is.Null);
        Assert.That(upResigned.ResignedAtDate, Is.EqualTo(ResignedOn));
        // History is left as Down() wrote it.
        Assert.That(notResignedVersion.ResignedAtDate, Is.EqualTo(DateTime.MinValue));
    }
}