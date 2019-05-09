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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using eFormShared;

namespace eFormSqlController
{
    public partial class site_survey_configurations : BaseEntity
    {
        [ForeignKey("site")]
        public int siteId { get; set; }

        [ForeignKey("survey_configuration")]
        public int surveyConfigurationId { get; set; }

        public virtual sites Site { get; set; }
        public virtual survey_configurations SurveyConfiguration { get; set; }
        public void Create(MicrotingDbAnySql dbContext)
        {
            workflow_state = Constants.WorkflowStates.Created;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
            version = 1;

            dbContext.site_survey_configurations.Add(this);
            dbContext.SaveChanges();

            dbContext.site_survey_configuration_versions.Add(MapVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            site_survey_configurations siteSurveyConfiguration =
                dbContext.site_survey_configurations.FirstOrDefault(x => x.id == id);

            if (siteSurveyConfiguration == null)
            {
                throw new NullReferenceException($"Could not find site survey configuration with ID: {id}");
            }

            siteSurveyConfiguration.siteId = siteId;
            siteSurveyConfiguration.surveyConfigurationId = surveyConfigurationId;

            if (dbContext.ChangeTracker.HasChanges())
            {
                version += 1;
                updated_at = DateTime.Now;

                dbContext.site_survey_configuration_versions.Add(MapVersions(siteSurveyConfiguration));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            
            site_survey_configurations siteSurveyConfiguration =
                dbContext.site_survey_configurations.FirstOrDefault(x => x.id == id);

            if (siteSurveyConfiguration == null)
            {
                throw new NullReferenceException($"Could not find site survey configuration with ID: {id}");
            }

            siteSurveyConfiguration.workflow_state = Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                version += 1;
                updated_at = DateTime.Now;

                dbContext.site_survey_configuration_versions.Add(MapVersions(siteSurveyConfiguration));
                dbContext.SaveChanges();
            }
        }

        private site_survey_configuration_versions MapVersions(site_survey_configurations siteSurveyConfiguration)
        {
            site_survey_configuration_versions siteSurveyConfigurationVersion = new site_survey_configuration_versions();

            siteSurveyConfigurationVersion.surveyConfigurationId = siteSurveyConfiguration.surveyConfigurationId;
            siteSurveyConfigurationVersion.siteId = siteSurveyConfiguration.siteId;
            siteSurveyConfigurationVersion.siteSurveyConfigurationId = siteSurveyConfiguration.id;
            siteSurveyConfigurationVersion.created_at = siteSurveyConfiguration.created_at;
            siteSurveyConfigurationVersion.updated_at = siteSurveyConfiguration.updated_at;
            siteSurveyConfigurationVersion.workflow_state = siteSurveyConfiguration.workflow_state;
            siteSurveyConfigurationVersion.version = siteSurveyConfiguration.version;

            return siteSurveyConfigurationVersion;
        }
    }
}