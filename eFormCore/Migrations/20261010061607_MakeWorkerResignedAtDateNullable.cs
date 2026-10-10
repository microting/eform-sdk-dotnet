using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.eForm.Migrations
{
    /// <inheritdoc />
    public partial class MakeWorkerResignedAtDateNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ResignedAtDate",
                table: "WorkerVersions",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResignedAtDate",
                table: "Workers",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            // A worker that is not resigned has no resignation date. Clear the
            // placeholder values on live rows only; WorkerVersions is history
            // and is left as it was written.
            migrationBuilder.Sql(
                "UPDATE `Workers` SET `ResignedAtDate` = NULL WHERE `Resigned` = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Give NULLs a value before the column becomes NOT NULL again.
            // '0001-01-01' is default(DateTime), the value every existing row got
            // when AddingResignedToWorker added the column, so it reads as "unset"
            // instead of inventing a plausible-looking resignation date.
            migrationBuilder.Sql(
                "UPDATE `WorkerVersions` SET `ResignedAtDate` = '0001-01-01 00:00:00' WHERE `ResignedAtDate` IS NULL");
            migrationBuilder.Sql(
                "UPDATE `Workers` SET `ResignedAtDate` = '0001-01-01 00:00:00' WHERE `ResignedAtDate` IS NULL");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResignedAtDate",
                table: "WorkerVersions",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResignedAtDate",
                table: "Workers",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);
        }
    }
}
