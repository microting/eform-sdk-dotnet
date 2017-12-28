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
        public void NotificationCreate_NewNotification_DoesStoreNotification()
        {
            // Arrance
            var notificationId = Guid.NewGuid().ToString();
            var microtingUId = Guid.NewGuid().ToString();

            // Act
            sut.NotificationCreate(notificationId, microtingUId, Constants.Notifications.RetrievedForm);

            // Assert
            var notification = DbContext.notifications.SingleOrDefault(x => x.notification_uid == notificationId && x.microting_uid == microtingUId);

            Assert.NotNull(notification);
        }
    }
}
