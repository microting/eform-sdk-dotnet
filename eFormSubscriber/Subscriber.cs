/*
The MIT License (MIT)

Copyright (c) 2014 microting

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

using eFormShared;
using eFormSqlController;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.Runtime;
using Amazon;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace eFormSubscriber
{
    public class Subscriber
    {
        #region var
        SqlController sqlController;
        bool keepSubscribed;
        bool isActive;
        Thread subscriberThread;
        Tools t = new Tools();
        #endregion

        #region con
        /// <summary>
        /// Microting notification server subscriber C# DLL.
        /// </summary>
        /// <param name="token">Your company's notification server access token.</param>
        /// <param name="address">Microting's notification server address.</param>
        /// <param name="name">Your name, as shown on the notification server.</param>
        public Subscriber(SqlController sqlController)
        {
            this.sqlController = sqlController;
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
                subscriberThread = new Thread(() => SubriberThread());
                subscriberThread.Start();
            }
        }

        /// <summary>
        /// Sends the close command to the Notification Subscriber
        /// </summary>
        public void Close()
        {
            try
            {
                keepSubscribed = false;
                int tries = 0;

                while (isActive)
                {
                    if (tries > 200) //50 secs
                        subscriberThread.Abort();

                    Thread.Sleep(250);
                    tries++;
                }
            }
            catch
            {

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
        private void SubriberThread()
        {
            #region setup
            isActive = true;
            keepSubscribed = true;

            string awsAccessKeyId = sqlController.SettingRead(Settings.awsAccessKeyId);
            string awsSecretAccessKey = sqlController.SettingRead(Settings.awsSecretAccessKey);
            string awsQueueUrl = sqlController.SettingRead(Settings.awsEndPoint) + sqlController.SettingRead(Settings.token);
            
            var sqsClient = new AmazonSQSClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.EUCentral1);
            DateTime lastExpection = DateTime.MinValue;
            DateTime lastCheckAdd15s;
            #endregion

            while (keepSubscribed)
            {
                try
                {
                    lastCheckAdd15s = DateTime.Now.AddSeconds(15);
                    var res = sqsClient.ReceiveMessage(awsQueueUrl);

                    if (res.Messages.Count > 0)
                        foreach (var message in res.Messages)
                        {
                            #region JSON -> var
                            var parsedData = JRaw.Parse(message.Body);
                            string notificationUId = parsedData["id"].ToString();
                            string microtingUId = parsedData["microting_uuid"].ToString();
                            string action = parsedData["text"].ToString();
                            #endregion
                            sqlController.NotificationCreate(notificationUId, microtingUId, action);

                            sqsClient.DeleteMessage(awsQueueUrl, message.ReceiptHandle);
                        }
                    else
                        while (lastCheckAdd15s > DateTime.Now)
                            Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    //Log expection

                    if (DateTime.Compare(lastExpection.AddMinutes(10), DateTime.Now) > 0)
                        keepSubscribed = false; //TODO //throw new Exception(t.GetMethodName() + " failed, more than twice in the last 10 minuts", ex);

                    lastExpection = DateTime.Now;
                }
            }

            #region clean up
            //EventMsgClient("Subscriber closed", null);
            keepSubscribed = false;
            isActive = false;
            #endregion
        }
        #endregion
    }
}
