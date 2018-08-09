using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eFormSqlController.Migrations
{
    public partial class RenamingTablesToEnableInterpolation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "dbo.version_check_list_sites", newName: "check_list_site_versions");
            migrationBuilder.RenameTable(name: "dbo.version_check_list_values", newName: "check_list_value_versions");
            migrationBuilder.RenameTable(name: "dbo.version_check_lists", newName: "check_list_versions");
            migrationBuilder.RenameTable(name: "dbo.version_data_uploaded", newName: "uploaded_data_versions");
            migrationBuilder.RenameTable(name: "dbo.version_entity_groups", newName: "entity_group_versions");
            migrationBuilder.RenameTable(name: "dbo.version_entity_items", newName: "entity_item_versions");
            migrationBuilder.RenameTable(name: "dbo.version_field_values", newName: "field_value_versions");
            migrationBuilder.RenameTable(name: "dbo.version_fields", newName: "field_versions");
            migrationBuilder.RenameTable(name: "dbo.version_site_workers", newName: "site_worker_versions");
            migrationBuilder.RenameTable(name: "dbo.version_units", newName: "unit_versions");
            migrationBuilder.RenameTable(name: "dbo.version_workers", newName: "worker_versions");
            migrationBuilder.RenameTable(name: "dbo.version_sites", newName: "site_versions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "dbo.site_versions", newName: "version_sites");
            migrationBuilder.RenameTable(name: "dbo.worker_versions", newName: "version_workers");
            migrationBuilder.RenameTable(name: "dbo.unit_versions", newName: "version_units");
            migrationBuilder.RenameTable(name: "dbo.site_worker_versions", newName: "version_site_workers");
            migrationBuilder.RenameTable(name: "dbo.field_versions", newName: "version_fields");
            migrationBuilder.RenameTable(name: "dbo.field_value_versions", newName: "version_field_values");
            migrationBuilder.RenameTable(name: "dbo.entity_item_versions", newName: "version_entity_items");
            migrationBuilder.RenameTable(name: "dbo.entity_group_versions", newName: "version_entity_groups");
            migrationBuilder.RenameTable(name: "dbo.uploaded_data_versions", newName: "version_data_uploaded");
            migrationBuilder.RenameTable(name: "dbo.check_list_versions", newName: "version_check_lists");
            migrationBuilder.RenameTable(name: "dbo.check_list_value_versions", newName: "version_check_list_values");
            migrationBuilder.RenameTable(name: "dbo.check_list_site_versions", newName: "version_check_list_sites");
        }
    }
}
