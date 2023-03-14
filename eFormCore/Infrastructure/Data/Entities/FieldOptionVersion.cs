using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class FieldOptionVersion : BaseEntity
    {
        public int FieldId { get; set; }

        public string Key { get; set; }

        public bool Selected { get; set; }

        public string DisplayOrder { get; set; }

        [ForeignKey("FieldOption")] public int FieldOptionId { get; set; }
    }
}