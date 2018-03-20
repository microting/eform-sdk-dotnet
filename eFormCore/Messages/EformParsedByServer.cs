using System;

namespace eForm.Messages
{
    public class EformParsedByServer
    {
        public string NotificationId { get; protected set; }
        public string MicrotringUUID { get; protected set; }

        public EformParsedByServer(string notificationId, string microtringUUID)
        {
            if (string.IsNullOrEmpty(notificationId)) throw new ArgumentNullException(nameof(notificationId));

            NotificationId = notificationId;
            MicrotringUUID = microtringUUID;
        }
    }
}
