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

using System.ComponentModel.DataAnnotations;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class FieldVersion : BaseEntity
    {
        public int? FieldId { get; set; }

        public int? ParentFieldId { get; set; }

        public int? CheckListId { get; set; }

        public int? FieldTypeId { get; set; }

        public short? Mandatory { get; set; }

        public short? ReadOnly { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        [StringLength(255)] public string Color { get; set; }

        public int? DisplayIndex { get; set; }

        public short? Dummy { get; set; }

        public string DefaultValue { get; set; }

        [StringLength(255)] public string UnitName { get; set; }

        [StringLength(255)] public string MinValue { get; set; }

        [StringLength(255)] public string MaxValue { get; set; }

        public int? MaxLength { get; set; }

        public int? DecimalCount { get; set; }

        public int? Multi { get; set; }

        public short? Optional { get; set; }

        public short? Selected { get; set; }

        public short? Split { get; set; }

        public short? GeolocationEnabled { get; set; }

        public short? GeolocationForced { get; set; }

        public short? GeolocationHidden { get; set; }

        public short? StopOnSave { get; set; }

        public short? IsNum { get; set; }

        public short? BarcodeEnabled { get; set; }

        [StringLength(255)] public string BarcodeType { get; set; }

        [StringLength(255)] public string QueryType { get; set; }

        public string KeyValuePairList { get; set; }

        public string Custom { get; set; }

        public int? EntityGroupId { get; set; }

        public string OriginalId { get; set; }
    }
}