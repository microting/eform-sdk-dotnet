using eForm.Messages;
using eFormSqlController;
using Rebus.Handlers;
using System;
using eFormShared;
using System.Threading.Tasks;

namespace eFormCore.Handlers
{
    public class EformParsingErrorHandler : IHandleMessages<EformParsingError>
    {
        private readonly SqlController sqlController;

        public EformParsingErrorHandler(SqlController sqlController)
        {
            this.sqlController = sqlController;
        }

        #pragma warning disable 1998
        public async Task Handle(EformParsingError message)
        {
            sqlController.NotificationCreate(message.NotificationId, message.MicrotringUUID, Constants.Notifications.EformParsingError);

            // Potentially send new message onto local queue
        }
    }
}
