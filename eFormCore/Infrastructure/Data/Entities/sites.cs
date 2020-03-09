/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class sites : BaseEntity
    {
        public sites()
        {
            Cases = new HashSet<cases>();
            Units = new HashSet<units>();
            SiteWorkers = new HashSet<site_workers>();
            CheckListSites = new HashSet<check_list_sites>();
            SiteTags = new List<site_tags>();
        }

        [StringLength(255)]
        public string Name { get; set; }

        public int? MicrotingUid { get; set; }

        public virtual ICollection<cases> Cases { get; set; }

        public virtual ICollection<units> Units { get; set; }

        public virtual ICollection<site_workers> SiteWorkers { get; set; }

        public virtual ICollection<check_list_sites> CheckListSites { get; set; }

        public virtual ICollection<site_tags> SiteTags { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.sites.Add(this);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            dbContext.site_versions.Add(MapSiteVersions(this));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            sites site = await dbContext.sites.FirstOrDefaultAsync(x => x.Id == Id);

            if (site == null)
            {
                throw new NullReferenceException($"Could not find Site with Id: {Id}");
            }

            site.Name = Name;
            site.MicrotingUid = MicrotingUid;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                site.Version += 1;
                site.UpdatedAt = DateTime.Now;
                
                dbContext.site_versions.Add(MapSiteVersions(site));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            sites site = await dbContext.sites.FirstOrDefaultAsync(x => x.Id == Id);

            if (site == null)
            {
                throw new NullReferenceException($"Could not find Site with Id: {Id}");
            }

            site.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                site.Version += 1;
                site.UpdatedAt = DateTime.Now;
                
                dbContext.site_versions.Add(MapSiteVersions(site));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

            }
        }

        private site_versions MapSiteVersions(sites site)
        {
            return new site_versions
            {
                WorkflowState = site.WorkflowState,
                Version = site.Version,
                CreatedAt = site.CreatedAt,
                UpdatedAt = site.UpdatedAt,
                MicrotingUid = site.MicrotingUid,
                Name = site.Name,
                SiteId = site.Id
            };
        }
    }
}
