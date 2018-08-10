using Microsoft.EntityFrameworkCore.Migrations;

namespace eFormSqlController.Migrations
{

    public partial class AddTranscriptionIdToUploadedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn("dbo.uploaded_data", "transcription_id", c => c.Int());
            //migrationBuilder.AddColumn("dbo.uploaded_data_versions", "transcription_id", c => c.Int());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("dbo.uploaded_data_versions", "transcription_id");
            migrationBuilder.DropColumn("dbo.uploaded_data", "transcription_id");
        }
    }
}
