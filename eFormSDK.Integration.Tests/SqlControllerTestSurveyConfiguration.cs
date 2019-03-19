using eFormCore;
using eFormCore.Helpers;
using eFormData;
using eFormShared;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace eFormSDK.Integration.Tests 
{
    public class SqlControllerTestSurveyConfiguration : DbTestFixture
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
        public void surveyConfiguration_Create_DoesCreate()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfigurations = new survey_configurations();

            surveyConfigurations.name = Guid.NewGuid().ToString();
            surveyConfigurations.stop = DateTime.Now;
            surveyConfigurations.start = DateTime.Now;
            surveyConfigurations.timeOut = rnd.Next(1, 255);
            surveyConfigurations.timeToLive = rnd.Next(1, 255);
            
            // Act
            surveyConfigurations.Create(DbContext);

            survey_configurations dbSurveyConfigurations = DbContext.survey_configurations.AsNoTracking().First();
            survey_configuration_versions dbSurveyConfigurationVersions =
                DbContext.survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSurveyConfigurations);
            Assert.NotNull(dbSurveyConfigurationVersions);
            
            Assert.AreEqual(surveyConfigurations.name, dbSurveyConfigurations.name);
            Assert.AreEqual(surveyConfigurations.stop.ToString(), dbSurveyConfigurations.stop.ToString());
            Assert.AreEqual(surveyConfigurations.start.ToString(), dbSurveyConfigurations.start.ToString());
            Assert.AreEqual(surveyConfigurations.timeOut, dbSurveyConfigurations.timeOut);
            Assert.AreEqual(surveyConfigurations.timeToLive, dbSurveyConfigurations.timeToLive);

        }     
        [Test]
        public void surveyConfiguration_Update_DoesUpdate()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            string oldName = Guid.NewGuid().ToString();
            surveyConfiguration.name = oldName;
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);

            surveyConfiguration.Create(DbContext);
            // Act
            
            string newName = Guid.NewGuid().ToString();
            surveyConfiguration.name = newName;
            surveyConfiguration.Update(DbContext);

            survey_configurations dbSurveyConfigurations = DbContext.survey_configurations.AsNoTracking().First();
            survey_configuration_versions dbSurveyConfigurationVersions =
                DbContext.survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSurveyConfigurations);
            Assert.NotNull(dbSurveyConfigurationVersions);
            
            Assert.AreNotEqual(oldName, dbSurveyConfigurations.name);
            Assert.AreEqual(newName, dbSurveyConfigurations.name);
            Assert.AreEqual(surveyConfiguration.stop.ToString(), dbSurveyConfigurations.stop.ToString());
            Assert.AreEqual(surveyConfiguration.start.ToString(), dbSurveyConfigurations.start.ToString());
            Assert.AreEqual(surveyConfiguration.timeOut, dbSurveyConfigurations.timeOut);
            Assert.AreEqual(surveyConfiguration.timeToLive, dbSurveyConfigurations.timeToLive);

        }
        [Test]
        public void surveyConfiguration_Delete_DoesDelete()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            string oldName = Guid.NewGuid().ToString();
            surveyConfiguration.name = oldName;
            surveyConfiguration.stop = DateTime.Now;
            surveyConfiguration.start = DateTime.Now;
            surveyConfiguration.timeOut = rnd.Next(1, 255);
            surveyConfiguration.timeToLive = rnd.Next(1, 255);

            surveyConfiguration.Create(DbContext);
            // Act

            surveyConfiguration.Delete(DbContext);

            survey_configurations dbSurveyConfigurations = DbContext.survey_configurations.AsNoTracking().First();
            survey_configuration_versions dbSurveyConfigurationVersions =
                DbContext.survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSurveyConfigurations);
            Assert.NotNull(dbSurveyConfigurationVersions);
            
            Assert.AreEqual(oldName, dbSurveyConfigurations.name);
            Assert.AreEqual(surveyConfiguration.stop.ToString(), dbSurveyConfigurations.stop.ToString());
            Assert.AreEqual(surveyConfiguration.start.ToString(), dbSurveyConfigurations.start.ToString());
            Assert.AreEqual(surveyConfiguration.timeOut, dbSurveyConfigurations.timeOut);
            Assert.AreEqual(surveyConfiguration.timeToLive, dbSurveyConfigurations.timeToLive);
            Assert.AreEqual(surveyConfiguration.workflow_state, eFormShared.Constants.WorkflowStates.Removed);

        }   
    }
}