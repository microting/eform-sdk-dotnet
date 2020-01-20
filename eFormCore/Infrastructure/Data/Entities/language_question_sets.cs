using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class language_question_sets : BaseEntity
    {
        [ForeignKey("language")]
        public int LanguageId { get; set; }
        
        [ForeignKey("question_set")]
        public int QuestionSetId { get; set; }
        
        public virtual question_sets QuestionSet { get; set; }
        
        public virtual languages Language { get; set; }
    }
}