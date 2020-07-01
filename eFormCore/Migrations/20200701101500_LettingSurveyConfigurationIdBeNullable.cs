using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class LettingSurveyConfigurationIdBeNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_survey_configurations_SurveyConfigurationId",
                table: "answers");

            migrationBuilder.AlterColumn<int>(
                name: "SurveyConfigurationId",
                table: "answers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "SurveyConfigurationId",
                table: "answer_versions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_answers_survey_configurations_SurveyConfigurationId",
                table: "answers",
                column: "SurveyConfigurationId",
                principalTable: "survey_configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_survey_configurations_SurveyConfigurationId",
                table: "answers");

            migrationBuilder.AlterColumn<int>(
                name: "SurveyConfigurationId",
                table: "answers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SurveyConfigurationId",
                table: "answer_versions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_survey_configurations_SurveyConfigurationId",
                table: "answers",
                column: "SurveyConfigurationId",
                principalTable: "survey_configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
