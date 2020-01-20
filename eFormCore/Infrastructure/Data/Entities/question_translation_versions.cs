using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class question_translation_versions : BaseEntity
    {
        public int QuestionId { get; set; }
        
        public int LanguageId { get; set; }
        
        public string Name { get; set; }
        
        [ForeignKey("question_translation")]
        public int QuestionTranslationId { get; set; }

        public virtual question_translations QuestionTranslation { get; set; }
    }
}