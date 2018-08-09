namespace eFormSqlController.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddingExceptionColumnToNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn("dbo.notifications", "exception", c => c.String(maxLength: int.MaxValue));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("dbo.notifications", "exception");
        }
    }
}
