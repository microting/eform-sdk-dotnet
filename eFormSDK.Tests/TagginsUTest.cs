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

            taggings dbTagging = DbContext.taggings.AsNoTracking().First();
            List<taggings> taggingsList = DbContext.taggings.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbTagging);                                                             
            Assert.NotNull(dbTagging.Id);                                                          

            Assert.AreEqual(1,taggingsList.Count());  
            
            Assert.AreEqual(tagging.CreatedAt.ToString(), dbTagging.CreatedAt.ToString());                                  
            Assert.AreEqual(tagging.Version, dbTagging.Version);                                      
            Assert.AreEqual(tagging.UpdatedAt.ToString(), dbTagging.UpdatedAt.ToString());                                  
            Assert.AreEqual(dbTagging.WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(tagging.Id, dbTagging.Id);
            //Assert.AreEqual(tagging.Tag, dbTagging.Tag); //TODO
            //Assert.AreEqual(tagging.CheckList, dbTagging.CheckList); //TODO
            Assert.AreEqual(tagging.TaggerId, dbTagging.TaggerId);
            Assert.AreEqual(tagging.TagId, dbTagging.TagId); //TODO
            Assert.AreEqual(tagging.CheckListId, dbTagging.CheckListId); //TODO
            Assert.AreEqual(tagging.CheckListId, checklist.Id);
            Assert.AreEqual(tagging.TagId, tag.Id);
            
        }
    }
}