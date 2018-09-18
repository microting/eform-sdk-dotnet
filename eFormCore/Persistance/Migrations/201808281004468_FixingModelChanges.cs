namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingModelChanges : DbMigration
    {
        public override void Up()
        {
            try { DropForeignKey("dbo.check_lists", "tags_id", "dbo.tags"); } catch { }
            try { DropIndex("dbo.check_lists", new[] { "tags_id" }); } catch { }            
            try { DropColumn("dbo.check_lists", "tags_id"); } catch { }            
        }
        
        public override void Down()
        {
            AddColumn("dbo.check_lists", "tags_id", c => c.Int());
            CreateIndex("dbo.check_lists", "tags_id");
            AddForeignKey("dbo.check_lists", "tags_id", "dbo.tags", "id");
        }
    }
}
