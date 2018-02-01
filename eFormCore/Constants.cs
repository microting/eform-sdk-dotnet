namespace eFormShared
{
    public static class Constants
    {
        public static class Notifications
        {
            public const string RetrievedForm = "unit_fetch";
            public const string Completed = "check_status";
            public const string UnitActivate = "unit_activate";
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
            // TODO Add types
        }
    }
}
