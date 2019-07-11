using System;
using System.Collections.Generic;
using System.Linq;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class TagsUTest : DbTestFixture
    {
        [Test]
        public void Tags_Create_DoesCreate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            tags tag = new tags();
            tag.Name = Guid.NewGuid().ToString();
            tag.TaggingsCount = rnd.Next(1, 255);
            
            //Act
            
            tag.Create(DbContext);

            tags dbTag = DbContext.tags.AsNoTracking().First();
            List<tags> tagsList = DbContext.tags.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbTag);                                                             
            Assert.NotNull(dbTag.Id);                                                          

            Assert.AreEqual(1,tagsList.Count());  
            
            Assert.AreEqual(tag.CreatedAt.ToString(), dbTag.CreatedAt.ToString());                                  
            Assert.AreEqual(tag.Version, dbTag.Version);                                      
            Assert.AreEqual(tag.UpdatedAt.ToString(), dbTag.UpdatedAt.ToString());                                  
            Assert.AreEqual(dbTag.WorkflowState, eFormShared.Constants.WorkflowStates.Created);
            Assert.AreEqual(tag.Name, dbTag.Name);
            Assert.AreEqual(tag.Id, dbTag.Id);
        }

        [Test]
        public void Tags_Update_DoesUpdate()
        {
            Random rnd = new Random();
            
            tags tag = new tags();
            tag.Name = Guid.NewGuid().ToString();
            tag.TaggingsCount = rnd.Next(1, 255);

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();
            
            //Act

            tag.Name = Guid.NewGuid().ToString();
            tag.TaggingsCount = rnd.Next(1, 255);
            
            tag.Update(DbContext);
            
            tags dbTag = DbContext.tags.AsNoTracking().First();
            List<tags> tagsList = DbContext.tags.AsNoTracking().ToList();
            List<tag_versions> tagsVersion = DbContext.tag_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbTag);                                                             
            Assert.NotNull(dbTag.Id);                                                          

            Assert.AreEqual(1,tagsList.Count()); 
            Assert.AreEqual(1, tagsVersion.Count());
            
            Assert.AreEqual(tag.CreatedAt.ToString(), dbTag.CreatedAt.ToString());                                  
            Assert.AreEqual(tag.Version, dbTag.Version);                                      
            Assert.AreEqual(tag.UpdatedAt.ToString(), dbTag.UpdatedAt.ToString());                                  
            Assert.AreEqual(tag.Name, dbTag.Name);
            Assert.AreEqual(tag.Id, dbTag.Id);
        }

        [Test]
        public void Tags_Delete_DoesSetWorkflowStateToRemoved()
        {
            Random rnd = new Random();
            
            tags tag = new tags();
            tag.Name = Guid.NewGuid().ToString();
            tag.TaggingsCount = rnd.Next(1, 255);

            DbContext.tags.Add(tag);
            DbContext.SaveChanges();
            
            //Act
            
            tag.Delete(DbContext);
            
            tags dbTag = DbContext.tags.AsNoTracking().First();
            List<tags> tagsList = DbContext.tags.AsNoTracking().ToList();
            List<tag_versions> tagsVersion = DbContext.tag_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbTag);                                                             
            Assert.NotNull(dbTag.Id);                                                          

            Assert.AreEqual(1,tagsList.Count()); 
            Assert.AreEqual(1, tagsVersion.Count());
            
            Assert.AreEqual(tag.CreatedAt.ToString(), dbTag.CreatedAt.ToString());                                  
            Assert.AreEqual(tag.Version, dbTag.Version);                                      
            Assert.AreEqual(tag.UpdatedAt.ToString(), dbTag.UpdatedAt.ToString());                                  
            Assert.AreEqual(tag.Name, dbTag.Name);
            Assert.AreEqual(tag.Id, dbTag.Id);
            
            Assert.AreEqual(dbTag.WorkflowState, eFormShared.Constants.WorkflowStates.Removed);
        }
    }
}