using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class FieldTranslation : PnBase
    {
        [ForeignKey("Field")] public int FieldId { get; set; }

        [ForeignKey("Language")] public int LanguageId { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }

        public virtual Field Field { get; set; }

        public string DefaultValue { get; set; }

        public int UploadedDataId { get; set; }
    }
}