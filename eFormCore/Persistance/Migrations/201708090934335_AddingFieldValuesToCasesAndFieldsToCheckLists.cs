namespace eFormSqlController.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddingFieldValuesToCasesAndFieldsToCheckLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn("dbo.cases", "field_value_1", c => c.String());
            //migrationBuilder.AddColumn("dbo.cases", "field_value_2", c => c.String());
            //migrationBuilder.AddColumn("dbo.cases", "field_value_3", c => c.String());
            //migrationBuilder.AddColumn("dbo.cases", "field_value_4", c => c.String());
            //migrationBuilder.AddColumn("dbo.cases", "field_value_5", c => c.String());
            //migrationBuilder.AddColumn("dbo.cases", "field_value_6", c => c.String());
            //migrationBuilder.AddColumn("dbo.cases", "field_value_7", c => c.String());
            //migrationBuilder.AddColumn("dbo.cases", "field_value_8", c => c.String());
            //migrationBuilder.AddColumn("dbo.cases", "field_value_9", c => c.String());
            //migrationBuilder.AddColumn("dbo.cases", "field_value_10", c => c.String());
            //migrationBuilder.AddColumn("dbo.check_lists", "field_1", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_lists", "field_2", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_lists", "field_3", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_lists", "field_4", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_lists", "field_5", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_lists", "field_6", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_lists", "field_7", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_lists", "field_8", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_lists", "field_9", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_lists", "field_10", c => c.Int());
            //migrationBuilder.AddColumn("dbo.case_versions", "field_value_1", c => c.String());
            //migrationBuilder.AddColumn("dbo.case_versions", "field_value_2", c => c.String());
            //migrationBuilder.AddColumn("dbo.case_versions", "field_value_3", c => c.String());
            //migrationBuilder.AddColumn("dbo.case_versions", "field_value_4", c => c.String());
            //migrationBuilder.AddColumn("dbo.case_versions", "field_value_5", c => c.String());
            //migrationBuilder.AddColumn("dbo.case_versions", "field_value_6", c => c.String());
            //migrationBuilder.AddColumn("dbo.case_versions", "field_value_7", c => c.String());
            //migrationBuilder.AddColumn("dbo.case_versions", "field_value_8", c => c.String());
            //migrationBuilder.AddColumn("dbo.case_versions", "field_value_9", c => c.String());
            //migrationBuilder.AddColumn("dbo.case_versions", "field_value_10", c => c.String());
            //migrationBuilder.AddColumn("dbo.check_list_versions", "field_1", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_list_versions", "field_2", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_list_versions", "field_3", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_list_versions", "field_4", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_list_versions", "field_5", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_list_versions", "field_6", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_list_versions", "field_7", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_list_versions", "field_8", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_list_versions", "field_9", c => c.Int());
            //migrationBuilder.AddColumn("dbo.check_list_versions", "field_10", c => c.Int());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("dbo.check_list_versions", "field_10");
            migrationBuilder.DropColumn("dbo.check_list_versions", "field_9");
            migrationBuilder.DropColumn("dbo.check_list_versions", "field_8");
            migrationBuilder.DropColumn("dbo.check_list_versions", "field_7");
            migrationBuilder.DropColumn("dbo.check_list_versions", "field_6");
            migrationBuilder.DropColumn("dbo.check_list_versions", "field_5");
            migrationBuilder.DropColumn("dbo.check_list_versions", "field_4");
            migrationBuilder.DropColumn("dbo.check_list_versions", "field_3");
            migrationBuilder.DropColumn("dbo.check_list_versions", "field_2");
            migrationBuilder.DropColumn("dbo.check_list_versions", "field_1");
            migrationBuilder.DropColumn("dbo.case_versions", "field_value_10");
            migrationBuilder.DropColumn("dbo.case_versions", "field_value_9");
            migrationBuilder.DropColumn("dbo.case_versions", "field_value_8");
            migrationBuilder.DropColumn("dbo.case_versions", "field_value_7");
            migrationBuilder.DropColumn("dbo.case_versions", "field_value_6");
            migrationBuilder.DropColumn("dbo.case_versions", "field_value_5");
            migrationBuilder.DropColumn("dbo.case_versions", "field_value_4");
            migrationBuilder.DropColumn("dbo.case_versions", "field_value_3");
            migrationBuilder.DropColumn("dbo.case_versions", "field_value_2");
            migrationBuilder.DropColumn("dbo.case_versions", "field_value_1");
            migrationBuilder.DropColumn("dbo.check_lists", "field_10");
            migrationBuilder.DropColumn("dbo.check_lists", "field_9");
            migrationBuilder.DropColumn("dbo.check_lists", "field_8");
            migrationBuilder.DropColumn("dbo.check_lists", "field_7");
            migrationBuilder.DropColumn("dbo.check_lists", "field_6");
            migrationBuilder.DropColumn("dbo.check_lists", "field_5");
            migrationBuilder.DropColumn("dbo.check_lists", "field_4");
            migrationBuilder.DropColumn("dbo.check_lists", "field_3");
            migrationBuilder.DropColumn("dbo.check_lists", "field_2");
            migrationBuilder.DropColumn("dbo.check_lists", "field_1");
            migrationBuilder.DropColumn("dbo.cases", "field_value_10");
            migrationBuilder.DropColumn("dbo.cases", "field_value_9");
            migrationBuilder.DropColumn("dbo.cases", "field_value_8");
            migrationBuilder.DropColumn("dbo.cases", "field_value_7");
            migrationBuilder.DropColumn("dbo.cases", "field_value_6");
            migrationBuilder.DropColumn("dbo.cases", "field_value_5");
            migrationBuilder.DropColumn("dbo.cases", "field_value_4");
            migrationBuilder.DropColumn("dbo.cases", "field_value_3");
            migrationBuilder.DropColumn("dbo.cases", "field_value_2");
            migrationBuilder.DropColumn("dbo.cases", "field_value_1");
        }
    }
}
