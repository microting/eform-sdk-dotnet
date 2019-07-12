using System;
using NUnit.Framework;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestSiteSurveyConfiguration : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

        public override void DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            sql.SettingUpdate(Settings.firstRunDone, "true");
            sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(ConnectionString);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }
        [Test]
        public void siteSurveyConfiguration_Create_DoesCreate()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations();
            siteSurveyConfiguration.SiteId = site1.Id;
            siteSurveyConfiguration.SurveyConfigurationId = surveyConfiguration.Id;
            // Act
            siteSurveyConfiguration.Create(DbContext);

            site_survey_configurations dbSiteSurveyConfiguration =
                DbContext.site_survey_configurations.AsNoTracking().First();
            site_survey_configuration_versions dbSiteSurveyConfigurationVersion =
                DbContext.site_survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSiteSurveyConfiguration);
            Assert.NotNull(dbSiteSurveyConfigurationVersion);

            Assert.AreEqual(site1.Id, dbSiteSurveyConfiguration.SiteId);
            Assert.AreEqual(surveyConfiguration.Id, siteSurveyConfiguration.SurveyConfigurationId);
        }
        [Test]
        public void siteSurveyConfiguration_Update_DoesUpdate()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            sites site1 = testHelpers.CreateSite("SiteName1", 88);
            
            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations();
            siteSurveyConfiguration.SiteId = site1.Id;
            siteSurveyConfiguration.SurveyConfigurationId = surveyConfiguration.Id;
            siteSurveyConfiguration.Create(DbContext);
            // Act
            sites site2 = testHelpers.CreateSite("siteName2", 666);
            survey_configurations surveyConfiguration2 = new survey_configurations();
            surveyConfiguration2.Name = Guid.NewGuid().ToString();
            surveyConfiguration2.Stop = DateTime.Now;
            surveyConfiguration2.Start = DateTime.Now;
            surveyConfiguration2.TimeOut = rnd.Next(1, 255);
            surveyConfiguration2.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration2.Create(DbContext);

            siteSurveyConfiguration.SiteId = site2.Id;
            siteSurveyConfiguration.SurveyConfigurationId = surveyConfiguration2.Id;
            
            siteSurveyConfiguration.Update(DbContext);
            site_survey_configurations dbSiteSurveyConfiguration =
                DbContext.site_survey_configurations.AsNoTracking().First();
            site_survey_configuration_versions dbSiteSurveyConfigurationVersion =
                DbContext.site_survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSiteSurveyConfiguration);
            Assert.NotNull(dbSiteSurveyConfigurationVersion);

            Assert.AreEqual(site2.Id, dbSiteSurveyConfiguration.SiteId);
            Assert.AreEqual(surveyConfiguration2.Id, siteSurveyConfiguration.SurveyConfigurationId);
        }
        [Test]
        public void siteSurveyConfiguration_Delete_DoesDelete()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations();
            siteSurveyConfiguration.SiteId = site1.Id;
            siteSurveyConfiguration.SurveyConfigurationId = surveyConfiguration.Id;
            siteSurveyConfiguration.Create(DbContext);
            // Act
            siteSurveyConfiguration.Delete(DbContext);
            site_survey_configurations dbSiteSurveyConfiguration =
                DbContext.site_survey_configurations.AsNoTracking().First();
            site_survey_configuration_versions dbSiteSurveyConfigurationVersion =
                DbContext.site_survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSiteSurveyConfiguration);
            Assert.NotNull(dbSiteSurveyConfigurationVersion);

            Assert.AreEqual(site1.Id, dbSiteSurveyConfiguration.SiteId);
            Assert.AreEqual(surveyConfiguration.Id, siteSurveyConfiguration.SurveyConfigurationId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, siteSurveyConfiguration.WorkflowState);
        }
    }
}