using System;
using System.IO;
using System.Net;
using System.Threading;
using WebSocketSharp;

namespace eFormSubscriber
{
    public class Subscriber
    {
        #region var
        public event EventHandler EventReply;
        public event EventHandler EventClient;

        private WebSocket _ws;
        private bool keepConnectionAlive = true;
        private string _authToken, _address, _token, _clientId;
        private int _numberOfMessages = 1;
        private object _lock = new object();
        #endregion

        #region con
        public Subscriber(string address, string token)
        {
            _address = address;
            _token = token;
        }
        #endregion

        #region public
        public void Start()
        {
            #region get auth token
            string html = string.Empty;
            string url = @"https://" + _address + ":443/feeds/" + _token + "/read";
            EventTriggerRequestToServer("URL  = " + url);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            _authToken = Locate(html, "authToken: '", "'");
            EventTriggerRequestToServer("Auth = " + _authToken);
            #endregion

            #region connect websocket
            using (var nf = new Notifier(this))
            using (var ws = new WebSocket("wss://" + _address + "/faye?subscriber_id=netapp&token=" + _token + "&host_id=netapp"))
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
                        Summary = String.Format("WebSocket Close ({0})", e.Code),
                        Body = e.Reason,
                        Icon = "notification-message-im"
                    });

                // Connect to the server.
                #endregion
                _ws.Connect();

                Thread.Sleep(25);
                SendToServer("[{\"id\":\"" + _numberOfMessages + "\",\"channel\":\"/meta/handshake\",\"version\":\"1.0\",\"supportedConnectionTypes\":[\"in-process\",\"websocket\",\"long-polling\"]}]");

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
                _clientId = Locate(reply, "clientId\":\"", "\"");
                SendToServer("[{\"id\":\"" + _numberOfMessages + "\",\"clientId\":\"" + _clientId + "\",\"channel\":\"/meta/subscribe\",\"subscription\":\"" + _authToken + "-update\"}]");

                Thread.Sleep(250);
                int timeout = int.Parse(Locate(reply, "\"timeout\":", "}")) - 2000;
                if (timeout < 100)
                    throw new SystemException("Timeout-2s is smaller than 0.1s. Timeout=" + timeout.ToString());

                //keeping connection alive
                while (keepConnectionAlive)
                {
                    SendToServer("[{\"id\":\"" + _numberOfMessages + "\",\"clientId\":\"" + _clientId + "\",\"channel\":\"/meta/connect\",\"connectionType\":\"websocket\"}]");
                    Thread.Sleep(timeout);
                }
            }
            #endregion
        }

        public void Close()
        {
            keepConnectionAlive = false;
        }

        public void ConfirmId(string notificationId)
        {
            string command = "[{\"channel\":\"/meta/done_msg\",\"data\":\"{\\\"token\\\":\\\"" + _token + "\\\",\\\"notification_id\\\":" + notificationId + "}\",\"clientId\":\"" + _clientId + "\",\"id\":\"" + _numberOfMessages + "\"}]";
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
                _numberOfMessages++;
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
