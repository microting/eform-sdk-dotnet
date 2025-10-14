namespace Microting.eForm.Infrastructure.Data.Entities;

public class FolderTranslationVersion : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int MicrotingUid { get; set; }
    public int LanguageId { get; set; }
    public int FolderId { get; set; }
    public int FolderTranslationId { get; set; }
}