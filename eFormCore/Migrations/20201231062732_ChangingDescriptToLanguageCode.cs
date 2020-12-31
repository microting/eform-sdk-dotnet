using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class ChangingDescriptToLanguageCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Languages",
                "LanguageCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LanguageCode",
                table: "Languages",
                "Description");
        }
    }
}
