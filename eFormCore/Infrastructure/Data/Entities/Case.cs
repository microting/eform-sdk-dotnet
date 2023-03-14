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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class Case : PnBase
    {
        public int? Status { get; set; }

        public DateTime? DoneAt { get; set; }

        [ForeignKey("Site")] public int? SiteId { get; set; }

        [ForeignKey("Unit")] public int? UnitId { get; set; }

        [ForeignKey("Worker")] public int? WorkerId { get; set; }

        [ForeignKey("CheckList")] public int? CheckListId { get; set; }

        [StringLength(255)] public string Type { get; set; }

        public int? MicrotingUid { get; set; }

        public int? MicrotingCheckUid { get; set; }

        [StringLength(255)] public string CaseUid { get; set; }

        public string Custom { get; set; }

        public string FieldValue1 { get; set; }

        public string FieldValue2 { get; set; }

        public string FieldValue3 { get; set; }

        public string FieldValue4 { get; set; }

        public string FieldValue5 { get; set; }

        public string FieldValue6 { get; set; }

        public string FieldValue7 { get; set; }

        public string FieldValue8 { get; set; }

        public string FieldValue9 { get; set; }

        public string FieldValue10 { get; set; }

        [ForeignKey("Folder")] public int? FolderId { get; set; }

        public bool IsArchived { get; set; }

        public DateTime? DoneAtUserModifiable { get; set; }

        public virtual CheckList CheckList { get; set; }

        public virtual Site Site { get; set; }

        public virtual Unit Unit { get; set; }

        public virtual Worker Worker { get; set; }

        public virtual Folder Folder { get; set; }

        public DateTime? ReceivedByServerAt { get; set; }
    }
}