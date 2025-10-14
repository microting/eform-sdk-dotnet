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
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingTranslationsToInSight : Migration
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
            migrationBuilder.CreateTable(
                name: "LanguageQuestionSets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    LanguageId = table.Column<int>(nullable: false),
                    QuestionSetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageQuestionSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageQuestionSets_languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageQuestionSets_question_sets_QuestionSetId",
                        column: x => x.QuestionSetId,
                        principalTable: "question_sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    OptionId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionTranslations_languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OptionTranslations_options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionTranslationVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    OptionId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OptionTranslationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionTranslationVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    QuestionId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionTranslations_languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionTranslations_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageQuestionSetVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    LanguageId = table.Column<int>(nullable: false),
                    QuestionSetId = table.Column<int>(nullable: false),
                    LanguageQuestionSetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageQuestionSetVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageQuestionSetVersions_LanguageQuestionSets_LanguageQuestionSetId",
                        column: x => x.LanguageQuestionSetId,
                        principalTable: "LanguageQuestionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTranslationVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    QuestionId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    QuestionTranslationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTranslationVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionTranslationVersions_QuestionTranslations_QuestionTranslationId",
                        column: x => x.QuestionTranslationId,
                        principalTable: "QuestionTranslations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageQuestionSets_LanguageId",
                table: "LanguageQuestionSets",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageQuestionSets_QuestionSetId",
                table: "LanguageQuestionSets",
                column: "QuestionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageQuestionSetVersions_LanguageQuestionSetId",
                table: "LanguageQuestionSetVersions",
                column: "LanguageQuestionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionTranslations_LanguageId",
                table: "OptionTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionTranslations_OptionId",
                table: "OptionTranslations",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTranslations_LanguageId",
                table: "QuestionTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTranslations_QuestionId",
                table: "QuestionTranslations",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTranslationVersions_QuestionTranslationId",
                table: "QuestionTranslationVersions",
                column: "QuestionTranslationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageQuestionSetVersions");

            migrationBuilder.DropTable(
                name: "OptionTranslations");

            migrationBuilder.DropTable(
                name: "OptionTranslationVersions");

            migrationBuilder.DropTable(
                name: "QuestionTranslationVersions");

            migrationBuilder.DropTable(
                name: "LanguageQuestionSets");

            migrationBuilder.DropTable(
                name: "QuestionTranslations");
        }
    }
}