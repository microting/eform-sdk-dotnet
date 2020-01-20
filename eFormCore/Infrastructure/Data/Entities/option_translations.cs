using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class option_translations : BaseEntity
    {
        [ForeignKey("option")]
        public int OptionId { get; set; }
        
        [ForeignKey("language")]
        public int LanguageId { get; set; }
        
        public string Name { get; set; }

        public virtual options option { get; set; }
        
        public virtual languages Language { get; set; }
    }
}