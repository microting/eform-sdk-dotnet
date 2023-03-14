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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace eFormSDK.InSight.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SiteSurveyConfigurationsUTest : DbTestFixture
    {
        [Test]
        public async Task SiteSurveyConfigurations_Create_DoesCreate()
        {
            //Arrange

            Random rnd = new Random();

            Site site = new Site();
            site.Name = Guid.NewGuid().ToString();
            site.MicrotingUid = rnd.Next(1, 255);
            await site.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSet = new QuestionSet
            {
                ParentId = 0
            };

            await questionSet.Create(DbContext).ConfigureAwait(false);

            SurveyConfiguration surveyConfiguration = new SurveyConfiguration
            {
                Name = Guid.NewGuid().ToString(),
                Start = DateTime.UtcNow,
                Stop = DateTime.UtcNow,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(DbContext).ConfigureAwait(false);

            SiteSurveyConfiguration siteSurveyConfiguration = new SiteSurveyConfiguration
            {
                SiteId = site.Id,
                SurveyConfigurationId = surveyConfiguration.Id
            };


            //Act

            await siteSurveyConfiguration.Create(DbContext).ConfigureAwait(false);

            List<SiteSurveyConfiguration> siteSurveyConfigurations =
                DbContext.SiteSurveyConfigurations.AsNoTracking().ToList();
            List<SiteSurveyConfigurationVersion> siteSurveyConfigurationVersions =
                DbContext.SiteSurveyConfigurationVersions.AsNoTracking().ToList();

            Assert.NotNull(siteSurveyConfigurations);
            Assert.NotNull(siteSurveyConfigurationVersions);

            Assert.AreEqual(1, siteSurveyConfigurations.Count());
            Assert.AreEqual(1, siteSurveyConfigurationVersions.Count());

            Assert.AreEqual(siteSurveyConfiguration.CreatedAt.ToString(),
                siteSurveyConfigurations[0].CreatedAt.ToString());
            Assert.AreEqual(siteSurveyConfiguration.Version, siteSurveyConfigurations[0].Version);
//            Assert.AreEqual(siteSurveyConfiguration.UpdatedAt.ToString(), siteSurveyConfigurations[0].UpdatedAt.ToString());
            Assert.AreEqual(siteSurveyConfigurations[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteSurveyConfiguration.Id, siteSurveyConfigurations[0].Id);
            Assert.AreEqual(siteSurveyConfiguration.SiteId, site.Id);
            Assert.AreEqual(siteSurveyConfiguration.SurveyConfigurationId, surveyConfiguration.Id);

            //Versions

            Assert.AreEqual(siteSurveyConfiguration.CreatedAt.ToString(),
                siteSurveyConfigurationVersions[0].CreatedAt.ToString());
            Assert.AreEqual(siteSurveyConfiguration.Version, siteSurveyConfigurationVersions[0].Version);
//            Assert.AreEqual(siteSurveyConfiguration.UpdatedAt.ToString(), siteSurveyConfigurationVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(siteSurveyConfigurationVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteSurveyConfiguration.Id, siteSurveyConfigurationVersions[0].Id);
            Assert.AreEqual(site.Id, siteSurveyConfigurationVersions[0].SiteId);
            Assert.AreEqual(surveyConfiguration.Id, siteSurveyConfigurationVersions[0].SurveyConfigurationId);
        }

        [Test]
        public async Task SiteSurveyConfiguration_Delete_DoesDelete()
        {
            //Arrange

            Random rnd = new Random();

            Site site = new Site
            {
                Name = Guid.NewGuid().ToString(),
                MicrotingUid = rnd.Next(1, 255)
            };
            await site.Create(DbContext).ConfigureAwait(false);

            QuestionSet questionSet = new QuestionSet
            {
                ParentId = 0
            };

            await questionSet.Create(DbContext).ConfigureAwait(false);

            SurveyConfiguration surveyConfiguration = new SurveyConfiguration
            {
                Name = Guid.NewGuid().ToString(),
                Start = DateTime.UtcNow,
                Stop = DateTime.UtcNow,
                TimeOut = rnd.Next(1, 255),
                TimeToLive = rnd.Next(1, 255),
                QuestionSetId = questionSet.Id
            };
            await surveyConfiguration.Create(DbContext).ConfigureAwait(false);

            SiteSurveyConfiguration siteSurveyConfiguration = new SiteSurveyConfiguration
            {
                SiteId = site.Id,
                SurveyConfigurationId = surveyConfiguration.Id
            };
            await siteSurveyConfiguration.Create(DbContext).ConfigureAwait(false);

            //Act

//            DateTime? oldUpdatedAt = siteSurveyConfiguration.UpdatedAt;

            await siteSurveyConfiguration.Delete(DbContext);


            List<SiteSurveyConfiguration> siteSurveyConfigurations =
                DbContext.SiteSurveyConfigurations.AsNoTracking().ToList();
            List<SiteSurveyConfigurationVersion> siteSurveyConfigurationVersions =
                DbContext.SiteSurveyConfigurationVersions.AsNoTracking().ToList();

            Assert.NotNull(siteSurveyConfigurations);
            Assert.NotNull(siteSurveyConfigurationVersions);

            Assert.AreEqual(1, siteSurveyConfigurations.Count());
            Assert.AreEqual(2, siteSurveyConfigurationVersions.Count());

            Assert.AreEqual(siteSurveyConfiguration.CreatedAt.ToString(),
                siteSurveyConfigurations[0].CreatedAt.ToString());
            Assert.AreEqual(siteSurveyConfiguration.Version, siteSurveyConfigurations[0].Version);
//            Assert.AreEqual(siteSurveyConfiguration.UpdatedAt.ToString(), siteSurveyConfigurations[0].UpdatedAt.ToString());
            Assert.AreEqual(siteSurveyConfigurations[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(siteSurveyConfiguration.Id, siteSurveyConfigurations[0].Id);
            Assert.AreEqual(siteSurveyConfiguration.SiteId, site.Id);
            Assert.AreEqual(siteSurveyConfiguration.SurveyConfigurationId, surveyConfiguration.Id);

            //Old Version

            Assert.AreEqual(siteSurveyConfiguration.CreatedAt.ToString(),
                siteSurveyConfigurationVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, siteSurveyConfigurationVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), siteSurveyConfigurationVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(siteSurveyConfigurationVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(siteSurveyConfiguration.Id, siteSurveyConfigurationVersions[0].SiteSurveyConfigurationId);
            Assert.AreEqual(site.Id, siteSurveyConfigurationVersions[0].SiteId);
            Assert.AreEqual(surveyConfiguration.Id, siteSurveyConfigurationVersions[0].SurveyConfigurationId);

            //New Version

            Assert.AreEqual(siteSurveyConfiguration.CreatedAt.ToString(),
                siteSurveyConfigurationVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, siteSurveyConfigurationVersions[1].Version);
//            Assert.AreEqual(siteSurveyConfiguration.UpdatedAt.ToString(), siteSurveyConfigurationVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(siteSurveyConfigurationVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(siteSurveyConfiguration.Id, siteSurveyConfigurationVersions[1].SiteSurveyConfigurationId);
            Assert.AreEqual(site.Id, siteSurveyConfigurationVersions[1].SiteId);
            Assert.AreEqual(surveyConfiguration.Id, siteSurveyConfigurationVersions[1].SurveyConfigurationId);
        }
    }
}