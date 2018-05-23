namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingFieldDefaultValueToBeMax : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.fields", "default_value", c => c.String(unicode: false, maxLength: int.MaxValue));
            AlterColumn("dbo.field_versions", "default_value", c => c.String(unicode: false, maxLength: int.MaxValue));
        }

        public override void Down()
        {
            AlterColumn("dbo.fields", "default_value", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("dbo.fields", "field_versions", c => c.String(maxLength: 255, unicode: false));
        }
    }
}
