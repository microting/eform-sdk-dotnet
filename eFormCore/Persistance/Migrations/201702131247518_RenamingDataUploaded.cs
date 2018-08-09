using Microsoft.EntityFrameworkCore.Migrations;

namespace eFormSqlController.Migrations
{
    public partial class RenamingDataUploaded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "dbo.data_uploaded", newName: "uploaded_data");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "dbo.uploaded_data", newName: "data_uploaded");
        }
    }
}
