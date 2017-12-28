using eForm.Messages;
using eFormSqlController;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;
using eFormShared;

namespace eFormCore.Handlers
{
    public class EformRetrievedHandler : IHandleMessages<EformRetrieved>
    {
        private readonly SqlController sqlController;

        public EformRetrievedHandler(SqlController sqlController)
        {
            this.sqlController = sqlController;
        }

        public async Task Handle(EformRetrieved message)
        {
            sqlController.NotificationCreate(message.NotificationId, message.MicrotringUUID, Constants.Notifications.RetrievedForm);

            // Potentially send new message onto local queue
        }
    }
}
