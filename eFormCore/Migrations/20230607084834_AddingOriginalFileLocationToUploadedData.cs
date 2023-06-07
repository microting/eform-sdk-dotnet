using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.eForm.Migrations
{
    /// <inheritdoc />
    public partial class AddingOriginalFileLocationToUploadedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriginalFileLocation",
                table: "UploadedDataVersions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OriginalFileLocation",
                table: "UploadedDatas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalFileLocation",
                table: "UploadedDataVersions");

            migrationBuilder.DropColumn(
                name: "OriginalFileLocation",
                table: "UploadedDatas");
        }
    }
}
