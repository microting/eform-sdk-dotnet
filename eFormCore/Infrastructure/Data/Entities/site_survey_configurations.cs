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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class site_survey_configurations : BaseEntity
    {
        [ForeignKey("site")]
        public int SiteId { get; set; }

        [ForeignKey("survey_configuration")]
        public int SurveyConfigurationId { get; set; }

        public virtual sites Site { get; set; }
        public virtual survey_configurations SurveyConfiguration { get; set; }
        public async Task Create(MicrotingDbAnySql dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Version = 1;

            dbContext.site_survey_configurations.Add(this);
            dbContext.SaveChanges();

            dbContext.site_survey_configuration_versions.Add(MapVersions(this));
            dbContext.SaveChanges();
        }

        public async Task Update(MicrotingDbAnySql dbContext)
        {
            site_survey_configurations siteSurveyConfiguration =
                dbContext.site_survey_configurations.FirstOrDefault(x => x.Id == Id);

            if (siteSurveyConfiguration == null)
            {
                throw new NullReferenceException($"Could not find site survey configuration with Id: {Id}");
            }

            siteSurveyConfiguration.SiteId = SiteId;
            siteSurveyConfiguration.SurveyConfigurationId = SurveyConfigurationId;

            if (dbContext.ChangeTracker.HasChanges())
            {
                Version += 1;
                UpdatedAt = DateTime.Now;

                dbContext.site_survey_configuration_versions.Add(MapVersions(siteSurveyConfiguration));
                dbContext.SaveChanges();
            }
        }

        public async Task Delete(MicrotingDbAnySql dbContext)
        {
            
            site_survey_configurations siteSurveyConfiguration =
                dbContext.site_survey_configurations.FirstOrDefault(x => x.Id == Id);

            if (siteSurveyConfiguration == null)
            {
                throw new NullReferenceException($"Could not find site survey configuration with Id: {Id}");
            }

            siteSurveyConfiguration.WorkflowState = Constants.Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                Version += 1;
                UpdatedAt = DateTime.Now;

                dbContext.site_survey_configuration_versions.Add(MapVersions(siteSurveyConfiguration));
                dbContext.SaveChanges();
            }
        }

        private site_survey_configuration_versions MapVersions(site_survey_configurations siteSurveyConfiguration)
        {
            site_survey_configuration_versions siteSurveyConfigurationVersion = new site_survey_configuration_versions();

            siteSurveyConfigurationVersion.SurveyConfigurationId = siteSurveyConfiguration.SurveyConfigurationId;
            siteSurveyConfigurationVersion.SiteId = siteSurveyConfiguration.SiteId;
            siteSurveyConfigurationVersion.SiteSurveyConfigurationId = siteSurveyConfiguration.Id;
            siteSurveyConfigurationVersion.CreatedAt = siteSurveyConfiguration.CreatedAt;
            siteSurveyConfigurationVersion.UpdatedAt = siteSurveyConfiguration.UpdatedAt;
            siteSurveyConfigurationVersion.WorkflowState = siteSurveyConfiguration.WorkflowState;
            siteSurveyConfigurationVersion.Version = siteSurveyConfiguration.Version;

            return siteSurveyConfigurationVersion;
        }
    }
}