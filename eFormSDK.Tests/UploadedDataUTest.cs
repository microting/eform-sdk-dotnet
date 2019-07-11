using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Runtime.Internal.Util;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class UploadedDataUTest : DbTestFixture
    {
        [Test]
        public void UploadedData_Create_DoesCreate()
        {
            //Arrange
            Random rnd = new Random();
            
            short minValue = Int16.MinValue;
            short maxValue = Int16.MaxValue;
            
            uploaded_data uploadedData = new uploaded_data();
            uploadedData.Checksum = Guid.NewGuid().ToString();
            uploadedData.Extension = Guid.NewGuid().ToString();
            uploadedData.Local = (short)rnd.Next(minValue, maxValue);
            uploadedData.CurrentFile = Guid.NewGuid().ToString();
            uploadedData.ExpirationDate = DateTime.Now;
            uploadedData.FileLocation = Guid.NewGuid().ToString();
            uploadedData.FileName = Guid.NewGuid().ToString();
            uploadedData.TranscriptionId = rnd.Next(1, 255);
            uploadedData.UploaderId = rnd.Next(1, 255);
            uploadedData.UploaderType = Guid.NewGuid().ToString();
            
            //Act
            
            uploadedData.Create(DbContext);

            uploaded_data dbUploadedData = DbContext.uploaded_data.AsNoTracking().First();
            List<uploaded_data> uploadedDataList = DbContext.uploaded_data.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbUploadedData);                                                             
            Assert.NotNull(dbUploadedData.Id);                                                          

            Assert.AreEqual(1,uploadedDataList.Count());                                                
            Assert.AreEqual(uploadedData.CreatedAt.ToString(), dbUploadedData.CreatedAt.ToString());                                  
            Assert.AreEqual(uploadedData.Version, dbUploadedData.Version);                                      
            Assert.AreEqual(uploadedData.UpdatedAt.ToString(), dbUploadedData.UpdatedAt.ToString());                                  
            Assert.AreEqual(dbUploadedData.WorkflowState, eFormShared.Constants.WorkflowStates.Created);
            Assert.AreEqual(uploadedData.Checksum, dbUploadedData.Checksum);                      
            Assert.AreEqual(uploadedData.Extension, dbUploadedData.Extension);                      
            Assert.AreEqual(uploadedData.Local, dbUploadedData.Local);
            Assert.AreEqual(uploadedData.CurrentFile, dbUploadedData.CurrentFile); 
            Assert.AreEqual(uploadedData.ExpirationDate.ToString(), dbUploadedData.ExpirationDate.ToString()); 
            Assert.AreEqual(uploadedData.FileLocation, dbUploadedData.FileLocation); 
            Assert.AreEqual(uploadedData.FileName, dbUploadedData.FileName); 
            Assert.AreEqual(uploadedData.TranscriptionId, dbUploadedData.TranscriptionId); 
            Assert.AreEqual(uploadedData.UploaderId, dbUploadedData.UploaderId);
            Assert.AreEqual(uploadedData.UploaderType, dbUploadedData.UploaderType);
        }

        [Test]
        public void UploadedData_Update_DoesUpdate()
        {
            //Arrange
            
            Random rnd = new Random();
            

            short minValue = Int16.MinValue;
            short maxValue = Int16.MaxValue;
            
            uploaded_data uploadedData = new uploaded_data();
            uploadedData.Checksum = Guid.NewGuid().ToString();
            uploadedData.Extension = Guid.NewGuid().ToString();
            uploadedData.Local = (short)rnd.Next(minValue, maxValue);
            uploadedData.CurrentFile = Guid.NewGuid().ToString();
            uploadedData.ExpirationDate = DateTime.Now;
            uploadedData.FileLocation = Guid.NewGuid().ToString();
            uploadedData.FileName = Guid.NewGuid().ToString();
            uploadedData.TranscriptionId = rnd.Next(1, 255);
            uploadedData.UploaderId = rnd.Next(1, 255);
            uploadedData.UploaderType = Guid.NewGuid().ToString();

            DbContext.uploaded_data.Add(uploadedData);
            DbContext.SaveChanges();

            //Act

            uploadedData.Checksum = Guid.NewGuid().ToString();
            uploadedData.Extension = Guid.NewGuid().ToString();
            uploadedData.Local = (short)rnd.Next(minValue, maxValue);
            uploadedData.CurrentFile = Guid.NewGuid().ToString();
            uploadedData.ExpirationDate = DateTime.Now;
            uploadedData.FileLocation = Guid.NewGuid().ToString();
            uploadedData.FileName = Guid.NewGuid().ToString();
            uploadedData.TranscriptionId = rnd.Next(1, 255);
            uploadedData.UploaderId = rnd.Next(1, 255);
            uploadedData.UploaderType = Guid.NewGuid().ToString();

            uploadedData.Update(DbContext);

            uploaded_data dbUploadedData = DbContext.uploaded_data.AsNoTracking().First();                               
            List<uploaded_data> uploadedDataList = DbContext.uploaded_data.AsNoTracking().ToList();
            List<uploaded_data_versions> uploadedDataVersionses = DbContext.uploaded_data_versions.AsNoTracking().ToList();

            //Assert                                                                            

            
            Assert.NotNull(dbUploadedData);                                                             
            Assert.NotNull(dbUploadedData.Id);                                                          

            Assert.AreEqual(1,uploadedDataList.Count());      
            Assert.AreEqual(1, uploadedDataVersionses.Count());
            
            Assert.AreEqual(uploadedData.CreatedAt.ToString(), dbUploadedData.CreatedAt.ToString());                                  
            Assert.AreEqual(uploadedData.Version, dbUploadedData.Version);                                      
            Assert.AreEqual(uploadedData.UpdatedAt.ToString(), dbUploadedData.UpdatedAt.ToString());                                  
            Assert.AreEqual(uploadedData.Checksum, dbUploadedData.Checksum);                      
            Assert.AreEqual(uploadedData.Extension, dbUploadedData.Extension);                      
            Assert.AreEqual(uploadedData.Local, dbUploadedData.Local);
            Assert.AreEqual(uploadedData.CurrentFile, dbUploadedData.CurrentFile); 
            Assert.AreEqual(uploadedData.ExpirationDate.ToString(), dbUploadedData.ExpirationDate.ToString()); 
            Assert.AreEqual(uploadedData.FileLocation, dbUploadedData.FileLocation); 
            Assert.AreEqual(uploadedData.FileName, dbUploadedData.FileName); 
            Assert.AreEqual(uploadedData.TranscriptionId, dbUploadedData.TranscriptionId); 
            Assert.AreEqual(uploadedData.UploaderId, dbUploadedData.UploaderId);
            Assert.AreEqual(uploadedData.UploaderType, dbUploadedData.UploaderType);
        }
        
        [Test]
        public void UploadedData_Delete_DoesSetWorkflowstateToRemoved()
        {
            //Arrange
            
            Random rnd = new Random();
            
            short minValue = Int16.MinValue;
            short maxValue = Int16.MaxValue;
            
            uploaded_data uploadedData = new uploaded_data();
            uploadedData.Checksum = Guid.NewGuid().ToString();
            uploadedData.Extension = Guid.NewGuid().ToString();
            uploadedData.Local = (short)rnd.Next(minValue, maxValue);
            uploadedData.CurrentFile = Guid.NewGuid().ToString();
            uploadedData.ExpirationDate = DateTime.Now;
            uploadedData.FileLocation = Guid.NewGuid().ToString();
            uploadedData.FileName = Guid.NewGuid().ToString();
            uploadedData.TranscriptionId = rnd.Next(1, 255);
            uploadedData.UploaderId = rnd.Next(1, 255);
            uploadedData.UploaderType = Guid.NewGuid().ToString();

            DbContext.uploaded_data.Add(uploadedData);
            DbContext.SaveChanges();
            
            //Act
            
            uploadedData.Delete(DbContext);
            
            uploaded_data dbUploadedData = DbContext.uploaded_data.AsNoTracking().First();                               
            List<uploaded_data> uploadedDataList = DbContext.uploaded_data.AsNoTracking().ToList();
            List<uploaded_data_versions> uploadedDataVersionses = DbContext.uploaded_data_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbUploadedData);                                                             
            Assert.NotNull(dbUploadedData.Id);                                                          

            Assert.AreEqual(1,uploadedDataList.Count()); 
            Assert.AreEqual(1, uploadedDataVersionses.Count());
            
            Assert.AreEqual(uploadedData.CreatedAt.ToString(), dbUploadedData.CreatedAt.ToString());                                  
            Assert.AreEqual(uploadedData.Version, dbUploadedData.Version);                                      
            Assert.AreEqual(uploadedData.UpdatedAt.ToString(), dbUploadedData.UpdatedAt.ToString());
            Assert.AreEqual(uploadedData.Checksum, dbUploadedData.Checksum);                      
            Assert.AreEqual(uploadedData.Extension, dbUploadedData.Extension);                      
            Assert.AreEqual(uploadedData.Local, dbUploadedData.Local);
            Assert.AreEqual(uploadedData.CurrentFile, dbUploadedData.CurrentFile); 
            Assert.AreEqual(uploadedData.ExpirationDate.ToString(), dbUploadedData.ExpirationDate.ToString()); 
            Assert.AreEqual(uploadedData.FileLocation, dbUploadedData.FileLocation); 
            Assert.AreEqual(uploadedData.FileName, dbUploadedData.FileName); 
            Assert.AreEqual(uploadedData.TranscriptionId, dbUploadedData.TranscriptionId); 
            Assert.AreEqual(uploadedData.UploaderId, dbUploadedData.UploaderId);
            Assert.AreEqual(uploadedData.UploaderType, dbUploadedData.UploaderType);
            
            Assert.AreEqual(dbUploadedData.WorkflowState, eFormShared.Constants.WorkflowStates.Removed);
        }
    }
}