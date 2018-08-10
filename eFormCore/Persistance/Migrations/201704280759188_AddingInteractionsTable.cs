namespace eFormSqlController.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddingInteractionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    "dbo.a_interaction_case_lists",
            //    c => new
            //    {
            //        id = c.Int(nullable: false, identity: true),
            //        workflow_state = c.String(maxLength: 255),
            //        version = c.Int(),
            //        created_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        a_interaction_case_id = c.Int(),
            //        siteId = c.Int(),
            //        stat = c.String(),
            //        microting_uid = c.String(),
            //        check_uid = c.String(),
            //        case_id = c.Int(),
            //    })
            //    .PrimaryKey(t => t.id)
            //    .ForeignKey("dbo.a_interaction_cases", t => t.a_interaction_case_id)
            //    .Index(t => t.a_interaction_case_id);

            //migrationBuilder.CreateTable(
            //    "dbo.a_interaction_cases",
            //    c => new
            //    {
            //        id = c.Int(nullable: false, identity: true),
            //        workflow_state = c.String(maxLength: 255),
            //        version = c.Int(),
            //        created_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        template_id = c.Int(nullable: false),
            //        case_uid = c.String(maxLength: 255),
            //        custom = c.String(),
            //        reversed = c.Short(),
            //        connected = c.Short(),
            //        replacements = c.String(),
            //        synced = c.Short(),
            //        expectionString = c.String(),
            //    })
            //    .PrimaryKey(t => t.id);

            migrationBuilder.DropTable("dbo.a_input_cases");
            migrationBuilder.DropTable("dbo.a_output_cases");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    "dbo.a_output_cases",
            //    c => new
            //    {
            //        id = c.Int(nullable: false, identity: true),
            //        workflow_state = c.String(maxLength: 255),
            //        created_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        microting_uid = c.String(maxLength: 255),
            //        check_uid = c.String(),
            //        check_list_id = c.Int(nullable: false),
            //        stat = c.String(maxLength: 255),
            //        site_uid = c.Int(nullable: false),
            //        case_type = c.String(),
            //        case_uid = c.String(maxLength: 255),
            //        custom = c.String(),
            //        case_id = c.Int(nullable: false),
            //    })
            //    .PrimaryKey(t => t.id);

            //migrationBuilder.CreateTable(
            //    "dbo.a_input_cases",
            //    c => new
            //    {
            //        id = c.Int(nullable: false, identity: true),
            //        workflow_state = c.String(maxLength: 255),
            //        created_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        site_uids = c.String(),
            //        case_uid = c.String(maxLength: 255),
            //        custom = c.String(),
            //        reversed = c.Short(),
            //        microting_uids = c.String(maxLength: 255),
            //        connected = c.Short(),
            //        template_id = c.Int(nullable: false),
            //        replacements = c.String(),
            //    })
            //    .PrimaryKey(t => t.id);

            migrationBuilder.DropForeignKey("dbo.a_interaction_case_lists", "a_interaction_case_id", "dbo.a_interaction_cases");
            //migrationBuilder.DropIndex("dbo.a_interaction_case_lists", new[] { "a_interaction_case_id" });
            migrationBuilder.DropTable("dbo.a_interaction_cases");
            migrationBuilder.DropTable("dbo.a_interaction_case_lists");
        }
    }
}
