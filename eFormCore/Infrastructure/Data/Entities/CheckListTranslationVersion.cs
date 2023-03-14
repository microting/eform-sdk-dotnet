using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class CheckListTranslationVersion : BaseEntity
    {
        public int CheckListId { get; set; }

        public int LanguageId { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }

        [ForeignKey("CheckListTranslation")] public int CheckListTranslationId { get; set; }
    }
}