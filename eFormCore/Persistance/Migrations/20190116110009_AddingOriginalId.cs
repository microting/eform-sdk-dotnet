using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingOriginalId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "original_id",
                table: "fields",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "original_id",
                table: "field_versions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "original_id",
                table: "check_lists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "original_id",
                table: "check_list_versions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "original_id",
                table: "fields");

            migrationBuilder.DropColumn(
                name: "original_id",
                table: "field_versions");

            migrationBuilder.DropColumn(
                name: "original_id",
                table: "check_lists");

            migrationBuilder.DropColumn(
                name: "original_id",
                table: "check_list_versions");
        }
    }
}
