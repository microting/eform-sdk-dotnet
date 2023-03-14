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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)] public string WorkflowState { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? MicrotingUid { get; set; }

        public string Transmission { get; set; }

        [StringLength(255)] public string NotificationUid { get; set; }

        public string Activity { get; set; }

        public string Exception { get; set; }

        public string Stacktrace { get; set; }

        public int Version { get; set; }

        public async Task Create(MicrotingDbContext dbContext)
        {
            WorkflowState = Constants.Constants.WorkflowStates.Created;
            Version = 1;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            dbContext.Notifications.Add(this);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            dbContext.NotificationVersions.Add(MapVersions(this));
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            Notification notification = await dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == Id);

            if (notification == null)
            {
                throw new NullReferenceException($"Could not find notification with id {Id}");
            }

            notification.WorkflowState = WorkflowState;
            notification.MicrotingUid = MicrotingUid;
            notification.Transmission = Transmission;
            notification.NotificationUid = NotificationUid;
            notification.Activity = Activity;
            notification.Exception = Exception;
            notification.Stacktrace = Stacktrace;

            if (dbContext.ChangeTracker.HasChanges())
            {
                notification.UpdatedAt = DateTime.UtcNow;
                notification.Version += 1;

                dbContext.NotificationVersions.Add(MapVersions(notification));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            Notification notification = await dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == Id);

            if (notification == null)
            {
                throw new NullReferenceException($"Could not find notification with id {Id}");
            }

            notification.WorkflowState = Constants.Constants.WorkflowStates.Removed;

            if (dbContext.ChangeTracker.HasChanges())
            {
                notification.UpdatedAt = DateTime.UtcNow;
                notification.Version += 1;

                dbContext.NotificationVersions.Add(MapVersions(notification));
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private NotificationVersion MapVersions(Notification notification)
        {
            return new NotificationVersion
            {
                WorkflowState = notification.WorkflowState,
                CreatedAt = notification.CreatedAt,
                UpdatedAt = notification.UpdatedAt,
                MicrotingUid = notification.MicrotingUid,
                Transmission = notification.Transmission,
                NotificationUid = notification.NotificationUid,
                Activity = notification.Activity,
                Exception = notification.Exception,
                Stacktrace = notification.Stacktrace,
                NotificationId = notification.Id,
                Version = notification.Version
            };
        }
    }
}