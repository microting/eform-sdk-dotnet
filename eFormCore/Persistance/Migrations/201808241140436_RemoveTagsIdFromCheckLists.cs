namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTagsIdFromCheckLists : DbMigration
    {
        public override void Up()
        {
            // FK_dbo.check_lists_dbo.tags_tags_id
            DropIndex("dbo.check_lists", new[] { "tags_id" });
            DropForeignKey("dbo.check_lists", "tags_id", "dbo.tags");
            DropColumn("dbo.check_lists", "tags_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.check_lists", "tags_id", c => c.Int());
        }
    }
}
