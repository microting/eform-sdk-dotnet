namespace eFormSqlController.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RenamingDataUploaded : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.data_uploaded", newName: "uploaded_data");
        }

        public override void Down()
        {
            RenameTable(name: "dbo.uploaded_data", newName: "data_uploaded");
        }
    }
}
