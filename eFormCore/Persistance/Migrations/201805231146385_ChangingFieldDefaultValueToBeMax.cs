namespace eFormSqlController.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangingFieldDefaultValueToBeMax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn("dbo.field_versions", "default_value", c => c.String(unicode: false, maxLength: int.MaxValue));
            //migrationBuilder.AlterColumn("dbo.fields", "default_value", c => c.String(unicode: false, maxLength: int.MaxValue));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn("dbo.field_versions", "default_value", c => c.String(maxLength: 255, unicode: false));
            //migrationBuilder.AlterColumn("dbo.fields", "default_value", c => c.String(maxLength: 255, unicode: false));
        }
    }
}
