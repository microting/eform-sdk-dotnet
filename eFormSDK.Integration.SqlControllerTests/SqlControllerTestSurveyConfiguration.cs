using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.Integration.SqlControllerTests 
{
    public class SqlControllerTestSurveyConfiguration : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;
        string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:", "");

        public override async Task DoSetup()
        {
            if (sut == null)
            {
                sut = new SqlController(ConnectionString);
                await sut.StartLog(new CoreBase());
            }
            testHelpers = new TestHelpers();
            await sut.SettingUpdate(Settings.fileLocationPicture, Path.Combine(path, "output", "dataFolder", "picture"));
            await sut.SettingUpdate(Settings.fileLocationPdf, Path.Combine(path, "output", "dataFolder", "pdf"));
            await sut.SettingUpdate(Settings.fileLocationJasper, Path.Combine(path, "output", "dataFolder", "reports"));
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