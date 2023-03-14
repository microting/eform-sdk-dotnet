using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class FieldOptionTranslationVersion : BaseEntity
    {
        public int FieldOptionId { get; set; }

        public int LanguageId { get; set; }

        public string Text { get; set; }

        [ForeignKey("FieldOptionTranslation")] public int FieldOptionTranslationId { get; set; }
    }
}