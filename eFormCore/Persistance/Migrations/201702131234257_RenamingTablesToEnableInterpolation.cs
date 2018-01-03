namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RenamingTablesToEnableInterpolation : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.version_check_list_sites", newName: "check_list_site_versions");
            RenameTable(name: "dbo.version_check_list_values", newName: "check_list_value_versions");
            RenameTable(name: "dbo.version_check_lists", newName: "check_list_versions");
            RenameTable(name: "dbo.version_data_uploaded", newName: "uploaded_data_versions");
            RenameTable(name: "dbo.version_entity_groups", newName: "entity_group_versions");
            RenameTable(name: "dbo.version_entity_items", newName: "entity_item_versions");
            RenameTable(name: "dbo.version_field_values", newName: "field_value_versions");
            RenameTable(name: "dbo.version_fields", newName: "field_versions");
            RenameTable(name: "dbo.version_site_workers", newName: "site_worker_versions");
            RenameTable(name: "dbo.version_units", newName: "unit_versions");
            RenameTable(name: "dbo.version_workers", newName: "worker_versions");
            RenameTable(name: "dbo.version_sites", newName: "site_versions");
        }

        public override void Down()
        {
            RenameTable(name: "dbo.site_versions", newName: "version_sites");
            RenameTable(name: "dbo.worker_versions", newName: "version_workers");
            RenameTable(name: "dbo.unit_versions", newName: "version_units");
            RenameTable(name: "dbo.site_worker_versions", newName: "version_site_workers");
            RenameTable(name: "dbo.field_versions", newName: "version_fields");
            RenameTable(name: "dbo.field_value_versions", newName: "version_field_values");
            RenameTable(name: "dbo.entity_item_versions", newName: "version_entity_items");
            RenameTable(name: "dbo.entity_group_versions", newName: "version_entity_groups");
            RenameTable(name: "dbo.uploaded_data_versions", newName: "version_data_uploaded");
            RenameTable(name: "dbo.check_list_versions", newName: "version_check_lists");
            RenameTable(name: "dbo.check_list_value_versions", newName: "version_check_list_values");
            RenameTable(name: "dbo.check_list_site_versions", newName: "version_check_list_sites");
        }
    }
}
