namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTranscriptionIdToUploadedData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.uploaded_data", "transcription_id", c => c.Int());
            AddColumn("dbo.uploaded_data_versions", "transcription_id", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.uploaded_data_versions", "transcription_id");
            DropColumn("dbo.uploaded_data", "transcription_id");
        }
    }
}
