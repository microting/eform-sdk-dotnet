using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class FixingSplitScreen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SplitScreen",
                table: "fields",
                newName: "Split");

            migrationBuilder.RenameColumn(
                name: "SplitScreen",
                table: "field_versions",
                newName: "Split");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Split",
                table: "fields",
                newName: "SplitScreen");

            migrationBuilder.RenameColumn(
                name: "Split",
                table: "field_versions",
                newName: "SplitScreen");
        }
    }
}
