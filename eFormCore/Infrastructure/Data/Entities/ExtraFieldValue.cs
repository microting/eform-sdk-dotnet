using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class ExtraFieldValue : PnBase
    {
        public DateTime? DoneAt { get; set; }

        public DateTime? Date { get; set; }

        [ForeignKey("Worker")] public int? WorkerId { get; set; }

        public int? CaseId { get; set; }

        [ForeignKey("CheckList")] public int? CheckListId { get; set; }

        public int? CheckListDuplicateId { get; set; }

        public int? CheckListValueId { get; set; }

        [ForeignKey("UploadedData")] public int? UploadedDataId { get; set; }

        public string Value { get; set; }

        [StringLength(255)] public string Latitude { get; set; }

        [StringLength(255)] public string Longitude { get; set; }

        [StringLength(255)] public string Altitude { get; set; }

        [StringLength(255)] public string Heading { get; set; }

        [StringLength(255)] public string Accuracy { get; set; }

        public string FieldType { get; set; }

        public int FieldTypeId { get; set; }
    }
}