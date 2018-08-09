namespace eFormSqlController.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RemovingAInteractiveCasesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("dbo.a_interaction_case_lists", "a_interaction_case_id", "dbo.a_interaction_cases");
            migrationBuilder.DropIndex("dbo.a_interaction_case_lists", new[] { "a_interaction_case_id" });
            migrationBuilder.DropTable("dbo.a_interaction_case_list_versions");
            migrationBuilder.DropTable("dbo.a_interaction_case_lists");
            migrationBuilder.DropTable("dbo.a_interaction_cases");
            migrationBuilder.DropTable("dbo.a_interaction_case_versions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "dbo.a_interaction_case_versions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        a_interaction_case_id = c.Int(),
                        workflow_state = c.String(maxLength: 255),
                        version = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
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

            migrationBuilder.CreateTable(
                "dbo.a_interaction_cases",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        workflow_state = c.String(maxLength: 255),
                        version = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        template_id = c.Int(nullable: false),
                        case_uid = c.String(maxLength: 255),
                        custom = c.String(),
                        connected = c.Short(),
                        replacements = c.String(),
                        synced = c.Short(),
                        expectionString = c.String(),
                    })
                .PrimaryKey(t => t.id);

            migrationBuilder.CreateTable(
                "dbo.a_interaction_case_lists",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        workflow_state = c.String(maxLength: 255),
                        version = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        a_interaction_case_id = c.Int(),
                        siteId = c.Int(),
                        stat = c.String(),
                        microting_uid = c.String(),
                        check_uid = c.String(),
                        case_id = c.Int(),
                    })
                .PrimaryKey(t => t.id);

            migrationBuilder.CreateTable(
                "dbo.a_interaction_case_list_versions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        a_interaction_case_list_version_id = c.Int(),
                        workflow_state = c.String(maxLength: 255),
                        version = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        siteId = c.Int(),
                        stat = c.String(),
                        microting_uid = c.String(),
                        check_uid = c.String(),
                        case_id = c.Int(),
                    })
                .PrimaryKey(t => t.id);

            migrationBuilder.CreateIndex("dbo.a_interaction_case_lists", "a_interaction_case_id");
            migrationBuilder.AddForeignKey("dbo.a_interaction_case_lists", "a_interaction_case_id", "dbo.a_interaction_cases", "id");
        }
    }
}
