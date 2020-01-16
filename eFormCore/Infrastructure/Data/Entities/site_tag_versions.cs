namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class site_tag_versions : BaseEntity
    {
        public int? TagId { get; set; }
        
        public int? SiteId { get; set; }
        
        public int SiteTagId { get; set; }
    }
}