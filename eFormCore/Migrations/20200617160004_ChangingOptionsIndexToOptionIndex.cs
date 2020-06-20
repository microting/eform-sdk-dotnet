using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class ChangingOptionsIndexToOptionIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OptionsIndex",
                table: "options",
                newName: "OptionIndex");

            migrationBuilder.RenameColumn(
                name: "OptionsIndex",
                table: "option_versions",
                newName: "OptionIndex");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OptionIndex",
                table: "options",
                newName: "OptionsIndex");

            migrationBuilder.RenameColumn(
                name: "OptionIndex",
                table: "option_versions",
                newName: "OptionsIndex");
        }
    }
}
