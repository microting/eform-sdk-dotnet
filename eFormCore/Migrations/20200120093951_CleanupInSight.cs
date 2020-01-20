using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class CleanupInSight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "survey_configurations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "survey_configuration_versions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "site_survey_configurations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "site_survey_configuration_versions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "question_versions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "question_sets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "question_set_versions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "options",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "option_versions",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedAt",
                table: "answers",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "answers",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedAt",
                table: "answer_versions",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "answer_versions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "answer_values",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "answer_value_versions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "survey_configurations");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "survey_configuration_versions");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "site_survey_configurations");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "site_survey_configuration_versions");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "questions");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "question_versions");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "question_sets");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "question_set_versions");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "options");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "option_versions");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "answers");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "answer_versions");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "answer_values");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "answer_value_versions");

            migrationBuilder.AlterColumn<int>(
                name: "FinishedAt",
                table: "answers",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "FinishedAt",
                table: "answer_versions",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
