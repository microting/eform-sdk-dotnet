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

namespace eFormSDK.Base.Tests
{
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

            Assert.NotNull(units);
            Assert.NotNull(unitsVersions);

            Assert.AreEqual(1, units.Count());
            Assert.AreEqual(1, unitsVersions.Count());


            Assert.AreEqual(unit.CustomerNo, units[0].CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, units[0].MicrotingUid);
            Assert.AreEqual(unit.OtpCode, units[0].OtpCode);
            Assert.AreEqual(unit.SiteId, site.Id);
            Assert.AreEqual(units[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(unit.CreatedAt.ToString(), units[0].CreatedAt.ToString());
            Assert.AreEqual(unit.Version, units[0].Version);
            Assert.AreEqual(unit.Id, units[0].Id);
//            Assert.AreEqual(unit.UpdatedAt.ToString(), units[0].UpdatedAt.ToString());
            Assert.AreEqual(unit.Model, units[0].Model);
            Assert.AreEqual(unit.Manufacturer, units[0].Manufacturer);
            Assert.AreEqual(unit.eFormVersion, units[0].eFormVersion);
            Assert.AreEqual(unit.InSightVersion, units[0].InSightVersion);
            Assert.AreEqual(unit.Note, units[0].Note);


            //Versions
            Assert.AreEqual(unit.CustomerNo, unitsVersions[0].CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, unitsVersions[0].MicrotingUid);
            Assert.AreEqual(unit.OtpCode, unitsVersions[0].OtpCode);
            Assert.AreEqual(site.Id, unitsVersions[0].SiteId);
            Assert.AreEqual(unitsVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(unit.CreatedAt.ToString(), unitsVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, unitsVersions[0].Version);
            Assert.AreEqual(unit.Id, unitsVersions[0].Id);
//            Assert.AreEqual(unit.UpdatedAt.ToString(), unitsVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(unit.Model, unitsVersions[0].Model);
            Assert.AreEqual(unit.Manufacturer, unitsVersions[0].Manufacturer);
            Assert.AreEqual(unit.eFormVersion, unitsVersions[0].eFormVersion);
            Assert.AreEqual(unit.InSightVersion, unitsVersions[0].InSightVersion);
            Assert.AreEqual(unit.Note, unitsVersions[0].Note);
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

            Assert.NotNull(units);
            Assert.NotNull(unitsVersions);

            Assert.AreEqual(1, units.Count());
            Assert.AreEqual(2, unitsVersions.Count());

            Assert.AreEqual(unit.CustomerNo, units[0].CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, units[0].MicrotingUid);
            Assert.AreEqual(unit.OtpCode, units[0].OtpCode);
            Assert.AreEqual(unit.SiteId, site.Id);
            Assert.AreEqual(unit.CreatedAt.ToString(), units[0].CreatedAt.ToString());
            Assert.AreEqual(unit.Version, units[0].Version);
//            Assert.AreEqual(unit.UpdatedAt.ToString(), units[0].UpdatedAt.ToString());
            Assert.AreEqual(unit.Id, units[0].Id);
            Assert.AreEqual(unit.Model, units[0].Model);
            Assert.AreEqual(unit.Manufacturer, units[0].Manufacturer);
            Assert.AreEqual(unit.eFormVersion, units[0].eFormVersion);
            Assert.AreEqual(unit.InSightVersion, units[0].InSightVersion);
            Assert.AreEqual(unit.Note, units[0].Note);
            //Version 1 Old Version
            Assert.AreEqual(oldCustomerNo, unitsVersions[0].CustomerNo);
            Assert.AreEqual(oldMicrotingUid, unitsVersions[0].MicrotingUid);
            Assert.AreEqual(oldOtpCode, unitsVersions[0].OtpCode);
            Assert.AreEqual(site.Id, unitsVersions[0].SiteId);
            Assert.AreEqual(unit.CreatedAt.ToString(), unitsVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, unitsVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), unitsVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(oldId, unitsVersions[0].UnitId);
            Assert.AreEqual(oldModel, unitsVersions[0].Model);
            Assert.AreEqual(oldManufacturer, unitsVersions[0].Manufacturer);
            Assert.AreEqual(unitEFormVersion, unitsVersions[0].eFormVersion);
            Assert.AreEqual(unitInSightVersion, unitsVersions[0].InSightVersion);
            Assert.AreEqual(oldNote, unitsVersions[0].Note);

            //Version 2 Updated Version
            Assert.AreEqual(unit.CustomerNo, unitsVersions[1].CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, unitsVersions[1].MicrotingUid);
            Assert.AreEqual(unit.OtpCode, unitsVersions[1].OtpCode);
            Assert.AreEqual(site.Id, unitsVersions[1].SiteId);
            Assert.AreEqual(unit.CreatedAt.ToString(), unitsVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, unitsVersions[1].Version);
//            Assert.AreEqual(unit.UpdatedAt.ToString(), unitsVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(unit.Id, unitsVersions[1].UnitId);
            Assert.AreEqual(unit.Model, unitsVersions[1].Model);
            Assert.AreEqual(unit.Manufacturer, unitsVersions[1].Manufacturer);
            Assert.AreEqual(unit.eFormVersion, unitsVersions[1].eFormVersion);
            Assert.AreEqual(unit.InSightVersion, unitsVersions[1].InSightVersion);
            Assert.AreEqual(unit.Note, unitsVersions[1].Note);
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

            Assert.NotNull(units);
            Assert.NotNull(unitsVersions);

            Assert.AreEqual(1, units.Count());
            Assert.AreEqual(2, unitsVersions.Count());

            Assert.AreEqual(unit.CustomerNo, units[0].CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, units[0].MicrotingUid);
            Assert.AreEqual(unit.OtpCode, units[0].OtpCode);
            Assert.AreEqual(unit.SiteId, site.Id);
            Assert.AreEqual(unit.CreatedAt.ToString(), units[0].CreatedAt.ToString());
            Assert.AreEqual(unit.Version, units[0].Version);
//            Assert.AreEqual(unit.UpdatedAt.ToString(), units[0].UpdatedAt.ToString());
            Assert.AreEqual(unit.Id, units[0].Id);
            Assert.AreEqual(units[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(unit.Model, units[0].Model);
            Assert.AreEqual(unit.Manufacturer, units[0].Manufacturer);
            Assert.AreEqual(unit.eFormVersion, units[0].eFormVersion);
            Assert.AreEqual(unit.InSightVersion, units[0].InSightVersion);
            Assert.AreEqual(unit.Note, units[0].Note);

            //Version 1
            Assert.AreEqual(unit.CustomerNo, unitsVersions[0].CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, unitsVersions[0].MicrotingUid);
            Assert.AreEqual(unit.OtpCode, unitsVersions[0].OtpCode);
            Assert.AreEqual(site.Id, unitsVersions[0].SiteId);
            Assert.AreEqual(unit.CreatedAt.ToString(), unitsVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, unitsVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), unitsVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(unit.Id, unitsVersions[0].UnitId);
            Assert.AreEqual(unit.Model, unitsVersions[0].Model);
            Assert.AreEqual(unit.Manufacturer, unitsVersions[0].Manufacturer);
            Assert.AreEqual(unit.eFormVersion, unitsVersions[0].eFormVersion);
            Assert.AreEqual(unit.InSightVersion, unitsVersions[0].InSightVersion);
            Assert.AreEqual(unit.Note, unitsVersions[0].Note);
            Assert.AreEqual(unitsVersions[0].WorkflowState, Constants.WorkflowStates.Created);

            //Version 2 Deleted Version
            Assert.AreEqual(unit.CustomerNo, unitsVersions[1].CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, unitsVersions[1].MicrotingUid);
            Assert.AreEqual(unit.OtpCode, unitsVersions[1].OtpCode);
            Assert.AreEqual(site.Id, unitsVersions[1].SiteId);
            Assert.AreEqual(unit.CreatedAt.ToString(), unitsVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, unitsVersions[1].Version);
//            Assert.AreEqual(unit.UpdatedAt.ToString(), unitsVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(unit.Id, unitsVersions[1].UnitId);
            Assert.AreEqual(unit.Model, unitsVersions[1].Model);
            Assert.AreEqual(unit.Manufacturer, unitsVersions[1].Manufacturer);
            Assert.AreEqual(unit.eFormVersion, unitsVersions[1].eFormVersion);
            Assert.AreEqual(unit.InSightVersion, unitsVersions[1].InSightVersion);
            Assert.AreEqual(unit.Note, unitsVersions[1].Note);
            Assert.AreEqual(unitsVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
        }
    }
}