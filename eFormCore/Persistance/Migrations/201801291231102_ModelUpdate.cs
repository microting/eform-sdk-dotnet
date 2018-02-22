namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelUpdate : DbMigration
    {
        public override void Up()
        {

            DropForeignKey("dbo.check_lists", "tags_id", "dbo.tags");
            DropIndex("dbo.check_lists", new[] { "tags_id" });
            DropColumn("dbo.check_lists", "tags_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.check_lists", "tags_id", c => c.Int());
            CreateIndex("dbo.check_lists", "tags_id");
            AddForeignKey("dbo.check_lists", "tags_id", "dbo.tags", "id");
        }
    }
}
