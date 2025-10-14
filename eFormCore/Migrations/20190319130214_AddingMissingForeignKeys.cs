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

using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingMissingForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_survey_configuration_versions_surveyConfigurationId",
                table: "survey_configuration_versions",
                column: "surveyConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_site_survey_configurations_siteId",
                table: "site_survey_configurations",
                column: "siteId");

            migrationBuilder.CreateIndex(
                name: "IX_site_survey_configurations_surveyConfigurationId",
                table: "site_survey_configurations",
                column: "surveyConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_site_survey_configuration_versions_siteSurveyConfigurationId",
                table: "site_survey_configuration_versions",
                column: "siteSurveyConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_questions_questionSetId",
                table: "questions",
                column: "questionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_question_versions_questionId",
                table: "question_versions",
                column: "questionId");

            migrationBuilder.CreateIndex(
                name: "IX_question_set_versions_questionSetId",
                table: "question_set_versions",
                column: "questionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_options_questionId",
                table: "options",
                column: "questionId");

            migrationBuilder.CreateIndex(
                name: "IX_option_versions_optionId",
                table: "option_versions",
                column: "optionId");

            migrationBuilder.CreateIndex(
                name: "IX_language_versions_languageId",
                table: "language_versions",
                column: "languageId");

            migrationBuilder.CreateIndex(
                name: "IX_answers_languageId",
                table: "answers",
                column: "languageId");

            migrationBuilder.CreateIndex(
                name: "IX_answers_questionSetId",
                table: "answers",
                column: "questionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_answers_siteId",
                table: "answers",
                column: "siteId");

            migrationBuilder.CreateIndex(
                name: "IX_answers_surveyConfigurationId",
                table: "answers",
                column: "surveyConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_answers_unitId",
                table: "answers",
                column: "unitId");

            migrationBuilder.CreateIndex(
                name: "IX_answer_values_answerId",
                table: "answer_values",
                column: "answerId");

            migrationBuilder.CreateIndex(
                name: "IX_answer_values_optionsId",
                table: "answer_values",
                column: "optionsId");

            migrationBuilder.CreateIndex(
                name: "IX_answer_values_questionId",
                table: "answer_values",
                column: "questionId");

            migrationBuilder.CreateIndex(
                name: "IX_answer_value_versions_answerValueId",
                table: "answer_value_versions",
                column: "answerValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_answer_value_versions_answer_values_answerValueId",
                table: "answer_value_versions",
                column: "answerValueId",
                principalTable: "answer_values",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answer_values_answers_answerId",
                table: "answer_values",
                column: "answerId",
                principalTable: "answers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answer_values_options_optionsId",
                table: "answer_values",
                column: "optionsId",
                principalTable: "options",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answer_values_questions_questionId",
                table: "answer_values",
                column: "questionId",
                principalTable: "questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_languages_languageId",
                table: "answers",
                column: "languageId",
                principalTable: "languages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_question_sets_questionSetId",
                table: "answers",
                column: "questionSetId",
                principalTable: "question_sets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_sites_siteId",
                table: "answers",
                column: "siteId",
                principalTable: "sites",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_survey_configurations_surveyConfigurationId",
                table: "answers",
                column: "surveyConfigurationId",
                principalTable: "survey_configurations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_units_unitId",
                table: "answers",
                column: "unitId",
                principalTable: "units",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_language_versions_languages_languageId",
                table: "language_versions",
                column: "languageId",
                principalTable: "languages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_option_versions_options_optionId",
                table: "option_versions",
                column: "optionId",
                principalTable: "options",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_options_questions_questionId",
                table: "options",
                column: "questionId",
                principalTable: "questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_question_set_versions_question_sets_questionSetId",
                table: "question_set_versions",
                column: "questionSetId",
                principalTable: "question_sets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_question_versions_questions_questionId",
                table: "question_versions",
                column: "questionId",
                principalTable: "questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_questions_question_sets_questionSetId",
                table: "questions",
                column: "questionSetId",
                principalTable: "question_sets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_site_survey_configuration_versions_site_survey_configurations_siteSurveyConfigurationId",
                table: "site_survey_configuration_versions",
                column: "siteSurveyConfigurationId",
                principalTable: "site_survey_configurations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_site_survey_configurations_sites_siteId",
                table: "site_survey_configurations",
                column: "siteId",
                principalTable: "sites",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_site_survey_configurations_survey_configurations_surveyConfigurationId",
                table: "site_survey_configurations",
                column: "surveyConfigurationId",
                principalTable: "survey_configurations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_survey_configuration_versions_survey_configurations_surveyConfigurationId",
                table: "survey_configuration_versions",
                column: "surveyConfigurationId",
                principalTable: "survey_configurations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answer_value_versions_answer_values_answerValueId",
                table: "answer_value_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_answer_values_answers_answerId",
                table: "answer_values");

            migrationBuilder.DropForeignKey(
                name: "FK_answer_values_options_optionsId",
                table: "answer_values");

            migrationBuilder.DropForeignKey(
                name: "FK_answer_values_questions_questionId",
                table: "answer_values");

            migrationBuilder.DropForeignKey(
                name: "FK_answers_languages_languageId",
                table: "answers");

            migrationBuilder.DropForeignKey(
                name: "FK_answers_question_sets_questionSetId",
                table: "answers");

            migrationBuilder.DropForeignKey(
                name: "FK_answers_sites_siteId",
                table: "answers");

            migrationBuilder.DropForeignKey(
                name: "FK_answers_survey_configurations_surveyConfigurationId",
                table: "answers");

            migrationBuilder.DropForeignKey(
                name: "FK_answers_units_unitId",
                table: "answers");

            migrationBuilder.DropForeignKey(
                name: "FK_language_versions_languages_languageId",
                table: "language_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_option_versions_options_optionId",
                table: "option_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_options_questions_questionId",
                table: "options");

            migrationBuilder.DropForeignKey(
                name: "FK_question_set_versions_question_sets_questionSetId",
                table: "question_set_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_question_versions_questions_questionId",
                table: "question_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_questions_question_sets_questionSetId",
                table: "questions");

            migrationBuilder.DropForeignKey(
                name: "FK_site_survey_configuration_versions_site_survey_configurations_siteSurveyConfigurationId",
                table: "site_survey_configuration_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_site_survey_configurations_sites_siteId",
                table: "site_survey_configurations");

            migrationBuilder.DropForeignKey(
                name: "FK_site_survey_configurations_survey_configurations_surveyConfigurationId",
                table: "site_survey_configurations");

            migrationBuilder.DropForeignKey(
                name: "FK_survey_configuration_versions_survey_configurations_surveyConfigurationId",
                table: "survey_configuration_versions");

            migrationBuilder.DropIndex(
                name: "IX_survey_configuration_versions_surveyConfigurationId",
                table: "survey_configuration_versions");

            migrationBuilder.DropIndex(
                name: "IX_site_survey_configurations_siteId",
                table: "site_survey_configurations");

            migrationBuilder.DropIndex(
                name: "IX_site_survey_configurations_surveyConfigurationId",
                table: "site_survey_configurations");

            migrationBuilder.DropIndex(
                name: "IX_site_survey_configuration_versions_siteSurveyConfigurationId",
                table: "site_survey_configuration_versions");

            migrationBuilder.DropIndex(
                name: "IX_questions_questionSetId",
                table: "questions");

            migrationBuilder.DropIndex(
                name: "IX_question_versions_questionId",
                table: "question_versions");

            migrationBuilder.DropIndex(
                name: "IX_question_set_versions_questionSetId",
                table: "question_set_versions");

            migrationBuilder.DropIndex(
                name: "IX_options_questionId",
                table: "options");

            migrationBuilder.DropIndex(
                name: "IX_option_versions_optionId",
                table: "option_versions");

            migrationBuilder.DropIndex(
                name: "IX_language_versions_languageId",
                table: "language_versions");

            migrationBuilder.DropIndex(
                name: "IX_answers_languageId",
                table: "answers");

            migrationBuilder.DropIndex(
                name: "IX_answers_questionSetId",
                table: "answers");

            migrationBuilder.DropIndex(
                name: "IX_answers_siteId",
                table: "answers");

            migrationBuilder.DropIndex(
                name: "IX_answers_surveyConfigurationId",
                table: "answers");

            migrationBuilder.DropIndex(
                name: "IX_answers_unitId",
                table: "answers");

            migrationBuilder.DropIndex(
                name: "IX_answer_values_answerId",
                table: "answer_values");

            migrationBuilder.DropIndex(
                name: "IX_answer_values_optionsId",
                table: "answer_values");

            migrationBuilder.DropIndex(
                name: "IX_answer_values_questionId",
                table: "answer_values");

            migrationBuilder.DropIndex(
                name: "IX_answer_value_versions_answerValueId",
                table: "answer_value_versions");
        }
    }
}