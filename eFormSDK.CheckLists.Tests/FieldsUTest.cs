/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.CheckLists.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
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

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Unit unit = new Unit
            {
                CustomerNo = rnd.Next(1, 255),
                MicrotingUid = rnd.Next(1, 255),
                OtpCode = rnd.Next(1, 255),
                SiteId = site.Id
            };
            await unit.Create(DbContext).ConfigureAwait(false);

            CheckList checklist = new CheckList
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Field1 = rnd.Next(1, 255),
                Field2 = rnd.Next(1, 255),
                Field4 = rnd.Next(1, 255),
                Field5 = rnd.Next(1, 255),
                Field6 = rnd.Next(1, 255),
                Field7 = rnd.Next(1, 255),
                Field8 = rnd.Next(1, 255),
                Field9 = rnd.Next(1, 255),
                Field10 = rnd.Next(1, 255),
                Label = Guid.NewGuid().ToString(),
                Repeated = rnd.Next(1, 255),
                ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                CaseType = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                DownloadEntities = (short)rnd.Next(shortMinValue, shortmaxValue),
                FastNavigation = (short)rnd.Next(shortMinValue, shortmaxValue),
                FolderName = Guid.NewGuid().ToString(),
                ManualSync = (short)rnd.Next(shortMinValue, shortmaxValue),
                MultiApproval = (short)rnd.Next(shortMinValue, shortmaxValue),
                OriginalId = Guid.NewGuid().ToString(),
                ReviewEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                DocxExportEnabled = randomBool,
                DoneButtonEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ExtraFieldsEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                JasperExportEnabled = randomBool,
                QuickSyncEnabled = (short)rnd.Next(shortMinValue, shortmaxValue)
            };
            await checklist.Create(DbContext).ConfigureAwait(false);

            EntityGroup entityGroup = new EntityGroup
            {
                Name = Guid.NewGuid().ToString(),
                Type = Guid.NewGuid().ToString(),
                MicrotingUid = Guid.NewGuid().ToString()
            };
            await entityGroup.Create(DbContext).ConfigureAwait(false);

            FieldType fieldType = new FieldType
            {
                Description = Guid.NewGuid().ToString(),
                Type = Guid.NewGuid().ToString()
            };
            await fieldType.Create(DbContext).ConfigureAwait(false);

            Field parentFIeld = new Field
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Dummy = (short)rnd.Next(shortMinValue, shortmaxValue),
                Label = Guid.NewGuid().ToString(),
                Mandatory = (short)rnd.Next(shortMinValue, shortmaxValue),
                Multi = rnd.Next(1, 255),
                Optional = (short)rnd.Next(shortMinValue, shortmaxValue),
                Selected = (short)rnd.Next(shortMinValue, shortmaxValue),
                BarcodeEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                BarcodeType = Guid.NewGuid().ToString(),
                DecimalCount = rnd.Next(1, 255),
                DefaultValue = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                GeolocationEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                GeolocationForced = (short)rnd.Next(shortMinValue, shortmaxValue),
                GeolocationHidden = (short)rnd.Next(shortMinValue, shortmaxValue),
                IsNum = (short)rnd.Next(shortMinValue, shortmaxValue),
                MaxLength = rnd.Next(1, 255),
                MaxValue = Guid.NewGuid().ToString(),
                MinValue = Guid.NewGuid().ToString(),
                OriginalId = Guid.NewGuid().ToString(),
                QueryType = Guid.NewGuid().ToString(),
                ReadOnly = (short)rnd.Next(shortMinValue, shortmaxValue),
                Split = (short)rnd.Next(shortMinValue, shortmaxValue),
                UnitName = Guid.NewGuid().ToString(),
                StopOnSave = (short)rnd.Next(shortMinValue, shortmaxValue),
                KeyValuePairList = Guid.NewGuid().ToString(),
                CheckListId = checklist.Id,
                EntityGroupId = entityGroup.Id,
                FieldTypeId = fieldType.Id
            };
            await parentFIeld.Create(DbContext).ConfigureAwait(false);

            Field field = new Field
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Dummy = (short)rnd.Next(shortMinValue, shortmaxValue),
                Label = Guid.NewGuid().ToString(),
                Mandatory = (short)rnd.Next(shortMinValue, shortmaxValue),
                Multi = rnd.Next(1, 255),
                Optional = (short)rnd.Next(shortMinValue, shortmaxValue),
                Selected = (short)rnd.Next(shortMinValue, shortmaxValue),
                BarcodeEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                BarcodeType = Guid.NewGuid().ToString(),
                DecimalCount = rnd.Next(1, 255),
                DefaultValue = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                GeolocationEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                GeolocationForced = (short)rnd.Next(shortMinValue, shortmaxValue),
                GeolocationHidden = (short)rnd.Next(shortMinValue, shortmaxValue),
                IsNum = (short)rnd.Next(shortMinValue, shortmaxValue),
                MaxLength = rnd.Next(1, 255),
                MaxValue = Guid.NewGuid().ToString(),
                MinValue = Guid.NewGuid().ToString(),
                OriginalId = Guid.NewGuid().ToString(),
                QueryType = Guid.NewGuid().ToString(),
                ReadOnly = (short)rnd.Next(shortMinValue, shortmaxValue),
                Split = (short)rnd.Next(shortMinValue, shortmaxValue),
                UnitName = Guid.NewGuid().ToString(),
                StopOnSave = (short)rnd.Next(shortMinValue, shortmaxValue),
                KeyValuePairList = Guid.NewGuid().ToString(),
                CheckListId = checklist.Id,
                EntityGroupId = entityGroup.Id,
                FieldTypeId = fieldType.Id,
                ParentFieldId = parentFIeld.Id
            };

            //Act

            await field.Create(DbContext).ConfigureAwait(false);

            List<Field> fields = DbContext.Fields.AsNoTracking().ToList();
            List<FieldVersion> fieldVersion = DbContext.FieldVersions.AsNoTracking().ToList();

            Assert.NotNull(fields);
            Assert.NotNull(fieldVersion);

            Assert.AreEqual(2, fields.Count());
            Assert.AreEqual(2, fieldVersion.Count());

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
            Assert.AreEqual(field.Split, fields[1].Split);
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
            Assert.AreEqual(field.Split, fieldVersion[1].Split);
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

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Unit unit = new Unit
            {
                CustomerNo = rnd.Next(1, 255),
                MicrotingUid = rnd.Next(1, 255),
                OtpCode = rnd.Next(1, 255),
                SiteId = site.Id
            };
            await unit.Create(DbContext).ConfigureAwait(false);

            CheckList checklist = new CheckList
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Field1 = rnd.Next(1, 255),
                Field2 = rnd.Next(1, 255),
                Field4 = rnd.Next(1, 255),
                Field5 = rnd.Next(1, 255),
                Field6 = rnd.Next(1, 255),
                Field7 = rnd.Next(1, 255),
                Field8 = rnd.Next(1, 255),
                Field9 = rnd.Next(1, 255),
                Field10 = rnd.Next(1, 255),
                Label = Guid.NewGuid().ToString(),
                Repeated = rnd.Next(1, 255),
                ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                CaseType = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                DownloadEntities = (short)rnd.Next(shortMinValue, shortmaxValue),
                FastNavigation = (short)rnd.Next(shortMinValue, shortmaxValue),
                FolderName = Guid.NewGuid().ToString(),
                ManualSync = (short)rnd.Next(shortMinValue, shortmaxValue),
                MultiApproval = (short)rnd.Next(shortMinValue, shortmaxValue),
                OriginalId = Guid.NewGuid().ToString(),
                ReviewEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                DocxExportEnabled = randomBool,
                DoneButtonEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ExtraFieldsEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                JasperExportEnabled = randomBool,
                QuickSyncEnabled = (short)rnd.Next(shortMinValue, shortmaxValue)
            };
            await checklist.Create(DbContext).ConfigureAwait(false);

            EntityGroup entityGroup = new EntityGroup
            {
                Name = Guid.NewGuid().ToString(),
                Type = Guid.NewGuid().ToString(),
                MicrotingUid = Guid.NewGuid().ToString()
            };
            await entityGroup.Create(DbContext).ConfigureAwait(false);

            FieldType fieldType = new FieldType
            {
                Description = Guid.NewGuid().ToString(), Type = Guid.NewGuid().ToString()
            };
            await fieldType.Create(DbContext).ConfigureAwait(false);

            Field parentFIeld = new Field
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Dummy = (short)rnd.Next(shortMinValue, shortmaxValue),
                Label = Guid.NewGuid().ToString(),
                Mandatory = (short)rnd.Next(shortMinValue, shortmaxValue),
                Multi = rnd.Next(1, 255),
                Optional = (short)rnd.Next(shortMinValue, shortmaxValue),
                Selected = (short)rnd.Next(shortMinValue, shortmaxValue),
                BarcodeEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                BarcodeType = Guid.NewGuid().ToString(),
                DecimalCount = rnd.Next(1, 255),
                DefaultValue = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                GeolocationEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                GeolocationForced = (short)rnd.Next(shortMinValue, shortmaxValue),
                GeolocationHidden = (short)rnd.Next(shortMinValue, shortmaxValue),
                IsNum = (short)rnd.Next(shortMinValue, shortmaxValue),
                MaxLength = rnd.Next(1, 255),
                MaxValue = Guid.NewGuid().ToString(),
                MinValue = Guid.NewGuid().ToString(),
                OriginalId = Guid.NewGuid().ToString(),
                QueryType = Guid.NewGuid().ToString(),
                ReadOnly = (short)rnd.Next(shortMinValue, shortmaxValue),
                Split = (short)rnd.Next(shortMinValue, shortmaxValue),
                UnitName = Guid.NewGuid().ToString(),
                StopOnSave = (short)rnd.Next(shortMinValue, shortmaxValue),
                KeyValuePairList = Guid.NewGuid().ToString(),
                CheckListId = checklist.Id,
                EntityGroupId = entityGroup.Id,
                FieldTypeId = fieldType.Id
            };
            await parentFIeld.Create(DbContext).ConfigureAwait(false);

            Field field = new Field
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Dummy = (short)rnd.Next(shortMinValue, shortmaxValue),
                Label = Guid.NewGuid().ToString(),
                Mandatory = (short)rnd.Next(shortMinValue, shortmaxValue),
                Multi = rnd.Next(1, 255),
                Optional = (short)rnd.Next(shortMinValue, shortmaxValue),
                Selected = (short)rnd.Next(shortMinValue, shortmaxValue),
                BarcodeEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                BarcodeType = Guid.NewGuid().ToString(),
                DecimalCount = rnd.Next(1, 255),
                DefaultValue = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                GeolocationEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                GeolocationForced = (short)rnd.Next(shortMinValue, shortmaxValue),
                GeolocationHidden = (short)rnd.Next(shortMinValue, shortmaxValue),
                IsNum = (short)rnd.Next(shortMinValue, shortmaxValue),
                MaxLength = rnd.Next(1, 255),
                MaxValue = Guid.NewGuid().ToString(),
                MinValue = Guid.NewGuid().ToString(),
                OriginalId = Guid.NewGuid().ToString(),
                QueryType = Guid.NewGuid().ToString(),
                ReadOnly = (short)rnd.Next(shortMinValue, shortmaxValue),
                Split = (short)rnd.Next(shortMinValue, shortmaxValue),
                UnitName = Guid.NewGuid().ToString(),
                StopOnSave = (short)rnd.Next(shortMinValue, shortmaxValue),
                KeyValuePairList = Guid.NewGuid().ToString(),
                CheckListId = checklist.Id,
                EntityGroupId = entityGroup.Id,
                FieldTypeId = fieldType.Id,
                ParentFieldId = parentFIeld.Id
            };
            await field.Create(DbContext).ConfigureAwait(false);

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
            short? oldSplit = field.Split;
            string oldUnitName = field.UnitName;
            short? oldStopOnSave = field.StopOnSave;
            string oldKeyValuePairList = field.KeyValuePairList;

            field.Color = Guid.NewGuid().ToString();
            field.Custom = Guid.NewGuid().ToString();
            field.Description = Guid.NewGuid().ToString();
            field.Dummy = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.Label = Guid.NewGuid().ToString();
            field.Mandatory = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.Multi = rnd.Next(1, 255);
            field.Optional = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.Selected = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.BarcodeEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.BarcodeType = Guid.NewGuid().ToString();
            field.DecimalCount = rnd.Next(1, 255);
            field.DefaultValue = Guid.NewGuid().ToString();
            field.DisplayIndex = rnd.Next(1, 255);
            field.GeolocationEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.GeolocationForced = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.GeolocationHidden = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.IsNum = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.MaxLength = rnd.Next(1, 255);
            field.MaxValue = Guid.NewGuid().ToString();
            field.MinValue = Guid.NewGuid().ToString();
            field.OriginalId = Guid.NewGuid().ToString();
            field.QueryType = Guid.NewGuid().ToString();
            field.ReadOnly = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.Split = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.UnitName = Guid.NewGuid().ToString();
            field.StopOnSave = (short)rnd.Next(shortMinValue, shortmaxValue);
            field.KeyValuePairList = Guid.NewGuid().ToString();

            await field.Update(DbContext).ConfigureAwait(false);

            List<Field> fields = DbContext.Fields.AsNoTracking().ToList();
            List<FieldVersion> fieldVersion = DbContext.FieldVersions.AsNoTracking().ToList();

            Assert.NotNull(fields);
            Assert.NotNull(fieldVersion);

            Assert.AreEqual(2, fields.Count());
            Assert.AreEqual(3, fieldVersion.Count());

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
            Assert.AreEqual(field.Split, fields[1].Split);
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
            Assert.AreEqual(oldSplit, fieldVersion[1].Split);
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
            Assert.AreEqual(field.Split, fieldVersion[2].Split);
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

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Unit unit = new Unit
            {
                CustomerNo = rnd.Next(1, 255),
                MicrotingUid = rnd.Next(1, 255),
                OtpCode = rnd.Next(1, 255),
                SiteId = site.Id
            };
            await unit.Create(DbContext).ConfigureAwait(false);

            CheckList checklist = new CheckList
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Field1 = rnd.Next(1, 255),
                Field2 = rnd.Next(1, 255),
                Field4 = rnd.Next(1, 255),
                Field5 = rnd.Next(1, 255),
                Field6 = rnd.Next(1, 255),
                Field7 = rnd.Next(1, 255),
                Field8 = rnd.Next(1, 255),
                Field9 = rnd.Next(1, 255),
                Field10 = rnd.Next(1, 255),
                Label = Guid.NewGuid().ToString(),
                Repeated = rnd.Next(1, 255),
                ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                CaseType = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                DownloadEntities = (short)rnd.Next(shortMinValue, shortmaxValue),
                FastNavigation = (short)rnd.Next(shortMinValue, shortmaxValue),
                FolderName = Guid.NewGuid().ToString(),
                ManualSync = (short)rnd.Next(shortMinValue, shortmaxValue),
                MultiApproval = (short)rnd.Next(shortMinValue, shortmaxValue),
                OriginalId = Guid.NewGuid().ToString(),
                ReviewEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                DocxExportEnabled = randomBool,
                DoneButtonEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                ExtraFieldsEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                JasperExportEnabled = randomBool,
                QuickSyncEnabled = (short)rnd.Next(shortMinValue, shortmaxValue)
            };
            await checklist.Create(DbContext).ConfigureAwait(false);

            EntityGroup entityGroup = new EntityGroup
            {
                Name = Guid.NewGuid().ToString(),
                Type = Guid.NewGuid().ToString(),
                MicrotingUid = Guid.NewGuid().ToString()
            };
            await entityGroup.Create(DbContext).ConfigureAwait(false);

            FieldType fieldType = new FieldType
            {
                Description = Guid.NewGuid().ToString(), Type = Guid.NewGuid().ToString()
            };
            await fieldType.Create(DbContext).ConfigureAwait(false);

            Field parentFIeld = new Field
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Dummy = (short)rnd.Next(shortMinValue, shortmaxValue),
                Label = Guid.NewGuid().ToString(),
                Mandatory = (short)rnd.Next(shortMinValue, shortmaxValue),
                Multi = rnd.Next(1, 255),
                Optional = (short)rnd.Next(shortMinValue, shortmaxValue),
                Selected = (short)rnd.Next(shortMinValue, shortmaxValue),
                BarcodeEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                BarcodeType = Guid.NewGuid().ToString(),
                DecimalCount = rnd.Next(1, 255),
                DefaultValue = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                GeolocationEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                GeolocationForced = (short)rnd.Next(shortMinValue, shortmaxValue),
                GeolocationHidden = (short)rnd.Next(shortMinValue, shortmaxValue),
                IsNum = (short)rnd.Next(shortMinValue, shortmaxValue),
                MaxLength = rnd.Next(1, 255),
                MaxValue = Guid.NewGuid().ToString(),
                MinValue = Guid.NewGuid().ToString(),
                OriginalId = Guid.NewGuid().ToString(),
                QueryType = Guid.NewGuid().ToString(),
                ReadOnly = (short)rnd.Next(shortMinValue, shortmaxValue),
                Split = (short)rnd.Next(shortMinValue, shortmaxValue),
                UnitName = Guid.NewGuid().ToString(),
                StopOnSave = (short)rnd.Next(shortMinValue, shortmaxValue),
                KeyValuePairList = Guid.NewGuid().ToString(),
                CheckListId = checklist.Id,
                EntityGroupId = entityGroup.Id,
                FieldTypeId = fieldType.Id
            };
            await parentFIeld.Create(DbContext).ConfigureAwait(false);

            Field field = new Field
            {
                Color = Guid.NewGuid().ToString(),
                Custom = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Dummy = (short)rnd.Next(shortMinValue, shortmaxValue),
                Label = Guid.NewGuid().ToString(),
                Mandatory = (short)rnd.Next(shortMinValue, shortmaxValue),
                Multi = rnd.Next(1, 255),
                Optional = (short)rnd.Next(shortMinValue, shortmaxValue),
                Selected = (short)rnd.Next(shortMinValue, shortmaxValue),
                BarcodeEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                BarcodeType = Guid.NewGuid().ToString(),
                DecimalCount = rnd.Next(1, 255),
                DefaultValue = Guid.NewGuid().ToString(),
                DisplayIndex = rnd.Next(1, 255),
                GeolocationEnabled = (short)rnd.Next(shortMinValue, shortmaxValue),
                GeolocationForced = (short)rnd.Next(shortMinValue, shortmaxValue),
                GeolocationHidden = (short)rnd.Next(shortMinValue, shortmaxValue),
                IsNum = (short)rnd.Next(shortMinValue, shortmaxValue),
                MaxLength = rnd.Next(1, 255),
                MaxValue = Guid.NewGuid().ToString(),
                MinValue = Guid.NewGuid().ToString(),
                OriginalId = Guid.NewGuid().ToString(),
                QueryType = Guid.NewGuid().ToString(),
                ReadOnly = (short)rnd.Next(shortMinValue, shortmaxValue),
                Split = (short)rnd.Next(shortMinValue, shortmaxValue),
                UnitName = Guid.NewGuid().ToString(),
                StopOnSave = (short)rnd.Next(shortMinValue, shortmaxValue),
                KeyValuePairList = Guid.NewGuid().ToString(),
                CheckListId = checklist.Id,
                EntityGroupId = entityGroup.Id,
                FieldTypeId = fieldType.Id,
                ParentFieldId = parentFIeld.Id
            };
            await field.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = field.UpdatedAt;

            await field.Delete(DbContext);

            List<Field> fields = DbContext.Fields.AsNoTracking().ToList();
            List<FieldVersion> fieldVersion = DbContext.FieldVersions.AsNoTracking().ToList();

            Assert.NotNull(fields);
            Assert.NotNull(fieldVersion);

            Assert.AreEqual(2, fields.Count());
            Assert.AreEqual(3, fieldVersion.Count());

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
            Assert.AreEqual(field.Split, fields[1].Split);
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
            Assert.AreEqual(field.Split, fieldVersion[1].Split);
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
            Assert.AreEqual(field.Split, fieldVersion[2].Split);
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