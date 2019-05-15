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
        public void surveyConfiguration_Create_DoesCreate()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfigurations = new survey_configurations();

            surveyConfigurations.Name = Guid.NewGuid().ToString();
            surveyConfigurations.Stop = DateTime.Now;
            surveyConfigurations.Start = DateTime.Now;
            surveyConfigurations.TimeOut = rnd.Next(1, 255);
            surveyConfigurations.TimeToLive = rnd.Next(1, 255);
            
            // Act
            surveyConfigurations.Create(DbContext);

            survey_configurations dbSurveyConfigurations = DbContext.survey_configurations.AsNoTracking().First();
            survey_configuration_versions dbSurveyConfigurationVersions =
                DbContext.survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSurveyConfigurations);
            Assert.NotNull(dbSurveyConfigurationVersions);
            
            Assert.AreEqual(surveyConfigurations.Name, dbSurveyConfigurations.Name);
            Assert.AreEqual(surveyConfigurations.Stop.ToString(), dbSurveyConfigurations.Stop.ToString());
            Assert.AreEqual(surveyConfigurations.Start.ToString(), dbSurveyConfigurations.Start.ToString());
            Assert.AreEqual(surveyConfigurations.TimeOut, dbSurveyConfigurations.TimeOut);
            Assert.AreEqual(surveyConfigurations.TimeToLive, dbSurveyConfigurations.TimeToLive);

        }     
        [Test]
        public void surveyConfiguration_Update_DoesUpdate()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            string oldName = Guid.NewGuid().ToString();
            surveyConfiguration.Name = oldName;
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);

            surveyConfiguration.Create(DbContext);
            // Act
            
            string newName = Guid.NewGuid().ToString();
            surveyConfiguration.Name = newName;
            surveyConfiguration.Update(DbContext);

            survey_configurations dbSurveyConfigurations = DbContext.survey_configurations.AsNoTracking().First();
            survey_configuration_versions dbSurveyConfigurationVersions =
                DbContext.survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSurveyConfigurations);
            Assert.NotNull(dbSurveyConfigurationVersions);
            
            Assert.AreNotEqual(oldName, dbSurveyConfigurations.Name);
            Assert.AreEqual(newName, dbSurveyConfigurations.Name);
            Assert.AreEqual(surveyConfiguration.Stop.ToString(), dbSurveyConfigurations.Stop.ToString());
            Assert.AreEqual(surveyConfiguration.Start.ToString(), dbSurveyConfigurations.Start.ToString());
            Assert.AreEqual(surveyConfiguration.TimeOut, dbSurveyConfigurations.TimeOut);
            Assert.AreEqual(surveyConfiguration.TimeToLive, dbSurveyConfigurations.TimeToLive);

        }
        [Test]
        public void surveyConfiguration_Delete_DoesDelete()
        {
            // Arrange
            Random rnd = new Random();
            
            survey_configurations surveyConfiguration = new survey_configurations();

            string oldName = Guid.NewGuid().ToString();
            surveyConfiguration.Name = oldName;
            surveyConfiguration.Stop = DateTime.Now;
            surveyConfiguration.Start = DateTime.Now;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);

            surveyConfiguration.Create(DbContext);
            // Act

            surveyConfiguration.Delete(DbContext);

            survey_configurations dbSurveyConfigurations = DbContext.survey_configurations.AsNoTracking().First();
            survey_configuration_versions dbSurveyConfigurationVersions =
                DbContext.survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSurveyConfigurations);
            Assert.NotNull(dbSurveyConfigurationVersions);
            
            Assert.AreEqual(oldName, dbSurveyConfigurations.Name);
            Assert.AreEqual(surveyConfiguration.Stop.ToString(), dbSurveyConfigurations.Stop.ToString());
            Assert.AreEqual(surveyConfiguration.Start.ToString(), dbSurveyConfigurations.Start.ToString());
            Assert.AreEqual(surveyConfiguration.TimeOut, dbSurveyConfigurations.TimeOut);
            Assert.AreEqual(surveyConfiguration.TimeToLive, dbSurveyConfigurations.TimeToLive);
            Assert.AreEqual(surveyConfiguration.WorkflowState, eFormShared.Constants.WorkflowStates.Removed);

        }   
    }
}