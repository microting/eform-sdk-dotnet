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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class Site : PnBase
    {
        public Site()
        {
            Cases = new HashSet<Case>();
            Units = new HashSet<Unit>();
            SiteWorkers = new HashSet<SiteWorker>();
            CheckListSites = new HashSet<CheckListSite>();
            SiteTags = new List<SiteTag>();
        }

        [StringLength(255)] public string Name { get; set; }

        public int? MicrotingUid { get; set; }

        [ForeignKey("Language")] public int LanguageId { get; set; }

        public int SearchableEntityItemId { get; set; }

        public int SelectableEntityItemId { get; set; }

        public bool IsLocked { get; set; }

        public virtual ICollection<Case> Cases { get; set; }

        public virtual ICollection<Unit> Units { get; set; }

        public virtual ICollection<SiteWorker> SiteWorkers { get; set; }

        public virtual ICollection<CheckListSite> CheckListSites { get; set; }

        public virtual ICollection<SiteTag> SiteTags { get; set; }

        public static async Task AddLanguage(MicrotingDbContext dbContext)
        {
            List<Site> sites = await dbContext.Sites.ToListAsync();
            Language language = await dbContext.Languages
                .FirstAsync(x => x.Name == "Danish");
            foreach (Site site in sites)
            {
                if (site.LanguageId == 0)
                {
                    site.LanguageId = language.Id;
                    await site.Update(dbContext);
                }
            }
        }
    }
}