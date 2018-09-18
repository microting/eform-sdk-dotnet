namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMigrationBoolToEntityItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.entity_item_versions", "migrated_entity_group_id", c => c.Boolean(nullable: false));
            AddColumn("dbo.entity_items", "migrated_entity_group_id", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.entity_items", "migrated_entity_group_id");
            DropColumn("dbo.entity_item_versions", "migrated_entity_group_id");
        }
    }
}
