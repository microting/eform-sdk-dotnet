namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingEntityGroupIdToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.entity_items", "entity_group_id", c => c.Int(nullable: false));
            AlterColumn("dbo.entity_item_versions", "entity_group_id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.entity_item_versions", "entity_group_id", c => c.String(unicode: false));
            AlterColumn("dbo.entity_items", "entity_group_id", c => c.String(unicode: false));
        }
    }
}
