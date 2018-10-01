namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFolderStructure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.folder_versions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        workflow_state = c.String(maxLength: 255),
                        version = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        microting_uuid = c.String(),
                        name = c.String(),
                        description = c.String(),
                        parent_id = c.Int(nullable: false),
                        display_order = c.Int(),
                        update_status = c.Short(),
                        no_click = c.Short(),
                        folders_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.folders",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        workflow_state = c.String(maxLength: 255),
                        version = c.Int(nullable: false),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        microting_uuid = c.String(),
                        name = c.String(),
                        description = c.String(),
                        parent_id = c.Int(nullable: false),
                        display_order = c.Int(nullable: false),
                        update_status = c.Short(),
                        no_click = c.Short(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.folders");
            DropTable("dbo.folder_versions");
        }
    }
}
