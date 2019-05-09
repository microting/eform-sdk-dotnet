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

using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace eFormSqlController
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using eFormShared;

    public partial class check_list_sites : BaseEntity
    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }
//
//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public int? version { get; set; }
//
//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        [ForeignKey("site")]
        public int? site_id { get; set; }

        [ForeignKey("check_list")]
        public int? check_list_id { get; set; }

        [StringLength(255)]
        public string microting_uid { get; set; }

        [StringLength(255)]
        public string last_check_id { get; set; }

        public virtual sites site { get; set; }

        public virtual check_lists check_list { get; set; }


        public void Create(MicrotingDbAnySql dbContext)
        {
            workflow_state = Constants.WorkflowStates.Created;
            version = 1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            dbContext.check_list_sites.Add(this);
            dbContext.SaveChanges();

            dbContext.check_list_site_versions.Add(MapCheckListSiteVersions(this));

        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            check_list_sites checkListSites = dbContext.check_list_sites.FirstOrDefault(x => x.Id == Id);

            if (checkListSites == null)
            {
                throw  new NullReferenceException($"Could not find Check List Site with Id: {Id}");
            }

            checkListSites.site_id = site_id;
            checkListSites.check_list_id = check_list_id;
            checkListSites.microting_uid = microting_uid;
            checkListSites.last_check_id = last_check_id;


            if (dbContext.ChangeTracker.HasChanges())
            {
                checkListSites.version += 1;
                checkListSites.updated_at = DateTime.Now;

                dbContext.check_list_site_versions.Add(MapCheckListSiteVersions(checkListSites));
                dbContext.SaveChanges();
                
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            check_list_sites checkListSites = dbContext.check_list_sites.FirstOrDefault(x => x.Id == Id);

            if (checkListSites == null)
            {
                throw  new NullReferenceException($"Could not find Check List Site with Id: {Id}");
            }

            checkListSites.workflow_state = Constants.WorkflowStates.Removed;
                
            if (dbContext.ChangeTracker.HasChanges())
            {
                checkListSites.version += 1;
                checkListSites.updated_at = DateTime.Now;

                dbContext.check_list_site_versions.Add(MapCheckListSiteVersions(checkListSites));
                dbContext.SaveChanges();
                
            }
        }
        
        private check_list_site_versions MapCheckListSiteVersions(check_list_sites checkListSite)
        {
            check_list_site_versions checkListSiteVer = new check_list_site_versions();
            checkListSiteVer.check_list_id = checkListSite.check_list_id;
            checkListSiteVer.created_at = checkListSite.created_at;
            checkListSiteVer.updated_at = checkListSite.updated_at;
            checkListSiteVer.last_check_id = checkListSite.last_check_id;
            checkListSiteVer.microting_uid = checkListSite.microting_uid;
            checkListSiteVer.site_id = checkListSite.site_id;
            checkListSiteVer.version = checkListSite.version;
            checkListSiteVer.workflow_state = checkListSite.workflow_state;

            checkListSiteVer.check_list_site_id = checkListSite.Id; //<<--

            return checkListSiteVer;
        }
    }
}