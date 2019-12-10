using System;

namespace Microting.eForm.Dto
{
    public class Case
    {
        public int Id { get; set; }

        public string WorkflowState { get; set; }

        public int? Version { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DoneAt { get; set; }

        public int? SiteId { get; set; }

        public string SiteName { get; set; }

        public int? UnitId { get; set; }

        public string WorkerName { get; set; }

        public int? TemplatId { get; set; }

        public string CaseType { get; set; }

        public int? MicrotingUId { get; set; }

        public int? CheckUIid { get; set; }

        public string CaseUId { get; set; }

        public string Custom { get; set; }

        public string FieldValue1 { get; set; }

        public string FieldValue2 { get; set; }

        public string FieldValue3 { get; set; }

        public string FieldValue4 { get; set; }

        public string FieldValue5 { get; set; }

        public string FieldValue6 { get; set; }

        public string FieldValue7 { get; set; }

        public string FieldValue8 { get; set; }

        public string FieldValue9 { get; set; }

        public string FieldValue10 { get; set; }
    }
}