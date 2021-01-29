using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AdjustTimeToUTC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase();
            migrationBuilder.Sql("update cases set DoneAt = CONVERT_TZ(DoneAt, 'Europe/Copenhagen','UTC')");
            migrationBuilder.Sql("update case_versions set DoneAt = CONVERT_TZ(DoneAt, 'Europe/Copenhagen','UTC')");
            migrationBuilder.Sql("update field_values set DoneAt = CONVERT_TZ(DoneAt, 'Europe/Copenhagen','UTC')");
            migrationBuilder.Sql("update field_value_versions set DoneAt = CONVERT_TZ(DoneAt, 'Europe/Copenhagen','UTC')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
