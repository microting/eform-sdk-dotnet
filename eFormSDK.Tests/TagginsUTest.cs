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
    public class TagginsUTest : DbTestFixture
    {
        [Test]
        public void Taggins_Create_DoesCreate()
        {
            Random rnd = new Random();
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            tags tag = new tags();
            tag.Name = Guid.NewGuid().ToString();
            tag.TaggingsCount = rnd.Next(1, 255);
            tag.Create(DbContext);
            
            check_lists checklist = new check_lists();
            checklist.Color = Guid.NewGuid().ToString();
            checklist.Custom = Guid.NewGuid().ToString();
            checklist.Description = Guid.NewGuid().ToString();
            checklist.Field1 = rnd.Next(1, 255);
            checklist.Field2 = rnd.Next(1, 255);
            checklist.Field3 = rnd.Next(1, 255);
            checklist.Field4 = rnd.Next(1, 255);
            checklist.Field5 = rnd.Next(1, 255);
            checklist.Field6 = rnd.Next(1, 255);
            checklist.Field7 = rnd.Next(1, 255);
            checklist.Field8 = rnd.Next(1, 255);
            checklist.Field9 = rnd.Next(1, 255);
            checklist.Field10 = rnd.Next(1, 255);
            checklist.Label = Guid.NewGuid().ToString();
            checklist.Repeated = rnd.Next(1, 255);
            checklist.ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.CaseType = Guid.NewGuid().ToString();
            checklist.DisplayIndex = rnd.Next(1, 255);
            checklist.DownloadEntities = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.FastNavigation = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.FolderName = Guid.NewGuid().ToString();
            checklist.ManualSync = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.MultiApproval = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.OriginalId = Guid.NewGuid().ToString();
            checklist.ParentId = rnd.Next(1, 255);
            checklist.ReviewEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.DocxExportEnabled = randomBool;
            checklist.DoneButtonEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.ExtraFieldsEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.JasperExportEnabled = randomBool;
            checklist.QuickSyncEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.Create(DbContext);
            
            taggings tagging = new taggings();
            tagging.Tag = tag;
            tagging.CheckList = checklist;
            tagging.TaggerId = rnd.Next(1, 255);
            tagging.TagId = rnd.Next(1, 255);
            tagging.CheckListId = checklist.Id;
            
            
            //Act
            
            tagging.Create(DbContext);

            List<taggings> taggings = DbContext.taggings.AsNoTracking().ToList();
            List<tagging_versions> taggingVersions = DbContext.tagging_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(taggings);                                                             
            Assert.NotNull(taggingVersions);                                                             

            Assert.AreEqual(1,taggings.Count());  
            Assert.AreEqual(1,taggingVersions.Count());  
            
            Assert.AreEqual(tagging.CreatedAt.ToString(), taggings[0].CreatedAt.ToString());                                  
            Assert.AreEqual(tagging.Version, taggings[0].Version);                                      
            Assert.AreEqual(tagging.UpdatedAt.ToString(), taggings[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(taggings[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(tagging.Id, taggings[0].Id);
            Assert.AreEqual(tagging.TaggerId, taggings[0].TaggerId);
            Assert.AreEqual(tagging.TagId, taggings[0].TagId); 
            Assert.AreEqual(tagging.CheckListId, taggings[0].CheckListId); 
            Assert.AreEqual(tagging.CheckListId, checklist.Id);
            Assert.AreEqual(tagging.TagId, tag.Id);

            //Version 1
            Assert.AreEqual(tagging.CreatedAt.ToString(), taggingVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(tagging.Version, taggingVersions[0].Version);                                      
            Assert.AreEqual(tagging.UpdatedAt.ToString(), taggingVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(taggingVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(tagging.Id, taggingVersions[0].Id);
            Assert.AreEqual(tagging.TaggerId, taggingVersions[0].TaggerId);
            Assert.AreEqual(tagging.TagId, taggingVersions[0].TagId); 
            Assert.AreEqual(tagging.CheckListId, taggingVersions[0].CheckListId); 
            Assert.AreEqual(tagging.CheckListId, checklist.Id);
            Assert.AreEqual(tagging.TagId, tag.Id);
        }

        [Test]
        public void Taggings_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange
            
             Random rnd = new Random();
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            tags tag = new tags();
            tag.Name = Guid.NewGuid().ToString();
            tag.TaggingsCount = rnd.Next(1, 255);
            DbContext.tags.Add(tag);
            DbContext.SaveChanges();
            
            check_lists checklist = new check_lists();
            checklist.Color = Guid.NewGuid().ToString();
            checklist.Custom = Guid.NewGuid().ToString();
            checklist.Description = Guid.NewGuid().ToString();
            checklist.Field1 = rnd.Next(1, 255);
            checklist.Field2 = rnd.Next(1, 255);
            checklist.Field3 = rnd.Next(1, 255);
            checklist.Field4 = rnd.Next(1, 255);
            checklist.Field5 = rnd.Next(1, 255);
            checklist.Field6 = rnd.Next(1, 255);
            checklist.Field7 = rnd.Next(1, 255);
            checklist.Field8 = rnd.Next(1, 255);
            checklist.Field9 = rnd.Next(1, 255);
            checklist.Field10 = rnd.Next(1, 255);
            checklist.Label = Guid.NewGuid().ToString();
            checklist.Repeated = rnd.Next(1, 255);
            checklist.ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.CaseType = Guid.NewGuid().ToString();
            checklist.DisplayIndex = rnd.Next(1, 255);
            checklist.DownloadEntities = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.FastNavigation = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.FolderName = Guid.NewGuid().ToString();
            checklist.ManualSync = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.MultiApproval = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.OriginalId = Guid.NewGuid().ToString();
            checklist.ParentId = rnd.Next(1, 255);
            checklist.ReviewEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.DocxExportEnabled = randomBool;
            checklist.DoneButtonEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.ExtraFieldsEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.JasperExportEnabled = randomBool;
            checklist.QuickSyncEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.Create(DbContext);

            taggings tagging = new taggings();
            tagging.Tag = tag;
            tagging.CheckList = checklist;
            tagging.TaggerId = rnd.Next(1, 255);
            tagging.TagId = rnd.Next(1, 255);
            tagging.CheckListId = checklist.Id;
            tagging.Create(DbContext);

            //Act

            DateTime? oldUpdatedAt = tagging.UpdatedAt;

            tagging.Delete(DbContext);
            
            List<taggings> taggings = DbContext.taggings.AsNoTracking().ToList();
            List<tagging_versions> taggingVersions = DbContext.tagging_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(taggings);                                                             
            Assert.NotNull(taggingVersions);                                                             

            Assert.AreEqual(1,taggings.Count());  
            Assert.AreEqual(2,taggingVersions.Count());  
            
            Assert.AreEqual(tagging.CreatedAt.ToString(), taggings[0].CreatedAt.ToString());                                  
            Assert.AreEqual(tagging.Version, taggings[0].Version);                                      
            Assert.AreEqual(tagging.UpdatedAt.ToString(), taggings[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(Constants.WorkflowStates.Removed, taggings[0].WorkflowState);
            Assert.AreEqual(tagging.Id, taggings[0].Id);
            Assert.AreEqual(tagging.TaggerId, taggings[0].TaggerId);
            Assert.AreEqual(tagging.TagId, taggings[0].TagId); 
            Assert.AreEqual(tagging.CheckListId, taggings[0].CheckListId); 
            Assert.AreEqual(tagging.CheckListId, checklist.Id);
            Assert.AreEqual(tagging.TagId, tag.Id);

            //Version 1
            Assert.AreEqual(tagging.CreatedAt.ToString(), taggingVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, taggingVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), taggingVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(Constants.WorkflowStates.Created, taggingVersions[0].WorkflowState);
            Assert.AreEqual(tagging.Id, taggingVersions[0].TaggingId);
            Assert.AreEqual(tagging.TaggerId, taggingVersions[0].TaggerId);
            Assert.AreEqual(tagging.TagId, taggingVersions[0].TagId); 
            Assert.AreEqual(tagging.CheckListId, taggingVersions[0].CheckListId); 
            Assert.AreEqual(tagging.CheckListId, checklist.Id);
            Assert.AreEqual(tagging.TagId, tag.Id);
            
            //Version 2 Deleted Version
            Assert.AreEqual(tagging.CreatedAt.ToString(), taggingVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, taggingVersions[1].Version);                                      
            Assert.AreEqual(tagging.UpdatedAt.ToString(), taggingVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(Constants.WorkflowStates.Removed, taggingVersions[1].WorkflowState);
            Assert.AreEqual(tagging.Id, taggingVersions[1].TaggingId);
            Assert.AreEqual(tagging.TaggerId, taggingVersions[1].TaggerId);
            Assert.AreEqual(tagging.TagId, taggingVersions[1].TagId); 
            Assert.AreEqual(tagging.CheckListId, taggingVersions[1].CheckListId); 
            Assert.AreEqual(tagging.CheckListId, checklist.Id);
            Assert.AreEqual(tagging.TagId, tag.Id);
        }
    }
}