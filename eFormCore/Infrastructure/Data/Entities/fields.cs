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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class fields : BaseEntity
    {
        public fields()
        {
            this.Children = new HashSet<fields>();
            this.FieldValues = new HashSet<field_values>();
        }

        public int? ParentFieldId { get; set; }

        [ForeignKey("check_list")]
        public int? CheckListId { get; set; }

        [ForeignKey("field_type")]
        public int? FieldTypeId { get; set; }

        public short? Mandatory { get; set; }

        public short? ReadOnly { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        [StringLength(255)]
        public string Color { get; set; }

        public int? DisplayIndex { get; set; }

        public short? Dummy { get; set; }

        public string DefaultValue { get; set; }

        [StringLength(255)]
        public string UnitName { get; set; }

        [StringLength(255)]
        public string MinValue { get; set; }

        [StringLength(255)]
        public string MaxValue { get; set; }

        public int? MaxLength { get; set; }

        public int? DecimalCount { get; set; }

        public int? Multi { get; set; }

        public short? Optional { get; set; }

        public short? Selected { get; set; }

        public short? SplitScreen { get; set; }

        public short? GeolocationEnabled { get; set; }

        public short? GeolocationForced { get; set; }

        public short? GeolocationHidden { get; set; }

        public short? StopOnSave { get; set; }

        public short? IsNum { get; set; }

        public short? BarcodeEnabled { get; set; }

        [StringLength(255)]
        public string BarcodeType { get; set; }

        [StringLength(255)]
        public string QueryType { get; set; }

        public string KeyValuePairList { get; set; }

        public string Custom { get; set; }

        public int? EntityGroupId { get; set; }

        public string OriginalId { get; set; }

        public virtual field_types FieldType { get; set; }

        public virtual check_lists CheckList { get; set; }

        public virtual fields Parent { get; set; }

        public virtual ICollection<fields> Children { get; set; }

        public virtual ICollection<field_values> FieldValues { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            dbContext.fields.Add(this);
            await dbContext.SaveChangesAsync();

            dbContext.field_versions.Add(MapFieldVersions(this));
            await dbContext.SaveChangesAsync();
        }


        public async Task Update(MicrotingDbContext dbContext)
        {
            fields field = await dbContext.fields.FirstOrDefaultAsync(x => x.Id == Id);

            if (field == null)
            {
                throw new NullReferenceException($"Could not find Field with Id: {Id}");
            }

            field.ParentFieldId = ParentFieldId;
            field.CheckListId = CheckListId;
            field.FieldTypeId = FieldTypeId;
            field.Mandatory = Mandatory;
            field.ReadOnly = ReadOnly;
            field.Label = Label;
            field.Description = Description;
            field.Color = Color;
            field.DisplayIndex = DisplayIndex;
            field.Dummy = Dummy;
            field.DefaultValue = DefaultValue;
            field.UnitName = UnitName;
            field.MinValue = MinValue;
            field.MaxValue = MaxValue;
            field.MaxLength = MaxLength;
            field.DecimalCount = DecimalCount;
            field.Multi = Multi;
            field.Optional = Optional;
            field.Selected = Selected;
            field.SplitScreen = SplitScreen;
            field.GeolocationForced = GeolocationForced;
            field.GeolocationHidden = GeolocationHidden;
            field.GeolocationEnabled = GeolocationEnabled;
            field.StopOnSave = StopOnSave;
            field.IsNum = IsNum;
            field.BarcodeEnabled = BarcodeEnabled;
            field.BarcodeType = BarcodeType;
            field.QueryType = QueryType;
            field.KeyValuePairList = KeyValuePairList;
            field.Custom = Custom;
            field.EntityGroupId = EntityGroupId;
            field.OriginalId = OriginalId;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                field.Version += 1;
                field.UpdatedAt = DateTime.Now;

                dbContext.field_versions.Add(MapFieldVersions(field));
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            fields field = await dbContext.fields.FirstOrDefaultAsync(x => x.Id == Id);

            if (field == null)
            {
                throw new NullReferenceException($"Could not find Field with Id: {Id}");
            }

            field.WorkflowState = Constants.Constants.WorkflowStates.Removed;
            
            if (dbContext.ChangeTracker.HasChanges())
            {
                field.Version += 1;
                field.UpdatedAt = DateTime.Now;

                dbContext.field_versions.Add(MapFieldVersions(field));
                await dbContext.SaveChangesAsync();
            }
        }

        private field_versions MapFieldVersions(fields field)
        {
            return new field_versions
            {
                Version = field.Version,
                CreatedAt = field.CreatedAt,
                UpdatedAt = field.UpdatedAt,
                Custom = field.Custom,
                WorkflowState = field.WorkflowState,
                CheckListId = field.CheckListId,
                Label = field.Label,
                Description = field.Description,
                FieldTypeId = field.FieldTypeId,
                DisplayIndex = field.DisplayIndex,
                Dummy = field.Dummy,
                ParentFieldId = field.ParentFieldId,
                Optional = field.Optional,
                Multi = field.Multi,
                DefaultValue = field.DefaultValue,
                Selected = field.Selected,
                MinValue = field.MinValue,
                MaxValue = field.MaxValue,
                MaxLength = field.MaxLength,
                SplitScreen = field.SplitScreen,
                DecimalCount = field.DecimalCount,
                UnitName = field.UnitName,
                KeyValuePairList = field.KeyValuePairList,
                GeolocationEnabled = field.GeolocationEnabled,
                GeolocationForced = field.GeolocationForced,
                GeolocationHidden = field.GeolocationHidden,
                StopOnSave = field.StopOnSave,
                Mandatory = field.Mandatory,
                ReadOnly = field.ReadOnly,
                Color = field.Color,
                IsNum = field.IsNum,
                OriginalId = field.OriginalId,
                BarcodeEnabled = field.BarcodeEnabled,
                BarcodeType = field.BarcodeType,
                QueryType = field.QueryType,
                EntityGroupId = field.EntityGroupId,
                FieldId = field.Id
            };
        }
    }
}
