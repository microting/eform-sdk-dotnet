using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class AddingMicrotingUidToInSight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "QuestionTranslationVersions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "QuestionTranslations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "OptionTranslationVersions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "OptionTranslations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "LanguageQuestionSetVersions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicrotingUid",
                table: "LanguageQuestionSets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "QuestionTranslationVersions");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "QuestionTranslations");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "OptionTranslationVersions");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "OptionTranslations");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "LanguageQuestionSetVersions");

            migrationBuilder.DropColumn(
                name: "MicrotingUid",
                table: "LanguageQuestionSets");
        }
    }
}
