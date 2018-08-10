namespace eFormSqlController.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddingQuickSyncEnabledToCheckLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn("dbo.check_lists", "quick_sync_enabled", c => c.Short());
            //migrationBuilder.AddColumn("dbo.check_list_versions", "quick_sync_enabled", c => c.Short());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("dbo.check_list_versions", "quick_sync_enabled");
            migrationBuilder.DropColumn("dbo.check_lists", "quick_sync_enabled");
        }
    }
}
