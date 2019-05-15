using eFormCore;
using eFormCore.Helpers;
using eFormData;
using eFormShared;
using eFormSqlController;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTestNotification : DbTestFixture
    {
        private SqlController sut;
        private TestHelpers testHelpers;

        public override void DoSetup()
        {
            #region Setup SettingsTableContent

            SqlController sql = new SqlController(ConnectionString);
            sql.SettingUpdate(Settings.token, "abc1234567890abc1234567890abcdef");
            sql.SettingUpdate(Settings.firstRunDone, "true");
            sql.SettingUpdate(Settings.knownSitesDone, "true");
            #endregion

            sut = new SqlController(ConnectionString);
            sut.StartLog(new CoreBase());
            testHelpers = new TestHelpers();
            sut.SettingUpdate(Settings.fileLocationPicture, @"\output\dataFolder\picture\");
            sut.SettingUpdate(Settings.fileLocationPdf, @"\output\dataFolder\pdf\");
            sut.SettingUpdate(Settings.fileLocationJasper, @"\output\dataFolder\reports\");
        }

        #region notification
        [Test]
        public void SQL_Notification_NewNotificationCreateRetrievedForm_DoesStoreNotification()
        {
            // Arrance
            var notificationId = Guid.NewGuid().ToString();
            var microtingUId = Guid.NewGuid().ToString();

            // Act
            sut.NotificationCreate(notificationId, microtingUId, Constants.Notifications.RetrievedForm);

            // Assert
            var notification = DbContext.notifications.SingleOrDefault(x => x.NotificationUid == notificationId && x.MicrotingUid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, DbContext.notifications.Count());
            Assert.AreEqual(Constants.Notifications.RetrievedForm, notification.Activity);
            Assert.AreEqual(Constants.WorkflowStates.Created, notification.WorkflowState);
        }

        [Test]
        public void SQL_Notification_NewNotificationCreateCompletedForm_DoesStoreNotification()
        {
            // Arrance
            var notificationId = Guid.NewGuid().ToString();
            var microtingUId = Guid.NewGuid().ToString();

            // Act
            sut.NotificationCreate(notificationId, microtingUId, Constants.Notifications.Completed);

            // Assert
            var notification = DbContext.notifications.SingleOrDefault(x => x.NotificationUid == notificationId && x.MicrotingUid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, DbContext.notifications.Count());
            Assert.AreEqual(Constants.Notifications.Completed, notification.Activity);
            Assert.AreEqual(Constants.WorkflowStates.Created, notification.WorkflowState);
        }

        [Test]
        public void SQL_Notification_NotificationReadFirst_DoesReturnFirstNotification()
        {
            // Arrance
            var notificationId1 = Guid.NewGuid().ToString();
            var microtingUId1 = Guid.NewGuid().ToString();
            var notificationId2 = Guid.NewGuid().ToString();
            var microtingUId2 = Guid.NewGuid().ToString();

            // Act
            sut.NotificationCreate(notificationId1, microtingUId1, Constants.Notifications.Completed);
            sut.NotificationCreate(notificationId2, microtingUId2, Constants.Notifications.Completed);

            // Assert
            Note_Dto notification = sut.NotificationReadFirst();

            Assert.NotNull(notification);
            Assert.AreEqual(2, DbContext.notifications.Count());
            Assert.AreEqual(Constants.Notifications.Completed, notification.Activity);
            Assert.AreEqual(microtingUId1, notification.MicrotingUId);
        }

        [Test]
        public void SQL_Notification_NotificationUpdate_DoesUpdateNotification()
        {
            // Arrance
            var notificationUId = Guid.NewGuid().ToString();
            var microtingUId = Guid.NewGuid().ToString();

            // Act
            sut.NotificationCreate(notificationUId, microtingUId, Constants.Notifications.Completed);
            sut.NotificationUpdate(notificationUId, microtingUId, Constants.WorkflowStates.Processed, "", "");

            // Assert
            var notification = DbContext.notifications.SingleOrDefault(x => x.NotificationUid == notificationUId && x.MicrotingUid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, DbContext.notifications.Count());
            Assert.AreEqual(Constants.Notifications.Completed, notification.Activity);
            Assert.AreEqual(Constants.WorkflowStates.Processed, notification.WorkflowState);
        }


        [Test]
        public void SQL_Notification_Notificationcreate_isCreated()
        {


            // Act
            sut.NotificationCreate(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Constants.Notifications.UnitActivate);
            List<notifications> notificationResult = DbContext.notifications.AsNoTracking().ToList();
            var versionedMatches = DbContext.notifications.AsNoTracking().ToList();

            // Assert

            Assert.NotNull(notificationResult);
            Assert.AreEqual(1, notificationResult.Count);
            Assert.AreEqual(Constants.Notifications.UnitActivate, notificationResult[0].Activity);
            Assert.AreEqual(Constants.WorkflowStates.Created, notificationResult[0].WorkflowState);
            Assert.AreEqual(Constants.WorkflowStates.Created, versionedMatches[0].WorkflowState);



        }

        [Test]
        public void SQL_Notification_NotificationReadFirst_doesReadFirst()
        {
            notifications aNote1 = new notifications();

            aNote1.WorkflowState = Constants.WorkflowStates.Created;
            aNote1.CreatedAt = DateTime.Now;
            aNote1.UpdatedAt = DateTime.Now;
            aNote1.NotificationUid = "0";
            aNote1.MicrotingUid = "1";
            aNote1.Activity = Constants.Notifications.UnitActivate;

            DbContext.notifications.Add(aNote1);
            DbContext.SaveChanges();

            // Act
            sut.NotificationReadFirst();
            List<notifications> notificationResult = DbContext.notifications.AsNoTracking().ToList();
            var versionedMatches = DbContext.notifications.AsNoTracking().ToList();


            // Assert
            Assert.AreEqual(Constants.WorkflowStates.Created, notificationResult[0].WorkflowState);


        }

        [Test]
        public void SQL_Notification_NotificationUpdate_doesGetUpdated()
        {
            notifications aNote1 = new notifications();

            aNote1.WorkflowState = Constants.WorkflowStates.Created;
            aNote1.CreatedAt = DateTime.Now;
            aNote1.UpdatedAt = DateTime.Now;
            aNote1.NotificationUid = "0";
            aNote1.MicrotingUid = "1";
            aNote1.Activity = Constants.Notifications.UnitActivate;

            DbContext.notifications.Add(aNote1);
            DbContext.SaveChanges();

            // Act
            sut.NotificationUpdate(aNote1.NotificationUid, aNote1.MicrotingUid, aNote1.WorkflowState, aNote1.Exception, "");
            List<notifications> notificationResult = DbContext.notifications.AsNoTracking().ToList();
            var versionedMatches = DbContext.notifications.AsNoTracking().ToList();

            // Assert

            Assert.AreEqual(aNote1.NotificationUid, notificationResult[0].NotificationUid);
            Assert.AreEqual(aNote1.MicrotingUid, notificationResult[0].MicrotingUid);
        }
        #endregion

        #region eventhandlers
        public void EventCaseCreated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseRetrived(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseCompleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventCaseDeleted(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventFileDownloaded(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }

        public void EventSiteActivated(object sender, EventArgs args)
        {
            // Does nothing for web implementation
        }
        #endregion
    }

}