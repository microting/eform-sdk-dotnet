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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities;

public class CheckList : PnBase
{
    public CheckList()
    {
        Cases = new HashSet<Case>();
        CheckListSites = new HashSet<CheckListSite>();
        Children = new HashSet<CheckList>();
        Fields = new HashSet<Field>();
        Taggings = new HashSet<Tagging>();
        Translations = new HashSet<CheckListTranslation>();
    }

    public string Label { get; set; }

    public string Description { get; set; }

    public string Custom { get; set; }

    public int? ParentId { get; set; }

    public int? Repeated { get; set; }

    public int? DisplayIndex { get; set; }

    [StringLength(255)] public string CaseType { get; set; }

    [StringLength(255)] public string FolderName { get; set; }

    public short? ReviewEnabled { get; set; }

    public short? ManualSync { get; set; }

    public short? ExtraFieldsEnabled { get; set; }

    public short? DoneButtonEnabled { get; set; }

    public short? ApprovalEnabled { get; set; }

    public short? MultiApproval { get; set; }

    public short? FastNavigation { get; set; }

    public short? DownloadEntities { get; set; }

    public int? Field1 { get; set; }

    public int? Field2 { get; set; }

    public int? Field3 { get; set; }

    public int? Field4 { get; set; }

    public int? Field5 { get; set; }

    public int? Field6 { get; set; }

    public int? Field7 { get; set; }

    public int? Field8 { get; set; }

    public int? Field9 { get; set; }

    public int? Field10 { get; set; }

    public short? QuickSyncEnabled { get; set; }

    public string OriginalId { get; set; }

    public string Color { get; set; }

    public bool JasperExportEnabled { get; set; }

    public bool DocxExportEnabled { get; set; }

    public bool ExcelExportEnabled { get; set; }

    public string ReportH1 { get; set; }

    public string ReportH2 { get; set; }

    public string ReportH3 { get; set; }

    public string ReportH4 { get; set; }

    public string ReportH5 { get; set; }

    public bool IsLocked { get; set; }

    public bool IsEditable { get; set; }

    public bool IsHidden { get; set; }

    public bool IsAchievable { get; set; }

    public bool IsDoneAtEditable { get; set; }

    public virtual ICollection<Case> Cases { get; set; }

    public virtual ICollection<CheckListSite> CheckListSites { get; set; }

    public virtual ICollection<Field> Fields { get; set; }

    public virtual CheckList Parent { get; set; }

    public virtual ICollection<CheckList> Children { get; set; }

    public virtual ICollection<Tagging> Taggings { get; set; }

    public virtual ICollection<CheckListTranslation> Translations { get; set; }

    public static async Task MoveTranslations(MicrotingDbContext dbContext)
    {
        List<CheckList> checkLists = await dbContext.CheckLists.ToListAsync();
        Language language = await dbContext.Languages.FirstAsync(x => x.Name == "Dansk");
        foreach (CheckList checkList in checkLists)
        {
            if (!string.IsNullOrEmpty(checkList.Label))
            {
                CheckListTranslation checkListTranslation = new CheckListTranslation
                {
                    Text = checkList.Label,
                    Description = checkList.Description,
                    CheckListId = checkList.Id,
                    LanguageId = language.Id
                };
                await checkListTranslation.Create(dbContext);
                checkList.Label = null;
                checkList.Description = null;
                await checkList.Update(dbContext);
            }
        }
    }
}