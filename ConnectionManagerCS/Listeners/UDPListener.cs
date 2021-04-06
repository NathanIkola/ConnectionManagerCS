//************************************************
// UDPListener.cs
//
// Listens for incoming UDP messages on a specific
// port and communicates it to all of the 
// Connection objects added to its Client list.
//
// It is undefined behavior to have more than one
// UDPListener running if one is utilizing
// IPAddress.Any under its IPEndPoint as it can
// receive messages meant for another UDPListener.
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System;
using System.Collections.Generic;
using ConnectionManagerCS.Protocols;
using System.Threading.Tasks;
using System.Net;

namespace ConnectionManagerCS.Listeners
{
    public class UDPListener : IListener
    {
        private UDPListener()
        {
            Clients = new List<Connection>();
            Listening = false;
        }

        public UDPListener(int port) : this()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            Protocol = new UDPConnectionProtocol(endPoint);
            ConnectionManager man = new ConnectionManager(Protocol);
            Clients.Add(man.GetConnection());
        }

        public UDPListener(IPEndPoint endPoint) : this()
        {
            Protocol = new UDPConnectionProtocol(endPoint);
            ConnectionManager man = new ConnectionManager(Protocol);
            Clients.Add(man.GetConnection());
        }

        public UDPListener(IPAddress address, int port) : this()
        {
            Protocol = new UDPConnectionProtocol(new IPEndPoint(address, port));
            ConnectionManager man = new ConnectionManager(Protocol);
            Clients.Add(man.GetConnection());
        }

        public void Start()
        {
            if(!Listening)
            {
                Listening = true;
                Task.Run(() => { Listen(); });
            }
        }

        public void Stop()
        {
            Listening = false;
        }

        private void Listen()
        {
            while(Listening)
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

                Message msg = Message.ParseNetworkBytes(messageBytes);

                foreach(Connection connection in Clients)
                {
                    connection.PassMessage(msg);
                }
            }
        }

        public UDPConnectionProtocol Protocol { get; private set; }
        public List<Connection> Clients { get; private set; }
        private bool Listening { get; set; }
    }
}
