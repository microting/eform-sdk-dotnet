using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingMissingAIonLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            //Setup for SQL Server Provider
           
            string autoIDGenStrategy = "SqlServer:ValueGenerationStrategy";
            object autoIDGenStrategyValue= SqlServerValueGenerationStrategy.IdentityColumn;

            // Setup for MySQL Provider
            if (migrationBuilder.ActiveProvider=="Pomelo.EntityFrameworkCore.MySql")
            {
                DbConfig.IsMySQL = true;
                autoIDGenStrategy = "MySql:ValueGenerationStrategy";
                autoIDGenStrategyValue = MySqlValueGenerationStrategy.IdentityColumn;
            }

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "logs").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                "log_exceptions").Annotation(autoIDGenStrategy, autoIDGenStrategyValue);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
