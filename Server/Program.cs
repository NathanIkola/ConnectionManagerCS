using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionManagerCS;
using ConnectionManagerCS.Listeners;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // begin listening for clients
            IListener listener = new TCPListener(3000);
            listener.Start();

            // give the client time to connect
            Thread.Sleep(500);

            // read from all connected clients
            foreach(Connection client in listener.Clients)
            {
                while(client.PendingMessages > 0)
                {
                    Message msg = client.ReadMessage();
                    Console.WriteLine(
                        String.Format("Message read with Job Specifier {0}, Transaction ID {1}",
                                        msg.JobSpecifier, msg.TransactionID));
                }
            }

            Console.WriteLine("Done");
        }
    }
}
