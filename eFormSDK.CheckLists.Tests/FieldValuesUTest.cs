/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

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
public class FieldValuesUTest : DbTestFixture
{
    [Test]
    public async Task FieldValues_Create_DoesCreate()
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
            FieldTypeId = fieldType.Id
        };
        await field.Create(DbContext).ConfigureAwait(false);


        Worker worker = new Worker
        {
            Email = Guid.NewGuid().ToString(),
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            MicrotingUid = rnd.Next(1, 255),
            PinCode = "1234",
            EmployeeNo = ""
        };
        await worker.Create(DbContext).ConfigureAwait(false);

        Case theCase = new Case
        {
            Custom = Guid.NewGuid().ToString(),
            Status = rnd.Next(1, 255),
            Type = Guid.NewGuid().ToString(),
            CaseUid = Guid.NewGuid().ToString(),
            DoneAt = DateTime.UtcNow,
            FieldValue1 = Guid.NewGuid().ToString(),
            FieldValue2 = Guid.NewGuid().ToString(),
            FieldValue3 = Guid.NewGuid().ToString(),
            FieldValue4 = Guid.NewGuid().ToString(),
            FieldValue5 = Guid.NewGuid().ToString(),
            FieldValue6 = Guid.NewGuid().ToString(),
            FieldValue7 = Guid.NewGuid().ToString(),
            FieldValue8 = Guid.NewGuid().ToString(),
            FieldValue9 = Guid.NewGuid().ToString(),
            FieldValue10 = Guid.NewGuid().ToString(),
            MicrotingUid = rnd.Next(shortMinValue, shortmaxValue),
            SiteId = site.Id,
            UnitId = unit.Id,
            WorkerId = worker.Id,
            CheckListId = checklist.Id,
            MicrotingCheckUid = rnd.Next(shortMinValue, shortmaxValue)
        };
        await theCase.Create(DbContext).ConfigureAwait(false);

        UploadedData uploadedData = new UploadedData
        {
            Checksum = Guid.NewGuid().ToString(),
            Extension = Guid.NewGuid().ToString(),
            Local = (short)rnd.Next(shortMinValue, shortmaxValue),
            CurrentFile = Guid.NewGuid().ToString(),
            ExpirationDate = DateTime.UtcNow,
            FileLocation = Guid.NewGuid().ToString(),
            FileName = Guid.NewGuid().ToString(),
            TranscriptionId = rnd.Next(1, 255),
            UploaderId = rnd.Next(1, 255),
            UploaderType = Guid.NewGuid().ToString()
        };
        await uploadedData.Create(DbContext).ConfigureAwait(false);

        FieldValue fieldValue = new FieldValue
        {
            Accuracy = Guid.NewGuid().ToString(),
            Altitude = Guid.NewGuid().ToString(),
            Date = DateTime.UtcNow,
            Heading = Guid.NewGuid().ToString(),
            Latitude = Guid.NewGuid().ToString(),
            Longitude = Guid.NewGuid().ToString(),
            Value = Guid.NewGuid().ToString(),
            CaseId = theCase.Id,
            DoneAt = DateTime.UtcNow,
            FieldId = field.Id,
            WorkerId = worker.Id,
            CheckListId = checklist.Id,
            UploadedDataId = uploadedData.Id
        };

        //Act

        await fieldValue.Create(DbContext).ConfigureAwait(false);

        List<FieldValue> fieldValues = DbContext.FieldValues.AsNoTracking().ToList();
        List<FieldValueVersion> fieldValueVersions = DbContext.FieldValueVersions.AsNoTracking().ToList();

        Assert.That(fieldValues, Is.Not.EqualTo(null));
        Assert.That(fieldValueVersions, Is.Not.EqualTo(null));

        Assert.That(fieldValues.Count(), Is.EqualTo(1));
        Assert.That(fieldValueVersions.Count(), Is.EqualTo(1));

        Assert.That(fieldValues[0].CreatedAt.ToString(), Is.EqualTo(fieldValue.CreatedAt.ToString()));
        Assert.That(fieldValues[0].Version, Is.EqualTo(fieldValue.Version));
        //            Assert.AreEqual(fieldValue.UpdatedAt.ToString(), fieldValues[0].UpdatedAt.ToString());
        Assert.That(fieldValues[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(fieldValues[0].Id, Is.EqualTo(fieldValue.Id));
        Assert.That(fieldValues[0].Accuracy, Is.EqualTo(fieldValue.Accuracy));
        Assert.That(fieldValues[0].Date.ToString(), Is.EqualTo(fieldValue.Date.ToString()));
        Assert.That(fieldValues[0].Heading, Is.EqualTo(fieldValue.Heading));
        Assert.That(fieldValues[0].Latitude, Is.EqualTo(fieldValue.Latitude));
        Assert.That(fieldValues[0].Longitude, Is.EqualTo(fieldValue.Longitude));
        Assert.That(fieldValues[0].Value, Is.EqualTo(fieldValue.Value));
        Assert.That(theCase.Id, Is.EqualTo(fieldValue.CaseId));
        Assert.That(fieldValues[0].DoneAt.ToString(), Is.EqualTo(fieldValue.DoneAt.ToString()));
        Assert.That(field.Id, Is.EqualTo(fieldValue.FieldId));
        Assert.That(worker.Id, Is.EqualTo(fieldValue.WorkerId));
        Assert.That(checklist.Id, Is.EqualTo(fieldValue.CheckListId));
        Assert.That(uploadedData.Id, Is.EqualTo(fieldValue.UploadedDataId));

        //Versions
        Assert.That(fieldValueVersions[0].CreatedAt.ToString(), Is.EqualTo(fieldValue.CreatedAt.ToString()));
        Assert.That(fieldValueVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(fieldValue.UpdatedAt.ToString(), fieldValueVersions[0].UpdatedAt.ToString());
        Assert.That(fieldValueVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(fieldValueVersions[0].FieldId, Is.EqualTo(fieldValue.Id));
        Assert.That(fieldValueVersions[0].Accuracy, Is.EqualTo(fieldValue.Accuracy));
        Assert.That(fieldValueVersions[0].Date.ToString(), Is.EqualTo(fieldValue.Date.ToString()));
        Assert.That(fieldValueVersions[0].Heading, Is.EqualTo(fieldValue.Heading));
        Assert.That(fieldValueVersions[0].Latitude, Is.EqualTo(fieldValue.Latitude));
        Assert.That(fieldValueVersions[0].Longitude, Is.EqualTo(fieldValue.Longitude));
        Assert.That(fieldValueVersions[0].Value, Is.EqualTo(fieldValue.Value));
        Assert.That(fieldValueVersions[0].CaseId, Is.EqualTo(theCase.Id));
        Assert.That(fieldValueVersions[0].DoneAt.ToString(), Is.EqualTo(fieldValue.DoneAt.ToString()));
        Assert.That(fieldValueVersions[0].FieldId, Is.EqualTo(field.Id));
        Assert.That(fieldValueVersions[0].WorkerId, Is.EqualTo(worker.Id));
        Assert.That(fieldValueVersions[0].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(fieldValueVersions[0].UploadedDataId, Is.EqualTo(uploadedData.Id));
    }

    [Test]
    public async Task FieldValues_Update_DoesUpdate()
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
            FieldTypeId = fieldType.Id
        };
        await field.Create(DbContext).ConfigureAwait(false);

        Worker worker = new Worker
        {
            Email = Guid.NewGuid().ToString(),
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            MicrotingUid = rnd.Next(1, 255),
            PinCode = "1234",
            EmployeeNo = ""
        };
        await worker.Create(DbContext).ConfigureAwait(false);

        Case theCase = new Case
        {
            Custom = Guid.NewGuid().ToString(),
            Status = rnd.Next(1, 255),
            Type = Guid.NewGuid().ToString(),
            CaseUid = Guid.NewGuid().ToString(),
            DoneAt = DateTime.UtcNow,
            FieldValue1 = Guid.NewGuid().ToString(),
            FieldValue2 = Guid.NewGuid().ToString(),
            FieldValue3 = Guid.NewGuid().ToString(),
            FieldValue4 = Guid.NewGuid().ToString(),
            FieldValue5 = Guid.NewGuid().ToString(),
            FieldValue6 = Guid.NewGuid().ToString(),
            FieldValue7 = Guid.NewGuid().ToString(),
            FieldValue8 = Guid.NewGuid().ToString(),
            FieldValue9 = Guid.NewGuid().ToString(),
            FieldValue10 = Guid.NewGuid().ToString(),
            MicrotingUid = rnd.Next(shortMinValue, shortmaxValue),
            SiteId = site.Id,
            UnitId = unit.Id,
            WorkerId = worker.Id,
            CheckListId = checklist.Id,
            MicrotingCheckUid = rnd.Next(shortMinValue, shortmaxValue)
        };
        await theCase.Create(DbContext).ConfigureAwait(false);

        UploadedData uploadedData = new UploadedData
        {
            Checksum = Guid.NewGuid().ToString(),
            Extension = Guid.NewGuid().ToString(),
            Local = (short)rnd.Next(shortMinValue, shortmaxValue),
            CurrentFile = Guid.NewGuid().ToString(),
            ExpirationDate = DateTime.UtcNow,
            FileLocation = Guid.NewGuid().ToString(),
            FileName = Guid.NewGuid().ToString(),
            TranscriptionId = rnd.Next(1, 255),
            UploaderId = rnd.Next(1, 255),
            UploaderType = Guid.NewGuid().ToString()
        };
        await uploadedData.Create(DbContext).ConfigureAwait(false);

        FieldValue fieldValue = new FieldValue
        {
            Accuracy = Guid.NewGuid().ToString(),
            Altitude = Guid.NewGuid().ToString(),
            Date = DateTime.UtcNow,
            Heading = Guid.NewGuid().ToString(),
            Latitude = Guid.NewGuid().ToString(),
            Longitude = Guid.NewGuid().ToString(),
            Value = Guid.NewGuid().ToString(),
            CaseId = theCase.Id,
            DoneAt = DateTime.UtcNow,
            FieldId = field.Id,
            WorkerId = worker.Id,
            CheckListId = checklist.Id,
            UploadedDataId = uploadedData.Id
        };
        await fieldValue.Create(DbContext).ConfigureAwait(false);

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
        fieldValue.Date = DateTime.UtcNow;
        fieldValue.Heading = Guid.NewGuid().ToString();
        fieldValue.Latitude = Guid.NewGuid().ToString();
        fieldValue.Longitude = Guid.NewGuid().ToString();
        fieldValue.Value = Guid.NewGuid().ToString();
        fieldValue.DoneAt = DateTime.UtcNow;

        await fieldValue.Update(DbContext).ConfigureAwait(false);

        List<FieldValue> fieldValues = DbContext.FieldValues.AsNoTracking().ToList();
        List<FieldValueVersion> fieldValueVersions = DbContext.FieldValueVersions.AsNoTracking().ToList();

        Assert.That(fieldValues, Is.Not.EqualTo(null));
        Assert.That(fieldValueVersions, Is.Not.EqualTo(null));

        Assert.That(fieldValues.Count(), Is.EqualTo(1));
        Assert.That(fieldValueVersions.Count(), Is.EqualTo(2));

        Assert.That(fieldValues[0].CreatedAt.ToString(), Is.EqualTo(fieldValue.CreatedAt.ToString()));
        Assert.That(fieldValues[0].Version, Is.EqualTo(fieldValue.Version));
        //            Assert.AreEqual(fieldValue.UpdatedAt.ToString(), fieldValues[0].UpdatedAt.ToString());
        Assert.That(fieldValues[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(fieldValues[0].Id, Is.EqualTo(fieldValue.Id));
        Assert.That(fieldValues[0].Accuracy, Is.EqualTo(fieldValue.Accuracy));
        Assert.That(fieldValues[0].Date.ToString(), Is.EqualTo(fieldValue.Date.ToString()));
        Assert.That(fieldValues[0].Heading, Is.EqualTo(fieldValue.Heading));
        Assert.That(fieldValues[0].Latitude, Is.EqualTo(fieldValue.Latitude));
        Assert.That(fieldValues[0].Longitude, Is.EqualTo(fieldValue.Longitude));
        Assert.That(fieldValues[0].Value, Is.EqualTo(fieldValue.Value));
        Assert.That(theCase.Id, Is.EqualTo(fieldValue.CaseId));
        Assert.That(fieldValues[0].DoneAt.ToString(), Is.EqualTo(fieldValue.DoneAt.ToString()));
        Assert.That(field.Id, Is.EqualTo(fieldValue.FieldId));
        Assert.That(worker.Id, Is.EqualTo(fieldValue.WorkerId));
        Assert.That(checklist.Id, Is.EqualTo(fieldValue.CheckListId));
        Assert.That(uploadedData.Id, Is.EqualTo(fieldValue.UploadedDataId));

        //Old Version
        Assert.That(fieldValueVersions[0].CreatedAt.ToString(), Is.EqualTo(fieldValue.CreatedAt.ToString()));
        Assert.That(fieldValueVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), fieldValueVersions[0].UpdatedAt.ToString());
        Assert.That(fieldValueVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(fieldValueVersions[0].FieldId, Is.EqualTo(fieldValue.Id));
        Assert.That(fieldValueVersions[0].Accuracy, Is.EqualTo(oldAccuracy));
        Assert.That(fieldValueVersions[0].Date.ToString(), Is.EqualTo(oldDate.ToString()));
        Assert.That(fieldValueVersions[0].Heading, Is.EqualTo(oldHeading));
        Assert.That(fieldValueVersions[0].Latitude, Is.EqualTo(oldLatitude));
        Assert.That(fieldValueVersions[0].Longitude, Is.EqualTo(oldLongitude));
        Assert.That(fieldValueVersions[0].Value, Is.EqualTo(oldValue));
        Assert.That(fieldValueVersions[0].CaseId, Is.EqualTo(theCase.Id));
        Assert.That(fieldValueVersions[0].DoneAt.ToString(), Is.EqualTo(oldDoneAt.ToString()));
        Assert.That(fieldValueVersions[0].FieldId, Is.EqualTo(field.Id));
        Assert.That(fieldValueVersions[0].WorkerId, Is.EqualTo(worker.Id));
        Assert.That(fieldValueVersions[0].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(fieldValueVersions[0].UploadedDataId, Is.EqualTo(uploadedData.Id));

        //New Version
        Assert.That(fieldValueVersions[1].CreatedAt.ToString(), Is.EqualTo(fieldValue.CreatedAt.ToString()));
        Assert.That(fieldValueVersions[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(fieldValue.UpdatedAt.ToString(), fieldValueVersions[1].UpdatedAt.ToString());
        Assert.That(fieldValueVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(fieldValueVersions[1].FieldId, Is.EqualTo(fieldValue.Id));
        Assert.That(fieldValueVersions[1].Accuracy, Is.EqualTo(fieldValue.Accuracy));
        Assert.That(fieldValueVersions[1].Date.ToString(), Is.EqualTo(fieldValue.Date.ToString()));
        Assert.That(fieldValueVersions[1].Heading, Is.EqualTo(fieldValue.Heading));
        Assert.That(fieldValueVersions[1].Latitude, Is.EqualTo(fieldValue.Latitude));
        Assert.That(fieldValueVersions[1].Longitude, Is.EqualTo(fieldValue.Longitude));
        Assert.That(fieldValueVersions[1].Value, Is.EqualTo(fieldValue.Value));
        Assert.That(fieldValueVersions[1].CaseId, Is.EqualTo(theCase.Id));
        Assert.That(fieldValueVersions[1].DoneAt.ToString(), Is.EqualTo(fieldValue.DoneAt.ToString()));
        Assert.That(fieldValueVersions[1].FieldId, Is.EqualTo(field.Id));
        Assert.That(fieldValueVersions[1].WorkerId, Is.EqualTo(worker.Id));
        Assert.That(fieldValueVersions[1].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(fieldValueVersions[1].UploadedDataId, Is.EqualTo(uploadedData.Id));
    }

    [Test]
    public async Task FieldValues_Delete_DoesSetWorkflowStateToRemoved()
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
            FieldTypeId = fieldType.Id
        };
        await field.Create(DbContext).ConfigureAwait(false);

        Worker worker = new Worker
        {
            Email = Guid.NewGuid().ToString(),
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            MicrotingUid = rnd.Next(1, 255),
            PinCode = "1234",
            EmployeeNo = ""
        };
        await worker.Create(DbContext).ConfigureAwait(false);

        Case theCase = new Case
        {
            Custom = Guid.NewGuid().ToString(),
            Status = rnd.Next(1, 255),
            Type = Guid.NewGuid().ToString(),
            CaseUid = Guid.NewGuid().ToString(),
            DoneAt = DateTime.UtcNow,
            FieldValue1 = Guid.NewGuid().ToString(),
            FieldValue2 = Guid.NewGuid().ToString(),
            FieldValue3 = Guid.NewGuid().ToString(),
            FieldValue4 = Guid.NewGuid().ToString(),
            FieldValue5 = Guid.NewGuid().ToString(),
            FieldValue6 = Guid.NewGuid().ToString(),
            FieldValue7 = Guid.NewGuid().ToString(),
            FieldValue8 = Guid.NewGuid().ToString(),
            FieldValue9 = Guid.NewGuid().ToString(),
            FieldValue10 = Guid.NewGuid().ToString(),
            MicrotingUid = rnd.Next(shortMinValue, shortmaxValue),
            SiteId = site.Id,
            UnitId = unit.Id,
            WorkerId = worker.Id,
            CheckListId = checklist.Id,
            MicrotingCheckUid = rnd.Next(shortMinValue, shortmaxValue)
        };
        await theCase.Create(DbContext).ConfigureAwait(false);

        UploadedData uploadedData = new UploadedData
        {
            Checksum = Guid.NewGuid().ToString(),
            Extension = Guid.NewGuid().ToString(),
            Local = (short)rnd.Next(shortMinValue, shortmaxValue),
            CurrentFile = Guid.NewGuid().ToString(),
            ExpirationDate = DateTime.UtcNow,
            FileLocation = Guid.NewGuid().ToString(),
            FileName = Guid.NewGuid().ToString(),
            TranscriptionId = rnd.Next(1, 255),
            UploaderId = rnd.Next(1, 255),
            UploaderType = Guid.NewGuid().ToString()
        };
        await uploadedData.Create(DbContext).ConfigureAwait(false);

        FieldValue fieldValue = new FieldValue
        {
            Accuracy = Guid.NewGuid().ToString(),
            Altitude = Guid.NewGuid().ToString(),
            Date = DateTime.UtcNow,
            Heading = Guid.NewGuid().ToString(),
            Latitude = Guid.NewGuid().ToString(),
            Longitude = Guid.NewGuid().ToString(),
            Value = Guid.NewGuid().ToString(),
            CaseId = theCase.Id,
            DoneAt = DateTime.UtcNow,
            FieldId = field.Id,
            WorkerId = worker.Id,
            CheckListId = checklist.Id,
            UploadedDataId = uploadedData.Id
        };
        await fieldValue.Create(DbContext).ConfigureAwait(false);

        //Act

        DateTime? oldUpdatedAt = fieldValue.UpdatedAt;


        await fieldValue.Delete(DbContext);

        List<FieldValue> fieldValues = DbContext.FieldValues.AsNoTracking().ToList();
        List<FieldValueVersion> fieldValueVersions = DbContext.FieldValueVersions.AsNoTracking().ToList();

        Assert.That(fieldValues, Is.Not.EqualTo(null));
        Assert.That(fieldValueVersions, Is.Not.EqualTo(null));

        Assert.That(fieldValues.Count(), Is.EqualTo(1));
        Assert.That(fieldValueVersions.Count(), Is.EqualTo(2));

        Assert.That(fieldValues[0].CreatedAt.ToString(), Is.EqualTo(fieldValue.CreatedAt.ToString()));
        Assert.That(fieldValues[0].Version, Is.EqualTo(fieldValue.Version));
        //            Assert.AreEqual(fieldValue.UpdatedAt.ToString(), fieldValues[0].UpdatedAt.ToString());
        Assert.That(fieldValues[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(fieldValues[0].Id, Is.EqualTo(fieldValue.Id));
        Assert.That(fieldValues[0].Accuracy, Is.EqualTo(fieldValue.Accuracy));
        Assert.That(fieldValues[0].Date.ToString(), Is.EqualTo(fieldValue.Date.ToString()));
        Assert.That(fieldValues[0].Heading, Is.EqualTo(fieldValue.Heading));
        Assert.That(fieldValues[0].Latitude, Is.EqualTo(fieldValue.Latitude));
        Assert.That(fieldValues[0].Longitude, Is.EqualTo(fieldValue.Longitude));
        Assert.That(fieldValues[0].Value, Is.EqualTo(fieldValue.Value));
        Assert.That(theCase.Id, Is.EqualTo(fieldValue.CaseId));
        Assert.That(fieldValues[0].DoneAt.ToString(), Is.EqualTo(fieldValue.DoneAt.ToString()));
        Assert.That(field.Id, Is.EqualTo(fieldValue.FieldId));
        Assert.That(worker.Id, Is.EqualTo(fieldValue.WorkerId));
        Assert.That(checklist.Id, Is.EqualTo(fieldValue.CheckListId));
        Assert.That(uploadedData.Id, Is.EqualTo(fieldValue.UploadedDataId));

        //Old Version
        Assert.That(fieldValueVersions[0].CreatedAt.ToString(), Is.EqualTo(fieldValue.CreatedAt.ToString()));
        Assert.That(fieldValueVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), fieldValueVersions[0].UpdatedAt.ToString());
        Assert.That(fieldValueVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(fieldValueVersions[0].FieldId, Is.EqualTo(fieldValue.Id));
        Assert.That(fieldValueVersions[0].Accuracy, Is.EqualTo(fieldValue.Accuracy));
        Assert.That(fieldValueVersions[0].Date.ToString(), Is.EqualTo(fieldValue.Date.ToString()));
        Assert.That(fieldValueVersions[0].Heading, Is.EqualTo(fieldValue.Heading));
        Assert.That(fieldValueVersions[0].Latitude, Is.EqualTo(fieldValue.Latitude));
        Assert.That(fieldValueVersions[0].Longitude, Is.EqualTo(fieldValue.Longitude));
        Assert.That(fieldValueVersions[0].Value, Is.EqualTo(fieldValue.Value));
        Assert.That(fieldValueVersions[0].CaseId, Is.EqualTo(theCase.Id));
        Assert.That(fieldValueVersions[0].DoneAt.ToString(), Is.EqualTo(fieldValue.DoneAt.ToString()));
        Assert.That(fieldValueVersions[0].FieldId, Is.EqualTo(field.Id));
        Assert.That(fieldValueVersions[0].WorkerId, Is.EqualTo(worker.Id));
        Assert.That(fieldValueVersions[0].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(fieldValueVersions[0].UploadedDataId, Is.EqualTo(uploadedData.Id));

        //New Version
        Assert.That(fieldValueVersions[1].CreatedAt.ToString(), Is.EqualTo(fieldValue.CreatedAt.ToString()));
        Assert.That(fieldValueVersions[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(fieldValue.UpdatedAt.ToString(), fieldValueVersions[1].UpdatedAt.ToString());
        Assert.That(fieldValueVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(fieldValueVersions[1].FieldId, Is.EqualTo(fieldValue.Id));
        Assert.That(fieldValueVersions[1].Accuracy, Is.EqualTo(fieldValue.Accuracy));
        Assert.That(fieldValueVersions[1].Date.ToString(), Is.EqualTo(fieldValue.Date.ToString()));
        Assert.That(fieldValueVersions[1].Heading, Is.EqualTo(fieldValue.Heading));
        Assert.That(fieldValueVersions[1].Latitude, Is.EqualTo(fieldValue.Latitude));
        Assert.That(fieldValueVersions[1].Longitude, Is.EqualTo(fieldValue.Longitude));
        Assert.That(fieldValueVersions[1].Value, Is.EqualTo(fieldValue.Value));
        Assert.That(fieldValueVersions[1].CaseId, Is.EqualTo(theCase.Id));
        Assert.That(fieldValueVersions[1].DoneAt.ToString(), Is.EqualTo(fieldValue.DoneAt.ToString()));
        Assert.That(fieldValueVersions[1].FieldId, Is.EqualTo(field.Id));
        Assert.That(fieldValueVersions[1].WorkerId, Is.EqualTo(worker.Id));
        Assert.That(fieldValueVersions[1].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(fieldValueVersions[1].UploadedDataId, Is.EqualTo(uploadedData.Id));
    }
}