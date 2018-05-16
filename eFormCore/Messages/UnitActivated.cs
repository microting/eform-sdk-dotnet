using System;

namespace eForm.Messages
{
    public class UnitActivated
    {
        public string notificationUId { get; protected set; }
        public string MicrotringUUID { get; protected set; }

        public UnitActivated(string notificationUId, string microtringUUID)
        {
            if (string.IsNullOrEmpty(notificationUId)) throw new ArgumentNullException(nameof(notificationUId));

            this.notificationUId = notificationUId;
            this.MicrotringUUID = microtringUUID;
        }
    }
}
