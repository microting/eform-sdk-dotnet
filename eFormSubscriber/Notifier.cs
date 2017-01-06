using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace eFormSubscriber
{
    internal class Notifier : IDisposable
    {
        #region var
        public string reply = "";

        private volatile bool              _enabled;
        private Queue<NotificationMessage> _queue;
        private object                     _sync;
        private ManualResetEvent           _waitHandle;
        #endregion

        #region public
        public Notifier(Subscriber subscriber)
        {
            _enabled = true;
            _queue = new Queue<NotificationMessage>();
            _sync = ((ICollection)_queue).SyncRoot;
            _waitHandle = new ManualResetEvent(false);

            ThreadPool.QueueUserWorkItem(
            state => {
                while (_enabled || Count > 0)
                {
                    var msg = dequeue();
                    if (msg != null)
                    {
                        if (msg.Summary == "WebSocket Message")
                            reply = msg.Body;
                        else
                            reply = msg.ToString();

                        subscriber.ReplyFromServer(reply);
                    }
                    else {
                        Thread.Sleep (100);
                    }
                }

            _waitHandle.Set ();
            }
            );
        }

        public int Count {
            get {
                lock (_sync)
                    return _queue.Count;
            }
        }

        public void Close ()
        {
            _enabled = false;
            _waitHandle.WaitOne ();
            _waitHandle.Close ();
        }

        public void Notify (NotificationMessage message)
        {
            lock (_sync)
                if (_enabled)
                    _queue.Enqueue (message);
        }
        
        void IDisposable.Dispose ()
        {
            Close ();
        }
        #endregion

        #region private
        private NotificationMessage dequeue()
        {
            lock (_sync)
                return _queue.Count > 0 ? _queue.Dequeue() : null;
        }
        #endregion
    }
}
