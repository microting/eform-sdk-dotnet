using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class language_question_set_versions : BaseEntity
    {
        public int LanguageId { get; set; }
        
        public int QuestionSetId { get; set; }
        
        [ForeignKey("language_question_set")]
        public int LanguageQuestionSetId { get; set; }
        
        public virtual language_question_sets LanguageQuestionSet { get; set; }
        
        public int? MicrotingUid { get; set; }
    }
}