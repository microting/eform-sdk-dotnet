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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public partial class units : BaseEntity
    {
        public int? MicrotingUid { get; set; }

        public int? OtpCode { get; set; }

        public int? CustomerNo { get; set; }

        [ForeignKey("site")]
        public int? SiteId { get; set; }

        public virtual sites Site { get; set; }
        
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
        
        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            dbContext.units.Add(this);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            dbContext.unit_versions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            units unit = await dbContext.units.FirstOrDefaultAsync(x => x.Id == Id);

            if (unit == null)
            {
                throw new NullReferenceException($"Could not find Unit with Id: {Id}");
            }

            unit.SiteId = SiteId;
            unit.MicrotingUid = MicrotingUid;
            unit.OtpCode = OtpCode;
            unit.Model = Model;
            unit.Manufacturer = Manufacturer;
            unit.eFormVersion = eFormVersion;
            unit.InSightVersion = InSightVersion;
            unit.Os = Os;
            unit.OsVersion = OsVersion;
            unit.Note = Note;
            unit.CustomerNo = CustomerNo;
            unit.SerialNumber = SerialNumber;
            unit.LastIp = LastIp;
            unit.SeparateFetchSend = SeparateFetchSend;
            unit.LeftMenuEnabled = LeftMenuEnabled;
            unit.SyncDialog = SyncDialog;
            unit.PushEnabled = PushEnabled;
            unit.SyncDelayEnabled = SyncDelayEnabled;
            unit.SyncDefaultDelay = SyncDefaultDelay;
            unit.SyncDelayPrCheckList = SyncDelayPrCheckList;

            if (dbContext.ChangeTracker.HasChanges())
            {
                unit.Version += 1;
                unit.UpdatedAt = DateTime.UtcNow;

                dbContext.unit_versions.Add(MapVersions(unit));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            
            units unit = await dbContext.units.FirstOrDefaultAsync(x => x.Id == Id);

            if (unit == null)
            {
                throw new NullReferenceException($"Could not find Unit with Id: {Id}");
            }

            unit.WorkflowState = Constants.Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                unit.Version += 1;
                unit.UpdatedAt = DateTime.UtcNow;

                dbContext.unit_versions.Add(MapVersions(unit));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }
        
        private unit_versions MapVersions(units unit)
        {
            return new unit_versions
            {
                WorkflowState = unit.WorkflowState,
                Version = unit.Version,
                CreatedAt = unit.CreatedAt,
                UpdatedAt = unit.UpdatedAt,
                MicrotingUid = unit.MicrotingUid,
                SiteId = unit.SiteId,
                CustomerNo = unit.CustomerNo,
                OtpCode = unit.OtpCode,
                UnitId = unit.Id,
                Manufacturer = unit.Manufacturer,
                Model = unit.Model,
                Os = unit.Os,
                OsVersion = unit.OsVersion,
                eFormVersion = unit.eFormVersion,
                InSightVersion = unit.InSightVersion,
                Note = unit.Note,
                SerialNumber = unit.SerialNumber,
                LastIp = unit.LastIp,
                SeparateFetchSend = unit.SeparateFetchSend,
                LeftMenuEnabled = unit.LeftMenuEnabled,
                SyncDialog = unit.SyncDialog,
                PushEnabled = unit.PushEnabled,
                SyncDelayEnabled = unit.SyncDelayEnabled,
                SyncDefaultDelay = unit.SyncDefaultDelay,
                SyncDelayPrCheckList = unit.SyncDelayPrCheckList
            };
        }
    }
}
