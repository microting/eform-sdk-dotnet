using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingExtraFieldValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExtraFieldValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DoneAt = table.Column<DateTime>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    WorkerId = table.Column<int>(nullable: true),
                    CaseId = table.Column<int>(nullable: true),
                    CheckListId = table.Column<int>(nullable: true),
                    CheckListDuplicateId = table.Column<int>(nullable: true),
                    CheckListValueId = table.Column<int>(nullable: true),
                    UploadedDataId = table.Column<int>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Latitude = table.Column<string>(maxLength: 255, nullable: true),
                    Longitude = table.Column<string>(maxLength: 255, nullable: true),
                    Altitude = table.Column<string>(maxLength: 255, nullable: true),
                    Heading = table.Column<string>(maxLength: 255, nullable: true),
                    Accuracy = table.Column<string>(maxLength: 255, nullable: true),
                    FieldType = table.Column<string>(nullable: true),
                    FieldTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraFieldValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtraFieldValueVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    ExtraFieldValueId = table.Column<int>(nullable: false),
                    DoneAt = table.Column<DateTime>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    WorkerId = table.Column<int>(nullable: true),
                    CaseId = table.Column<int>(nullable: true),
                    CheckListId = table.Column<int>(nullable: true),
                    CheckListDuplicateId = table.Column<int>(nullable: true),
                    CheckListValueId = table.Column<int>(nullable: true),
                    UploadedDataId = table.Column<int>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Latitude = table.Column<string>(maxLength: 255, nullable: true),
                    Longitude = table.Column<string>(maxLength: 255, nullable: true),
                    Altitude = table.Column<string>(maxLength: 255, nullable: true),
                    Heading = table.Column<string>(maxLength: 255, nullable: true),
                    Accuracy = table.Column<string>(maxLength: 255, nullable: true),
                    FieldType = table.Column<string>(nullable: true),
                    FieldTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraFieldValueVersions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtraFieldValues");

            migrationBuilder.DropTable(
                name: "ExtraFieldValueVersions");
        }
    }
}
