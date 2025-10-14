using System;

namespace Microting.eForm.Messages;

public class SurveyConfigurationChanged
{
    public string NotificationUId { get; protected set; }
    public int MicrotringUUID { get; protected set; }

    public SurveyConfigurationChanged(string notificationUId, int microtringUUID)
    {
        if (string.IsNullOrEmpty(notificationUId)) throw new ArgumentNullException(nameof(notificationUId));

        NotificationUId = notificationUId;
        MicrotringUUID = microtringUUID;
    }
}