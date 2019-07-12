using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingMissingParentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "folders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "folder_versions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "folders");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "folder_versions");
        }
    }
}
