﻿/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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
