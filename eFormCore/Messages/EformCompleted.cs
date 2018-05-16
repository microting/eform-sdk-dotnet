using System;

namespace eForm.Messages
{
    public class EformCompleted
    {
        public string NotificationUId { get; protected set; }
        public string MicrotringUUID { get; protected set; }

        public EformCompleted(string notificationUId, string microtringUUID)
        {
            if (string.IsNullOrEmpty(notificationUId)) throw new ArgumentNullException(nameof(notificationUId));

            NotificationUId = notificationUId;
            MicrotringUUID = microtringUUID;
        }
    }
}
