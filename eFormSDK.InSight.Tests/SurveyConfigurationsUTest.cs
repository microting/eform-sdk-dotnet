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
    public class SurveyConfigurationsUTest : DbTestFixture
    {
        [Test]
        public async Task SurveyConfigurations_Create_DoesCreate()
        {
            //Arrange
            Random rnd = new Random();

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

            //Act

            await surveyConfiguration.Create(DbContext).ConfigureAwait(false);

            List<SurveyConfiguration> surveyConfigurations = DbContext.SurveyConfigurations.AsNoTracking().ToList();
            List<SurveyConfigurationVersion> surveyConfigurationVersions =
                DbContext.SurveyConfigurationVersions.AsNoTracking().ToList();

            Assert.NotNull(surveyConfigurations);
            Assert.NotNull(surveyConfigurationVersions);

            Assert.AreEqual(1, surveyConfigurations.Count());
            Assert.AreEqual(1, surveyConfigurationVersions.Count());

            Assert.AreEqual(surveyConfiguration.CreatedAt.ToString(), surveyConfigurations[0].CreatedAt.ToString());
            Assert.AreEqual(surveyConfiguration.Version, surveyConfigurations[0].Version);
//            Assert.AreEqual(surveyConfiguration.UpdatedAt.ToString(), surveyConfigurations[0].UpdatedAt.ToString());
            Assert.AreEqual(surveyConfigurations[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(surveyConfiguration.Id, surveyConfigurations[0].Id);
            Assert.AreEqual(surveyConfiguration.Name, surveyConfigurations[0].Name);
            Assert.AreEqual(surveyConfiguration.Start.ToString(), surveyConfigurations[0].Start.ToString());
            Assert.AreEqual(surveyConfiguration.Stop.ToString(), surveyConfigurations[0].Stop.ToString());
            Assert.AreEqual(surveyConfiguration.TimeOut, surveyConfigurations[0].TimeOut);
            Assert.AreEqual(surveyConfiguration.TimeToLive, surveyConfigurations[0].TimeToLive);

            //Versions

            Assert.AreEqual(surveyConfiguration.CreatedAt.ToString(),
                surveyConfigurationVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, surveyConfigurationVersions[0].Version);
//            Assert.AreEqual(surveyConfiguration.UpdatedAt.ToString(), surveyConfigurationVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(surveyConfigurationVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(surveyConfiguration.Id, surveyConfigurationVersions[0].SurveyConfigurationId);
            Assert.AreEqual(surveyConfiguration.Name, surveyConfigurationVersions[0].Name);
            Assert.AreEqual(surveyConfiguration.Start.ToString(), surveyConfigurationVersions[0].Start.ToString());
            Assert.AreEqual(surveyConfiguration.Stop.ToString(), surveyConfigurationVersions[0].Stop.ToString());
            Assert.AreEqual(surveyConfiguration.TimeOut, surveyConfigurationVersions[0].TimeOut);
            Assert.AreEqual(surveyConfiguration.TimeToLive, surveyConfigurationVersions[0].TimeToLive);
        }

        [Test]
        public async Task SurveyConfigurations_Update_DoesUpdate()
        {
            //Arrange
            Random rnd = new Random();

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

            //Act

            DateTime? oldUpdatedAt = surveyConfiguration.UpdatedAt;
            string oldName = surveyConfiguration.Name;
            DateTime? oldStart = surveyConfiguration.Start;
            DateTime? oldStop = surveyConfiguration.Stop;
            int? oldTimeOut = surveyConfiguration.TimeOut;
            int? oldTimeToLive = surveyConfiguration.TimeToLive;

            surveyConfiguration.Name = Guid.NewGuid().ToString();
            surveyConfiguration.Start = DateTime.UtcNow;
            surveyConfiguration.Stop = DateTime.UtcNow;
            surveyConfiguration.TimeOut = rnd.Next(1, 255);
            surveyConfiguration.TimeToLive = rnd.Next(1, 255);
            await surveyConfiguration.Update(DbContext).ConfigureAwait(false);

            List<SurveyConfiguration> surveyConfigurations = DbContext.SurveyConfigurations.AsNoTracking().ToList();
            List<SurveyConfigurationVersion> surveyConfigurationVersions =
                DbContext.SurveyConfigurationVersions.AsNoTracking().ToList();

            Assert.NotNull(surveyConfigurations);
            Assert.NotNull(surveyConfigurationVersions);

            Assert.AreEqual(1, surveyConfigurations.Count());
            Assert.AreEqual(2, surveyConfigurationVersions.Count());

            Assert.AreEqual(surveyConfiguration.CreatedAt.ToString(), surveyConfigurations[0].CreatedAt.ToString());
            Assert.AreEqual(surveyConfiguration.Version, surveyConfigurations[0].Version);
//            Assert.AreEqual(surveyConfiguration.UpdatedAt.ToString(), surveyConfigurations[0].UpdatedAt.ToString());
            Assert.AreEqual(surveyConfigurations[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(surveyConfiguration.Id, surveyConfigurations[0].Id);
            Assert.AreEqual(surveyConfiguration.Name, surveyConfigurations[0].Name);
            Assert.AreEqual(surveyConfiguration.Start.ToString(), surveyConfigurations[0].Start.ToString());
            Assert.AreEqual(surveyConfiguration.Stop.ToString(), surveyConfigurations[0].Stop.ToString());
            Assert.AreEqual(surveyConfiguration.TimeOut, surveyConfigurations[0].TimeOut);
            Assert.AreEqual(surveyConfiguration.TimeToLive, surveyConfigurations[0].TimeToLive);

            //Old Version

            Assert.AreEqual(surveyConfiguration.CreatedAt.ToString(),
                surveyConfigurationVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, surveyConfigurationVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), surveyConfigurationVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(surveyConfigurationVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(surveyConfiguration.Id, surveyConfigurationVersions[0].SurveyConfigurationId);
            Assert.AreEqual(oldName, surveyConfigurationVersions[0].Name);
            Assert.AreEqual(oldStart.ToString(), surveyConfigurationVersions[0].Start.ToString());
            Assert.AreEqual(oldStop.ToString(), surveyConfigurationVersions[0].Stop.ToString());
            Assert.AreEqual(oldTimeOut, surveyConfigurationVersions[0].TimeOut);
            Assert.AreEqual(oldTimeToLive, surveyConfigurationVersions[0].TimeToLive);

            //New Version

            Assert.AreEqual(surveyConfiguration.CreatedAt.ToString(),
                surveyConfigurationVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, surveyConfigurationVersions[1].Version);
//            Assert.AreEqual(surveyConfiguration.UpdatedAt.ToString(), surveyConfigurationVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(surveyConfigurationVersions[1].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(surveyConfiguration.Id, surveyConfigurationVersions[1].SurveyConfigurationId);
            Assert.AreEqual(surveyConfiguration.Name, surveyConfigurationVersions[1].Name);
            Assert.AreEqual(surveyConfiguration.Start.ToString(), surveyConfigurationVersions[1].Start.ToString());
            Assert.AreEqual(surveyConfiguration.Stop.ToString(), surveyConfigurationVersions[1].Stop.ToString());
            Assert.AreEqual(surveyConfiguration.TimeOut, surveyConfigurationVersions[1].TimeOut);
            Assert.AreEqual(surveyConfiguration.TimeToLive, surveyConfigurationVersions[1].TimeToLive);
        }

        [Test]
        public async Task SurveyConfigurations_Delete_DoesSetWorkflowStateToRemoved()
        {
            //Arrange
            Random rnd = new Random();

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


            //Act

            DateTime? oldUpdatedAt = surveyConfiguration.UpdatedAt;

            await surveyConfiguration.Delete(DbContext);

            List<SurveyConfiguration> surveyConfigurations = DbContext.SurveyConfigurations.AsNoTracking().ToList();
            List<SurveyConfigurationVersion> surveyConfigurationVersions =
                DbContext.SurveyConfigurationVersions.AsNoTracking().ToList();

            Assert.NotNull(surveyConfigurations);
            Assert.NotNull(surveyConfigurationVersions);

            Assert.AreEqual(1, surveyConfigurations.Count());
            Assert.AreEqual(2, surveyConfigurationVersions.Count());

            Assert.AreEqual(surveyConfiguration.CreatedAt.ToString(), surveyConfigurations[0].CreatedAt.ToString());
            Assert.AreEqual(surveyConfiguration.Version, surveyConfigurations[0].Version);
//            Assert.AreEqual(surveyConfiguration.UpdatedAt.ToString(), surveyConfigurations[0].UpdatedAt.ToString());
            Assert.AreEqual(surveyConfigurations[0].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(surveyConfiguration.Id, surveyConfigurations[0].Id);
            Assert.AreEqual(surveyConfiguration.Name, surveyConfigurations[0].Name);
            Assert.AreEqual(surveyConfiguration.Start.ToString(), surveyConfigurations[0].Start.ToString());
            Assert.AreEqual(surveyConfiguration.Stop.ToString(), surveyConfigurations[0].Stop.ToString());
            Assert.AreEqual(surveyConfiguration.TimeOut, surveyConfigurations[0].TimeOut);
            Assert.AreEqual(surveyConfiguration.TimeToLive, surveyConfigurations[0].TimeToLive);

            //Old Version

            Assert.AreEqual(surveyConfiguration.CreatedAt.ToString(),
                surveyConfigurationVersions[0].CreatedAt.ToString());
            Assert.AreEqual(1, surveyConfigurationVersions[0].Version);
//            Assert.AreEqual(oldUpdatedAt.ToString(), surveyConfigurationVersions[0].UpdatedAt.ToString());
            Assert.AreEqual(surveyConfigurationVersions[0].WorkflowState, Constants.WorkflowStates.Created);
            Assert.AreEqual(surveyConfiguration.Id, surveyConfigurationVersions[0].SurveyConfigurationId);
            Assert.AreEqual(surveyConfiguration.Name, surveyConfigurationVersions[0].Name);
            Assert.AreEqual(surveyConfiguration.Start.ToString(), surveyConfigurationVersions[0].Start.ToString());
            Assert.AreEqual(surveyConfiguration.Stop.ToString(), surveyConfigurationVersions[0].Stop.ToString());
            Assert.AreEqual(surveyConfiguration.TimeOut, surveyConfigurationVersions[0].TimeOut);
            Assert.AreEqual(surveyConfiguration.TimeToLive, surveyConfigurationVersions[0].TimeToLive);

            //New Version

            Assert.AreEqual(surveyConfiguration.CreatedAt.ToString(),
                surveyConfigurationVersions[1].CreatedAt.ToString());
            Assert.AreEqual(2, surveyConfigurationVersions[1].Version);
//            Assert.AreEqual(surveyConfiguration.UpdatedAt.ToString(), surveyConfigurationVersions[1].UpdatedAt.ToString());
            Assert.AreEqual(surveyConfigurationVersions[1].WorkflowState, Constants.WorkflowStates.Removed);
            Assert.AreEqual(surveyConfiguration.Id, surveyConfigurationVersions[1].SurveyConfigurationId);
            Assert.AreEqual(surveyConfiguration.Name, surveyConfigurationVersions[1].Name);
            Assert.AreEqual(surveyConfiguration.Start.ToString(), surveyConfigurationVersions[1].Start.ToString());
            Assert.AreEqual(surveyConfiguration.Stop.ToString(), surveyConfigurationVersions[1].Stop.ToString());
            Assert.AreEqual(surveyConfiguration.TimeOut, surveyConfigurationVersions[1].TimeOut);
            Assert.AreEqual(surveyConfiguration.TimeToLive, surveyConfigurationVersions[1].TimeToLive);
        }
    }
}