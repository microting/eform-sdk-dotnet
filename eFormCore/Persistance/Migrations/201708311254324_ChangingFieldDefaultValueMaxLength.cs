namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingFieldDefaultValueMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.fields", "default_value", c => c.String(maxLength: int.MaxValue));
            AlterColumn("dbo.field_versions", "default_value", c => c.String(maxLength: int.MaxValue));
        }

        public override void Down()
        {
            AlterColumn("dbo.fields", "default_value", c => c.String(maxLength: 255));
            AlterColumn("dbo.field_versions", "default_value", c => c.String(maxLength: 255));
        }
    }
}
