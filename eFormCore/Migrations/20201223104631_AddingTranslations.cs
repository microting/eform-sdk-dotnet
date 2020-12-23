using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingTranslations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogExceptions");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "SiteVersions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Sites",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReportH1",
                table: "CheckListVersions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportH2",
                table: "CheckListVersions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportH3",
                table: "CheckListVersions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportH4",
                table: "CheckListVersions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportH5",
                table: "CheckListVersions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportH1",
                table: "CheckLists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportH2",
                table: "CheckLists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportH3",
                table: "CheckLists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportH4",
                table: "CheckLists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportH5",
                table: "CheckLists",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CheckLisTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    CheckListId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "CheckListTranslationVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    CheckListId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    CheckListTranslationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListTranslationVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    FieldId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Selected = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldOptions_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldOptionTranslationVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    FieldOptionId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    FieldOptionTranslation = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOptionTranslationVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldOptionVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    FieldId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Selected = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<string>(nullable: true),
                    FieldOptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOptionVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    FieldId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldTranslations_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldTranslationVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    FieldId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    FieldTranslationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldTranslationVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldOptionTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    FieldOptionId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOptionTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldOptionTranslations_FieldOptions_FieldOptionId",
                        column: x => x.FieldOptionId,
                        principalTable: "FieldOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckLisTranslations_CheckListId",
                table: "CheckLisTranslations",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOptions_FieldId",
                table: "FieldOptions",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOptionTranslations_FieldOptionId",
                table: "FieldOptionTranslations",
                column: "FieldOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldTranslations_FieldId",
                table: "FieldTranslations",
                column: "FieldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckLisTranslations");

            migrationBuilder.DropTable(
                name: "CheckListTranslationVersions");

            migrationBuilder.DropTable(
                name: "FieldOptionTranslations");

            migrationBuilder.DropTable(
                name: "FieldOptionTranslationVersions");

            migrationBuilder.DropTable(
                name: "FieldOptionVersions");

            migrationBuilder.DropTable(
                name: "FieldTranslations");

            migrationBuilder.DropTable(
                name: "FieldTranslationVersions");

            migrationBuilder.DropTable(
                name: "FieldOptions");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "SiteVersions");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "ReportH1",
                table: "CheckListVersions");

            migrationBuilder.DropColumn(
                name: "ReportH2",
                table: "CheckListVersions");

            migrationBuilder.DropColumn(
                name: "ReportH3",
                table: "CheckListVersions");

            migrationBuilder.DropColumn(
                name: "ReportH4",
                table: "CheckListVersions");

            migrationBuilder.DropColumn(
                name: "ReportH5",
                table: "CheckListVersions");

            migrationBuilder.DropColumn(
                name: "ReportH1",
                table: "CheckLists");

            migrationBuilder.DropColumn(
                name: "ReportH2",
                table: "CheckLists");

            migrationBuilder.DropColumn(
                name: "ReportH3",
                table: "CheckLists");

            migrationBuilder.DropColumn(
                name: "ReportH4",
                table: "CheckLists");

            migrationBuilder.DropColumn(
                name: "ReportH5",
                table: "CheckLists");

            migrationBuilder.CreateTable(
                name: "LogExceptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Type = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogExceptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Type = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteGroups_SiteGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "SiteGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SiteGroupSiteVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SiteGroupId = table.Column<int>(type: "int", nullable: false),
                    SiteGroupSiteId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteGroupSiteVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteGroupVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    SiteGroupId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteGroupVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteGroupSites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SiteGroupId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteGroupSites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteGroupSites_SiteGroups_SiteGroupId",
                        column: x => x.SiteGroupId,
                        principalTable: "SiteGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiteGroupSites_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteGroups_ParentId",
                table: "SiteGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteGroupSites_SiteGroupId",
                table: "SiteGroupSites",
                column: "SiteGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteGroupSites_SiteId",
                table: "SiteGroupSites",
                column: "SiteId");
        }
    }
}
