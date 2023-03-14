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

            Assert.NotNull(workers);
            Assert.NotNull(workersVersion);

            Assert.AreEqual(1, workersVersion.Count());
            Assert.AreEqual(1, workers.Count());

            Assert.AreEqual(worker.CreatedAt.ToString(), workers[0].CreatedAt.ToString());
            Assert.AreEqual(worker.Version, workers[0].Version);
//            Assert.AreEqual(worker.UpdatedAt.ToString(), workers[0].UpdatedAt.ToString());
            Assert.AreEqual(workers[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(worker.Email, workers[0].Email);
            Assert.AreEqual(worker.FirstName, workers[0].FirstName);
            Assert.AreEqual(worker.LastName, workers[0].LastName);
            Assert.AreEqual(worker.MicrotingUid, workers[0].MicrotingUid);
            Assert.AreEqual(worker.full_name(), workers[0].full_name());

            //Versions
            Assert.AreEqual(worker.CreatedAt.ToString(), workersVersion[0].CreatedAt.ToString());
            Assert.AreEqual(1, workersVersion[0].Version);
//            Assert.AreEqual(worker.UpdatedAt.ToString(), workersVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(workersVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(worker.Email, workersVersion[0].Email);
            Assert.AreEqual(worker.FirstName, workersVersion[0].FirstName);
            Assert.AreEqual(worker.LastName, workersVersion[0].LastName);
            Assert.AreEqual(worker.MicrotingUid, workersVersion[0].MicrotingUid);
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

            Assert.NotNull(workers);
            Assert.NotNull(workersVersion);

            Assert.AreEqual(1, workers.Count());
            Assert.AreEqual(2, workersVersion.Count());

            Assert.AreEqual(worker.CreatedAt.ToString(), workers[0].CreatedAt.ToString());
            Assert.AreEqual(worker.Version, workers[0].Version);
//            Assert.AreEqual(worker.UpdatedAt.ToString(), workers[0].UpdatedAt.ToString());
            Assert.AreEqual(worker.Email, workers[0].Email);
            Assert.AreEqual(worker.FirstName, workers[0].FirstName);
            Assert.AreEqual(worker.LastName, workers[0].LastName);
            Assert.AreEqual(worker.MicrotingUid, workers[0].MicrotingUid);
            Assert.AreEqual(worker.full_name(), workers[0].full_name());

            //Version 1 Old Version
            Assert.AreEqual(worker.CreatedAt.ToString(), workersVersion[0].CreatedAt.ToString());
            Assert.AreEqual(1, workersVersion[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), workersVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(oldEmail, workersVersion[0].Email);
            Assert.AreEqual(oldFirstName, workersVersion[0].FirstName);
            Assert.AreEqual(oldLastName, workersVersion[0].LastName);
            Assert.AreEqual(oldMicrotingUid, workersVersion[0].MicrotingUid);


            //Version 2 Updated Version
            Assert.AreEqual(worker.CreatedAt.ToString(), workersVersion[1].CreatedAt.ToString());
            Assert.AreEqual(2, workersVersion[1].Version);
//            Assert.AreEqual(worker.UpdatedAt.ToString(), workersVersion[1].UpdatedAt.ToString());
            Assert.AreEqual(worker.Email, workersVersion[1].Email);
            Assert.AreEqual(worker.FirstName, workersVersion[1].FirstName);
            Assert.AreEqual(worker.LastName, workersVersion[1].LastName);
            Assert.AreEqual(worker.MicrotingUid, workersVersion[1].MicrotingUid);
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

            Assert.NotNull(workers);
            Assert.NotNull(workersVersion);

            Assert.AreEqual(1, workers.Count());
            Assert.AreEqual(2, workersVersion.Count());

            Assert.AreEqual(worker.CreatedAt.ToString(), workers[0].CreatedAt.ToString());
            Assert.AreEqual(worker.Version, workers[0].Version);
//            Assert.AreEqual(worker.UpdatedAt.ToString(), workers[0].UpdatedAt.ToString());
            Assert.AreEqual(worker.Email, workers[0].Email);
            Assert.AreEqual(worker.FirstName, workers[0].FirstName);
            Assert.AreEqual(worker.LastName, workers[0].LastName);
            Assert.AreEqual(worker.MicrotingUid, workers[0].MicrotingUid);
            Assert.AreEqual(worker.full_name(), workers[0].full_name());

            Assert.AreEqual(workers[0].WorkflowState, Constants.WorkflowStates.Removed);

            //Version 1
            Assert.AreEqual(worker.CreatedAt.ToString(), workersVersion[0].CreatedAt.ToString());
            Assert.AreEqual(1, workersVersion[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), workersVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(worker.Email, workersVersion[0].Email);
            Assert.AreEqual(worker.FirstName, workersVersion[0].FirstName);
            Assert.AreEqual(worker.LastName, workersVersion[0].LastName);
            Assert.AreEqual(worker.MicrotingUid, workersVersion[0].MicrotingUid);

            Assert.AreEqual(workersVersion[0].WorkflowState, Constants.WorkflowStates.Created);

            //Version 2 Deleted Version
            Assert.AreEqual(worker.CreatedAt.ToString(), workersVersion[1].CreatedAt.ToString());
            Assert.AreEqual(2, workersVersion[1].Version);
//            Assert.AreEqual(worker.UpdatedAt.ToString(), workersVersion[1].UpdatedAt.ToString());
            Assert.AreEqual(worker.Email, workersVersion[1].Email);
            Assert.AreEqual(worker.FirstName, workersVersion[1].FirstName);
            Assert.AreEqual(worker.LastName, workersVersion[1].LastName);
            Assert.AreEqual(worker.MicrotingUid, workersVersion[1].MicrotingUid);

            Assert.AreEqual(workersVersion[1].WorkflowState, Constants.WorkflowStates.Removed);
        }
    }
}