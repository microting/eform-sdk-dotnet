using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class CLAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEditable",
                table: "CheckListVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "CheckListVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "CheckListVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEditable",
                table: "CheckLists",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "CheckLists",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "CheckLists",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEditable",
                table: "CheckListVersions");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "CheckListVersions");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "CheckListVersions");

            migrationBuilder.DropColumn(
                name: "IsEditable",
                table: "CheckLists");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "CheckLists");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "CheckLists");
        }
    }
}
