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
            MicrotingUid = rnd.Next(1, 255),
            PinCode = "1234",
            EmployeeNo = ""
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

        Assert.That(cases, Is.Not.EqualTo(null));
        Assert.That(caseVersions, Is.Not.EqualTo(null));

        Assert.That(cases.Count(), Is.EqualTo(1));
        Assert.That(caseVersions.Count(), Is.EqualTo(1));

        Assert.That(cases[0].CreatedAt.ToString(), Is.EqualTo(theCase.CreatedAt.ToString()));
        Assert.That(cases[0].Version, Is.EqualTo(theCase.Version));
        //             Assert.AreEqual(theCase.UpdatedAt.ToString(), cases[0].UpdatedAt.ToString());
        Assert.That(cases[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(cases[0].Id, Is.EqualTo(theCase.Id));
        Assert.That(cases[0].Custom, Is.EqualTo(theCase.Custom));
        Assert.That(site.Id, Is.EqualTo(theCase.SiteId));
        Assert.That(cases[0].Status, Is.EqualTo(theCase.Status));
        Assert.That(cases[0].Type, Is.EqualTo(theCase.Type));
        Assert.That(unit.Id, Is.EqualTo(theCase.UnitId));
        Assert.That(worker.Id, Is.EqualTo(theCase.WorkerId));
        Assert.That(cases[0].CaseUid, Is.EqualTo(theCase.CaseUid));
        Assert.That(checklist.Id, Is.EqualTo(theCase.CheckListId));
        Assert.That(cases[0].DoneAt.ToString(), Is.EqualTo(theCase.DoneAt.ToString()));
        Assert.That(cases[0].FieldValue1, Is.EqualTo(theCase.FieldValue1));
        Assert.That(cases[0].FieldValue2, Is.EqualTo(theCase.FieldValue2));
        Assert.That(cases[0].FieldValue3, Is.EqualTo(theCase.FieldValue3));
        Assert.That(cases[0].FieldValue4, Is.EqualTo(theCase.FieldValue4));
        Assert.That(cases[0].FieldValue5, Is.EqualTo(theCase.FieldValue5));
        Assert.That(cases[0].FieldValue6, Is.EqualTo(theCase.FieldValue6));
        Assert.That(cases[0].FieldValue7, Is.EqualTo(theCase.FieldValue7));
        Assert.That(cases[0].FieldValue8, Is.EqualTo(theCase.FieldValue8));
        Assert.That(cases[0].FieldValue9, Is.EqualTo(theCase.FieldValue9));
        Assert.That(cases[0].FieldValue10, Is.EqualTo(theCase.FieldValue10));
        Assert.That(cases[0].MicrotingUid, Is.EqualTo(theCase.MicrotingUid));
        Assert.That(cases[0].MicrotingCheckUid, Is.EqualTo(theCase.MicrotingCheckUid));

        //Versions
        Assert.That(caseVersions[0].CreatedAt.ToString(), Is.EqualTo(theCase.CreatedAt.ToString()));
        Assert.That(caseVersions[0].Version, Is.EqualTo(1));
        //             Assert.AreEqual(theCase.UpdatedAt.ToString(), caseVersions[0].UpdatedAt.ToString());
        Assert.That(caseVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(caseVersions[0].CaseId, Is.EqualTo(theCase.Id));
        Assert.That(caseVersions[0].Custom, Is.EqualTo(theCase.Custom));
        Assert.That(caseVersions[0].SiteId, Is.EqualTo(site.Id));
        Assert.That(caseVersions[0].Status, Is.EqualTo(theCase.Status));
        Assert.That(caseVersions[0].Type, Is.EqualTo(theCase.Type));
        Assert.That(caseVersions[0].UnitId, Is.EqualTo(unit.Id));
        Assert.That(caseVersions[0].WorkerId, Is.EqualTo(worker.Id));
        Assert.That(caseVersions[0].CaseUid, Is.EqualTo(theCase.CaseUid));
        Assert.That(caseVersions[0].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(caseVersions[0].DoneAt.ToString(), Is.EqualTo(theCase.DoneAt.ToString()));
        Assert.That(caseVersions[0].FieldValue1, Is.EqualTo(theCase.FieldValue1));
        Assert.That(caseVersions[0].FieldValue2, Is.EqualTo(theCase.FieldValue2));
        Assert.That(caseVersions[0].FieldValue3, Is.EqualTo(theCase.FieldValue3));
        Assert.That(caseVersions[0].FieldValue4, Is.EqualTo(theCase.FieldValue4));
        Assert.That(caseVersions[0].FieldValue5, Is.EqualTo(theCase.FieldValue5));
        Assert.That(caseVersions[0].FieldValue6, Is.EqualTo(theCase.FieldValue6));
        Assert.That(caseVersions[0].FieldValue7, Is.EqualTo(theCase.FieldValue7));
        Assert.That(caseVersions[0].FieldValue8, Is.EqualTo(theCase.FieldValue8));
        Assert.That(caseVersions[0].FieldValue9, Is.EqualTo(theCase.FieldValue9));
        Assert.That(caseVersions[0].FieldValue10, Is.EqualTo(theCase.FieldValue10));
        Assert.That(caseVersions[0].MicrotingUid, Is.EqualTo(theCase.MicrotingUid));
        Assert.That(caseVersions[0].MicrotingCheckUid, Is.EqualTo(theCase.MicrotingCheckUid));
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
            MicrotingUid = rnd.Next(1, 255),
            PinCode = "1234",
            EmployeeNo = ""
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

        Assert.That(cases, Is.Not.EqualTo(null));
        Assert.That(caseVersions, Is.Not.EqualTo(null));

        Assert.That(cases.Count(), Is.EqualTo(1));
        Assert.That(caseVersions.Count(), Is.EqualTo(2));

        Assert.That(cases[0].CreatedAt.ToString(), Is.EqualTo(theCase.CreatedAt.ToString()));
        Assert.That(cases[0].Version, Is.EqualTo(theCase.Version));
        //            Assert.AreEqual(theCase.UpdatedAt.ToString(), cases[0].UpdatedAt.ToString());
        Assert.That(cases[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(cases[0].Id, Is.EqualTo(theCase.Id));
        Assert.That(cases[0].Custom, Is.EqualTo(theCase.Custom));
        Assert.That(site.Id, Is.EqualTo(theCase.SiteId));
        Assert.That(cases[0].Status, Is.EqualTo(theCase.Status));
        Assert.That(cases[0].Type, Is.EqualTo(theCase.Type));
        Assert.That(unit.Id, Is.EqualTo(theCase.UnitId));
        Assert.That(worker.Id, Is.EqualTo(theCase.WorkerId));
        Assert.That(cases[0].CaseUid, Is.EqualTo(theCase.CaseUid));
        Assert.That(checklist.Id, Is.EqualTo(theCase.CheckListId));
        Assert.That(cases[0].DoneAt.ToString(), Is.EqualTo(theCase.DoneAt.ToString()));
        Assert.That(cases[0].FieldValue1, Is.EqualTo(theCase.FieldValue1));
        Assert.That(cases[0].FieldValue2, Is.EqualTo(theCase.FieldValue2));
        Assert.That(cases[0].FieldValue3, Is.EqualTo(theCase.FieldValue3));
        Assert.That(cases[0].FieldValue4, Is.EqualTo(theCase.FieldValue4));
        Assert.That(cases[0].FieldValue5, Is.EqualTo(theCase.FieldValue5));
        Assert.That(cases[0].FieldValue6, Is.EqualTo(theCase.FieldValue6));
        Assert.That(cases[0].FieldValue7, Is.EqualTo(theCase.FieldValue7));
        Assert.That(cases[0].FieldValue8, Is.EqualTo(theCase.FieldValue8));
        Assert.That(cases[0].FieldValue9, Is.EqualTo(theCase.FieldValue9));
        Assert.That(cases[0].FieldValue10, Is.EqualTo(theCase.FieldValue10));
        Assert.That(cases[0].MicrotingUid, Is.EqualTo(theCase.MicrotingUid));
        Assert.That(cases[0].MicrotingCheckUid, Is.EqualTo(theCase.MicrotingCheckUid));

        //Old Versions
        Assert.That(caseVersions[0].CreatedAt.ToString(), Is.EqualTo(theCase.CreatedAt.ToString()));
        Assert.That(caseVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), caseVersions[0].UpdatedAt.ToString());
        Assert.That(caseVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(caseVersions[0].Id, Is.EqualTo(theCase.Id));
        Assert.That(caseVersions[0].Custom, Is.EqualTo(oldCustom));
        Assert.That(caseVersions[0].SiteId, Is.EqualTo(site.Id));
        Assert.That(caseVersions[0].Status, Is.EqualTo(oldStatus));
        Assert.That(caseVersions[0].Type, Is.EqualTo(oldType));
        Assert.That(caseVersions[0].UnitId, Is.EqualTo(unit.Id));
        Assert.That(caseVersions[0].WorkerId, Is.EqualTo(worker.Id));
        Assert.That(caseVersions[0].CaseUid, Is.EqualTo(oldCaseUid));
        Assert.That(caseVersions[0].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(caseVersions[0].DoneAt.ToString(), Is.EqualTo(oldDoneAt.ToString()));
        Assert.That(caseVersions[0].FieldValue1, Is.EqualTo(oldFieldValue1));
        Assert.That(caseVersions[0].FieldValue2, Is.EqualTo(oldFieldValue2));
        Assert.That(caseVersions[0].FieldValue3, Is.EqualTo(oldFieldValue3));
        Assert.That(caseVersions[0].FieldValue4, Is.EqualTo(oldFieldValue4));
        Assert.That(caseVersions[0].FieldValue5, Is.EqualTo(oldFieldValue5));
        Assert.That(caseVersions[0].FieldValue6, Is.EqualTo(oldFieldValue6));
        Assert.That(caseVersions[0].FieldValue7, Is.EqualTo(oldFieldValue7));
        Assert.That(caseVersions[0].FieldValue8, Is.EqualTo(oldFieldValue8));
        Assert.That(caseVersions[0].FieldValue9, Is.EqualTo(oldFieldValue9));
        Assert.That(caseVersions[0].FieldValue10, Is.EqualTo(oldFieldValue10));
        Assert.That(caseVersions[0].MicrotingUid, Is.EqualTo(oldMicrotingUid));
        Assert.That(caseVersions[0].MicrotingCheckUid, Is.EqualTo(oldMicrotingCheckUid));

        //New Version
        Assert.That(caseVersions[1].CreatedAt.ToString(), Is.EqualTo(theCase.CreatedAt.ToString()));
        Assert.That(caseVersions[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(theCase.UpdatedAt.ToString(), caseVersions[1].UpdatedAt.ToString());
        Assert.That(caseVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(caseVersions[1].CaseId, Is.EqualTo(theCase.Id));
        Assert.That(caseVersions[1].Custom, Is.EqualTo(theCase.Custom));
        Assert.That(caseVersions[1].SiteId, Is.EqualTo(site.Id));
        Assert.That(caseVersions[1].Status, Is.EqualTo(theCase.Status));
        Assert.That(caseVersions[1].Type, Is.EqualTo(theCase.Type));
        Assert.That(caseVersions[1].UnitId, Is.EqualTo(unit.Id));
        Assert.That(caseVersions[1].WorkerId, Is.EqualTo(worker.Id));
        Assert.That(caseVersions[1].CaseUid, Is.EqualTo(theCase.CaseUid));
        Assert.That(caseVersions[1].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(caseVersions[1].DoneAt.ToString(), Is.EqualTo(theCase.DoneAt.ToString()));
        Assert.That(caseVersions[1].FieldValue1, Is.EqualTo(theCase.FieldValue1));
        Assert.That(caseVersions[1].FieldValue2, Is.EqualTo(theCase.FieldValue2));
        Assert.That(caseVersions[1].FieldValue3, Is.EqualTo(theCase.FieldValue3));
        Assert.That(caseVersions[1].FieldValue4, Is.EqualTo(theCase.FieldValue4));
        Assert.That(caseVersions[1].FieldValue5, Is.EqualTo(theCase.FieldValue5));
        Assert.That(caseVersions[1].FieldValue6, Is.EqualTo(theCase.FieldValue6));
        Assert.That(caseVersions[1].FieldValue7, Is.EqualTo(theCase.FieldValue7));
        Assert.That(caseVersions[1].FieldValue8, Is.EqualTo(theCase.FieldValue8));
        Assert.That(caseVersions[1].FieldValue9, Is.EqualTo(theCase.FieldValue9));
        Assert.That(caseVersions[1].FieldValue10, Is.EqualTo(theCase.FieldValue10));
        Assert.That(caseVersions[1].MicrotingUid, Is.EqualTo(theCase.MicrotingUid));
        Assert.That(caseVersions[1].MicrotingCheckUid, Is.EqualTo(theCase.MicrotingCheckUid));
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
            MicrotingUid = rnd.Next(1, 255),
            PinCode = "1234",
            EmployeeNo = ""
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

        Assert.That(cases, Is.Not.EqualTo(null));
        Assert.That(caseVersions, Is.Not.EqualTo(null));

        Assert.That(cases.Count(), Is.EqualTo(1));
        Assert.That(caseVersions.Count(), Is.EqualTo(2));

        Assert.That(cases[0].CreatedAt.ToString(), Is.EqualTo(theCase.CreatedAt.ToString()));
        Assert.That(cases[0].Version, Is.EqualTo(theCase.Version));
        //            Assert.AreEqual(theCase.UpdatedAt.ToString(), cases[0].UpdatedAt.ToString());
        Assert.That(cases[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(cases[0].Id, Is.EqualTo(theCase.Id));
        Assert.That(cases[0].Custom, Is.EqualTo(theCase.Custom));
        Assert.That(site.Id, Is.EqualTo(theCase.SiteId));
        Assert.That(cases[0].Status, Is.EqualTo(theCase.Status));
        Assert.That(cases[0].Type, Is.EqualTo(theCase.Type));
        Assert.That(unit.Id, Is.EqualTo(theCase.UnitId));
        Assert.That(worker.Id, Is.EqualTo(theCase.WorkerId));
        Assert.That(cases[0].CaseUid, Is.EqualTo(theCase.CaseUid));
        Assert.That(checklist.Id, Is.EqualTo(theCase.CheckListId));
        Assert.That(cases[0].DoneAt.ToString(), Is.EqualTo(theCase.DoneAt.ToString()));
        Assert.That(cases[0].FieldValue1, Is.EqualTo(theCase.FieldValue1));
        Assert.That(cases[0].FieldValue2, Is.EqualTo(theCase.FieldValue2));
        Assert.That(cases[0].FieldValue3, Is.EqualTo(theCase.FieldValue3));
        Assert.That(cases[0].FieldValue4, Is.EqualTo(theCase.FieldValue4));
        Assert.That(cases[0].FieldValue5, Is.EqualTo(theCase.FieldValue5));
        Assert.That(cases[0].FieldValue6, Is.EqualTo(theCase.FieldValue6));
        Assert.That(cases[0].FieldValue7, Is.EqualTo(theCase.FieldValue7));
        Assert.That(cases[0].FieldValue8, Is.EqualTo(theCase.FieldValue8));
        Assert.That(cases[0].FieldValue9, Is.EqualTo(theCase.FieldValue9));
        Assert.That(cases[0].FieldValue10, Is.EqualTo(theCase.FieldValue10));
        Assert.That(cases[0].MicrotingUid, Is.EqualTo(theCase.MicrotingUid));
        Assert.That(cases[0].MicrotingCheckUid, Is.EqualTo(theCase.MicrotingCheckUid));

        //Old Version
        Assert.That(caseVersions[0].CreatedAt.ToString(), Is.EqualTo(theCase.CreatedAt.ToString()));
        Assert.That(caseVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), caseVersions[0].UpdatedAt.ToString());
        Assert.That(caseVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(caseVersions[0].CaseId, Is.EqualTo(theCase.Id));
        Assert.That(caseVersions[0].Custom, Is.EqualTo(theCase.Custom));
        Assert.That(caseVersions[0].SiteId, Is.EqualTo(site.Id));
        Assert.That(caseVersions[0].Status, Is.EqualTo(theCase.Status));
        Assert.That(caseVersions[0].Type, Is.EqualTo(theCase.Type));
        Assert.That(caseVersions[0].UnitId, Is.EqualTo(unit.Id));
        Assert.That(caseVersions[0].WorkerId, Is.EqualTo(worker.Id));
        Assert.That(caseVersions[0].CaseUid, Is.EqualTo(theCase.CaseUid));
        Assert.That(caseVersions[0].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(caseVersions[0].DoneAt.ToString(), Is.EqualTo(theCase.DoneAt.ToString()));
        Assert.That(caseVersions[0].FieldValue1, Is.EqualTo(theCase.FieldValue1));
        Assert.That(caseVersions[0].FieldValue2, Is.EqualTo(theCase.FieldValue2));
        Assert.That(caseVersions[0].FieldValue3, Is.EqualTo(theCase.FieldValue3));
        Assert.That(caseVersions[0].FieldValue4, Is.EqualTo(theCase.FieldValue4));
        Assert.That(caseVersions[0].FieldValue5, Is.EqualTo(theCase.FieldValue5));
        Assert.That(caseVersions[0].FieldValue6, Is.EqualTo(theCase.FieldValue6));
        Assert.That(caseVersions[0].FieldValue7, Is.EqualTo(theCase.FieldValue7));
        Assert.That(caseVersions[0].FieldValue8, Is.EqualTo(theCase.FieldValue8));
        Assert.That(caseVersions[0].FieldValue9, Is.EqualTo(theCase.FieldValue9));
        Assert.That(caseVersions[0].FieldValue10, Is.EqualTo(theCase.FieldValue10));
        Assert.That(caseVersions[0].MicrotingUid, Is.EqualTo(theCase.MicrotingUid));
        Assert.That(caseVersions[0].MicrotingCheckUid, Is.EqualTo(theCase.MicrotingCheckUid));

        //New Version
        Assert.That(caseVersions[1].CreatedAt.ToString(), Is.EqualTo(theCase.CreatedAt.ToString()));
        Assert.That(cases[0].Version, Is.EqualTo(2));
        //            Assert.AreEqual(theCase.UpdatedAt.ToString(), caseVersions[1].UpdatedAt.ToString());
        Assert.That(caseVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(caseVersions[1].CaseId, Is.EqualTo(theCase.Id));
        Assert.That(caseVersions[1].Custom, Is.EqualTo(theCase.Custom));
        Assert.That(caseVersions[1].SiteId, Is.EqualTo(site.Id));
        Assert.That(caseVersions[1].Status, Is.EqualTo(theCase.Status));
        Assert.That(caseVersions[1].Type, Is.EqualTo(theCase.Type));
        Assert.That(caseVersions[1].UnitId, Is.EqualTo(unit.Id));
        Assert.That(caseVersions[1].WorkerId, Is.EqualTo(worker.Id));
        Assert.That(caseVersions[1].CaseUid, Is.EqualTo(theCase.CaseUid));
        Assert.That(caseVersions[1].CheckListId, Is.EqualTo(checklist.Id));
        Assert.That(caseVersions[1].DoneAt.ToString(), Is.EqualTo(theCase.DoneAt.ToString()));
        Assert.That(caseVersions[1].FieldValue1, Is.EqualTo(theCase.FieldValue1));
        Assert.That(caseVersions[1].FieldValue2, Is.EqualTo(theCase.FieldValue2));
        Assert.That(caseVersions[1].FieldValue3, Is.EqualTo(theCase.FieldValue3));
        Assert.That(caseVersions[1].FieldValue4, Is.EqualTo(theCase.FieldValue4));
        Assert.That(caseVersions[1].FieldValue5, Is.EqualTo(theCase.FieldValue5));
        Assert.That(caseVersions[1].FieldValue6, Is.EqualTo(theCase.FieldValue6));
        Assert.That(caseVersions[1].FieldValue7, Is.EqualTo(theCase.FieldValue7));
        Assert.That(caseVersions[1].FieldValue8, Is.EqualTo(theCase.FieldValue8));
        Assert.That(caseVersions[1].FieldValue9, Is.EqualTo(theCase.FieldValue9));
        Assert.That(caseVersions[1].FieldValue10, Is.EqualTo(theCase.FieldValue10));
        Assert.That(caseVersions[1].MicrotingUid, Is.EqualTo(theCase.MicrotingUid));
        Assert.That(caseVersions[1].MicrotingCheckUid, Is.EqualTo(theCase.MicrotingCheckUid));
    }
}