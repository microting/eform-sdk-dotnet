namespace eFormShared
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
        }

        public static class WorkflowStates
        {
            public const string Created = "created";
            public const string Processed = "processed";
            public const string PreCreated = "pre_created";
            public const string NotFound = "not_found";
            public const string Removed = "removed";
            public const string Retracted = "retracted";
            public const string FailedToSync = "failed_to_sync";

        }

        public static class UploaderTypes
        {
            public const string System = "system";
        }

        public static class TamplateSortParameters
        {
            public const string Label = "label";
            public const string Description = "description";
            public const string CreatedAt = "created_at";
        }

        public static class CaseSortParameters
        {
            public const string CreatedAt = "created_at";
            public const string DoneAt = "done_at";
            public const string WorkerName = "worker_name";
            public const string SiteName = "site_name";
            public const string UnitId = "unit_id";
            public const string Status = "status";
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

        public static class FieldColors
        {
            public const string Blue = "e2f4fb";
            public const string Purple = "e2f4fb";
            public const string Green = "f0f8db";
            public const string Yellow = "fff6df";
            public const string Red = "ffe4e4";
            public const string Default = "None";
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
    }
}
