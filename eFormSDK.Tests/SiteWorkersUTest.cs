using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class SiteWorkersUTest : DbTestFixture
    {
        [Test]
        public void SiteWorkers_Create_DoesCreate()
        {
            // Arrange

            Random rnd = new Random();

            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);

            workers worker = new workers();
            worker.Email = Guid.NewGuid().ToString();
            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);
            worker.Create(DbContext);

            site_workers siteWorker = new site_workers();
            siteWorker.MicrotingUid = rnd.Next(1, 255);
            siteWorker.SiteId = site.Id;
            siteWorker.WorkerId = worker.Id;
            
            //Act
            
            siteWorker.Create(DbContext);
            
            List<site_workers> siteWorkers = DbContext.site_workers.AsNoTracking().ToList();
            List<site_worker_versions> siteWorkerVersions = DbContext.site_worker_versions.AsNoTracking().ToList();
            
            Assert.NotNull(siteWorkers);                                                             
            Assert.NotNull(siteWorkerVersions);                                                             

            Assert.AreEqual(1,siteWorkers.Count());  
            Assert.AreEqual(1,siteWorkerVersions.Count());  
            
            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkers[0].CreatedAt.ToString());                                  
            Assert.AreEqual(siteWorker.Version, siteWorkers[0].Version);                                      
            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkers[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteWorkers[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteWorker.Id, siteWorkers[0].Id);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkers[0].MicrotingUid);
            Assert.AreEqual(siteWorker.SiteId, site.Id);
            Assert.AreEqual(siteWorker.WorkerId, worker.Id);
            
            //Versions
            
            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkerVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, siteWorkerVersions[0].Version);                                      
            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkerVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteWorkerVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteWorker.Id, siteWorkerVersions[0].SiteWorkerId);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkerVersions[0].MicrotingUid);
            Assert.AreEqual(site.Id, siteWorkerVersions[0].SiteId);
            Assert.AreEqual(worker.Id, siteWorkerVersions[0].WorkerId);
        }

        [Test]
        public void SiteWorkers_Update_DoesUpdate()
        {
            // Arrange

            Random rnd = new Random();

            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);

            workers worker = new workers();
            worker.Email = Guid.NewGuid().ToString();
            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);
            worker.Create(DbContext);

            site_workers siteWorker = new site_workers();
            siteWorker.MicrotingUid = rnd.Next(1, 255);
            siteWorker.SiteId = site.Id;
            siteWorker.WorkerId = worker.Id;
            siteWorker.Create(DbContext);

            
            //Act

            DateTime? oldUpdatedAt = siteWorker.UpdatedAt;
            int? oldMicroTingUid = siteWorker.MicrotingUid;

            siteWorker.MicrotingUid = rnd.Next(1, 255);
            
            siteWorker.Update(DbContext);

            
            List<site_workers> siteWorkers = DbContext.site_workers.AsNoTracking().ToList();
            List<site_worker_versions> siteWorkerVersions = DbContext.site_worker_versions.AsNoTracking().ToList();
            
            Assert.NotNull(siteWorkers);                                                             
            Assert.NotNull(siteWorkerVersions);                                                             

            Assert.AreEqual(1,siteWorkers.Count());  
            Assert.AreEqual(2,siteWorkerVersions.Count());  
            
            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkers[0].CreatedAt.ToString());                                  
            Assert.AreEqual(siteWorker.Version, siteWorkers[0].Version);                                      
            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkers[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteWorkers[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteWorker.Id, siteWorkers[0].Id);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkers[0].MicrotingUid);
            Assert.AreEqual(siteWorker.SiteId, site.Id);
            Assert.AreEqual(siteWorker.WorkerId, worker.Id);
            
            //Old Version
            
            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkerVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, siteWorkerVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), siteWorkerVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteWorkerVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteWorker.Id, siteWorkerVersions[0].SiteWorkerId);
            Assert.AreEqual(oldMicroTingUid, siteWorkerVersions[0].MicrotingUid);
            Assert.AreEqual(site.Id, siteWorkerVersions[0].SiteId);
            Assert.AreEqual(worker.Id, siteWorkerVersions[0].WorkerId);
            
            //New Version
            
            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkerVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, siteWorkerVersions[1].Version);                                      
            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkerVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteWorkerVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteWorker.Id, siteWorkerVersions[1].SiteWorkerId);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkerVersions[1].MicrotingUid);
            Assert.AreEqual(site.Id, siteWorkerVersions[1].SiteId);
            Assert.AreEqual(worker.Id, siteWorkerVersions[1].WorkerId);
        }

        [Test]
        public void SiteWorkers_Delete_DoesSetWorkflowStateToRemoved()
        {
            // Arrange

            Random rnd = new Random();

            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);

            workers worker = new workers();
            worker.Email = Guid.NewGuid().ToString();
            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);
            worker.Create(DbContext);

            site_workers siteWorker = new site_workers();
            siteWorker.MicrotingUid = rnd.Next(1, 255);
            siteWorker.SiteId = site.Id;
            siteWorker.WorkerId = worker.Id;
            siteWorker.Create(DbContext);

            
            //Act

            DateTime? oldUpdatedAt = siteWorker.UpdatedAt;
            
            siteWorker.Delete(DbContext);

            
            List<site_workers> siteWorkers = DbContext.site_workers.AsNoTracking().ToList();
            List<site_worker_versions> siteWorkerVersions = DbContext.site_worker_versions.AsNoTracking().ToList();
            
            Assert.NotNull(siteWorkers);                                                             
            Assert.NotNull(siteWorkerVersions);                                                             

            Assert.AreEqual(1,siteWorkers.Count());  
            Assert.AreEqual(2,siteWorkerVersions.Count());  
            
            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkers[0].CreatedAt.ToString());                                  
            Assert.AreEqual(siteWorker.Version, siteWorkers[0].Version);                                      
            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkers[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteWorkers[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(siteWorker.Id, siteWorkers[0].Id);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkers[0].MicrotingUid);
            Assert.AreEqual(siteWorker.SiteId, site.Id);
            Assert.AreEqual(siteWorker.WorkerId, worker.Id);
            
            //Old Version
            
            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkerVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, siteWorkerVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), siteWorkerVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteWorkerVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteWorker.Id, siteWorkerVersions[0].SiteWorkerId);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkerVersions[0].MicrotingUid);
            Assert.AreEqual(site.Id, siteWorkerVersions[0].SiteId);
            Assert.AreEqual(worker.Id, siteWorkerVersions[0].WorkerId);
            
            //New Version
            
            Assert.AreEqual(siteWorker.CreatedAt.ToString(), siteWorkerVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, siteWorkerVersions[1].Version);                                      
            Assert.AreEqual(siteWorker.UpdatedAt.ToString(), siteWorkerVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteWorkerVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(siteWorker.Id, siteWorkerVersions[1].SiteWorkerId);
            Assert.AreEqual(siteWorker.MicrotingUid, siteWorkerVersions[1].MicrotingUid);
            Assert.AreEqual(site.Id, siteWorkerVersions[1].SiteId);
            Assert.AreEqual(worker.Id, siteWorkerVersions[1].WorkerId);
        }
    }
}