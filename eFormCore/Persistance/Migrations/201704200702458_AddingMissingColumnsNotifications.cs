namespace eFormSqlController.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddingMissingColumnsNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            try
            {
                migrationBuilder.AddColumn("dbo.notifications", "notification_uid", c => c.String());
            }
            catch { }
            try
            {
                migrationBuilder.AddColumn("dbo.notifications", "activity", c => c.String());
            }
            catch { }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("dbo.notifications", "notification_uid");
            migrationBuilder.DropColumn("dbo.notifications", "activity");
        }
    }
}
