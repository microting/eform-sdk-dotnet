namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingEntityGroupIdToInt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.entity_item_versions", "migrated_entity_group_id", c => c.Boolean(nullable: false));
            AddColumn("dbo.entity_items", "migrated_entity_group_id", c => c.Boolean(nullable: false));
            AlterColumn("dbo.entity_item_versions", "entity_group_id", c => c.Int());
            AlterColumn("dbo.entity_items", "entity_group_id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.entity_items", "entity_group_id", c => c.String(unicode: false));
            AlterColumn("dbo.entity_item_versions", "entity_group_id", c => c.String(unicode: false));
            DropColumn("dbo.entity_items", "migrated_entity_group_id");
            DropColumn("dbo.entity_item_versions", "migrated_entity_group_id");
        }
    }
}
