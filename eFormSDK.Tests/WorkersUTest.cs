/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 Microting A/S

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
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
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
            Assert.AreEqual(dbWorkers.WorkflowState, Constants.WorkflowStates.Created);
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
            
            Assert.AreEqual(dbWorkers.WorkflowState, Constants.WorkflowStates.Removed);
        }
    }
}