using eForm.Messages;
using eFormSqlController;
using Rebus.Handlers;
using System;
using eFormShared;
using System.Threading.Tasks;

namespace eFormCore.Handlers
{
    public class EformCompletedHandler : IHandleMessages<EformCompleted>
    {
        private readonly SqlController sqlController;

        public EformCompletedHandler(SqlController sqlController)
        {
            this.sqlController = sqlController;
        }

        #pragma warning disable 1998
        public async Task Handle(EformCompleted message)
        {
            sqlController.NotificationCreate(message.NotificationId, message.MicrotringUUID, Constants.Notifications.Completed);

            // Potentially send new message onto local queue
        }
    }
}
