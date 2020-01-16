using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingSiteTaggins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Setup for SQL Server Provider
           
            string autoIDGenStrategy = "SqlServer:ValueGenerationStrategy";
            object autoIDGenStrategyValue= SqlServerValueGenerationStrategy.IdentityColumn;

            // Setup for MySQL Provider
            if (migrationBuilder.ActiveProvider=="Pomelo.EntityFrameworkCore.MySql")
            {
                DbConfig.IsMySQL = true;
                autoIDGenStrategy = "MySql:ValueGenerationStrategy";
                autoIDGenStrategyValue = MySqlValueGenerationStrategy.IdentityColumn;
            }
            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "units",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "units",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "units",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OsVersion",
                table: "units",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoftwareVersion",
                table: "units",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "unit_versions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "unit_versions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "unit_versions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OsVersion",
                table: "unit_versions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoftwareVersion",
                table: "unit_versions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionSetId",
                table: "survey_configurations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionSetId",
                table: "survey_configuration_versions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SiteTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    TagId = table.Column<int>(nullable: true),
                    SiteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteTags_sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SiteTags_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SiteTagVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    Version = table.Column<int>(nullable: true),
                    WorkflowState = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    TagId = table.Column<int>(nullable: true),
                    SiteId = table.Column<int>(nullable: true),
                    SiteTagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteTagVersions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteTags_SiteId",
                table: "SiteTags",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteTags_TagId",
                table: "SiteTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteTags");

            migrationBuilder.DropTable(
                name: "SiteTagVersions");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "units");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "units");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "units");

            migrationBuilder.DropColumn(
                name: "OsVersion",
                table: "units");

            migrationBuilder.DropColumn(
                name: "SoftwareVersion",
                table: "units");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "OsVersion",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "SoftwareVersion",
                table: "unit_versions");

            migrationBuilder.DropColumn(
                name: "QuestionSetId",
                table: "survey_configurations");

            migrationBuilder.DropColumn(
                name: "QuestionSetId",
                table: "survey_configuration_versions");
        }
    }
}
