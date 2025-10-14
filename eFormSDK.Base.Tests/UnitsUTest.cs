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

namespace eFormSDK.Base.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class UnitsUTest : DbTestFixture
{
    [Test]
    public async Task Units_Create_DoesCreate()
    {
        //Arrange

        Random rnd = new Random();


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
            Site = site,
            SiteId = site.Id,
            Manufacturer = Guid.NewGuid().ToString(),
            Model = Guid.NewGuid().ToString(),
            Note = Guid.NewGuid().ToString(),
            eFormVersion = Guid.NewGuid().ToString(),
            InSightVersion = Guid.NewGuid().ToString()
        };

        //Act

        await unit.Create(DbContext).ConfigureAwait(false);

        List<Unit> units = DbContext.Units.AsNoTracking().ToList();
        List<UnitVersion> unitsVersions = DbContext.UnitVersions.AsNoTracking().ToList();

        //Assert

        Assert.That(units, Is.Not.EqualTo(null));
        Assert.That(unitsVersions, Is.Not.EqualTo(null));

        Assert.That(units.Count(), Is.EqualTo(1));
        Assert.That(unitsVersions.Count(), Is.EqualTo(1));


        Assert.That(units[0].CustomerNo, Is.EqualTo(unit.CustomerNo));
        Assert.That(units[0].MicrotingUid, Is.EqualTo(unit.MicrotingUid));
        Assert.That(units[0].OtpCode, Is.EqualTo(unit.OtpCode));
        Assert.That(site.Id, Is.EqualTo(unit.SiteId));
        Assert.That(units[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(units[0].CreatedAt.ToString(), Is.EqualTo(unit.CreatedAt.ToString()));
        Assert.That(units[0].Version, Is.EqualTo(unit.Version));
        Assert.That(units[0].Id, Is.EqualTo(unit.Id));
        //            Assert.AreEqual(unit.UpdatedAt.ToString(), units[0].UpdatedAt.ToString());
        Assert.That(units[0].Model, Is.EqualTo(unit.Model));
        Assert.That(units[0].Manufacturer, Is.EqualTo(unit.Manufacturer));
        Assert.That(units[0].eFormVersion, Is.EqualTo(unit.eFormVersion));
        Assert.That(units[0].InSightVersion, Is.EqualTo(unit.InSightVersion));
        Assert.That(units[0].Note, Is.EqualTo(unit.Note));


        //Versions
        Assert.That(unitsVersions[0].CustomerNo, Is.EqualTo(unit.CustomerNo));
        Assert.That(unitsVersions[0].MicrotingUid, Is.EqualTo(unit.MicrotingUid));
        Assert.That(unitsVersions[0].OtpCode, Is.EqualTo(unit.OtpCode));
        Assert.That(unitsVersions[0].SiteId, Is.EqualTo(site.Id));
        Assert.That(unitsVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));
        Assert.That(unitsVersions[0].CreatedAt.ToString(), Is.EqualTo(unit.CreatedAt.ToString()));
        Assert.That(unitsVersions[0].Version, Is.EqualTo(1));
        Assert.That(unitsVersions[0].Id, Is.EqualTo(unit.Id));
        //            Assert.AreEqual(unit.UpdatedAt.ToString(), unitsVersions[0].UpdatedAt.ToString());
        Assert.That(unitsVersions[0].Model, Is.EqualTo(unit.Model));
        Assert.That(unitsVersions[0].Manufacturer, Is.EqualTo(unit.Manufacturer));
        Assert.That(unitsVersions[0].eFormVersion, Is.EqualTo(unit.eFormVersion));
        Assert.That(unitsVersions[0].InSightVersion, Is.EqualTo(unit.InSightVersion));
        Assert.That(unitsVersions[0].Note, Is.EqualTo(unit.Note));
    }

    [Test]
    public async Task Units_Update_DoesUpdate()
    {
        //Arrange

        Random rnd = new Random();


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
            Site = site,
            SiteId = site.Id,
            Manufacturer = Guid.NewGuid().ToString(),
            Model = Guid.NewGuid().ToString(),
            Note = Guid.NewGuid().ToString(),
            eFormVersion = Guid.NewGuid().ToString(),
            InSightVersion = Guid.NewGuid().ToString()
        };

        await unit.Create(DbContext).ConfigureAwait(false);

        //Act

        int? oldCustomerNo = unit.CustomerNo;
        int? oldMicrotingUid = unit.MicrotingUid;
        int? oldOtpCode = unit.OtpCode;
        int? oldSiteId = unit.SiteId;
        DateTime? oldUpdatedAt = unit.UpdatedAt;
        int? oldId = unit.Id;
        string oldManufacturer = unit.Manufacturer;
        string oldModel = unit.Model;
        string oldNote = unit.Note;
        string unitEFormVersion = unit.eFormVersion;
        string unitInSightVersion = unit.InSightVersion;


        unit.CustomerNo = rnd.Next(1, 255);
        unit.MicrotingUid = rnd.Next(1, 255);
        unit.OtpCode = rnd.Next(1, 255);

        await unit.Update(DbContext).ConfigureAwait(false);

        List<Unit> units = DbContext.Units.AsNoTracking().ToList();
        List<UnitVersion> unitsVersions = DbContext.UnitVersions.AsNoTracking().ToList();

        //Assert

        Assert.That(units, Is.Not.EqualTo(null));
        Assert.That(unitsVersions, Is.Not.EqualTo(null));

        Assert.That(units.Count(), Is.EqualTo(1));
        Assert.That(unitsVersions.Count(), Is.EqualTo(2));

        Assert.That(units[0].CustomerNo, Is.EqualTo(unit.CustomerNo));
        Assert.That(units[0].MicrotingUid, Is.EqualTo(unit.MicrotingUid));
        Assert.That(units[0].OtpCode, Is.EqualTo(unit.OtpCode));
        Assert.That(site.Id, Is.EqualTo(unit.SiteId));
        Assert.That(units[0].CreatedAt.ToString(), Is.EqualTo(unit.CreatedAt.ToString()));
        Assert.That(units[0].Version, Is.EqualTo(unit.Version));
        //            Assert.AreEqual(unit.UpdatedAt.ToString(), units[0].UpdatedAt.ToString());
        Assert.That(units[0].Id, Is.EqualTo(unit.Id));
        Assert.That(units[0].Model, Is.EqualTo(unit.Model));
        Assert.That(units[0].Manufacturer, Is.EqualTo(unit.Manufacturer));
        Assert.That(units[0].eFormVersion, Is.EqualTo(unit.eFormVersion));
        Assert.That(units[0].InSightVersion, Is.EqualTo(unit.InSightVersion));
        Assert.That(units[0].Note, Is.EqualTo(unit.Note));
        //Version 1 Old Version
        Assert.That(unitsVersions[0].CustomerNo, Is.EqualTo(oldCustomerNo));
        Assert.That(unitsVersions[0].MicrotingUid, Is.EqualTo(oldMicrotingUid));
        Assert.That(unitsVersions[0].OtpCode, Is.EqualTo(oldOtpCode));
        Assert.That(unitsVersions[0].SiteId, Is.EqualTo(site.Id));
        Assert.That(unitsVersions[0].CreatedAt.ToString(), Is.EqualTo(unit.CreatedAt.ToString()));
        Assert.That(unitsVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), unitsVersions[0].UpdatedAt.ToString());
        Assert.That(unitsVersions[0].UnitId, Is.EqualTo(oldId));
        Assert.That(unitsVersions[0].Model, Is.EqualTo(oldModel));
        Assert.That(unitsVersions[0].Manufacturer, Is.EqualTo(oldManufacturer));
        Assert.That(unitsVersions[0].eFormVersion, Is.EqualTo(unitEFormVersion));
        Assert.That(unitsVersions[0].InSightVersion, Is.EqualTo(unitInSightVersion));
        Assert.That(unitsVersions[0].Note, Is.EqualTo(oldNote));

        //Version 2 Updated Version
        Assert.That(unitsVersions[1].CustomerNo, Is.EqualTo(unit.CustomerNo));
        Assert.That(unitsVersions[1].MicrotingUid, Is.EqualTo(unit.MicrotingUid));
        Assert.That(unitsVersions[1].OtpCode, Is.EqualTo(unit.OtpCode));
        Assert.That(unitsVersions[1].SiteId, Is.EqualTo(site.Id));
        Assert.That(unitsVersions[1].CreatedAt.ToString(), Is.EqualTo(unit.CreatedAt.ToString()));
        Assert.That(unitsVersions[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(unit.UpdatedAt.ToString(), unitsVersions[1].UpdatedAt.ToString());
        Assert.That(unitsVersions[1].UnitId, Is.EqualTo(unit.Id));
        Assert.That(unitsVersions[1].Model, Is.EqualTo(unit.Model));
        Assert.That(unitsVersions[1].Manufacturer, Is.EqualTo(unit.Manufacturer));
        Assert.That(unitsVersions[1].eFormVersion, Is.EqualTo(unit.eFormVersion));
        Assert.That(unitsVersions[1].InSightVersion, Is.EqualTo(unit.InSightVersion));
        Assert.That(unitsVersions[1].Note, Is.EqualTo(unit.Note));
    }

    [Test]
    public async Task Units_Delete_DoesSetWorkflowStateToRemoved()
    {
        //Arrange

        Random rnd = new Random();


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
            Site = site,
            SiteId = site.Id,
            Manufacturer = Guid.NewGuid().ToString(),
            Model = Guid.NewGuid().ToString(),
            Note = Guid.NewGuid().ToString(),
            eFormVersion = Guid.NewGuid().ToString(),
            InSightVersion = Guid.NewGuid().ToString()
        };

        await unit.Create(DbContext).ConfigureAwait(false);

        //Act
        DateTime? oldUpdatedAt = unit.UpdatedAt;

        await unit.Delete(DbContext);

        List<Unit> units = DbContext.Units.AsNoTracking().ToList();
        List<UnitVersion> unitsVersions = DbContext.UnitVersions.AsNoTracking().ToList();

        //Assert

        Assert.That(units, Is.Not.EqualTo(null));
        Assert.That(unitsVersions, Is.Not.EqualTo(null));

        Assert.That(units.Count(), Is.EqualTo(1));
        Assert.That(unitsVersions.Count(), Is.EqualTo(2));

        Assert.That(units[0].CustomerNo, Is.EqualTo(unit.CustomerNo));
        Assert.That(units[0].MicrotingUid, Is.EqualTo(unit.MicrotingUid));
        Assert.That(units[0].OtpCode, Is.EqualTo(unit.OtpCode));
        Assert.That(site.Id, Is.EqualTo(unit.SiteId));
        Assert.That(units[0].CreatedAt.ToString(), Is.EqualTo(unit.CreatedAt.ToString()));
        Assert.That(units[0].Version, Is.EqualTo(unit.Version));
        //            Assert.AreEqual(unit.UpdatedAt.ToString(), units[0].UpdatedAt.ToString());
        Assert.That(units[0].Id, Is.EqualTo(unit.Id));
        Assert.That(units[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
        Assert.That(units[0].Model, Is.EqualTo(unit.Model));
        Assert.That(units[0].Manufacturer, Is.EqualTo(unit.Manufacturer));
        Assert.That(units[0].eFormVersion, Is.EqualTo(unit.eFormVersion));
        Assert.That(units[0].InSightVersion, Is.EqualTo(unit.InSightVersion));
        Assert.That(units[0].Note, Is.EqualTo(unit.Note));

        //Version 1
        Assert.That(unitsVersions[0].CustomerNo, Is.EqualTo(unit.CustomerNo));
        Assert.That(unitsVersions[0].MicrotingUid, Is.EqualTo(unit.MicrotingUid));
        Assert.That(unitsVersions[0].OtpCode, Is.EqualTo(unit.OtpCode));
        Assert.That(unitsVersions[0].SiteId, Is.EqualTo(site.Id));
        Assert.That(unitsVersions[0].CreatedAt.ToString(), Is.EqualTo(unit.CreatedAt.ToString()));
        Assert.That(unitsVersions[0].Version, Is.EqualTo(1));
        //            Assert.AreEqual(oldUpdatedAt.ToString(), unitsVersions[0].UpdatedAt.ToString());
        Assert.That(unitsVersions[0].UnitId, Is.EqualTo(unit.Id));
        Assert.That(unitsVersions[0].Model, Is.EqualTo(unit.Model));
        Assert.That(unitsVersions[0].Manufacturer, Is.EqualTo(unit.Manufacturer));
        Assert.That(unitsVersions[0].eFormVersion, Is.EqualTo(unit.eFormVersion));
        Assert.That(unitsVersions[0].InSightVersion, Is.EqualTo(unit.InSightVersion));
        Assert.That(unitsVersions[0].Note, Is.EqualTo(unit.Note));
        Assert.That(unitsVersions[0].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Created));

        //Version 2 Deleted Version
        Assert.That(unitsVersions[1].CustomerNo, Is.EqualTo(unit.CustomerNo));
        Assert.That(unitsVersions[1].MicrotingUid, Is.EqualTo(unit.MicrotingUid));
        Assert.That(unitsVersions[1].OtpCode, Is.EqualTo(unit.OtpCode));
        Assert.That(unitsVersions[1].SiteId, Is.EqualTo(site.Id));
        Assert.That(unitsVersions[1].CreatedAt.ToString(), Is.EqualTo(unit.CreatedAt.ToString()));
        Assert.That(unitsVersions[1].Version, Is.EqualTo(2));
        //            Assert.AreEqual(unit.UpdatedAt.ToString(), unitsVersions[1].UpdatedAt.ToString());
        Assert.That(unitsVersions[1].UnitId, Is.EqualTo(unit.Id));
        Assert.That(unitsVersions[1].Model, Is.EqualTo(unit.Model));
        Assert.That(unitsVersions[1].Manufacturer, Is.EqualTo(unit.Manufacturer));
        Assert.That(unitsVersions[1].eFormVersion, Is.EqualTo(unit.eFormVersion));
        Assert.That(unitsVersions[1].InSightVersion, Is.EqualTo(unit.InSightVersion));
        Assert.That(unitsVersions[1].Note, Is.EqualTo(unit.Note));
        Assert.That(unitsVersions[1].WorkflowState, Is.EqualTo(Constants.WorkflowStates.Removed));
    }
}