//************************************************
// ConMan.cs
//
// The class responsible for managing a single
// connection between two computers over a single
// internet protocol.
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ConnectionManagerCS.Protocols;

namespace ConnectionManagerCS
{
    public class ConnectionManager
    {
        public delegate void Handler(byte[] messageData);

        public ConnectionManager(IConnectionProtocol protocol)
        {
            if (protocol == null) throw new ArgumentNullException();
            Protocol = protocol;
            Handlers = new Dictionary<byte, List<Queue<Message>>>();
            Task.Run(() => { ReadAllMessages(); });
        }

        private void ReadAllMessages()
        {
            while(true)
            {
                Message msg = ReadMessage();
                if(Handlers.ContainsKey(msg.JobSpecifier))
                {
                    foreach (Queue<Message> messageQueue in Handlers[msg.JobSpecifier])
                        messageQueue.Enqueue(msg);
                }
            }
        }

        private Message ReadMessage()
        {
            byte[] header = Protocol.ReadBytes(5);

            // per the framework spec: bytes 1-4 are the payload size and transaction ID
            int payloadSize = BitConverter.ToInt32(header, 1);
            // byte 1 is the transaction ID, so take bytes 2-4 as the payload size
            payloadSize = IPAddress.NetworkToHostOrder(payloadSize) & 0x00FFFFFF;
            byte[] payload = Protocol.ReadBytes(payloadSize);

            byte[] messageBytes = new byte[header.Length + payloadSize];
            header.CopyTo(messageBytes, 0);
            payload.CopyTo(messageBytes, header.Length);

            return Message.ParseNetworkBytes(messageBytes);
        }

        public void WriteMessage(Message msg)
        {
            byte[] msgBytes = msg.GetNetworkBytes();
            Protocol.WriteBytes(msgBytes);
        }

        public void Subscribe(byte jobSpecifier, Queue<Message> messageQueue)
        {
            if (!Handlers.ContainsKey(jobSpecifier))
                Handlers.Add(jobSpecifier, new List<Queue<Message>>());
            Handlers[jobSpecifier].Add(messageQueue);
        }

        public void Unsubscribe(byte jobSpecifier, Queue<Message> messageQueue)
        {
            if (!Handlers.ContainsKey(jobSpecifier))
                throw new ArgumentException("No subscription to the given job specifier exists");
            if(Handlers[jobSpecifier].Contains(messageQueue))
            {
                Handlers[jobSpecifier].Remove(messageQueue);
            }
        }

        public Connection GetConnection()
        {
            return new Connection(this);
        }

        public bool IsAlive { get { return Protocol.IsAlive(); } }

        //******************************
        // Attributes
        //******************************
        private IConnectionProtocol Protocol { get; set; }
        private Dictionary<byte, List<Queue<Message>>> Handlers { get; set; }
    }
}