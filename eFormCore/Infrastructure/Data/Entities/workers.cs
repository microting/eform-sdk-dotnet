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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class workers : BaseEntity
    {
        public int MicrotingUid { get; set; }

        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string LastName { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        public virtual ICollection<site_workers> SiteWorkers { get; set; }

        public string full_name()
        {
            return this.FirstName + " " + this.LastName;
        }
        
        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            dbContext.workers.Add(this);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            dbContext.worker_versions.Add(MapWorkerVersions(this));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            workers worker = await dbContext.workers.FirstOrDefaultAsync(x => x.Id == Id);

            if (worker == null)
            {
                throw new NullReferenceException($"Could not find Worker with Id: {Id}");
            }

            worker.MicrotingUid = MicrotingUid;
            worker.FirstName = FirstName;
            worker.LastName = LastName;
            worker.Email = Email;

            if (dbContext.ChangeTracker.HasChanges())
            {
                worker.Version += 1;
                worker.UpdatedAt = DateTime.UtcNow;

                dbContext.worker_versions.Add(MapWorkerVersions(worker));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            workers worker = await dbContext.workers.FirstOrDefaultAsync(x => x.Id == Id);

            if (worker == null)
            {
                throw new NullReferenceException($"Could not find Worker with Id: {Id}");
            }

            worker.WorkflowState = Constants.Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                worker.Version += 1;
                worker.UpdatedAt = DateTime.UtcNow;

                dbContext.worker_versions.Add(MapWorkerVersions(worker));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        
        private worker_versions MapWorkerVersions(workers workers)
        {
            return new worker_versions
            {
                WorkflowState = workers.WorkflowState,
                Version = workers.Version,
                CreatedAt = workers.CreatedAt,
                UpdatedAt = workers.UpdatedAt,
                MicrotingUid = workers.MicrotingUid,
                FirstName = workers.FirstName,
                LastName = workers.LastName,
                Email = workers.Email,
                WorkerId = workers.Id
            };
        }
    }
}
