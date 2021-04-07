namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class FolderTranslation : PnBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MicrotingUid { get; set; }
        public int LanguageId { get; set; }
        public int FolderId { get; set; }
    }
}