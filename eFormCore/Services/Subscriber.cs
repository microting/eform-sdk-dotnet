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
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Microting.eForm.Dto;
using Microting.eForm.Infrastructure;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Data.Entities;
using Microting.eForm.Messages;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client.Exceptions;
using Rebus.Bus;

namespace Microting.eForm.Services
{
    public class Subscriber
    {
        // start var
        readonly SqlController _sqlController;
        readonly Log _log;
        private readonly IBus _bus;
        bool _isActive;
        Thread _subscriberThread;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;

        readonly Tools _t = new Tools();
        // end

        // start con
        /// <summary>
        /// Microting notification server subscriber C# DLL.
        /// </summary>
        public Subscriber(SqlController sqlController, Log log, IBus bus)
        {
            _sqlController = sqlController;
            _log = log;
            _bus = bus;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }
        // end

        // start public
        /// <summary>
        /// Starts a Notification Subscriber to Microting
        /// </summary>
        public void Start()
        {
            if (!_isActive)
            {
                _subscriberThread = new Thread(SubscriberThread);
                _subscriberThread.Start();

                int tries = 0;
                while (!_isActive)
                {
                    Thread.Sleep(100);
                    tries++;

                    if (tries > 100)
                        throw new Exception("Failed to start Subscriber after 10 secs");
                }
            }
        }

        /// <summary>
        /// Sends the close command to the Notification Subscriber
        /// </summary>
        public Task Close()
        {
            try
            {
                _cancellationTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                _log.LogException(_t.GetMethodName("Subscriber"), "failed", ex);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Tells if the Notification Subscriber to Microting is running.
        /// </summary>
        public bool IsActive()
        {
            return _isActive;
        }

        private void SubscriberThread()
        {
            var token = _sqlController.SettingRead(Settings.token).GetAwaiter().GetResult();
            if (token != "UNIT_TEST___________________L:32" && token != "abc1234567890abc1234567890abcdef")
            {
                _log.LogStandard(_t.GetMethodName("Subscriber"), $"{DateTime.UtcNow.ToString()} - Starting up");
                _isActive = true;

                string awsAccessKeyId = _sqlController.SettingRead(Settings.awsAccessKeyId).Result;
                string awsSecretAccessKey = _sqlController.SettingRead(Settings.awsSecretAccessKey).Result;
                string awsQueueUrl = _sqlController.SettingRead(Settings.awsEndPoint).Result +
                                     _sqlController.SettingRead(Settings.token).Result;

                var sqsClient = new AmazonSQSClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.EUCentral1);
                DateTime lastException = DateTime.MinValue;

                while (!_cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var res = sqsClient.ReceiveMessageAsync(awsQueueUrl, _cancellationToken).GetAwaiter()
                            .GetResult();

                        if (res.Messages.Count > 0)
                            foreach (var message in res.Messages)
                            {
                                var parsedData = JToken.Parse(message.Body);
                                string notificationUId = parsedData["id"]!.ToString();
                                int microtingUId = int.Parse(parsedData["microting_uuid"]!.ToString());
                                string action = parsedData["text"]!.ToString();

                                _log.LogStandard(_t.GetMethodName("Subscriber"),
                                    "Notification notificationUId : " + notificationUId + " microtingUId : " +
                                    microtingUId + " action : " + action);
                                switch (action)
                                {
                                    case Constants.Notifications.Completed:
                                        _sqlController.NotificationCreate(notificationUId, microtingUId,
                                            Constants.Notifications.Completed).GetAwaiter().GetResult();
                                        _bus.SendLocal(new EformCompleted(notificationUId, microtingUId)).GetAwaiter()
                                            .GetResult();
                                        break;
                                    case Constants.Notifications.EformParsedByServer:
                                        _bus.SendLocal(new EformParsedByServer(notificationUId, microtingUId))
                                            .GetAwaiter().GetResult();
                                        break;
                                    case Constants.Notifications.EformParsingError:
                                        _bus.SendLocal(new EformParsingError(notificationUId, microtingUId))
                                            .GetAwaiter().GetResult();
                                        break;
                                    case Constants.Notifications.RetrievedForm:
                                        _sqlController.NotificationCreate(notificationUId, microtingUId,
                                            Constants.Notifications.RetrievedForm).GetAwaiter().GetResult();
                                        _bus.SendLocal(new EformRetrieved(notificationUId, microtingUId)).GetAwaiter()
                                            .GetResult();
                                        break;
                                    case Constants.Notifications.UnitActivate:
                                        _sqlController.NotificationCreate(notificationUId, microtingUId,
                                            Constants.Notifications.UnitActivate).GetAwaiter().GetResult();
                                        _bus.SendLocal(new UnitActivated(notificationUId, microtingUId)).GetAwaiter()
                                            .GetResult();
                                        break;
                                    case Constants.Notifications.SpeechToTextCompleted:
                                        _sqlController.NotificationCreate(notificationUId, microtingUId,
                                            Constants.Notifications.SpeechToTextCompleted).GetAwaiter().GetResult();
                                        _bus.SendLocal(new TranscriptionCompleted(notificationUId, microtingUId))
                                            .GetAwaiter().GetResult();
                                        break;
                                    case Constants.Notifications.InSightAnswerDone:
                                        _sqlController.NotificationCreate(notificationUId, microtingUId,
                                            Constants.Notifications.InSightAnswerDone).GetAwaiter().GetResult();
                                        _bus.SendLocal(new AnswerCompleted(notificationUId, microtingUId)).GetAwaiter()
                                            .GetResult();
                                        break;
                                    case Constants.Notifications.InSightSurveyConfigurationChanged:
                                    case Constants.Notifications.InSightSurveyConfigurationCreated:
                                        _sqlController.NotificationCreate(notificationUId, microtingUId,
                                                Constants.Notifications.InSightSurveyConfigurationChanged).GetAwaiter()
                                            .GetResult();
                                        _bus.SendLocal(new SurveyConfigurationChanged(notificationUId, microtingUId))
                                            .GetAwaiter().GetResult();
                                        break;
                                }

                                sqsClient.DeleteMessageAsync(awsQueueUrl, message.ReceiptHandle, _cancellationToken)
                                    .GetAwaiter().GetResult();
                            }
                        else
                        {
                            _log.LogStandard(_t.GetMethodName("Subscriber"),
                                $"{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)} -  No messages for us right now!");
                        }
                    }
                    catch (WebException webException)
                    {
                        _log.LogWarning(_t.GetMethodName("Subscriber"),
                            _t.PrintException(_t.GetMethodName("Subscriber") + " failed", webException));
                        // We try to sleep 20 seconds to see if the problem goes away by it self.
                        Thread.Sleep(20000);
                    }

                    catch (Exception ex)
                    {
                        _log.LogWarning(_t.GetMethodName("Subscriber"),
                            _t.PrintException(_t.GetMethodName("Subscriber") + " failed", ex));

                        if (DateTime.Compare(lastException.AddMinutes(5), DateTime.UtcNow) > 0)
                        {
                            _log.LogException(_t.GetMethodName("Subscriber"), "failed, twice in the last 5 minuts", ex);
                            // TODO handle crash so we could restart!!!
                            throw;
                        }

                        lastException = DateTime.UtcNow;
                    }
                }

                _log.LogStandard(_t.GetMethodName("Subscriber"), "--- WE WHERE TOLD NOT TO CONTINUE TO SUBSCRIBE ---");
                sqsClient.Dispose();
                _isActive = false;
            }
            // end
            else
                // start unit test
            {
                _log.LogStandard(_t.GetMethodName("Subscriber"), "Subscriber faked");
                _isActive = true;

                while (!_cancellationToken.IsCancellationRequested)
                {
                    _log.LogStandard(_t.GetMethodName("Subscriber"),
                        $"{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)} -  No messages for us right now!");
                    Thread.Sleep(1000);
                }

                _isActive = false;
            }
        }
    }
}