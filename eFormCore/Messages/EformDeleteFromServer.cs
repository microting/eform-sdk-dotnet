using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microting.eForm.Messages
{
    class EformDeleteFromServer
    {
        public string MicrotringUUID { get; protected set; }

        public EformDeleteFromServer( string microtringUUID)
        {
            if (string.IsNullOrEmpty(microtringUUID)) throw new ArgumentNullException(nameof(microtringUUID));

            MicrotringUUID = microtringUUID;
        }
    }
}
