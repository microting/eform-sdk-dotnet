using eForm.Messages;
using eFormSqlController;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;
using eFormShared;
using eFormCommunicator;

namespace eFormCore.Handlers
{
    public class TranscriptionCompletedHandler : IHandleMessages<TranscriptionCompleted>
    {
        private readonly SqlController sqlController;
        private readonly Communicator communicator;
        private readonly Log log;
        private readonly Core core;
        Tools t = new Tools();

        public TranscriptionCompletedHandler(SqlController sqlController, Communicator communicator, Log log, Core core)
        {
            this.sqlController = sqlController;
            this.communicator = communicator;
            this.log = log;
            this.core = core;
        }

#pragma warning disable 1998
        public async Task Handle(TranscriptionCompleted message)
        {
            try
            {
                field_values fv = sqlController.GetFieldValueByTranscriptionId(int.Parse(message.MicrotringUUID));
                string result = communicator.SpeechToText(int.Parse(message.MicrotringUUID));

                sqlController.FieldValueUpdate((int)fv.case_id, (int)fv.field_id, result);
                sqlController.NotificationUpdate(message.notificationUId, message.MicrotringUUID, Constants.WorkflowStates.Processed, "", "");

                log.LogStandard(t.GetMethodName("TranscriptionCompletedHandler"), "Transcription with id " + message.MicrotringUUID + " has been transcribed");
            }
            catch (Exception ex)
            {
                sqlController.NotificationUpdate(message.notificationUId, message.MicrotringUUID, Constants.WorkflowStates.NotFound, ex.Message, ex.StackTrace.ToString());
            }
        }
    }
}
