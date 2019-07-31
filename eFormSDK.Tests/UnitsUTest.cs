/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 Microting A/S

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
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class UnitsUTest : DbTestFixture
    {
        [Test]
        public void Units_Create_DoesCreate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);
            
            units unit = new units();
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);
            unit.Site = site;
            unit.SiteId = site.Id;
            
            //Act
            
            unit.Create(DbContext);

            List<units> units = DbContext.units.AsNoTracking().ToList();                            
            List<unit_versions> unitsVersions = DbContext.unit_versions.AsNoTracking().ToList(); 
            
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
            Assert.AreEqual(unit.UpdatedAt.ToString(), units[0].UpdatedAt.ToString());
            
            //Versions
            Assert.AreEqual(unit.CustomerNo, unitsVersions[0].CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, unitsVersions[0].MicrotingUid);
            Assert.AreEqual(unit.OtpCode, unitsVersions[0].OtpCode);
            Assert.AreEqual(site.Id, unitsVersions[0].SiteId);
            Assert.AreEqual(unitsVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(unit.CreatedAt.ToString(), unitsVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, unitsVersions[0].Version);            
            Assert.AreEqual(unit.Id, unitsVersions[0].Id);
            Assert.AreEqual(unit.UpdatedAt.ToString(), unitsVersions[0].UpdatedAt.ToString());
        }

        [Test]
        public void Units_Update_DoesUpdate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);
            
            units unit = new units();
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);
            unit.Site = site;
            unit.SiteId = site.Id;

            unit.Create(DbContext);
            
            //Act

            int? oldCustomerNo = unit.CustomerNo;
            int? oldMicrotingUid = unit.MicrotingUid;
            int? oldOtpCode = unit.OtpCode;
            int? oldSiteId = unit.SiteId;
            DateTime? oldUpdatedAt = unit.UpdatedAt;
            int? oldId = unit.Id;
            
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);

            unit.Update(DbContext);

            List<units> units = DbContext.units.AsNoTracking().ToList();                            
            List<unit_versions> unitsVersions = DbContext.unit_versions.AsNoTracking().ToList();
            
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
            Assert.AreEqual(unit.UpdatedAt.ToString(), units[0].UpdatedAt.ToString());
            Assert.AreEqual(unit.Id, units[0].Id);
            
            //Version 1 Old Version
            Assert.AreEqual(oldCustomerNo, unitsVersions[0].CustomerNo);
            Assert.AreEqual(oldMicrotingUid, unitsVersions[0].MicrotingUid);
            Assert.AreEqual(oldOtpCode, unitsVersions[0].OtpCode);
            Assert.AreEqual(site.Id, unitsVersions[0].SiteId);
            Assert.AreEqual(unit.CreatedAt.ToString(), unitsVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, unitsVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), unitsVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(oldId, unitsVersions[0].UnitId);
            
            //Version 2 Updated Version
            Assert.AreEqual(unit.CustomerNo, unitsVersions[1].CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, unitsVersions[1].MicrotingUid);
            Assert.AreEqual(unit.OtpCode, unitsVersions[1].OtpCode);
            Assert.AreEqual(site.Id, unitsVersions[1].SiteId);
            Assert.AreEqual(unit.CreatedAt.ToString(), unitsVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, unitsVersions[1].Version);                                      
            Assert.AreEqual(unit.UpdatedAt.ToString(), unitsVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(unit.Id, unitsVersions[1].UnitId);

        }

        [Test]
        public void Units_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange
            
            Random rnd = new Random();
            
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);

            units unit = new units();
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);
            unit.Site = site;
            unit.SiteId = site.Id;

            unit.Create(DbContext);
            
            //Act
            DateTime? oldUpdatedAt = unit.UpdatedAt;
            
            unit.Delete(DbContext);
            
            List<units> units = DbContext.units.AsNoTracking().ToList();                            
            List<unit_versions> unitsVersions = DbContext.unit_versions.AsNoTracking().ToList();
            
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
            Assert.AreEqual(unit.UpdatedAt.ToString(), units[0].UpdatedAt.ToString());
            Assert.AreEqual(unit.Id, units[0].Id);
            Assert.AreEqual(units[0].WorkflowState, Constants.WorkflowStates.Removed);
            
            //Version 1
            Assert.AreEqual(unit.CustomerNo, unitsVersions[0].CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, unitsVersions[0].MicrotingUid);
            Assert.AreEqual(unit.OtpCode, unitsVersions[0].OtpCode);
            Assert.AreEqual(site.Id, unitsVersions[0].SiteId);
            Assert.AreEqual(unit.CreatedAt.ToString(), unitsVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, unitsVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), unitsVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(unit.Id, unitsVersions[0].UnitId);
            
            Assert.AreEqual(unitsVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            
            //Version 2 Deleted Version
            Assert.AreEqual(unit.CustomerNo, unitsVersions[1].CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, unitsVersions[1].MicrotingUid);
            Assert.AreEqual(unit.OtpCode, unitsVersions[1].OtpCode);
            Assert.AreEqual(site.Id, unitsVersions[1].SiteId);
            Assert.AreEqual(unit.CreatedAt.ToString(), unitsVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, unitsVersions[1].Version);                                      
            Assert.AreEqual(unit.UpdatedAt.ToString(), unitsVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(unit.Id, unitsVersions[1].UnitId);
            
            Assert.AreEqual(unitsVersions[1].WorkflowState, Constants.WorkflowStates.Removed);

        }
    }
}