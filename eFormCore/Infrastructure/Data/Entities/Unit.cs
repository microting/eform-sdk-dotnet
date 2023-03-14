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

using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class Unit : PnBase
    {
        public int? MicrotingUid { get; set; }

        public int? OtpCode { get; set; }

        public int? CustomerNo { get; set; }

        [ForeignKey("Site")] public int? SiteId { get; set; }

        public virtual Site Site { get; set; }

        public string Os { get; set; }

        public string OsVersion { get; set; }

        public string eFormVersion { get; set; }

        public string InSightVersion { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public string Note { get; set; }

        public string SerialNumber { get; set; }

        public string LastIp { get; set; }

        public bool SeparateFetchSend { get; set; }

        public bool LeftMenuEnabled { get; set; }

        public bool SyncDialog { get; set; }

        public bool PushEnabled { get; set; }

        public bool SyncDelayEnabled { get; set; }

        public int SyncDefaultDelay { get; set; }

        public int SyncDelayPrCheckList { get; set; }

        public bool IsLocked { get; set; }
    }
}