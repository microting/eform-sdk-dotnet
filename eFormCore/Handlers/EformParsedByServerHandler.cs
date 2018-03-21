using eForm.Messages;
using eFormSqlController;
using Rebus.Handlers;
using System;
using eFormShared;
using System.Threading.Tasks;
using eFormCommunicator;

namespace eFormCore.Handlers
{
    public class EformParsedByServerHandler : IHandleMessages<EformParsedByServer>
    {
        private readonly SqlController sqlController;
        private readonly Communicator communicator;
        private readonly Log log;
        private readonly Core core;

        public EformParsedByServerHandler(SqlController sqlController, Communicator communicator, Log log, Core core)
        {
            this.sqlController = sqlController;
            this.communicator = communicator;
            this.log = log;
            this.core = core;
        }

#pragma warning disable 1998
        public async Task Handle(EformParsedByServer message)
        {
            sqlController.NotificationCreate(message.NotificationId, message.MicrotringUUID, Constants.Notifications.EformParsedByServer);

            // Potentially send new message onto local queue
        }
    }
}
