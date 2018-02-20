using eForm.Messages;
using eFormSqlController;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;
using eFormShared;
using eFormCommunicator;

namespace eFormCore.Handlers
{
    public class UnitActivatedHandler : IHandleMessages<UnitActivated>
    {
        private readonly SqlController sqlController;
        private readonly Communicator communicator;
        private readonly Log log;
        private readonly Core core;

        public UnitActivatedHandler(SqlController sqlController, Communicator communicator, Log log, Core core)
        {
            this.sqlController = sqlController;
            this.communicator = communicator;
            this.log = log;
            this.core = core;
        }

#pragma warning disable 1998
        public async Task Handle(UnitActivated message)
        {
            notifications not = sqlController.NotificationCreate(message.NotificationId, message.MicrotringUUID, Constants.Notifications.UnitActivate);

            try
            {
                Unit_Dto unitDto = sqlController.UnitRead(int.Parse(message.MicrotringUUID));
                sqlController.UnitUpdate(unitDto.UnitUId, unitDto.CustomerNo, 0, unitDto.SiteUId);
                sqlController.NotificationUpdate(message.NotificationId, message.MicrotringUUID, Constants.WorkflowStates.Processed, "");
            }
            catch (Exception ex)
            {
                sqlController.NotificationUpdate(message.NotificationId, message.MicrotringUUID, Constants.WorkflowStates.NotFound, ex.Message);
                Note_Dto note_Dto = new Note_Dto(not.notification_uid, not.microting_uid, not.activity);
                core.FireHandleNotificationNotFound(note_Dto);
            }

            // Potentially send new message onto local queue
        }
    }
}
