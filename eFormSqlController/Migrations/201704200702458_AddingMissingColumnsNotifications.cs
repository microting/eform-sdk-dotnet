namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMissingColumnsNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.notifications", "notification_uid", c => c.String());
            AddColumn("dbo.notifications", "activity", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.notifications", "notification_uid");
            DropColumn("dbo.notifications", "activity");
        }
    }
}
