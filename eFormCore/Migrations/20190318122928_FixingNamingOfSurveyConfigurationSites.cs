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