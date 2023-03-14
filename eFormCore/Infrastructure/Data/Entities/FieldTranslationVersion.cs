using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class FieldTranslationVersion : BaseEntity
    {
        public int FieldId { get; set; }

        public int LanguageId { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }

        [ForeignKey("FieldTranslation")] public int FieldTranslationId { get; set; }

        public string DefaultValue { get; set; }

        public int UploadedDataId { get; set; }
    }
}