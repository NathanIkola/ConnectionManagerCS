//************************************************
// TCPListener.cs
//
// Listens for incoming TCP connections and
// generates a ConnectionManager and subsequent
// Connection for that client, and adds it to the
// list of currently connected clients. 
// Responsible for managing the living connections
// to a server.
//
// Author: Nathan Ikola
// nathan.ikola@gmail.com
//************************************************

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ConnectionManagerCS.Protocols;
using System.Threading;

namespace ConnectionManagerCS.Listeners
{
    public class TCPListener : IListener
    {
        private TCPListener()
        {
            Clients = new List<Connection>();
            Listening = false;
            Task.Run(() => { CullClients(); });
        }

        public TCPListener(int port) : this()
        {
            Server = new TcpListener(IPAddress.Any, port);
        }

        public TCPListener(IPEndPoint endPoint) : this()
        {
            Server = new TcpListener(endPoint);
        }

        public TCPListener(IPAddress address, int port) : this()
        {
            Server = new TcpListener(address, port);
        }

        public void Start()
        {
            if (!Listening)
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
            Server.Start();
            while(Listening)
            {
                TcpClient client = Server.AcceptTcpClient();
                IConnectionProtocol protocol = new TCPConnectionProtocol(client);
                ConnectionManager manager = new ConnectionManager(protocol);
                Connection connection = manager.GetConnection();
                connection.SubscribeToAll();
                Clients.Add(connection);
            }
            Server.Stop();
        }

        private void CullClients()
        {
            Clients.RemoveAll(x => !x.Manager.IsAlive);
            Thread.Sleep(1000);
        }

        private TcpListener Server { get; set; }
        public List<Connection> Clients { get; private set; }
        private bool Listening { get; set; }
    }
}
