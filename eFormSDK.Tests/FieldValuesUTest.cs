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
    public class FieldValuesUTest : DbTestFixture
    {
        [Test]
        public async Task FieldValues_Create_DoesCreate()
        {
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            Random rnd = new Random();
            
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
            
            entity_groups entityGroup = new entity_groups();
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            entityGroup.Create(DbContext);
            
            field_types fieldType = new field_types();
            fieldType.Description = Guid.NewGuid().ToString();
            fieldType.FieldType = Guid.NewGuid().ToString();
            fieldType.Create(DbContext);
            
            fields field = new fields();
            field.Color = Guid.NewGuid().ToString();
            field.Custom = Guid.NewGuid().ToString();
            field.Description = Guid.NewGuid().ToString();
            field.Dummy = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.Label = Guid.NewGuid().ToString();
            field.Mandatory = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.Multi = rnd.Next(1, 255);
            field.Optional = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.Selected = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.BarcodeEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.BarcodeType = Guid.NewGuid().ToString();
            field.DecimalCount = rnd.Next(1, 255);
            field.DefaultValue = Guid.NewGuid().ToString();
            field.DisplayIndex = rnd.Next(1, 255);
            field.GeolocationEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.GeolocationForced = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.GeolocationHidden = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.IsNum = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.MaxLength = rnd.Next(1, 255);
            field.MaxValue = Guid.NewGuid().ToString();
            field.MinValue = Guid.NewGuid().ToString();
            field.OriginalId = Guid.NewGuid().ToString();
            field.QueryType = Guid.NewGuid().ToString();
            field.ReadOnly = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.SplitScreen = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.UnitName = Guid.NewGuid().ToString();
            field.StopOnSave = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.KeyValuePairList = Guid.NewGuid().ToString();
            field.CheckListId = checklist.Id;
            field.EntityGroupId = entityGroup.Id;
            field.FieldTypeId = fieldType.Id;
            field.Create(DbContext);
            
            
            
            workers worker = new workers();
            worker.Email = Guid.NewGuid().ToString();
            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);
            worker.Create(DbContext);
            
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
            
            uploaded_data uploadedData = new uploaded_data();
            uploadedData.Checksum = Guid.NewGuid().ToString();
            uploadedData.Extension = Guid.NewGuid().ToString();
            uploadedData.Local = (short)rnd.Next(shortMinValue, shortmaxValue);
            uploadedData.CurrentFile = Guid.NewGuid().ToString();
            uploadedData.ExpirationDate = DateTime.Now;
            uploadedData.FileLocation = Guid.NewGuid().ToString();
            uploadedData.FileName = Guid.NewGuid().ToString();
            uploadedData.TranscriptionId = rnd.Next(1, 255);
            uploadedData.UploaderId = rnd.Next(1, 255);
            uploadedData.UploaderType = Guid.NewGuid().ToString();
            uploadedData.Create(DbContext);

            field_values fieldValue = new field_values();
            fieldValue.Accuracy = Guid.NewGuid().ToString();
            fieldValue.Altitude = Guid.NewGuid().ToString();
            fieldValue.Date = DateTime.Now;
            fieldValue.Heading = Guid.NewGuid().ToString();
            fieldValue.Latitude = Guid.NewGuid().ToString();
            fieldValue.Longitude = Guid.NewGuid().ToString();
            fieldValue.Value = Guid.NewGuid().ToString();
            fieldValue.CaseId = theCase.Id;
            fieldValue.DoneAt = DateTime.Now;
            fieldValue.FieldId = field.Id;
            fieldValue.WorkerId = worker.Id;
            fieldValue.CheckListId = checklist.Id;
            fieldValue.UploadedDataId = uploadedData.Id;

            //Act
            
            fieldValue.Create(DbContext);
            
            List<field_values> fieldValues = DbContext.field_values.AsNoTracking().ToList();
            List<field_value_versions> fieldValueVersions = DbContext.field_value_versions.AsNoTracking().ToList();
            
            Assert.NotNull(fieldValues);                                                             
            Assert.NotNull(fieldValueVersions);                                                             

            Assert.AreEqual(1,fieldValues.Count());  
            Assert.AreEqual(1,fieldValueVersions.Count());  
            
            Assert.AreEqual(fieldValue.CreatedAt.ToString(), fieldValues[0].CreatedAt.ToString());                                  
            Assert.AreEqual(fieldValue.Version, fieldValues[0].Version);                                      
            Assert.AreEqual(fieldValue.UpdatedAt.ToString(), fieldValues[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldValues[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(fieldValue.Id, fieldValues[0].Id);
            Assert.AreEqual(fieldValue.Accuracy, fieldValues[0].Accuracy);
            Assert.AreEqual(fieldValue.Date.ToString(), fieldValues[0].Date.ToString());
            Assert.AreEqual(fieldValue.Heading, fieldValues[0].Heading);
            Assert.AreEqual(fieldValue.Latitude, fieldValues[0].Latitude);
            Assert.AreEqual(fieldValue.Longitude, fieldValues[0].Longitude);
            Assert.AreEqual(fieldValue.Value, fieldValues[0].Value);
            Assert.AreEqual(fieldValue.CaseId, theCase.Id);
            Assert.AreEqual(fieldValue.DoneAt.ToString(), fieldValues[0].DoneAt.ToString());
            Assert.AreEqual(fieldValue.FieldId, field.Id);
            Assert.AreEqual(fieldValue.WorkerId, worker.Id);
            Assert.AreEqual(fieldValue.CheckListId, checklist.Id);
            Assert.AreEqual(fieldValue.UploadedDataId, uploadedData.Id);
            
            //Versions
            Assert.AreEqual(fieldValue.CreatedAt.ToString(), fieldValueVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, fieldValueVersions[0].Version);                                      
            Assert.AreEqual(fieldValue.UpdatedAt.ToString(), fieldValueVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldValueVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(fieldValue.Id, fieldValueVersions[0].FieldId);
            Assert.AreEqual(fieldValue.Accuracy, fieldValueVersions[0].Accuracy);
            Assert.AreEqual(fieldValue.Date.ToString(), fieldValueVersions[0].Date.ToString());
            Assert.AreEqual(fieldValue.Heading, fieldValueVersions[0].Heading);
            Assert.AreEqual(fieldValue.Latitude, fieldValueVersions[0].Latitude);
            Assert.AreEqual(fieldValue.Longitude, fieldValueVersions[0].Longitude);
            Assert.AreEqual(fieldValue.Value, fieldValueVersions[0].Value);
            Assert.AreEqual(theCase.Id, fieldValueVersions[0].CaseId);
            Assert.AreEqual(fieldValue.DoneAt.ToString(), fieldValueVersions[0].DoneAt.ToString());
            Assert.AreEqual(field.Id, fieldValueVersions[0].FieldId);
            Assert.AreEqual(worker.Id, fieldValueVersions[0].WorkerId);
            Assert.AreEqual(checklist.Id, fieldValueVersions[0].CheckListId);
            Assert.AreEqual(uploadedData.Id, fieldValueVersions[0].UploadedDataId);
        }

        [Test]
        public async Task FieldValues_Update_DoesUpdate()
        {
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            Random rnd = new Random();
            
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
            
            entity_groups entityGroup = new entity_groups();
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            entityGroup.Create(DbContext);
            
            field_types fieldType = new field_types();
            fieldType.Description = Guid.NewGuid().ToString();
            fieldType.FieldType = Guid.NewGuid().ToString();
            fieldType.Create(DbContext);
            
            fields field = new fields();
            field.Color = Guid.NewGuid().ToString();
            field.Custom = Guid.NewGuid().ToString();
            field.Description = Guid.NewGuid().ToString();
            field.Dummy = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.Label = Guid.NewGuid().ToString();
            field.Mandatory = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.Multi = rnd.Next(1, 255);
            field.Optional = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.Selected = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.BarcodeEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.BarcodeType = Guid.NewGuid().ToString();
            field.DecimalCount = rnd.Next(1, 255);
            field.DefaultValue = Guid.NewGuid().ToString();
            field.DisplayIndex = rnd.Next(1, 255);
            field.GeolocationEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.GeolocationForced = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.GeolocationHidden = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.IsNum = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.MaxLength = rnd.Next(1, 255);
            field.MaxValue = Guid.NewGuid().ToString();
            field.MinValue = Guid.NewGuid().ToString();
            field.OriginalId = Guid.NewGuid().ToString();
            field.QueryType = Guid.NewGuid().ToString();
            field.ReadOnly = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.SplitScreen = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.UnitName = Guid.NewGuid().ToString();
            field.StopOnSave = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.KeyValuePairList = Guid.NewGuid().ToString();
            field.CheckListId = checklist.Id;
            field.EntityGroupId = entityGroup.Id;
            field.FieldTypeId = fieldType.Id;
            field.Create(DbContext);
            
            workers worker = new workers();
            worker.Email = Guid.NewGuid().ToString();
            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);
            worker.Create(DbContext);
            
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
            
            uploaded_data uploadedData = new uploaded_data();
            uploadedData.Checksum = Guid.NewGuid().ToString();
            uploadedData.Extension = Guid.NewGuid().ToString();
            uploadedData.Local = (short)rnd.Next(shortMinValue, shortmaxValue);
            uploadedData.CurrentFile = Guid.NewGuid().ToString();
            uploadedData.ExpirationDate = DateTime.Now;
            uploadedData.FileLocation = Guid.NewGuid().ToString();
            uploadedData.FileName = Guid.NewGuid().ToString();
            uploadedData.TranscriptionId = rnd.Next(1, 255);
            uploadedData.UploaderId = rnd.Next(1, 255);
            uploadedData.UploaderType = Guid.NewGuid().ToString();
            uploadedData.Create(DbContext);

            field_values fieldValue = new field_values();
            fieldValue.Accuracy = Guid.NewGuid().ToString();
            fieldValue.Altitude = Guid.NewGuid().ToString();
            fieldValue.Date = DateTime.Now;
            fieldValue.Heading = Guid.NewGuid().ToString();
            fieldValue.Latitude = Guid.NewGuid().ToString();
            fieldValue.Longitude = Guid.NewGuid().ToString();
            fieldValue.Value = Guid.NewGuid().ToString();
            fieldValue.CaseId = theCase.Id;
            fieldValue.DoneAt = DateTime.Now;
            fieldValue.FieldId = field.Id;
            fieldValue.WorkerId = worker.Id;
            fieldValue.CheckListId = checklist.Id;
            fieldValue.UploadedDataId = uploadedData.Id;
            fieldValue.Create(DbContext);

            //Act

            DateTime? oldUpdatedAt = fieldValue.UpdatedAt;
            string oldAccuracy = fieldValue.Accuracy;
            string oldAltitude = fieldValue.Altitude;
            DateTime? oldDate = fieldValue.Date;
            string oldHeading = fieldValue.Heading;
            string oldLatitude = fieldValue.Latitude;
            string oldLongitude = fieldValue.Longitude;
            string oldValue = fieldValue.Value;
            DateTime? oldDoneAt = fieldValue.DoneAt;
            
            fieldValue.Accuracy = Guid.NewGuid().ToString();
            fieldValue.Altitude = Guid.NewGuid().ToString();
            fieldValue.Date = DateTime.Now;
            fieldValue.Heading = Guid.NewGuid().ToString();
            fieldValue.Latitude = Guid.NewGuid().ToString();
            fieldValue.Longitude = Guid.NewGuid().ToString();
            fieldValue.Value = Guid.NewGuid().ToString();
            fieldValue.DoneAt = DateTime.Now;

            fieldValue.Update(DbContext);
            
            List<field_values> fieldValues = DbContext.field_values.AsNoTracking().ToList();
            List<field_value_versions> fieldValueVersions = DbContext.field_value_versions.AsNoTracking().ToList();
            
            Assert.NotNull(fieldValues);                                                             
            Assert.NotNull(fieldValueVersions);                                                             

            Assert.AreEqual(1,fieldValues.Count());  
            Assert.AreEqual(2,fieldValueVersions.Count());  
            
            Assert.AreEqual(fieldValue.CreatedAt.ToString(), fieldValues[0].CreatedAt.ToString());                                  
            Assert.AreEqual(fieldValue.Version, fieldValues[0].Version);                                      
            Assert.AreEqual(fieldValue.UpdatedAt.ToString(), fieldValues[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldValues[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(fieldValue.Id, fieldValues[0].Id);
            Assert.AreEqual(fieldValue.Accuracy, fieldValues[0].Accuracy);
            Assert.AreEqual(fieldValue.Date.ToString(), fieldValues[0].Date.ToString());
            Assert.AreEqual(fieldValue.Heading, fieldValues[0].Heading);
            Assert.AreEqual(fieldValue.Latitude, fieldValues[0].Latitude);
            Assert.AreEqual(fieldValue.Longitude, fieldValues[0].Longitude);
            Assert.AreEqual(fieldValue.Value, fieldValues[0].Value);
            Assert.AreEqual(fieldValue.CaseId, theCase.Id);
            Assert.AreEqual(fieldValue.DoneAt.ToString(), fieldValues[0].DoneAt.ToString());
            Assert.AreEqual(fieldValue.FieldId, field.Id);
            Assert.AreEqual(fieldValue.WorkerId, worker.Id);
            Assert.AreEqual(fieldValue.CheckListId, checklist.Id);
            Assert.AreEqual(fieldValue.UploadedDataId, uploadedData.Id);
            
            //Old Version
            Assert.AreEqual(fieldValue.CreatedAt.ToString(), fieldValueVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, fieldValueVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), fieldValueVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldValueVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(fieldValue.Id, fieldValueVersions[0].FieldId);
            Assert.AreEqual(oldAccuracy, fieldValueVersions[0].Accuracy);
            Assert.AreEqual(oldDate.ToString(), fieldValueVersions[0].Date.ToString());
            Assert.AreEqual(oldHeading, fieldValueVersions[0].Heading);
            Assert.AreEqual(oldLatitude, fieldValueVersions[0].Latitude);
            Assert.AreEqual(oldLongitude, fieldValueVersions[0].Longitude);
            Assert.AreEqual(oldValue, fieldValueVersions[0].Value);
            Assert.AreEqual(theCase.Id, fieldValueVersions[0].CaseId);
            Assert.AreEqual(oldDoneAt.ToString(), fieldValueVersions[0].DoneAt.ToString());
            Assert.AreEqual(field.Id, fieldValueVersions[0].FieldId);
            Assert.AreEqual(worker.Id, fieldValueVersions[0].WorkerId);
            Assert.AreEqual(checklist.Id, fieldValueVersions[0].CheckListId);
            Assert.AreEqual(uploadedData.Id, fieldValueVersions[0].UploadedDataId);
            
            //New Version
            Assert.AreEqual(fieldValue.CreatedAt.ToString(), fieldValueVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, fieldValueVersions[1].Version);                                      
            Assert.AreEqual(fieldValue.UpdatedAt.ToString(), fieldValueVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldValueVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(fieldValue.Id, fieldValueVersions[1].FieldId);
            Assert.AreEqual(fieldValue.Accuracy, fieldValueVersions[1].Accuracy);
            Assert.AreEqual(fieldValue.Date.ToString(), fieldValueVersions[1].Date.ToString());
            Assert.AreEqual(fieldValue.Heading, fieldValueVersions[1].Heading);
            Assert.AreEqual(fieldValue.Latitude, fieldValueVersions[1].Latitude);
            Assert.AreEqual(fieldValue.Longitude, fieldValueVersions[1].Longitude);
            Assert.AreEqual(fieldValue.Value, fieldValueVersions[1].Value);
            Assert.AreEqual(theCase.Id, fieldValueVersions[1].CaseId);
            Assert.AreEqual(fieldValue.DoneAt.ToString(), fieldValueVersions[1].DoneAt.ToString());
            Assert.AreEqual(field.Id, fieldValueVersions[1].FieldId);
            Assert.AreEqual(worker.Id, fieldValueVersions[1].WorkerId);
            Assert.AreEqual(checklist.Id, fieldValueVersions[1].CheckListId);
            Assert.AreEqual(uploadedData.Id, fieldValueVersions[1].UploadedDataId);
        }

        [Test]
        public async Task FieldValues_Delete_DoesSetWorkflowStateToRemoved()
        {
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            Random rnd = new Random();
            
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
            
            entity_groups entityGroup = new entity_groups();
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            entityGroup.Create(DbContext);
            
            field_types fieldType = new field_types();
            fieldType.Description = Guid.NewGuid().ToString();
            fieldType.FieldType = Guid.NewGuid().ToString();
            fieldType.Create(DbContext);
            
            fields field = new fields();
            field.Color = Guid.NewGuid().ToString();
            field.Custom = Guid.NewGuid().ToString();
            field.Description = Guid.NewGuid().ToString();
            field.Dummy = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.Label = Guid.NewGuid().ToString();
            field.Mandatory = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.Multi = rnd.Next(1, 255);
            field.Optional = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.Selected = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.BarcodeEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.BarcodeType = Guid.NewGuid().ToString();
            field.DecimalCount = rnd.Next(1, 255);
            field.DefaultValue = Guid.NewGuid().ToString();
            field.DisplayIndex = rnd.Next(1, 255);
            field.GeolocationEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.GeolocationForced = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.GeolocationHidden = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.IsNum = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.MaxLength = rnd.Next(1, 255);
            field.MaxValue = Guid.NewGuid().ToString();
            field.MinValue = Guid.NewGuid().ToString();
            field.OriginalId = Guid.NewGuid().ToString();
            field.QueryType = Guid.NewGuid().ToString();
            field.ReadOnly = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.SplitScreen = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.UnitName = Guid.NewGuid().ToString();
            field.StopOnSave = (short) rnd.Next(shortMinValue, shortmaxValue);
            field.KeyValuePairList = Guid.NewGuid().ToString();
            field.CheckListId = checklist.Id;
            field.EntityGroupId = entityGroup.Id;
            field.FieldTypeId = fieldType.Id;
            field.Create(DbContext);
            
            workers worker = new workers();
            worker.Email = Guid.NewGuid().ToString();
            worker.FirstName = Guid.NewGuid().ToString();
            worker.LastName = Guid.NewGuid().ToString();
            worker.MicrotingUid = rnd.Next(1, 255);
            worker.Create(DbContext);
            
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
            
            uploaded_data uploadedData = new uploaded_data();
            uploadedData.Checksum = Guid.NewGuid().ToString();
            uploadedData.Extension = Guid.NewGuid().ToString();
            uploadedData.Local = (short)rnd.Next(shortMinValue, shortmaxValue);
            uploadedData.CurrentFile = Guid.NewGuid().ToString();
            uploadedData.ExpirationDate = DateTime.Now;
            uploadedData.FileLocation = Guid.NewGuid().ToString();
            uploadedData.FileName = Guid.NewGuid().ToString();
            uploadedData.TranscriptionId = rnd.Next(1, 255);
            uploadedData.UploaderId = rnd.Next(1, 255);
            uploadedData.UploaderType = Guid.NewGuid().ToString();
            uploadedData.Create(DbContext);

            field_values fieldValue = new field_values();
            fieldValue.Accuracy = Guid.NewGuid().ToString();
            fieldValue.Altitude = Guid.NewGuid().ToString();
            fieldValue.Date = DateTime.Now;
            fieldValue.Heading = Guid.NewGuid().ToString();
            fieldValue.Latitude = Guid.NewGuid().ToString();
            fieldValue.Longitude = Guid.NewGuid().ToString();
            fieldValue.Value = Guid.NewGuid().ToString();
            fieldValue.CaseId = theCase.Id;
            fieldValue.DoneAt = DateTime.Now;
            fieldValue.FieldId = field.Id;
            fieldValue.WorkerId = worker.Id;
            fieldValue.CheckListId = checklist.Id;
            fieldValue.UploadedDataId = uploadedData.Id;
            fieldValue.Create(DbContext);

            //Act

            DateTime? oldUpdatedAt = fieldValue.UpdatedAt;
            

            fieldValue.Delete(DbContext);
            
            List<field_values> fieldValues = DbContext.field_values.AsNoTracking().ToList();
            List<field_value_versions> fieldValueVersions = DbContext.field_value_versions.AsNoTracking().ToList();
            
            Assert.NotNull(fieldValues);                                                             
            Assert.NotNull(fieldValueVersions);                                                             

            Assert.AreEqual(1,fieldValues.Count());  
            Assert.AreEqual(2,fieldValueVersions.Count());  
            
            Assert.AreEqual(fieldValue.CreatedAt.ToString(), fieldValues[0].CreatedAt.ToString());                                  
            Assert.AreEqual(fieldValue.Version, fieldValues[0].Version);                                      
            Assert.AreEqual(fieldValue.UpdatedAt.ToString(), fieldValues[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldValues[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(fieldValue.Id, fieldValues[0].Id);
            Assert.AreEqual(fieldValue.Accuracy, fieldValues[0].Accuracy);
            Assert.AreEqual(fieldValue.Date.ToString(), fieldValues[0].Date.ToString());
            Assert.AreEqual(fieldValue.Heading, fieldValues[0].Heading);
            Assert.AreEqual(fieldValue.Latitude, fieldValues[0].Latitude);
            Assert.AreEqual(fieldValue.Longitude, fieldValues[0].Longitude);
            Assert.AreEqual(fieldValue.Value, fieldValues[0].Value);
            Assert.AreEqual(fieldValue.CaseId, theCase.Id);
            Assert.AreEqual(fieldValue.DoneAt.ToString(), fieldValues[0].DoneAt.ToString());
            Assert.AreEqual(fieldValue.FieldId, field.Id);
            Assert.AreEqual(fieldValue.WorkerId, worker.Id);
            Assert.AreEqual(fieldValue.CheckListId, checklist.Id);
            Assert.AreEqual(fieldValue.UploadedDataId, uploadedData.Id);
            
            //Old Version
            Assert.AreEqual(fieldValue.CreatedAt.ToString(), fieldValueVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, fieldValueVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), fieldValueVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldValueVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(fieldValue.Id, fieldValueVersions[0].FieldId);
            Assert.AreEqual(fieldValue.Accuracy, fieldValueVersions[0].Accuracy);
            Assert.AreEqual(fieldValue.Date.ToString(), fieldValueVersions[0].Date.ToString());
            Assert.AreEqual(fieldValue.Heading, fieldValueVersions[0].Heading);
            Assert.AreEqual(fieldValue.Latitude, fieldValueVersions[0].Latitude);
            Assert.AreEqual(fieldValue.Longitude, fieldValueVersions[0].Longitude);
            Assert.AreEqual(fieldValue.Value, fieldValueVersions[0].Value);
            Assert.AreEqual(theCase.Id, fieldValueVersions[0].CaseId);
            Assert.AreEqual(fieldValue.DoneAt.ToString(), fieldValueVersions[0].DoneAt.ToString());
            Assert.AreEqual(field.Id, fieldValueVersions[0].FieldId);
            Assert.AreEqual(worker.Id, fieldValueVersions[0].WorkerId);
            Assert.AreEqual(checklist.Id, fieldValueVersions[0].CheckListId);
            Assert.AreEqual(uploadedData.Id, fieldValueVersions[0].UploadedDataId);
            
            //New Version
            Assert.AreEqual(fieldValue.CreatedAt.ToString(), fieldValueVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, fieldValueVersions[1].Version);                                      
            Assert.AreEqual(fieldValue.UpdatedAt.ToString(), fieldValueVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldValueVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(fieldValue.Id, fieldValueVersions[1].FieldId);
            Assert.AreEqual(fieldValue.Accuracy, fieldValueVersions[1].Accuracy);
            Assert.AreEqual(fieldValue.Date.ToString(), fieldValueVersions[1].Date.ToString());
            Assert.AreEqual(fieldValue.Heading, fieldValueVersions[1].Heading);
            Assert.AreEqual(fieldValue.Latitude, fieldValueVersions[1].Latitude);
            Assert.AreEqual(fieldValue.Longitude, fieldValueVersions[1].Longitude);
            Assert.AreEqual(fieldValue.Value, fieldValueVersions[1].Value);
            Assert.AreEqual(theCase.Id, fieldValueVersions[1].CaseId);
            Assert.AreEqual(fieldValue.DoneAt.ToString(), fieldValueVersions[1].DoneAt.ToString());
            Assert.AreEqual(field.Id, fieldValueVersions[1].FieldId);
            Assert.AreEqual(worker.Id, fieldValueVersions[1].WorkerId);
            Assert.AreEqual(checklist.Id, fieldValueVersions[1].CheckListId);
            Assert.AreEqual(uploadedData.Id, fieldValueVersions[1].UploadedDataId);
        }
    }
}