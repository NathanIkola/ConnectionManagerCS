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
            if (String.IsNullOrWhiteSpace(hostName)) 
                throw new ArgumentException("Hostname was null or empty");
            if (port < 0 || port > 65535)
                throw new ArgumentOutOfRangeException(String.Format("Port was out of allowed range (0-65535): {0}", port));
            Client.Connect(hostName, port);
        }

        public TCPConnectionProtocol(IPAddress address, int port) : this()
        {
            if (address == null)
                throw new ArgumentNullException("IP address was null");
            if (port < 0 || port > 65535)
                throw new ArgumentOutOfRangeException(String.Format("Port was out of allowed range (0-65535): {0}", port));
            Client.Connect(address, port);
        }

        public TCPConnectionProtocol(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException("Client was null");
            Client = client;
        }

        public byte[] ReadBytes(int messageLength)
        {
            if (messageLength <= 0) return new byte[0];
            if (!NetworkStream.CanRead) throw new ConnectionNotReadyException("NetworkStream unable to read");

            byte[] messageBytes = new byte[messageLength];
            int bytesRead = 0;
            while (bytesRead != messageLength)
            {
                bytesRead += NetworkStream.Read(messageBytes, bytesRead, messageLength - bytesRead);
            }

            if (bytesRead != messageLength) throw new Exception("Failed to read data from the stream");
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
