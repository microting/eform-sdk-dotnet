using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.eForm.Migrations
{
    public partial class AddingDoneAtEditable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAchievable",
                table: "CheckListVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDoneAtEditable",
                table: "CheckListVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAchievable",
                table: "CheckLists",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDoneAtEditable",
                table: "CheckLists",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DoneAtUserModifiable",
                table: "CaseVersions",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DoneAtUserModifiable",
                table: "Cases",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAchievable",
                table: "CheckListVersions");

            migrationBuilder.DropColumn(
                name: "IsDoneAtEditable",
                table: "CheckListVersions");

            migrationBuilder.DropColumn(
                name: "IsAchievable",
                table: "CheckLists");

            migrationBuilder.DropColumn(
                name: "IsDoneAtEditable",
                table: "CheckLists");

            migrationBuilder.DropColumn(
                name: "DoneAtUserModifiable",
                table: "CaseVersions");

            migrationBuilder.DropColumn(
                name: "DoneAtUserModifiable",
                table: "Cases");
        }
    }
}
