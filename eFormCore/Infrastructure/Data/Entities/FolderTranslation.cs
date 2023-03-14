using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class FolderTranslation : PnBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MicrotingUid { get; set; }

        [ForeignKey("Language")] public int LanguageId { get; set; }

        [ForeignKey("Folder")] public int FolderId { get; set; }

        public virtual Folder Folder { get; set; }

        public virtual Language Language { get; set; }
    }
}