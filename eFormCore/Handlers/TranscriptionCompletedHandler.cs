using eForm.Messages;
using eFormSqlController;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;
using eFormShared;
using eFormCommunicator;
using Newtonsoft.Json.Linq;

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
                JToken result = communicator.SpeechToText(int.Parse(message.MicrotringUUID));

                sqlController.FieldValueUpdate((int)fv.case_id, (int)fv.id, result["text"].ToString());

                #region download file
                uploaded_data ud = sqlController.GetUploaded_DataByTranscriptionId(int.Parse(message.MicrotringUUID));

                if (ud.file_name.Contains("3gp"))
                {
                    log.LogStandard(t.GetMethodName("TranscriptionCompletedHandler"), "file_name contains 3gp");
                    string urlStr = sqlController.SettingRead(Settings.comSpeechToText) + "";
                    string fileLocationPicture = sqlController.SettingRead(Settings.fileLocationPicture);
                    using (var client = new System.Net.WebClient())
                    {
                        try
                        {
                            log.LogStandard(t.GetMethodName("TranscriptionCompletedHandler"), "Trying to donwload file from : " + urlStr);
                            client.DownloadFile(urlStr, fileLocationPicture + ud.file_name.Replace(".3gp", ".wav"));
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Downloading and creating fil locally failed.", ex);
                        }
                    }
                }
                #endregion

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
