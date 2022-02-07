using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.eForm.Migrations
{
    public partial class AddingIsLockedToSiteUnitWorkers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "WorkerVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Workers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "UnitVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Units",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "SiteVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Sites",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "WorkerVersions");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "UnitVersions");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "SiteVersions");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Sites");
        }
    }
}
