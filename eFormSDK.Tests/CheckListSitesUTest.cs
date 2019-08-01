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
    public class CheckListSitesUTest : DbTestFixture
    {
        [Test]
        public void CheckListSites_Create_DoesCreate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);
            
            check_lists checklist = new check_lists();
            checklist.Color = Guid.NewGuid().ToString();
            checklist.Custom = Guid.NewGuid().ToString();
            checklist.Description = Guid.NewGuid().ToString();
            checklist.Field1 = rnd.Next(1, 255);
            checklist.Field2 = rnd.Next(1, 255);
            checklist.Field4 = rnd.Next(1, 255);
            checklist.Field5 = rnd.Next(1, 255);
            checklist.Field6 = rnd.Next(1, 255);
            checklist.Field7 = rnd.Next(1, 255);
            checklist.Field8 = rnd.Next(1, 255);
            checklist.Field9 = rnd.Next(1, 255);
            checklist.Field10 = rnd.Next(1, 255);
            checklist.Label = Guid.NewGuid().ToString();
            checklist.Repeated = rnd.Next(1, 255);
            checklist.ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.CaseType = Guid.NewGuid().ToString();
            checklist.DisplayIndex = rnd.Next(1, 255);
            checklist.DownloadEntities = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.FastNavigation = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.FolderName = Guid.NewGuid().ToString();
            checklist.ManualSync = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.MultiApproval = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.OriginalId = Guid.NewGuid().ToString();
            checklist.ReviewEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.DocxExportEnabled = randomBool;
            checklist.DoneButtonEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.ExtraFieldsEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.JasperExportEnabled = randomBool;
            checklist.QuickSyncEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.Create(DbContext);

            check_list_sites checkListSite = new check_list_sites();
            checkListSite.MicrotingUid = Guid.NewGuid().ToString();
            checkListSite.SiteId = site.Id;
            checkListSite.CheckListId = checklist.Id;
            checkListSite.LastCheckId = Guid.NewGuid().ToString();
            
            //Act
            
            checkListSite.Create(DbContext);
            
            List<check_list_sites> checkListSites = DbContext.check_list_sites.AsNoTracking().ToList();
            List<check_list_site_versions> checkListSitesVersion = DbContext.check_list_site_versions.AsNoTracking().ToList();

            //Assert
            
            Assert.NotNull(checkListSites);                                                             
            Assert.NotNull(checkListSitesVersion);
            
            Assert.AreEqual(1,checkListSites.Count());  
            Assert.AreEqual(1,checkListSitesVersion.Count());
            
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSites[0].CreatedAt.ToString());
            Assert.AreEqual(checkListSite.Version, checkListSites[0].Version);
            Assert.AreEqual(checkListSite.UpdatedAt.ToString(), checkListSites[0].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Created, checkListSites[0].WorkflowState);
            Assert.AreEqual(checkListSite.Id, checkListSites[0].Id);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSites[0].MicrotingUid);
            Assert.AreEqual(checkListSite.SiteId, site.Id);
            Assert.AreEqual(checkListSite.CheckListId, checklist.Id);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSites[0].LastCheckId);
            
            //Versions
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSitesVersion[0].CreatedAt.ToString());
            Assert.AreEqual(checkListSite.Version, checkListSitesVersion[0].Version);
            Assert.AreEqual(checkListSite.UpdatedAt.ToString(), checkListSitesVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(checkListSite.WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListSite.Id, checkListSitesVersion[0].Id);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSitesVersion[0].MicrotingUid);
            Assert.AreEqual(site.Id, checkListSitesVersion[0].SiteId);
            Assert.AreEqual(checklist.Id, checkListSitesVersion[0].CheckListId);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSitesVersion[0].LastCheckId);
        }

        [Test]
        public void CheckListSites_Update_DoesUpdate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);
            
            check_lists checklist = new check_lists();
            checklist.Color = Guid.NewGuid().ToString();
            checklist.Custom = Guid.NewGuid().ToString();
            checklist.Description = Guid.NewGuid().ToString();
            checklist.Field1 = rnd.Next(1, 255);
            checklist.Field2 = rnd.Next(1, 255);
            checklist.Field4 = rnd.Next(1, 255);
            checklist.Field5 = rnd.Next(1, 255);
            checklist.Field6 = rnd.Next(1, 255);
            checklist.Field7 = rnd.Next(1, 255);
            checklist.Field8 = rnd.Next(1, 255);
            checklist.Field9 = rnd.Next(1, 255);
            checklist.Field10 = rnd.Next(1, 255);
            checklist.Label = Guid.NewGuid().ToString();
            checklist.Repeated = rnd.Next(1, 255);
            checklist.ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.CaseType = Guid.NewGuid().ToString();
            checklist.DisplayIndex = rnd.Next(1, 255);
            checklist.DownloadEntities = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.FastNavigation = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.FolderName = Guid.NewGuid().ToString();
            checklist.ManualSync = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.MultiApproval = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.OriginalId = Guid.NewGuid().ToString();
            checklist.ReviewEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.DocxExportEnabled = randomBool;
            checklist.DoneButtonEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.ExtraFieldsEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.JasperExportEnabled = randomBool;
            checklist.QuickSyncEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.Create(DbContext);

            check_list_sites checkListSite = new check_list_sites();
            checkListSite.MicrotingUid = Guid.NewGuid().ToString();
            checkListSite.SiteId = site.Id;
            checkListSite.CheckListId = checklist.Id;
            checkListSite.LastCheckId = Guid.NewGuid().ToString();
            checkListSite.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = checkListSite.UpdatedAt;
            string oldMicrotingUid = checkListSite.MicrotingUid;
            string oldLastCheckId = checkListSite.LastCheckId;

            checkListSite.MicrotingUid = Guid.NewGuid().ToString();
            checkListSite.LastCheckId = Guid.NewGuid().ToString();
            
            checkListSite.Update(DbContext);
            
            List<check_list_sites> checkListSites = DbContext.check_list_sites.AsNoTracking().ToList();
            List<check_list_site_versions> checkListSitesVersion = DbContext.check_list_site_versions.AsNoTracking().ToList();

            //Assert
            
            Assert.NotNull(checkListSites);                                                             
            Assert.NotNull(checkListSitesVersion);
            
            Assert.AreEqual(1,checkListSites.Count());  
            Assert.AreEqual(2,checkListSitesVersion.Count());
            
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSites[0].CreatedAt.ToString());
            Assert.AreEqual(checkListSite.Version, checkListSites[0].Version);
            Assert.AreEqual(checkListSite.UpdatedAt.ToString(), checkListSites[0].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Created, checkListSites[0].WorkflowState);
            Assert.AreEqual(checkListSite.Id, checkListSites[0].Id);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSites[0].MicrotingUid);
            Assert.AreEqual(checkListSite.SiteId, site.Id);
            Assert.AreEqual(checkListSite.CheckListId, checklist.Id);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSites[0].LastCheckId);
            
            //Old Version
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSitesVersion[0].CreatedAt.ToString());
            Assert.AreEqual(1, checkListSitesVersion[0].Version);
            Assert.AreEqual(oldUpdatedAt.ToString(), checkListSitesVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(checkListSite.WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListSite.Id, checkListSitesVersion[0].Id);
            Assert.AreEqual(oldMicrotingUid, checkListSitesVersion[0].MicrotingUid);
            Assert.AreEqual(site.Id, checkListSitesVersion[0].SiteId);
            Assert.AreEqual(checklist.Id, checkListSitesVersion[0].CheckListId);
            Assert.AreEqual(oldLastCheckId, checkListSitesVersion[0].LastCheckId);
            
            //New Version
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSitesVersion[1].CreatedAt.ToString());
            Assert.AreEqual(2, checkListSitesVersion[1].Version);
            Assert.AreEqual(checkListSite.UpdatedAt.ToString(), checkListSitesVersion[1].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Created, checkListSitesVersion[1].WorkflowState);
            Assert.AreEqual(checkListSite.Id, checkListSitesVersion[1].CheckListId);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSitesVersion[1].MicrotingUid);
            Assert.AreEqual(site.Id, checkListSitesVersion[1].SiteId);
            Assert.AreEqual(checklist.Id, checkListSitesVersion[1].CheckListId);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSitesVersion[1].LastCheckId);
        }

        [Test]
        public void CheckListSites_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange
            
            Random rnd = new Random();
            
            short shortMinValue = Int16.MinValue;
            short shortmaxValue = Int16.MaxValue;
            
            bool randomBool = rnd.Next(0, 2) > 0;
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);
            
            check_lists checklist = new check_lists();
            checklist.Color = Guid.NewGuid().ToString();
            checklist.Custom = Guid.NewGuid().ToString();
            checklist.Description = Guid.NewGuid().ToString();
            checklist.Field1 = rnd.Next(1, 255);
            checklist.Field2 = rnd.Next(1, 255);
            checklist.Field4 = rnd.Next(1, 255);
            checklist.Field5 = rnd.Next(1, 255);
            checklist.Field6 = rnd.Next(1, 255);
            checklist.Field7 = rnd.Next(1, 255);
            checklist.Field8 = rnd.Next(1, 255);
            checklist.Field9 = rnd.Next(1, 255);
            checklist.Field10 = rnd.Next(1, 255);
            checklist.Label = Guid.NewGuid().ToString();
            checklist.Repeated = rnd.Next(1, 255);
            checklist.ApprovalEnabled = (short)rnd.Next(shortMinValue, shortmaxValue);
            checklist.CaseType = Guid.NewGuid().ToString();
            checklist.DisplayIndex = rnd.Next(1, 255);
            checklist.DownloadEntities = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.FastNavigation = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.FolderName = Guid.NewGuid().ToString();
            checklist.ManualSync = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.MultiApproval = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.OriginalId = Guid.NewGuid().ToString();
            checklist.ReviewEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.DocxExportEnabled = randomBool;
            checklist.DoneButtonEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.ExtraFieldsEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.JasperExportEnabled = randomBool;
            checklist.QuickSyncEnabled = (short) rnd.Next(shortMinValue, shortmaxValue);
            checklist.Create(DbContext);

            check_list_sites checkListSite = new check_list_sites();
            checkListSite.MicrotingUid = Guid.NewGuid().ToString();
            checkListSite.SiteId = site.Id;
            checkListSite.CheckListId = checklist.Id;
            checkListSite.LastCheckId = Guid.NewGuid().ToString();
            checkListSite.Create(DbContext);
            
            //Act

            DateTime? oldUpdatedAt = checkListSite.UpdatedAt;

            checkListSite.Delete(DbContext);
            
            List<check_list_sites> checkListSites = DbContext.check_list_sites.AsNoTracking().ToList();
            List<check_list_site_versions> checkListSitesVersion = DbContext.check_list_site_versions.AsNoTracking().ToList();

            //Assert
            
            Assert.NotNull(checkListSites);                                                             
            Assert.NotNull(checkListSitesVersion);
            
            Assert.AreEqual(1,checkListSites.Count());  
            Assert.AreEqual(2,checkListSitesVersion.Count());
            
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSites[0].CreatedAt.ToString());
            Assert.AreEqual(checkListSite.Version, checkListSites[0].Version);
            Assert.AreEqual(checkListSite.UpdatedAt.ToString(), checkListSites[0].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Removed, checkListSites[0].WorkflowState);
            Assert.AreEqual(checkListSite.Id, checkListSites[0].Id);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSites[0].MicrotingUid);
            Assert.AreEqual(checkListSite.SiteId, site.Id);
            Assert.AreEqual(checkListSite.CheckListId, checklist.Id);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSites[0].LastCheckId);
            
            //Old Version
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSitesVersion[0].CreatedAt.ToString());
            Assert.AreEqual(1, checkListSitesVersion[0].Version);
            Assert.AreEqual(oldUpdatedAt.ToString(), checkListSitesVersion[0].UpdatedAt.ToString());
            Assert.AreEqual(checkListSitesVersion[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(checkListSite.Id, checkListSitesVersion[0].Id);
            Assert.AreEqual(site.Id, checkListSitesVersion[0].SiteId);
            Assert.AreEqual(checklist.Id, checkListSitesVersion[0].CheckListId);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSitesVersion[0].LastCheckId);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSitesVersion[0].MicrotingUid);
            
            //New Version
            Assert.AreEqual(checkListSite.CreatedAt.ToString(), checkListSitesVersion[1].CreatedAt.ToString());
            Assert.AreEqual(2, checkListSitesVersion[1].Version);
            Assert.AreEqual(checkListSite.UpdatedAt.ToString(), checkListSitesVersion[1].UpdatedAt.ToString());
            Assert.AreEqual(Constants.WorkflowStates.Removed, checkListSitesVersion[1].WorkflowState);
            Assert.AreEqual(checkListSite.Id, checkListSitesVersion[1].CheckListSiteId);
            Assert.AreEqual(checkListSite.MicrotingUid, checkListSitesVersion[1].MicrotingUid);
            Assert.AreEqual(site.Id, checkListSitesVersion[1].SiteId);
            Assert.AreEqual(checklist.Id, checkListSitesVersion[1].CheckListId);
            Assert.AreEqual(checkListSite.LastCheckId, checkListSitesVersion[1].LastCheckId);
        }
    }
}