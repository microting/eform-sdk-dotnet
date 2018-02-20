using eForm.Messages;
using eFormSqlController;
using Rebus.Handlers;
using System;
using eFormShared;
using System.Threading.Tasks;

namespace eFormCore.Handlers
{
    public class EformParsedByServerHandler : IHandleMessages<EformParsedByServer>
    {
        private readonly SqlController sqlController;

        public EformParsedByServerHandler(SqlController sqlController)
        {
            this.sqlController = sqlController;
        }

        #pragma warning disable 1998
        public async Task Handle(EformParsedByServer message)
        {
            sqlController.NotificationCreate(message.NotificationId, message.MicrotringUUID, Constants.Notifications.EformParsedByServer);

            // Potentially send new message onto local queue
        }
    }
}
