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
    public class CheckListsUTest : DbTestFixture
    {
        [Test]
        public void Checklists_create_DoesCreate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            check_lists checklistParent = new check_lists();
            checklistParent.Color = Guid.NewGuid().ToString();
            checklistParent.Custom = Guid.NewGuid().ToString();
            checklistParent.Description = Guid.NewGuid().ToString();
            checklistParent.Field1 = rnd.Next(1, 255);
            checklistParent.Field2 = rnd.Next(1, 255);
            checklistParent.Field4 = rnd.Next(1, 255);
            checklistParent.Field5 = rnd.Next(1, 255);
            checklistParent.Field6 = rnd.Next(1, 255);
            checklistParent.Field7 = rnd.Next(1, 255);
            checklistParent.Field8 = rnd.Next(1, 255);
            checklistParent.Field9 = rnd.Next(1, 255);
            checklistParent.Field10 = rnd.Next(1, 255);
            checklistParent.Label = Guid.NewGuid().ToString();
            checklistParent.Repeated = rnd.Next(1, 255);
            checklistParent.ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.CaseType = Guid.NewGuid().ToString();
            checklistParent.DisplayIndex = rnd.Next(1, 255);
            checklistParent.DownloadEntities = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.FastNavigation = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.FolderName = Guid.NewGuid().ToString();
            checklistParent.ManualSync = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.MultiApproval = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.OriginalId = Guid.NewGuid().ToString();
            checklistParent.ReviewEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.DocxExportEnabled = randomBool;
            checklistParent.DoneButtonEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.ExtraFieldsEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.JasperExportEnabled = randomBool;
            checklistParent.QuickSyncEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.Create(DbContext);
            
            check_lists checklist = new check_lists();
            checklist.Color = Guid.NewGuid().ToString();
            checklist.Custom = Guid.NewGuid().ToString();
            checklist.Description = Guid.NewGuid().ToString();
            checklist.Field1 = rnd.Next(1, 255);
            checklist.Field2 = rnd.Next(1, 255);
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
            checklist.ReviewEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.DocxExportEnabled = randomBool;
            checklist.DoneButtonEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.ExtraFieldsEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.JasperExportEnabled = randomBool;
            checklist.QuickSyncEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.ParentId = checklistParent.Id;
            
            //Act
            
            checklist.Create(DbContext);
            
            List<check_lists> checkLists = DbContext.check_lists.AsNoTracking().ToList();
            List<check_list_versions> checkListVersion = DbContext.check_list_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(checkLists);                                                             
            Assert.NotNull(checkListVersion);                                                             

            Assert.AreEqual(2,checkLists.Count());  
            Assert.AreEqual(2,checkListVersion.Count());  
            
            Assert.AreEqual(checklist.CreatedAt.ToString(), checkLists[1].CreatedAt.ToString());                                  
            Assert.AreEqual(checklist.Version, checkLists[1].Version);                                      
            Assert.AreEqual(checklist.UpdatedAt.ToString(), checkLists[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkLists[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checklist.Id, checkLists[1].Id);
            Assert.AreEqual(checklist.Color, checkLists[1].Color);
            Assert.AreEqual(checklist.Custom, checkLists[1].Custom);
            Assert.AreEqual(checklist.Description, checkLists[1].Description);
            Assert.AreEqual(checklist.Field1, checkLists[1].Field1);
            Assert.AreEqual(checklist.Field2, checkLists[1].Field2);
            Assert.AreEqual(checklist.Field3, checkLists[1].Field3);
            Assert.AreEqual(checklist.Field4, checkLists[1].Field4);
            Assert.AreEqual(checklist.Field5, checkLists[1].Field5);
            Assert.AreEqual(checklist.Field6, checkLists[1].Field6);
            Assert.AreEqual(checklist.Field7, checkLists[1].Field7);
            Assert.AreEqual(checklist.Field8, checkLists[1].Field8);
            Assert.AreEqual(checklist.Field9, checkLists[1].Field9);
            Assert.AreEqual(checklist.Field10, checkLists[1].Field10);
            Assert.AreEqual(checklist.Label, checkLists[1].Label);
            Assert.AreEqual(checklist.Repeated, checkLists[1].Repeated);
            Assert.AreEqual(checklist.ApprovalEnabled, checkLists[1].ApprovalEnabled);
            Assert.AreEqual(checklist.CaseType, checkLists[1].CaseType);
            Assert.AreEqual(checklist.DisplayIndex, checkLists[1].DisplayIndex);
            Assert.AreEqual(checklist.DownloadEntities, checkLists[1].DownloadEntities);
            Assert.AreEqual(checklist.FastNavigation, checkLists[1].FastNavigation);
            Assert.AreEqual(checklist.FolderName, checkLists[1].FolderName);
            Assert.AreEqual(checklist.ManualSync, checkLists[1].ManualSync);
            Assert.AreEqual(checklist.MultiApproval, checkLists[1].MultiApproval);
            Assert.AreEqual(checklist.OriginalId, checkLists[1].OriginalId);
            Assert.AreEqual(checklist.ParentId, checkLists[1].ParentId);
            Assert.AreEqual(checklist.ReviewEnabled, checkLists[1].ReviewEnabled);
            Assert.AreEqual(checklist.DocxExportEnabled, checkLists[1].DocxExportEnabled);
            Assert.AreEqual(checklist.DoneButtonEnabled, checkLists[1].DoneButtonEnabled);
            Assert.AreEqual(checklist.ExtraFieldsEnabled, checkLists[1].ExtraFieldsEnabled);
            Assert.AreEqual(checklist.JasperExportEnabled, checkLists[1].JasperExportEnabled);
            Assert.AreEqual(checklist.QuickSyncEnabled, checkLists[1].QuickSyncEnabled);
            
            //Versions
            Assert.AreEqual(checklist.CreatedAt.ToString(), checkListVersion[1].CreatedAt.ToString());                                  
            Assert.AreEqual(checklist.Version, checkListVersion[1].Version);                                      
            Assert.AreEqual(checklist.UpdatedAt.ToString(), checkListVersion[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListVersion[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checklist.Id, checkListVersion[1].Id);
            Assert.AreEqual(checklist.Color, checkListVersion[1].Color);
            Assert.AreEqual(checklist.Custom, checkListVersion[1].Custom);
            Assert.AreEqual(checklist.Description, checkListVersion[1].Description);
            Assert.AreEqual(checklist.Field1, checkListVersion[1].Field1);
            Assert.AreEqual(checklist.Field2, checkListVersion[1].Field2);
            Assert.AreEqual(checklist.Field3, checkListVersion[1].Field3);
            Assert.AreEqual(checklist.Field4, checkListVersion[1].Field4);
            Assert.AreEqual(checklist.Field5, checkListVersion[1].Field5);
            Assert.AreEqual(checklist.Field6, checkListVersion[1].Field6);
            Assert.AreEqual(checklist.Field7, checkListVersion[1].Field7);
            Assert.AreEqual(checklist.Field8, checkListVersion[1].Field8);
            Assert.AreEqual(checklist.Field9, checkListVersion[1].Field9);
            Assert.AreEqual(checklist.Field10, checkListVersion[1].Field10);
            Assert.AreEqual(checklist.Label, checkListVersion[1].Label);
            Assert.AreEqual(checklist.Repeated, checkListVersion[1].Repeated);
            Assert.AreEqual(checklist.ApprovalEnabled, checkListVersion[1].ApprovalEnabled);
            Assert.AreEqual(checklist.CaseType, checkListVersion[1].CaseType);
            Assert.AreEqual(checklist.DisplayIndex, checkListVersion[1].DisplayIndex);
            Assert.AreEqual(checklist.DownloadEntities, checkListVersion[1].DownloadEntities);
            Assert.AreEqual(checklist.FastNavigation, checkListVersion[1].FastNavigation);
            Assert.AreEqual(checklist.FolderName, checkListVersion[1].FolderName);
            Assert.AreEqual(checklist.ManualSync, checkListVersion[1].ManualSync);
            Assert.AreEqual(checklist.MultiApproval, checkListVersion[1].MultiApproval);
            Assert.AreEqual(checklist.OriginalId, checkListVersion[1].OriginalId);
            Assert.AreEqual(checklist.ParentId, checkListVersion[1].ParentId);
            Assert.AreEqual(checklist.ReviewEnabled, checkListVersion[1].ReviewEnabled);
            Assert.AreEqual(checklist.DocxExportEnabled, checkListVersion[1].DocxExportEnabled);
            Assert.AreEqual(checklist.DoneButtonEnabled, checkListVersion[1].DoneButtonEnabled);
            Assert.AreEqual(checklist.ExtraFieldsEnabled, checkListVersion[1].ExtraFieldsEnabled);
            Assert.AreEqual(checklist.JasperExportEnabled, checkListVersion[1].JasperExportEnabled);
            Assert.AreEqual(checklist.QuickSyncEnabled, checkListVersion[1].QuickSyncEnabled);
        }

        [Test]
        public void Checklists_Update_DoesUpdate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            check_lists checklistParent = new check_lists();
            checklistParent.Color = Guid.NewGuid().ToString();
            checklistParent.Custom = Guid.NewGuid().ToString();
            checklistParent.Description = Guid.NewGuid().ToString();
            checklistParent.Field1 = rnd.Next(1, 255);
            checklistParent.Field2 = rnd.Next(1, 255);
            checklistParent.Field4 = rnd.Next(1, 255);
            checklistParent.Field5 = rnd.Next(1, 255);
            checklistParent.Field6 = rnd.Next(1, 255);
            checklistParent.Field7 = rnd.Next(1, 255);
            checklistParent.Field8 = rnd.Next(1, 255);
            checklistParent.Field9 = rnd.Next(1, 255);
            checklistParent.Field10 = rnd.Next(1, 255);
            checklistParent.Label = Guid.NewGuid().ToString();
            checklistParent.Repeated = rnd.Next(1, 255);
            checklistParent.ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.CaseType = Guid.NewGuid().ToString();
            checklistParent.DisplayIndex = rnd.Next(1, 255);
            checklistParent.DownloadEntities = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.FastNavigation = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.FolderName = Guid.NewGuid().ToString();
            checklistParent.ManualSync = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.MultiApproval = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.OriginalId = Guid.NewGuid().ToString();
            checklistParent.ReviewEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.DocxExportEnabled = randomBool;
            checklistParent.DoneButtonEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.ExtraFieldsEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.JasperExportEnabled = randomBool;
            checklistParent.QuickSyncEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.Create(DbContext);
            
            check_lists checklist = new check_lists();
            checklist.Color = Guid.NewGuid().ToString();
            checklist.Custom = Guid.NewGuid().ToString();
            checklist.Description = Guid.NewGuid().ToString();
            checklist.Field1 = rnd.Next(1, 255);
            checklist.Field2 = rnd.Next(1, 255);
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
            checklist.ReviewEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.DocxExportEnabled = randomBool;
            checklist.DoneButtonEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.ExtraFieldsEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.JasperExportEnabled = randomBool;
            checklist.QuickSyncEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.ParentId = checklistParent.Id;
            checklist.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = checklist.UpdatedAt;
            string oldColour = checklist.Color;
            string oldCustom = checklist.Custom;
            string oldDescription = checklist.Description;
            int? oldField1 = checklist.Field1;
            int? oldField2 = checklist.Field2;
            int? oldField3 = checklist.Field3;
            int? oldField4 = checklist.Field4;
            int? oldField5 = checklist.Field5;
            int? oldField6 = checklist.Field6;
            int? oldField7 = checklist.Field7;
            int? oldField8 = checklist.Field8;
            int? oldField9 = checklist.Field9;
            int? oldField10 = checklist.Field10;
            string oldLabel = checklist.Label;
            int? oldRepeated = checklist.Repeated;
            short? oldApprovalEnabled = checklist.ApprovalEnabled;
            string oldCaseType = checklist.CaseType;
            int? oldDisplayIndex = checklist.DisplayIndex;
            short? oldDownloadEntities = checklist.DownloadEntities;
            short? oldFastNavigation = checklist.FastNavigation;
            string oldFolderName = checklist.FolderName;
            short? oldManualSync = checklist.ManualSync;
            short? oldMultiApproval = checklist.MultiApproval;
            string oldOriginalId = checklist.OriginalId;
            short? oldReviewEnabled = checklist.ReviewEnabled;
            bool oldDocxExportEnabled = checklist.DocxExportEnabled;
            short? oldDoneButtonEnabled = checklist.DoneButtonEnabled;
            short? oldExtraFieldsEnabled = checklist.ExtraFieldsEnabled;
            bool oldJasperExportEnabled = checklist.JasperExportEnabled;
            short? oldQuickSyncEnabled = checklist.QuickSyncEnabled;
            
            checklist.Color = Guid.NewGuid().ToString();
            checklist.Custom = Guid.NewGuid().ToString();
            checklist.Description = Guid.NewGuid().ToString();
            checklist.Field1 = rnd.Next(1, 255);
            checklist.Field2 = rnd.Next(1, 255);
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
            checklist.ReviewEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.DocxExportEnabled = randomBool;
            checklist.DoneButtonEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.ExtraFieldsEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.JasperExportEnabled = randomBool;
            checklist.QuickSyncEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            
            checklist.Update(DbContext);


            List<check_lists> checkLists = DbContext.check_lists.AsNoTracking().ToList();
            List<check_list_versions> checkListVersion = DbContext.check_list_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(checkLists);                                                             
            Assert.NotNull(checkListVersion);                                                             

            Assert.AreEqual(2,checkLists.Count());  
            Assert.AreEqual(3,checkListVersion.Count());  
            
            Assert.AreEqual(checklist.CreatedAt.ToString(), checkLists[1].CreatedAt.ToString());                                  
            Assert.AreEqual(checklist.Version, checkLists[1].Version);                                      
            Assert.AreEqual(checklist.UpdatedAt.ToString(), checkLists[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkLists[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checklist.Id, checkLists[1].Id);
            Assert.AreEqual(checklist.Color, checkLists[1].Color);
            Assert.AreEqual(checklist.Custom, checkLists[1].Custom);
            Assert.AreEqual(checklist.Description, checkLists[1].Description);
            Assert.AreEqual(checklist.Field1, checkLists[1].Field1);
            Assert.AreEqual(checklist.Field2, checkLists[1].Field2);
            Assert.AreEqual(checklist.Field3, checkLists[1].Field3);
            Assert.AreEqual(checklist.Field4, checkLists[1].Field4);
            Assert.AreEqual(checklist.Field5, checkLists[1].Field5);
            Assert.AreEqual(checklist.Field6, checkLists[1].Field6);
            Assert.AreEqual(checklist.Field7, checkLists[1].Field7);
            Assert.AreEqual(checklist.Field8, checkLists[1].Field8);
            Assert.AreEqual(checklist.Field9, checkLists[1].Field9);
            Assert.AreEqual(checklist.Field10, checkLists[1].Field10);
            Assert.AreEqual(checklist.Label, checkLists[1].Label);
            Assert.AreEqual(checklist.Repeated, checkLists[1].Repeated);
            Assert.AreEqual(checklist.ApprovalEnabled, checkLists[1].ApprovalEnabled);
            Assert.AreEqual(checklist.CaseType, checkLists[1].CaseType);
            Assert.AreEqual(checklist.DisplayIndex, checkLists[1].DisplayIndex);
            Assert.AreEqual(checklist.DownloadEntities, checkLists[1].DownloadEntities);
            Assert.AreEqual(checklist.FastNavigation, checkLists[1].FastNavigation);
            Assert.AreEqual(checklist.FolderName, checkLists[1].FolderName);
            Assert.AreEqual(checklist.ManualSync, checkLists[1].ManualSync);
            Assert.AreEqual(checklist.MultiApproval, checkLists[1].MultiApproval);
            Assert.AreEqual(checklist.OriginalId, checkLists[1].OriginalId);
            Assert.AreEqual(checklist.ParentId, checkLists[1].ParentId);
            Assert.AreEqual(checklist.ReviewEnabled, checkLists[1].ReviewEnabled);
            Assert.AreEqual(checklist.DocxExportEnabled, checkLists[1].DocxExportEnabled);
            Assert.AreEqual(checklist.DoneButtonEnabled, checkLists[1].DoneButtonEnabled);
            Assert.AreEqual(checklist.ExtraFieldsEnabled, checkLists[1].ExtraFieldsEnabled);
            Assert.AreEqual(checklist.JasperExportEnabled, checkLists[1].JasperExportEnabled);
            Assert.AreEqual(checklist.QuickSyncEnabled, checkLists[1].QuickSyncEnabled);
            
            //Old Version
            Assert.AreEqual(checklist.CreatedAt.ToString(), checkListVersion[1].CreatedAt.ToString());                                  
            Assert.AreEqual(1, checkListVersion[1].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), checkListVersion[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListVersion[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checklist.Id, checkListVersion[1].CheckListId);
            Assert.AreEqual(oldColour, checkListVersion[1].Color);
            Assert.AreEqual(oldCustom, checkListVersion[1].Custom);
            Assert.AreEqual(oldDescription, checkListVersion[1].Description);
            Assert.AreEqual(oldField1, checkListVersion[1].Field1);
            Assert.AreEqual(oldField2, checkListVersion[1].Field2);
            Assert.AreEqual(oldField3, checkListVersion[1].Field3);
            Assert.AreEqual(oldField4, checkListVersion[1].Field4);
            Assert.AreEqual(oldField5, checkListVersion[1].Field5);
            Assert.AreEqual(oldField6, checkListVersion[1].Field6);
            Assert.AreEqual(oldField7, checkListVersion[1].Field7);
            Assert.AreEqual(oldField8, checkListVersion[1].Field8);
            Assert.AreEqual(oldField9, checkListVersion[1].Field9);
            Assert.AreEqual(oldField10, checkListVersion[1].Field10);
            Assert.AreEqual(oldLabel, checkListVersion[1].Label);
            Assert.AreEqual(oldRepeated, checkListVersion[1].Repeated);
            Assert.AreEqual(oldApprovalEnabled, checkListVersion[1].ApprovalEnabled);
            Assert.AreEqual(oldCaseType, checkListVersion[1].CaseType);
            Assert.AreEqual(oldDisplayIndex, checkListVersion[1].DisplayIndex);
            Assert.AreEqual(oldDownloadEntities, checkListVersion[1].DownloadEntities);
            Assert.AreEqual(oldFastNavigation, checkListVersion[1].FastNavigation);
            Assert.AreEqual(oldFolderName, checkListVersion[1].FolderName);
            Assert.AreEqual(oldManualSync, checkListVersion[1].ManualSync);
            Assert.AreEqual(oldMultiApproval, checkListVersion[1].MultiApproval);
            Assert.AreEqual(oldOriginalId, checkListVersion[1].OriginalId);
            Assert.AreEqual(checklist.ParentId, checkListVersion[1].ParentId);
            Assert.AreEqual(oldReviewEnabled, checkListVersion[1].ReviewEnabled);
            Assert.AreEqual(oldDocxExportEnabled, checkListVersion[1].DocxExportEnabled);
            Assert.AreEqual(oldDoneButtonEnabled, checkListVersion[1].DoneButtonEnabled);
            Assert.AreEqual(oldExtraFieldsEnabled, checkListVersion[1].ExtraFieldsEnabled);
            Assert.AreEqual(oldJasperExportEnabled, checkListVersion[1].JasperExportEnabled);
            Assert.AreEqual(oldQuickSyncEnabled, checkListVersion[1].QuickSyncEnabled);
            
            //New Version
            Assert.AreEqual(checklist.CreatedAt.ToString(), checkListVersion[2].CreatedAt.ToString());                                  
            Assert.AreEqual(2, checkListVersion[2].Version);                                      
            Assert.AreEqual(checklist.UpdatedAt.ToString(), checkListVersion[2].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListVersion[2].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checklist.Id, checkListVersion[2].CheckListId);
            Assert.AreEqual(checklist.Color, checkListVersion[2].Color);
            Assert.AreEqual(checklist.Custom, checkListVersion[2].Custom);
            Assert.AreEqual(checklist.Description, checkListVersion[2].Description);
            Assert.AreEqual(checklist.Field1, checkListVersion[2].Field1);
            Assert.AreEqual(checklist.Field2, checkListVersion[2].Field2);
            Assert.AreEqual(checklist.Field3, checkListVersion[2].Field3);
            Assert.AreEqual(checklist.Field4, checkListVersion[2].Field4);
            Assert.AreEqual(checklist.Field5, checkListVersion[2].Field5);
            Assert.AreEqual(checklist.Field6, checkListVersion[2].Field6);
            Assert.AreEqual(checklist.Field7, checkListVersion[2].Field7);
            Assert.AreEqual(checklist.Field8, checkListVersion[2].Field8);
            Assert.AreEqual(checklist.Field9, checkListVersion[2].Field9);
            Assert.AreEqual(checklist.Field10, checkListVersion[2].Field10);
            Assert.AreEqual(checklist.Label, checkListVersion[2].Label);
            Assert.AreEqual(checklist.Repeated, checkListVersion[2].Repeated);
            Assert.AreEqual(checklist.ApprovalEnabled, checkListVersion[2].ApprovalEnabled);
            Assert.AreEqual(checklist.CaseType, checkListVersion[2].CaseType);
            Assert.AreEqual(checklist.DisplayIndex, checkListVersion[2].DisplayIndex);
            Assert.AreEqual(checklist.DownloadEntities, checkListVersion[2].DownloadEntities);
            Assert.AreEqual(checklist.FastNavigation, checkListVersion[2].FastNavigation);
            Assert.AreEqual(checklist.FolderName, checkListVersion[2].FolderName);
            Assert.AreEqual(checklist.ManualSync, checkListVersion[2].ManualSync);
            Assert.AreEqual(checklist.MultiApproval, checkListVersion[2].MultiApproval);
            Assert.AreEqual(checklist.OriginalId, checkListVersion[2].OriginalId);
            Assert.AreEqual(checklist.ParentId, checkListVersion[2].ParentId);
            Assert.AreEqual(checklist.ReviewEnabled, checkListVersion[2].ReviewEnabled);
            Assert.AreEqual(checklist.DocxExportEnabled, checkListVersion[2].DocxExportEnabled);
            Assert.AreEqual(checklist.DoneButtonEnabled, checkListVersion[2].DoneButtonEnabled);
            Assert.AreEqual(checklist.ExtraFieldsEnabled, checkListVersion[2].ExtraFieldsEnabled);
            Assert.AreEqual(checklist.JasperExportEnabled, checkListVersion[2].JasperExportEnabled);
            Assert.AreEqual(checklist.QuickSyncEnabled, checkListVersion[2].QuickSyncEnabled);
        }

        [Test]
        public void CheckLists_Delete_DoesSetWorkflowStateToRemoved()
        {
             //Arrange
            
            Random rnd = new Random();
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            check_lists checklistParent = new check_lists();
            checklistParent.Color = Guid.NewGuid().ToString();
            checklistParent.Custom = Guid.NewGuid().ToString();
            checklistParent.Description = Guid.NewGuid().ToString();
            checklistParent.Field1 = rnd.Next(1, 255);
            checklistParent.Field2 = rnd.Next(1, 255);
            checklistParent.Field4 = rnd.Next(1, 255);
            checklistParent.Field5 = rnd.Next(1, 255);
            checklistParent.Field6 = rnd.Next(1, 255);
            checklistParent.Field7 = rnd.Next(1, 255);
            checklistParent.Field8 = rnd.Next(1, 255);
            checklistParent.Field9 = rnd.Next(1, 255);
            checklistParent.Field10 = rnd.Next(1, 255);
            checklistParent.Label = Guid.NewGuid().ToString();
            checklistParent.Repeated = rnd.Next(1, 255);
            checklistParent.ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.CaseType = Guid.NewGuid().ToString();
            checklistParent.DisplayIndex = rnd.Next(1, 255);
            checklistParent.DownloadEntities = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.FastNavigation = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.FolderName = Guid.NewGuid().ToString();
            checklistParent.ManualSync = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.MultiApproval = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.OriginalId = Guid.NewGuid().ToString();
            checklistParent.ReviewEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.DocxExportEnabled = randomBool;
            checklistParent.DoneButtonEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.ExtraFieldsEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.JasperExportEnabled = randomBool;
            checklistParent.QuickSyncEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklistParent.Create(DbContext);
            
            check_lists checklist = new check_lists();
            checklist.Color = Guid.NewGuid().ToString();
            checklist.Custom = Guid.NewGuid().ToString();
            checklist.Description = Guid.NewGuid().ToString();
            checklist.Field1 = rnd.Next(1, 255);
            checklist.Field2 = rnd.Next(1, 255);
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
            checklist.ReviewEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.DocxExportEnabled = randomBool;
            checklist.DoneButtonEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.ExtraFieldsEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.JasperExportEnabled = randomBool;
            checklist.QuickSyncEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.ParentId = checklistParent.Id;
            checklist.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = checklist.UpdatedAt;
            
            
            checklist.Delete(DbContext);


            List<check_lists> checkLists = DbContext.check_lists.AsNoTracking().ToList();
            List<check_list_versions> checkListVersion = DbContext.check_list_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(checkLists);                                                             
            Assert.NotNull(checkListVersion);                                                             

            Assert.AreEqual(2,checkLists.Count());  
            Assert.AreEqual(3,checkListVersion.Count());  
            
            Assert.AreEqual(checklist.CreatedAt.ToString(), checkLists[1].CreatedAt.ToString());                                  
            Assert.AreEqual(checklist.Version, checkLists[1].Version);                                      
            Assert.AreEqual(checklist.UpdatedAt.ToString(), checkLists[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkLists[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(checklist.Id, checkLists[1].Id);
            Assert.AreEqual(checklist.Color, checkLists[1].Color);
            Assert.AreEqual(checklist.Custom, checkLists[1].Custom);
            Assert.AreEqual(checklist.Description, checkLists[1].Description);
            Assert.AreEqual(checklist.Field1, checkLists[1].Field1);
            Assert.AreEqual(checklist.Field2, checkLists[1].Field2);
            Assert.AreEqual(checklist.Field3, checkLists[1].Field3);
            Assert.AreEqual(checklist.Field4, checkLists[1].Field4);
            Assert.AreEqual(checklist.Field5, checkLists[1].Field5);
            Assert.AreEqual(checklist.Field6, checkLists[1].Field6);
            Assert.AreEqual(checklist.Field7, checkLists[1].Field7);
            Assert.AreEqual(checklist.Field8, checkLists[1].Field8);
            Assert.AreEqual(checklist.Field9, checkLists[1].Field9);
            Assert.AreEqual(checklist.Field10, checkLists[1].Field10);
            Assert.AreEqual(checklist.Label, checkLists[1].Label);
            Assert.AreEqual(checklist.Repeated, checkLists[1].Repeated);
            Assert.AreEqual(checklist.ApprovalEnabled, checkLists[1].ApprovalEnabled);
            Assert.AreEqual(checklist.CaseType, checkLists[1].CaseType);
            Assert.AreEqual(checklist.DisplayIndex, checkLists[1].DisplayIndex);
            Assert.AreEqual(checklist.DownloadEntities, checkLists[1].DownloadEntities);
            Assert.AreEqual(checklist.FastNavigation, checkLists[1].FastNavigation);
            Assert.AreEqual(checklist.FolderName, checkLists[1].FolderName);
            Assert.AreEqual(checklist.ManualSync, checkLists[1].ManualSync);
            Assert.AreEqual(checklist.MultiApproval, checkLists[1].MultiApproval);
            Assert.AreEqual(checklist.OriginalId, checkLists[1].OriginalId);
            Assert.AreEqual(checklist.ParentId, checkLists[1].ParentId);
            Assert.AreEqual(checklist.ReviewEnabled, checkLists[1].ReviewEnabled);
            Assert.AreEqual(checklist.DocxExportEnabled, checkLists[1].DocxExportEnabled);
            Assert.AreEqual(checklist.DoneButtonEnabled, checkLists[1].DoneButtonEnabled);
            Assert.AreEqual(checklist.ExtraFieldsEnabled, checkLists[1].ExtraFieldsEnabled);
            Assert.AreEqual(checklist.JasperExportEnabled, checkLists[1].JasperExportEnabled);
            Assert.AreEqual(checklist.QuickSyncEnabled, checkLists[1].QuickSyncEnabled);
            
            //Old Version
            Assert.AreEqual(checklist.CreatedAt.ToString(), checkListVersion[1].CreatedAt.ToString());                                  
            Assert.AreEqual(1, checkListVersion[1].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), checkListVersion[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListVersion[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checklist.Id, checkListVersion[1].CheckListId);
            Assert.AreEqual(checklist.Color, checkListVersion[1].Color);
            Assert.AreEqual(checklist.Custom, checkListVersion[1].Custom);
            Assert.AreEqual(checklist.Description, checkListVersion[1].Description);
            Assert.AreEqual(checklist.Field1, checkListVersion[1].Field1);
            Assert.AreEqual(checklist.Field2, checkListVersion[1].Field2);
            Assert.AreEqual(checklist.Field3, checkListVersion[1].Field3);
            Assert.AreEqual(checklist.Field4, checkListVersion[1].Field4);
            Assert.AreEqual(checklist.Field5, checkListVersion[1].Field5);
            Assert.AreEqual(checklist.Field6, checkListVersion[1].Field6);
            Assert.AreEqual(checklist.Field7, checkListVersion[1].Field7);
            Assert.AreEqual(checklist.Field8, checkListVersion[1].Field8);
            Assert.AreEqual(checklist.Field9, checkListVersion[1].Field9);
            Assert.AreEqual(checklist.Field10, checkListVersion[1].Field10);
            Assert.AreEqual(checklist.Label, checkListVersion[1].Label);
            Assert.AreEqual(checklist.Repeated, checkListVersion[1].Repeated);
            Assert.AreEqual(checklist.ApprovalEnabled, checkListVersion[1].ApprovalEnabled);
            Assert.AreEqual(checklist.CaseType, checkListVersion[1].CaseType);
            Assert.AreEqual(checklist.DisplayIndex, checkListVersion[1].DisplayIndex);
            Assert.AreEqual(checklist.DownloadEntities, checkListVersion[1].DownloadEntities);
            Assert.AreEqual(checklist.FastNavigation, checkListVersion[1].FastNavigation);
            Assert.AreEqual(checklist.FolderName, checkListVersion[1].FolderName);
            Assert.AreEqual(checklist.ManualSync, checkListVersion[1].ManualSync);
            Assert.AreEqual(checklist.MultiApproval, checkListVersion[1].MultiApproval);
            Assert.AreEqual(checklist.OriginalId, checkListVersion[1].OriginalId);
            Assert.AreEqual(checklist.ParentId, checkListVersion[1].ParentId);
            Assert.AreEqual(checklist.ReviewEnabled, checkListVersion[1].ReviewEnabled);
            Assert.AreEqual(checklist.DocxExportEnabled, checkListVersion[1].DocxExportEnabled);
            Assert.AreEqual(checklist.DoneButtonEnabled, checkListVersion[1].DoneButtonEnabled);
            Assert.AreEqual(checklist.ExtraFieldsEnabled, checkListVersion[1].ExtraFieldsEnabled);
            Assert.AreEqual(checklist.JasperExportEnabled, checkListVersion[1].JasperExportEnabled);
            Assert.AreEqual(checklist.QuickSyncEnabled, checkListVersion[1].QuickSyncEnabled);
            
            //New Version
            Assert.AreEqual(checklist.CreatedAt.ToString(), checkListVersion[2].CreatedAt.ToString());                                  
            Assert.AreEqual(2, checkListVersion[2].Version);                                      
            Assert.AreEqual(checklist.UpdatedAt.ToString(), checkListVersion[2].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListVersion[2].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(checklist.Id, checkListVersion[2].CheckListId);
            Assert.AreEqual(checklist.Color, checkListVersion[2].Color);
            Assert.AreEqual(checklist.Custom, checkListVersion[2].Custom);
            Assert.AreEqual(checklist.Description, checkListVersion[2].Description);
            Assert.AreEqual(checklist.Field1, checkListVersion[2].Field1);
            Assert.AreEqual(checklist.Field2, checkListVersion[2].Field2);
            Assert.AreEqual(checklist.Field3, checkListVersion[2].Field3);
            Assert.AreEqual(checklist.Field4, checkListVersion[2].Field4);
            Assert.AreEqual(checklist.Field5, checkListVersion[2].Field5);
            Assert.AreEqual(checklist.Field6, checkListVersion[2].Field6);
            Assert.AreEqual(checklist.Field7, checkListVersion[2].Field7);
            Assert.AreEqual(checklist.Field8, checkListVersion[2].Field8);
            Assert.AreEqual(checklist.Field9, checkListVersion[2].Field9);
            Assert.AreEqual(checklist.Field10, checkListVersion[2].Field10);
            Assert.AreEqual(checklist.Label, checkListVersion[2].Label);
            Assert.AreEqual(checklist.Repeated, checkListVersion[2].Repeated);
            Assert.AreEqual(checklist.ApprovalEnabled, checkListVersion[2].ApprovalEnabled);
            Assert.AreEqual(checklist.CaseType, checkListVersion[2].CaseType);
            Assert.AreEqual(checklist.DisplayIndex, checkListVersion[2].DisplayIndex);
            Assert.AreEqual(checklist.DownloadEntities, checkListVersion[2].DownloadEntities);
            Assert.AreEqual(checklist.FastNavigation, checkListVersion[2].FastNavigation);
            Assert.AreEqual(checklist.FolderName, checkListVersion[2].FolderName);
            Assert.AreEqual(checklist.ManualSync, checkListVersion[2].ManualSync);
            Assert.AreEqual(checklist.MultiApproval, checkListVersion[2].MultiApproval);
            Assert.AreEqual(checklist.OriginalId, checkListVersion[2].OriginalId);
            Assert.AreEqual(checklist.ParentId, checkListVersion[2].ParentId);
            Assert.AreEqual(checklist.ReviewEnabled, checkListVersion[2].ReviewEnabled);
            Assert.AreEqual(checklist.DocxExportEnabled, checkListVersion[2].DocxExportEnabled);
            Assert.AreEqual(checklist.DoneButtonEnabled, checkListVersion[2].DoneButtonEnabled);
            Assert.AreEqual(checklist.ExtraFieldsEnabled, checkListVersion[2].ExtraFieldsEnabled);
            Assert.AreEqual(checklist.JasperExportEnabled, checkListVersion[2].JasperExportEnabled);
            Assert.AreEqual(checklist.QuickSyncEnabled, checkListVersion[2].QuickSyncEnabled);
        }

    }
}