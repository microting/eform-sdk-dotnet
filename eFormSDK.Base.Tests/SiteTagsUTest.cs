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
    public class SiteTagsUTest : DbTestFixture
    {
        [Test]
        public async Task SiteTags_Create_DoesCreate()
        {
            // Arrange
            Random rnd = new Random();

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Tag tag = new Tag
            {
                Name = Guid.NewGuid().ToString(),
                TaggingsCount = rnd.Next(1, 255)
            };
            await tag.Create(DbContext).ConfigureAwait(false);

            SiteTag siteTag = new SiteTag
            {
                SiteId = site.Id,
                TagId = tag.Id
            };

            // Act
            await siteTag.Create(DbContext).ConfigureAwait(false);


            List<SiteTag> siteTags = DbContext.SiteTags.AsNoTracking().ToList();
            List<SiteTagVersion> siteTagVersions = DbContext.SiteTagVersions.AsNoTracking().ToList();
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

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Site site2 = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site2.Create(DbContext).ConfigureAwait(false);


            Tag tag = new Tag
            {
                Name = Guid.NewGuid().ToString(),
                TaggingsCount = rnd.Next(1, 255)
            };
            await tag.Create(DbContext).ConfigureAwait(false);

            Tag tag2 = new Tag
            {
                Name = Guid.NewGuid().ToString(),
                TaggingsCount = rnd.Next(1, 255)
            };
            await tag2.Create(DbContext).ConfigureAwait(false);


            SiteTag siteTag = new SiteTag
            {
                SiteId = site.Id,
                TagId = tag.Id
            };
            await siteTag.Create(DbContext).ConfigureAwait(false);

            int? oldSiteId = siteTag.SiteId;
            int? oldTagId = siteTag.TagId;

            siteTag.SiteId = site2.Id;
            siteTag.TagId = tag2.Id;

            // Act
            await siteTag.Update(DbContext).ConfigureAwait(false);

            List<SiteTag> siteTags = DbContext.SiteTags.AsNoTracking().ToList();
            List<SiteTagVersion> siteTagVersions = DbContext.SiteTagVersions.AsNoTracking().ToList();
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

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Tag tag = new Tag
            {
                Name = Guid.NewGuid().ToString(),
                TaggingsCount = rnd.Next(1, 255)
            };
            await tag.Create(DbContext).ConfigureAwait(false);


            SiteTag siteTag = new SiteTag
            {
                SiteId = site.Id,
                TagId = tag.Id
            };
            await siteTag.Create(DbContext).ConfigureAwait(false);

            // Act
            await siteTag.Delete(DbContext).ConfigureAwait(false);

            List<SiteTag> siteTags = DbContext.SiteTags.AsNoTracking().ToList();
            List<SiteTagVersion> siteTagVersions = DbContext.SiteTagVersions.AsNoTracking().ToList();
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

        [Test]
        public async Task SiteTags_MultipleCreateAndDelete()
        {
            Random rnd = new Random();

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            Tag tag = new Tag
            {
                Name = Guid.NewGuid().ToString(),
                TaggingsCount = rnd.Next(1, 255)
            };
            await tag.Create(DbContext).ConfigureAwait(false);


            SiteTag siteTag = new SiteTag
            {
                SiteId = site.Id,
                TagId = tag.Id
            };
            // Act
            await siteTag.Create(DbContext).ConfigureAwait(false);
            await siteTag.Delete(DbContext).ConfigureAwait(false);
            siteTag.WorkflowState = Constants.WorkflowStates.Created;
            await siteTag.Update(DbContext).ConfigureAwait(false);
            await siteTag.Delete(DbContext).ConfigureAwait(false);

            List<SiteTag> siteTags = DbContext.SiteTags.AsNoTracking().ToList();
            List<SiteTagVersion> siteTagVersions = DbContext.SiteTagVersions.AsNoTracking().ToList();
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

            Assert.AreEqual(siteTag.SiteId, siteTagVersions[2].SiteId);
            Assert.AreEqual(siteTag.TagId, siteTagVersions[2].TagId);
            Assert.AreEqual(siteTagVersions[2].WorkflowState, Constants.WorkflowStates.Created);

            Assert.AreEqual(siteTag.SiteId, siteTagVersions[3].SiteId);
            Assert.AreEqual(siteTag.TagId, siteTagVersions[3].TagId);
            Assert.AreEqual(siteTagVersions[3].WorkflowState, Constants.WorkflowStates.Removed);
        }
    }
}