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
    public class CheckListValuesUTest : DbTestFixture
    {
        [Test]
        public void CheckListValues_Create_DoesCreate()
        {
            
            //Arrange
            
            Random rnd = new Random();
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);
            
            units unit = new units();
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);
            unit.SiteId = site.Id;
            unit.Create(DbContext);
            
            workers worker = new workers();
            worker.Email = Guid.NewGuid().ToString();
            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);
            worker.Create(DbContext);
            
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
            checklist.Create(DbContext);
            
            cases theCase = new cases();

            theCase.Custom = Guid.NewGuid().ToString();
            theCase.Status = rnd.Next(1, 255);
            theCase.Type = Guid.NewGuid().ToString();
            theCase.CaseUid = Guid.NewGuid().ToString();
            theCase.DoneAt = DateTime.Now;
            theCase.FieldValue1 = Guid.NewGuid().ToString();
            theCase.FieldValue2 = Guid.NewGuid().ToString();
            theCase.FieldValue3 = Guid.NewGuid().ToString();
            theCase.FieldValue4 = Guid.NewGuid().ToString();
            theCase.FieldValue5 = Guid.NewGuid().ToString();
            theCase.FieldValue6 = Guid.NewGuid().ToString();
            theCase.FieldValue7 = Guid.NewGuid().ToString();
            theCase.FieldValue8 = Guid.NewGuid().ToString();
            theCase.FieldValue9 = Guid.NewGuid().ToString();
            theCase.FieldValue10 = Guid.NewGuid().ToString();
            theCase.MicrotingUid = Guid.NewGuid().ToString();
            theCase.SiteId = site.Id;
            theCase.UnitId = unit.Id;
            theCase.WorkerId = worker.Id;
            theCase.CheckListId = checklist.Id;
            theCase.MicrotingCheckUid = Guid.NewGuid().ToString();
            theCase.Create(DbContext);

            check_list_values checkListValue = new check_list_values();
            checkListValue.Status = Guid.NewGuid().ToString();
            checkListValue.CaseId = theCase.Id;
            checkListValue.CheckListId = checklist.Id;

            //Act

            checkListValue.Create(DbContext);
            
            List<check_list_values> checkListValues = DbContext.check_list_values.AsNoTracking().ToList();
            List<check_list_value_versions> checkListValueVersions = DbContext.check_list_value_versions.AsNoTracking().ToList();

            Assert.NotNull(checkListValues);                                                             
            Assert.NotNull(checkListValueVersions);                                                             

            Assert.AreEqual(1,checkListValues.Count());  
            Assert.AreEqual(1,checkListValueVersions.Count());  
            
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValues[0].CreatedAt.ToString());                                  
            Assert.AreEqual(checkListValue.Version, checkListValues[0].Version);                                      
            Assert.AreEqual(checkListValue.UpdatedAt.ToString(), checkListValues[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListValues[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListValue.Id, checkListValues[0].Id);
            Assert.AreEqual(checkListValue.Status, checkListValues[0].Status);
            Assert.AreEqual(checkListValue.CaseId, checkListValues[0].CaseId);
            Assert.AreEqual(checkListValue.Status, checkListValues[0].Status);
            Assert.AreEqual(checkListValue.CheckListId, checkListValues[0].CheckListId);

            //Versions
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValueVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(checkListValue.Version, checkListValueVersions[0].Version);                                      
            Assert.AreEqual(checkListValue.UpdatedAt.ToString(), checkListValueVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListValueVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListValue.Id, checkListValueVersions[0].Id);
            Assert.AreEqual(checkListValue.Status, checkListValueVersions[0].Status);
            Assert.AreEqual(checkListValue.CaseId, checkListValueVersions[0].CaseId);
            Assert.AreEqual(checkListValue.Status, checkListValueVersions[0].Status);
            Assert.AreEqual(checkListValue.CheckListId, checkListValueVersions[0].CheckListId);
        }

        [Test]
        public void CheckListValues_Update_DoesUpdate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);
            
            units unit = new units();
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);
            unit.SiteId = site.Id;
            unit.Create(DbContext);
            
            workers worker = new workers();
            worker.Email = Guid.NewGuid().ToString();
            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);
            worker.Create(DbContext);
            
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
            checklist.Create(DbContext);
            
            cases theCase = new cases();

            theCase.Custom = Guid.NewGuid().ToString();
            theCase.Status = rnd.Next(1, 255);
            theCase.Type = Guid.NewGuid().ToString();
            theCase.CaseUid = Guid.NewGuid().ToString();
            theCase.DoneAt = DateTime.Now;
            theCase.FieldValue1 = Guid.NewGuid().ToString();
            theCase.FieldValue2 = Guid.NewGuid().ToString();
            theCase.FieldValue3 = Guid.NewGuid().ToString();
            theCase.FieldValue4 = Guid.NewGuid().ToString();
            theCase.FieldValue5 = Guid.NewGuid().ToString();
            theCase.FieldValue6 = Guid.NewGuid().ToString();
            theCase.FieldValue7 = Guid.NewGuid().ToString();
            theCase.FieldValue8 = Guid.NewGuid().ToString();
            theCase.FieldValue9 = Guid.NewGuid().ToString();
            theCase.FieldValue10 = Guid.NewGuid().ToString();
            theCase.MicrotingUid = Guid.NewGuid().ToString();
            theCase.SiteId = site.Id;
            theCase.UnitId = unit.Id;
            theCase.WorkerId = worker.Id;
            theCase.CheckListId = checklist.Id;
            theCase.MicrotingCheckUid = Guid.NewGuid().ToString();
            theCase.Create(DbContext);

            check_list_values checkListValue = new check_list_values();
            checkListValue.Status = Guid.NewGuid().ToString();
            checkListValue.CaseId = theCase.Id;
            checkListValue.CheckListId = checklist.Id;
            checkListValue.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = checkListValue.UpdatedAt;
            string oldStatus = checkListValue.Status;

            checkListValue.Status = Guid.NewGuid().ToString();
            
            checkListValue.Update(DbContext);
            
            List<check_list_values> checkListValues = DbContext.check_list_values.AsNoTracking().ToList();
            List<check_list_value_versions> checkListValueVersions = DbContext.check_list_value_versions.AsNoTracking().ToList();

            Assert.NotNull(checkListValues);                                                             
            Assert.NotNull(checkListValueVersions);                                                             

            Assert.AreEqual(1,checkListValues.Count());  
            Assert.AreEqual(2,checkListValueVersions.Count());  
            
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValues[0].CreatedAt.ToString());                                  
            Assert.AreEqual(checkListValue.Version, checkListValues[0].Version);                                      
            Assert.AreEqual(checkListValue.UpdatedAt.ToString(), checkListValues[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListValues[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListValue.Id, checkListValues[0].Id);
            Assert.AreEqual(checkListValue.Status, checkListValues[0].Status);
            Assert.AreEqual(checkListValue.CaseId, checkListValues[0].CaseId);
            Assert.AreEqual(checkListValue.Status, checkListValues[0].Status);
            Assert.AreEqual(checkListValue.CheckListId, checkListValues[0].CheckListId);

            //Old Version
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValueVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, checkListValueVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), checkListValueVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListValueVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListValue.Id, checkListValueVersions[0].CheckListValueId);
            Assert.AreEqual(oldStatus, checkListValueVersions[0].Status);
            Assert.AreEqual(checkListValue.CaseId, checkListValueVersions[0].CaseId);
            Assert.AreEqual(checkListValue.CheckListId, checkListValueVersions[0].CheckListId);
            
            //New Version
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValueVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, checkListValueVersions[1].Version);                                      
            Assert.AreEqual(checkListValue.UpdatedAt.ToString(), checkListValueVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListValueVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListValue.Id, checkListValueVersions[1].CheckListValueId);
            Assert.AreEqual(checkListValue.Status, checkListValueVersions[1].Status);
            Assert.AreEqual(checkListValue.CaseId, checkListValueVersions[1].CaseId);
            Assert.AreEqual(checkListValue.CheckListId, checkListValueVersions[1].CheckListId);
        }

        [Test]
        public void CheckListValues_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange
            
            Random rnd = new Random();
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);
            
            units unit = new units();
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);
            unit.SiteId = site.Id;
            unit.Create(DbContext);
            
            workers worker = new workers();
            worker.Email = Guid.NewGuid().ToString();
            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);
            worker.Create(DbContext);
            
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
            checklist.Create(DbContext);
            
            cases theCase = new cases();

            theCase.Custom = Guid.NewGuid().ToString();
            theCase.Status = rnd.Next(1, 255);
            theCase.Type = Guid.NewGuid().ToString();
            theCase.CaseUid = Guid.NewGuid().ToString();
            theCase.DoneAt = DateTime.Now;
            theCase.FieldValue1 = Guid.NewGuid().ToString();
            theCase.FieldValue2 = Guid.NewGuid().ToString();
            theCase.FieldValue3 = Guid.NewGuid().ToString();
            theCase.FieldValue4 = Guid.NewGuid().ToString();
            theCase.FieldValue5 = Guid.NewGuid().ToString();
            theCase.FieldValue6 = Guid.NewGuid().ToString();
            theCase.FieldValue7 = Guid.NewGuid().ToString();
            theCase.FieldValue8 = Guid.NewGuid().ToString();
            theCase.FieldValue9 = Guid.NewGuid().ToString();
            theCase.FieldValue10 = Guid.NewGuid().ToString();
            theCase.MicrotingUid = Guid.NewGuid().ToString();
            theCase.SiteId = site.Id;
            theCase.UnitId = unit.Id;
            theCase.WorkerId = worker.Id;
            theCase.CheckListId = checklist.Id;
            theCase.MicrotingCheckUid = Guid.NewGuid().ToString();
            theCase.Create(DbContext);

            check_list_values checkListValue = new check_list_values();
            checkListValue.Status = Guid.NewGuid().ToString();
            checkListValue.CaseId = theCase.Id;
            checkListValue.CheckListId = checklist.Id;
            checkListValue.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = checkListValue.UpdatedAt;

            checkListValue.Delete(DbContext);
            
            List<check_list_values> checkListValues = DbContext.check_list_values.AsNoTracking().ToList();
            List<check_list_value_versions> checkListValueVersions = DbContext.check_list_value_versions.AsNoTracking().ToList();

            Assert.NotNull(checkListValues);                                                             
            Assert.NotNull(checkListValueVersions);                                                             

            Assert.AreEqual(1,checkListValues.Count());  
            Assert.AreEqual(2,checkListValueVersions.Count());  
            
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValues[0].CreatedAt.ToString());                                  
            Assert.AreEqual(checkListValue.Version, checkListValues[0].Version);                                      
            Assert.AreEqual(checkListValue.UpdatedAt.ToString(), checkListValues[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListValues[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(checkListValue.Id, checkListValues[0].Id);
            Assert.AreEqual(checkListValue.Status, checkListValues[0].Status);
            Assert.AreEqual(checkListValue.CaseId, checkListValues[0].CaseId);
            Assert.AreEqual(checkListValue.Status, checkListValues[0].Status);
            Assert.AreEqual(checkListValue.CheckListId, checkListValues[0].CheckListId);

            //Old Version
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValueVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, checkListValueVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), checkListValueVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListValueVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListValue.Id, checkListValueVersions[0].CheckListValueId);
            Assert.AreEqual(checkListValue.Status, checkListValueVersions[0].Status);
            Assert.AreEqual(checkListValue.CaseId, checkListValueVersions[0].CaseId);
            Assert.AreEqual(checkListValue.CheckListId, checkListValueVersions[0].CheckListId);
            
            //New Version
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValueVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, checkListValueVersions[1].Version);                                      
            Assert.AreEqual(checkListValue.UpdatedAt.ToString(), checkListValueVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(checkListValueVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(checkListValue.Id, checkListValueVersions[1].CheckListValueId);
            Assert.AreEqual(checkListValue.Status, checkListValueVersions[1].Status);
            Assert.AreEqual(checkListValue.CaseId, checkListValueVersions[1].CaseId);
            Assert.AreEqual(checkListValue.CheckListId, checkListValueVersions[1].CheckListId);
        }
        
    }
}