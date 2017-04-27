namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMissingColumnsNotifications : DbMigration
    {
        public override void Up()
        {
            try
            {
                AddColumn("dbo.notifications", "notification_uid", c => c.String());
            } catch { }
            try
            {
                AddColumn("dbo.notifications", "activity", c => c.String());
            } catch { }
        }
        
        public override void Down()
        {
            DropColumn("dbo.notifications", "notification_uid");
            DropColumn("dbo.notifications", "activity");
        }
    }
}
