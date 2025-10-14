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
    public partial class AddingModelseForInsight : Migration
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
                name: "answer_value_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    answerId = table.Column<int>(nullable: false),
                    questionId = table.Column<int>(nullable: false),
                    optionsId = table.Column<int>(nullable: false),
                    value = table.Column<int>(nullable: false),
                    answerValueId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer_value_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "answer_values",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    answerId = table.Column<int>(nullable: false),
                    questionId = table.Column<int>(nullable: false),
                    optionsId = table.Column<int>(nullable: false),
                    value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer_values", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "answer_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    unitId = table.Column<int>(nullable: false),
                    siteId = table.Column<int>(nullable: false),
                    answerDuration = table.Column<int>(nullable: false),
                    languageId = table.Column<int>(nullable: false),
                    surveyConfigurationId = table.Column<int>(nullable: false),
                    finishedAt = table.Column<int>(nullable: false),
                    questionSetId = table.Column<int>(nullable: false),
                    UTCAdjusted = table.Column<bool>(nullable: false),
                    timeZone = table.Column<string>(nullable: true),
                    answerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "answers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    unitId = table.Column<int>(nullable: false),
                    siteId = table.Column<int>(nullable: false),
                    answerDuration = table.Column<int>(nullable: false),
                    languageId = table.Column<int>(nullable: false),
                    surveyConfigurationId = table.Column<int>(nullable: false),
                    finishedAt = table.Column<int>(nullable: false),
                    questionSetId = table.Column<int>(nullable: false),
                    UTCAdjusted = table.Column<bool>(nullable: false),
                    timeZone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "language_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    languageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_language_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "languages",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "option_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    nextQuestionId = table.Column<int>(nullable: false),
                    weight = table.Column<int>(nullable: false),
                    weightValue = table.Column<int>(nullable: false),
                    continuousOptionId = table.Column<int>(nullable: false),
                    questionId = table.Column<int>(nullable: false),
                    optionsIndex = table.Column<int>(nullable: false),
                    optionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_option_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "options",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    nextQuestionId = table.Column<int>(nullable: false),
                    weight = table.Column<int>(nullable: false),
                    weightValue = table.Column<int>(nullable: false),
                    continuousOptionId = table.Column<int>(nullable: false),
                    questionId = table.Column<int>(nullable: false),
                    optionsIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_options", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "question_set_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    hasChild = table.Column<bool>(nullable: false),
                    posiblyDeployed = table.Column<bool>(nullable: false),
                    parentId = table.Column<int>(nullable: false),
                    share = table.Column<bool>(nullable: false),
                    questionSetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_set_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "question_sets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    hasChild = table.Column<bool>(nullable: false),
                    posiblyDeployed = table.Column<bool>(nullable: false),
                    parentId = table.Column<int>(nullable: false),
                    share = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_sets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "question_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    questionSetId = table.Column<int>(nullable: false),
                    questionType = table.Column<string>(nullable: true),
                    minimum = table.Column<int>(nullable: false),
                    maximum = table.Column<int>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    refId = table.Column<int>(nullable: false),
                    questionIndex = table.Column<int>(nullable: false),
                    image = table.Column<bool>(nullable: false),
                    continuousQuestionId = table.Column<int>(nullable: false),
                    imagePostion = table.Column<string>(nullable: true),
                    prioritised = table.Column<bool>(nullable: false),
                    backButtonEnabled = table.Column<bool>(nullable: false),
                    fontSize = table.Column<string>(nullable: true),
                    minDuration = table.Column<int>(nullable: false),
                    maxDuration = table.Column<int>(nullable: false),
                    validDisplay = table.Column<bool>(nullable: false),
                    questionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    questionSetId = table.Column<int>(nullable: false),
                    questionType = table.Column<string>(nullable: true),
                    minimum = table.Column<int>(nullable: false),
                    maximum = table.Column<int>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    refId = table.Column<int>(nullable: false),
                    questionIndex = table.Column<int>(nullable: false),
                    image = table.Column<bool>(nullable: false),
                    continuousQuestionId = table.Column<int>(nullable: false),
                    imagePostion = table.Column<string>(nullable: true),
                    prioritised = table.Column<bool>(nullable: false),
                    backButtonEnabled = table.Column<bool>(nullable: false),
                    fontSize = table.Column<string>(nullable: true),
                    minDuration = table.Column<int>(nullable: false),
                    maxDuration = table.Column<int>(nullable: false),
                    validDisplay = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "site_survey_configuraion_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    siteId = table.Column<int>(nullable: false),
                    surveyConfigId = table.Column<int>(nullable: false),
                    siteSurveyConfigurationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_survey_configuraion_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "site_survey_configuraions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    siteId = table.Column<int>(nullable: false),
                    surveyConfigurationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_survey_configuraions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "survey_configuration_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    start = table.Column<DateTime>(nullable: false),
                    stop = table.Column<DateTime>(nullable: false),
                    timeToLive = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    timeOut = table.Column<int>(nullable: false),
                    surveyConfigurationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey_configuration_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "survey_configurations",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    start = table.Column<DateTime>(nullable: false),
                    stop = table.Column<DateTime>(nullable: false),
                    timeToLive = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    timeOut = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey_configurations", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answer_value_versions");

            migrationBuilder.DropTable(
                name: "answer_values");

            migrationBuilder.DropTable(
                name: "answer_versions");

            migrationBuilder.DropTable(
                name: "answers");

            migrationBuilder.DropTable(
                name: "language_versions");

            migrationBuilder.DropTable(
                name: "languages");

            migrationBuilder.DropTable(
                name: "option_versions");

            migrationBuilder.DropTable(
                name: "options");

            migrationBuilder.DropTable(
                name: "question_set_versions");

            migrationBuilder.DropTable(
                name: "question_sets");

            migrationBuilder.DropTable(
                name: "question_versions");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "site_survey_configuraion_versions");

            migrationBuilder.DropTable(
                name: "site_survey_configuraions");

            migrationBuilder.DropTable(
                name: "survey_configuration_versions");

            migrationBuilder.DropTable(
                name: "survey_configurations");
        }
    }
}