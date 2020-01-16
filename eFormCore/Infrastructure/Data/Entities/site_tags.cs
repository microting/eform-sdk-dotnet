using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class site_tags : BaseEntity
    {
        [ForeignKey("tag")]
        public int? TagId { get; set; }
        
        [ForeignKey("site")]
        public int? SiteId { get; set; }
        
        public virtual sites Site { get; set; }
        
        public virtual tags Tag { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.SiteTags.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.SiteTagVersions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            site_tags siteTags = await dbContext.SiteTags.FirstOrDefaultAsync(x => x.Id == Id);

            if (siteTags == null)
            {
                throw new NullReferenceException($"Could not find SiteTag withe Id {Id}");
            }

            siteTags.SiteId = SiteId;
            siteTags.TagId = TagId;

            if (dbContext.ChangeTracker.HasChanges())
            {
                siteTags.Version += 1;
                siteTags.UpdatedAt = DateTime.Now;

                dbContext.SiteTagVersions.Add(MapVersions(siteTags));
                await dbContext.SaveChangesAsync();
            }
            
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            site_tags siteTags = await dbContext.SiteTags.FirstOrDefaultAsync(x => x.Id == Id);

            if (siteTags == null)
            {
                throw new NullReferenceException($"Could not find SiteTag withe Id {Id}");
            }

            siteTags.WorkflowState = Constants.Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                siteTags.Version += 1;
                siteTags.UpdatedAt = DateTime.Now;

                dbContext.SiteTagVersions.Add(MapVersions(siteTags));
                await dbContext.SaveChangesAsync();
            }
        }

        private site_tag_versions MapVersions(site_tags siteTags)
        {
            site_tag_versions siteTagVersions = new site_tag_versions
            {
                SiteId = siteTags.SiteId,
                TagId = siteTags.TagId,
                WorkflowState = siteTags.WorkflowState,
                Version = siteTags.Version,
                CreatedAt = siteTags.CreatedAt,
                UpdatedAt = siteTags.UpdatedAt,
                SiteTagId = siteTags.Id
            };

            return siteTagVersions;
        }
    }
}