using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingJasperDocxEnabledAttributesToCheckList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DocxExportEnabled",
                table: "check_lists",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "JasperExportEnabled",
                table: "check_lists",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DocxExportEnabled",
                table: "check_list_versions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "JasperExportEnabled",
                table: "check_list_versions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocxExportEnabled",
                table: "check_lists");

            migrationBuilder.DropColumn(
                name: "JasperExportEnabled",
                table: "check_lists");

            migrationBuilder.DropColumn(
                name: "DocxExportEnabled",
                table: "check_list_versions");

            migrationBuilder.DropColumn(
                name: "JasperExportEnabled",
                table: "check_list_versions");
        }
    }
}
