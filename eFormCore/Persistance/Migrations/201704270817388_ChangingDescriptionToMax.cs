namespace eFormSqlController.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangingDescriptionToMax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn("dbo.check_lists", "description", c => c.String(maxLength: int.MaxValue));
            migrationBuilder.AlterColumn("dbo.fields", "description", c => c.String(maxLength: int.MaxValue));
            migrationBuilder.AlterColumn("dbo.check_list_versions", "description", c => c.String(maxLength: int.MaxValue));
            migrationBuilder.AlterColumn("dbo.field_versions", "description", c => c.String(maxLength: int.MaxValue));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn("dbo.check_lists", "description", c => c.String(maxLength: 255));
            migrationBuilder.AlterColumn("dbo.fields", "description", c => c.String(maxLength: 255));
            migrationBuilder.AlterColumn("dbo.check_list_versions", "description", c => c.String(maxLength: 255));
            migrationBuilder.AlterColumn("dbo.field_versions", "description", c => c.String(maxLength: 255));
        }
    }
}
