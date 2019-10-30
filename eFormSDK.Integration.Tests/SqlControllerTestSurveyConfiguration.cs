using eFormCore;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
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
    public class SqlControllerTestSurveyConfiguration : DbTestFixture
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
        public async Task surveyConfiguration_Create_DoesCreate()
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
            await surveyConfigurations.Create(dbContext);

            survey_configurations dbSurveyConfigurations = dbContext.survey_configurations.AsNoTracking().First();
            survey_configuration_versions dbSurveyConfigurationVersions =
                dbContext.survey_configuration_versions.AsNoTracking().First();
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
        public async Task surveyConfiguration_Update_DoesUpdate()
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

            await surveyConfiguration.Create(dbContext);
            // Act
            
            string newName = Guid.NewGuid().ToString();
            surveyConfiguration.Name = newName;
            await surveyConfiguration.Update(dbContext);

            survey_configurations dbSurveyConfigurations = dbContext.survey_configurations.AsNoTracking().First();
            survey_configuration_versions dbSurveyConfigurationVersions =
                dbContext.survey_configuration_versions.AsNoTracking().First();
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
        public async Task surveyConfiguration_Delete_DoesDelete()
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

            await surveyConfiguration.Create(dbContext);
            // Act

            await surveyConfiguration.Delete(dbContext);

            survey_configurations dbSurveyConfigurations = dbContext.survey_configurations.AsNoTracking().First();
            survey_configuration_versions dbSurveyConfigurationVersions =
                dbContext.survey_configuration_versions.AsNoTracking().First();
            // Assert
            Assert.NotNull(dbSurveyConfigurations);
            Assert.NotNull(dbSurveyConfigurationVersions);
            
            Assert.AreEqual(oldName, dbSurveyConfigurations.Name);
            Assert.AreEqual(surveyConfiguration.Stop.ToString(), dbSurveyConfigurations.Stop.ToString());
            Assert.AreEqual(surveyConfiguration.Start.ToString(), dbSurveyConfigurations.Start.ToString());
            Assert.AreEqual(surveyConfiguration.TimeOut, dbSurveyConfigurations.TimeOut);
            Assert.AreEqual(surveyConfiguration.TimeToLive, dbSurveyConfigurations.TimeToLive);
            Assert.AreEqual(surveyConfiguration.WorkflowState, Constants.WorkflowStates.Removed);

        }   
    }
}