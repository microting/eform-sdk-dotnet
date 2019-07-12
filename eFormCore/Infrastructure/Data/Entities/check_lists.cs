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

using System.Linq;
using eFormShared;

namespace eFormSqlController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class check_lists : BaseEntity
    {
        public check_lists()
        {
            this.Cases = new HashSet<cases>();
            this.CheckListSites = new HashSet<check_list_sites>();
            this.Children = new HashSet<check_lists>();
            this.Fields = new HashSet<fields>();
            this.Taggings = new HashSet<taggings>();
        }
//
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

        public string Label { get; set; }

        public string Description { get; set; }

        public string Custom { get; set; }

        public int? ParentId { get; set; }

        public int? Repeated { get; set; }

        public int? DisplayIndex { get; set; }

        [StringLength(255)]
        public string CaseType { get; set; }

        [StringLength(255)]
        public string FolderName { get; set; }

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

        public virtual ICollection<cases> Cases { get; set; }

        public virtual ICollection<check_list_sites> CheckListSites { get; set; }

        public virtual ICollection<fields> Fields { get; set; }

        public virtual check_lists Parent { get; set; }

        public virtual ICollection<check_lists> Children { get; set; }

        public virtual ICollection<taggings> Taggings { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Version = 1;
            WorkflowState = Constants.WorkflowStates.Created;

            dbContext.check_lists.Add(this);
            dbContext.SaveChanges();

            dbContext.check_list_versions.Add(MapCheckListVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            check_lists checkList = dbContext.check_lists.FirstOrDefault(x => x.Id == Id);

            if (checkList == null)
            {
                throw new NullReferenceException($"Could not find Checklist with Id: {Id}");
            }

            checkList.Label = Label;
            checkList.Description = Description;
            checkList.Custom = Custom;
            checkList.ParentId = ParentId;
            checkList.Repeated = Repeated;
            checkList.DisplayIndex = DisplayIndex;
            checkList.CaseType = CaseType;
            checkList.FolderName = FolderName;
            checkList.ReviewEnabled = ReviewEnabled;
            checkList.ManualSync = ManualSync;
            checkList.ExtraFieldsEnabled = ExtraFieldsEnabled;
            checkList.DoneButtonEnabled = DoneButtonEnabled;
            checkList.ApprovalEnabled = ApprovalEnabled;
            checkList.MultiApproval = MultiApproval;
            checkList.FastNavigation = FastNavigation;
            checkList.DownloadEntities = DownloadEntities;
            checkList.Field1 = Field1;
            checkList.Field2 = Field2;
            checkList.Field3 = Field3;
            checkList.Field4 = Field4;
            checkList.Field5 = Field5;
            checkList.Field6 = Field6;
            checkList.Field7 = Field7;
            checkList.Field8 = Field8;
            checkList.Field9 = Field9;
            checkList.Field10 = Field10;
            checkList.Color = Color;
            checkList.QuickSyncEnabled = QuickSyncEnabled;
            checkList.OriginalId = OriginalId;
            checkList.JasperExportEnabled = JasperExportEnabled;
            checkList.DocxExportEnabled = DocxExportEnabled;

            if (dbContext.ChangeTracker.HasChanges())
            {
                checkList.UpdatedAt = DateTime.Now;
                checkList.Version += 1;

                dbContext.check_list_versions.Add(MapCheckListVersions(checkList));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            check_lists checkList = dbContext.check_lists.FirstOrDefault(x => x.Id == Id);

            if (checkList == null)
            {
                throw new NullReferenceException($"Could not find Checklist with Id: {Id}");
            }

            checkList.WorkflowState = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                checkList.UpdatedAt = DateTime.Now;
                checkList.Version += 1;

                dbContext.check_list_versions.Add(MapCheckListVersions(checkList));
                dbContext.SaveChanges();
            }
            
        }
        
        private check_list_versions MapCheckListVersions(check_lists checkList)
        {
            check_list_versions clv = new check_list_versions();
            clv.CreatedAt = checkList.CreatedAt;
            clv.UpdatedAt = checkList.UpdatedAt;
            clv.Label = checkList.Label;
            clv.Description = checkList.Description;
            clv.Custom = checkList.Custom;
            clv.WorkflowState = checkList.WorkflowState;
            clv.ParentId = checkList.ParentId;
            clv.Repeated = checkList.Repeated;
            clv.Version = checkList.Version;
            clv.CaseType = checkList.CaseType;
            clv.FolderName = checkList.FolderName;
            clv.DisplayIndex = checkList.DisplayIndex;
            clv.ReviewEnabled = checkList.ReviewEnabled;
            clv.ManualSync = checkList.ManualSync;
            clv.ExtraFieldsEnabled = checkList.ExtraFieldsEnabled;
            clv.DoneButtonEnabled = checkList.DoneButtonEnabled;
            clv.ApprovalEnabled = checkList.ApprovalEnabled;
            clv.MultiApproval = checkList.MultiApproval;
            clv.FastNavigation = checkList.FastNavigation;
            clv.DownloadEntities = checkList.DownloadEntities;
            clv.Field1 = checkList.Field1;
            clv.Field2 = checkList.Field2;
            clv.Field3 = checkList.Field3;
            clv.Field4 = checkList.Field4;
            clv.Field5 = checkList.Field5;
            clv.Field6 = checkList.Field6;
            clv.Field7 = checkList.Field7;
            clv.Field8 = checkList.Field8;
            clv.Field9 = checkList.Field9;
            clv.Field10 = checkList.Field10;
            clv.Color = checkList.Color;
            clv.QuickSyncEnabled = checkList.QuickSyncEnabled;
            clv.OriginalId = checkList.OriginalId;
            clv.JasperExportEnabled = checkList.JasperExportEnabled;
            clv.DocxExportEnabled = checkList.DocxExportEnabled;

            clv.CheckListId = checkList.Id; //<<--

            return clv;
        }

    }
}
