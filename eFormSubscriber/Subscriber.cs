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
using System;
using System.IO;
using System.Net;
using System.Threading;
using WebSocketSharp;

namespace eFormSubscriber
{
    public class Subscriber
    {
        public event EventHandler EventReply;
        public event EventHandler EventClient;

        #region var
        private WebSocket _ws;
        private bool keepConnectionAlive = true;
        private string authToken, address, token, clientId;
        private int numberOfMessages = 1;
        private object _lock = new object();
        #endregion

        #region con
        public Subscriber()
        {

        }
        #endregion

        #region public
        /// <summary>
        /// Starts a notification subscriber to Microting. Messages from the Microting and the subscriber trigger events.
        /// </summary>
        public void Start(string token, string serverAddress)
        {
            this.token = token;
            address = serverAddress;

            #region get auth token
            string html = string.Empty;
            string url = @"https://" + address + ":443/feeds/" + this.token + "/read";
            EventTriggerRequestToServer("URL  = " + url);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            authToken = Locate(html, "authToken: '", "'");
            EventTriggerRequestToServer("Auth = " + authToken);
            #endregion

            #region connect websocket
            using (var nf = new Notifier(this))
            using (var ws = new WebSocket("wss://" + address + "/faye?subscriber_id=netapp&token=" + this.token + "&host_id=netapp"))
            {
                _ws = ws;
                #region create nf events
                //ws.OnOpen += (sender, e) => ws.Send("Hi, there!");

                ws.OnMessage += (sender, e) =>
                  nf.Notify(
                    new NotificationMessage
                    {
                        Summary = "WebSocket Message",
                        Body = !e.IsPing ? e.Data : "Received a ping.",
                        Icon = "notification-message-im"
                    });

                ws.OnError += (sender, e) =>
                  nf.Notify(
                    new NotificationMessage
                    {
                        Summary = "WebSocket Error",
                        Body = e.Message,
                        Icon = "notification-message-im"
                    });

                ws.OnClose += (sender, e) =>
                  nf.Notify(
                    new NotificationMessage
                    {
                        Summary = string.Format("WebSocket Close ({0})", e.Code),
                        Body = e.Reason,
                        Icon = "notification-message-im"
                    });

                // Connect to the server.
                #endregion
                _ws.Connect();

                Thread.Sleep(25);
                SendToServer("[{\"id\":\"" + numberOfMessages + "\",\"channel\":\"/meta/handshake\",\"version\":\"1.0\",\"supportedConnectionTypes\":[\"in-process\",\"websocket\",\"long-polling\"]}]");

                #region string reply = reply from Notifier
                int runs = 0;
                string reply = nf.reply;
                while ("" == reply)
                {
                    Thread.Sleep(100);
                    reply = nf.reply;
                    runs++;
                    if (runs > 100) //after 10secs throws TimeoutException
                    {
                        throw new TimeoutException("Subscriber timed out, due to no reply to handshake.");
                    }
                }
                #endregion
                clientId = Locate(reply, "clientId\":\"", "\"");
                SendToServer("[{\"id\":\"" + numberOfMessages + "\",\"clientId\":\"" + clientId + "\",\"channel\":\"/meta/subscribe\",\"subscription\":\"" + authToken + "-update\"}]");

                Thread.Sleep(250);
                int timeout = int.Parse(Locate(reply, "\"timeout\":", "}")) - 2000;
                if (timeout < 100)
                    throw new SystemException("Timeout-2s is smaller than 0.1s. Timeout=" + timeout.ToString());

                //keeping connection alive
                while (keepConnectionAlive)
                {
                    SendToServer("[{\"id\":\"" + numberOfMessages + "\",\"clientId\":\"" + clientId + "\",\"channel\":\"/meta/connect\",\"connectionType\":\"websocket\"}]");
                    Thread.Sleep(timeout);
                }
            }
            #endregion
        }

        /// <summary>
        /// Closes the notification subscriber to Microting.
        /// </summary>
        public void Close()
        {
            keepConnectionAlive = false;
        }

        /// <summary>
        /// Informs the notification server, that message has been recieved.
        /// </summary>
        /// <param name="notificationId">Id of message that is confirmed recieved from the notification server.</param>
        public void ConfirmId(string notificationId)
        {
            string command = "[{\"channel\":\"/meta/done_msg\",\"data\":\"{\\\"token\\\":\\\"" + token + "\\\",\\\"notification_id\\\":" + notificationId + "}\",\"clientId\":\"" + clientId + "\",\"id\":\"" + numberOfMessages + "\"}]";
            SendToServer(command);
        }
        #endregion

        #region internal
        internal void EventTriggerReplyFromServer(string msg)
        {
            System.EventHandler handler = EventReply;
            if (handler != null)
            {
                handler(msg, EventArgs.Empty);
            }
        }

        internal void EventTriggerRequestToServer(string msg)
        {
            System.EventHandler handler = EventClient;
            if (handler != null)
            {
                handler(msg, EventArgs.Empty);
            }
        }
        #endregion

        #region private
        private void SendToServer(string command)
        {
            lock (_lock)
            {
                numberOfMessages++;
                EventTriggerRequestToServer(command);
                _ws.Send(command);
            }
        }
        
        private string Locate(string textStr, string startStr, string endStr)
        {
            int startIndex = textStr.IndexOf(startStr) + startStr.Length;
            int lenght = textStr.IndexOf(endStr, startIndex) - startIndex;
            return textStr.Substring(startIndex, lenght);
        }
        #endregion
    }
}
