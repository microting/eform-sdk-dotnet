using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class FixingBrokenTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "CheckLisTranslations",
                null,
                "ChecklistTranslations");

            migrationBuilder.RenameColumn(
                "FieldOptionTranslation",
                "FieldOptionTranslationVersions",
                "FieldOptionTranslationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckListTranslations");

            migrationBuilder.DropColumn(
                name: "FieldOptionTranslationId",
                table: "FieldOptionTranslationVersions");

            migrationBuilder.AddColumn<int>(
                name: "FieldOptionTranslation",
                table: "FieldOptionTranslationVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CheckLisTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CheckListId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckLisTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckLisTranslations_CheckLists_CheckListId",
                        column: x => x.CheckListId,
                        principalTable: "CheckLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckLisTranslations_CheckListId",
                table: "CheckLisTranslations",
                column: "CheckListId");
        }
    }
}
