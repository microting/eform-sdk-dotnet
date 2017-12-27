using System;

namespace eForm.Messages
{
    public class EformRetrieved
    {
        public string NotificationId { get; protected set; }
        public string MicrotringUUID { get; protected set; }

        public EformRetrieved(string notificationId, string microtringUUID)
        {
            if (string.IsNullOrEmpty(notificationId)) throw new ArgumentNullException(nameof(notificationId));

            this.NotificationId = notificationId;
            this.MicrotringUUID = microtringUUID;
        }
    }
}
