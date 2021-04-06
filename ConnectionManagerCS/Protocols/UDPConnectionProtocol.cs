//************************************************
// UDPConnectionProtocol.cs
//
// Houses the information required to connect
// via UDP to a machine.
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System;
using System.Net.Sockets;
using System.Net;

namespace ConnectionManagerCS.Protocols
{
    public class UDPConnectionProtocol : IConnectionProtocol
    {
        public UDPConnectionProtocol(IPEndPoint ipEndPoint)
        {
            if (ipEndPoint == null)
                throw new ArgumentNullException("IPEndPoint was null");
            IPEndPoint = ipEndPoint;
            Client = new UdpClient();
            Client.Client.ReceiveBufferSize = (int)Message.MaxPayloadSize + 5;
            Client.Client.SendBufferSize = (int)Message.MaxPayloadSize + 5;
            Client.Client.Bind(IPEndPoint);
        }

        public UDPConnectionProtocol(IPEndPoint ipEndPoint, string hostName, int port) : this(ipEndPoint)
        {
            if (String.IsNullOrWhiteSpace(hostName))
                throw new ArgumentException("Hostname was null or empty");
            if (port < 0 || port > 65535)
                throw new ArgumentOutOfRangeException(String.Format("Port was out of allowed range (0-65535): {0}", port));
            Client.Connect(hostName, port);
        }

        public UDPConnectionProtocol(IPEndPoint ipEndPoint, IPAddress address, int port) : this(ipEndPoint)
        {
            if (address == null)
                throw new ArgumentNullException("IP address was null");
            if (port < 0 || port > 65535)
                throw new ArgumentOutOfRangeException(String.Format("Port was out of allowed range (0-65535): {0}", port));
            Client.Connect(address, port);
        }

        public UDPConnectionProtocol(IPEndPoint ipEndPoint, UdpClient client) : this(ipEndPoint)
        {
            if (client == null)
                throw new ArgumentNullException("Client was null");
            Client = client;
        }

        public byte[] ReadBytes(int messageLength)
        {
            if (messageLength <= 0) return new byte[0];

            byte[] messageBytes = new byte[messageLength];
            if (Fragment == null)
            {
                IPEndPoint ip = IPEndPoint;
                Fragment = Client.Receive(ref ip);
                if (Fragment.Length == 0) return new byte[0];
            }

            if (messageLength > Fragment.Length)
                throw new ArgumentOutOfRangeException("Reading past Message boundary");

            byte[] newFragment = new byte[Fragment.Length - messageLength];
            for (int _byte = 0; _byte < messageBytes.Length; ++_byte)
                messageBytes[_byte] = Fragment[_byte];
            for (int _byte = 0; _byte < Fragment.Length - messageLength; ++_byte)
                newFragment[_byte] = Fragment[_byte + messageLength];

            if (newFragment.Length != 0)
                Fragment = newFragment;
            else
                Fragment = null;

            return messageBytes;
        }

        public void WriteBytes(byte[] messageBytes)
        {
            if (messageBytes.Length > MaxSupportedSize)
                throw new ArgumentOutOfRangeException("Message too large to send reliably on datagram");
            Client.Send(messageBytes, messageBytes.Length);
        }

        public bool IsAlive()
        {
            // UDP is always able to send and receive, even if nobody hears them
            return true;
        }

        public int MaxSupportedSize { get { return 65507; } }
        private UdpClient Client { get; set; }
        private IPEndPoint IPEndPoint { get; set; }
        private byte[] Fragment { get; set; }
    }
}
