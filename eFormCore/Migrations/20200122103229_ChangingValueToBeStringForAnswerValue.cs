using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class ChangingValueToBeStringForAnswerValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answer_values_options_OptionsId",
                table: "answer_values");

            migrationBuilder.RenameColumn(
                name: "OptionsId",
                table: "answer_values",
                newName: "OptionId");

            migrationBuilder.RenameIndex(
                name: "IX_answer_values_OptionsId",
                table: "answer_values",
                newName: "IX_answer_values_OptionId");

            migrationBuilder.RenameColumn(
                name: "OptionsId",
                table: "answer_value_versions",
                newName: "OptionId");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "answer_values",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "answer_value_versions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_survey_configurations_QuestionSetId",
                table: "survey_configurations",
                column: "QuestionSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_answer_values_options_OptionId",
                table: "answer_values",
                column: "OptionId",
                principalTable: "options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_survey_configurations_question_sets_QuestionSetId",
                table: "survey_configurations",
                column: "QuestionSetId",
                principalTable: "question_sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answer_values_options_OptionId",
                table: "answer_values");

            migrationBuilder.DropForeignKey(
                name: "FK_survey_configurations_question_sets_QuestionSetId",
                table: "survey_configurations");

            migrationBuilder.DropIndex(
                name: "IX_survey_configurations_QuestionSetId",
                table: "survey_configurations");

            migrationBuilder.RenameColumn(
                name: "OptionId",
                table: "answer_values",
                newName: "OptionsId");

            migrationBuilder.RenameIndex(
                name: "IX_answer_values_OptionId",
                table: "answer_values",
                newName: "IX_answer_values_OptionsId");

            migrationBuilder.RenameColumn(
                name: "OptionId",
                table: "answer_value_versions",
                newName: "OptionsId");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "answer_values",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "answer_value_versions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_answer_values_options_OptionsId",
                table: "answer_values",
                column: "OptionsId",
                principalTable: "options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
