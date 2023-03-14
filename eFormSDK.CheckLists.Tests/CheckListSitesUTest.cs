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

namespace eFormSDK.CheckLists.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class CheckListSitesUTest : DbTestFixture
    {
        [Test]
        public async Task CheckListSites_Create_DoesCreate()
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

            CheckListSite checkListSite = new CheckListSite
            {
                MicrotingUid = rnd.Next(1, 255),
                SiteId = site.Id,
                CheckListId = checklist.Id,
                LastCheckId = rnd.Next(1, 255)
            };

            //Act

            await checkListSite.Create(DbContext).ConfigureAwait(false);

            List<CheckListSite> checkListSites = DbContext.CheckListSites.AsNoTracking().ToList();
            List<CheckListSiteVersion> checkListSitesVersion = DbContext.CheckListSiteVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(checkListSites);
            Assert.NotNull(checkListSitesVersion);

            Assert.AreEqual(1, checkListSites.Count());
            Assert.AreEqual(1, checkListSitesVersion.Count());

            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSites[0].CreatedAt.ToString());
            Assert.AreEqual(checkListSite.Version, checkListSites[0].Version);
//            Assert.AreEqual(checkListSite.UpdatedAt.ToString(), checkListSites[0].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Created, checkListSites[0].WorkflowState);
            Assert.AreEqual(checkListSite.Id, checkListSites[0].Id);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSites[0].MicrotingUid);
            Assert.AreEqual(checkListSite.SiteId, site.Id);
            Assert.AreEqual(checkListSite.CheckListId, checklist.Id);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSites[0].LastCheckId);

            //Versions
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSitesVersion[0].CreatedAt.ToString());
            Assert.AreEqual(checkListSite.Version, checkListSitesVersion[0].Version);
//            Assert.AreEqual(checkListSite.UpdatedAt.ToString(), checkListSitesVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(checkListSite.WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListSite.Id, checkListSitesVersion[0].Id);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSitesVersion[0].MicrotingUid);
            Assert.AreEqual(site.Id, checkListSitesVersion[0].SiteId);
            Assert.AreEqual(checklist.Id, checkListSitesVersion[0].CheckListId);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSitesVersion[0].LastCheckId);
        }

        [Test]
        public async Task CheckListSites_Update_DoesUpdate()
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

            CheckListSite checkListSite = new CheckListSite
            {
                MicrotingUid = rnd.Next(1, 255),
                SiteId = site.Id,
                CheckListId = checklist.Id,
                LastCheckId = rnd.Next(1, 255)
            };
            await checkListSite.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = checkListSite.UpdatedAt;
            int oldMicrotingUid = checkListSite.MicrotingUid;
            int oldLastCheckId = checkListSite.LastCheckId;

            checkListSite.MicrotingUid = rnd.Next(1, 255);
            checkListSite.LastCheckId = rnd.Next(1, 255);

            await checkListSite.Update(DbContext).ConfigureAwait(false);

            List<CheckListSite> checkListSites = DbContext.CheckListSites.AsNoTracking().ToList();
            List<CheckListSiteVersion> checkListSitesVersion = DbContext.CheckListSiteVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(checkListSites);
            Assert.NotNull(checkListSitesVersion);

            Assert.AreEqual(1, checkListSites.Count());
            Assert.AreEqual(2, checkListSitesVersion.Count());

            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSites[0].CreatedAt.ToString());
            Assert.AreEqual(checkListSite.Version, checkListSites[0].Version);
//            Assert.AreEqual(checkListSite.UpdatedAt.ToString(), checkListSites[0].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Created, checkListSites[0].WorkflowState);
            Assert.AreEqual(checkListSite.Id, checkListSites[0].Id);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSites[0].MicrotingUid);
            Assert.AreEqual(checkListSite.SiteId, site.Id);
            Assert.AreEqual(checkListSite.CheckListId, checklist.Id);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSites[0].LastCheckId);

            //Old Version
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSitesVersion[0].CreatedAt.ToString());
            Assert.AreEqual(1, checkListSitesVersion[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), checkListSitesVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(checkListSite.WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListSite.Id, checkListSitesVersion[0].Id);
            Assert.AreEqual(oldMicrotingUid, checkListSitesVersion[0].MicrotingUid);
            Assert.AreEqual(site.Id, checkListSitesVersion[0].SiteId);
            Assert.AreEqual(checklist.Id, checkListSitesVersion[0].CheckListId);
            Assert.AreEqual(oldLastCheckId, checkListSitesVersion[0].LastCheckId);

            //New Version
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSitesVersion[1].CreatedAt.ToString());
            Assert.AreEqual(2, checkListSitesVersion[1].Version);
//            Assert.AreEqual(checkListSite.UpdatedAt.ToString(), checkListSitesVersion[1].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Created, checkListSitesVersion[1].WorkflowState);
            Assert.AreEqual(checkListSite.Id, checkListSitesVersion[1].CheckListId);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSitesVersion[1].MicrotingUid);
            Assert.AreEqual(site.Id, checkListSitesVersion[1].SiteId);
            Assert.AreEqual(checklist.Id, checkListSitesVersion[1].CheckListId);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSitesVersion[1].LastCheckId);
        }

        [Test]
        public async Task CheckListSites_Delete_DoesSetWorkflowStateToRemoved()
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

            CheckListSite checkListSite = new CheckListSite
            {
                MicrotingUid = rnd.Next(1, 255),
                SiteId = site.Id,
                CheckListId = checklist.Id,
                LastCheckId = rnd.Next(1, 255)
            };
            await checkListSite.Create(DbContext).ConfigureAwait(false);

            //Act

            DateTime? oldUpdatedAt = checkListSite.UpdatedAt;

            await checkListSite.Delete(DbContext);

            List<CheckListSite> checkListSites = DbContext.CheckListSites.AsNoTracking().ToList();
            List<CheckListSiteVersion> checkListSitesVersion = DbContext.CheckListSiteVersions.AsNoTracking().ToList();

            //Assert

            Assert.NotNull(checkListSites);
            Assert.NotNull(checkListSitesVersion);

            Assert.AreEqual(1, checkListSites.Count());
            Assert.AreEqual(2, checkListSitesVersion.Count());

            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSites[0].CreatedAt.ToString());
            Assert.AreEqual(checkListSite.Version, checkListSites[0].Version);
//            Assert.AreEqual(checkListSite.UpdatedAt.ToString(), checkListSites[0].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Removed, checkListSites[0].WorkflowState);
            Assert.AreEqual(checkListSite.Id, checkListSites[0].Id);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSites[0].MicrotingUid);
            Assert.AreEqual(checkListSite.SiteId, site.Id);
            Assert.AreEqual(checkListSite.CheckListId, checklist.Id);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSites[0].LastCheckId);

            //Old Version
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSitesVersion[0].CreatedAt.ToString());
            Assert.AreEqual(1, checkListSitesVersion[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), checkListSitesVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(checkListSitesVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListSite.Id, checkListSitesVersion[0].Id);
            Assert.AreEqual(site.Id, checkListSitesVersion[0].SiteId);
            Assert.AreEqual(checklist.Id, checkListSitesVersion[0].CheckListId);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSitesVersion[0].LastCheckId);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSitesVersion[0].MicrotingUid);

            //New Version
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSitesVersion[1].CreatedAt.ToString());
            Assert.AreEqual(2, checkListSitesVersion[1].Version);
//            Assert.AreEqual(checkListSite.UpdatedAt.ToString(), checkListSitesVersion[1].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Removed, checkListSitesVersion[1].WorkflowState);
            Assert.AreEqual(checkListSite.Id, checkListSitesVersion[1].CheckListSiteId);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSitesVersion[1].MicrotingUid);
            Assert.AreEqual(site.Id, checkListSitesVersion[1].SiteId);
            Assert.AreEqual(checklist.Id, checkListSitesVersion[1].CheckListId);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSitesVersion[1].LastCheckId);
        }
    }
}