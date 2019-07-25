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
    public class EntityItemsUTest : DbTestFixture
    {
        [Test]
        public void EntityItems_Create_DoesCreate()
        {
            //Arrange
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            Random rnd = new Random();
            
            entity_groups entityGroup = new entity_groups();
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            
            entity_items entityItem = new entity_items();
            entityItem.Description = Guid.NewGuid().ToString();
            entityItem.Name = Guid.NewGuid().ToString();
            entityItem.Synced = (short)rnd.Next(shortMinValue, shortmaxValue);
            entityItem.DisplayIndex = rnd.Next(1, 255);
            entityItem.MicrotingUid = Guid.NewGuid().ToString();
            entityItem.EntityItemUid = Guid.NewGuid().ToString();
            entityItem.EntityGroupId = entityGroup.Id;
            
            //Act
            
            entityItem.Create(DbContext);
            
            List<entity_items> entityItems = DbContext.entity_items.AsNoTracking().ToList();
            List<entity_item_versions> entityItemVersion= DbContext.entity_item_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(entityItems);                                                             
            Assert.NotNull(entityItemVersion);                                                             

            Assert.AreEqual(1,entityItems.Count());  
            Assert.AreEqual(1,entityItemVersion.Count());
            
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItems[0].CreatedAt.ToString());                                  
            Assert.AreEqual(entityItem.Version, entityItems[0].Version);                                      
            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItems[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityItems[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityItem.Id, entityItems[0].Id);
            Assert.AreEqual(entityItem.Name, entityItems[0].Name);
            Assert.AreEqual(entityItem.Description, entityItems[0].Description);
            Assert.AreEqual(entityItem.Synced, entityItems[0].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItems[0].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItems[0].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItems[0].MicrotingUid);
            Assert.AreEqual(entityItem.EntityGroupId, entityItems[0].EntityGroupId);
            
            //Versions
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItemVersion[0].CreatedAt.ToString());                                  
            Assert.AreEqual(entityItem.Version, entityItemVersion[0].Version);                                      
            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItemVersion[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityItemVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityItem.Id, entityItemVersion[0].EntityItemsId);
            Assert.AreEqual(entityItem.Name, entityItemVersion[0].Name);
            Assert.AreEqual(entityItem.Description, entityItems[0].Description);
            Assert.AreEqual(entityItem.Synced, entityItems[0].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItems[0].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItems[0].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItemVersion[0].MicrotingUid);
            Assert.AreEqual(entityItem.EntityGroupId, entityItems[0].EntityGroupId);
        }

        [Test]
        public void EntityItems_Update_DoesUpdate()
        {
            //Arrange
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            Random rnd = new Random();
            
            entity_groups entityGroup = new entity_groups();
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            
            entity_items entityItem = new entity_items();
            entityItem.Description = Guid.NewGuid().ToString();
            entityItem.Name = Guid.NewGuid().ToString();
            entityItem.Synced = (short)rnd.Next(shortMinValue, shortmaxValue);
            entityItem.DisplayIndex = rnd.Next(1, 255);
            entityItem.MicrotingUid = Guid.NewGuid().ToString();
            entityItem.EntityItemUid = Guid.NewGuid().ToString();
            entityItem.EntityGroupId = entityGroup.Id;
            entityItem.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = entityItem.UpdatedAt;
            string oldDescription = entityItem.Description;
            string oldName = entityItem.Name;
            short? oldSynced = entityItem.Synced;
            int oldDisplayIndex = entityItem.DisplayIndex;
            string oldMicrotingUid = entityItem.MicrotingUid;
            string oldEntityItemUid = entityItem.EntityItemUid;
            
            entityItem.Description = Guid.NewGuid().ToString();
            entityItem.Name = Guid.NewGuid().ToString();
            entityItem.Synced = (short)rnd.Next(shortMinValue, shortmaxValue);
            entityItem.DisplayIndex = rnd.Next(1, 255);
            entityItem.MicrotingUid = Guid.NewGuid().ToString();
            entityItem.EntityItemUid = Guid.NewGuid().ToString();

            entityItem.Update(DbContext);
            
            List<entity_items> entityItems = DbContext.entity_items.AsNoTracking().ToList();
            List<entity_item_versions> entityItemVersion= DbContext.entity_item_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(entityItems);                                                             
            Assert.NotNull(entityItemVersion);                                                             

            Assert.AreEqual(1,entityItems.Count());  
            Assert.AreEqual(2,entityItemVersion.Count());
            
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItems[0].CreatedAt.ToString());                                  
            Assert.AreEqual(entityItem.Version, entityItems[0].Version);                                      
            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItems[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityItems[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityItem.Id, entityItems[0].Id);
            Assert.AreEqual(entityItem.Name, entityItems[0].Name);
            Assert.AreEqual(entityItem.Description, entityItems[0].Description);
            Assert.AreEqual(entityItem.Synced, entityItems[0].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItems[0].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItems[0].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItems[0].MicrotingUid);
            Assert.AreEqual(entityItem.EntityGroupId, entityItems[0].EntityGroupId);
            
            //Old Version
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItemVersion[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, entityItemVersion[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), entityItemVersion[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityItemVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityItem.Id, entityItemVersion[0].EntityItemsId);
            Assert.AreEqual(oldName, entityItemVersion[0].Name);
            Assert.AreEqual(oldDescription, entityItemVersion[0].Description);
            Assert.AreEqual(oldSynced, entityItemVersion[0].Synced);
            Assert.AreEqual(oldDisplayIndex, entityItemVersion[0].DisplayIndex);
            Assert.AreEqual(oldEntityItemUid, entityItemVersion[0].EntityItemUid);
            Assert.AreEqual(oldMicrotingUid, entityItemVersion[0].MicrotingUid);
            Assert.AreEqual(entityItem.EntityGroupId, entityItemVersion[0].EntityGroupId);
            
            //New Version
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItemVersion[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, entityItemVersion[1].Version);                                      
            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItemVersion[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityItemVersion[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityItem.Id, entityItemVersion[1].EntityItemsId);
            Assert.AreEqual(entityItem.Name, entityItemVersion[1].Name);
            Assert.AreEqual(entityItem.Description, entityItemVersion[1].Description);
            Assert.AreEqual(entityItem.Synced, entityItemVersion[1].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItemVersion[1].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItemVersion[1].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItemVersion[1].MicrotingUid);
            Assert.AreEqual(entityItem.EntityGroupId, entityItemVersion[1].EntityGroupId);
        }

        [Test]
        public void EntityItems_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            Random rnd = new Random();
            
            entity_groups entityGroup = new entity_groups();
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            
            entity_items entityItem = new entity_items();
            entityItem.Description = Guid.NewGuid().ToString();
            entityItem.Name = Guid.NewGuid().ToString();
            entityItem.Synced = (short)rnd.Next(shortMinValue, shortmaxValue);
            entityItem.DisplayIndex = rnd.Next(1, 255);
            entityItem.MicrotingUid = Guid.NewGuid().ToString();
            entityItem.EntityItemUid = Guid.NewGuid().ToString();
            entityItem.EntityGroupId = entityGroup.Id;
            entityItem.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = entityItem.UpdatedAt;

            entityItem.Delete(DbContext);
            
            List<entity_items> entityItems = DbContext.entity_items.AsNoTracking().ToList();
            List<entity_item_versions> entityItemVersion= DbContext.entity_item_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(entityItems);                                                             
            Assert.NotNull(entityItemVersion);                                                             

            Assert.AreEqual(1,entityItems.Count());  
            Assert.AreEqual(2,entityItemVersion.Count());
            
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItems[0].CreatedAt.ToString());                                  
            Assert.AreEqual(entityItem.Version, entityItems[0].Version);                                      
            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItems[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityItems[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(entityItem.Id, entityItems[0].Id);
            Assert.AreEqual(entityItem.Name, entityItems[0].Name);
            Assert.AreEqual(entityItem.Description, entityItems[0].Description);
            Assert.AreEqual(entityItem.Synced, entityItems[0].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItems[0].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItems[0].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItems[0].MicrotingUid);
            Assert.AreEqual(entityItem.EntityGroupId, entityItems[0].EntityGroupId);
            
            //Old Version
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItemVersion[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, entityItemVersion[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), entityItemVersion[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityItemVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityItem.Id, entityItemVersion[0].EntityItemsId);
            Assert.AreEqual(entityItem.Name, entityItemVersion[0].Name);
            Assert.AreEqual(entityItem.Description, entityItemVersion[0].Description);
            Assert.AreEqual(entityItem.Synced, entityItemVersion[0].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItemVersion[0].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItemVersion[0].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItemVersion[0].MicrotingUid);
            Assert.AreEqual(entityItem.EntityGroupId, entityItemVersion[0].EntityGroupId);
            
            //New Version
            Assert.AreEqual(entityItem.CreatedAt.ToString(), entityItemVersion[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, entityItemVersion[1].Version);                                      
            Assert.AreEqual(entityItem.UpdatedAt.ToString(), entityItemVersion[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityItemVersion[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(entityItem.Id, entityItemVersion[1].EntityItemsId);
            Assert.AreEqual(entityItem.Name, entityItemVersion[1].Name);
            Assert.AreEqual(entityItem.Description, entityItemVersion[1].Description);
            Assert.AreEqual(entityItem.Synced, entityItemVersion[1].Synced);
            Assert.AreEqual(entityItem.DisplayIndex, entityItemVersion[1].DisplayIndex);
            Assert.AreEqual(entityItem.EntityItemUid, entityItemVersion[1].EntityItemUid);
            Assert.AreEqual(entityItem.MicrotingUid, entityItemVersion[1].MicrotingUid);
            Assert.AreEqual(entityItem.EntityGroupId, entityItemVersion[1].EntityGroupId);
        }
    }
}