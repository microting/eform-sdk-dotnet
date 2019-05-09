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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class site_workers : BaseEntity
    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int id { get; set; }

        [ForeignKey("site")]
        public int? site_id { get; set; }

        [ForeignKey("worker")]
        public int? worker_id { get; set; }

        public int? microting_uid { get; set; }

//        public int? version { get; set; }
//
//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        public virtual sites site { get; set; }

        public virtual workers worker { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.site_workers.Add(this);
            dbContext.SaveChanges();

            dbContext.site_worker_versions.Add(MapSiteWorkerVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            site_workers siteWorkers = dbContext.site_workers.FirstOrDefault(x => x.id == id);

            if (siteWorkers == null)
            {
                throw new NullReferenceException($"Could not find site worker tish ID: {id}");
            }

            siteWorkers.site_id = site_id;
            siteWorkers.worker_id = worker_id;
            siteWorkers.microting_uid = microting_uid;

            if (dbContext.ChangeTracker.HasChanges())
            {
                siteWorkers.version += 1;
                siteWorkers.updated_at = DateTime.Now;

                dbContext.site_worker_versions.Add(MapSiteWorkerVersions(siteWorkers));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            site_workers siteWorkers = dbContext.site_workers.FirstOrDefault(x => x.id == id);

            if (siteWorkers == null)
            {
                throw new NullReferenceException($"Could not find site worker tish ID: {id}");
            }

            siteWorkers.workflow_state = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                siteWorkers.version += 1;
                siteWorkers.updated_at = DateTime.Now;

                dbContext.site_worker_versions.Add(MapSiteWorkerVersions(siteWorkers));
                dbContext.SaveChanges();
            }
        }
        
        
        private site_worker_versions MapSiteWorkerVersions(site_workers site_workers)
        {
            site_worker_versions siteWorkerVer = new site_worker_versions();
            siteWorkerVer.workflow_state = site_workers.workflow_state;
            siteWorkerVer.version = site_workers.version;
            siteWorkerVer.created_at = site_workers.created_at;
            siteWorkerVer.updated_at = site_workers.updated_at;
            siteWorkerVer.microting_uid = site_workers.microting_uid;
            siteWorkerVer.site_id = site_workers.site_id;
            siteWorkerVer.worker_id = site_workers.worker_id;

            siteWorkerVer.site_worker_id = site_workers.id; //<<--

            return siteWorkerVer;
        }

    }
}
