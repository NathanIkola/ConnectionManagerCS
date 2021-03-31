//************************************************
// TCPConnectionProtocol.cs
//
// Houses the information required to connect
// via TCP to a machine. 
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using ConnectionManager.Exceptions;

namespace ConnectionManager.Protocols
{
    public class TCPConnectionProtocol : IConnectionProtocol
    {
        public TCPConnectionProtocol() 
        {
            Client = new TcpClient();
        }

        public TCPConnectionProtocol(string hostName, int port) : this()
        {
            Client.Connect(hostName, port);
        }

        public TCPConnectionProtocol(IPAddress address, int port) : this()
        {
            Client.Connect(address, port);
        }

        public byte[] ReadBytes(int messageLength)
        {
            if (!NetworkStream.CanRead) throw new ConnectionNotReadyException("NetworkStream unable to read");
            int networkQueueSize = Client.Available;
            if (networkQueueSize == 0) return null;
            if (messageLength > networkQueueSize) messageLength = networkQueueSize;

            byte[] messageBytes = new byte[messageLength];
            int bytesRead = NetworkStream.Read(messageBytes, 0, messageLength);
            if (bytesRead <= 0) throw new Exception("Failed to read data from the stream");
            return messageBytes;
        }

        public void WriteBytes(byte[] messageBytes)
        {
            throw new NotImplementedException();
        }

        private TcpClient Client { get; set; }
        private NetworkStream NetworkStream { get { return Client.GetStream(); } }
    }
}
