using System;
using System.Collections.Generic;
using System.Linq;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
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
            Assert.AreEqual(dbTagging.WorkflowState, eFormShared.Constants.WorkflowStates.Created);
            Assert.AreEqual(tagging.Id, dbTagging.Id);
            Assert.AreEqual(tagging.Tag, dbTagging.Tag);
            Assert.AreEqual(tagging.CheckList, dbTagging.CheckList);
            Assert.AreEqual(tagging.TaggerId, dbTagging.TaggerId);
            Assert.AreEqual(tagging.TagId, dbTagging.TagId);
            Assert.AreEqual(tagging.CheckListId, dbTagging.CheckListId);
            
        }
    }
}