using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionManagerCS;
using ConnectionManagerCS.Protocols;
using System.Threading;
using ConnectionManagerCS.Listeners;
using System.Diagnostics;
using System.Net;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            //IConnectionProtocol protocol = new UDPConnectionProtocol(endPoint, "localhost", 3000);
            IConnectionProtocol protocol = new TCPConnectionProtocol("localhost", 3000);
            ConnectionManager manager = new ConnectionManager(protocol);

            // send a message to the server with job code 5, and transaction ID of 2
            byte[] payload = new byte[protocol.MaxSupportedSize-5];

            Message msg = new Message(3, 99, payload);
            manager.WriteMessage(msg);

            // give the server time to read the data before closing the connection
            Thread.Sleep(3000);
        }
    }
}
