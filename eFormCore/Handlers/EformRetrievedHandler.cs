using eForm.Messages;
using eFormSqlController;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;
using eFormShared;
using eFormCommunicator;

namespace eFormCore.Handlers
{
    public class EformRetrievedHandler : IHandleMessages<EformRetrieved>
    {
        private readonly SqlController sqlController;
        private readonly Communicator communicator;
        private readonly Log log;
        private readonly Core core;
        Tools t = new Tools();

        public EformRetrievedHandler(SqlController sqlController, Communicator communicator, Log log, Core core)
        {
            this.sqlController = sqlController;
            this.communicator = communicator;
            this.log = log;
            this.core = core;
        }

#pragma warning disable 1998
        public async Task Handle(EformRetrieved message)
        {
            try
            {
                sqlController.CaseUpdateRetrived(message.MicrotringUUID);
                
                sqlController.NotificationUpdate(message.notificationUId, message.MicrotringUUID, Constants.WorkflowStates.Processed, "", "");

                Case_Dto cDto = sqlController.CaseReadByMUId(message.MicrotringUUID);
                log.LogStandard(t.GetMethodName("EformRetrievedHandler"), cDto.ToString() + " has been retrived");
                core.FireHandleCaseRetrived(cDto);
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
