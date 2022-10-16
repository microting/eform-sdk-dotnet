using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.eForm.Migrations
{
    public partial class AddingIsActiveToLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "LanguageVersions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Languages",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "LanguageVersions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Languages");
        }
    }
}