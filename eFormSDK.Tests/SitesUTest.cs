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
using Amazon.Runtime.Internal.Util;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class SitesUTest : DbTestFixture
    {
        [Test]
        public async Task Site_Create_DoesCreate()
        {
            //Arrange
            Random rnd = new Random();
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            
            //Act
            
            await site.Create(dbContext);

            sites dbSites = dbContext.sites.AsNoTracking().First();
            List<sites> sitesList = dbContext.sites.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbSites);
            Assert.NotNull(dbSites.Id);
            
            Assert.AreEqual(1, sitesList.Count());
            Assert.AreEqual(site.CreatedAt.ToString(), dbSites.CreatedAt.ToString());
            Assert.AreEqual(site.Version, dbSites.Version);
//            Assert.AreEqual(site.UpdatedAt.ToString(), dbSites.UpdatedAt.ToString()); 
            Assert.AreEqual(dbSites.WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(site.Name, dbSites.Name);
            Assert.AreEqual(site.MicrotingUid, dbSites.MicrotingUid);
        }

        [Test]
        public async Task Sites_Update_DoesUpdate()
        {
            //Arrange
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();

            dbContext.sites.Add(site);
            await dbContext.SaveChangesAsync();
            
            //Act

            site.Name = Guid.NewGuid().ToString();
            await site.Update(dbContext);

            sites dbSites = dbContext.sites.AsNoTracking().First();
            List<sites> sitesList = dbContext.sites.AsNoTracking().ToList();
            List<site_versions> sitesVersions = dbContext.site_versions.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbSites);                                                             
            Assert.NotNull(dbSites.Id);                                                          

            Assert.AreEqual(1,sitesList.Count());
            Assert.AreEqual(1, sitesVersions.Count());

            Assert.AreEqual(site.CreatedAt.ToString(), dbSites.CreatedAt.ToString());                                  
            Assert.AreEqual(site.Version, dbSites.Version);                                      
//            Assert.AreEqual(site.UpdatedAt.ToString(), dbSites.UpdatedAt.ToString());
            Assert.AreEqual(site.Name, dbSites.Name);
        }

        [Test]
        public async Task Sites_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange

            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();

            dbContext.sites.Add(site);
            await dbContext.SaveChangesAsync();

            //Act

            await site.Delete(dbContext);

            sites dbSites = dbContext.sites.AsNoTracking().First();                               
            List<sites> sitesList = dbContext.sites.AsNoTracking().ToList();
            List<site_versions> sitesVersions = dbContext.site_versions.AsNoTracking().ToList();

            //Assert                                                                            

            Assert.NotNull(dbSites);                                                             
            Assert.NotNull(dbSites.Id);                                                          

            Assert.AreEqual(1,sitesList.Count());
            Assert.AreEqual(1, sitesVersions.Count());

            Assert.AreEqual(site.CreatedAt.ToString(), dbSites.CreatedAt.ToString());                                  
            Assert.AreEqual(site.Version, dbSites.Version);                                      
//            Assert.AreEqual(site.UpdatedAt.ToString(), dbSites.UpdatedAt.ToString());
            Assert.AreEqual(site.Name, dbSites.Name);    

            Assert.AreEqual(dbSites.WorkflowState, Constants.WorkflowStates.Removed);
        }
    }
}