using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingDescriptionToEntityGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageQuestionSetVersions_LanguageQuestionSets_LanguageQues",
                table: "LanguageQuestionSetVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTranslationVersions_QuestionTranslations_QuestionTran",
                table: "QuestionTranslationVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_site_survey_configuration_versions_site_survey_configurations",
                table: "site_survey_configuration_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_site_survey_configurations_survey_configurations_SurveyConfig",
                table: "site_survey_configurations");

            migrationBuilder.DropForeignKey(
                name: "FK_survey_configuration_versions_survey_configurations_SurveyCon",
                table: "survey_configuration_versions");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "entity_groups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "entity_group_versions",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageQuestionSetVersions_LanguageQuestionSets_LanguageQue~",
                table: "LanguageQuestionSetVersions",
                column: "LanguageQuestionSetId",
                principalTable: "LanguageQuestionSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionTranslationVersions_QuestionTranslations_QuestionTra~",
                table: "QuestionTranslationVersions",
                column: "QuestionTranslationId",
                principalTable: "QuestionTranslations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_site_survey_configuration_versions_site_survey_configuration~",
                table: "site_survey_configuration_versions",
                column: "SiteSurveyConfigurationId",
                principalTable: "site_survey_configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_site_survey_configurations_survey_configurations_SurveyConfi~",
                table: "site_survey_configurations",
                column: "SurveyConfigurationId",
                principalTable: "survey_configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_survey_configuration_versions_survey_configurations_SurveyCo~",
                table: "survey_configuration_versions",
                column: "SurveyConfigurationId",
                principalTable: "survey_configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageQuestionSetVersions_LanguageQuestionSets_LanguageQue~",
                table: "LanguageQuestionSetVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTranslationVersions_QuestionTranslations_QuestionTra~",
                table: "QuestionTranslationVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_site_survey_configuration_versions_site_survey_configuration~",
                table: "site_survey_configuration_versions");

            migrationBuilder.DropForeignKey(
                name: "FK_site_survey_configurations_survey_configurations_SurveyConfi~",
                table: "site_survey_configurations");

            migrationBuilder.DropForeignKey(
                name: "FK_survey_configuration_versions_survey_configurations_SurveyCo~",
                table: "survey_configuration_versions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "entity_groups");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "entity_group_versions");

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageQuestionSetVersions_LanguageQuestionSets_LanguageQuestionSetId",
                table: "LanguageQuestionSetVersions",
                column: "LanguageQuestionSetId",
                principalTable: "LanguageQuestionSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionTranslationVersions_QuestionTranslations_QuestionTranslationId",
                table: "QuestionTranslationVersions",
                column: "QuestionTranslationId",
                principalTable: "QuestionTranslations",
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
                name: "FK_site_survey_configurations_survey_configurations_SurveyConfigurationId",
                table: "site_survey_configurations",
                column: "SurveyConfigurationId",
                principalTable: "survey_configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_survey_configuration_versions_survey_configurations_SurveyConfigurationId",
                table: "survey_configuration_versions",
                column: "SurveyConfigurationId",
                principalTable: "survey_configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
