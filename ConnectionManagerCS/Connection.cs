//************************************************
// Connection.cs
//
// A logical connection between two machines. In
// reality it is simply a conduit by which the
// ConnectionManager that spawned it is able to
// pass network traffic around to parts of the
// application.
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System;
using System.Collections.Generic;

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

        public void PassMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("Message was null");
            MessageQueue.Enqueue(message);
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
