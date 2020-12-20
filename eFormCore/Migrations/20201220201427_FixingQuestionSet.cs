using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class FixingQuestionSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("PosiblyDeployed", "question_sets", "PossiblyDeployed");

            migrationBuilder.CreateIndex(
                name: "IX_field_values_UploadedDataId",
                table: "field_values",
                column: "UploadedDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_field_values_uploaded_datas_UploadedDataId",
                table: "field_values",
                column: "UploadedDataId",
                principalTable: "uploaded_datas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_field_values_uploaded_datas_UploadedDataId",
                table: "field_values");

            migrationBuilder.DropIndex(
                name: "IX_field_values_UploadedDataId",
                table: "field_values");

            migrationBuilder.DropColumn(
                name: "PossiblyDeployed",
                table: "question_sets");

            migrationBuilder.AddColumn<bool>(
                name: "PosiblyDeployed",
                table: "question_sets",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UploadedDatasId",
                table: "field_values",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_field_values_UploadedDatasId",
                table: "field_values",
                column: "UploadedDatasId");

            migrationBuilder.AddForeignKey(
                name: "FK_field_values_uploaded_datas_UploadedDatasId",
                table: "field_values",
                column: "UploadedDatasId",
                principalTable: "uploaded_datas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
