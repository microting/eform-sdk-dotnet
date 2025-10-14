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
    public partial class RefactoringidtoId : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_cases_workers_done_by_user_id",
                table: "cases");

            migrationBuilder.DropForeignKey(
                name: "FK_field_values_workers_user_id",
                table: "field_values");

            migrationBuilder.DropForeignKey(
                name: "FK_site_workers_workers_worker_id",
                table: "site_workers");

            migrationBuilder.DropForeignKey(
                name: "FK_cases_check_lists_check_list_id",
                table: "cases");

            migrationBuilder.DropForeignKey(
                name: "FK_field_values_uploaded_data_uploaded_data_id",
                table: "field_values");

            migrationBuilder.DropForeignKey(
                name: "FK_field_values_fields_field_id",
                table: "field_values");

            migrationBuilder.DropForeignKey(
                name: "FK_fields_fields_parentid",
                table: "fields");

            migrationBuilder.DropForeignKey(
                name: "FK_fields_check_lists_check_list_id",
                table: "fields");

            migrationBuilder.DropForeignKey(
                name: "FK_field_values_check_lists_check_list_id",
                table: "field_values");

            migrationBuilder.DropForeignKey(
                name: "FK_check_list_sites_check_lists_check_list_id",
                table: "check_list_sites");

            migrationBuilder.DropForeignKey(
                name: "FK_check_lists_check_lists_parentid",
                table: "check_lists");

            migrationBuilder.DropForeignKey(
                name: "FK_taggings_check_lists_check_list_id",
                table: "taggings");

            migrationBuilder.DropForeignKey(
                name: "FK_taggings_tags_tag_id",
                table: "taggings");

            migrationBuilder.DropForeignKey(
                name: "FK_answers_survey_configurations_surveyConfigurationId",
                table: "answers");

            migrationBuilder.DropForeignKey(
                name: "FK_site_survey_configurations_survey_configurations_surveyConfig",
                table: "site_survey_configurations");

            migrationBuilder.DropForeignKey(
                name: "FK_survey_configuration_versions_survey_configurations_surveyCon",
                table: "survey_configuration_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_answers_units_unitId",
                table: "answers");

            migrationBuilder.DropForeignKey(
                name: "FK_cases_units_unit_id",
                table: "cases");

            migrationBuilder.DropForeignKey(
                name: "FK_answers_sites_siteId",
                table: "answers");

            migrationBuilder.DropForeignKey(
                name: "FK_cases_sites_site_id",
                table: "cases");

            migrationBuilder.DropForeignKey(
                name: "FK_check_list_sites_sites_site_id",
                table: "check_list_sites");

            migrationBuilder.DropForeignKey(
                name: "FK_site_survey_configurations_sites_siteId",
                table: "site_survey_configurations");

            migrationBuilder.DropForeignKey(
                name: "FK_site_workers_sites_site_id",
                table: "site_workers");

            migrationBuilder.DropForeignKey(
                name: "FK_units_sites_site_id",
                table: "units");

            migrationBuilder.DropForeignKey(
                name: "FK_site_survey_configuration_versions_site_survey_configurations",
                table: "site_survey_configuration_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_answer_values_questions_questionId",
                table: "answer_values");

            migrationBuilder.DropForeignKey(
                name: "FK_options_questions_questionId",
                table: "options");

            migrationBuilder.DropForeignKey(
                name: "FK_question_versions_questions_questionId",
                table: "question_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_answers_question_sets_questionSetId",
                table: "answers");

            migrationBuilder.DropForeignKey(
                name: "FK_question_set_versions_question_sets_questionSetId",
                table: "question_set_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_questions_question_sets_questionSetId",
                table: "questions");

            migrationBuilder.DropForeignKey(
                name: "FK_answer_values_options_optionsId",
                table: "answer_values");

            migrationBuilder.DropForeignKey(
                name: "FK_option_versions_options_optionId",
                table: "option_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_answers_languages_languageId",
                table: "answers");

            migrationBuilder.DropForeignKey(
                name: "FK_language_versions_languages_languageId",
                table: "language_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_folders_folders_parentid",
                table: "folders");

            migrationBuilder.DropForeignKey(
                name: "FK_fields_field_types_field_type_id",
                table: "fields");

            migrationBuilder.DropForeignKey(
                name: "FK_answer_values_answers_answerId",
                table: "answer_values");

            migrationBuilder.DropForeignKey(
                name: "FK_answer_value_versions_answer_values_answerValueId",
                table: "answer_value_versions");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "workers",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "workers").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "worker_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "worker_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "uploaded_data_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "uploaded_data_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "uploaded_data",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "uploaded_data").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "units",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "units").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);
//
            migrationBuilder.RenameColumn(
                name: "id",
                table: "unit_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "unit_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tags",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "tags").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "taggings",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "taggings").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);
//
            migrationBuilder.RenameColumn(
                name: "id",
                table: "tagging_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "tagging_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tag_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "tag_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "survey_configurations",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "survey_configurations").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "survey_configuration_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "survey_configuration_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "sites",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "sites").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "site_workers",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "site_workers").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "site_worker_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "site_worker_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "site_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "site_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "site_survey_configurations",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "site_survey_configurations").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "site_survey_configuration_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "site_survey_configuration_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "questions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "questions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "question_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "question_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "question_sets",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "question_sets").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "question_set_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "question_set_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "options",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "options").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "option_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "option_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "languages",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "languages").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "language_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "language_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "parentid",
                table: "folders",
                newName: "parentId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "folders",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "folders").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "folder_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "folder_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "fields",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "fields").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "field_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "field_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "field_values",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "field_values").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "field_value_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "field_value_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "field_types",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "field_types").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "entity_items",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "entity_items").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "entity_item_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "entity_item_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "entity_groups",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "entity_groups").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "entity_group_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "entity_group_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "check_lists",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "check_lists").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "check_list_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "check_list_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "check_list_values",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "check_list_values").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "check_list_value_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "check_list_value_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "check_list_sites",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "check_list_sites").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "check_list_site_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "check_list_site_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "cases",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "cases").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "case_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "case_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "answers",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "answers").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "answer_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "answer_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "answer_values",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "answer_values").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.RenameColumn(
                name: "id",
                table: "answer_value_versions",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "answer_value_versions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

//            migrationBuilder.AddForeignKey(
//                name: "FK_folders_folders_parentId",
//                table: "folders",
//                column: "parentId",
//                principalTable: "folders",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_folders_folders_parentId",
                table: "folders");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "workers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "worker_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "uploaded_data_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "uploaded_data",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "units",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "unit_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tags",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "taggings",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tagging_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tag_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "survey_configurations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "survey_configuration_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sites",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "site_workers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "site_worker_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "site_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "site_survey_configurations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "site_survey_configuration_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "questions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "question_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "question_sets",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "question_set_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "options",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "option_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "languages",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "language_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "parentId",
                table: "folders",
                newName: "parentid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "folders",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_folders_parentId",
                table: "folders",
                newName: "IX_folders_parentid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "folder_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "fields",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "field_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "field_values",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "field_value_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "field_types",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "entity_items",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "entity_item_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "entity_groups",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "entity_group_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "check_lists",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "check_list_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "check_list_values",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "check_list_value_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "check_list_sites",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "check_list_site_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "cases",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "case_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "answers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "answer_versions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "answer_values",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "answer_value_versions",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_folders_folders_parentid",
                table: "folders",
                column: "parentid",
                principalTable: "folders",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}