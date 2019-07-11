using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class WorkersUTest : DbTestFixture
    {
        [Test]
        public void Workers_Create_DoesCreate()
        {
            //Arrange
            Random rnd = new Random();
            

            workers worker = new workers();
            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.Email = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);

            //Act

            worker.Create(DbContext);                                                             

            workers dbWorkers = DbContext.workers.AsNoTracking().First();                               
            List<workers> workersList = DbContext.workers.AsNoTracking().ToList();                      

            //Assert                                                                            

            Assert.NotNull(dbWorkers);                                                             
            Assert.NotNull(dbWorkers.Id);                                                          

            Assert.AreEqual(1,workersList.Count());                                                
            Assert.AreEqual(worker.CreatedAt.ToString(), dbWorkers.CreatedAt.ToString());                                  
            Assert.AreEqual(worker.Version, dbWorkers.Version);                                      
            Assert.AreEqual(worker.UpdatedAt.ToString(), dbWorkers.UpdatedAt.ToString());                                  
            Assert.AreEqual(dbWorkers.WorkflowState, eFormShared.Constants.WorkflowStates.Created);
            Assert.AreEqual(worker.Email, dbWorkers.Email);                      
            Assert.AreEqual(worker.FirstName, dbWorkers.FirstName);                      
            Assert.AreEqual(worker.LastName, dbWorkers.LastName);
            Assert.AreEqual(worker.MicrotingUid, dbWorkers.MicrotingUid); 
            Assert.AreEqual(worker.full_name(), dbWorkers.full_name()); 
        }

        [Test]
        public void Workers_Update_DoesUpdate()
        {
            //Arrange
            
            Random rnd = new Random();
            

            workers worker = new workers();
            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.Email = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);

            DbContext.workers.Add(worker);
            DbContext.SaveChanges();

            //Act

            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.Email = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);

            worker.Update(DbContext);

            workers dbWorkers = DbContext.workers.AsNoTracking().First();                               
            List<workers> workersList = DbContext.workers.AsNoTracking().ToList();
            List<worker_versions> machineVersions = DbContext.worker_versions.AsNoTracking().ToList();

            //Assert                                                                            

            Assert.NotNull(dbWorkers);                                                             
            Assert.NotNull(dbWorkers.Id);                                                          

            Assert.AreEqual(1,workersList.Count()); 
            Assert.AreEqual(1, machineVersions.Count());
            
            Assert.AreEqual(worker.CreatedAt.ToString(), dbWorkers.CreatedAt.ToString());                                  
            Assert.AreEqual(worker.Version, dbWorkers.Version);                                      
            Assert.AreEqual(worker.UpdatedAt.ToString(), dbWorkers.UpdatedAt.ToString());
            Assert.AreEqual(worker.Email, dbWorkers.Email);                      
            Assert.AreEqual(worker.FirstName, dbWorkers.FirstName);                      
            Assert.AreEqual(worker.LastName, dbWorkers.LastName);
            Assert.AreEqual(worker.MicrotingUid, dbWorkers.MicrotingUid); 
            Assert.AreEqual(worker.full_name(), dbWorkers.full_name()); 
        }

        [Test]
        public void Workers_Delete_DoesSetWorkflowstateToRemoved()
        {
            //Arrange
            
            Random rnd = new Random();
            

            workers worker = new workers();
            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.Email = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);

            DbContext.workers.Add(worker);
            DbContext.SaveChanges();
            
            //Act
            
            worker.Delete(DbContext);
            
            workers dbWorkers = DbContext.workers.AsNoTracking().First();                               
            List<workers> workersList = DbContext.workers.AsNoTracking().ToList();
            List<worker_versions> machineVersions = DbContext.worker_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbWorkers);                                                             
            Assert.NotNull(dbWorkers.Id);                                                          

            Assert.AreEqual(1,workersList.Count()); 
            Assert.AreEqual(1, machineVersions.Count());
            
            Assert.AreEqual(worker.CreatedAt.ToString(), dbWorkers.CreatedAt.ToString());                                  
            Assert.AreEqual(worker.Version, dbWorkers.Version);                                      
            Assert.AreEqual(worker.UpdatedAt.ToString(), dbWorkers.UpdatedAt.ToString());
            Assert.AreEqual(worker.Email, dbWorkers.Email);                      
            Assert.AreEqual(worker.FirstName, dbWorkers.FirstName);                      
            Assert.AreEqual(worker.LastName, dbWorkers.LastName);
            Assert.AreEqual(worker.MicrotingUid, dbWorkers.MicrotingUid); 
            Assert.AreEqual(worker.full_name(), dbWorkers.full_name()); 
            
            Assert.AreEqual(dbWorkers.WorkflowState, eFormShared.Constants.WorkflowStates.Removed);
        }
    }
}