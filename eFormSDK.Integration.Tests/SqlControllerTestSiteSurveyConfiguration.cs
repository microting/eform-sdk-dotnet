using System;
using eFormSqlController;
using NUnit.Framework;
using eFormCore;
using eFormCore.Helpers;
using eFormData;
using eFormShared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestSiteSurveyConfiguration : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
        private string path;

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
            sut.SettingUpdate(Settings.fileLocationPicture, path + @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, path + @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, path + @"\output\dataFolder\reports\");
        }
        [Test]
        public void siteSurveyConfiguration_Create_DoesCreate()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            surveyConfiguration.name = Guid.NewGuid().ToString();
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations();
            siteSurveyConfiguration.siteId = site1.id;
            siteSurveyConfiguration.surveyConfigurationId = surveyConfiguration.id;
            // Act
            siteSurveyConfiguration.Create(DbContext);

            site_survey_configurations dbSiteSurveyConfiguration =
                DbContext.site_survey_configurations.AsNoTracking().First();
            site_survey_configuration_versions dbSiteSurveyConfigurationVersion =
                DbContext.site_survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSiteSurveyConfiguration);
            Assert.NotNull(dbSiteSurveyConfigurationVersion);

            Assert.AreEqual(site1.id, dbSiteSurveyConfiguration.siteId);
            Assert.AreEqual(surveyConfiguration.id, siteSurveyConfiguration.surveyConfigurationId);
        }
        [Test]
        public void siteSurveyConfiguration_Update_DoesUpdate()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            surveyConfiguration.name = Guid.NewGuid().ToString();
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            sites site1 = testHelpers.CreateSite("SiteName1", 88);
            
            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations();
            siteSurveyConfiguration.siteId = site1.id;
            siteSurveyConfiguration.surveyConfigurationId = surveyConfiguration.id;
            siteSurveyConfiguration.Create(DbContext);
            // Act
            sites site2 = testHelpers.CreateSite("siteName2", 666);
            survey_configurations surveyConfiguration2 = new survey_configurations();
            surveyConfiguration2.name = Guid.NewGuid().ToString();
            surveyConfiguration2.stop = DateTime.Now;
            surveyConfiguration2.start = DateTime.Now;
            surveyConfiguration2.timeOut = rnd.Next(1, 255);
            surveyConfiguration2.timeToLive = rnd.Next(1, 255);
            surveyConfiguration2.Create(DbContext);

            siteSurveyConfiguration.siteId = site2.id;
            siteSurveyConfiguration.surveyConfigurationId = surveyConfiguration2.id;
            
            siteSurveyConfiguration.Update(DbContext);
            site_survey_configurations dbSiteSurveyConfiguration =
                DbContext.site_survey_configurations.AsNoTracking().First();
            site_survey_configuration_versions dbSiteSurveyConfigurationVersion =
                DbContext.site_survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSiteSurveyConfiguration);
            Assert.NotNull(dbSiteSurveyConfigurationVersion);

            Assert.AreEqual(site2.id, dbSiteSurveyConfiguration.siteId);
            Assert.AreEqual(surveyConfiguration2.id, siteSurveyConfiguration.surveyConfigurationId);
        }
        [Test]
        public void siteSurveyConfiguration_Delete_DoesDelete()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            surveyConfiguration.name = Guid.NewGuid().ToString();
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);
            surveyConfiguration.Create(DbContext);

            sites site1 = testHelpers.CreateSite("SiteName1", 88);

            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations();
            siteSurveyConfiguration.siteId = site1.id;
            siteSurveyConfiguration.surveyConfigurationId = surveyConfiguration.id;
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

            Assert.AreEqual(site1.id, dbSiteSurveyConfiguration.siteId);
            Assert.AreEqual(surveyConfiguration.id, siteSurveyConfiguration.surveyConfigurationId);
            Assert.AreEqual(Constants.WorkflowStates.Removed, siteSurveyConfiguration.workflow_state);
        }
    }
}