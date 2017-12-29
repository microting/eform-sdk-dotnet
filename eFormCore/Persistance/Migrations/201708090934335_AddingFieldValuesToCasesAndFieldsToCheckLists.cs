namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFieldValuesToCasesAndFieldsToCheckLists : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.cases", "field_value_1", c => c.String());
            AddColumn("dbo.cases", "field_value_2", c => c.String());
            AddColumn("dbo.cases", "field_value_3", c => c.String());
            AddColumn("dbo.cases", "field_value_4", c => c.String());
            AddColumn("dbo.cases", "field_value_5", c => c.String());
            AddColumn("dbo.cases", "field_value_6", c => c.String());
            AddColumn("dbo.cases", "field_value_7", c => c.String());
            AddColumn("dbo.cases", "field_value_8", c => c.String());
            AddColumn("dbo.cases", "field_value_9", c => c.String());
            AddColumn("dbo.cases", "field_value_10", c => c.String());
            AddColumn("dbo.check_lists", "field_1", c => c.Int());
            AddColumn("dbo.check_lists", "field_2", c => c.Int());
            AddColumn("dbo.check_lists", "field_3", c => c.Int());
            AddColumn("dbo.check_lists", "field_4", c => c.Int());
            AddColumn("dbo.check_lists", "field_5", c => c.Int());
            AddColumn("dbo.check_lists", "field_6", c => c.Int());
            AddColumn("dbo.check_lists", "field_7", c => c.Int());
            AddColumn("dbo.check_lists", "field_8", c => c.Int());
            AddColumn("dbo.check_lists", "field_9", c => c.Int());
            AddColumn("dbo.check_lists", "field_10", c => c.Int());
            AddColumn("dbo.case_versions", "field_value_1", c => c.String());
            AddColumn("dbo.case_versions", "field_value_2", c => c.String());
            AddColumn("dbo.case_versions", "field_value_3", c => c.String());
            AddColumn("dbo.case_versions", "field_value_4", c => c.String());
            AddColumn("dbo.case_versions", "field_value_5", c => c.String());
            AddColumn("dbo.case_versions", "field_value_6", c => c.String());
            AddColumn("dbo.case_versions", "field_value_7", c => c.String());
            AddColumn("dbo.case_versions", "field_value_8", c => c.String());
            AddColumn("dbo.case_versions", "field_value_9", c => c.String());
            AddColumn("dbo.case_versions", "field_value_10", c => c.String());
            AddColumn("dbo.check_list_versions", "field_1", c => c.Int());
            AddColumn("dbo.check_list_versions", "field_2", c => c.Int());
            AddColumn("dbo.check_list_versions", "field_3", c => c.Int());
            AddColumn("dbo.check_list_versions", "field_4", c => c.Int());
            AddColumn("dbo.check_list_versions", "field_5", c => c.Int());
            AddColumn("dbo.check_list_versions", "field_6", c => c.Int());
            AddColumn("dbo.check_list_versions", "field_7", c => c.Int());
            AddColumn("dbo.check_list_versions", "field_8", c => c.Int());
            AddColumn("dbo.check_list_versions", "field_9", c => c.Int());
            AddColumn("dbo.check_list_versions", "field_10", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.check_list_versions", "field_10");
            DropColumn("dbo.check_list_versions", "field_9");
            DropColumn("dbo.check_list_versions", "field_8");
            DropColumn("dbo.check_list_versions", "field_7");
            DropColumn("dbo.check_list_versions", "field_6");
            DropColumn("dbo.check_list_versions", "field_5");
            DropColumn("dbo.check_list_versions", "field_4");
            DropColumn("dbo.check_list_versions", "field_3");
            DropColumn("dbo.check_list_versions", "field_2");
            DropColumn("dbo.check_list_versions", "field_1");
            DropColumn("dbo.case_versions", "field_value_10");
            DropColumn("dbo.case_versions", "field_value_9");
            DropColumn("dbo.case_versions", "field_value_8");
            DropColumn("dbo.case_versions", "field_value_7");
            DropColumn("dbo.case_versions", "field_value_6");
            DropColumn("dbo.case_versions", "field_value_5");
            DropColumn("dbo.case_versions", "field_value_4");
            DropColumn("dbo.case_versions", "field_value_3");
            DropColumn("dbo.case_versions", "field_value_2");
            DropColumn("dbo.case_versions", "field_value_1");
            DropColumn("dbo.check_lists", "field_10");
            DropColumn("dbo.check_lists", "field_9");
            DropColumn("dbo.check_lists", "field_8");
            DropColumn("dbo.check_lists", "field_7");
            DropColumn("dbo.check_lists", "field_6");
            DropColumn("dbo.check_lists", "field_5");
            DropColumn("dbo.check_lists", "field_4");
            DropColumn("dbo.check_lists", "field_3");
            DropColumn("dbo.check_lists", "field_2");
            DropColumn("dbo.check_lists", "field_1");
            DropColumn("dbo.cases", "field_value_10");
            DropColumn("dbo.cases", "field_value_9");
            DropColumn("dbo.cases", "field_value_8");
            DropColumn("dbo.cases", "field_value_7");
            DropColumn("dbo.cases", "field_value_6");
            DropColumn("dbo.cases", "field_value_5");
            DropColumn("dbo.cases", "field_value_4");
            DropColumn("dbo.cases", "field_value_3");
            DropColumn("dbo.cases", "field_value_2");
            DropColumn("dbo.cases", "field_value_1");
        }
    }
}
