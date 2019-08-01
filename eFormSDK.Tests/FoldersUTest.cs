using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Models;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class FoldersUTest : DbTestFixture
    {
        [Test]
        public void Folders_Create_DoesCreate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            folders parentFolder = new folders();
            parentFolder.Description = Guid.NewGuid().ToString();
            parentFolder.Name = Guid.NewGuid().ToString();
            parentFolder.MicrotingUid = rnd.Next(1, 255);
            parentFolder.Save(DbContext);
            
            folders folder = new folders();
            folder.Description = Guid.NewGuid().ToString();
            folder.Name = Guid.NewGuid().ToString();
            folder.MicrotingUid = rnd.Next(1, 255);
            folder.ParentId = parentFolder.Id;
            
            //Act
            
            folder.Save(DbContext);
            
            List<folders> folders = DbContext.folders.AsNoTracking().ToList();
            List<folder_versions> folderVersions = DbContext.folder_versions.AsNoTracking().ToList();
            
            Assert.NotNull(folders);                                                             
            Assert.NotNull(folderVersions);                                                             

            Assert.AreEqual(2,folders.Count());  
            Assert.AreEqual(2,folderVersions.Count());  
            
            Assert.AreEqual(folder.CreatedAt.ToString(), folders[1].CreatedAt.ToString());                                  
            Assert.AreEqual(folder.Version, folders[1].Version);                                      
            Assert.AreEqual(folder.UpdatedAt.ToString(), folders[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(folders[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folders[1].Id);
            Assert.AreEqual(folder.Description, folders[1].Description);
            Assert.AreEqual(folder.Name, folders[1].Name);
            Assert.AreEqual(folder.MicrotingUid, folders[1].MicrotingUid);
            Assert.AreEqual(folder.ParentId, parentFolder.Id);
            
            //Versions
            
            Assert.AreEqual(folder.CreatedAt.ToString(), folderVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(1, folderVersions[1].Version);                                      
            Assert.AreEqual(folder.UpdatedAt.ToString(), folderVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(folderVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folderVersions[1].FolderId);
            Assert.AreEqual(folder.Description, folderVersions[1].Description);
            Assert.AreEqual(folder.Name, folderVersions[1].Name);
            Assert.AreEqual(folder.MicrotingUid, folderVersions[1].MicrotingUid);
            Assert.AreEqual(parentFolder.Id, folderVersions[1].ParentId);
        }

        [Test]
        public void Folders_Update_DoesUpdate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            folders parentFolder = new folders();
            parentFolder.Description = Guid.NewGuid().ToString();
            parentFolder.Name = Guid.NewGuid().ToString();
            parentFolder.MicrotingUid = rnd.Next(1, 255);
            parentFolder.Save(DbContext);
            
            folders folder = new folders();
            folder.Description = Guid.NewGuid().ToString();
            folder.Name = Guid.NewGuid().ToString();
            folder.MicrotingUid = rnd.Next(1, 255);
            folder.ParentId = parentFolder.Id;
            folder.Save(DbContext);

            //Act
            DateTime? oldUpdatedAt = folder.UpdatedAt;
            string oldDescription = folder.Description;
            string oldName = folder.Name;
            int? oldMicrotingUid = folder.MicrotingUid;
            
            folder.Description = Guid.NewGuid().ToString();
            folder.Name = Guid.NewGuid().ToString();
            folder.MicrotingUid = rnd.Next(1, 255);
            folder.Update(DbContext);
            
            List<folders> folders = DbContext.folders.AsNoTracking().ToList();
            List<folder_versions> folderVersions = DbContext.folder_versions.AsNoTracking().ToList();
            
            Assert.NotNull(folders);                                                             
            Assert.NotNull(folderVersions);                                                             

            Assert.AreEqual(2,folders.Count());  
            Assert.AreEqual(3,folderVersions.Count());  
            
            Assert.AreEqual(folder.CreatedAt.ToString(), folders[1].CreatedAt.ToString());                                  
            Assert.AreEqual(folder.Version, folders[1].Version);                                      
            Assert.AreEqual(folder.UpdatedAt.ToString(), folders[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(folders[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folders[1].Id);
            Assert.AreEqual(folder.Description, folders[1].Description);
            Assert.AreEqual(folder.Name, folders[1].Name);
            Assert.AreEqual(folder.MicrotingUid, folders[1].MicrotingUid);
            Assert.AreEqual(folder.ParentId, parentFolder.Id);
            
            //Old Version
            
            Assert.AreEqual(folder.CreatedAt.ToString(), folderVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(1, folderVersions[1].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), folderVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(folderVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folderVersions[1].FolderId);
            Assert.AreEqual(oldDescription, folderVersions[1].Description);
            Assert.AreEqual(oldName, folderVersions[1].Name);
            Assert.AreEqual(oldMicrotingUid, folderVersions[1].MicrotingUid);
            Assert.AreEqual(parentFolder.Id, folderVersions[1].ParentId);
            
            //New Version
            
            Assert.AreEqual(folder.CreatedAt.ToString(), folderVersions[2].CreatedAt.ToString());                                  
            Assert.AreEqual(2, folderVersions[2].Version);                                      
            Assert.AreEqual(folder.UpdatedAt.ToString(), folderVersions[2].UpdatedAt.ToString());                                  
            Assert.AreEqual(folderVersions[2].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folderVersions[2].FolderId);
            Assert.AreEqual(folder.Description, folderVersions[2].Description);
            Assert.AreEqual(folder.Name, folderVersions[2].Name);
            Assert.AreEqual(folder.MicrotingUid, folderVersions[2].MicrotingUid);
            Assert.AreEqual(parentFolder.Id, folderVersions[2].ParentId);
        }

        [Test]
        public void Folders_Delete_DoesSetWorkflowStateToRemoved()
        {
             //Arrange
            
            Random rnd = new Random();
            
            folders parentFolder = new folders();
            parentFolder.Description = Guid.NewGuid().ToString();
            parentFolder.Name = Guid.NewGuid().ToString();
            parentFolder.MicrotingUid = rnd.Next(1, 255);
            parentFolder.Save(DbContext);
            
            folders folder = new folders();
            folder.Description = Guid.NewGuid().ToString();
            folder.Name = Guid.NewGuid().ToString();
            folder.MicrotingUid = rnd.Next(1, 255);
            folder.ParentId = parentFolder.Id;
            folder.Save(DbContext);

            //Act
            
            folder.Delete(DbContext);
            
            List<folders> folders = DbContext.folders.AsNoTracking().ToList();
            List<folder_versions> folderVersions = DbContext.folder_versions.AsNoTracking().ToList();
            
            Assert.NotNull(folders);                                                             
            Assert.NotNull(folderVersions);                                                             

            Assert.AreEqual(2,folders.Count());  
            Assert.AreEqual(3,folderVersions.Count());  
            
            Assert.AreEqual(folder.CreatedAt.ToString(), folders[1].CreatedAt.ToString());                                  
            Assert.AreEqual(folder.Version, folders[1].Version);                                      
            Assert.AreEqual(folder.UpdatedAt.ToString(), folders[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(folders[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(folder.Id, folders[1].Id);
            Assert.AreEqual(folder.Description, folders[1].Description);
            Assert.AreEqual(folder.Name, folders[1].Name);
            Assert.AreEqual(folder.MicrotingUid, folders[1].MicrotingUid);
            Assert.AreEqual(folder.ParentId, parentFolder.Id);
            
            //Old Version
            
            Assert.AreEqual(folder.CreatedAt.ToString(), folderVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(1, folderVersions[1].Version);                                      
            Assert.AreEqual(folder.UpdatedAt.ToString(), folderVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(folderVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folderVersions[1].FolderId);
            Assert.AreEqual(folder.Description, folderVersions[1].Description);
            Assert.AreEqual(folder.Name, folderVersions[1].Name);
            Assert.AreEqual(folder.MicrotingUid, folderVersions[1].MicrotingUid);
            Assert.AreEqual(parentFolder.Id, folderVersions[1].ParentId);
            
            //New Version
            
            Assert.AreEqual(folder.CreatedAt.ToString(), folderVersions[2].CreatedAt.ToString());                                  
            Assert.AreEqual(2, folderVersions[2].Version);                                      
            Assert.AreEqual(folder.UpdatedAt.ToString(), folderVersions[2].UpdatedAt.ToString());                                  
            Assert.AreEqual(folderVersions[2].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(folder.Id, folderVersions[2].FolderId);
            Assert.AreEqual(folder.Description, folderVersions[2].Description);
            Assert.AreEqual(folder.Name, folderVersions[2].Name);
            Assert.AreEqual(folder.MicrotingUid, folderVersions[2].MicrotingUid);
            Assert.AreEqual(parentFolder.Id, folderVersions[2].ParentId);
        }
    }
}