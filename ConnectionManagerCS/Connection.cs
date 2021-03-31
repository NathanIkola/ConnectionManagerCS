using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionManagerCS
{
    public class Connection
    {
        public Connection(ConnectionManager manager)
        {
            if (manager == null) throw new ArgumentNullException();
            MessageQueue = new Queue<Message>();
            Manager = manager;
        }

        public void WriteMessage(Message msg)
        {
            Manager.WriteMessage(msg);
        }

        public Message ReadMessage()
        {
            while (MessageQueue.Count == 0) { }
            return MessageQueue.Dequeue();
        }

        public void SubscribeToAll()
        {
            for (int jobSpecifier = 0; jobSpecifier < 256; ++jobSpecifier)
                Subscribe((byte)jobSpecifier);
        }

        public void Subscribe(byte jobSpecifier)
        {
            Manager.Subscribe(jobSpecifier, MessageQueue);
        }

        public void Unsubscribe(byte jobSpecifier)
        {
            Manager.Unsubscribe(jobSpecifier, MessageQueue);
        }

        private Queue<Message> MessageQueue { get; set; }
        public ConnectionManager Manager { get; private set; }
        public int PendingMessages { get { return MessageQueue.Count; } }
    }
}
