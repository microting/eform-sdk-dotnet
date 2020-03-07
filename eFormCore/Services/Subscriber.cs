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
using Rebus.Bus;

namespace Microting.eForm.Services
{
    public class Subscriber
    {
        #region var
        SqlController sqlController;
        Log log;
        private readonly IBus bus;
        bool keepSubscribed;
        bool isActive;
        Thread subscriberThread;
        Tools t = new Tools();
        #endregion

        #region con
        /// <summary>
        /// Microting notification server subscriber C# DLL.
        /// </summary>
        public Subscriber(SqlController sqlController, Log log, IBus bus)
        {
            this.sqlController = sqlController;
            this.log = log;
            this.bus = bus;
        }
        #endregion

        #region public
        /// <summary>
        /// Starts a Notification Subscriber to Microting
        /// </summary>
        public void Start()
        {
            if (!isActive)
            {
                subscriberThread = new Thread(() => SubscriberThread());
                subscriberThread.Start();

                int tries = 0;
                while (!isActive)
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
        public async Task Close()
        {
            try
            {
                keepSubscribed = false;
                int tries = 0;

                while (isActive)
                {
                    if (tries > 100) //25 secs
                        subscriberThread.Abort();

                    Thread.Sleep(250);
                    tries++;
                }
            }
            catch (Exception ex)
            {
                await log.LogException(t.GetMethodName("Subscriber"), "failed", ex);
            }
        }

        /// <summary>
        /// Tells if the Notification Subscriber to Microting is running.
        /// </summary>
        public bool IsActive()
        {
            return isActive;
        }
        #endregion

        #region private
#pragma warning disable 4014
        private void SubscriberThread()
        {
            if (sqlController.SettingRead(Settings.token).Result != "UNIT_TEST___________________L:32")
            #region amazon
            {
                
                log.LogStandard(t.GetMethodName("Subscriber"), $"{DateTime.Now.ToString()} - Starting up");

                #region setup
                isActive = true;
                keepSubscribed = true;

                string awsAccessKeyId = sqlController.SettingRead(Settings.awsAccessKeyId).Result;
                string awsSecretAccessKey = sqlController.SettingRead(Settings.awsSecretAccessKey).Result;
                string awsQueueUrl = sqlController.SettingRead(Settings.awsEndPoint).Result + sqlController.SettingRead(Settings.token).Result;

                var sqsClient = new AmazonSQSClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.EUCentral1);
                DateTime lastException = DateTime.MinValue;
                #endregion

                while (keepSubscribed)
                {
                    try
                    {
                        var res = sqsClient.ReceiveMessageAsync(awsQueueUrl).Result;

                        if (res.Messages.Count > 0)
                            foreach (var message in res.Messages)
                            {
                                #region JSON -> var

                                var parsedData = JRaw.Parse(message.Body);
                                string notificationUId = parsedData["id"].ToString();
                                int microtingUId = int.Parse(parsedData["microting_uuid"].ToString());
                                string action = parsedData["text"].ToString();

                                #endregion

                                log.LogStandard(t.GetMethodName("Subscriber"),
                                    "Notification notificationUId : " + notificationUId + " microtingUId : " +
                                    microtingUId + " action : " + action);
                                notifications result;
                                switch (action)
                                {
                                    case Constants.Notifications.Completed:
                                        result = sqlController.NotificationCreate(notificationUId, microtingUId,
                                                Constants.Notifications.Completed).Result;
                                            bus.SendLocal(new EformCompleted(notificationUId, microtingUId));
                                        break;
                                    case Constants.Notifications.EformParsedByServer:
                                        bus.SendLocal(new EformParsedByServer(notificationUId, microtingUId));
                                        break;
                                    case Constants.Notifications.EformParsingError:
                                        bus.SendLocal(new EformParsingError(notificationUId, microtingUId));
                                        break;
                                    case Constants.Notifications.RetrievedForm:
                                        result = sqlController.NotificationCreate(notificationUId, microtingUId,
                                            Constants.Notifications.RetrievedForm).Result;
                                        bus.SendLocal(new EformRetrieved(notificationUId, microtingUId));
                                        break;
                                    case Constants.Notifications.UnitActivate:
                                        result = sqlController.NotificationCreate(notificationUId, microtingUId,
                                            Constants.Notifications.UnitActivate).Result;
                                        bus.SendLocal(new UnitActivated(notificationUId, microtingUId));
                                        break;
                                    case Constants.Notifications.SpeechToTextCompleted:
                                        result = sqlController.NotificationCreate(notificationUId, microtingUId,
                                            Constants.Notifications.SpeechToTextCompleted).Result;
                                        bus.SendLocal(new TranscriptionCompleted(notificationUId, microtingUId));
                                        break;
                                    case Constants.Notifications.InSightAnswerDone:
                                        result = sqlController.NotificationCreate(notificationUId, microtingUId,
                                            Constants.Notifications.InSightAnswerDone).Result;
                                        bus.SendLocal(new AnswerCompleted(notificationUId, microtingUId));
                                        break;
                                }

                                sqsClient.DeleteMessageAsync(awsQueueUrl, message.ReceiptHandle);
                            }
                        else
                        {
                            log.LogStandard(t.GetMethodName("Subscriber"),
                                $"{DateTime.Now.ToString()} -  No messages for us right now!");
                        }

                    }
                    catch (WebException webException)
                    {
                        log.LogWarning(t.GetMethodName("Subscriber"), t.PrintException(t.GetMethodName("Subscriber") + " failed", webException));
                        // We try to sleep 20 seconds to see if the problem goes away by it self.
                        Thread.Sleep(20000);
                    }
                    
                    catch (Exception ex)
                    {
                        // Log exception
                        log.LogWarning(t.GetMethodName("Subscriber"), t.PrintException(t.GetMethodName("Subscriber") + " failed", ex));

                        if (DateTime.Compare(lastException.AddMinutes(5), DateTime.Now) > 0)
                        {
                            keepSubscribed = false;
                            log.LogException(t.GetMethodName("Subscriber"), "failed, twice in the last 5 minuts", ex);
                            // TODO handle crash so we could restart!!!
                        }

                        lastException = DateTime.Now;
                    }
                }
                log.LogStandard(t.GetMethodName("Subscriber"), "--- WE WHERE TOLD NOT TO CONTINUE TO SUBSCRIBE ---");
                sqsClient.Dispose();
                //EventMsgClient("Subscriber closed", null);
                keepSubscribed = false;
                isActive = false;
            }
            #endregion
            else
            #region unit test
            {
                log.LogStandard(t.GetMethodName("Subscriber"), "Subscriber faked").RunSynchronously();
                isActive = true;
                keepSubscribed = true;

                while (keepSubscribed)
                    Thread.Sleep(100);

                keepSubscribed = false;
                isActive = false;
            }
            #endregion
        }
#pragma warning restore 4014
        #endregion
    }
}