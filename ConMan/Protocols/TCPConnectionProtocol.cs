//************************************************
// TCPConnectionProtocol.cs
//
// Houses the information required to connect
// via TCP to a machine. 
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System.Net;
using System.Net.Sockets;

namespace ConnectionManager.Protocols
{
    public class TCPConnectionProtocol : IConnectionProtocol
    {
        public TCPConnectionProtocol()
        {

        }

        public TCPConnectionProtocol(string hostName, int port)
        {
            m_client.Connect(hostName, port);
        }

        public TCPConnectionProtocol(IPAddress address, int port)
        {
            m_client.Connect(address, port);
        }

        public byte[] ReadMessage()
        {
            throw new System.NotImplementedException();
        }

        public void WriteMessage(byte[] messageBytes)
        {
            throw new System.NotImplementedException();
        }

        private TcpClient m_client = new TcpClient();
    }
}
