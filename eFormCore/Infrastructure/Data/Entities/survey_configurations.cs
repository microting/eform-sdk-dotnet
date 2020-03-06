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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class survey_configurations : BaseEntity
    {
        public DateTime Start { get; set; }
        
        public DateTime Stop { get; set; }
        
        public int TimeToLive { get; set; }
        
        public string Name { get; set; }
        
        public int TimeOut { get; set; }
        
        [ForeignKey("question_set")]
        public int QuestionSetId { get; set; }
        
        public int? MicrotingUid { get; set; }
        
        public virtual ICollection<site_survey_configurations> SiteSurveyConfigurations { get; set; }
        
        public virtual question_sets QuestionSet { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Version = 1;
            if (WorkflowState == null)
            {
                WorkflowState = Constants.Constants.WorkflowStates.Created; 
            }

            dbContext.survey_configurations.Add(this);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            dbContext.survey_configuration_versions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            survey_configurations surveyConfigurations =
                await dbContext.survey_configurations.FirstOrDefaultAsync(x => x.Id == Id);

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
                await dbContext.SaveChangesAsync();
                
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            survey_configurations surveyConfigurations =
                await dbContext.survey_configurations.FirstOrDefaultAsync(x => x.Id == Id);

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
                await dbContext.SaveChangesAsync();
                
            }
        }

        private survey_configuration_versions MapVersions(survey_configurations surveyConfiguration)
        {
            survey_configuration_versions surveyConfigurationVersions = new survey_configuration_versions
            {
                SurveyConfigurationId = surveyConfiguration.Id,
                Name = surveyConfiguration.Name,
                Stop = surveyConfiguration.Stop,
                Start = surveyConfiguration.Start,
                TimeOut = surveyConfiguration.TimeOut,
                TimeToLive = surveyConfiguration.TimeToLive,
                Version = surveyConfiguration.Version,
                CreatedAt = surveyConfiguration.CreatedAt,
                UpdatedAt = surveyConfiguration.UpdatedAt,
                WorkflowState = surveyConfiguration.WorkflowState,
                QuestionSetId = surveyConfiguration.QuestionSetId,
                MicrotingUid = surveyConfiguration.MicrotingUid
            };

            return surveyConfigurationVersions;
        }
    }
}