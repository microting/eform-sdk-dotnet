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

namespace eFormSDK.CheckLists.Tests;

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

        Assert.That(fields, Is.Not.EqualTo(null));
        Assert.That(fieldVersion, Is.Not.EqualTo(null));

        Assert.That(fields.Count(), Is.EqualTo(2));
        Assert.That(fieldVersion.Count(), Is.EqualTo(2));

        Assert.That(fields[1].CreatedAt.ToString(), Is.EqualTo(field.CreatedAt.ToString()));
        Assert.That(fields[1].Version, Is.EqualTo(field.Version));
        //            Assert.AreEqual(field.UpdatedAt.ToString(), fields[1].UpdatedAt.ToString());
        Assert.That(fields[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(fields[1].Id, Is.EqualTo(field.Id));
        Assert.That(fields[1].Color, Is.EqualTo(field.Color));
        Assert.That(fields[1].Custom, Is.EqualTo(field.Custom));
        Assert.That(fields[1].Description, Is.EqualTo(field.Description));
        Assert.That(fields[1].Dummy, Is.EqualTo(field.Dummy));
        Assert.That(fields[1].Label, Is.EqualTo(field.Label));
        Assert.That(fields[1].Mandatory, Is.EqualTo(field.Mandatory));
        Assert.That(fields[1].Multi, Is.EqualTo(field.Multi));
        Assert.That(fields[1].Optional, Is.EqualTo(field.Optional));
        Assert.That(fields[1].Selected, Is.EqualTo(field.Selected));
        Assert.That(fields[1].BarcodeEnabled, Is.EqualTo(field.BarcodeEnabled));
        Assert.That(fields[1].BarcodeType, Is.EqualTo(field.BarcodeType));
        Assert.That(fields[1].DecimalCount, Is.EqualTo(field.DecimalCount));
        Assert.That(fields[1].DefaultValue, Is.EqualTo(field.DefaultValue));
        Assert.That(fields[1].DisplayIndex, Is.EqualTo(field.DisplayIndex));
        Assert.That(fields[1].GeolocationEnabled, Is.EqualTo(field.GeolocationEnabled));
        Assert.That(fields[1].GeolocationForced, Is.EqualTo(field.GeolocationForced));
        Assert.That(fields[1].GeolocationHidden, Is.EqualTo(field.GeolocationHidden));
        Assert.That(fields[1].IsNum, Is.EqualTo(field.IsNum));
        Assert.That(fields[1].MaxLength, Is.EqualTo(field.MaxLength));
        Assert.That(fields[1].MaxValue, Is.EqualTo(field.MaxValue));
        Assert.That(fields[1].MinValue, Is.EqualTo(field.MinValue));
        Assert.That(fields[1].OriginalId, Is.EqualTo(field.OriginalId));
        Assert.That(fields[1].ReadOnly, Is.EqualTo(field.ReadOnly));
        Assert.That(fields[1].Split, Is.EqualTo(field.Split));
        Assert.That(fields[1].QueryType, Is.EqualTo(field.QueryType));
        Assert.That(fields[1].UnitName, Is.EqualTo(field.UnitName));
        Assert.That(entityGroup.Id, Is.EqualTo(field.EntityGroupId));
        Assert.That(fieldType.Id, Is.EqualTo(field.FieldTypeId));
        Assert.That(fields[1].ParentFieldId, Is.EqualTo(field.ParentFieldId));
        Assert.That(checklist.Id, Is.EqualTo(field.CheckListId));
        Assert.That(fields[1].StopOnSave, Is.EqualTo(field.StopOnSave));
        Assert.That(fields[1].KeyValuePairList, Is.EqualTo(field.KeyValuePairList));


        //Versions
        Assert.That(fieldVersion[1].CreatedAt.ToString(), Is.EqualTo(field.CreatedAt.ToString()));
        Assert.That(fieldVersion[1].Version, Is.EqualTo(1));
        //            Assert.AreEqual(field.UpdatedAt.ToString(), fieldVersion[1].UpdatedAt.ToString());
        Assert.That(fieldVersion[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(fieldVersion[1].FieldId, Is.EqualTo(field.Id));
        Assert.That(fieldVersion[1].Color, Is.EqualTo(field.Color));
        Assert.That(fieldVersion[1].Custom, Is.EqualTo(field.Custom));
        Assert.That(fieldVersion[1].Description, Is.EqualTo(field.Description));
        Assert.That(fieldVersion[1].Dummy, Is.EqualTo(field.Dummy));
        Assert.That(fieldVersion[1].Label, Is.EqualTo(field.Label));
        Assert.That(fieldVersion[1].Mandatory, Is.EqualTo(field.Mandatory));
        Assert.That(fieldVersion[1].Multi, Is.EqualTo(field.Multi));
        Assert.That(fieldVersion[1].Optional, Is.EqualTo(field.Optional));
        Assert.That(fieldVersion[1].Selected, Is.EqualTo(field.Selected));
        Assert.That(fieldVersion[1].BarcodeEnabled, Is.EqualTo(field.BarcodeEnabled));
        Assert.That(fieldVersion[1].BarcodeType, Is.EqualTo(field.BarcodeType));
        Assert.That(fieldVersion[1].DecimalCount, Is.EqualTo(field.DecimalCount));
        Assert.That(fieldVersion[1].DefaultValue, Is.EqualTo(field.DefaultValue));
        Assert.That(fieldVersion[1].DisplayIndex, Is.EqualTo(field.DisplayIndex));
        Assert.That(fieldVersion[1].GeolocationEnabled, Is.EqualTo(field.GeolocationEnabled));
        Assert.That(fieldVersion[1].GeolocationForced, Is.EqualTo(field.GeolocationForced));
        Assert.That(fieldVersion[1].GeolocationHidden, Is.EqualTo(field.GeolocationHidden));
        Assert.That(fieldVersion[1].IsNum, Is.EqualTo(field.IsNum));
        Assert.That(fieldVersion[1].MaxLength, Is.EqualTo(field.MaxLength));
        Assert.That(fieldVersion[1].MaxValue, Is.EqualTo(field.MaxValue));
        Assert.That(fieldVersion[1].MinValue, Is.EqualTo(field.MinValue));
        Assert.That(fieldVersion[1].OriginalId, Is.EqualTo(field.OriginalId));
        Assert.That(fieldVersion[1].ReadOnly, Is.EqualTo(field.ReadOnly));
        Assert.That(fieldVersion[1].Split, Is.EqualTo(field.Split));
        Assert.That(fieldVersion[1].QueryType, Is.EqualTo(field.QueryType));
        Assert.That(fieldVersion[1].UnitName, Is.EqualTo(field.UnitName));
        Assert.That(fieldVersion[1].EntityGroupId, Is.EqualTo(entityGroup.Id));
        Assert.That(fieldVersion[1].FieldTypeId, Is.EqualTo(fieldType.Id));
        Assert.That(fieldVersion[1].ParentFieldId, Is.EqualTo(field.ParentFieldId));
        Assert.That(fieldVersion[1].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(fieldVersion[1].StopOnSave, Is.EqualTo(field.StopOnSave));
        Assert.That(fieldVersion[1].KeyValuePairList, Is.EqualTo(field.KeyValuePairList));
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

        Assert.That(fields, Is.Not.EqualTo(null));
        Assert.That(fieldVersion, Is.Not.EqualTo(null));

        Assert.That(fields.Count(), Is.EqualTo(2));
        Assert.That(fieldVersion.Count(), Is.EqualTo(3));

        Assert.That(fields[1].CreatedAt.ToString(), Is.EqualTo(field.CreatedAt.ToString()));
        Assert.That(fields[1].Version, Is.EqualTo(field.Version));
        //            Assert.AreEqual(field.UpdatedAt.ToString(), fields[1].UpdatedAt.ToString());
        Assert.That(fields[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(fields[1].Id, Is.EqualTo(field.Id));
        Assert.That(fields[1].Color, Is.EqualTo(field.Color));
        Assert.That(fields[1].Custom, Is.EqualTo(field.Custom));
        Assert.That(fields[1].Description, Is.EqualTo(field.Description));
        Assert.That(fields[1].Dummy, Is.EqualTo(field.Dummy));
        Assert.That(fields[1].Label, Is.EqualTo(field.Label));
        Assert.That(fields[1].Mandatory, Is.EqualTo(field.Mandatory));
        Assert.That(fields[1].Multi, Is.EqualTo(field.Multi));
        Assert.That(fields[1].Optional, Is.EqualTo(field.Optional));
        Assert.That(fields[1].Selected, Is.EqualTo(field.Selected));
        Assert.That(fields[1].BarcodeEnabled, Is.EqualTo(field.BarcodeEnabled));
        Assert.That(fields[1].BarcodeType, Is.EqualTo(field.BarcodeType));
        Assert.That(fields[1].DecimalCount, Is.EqualTo(field.DecimalCount));
        Assert.That(fields[1].DefaultValue, Is.EqualTo(field.DefaultValue));
        Assert.That(fields[1].DisplayIndex, Is.EqualTo(field.DisplayIndex));
        Assert.That(fields[1].GeolocationEnabled, Is.EqualTo(field.GeolocationEnabled));
        Assert.That(fields[1].GeolocationForced, Is.EqualTo(field.GeolocationForced));
        Assert.That(fields[1].GeolocationHidden, Is.EqualTo(field.GeolocationHidden));
        Assert.That(fields[1].IsNum, Is.EqualTo(field.IsNum));
        Assert.That(fields[1].MaxLength, Is.EqualTo(field.MaxLength));
        Assert.That(fields[1].MaxValue, Is.EqualTo(field.MaxValue));
        Assert.That(fields[1].MinValue, Is.EqualTo(field.MinValue));
        Assert.That(fields[1].OriginalId, Is.EqualTo(field.OriginalId));
        Assert.That(fields[1].ReadOnly, Is.EqualTo(field.ReadOnly));
        Assert.That(fields[1].Split, Is.EqualTo(field.Split));
        Assert.That(fields[1].QueryType, Is.EqualTo(field.QueryType));
        Assert.That(fields[1].UnitName, Is.EqualTo(field.UnitName));
        Assert.That(entityGroup.Id, Is.EqualTo(field.EntityGroupId));
        Assert.That(fieldType.Id, Is.EqualTo(field.FieldTypeId));
        Assert.That(fields[1].ParentFieldId, Is.EqualTo(field.ParentFieldId));
        Assert.That(checklist.Id, Is.EqualTo(field.CheckListId));
        Assert.That(fields[1].StopOnSave, Is.EqualTo(field.StopOnSave));
        Assert.That(fields[1].KeyValuePairList, Is.EqualTo(field.KeyValuePairList));


        //Old Version
        Assert.That(fieldVersion[1].CreatedAt.ToString(), Is.EqualTo(field.CreatedAt.ToString()));
        Assert.That(fieldVersion[1].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), fieldVersion[1].UpdatedAt.ToString());
        Assert.That(fieldVersion[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(fieldVersion[1].FieldId, Is.EqualTo(field.Id));
        Assert.That(fieldVersion[1].Color, Is.EqualTo(oldColor));
        Assert.That(fieldVersion[1].Custom, Is.EqualTo(oldCustom));
        Assert.That(fieldVersion[1].Description, Is.EqualTo(oldDescription));
        Assert.That(fieldVersion[1].Dummy, Is.EqualTo(oldDummy));
        Assert.That(fieldVersion[1].Label, Is.EqualTo(oldLabel));
        Assert.That(fieldVersion[1].Mandatory, Is.EqualTo(oldMandatory));
        Assert.That(fieldVersion[1].Multi, Is.EqualTo(oldMulti));
        Assert.That(fieldVersion[1].Optional, Is.EqualTo(oldOptional));
        Assert.That(fieldVersion[1].Selected, Is.EqualTo(oldSelected));
        Assert.That(fieldVersion[1].BarcodeEnabled, Is.EqualTo(oldBarcodeEnabled));
        Assert.That(fieldVersion[1].BarcodeType, Is.EqualTo(oldBarcodeType));
        Assert.That(fieldVersion[1].DecimalCount, Is.EqualTo(oldDecimalCount));
        Assert.That(fieldVersion[1].DefaultValue, Is.EqualTo(oldDefaultValue));
        Assert.That(fieldVersion[1].DisplayIndex, Is.EqualTo(oldDisplayIndex));
        Assert.That(fieldVersion[1].GeolocationEnabled, Is.EqualTo(oldGeolocationEnabled));
        Assert.That(fieldVersion[1].GeolocationForced, Is.EqualTo(oldGeolocationForced));
        Assert.That(fieldVersion[1].GeolocationHidden, Is.EqualTo(oldGeolocationHidden));
        Assert.That(fieldVersion[1].IsNum, Is.EqualTo(oldIsNum));
        Assert.That(fieldVersion[1].MaxLength, Is.EqualTo(oldMaxLength));
        Assert.That(fieldVersion[1].MaxValue, Is.EqualTo(oldMaxValue));
        Assert.That(fieldVersion[1].MinValue, Is.EqualTo(oldMinValue));
        Assert.That(fieldVersion[1].OriginalId, Is.EqualTo(oldOriginalId));
        Assert.That(fieldVersion[1].ReadOnly, Is.EqualTo(oldReadonly));
        Assert.That(fieldVersion[1].Split, Is.EqualTo(oldSplit));
        Assert.That(fieldVersion[1].QueryType, Is.EqualTo(oldQueryType));
        Assert.That(fieldVersion[1].UnitName, Is.EqualTo(oldUnitName));
        Assert.That(fieldVersion[1].EntityGroupId, Is.EqualTo(entityGroup.Id));
        Assert.That(fieldVersion[1].FieldTypeId, Is.EqualTo(fieldType.Id));
        Assert.That(fieldVersion[1].ParentFieldId, Is.EqualTo(field.ParentFieldId));
        Assert.That(fieldVersion[1].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(fieldVersion[1].StopOnSave, Is.EqualTo(oldStopOnSave));
        Assert.That(fieldVersion[1].KeyValuePairList, Is.EqualTo(oldKeyValuePairList));

        //New Version
        Assert.That(fieldVersion[2].CreatedAt.ToString(), Is.EqualTo(field.CreatedAt.ToString()));
        Assert.That(fieldVersion[2].Version, Is.EqualTo(2));
        //            Assert.AreEqual(field.UpdatedAt.ToString(), fieldVersion[2].UpdatedAt.ToString());
        Assert.That(fieldVersion[2].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(fieldVersion[2].FieldId, Is.EqualTo(field.Id));
        Assert.That(fieldVersion[2].Color, Is.EqualTo(field.Color));
        Assert.That(fieldVersion[2].Custom, Is.EqualTo(field.Custom));
        Assert.That(fieldVersion[2].Description, Is.EqualTo(field.Description));
        Assert.That(fieldVersion[2].Dummy, Is.EqualTo(field.Dummy));
        Assert.That(fieldVersion[2].Label, Is.EqualTo(field.Label));
        Assert.That(fieldVersion[2].Mandatory, Is.EqualTo(field.Mandatory));
        Assert.That(fieldVersion[2].Multi, Is.EqualTo(field.Multi));
        Assert.That(fieldVersion[2].Optional, Is.EqualTo(field.Optional));
        Assert.That(fieldVersion[2].Selected, Is.EqualTo(field.Selected));
        Assert.That(fieldVersion[2].BarcodeEnabled, Is.EqualTo(field.BarcodeEnabled));
        Assert.That(fieldVersion[2].BarcodeType, Is.EqualTo(field.BarcodeType));
        Assert.That(fieldVersion[2].DecimalCount, Is.EqualTo(field.DecimalCount));
        Assert.That(fieldVersion[2].DefaultValue, Is.EqualTo(field.DefaultValue));
        Assert.That(fieldVersion[2].DisplayIndex, Is.EqualTo(field.DisplayIndex));
        Assert.That(fieldVersion[2].GeolocationEnabled, Is.EqualTo(field.GeolocationEnabled));
        Assert.That(fieldVersion[2].GeolocationForced, Is.EqualTo(field.GeolocationForced));
        Assert.That(fieldVersion[2].GeolocationHidden, Is.EqualTo(field.GeolocationHidden));
        Assert.That(fieldVersion[2].IsNum, Is.EqualTo(field.IsNum));
        Assert.That(fieldVersion[2].MaxLength, Is.EqualTo(field.MaxLength));
        Assert.That(fieldVersion[2].MaxValue, Is.EqualTo(field.MaxValue));
        Assert.That(fieldVersion[2].MinValue, Is.EqualTo(field.MinValue));
        Assert.That(fieldVersion[2].OriginalId, Is.EqualTo(field.OriginalId));
        Assert.That(fieldVersion[2].ReadOnly, Is.EqualTo(field.ReadOnly));
        Assert.That(fieldVersion[2].Split, Is.EqualTo(field.Split));
        Assert.That(fieldVersion[2].QueryType, Is.EqualTo(field.QueryType));
        Assert.That(fieldVersion[2].UnitName, Is.EqualTo(field.UnitName));
        Assert.That(fieldVersion[2].EntityGroupId, Is.EqualTo(entityGroup.Id));
        Assert.That(fieldVersion[2].FieldTypeId, Is.EqualTo(fieldType.Id));
        Assert.That(fieldVersion[2].ParentFieldId, Is.EqualTo(field.ParentFieldId));
        Assert.That(fieldVersion[2].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(fieldVersion[2].StopOnSave, Is.EqualTo(field.StopOnSave));
        Assert.That(fieldVersion[2].KeyValuePairList, Is.EqualTo(field.KeyValuePairList));
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

        Assert.That(fields, Is.Not.EqualTo(null));
        Assert.That(fieldVersion, Is.Not.EqualTo(null));

        Assert.That(fields.Count(), Is.EqualTo(2));
        Assert.That(fieldVersion.Count(), Is.EqualTo(3));

        Assert.That(fields[1].CreatedAt.ToString(), Is.EqualTo(field.CreatedAt.ToString()));
        Assert.That(fields[1].Version, Is.EqualTo(field.Version));
        //            Assert.AreEqual(field.UpdatedAt.ToString(), fields[1].UpdatedAt.ToString());
        Assert.That(fields[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(fields[1].Id, Is.EqualTo(field.Id));
        Assert.That(fields[1].Color, Is.EqualTo(field.Color));
        Assert.That(fields[1].Custom, Is.EqualTo(field.Custom));
        Assert.That(fields[1].Description, Is.EqualTo(field.Description));
        Assert.That(fields[1].Dummy, Is.EqualTo(field.Dummy));
        Assert.That(fields[1].Label, Is.EqualTo(field.Label));
        Assert.That(fields[1].Mandatory, Is.EqualTo(field.Mandatory));
        Assert.That(fields[1].Multi, Is.EqualTo(field.Multi));
        Assert.That(fields[1].Optional, Is.EqualTo(field.Optional));
        Assert.That(fields[1].Selected, Is.EqualTo(field.Selected));
        Assert.That(fields[1].BarcodeEnabled, Is.EqualTo(field.BarcodeEnabled));
        Assert.That(fields[1].BarcodeType, Is.EqualTo(field.BarcodeType));
        Assert.That(fields[1].DecimalCount, Is.EqualTo(field.DecimalCount));
        Assert.That(fields[1].DefaultValue, Is.EqualTo(field.DefaultValue));
        Assert.That(fields[1].DisplayIndex, Is.EqualTo(field.DisplayIndex));
        Assert.That(fields[1].GeolocationEnabled, Is.EqualTo(field.GeolocationEnabled));
        Assert.That(fields[1].GeolocationForced, Is.EqualTo(field.GeolocationForced));
        Assert.That(fields[1].GeolocationHidden, Is.EqualTo(field.GeolocationHidden));
        Assert.That(fields[1].IsNum, Is.EqualTo(field.IsNum));
        Assert.That(fields[1].MaxLength, Is.EqualTo(field.MaxLength));
        Assert.That(fields[1].MaxValue, Is.EqualTo(field.MaxValue));
        Assert.That(fields[1].MinValue, Is.EqualTo(field.MinValue));
        Assert.That(fields[1].OriginalId, Is.EqualTo(field.OriginalId));
        Assert.That(fields[1].ReadOnly, Is.EqualTo(field.ReadOnly));
        Assert.That(fields[1].Split, Is.EqualTo(field.Split));
        Assert.That(fields[1].QueryType, Is.EqualTo(field.QueryType));
        Assert.That(fields[1].UnitName, Is.EqualTo(field.UnitName));
        Assert.That(entityGroup.Id, Is.EqualTo(field.EntityGroupId));
        Assert.That(fieldType.Id, Is.EqualTo(field.FieldTypeId));
        Assert.That(fields[1].ParentFieldId, Is.EqualTo(field.ParentFieldId));
        Assert.That(checklist.Id, Is.EqualTo(field.CheckListId));
        Assert.That(fields[1].StopOnSave, Is.EqualTo(field.StopOnSave));
        Assert.That(fields[1].KeyValuePairList, Is.EqualTo(field.KeyValuePairList));


        //Old Version
        Assert.That(fieldVersion[1].CreatedAt.ToString(), Is.EqualTo(field.CreatedAt.ToString()));
        Assert.That(fieldVersion[1].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), fieldVersion[1].UpdatedAt.ToString());
        Assert.That(fieldVersion[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(fieldVersion[1].FieldId, Is.EqualTo(field.Id));
        Assert.That(fieldVersion[1].Color, Is.EqualTo(field.Color));
        Assert.That(fieldVersion[1].Custom, Is.EqualTo(field.Custom));
        Assert.That(fieldVersion[1].Description, Is.EqualTo(field.Description));
        Assert.That(fieldVersion[1].Dummy, Is.EqualTo(field.Dummy));
        Assert.That(fieldVersion[1].Label, Is.EqualTo(field.Label));
        Assert.That(fieldVersion[1].Mandatory, Is.EqualTo(field.Mandatory));
        Assert.That(fieldVersion[1].Multi, Is.EqualTo(field.Multi));
        Assert.That(fieldVersion[1].Optional, Is.EqualTo(field.Optional));
        Assert.That(fieldVersion[1].Selected, Is.EqualTo(field.Selected));
        Assert.That(fieldVersion[1].BarcodeEnabled, Is.EqualTo(field.BarcodeEnabled));
        Assert.That(fieldVersion[1].BarcodeType, Is.EqualTo(field.BarcodeType));
        Assert.That(fieldVersion[1].DecimalCount, Is.EqualTo(field.DecimalCount));
        Assert.That(fieldVersion[1].DefaultValue, Is.EqualTo(field.DefaultValue));
        Assert.That(fieldVersion[1].DisplayIndex, Is.EqualTo(field.DisplayIndex));
        Assert.That(fieldVersion[1].GeolocationEnabled, Is.EqualTo(field.GeolocationEnabled));
        Assert.That(fieldVersion[1].GeolocationForced, Is.EqualTo(field.GeolocationForced));
        Assert.That(fieldVersion[1].GeolocationHidden, Is.EqualTo(field.GeolocationHidden));
        Assert.That(fieldVersion[1].IsNum, Is.EqualTo(field.IsNum));
        Assert.That(fieldVersion[1].MaxLength, Is.EqualTo(field.MaxLength));
        Assert.That(fieldVersion[1].MaxValue, Is.EqualTo(field.MaxValue));
        Assert.That(fieldVersion[1].MinValue, Is.EqualTo(field.MinValue));
        Assert.That(fieldVersion[1].OriginalId, Is.EqualTo(field.OriginalId));
        Assert.That(fieldVersion[1].ReadOnly, Is.EqualTo(field.ReadOnly));
        Assert.That(fieldVersion[1].Split, Is.EqualTo(field.Split));
        Assert.That(fieldVersion[1].QueryType, Is.EqualTo(field.QueryType));
        Assert.That(fieldVersion[1].UnitName, Is.EqualTo(field.UnitName));
        Assert.That(fieldVersion[1].EntityGroupId, Is.EqualTo(entityGroup.Id));
        Assert.That(fieldVersion[1].FieldTypeId, Is.EqualTo(fieldType.Id));
        Assert.That(fieldVersion[1].ParentFieldId, Is.EqualTo(field.ParentFieldId));
        Assert.That(fieldVersion[1].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(fieldVersion[1].StopOnSave, Is.EqualTo(field.StopOnSave));
        Assert.That(fieldVersion[1].KeyValuePairList, Is.EqualTo(field.KeyValuePairList));

        //New Version
        Assert.That(fieldVersion[2].CreatedAt.ToString(), Is.EqualTo(field.CreatedAt.ToString()));
        Assert.That(fieldVersion[2].Version, Is.EqualTo(2));
        //            Assert.AreEqual(field.UpdatedAt.ToString(), fieldVersion[2].UpdatedAt.ToString());
        Assert.That(fieldVersion[2].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(fieldVersion[2].FieldId, Is.EqualTo(field.Id));
        Assert.That(fieldVersion[2].Color, Is.EqualTo(field.Color));
        Assert.That(fieldVersion[2].Custom, Is.EqualTo(field.Custom));
        Assert.That(fieldVersion[2].Description, Is.EqualTo(field.Description));
        Assert.That(fieldVersion[2].Dummy, Is.EqualTo(field.Dummy));
        Assert.That(fieldVersion[2].Label, Is.EqualTo(field.Label));
        Assert.That(fieldVersion[2].Mandatory, Is.EqualTo(field.Mandatory));
        Assert.That(fieldVersion[2].Multi, Is.EqualTo(field.Multi));
        Assert.That(fieldVersion[2].Optional, Is.EqualTo(field.Optional));
        Assert.That(fieldVersion[2].Selected, Is.EqualTo(field.Selected));
        Assert.That(fieldVersion[2].BarcodeEnabled, Is.EqualTo(field.BarcodeEnabled));
        Assert.That(fieldVersion[2].BarcodeType, Is.EqualTo(field.BarcodeType));
        Assert.That(fieldVersion[2].DecimalCount, Is.EqualTo(field.DecimalCount));
        Assert.That(fieldVersion[2].DefaultValue, Is.EqualTo(field.DefaultValue));
        Assert.That(fieldVersion[2].DisplayIndex, Is.EqualTo(field.DisplayIndex));
        Assert.That(fieldVersion[2].GeolocationEnabled, Is.EqualTo(field.GeolocationEnabled));
        Assert.That(fieldVersion[2].GeolocationForced, Is.EqualTo(field.GeolocationForced));
        Assert.That(fieldVersion[2].GeolocationHidden, Is.EqualTo(field.GeolocationHidden));
        Assert.That(fieldVersion[2].IsNum, Is.EqualTo(field.IsNum));
        Assert.That(fieldVersion[2].MaxLength, Is.EqualTo(field.MaxLength));
        Assert.That(fieldVersion[2].MaxValue, Is.EqualTo(field.MaxValue));
        Assert.That(fieldVersion[2].MinValue, Is.EqualTo(field.MinValue));
        Assert.That(fieldVersion[2].OriginalId, Is.EqualTo(field.OriginalId));
        Assert.That(fieldVersion[2].ReadOnly, Is.EqualTo(field.ReadOnly));
        Assert.That(fieldVersion[2].Split, Is.EqualTo(field.Split));
        Assert.That(fieldVersion[2].QueryType, Is.EqualTo(field.QueryType));
        Assert.That(fieldVersion[2].UnitName, Is.EqualTo(field.UnitName));
        Assert.That(fieldVersion[2].EntityGroupId, Is.EqualTo(entityGroup.Id));
        Assert.That(fieldVersion[2].FieldTypeId, Is.EqualTo(fieldType.Id));
        Assert.That(fieldVersion[2].ParentFieldId, Is.EqualTo(field.ParentFieldId));
        Assert.That(fieldVersion[2].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(fieldVersion[2].StopOnSave, Is.EqualTo(field.StopOnSave));
        Assert.That(fieldVersion[2].KeyValuePairList, Is.EqualTo(field.KeyValuePairList));
    }
}