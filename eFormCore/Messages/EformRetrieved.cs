using System;

namespace eForm.Messages
{
    public class EformRetrieved
    {
        public string notificationUId { get; protected set; }
        public string MicrotringUUID { get; protected set; }

        public EformRetrieved(string notificationUId, string microtringUUID)
        {
            if (string.IsNullOrEmpty(notificationUId)) throw new ArgumentNullException(nameof(notificationUId));

            this.notificationUId = notificationUId;
            this.MicrotringUUID = microtringUUID;
        }
    }
}
