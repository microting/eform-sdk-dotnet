using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class question_translations : BaseEntity
    {
        [ForeignKey("question")]
        public int QuestionId { get; set; }
        
        [ForeignKey("language")]
        public int LanguageId { get; set; }
        
        public string Name { get; set; }
        
        public virtual questions Question { get; set; }
        
        public virtual languages Language { get; set; }
    }
}