namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingExceptionColumnToNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.notifications", "exception", c => c.String(maxLength: int.MaxValue));
        }
        
        public override void Down()
        {
            DropColumn("dbo.notifications", "exception");
        }
    }
}
