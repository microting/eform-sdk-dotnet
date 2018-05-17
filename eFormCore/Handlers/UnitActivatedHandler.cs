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
        Tools t = new Tools();

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
            try
            {
                Unit_Dto unitDto = sqlController.UnitRead(int.Parse(message.MicrotringUUID));
                sqlController.UnitUpdate(unitDto.UnitUId, unitDto.CustomerNo, 0, unitDto.SiteUId);
                sqlController.NotificationUpdate(message.notificationUId, message.MicrotringUUID, Constants.WorkflowStates.Processed, "", "");

                log.LogStandard(t.GetMethodName("UnitActivatedHandler"), "Unit with id " + message.MicrotringUUID + " has been activated");

                Note_Dto note_Dto = new Note_Dto(message.notificationUId, message.MicrotringUUID, Constants.WorkflowStates.Processed);
                core.FireHandleSiteActivated(note_Dto);
            }
            catch (Exception ex)
            {
                sqlController.NotificationUpdate(message.notificationUId, message.MicrotringUUID, Constants.WorkflowStates.NotFound, ex.Message, ex.StackTrace.ToString());
                Note_Dto note_Dto = new Note_Dto(message.notificationUId, message.MicrotringUUID, Constants.WorkflowStates.NotFound);
                core.FireHandleNotificationNotFound(note_Dto);
            }
        }
    }
}
