namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingStackTraceToNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.notifications", "stacktrace", c => c.String(maxLength: int.MaxValue));
        }
        
        public override void Down()
        {
            DropColumn("dbo.notifications", "stacktrace");
        }
    }
}
