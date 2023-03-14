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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class FieldValue : PnBase
    {
        public DateTime? DoneAt { get; set; }

        public DateTime? Date { get; set; }

        [ForeignKey("Worker")] public int? WorkerId { get; set; }

        public int? CaseId { get; set; }

        [ForeignKey("Field")] public int? FieldId { get; set; }

        [ForeignKey("CheckList")] public int? CheckListId { get; set; }

        public int? CheckListDuplicateId { get; set; }

        [ForeignKey("UploadedData")] public int? UploadedDataId { get; set; }

        public string Value { get; set; }

        [StringLength(255)] public string Latitude { get; set; }

        [StringLength(255)] public string Longitude { get; set; }

        [StringLength(255)] public string Altitude { get; set; }

        [StringLength(255)] public string Heading { get; set; }

        [StringLength(255)] public string Accuracy { get; set; }

        public virtual Worker Worker { get; set; }

        public virtual Field Field { get; set; }

        public virtual CheckList CheckList { get; set; }

        public virtual UploadedData UploadedData { get; set; }
    }
}