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

            sites site = new sites
            {
                Name = Guid.NewGuid().ToString(), 
                MicrotingUid = rnd.Next(1, 255)
            };

            //Act
            
            await site.Create(DbContext).ConfigureAwait(false);

            sites dbSites = DbContext.sites.AsNoTracking().First();
            List<sites> sitesList = DbContext.sites.AsNoTracking().ToList();
            
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

            sites site = new sites
            {
                Name = Guid.NewGuid().ToString()
            };

            DbContext.sites.Add(site);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            
            //Act

            site.Name = Guid.NewGuid().ToString();
            await site.Update(DbContext).ConfigureAwait(false);

            sites dbSites = DbContext.sites.AsNoTracking().First();
            List<sites> sitesList = DbContext.sites.AsNoTracking().ToList();
            List<site_versions> sitesVersions = DbContext.site_versions.AsNoTracking().ToList();
            
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

            sites site = new sites
            {
                Name = Guid.NewGuid().ToString()
            };

            DbContext.sites.Add(site);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            //Act

            await site.Delete(DbContext);

            sites dbSites = DbContext.sites.AsNoTracking().First();                               
            List<sites> sitesList = DbContext.sites.AsNoTracking().ToList();
            List<site_versions> sitesVersions = DbContext.site_versions.AsNoTracking().ToList();

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