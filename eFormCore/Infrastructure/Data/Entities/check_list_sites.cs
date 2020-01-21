/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 Microting A/S

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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class check_list_sites : BaseEntity
    {
        [ForeignKey("site")]
        public int? SiteId { get; set; }

        [ForeignKey("check_list")]
        public int? CheckListId { get; set; }

        public int MicrotingUid { get; set; }

        public int LastCheckId { get; set; }

        public virtual sites Site { get; set; }

        public virtual check_lists CheckList { get; set; }
        
        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.check_list_sites.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.check_list_site_versions.Add(MapCheckListSiteVersions(this));
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            check_list_sites checkListSites = await dbContext.check_list_sites.FirstOrDefaultAsync(x => x.Id == Id);

            if (checkListSites == null)
            {
                throw  new NullReferenceException($"Could not find Check List Site with Id: {Id}");
            }

            checkListSites.SiteId = SiteId;
            checkListSites.CheckListId = CheckListId;
            checkListSites.MicrotingUid = MicrotingUid;
            checkListSites.LastCheckId = LastCheckId;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                checkListSites.Version += 1;
                checkListSites.UpdatedAt = DateTime.Now;

                dbContext.check_list_site_versions.Add(MapCheckListSiteVersions(checkListSites));
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            check_list_sites checkListSites = await dbContext.check_list_sites.FirstOrDefaultAsync(x => x.Id == Id);

            if (checkListSites == null)
            {
                throw  new NullReferenceException($"Could not find Check List Site with Id: {Id}");
            }

            checkListSites.WorkflowState = Constants.Constants.WorkflowStates.Removed;
                
            if (dbContext.ChangeTracker.HasChanges())
            {
                checkListSites.Version += 1;
                checkListSites.UpdatedAt = DateTime.Now;

                dbContext.check_list_site_versions.Add(MapCheckListSiteVersions(checkListSites));
                await dbContext.SaveChangesAsync();
            }
        }
        
        private check_list_site_versions MapCheckListSiteVersions(check_list_sites checkListSite)
        {
            return new check_list_site_versions
            {
                CheckListId = checkListSite.CheckListId,
                CreatedAt = checkListSite.CreatedAt,
                UpdatedAt = checkListSite.UpdatedAt,
                LastCheckId = checkListSite.LastCheckId,
                MicrotingUid = checkListSite.MicrotingUid,
                SiteId = checkListSite.SiteId,
                Version = checkListSite.Version,
                WorkflowState = checkListSite.WorkflowState,
                CheckListSiteId = checkListSite.Id
            };
        }
    }
}