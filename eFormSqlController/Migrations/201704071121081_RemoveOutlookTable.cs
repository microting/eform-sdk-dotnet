namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveOutlookTable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.outlook");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.outlook",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        workflow_state = c.String(maxLength: 255, unicode: false),
                        version = c.Int(),
                        created_at = c.DateTime(precision: 0, storeType: "datetime2"),
                        updated_at = c.DateTime(precision: 0, storeType: "datetime2"),
                        global_id = c.String(unicode: false),
                        start_at = c.DateTime(precision: 0, storeType: "datetime2"),
                        expire_at = c.DateTime(precision: 0, storeType: "datetime2"),
                        duration = c.Int(),
                        templat_id = c.Int(),
                        subject = c.String(maxLength: 255, unicode: false),
                        location = c.String(maxLength: 255, unicode: false),
                        body = c.String(unicode: false),
                        site_ids = c.String(unicode: false),
                        title = c.String(maxLength: 255, unicode: false),
                        info = c.String(unicode: false),
                        custom_fields = c.String(unicode: false),
                        microting_uid = c.String(maxLength: 255, unicode: false),
                        connected = c.Short(),
                        completed = c.Short(),
                    })
                .PrimaryKey(t => t.id);
        }
    }
}
