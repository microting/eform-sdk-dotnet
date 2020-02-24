using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingMoreAttributesToUnits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastIp",
                table: "units",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LeftMenuEnabled",
                table: "units",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PushEnabled",
                table: "units",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SeparateFetchSend",
                table: "units",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "units",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SyncDefaultDelay",
                table: "units",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SyncDelayEnabled",
                table: "units",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SyncDelayPrCheckList",
                table: "units",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SyncDialog",
                table: "units",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastIp",
                table: "unit_versions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LeftMenuEnabled",
                table: "unit_versions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PushEnabled",
                table: "unit_versions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SeparateFetchSend",
                table: "unit_versions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "unit_versions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SyncDefaultDelay",
                table: "unit_versions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SyncDelayEnabled",
                table: "unit_versions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SyncDelayPrCheckList",
                table: "unit_versions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SyncDialog",
                table: "unit_versions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastIp",
                table: "units");

            migrationBuilder.DropColumn(
                name: "LeftMenuEnabled",
                table: "units");

            migrationBuilder.DropColumn(
                name: "PushEnabled",
                table: "units");

            migrationBuilder.DropColumn(
                name: "SeparateFetchSend",
                table: "units");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "units");

            migrationBuilder.DropColumn(
                name: "SyncDefaultDelay",
                table: "units");

            migrationBuilder.DropColumn(
                name: "SyncDelayEnabled",
                table: "units");

            migrationBuilder.DropColumn(
                name: "SyncDelayPrCheckList",
                table: "units");

            migrationBuilder.DropColumn(
                name: "SyncDialog",
                table: "units");

            migrationBuilder.DropColumn(
                name: "LastIp",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "LeftMenuEnabled",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "PushEnabled",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "SeparateFetchSend",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "SyncDefaultDelay",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "SyncDelayEnabled",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "SyncDelayPrCheckList",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "SyncDialog",
                table: "unit_versions");
        }
    }
}
