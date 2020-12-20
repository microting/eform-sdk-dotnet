using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class FixingTableColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                "uploaded_data",
                null,
                "uploaded_datas");

            migrationBuilder.RenameColumn(
                "DataUploadedId",
                "uploaded_data_versions",
                "UploadedDataId");

            migrationBuilder.RenameColumn(
                "EntityItemsId",
                "entity_item_versions",
                "EntityItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_field_values_uploaded_datas_UploadedDatasId",
                table: "field_values");

            migrationBuilder.DropTable(
                name: "uploaded_datas");

            migrationBuilder.DropIndex(
                name: "IX_field_values_UploadedDatasId",
                table: "field_values");

            migrationBuilder.DropColumn(
                name: "UploadedDataId",
                table: "uploaded_data_versions");

            migrationBuilder.DropColumn(
                name: "UploadedDatasId",
                table: "field_values");

            migrationBuilder.DropColumn(
                name: "EntityItemId",
                table: "entity_item_versions");

            migrationBuilder.AddColumn<int>(
                name: "DataUploadedId",
                table: "uploaded_data_versions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EntityItemsId",
                table: "entity_item_versions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "uploaded_data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Checksum = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CurrentFile = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Extension = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    FileLocation = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    FileName = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Local = table.Column<short>(type: "smallint", nullable: true),
                    TranscriptionId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UploaderId = table.Column<int>(type: "int", nullable: true),
                    UploaderType = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploaded_data", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_field_values_UploadedDataId",
                table: "field_values",
                column: "UploadedDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_field_values_uploaded_data_UploadedDataId",
                table: "field_values",
                column: "UploadedDataId",
                principalTable: "uploaded_data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
