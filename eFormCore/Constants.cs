namespace eFormShared
{
    public static class Constants
    {
        public static class Notifications
        {
            public const string RetrievedForm = "unit_fetch";
            public const string Completed = "check_status";
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
    }
}
