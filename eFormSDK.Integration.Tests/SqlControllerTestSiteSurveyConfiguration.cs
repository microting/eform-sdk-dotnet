/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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
using Microting.eForm.Infrastructure.Helpers;

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

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(dbContextHelper);
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
            
            question_sets questionSet = new question_sets()
            {
                ParentId = 0
            };
            
            await questionSet.Create(dbContext);

            survey_configurations surveyConfiguration = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };

            await surveyConfiguration.Create(dbContext);

            sites site1 = await testHelpers.CreateSite("SiteName1", 88);

            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations
            {
                SiteId = site1.Id, 
                SurveyConfigurationId = surveyConfiguration.Id
            };
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
            
            question_sets questionSet = new question_sets()
            {
                ParentId = 0
            };
            
            await questionSet.Create(dbContext);

            survey_configurations surveyConfiguration = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };

            await surveyConfiguration.Create(dbContext);

            sites site1 = await testHelpers.CreateSite("SiteName1", 88);

            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations
            {
                SiteId = site1.Id, SurveyConfigurationId = surveyConfiguration.Id
            };
            await siteSurveyConfiguration.Create(dbContext);
            // Act
            sites site2 = await testHelpers.CreateSite("siteName2", 666);
            survey_configurations surveyConfiguration2 = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
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
            
            question_sets questionSet = new question_sets()
            {
                ParentId = 0
            };
            
            await questionSet.Create(dbContext);

            survey_configurations surveyConfiguration = new survey_configurations
            {
                Name = Guid.NewGuid().ToString(),
                Stop = DateTime.Now,
                Start = DateTime.Now,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };

            await surveyConfiguration.Create(dbContext);

            sites site1 = await testHelpers.CreateSite("SiteName1", 88);

            site_survey_configurations siteSurveyConfiguration = new site_survey_configurations
            {
                SiteId = site1.Id,
                SurveyConfigurationId = surveyConfiguration.Id
            };
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