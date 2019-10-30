/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 Microting A/S

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
using System.Threading.Tasks;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class survey_configurations : BaseEntity
    {
        public DateTime Start { get; set; }
        
        public DateTime Stop { get; set; }
        
        public int TimeToLive { get; set; }
        
        public string Name { get; set; }
        
        public int TimeOut { get; set; }


        public async Task Create(MicrotingDbAnySql dbContext)
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Version = 1;
            WorkflowState = Constants.Constants.WorkflowStates.Created;

            dbContext.survey_configurations.Add(this);
            dbContext.SaveChanges();

            dbContext.survey_configuration_versions.Add(MapVersions(this));
            dbContext.SaveChanges();
        }

        public async Task Update(MicrotingDbAnySql dbContext)
        {
            survey_configurations surveyConfigurations =
                dbContext.survey_configurations.FirstOrDefault(x => x.Id == Id);

            if (surveyConfigurations == null)
            {
                throw new NullReferenceException($"Could not find survey configuration with Id: {Id}");
            }

            surveyConfigurations.Name = Name;
            surveyConfigurations.Stop = Stop;
            surveyConfigurations.Start = Start;
            surveyConfigurations.TimeOut = TimeOut;
            surveyConfigurations.TimeToLive = TimeToLive;

            if (dbContext.ChangeTracker.HasChanges())
            {
                surveyConfigurations.Version += 1;
                surveyConfigurations.UpdatedAt = DateTime.Now;

                dbContext.survey_configuration_versions.Add(MapVersions(surveyConfigurations));
                dbContext.SaveChanges();
                
            }
        }

        public async Task Delete(MicrotingDbAnySql dbContext)
        {
            survey_configurations surveyConfigurations =
                dbContext.survey_configurations.FirstOrDefault(x => x.Id == Id);

            if (surveyConfigurations == null)
            {
                throw new NullReferenceException($"Could not find survey configuration with Id: {Id}");
            }

            surveyConfigurations.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                surveyConfigurations.Version += 1;
                surveyConfigurations.UpdatedAt = DateTime.Now;

                dbContext.survey_configuration_versions.Add(MapVersions(surveyConfigurations));
                dbContext.SaveChanges();
                
            }
        }

        private survey_configuration_versions MapVersions(survey_configurations surveyConfiguration)
        {
            survey_configuration_versions surveyConfigurationVersions = new survey_configuration_versions();

            surveyConfigurationVersions.SurveyConfigurationId = surveyConfiguration.Id;
            surveyConfigurationVersions.Name = surveyConfiguration.Name;
            surveyConfigurationVersions.Stop = surveyConfiguration.Stop;
            surveyConfigurationVersions.Start = surveyConfiguration.Start;
            surveyConfigurationVersions.TimeOut = surveyConfiguration.TimeOut;
            surveyConfigurationVersions.TimeToLive = surveyConfiguration.TimeToLive;
            surveyConfigurationVersions.Version = surveyConfiguration.Version;
            surveyConfigurationVersions.CreatedAt = surveyConfiguration.CreatedAt;
            surveyConfigurationVersions.UpdatedAt = surveyConfiguration.UpdatedAt;
            surveyConfigurationVersions.WorkflowState = surveyConfiguration.WorkflowState;

            
            return surveyConfigurationVersions;
        }
    }
}