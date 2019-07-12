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

            units dbUnit = DbContext.units.AsNoTracking().First();
            List<units> unitsList = DbContext.units.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbUnit);
            Assert.NotNull(dbUnit.Id);
            
            Assert.AreEqual(1, unitsList.Count());
            Assert.AreEqual(unit.CustomerNo, dbUnit.CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, dbUnit.MicrotingUid);
            Assert.AreEqual(unit.OtpCode, dbUnit.OtpCode);
            Assert.AreEqual(unit.SiteId, dbUnit.SiteId);
            Assert.AreEqual(dbUnit.WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(unit.CreatedAt.ToString(), dbUnit.CreatedAt.ToString());                                  
            Assert.AreEqual(unit.Version, dbUnit.Version);            
            Assert.AreEqual(unit.Id, dbUnit.Id);
            Assert.AreEqual(unit.UpdatedAt.ToString(), dbUnit.UpdatedAt.ToString());
        }

        [Test]
        public void Units_Update_DoesUpdate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            DbContext.sites.Add(site);
            DbContext.SaveChanges();
            
            units unit = new units();
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);
            unit.Site = site;
            unit.SiteId = site.Id;

            DbContext.units.Add(unit);
            DbContext.SaveChanges();
            
            //Act

            sites newSite = new sites();
            newSite.Name = Guid.NewGuid().ToString();
            newSite.MicrotingUid = rnd.Next(1, 255);
            DbContext.sites.Add(newSite);
            DbContext.SaveChanges();

            unit.Site = newSite;
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);
            unit.Site = site;
            unit.SiteId = site.Id;
            
            unit.Update(DbContext);

            units dbUnit = DbContext.units.AsNoTracking().First();
            List<units> unitsList = DbContext.units.AsNoTracking().ToList();
            List<unit_versions> unitsVersions = DbContext.unit_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbUnit);
            Assert.NotNull(dbUnit.Id);
            
            Assert.AreEqual(1, unitsList.Count());
            Assert.AreEqual(1, unitsVersions.Count());
            
            Assert.AreEqual(unit.CustomerNo, dbUnit.CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, dbUnit.MicrotingUid);
            Assert.AreEqual(unit.OtpCode, dbUnit.OtpCode);
            Assert.AreEqual(unit.SiteId, dbUnit.SiteId);
            Assert.AreEqual(unit.CreatedAt.ToString(), dbUnit.CreatedAt.ToString());                                  
            Assert.AreEqual(unit.Version, dbUnit.Version);                                      
            Assert.AreEqual(unit.UpdatedAt.ToString(), dbUnit.UpdatedAt.ToString());
            Assert.AreEqual(unit.Id, dbUnit.Id);

        }

        [Test]
        public void Units_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange
            
            Random rnd = new Random();
            
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            DbContext.sites.Add(site);
            DbContext.SaveChanges();
            
            units unit = new units();
            unit.CustomerNo = rnd.Next(1, 255);
            unit.MicrotingUid = rnd.Next(1, 255);
            unit.OtpCode = rnd.Next(1, 255);
            unit.Site = site;
            unit.SiteId = site.Id;

            DbContext.units.Add(unit);
            DbContext.SaveChanges();
            
            //Act
            
            unit.Delete(DbContext);
            
            units dbUnit = DbContext.units.AsNoTracking().First();
            List<units> unitsList = DbContext.units.AsNoTracking().ToList();
            List<unit_versions> unitsVersions = DbContext.unit_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbUnit);
            Assert.NotNull(dbUnit.Id);
            
            Assert.AreEqual(1, unitsList.Count());
            Assert.AreEqual(1, unitsVersions.Count());
            
            Assert.AreEqual(unit.CustomerNo, dbUnit.CustomerNo);
            Assert.AreEqual(unit.MicrotingUid, dbUnit.MicrotingUid);
            Assert.AreEqual(unit.OtpCode, dbUnit.OtpCode);
            Assert.AreEqual(unit.SiteId, dbUnit.SiteId);
            Assert.AreEqual(unit.CreatedAt.ToString(), dbUnit.CreatedAt.ToString());                                  
            Assert.AreEqual(unit.Version, dbUnit.Version);                                      
            Assert.AreEqual(unit.UpdatedAt.ToString(), dbUnit.UpdatedAt.ToString());
            Assert.AreEqual(unit.Id, dbUnit.Id);
            Assert.AreEqual(dbUnit.WorkflowState, Constants.WorkflowStates.Removed);
        }
    }
}