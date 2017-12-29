namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveingReversedFromAInteractiveCase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.a_interaction_case_list_versions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        a_interaction_case_list_version_id = c.Int(),
                        workflow_state = c.String(maxLength: 255),
                        version = c.Int(),
                        created_at = c.DateTime(precision: 7, storeType: "datetime2"),
                        updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
                        siteId = c.Int(),
                        stat = c.String(),
                        microting_uid = c.String(),
                        check_uid = c.String(),
                        case_id = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.a_interaction_case_versions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        a_interaction_case_id = c.Int(),
                        workflow_state = c.String(maxLength: 255),
                        version = c.Int(),
                        created_at = c.DateTime(precision: 7, storeType: "datetime2"),
                        updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
                        template_id = c.Int(nullable: false),
                        case_uid = c.String(maxLength: 255),
                        custom = c.String(),
                        reversed = c.Short(),
                        connected = c.Short(),
                        replacements = c.String(),
                        synced = c.Short(),
                        expectionString = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            DropColumn("dbo.a_interaction_cases", "reversed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.a_interaction_cases", "reversed", c => c.Short());
            DropTable("dbo.a_interaction_case_versions");
            DropTable("dbo.a_interaction_case_list_versions");
        }
    }
}
