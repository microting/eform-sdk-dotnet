/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 microting

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
using System.Linq;

namespace eFormSqlController
{
    public partial class survey_configurations : base_entity
    {
        public DateTime start { get; set; }
        
        public DateTime stop { get; set; }
        
        public int timeToLive { get; set; }
        
        public string name { get; set; }
        
        public int timeOut { get; set; }


        public void Create(MicrotingDbAnySql dbContext)
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
            version = 1;
            workflow_state = eFormShared.Constants.WorkflowStates.Created;

            dbContext.survey_configurations.Add(this);
            dbContext.SaveChanges();

            dbContext.survey_configuration_versions.Add(MapVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            survey_configurations surveyConfigurations =
                dbContext.survey_configurations.FirstOrDefault(x => x.id == id);

            if (surveyConfigurations == null)
            {
                throw new NullReferenceException($"Could not find survey configuration with ID: {id}");
            }

            surveyConfigurations.name = name;
            surveyConfigurations.stop = stop;
            surveyConfigurations.start = start;
            surveyConfigurations.timeOut = timeOut;
            surveyConfigurations.timeToLive = timeToLive;

            if (dbContext.ChangeTracker.HasChanges())
            {
                surveyConfigurations.version += 1;
                surveyConfigurations.updated_at = DateTime.Now;

                dbContext.survey_configuration_versions.Add(MapVersions(surveyConfigurations));
                dbContext.SaveChanges();
                
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            survey_configurations surveyConfigurations =
                dbContext.survey_configurations.FirstOrDefault(x => x.id == id);

            if (surveyConfigurations == null)
            {
                throw new NullReferenceException($"Could not find survey configuration with ID: {id}");
            }

            surveyConfigurations.workflow_state = eFormShared.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                surveyConfigurations.version += 1;
                surveyConfigurations.updated_at = DateTime.Now;

                dbContext.survey_configuration_versions.Add(MapVersions(surveyConfigurations));
                dbContext.SaveChanges();
                
            }
        }

        private survey_configuration_versions MapVersions(survey_configurations surveyConfiguration)
        {
            survey_configuration_versions surveyConfigurationVersions = new survey_configuration_versions();

            surveyConfigurationVersions.surveyConfigurationId = surveyConfiguration.id;
            surveyConfigurationVersions.name = surveyConfiguration.name;
            surveyConfigurationVersions.stop = surveyConfiguration.stop;
            surveyConfigurationVersions.start = surveyConfiguration.start;
            surveyConfigurationVersions.timeOut = surveyConfiguration.timeOut;
            surveyConfigurationVersions.timeToLive = surveyConfiguration.timeToLive;
            surveyConfigurationVersions.version = surveyConfiguration.version;
            surveyConfigurationVersions.created_at = surveyConfiguration.created_at;
            surveyConfigurationVersions.updated_at = surveyConfiguration.updated_at;
            surveyConfigurationVersions.workflow_state = surveyConfiguration.workflow_state;

            
            return surveyConfigurationVersions;
        }
    }
}