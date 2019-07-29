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
    public class SiteSurveyConfigurationsUTest : DbTestFixture
    {
        [Test]
        public void SiteSurveyConfigurations_Create_DoesCreate()
        {
            //Arrange
            
            Random rnd = new Random();
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations();
            siteSurveyConfiguration.SiteId = site.Id;
            siteSurveyConfiguration.SurveyConfigurationId = surveyConfiguration.Id;
            
            
            //Act
            
            siteSurveyConfiguration.Create(DbContext);
            
            List<site_survey_configurations> siteSurveyConfigurations = DbContext.site_survey_configurations.AsNoTracking().ToList();
            List<site_survey_configuration_versions> siteSurveyConfigurationVersions = DbContext.site_survey_configuration_versions.AsNoTracking().ToList();
            
            Assert.NotNull(siteSurveyConfigurations);                                                             
            Assert.NotNull(siteSurveyConfigurationVersions);                                                             

            Assert.AreEqual(1,siteSurveyConfigurations.Count());  
            Assert.AreEqual(1,siteSurveyConfigurationVersions.Count());  
            
            Assert.AreEqual(siteSurveyConfiguration.CreatedAt.ToString(), siteSurveyConfigurations[0].CreatedAt.ToString());                                  
            Assert.AreEqual(siteSurveyConfiguration.Version, siteSurveyConfigurations[0].Version);                                      
            Assert.AreEqual(siteSurveyConfiguration.UpdatedAt.ToString(), siteSurveyConfigurations[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteSurveyConfigurations[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteSurveyConfiguration.Id, siteSurveyConfigurations[0].Id);
            Assert.AreEqual(siteSurveyConfiguration.SiteId, siteSurveyConfigurations[0].SiteId);
            Assert.AreEqual(siteSurveyConfiguration.SurveyConfigurationId, siteSurveyConfigurations[0].SurveyConfigurationId);
            
            //Versions
            
            Assert.AreEqual(siteSurveyConfiguration.CreatedAt.ToString(), siteSurveyConfigurationVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(siteSurveyConfiguration.Version, siteSurveyConfigurationVersions[0].Version);                                      
            Assert.AreEqual(siteSurveyConfiguration.UpdatedAt.ToString(), siteSurveyConfigurationVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteSurveyConfigurationVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteSurveyConfiguration.Id, siteSurveyConfigurationVersions[0].Id);
            Assert.AreEqual(siteSurveyConfiguration.SiteId, siteSurveyConfigurationVersions[0].SiteId);
            Assert.AreEqual(siteSurveyConfiguration.SurveyConfigurationId, siteSurveyConfigurationVersions[0].SurveyConfigurationId);
        }

        [Test]
        public void SiteSurveyConfiguration_Delete_DoesDelete()
        {
            //Arrange
            
            Random rnd = new Random();
            
            sites site = new sites();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            site.Create(DbContext);
            
            survey_configurations surveyConfiguration = new survey_configurations();
            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations();
            siteSurveyConfiguration.SiteId = site.Id;
            siteSurveyConfiguration.SurveyConfigurationId = surveyConfiguration.Id;
            siteSurveyConfiguration.Create(DbContext);

            //Act

            DateTime? oldUpdatedAt = siteSurveyConfiguration.UpdatedAt;
            
            siteSurveyConfiguration.Delete(DbContext);

            
            List<site_survey_configurations> siteSurveyConfigurations = DbContext.site_survey_configurations.AsNoTracking().ToList();
            List<site_survey_configuration_versions> siteSurveyConfigurationVersions = DbContext.site_survey_configuration_versions.AsNoTracking().ToList();
            
            Assert.NotNull(siteSurveyConfigurations);                                                             
            Assert.NotNull(siteSurveyConfigurationVersions);                                                             

            Assert.AreEqual(1,siteSurveyConfigurations.Count());  
            Assert.AreEqual(2,siteSurveyConfigurationVersions.Count());  
            
            Assert.AreEqual(siteSurveyConfiguration.CreatedAt.ToString(), siteSurveyConfigurations[0].CreatedAt.ToString());                                  
            Assert.AreEqual(siteSurveyConfiguration.Version, siteSurveyConfigurations[0].Version);                                      
            Assert.AreEqual(siteSurveyConfiguration.UpdatedAt.ToString(), siteSurveyConfigurations[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteSurveyConfigurations[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(siteSurveyConfiguration.Id, siteSurveyConfigurations[0].Id);
            Assert.AreEqual(siteSurveyConfiguration.SiteId, siteSurveyConfigurations[0].SiteId);
            Assert.AreEqual(siteSurveyConfiguration.SurveyConfigurationId, siteSurveyConfigurations[0].SurveyConfigurationId);
            
            //Old Version
            
            Assert.AreEqual(siteSurveyConfiguration.CreatedAt.ToString(), siteSurveyConfigurationVersions[0].CreatedAt.ToString());                                  
            Assert.AreEqual(1, siteSurveyConfigurationVersions[0].Version);                                      
            Assert.AreEqual(oldUpdatedAt.ToString(), siteSurveyConfigurationVersions[0].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteSurveyConfigurationVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteSurveyConfiguration.Id, siteSurveyConfigurationVersions[0].SiteSurveyConfigurationId);
            Assert.AreEqual(siteSurveyConfiguration.SiteId, siteSurveyConfigurationVersions[0].SiteId);
            Assert.AreEqual(siteSurveyConfiguration.SurveyConfigurationId, siteSurveyConfigurationVersions[0].SurveyConfigurationId);
            
            //New Version
            
            Assert.AreEqual(siteSurveyConfiguration.CreatedAt.ToString(), siteSurveyConfigurationVersions[1].CreatedAt.ToString());                                  
            Assert.AreEqual(2, siteSurveyConfigurationVersions[1].Version);                                      
            Assert.AreEqual(siteSurveyConfiguration.UpdatedAt.ToString(), siteSurveyConfigurationVersions[1].UpdatedAt.ToString());                                  
            Assert.AreEqual(siteSurveyConfigurationVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(siteSurveyConfiguration.Id, siteSurveyConfigurationVersions[1].SiteSurveyConfigurationId);
            Assert.AreEqual(siteSurveyConfiguration.SiteId, siteSurveyConfigurationVersions[1].SiteId);
            Assert.AreEqual(siteSurveyConfiguration.SurveyConfigurationId, siteSurveyConfigurationVersions[1].SurveyConfigurationId);
        }
    }
}