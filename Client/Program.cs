using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionManagerCS;
using ConnectionManagerCS.Protocols;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // connect to the server via TCP
            IConnectionProtocol protocol = new TCPConnectionProtocol("localhost", 3000);
            ConnectionManager manager = new ConnectionManager(protocol);

            // send a message to the server with job code 5, and transaction ID of 2
            Message msg = new Message(5, 2, null);
            manager.WriteMessage(msg);

            // send a message to the server with job code 5, and transaction ID of 2
            msg = new Message(4, 2, null);
            manager.WriteMessage(msg);

            // send a message to the server with job code 5, and transaction ID of 2
            msg = new Message(3, 99, null);
            manager.WriteMessage(msg);

            // give the server time to read the data before closing the connection
            Thread.Sleep(3000);
        }
    }
}
