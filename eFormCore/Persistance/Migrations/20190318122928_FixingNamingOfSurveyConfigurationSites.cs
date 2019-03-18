using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class FixingNamingOfSurveyConfigurationSites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_site_survey_configuraions",
                table: "site_survey_configuraions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_site_survey_configuraion_versions",
                table: "site_survey_configuraion_versions");

            migrationBuilder.RenameTable(
                name: "site_survey_configuraions",
                newName: "site_survey_configurations");

            migrationBuilder.RenameTable(
                name: "site_survey_configuraion_versions",
                newName: "site_survey_configuration_versions");

            migrationBuilder.RenameColumn(
                name: "surveyConfigId",
                table: "site_survey_configuration_versions",
                newName: "surveyConfigurationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_site_survey_configurations",
                table: "site_survey_configurations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_site_survey_configuration_versions",
                table: "site_survey_configuration_versions",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_site_survey_configurations",
                table: "site_survey_configurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_site_survey_configuration_versions",
                table: "site_survey_configuration_versions");

            migrationBuilder.RenameTable(
                name: "site_survey_configurations",
                newName: "site_survey_configuraions");

            migrationBuilder.RenameTable(
                name: "site_survey_configuration_versions",
                newName: "site_survey_configuraion_versions");

            migrationBuilder.RenameColumn(
                name: "surveyConfigurationId",
                table: "site_survey_configuraion_versions",
                newName: "surveyConfigId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_site_survey_configuraions",
                table: "site_survey_configuraions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_site_survey_configuraion_versions",
                table: "site_survey_configuraion_versions",
                column: "id");
        }
    }
}
