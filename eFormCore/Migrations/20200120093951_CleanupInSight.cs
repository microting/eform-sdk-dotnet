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