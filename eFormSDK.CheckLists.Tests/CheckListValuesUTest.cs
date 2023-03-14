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
    public class CheckListValuesUTest : DbTestFixture
    {
        [Test]
        public async Task CheckListValues_Create_DoesCreate()
        {
            //Arrange

            Random rnd = new Random();

            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;

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

            Worker worker = new Worker
            {
                Email = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await worker.Create(DbContext).ConfigureAwait(false);

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

            CheckListValue checkListValue = new CheckListValue
            {
                Status = Guid.NewGuid().ToString(),
                CaseId = theCase.Id,
                CheckListId = checklist.Id
            };

            //Act

            await checkListValue.Create(DbContext).ConfigureAwait(false);

            List<CheckListValue> checkListValues = DbContext.CheckListValues.AsNoTracking().ToList();
            List<CheckListValueVersion> checkListValueVersions =
                DbContext.CheckListValueVersions.AsNoTracking().ToList();

            Assert.NotNull(checkListValues);
            Assert.NotNull(checkListValueVersions);

            Assert.AreEqual(1, checkListValues.Count());
            Assert.AreEqual(1, checkListValueVersions.Count());

            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValues[0].CreatedAt.ToString());
            Assert.AreEqual(checkListValue.Version, checkListValues[0].Version);
//            Assert.AreEqual(checkListValue.UpdatedAt.ToString(), checkListValues[0].UpdatedAt.ToString());
            Assert.AreEqual(checkListValues[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListValue.Id, checkListValues[0].Id);
            Assert.AreEqual(checkListValue.Status, checkListValues[0].Status);
            Assert.AreEqual(checkListValue.CaseId, theCase.Id);
            Assert.AreEqual(checkListValue.CheckListId, checklist.Id);

            //Versions
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValueVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, checkListValueVersions[0].Version);
//            Assert.AreEqual(checkListValue.UpdatedAt.ToString(), checkListValueVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(checkListValueVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListValue.Id, checkListValueVersions[0].Id);
            Assert.AreEqual(checkListValue.Status, checkListValueVersions[0].Status);
            Assert.AreEqual(theCase.Id, checkListValueVersions[0].CaseId);
            Assert.AreEqual(checklist.Id, checkListValueVersions[0].CheckListId);
        }

        [Test]
        public async Task CheckListValues_Update_DoesUpdate()
        {
            //Arrange

            Random rnd = new Random();

            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;

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

            Worker worker = new Worker
            {
                Email = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await worker.Create(DbContext).ConfigureAwait(false);

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

            CheckListValue checkListValue = new CheckListValue
            {
                Status = Guid.NewGuid().ToString(),
                CaseId = theCase.Id,
                CheckListId = checklist.Id
            };
            await checkListValue.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = checkListValue.UpdatedAt;
            string oldStatus = checkListValue.Status;

            checkListValue.Status = Guid.NewGuid().ToString();

            await checkListValue.Update(DbContext).ConfigureAwait(false);

            List<CheckListValue> checkListValues = DbContext.CheckListValues.AsNoTracking().ToList();
            List<CheckListValueVersion> checkListValueVersions =
                DbContext.CheckListValueVersions.AsNoTracking().ToList();

            Assert.NotNull(checkListValues);
            Assert.NotNull(checkListValueVersions);

            Assert.AreEqual(1, checkListValues.Count());
            Assert.AreEqual(2, checkListValueVersions.Count());

            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValues[0].CreatedAt.ToString());
            Assert.AreEqual(checkListValue.Version, checkListValues[0].Version);
//            Assert.AreEqual(checkListValue.UpdatedAt.ToString(), checkListValues[0].UpdatedAt.ToString());
            Assert.AreEqual(checkListValues[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListValue.Id, checkListValues[0].Id);
            Assert.AreEqual(checkListValue.Status, checkListValues[0].Status);
            Assert.AreEqual(checkListValue.CaseId, theCase.Id);
            Assert.AreEqual(checkListValue.CheckListId, checklist.Id);

            //Old Version
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValueVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, checkListValueVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), checkListValueVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(checkListValueVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListValue.Id, checkListValueVersions[0].CheckListValueId);
            Assert.AreEqual(oldStatus, checkListValueVersions[0].Status);
            Assert.AreEqual(theCase.Id, checkListValueVersions[0].CaseId);
            Assert.AreEqual(checklist.Id, checkListValueVersions[0].CheckListId);

            //New Version
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValueVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, checkListValueVersions[1].Version);
//            Assert.AreEqual(checkListValue.UpdatedAt.ToString(), checkListValueVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(checkListValueVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListValue.Id, checkListValueVersions[1].CheckListValueId);
            Assert.AreEqual(checkListValue.Status, checkListValueVersions[1].Status);
            Assert.AreEqual(theCase.Id, checkListValueVersions[1].CaseId);
            Assert.AreEqual(checklist.Id, checkListValueVersions[1].CheckListId);
        }

        [Test]
        public async Task CheckListValues_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange

            Random rnd = new Random();

            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;

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

            Worker worker = new Worker
            {
                Email = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await worker.Create(DbContext).ConfigureAwait(false);

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

            CheckListValue checkListValue = new CheckListValue
            {
                Status = Guid.NewGuid().ToString(),
                CaseId = theCase.Id,
                CheckListId = checklist.Id
            };
            await checkListValue.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = checkListValue.UpdatedAt;

            await checkListValue.Delete(DbContext);

            List<CheckListValue> checkListValues = DbContext.CheckListValues.AsNoTracking().ToList();
            List<CheckListValueVersion> checkListValueVersions =
                DbContext.CheckListValueVersions.AsNoTracking().ToList();

            Assert.NotNull(checkListValues);
            Assert.NotNull(checkListValueVersions);

            Assert.AreEqual(1, checkListValues.Count());
            Assert.AreEqual(2, checkListValueVersions.Count());

            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValues[0].CreatedAt.ToString());
            Assert.AreEqual(checkListValue.Version, checkListValues[0].Version);
//            Assert.AreEqual(checkListValue.UpdatedAt.ToString(), checkListValues[0].UpdatedAt.ToString());
            Assert.AreEqual(checkListValues[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(checkListValue.Id, checkListValues[0].Id);
            Assert.AreEqual(checkListValue.Status, checkListValues[0].Status);
            Assert.AreEqual(checkListValue.CaseId, theCase.Id);
            Assert.AreEqual(checkListValue.CheckListId, checklist.Id);

            //Old Version
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValueVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, checkListValueVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), checkListValueVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(checkListValueVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListValue.Id, checkListValueVersions[0].CheckListValueId);
            Assert.AreEqual(checkListValue.Status, checkListValueVersions[0].Status);
            Assert.AreEqual(theCase.Id, checkListValueVersions[0].CaseId);
            Assert.AreEqual(checklist.Id, checkListValueVersions[0].CheckListId);

            //New Version
            Assert.AreEqual(checkListValue.CreatedAt.ToString(), checkListValueVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, checkListValueVersions[1].Version);
//            Assert.AreEqual(checkListValue.UpdatedAt.ToString(), checkListValueVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(checkListValueVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(checkListValue.Id, checkListValueVersions[1].CheckListValueId);
            Assert.AreEqual(checkListValue.Status, checkListValueVersions[1].Status);
            Assert.AreEqual(theCase.Id, checkListValueVersions[1].CaseId);
            Assert.AreEqual(checklist.Id, checkListValueVersions[1].CheckListId);
        }
    }
}