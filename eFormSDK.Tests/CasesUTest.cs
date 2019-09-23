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
    public class CasesUTest : DbTestFixture
    {
        [Test]
        public void Cases_Create_DoesCreate()
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
            theCase.MicrotingUid = rnd.Next(shortMinValue, shortmaxValue);
            theCase.SiteId = site.Id;
            theCase.UnitId = unit.Id;
            theCase.WorkerId = worker.Id;
            theCase.CheckListId = checklist.Id;
            theCase.MicrotingCheckUid = rnd.Next(shortMinValue, shortmaxValue);
            
            
            //Act
            
            theCase.Create(DbContext);
            
            List<cases> cases = DbContext.cases.AsNoTracking().ToList();
            List<case_versions> caseVersions = DbContext.case_versions.AsNoTracking().ToList();
            
            //Assert
            
             Assert.NotNull(cases);                                                             
             Assert.NotNull(caseVersions);                                                             

             Assert.AreEqual(1,cases.Count());  
             Assert.AreEqual(1,caseVersions.Count());  
            
             Assert.AreEqual(theCase.CreatedAt.ToString(), cases[0].CreatedAt.ToString());                                  
             Assert.AreEqual(theCase.Version, cases[0].Version);                                      
             Assert.AreEqual(theCase.UpdatedAt.ToString(), cases[0].UpdatedAt.ToString());                                  
             Assert.AreEqual(cases[0].WorkflowState, Constants.WorkflowStates.Created);
             Assert.AreEqual(theCase.Id, cases[0].Id);
             Assert.AreEqual(theCase.Custom, cases[0].Custom);
             Assert.AreEqual(theCase.SiteId, site.Id);
             Assert.AreEqual(theCase.Status, cases[0].Status);
             Assert.AreEqual(theCase.Type, cases[0].Type);
             Assert.AreEqual(theCase.UnitId, unit.Id);
             Assert.AreEqual(theCase.WorkerId, worker.Id);
             Assert.AreEqual(theCase.CaseUid, cases[0].CaseUid);
             Assert.AreEqual(theCase.CheckListId, checklist.Id);
             Assert.AreEqual(theCase.DoneAt.ToString(), cases[0].DoneAt.ToString());
             Assert.AreEqual(theCase.FieldValue1, cases[0].FieldValue1);
             Assert.AreEqual(theCase.FieldValue2, cases[0].FieldValue2);
             Assert.AreEqual(theCase.FieldValue3, cases[0].FieldValue3);
             Assert.AreEqual(theCase.FieldValue4, cases[0].FieldValue4);
             Assert.AreEqual(theCase.FieldValue5, cases[0].FieldValue5);
             Assert.AreEqual(theCase.FieldValue6, cases[0].FieldValue6);
             Assert.AreEqual(theCase.FieldValue7, cases[0].FieldValue7);
             Assert.AreEqual(theCase.FieldValue8, cases[0].FieldValue8);
             Assert.AreEqual(theCase.FieldValue9, cases[0].FieldValue9);
             Assert.AreEqual(theCase.FieldValue10, cases[0].FieldValue10);
             Assert.AreEqual(theCase.MicrotingUid, cases[0].MicrotingUid);
             Assert.AreEqual(theCase.MicrotingCheckUid, cases[0].MicrotingCheckUid);
             
             //Versions
             Assert.AreEqual(theCase.CreatedAt.ToString(), caseVersions[0].CreatedAt.ToString());                                  
             Assert.AreEqual(1, caseVersions[0].Version);                                      
             Assert.AreEqual(theCase.UpdatedAt.ToString(), caseVersions[0].UpdatedAt.ToString());                                  
             Assert.AreEqual(caseVersions[0].WorkflowState, Constants.WorkflowStates.Created);
             Assert.AreEqual(theCase.Id, caseVersions[0].CaseId);
             Assert.AreEqual(theCase.Custom, caseVersions[0].Custom);
             Assert.AreEqual(site.Id, caseVersions[0].SiteId);
             Assert.AreEqual(theCase.Status, caseVersions[0].Status);
             Assert.AreEqual(theCase.Type, caseVersions[0].Type);
             Assert.AreEqual(unit.Id, caseVersions[0].UnitId);
             Assert.AreEqual(worker.Id, caseVersions[0].WorkerId);
             Assert.AreEqual(theCase.CaseUid, caseVersions[0].CaseUid);
             Assert.AreEqual(checklist.Id, caseVersions[0].CheckListId);
             Assert.AreEqual(theCase.DoneAt.ToString(), caseVersions[0].DoneAt.ToString());
             Assert.AreEqual(theCase.FieldValue1, caseVersions[0].FieldValue1);
             Assert.AreEqual(theCase.FieldValue2, caseVersions[0].FieldValue2);
             Assert.AreEqual(theCase.FieldValue3, caseVersions[0].FieldValue3);
             Assert.AreEqual(theCase.FieldValue4, caseVersions[0].FieldValue4);
             Assert.AreEqual(theCase.FieldValue5, caseVersions[0].FieldValue5);
             Assert.AreEqual(theCase.FieldValue6, caseVersions[0].FieldValue6);
             Assert.AreEqual(theCase.FieldValue7, caseVersions[0].FieldValue7);
             Assert.AreEqual(theCase.FieldValue8, caseVersions[0].FieldValue8);
             Assert.AreEqual(theCase.FieldValue9, caseVersions[0].FieldValue9);
             Assert.AreEqual(theCase.FieldValue10, caseVersions[0].FieldValue10);
             Assert.AreEqual(theCase.MicrotingUid, caseVersions[0].MicrotingUid);
             Assert.AreEqual(theCase.MicrotingCheckUid, caseVersions[0].MicrotingCheckUid);
        }

        [Test]
        public void Cases_Update_DoesUpdate()
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
            theCase.MicrotingUid = rnd.Next(shortMinValue, shortmaxValue);
            theCase.SiteId = site.Id;
            theCase.UnitId = unit.Id;
            theCase.WorkerId = worker.Id;
            theCase.CheckListId = checklist.Id;
            theCase.MicrotingCheckUid = rnd.Next(shortMinValue, shortmaxValue);
            theCase.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = theCase.UpdatedAt;
            int? oldStatus = theCase.Status;
            string oldType = theCase.Type;
            string oldCaseUid = theCase.CaseUid;
            DateTime? oldDoneAt = theCase.DoneAt;
            string oldFieldValue1 = theCase.FieldValue1;
            string oldFieldValue2 = theCase.FieldValue2;
            string oldFieldValue3 = theCase.FieldValue3;
            string oldFieldValue4 = theCase.FieldValue4;
            string oldFieldValue5 = theCase.FieldValue5;
            string oldFieldValue6 = theCase.FieldValue6;
            string oldFieldValue7 = theCase.FieldValue7;
            string oldFieldValue8 = theCase.FieldValue8;
            string oldFieldValue9 = theCase.FieldValue9;
            string oldFieldValue10 = theCase.FieldValue10;
            int? oldMicrotingUid = theCase.MicrotingUid;
            int? oldMicrotingCheckUid = theCase.MicrotingCheckUid;
            string oldCustom = theCase.Custom;
            
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
            theCase.MicrotingUid = rnd.Next(shortMinValue, shortmaxValue);
            theCase.MicrotingCheckUid = rnd.Next(shortMinValue, shortmaxValue);
            
            theCase.Update(DbContext);
            
            List<cases> cases = DbContext.cases.AsNoTracking().ToList();
            List<case_versions> caseVersions = DbContext.case_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(cases);                                                             
            Assert.NotNull(caseVersions);                                                             

            Assert.AreEqual(1,cases.Count());  
            Assert.AreEqual(2,caseVersions.Count());
            
            Assert.AreEqual(theCase.CreatedAt.ToString(), cases[0].CreatedAt.ToString());                                  
            Assert.AreEqual(theCase.Version, cases[0].Version);                                      
            Assert.AreEqual(theCase.UpdatedAt.ToString(), cases[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(cases[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(theCase.Id, cases[0].Id);
            Assert.AreEqual(theCase.Custom, cases[0].Custom);
            Assert.AreEqual(theCase.SiteId, site.Id);
            Assert.AreEqual(theCase.Status, cases[0].Status);
            Assert.AreEqual(theCase.Type, cases[0].Type);
            Assert.AreEqual(theCase.UnitId, unit.Id);
            Assert.AreEqual(theCase.WorkerId, worker.Id);
            Assert.AreEqual(theCase.CaseUid, cases[0].CaseUid);
            Assert.AreEqual(theCase.CheckListId, checklist.Id);
            Assert.AreEqual(theCase.DoneAt.ToString(), cases[0].DoneAt.ToString());
            Assert.AreEqual(theCase.FieldValue1, cases[0].FieldValue1);
            Assert.AreEqual(theCase.FieldValue2, cases[0].FieldValue2);
            Assert.AreEqual(theCase.FieldValue3, cases[0].FieldValue3);
            Assert.AreEqual(theCase.FieldValue4, cases[0].FieldValue4);
            Assert.AreEqual(theCase.FieldValue5, cases[0].FieldValue5);
            Assert.AreEqual(theCase.FieldValue6, cases[0].FieldValue6);
            Assert.AreEqual(theCase.FieldValue7, cases[0].FieldValue7);
            Assert.AreEqual(theCase.FieldValue8, cases[0].FieldValue8);
            Assert.AreEqual(theCase.FieldValue9, cases[0].FieldValue9);
            Assert.AreEqual(theCase.FieldValue10, cases[0].FieldValue10);
            Assert.AreEqual(theCase.MicrotingUid, cases[0].MicrotingUid);
            Assert.AreEqual(theCase.MicrotingCheckUid, cases[0].MicrotingCheckUid);
             
             //Old Versions
            Assert.AreEqual(theCase.CreatedAt.ToString(), caseVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, caseVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), caseVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(caseVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(theCase.Id, caseVersions[0].Id);
            Assert.AreEqual(oldCustom, caseVersions[0].Custom);
            Assert.AreEqual(site.Id, caseVersions[0].SiteId);
            Assert.AreEqual(oldStatus, caseVersions[0].Status);
            Assert.AreEqual(oldType, caseVersions[0].Type);
            Assert.AreEqual(unit.Id, caseVersions[0].UnitId);
            Assert.AreEqual(worker.Id, caseVersions[0].WorkerId);
            Assert.AreEqual(oldCaseUid, caseVersions[0].CaseUid);
            Assert.AreEqual(checklist.Id, caseVersions[0].CheckListId);
            Assert.AreEqual(oldDoneAt.ToString(), caseVersions[0].DoneAt.ToString());
            Assert.AreEqual(oldFieldValue1, caseVersions[0].FieldValue1);
            Assert.AreEqual(oldFieldValue2, caseVersions[0].FieldValue2);
            Assert.AreEqual(oldFieldValue3, caseVersions[0].FieldValue3);
            Assert.AreEqual(oldFieldValue4, caseVersions[0].FieldValue4);
            Assert.AreEqual(oldFieldValue5, caseVersions[0].FieldValue5);
            Assert.AreEqual(oldFieldValue6, caseVersions[0].FieldValue6);
            Assert.AreEqual(oldFieldValue7, caseVersions[0].FieldValue7);
            Assert.AreEqual(oldFieldValue8, caseVersions[0].FieldValue8);
            Assert.AreEqual(oldFieldValue9, caseVersions[0].FieldValue9);
            Assert.AreEqual(oldFieldValue10, caseVersions[0].FieldValue10);
            Assert.AreEqual(oldMicrotingUid, caseVersions[0].MicrotingUid);
            Assert.AreEqual(oldMicrotingCheckUid, caseVersions[0].MicrotingCheckUid);
            
            //New Version
            Assert.AreEqual(theCase.CreatedAt.ToString(), caseVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, caseVersions[1].Version);                                      
            Assert.AreEqual(theCase.UpdatedAt.ToString(), caseVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(caseVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(theCase.Id, caseVersions[1].CaseId);
            Assert.AreEqual(theCase.Custom, caseVersions[1].Custom);
            Assert.AreEqual(site.Id, caseVersions[1].SiteId);
            Assert.AreEqual(theCase.Status, caseVersions[1].Status);
            Assert.AreEqual(theCase.Type, caseVersions[1].Type);
            Assert.AreEqual(unit.Id, caseVersions[1].UnitId);
            Assert.AreEqual(worker.Id, caseVersions[1].WorkerId);
            Assert.AreEqual(theCase.CaseUid, caseVersions[1].CaseUid);
            Assert.AreEqual(checklist.Id, caseVersions[1].CheckListId);
            Assert.AreEqual(theCase.DoneAt.ToString(), caseVersions[1].DoneAt.ToString());
            Assert.AreEqual(theCase.FieldValue1, caseVersions[1].FieldValue1);
            Assert.AreEqual(theCase.FieldValue2, caseVersions[1].FieldValue2);
            Assert.AreEqual(theCase.FieldValue3, caseVersions[1].FieldValue3);
            Assert.AreEqual(theCase.FieldValue4, caseVersions[1].FieldValue4);
            Assert.AreEqual(theCase.FieldValue5, caseVersions[1].FieldValue5);
            Assert.AreEqual(theCase.FieldValue6, caseVersions[1].FieldValue6);
            Assert.AreEqual(theCase.FieldValue7, caseVersions[1].FieldValue7);
            Assert.AreEqual(theCase.FieldValue8, caseVersions[1].FieldValue8);
            Assert.AreEqual(theCase.FieldValue9, caseVersions[1].FieldValue9);
            Assert.AreEqual(theCase.FieldValue10, caseVersions[1].FieldValue10);
            Assert.AreEqual(theCase.MicrotingUid, caseVersions[1].MicrotingUid);
            Assert.AreEqual(theCase.MicrotingCheckUid, caseVersions[1].MicrotingCheckUid);
        }

        [Test]
        public void Cases_Delete_DoesSetWorkflowStateToRemoved()
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
            theCase.MicrotingUid = rnd.Next(shortMinValue, shortmaxValue);
            theCase.SiteId = site.Id;
            theCase.UnitId = unit.Id;
            theCase.WorkerId = worker.Id;
            theCase.CheckListId = checklist.Id;
            theCase.MicrotingCheckUid = rnd.Next(shortMinValue, shortmaxValue);
            theCase.Create(DbContext);
            
            
            //Act

            DateTime? oldUpdatedAt = theCase.UpdatedAt;
            
            theCase.Delete(DbContext);
            
            List<cases> cases = DbContext.cases.AsNoTracking().ToList();
            List<case_versions> caseVersions = DbContext.case_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(cases);                                                             
            Assert.NotNull(caseVersions);                                                             

            Assert.AreEqual(1,cases.Count());  
            Assert.AreEqual(2,caseVersions.Count());
            
            Assert.AreEqual(theCase.CreatedAt.ToString(), cases[0].CreatedAt.ToString());                                  
            Assert.AreEqual(theCase.Version, cases[0].Version);                                      
            Assert.AreEqual(theCase.UpdatedAt.ToString(), cases[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(cases[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(theCase.Id, cases[0].Id);
            Assert.AreEqual(theCase.Custom, cases[0].Custom);
            Assert.AreEqual(theCase.SiteId, site.Id);
            Assert.AreEqual(theCase.Status, cases[0].Status);
            Assert.AreEqual(theCase.Type, cases[0].Type);
            Assert.AreEqual(theCase.UnitId, unit.Id);
            Assert.AreEqual(theCase.WorkerId, worker.Id);
            Assert.AreEqual(theCase.CaseUid, cases[0].CaseUid);
            Assert.AreEqual(theCase.CheckListId, checklist.Id);
            Assert.AreEqual(theCase.DoneAt.ToString(), cases[0].DoneAt.ToString());
            Assert.AreEqual(theCase.FieldValue1, cases[0].FieldValue1);
            Assert.AreEqual(theCase.FieldValue2, cases[0].FieldValue2);
            Assert.AreEqual(theCase.FieldValue3, cases[0].FieldValue3);
            Assert.AreEqual(theCase.FieldValue4, cases[0].FieldValue4);
            Assert.AreEqual(theCase.FieldValue5, cases[0].FieldValue5);
            Assert.AreEqual(theCase.FieldValue6, cases[0].FieldValue6);
            Assert.AreEqual(theCase.FieldValue7, cases[0].FieldValue7);
            Assert.AreEqual(theCase.FieldValue8, cases[0].FieldValue8);
            Assert.AreEqual(theCase.FieldValue9, cases[0].FieldValue9);
            Assert.AreEqual(theCase.FieldValue10, cases[0].FieldValue10);
            Assert.AreEqual(theCase.MicrotingUid, cases[0].MicrotingUid);
            Assert.AreEqual(theCase.MicrotingCheckUid, cases[0].MicrotingCheckUid);
            
            //Old Version
            Assert.AreEqual(theCase.CreatedAt.ToString(), caseVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, caseVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), caseVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(caseVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(theCase.Id, caseVersions[0].CaseId);
            Assert.AreEqual(theCase.Custom, caseVersions[0].Custom);
            Assert.AreEqual(site.Id, caseVersions[0].SiteId);
            Assert.AreEqual(theCase.Status, caseVersions[0].Status);
            Assert.AreEqual(theCase.Type, caseVersions[0].Type);
            Assert.AreEqual(unit.Id, caseVersions[0].UnitId);
            Assert.AreEqual(worker.Id, caseVersions[0].WorkerId);
            Assert.AreEqual(theCase.CaseUid, caseVersions[0].CaseUid);
            Assert.AreEqual(checklist.Id, caseVersions[0].CheckListId);
            Assert.AreEqual(theCase.DoneAt.ToString(), caseVersions[0].DoneAt.ToString());
            Assert.AreEqual(theCase.FieldValue1, caseVersions[0].FieldValue1);
            Assert.AreEqual(theCase.FieldValue2, caseVersions[0].FieldValue2);
            Assert.AreEqual(theCase.FieldValue3, caseVersions[0].FieldValue3);
            Assert.AreEqual(theCase.FieldValue4, caseVersions[0].FieldValue4);
            Assert.AreEqual(theCase.FieldValue5, caseVersions[0].FieldValue5);
            Assert.AreEqual(theCase.FieldValue6, caseVersions[0].FieldValue6);
            Assert.AreEqual(theCase.FieldValue7, caseVersions[0].FieldValue7);
            Assert.AreEqual(theCase.FieldValue8, caseVersions[0].FieldValue8);
            Assert.AreEqual(theCase.FieldValue9, caseVersions[0].FieldValue9);
            Assert.AreEqual(theCase.FieldValue10, caseVersions[0].FieldValue10);
            Assert.AreEqual(theCase.MicrotingUid, caseVersions[0].MicrotingUid);
            Assert.AreEqual(theCase.MicrotingCheckUid, caseVersions[0].MicrotingCheckUid);
            
            //New Version
            Assert.AreEqual(theCase.CreatedAt.ToString(), caseVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, cases[0].Version);                                      
            Assert.AreEqual(theCase.UpdatedAt.ToString(), caseVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(caseVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(theCase.Id, caseVersions[1].CaseId);
            Assert.AreEqual(theCase.Custom, caseVersions[1].Custom);
            Assert.AreEqual(site.Id, caseVersions[1].SiteId);
            Assert.AreEqual(theCase.Status, caseVersions[1].Status);
            Assert.AreEqual(theCase.Type, caseVersions[1].Type);
            Assert.AreEqual(unit.Id, caseVersions[1].UnitId);
            Assert.AreEqual(worker.Id, caseVersions[1].WorkerId);
            Assert.AreEqual(theCase.CaseUid, caseVersions[1].CaseUid);
            Assert.AreEqual(checklist.Id, caseVersions[1].CheckListId);
            Assert.AreEqual(theCase.DoneAt.ToString(), caseVersions[1].DoneAt.ToString());
            Assert.AreEqual(theCase.FieldValue1, caseVersions[1].FieldValue1);
            Assert.AreEqual(theCase.FieldValue2, caseVersions[1].FieldValue2);
            Assert.AreEqual(theCase.FieldValue3, caseVersions[1].FieldValue3);
            Assert.AreEqual(theCase.FieldValue4, caseVersions[1].FieldValue4);
            Assert.AreEqual(theCase.FieldValue5, caseVersions[1].FieldValue5);
            Assert.AreEqual(theCase.FieldValue6, caseVersions[1].FieldValue6);
            Assert.AreEqual(theCase.FieldValue7, caseVersions[1].FieldValue7);
            Assert.AreEqual(theCase.FieldValue8, caseVersions[1].FieldValue8);
            Assert.AreEqual(theCase.FieldValue9, caseVersions[1].FieldValue9);
            Assert.AreEqual(theCase.FieldValue10, caseVersions[1].FieldValue10);
            Assert.AreEqual(theCase.MicrotingUid, caseVersions[1].MicrotingUid);
            Assert.AreEqual(theCase.MicrotingCheckUid, caseVersions[1].MicrotingCheckUid);
        }
    }
}