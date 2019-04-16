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

    public partial class check_lists : base_entity
    {
        public check_lists()
        {
            this.cases = new HashSet<cases>();
            this.check_list_sites = new HashSet<check_list_sites>();
            this.children = new HashSet<check_lists>();
            this.fields = new HashSet<fields>();
            this.taggings = new HashSet<taggings>();
        }
//
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int id { get; set; }
//
//        [StringLength(255)]
//        public string workflow_state { get; set; }
//
//        public int? version { get; set; }
//
//        public DateTime? created_at { get; set; }
//
//        public DateTime? updated_at { get; set; }

        public string label { get; set; }

        public string description { get; set; }

        public string custom { get; set; }

        public int? parent_id { get; set; }

        public int? repeated { get; set; }

        public int? display_index { get; set; }

        [StringLength(255)]
        public string case_type { get; set; }

        [StringLength(255)]
        public string folder_name { get; set; }

        public short? review_enabled { get; set; }

        public short? manual_sync { get; set; }

        public short? extra_fields_enabled { get; set; }

        public short? done_button_enabled { get; set; }

        public short? approval_enabled { get; set; }

        public short? multi_approval { get; set; }

        public short? fast_navigation { get; set; }

        public short? download_entities { get; set; }

        public int? field_1 { get; set; }

        public int? field_2 { get; set; }

        public int? field_3 { get; set; }

        public int? field_4 { get; set; }

        public int? field_5 { get; set; }

        public int? field_6 { get; set; }

        public int? field_7 { get; set; }

        public int? field_8 { get; set; }

        public int? field_9 { get; set; }

        public int? field_10 { get; set; }

        public short? quick_sync_enabled { get; set; }

        public string original_id { get; set; }

        public virtual ICollection<cases> cases { get; set; }

        public virtual ICollection<check_list_sites> check_list_sites { get; set; }

        public virtual ICollection<fields> fields { get; set; }

        public virtual check_lists parent { get; set; }

        public virtual ICollection<check_lists> children { get; set; }

        public virtual ICollection<taggings> taggings { get; set; }

        public void Create(MicrotingDbAnySql dbContext)
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
            version = 1;
            workflow_state = Constants.WorkflowStates.Created;

            dbContext.check_lists.Add(this);
            dbContext.SaveChanges();

            dbContext.check_list_versions.Add(MapCheckListVersions(this));
            dbContext.SaveChanges();
        }

        public void Update(MicrotingDbAnySql dbContext)
        {
            check_lists checkList = dbContext.check_lists.FirstOrDefault(x => x.id == id);

            if (checkList == null)
            {
                throw new NullReferenceException($"Could not find Checklist with ID: {id}");
            }

            checkList.label = label;
            checkList.description = description;
            checkList.custom = custom;
            checkList.parent_id = parent_id;
            checkList.repeated = repeated;
            checkList.display_index = display_index;
            checkList.case_type = case_type;
            checkList.folder_name = folder_name;
            checkList.review_enabled = review_enabled;
            checkList.manual_sync = manual_sync;
            checkList.extra_fields_enabled = extra_fields_enabled;
            checkList.done_button_enabled = done_button_enabled;
            checkList.approval_enabled = approval_enabled;
            checkList.multi_approval = multi_approval;
            checkList.fast_navigation = fast_navigation;
            checkList.download_entities = download_entities;
            checkList.field_1 = field_1;
            checkList.field_2 = field_2;
            checkList.field_3 = field_3;
            checkList.field_4 = field_4;
            checkList.field_5 = field_5;
            checkList.field_6 = field_6;
            checkList.field_7 = field_7;
            checkList.field_8 = field_8;
            checkList.field_9 = field_9;
            checkList.field_10 = field_10;
            checkList.quick_sync_enabled = quick_sync_enabled;
            checkList.original_id = original_id;

            if (dbContext.ChangeTracker.HasChanges())
            {
                checkList.updated_at = DateTime.Now;
                checkList.version += 1;

                dbContext.check_list_versions.Add(MapCheckListVersions(checkList));
                dbContext.SaveChanges();
            }
        }

        public void Delete(MicrotingDbAnySql dbContext)
        {
            check_lists checkList = dbContext.check_lists.FirstOrDefault(x => x.id == id);

            if (checkList == null)
            {
                throw new NullReferenceException($"Could not find Checklist with ID: {id}");
            }

            checkList.workflow_state = Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                checkList.updated_at = DateTime.Now;
                checkList.version += 1;

                dbContext.check_list_versions.Add(MapCheckListVersions(checkList));
                dbContext.SaveChanges();
            }
            
        }
        
        private check_list_versions MapCheckListVersions(check_lists checkList)
        {
            check_list_versions clv = new check_list_versions();
            clv.created_at = checkList.created_at;
            clv.updated_at = checkList.updated_at;
            clv.label = checkList.label;
            clv.description = checkList.description;
            clv.custom = checkList.custom;
            clv.workflow_state = checkList.workflow_state;
            clv.parent_id = checkList.parent_id;
            clv.repeated = checkList.repeated;
            clv.version = checkList.version;
            clv.case_type = checkList.case_type;
            clv.folder_name = checkList.folder_name;
            clv.display_index = checkList.display_index;
            clv.review_enabled = checkList.review_enabled;
            clv.manual_sync = checkList.manual_sync;
            clv.extra_fields_enabled = checkList.extra_fields_enabled;
            clv.done_button_enabled = checkList.done_button_enabled;
            clv.approval_enabled = checkList.approval_enabled;
            clv.multi_approval = checkList.multi_approval;
            clv.fast_navigation = checkList.fast_navigation;
            clv.download_entities = checkList.download_entities;
            clv.field_1 = checkList.field_1;
            clv.field_2 = checkList.field_2;
            clv.field_3 = checkList.field_3;
            clv.field_4 = checkList.field_4;
            clv.field_5 = checkList.field_5;
            clv.field_6 = checkList.field_6;
            clv.field_7 = checkList.field_7;
            clv.field_8 = checkList.field_8;
            clv.field_9 = checkList.field_9;
            clv.field_10 = checkList.field_10;

            clv.check_list_id = checkList.id; //<<--

            return clv;
        }

    }
}
