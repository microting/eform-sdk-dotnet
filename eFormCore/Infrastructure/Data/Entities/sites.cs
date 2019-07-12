/*
The MIT License (MIT)

Copyright (c) 2007 - 2019 microting

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
using eFormShared;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class sites : BaseEntity
    {
        public sites()
        {
            this.Cases = new HashSet<cases>();
            this.Units = new HashSet<units>();
            this.SiteWorkers = new HashSet<site_workers>();
            this.CheckListSites = new HashSet<check_list_sites>();
        }

//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }
//
//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int? MicrotingUid { get; set; }

//        public int? version { get; set; }
//
//        [StringLength(255)]
//        public string workflow_state { get; set; }

        public virtual ICollection<cases> Cases { get; set; }

        public virtual ICollection<units> Units { get; set; }

        public virtual ICollection<site_workers> SiteWorkers { get; set; }

        public virtual ICollection<check_list_sites> CheckListSites { get; set; }


        public void Create(MicrotingDbAnySql dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.sites.Add(this);
            dbContext.SaveChanges();

            dbContext.site_versions.Add(MapSiteVersions(this));
            dbContext.SaveChanges();
            
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            sites site = dbContext.sites.FirstOrDefault(x => x.Id == Id);

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
                dbContext.SaveChanges();

            }
           
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            sites site = dbContext.sites.FirstOrDefault(x => x.Id == Id);

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
                dbContext.SaveChanges();

            }
        }
        
        
        private site_versions MapSiteVersions(sites site)
        {
            site_versions siteVer = new site_versions();
            siteVer.WorkflowState = site.WorkflowState;
            siteVer.Version = site.Version;
            siteVer.CreatedAt = site.CreatedAt;
            siteVer.UpdatedAt = site.UpdatedAt;
            siteVer.MicrotingUid = site.MicrotingUid;
            siteVer.Name = site.Name;

            siteVer.SiteId = site.Id; //<<--

            return siteVer;
        }
    }
}
