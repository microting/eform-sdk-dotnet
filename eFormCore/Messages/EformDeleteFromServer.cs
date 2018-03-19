using System;

namespace eForm.Messages
{
    public class EformDeleteFromServer
    {
        public string MicrotringUUID { get; protected set; }

        public EformDeleteFromServer(string microtringUUID)
        {
            this.MicrotringUUID = microtringUUID;
        }
    }
}
