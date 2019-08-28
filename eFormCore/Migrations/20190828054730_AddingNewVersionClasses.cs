using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingNewVersionClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangedByName",
                table: "settings",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "settings",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "settings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "settings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "notifications",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedByName",
                table: "settings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "settings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "settings");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "settings");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "notifications");
        }
    }
}
