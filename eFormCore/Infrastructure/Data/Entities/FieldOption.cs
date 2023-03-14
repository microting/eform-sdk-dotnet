using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class FieldOption : PnBase
    {
        public FieldOption()
        {
            FieldOptionTranslations = new HashSet<FieldOptionTranslation>();
        }

        [ForeignKey("Field")] public int FieldId { get; set; }

        public string Key { get; set; }

        public bool Selected { get; set; }

        public string DisplayOrder { get; set; }

        public virtual Field Field { get; set; }

        public virtual ICollection<FieldOptionTranslation> FieldOptionTranslations { get; set; }
    }
}