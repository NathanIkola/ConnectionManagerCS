using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Clients = new List<ConnectionManager>();
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
                Clients.Add(manager);
            }
            Server.Stop();
        }

        private void CullClients()
        {
            Clients.RemoveAll(x => x.IsAlive == false);
            Thread.Sleep(1000);
        }

        private TcpListener Server { get; set; }
        public List<ConnectionManager> Clients { get; private set; }
        private bool Listening { get; set; }
    }
}
