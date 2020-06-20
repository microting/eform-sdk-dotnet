using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingExcelExportEnabledToCheckList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExcelExportEnabled",
                table: "check_lists",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExcelExportEnabled",
                table: "check_list_versions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExcelExportEnabled",
                table: "check_lists");

            migrationBuilder.DropColumn(
                name: "ExcelExportEnabled",
                table: "check_list_versions");
        }
    }
}
