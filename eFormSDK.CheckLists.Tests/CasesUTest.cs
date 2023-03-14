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
    public class CasesUTest : DbTestFixture
    {
        [Test]
        public async Task Cases_Create_DoesCreate()
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


            //Act

            await theCase.Create(DbContext).ConfigureAwait(false);

            List<Case> cases = DbContext.Cases.AsNoTracking().ToList();
            List<CaseVersion> caseVersions = DbContext.CaseVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(cases);
            Assert.NotNull(caseVersions);

            Assert.AreEqual(1, cases.Count());
            Assert.AreEqual(1, caseVersions.Count());

            Assert.AreEqual(theCase.CreatedAt.ToString(), cases[0].CreatedAt.ToString());
            Assert.AreEqual(theCase.Version, cases[0].Version);
//             Assert.AreEqual(theCase.UpdatedAt.ToString(), cases[0].UpdatedAt.ToString());
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
//             Assert.AreEqual(theCase.UpdatedAt.ToString(), caseVersions[0].UpdatedAt.ToString());
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
        public async Task Cases_Update_DoesUpdate()
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
            theCase.DoneAt = DateTime.UtcNow;
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

            await theCase.Update(DbContext).ConfigureAwait(false);

            List<Case> cases = DbContext.Cases.AsNoTracking().ToList();
            List<CaseVersion> caseVersions = DbContext.CaseVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(cases);
            Assert.NotNull(caseVersions);

            Assert.AreEqual(1, cases.Count());
            Assert.AreEqual(2, caseVersions.Count());

            Assert.AreEqual(theCase.CreatedAt.ToString(), cases[0].CreatedAt.ToString());
            Assert.AreEqual(theCase.Version, cases[0].Version);
//            Assert.AreEqual(theCase.UpdatedAt.ToString(), cases[0].UpdatedAt.ToString());
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
//            Assert.AreEqual(oldUpdatedAt.ToString(), caseVersions[0].UpdatedAt.ToString());
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
//            Assert.AreEqual(theCase.UpdatedAt.ToString(), caseVersions[1].UpdatedAt.ToString());
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
        public async Task Cases_Delete_DoesSetWorkflowStateToRemoved()
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


            //Act

            DateTime? oldUpdatedAt = theCase.UpdatedAt;

            await theCase.Delete(DbContext);

            List<Case> cases = DbContext.Cases.AsNoTracking().ToList();
            List<CaseVersion> caseVersions = DbContext.CaseVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(cases);
            Assert.NotNull(caseVersions);

            Assert.AreEqual(1, cases.Count());
            Assert.AreEqual(2, caseVersions.Count());

            Assert.AreEqual(theCase.CreatedAt.ToString(), cases[0].CreatedAt.ToString());
            Assert.AreEqual(theCase.Version, cases[0].Version);
//            Assert.AreEqual(theCase.UpdatedAt.ToString(), cases[0].UpdatedAt.ToString());
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
//            Assert.AreEqual(oldUpdatedAt.ToString(), caseVersions[0].UpdatedAt.ToString());
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
//            Assert.AreEqual(theCase.UpdatedAt.ToString(), caseVersions[1].UpdatedAt.ToString());
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