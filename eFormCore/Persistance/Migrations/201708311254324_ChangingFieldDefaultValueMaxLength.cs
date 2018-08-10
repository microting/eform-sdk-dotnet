namespace eFormSqlController.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangingFieldDefaultValueMaxLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn("dbo.fields", "default_value", c => c.String(maxLength: int.MaxValue));
            //migrationBuilder.AlterColumn("dbo.field_versions", "default_value", c => c.String(maxLength: int.MaxValue));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn("dbo.fields", "default_value", c => c.String(maxLength: 255));
            //migrationBuilder.AlterColumn("dbo.field_versions", "default_value", c => c.String(maxLength: 255));
        }
    }
}
