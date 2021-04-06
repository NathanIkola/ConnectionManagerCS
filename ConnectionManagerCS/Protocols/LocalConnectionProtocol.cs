//************************************************
// LocalConnectionProtocol.cs
//
// Simply echoes bytes written with WriteBytes to
// bytes read with ReadBytes. Used by an 
// application to communicate to itself locally as
// if it were over the network. Useful for mocking
// network traffic and for creating modules that
// are agnostic to servers hosted locally and
// remotely.
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System.Collections.Generic;

namespace ConnectionManagerCS.Protocols
{
    public class LocalConnectionProtocol : IConnectionProtocol
    {
        public LocalConnectionProtocol()
        {
            Messages = new Queue<byte[]>();
        }

        public void WriteBytes(byte[] messageBytes)
        {
            Messages.Enqueue(messageBytes);
        }

        public byte[] ReadBytes(int messageLength)
        {
            return Messages.Dequeue();
        }

        public bool IsAlive()
        {
            return true;
        }

        public int MaxSupportedSize { get { return (int)Message.MaxPayloadSize + 5; } }

        private Queue<byte[]> Messages { get; set; }
    }
}
