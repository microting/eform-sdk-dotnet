using eForm.Messages;
using eFormSqlController;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;
using eFormShared;

namespace eFormCore.Handlers
{
    public class UnitActivatedHandler : IHandleMessages<UnitActivated>
    {
        private readonly SqlController sqlController;

        public UnitActivatedHandler(SqlController sqlController)
        {
            this.sqlController = sqlController;
        }

        #pragma warning disable 1998
        public async Task Handle(UnitActivated message)
        {
            sqlController.NotificationCreate(message.NotificationId, message.MicrotringUUID, Constants.Notifications.UnitActivate);

            // Potentially send new message onto local queue
        }
    }
}
