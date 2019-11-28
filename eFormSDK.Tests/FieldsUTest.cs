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
    public class FieldsUTest : DbTestFixture
    {
        [Test]
        public async Task Fields_Create_DoesCreate()
        {
           short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            Random rnd = new Random();
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            await site.Create(dbContext);
            
            units unit = new units();
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);
            unit.SiteId = site.Id;
            await unit.Create(dbContext);
            
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
            await checklist.Create(dbContext);
            
            entity_groups entityGroup = new entity_groups();
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            await entityGroup.Create(dbContext);

            field_types fieldType = dbContext.field_types.First(); 
            
            fields parentFIeld = new fields();
            parentFIeld.Color = Guid.NewGuid().ToString();
            parentFIeld.Custom = Guid.NewGuid().ToString();
            parentFIeld.Description = Guid.NewGuid().ToString();
            parentFIeld.Dummy = (short)rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.Label = Guid.NewGuid().ToString();
            parentFIeld.Mandatory = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.Multi = rnd.Next(1, 255);
            parentFIeld.Optional = (short)rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.Selected = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.BarcodeEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.BarcodeType = Guid.NewGuid().ToString();
            parentFIeld.DecimalCount = rnd.Next(1, 255);
            parentFIeld.DefaultValue = Guid.NewGuid().ToString();
            parentFIeld.DisplayIndex = rnd.Next(1, 255);
            parentFIeld.GeolocationEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.GeolocationForced = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.GeolocationHidden = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.IsNum = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.MaxLength = rnd.Next(1, 255);
            parentFIeld.MaxValue = Guid.NewGuid().ToString();
            parentFIeld.MinValue = Guid.NewGuid().ToString();
            parentFIeld.OriginalId = Guid.NewGuid().ToString();
            parentFIeld.QueryType = Guid.NewGuid().ToString();
            parentFIeld.ReadOnly = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.SplitScreen = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.UnitName = Guid.NewGuid().ToString();
            parentFIeld.StopOnSave = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.KeyValuePairList = Guid.NewGuid().ToString();
            parentFIeld.CheckListId = checklist.Id;
            parentFIeld.EntityGroupId = entityGroup.Id;
            parentFIeld.FieldTypeId = fieldType.Id;
            await parentFIeld.Create(dbContext);
            
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
            field.ParentFieldId = parentFIeld.Id;
            
            //Act
            
            await field.Create(dbContext);
            
            List<fields> fields = dbContext.fields.AsNoTracking().ToList();
            List<field_versions> fieldVersion = dbContext.field_versions.AsNoTracking().ToList();
            
            Assert.NotNull(fields);                                                             
            Assert.NotNull(fieldVersion);                                                             

            Assert.AreEqual(2,fields.Count());  
            Assert.AreEqual(2,fieldVersion.Count()); 
            
            Assert.AreEqual(field.CreatedAt.ToString(), fields[1].CreatedAt.ToString());                                  
            Assert.AreEqual(field.Version, fields[1].Version);                                      
//            Assert.AreEqual(field.UpdatedAt.ToString(), fields[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(fields[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(field.Id, fields[1].Id);
            Assert.AreEqual(field.Color, fields[1].Color);
            Assert.AreEqual(field.Custom, fields[1].Custom);
            Assert.AreEqual(field.Description, fields[1].Description);
            Assert.AreEqual(field.Dummy, fields[1].Dummy);
            Assert.AreEqual(field.Label, fields[1].Label);
            Assert.AreEqual(field.Mandatory, fields[1].Mandatory);
            Assert.AreEqual(field.Multi, fields[1].Multi);
            Assert.AreEqual(field.Optional, fields[1].Optional);
            Assert.AreEqual(field.Selected, fields[1].Selected);
            Assert.AreEqual(field.BarcodeEnabled, fields[1].BarcodeEnabled);
            Assert.AreEqual(field.BarcodeType, fields[1].BarcodeType);
            Assert.AreEqual(field.DecimalCount, fields[1].DecimalCount);
            Assert.AreEqual(field.DefaultValue, fields[1].DefaultValue);
            Assert.AreEqual(field.DisplayIndex, fields[1].DisplayIndex);
            Assert.AreEqual(field.GeolocationEnabled, fields[1].GeolocationEnabled);
            Assert.AreEqual(field.GeolocationForced, fields[1].GeolocationForced);
            Assert.AreEqual(field.GeolocationHidden, fields[1].GeolocationHidden);
            Assert.AreEqual(field.IsNum, fields[1].IsNum);
            Assert.AreEqual(field.MaxLength, fields[1].MaxLength);
            Assert.AreEqual(field.MaxValue, fields[1].MaxValue);
            Assert.AreEqual(field.MinValue, fields[1].MinValue);
            Assert.AreEqual(field.OriginalId, fields[1].OriginalId);
            Assert.AreEqual(field.ReadOnly, fields[1].ReadOnly);
            Assert.AreEqual(field.SplitScreen, fields[1].SplitScreen);
            Assert.AreEqual(field.QueryType, fields[1].QueryType);
            Assert.AreEqual(field.UnitName, fields[1].UnitName);
            Assert.AreEqual(field.EntityGroupId, entityGroup.Id);
            Assert.AreEqual(field.FieldTypeId, fieldType.Id);
            Assert.AreEqual(field.ParentFieldId, fields[1].ParentFieldId);
            Assert.AreEqual(field.CheckListId, checklist.Id);
            Assert.AreEqual(field.StopOnSave, fields[1].StopOnSave);
            Assert.AreEqual(field.KeyValuePairList, fields[1].KeyValuePairList);
         
            
            //Versions
            Assert.AreEqual(field.CreatedAt.ToString(), fieldVersion[1].CreatedAt.ToString());                                  
            Assert.AreEqual(1, fieldVersion[1].Version);                                      
//            Assert.AreEqual(field.UpdatedAt.ToString(), fieldVersion[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldVersion[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(field.Id, fieldVersion[1].FieldId);
            Assert.AreEqual(field.Color, fieldVersion[1].Color);
            Assert.AreEqual(field.Custom, fieldVersion[1].Custom);
            Assert.AreEqual(field.Description, fieldVersion[1].Description);
            Assert.AreEqual(field.Dummy, fieldVersion[1].Dummy);
            Assert.AreEqual(field.Label, fieldVersion[1].Label);
            Assert.AreEqual(field.Mandatory, fieldVersion[1].Mandatory);
            Assert.AreEqual(field.Multi, fieldVersion[1].Multi);
            Assert.AreEqual(field.Optional, fieldVersion[1].Optional);
            Assert.AreEqual(field.Selected, fieldVersion[1].Selected);
            Assert.AreEqual(field.BarcodeEnabled, fieldVersion[1].BarcodeEnabled);
            Assert.AreEqual(field.BarcodeType, fieldVersion[1].BarcodeType);
            Assert.AreEqual(field.DecimalCount, fieldVersion[1].DecimalCount);
            Assert.AreEqual(field.DefaultValue, fieldVersion[1].DefaultValue);
            Assert.AreEqual(field.DisplayIndex, fieldVersion[1].DisplayIndex);
            Assert.AreEqual(field.GeolocationEnabled, fieldVersion[1].GeolocationEnabled);
            Assert.AreEqual(field.GeolocationForced, fieldVersion[1].GeolocationForced);
            Assert.AreEqual(field.GeolocationHidden, fieldVersion[1].GeolocationHidden);
            Assert.AreEqual(field.IsNum, fieldVersion[1].IsNum);
            Assert.AreEqual(field.MaxLength, fieldVersion[1].MaxLength);
            Assert.AreEqual(field.MaxValue, fieldVersion[1].MaxValue);
            Assert.AreEqual(field.MinValue, fieldVersion[1].MinValue);
            Assert.AreEqual(field.OriginalId, fieldVersion[1].OriginalId);
            Assert.AreEqual(field.ReadOnly, fieldVersion[1].ReadOnly);
            Assert.AreEqual(field.SplitScreen, fieldVersion[1].SplitScreen);
            Assert.AreEqual(field.QueryType, fieldVersion[1].QueryType);
            Assert.AreEqual(field.UnitName, fieldVersion[1].UnitName);
            Assert.AreEqual(entityGroup.Id, fieldVersion[1].EntityGroupId);
            Assert.AreEqual(fieldType.Id, fieldVersion[1].FieldTypeId);
            Assert.AreEqual(field.ParentFieldId, fieldVersion[1].ParentFieldId);
            Assert.AreEqual(checklist.Id, fieldVersion[1].CheckListId);
            Assert.AreEqual(field.StopOnSave, fieldVersion[1].StopOnSave);
            Assert.AreEqual(field.KeyValuePairList, fieldVersion[1].KeyValuePairList);
        }

        [Test]
        public async Task Fields_Update_DoesUpdate()
        {
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            Random rnd = new Random();
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            await site.Create(dbContext);
            
            units unit = new units();
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);
            unit.SiteId = site.Id;
            await unit.Create(dbContext);
            
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
            await checklist.Create(dbContext);
            
            entity_groups entityGroup = new entity_groups();
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            await entityGroup.Create(dbContext);
            
            field_types fieldType =dbContext.field_types.First(); 
            
            fields parentFIeld = new fields();
            parentFIeld.Color = Guid.NewGuid().ToString();
            parentFIeld.Custom = Guid.NewGuid().ToString();
            parentFIeld.Description = Guid.NewGuid().ToString();
            parentFIeld.Dummy = (short)rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.Label = Guid.NewGuid().ToString();
            parentFIeld.Mandatory = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.Multi = rnd.Next(1, 255);
            parentFIeld.Optional = (short)rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.Selected = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.BarcodeEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.BarcodeType = Guid.NewGuid().ToString();
            parentFIeld.DecimalCount = rnd.Next(1, 255);
            parentFIeld.DefaultValue = Guid.NewGuid().ToString();
            parentFIeld.DisplayIndex = rnd.Next(1, 255);
            parentFIeld.GeolocationEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.GeolocationForced = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.GeolocationHidden = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.IsNum = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.MaxLength = rnd.Next(1, 255);
            parentFIeld.MaxValue = Guid.NewGuid().ToString();
            parentFIeld.MinValue = Guid.NewGuid().ToString();
            parentFIeld.OriginalId = Guid.NewGuid().ToString();
            parentFIeld.QueryType = Guid.NewGuid().ToString();
            parentFIeld.ReadOnly = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.SplitScreen = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.UnitName = Guid.NewGuid().ToString();
            parentFIeld.StopOnSave = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.KeyValuePairList = Guid.NewGuid().ToString();
            parentFIeld.CheckListId = checklist.Id;
            parentFIeld.EntityGroupId = entityGroup.Id;
            parentFIeld.FieldTypeId = fieldType.Id;
            await parentFIeld.Create(dbContext);
            
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
            field.ParentFieldId = parentFIeld.Id;
            await field.Create(dbContext);
            
            //Act

            DateTime? oldUpdatedAt = field.UpdatedAt;
            string oldColor = field.Color;
            string oldCustom = field.Custom;
            string oldDescription = field.Description;
            short? oldDummy = field.Dummy;
            string oldLabel = field.Label;
            short? oldMandatory = field.Mandatory;
            int? oldMulti = field.Multi;
            short? oldOptional = field.Optional;
            short? oldSelected = field.Selected;
            short? oldBarcodeEnabled = field.BarcodeEnabled;
            string oldBarcodeType = field.BarcodeType;
            int? oldDecimalCount = field.DecimalCount;
            string oldDefaultValue = field.DefaultValue;
            int? oldDisplayIndex = field.DisplayIndex;
            short? oldGeolocationEnabled = field.GeolocationEnabled;
            short? oldGeolocationForced = field.GeolocationForced;
            short? oldGeolocationHidden = field.GeolocationHidden;
            short? oldIsNum = field.IsNum;
            int? oldMaxLength = field.MaxLength;
            string oldMaxValue = field.MaxValue;
            string oldMinValue = field.MinValue;
            string oldOriginalId = field.OriginalId;
            string oldQueryType = field.QueryType;
            short? oldReadonly = field.ReadOnly;
            short? oldSplitscreen = field.SplitScreen;
            string oldUnitName = field.UnitName;
            short? oldStopOnSave = field.StopOnSave;
            string oldKeyValuePairList = field.KeyValuePairList;
            
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

            await field.Update(dbContext);

            List<fields> fields = dbContext.fields.AsNoTracking().ToList();
            List<field_versions> fieldVersion = dbContext.field_versions.AsNoTracking().ToList();
            
            Assert.NotNull(fields);                                                             
            Assert.NotNull(fieldVersion);                                                             

            Assert.AreEqual(2,fields.Count());  
            Assert.AreEqual(3,fieldVersion.Count()); 
            
            Assert.AreEqual(field.CreatedAt.ToString(), fields[1].CreatedAt.ToString());                                  
            Assert.AreEqual(field.Version, fields[1].Version);                                      
//            Assert.AreEqual(field.UpdatedAt.ToString(), fields[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(fields[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(field.Id, fields[1].Id);
            Assert.AreEqual(field.Color, fields[1].Color);
            Assert.AreEqual(field.Custom, fields[1].Custom);
            Assert.AreEqual(field.Description, fields[1].Description);
            Assert.AreEqual(field.Dummy, fields[1].Dummy);
            Assert.AreEqual(field.Label, fields[1].Label);
            Assert.AreEqual(field.Mandatory, fields[1].Mandatory);
            Assert.AreEqual(field.Multi, fields[1].Multi);
            Assert.AreEqual(field.Optional, fields[1].Optional);
            Assert.AreEqual(field.Selected, fields[1].Selected);
            Assert.AreEqual(field.BarcodeEnabled, fields[1].BarcodeEnabled);
            Assert.AreEqual(field.BarcodeType, fields[1].BarcodeType);
            Assert.AreEqual(field.DecimalCount, fields[1].DecimalCount);
            Assert.AreEqual(field.DefaultValue, fields[1].DefaultValue);
            Assert.AreEqual(field.DisplayIndex, fields[1].DisplayIndex);
            Assert.AreEqual(field.GeolocationEnabled, fields[1].GeolocationEnabled);
            Assert.AreEqual(field.GeolocationForced, fields[1].GeolocationForced);
            Assert.AreEqual(field.GeolocationHidden, fields[1].GeolocationHidden);
            Assert.AreEqual(field.IsNum, fields[1].IsNum);
            Assert.AreEqual(field.MaxLength, fields[1].MaxLength);
            Assert.AreEqual(field.MaxValue, fields[1].MaxValue);
            Assert.AreEqual(field.MinValue, fields[1].MinValue);
            Assert.AreEqual(field.OriginalId, fields[1].OriginalId);
            Assert.AreEqual(field.ReadOnly, fields[1].ReadOnly);
            Assert.AreEqual(field.SplitScreen, fields[1].SplitScreen);
            Assert.AreEqual(field.QueryType, fields[1].QueryType);
            Assert.AreEqual(field.UnitName, fields[1].UnitName);
            Assert.AreEqual(field.EntityGroupId, entityGroup.Id);
            Assert.AreEqual(field.FieldTypeId, fieldType.Id);
            Assert.AreEqual(field.ParentFieldId, fields[1].ParentFieldId);
            Assert.AreEqual(field.CheckListId, checklist.Id);
            Assert.AreEqual(field.StopOnSave, fields[1].StopOnSave);
            Assert.AreEqual(field.KeyValuePairList, fields[1].KeyValuePairList);
         
            
            //Old Version
            Assert.AreEqual(field.CreatedAt.ToString(), fieldVersion[1].CreatedAt.ToString());                                  
            Assert.AreEqual(1, fieldVersion[1].Version);                                      
//            Assert.AreEqual(oldUpdatedAt.ToString(), fieldVersion[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldVersion[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(field.Id, fieldVersion[1].FieldId);
            Assert.AreEqual(oldColor, fieldVersion[1].Color);
            Assert.AreEqual(oldCustom, fieldVersion[1].Custom);
            Assert.AreEqual(oldDescription, fieldVersion[1].Description);
            Assert.AreEqual(oldDummy, fieldVersion[1].Dummy);
            Assert.AreEqual(oldLabel, fieldVersion[1].Label);
            Assert.AreEqual(oldMandatory, fieldVersion[1].Mandatory);
            Assert.AreEqual(oldMulti, fieldVersion[1].Multi);
            Assert.AreEqual(oldOptional, fieldVersion[1].Optional);
            Assert.AreEqual(oldSelected, fieldVersion[1].Selected);
            Assert.AreEqual(oldBarcodeEnabled, fieldVersion[1].BarcodeEnabled);
            Assert.AreEqual(oldBarcodeType, fieldVersion[1].BarcodeType);
            Assert.AreEqual(oldDecimalCount, fieldVersion[1].DecimalCount);
            Assert.AreEqual(oldDefaultValue, fieldVersion[1].DefaultValue);
            Assert.AreEqual(oldDisplayIndex, fieldVersion[1].DisplayIndex);
            Assert.AreEqual(oldGeolocationEnabled, fieldVersion[1].GeolocationEnabled);
            Assert.AreEqual(oldGeolocationForced, fieldVersion[1].GeolocationForced);
            Assert.AreEqual(oldGeolocationHidden, fieldVersion[1].GeolocationHidden);
            Assert.AreEqual(oldIsNum, fieldVersion[1].IsNum);
            Assert.AreEqual(oldMaxLength, fieldVersion[1].MaxLength);
            Assert.AreEqual(oldMaxValue, fieldVersion[1].MaxValue);
            Assert.AreEqual(oldMinValue, fieldVersion[1].MinValue);
            Assert.AreEqual(oldOriginalId, fieldVersion[1].OriginalId);
            Assert.AreEqual(oldReadonly, fieldVersion[1].ReadOnly);
            Assert.AreEqual(oldSplitscreen, fieldVersion[1].SplitScreen);
            Assert.AreEqual(oldQueryType, fieldVersion[1].QueryType);
            Assert.AreEqual(oldUnitName, fieldVersion[1].UnitName);
            Assert.AreEqual(entityGroup.Id, fieldVersion[1].EntityGroupId);
            Assert.AreEqual(fieldType.Id, fieldVersion[1].FieldTypeId);
            Assert.AreEqual(field.ParentFieldId, fieldVersion[1].ParentFieldId);
            Assert.AreEqual(checklist.Id, fieldVersion[1].CheckListId);
            Assert.AreEqual(oldStopOnSave, fieldVersion[1].StopOnSave);
            Assert.AreEqual(oldKeyValuePairList, fieldVersion[1].KeyValuePairList);
            
            //New Version
            Assert.AreEqual(field.CreatedAt.ToString(), fieldVersion[2].CreatedAt.ToString());                                  
            Assert.AreEqual(2, fieldVersion[2].Version);                                      
//            Assert.AreEqual(field.UpdatedAt.ToString(), fieldVersion[2].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldVersion[2].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(field.Id, fieldVersion[2].FieldId);
            Assert.AreEqual(field.Color, fieldVersion[2].Color);
            Assert.AreEqual(field.Custom, fieldVersion[2].Custom);
            Assert.AreEqual(field.Description, fieldVersion[2].Description);
            Assert.AreEqual(field.Dummy, fieldVersion[2].Dummy);
            Assert.AreEqual(field.Label, fieldVersion[2].Label);
            Assert.AreEqual(field.Mandatory, fieldVersion[2].Mandatory);
            Assert.AreEqual(field.Multi, fieldVersion[2].Multi);
            Assert.AreEqual(field.Optional, fieldVersion[2].Optional);
            Assert.AreEqual(field.Selected, fieldVersion[2].Selected);
            Assert.AreEqual(field.BarcodeEnabled, fieldVersion[2].BarcodeEnabled);
            Assert.AreEqual(field.BarcodeType, fieldVersion[2].BarcodeType);
            Assert.AreEqual(field.DecimalCount, fieldVersion[2].DecimalCount);
            Assert.AreEqual(field.DefaultValue, fieldVersion[2].DefaultValue);
            Assert.AreEqual(field.DisplayIndex, fieldVersion[2].DisplayIndex);
            Assert.AreEqual(field.GeolocationEnabled, fieldVersion[2].GeolocationEnabled);
            Assert.AreEqual(field.GeolocationForced, fieldVersion[2].GeolocationForced);
            Assert.AreEqual(field.GeolocationHidden, fieldVersion[2].GeolocationHidden);
            Assert.AreEqual(field.IsNum, fieldVersion[2].IsNum);
            Assert.AreEqual(field.MaxLength, fieldVersion[2].MaxLength);
            Assert.AreEqual(field.MaxValue, fieldVersion[2].MaxValue);
            Assert.AreEqual(field.MinValue, fieldVersion[2].MinValue);
            Assert.AreEqual(field.OriginalId, fieldVersion[2].OriginalId);
            Assert.AreEqual(field.ReadOnly, fieldVersion[2].ReadOnly);
            Assert.AreEqual(field.SplitScreen, fieldVersion[2].SplitScreen);
            Assert.AreEqual(field.QueryType, fieldVersion[2].QueryType);
            Assert.AreEqual(field.UnitName, fieldVersion[2].UnitName);
            Assert.AreEqual(entityGroup.Id, fieldVersion[2].EntityGroupId);
            Assert.AreEqual(fieldType.Id, fieldVersion[2].FieldTypeId);
            Assert.AreEqual(field.ParentFieldId, fieldVersion[2].ParentFieldId);
            Assert.AreEqual(checklist.Id, fieldVersion[2].CheckListId);
            Assert.AreEqual(field.StopOnSave, fieldVersion[2].StopOnSave);
            Assert.AreEqual(field.KeyValuePairList, fieldVersion[2].KeyValuePairList);
        }

        [Test]
        public async Task Fields_Delete_DoesSetWorkflowStateToRemoved()
        {
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            Random rnd = new Random();
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            await site.Create(dbContext);
            
            units unit = new units();
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);
            unit.SiteId = site.Id;
            await unit.Create(dbContext);
            
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
            await checklist.Create(dbContext);
            
            entity_groups entityGroup = new entity_groups();
            entityGroup.Name = Guid.NewGuid().ToString();
            entityGroup.Type = Guid.NewGuid().ToString();
            entityGroup.MicrotingUid = Guid.NewGuid().ToString();
            await entityGroup.Create(dbContext);
            
            field_types fieldType = dbContext.field_types.First();
            
            fields parentFIeld = new fields();
            parentFIeld.Color = Guid.NewGuid().ToString();
            parentFIeld.Custom = Guid.NewGuid().ToString();
            parentFIeld.Description = Guid.NewGuid().ToString();
            parentFIeld.Dummy = (short)rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.Label = Guid.NewGuid().ToString();
            parentFIeld.Mandatory = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.Multi = rnd.Next(1, 255);
            parentFIeld.Optional = (short)rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.Selected = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.BarcodeEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.BarcodeType = Guid.NewGuid().ToString();
            parentFIeld.DecimalCount = rnd.Next(1, 255);
            parentFIeld.DefaultValue = Guid.NewGuid().ToString();
            parentFIeld.DisplayIndex = rnd.Next(1, 255);
            parentFIeld.GeolocationEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.GeolocationForced = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.GeolocationHidden = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.IsNum = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.MaxLength = rnd.Next(1, 255);
            parentFIeld.MaxValue = Guid.NewGuid().ToString();
            parentFIeld.MinValue = Guid.NewGuid().ToString();
            parentFIeld.OriginalId = Guid.NewGuid().ToString();
            parentFIeld.QueryType = Guid.NewGuid().ToString();
            parentFIeld.ReadOnly = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.SplitScreen = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.UnitName = Guid.NewGuid().ToString();
            parentFIeld.StopOnSave = (short) rnd.Next(shortMinValue, shortmaxValue);
            parentFIeld.KeyValuePairList = Guid.NewGuid().ToString();
            parentFIeld.CheckListId = checklist.Id;
            parentFIeld.EntityGroupId = entityGroup.Id;
            parentFIeld.FieldTypeId = fieldType.Id;
            await parentFIeld.Create(dbContext);
            
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
            field.ParentFieldId = parentFIeld.Id;
            await field.Create(dbContext);
            
            //Act

            DateTime? oldUpdatedAt = field.UpdatedAt;

            await field.Delete(dbContext);

            List<fields> fields = dbContext.fields.AsNoTracking().ToList();
            List<field_versions> fieldVersion = dbContext.field_versions.AsNoTracking().ToList();
            
            Assert.NotNull(fields);                                                             
            Assert.NotNull(fieldVersion);                                                             

            Assert.AreEqual(2,fields.Count());  
            Assert.AreEqual(3,fieldVersion.Count()); 
            
            Assert.AreEqual(field.CreatedAt.ToString(), fields[1].CreatedAt.ToString());                                  
            Assert.AreEqual(field.Version, fields[1].Version);                                      
//            Assert.AreEqual(field.UpdatedAt.ToString(), fields[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(fields[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(field.Id, fields[1].Id);
            Assert.AreEqual(field.Color, fields[1].Color);
            Assert.AreEqual(field.Custom, fields[1].Custom);
            Assert.AreEqual(field.Description, fields[1].Description);
            Assert.AreEqual(field.Dummy, fields[1].Dummy);
            Assert.AreEqual(field.Label, fields[1].Label);
            Assert.AreEqual(field.Mandatory, fields[1].Mandatory);
            Assert.AreEqual(field.Multi, fields[1].Multi);
            Assert.AreEqual(field.Optional, fields[1].Optional);
            Assert.AreEqual(field.Selected, fields[1].Selected);
            Assert.AreEqual(field.BarcodeEnabled, fields[1].BarcodeEnabled);
            Assert.AreEqual(field.BarcodeType, fields[1].BarcodeType);
            Assert.AreEqual(field.DecimalCount, fields[1].DecimalCount);
            Assert.AreEqual(field.DefaultValue, fields[1].DefaultValue);
            Assert.AreEqual(field.DisplayIndex, fields[1].DisplayIndex);
            Assert.AreEqual(field.GeolocationEnabled, fields[1].GeolocationEnabled);
            Assert.AreEqual(field.GeolocationForced, fields[1].GeolocationForced);
            Assert.AreEqual(field.GeolocationHidden, fields[1].GeolocationHidden);
            Assert.AreEqual(field.IsNum, fields[1].IsNum);
            Assert.AreEqual(field.MaxLength, fields[1].MaxLength);
            Assert.AreEqual(field.MaxValue, fields[1].MaxValue);
            Assert.AreEqual(field.MinValue, fields[1].MinValue);
            Assert.AreEqual(field.OriginalId, fields[1].OriginalId);
            Assert.AreEqual(field.ReadOnly, fields[1].ReadOnly);
            Assert.AreEqual(field.SplitScreen, fields[1].SplitScreen);
            Assert.AreEqual(field.QueryType, fields[1].QueryType);
            Assert.AreEqual(field.UnitName, fields[1].UnitName);
            Assert.AreEqual(field.EntityGroupId, entityGroup.Id);
            Assert.AreEqual(field.FieldTypeId, fieldType.Id);
            Assert.AreEqual(field.ParentFieldId, fields[1].ParentFieldId);
            Assert.AreEqual(field.CheckListId, checklist.Id);
            Assert.AreEqual(field.StopOnSave, fields[1].StopOnSave);
            Assert.AreEqual(field.KeyValuePairList, fields[1].KeyValuePairList);
         
            
            //Old Version
            Assert.AreEqual(field.CreatedAt.ToString(), fieldVersion[1].CreatedAt.ToString());                                  
            Assert.AreEqual(1, fieldVersion[1].Version);                                      
//            Assert.AreEqual(oldUpdatedAt.ToString(), fieldVersion[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldVersion[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(field.Id, fieldVersion[1].FieldId);
            Assert.AreEqual(field.Color, fieldVersion[1].Color);
            Assert.AreEqual(field.Custom, fieldVersion[1].Custom);
            Assert.AreEqual(field.Description, fieldVersion[1].Description);
            Assert.AreEqual(field.Dummy, fieldVersion[1].Dummy);
            Assert.AreEqual(field.Label, fieldVersion[1].Label);
            Assert.AreEqual(field.Mandatory, fieldVersion[1].Mandatory);
            Assert.AreEqual(field.Multi, fieldVersion[1].Multi);
            Assert.AreEqual(field.Optional, fieldVersion[1].Optional);
            Assert.AreEqual(field.Selected, fieldVersion[1].Selected);
            Assert.AreEqual(field.BarcodeEnabled, fieldVersion[1].BarcodeEnabled);
            Assert.AreEqual(field.BarcodeType, fieldVersion[1].BarcodeType);
            Assert.AreEqual(field.DecimalCount, fieldVersion[1].DecimalCount);
            Assert.AreEqual(field.DefaultValue, fieldVersion[1].DefaultValue);
            Assert.AreEqual(field.DisplayIndex, fieldVersion[1].DisplayIndex);
            Assert.AreEqual(field.GeolocationEnabled, fieldVersion[1].GeolocationEnabled);
            Assert.AreEqual(field.GeolocationForced, fieldVersion[1].GeolocationForced);
            Assert.AreEqual(field.GeolocationHidden, fieldVersion[1].GeolocationHidden);
            Assert.AreEqual(field.IsNum, fieldVersion[1].IsNum);
            Assert.AreEqual(field.MaxLength, fieldVersion[1].MaxLength);
            Assert.AreEqual(field.MaxValue, fieldVersion[1].MaxValue);
            Assert.AreEqual(field.MinValue, fieldVersion[1].MinValue);
            Assert.AreEqual(field.OriginalId, fieldVersion[1].OriginalId);
            Assert.AreEqual(field.ReadOnly, fieldVersion[1].ReadOnly);
            Assert.AreEqual(field.SplitScreen, fieldVersion[1].SplitScreen);
            Assert.AreEqual(field.QueryType, fieldVersion[1].QueryType);
            Assert.AreEqual(field.UnitName, fieldVersion[1].UnitName);
            Assert.AreEqual(entityGroup.Id, fieldVersion[1].EntityGroupId);
            Assert.AreEqual(fieldType.Id, fieldVersion[1].FieldTypeId);
            Assert.AreEqual(field.ParentFieldId, fieldVersion[1].ParentFieldId);
            Assert.AreEqual(checklist.Id, fieldVersion[1].CheckListId);
            Assert.AreEqual(field.StopOnSave, fieldVersion[1].StopOnSave);
            Assert.AreEqual(field.KeyValuePairList, fieldVersion[1].KeyValuePairList);
            
            //New Version
            Assert.AreEqual(field.CreatedAt.ToString(), fieldVersion[2].CreatedAt.ToString());                                  
            Assert.AreEqual(2, fieldVersion[2].Version);                                      
//            Assert.AreEqual(field.UpdatedAt.ToString(), fieldVersion[2].UpdatedAt.ToString());                                  
            Assert.AreEqual(fieldVersion[2].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(field.Id, fieldVersion[2].FieldId);
            Assert.AreEqual(field.Color, fieldVersion[2].Color);
            Assert.AreEqual(field.Custom, fieldVersion[2].Custom);
            Assert.AreEqual(field.Description, fieldVersion[2].Description);
            Assert.AreEqual(field.Dummy, fieldVersion[2].Dummy);
            Assert.AreEqual(field.Label, fieldVersion[2].Label);
            Assert.AreEqual(field.Mandatory, fieldVersion[2].Mandatory);
            Assert.AreEqual(field.Multi, fieldVersion[2].Multi);
            Assert.AreEqual(field.Optional, fieldVersion[2].Optional);
            Assert.AreEqual(field.Selected, fieldVersion[2].Selected);
            Assert.AreEqual(field.BarcodeEnabled, fieldVersion[2].BarcodeEnabled);
            Assert.AreEqual(field.BarcodeType, fieldVersion[2].BarcodeType);
            Assert.AreEqual(field.DecimalCount, fieldVersion[2].DecimalCount);
            Assert.AreEqual(field.DefaultValue, fieldVersion[2].DefaultValue);
            Assert.AreEqual(field.DisplayIndex, fieldVersion[2].DisplayIndex);
            Assert.AreEqual(field.GeolocationEnabled, fieldVersion[2].GeolocationEnabled);
            Assert.AreEqual(field.GeolocationForced, fieldVersion[2].GeolocationForced);
            Assert.AreEqual(field.GeolocationHidden, fieldVersion[2].GeolocationHidden);
            Assert.AreEqual(field.IsNum, fieldVersion[2].IsNum);
            Assert.AreEqual(field.MaxLength, fieldVersion[2].MaxLength);
            Assert.AreEqual(field.MaxValue, fieldVersion[2].MaxValue);
            Assert.AreEqual(field.MinValue, fieldVersion[2].MinValue);
            Assert.AreEqual(field.OriginalId, fieldVersion[2].OriginalId);
            Assert.AreEqual(field.ReadOnly, fieldVersion[2].ReadOnly);
            Assert.AreEqual(field.SplitScreen, fieldVersion[2].SplitScreen);
            Assert.AreEqual(field.QueryType, fieldVersion[2].QueryType);
            Assert.AreEqual(field.UnitName, fieldVersion[2].UnitName);
            Assert.AreEqual(entityGroup.Id, fieldVersion[2].EntityGroupId);
            Assert.AreEqual(fieldType.Id, fieldVersion[2].FieldTypeId);
            Assert.AreEqual(field.ParentFieldId, fieldVersion[2].ParentFieldId);
            Assert.AreEqual(checklist.Id, fieldVersion[2].CheckListId);
            Assert.AreEqual(field.StopOnSave, fieldVersion[2].StopOnSave);
            Assert.AreEqual(field.KeyValuePairList, fieldVersion[2].KeyValuePairList); 
        }
    }
}