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
using ConnectionManagerCS.Exceptions;

namespace ConnectionManagerCS.Protocols
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

        public TCPConnectionProtocol(TcpClient client)
        {
            Client = client;
        }

        public byte[] ReadBytes(int messageLength)
        {
            if (messageLength == 0) return new byte[0];
            if (!NetworkStream.CanRead) throw new ConnectionNotReadyException("NetworkStream unable to read");

            byte[] messageBytes = new byte[messageLength];
            int bytesRead = NetworkStream.Read(messageBytes, 0, messageLength);
            if (bytesRead <= 0) throw new Exception("Failed to read data from the stream");
            return messageBytes;
        }

        public void WriteBytes(byte[] messageBytes)
        {
            if (!NetworkStream.CanWrite) throw new ConnectionNotReadyException("Network stream unable to write");
            NetworkStream.Write(messageBytes, 0, messageBytes.Length);
        }

        public bool IsAlive()
        {
            return Client.Connected;
        }

        private TcpClient Client { get; set; }
        private NetworkStream NetworkStream { get { return Client.GetStream(); } }
    }
}
