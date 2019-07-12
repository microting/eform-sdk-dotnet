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
            Assert.AreEqual(dbTag.WorkflowState, Constants.WorkflowStates.Created);
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
            
            Assert.AreEqual(dbTag.WorkflowState, Constants.WorkflowStates.Removed);
        }
    }
}