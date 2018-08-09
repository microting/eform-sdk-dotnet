namespace eFormSqlController.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey("dbo.check_lists", "tags_id", "dbo.tags");
            migrationBuilder.DropIndex("dbo.check_lists", new[] { "tags_id" });
            migrationBuilder.DropColumn("dbo.check_lists", "tags_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn("dbo.check_lists", "tags_id", c => c.Int());
            migrationBuilder.CreateIndex("dbo.check_lists", "tags_id");
            migrationBuilder.AddForeignKey("dbo.check_lists", "tags_id", "dbo.tags", "id");
        }
    }
}
