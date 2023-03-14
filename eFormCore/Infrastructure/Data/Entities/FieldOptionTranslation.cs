using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class FieldOptionTranslation : PnBase
    {
        [ForeignKey("FieldOption")] public int FieldOptionId { get; set; }

        [ForeignKey("Language")] public int LanguageId { get; set; }

        public string Text { get; set; }

        public virtual FieldOption FieldOption { get; set; }
    }
}