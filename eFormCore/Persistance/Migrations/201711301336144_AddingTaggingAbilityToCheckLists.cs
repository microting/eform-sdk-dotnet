namespace eFormSqlController.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddingTaggingAbilityToCheckLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "dbo.taggings",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    tag_id = c.Int(),
                    check_list_id = c.Int(),
                    tagger_id = c.Int(),
                    version = c.Int(),
                    workflow_state = c.String(maxLength: 255, unicode: false),
                    created_at = c.DateTime(),
                    updated_at = c.DateTime(),
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.check_lists", t => t.check_list_id)
                .ForeignKey("dbo.tags", t => t.tag_id)
                .Index(t => t.tag_id)
                .Index(t => t.check_list_id);

            migrationBuilder.CreateTable(
                "dbo.tags",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    created_at = c.DateTime(),
                    updated_at = c.DateTime(),
                    name = c.String(maxLength: 255),
                    taggings_count = c.Int(),
                    version = c.Int(),
                    workflow_state = c.String(maxLength: 255, unicode: false),
                })
                .PrimaryKey(t => t.id);

            migrationBuilder.CreateTable(
                "dbo.tag_versions",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    created_at = c.DateTime(),
                    updated_at = c.DateTime(),
                    name = c.String(maxLength: 255),
                    taggings_count = c.Int(),
                    version = c.Int(),
                    workflow_state = c.String(maxLength: 255, unicode: false),
                    tag_id = c.Int(),
                })
                .PrimaryKey(t => t.id);

            migrationBuilder.CreateTable(
                "dbo.tagging_versions",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    tag_id = c.Int(),
                    check_list_id = c.Int(),
                    tagger_id = c.Int(),
                    version = c.Int(),
                    workflow_state = c.String(maxLength: 255, unicode: false),
                    created_at = c.DateTime(),
                    updated_at = c.DateTime(),
                    tagging_id = c.Int(),
                })
                .PrimaryKey(t => t.id);

            migrationBuilder.AddColumn("dbo.check_lists", "tags_id", c => c.Int());
            migrationBuilder.AlterColumn("dbo.a_interaction_case_list_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.a_interaction_case_list_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.a_interaction_case_lists", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.a_interaction_case_lists", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.a_interaction_cases", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.a_interaction_cases", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.a_interaction_case_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.a_interaction_case_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.cases", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.cases", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.cases", "done_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.check_lists", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.check_lists", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.check_list_sites", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.check_list_sites", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.sites", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.sites", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.site_workers", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.site_workers", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.workers", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.workers", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.units", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.units", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.fields", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.fields", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.field_values", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.field_values", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.field_values", "done_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.field_values", "date", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.uploaded_data", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.uploaded_data", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.uploaded_data", "expiration_date", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.check_list_values", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.check_list_values", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.entity_groups", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.entity_groups", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.entity_items", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.entity_items", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.log_exceptions", "created_at", c => c.DateTime(nullable: false));
            migrationBuilder.AlterColumn("dbo.logs", "created_at", c => c.DateTime(nullable: false));
            migrationBuilder.AlterColumn("dbo.notifications", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.notifications", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.case_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.case_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.case_versions", "done_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.check_list_site_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.check_list_site_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.check_list_value_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.check_list_value_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.check_list_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.check_list_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.uploaded_data_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.uploaded_data_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.uploaded_data_versions", "expiration_date", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.entity_group_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.entity_group_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.entity_item_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.entity_item_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.field_value_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.field_value_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.field_value_versions", "done_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.field_value_versions", "date", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.field_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.field_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.site_worker_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.site_worker_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.site_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.site_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.unit_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.unit_versions", "updated_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.worker_versions", "created_at", c => c.DateTime());
            migrationBuilder.AlterColumn("dbo.worker_versions", "updated_at", c => c.DateTime());
            migrationBuilder.CreateIndex("dbo.check_lists", "tags_id");
            migrationBuilder.AddForeignKey("dbo.check_lists", "tags_id", "dbo.tags", "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("dbo.taggings", "tag_id", "dbo.tags");
            migrationBuilder.DropForeignKey("dbo.check_lists", "tags_id", "dbo.tags");
            migrationBuilder.DropForeignKey("dbo.taggings", "check_list_id", "dbo.check_lists");
            migrationBuilder.DropIndex("dbo.taggings", new[] { "check_list_id" });
            migrationBuilder.DropIndex("dbo.taggings", new[] { "tag_id" });
            migrationBuilder.DropIndex("dbo.check_lists", new[] { "tags_id" });
            migrationBuilder.AlterColumn("dbo.worker_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.worker_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.unit_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.unit_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.site_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.site_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.site_worker_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.site_worker_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.field_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.field_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.field_value_versions", "date", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.field_value_versions", "done_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.field_value_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.field_value_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.entity_item_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.entity_item_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.entity_group_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.entity_group_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.uploaded_data_versions", "expiration_date", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.uploaded_data_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.uploaded_data_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.check_list_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.check_list_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.check_list_value_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.check_list_value_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.check_list_site_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.check_list_site_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.case_versions", "done_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.case_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.case_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.notifications", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.notifications", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.logs", "created_at", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.log_exceptions", "created_at", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.entity_items", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.entity_items", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.entity_groups", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.entity_groups", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.check_list_values", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.check_list_values", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.uploaded_data", "expiration_date", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.uploaded_data", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.uploaded_data", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.field_values", "date", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.field_values", "done_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.field_values", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.field_values", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.fields", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.fields", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.units", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.units", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.workers", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.workers", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.site_workers", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.site_workers", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.sites", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.sites", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.check_list_sites", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.check_list_sites", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.check_lists", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.check_lists", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.cases", "done_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.cases", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.cases", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.a_interaction_case_versions", "updated_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.a_interaction_case_versions", "created_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.a_interaction_cases", "updated_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.a_interaction_cases", "created_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.a_interaction_case_lists", "updated_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.a_interaction_case_lists", "created_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.a_interaction_case_list_versions", "updated_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            migrationBuilder.AlterColumn("dbo.a_interaction_case_list_versions", "created_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            migrationBuilder.DropColumn("dbo.check_lists", "tags_id");
            migrationBuilder.DropTable("dbo.tagging_versions");
            migrationBuilder.DropTable("dbo.tag_versions");
            migrationBuilder.DropTable("dbo.tags");
            migrationBuilder.DropTable("dbo.taggings");
        }
    }
}
