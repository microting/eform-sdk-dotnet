using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingFolderIdToCasesAndCheckListSites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FolderId",
                table: "check_list_sites",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FolderId",
                table: "check_list_site_versions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FolderId",
                table: "cases",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FolderId",
                table: "case_versions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_check_list_sites_FolderId",
                table: "check_list_sites",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_cases_FolderId",
                table: "cases",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_cases_folders_FolderId",
                table: "cases",
                column: "FolderId",
                principalTable: "folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_check_list_sites_folders_FolderId",
                table: "check_list_sites",
                column: "FolderId",
                principalTable: "folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cases_folders_FolderId",
                table: "cases");

            migrationBuilder.DropForeignKey(
                name: "FK_check_list_sites_folders_FolderId",
                table: "check_list_sites");

            migrationBuilder.DropIndex(
                name: "IX_check_list_sites_FolderId",
                table: "check_list_sites");

            migrationBuilder.DropIndex(
                name: "IX_cases_FolderId",
                table: "cases");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "check_list_sites");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "check_list_site_versions");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "cases");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "case_versions");
        }
    }
}
