using System;

namespace eForm.Messages
{
    public class EformCompleted
    {
        public string NotificationId { get; protected set; }
        public string MicrotringUUID { get; protected set; }

        public EformCompleted(string notificationId, string microtringUUID)
        {
            if (string.IsNullOrEmpty(notificationId)) throw new ArgumentNullException(nameof(notificationId));

            this.NotificationId = notificationId;
            this.MicrotringUUID = microtringUUID;
        }
    }
}
