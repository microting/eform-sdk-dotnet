using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingAttributesToUnits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SoftwareVersion",
                table: "units",
                newName: "eFormVersion");

            migrationBuilder.RenameColumn(
                name: "SoftwareVersion",
                table: "unit_versions",
                newName: "eFormVersion");

            migrationBuilder.AddColumn<string>(
                name: "InSightVersion",
                table: "units",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Os",
                table: "units",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InSightVersion",
                table: "unit_versions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Os",
                table: "unit_versions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InSightVersion",
                table: "units");

            migrationBuilder.DropColumn(
                name: "Os",
                table: "units");

            migrationBuilder.DropColumn(
                name: "InSightVersion",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "Os",
                table: "unit_versions");

            migrationBuilder.RenameColumn(
                name: "eFormVersion",
                table: "units",
                newName: "SoftwareVersion");

            migrationBuilder.RenameColumn(
                name: "eFormVersion",
                table: "unit_versions",
                newName: "SoftwareVersion");
        }
    }
}
