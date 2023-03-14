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
    public class SitesUTest : DbTestFixture
    {
        [Test]
        public async Task Site_Create_DoesCreate()
        {
            //Arrange
            Random rnd = new Random();

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };

            //Act

            await site.Create(DbContext).ConfigureAwait(false);

            Site dbSite = DbContext.Sites.AsNoTracking().First();
            List<Site> sitesList = DbContext.Sites.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(dbSite);
            Assert.NotNull(dbSite.Id);

            Assert.AreEqual(1, sitesList.Count());
            Assert.AreEqual(site.CreatedAt.ToString(), dbSite.CreatedAt.ToString());
            Assert.AreEqual(site.Version, dbSite.Version);
//            Assert.AreEqual(site.UpdatedAt.ToString(), dbSites.UpdatedAt.ToString());
            Assert.AreEqual(dbSite.WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(site.Name, dbSite.Name);
            Assert.AreEqual(site.MicrotingUid, dbSite.MicrotingUid);
        }

        [Test]
        public async Task Sites_Update_DoesUpdate()
        {
            //Arrange

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString()
            };

            DbContext.Sites.Add(site);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            //Act

            site.Name = Guid.NewGuid().ToString();
            await site.Update(DbContext).ConfigureAwait(false);

            Site dbSite = DbContext.Sites.AsNoTracking().First();
            List<Site> sitesList = DbContext.Sites.AsNoTracking().ToList();
            List<SiteVersion> sitesVersions = DbContext.SiteVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(dbSite);
            Assert.NotNull(dbSite.Id);

            Assert.AreEqual(1, sitesList.Count());
            Assert.AreEqual(1, sitesVersions.Count());

            Assert.AreEqual(site.CreatedAt.ToString(), dbSite.CreatedAt.ToString());
            Assert.AreEqual(site.Version, dbSite.Version);
//            Assert.AreEqual(site.UpdatedAt.ToString(), dbSites.UpdatedAt.ToString());
            Assert.AreEqual(site.Name, dbSite.Name);
        }

        [Test]
        public async Task Sites_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString()
            };

            DbContext.Sites.Add(site);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            //Act

            await site.Delete(DbContext);

            Site dbSite = DbContext.Sites.AsNoTracking().First();
            List<Site> sitesList = DbContext.Sites.AsNoTracking().ToList();
            List<SiteVersion> sitesVersions = DbContext.SiteVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(dbSite);
            Assert.NotNull(dbSite.Id);

            Assert.AreEqual(1, sitesList.Count());
            Assert.AreEqual(1, sitesVersions.Count());

            Assert.AreEqual(site.CreatedAt.ToString(), dbSite.CreatedAt.ToString());
            Assert.AreEqual(site.Version, dbSite.Version);
//            Assert.AreEqual(site.UpdatedAt.ToString(), dbSites.UpdatedAt.ToString());
            Assert.AreEqual(site.Name, dbSite.Name);

            Assert.AreEqual(dbSite.WorkflowState, Constants.WorkflowStates.Removed);
        }
    }
}