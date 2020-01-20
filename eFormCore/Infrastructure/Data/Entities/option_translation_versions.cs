using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class option_translation_versions : BaseEntity
    {
        public int OptionId { get; set; }
        
        public int LanguageId { get; set; }
        
        public string Name { get; set; }
        
        [ForeignKey("option_translation")]
        public int OptionTranslationId { get; set; }
    }
}