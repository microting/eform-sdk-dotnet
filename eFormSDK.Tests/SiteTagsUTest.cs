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


namespace eFormSDK.Tests
{
    [TestFixture]
    public class SiteTagsUTest : DbTestFixture
    {
        [Test]
        public async Task SiteTags_Create_DoesCreate()
        {
            // Arrange
            Random rnd = new Random();

            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            await site.Create(dbContext);
            
            tags tag = new tags();
            tag.Name = Guid.NewGuid().ToString();
            tag.TaggingsCount = rnd.Next(1, 255);
            await tag.Create(dbContext);


            site_tags siteTag = new site_tags
            {
                SiteId = site.Id,
                TagId = tag.Id
            };

            // Act
            await siteTag.Create(dbContext);


            List<site_tags> siteTags = dbContext.SiteTags.AsNoTracking().ToList();
            List<site_tag_versions> siteTagVersions = dbContext.SiteTagVersions.AsNoTracking().ToList();
            // Assert
            Assert.NotNull(siteTags);
            Assert.NotNull(siteTagVersions);

            Assert.AreEqual(siteTag.SiteId, siteTags[0].SiteId);
            Assert.AreEqual(siteTag.TagId, siteTags[0].TagId);
            
            
            Assert.AreEqual(siteTag.SiteId, siteTagVersions[0].SiteId);
            Assert.AreEqual(siteTag.TagId, siteTagVersions[0].TagId);

        }

        [Test]
        public async Task SiteTags_Update_DoesUpdate()
        {
            // Arrange
            Random rnd = new Random();

            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            await site.Create(dbContext);
            
            sites site2 = new sites();
            site2.Name = Guid.NewGuid().ToString();
            site2.MicrotingUid = rnd.Next(1, 255);
            await site2.Create(dbContext);
            
            
            tags tag = new tags();
            tag.Name = Guid.NewGuid().ToString();
            tag.TaggingsCount = rnd.Next(1, 255);
            await tag.Create(dbContext);
          
            tags tag2 = new tags();
            tag2.Name = Guid.NewGuid().ToString();
            tag2.TaggingsCount = rnd.Next(1, 255);
            await tag2.Create(dbContext);

            

            site_tags siteTag = new site_tags
            {
                SiteId = site.Id,
                TagId = tag.Id
            };
            await siteTag.Create(dbContext);

            int? oldSiteId = siteTag.SiteId;
            int? oldTagId = siteTag.TagId;

            siteTag.SiteId = site2.Id;
            siteTag.TagId = tag2.Id;
            
            // Act
            await siteTag.Update(dbContext);

            List<site_tags> siteTags = dbContext.SiteTags.AsNoTracking().ToList();
            List<site_tag_versions> siteTagVersions = dbContext.SiteTagVersions.AsNoTracking().ToList();
            // Assert
            Assert.NotNull(siteTags);
            Assert.NotNull(siteTagVersions);

            Assert.AreEqual(siteTag.SiteId, siteTags[0].SiteId);
            Assert.AreEqual(siteTag.TagId, siteTags[0].TagId);
            
            
            Assert.AreEqual(oldSiteId, siteTagVersions[0].SiteId);
            Assert.AreEqual(oldTagId, siteTagVersions[0].TagId);

            Assert.AreEqual(siteTag.SiteId, siteTagVersions[1].SiteId);
            Assert.AreEqual(siteTag.TagId, siteTagVersions[1].TagId);
        }

        [Test]
        public async Task SiteTags_Delete_DoesDelete()
        {
                        // Arrange
            Random rnd = new Random();

            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            await site.Create(dbContext);

            tags tag = new tags();
            tag.Name = Guid.NewGuid().ToString();
            tag.TaggingsCount = rnd.Next(1, 255);
            await tag.Create(dbContext);


            site_tags siteTag = new site_tags
            {
                SiteId = site.Id,
                TagId = tag.Id
            };
            await siteTag.Create(dbContext);

            // Act
            await siteTag.Delete(dbContext);

            List<site_tags> siteTags = dbContext.SiteTags.AsNoTracking().ToList();
            List<site_tag_versions> siteTagVersions = dbContext.SiteTagVersions.AsNoTracking().ToList();
            // Assert
            Assert.NotNull(siteTags);
            Assert.NotNull(siteTagVersions);
            
            Assert.AreEqual(siteTag.SiteId, siteTags[0].SiteId);
            Assert.AreEqual(siteTag.TagId, siteTags[0].TagId);
            Assert.AreEqual(siteTag.WorkflowState, Constants.WorkflowStates.Removed);
            
            Assert.AreEqual(siteTag.SiteId, siteTagVersions[0].SiteId);
            Assert.AreEqual(siteTag.TagId, siteTagVersions[0].TagId);
            Assert.AreEqual(siteTagVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            
            Assert.AreEqual(siteTag.SiteId, siteTagVersions[1].SiteId);
            Assert.AreEqual(siteTag.TagId, siteTagVersions[1].TagId);
            Assert.AreEqual(siteTagVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            
        }
    }
}