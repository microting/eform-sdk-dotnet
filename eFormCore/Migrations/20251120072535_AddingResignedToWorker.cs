using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.eForm.Migrations
{
    /// <inheritdoc />
    public partial class AddingResignedToWorker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Resigned",
                table: "WorkerVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResignedAtDate",
                table: "WorkerVersions",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Resigned",
                table: "Workers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResignedAtDate",
                table: "Workers",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resigned",
                table: "WorkerVersions");

            migrationBuilder.DropColumn(
                name: "ResignedAtDate",
                table: "WorkerVersions");

            migrationBuilder.DropColumn(
                name: "Resigned",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "ResignedAtDate",
                table: "Workers");
        }
    }
}
