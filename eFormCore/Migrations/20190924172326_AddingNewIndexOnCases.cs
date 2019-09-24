using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingNewIndexOnCases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_cases_MicrotingUid_MicrotingCheckUid",
                table: "cases",
                columns: new[] { "MicrotingUid", "MicrotingCheckUid" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_cases_MicrotingUid_MicrotingCheckUid",
                table: "cases");
        }
    }
}
