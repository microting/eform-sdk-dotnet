using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class EntityGroupsUTest : DbTestFixture
    {
        [Test]
        public async Task EntityGroups_Create_DoesCreate()
        {
            entity_groups entityGroup = new entity_groups();
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            
            //Act
            
            entityGroup.Create(DbContext);
            
            List<entity_groups> entityGroups = DbContext.entity_groups.AsNoTracking().ToList();
            List<entity_group_versions> entityGroupVersion = DbContext.entity_group_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(entityGroups);                                                             
            Assert.NotNull(entityGroupVersion);                                                             

            Assert.AreEqual(1,entityGroups.Count());  
            Assert.AreEqual(1,entityGroupVersion.Count());
            
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroups[0].CreatedAt.ToString());                                  
            Assert.AreEqual(entityGroup.Version, entityGroups[0].Version);                                      
            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroups[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityGroups[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityGroup.Id, entityGroups[0].Id);
            Assert.AreEqual(entityGroup.Name, entityGroups[0].Name);
            Assert.AreEqual(entityGroup.Type, entityGroups[0].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroups[0].MicrotingUid);
            
            //Versions
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroupVersion[0].CreatedAt.ToString());                                  
            Assert.AreEqual(entityGroup.Version, entityGroupVersion[0].Version);                                      
            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroupVersion[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityGroupVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityGroup.Id, entityGroupVersion[0].Id);
            Assert.AreEqual(entityGroup.Name, entityGroupVersion[0].Name);
            Assert.AreEqual(entityGroup.Type, entityGroupVersion[0].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroupVersion[0].MicrotingUid);
        }

        [Test]
        public async Task EntityGroups_Update_DoesUpdate()
        {
            entity_groups entityGroup = new entity_groups();
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            entityGroup.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = entityGroup.UpdatedAt;
            string oldName = entityGroup.Name;
            string oldType = entityGroup.Type;
            string oldMicrotingUid = entityGroup.MicrotingUid;
            
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            
            entityGroup.Update(DbContext);
            
            List<entity_groups> entityGroups = DbContext.entity_groups.AsNoTracking().ToList();
            List<entity_group_versions> entityGroupVersion = DbContext.entity_group_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(entityGroups);                                                             
            Assert.NotNull(entityGroupVersion);                                                             

            Assert.AreEqual(1,entityGroups.Count());  
            Assert.AreEqual(2,entityGroupVersion.Count());
            
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroups[0].CreatedAt.ToString());                                  
            Assert.AreEqual(entityGroup.Version, entityGroups[0].Version);                                      
            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroups[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityGroups[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityGroup.Id, entityGroups[0].Id);
            Assert.AreEqual(entityGroup.Name, entityGroups[0].Name);
            Assert.AreEqual(entityGroup.Type, entityGroups[0].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroups[0].MicrotingUid);
            
            //Old Version
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroupVersion[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, entityGroupVersion[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), entityGroupVersion[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityGroupVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityGroup.Id, entityGroupVersion[0].EntityGroupId);
            Assert.AreEqual(oldName, entityGroupVersion[0].Name);
            Assert.AreEqual(oldType, entityGroupVersion[0].Type);
            Assert.AreEqual(oldMicrotingUid, entityGroupVersion[0].MicrotingUid);
            
            //New Version
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroupVersion[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, entityGroupVersion[1].Version);                                      
            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroupVersion[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityGroupVersion[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityGroup.Id, entityGroupVersion[1].EntityGroupId);
            Assert.AreEqual(entityGroup.Name, entityGroupVersion[1].Name);
            Assert.AreEqual(entityGroup.Type, entityGroupVersion[1].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroupVersion[1].MicrotingUid);
        }

        [Test]
        public async Task EntityGroups_Delete_DoesSetWorkflowStateToRemoved()
        {
            entity_groups entityGroup = new entity_groups();
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            entityGroup.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = entityGroup.UpdatedAt;

            entityGroup.Delete(DbContext);
            
            List<entity_groups> entityGroups = DbContext.entity_groups.AsNoTracking().ToList();
            List<entity_group_versions> entityGroupVersion = DbContext.entity_group_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(entityGroups);                                                             
            Assert.NotNull(entityGroupVersion);                                                             

            Assert.AreEqual(1,entityGroups.Count());  
            Assert.AreEqual(2,entityGroupVersion.Count());
            
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroups[0].CreatedAt.ToString());                                  
            Assert.AreEqual(entityGroup.Version, entityGroups[0].Version);                                      
            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroups[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityGroups[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(entityGroup.Id, entityGroups[0].Id);
            Assert.AreEqual(entityGroup.Name, entityGroups[0].Name);
            Assert.AreEqual(entityGroup.Type, entityGroups[0].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroups[0].MicrotingUid);
            
            //Old Version
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroupVersion[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, entityGroupVersion[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), entityGroupVersion[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityGroupVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(entityGroup.Id, entityGroupVersion[0].EntityGroupId);
            Assert.AreEqual(entityGroup.Name, entityGroupVersion[0].Name);
            Assert.AreEqual(entityGroup.Type, entityGroupVersion[0].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroupVersion[0].MicrotingUid);
            
            //New Version
            Assert.AreEqual(entityGroup.CreatedAt.ToString(), entityGroupVersion[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, entityGroupVersion[1].Version);                                      
            Assert.AreEqual(entityGroup.UpdatedAt.ToString(), entityGroupVersion[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(entityGroupVersion[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(entityGroup.Id, entityGroupVersion[1].EntityGroupId);
            Assert.AreEqual(entityGroup.Name, entityGroupVersion[1].Name);
            Assert.AreEqual(entityGroup.Type, entityGroupVersion[1].Type);
            Assert.AreEqual(entityGroup.MicrotingUid, entityGroupVersion[1].MicrotingUid);
        }
    }
}