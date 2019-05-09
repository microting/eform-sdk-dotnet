using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class RefactoringidtoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_folders_folders_parentid",
                table: "folders");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "workers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "worker_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "uploaded_data_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "uploaded_data",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "units",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "unit_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tags",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "taggings",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tagging_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tag_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "survey_configurations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "survey_configuration_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "sites",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "site_workers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "site_worker_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "site_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "site_survey_configurations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "site_survey_configuration_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "questions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "question_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "question_sets",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "question_set_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "options",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "option_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "languages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "language_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "parentid",
                table: "folders",
                newName: "parentId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "folders",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_folders_parentid",
                table: "folders",
                newName: "IX_folders_parentId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "folder_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "fields",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "field_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "field_values",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "field_value_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "field_types",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "entity_items",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "entity_item_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "entity_groups",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "entity_group_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "check_lists",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "check_list_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "check_list_values",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "check_list_value_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "check_list_sites",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "check_list_site_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "cases",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "case_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "answers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "answer_versions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "answer_values",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "answer_value_versions",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_folders_folders_parentId",
                table: "folders",
                column: "parentId",
                principalTable: "folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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
