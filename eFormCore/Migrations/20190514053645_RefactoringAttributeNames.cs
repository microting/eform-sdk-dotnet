/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

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

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class RefactoringAttributeNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Setup for SQL Server Provider

            string autoIDGenStrategy = "SqlServer:ValueGenerationStrategy";
            object autoIDGenStrategyValue= MySqlValueGenerationStrategy.IdentityColumn;

            // Setup for MySQL Provider
            if (migrationBuilder.ActiveProvider=="Pomelo.EntityFrameworkCore.MySql")
            {
                DbConfig.IsMySQL = true;
                autoIDGenStrategy = "MySql:ValueGenerationStrategy";
                autoIDGenStrategyValue = MySqlValueGenerationStrategy.IdentityColumn;
            }
//            migrationBuilder.DropForeignKey(
//                name: "FK_answer_value_versions_answer_values_answerValueId",
//                table: "answer_value_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answer_values_answers_answerId",
//                table: "answer_values");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answer_values_options_optionsId",
//                table: "answer_values");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answer_values_questions_questionId",
//                table: "answer_values");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answers_languages_languageId",
//                table: "answers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answers_question_sets_questionSetId",
//                table: "answers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answers_sites_siteId",
//                table: "answers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answers_survey_configurations_surveyConfigurationId",
//                table: "answers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answers_units_unitId",
//                table: "answers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_cases_check_lists_check_list_id",
//                table: "cases");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_cases_workers_done_by_user_id",
//                table: "cases");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_cases_sites_site_id",
//                table: "cases");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_cases_units_unit_id",
//                table: "cases");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_check_list_sites_check_lists_check_list_id",
//                table: "check_list_sites");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_check_list_sites_sites_site_id",
//                table: "check_list_sites");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_check_lists_check_lists_parent_id",
//                table: "check_lists");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_fields_check_lists_check_list_id",
//                table: "fields");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_fields_field_types_field_type_id",
//                table: "fields");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_fields_fields_parent_field_id",
//                table: "fields");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_folders_folders_parentId",
//                table: "folders");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_language_versions_languages_languageId",
//                table: "language_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_option_versions_options_optionId",
//                table: "option_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_options_questions_questionId",
//                table: "options");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_question_set_versions_question_sets_questionSetId",
//                table: "question_set_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_question_versions_questions_questionId",
//                table: "question_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_questions_question_sets_questionSetId",
//                table: "questions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_site_survey_configuration_versions_site_survey_configurations_siteSurveyConfigurationId",
//                table: "site_survey_configuration_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_site_survey_configurations_sites_siteId",
//                table: "site_survey_configurations");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_site_survey_configurations_survey_configurations_surveyConfigurationId",
//                table: "site_survey_configurations");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_site_workers_sites_site_id",
//                table: "site_workers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_site_workers_workers_worker_id",
//                table: "site_workers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_survey_configuration_versions_survey_configurations_surveyConfigurationId",
//                table: "survey_configuration_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_taggings_check_lists_check_list_id",
//                table: "taggings");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_taggings_tags_tag_id",
//                table: "taggings");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_units_sites_site_id",
//                table: "units");

//            migrationBuilder.DropIndex(
//                name: "IX_cases_check_list_id",
//                table: "cases");

//            migrationBuilder.DropColumn(
//                name: "microting_uid",
//                table: "folders");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "workers",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "workers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "workers",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "workers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "workers",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "workers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "workers",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "workers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "worker_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "worker_versions",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "worker_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "worker_id",
                table: "worker_versions",
                newName: "WorkerId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "worker_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "worker_versions",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "worker_versions",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "worker_versions",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "worker_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "uploaded_data_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "local",
                table: "uploaded_data_versions",
                newName: "Local");

            migrationBuilder.RenameColumn(
                name: "extension",
                table: "uploaded_data_versions",
                newName: "Extension");

            migrationBuilder.RenameColumn(
                name: "checksum",
                table: "uploaded_data_versions",
                newName: "Checksum");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "uploaded_data_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "uploader_type",
                table: "uploaded_data_versions",
                newName: "UploaderType");

            migrationBuilder.RenameColumn(
                name: "uploader_id",
                table: "uploaded_data_versions",
                newName: "UploaderId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "uploaded_data_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "transcription_id",
                table: "uploaded_data_versions",
                newName: "TranscriptionId");

            migrationBuilder.RenameColumn(
                name: "file_name",
                table: "uploaded_data_versions",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "file_location",
                table: "uploaded_data_versions",
                newName: "FileLocation");

            migrationBuilder.RenameColumn(
                name: "expiration_date",
                table: "uploaded_data_versions",
                newName: "ExpirationDate");

            migrationBuilder.RenameColumn(
                name: "data_uploaded_id",
                table: "uploaded_data_versions",
                newName: "DataUploadedId");

            migrationBuilder.RenameColumn(
                name: "current_file",
                table: "uploaded_data_versions",
                newName: "CurrentFile");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "uploaded_data_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "uploaded_data",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "local",
                table: "uploaded_data",
                newName: "Local");

            migrationBuilder.RenameColumn(
                name: "extension",
                table: "uploaded_data",
                newName: "Extension");

            migrationBuilder.RenameColumn(
                name: "checksum",
                table: "uploaded_data",
                newName: "Checksum");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "uploaded_data",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "uploader_type",
                table: "uploaded_data",
                newName: "UploaderType");

            migrationBuilder.RenameColumn(
                name: "uploader_id",
                table: "uploaded_data",
                newName: "UploaderId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "uploaded_data",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "transcription_id",
                table: "uploaded_data",
                newName: "TranscriptionId");

            migrationBuilder.RenameColumn(
                name: "file_name",
                table: "uploaded_data",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "file_location",
                table: "uploaded_data",
                newName: "FileLocation");

            migrationBuilder.RenameColumn(
                name: "expiration_date",
                table: "uploaded_data",
                newName: "ExpirationDate");

            migrationBuilder.RenameColumn(
                name: "current_file",
                table: "uploaded_data",
                newName: "CurrentFile");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "uploaded_data",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "units",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "units",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "units",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "site_id",
                table: "units",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "otp_code",
                table: "units",
                newName: "OtpCode");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "units",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "customer_no",
                table: "units",
                newName: "CustomerNo");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "units",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_units_site_id",
//                table: "units",
//                newName: "IX_units_SiteId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "unit_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "unit_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "unit_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_id",
                table: "unit_versions",
                newName: "UnitId");

            migrationBuilder.RenameColumn(
                name: "site_id",
                table: "unit_versions",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "otp_code",
                table: "unit_versions",
                newName: "OtpCode");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "unit_versions",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "customer_no",
                table: "unit_versions",
                newName: "CustomerNo");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "unit_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "tags",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "tags",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "tags",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "tags",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "taggings_count",
                table: "tags",
                newName: "TaggingsCount");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "tags",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "taggings",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "taggings",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "taggings",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tagger_id",
                table: "taggings",
                newName: "TaggerId");

            migrationBuilder.RenameColumn(
                name: "tag_id",
                table: "taggings",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "taggings",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "taggings",
                newName: "CheckListId");

//            migrationBuilder.RenameIndex(
//                name: "IX_taggings_tag_id",
//                table: "taggings",
//                newName: "IX_taggings_TagId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_taggings_check_list_id",
//                table: "taggings",
//                newName: "IX_taggings_CheckListId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "tagging_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "tagging_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "tagging_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tagging_id",
                table: "tagging_versions",
                newName: "TaggingId");

            migrationBuilder.RenameColumn(
                name: "tagger_id",
                table: "tagging_versions",
                newName: "TaggerId");

            migrationBuilder.RenameColumn(
                name: "tag_id",
                table: "tagging_versions",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "tagging_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "tagging_versions",
                newName: "CheckListId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "tag_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "tag_versions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "tag_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "tag_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "taggings_count",
                table: "tag_versions",
                newName: "TaggingsCount");

            migrationBuilder.RenameColumn(
                name: "tag_id",
                table: "tag_versions",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "tag_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "survey_configurations",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "timeToLive",
                table: "survey_configurations",
                newName: "TimeToLive");

            migrationBuilder.RenameColumn(
                name: "timeOut",
                table: "survey_configurations",
                newName: "TimeOut");

            migrationBuilder.RenameColumn(
                name: "stop",
                table: "survey_configurations",
                newName: "Stop");

            migrationBuilder.RenameColumn(
                name: "start",
                table: "survey_configurations",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "survey_configurations",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "survey_configurations",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "survey_configurations",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "survey_configurations",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "survey_configuration_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "timeToLive",
                table: "survey_configuration_versions",
                newName: "TimeToLive");

            migrationBuilder.RenameColumn(
                name: "timeOut",
                table: "survey_configuration_versions",
                newName: "TimeOut");

            migrationBuilder.RenameColumn(
                name: "surveyConfigurationId",
                table: "survey_configuration_versions",
                newName: "SurveyConfigurationId");

            migrationBuilder.RenameColumn(
                name: "stop",
                table: "survey_configuration_versions",
                newName: "Stop");

            migrationBuilder.RenameColumn(
                name: "start",
                table: "survey_configuration_versions",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "survey_configuration_versions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "survey_configuration_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "survey_configuration_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "survey_configuration_versions",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_survey_configuration_versions_surveyConfigurationId",
//                table: "survey_configuration_versions",
//                newName: "IX_survey_configuration_versions_SurveyConfigurationId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "sites",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "sites",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "sites",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "sites",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "sites",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "sites",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "site_workers",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "site_workers",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "worker_id",
                table: "site_workers",
                newName: "WorkerId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "site_workers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "site_id",
                table: "site_workers",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "site_workers",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "site_workers",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_site_workers_worker_id",
//                table: "site_workers",
//                newName: "IX_site_workers_WorkerId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_site_workers_site_id",
//                table: "site_workers",
//                newName: "IX_site_workers_SiteId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "site_worker_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "site_worker_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "worker_id",
                table: "site_worker_versions",
                newName: "WorkerId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "site_worker_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "site_worker_id",
                table: "site_worker_versions",
                newName: "SiteWorkerId");

            migrationBuilder.RenameColumn(
                name: "site_id",
                table: "site_worker_versions",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "site_worker_versions",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "site_worker_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "site_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "site_versions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "site_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "site_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "site_id",
                table: "site_versions",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "site_versions",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "site_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "site_survey_configurations",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "surveyConfigurationId",
                table: "site_survey_configurations",
                newName: "SurveyConfigurationId");

            migrationBuilder.RenameColumn(
                name: "siteId",
                table: "site_survey_configurations",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "site_survey_configurations",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "site_survey_configurations",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "site_survey_configurations",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_site_survey_configurations_surveyConfigurationId",
//                table: "site_survey_configurations",
//                newName: "IX_site_survey_configurations_SurveyConfigurationId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_site_survey_configurations_siteId",
//                table: "site_survey_configurations",
//                newName: "IX_site_survey_configurations_SiteId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "site_survey_configuration_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "surveyConfigurationId",
                table: "site_survey_configuration_versions",
                newName: "SurveyConfigurationId");

            migrationBuilder.RenameColumn(
                name: "siteSurveyConfigurationId",
                table: "site_survey_configuration_versions",
                newName: "SiteSurveyConfigurationId");

            migrationBuilder.RenameColumn(
                name: "siteId",
                table: "site_survey_configuration_versions",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "site_survey_configuration_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "site_survey_configuration_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "site_survey_configuration_versions",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_site_survey_configuration_versions_siteSurveyConfigurationId",
//                table: "site_survey_configuration_versions",
//                newName: "IX_site_survey_configuration_versions_SiteSurveyConfigurationId");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "settings",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "settings",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "settings",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "questions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "validDisplay",
                table: "questions",
                newName: "ValidDisplay");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "questions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "refId",
                table: "questions",
                newName: "RefId");

            migrationBuilder.RenameColumn(
                name: "questionType",
                table: "questions",
                newName: "QuestionType");

            migrationBuilder.RenameColumn(
                name: "questionSetId",
                table: "questions",
                newName: "QuestionSetId");

            migrationBuilder.RenameColumn(
                name: "questionIndex",
                table: "questions",
                newName: "QuestionIndex");

            migrationBuilder.RenameColumn(
                name: "prioritised",
                table: "questions",
                newName: "Prioritised");

            migrationBuilder.RenameColumn(
                name: "minimum",
                table: "questions",
                newName: "Minimum");

            migrationBuilder.RenameColumn(
                name: "minDuration",
                table: "questions",
                newName: "MinDuration");

            migrationBuilder.RenameColumn(
                name: "maximum",
                table: "questions",
                newName: "Maximum");

            migrationBuilder.RenameColumn(
                name: "maxDuration",
                table: "questions",
                newName: "MaxDuration");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "questions",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "fontSize",
                table: "questions",
                newName: "FontSize");

            migrationBuilder.RenameColumn(
                name: "continuousQuestionId",
                table: "questions",
                newName: "ContinuousQuestionId");

            migrationBuilder.RenameColumn(
                name: "backButtonEnabled",
                table: "questions",
                newName: "BackButtonEnabled");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "questions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "questions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "imagePostion",
                table: "questions",
                newName: "ImagePosition");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "questions",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_questions_questionSetId",
//                table: "questions",
//                newName: "IX_questions_QuestionSetId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "question_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "validDisplay",
                table: "question_versions",
                newName: "ValidDisplay");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "question_versions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "refId",
                table: "question_versions",
                newName: "RefId");

            migrationBuilder.RenameColumn(
                name: "questionType",
                table: "question_versions",
                newName: "QuestionType");

            migrationBuilder.RenameColumn(
                name: "questionSetId",
                table: "question_versions",
                newName: "QuestionSetId");

            migrationBuilder.RenameColumn(
                name: "questionIndex",
                table: "question_versions",
                newName: "QuestionIndex");

            migrationBuilder.RenameColumn(
                name: "questionId",
                table: "question_versions",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "prioritised",
                table: "question_versions",
                newName: "Prioritised");

            migrationBuilder.RenameColumn(
                name: "minimum",
                table: "question_versions",
                newName: "Minimum");

            migrationBuilder.RenameColumn(
                name: "minDuration",
                table: "question_versions",
                newName: "MinDuration");

            migrationBuilder.RenameColumn(
                name: "maximum",
                table: "question_versions",
                newName: "Maximum");

            migrationBuilder.RenameColumn(
                name: "maxDuration",
                table: "question_versions",
                newName: "MaxDuration");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "question_versions",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "fontSize",
                table: "question_versions",
                newName: "FontSize");

            migrationBuilder.RenameColumn(
                name: "continuousQuestionId",
                table: "question_versions",
                newName: "ContinuousQuestionId");

            migrationBuilder.RenameColumn(
                name: "backButtonEnabled",
                table: "question_versions",
                newName: "BackButtonEnabled");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "question_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "question_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "imagePostion",
                table: "question_versions",
                newName: "ImagePosition");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "question_versions",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_question_versions_questionId",
//                table: "question_versions",
//                newName: "IX_question_versions_QuestionId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "question_sets",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "share",
                table: "question_sets",
                newName: "Share");

            migrationBuilder.RenameColumn(
                name: "posiblyDeployed",
                table: "question_sets",
                newName: "PosiblyDeployed");

            migrationBuilder.RenameColumn(
                name: "parentId",
                table: "question_sets",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "question_sets",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "hasChild",
                table: "question_sets",
                newName: "HasChild");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "question_sets",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "question_sets",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "question_sets",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "question_set_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "share",
                table: "question_set_versions",
                newName: "Share");

            migrationBuilder.RenameColumn(
                name: "questionSetId",
                table: "question_set_versions",
                newName: "QuestionSetId");

            migrationBuilder.RenameColumn(
                name: "parentId",
                table: "question_set_versions",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "question_set_versions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "hasChild",
                table: "question_set_versions",
                newName: "HasChild");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "question_set_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "question_set_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "posiblyDeployed",
                table: "question_set_versions",
                newName: "PossiblyDeployed");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "question_set_versions",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_question_set_versions_questionSetId",
//                table: "question_set_versions",
//                newName: "IX_question_set_versions_QuestionSetId");

            migrationBuilder.RenameColumn(
                name: "weightValue",
                table: "options",
                newName: "WeightValue");

            migrationBuilder.RenameColumn(
                name: "weight",
                table: "options",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "options",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "questionId",
                table: "options",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "optionsIndex",
                table: "options",
                newName: "OptionsIndex");

            migrationBuilder.RenameColumn(
                name: "nextQuestionId",
                table: "options",
                newName: "NextQuestionId");

            migrationBuilder.RenameColumn(
                name: "continuousOptionId",
                table: "options",
                newName: "ContinuousOptionId");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "options",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "options",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "options",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_options_questionId",
//                table: "options",
//                newName: "IX_options_QuestionId");

            migrationBuilder.RenameColumn(
                name: "weightValue",
                table: "option_versions",
                newName: "WeightValue");

            migrationBuilder.RenameColumn(
                name: "weight",
                table: "option_versions",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "option_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "questionId",
                table: "option_versions",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "optionsIndex",
                table: "option_versions",
                newName: "OptionsIndex");

            migrationBuilder.RenameColumn(
                name: "optionId",
                table: "option_versions",
                newName: "OptionId");

            migrationBuilder.RenameColumn(
                name: "nextQuestionId",
                table: "option_versions",
                newName: "NextQuestionId");

            migrationBuilder.RenameColumn(
                name: "continuousOptionId",
                table: "option_versions",
                newName: "ContinuousOptionId");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "option_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "option_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "option_versions",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_option_versions_optionId",
//                table: "option_versions",
//                newName: "IX_option_versions_OptionId");

            migrationBuilder.RenameColumn(
                name: "transmission",
                table: "notifications",
                newName: "Transmission");

            migrationBuilder.RenameColumn(
                name: "stacktrace",
                table: "notifications",
                newName: "Stacktrace");

            migrationBuilder.RenameColumn(
                name: "exception",
                table: "notifications",
                newName: "Exception");

            migrationBuilder.RenameColumn(
                name: "activity",
                table: "notifications",
                newName: "Activity");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "notifications",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "notifications").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "notifications",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "notifications",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "notification_uid",
                table: "notifications",
                newName: "NotificationUid");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "notifications",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "notifications",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "logs",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "logs",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "logs",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "logs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "logs",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "log_exceptions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "log_exceptions",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "log_exceptions",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "log_exceptions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "log_exceptions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "languages",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "languages",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "languages",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "languages",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "languages",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "languages",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "language_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "language_versions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "languageId",
                table: "language_versions",
                newName: "LanguageId");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "language_versions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "language_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "language_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "language_versions",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_language_versions_languageId",
//                table: "language_versions",
//                newName: "IX_language_versions_LanguageId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "folders",
                newName: "Version");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "folders");

            migrationBuilder.RenameColumn(
                name: "parentId",
                table: "folders",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "folders",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "folders",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "folders",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "folders",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "folders",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "folders",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_folders_parentId",
//                table: "folders",
//                newName: "IX_folders_ParentId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "folder_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "folder_versions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "folder_versions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "folder_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "folder_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "folder_versions",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "folder_versions",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "folder_id",
                table: "folder_versions",
                newName: "FolderId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "folder_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "fields",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "selected",
                table: "fields",
                newName: "Selected");

            migrationBuilder.RenameColumn(
                name: "optional",
                table: "fields",
                newName: "Optional");

            migrationBuilder.RenameColumn(
                name: "multi",
                table: "fields",
                newName: "Multi");

            migrationBuilder.RenameColumn(
                name: "mandatory",
                table: "fields",
                newName: "Mandatory");

            migrationBuilder.RenameColumn(
                name: "label",
                table: "fields",
                newName: "Label");

            migrationBuilder.RenameColumn(
                name: "dummy",
                table: "fields",
                newName: "Dummy");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "fields",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "custom",
                table: "fields",
                newName: "Custom");

            migrationBuilder.RenameColumn(
                name: "color",
                table: "fields",
                newName: "Color");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "fields",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "fields",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_name",
                table: "fields",
                newName: "UnitName");

            migrationBuilder.RenameColumn(
                name: "stop_on_save",
                table: "fields",
                newName: "StopOnSave");

            migrationBuilder.RenameColumn(
                name: "split_screen",
                table: "fields",
                newName: "SplitScreen");

            migrationBuilder.RenameColumn(
                name: "read_only",
                table: "fields",
                newName: "ReadOnly");

            migrationBuilder.RenameColumn(
                name: "query_type",
                table: "fields",
                newName: "QueryType");

            migrationBuilder.RenameColumn(
                name: "parent_field_id",
                table: "fields",
                newName: "ParentFieldId");

            migrationBuilder.RenameColumn(
                name: "original_id",
                table: "fields",
                newName: "OriginalId");

            migrationBuilder.RenameColumn(
                name: "min_value",
                table: "fields",
                newName: "MinValue");

            migrationBuilder.RenameColumn(
                name: "max_value",
                table: "fields",
                newName: "MaxValue");

            migrationBuilder.RenameColumn(
                name: "max_length",
                table: "fields",
                newName: "MaxLength");

            migrationBuilder.RenameColumn(
                name: "key_value_pair_list",
                table: "fields",
                newName: "KeyValuePairList");

            migrationBuilder.RenameColumn(
                name: "is_num",
                table: "fields",
                newName: "IsNum");

            migrationBuilder.RenameColumn(
                name: "geolocation_hidden",
                table: "fields",
                newName: "GeolocationHidden");

            migrationBuilder.RenameColumn(
                name: "geolocation_forced",
                table: "fields",
                newName: "GeolocationForced");

            migrationBuilder.RenameColumn(
                name: "geolocation_enabled",
                table: "fields",
                newName: "GeolocationEnabled");

            migrationBuilder.RenameColumn(
                name: "field_type_id",
                table: "fields",
                newName: "FieldTypeId");

            migrationBuilder.RenameColumn(
                name: "entity_group_id",
                table: "fields",
                newName: "EntityGroupId");

            migrationBuilder.RenameColumn(
                name: "display_index",
                table: "fields",
                newName: "DisplayIndex");

            migrationBuilder.RenameColumn(
                name: "default_value",
                table: "fields",
                newName: "DefaultValue");

            migrationBuilder.RenameColumn(
                name: "decimal_count",
                table: "fields",
                newName: "DecimalCount");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "fields",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "fields",
                newName: "CheckListId");

            migrationBuilder.RenameColumn(
                name: "barcode_type",
                table: "fields",
                newName: "BarcodeType");

            migrationBuilder.RenameColumn(
                name: "barcode_enabled",
                table: "fields",
                newName: "BarcodeEnabled");

//            migrationBuilder.RenameIndex(
//                name: "IX_fields_parent_field_id",
//                table: "fields",
//                newName: "IX_fields_ParentFieldId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_fields_field_type_id",
//                table: "fields",
//                newName: "IX_fields_FieldTypeId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_fields_check_list_id",
//                table: "fields",
//                newName: "IX_fields_CheckListId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "field_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "selected",
                table: "field_versions",
                newName: "Selected");

            migrationBuilder.RenameColumn(
                name: "optional",
                table: "field_versions",
                newName: "Optional");

            migrationBuilder.RenameColumn(
                name: "multi",
                table: "field_versions",
                newName: "Multi");

            migrationBuilder.RenameColumn(
                name: "mandatory",
                table: "field_versions",
                newName: "Mandatory");

            migrationBuilder.RenameColumn(
                name: "label",
                table: "field_versions",
                newName: "Label");

            migrationBuilder.RenameColumn(
                name: "dummy",
                table: "field_versions",
                newName: "Dummy");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "field_versions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "custom",
                table: "field_versions",
                newName: "Custom");

            migrationBuilder.RenameColumn(
                name: "color",
                table: "field_versions",
                newName: "Color");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "field_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "field_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_name",
                table: "field_versions",
                newName: "UnitName");

            migrationBuilder.RenameColumn(
                name: "stop_on_save",
                table: "field_versions",
                newName: "StopOnSave");

            migrationBuilder.RenameColumn(
                name: "split_screen",
                table: "field_versions",
                newName: "SplitScreen");

            migrationBuilder.RenameColumn(
                name: "read_only",
                table: "field_versions",
                newName: "ReadOnly");

            migrationBuilder.RenameColumn(
                name: "query_type",
                table: "field_versions",
                newName: "QueryType");

            migrationBuilder.RenameColumn(
                name: "parent_field_id",
                table: "field_versions",
                newName: "ParentFieldId");

            migrationBuilder.RenameColumn(
                name: "original_id",
                table: "field_versions",
                newName: "OriginalId");

            migrationBuilder.RenameColumn(
                name: "min_value",
                table: "field_versions",
                newName: "MinValue");

            migrationBuilder.RenameColumn(
                name: "max_value",
                table: "field_versions",
                newName: "MaxValue");

            migrationBuilder.RenameColumn(
                name: "max_length",
                table: "field_versions",
                newName: "MaxLength");

            migrationBuilder.RenameColumn(
                name: "key_value_pair_list",
                table: "field_versions",
                newName: "KeyValuePairList");

            migrationBuilder.RenameColumn(
                name: "is_num",
                table: "field_versions",
                newName: "IsNum");

            migrationBuilder.RenameColumn(
                name: "geolocation_hidden",
                table: "field_versions",
                newName: "GeolocationHidden");

            migrationBuilder.RenameColumn(
                name: "geolocation_forced",
                table: "field_versions",
                newName: "GeolocationForced");

            migrationBuilder.RenameColumn(
                name: "geolocation_enabled",
                table: "field_versions",
                newName: "GeolocationEnabled");

            migrationBuilder.RenameColumn(
                name: "field_type_id",
                table: "field_versions",
                newName: "FieldTypeId");

            migrationBuilder.RenameColumn(
                name: "field_id",
                table: "field_versions",
                newName: "FieldId");

            migrationBuilder.RenameColumn(
                name: "entity_group_id",
                table: "field_versions",
                newName: "EntityGroupId");

            migrationBuilder.RenameColumn(
                name: "display_index",
                table: "field_versions",
                newName: "DisplayIndex");

            migrationBuilder.RenameColumn(
                name: "default_value",
                table: "field_versions",
                newName: "DefaultValue");

            migrationBuilder.RenameColumn(
                name: "decimal_count",
                table: "field_versions",
                newName: "DecimalCount");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "field_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "field_versions",
                newName: "CheckListId");

            migrationBuilder.RenameColumn(
                name: "barcode_type",
                table: "field_versions",
                newName: "BarcodeType");

            migrationBuilder.RenameColumn(
                name: "barcode_enabled",
                table: "field_versions",
                newName: "BarcodeEnabled");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "field_values",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "field_values",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "field_values",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "field_values",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "field_value_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "field_value_versions",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "field_value_versions",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "field_value_versions",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "heading",
                table: "field_value_versions",
                newName: "Heading");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "field_value_versions",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "altitude",
                table: "field_value_versions",
                newName: "Altitude");

            migrationBuilder.RenameColumn(
                name: "accuracy",
                table: "field_value_versions",
                newName: "Accuracy");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "field_value_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "field_value_versions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "uploaded_data_id",
                table: "field_value_versions",
                newName: "UploadedDataId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "field_value_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "field_value_id",
                table: "field_value_versions",
                newName: "FieldValueId");

            migrationBuilder.RenameColumn(
                name: "field_id",
                table: "field_value_versions",
                newName: "FieldId");

            migrationBuilder.RenameColumn(
                name: "done_at",
                table: "field_value_versions",
                newName: "DoneAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "field_value_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "field_value_versions",
                newName: "CheckListId");

            migrationBuilder.RenameColumn(
                name: "check_list_duplicate_id",
                table: "field_value_versions",
                newName: "CheckListDuplicateId");

            migrationBuilder.RenameColumn(
                name: "case_id",
                table: "field_value_versions",
                newName: "CaseId");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "field_types",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "field_type",
                table: "field_types",
                newName: "FieldType");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "entity_items",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "synced",
                table: "entity_items",
                newName: "Synced");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "entity_items",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "entity_items",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "entity_items",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "entity_items",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "entity_items",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "entity_item_uid",
                table: "entity_items",
                newName: "EntityItemUid");

            migrationBuilder.RenameColumn(
                name: "entity_group_id",
                table: "entity_items",
                newName: "EntityGroupId");

            migrationBuilder.RenameColumn(
                name: "display_index",
                table: "entity_items",
                newName: "DisplayIndex");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "entity_items",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "entity_item_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "synced",
                table: "entity_item_versions",
                newName: "Synced");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "entity_item_versions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "entity_item_versions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "entity_item_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "entity_item_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "entity_item_versions",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "entity_items_id",
                table: "entity_item_versions",
                newName: "EntityItemsId");

            migrationBuilder.RenameColumn(
                name: "entity_item_uid",
                table: "entity_item_versions",
                newName: "EntityItemUid");

            migrationBuilder.RenameColumn(
                name: "entity_group_id",
                table: "entity_item_versions",
                newName: "EntityGroupId");

            migrationBuilder.RenameColumn(
                name: "display_index",
                table: "entity_item_versions",
                newName: "DisplayIndex");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "entity_item_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "entity_groups",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "entity_groups",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "entity_groups",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "entity_groups",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "entity_groups",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "entity_groups",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "entity_groups",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "entity_group_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "entity_group_versions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "entity_group_versions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "entity_group_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "entity_group_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "entity_group_versions",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "entity_group_id",
                table: "entity_group_versions",
                newName: "EntityGroupId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "entity_group_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "check_lists",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "repeated",
                table: "check_lists",
                newName: "Repeated");

            migrationBuilder.RenameColumn(
                name: "label",
                table: "check_lists",
                newName: "Label");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "check_lists",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "custom",
                table: "check_lists",
                newName: "Custom");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "check_lists",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "check_lists",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "review_enabled",
                table: "check_lists",
                newName: "ReviewEnabled");

            migrationBuilder.RenameColumn(
                name: "quick_sync_enabled",
                table: "check_lists",
                newName: "QuickSyncEnabled");

            migrationBuilder.DropColumn(
                name: "parentid",
                table: "check_lists");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "check_lists",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "original_id",
                table: "check_lists",
                newName: "OriginalId");

            migrationBuilder.RenameColumn(
                name: "multi_approval",
                table: "check_lists",
                newName: "MultiApproval");

            migrationBuilder.RenameColumn(
                name: "manual_sync",
                table: "check_lists",
                newName: "ManualSync");

            migrationBuilder.RenameColumn(
                name: "folder_name",
                table: "check_lists",
                newName: "FolderName");

            migrationBuilder.RenameColumn(
                name: "field_9",
                table: "check_lists",
                newName: "Field9");

            migrationBuilder.RenameColumn(
                name: "field_8",
                table: "check_lists",
                newName: "Field8");

            migrationBuilder.RenameColumn(
                name: "field_7",
                table: "check_lists",
                newName: "Field7");

            migrationBuilder.RenameColumn(
                name: "field_6",
                table: "check_lists",
                newName: "Field6");

            migrationBuilder.RenameColumn(
                name: "field_5",
                table: "check_lists",
                newName: "Field5");

            migrationBuilder.RenameColumn(
                name: "field_4",
                table: "check_lists",
                newName: "Field4");

            migrationBuilder.RenameColumn(
                name: "field_3",
                table: "check_lists",
                newName: "Field3");

            migrationBuilder.RenameColumn(
                name: "field_2",
                table: "check_lists",
                newName: "Field2");

            migrationBuilder.RenameColumn(
                name: "field_10",
                table: "check_lists",
                newName: "Field10");

            migrationBuilder.RenameColumn(
                name: "field_1",
                table: "check_lists",
                newName: "Field1");

            migrationBuilder.RenameColumn(
                name: "fast_navigation",
                table: "check_lists",
                newName: "FastNavigation");

            migrationBuilder.RenameColumn(
                name: "extra_fields_enabled",
                table: "check_lists",
                newName: "ExtraFieldsEnabled");

            migrationBuilder.RenameColumn(
                name: "download_entities",
                table: "check_lists",
                newName: "DownloadEntities");

            migrationBuilder.RenameColumn(
                name: "done_button_enabled",
                table: "check_lists",
                newName: "DoneButtonEnabled");

            migrationBuilder.RenameColumn(
                name: "display_index",
                table: "check_lists",
                newName: "DisplayIndex");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "check_lists",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "case_type",
                table: "check_lists",
                newName: "CaseType");

            migrationBuilder.RenameColumn(
                name: "approval_enabled",
                table: "check_lists",
                newName: "ApprovalEnabled");

//            migrationBuilder.RenameIndex(
//                name: "IX_check_lists_parent_id",
//                table: "check_lists",
//                newName: "IX_check_lists_ParentId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "check_list_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "repeated",
                table: "check_list_versions",
                newName: "Repeated");

            migrationBuilder.RenameColumn(
                name: "label",
                table: "check_list_versions",
                newName: "Label");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "check_list_versions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "custom",
                table: "check_list_versions",
                newName: "Custom");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "check_list_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "check_list_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "review_enabled",
                table: "check_list_versions",
                newName: "ReviewEnabled");

            migrationBuilder.RenameColumn(
                name: "quick_sync_enabled",
                table: "check_list_versions",
                newName: "QuickSyncEnabled");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "check_list_versions",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "original_id",
                table: "check_list_versions",
                newName: "OriginalId");

            migrationBuilder.RenameColumn(
                name: "multi_approval",
                table: "check_list_versions",
                newName: "MultiApproval");

            migrationBuilder.RenameColumn(
                name: "manual_sync",
                table: "check_list_versions",
                newName: "ManualSync");

            migrationBuilder.RenameColumn(
                name: "folder_name",
                table: "check_list_versions",
                newName: "FolderName");

            migrationBuilder.RenameColumn(
                name: "field_9",
                table: "check_list_versions",
                newName: "Field9");

            migrationBuilder.RenameColumn(
                name: "field_8",
                table: "check_list_versions",
                newName: "Field8");

            migrationBuilder.RenameColumn(
                name: "field_7",
                table: "check_list_versions",
                newName: "Field7");

            migrationBuilder.RenameColumn(
                name: "field_6",
                table: "check_list_versions",
                newName: "Field6");

            migrationBuilder.RenameColumn(
                name: "field_5",
                table: "check_list_versions",
                newName: "Field5");

            migrationBuilder.RenameColumn(
                name: "field_4",
                table: "check_list_versions",
                newName: "Field4");

            migrationBuilder.RenameColumn(
                name: "field_3",
                table: "check_list_versions",
                newName: "Field3");

            migrationBuilder.RenameColumn(
                name: "field_2",
                table: "check_list_versions",
                newName: "Field2");

            migrationBuilder.RenameColumn(
                name: "field_10",
                table: "check_list_versions",
                newName: "Field10");

            migrationBuilder.RenameColumn(
                name: "field_1",
                table: "check_list_versions",
                newName: "Field1");

            migrationBuilder.RenameColumn(
                name: "fast_navigation",
                table: "check_list_versions",
                newName: "FastNavigation");

            migrationBuilder.RenameColumn(
                name: "extra_fields_enabled",
                table: "check_list_versions",
                newName: "ExtraFieldsEnabled");

            migrationBuilder.RenameColumn(
                name: "download_entities",
                table: "check_list_versions",
                newName: "DownloadEntities");

            migrationBuilder.RenameColumn(
                name: "done_button_enabled",
                table: "check_list_versions",
                newName: "DoneButtonEnabled");

            migrationBuilder.RenameColumn(
                name: "display_index",
                table: "check_list_versions",
                newName: "DisplayIndex");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "check_list_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "check_list_versions",
                newName: "CheckListId");

            migrationBuilder.RenameColumn(
                name: "case_type",
                table: "check_list_versions",
                newName: "CaseType");

            migrationBuilder.RenameColumn(
                name: "approval_enabled",
                table: "check_list_versions",
                newName: "ApprovalEnabled");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "check_list_values",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "check_list_values",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "check_list_values",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "check_list_values",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "check_list_values",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "check_list_values",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "check_list_values",
                newName: "CheckListId");

            migrationBuilder.RenameColumn(
                name: "check_list_duplicate_id",
                table: "check_list_values",
                newName: "CheckListDuplicateId");

            migrationBuilder.RenameColumn(
                name: "case_id",
                table: "check_list_values",
                newName: "CaseId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "check_list_value_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "check_list_value_versions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "check_list_value_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "check_list_value_versions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "check_list_value_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "check_list_value_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "check_list_value_id",
                table: "check_list_value_versions",
                newName: "CheckListValueId");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "check_list_value_versions",
                newName: "CheckListId");

            migrationBuilder.RenameColumn(
                name: "check_list_duplicate_id",
                table: "check_list_value_versions",
                newName: "CheckListDuplicateId");

            migrationBuilder.RenameColumn(
                name: "case_id",
                table: "check_list_value_versions",
                newName: "CaseId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "check_list_sites",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "check_list_sites",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "check_list_sites",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "site_id",
                table: "check_list_sites",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "check_list_sites",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "last_check_id",
                table: "check_list_sites",
                newName: "LastCheckId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "check_list_sites",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "check_list_sites",
                newName: "CheckListId");

//            migrationBuilder.RenameIndex(
//                name: "IX_check_list_sites_site_id",
//                table: "check_list_sites",
//                newName: "IX_check_list_sites_SiteId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_check_list_sites_check_list_id",
//                table: "check_list_sites",
//                newName: "IX_check_list_sites_CheckListId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "check_list_site_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "check_list_site_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "check_list_site_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "site_id",
                table: "check_list_site_versions",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "check_list_site_versions",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "last_check_id",
                table: "check_list_site_versions",
                newName: "LastCheckId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "check_list_site_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "check_list_site_id",
                table: "check_list_site_versions",
                newName: "CheckListSiteId");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "check_list_site_versions",
                newName: "CheckListId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "cases",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "cases",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "cases",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "custom",
                table: "cases",
                newName: "Custom");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "cases",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "cases",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_id",
                table: "cases",
                newName: "UnitId");

            migrationBuilder.RenameColumn(
                name: "site_id",
                table: "cases",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "cases",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "microting_check_uid",
                table: "cases",
                newName: "MicrotingCheckUid");

            migrationBuilder.RenameColumn(
                name: "field_value_9",
                table: "cases",
                newName: "FieldValue9");

            migrationBuilder.RenameColumn(
                name: "field_value_8",
                table: "cases",
                newName: "FieldValue8");

            migrationBuilder.RenameColumn(
                name: "field_value_7",
                table: "cases",
                newName: "FieldValue7");

            migrationBuilder.RenameColumn(
                name: "field_value_6",
                table: "cases",
                newName: "FieldValue6");

            migrationBuilder.RenameColumn(
                name: "field_value_5",
                table: "cases",
                newName: "FieldValue5");

            migrationBuilder.RenameColumn(
                name: "field_value_4",
                table: "cases",
                newName: "FieldValue4");

            migrationBuilder.RenameColumn(
                name: "field_value_3",
                table: "cases",
                newName: "FieldValue3");

            migrationBuilder.RenameColumn(
                name: "field_value_2",
                table: "cases",
                newName: "FieldValue2");

            migrationBuilder.RenameColumn(
                name: "field_value_10",
                table: "cases",
                newName: "FieldValue10");

            migrationBuilder.RenameColumn(
                name: "field_value_1",
                table: "cases",
                newName: "FieldValue1");

            migrationBuilder.RenameColumn(
                name: "done_by_user_id",
                table: "cases",
                newName: "WorkerId");

            migrationBuilder.RenameColumn(
                name: "done_at",
                table: "cases",
                newName: "DoneAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "cases",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "cases",
                newName: "CheckListId");

            migrationBuilder.RenameColumn(
                name: "case_uid",
                table: "cases",
                newName: "CaseUid");

//            migrationBuilder.RenameIndex(
//                name: "IX_cases_unit_id",
//                table: "cases",
//                newName: "IX_cases_WorkerId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_cases_site_id",
//                table: "cases",
//                newName: "IX_cases_UnitId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_cases_done_by_user_id",
//                table: "cases",
//                newName: "IX_cases_SiteId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "case_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "case_versions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "case_versions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "custom",
                table: "case_versions",
                newName: "Custom");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "case_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "case_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_id",
                table: "case_versions",
                newName: "UnitId");

            migrationBuilder.RenameColumn(
                name: "site_id",
                table: "case_versions",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "microting_uid",
                table: "case_versions",
                newName: "MicrotingUid");

            migrationBuilder.RenameColumn(
                name: "microting_check_uid",
                table: "case_versions",
                newName: "MicrotingCheckUid");

            migrationBuilder.RenameColumn(
                name: "field_value_9",
                table: "case_versions",
                newName: "FieldValue9");

            migrationBuilder.RenameColumn(
                name: "field_value_8",
                table: "case_versions",
                newName: "FieldValue8");

            migrationBuilder.RenameColumn(
                name: "field_value_7",
                table: "case_versions",
                newName: "FieldValue7");

            migrationBuilder.RenameColumn(
                name: "field_value_6",
                table: "case_versions",
                newName: "FieldValue6");

            migrationBuilder.RenameColumn(
                name: "field_value_5",
                table: "case_versions",
                newName: "FieldValue5");

            migrationBuilder.RenameColumn(
                name: "field_value_4",
                table: "case_versions",
                newName: "FieldValue4");

            migrationBuilder.RenameColumn(
                name: "field_value_3",
                table: "case_versions",
                newName: "FieldValue3");

            migrationBuilder.RenameColumn(
                name: "field_value_2",
                table: "case_versions",
                newName: "FieldValue2");

            migrationBuilder.RenameColumn(
                name: "field_value_10",
                table: "case_versions",
                newName: "FieldValue10");

            migrationBuilder.RenameColumn(
                name: "field_value_1",
                table: "case_versions",
                newName: "FieldValue1");

            migrationBuilder.RenameColumn(
                name: "done_by_user_id",
                table: "case_versions",
                newName: "WorkerId");

            migrationBuilder.RenameColumn(
                name: "done_at",
                table: "case_versions",
                newName: "DoneAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "case_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "case_versions",
                newName: "CheckListId");

            migrationBuilder.RenameColumn(
                name: "case_uid",
                table: "case_versions",
                newName: "CaseUid");

            migrationBuilder.RenameColumn(
                name: "case_id",
                table: "case_versions",
                newName: "CaseId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "answers",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "unitId",
                table: "answers",
                newName: "UnitId");

            migrationBuilder.RenameColumn(
                name: "timeZone",
                table: "answers",
                newName: "TimeZone");

            migrationBuilder.RenameColumn(
                name: "surveyConfigurationId",
                table: "answers",
                newName: "SurveyConfigurationId");

            migrationBuilder.RenameColumn(
                name: "siteId",
                table: "answers",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "questionSetId",
                table: "answers",
                newName: "QuestionSetId");

            migrationBuilder.RenameColumn(
                name: "languageId",
                table: "answers",
                newName: "LanguageId");

            migrationBuilder.RenameColumn(
                name: "finishedAt",
                table: "answers",
                newName: "FinishedAt");

            migrationBuilder.RenameColumn(
                name: "answerDuration",
                table: "answers",
                newName: "AnswerDuration");

            migrationBuilder.RenameColumn(
                name: "UTCAdjusted",
                table: "answers",
                newName: "UtcAdjusted");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "answers",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "answers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "answers",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_answers_unitId",
//                table: "answers",
//                newName: "IX_answers_UnitId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_answers_surveyConfigurationId",
//                table: "answers",
//                newName: "IX_answers_SurveyConfigurationId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_answers_siteId",
//                table: "answers",
//                newName: "IX_answers_SiteId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_answers_questionSetId",
//                table: "answers",
//                newName: "IX_answers_QuestionSetId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_answers_languageId",
//                table: "answers",
//                newName: "IX_answers_LanguageId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "answer_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "unitId",
                table: "answer_versions",
                newName: "UnitId");

            migrationBuilder.RenameColumn(
                name: "timeZone",
                table: "answer_versions",
                newName: "TimeZone");

            migrationBuilder.RenameColumn(
                name: "surveyConfigurationId",
                table: "answer_versions",
                newName: "SurveyConfigurationId");

            migrationBuilder.RenameColumn(
                name: "siteId",
                table: "answer_versions",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "questionSetId",
                table: "answer_versions",
                newName: "QuestionSetId");

            migrationBuilder.RenameColumn(
                name: "finishedAt",
                table: "answer_versions",
                newName: "FinishedAt");

            migrationBuilder.RenameColumn(
                name: "answerId",
                table: "answer_versions",
                newName: "AnswerId");

            migrationBuilder.RenameColumn(
                name: "answerDuration",
                table: "answer_versions",
                newName: "AnswerDuration");

            migrationBuilder.RenameColumn(
                name: "UTCAdjusted",
                table: "answer_versions",
                newName: "UtcAdjusted");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "answer_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "answer_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "answer_versions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "languageid",
                table: "answer_versions",
                newName: "LanguageId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "answer_values",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "answer_values",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "questionId",
                table: "answer_values",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "optionsId",
                table: "answer_values",
                newName: "OptionsId");

            migrationBuilder.RenameColumn(
                name: "answerId",
                table: "answer_values",
                newName: "AnswerId");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "answer_values",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "answer_values",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "answer_values",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_answer_values_questionId",
//                table: "answer_values",
//                newName: "IX_answer_values_QuestionId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_answer_values_optionsId",
//                table: "answer_values",
//                newName: "IX_answer_values_OptionsId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_answer_values_answerId",
//                table: "answer_values",
//                newName: "IX_answer_values_AnswerId");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "answer_value_versions",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "answer_value_versions",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "questionId",
                table: "answer_value_versions",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "optionsId",
                table: "answer_value_versions",
                newName: "OptionsId");

            migrationBuilder.RenameColumn(
                name: "answerValueId",
                table: "answer_value_versions",
                newName: "AnswerValueId");

            migrationBuilder.RenameColumn(
                name: "answerId",
                table: "answer_value_versions",
                newName: "AnswerId");

            migrationBuilder.RenameColumn(
                name: "workflow_state",
                table: "answer_value_versions",
                newName: "WorkflowState");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "answer_value_versions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "answer_value_versions",
                newName: "CreatedAt");

//            migrationBuilder.RenameIndex(
//                name: "IX_answer_value_versions_answerValueId",
//                table: "answer_value_versions",
//                newName: "IX_answer_value_versions_AnswerValueId");

//            migrationBuilder.AddColumn<int>(
//                name: "CheckListId",
//                table: "cases",
//                nullable: true);
//
            migrationBuilder.CreateIndex(
                name: "IX_cases_CheckListId",
                table: "cases",
                column: "CheckListId");

            migrationBuilder.AddForeignKey(
                name: "FK_answer_value_versions_answer_values_AnswerValueId",
                table: "answer_value_versions",
                column: "AnswerValueId",
                principalTable: "answer_values",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answer_values_answers_AnswerId",
                table: "answer_values",
                column: "AnswerId",
                principalTable: "answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answer_values_options_OptionsId",
                table: "answer_values",
                column: "OptionsId",
                principalTable: "options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answer_values_questions_QuestionId",
                table: "answer_values",
                column: "QuestionId",
                principalTable: "questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_languages_LanguageId",
                table: "answers",
                column: "LanguageId",
                principalTable: "languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_question_sets_QuestionSetId",
                table: "answers",
                column: "QuestionSetId",
                principalTable: "question_sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_sites_SiteId",
                table: "answers",
                column: "SiteId",
                principalTable: "sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_survey_configurations_SurveyConfigurationId",
                table: "answers",
                column: "SurveyConfigurationId",
                principalTable: "survey_configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_units_UnitId",
                table: "answers",
                column: "UnitId",
                principalTable: "units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cases_check_lists_CheckListId",
                table: "cases",
                column: "CheckListId",
                principalTable: "check_lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_cases_sites_SiteId",
                table: "cases",
                column: "SiteId",
                principalTable: "sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_cases_units_UnitId",
                table: "cases",
                column: "UnitId",
                principalTable: "units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_cases_workers_WorkerId",
                table: "cases",
                column: "WorkerId",
                principalTable: "workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_check_list_sites_check_lists_CheckListId",
                table: "check_list_sites",
                column: "CheckListId",
                principalTable: "check_lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_check_list_sites_sites_SiteId",
                table: "check_list_sites",
                column: "SiteId",
                principalTable: "sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

//            migrationBuilder.AddForeignKey(
//                name: "FK_check_lists_check_lists_ParentId",
//                table: "check_lists",
//                column: "ParentId",
//                principalTable: "check_lists",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_fields_check_lists_CheckListId",
                table: "fields",
                column: "CheckListId",
                principalTable: "check_lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_fields_field_types_FieldTypeId",
                table: "fields",
                column: "FieldTypeId",
                principalTable: "field_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_fields_fields_ParentFieldId",
                table: "fields",
                column: "ParentFieldId",
                principalTable: "fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_folders_folders_ParentId",
                table: "folders",
                column: "ParentId",
                principalTable: "folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_language_versions_languages_LanguageId",
                table: "language_versions",
                column: "LanguageId",
                principalTable: "languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_option_versions_options_OptionId",
                table: "option_versions",
                column: "OptionId",
                principalTable: "options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_options_questions_QuestionId",
                table: "options",
                column: "QuestionId",
                principalTable: "questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_question_set_versions_question_sets_QuestionSetId",
                table: "question_set_versions",
                column: "QuestionSetId",
                principalTable: "question_sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_question_versions_questions_QuestionId",
                table: "question_versions",
                column: "QuestionId",
                principalTable: "questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_questions_question_sets_QuestionSetId",
                table: "questions",
                column: "QuestionSetId",
                principalTable: "question_sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_site_survey_configuration_versions_site_survey_configurations_SiteSurveyConfigurationId",
                table: "site_survey_configuration_versions",
                column: "SiteSurveyConfigurationId",
                principalTable: "site_survey_configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_site_survey_configurations_sites_SiteId",
                table: "site_survey_configurations",
                column: "SiteId",
                principalTable: "sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_site_survey_configurations_survey_configurations_SurveyConfigurationId",
                table: "site_survey_configurations",
                column: "SurveyConfigurationId",
                principalTable: "survey_configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_site_workers_sites_SiteId",
                table: "site_workers",
                column: "SiteId",
                principalTable: "sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_site_workers_workers_WorkerId",
                table: "site_workers",
                column: "WorkerId",
                principalTable: "workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_survey_configuration_versions_survey_configurations_SurveyConfigurationId",
                table: "survey_configuration_versions",
                column: "SurveyConfigurationId",
                principalTable: "survey_configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_taggings_check_lists_CheckListId",
                table: "taggings",
                column: "CheckListId",
                principalTable: "check_lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_taggings_tags_TagId",
                table: "taggings",
                column: "TagId",
                principalTable: "tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_units_sites_SiteId",
                table: "units",
                column: "SiteId",
                principalTable: "sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
//            migrationBuilder.DropForeignKey(
//                name: "FK_answer_value_versions_answer_values_AnswerValueId",
//                table: "answer_value_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answer_values_answers_AnswerId",
//                table: "answer_values");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answer_values_options_OptionsId",
//                table: "answer_values");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answer_values_questions_QuestionId",
//                table: "answer_values");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answers_languages_LanguageId",
//                table: "answers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answers_question_sets_QuestionSetId",
//                table: "answers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answers_sites_SiteId",
//                table: "answers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answers_survey_configurations_SurveyConfigurationId",
//                table: "answers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_answers_units_UnitId",
//                table: "answers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_cases_check_lists_CheckListId",
//                table: "cases");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_cases_sites_SiteId",
//                table: "cases");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_cases_units_UnitId",
//                table: "cases");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_cases_workers_WorkerId",
//                table: "cases");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_check_list_sites_check_lists_CheckListId",
//                table: "check_list_sites");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_check_list_sites_sites_SiteId",
//                table: "check_list_sites");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_check_lists_check_lists_ParentId",
//                table: "check_lists");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_fields_check_lists_CheckListId",
//                table: "fields");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_fields_field_types_FieldTypeId",
//                table: "fields");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_fields_fields_ParentFieldId",
//                table: "fields");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_folders_folders_ParentId",
//                table: "folders");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_language_versions_languages_LanguageId",
//                table: "language_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_option_versions_options_OptionId",
//                table: "option_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_options_questions_QuestionId",
//                table: "options");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_question_set_versions_question_sets_QuestionSetId",
//                table: "question_set_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_question_versions_questions_QuestionId",
//                table: "question_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_questions_question_sets_QuestionSetId",
//                table: "questions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_site_survey_configuration_versions_site_survey_configurations_SiteSurveyConfigurationId",
//                table: "site_survey_configuration_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_site_survey_configurations_sites_SiteId",
//                table: "site_survey_configurations");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_site_survey_configurations_survey_configurations_SurveyConfigurationId",
//                table: "site_survey_configurations");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_site_workers_sites_SiteId",
//                table: "site_workers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_site_workers_workers_WorkerId",
//                table: "site_workers");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_survey_configuration_versions_survey_configurations_SurveyConfigurationId",
//                table: "survey_configuration_versions");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_taggings_check_lists_CheckListId",
//                table: "taggings");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_taggings_tags_TagId",
//                table: "taggings");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_units_sites_SiteId",
//                table: "units");
//
//            migrationBuilder.DropIndex(
//                name: "IX_cases_CheckListId",
//                table: "cases");
//
////            migrationBuilder.DropColumn(
////                name: "CheckListId",
////                table: "cases");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "workers",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Email",
//                table: "workers",
//                newName: "email");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "workers",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "workers",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "workers",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "LastName",
//                table: "workers",
//                newName: "last_name");
//
//            migrationBuilder.RenameColumn(
//                name: "FirstName",
//                table: "workers",
//                newName: "first_name");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "workers",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "worker_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Email",
//                table: "worker_versions",
//                newName: "email");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "worker_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkerId",
//                table: "worker_versions",
//                newName: "worker_id");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "worker_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "worker_versions",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "LastName",
//                table: "worker_versions",
//                newName: "last_name");
//
//            migrationBuilder.RenameColumn(
//                name: "FirstName",
//                table: "worker_versions",
//                newName: "first_name");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "worker_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "uploaded_data_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Local",
//                table: "uploaded_data_versions",
//                newName: "local");
//
//            migrationBuilder.RenameColumn(
//                name: "Extension",
//                table: "uploaded_data_versions",
//                newName: "extension");
//
//            migrationBuilder.RenameColumn(
//                name: "Checksum",
//                table: "uploaded_data_versions",
//                newName: "checksum");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "uploaded_data_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UploaderType",
//                table: "uploaded_data_versions",
//                newName: "uploader_type");
//
//            migrationBuilder.RenameColumn(
//                name: "UploaderId",
//                table: "uploaded_data_versions",
//                newName: "uploader_id");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "uploaded_data_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "TranscriptionId",
//                table: "uploaded_data_versions",
//                newName: "transcription_id");
//
//            migrationBuilder.RenameColumn(
//                name: "FileName",
//                table: "uploaded_data_versions",
//                newName: "file_name");
//
//            migrationBuilder.RenameColumn(
//                name: "FileLocation",
//                table: "uploaded_data_versions",
//                newName: "file_location");
//
//            migrationBuilder.RenameColumn(
//                name: "ExpirationDate",
//                table: "uploaded_data_versions",
//                newName: "expiration_date");
//
//            migrationBuilder.RenameColumn(
//                name: "DataUploadedId",
//                table: "uploaded_data_versions",
//                newName: "data_uploaded_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CurrentFile",
//                table: "uploaded_data_versions",
//                newName: "current_file");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "uploaded_data_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "uploaded_data",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Local",
//                table: "uploaded_data",
//                newName: "local");
//
//            migrationBuilder.RenameColumn(
//                name: "Extension",
//                table: "uploaded_data",
//                newName: "extension");
//
//            migrationBuilder.RenameColumn(
//                name: "Checksum",
//                table: "uploaded_data",
//                newName: "checksum");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "uploaded_data",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UploaderType",
//                table: "uploaded_data",
//                newName: "uploader_type");
//
//            migrationBuilder.RenameColumn(
//                name: "UploaderId",
//                table: "uploaded_data",
//                newName: "uploader_id");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "uploaded_data",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "TranscriptionId",
//                table: "uploaded_data",
//                newName: "transcription_id");
//
//            migrationBuilder.RenameColumn(
//                name: "FileName",
//                table: "uploaded_data",
//                newName: "file_name");
//
//            migrationBuilder.RenameColumn(
//                name: "FileLocation",
//                table: "uploaded_data",
//                newName: "file_location");
//
//            migrationBuilder.RenameColumn(
//                name: "ExpirationDate",
//                table: "uploaded_data",
//                newName: "expiration_date");
//
//            migrationBuilder.RenameColumn(
//                name: "CurrentFile",
//                table: "uploaded_data",
//                newName: "current_file");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "uploaded_data",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "units",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "units",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "units",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "units",
//                newName: "site_id");
//
//            migrationBuilder.RenameColumn(
//                name: "OtpCode",
//                table: "units",
//                newName: "otp_code");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "units",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "CustomerNo",
//                table: "units",
//                newName: "customer_no");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "units",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_units_SiteId",
////                table: "units",
////                newName: "IX_units_site_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "unit_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "unit_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "unit_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "UnitId",
//                table: "unit_versions",
//                newName: "unit_id");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "unit_versions",
//                newName: "site_id");
//
//            migrationBuilder.RenameColumn(
//                name: "OtpCode",
//                table: "unit_versions",
//                newName: "otp_code");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "unit_versions",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "CustomerNo",
//                table: "unit_versions",
//                newName: "customer_no");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "unit_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "tags",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "tags",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "tags",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "tags",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "TaggingsCount",
//                table: "tags",
//                newName: "taggings_count");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "tags",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "taggings",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "taggings",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "taggings",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "TaggerId",
//                table: "taggings",
//                newName: "tagger_id");
//
//            migrationBuilder.RenameColumn(
//                name: "TagId",
//                table: "taggings",
//                newName: "tag_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "taggings",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListId",
//                table: "taggings",
//                newName: "check_list_id");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_taggings_TagId",
////                table: "taggings",
////                newName: "IX_taggings_tag_id");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_taggings_CheckListId",
////                table: "taggings",
////                newName: "IX_taggings_check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "tagging_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "tagging_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "tagging_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "TaggingId",
//                table: "tagging_versions",
//                newName: "tagging_id");
//
//            migrationBuilder.RenameColumn(
//                name: "TaggerId",
//                table: "tagging_versions",
//                newName: "tagger_id");
//
//            migrationBuilder.RenameColumn(
//                name: "TagId",
//                table: "tagging_versions",
//                newName: "tag_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "tagging_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListId",
//                table: "tagging_versions",
//                newName: "check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "tag_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "tag_versions",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "tag_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "tag_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "TaggingsCount",
//                table: "tag_versions",
//                newName: "taggings_count");
//
//            migrationBuilder.RenameColumn(
//                name: "TagId",
//                table: "tag_versions",
//                newName: "tag_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "tag_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "survey_configurations",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "TimeToLive",
//                table: "survey_configurations",
//                newName: "timeToLive");
//
//            migrationBuilder.RenameColumn(
//                name: "TimeOut",
//                table: "survey_configurations",
//                newName: "timeOut");
//
//            migrationBuilder.RenameColumn(
//                name: "Stop",
//                table: "survey_configurations",
//                newName: "stop");
//
//            migrationBuilder.RenameColumn(
//                name: "Start",
//                table: "survey_configurations",
//                newName: "start");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "survey_configurations",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "survey_configurations",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "survey_configurations",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "survey_configurations",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "survey_configuration_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "TimeToLive",
//                table: "survey_configuration_versions",
//                newName: "timeToLive");
//
//            migrationBuilder.RenameColumn(
//                name: "TimeOut",
//                table: "survey_configuration_versions",
//                newName: "timeOut");
//
//            migrationBuilder.RenameColumn(
//                name: "SurveyConfigurationId",
//                table: "survey_configuration_versions",
//                newName: "surveyConfigurationId");
//
//            migrationBuilder.RenameColumn(
//                name: "Stop",
//                table: "survey_configuration_versions",
//                newName: "stop");
//
//            migrationBuilder.RenameColumn(
//                name: "Start",
//                table: "survey_configuration_versions",
//                newName: "start");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "survey_configuration_versions",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "survey_configuration_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "survey_configuration_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "survey_configuration_versions",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_survey_configuration_versions_SurveyConfigurationId",
////                table: "survey_configuration_versions",
////                newName: "IX_survey_configuration_versions_surveyConfigurationId");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "sites",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "sites",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "sites",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "sites",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "sites",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "sites",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "site_workers",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "site_workers",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkerId",
//                table: "site_workers",
//                newName: "worker_id");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "site_workers",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "site_workers",
//                newName: "site_id");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "site_workers",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "site_workers",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_site_workers_WorkerId",
////                table: "site_workers",
////                newName: "IX_site_workers_worker_id");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_site_workers_SiteId",
////                table: "site_workers",
////                newName: "IX_site_workers_site_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "site_worker_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "site_worker_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkerId",
//                table: "site_worker_versions",
//                newName: "worker_id");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "site_worker_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteWorkerId",
//                table: "site_worker_versions",
//                newName: "site_worker_id");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "site_worker_versions",
//                newName: "site_id");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "site_worker_versions",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "site_worker_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "site_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "site_versions",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "site_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "site_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "site_versions",
//                newName: "site_id");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "site_versions",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "site_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "site_survey_configurations",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "SurveyConfigurationId",
//                table: "site_survey_configurations",
//                newName: "surveyConfigurationId");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "site_survey_configurations",
//                newName: "siteId");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "site_survey_configurations",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "site_survey_configurations",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "site_survey_configurations",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_site_survey_configurations_SurveyConfigurationId",
////                table: "site_survey_configurations",
////                newName: "IX_site_survey_configurations_surveyConfigurationId");
////
////            migrationBuilder.RenameIndex(
////                name: "IX_site_survey_configurations_SiteId",
////                table: "site_survey_configurations",
////                newName: "IX_site_survey_configurations_siteId");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "site_survey_configuration_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "SurveyConfigurationId",
//                table: "site_survey_configuration_versions",
//                newName: "surveyConfigurationId");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteSurveyConfigurationId",
//                table: "site_survey_configuration_versions",
//                newName: "siteSurveyConfigurationId");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "site_survey_configuration_versions",
//                newName: "siteId");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "site_survey_configuration_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "site_survey_configuration_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "site_survey_configuration_versions",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_site_survey_configuration_versions_SiteSurveyConfigurationId",
////                table: "site_survey_configuration_versions",
////                newName: "IX_site_survey_configuration_versions_siteSurveyConfigurationId");
//
//            migrationBuilder.RenameColumn(
//                name: "Value",
//                table: "settings",
//                newName: "value");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "settings",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "Id",
//                table: "settings",
//                newName: "id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "questions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "ValidDisplay",
//                table: "questions",
//                newName: "validDisplay");
//
//            migrationBuilder.RenameColumn(
//                name: "Type",
//                table: "questions",
//                newName: "type");
//
//            migrationBuilder.RenameColumn(
//                name: "RefId",
//                table: "questions",
//                newName: "refId");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionType",
//                table: "questions",
//                newName: "questionType");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionSetId",
//                table: "questions",
//                newName: "questionSetId");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionIndex",
//                table: "questions",
//                newName: "questionIndex");
//
//            migrationBuilder.RenameColumn(
//                name: "Prioritised",
//                table: "questions",
//                newName: "prioritised");
//
//            migrationBuilder.RenameColumn(
//                name: "Minimum",
//                table: "questions",
//                newName: "minimum");
//
//            migrationBuilder.RenameColumn(
//                name: "MinDuration",
//                table: "questions",
//                newName: "minDuration");
//
//            migrationBuilder.RenameColumn(
//                name: "Maximum",
//                table: "questions",
//                newName: "maximum");
//
//            migrationBuilder.RenameColumn(
//                name: "MaxDuration",
//                table: "questions",
//                newName: "maxDuration");
//
//            migrationBuilder.RenameColumn(
//                name: "Image",
//                table: "questions",
//                newName: "image");
//
//            migrationBuilder.RenameColumn(
//                name: "FontSize",
//                table: "questions",
//                newName: "fontSize");
//
//            migrationBuilder.RenameColumn(
//                name: "ContinuousQuestionId",
//                table: "questions",
//                newName: "continuousQuestionId");
//
//            migrationBuilder.RenameColumn(
//                name: "BackButtonEnabled",
//                table: "questions",
//                newName: "backButtonEnabled");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "questions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "questions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "ImagePosition",
//                table: "questions",
//                newName: "imagePostion");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "questions",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_questions_QuestionSetId",
////                table: "questions",
////                newName: "IX_questions_questionSetId");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "question_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "ValidDisplay",
//                table: "question_versions",
//                newName: "validDisplay");
//
//            migrationBuilder.RenameColumn(
//                name: "Type",
//                table: "question_versions",
//                newName: "type");
//
//            migrationBuilder.RenameColumn(
//                name: "RefId",
//                table: "question_versions",
//                newName: "refId");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionType",
//                table: "question_versions",
//                newName: "questionType");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionSetId",
//                table: "question_versions",
//                newName: "questionSetId");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionIndex",
//                table: "question_versions",
//                newName: "questionIndex");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionId",
//                table: "question_versions",
//                newName: "questionId");
//
//            migrationBuilder.RenameColumn(
//                name: "Prioritised",
//                table: "question_versions",
//                newName: "prioritised");
//
//            migrationBuilder.RenameColumn(
//                name: "Minimum",
//                table: "question_versions",
//                newName: "minimum");
//
//            migrationBuilder.RenameColumn(
//                name: "MinDuration",
//                table: "question_versions",
//                newName: "minDuration");
//
//            migrationBuilder.RenameColumn(
//                name: "Maximum",
//                table: "question_versions",
//                newName: "maximum");
//
//            migrationBuilder.RenameColumn(
//                name: "MaxDuration",
//                table: "question_versions",
//                newName: "maxDuration");
//
//            migrationBuilder.RenameColumn(
//                name: "Image",
//                table: "question_versions",
//                newName: "image");
//
//            migrationBuilder.RenameColumn(
//                name: "FontSize",
//                table: "question_versions",
//                newName: "fontSize");
//
//            migrationBuilder.RenameColumn(
//                name: "ContinuousQuestionId",
//                table: "question_versions",
//                newName: "continuousQuestionId");
//
//            migrationBuilder.RenameColumn(
//                name: "BackButtonEnabled",
//                table: "question_versions",
//                newName: "backButtonEnabled");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "question_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "question_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "ImagePosition",
//                table: "question_versions",
//                newName: "imagePostion");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "question_versions",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_question_versions_QuestionId",
////                table: "question_versions",
////                newName: "IX_question_versions_questionId");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "question_sets",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Share",
//                table: "question_sets",
//                newName: "share");
//
//            migrationBuilder.RenameColumn(
//                name: "PosiblyDeployed",
//                table: "question_sets",
//                newName: "posiblyDeployed");
//
//            migrationBuilder.RenameColumn(
//                name: "ParentId",
//                table: "question_sets",
//                newName: "parentId");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "question_sets",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "HasChild",
//                table: "question_sets",
//                newName: "hasChild");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "question_sets",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "question_sets",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "question_sets",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "question_set_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Share",
//                table: "question_set_versions",
//                newName: "share");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionSetId",
//                table: "question_set_versions",
//                newName: "questionSetId");
//
//            migrationBuilder.RenameColumn(
//                name: "ParentId",
//                table: "question_set_versions",
//                newName: "parentId");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "question_set_versions",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "HasChild",
//                table: "question_set_versions",
//                newName: "hasChild");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "question_set_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "question_set_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "PossiblyDeployed",
//                table: "question_set_versions",
//                newName: "posiblyDeployed");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "question_set_versions",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_question_set_versions_QuestionSetId",
////                table: "question_set_versions",
////                newName: "IX_question_set_versions_questionSetId");
//
//            migrationBuilder.RenameColumn(
//                name: "WeightValue",
//                table: "options",
//                newName: "weightValue");
//
//            migrationBuilder.RenameColumn(
//                name: "Weight",
//                table: "options",
//                newName: "weight");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "options",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionId",
//                table: "options",
//                newName: "questionId");
//
//            migrationBuilder.RenameColumn(
//                name: "OptionsIndex",
//                table: "options",
//                newName: "optionsIndex");
//
//            migrationBuilder.RenameColumn(
//                name: "NextQuestionId",
//                table: "options",
//                newName: "nextQuestionId");
//
//            migrationBuilder.RenameColumn(
//                name: "ContinuousOptionId",
//                table: "options",
//                newName: "continuousOptionId");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "options",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "options",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "options",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_options_QuestionId",
////                table: "options",
////                newName: "IX_options_questionId");
//
//            migrationBuilder.RenameColumn(
//                name: "WeightValue",
//                table: "option_versions",
//                newName: "weightValue");
//
//            migrationBuilder.RenameColumn(
//                name: "Weight",
//                table: "option_versions",
//                newName: "weight");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "option_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionId",
//                table: "option_versions",
//                newName: "questionId");
//
//            migrationBuilder.RenameColumn(
//                name: "OptionsIndex",
//                table: "option_versions",
//                newName: "optionsIndex");
//
//            migrationBuilder.RenameColumn(
//                name: "OptionId",
//                table: "option_versions",
//                newName: "optionId");
//
//            migrationBuilder.RenameColumn(
//                name: "NextQuestionId",
//                table: "option_versions",
//                newName: "nextQuestionId");
//
//            migrationBuilder.RenameColumn(
//                name: "ContinuousOptionId",
//                table: "option_versions",
//                newName: "continuousOptionId");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "option_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "option_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "option_versions",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_option_versions_OptionId",
////                table: "option_versions",
////                newName: "IX_option_versions_optionId");
//
//            migrationBuilder.RenameColumn(
//                name: "Transmission",
//                table: "notifications",
//                newName: "transmission");
//
//            migrationBuilder.RenameColumn(
//                name: "Stacktrace",
//                table: "notifications",
//                newName: "stacktrace");
//
//            migrationBuilder.RenameColumn(
//                name: "Exception",
//                table: "notifications",
//                newName: "exception");
//
//            migrationBuilder.RenameColumn(
//                name: "Activity",
//                table: "notifications",
//                newName: "activity");
//
//            migrationBuilder.RenameColumn(
//                name: "Id",
//                table: "notifications",
//                newName: "id");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "notifications",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "notifications",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "NotificationUid",
//                table: "notifications",
//                newName: "notification_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "notifications",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "notifications",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Type",
//                table: "logs",
//                newName: "type");
//
//            migrationBuilder.RenameColumn(
//                name: "Message",
//                table: "logs",
//                newName: "message");
//
//            migrationBuilder.RenameColumn(
//                name: "Level",
//                table: "logs",
//                newName: "level");
//
//            migrationBuilder.RenameColumn(
//                name: "Id",
//                table: "logs",
//                newName: "id");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "logs",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Type",
//                table: "log_exceptions",
//                newName: "type");
//
//            migrationBuilder.RenameColumn(
//                name: "Message",
//                table: "log_exceptions",
//                newName: "message");
//
//            migrationBuilder.RenameColumn(
//                name: "Level",
//                table: "log_exceptions",
//                newName: "level");
//
//            migrationBuilder.RenameColumn(
//                name: "Id",
//                table: "log_exceptions",
//                newName: "id");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "log_exceptions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "languages",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "languages",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "Description",
//                table: "languages",
//                newName: "description");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "languages",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "languages",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "languages",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "language_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "language_versions",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "LanguageId",
//                table: "language_versions",
//                newName: "languageId");
//
//            migrationBuilder.RenameColumn(
//                name: "Description",
//                table: "language_versions",
//                newName: "description");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "language_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "language_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "language_versions",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_language_versions_LanguageId",
////                table: "language_versions",
////                newName: "IX_language_versions_languageId");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "folders",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "ParentId",
//                table: "folders",
//                newName: "parentId");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "folders",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "Description",
//                table: "folders",
//                newName: "description");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "folders",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "folders",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "folders",
//                newName: "parent_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "folders",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_folders_ParentId",
////                table: "folders",
////                newName: "IX_folders_parentId");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "folder_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "folder_versions",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "Description",
//                table: "folder_versions",
//                newName: "description");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "folder_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "folder_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "ParentId",
//                table: "folder_versions",
//                newName: "parent_id");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "folder_versions",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "FolderId",
//                table: "folder_versions",
//                newName: "folder_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "folder_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "fields",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Selected",
//                table: "fields",
//                newName: "selected");
//
//            migrationBuilder.RenameColumn(
//                name: "Optional",
//                table: "fields",
//                newName: "optional");
//
//            migrationBuilder.RenameColumn(
//                name: "Multi",
//                table: "fields",
//                newName: "multi");
//
//            migrationBuilder.RenameColumn(
//                name: "Mandatory",
//                table: "fields",
//                newName: "mandatory");
//
//            migrationBuilder.RenameColumn(
//                name: "Label",
//                table: "fields",
//                newName: "label");
//
//            migrationBuilder.RenameColumn(
//                name: "Dummy",
//                table: "fields",
//                newName: "dummy");
//
//            migrationBuilder.RenameColumn(
//                name: "Description",
//                table: "fields",
//                newName: "description");
//
//            migrationBuilder.RenameColumn(
//                name: "Custom",
//                table: "fields",
//                newName: "custom");
//
//            migrationBuilder.RenameColumn(
//                name: "Color",
//                table: "fields",
//                newName: "color");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "fields",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "fields",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "UnitName",
//                table: "fields",
//                newName: "unit_name");
//
//            migrationBuilder.RenameColumn(
//                name: "StopOnSave",
//                table: "fields",
//                newName: "stop_on_save");
//
//            migrationBuilder.RenameColumn(
//                name: "SplitScreen",
//                table: "fields",
//                newName: "split_screen");
//
//            migrationBuilder.RenameColumn(
//                name: "ReadOnly",
//                table: "fields",
//                newName: "read_only");
//
//            migrationBuilder.RenameColumn(
//                name: "QueryType",
//                table: "fields",
//                newName: "query_type");
//
//            migrationBuilder.RenameColumn(
//                name: "ParentFieldId",
//                table: "fields",
//                newName: "parent_field_id");
//
//            migrationBuilder.RenameColumn(
//                name: "OriginalId",
//                table: "fields",
//                newName: "original_id");
//
//            migrationBuilder.RenameColumn(
//                name: "MinValue",
//                table: "fields",
//                newName: "min_value");
//
//            migrationBuilder.RenameColumn(
//                name: "MaxValue",
//                table: "fields",
//                newName: "max_value");
//
//            migrationBuilder.RenameColumn(
//                name: "MaxLength",
//                table: "fields",
//                newName: "max_length");
//
//            migrationBuilder.RenameColumn(
//                name: "KeyValuePairList",
//                table: "fields",
//                newName: "key_value_pair_list");
//
//            migrationBuilder.RenameColumn(
//                name: "IsNum",
//                table: "fields",
//                newName: "is_num");
//
//            migrationBuilder.RenameColumn(
//                name: "GeolocationHidden",
//                table: "fields",
//                newName: "geolocation_hidden");
//
//            migrationBuilder.RenameColumn(
//                name: "GeolocationForced",
//                table: "fields",
//                newName: "geolocation_forced");
//
//            migrationBuilder.RenameColumn(
//                name: "GeolocationEnabled",
//                table: "fields",
//                newName: "geolocation_enabled");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldTypeId",
//                table: "fields",
//                newName: "field_type_id");
//
//            migrationBuilder.RenameColumn(
//                name: "EntityGroupId",
//                table: "fields",
//                newName: "entity_group_id");
//
//            migrationBuilder.RenameColumn(
//                name: "DisplayIndex",
//                table: "fields",
//                newName: "display_index");
//
//            migrationBuilder.RenameColumn(
//                name: "DefaultValue",
//                table: "fields",
//                newName: "default_value");
//
//            migrationBuilder.RenameColumn(
//                name: "DecimalCount",
//                table: "fields",
//                newName: "decimal_count");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "fields",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListId",
//                table: "fields",
//                newName: "check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "BarcodeType",
//                table: "fields",
//                newName: "barcode_type");
//
//            migrationBuilder.RenameColumn(
//                name: "BarcodeEnabled",
//                table: "fields",
//                newName: "barcode_enabled");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_fields_ParentFieldId",
////                table: "fields",
////                newName: "IX_fields_parent_field_id");
////
////            migrationBuilder.RenameIndex(
////                name: "IX_fields_FieldTypeId",
////                table: "fields",
////                newName: "IX_fields_field_type_id");
////
////            migrationBuilder.RenameIndex(
////                name: "IX_fields_CheckListId",
////                table: "fields",
////                newName: "IX_fields_check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "field_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Selected",
//                table: "field_versions",
//                newName: "selected");
//
//            migrationBuilder.RenameColumn(
//                name: "Optional",
//                table: "field_versions",
//                newName: "optional");
//
//            migrationBuilder.RenameColumn(
//                name: "Multi",
//                table: "field_versions",
//                newName: "multi");
//
//            migrationBuilder.RenameColumn(
//                name: "Mandatory",
//                table: "field_versions",
//                newName: "mandatory");
//
//            migrationBuilder.RenameColumn(
//                name: "Label",
//                table: "field_versions",
//                newName: "label");
//
//            migrationBuilder.RenameColumn(
//                name: "Dummy",
//                table: "field_versions",
//                newName: "dummy");
//
//            migrationBuilder.RenameColumn(
//                name: "Description",
//                table: "field_versions",
//                newName: "description");
//
//            migrationBuilder.RenameColumn(
//                name: "Custom",
//                table: "field_versions",
//                newName: "custom");
//
//            migrationBuilder.RenameColumn(
//                name: "Color",
//                table: "field_versions",
//                newName: "color");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "field_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "field_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "UnitName",
//                table: "field_versions",
//                newName: "unit_name");
//
//            migrationBuilder.RenameColumn(
//                name: "StopOnSave",
//                table: "field_versions",
//                newName: "stop_on_save");
//
//            migrationBuilder.RenameColumn(
//                name: "SplitScreen",
//                table: "field_versions",
//                newName: "split_screen");
//
//            migrationBuilder.RenameColumn(
//                name: "ReadOnly",
//                table: "field_versions",
//                newName: "read_only");
//
//            migrationBuilder.RenameColumn(
//                name: "QueryType",
//                table: "field_versions",
//                newName: "query_type");
//
//            migrationBuilder.RenameColumn(
//                name: "ParentFieldId",
//                table: "field_versions",
//                newName: "parent_field_id");
//
//            migrationBuilder.RenameColumn(
//                name: "OriginalId",
//                table: "field_versions",
//                newName: "original_id");
//
//            migrationBuilder.RenameColumn(
//                name: "MinValue",
//                table: "field_versions",
//                newName: "min_value");
//
//            migrationBuilder.RenameColumn(
//                name: "MaxValue",
//                table: "field_versions",
//                newName: "max_value");
//
//            migrationBuilder.RenameColumn(
//                name: "MaxLength",
//                table: "field_versions",
//                newName: "max_length");
//
//            migrationBuilder.RenameColumn(
//                name: "KeyValuePairList",
//                table: "field_versions",
//                newName: "key_value_pair_list");
//
//            migrationBuilder.RenameColumn(
//                name: "IsNum",
//                table: "field_versions",
//                newName: "is_num");
//
//            migrationBuilder.RenameColumn(
//                name: "GeolocationHidden",
//                table: "field_versions",
//                newName: "geolocation_hidden");
//
//            migrationBuilder.RenameColumn(
//                name: "GeolocationForced",
//                table: "field_versions",
//                newName: "geolocation_forced");
//
//            migrationBuilder.RenameColumn(
//                name: "GeolocationEnabled",
//                table: "field_versions",
//                newName: "geolocation_enabled");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldTypeId",
//                table: "field_versions",
//                newName: "field_type_id");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldId",
//                table: "field_versions",
//                newName: "field_id");
//
//            migrationBuilder.RenameColumn(
//                name: "EntityGroupId",
//                table: "field_versions",
//                newName: "entity_group_id");
//
//            migrationBuilder.RenameColumn(
//                name: "DisplayIndex",
//                table: "field_versions",
//                newName: "display_index");
//
//            migrationBuilder.RenameColumn(
//                name: "DefaultValue",
//                table: "field_versions",
//                newName: "default_value");
//
//            migrationBuilder.RenameColumn(
//                name: "DecimalCount",
//                table: "field_versions",
//                newName: "decimal_count");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "field_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListId",
//                table: "field_versions",
//                newName: "check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "BarcodeType",
//                table: "field_versions",
//                newName: "barcode_type");
//
//            migrationBuilder.RenameColumn(
//                name: "BarcodeEnabled",
//                table: "field_versions",
//                newName: "barcode_enabled");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "field_values",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "field_values",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "field_values",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "field_values",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "field_value_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Value",
//                table: "field_value_versions",
//                newName: "value");
//
//            migrationBuilder.RenameColumn(
//                name: "Longitude",
//                table: "field_value_versions",
//                newName: "longitude");
//
//            migrationBuilder.RenameColumn(
//                name: "Latitude",
//                table: "field_value_versions",
//                newName: "latitude");
//
//            migrationBuilder.RenameColumn(
//                name: "Heading",
//                table: "field_value_versions",
//                newName: "heading");
//
//            migrationBuilder.RenameColumn(
//                name: "Date",
//                table: "field_value_versions",
//                newName: "date");
//
//            migrationBuilder.RenameColumn(
//                name: "Altitude",
//                table: "field_value_versions",
//                newName: "altitude");
//
//            migrationBuilder.RenameColumn(
//                name: "Accuracy",
//                table: "field_value_versions",
//                newName: "accuracy");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "field_value_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UserId",
//                table: "field_value_versions",
//                newName: "user_id");
//
//            migrationBuilder.RenameColumn(
//                name: "UploadedDataId",
//                table: "field_value_versions",
//                newName: "uploaded_data_id");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "field_value_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValueId",
//                table: "field_value_versions",
//                newName: "field_value_id");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldId",
//                table: "field_value_versions",
//                newName: "field_id");
//
//            migrationBuilder.RenameColumn(
//                name: "DoneAt",
//                table: "field_value_versions",
//                newName: "done_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "field_value_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListId",
//                table: "field_value_versions",
//                newName: "check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListDuplicateId",
//                table: "field_value_versions",
//                newName: "check_list_duplicate_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CaseId",
//                table: "field_value_versions",
//                newName: "case_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Description",
//                table: "field_types",
//                newName: "description");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldType",
//                table: "field_types",
//                newName: "field_type");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "entity_items",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Synced",
//                table: "entity_items",
//                newName: "synced");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "entity_items",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "Description",
//                table: "entity_items",
//                newName: "description");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "entity_items",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "entity_items",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "entity_items",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "EntityItemUid",
//                table: "entity_items",
//                newName: "entity_item_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "EntityGroupId",
//                table: "entity_items",
//                newName: "entity_group_id");
//
//            migrationBuilder.RenameColumn(
//                name: "DisplayIndex",
//                table: "entity_items",
//                newName: "display_index");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "entity_items",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "entity_item_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Synced",
//                table: "entity_item_versions",
//                newName: "synced");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "entity_item_versions",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "Description",
//                table: "entity_item_versions",
//                newName: "description");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "entity_item_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "entity_item_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "entity_item_versions",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "EntityItemsId",
//                table: "entity_item_versions",
//                newName: "entity_items_id");
//
//            migrationBuilder.RenameColumn(
//                name: "EntityItemUid",
//                table: "entity_item_versions",
//                newName: "entity_item_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "EntityGroupId",
//                table: "entity_item_versions",
//                newName: "entity_group_id");
//
//            migrationBuilder.RenameColumn(
//                name: "DisplayIndex",
//                table: "entity_item_versions",
//                newName: "display_index");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "entity_item_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "entity_groups",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Type",
//                table: "entity_groups",
//                newName: "type");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "entity_groups",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "entity_groups",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "entity_groups",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "entity_groups",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "entity_groups",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "entity_group_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Type",
//                table: "entity_group_versions",
//                newName: "type");
//
//            migrationBuilder.RenameColumn(
//                name: "Name",
//                table: "entity_group_versions",
//                newName: "name");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "entity_group_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "entity_group_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "entity_group_versions",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "EntityGroupId",
//                table: "entity_group_versions",
//                newName: "entity_group_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "entity_group_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "check_lists",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Repeated",
//                table: "check_lists",
//                newName: "repeated");
//
//            migrationBuilder.RenameColumn(
//                name: "Label",
//                table: "check_lists",
//                newName: "label");
//
//            migrationBuilder.RenameColumn(
//                name: "Description",
//                table: "check_lists",
//                newName: "description");
//
//            migrationBuilder.RenameColumn(
//                name: "Custom",
//                table: "check_lists",
//                newName: "custom");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "check_lists",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "check_lists",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "ReviewEnabled",
//                table: "check_lists",
//                newName: "review_enabled");
//
//            migrationBuilder.RenameColumn(
//                name: "QuickSyncEnabled",
//                table: "check_lists",
//                newName: "quick_sync_enabled");
//
//            migrationBuilder.RenameColumn(
//                name: "ParentId",
//                table: "check_lists",
//                newName: "parent_id");
//
//            migrationBuilder.RenameColumn(
//                name: "OriginalId",
//                table: "check_lists",
//                newName: "original_id");
//
//            migrationBuilder.RenameColumn(
//                name: "MultiApproval",
//                table: "check_lists",
//                newName: "multi_approval");
//
//            migrationBuilder.RenameColumn(
//                name: "ManualSync",
//                table: "check_lists",
//                newName: "manual_sync");
//
//            migrationBuilder.RenameColumn(
//                name: "FolderName",
//                table: "check_lists",
//                newName: "folder_name");
//
//            migrationBuilder.RenameColumn(
//                name: "Field9",
//                table: "check_lists",
//                newName: "field_9");
//
//            migrationBuilder.RenameColumn(
//                name: "Field8",
//                table: "check_lists",
//                newName: "field_8");
//
//            migrationBuilder.RenameColumn(
//                name: "Field7",
//                table: "check_lists",
//                newName: "field_7");
//
//            migrationBuilder.RenameColumn(
//                name: "Field6",
//                table: "check_lists",
//                newName: "field_6");
//
//            migrationBuilder.RenameColumn(
//                name: "Field5",
//                table: "check_lists",
//                newName: "field_5");
//
//            migrationBuilder.RenameColumn(
//                name: "Field4",
//                table: "check_lists",
//                newName: "field_4");
//
//            migrationBuilder.RenameColumn(
//                name: "Field3",
//                table: "check_lists",
//                newName: "field_3");
//
//            migrationBuilder.RenameColumn(
//                name: "Field2",
//                table: "check_lists",
//                newName: "field_2");
//
//            migrationBuilder.RenameColumn(
//                name: "Field10",
//                table: "check_lists",
//                newName: "field_10");
//
//            migrationBuilder.RenameColumn(
//                name: "Field1",
//                table: "check_lists",
//                newName: "field_1");
//
//            migrationBuilder.RenameColumn(
//                name: "FastNavigation",
//                table: "check_lists",
//                newName: "fast_navigation");
//
//            migrationBuilder.RenameColumn(
//                name: "ExtraFieldsEnabled",
//                table: "check_lists",
//                newName: "extra_fields_enabled");
//
//            migrationBuilder.RenameColumn(
//                name: "DownloadEntities",
//                table: "check_lists",
//                newName: "download_entities");
//
//            migrationBuilder.RenameColumn(
//                name: "DoneButtonEnabled",
//                table: "check_lists",
//                newName: "done_button_enabled");
//
//            migrationBuilder.RenameColumn(
//                name: "DisplayIndex",
//                table: "check_lists",
//                newName: "display_index");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "check_lists",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CaseType",
//                table: "check_lists",
//                newName: "case_type");
//
//            migrationBuilder.RenameColumn(
//                name: "ApprovalEnabled",
//                table: "check_lists",
//                newName: "approval_enabled");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_check_lists_ParentId",
////                table: "check_lists",
////                newName: "IX_check_lists_parent_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "check_list_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Repeated",
//                table: "check_list_versions",
//                newName: "repeated");
//
//            migrationBuilder.RenameColumn(
//                name: "Label",
//                table: "check_list_versions",
//                newName: "label");
//
//            migrationBuilder.RenameColumn(
//                name: "Description",
//                table: "check_list_versions",
//                newName: "description");
//
//            migrationBuilder.RenameColumn(
//                name: "Custom",
//                table: "check_list_versions",
//                newName: "custom");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "check_list_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "check_list_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "ReviewEnabled",
//                table: "check_list_versions",
//                newName: "review_enabled");
//
//            migrationBuilder.RenameColumn(
//                name: "QuickSyncEnabled",
//                table: "check_list_versions",
//                newName: "quick_sync_enabled");
//
//            migrationBuilder.RenameColumn(
//                name: "ParentId",
//                table: "check_list_versions",
//                newName: "parent_id");
//
//            migrationBuilder.RenameColumn(
//                name: "OriginalId",
//                table: "check_list_versions",
//                newName: "original_id");
//
//            migrationBuilder.RenameColumn(
//                name: "MultiApproval",
//                table: "check_list_versions",
//                newName: "multi_approval");
//
//            migrationBuilder.RenameColumn(
//                name: "ManualSync",
//                table: "check_list_versions",
//                newName: "manual_sync");
//
//            migrationBuilder.RenameColumn(
//                name: "FolderName",
//                table: "check_list_versions",
//                newName: "folder_name");
//
//            migrationBuilder.RenameColumn(
//                name: "Field9",
//                table: "check_list_versions",
//                newName: "field_9");
//
//            migrationBuilder.RenameColumn(
//                name: "Field8",
//                table: "check_list_versions",
//                newName: "field_8");
//
//            migrationBuilder.RenameColumn(
//                name: "Field7",
//                table: "check_list_versions",
//                newName: "field_7");
//
//            migrationBuilder.RenameColumn(
//                name: "Field6",
//                table: "check_list_versions",
//                newName: "field_6");
//
//            migrationBuilder.RenameColumn(
//                name: "Field5",
//                table: "check_list_versions",
//                newName: "field_5");
//
//            migrationBuilder.RenameColumn(
//                name: "Field4",
//                table: "check_list_versions",
//                newName: "field_4");
//
//            migrationBuilder.RenameColumn(
//                name: "Field3",
//                table: "check_list_versions",
//                newName: "field_3");
//
//            migrationBuilder.RenameColumn(
//                name: "Field2",
//                table: "check_list_versions",
//                newName: "field_2");
//
//            migrationBuilder.RenameColumn(
//                name: "Field10",
//                table: "check_list_versions",
//                newName: "field_10");
//
//            migrationBuilder.RenameColumn(
//                name: "Field1",
//                table: "check_list_versions",
//                newName: "field_1");
//
//            migrationBuilder.RenameColumn(
//                name: "FastNavigation",
//                table: "check_list_versions",
//                newName: "fast_navigation");
//
//            migrationBuilder.RenameColumn(
//                name: "ExtraFieldsEnabled",
//                table: "check_list_versions",
//                newName: "extra_fields_enabled");
//
//            migrationBuilder.RenameColumn(
//                name: "DownloadEntities",
//                table: "check_list_versions",
//                newName: "download_entities");
//
//            migrationBuilder.RenameColumn(
//                name: "DoneButtonEnabled",
//                table: "check_list_versions",
//                newName: "done_button_enabled");
//
//            migrationBuilder.RenameColumn(
//                name: "DisplayIndex",
//                table: "check_list_versions",
//                newName: "display_index");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "check_list_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListId",
//                table: "check_list_versions",
//                newName: "check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CaseType",
//                table: "check_list_versions",
//                newName: "case_type");
//
//            migrationBuilder.RenameColumn(
//                name: "ApprovalEnabled",
//                table: "check_list_versions",
//                newName: "approval_enabled");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "check_list_values",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Status",
//                table: "check_list_values",
//                newName: "status");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "check_list_values",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UserId",
//                table: "check_list_values",
//                newName: "user_id");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "check_list_values",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "check_list_values",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListId",
//                table: "check_list_values",
//                newName: "check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListDuplicateId",
//                table: "check_list_values",
//                newName: "check_list_duplicate_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CaseId",
//                table: "check_list_values",
//                newName: "case_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "check_list_value_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Status",
//                table: "check_list_value_versions",
//                newName: "status");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "check_list_value_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UserId",
//                table: "check_list_value_versions",
//                newName: "user_id");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "check_list_value_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "check_list_value_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListValueId",
//                table: "check_list_value_versions",
//                newName: "check_list_value_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListId",
//                table: "check_list_value_versions",
//                newName: "check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListDuplicateId",
//                table: "check_list_value_versions",
//                newName: "check_list_duplicate_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CaseId",
//                table: "check_list_value_versions",
//                newName: "case_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "check_list_sites",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "check_list_sites",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "check_list_sites",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "check_list_sites",
//                newName: "site_id");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "check_list_sites",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "LastCheckId",
//                table: "check_list_sites",
//                newName: "last_check_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "check_list_sites",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListId",
//                table: "check_list_sites",
//                newName: "check_list_id");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_check_list_sites_SiteId",
////                table: "check_list_sites",
////                newName: "IX_check_list_sites_site_id");
////
////            migrationBuilder.RenameIndex(
////                name: "IX_check_list_sites_CheckListId",
////                table: "check_list_sites",
////                newName: "IX_check_list_sites_check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "check_list_site_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "check_list_site_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "check_list_site_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "check_list_site_versions",
//                newName: "site_id");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "check_list_site_versions",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "LastCheckId",
//                table: "check_list_site_versions",
//                newName: "last_check_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "check_list_site_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListSiteId",
//                table: "check_list_site_versions",
//                newName: "check_list_site_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListId",
//                table: "check_list_site_versions",
//                newName: "check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "cases",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Type",
//                table: "cases",
//                newName: "type");
//
//            migrationBuilder.RenameColumn(
//                name: "Status",
//                table: "cases",
//                newName: "status");
//
//            migrationBuilder.RenameColumn(
//                name: "Custom",
//                table: "cases",
//                newName: "custom");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "cases",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkerId",
//                table: "cases",
//                newName: "unit_id");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "cases",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "UnitId",
//                table: "cases",
//                newName: "site_id");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "cases",
//                newName: "done_by_user_id");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "cases",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingCheckUid",
//                table: "cases",
//                newName: "microting_check_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue9",
//                table: "cases",
//                newName: "field_value_9");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue8",
//                table: "cases",
//                newName: "field_value_8");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue7",
//                table: "cases",
//                newName: "field_value_7");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue6",
//                table: "cases",
//                newName: "field_value_6");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue5",
//                table: "cases",
//                newName: "field_value_5");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue4",
//                table: "cases",
//                newName: "field_value_4");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue3",
//                table: "cases",
//                newName: "field_value_3");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue2",
//                table: "cases",
//                newName: "field_value_2");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue10",
//                table: "cases",
//                newName: "field_value_10");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue1",
//                table: "cases",
//                newName: "field_value_1");
//
//            migrationBuilder.RenameColumn(
//                name: "DoneByUserId",
//                table: "cases",
//                newName: "check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "DoneAt",
//                table: "cases",
//                newName: "done_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "cases",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CaseUid",
//                table: "cases",
//                newName: "case_uid");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_cases_WorkerId",
////                table: "cases",
////                newName: "IX_cases_unit_id");
////
////            migrationBuilder.RenameIndex(
////                name: "IX_cases_UnitId",
////                table: "cases",
////                newName: "IX_cases_site_id");
////
////            migrationBuilder.RenameIndex(
////                name: "IX_cases_SiteId",
////                table: "cases",
////                newName: "IX_cases_done_by_user_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "case_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Type",
//                table: "case_versions",
//                newName: "type");
//
//            migrationBuilder.RenameColumn(
//                name: "Status",
//                table: "case_versions",
//                newName: "status");
//
//            migrationBuilder.RenameColumn(
//                name: "Custom",
//                table: "case_versions",
//                newName: "custom");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "case_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "case_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "UnitId",
//                table: "case_versions",
//                newName: "unit_id");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "case_versions",
//                newName: "site_id");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingUid",
//                table: "case_versions",
//                newName: "microting_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "MicrotingCheckUid",
//                table: "case_versions",
//                newName: "microting_check_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue9",
//                table: "case_versions",
//                newName: "field_value_9");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue8",
//                table: "case_versions",
//                newName: "field_value_8");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue7",
//                table: "case_versions",
//                newName: "field_value_7");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue6",
//                table: "case_versions",
//                newName: "field_value_6");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue5",
//                table: "case_versions",
//                newName: "field_value_5");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue4",
//                table: "case_versions",
//                newName: "field_value_4");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue3",
//                table: "case_versions",
//                newName: "field_value_3");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue2",
//                table: "case_versions",
//                newName: "field_value_2");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue10",
//                table: "case_versions",
//                newName: "field_value_10");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldValue1",
//                table: "case_versions",
//                newName: "field_value_1");
//
//            migrationBuilder.RenameColumn(
//                name: "DoneByUserId",
//                table: "case_versions",
//                newName: "done_by_user_id");
//
//            migrationBuilder.RenameColumn(
//                name: "DoneAt",
//                table: "case_versions",
//                newName: "done_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "case_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListId",
//                table: "case_versions",
//                newName: "check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CaseUid",
//                table: "case_versions",
//                newName: "case_uid");
//
//            migrationBuilder.RenameColumn(
//                name: "CaseId",
//                table: "case_versions",
//                newName: "case_id");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "answers",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "UtcAdjusted",
//                table: "answers",
//                newName: "UTCAdjusted");
//
//            migrationBuilder.RenameColumn(
//                name: "UnitId",
//                table: "answers",
//                newName: "unitId");
//
//            migrationBuilder.RenameColumn(
//                name: "TimeZone",
//                table: "answers",
//                newName: "timeZone");
//
//            migrationBuilder.RenameColumn(
//                name: "SurveyConfigurationId",
//                table: "answers",
//                newName: "surveyConfigurationId");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "answers",
//                newName: "siteId");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionSetId",
//                table: "answers",
//                newName: "questionSetId");
//
//            migrationBuilder.RenameColumn(
//                name: "LanguageId",
//                table: "answers",
//                newName: "languageId");
//
//            migrationBuilder.RenameColumn(
//                name: "FinishedAt",
//                table: "answers",
//                newName: "finishedAt");
//
//            migrationBuilder.RenameColumn(
//                name: "AnswerDuration",
//                table: "answers",
//                newName: "answerDuration");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "answers",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "answers",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "answers",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_answers_UnitId",
////                table: "answers",
////                newName: "IX_answers_unitId");
////
////            migrationBuilder.RenameIndex(
////                name: "IX_answers_SurveyConfigurationId",
////                table: "answers",
////                newName: "IX_answers_surveyConfigurationId");
////
////            migrationBuilder.RenameIndex(
////                name: "IX_answers_SiteId",
////                table: "answers",
////                newName: "IX_answers_siteId");
////
////            migrationBuilder.RenameIndex(
////                name: "IX_answers_QuestionSetId",
////                table: "answers",
////                newName: "IX_answers_questionSetId");
////
////            migrationBuilder.RenameIndex(
////                name: "IX_answers_LanguageId",
////                table: "answers",
////                newName: "IX_answers_languageId");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "answer_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "UtcAdjusted",
//                table: "answer_versions",
//                newName: "UTCAdjusted");
//
//            migrationBuilder.RenameColumn(
//                name: "UnitId",
//                table: "answer_versions",
//                newName: "unitId");
//
//            migrationBuilder.RenameColumn(
//                name: "TimeZone",
//                table: "answer_versions",
//                newName: "timeZone");
//
//            migrationBuilder.RenameColumn(
//                name: "SurveyConfigurationId",
//                table: "answer_versions",
//                newName: "surveyConfigurationId");
//
//            migrationBuilder.RenameColumn(
//                name: "SiteId",
//                table: "answer_versions",
//                newName: "siteId");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionSetId",
//                table: "answer_versions",
//                newName: "questionSetId");
//
//            migrationBuilder.RenameColumn(
//                name: "FinishedAt",
//                table: "answer_versions",
//                newName: "finishedAt");
//
//            migrationBuilder.RenameColumn(
//                name: "AnswerId",
//                table: "answer_versions",
//                newName: "answerId");
//
//            migrationBuilder.RenameColumn(
//                name: "AnswerDuration",
//                table: "answer_versions",
//                newName: "answerDuration");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "answer_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "answer_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "answer_versions",
//                newName: "created_at");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "answer_values",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Value",
//                table: "answer_values",
//                newName: "value");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionId",
//                table: "answer_values",
//                newName: "questionId");
//
//            migrationBuilder.RenameColumn(
//                name: "OptionsId",
//                table: "answer_values",
//                newName: "optionsId");
//
//            migrationBuilder.RenameColumn(
//                name: "AnswerId",
//                table: "answer_values",
//                newName: "answerId");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "answer_values",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "answer_values",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "answer_values",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_answer_values_QuestionId",
////                table: "answer_values",
////                newName: "IX_answer_values_questionId");
////
////            migrationBuilder.RenameIndex(
////                name: "IX_answer_values_OptionsId",
////                table: "answer_values",
////                newName: "IX_answer_values_optionsId");
////
////            migrationBuilder.RenameIndex(
////                name: "IX_answer_values_AnswerId",
////                table: "answer_values",
////                newName: "IX_answer_values_answerId");
//
//            migrationBuilder.RenameColumn(
//                name: "Version",
//                table: "answer_value_versions",
//                newName: "version");
//
//            migrationBuilder.RenameColumn(
//                name: "Value",
//                table: "answer_value_versions",
//                newName: "value");
//
//            migrationBuilder.RenameColumn(
//                name: "QuestionId",
//                table: "answer_value_versions",
//                newName: "questionId");
//
//            migrationBuilder.RenameColumn(
//                name: "OptionsId",
//                table: "answer_value_versions",
//                newName: "optionsId");
//
//            migrationBuilder.RenameColumn(
//                name: "AnswerValueId",
//                table: "answer_value_versions",
//                newName: "answerValueId");
//
//            migrationBuilder.RenameColumn(
//                name: "AnswerId",
//                table: "answer_value_versions",
//                newName: "answerId");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkflowState",
//                table: "answer_value_versions",
//                newName: "workflow_state");
//
//            migrationBuilder.RenameColumn(
//                name: "UpdatedAt",
//                table: "answer_value_versions",
//                newName: "updated_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CreatedAt",
//                table: "answer_value_versions",
//                newName: "created_at");
//
////            migrationBuilder.RenameIndex(
////                name: "IX_answer_value_versions_AnswerValueId",
////                table: "answer_value_versions",
////                newName: "IX_answer_value_versions_answerValueId");
//
//            migrationBuilder.AddColumn<int>(
//                name: "microting_uid",
//                table: "folders",
//                nullable: true);
//
//            migrationBuilder.CreateIndex(
//                name: "IX_cases_check_list_id",
//                table: "cases",
//                column: "check_list_id");
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_answer_value_versions_answer_values_answerValueId",
//                table: "answer_value_versions",
//                column: "answerValueId",
//                principalTable: "answer_values",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_answer_values_answers_answerId",
//                table: "answer_values",
//                column: "answerId",
//                principalTable: "answers",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_answer_values_options_optionsId",
//                table: "answer_values",
//                column: "optionsId",
//                principalTable: "options",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_answer_values_questions_questionId",
//                table: "answer_values",
//                column: "questionId",
//                principalTable: "questions",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_answers_languages_languageId",
//                table: "answers",
//                column: "languageId",
//                principalTable: "languages",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_answers_question_sets_questionSetId",
//                table: "answers",
//                column: "questionSetId",
//                principalTable: "question_sets",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_answers_sites_siteId",
//                table: "answers",
//                column: "siteId",
//                principalTable: "sites",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_answers_survey_configurations_surveyConfigurationId",
//                table: "answers",
//                column: "surveyConfigurationId",
//                principalTable: "survey_configurations",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_answers_units_unitId",
//                table: "answers",
//                column: "unitId",
//                principalTable: "units",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_cases_check_lists_check_list_id",
//                table: "cases",
//                column: "check_list_id",
//                principalTable: "check_lists",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_cases_workers_done_by_user_id",
//                table: "cases",
//                column: "done_by_user_id",
//                principalTable: "workers",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_cases_sites_site_id",
//                table: "cases",
//                column: "site_id",
//                principalTable: "sites",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_cases_units_unit_id",
//                table: "cases",
//                column: "unit_id",
//                principalTable: "units",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_check_list_sites_check_lists_check_list_id",
//                table: "check_list_sites",
//                column: "check_list_id",
//                principalTable: "check_lists",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_check_list_sites_sites_site_id",
//                table: "check_list_sites",
//                column: "site_id",
//                principalTable: "sites",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_check_lists_check_lists_parent_id",
//                table: "check_lists",
//                column: "parent_id",
//                principalTable: "check_lists",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_fields_check_lists_check_list_id",
//                table: "fields",
//                column: "check_list_id",
//                principalTable: "check_lists",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_fields_field_types_field_type_id",
//                table: "fields",
//                column: "field_type_id",
//                principalTable: "field_types",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_fields_fields_parent_field_id",
//                table: "fields",
//                column: "parent_field_id",
//                principalTable: "fields",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_folders_folders_parentId",
//                table: "folders",
//                column: "parentId",
//                principalTable: "folders",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_language_versions_languages_languageId",
//                table: "language_versions",
//                column: "languageId",
//                principalTable: "languages",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_option_versions_options_optionId",
//                table: "option_versions",
//                column: "optionId",
//                principalTable: "options",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_options_questions_questionId",
//                table: "options",
//                column: "questionId",
//                principalTable: "questions",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_question_set_versions_question_sets_questionSetId",
//                table: "question_set_versions",
//                column: "questionSetId",
//                principalTable: "question_sets",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_question_versions_questions_questionId",
//                table: "question_versions",
//                column: "questionId",
//                principalTable: "questions",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_questions_question_sets_questionSetId",
//                table: "questions",
//                column: "questionSetId",
//                principalTable: "question_sets",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_site_survey_configuration_versions_site_survey_configurations_siteSurveyConfigurationId",
//                table: "site_survey_configuration_versions",
//                column: "siteSurveyConfigurationId",
//                principalTable: "site_survey_configurations",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_site_survey_configurations_sites_siteId",
//                table: "site_survey_configurations",
//                column: "siteId",
//                principalTable: "sites",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_site_survey_configurations_survey_configurations_surveyConfigurationId",
//                table: "site_survey_configurations",
//                column: "surveyConfigurationId",
//                principalTable: "survey_configurations",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_site_workers_sites_site_id",
//                table: "site_workers",
//                column: "site_id",
//                principalTable: "sites",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_site_workers_workers_worker_id",
//                table: "site_workers",
//                column: "worker_id",
//                principalTable: "workers",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_survey_configuration_versions_survey_configurations_surveyConfigurationId",
//                table: "survey_configuration_versions",
//                column: "surveyConfigurationId",
//                principalTable: "survey_configurations",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Cascade);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_taggings_check_lists_check_list_id",
//                table: "taggings",
//                column: "check_list_id",
//                principalTable: "check_lists",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_taggings_tags_tag_id",
//                table: "taggings",
//                column: "tag_id",
//                principalTable: "tags",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_units_sites_site_id",
//                table: "units",
//                column: "site_id",
//                principalTable: "sites",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
        }
    }
}