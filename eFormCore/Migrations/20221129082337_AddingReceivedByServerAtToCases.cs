using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.eForm.Migrations
{
    public partial class AddingReceivedByServerAtToCases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedByServerAt",
                table: "CaseVersions",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedByServerAt",
                table: "Cases",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceivedByServerAt",
                table: "CaseVersions");

            migrationBuilder.DropColumn(
                name: "ReceivedByServerAt",
                table: "Cases");
        }
    }
}
