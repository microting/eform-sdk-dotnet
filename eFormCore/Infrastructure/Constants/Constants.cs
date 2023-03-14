/*
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

namespace Microting.eForm.Infrastructure.Constants
{
    public static class Constants
    {
        public static class Notifications
        {
            public const string RetrievedForm = "unit_fetch";
            public const string Completed = "check_status";
            public const string UnitActivate = "unit_activate";
            public const string EformParsedByServer = "eform_parsed_by_server";
            public const string EformParsingError = "eform_parsing_error";
            public const string SpeechToTextCompleted = "speech_to_text_completed";
            public const string InSightAnswerDone = "insight_answer_done";
            public const string InSightSurveyConfigurationCreated = "insight_survey_configuration_created";
            public const string InSightSurveyConfigurationChanged = "insight_survey_configuration_changed";
        }

        public static class WorkflowStates
        {
            public const string Active = "active";
            public const string Created = "created";
            public const string Processed = "processed";
            public const string PreCreated = "pre_created";
            public const string NotFound = "not_found";
            public const string Removed = "removed";
            public const string Retracted = "retracted";
            public const string FailedToSync = "failed_to_sync";
            public const string NotRemoved = "not_removed";
            public const string NotRetracted = "not_retracted";
        }

        public static class UploaderTypes
        {
            public const string System = "system";
        }

        public static class eFormSortParameters
        {
            public const string Label = "label";
            public const string Description = "description";
            public const string CreatedAt = "created_at";
            public const string Tags = "tags";
        }

        public static class CaseSortParameters
        {
            public const string CreatedAt = "created_at";
            public const string DoneAt = "done_at";
            public const string WorkerName = "worker_name";
            public const string SiteName = "site_name";
            public const string UnitId = "unit_id";
            public const string Status = "status";
            public const string Field1 = "field1";
            public const string Field2 = "field2";
            public const string Field3 = "field3";
            public const string Field4 = "field4";
            public const string Field5 = "field5";
            public const string Field6 = "field6";
            public const string Field7 = "field7";
            public const string Field8 = "field8";
            public const string Field9 = "field9";
            public const string Field10 = "field10";
            public const string FieldValue1 = "field_value_1";
            public const string FieldValue2 = "field_value_2";
            public const string FieldValue3 = "field_value_3";
            public const string FieldValue4 = "field_value_4";
            public const string FieldValue5 = "field_value_5";
            public const string FieldValue6 = "field_value_6";
            public const string FieldValue7 = "field_value_7";
            public const string FieldValue8 = "field_value_8";
            public const string FieldValue9 = "field_value_9";
            public const string FieldValue10 = "field_value_10";
        }

        public static class EntityItemSortParameters
        {
            public const string Id = "Id";
            public const string Name = "Name";
            public const string DisplayIndex = "DisplayIndex";
        }

        public static class FieldTypes
        {
            public const string Audio = "Audio";
            public const string Movie = "Movie";
            public const string CheckBox = "CheckBox";
            public const string Comment = "Comment";
            public const string Date = "Date";
            public const string None = "None";
            public const string Number = "Number";
            public const string MultiSelect = "MultiSelect";
            public const string Picture = "Picture";
            public const string SaveButton = "SaveButton";
            public const string ShowPdf = "ShowPdf";
            public const string Signature = "Signature";
            public const string SingleSelect = "SingleSelect";
            public const string Text = "Text";
            public const string Timer = "Timer";
            public const string EntitySearch = "EntitySearch";
            public const string EntitySelect = "EntitySelect";
            public const string FieldGroup = "FieldGroup";
            public const string NumberStepper = "NumberStepper";
        }

        public static class FieldColors
        {
            public const string Blue = "e2f4fb";
            public const string Purple = "f5eafa";
            public const string Green = "f0f8db";
            public const string Yellow = "fff6df";
            public const string Red = "ffe4e4";
            public const string Grey = "e7e7e7";
            public const string Default = "e8eaf6";
            public const string None = "e8eaf6";
        }

        public static class CheckListColors
        {
            public const string Grey = "grey";
            public const string Red = "red";
            public const string Green = "green";
        }

        public static class BarcodeTypes
        {
            public const string AndroidOnlyCodabar = "CODABAR"; // Android only
            public const string AndroidOnlyCode93 = "CODE_93"; // Android only
            public const string AndroidOnlyPdf417 = "PDF417"; // Android only
            public const string AndroidOnlyRss14 = "RSS14"; // Android only
            public const string AndroidOnlyRssExpandend = "RSS_EXPANDED"; // Android only
            public const string Code128 = "CODE_128";
            public const string Code39 = "CODE_39";
            public const string DataMatrix = "DATA_MATRIX";
            public const string Ean13 = "EAN_13";
            public const string Ean8 = "EAN_8";
            public const string Itf = "ITF";
            public const string QrCode = "QR_CODE";
            public const string UpcA = "UPC_A";
            public const string UpcB = "UPC_E";
        }

        public static class CheckListValues
        {
            public const string Checked = "Checked";
            public const string NotChecked = "Not Checked";
            public const string NotApproved = "Not Approved";
        }

        public static class QuestionTypes
        {
            public const string Smiley = "smiley";
            public const string Smiley2 = "smiley2";
            public const string Smiley3 = "smiley3";
            public const string Smiley4 = "smiley4";
            public const string Smiley5 = "smiley5";
            public const string Smiley6 = "smiley6";
            public const string Smiley7 = "smiley7";
            public const string Smiley8 = "smiley8";
            public const string Smiley9 = "smiley9";
            public const string Smiley10 = "smiley10";

            public const string Buttons = "buttons";
            public const string List = "list";
            public const string Multi = "multi";
            public const string Text = "text";
            public const string Number = "number";
            public const string InfoText = "info_text";
            public const string Picture = "picture";
            public const string ZipCode = "zipcode";
            public const string TextEamil = "text_email";
        }
    }
}