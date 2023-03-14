/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using eFormCore;
using Microting.eForm.Communication;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Messages;
using Newtonsoft.Json.Linq;
using Rebus.Handlers;

namespace Microting.eForm.Handlers
{
    public class TranscriptionCompletedHandler : IHandleMessages<TranscriptionCompleted>
    {
        private readonly SqlController sqlController;
        private readonly Communicator communicator;
        private readonly Log log;
        private readonly Core core;

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
                FieldValue fv = await sqlController.GetFieldValueByTranscriptionId(message.MicrotringUUID);
                JToken result = await communicator.SpeechToText(message.MicrotringUUID);

                await sqlController.FieldValueUpdate((int)fv.CaseId, fv.Id, result["text"].ToString());

                #region download file

                UploadedData ud = await sqlController.GetUploaded_DataByTranscriptionId(message.MicrotringUUID);

                if (ud.FileName.Contains("3gp"))
                {
                    log.LogStandard("TranscriptionCompletedHandler.Handle", "file_name contains 3gp");
                    string urlStr = sqlController.SettingRead(Settings.comSpeechToText).GetAwaiter().GetResult() +
                                    "/download_file/" + message.MicrotringUUID + ".wav?token=" +
                                    sqlController.SettingRead(Settings.token).GetAwaiter().GetResult();
                    using var client = new HttpClient();
                    try
                    {
                        log.LogStandard("TranscriptionCompletedHandler.Handle",
                            "Trying to download file from : " + urlStr);
                        var stream = await client.GetStreamAsync(urlStr);
                        MemoryStream baseMemoryStream = new MemoryStream();
                        await stream.CopyToAsync(baseMemoryStream);
                        await stream.DisposeAsync();
                        stream.Close();
                        await core.PutFileToS3Storage(baseMemoryStream, ud.FileName.Replace(".3gp", ".wav"));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Downloading and creating fil locally failed.", ex);
                    }
                }

                #endregion

                await sqlController.NotificationUpdate(message.notificationUId, message.MicrotringUUID,
                    Constants.WorkflowStates.Processed, "", "");

                log.LogStandard("TranscriptionCompletedHandler.Handle",
                    "Transcription with id " + message.MicrotringUUID + " has been transcribed");
            }
            catch (Exception ex)
            {
                await sqlController.NotificationUpdate(message.notificationUId, message.MicrotringUUID,
                    Constants.WorkflowStates.NotFound, ex.Message, ex.StackTrace);
            }
        }
    }
}