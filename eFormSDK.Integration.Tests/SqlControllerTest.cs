using eFormData;
using eFormShared;
using eFormSqlController;
using NUnit.Framework;
using System;
using System.Linq;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTest : DbTestFixture
    {
        private SqlController sut;

        public override void DoSetup()
        {
            sut = new SqlController(ConnectionString);
            sut.StartLog(new CoreBase());
        }

        [Test]
        public void Notification_NewNotificationCreateRetrievedForm_DoesStoreNotification()
        {
            // Arrance
            var notificationId = Guid.NewGuid().ToString();
            var microtingUId = Guid.NewGuid().ToString();

            // Act
            sut.NotificationCreate(notificationId, microtingUId, Constants.Notifications.RetrievedForm);

            // Assert
            var notification = DbContext.notifications.SingleOrDefault(x => x.notification_uid == notificationId && x.microting_uid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, DbContext.notifications.Count());
            Assert.AreEqual(Constants.Notifications.RetrievedForm, notification.activity);
            Assert.AreEqual(Constants.WorkflowStates.Created, notification.workflow_state);
        }

        [Test]
        public void Notification_NewNotificationCreateCompletedForm_DoesStoreNotification()
        {
            // Arrance
            var notificationId = Guid.NewGuid().ToString();
            var microtingUId = Guid.NewGuid().ToString();

            // Act
            sut.NotificationCreate(notificationId, microtingUId, Constants.Notifications.Completed);

            // Assert
            var notification = DbContext.notifications.SingleOrDefault(x => x.notification_uid == notificationId && x.microting_uid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, DbContext.notifications.Count());
            Assert.AreEqual(Constants.Notifications.Completed, notification.activity);
            Assert.AreEqual(Constants.WorkflowStates.Created, notification.workflow_state);
        }

        [Test]
        public void Notification_NotificationReadFirst_DoesReturnFirstNotification()
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
        public void Notification_NotificationUpdate_DoesUpdateNotification()
        {
            // Arrance
            var notificationUId = Guid.NewGuid().ToString();
            var microtingUId = Guid.NewGuid().ToString();

            // Act
            sut.NotificationCreate(notificationUId, microtingUId, Constants.Notifications.Completed);
            sut.NotificationUpdate(notificationUId, microtingUId, Constants.WorkflowStates.Processed);

            // Assert
            var notification = DbContext.notifications.SingleOrDefault(x => x.notification_uid == notificationUId && x.microting_uid == microtingUId);

            Assert.NotNull(notification);
            Assert.AreEqual(1, DbContext.notifications.Count());
            Assert.AreEqual(Constants.Notifications.Completed, notification.activity);
            Assert.AreEqual(Constants.WorkflowStates.Processed, notification.workflow_state);
        }

        [Test]
        public void UploadedData_FileRead_DoesReturnOneUploadedData()
        {
            // Arrance
            string checksum = "";
            string extension = "jpg";
            string currentFile = "Hello.jpg";
            int uploaderId = 1;
            string fileLocation = @"c:\here";
            string fileName = "Hello.jpg";

            // Act
            uploaded_data dU = new uploaded_data();

            dU.created_at = DateTime.Now;
            dU.updated_at = DateTime.Now;
            dU.extension = extension;
            dU.uploader_id = uploaderId;
            dU.uploader_type = Constants.UploaderTypes.System;
            dU.workflow_state = Constants.WorkflowStates.PreCreated;
            dU.version = 1;
            dU.local = 0;
            dU.file_location = fileLocation;
            dU.file_name = fileName;
            dU.current_file = currentFile;
            dU.checksum = checksum;

            DbContext.uploaded_data.Add(dU);
            DbContext.SaveChanges();

            UploadedData ud = sut.FileRead();

            // Assert
            Assert.NotNull(ud);
            Assert.AreEqual(dU.id, ud.Id);
            Assert.AreEqual(dU.checksum, ud.Checksum);
            Assert.AreEqual(dU.extension, ud.Extension);
            Assert.AreEqual(dU.current_file, ud.CurrentFile);
            Assert.AreEqual(dU.uploader_id, ud.UploaderId);
            Assert.AreEqual(dU.uploader_type, ud.UploaderType);
            Assert.AreEqual(dU.file_location, ud.FileLocation);
            Assert.AreEqual(dU.file_name, ud.FileName);
            //Assert.AreEqual(dU.local, ud.);

        }


        // Arrance

        // Act

        // Assert
    }
}
