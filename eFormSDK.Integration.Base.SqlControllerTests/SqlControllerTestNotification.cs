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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Infrastructure.Helpers;
using NUnit.Framework;

namespace eFormSDK.Integration.Base.SqlControllerTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SqlControllerTestNotification : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            DbContextHelper dbContextHelper = new DbContextHelper(ConnectionString);
            SqlController sql = new SqlController(dbContextHelper);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");

            #endregion

            sut = new SqlController(dbContextHelper);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers(ConnectionString);
            await testHelpers.GenerateDefaultLanguages();
            await sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            await sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            await sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }

        #region notification

        [Test]
        public async Task SQL_Notification_NewNotificationCreateRetrievedForm_DoesStoreNotification()
        {
            // Arrance
            Random rnd = new Random();
            var notificationId = Guid.NewGuid().ToString();
            var microtingUId = rnd.Next(1, 255);

            // Act
            await sut.NotificationCreate(notificationId, microtingUId, Constants.Notifications.RetrievedForm);

            // Assert
            var notification = await DbContext.Notifications.FirstOrDefaultAsync(x =>
                x.NotificationUid == notificationId && x.MicrotingUid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, DbContext.Notifications.Count());
            Assert.AreEqual(Constants.Notifications.RetrievedForm, notification.Activity);
            Assert.AreEqual(Constants.WorkflowStates.Created, notification.WorkflowState);
        }

        [Test]
        public async Task SQL_Notification_NewNotificationCreateCompletedForm_DoesStoreNotification()
        {
            // Arrance
            Random rnd = new Random();
            var notificationId = Guid.NewGuid().ToString();
            var microtingUId = rnd.Next(1, 255);

            // Act
            await sut.NotificationCreate(notificationId, microtingUId, Constants.Notifications.Completed);

            // Assert
            var notification = await DbContext.Notifications.FirstOrDefaultAsync(x =>
                x.NotificationUid == notificationId && x.MicrotingUid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, DbContext.Notifications.Count());
            Assert.AreEqual(Constants.Notifications.Completed, notification.Activity);
            Assert.AreEqual(Constants.WorkflowStates.Created, notification.WorkflowState);
        }

        [Test]
        public async Task SQL_Notification_NotificationReadFirst_DoesReturnFirstNotification()
        {
            // Arrance
            Random rnd = new Random();
            var notificationId1 = Guid.NewGuid().ToString();
            var microtingUId1 = rnd.Next(1, 255);
            var notificationId2 = Guid.NewGuid().ToString();
            var microtingUId2 = rnd.Next(1, 255);

            // Act
            await sut.NotificationCreate(notificationId1, microtingUId1, Constants.Notifications.Completed);
            await sut.NotificationCreate(notificationId2, microtingUId2, Constants.Notifications.Completed);

            // Assert
            NoteDto notification = await sut.NotificationReadFirst();

            Assert.NotNull(notification);
            Assert.AreEqual(2, DbContext.Notifications.Count());
            Assert.AreEqual(Constants.Notifications.Completed, notification.Activity);
            Assert.AreEqual(microtingUId1, notification.MicrotingUId);
        }

        [Test]
        public async Task SQL_Notification_NotificationUpdate_DoesUpdateNotification()
        {
            // Arrance
            Random rnd = new Random();
            var notificationUId = Guid.NewGuid().ToString();
            var microtingUId = rnd.Next(1, 255);

            // Act
            await sut.NotificationCreate(notificationUId, microtingUId, Constants.Notifications.Completed);
            await sut.NotificationUpdate(notificationUId, microtingUId, Constants.WorkflowStates.Processed, "", "");

            // Assert
            var notification = await DbContext.Notifications.FirstOrDefaultAsync(x =>
                x.NotificationUid == notificationUId && x.MicrotingUid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, DbContext.Notifications.Count());
            Assert.AreEqual(Constants.Notifications.Completed, notification.Activity);
            Assert.AreEqual(Constants.WorkflowStates.Processed, notification.WorkflowState);
        }


        [Test]
        public async Task SQL_Notification_Notificationcreate_isCreated()
        {
            // Act
            Random rnd = new Random();
            await sut.NotificationCreate(Guid.NewGuid().ToString(), rnd.Next(1, 255),
                Constants.Notifications.UnitActivate);
            List<Notification> notificationResult = DbContext.Notifications.AsNoTracking().ToList();
            var versionedMatches = DbContext.Notifications.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(notificationResult);
            Assert.AreEqual(1, notificationResult.Count);
            Assert.AreEqual(Constants.Notifications.UnitActivate, notificationResult[0].Activity);
            Assert.AreEqual(Constants.WorkflowStates.Created, notificationResult[0].WorkflowState);
            Assert.AreEqual(Constants.WorkflowStates.Created, versionedMatches[0].WorkflowState);
        }

        [Test]
        public async Task SQL_Notification_NotificationReadFirst_doesReadFirst()
        {
            Random rnd = new Random();
            Notification aNote1 = new Notification
            {
                WorkflowState = Constants.WorkflowStates.Created,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                NotificationUid = "0",
                MicrotingUid = rnd.Next(1, 255),
                Activity = Constants.Notifications.UnitActivate
            };


            DbContext.Notifications.Add(aNote1);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            await sut.NotificationReadFirst();
            List<Notification> notificationResult = DbContext.Notifications.AsNoTracking().ToList();
            var versionedMatches = DbContext.Notifications.AsNoTracking().ToList();


            // Assert
            Assert.AreEqual(Constants.WorkflowStates.Created, notificationResult[0].WorkflowState);
        }

        [Test]
        public async Task SQL_Notification_NotificationUpdate_doesGetUpdated()
        {
            Random rnd = new Random();
            Notification aNote1 = new Notification
            {
                WorkflowState = Constants.WorkflowStates.Created,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                NotificationUid = "0",
                MicrotingUid = rnd.Next(1, 255),
                Activity = Constants.Notifications.UnitActivate
            };


            DbContext.Notifications.Add(aNote1);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            await sut.NotificationUpdate(aNote1.NotificationUid, (int)aNote1.MicrotingUid, aNote1.WorkflowState,
                aNote1.Exception, "");
            List<Notification> notificationResult = DbContext.Notifications.AsNoTracking().ToList();
            var versionedMatches = DbContext.Notifications.AsNoTracking().ToList();

            // Assert

            Assert.AreEqual(aNote1.NotificationUid, notificationResult[0].NotificationUid);
            Assert.AreEqual(aNote1.MicrotingUid, notificationResult[0].MicrotingUid);
        }

        #endregion


        #region eventhandlers

#pragma warning disable 1998
        public async Task EventCaseCreated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventCaseRetrived(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventCaseCompleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventCaseDeleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventFileDownloaded(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public async Task EventSiteActivated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }
#pragma warning restore 1998

        #endregion
    }
}