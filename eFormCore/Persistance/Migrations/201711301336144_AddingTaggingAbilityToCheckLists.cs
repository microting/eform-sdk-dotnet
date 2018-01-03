namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddingTaggingAbilityToCheckLists : DbMigration
    {
        public override void Up()
        {
            CreateTable(
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

            CreateTable(
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

            CreateTable(
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

            CreateTable(
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

            AddColumn("dbo.check_lists", "tags_id", c => c.Int());
            AlterColumn("dbo.a_interaction_case_list_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.a_interaction_case_list_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.a_interaction_case_lists", "created_at", c => c.DateTime());
            AlterColumn("dbo.a_interaction_case_lists", "updated_at", c => c.DateTime());
            AlterColumn("dbo.a_interaction_cases", "created_at", c => c.DateTime());
            AlterColumn("dbo.a_interaction_cases", "updated_at", c => c.DateTime());
            AlterColumn("dbo.a_interaction_case_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.a_interaction_case_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.cases", "created_at", c => c.DateTime());
            AlterColumn("dbo.cases", "updated_at", c => c.DateTime());
            AlterColumn("dbo.cases", "done_at", c => c.DateTime());
            AlterColumn("dbo.check_lists", "created_at", c => c.DateTime());
            AlterColumn("dbo.check_lists", "updated_at", c => c.DateTime());
            AlterColumn("dbo.check_list_sites", "created_at", c => c.DateTime());
            AlterColumn("dbo.check_list_sites", "updated_at", c => c.DateTime());
            AlterColumn("dbo.sites", "created_at", c => c.DateTime());
            AlterColumn("dbo.sites", "updated_at", c => c.DateTime());
            AlterColumn("dbo.site_workers", "created_at", c => c.DateTime());
            AlterColumn("dbo.site_workers", "updated_at", c => c.DateTime());
            AlterColumn("dbo.workers", "created_at", c => c.DateTime());
            AlterColumn("dbo.workers", "updated_at", c => c.DateTime());
            AlterColumn("dbo.units", "created_at", c => c.DateTime());
            AlterColumn("dbo.units", "updated_at", c => c.DateTime());
            AlterColumn("dbo.fields", "created_at", c => c.DateTime());
            AlterColumn("dbo.fields", "updated_at", c => c.DateTime());
            AlterColumn("dbo.field_values", "created_at", c => c.DateTime());
            AlterColumn("dbo.field_values", "updated_at", c => c.DateTime());
            AlterColumn("dbo.field_values", "done_at", c => c.DateTime());
            AlterColumn("dbo.field_values", "date", c => c.DateTime());
            AlterColumn("dbo.uploaded_data", "created_at", c => c.DateTime());
            AlterColumn("dbo.uploaded_data", "updated_at", c => c.DateTime());
            AlterColumn("dbo.uploaded_data", "expiration_date", c => c.DateTime());
            AlterColumn("dbo.check_list_values", "created_at", c => c.DateTime());
            AlterColumn("dbo.check_list_values", "updated_at", c => c.DateTime());
            AlterColumn("dbo.entity_groups", "created_at", c => c.DateTime());
            AlterColumn("dbo.entity_groups", "updated_at", c => c.DateTime());
            AlterColumn("dbo.entity_items", "created_at", c => c.DateTime());
            AlterColumn("dbo.entity_items", "updated_at", c => c.DateTime());
            AlterColumn("dbo.log_exceptions", "created_at", c => c.DateTime(nullable: false));
            AlterColumn("dbo.logs", "created_at", c => c.DateTime(nullable: false));
            AlterColumn("dbo.notifications", "created_at", c => c.DateTime());
            AlterColumn("dbo.notifications", "updated_at", c => c.DateTime());
            AlterColumn("dbo.case_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.case_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.case_versions", "done_at", c => c.DateTime());
            AlterColumn("dbo.check_list_site_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.check_list_site_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.check_list_value_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.check_list_value_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.check_list_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.check_list_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.uploaded_data_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.uploaded_data_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.uploaded_data_versions", "expiration_date", c => c.DateTime());
            AlterColumn("dbo.entity_group_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.entity_group_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.entity_item_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.entity_item_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.field_value_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.field_value_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.field_value_versions", "done_at", c => c.DateTime());
            AlterColumn("dbo.field_value_versions", "date", c => c.DateTime());
            AlterColumn("dbo.field_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.field_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.site_worker_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.site_worker_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.site_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.site_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.unit_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.unit_versions", "updated_at", c => c.DateTime());
            AlterColumn("dbo.worker_versions", "created_at", c => c.DateTime());
            AlterColumn("dbo.worker_versions", "updated_at", c => c.DateTime());
            CreateIndex("dbo.check_lists", "tags_id");
            AddForeignKey("dbo.check_lists", "tags_id", "dbo.tags", "id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.taggings", "tag_id", "dbo.tags");
            DropForeignKey("dbo.check_lists", "tags_id", "dbo.tags");
            DropForeignKey("dbo.taggings", "check_list_id", "dbo.check_lists");
            DropIndex("dbo.taggings", new[] { "check_list_id" });
            DropIndex("dbo.taggings", new[] { "tag_id" });
            DropIndex("dbo.check_lists", new[] { "tags_id" });
            AlterColumn("dbo.worker_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.worker_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.unit_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.unit_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.site_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.site_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.site_worker_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.site_worker_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.field_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.field_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.field_value_versions", "date", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.field_value_versions", "done_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.field_value_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.field_value_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.entity_item_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.entity_item_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.entity_group_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.entity_group_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.uploaded_data_versions", "expiration_date", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.uploaded_data_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.uploaded_data_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.check_list_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.check_list_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.check_list_value_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.check_list_value_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.check_list_site_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.check_list_site_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.case_versions", "done_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.case_versions", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.case_versions", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.notifications", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.notifications", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.logs", "created_at", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.log_exceptions", "created_at", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.entity_items", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.entity_items", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.entity_groups", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.entity_groups", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.check_list_values", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.check_list_values", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.uploaded_data", "expiration_date", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.uploaded_data", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.uploaded_data", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.field_values", "date", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.field_values", "done_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.field_values", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.field_values", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.fields", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.fields", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.units", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.units", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.workers", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.workers", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.site_workers", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.site_workers", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.sites", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.sites", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.check_list_sites", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.check_list_sites", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.check_lists", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.check_lists", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.cases", "done_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.cases", "updated_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.cases", "created_at", c => c.DateTime(precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.a_interaction_case_versions", "updated_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.a_interaction_case_versions", "created_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.a_interaction_cases", "updated_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.a_interaction_cases", "created_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.a_interaction_case_lists", "updated_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.a_interaction_case_lists", "created_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.a_interaction_case_list_versions", "updated_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.a_interaction_case_list_versions", "created_at", c => c.DateTime(precision: 7, storeType: "datetime2"));
            DropColumn("dbo.check_lists", "tags_id");
            DropTable("dbo.tagging_versions");
            DropTable("dbo.tag_versions");
            DropTable("dbo.tags");
            DropTable("dbo.taggings");
        }
    }
}
