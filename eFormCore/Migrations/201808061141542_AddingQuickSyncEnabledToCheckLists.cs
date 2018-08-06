namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingQuickSyncEnabledToCheckLists : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.check_lists", "quick_sync_enabled", c => c.Short());
            AddColumn("dbo.check_list_versions", "quick_sync_enabled", c => c.Short());
        }
        
        public override void Down()
        {
            DropColumn("dbo.check_list_versions", "quick_sync_enabled");
            DropColumn("dbo.check_lists", "quick_sync_enabled");
        }
    }
}
