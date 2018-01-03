namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddInputAndOutputTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.a_input_cases",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    workflow_state = c.String(maxLength: 255, unicode: false),
                    created_at = c.DateTime(precision: 0, storeType: "datetime2"),
                    updated_at = c.DateTime(precision: 0, storeType: "datetime2"),
                    site_uids = c.String(unicode: false),
                    case_uid = c.String(maxLength: 255, unicode: false),
                    custom = c.String(unicode: false),
                    reversed = c.Short(),
                    microting_uids = c.String(maxLength: 255, unicode: false),
                    connected = c.Short(),
                    template_id = c.Int(),
                    replacements = c.String(unicode: false)
                })
                .PrimaryKey(t => t.id);
            CreateTable(
                "dbo.a_output_cases",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    workflow_state = c.String(maxLength: 255, unicode: false),
                    created_at = c.DateTime(precision: 0, storeType: "datetime2"),
                    updated_at = c.DateTime(precision: 0, storeType: "datetime2"),
                    microting_uid = c.String(maxLength: 255, unicode: false),
                    check_uid = c.String(unicode: false),
                    check_list_id = c.Int(),
                    stat = c.String(maxLength: 255, unicode: false),
                    site_uid = c.String(unicode: false),
                    case_type = c.String(unicode: false),
                    case_uid = c.String(maxLength: 255, unicode: false),
                    custom = c.String(unicode: false),
                    case_id = c.Int()
                })
                .PrimaryKey(t => t.id);
        }

        public override void Down()
        {
            DropTable("dbo.a_input_cases");
            DropTable("dbo.a_output_cases");
        }
    }
}
