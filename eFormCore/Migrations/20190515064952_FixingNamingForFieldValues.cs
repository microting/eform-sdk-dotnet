/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

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
    public partial class FixingNamingForFieldValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
//            migrationBuilder.DropForeignKey(
//                name: "FK_field_values_check_lists_check_list_id",
//                table: "field_values");

//            migrationBuilder.DropForeignKey(
//                name: "FK_field_values_fields_field_id",
//                table: "field_values");

//            migrationBuilder.DropForeignKey(
//                name: "FK_field_values_uploaded_data_uploaded_data_id",
//                table: "field_values");

//            migrationBuilder.DropForeignKey(
//                name: "FK_field_values_workers_user_id",
//                table: "field_values");

//            migrationBuilder.DropIndex(
//                name: "IX_field_values_user_id",
//                table: "field_values");

//            migrationBuilder.DropIndex(
//                name: "IX_field_values_uploaded_data_id",
//                table: "field_values");

//            migrationBuilder.DropIndex(
//                name: "IX_field_values_field_id",
//                table: "field_values");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "field_values",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "field_values",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "field_values",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "heading",
                table: "field_values",
                newName: "Heading");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "field_values",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "altitude",
                table: "field_values",
                newName: "Altitude");

            migrationBuilder.RenameColumn(
                name: "accuracy",
                table: "field_values",
                newName: "Accuracy");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "field_values",
                newName: "WorkerId");

            migrationBuilder.RenameColumn(
                name: "uploaded_data_id",
                table: "field_values",
                newName: "UploadedDataId");

            migrationBuilder.RenameColumn(
                name: "field_id",
                table: "field_values",
                newName: "FieldId");

            migrationBuilder.RenameColumn(
                name: "done_at",
                table: "field_values",
                newName: "DoneAt");

            migrationBuilder.RenameColumn(
                name: "check_list_id",
                table: "field_values",
                newName: "CheckListId");

            migrationBuilder.RenameColumn(
                name: "check_list_duplicate_id",
                table: "field_values",
                newName: "CheckListDuplicateId");

            migrationBuilder.RenameColumn(
                name: "case_id",
                table: "field_values",
                newName: "CaseId");

//            migrationBuilder.RenameIndex(
//                name: "IX_field_values_user_id",
//                table: "field_values",
//                newName: "IX_field_values_WorkerId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_field_values_uploaded_data_id",
//                table: "field_values",
//                newName: "IX_field_values_UploadedDataId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_field_values_field_id",
//                table: "field_values",
//                newName: "IX_field_values_FieldId");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_field_values_check_list_id",
//                table: "field_values",
//                newName: "IX_field_values_CheckListId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "field_value_versions",
                newName: "WorkerId");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "check_lists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "check_list_versions",
                nullable: true);

//            migrationBuilder.AddForeignKey(
//                name: "FK_field_values_check_lists_CheckListId",
//                table: "field_values",
//                column: "CheckListId",
//                principalTable: "check_lists",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_field_values_fields_FieldId",
//                table: "field_values",
//                column: "FieldId",
//                principalTable: "fields",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_field_values_uploaded_data_UploadedDataId",
//                table: "field_values",
//                column: "UploadedDataId",
//                principalTable: "uploaded_data",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_field_values_workers_WorkerId",
//                table: "field_values",
//                column: "WorkerId",
//                principalTable: "workers",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
//            migrationBuilder.DropForeignKey(
//                name: "FK_field_values_check_lists_CheckListId",
//                table: "field_values");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_field_values_fields_FieldId",
//                table: "field_values");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_field_values_uploaded_data_UploadedDataId",
//                table: "field_values");
//
//            migrationBuilder.DropForeignKey(
//                name: "FK_field_values_workers_WorkerId",
//                table: "field_values");
//
//            migrationBuilder.DropColumn(
//                name: "Color",
//                table: "check_lists");
//
//            migrationBuilder.DropColumn(
//                name: "Color",
//                table: "check_list_versions");
//
//            migrationBuilder.RenameColumn(
//                name: "Value",
//                table: "field_values",
//                newName: "value");
//
//            migrationBuilder.RenameColumn(
//                name: "Longitude",
//                table: "field_values",
//                newName: "longitude");
//
//            migrationBuilder.RenameColumn(
//                name: "Latitude",
//                table: "field_values",
//                newName: "latitude");
//
//            migrationBuilder.RenameColumn(
//                name: "Heading",
//                table: "field_values",
//                newName: "heading");
//
//            migrationBuilder.RenameColumn(
//                name: "Date",
//                table: "field_values",
//                newName: "date");
//
//            migrationBuilder.RenameColumn(
//                name: "Altitude",
//                table: "field_values",
//                newName: "altitude");
//
//            migrationBuilder.RenameColumn(
//                name: "Accuracy",
//                table: "field_values",
//                newName: "accuracy");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkerId",
//                table: "field_values",
//                newName: "user_id");
//
//            migrationBuilder.RenameColumn(
//                name: "UploadedDataId",
//                table: "field_values",
//                newName: "uploaded_data_id");
//
//            migrationBuilder.RenameColumn(
//                name: "FieldId",
//                table: "field_values",
//                newName: "field_id");
//
//            migrationBuilder.RenameColumn(
//                name: "DoneAt",
//                table: "field_values",
//                newName: "done_at");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListId",
//                table: "field_values",
//                newName: "check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CheckListDuplicateId",
//                table: "field_values",
//                newName: "check_list_duplicate_id");
//
//            migrationBuilder.RenameColumn(
//                name: "CaseId",
//                table: "field_values",
//                newName: "case_id");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_field_values_WorkerId",
//                table: "field_values",
//                newName: "IX_field_values_user_id");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_field_values_UploadedDataId",
//                table: "field_values",
//                newName: "IX_field_values_uploaded_data_id");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_field_values_FieldId",
//                table: "field_values",
//                newName: "IX_field_values_field_id");
//
//            migrationBuilder.RenameIndex(
//                name: "IX_field_values_CheckListId",
//                table: "field_values",
//                newName: "IX_field_values_check_list_id");
//
//            migrationBuilder.RenameColumn(
//                name: "WorkerId",
//                table: "case_versions",
//                newName: "DoneByUserId");
//
//            migrationBuilder.AddColumn<int>(
//                name: "DoneByUserId",
//                table: "cases",
//                nullable: true);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_field_values_check_lists_check_list_id",
//                table: "field_values",
//                column: "check_list_id",
//                principalTable: "check_lists",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_field_values_fields_field_id",
//                table: "field_values",
//                column: "field_id",
//                principalTable: "fields",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_field_values_uploaded_data_uploaded_data_id",
//                table: "field_values",
//                column: "uploaded_data_id",
//                principalTable: "uploaded_data",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
//
//            migrationBuilder.AddForeignKey(
//                name: "FK_field_values_workers_user_id",
//                table: "field_values",
//                column: "user_id",
//                principalTable: "workers",
//                principalColumn: "Id",
//                onDelete: ReferentialAction.Restrict);
        }
    }
}