using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Runtime.Internal.Util;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace eFormSDK.Tests
{
    [TestFixture]
    public class SitesUTest : DbTestFixture
    {
        [Test]
        public void Site_Create_DoesCreate()
        {
            //Arrange
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            
            //Act
            
            site.Create(DbContext);

            sites dbSites = DbContext.sites.AsNoTracking().First();
            List<sites> sitesList = DbContext.sites.AsNoTracking().ToList();
            
            //Assert
            
            Assert.NotNull(dbSites);
            Assert.NotNull(dbSites.Id);
            
            Assert.AreEqual(1, sitesList.Count());
            Assert.AreEqual(site.CreatedAt.ToString(), dbSites.CreatedAt.ToString());
            Assert.AreEqual(site.Version, dbSites.Version);
            Assert.AreEqual(site.UpdatedAt.ToString(), dbSites.UpdatedAt.ToString()); 
            Assert.AreEqual(dbSites.WorkflowState, eFormShared.Constants.WorkflowStates.Created);
            Assert.AreEqual(site.Name, dbSites.Name);
        }

        [Test]
        public void Sites_Update_DoesUpdate()
        {
            //Arrange
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();

            DbContext.sites.Add(site);
            DbContext.SaveChanges();
            
            //Act

            site.Name = Guid.NewGuid().ToString();
            site.Update(DbContext);

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
            Assert.AreEqual(site.UpdatedAt.ToString(), dbSites.UpdatedAt.ToString());
            Assert.AreEqual(site.Name, dbSites.Name);
        }

        [Test]
        public void Sites_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange

            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();

            DbContext.sites.Add(site);
            DbContext.SaveChanges();

            //Act

            site.Delete(DbContext);

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
            Assert.AreEqual(site.UpdatedAt.ToString(), dbSites.UpdatedAt.ToString());
            Assert.AreEqual(site.Name, dbSites.Name);    

            Assert.AreEqual(dbSites.WorkflowState, eFormShared.Constants.WorkflowStates.Removed);
        }
    }
}