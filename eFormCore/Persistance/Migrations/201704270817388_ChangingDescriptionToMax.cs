namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ChangingDescriptionToMax : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.check_lists", "description", c => c.String(maxLength: int.MaxValue));
            AlterColumn("dbo.fields", "description", c => c.String(maxLength: int.MaxValue));
            AlterColumn("dbo.check_list_versions", "description", c => c.String(maxLength: int.MaxValue));
            AlterColumn("dbo.field_versions", "description", c => c.String(maxLength: int.MaxValue));
        }

        public override void Down()
        {
            AlterColumn("dbo.check_lists", "description", c => c.String(maxLength: 255));
            AlterColumn("dbo.fields", "description", c => c.String(maxLength: 255));
            AlterColumn("dbo.check_list_versions", "description", c => c.String(maxLength: 255));
            AlterColumn("dbo.field_versions", "description", c => c.String(maxLength: 255));
        }
    }
}
