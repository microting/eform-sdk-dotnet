/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 microting

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

using System.Linq;
using eFormShared;

namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class workers : BaseEntity
    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }
//
//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        public int MicrotingUid { get; set; }

//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public int? version { get; set; }

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
        
        
        public void Create(MicrotingDbAnySql dbContext)
        {
            WorkflowState = Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.workers.Add(this);
            dbContext.SaveChanges();

            dbContext.worker_versions.Add(MapWorkerVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            workers worker = dbContext.workers.FirstOrDefault(x => x.Id == Id);

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
                worker.UpdatedAt = DateTime.Now;

                dbContext.worker_versions.Add(MapWorkerVersions(worker));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            workers worker = dbContext.workers.FirstOrDefault(x => x.Id == Id);

            if (worker == null)
            {
                throw new NullReferenceException($"Could not find Worker with Id: {Id}");
            }

            worker.WorkflowState = Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                worker.Version += 1;
                worker.UpdatedAt = DateTime.Now;

                dbContext.worker_versions.Add(MapWorkerVersions(worker));
                dbContext.SaveChanges();
            }
        }

        
        private worker_versions MapWorkerVersions(workers workers)
        {
            worker_versions workerVer = new worker_versions();
            workerVer.WorkflowState = workers.WorkflowState;
            workerVer.Version = workers.Version;
            workerVer.CreatedAt = workers.CreatedAt;
            workerVer.UpdatedAt = workers.UpdatedAt;
            workerVer.MicrotingUid = workers.MicrotingUid;
            workerVer.FirstName = workers.FirstName;
            workerVer.LastName = workers.LastName;

            workerVer.WorkerId = workers.Id; //<<--

            return workerVer;
        }
    }
}
