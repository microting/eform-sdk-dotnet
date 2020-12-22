using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class HugheTableRenaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "answer_value_versions",
                null,
                "AnswerValueVersions");

            migrationBuilder.RenameTable(
                name: "answer_versions",
                null,
                "AnswerVersions");

            migrationBuilder.RenameTable(
                name: "case_versions",
                null,
                "CaseVersions");

            migrationBuilder.RenameTable(
                name: "check_list_site_versions",
                null,
                "CheckListSiteVersions");

            migrationBuilder.RenameTable(
                name: "check_list_sites",
                null,
                "CheckListSites");

            migrationBuilder.RenameTable(
                name: "check_list_value_versions",
                null,
                "CheckListValueVersions");

            migrationBuilder.RenameTable(
                name: "check_list_values",
                null,
                "CheckListValues");

            migrationBuilder.RenameTable(
                name: "check_list_versions",
                null,
                "CheckListVersions");

            migrationBuilder.RenameTable(
                name: "entity_group_versions",
                null,
                "EntityGroupVersions");

            migrationBuilder.RenameTable(
                name: "entity_groups",
                null,
                "EntityGroups");

            migrationBuilder.RenameTable(
                name: "entity_item_versions",
                null,
                "EntityItemVersions");

            migrationBuilder.RenameTable(
                name: "entity_items",
                null,
                "EntityItems");

            migrationBuilder.RenameTable(
                name: "field_types",
                null,
                "FieldTypes");

            migrationBuilder.RenameTable(
                name: "field_value_versions",
                null,
                "FieldValueVersions");

            migrationBuilder.RenameTable(
                name: "field_values",
                null,
                "FieldValues");

            migrationBuilder.RenameTable(
                name: "field_versions",
                null,
                "FieldVersions");

            migrationBuilder.RenameTable(
                name: "folder_versions",
                null,
                "FolderVersions");

            migrationBuilder.RenameTable(
                name: "language_versions",
                null,
                "LanguageVersions");

            migrationBuilder.RenameTable(
                name: "log_exceptions",
                null,
                "LogExceptions");

            migrationBuilder.RenameTable(
                name: "notification_versions",
                null,
                "NotificationVersions");

            migrationBuilder.RenameTable(
                name: "option_versions",
                null,
                "OptionVersions");

            migrationBuilder.RenameTable(
                name: "question_set_versions",
                null,
                "QuestionSetVersions");

            migrationBuilder.RenameTable(
                name: "setting_versions",
                null,
                "SettingVersions");

            migrationBuilder.RenameTable(
                name: "site_survey_configuration_versions",
                null,
                "SiteSurveyConfigurationVersions");

            migrationBuilder.RenameTable(
                name: "site_versions",
                null,
               "SiteVersions");

            migrationBuilder.RenameTable(
                name: "site_worker_versions",
                null,
                "SiteWorkerVersions");

            migrationBuilder.RenameTable(
                name: "survey_configuration_versions",
                null,
                "SurveyConfigurationVersions");

            migrationBuilder.RenameTable(
                name: "tag_versions",
                null,
                "TagVersions");

            migrationBuilder.RenameTable(
                name: "tagging_versions",
                null,
                "TaggingVersions");

            migrationBuilder.RenameTable(
                name: "unit_versions",
                null,
                "UnitVersions");

            migrationBuilder.RenameTable(
                name: "uploaded_data_versions",
                null,
                "UploadedDataVersions");

            migrationBuilder.RenameTable(
                name: "worker_versions",
                null,
                "WorkerVersions");

            migrationBuilder.RenameTable(
                name: "answer_values",
                null,
                "AnswerValues");

            migrationBuilder.RenameTable(
                name: "check_lists",
                null,
                "CheckLists");

            migrationBuilder.RenameTable(
                name: "uploaded_datas",
                null,
                "UploadedDatas");

            migrationBuilder.RenameTable(
                name: "site_survey_configurations",
                null,
                "SiteSurveyConfigurations");

            migrationBuilder.RenameTable(
                name: "survey_configurations",
                null,
                "SurveyConfigurations");

            migrationBuilder.RenameTable(
                name: "question_sets",
                null,
                "QuestionSets");

            migrationBuilder.RenameTable(
                name: "workers",
                null,
                newName: "Workers");

            migrationBuilder.RenameTable(
                name: "units",
                null,
                newName: "Units");

            migrationBuilder.RenameTable(
                name: "tags",
                null,
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "taggings",
                null,
                newName: "Taggings");

            migrationBuilder.RenameTable(
                name: "sites",
                null,
                newName: "Sites");

            migrationBuilder.RenameTable(
                name: "settings",
                null,
                newName: "Settings");

            migrationBuilder.RenameTable(
                name: "questions",
                null,
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "options",
                null,
                newName: "Options");

            migrationBuilder.RenameTable(
                name: "notifications",
                null,
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "logs",
                null,
                newName: "Logs");

            migrationBuilder.RenameTable(
                name: "languages",
                null,
                newName: "Languages");

            migrationBuilder.RenameTable(
                name: "folders",
                null,
                newName: "Folders");

            migrationBuilder.RenameTable(
                name: "fields",
                null,
                newName: "Fields");

            migrationBuilder.RenameTable(
                name: "cases",
                null,
                newName: "Cases");

            migrationBuilder.RenameTable(
                name: "answers",
                null,
                newName: "Answers");

            migrationBuilder.RenameTable(
                name: "site_workers",
                null,
                newName: "SiteWorkers");

            migrationBuilder.RenameTable(
                name: "question_versions",
                null,
                newName: "QuestionVersions");

            migrationBuilder.RenameColumn(
                "FieldType",
                "FieldTypes",
                "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Languages_LanguageId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_QuestionSets_QuestionSetId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Sites_SiteId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_SurveyConfigurations_SurveyConfigurationId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Units_UnitId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_CheckLists_CheckListId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Folders_FolderId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Sites_SiteId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Units_UnitId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Workers_WorkerId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_CheckLists_CheckListId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_FieldTypes_FieldTypeId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Fields_ParentFieldId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Folders_ParentId",
                table: "Folders");

            migrationBuilder.DropForeignKey(
                name: "FK_LanguageQuestionSets_Languages_LanguageId",
                table: "LanguageQuestionSets");

            migrationBuilder.DropForeignKey(
                name: "FK_LanguageQuestionSets_QuestionSets_QuestionSetId",
                table: "LanguageQuestionSets");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_OptionTranslations_Languages_LanguageId",
                table: "OptionTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_OptionTranslations_Options_OptionId",
                table: "OptionTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionSets_QuestionSetId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTranslations_Languages_LanguageId",
                table: "QuestionTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTranslations_Questions_QuestionId",
                table: "QuestionTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionVersions_Questions_QuestionId",
                table: "QuestionVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_SiteGroupSites_Sites_SiteId",
                table: "SiteGroupSites");

            migrationBuilder.DropForeignKey(
                name: "FK_SiteTags_Sites_SiteId",
                table: "SiteTags");

            migrationBuilder.DropForeignKey(
                name: "FK_SiteTags_Tags_TagId",
                table: "SiteTags");

            migrationBuilder.DropForeignKey(
                name: "FK_SiteWorkers_Sites_SiteId",
                table: "SiteWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_SiteWorkers_Workers_WorkerId",
                table: "SiteWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_Taggings_CheckLists_CheckListId",
                table: "Taggings");

            migrationBuilder.DropForeignKey(
                name: "FK_Taggings_Tags_TagId",
                table: "Taggings");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Sites_SiteId",
                table: "Units");

            migrationBuilder.DropTable(
                name: "AnswerValueVersions");

            migrationBuilder.DropTable(
                name: "AnswerVersions");

            migrationBuilder.DropTable(
                name: "CaseVersions");

            migrationBuilder.DropTable(
                name: "CheckListSites");

            migrationBuilder.DropTable(
                name: "CheckListSiteVersions");

            migrationBuilder.DropTable(
                name: "CheckListValues");

            migrationBuilder.DropTable(
                name: "CheckListValueVersions");

            migrationBuilder.DropTable(
                name: "CheckListVersions");

            migrationBuilder.DropTable(
                name: "EntityGroups");

            migrationBuilder.DropTable(
                name: "EntityGroupVersions");

            migrationBuilder.DropTable(
                name: "EntityItems");

            migrationBuilder.DropTable(
                name: "EntityItemVersions");

            migrationBuilder.DropTable(
                name: "FieldTypes");

            migrationBuilder.DropTable(
                name: "FieldValues");

            migrationBuilder.DropTable(
                name: "FieldValueVersions");

            migrationBuilder.DropTable(
                name: "FieldVersions");

            migrationBuilder.DropTable(
                name: "FolderVersions");

            migrationBuilder.DropTable(
                name: "LanguageVersions");

            migrationBuilder.DropTable(
                name: "LogExceptions");

            migrationBuilder.DropTable(
                name: "NotificationVersions");

            migrationBuilder.DropTable(
                name: "OptionVersions");

            migrationBuilder.DropTable(
                name: "QuestionSetVersions");

            migrationBuilder.DropTable(
                name: "SettingVersions");

            migrationBuilder.DropTable(
                name: "SiteSurveyConfigurationVersions");

            migrationBuilder.DropTable(
                name: "SiteVersions");

            migrationBuilder.DropTable(
                name: "SiteWorkerVersions");

            migrationBuilder.DropTable(
                name: "SurveyConfigurationVersions");

            migrationBuilder.DropTable(
                name: "TaggingVersions");

            migrationBuilder.DropTable(
                name: "TagVersions");

            migrationBuilder.DropTable(
                name: "UnitVersions");

            migrationBuilder.DropTable(
                name: "UploadedDataVersions");

            migrationBuilder.DropTable(
                name: "WorkerVersions");

            migrationBuilder.DropTable(
                name: "AnswerValues");

            migrationBuilder.DropTable(
                name: "CheckLists");

            migrationBuilder.DropTable(
                name: "UploadedDatas");

            migrationBuilder.DropTable(
                name: "SiteSurveyConfigurations");

            migrationBuilder.DropTable(
                name: "SurveyConfigurations");

            migrationBuilder.DropTable(
                name: "QuestionSets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workers",
                table: "Workers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Taggings",
                table: "Taggings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sites",
                table: "Sites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Settings",
                table: "Settings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Options",
                table: "Options");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                table: "Languages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Folders",
                table: "Folders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fields",
                table: "Fields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cases",
                table: "Cases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answers",
                table: "Answers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SiteWorkers",
                table: "SiteWorkers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionVersions",
                table: "QuestionVersions");

            migrationBuilder.RenameTable(
                name: "Workers",
                newName: "workers");

            migrationBuilder.RenameTable(
                name: "Units",
                newName: "units");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "tags");

            migrationBuilder.RenameTable(
                name: "Taggings",
                newName: "taggings");

            migrationBuilder.RenameTable(
                name: "Sites",
                newName: "sites");

            migrationBuilder.RenameTable(
                name: "Settings",
                newName: "settings");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "questions");

            migrationBuilder.RenameTable(
                name: "Options",
                newName: "options");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "notifications");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "logs");

            migrationBuilder.RenameTable(
                name: "Languages",
                newName: "languages");

            migrationBuilder.RenameTable(
                name: "Folders",
                newName: "folders");

            migrationBuilder.RenameTable(
                name: "Fields",
                newName: "fields");

            migrationBuilder.RenameTable(
                name: "Cases",
                newName: "cases");

            migrationBuilder.RenameTable(
                name: "Answers",
                newName: "answers");

            migrationBuilder.RenameTable(
                name: "SiteWorkers",
                newName: "site_workers");

            migrationBuilder.RenameTable(
                name: "QuestionVersions",
                newName: "question_versions");

            migrationBuilder.RenameIndex(
                name: "IX_Units_SiteId",
                table: "units",
                newName: "IX_units_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Taggings_TagId",
                table: "taggings",
                newName: "IX_taggings_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_Taggings_CheckListId",
                table: "taggings",
                newName: "IX_taggings_CheckListId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_QuestionSetId",
                table: "questions",
                newName: "IX_questions_QuestionSetId");

            migrationBuilder.RenameIndex(
                name: "IX_Options_QuestionId",
                table: "options",
                newName: "IX_options_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Folders_ParentId",
                table: "folders",
                newName: "IX_folders_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Fields_ParentFieldId",
                table: "fields",
                newName: "IX_fields_ParentFieldId");

            migrationBuilder.RenameIndex(
                name: "IX_Fields_FieldTypeId",
                table: "fields",
                newName: "IX_fields_FieldTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Fields_CheckListId",
                table: "fields",
                newName: "IX_fields_CheckListId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_MicrotingUid_MicrotingCheckUid",
                table: "cases",
                newName: "IX_cases_MicrotingUid_MicrotingCheckUid");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_WorkerId",
                table: "cases",
                newName: "IX_cases_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_UnitId",
                table: "cases",
                newName: "IX_cases_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_SiteId",
                table: "cases",
                newName: "IX_cases_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_FolderId",
                table: "cases",
                newName: "IX_cases_FolderId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_CheckListId",
                table: "cases",
                newName: "IX_cases_CheckListId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_UnitId",
                table: "answers",
                newName: "IX_answers_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_SurveyConfigurationId",
                table: "answers",
                newName: "IX_answers_SurveyConfigurationId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_SiteId",
                table: "answers",
                newName: "IX_answers_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_QuestionSetId",
                table: "answers",
                newName: "IX_answers_QuestionSetId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_LanguageId",
                table: "answers",
                newName: "IX_answers_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_SiteWorkers_WorkerId",
                table: "site_workers",
                newName: "IX_site_workers_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_SiteWorkers_SiteId",
                table: "site_workers",
                newName: "IX_site_workers_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionVersions_QuestionId",
                table: "question_versions",
                newName: "IX_question_versions_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_workers",
                table: "workers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_units",
                table: "units",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tags",
                table: "tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_taggings",
                table: "taggings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sites",
                table: "sites",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_settings",
                table: "settings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_questions",
                table: "questions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_options",
                table: "options",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notifications",
                table: "notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_logs",
                table: "logs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_languages",
                table: "languages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_folders",
                table: "folders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fields",
                table: "fields",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cases",
                table: "cases",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_answers",
                table: "answers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_site_workers",
                table: "site_workers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_question_versions",
                table: "question_versions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "answer_values",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AnswerId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer_values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_answer_values_answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_answer_values_options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_answer_values_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "answer_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AnswerDuration = table.Column<int>(type: "int", nullable: false),
                    AnswerId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    QuestionSetId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    SurveyConfigurationId = table.Column<int>(type: "int", nullable: true),
                    TimeZone = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UtcAdjusted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CaseId = table.Column<int>(type: "int", nullable: true),
                    CaseUid = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    CheckListId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Custom = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DoneAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FieldValue1 = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FieldValue10 = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FieldValue2 = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FieldValue3 = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FieldValue4 = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FieldValue5 = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FieldValue6 = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FieldValue7 = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FieldValue8 = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FieldValue9 = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FolderId = table.Column<int>(type: "int", nullable: true),
                    MicrotingCheckUid = table.Column<int>(type: "int", nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "check_list_site_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CheckListId = table.Column<int>(type: "int", nullable: true),
                    CheckListSiteId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FolderId = table.Column<int>(type: "int", nullable: true),
                    LastCheckId = table.Column<int>(type: "int", nullable: false),
                    MicrotingUid = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_list_site_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "check_list_value_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CaseId = table.Column<int>(type: "int", nullable: true),
                    CheckListDuplicateId = table.Column<int>(type: "int", nullable: true),
                    CheckListId = table.Column<int>(type: "int", nullable: true),
                    CheckListValueId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Status = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_list_value_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "check_list_values",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CaseId = table.Column<int>(type: "int", nullable: true),
                    CheckListDuplicateId = table.Column<int>(type: "int", nullable: true),
                    CheckListId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Status = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_list_values", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "check_list_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApprovalEnabled = table.Column<short>(type: "smallint", nullable: true),
                    CaseType = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    CheckListId = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Custom = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DisplayIndex = table.Column<int>(type: "int", nullable: true),
                    DocxExportEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DoneButtonEnabled = table.Column<short>(type: "smallint", nullable: true),
                    DownloadEntities = table.Column<short>(type: "smallint", nullable: true),
                    ExcelExportEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExtraFieldsEnabled = table.Column<short>(type: "smallint", nullable: true),
                    FastNavigation = table.Column<short>(type: "smallint", nullable: true),
                    Field1 = table.Column<int>(type: "int", nullable: true),
                    Field10 = table.Column<int>(type: "int", nullable: true),
                    Field2 = table.Column<int>(type: "int", nullable: true),
                    Field3 = table.Column<int>(type: "int", nullable: true),
                    Field4 = table.Column<int>(type: "int", nullable: true),
                    Field5 = table.Column<int>(type: "int", nullable: true),
                    Field6 = table.Column<int>(type: "int", nullable: true),
                    Field7 = table.Column<int>(type: "int", nullable: true),
                    Field8 = table.Column<int>(type: "int", nullable: true),
                    Field9 = table.Column<int>(type: "int", nullable: true),
                    FolderName = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    JasperExportEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Label = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ManualSync = table.Column<short>(type: "smallint", nullable: true),
                    MultiApproval = table.Column<short>(type: "smallint", nullable: true),
                    OriginalId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    QuickSyncEnabled = table.Column<short>(type: "smallint", nullable: true),
                    Repeated = table.Column<int>(type: "int", nullable: true),
                    ReviewEnabled = table.Column<short>(type: "smallint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_list_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "check_lists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApprovalEnabled = table.Column<short>(type: "smallint", nullable: true),
                    CaseType = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Color = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Custom = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DisplayIndex = table.Column<int>(type: "int", nullable: true),
                    DocxExportEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DoneButtonEnabled = table.Column<short>(type: "smallint", nullable: true),
                    DownloadEntities = table.Column<short>(type: "smallint", nullable: true),
                    ExcelExportEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExtraFieldsEnabled = table.Column<short>(type: "smallint", nullable: true),
                    FastNavigation = table.Column<short>(type: "smallint", nullable: true),
                    Field1 = table.Column<int>(type: "int", nullable: true),
                    Field10 = table.Column<int>(type: "int", nullable: true),
                    Field2 = table.Column<int>(type: "int", nullable: true),
                    Field3 = table.Column<int>(type: "int", nullable: true),
                    Field4 = table.Column<int>(type: "int", nullable: true),
                    Field5 = table.Column<int>(type: "int", nullable: true),
                    Field6 = table.Column<int>(type: "int", nullable: true),
                    Field7 = table.Column<int>(type: "int", nullable: true),
                    Field8 = table.Column<int>(type: "int", nullable: true),
                    Field9 = table.Column<int>(type: "int", nullable: true),
                    FolderName = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    JasperExportEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Label = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ManualSync = table.Column<short>(type: "smallint", nullable: true),
                    MultiApproval = table.Column<short>(type: "smallint", nullable: true),
                    OriginalId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    QuickSyncEnabled = table.Column<short>(type: "smallint", nullable: true),
                    Repeated = table.Column<int>(type: "int", nullable: true),
                    ReviewEnabled = table.Column<short>(type: "smallint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_lists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_check_lists_check_lists_ParentId",
                        column: x => x.ParentId,
                        principalTable: "check_lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "entity_group_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    EntityGroupId = table.Column<int>(type: "int", nullable: false),
                    MicrotingUid = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Type = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_group_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "entity_groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    MicrotingUid = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Type = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "entity_item_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DisplayIndex = table.Column<int>(type: "int", nullable: false),
                    EntityGroupId = table.Column<int>(type: "int", nullable: true),
                    EntityItemId = table.Column<int>(type: "int", nullable: false),
                    EntityItemUid = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    MicrotingUid = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Synced = table.Column<short>(type: "smallint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_item_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "entity_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DisplayIndex = table.Column<int>(type: "int", nullable: false),
                    EntityGroupId = table.Column<int>(type: "int", nullable: false),
                    EntityItemUid = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    MicrotingUid = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Synced = table.Column<short>(type: "smallint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "field_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    FieldType = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_field_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "field_value_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Accuracy = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Altitude = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    CaseId = table.Column<int>(type: "int", nullable: true),
                    CheckListDuplicateId = table.Column<int>(type: "int", nullable: true),
                    CheckListId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DoneAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FieldId = table.Column<int>(type: "int", nullable: true),
                    FieldValueId = table.Column<int>(type: "int", nullable: true),
                    Heading = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Latitude = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Longitude = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UploadedDataId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_field_value_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "field_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BarcodeEnabled = table.Column<short>(type: "smallint", nullable: true),
                    BarcodeType = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    CheckListId = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Custom = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DecimalCount = table.Column<int>(type: "int", nullable: true),
                    DefaultValue = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DisplayIndex = table.Column<int>(type: "int", nullable: true),
                    Dummy = table.Column<short>(type: "smallint", nullable: true),
                    EntityGroupId = table.Column<int>(type: "int", nullable: true),
                    FieldId = table.Column<int>(type: "int", nullable: true),
                    FieldTypeId = table.Column<int>(type: "int", nullable: true),
                    GeolocationEnabled = table.Column<short>(type: "smallint", nullable: true),
                    GeolocationForced = table.Column<short>(type: "smallint", nullable: true),
                    GeolocationHidden = table.Column<short>(type: "smallint", nullable: true),
                    IsNum = table.Column<short>(type: "smallint", nullable: true),
                    KeyValuePairList = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Label = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Mandatory = table.Column<short>(type: "smallint", nullable: true),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    MaxValue = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    MinValue = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Multi = table.Column<int>(type: "int", nullable: true),
                    Optional = table.Column<short>(type: "smallint", nullable: true),
                    OriginalId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ParentFieldId = table.Column<int>(type: "int", nullable: true),
                    QueryType = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    ReadOnly = table.Column<short>(type: "smallint", nullable: true),
                    Selected = table.Column<short>(type: "smallint", nullable: true),
                    Split = table.Column<short>(type: "smallint", nullable: true),
                    StopOnSave = table.Column<short>(type: "smallint", nullable: true),
                    UnitName = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_field_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "folder_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FolderId = table.Column<int>(type: "int", nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_folder_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "language_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_language_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_language_versions_languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "log_exceptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Type = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_exceptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "notification_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Activity = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Exception = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    NotificationUid = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Stacktrace = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Transmission = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "option_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ContinuousOptionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DisplayIndex = table.Column<int>(type: "int", nullable: false),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    NextQuestionId = table.Column<int>(type: "int", nullable: true),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    OptionIndex = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    WeightValue = table.Column<int>(type: "int", nullable: false),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_option_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_option_versions_options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "question_sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    HasChild = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    PossiblyDeployed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Share = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_sets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "setting_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ChangedByName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Name = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false),
                    SettingId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_setting_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "site_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "site_worker_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    SiteWorkerId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_worker_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tag_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Name = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    TagId = table.Column<int>(type: "int", nullable: true),
                    TaggingsCount = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tagging_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CheckListId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TagId = table.Column<int>(type: "int", nullable: true),
                    TaggerId = table.Column<int>(type: "int", nullable: true),
                    TaggingId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tagging_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "unit_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CustomerNo = table.Column<int>(type: "int", nullable: true),
                    InSightVersion = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    LastIp = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    LeftMenuEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Manufacturer = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    Model = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Note = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Os = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    OsVersion = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    OtpCode = table.Column<int>(type: "int", nullable: true),
                    PushEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SeparateFetchSend = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SerialNumber = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    SyncDefaultDelay = table.Column<int>(type: "int", nullable: false),
                    SyncDelayEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SyncDelayPrCheckList = table.Column<int>(type: "int", nullable: false),
                    SyncDialog = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    eFormVersion = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unit_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uploaded_data_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Checksum = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CurrentFile = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Extension = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    FileLocation = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    FileName = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Local = table.Column<short>(type: "smallint", nullable: true),
                    TranscriptionId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UploadedDataId = table.Column<int>(type: "int", nullable: true),
                    UploaderId = table.Column<int>(type: "int", nullable: true),
                    UploaderType = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploaded_data_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "uploaded_datas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Checksum = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CurrentFile = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Extension = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    FileLocation = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    FileName = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Local = table.Column<short>(type: "smallint", nullable: true),
                    TranscriptionId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UploaderId = table.Column<int>(type: "int", nullable: true),
                    UploaderType = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploaded_datas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "worker_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Email = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    FirstName = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_worker_versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "answer_value_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AnswerId = table.Column<int>(type: "int", nullable: false),
                    AnswerValueId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer_value_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_answer_value_versions_answer_values_AnswerValueId",
                        column: x => x.AnswerValueId,
                        principalTable: "answer_values",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "check_list_sites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CheckListId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FolderId = table.Column<int>(type: "int", nullable: true),
                    LastCheckId = table.Column<int>(type: "int", nullable: false),
                    MicrotingUid = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_list_sites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_check_list_sites_check_lists_CheckListId",
                        column: x => x.CheckListId,
                        principalTable: "check_lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_check_list_sites_folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_check_list_sites_sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "question_set_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    HasChild = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    PossiblyDeployed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    QuestionSetId = table.Column<int>(type: "int", nullable: false),
                    Share = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_set_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_question_set_versions_question_sets_QuestionSetId",
                        column: x => x.QuestionSetId,
                        principalTable: "question_sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "survey_configurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    QuestionSetId = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Stop = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TimeOut = table.Column<int>(type: "int", nullable: false),
                    TimeToLive = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey_configurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_survey_configurations_question_sets_QuestionSetId",
                        column: x => x.QuestionSetId,
                        principalTable: "question_sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "field_values",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Accuracy = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Altitude = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    CaseId = table.Column<int>(type: "int", nullable: true),
                    CheckListDuplicateId = table.Column<int>(type: "int", nullable: true),
                    CheckListId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DoneAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FieldId = table.Column<int>(type: "int", nullable: true),
                    Heading = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Latitude = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Longitude = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UploadedDataId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_field_values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_field_values_check_lists_CheckListId",
                        column: x => x.CheckListId,
                        principalTable: "check_lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_field_values_fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_field_values_uploaded_datas_UploadedDataId",
                        column: x => x.UploadedDataId,
                        principalTable: "uploaded_datas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_field_values_workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "site_survey_configurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    SurveyConfigurationId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_survey_configurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_site_survey_configurations_sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_site_survey_configurations_survey_configurations_SurveyConfi~",
                        column: x => x.SurveyConfigurationId,
                        principalTable: "survey_configurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "survey_configuration_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    QuestionSetId = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Stop = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SurveyConfigurationId = table.Column<int>(type: "int", nullable: false),
                    TimeOut = table.Column<int>(type: "int", nullable: false),
                    TimeToLive = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey_configuration_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_survey_configuration_versions_survey_configurations_SurveyCo~",
                        column: x => x.SurveyConfigurationId,
                        principalTable: "survey_configurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "site_survey_configuration_versions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MicrotingUid = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    SiteSurveyConfigurationId = table.Column<int>(type: "int", nullable: false),
                    SurveyConfigurationId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_survey_configuration_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_site_survey_configuration_versions_site_survey_configuration~",
                        column: x => x.SiteSurveyConfigurationId,
                        principalTable: "site_survey_configurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_answer_value_versions_AnswerValueId",
                table: "answer_value_versions",
                column: "AnswerValueId");

            migrationBuilder.CreateIndex(
                name: "IX_answer_values_AnswerId",
                table: "answer_values",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_answer_values_OptionId",
                table: "answer_values",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_answer_values_QuestionId",
                table: "answer_values",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_check_list_sites_CheckListId",
                table: "check_list_sites",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_check_list_sites_FolderId",
                table: "check_list_sites",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_check_list_sites_SiteId",
                table: "check_list_sites",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_check_lists_ParentId",
                table: "check_lists",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_field_values_CheckListId",
                table: "field_values",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_field_values_FieldId",
                table: "field_values",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_field_values_UploadedDataId",
                table: "field_values",
                column: "UploadedDataId");

            migrationBuilder.CreateIndex(
                name: "IX_field_values_WorkerId",
                table: "field_values",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_language_versions_LanguageId",
                table: "language_versions",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_option_versions_OptionId",
                table: "option_versions",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_question_set_versions_QuestionSetId",
                table: "question_set_versions",
                column: "QuestionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_site_survey_configuration_versions_SiteSurveyConfigurationId",
                table: "site_survey_configuration_versions",
                column: "SiteSurveyConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_site_survey_configurations_SiteId",
                table: "site_survey_configurations",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_site_survey_configurations_SurveyConfigurationId",
                table: "site_survey_configurations",
                column: "SurveyConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_survey_configuration_versions_SurveyConfigurationId",
                table: "survey_configuration_versions",
                column: "SurveyConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_survey_configurations_QuestionSetId",
                table: "survey_configurations",
                column: "QuestionSetId");

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
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_units_UnitId",
                table: "answers",
                column: "UnitId",
                principalTable: "units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_cases_check_lists_CheckListId",
                table: "cases",
                column: "CheckListId",
                principalTable: "check_lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_cases_folders_FolderId",
                table: "cases",
                column: "FolderId",
                principalTable: "folders",
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
                name: "FK_LanguageQuestionSets_languages_LanguageId",
                table: "LanguageQuestionSets",
                column: "LanguageId",
                principalTable: "languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageQuestionSets_question_sets_QuestionSetId",
                table: "LanguageQuestionSets",
                column: "QuestionSetId",
                principalTable: "question_sets",
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
                name: "FK_OptionTranslations_languages_LanguageId",
                table: "OptionTranslations",
                column: "LanguageId",
                principalTable: "languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OptionTranslations_options_OptionId",
                table: "OptionTranslations",
                column: "OptionId",
                principalTable: "options",
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
                name: "FK_QuestionTranslations_languages_LanguageId",
                table: "QuestionTranslations",
                column: "LanguageId",
                principalTable: "languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionTranslations_questions_QuestionId",
                table: "QuestionTranslations",
                column: "QuestionId",
                principalTable: "questions",
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
                name: "FK_SiteGroupSites_sites_SiteId",
                table: "SiteGroupSites",
                column: "SiteId",
                principalTable: "sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SiteTags_sites_SiteId",
                table: "SiteTags",
                column: "SiteId",
                principalTable: "sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SiteTags_tags_TagId",
                table: "SiteTags",
                column: "TagId",
                principalTable: "tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
    }
}
