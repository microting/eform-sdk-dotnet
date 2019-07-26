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
            
            Assert.AreEqual(folder.CreatedAt.ToString(), folders[0].CreatedAt.ToString());                                  
            Assert.AreEqual(folder.Version, folders[0].Version);                                      
            Assert.AreEqual(folder.UpdatedAt.ToString(), folders[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(folders[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folders[0].Id);
            Assert.AreEqual(folder.Description, folders[0].Description);
            Assert.AreEqual(folder.Name, folders[0].Name);
            Assert.AreEqual(folder.MicrotingUid, folders[0].MicrotingUid);
            Assert.AreEqual(folder.ParentId, folders[0].ParentId);
            
            //Versions
            
            Assert.AreEqual(folder.CreatedAt.ToString(), folderVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(folder.Version, folderVersions[0].Version);                                      
            Assert.AreEqual(folder.UpdatedAt.ToString(), folderVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(folderVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(folder.Id, folderVersions[0].FolderId);
            Assert.AreEqual(folder.Description, folderVersions[0].Description);
            Assert.AreEqual(folder.Name, folderVersions[0].Name);
            Assert.AreEqual(folder.MicrotingUid, folderVersions[0].MicrotingUid);
            Assert.AreEqual(folder.ParentId, folderVersions[0].ParentId);
        }
    }
}