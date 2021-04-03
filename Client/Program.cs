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

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // connect to the server via TCP
            IListener listener = new TCPListener(3000);
            listener.Start();
            IConnectionProtocol protocol = new TCPConnectionProtocol("localhost", 3000);
            ConnectionManager manager = new ConnectionManager(protocol);

            // send a message to the server with job code 5, and transaction ID of 2
            byte[] payload = new byte[Message.MaxPayloadSize];
            // start tracking time
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Message msg = new Message(3, 99, payload);
            manager.WriteMessage(msg);

            foreach (Connection client in listener.Clients)
            {
                while (client.PendingMessages == 0) { }
                while (client.PendingMessages > 0)
                {
                    msg = client.ReadMessage();
                    Console.WriteLine(
                        String.Format("Message read with Job Specifier {0}, Transaction ID {1}, payload size {2}",
                                        msg.JobSpecifier, msg.TransactionID, msg.PayloadSize));
                }
            }
            stopwatch.Stop();
            Console.WriteLine(String.Format("{0}ms elapsed", stopwatch.ElapsedMilliseconds));

            // give the server time to read the data before closing the connection
            Thread.Sleep(3000);
        }
    }
}
