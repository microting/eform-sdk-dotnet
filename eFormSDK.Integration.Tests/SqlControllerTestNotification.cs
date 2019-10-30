using eFormCore;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microting.eForm;
using Microting.eForm.Dto;
using Microting.eForm.Helpers;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestNotification : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

        public override async Task DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            await sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            await sql.SettingUpdate(Settings.firstRunDone, "true");
            await sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(ConnectionString);
            await sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
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
            var notification = await dbContext.notifications.SingleOrDefaultAsync(x => x.NotificationUid == notificationId && x.MicrotingUid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, dbContext.notifications.Count());
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
            var notification = await dbContext.notifications.SingleOrDefaultAsync(x => x.NotificationUid == notificationId && x.MicrotingUid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, dbContext.notifications.Count());
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
            Note_Dto notification = await sut.NotificationReadFirst();

            Assert.NotNull(notification);
            Assert.AreEqual(2, dbContext.notifications.Count());
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
            var notification = await dbContext.notifications.SingleOrDefaultAsync(x => x.NotificationUid == notificationUId && x.MicrotingUid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, dbContext.notifications.Count());
            Assert.AreEqual(Constants.Notifications.Completed, notification.Activity);
            Assert.AreEqual(Constants.WorkflowStates.Processed, notification.WorkflowState);
        }


        [Test]
        public async Task SQL_Notification_Notificationcreate_isCreated()
        {


            // Act
            Random rnd = new Random();
            await sut.NotificationCreate(Guid.NewGuid().ToString(), rnd.Next(1, 255), Constants.Notifications.UnitActivate);
            List<notifications> notificationResult = dbContext.notifications.AsNoTracking().ToList();
            var versionedMatches = dbContext.notifications.AsNoTracking().ToList();

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
            notifications aNote1 = new notifications();

            aNote1.WorkflowState = Constants.WorkflowStates.Created;
            aNote1.CreatedAt = DateTime.Now;
            aNote1.UpdatedAt = DateTime.Now;
            aNote1.NotificationUid = "0";
            aNote1.MicrotingUid = rnd.Next(1, 255);
            aNote1.Activity = Constants.Notifications.UnitActivate;

            dbContext.notifications.Add(aNote1);
            await dbContext.SaveChangesAsync();

            // Act
            await sut.NotificationReadFirst();
            List<notifications> notificationResult = dbContext.notifications.AsNoTracking().ToList();
            var versionedMatches = dbContext.notifications.AsNoTracking().ToList();


            // Assert
            Assert.AreEqual(Constants.WorkflowStates.Created, notificationResult[0].WorkflowState);


        }

        [Test]
        public async Task SQL_Notification_NotificationUpdate_doesGetUpdated()
        {
            Random rnd = new Random();
            notifications aNote1 = new notifications();

            aNote1.WorkflowState = Constants.WorkflowStates.Created;
            aNote1.CreatedAt = DateTime.Now;
            aNote1.UpdatedAt = DateTime.Now;
            aNote1.NotificationUid = "0";
            aNote1.MicrotingUid = rnd.Next(1, 255);
            aNote1.Activity = Constants.Notifications.UnitActivate;

            dbContext.notifications.Add(aNote1);
            await dbContext.SaveChangesAsync();

            // Act
            await sut.NotificationUpdate(aNote1.NotificationUid, (int)aNote1.MicrotingUid, aNote1.WorkflowState, aNote1.Exception, "");
            List<notifications> notificationResult = dbContext.notifications.AsNoTracking().ToList();
            var versionedMatches = dbContext.notifications.AsNoTracking().ToList();

            // Assert

            Assert.AreEqual(aNote1.NotificationUid, notificationResult[0].NotificationUid);
            Assert.AreEqual(aNote1.MicrotingUid, notificationResult[0].MicrotingUid);
        }
        #endregion

        #region eventhandlers
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
        #endregion
    }

}