using System;
using NUnit.Framework;
using eFormCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(ConnectionString);
            await sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            await sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            await sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            await sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }
        [Test]
        public async Task siteSurveyConfiguration_Create_DoesCreate()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration.Create(dbContext);

            sites site1 = await testHelpers.CreateSite("SiteName1", 88);

            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations();
            siteSurveyConfiguration.SiteId = site1.Id;
            siteSurveyConfiguration.SurveyConfigurationId = surveyConfiguration.Id;
            // Act
            await siteSurveyConfiguration.Create(dbContext);

            site_survey_configurations dbSiteSurveyConfiguration =
                dbContext.site_survey_configurations.AsNoTracking().First();
            site_survey_configuration_versions dbSiteSurveyConfigurationVersion =
                dbContext.site_survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSiteSurveyConfiguration);
            Assert.NotNull(dbSiteSurveyConfigurationVersion);

            Assert.AreEqual(site1.Id, dbSiteSurveyConfiguration.SiteId);
            Assert.AreEqual(surveyConfiguration.Id, siteSurveyConfiguration.SurveyConfigurationId);
        }
        [Test]
        public async Task siteSurveyConfiguration_Update_DoesUpdate()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration.Create(dbContext);

            sites site1 = await testHelpers.CreateSite("SiteName1", 88);
            
            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations();
            siteSurveyConfiguration.SiteId = site1.Id;
            siteSurveyConfiguration.SurveyConfigurationId = surveyConfiguration.Id;
            await siteSurveyConfiguration.Create(dbContext);
            // Act
            sites site2 = await testHelpers.CreateSite("siteName2", 666);
            survey_configurations surveyConfiguration2 = new survey_configurations();
            surveyConfiguration2.Name = Guid.NewGuid().ToString();
            surveyConfiguration2.Stop = DateTime.Now;
            surveyConfiguration2.Start = DateTime.Now;
            surveyConfiguration2.TimeOut = rnd.Next(1, 255);
            surveyConfiguration2.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration2.Create(dbContext);

            siteSurveyConfiguration.SiteId = site2.Id;
            siteSurveyConfiguration.SurveyConfigurationId = surveyConfiguration2.Id;
            
            await siteSurveyConfiguration.Update(dbContext);
            site_survey_configurations dbSiteSurveyConfiguration =
                dbContext.site_survey_configurations.AsNoTracking().First();
            site_survey_configuration_versions dbSiteSurveyConfigurationVersion =
                dbContext.site_survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSiteSurveyConfiguration);
            Assert.NotNull(dbSiteSurveyConfigurationVersion);

            Assert.AreEqual(site2.Id, dbSiteSurveyConfiguration.SiteId);
            Assert.AreEqual(surveyConfiguration2.Id, siteSurveyConfiguration.SurveyConfigurationId);
        }
        [Test]
        public async Task siteSurveyConfiguration_Delete_DoesDelete()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration.Create(dbContext);

            sites site1 = await testHelpers.CreateSite("SiteName1", 88);

            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations();
            siteSurveyConfiguration.SiteId = site1.Id;
            siteSurveyConfiguration.SurveyConfigurationId = surveyConfiguration.Id;
            await siteSurveyConfiguration.Create(dbContext);
            // Act
            await siteSurveyConfiguration.Delete(dbContext);
            site_survey_configurations dbSiteSurveyConfiguration =
                dbContext.site_survey_configurations.AsNoTracking().First();
            site_survey_configuration_versions dbSiteSurveyConfigurationVersion =
                dbContext.site_survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSiteSurveyConfiguration);
            Assert.NotNull(dbSiteSurveyConfigurationVersion);

            Assert.AreEqual(site1.Id, dbSiteSurveyConfiguration.SiteId);
            Assert.AreEqual(surveyConfiguration.Id, siteSurveyConfiguration.SurveyConfigurationId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, siteSurveyConfiguration.WorkflowState);
        }
    }
}