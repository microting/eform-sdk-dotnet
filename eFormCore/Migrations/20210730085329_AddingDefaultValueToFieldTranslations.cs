using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingDefaultValueToFieldTranslations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "FieldTranslationVersions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "UploadedDataId",
                table: "FieldTranslationVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "FieldTranslations",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "UploadedDataId",
                table: "FieldTranslations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "FieldTranslationVersions");

            migrationBuilder.DropColumn(
                name: "UploadedDataId",
                table: "FieldTranslationVersions");

            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "FieldTranslations");

            migrationBuilder.DropColumn(
                name: "UploadedDataId",
                table: "FieldTranslations");
        }
    }
}
