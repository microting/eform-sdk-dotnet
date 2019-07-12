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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace eFormSqlController
{
    public interface MicrotingContextInterface : IDisposable
    {
        DbSet<answer_values> answer_values { get; set; }
        DbSet<answer_value_versions> answer_value_versions { get; set; }
        DbSet<answers> answers { get; set; }
        DbSet<answer_versions> answer_versions { get; set; }
        DbSet<case_versions> case_versions { get; set; }
        DbSet<cases> cases { get; set; }
        DbSet<check_list_site_versions> check_list_site_versions { get; set; }
        DbSet<check_list_sites> check_list_sites { get; set; }
        DbSet<check_list_value_versions> check_list_value_versions { get; set; }
        DbSet<check_list_values> check_list_values { get; set; }
        DbSet<check_list_versions> check_list_versions { get; set; }
        DbSet<check_lists> check_lists { get; set; }
        DbSet<entity_group_versions> entity_group_versions { get; set; }
        DbSet<entity_groups> entity_groups { get; set; }
        DbSet<entity_item_versions> entity_item_versions { get; set; }
        DbSet<entity_items> entity_items { get; set; }
        DbSet<field_types> field_types { get; set; }
        DbSet<field_value_versions> field_value_versions { get; set; }
        DbSet<field_values> field_values { get; set; }
        DbSet<field_versions> field_versions { get; set; }
        DbSet<fields> fields { get; set; }
        DbSet<languages> languages { get; set; }
        DbSet<language_versions> language_versions { get; set; }
        DbSet<log_exceptions> log_exceptions { get; set; }
        DbSet<logs> logs { get; set; }
        DbSet<notifications> notifications { get; set; }
        DbSet<options> options { get; set; }
        DbSet<option_versions> option_versions { get; set; }
        DbSet<question_sets> question_sets { get; set; }
        DbSet<question_set_versions> question_set_versions { get; set; }
        DbSet<questions> questions { get; set; }
        DbSet<question_versions> question_versions { get; set; }
        DbSet<settings> settings { get; set; }
        DbSet<site_survey_configurations> site_survey_configuraions { get; set; }
        DbSet<site_survey_configuration_versions> site_survey_configuraion_versions { get; set; }
        DbSet<site_versions> site_versions { get; set; }
        DbSet<site_worker_versions> site_worker_versions { get; set; }
        DbSet<site_workers> site_workers { get; set; }
        DbSet<sites> sites { get; set; }
        DbSet<survey_configurations> survey_configurations { get; set; }
        DbSet<survey_configuration_versions> survey_configuration_versions { get; set; }
        DbSet<unit_versions> unit_versions { get; set; }
        DbSet<units> units { get; set; }
        DbSet<uploaded_data> uploaded_data { get; set; }
        DbSet<uploaded_data_versions> uploaded_data_versions { get; set; }
        DbSet<worker_versions> worker_versions { get; set; }
        DbSet<workers> workers { get; set; }
        DbSet<tags> tags { get; set; }
        DbSet<tag_versions> tag_versions { get; set; }
        DbSet<taggings> taggings { get; set; }
        DbSet<tagging_versions> tagging_versions { get; set; }
        DbSet<folders> folders { get; set; }
        DbSet<folder_versions> folder_versoions { get; set; }

        int SaveChanges();
        Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade ContextDatabase
        {
            get;
        }

    }
}